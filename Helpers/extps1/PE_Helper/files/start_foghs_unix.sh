#!/bin/bash

FOGHELPER_SCRIPTPATH="$PWD/pxehelpers/fog/foghelper_server_unix.ps1"
FOGHELPER_PORT="8080"

clear

whereis pwsh >/dev/null 2>&1

if [[ ! $? ]]; then
    echo "PowerShell is not installed. Refer to Microsoft documentation in order to install PowerShell for UNIX."
    echo "If you see this message even when PowerShell is installed on your system, please report this issue."
    echo ""
    echo "Showing usage instructions and exiting..."
    if [[ -f "./pxehelpers/fog/foghs_unix_notes.txt" ]]; then
        more ./pxehelpers/fog/foghs_unix_notes.txt
    fi
    exit 1
fi

if [[ ! -f "$FOGHELPER_SCRIPTPATH" ]]; then
    echo "The FOG Helper server component for Unix systems is not available in either the current path or the provided disc."
    echo "Showing usage instructions and exiting..."
    if [[ -f "./pxehelpers/fog/foghs_unix_notes.txt" ]]; then
        more ./pxehelpers/fog/foghs_unix_notes.txt
    fi
    exit 2
fi

# get amount of systemd units for both fog and mariadb
FOG_SYSTEMD_UNITS=$(ls -lA /usr/lib/systemd/system/FOG*.service 2>/dev/null | wc -l)

if [[ $FOG_SYSTEMD_UNITS -le 0 ]]; then
    echo "No FOG services have been detected on this system."
    echo "Showing usage instructions and exiting..."
    if [[ -f "./pxehelpers/fog/foghs_unix_notes.txt" ]]; then
        more ./pxehelpers/fog/foghs_unix_notes.txt
    fi
    exit 3
fi

if [[ ! -f "/usr/lib/systemd/system/mariadb.service" ]]; then
    echo "MariaDB service has not been detected on this system."
    echo "Showing usage instructions and exiting..."
    if [[ -f "./pxehelpers/fog/foghs_unix_notes.txt" ]]; then
        more ./pxehelpers/fog/foghs_unix_notes.txt
    fi
    exit 3
fi

echo "You may be asked for your password as starting up the FOG Helper Server requires setting up firewall rules for"
echo "the API listeners and the web server."

sudo iptables -A INPUT -p tcp --dport $FOGHELPER_PORT -j ACCEPT
export NOFWRULESETUP=1
pwsh -file "$FOGHELPER_SCRIPTPATH"

echo "Firewall rules set up by this script will now be removed. You may need to enter your superuser password for this"
echo "task to succeed."

sudo iptables -D INPUT -p tcp --dport $FOGHELPER_PORT -j ACCEPT
unset NOFWRULESETUP
