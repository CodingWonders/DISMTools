@ECHO OFF
SETLOCAL ENABLEDELAYEDEXPANSION

CD %~dp0

SET ARGCOUNT=0
FOR %%A IN (%*) DO SET /A ARGCOUNT+=1

REM check privileges AND versions; pnputil surely changed from Windows 8 to Windows 10.
NET SESSION >NUL 2>&1
IF %ERRORLEVEL% GTR 0 (
	ECHO Administrator privileges are required to run this script.
	PAUSE > NUL
	EXIT /B 1
)

FOR /F "tokens=3" %%A IN ('reg query "HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion" /v CurrentBuildNumber 2^>nul') DO (
	IF %%A GEQ 6000 IF %%A GTR 9600 (
		ECHO This script can only be run from Windows Vista to 8.1.
		PAUSE > NUL
		EXIT /B 1
	)
)

IF %ARGCOUNT% NEQ 1 (
	ECHO Usage: nt6_online_pnputil_recurse.bat ^<path_to_driver_folder^>
	EXIT /B 0
)

SET _SourceDir=%1
FOR %%A IN (%_SourceDir%) DO SET _SourceDir=%%~A

IF EXIST "%_SourceDir%\" (
	ECHO Adding drivers from directory...
	FOR /F "delims=" %%A IN ('dir "%_SourceDir%\*.inf" /s /b /a-d') DO (
		pnputil -i -a "%%A"
	)
) ELSE IF EXIST "%_SourceDir%" (
	ECHO The path provided appears to be a file.
	PAUSE > NUL
	EXIT /B 1
) ELSE (
	ECHO The path provided does not exist.
	PAUSE > NUL
	EXIT /B 1
)