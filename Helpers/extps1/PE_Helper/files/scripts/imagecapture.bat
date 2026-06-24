@echo off
set sysdrive=%SYSTEMDRIVE%
set _DEBUG=0
setlocal enabledelayedexpansion

:main
cls
echo Image Capture Utility
echo =========================
echo This utility helps you capture an entire Windows installation to a WIM file. This file can be mounted or applied later.
echo.
echo Note that, if you plan on using the Windows installation to deploy it to a network, this utility may produce a result incompatible with this process.
echo.
echo You must have 2 volumes: the volume you want to capture, and the volume on which you want to store the WIM file. You should know the letters assigned to the volumes.
echo.
echo Showing drive letter assignments...
set scriptpath=%TEMP%\%RANDOM%.txt
set configlistpath=%TEMP%\configlist.ini
set wdscapturepath=%SYSTEMROOT%\system32\wdscapture.inf

IF %_DEBUG% EQU 1 echo Path for diskpart script file : %scriptpath%
IF %_DEBUG% EQU 1 echo Configuration list file (DISM): %configlistpath%
IF %_DEBUG% EQU 1 echo Configuration list file (WDS) : %wdscapturepath%

echo lis vol > %scriptpath%
echo exi >> %scriptpath%

diskpart /s %scriptpath%

echo.
echo - To install drivers if you don't see your drives, type "DIM"
if exist "%SYSTEMROOT%\system32\wdscapture.exe" ( echo - To prepare a capture for a Windows Deployment Services server, type "WDS" )
echo - To save the image to a network share, type "NET"
echo - To perform quick disk and partition administration, type "DP"
echo - To change the keyboard layout to use, type "KBD"
echo.

set /p sourcedrive=Please enter the letter of the volume to capture, or option to invoke: 
if not defined sourcedrive (
	echo The letter of the volume to capture must be specified.
	exit /b 1
)

if /i "%sourcedrive%" equ "DIM" (
	call :dt_dim_driver_install
	goto :main
)

if /i "%sourcedrive%" equ "WDS" (
	if not exist "%SYSTEMROOT%\system32\wdscapture.exe" ( goto :main )
	call :create_wdscapture_config_list
	"%SYSTEMROOT%\system32\wdscapture.exe"
	if %ERRORLEVEL% equ 0 (
		echo WDS capture succeeded.
		call :sysprep_hotinstall_remove_temp_files
	)
	exit /b
)

if /i "%sourcedrive%" equ "NET" (
	cls
	echo This process will help you map a network drive to which you can save your Windows image. Keep
	echo in mind, however, that this will NOT produce an installation image compatible with network-based
	echo installation solutions ^(WDS^); it will just create an image suitable for local installations and
	echo save it in the network share that you specify here. Press the Enter key NOW if you want to go back.
	echo.

	set /p "destip=Please enter the UNC path (e.g. \\192.168.1.10\Share): "
	if not defined destip (goto :main)
	set /p "destuser=Please enter the username: "
	set /p "destpassword=Please enter the password: "

	echo Connecting to network share...
	IF %_DEBUG% EQU 1 echo Running net use...
	REM for results to appear in HKCU\Network, we need to make the share persistent
	net use * "%destip%" %destpassword% /USER:%destuser% /P:Yes
	
	IF %_DEBUG% EQU 1 echo net use exitcode: !errorlevel!

	if !errorlevel! neq 0 (
		echo Could not map network drive. This can happen if the computer can't contact the destination.
		echo Press ENTER to go back, and try again.
		pause > nul
		goto :main
	)
	
	REM because we use NET USE * it assigns an available letter to the share; it may not always
	REM be Z:, so we'll check
	for /f %%a in ('reg query HKCU\Network') do (
		for /f "tokens=3" %%b in ('reg query "HKCU\Network\%%~nxa" /v RemotePath') do (
			IF %_DEBUG% EQU 1 echo Drive letter mapping to evaluate: %%~nxa
			if "%%b" EQU "%destip%" (set destdrive=%%~nxa)
		)
	)
	
	echo Share is mapped to !destdrive!:
	echo Now, you will need to specify the source drive to capture.
	
	ping /n 3 127.0.0.1 >nul 2>&1
	
	REM we have to ask for the source drive again
	goto :main
)

if /i "%sourcedrive%" equ "DP" (
	echo Entering DiskPart...
	diskpart
	goto :main
)

if /i "%sourcedrive%" equ "KBD" (
	powershell -noprofile -file "%sysdrive%\ChangeKeyboardLayout.ps1"
	goto :main
)

if %_DEBUG% EQU 1 echo Checking presence of marker...
if %_DEBUG% EQU 1 echo "%sourcedrive%:\Windows\system32\sysprep\Sysprep_succeeded.tag"
if not exist "%sourcedrive%:\Windows\system32\sysprep\Sysprep_succeeded.tag" (
	echo The installation in the drive that you selected has not been prepared by Sysprep. It is recommended that you
	echo prepare the installation on a reference computer before running this script.
	echo.
	set /p question=Continue? ^(y/N^): || goto :main
	if /i "!question!" == "n" goto :main
)

