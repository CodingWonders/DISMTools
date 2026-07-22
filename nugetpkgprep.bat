@echo off
setlocal
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0tools\build\PrepareNuGetPackages.ps1"
exit /b %ERRORLEVEL%
