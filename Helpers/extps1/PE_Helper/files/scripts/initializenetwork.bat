@echo off

if exist "%windir%\system32\wpeutil.exe" (
	REM we know we are running it on WinPE
	
	echo Initializing network...
	wpeutil initializenetwork
	echo Enabling firewall...
	wpeutil enablefirewall
) else (
	echo This script is intended to be run on Windows PE.
)