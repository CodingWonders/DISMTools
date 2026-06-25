@ECHO OFF

CD %~dp0

FOR /F "SKIP=2 TOKENS=1,2,*" %%A IN ('reg query "HKCU\Control Panel\Desktop" /v WallPaper 2^>nul') DO (
	IF /I NOT "%%~xC" == ".jpg" (
		ECHO Only JPG files are supported.
		PAUSE > NUL
		EXIT /B 1
	)
	ECHO Copying "%%C" to your user data folder...
	IF EXIST "%%C" COPY /Y "%%C" ..\..\userdata\dtpe_backgrounds\wallpaper.jpg
)