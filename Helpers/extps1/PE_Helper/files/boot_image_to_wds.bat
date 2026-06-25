@ECHO OFF
SET "ImageName=DISMTools Preinstallation Environment"
SET "ImageDescription=Microsoft Windows Preinstallation Environment"

TITLE Upload boot image to WDS server

REM query service information to detect if we have WDSServer
sc queryex WDSServer >nul 2>&1
IF %ERRORLEVEL% EQU 1060 (
	ECHO The WDS Service is not installed on this machine. This is not a WDS server.
	PAUSE > nul
	EXIT /B 1
)

REM Calculate the number of arguments that were passed to the script.
SET ARGUMENTCOUNT=0
FOR %%X IN (%*) DO SET /A ARGUMENTCOUNT+=1

IF %ARGUMENTCOUNT% EQU 2 (
	IF /I "%1" == "/arch=x86" SET Architecture=x86
	IF /I "%1" == "/arch=amd64" SET Architecture=x64
	IF /I "%1" == "/arch=arm64" SET Architecture=arm64
	IF NOT DEFINED Architecture SET Architecture=x64
) ELSE (
	SET Architecture=x64
	ECHO Choose boot image architecture, wait 10 seconds for default value (X64^):
	CHOICE /C 123 /T 10 /D 2 /M "1: X86, 2: X64, 3: ARM64 -- " /N
	IF %ERRORLEVEL% EQU 1 SET Architecture=x86
	IF %ERRORLEVEL% EQU 2 SET Architecture=x64
	IF %ERRORLEVEL% EQU 3 SET Architecture=arm64
)

REM to determine the status of the service we'll check if PID > 4 (ntoskrnl) -- if not, then it is stopped
SET PID=0
FOR /F "tokens=3" %%a in ('sc queryex WDSServer ^| findstr PID') DO SET PID=%%a
IF %PID% LEQ 4 (
	echo Starting WDS Server service...
	net start WDSServer >nul 2>&1
)

CD %~dp0
wdsutil /get-image /image:"%ImageName%" /imagetype:boot /architecture:%Architecture% /filename:boot_dtpe_iso.wim >nul 2>&1

IF %ERRORLEVEL% EQU 0 (
	ECHO Replacing boot image...
	wdsutil /verbose /progress /replace-image /image:"%ImageName%" /server:%COMPUTERNAME% /imagetype:boot /architecture:%Architecture% /filename:boot_dtpe_iso.wim /replacementimage /imagefile:.\sources\boot.wim /name:"%ImageName%"
) ELSE (
	ECHO Adding boot image...
	wdsutil /verbose /progress /add-image /imagefile:.\sources\boot.wim /server:%COMPUTERNAME% /imagetype:Boot /name:"%ImageName%" /description:"%ImageDescription%" /filename:boot_dtpe_iso.wim
)