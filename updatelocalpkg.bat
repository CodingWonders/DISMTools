@echo off
:: Refresh the local packages directory
if exist .\pkgsrc.zip (del .\pkgsrc.zip /f /q)
powershell -command Compress-Archive -Path ".\packages\*.*" -Destination ".\pkgsrc.zip" -Force
