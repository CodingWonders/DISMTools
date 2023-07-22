@echo off
:: Refresh the NuGet packages directory
if exist .\packages (rd .\packages /s /q)
if exist .\pkgsrc (copy .\pkgsrc .\packages)
