@echo off
setlocal EnableDelayedExpansion

echo DISMTools Command Help

if "%1%"=="" (goto command_help)
if "%1%"=="append-image" (
    goto cmd_append_image
) else if "%1%"=="apply-ffu" (
    goto cmd_apply_ffu
) else if "%1%"=="apply-image" (
    goto cmd_apply_image
) else if "%1%"=="capture-customimage" (
    goto cmd_capture_customimage
) else if "%1%"=="capture-ffu" (
    goto cmd_capture_ffu
) else if "%1%"=="capture-image" (
    goto cmd_capture_image
) else if "%1%"=="cleanup-mountpoints" (
    goto cmd_cleanup_mountpoints
) else if "%1%"=="commit-image" (
    goto cmd_commit_image
) else if "%1%"=="delete-image" (
    goto cmd_delete_image
) else if "%1%"=="export-image" (
    goto cmd_export_image
) else if "%1%"=="get-imageinfo" (
    goto cmd_get_imageinfo
) else if "%1%"=="get-mountedimageinfo" (
    goto cmd_get_mountedimageinfo
) else if "%1%"=="get-wimbootentry" (
    goto cmd_get_wimbootentry
) else if "%1%"=="list-image" (
    goto cmd_list_image
) else if "%1%"=="mount-image" (
    goto cmd_mount_image
) else if "%1%"=="optimize-ffu" (
    goto cmd_optimize_ffu
) else if "%1%"=="optimize-image" (
    goto cmd_optimize_image
) else if "%1%"=="remount-image" (
    goto cmd_remount_image
) else if "%1%"=="split-ffu" (
    goto cmd_split_ffu
) else if "%1%"=="split-image" (
    goto cmd_split_image
) else if "%1%"=="unmount-image" (
    goto cmd_unmount_image
) else if "%1%"=="update-wimbootentry" (
    goto cmd_update_wimbootentry
) else if "%1%"=="apply-siloedpackage" (
    goto cmd_apply_siloedpackage
) else if "%1%"=="global-optns" (
    goto cmd_global
) else if "%1%"=="get-packages" (
    goto cmd_get_packages
) else if "%1%"=="get-packageinfo" (
    goto cmd_get_packageinfo
) else if "%1%"=="add-package" (
    goto cmd_add_package
) else if "%1%"=="remove-package" (
    goto cmd_remove_package
) else if "%1%"=="get-features" (
    goto cmd_get_features
) else if "%1%"=="get-featureinfo" (
    goto cmd_get_featureinfo
) else if "%1%"=="enable-feature" (
    goto cmd_enable_feature
) else if "%1%"=="disable-feature" (
	goto cmd_disable_feature
) else if "%1%"=="cleanup-image" (
    goto cmd_cleanup_image
) else if "%1%"=="--go-online" (
    goto online_help
) else (
    echo.
    echo The command you have entered does not seem to be a valid one to show help for.
    echo Please specify a command where you have doubts.
    goto command_help
)



:command_help
echo.
echo These are the commands you can use with the Deployment Image Servicing and Management (DISM) tool (sorted by functionality):
echo.
echo    Image management: append-image apply-ffu apply-image capture-customimage capture-ffu capture-image
echo                      cleanup-mountpoints commit-image delete-image export-image get-imageinfo get-mountedimageinfo
echo                      get-wimbootentry list-image mount-image optimize-ffu optimize-image remount-image split-ffu
echo                      split-image unmount-image update-wimbootentry apply-siloedpackage
echo.
echo         OS packages: get-packages get-packageinfo add-package remove-package get-features get-featureinfo enable-feature
echo                      disable-feature cleanup-image
echo.
echo   Provisioning pkgs: add-provisioningpackage get-provisioningpackageinfo apply-customdataimage
echo.
echo   App pkg servicing: get-provisionedappxpackages add-provisionedappxpackage remove-provisionedappxpackage
echo                      optimize-provisionedappxpackage set-provisionedappxdatafile {stubpackageoption}
echo.
echo  AppPatch servicing: check-apppatch get-apppatchinfo get-apppatches get-appinfo
echo.
echo  Default app assoc.: export-defaultappassociations get-defaultappassociations import-defaultappassociations
echo           servicing  remove-defaultappassociations
echo.
echo      Langs and intl: get-intl set-uilang set-uilangfallback set-sysuilang set-syslocale set-userlocale set-inputlocale
echo           servicing  set-allintl set-timezone set-skuintldefaults set-layereddriver gen-langini set-setupuilang
echo                      {distribution}
echo.
echo    Capabilities pkg: add-capability export-source get-capability get-capabilityinfo remove-capability
echo           servicing
echo.
echo     Windows edition: get-currentedition get-targeteditions set-edition set-productkey
echo           servicing
echo.
echo    Driver servicing: get-drivers get-driverinfo add-driver remove-driver export-driver
echo.
echo Unattended ans file: apply-unattend
echo           servicing
echo.
echo     WinPE servicing: get-pesettings get-scratchspace get-targetpath set-scratchspace set-targetpath
echo.
echo        OS uninstall: get-osuninstallwindow initiate-osuninstall remove-osuninstall set-osuninstallwindow
echo.
echo    Reserved storage: set-reservedstoragestate get-reservedstoragestate
echo.
echo      Microsoft Edge: add-edge add-edgebrowser add-edgewebview
echo           servicing
echo.
echo ------------------------------------------------------------------------------------------------------------------------
echo.
echo       Miscellaneous: global-optns
echo.
echo CREDITS:
echo.
echo    Command help documentation source: Microsoft
echo.
echo    NOTE: you can access online documentation by passing the "--go-online" flag
echo.
echo NOTES:
echo - The items shown between curly brackets "{}" are arguments that can be passed to other servicing commands of a specific
echo   functionality type
echo - Not all commands may be present in all versions of DISM, and may require specific ones. If your operating system's version does not support a specific command, consider downloading the Assessment and Deployment Kit (ADK), which also contains DISM
echo - Not all commands may be applicable to all Windows images, and may vary between versions
echo.
echo USAGE:
echo.
echo     cmdhelp {command}
echo.
echo   Examples:
echo.
echo     cmdhelp gen-langini
echo       Launches the "gen-langini" section of the command help.
echo.
exit /b

:cmd_append_image
echo.
echo             Command: append-image
echo.
echo         Description: Adds an additional image to a .wim file
echo.
echo              Syntax: "dism /append-image /imagefile:<destination_file> /capturedir:<source_directory> /name:<image_name> (/description:<image_description>) (/configfile=<config_file.ini>) (/bootable) /wimboot (/checkintegrity) (/verify) (/norpfix)"
echo.
echo Append-Image compares new files to the resources in the existing .wim file specified by the "/imagefile" argument, and
echo stores only a single copy of each unique file to make sure that each file is captured once. The .wim file can have only
echo one assigned compression type, so each file can be only appended using the same compression type.
echo.
echo NOTES:
echo - Make sure you have enough disk space for the command to run. If you run out of space, you may corrupt the .wim file
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /wimboot                                                      Append image with Windows image file boot
echo                                                                  configuration (WIMBoot)
echo                                                                  NOTE: this option only applies to Windows 8.1
echo                                                                        that have been captured or exported as a
echo                                                                        WIMBoot file, and is not supported on Windows
echo                                                                        10 or newer
echo.
echo    /configfile                                                   Specify the location of a valid, existing
echo                                                                  configuration file to set exclusions for
echo                                                                  image capture and compress commands.
echo.
echo    /bootable                                                     Mark a volume as being a bootable image
echo                                                                  NOTE: this option only applies to Windows
echo                                                                        Preinstallation Environment (WinPE)
echo                                                                        images, and only one volume image can be
echo                                                                        marked as bootable in a .wim file
echo.
echo    /checkintegrity                                               Detect and track .wim file corruption when being
echo                                                                  used with capture, unmount, export, and commit
echo                                                                  operations
echo                                                                  NOTE: if DISM detects corruption in the
echo                                                                        .wim file when used with apply or mount
echo                                                                        operations, /checkintegrity stops the
echo                                                                        operation
echo.
echo    /verify                                                       Check for errors and file duplication
echo.
echo    /norpfix                                                      Disable the reparse point tag fix (a file 
echo                                                                  that contains a link to another file on the file
echo                                                                  system). If this option is specified, reparse
echo                                                                  points that resolve to paths outside of the value
echo                                                                  specified by "/imagefile" will not be captured
echo.
echo EXAMPLE:
echo.
echo     "Dism /Append-Image /ImageFile:install.wim /CaptureDir:D:\ /Name:Drive-D"
echo.
exit /b

