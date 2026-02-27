@ECHO OFF
REM Enable ANSI escape sequence support
FOR /f %%A IN ('ECHO prompt $E ^| CMD') DO SET "ESC=%%A"

CALL :DISPLAY_BANNER

NET SESSION >NUL 2>&1
IF %ERRORLEVEL% GTR 0 (
	GOTO :ERR_NOADMIN_PRIVS
)

REM File names for diskpart scripts
SET DISKPART_LISTDISKS_FILE=%TEMP%\diskpart_listdisk_%RANDOM%.dp
SET DISKPART_LISTPARTS_FILE=%TEMP%\diskpart_listpart_%RANDOM%.dp
SET DISKPART_LISTVOLS_FILE=%TEMP%\diskpart_listvols_%RANDOM%.dp
SET DISKPART_ASSIGNLETTER_FILE=%TEMP%\diskpart_assignletter_%RANDOM%.dp

SETLOCAL ENABLEDELAYEDEXPANSION

REM The operating mode of the script. 0 -- Unknown; 1 -- Legacy; 2 -- UEFI
SET OPERATINGMODE=0

REM Calculate the number of arguments that were passed to the script.
SET ARGUMENTCOUNT=0
FOR %%X IN (%*) DO SET /A ARGUMENTCOUNT+=1

IF %ARGUMENTCOUNT% EQU 1 (
	REM Check the specified fwconfig value.
	IF "%1" == "BIOS" SET OPERATINGMODE=1
	IF "%1" == "LEGACY" SET OPERATINGMODE=1
	IF "%1" == "MBR" SET OPERATINGMODE=1
	IF "%1" == "UEFI" SET OPERATINGMODE=2
	IF "%1" == "GPT" SET OPERATINGMODE=2
	
	REM If the operating mode is still unknown, that means we haven't passed the right value.
	IF !OPERATINGMODE! LEQ 0 GOTO :ERR_INVALID_FWVALUE
) ELSE (
	GOTO :USAGE
)