if defined destdrive if %_DEBUG% equ 1 echo Destination drive already set by networking code.
if not defined destdrive ( set /p destdrive=Please enter the letter of the volume the file will be stored on: )
if not defined destdrive (
	echo The letter of the volume where the image will be stored must be specified.
	exit /b 1
)

echo.
set /p destfile=Enter a file name for the target WIM file. Press ENTER without specifying anything to continue with a random name: 
if not defined destfile (
	IF %_DEBUG% EQU 1 echo Destination file path not provided. Continuing with random name.
	set destfile=install_%RANDOM%.wim
)

REM verify if we typed the correct extension -- if not, add it
for %%a in (%destfile%) do (
	if /i not "%%~xa" == ".WIM" set destfile=!destfile!.wim
)

set /p imagename=Provide a custom name (without quotes) for the resulting Windows image (e.g., "My Amazing Windows installation"): 
if not defined imagename (
	IF %_DEBUG% EQU 1 echo Destination name not provided. Continuing with default name.
	set imagename=Windows
)

echo Capturing Windows installation to the target WIM file. This can take a long time, depending on the computer's speed.
call :create_config_list %sourcedrive%

set dismstart=%date% %time%
IF %_DEBUG% EQU 1 echo DISM start time: %dismstart%
IF %_DEBUG% EQU 1 echo Launching DISM...
IF %_DEBUG% EQU 1 echo   Destination file : %destdrive%:\%destfile%
IF %_DEBUG% EQU 1 echo   Source directory : %sourcedrive%:\
IF %_DEBUG% EQU 1 echo   Scratch directory: %destdrive%:\
IF %_DEBUG% EQU 1 echo   Image Name       : %imagename%
dism /capture-image /imagefile="%destdrive%:\%destfile%" /capturedir=%sourcedrive%:\ /scratchdir=%destdrive%:\ /name="%imagename%" /configfile="%configlistpath%" /compress=max /checkintegrity /bootable /verify
if %ERRORLEVEL% equ 0 (
	set succeeded=true
	if exist "%SYSTEMDRIVE%\SysprepPrepTool" call :sysprep_hotinstall_remove_temp_files
) else (
	set succeeded=false
)
set dismend=%date% %time%
IF %_DEBUG% EQU 1 echo DISM end time: %dismend%
echo.
echo Capture Run RESULTS:
echo ======================================================
if "%succeeded%" equ "true" (
	echo   STATUS         : The run succeeded
) else (
	echo   STATUS         : The run failed
)
echo   DISM Start Time: %dismstart%
echo   DISM End Time  : %dismend%
echo.
if "%succeeded%" equ "true" (
	echo   The file has been saved to "%destdrive%:\%destfile%".
)
echo ======================================================
exit /b

:sysprep_hotinstall_remove_temp_files
echo The capture script was invoked by the Sysprep preparation tool. Removing files...
IF %_DEBUG% EQU 1 echo Removing current BCD entry...
bcdedit /delete {current} /f
IF %_DEBUG% EQU 1 echo Removing DT.BT...
if exist "%sourcedrive%:\$DISMTOOLS.~BT" rd "%sourcedrive%:\$DISMTOOLS.~BT" /s /q >nul 2>&1
IF %_DEBUG% EQU 1 echo Removing DT.WS...
if exist "%sourcedrive%:\$DISMTOOLS.~WS" rd "%sourcedrive%:\$DISMTOOLS.~WS" /s /q >nul 2>&1
IF %_DEBUG% EQU 1 echo Removing SYSPRP temp files...
if exist "%sourcedrive%:\CWS_SYSPRP" rd "%sourcedrive%:\CWS_SYSPRP" /s /q >nul 2>&1
IF %_DEBUG% EQU 1 echo Removing sysprep preptool completed marker...
if exist "%sourcedrive%:\capture_completed" del "%sourcedrive%:\capture_completed" /f /q >nul 2>&1
IF %_DEBUG% EQU 1 echo Removal complete.
exit /b

:dt_dim_driver_install
set _ShowPnputilOut=1
for /f "tokens=3" %%a in ('reg query "HKLM\SOFTWARE\DISMTools\Preinstallation Environment\Policies" /v DTDimShowPnputilOut 2^>nul') do (
	if /i "%%a" == "0x1" set _ShowPnputilOut=1
	if /i "%%a" == "0x0" set _ShowPnputilOut=0
)

