@echo off

REM bcd entries are in bcdinfo; if we have bcdinfo we can query
REM boot entry guids; otherwise we don't do anything

SET sysdrive=%SYSTEMDRIVE%
CLS

IF NOT EXIST "%sysdrive%\bcdinfo" (
    ECHO Boot configuration data files not detected. Exiting...
    EXIT /B 1
)

ECHO   --------------------------------------------------------------------------------------------------------------------
ECHO      Please wait while changes made to your system are being undone. This will take some time; please be patient...
ECHO   --------------------------------------------------------------------------------------------------------------------

SET _NONEXISTENT_BCDINFO=0
IF NOT EXIST "%sysdrive%\bcdinfo\capture_env_entry_guid.txt" SET /A _NONEXISTENT_BCDINFO+=1 >NUL
IF NOT EXIST "%sysdrive%\bcdinfo\current_bcd_entry_guid.txt" SET /A _NONEXISTENT_BCDINFO+=1 >NUL

IF %_NONEXISTENT_BCDINFO% GTR 0 (
    ECHO Boot configuration data files not detected. Exiting...
    EXIT /B 1
)

ECHO Resetting boot entries...
FOR /F %%A IN ('type "%sysdrive%\bcdinfo\capture_env_entry_guid.txt"') DO BCDEDIT /DELETE %%A /F
FOR /F %%A IN ('type "%sysdrive%\bcdinfo\current_bcd_entry_guid.txt"') DO BCDEDIT /DEFAULT %%A
BCDEDIT /DELETE {current} /F

ECHO Scanning drives for temporary Setup files...
FOR %%A IN (A B C D E F G H I J K L M N O P Q R S T U V W X Y Z) DO (
    IF EXIST "%%A:\$DISMTOOLS.~BT" (
        ECHO Deleting setup files in drive %%A...
        RD "%%A:\$DISMTOOLS.~BT" /S /Q
    )
)
PING 127.0.0.1 -N 3 >NUL 2>&1