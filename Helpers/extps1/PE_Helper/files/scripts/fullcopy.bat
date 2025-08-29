@echo off

REM set this to a value higher than 0 to manually specify the number of threads for robocopy. 0 or lesser will default to 8
set manualthreads=0
REM set this value to customize the wait time between retries. Value of 0: no wait time
set waittime=5
REM set this value to customize the number of retries on failed copies. Value of 0: no retries
set numberofretries=2
REM set this value to customize what to copy when copying files. Refer to robocopy documentation
set filecopy_operation=DAT
REM set this value to customize what to copy when copying directories. Refer to robocopy documentation
set dircopy_operation=DAT

if not exist "%windir%\system32\robocopy.exe" (
	echo Robocopy is a requirement.
	exit 1
)

if %manualthreads% leq 0 (
	set manualthreads=8
)

echo Full Disk Copy Utility -- this will copy the full contents of a drive to another drive. Make sure destination drive is blank
set /p sourcedrive=Please enter the drive letter for the source drive: 
set /p destdrive=Please enter the drive letter for the destination drive: 

echo The copy operation can take a while, depending on the speed of both disks.

robocopy %sourcedrive%:\ %destdrive%:\ /e /w:%waittime% /r:%numberofretries% /copy:%filecopy_operation% /dcopy:%dircopy_operation% /mt:%manualthreads%