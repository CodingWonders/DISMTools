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

powershell -command Expand-Archive -Path "%CD%\SdsServer-1.0.0-win-x64.zip" -Destination "%SERVERDIR%" -Force
powershell -command Expand-Archive -Path "%CD%\SdsWeb-1.0.0-win-x64.zip" -Destination "%WEBDIR%" -Force

START "SDS" /MIN /D "%SERVERDIR%" "%SERVERDIR%\SdsServer.exe"
START "SDS" /MIN /D "%WEBDIR%" "%WEBDIR%\SdsWeb.exe"
PING /N 6 127.0.0.1 >NUL 2>&1

IF NOT EXIST "%SYSTEMDRIVE%\SDSREMINST\PXECORE" MD "%SYSTEMDRIVE%\SDSREMINST\PXECORE"
XCOPY "%SERVERDIR%\SharpPXE\*.*" "%SYSTEMDRIVE%\SDSREMINST\PXECORE" /CEHYI
RD "%SERVERDIR%\SharpPXE" /s /q

REM begin copying the ipxe valueadd

IF EXIST "%CD%\..\ipxe" XCOPY "%CD%\..\ipxe\*.*" "%SYSTEMDRIVE%\SDSREMINST\PXECORE\files" /cehyi

SET "IPADDR="
FOR /F "delims=" %%a IN ('powershell -NoProfile -Command "$r = Get-CimInstance Win32_IP4RouteTable -Filter 'Destination=''0.0.0.0''' | Sort-Object Metric1 | Select-Object -First 1; (Get-CimInstance Win32_NetworkAdapterConfiguration -Filter ('InterfaceIndex=' + $r.InterfaceIndex)).IPAddress[0]"') do SET "IPADDR=%%a"

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