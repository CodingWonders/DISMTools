:: DISMTools 0.3 Temporary options script
:: - by CodingWonders

@echo off
choice /c YN /n /m "Preferences set by this tool will be reset when closing the console session. Do you want to continue? "
if %ERRORLEVEL% == 1 (
	goto main
) else if %ERRORLEVEL% == 2 (
	cls
	exit /b 1
)

:main
