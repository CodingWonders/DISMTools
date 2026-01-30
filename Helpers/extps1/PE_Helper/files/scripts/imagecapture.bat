@echo off
set sysdrive=%SYSTEMDRIVE%
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
	set /p destuser=Please enter the username: 
	set /p destpassword=Please enter the password: 

	echo Connecting to network share...
	REM for results to appear in HKCU\Network, we need to make the share persistent
	net use * "%destip%" %destpassword% /USER:%destuser% /P:Yes

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

if not defined destdrive ( set /p destdrive=Please enter the letter of the volume the file will be stored on: )
if not defined destdrive (
	echo The letter of the volume where the image will be stored must be specified.
	exit /b 1
)

echo.
set /p destfile=Enter a file name for the target WIM file. Press ENTER without specifying anything to continue with a random name: 
if not defined destfile (
	set destfile=install_%RANDOM%.wim
)

REM verify if we typed the correct extension -- if not, add it
for %%a in (%destfile%) do (
	if /i not "%%~xa" == ".WIM" set destfile=!destfile!.wim
)

set /p imagename=Provide a custom name (without quotes) for the resulting Windows image (e.g., "My Amazing Windows installation"): 
if not defined imagename (
	set imagename=Windows
)

echo Capturing Windows installation to the target WIM file. This can take a long time, depending on the computer's speed.
call :create_config_list %sourcedrive%
if exist "%SYSTEMDRIVE%\SysprepPrepTool" (
	call :sysprep_hotinstall_remove_temp_files
)
set dismstart=%date% %time%
dism /capture-image /imagefile="%destdrive%:\%destfile%" /capturedir=%sourcedrive%:\ /scratchdir=%destdrive%:\ /name="%imagename%" /configfile="%configlistpath%" /compress=max /checkintegrity /bootable /verify
if %ERRORLEVEL% equ 0 (
	set succeeded=true
) else (
	set succeeded=false
)
set dismend=%date% %time%
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
bcdedit /delete {current} /f
if exist "%sourcedrive%:\$DISMTOOLS.~BT" rd "%sourcedrive%:\$DISMTOOLS.~BT" /s /q >nul 2>&1
if exist "%sourcedrive%:\$DISMTOOLS.~WS" rd "%sourcedrive%:\$DISMTOOLS.~WS" /s /q >nul 2>&1
if exist "%sourcedrive%:\CWS_SYSPRP" rd "%sourcedrive%:\CWS_SYSPRP" /s /q >nul 2>&1
if exist "%sourcedrive%:\capture_completed" del "%sourcedrive%:\capture_completed" /f /s /q >nul 2>&1
exit /b

:dt_dim_driver_install
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
exit /b
