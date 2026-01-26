@ECHO OFF

REM This script automates the removal of the Automatic Reload service on test
REM equipment.

NET SESSION >NUL 2>&1

IF %ERRORLEVEL% GTR 0 (
	ECHO Start this script as an administrator.
	PAUSE > NUL
	EXIT /B 1
)

NET STOP DT_AutoReload 2>NUL
sc delete DT_AutoReload