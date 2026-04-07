# For a Given Config File Workshopitems=STEAMID;STEAMID; Mods=MODID;MODID
# For Workshopitems are all of the id in order listed -> from Part_1
# For Mods are all of the id in order listed -> from Part_2
# The Mission of the script is to extract the current modlist using the gamemanagepanel bash tool function.
# which return the associative array of Part -> Mods (Id)
# once you get the mods dictionary you will start processing and building the file output
# you will search inside /home/lgsm/lgsm/config-lgsm/pzserver/common.cfg "Workshopitems=" and "Mods="
# if those line exist remove it and write new ones with the joined array of part_1 into Workshopitems as Workshopitems=ID;ID;ID;ID; and same for Mods=ID;ID;ID; from Part_2 in dictionary
# this is the command to extract the modlist bash -c 'source /usr/lib/lunaticpanel/plugins/maksimshimshon_gamemanagepanel/bash/modlist/modlist_tool.sh; load_modlist "935e9c3a-c61b-47c2-91ad-017ccaf8bcca"; declare -p MODLIST_DICT'
# the modlist id is located inside /home/lgsm/lunaticpanel/plugins/maksimshimshon_gamemanagepanel/config/modlist/modlist_current.json if file does not exist then there is nothing to apply just jump to remove Workshopitems=ID;ID;ID;ID; and same for Mods=ID;ID;ID; from /home/lgsm/lgsm/config-lgsm/pzserver/common.cfg if any defined


#!/bin/bash

CFG_FILE="/home/lgsm/lgsm/config-lgsm/pzserver/common.cfg"
MODLIST_JSON="/home/lgsm/lunaticpanel/plugins/maksimshimshon_gamemanagepanel/config/modlist/modlist_current.json"
MODLIST_TOOL="/usr/lib/lunaticpanel/plugins/maksimshimshon_gamemanagepanel/bash/modlist/modlist_tool.sh"

remove_existing_lines() {
    sed -i '/^Workshopitems=/d' "$CFG_FILE"
    sed -i '/^Mods=/d' "$CFG_FILE"
}

write_new_lines() {
    {
        echo "$1"
        echo "$2"
    } >> "$CFG_FILE"
}

# If no modlist file → remove lines and exit
if [[ ! -f "$MODLIST_JSON" ]]; then
    remove_existing_lines
    exit 0
fi

# File contains ONLY the GUID
MODLIST_ID=$(tr -d ' \t\r\n' < "$MODLIST_JSON")

if [[ -z "$MODLIST_ID" ]]; then
    remove_existing_lines
    exit 0
fi

# Load the modlist tool directly
source "$MODLIST_TOOL"

# Call the function normally (populates MODLIST_DICT)
load_modlist "$MODLIST_ID"

# Ensure dictionary exists
if [[ -z "${MODLIST_DICT[Part_1]+x}" && -z "${MODLIST_DICT[Part_2]+x}" ]]; then
    remove_existing_lines
    exit 0
fi

# Extract raw values
PART_1_RAW="${MODLIST_DICT[Part_1]}"
PART_2_RAW="${MODLIST_DICT[Part_2]}"

# Normalize separators to semicolons and ensure trailing ;
normalize_ids() {
    local s="$1"
    s="${s//,/;}"              # commas → semicolons
    s="${s//[[:space:]]/}"     # remove whitespace
    [[ -n "$s" && "$s" != *";" ]] && s="${s};"
    echo "$s"
}

WORKSHOP_JOINED=$(normalize_ids "$PART_1_RAW")
MODS_JOINED=$(normalize_ids "$PART_2_RAW")

# Rewrite config
remove_existing_lines
write_new_lines \
    "Workshopitems=${WORKSHOP_JOINED}" \
    "Mods=${MODS_JOINED}"