:cmd_apply_ffu
echo.
echo             Command: apply-ffu
echo.
echo         Description: Applies a FFU or SFU file to a physical drive
echo.
echo              Syntax: "dism /apply-ffu /imagefile:<path_to_image_file> /applydrive:<physical_drive_path> (/sfufile:<sfu_pattern>)"
echo.
echo Apply-FFU applies a Full Flash Utility (FFU) or split FFU (SFU) file to a specified physical drive
echo.
echo NOTES:
echo - For more general image files, see "apply-image"
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /imagefile                                                    The path and name of the FFU image file to apply
echo.
echo    /applydrive                                                   The path of the physical drive to apply the image
echo                                                                  to
echo.
echo    /sfufile:"<pattern>"                                          (OPTIONAL, for uncompressed split FFU files (SFU))
echo                                                                  Reference split FFU files. "<pattern>" is the naming
echo                                                                  pattern and location of split files. A wildcard
echo                                                                  character ("*") should be used when specifying the
echo                                                                  naming pattern.
echo                                                                  REFERENCE: "E:\img\install*.sfu" will
echo                                                                             apply all split files in the "E:\img"
echo                                                                             directory:
echo                                                                             - "E:\img\install1.sfu"
echo                                                                             - "E:\img\install2.sfu"
echo                                                                             and so on
echo.
echo EXAMPLE:
echo.
echo     "DISM.exe /Apply-Ffu /ImageFile:flash.ffu /ApplyDrive:\\.\PhysicalDrive0"
echo.
exit /b


:cmd_apply_image
echo.
echo             Command: apply-image
echo.
echo         Description: Applies an image file (.wim, .swm) to a specified partition
echo.
echo              Syntax: "dism /apply-image /imagefile:<path_to_image_file> (/swmfile:<pattern>) /applydir:<target_dir> {/index:<img_index> | /name:<img_name>} (/checkintegrity) (/verify) (/norpfix) (/confirmtrustedfile) (/wimboot) (/compact) (/ea)"
echo.
echo Apply-Image applies a Windows Image file (.wim) or a split Windows image (.swm) to a specified partition.
echo.
echo NOTES:
echo - For Full Flash Utility (.ffu) or split FFU (.sfu) files, see "apply-ffu"
echo - Starting with Windows 10, version 1607 (Anniversary Update), DISM can apply and capture extended attributes
echo - This option does not support applying images from virtual hard disk (.vhd(x)) files, but you can apply images
echo   to attached, partitioned and formatted VHDX files
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /imagefile                                                    The path and name of the WIM/SWM image file to apply
echo.
echo    /applydrive                                                   The path of the physical drive to apply the image
echo                                                                  to
echo.
echo    /swmfile:"<pattern>"                                          Reference split WIM files. "<pattern>" is the naming
echo                                                                  pattern and location of split files. A wildcard
echo                                                                  character ("*") should be used when specifying the
echo                                                                  naming pattern.
echo                                                                  REFERENCE: "E:\img\install*.swm" will
echo                                                                             apply all split files in the "E:\img"
echo                                                                             directory:
echo                                                                             - "E:\img\install1.swm"
echo                                                                             - "E:\img\install2.swm"
echo                                                                             and so on
echo.
echo    /checkintegrity                                               Detect and track .wim file corruption when being
echo                                                                  used with capture, unmount, export, and commit
echo                                                                  operations
echo                                                                  NOTE: if DISM detects corruption in the
echo                                                                        .wim file when used with apply or mount
echo                                                                        operations, /checkintegrity stops the
echo                                                                        operation
echo.
echo    /verify                                                       Check for errors and file duplication
echo.
echo    /norpfix                                                      Disable the reparse point tag fix (a file 
echo                                                                  that contains a link to another file on the file
echo                                                                  system). If this option is specified, reparse
echo                                                                  points that resolve to paths outside of the value
echo                                                                  specified by "/imagefile" will not be captured
echo.
echo    /confirmtrustedfile                                           Validate the image for Trusted Desktop.
echo                                                                  NOTE: this option only applies to computers running
echo                                                                        at least WinPE 4.0. When using this option, 
echo                                                                        the "/scratchdir" option must be specified.
echo                                                                        This ensures that short file names will always
echo                                                                        be available.
echo.
echo    /wimboot                                                      Append the image with Windows Image file boot (WIMBoot)
echo                                                                  configuration.
echo                                                                  NOTE: this option only applies to Windows 8.1 images
echo                                                                        that have been captured or exported as a WIMBoot
echo                                                                        file. This option is not supported on Windows 10
echo                                                                        or newer.
echo.
echo    /compact                                                      Apply an image in compact mode, saving drive space.
echo                                                                  NOTE: this option replaces WIMBoot, and is for desktop
echo                                                                        editions of Windows 10 or newer (Home, Pro, 
echo                                                                        Enterprise or Education). If you specify the 
echo                                                                        "/scratchdir" option, it is not recommended for
echo                                                                        the scratchdir folder to be on a FAT32-formatted 
echo                                                                        volume, as that can result in unexpected reboots
echo                                                                        during initial configuration (OOBE)
echo.
echo    /ea                                                           Apply extended attributes
echo                                                                  NOTE: this option is only available in Windows 10, 
echo                                                                        version 1607 or newer (Windows 11 included)
echo.
echo    /sfufile:"<pattern>"                                          Reference split FFU files. "<pattern>" is the naming
echo                                                                  pattern and location of split files.
echo.
echo EXAMPLES:
echo.
echo     "Dism /apply-image /imagefile:install.wim /index:1 /ApplyDir:D:\"
echo.
echo     "Dism /apply-image /imagefile:install.swm /swmfile:install*.swm /index:1 /applydir:D:"
echo.
exit /b


:cmd_capture_customimage
echo.
echo             Command: capture-customimage
echo.
echo         Description: Captures incremental file changes to a new file
echo.
echo              Syntax: "dism /capture-customimage /capturedir:<source_dir> (/configfile:<config_file.ini>) (/checkintegrity) (/verify) (/confirmtrustedfile)"
echo.
echo Capture-CustomImage captures the incremental file changes based on the specific install.wim file to a new file,
echo custom.wim, for a WIMBoot image
echo.
echo NOTES:
echo - You cannot capture an empty directory
echo - The "custom.wim" file is stored on the same location next to the specified "install.wim" file, and is recommended
echo   not to move either of them
echo - This command only captures the customization files, and cannot be used to capture installation files to a new WIM.
echo   To do that, see "capture-image"
echo - You can only capture the incremental changes to a custom image once, and cannot be done more times
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /capturedir                                                   The directory to which the image was applied and
echo                                                                  customized
echo.
echo    /configfile                                                   Specify the location of a valid, existing
echo                                                                  configuration file to set exclusions for
echo                                                                  image capture and compress commands.
echo.
echo    /checkintegrity                                               Detect and track .wim file corruption when being
echo                                                                  used with capture, unmount, export, and commit
echo                                                                  operations
echo                                                                  NOTE: if DISM detects corruption in the
echo                                                                        .wim file when used with apply or mount
echo                                                                        operations, /checkintegrity stops the
echo                                                                        operation
echo.
echo    /verify                                                       Check for errors and file duplication
echo.
echo    /confirmtrustedfile                                           Validate the image for Trusted Desktop.
echo                                                                  NOTE: this option only applies to computers running
echo                                                                        at least WinPE 4.0.
echo.
echo EXAMPLE:
echo.
echo     "DISM.exe /Capture-CustomImage /CaptureDir:D:\"
echo.
exit /b

