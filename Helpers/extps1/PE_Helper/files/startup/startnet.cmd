@echo off
setlocal ENABLEDELAYEDEXPANSION
title DISMTools Preinstallation Environment
set version=0.8
set sysdrive=%SYSTEMDRIVE%
set debug=0
echo DISMTools %version% - Preinstallation Environment
echo (c) 2024-2026. CodingWonders Software
echo.
echo Please wait while the environment starts up...
wpeinit
doskey inv=powershell -file "%sysdrive%\DTPE_Inventory.ps1"
if %debug% equ 1 (
	echo Debug mode enabled.
	taskmgr
)
:: powershell -command Set-ExecutionPolicy Unrestricted
:: We no longer do it like this for the sake of performance. If we could not set the ExecutionPolicy value in registry,
:: we'll add it here. If we still couldn't do it, run PowerShell as a fallback
reg query "HKLM\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell" /v "ExecutionPolicy" >nul 2>&1
if !ERRORLEVEL! equ 1 (
	reg add "HKLM\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell" /v "ExecutionPolicy" /t REG_SZ /d "Unrestricted" /f >nul 2>&1
	if !ERRORLEVEL! equ 1 (
		powershell -command Set-ExecutionPolicy Unrestricted
	)
)

:: Detect the keyboard layout to use via policy. Because we don't have findstr here on winpe we have to use a
:: different method.
SET "DefaultKeyboardLayoutCode=00000409"

FOR /F "tokens=3" %%A IN ('reg query "HKLM\SOFTWARE\DISMTools\Preinstallation Environment\Policies" /v KeyboardLayoutCode /t REG_SZ 2^>NUL ^| find "REG_"') DO (
	SET "DefaultKeyboardLayoutCode=%%A"
)

SET KeyboardLayoutConfigured=0
:: If we have a substitute then we have configured the keyboard layout... but first we'll query the preloads
SET "DefaultPreload=00000409"
FOR /F "tokens=3" %%A IN ('reg query "HKCU\Keyboard Layout\Preload" /v 1 /t REG_SZ 2^>nul ^| find "REG_"') DO (
	SET "DefaultPreload=%%A"
)
REG QUERY "HKCU\Keyboard Layout\Substitutes" /v "%DefaultPreload%" /t REG_SZ >nul 2>&1
IF %ERRORLEVEL% EQU 0 SET KeyboardLayoutConfigured=1

IF %KeyboardLayoutConfigured% NEQ 1 (
	FOR /F "tokens=3" %%A IN ('reg query "HKCU\Control Panel\International" /v Locale /t REG_SZ 2^>nul ^| find "REG_"') DO (
		IF /I NOT "%DefaultKeyboardLayoutCode%" == "%%A" (
			echo Configuring keyboard layout to %DefaultKeyboardLayoutCode%...
			wpeutil setkeyboardlayout 0409:%DefaultKeyboardLayoutCode%
			REM we need to open a new session; we can no longer use this one as it will still use the older
			REM keyboard layout
			IF !ERRORLEVEL! EQU 0 start /wait cmd.exe /k "%SYSTEMROOT%\system32\startnet.cmd"
		)
	)
)

:: Determine if we have set the policy to show a watermark
SET ShowWatermark=0
FOR /F "tokens=3" %%A IN ('reg query "HKLM\SOFTWARE\DISMTools\Preinstallation Environment\Policies" /v ShowWatermark 2^>NUL') DO (
	IF /I "%%A" == "0x0" SET ShowWatermark=0
	IF /I "%%A" == "0x1" SET ShowWatermark=1
)

reg query "HKLM\SOFTWARE\DISMTools\Preinstallation Environment" /v DRY_Watermark >nul 2>&1
IF !ERRORLEVEL! EQU 0 SET ShowWatermark=0

