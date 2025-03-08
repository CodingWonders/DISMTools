@echo off
setlocal ENABLEDELAYEDEXPANSION
set sysdrive=%SYSTEMDRIVE%

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