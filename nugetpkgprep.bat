@echo off
:: Refresh the NuGet packages directory
if exist .\packages (rd .\packages /s /q)
md packages
if exist .\pkgsrc (xcopy .\pkgsrc\* .\packages\ /cehyi)
