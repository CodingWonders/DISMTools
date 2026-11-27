@ECHO OFF
CD /D "%~DP0"

SET ARGCOUNT=0
FOR %%X IN (%*) DO SET /A ARGCOUNT+=1

SET "BOOTWIM="
IF %ARGCOUNT% EQU 1 SET "BOOTWIM=%1"

REM check admin privs
NET SESSION >NUL 2>&1
IF %ERRORLEVEL% GTR 0 (
    ECHO This script must be run as an administrator.
    PAUSE > NUL
    EXIT /B 1
)

ECHO Unpacking and setting up the System Deployment Services Server. Please wait...

SET "DESTDIR=%SYSTEMDRIVE%\SDSSERVER"
SET "SERVERDIR=%DESTDIR%\SERVER"
SET "WEBDIR=%DESTDIR%\WEBCLIENT"

IF NOT EXIST %DESTDIR% MD "%DESTDIR%"
IF NOT EXIST %SERVERDIR% MD "%SERVERDIR%"
IF NOT EXIST %WEBDIR% MD "%WEBDIR%"

powershell -command Expand-Archive -Path '%CD%\SdsServer-1.0.0-win-x64.zip' -Destination "%SERVERDIR%" -Force
powershell -command Expand-Archive -Path '%CD%\SdsWeb-1.0.0-win-x64.zip' -Destination "%WEBDIR%" -Force

START "SDS" /MIN /D "%SERVERDIR%" "%SERVERDIR%\SdsServer.exe"
START "SDS" /MIN /D "%WEBDIR%" "%WEBDIR%\SdsWeb.exe"
PING /N 6 127.0.0.1 >NUL 2>&1

IF NOT EXIST "%SYSTEMDRIVE%\SDSREMINST\PXECORE" MD "%SYSTEMDRIVE%\SDSREMINST\PXECORE"
XCOPY "%SERVERDIR%\SharpPXE\*.*" "%SYSTEMDRIVE%\SDSREMINST\PXECORE" /CEHYI
RD "%SERVERDIR%\SharpPXE" /s /q

REM begin copying the ipxe valueadd

IF EXIST "%CD%\..\ipxe" XCOPY "%CD%\..\ipxe\*.*" "%SYSTEMDRIVE%\SDSREMINST\PXECORE\files" /cehyi
IF EXIST "%CD%\..\pxeconfig\SharpPXE.ini" COPY /Y "%CD%\..\pxeconfig\SharpPXE.ini" "%SYSTEMDRIVE%\SDSREMINST\PXECORE\SharpPXE.ini"

REM Pick the address of the interface PXE clients can reach. Looking only at the route
REM metric is not enough: VPN clients add their own default route (often with a lower
REM metric) on a virtual adapter. Candidates are the up adapters with a default route
REM and a non-APIPA IPv4 address, ranked by:
REM   1. physical adapters first (VPN, TAP/Wintun and tunnel adapters are not hardware)
REM   2. routes with a real gateway first (VPN default routes are usually on-link)
REM   3. lowest effective metric (route metric + interface metric, as Windows does)
SET "PSIP=$c = foreach ($r in Get-NetRoute -AddressFamily IPv4 -DestinationPrefix '0.0.0.0/0' -ErrorAction SilentlyContinue) {"
SET "PSIP=%PSIP% $a = Get-NetAdapter -InterfaceIndex $r.ifIndex -ErrorAction SilentlyContinue;"
SET "PSIP=%PSIP% if (-not $a -or $a.Status -ne 'Up') { continue };"
SET "PSIP=%PSIP% $ip = Get-NetIPAddress -InterfaceIndex $r.ifIndex -AddressFamily IPv4 -ErrorAction SilentlyContinue | Where-Object { $_.IPAddress -notlike '169.254.*' } | Select-Object -First 1;"
SET "PSIP=%PSIP% if (-not $ip) { continue };"
SET "PSIP=%PSIP% $if = Get-NetIPInterface -InterfaceIndex $r.ifIndex -AddressFamily IPv4 -ErrorAction SilentlyContinue;"
SET "PSIP=%PSIP% [pscustomobject]@{ IP = $ip.IPAddress; Virt = [int](-not $a.HardwareInterface); OnLink = [int]($r.NextHop -eq '0.0.0.0'); Metric = $r.RouteMetric + $if.InterfaceMetric } };"
SET "PSIP=%PSIP% ($c | Sort-Object Virt, OnLink, Metric | Select-Object -First 1).IP"

SET "IPADDR="
FOR /F "delims=" %%a IN ('powershell -NoProfile -Command "%PSIP%"') DO SET "IPADDR=%%a"

