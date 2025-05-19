@echo off
setlocal ENABLEDELAYEDEXPANSION
title DISMTools Preinstallation Environment
set version=0.7
set sysdrive=%SYSTEMDRIVE%
set debug=0
echo DISMTools %version% - Preinstallation Environment
echo (c) 2024-2025. CodingWonders Software
echo.
echo Please wait while the environment starts up...
wpeinit
if %debug% equ 1 (
	echo Debug mode enabled.
	taskmgr
)
powershell -command Set-ExecutionPolicy Unrestricted
REM if not exist "%sysdrive%\HotInstall" (
	REM echo Choose your preferred installation method:
	REM echo.
	REM echo 1 - Local Installation
	REM echo     Choose this method if you started the Preinstallation Environment using local media, such as
	REM echo     DVD or USB drives. This is recommended for newcomers
	REM Next Section Is Not Ready Yet
	REM echo 2 - Network Installation
	REM echo     Choose this method if you started the Preinstallation Environment using a network-based
	REM echo     deployment solution. This is recommended for system administrators that want to deploy a system
	REM echo     image to multiple computers at once.
	REM echo S - Shut down my computer
	REM echo R - Restart my computer
	REM echo.
	REM echo You will not be able to go back to choose another option after making your decision. You must reboot your
	REM echo computer and select the correct option. You can also restart your computer by closing this window.
	REM echo.
	REM choice /C 12SR /M "Choose an installation method by typing the option and pressing ENTER: "
	REM if %errorlevel% equ 3 (
		REM wpeutil shutdown
	REM ) else if %errorlevel% equ 4 (
		REM wpeutil reboot
	REM )
REM )
if %debug% neq 2 if exist "%sysdrive%\HotInstall" (
	echo Please insert the disc image and press ENTER...
	pause > nul
)
if %debug% lss 2 (
	for %%D in (A B C D E F G H I J K L M N O P Q R S T U V W X Y Z) do (
		if exist "%%D:\" (
			if exist "%%D:\PE_Helper.ps1" (
				echo Starting script in drive %%D:...
				cd /d %%D:
				if exist "%%D:\Tools\DIM" (
					echo.
					echo Copying program tools to the environment...
					if not exist "%sysdrive%\Tools\DIM" (md "%sysdrive%\Tools\DIM")
					xcopy "%%D:\Tools\DIM\*" "%sysdrive%\Tools\DIM" /cehyi > nul
				)
				if exist "%%D:\Tools\RestartDialog" (
					if not exist "%sysdrive%\Tools\RestartDialog" (md "%sysdrive%\Tools\RestartDialog")
					xcopy "%%D:\Tools\RestartDialog\*" "%sysdrive%\Tools\RestartDialog" /cehyi > nul
				)
				powershell .\PE_Helper.ps1 StartApply
			)
		)
	)
) else (
	echo.
	echo.
	echo You have been dropped to a command shell, in which you can test your applications for Windows PE compatibility.
	echo.
	echo - To shut down the system, type "wpeutil shutdown" and press ENTER
	echo - To restart the system, either close this window or type "wpeutil reboot" and press ENTER
	echo - For more Windows PE commands, type "wpeutil"
	echo.
	echo - To manually start the installation procedure, type "StartInstall" and press ENTER. You need a drive containing a Windows image
	echo - To start the Driver Installation Module in case you need to load drivers, type "StartDim" and press ENTER
	echo.
	doskey StartInstall=powershell -file "%sysdrive%\StartInstall.ps1"
	doskey StartDim=cmd /c "%sysdrive%\dimstart.bat"
	exit /b
)