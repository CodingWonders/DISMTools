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
if %debug% lss 2 if not exist "%sysdrive%\HotInstall" (
	powershell -file "%sysdrive%\menu.ps1"
	if exist "%sysdrive%\netinstall" (
		cd /d "%sysdrive%"\
		powershell -file ".\pxehelpers\wds\wdshelper.ps1"
	)
)
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