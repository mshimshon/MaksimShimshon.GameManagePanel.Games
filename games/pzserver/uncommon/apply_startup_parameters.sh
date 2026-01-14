#!/bin/bash

# Script to merge startup parameters and update common.cfg
# Usage: ./merge_startup_parameters.sh

set -e

GAME_INFO_FILE="/home/lgsm/blazor_lgsm/scripts/game_info.json"
USER_PARAMS_FILE="/home/lgsm/blazor_lgsm/user_startup_parameters.json"
CONFIG_FILE="/home/lgsm/lgsm/config-lgsm/pzserver/common.cfg"


if [ ! -f "$GAME_INFO_FILE" ]; then
    echo "Error: Game info file not found: $GAME_INFO_FILE"
    exit 1
fi

if [ ! -f "$CONFIG_FILE" ]; then
    echo "Error: Config file not found: $CONFIG_FILE"
    exit 1
fi

declare -A params
declare -A param_types
declare -A param_separators
declare -A param_wrappers

# Load default parameters from game_info.json
while IFS= read -r line; do
    key=$(echo "$line" | jq -r '.key')
    value=$(echo "$line" | jq -r '.defaultValue')
    type=$(echo "$line" | jq -r '.type')
    has_separator=$(echo "$line" | jq 'has("separator")')
    separator=$(echo "$line" | jq -r '.separator // ""')
    has_wrapper=$(echo "$line" | jq 'has("wrapper")')
    wrapper=$(echo "$line" | jq -r '.wrapper // ""')
    
    if [ "$value" != "null" ] && [ -n "$value" ]; then
        params["$key"]="$value"
        param_types["$key"]="$type"
        
        if [ "$has_separator" = "true" ]; then
            param_separators["$key"]="$separator"
        fi
        
        if [ "$has_wrapper" = "true" ]; then
            param_wrappers["$key"]="$wrapper"
        fi
    fi
done < <(jq -c '.parameters[] | select(.defaultValue != null)' "$GAME_INFO_FILE")

# Load user-defined parameters
if [ -f "$USER_PARAMS_FILE" ]; then
    while IFS= read -r line; do
        key=$(echo "$line" | jq -r '.key')
        value=$(echo "$line" | jq -r '.value')
        params["$key"]="$value"
    done < <(jq -c '.[]' "$USER_PARAMS_FILE")
fi

# Build the startparameters string
param_string=""
for key in "${!params[@]}"; do
    value="${params[$key]}"
    type="${param_types[$key]}"
    
    value_lower=$(echo "$value" | tr '[:upper:]' '[:lower:]')
    
    if [ "$value_lower" = "true" ]; then
        param_string+="$key "
        continue
    elif [ "$value_lower" = "false" ]; then
        continue
    fi
    
    # Determine separator
    if [[ -v param_separators[$key] ]]; then
        sep="${param_separators[$key]}"
    else
        sep=" "
    fi
    
    # Strip quotes from JSON string values
    clean_value="${value#\"}"
    clean_value="${clean_value%\"}"
    
    # Format based on wrapper or type
    if [[ -v param_wrappers[$key] ]]; then
        wrapper="${param_wrappers[$key]}"
        escaped_value="${clean_value//\"/\\\"}"
        escaped_wrapper="${wrapper//\"/\\\"}"
        param_string+="$key${sep}${escaped_wrapper}$escaped_value${escaped_wrapper} "
    elif [ "$type" = "string" ]; then
        escaped_value="${clean_value//\"/\\\"}"
        param_string+="$key${sep}\\\"$escaped_value\\\" "
    else
        param_string+="$key${sep}$clean_value "
    fi
done

param_string="${param_string% }"
new_line="startparameters=\"$param_string\""

sed -i '/^startparameters=/d' "$CONFIG_FILE"
echo "$new_line" >> "$CONFIG_FILE"

echo "Success: Updated startparameters in $CONFIG_FILE"
echo "Parameters: $param_string"
exit 0