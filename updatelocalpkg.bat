@echo off
:: Refresh the local NuGet package bundle used by CI and offline builds.
if not exist ".\tools\build" mkdir ".\tools\build"
if exist ".\tools\build\pkgsrc.bundle" del ".\tools\build\pkgsrc.bundle" /f /q
powershell -NoProfile -ExecutionPolicy Bypass -Command "Compress-Archive -Path .\packages\* -DestinationPath .\tools\build\pkgsrc.bundle -Force"
