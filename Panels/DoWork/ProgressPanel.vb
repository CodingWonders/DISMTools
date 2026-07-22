' DISMTools: operation numbers

' OperationNum          Action
' 00                    Create DISMTools project

' OperationNums for image management (.wim/.ffu/.vhd)
' ---------------------------------------------------
' 01                    Append-Image
' 02                    Apply-FFU
' 03                    Apply-Image
' 04                    Capture-CustomImage
' 05                    Capture-FFU
' 06                    Capture-Image
' 07                    Cleanup-Mountpoints
' 08                    Commit-Image
' 09                    Delete-Image
' 10                    Export-Image
' 11                    Get-ImageInfo
' 12                    Get-MountedImageInfo
' 13                    Get-WIMBootEntry
' 14                    List-Image
' 15                    Mount-Image
' 16                    Optimize-FFU
' 17                    Optimize-Image
' 18                    Remount-Image
' 19                    Split-FFU
' 20                    Split-Image
' 21                    Unmount-Image
' 22                    Update-WIMBootEntry
' 23                    Apply-SiloedPackage

' OperationNums for OS packages (.cab/.msu)
' -----------------------------------------
' 24                    Get-Packages
' 25                    Get-PackageInfo
' 26                    Add-Package
' 27                    Remove-Package
' 28                    Get-Features
' 29                    Get-FeatureInfo
' 30                    Enable-Feature
' 31                    Disable-Feature
' 32                    Cleanup-Image

' OperationNums for provisioning packages (.ppkg)
' -----------------------------------------------
' 33                    Add-ProvisioningPackage
' 34                    Get-ProvisioningPackageInfo
' 35                    Apply-CustomDataImage

' OperationNums for app package (.appx/.appxbundle) servicing
' -----------------------------------------------------------
' 36                    Get-ProvisionedAppxPackages
' 37                    Add-ProvisionedAppxPackage
' 38                    Remove-ProvisionedAppxPackage
' 39                    Optimize-ProvisionedAppxPackages
' 40                    Set-ProvisionedAppxDataFile

' OperationNums for application servicing (.msp)
' ----------------------------------------------
' 41                    Check-AppPatch
' 42                    Get-AppPatchInfo
' 43                    Get-AppPatches
' 44                    Get-AppInfo
' 45                    Get-Apps

' OperationNums for application association servicing
' ---------------------------------------------------
' 46                    Export-DefaultAppAssociations
' 47                    Get-DefaultAppAssociations
' 48                    Import-DefaultAppAssociations
' 49                    Remove-DefaultAppAssociations

' OperationNums for languages and international servicing
' -------------------------------------------------------
' 50                    Get-Intl                (also pass OperationNum 63)
' 51                    Set-UILang
' 52                    Set-UILangFallback
' 53                    Set-SysUILang
' 54                    Set-SysLocale
' 55                    Set-UserLocale
' 56                    Set-InputLocale
' 57                    Set-AllIntl
' 58                    Set-TimeZone
' 59                    Set-SKUIntlDefaults
' 60                    Set-LayeredDriver
' 61                    Gen-LangINI             (also pass OperationNum 63)
' 62                    Set-SetupUILang
' 63                    Distribution

' OperationNums for capabilities package servicing
' ------------------------------------------------
' 64                    Add-Capability
' 65                    Export-Source
' 66                    Get-Capabilities
' 67                    Get-CapabilityInfo
' 68                    Remove-Capability

' OperationNums for Windows Edition-Servicing
' -------------------------------------------
' 69                    Get-CurrentEdition
' 70                    Get-TargetEditions
' 71                    Set-Edition             (from lowest to highest)
' 72                    Set-ProductKey

' OperationNums for Driver Servicing (.inf)
' -----------------------------------------
' 73                    Get-Drivers
' 74                    Get-DriverInfo
' 75                    Add-Driver
' 76                    Remove-Driver           (should be used with care)
' 77                    Export-Driver
' 78                    Import-Driver

' OperationNums for unattended servicing
' --------------------------------------
' 79                    Apply-Unattend

' OperationNums for Windows PE servicing
' --------------------------------------
' 80                    Get-PESettings
' 81                    Get-ScratchSpace
' 82                    Get-TargetPath
' 83                    Set-ScratchSpace
' 84                    Set-TargetPath

' OperationNums for operating system uninstall
' --------------------------------------------
' 85                    Get-OSUninstallWindow
' 86                    Initiate-OSUninstall
' 87                    Remove-OSUninstall
' 88                    Set-OSUninstallWindow

' OperationNums for reserved storage
' ----------------------------------
' 89                    Set-ReservedStorageState
' 90                    Get-ReservedStorageState

' OperationNums for Microsoft Edge servicing
' ------------------------------------------
' 91                    Add-Edge
' 92                    Add-EdgeBrowser
' 93                    Add-EdgeWebView

' DISMTools reserved OperationNums
'---------------------------------
' 990                   LoadDTProj
' 991                   ConvertESD-WIM
' 992                   Merge-SWM
' 993                   Get-PkgNames
' 994                   Get-FeatureNamesAndStatus
' 995                   Get-Indexes
' 996                   Switch-Indexes
' 997                   Remount-ReadWrite
' 998                   Replace-FFU


Imports Microsoft.VisualBasic.ControlChars
Imports System.Threading
Imports System.IO
Imports System.Net
Imports System.Text.Encoding
Imports Microsoft.Dism
Imports System.Text.RegularExpressions
Imports DISMTools.Elements
Imports DISMTools.Utilities
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports DISMTools.Elements.Contemporaneus.ImageOperations

Public Class ProgressPanel

    Friend NotInheritable Class NativeMethods

        Public Sub New()
        End Sub

        <DllImport("user32.dll")>
        Public Shared Function SendMessage(hwnd As IntPtr, wMsg As UInteger, wParam As UInteger, lParam As IntPtr) As IntPtr
        End Function

    End Class

    Public taskCount As Long
    Dim currentTCont As Integer = 1
    Public OperationNum As Long

    Public IsSuccessful As Boolean
    Public IsDebugged As Boolean

    Public errCode As String

    Public CommandArgs As String = ""                       ' Ubiquitous across OperationNums. DO NOT DELETE !!!
    Public DismVersionChecker As FileVersionInfo
    Public DismProgram As String

    Dim DismExitCode As Integer

    Dim dateStr As String = "DISMTools-"

    Dim mntString As String = ""                            ' Mount directory, necessary for the DISM API

    Dim OnlineMgmt As Boolean                               ' Determine whether to perform actions to the active installation or the mounted Windows image

    Public TaskList As New List(Of Integer)                 ' Task list

    Dim AllDrivers As Boolean                               ' Detects whether the program should detect all image drivers, taken from MainForm

    Public SystemEditor As String                           ' System Editor to launch for logs. Backup file is provided below, in case the specified editor doesn't exist
    Dim SystemEditorBackup As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "notepad.exe")

    Dim ImgVersion As Version

    Private PreventSystemFromSleeping As Boolean

    ' Initial settings
    Dim DismExe As String
    Dim AutoLogs As Boolean
    Dim LogPath As String
    Dim LogLevel As Integer
    Dim QuietOps As Boolean

    Dim SkipSysRestart As Boolean
    Dim UseScratchDir As Boolean
    Dim AutoScratch As Boolean
    Dim ScratchDirPath As String
    Dim EnglishOut As Boolean
    ' Backup command arguments
    Dim BckArgs As String

    Dim IsExpanded As Boolean
    Private CancelButtonClosesDialog As Boolean


    ' OperationNum: 0
    Public projName As String
    Public projPath As String
    Public MountAfterCreation As Boolean

    ' OperationNum: 1
    Public AppendixSourceDir As String                      ' Source directory containing the image to append
    Public AppendixDestinationImage As String               ' The destination image to append to
    Public AppendixName As String                           ' Appended image name
    Public AppendixDescription As String                    ' Appended image description
    Public AppendixWimScriptConfig As String                ' Path for WimScript.ini (configuration list file)
    Public AppendixUseWimBoot As Boolean                    ' Determine whether to append the image with WIMBoot configuration
    Public AppendixBootable As Boolean                      ' Determine whether to make target image bootable (Windows PE only)
    Public AppendixCheckIntegrity As Boolean                ' Determine whether to check integrity of the WIM file
    Public AppendixVerify As Boolean                        ' Determine whether to check for errors and file duplication
    Public AppendixReparsePt As Boolean                     ' Determine whether to use the reparse point tag fix
    Public AppendixCaptureExtendedAttribs As Boolean        ' Determine whether to capture extended attributes

    ' OperationNum: 2
    Public FFUApplicationSourceImg As String                ' String which determines which image to apply
    Public FFUApplicationDestDrive As String                ' Gather destination disk ID
    Public FFUApplicationSFUPattern As String               ' Spanned/Split WIM (SWM) file pattern string. Usually "install*.sfu", so don't use an array

    ' OperationNum: 3
    Public ApplicationSourceImg As String                   ' String which determines which image to apply
    Public ApplicationIndex As Integer                      ' Index to apply to destination
    Public ApplicationDestDir As String                     ' Destination directory to apply image to
    Public ApplicationCheckInt As Boolean                   ' Determine whether to check image corruption before applying
    Public ApplicationVerify As Boolean                     ' Determine whether to check for file duplication and errors
    Public ApplicationReparsePt As Boolean                  ' Determine whether to use reparse points
    Public ApplicationSWMPattern As String                  ' Spanned/Split WIM (SWM) file pattern string. Usually "install*.swm", so don't use an array
    Public ApplicationValidateForTD As Boolean              ' Determine whether to validate image for Trusted Desktop (WinPE 4.0+ only)
    Public ApplicationUseWimBoot As Boolean                 ' Determine whether to append image with WIMBoot configuration
    Public ApplicationCompactMode As Boolean                ' Determine whether to apply image in Compact mode (Win10+ only)
    Public ApplicationUseExtAttr As Boolean                 ' Determine whether to apply extended attributes (Win10 1607+ only)

    ' OperationNum: 5
    Public FFUCaptureSourceDrive As String                  ' Source drive to be captured
    Public FFUCaptureDestinationFfuImage As String          ' Destination FFU image
    Public FFUCaptureName As String                         ' Captured FFU name
    Public FFUCaptureDescription As String                  ' Captured FFU description (optional)
    Public FFUCaptureCompressType As Integer                ' Compression used for the capture (0: none; 1: default)

    ' OperationNum: 6
    Public CaptureSourceDir As String                       ' Source directory to be captured
    Public CaptureDestinationImage As String                ' Destination image
    Public CaptureName As String                            ' Captured image name
    Public CaptureDescription As String                     ' Captured image description (optional)
    Public CaptureWimScriptConfig As String                 ' Path for WimScript.ini
    Public CaptureCompressType As Integer                   ' Compression used for the capture (0: none; 1: fast; 2: max)
    Public CaptureBootable As Boolean                       ' Make captured image bootable (WinPE only)
    Public CaptureCheckIntegrity As Boolean                 ' Check integrity of WIM file
    Public CaptureVerify As Boolean                         ' Check for errors and file duplication
    Public CaptureReparsePt As Boolean                      ' Determine whether to use the reparse point tag fix
    Public CaptureUseWimBoot As Boolean                     ' Determine whether to append image with WIMBoot configuration
    Public CaptureExtendedAttributes As Boolean             ' Determine whether to capture extended attributes (Win10 1607+ only)
    Public CaptureMountDestImg As Boolean                   ' Determine whether to unmount the source VHD(X) file and mount the destination image (still experimental)

    ' OperationNum: 9
    Public imgIndexDeletionNames(65535) As String           ' Remove volume images by name (it can be a bit confusing by index number. Index 6: 1, 1, 1, 1, 1, 2, 2, 2, 2...)
    Public imgIndexDeletionSourceImg As String              ' Source image to remove volume images from
    Public imgIndexDeletionIntCheck As Boolean              ' Determine whether to check image integrity before removing volume images
    Public imgIndexDeletionUnmount As Boolean               ' Determine whether to unmount source image if it is mounted
    Public imgIndexDeletionLastName As String               ' Last name of index checked
    Public imgIndexDeletionCount As Integer                 ' Volume image removal count

    ' OperationNum: 10
    Public imgExportSourceImage As String                   ' The source image to export
    Public imgExportSourceIndex As Integer                  ' The source index to export
    Public imgExportDestinationImage As String              ' The export target
    Public imgExportDestinationUseCustomName As Boolean     ' Determine whether to use a custom destination name
    Public imgExportDestinationName As String               ' The custom destination name
    Public imgExportCompressType As Integer                 ' Compression used for the export (0: none; 1: fast; 2: max; 3: recovery)
    Public imgExportMarkBootable As Boolean                 ' Determine whether to mark the target image as bootable (Windows PE only)
    Public imgExportUseWimBoot As Boolean                   ' Determine whether to append the target image with WIMBoot configurations
    Public imgExportCheckIntegrity As Boolean               ' Determine whether to check the integrity of the image before exporting it

    ' OperationNum: 11
    Public GetFromMountedImg As Boolean                     ' Get information from mounted image
    Public GetSpecificIndexInfo As Boolean                  ' Get information from specific image index
    Public GetFromMountedIndex As Boolean                   ' Get information from mounted image index
    Public InfoFromSourceImg As String                      ' Source image information string
    Public InfoFromSpecificImg As String                    ' Specific image information string
    Public InfoFromSourceIndex As Integer                   ' Source image index information int
    Public InfoFromSpecificIndex As Integer                 ' Specific image index information int

    ' OperationNum: 15
    Public SourceImg As String                              ' Mandatory
    Public ImgIndex As Integer                              ' Mandatory
    Public MountDir As String                               ' Mandatory
    Public isReadOnly As Boolean                            ' Determine whether image will be mounted with read-only permissions
    Public isOptimized As Boolean                           ' Determine whether image will be optimized to mount in a shorter time
    Public isIntegrityTested As Boolean                     ' Determine whether the integrity of the image should be tested before mounting the image

    ' OperationNum: 16
    Public FFUOptimizationSource As String                  ' Source image file to optimize
    Public FFUOptimizationCustomPartitionNum As Integer     ' The number of the partition to optimize. If set to 0, the default one will be used

    ' OperationNum: 17
    Public OptimizationSource As String                     ' Source image file to optimize
    Public OptimizationMode As Integer                      ' The mode with which the image must be optimized (0: boot; 1: wimboot)

    ' OperationNum: 18
    Public remountisReadOnly As Boolean                     ' Determine whether the remount happened because of a read-only mounted image

    ' OperationNum: 19
    Public SFUSplitSourceFile As String                     ' Source image file to be split into SFU files
    Public SFUSplitFileSize As Integer                      ' The maximum size in MB for each created image
    Public SFUSplitTargetFile As String                     ' The path of the SFU files
    Public SFUSplitCheckIntegrity As Boolean                ' Checks the integrity of the source image before splitting it

    ' OperationNum: 20
    Public SWMSplitSourceFile As String                     ' Source image file to be split into SWM files
    Public SWMSplitFileSize As Integer                      ' The maximum size in MB for each created image
    Public SWMSplitTargetFile As String                     ' The path of the SWM files
    Public SWMSplitCheckIntegrity As Boolean                ' Checks the integrity of the source image before splitting it

    ' OperationNum: 21
    Public UMountImgIndex As Integer
    Public UMountLocalDir As Boolean
    Public UMountOp As Integer                              ' 0: commit, then unmount; 1: unmount without saving
    Public RandomMountDir As String                         ' Don't know about that mount dir, other that it was not loaded
    Public CheckImgIntegrity As Boolean
    Public SaveToNewIndex As Boolean

    ' OperationNum: 26
    Public pkgSource As String                              ' Determine where the packages came from
    Dim pkgName As String                                   ' Determine how the package is called
    Dim pkgDesc As String                                   ' Determine package description (e.g., "Fix for KB5014113")
    Dim pkgApplicabilityStatus As String                    ' Determine whether or not package is applicable
    Dim pkgInstallationState As String                      ' Determine whether or not package was installed
    Public pkgs(65535) As String                            ' Array used to determine package locations. DO NOT DELETE !!!
    Public pkgLastCheckedPackageName As String              ' Last index name of the aforementioned array. DO NOT DELETE !!!
    Public pkgIsApplicable As Boolean                       ' Using data from pkgApplicabilityStatus, determine whether package is applicable
    Public pkgIsAlreadyAdded As Boolean                     ' Using data from pkgInstallationState, determine whether package is installed
    Public pkgIgnoreApplicabilityChecks As Boolean          ' If option is checked, ignore applicability checks
    Public pkgPreventIfPendingOnline As Boolean             ' If option is checked, ignore package if online actions are required on the image
    Public pkgAdditionCommit As Boolean                     ' If option is checked, commit image after operations are done
    Public pkgAdditionOp As Integer                         ' 0: recursive operation; 1: selective operation; 2: Microsoft Update Manifest operation
    Public pkgCount As Integer                              ' Gather package count
    Public pkgCurrentNum As Integer                         ' Current package number
    Public pkgSuccessfulAdditions As Integer                ' Determine successful package additions
    Public pkgFailedAdditions As Integer                    ' Determine failed package additions

    ' OperationNum: 27
    Public pkgRemovalSource As String                       ' Set this variable if a removal source is used
    Public pkgRemovalNames(65535) As String                 ' Array used to determine package names for removal
    Public pkgRemovalFiles(65535) As String                 ' Array used to determine package files for removal
    Public pkgIsReadyForRemoval As Boolean                  ' Determine whether package is ready for removal (whether package is added or not)
    Public pkgSuccessfulRemovals As Integer                 ' Determine successful package removals
    Public pkgFailedRemovals As Integer                     ' Determine failed package removals
    Public pkgRemovalOp As Integer                          ' 0: package names; 1: package files
    Public pkgRemovalLastName As String                     ' Last package name checked
    Public pkgRemovalLastFile As String                     ' Last package file checked
    Public pkgRemovalCount As Integer                       ' Selected package {name | file} count
    Public pkgRemovalState As String                        ' State the package is at
    Public pkgRemovalName As String                         ' Name of package to be removed

    ' OperationNum: 30
    Public featEnablementNames(65535) As String             ' Array used to determine which features need to be enabled
    Public featEnablementLastName As String                 ' Last feature entry checked
    Public featisParentPkgNameUsed As Boolean               ' Determine whether to specify the parent package name for the features
    Public featParentPkgName As String                      ' Parent package name to use when enabling features
    Public featisSourceSpecified As Boolean                 ' Determine whether to use a feature source
    Public featSource As String                             ' Feature source
    Public featParentIsEnabled As Boolean                   ' Determine whether all parent features need to be enabled
    Public featContactWindowsUpdate As Boolean              ' Determine whether to contact Windows Update (WU) for online images
    Public featEnablementCommit As Boolean                  ' Determine whether to commit image after enabling features
    Public featEnablementCount As Integer                   ' Count number of features to enable
    Public featCanContactWU As Boolean                      ' Determine whether program can contact Windows Update
    Dim featSuccessfulEnablements As Integer                ' Successful feature enablement count
    Dim featFailedEnablements As Integer                    ' Failed feature enablement count

    ' OperationNum: 31
    Public featDisablementNames(65535) As String            ' Array used to determine which features need to be disabled
    Public featDisablementLastName As String                ' Last feature entry checked
    Public featDisablementParentPkgUsed As Boolean          ' Determine whether to specify the parent package name for the features
    Public featDisablementParentPkg As String               ' Parent package name to use when disabling features
    Public featDisablementRemoveManifest As Boolean         ' Remove feature without removing manifest
    Public featDisablementCount As Integer                  ' Count number of features to disable
    Dim featSuccessfulDisablements As Integer               ' Successful feature disablement count
    Dim featFailedDisablements As Integer                   ' Failed feature disablement count

    ' OperationNum: 32
    Public CleanupTask As Integer                           ' The task that will be performed on component cleanup, ranging from 0 to 6
    ' CleanupTask = 1
    Public CleanupHideSP As Boolean                         ' Determines whether to hide Service Pack installations from the Installed Updates list
    ' CleanupTask = 2
    Public ResetCompBase As Boolean                         ' Determines whether to perform a component base reset
    Public DeferCleanupOps As Boolean                       ' Determines whether to defer long-running cleanup operations (those that take more than 30 mins)
    ' CleanupTask = 6
    Public UseCompRepairSource As Boolean                   ' Determines whether to use a custom component store repair source
    Public ComponentRepairSource As String                  ' A custom source that will be used for component store repair
    Public LimitWUAccess As Boolean                         ' Determines whether to limit access to Windows Update and strictly use the custom source (only for online images)

    ' OperationNum: 33
    Public ppkgAdditionPackagePath As String                ' The path of the provisioning package to add
    Public ppkgAdditionCatalogPath As String                ' The path of the catalog file to add
    Public ppkgAdditionCommit As Boolean                    ' Determines whether to commit the image after adding the provisioning package

    ' OperationNum: 37
    Public appxAdditionPackages(65535) As String            ' Array used to store AppX packages to add
    Public appxAdditionDependencies(65535) As String        ' Array used to store dependencies of AppX packages
    Public appxAdditionUseLicenseFile As Boolean            ' Determine whether to use a license file
    Public appxAdditionLicenseFile As String                ' License file to use on AppX packages (program limitation: it uses the same license on all AppX packages)
    Public appxAdditionUseCustomDataFile As Boolean         ' Determine whether to use a custom data file for AppX provisioning
    Public appxAdditionCustomDataFile As String             ' Custom data file applied on AppX packages
    Public appxAdditionUseAllRegions As Boolean             ' Determine whether to use all regions for all AppX packages
    Public appxAdditionRegions As String                    ' Regions to apply on AppX packages
    Public appxAdditionPackageList As New List(Of AppxPackage)

    Public appxAdditionCommit As Boolean                    ' Determine whether to commit the image after adding AppX packages
    Public appxAdditionCount As Integer                     ' Count number of AppX packages to add
    Public appxAdditionLastPackage As String                ' Last package entry selected
    Public appxAdditionLastDependency As String             ' Last dependency entry
    Dim appxSuccessfulAdditions As Integer                  ' Successful AppX package addition count
    Dim appxFailedAdditions As Integer                      ' Failed AppX package addition count

    ' OperationNum: 38
    Public appxRemovalPackages(65535) As String             ' Array used to store AppX packages to remove
    Public appxRemovalPkgNames(65535) As String             ' Array used to store AppX friendly names
    Public appxRemovalLastPackage As String                 ' Last package entry selected
    Public appxRemovalCount As Integer                      ' Count number of AppX packages to remove
    Dim appxSuccessfulRemovals As Integer                   ' Successful AppX package removal count
    Dim appxFailedRemovals As Integer                       ' Failed AppX package addition count

    ' OperationNum: 60
    Dim currentLay As KeyboardDrivers.LayeredKeyboardDriver ' Current keyboard layered driver
    Dim newKeybLay As KeyboardDrivers.LayeredKeyboardDriver ' New keyboard layered driver
    Public currentKeybLayeredDriverType As Integer          ' Integer that defines the current keyboard layered driver
    Public KeyboardLayeredDriverType As Integer             ' Integer that defines the keyboard layered driver to set

    ' OperationNum: 64
    Public capAdditionIds(65535) As String                  ' Array used to store IDs of capabilities to add
    Public capAdditionLastId As String                      ' Last capability ID selected
    Public capAdditionUseSource As Boolean                  ' Determine whether to use a custom source for capability addition
    Public capAdditionSource As String                      ' Capability addition source
    Public capAdditionCount As Integer                      ' Total number of capabilities to add
    Public capAdditionLimitWUAccess As Boolean              ' Determine whether to limit access to Windows Update and stick to the source specified (online images only)
    Public capAdditionCommit As Boolean                     ' Determine whether to commit image after adding capabilities
    Public capSuccessfulAdditions As Integer                ' Number of successful capability additions
    Public capFailedAdditions As Integer                    ' Number of failed capability additions

    ' OperationNum: 68
    Public capRemovalIds(65535) As String                   ' Array used to store IDs of capabilities to remove
    Public capRemovalLastId As String                       ' Last capability ID selected for removal
    Public capRemovalCount As Integer                       ' Total number of capabilities to remove
    Public capSuccessfulRemovals As Integer                 ' Number of successful capability removals
    Public capFailedRemovals As Integer                     ' Number of failed capability removals

    ' OperationNum: 71
    Public imgEditionNewEdition As String                   ' The edition to upgrade the image to
    Public imgEditionCopyEula As Boolean                    ' Determines whether or not to copy the end-user license agreement to a destination (Windows Server installations only)
    Public imgEditionEulaDestination As String              ' The destination to copy the EULA to
    Public imgEditionAcceptEula As Boolean                  ' Determines whether to accept the end-user license agreement (Windows Server installations only)
    Public imgEditionEditionKey As String                   ' The product key with which the EULA will be accepted

    ' OperationNum: 72
    Public pkSetNewProductKey As String                     ' The new product key to set in the Windows image or installation

    ' OperationNum: 75
    Public drvAdditionPkgs(65535) As String                 ' Array used to store all drivers to add, whether they are in specified folders or not
    Public drvAdditionLastPkg As String                     ' Last driver package specified for addition
    Public drvAdditionFolderRecursiveScan(65535) As String  ' Folders the program needs to scan recursively on
    Public drvAdditionCount As Integer                      ' Total number of driver packages to add
    Public drvAdditionForceUnsigned As Boolean              ' Determine whether to add unsigned drivers on 64-bit images
    Public drvAdditionCommit As Boolean                     ' Determine whether to save image changes after adding driver packages
    Public drvSuccessfulAdditions As Integer                ' Number of successful driver package additions
    Public drvFailedAdditions As Integer                    ' Number of failed driver package additions

    ' OperationNum: 76
    Public drvRemovalPkgs(65535) As String                  ' Array used to store all drivers to remove
    Public drvRemovalLastPkg As String                      ' Last driver package specified for removal
    Public drvRemovalCount As Integer                       ' Total number of driver packages to remove
    Public drvSuccessfulRemovals As Integer                 ' Number of successful driver package removals
    Public drvFailedRemovals As Integer                     ' Number of failed driver package removals
    Dim drvCollection As DismDriverPackageCollection        ' Collection of image drivers for driver package removal

    ' OperationNum: 77
    Public drvExportTarget As String                        ' Path the drivers will be exported to
    Public drvExportAllDrvs As Boolean                      ' Determines whether to export all drivers, or drivers based on the class name
    Public drvExportSpecificClassName As String             ' The class name that the drivers to export have set
    Public drvExportWin7Mode As Boolean                     ' Run driver exports in Windows 7 mode

    ' OperationNum: 78
    Public ImportSourceInt As Integer                       ' The import source
    ' ImportSourceInt = 0
    Public DrvImport_SourceImage As String                  ' The mounted image that will act as the source for the driver import
    ' ImportSourceInt = 2
    Public DrvImport_SourceDisk As String                   ' The disk drive that will act as the source for the driver import

    ' OperationNum: 79
    Public UnattendedFile As String                         ' The path of the unattended answer file
    Public UnattendedCopyToSysprep As Boolean               ' Determines whether to copy the unattended answer file to Sysprep

    ' OperationNum: 83
    Public peNewScratchSpace As Integer                     ' New scratch space amount to apply to the Windows PE image

    ' OperationNum: 84
    Public peNewTargetPath As String                        ' New target path to apply to the Windows PE image

    ' <Space for other OperationNums>
    ' OperationNum: 88
    Public osUninstDayCount As Integer                      ' Number of days the user has to uninstall an OS upgrade

    ' OperationNum: 991
    Public imgSrcFile As String                             ' Source image file for conversion
    Public imgConversionIndex As Integer                    ' Index to convert to the target image format
    Public imgDestFile As String                            ' Destination image file for conversion
    Public imgConversionMode As Integer                     ' 0: WIM -> ESD; 1: WIM <- ESD

    ' OperationNum: 992
    Public imgSwmSource As String                           ' Source SWM file to merge its pattern to WIM
    Public imgMergerIndex As Integer                        ' Index of the SWM file of which to export to the merged WIM file
    Public imgWimDestination As String                      ' Destination WIM file to merge SWM files to

    ' OperationNum: 996
    Public SwitchTarget As String                           ' Target to switch indexes from
    Public SwitchSourceIndex As Integer                     ' Source image index
    Public SwitchTargetIndex As Integer                     ' Target image index
    Public SwitchTargetIndexName As String                  ' Target index name
    Public SwitchCommitSourceIndex As Boolean               ' Determine whether to commit source index
    Public SwitchMountAsReadOnly As Boolean                 ' Determine whether to mount target index with read-only permissions
    Public SwitchSourceImg As String                        ' Source image

    ' OperationNum: 997
    Public RWRemountSourceImg As String                     ' Source image to remount with R/W permissions

    ' OperationNum: 998
    Public FFUReplaceSourceFFU As String                    ' Path to source FFU file that will act as a replacement of the destination
    Public FFUReplaceDestinationFFU As String               ' Path to destination FFU file that will be replaced by the source FFU

    ' Miscellaneous error variables
    Dim PackageErrorCodes As New List(Of String)
    Dim FeatureErrorCodes As New List(Of String)

    ' Contemporaneus WAVE 2
    Private EnableExperiments As Boolean

    Private ImageOperationDefinitions As New Dictionary(Of Integer, ImageOperation) From {
        {15, New MountImageIO(Function(filePath, args) DISM_LogView.StartProcess(filePath, args))}
    }

    ' --- Event handlers
    Private Event AllTasksLogReported(AllTasksMessage As String)
    Private Event CurrTaskLogReported(CurrTaskMessage As String)
    Private Event LogActivityReported(LogMessage As String)

    Private ReferenceImage As WindowsImage


    Private Function ProgressLogText(itemKey As String) As String
        Return LocalizationService.ForSection("Progress.LogText")(itemKey)
    End Function

    Private Sub OnAllTasksLogReported(AllTasksMessage As String) Handles Me.AllTasksLogReported
        allTasks.Text = AllTasksMessage
    End Sub

    Private Sub OnCurrTaskLogReported(CurrTaskMessage As String) Handles Me.CurrTaskLogReported
        currentTask.Text = CurrTaskMessage
    End Sub

    Private Sub OnLogActivityReported(LogMessage As String) Handles Me.LogActivityReported
        LogView.AppendText(LogMessage)
    End Sub

    Private Sub ReportAllTasks(AllTasksMessage As String)
        RaiseEvent AllTasksLogReported(AllTasksMessage)
    End Sub

    Private Sub ReportCurrTask(CurrTaskMessage As String)
        RaiseEvent CurrTaskLogReported(CurrTaskMessage)
    End Sub

    Private Sub ReportLogActivity(LogMessage As String)
        RaiseEvent LogActivityReported(LogMessage)
    End Sub

    Private Sub PrepareAllReporters()
        For Each OperationKey In ImageOperationDefinitions.Keys
            ImageOperationDefinitions(OperationKey).LogCurrTaskReporter = Sub(CurrTaskMessage As String)
                                                                              ReportCurrTask(CurrTaskMessage)
                                                                          End Sub
            ImageOperationDefinitions(OperationKey).LogAllTasksReporter = Sub(AllTasksMessage As String)
                                                                              ReportAllTasks(AllTasksMessage)
                                                                          End Sub
            ImageOperationDefinitions(OperationKey).LogActivityReporter = Sub(LogMessage As String)
                                                                              ReportLogActivity(LogMessage)
                                                                          End Sub
        Next
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        If CancelButtonClosesDialog Then
            Close()
            Return
        End If

        ProgressBW.CancelAsync()
    End Sub

    Private Sub LogButton_Click(sender As Object, e As EventArgs) Handles LogButton.Click
        Dim collapsedHeight As Integer = WindowHelper.ScaleLogical(240)
        Dim expandedHeight As Integer = WindowHelper.ScaleLogical(420)
        If Not IsExpanded Then
            LogButton.Text = LocalizationService.ForSection("Progress.Log")("HideLog.Label")
            Height = expandedHeight
        Else
            LogButton.Text = LocalizationService.ForSection("Progress.Log")("ShowLog.Item")
            Height = collapsedHeight
        End If
        IsExpanded = Not IsExpanded
        BodyPanel.Refresh()
        CenterToParent()
    End Sub

    Sub GetTasks(opNum As Integer)
        DynaLog.LogMessage("Getting number of tasks...")
        DynaLog.LogMessage("Operation number: " & opNum)
        If opNum = 6 Then
            If CaptureMountDestImg Then
                taskCount = 3
            Else
                taskCount = 1
            End If
        ElseIf opNum = 9 Then
            If imgIndexDeletionUnmount Then
                taskCount = 2
            Else
                taskCount = 1
            End If
        ElseIf opNum = 26 Then
            If pkgAdditionCommit Then
                taskCount = 2
            Else
                taskCount = 1
            End If
        ElseIf opNum = 30 Then
            If featEnablementCommit Then
                taskCount = 2
            Else
                taskCount = 1
            End If
        ElseIf opNum = 33 Then
            If ppkgAdditionCommit Then
                taskCount = 2
            Else
                taskCount = 1
            End If
        ElseIf opNum = 37 Then
            If appxAdditionCommit Then
                taskCount = 2
            Else
                taskCount = 1
            End If
        ElseIf opNum = 64 Then
            If capAdditionCommit Then
                taskCount = 2
            Else
                taskCount = 1
            End If
        ElseIf opNum = 75 Then
            If drvAdditionCommit Then
                taskCount = 2
            Else
                taskCount = 1
            End If
        ElseIf opNum = 996 Then
            taskCount = 2
        Else
            taskCount = 1
        End If
        DynaLog.LogMessage("Number of tasks: " & taskCount)
        AllPB.Maximum = taskCount * 100
        taskCountLbl.Text = LocalizationService.ForSection("Progress.GetTasks").Format("Tasks.Label", taskCount)
        CenterToParent()
    End Sub

    ''' <summary>
    ''' Gathers the initial list of settings to use for DISM
    ''' </summary>
    ''' <remarks>These settings can be configured at any time using the Options dialog</remarks>
    Sub GatherInitialSwitches()
        DynaLog.LogMessage("Getting initial set of switches for DISM...")
        CommandArgs = "/logpath=" & Quote & If(AutoLogs, Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now), LogPath) & Quote & " /loglevel=" & LogLevel & If(UseScratchDir, If(AutoScratch, If(OnlineMgmt, " /scratchdir=" & Quote & Application.StartupPath & "\scratch" & Quote, " /scratchdir=" & Quote & projPath & "\scr_temp"), If(ScratchDirPath <> "", " /scratchdir=" & Quote & ScratchDirPath & Quote, "")), "") & If(EnglishOut, " /english", "")
        DynaLog.LogMessage("Initial switches: " & CommandArgs)
        BckArgs = CommandArgs
    End Sub

    ''' <summary>
    ''' Sets the name of the log file using the current date and time
    ''' </summary>
    ''' <param name="CurrentDate">The date to add. It is always "Now"</param>
    ''' <returns>This function returns a file name that can be used in log files, file-system friendly on both Unix and Windows</returns>
    ''' <remarks></remarks>
    Function GetCurrentDateAndTime(CurrentDate As Date) As String
        DynaLog.LogMessage("Getting a suitable name for log files with current date...")
        DynaLog.LogMessage("Current date: " & CurrentDate.ToString())
        dateStr = "DISMTools-" & CurrentDate.ToString()
        ' Make sure the file with the name is file-system friendly
        If dateStr.Contains("/") Or dateStr.Contains(":") Then
            dateStr = dateStr.Replace("/", "-").Trim().Replace(":", "-").Trim()
        End If
        dateStr &= ".log"
        Return dateStr
    End Function

    Sub RunTaskList(taskList As List(Of Integer))
        DynaLog.LogMessage("Running items in task list...")
        DynaLog.LogMessage("- Items in task list: " & taskList.Count)
        Dim successfulTasks As Integer = 0
        Dim failedTasks As Integer = 0
        Dim prevValue As Integer = 0
        For Each Task In taskList
            DynaLog.LogMessage("Running task " & taskList.IndexOf(Task) + 1 & " of " & taskList.Count & " (operation number " & Task & ")...")
            RunOps(Task)
            AllPB.Value = prevValue + (AllPB.Maximum / taskList.Count)
            prevValue = AllPB.Value
            If Not currentTCont = taskList.Count Then currentTCont += 1
            taskCountLbl.Text = LocalizationService.ForSection("Progress").Format("Tasks.Label", currentTCont, taskList.Count)
            DynaLog.LogMessage("Determining if tasks are successful...")
            If IsSuccessful Then successfulTasks += 1 Else failedTasks += 1
        Next
        DynaLog.LogMessage("Task summary:")
        DynaLog.LogMessage("- Tasks that succeeded: " & successfulTasks)
        DynaLog.LogMessage("- Tasks that failed: " & failedTasks)
        DynaLog.LogMessage("Are overall tasks successful? " & If(successfulTasks >= failedTasks, "Yes", "No"))
        IsSuccessful = (successfulTasks >= failedTasks)
    End Sub

    ''' <summary>
    ''' Runs the specified process and returns an exit code
    ''' </summary>
    ''' <param name="FilePath">The path of the file to run</param>
    ''' <param name="CommandArguments">The command-line arguments to pass to the file to run</param>
    ''' <param name="WorkingDirectory">The directory the file is in. This is optional and can be set to fix issues with the file to open</param>
    ''' <param name="DoNotRedirect">Determines whether to redirect output to console text area</param>
    ''' <remarks>Any logging is done with DynaLog</remarks>
    Sub RunProcess(FilePath As String, CommandArguments As String, Optional WorkingDirectory As String = "", Optional DoNotRedirect As Boolean = False)
        Try
            DynaLog.LogMessage("Preparing to run process...")
            DynaLog.LogMessage("- Process path: " & Quote & FilePath & Quote)
            DynaLog.LogMessage("- Arguments: " & CommandArguments)
            DynaLog.LogMessage("- Working directory: " & Quote & WorkingDirectory & Quote)
            DynaLog.LogMessage("- Process command without redirecting output to console? " & If(DoNotRedirect, "Yes", "No"))
            DISMProc.StartInfo.FileName = FilePath
            DISMProc.StartInfo.Arguments = CommandArguments
            If WorkingDirectory <> "" Then
                DISMProc.StartInfo.WorkingDirectory = WorkingDirectory
            End If
            If Debugger.IsAttached Or DoNotRedirect Then
                DISMProc.StartInfo.CreateNoWindow = False
                DISMProc.StartInfo.WindowStyle = ProcessWindowStyle.Normal
            Else
                DISMProc.StartInfo.CreateNoWindow = True
                DISMProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            End If
            If DoNotRedirect Then
                DISMProc.Start()
                DISMProc.WaitForExit()
                DismExitCode = DISMProc.ExitCode
            Else
                DismExitCode = DISM_LogView.StartProcess(DISMProc.StartInfo.FileName, DISMProc.StartInfo.Arguments)
            End If
            DynaLog.LogMessage("Process finished with exit code " & Hex(DismExitCode))
        Catch ex As Exception
            DynaLog.LogMessage("Could not run process. Error message: " & ex.Message)
        End Try
    End Sub

    Private Function GetTargetImage() As String
        Dim OperationUseQuotes As Boolean
        Dim targetImage As String

        OperationUseQuotes = Not Path.GetPathRoot(MountDir) = MountDir
        targetImage = If(OperationUseQuotes, Quote & MountDir & Quote, MountDir)
        DynaLog.LogMessage("Target image to pass to DISM command arguments: " & targetImage)
        Return targetImage
    End Function

    Private Sub RunOps(opNum As Integer)
        DynaLog.LogMessage("Running operations...")
        DynaLog.LogMessage("Operation number: " & opNum)
        DynaLog.LogMessage("Setting DISM program and grabbing version information...")
        If DismProgram = "" Then DismProgram = MainForm.DismExe
        If Not File.Exists(DismProgram) Then DismProgram = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
        DismVersionChecker = FileVersionInfo.GetVersionInfo(DismProgram)
        CurrentPB.Value = 0
        PackageErrorCodes.Clear()
        FeatureErrorCodes.Clear()
        DynaLog.LogMessage("Mount directory to apply changes to: " & MountDir)
        Dim targetImage As String = ""
        If MountDir <> "" Then
            targetImage = GetTargetImage()
        End If
        Select Case opNum
            Case 0
                CreateProject()
            Case 1
                AppendImage()
            Case 2
                ApplyFfuImage()
            Case 3
                ApplyImage()
            Case 5
                CaptureFfuImage()
            Case 6
                CaptureImage()
            Case 7
                CleanupMountpoints()
            Case 8
                CommitImage()
            Case 9
                RemoveVolumeImages()
            Case 10
                ExportImage()
            Case 15
                MountImage()
            Case 16
                OptimizeFfuImage()
            Case 17
                OptimizeImage()
            Case 18
                RemountImage()
            Case 19
                SplitFfuImage()
            Case 20
                SplitImage()
            Case 21
                UnmountImage()
            Case 26
                AddPackages(targetImage)
            Case 27
                RemovePackages(targetImage)
            Case 30
                EnableFeatures(targetImage)
            Case 31
                DisableFeatures(targetImage)
            Case 32
                CleanupImage(targetImage)
            Case 33
                AddProvisioningPackage(targetImage)
            Case 37
                AddProvisionedAppxPackages(targetImage)
            Case 38
                RemoveProvisionedAppxPackages(targetImage)
            Case 60
                SetKeyboardLayeredDriver(targetImage)
            Case 64
                AddCapabilities(targetImage)
            Case 68
                RemoveCapabilities(targetImage)
            Case 71
                SetImageEdition(targetImage)
            Case 72
                SetImageProductKey(targetImage)
            Case 75
                AddDrivers(targetImage)
            Case 76
                RemoveDrivers(targetImage)
            Case 77
                ExportDrivers(targetImage)
            Case 78
                ImportDrivers(targetImage)
            Case 79
                ApplyUnattendedFile(targetImage)
            Case 83
                SetScratchSpace(targetImage)
            Case 84
                SetTargetPath(targetImage)
            Case 86
                InitiateOSUnistall()
            Case 87
                RemoveOSUnistall()
            Case 88
                SetOSUnistallWindow()
            Case 991
                ConvertImage()
            Case 992
                MergeSWM()
            Case 996
                SwitchIndexes()
            Case 998
                ReplaceFfuFile()
        End Select
        CurrentPB.Value = CurrentPB.Maximum
        AllPB.Value = AllPB.Maximum
        Thread.Sleep(1000)
    End Sub