IF %OPERATINGMODE% EQU 1 ECHO The script will operate in %ESC%[7mLEGACY%ESC%[0m mode.
IF %OPERATINGMODE% EQU 2 ECHO The script will operate in %ESC%[7mUEFI%ESC%[0m mode.

REM Check the mode because we can be targeting the wrong firmware type.
IF %OPERATINGMODE% EQU 1 (
	IF NOT "%FIRMWARE_TYPE%" == "Legacy" (
		CALL :WARN_INCOMPATIBLE_TARGET_FIRMWARE || EXIT /B
	)
) ELSE IF %OPERATINGMODE% EQU 2 (
	IF NOT "%FIRMWARE_TYPE%" == "UEFI" (
		CALL :WARN_INCOMPATIBLE_TARGET_FIRMWARE || EXIT /B
	)
)

ECHO Writing data for scripts...

ECHO lis dis > %DISKPART_LISTDISKS_FILE%
ECHO exi >> %DISKPART_LISTDISKS_FILE%
ECHO lis vol > %DISKPART_LISTVOLS_FILE%
ECHO exi >> %DISKPART_LISTVOLS_FILE%

diskpart /s "%DISKPART_LISTDISKS_FILE%"

REM echo.
REM echo - To install drivers if you don't see your drives, type "DIM"
REM echo.

ECHO.

SET /p sourcediskid=Please enter the disk number: 
IF NOT DEFINED sourcediskid (
	ECHO The disk number must be specified.
	EXIT /B 1
)

ECHO sel dis %sourcediskid% > %DISKPART_LISTPARTS_FILE%
ECHO lis par >> %DISKPART_LISTPARTS_FILE%
ECHO exi >> %DISKPART_LISTPARTS_FILE%

diskpart /s "%DISKPART_LISTPARTS_FILE%"

ECHO.
SET /p destpartid=Please enter the partition number for the EFI System Partition (UEFI) or System Reserved Partition (Legacy): 
IF NOT DEFINED destpartid (
	ECHO The partition number must be specified.
	EXIT /B 1
)

diskpart /s "%DISKPART_LISTVOLS_FILE%"

ECHO.
SET /p sourcevolletter=Please enter the letter of the source volume that contains the boot files: 
IF NOT DEFINED sourcevolletter (
	ECHO The volume letter must be specified.
	EXIT /B 1
)
IF NOT EXIST "%sourcevolletter%:\Windows\Boot" (
	ECHO A source volume with no boot files has been specified.
	EXIT /B 1
)
ECHO.
ECHO The following settings will be used for boot file creation:
ECHO - A ESP or System Reserved partition is located on disk %sourcediskid% at partition %destpartid%
ECHO - The source boot files are in %sourcevolletter%:\Windows\Boot
ECHO.
SET /p proceed=Do you want to proceed with boot file creation (Y/N)? 
IF DEFINED proceed IF /I NOT "%proceed%" == "Y" (
	ECHO Operation cancelled. Run the script again.
	EXIT /B 1
)
CALL :CREATE_BOOT_FILES %sourcediskid% %destpartid% %sourcevolletter% %OPERATINGMODE%
ECHO Restart your computer for the boot configuration settings to take effect.
EXIT /B

:DISPLAY_BANNER
ECHO Create Windows boot files
ECHO -------------------------------
EXIT /B

:ERR_NOADMIN_PRIVS
ECHO Administrator privileges need to be present in order to run this script.
EXIT /B 1

:ERR_INVALID_FWVALUE
ECHO An invalid firmware type value has been provided. Supported values are: BIOS; LEGACY; MBR; UEFI; GPT.
EXIT /B 1

:WARN_INCOMPATIBLE_TARGET_FIRMWARE
ECHO.
ECHO     An incompatible firmware type has been specified. While you can continue, you may not be able to boot your computer
ECHO     to your desired operating system even after creating the boot files.
ECHO. 
ECHO     Feel free to IGNORE this warning if you are creating boot files for a drive that will go on another computer that
ECHO     uses, or at least supports, the specified firmware type.
ECHO.
IF %OPERATINGMODE% EQU 1 ECHO       - Target platform for boot files: BIOS/Legacy
IF %OPERATINGMODE% EQU 2 ECHO       - Target platform for boot files: UEFI
ECHO       - Detected firmware type: %FIRMWARE_TYPE%
ECHO.
SET /P INCOMPATIBILITY_CHOICE_OPTION=Do you want to continue (y/N)? 
IF DEFINED INCOMPATIBILITY_CHOICE_OPTION IF /I NOT "%INCOMPATIBILITY_CHOICE_OPTION%" == "Y" (
	EXIT /B 1
) ELSE IF NOT DEFINED INCOMPATIBILITY_CHOICE_OPTION (
	EXIT /B 1
)
EXIT /B

:CREATE_BOOT_FILES
SET VOLLETTER=W
ECHO Assigning volume letter %VOLLETTER% to specified ESP/MSR partition...
ECHO sel dis %1 > %DISKPART_ASSIGNLETTER_FILE%
ECHO sel par %2 >> %DISKPART_ASSIGNLETTER_FILE%
IF %4 EQU 1 ECHO act >> %DISKPART_ASSIGNLETTER_FILE%
ECHO ass letter %VOLLETTER% >> %DISKPART_ASSIGNLETTER_FILE%
ECHO exi >> %DISKPART_ASSIGNLETTER_FILE%
diskpart /s "%DISKPART_ASSIGNLETTER_FILE%"

IF %4 EQU 1 (
	ECHO Creating MBR boot sector...
	bootsect /nt60 %VOLLETTER%:
	bootsect /nt60 %VOLLETTER%: /mbr
)

ECHO Creating boot files...
bcdboot %3:\Windows /s %VOLLETTER%: /f ALL
EXIT /B

:USAGE
ECHO Usage:
ECHO.
ECHO     To use this script, you need to pass at least one of the following values:
ECHO.
ECHO     ---- BIOS^|LEGACY^|MBR -^> This will operate the script in LEGACY mode.
ECHO     ---- UEFI^|GPT        -^> This will operate the script in UEFI mode.
ECHO.
ECHO Information:
ECHO.
ECHO     The script creates boot files for the target operating platform based on the specified mode. On Legacy/BIOS
ECHO     systems, the script will create a MBR boot sector and will create boot files on a disk's System Reserved
ECHO     partition. On UEFI systems, the script will create boot files on a disk's EFI System Partition (ESP).
ECHO.
ECHO     The target disk must follow the partition scheme mandated by the desired operating mode.
ECHO.
ECHO   For UEFI Systems:
ECHO.
ECHO     The script may be operated in Legacy mode if the target system has the required modules enabled in its firmware.
ECHO     Specifically, the %ESC%[1mCompatibility Support Module (CSM)%ESC%[0m. If this module is DISABLED in the firmware, the script can
ECHO     only produce valid results in UEFI mode. To prevent compatibility issues, the script will check the current
ECHO     operating mode of the firmware in the CURRENT environment and warn you of any incompatibilities present. 
ECHO.
ECHO     %ESC%[30;43mNote, however, that the checks performed by this script on this computer may NOT be valid if the drive on%ESC%[0m
ECHO     %ESC%[30;43mwhich the boot files will be stored is used on a computer with a different firmware configuration.%ESC%[0m