@ECHO OFF

CD %~DP0

NET SESSION >NUL 2>&1

IF %ERRORLEVEL% GTR 0 (
	ECHO Start this script as an administrator.
	PAUSE > NUL
	EXIT /B 1
)

sc create DT_AutoReload binPath=".\AutoReloadSvc.exe" start=auto DisplayName="DISMTools Automatic image reload service" depend=EventLog
sc description DT_AutoReload "This service automatically reloads the servicing sessions of all mounted images on this computer. Feel free to disable this service if you don't need it."