:cmd_capture_ffu
echo.
echo             Command: capture-ffu
echo.
echo         Description: Captures an image of a physical drive's partitions to a new .ffu file
echo.
echo              Syntax: "dism /capture-ffu /imagefile:<path_to_img_file> /capturedrive:<physical_drive_path> /name:<img_name> (/description:<img_desc>) (/platformids:<platform_ids>) (/compress:{default|none})"
echo.
echo Capture-FFU lets you capture a physical drive's partitions to a Full Flash Utility (.ffu) image or a set of
echo split FFU (.sfu) files.
echo.
echo NOTES:
echo - To capture to a WIM file, see "capture-image"
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /capturedrive:"\\.\PhysicalDriveX"                            The physical drive to be captured. This option
echo                                                                  uses the format "\\.\PhysicalDriveX", where "X" is
echo                                                                  the disk number.
echo                                                                  NOTE: to view the disk numbers, please run
echo                                                                  "diskpart", and then type "list disk"
echo.
echo    /platformids                                                  Specify one or more platform IDs (separated with
echo                                                                  a semicolon, ";") to be added to the image. If not
echo                                                                  specified, the platform ID will be "*".
echo                                                                  NOTE: this is not needed for desktop capture
echo.
echo    /compress                                                     Specify the type of compression used when capturing.
echo                                                                  NOTE: if you later consider splitting the FFU file,
echo                                                                  specify "none". DISM does not support
echo                                                                  splitting compressed FFUs.
echo.
echo EXAMPLES:
echo.
echo     "DISM.exe /Capture-Ffu /ImageFile:install.ffu /CaptureDrive:\\.\PhysicalDrive0 /Name:Drive0"
echo.
echo          Capture a desktop FFU
echo.
echo     "DISM.exe /Capture-Ffu /ImageFile:install.ffu /CaptureDrive:\\.\PhysicalDrive0 /Name:Drive0 /Compress:none"
echo.
echo          Capture a desktop FFU that will later be split
echo.
exit /b


:cmd_capture_image
echo.
echo             Command: capture-image
echo.
echo         Description: Captures an image of a drive to a new .wim file
echo.
echo              Syntax: "dism /capture-image /imagefile:<path_to_img_file> /capturedir:<source_dir> /name:<img_name> (/description:<img_desc>) (/configfile:<config_file.ini>) (/compress:{max|fast|none} (/bootable) | (/wimboot)) (/checkintegrity) (/verify) (/norpfix) (/ea)"
echo.
echo Capture-Image captures an image of a drive to a new .wim file. Captured directories include all subfolders
echo and data, so the capture directory cannot be empty (it must contain at least one file). If specified, DISM
echo can apply and capture extended attributes.
echo.
echo NOTES:
echo - You can capture the image as a WIM file or a set of split Windows images (.swm), but you cannot capture
echo   virtual hard disk (.vhd(x))
echo - To capture a FFU file, see "capture-ffu"
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /capturedir:"<source_dir>"                                    The physical drive to be captured.
echo.
echo    /imagefile                                                    The target image to capture the drive's contents to.
echo.
echo    /compress                                                     Specify the type of compression used when capturing.
echo                                                                  The "maximum" option provides the best compression,
echo                                                                  but takes the most time to capture the image.
echo                                                                  The "fast" option provides faster image compression,
echo                                                                  but the resulting files are larger than those
echo                                                                  compressed by using the "maximum" option (this is
echo                                                                  also the default option)
echo                                                                  The "none" option does not apply compression to the
echo                                                                  image.
echo.
echo    /bootable                                                     Mark a volume as being a bootable image
echo                                                                  NOTE: this option only applies to Windows
echo                                                                        Preinstallation Environment (WinPE)
echo                                                                        images, and only one volume image can be
echo                                                                        marked as bootable in a .wim file
echo.
echo    /checkintegrity                                               Detect and track .wim file corruption when being
echo                                                                  used with capture, unmount, export, and commit
echo                                                                  operations
echo                                                                  NOTE: if DISM detects corruption in the
echo                                                                        .wim file when used with apply or mount
echo                                                                        operations, /checkintegrity stops the
echo                                                                        operation
echo.
echo    /verify                                                       Check for errors and file duplication
echo.
echo    /norpfix                                                      Disable the reparse point tag fix (a file 
echo                                                                  that contains a link to another file on the file
echo                                                                  system). If this option is specified, reparse
echo                                                                  points that resolve to paths outside of the value
echo                                                                  specified by "/imagefile" will not be captured
echo.
echo    /wimboot                                                      Append the image with Windows Image file boot (WIMBoot)
echo                                                                  configuration.
echo                                                                  NOTE: this option only applies to Windows 8.1 images
echo                                                                        that have been captured or exported as a WIMBoot
echo                                                                        file. This option is not supported on Windows 10
echo                                                                        or newer.
echo.
echo    /ea                                                           Apply extended attributes
echo                                                                  NOTE: this option is only available in Windows 10, 
echo                                                                        version 1607 or newer (Windows 11 included)
echo.
echo EXAMPLES:
echo.
echo     "DISM.exe /Capture-Image /ImageFile:install.wim /CaptureDir:D:\ /Name:Drive-D"
echo.
echo     "DISM.exe /Capture-Image /CaptureDir:C:\ /ImageFile:"C:\WindowsWithOffice.wim" /Name:"Chinese Traditional" /ea"
echo.
exit /b


:cmd_cleanup_mountpoints
echo.
echo             Command: cleanup-mountpoints
echo.
echo         Description: Deletes resources associated with a mounted image that has been corrupted
echo.
echo              Syntax: "dism /cleanup-mountpoints"
echo.
echo Cleanup-Mountpoints will not unmount images that have already been mounted, or delete images recoverable using the
echo "/Remount-Image" command.
echo.
echo NOTES:
echo - If you need to recover an orphaned image, see "remount-image"
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo                                      There are no parameters for this command
echo.
echo EXAMPLE:
echo.
echo     "DISM.exe /Cleanup-Mountpoints"
echo.
exit /b


:cmd_commit_image
echo.
echo             Command: commit-image
echo.
echo         Description: Applies the changes you have made to the mounted image.
echo.
echo              Syntax: "dism /commit-image /mountdir:<path_to_mount_dir> (/checkintegrity) (/append)"
echo.
echo NOTES:
echo - After the operation, the image remains mounted, until the "/Unmount-Image" option is used.
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /checkintegrity                                               Detect and track .wim file corruption when being
echo                                                                  used with capture, unmount, export, and commit
echo                                                                  operations
echo                                                                  NOTE: if DISM detects corruption in the
echo                                                                        .wim file when used with apply or mount
echo                                                                        operations, /checkintegrity stops the
echo                                                                        operation
echo.
echo    /append                                                       Add the modified image to the existing .wim file
echo                                                                  instead of overwriting it.
echo                                                                  NOTE: both arguments do not apply to virtual hard
echo                                                                        disk (VHD(X)) files
echo.
echo EXAMPLE:
echo.
echo     "DISM.exe /Commit-Image /MountDir:C:\test\offline"
echo.
exit /b


