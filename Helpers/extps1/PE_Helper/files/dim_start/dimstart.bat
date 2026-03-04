@echo off
setlocal ENABLEDELAYEDEXPANSION
set sysdrive=%SYSTEMDRIVE%

set _ShowPnputilOut=1
for /f "tokens=3" %%a in ('reg query "HKLM\SOFTWARE\DISMTools\Preinstallation Environment\Policies" /v DTDimShowPnputilOut 2^>nul') do (
	if /i "%%a" == "0x1" set _ShowPnputilOut=1
	if /i "%%a" == "0x0" set _ShowPnputilOut=0
)

if !_ShowPnputilOut! equ 1 (
	REM display hardware IDs that require drivers...
	echo These are the device IDs of the hardware devices that could not be detected. Please > %sysdrive%\unknowndevs.txt
	echo install device drivers based on hardware IDs. After installation, please close this window. >> %sysdrive%\unknowndevs.txt
	pnputil /enum-devices /problem >> %sysdrive%\unknowndevs.txt
	start "" notepad %sysdrive%\unknowndevs.txt
)

for %%D in (A B C D E F G H I J K L M N O P Q R S T U V W X Y Z) do (
	if exist "%%D:\" (
		if exist "%%D:\Tools\DIM" (
			echo Copying program tools to the environment...
			cd /d %%D:
			if not exist "%sysdrive%\Tools\DIM" (md "%sysdrive%\Tools\DIM")
			xcopy "%%D:\Tools\DIM\*" "%sysdrive%\Tools\DIM" /cehyi > nul
			cd /d %sysdrive%
			echo Starting the Driver Installation Module for architecture %PROCESSOR_ARCHITECTURE%...
			if "%PROCESSOR_ARCHITECTURE%" equ "X86" (
				"%sysdrive%\Tools\DIM\i386\DT-DIM.exe"
			) else if "%PROCESSOR_ARCHITECTURE%" equ "AMD64" (
				"%sysdrive%\Tools\DIM\amd64\DT-DIM.exe"
			) else if "%PROCESSOR_ARCHITECTURE%" equ "ARM64" (
				"%sysdrive%\Tools\DIM\aarch64\DT-DIM.exe"
			)
			exit
		)
	)
)