if !ShowWatermark! EQU 1 (
	start /b powershell -file "%sysdrive%\ShowWatermark.ps1"
	reg add "HKLM\SOFTWARE\DISMTools\Preinstallation Environment" /f /v DRY_Watermark /t REG_DWORD /d 1 >nul 2>&1
)
if %debug% lss 2 if exist "%sysdrive%\SysprepPrepTool" (
	if exist "%sysdrive%\scripts\imagecapture.bat" (
		echo An image capture will begin now...
		call "%sysdrive%\scripts\imagecapture.bat"
		powershell -command wpeutil shutdown
	)
)
if %debug% lss 2 if not exist "%sysdrive%\HotInstall" (
	powershell -noprofile -file "%sysdrive%\menu.ps1"
	if exist "%sysdrive%\netinstall" (
		REM Determine if we are in a PXE environment
		reg query "HKLM\SYSTEM\CurrentControlSet\Control\PXE" >nul 2>&1
		if !ERRORLEVEL! gtr 0 (
			REM we are NOT in PXE
			echo We have detected that you are launching the PXE Helpers in a non-PXE environment. This
			echo set of conditions is not supported by the PXE Helpers and they may not work correctly.
			echo.
			echo Press ENTER to continue anyway, otherwise, restart your computer by closing this window.
			pause > nul
		)
		
		cd /d "%sysdrive%"\
		powershell -noprofile -file ".\pxehelpers\PXEHelpers.Startup.ps1"
	) else if exist "%sysdrive%\cmdcons" (
		set debug=2
	) else if exist "%sysdrive%\changekeyb" (
		cd /d "%sysdrive%"\
		powershell -noprofile -file ".\ChangeKeyboardLayout.ps1"
	)
)
if %debug% neq 2 if exist "%sysdrive%\HotInstall" (
	echo Please insert the disc image and press ENTER...
	pause > nul
	if exist "%sysdrive%\driver_supplements_added" (
		echo Supplementary drivers were added to the preinstallation environment to allow it to recognize
		echo your drives. However, you will need to use the Driver Installation Module to reinstall these
		echo so they can be carried over to the new installation. The drivers you need are in a folder called
		echo "CWS_HI_SCSI".
	)
)
if %debug% lss 2 (
	REM Determine if we are in a PXE environment
	reg query "HKLM\SYSTEM\CurrentControlSet\Control\PXE" >nul 2>&1
	if !ERRORLEVEL! equ 0 (
		REM we are in PXE
		echo We have detected that you are launching the PE Helper in a PXE environment. This
		echo environment is not supported by the PE Helper.
		echo.
		echo Press ENTER to restart your computer.
		pause > nul
		wpeutil reboot
	)
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
				powershell -noprofile .\PE_Helper.ps1 StartApply
			)
		)
	)
) else (
	echo.
	echo.
	if exist "%sysdrive%\cmdcons" ( cls )
	echo You have been dropped to a command shell.
	echo.
	echo - To shut down the system, type "wpeutil shutdown" and press ENTER
	echo - To restart the system, either close this window or type "wpeutil reboot" and press ENTER
	echo - To initialize networking, type "netinit" and press ENTER
	echo - To show hardware and software inventory, type "inv" and press ENTER
	echo - To change the keyboard layout, type "keyboardchange" and press ENTER
	echo - For more Windows PE commands, type "wpeutil"
	echo.
	echo - To manually start the installation procedure, type "StartInstall" and press ENTER. You need a drive containing a Windows image
	echo - To start the Driver Installation Module in case you need to load drivers, type "StartDim" and press ENTER
	echo.
	echo Some administration scripts are included in the "scripts" directory, in "%sysdrive%". Type "cd %sysdrive%\scripts" to
	echo go to this directory.
	echo If you have a script that you think will be useful for this kind of environment, feel free to make it a contribution.
	echo The more, the better.
	echo.
	echo This environment will automatically shut down in 72 hours.
	echo.
	doskey StartInstall=powershell -file "%sysdrive%\StartInstall.ps1"
	doskey StartDim=cmd /c "%sysdrive%\dimstart.bat"
	doskey netinit=cmd /c "%sysdrive%\scripts\initializenetwork.bat"
	doskey keyboardchange=powershell -noprofile -file "%sysdrive%\ChangeKeyboardLayout.ps1"
	exit /b
)