:cmd_delete_image
echo.
echo             Command: delete-image
echo.
echo         Description: Deletes the specified volume image from a .wim file that has multiple volume
echo                      images
echo.
echo              Syntax: "dism /delete-image /imagefile:<path_to_img_file> {/index:<img_index> | /name:<img_name>} (/checkintegrity)"
echo.
echo Delete-Image only deletes the metadata and XML entries, but does neither delete the stream data nor optimizes
echo the .wim file
echo.
echo NOTES:
echo - The option does not apply to virtual hard disk (VHD(X)) files
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /checkintegrity                                               Detect and track .wim file corruption when being
echo                                                                  used with capture, unmount, export, and commit
echo                                                                  operations
echo                                                                  NOTE: if DISM detects corruption in the
echo                                                                        .wim file when used with apply or mount
echo                                                                        operations, /checkintegrity stops the
echo                                                                        operation
echo.
echo EXAMPLE:
echo.
echo     "DISM.exe /Delete-Image /ImageFile:install.wim /Index:1"
echo.
exit /b


:cmd_export_image
echo.
echo             Command: export-image
echo.
echo         Description: Exports a copy of the specified image to another file.
echo.
echo              Syntax: "dism /export-image /sourceimagefile:<path_to_img_file> {/sourceindex:<img_index> | /sourcename:<img_name} /destinationimagefile:<path_to_img_file> (/destinationname:<name>) (/compress:{fast|max|none|recovery}) (/bootable) (/wimboot) (/checkintegrity)"
echo.
echo When you modify an image, DISM stores additional resource files that increase the overall size of the image; and
echo exporting the image will remove unnecessary resource files.
echo.
echo NOTES:
echo - The source and destination files must use the same compression type
echo - You can also optimize an image by exporting to a new image file
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /swmfile:"<pattern>"                                          Reference split .wim (SWM) files. "<pattern>"
echo                                                                  is the naming pattern and location of split files.
echo                                                                  You can also specify wildcard ("*") characters.
echo                                                                  REFERENCE: "E:\img\install*.swm" will export the
echo                                                                             split files in the "E:\img" directory:
echo                                                                             - "E:\img\install1.swm"
echo                                                                             - "E:\img\install2.swm"
echo                                                                             and so on
echo.
echo    /compress:"{fast|max|none|recovery}"                          Specify the type of compression used for the initial
echo                                                                  capture operation. The "maximum" option provides the
echo                                                                  best compression, but takes more time to capture the
echo                                                                  image. The "fast" option provides faster image
echo                                                                  compression, but the resulting files are larger than
echo                                                                  those compressed by using the "maximum" option. This
echo                                                                  is the default option if none is specified. The
echo                                                                  "recovery" option exports push-button reset images.
echo                                                                  The resulting files are smaller in size, which in
echo                                                                  turn, greatly reduce the amount of disk space needed
echo                                                                  for saving the push-button reset image on a recovery
echo                                                                  drive. NOTE: the destination image file must have an
echo                                                                  ESD extension. Finally, the "none" option does not
echo                                                                  compress the captured image
echo                                                                  NOTE: this option does not apply when exporting an
echo                                                                        image to an exising one
echo.
echo    /bootable                                                     Mark a volume as being a bootable image
echo                                                                  NOTE: this option only applies to Windows
echo                                                                        Preinstallation Environment (WinPE)
echo                                                                        images, and only one volume image can be
echo                                                                        marked as bootable in a .wim file
echo.
echo    /wimboot                                                      Append the image with Windows Image file boot (WIMBoot)
echo                                                                  configuration.
echo                                                                  NOTE: this option only applies to Windows 8.1 images
echo                                                                        that have been captured or exported as a WIMBoot
echo                                                                        file. This option is not supported on Windows 10
echo                                                                        or newer.
echo.
echo    /checkintegrity                                               Detect and track .wim file corruption when being
echo                                                                  used with capture, unmount, export, and commit
echo                                                                  operations
echo                                                                  NOTE: if DISM detects corruption in the
echo                                                                        .wim file when used with apply or mount
echo                                                                        operations, /checkintegrity stops the
echo                                                                        operation
echo.
echo EXAMPLE:
echo.
echo     "DISM.exe /Export-Image /SourceImageFile:install.wim /SourceIndex:1 /DestinationImageFile:install2.wim"
echo.
exit /b


:cmd_get_imageinfo
echo.
echo             Command: get-imageinfo
echo.
echo         Description: Displays information about the images that are contained in a .wim, .ffu or .vhd(x) file.
echo.
echo              Syntax: "dism /get-imageinfo /imagefile:<path_to_img_file> ({/index:<img_index> | /name:<img_name>})"
echo.
echo When used with the "/Index" or "/Name" argument, information about the specified image is displayed
echo.
echo NOTES:
echo - The "/name" argument does not apply to VHD(X) files
echo - If you wish to get information of a FFU or VHDX file, you must specify "/Index:1"
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo                                      There are no parameters for this command
echo.
echo EXAMPLES:
echo.
echo     "DISM.exe /Get-ImageInfo /ImageFile:C:\test\offline\install.wim"
echo.
echo     "DISM.exe /Get-ImageInfo /ImageFile:C:\test\offline\myimage.vhd /Index:1"
echo.
exit /b


:cmd_get_mountedimageinfo
echo.
echo             Command: get-mountedimageinfo
echo.
echo         Description: Returns a list of .ffu, .vhd(x) and .wim images that are currently mounted
echo.
echo              Syntax: "dism /get-mountedimageinfo"
echo.
echo Get-MountedImageInfo also shows information about the mounted image, such as whether the image is valid,
echo read/write permissions, mount location, mounted file path, and mounted image index
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo                                      There are no parameters for this command
echo.
echo EXAMPLE:
echo.
echo     "DISM.exe /Get-MountedImageInfo"
echo.
exit /b


:cmd_get_wimbootentry
echo.
echo             Command: get-wimbootentry
echo.
echo         Description: Displays WIMBoot configuration entries for the specified disk volume
echo.
echo              Syntax: "dism /get-wimbootentry /path:<volume_path>"
echo.
echo NOTES:
echo - This only applies to Windows 8.1; this feature is not supported in Windows 10
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo                                      There are no parameters for this command
echo.
echo EXAMPLE:
echo.
echo     "DISM.exe /Get-WIMBootEntry /Path:C:\"
echo.
exit /b


:cmd_list_image
echo.
echo             Command: list-image
echo.
echo         Description: Displays a list of the files and folders in a specified image
echo.
echo              Syntax: "dism /list-image /imagefile:<path_to_img_file> {/index:<img_index> | /name:<img_name>}"
echo.
echo NOTES:
echo - This option does not apply to VHD(X) files
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo                                      There are no parameters for this command
echo.
echo EXAMPLE:
echo.
echo     "DISM.exe /List-Image /ImageFile:install.wim /Index:1"
echo.
exit /b


:cmd_mount_image
echo.
echo             Command: mount-image
echo.
echo         Description: Mounts an image from a .ffu, .wim or .vhd(x) file to the specified directory to make it
echo                      available for servicing
echo.
echo              Syntax: "dism /mount-image /imagefile:<path_to_img_file> {/index:<img_index> | /name:<img_name>} /mountdir:<path_to_mount_dir> (/readonly) (/optimize) (/checkintegrity)"
echo.
echo NOTES:
echo - When mounting the image, note the following:
echo   - The mount directory must be created, but empty
echo   - An index or name value is required for all image types. For FFU and VHD, use "/index:1"
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /readonly                                                     (OPTIONAL) Set the mounted image with
echo                                                                  read-only permissions
echo.
echo    /optimize                                                     Reduce initial mount time
echo.
echo    /checkintegrity                                               Detect and track .wim file corruption when being
echo                                                                  used with capture, unmount, export, and commit
echo                                                                  operations
echo                                                                  NOTE: if DISM detects corruption in the
echo                                                                        .wim file when used with apply or mount
echo                                                                        operations, /checkintegrity stops the
echo                                                                        operation
echo.
echo EXAMPLES:
echo.
echo     "DISM.exe /Mount-Image /ImageFile:C:\test\images\myimage.wim /index:1 /mountdir:C:\test\offline"
echo.
echo     "DISM.exe /Mount-Image /ImageFile:C:\test\images\myimage.vhd /index:1 /mountdir:C:\test\offline /ReadOnly"
echo.
echo     "DISM.exe /Mount-Image /ImageFile:C:\test\images\WinOEM.ffu /MountDir:C:\test\offline /index:1"
echo.
exit /b