#Region "Project Management Tasks"

    Private Sub CreateProject()
        DynaLog.LogMessage("Creating a project...")
        DynaLog.LogMessage("- Project name: " & Quote & projName & Quote)
        DynaLog.LogMessage("- Project path: " & Quote & projPath & Quote)
        allTasks.Text = LocalizationService.ForSection("Progress.CreateProject").Format("CreatingProject.Label", projName)
        currentTask.Text = LocalizationService.ForSection("Progress.CreateProject")("CreateProject.Button")
        LogView.AppendText(CrLf & ProgressLogText("Creating.Project.Structure"))
        Try
            DynaLog.LogMessage("Creating main project directory...")
            Directory.CreateDirectory(projPath & "\" & projName)
            CurrentPB.Value = 16.66
            Thread.Sleep(125)
            AllPB.Value = CurrentPB.Value
            DynaLog.LogMessage("Creating project settings directory...")
            Directory.CreateDirectory(projPath & "\" & projName & "\" & "settings")
            CurrentPB.Value = 33.329999999999998
            Thread.Sleep(125)
            AllPB.Value = CurrentPB.Value
            DynaLog.LogMessage("Creating mount directory...")
            Directory.CreateDirectory(projPath & "\" & projName & "\" & "mount")
            CurrentPB.Value = 50
            Thread.Sleep(125)
            AllPB.Value = CurrentPB.Value
            DynaLog.LogMessage("Creating scratch directory...")
            Directory.CreateDirectory(projPath & "\" & projName & "\" & "scr_temp")
            DynaLog.LogMessage("Creating unattended answer file directory...")
            Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml")
            DynaLog.LogMessage("Creating reports directory...")
            Directory.CreateDirectory(projPath & "\" & projName & "\" & "reports")
            DynaLog.LogMessage("Creating ADK deployment tools directory...")
            Directory.CreateDirectory(projPath & "\" & projName & "\" & "DandI")
            Directory.CreateDirectory(projPath & "\" & projName & "\" & "DandI\x86")
            Directory.CreateDirectory(projPath & "\" & projName & "\" & "DandI\amd64")
            Directory.CreateDirectory(projPath & "\" & projName & "\" & "DandI\arm")
            Directory.CreateDirectory(projPath & "\" & projName & "\" & "DandI\arm64")
            CurrentPB.Value = 66.659999999999997
            Thread.Sleep(125)
            AllPB.Value = CurrentPB.Value
            DynaLog.LogMessage("Writing project configuration...")
            File.WriteAllText(projPath & "\" & projName & "\" & "settings\project.ini",
                              "[ProjOptions]" & CrLf &
                              "Name=" & Quote & projName & Quote & CrLf &
                              "Location=" & projPath & CrLf &
                              "EpochCreationTime=" & DateTimeOffset.Now.ToUnixTimeSeconds().ToString() & CrLf & CrLf &
                              "[ImageOptions]" & CrLf &
                              "ImageFile=N/A" & CrLf &
                              "ImageIndex=N/A" & CrLf &
                              "ImageMountPoint=N/A" & CrLf &
                              "ImageVersion=N/A" & CrLf &
                              "ImageName=N/A" & CrLf &
                              "ImageDescription=N/A" & CrLf &
                              "ImageWIMBoot=N/A" & CrLf &
                              "ImageArch=N/A" & CrLf &
                              "ImageHal=N/A" & CrLf &
                              "ImageSPBuild=N/A" & CrLf &
                              "ImageSPLevel=N/A" & CrLf &
                              "ImageEdition=N/A" & CrLf &
                              "ImagePType=N/A" & CrLf &
                              "ImagePSuite=N/A" & CrLf &
                              "ImageSysRoot=N/A" & CrLf &
                              "ImageDirCount=N/A" & CrLf &
                              "ImageFileCount=N/A" & CrLf &
                              "ImageEpochCreate=N/A" & CrLf &
                              "ImageEpochModify=N/A" & CrLf &
                              "ImageLang=N/A" & CrLf & CrLf &
                              "[Params]" & CrLf &
                              "ImageReadWrite=N/A", ASCII)
            CurrentPB.Value = 83.329999999999998
            Thread.Sleep(125)
            AllPB.Value = CurrentPB.Value
            DynaLog.LogMessage("Writing DTProj file contents...")
            File.WriteAllText(projPath & "\" & projName & "\" & projName & ".dtproj",
                              "# DISMTools project file. File version: 0.1" & CrLf &
                              "[Settings]" & CrLf &
                              "SettingsInclude=\settings\project.ini" & CrLf & CrLf &
                              "[Project]" & CrLf &
                              "ProjName=" & projName & CrLf &
                              "ProjGuid=" & Guid.NewGuid().ToString(), ASCII)
            CurrentPB.Value = 100
            Thread.Sleep(125)
            AllPB.Value = CurrentPB.Value
            LogView.AppendText(CrLf & ProgressLogText("Project.Created.Successfully"))
            CurrentPB.Value = CurrentPB.Maximum
            AllPB.Value = AllPB.Maximum
            IsSuccessful = True
        Catch ex As Exception
            DynaLog.LogMessage("Could not create the project. Error message: " & ex.Message)
            LogView.AppendText(CrLf & ProgressLogText("An.Error.Has.Occurred.Please.Read.The.Details") & CrLf & ex.GetType().ToString() & ": " & Err.Description)
            If IsDebugged Then
                LogView.AppendText(CrLf & ProgressLogText("Debugging.Information") & ex.StackTrace)
            End If
            IsSuccessful = False
        End Try
    End Sub

#End Region

#Region "Image File Management Tasks"

    Private Sub AppendImage()
        DynaLog.LogMessage("Appending mount directory to the target image...")
        ' This variable tells the program to use quotes when appending a mount directory in a drive.
        ' This is false when we want to append an entire drive.
        Dim AppendixUseQuotes As Boolean = Not Path.GetPathRoot(AppendixSourceDir) = AppendixSourceDir
        DynaLog.LogMessage("Should quotes be used? " & If(AppendixUseQuotes, "Yes", "No"))
        If Not AppendixUseQuotes Then DynaLog.LogMessage("An entire drive will be appended to the target image.")
        DynaLog.LogMessage("- Source directory: " & Quote & AppendixSourceDir & Quote)
        DynaLog.LogMessage("- Destination image: " & Quote & AppendixDestinationImage & Quote)
        DynaLog.LogMessage("- Destination image name: " & Quote & AppendixName & Quote)
        DynaLog.LogMessage("- Destination image description: " & Quote & AppendixDescription & Quote)
        DynaLog.LogMessage("- WIMScript configuration list file: " & Quote & AppendixWimScriptConfig & Quote)
        DynaLog.LogMessage("- Append with WIMBoot configuration? " & If(AppendixUseWimBoot, "Yes", "No"))
        DynaLog.LogMessage("- Make image bootable? " & If(AppendixBootable, "Yes", "No"))
        DynaLog.LogMessage("- Verify image integrity? " & If(AppendixVerify, "Yes", "No"))
        DynaLog.LogMessage("- Check for file errors? " & If(AppendixCheckIntegrity, "Yes", "No"))
        DynaLog.LogMessage("- Use reparse point tag fix? " & If(AppendixReparsePt, "Yes", "No"))
        DynaLog.LogMessage("- Capture extended attributes (EAs)? " & If(AppendixCaptureExtendedAttribs, "Yes", "No"))
                allTasks.Text = LocalizationService.ForSection("Progress.AppendImage")("AppendingImage.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.AppendImage")("Appending.Mount.Dir.Button")
        LogView.AppendText(CrLf & ProgressLogText("Appending.Mount.Directory.To.Specified.Target.Image") & CrLf & ProgressLogText("Options") & CrLf &
                           ProgressLogText("Source.Image.Directory") & AppendixSourceDir & CrLf &
                           ProgressLogText("Destination.Image.File") & AppendixDestinationImage & CrLf &
                           ProgressLogText("Destination.Image.Name") & AppendixName & CrLf &
                           ProgressLogText("Destination.Image.Description") & If(AppendixDescription = "", ProgressLogText("None.Specified"), AppendixDescription) & CrLf)
        If AppendixWimScriptConfig = "" Then
            DynaLog.LogMessage("No configuration list file has been specified.")
            LogView.AppendText(ProgressLogText("Configuration.List.File.Not.Specified") & CrLf)
        Else
            DynaLog.LogMessage("A configuration list file has been specified. Checking if it exists...")
            LogView.AppendText(ProgressLogText("Configuration.List.File") & Quote & AppendixWimScriptConfig & Quote & CrLf)
            If Not File.Exists(AppendixWimScriptConfig) Then
                DynaLog.LogMessage("The configuration list file does not exist in the file system and will be skipped.")
                LogView.AppendText(ProgressLogText("WARNING.The.Configuration.List.File.Does.Not.Exist") & CrLf)
            End If
        End If
        LogView.AppendText(ProgressLogText("Append.Image.With.WIMBOOT.Configuration") & If(AppendixUseWimBoot, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf &
                           ProgressLogText("Make.Image.Bootable") & If(AppendixBootable, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf &
                           ProgressLogText("Verify.Image.Integrity") & If(AppendixCheckIntegrity, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf &
                           ProgressLogText("Check.For.File.Errors") & If(AppendixVerify, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf &
                           ProgressLogText("Use.The.Reparse.Point.Tag.Fix") & If(AppendixReparsePt, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf &
                           ProgressLogText("Capture.Extended.Attributes") & If(AppendixCaptureExtendedAttribs, ProgressLogText("Yes"), ProgressLogText("No")))
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        ' Not available
                    Case Is >= 2
                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /append-image /imagefile=" & Quote & AppendixDestinationImage & Quote & " /capturedir=" & If(AppendixUseQuotes, Quote, "") & AppendixSourceDir & If(AppendixUseQuotes, Quote, "") & " /name=" & Quote & AppendixName & Quote
                End Select
            Case 10
                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /append-image /imagefile=" & Quote & AppendixDestinationImage & Quote & " /capturedir=" & If(AppendixUseQuotes, Quote, "") & AppendixSourceDir & If(AppendixUseQuotes, Quote, "") & " /name=" & Quote & AppendixName & Quote
        End Select
        If AppendixDescription <> "" Then
            DynaLog.LogMessage("A description has been provided.")
            CommandArgs &= " /description=" & Quote & AppendixDescription & Quote
        End If
        If AppendixWimScriptConfig <> "" AndAlso File.Exists(AppendixWimScriptConfig) Then
            DynaLog.LogMessage("A configuration list file has been specified and exists in the file system.")
            CommandArgs &= " /configfile=" & Quote & AppendixWimScriptConfig & Quote
        End If
        If AppendixBootable Then CommandArgs &= " /bootable"
        If AppendixUseWimBoot Then CommandArgs &= " /wimboot"
        If AppendixCheckIntegrity Then CommandArgs &= " /checkintegrity"
        If AppendixVerify Then CommandArgs &= " /verify"
        If Not AppendixReparsePt Then CommandArgs &= " /norpfix"
        If AppendixCaptureExtendedAttribs Then CommandArgs &= " /EA"
        RunProcess(DismProgram, CommandArgs)
                currentTask.Text = LocalizationService.ForSection("Progress.AppendImage")("Gathering.Error.Level.Item")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

    Private Sub ApplyFfuImage()
        DynaLog.LogMessage("Applying specified FFU image to the specified application drive...")
        DynaLog.LogMessage("- Image to apply: " & Quote & FFUApplicationSourceImg & Quote)
        DynaLog.LogMessage("- Application drive: " & Quote & FFUApplicationDestDrive & Quote)
        DynaLog.LogMessage("- SFU name pattern: " & Quote & FFUApplicationSFUPattern & Quote)
                allTasks.Text = LocalizationService.ForSection("Progress.ApplyFfuImage")("ApplyingImage.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.ApplyFfuImage")("Applying.Image.Dest.Button")
        LogView.AppendText(CrLf & ProgressLogText("Applying.Image") & CrLf & ProgressLogText("Options") & CrLf &
                           ProgressLogText("Source.Image.File") & ApplicationSourceImg & CrLf &
                           ProgressLogText("Index.To.Apply") & ApplicationIndex & CrLf &
                           ProgressLogText("Target.Directory") & ApplicationDestDir & CrLf)
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        ' It seems like it's not available :(
                    Case Is >= 2
                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /apply-ffu /imagefile=" & Quote & FFUApplicationSourceImg & Quote
                End Select
            Case 10
                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /apply-ffu /imagefile=" & Quote & FFUApplicationSourceImg & Quote
        End Select
        ' Detect additional options and set CommandArgs
        CommandArgs &= " /applydrive=" & Quote & FFUApplicationDestDrive & Quote
        If FFUApplicationSFUPattern = "" Then
            LogView.AppendText(ProgressLogText("Split.FFU.SFU.File.Pattern.Not.Specified.Not") & CrLf)
        Else
            LogView.AppendText(ProgressLogText("Split.FFU.SFU.File.Pattern") & FFUApplicationSFUPattern & CrLf)
            CommandArgs &= " /sfufile=" & FFUApplicationSFUPattern
        End If
        RunProcess(DismProgram, CommandArgs)
                currentTask.Text = LocalizationService.ForSection("Progress.ApplyFfuImage")("Gathering.Error.Level.Item")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

    Private Sub ApplyImage()
        DynaLog.LogMessage("Applying specified Windows image to the specified application directory...")
        DynaLog.LogMessage("- Image to apply: " & Quote & ApplicationSourceImg & Quote)
        DynaLog.LogMessage("- Image index: " & ApplicationIndex)
        DynaLog.LogMessage("- Application directory: " & Quote & ApplicationDestDir & Quote)
        DynaLog.LogMessage("- Verify image integrity? " & If(ApplicationCheckInt, "Yes", "No"))
        DynaLog.LogMessage("- Check for file errors? " & If(ApplicationVerify, "Yes", "No"))
        DynaLog.LogMessage("- Use reparse point tag fix? " & If(ApplicationReparsePt, "Yes", "No"))
        DynaLog.LogMessage("- SWM name pattern: " & Quote & ApplicationSWMPattern & Quote)
        DynaLog.LogMessage("- Validate image for Trusted Desktop? " & If(ApplicationValidateForTD, "Yes", "No (it may not be supported)"))
        DynaLog.LogMessage("- Apply with WIMBoot configuration? " & If(ApplicationUseWimBoot, "Yes", "No"))
        DynaLog.LogMessage("- Apply in compact mode? " & If(ApplicationCompactMode, "Yes", "No"))
        DynaLog.LogMessage("- Apply extended attributes (EAs)? " & If(ApplicationUseExtAttr, "Yes", "No"))
                allTasks.Text = LocalizationService.ForSection("Progress.ApplyImage")("ApplyingImage.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.ApplyImage")("Applying.Image.Dest.Button")
        LogView.AppendText(CrLf & ProgressLogText("Applying.Image") & CrLf & ProgressLogText("Options") & CrLf &
                           ProgressLogText("Source.Image.File") & ApplicationSourceImg & CrLf &
                           ProgressLogText("Index.To.Apply") & ApplicationIndex & CrLf &
                           ProgressLogText("Target.Directory") & ApplicationDestDir & CrLf)
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        ' It seems like it's not available :(
                    Case Is >= 2
                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /apply-image /imagefile=" & Quote & ApplicationSourceImg & Quote & " /index=" & ApplicationIndex
                End Select
            Case 10
                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /apply-image /imagefile=" & Quote & ApplicationSourceImg & Quote & " /index=" & ApplicationIndex
        End Select
        ' Detect additional options and set CommandArgs
        CommandArgs &= " /applydir=" & Quote & ApplicationDestDir & Quote
        If ApplicationCheckInt Then
            LogView.AppendText(ProgressLogText("Verify.Image.Integrity.Yes") & CrLf)
            CommandArgs &= " /checkintegrity"
        Else
            LogView.AppendText(ProgressLogText("Verify.Image.Integrity.No") & CrLf)
        End If
        If ApplicationVerify Then
            LogView.AppendText(ProgressLogText("Check.For.File.Errors.Yes") & CrLf)
            CommandArgs &= " /verify"
        Else
            LogView.AppendText(ProgressLogText("Check.For.File.Errors.No") & CrLf)
        End If
        If ApplicationReparsePt Then
            LogView.AppendText(ProgressLogText("Use.Reparse.Point.Tag.Fix.Yes") & CrLf)
        Else
            LogView.AppendText(ProgressLogText("Use.Reparse.Point.Tag.Fix.No") & CrLf)
            CommandArgs &= " /norpfix"
        End If
        If ApplicationSWMPattern = "" Then
            LogView.AppendText(ProgressLogText("Split.WIM.SWM.File.Pattern.Not.Specified.Not") & CrLf)
        Else
            LogView.AppendText(ProgressLogText("Split.WIM.SWM.File.Pattern") & ApplicationSWMPattern & CrLf)
            CommandArgs &= " /swmfile=" & ApplicationSWMPattern
        End If
        If ApplicationValidateForTD Then
            LogView.AppendText(ProgressLogText("Validate.For.Trusted.Desktop.Yes") & CrLf)
            CommandArgs &= " /confirmtrustedfile"
        Else
            LogView.AppendText(ProgressLogText("Validate.For.Trusted.Desktop.No.Not.Supported") & CrLf)
        End If
        If ApplicationUseWimBoot Then
            LogView.AppendText(ProgressLogText("Apply.Using.WIMBOOT.Configuration.Yes") & CrLf)
            CommandArgs &= " /wimboot"
        Else
            LogView.AppendText(ProgressLogText("Apply.Using.WIMBOOT.Configuration.No") & CrLf)
        End If
        If ApplicationCompactMode Then
            LogView.AppendText(ProgressLogText("Use.Compact.Mode.Yes") & CrLf)
            CommandArgs &= " /compact"
        Else
            LogView.AppendText(ProgressLogText("Use.Compact.Mode.No") & CrLf)
        End If
        If ApplicationUseExtAttr Then
            LogView.AppendText(ProgressLogText("Apply.Using.Extended.Attributes.Yes") & CrLf)
            CommandArgs &= " /ea"
        Else
            LogView.AppendText(ProgressLogText("Apply.Using.Extended.Attributes.No") & CrLf)
        End If
        RunProcess(DismProgram, CommandArgs)
                currentTask.Text = LocalizationService.ForSection("Progress.ApplyImage")("Gathering.Error.Level.Item")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

    Private Sub CaptureFfuImage()
        DynaLog.LogMessage("Capturing physical drive to the target image...")
        DynaLog.LogMessage("- Source drive: " & FFUCaptureSourceDrive)
        DynaLog.LogMessage("- Destination image: " & Quote & FFUCaptureDestinationFfuImage & Quote)
        DynaLog.LogMessage("- Destination image name: " & Quote & FFUCaptureName & Quote)
        DynaLog.LogMessage("- Destination image description: " & Quote & FFUCaptureDescription & Quote)
                allTasks.Text = LocalizationService.ForSection("Progress.CaptureFfuImage")("CapturingImage.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.CaptureFfuImage")("CaptureDir.Button")
        LogView.AppendText(CrLf & ProgressLogText("Capturing.Directory") & CrLf & ProgressLogText("Options") & CrLf &
                           ProgressLogText("Source.Directory") & FFUCaptureSourceDrive & CrLf &
                           ProgressLogText("Destination.Image") & FFUCaptureDestinationFfuImage & CrLf &
                           ProgressLogText("Captured.Image.Name") & FFUCaptureName & CrLf)
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        ' Not available
                    Case Is >= 2
                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /capture-ffu /imagefile=" & Quote & FFUCaptureDestinationFfuImage & Quote & " /capturedrive=" & FFUCaptureSourceDrive & " /name=" & Quote & FFUCaptureName & Quote
                End Select
            Case 10
                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /capture-ffu /imagefile=" & Quote & FFUCaptureDestinationFfuImage & Quote & " /capturedrive=" & FFUCaptureSourceDrive & " /name=" & Quote & FFUCaptureName & Quote
        End Select
        ' Get additional options
        If FFUCaptureDescription = "" Then
            LogView.AppendText(ProgressLogText("Captured.Image.Description.None.Specified") & CrLf)
        Else
            DynaLog.LogMessage("A description has been provided.")
            LogView.AppendText(ProgressLogText("Captured.Image.Description") & Quote & FFUCaptureDescription & Quote & CrLf)
            CommandArgs &= " /description=" & Quote & FFUCaptureDescription & Quote
        End If
        If FFUCaptureCompressType = 0 Then
            LogView.AppendText(ProgressLogText("Compression.Type.None") & CrLf)
            CommandArgs &= " /compress=none"
        ElseIf FFUCaptureCompressType = 1 Then
            LogView.AppendText(ProgressLogText("Compression.Type.Default") & CrLf)
            CommandArgs &= " /compress=default"
        End If
        LogView.AppendText(CrLf & ProgressLogText("Capturing.Image"))
        RunProcess(DismProgram, CommandArgs)
                currentTask.Text = LocalizationService.ForSection("Progress.CaptureFfuImage")("Gathering.Error.Level.Item")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

    Private Sub CaptureImage()
        DynaLog.LogMessage("Capturing mount directory to the target image...")
        ' This variable tells the program to use quotes when capturing a mount directory in a drive.
        ' This is false when we want to capture an entire drive.
        Dim UseQuotes As Boolean = Not Path.GetPathRoot(CaptureSourceDir) = CaptureSourceDir
        DynaLog.LogMessage("Should quotes be used? " & If(UseQuotes, "Yes", "No"))
        If Not UseQuotes Then DynaLog.LogMessage("An entire drive will be captured to the target image.")
        DynaLog.LogMessage("- Source directory: " & Quote & CaptureSourceDir & Quote)
        DynaLog.LogMessage("- Destination image: " & Quote & CaptureDestinationImage & Quote)
        DynaLog.LogMessage("- Destination image name: " & Quote & CaptureName & Quote)
        DynaLog.LogMessage("- Destination image description: " & Quote & CaptureDescription & Quote)
        DynaLog.LogMessage("- WIMScript configuration list file: " & Quote & CaptureWimScriptConfig & Quote)
        DynaLog.LogMessage("- Append with WIMBoot configuration? " & If(CaptureUseWimBoot, "Yes", "No"))
        DynaLog.LogMessage("- Make image bootable? " & If(CaptureBootable, "Yes", "No"))
        DynaLog.LogMessage("- Verify image integrity? " & If(CaptureVerify, "Yes", "No"))
        DynaLog.LogMessage("- Check for file errors? " & If(CaptureCheckIntegrity, "Yes", "No"))
        DynaLog.LogMessage("- Use reparse point tag fix? " & If(CaptureReparsePt, "Yes", "No"))
        DynaLog.LogMessage("- Capture extended attributes (EAs)? " & If(CaptureExtendedAttributes, "Yes", "No"))
        DynaLog.LogMessage("- Capture compression level type: " & CaptureCompressType)
                allTasks.Text = LocalizationService.ForSection("Progress.CaptureImage")("CapturingImage.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.CaptureImage")("CaptureDir.Button")
        LogView.AppendText(CrLf & ProgressLogText("Capturing.Directory") & CrLf & ProgressLogText("Options") & CrLf &
                           ProgressLogText("Source.Directory") & CaptureSourceDir & CrLf &
                           ProgressLogText("Destination.Image") & CaptureDestinationImage & CrLf &
                           ProgressLogText("Captured.Image.Name") & CaptureName & CrLf)
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        ' Not available
                    Case Is >= 2
                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /capture-image /imagefile=" & Quote & CaptureDestinationImage & Quote & " /capturedir=" & If(UseQuotes, Quote, "") & CaptureSourceDir & If(UseQuotes, Quote, "") & " /name=" & Quote & CaptureName & Quote
                End Select
            Case 10
                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /capture-image /imagefile=" & Quote & CaptureDestinationImage & Quote & " /capturedir=" & If(UseQuotes, Quote, "") & CaptureSourceDir & If(UseQuotes, Quote, "") & " /name=" & Quote & CaptureName & Quote
        End Select
        ' Get additional options
        If CaptureDescription = "" Then
            LogView.AppendText(ProgressLogText("Captured.Image.Description.None.Specified") & CrLf)
        Else
            DynaLog.LogMessage("A description has been provided.")
            LogView.AppendText(ProgressLogText("Captured.Image.Description") & Quote & CaptureDescription & Quote & CrLf)
            CommandArgs &= " /description=" & Quote & CaptureDescription & Quote
        End If
        If CaptureWimScriptConfig = "" Then
            DynaLog.LogMessage("No configuration list file has been specified.")
            LogView.AppendText(ProgressLogText("Configuration.List.File.Not.Specified") & CrLf)
        Else
            DynaLog.LogMessage("A configuration list file has been specified. Checking if it exists...")
            LogView.AppendText(ProgressLogText("Configuration.List.File") & CaptureWimScriptConfig & CrLf)
            ' Possibly, the file may have been deleted after being specified. Determine whether it still exists
            If File.Exists(CaptureWimScriptConfig) Then
                CommandArgs &= " /configfile=" & Quote & CaptureWimScriptConfig & Quote
            Else
                DynaLog.LogMessage("The configuration list file does not exist in the file system and will be skipped.")
                LogView.AppendText(ProgressLogText("WARNING.The.Configuration.List.File.Does.Not.Exist") & CrLf)
            End If
        End If
        If CaptureCompressType = 0 Then
            LogView.AppendText(ProgressLogText("Compression.Type.None") & CrLf)
            CommandArgs &= " /compress=none"
        ElseIf CaptureCompressType = 1 Then
            LogView.AppendText(ProgressLogText("Compression.Type.Fast") & CrLf)
            CommandArgs &= " /compress=fast"
        ElseIf CaptureCompressType = 2 Then
            LogView.AppendText(ProgressLogText("Compression.Type.Maximum") & CrLf)
            CommandArgs &= " /compress=max"
        End If
        If CaptureBootable Then
            LogView.AppendText(ProgressLogText("Mark.Image.As.Bootable.Yes") & CrLf)
            CommandArgs &= " /bootable"
        Else
            LogView.AppendText(ProgressLogText("Mark.Image.As.Bootable.No") & CrLf)
        End If
        If CaptureCheckIntegrity Then
            LogView.AppendText(ProgressLogText("Check.Image.Integrity.Yes") & CrLf)
            CommandArgs &= " /checkintegrity"
        Else
            LogView.AppendText(ProgressLogText("Check.Image.Integrity.No") & CrLf)
        End If
        If CaptureVerify Then
            LogView.AppendText(ProgressLogText("Verify.File.Errors.Yes") & CrLf)
            CommandArgs &= " /verify"
        Else
            LogView.AppendText(ProgressLogText("Verify.File.Errors.No") & CrLf)
        End If
        If CaptureReparsePt Then
            LogView.AppendText(ProgressLogText("Use.The.Reparse.Point.Tag.Fix.Yes") & CrLf)
        Else
            LogView.AppendText(ProgressLogText("Use.The.Reparse.Point.Tag.Fix.No") & CrLf)
            CommandArgs &= " /norpfix"
        End If
        If CaptureUseWimBoot Then
            LogView.AppendText(ProgressLogText("Append.With.WIMBOOT.Configuration.Yes") & CrLf)
            CommandArgs &= " /wimboot"
        Else
            LogView.AppendText(ProgressLogText("Append.With.WIMBOOT.Configuration.No") & CrLf)
        End If
        If CaptureExtendedAttributes Then
            LogView.AppendText(ProgressLogText("Capture.Extended.Attributes.Yes") & CrLf)
            CommandArgs &= " /ea"
        Else
            LogView.AppendText(ProgressLogText("Capture.Extended.Attributes.No") & CrLf)
        End If
        LogView.AppendText(CrLf & ProgressLogText("Capturing.Image"))
        RunProcess(DismProgram, CommandArgs)
                currentTask.Text = LocalizationService.ForSection("Progress.CaptureImage")("Gathering.Error.Level.Item")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

    Private Sub CleanupMountpoints()
        DynaLog.LogMessage("Cleaning up mount points by deleting resources from old or corrupted images...")
        DynaLog.LogMessage("This does not require any additional options and invokes an API call. This will take some time depending on your system performance.")
                allTasks.Text = LocalizationService.ForSection("Progress.CleanupMounts")("Cleaning.Up.Mount.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.CleanupMounts")("Deleting.Corrupted.Button")
        LogView.AppendText(CrLf & ProgressLogText("Cleaning.Up.Mount.Points") & CrLf & CrLf &
                           ProgressLogText("This.Can.Take.Some.Time.Depending.On.The"))
        Try
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(If(LogLevel = 1, DismLogLevel.LogErrors, If(LogLevel = 2, DismLogLevel.LogErrorsWarnings, If(LogLevel = 3, DismLogLevel.LogErrorsWarningsInfo, DismLogLevel.LogErrorsWarningsInfo))), If(AutoLogs, Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now), LogPath))
            DynaLog.LogMessage("Cleaning up mount points...")
            DismApi.CleanupMountpoints()
        Catch ex As DismException
            DynaLog.LogMessage("Could not clean up mount points. Error message: " & ex.Message)
            errCode = Hex(ex.ErrorCode)
        Finally
            Try
                DynaLog.LogMessage("Shutting down API...")
                DismApi.Shutdown()
            Catch ex As Exception

            End Try
        End Try
        CurrentPB.Value = 50
        AllPB.Value = CurrentPB.Value
                currentTask.Text = LocalizationService.ForSection("Progress.CleanupMounts")("Gathering.Error.Level.Item")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        If errCode Is Nothing Then
            errCode = 0
            IsSuccessful = True
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

    Private Sub CommitFfu()
        Dim tempFfuPath As String = String.Format("capturedFFU_{0}.ffu", New Random().Next(Integer.MaxValue))

        ' Options for capture task
        FFUCaptureSourceDrive = ReferenceImage.FFUInfo.MountDiskPath
        FFUCaptureDestinationFfuImage = Path.Combine(Path.GetTempPath(), tempFfuPath)
        FFUCaptureName = ReferenceImage.ImageName
        FFUCaptureDescription = ReferenceImage.ImageDescription
        FFUCaptureCompressType = 1

        ' Options for unmount task
        MountDir = MountDir
        UMountOp = 1
        UMountLocalDir = True
        RandomMountDir = ""
        CheckImgIntegrity = False
        SaveToNewIndex = False
        UMountImgIndex = 1

        ' Options for replace task
        FFUReplaceSourceFFU = Path.Combine(Path.GetTempPath(), tempFfuPath)
        FFUReplaceDestinationFFU = ReferenceImage.ImageFile

        ' Options for mount task
        SourceImg = ReferenceImage.ImageFile
        ImgIndex = 1
        isReadOnly = False
        isOptimized = False
        isIntegrityTested = False

        CaptureFfuImage()
        UnmountImage()
        ReplaceFfuFile()
        MountImage()
    End Sub

    Private Sub CommitImage()
        DynaLog.LogMessage("Saving changes to the Windows image...")
                allTasks.Text = LocalizationService.ForSection("Progress.CommitImage")("CommittingImage.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.CommitImage")("Saving.Changes.Image.Button")
        If ReferenceImage IsNot Nothing Then
            If Path.GetExtension(ReferenceImage.ImageFile).Equals(".ffu", StringComparison.OrdinalIgnoreCase) Then
                CommitFfu()
                Exit Sub
            End If
        End If
        LogView.AppendText(CrLf & ProgressLogText("Saving.Changes") & CrLf & ProgressLogText("Options") & CrLf &
                           ProgressLogText("Mount.Directory") & MountDir)
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /commit-wim /mountdir=" & Quote & MountDir & Quote
                    Case Is >= 2
                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /commit-image /mountdir=" & Quote & MountDir & Quote
                End Select
            Case 10
                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /commit-image /mountdir=" & Quote & MountDir & Quote
        End Select
        ' TODO: Add additional options later
        RunProcess(DismProgram, CommandArgs)
                currentTask.Text = LocalizationService.ForSection("Progress.CommitImage")("Gathering.Error.Level.Item")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

    Private Sub RemoveVolumeImages()
        DynaLog.LogMessage("Preparing to remove volume images from the specified Windows image file...")
        DynaLog.LogMessage("Will this operation require an unmount of the specified image? " & If(imgIndexDeletionUnmount, "Yes", "No"))
        If imgIndexDeletionUnmount Then
            DynaLog.LogMessage("Preparing to unmount the Windows image...")
            RunOps(21)
            AllPB.Value = AllPB.Maximum / taskCount
            currentTCont += 1
            taskCountLbl.Text = LocalizationService.ForSection("Progress").Format("Tasks.Label", currentTCont, taskCount)
        End If
                allTasks.Text = LocalizationService.ForSection("Progress.RemoveVolumes")("DeletingImages.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.RemoveVolumes")("Prepare.Remove.Button")
        DynaLog.LogMessage("Source image to remove indexes from: " & Quote & imgIndexDeletionSourceImg & Quote)
        LogView.AppendText(CrLf & ProgressLogText("Removing.Volume.Images.From.File") & CrLf &
                           ProgressLogText("Options") & CrLf &
                           ProgressLogText("Source.Image") & imgIndexDeletionSourceImg & CrLf)
        If imgIndexDeletionIntCheck Then
            LogView.AppendText(ProgressLogText("Check.Image.Integrity.Yes"))
        Else
            LogView.AppendText(ProgressLogText("Check.Image.Integrity.No"))
        End If
        CurrentPB.Maximum = imgIndexDeletionCount
        ' Removing volume images
        LogView.AppendText(CrLf &
                           ProgressLogText("Removing.Volume.Images") & CrLf)
        For x = 0 To Array.LastIndexOf(imgIndexDeletionNames, imgIndexDeletionLastName)
            If x + 1 > CurrentPB.Maximum Then Exit For
            DynaLog.LogMessage("Volume image to remove: " & Quote & imgIndexDeletionNames(x) & Quote)
            DynaLog.LogMessage("Processing task...")
            CurrentPB.Value = x + 1
            currentTask.Text = LocalizationService.ForSection("Progress.RemoveVolumes").Format("Volume.Image.Item", imgIndexDeletionNames(x))
            LogView.AppendText(CrLf &
                               "- " & imgIndexDeletionNames(x) & "...")
            CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /delete-image /imagefile=" & Quote & imgIndexDeletionSourceImg & Quote & " /name=" & Quote & imgIndexDeletionNames(x) & Quote
            If imgIndexDeletionIntCheck Then
                CommandArgs &= " /checkintegrity"
            End If
            RunProcess(DismProgram, CommandArgs)
            If Hex(DismExitCode).Length < 8 Then
                LogView.AppendText(ProgressLogText("Error.Level.2") & DismExitCode)
            Else
                LogView.AppendText(ProgressLogText("Error.Level.0x.2") & Hex(DismExitCode))
            End If
        Next
        CurrentPB.Value = CurrentPB.Maximum
        AllPB.Value = 100
        GetErrorCode(False)
    End Sub

    Private Sub ExportImage()
        DynaLog.LogMessage("Exporting specified Windows image...")
        DynaLog.LogMessage("- Source image to export: " & Quote & imgExportSourceImage & Quote)
        DynaLog.LogMessage("- Source index to export: " & imgExportSourceIndex)
        DynaLog.LogMessage("- Destination image file: " & Quote & imgExportDestinationImage & Quote)
        DynaLog.LogMessage("- Will a custom name be used? " & If(imgExportDestinationUseCustomName, "Yes", "No"))
        If imgExportDestinationUseCustomName Then
            DynaLog.LogMessage("  The custom name for the destination image file will be " & Quote & imgExportDestinationName & Quote)
        Else
            DynaLog.LogMessage("  The name of the source index will be used by the destination image file")
        End If
        DynaLog.LogMessage("- Compression type: " & imgExportCompressType)
        DynaLog.LogMessage("- Mark the image as bootable? " & If(imgExportMarkBootable, "Yes", "No"))
        DynaLog.LogMessage("- Use WIMBoot configuration? " & If(imgExportUseWimBoot, "Yes", "No"))
        DynaLog.LogMessage("- Check image integrity? " & If(imgExportCheckIntegrity, "Yes", "No"))
                allTasks.Text = LocalizationService.ForSection("Progress.ExportImage")("ExportingImage.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.ExportImage")("Exporting.Image.Button")
        LogView.AppendText(CrLf & ProgressLogText("Exporting.The.Specified.Image.To.A.Destination.Image") & CrLf & ProgressLogText("Options") & CrLf &
                           ProgressLogText("Source.Image.File") & imgExportSourceImage & CrLf &
                           ProgressLogText("Source.Image.Index") & imgExportSourceIndex & CrLf &
                           ProgressLogText("Destination.Image.File") & imgExportDestinationImage & CrLf &
                           If(imgExportDestinationUseCustomName, ProgressLogText("Destination.Image.Name") & imgExportDestinationName, ""))
        Select Case imgExportCompressType
            Case 0
                LogView.AppendText(CrLf & ProgressLogText("Compression.Type.No.Compression"))
            Case 1
                LogView.AppendText(CrLf & ProgressLogText("Compression.Type.Fast.Compression"))
            Case 2
                LogView.AppendText(CrLf & ProgressLogText("Compression.Type.Maximum.Compression"))
            Case 3
                LogView.AppendText(CrLf & ProgressLogText("Compression.Type.ESD.Conversion.Recovery"))
        End Select
        LogView.AppendText(CrLf & ProgressLogText("Mark.The.Image.As.Bootable") & If(imgExportMarkBootable, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf &
                           ProgressLogText("Append.Image.With.WIMBOOT.Configuration") & If(imgExportUseWimBoot, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf &
                           ProgressLogText("Check.Image.Integrity.Before.Exporting.The.Image") & If(imgExportCheckIntegrity, ProgressLogText("Yes"), ProgressLogText("No")))
        ' Show information regarding SWM files
        DynaLog.LogMessage("Extension of source image file: " & Path.GetExtension(imgExportSourceImage))
        If Path.GetExtension(imgExportSourceImage).EndsWith("swm", StringComparison.OrdinalIgnoreCase) Then
            DynaLog.LogMessage("We are dealing with SWM files. Showing why we mark all of them for export...")
            LogView.AppendText(CrLf & CrLf & ProgressLogText("NOTE.The.Source.Image.Contains.An.Asterisk.Sign"))
        End If
        ' Configure basic command arguments
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        ' Not available
                    Case Is >= 2
                        CommandArgs &= " /export-image /sourceimagefile=" & Quote & imgExportSourceImage & Quote & " /sourceindex=" & imgExportSourceIndex & " /destinationimagefile=" & Quote & imgExportDestinationImage & Quote
                End Select
            Case 10
                CommandArgs &= " /export-image /sourceimagefile=" & Quote & imgExportSourceImage & Quote & " /sourceindex=" & imgExportSourceIndex & " /destinationimagefile=" & Quote & imgExportDestinationImage & Quote
        End Select
        ' Configure additional command arguments
        If imgExportDestinationUseCustomName Then
            CommandArgs &= " /destinationname=" & Quote & imgExportDestinationName & Quote
        End If
        Select Case imgExportCompressType
            Case 0
                CommandArgs &= " /compress:none"
            Case 1
                CommandArgs &= " /compress:fast"
            Case 2
                CommandArgs &= " /compress:max"
            Case 3
                CommandArgs &= " /compress:recovery"
        End Select
        If imgExportMarkBootable Then CommandArgs &= " /bootable"
        If imgExportUseWimBoot Then CommandArgs &= " /wimboot"
        If imgExportCheckIntegrity Then CommandArgs &= " /checkintegrity"
        RunProcess(DismProgram, CommandArgs)
                currentTask.Text = LocalizationService.ForSection("Progress.ExportImage")("Gathering.Error.Level.Item")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

    Private Sub MountImage()
        If EnableExperiments Then
            ImageOperationDefinitions(15).OperationOptions = New Dictionary(Of String, Object) From {
                {"DismProgram", DismProgram},
                {"DismVersionChecker", DismVersionChecker},
                {"SourceImg", SourceImg},
                {"ImgIndex", ImgIndex},
                {"MountDir", MountDir},
                {"IsReadOnly", isReadOnly},
                {"IsOptimized", isOptimized},
                {"IsIntegrityTested", isIntegrityTested}
            }
            errCode = ImageOperationDefinitions(15).RunOperation().ToString()
        Else
            DynaLog.LogMessage("Preparing to mount the Windows image...")
            DynaLog.LogMessage("- Image file to mount: " & Quote & SourceImg & Quote)
            DynaLog.LogMessage("- Image index to mount: " & ImgIndex)
            DynaLog.LogMessage("- Location to mount image to: " & Quote & MountDir & Quote)
            DynaLog.LogMessage("- Mount with read-only permissions? " & If(isReadOnly, "Yes", "No"))
            DynaLog.LogMessage("- Optimize mount times? " & If(isOptimized, "Yes", "No"))
            DynaLog.LogMessage("- Check image integrity? " & If(isIntegrityTested, "Yes", "No"))
                    allTasks.Text = LocalizationService.ForSection("Progress.MountImage")("MountingImage.Button")
                    currentTask.Text = LocalizationService.ForSection("Progress.MountImage")("Mounting.Image.Button")
            LogView.AppendText(CrLf & ProgressLogText("Mounting.Image") & CrLf & ProgressLogText("Options") & CrLf &
                               ProgressLogText("Image.File") & SourceImg & CrLf &
                               ProgressLogText("Image.Index") & ImgIndex & CrLf &
                               ProgressLogText("Mount.Point") & MountDir)
            Try
                If Not isReadOnly AndAlso (File.GetAttributes(SourceImg) And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                    DynaLog.LogMessage("Source image contains read-only flag. Attempting to remove it...")
                    ' Remove readonly flag
                    File.SetAttributes(SourceImg, (File.GetAttributes(SourceImg) And Not FileAttributes.ReadOnly))
                    DynaLog.LogMessage("Flags were removed successfully.")
                End If
            Catch ex As Exception
                DynaLog.LogMessage("Could not remove or get flags. Error message: " & ex.Message)
            End Try
            Select Case DismVersionChecker.ProductMajorPart
                Case 6
                    Select Case DismVersionChecker.ProductMinorPart
                        Case 1
                            CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /mount-wim /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " /mountdir=" & Quote & MountDir & Quote
                        Case Is >= 2
                            CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /mount-image /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " /mountdir=" & Quote & MountDir & Quote
                    End Select
                Case 10
                    CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /mount-image /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " /mountdir=" & Quote & MountDir & Quote
            End Select
            If isReadOnly Then
                LogView.AppendText(CrLf & ProgressLogText("Mount.Image.With.Read.Only.Permissions.Yes"))
                CommandArgs &= " /readonly"
            Else
                LogView.AppendText(CrLf & ProgressLogText("Mount.Image.With.Read.Only.Permissions.No"))
            End If
            If isOptimized Then
                LogView.AppendText(CrLf & ProgressLogText("Optimize.Mount.Time.Yes"))
                CommandArgs &= " /optimize"
            Else
                LogView.AppendText(CrLf & ProgressLogText("Optimize.Mount.Time.No"))
            End If
            If isIntegrityTested Then
                LogView.AppendText(CrLf & ProgressLogText("Check.Image.Integrity.Yes"))
                CommandArgs &= " /checkintegrity"
            Else
                LogView.AppendText(CrLf & ProgressLogText("Check.Image.Integrity.No"))
            End If
            RunProcess(DismProgram, CommandArgs)
                    currentTask.Text = LocalizationService.ForSection("Progress.MountImage")("Gathering.Error.Level.Item")
            LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        End If
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

    Private Sub OptimizeFfuImage()
        DynaLog.LogMessage("Optimizing the Windows FFU image...")
        DynaLog.LogMessage("- Source image to optimize: " & Quote & FFUOptimizationSource & Quote)
        DynaLog.LogMessage("- Partition to optimize: " & FFUOptimizationCustomPartitionNum & If(FFUOptimizationCustomPartitionNum = 0, " (Default partition in the FFU will be optimized)", ""))
        allTasks.Text = LocalizationService.ForSection("Progress.Operation")("OptimizingImage.Label")
        currentTask.Text = LocalizationService.ForSection("Progress.Operation")("Optimizing.Windows.Label")
        LogView.AppendText(CrLf & ProgressLogText("Optimizing.Windows.Image") & CrLf &
                           ProgressLogText("Source.Image.To.Optimize") & Quote & FFUOptimizationSource & Quote & CrLf &
                           ProgressLogText("Partition.To.Optimize") & FFUOptimizationCustomPartitionNum & If(FFUOptimizationCustomPartitionNum = 0, ProgressLogText("Default.Partition.In.The.FFU.Will.Be.Optimized"), "") & CrLf)
        ' Check the DISM version, as the Windows 7-8.1 versions don't allow this action
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                ' Not supported
            Case 10
                CommandArgs &= " /optimize-ffu /imagefile=" & Quote & FFUOptimizationSource & Quote
        End Select

        If FFUOptimizationCustomPartitionNum > 0 Then CommandArgs &= " /partitionnumber=" & FFUOptimizationCustomPartitionNum

        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
        Else
            LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
        End If
        GetErrorCode(False)
    End Sub

    Private Sub OptimizeImage()
        DynaLog.LogMessage("Optimizing the Windows image...")
        DynaLog.LogMessage("- Source image to optimize: " & Quote & OptimizationSource & Quote)
        DynaLog.LogMessage("- Optimization mode: " & OptimizationMode)
        allTasks.Text = LocalizationService.ForSection("Progress.Operation")("OptimizingImage.Label")
        currentTask.Text = LocalizationService.ForSection("Progress.Operation")("Optimizing.Windows.Label")
        LogView.AppendText(CrLf & ProgressLogText("Optimizing.Windows.Image") & CrLf &
                           ProgressLogText("Source.Image.To.Optimize") & Quote & OptimizationSource & Quote & CrLf &
                           ProgressLogText("Optimization.Mode") & If(OptimizationMode = 0, ProgressLogText("Reduce.Online.Configuration.Time"), ProgressLogText("Prepare.Image.For.WIMBOOT.System")) & CrLf)
        ' Check the DISM version, as the Windows 7-8.1 versions don't allow this action
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                ' Not supported
            Case 10
                CommandArgs &= " /image=" & Quote & OptimizationSource & Quote & " /optimize-image " & If(OptimizationMode = 0, "/boot", "/wimboot")
        End Select
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
        Else
            LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
        End If
        GetErrorCode(False)
    End Sub

    Private Sub RemountImage()
        DynaLog.LogMessage("Reloading the servicing session of the mounted image...")
        DynaLog.LogMessage("- Mount location of the image file we are interested in reloading: " & Quote & MountDir & Quote)
        DynaLog.LogMessage("This invokes an API call. This process will take some time depending on your system performance and how big the Windows image is.")
                allTasks.Text = LocalizationService.ForSection("Progress.RemountImage")("RemountingImage.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.RemountImage")("ReloadSession.Button")
        LogView.AppendText(CrLf & ProgressLogText("Reloading.Servicing.Session") & CrLf &
                           ProgressLogText("Mount.Directory") & MountDir)
        Try
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(If(LogLevel = 1, DismLogLevel.LogErrors, If(LogLevel = 2, DismLogLevel.LogErrorsWarnings, If(LogLevel = 3, DismLogLevel.LogErrorsWarningsInfo, DismLogLevel.LogErrorsWarningsInfo))), If(AutoLogs, Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now), LogPath))
            DynaLog.LogMessage("Remounting image...")
            DismApi.RemountImage(MountDir)
        Catch ex As DismException
            DynaLog.LogMessage("Could not remount Windows image. Error message: " & ex.Message)
            errCode = Hex(ex.ErrorCode)
            IsSuccessful = False
        Finally
            Try
                DynaLog.LogMessage("Shutting down API...")
                DismApi.Shutdown()
            Catch ex As Exception

            End Try
        End Try
        CurrentPB.Value = 50
        AllPB.Value = CurrentPB.Value
                currentTask.Text = LocalizationService.ForSection("Progress.RemountImage")("Gathering.Error.Level.Item")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        If errCode Is Nothing Then
            errCode = 0
            IsSuccessful = True
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

    Private Sub SplitFfuImage()
        DynaLog.LogMessage("Splitting the Windows FFU image...")
        DynaLog.LogMessage("- Source image file to split: " & Quote & SFUSplitSourceFile & Quote)
        DynaLog.LogMessage("- Maximum size of split images: " & SFUSplitFileSize & " MB")
        DynaLog.LogMessage("- Destination of SFU files: " & Quote & SFUSplitTargetFile & Quote)
        DynaLog.LogMessage("- Check image integrity? " & If(SFUSplitCheckIntegrity, "Yes", "No"))
                allTasks.Text = LocalizationService.ForSection("Progress.SplitFfuImage")("SplittingImage.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.SplitFfuImage")("Splitting.File.Button")
        LogView.AppendText(CrLf & ProgressLogText("Splitting.FFU.File.Into.SFU.Files") & CrLf &
                           ProgressLogText("Source.Image.File.To.Split") & Quote & SFUSplitSourceFile & Quote & CrLf &
                           ProgressLogText("Maximum.Size.Of.The.Split.Images.In.MB") & SFUSplitFileSize & " MB" & CrLf &
                           ProgressLogText("Name.And.Path.Of.The.Target.SFU.File") & Quote & SFUSplitTargetFile & Quote & CrLf &
                           ProgressLogText("Check.Integrity.Before.Splitting.This.Image") & If(SFUSplitCheckIntegrity, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf & CrLf &
                           ProgressLogText("Do.Note.That.If.The.Image.Contains.A") & CrLf)
        ' Check the DISM version, as the Windows 7 version doesn't allow this action
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        ' Not supported
                    Case Is >= 2
                        CommandArgs &= " /split-ffu /imagefile=" & Quote & SFUSplitSourceFile & Quote & " /sfufile=" & Quote & SFUSplitTargetFile & Quote & " /filesize=" & SFUSplitFileSize & If(SFUSplitCheckIntegrity, " /checkintegrity", "")
                End Select
            Case 10
                CommandArgs &= " /split-image /imagefile=" & Quote & SFUSplitSourceFile & Quote & " /sfufile=" & Quote & SFUSplitTargetFile & Quote & " /filesize=" & SFUSplitFileSize & If(SFUSplitCheckIntegrity, " /checkintegrity", "")
        End Select
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
        Else
            LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
        End If
        GetErrorCode(False)
    End Sub

    Private Sub SplitImage()
        DynaLog.LogMessage("Splitting the Windows image...")
        DynaLog.LogMessage("- Source image file to split: " & Quote & SWMSplitSourceFile & Quote)
        DynaLog.LogMessage("- Maximum size of split images: " & SWMSplitFileSize & " MB")
        DynaLog.LogMessage("- Destination of SWM files: " & Quote & SWMSplitTargetFile & Quote)
        DynaLog.LogMessage("- Check image integrity? " & If(SWMSplitCheckIntegrity, "Yes", "No"))
                allTasks.Text = LocalizationService.ForSection("Progress.SplitImage")("SplittingImage.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.SplitImage")("Splitting.WIM.File.Button")
        LogView.AppendText(CrLf & ProgressLogText("Splitting.WIM.File.Into.SWM.Files") & CrLf &
                           ProgressLogText("Source.Image.File.To.Split") & Quote & SWMSplitSourceFile & Quote & CrLf &
                           ProgressLogText("Maximum.Size.Of.The.Split.Images.In.MB") & SWMSplitFileSize & " MB" & CrLf &
                           ProgressLogText("Name.And.Path.Of.The.Target.SWM.File") & Quote & SWMSplitTargetFile & Quote & CrLf &
                           ProgressLogText("Check.Integrity.Before.Splitting.This.Image") & If(SWMSplitCheckIntegrity, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf & CrLf &
                           ProgressLogText("Do.Note.That.If.The.Image.Contains.A.2") & CrLf)
        ' Check the DISM version, as the Windows 7 version doesn't allow this action
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        ' Not supported
                    Case Is >= 2
                        CommandArgs &= " /split-image /imagefile=" & Quote & SWMSplitSourceFile & Quote & " /swmfile=" & Quote & SWMSplitTargetFile & Quote & " /filesize=" & SWMSplitFileSize & If(SWMSplitCheckIntegrity, " /checkintegrity", "")
                End Select
            Case 10
                CommandArgs &= " /split-image /imagefile=" & Quote & SWMSplitSourceFile & Quote & " /swmfile=" & Quote & SWMSplitTargetFile & Quote & " /filesize=" & SWMSplitFileSize & If(SWMSplitCheckIntegrity, " /checkintegrity", "")
        End Select
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
        Else
            LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
        End If
        GetErrorCode(False)
    End Sub

    Private Sub UnmountImage()
        DynaLog.LogMessage("Unmounting the Windows image...")
        DynaLog.LogMessage("- Mount directory of image to unmount: " & Quote & MountDir & Quote)
        DynaLog.LogMessage("- Image index: " & UMountImgIndex)
        DynaLog.LogMessage("- Unmount operation (may not reflect actual operation): " & UMountOp)
        DynaLog.LogMessage("  - Check image integrity before committing changes? " & If(CheckImgIntegrity, "Yes", "No"))
        DynaLog.LogMessage("  - Append changes to new index? " & If(SaveToNewIndex, "Yes", "No"))
                allTasks.Text = LocalizationService.ForSection("Progress.UnmountImage")("UnmountingImage.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.UnmountImage")("Unmounting.ImageFile.Button")
        If Not UMountLocalDir Then
            DynaLog.LogMessage("The image that was mounted in the project mount directory will not be unmounted. Using mountdir " & Quote & RandomMountDir & Quote & "...")
            MountDir = RandomMountDir
        End If
        LogView.AppendText(CrLf & ProgressLogText("Unmounting.Image.File.From.Mount.Point") & CrLf &
                           ProgressLogText("Mount.Directory") & MountDir & CrLf &
                           ProgressLogText("Image.Index") & UMountImgIndex)
        Try
            Select Case DismVersionChecker.ProductMajorPart
                Case 6
                    Select Case DismVersionChecker.ProductMinorPart
                        Case 1
                            If UMountLocalDir Then
                                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-wim /mountdir=" & Quote & MountDir & Quote
                            Else
                                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-wim /mountdir=" & Quote & RandomMountDir & Quote
                            End If
                        Case Is >= 2
                            If UMountLocalDir Then
                                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-image /mountdir=" & Quote & MountDir & Quote
                            Else
                                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-image /mountdir=" & Quote & RandomMountDir & Quote
                            End If
                    End Select
                Case 10
                    If UMountLocalDir Then
                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-image /mountdir=" & Quote & MountDir & Quote
                    Else
                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-image /mountdir=" & Quote & RandomMountDir & Quote
                    End If
            End Select
            If UMountOp = 0 Then
                LogView.AppendText(CrLf & ProgressLogText("Unmount.Operation.Commit"))
                CommandArgs &= " /commit"
            ElseIf UMountOp = 1 Then
                LogView.AppendText(CrLf & ProgressLogText("Unmount.Operation.Discard"))
                CommandArgs &= " /discard"
            End If
            If UMountOp = 0 Then
                If CheckImgIntegrity Then
                    LogView.AppendText(CrLf & ProgressLogText("Check.Image.Integrity.Yes"))
                    CommandArgs &= " /checkintegrity"
                Else
                    LogView.AppendText(CrLf & ProgressLogText("Check.Image.Integrity.No"))
                End If
                If SaveToNewIndex Then
                    LogView.AppendText(CrLf & ProgressLogText("Append.Changes.To.New.Index.Yes"))
                    CommandArgs &= " /append"
                Else
                    LogView.AppendText(CrLf & ProgressLogText("Append.Changes.To.New.Index.No"))
                End If
            End If
            RunProcess(DismProgram, CommandArgs)
        Catch ex As Exception
            ' Let's try this before setting things up here
        End Try
                currentTask.Text = LocalizationService.ForSection("Progress.UnmountImage")("Gathering.Error.Level.Item")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub


#End Region

#Region "Package/Feature Management Tasks"

    Private Sub ShowPackageInformation(pkgInfo As DismPackageInfo)
        LogView.AppendText(CrLf & CrLf &
                           ProgressLogText("Package.Name") & pkgInfo.PackageName & CrLf &
                           ProgressLogText("Package.Description") & pkgInfo.Description & CrLf &
                           ProgressLogText("Package.Release.Type") & Casters.CastDismReleaseType(pkgInfo.ReleaseType) & CrLf &
                           ProgressLogText("Package.Is.Applicable.To.This.Image") & If(pkgInfo.Applicable, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf &
                           ProgressLogText("Package.Is.Already.Installed") & If(pkgInfo.PackageState = DismPackageFeatureState.Installed Or pkgInfo.PackageState = DismPackageFeatureState.InstallPending, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf)
    End Sub

    Private Sub CountPackagesToAdd()
        If pkgAdditionOp = 0 Then
            DynaLog.LogMessage("Addition operation is recursive addition. Getting total amount of packages in source folder...")
            Try
                DynaLog.LogMessage("Getting CAB files (recursive operation)...")
                For Each CabPkg In My.Computer.FileSystem.GetFiles(pkgSource, FileIO.SearchOption.SearchAllSubDirectories, "*.cab")
                    pkgCount += 1
                Next
                DynaLog.LogMessage("Getting MSU files (recursive operation)...")
                For Each MsuPkg In My.Computer.FileSystem.GetFiles(pkgSource, FileIO.SearchOption.SearchAllSubDirectories, "*.msu")
                    pkgCount += 1
                Next
                DynaLog.LogMessage("Package count: " & pkgCount)
                LogView.AppendText(CrLf & ProgressLogText("Total.Number.Of.Packages") & pkgCount)
            Catch ex As Exception
                DynaLog.LogMessage("Could not get packages in all subdirectories. Error message: " & ex.Message)
                LogView.AppendText(CrLf & ProgressLogText("Exception") & ex.GetType().ToString() & ProgressLogText("Has.Occurred.While.Enumerating.Packages.Enumerating.Packages.In"))
                DynaLog.LogMessage("Getting CAB files...")
                For Each CabPkg In My.Computer.FileSystem.GetFiles(pkgSource, FileIO.SearchOption.SearchTopLevelOnly, "*.cab")
                    pkgCount += 1
                Next
                DynaLog.LogMessage("Getting MSU files...")
                For Each MsuPkg In My.Computer.FileSystem.GetFiles(pkgSource, FileIO.SearchOption.SearchTopLevelOnly, "*.msu")
                    pkgCount += 1
                Next
                DynaLog.LogMessage("Package count: " & pkgCount)
                LogView.AppendText(CrLf & ProgressLogText("Total.Number.Of.Packages") & pkgCount)
            End Try
        ElseIf pkgAdditionOp = 1 Then
            DynaLog.LogMessage("Addition operation is selective addition. A package count has already been obtained from the queue.")
            LogView.AppendText(CrLf & ProgressLogText("Total.Number.Of.Packages") & pkgCount)
        ElseIf pkgAdditionOp = 2 Then
            DynaLog.LogMessage("Addition operation is Update Manifest addition. Only 1 package will be added.")
            LogView.AppendText(CrLf & ProgressLogText("Total.Number.Of.Packages.1"))
        End If
    End Sub

    Private Sub AddPackagesRecursively(targetImage As String)
        CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /add-package /packagepath=" & Quote & pkgSource & Quote
        If pkgIgnoreApplicabilityChecks Then
            CommandArgs &= " /ignorecheck"
        End If
        If pkgPreventIfPendingOnline Then
            CommandArgs &= " /preventpending"
        End If
        RunProcess(DismProgram, CommandArgs)
                currentTask.Text = LocalizationService.ForSection("Progress.Packages.AddRecursive")("Gathering.Error.Level.Button")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
    End Sub

    Private Sub AddPackages(targetImage As String)
        DynaLog.LogMessage("Preparing to add packages...")
        DynaLog.LogMessage("- Package addition source: " & Quote & pkgSource & Quote)
        DynaLog.LogMessage("- Package addition operation: " & pkgAdditionOp)
        DynaLog.LogMessage("- Ignore applicability checks? " & If(pkgIgnoreApplicabilityChecks, "Yes", "No"))
        DynaLog.LogMessage("- Prevent addition if online operations are pending? " & If(pkgPreventIfPendingOnline, "Yes", "No"))
        DynaLog.LogMessage("- Save changes to the Windows image after finishing? " & If(pkgAdditionCommit, "Yes", "No"))
        ' Reset internal integers
        pkgCurrentNum = 0
                allTasks.Text = LocalizationService.ForSection("Progress.AddPackages")("AddingPackages.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.AddPackages")("Preparing.Packages.Button")
        LogView.AppendText(CrLf & ProgressLogText("Adding.Packages.To.Mounted.Image") & CrLf &
                           ProgressLogText("Package.Source") & pkgSource & CrLf)
        If pkgAdditionOp = 0 Then
            LogView.AppendText(ProgressLogText("Addition.Operation.Recursive"))
        ElseIf pkgAdditionOp = 1 Then
            LogView.AppendText(ProgressLogText("Addition.Operation.Selective"))
        End If
        If pkgIgnoreApplicabilityChecks Then
            LogView.AppendText(CrLf & ProgressLogText("Ignore.Applicability.Checks.Yes"))
        Else
            LogView.AppendText(CrLf & ProgressLogText("Ignore.Applicability.Checks.No"))
        End If
        If pkgPreventIfPendingOnline Then
            LogView.AppendText(CrLf & ProgressLogText("Prevent.Package.Addition.If.Online.Actions.Need.To") & CrLf &
                               ProgressLogText("NOTE.If.The.Mounted.Image.Requires.That.Online"))
        Else
            LogView.AppendText(CrLf & ProgressLogText("Prevent.Package.Addition.If.Online.Actions.Need.To.2"))
        End If
        If pkgAdditionCommit Then
            LogView.AppendText(CrLf & ProgressLogText("Commit.Image.After.Operations.Are.Done.Yes"))
        Else
            LogView.AppendText(CrLf & ProgressLogText("Commit.Image.After.Operations.Are.Done.No"))
        End If

        ' Perform package enumeration
        LogView.AppendText(CrLf & ProgressLogText("Enumerating.Packages.To.Add.Please.Wait"))
        CountPackagesToAdd()
        Thread.Sleep(2000)      ' Sleep to prevent thrashing

        ' Begin package addition
        currentTask.Text = LocalizationService.ForSection("Progress.AddPackages").Format("AddingPackages.Item", pkgCount)
        CurrentPB.Style = ProgressBarStyle.Blocks
        LogView.AppendText(CrLf & CrLf &
                           ProgressLogText("Processing") & pkgCount & ProgressLogText("Packages") & CrLf)
        If pkgAdditionOp = 0 Then
            DynaLog.LogMessage("Addition operation is recursive addition. DISM will scan the package source for packages to add.")
            AddPackagesRecursively(targetImage)
        ElseIf pkgAdditionOp = 1 Then
            DynaLog.LogMessage("Addition operation is selective addition. We are in control of the packages to add.")
            AddPackagesSelectively(targetImage)
        ElseIf pkgAdditionOp = 2 Then
            AddUpdateManifest(targetImage)
        End If
        Thread.Sleep(2000)
        If pkgAdditionCommit Then
            DynaLog.LogMessage("Preparing to save changes...")
            AllPB.Value = AllPB.Maximum / taskCount
            currentTCont += 1
            taskCountLbl.Text = LocalizationService.ForSection("Progress").Format("Tasks.Label", currentTCont, taskCount)
            RunOps(8)
        Else
            AllPB.Value = 100
        End If
        If pkgAdditionOp = 0 Then
            GetErrorCode(False)
        ElseIf (pkgAdditionOp = 1 Or pkgAdditionOp = 2) And pkgSuccessfulAdditions > 0 Then
            GetErrorCode(True)
        ElseIf (pkgAdditionOp = 1 Or pkgAdditionOp = 2) And pkgSuccessfulAdditions <= 0 Then
            GetErrorCode(False)
        End If
        If PackageErrorCodes.Contains("BC2") Then
            DynaLog.LogMessage("A system restart is needed to fully apply some packages.")
            LogView.AppendText(CrLf & ProgressLogText("Some.Packages.Require.A.System.Restart.To.Be"))
        End If
    End Sub

    Private Sub AddPackagesSelectively(targetImage As String)
        CurrentPB.Maximum = pkgCount
        For x = 0 To Array.LastIndexOf(pkgs, pkgLastCheckedPackageName)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            currentTask.Text = LocalizationService.ForSection("Progress.AddPackages").Format("AddingPackage.Item", x + 1, pkgCount)
            CurrentPB.Value = x + 1
            LogView.AppendText(CrLf &
                               ProgressLogText("Package") & (x + 1) & ProgressLogText("Of.Word") & pkgCount)        ' You don't want to see "Package 0 of 407", right?

            ' Get package information with the DISM API
            DynaLog.LogMessage("Getting information about package file " & Quote & Path.GetFileName(pkgs(x)) & Quote & "...")
            Dim pkgIsApplicable As Boolean
            Dim pkgIsInstalled As Boolean
            Try
                DynaLog.LogMessage("Extension of package file: " & Path.GetExtension(pkgs(x)))
                If Not Path.GetExtension(pkgs(x)).EndsWith("msu", StringComparison.OrdinalIgnoreCase) Then
                    DynaLog.LogMessage("Initializing API...")
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    DynaLog.LogMessage("Opening image session...")
                    Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                        DynaLog.LogMessage("Getting package information...")
                        Dim pkgInfo As DismPackageInfo = DismApi.GetPackageInfoByPath(imgSession, pkgs(x))
                        ShowPackageInformation(pkgInfo)
                        pkgIsApplicable = pkgInfo.Applicable
                        If pkgInfo.PackageState = DismPackageFeatureState.Installed Or pkgInfo.PackageState = DismPackageFeatureState.InstallPending Then pkgIsInstalled = True Else pkgIsInstalled = False
                        If pkgInfo.Applicable Then
                            DynaLog.LogMessage("The package can be added to the Windows image. Determining installation state of package...")
                            If pkgInfo.PackageState = DismPackageFeatureState.Installed Or pkgInfo.PackageState = DismPackageFeatureState.InstallPending Then
                                DynaLog.LogMessage("The package has already been added at some point.")
                                LogView.AppendText(CrLf & ProgressLogText("Package.Is.Already.Added.Skipping.Installation.Of.This"))
                                pkgFailedAdditions += 1
                            End If
                        Else
                            DynaLog.LogMessage("The package cannot be added to the Windows image as it is not applicable.")
                            If Not pkgIgnoreApplicabilityChecks Then
                                DynaLog.LogMessage("Applicability checks are not ignored.")
                                LogView.AppendText(CrLf & ProgressLogText("Package.Is.Not.Applicable.To.This.Image.Skipping"))
                                If PackageErrorCodes.Count <= 0 Then
                                    PackageErrorCodes.Add("0x800F8023")
                                Else
                                    PackageErrorCodes.Add("0x800F8023")
                                End If
                                pkgFailedAdditions += 1
                            End If
                        End If
                    End Using
                Else
                    LogView.AppendText(CrLf & ProgressLogText("The.Package.About.To.Be.Added.Is.A"))
                    ' Force these values to continue package addition
                    pkgIsApplicable = True
                    pkgIsInstalled = False
                End If
            Catch ex As Exception
                DynaLog.LogMessage("Could not get package information. Error message: " & ex.Message)
                DynaLog.LogMessage("Logging immediate failure...")
                LogView.AppendText(CrLf & ex.Message)
                If PackageErrorCodes.Count <= 0 Then
                    PackageErrorCodes.Add(If(Hex(ex.HResult).Length >= 8, "0x" & Hex(ex.HResult), Hex(ex.HResult)))
                Else
                    PackageErrorCodes.Add(If(Hex(ex.HResult).Length >= 8, "0x" & Hex(ex.HResult), Hex(ex.HResult)))
                End If
                pkgFailedAdditions += 1
                pkgIsApplicable = False
            Finally
                Try
                    DynaLog.LogMessage("Shutting down API...")
                    DismApi.Shutdown()
                Catch ex As Exception

                End Try
            End Try
            If Not pkgIsApplicable Or pkgIsInstalled Then Continue For
            DynaLog.LogMessage("The package is applicable and has not been installed yet. Adding it...")
            LogView.AppendText(CrLf & ProgressLogText("Processing.Package"))
            CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /add-package /packagepath=" & Quote & pkgs(x) & Quote
            If pkgIgnoreApplicabilityChecks Then
                CommandArgs &= " /ignorecheck"
            End If
            If pkgPreventIfPendingOnline Then
                CommandArgs &= " /preventpending"
            End If
            RunProcess(DismProgram, CommandArgs)
            LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
            GetPkgErrorLevel()
            LogView.AppendText(ProgressLogText("Error.Level.3") & errCode)
            If PackageErrorCodes.Count <= 0 Then
                PackageErrorCodes.Add(errCode)
            Else
                PackageErrorCodes.Add(errCode)
            End If
        Next
        CurrentPB.Value = CurrentPB.Maximum
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level.For.Selected.Packages") & CrLf)
        For x = 0 To PackageErrorCodes.Count - 1
            LogView.AppendText(CrLf & ProgressLogText("Package.No") & (x + 1) & ": " & PackageErrorCodes(x))
        Next
    End Sub

    Private Sub AddUpdateManifest(targetImage As String)
        DynaLog.LogMessage("Addition operation is Update Manifest addition.")
        CurrentPB.Maximum = pkgCount
        CommandArgs = BckArgs
        currentTask.Text = LocalizationService.ForSection("Progress.AddPackages").Format("AddingPackage.Item", 1, pkgCount)
        CurrentPB.Value = 1
        LogView.AppendText(CrLf & ProgressLogText("The.Package.About.To.Be.Added.Is.A.2"))
        LogView.AppendText(CrLf & ProgressLogText("Processing.Package"))
        CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /add-package /packagepath=" & Quote & pkgs(0) & Quote
        If pkgIgnoreApplicabilityChecks Then
            CommandArgs &= " /ignorecheck"
        End If
        If pkgPreventIfPendingOnline Then
            CommandArgs &= " /preventpending"
        End If
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
        GetPkgErrorLevel()
        LogView.AppendText(ProgressLogText("Error.Level.3") & errCode)
        If PackageErrorCodes.Count <= 0 Then
            PackageErrorCodes.Add(errCode)
        Else
            PackageErrorCodes.Add(errCode)
        End If
        CurrentPB.Value = CurrentPB.Maximum
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level.For.Selected.Packages") & CrLf)
        For x = 0 To PackageErrorCodes.Count - 1
            LogView.AppendText(CrLf & ProgressLogText("Package.No") & (x + 1) & ": " & PackageErrorCodes(x))
        Next
    End Sub

    Private Sub RemovePackages(targetImage As String)
        DynaLog.LogMessage("Preparing to remove packages...")
        DynaLog.LogMessage("- Package removal operation: " & pkgRemovalOp)
        DynaLog.LogMessage("- Amount of packages to remove: " & pkgRemovalCount)
                allTasks.Text = LocalizationService.ForSection("Progress.RemovePackages")("RemovingPackages.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.RemovePackages")("PrepareRemove.Button")
        LogView.AppendText(CrLf & ProgressLogText("Removing.Packages.From.Mounted.Image") & CrLf &
                           ProgressLogText("Enumerating.Packages.To.Remove.Please.Wait"))
        Thread.Sleep(1000)
        LogView.AppendText(CrLf & ProgressLogText("Amount.Of.Packages.To.Remove") & pkgRemovalCount)

        ' Begin package removal
                currentTask.Text = LocalizationService.ForSection("Progress.RemovePackages")("RemovingPackages.Item")
        CurrentPB.Maximum = pkgRemovalCount
        If pkgRemovalOp = 0 Then
            DynaLog.LogMessage("Packages that are installed will be removed from the Windows image.")
            RemoveInstalledPackages(targetImage)
        ElseIf pkgRemovalOp = 1 Then
            DynaLog.LogMessage("Package files will be removed from the Windows image.")
            DynaLog.LogMessage("It is likely that some specified packages may not be even installed in this image.")
            RemovePackageFiles(targetImage)
        End If
        Directory.Delete(Application.StartupPath & "\tempinfo", True)
        CurrentPB.Value = CurrentPB.Maximum
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level.For.Selected.Packages") & CrLf)
        For x = 0 To PackageErrorCodes.Count - 1
            LogView.AppendText(CrLf & ProgressLogText("Package.No") & (x + 1) & ": " & PackageErrorCodes(x))
        Next
        Thread.Sleep(2000)
        AllPB.Value = 100
        If pkgSuccessfulRemovals > 0 Then
            GetErrorCode(True)
        ElseIf pkgSuccessfulRemovals <= 0 Then
            GetErrorCode(False)
        End If
        If PackageErrorCodes.Contains("BC2") Then
            DynaLog.LogMessage("A system restart is needed to fully remove some packages.")
            LogView.AppendText(CrLf & ProgressLogText("Some.Packages.Require.A.System.Restart.To.Be"))
        End If
    End Sub

    Private Sub RemovePackageFiles(targetImage As String)
        For x = 0 To Array.LastIndexOf(pkgRemovalFiles, pkgRemovalLastFile)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            currentTask.Text = LocalizationService.ForSection("Progress.RemovePackages").Format("RemovingPackage.Item", x + 1, pkgRemovalCount)
            LogView.AppendText(CrLf &
                               ProgressLogText("Package") & (x + 1) & ProgressLogText("Of.Word") & pkgRemovalCount)
            CurrentPB.Value = x + 1
            Directory.CreateDirectory(Application.StartupPath & "\tempinfo")
            DynaLog.LogMessage("Getting information about package file " & Quote & Path.GetFileName(pkgRemovalFiles(x)) & Quote & "...")
            Dim pkgIsRemovable As Boolean
            Try
                DynaLog.LogMessage("Initializing API...")
                DismApi.Initialize(DismLogLevel.LogErrors)
                DynaLog.LogMessage("Opening image session...")
                Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                    DynaLog.LogMessage("Getting package information...")
                    Dim pkgInfo As DismPackageInfo = DismApi.GetPackageInfoByPath(imgSession, pkgRemovalFiles(x))
                    LogView.AppendText(CrLf & CrLf &
                                       ProgressLogText("Package.Name") & pkgInfo.PackageName & CrLf)
                    If pkgInfo.PackageState = DismPackageFeatureState.Installed Then
                        LogView.AppendText(ProgressLogText("Package.State.Installed") & CrLf)
                    ElseIf pkgInfo.PackageState = DismPackageFeatureState.UninstallPending Then
                        LogView.AppendText(ProgressLogText("Package.State.An.Uninstall.Is.Pending") & CrLf)
                    ElseIf pkgInfo.PackageState = DismPackageFeatureState.InstallPending Then
                        LogView.AppendText(ProgressLogText("Package.State.An.Install.Is.Pending") & CrLf)
                    End If
                    If pkgInfo.PackageState = DismPackageFeatureState.Installed Or pkgInfo.PackageState = DismPackageFeatureState.InstallPending Then
                        DynaLog.LogMessage("This package is either installed or about to be installed, and can be removed.")
                        pkgIsReadyForRemoval = True
                    Else
                        DynaLog.LogMessage("This package is neither installed nor about to be installed, and cannot be removed.")
                        pkgIsReadyForRemoval = False
                    End If
                End Using
                pkgIsRemovable = True
            Catch ex As Exception
                DynaLog.LogMessage("Could not get package information. Error message: " & ex.Message)
                DynaLog.LogMessage("Logging immediate failure...")
                LogView.AppendText(CrLf & ex.Message)
                If PackageErrorCodes.Count <= 0 Then
                    PackageErrorCodes.Add(If(Hex(ex.HResult).Length >= 8, "0x" & Hex(ex.HResult), Hex(ex.HResult)))
                Else
                    PackageErrorCodes.Add(If(Hex(ex.HResult).Length >= 8, "0x" & Hex(ex.HResult), Hex(ex.HResult)))
                End If
                pkgFailedRemovals += 1
                pkgIsRemovable = False
            Finally
                Try
                    DynaLog.LogMessage("Shutting down API...")
                    DismApi.Shutdown()
                Catch ex As Exception

                End Try
            End Try
            If Not pkgIsRemovable Then Continue For
            If pkgIsReadyForRemoval Then
                DynaLog.LogMessage("The package can be removed.")
                LogView.AppendText(CrLf & ProgressLogText("Processing.Package.Removal"))
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /remove-package /packagepath=" & pkgRemovalFiles(x)
                RunProcess(DismProgram, CommandArgs)
                LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
                errCode = Hex(Decimal.ToInt32(DismExitCode))
                If DismExitCode = 0 Then
                    pkgSuccessfulRemovals += 1
                Else
                    pkgFailedRemovals += 1
                End If
                If errCode.Length >= 8 Then
                    LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x.2") & errCode)
                Else
                    LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.2") & errCode)
                End If
                If PackageErrorCodes.Count <= 0 Then
                    If errCode.Length >= 8 Then
                        PackageErrorCodes.Add("0x" & errCode)
                    Else
                        PackageErrorCodes.Add(errCode)
                    End If
                Else
                    If errCode.Length >= 8 Then
                        PackageErrorCodes.Add("0x" & errCode)
                    Else
                        PackageErrorCodes.Add(errCode)
                    End If
                End If
            Else
                DynaLog.LogMessage("The package cannot be removed.")
                LogView.AppendText(CrLf & ProgressLogText("This.Package.Can.T.Be.Removed.Skipping.Removal"))
                pkgFailedRemovals += 1
                Continue For
            End If
        Next
    End Sub

    Private Sub RemoveInstalledPackages(targetImage As String)
        For x = 0 To Array.LastIndexOf(pkgRemovalNames, pkgRemovalLastName)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            currentTask.Text = LocalizationService.ForSection("Progress.RemovePackages").Format("RemovingPackage.Item", x + 1, pkgRemovalCount)
            LogView.AppendText(CrLf &
                               ProgressLogText("Package") & (x + 1) & ProgressLogText("Of.Word") & pkgRemovalCount)
            CurrentPB.Value = x + 1
            Directory.CreateDirectory(Application.StartupPath & "\tempinfo")

            DynaLog.LogMessage("Getting information about package file " & Quote & pkgRemovalNames(x) & Quote & "...")
            Dim pkgIsRemovable As Boolean
            Try
                DynaLog.LogMessage("Initializing API...")
                DismApi.Initialize(DismLogLevel.LogErrors)
                DynaLog.LogMessage("Opening image session...")
                Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                    DynaLog.LogMessage("Getting package information...")
                    Dim pkgInfo As DismPackageInfo = DismApi.GetPackageInfoByName(imgSession, pkgRemovalNames(x))
                    LogView.AppendText(CrLf & CrLf &
                                       ProgressLogText("Package.Name") & pkgInfo.PackageName & CrLf &
                                       ProgressLogText("Package.State") & Casters.CastDismPackageState(pkgInfo.PackageState))
                    If pkgInfo.PackageState = DismPackageFeatureState.Installed Or pkgInfo.PackageState = DismPackageFeatureState.InstallPending Then
                        DynaLog.LogMessage("This package is either installed or about to be installed, and can be removed.")
                        pkgIsReadyForRemoval = True
                    Else
                        DynaLog.LogMessage("This package is neither installed nor about to be installed, and cannot be removed.")
                        pkgIsReadyForRemoval = False
                    End If
                End Using
                pkgIsRemovable = True
            Catch ex As Exception
                DynaLog.LogMessage("Could not get package information. Error message: " & ex.Message)
                DynaLog.LogMessage("Logging immediate failure...")
                LogView.AppendText(CrLf & ex.Message)
                If PackageErrorCodes.Count <= 0 Then
                    PackageErrorCodes.Add(If(Hex(ex.HResult).Length >= 8, "0x" & Hex(ex.HResult), Hex(ex.HResult)))
                Else
                    PackageErrorCodes.Add(If(Hex(ex.HResult).Length >= 8, "0x" & Hex(ex.HResult), Hex(ex.HResult)))
                End If
                pkgFailedRemovals += 1
                pkgIsRemovable = False
            Finally
                Try
                    DynaLog.LogMessage("Shutting down API...")
                    DismApi.Shutdown()
                Catch ex As Exception

                End Try
            End Try
            If Not pkgIsRemovable Then Continue For
            If pkgIsReadyForRemoval Then
                DynaLog.LogMessage("The package can be removed.")
                LogView.AppendText(CrLf & ProgressLogText("Processing.Package.Removal"))
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /remove-package /packagename=" & pkgRemovalNames(x)
                RunProcess(DismProgram, CommandArgs)
                LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
                errCode = Hex(Decimal.ToInt32(DismExitCode))
                If DismExitCode = 0 Then
                    pkgSuccessfulRemovals += 1
                Else
                    pkgFailedRemovals += 1
                End If
                If errCode.Length >= 8 Then
                    LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x.2") & errCode)
                Else
                    LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.2") & errCode)
                End If
                If PackageErrorCodes.Count <= 0 Then
                    If errCode.Length >= 8 Then
                        PackageErrorCodes.Add("0x" & errCode)
                    Else
                        PackageErrorCodes.Add(errCode)
                    End If
                Else
                    If errCode.Length >= 8 Then
                        PackageErrorCodes.Add("0x" & errCode)
                    Else
                        PackageErrorCodes.Add(errCode)
                    End If
                End If
            Else
                DynaLog.LogMessage("The package cannot be removed.")
                LogView.AppendText(CrLf & ProgressLogText("This.Package.Can.T.Be.Removed.Skipping.Removal"))
                pkgFailedRemovals += 1
                Continue For
            End If
        Next
    End Sub

    Private Sub EnableFeatures(targetImage As String)
        DynaLog.LogMessage("Preparing to enable features...")
        DynaLog.LogMessage("- Will a parent package name be used? " & If(featisParentPkgNameUsed, "Yes", "No"))
        DynaLog.LogMessage("- Parent package name: " & Quote & featParentPkgName & Quote)
        DynaLog.LogMessage("- Has a source been specified? " & If(featisSourceSpecified, "Yes", "No"))
        DynaLog.LogMessage("- Feature source: " & Quote & featSource & Quote)
        DynaLog.LogMessage("- Will all parent features be enabled? " & If(featParentIsEnabled, "Yes", "No"))
        DynaLog.LogMessage("- Contact Windows Update for feature enablement (only for active installations)? " & If(featContactWindowsUpdate, "Yes", "No"))
        DynaLog.LogMessage("- Save changes to the Windows image after finishing? " & If(featEnablementCommit, "Yes", "No"))
                allTasks.Text = LocalizationService.ForSection("Progress.EnableFeatures")("EnablingFeatures.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.EnableFeatures")("PrepareEnable.Button")
        LogView.AppendText(CrLf & ProgressLogText("Enabling.Features") & CrLf &
                           ProgressLogText("Options") & CrLf)
        If featisParentPkgNameUsed Then
            LogView.AppendText(ProgressLogText("Use.Parent.Package.To.Enable.Features.Yes"))
        Else
            LogView.AppendText(ProgressLogText("Use.Parent.Package.To.Enable.Features.No"))
        End If
        If featParentPkgName = "" Then
            LogView.AppendText(CrLf & ProgressLogText("Parent.Package.Name.Not.Specified"))
        Else
            LogView.AppendText(CrLf & ProgressLogText("Parent.Package.Name") & Quote & featParentPkgName & Quote)
        End If
        If featisSourceSpecified Then
            LogView.AppendText(CrLf & ProgressLogText("Use.Feature.Source.Yes"))
        Else
            LogView.AppendText(CrLf & ProgressLogText("Use.Feature.Source.No"))
        End If
        If featSource = "" Then
            LogView.AppendText(CrLf & ProgressLogText("Feature.Source.Not.Specified"))
        Else
            LogView.AppendText(CrLf & ProgressLogText("Feature.Source") & Quote & featSource & Quote)
        End If
        If featParentIsEnabled Then
            LogView.AppendText(CrLf & ProgressLogText("Enable.All.Parent.Features.Yes"))
        Else
            LogView.AppendText(CrLf & ProgressLogText("Enable.All.Parent.Features.No"))
        End If
        DynaLog.LogMessage("Boot mode of the host system: " & SystemInformation.BootMode)
        If featContactWindowsUpdate And OnlineMgmt And SystemInformation.BootMode <> BootMode.FailSafe Then
            DynaLog.LogMessage("Host system is booted to normal mode or Safe Mode with networking.")
            LogView.AppendText(CrLf & ProgressLogText("Contact.Windows.Update.Yes"))
        ElseIf featContactWindowsUpdate And OnlineMgmt And SystemInformation.BootMode = BootMode.FailSafe Then
            DynaLog.LogMessage("Host system is booted to Safe Mode.")
            LogView.AppendText(CrLf & ProgressLogText("Contact.Windows.Update.No.The.System.Is.In"))
        ElseIf featContactWindowsUpdate And Not OnlineMgmt Then
            DynaLog.LogMessage("The active installation is not being managed.")
            LogView.AppendText(CrLf & ProgressLogText("Contact.Windows.Update.No.This.Is.Not.An"))
        Else
            LogView.AppendText(CrLf & ProgressLogText("Contact.Windows.Update.No"))
        End If
        If featEnablementCommit Then
            LogView.AppendText(CrLf & ProgressLogText("Commit.Image.After.Enabling.Features.Yes"))
        Else
            LogView.AppendText(CrLf & ProgressLogText("Commit.Image.After.Enabling.Features.No"))
        End If
        LogView.AppendText(CrLf & CrLf & ProgressLogText("Enumerating.Features.To.Enable"))
        Thread.Sleep(500)
        LogView.AppendText(CrLf & ProgressLogText("Total.Number.Of.Features.To.Enable") & featEnablementCount)
                currentTask.Text = LocalizationService.ForSection("Progress.EnableFeatures")("EnablingFeatures.Item")
        CurrentPB.Maximum = featEnablementCount
        For x = 0 To Array.LastIndexOf(featEnablementNames, featEnablementLastName)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            currentTask.Text = LocalizationService.ForSection("Progress.EnableFeatures").Format("EnablingFeature.Item", x + 1, featEnablementCount)
            LogView.AppendText(CrLf &
                               ProgressLogText("Feature") & (x + 1) & ProgressLogText("Of.Word") & featEnablementCount)
            CurrentPB.Value = x + 1
            DynaLog.LogMessage("Getting information about feature " & Quote & featEnablementNames(x).Replace("ListViewItem: ", "").Trim().Replace("{", "").Trim().Replace("}", "").Trim() & Quote & "...")
            Try
                DynaLog.LogMessage("Initializing API...")
                DismApi.Initialize(DismLogLevel.LogErrors)
                DynaLog.LogMessage("Opening image session...")
                Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                    DynaLog.LogMessage("Getting feature information...")
                    Dim featInfo As DismFeatureInfo = DismApi.GetFeatureInfo(imgSession, featEnablementNames(x).Replace("ListViewItem: ", "").Trim().Replace("{", "").Trim().Replace("}", "").Trim())
                    LogView.AppendText(CrLf & CrLf &
                                       ProgressLogText("Feature.Name") & featInfo.FeatureName & CrLf &
                                       ProgressLogText("Feature.Description") & featInfo.Description & CrLf)
                End Using
            Finally
                Try
                    DynaLog.LogMessage("Shutting down API...")
                    DismApi.Shutdown()
                Catch ex As Exception

                End Try
            End Try
            CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /enable-feature /featurename=" & featEnablementNames(x).Replace("ListViewItem: ", "").Trim().Replace("{", "").Trim().Replace("}", "").Trim()
            If featisParentPkgNameUsed And featParentPkgName <> "" Then
                CommandArgs &= " /packagename=" & featParentPkgName
            End If
            If featisSourceSpecified And featSource <> "" Then
                CommandArgs &= " /source=" & Quote & featSource & Quote
            End If
            If featParentIsEnabled Then
                CommandArgs &= " /all"
            End If
            If Not featContactWindowsUpdate And OnlineMgmt Then
                CommandArgs &= " /limitaccess"
            End If
            RunProcess(DismProgram, CommandArgs)
            LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
            GetFeatErrorLevel()
            If errCode.Length >= 8 Then
                LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
            Else
                LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
            End If
            If FeatureErrorCodes.Count <= 0 Then
                If errCode.Length >= 8 Then
                    FeatureErrorCodes.Add("0x" & errCode)
                Else
                    FeatureErrorCodes.Add(errCode)
                End If
            Else
                If errCode.Length >= 8 Then
                    FeatureErrorCodes.Add("0x" & errCode)
                Else
                    FeatureErrorCodes.Add(errCode)
                End If
            End If
        Next
        CurrentPB.Value = CurrentPB.Maximum
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level.For.Selected.Features") & CrLf)
        For x = 0 To FeatureErrorCodes.Count - 1
            LogView.AppendText(CrLf & ProgressLogText("Feature.No") & (x + 1) & ": " & FeatureErrorCodes(x))
        Next
        Thread.Sleep(2000)
        If featEnablementCommit Then
            DynaLog.LogMessage("Preparing to save changes...")
            AllPB.Value = AllPB.Maximum / taskCount
            currentTCont += 1
            taskCountLbl.Text = LocalizationService.ForSection("Progress").Format("Tasks.Label", currentTCont, taskCount)
            RunOps(8)
        Else
            AllPB.Value = 100
        End If
        If featSuccessfulEnablements > 0 Then
            GetErrorCode(True)
        ElseIf featSuccessfulEnablements <= 0 Then
            GetErrorCode(False)
        End If
        If FeatureErrorCodes.Contains("BC2") Then
            DynaLog.LogMessage("A system restart is needed to fully apply some features.")
            LogView.AppendText(CrLf & ProgressLogText("Some.Features.Require.A.System.Restart.To.Be"))
        End If
    End Sub

    Private Sub DisableFeatures(targetImage As String)
        DynaLog.LogMessage("Preparing to disable features...")
        DynaLog.LogMessage("- Will a parent package name be used? " & If(featDisablementParentPkgUsed, "Yes", "No"))
        DynaLog.LogMessage("- Parent package name: " & Quote & featDisablementParentPkg & Quote)
        DynaLog.LogMessage("- Remove feature manifest? " & If(featDisablementRemoveManifest, "Yes", "No"))
                allTasks.Text = LocalizationService.ForSection("Progress.DisableFeatures")("Disabling.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.DisableFeatures")("PrepareDisable.Button")
        LogView.AppendText(CrLf & ProgressLogText("Disabling.Features") & CrLf &
                           ProgressLogText("Options") & CrLf)
        If featDisablementParentPkgUsed Then
            LogView.AppendText(ProgressLogText("Use.Parent.Package.To.Disable.Features.Yes"))
        Else
            LogView.AppendText(ProgressLogText("Use.Parent.Package.To.Disable.Features.No"))
        End If
        If featDisablementParentPkg = "" Then
            LogView.AppendText(CrLf & ProgressLogText("Parent.Package.Name.Not.Specified"))
        Else
            LogView.AppendText(CrLf & ProgressLogText("Parent.Package.Name") & Quote & featDisablementParentPkg & Quote)
        End If
        If featDisablementRemoveManifest Then
            LogView.AppendText(CrLf & ProgressLogText("Remove.Feature.Manifest.Yes"))
        Else
            LogView.AppendText(CrLf & ProgressLogText("Remove.Feature.Manifest.No"))
        End If
        LogView.AppendText(CrLf & CrLf & ProgressLogText("Enumerating.Features.To.Disable"))
        Thread.Sleep(500)
        LogView.AppendText(CrLf & ProgressLogText("Total.Number.Of.Features.To.Disable") & featDisablementCount)
                currentTask.Text = LocalizationService.ForSection("Progress.DisableFeatures")("Disabling.Item")
        CurrentPB.Maximum = featDisablementCount
        For x = 0 To Array.LastIndexOf(featDisablementNames, featDisablementLastName)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            currentTask.Text = LocalizationService.ForSection("Progress.DisableFeatures").Format("DisablingFeature.Item", x + 1, featDisablementCount)
            LogView.AppendText(CrLf &
                               ProgressLogText("Feature") & (x + 1) & ProgressLogText("Of.Word") & featDisablementCount)
            CurrentPB.Value = x + 1
            DynaLog.LogMessage("Getting information about feature " & Quote & featDisablementNames(x).Replace("ListViewItem: ", "").Trim().Replace("{", "").Trim().Replace("}", "").Trim() & Quote & "...")
            Try
                DynaLog.LogMessage("Initializing API...")
                DismApi.Initialize(DismLogLevel.LogErrors)
                DynaLog.LogMessage("Opening image session...")
                Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                    DynaLog.LogMessage("Getting feature information...")
                    Dim featInfo As DismFeatureInfo = DismApi.GetFeatureInfo(imgSession, featDisablementNames(x).Replace("ListViewItem: ", "").Trim().Replace("{", "").Trim().Replace("}", "").Trim())
                    LogView.AppendText(CrLf & CrLf &
                                       ProgressLogText("Feature.Name") & featInfo.FeatureName & CrLf &
                                       ProgressLogText("Feature.Description") & featInfo.Description & CrLf)

                End Using
            Finally
                Try
                    DynaLog.LogMessage("Shutting down API...")
                    DismApi.Shutdown()
                Catch ex As Exception

                End Try
            End Try
            CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /disable-feature /featurename=" & featDisablementNames(x).Replace("ListViewItem: ", "").Trim().Replace("{", "").Trim().Replace("}", "").Trim()
            If featDisablementParentPkgUsed And featDisablementParentPkg <> "" Then
                CommandArgs &= " /packagename=" & featParentPkgName
            End If
            If Not featDisablementRemoveManifest Then
                CommandArgs &= " /remove"
            End If
            RunProcess(DismProgram, CommandArgs)
            LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
            errCode = Hex(Decimal.ToInt32(DismExitCode))
            If DismExitCode = 0 Then
                featSuccessfulDisablements += 1
            Else
                featFailedDisablements += 1
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
            Else
                LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
            End If
            If FeatureErrorCodes.Count <= 0 Then
                If errCode.Length >= 8 Then
                    FeatureErrorCodes.Add("0x" & errCode)
                Else
                    FeatureErrorCodes.Add(errCode)
                End If
            Else
                If errCode.Length >= 8 Then
                    FeatureErrorCodes.Add("0x" & errCode)
                Else
                    FeatureErrorCodes.Add(errCode)
                End If
            End If
        Next
        CurrentPB.Value = CurrentPB.Maximum
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level.For.Selected.Features") & CrLf)
        For x = 0 To FeatureErrorCodes.Count - 1
            LogView.AppendText(CrLf & ProgressLogText("Feature.No") & (x + 1) & ": " & FeatureErrorCodes(x))
        Next
        Thread.Sleep(2000)
        If featSuccessfulDisablements > 0 Then
            GetErrorCode(True)
        ElseIf featSuccessfulDisablements <= 0 Then
            GetErrorCode(False)
        End If
        If FeatureErrorCodes.Contains("BC2") Then
            DynaLog.LogMessage("A system restart is needed to fully apply some features.")
            LogView.AppendText(CrLf & ProgressLogText("Some.Features.Require.A.System.Restart.To.Be"))
        End If
    End Sub

    Private Sub CleanupImage(targetImage As String)
        DynaLog.LogMessage("Preparing to clean up the image...")
        DynaLog.LogMessage("Cleanup task: " & CleanupTask)
                allTasks.Text = LocalizationService.ForSection("Progress.CleanupImage")("Cleaning.Up.Image.Button")
        CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /cleanup-image"
        Select Case CleanupTask
            Case 0
                DynaLog.LogMessage("Reverting pending servicing actions to a last known good state...")
                        currentTask.Text = LocalizationService.ForSection("Progress.CleanupImage")("RevertPending.Button")
                LogView.AppendText(CrLf &
                                   ProgressLogText("Reverting.Pending.Servicing.Actions"))
                CommandArgs &= " /revertpendingactions"
            Case 1
                DynaLog.LogMessage("Cleaning up Service Pack backup files...")
                DynaLog.LogMessage("- Hide Service Packs from Installed Updates list? " & If(CleanupHideSP, "Yes", "No"))
                        currentTask.Text = LocalizationService.ForSection("Progress.CleanupImage")("Cleaning.Up.ServicePack.Item")
                LogView.AppendText(CrLf &
                                   ProgressLogText("Cleaning.Up.Service.Pack.Backup.Files") & CrLf &
                                   ProgressLogText("Options") & CrLf &
                                   ProgressLogText("Hide.Service.Packs.From.The.Installed.Updates.List") & If(CleanupHideSP, ProgressLogText("Yes"), ProgressLogText("No")))
                CommandArgs &= " /spsuperseded" & If(CleanupHideSP, " /hidesp", "")
            Case 2
                DynaLog.LogMessage("Cleaning up component store...")
                DynaLog.LogMessage("- Reset superseded component base? " & If(ResetCompBase, "Yes", "No"))
                DynaLog.LogMessage("- Defer long operations? " & If(DeferCleanupOps, "Yes", "No"))
                        currentTask.Text = LocalizationService.ForSection("Progress.CleanupImage")("Cleaning.Up.Component.Item")
                LogView.AppendText(CrLf &
                                   ProgressLogText("Cleaning.Up.The.Component.Store") & CrLf &
                                   ProgressLogText("Options") & CrLf &
                                   ProgressLogText("Perform.Superseded.Component.Base.Reset") & If(ResetCompBase, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf &
                                   ProgressLogText("Defer.Long.Running.Operations") & If(DeferCleanupOps, ProgressLogText("Yes"), ProgressLogText("No")))
                CommandArgs &= " /startcomponentcleanup" & If(ResetCompBase, " /resetbase", "") & If(ResetCompBase And DeferCleanupOps, " /defer", "")
            Case 3
                DynaLog.LogMessage("Analyzing component store...")
                        currentTask.Text = LocalizationService.ForSection("Progress.CleanupImage")("Analyzing.Component.Item")
                LogView.AppendText(CrLf &
                                   ProgressLogText("Analyzing.The.Component.Store"))
                CommandArgs &= " /analyzecomponentstore"
            Case 4
                DynaLog.LogMessage("Checking component store health...")
                        currentTask.Text = LocalizationService.ForSection("Progress.CleanupImage")("Checking.Comp.Store.Item")
                LogView.AppendText(CrLf &
                                   ProgressLogText("Checking.The.Component.Store.Health"))
                CommandArgs &= " /checkhealth"
            Case 5
                DynaLog.LogMessage("Scanning component store...")
                        currentTask.Text = LocalizationService.ForSection("Progress.CleanupImage")("Scanning.Component.Item")
                LogView.AppendText(CrLf &
                                   ProgressLogText("Scanning.The.Component.Store"))
                CommandArgs &= " /scanhealth"
            Case 6
                DynaLog.LogMessage("Repairing component store...")
                DynaLog.LogMessage("- Source: " & Quote & ComponentRepairSource & Quote)
                DynaLog.LogMessage("- Limit Windows Update access (only for active installations)? " & If(LimitWUAccess, "Yes", "No"))
                DynaLog.LogMessage("Boot mode of host system: " & SystemInformation.BootMode)
                ' The most known thing about DISM : dism /online /cleanup-image /restorehealth
                        currentTask.Text = LocalizationService.ForSection("Progress.CleanupImage")("Repairing.Component.Item")
                LogView.AppendText(CrLf &
                                   ProgressLogText("Repairing.The.Component.Store") & CrLf &
                                   ProgressLogText("Options") & CrLf &
                                   ProgressLogText("Use.Different.Source") & If(UseCompRepairSource, ProgressLogText("Yes.2") & Quote & ComponentRepairSource & Quote & ")", ProgressLogText("No")) & CrLf &
                                   ProgressLogText("Limit.Windows.Update.Access") & If(LimitWUAccess And OnlineMgmt, ProgressLogText("Yes"), If(LimitWUAccess And Not OnlineMgmt, ProgressLogText("No.This.Is.Not.An.Online.Installation"), ProgressLogText("No"))) &
                                   If(Not LimitWUAccess And OnlineMgmt And SystemInformation.BootMode = BootMode.FailSafe, ProgressLogText("The.System.Is.In.Safe.Mode"), ""))
                CommandArgs &= " /restorehealth" & If(UseCompRepairSource And File.Exists(ComponentRepairSource), " /source=" & Quote & ComponentRepairSource & Quote, "") & If(LimitWUAccess And OnlineMgmt, " /limitaccess", "")
        End Select
        RunProcess(DismProgram, CommandArgs)
                currentTask.Text = LocalizationService.ForSection("Progress.CleanupImage")("Gathering.Error.Level.Item")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

#End Region

#Region "Provisioning Package Management Tasks"

    Private Sub AddProvisioningPackage(targetImage As String)
        DynaLog.LogMessage("Preparing to add provisioning package to the Windows image...")
        DynaLog.LogMessage("- Provisioning package: " & Quote & ppkgAdditionPackagePath & Quote)
        DynaLog.LogMessage("- Catalog path: " & Quote & ppkgAdditionCatalogPath & Quote)
        DynaLog.LogMessage("- Commit image after finishing? " & If(ppkgAdditionCommit, "Yes", "No"))
                allTasks.Text = LocalizationService.ForSection("Progress.ProvPackage.Add")("AddingPackage.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.ProvPackage.Add")("Image.Button")
        LogView.AppendText(ProgressLogText("Adding.Provisioning.Package.To.The.Image") & CrLf &
                           ProgressLogText("Options") & CrLf & CrLf &
                           ProgressLogText("Provisioning.Package") & Quote & ppkgAdditionPackagePath & Quote & CrLf &
                           ProgressLogText("Catalog.File") & If(ppkgAdditionCatalogPath = "", ProgressLogText("None.Specified.2"), Quote & ppkgAdditionCatalogPath & Quote) & CrLf &
                           ProgressLogText("Commit.Image.After.Adding.Provisioning.Package") & If(ppkgAdditionCommit, ProgressLogText("Yes"), ProgressLogText("No")))
        CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /add-provisioningpackage /packagepath=" & Quote & ppkgAdditionPackagePath & Quote & If(ppkgAdditionCatalogPath <> "" And File.Exists(ppkgAdditionCatalogPath), " /catalogpath=" & Quote & ppkgAdditionCatalogPath & Quote, "")
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
        Else
            LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
        End If
        If ppkgAdditionCommit Then
            DynaLog.LogMessage("Preparing to save changes...")
            AllPB.Value = AllPB.Maximum / taskCount
            currentTCont += 1
            taskCountLbl.Text = LocalizationService.ForSection("Progress").Format("Tasks.Label", currentTCont, taskCount)
            RunOps(8)
        Else
            AllPB.Value = 100
        End If
        GetErrorCode(False)
    End Sub

#End Region

#Region "AppX Package Management Tasks"

    Private Sub AddProvisionedAppxPackages(targetImage As String)
        DynaLog.LogMessage("Preparing to add provisioned AppX packages...")
                allTasks.Text = LocalizationService.ForSection("Progress.ProvAppx.Add")("AddingPackages.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.ProvAppx.Add")("Preparing.Button")
        LogView.AppendText(CrLf & ProgressLogText("Adding.Provisioned.APPX.Packages") & CrLf &
                           ProgressLogText("Options") & CrLf)
        If appxAdditionUseLicenseFile Then
            LogView.AppendText(ProgressLogText("Use.A.License.File.For.APPX.Packages.Yes") & CrLf &
                               ProgressLogText("License.File") & appxAdditionLicenseFile & CrLf)
        Else
            LogView.AppendText(ProgressLogText("Use.A.License.File.For.APPX.Packages.No") & CrLf &
                               ProgressLogText("License.File.Not.Using") & CrLf)
        End If
        If appxAdditionUseCustomDataFile Then
            LogView.AppendText(ProgressLogText("Use.A.Custom.Data.File.For.APPX.Packages") & CrLf &
                               ProgressLogText("Custom.Data.File") & appxAdditionCustomDataFile & CrLf)
        Else
            LogView.AppendText(ProgressLogText("Use.A.Custom.Data.File.For.APPX.Packages.2") & CrLf &
                               ProgressLogText("Custom.Data.File.Not.Using") & CrLf)
        End If
        If appxAdditionUseAllRegions Then
            LogView.AppendText(ProgressLogText("Use.All.Regions.For.APPX.Packages.Yes") & CrLf &
                               ProgressLogText("Package.Regions.All") & CrLf)
        Else
            LogView.AppendText(ProgressLogText("Use.All.Regions.For.APPX.Packages.No") & CrLf &
                               ProgressLogText("Package.Regions") & Quote & appxAdditionRegions & Quote & CrLf)
        End If
        If appxAdditionCommit Then
            LogView.AppendText(ProgressLogText("Commit.Image.After.Adding.APPX.Packages.Yes"))
        Else
            LogView.AppendText(ProgressLogText("Commit.Image.After.Adding.APPX.Packages.No"))
        End If
        LogView.AppendText(CrLf & CrLf & ProgressLogText("Enumerating.APPX.Packages.To.Add"))
        Thread.Sleep(500)
        LogView.AppendText(CrLf & ProgressLogText("Total.Number.Of.Packages.To.Add") & appxAdditionCount)
                currentTask.Text = LocalizationService.ForSection("Progress.ProvAppx.Add")("AddingPackages.Item")
        CurrentPB.Maximum = appxAdditionCount
        For x = 0 To Array.LastIndexOf(appxAdditionPackages, appxAdditionLastPackage)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            currentTask.Text = LocalizationService.ForSection("Progress.ProvAppx.Add").Format("AddingPackage.Item", x + 1, appxAdditionCount)
            LogView.AppendText(CrLf &
                               ProgressLogText("Package") & (x + 1) & ProgressLogText("Of.Word") & appxAdditionCount)
            CurrentPB.Value = x + 1
            DynaLog.LogMessage("Information about the AppX package:")
            DynaLog.LogMessage(appxAdditionPackageList(x).ToString())
            LogView.AppendText(CrLf &
                               ProgressLogText("APPX.Package.File") & appxAdditionPackageList(x).PackageFile & CrLf &
                               ProgressLogText("Application.Name") & appxAdditionPackageList(x).PackageName & CrLf &
                               ProgressLogText("Application.Publisher") & appxAdditionPackageList(x).PackagePublisher & CrLf &
                               ProgressLogText("Application.Version") & appxAdditionPackageList(x).PackageVersion & CrLf)
            ' Detect if it is an encrypted application
            DynaLog.LogMessage("Extension of AppX package: " & Path.GetExtension(appxAdditionPackageList(x).PackageFile))
            If Path.GetExtension(appxAdditionPackageList(x).PackageFile).Replace(".", "").Trim().StartsWith("e", StringComparison.OrdinalIgnoreCase) AndAlso OnlineMgmt Then
                DynaLog.LogMessage("The application is encrypted and the active installation is being managed. Adding package using PowerShell...")
                ' Run PowerShell command. Support will be improved
                LogView.AppendText(CrLf & ProgressLogText("The.Application.About.To.Be.Added.Is.An") & CrLf)
                Dim AppxAuxProc As New Process()
                AppxAuxProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
                CommandArgs = "-Command Add-AppxPackage -Path '" & appxAdditionPackageList(x).PackageFile & "'"
                AppxAuxProc.StartInfo.Arguments = CommandArgs
                AppxAuxProc.Start()
                AppxAuxProc.WaitForExit()
                LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
                If Hex(AppxAuxProc.ExitCode).Length < 8 Then
                    errCode = AppxAuxProc.ExitCode
                Else
                    errCode = Hex(AppxAuxProc.ExitCode)
                End If
                If AppxAuxProc.ExitCode = 0 Then
                    appxSuccessfulAdditions += 1
                Else
                    appxFailedAdditions += 1
                End If
                If errCode.Length >= 8 Then
                    LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
                Else
                    LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
                End If
                If PackageErrorCodes.Count <= 0 Then
                    If errCode.Length >= 8 Then
                        PackageErrorCodes.Add("0x" & errCode)
                    Else
                        PackageErrorCodes.Add(errCode)
                    End If
                Else
                    If errCode.Length >= 8 Then
                        PackageErrorCodes.Add("0x" & errCode)
                    Else
                        PackageErrorCodes.Add(errCode)
                    End If
                End If
                Continue For
            ElseIf Path.GetExtension(appxAdditionPackageList(x).PackageFile).Replace(".", "").Trim().StartsWith("e", StringComparison.OrdinalIgnoreCase) AndAlso Not OnlineMgmt Then
                DynaLog.LogMessage("The application is encrypted but the active installation is not being managed.")
                ' Continue loop without installing application
                LogView.AppendText(CrLf & ProgressLogText("The.Application.About.To.Be.Added.Is.An.2") & CrLf)
                Continue For
            Else
                DynaLog.LogMessage("The application is not encrypted. Continuing addition...")
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /add-provisionedappxpackage "
                If (File.GetAttributes(appxAdditionPackageList(x).PackageFile) And FileAttributes.Directory) = FileAttributes.Directory Then
                    CommandArgs &= "/folderpath=" & Quote & appxAdditionPackageList(x).PackageFile & Quote
                Else
                    CommandArgs &= "/packagepath=" & Quote & appxAdditionPackageList(x).PackageFile & Quote
                End If
                If appxAdditionPackageList(x).PackageLicenseFile <> "" And File.Exists(appxAdditionPackageList(x).PackageLicenseFile) Then
                    DynaLog.LogMessage("A license file has been specified and it exists in the file system.")
                    CommandArgs &= " /licensepath=" & Quote & appxAdditionPackageList(x).PackageLicenseFile & Quote
                Else
                    DynaLog.LogMessage("Either no license file has been specified or it does not exist in the file system.")
                    If appxAdditionPackageList(x).PackageLicenseFile <> "" Then
                        LogView.AppendText(CrLf &
                                           ProgressLogText("Warning.The.License.File.Does.Not.Exist.Continuing") & CrLf &
                                           ProgressLogText("Do.Note.That.If.This.App.Requires.A") & CrLf &
                                           ProgressLogText("Also.This.May.Compromise.The.Image"))
                    End If
                    CommandArgs &= " /skiplicense"
                End If
                ' Inform user that a package will be installed with dependencies
                DynaLog.LogMessage("Count of dependencies: " & appxAdditionPackageList(x).PackageSpecifiedDependencies.Count)
                If appxAdditionPackageList(x).PackageSpecifiedDependencies.Count > 0 Then
                    LogView.AppendText(ProgressLogText("The.Following.Dependency.Packages.Will.Be.Installed.Alongside") & CrLf)
                End If
                ' Add dependencies
                For Each Dependency As AppxDependency In appxAdditionPackageList(x).PackageSpecifiedDependencies
                    DynaLog.LogMessage("Verifying if dependency " & Quote & Path.GetFileName(Dependency.DependencyFile) & Quote & " exists...")
                    If File.Exists(Dependency.DependencyFile) Then
                        DynaLog.LogMessage("The dependency exists in the file system.")
                        LogView.AppendText(ProgressLogText("Dependency") & Quote & Path.GetFileName(Dependency.DependencyFile) & Quote & CrLf)
                        CommandArgs &= " /dependencypackagepath=" & Quote & Dependency.DependencyFile & Quote
                    Else
                        DynaLog.LogMessage("The dependency does not exist in the file system.")
                        LogView.AppendText(CrLf &
                                           ProgressLogText("Warning.The.Dependency") & CrLf &
                                           Quote & Dependency.DependencyFile & Quote & CrLf &
                                           ProgressLogText("Does.Not.Exist.In.The.File.System.Skipping"))
                        Continue For
                    End If
                Next
                If appxAdditionPackageList(x).PackageCustomDataFile <> "" And File.Exists(appxAdditionPackageList(x).PackageCustomDataFile) Then
                    DynaLog.LogMessage("A custom data file has been specified and it exists in the file system.")
                    CommandArgs &= " /customdatapath=" & Quote & appxAdditionCustomDataFile & Quote
                ElseIf appxAdditionPackageList(x).PackageCustomDataFile <> "" And Not File.Exists(appxAdditionPackageList(x).PackageCustomDataFile) Then
                    DynaLog.LogMessage("A custom data file has been specified but it does not exist in the file system.")
                    LogView.AppendText(CrLf &
                                       ProgressLogText("Warning.The.Custom.Data.File.Does.Not.Exist"))
                End If
                If (FileVersionInfo.GetVersionInfo(DismProgram).ProductMajorPart = 10 And FileVersionInfo.GetVersionInfo(DismProgram).ProductBuildPart >= 17134) And
                   (ImgVersion.Major = 10 And ImgVersion.Build >= 17134) Then
                    DynaLog.LogMessage("All conditions are met for region configuration (DISM version >= 10.0.17134 ; Image version >= 10.0.17134). Configuring regions...")
                    If appxAdditionPackageList(x).PackageRegions = "" Then
                        DynaLog.LogMessage("The application will be configured for all regions.")
                        CommandArgs &= " /region:all"
                    Else
                        DynaLog.LogMessage("The application will be configured for specific regions.")
                        CommandArgs &= " /region:" & Quote & appxAdditionPackageList(x).PackageRegions & Quote
                    End If
                End If
                If (FileVersionInfo.GetVersionInfo(DismProgram).ProductMajorPart >= 10 And ImgVersion.Major >= 10) And appxAdditionPackageList(x).SupportsStub Then
                    DynaLog.LogMessage("All conditions are met for stub package configuration (DISM version >= 10.0 ; Image version >= 10.0). Configuring stub package preferences...")
                    Select Case appxAdditionPackageList(x).StubPackageOption
                        Case StubPreference.NoPreference
                            DynaLog.LogMessage("No preference has been set for the stub package.")
                            ' Don't add stub package option flag
                        Case StubPreference.StubOnly
                            DynaLog.LogMessage("The stub package will be installed.")
                            CommandArgs &= " /stubpackageoption:installstub"
                        Case StubPreference.FullPackage
                            DynaLog.LogMessage("The full package will be installed.")
                            CommandArgs &= " /stubpackageoption:installfull"
                    End Select
                End If
                RunProcess(DismProgram, CommandArgs)
            End If
            LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
            If Hex(DismExitCode).Length < 8 Then
                errCode = DismExitCode
            Else
                errCode = Hex(DismExitCode)
            End If
            If DismExitCode = 0 Then
                appxSuccessfulAdditions += 1
            Else
                appxFailedAdditions += 1
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
            Else
                LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
            End If
            If PackageErrorCodes.Count <= 0 Then
                If errCode.Length >= 8 Then
                    PackageErrorCodes.Add("0x" & errCode)
                Else
                    PackageErrorCodes.Add(errCode)
                End If
            Else
                If errCode.Length >= 8 Then
                    PackageErrorCodes.Add("0x" & errCode)
                Else
                    PackageErrorCodes.Add(errCode)
                End If
            End If
        Next
        CurrentPB.Value = CurrentPB.Maximum
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level.For.Selected.APPX.Packages") & CrLf)
        For x = 0 To PackageErrorCodes.Count - 1
            LogView.AppendText(CrLf & ProgressLogText("Package.No") & (x + 1) & ": " & PackageErrorCodes(x))
        Next
        Thread.Sleep(2000)
        If appxAdditionCommit Then
            DynaLog.LogMessage("Preparing to save changes...")
            AllPB.Value = AllPB.Maximum / taskCount
            currentTCont += 1
            taskCountLbl.Text = LocalizationService.ForSection("Progress").Format("Tasks.Label", currentTCont, taskCount)
            RunOps(8)
        Else
            AllPB.Value = 100
        End If
        If appxSuccessfulAdditions > 0 Then
            GetErrorCode(True)
        ElseIf appxSuccessfulAdditions <= 0 Then
            GetErrorCode(False)
        End If
    End Sub

    Private Sub CheckAppRegistrationStatus(removalStoreApp As String)
        DynaLog.LogMessage("Checking if package " & Quote & removalStoreApp & Quote & " is registered to a user...")
        If Directory.Exists(MountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & removalStoreApp) Then
            If My.Computer.FileSystem.GetFiles(MountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & removalStoreApp, FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").Count = 0 Then
                DynaLog.LogMessage(".pckgdep files for AppX package " & Quote & removalStoreApp & Quote & " = 0. This app is not registered to a user")
                ' Application is not registered to any user
                LogView.AppendText(CrLf &
                                   ProgressLogText("Application.Is.Registered.To.A.User.No"))
            Else
                DynaLog.LogMessage(".pckgdep files for AppX package " & Quote & removalStoreApp & Quote & " > 0. This app is registered to users")
                ' Application is registered to a user
                LogView.AppendText(CrLf &
                                   ProgressLogText("Application.Is.Registered.To.A.User.Yes") & CrLf &
                                   ProgressLogText("The.Removal.Of.This.Application.May.Require.You"))
            End If
        Else
            DynaLog.LogMessage(".pckgdep files for AppX package " & Quote & removalStoreApp & Quote & " = 0. This app is not registered to a user")
            ' Application is not registered to any user
            LogView.AppendText(CrLf &
                               ProgressLogText("Application.Is.Registered.To.A.User.No"))
        End If
    End Sub

    Private Sub RemoveOnlineAppxPackages(ParamArray PackageNames As String())
        Dim extAppxHelperPath As String = Path.Combine(Application.StartupPath, "bin", "extps1", "online_appx_removal.ps1")
        If File.Exists(extAppxHelperPath) Then
            DynaLog.LogMessage("AppX removal helper exists. Proceeding with the removal of those bastards!")
            LogView.AppendText(CrLf & ProgressLogText("A.PowerShell.Helper.Will.Be.Used.To.Remove"))
            RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "WindowsPowerShell", "v1.0", "powershell.exe"),
                       String.Format("-executionpolicy Bypass -noprofile -nologo -file {0}{1}{0} -appxFullNames {0}{2}{0}", Quote, extAppxHelperPath,
                                     String.Join(";", PackageNames.Where(Function(PackageName) Not String.IsNullOrEmpty(PackageName)))))
            LogView.AppendText(CrLf & ProgressLogText("Log.Off.For.The.Deprovisioning.Of.Applications.To"))
        End If
    End Sub

    Private Sub RemoveProvisionedAppxPackages(targetImage As String)
        DynaLog.LogMessage("Preparing to remove AppX packages...")
                allTasks.Text = LocalizationService.ForSection("Progress.ProvAppx.Remove")("RemovingPackages.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.ProvAppx.Remove")("Preparing.Button")
        LogView.AppendText(CrLf & ProgressLogText("Removing.Provisioned.APPX.Packages") & CrLf & CrLf &
                           ProgressLogText("Enumerating.APPX.Packages.To.Remove"))
        Thread.Sleep(500)
        LogView.AppendText(CrLf & ProgressLogText("Total.Number.Of.Packages.To.Remove") & appxRemovalCount)
                currentTask.Text = LocalizationService.ForSection("Progress.ProvAppx.Remove")("RemovingPackages.Item")
        CurrentPB.Maximum = appxRemovalCount
        If OnlineMgmt Then
            RemoveOnlineAppxPackages(appxRemovalPackages)
            CurrentPB.Value = CurrentPB.Maximum
            Thread.Sleep(2000)
            AllPB.Value = 100
            GetErrorCode(True)
        Else
            For x = 0 To Array.LastIndexOf(appxRemovalPackages, appxRemovalLastPackage)
                If x + 1 > CurrentPB.Maximum Then Exit For
                CommandArgs = BckArgs
                Dim removalStoreApp As String = appxRemovalPackages(x)
                currentTask.Text = LocalizationService.ForSection("Progress.ProvAppx.Remove").Format("RemovingPackage.Item", x + 1, appxRemovalCount)
                LogView.AppendText(CrLf &
                                   ProgressLogText("Package") & (x + 1) & ProgressLogText("Of.Word") & appxRemovalCount)
                CurrentPB.Value = x + 1
                ' Display package name and DisplayName
                LogView.AppendText(CrLf &
                                   ProgressLogText("Package.Name") & appxRemovalPackages(x) & CrLf &
                                   ProgressLogText("Display.Name") & appxRemovalPkgNames(x))
                ' Display whether an application is registered to a user
                CheckAppRegistrationStatus(removalStoreApp)
                ' Initialize command. Its syntax is simple, so don't spend too much time determining options
                LogView.AppendText(CrLf & CrLf &
                                   ProgressLogText("Processing.Package"))
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /remove-provisionedappxpackage /packagename=" & appxRemovalPackages(x)
                RunProcess(DismProgram, CommandArgs)
                LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
                If Hex(DismExitCode).Length < 8 Then
                    errCode = DismExitCode
                Else
                    errCode = Hex(DismExitCode)
                End If
                If DismExitCode = 0 Then
                    appxSuccessfulRemovals += 1
                Else
                    appxFailedRemovals += 1
                End If
                If errCode.Length >= 8 Then
                    LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
                Else
                    LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
                End If
                If PackageErrorCodes.Count <= 0 Then
                    If errCode.Length >= 8 Then
                        PackageErrorCodes.Add("0x" & errCode)
                    Else
                        PackageErrorCodes.Add(errCode)
                    End If
                Else
                    If errCode.Length >= 8 Then
                        PackageErrorCodes.Add("0x" & errCode)
                    Else
                        PackageErrorCodes.Add(errCode)
                    End If
                End If
            Next
            CurrentPB.Value = CurrentPB.Maximum
            LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level.For.Selected.APPX.Packages") & CrLf)
            For x = 0 To PackageErrorCodes.Count - 1
                LogView.AppendText(CrLf & ProgressLogText("Package.No") & (x + 1) & ": " & PackageErrorCodes(x))
            Next
            Thread.Sleep(2000)
            AllPB.Value = 100
            If appxSuccessfulRemovals > 0 Then
                GetErrorCode(True)
            ElseIf appxSuccessfulRemovals <= 0 Then
                GetErrorCode(False)
            End If
        End If

    End Sub

#End Region

#Region "Language Management Tasks"

    Private Sub SetKeyboardLayeredDriver(targetImage As String)
        DynaLog.LogMessage("Preparing to set keyboard layered driver...")
        DynaLog.LogMessage("Type of new keyboard layered driver: " & KeyboardLayeredDriverType)
                allTasks.Text = LocalizationService.ForSection("Progress.LayeredDriver")("SettingDriver.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.LayeredDriver")("Setting.Keyboard.Button")
        currentLay = New KeyboardDrivers(currentKeybLayeredDriverType).LayeredDriver
        newKeybLay = New KeyboardDrivers(KeyboardLayeredDriverType).LayeredDriver
        Dim currentLayout As String = ""
        Dim newLayout As String = ""
        Select Case currentLay
            Case KeyboardDrivers.LayeredKeyboardDriver.Unknown
                currentLayout = "Unknown/Not installed"
            Case KeyboardDrivers.LayeredKeyboardDriver.PCATKey
                currentLayout = "PC/AT Enhanced Keyboard (101/102-Key)"
            Case KeyboardDrivers.LayeredKeyboardDriver.K_PCATKeyT1
                currentLayout = "Korean PC/AT 101-Key Compatible Keyboard/MS Natural Keyboard (Type 1)"
            Case KeyboardDrivers.LayeredKeyboardDriver.K_PCATKeyT2
                currentLayout = "Korean PC/AT 101-Key Compatible Keyboard/MS Natural Keyboard (Type 2)"
            Case KeyboardDrivers.LayeredKeyboardDriver.K_PCATKeyT3
                currentLayout = "Korean PC/AT 101-Key Compatible Keyboard/MS Natural Keyboard (Type 3)"
            Case KeyboardDrivers.LayeredKeyboardDriver.K_103106Key
                currentLayout = "Korean Keyboard (103/106 Key)"
            Case KeyboardDrivers.LayeredKeyboardDriver.J_106109Key
                currentLayout = "Japanese Keyboard (106/109 Key)"
        End Select
        Select Case newKeybLay
            Case KeyboardDrivers.LayeredKeyboardDriver.Unknown
                newLayout = "Unknown/Not installed"
            Case KeyboardDrivers.LayeredKeyboardDriver.PCATKey
                newLayout = "PC/AT Enhanced Keyboard (101/102-Key)"
            Case KeyboardDrivers.LayeredKeyboardDriver.K_PCATKeyT1
                newLayout = "Korean PC/AT 101-Key Compatible Keyboard/MS Natural Keyboard (Type 1)"
            Case KeyboardDrivers.LayeredKeyboardDriver.K_PCATKeyT2
                newLayout = "Korean PC/AT 101-Key Compatible Keyboard/MS Natural Keyboard (Type 2)"
            Case KeyboardDrivers.LayeredKeyboardDriver.K_PCATKeyT3
                newLayout = "Korean PC/AT 101-Key Compatible Keyboard/MS Natural Keyboard (Type 3)"
            Case KeyboardDrivers.LayeredKeyboardDriver.K_103106Key
                newLayout = "Korean Keyboard (103/106 Key)"
            Case KeyboardDrivers.LayeredKeyboardDriver.J_106109Key
                newLayout = "Japanese Keyboard (106/109 Key)"
        End Select
        LogView.AppendText(CrLf & ProgressLogText("Setting.The.Keyboard.Layered.Driver") & CrLf &
                           ProgressLogText("Current.Keyboard.Layered.Driver") & currentLayout & CrLf &
                           ProgressLogText("New.Keyboard.Layered.Driver") & newLayout & CrLf)
        CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /set-layereddriver:" & KeyboardLayeredDriverType
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
        Else
            LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
        End If
        GetErrorCode(False)
    End Sub

#End Region

#Region "Capability Management Tasks"

    Private Sub AddCapabilities(targetImage As String)
        DynaLog.LogMessage("Preparing to add capabilities...")
        DynaLog.LogMessage("- Has a source been specified? " & If(capAdditionUseSource, "Yes", "No"))
        DynaLog.LogMessage("- Capability source: " & Quote & capAdditionSource & Quote)
        DynaLog.LogMessage("- Limit Windows Update access (only for active installations)? " & If(capAdditionLimitWUAccess, "Yes", "No"))
        DynaLog.LogMessage("- Save changes to the Windows image after finishing? " & If(capAdditionCommit, "Yes", "No"))
                allTasks.Text = LocalizationService.ForSection("Progress.AddCapabilities")("Add.Capabilities.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.AddCapabilities")("PrepareAdd.Button")
        DynaLog.LogMessage("Boot mode of the host system: " & SystemInformation.BootMode)
        LogView.AppendText(CrLf & ProgressLogText("Adding.Capabilities.To.Mounted.Image") & CrLf &
                           ProgressLogText("Options") & CrLf &
                           ProgressLogText("Use.A.Source.For.Capability.Addition") & If(capAdditionUseSource, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf &
                           ProgressLogText("Capability.Source") & If(capAdditionUseSource, Quote & capAdditionSource & Quote, ProgressLogText("No.Source.Has.Been.Provided")) & CrLf &
                           ProgressLogText("Limit.Access.To.Windows.Update") & If(capAdditionLimitWUAccess And OnlineMgmt, ProgressLogText("Yes"), If(capAdditionLimitWUAccess And Not OnlineMgmt, ProgressLogText("No.This.Is.Not.An.Online.Installation"), ProgressLogText("No"))) & If(Not capAdditionLimitWUAccess And OnlineMgmt And SystemInformation.BootMode = BootMode.FailSafe, ProgressLogText("The.System.Is.In.Safe.Mode"), "") & CrLf &
                           ProgressLogText("Commit.Image.After.Adding.Capabilities") & If(capAdditionCommit, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf)
        If capAdditionUseSource And Not Directory.Exists(capAdditionSource) Then
            DynaLog.LogMessage("A source is expected to be used but it does not exist in the file system.")
            LogView.AppendText(CrLf &
                               ProgressLogText("Warning.The.Specified.Source.Does.Not.Exist.In"))
        End If
                currentTask.Text = LocalizationService.ForSection("Progress.AddCapabilities")("Add.Capabilities.Item")
        LogView.AppendText(CrLf & ProgressLogText("Enumerating.Capabilities.To.Add.Please.Wait") & CrLf &
                           ProgressLogText("Total.Number.Of.Capabilities") & capAdditionCount)
        CurrentPB.Maximum = capAdditionCount
        For x = 0 To Array.LastIndexOf(capAdditionIds, capAdditionLastId)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            currentTask.Text = LocalizationService.ForSection("Progress.AddCapabilities").Format("AddingCapability.Item", x + 1, capAdditionCount)
            CurrentPB.Value = x + 1
            DynaLog.LogMessage("Getting information about capability " & Quote & capAdditionIds(x) & Quote & "...")
            LogView.AppendText(CrLf &
                               ProgressLogText("Capability") & (x + 1) & ProgressLogText("Of.Word") & capAdditionCount)
            ' Get capability information
            ' Try opening the session. If API is not initialized, initialize it
            Try
                DynaLog.LogMessage("Initializing API...")
                DismApi.Initialize(DismLogLevel.LogErrors)
                DynaLog.LogMessage("Opening image session...")
                Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                    DynaLog.LogMessage("Getting capability information...")
                    ' Get capability information
                    Dim capInfo As DismCapabilityInfo = DismApi.GetCapabilityInfo(imgSession, capAdditionIds(x))
                    LogView.AppendText(CrLf & CrLf &
                                       ProgressLogText("Capability.Identity") & capInfo.Name & CrLf &
                                       ProgressLogText("Capability.Name") & capInfo.DisplayName & CrLf &
                                       ProgressLogText("Capability.Description") & capInfo.Description & CrLf)
                End Using
            Finally
                Try
                    DynaLog.LogMessage("Shutting down API...")
                    DismApi.Shutdown()
                Catch ex As Exception

                End Try
            End Try
            CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /add-capability /capabilityname=" & capAdditionIds(x)
            If capAdditionUseSource And Directory.Exists(capAdditionSource) Then
                CommandArgs &= " /source=" & Quote & capAdditionSource & Quote
            End If
            If capAdditionLimitWUAccess And OnlineMgmt Then CommandArgs &= " /limitaccess"
            RunProcess(DismProgram, CommandArgs)
            LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
            errCode = Hex(Decimal.ToInt32(DismExitCode))
            If DismExitCode = 0 Then
                capSuccessfulAdditions += 1
            Else
                capFailedAdditions += 1
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
            Else
                LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
            End If
            If FeatureErrorCodes.Count <= 0 Then
                If errCode.Length >= 8 Then
                    FeatureErrorCodes.Add("0x" & errCode)
                Else
                    FeatureErrorCodes.Add(errCode)
                End If
            Else
                If errCode.Length >= 8 Then
                    FeatureErrorCodes.Add("0x" & errCode)
                Else
                    FeatureErrorCodes.Add(errCode)
                End If
            End If
        Next
        CurrentPB.Value = CurrentPB.Maximum
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level.For.Selected.Capabilities") & CrLf)
        For x = 0 To FeatureErrorCodes.Count - 1
            LogView.AppendText(CrLf & ProgressLogText("Capability.No") & (x + 1) & ": " & FeatureErrorCodes(x))
        Next
        Thread.Sleep(2000)
        If capAdditionCommit Then
            DynaLog.LogMessage("Preparing to save changes...")
            AllPB.Value = AllPB.Maximum / taskCount
            currentTCont += 1
            taskCountLbl.Text = LocalizationService.ForSection("Progress").Format("Tasks.Label", currentTCont, taskCount)
            RunOps(8)
        End If
        If capSuccessfulAdditions > 0 Then
            GetErrorCode(True)
        ElseIf capSuccessfulAdditions <= 0 Then
            GetErrorCode(False)
        End If
        If FeatureErrorCodes.Contains("BC2") Then
            DynaLog.LogMessage("A system restart is needed to fully apply some capabilities.")
            LogView.AppendText(CrLf & ProgressLogText("Some.Capabilities.Require.A.System.Restart.To.Be"))
        End If
    End Sub

    Private Sub RemoveCapabilities(targetImage As String)
        DynaLog.LogMessage("Preparing to remove capabilities...")
                allTasks.Text = LocalizationService.ForSection("Progress.RemoveCapabilities")("Remove.Capabilities.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.RemoveCaps")("Preparing.Button")
        LogView.AppendText(CrLf & ProgressLogText("Removing.Capabilities.From.Mounted.Image") & CrLf)
                currentTask.Text = LocalizationService.ForSection("Progress.RemoveCapabilities")("Remove.Capabilities.Item")
        LogView.AppendText(CrLf & ProgressLogText("Enumerating.Capabilities.To.Remove.Please.Wait") & CrLf &
                           ProgressLogText("Total.Number.Of.Capabilities") & capRemovalCount)
        CurrentPB.Maximum = capRemovalCount
        For x = 0 To Array.LastIndexOf(capRemovalIds, capRemovalLastId)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            currentTask.Text = LocalizationService.ForSection("Progress.RemoveCapabilities").Format("Capability.Item", x + 1, capRemovalCount)
            DynaLog.LogMessage("Getting information about capability " & Quote & capRemovalIds(x) & Quote & "...")
            CurrentPB.Value = x + 1
            LogView.AppendText(CrLf &
                               ProgressLogText("Capability") & (x + 1) & ProgressLogText("Of.Word") & capRemovalCount)
            Try
                DynaLog.LogMessage("Initializing API...")
                DismApi.Initialize(DismLogLevel.LogErrors)
                DynaLog.LogMessage("Opening image session...")
                Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                    DynaLog.LogMessage("Getting capability information...")
                    Dim capInfo As DismCapabilityInfo = DismApi.GetCapabilityInfo(imgSession, capRemovalIds(x))
                    LogView.AppendText(CrLf & CrLf &
                                       ProgressLogText("Capability.Identity") & capInfo.Name & CrLf &
                                       ProgressLogText("Capability.Name") & capInfo.DisplayName & CrLf &
                                       ProgressLogText("Capability.Description") & capInfo.Description & CrLf)
                End Using
            Finally
                Try
                    DynaLog.LogMessage("Shutting down API...")
                    DismApi.Shutdown()
                Catch ex As Exception

                End Try
            End Try
            CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /remove-capability /capabilityname=" & capRemovalIds(x)
            RunProcess(DismProgram, CommandArgs)
            LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
            errCode = Hex(Decimal.ToInt32(DismExitCode))
            If DismExitCode = 0 Then
                capSuccessfulRemovals += 1
            Else
                capFailedRemovals += 1
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
            Else
                LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
            End If
            If FeatureErrorCodes.Count <= 0 Then
                If errCode.Length >= 8 Then
                    FeatureErrorCodes.Add("0x" & errCode)
                Else
                    FeatureErrorCodes.Add(errCode)
                End If
            Else
                If errCode.Length >= 8 Then
                    FeatureErrorCodes.Add("0x" & errCode)
                Else
                    FeatureErrorCodes.Add(errCode)
                End If
            End If
        Next
        CurrentPB.Value = CurrentPB.Maximum
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level.For.Selected.Capabilities") & CrLf)
        For x = 0 To FeatureErrorCodes.Count - 1
            LogView.AppendText(CrLf & ProgressLogText("Capability.No") & (x + 1) & ": " & FeatureErrorCodes(x))
        Next
        Thread.Sleep(2000)
        If capSuccessfulRemovals > 0 Then
            GetErrorCode(True)
        ElseIf capSuccessfulRemovals <= 0 Then
            GetErrorCode(False)
        End If
        If FeatureErrorCodes.Contains("BC2") Then
            DynaLog.LogMessage("A system restart is needed to fully remove some capabilities.")
            LogView.AppendText(CrLf & ProgressLogText("Some.Capabilities.Require.A.System.Restart.To.Be"))
        End If
    End Sub

#End Region

#Region "Edition Management Tasks"

    Private Sub SetImageEdition(targetImage As String)
        DynaLog.LogMessage("Preparing image edition upgrade...")
        DynaLog.LogMessage("- New Edition: " & imgEditionNewEdition)
        DynaLog.LogMessage("- Copy the EULA? " & If(imgEditionCopyEula, "Yes", "No"))
        DynaLog.LogMessage("- EULA destination (if chosen to copy the EULA): " & imgEditionEulaDestination)
        DynaLog.LogMessage("- Accept the EULA? " & If(imgEditionAcceptEula, "Yes", "No"))
        DynaLog.LogMessage("- Product key (if chosen to accept the EULA): " & imgEditionEditionKey)
        allTasks.Text = LocalizationService.ForSection("Progress.Operation")("UpgradingImage.Label")
        currentTask.Text = LocalizationService.ForSection("Progress.Operation")("Setting.New.Image.Label")
        LogView.AppendText(CrLf & ProgressLogText("Setting.The.New.Image.Edition") & CrLf &
                           ProgressLogText("Options") & CrLf &
                           ProgressLogText("New.Edition") & imgEditionNewEdition & CrLf &
                           ProgressLogText("Will.The.EULA.Be.Copied") & If(imgEditionCopyEula, ProgressLogText("Yes.To.The.Following.Destination") & imgEditionEulaDestination, ProgressLogText("No")) & CrLf &
                           ProgressLogText("Will.The.EULA.Be.Accepted") & If(imgEditionAcceptEula, ProgressLogText("Yes.With.The.Following.Product.Key") & imgEditionEditionKey, ProgressLogText("No")) & CrLf)
        CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /set-edition=" & imgEditionNewEdition
        DynaLog.LogMessage("Checking if the active installation is being managed...")
        If OnlineMgmt Then
            DynaLog.LogMessage("The active installation is being managed. Taking into account other settings the user may have specified...")
            If imgEditionCopyEula Then
                CommandArgs &= " /geteula=" & Quote & imgEditionEulaDestination & Quote
            ElseIf imgEditionAcceptEula Then
                CommandArgs &= " /accepteula /productkey=" & imgEditionEditionKey
            End If
        Else
            DynaLog.LogMessage("The active installation is not being managed. Ignoring other settings...")
        End If
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
        Else
            LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
        End If
        GetErrorCode(False)
    End Sub

    Private Sub SetImageProductKey(targetImage As String)
        DynaLog.LogMessage("Preparing to set the product key...")
        DynaLog.LogMessage("- New Product Key: " & pkSetNewProductKey)
        allTasks.Text = LocalizationService.ForSection("Progress.Operation")("Setting.ProductKey.Label")
        currentTask.Text = LocalizationService.ForSection("Progress.Operation")("Setting.New.ProductKey.Label")
        LogView.AppendText(CrLf & ProgressLogText("Setting.The.New.Product.Key") & CrLf &
                           ProgressLogText("Options") & CrLf &
                           ProgressLogText("New.Product.Key") & pkSetNewProductKey & CrLf)
        CommandArgs &= " /image=" & targetImage & " /norestart /set-productkey=" & pkSetNewProductKey
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
        Else
            LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
        End If
        GetErrorCode(False)
    End Sub

#End Region

#Region "Driver Management Tasks"

    Private Sub AddDrivers(targetImage As String)
        DynaLog.LogMessage("Preparing to add OS drivers...")
        DynaLog.LogMessage("- Force installation of unsigned drivers? " & If(drvAdditionForceUnsigned, "Yes", "No"))
        DynaLog.LogMessage("- Save changes to the Windows image after finishing? " & If(drvAdditionCommit, "Yes", "No"))
                allTasks.Text = LocalizationService.ForSection("Progress.AddDrivers")("AddingDrivers.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.AddDrivers")("Preparing.Drivers.Button")
        LogView.AppendText(CrLf & ProgressLogText("Adding.Driver.Packages.To.Mounted.Image") & CrLf &
                           ProgressLogText("Options") & CrLf &
                           ProgressLogText("Force.Installation.Of.Unsigned.Drivers") & If(drvAdditionForceUnsigned, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf &
                           ProgressLogText("Commit.Image.After.Adding.Driver.Packages") & If(drvAdditionCommit, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf)
        If drvAdditionForceUnsigned Then
            LogView.AppendText(CrLf &
                               ProgressLogText("Warning.The.Option.To.Force.Installation.Of.Unsigned"))
        End If
                currentTask.Text = LocalizationService.ForSection("Progress.AddDrivers")("AddingDrivers.Item")
        LogView.AppendText(CrLf & ProgressLogText("Enumerating.Drivers.To.Add.Please.Wait") & CrLf &
                           ProgressLogText("Total.Number.Of.Drivers") & drvAdditionCount)
        CurrentPB.Maximum = drvAdditionCount
        For x = 0 To Array.LastIndexOf(drvAdditionPkgs, drvAdditionLastPkg)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            currentTask.Text = LocalizationService.ForSection("Progress.AddDrivers").Format("AddingDriver.Item", x + 1, drvAdditionCount)
            CurrentPB.Value = x + 1
            LogView.AppendText(CrLf &
                               ProgressLogText("Driver") & (x + 1) & ProgressLogText("Of.Word") & drvAdditionCount)
            ' Get driver information
            DynaLog.LogMessage("Checking file system attributes of driver...")
            If Not (File.GetAttributes(drvAdditionPkgs(x)) And FileAttributes.Directory) = FileAttributes.Directory Then
                DynaLog.LogMessage("The driver is not a folder.")
                DynaLog.LogMessage("Getting information about driver file " & Quote & Path.GetFileName(drvAdditionPkgs(x)) & Quote & "...")
                Try
                    DynaLog.LogMessage("Initializing API...")
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    DynaLog.LogMessage("Opening image session...")
                    Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                        DynaLog.LogMessage("Getting driver information...")
                        Dim drvInfoCollection As DismDriverCollection = DismApi.GetDriverInfo(imgSession, drvAdditionPkgs(x))
                        DynaLog.LogMessage("Information collection count: " & drvInfoCollection.Count)
                        If drvInfoCollection.Count > 0 And drvInfoCollection.Count <= 10 Then
                            For Each drvInfo As DismDriver In drvInfoCollection
                                LogView.AppendText(CrLf & CrLf &
                                                   ProgressLogText("Hardware.Description") & drvInfo.HardwareDescription & CrLf &
                                                   ProgressLogText("Hardware.ID") & drvInfo.HardwareId & CrLf &
                                                   ProgressLogText("Additional.IDs") & CrLf &
                                                   ProgressLogText("Compatible.IDs") & drvInfo.CompatibleIds & CrLf &
                                                   ProgressLogText("Excluded.IDs") & drvInfo.ExcludeIds & CrLf &
                                                   ProgressLogText("Hardware.Manufacturer") & drvInfo.ManufacturerName & CrLf &
                                                   ProgressLogText("Hardware.Architecture") & Casters.CastDismArchitecture(drvInfo.Architecture))
                            Next
                        ElseIf drvInfoCollection.Count > 10 Then
                            DynaLog.LogMessage("The driver information contains more than 10 hardware targets.")
                            LogView.AppendText(CrLf & CrLf &
                                               ProgressLogText("This.Driver.File.Targets.More.Than.10.Devices") & CrLf &
                                               ProgressLogText("If.You.Want.To.Get.Information.Of.This") & CrLf & CrLf &
                                               "    " & Path.GetFileName(drvAdditionPkgs(x)))
                        Else
                            LogView.AppendText(CrLf & CrLf &
                                               ProgressLogText("We.Couldn.T.Get.Information.Of.This.Driver"))
                        End If
                    End Using
                Finally
                    Try
                        DynaLog.LogMessage("Shutting down API...")
                        DismApi.Shutdown()
                    Catch ex As Exception

                    End Try
                End Try
            Else
                DynaLog.LogMessage("The driver is a folder. It will be processed recursively.")
                LogView.AppendText(CrLf & CrLf &
                                   ProgressLogText("The.Driver.Package.Currently.About.To.Be.Processed"))
            End If
            DynaLog.LogMessage("Checking current operating mode...")
            Dim isRecursive As Boolean = (File.GetAttributes(drvAdditionPkgs(x)) And FileAttributes.Directory) = FileAttributes.Directory And drvAdditionFolderRecursiveScan.Contains(drvAdditionPkgs(x))
            If OnlineMgmt Then
                DynaLog.LogMessage("Online installation management mode detected. Using PNPUTIL to add the driver...")
                ' Much like deleting drivers with PNPUTIL, said tool changed syntax in Windows 10
                DynaLog.LogMessage("Checking pnputil version...")
                Dim pnpUtilArgs As String = ""
                Try
                    Dim pnputilVersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "pnputil.exe"))
                    DynaLog.LogMessage("PNPUTIL version info: " & pnputilVersionInfo.FileVersion)
                    If pnputilVersionInfo.FileMajorPart >= 10 Then
                        DynaLog.LogMessage("System PNPUTIL comes from Windows 10 or newer.")
                        pnpUtilArgs = String.Format("/add-driver {0} /install", If(isRecursive, Quote & drvAdditionPkgs(x) & "\*.inf" & Quote & " /subdirs", Quote & drvAdditionPkgs(x) & Quote))
                    Else
                        DynaLog.LogMessage("System PNPUTIL comes from Windows 8.")

                        ' NT6 pnputil does not support recursive driver package addition like NT10 pnputil, in that it does not support
                        ' the /subdirs parameter of the NT10 pnputil. Thus, we have to intervene with INF file enumeration.
                        If isRecursive Then
                            For Each InfFile In Directory.EnumerateFiles(drvAdditionPkgs(x), "*.inf", SearchOption.AllDirectories)
                                pnpUtilArgs = String.Format("-i -a {0}", Quote & InfFile & Quote)
                                RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "pnputil.exe"),
                                           pnpUtilArgs, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32"), True)
                            Next
                        Else
                            pnpUtilArgs = String.Format("-i -a {0}", Quote & drvAdditionPkgs(x) & Quote)
                            RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "pnputil.exe"),
                                       pnpUtilArgs, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32"), True)
                        End If
                    End If
                Catch ex As Exception
                    DynaLog.LogMessage("An error occurred with this method. Error message: " & ex.Message & " (exit code " & Hex(ex.HResult) & "). Since it's our only way of removing drivers in this mode, signal an error message")
                    DismExitCode = ex.HResult
                End Try
            Else
                DynaLog.LogMessage("Online installation management mode not detected. Using DISM to add the driver...")
                CommandArgs &= " /image=" & targetImage & " /add-driver /driver=" & Quote & drvAdditionPkgs(x) & Quote
                If drvAdditionForceUnsigned Then
                    CommandArgs &= " /forceunsigned"
                End If
                If isRecursive Then
                    LogView.AppendText(CrLf & ProgressLogText("This.Folder.Will.Be.Scanned.Recursively.Driver.Addition"))
                    CommandArgs &= " /recurse"
                End If
                RunProcess(DismProgram, CommandArgs)
            End If
            LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
            errCode = Hex(Decimal.ToInt32(DismExitCode))
            If DismExitCode = 0 Then
                drvSuccessfulAdditions += 1
            Else
                drvFailedAdditions += 1
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
            Else
                LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
            End If
            If PackageErrorCodes.Count <= 0 Then
                If errCode.Length >= 8 Then
                    PackageErrorCodes.Add("0x" & errCode)
                Else
                    PackageErrorCodes.Add(errCode)
                End If
            Else
                If errCode.Length >= 8 Then
                    PackageErrorCodes.Add("0x" & errCode)
                Else
                    PackageErrorCodes.Add(errCode)
                End If
            End If
        Next
        CurrentPB.Value = CurrentPB.Maximum
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level.For.Selected.Drivers") & CrLf)
        For x = 0 To PackageErrorCodes.Count - 1
            LogView.AppendText(CrLf & ProgressLogText("Driver.No") & (x + 1) & ": " & PackageErrorCodes(x))
        Next
        Thread.Sleep(2000)
        If drvAdditionCommit Then
            DynaLog.LogMessage("Preparing to save changes...")
            AllPB.Value = AllPB.Maximum / taskCount
            currentTCont += 1
            taskCountLbl.Text = LocalizationService.ForSection("Progress").Format("Tasks.Label", currentTCont, taskCount)
            RunOps(8)
        End If
        If drvSuccessfulAdditions > 0 Then
            GetErrorCode(True)
        ElseIf drvSuccessfulAdditions <= 0 Then
            GetErrorCode(False)
        End If
    End Sub

    Private Sub GetThirdPartyDrivers()
        Try
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            DynaLog.LogMessage("Opening image session...")
            Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                drvCollection = DismApi.GetDrivers(imgSession, AllDrivers)
            End Using
            DynaLog.LogMessage("Information collection count: " & drvCollection.Count)
        Finally
            Try
                DynaLog.LogMessage("Shutting down API...")
                DismApi.Shutdown()
            Catch ex As Exception

            End Try
        End Try
    End Sub

    Private Sub RemoveDrivers(targetImage As String)
        DynaLog.LogMessage("Preparing to remove OS drivers...")
                allTasks.Text = LocalizationService.ForSection("Progress.RemoveDrivers")("RemovingDrivers.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.RemoveDrivers")("Preparing.Drivers.Button")
        LogView.AppendText(CrLf & ProgressLogText("Removing.Driver.Packages.From.Mounted.Image") & CrLf)
        ' Get all driver packages
        DynaLog.LogMessage("Getting drivers of the Windows image... This can take some time, depending on the amount of drivers installed.")
        LogView.AppendText(CrLf & ProgressLogText("Getting.Image.Drivers.This.May.Take.Some.Time") & CrLf)
        GetThirdPartyDrivers()
                currentTask.Text = LocalizationService.ForSection("Progress.RemoveDrivers")("RemovingDrivers.Item")
        LogView.AppendText(CrLf & ProgressLogText("Enumerating.Drivers.To.Remove.Please.Wait") & CrLf &
                           ProgressLogText("Total.Number.Of.Drivers") & drvRemovalCount)
        CurrentPB.Maximum = drvRemovalCount
        For x = 0 To Array.LastIndexOf(drvRemovalPkgs, drvRemovalLastPkg)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            Dim driverRemovalPackage As String = drvRemovalPkgs(x)
            currentTask.Text = LocalizationService.ForSection("Progress.RemoveDrivers").Format("RemovingDriver.Item", x + 1, drvRemovalCount)
            DynaLog.LogMessage("Getting information about driver file " & Quote & Path.GetFileName(driverRemovalPackage) & Quote & "...")
            CurrentPB.Value = x + 1
            LogView.AppendText(CrLf &
                               ProgressLogText("Driver") & (x + 1) & ProgressLogText("Of.Word") & drvRemovalCount)
            ' Get driver information
            ShowDriverInformationForRemoval(driverRemovalPackage)
            DynaLog.LogMessage("Checking current operating mode...")
            If OnlineMgmt Then
                DynaLog.LogMessage("Online installation management mode detected. Using PNPUTIL to delete the driver...")
                DynaLog.LogMessage("Checking pnputil version...")
                Try
                    Dim pnputilVersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "pnputil.exe"))
                    DynaLog.LogMessage("PNPUTIL version info: " & pnputilVersionInfo.FileVersion)
                    If pnputilVersionInfo.FileMajorPart >= 10 Then
                        DynaLog.LogMessage("System PNPUTIL comes from Windows 10 or newer.")
                        RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "pnputil.exe"),
                                   "/delete-driver " & driverRemovalPackage & " /force", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32"), True)
                    Else
                        DynaLog.LogMessage("System PNPUTIL comes from Windows 8.")
                        RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "pnputil.exe"),
                                   "-f -d " & driverRemovalPackage, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32"), True)
                    End If
                Catch ex As Exception
                    DynaLog.LogMessage("An error occurred with this method. Error message: " & ex.Message & " (exit code " & Hex(ex.HResult) & "). Since it's our only way of removing drivers in this mode, signal an error message")
                    DismExitCode = ex.HResult
                End Try
            Else
                DynaLog.LogMessage("Online installation management mode not detected. Using DISM to delete the driver...")
                CommandArgs &= " /image=" & targetImage & " /remove-driver /driver=" & Quote & driverRemovalPackage & Quote
                RunProcess(DismProgram, CommandArgs)
            End If
            LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
            errCode = Hex(Decimal.ToInt32(DismExitCode))
            If DismExitCode = 0 Then
                drvSuccessfulRemovals += 1
            Else
                drvFailedRemovals += 1
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
            Else
                LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
            End If
            If PackageErrorCodes.Count <= 0 Then
                If errCode.Length >= 8 Then
                    PackageErrorCodes.Add("0x" & errCode)
                Else
                    PackageErrorCodes.Add(errCode)
                End If
            Else
                If errCode.Length >= 8 Then
                    PackageErrorCodes.Add("0x" & errCode)
                Else
                    PackageErrorCodes.Add(errCode)
                End If
            End If
        Next
        CurrentPB.Value = CurrentPB.Maximum
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level.For.Selected.Drivers") & CrLf)
        For x = 0 To PackageErrorCodes.Count - 1
            LogView.AppendText(CrLf & ProgressLogText("Driver.No") & (x + 1) & ": " & PackageErrorCodes(x))
        Next
        Thread.Sleep(2000)
        If drvSuccessfulRemovals > 0 Then
            GetErrorCode(True)
        ElseIf drvSuccessfulRemovals <= 0 Then
            GetErrorCode(False)
        End If
    End Sub

    Private Sub ExportDrivers(targetImage As String)
        DynaLog.LogMessage("Preparing to export image drivers...")
        DynaLog.LogMessage("Export target: " & Quote & drvExportTarget & Quote)
        DynaLog.LogMessage("Export all drivers? " & If(drvExportAllDrvs, "Yes", "No"))
        If Not drvExportAllDrvs Then DynaLog.LogMessage("Class name to use as filter for driver exports: " & Quote & drvExportSpecificClassName & Quote)
                allTasks.Text = LocalizationService.ForSection("Progress.ExportDrivers")("ExportingDrivers.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.ExportDrivers")("ExportThirdParty.Button")
        LogView.AppendText(CrLf & ProgressLogText("Exporting.Drivers.To.Specified.Folder") & CrLf &
                           ProgressLogText("Export.Target") & Quote & drvExportTarget & Quote & CrLf &
                           ProgressLogText("Export.All.Drivers.Or.Just.Those.With.Matching") & If(drvExportAllDrvs, ProgressLogText("All.Drivers"), ProgressLogText("Drivers.With.Matching.Class.Name")) & CrLf &
                           ProgressLogText("If.Not.All.Drivers.Are.Exported.Which.Class") & drvExportSpecificClassName & CrLf)
        If drvExportAllDrvs Then
            If drvExportWin7Mode Then
                Try
                    Dim ImageDrivers As New List(Of ImageDriver)

                    ' Run DISM and parse the output in one go.
                    Using DriverEnumerationProc As New Process() With {
                        .StartInfo = New ProcessStartInfo() With {
                            .FileName = DismProgram,
                            .Arguments = String.Format("/English /image={0} /get-drivers{1}", Quote & MountDir & Quote, If(AllDrivers, " /all", "")),
                            .CreateNoWindow = True,
                            .WindowStyle = ProcessWindowStyle.Hidden,
                            .UseShellExecute = False,
                            .RedirectStandardOutput = True
                        }
                    }
                        Dim output As String = ""
                        DriverEnumerationProc.Start()
                        output = DriverEnumerationProc.StandardOutput.ReadToEnd()
                        DriverEnumerationProc.WaitForExit()
                        If DriverEnumerationProc.ExitCode = 0 Then
                            ' Parse the output.
                            Dim outputLines As String() = output.Split({vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries).SkipWhile(Function(line) Not line.StartsWith("Published Name : ", StringComparison.InvariantCultureIgnoreCase)).ToArray()
                            Dim drvPublishedNameString As String = "",
                                drvOriginalFileNameString As String = "",
                                drvInboxString As String = "",
                                drvClassNameString As String = "",
                                drvProviderNameString As String = "",
                                drvDateString As String = "",
                                drvVersionString As String = ""
                            For Each outputLine In outputLines
                                If outputLine.StartsWith("Published Name : ") Then
                                    drvPublishedNameString = outputLine.Replace("Published Name : ", "")
                                ElseIf outputLine.StartsWith("Original File Name : ") Then
                                    drvOriginalFileNameString = outputLine.Replace("Original File Name : ", "")
                                ElseIf outputLine.StartsWith("Inbox : ") Then
                                    drvInboxString = outputLine.Replace("Inbox : ", "")
                                ElseIf outputLine.StartsWith("Class Name : ") Then
                                    drvClassNameString = outputLine.Replace("Class Name : ", "")
                                ElseIf outputLine.StartsWith("Provider Name : ") Then
                                    drvProviderNameString = outputLine.Replace("Provider Name : ", "")
                                ElseIf outputLine.StartsWith("Date : ") Then
                                    drvDateString = outputLine.Replace("Date : ", "")
                                ElseIf outputLine.StartsWith("Version : ") Then
                                    drvVersionString = outputLine.Replace("Version : ", "")
                                End If

                                ' If we've grabbed everything at this point, we add it to our list,
                                ' then clear everything and move on.
                                If drvPublishedNameString <> "" AndAlso
                                    drvOriginalFileNameString <> "" AndAlso
                                    drvInboxString <> "" AndAlso
                                    drvClassNameString <> "" AndAlso
                                    drvProviderNameString <> "" AndAlso
                                    drvDateString <> "" AndAlso
                                    drvVersionString <> "" Then
                                    ImageDrivers.Add(New ImageDriver(drvPublishedNameString,
                                                                     drvOriginalFileNameString,
                                                                     drvInboxString.Equals("Yes", StringComparison.InvariantCultureIgnoreCase),
                                                                     drvClassNameString,
                                                                     drvProviderNameString,
                                                                     drvDateString,
                                                                     New Version(drvVersionString)))
                                    drvPublishedNameString = ""
                                    drvOriginalFileNameString = ""
                                    drvInboxString = ""
                                    drvClassNameString = ""
                                    drvProviderNameString = ""
                                    drvDateString = ""
                                    drvVersionString = ""
                                End If
                            Next
                        Else
                            Throw New Exception(DISMProc.ExitCode)
                        End If
                    End Using

                    Dim driversToExport As IEnumerable(Of ImageDriver) = ImageDrivers
                    If driversToExport Is Nothing Then Exit Try

                    DynaLog.LogMessage("Amount of drivers to export: " & driversToExport.Count)
                    LogView.AppendText(CrLf & driversToExport.Count & ProgressLogText("Driver.S.Will.Be.Exported.To.The.Destination"))
                    For Each driverToExport In driversToExport
                        LogView.AppendText(CrLf & ProgressLogText("Exporting.Driver.File") & Path.GetFileName(driverToExport.DriverOriginalFileName) & "...")
                        Dim drvName As String = Path.GetFileName(driverToExport.DriverOriginalFileName)
                        Dim destinationDriverPath As String = Path.Combine(drvExportTarget, drvName)
                        CopyRecursive(Path.GetDirectoryName(driverToExport.DriverOriginalFileName), destinationDriverPath)
                    Next
                Catch ex As Exception
                    DynaLog.LogMessage("Could not export specific drivers. Error message: " & ex.Message)
                    DismExitCode = ex.HResult
                End Try
            Else
                ' Check the DISM version, as the Windows 7 version doesn't allow this action
                Select Case DismVersionChecker.ProductMajorPart
                    Case 6
                        Select Case DismVersionChecker.ProductMinorPart
                            Case 1
                                ' Not supported
                            Case Is >= 2
                                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /export-driver /destination=" & Quote & drvExportTarget & Quote
                        End Select
                    Case 10
                        CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /export-driver /destination=" & Quote & drvExportTarget & Quote
                End Select
                RunProcess(DismProgram, CommandArgs)
            End If
        Else
            ' Selective driver exports, based on class name, cannot be done with DISM as DISM will export all drivers no matter what.
            ' We have to get the drivers from the image, which will let us filter by class name, then we copy them manually to the destination.
            If drvExportWin7Mode Then
                Try
                    Dim ImageDrivers As New List(Of ImageDriver)

                    ' Run DISM and parse the output in one go.
                    Using DriverEnumerationProc As New Process() With {
                        .StartInfo = New ProcessStartInfo() With {
                            .FileName = DismProgram,
                            .Arguments = String.Format("/English /image={0} /get-drivers{1}", Quote & MountDir & Quote, If(AllDrivers, " /all", "")),
                            .CreateNoWindow = True,
                            .WindowStyle = ProcessWindowStyle.Hidden,
                            .UseShellExecute = False,
                            .RedirectStandardOutput = True
                        }
                    }
                        Dim output As String = ""
                        DriverEnumerationProc.Start()
                        output = DriverEnumerationProc.StandardOutput.ReadToEnd()
                        DriverEnumerationProc.WaitForExit()
                        If DriverEnumerationProc.ExitCode = 0 Then
                            ' Parse the output.
                            Dim outputLines As String() = output.Split({vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries).SkipWhile(Function(line) Not line.StartsWith("Published Name : ", StringComparison.InvariantCultureIgnoreCase)).ToArray()
                            Dim drvPublishedNameString As String = "",
                                drvOriginalFileNameString As String = "",
                                drvInboxString As String = "",
                                drvClassNameString As String = "",
                                drvProviderNameString As String = "",
                                drvDateString As String = "",
                                drvVersionString As String = ""
                            For Each outputLine In outputLines
                                If outputLine.StartsWith("Published Name : ") Then
                                    drvPublishedNameString = outputLine.Replace("Published Name : ", "")
                                ElseIf outputLine.StartsWith("Original File Name : ") Then
                                    drvOriginalFileNameString = outputLine.Replace("Original File Name : ", "")
                                ElseIf outputLine.StartsWith("Inbox : ") Then
                                    drvInboxString = outputLine.Replace("Inbox : ", "")
                                ElseIf outputLine.StartsWith("Class Name : ") Then
                                    drvClassNameString = outputLine.Replace("Class Name : ", "")
                                ElseIf outputLine.StartsWith("Provider Name : ") Then
                                    drvProviderNameString = outputLine.Replace("Provider Name : ", "")
                                ElseIf outputLine.StartsWith("Date : ") Then
                                    drvDateString = outputLine.Replace("Date : ", "")
                                ElseIf outputLine.StartsWith("Version : ") Then
                                    drvVersionString = outputLine.Replace("Version : ", "")
                                End If

                                ' If we've grabbed everything at this point, we add it to our list,
                                ' then clear everything and move on.
                                If drvPublishedNameString <> "" AndAlso
                                    drvOriginalFileNameString <> "" AndAlso
                                    drvInboxString <> "" AndAlso
                                    drvClassNameString <> "" AndAlso
                                    drvProviderNameString <> "" AndAlso
                                    drvDateString <> "" AndAlso
                                    drvVersionString <> "" Then
                                    ImageDrivers.Add(New ImageDriver(drvPublishedNameString,
                                                                     drvOriginalFileNameString,
                                                                     drvInboxString.Equals("Yes", StringComparison.InvariantCultureIgnoreCase),
                                                                     drvClassNameString,
                                                                     drvProviderNameString,
                                                                     drvDateString,
                                                                     New Version(drvVersionString)))
                                    drvPublishedNameString = ""
                                    drvOriginalFileNameString = ""
                                    drvInboxString = ""
                                    drvClassNameString = ""
                                    drvProviderNameString = ""
                                    drvDateString = ""
                                    drvVersionString = ""
                                End If
                            Next
                        Else
                            Throw New Exception(DISMProc.ExitCode)
                        End If
                    End Using

                    DynaLog.LogMessage("Filtering driver collection based on class name...")
                    Dim driversToExport As IEnumerable(Of ImageDriver) = ImageDrivers.Where(Function(driver) driver.DriverClassName.Equals(drvExportSpecificClassName, StringComparison.OrdinalIgnoreCase))
                    If driversToExport Is Nothing Then Exit Try

                    DynaLog.LogMessage("Amount of drivers to export: " & driversToExport.Count)
                    LogView.AppendText(CrLf & driversToExport.Count & ProgressLogText("Driver.S.Will.Be.Exported.To.The.Destination"))
                    For Each driverToExport In driversToExport
                        LogView.AppendText(CrLf & ProgressLogText("Exporting.Driver.File") & Path.GetFileName(driverToExport.DriverOriginalFileName) & "...")
                        Dim drvName As String = Path.GetFileName(driverToExport.DriverOriginalFileName)
                        Dim destinationDriverPath As String = Path.Combine(drvExportTarget, drvName)
                        CopyRecursive(Path.GetDirectoryName(driverToExport.DriverOriginalFileName), destinationDriverPath)
                    Next
                Catch ex As Exception
                    DynaLog.LogMessage("Could not export specific drivers. Error message: " & ex.Message)
                    DismExitCode = ex.HResult
                End Try
            Else
                Try
                    LogView.AppendText(CrLf & ProgressLogText("Getting.Image.Drivers"))
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    Using session As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MountDir))
                        DynaLog.LogMessage("Getting drivers with DISMAPI...")
                        Dim driverPackages As DismDriverPackageCollection = DismApi.GetDrivers(session, False)
                        If driverPackages Is Nothing Then Exit Try
                        DynaLog.LogMessage("Filtering driver collection based on class name...")
                        Dim driversToExport As IEnumerable(Of DismDriverPackage) = driverPackages.Where(Function(driver) driver.ClassName.Equals(drvExportSpecificClassName, StringComparison.OrdinalIgnoreCase))
                        If driversToExport Is Nothing Then Exit Try

                        DynaLog.LogMessage("Amount of drivers to export: " & driversToExport.Count)
                        LogView.AppendText(CrLf & driversToExport.Count & ProgressLogText("Driver.S.Will.Be.Exported.To.The.Destination"))
                        For Each driverToExport In driversToExport
                            LogView.AppendText(CrLf & ProgressLogText("Exporting.Driver.File") & Path.GetFileName(driverToExport.OriginalFileName) & "...")
                            Dim drvName As String = Path.GetFileName(driverToExport.OriginalFileName)
                            Dim destinationDriverPath As String = Path.Combine(drvExportTarget, drvName)
                            CopyRecursive(Path.GetDirectoryName(driverToExport.OriginalFileName), destinationDriverPath)
                        Next
                    End Using
                    DismExitCode = 0
                Catch ex As Exception
                    DynaLog.LogMessage("Could not export specific drivers. Error message: " & ex.Message)
                    DismExitCode = ex.HResult
                Finally
                    Try
                        DismApi.Shutdown()
                    Catch ex As Exception

                    End Try
                End Try
            End If
        End If
        LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
        Else
            LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
        End If
        GetErrorCode(False)
    End Sub

    ''' <summary>
    ''' Copies the contents of a directory, and any subdirectories within the directory,
    ''' to a given destination.
    ''' </summary>
    ''' <param name="SourceDirectory">The directory to copy</param>
    ''' <param name="DestinationDirectory">The destination of the copied files</param>
    ''' <returns>Whether the copy succeeded</returns>
    Private Function CopyRecursive(SourceDirectory As String, DestinationDirectory As String) As Boolean
        ' We make sure the directory exists, if it doesn't exist, we stop.
        If Not Directory.Exists(SourceDirectory) Then Return False

        ' If the destination folder does not exist, then we try creating it. If we couldn't,
        ' we simply give up.
        If Not Directory.Exists(DestinationDirectory) Then
            Try
                Directory.CreateDirectory(DestinationDirectory)
            Catch ex As Exception
                Return False
            End Try
        End If

        Try
            ' Now, we create all the directories of the source folder to the destination
            Dim dirsInSource As String() = Directory.GetDirectories(SourceDirectory, "*", SearchOption.AllDirectories)
            For Each dirInSource In dirsInSource
                Dim sourcePath As String = dirInSource.Substring(SourceDirectory.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                Dim destinationPath As String = Path.Combine(DestinationDirectory, sourcePath)

                If Not Directory.Exists(destinationPath) Then
                    Directory.CreateDirectory(destinationPath)
                End If
            Next

            ' Next, we copy all the files in the source directory to the destination
            For Each FileToCopy In Directory.GetFiles(SourceDirectory, "*", SearchOption.AllDirectories)
                Dim sourcePath As String = FileToCopy.Substring(SourceDirectory.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                Dim destinationPath As String = Path.Combine(DestinationDirectory, sourcePath)

                File.Copy(FileToCopy, destinationPath, True)
            Next
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    Private Sub ImportDrivers(targetImage As String)
        DynaLog.LogMessage("Preparing to import image drivers...")
        DynaLog.LogMessage("Source type: " & ImportSourceInt)
                allTasks.Text = LocalizationService.ForSection("Progress.ImportDrivers")("ImportingDrivers.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.ImportDrivers")("PrepareImport.Button")
        LogView.AppendText(CrLf & ProgressLogText("Importing.Third.Party.Drivers") & CrLf)
        Select Case ImportSourceInt
            Case 0
                LogView.AppendText(ProgressLogText("Driver.Import.Source.Windows.Image") & Quote & DrvImport_SourceImage & Quote & ")" & CrLf)
            Case 1
                LogView.AppendText(ProgressLogText("Driver.Import.Source.Active.Installation") & CrLf)
            Case 2
                LogView.AppendText(ProgressLogText("Driver.Import.Source.Offline.Installation") & Quote & DrvImport_SourceDisk & Quote & ")" & CrLf)
        End Select
        Thread.Sleep(500)
        LogView.AppendText(CrLf & ProgressLogText("Creating.Temporary.Folder.For.Driver.Exports") & CrLf)
                currentTask.Text = LocalizationService.ForSection("Progress.ImportDrivers")("ExportThirdParty.Item")
        Try
            DynaLog.LogMessage("Creating directory where drivers will be exported to...")
            Directory.CreateDirectory(Application.StartupPath & "\export_temp")
        Catch ex As Exception
            DynaLog.LogMessage("Could not create the driver export directory. Error message: " & ex.Message)
            LogView.AppendText(CrLf & ProgressLogText("The.Temporary.Folder.Could.Not.Be.Created.See") & CrLf & CrLf & ex.ToString() & "-" & ex.Message)
        End Try
        If Directory.Exists(Application.StartupPath & "\export_temp") Then
            DynaLog.LogMessage("Exporting drivers...")
            LogView.AppendText(CrLf & ProgressLogText("Exporting.Third.Party.Drivers.From.Import.Source") & CrLf)
            Dim importSource As String = ""
            Select Case ImportSourceInt
                Case 0
                    importSource = If(Path.GetPathRoot(DrvImport_SourceImage) = DrvImport_SourceImage, DrvImport_SourceImage, Quote & DrvImport_SourceImage & Quote)
                Case 2
                    importSource = If(Path.GetPathRoot(DrvImport_SourceDisk) = DrvImport_SourceDisk, DrvImport_SourceDisk, Quote & DrvImport_SourceDisk & Quote)
            End Select
            CommandArgs &= If(ImportSourceInt = 1, " /online", " /image=" & importSource) & " /export-driver /destination=" & Quote & Application.StartupPath & "\export_temp" & Quote
            RunProcess(DismProgram, CommandArgs)
            LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
            If Hex(DismExitCode).Length < 8 Then
                errCode = DismExitCode
            Else
                errCode = Hex(DismExitCode)
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
            Else
                LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
            End If
            If DismExitCode = 0 Then
                DynaLog.LogMessage("The previous operation succeeded. Adding the drivers...")
                CurrentPB.Value = CurrentPB.Maximum / 2
                AllPB.Value = AllPB.Maximum / 2
                        currentTask.Text = LocalizationService.ForSection("Progress.ImportDrivers")("ImportThirdParty.Item")
                LogView.AppendText(CrLf & ProgressLogText("Importing.Third.Party.Drivers.From.The.Temporary.Export"))
                CommandArgs = BckArgs
                If OnlineMgmt Then
                    DynaLog.LogMessage("Online installation management mode detected. Using PNPUTIL to add the driver...")
                    DynaLog.LogMessage("Checking pnputil version...")
                    Dim pnpUtilArgs As String = ""
                    Dim pnputilVersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "pnputil.exe"))
                    DynaLog.LogMessage("PNPUTIL version info: " & pnputilVersionInfo.FileVersion)
                    If pnputilVersionInfo.FileMajorPart >= 10 Then
                        DynaLog.LogMessage("System PNPUTIL comes from Windows 10 or newer.")
                        pnpUtilArgs = String.Format("/add-driver {0} /install", Quote & Application.StartupPath & "\export_temp" & "\*.inf" & Quote & " /subdirs")
                        RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "pnputil.exe"),
                                   pnpUtilArgs, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32"), True)
                    Else
                        DynaLog.LogMessage("System PNPUTIL comes from Windows 8.")
                        For Each InfFile In Directory.EnumerateFiles(Path.Combine(Application.StartupPath, "export_temp"), "*.inf", SearchOption.AllDirectories)
                            pnpUtilArgs = String.Format("-i -a {0}", Quote & InfFile & Quote)
                            RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32", "pnputil.exe"),
                                       pnpUtilArgs, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32"), True)
                        Next
                    End If
                Else
                    DynaLog.LogMessage("Online installation management mode not detected. Using DISM to add the driver...")
                    CommandArgs &= " /image=" & targetImage & " /add-driver /driver=" & Quote & Application.StartupPath & "\export_temp" & Quote & " /recurse"
                    RunProcess(DismProgram, CommandArgs)
                End If
                If Hex(DismExitCode).Length < 8 Then
                    errCode = DismExitCode
                Else
                    errCode = Hex(DismExitCode)
                End If
                If errCode.Length >= 8 Then
                    LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
                Else
                    LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
                End If
                GetErrorCode(False)
            End If
            LogView.AppendText(CrLf & ProgressLogText("Deleting.Temporary.Export.Directory"))
            Try
                DynaLog.LogMessage("Attempting to delete the driver export directory...")
                Directory.Delete(Application.StartupPath & "\export_temp", True)
            Catch ex As Exception
                DynaLog.LogMessage("Could not delete driver export directory. Error message: " & ex.Message)
                LogView.AppendText(CrLf & ProgressLogText("We.Couldn.T.Delete.The.Temporary.Export.Directory") & Quote & ProgressLogText("Export.Temp") & Quote & ProgressLogText("Directory.Manually"))
            End Try
        End If
    End Sub

    Private Sub ShowDriverInformationForRemoval(driverRemovalPackage As String)
        Try
            DismApi.Initialize(DismLogLevel.LogErrors)
            Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                For Each drv As DismDriverPackage In drvCollection
                    If drv.PublishedName = driverRemovalPackage Then
                        LogView.AppendText(CrLf & CrLf &
                                           ProgressLogText("Published.Name") & drv.PublishedName & CrLf &
                                           ProgressLogText("Provider.Name") & drv.ProviderName & CrLf &
                                           ProgressLogText("Class.Name") & drv.ClassName & CrLf &
                                           ProgressLogText("Class.Description") & drv.ClassDescription & CrLf &
                                           ProgressLogText("Class.GUID") & drv.ClassGuid & CrLf &
                                           ProgressLogText("Version.And.Date") & drv.Version.ToString() & " / " & drv.Date.ToString() & CrLf &
                                           ProgressLogText("Is.Part.Of.The.Windows.Distribution") & If(drv.InBox, ProgressLogText("Yes"), ProgressLogText("No")) & CrLf &
                                           ProgressLogText("Is.Critical.To.The.Boot.Process") & If(drv.BootCritical, ProgressLogText("Yes"), ProgressLogText("No")))
                        If drv.InBox Then
                            DynaLog.LogMessage("This driver is part of the Windows distribution.")
                            LogView.AppendText(CrLf & CrLf &
                                               ProgressLogText("Warning.This.Driver.Package.Is.Part.Of.The"))
                        End If
                        If drv.BootCritical Then
                            DynaLog.LogMessage("This driver is critical to the boot process of the Windows image.")
                            LogView.AppendText(CrLf & CrLf &
                                               ProgressLogText("Warning.This.Driver.Package.Is.Critical.To.The"))
                        End If
                        Exit For
                    End If
                Next
            End Using
        Finally
            Try
                DynaLog.LogMessage("Shutting down API...")
                DismApi.Shutdown()
            Catch ex As Exception

            End Try
        End Try
    End Sub

#End Region

#Region "Unattended Answer File Management Tasks"

    Private Sub ApplyUnattendedFile(targetImage As String)
        DynaLog.LogMessage("Preparing to apply unattended answer file...")
        DynaLog.LogMessage("Answer file: " & Quote & Path.GetFileName(UnattendedFile) & Quote)
                allTasks.Text = LocalizationService.ForSection("Progress.ApplyUnattend")("ApplyAnswerFile.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.ApplyUnattend")("Applying.Answer.Button")
        LogView.AppendText(CrLf & ProgressLogText("Applying.Unattended.Answer.File.Options") & CrLf &
                           ProgressLogText("Unattended.Answer.File") & UnattendedFile)
        Try
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Creating.Directories.And.Copying.Files"))
            DynaLog.LogMessage("Copying unattended answer file to the Panther directory of the Windows image...")
            If Not Directory.Exists(Path.Combine(MountDir, "Windows", "Panther")) Then
                Directory.CreateDirectory(Path.Combine(MountDir, "Windows", "Panther"))
            End If
            File.Copy(UnattendedFile, Path.Combine(MountDir, "Windows", "Panther", "unattend.xml"), True)
            If UnattendedCopyToSysprep Then
                DynaLog.LogMessage("Copying unattended answer file to the Sysprep directory of the Windows image...")
                If Not Directory.Exists(Path.Combine(MountDir, "Windows", "system32", "Sysprep")) Then
                    Directory.CreateDirectory(Path.Combine(MountDir, "Windows", "system32", "Sysprep"))
                End If
                File.Copy(UnattendedFile, Path.Combine(MountDir, "Windows", "system32", "sysprep", "unattend.xml"), True)
            End If
            LogView.AppendText(CrLf & ProgressLogText("The.Unattended.Answer.File.Has.Been.Successfully.Copied"))
            GetErrorCode(True)
        Catch ex As Exception
            DynaLog.LogMessage("Could not copy unattended answer file to targets. Error message: " & ex.Message)
            CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /apply-unattend=" & Quote & UnattendedFile & Quote
            RunProcess(DismProgram, CommandArgs)
                    currentTask.Text = LocalizationService.ForSection("Progress.ApplyUnattend")("Gathering.Error.Level.Item")
            LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
            GetErrorCode(False)
            If errCode.Length >= 8 Then
                LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
            Else
                LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
            End If
        End Try
    End Sub

#End Region

#Region "Windows PE Management Tasks"

    Private Sub SetTargetPath(targetImage As String)
        DynaLog.LogMessage("Preparing to set the target path of the Windows PE image...")
        DynaLog.LogMessage("Target path to set: " & Quote & peNewTargetPath & Quote)
                allTasks.Text = LocalizationService.ForSection("Progress.SetTargetPath")("Setting.Target.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.SetTargetPath")("Setting.Windows.Button")
        LogView.AppendText(CrLf & ProgressLogText("Setting.The.Windows.PE.Target.Path") & CrLf &
                           ProgressLogText("New.Target.Path") & Quote & peNewTargetPath & Quote)
        CommandArgs &= " /image=" & targetImage & " /set-targetpath=" & peNewTargetPath
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
        Else
            LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
        End If
        GetErrorCode(False)
    End Sub

    Private Sub SetScratchSpace(targetImage As String)
        DynaLog.LogMessage("Preparing to set the scratch space of the Windows PE image...")
        DynaLog.LogMessage("Scratch space to set: " & peNewScratchSpace & " MB")
                allTasks.Text = LocalizationService.ForSection("Progress.ScratchSpace")("Setting.ScratchSpace.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.ScratchSpace")("SetScratchSpace.Button")
        LogView.AppendText(CrLf & ProgressLogText("Setting.The.Windows.PE.Scratch.Space") & CrLf &
                           ProgressLogText("New.Scratch.Space.Amount") & peNewScratchSpace & " MB")
        CommandArgs &= " /image=" & targetImage & " /set-scratchspace=" & peNewScratchSpace
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & ProgressLogText("Getting.Error.Level"))
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(ProgressLogText("Error.Level.0x.2") & errCode)
        Else
            LogView.AppendText(ProgressLogText("Error.Level.2") & errCode)
        End If
        GetErrorCode(False)
    End Sub

#End Region

#Region "OS Uninstall Management Tasks"

    Private Sub SetOSUnistallWindow()
        DynaLog.LogMessage("Preparing to set the OS rollback window...")
        DynaLog.LogMessage("New window: " & osUninstDayCount & " day(s)")
                allTasks.Text = LocalizationService.ForSection("Progress.RollbackWindow")("SetWindow.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.RollbackWindow")("SetDays.Button")
        LogView.AppendText(CrLf & ProgressLogText("Setting.The.Amount.Of.Days.An.Uninstall.Can") & CrLf &
                           ProgressLogText("Number.Of.Days") & osUninstDayCount)
        CommandArgs &= " /online /set-osuninstallwindow /value:" & osUninstDayCount
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

    Private Sub RemoveOSUnistall()
        DynaLog.LogMessage("Preparing to remove the OS rollback...")
                allTasks.Text = LocalizationService.ForSection("Progress.RemoveRollback")("RemoveRollback.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.RemoveRollback")("RemoveRevert.Button")
        LogView.AppendText(CrLf & ProgressLogText("Removing.The.Ability.To.Revert.To.An.Old"))
        CommandArgs &= " /online /remove-osuninstall"
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

    Private Sub InitiateOSUnistall()
        DynaLog.LogMessage("Preparing to initiate the OS rollback...")
                allTasks.Text = LocalizationService.ForSection("Progress.OSUninstall")("Uninstalling.Version.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.StartRollback")("Preparing.OSRollback.Button")
        LogView.AppendText(CrLf & ProgressLogText("Preparing.Operating.System.Rollback"))
        CommandArgs = " /online /norestart /initiate-osuninstall"
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

#End Region

#Region "Miscellaneous DISMTools Tasks"

    Private Sub ConvertImage()
        DynaLog.LogMessage("Preparing to convert the Windows image...")
        DynaLog.LogMessage("- Source image file: " & Quote & imgSrcFile & Quote)
        DynaLog.LogMessage("- Source image index: " & imgConversionIndex)
        DynaLog.LogMessage("- Destination image file: " & Quote & imgDestFile & Quote)
        DynaLog.LogMessage("- Conversion mode: " & imgConversionMode)
                allTasks.Text = LocalizationService.ForSection("Progress.ConvertImage")("ConvertingImage.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.ConvertImage")("Converting.Image.Button")
        LogView.AppendText(CrLf & ProgressLogText("Converting.Image") & CrLf &
                           ProgressLogText("Options") & CrLf)

        ' Gather options
        LogView.AppendText(ProgressLogText("Source.Image.File") & imgSrcFile & CrLf &
                           ProgressLogText("Index.To.Convert") & imgConversionIndex & CrLf &
                           ProgressLogText("Destination.Image.File") & imgDestFile & CrLf)
        If imgConversionMode = 0 Then
            LogView.AppendText(ProgressLogText("Image.Conversion.Mode.Windows.Imaging.WIM.Electronic.Software"))
        ElseIf imgConversionMode = 1 Then
            LogView.AppendText(ProgressLogText("Image.Conversion.Mode.Electronic.Software.Distribution.ESD.Windows"))
        End If

        ' Run commands
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        ' Not available
                    Case Is >= 2
                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /export-image /sourceimagefile=" & Quote & imgSrcFile & Quote & " /sourceindex=" & imgConversionIndex & " /destinationimagefile=" & Quote & imgDestFile & Quote
                End Select
            Case 10
                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /export-image /sourceimagefile=" & Quote & imgSrcFile & Quote & " /sourceindex=" & imgConversionIndex & " /destinationimagefile=" & Quote & imgDestFile & Quote
        End Select
        If imgConversionMode = 0 Then
            CommandArgs &= " /compress:recovery"
        ElseIf imgConversionMode = 1 Then
            CommandArgs &= " /compress:max"
        End If
        RunProcess(DismProgram, CommandArgs)
                currentTask.Text = LocalizationService.ForSection("Progress.ConvertImage")("Gathering.Error.Level.Item")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

    Private Sub MergeSWM()
        DynaLog.LogMessage("Preparing to merge SWM files...")
        DynaLog.LogMessage("- Source image file: " & Quote & imgSwmSource & Quote)
        DynaLog.LogMessage("- Source image index: " & imgMergerIndex)
        DynaLog.LogMessage("- Destination image file: " & Quote & imgWimDestination & Quote)
                allTasks.Text = LocalizationService.ForSection("Progress.MergeSWM")("MergingSwmfiles.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.MergeSWM")("Merging.Swmfiles.WIM.Button")
        LogView.AppendText(CrLf & ProgressLogText("Merging.SWM.Files.Into.A.WIM.File") & CrLf &
                           ProgressLogText("Options") & CrLf)
        ' Gather options
        LogView.AppendText(ProgressLogText("Source.Image.File") & imgSwmSource & CrLf &
                           ProgressLogText("Target.Index") & imgMergerIndex & CrLf &
                           ProgressLogText("Destination.Image.File") & imgWimDestination & CrLf)

        ' Run commands
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        ' Not available
                    Case Is >= 2
                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /export-image /sourceimagefile=" & Quote & imgSwmSource & Quote & " /swmfile=" & Quote & Path.GetDirectoryName(imgSwmSource) & "\" & Path.GetFileNameWithoutExtension(imgSwmSource) & "*.swm" & Quote & " /sourceindex=" & imgMergerIndex & " /destinationimagefile=" & Quote & imgWimDestination & Quote & " /compress=max /checkintegrity"
                End Select
            Case 10
                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /export-image /sourceimagefile=" & Quote & imgSwmSource & Quote & " /swmfile=" & Quote & Path.GetDirectoryName(imgSwmSource) & "\" & Path.GetFileNameWithoutExtension(imgSwmSource) & "*.swm" & Quote & " /sourceindex=" & imgMergerIndex & " /destinationimagefile=" & Quote & imgWimDestination & Quote & " /compress=max /checkintegrity"
        End Select
        RunProcess(DismProgram, CommandArgs)
                currentTask.Text = LocalizationService.ForSection("Progress.MergeSWM")("Gathering.Error.Level.Item")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

    Private Sub SwitchIndexes()
        DynaLog.LogMessage("Preparing to switch image indexes...")
        DynaLog.LogMessage("- Source image file: " & Quote & SwitchSourceImg & Quote)
        DynaLog.LogMessage("- Source image index: " & SwitchSourceIndex)
        DynaLog.LogMessage("- Target image index: " & SwitchTargetIndex)
                allTasks.Text = LocalizationService.ForSection("Progress.SwitchIndexes")("Switching.Image.Button")
                currentTask.Text = LocalizationService.ForSection("Progress.SwitchIndexes")("Unmounting.Source.Button")
        LogView.AppendText(CrLf & ProgressLogText("Switching.Image.Indexes") & CrLf &
                           ProgressLogText("Options") & CrLf)
        ' Gather options
        LogView.AppendText(ProgressLogText("Target.Mount.Directory") & SwitchTarget & CrLf &
                           ProgressLogText("Source.Image.Index") & SwitchSourceIndex & CrLf &
                           ProgressLogText("Target.Image.Index") & SwitchTargetIndex & " (" & SwitchTargetIndexName & ")")
        If SwitchCommitSourceIndex Then
            LogView.AppendText(CrLf & ProgressLogText("Commit.Source.Index.Yes"))
        Else
            LogView.AppendText(CrLf & ProgressLogText("Commit.Source.Index.No"))
        End If
        DynaLog.LogMessage("Unmounting source image whilst saving changes...")
        ' Run commands
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-wim /mountdir=" & Quote & SwitchTarget & Quote
                    Case Is >= 2
                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-image /mountdir=" & Quote & SwitchTarget & Quote
                End Select
            Case 10
                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-image /mountdir=" & Quote & SwitchTarget & Quote
        End Select
        If SwitchCommitSourceIndex Then
            CommandArgs &= " /commit"
        Else
            CommandArgs &= " /discard"
        End If
        RunProcess(DismProgram, CommandArgs)
                currentTask.Text = LocalizationService.ForSection("Progress.SwitchIndexes")("Gathering.Error.Level.Item")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
        If Decimal.ToInt32(DismExitCode) <> 0 Then
            DynaLog.LogMessage("Could not save changes to the image. Unmounting image whilst discarding changes...")
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Could.Not.Commit.Changes.To.The.Image.Discarding"))
                    currentTask.Text = LocalizationService.ForSection("Progress.SwitchIndexes")("Unmounting.Source.Index.Item")
            Select Case DismVersionChecker.ProductMajorPart
                Case 6
                    Select Case DismVersionChecker.ProductMinorPart
                        Case 1
                            CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-wim /mountdir=" & Quote & SwitchTarget & Quote & " /discard"
                        Case Is >= 2
                            CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-image /mountdir=" & Quote & SwitchTarget & Quote & " /discard"
                    End Select
                Case 10
                    CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-image /mountdir=" & Quote & SwitchTarget & Quote & " /discard"
            End Select
            RunProcess(DismProgram, CommandArgs)
                    currentTask.Text = LocalizationService.ForSection("Progress.SwitchIndexes")("CurrentTask.Item")
            LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
            GetErrorCode(False)
            If errCode.Length >= 8 Then
                LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
            Else
                LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
            End If
            If Decimal.ToInt32(DismExitCode) <> 0 Then
                DynaLog.LogMessage("Could not unmount the image.")
                Return
            End If
        End If
        AllPB.Value = AllPB.Maximum / taskCount
        currentTCont += 1
        DynaLog.LogMessage("Mounting Windows image...")
        taskCountLbl.Text = LocalizationService.ForSection("Progress").Format("Tasks.Label", currentTCont, taskCount)
        currentTask.Text = LocalizationService.ForSection("Progress.SwitchIndexes")("Mounting.Target.Index.Item")
        LogView.AppendText(CrLf & ProgressLogText("Mounting.Image.Index") & SwitchTargetIndex & ")...")
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /mount-wim /wimfile=" & Quote & SwitchSourceImg & Quote & " /index=" & SwitchTargetIndex & " /mountdir=" & Quote & SwitchTarget & Quote
                    Case Is >= 2
                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /mount-image /imagefile=" & Quote & SwitchSourceImg & Quote & " /index=" & SwitchTargetIndex & " /mountdir=" & Quote & SwitchTarget & Quote
                End Select
            Case 10
                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /mount-image /imagefile=" & Quote & SwitchSourceImg & Quote & " /index=" & SwitchTargetIndex & " /mountdir=" & Quote & SwitchTarget & Quote
        End Select
        If SwitchMountAsReadOnly Then
            CommandArgs &= " /readonly"
        End If
        RunProcess(DismProgram, CommandArgs)
                currentTask.Text = LocalizationService.ForSection("Progress.SwitchIndexes")("Gathering.Error.Level.Item")
        LogView.AppendText(CrLf & ProgressLogText("Gathering.Error.Level"))
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level.0x") & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & ProgressLogText("Error.Level") & errCode)
        End If
    End Sub

    Private Sub ReplaceFfuFile()
        DynaLog.LogMessage("Preparing to replace FFU files...")
        DynaLog.LogMessage("- Source file: " & Quote & FFUReplaceSourceFFU & Quote)
        DynaLog.LogMessage("- Destination file: " & Quote & FFUReplaceDestinationFFU & Quote)
        allTasks.Text = LocalizationService.ForSection("Progress.Operation")("Replacing.FFU.Files.Label")
        currentTask.Text = LocalizationService.ForSection("Progress.Operation")("Replacing.Original.FFU.Label")
        LogView.AppendText(CrLf & ProgressLogText("Replacing.FFU.File") & Quote & FFUReplaceDestinationFFU & Quote & ProgressLogText("With") & Quote & FFUReplaceSourceFFU & Quote & "...")
        Try
            If Not File.Exists(FFUReplaceSourceFFU) Or Not File.Exists(FFUReplaceDestinationFFU) Then Throw New Exception("One or both FFU files do not exist.")
            File.Delete(FFUReplaceDestinationFFU)
            File.Move(FFUReplaceSourceFFU, FFUReplaceDestinationFFU)
            IsSuccessful = True
            LogView.AppendText(CrLf & ProgressLogText("The.FFU.File.Has.Been.Successfully.Replaced"))
        Catch ex As Exception
            DynaLog.LogMessage("FFU files could not be replaced. Error message: " & ex.Message)
            IsSuccessful = False
            LogView.AppendText(CrLf & ProgressLogText("The.FFU.File.Could.Not.Be.Replaced") & ex.Message)
        End Try
    End Sub

#End Region

    Sub GetPkgErrorLevel()
        errCode = Hex(Decimal.ToInt32(DismExitCode))
        Select Case errCode
            Case 0
                DynaLog.LogMessage("Package addition succeeded.")
                pkgSuccessfulAdditions += 1
            Case Else
                DynaLog.LogMessage("Package addition failed.")
                pkgFailedAdditions += 1
        End Select
    End Sub

    Sub GetFeatErrorLevel()
        errCode = Hex(Decimal.ToInt32(DismExitCode))
        Select Case errCode
            Case 0
                DynaLog.LogMessage("Feature enablement succeeded.")
                featSuccessfulEnablements += 1
            Case Else
                DynaLog.LogMessage("Feature enablement failed.")
                featFailedEnablements += 1
        End Select
    End Sub

    Private Sub ProgressBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ProgressBW.DoWork
        DynaLog.LogMessage("Detecting items in task list...")
        DynaLog.LogMessage("Task list items: " & TaskList.Count)
        If TaskList.Count >= 2 Then
            DynaLog.LogMessage("Running tasks in task list...")
            RunTaskList(TaskList)
        Else
            DynaLog.LogMessage("Running task...")
            RunOps(OperationNum)
        End If
    End Sub

    Sub SaveLog(LogFile As String)
        DynaLog.LogMessage("Saving contents of log to a file...")
        DynaLog.LogMessage("- Log destination: " & Quote & LogFile & Quote)
        DynaLog.LogMessage("Determining if log file exists...")
        If Not File.Exists(LogFile) Then
            DynaLog.LogMessage("Log file does not exist. Attempting to create it...")
            ' Create file
            Try
                File.WriteAllText(LogFile, String.Empty)
            Catch ex As Exception
                DynaLog.LogMessage("Could not create log file. Error message: " & ex.Message)
                LogView.AppendText(CrLf &
                                   ProgressLogText("Warning.The.Contents.Of.The.Log.Window.Could") & ex.Message)
                Exit Sub
            End Try
        End If
        Dim FileLength As Integer = 0
        FileLength = New FileInfo(LogFile).Length
        DynaLog.LogMessage("Size of log file in bytes: " & FileLength)
        Try
            If FileLength <> 0 Then
                File.AppendAllText(LogFile, CrLf & "==================== DISMTools Log Window Contents (" & DateTime.Now.ToString() & ") ====================", ASCII)
            Else
                File.AppendAllText(LogFile, "======================== DISMTools Log File ========================" & CrLf &
                                            "This is an automatically generated log file created by DISMTools." & CrLf &
                                            "This file can be viewed at any time to view successful and/or" & CrLf &
                                            "failed tasks." & CrLf & CrLf &
                                            "This log file is updated every time an operation is performed." & CrLf &
                                            "However, it does not contain the actual DISM log file, which is" & CrLf &
                                            "also automatically generated each time DISM is run from this" & CrLf &
                                            "program. These log files are named: " & CrLf &
                                            "                    " & Quote & "DISMTools-<date/time>.log" & Quote & "                    " & CrLf &
                                            "====================================================================", ASCII)
            End If
            File.AppendAllText(LogFile, CrLf & LogView.Text, ASCII)
        Catch ex As Exception
            DynaLog.LogMessage("Could not log this operation. Error message: " & ex.Message)
        End Try
    End Sub

    Sub SaveDismOutput(OutputFile As String)
        DynaLog.LogMessage("Saving DISM output to a file...")
        DynaLog.LogMessage("- Log destination: " & Quote & OutputFile & Quote)
        If String.IsNullOrEmpty(DISM_LogView.RichTextBox1.Text) Then
            DynaLog.LogMessage("There is no content to save.")
            Exit Sub
        End If
        Try
            If Not File.Exists(OutputFile) Then
                DynaLog.LogMessage("Attempting to create output file...")
                ' Create file
                Try
                    File.WriteAllText(OutputFile, String.Empty)
                Catch ex As Exception
                    DynaLog.LogMessage("Could not create log file. Error message: " & ex.Message)
                    LogView.AppendText(CrLf &
                                       ProgressLogText("Warning.The.Contents.Of.The.Log.Window.Could") & ex.Message)
                    Exit Sub
                End Try
            End If
            File.AppendAllText(OutputFile, DISM_LogView.RichTextBox1.Text, ASCII)
        Catch ex As Exception
            DynaLog.LogMessage("Could not log this operation. Error message: " & ex.Message)
        End Try
    End Sub

    Private Sub ProgressBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ProgressBW.RunWorkerCompleted
        TaskList.Clear()
        If PreventSystemFromSleeping Then
            ' Restore sleep mode
            DynaLog.LogMessage("Restoring system sleep mode...")
            PowerManagementHelper.EnableSystemSleepMode()
        End If
        If IsSuccessful Then
            DynaLog.LogMessage("Tasks have been successful.")
            If OperationNum = 9 Then LogView.AppendText(CrLf &
                               ProgressLogText("The.Volume.Images.Have.Been.Deleted.If.You") & Quote & ProgressLogText("Mount.Image") & Quote & ProgressLogText("Option.Or.Use.This.Command.If.You.Want") & CrLf &
                               ProgressLogText("DISM.Mount.Image.Imagefile") & Quote & imgIndexDeletionSourceImg & Quote & ProgressLogText("Index.Preferred.Index.Mountdir.Preferred.Mountpoint"))
            DynaLog.LogMessage("Saving operation logs...")
            SaveLog(Application.StartupPath & "\logs\DISMTools.log")
            SaveDismOutput(Application.StartupPath & "\logs\DISM_Output_" & Date.Now.ToString("yy-MM-dd-HH-mm-ss") & ".log")
            Try
                CurrentPB.Value = 100
            Catch ex As Exception
                ' Continue
            End Try
            AllPB.Value = AllPB.Maximum
            Refresh()
            MainForm.isModified = True
            If OperationNum < 993 And Not OperationNum = 0 Then
                Thread.Sleep(2000)
            End If
            If OperationNum = 0 Then
                DynaLog.LogMessage("Loading project...")
                MainForm.LoadDTProj(projPath & "\" & projName & "\" & projName & ".dtproj", projName, True, False)
            ElseIf OperationNum = 6 Then
                If CaptureMountDestImg Then
                    DynaLog.LogMessage("The captured Windows image has been mounted in the project.")
                    MainForm.SourceImg = SourceImg
                    MainForm.ImgIndex = ImgIndex
                    MainForm.MountDir = MountDir
                    If isReadOnly Then
                        MainForm.UpdateProjProperties(True, True)
                    Else
                        MainForm.UpdateProjProperties(True, False)
                    End If
                    MainForm.SaveDTProj()
                End If
            ElseIf OperationNum = 8 Then
                DynaLog.LogMessage("Changes have been successfully saved to the Windows image. Saving project...")
                MainForm.SaveDTProj()
            ElseIf OperationNum = 9 Then
                If imgIndexDeletionUnmount Then
                    DynaLog.LogMessage("Refreshing mounted image lists...")
                    ' Detect mounted images if the program needed to unmount the source image
                    MainForm.DetectMountedImages(False)
                    If UMountLocalDir Then
                        DynaLog.LogMessage("Updating project properties...")
                        MainForm.UpdateProjProperties(False, False)
                        MainForm.MountDir = "N/A"
                        ' This is a crucial change, so save things immediately
                        MainForm.SaveDTProj()
                        ImgMount.TextBox1.Text = ""     ' The program has a bug where mounting the same image after doing this results in the image file being ""
                        If MainForm.imgCommitOperation <> -1 Then
                            MainForm.imgCommitOperation = -1    ' Let program close on later occassions
                        End If
                    End If
                End If
            ElseIf OperationNum = 15 Then
                DynaLog.LogMessage("Updating project configuration and running background processes...")
                MainForm.SourceImg = SourceImg
                MainForm.ImgIndex = ImgIndex
                MainForm.MountDir = MountDir
                MainForm.bwBackgroundProcessAction = 0
                MainForm.bwGetImageInfo = True
                MainForm.bwGetAdvImgInfo = True
                MainForm.DetectMountedImages(False)
                If isReadOnly Then
                    MainForm.UpdateProjProperties(True, True)
                Else
                    MainForm.UpdateProjProperties(True, False)
                End If
                ' This is a crucial change, so save things immediately
                MainForm.SaveDTProj()
            ElseIf OperationNum = 18 Then
                DynaLog.LogMessage("Refreshing mounted image lists and updating project configuration...")
                MainForm.DetectMountedImages(False)
                If MainForm.isProjectLoaded And MountDir = MainForm.MountDir Then
                    MainForm.bwBackgroundProcessAction = 0
                    MainForm.bwGetImageInfo = True
                    MainForm.bwGetAdvImgInfo = True
                    If remountisReadOnly Then
                        MainForm.UpdateProjProperties(True, True)
                    Else
                        MainForm.UpdateProjProperties(True, False)
                    End If
                    MainForm.isModified = False
                End If
            ElseIf OperationNum = 20 Then
                MainForm.DetectMountedImages(False)
            ElseIf OperationNum = 21 Then
                If MainForm.isProjectLoaded And MountDir = MainForm.MountDir Or RandomMountDir = MainForm.MountDir Then
                    DynaLog.LogMessage("Updating project configuration and saving project...")
                    MainForm.bwBackgroundProcessAction = 0
                    MainForm.bwGetImageInfo = True
                    MainForm.bwGetAdvImgInfo = True
                    MainForm.UpdateProjProperties(False, False)
                    MainForm.MountDir = "N/A"
                    ' This is a crucial change, so save things immediately
                    MainForm.SaveDTProj()
                    ImgMount.TextBox1.Text = ""     ' The program has a bug where mounting the same image after doing this results in the image file being ""
                    If MainForm.imgCommitOperation <> -1 Then
                        MainForm.imgCommitOperation = -1    ' Let program close on later occassions
                    End If
                End If
                DynaLog.LogMessage("Refreshing mounted image lists...")
                MainForm.DetectMountedImages(False)
            ElseIf OperationNum = 26 Then
                DynaLog.LogMessage("Updating project configuration and saving project...")
                MainForm.ReinitializeCurImage = False
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                If Not MainForm.RunAllProcs Then MainForm.bwBackgroundProcessAction = 1
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 27 Then
                DynaLog.LogMessage("Updating project configuration and saving project...")
                MainForm.ReinitializeCurImage = False
                If Not MainForm.RunAllProcs Then MainForm.bwBackgroundProcessAction = 1
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 30 Then
                DynaLog.LogMessage("Updating project configuration and saving project...")
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 2
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 31 Then
                DynaLog.LogMessage("Updating project configuration and saving project...")
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 2
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 33 Then
                DynaLog.LogMessage("Updating project configuration and saving project...")
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 37 Then
                DynaLog.LogMessage("Updating project configuration and saving project...")
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 3
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 38 Then
                DynaLog.LogMessage("Updating project configuration and saving project...")
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 3
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 64 Then
                DynaLog.LogMessage("Updating project configuration and saving project...")
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 4
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 68 Then
                DynaLog.LogMessage("Updating project configuration and saving project...")
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 4
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 75 Then
                DynaLog.LogMessage("Updating project configuration and saving project...")
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 5
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 76 Then
                DynaLog.LogMessage("Updating project configuration and saving project...")
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 5
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 78 Then
                DynaLog.LogMessage("Updating project configuration and saving project...")
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 5
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 79 Then
                DynaLog.LogMessage("Saving project...")
                MainForm.SaveDTProj()
            ElseIf OperationNum = 991 Then
                DynaLog.LogMessage("Conversion succeeded.")
                Visible = False
                ImgConversionSuccessDialog.ShowDialog(MainForm)
                If ImgConversionSuccessDialog.DialogResult = Windows.Forms.DialogResult.OK Then
                    DynaLog.LogMessage("Opening image file location in File Explorer...")
                    Process.Start(Environment.GetEnvironmentVariable("SYSTEMROOT") & "\explorer.exe", "/select," & Quote & imgDestFile & Quote)
                End If
            ElseIf OperationNum = 996 Then
                DynaLog.LogMessage("Updating mounted image lists, updating project configuration and saving project...")
                MainForm.DetectMountedImages(False)
                MainForm.ImgIndex = SwitchTargetIndex
                MainForm.SaveDTProj()
                If SwitchMountAsReadOnly Then
                    MainForm.UpdateProjProperties(True, True)
                Else
                    MainForm.UpdateProjProperties(True, False)
                End If
                ' This is a crucial change, so save things immediately
                MainForm.SaveDTProj()
            End If
            MainForm.MenuDesc.Text = LocalizationService.ForSection("Progress.Background")("Ready.Label")
            TaskList.Clear()
            MainForm.StatusStrip.BackColor = CurrentTheme.AccentColors(1)
            MainForm.StartMountedImageDetector()
            Close()
        Else
            DynaLog.LogMessage("Tasks have not been successful.")
            Cancel_Button.Visible = True
            Label1.Text = LocalizationService.ForSection("Progress.Background")("Perform.Image.Label")
            Label2.Text = LocalizationService.ForSection("Progress.Background")("Error.Has.Message")
            CurrentPB.Value = CurrentPB.Maximum
            AllPB.Value = AllPB.Maximum
            If Not IsExpanded Then
                LogButton.PerformClick()
            End If
            CancelButtonClosesDialog = True
            Cancel_Button.Text = LocalizationService.ForSection("Progress.Background")("Ok.Button")
            LinkLabel1.Visible = True
            ' Add details for error codes
            DynaLog.LogMessage("Error code: " & errCode)
            If errCode = "C1420126" Then
                ' An image that was selected for mounting is already mounted
                LogView.AppendText(CrLf & ProgressLogText("The.Specified.Image.Is.Already.Mounted.This.Command") & Quote & ProgressLogText("Orphaned") & Quote & ProgressLogText("Images"))
            ElseIf errCode = "C142010C" Then
                ' The image, with read-only permissions, was attempted to be written
                LogView.AppendText(CrLf & ProgressLogText("The.Program.Tried.To.Save.Changes.To.An") & CrLf &
                                          ProgressLogText("To.Solve.This.Close.This.Dialog.And.Click") & Quote & ProgressLogText("Tools.Remount.Image.With.Write.Permissions") & Quote & CrLf &
                                          ProgressLogText("Do.Note.That.If.The.Image.Came.From"))
            ElseIf errCode = "C1420117" Then
                ' Some applications (or hidden processes) have open handles on the mount dir
                LogView.AppendText(CrLf & ProgressLogText("The.Program.Tried.To.Unmount.The.Image.But") & CrLf &
                                          ProgressLogText("Make.Sure.No.Application.Or.Process.Is.Using") & CrLf &
                                          ProgressLogText("If.The.Error.Occurred.At.The.End.Of"))
            ElseIf errCode = "C142011D" Then
                ' A partial unmount or an in-progress mount operation happened
                LogView.AppendText(CrLf & ProgressLogText("The.Mounted.Image.Cannot.Be.Committed.Back.Into") & CrLf &
                                          ProgressLogText("A.Partial.Unmount.Might.Have.Happened.Or.The") & CrLf &
                                          ProgressLogText("If.The.Image.Was.Unmounted.Whilst.Saving.Changes"))
            ElseIf errCode = "C1510111" Then
                ' The specified image, that was marked to mount with read-write permissions, came from a read-only source (e.g., a Windows installation disc)
                LogView.AppendText(CrLf & ProgressLogText("The.Source.File.Comes.From.A.Read.Only") & CrLf &
                                          ProgressLogText("Please.Re.Specify.The.Image.In.The.Mount") & Quote & ProgressLogText("Read.Only") & Quote & ProgressLogText("Check.Box.You.Can.Also.Try.Copying.The"))
            ElseIf errCode = "00000087" Then
                ' Internal errors
                LogView.AppendText(CrLf & ProgressLogText("There.Is.Essential.Data.That.Was.Not.Picked"))
            ElseIf OperationNum = 26 Then
                ' No packages have been added successfully
                LogView.AppendText(CrLf & ProgressLogText("No.Packages.Have.Been.Added.Successfully.Try.Looking"))
            ElseIf OperationNum = 27 Then
                ' No packages have been removed successfully
                LogView.AppendText(CrLf & ProgressLogText("No.Packages.Have.Been.Removed.Successfully.Try.Looking"))
            ElseIf OperationNum = 30 Then
                ' No features have been enabled successfully
                LogView.AppendText(CrLf & ProgressLogText("No.Features.Have.Been.Enabled.Successfully.Try.Looking"))
            ElseIf OperationNum = 31 Then
                ' No features have been disabled successfully
                LogView.AppendText(CrLf & ProgressLogText("No.Features.Have.Been.Disabled.Successfully.Try.Looking"))
            ElseIf OperationNum = 78 Then
                ' Cause is undetermined
                LogView.AppendText(CrLf & ProgressLogText("Either.This.Operation.Has.Failed.Or.Some.Drivers") & CrLf & CrLf &
                                   ProgressLogText("If.There.Are.Driver.Changes.Consider.Reading.The") & CrLf & CrLf &
                                   ProgressLogText("You.Can.Also.Manually.Customize.The.Export.Directory"))
            ElseIf errCode = "00000001" Then

            ElseIf errCode = "C000013A" Then
                ' Keyboard interrupt (Ctrl-C) or forced program closure. The former may not trigger this error, as it may trigger error 1223
                LogView.AppendText(CrLf & ProgressLogText("The.Program.Has.Suffered.A.Keyboard.Interrupt.Or"))
            ElseIf errCode = "C2FE0101" Then
                ' This happens on operation numbers 90, 91, and 92; related to Microsoft Edge servicing, if the components have already been installed.
                ' Since these operation numbers are meant for different things, detect them
                If OperationNum = 90 Then
                    LogView.AppendText(CrLf & ProgressLogText("The.Microsoft.Edge.Components.Have.Already.Been.Installed"))
                ElseIf OperationNum = 91 Then
                    LogView.AppendText(CrLf & ProgressLogText("The.Microsoft.Edge.Browser.Has.Already.Been.Installed"))
                ElseIf OperationNum = 92 Then
                    LogView.AppendText(CrLf & ProgressLogText("The.Microsoft.Edge.WebView2.Component.Has.Already.Been"))
                End If
            ElseIf errCode = "800F0806" Then
                ' There are pending image operations
                LogView.AppendText(CrLf & ProgressLogText("The.Operation.Could.Not.Be.Performed.Because.This"))
            ElseIf errCode = "BC2" Then
                DynaLog.LogMessage("The task has succeded but requires a restart...")
                If OperationNum = 86 Then
                    DynaLog.LogMessage("Rollback initiated. Restarting system automatically in 10 seconds...")
                    LogView.AppendText(CrLf & ProgressLogText("The.Rollback.Process.Has.Started.Your.System.Needs"))
                    Dim restartProc As New Process()
                    restartProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\shutdown.exe"
                    restartProc.StartInfo.Arguments = "/r /t 10 /c " & Quote & "Shutdown initiated by DISMTools" & Quote
                    restartProc.StartInfo.CreateNoWindow = True
                    restartProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    restartProc.Start()
                Else
                    LogView.AppendText(CrLf & ProgressLogText("The.Specified.Operation.Completed.Successfully.But.Requires.A"))
                End If
            Else
                Try
                    Dim exitDesc As New Win32Exception(Int32.Parse(errCode, Globalization.NumberStyles.HexNumber))
                    LogView.AppendText(CrLf & CrLf & exitDesc.Message)
                Catch ex As Exception
                    ' Errors that weren't added to the database
                    LogView.AppendText(CrLf & ProgressLogText("This.Error.Has.Not.Yet.Been.Added.To"))
                End Try
            End If
            LogView.AppendText(CrLf & CrLf & ProgressLogText("For.Detailed.Information.Consider.Reading.The.DISM.Operation"))
            MainForm.MenuDesc.Text = LocalizationService.ForSection("Progress.Background")("Ready.Item")
            MainForm.StatusStrip.BackColor = CurrentTheme.AccentColors(1)
            SaveLog(Application.StartupPath & "\logs\DISMTools.log")
            SaveDismOutput(Application.StartupPath & "\logs\DISM_Output_" & Date.Now.ToString("yy-MM-dd-HH-mm-ss") & ".log")
        End If
    End Sub

    Sub GetErrorCode(Bypass As Boolean)
        If Bypass Then
            errCode = 0
        Else
            errCode = Hex(Decimal.ToInt32(DismExitCode))
        End If
        Select Case errCode
            Case 0
                IsSuccessful = True
            Case Else
                IsSuccessful = False
        End Select
    End Sub

    Private Sub ProgressPanel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        EnableExperiments = MainForm.EnableExperiments
        DynaLog.LogMessage("Preparing to start image operations...")
        Text = LocalizationService.ForSection("Progress")("Progress.Label")
        Label1.Text = LocalizationService.ForSection("Progress")("Image.Operations.Label")
        Label2.Text = LocalizationService.ForSection("Progress")("Wait.Tasks.Label")
        CancelButtonClosesDialog = False
        Cancel_Button.Text = LocalizationService.ForSection("Progress")("Cancel.Button")
        LogButton.Text = If(Not IsExpanded, LocalizationService.ForSection("Progress")("ShowLog.Label"), LocalizationService.ForSection("Progress")("HideLog.Label"))
        LinkLabel1.Text = LocalizationService.ForSection("Progress")("Show.Dismlog.File.Link")
        allTasks.Text = LocalizationService.ForSection("Progress")("Wait.Label")
        currentTask.Text = LocalizationService.ForSection("Progress")("CurrentTask.Label")
        PrepareAllReporters()
        If MainForm.ExpandedProgressPanel AndAlso Not IsExpanded Then
            LogButton.PerformClick()
        End If
        PreventSystemFromSleeping = MainForm.PreventSystemFromSleeping
        If PreventSystemFromSleeping Then
            ' Disable sleep mode now
            DynaLog.LogMessage("Preventing the system from sleeping...")
            PowerManagementHelper.DisableSystemSleepMode()
        End If
        taskCountLbl.Visible = False
        MainForm.bwBackgroundProcessAction = 0
        MainForm.bwGetImageInfo = True
        MainForm.bwGetAdvImgInfo = True
        AllDrivers = MainForm.AllDrivers
        BodyPanel.BorderStyle = BorderStyle.None
        If MainForm.CurrentImage IsNot Nothing Then
            ReferenceImage = MainForm.CurrentImage
            ImgVersion = MainForm.CurrentImage.ImageVersion
        End If
        ' Determine program colors
        BodyPanel.BackColor = CurrentTheme.BackgroundColor
        BodyPanel.ForeColor = CurrentTheme.ForegroundColor
        LogView.BackColor = CurrentTheme.BackgroundColor
        LogView.ForeColor = CurrentTheme.ForegroundColor
        DISM_LogView.RichTextBox1.BackColor = CurrentTheme.BackgroundColor
        DISM_LogView.RichTextBox1.ForeColor = CurrentTheme.ForegroundColor
        LogSwitcherPic1.Image = GetGlyphResource("options_logs")
        LogSwitcherPic2.Image = GetGlyphResource("options_output")
        LogSwitcherPic1.FlatAppearance.MouseOverBackColor = Color.DarkGray
        LogSwitcherPic1.FlatAppearance.MouseDownBackColor = Color.DimGray
        LogSwitcherPic2.FlatAppearance.MouseOverBackColor = Color.DarkGray
        LogSwitcherPic2.FlatAppearance.MouseDownBackColor = Color.DimGray
        LogSwitcherPic1.FlatAppearance.BorderColor = CurrentTheme.ForegroundColor
        LogSwitcherPic2.FlatAppearance.BorderColor = CurrentTheme.ForegroundColor
        CurrentPB.Value = 0
        AllPB.Value = 0
        If LogView.Text <> "" Then LogView.Clear()
        If DISM_LogView.RichTextBox1.Text <> "" Then DISM_LogView.RichTextBox1.Clear()
        ' It does not have any purpose when doing tasks yet
        Cancel_Button.Visible = False
        ' If running, cancel background processes
        DynaLog.LogMessage("Detecting if background processes are busy...")
        If MainForm.ImgBW.IsBusy Then
            DynaLog.LogMessage("Background processes are running. Cancelling them...")
            ' Make form visible sooner. We may have to set more things up here,
            ' but we'll see
            Visible = True
            LogView.AppendText(ProgressLogText("Cancelling.Background.Processes"))
            MainForm.ImgBW.CancelAsync()
            While MainForm.ImgBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(100)
            End While
            ' TODO: Grab items remaining to finish the background processes
        End If
        ' Cancel detector background worker which can interfere with image operations and cause crashes due to access violations
        DynaLog.LogMessage("Mounted image detector might be busy. Stopping it if it is...")
        MainForm.StopMountedImageDetector()
        DynaLog.LogMessage("Setting mount directory target for operations...")
        DynaLog.LogMessage("Images mounted in this system: " & MainForm.MountedImageList.Count)
        ' Go through all mounted images to determine which one to get info from with the DISM API,
        ' if a project has been loaded and if that project has a mounted image
        If MainForm.MountedImageList.Count > 0 Then
            Dim imageToProcess As WindowsImage = MainForm.MountedImageList.FirstOrDefault(Function(mountedImage) mountedImage.ImageMountDirectory = MainForm.MountDir)
            If imageToProcess IsNot Nothing Then
                mntString = imageToProcess.ImageMountDirectory
            End If
        End If
        If MainForm.OfflineManagement Then mntString = MainForm.MountDir
        DismProgram = MainForm.DismExe
        If MountDir = "" Then MountDir = MainForm.MountDir
        DISMProc.StartInfo.CreateNoWindow = False
        DynaLog.LogMessage("Setting log font settings...")
        Try
            If MainForm.LogFontIsBold Then
                LogView.Font = New Font(MainForm.LogFont, MainForm.LogFontSize, FontStyle.Bold)
            Else
                LogView.Font = New Font(MainForm.LogFont, MainForm.LogFontSize)
            End If
        Catch ex As Exception
            LogView.Font = New Font("Consolas", 11.25)
        End Try
        DISM_LogView.Font = LogView.Font
        MainForm.MenuDesc.Text = LocalizationService.ForSection("Progress")("Performing.Image.Ops.Button")
        MainForm.StatusStrip.BackColor = CurrentTheme.AccentColors(3)
        If Debugger.IsAttached Then
            IsDebugged = True
        Else
            IsDebugged = False
        End If
        Control.CheckForIllegalCrossThreadCalls = False
        LinkLabel1.Visible = False
        DynaLog.LogMessage("Detecting presence of directory in which operation logs are stored...")
        If Not Directory.Exists(Application.StartupPath & "\logs") Then
            Try
                Directory.CreateDirectory(Application.StartupPath & "\logs")
            Catch ex As Exception
                ' don't create such a folder then
            End Try
        End If
        ' Detect settings
        DynaLog.LogMessage("Configuring settings...")
        OnlineMgmt = MainForm.OnlineManagement
        AutoLogs = MainForm.AutoLogs
        LogPath = MainForm.LogFile
        LogLevel = MainForm.LogLevel
        QuietOps = MainForm.QuietOperations
        SkipSysRestart = MainForm.SysNoRestart
        UseScratchDir = MainForm.UseScratch
        AutoScratch = MainForm.AutoScrDir
        ScratchDirPath = MainForm.ScratchDir
        EnglishOut = MainForm.EnglishOutput
        SystemEditor = MainForm.SystemEditor
        DynaLog.LogMessage("Provided system editor for logs: " & Quote & SystemEditor & Quote)
        DynaLog.LogMessage("Checking if provided system editor exists...")
        If Not File.Exists(SystemEditor) Then
            DynaLog.LogMessage("Provided system editor does not exist. Defaulting to notepad...")
            SystemEditor = SystemEditorBackup
        End If
        DynaLog.LogMessage("Preparing scratch directory if program is configured to use default directories...")
        If UseScratchDir And AutoScratch And OnlineMgmt And Not Directory.Exists(Application.StartupPath & "\scratch") Then Directory.CreateDirectory(Application.StartupPath & "\scratch")
        GatherInitialSwitches()
        DynaLog.LogMessage("Detecting tasks in task list...")
        If TaskList IsNot Nothing AndAlso TaskList.Count > 0 Then
            DynaLog.LogMessage("Task count in task list: " & TaskList.Count)
        End If
        If TaskList.Count >= 2 Then
            DynaLog.LogMessage("More than 2 tasks will be made.")
            AllPB.Maximum = TaskList.Count * 100
            taskCountLbl.Text = LocalizationService.ForSection("Progress").Format("TaskCount.Label", TaskList.Count)
            OperationNum = 1000
        Else
            DynaLog.LogMessage("Getting the tasks of the specified operation...")
            GetTasks(OperationNum)
        End If
        taskCountLbl.Visible = True
        DynaLog.LogMessage("Getting state of image registry control panel...")
        If RegistryControlPanel.Visible Then
            DynaLog.LogMessage("Image registry control panel is open. Attempting to close...")
            RegistryControlPanel.Close()
            If RegistryControlPanel.Visible Then
                DynaLog.LogMessage("Second check determined the image registry control panel is still open. Cannot continue performing tasks until it's closed")
                LogView.AppendText(CrLf & ProgressLogText("The.Image.Registry.Hives.Need.To.Be.Unloaded"))
            End If
        End If
        If Not RegistryControlPanel.Visible Then
            DynaLog.LogMessage("The image registry control panel is no longer open. Performing tasks...")
            ProgressBW.RunWorkerAsync()
        Else
            DynaLog.LogMessage("The image registry control panel is still open.")
            Visible = True
            Application.DoEvents()
            Thread.Sleep(2000)
            Close()
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Try
            DynaLog.LogMessage("Checking if log file exists and opening it in Notepad...")
            If File.Exists(Application.StartupPath & "\logs\" & dateStr) Then
                Process.Start(SystemEditor, Application.StartupPath & "\logs\" & dateStr)
            ElseIf File.Exists(LogPath) Then
                Process.Start(SystemEditor, LogPath)
            End If
        Catch ex As Exception
            If Not File.Exists(SystemEditor) Then
                DynaLog.LogMessage("The system editor was not found on this system.")
                LogView.AppendText(CrLf & ProgressLogText("System.Editor.Was.Not.Found"))
            ElseIf Not File.Exists(Application.StartupPath & "\logs\" & dateStr) Or Not File.Exists(LogPath) Then
                DynaLog.LogMessage("The log file is not found on this system.")
                LogView.AppendText(CrLf & ProgressLogText("The.Log.File.Was.Not.Found"))
            End If
        End Try
    End Sub

    Private Sub BodyPanel_Paint(sender As Object, e As PaintEventArgs) Handles BodyPanel.Paint
        ControlPaint.DrawBorder(e.Graphics, BodyPanel.ClientRectangle, CurrentTheme.AccentColors(1), ButtonBorderStyle.Solid)
    End Sub

    Private Sub ProgressPanel_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        MainForm.MenuDesc.Text = LocalizationService.ForSection("Progress.Close")("Ready.Label")
        MainForm.StatusStrip.BackColor = CurrentTheme.AccentColors(1)
        MainForm.StartMountedImageDetector()
    End Sub

    Sub SwitchLogContext(Context As Integer)
        DynaLog.LogMessage("Switching operation log context...")
        DynaLog.LogMessage("- New Context: " & Context)
        If Context = 0 Then
            NativeMethods.SendMessage(LogView.Handle, &H115, 7, IntPtr.Zero)
        End If
        LogSwitcherPic1.FlatAppearance.BorderSize = If(Context = 0, 1, 0)
        LogSwitcherPic2.FlatAppearance.BorderSize = If(Context = 1, 1, 0)
        DT_OpLogs.Visible = (Context = 0)
        DISM_OpLogs.Visible = (Context = 1)
    End Sub

    Private Sub LogSwitcher1_LinkClicked(sender As Object, e As EventArgs) Handles LogSwitcherPic1.Click
        SwitchLogContext(0)
    End Sub

    Private Sub LogSwitcher2_LinkClicked(sender As Object, e As EventArgs) Handles LogSwitcherPic2.Click
        SwitchLogContext(1)
    End Sub

    Private Sub LogSwitcherPic1_MouseHover(sender As Object, e As EventArgs) Handles LogSwitcherPic1.MouseHover
        Dim olcText As String = ""
                olcText = LocalizationService.ForSection("Progress.Logs.Operation")("Label")
        WindowHelper.DisplayToolTip(sender, olcText)
    End Sub

    Private Sub LogSwitcherPic2_MouseHover(sender As Object, e As EventArgs) Handles LogSwitcherPic2.MouseHover
        Dim olcText As String = ""
                olcText = LocalizationService.ForSection("Progress.Logs.DismOutput")("Label")
        WindowHelper.DisplayToolTip(sender, olcText)
    End Sub

    Private Sub ProgressPanel_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If WindowState = FormWindowState.Maximized Then
            WindowState = FormWindowState.Normal
        End If
    End Sub
End Class