if !_ShowPnputilOut! equ 1 (
	REM display hardware IDs that require drivers...
	echo These are the device IDs of the hardware devices that could not be detected. Please > %sysdrive%\unknowndevs.txt
	echo install device drivers based on hardware IDs. After installation, please close this window. >> %sysdrive%\unknowndevs.txt
	echo. >> %sysdrive%\unknowndevs.txt
	echo To find the drivers for this specific device, please check the following information: >> %sysdrive%\unknowndevs.txt
	set manufacturer=""
	set model=""
	set boardModel=""
	for /f "usebackq tokens=1,2,3 delims=|" %%A in (`powershell -noprofile -command "$compSys = Get-CimInstance -Query 'SELECT Manufacturer, Model FROM Win32_ComputerSystem'; $baseBrd = Get-CimInstance -Query 'SELECT Product FROM Win32_BaseBoard'; Write-Output ($compSys.Manufacturer + '|' + $compSys.Model + '|' + $baseBrd.Product)"`) do (
		set "manufacturer=%%A"
		set "model=%%B"
		set "boardModel=%%C"
	)
	echo - Manufacturer/Model: !manufacturer! !model! >> %sysdrive%\unknowndevs.txt
	echo - Motherboard model : !boardModel! >> %sysdrive%\unknowndevs.txt
	echo. >> %sysdrive%\unknowndevs.txt
	pnputil /enum-devices /problem >> %sysdrive%\unknowndevs.txt
	start "" notepad %sysdrive%\unknowndevs.txt
)
echo Starting the Driver Installation Module for architecture %PROCESSOR_ARCHITECTURE%...
if "%PROCESSOR_ARCHITECTURE%" equ "X86" (
	"%sysdrive%\Tools\DIM\i386\DT-DIM.exe"
) else if "%PROCESSOR_ARCHITECTURE%" equ "AMD64" (
	"%sysdrive%\Tools\DIM\amd64\DT-DIM.exe"
) else if "%PROCESSOR_ARCHITECTURE%" equ "ARM64" (
	"%sysdrive%\Tools\DIM\aarch64\DT-DIM.exe"
)
exit /b

:create_config_list
echo Setting up file/folder exclusions for source volume...
REM create the config list file. It will call echo lots of times
echo. > %configlistpath%
echo [ExclusionList] >> %configlistpath%
echo \$ntfs.log >> %configlistpath%
echo \hiberfil.sys >> %configlistpath%
echo \pagefile.sys >> %configlistpath%
echo \swapfile.sys >> %configlistpath%
echo \System Volume Information >> %configlistpath%
echo \RECYCLER >> %configlistpath%
echo \Windows\CSC >> %configlistpath%
for /d %%f in (%~1:\Users\*) do (
	if exist "%%f\OneDrive" ( echo %%f\OneDrive >> %configlistpath% )
	if exist "%%f\SkyDrive" ( echo %%f\SkyDrive >> %configlistpath% )
)
if exist "%SYSTEMDRIVE%\SysprepPrepTool" (
	echo \$DISMTOOLS.~BT >> %configlistpath%
	echo \$DISMTOOLS.~WS >> %configlistpath%
	echo \CWS_SYSPRP >> %configlistpath%
	echo \capture_completed >> %configlistpath%
)
echo. >> %configlistpath%
echo [CompressionExclusionList] >> %configlistpath%
echo *.mp3 >> %configlistpath%
echo *.zip >> %configlistpath%
echo *.cab >> %configlistpath%
echo \WINDOWS\inf\*.pnf >> %configlistpath%
IF %_DEBUG% EQU 1 echo ConfigListFile created.
exit /b

:create_wdscapture_config_list
echo Preparing wdscapture.inf...
REM we can perform modifications to wdscapture.inf without touching the ACLs.
echo [Capture] > %wdscapturepath%
echo Unattended=No >> %wdscapturepath%
echo VolumeToCapture= >> %wdscapturepath%
echo SystemRoot= >> %wdscapturepath%
echo ImageName= >> %wdscapturepath%
echo ImageDescription= >> %wdscapturepath%
echo DestinationFile= >> %wdscapturepath%
echo Overwrite=No >> %wdscapturepath%
echo. >> %wdscapturepath%
echo [ExclusionList] >> %wdscapturepath%
echo $ntfs.log >> %wdscapturepath%
echo hiberfil.sys >> %wdscapturepath%
echo pagefile.sys >> %wdscapturepath%
echo "System Volume Information" >> %wdscapturepath%
echo RECYCLER >> %wdscapturepath%
echo winpepge.sys >> %wdscapturepath%
echo %%SYSTEMROOT%%\CSC >> %wdscapturepath%
echo $DISMTOOLS.~BT >> %wdscapturepath%
echo $DISMTOOLS.~WS >> %wdscapturepath%
echo CWS_SYSPRP >> %wdscapturepath%
echo capture_completed >> %wdscapturepath%
echo. >> %wdscapturepath%
echo [WDS] >> %wdscapturepath%
echo UploadToWDSServer=No >> %wdscapturepath%
echo WDSServerName= >> %wdscapturepath%
echo WDSImageGroup= >> %wdscapturepath%
echo Username= >> %wdscapturepath%
echo Password= >> %wdscapturepath%
echo DeleteLocalWimOnSuccess=No >> %wdscapturepath%
echo. >> %wdscapturepath%
IF %_DEBUG% EQU 1 echo WdsCapture file created.
exit /b