:cmd_optimize_ffu
echo.
echo             Command: optimize-ffu
echo.
echo         Description: Optimizes an FFU image so they are faster to deploy
echo.
echo              Syntax: "dism /optimize-ffu /imagefile:<path_to_ffu_file> (/partitionnumber:<partnum>)"
echo.
echo This option can also make more easily for an image to be deployed to differently-sized disks.
echo.
echo NOTES:
echo - For other types of files, see "optimize-image"
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /imagefile                                                    Path of the FFU file you want to optimize
echo.
echo    /partitionnumber                                              (OPTIONAL) By default, "/optimize-ffu"
echo                                                                  optimizes the OS partition. This option lets
echo                                                                  you specify the partition you want to optimize
echo.
echo EXAMPLES:
echo.
echo     "DISM.exe /Optimize-FFU /ImageFile:flash.ffu"
echo.
echo     "DISM.exe /Optimize-FFU /ImageFile:flash.ffu /PartitionNumber:2"
echo.
exit /b


:cmd_optimize_image
echo.
echo             Command: optimize-image
echo.
echo         Description: Optimizes an image so it is faster to deploy
echo.
echo              Syntax: "dism /image:<path_to_mount_dir> /optimize-image {/boot | /wimboot}"
echo.
echo This option should be the last one run against an image before it is applied to a device, and can reduce
echo time on the factory floor when building devices for build-to-stock scenarios.
echo.
echo NOTES:
echo - "/Boot" tries to reduce the online configuration time that the OS spends during boot. 
echo   This optimization may be rendered invalid if any servicing operations are performed on the image after optimizing
echo   it.
echo   "dism /optimize-image /boot" is available in the following operating systems:
echo   - Windows 11
echo   - Windows 10, version 1607
echo   - Windows 10, version 1809 and later
echo   - Windows Server 2012 R2 and later
echo - "/WIMBoot" is for configuring an offline image for installing on a WIMBoot system
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo                                      There are no parameters for this command
echo.
echo EXAMPLES:
echo.
echo     "DISM.exe /Image:C:\test\offline /Optimize-Image /Boot"
echo.
echo     "DISM.exe /Image:C:\test\offline /Optimize-Image /WIMBoot"
echo.
exit /b


:cmd_remount_image
echo.
echo             Command: remount-image
echo.
echo         Description: Remounts a mounted image that has become inaccessible and makes it available for servicing
echo.
echo              Syntax: "dism /remount-image /mountdir:<path_to_mount_dir>"
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo                                      There are no parameters for this command
echo.
echo EXAMPLE:
echo.
echo     "DISM.exe /Remount-Image /MountDir:C:\test\offline"
echo.
exit /b


:cmd_split_ffu
echo.
echo             Command: split-ffu
echo.
echo         Description: Splits an existing Full Flash Utility (FFU) file into multiple read-only split (.sfu) files
echo.
echo              Syntax: "dism /split-ffu /imagefile:<path_to_image_file> /sfufile:<pattern> /filesize:<MB-size> (/checkintegrity)"
echo.
echo Split-FFU creates the .sfu files in the specified directory, naming each file as the specified "/SFUFile" but with an
echo appended number. For example, if you use "C:\flash.sfu", you will get a "flash.sfu" file, a "flash2.sfu" file, a
echo "flash3.sfu" file, and so on, defining each portion of the split .sfu file and saving it to the C:\ directory.
echo.
echo NOTES:
echo - For other types of files, see "split-image"
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /filesize                                                     Specify the maximum size in megabytes (MB) for each
echo                                                                  created file. If a single file is larger than the
echo                                                                  value specified in the "/FileSize" option, one of
echo                                                                  the split .sfu files that results will be larger
echo                                                                  than the value specified, in order to accommodate
echo                                                                  the larger file
echo.
echo    /checkintegrity                                               Detect and track .wim file corruption when being
echo                                                                  used with capture, unmount, export, and commit
echo                                                                  operations
echo                                                                  NOTE: if DISM detects corruption in the
echo                                                                        .wim file when used with apply or mount
echo                                                                        operations, /checkintegrity stops the
echo                                                                        operation
echo.
echo    /imagefile                                                    Specify the path of a .FFU file
echo.
echo    /sfufile:"<pattern>"                                          Reference split FFU files (SFUs). "<pattern>" is the
echo                                                                  naming pattern and location of split files
echo.
echo EXAMPLE:
echo.
echo     "DISM.exe /Split-FFU /ImageFile:flash.ffu /SFUFile:flash.ffu /FileSize:650"
echo.
exit /b


:cmd_split_image
echo.
echo             Command: split-image
echo.
echo         Description: Splits an existing .wim file into multiple read-only split (.swm) files
echo.
echo              Syntax: "dism /split-image /imagefile:<path_to_image_file> /swmfile:<pattern> /filesize:<MB-size> (/checkintegrity)"
echo.
echo Split-Image creates the .swm files in the specified directory, naming each file as the specified "/SWMFile" but with an
echo appended number. For example, if you use "C:\Data.swm", you will get a "Data.swm" file, a "Data2.swm" file, a
echo "Data3.swm" file, and so on, defining each portion of the split .swm file and saving it to the C:\ directory.
echo.
echo NOTES:
echo - For FFU files, see "split-ffu"
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /filesize                                                     Specify the maximum size in megabytes (MB) for each
echo                                                                  created file. If a single file is larger than the
echo                                                                  value specified in the "/FileSize" option, one of
echo                                                                  the split .swm files that results will be larger
echo                                                                  than the value specified, in order to accommodate
echo                                                                  the larger file
echo.
echo    /checkintegrity                                               Detect and track .wim file corruption when being
echo                                                                  used with capture, unmount, export, and commit
echo                                                                  operations
echo                                                                  NOTE: if DISM detects corruption in the
echo                                                                        .wim file when used with apply or mount
echo                                                                        operations, /checkintegrity stops the
echo                                                                        operation
echo.
echo    /imagefile                                                    Specify the path of a .WIM file
echo.
echo    /swmfile:"<pattern>"                                          Reference split WIM files (SWMs). "<pattern>" is the
echo                                                                  naming pattern and location of split files
echo.
echo EXAMPLE:
echo.
echo     "DISM.exe /Split-Image /ImageFile:install.wim /SWMFile:split.swm /FileSize:650"
echo.
exit /b


:cmd_unmount_image
echo.
echo             Command: unmount-image
echo.
echo         Description: Unmounts the .ffu, .wim or .vhd(x) file and either commits or discards the changes that were
echo                      made when the image was mounted
echo.
echo              Syntax: "dism /unmount-image /mountdir:<path_to_mount_dir> {/commit | /discard} (/checkintegrity) (/append)"
echo.
echo NOTES:
echo - You must use either the "/commit" or "/discard" argument when you use "/Unmount-Image"
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /checkintegrity                                               Detect and track .wim file corruption when being
echo                                                                  used with capture, unmount, export, and commit
echo                                                                  operations
echo                                                                  NOTE: if DISM detects corruption in the
echo                                                                        .wim file when used with apply or mount
echo                                                                        operations, /checkintegrity stops the
echo                                                                        operation
echo.
echo    /append                                                       Add the modified image to the existing .wim file
echo                                                                  instead of overwriting it.
echo                                                                  NOTE: both arguments do not apply to virtual hard
echo                                                                        disk (VHD(X)) files
echo.
echo EXAMPLES:
echo.
echo     "DISM.exe /Unmount-Image /MountDir:C:\test\offline /commit"
echo.
echo     "DISM.exe /Unmount-Image /MountDir:C:\test\offline /discard"
echo.
exit /b


