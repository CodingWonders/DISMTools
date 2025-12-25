@echo off
:: Refresh the local packages directory
if exist .\pkgsrc (rd .\pkgsrc /s /q)
md pkgsrc
if exist .\pkgsrc (xcopy .\packages\* .\pkgsrc\ /cehyi)
