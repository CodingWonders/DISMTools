@echo off
:: Refresh the NuGet packages directory
if exist .\packages (rd .\packages /s /q)
md packages
if exist .\pkgsrc.zip powershell -command Expand-Archive -Path ".\pkgsrc.zip" -Destination ".\packages" -Force