:cmd_update_wimbootentry
echo.
echo             Command: update-wimbootentry
echo.
echo         Description: Updates the WIMBoot configuration entry, associated with the specified data source ID, with the
echo                      renamed image file or moved image file path
echo.
echo              Syntax: "dism /update-wimbootentry /path:<vol_path> /datasourceid:<data_source_id> /imagefile:<ren_img>"
echo.
echo NOTES:
echo - "/Update-WIMBootEntry" requires a restart in order for any updates to take effect
echo - This option is deprecated, and is not applicable to Windows 10 or newer
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /path                                                         Specify the disk volume of the WIMBoot configuration
echo.
echo    /datasourceid                                                 Specify the data source ID as displayed by
echo                                                                  "/get-wimbootentry"
echo.
echo EXAMPLE:
echo.
echo     "DISM.exe /Update-WIMBootEntry /Path:C:\ /DataSourceID:0 /ImageFile:R:\install.wim"
echo.
exit /b


:cmd_apply_siloedpackage
echo.
echo             Command: apply-siloedpackage
echo.
echo         Description: Applies one or more siloed provisioning packages (SPPs) to a specified image
echo.
echo              Syntax: "dism /apply-siloedpackage /packagepath:<pkg_path> /imagepath:<applied_img_path>"
echo.
echo NOTES:
echo - This option is only available after running "CopyDandI.cmd" from the Windows ADK, version 1607 or later;
echo   and running "dism.exe /apply-siloedpackage"
echo - "/PackagePath" can be used more than once in the same command to apply multiple SPPs in the specified order
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /packagepath                                                  Specify the path of a siloed provisioning package
echo                                                                  file
echo.
echo    /imagepath                                                    Specify the path of the Windows image where you
echo                                                                  are applying the SPP.
echo.
echo EXAMPLE:
echo.
echo     "DISM.exe /Apply-SiloedPackage /PackagePath:C:\test\word.spp /PackagePath:C:\test\spp2.spp /ImagePath:C:\"
echo.
exit /b


:cmd_global
echo.
echo  Syntax for offline: "dism /image:<path_to_offline_img_dir> (/windir:<path_to_%windir%) (/logpath:<path_to_log.log>)"
echo          operations  "(/loglevel:<n>) (/sysdrivedir:<path_to_bootMgr_file>) (/quiet) (/norestart)"
echo                      "(/scratchdir:<path_to_scratch_dir>) (/english) (/format:{table | list})"
echo.
echo   Syntax for online: "dism /online (/logpath:<path_to_log.log>) (/loglevel:<n>) (/sysdrivedir:<path_to_bootMgr_files>)"
echo          operations  "(/quiet) (/norestart) (/scratchdir:<path_to_scratch_dir>) (/english) (/format:{table | list})"
echo.
echo OPTIONS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /get-help; "?"                                                Display information about available DISM command-
echo                                                                  line options and arguments. On image management
echo                                                                  commands, use these arguments without specifying an
echo                                                                  image file (e.g., "/Mount-Image")
echo                                                                  EXAMPLE:
echo                                                                    "dism /?"
echo                                                                  Specify an image with the "/Image:<img>" option or
echo                                                                  use the "/Online" option to get help on the
echo                                                                  servicing command in the image, such as
echo                                                                  "/Get-Packages"
echo                                                                  EXAMPLES:
echo                                                                    "dism /image:C:\test\offline /?"
echo                                                                    "dism /online /?"
echo                                                                  You can display additional Help by specifying a
echo                                                                  command-line option.
echo                                                                  EXAMPLES:
echo                                                                    "dism /image:C:\test\offline /add-driver /?"
echo                                                                    "dism /image:C:\test\offline /add-package /?"
echo                                                                    "dism /online /get-drivers /?"
echo.
echo    /logpath:"<path_to_log.log>"                                  Specify the full path and file name to log to. If
echo                                                                  not set, the default is:
echo                                                                  "%WINDIR%\Logs\DISM\DISM.log"
:: Should be %WINDIR%, but alright; the Command Interpreter is kidding me
echo                                                                  When using a network share that is not joined to a
echo                                                                  domain, use the "net use" command together with
echo                                                                  domain credentials to set access permissions before
echo                                                                  you set the log path for the DISM log
echo                                                                  IMPORTANT: in Windows PE, the default directory is
echo                                                                             the RAMDISK scratch space which can be as
echo                                                                             low as 32 MB.
echo                                                                             The log file will automatically be
echo                                                                             archived, which will be saved with .bak
echo                                                                             appended to the file name and a new log
echo                                                                             file will be created. Each time it is
echo                                                                             archived, the .bak file will be
echo                                                                             overwritten.
echo                                                                  EXAMPLE:
echo                                                                    "dism /image:C:\test\offline"
echo                                                                    "/logpath:AddPackage.log /Add-Package"
echo                                                                    "/PackagePath:C:\packages\package.cab"
echo.
echo    /loglevel:"<n>"                                               Specify the maximum output level shown in the logs.
echo                                                                  The default log level is 3. The accepted values are
echo                                                                  as follows:
echo                                                                  1 = Errors only
echo                                                                  2 = Errors and warnings
echo                                                                  3 = Errors, warnings and information messages
echo                                                                  4 = Errors, warnings, information messages and debug
echo                                                                      output
echo                                                                  EXAMPLE:
echo                                                                    "dism /image:C:\test\offline"
echo                                                                    "/logpath:AddPackage.log /loglevel:1"
echo                                                                    "/add-package /packagepath:C:\packages\package.cab"
echo.
echo    /image:"<path_to_offline_img_dir>"                            This is the full path to the root directory of the
echo                                                                  offline Windows image you will service. If the
echo                                                                  directory named "Windows" is not a subdir of the
echo                                                                  root directory, "/WinDir" must be specified.
echo                                                                  NOTE: this option cannot be used with "/online"
echo                                                                  EXAMPLE:
echo                                                                    "dism /image:C:\test\offline"
echo                                                                    "/logpath:AddPackage.log /loglevel:1"
echo                                                                    "/add-package /packagepath:C:\packages\package.cab"
echo.
echo    /windir:"<path_to_windir>"                                    Used with the "/Image" option to specify the path to
echo                                                                  the Windows directory relative to the image path.
echo                                                                  This cannot be the full path to the Windows
echo                                                                  directory; it should be a relative path. If not
echo                                                                  specified, the default is the Windows directory in
echo                                                                  the root of the offline image directory.
echo                                                                  NOTE: this option cannot be used with "/Online"
echo                                                                  EXAMPLE:
echo                                                                    "dism /image:C:\test\offline /windir:WinNT"
echo                                                                    "/add-package /packagepath:C:\packages\package.cab"
echo.
echo    /online                                                       Specify that the action is to be taken on the
echo                                                                  currently running operating system.
echo                                                                  NOTE: this option cannot be used with "/Image" or
echo                                                                        "/WinDir". When "/Online" is used the Windows
echo                                                                        directory for the online image is automatically
echo                                                                        detected.
echo                                                                  EXAMPLE:
echo                                                                    "dism /online /get-packages"
echo.
echo    /sysdrivedir:"<path_to_sysdrive_directory>"                   Use "/sysdrivedir" to service an installed Windows
echo                                                                  image from a Windows PE environment.
echo                                                                  The "/sysdrivedir" option specifies the path to the
echo                                                                  location of the BootMgr files. This is necessary only
echo                                                                  when the BootMgr files are located on a partition
echo                                                                  other than the one you are running the command from.
echo                                                                  EXAMPLE (at a Windows PE command prompt)
echo                                                                    "dism /image:C:\Windows /sysdrivedir:C:"
echo.
echo    /quiet                                                        Turns off information and progress output to the
echo                                                                  console, and error messages will only be displayed.
echo                                                                  This option must be set every time the command-line
echo                                                                  utility is run.
echo                                                                  NOTE: do not use the "/quiet" option with "/get"
echo                                                                        commands. No information will be displayed.
echo                                                                  EXAMPLE:
echo                                                                    "dism /image:C:\test\offline /add-package"
echo                                                                    "/packagepath:C:\packages\package.cab /quiet"
echo.
echo    /norestart                                                    Suppress reboot. If it is not required, this option
echo                                                                  does nothing. It, however, will keep the application
echo                                                                  from prompting for a restart (or keep it from
echo                                                                  restarting automatically if the "/quiet" option
echo                                                                  is used)
echo                                                                  EXAMPLE:
echo                                                                    "dism /online /add-package"
echo                                                                    "/packagepath:C:\packages\package.cab /norestart"
echo                                                                    "/quiet"
echo.
echo    /scratchdir:"<path_to_scratch_dir>"                           Specify a temporary directory that will be used
echo                                                                  when extracting files for temporary use during
echo                                                                  servicing. The directory must exist locally. If not
echo                                                                  specified, the "\Windows\Temp" directory will be
echo                                                                  used, with a subdirectory name of randomly generated
echo                                                                  hexadecimal value for each run of DISM. Items in the
echo                                                                  scratch directory are deleted after each operation.
echo                                                                  You should not use a network share location as a
echo                                                                  scratch directory to expand a package (.cab or .msu
echo                                                                  file) for installation. The directory used for
echo                                                                  extracting files for temporary usage during servicing
echo                                                                  should be a local directory.
echo                                                                  EXAMPLE:
echo                                                                    "dism /image:C:\test\offline /scratchdir:C:\scratch"
echo                                                                    "/add-package /packagepath:C:\packages\package.cab"
echo.
echo    /english                                                      Display command-line output in English.
echo                                                                  NOTE: some resources cannot be displayed in English,
echo                                                                        and this option is not supported when you use
echo                                                                        the "/?" parameter
echo                                                                  EXAMPLE:
echo                                                                    "dism /get-imageinfo"
echo                                                                    "/imagefile:C:\test\offline\install.wim /index:1"
echo                                                                    "/english"
echo.
echo    /format:"{table | list}"                                      Specify the report output format.
echo                                                                  NOTE: some commands do not support outputting reports
echo                                                                        as tables
echo                                                                  EXAMPLE:
echo                                                                    "dism /image:C:\test\offline /get-apps"
echo                                                                    "/format:table"
echo.
exit /b


