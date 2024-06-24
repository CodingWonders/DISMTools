@echo off
setlocal ENABLEDELAYEDEXPANSION
title DISMTools Preinstallation Environment
set version=0.5.1
echo DISMTools %version% - Preinstallation Environment
echo (c) 2024. CodingWonders Software
echo.
echo Please wait while the environment starts up...
wpeinit
powershell -command Set-ExecutionPolicy Unrestricted
set FOUND=0
for %%D in (A B C D E F G H I J K L M N O P Q R S T U V W X Y Z) do (
    if exist "%%D:\" (
        if %FOUND% equ 1 (goto END)
        if exist "%%D:\PE_Helper.ps1" (
            echo Starting script in drive %%D:...
            cd /d %%D:
            powershell .\PE_Helper.ps1 StartApply
            if %ERRORLEVEL% equ 0 (
                set FOUND=1
            )            
        )
    )
)

:END
wpeutil reboot