IF NOT DEFINED IPADDR (
    ECHO We could not find a network adapter. iPXE will be configured to point to the loopback.
	SET "IPADDR=127.0.0.1"
)

REM determine if we are in scoped ipv6
FOR /F "delims=%%" %%a IN ("%IPADDR%") DO SET "IPADDR=%%a"

REM if we are in ipv6 we have to add brackets
SET "FINALHOST=%IPADDR%"
IF NOT "%IPADDR::=%"=="%IPADDR%" SET "FINALHOST=[%IPADDR%]"

REM replace the address placeholder in the ipxe file
SET "PXEFILES=%SYSTEMDRIVE%\SDSREMINST\PXECORE\files"
SET "DESTIPXE=%PXEFILES%\boot.ipxe"
SET "TMPLIPXE=%PXEFILES%\boot.ipxe.tmpl"

REM Refresh the template only when boot.ipxe still contains the placeholder,
REM so a re-run never overwrites the template with an already-processed file
IF EXIST "%DESTIPXE%" FINDSTR /L /C:"#DT_SDS_IPADDR#" "%DESTIPXE%" >NUL && COPY /Y "%DESTIPXE%" "%TMPLIPXE%" >NUL

IF NOT EXIST "%TMPLIPXE%" (
    ECHO ERROR: No iPXE template found at "%TMPLIPXE%".
    EXIT /B 1
)

REM Command echoing must be off here, or with ECHO ON every echoed command of this
REM block would be redirected into boot.ipxe together with the real content
@ECHO OFF
(
    FOR /F "delims=" %%a IN ('findstr /N "^" "%TMPLIPXE%"') DO (
        SET "LINE=%%a"
        SETLOCAL EnableDelayedExpansion
        SET "LINE=!LINE:*:=!"
        IF DEFINED LINE (
            SET "LINE=!LINE:#DT_SDS_IPADDR#=%FINALHOST%!"
            ECHO(!LINE!
        ) ELSE (
            ECHO(
        )
        ENDLOCAL
    )
) > "%DESTIPXE%"

FINDSTR /L /C:"#DT_SDS_IPADDR#" "%DESTIPXE%" >NUL && (
    ECHO WARNING: The address placeholder is still present in "%DESTIPXE%".
) || (
    DEL "%TMPLIPXE%" /F /Q
)

SET "BOOTWIMDIR="
SET "BOOTWIMFILE="

FOR /F "delims=" %%A IN ('ECHO %BOOTWIM%') DO (
    FOR %%B IN ("%%~DPA.") DO SET "BOOTWIMDIR=%%~FB"
    SET "BOOTWIMFILE=%%~NXA"
)

IF EXIST "%BOOTWIM%" (
    MD "%SYSTEMDRIVE%\SDSREMINST\PXECORE\files\winpe"
    ROBOCOPY "%BOOTWIMDIR%" "%SYSTEMDRIVE%\SDSREMINST\PXECORE\files\winpe" "%BOOTWIMFILE%" /J
    MOVE /Y "%SYSTEMDRIVE%\SDSREMINST\PXECORE\files\winpe\%BOOTWIMFILE%" "%SYSTEMDRIVE%\SDSREMINST\PXECORE\files\winpe\boot.wim"
    CALL :PREPARE_WINPE
)

START "SHARPPXE" /MIN /D "%SYSTEMDRIVE%\SDSREMINST\PXECORE" "%SYSTEMDRIVE%\SDSREMINST\PXECORE\SharpPXE.exe"
ECHO SDS > "%SYSTEMDRIVE%\SDSREMINST\__SDS__"

ENDLOCAL
EXIT /B

:PREPARE_WINPE
MD "%SYSTEMDRIVE%\TMPMOUNT"
ECHO Preparing Windows PE environments...
DISM /MOUNT-IMAGE /IMAGEFILE="%SYSTEMDRIVE%\SDSREMINST\PXECORE\files\winpe\boot.wim" /INDEX=1 /MOUNTDIR="%SYSTEMDRIVE%\TMPMOUNT" /READONLY
ECHO Preparing boot files...
COPY /Y "%SYSTEMDRIVE%\TMPMOUNT\Windows\Boot\DVD\EFI\BCD" "%SYSTEMDRIVE%\SDSREMINST\PXECORE\files\winpe"
COPY /Y "%SYSTEMDRIVE%\TMPMOUNT\Windows\Boot\DVD\EFI\boot.sdi" "%SYSTEMDRIVE%\SDSREMINST\PXECORE\files\winpe"
DISM /UNMOUNT-IMAGE /MOUNTDIR="%SYSTEMDRIVE%\TMPMOUNT" /DISCARD
RD "%SYSTEMDRIVE%\TMPMOUNT" /S /Q
EXIT /B