:cmd_get_packages
echo.
echo             Command: get-packages
echo.
echo         Description: Displays basic information about all packages in the image
echo.
echo              Syntax: "dism /get-packages (/format:{table | list})"
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo                                      There are no parameters for this command
echo.
echo EXAMPLES:
echo.
echo     "DISM.exe /Image:C:\test\offline /Get-Packages"
echo.
echo     "DISM.exe /Image:C:\test\offline /Get-Packages /Format:Table"
echo.
echo     "DISM.exe /Online /Get-Packages"
echo.
exit /b


:cmd_get_packageinfo
echo.
echo             Command: get-packageinfo
echo.
echo         Description: Displays detailed information about a package provided as a .cab file
echo.
echo              Syntax: "dism /get-packageinfo {/packagename:<name_in_img> | /packagepath:<path_to_pkg>}"
echo.
echo NOTES:
echo - Only .cab files can be specified and, as a result, you cannot obtain package information for .msu files
echo - You can also use "/PackagePath:<path_to_pkg>" to point to a folder
echo - The path to the .cab file should point to its original source
echo - For more basic information, see "get-packages"
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo                                      There are no parameters for this command
echo.
echo EXAMPLES:
echo.
echo     "DISM.exe /Image:C:\test\offline /Get-PackageInfo /PackagePath:C:\packages\package.cab"
echo.
echo     "DISM.exe /Image:C:\test\offline /Get-PackageInfo /PackageName:Microsoft.Windows.Calc.Demo~6595b6144ccf1df~x86~en~1.0.0.0"
echo.
exit /b


:cmd_add_package
echo.
echo             Command: add-package
echo.
echo         Description: Installs a specified .cab or .msu package in the image
echo.
echo              Syntax: "dism /add-package /packagepath:<path_to_file> (/ignorecheck) (/preventpending)"
echo.
echo Multiple packages can be added on one command line. The applicability of each package will be checked.
echo Unless the "/IgnoreCheck" argument is used, and if a package cannot be applied, an error message will
echo be displayed.
echo.
echo NOTES:
echo - "/PackagePath" can point to:
echo   - A single .cab or .msu file
echo   - A folder that contains a single expanded .cab file
echo   - A folder that contains a single .msu file
echo   - A folder that contains multiple .cab or .msu files
echo - If "/PackagePath" points to a folder containing .cab or .msu files at its root, its subdirectories will be checked
echo   recursively for files
echo - When "/PreventPending" is used, it skips the installation of a package if it or the Windows image has pending online
echo   actions
echo - "/Add-Package" does not perform dependency resolution or applicability checks. Make sure all dependencies of a
echo   package are installed before adding it
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo                                      There are no parameters for this command
echo.
echo EXAMPLES:
echo.
echo     "DISM.exe /Image:C:\test\offline /LogPath:AddPackage.log /Add-Package /PackagePath:C:\packages\package.msu"
echo.
echo     "DISM.exe /Image:C:\test\offline /Add-Package /PackagePath:C:\packages\package1.cab"
echo     "/PackagePath:C:\packages\package2.cab /IgnoreCheck"
echo.
echo     "DISM.exe /Image:C:\test\offline /Add-Package /PackagePath:C:\test\packages\package.cab /PreventPending"
echo.
exit /b


:cmd_remove_package
echo.
echo             Command: remove-package
echo.
echo         Description: Removes a specified .cab file package from the image
echo.
echo              Syntax: "dism /remove-package {/packagepath:<path_to_file> | /packagename:<name_in_img>}"
echo.
echo With "/Remove-Package", only .cab files can be specified. .msu files cannot be removed from an image
echo.
echo NOTES:
echo - "/PackagePath" needs to point to the original source of the package
echo - To find the name of the package, use the "/Get-Packages" command
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo                                      There are no parameters for this command
echo.
echo EXAMPLES:
echo.
echo     "DISM.exe /Image:C:\test\offline /LogPath:C:\Test\RemovePackage.log /Remove-Package /PackageName:Microsoft.Windows.Calc.Demo~6595b6144ccf1df~x86~en~1.0.0.0"
echo.
echo     "DISM.exe /Image:C:\test\offline /LogPath:C:\Test\RemovePackage.log /Remove-Package /PackageName:Microsoft.Windows.Calc.Demo~6595b6144ccf1df~x86~en~1.0.0.0 /PackageName:Microsoft-Windows-MediaPlayer-Package~31bf3856ad364e35~x86~~6.1.6801.0"
echo.
echo     "DISM.exe /Image:C:\test\offline /LogPath:C:\Test\RemovePackage.log /Remove-Package /PackagePath:C:\packages\package1.cab /PackagePath:C:\packages\package2.cab"
echo.
exit /b


