:: DISMTools Helper Script - version 0.3
@echo off


:init
:: Set initial vars
set script_ver=v0.3
set outputmode=0
:: outputmode=0 (output to file)
::            1 (output to console)
set mountdir=
prompt $g 

:admin_check
net session >NUL 2>&1
if %ERRORLEVEL% gtr 0 (
    echo This script is not being run as an administrator. Right-click it, then select "Run as administrator".
    pause > NUL
    exit /b 1
) else (
    goto detect_args
)

:detect_args
if "%1%"=="/sh" (
    goto dt_sh
) else if "%1%"=="/drinfo" (
	goto dt_wmic
) else (
	echo Unrecognized parameter. Available parameters: sh, drinfo
	exit /b 1
)

:dt_wmic
cls
echo Getting drive information. Please wait...
if exist .\wmic (
	del .\wmic
)
wmic diskdrive list brief > .\wmic
if %ERRORLEVEL% equ 0 (
	exit /b 0
) else (
	exit /b 1
)

:dt_sh
cls
title DISMTools Command Console
echo DISMTools - Command Console (%script_ver%)
echo.
echo Current time is: %TIME% on %DATE%
echo Ready to accept user input.
echo If you are new to the command line, or just want to know how to do a specific task, type CMDHELP (case-insensitive) to show the DISMTools Command Help (console view).
echo.
doskey pwd=cd
doskey getappxpkg=powershell "bin\extps1\extappx.ps1"
if exist ".\extbatch" (
    path %cd%"\extbatch";"%windir%\system32";"%windir%\system32\wbem";"%windir%\system32\WindowsPowerShell\v1.0"
) else (
    path %cd%".\bin\extbatch";"%windir%\system32";"%windir%\system32\wbem";"%windir%\system32\WindowsPowerShell\v1.0"
)