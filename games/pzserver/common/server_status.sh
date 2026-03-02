#!/usr/bin/env bash
set -Eeuo pipefail

exec 3>&1 4>/dev/tty 1>&2

json_fail() {
    echo "ERROR: ${BASH_COMMAND}" >&4
    jq -n '{completed:true,failure:"Script has failed"}' >&3
    exit 1
}

trap json_fail ERR

LGSM_USER="lgsm"
LGSM_HOME="/home/${LGSM_USER}"
SERVER_CMD="${LGSM_HOME}/pzserver"

out="$(
runuser -l "$LGSM_USER" -c "
    set -Eeuo pipefail
    cd \"$LGSM_HOME\"
    \"$SERVER_CMD\" details
" 2>&1
)" || json_fail

clean="$(
printf "%s" "$out" \
| tr -d '\r' \
| sed -E 's/\x1B\[[0-9;]*[[:alpha:]]//g'
)"

status_text="$(
printf "%s\n" "$clean" \
| grep -i '^Status:' \
| tail -n 1 \
| tr '[:upper:]' '[:lower:]'
)"

name="$(
printf "%s\n" "$clean" \
| grep -i '^Server name:' \
| head -n 1 \
| sed -E 's/^[^:]+:[[:space:]]*//'
)"

ip_port="$(
printf "%s\n" "$clean" \
| grep -i '^Server IP:' \
| head -n 1 \
| sed -E 's/^[^:]+:[[:space:]]*//'
)"

ip="${ip_port%:*}"
port="${ip_port##*:}"

case "$status_text" in
    *started*|*online*|*running*)
        status=1
        ;;
    *stopped*|*offline*)
        status=0
        ;;
    *)
        status=0
        ;;
esac

jq -n \
  --arg name "$name" \
  --arg ip "$ip" \
  --arg port "$port" \
  --arg status "$status" \
  '{completed:true,data:{status:($status|tonumber),name:$name,ip:$ip,port:$port,pid:null}}' >&3
