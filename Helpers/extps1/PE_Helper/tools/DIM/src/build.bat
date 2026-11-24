@ECHO OFF

SET VsDirectory=%ProgramFiles%\Microsoft Visual Studio\18
SET BuildToolPath=""

IF NOT EXIST "%VsDirectory%" (
	GOTO Fail_NoVS
)

FOR /F %%I IN ('dir "%VsDirectory%" /B') DO (
	:: Check if MSBuild exists in the edition. That way we check what editions of VS are installed
	IF EXIST "%VsDirectory%\%%I\MSBuild\Current\Bin\%PROCESSOR_ARCHITECTURE%\MSBuild.exe" (
		SET BuildToolPath="%VsDirectory%\%%I\MSBuild\Current\Bin\%PROCESSOR_ARCHITECTURE%\MSBuild.exe"
		ECHO Building executables...
		GOTO BuildNow
	)
)
GOTO Fail_NoBuildTools

:BuildNow
%BuildToolPath% DT-DIM.vcxproj /p:Configuration=Debug /p:Platform=Win32
%BuildToolPath% DT-DIM.vcxproj /p:Configuration=Debug /p:Platform=x64
%BuildToolPath% DT-DIM.vcxproj /p:Configuration=Debug /p:Platform=ARM64
ECHO Executables were built. Copying them to final outputs...
XCOPY Win32\Debug\DT-DIM.exe ..\i386\DT-DIM.exe /cey /-i
XCOPY x64\Debug\DT-DIM.exe ..\amd64\DT-DIM.exe /cey /-i
XCOPY ARM64\Debug\DT-DIM.exe ..\aarch64\DT-DIM.exe /cey /-i
EXIT /B

:Fail_NoVS
ECHO No Visual Studio 2026 installation has been found in the standard location.
EXIT /B

:Fail_NoBuildTools
ECHO No build tools were found
EXIT /B