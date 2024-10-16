@echo off
setlocal ENABLEDELAYEDEXPANSION
title DISMTools Preinstallation Environment
set version=0.6
set sysdrive=%SYSTEMDRIVE%
set debug=0
echo DISMTools %version% - Preinstallation Environment
echo (c) 2024. CodingWonders Software
echo.
echo Please wait while the environment starts up...
wpeinit
if %debug% equ 1 (
	echo Debug mode enabled.
	taskmgr
)
powershell -command Set-ExecutionPolicy Unrestricted
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
	echo - To manually start the installation procedure, type "StartInstall" and press ENTER. You need a driver containing a Windows image
	echo.
	doskey StartInstall=powershell -file "\StartInstall.ps1"
	exit /b
)