:cmd_get_features
echo.
echo             Command: get-features
echo.
echo         Description: Displays basic information about all features in a package
echo.
echo              Syntax: "dism /get-features {/packagepath:<path_to_file> | /packagename:<name_in_img>} (/format:{table | list})"
echo.
echo NOTES:
echo - Features refer to OS components that include optional Windows foundation features
echo - "/Get-Features" finds the name of the packages in the image, or in the package's original source. If a package name
echo   or path is not specified, all features will be listed
echo - "/PackageName" is a package in an image. To see the package names in an image, see "get-packages"
echo - "/PackagePath" can point to either a .cab file or a folder
echo - You can display the output as a table or a list by using the "/Format" parameter
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo                                      There are no parameters for this command
echo.
echo EXAMPLES:
echo.
echo     "DISM.exe /Image:C:\test\offline /Get-Features"
echo.
echo     "DISM.exe /Image:C:\test\offline /Get-Features /Format:List"
echo.
echo     "DISM.exe /Image:C:\test\offline /Get-Features /PackageName:Microsoft.Windows.Calc.Demo~6595b6144ccf1df~x86~en~1.0.0.0"
echo.
echo     "DISM.exe /Image:C:\test\offline /Get-Features /PackagePath:C:\packages\package1.cab"
echo.
exit /b


:cmd_get_featureinfo
echo.
echo             Command: get-featureinfo
echo.
echo         Description: Displays detailed information about a feature
echo.
echo              Syntax: "dism /get-featureinfo /featurename:<name_in_img> ({/packagepath:<path_to_file> | /packagename:<name_in_img>})"
echo.
echo NOTES:
echo - Features refer to OS components that include optional Windows foundation features
echo - "/FeatureName" must be specified
echo - To find the name of a feature, or to view basic feature information, see "get-features"
echo - "/PackageName" and "/PackagePath" are optional and can be used to find a specific feature in a package
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo                                      There are no parameters for this command
echo.
echo EXAMPLES:
echo.
echo     "DISM.exe /Image:C:\test\offline /Get-FeatureInfo /FeatureName:Hearts"
echo.
echo     "DISM.exe /Image:C:\test\offline /Get-FeatureInfo /FeatureName:Hearts /PackagePath:C:\packages\package.cab"
echo.
exit /b


:cmd_enable_feature
echo.
echo             Command: enable-feature
echo.
echo         Description: Enables or updates the specified feature in the image
echo.
echo              Syntax: "dism /enable-feature /featurename:<name_in_img> (/packagename:<name_in_img>) (/source:<source>) (/limitaccess) (/all)"
echo.
echo NOTES:
echo - The "/FeatureName" option must be specified. Additionally, you can specify this option multiple times
echo - Unless the package is a Windows Foundation Package, you can specify the "/PackageName" option
echo - You can restore and enable a previously removed feature
echo - The source of the files can be:
echo   a) the Windows folder in a mounted image
echo   b) a Windows side-by-side (SxS) folder
echo - If multiple "/Source" arguments are specified, the files are gathered from the first location where they are found, and the rest of the locations are ignored
echo - If a "/Source" argument is not specified, DISM:
echo   a) will use the default location in the registry (for offline images)
echo   b) will use Windows Update (for online images)
echo - Use "/LimitAccess" to prevent DISM from contacting Windows Update (online images only)
echo - Use "/All" to enable all parent features of the specified feature
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo                                      There are no parameters for this command
echo.
echo EXAMPLES:
echo.
echo     "DISM.exe /Online /Enable-Feature /FeatureName:Hearts /All"
echo.
echo     "DISM.exe /Online /Enable-Feature /FeatureName:Calc /Source:c:\test\mount\Windows /LimitAccess"
echo.
echo     "Dism /Image:C:\test\offline /Enable-Feature /FeatureName:Calc /PackageName:Microsoft.Windows.Calc.Demo~6595b6144ccf1df~x86~en~1.0.0.0"
echo.
exit /b


:cmd_disable_feature
echo.
echo             Command: disable-feature
echo.
echo         Description: Disables the specified feature in the image
echo.
echo              Syntax: "dism /disable-feature /featurename:<name_in_img> (/packagename:<name_in_img>) (/remove)"
echo.
echo NOTES:
echo - You can specify the "/FeatureName" multiple times in one command line
echo - Unless the package is a Windows Foundation Package, use "/PackageName" to specify the parent package of the feature
echo - To remove a feature without removing its manifest from the image, use "/Remove". The feature will be listed as "Removed"
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo                                      There are no parameters for this command
echo.
echo EXAMPLES:
echo.
echo     "DISM.exe /Online /Disable-Feature /FeatureName:Hearts"
echo.
echo     "DISM.exe /Online /Disable-Feature /FeatureName:Calc /PackageName:Microsoft.Windows.Calc.Demo~6595b6144ccf1df~x86~en~1.0.0.0"
echo.
exit /b

:cmd_cleanup_image
echo.
echo             Command: cleanup-image
echo.
echo         Description: Performs cleanup or recovery operations on the image
echo.
echo              Syntax: "dism /cleanup-image [/revertpendingactions | /spsuperseded (/hidesp)] | /startcomponentcleanup"
echo                      " (/resetbase (/defer)) | /analyzecomponentstore | /checkhealth | /scanhealth | /restorehealth"
echo                      " (/source:<filepath>) (/limitaccess)"
echo.
echo NOTES:
echo - "/AnalyzeComponentStore" and "/ResetBase" can be used with Windows 8.1 and later, or Windows PE 5.0 or later images
echo - "/Defer" can be specified with "/ResetBase", starting with Windows 10, version 1607
echo.
echo ARGUMENTS:
echo.
echo    PARAMETER                                                     DESCRIPTION
echo   -------------------------------------------------------------------------------------------------------------------
echo    /revertpendingactions                                         If you experience a boot failure, this option can
echo                                                                  try to recover the system, by reverting all pending
echo                                                                  actions from the previous servicing operations if
echo                                                                  they are the culprit.
echo.
echo    /spsuperseded                                                 Removes any backup files created during the
echo                                                                  installation of a Service Pack. "/HideSP" lets you
echo                                                                  prevent the Service Pack from being listed in the
echo                                                                  Installed Updates Control Panel. Service Packs can't
echo                                                                  be uninstalled after this operation is completed.
echo.
echo    /startcomponentcleanup                                        Cleans up the superseded components and reduces the
echo                                                                  size of the component store. "/ResetBase" resets the
echo                                                                  base of superseded components, further reducing the
echo                                                                  component store size. Installed Windows updates
echo                                                                  can't be uninstalled after running this operation.
echo                                                                  "/Defer" can be used with "/ResetBase" to defer
echo                                                                  long-running cleanup operations to the next
echo                                                                  automatic maintenance.
echo.
echo    /analyzecomponentstore                                        Creates a report of the component store.
echo.
echo    /checkhealth                                                  Checks whether the image has been flagged as
echo                                                                  corrupted by a failed process and whether the
echo                                                                  corruption can be repaired.
echo.
echo    /scanhealth                                                   Scans the image for component store corruption. This
echo                                                                  operation will take several minutes.
echo.
echo    /restorehealth                                                Scans the image for component store corruption, and
echo                                                                  then performs repair operations automatically.
echo                                                                  - "/Source" lets you specify the location of good
echo                                                                    versions of files that can be used for the repair
echo                                                                  - "/LimitAccess" prevents DISM from contacting
echo                                                                    Windows Update for repair of online images
echo.
echo EXAMPLES:
echo.
echo     "Dism /Image=C:\test\offline /Cleanup-Image /RevertPendingActions"
echo.
echo     "Dism /Image=C:\test\offline /Cleanup-Image /SPSuperseded /HideSP"
echo.
echo     "Dism /Online /Cleanup-Image /ScanHealth"
echo.
echo     "Dism /Online /Cleanup-Image /RestoreHealth"
echo.
exit /b

:online_help
echo Accessing Microsoft online documentation...
echo To download the full documentation as a PDF file, click "Download PDF" on the bottom left.
start https://docs.microsoft.com/en-us/windows-hardware/manufacture/desktop/deployment-image-servicing-and-management--dism--command-line-options
exit /b
endlocal