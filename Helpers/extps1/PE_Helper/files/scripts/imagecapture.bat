@echo off
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

echo lis vol > %scriptpath%
echo exi >> %scriptpath%

diskpart /s %scriptpath%

set /p sourcedrive=Please enter the letter of the volume to capture: 
if not defined sourcedrive (
	echo The letter of the volume to capture must be specified.
	exit /b 1
)
set /p destdrive=Please enter the letter of the volume the file will be stored on: 
if not defined destdrive (
	echo The letter of the volume where the image will be stored must be specified.
	exit /b 1
)
echo.
set /p destfile=Enter a file name for the target WIM file. Press ENTER without specifying anything to continue with a random name: 
if not defined destfile (
	set destfile=install_%RANDOM%.wim
)

set /p imagename=Provide a custom name (without quotes) for the resulting Windows image (e.g., "My Amazing Windows installation"): 
if not defined imagename (
	set imagename=Windows
)

echo Capturing Windows installation to the target WIM file. This can take a long time, depending on the computer's speed.
call :create_config_list %sourcedrive%
set dismstart=%date% %time%
dism /capture-image /imagefile=%destdrive%:\%destfile% /capturedir=%sourcedrive%:\ /scratchdir=%destdrive%:\ /name="%imagename%" /configfile="%configlistpath%" /compress=max /checkintegrity /bootable /verify
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
echo. >> %configlistpath%
echo [CompressionExclusionList] >> %configlistpath%
echo *.mp3 >> %configlistpath%
echo *.zip >> %configlistpath%
echo *.cab >> %configlistpath%
echo \WINDOWS\inf\*.pnf >> %configlistpath%