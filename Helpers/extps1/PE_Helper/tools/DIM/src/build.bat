@ECHO OFF

:: Usage: build.bat [/clean]
::   /clean (also -clean, --clean): deletes the intermediate files of every platform before building,
::   so nothing from a previous build (for example, objects made with a different compiler) is reused.

SET VsDirectory=%ProgramFiles%\Microsoft Visual Studio\18
SET BuildToolPath=""
SET BuildTarget=Build

:ParseArgs
IF "%~1"=="" GOTO ArgsParsed
IF /I "%~1"=="/clean" SET BuildTarget=Rebuild& SHIFT & GOTO ParseArgs
IF /I "%~1"=="-clean" SET BuildTarget=Rebuild& SHIFT & GOTO ParseArgs
IF /I "%~1"=="--clean" SET BuildTarget=Rebuild& SHIFT & GOTO ParseArgs
IF "%~1"=="/?" GOTO Usage
ECHO Unknown argument: %~1
GOTO Usage
:ArgsParsed

IF NOT EXIST "%VsDirectory%" (
	GOTO Fail_NoVS
)

FOR /F %%I IN ('dir "%VsDirectory%" /B') DO (
	:: Check if MSBuild exists in the edition. That way we check what editions of VS are installed
	IF EXIST "%VsDirectory%\%%I\MSBuild\Current\Bin\%PROCESSOR_ARCHITECTURE%\MSBuild.exe" (
		SET BuildToolPath="%VsDirectory%\%%I\MSBuild\Current\Bin\%PROCESSOR_ARCHITECTURE%\MSBuild.exe"
		GOTO BuildNow
	)
)
GOTO Fail_NoBuildTools

:BuildNow
IF "%BuildTarget%"=="Rebuild" (
	ECHO Performing a clean build of the executables...
) ELSE (
	ECHO Building executables...
)
%BuildToolPath% DT-DIM.vcxproj /t:%BuildTarget% /p:Configuration=Debug /p:Platform=Win32
%BuildToolPath% DT-DIM.vcxproj /t:%BuildTarget% /p:Configuration=Debug /p:Platform=x64
%BuildToolPath% DT-DIM.vcxproj /t:%BuildTarget% /p:Configuration=Debug /p:Platform=ARM64
ECHO Executables were built. Copying them to final outputs...
XCOPY Win32\Debug\DT-DIM.exe ..\i386\DT-DIM.exe /cey /-i
XCOPY x64\Debug\DT-DIM.exe ..\amd64\DT-DIM.exe /cey /-i
XCOPY ARM64\Debug\DT-DIM.exe ..\aarch64\DT-DIM.exe /cey /-i
EXIT /B

:Usage
ECHO Usage: build.bat [/clean]
ECHO.
ECHO   /clean   Deletes the intermediate files of every platform and builds from scratch.
ECHO            -clean and --clean are also accepted.
EXIT /B 1

:Fail_NoVS
ECHO No Visual Studio 2026 installation has been found in the standard location.
EXIT /B

:Fail_NoBuildTools
ECHO No build tools were found
EXIT /B