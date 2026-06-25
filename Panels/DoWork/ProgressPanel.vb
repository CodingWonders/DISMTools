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

    Dim Language As Integer = 0                             ' Form language, taken from MainForm

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
        If Cancel_Button.Text = "Cancel" Or Cancel_Button.Text = "Cancelar" Or Cancel_Button.Text = "Annulla" Then
            ProgressBW.CancelAsync()
        ElseIf Cancel_Button.Text = "OK" Or Cancel_Button.Text = "Aceptar" Then
            Close()
        End If
    End Sub

    Private Sub LogButton_Click(sender As Object, e As EventArgs) Handles LogButton.Click
        Dim collapsedHeight As Integer = WindowHelper.ScaleLogical(240)
        Dim expandedHeight As Integer = WindowHelper.ScaleLogical(420)
        If Not IsExpanded Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            LogButton.Text = "Hide log"
                        Case "ESN"
                            LogButton.Text = "Ocultar registro"
                        Case "FRA"
                            LogButton.Text = "Cacher le journal"
                        Case "PTB", "PTG"
                            LogButton.Text = "Ocultar registo"
                        Case "ITA"
                            LogButton.Text = "Nascondi registro"
                    End Select
                Case 1
                    LogButton.Text = "Hide log"
                Case 2
                    LogButton.Text = "Ocultar registro"
                Case 3
                    LogButton.Text = "Cacher le journal"
                Case 4
                    LogButton.Text = "Ocultar registo"
                Case 5
                    LogButton.Text = "Nascondi registro"
            End Select
            Height = expandedHeight
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            LogButton.Text = "Show log"
                        Case "ESN"
                            LogButton.Text = "Mostrar registro"
                        Case "FRA"
                            LogButton.Text = "Afficher le journal"
                        Case "PTB", "PTG"
                            LogButton.Text = "Mostrar registo"
                        Case "ITA"
                            LogButton.Text = "Visualizza registro"
                    End Select
                Case 1
                    LogButton.Text = "Show log"
                Case 2
                    LogButton.Text = "Mostrar registro"
                Case 3
                    LogButton.Text = "Afficher le journal"
                Case 4
                    LogButton.Text = "Mostrar registo"
                Case 5
                    LogButton.Text = "Visualizza registro"
            End Select
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
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        taskCountLbl.Text = "Tasks: 1/" & taskCount
                    Case "ESN"
                        taskCountLbl.Text = "Tareas: 1/" & taskCount
                    Case "FRA"
                        taskCountLbl.Text = "Tâches : 1/" & taskCount
                    Case "PTB", "PTG"
                        taskCountLbl.Text = "Tarefas: 1/" & taskCount
                    Case "ITA"
                        taskCountLbl.Text = "Attività: 1/" & taskCount
                End Select
            Case 1
                taskCountLbl.Text = "Tasks: 1/" & taskCount
            Case 2
                taskCountLbl.Text = "Tareas: 1/" & taskCount
            Case 3
                taskCountLbl.Text = "Tâches : 1/" & taskCount
            Case 4
                taskCountLbl.Text = "Tarefas: 1/" & taskCount
            Case 5
                taskCountLbl.Text = "Attività: 1/" & taskCount
        End Select
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
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskList.Count
                        Case "ESN"
                            taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskList.Count
                        Case "FRA"
                            taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskList.Count
                        Case "PTB", "PTG"
                            taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskList.Count
                        Case "ITA"
                            taskCountLbl.Text = "Attività: " & currentTCont & "/" & taskList.Count
                    End Select
                Case 1
                    taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskList.Count
                Case 2
                    taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskList.Count
                Case 3
                    taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskList.Count
                Case 4
                    taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskList.Count
                Case 5
                    taskCountLbl.Text = "Attività: " & currentTCont & "/" & taskList.Count
            End Select
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Creating project: " & Quote & projName & Quote
                        currentTask.Text = "Creating DISMTools project structure..."
                    Case "ESN"
                        allTasks.Text = "Creando proyecto: " & Quote & projName & Quote
                        currentTask.Text = "Creando estructura del proyecto de DISMTools..."
                    Case "FRA"
                        allTasks.Text = "Création d'un projet en cours : " & Quote & projName & Quote
                        currentTask.Text = "Création de la structure du projet DISMTools en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Criar projeto: " & Quote & projName & Quote
                        currentTask.Text = "Criar a estrutura do projeto DISMTools..."
                    Case "ITA"
                        allTasks.Text = "Creazione di progetto: " & Quote & projName & Quote
                        currentTask.Text = "Creazione struttura progetto DISMTools..."
                End Select
            Case 1
                allTasks.Text = "Creating project: " & Quote & projName & Quote
                currentTask.Text = "Creating DISMTools project structure..."
            Case 2
                allTasks.Text = "Creando proyecto: " & Quote & projName & Quote
                currentTask.Text = "Creando estructura del proyecto de DISMTools..."
            Case 3
                allTasks.Text = "Création d'un projet en cours : " & Quote & projName & Quote
                currentTask.Text = "Création de la structure du projet DISMTools en cours..."
            Case 4
                allTasks.Text = "Criar projeto: " & Quote & projName & Quote
                currentTask.Text = "Criar a estrutura do projeto DISMTools..."
            Case 5
                allTasks.Text = "Creazione di progetto: " & Quote & projName & Quote
                currentTask.Text = "Creazione struttura progetto DISMTools..."
        End Select
        LogView.AppendText(CrLf & "Creating project structure...")
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
            LogView.AppendText(CrLf & "Project created successfully.")
            CurrentPB.Value = CurrentPB.Maximum
            AllPB.Value = AllPB.Maximum
            IsSuccessful = True
        Catch ex As Exception
            DynaLog.LogMessage("Could not create the project. Error message: " & ex.Message)
            LogView.AppendText(CrLf & "An error has occurred. Please read the details below: " & CrLf & ex.GetType().ToString() & ": " & Err.Description)
            If IsDebugged Then
                LogView.AppendText(CrLf & "Debugging information: " & ex.StackTrace)
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Appending to image..."
                        currentTask.Text = "Appending specified mount directory to the specified target image..."
                    Case "ESN"
                        allTasks.Text = "Anexando a la imagen..."
                        currentTask.Text = "Anexando el directorio de montaje especificado a la imagen de destino..."
                    Case "FRA"
                        allTasks.Text = "Annexe à l'image... "
                        currentTask.Text = "Annexe du répertoire de montage spécifié à l'image cible spécifiée..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Anexo à imagem..."
                        currentTask.Text = "Anexo do diretório de montagem especificado à imagem de destino especificada..."
                    Case "ITA"
                        allTasks.Text = "Applicazione all'immagine..."
                        currentTask.Text = "Applicazione cartella montaggio specificata all'immagine destinazione specificata..."
                End Select
            Case 1
                allTasks.Text = "Appending to image..."
                currentTask.Text = "Appending specified mount directory to the specified target image..."
            Case 2
                allTasks.Text = "Anexando a la imagen..."
                currentTask.Text = "Anexando el directorio de montaje especificado a la imagen de destino..."
            Case 3
                allTasks.Text = "Annexe à l'image... "
                currentTask.Text = "Annexe du répertoire de montage spécifié à l'image cible spécifiée..."
            Case 4
                allTasks.Text = "Anexo à imagem..."
                currentTask.Text = "Anexo do diretório de montagem especificado à imagem de destino especificada..."
            Case 5
                allTasks.Text = "Applicazione all'immagine..."
                currentTask.Text = "Applicazione cartella montaggio specificata all'immagine destinazione specificata..."
        End Select
        LogView.AppendText(CrLf & "Appending mount directory to specified target image..." & CrLf & "Options:" & CrLf &
                           "- Source image directory: " & AppendixSourceDir & CrLf &
                           "- Destination image file: " & AppendixDestinationImage & CrLf &
                           "- Destination image name: " & AppendixName & CrLf &
                           "- Destination image description: " & If(AppendixDescription = "", "(none specified)", AppendixDescription) & CrLf)
        If AppendixWimScriptConfig = "" Then
            DynaLog.LogMessage("No configuration list file has been specified.")
            LogView.AppendText("- Configuration list file: not specified" & CrLf)
        Else
            DynaLog.LogMessage("A configuration list file has been specified. Checking if it exists...")
            LogView.AppendText("- Configuration list file: " & Quote & AppendixWimScriptConfig & Quote & CrLf)
            If Not File.Exists(AppendixWimScriptConfig) Then
                DynaLog.LogMessage("The configuration list file does not exist in the file system and will be skipped.")
                LogView.AppendText("   WARNING: the configuration list file does not exist in the file system. Skipping file..." & CrLf)
            End If
        End If
        LogView.AppendText("- Append image with WIMBoot configuration? " & If(AppendixUseWimBoot, "Yes", "No") & CrLf &
                           "- Make image bootable? " & If(AppendixBootable, "Yes", "No") & CrLf &
                           "- Verify image integrity? " & If(AppendixCheckIntegrity, "Yes", "No") & CrLf &
                           "- Check for file errors? " & If(AppendixVerify, "Yes", "No") & CrLf &
                           "- Use the reparse point tag fix? " & If(AppendixReparsePt, "Yes", "No") & CrLf &
                           "- Capture extended attributes? " & If(AppendixCaptureExtendedAttribs, "Yes", "No"))
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta livello errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        End If
    End Sub

    Private Sub ApplyFfuImage()
        DynaLog.LogMessage("Applying specified FFU image to the specified application drive...")
        DynaLog.LogMessage("- Image to apply: " & Quote & FFUApplicationSourceImg & Quote)
        DynaLog.LogMessage("- Application drive: " & Quote & FFUApplicationDestDrive & Quote)
        DynaLog.LogMessage("- SFU name pattern: " & Quote & FFUApplicationSFUPattern & Quote)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Applying image..."
                        currentTask.Text = "Applying specified image to the specified destination..."
                    Case "ESN"
                        allTasks.Text = "Aplicando imagen..."
                        currentTask.Text = "Aplicando imagen especificada al destino especificado..."
                    Case "FRA"
                        allTasks.Text = "Application de l'image en cours..."
                        currentTask.Text = "Application de l'image spécifiée à la destination spécifiée en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Aplicar imagem..."
                        currentTask.Text = "Aplicar a imagem especificada ao destino especificado..."
                    Case "ITA"
                        allTasks.Text = "Applicazione dell'immagine..."
                        currentTask.Text = "Applicazione immagine specificata alla destinazione specificata..."
                End Select
            Case 1
                allTasks.Text = "Applying image..."
                currentTask.Text = "Applying specified image to the specified destination..."
            Case 2
                allTasks.Text = "Aplicando imagen..."
                currentTask.Text = "Aplicando imagen especificada al destino especificado..."
            Case 3
                allTasks.Text = "Application de l'image en cours..."
                currentTask.Text = "Application de l'image spécifiée à la destination spécifiée en cours..."
            Case 4
                allTasks.Text = "Aplicar imagem..."
                currentTask.Text = "Aplicar a imagem especificada ao destino especificado..."
            Case 5
                allTasks.Text = "Applicazione dell'immagine..."
                currentTask.Text = "Applicazione dell'immagine specificata alla destinazione specificata..."
        End Select
        LogView.AppendText(CrLf & "Applying image..." & CrLf & "Options:" & CrLf &
                           "- Source image file: " & ApplicationSourceImg & CrLf &
                           "- Index to apply: " & ApplicationIndex & CrLf &
                           "- Target directory: " & ApplicationDestDir & CrLf)
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
            LogView.AppendText("- Split FFU (SFU) file pattern: not specified/not using SFU file" & CrLf)
        Else
            LogView.AppendText("- Split FFU (SFU) file pattern: " & FFUApplicationSFUPattern & CrLf)
            CommandArgs &= " /sfufile=" & FFUApplicationSFUPattern
        End If
        RunProcess(DismProgram, CommandArgs)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta livello errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Applying image..."
                        currentTask.Text = "Applying specified image to the specified destination..."
                    Case "ESN"
                        allTasks.Text = "Aplicando imagen..."
                        currentTask.Text = "Aplicando imagen especificada al destino especificado..."
                    Case "FRA"
                        allTasks.Text = "Application de l'image en cours..."
                        currentTask.Text = "Application de l'image spécifiée à la destination spécifiée en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Aplicar imagem..."
                        currentTask.Text = "Aplicar a imagem especificada ao destino especificado..."
                    Case "ITA"
                        allTasks.Text = "Applicazione dell'immagine..."
                        currentTask.Text = "Applicazione immagine specificata alla destinazione specificata..."
                End Select
            Case 1
                allTasks.Text = "Applying image..."
                currentTask.Text = "Applying specified image to the specified destination..."
            Case 2
                allTasks.Text = "Aplicando imagen..."
                currentTask.Text = "Aplicando imagen especificada al destino especificado..."
            Case 3
                allTasks.Text = "Application de l'image en cours..."
                currentTask.Text = "Application de l'image spécifiée à la destination spécifiée en cours..."
            Case 4
                allTasks.Text = "Aplicar imagem..."
                currentTask.Text = "Aplicar a imagem especificada ao destino especificado..."
            Case 5
                allTasks.Text = "Applicazione dell'immagine..."
                currentTask.Text = "Applicazione dell'immagine specificata alla destinazione specificata..."
        End Select
        LogView.AppendText(CrLf & "Applying image..." & CrLf & "Options:" & CrLf &
                           "- Source image file: " & ApplicationSourceImg & CrLf &
                           "- Index to apply: " & ApplicationIndex & CrLf &
                           "- Target directory: " & ApplicationDestDir & CrLf)
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
            LogView.AppendText("- Verify image integrity? Yes" & CrLf)
            CommandArgs &= " /checkintegrity"
        Else
            LogView.AppendText("- Verify image integrity? No" & CrLf)
        End If
        If ApplicationVerify Then
            LogView.AppendText("- Check for file errors? Yes" & CrLf)
            CommandArgs &= " /verify"
        Else
            LogView.AppendText("- Check for file errors? No" & CrLf)
        End If
        If ApplicationReparsePt Then
            LogView.AppendText("- Use reparse point tag fix? Yes" & CrLf)
        Else
            LogView.AppendText("- Use reparse point tag fix? No" & CrLf)
            CommandArgs &= " /norpfix"
        End If
        If ApplicationSWMPattern = "" Then
            LogView.AppendText("- Split WIM (SWM) file pattern: not specified/not using SWM file" & CrLf)
        Else
            LogView.AppendText("- Split WIM (SWM) file pattern: " & ApplicationSWMPattern & CrLf)
            CommandArgs &= " /swmfile=" & ApplicationSWMPattern
        End If
        If ApplicationValidateForTD Then
            LogView.AppendText("- Validate for Trusted Desktop? Yes" & CrLf)
            CommandArgs &= " /confirmtrustedfile"
        Else
            LogView.AppendText("- Validate for Trusted Desktop? No/Not supported" & CrLf)
        End If
        If ApplicationUseWimBoot Then
            LogView.AppendText("- Apply using WIMBoot configuration? Yes" & CrLf)
            CommandArgs &= " /wimboot"
        Else
            LogView.AppendText("- Apply using WIMBoot configuration? No" & CrLf)
        End If
        If ApplicationCompactMode Then
            LogView.AppendText("- Use Compact mode? Yes" & CrLf)
            CommandArgs &= " /compact"
        Else
            LogView.AppendText("- Use Compact mode? No" & CrLf)
        End If
        If ApplicationUseExtAttr Then
            LogView.AppendText("- Apply using extended attributes? Yes" & CrLf)
            CommandArgs &= " /ea"
        Else
            LogView.AppendText("- Apply using extended attributes? No" & CrLf)
        End If
        RunProcess(DismProgram, CommandArgs)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta livello errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        End If
    End Sub

    Private Sub CaptureFfuImage()
        DynaLog.LogMessage("Capturing physical drive to the target image...")
        DynaLog.LogMessage("- Source drive: " & FFUCaptureSourceDrive)
        DynaLog.LogMessage("- Destination image: " & Quote & FFUCaptureDestinationFfuImage & Quote)
        DynaLog.LogMessage("- Destination image name: " & Quote & FFUCaptureName & Quote)
        DynaLog.LogMessage("- Destination image description: " & Quote & FFUCaptureDescription & Quote)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Capturing image..."
                        currentTask.Text = "Capturing specified directory into a new image..."
                    Case "ESN"
                        allTasks.Text = "Capturando imagen..."
                        currentTask.Text = "Capturando directorio especificado en una nueva imagen..."
                    Case "FRA"
                        allTasks.Text = "Capture de l'image en cours..."
                        currentTask.Text = "Capture du répertoire spécifié dans une nouvelle image en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Capturar imagem..."
                        currentTask.Text = "Capturar o diretório especificado para uma nova imagem..."
                    Case "ITA"
                        allTasks.Text = "Cattura immagine..."
                        currentTask.Text = "Cattura cartella specificata in una nuova immagine..."
                End Select
            Case 1
                allTasks.Text = "Capturing image..."
                currentTask.Text = "Capturing specified directory into a new image..."
            Case 2
                allTasks.Text = "Capturando imagen..."
                currentTask.Text = "Capturando directorio especificado en una nueva imagen..."
            Case 3
                allTasks.Text = "Capture de l'image en cours..."
                currentTask.Text = "Capture du répertoire spécifié dans une nouvelle image en cours..."
            Case 4
                allTasks.Text = "Capturar imagem..."
                currentTask.Text = "Capturar o diretório especificado para uma nova imagem..."
            Case 5
                allTasks.Text = "Cattura immagine..."
                currentTask.Text = "Cattura cartella specificata in una nuova immagine..."
        End Select
        LogView.AppendText(CrLf & "Capturing directory..." & CrLf & "Options:" & CrLf &
                           "- Source directory: " & FFUCaptureSourceDrive & CrLf &
                           "- Destination image: " & FFUCaptureDestinationFfuImage & CrLf &
                           "- Captured image name: " & FFUCaptureName & CrLf)
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
            LogView.AppendText("- Captured image description: none specified" & CrLf)
        Else
            DynaLog.LogMessage("A description has been provided.")
            LogView.AppendText("- Captured image description: " & Quote & FFUCaptureDescription & Quote & CrLf)
            CommandArgs &= " /description=" & Quote & FFUCaptureDescription & Quote
        End If
        If FFUCaptureCompressType = 0 Then
            LogView.AppendText("- Compression type: none" & CrLf)
            CommandArgs &= " /compress=none"
        ElseIf FFUCaptureCompressType = 1 Then
            LogView.AppendText("- Compression type: default" & CrLf)
            CommandArgs &= " /compress=default"
        End If
        LogView.AppendText(CrLf & "Capturing image...")
        RunProcess(DismProgram, CommandArgs)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta livello errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Capturing image..."
                        currentTask.Text = "Capturing specified directory into a new image..."
                    Case "ESN"
                        allTasks.Text = "Capturando imagen..."
                        currentTask.Text = "Capturando directorio especificado en una nueva imagen..."
                    Case "FRA"
                        allTasks.Text = "Capture de l'image en cours..."
                        currentTask.Text = "Capture du répertoire spécifié dans une nouvelle image en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Capturar imagem..."
                        currentTask.Text = "Capturar o diretório especificado para uma nova imagem..."
                    Case "ITA"
                        allTasks.Text = "Cattura immagine..."
                        currentTask.Text = "Cattura cartella specificata in una nuova immagine..."
                End Select
            Case 1
                allTasks.Text = "Capturing image..."
                currentTask.Text = "Capturing specified directory into a new image..."
            Case 2
                allTasks.Text = "Capturando imagen..."
                currentTask.Text = "Capturando directorio especificado en una nueva imagen..."
            Case 3
                allTasks.Text = "Capture de l'image en cours..."
                currentTask.Text = "Capture du répertoire spécifié dans une nouvelle image en cours..."
            Case 4
                allTasks.Text = "Capturar imagem..."
                currentTask.Text = "Capturar o diretório especificado para uma nova imagem..."
            Case 5
                allTasks.Text = "Cattura immagine..."
                currentTask.Text = "Cattura cartella specificata in una nuova immagine..."
        End Select
        LogView.AppendText(CrLf & "Capturing directory..." & CrLf & "Options:" & CrLf &
                           "- Source directory: " & CaptureSourceDir & CrLf &
                           "- Destination image: " & CaptureDestinationImage & CrLf &
                           "- Captured image name: " & CaptureName & CrLf)
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
            LogView.AppendText("- Captured image description: none specified" & CrLf)
        Else
            DynaLog.LogMessage("A description has been provided.")
            LogView.AppendText("- Captured image description: " & Quote & CaptureDescription & Quote & CrLf)
            CommandArgs &= " /description=" & Quote & CaptureDescription & Quote
        End If
        If CaptureWimScriptConfig = "" Then
            DynaLog.LogMessage("No configuration list file has been specified.")
            LogView.AppendText("- Configuration list file: not specified" & CrLf)
        Else
            DynaLog.LogMessage("A configuration list file has been specified. Checking if it exists...")
            LogView.AppendText("- Configuration list file: " & CaptureWimScriptConfig & CrLf)
            ' Possibly, the file may have been deleted after being specified. Determine whether it still exists
            If File.Exists(CaptureWimScriptConfig) Then
                CommandArgs &= " /configfile=" & Quote & CaptureWimScriptConfig & Quote
            Else
                DynaLog.LogMessage("The configuration list file does not exist in the file system and will be skipped.")
                LogView.AppendText("   WARNING: the configuration list file does not exist in the file system. Skipping file..." & CrLf)
            End If
        End If
        If CaptureCompressType = 0 Then
            LogView.AppendText("- Compression type: none" & CrLf)
            CommandArgs &= " /compress=none"
        ElseIf CaptureCompressType = 1 Then
            LogView.AppendText("- Compression type: fast" & CrLf)
            CommandArgs &= " /compress=fast"
        ElseIf CaptureCompressType = 2 Then
            LogView.AppendText("- Compression type: maximum" & CrLf)
            CommandArgs &= " /compress=max"
        End If
        If CaptureBootable Then
            LogView.AppendText("- Mark image as bootable? Yes" & CrLf)
            CommandArgs &= " /bootable"
        Else
            LogView.AppendText("- Mark image as bootable? No" & CrLf)
        End If
        If CaptureCheckIntegrity Then
            LogView.AppendText("- Check image integrity? Yes" & CrLf)
            CommandArgs &= " /checkintegrity"
        Else
            LogView.AppendText("- Check image integrity? No" & CrLf)
        End If
        If CaptureVerify Then
            LogView.AppendText("- Verify file errors? Yes" & CrLf)
            CommandArgs &= " /verify"
        Else
            LogView.AppendText("- Verify file errors? No" & CrLf)
        End If
        If CaptureReparsePt Then
            LogView.AppendText("- Use the Reparse Point tag fix? Yes" & CrLf)
        Else
            LogView.AppendText("- Use the Reparse Point tag fix? No" & CrLf)
            CommandArgs &= " /norpfix"
        End If
        If CaptureUseWimBoot Then
            LogView.AppendText("- Append with WIMBoot configuration? Yes" & CrLf)
            CommandArgs &= " /wimboot"
        Else
            LogView.AppendText("- Append with WIMBoot configuration? No" & CrLf)
        End If
        If CaptureExtendedAttributes Then
            LogView.AppendText("- Capture extended attributes? Yes" & CrLf)
            CommandArgs &= " /ea"
        Else
            LogView.AppendText("- Capture extended attributes? No" & CrLf)
        End If
        LogView.AppendText(CrLf & "Capturing image...")
        RunProcess(DismProgram, CommandArgs)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta livello errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        End If
    End Sub

    Private Sub CleanupMountpoints()
        DynaLog.LogMessage("Cleaning up mount points by deleting resources from old or corrupted images...")
        DynaLog.LogMessage("This does not require any additional options and invokes an API call. This will take some time depending on your system performance.")
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Cleaning up mount points..."
                        currentTask.Text = "Deleting resources from old or corrupted images..."
                    Case "ESN"
                        allTasks.Text = "Limpiando puntos de montaje..."
                        currentTask.Text = "Eliminando recursos de imágenes antiguas o corruptas..."
                    Case "FRA"
                        allTasks.Text = "Nettoyage des points de montage en cours..."
                        currentTask.Text = "Suppression des ressources des images anciennes ou corrompues en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Limpeza de pontos de montagem..."
                        currentTask.Text = "Eliminar recursos de imagens antigas ou corrompidas..."
                    Case "ITA"
                        allTasks.Text = "Pulizia punti montaggio..."
                        currentTask.Text = "Eliminazione risorse da immagini vecchie o corrotte..."
                End Select
            Case 1
                allTasks.Text = "Cleaning up mount points..."
                currentTask.Text = "Deleting resources from old or corrupted images..."
            Case 2
                allTasks.Text = "Limpiando puntos de montaje..."
                currentTask.Text = "Eliminando recursos de imágenes antiguas o corruptas..."
            Case 3
                allTasks.Text = "Nettoyage des points de montage en cours..."
                currentTask.Text = "Suppression des ressources des images anciennes ou corrompues en cours..."
            Case 4
                allTasks.Text = "Limpeza de pontos de montagem..."
                currentTask.Text = "Eliminar recursos de imagens antigas ou corrompidas..."
            Case 5
                allTasks.Text = "Pulizia punti montaggio..."
                currentTask.Text = "Eliminazione risorse da immagini vecchie o corrotte..."
        End Select
        LogView.AppendText(CrLf & "Cleaning up mount points..." & CrLf & CrLf &
                           "This can take some time, depending on the drives connected to this system.")
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta livello errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        If errCode Is Nothing Then
            errCode = 0
            IsSuccessful = True
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Committing image..."
                        currentTask.Text = "Saving changes to the image..."
                    Case "ESN"
                        allTasks.Text = "Guardando imagen..."
                        currentTask.Text = "Guardando cambios en la imagen..."
                    Case "FRA"
                        allTasks.Text = "Sauvegarde de l'image en cours..."
                        currentTask.Text = "Sauvegarde des modifications apportées à l'image en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "A confirmar a imagem..."
                        currentTask.Text = "Guardar alterações na imagem..."
                    Case "ITA"
                        allTasks.Text = "Modifica immagine..."
                        currentTask.Text = "Salvataggio modifiche nell'immagine..."
                End Select
            Case 1
                allTasks.Text = "Committing image..."
                currentTask.Text = "Saving changes to the image..."
            Case 2
                allTasks.Text = "Guardando imagen..."
                currentTask.Text = "Guardando cambios en la imagen..."
            Case 3
                allTasks.Text = "Sauvegarde de l'image en cours..."
                currentTask.Text = "Sauvegarde des modifications apportées à l'image en cours..."
            Case 4
                allTasks.Text = "A confirmar a imagem..."
                currentTask.Text = "Guardar alterações na imagem..."
            Case 5
                allTasks.Text = "Modifica immagine..."
                currentTask.Text = "Salvataggio modifiche nell'immagine..."
        End Select
        If ReferenceImage IsNot Nothing Then
            If Path.GetExtension(ReferenceImage.ImageFile).Equals(".ffu", StringComparison.OrdinalIgnoreCase) Then
                CommitFfu()
                Exit Sub
            End If
        End If
        LogView.AppendText(CrLf & "Saving changes..." & CrLf & "Options:" & CrLf &
                           "- Mount directory: " & MountDir)
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta livello errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
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
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                        Case "ESN"
                            taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                        Case "FRA"
                            taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                        Case "PTB", "PTG"
                            taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                        Case "ITA"
                            taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
                    End Select
                Case 1
                    taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                Case 2
                    taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                Case 3
                    taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                Case 4
                    taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                Case 5
                    taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
            End Select
        End If
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Deleting images..."
                        currentTask.Text = "Preparing to remove volume images..."
                    Case "ESN"
                        allTasks.Text = "Eliminando imágenes..."
                        currentTask.Text = "Preparando para eliminar imágenes de volumen..."
                    Case "FRA"
                        allTasks.Text = "Suppression des images en cours..."
                        currentTask.Text = "Préparation de la suppression des images de volume en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "A eliminar imagens..."
                        currentTask.Text = "A preparar a remoção de imagens de volume..."
                    Case "ITA"
                        allTasks.Text = "Eliminazione immagini..."
                        currentTask.Text = "Preparazione rimozione immagini volume..."
                End Select
            Case 1
                allTasks.Text = "Deleting images..."
                currentTask.Text = "Preparing to remove volume images..."
            Case 2
                allTasks.Text = "Eliminando imágenes..."
                currentTask.Text = "Preparando para eliminar imágenes de volumen..."
            Case 3
                allTasks.Text = "Suppression des images en cours..."
                currentTask.Text = "Préparation de la suppression des images de volume en cours..."
            Case 4
                allTasks.Text = "A eliminar imagens..."
                currentTask.Text = "A preparar a remoção de imagens de volume..."
            Case 5
                allTasks.Text = "Eliminazione immagini..."
                currentTask.Text = "Preparazione rimozione immagini volume..."
        End Select
        DynaLog.LogMessage("Source image to remove indexes from: " & Quote & imgIndexDeletionSourceImg & Quote)
        LogView.AppendText(CrLf & "Removing volume images from file..." & CrLf &
                           "Options:" & CrLf &
                           "- Source image: " & imgIndexDeletionSourceImg & CrLf)
        If imgIndexDeletionIntCheck Then
            LogView.AppendText("- Check image integrity? Yes")
        Else
            LogView.AppendText("- Check image integrity? No")
        End If
        CurrentPB.Maximum = imgIndexDeletionCount
        ' Removing volume images
        LogView.AppendText(CrLf &
                           "Removing volume images..." & CrLf)
        For x = 0 To Array.LastIndexOf(imgIndexDeletionNames, imgIndexDeletionLastName)
            If x + 1 > CurrentPB.Maximum Then Exit For
            DynaLog.LogMessage("Volume image to remove: " & Quote & imgIndexDeletionNames(x) & Quote)
            DynaLog.LogMessage("Processing task...")
            CurrentPB.Value = x + 1
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            currentTask.Text = "Removing volume image " & Quote & imgIndexDeletionNames(x) & Quote & "..."
                        Case "ESN"
                            currentTask.Text = "Eliminando imagen de volumen " & Quote & imgIndexDeletionNames(x) & Quote & "..."
                        Case "FRA"
                            currentTask.Text = "Suppression de l'image de volume " & Quote & imgIndexDeletionNames(x) & Quote & " en cours..."
                        Case "PTB", "PTG"
                            currentTask.Text = "Remover a imagem do volume " & Quote & imgIndexDeletionNames(x) & Quote & "..."
                        Case "ITA"
                            currentTask.Text = "Rimozione immagine volume " & Quote & imgIndexDeletionNames(x) & Quote & "..."
                    End Select
                Case 1
                    currentTask.Text = "Removing volume image " & Quote & imgIndexDeletionNames(x) & Quote & "..."
                Case 2
                    currentTask.Text = "Eliminando imagen de volumen " & Quote & imgIndexDeletionNames(x) & Quote & "..."
                Case 3
                    currentTask.Text = "Suppression de l'image de volume " & Quote & imgIndexDeletionNames(x) & Quote & " en cours..."
                Case 4
                    currentTask.Text = "Remover a imagem do volume " & Quote & imgIndexDeletionNames(x) & Quote & "..."
                Case 5
                    currentTask.Text = "Rimozione immagine volume " & Quote & imgIndexDeletionNames(x) & Quote & "..."
            End Select
            LogView.AppendText(CrLf &
                               "- " & imgIndexDeletionNames(x) & "...")
            CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /delete-image /imagefile=" & Quote & imgIndexDeletionSourceImg & Quote & " /name=" & Quote & imgIndexDeletionNames(x) & Quote
            If imgIndexDeletionIntCheck Then
                CommandArgs &= " /checkintegrity"
            End If
            RunProcess(DismProgram, CommandArgs)
            If Hex(DismExitCode).Length < 8 Then
                LogView.AppendText(" Error level : " & DismExitCode)
            Else
                LogView.AppendText(" Error level : 0x" & Hex(DismExitCode))
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Exporting image..."
                        currentTask.Text = "Exporting specified image..."
                    Case "ESN"
                        allTasks.Text = "Exportando imagen..."
                        currentTask.Text = "Exportando imagen especificada..."
                    Case "FRA"
                        allTasks.Text = "Exportation de l'image en cours..."
                        currentTask.Text = "Exportation de l'image spécifiée en cours..."
                    Case "PTB"
                        allTasks.Text = "Exportar imagem..."
                        currentTask.Text = "Exportar imagem especificada..."
                    Case "ITA"
                        allTasks.Text = "Esportazione immagine..."
                        currentTask.Text = "Esportazione immagine specificata..."
                End Select
            Case 1
                allTasks.Text = "Exporting image..."
                currentTask.Text = "Exporting specified image..."
            Case 2
                allTasks.Text = "Exportando imagen..."
                currentTask.Text = "Exportando imagen especificada..."
            Case 3
                allTasks.Text = "Exportation de l'image en cours..."
                currentTask.Text = "Exportation de l'image spécifiée en cours..."
            Case 4
                allTasks.Text = "Exportar imagem..."
                currentTask.Text = "Exportar imagem especificada..."
            Case 5
                allTasks.Text = "Esportazione immagine..."
                currentTask.Text = "Esportazione immagine specificata..."
        End Select
        LogView.AppendText(CrLf & "Exporting the specified image to a destination image..." & CrLf & "Options:" & CrLf &
                           "- Source image file: " & imgExportSourceImage & CrLf &
                           "- Source image index: " & imgExportSourceIndex & CrLf &
                           "- Destination image file: " & imgExportDestinationImage & CrLf &
                           If(imgExportDestinationUseCustomName, "- Destination image name: " & imgExportDestinationName, ""))
        Select Case imgExportCompressType
            Case 0
                LogView.AppendText(CrLf & "- Compression type: no compression")
            Case 1
                LogView.AppendText(CrLf & "- Compression type: fast compression")
            Case 2
                LogView.AppendText(CrLf & "- Compression type: maximum compression")
            Case 3
                LogView.AppendText(CrLf & "- Compression type: ESD conversion (recovery)")
        End Select
        LogView.AppendText(CrLf & "- Mark the image as bootable? " & If(imgExportMarkBootable, "Yes", "No") & CrLf &
                           "- Append image with WIMBoot configuration? " & If(imgExportUseWimBoot, "Yes", "No") & CrLf &
                           "- Check image integrity before exporting the image? " & If(imgExportCheckIntegrity, "Yes", "No"))
        ' Show information regarding SWM files
        DynaLog.LogMessage("Extension of source image file: " & Path.GetExtension(imgExportSourceImage))
        If Path.GetExtension(imgExportSourceImage).EndsWith("swm", StringComparison.OrdinalIgnoreCase) Then
            DynaLog.LogMessage("We are dealing with SWM files. Showing why we mark all of them for export...")
            LogView.AppendText(CrLf & CrLf & "NOTE: the source image contains an asterisk sign (*) in the file name to merge all SWM files")
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta livello errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
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
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            allTasks.Text = "Mounting image..."
                            currentTask.Text = "Mounting specified image..."
                        Case "ESN"
                            allTasks.Text = "Montando imagen..."
                            currentTask.Text = "Montando imagen especificada..."
                        Case "FRA"
                            allTasks.Text = "Montage de l'image en cours..."
                            currentTask.Text = "Montage de l'image spécifiée en cours..."
                        Case "PTB", "PTG"
                            allTasks.Text = "Montagem de imagem..."
                            currentTask.Text = "Montagem da imagem especificada..."
                        Case "ITA"
                            allTasks.Text = "Montaggio immagine..."
                            currentTask.Text = "Montaggio immagine specificata..."
                    End Select
                Case 1
                    allTasks.Text = "Mounting image..."
                    currentTask.Text = "Mounting specified image..."
                Case 2
                    allTasks.Text = "Montando imagen..."
                    currentTask.Text = "Montando imagen especificada..."
                Case 3
                    allTasks.Text = "Montage de l'image en cours..."
                    currentTask.Text = "Montage de l'image spécifiée en cours..."
                Case 4
                    allTasks.Text = "Montagem de imagem..."
                    currentTask.Text = "Montagem da imagem especificada..."
                Case 5
                    allTasks.Text = "Montaggio immagine..."
                    currentTask.Text = "Montaggio immagine specificata..."
            End Select
            LogView.AppendText(CrLf & "Mounting image..." & CrLf & "Options:" & CrLf &
                               "- Image file: " & SourceImg & CrLf &
                               "- Image index: " & ImgIndex & CrLf &
                               "- Mount point: " & MountDir)
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
                LogView.AppendText(CrLf & "- Mount image with read-only permissions? Yes")
                CommandArgs &= " /readonly"
            Else
                LogView.AppendText(CrLf & "- Mount image with read-only permissions? No")
            End If
            If isOptimized Then
                LogView.AppendText(CrLf & "- Optimize mount time? Yes")
                CommandArgs &= " /optimize"
            Else
                LogView.AppendText(CrLf & "- Optimize mount time? No")
            End If
            If isIntegrityTested Then
                LogView.AppendText(CrLf & "- Check image integrity? Yes")
                CommandArgs &= " /checkintegrity"
            Else
                LogView.AppendText(CrLf & "- Check image integrity? No")
            End If
            RunProcess(DismProgram, CommandArgs)
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            currentTask.Text = "Gathering error level..."
                        Case "ESN"
                            currentTask.Text = "Recopilando nivel de error..."
                        Case "FRA"
                            currentTask.Text = "Recueil du niveau d'erreur en cours..."
                        Case "PTB", "PTG"
                            currentTask.Text = "A recolher o nível de erro..."
                        Case "ITA"
                            currentTask.Text = "Raccolta livello errore..."
                    End Select
                Case 1
                    currentTask.Text = "Gathering error level..."
                Case 2
                    currentTask.Text = "Recopilando nivel de error..."
                Case 3
                    currentTask.Text = "Recueil du niveau d'erreur en cours..."
                Case 4
                    currentTask.Text = "A recolher o nível de erro..."
                Case 5
                    currentTask.Text = "Raccolta del livello di errore..."
            End Select
            LogView.AppendText(CrLf & "Gathering error level...")
        End If
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        End If
    End Sub

    Private Sub OptimizeFfuImage()
        DynaLog.LogMessage("Optimizing the Windows FFU image...")
        DynaLog.LogMessage("- Source image to optimize: " & Quote & FFUOptimizationSource & Quote)
        DynaLog.LogMessage("- Partition to optimize: " & FFUOptimizationCustomPartitionNum & If(FFUOptimizationCustomPartitionNum = 0, " (Default partition in the FFU will be optimized)", ""))
        allTasks.Text = "Optimizing image..."
        currentTask.Text = "Optimizing Windows image..."
        LogView.AppendText(CrLf & "Optimizing Windows image..." & CrLf &
                           "- Source image to optimize: " & Quote & FFUOptimizationSource & Quote & CrLf &
                           "- Partition to optimize: " & FFUOptimizationCustomPartitionNum & If(FFUOptimizationCustomPartitionNum = 0, " (Default partition in the FFU will be optimized)", "") & CrLf)
        ' Check the DISM version, as the Windows 7-8.1 versions don't allow this action
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                ' Not supported
            Case 10
                CommandArgs &= " /optimize-ffu /imagefile=" & Quote & FFUOptimizationSource & Quote
        End Select

        If FFUOptimizationCustomPartitionNum > 0 Then CommandArgs &= " /partitionnumber=" & FFUOptimizationCustomPartitionNum

        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & "Getting error level...")
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(" Error level : 0x" & errCode)
        Else
            LogView.AppendText(" Error level : " & errCode)
        End If
        GetErrorCode(False)
    End Sub

    Private Sub OptimizeImage()
        DynaLog.LogMessage("Optimizing the Windows image...")
        DynaLog.LogMessage("- Source image to optimize: " & Quote & OptimizationSource & Quote)
        DynaLog.LogMessage("- Optimization mode: " & OptimizationMode)
        allTasks.Text = "Optimizing image..."
        currentTask.Text = "Optimizing Windows image..."
        LogView.AppendText(CrLf & "Optimizing Windows image..." & CrLf &
                           "- Source image to optimize: " & Quote & OptimizationSource & Quote & CrLf &
                           "- Optimization mode: " & If(OptimizationMode = 0, "Reduce online configuration time", "Prepare image for WIMBoot system") & CrLf)
        ' Check the DISM version, as the Windows 7-8.1 versions don't allow this action
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                ' Not supported
            Case 10
                CommandArgs &= " /image=" & Quote & OptimizationSource & Quote & " /optimize-image " & If(OptimizationMode = 0, "/boot", "/wimboot")
        End Select
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & "Getting error level...")
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(" Error level : 0x" & errCode)
        Else
            LogView.AppendText(" Error level : " & errCode)
        End If
        GetErrorCode(False)
    End Sub

    Private Sub RemountImage()
        DynaLog.LogMessage("Reloading the servicing session of the mounted image...")
        DynaLog.LogMessage("- Mount location of the image file we are interested in reloading: " & Quote & MountDir & Quote)
        DynaLog.LogMessage("This invokes an API call. This process will take some time depending on your system performance and how big the Windows image is.")
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Remounting image..."
                        currentTask.Text = "Reloading servicing session for mounted image..."
                    Case "ESN"
                        allTasks.Text = "Remontando imagen..."
                        currentTask.Text = "Recargando sesión de servicio para la imagen montada..."
                    Case "FRA"
                        allTasks.Text = "Remontage de l'image en cours..."
                        currentTask.Text = "Rechargement de la session de maintenance pour l'image montée en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Remontando imagem..."
                        currentTask.Text = "Recarregar sessão de manutenção para a imagem montada..."
                    Case "ITA"
                        allTasks.Text = "Rimontaggio immagine..."
                        currentTask.Text = "Ricaricamento sessione assistenza per l'immagine montata..."
                End Select
            Case 1
                allTasks.Text = "Remounting image..."
                currentTask.Text = "Reloading servicing session for mounted image..."
            Case 2
                allTasks.Text = "Remontando imagen..."
                currentTask.Text = "Recargando sesión de servicio para la imagen montada..."
            Case 3
                allTasks.Text = "Remontage de l'image en cours..."
                currentTask.Text = "Rechargement de la session de maintenance pour l'image montée en cours..."
            Case 4
                allTasks.Text = "Remontando imagem..."
                currentTask.Text = "Recarregar sessão de manutenção para a imagem montada..."
            Case 5
                allTasks.Text = "Rimontaggio immagine..."
                currentTask.Text = "Ricaricamento sessione assistenza per l'immagine montata..."
        End Select
        LogView.AppendText(CrLf & "Reloading servicing session..." & CrLf &
                           "- Mount directory: " & MountDir)
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta livello errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        If errCode Is Nothing Then
            errCode = 0
            IsSuccessful = True
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        End If
    End Sub

    Private Sub SplitFfuImage()
        DynaLog.LogMessage("Splitting the Windows FFU image...")
        DynaLog.LogMessage("- Source image file to split: " & Quote & SFUSplitSourceFile & Quote)
        DynaLog.LogMessage("- Maximum size of split images: " & SFUSplitFileSize & " MB")
        DynaLog.LogMessage("- Destination of SFU files: " & Quote & SFUSplitTargetFile & Quote)
        DynaLog.LogMessage("- Check image integrity? " & If(SFUSplitCheckIntegrity, "Yes", "No"))
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Splitting image..."
                        currentTask.Text = "Splitting FFU file..."
                    Case "ESN"
                        allTasks.Text = "Dividiendo imagen..."
                        currentTask.Text = "Dividiendo archivo FFU..."
                    Case "FRA"
                        allTasks.Text = "Division de l'image en cours..."
                        currentTask.Text = "Division du fichier FFU en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Dividir imagem..."
                        currentTask.Text = "Dividir ficheiro FFU..."
                    Case "ITA"
                        allTasks.Text = "Divisione immagine..."
                        currentTask.Text = "Divisione file FFU..."
                End Select
            Case 1
                allTasks.Text = "Splitting image..."
                currentTask.Text = "Splitting FFU file..."
            Case 2
                allTasks.Text = "Dividiendo imagen..."
                currentTask.Text = "Dividiendo archivo FFU..."
            Case 3
                allTasks.Text = "Division de l'image en cours..."
                currentTask.Text = "Division du fichier FFU en cours..."
            Case 4
                allTasks.Text = "Dividir imagem..."
                currentTask.Text = "Dividir ficheiro FFU..."
            Case 5
                allTasks.Text = "Divisione immagine..."
                currentTask.Text = "Divisione file FFU..."
        End Select
        LogView.AppendText(CrLf & "Splitting FFU file into SFU files..." & CrLf &
                           "- Source image file to split: " & Quote & SFUSplitSourceFile & Quote & CrLf &
                           "- Maximum size of the split images (in MB): " & SFUSplitFileSize & " MB" & CrLf &
                           "- Name and path of the target SFU file: " & Quote & SFUSplitTargetFile & Quote & CrLf &
                           "- Check integrity before splitting this image? " & If(SFUSplitCheckIntegrity, "Yes", "No") & CrLf & CrLf &
                           "Do note that, if the image contains a large file that can't fit within the maximum size, a SFU file may be larger than the rest, to accommodate it." & CrLf)
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
        LogView.AppendText(CrLf & "Getting error level...")
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(" Error level : 0x" & errCode)
        Else
            LogView.AppendText(" Error level : " & errCode)
        End If
        GetErrorCode(False)
    End Sub

    Private Sub SplitImage()
        DynaLog.LogMessage("Splitting the Windows image...")
        DynaLog.LogMessage("- Source image file to split: " & Quote & SWMSplitSourceFile & Quote)
        DynaLog.LogMessage("- Maximum size of split images: " & SWMSplitFileSize & " MB")
        DynaLog.LogMessage("- Destination of SWM files: " & Quote & SWMSplitTargetFile & Quote)
        DynaLog.LogMessage("- Check image integrity? " & If(SWMSplitCheckIntegrity, "Yes", "No"))
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Splitting image..."
                        currentTask.Text = "Splitting WIM file..."
                    Case "ESN"
                        allTasks.Text = "Dividiendo imagen..."
                        currentTask.Text = "Dividiendo archivo WIM..."
                    Case "FRA"
                        allTasks.Text = "Division de l'image en cours..."
                        currentTask.Text = "Division du fichier WIM en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Dividir imagem..."
                        currentTask.Text = "Dividir ficheiro WIM..."
                    Case "ITA"
                        allTasks.Text = "Divisione immagine..."
                        currentTask.Text = "Divisione file WIM..."
                End Select
            Case 1
                allTasks.Text = "Splitting image..."
                currentTask.Text = "Splitting WIM file..."
            Case 2
                allTasks.Text = "Dividiendo imagen..."
                currentTask.Text = "Dividiendo archivo WIM..."
            Case 3
                allTasks.Text = "Division de l'image en cours..."
                currentTask.Text = "Division du fichier WIM en cours..."
            Case 4
                allTasks.Text = "Dividir imagem..."
                currentTask.Text = "Dividir ficheiro WIM..."
            Case 5
                allTasks.Text = "Divisione immagine..."
                currentTask.Text = "Divisione file WIM..."
        End Select
        LogView.AppendText(CrLf & "Splitting WIM file into SWM files..." & CrLf &
                           "- Source image file to split: " & Quote & SWMSplitSourceFile & Quote & CrLf &
                           "- Maximum size of the split images (in MB): " & SWMSplitFileSize & " MB" & CrLf &
                           "- Name and path of the target SWM file: " & Quote & SWMSplitTargetFile & Quote & CrLf &
                           "- Check integrity before splitting this image? " & If(SWMSplitCheckIntegrity, "Yes", "No") & CrLf & CrLf &
                           "Do note that, if the image contains a large file that can't fit within the maximum size, a SWM file may be larger than the rest, to accommodate it." & CrLf)
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
        LogView.AppendText(CrLf & "Getting error level...")
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(" Error level : 0x" & errCode)
        Else
            LogView.AppendText(" Error level : " & errCode)
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Unmounting image..."
                        currentTask.Text = "Unmounting image file..."
                    Case "ESN"
                        allTasks.Text = "Desmontando imagen..."
                        currentTask.Text = "Desmontando archivo de imagen..."
                    Case "FRA"
                        allTasks.Text = "Démontage de l'image en cours..."
                        currentTask.Text = "Démontage du fichier d'image en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Desmontar imagem..."
                        currentTask.Text = "Desmontar ficheiro de imagem..."
                    Case "ITA"
                        allTasks.Text = "Smontaggio immagine..."
                        currentTask.Text = "Smontaggio file immagine..."
                End Select
            Case 1
                allTasks.Text = "Unmounting image..."
                currentTask.Text = "Unmounting image file..."
            Case 2
                allTasks.Text = "Desmontando imagen..."
                currentTask.Text = "Desmontando archivo de imagen..."
            Case 3
                allTasks.Text = "Démontage de l'image en cours..."
                currentTask.Text = "Démontage du fichier d'image en cours..."
            Case 4
                allTasks.Text = "Desmontar imagem..."
                currentTask.Text = "Desmontar ficheiro de imagem..."
            Case 5
                allTasks.Text = "Smontaggio immagine..."
                currentTask.Text = "Smontaggio file immagine..."
        End Select
        If Not UMountLocalDir Then
            DynaLog.LogMessage("The image that was mounted in the project mount directory will not be unmounted. Using mountdir " & Quote & RandomMountDir & Quote & "...")
            MountDir = RandomMountDir
        End If
        LogView.AppendText(CrLf & "Unmounting image file from mount point..." & CrLf &
                           "- Mount directory: " & MountDir & CrLf &
                           "- Image index: " & UMountImgIndex)
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
                LogView.AppendText(CrLf & "- Unmount operation: Commit")
                CommandArgs &= " /commit"
            ElseIf UMountOp = 1 Then
                LogView.AppendText(CrLf & "- Unmount operation: Discard")
                CommandArgs &= " /discard"
            End If
            If UMountOp = 0 Then
                If CheckImgIntegrity Then
                    LogView.AppendText(CrLf & "- Check image integrity? Yes")
                    CommandArgs &= " /checkintegrity"
                Else
                    LogView.AppendText(CrLf & "- Check image integrity? No")
                End If
                If SaveToNewIndex Then
                    LogView.AppendText(CrLf & "- Append changes to new index? Yes")
                    CommandArgs &= " /append"
                Else
                    LogView.AppendText(CrLf & "- Append changes to new index? No")
                End If
            End If
            RunProcess(DismProgram, CommandArgs)
        Catch ex As Exception
            ' Let's try this before setting things up here
        End Try
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta livello errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        End If
    End Sub


#End Region

#Region "Package/Feature Management Tasks"

    Private Sub ShowPackageInformation(pkgInfo As DismPackageInfo)
        LogView.AppendText(CrLf & CrLf &
                           "- Package name: " & pkgInfo.PackageName & CrLf &
                           "- Package description: " & pkgInfo.Description & CrLf &
                           "- Package release type: " & Casters.CastDismReleaseType(pkgInfo.ReleaseType) & CrLf &
                           "- Package is applicable to this image? " & If(pkgInfo.Applicable, "Yes", "No") & CrLf &
                           "- Package is already installed? " & If(pkgInfo.PackageState = DismPackageFeatureState.Installed Or pkgInfo.PackageState = DismPackageFeatureState.InstallPending, "Yes", "No") & CrLf)
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
                LogView.AppendText(CrLf & "Total number of packages: " & pkgCount)
            Catch ex As Exception
                DynaLog.LogMessage("Could not get packages in all subdirectories. Error message: " & ex.Message)
                LogView.AppendText(CrLf & "Exception " & ex.GetType().ToString() & " has occurred while enumerating packages. Enumerating packages in the top folder...")
                DynaLog.LogMessage("Getting CAB files...")
                For Each CabPkg In My.Computer.FileSystem.GetFiles(pkgSource, FileIO.SearchOption.SearchTopLevelOnly, "*.cab")
                    pkgCount += 1
                Next
                DynaLog.LogMessage("Getting MSU files...")
                For Each MsuPkg In My.Computer.FileSystem.GetFiles(pkgSource, FileIO.SearchOption.SearchTopLevelOnly, "*.msu")
                    pkgCount += 1
                Next
                DynaLog.LogMessage("Package count: " & pkgCount)
                LogView.AppendText(CrLf & "Total number of packages: " & pkgCount)
            End Try
        ElseIf pkgAdditionOp = 1 Then
            DynaLog.LogMessage("Addition operation is selective addition. A package count has already been obtained from the queue.")
            LogView.AppendText(CrLf & "Total number of packages: " & pkgCount)
        ElseIf pkgAdditionOp = 2 Then
            DynaLog.LogMessage("Addition operation is Update Manifest addition. Only 1 package will be added.")
            LogView.AppendText(CrLf & "Total number of packages: 1")
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta livello errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Adding packages..."
                        currentTask.Text = "Preparing to add packages..."
                    Case "ESN"
                        allTasks.Text = "Añadiendo paquetes..."
                        currentTask.Text = "Preparándonos para añadir paquetes..."
                    Case "FRA"
                        allTasks.Text = "Ajout des paquets en cours..."
                        currentTask.Text = "Préparation de l'ajout des paquets en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "A adicionar pacotes..."
                        currentTask.Text = "A preparar a adição de pacotes..."
                    Case "ITA"
                        allTasks.Text = "Aggiunta pacchetti..."
                        currentTask.Text = "Preparazione aggiunta pacchetti..."
                End Select
            Case 1
                allTasks.Text = "Adding packages..."
                currentTask.Text = "Preparing to add packages..."
            Case 2
                allTasks.Text = "Añadiendo paquetes..."
                currentTask.Text = "Preparándonos para añadir paquetes..."
            Case 3
                allTasks.Text = "Ajout des paquets en cours..."
                currentTask.Text = "Préparation de l'ajout des paquets en cours..."
            Case 4
                allTasks.Text = "A adicionar pacotes..."
                currentTask.Text = "A preparar a adição de pacotes..."
            Case 5
                allTasks.Text = "Aggiunta pacchetti..."
                currentTask.Text = "Preparazione aggiunta pacchetti..."
        End Select
        LogView.AppendText(CrLf & "Adding packages to mounted image..." & CrLf &
                           "- Package source: " & pkgSource & CrLf)
        If pkgAdditionOp = 0 Then
            LogView.AppendText("- Addition operation: recursive")
        ElseIf pkgAdditionOp = 1 Then
            LogView.AppendText("- Addition operation: selective")
        End If
        If pkgIgnoreApplicabilityChecks Then
            LogView.AppendText(CrLf & "- Ignore applicability checks? Yes")
        Else
            LogView.AppendText(CrLf & "- Ignore applicability checks? No")
        End If
        If pkgPreventIfPendingOnline Then
            LogView.AppendText(CrLf & "- Prevent package addition if online actions need to be performed? Yes" & CrLf &
                               "NOTE: if the mounted image requires that online actions be performed, all packages might fail installation; but the operation might still be successful")
        Else
            LogView.AppendText(CrLf & "- Prevent package addition if online actions need to be performed? No")
        End If
        If pkgAdditionCommit Then
            LogView.AppendText(CrLf & "- Commit image after operations are done? Yes")
        Else
            LogView.AppendText(CrLf & "- Commit image after operations are done? No")
        End If

        ' Perform package enumeration
        LogView.AppendText(CrLf & "Enumerating packages to add. Please wait...")
        CountPackagesToAdd()
        Thread.Sleep(2000)      ' Sleep to prevent thrashing

        ' Begin package addition
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Adding " & pkgCount & " packages..."
                    Case "ESN"
                        currentTask.Text = "Añadiendo " & pkgCount & " paquetes..."
                    Case "FRA"
                        currentTask.Text = "Ajout de " & pkgCount & " paquets en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "Adicionando " & pkgCount & " pacotes..."
                    Case "ITA"
                        currentTask.Text = "Aggiunta di " & pkgCount & " pacchetti..."
                End Select
            Case 1
                currentTask.Text = "Adding " & pkgCount & " packages..."
            Case 2
                currentTask.Text = "Añadiendo " & pkgCount & " paquetes..."
            Case 3
                currentTask.Text = "Ajout de " & pkgCount & " paquets en cours..."
            Case 4
                currentTask.Text = "Adicionando " & pkgCount & " pacotes..."
            Case 5
                currentTask.Text = "Aggiunta di " & pkgCount & " pacchetti..."
        End Select
        CurrentPB.Style = ProgressBarStyle.Blocks
        LogView.AppendText(CrLf & CrLf &
                           "Processing " & pkgCount & " packages..." & CrLf)
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
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                        Case "ESN"
                            taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                        Case "FRA"
                            taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                        Case "PTB", "PTG"
                            taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                        Case "ITA"
                            taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
                    End Select
                Case 1
                    taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                Case 2
                    taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                Case 3
                    taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                Case 4
                    taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                Case 5
                    taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
            End Select
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
            LogView.AppendText(CrLf & "Some packages require a system restart to be fully processed. Save your work, close your programs, and restart when ready")
        End If
    End Sub

    Private Sub AddPackagesSelectively(targetImage As String)
        CurrentPB.Maximum = pkgCount
        For x = 0 To Array.LastIndexOf(pkgs, pkgLastCheckedPackageName)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            currentTask.Text = "Adding package " & (x + 1) & " of " & pkgCount & "..."
                        Case "ESN"
                            currentTask.Text = "Añadiendo paquete " & (x + 1) & " de " & pkgCount & "..."
                        Case "FRA"
                            currentTask.Text = "Ajout du paquet " & (x + 1) & " de " & pkgCount & " en cours..."
                        Case "PTB", "PTG"
                            currentTask.Text = "A adicionar o pacote " & (x + 1) & " de " & pkgCount & "..."
                        Case "ITA"
                            currentTask.Text = "Aggiunta del pacchetto " & (x + 1) & " di " & pkgCount & "..."
                    End Select
                Case 1
                    currentTask.Text = "Adding package " & (x + 1) & " of " & pkgCount & "..."
                Case 2
                    currentTask.Text = "Añadiendo paquete " & (x + 1) & " de " & pkgCount & "..."
                Case 3
                    currentTask.Text = "Ajout du paquet " & (x + 1) & " de " & pkgCount & " en cours..."
                Case 4
                    currentTask.Text = "A adicionar o pacote " & (x + 1) & " de " & pkgCount & "..."
                Case 5
                    currentTask.Text = "Aggiunta del pacchetto " & (x + 1) & " di " & pkgCount & "..."
            End Select
            CurrentPB.Value = x + 1
            LogView.AppendText(CrLf &
                               "Package " & (x + 1) & " of " & pkgCount)        ' You don't want to see "Package 0 of 407", right?

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
                                LogView.AppendText(CrLf & "Package is already added. Skipping installation of this package...")
                                pkgFailedAdditions += 1
                            End If
                        Else
                            DynaLog.LogMessage("The package cannot be added to the Windows image as it is not applicable.")
                            If Not pkgIgnoreApplicabilityChecks Then
                                DynaLog.LogMessage("Applicability checks are not ignored.")
                                LogView.AppendText(CrLf & "Package is not applicable to this image. Skipping installation of this package...")
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
                    LogView.AppendText(CrLf & "The package about to be added is a MSU file. Continuing...")
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
            LogView.AppendText(CrLf & "Processing package...")
            CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /add-package /packagepath=" & Quote & pkgs(x) & Quote
            If pkgIgnoreApplicabilityChecks Then
                CommandArgs &= " /ignorecheck"
            End If
            If pkgPreventIfPendingOnline Then
                CommandArgs &= " /preventpending"
            End If
            RunProcess(DismProgram, CommandArgs)
            LogView.AppendText(CrLf & "Getting error level...")
            GetPkgErrorLevel()
            LogView.AppendText(" Error level: " & errCode)
            If PackageErrorCodes.Count <= 0 Then
                PackageErrorCodes.Add(errCode)
            Else
                PackageErrorCodes.Add(errCode)
            End If
        Next
        CurrentPB.Value = CurrentPB.Maximum
        LogView.AppendText(CrLf & "Gathering error level for selected packages..." & CrLf)
        For x = 0 To PackageErrorCodes.Count - 1
            LogView.AppendText(CrLf & "- Package no. " & (x + 1) & ": " & PackageErrorCodes(x))
        Next
    End Sub

    Private Sub AddUpdateManifest(targetImage As String)
        DynaLog.LogMessage("Addition operation is Update Manifest addition.")
        CurrentPB.Maximum = pkgCount
        CommandArgs = BckArgs
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Adding package 1 of " & pkgCount & "..."
                    Case "ESN"
                        currentTask.Text = "Añadiendo paquete 1 de " & pkgCount & "..."
                    Case "FRA"
                        currentTask.Text = "Ajout du paquet 1 de " & pkgCount & " en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A adicionar o pacote 1 de " & pkgCount & "..."
                    Case "ITA"
                        currentTask.Text = "Aggiunta del pacchetto 1 di " & pkgCount & "..."
                End Select
            Case 1
                currentTask.Text = "Adding package 1 of " & pkgCount & "..."
            Case 2
                currentTask.Text = "Añadiendo paquete 1 de " & pkgCount & "..."
            Case 3
                currentTask.Text = "Ajout du paquet 1 de " & pkgCount & " en cours..."
            Case 4
                currentTask.Text = "A adicionar o pacote 1 de " & pkgCount & "..."
            Case 5
                currentTask.Text = "Aggiunta del pacchetto 1 di " & pkgCount & "..."
        End Select
        CurrentPB.Value = 1
        LogView.AppendText(CrLf & "The package about to be added is a Microsoft Update Manifest (MUM) file.")
        LogView.AppendText(CrLf & "Processing package...")
        CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /add-package /packagepath=" & Quote & pkgs(0) & Quote
        If pkgIgnoreApplicabilityChecks Then
            CommandArgs &= " /ignorecheck"
        End If
        If pkgPreventIfPendingOnline Then
            CommandArgs &= " /preventpending"
        End If
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & "Getting error level...")
        GetPkgErrorLevel()
        LogView.AppendText(" Error level: " & errCode)
        If PackageErrorCodes.Count <= 0 Then
            PackageErrorCodes.Add(errCode)
        Else
            PackageErrorCodes.Add(errCode)
        End If
        CurrentPB.Value = CurrentPB.Maximum
        LogView.AppendText(CrLf & "Gathering error level for selected packages..." & CrLf)
        For x = 0 To PackageErrorCodes.Count - 1
            LogView.AppendText(CrLf & "- Package no. " & (x + 1) & ": " & PackageErrorCodes(x))
        Next
    End Sub

    Private Sub RemovePackages(targetImage As String)
        DynaLog.LogMessage("Preparing to remove packages...")
        DynaLog.LogMessage("- Package removal operation: " & pkgRemovalOp)
        DynaLog.LogMessage("- Amount of packages to remove: " & pkgRemovalCount)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Removing packages..."
                        currentTask.Text = "Preparing to remove packages..."
                    Case "ESN"
                        allTasks.Text = "Eliminando paquetes..."
                        currentTask.Text = "Preparándonos para eliminar paquetes..."
                    Case "FRA"
                        allTasks.Text = "Suppression des paquets en cours..."
                        currentTask.Text = "Préparation de la suppression des paquets en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "A remover pacotes..."
                        currentTask.Text = "A preparar a remoção de pacotes..."
                    Case "ITA"
                        allTasks.Text = "Rimozione pacchetti..."
                        currentTask.Text = "Preparazione rimozione pacchetti..."
                End Select
            Case 1
                allTasks.Text = "Removing packages..."
                currentTask.Text = "Preparing to remove packages..."
            Case 2
                allTasks.Text = "Eliminando paquetes..."
                currentTask.Text = "Preparándonos para eliminar paquetes..."
            Case 3
                allTasks.Text = "Suppression des paquets en cours..."
                currentTask.Text = "Préparation de la suppression des paquets en cours..."
            Case 4
                allTasks.Text = "A remover pacotes..."
                currentTask.Text = "A preparar a remoção de pacotes..."
            Case 5
                allTasks.Text = "Rimozione pacchetti..."
                currentTask.Text = "Preparazione rimozione pacchetti..."
        End Select
        LogView.AppendText(CrLf & "Removing packages from mounted image..." & CrLf &
                           "Enumerating packages to remove. Please wait...")
        Thread.Sleep(1000)
        LogView.AppendText(CrLf & "Amount of packages to remove: " & pkgRemovalCount)

        ' Begin package removal
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Removing packages..."
                    Case "ESN"
                        currentTask.Text = "Eliminando paquetes..."
                    Case "FRA"
                        currentTask.Text = "Suppression des paquets en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A remover pacotes..."
                    Case "ITA"
                        currentTask.Text = "Rimozione pacchetti..."
                End Select
            Case 1
                currentTask.Text = "Removing packages..."
            Case 2
                currentTask.Text = "Eliminando paquetes..."
            Case 3
                currentTask.Text = "Suppression des paquets en cours..."
            Case 4
                currentTask.Text = "A remover pacotes..."
            Case 5
                currentTask.Text = "Rimozione pacchetti..."
        End Select
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
        LogView.AppendText(CrLf & "Gathering error level for selected packages..." & CrLf)
        For x = 0 To PackageErrorCodes.Count - 1
            LogView.AppendText(CrLf & "- Package no. " & (x + 1) & ": " & PackageErrorCodes(x))
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
            LogView.AppendText(CrLf & "Some packages require a system restart to be fully processed. Save your work, close your programs, and restart when ready")
        End If
    End Sub

    Private Sub RemovePackageFiles(targetImage As String)
        For x = 0 To Array.LastIndexOf(pkgRemovalFiles, pkgRemovalLastFile)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            currentTask.Text = "Removing package " & (x + 1) & " of " & pkgRemovalCount & "..."
                        Case "ESN"
                            currentTask.Text = "Eliminando paquete " & (x + 1) & " de " & pkgRemovalCount & "..."
                        Case "FRA"
                            currentTask.Text = "Suppression du paquet " & (x + 1) & " de " & pkgRemovalCount & " en cours..."
                        Case "PTB", "PTG"
                            currentTask.Text = "A remover o pacote " & (x + 1) & " de " & pkgRemovalCount & "..."
                        Case "ITA"
                            currentTask.Text = "Rimozione del pacchetto " & (x + 1) & " di " & pkgRemovalCount & "..."
                    End Select
                Case 1
                    currentTask.Text = "Removing package " & (x + 1) & " of " & pkgRemovalCount & "..."
                Case 2
                    currentTask.Text = "Eliminando paquete " & (x + 1) & " de " & pkgRemovalCount & "..."
                Case 3
                    currentTask.Text = "Suppression du paquet " & (x + 1) & " de " & pkgRemovalCount & " en cours..."
                Case 4
                    currentTask.Text = "A remover o pacote " & (x + 1) & " de " & pkgRemovalCount & "..."
                Case 5
                    currentTask.Text = "Rimozione del pacchetto " & (x + 1) & " di " & pkgRemovalCount & "..."
            End Select
            LogView.AppendText(CrLf &
                               "Package " & (x + 1) & " of " & pkgRemovalCount)
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
                                       "- Package name: " & pkgInfo.PackageName & CrLf)
                    If pkgInfo.PackageState = DismPackageFeatureState.Installed Then
                        LogView.AppendText("- Package state: installed" & CrLf)
                    ElseIf pkgInfo.PackageState = DismPackageFeatureState.UninstallPending Then
                        LogView.AppendText("- Package state: an uninstall is pending" & CrLf)
                    ElseIf pkgInfo.PackageState = DismPackageFeatureState.InstallPending Then
                        LogView.AppendText("- Package state: an install is pending" & CrLf)
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
                LogView.AppendText(CrLf & "Processing package removal...")
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /remove-package /packagepath=" & pkgRemovalFiles(x)
                RunProcess(DismProgram, CommandArgs)
                LogView.AppendText(CrLf & "Getting error level...")
                errCode = Hex(Decimal.ToInt32(DismExitCode))
                If DismExitCode = 0 Then
                    pkgSuccessfulRemovals += 1
                Else
                    pkgFailedRemovals += 1
                End If
                If errCode.Length >= 8 Then
                    LogView.AppendText(CrLf & CrLf & " Error level : 0x" & errCode)
                Else
                    LogView.AppendText(CrLf & CrLf & " Error level : " & errCode)
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
                LogView.AppendText(CrLf & "This package can't be removed. Skipping removal of this package...")
                pkgFailedRemovals += 1
                Continue For
            End If
        Next
    End Sub

    Private Sub RemoveInstalledPackages(targetImage As String)
        For x = 0 To Array.LastIndexOf(pkgRemovalNames, pkgRemovalLastName)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            currentTask.Text = "Removing package " & (x + 1) & " of " & pkgRemovalCount & "..."
                        Case "ESN"
                            currentTask.Text = "Eliminando paquete " & (x + 1) & " de " & pkgRemovalCount & "..."
                        Case "FRA"
                            currentTask.Text = "Suppression du paquet " & (x + 1) & " de " & pkgRemovalCount & " en cours..."
                        Case "PTB", "PTG"
                            currentTask.Text = "A remover o pacote " & (x + 1) & " de " & pkgRemovalCount & "..."
                        Case "ITA"
                            currentTask.Text = "Rimozione del pacchetto " & (x + 1) & " di " & pkgRemovalCount & "..."
                    End Select
                Case 1
                    currentTask.Text = "Removing package " & (x + 1) & " of " & pkgRemovalCount & "..."
                Case 2
                    currentTask.Text = "Eliminando paquete " & (x + 1) & " de " & pkgRemovalCount & "..."
                Case 3
                    currentTask.Text = "Suppression du paquet " & (x + 1) & " de " & pkgRemovalCount & " en cours..."
                Case 4
                    currentTask.Text = "A remover o pacote " & (x + 1) & " de " & pkgRemovalCount & "..."
                Case 5
                    currentTask.Text = "Rimozione del pacchetto " & (x + 1) & " di " & pkgRemovalCount & "..."
            End Select
            LogView.AppendText(CrLf &
                               "Package " & (x + 1) & " of " & pkgRemovalCount)
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
                                       "- Package name: " & pkgInfo.PackageName & CrLf &
                                       "- Package state: " & Casters.CastDismPackageState(pkgInfo.PackageState))
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
                LogView.AppendText(CrLf & "Processing package removal...")
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /remove-package /packagename=" & pkgRemovalNames(x)
                RunProcess(DismProgram, CommandArgs)
                LogView.AppendText(CrLf & "Getting error level...")
                errCode = Hex(Decimal.ToInt32(DismExitCode))
                If DismExitCode = 0 Then
                    pkgSuccessfulRemovals += 1
                Else
                    pkgFailedRemovals += 1
                End If
                If errCode.Length >= 8 Then
                    LogView.AppendText(CrLf & CrLf & " Error level : 0x" & errCode)
                Else
                    LogView.AppendText(CrLf & CrLf & " Error level : " & errCode)
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
                LogView.AppendText(CrLf & "This package can't be removed. Skipping removal of this package...")
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Enabling features..."
                        currentTask.Text = "Preparing to enable features..."
                    Case "ESN"
                        allTasks.Text = "Habilitando características..."
                        currentTask.Text = "Preparándonos para habilitar características..."
                    Case "FRA"
                        allTasks.Text = "Activation des caractéristiques en cours..."
                        currentTask.Text = "Préparation de l'activation des caractéristiques en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Ativar características..."
                        currentTask.Text = "A preparar a ativação de características..."
                    Case "ITA"
                        allTasks.Text = "Abilitazione funzionalità..."
                        currentTask.Text = "Preparazione abilitazione funzionalità..."
                End Select
            Case 1
                allTasks.Text = "Enabling features..."
                currentTask.Text = "Preparing to enable features..."
            Case 2
                allTasks.Text = "Habilitando características..."
                currentTask.Text = "Preparándonos para habilitar características..."
            Case 3
                allTasks.Text = "Activation des caractéristiques en cours..."
                currentTask.Text = "Préparation de l'activation des caractéristiques en cours..."
            Case 4
                allTasks.Text = "Ativar características..."
                currentTask.Text = "A preparar a ativação de características..."
            Case 5
                allTasks.Text = "Abilitazione funzionalità..."
                currentTask.Text = "Preparazione abilitazione funzionalità..."
        End Select
        LogView.AppendText(CrLf & "Enabling features..." & CrLf &
                           "Options:" & CrLf)
        If featisParentPkgNameUsed Then
            LogView.AppendText("- Use parent package to enable features? Yes")
        Else
            LogView.AppendText("- Use parent package to enable features? No")
        End If
        If featParentPkgName = "" Then
            LogView.AppendText(CrLf & "- Parent package name: not specified")
        Else
            LogView.AppendText(CrLf & "- Parent package name: " & Quote & featParentPkgName & Quote)
        End If
        If featisSourceSpecified Then
            LogView.AppendText(CrLf & "- Use feature source? Yes")
        Else
            LogView.AppendText(CrLf & "- Use feature source? No")
        End If
        If featSource = "" Then
            LogView.AppendText(CrLf & "- Feature source: not specified")
        Else
            LogView.AppendText(CrLf & "- Feature source: " & Quote & featSource & Quote)
        End If
        If featParentIsEnabled Then
            LogView.AppendText(CrLf & "- Enable all parent features? Yes")
        Else
            LogView.AppendText(CrLf & "- Enable all parent features? No")
        End If
        DynaLog.LogMessage("Boot mode of the host system: " & SystemInformation.BootMode)
        If featContactWindowsUpdate And OnlineMgmt And SystemInformation.BootMode <> BootMode.FailSafe Then
            DynaLog.LogMessage("Host system is booted to normal mode or Safe Mode with networking.")
            LogView.AppendText(CrLf & "- Contact Windows Update? Yes")
        ElseIf featContactWindowsUpdate And OnlineMgmt And SystemInformation.BootMode = BootMode.FailSafe Then
            DynaLog.LogMessage("Host system is booted to Safe Mode.")
            LogView.AppendText(CrLf & "- Contact Windows Update? No, the system is in Safe Mode")
        ElseIf featContactWindowsUpdate And Not OnlineMgmt Then
            DynaLog.LogMessage("The active installation is not being managed.")
            LogView.AppendText(CrLf & "- Contact Windows Update? No, this is not an online installation")
        Else
            LogView.AppendText(CrLf & "- Contact Windows Update? No")
        End If
        If featEnablementCommit Then
            LogView.AppendText(CrLf & "- Commit image after enabling features? Yes")
        Else
            LogView.AppendText(CrLf & "- Commit image after enabling features? No")
        End If
        LogView.AppendText(CrLf & CrLf & "Enumerating features to enable...")
        Thread.Sleep(500)
        LogView.AppendText(CrLf & "Total number of features to enable: " & featEnablementCount)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Enabling features..."
                    Case "ESN"
                        currentTask.Text = "Habilitando características..."
                    Case "FRA"
                        currentTask.Text = "Activation des caractéristiques en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "Ativar características..."
                    Case "ITA"
                        currentTask.Text = "Abilitazione funzionalità..."
                End Select
            Case 1
                currentTask.Text = "Enabling features..."
            Case 2
                currentTask.Text = "Habilitando características..."
            Case 3
                currentTask.Text = "Activation des caractéristiques en cours..."
            Case 4
                currentTask.Text = "Ativar características..."
            Case 5
                currentTask.Text = "Abilitazione funzionalità..."
        End Select
        CurrentPB.Maximum = featEnablementCount
        For x = 0 To Array.LastIndexOf(featEnablementNames, featEnablementLastName)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            currentTask.Text = "Enabling feature " & (x + 1) & " of " & featEnablementCount & "..."
                        Case "ESN"
                            currentTask.Text = "Habilitando característica " & (x + 1) & " de " & featEnablementCount & "..."
                        Case "FRA"
                            currentTask.Text = "Activation de la caractéristique " & (x + 1) & " de " & featEnablementCount & " en cours..."
                        Case "PTB", "PTG"
                            currentTask.Text = "Ativar a caraterística " & (x + 1) & " de " & featEnablementCount & "..."
                        Case "ITA"
                            currentTask.Text = "Abilitazione funzionalità " & (x + 1) & " di " & featEnablementCount & "..."
                    End Select
                Case 1
                    currentTask.Text = "Enabling feature " & (x + 1) & " of " & featEnablementCount & "..."
                Case 2
                    currentTask.Text = "Habilitando característica " & (x + 1) & " de " & featEnablementCount & "..."
                Case 3
                    currentTask.Text = "Activation de la caractéristique " & (x + 1) & " de " & featEnablementCount & " en cours..."
                Case 4
                    currentTask.Text = "Ativar a caraterística " & (x + 1) & " de " & featEnablementCount & "..."
                Case 5
                    currentTask.Text = "Abilitazione funzionalità " & (x + 1) & " di " & featEnablementCount & "..."
            End Select
            LogView.AppendText(CrLf &
                               "Feature " & (x + 1) & " of " & featEnablementCount)
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
                                       "- Feature name: " & featInfo.FeatureName & CrLf &
                                       "- Feature description: " & featInfo.Description & CrLf)
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
            LogView.AppendText(CrLf & "Getting error level...")
            GetFeatErrorLevel()
            If errCode.Length >= 8 Then
                LogView.AppendText(" Error level : 0x" & errCode)
            Else
                LogView.AppendText(" Error level : " & errCode)
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
        LogView.AppendText(CrLf & "Gathering error level for selected features..." & CrLf)
        For x = 0 To FeatureErrorCodes.Count - 1
            LogView.AppendText(CrLf & "- Feature no. " & (x + 1) & ": " & FeatureErrorCodes(x))
        Next
        Thread.Sleep(2000)
        If featEnablementCommit Then
            DynaLog.LogMessage("Preparing to save changes...")
            AllPB.Value = AllPB.Maximum / taskCount
            currentTCont += 1
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                        Case "ESN"
                            taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                        Case "FRA"
                            taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                        Case "PTB", "PTG"
                            taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                        Case "ITA"
                            taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
                    End Select
                Case 1
                    taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                Case 2
                    taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                Case 3
                    taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                Case 4
                    taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                Case 5
                    taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
            End Select
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
            LogView.AppendText(CrLf & "Some features require a system restart to be fully processed. Save your work, close your programs, and restart when ready")
        End If
    End Sub

    Private Sub DisableFeatures(targetImage As String)
        DynaLog.LogMessage("Preparing to disable features...")
        DynaLog.LogMessage("- Will a parent package name be used? " & If(featDisablementParentPkgUsed, "Yes", "No"))
        DynaLog.LogMessage("- Parent package name: " & Quote & featDisablementParentPkg & Quote)
        DynaLog.LogMessage("- Remove feature manifest? " & If(featDisablementRemoveManifest, "Yes", "No"))
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Disabling features..."
                        currentTask.Text = "Preparing to disable features..."
                    Case "ESN"
                        allTasks.Text = "Deshabilitando características..."
                        currentTask.Text = "Preparándonos para deshabilitar características..."
                    Case "FRA"
                        allTasks.Text = "Désactivation des caractéristiques en cours..."
                        currentTask.Text = "Préparation de la désactivation des caractéristiques en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Desativar características..."
                        currentTask.Text = "A preparar a desativação de características..."
                    Case "ITA"
                        allTasks.Text = "Disabilitazione funzionalità..."
                        currentTask.Text = "Preparazione disabilitazione funzionalità..."
                End Select
            Case 1
                allTasks.Text = "Disabling features..."
                currentTask.Text = "Preparing to disable features..."
            Case 2
                allTasks.Text = "Deshabilitando características..."
                currentTask.Text = "Preparándonos para deshabilitar características..."
            Case 3
                allTasks.Text = "Désactivation des caractéristiques en cours..."
                currentTask.Text = "Préparation de la désactivation des caractéristiques en cours..."
            Case 4
                allTasks.Text = "Desativar características..."
                currentTask.Text = "A preparar a desativação de características..."
            Case 5
                allTasks.Text = "Disabilitazione funzionalità..."
                currentTask.Text = "Preparazione disabilitazione funzionalità..."
        End Select
        LogView.AppendText(CrLf & "Disabling features..." & CrLf &
                           "Options:" & CrLf)
        If featDisablementParentPkgUsed Then
            LogView.AppendText("- Use parent package to disable features? Yes")
        Else
            LogView.AppendText("- Use parent package to disable features? No")
        End If
        If featDisablementParentPkg = "" Then
            LogView.AppendText(CrLf & "- Parent package name: not specified")
        Else
            LogView.AppendText(CrLf & "- Parent package name: " & Quote & featDisablementParentPkg & Quote)
        End If
        If featDisablementRemoveManifest Then
            LogView.AppendText(CrLf & "- Remove feature manifest? Yes")
        Else
            LogView.AppendText(CrLf & "- Remove feature manifest? No")
        End If
        LogView.AppendText(CrLf & CrLf & "Enumerating features to disable...")
        Thread.Sleep(500)
        LogView.AppendText(CrLf & "Total number of features to disable: " & featDisablementCount)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Disabling features..."
                    Case "ESN"
                        currentTask.Text = "Deshabilitando características..."
                    Case "FRA"
                        currentTask.Text = "Désactivation des caractéristiques en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "Desativar características..."
                    Case "ITA"
                        currentTask.Text = "Disabilitazione funzionalità..."
                End Select
            Case 1
                currentTask.Text = "Disabling features..."
            Case 2
                currentTask.Text = "Deshabilitando características..."
            Case 3
                currentTask.Text = "Désactivation des caractéristiques en cours..."
            Case 4
                currentTask.Text = "Desativar características..."
            Case 5
                currentTask.Text = "Disabilitazione funzionalità..."
        End Select
        CurrentPB.Maximum = featDisablementCount
        For x = 0 To Array.LastIndexOf(featDisablementNames, featDisablementLastName)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            currentTask.Text = "Disabling feature " & (x + 1) & " of " & featDisablementCount & "..."
                        Case "ESN"
                            currentTask.Text = "Deshabilitando característica " & (x + 1) & " de " & featDisablementCount & "..."
                        Case "FRA"
                            currentTask.Text = "Désactivation de la caractéristique " & (x + 1) & " de " & featDisablementCount & " en cours..."
                        Case "PTB", "PTG"
                            currentTask.Text = "Desativar a caraterística " & (x + 1) & " de " & featDisablementCount & "..."
                        Case "ITA"
                            currentTask.Text = "Disabilitazione funzionalità " & (x + 1) & " di " & featDisablementCount & "..."
                    End Select
                Case 1
                    currentTask.Text = "Disabling feature " & (x + 1) & " of " & featDisablementCount & "..."
                Case 2
                    currentTask.Text = "Deshabilitando característica " & (x + 1) & " de " & featDisablementCount & "..."
                Case 3
                    currentTask.Text = "Désactivation de la caractéristique " & (x + 1) & " de " & featDisablementCount & " en cours..."
                Case 4
                    currentTask.Text = "Desativar a caraterística " & (x + 1) & " de " & featDisablementCount & "..."
                Case 5
                    currentTask.Text = "Disabilitazione funzionalità " & (x + 1) & " di " & featDisablementCount & "..."
            End Select
            LogView.AppendText(CrLf &
                               "Feature " & (x + 1) & " of " & featDisablementCount)
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
                                       "- Feature name: " & featInfo.FeatureName & CrLf &
                                       "- Feature description: " & featInfo.Description & CrLf)

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
            LogView.AppendText(CrLf & "Getting error level...")
            errCode = Hex(Decimal.ToInt32(DismExitCode))
            If DismExitCode = 0 Then
                featSuccessfulDisablements += 1
            Else
                featFailedDisablements += 1
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(" Error level : 0x" & errCode)
            Else
                LogView.AppendText(" Error level : " & errCode)
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
        LogView.AppendText(CrLf & "Gathering error level for selected features..." & CrLf)
        For x = 0 To FeatureErrorCodes.Count - 1
            LogView.AppendText(CrLf & "- Feature no. " & (x + 1) & ": " & FeatureErrorCodes(x))
        Next
        Thread.Sleep(2000)
        If featSuccessfulDisablements > 0 Then
            GetErrorCode(True)
        ElseIf featSuccessfulDisablements <= 0 Then
            GetErrorCode(False)
        End If
        If FeatureErrorCodes.Contains("BC2") Then
            DynaLog.LogMessage("A system restart is needed to fully apply some features.")
            LogView.AppendText(CrLf & "Some features require a system restart to be fully processed. Save your work, close your programs, and restart when ready")
        End If
    End Sub

    Private Sub CleanupImage(targetImage As String)
        DynaLog.LogMessage("Preparing to clean up the image...")
        DynaLog.LogMessage("Cleanup task: " & CleanupTask)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Cleaning up the image..."
                    Case "ESN"
                        allTasks.Text = "Limpiando la imagen..."
                    Case "FRA"
                        allTasks.Text = "Nettoyage de l'image en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Limpar a imagem..."
                    Case "ITA"
                        allTasks.Text = "Pulizia immagine..."
                End Select
            Case 1
                allTasks.Text = "Cleaning up the image..."
            Case 2
                allTasks.Text = "Limpiando la imagen..."
            Case 3
                allTasks.Text = "Nettoyage de l'image en cours..."
            Case 4
                allTasks.Text = "Limpar a imagem..."
            Case 5
                allTasks.Text = "Pulizia immagine..."
        End Select
        CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /cleanup-image"
        Select Case CleanupTask
            Case 0
                DynaLog.LogMessage("Reverting pending servicing actions to a last known good state...")
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                currentTask.Text = "Reverting pending servicing actions..."
                            Case "ESN"
                                currentTask.Text = "Revirtiendo acciones de servicio pendientes..."
                            Case "FRA"
                                currentTask.Text = "Annulation des actions de maintenance en cours..."
                            Case "PTB", "PTG"
                                currentTask.Text = "Reverter acções de manutenção pendentes..."
                            Case "ITA"
                                currentTask.Text = "Ripristino azioni assistenza in sospeso..."
                        End Select
                    Case 1
                        currentTask.Text = "Reverting pending servicing actions..."
                    Case 2
                        currentTask.Text = "Revirtiendo acciones de servicio pendientes..."
                    Case 3
                        currentTask.Text = "Annulation des actions de maintenance en cours..."
                    Case 4
                        currentTask.Text = "Reverter acções de manutenção pendentes..."
                    Case 5
                        currentTask.Text = "Ripristino azioni assistenza in sospeso..."
                End Select
                LogView.AppendText(CrLf &
                                   "Reverting pending servicing actions...")
                CommandArgs &= " /revertpendingactions"
            Case 1
                DynaLog.LogMessage("Cleaning up Service Pack backup files...")
                DynaLog.LogMessage("- Hide Service Packs from Installed Updates list? " & If(CleanupHideSP, "Yes", "No"))
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                currentTask.Text = "Cleaning up Service Pack backup files..."
                            Case "ESN"
                                currentTask.Text = "Limpiando archivos de copia de seguridad del Service Pack..."
                            Case "FRA"
                                currentTask.Text = "Nettoyage des fichiers de sauvegarde du Service Pack en cours..."
                            Case "PTB", "PTG"
                                currentTask.Text = "Limpeza dos ficheiros de cópia de segurança do Service Pack..."
                            Case "ITA"
                                currentTask.Text = "Pulizia file backup Service Pack..."
                        End Select
                    Case 1
                        currentTask.Text = "Cleaning up Service Pack backup files..."
                    Case 2
                        currentTask.Text = "Limpiando archivos de copia de seguridad del Service Pack..."
                    Case 3
                        currentTask.Text = "Nettoyage des fichiers de sauvegarde du Service Pack en cours..."
                    Case 4
                        currentTask.Text = "Limpeza dos ficheiros de cópia de segurança do Service Pack..."
                    Case 5
                        currentTask.Text = "Pulizia file backup Service Pack..."
                End Select
                LogView.AppendText(CrLf &
                                   "Cleaning up Service Pack backup files..." & CrLf &
                                   "Options:" & CrLf &
                                   "- Hide Service Packs from the Installed Updates list? " & If(CleanupHideSP, "Yes", "No"))
                CommandArgs &= " /spsuperseded" & If(CleanupHideSP, " /hidesp", "")
            Case 2
                DynaLog.LogMessage("Cleaning up component store...")
                DynaLog.LogMessage("- Reset superseded component base? " & If(ResetCompBase, "Yes", "No"))
                DynaLog.LogMessage("- Defer long operations? " & If(DeferCleanupOps, "Yes", "No"))
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                currentTask.Text = "Cleaning up the component store..."
                            Case "ESN"
                                currentTask.Text = "Limpiando el almacén de componentes..."
                            Case "FRA"
                                currentTask.Text = "Nettoyage du stock de composants en cours..."
                            Case "PTB", "PTG"
                                currentTask.Text = "Limpar o armazenamento de componentes..."
                            Case "ITA"
                                currentTask.Text = "Pulizia archivio componenti..."
                        End Select
                    Case 1
                        currentTask.Text = "Cleaning up the component store..."
                    Case 2
                        currentTask.Text = "Limpiando el almacén de componentes..."
                    Case 3
                        currentTask.Text = "Nettoyage du stock de composants en cours..."
                    Case 4
                        currentTask.Text = "Limpar o armazenamento de componentes..."
                    Case 5
                        currentTask.Text = "Pulizia archivio componenti..."
                End Select
                LogView.AppendText(CrLf &
                                   "Cleaning up the component store..." & CrLf &
                                   "Options:" & CrLf &
                                   "- Perform superseded component base reset? " & If(ResetCompBase, "Yes", "No") & CrLf &
                                   "- Defer long-running operations? " & If(DeferCleanupOps, "Yes", "No"))
                CommandArgs &= " /startcomponentcleanup" & If(ResetCompBase, " /resetbase", "") & If(ResetCompBase And DeferCleanupOps, " /defer", "")
            Case 3
                DynaLog.LogMessage("Analyzing component store...")
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                currentTask.Text = "Analyzing the component store..."
                            Case "ESN"
                                currentTask.Text = "Analizando el almacén de componentes..."
                            Case "FRA"
                                currentTask.Text = "Analyse du stock de composants en cours..."
                            Case "PTB", "PTG"
                                currentTask.Text = "Analisando o armazenamento de componentes..."
                            Case "ITA"
                                currentTask.Text = "Analisi archivio componenti..."
                        End Select
                    Case 1
                        currentTask.Text = "Analyzing the component store..."
                    Case 2
                        currentTask.Text = "Analizando el almacén de componentes..."
                    Case 3
                        currentTask.Text = "Analyse du stock de composants en cours..."
                    Case 4
                        currentTask.Text = "Analisando o armazenamento de componentes..."
                    Case 5
                        currentTask.Text = "Analisi archivio componenti..."
                End Select
                LogView.AppendText(CrLf &
                                   "Analyzing the component store...")
                CommandArgs &= " /analyzecomponentstore"
            Case 4
                DynaLog.LogMessage("Checking component store health...")
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                currentTask.Text = "Checking the component store health..."
                            Case "ESN"
                                currentTask.Text = "Comprobando la salud del almacén de componentes..."
                            Case "FRA"
                                currentTask.Text = "Vérification de l'état de santé du stock de composants en cours..."
                            Case "PTB", "PTG"
                                currentTask.Text = "Verificar a integridade do armazenamento de componentes..."
                            Case "ITA"
                                currentTask.Text = "Controllo stato di salute archivio componenti..."
                        End Select
                    Case 1
                        currentTask.Text = "Checking the component store health..."
                    Case 2
                        currentTask.Text = "Comprobando la salud del almacén de componentes..."
                    Case 3
                        currentTask.Text = "Vérification de l'état de santé du stock de composants en cours..."
                    Case 4
                        currentTask.Text = "Verificar a integridade do armazenamento de componentes..."
                    Case 5
                        currentTask.Text = "Controllo stato di salute archivio componenti..."
                End Select
                LogView.AppendText(CrLf &
                                   "Checking the component store health...")
                CommandArgs &= " /checkhealth"
            Case 5
                DynaLog.LogMessage("Scanning component store...")
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                currentTask.Text = "Scanning the component store..."
                            Case "ESN"
                                currentTask.Text = "Escaneando el almacén de componentes..."
                            Case "FRA"
                                currentTask.Text = "Analyse du stock de composants en cours..."
                            Case "PTB", "PTG"
                                currentTask.Text = "A analisar o armazenamento de componentes..."
                            Case "ITA"
                                currentTask.Text = "Scansione archivio componenti..."
                        End Select
                    Case 1
                        currentTask.Text = "Scanning the component store..."
                    Case 2
                        currentTask.Text = "Escaneando el almacén de componentes..."
                    Case 3
                        currentTask.Text = "Analyse du stock de composants en cours..."
                    Case 4
                        currentTask.Text = "A analisar o armazenamento de componentes..."
                    Case 5
                        currentTask.Text = "Scansione archivio componenti..."
                End Select
                LogView.AppendText(CrLf &
                                   "Scanning the component store...")
                CommandArgs &= " /scanhealth"
            Case 6
                DynaLog.LogMessage("Repairing component store...")
                DynaLog.LogMessage("- Source: " & Quote & ComponentRepairSource & Quote)
                DynaLog.LogMessage("- Limit Windows Update access (only for active installations)? " & If(LimitWUAccess, "Yes", "No"))
                DynaLog.LogMessage("Boot mode of host system: " & SystemInformation.BootMode)
                ' The most known thing about DISM : dism /online /cleanup-image /restorehealth
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                currentTask.Text = "Repairing the component store..."
                            Case "ESN"
                                currentTask.Text = "Reparando el almacén de componentes..."
                            Case "FRA"
                                currentTask.Text = "Réparation du stock de composants en cours..."
                            Case "PTB", "PTG"
                                currentTask.Text = "Reparar o armazenamento de componentes..."
                            Case "ITA"
                                currentTask.Text = "Riparazione archivio componenti..."
                        End Select
                    Case 1
                        currentTask.Text = "Repairing the component store..."
                    Case 2
                        currentTask.Text = "Reparando el almacén de componentes..."
                    Case 3
                        currentTask.Text = "Réparation du stock de composants en cours..."
                    Case 4
                        currentTask.Text = "Reparar o armazenamento de componentes..."
                    Case 5
                        currentTask.Text = "Riparazione archivio componenti..."
                End Select
                LogView.AppendText(CrLf &
                                   "Repairing the component store..." & CrLf &
                                   "Options:" & CrLf &
                                   "- Use different source? " & If(UseCompRepairSource, "Yes (" & Quote & ComponentRepairSource & Quote & ")", "No") & CrLf &
                                   "- Limit Windows Update access? " & If(LimitWUAccess And OnlineMgmt, "Yes", If(LimitWUAccess And Not OnlineMgmt, "No, this is not an online installation", "No")) &
                                   If(Not LimitWUAccess And OnlineMgmt And SystemInformation.BootMode = BootMode.FailSafe, ", the system is in Safe Mode", ""))
                CommandArgs &= " /restorehealth" & If(UseCompRepairSource And File.Exists(ComponentRepairSource), " /source=" & Quote & ComponentRepairSource & Quote, "") & If(LimitWUAccess And OnlineMgmt, " /limitaccess", "")
        End Select
        RunProcess(DismProgram, CommandArgs)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta livello errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        End If
    End Sub

#End Region

#Region "Provisioning Package Management Tasks"

    Private Sub AddProvisioningPackage(targetImage As String)
        DynaLog.LogMessage("Preparing to add provisioning package to the Windows image...")
        DynaLog.LogMessage("- Provisioning package: " & Quote & ppkgAdditionPackagePath & Quote)
        DynaLog.LogMessage("- Catalog path: " & Quote & ppkgAdditionCatalogPath & Quote)
        DynaLog.LogMessage("- Commit image after finishing? " & If(ppkgAdditionCommit, "Yes", "No"))
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Adding provisioning package..."
                        currentTask.Text = "Adding provisioning package to the image..."
                    Case "ESN"
                        allTasks.Text = "Añadiendo paquete de aprovisionamiento..."
                        currentTask.Text = "Añadiendo paquete de aprovisionamiento a la imagen..."
                    Case "FRA"
                        allTasks.Text = "Ajout d'un paquet de provisionnement en cours..."
                        currentTask.Text = "Ajout d'un paquet de provisionnement à l'image en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Adicionando pacote de provisionamento..."
                        currentTask.Text = "Adicionar pacote de aprovisionamento à imagem..."
                    Case "ITA"
                        allTasks.Text = "Aggiunta pacchetto approvvigionamento..."
                        currentTask.Text = "Aggiunta pacchetto approvvigionamento all'immagine..."
                End Select
            Case 1
                allTasks.Text = "Adding provisioning package..."
                currentTask.Text = "Adding provisioning package to the image..."
            Case 2
                allTasks.Text = "Añadiendo paquete de aprovisionamiento..."
                currentTask.Text = "Añadiendo paquete de aprovisionamiento a la imagen..."
            Case 3
                allTasks.Text = "Ajout d'un paquet de provisionnement en cours..."
                currentTask.Text = "Ajout d'un paquet de provisionnement à l'image en cours..."
            Case 4
                allTasks.Text = "Adicionando pacote de provisionamento..."
                currentTask.Text = "Adicionar pacote de aprovisionamento à imagem..."
            Case 5
                allTasks.Text = "Aggiunta pacchetto approvvigionamento..."
                currentTask.Text = "Aggiunta pacchetto approvvigionamento all'immagine..."
        End Select
        LogView.AppendText("Adding provisioning package to the image..." & CrLf &
                           "Options:" & CrLf & CrLf &
                           "- Provisioning package: " & Quote & ppkgAdditionPackagePath & Quote & CrLf &
                           "- Catalog file: " & If(ppkgAdditionCatalogPath = "", "none specified", Quote & ppkgAdditionCatalogPath & Quote) & CrLf &
                           "- Commit image after adding provisioning package? " & If(ppkgAdditionCommit, "Yes", "No"))
        CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /add-provisioningpackage /packagepath=" & Quote & ppkgAdditionPackagePath & Quote & If(ppkgAdditionCatalogPath <> "" And File.Exists(ppkgAdditionCatalogPath), " /catalogpath=" & Quote & ppkgAdditionCatalogPath & Quote, "")
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & "Getting error level...")
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(" Error level : 0x" & errCode)
        Else
            LogView.AppendText(" Error level : " & errCode)
        End If
        If ppkgAdditionCommit Then
            DynaLog.LogMessage("Preparing to save changes...")
            AllPB.Value = AllPB.Maximum / taskCount
            currentTCont += 1
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                        Case "ESN"
                            taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                        Case "FRA"
                            taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                        Case "PTB", "PTG"
                            taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                        Case "ITA"
                            taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
                    End Select
                Case 1
                    taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                Case 2
                    taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                Case 3
                    taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                Case 4
                    taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                Case 5
                    taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
            End Select
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Adding AppX packages..."
                        currentTask.Text = "Preparing to add provisioned AppX packages..."
                    Case "ESN"
                        allTasks.Text = "Añadiendo paquetes aprovisionados AppX..."
                        currentTask.Text = "Preparándonos para añadir paquetes aprovisionados AppX..."
                    Case "FRA"
                        allTasks.Text = "Ajout de paquets AppX en cours..."
                        currentTask.Text = "Préparation de l'ajout de paquets AppX provisionnés en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "A adicionar pacotes AppX..."
                        currentTask.Text = "A preparar a adição de pacotes AppX provisionados..."
                    Case "ITA"
                        allTasks.Text = "Aggiunta pacchetti AppX..."
                        currentTask.Text = "Preparazione aggiunta pacchetti AppX approvvigionati..."
                End Select
            Case 1
                allTasks.Text = "Adding AppX packages..."
                currentTask.Text = "Preparing to add provisioned AppX packages..."
            Case 2
                allTasks.Text = "Añadiendo paquetes aprovisionados AppX..."
                currentTask.Text = "Preparándonos para añadir paquetes aprovisionados AppX..."
            Case 3
                allTasks.Text = "Ajout de paquets AppX en cours..."
                currentTask.Text = "Préparation de l'ajout de paquets AppX provisionnés en cours..."
            Case 4
                allTasks.Text = "A adicionar pacotes AppX..."
                currentTask.Text = "A preparar a adição de pacotes AppX provisionados..."
            Case 5
                allTasks.Text = "Aggiunta pacchetti AppX..."
                currentTask.Text = "Preparazione aggiunta pacchetti AppX approvvigionati..."
        End Select
        LogView.AppendText(CrLf & "Adding provisioned AppX packages..." & CrLf &
                           "Options:" & CrLf)
        If appxAdditionUseLicenseFile Then
            LogView.AppendText("- Use a license file for AppX packages? Yes" & CrLf &
                               "- License file: " & appxAdditionLicenseFile & CrLf)
        Else
            LogView.AppendText("- Use a license file for AppX packages? No" & CrLf &
                               "- License file: not using" & CrLf)
        End If
        If appxAdditionUseCustomDataFile Then
            LogView.AppendText("- Use a custom data file for AppX packages? Yes" & CrLf &
                               "- Custom data file: " & appxAdditionCustomDataFile & CrLf)
        Else
            LogView.AppendText("- Use a custom data file for AppX packages? No" & CrLf &
                               "- Custom data file: not using" & CrLf)
        End If
        If appxAdditionUseAllRegions Then
            LogView.AppendText("- Use all regions for AppX packages? Yes" & CrLf &
                               "- Package regions: all" & CrLf)
        Else
            LogView.AppendText("- Use all regions for AppX packages? No" & CrLf &
                               "- Package regions: " & Quote & appxAdditionRegions & Quote & CrLf)
        End If
        If appxAdditionCommit Then
            LogView.AppendText("- Commit image after adding AppX packages? Yes")
        Else
            LogView.AppendText("- Commit image after adding AppX packages? No")
        End If
        LogView.AppendText(CrLf & CrLf & "Enumerating AppX packages to add...")
        Thread.Sleep(500)
        LogView.AppendText(CrLf & "Total number of packages to add: " & appxAdditionCount)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Adding AppX packages..."
                    Case "ESN"
                        currentTask.Text = "Añadiendo paquetes AppX..."
                    Case "FRA"
                        currentTask.Text = "Ajout de paquets AppX en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A adicionar pacotes AppX..."
                    Case "ITA"
                        currentTask.Text = "Aggiunta pacchetti AppX..."
                End Select
            Case 1
                currentTask.Text = "Adding AppX packages..."
            Case 2
                currentTask.Text = "Añadiendo paquetes AppX..."
            Case 3
                currentTask.Text = "Ajout de paquets AppX en cours..."
            Case 4
                currentTask.Text = "A adicionar pacotes AppX..."
            Case 5
                currentTask.Text = "Aggiunta pacchetti AppX..."
        End Select
        CurrentPB.Maximum = appxAdditionCount
        For x = 0 To Array.LastIndexOf(appxAdditionPackages, appxAdditionLastPackage)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            currentTask.Text = "Adding package " & (x + 1) & " of " & appxAdditionCount & "..."
                        Case "ESN"
                            currentTask.Text = "Añadiendo paquete " & (x + 1) & " de " & appxAdditionCount & "..."
                        Case "FRA"
                            currentTask.Text = "Ajout du paquet " & (x + 1) & " de " & appxAdditionCount & " en cours..."
                        Case "PTB", "PTG"
                            currentTask.Text = "A adicionar pacote " & (x + 1) & " de " & appxAdditionCount & "..."
                        Case "ITA"
                            currentTask.Text = "Aggiunta pacchetto " & (x + 1) & " di " & appxAdditionCount & "..."
                    End Select
                Case 1
                    currentTask.Text = "Adding package " & (x + 1) & " of " & appxAdditionCount & "..."
                Case 2
                    currentTask.Text = "Añadiendo paquete " & (x + 1) & " de " & appxAdditionCount & "..."
                Case 3
                    currentTask.Text = "Ajout du paquet " & (x + 1) & " de " & appxAdditionCount & " en cours..."
                Case 4
                    currentTask.Text = "A adicionar pacote " & (x + 1) & " de " & appxAdditionCount & "..."
                Case 5
                    currentTask.Text = "Aggiunta pacchetto " & (x + 1) & " di " & appxAdditionCount & "..."
            End Select
            LogView.AppendText(CrLf &
                               "Package " & (x + 1) & " of " & appxAdditionCount)
            CurrentPB.Value = x + 1
            DynaLog.LogMessage("Information about the AppX package:")
            DynaLog.LogMessage(appxAdditionPackageList(x).ToString())
            LogView.AppendText(CrLf &
                               "- AppX package file: " & appxAdditionPackageList(x).PackageFile & CrLf &
                               "- Application name: " & appxAdditionPackageList(x).PackageName & CrLf &
                               "- Application publisher: " & appxAdditionPackageList(x).PackagePublisher & CrLf &
                               "- Application version: " & appxAdditionPackageList(x).PackageVersion & CrLf)
            ' Detect if it is an encrypted application
            DynaLog.LogMessage("Extension of AppX package: " & Path.GetExtension(appxAdditionPackageList(x).PackageFile))
            If Path.GetExtension(appxAdditionPackageList(x).PackageFile).Replace(".", "").Trim().StartsWith("e", StringComparison.OrdinalIgnoreCase) AndAlso OnlineMgmt Then
                DynaLog.LogMessage("The application is encrypted and the active installation is being managed. Adding package using PowerShell...")
                ' Run PowerShell command. Support will be improved
                LogView.AppendText(CrLf & "The application about to be added is an encrypted file. Since the program is managing the active installation, a PowerShell command will be run." & CrLf)
                Dim AppxAuxProc As New Process()
                AppxAuxProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
                CommandArgs = "-Command Add-AppxPackage -Path '" & appxAdditionPackageList(x).PackageFile & "'"
                AppxAuxProc.StartInfo.Arguments = CommandArgs
                AppxAuxProc.Start()
                AppxAuxProc.WaitForExit()
                LogView.AppendText(CrLf & "Getting error level...")
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
                    LogView.AppendText(" Error level : 0x" & errCode)
                Else
                    LogView.AppendText(" Error level : " & errCode)
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
                LogView.AppendText(CrLf & "The application about to be added is an encrypted file. Encrypted packages can only be added to active installations. Skipping this package..." & CrLf)
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
                                           "Warning: the license file does not exist. Continuing without one..." & CrLf &
                                           "         Do note that, if this app requires a license file, it may fail addition." & CrLf &
                                           "         Also, this may compromise the image.")
                    End If
                    CommandArgs &= " /skiplicense"
                End If
                ' Inform user that a package will be installed with dependencies
                DynaLog.LogMessage("Count of dependencies: " & appxAdditionPackageList(x).PackageSpecifiedDependencies.Count)
                If appxAdditionPackageList(x).PackageSpecifiedDependencies.Count > 0 Then
                    LogView.AppendText("- The following dependency packages will be installed alongside this application:" & CrLf)
                End If
                ' Add dependencies
                For Each Dependency As AppxDependency In appxAdditionPackageList(x).PackageSpecifiedDependencies
                    DynaLog.LogMessage("Verifying if dependency " & Quote & Path.GetFileName(Dependency.DependencyFile) & Quote & " exists...")
                    If File.Exists(Dependency.DependencyFile) Then
                        DynaLog.LogMessage("The dependency exists in the file system.")
                        LogView.AppendText("    - Dependency: " & Quote & Path.GetFileName(Dependency.DependencyFile) & Quote & CrLf)
                        CommandArgs &= " /dependencypackagepath=" & Quote & Dependency.DependencyFile & Quote
                    Else
                        DynaLog.LogMessage("The dependency does not exist in the file system.")
                        LogView.AppendText(CrLf &
                                           "Warning: the dependency" & CrLf &
                                           Quote & Dependency.DependencyFile & Quote & CrLf &
                                           "does not exist in the file system. Skipping dependency...")
                        Continue For
                    End If
                Next
                If appxAdditionPackageList(x).PackageCustomDataFile <> "" And File.Exists(appxAdditionPackageList(x).PackageCustomDataFile) Then
                    DynaLog.LogMessage("A custom data file has been specified and it exists in the file system.")
                    CommandArgs &= " /customdatapath=" & Quote & appxAdditionCustomDataFile & Quote
                ElseIf appxAdditionPackageList(x).PackageCustomDataFile <> "" And Not File.Exists(appxAdditionPackageList(x).PackageCustomDataFile) Then
                    DynaLog.LogMessage("A custom data file has been specified but it does not exist in the file system.")
                    LogView.AppendText(CrLf &
                                       "Warning: the custom data file does not exist. Continuing without one...")
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
            LogView.AppendText(CrLf & "Getting error level...")
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
                LogView.AppendText(" Error level : 0x" & errCode)
            Else
                LogView.AppendText(" Error level : " & errCode)
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
        LogView.AppendText(CrLf & "Gathering error level for selected AppX packages..." & CrLf)
        For x = 0 To PackageErrorCodes.Count - 1
            LogView.AppendText(CrLf & "- Package no. " & (x + 1) & ": " & PackageErrorCodes(x))
        Next
        Thread.Sleep(2000)
        If appxAdditionCommit Then
            DynaLog.LogMessage("Preparing to save changes...")
            AllPB.Value = AllPB.Maximum / taskCount
            currentTCont += 1
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                        Case "ESN"
                            taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                        Case "FRA"
                            taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                        Case "PTB", "PTG"
                            taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                        Case "ITA"
                            taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
                    End Select
                Case 1
                    taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                Case 2
                    taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                Case 3
                    taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                Case 4
                    taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                Case 5
                    taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
            End Select
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
                                   "- Application is registered to a user? No")
            Else
                DynaLog.LogMessage(".pckgdep files for AppX package " & Quote & removalStoreApp & Quote & " > 0. This app is registered to users")
                ' Application is registered to a user
                LogView.AppendText(CrLf &
                                   "- Application is registered to a user? Yes" & CrLf &
                                   "  The removal of this application may require you to use PowerShell to completely remove it")
            End If
        Else
            DynaLog.LogMessage(".pckgdep files for AppX package " & Quote & removalStoreApp & Quote & " = 0. This app is not registered to a user")
            ' Application is not registered to any user
            LogView.AppendText(CrLf &
                               "- Application is registered to a user? No")
        End If
    End Sub

    Private Sub RemoveOnlineAppxPackages(ParamArray PackageNames As String())
        Dim extAppxHelperPath As String = Path.Combine(Application.StartupPath, "bin", "extps1", "online_appx_removal.ps1")
        If File.Exists(extAppxHelperPath) Then
            DynaLog.LogMessage("AppX removal helper exists. Proceeding with the removal of those bastards!")
            LogView.AppendText(CrLf & "A PowerShell helper will be used to remove AppX packages. Please wait...")
            RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "WindowsPowerShell", "v1.0", "powershell.exe"),
                       String.Format("-executionpolicy Bypass -noprofile -nologo -file {0}{1}{0} -appxFullNames {0}{2}{0}", Quote, extAppxHelperPath,
                                     String.Join(";", PackageNames.Where(Function(PackageName) Not String.IsNullOrEmpty(PackageName)))))
            LogView.AppendText(CrLf & "Log off for the deprovisioning of applications to be fully carried out.")
        End If
    End Sub

    Private Sub RemoveProvisionedAppxPackages(targetImage As String)
        DynaLog.LogMessage("Preparing to remove AppX packages...")
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Removing AppX packages..."
                        currentTask.Text = "Preparing to remove provisioned AppX packages..."
                    Case "ESN"
                        allTasks.Text = "Eliminando paquetes AppX..."
                        currentTask.Text = "Preparándonos para eliminar paquetes aprovisionados AppX..."
                    Case "FRA"
                        allTasks.Text = "Suppression des paquets AppX en cours..."
                        currentTask.Text = "Préparation de la suppression des paquets AppX en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Removendo pacotes AppX..."
                        currentTask.Text = "A preparar a remoção de pacotes AppX provisionados..."
                    Case "ITA"
                        allTasks.Text = "Rimozione pacchetti AppX..."
                        currentTask.Text = "Preparazione rimozione pacchetti AppX approvvigionati..."
                End Select
            Case 1
                allTasks.Text = "Removing AppX packages..."
                currentTask.Text = "Preparing to remove provisioned AppX packages..."
            Case 2
                allTasks.Text = "Eliminando paquetes AppX..."
                currentTask.Text = "Preparándonos para eliminar paquetes aprovisionados AppX..."
            Case 3
                allTasks.Text = "Suppression des paquets AppX en cours..."
                currentTask.Text = "Préparation de la suppression des paquets AppX en cours..."
            Case 4
                allTasks.Text = "Removendo pacotes AppX..."
                currentTask.Text = "A preparar a remoção de pacotes AppX provisionados..."
            Case 5
                allTasks.Text = "Rimozione pacchetti AppX..."
                currentTask.Text = "Preparazione rimozione pacchetti AppX approvvigionati..."
        End Select
        LogView.AppendText(CrLf & "Removing provisioned AppX packages..." & CrLf & CrLf &
                           "Enumerating AppX packages to remove...")
        Thread.Sleep(500)
        LogView.AppendText(CrLf & "Total number of packages to remove: " & appxRemovalCount)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Removing AppX packages..."
                    Case "ESN"
                        currentTask.Text = "Eliminando paquetes AppX..."
                    Case "FRA"
                        currentTask.Text = "Suppression des paquets AppX en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "Removendo pacotes AppX..."
                    Case "ITA"
                        currentTask.Text = "Rimozione pacchetti AppX..."
                End Select
            Case 1
                currentTask.Text = "Removing AppX packages..."
            Case 2
                currentTask.Text = "Eliminando paquetes AppX..."
            Case 3
                currentTask.Text = "Suppression des paquets AppX en cours..."
            Case 4
                currentTask.Text = "Removendo pacotes AppX..."
            Case 5
                currentTask.Text = "Rimozione pacchetti AppX..."
        End Select
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
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                currentTask.Text = "Removing package " & (x + 1) & " of " & appxRemovalCount & "..."
                            Case "ESN"
                                currentTask.Text = "Eliminando paquete " & (x + 1) & " de " & appxRemovalCount & "..."
                            Case "FRA"
                                currentTask.Text = "Suppression du paquet " & (x + 1) & " de " & appxRemovalCount & " en cours..."
                            Case "PTB", "PTG"
                                currentTask.Text = "A remover o pacote " & (x + 1) & " de " & appxRemovalCount & "..."
                            Case "ITA"
                                currentTask.Text = "Rimozione pacchetto " & (x + 1) & " di " & appxRemovalCount & "..."
                        End Select
                    Case 1
                        currentTask.Text = "Removing package " & (x + 1) & " of " & appxRemovalCount & "..."
                    Case 2
                        currentTask.Text = "Eliminando paquete " & (x + 1) & " de " & appxRemovalCount & "..."
                    Case 3
                        currentTask.Text = "Suppression du paquet " & (x + 1) & " de " & appxRemovalCount & " en cours..."
                    Case 4
                        currentTask.Text = "A remover o pacote " & (x + 1) & " de " & appxRemovalCount & "..."
                    Case 5
                        currentTask.Text = "Rimozione pacchetto " & (x + 1) & " di " & appxRemovalCount & "..."
                End Select
                LogView.AppendText(CrLf &
                                   "Package " & (x + 1) & " of " & appxRemovalCount)
                CurrentPB.Value = x + 1
                ' Display package name and DisplayName
                LogView.AppendText(CrLf &
                                   "- Package name: " & appxRemovalPackages(x) & CrLf &
                                   "- Display name: " & appxRemovalPkgNames(x))
                ' Display whether an application is registered to a user
                CheckAppRegistrationStatus(removalStoreApp)
                ' Initialize command. Its syntax is simple, so don't spend too much time determining options
                LogView.AppendText(CrLf & CrLf &
                                   "Processing package...")
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /remove-provisionedappxpackage /packagename=" & appxRemovalPackages(x)
                RunProcess(DismProgram, CommandArgs)
                LogView.AppendText(CrLf & "Getting error level...")
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
                    LogView.AppendText(" Error level : 0x" & errCode)
                Else
                    LogView.AppendText(" Error level : " & errCode)
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
            LogView.AppendText(CrLf & "Gathering error level for selected AppX packages..." & CrLf)
            For x = 0 To PackageErrorCodes.Count - 1
                LogView.AppendText(CrLf & "- Package no. " & (x + 1) & ": " & PackageErrorCodes(x))
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Setting layered driver..."
                        currentTask.Text = "Setting keyboard layered driver..."
                    Case "ESN"
                        allTasks.Text = "Estableciendo controlador superpuesto..."
                        currentTask.Text = "Estableciendo controlador de teclado superpuesto..."
                    Case "FRA"
                        allTasks.Text = "Configuration du pilote en couches en cours..."
                        currentTask.Text = "Configuration du pilote en couches pour le clavier en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Configuração do controlador em camadas..."
                        currentTask.Text = "Configuração do controlador de teclado em camadas..."
                    Case "ITA"
                        allTasks.Text = "Impostazione driver stratificato..."
                        currentTask.Text = "Impostazione driver stratificato tastiera..."
                End Select
            Case 1
                allTasks.Text = "Setting layered driver..."
                currentTask.Text = "Setting keyboard layered driver..."
            Case 2
                allTasks.Text = "Estableciendo controlador superpuesto..."
                currentTask.Text = "Estableciendo controlador de teclado superpuesto..."
            Case 3
                allTasks.Text = "Configuration du pilote en couches en cours..."
                currentTask.Text = "Configuration du pilote en couches pour le clavier en cours..."
            Case 4
                allTasks.Text = "Configuração do controlador em camadas..."
                currentTask.Text = "Configuração do controlador de teclado em camadas..."
            Case 5
                allTasks.Text = "Impostazione driver stratificato..."
                currentTask.Text = "Impostazione driver stratificato la tastiera..."
        End Select
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
        LogView.AppendText(CrLf & "Setting the keyboard layered driver..." & CrLf &
                           "- Current keyboard layered driver: " & currentLayout & CrLf &
                           "- New keyboard layered driver: " & newLayout & CrLf)
        CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /set-layereddriver:" & KeyboardLayeredDriverType
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & "Getting error level...")
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(" Error level : 0x" & errCode)
        Else
            LogView.AppendText(" Error level : " & errCode)
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Adding capabilities..."
                        currentTask.Text = "Preparing to add capabilities..."
                    Case "ESN"
                        allTasks.Text = "Añadiendo funcionalidades..."
                        currentTask.Text = "Preparándonos para añadir funcionalidades..."
                    Case "FRA"
                        allTasks.Text = "Ajout des capacités en cours..."
                        currentTask.Text = "Préparation de l'ajout des capacités en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "A adicionar capacidades..."
                        currentTask.Text = "A preparar para adicionar capacidades..."
                    Case "ITA"
                        allTasks.Text = "Aggiunta capacità..."
                        currentTask.Text = "Preparazione aggiunta capacità..."
                End Select
            Case 1
                allTasks.Text = "Adding capabilities..."
                currentTask.Text = "Preparing to add capabilities..."
            Case 2
                allTasks.Text = "Añadiendo funcionalidades..."
                currentTask.Text = "Preparándonos para añadir funcionalidades..."
            Case 3
                allTasks.Text = "Ajout des capacités en cours..."
                currentTask.Text = "Préparation de l'ajout des capacités en cours..."
            Case 4
                allTasks.Text = "A adicionar capacidades..."
                currentTask.Text = "A preparar para adicionar capacidades..."
            Case 5
                allTasks.Text = "Aggiunta capacità..."
                currentTask.Text = "Preparazione aggiunta capacità..."
        End Select
        DynaLog.LogMessage("Boot mode of the host system: " & SystemInformation.BootMode)
        LogView.AppendText(CrLf & "Adding capabilities to mounted image..." & CrLf &
                           "Options:" & CrLf &
                           "- Use a source for capability addition? " & If(capAdditionUseSource, "Yes", "No") & CrLf &
                           "- Capability source: " & If(capAdditionUseSource, Quote & capAdditionSource & Quote, "No source has been provided") & CrLf &
                           "- Limit access to Windows Update? " & If(capAdditionLimitWUAccess And OnlineMgmt, "Yes", If(capAdditionLimitWUAccess And Not OnlineMgmt, "No, this is not an online installation", "No")) & If(Not capAdditionLimitWUAccess And OnlineMgmt And SystemInformation.BootMode = BootMode.FailSafe, ", the system is in Safe Mode", "") & CrLf &
                           "- Commit image after adding capabilities? " & If(capAdditionCommit, "Yes", "No") & CrLf)
        If capAdditionUseSource And Not Directory.Exists(capAdditionSource) Then
            DynaLog.LogMessage("A source is expected to be used but it does not exist in the file system.")
            LogView.AppendText(CrLf &
                               "Warning: the specified source does not exist in the file system, and it will be skipped")
        End If
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Adding capabilities..."
                    Case "ESN"
                        currentTask.Text = "Añadiendo funcionalidades..."
                    Case "FRA"
                        currentTask.Text = "Ajout des capacités en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A adicionar capacidades..."
                    Case "ITA"
                        currentTask.Text = "Aggiunta capacità..."
                End Select
            Case 1
                currentTask.Text = "Adding capabilities..."
            Case 2
                currentTask.Text = "Añadiendo funcionalidades..."
            Case 3
                currentTask.Text = "Ajout des capacités en cours..."
            Case 4
                currentTask.Text = "A adicionar capacidades..."
            Case 5
                currentTask.Text = "Aggiunta capacità..."
        End Select
        LogView.AppendText(CrLf & "Enumerating capabilities to add. Please wait..." & CrLf &
                           "Total number of capabilities: " & capAdditionCount)
        CurrentPB.Maximum = capAdditionCount
        For x = 0 To Array.LastIndexOf(capAdditionIds, capAdditionLastId)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            currentTask.Text = "Adding capability " & (x + 1) & " of " & capAdditionCount & "..."
                        Case "ESN"
                            currentTask.Text = "Añadiendo funcionalidad " & (x + 1) & " de " & capAdditionCount & "..."
                        Case "FRA"
                            currentTask.Text = "Ajout de la capacité " & (x + 1) & " de " & capAdditionCount & " en cours..."
                        Case "PTB", "PTG"
                            currentTask.Text = "Adicionar capacidade " & (x + 1) & " de " & capAdditionCount & "..."
                        Case "ITA"
                            currentTask.Text = "Aggiunta capacità " & (x + 1) & " di " & capAdditionCount & "..."
                    End Select
                Case 1
                    currentTask.Text = "Adding capability " & (x + 1) & " of " & capAdditionCount & "..."
                Case 2
                    currentTask.Text = "Añadiendo funcionalidad " & (x + 1) & " de " & capAdditionCount & "..."
                Case 3
                    currentTask.Text = "Ajout de la capacité " & (x + 1) & " de " & capAdditionCount & " en cours..."
                Case 4
                    currentTask.Text = "Adicionar capacidade " & (x + 1) & " de " & capAdditionCount & "..."
                Case 5
                    currentTask.Text = "Aggiunta capacità " & (x + 1) & " di " & capAdditionCount & "..."
            End Select
            CurrentPB.Value = x + 1
            DynaLog.LogMessage("Getting information about capability " & Quote & capAdditionIds(x) & Quote & "...")
            LogView.AppendText(CrLf &
                               "Capability " & (x + 1) & " of " & capAdditionCount)
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
                                       "- Capability identity: " & capInfo.Name & CrLf &
                                       "- Capability name: " & capInfo.DisplayName & CrLf &
                                       "- Capability description: " & capInfo.Description & CrLf)
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
            LogView.AppendText(CrLf & "Getting error level...")
            errCode = Hex(Decimal.ToInt32(DismExitCode))
            If DismExitCode = 0 Then
                capSuccessfulAdditions += 1
            Else
                capFailedAdditions += 1
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(" Error level : 0x" & errCode)
            Else
                LogView.AppendText(" Error level : " & errCode)
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
        LogView.AppendText(CrLf & "Gathering error level for selected capabilities..." & CrLf)
        For x = 0 To FeatureErrorCodes.Count - 1
            LogView.AppendText(CrLf & "- Capability no. " & (x + 1) & ": " & FeatureErrorCodes(x))
        Next
        Thread.Sleep(2000)
        If capAdditionCommit Then
            DynaLog.LogMessage("Preparing to save changes...")
            AllPB.Value = AllPB.Maximum / taskCount
            currentTCont += 1
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                        Case "ESN"
                            taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                        Case "FRA"
                            taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                        Case "PTB", "PTG"
                            taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                        Case "ITA"
                            taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
                    End Select
                Case 1
                    taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                Case 2
                    taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                Case 3
                    taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                Case 4
                    taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                Case 5
                    taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
            End Select
            RunOps(8)
        End If
        If capSuccessfulAdditions > 0 Then
            GetErrorCode(True)
        ElseIf capSuccessfulAdditions <= 0 Then
            GetErrorCode(False)
        End If
        If FeatureErrorCodes.Contains("BC2") Then
            DynaLog.LogMessage("A system restart is needed to fully apply some capabilities.")
            LogView.AppendText(CrLf & "Some capabilities require a system restart to be fully processed. Save your work, close your programs, and restart when ready")
        End If
    End Sub

    Private Sub RemoveCapabilities(targetImage As String)
        DynaLog.LogMessage("Preparing to remove capabilities...")
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Removing capabilities..."
                        currentTask.Text = "Preparing to remove capabilities..."
                    Case "ESN"
                        allTasks.Text = "Eliminando funcionalidades..."
                        currentTask.Text = "Preparándonos para eliminar funcionalidades..."
                    Case "FRA"
                        allTasks.Text = "Suppression des capacités en cours..."
                        currentTask.Text = "Préparation de la suppression des capacités en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "A remover capacidades..."
                        currentTask.Text = "A preparar a remoção de capacidades..."
                    Case "ITA"
                        allTasks.Text = "Rimozione capacità..."
                        currentTask.Text = "Preparazione rimozione capacità..."
                End Select
            Case 1
                allTasks.Text = "Removing capabilities..."
                currentTask.Text = "Preparing to remove capabilities..."
            Case 2
                allTasks.Text = "Eliminando funcionalidades..."
                currentTask.Text = "Preparándonos para eliminar funcionalidades..."
            Case 3
                allTasks.Text = "Suppression des capacités en cours..."
                currentTask.Text = "Préparation de la suppression des capacités en cours..."
            Case 4
                allTasks.Text = "A remover capacidades..."
                currentTask.Text = "A preparar a remoção de capacidades..."
            Case 5
                allTasks.Text = "Rimozione capacità..."
                currentTask.Text = "Preparazione rimozione capacità..."
        End Select
        LogView.AppendText(CrLf & "Removing capabilities from mounted image..." & CrLf)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Removing capabilities..."
                    Case "ESN"
                        currentTask.Text = "Eliminando funcionalidades..."
                    Case "FRA"
                        currentTask.Text = "Suppression des capacités en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A remover capacidades..."
                    Case "ITA"
                        currentTask.Text = "Rimozione capacità..."
                End Select
            Case 1
                currentTask.Text = "Removing capabilities..."
            Case 2
                currentTask.Text = "Eliminando funcionalidades..."
            Case 3
                currentTask.Text = "Suppression des capacités en cours..."
            Case 4
                currentTask.Text = "A remover capacidades..."
            Case 5
                currentTask.Text = "Rimozione capacità..."
        End Select
        LogView.AppendText(CrLf & "Enumerating capabilities to remove. Please wait..." & CrLf &
                           "Total number of capabilities: " & capRemovalCount)
        CurrentPB.Maximum = capRemovalCount
        For x = 0 To Array.LastIndexOf(capRemovalIds, capRemovalLastId)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            currentTask.Text = "Removing capability " & (x + 1) & " of " & capRemovalCount & "..."
                        Case "ESN"
                            currentTask.Text = "Eliminando funcionalidad " & (x + 1) & " de " & capRemovalCount & "..."
                        Case "FRA"
                            currentTask.Text = "Suppression de la capacité " & (x + 1) & " de " & capRemovalCount & " en cours..."
                        Case "PTB", "PTG"
                            currentTask.Text = "Remover a capacidade " & (x + 1) & " de " & capRemovalCount & "..."
                        Case "ITA"
                            currentTask.Text = "Rimozione capacità " & (x + 1) & " di " & capRemovalCount & "..."
                    End Select
                Case 1
                    currentTask.Text = "Removing capability " & (x + 1) & " of " & capRemovalCount & "..."
                Case 2
                    currentTask.Text = "Eliminando funcionalidad " & (x + 1) & " de " & capRemovalCount & "..."
                Case 3
                    currentTask.Text = "Suppression de la capacité " & (x + 1) & " de " & capRemovalCount & " en cours..."
                Case 4
                    currentTask.Text = "Remover a capacidade " & (x + 1) & " de " & capRemovalCount & "..."
                Case 5
                    currentTask.Text = "Rimozione capacità " & (x + 1) & " di " & capRemovalCount & "..."
            End Select
            DynaLog.LogMessage("Getting information about capability " & Quote & capRemovalIds(x) & Quote & "...")
            CurrentPB.Value = x + 1
            LogView.AppendText(CrLf &
                               "Capability " & (x + 1) & " of " & capRemovalCount)
            Try
                DynaLog.LogMessage("Initializing API...")
                DismApi.Initialize(DismLogLevel.LogErrors)
                DynaLog.LogMessage("Opening image session...")
                Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                    DynaLog.LogMessage("Getting capability information...")
                    Dim capInfo As DismCapabilityInfo = DismApi.GetCapabilityInfo(imgSession, capRemovalIds(x))
                    LogView.AppendText(CrLf & CrLf &
                                       "- Capability identity: " & capInfo.Name & CrLf &
                                       "- Capability name: " & capInfo.DisplayName & CrLf &
                                       "- Capability description: " & capInfo.Description & CrLf)
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
            LogView.AppendText(CrLf & "Getting error level...")
            errCode = Hex(Decimal.ToInt32(DismExitCode))
            If DismExitCode = 0 Then
                capSuccessfulRemovals += 1
            Else
                capFailedRemovals += 1
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(" Error level : 0x" & errCode)
            Else
                LogView.AppendText(" Error level : " & errCode)
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
        LogView.AppendText(CrLf & "Gathering error level for selected capabilities..." & CrLf)
        For x = 0 To FeatureErrorCodes.Count - 1
            LogView.AppendText(CrLf & "- Capability no. " & (x + 1) & ": " & FeatureErrorCodes(x))
        Next
        Thread.Sleep(2000)
        If capSuccessfulRemovals > 0 Then
            GetErrorCode(True)
        ElseIf capSuccessfulRemovals <= 0 Then
            GetErrorCode(False)
        End If
        If FeatureErrorCodes.Contains("BC2") Then
            DynaLog.LogMessage("A system restart is needed to fully remove some capabilities.")
            LogView.AppendText(CrLf & "Some capabilities require a system restart to be fully processed. Save your work, close your programs, and restart when ready")
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
        allTasks.Text = "Upgrading the image..."
        currentTask.Text = "Setting the new image edition..."
        LogView.AppendText(CrLf & "Setting the new image edition..." & CrLf &
                           "Options:" & CrLf &
                           "- New edition: " & imgEditionNewEdition & CrLf &
                           "- Will the EULA be copied? " & If(imgEditionCopyEula, "Yes, to the following destination: " & imgEditionEulaDestination, "No") & CrLf &
                           "- Will the EULA be accepted? " & If(imgEditionAcceptEula, "Yes, with the following product key: " & imgEditionEditionKey, "No") & CrLf)
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
        LogView.AppendText(CrLf & "Getting error level...")
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(" Error level : 0x" & errCode)
        Else
            LogView.AppendText(" Error level : " & errCode)
        End If
        GetErrorCode(False)
    End Sub

    Private Sub SetImageProductKey(targetImage As String)
        DynaLog.LogMessage("Preparing to set the product key...")
        DynaLog.LogMessage("- New Product Key: " & pkSetNewProductKey)
        allTasks.Text = "Setting the product key..."
        currentTask.Text = "Setting the new product key..."
        LogView.AppendText(CrLf & "Setting the new product key..." & CrLf &
                           "Options:" & CrLf &
                           "- New product key: " & pkSetNewProductKey & CrLf)
        CommandArgs &= " /image=" & targetImage & " /norestart /set-productkey=" & pkSetNewProductKey
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & "Getting error level...")
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(" Error level : 0x" & errCode)
        Else
            LogView.AppendText(" Error level : " & errCode)
        End If
        GetErrorCode(False)
    End Sub

#End Region

#Region "Driver Management Tasks"

    Private Sub AddDrivers(targetImage As String)
        DynaLog.LogMessage("Preparing to add OS drivers...")
        DynaLog.LogMessage("- Force installation of unsigned drivers? " & If(drvAdditionForceUnsigned, "Yes", "No"))
        DynaLog.LogMessage("- Save changes to the Windows image after finishing? " & If(drvAdditionCommit, "Yes", "No"))
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Adding drivers..."
                        currentTask.Text = "Preparing to add drivers..."
                    Case "ESN"
                        allTasks.Text = "Añadiendo controladores..."
                        currentTask.Text = "Preparándonos para añadir controladores..."
                    Case "FRA"
                        allTasks.Text = "Ajout des pilotes en cours..."
                        currentTask.Text = "Préparation de l'ajout des pilotes en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "A adicionar controladores..."
                        currentTask.Text = "A preparar para adicionar controladores..."
                    Case "ITA"
                        allTasks.Text = "Aggiunta driver..."
                        currentTask.Text = "Preparazione aggiunta driver..."
                End Select
            Case 1
                allTasks.Text = "Adding drivers..."
                currentTask.Text = "Preparing to add drivers..."
            Case 2
                allTasks.Text = "Añadiendo controladores..."
                currentTask.Text = "Preparándonos para añadir controladores..."
            Case 3
                allTasks.Text = "Ajout des pilotes en cours..."
                currentTask.Text = "Préparation de l'ajout des pilotes en cours..."
            Case 4
                allTasks.Text = "A adicionar controladores..."
                currentTask.Text = "A preparar para adicionar controladores..."
            Case 5
                allTasks.Text = "Aggiunta driver..."
                currentTask.Text = "Preparazione aggiunta driver..."
        End Select
        LogView.AppendText(CrLf & "Adding driver packages to mounted image..." & CrLf &
                           "Options:" & CrLf &
                           "- Force installation of unsigned drivers? " & If(drvAdditionForceUnsigned, "Yes", "No") & CrLf &
                           "- Commit image after adding driver packages? " & If(drvAdditionCommit, "Yes", "No") & CrLf)
        If drvAdditionForceUnsigned Then
            LogView.AppendText(CrLf &
                               "Warning: the option to force installation of unsigned drivers has been checked. Do note that unsigned drivers might cause instability on the resulting Windows image.")
        End If
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Adding drivers..."
                    Case "ESN"
                        currentTask.Text = "Añadiendo controladores..."
                    Case "FRA"
                        currentTask.Text = "Ajout des pilotes en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A adicionar controladores..."
                    Case "ITA"
                        currentTask.Text = "Aggiunta driver..."
                End Select
            Case 1
                currentTask.Text = "Adding drivers..."
            Case 2
                currentTask.Text = "Añadiendo controladores..."
            Case 3
                currentTask.Text = "Ajout des pilotes en cours..."
            Case 4
                currentTask.Text = "A adicionar controladores..."
            Case 5
                currentTask.Text = "Aggiunta driver..."
        End Select
        LogView.AppendText(CrLf & "Enumerating drivers to add. Please wait..." & CrLf &
                           "Total number of drivers: " & drvAdditionCount)
        CurrentPB.Maximum = drvAdditionCount
        For x = 0 To Array.LastIndexOf(drvAdditionPkgs, drvAdditionLastPkg)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            currentTask.Text = "Adding driver " & (x + 1) & " of " & drvAdditionCount & "..."
                        Case "ESN"
                            currentTask.Text = "Añadiendo controlador " & (x + 1) & " de " & drvAdditionCount & "..."
                        Case "FRA"
                            currentTask.Text = "Ajout du pilote " & (x + 1) & " de " & drvAdditionCount & " en cours..."
                        Case "PTB", "PTG"
                            currentTask.Text = "A adicionar o controlador " & (x + 1) & " de " & drvAdditionCount & "..."
                        Case "ITA"
                            currentTask.Text = "Aggiunta driver " & (x + 1) & " di " & drvAdditionCount & "..."
                    End Select
                Case 1
                    currentTask.Text = "Adding driver " & (x + 1) & " of " & drvAdditionCount & "..."
                Case 2
                    currentTask.Text = "Añadiendo controlador " & (x + 1) & " de " & drvAdditionCount & "..."
                Case 3
                    currentTask.Text = "Ajout du pilote " & (x + 1) & " de " & drvAdditionCount & " en cours..."
                Case 4
                    currentTask.Text = "A adicionar o controlador " & (x + 1) & " de " & drvAdditionCount & "..."
                Case 5
                    currentTask.Text = "Aggiunta driver " & (x + 1) & " di " & drvAdditionCount & "..."
            End Select
            CurrentPB.Value = x + 1
            LogView.AppendText(CrLf &
                               "Driver " & (x + 1) & " of " & drvAdditionCount)
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
                                                   "- Hardware description: " & drvInfo.HardwareDescription & CrLf &
                                                   "- Hardware ID: " & drvInfo.HardwareId & CrLf &
                                                   "- Additional IDs" & CrLf &
                                                   "  - Compatible IDs: " & drvInfo.CompatibleIds & CrLf &
                                                   "  - Excluded IDs: " & drvInfo.ExcludeIds & CrLf &
                                                   "- Hardware manufacturer: " & drvInfo.ManufacturerName & CrLf &
                                                   "- Hardware architecture: " & Casters.CastDismArchitecture(drvInfo.Architecture))
                            Next
                        ElseIf drvInfoCollection.Count > 10 Then
                            DynaLog.LogMessage("The driver information contains more than 10 hardware targets.")
                            LogView.AppendText(CrLf & CrLf &
                                               "This driver file targets more than 10 devices. To avoid creating log files large in size, we will not show information of this driver package, and will proceed anyway." & CrLf &
                                               "If you want to get information of this driver package, go to Commands > Drivers > Get driver information > I want to get information about driver files, and specify this driver file:" & CrLf & CrLf &
                                               "    " & Path.GetFileName(drvAdditionPkgs(x)))
                        Else
                            LogView.AppendText(CrLf & CrLf &
                                               "We couldn't get information of this driver package. Proceeding anyway...")
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
                                   "The driver package currently about to be processed is a folder, so information about it can't be obtained. Proceeding anyway...")
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
                    LogView.AppendText(CrLf & "This folder will be scanned recursively. Driver addition may take a longer time...")
                    CommandArgs &= " /recurse"
                End If
                RunProcess(DismProgram, CommandArgs)
            End If
            LogView.AppendText(CrLf & "Getting error level...")
            errCode = Hex(Decimal.ToInt32(DismExitCode))
            If DismExitCode = 0 Then
                drvSuccessfulAdditions += 1
            Else
                drvFailedAdditions += 1
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(" Error level : 0x" & errCode)
            Else
                LogView.AppendText(" Error level : " & errCode)
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
        LogView.AppendText(CrLf & "Gathering error level for selected drivers..." & CrLf)
        For x = 0 To PackageErrorCodes.Count - 1
            LogView.AppendText(CrLf & "- Driver no. " & (x + 1) & ": " & PackageErrorCodes(x))
        Next
        Thread.Sleep(2000)
        If drvAdditionCommit Then
            DynaLog.LogMessage("Preparing to save changes...")
            AllPB.Value = AllPB.Maximum / taskCount
            currentTCont += 1
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                        Case "ESN"
                            taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                        Case "FRA"
                            taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                        Case "PTB", "PTG"
                            taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                        Case "ITA"
                            taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
                    End Select
                Case 1
                    taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                Case 2
                    taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                Case 3
                    taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                Case 4
                    taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                Case 5
                    taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
            End Select
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Removing drivers..."
                        currentTask.Text = "Preparing to remove drivers..."
                    Case "ESN"
                        allTasks.Text = "Eliminando controladores..."
                        currentTask.Text = "Preparándonos para eliminar controladores..."
                    Case "FRA"
                        allTasks.Text = "Suppression des pilotes en cours..."
                        currentTask.Text = "Préparation de la suppression des pilotes en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "A remover controladores..."
                        currentTask.Text = "A preparar a remoção de controladores..."
                    Case "ITA"
                        allTasks.Text = "Rimozione driver..."
                        currentTask.Text = "Preparazione rimozione driver..."
                End Select
            Case 1
                allTasks.Text = "Removing drivers..."
                currentTask.Text = "Preparing to remove drivers..."
            Case 2
                allTasks.Text = "Eliminando controladores..."
                currentTask.Text = "Preparándonos para eliminar controladores..."
            Case 3
                allTasks.Text = "Suppression des pilotes en cours..."
                currentTask.Text = "Préparation de la suppression des pilotes en cours..."
            Case 4
                allTasks.Text = "A remover controladores..."
                currentTask.Text = "A preparar a remoção de controladores..."
            Case 5
                allTasks.Text = "Rimozione driver..."
                currentTask.Text = "Preparazione rimozione dei driver..."
        End Select
        LogView.AppendText(CrLf & "Removing driver packages from mounted image..." & CrLf)
        ' Get all driver packages
        DynaLog.LogMessage("Getting drivers of the Windows image... This can take some time, depending on the amount of drivers installed.")
        LogView.AppendText(CrLf & "Getting image drivers. This may take some time..." & CrLf)
        GetThirdPartyDrivers()
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Removing drivers..."
                    Case "ESN"
                        currentTask.Text = "Eliminando controladores..."
                    Case "FRA"
                        currentTask.Text = "Suppression des pilotes en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A remover controladores..."
                    Case "ITA"
                        currentTask.Text = "Rimozione driver..."
                End Select
            Case 1
                currentTask.Text = "Removing drivers..."
            Case 2
                currentTask.Text = "Eliminando controladores..."
            Case 3
                currentTask.Text = "Suppression des pilotes en cours..."
            Case 4
                currentTask.Text = "A remover controladores..."
            Case 5
                currentTask.Text = "Rimozione driver..."
        End Select
        LogView.AppendText(CrLf & "Enumerating drivers to remove. Please wait..." & CrLf &
                           "Total number of drivers: " & drvRemovalCount)
        CurrentPB.Maximum = drvRemovalCount
        For x = 0 To Array.LastIndexOf(drvRemovalPkgs, drvRemovalLastPkg)
            If x + 1 > CurrentPB.Maximum Then Exit For
            CommandArgs = BckArgs
            Dim driverRemovalPackage As String = drvRemovalPkgs(x)
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            currentTask.Text = "Removing driver " & (x + 1) & " of " & drvRemovalCount & "..."
                        Case "ESN"
                            currentTask.Text = "Eliminando controlador " & (x + 1) & " de " & drvRemovalCount & "..."
                        Case "FRA"
                            currentTask.Text = "Suppression du pilote " & (x + 1) & " de " & drvRemovalCount & " en cours..."
                        Case "PTB", "PTG"
                            currentTask.Text = "A remover o controlador " & (x + 1) & " de " & drvRemovalCount & "..."
                        Case "ITA"
                            currentTask.Text = "Rimozione driver " & (x + 1) & " di " & drvRemovalCount & "..."
                    End Select
                Case 1
                    currentTask.Text = "Removing driver " & (x + 1) & " of " & drvRemovalCount & "..."
                Case 2
                    currentTask.Text = "Eliminando controlador " & (x + 1) & " de " & drvRemovalCount & "..."
                Case 3
                    currentTask.Text = "Suppression du pilote " & (x + 1) & " de " & drvRemovalCount & " en cours..."
                Case 4
                    currentTask.Text = "A remover o controlador " & (x + 1) & " de " & drvRemovalCount & "..."
                Case 5
                    currentTask.Text = "Rimozione driver " & (x + 1) & " di " & drvRemovalCount & "..."
            End Select
            DynaLog.LogMessage("Getting information about driver file " & Quote & Path.GetFileName(driverRemovalPackage) & Quote & "...")
            CurrentPB.Value = x + 1
            LogView.AppendText(CrLf &
                               "Driver " & (x + 1) & " of " & drvRemovalCount)
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
            LogView.AppendText(CrLf & "Getting error level...")
            errCode = Hex(Decimal.ToInt32(DismExitCode))
            If DismExitCode = 0 Then
                drvSuccessfulRemovals += 1
            Else
                drvFailedRemovals += 1
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(" Error level : 0x" & errCode)
            Else
                LogView.AppendText(" Error level : " & errCode)
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
        LogView.AppendText(CrLf & "Gathering error level for selected drivers..." & CrLf)
        For x = 0 To PackageErrorCodes.Count - 1
            LogView.AppendText(CrLf & "- Driver no. " & (x + 1) & ": " & PackageErrorCodes(x))
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Exporting drivers..."
                        currentTask.Text = "Exporting third-party drivers to the specified folder..."
                    Case "ESN"
                        allTasks.Text = "Exportando controladores..."
                        currentTask.Text = "Exportando controladores de terceros a la carpeta especificada..."
                    Case "FRA"
                        allTasks.Text = "Exportation des pilotes en cours..."
                        currentTask.Text = "Exportation de pilotes tiers dans le dossier spécifié en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Exportar controladores..."
                        currentTask.Text = "Exportar controladores de terceiros para a pasta especificada..."
                    Case "ITA"
                        allTasks.Text = "Esportazione driver..."
                        currentTask.Text = "Esportazione driver terze parti nella cartella specificata..."
                End Select
            Case 1
                allTasks.Text = "Exporting drivers..."
                currentTask.Text = "Exporting third-party drivers to the specified folder..."
            Case 2
                allTasks.Text = "Exportando controladores..."
                currentTask.Text = "Exportando controladores de terceros a la carpeta especificada..."
            Case 3
                allTasks.Text = "Exportation des pilotes en cours..."
                currentTask.Text = "Exportation de pilotes tiers dans le dossier spécifié en cours..."
            Case 4
                allTasks.Text = "Exportar controladores..."
                currentTask.Text = "Exportar controladores de terceiros para a pasta especificada..."
            Case 5
                allTasks.Text = "Esportazione driver..."
                currentTask.Text = "Esportazione driver terze parti nella cartella specificata..."
        End Select
        LogView.AppendText(CrLf & "Exporting drivers to specified folder..." & CrLf &
                           "- Export target: " & Quote & drvExportTarget & Quote & CrLf &
                           "- Export all drivers, or just those with matching class names? " & If(drvExportAllDrvs, "All Drivers", "Drivers with matching class name") & CrLf &
                           "- If not all drivers are exported, which class name is used for drivers that will be exported? " & drvExportSpecificClassName & CrLf)
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
                    LogView.AppendText(CrLf & driversToExport.Count & " driver(s) will be exported to the destination")
                    For Each driverToExport In driversToExport
                        LogView.AppendText(CrLf & "Exporting driver file " & Path.GetFileName(driverToExport.DriverOriginalFileName) & "...")
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
                    LogView.AppendText(CrLf & driversToExport.Count & " driver(s) will be exported to the destination")
                    For Each driverToExport In driversToExport
                        LogView.AppendText(CrLf & "Exporting driver file " & Path.GetFileName(driverToExport.DriverOriginalFileName) & "...")
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
                    LogView.AppendText(CrLf & "Getting image drivers...")
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    Using session As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MountDir))
                        DynaLog.LogMessage("Getting drivers with DISMAPI...")
                        Dim driverPackages As DismDriverPackageCollection = DismApi.GetDrivers(session, False)
                        If driverPackages Is Nothing Then Exit Try
                        DynaLog.LogMessage("Filtering driver collection based on class name...")
                        Dim driversToExport As IEnumerable(Of DismDriverPackage) = driverPackages.Where(Function(driver) driver.ClassName.Equals(drvExportSpecificClassName, StringComparison.OrdinalIgnoreCase))
                        If driversToExport Is Nothing Then Exit Try

                        DynaLog.LogMessage("Amount of drivers to export: " & driversToExport.Count)
                        LogView.AppendText(CrLf & driversToExport.Count & " driver(s) will be exported to the destination")
                        For Each driverToExport In driversToExport
                            LogView.AppendText(CrLf & "Exporting driver file " & Path.GetFileName(driverToExport.OriginalFileName) & "...")
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
        LogView.AppendText(CrLf & "Getting error level...")
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(" Error level : 0x" & errCode)
        Else
            LogView.AppendText(" Error level : " & errCode)
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Importing drivers..."
                        currentTask.Text = "Preparing to import third-party drivers..."
                    Case "ESN"
                        allTasks.Text = "Importando controladores..."
                        currentTask.Text = "Preparándonos para importar controladores de terceros..."
                    Case "FRA"
                        allTasks.Text = "Importation des pilotes en cours..."
                        currentTask.Text = "Préparation de l'importation de pilotes tiers en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "A importar controladores..."
                        currentTask.Text = "A preparar a importação de controladores de terceiros..."
                    Case "ITA"
                        allTasks.Text = "Importazione driver..."
                        currentTask.Text = "Preparazione importazione driver terze parti..."
                End Select
            Case 1
                allTasks.Text = "Importing drivers..."
                currentTask.Text = "Preparing to import third-party drivers..."
            Case 2
                allTasks.Text = "Importando controladores..."
                currentTask.Text = "Preparándonos para importar controladores de terceros..."
            Case 3
                allTasks.Text = "Importation des pilotes en cours..."
                currentTask.Text = "Préparation de l'importation de pilotes tiers en cours..."
            Case 4
                allTasks.Text = "A importar controladores..."
                currentTask.Text = "A preparar a importação de controladores de terceiros..."
            Case 5
                allTasks.Text = "Importazione dei driver..."
                currentTask.Text = "Preparazione all'importazione di driver di terze parti..."
        End Select
        LogView.AppendText(CrLf & "Importing third party drivers..." & CrLf)
        Select Case ImportSourceInt
            Case 0
                LogView.AppendText("- Driver import source: Windows image (" & Quote & DrvImport_SourceImage & Quote & ")" & CrLf)
            Case 1
                LogView.AppendText("- Driver import source: active installation" & CrLf)
            Case 2
                LogView.AppendText("- Driver import source: offline installation (" & Quote & DrvImport_SourceDisk & Quote & ")" & CrLf)
        End Select
        Thread.Sleep(500)
        LogView.AppendText(CrLf & "Creating temporary folder for driver exports..." & CrLf)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Exporting third-party drivers from driver import source..."
                    Case "ESN"
                        currentTask.Text = "Exportando controladores de terceros del origen de importación de controladores..."
                    Case "FRA"
                        currentTask.Text = "Exportation de pilotes tiers à partir de la source d'importation des pilotes en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "Exportar controladores de terceiros a partir da fonte de importação de controladores..."
                    Case "ITA"
                        currentTask.Text = "Esportazione driver terze parti dalla sorgente importazione driver..."
                End Select
            Case 1
                currentTask.Text = "Exporting third-party drivers from driver import source..."
            Case 2
                currentTask.Text = "Exportando controladores de terceros del origen de importación de controladores..."
            Case 3
                currentTask.Text = "Exportation de pilotes tiers à partir de la source d'importation des pilotes en cours..."
            Case 4
                currentTask.Text = "Exportar controladores de terceiros a partir da fonte de importação de controladores..."
            Case 5
                currentTask.Text = "Esportazione di driver di terze parti dall'origine di importazione dei driver..."
        End Select
        Try
            DynaLog.LogMessage("Creating directory where drivers will be exported to...")
            Directory.CreateDirectory(Application.StartupPath & "\export_temp")
        Catch ex As Exception
            DynaLog.LogMessage("Could not create the driver export directory. Error message: " & ex.Message)
            LogView.AppendText(CrLf & "The temporary folder could not be created. See below for reasons why:" & CrLf & CrLf & ex.ToString() & "-" & ex.Message)
        End Try
        If Directory.Exists(Application.StartupPath & "\export_temp") Then
            DynaLog.LogMessage("Exporting drivers...")
            LogView.AppendText(CrLf & "Exporting third-party drivers from import source..." & CrLf)
            Dim importSource As String = ""
            Select Case ImportSourceInt
                Case 0
                    importSource = If(Path.GetPathRoot(DrvImport_SourceImage) = DrvImport_SourceImage, DrvImport_SourceImage, Quote & DrvImport_SourceImage & Quote)
                Case 2
                    importSource = If(Path.GetPathRoot(DrvImport_SourceDisk) = DrvImport_SourceDisk, DrvImport_SourceDisk, Quote & DrvImport_SourceDisk & Quote)
            End Select
            CommandArgs &= If(ImportSourceInt = 1, " /online", " /image=" & importSource) & " /export-driver /destination=" & Quote & Application.StartupPath & "\export_temp" & Quote
            RunProcess(DismProgram, CommandArgs)
            LogView.AppendText(CrLf & "Getting error level...")
            If Hex(DismExitCode).Length < 8 Then
                errCode = DismExitCode
            Else
                errCode = Hex(DismExitCode)
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(" Error level : 0x" & errCode)
            Else
                LogView.AppendText(" Error level : " & errCode)
            End If
            If DismExitCode = 0 Then
                DynaLog.LogMessage("The previous operation succeeded. Adding the drivers...")
                CurrentPB.Value = CurrentPB.Maximum / 2
                AllPB.Value = AllPB.Maximum / 2
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                currentTask.Text = "Importing third-party drivers to destination image..."
                            Case "ESN"
                                currentTask.Text = "Importando controladores de terceros a la imagen de destino..."
                            Case "FRA"
                                currentTask.Text = "Importation des pilotes tiers dans l'image de destination en cours..."
                            Case "PTB", "PTG"
                                currentTask.Text = "A importar controladores de terceiros para a imagem de destino..."
                            Case "ITA"
                                currentTask.Text = "Importazione driver terze parti nell'immagine destinazione..."
                        End Select
                    Case 1
                        currentTask.Text = "Importing third-party drivers to destination image..."
                    Case 2
                        currentTask.Text = "Importando controladores de terceros a la imagen de destino..."
                    Case 3
                        currentTask.Text = "Importation des pilotes tiers dans l'image de destination en cours..."
                    Case 4
                        currentTask.Text = "A importar controladores de terceiros para a imagem de destino..."
                    Case 5
                        currentTask.Text = "Importazione driver di terze parti nell'immagine destinazione..."
                End Select
                LogView.AppendText(CrLf & "Importing third-party drivers from the temporary export directory to the destination image...")
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
                    LogView.AppendText(" Error level : 0x" & errCode)
                Else
                    LogView.AppendText(" Error level : " & errCode)
                End If
                GetErrorCode(False)
            End If
            LogView.AppendText(CrLf & "Deleting temporary export directory...")
            Try
                DynaLog.LogMessage("Attempting to delete the driver export directory...")
                Directory.Delete(Application.StartupPath & "\export_temp", True)
            Catch ex As Exception
                DynaLog.LogMessage("Could not delete driver export directory. Error message: " & ex.Message)
                LogView.AppendText(CrLf & "We couldn't delete the temporary export directory. You'll need to delete the " & Quote & "export_temp" & Quote & " directory manually.")
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
                                           "- Published name: " & drv.PublishedName & CrLf &
                                           "- Provider name: " & drv.ProviderName & CrLf &
                                           "- Class name: " & drv.ClassName & CrLf &
                                           "- Class description: " & drv.ClassDescription & CrLf &
                                           "- Class GUID: " & drv.ClassGuid & CrLf &
                                           "- Version and date: " & drv.Version.ToString() & " / " & drv.Date.ToString() & CrLf &
                                           "- Is part of the Windows distribution? " & If(drv.InBox, "Yes", "No") & CrLf &
                                           "- Is critical to the boot process? " & If(drv.BootCritical, "Yes", "No"))
                        If drv.InBox Then
                            DynaLog.LogMessage("This driver is part of the Windows distribution.")
                            LogView.AppendText(CrLf & CrLf &
                                               "Warning: this driver package is part of the Windows distribution. Some areas may no longer work after this driver has been removed")
                        End If
                        If drv.BootCritical Then
                            DynaLog.LogMessage("This driver is critical to the boot process of the Windows image.")
                            LogView.AppendText(CrLf & CrLf &
                                               "Warning: this driver package is critical to the boot process. The target image may no longer boot or work correctly after this driver has been removed")
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Applying unattended answer file..."
                        currentTask.Text = "Applying specified unattended answer file to the target image..."
                    Case "ESN"
                        allTasks.Text = "Aplicando archivo de respuesta desatendida..."
                        currentTask.Text = "Aplicando archivo de respuesta desatendida especificado a la imagen de destino..."
                    Case "FRA"
                        allTasks.Text = "Appliquer le fichier de réponse sans surveillance en cours..."
                        currentTask.Text = "Appliquer le fichier de réponse non assisté spécifié à l'image cible en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Aplicar ficheiro de resposta não assistido..."
                        currentTask.Text = "Aplicar o ficheiro de resposta automática especificado à imagem de destino..."
                    Case "ITA"
                        allTasks.Text = "Applicazione file risposta non presidiate..."
                        currentTask.Text = "Applicazione file risposta non presidiate specificato all'immagine destinazione..."
                End Select
            Case 1
                allTasks.Text = "Applying unattended answer file..."
                currentTask.Text = "Applying specified unattended answer file to the target image..."
            Case 2
                allTasks.Text = "Aplicando archivo de respuesta desatendida..."
                currentTask.Text = "Aplicando archivo de respuesta desatendida especificado a la imagen de destino..."
            Case 3
                allTasks.Text = "Appliquer le fichier de réponse sans surveillance en cours..."
                currentTask.Text = "Appliquer le fichier de réponse non assisté spécifié à l'image cible en cours..."
            Case 4
                allTasks.Text = "Aplicar ficheiro de resposta não assistido..."
                currentTask.Text = "Aplicar o ficheiro de resposta automática especificado à imagem de destino..."
            Case 5
                allTasks.Text = "Applicazione del file di risposta non presidiato..."
                currentTask.Text = "Applicazione file risposta non presidiata specificato all'immagine destinazione..."
        End Select
        LogView.AppendText(CrLf & "Applying unattended answer file. Options:" & CrLf &
                           "- Unattended answer file: " & UnattendedFile)
        Try
            LogView.AppendText(CrLf & CrLf & "Creating directories and copying files...")
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
            LogView.AppendText(CrLf & "The unattended answer file has been successfully copied.")
            GetErrorCode(True)
        Catch ex As Exception
            DynaLog.LogMessage("Could not copy unattended answer file to targets. Error message: " & ex.Message)
            CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /apply-unattend=" & Quote & UnattendedFile & Quote
            RunProcess(DismProgram, CommandArgs)
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            currentTask.Text = "Gathering error level..."
                        Case "ESN"
                            currentTask.Text = "Recopilando nivel de error..."
                        Case "FRA"
                            currentTask.Text = "Recueil du niveau d'erreur en cours..."
                        Case "PTB", "PTG"
                            currentTask.Text = "A recolher o nível de erro..."
                        Case "ITA"
                            currentTask.Text = "Raccolta livello errore..."
                    End Select
                Case 1
                    currentTask.Text = "Gathering error level..."
                Case 2
                    currentTask.Text = "Recopilando nivel de error..."
                Case 3
                    currentTask.Text = "Recueil du niveau d'erreur en cours..."
                Case 4
                    currentTask.Text = "A recolher o nível de erro..."
                Case 5
                    currentTask.Text = "Raccolta livello errore..."
            End Select
            LogView.AppendText(CrLf & "Gathering error level...")
            GetErrorCode(False)
            If errCode.Length >= 8 Then
                LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
            Else
                LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
            End If
        End Try
    End Sub

#End Region

#Region "Windows PE Management Tasks"

    Private Sub SetTargetPath(targetImage As String)
        DynaLog.LogMessage("Preparing to set the target path of the Windows PE image...")
        DynaLog.LogMessage("Target path to set: " & Quote & peNewTargetPath & Quote)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Setting the target path..."
                        currentTask.Text = "Setting the Windows PE target path..."
                    Case "ESN"
                        allTasks.Text = "Estableciendo la ruta de destino..."
                        currentTask.Text = "Estableciendo la ruta de destino de Windows PE..."
                    Case "FRA"
                        allTasks.Text = "Configuration du chemin cible en cours..."
                        currentTask.Text = "Configuration du chemin cible de Windows PE en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "A configurar a localização de destino..."
                        currentTask.Text = "A configurar a localização de destino do Windows PE..."
                    Case "ITA"
                        allTasks.Text = "Impostazione percorso destinazione..."
                        currentTask.Text = "Impostazione percorso destinazione Windows PE..."
                End Select
            Case 1
                allTasks.Text = "Setting the target path..."
                currentTask.Text = "Setting the Windows PE target path..."
            Case 2
                allTasks.Text = "Estableciendo la ruta de destino..."
                currentTask.Text = "Estableciendo la ruta de destino de Windows PE..."
            Case 3
                allTasks.Text = "Configuration du chemin cible en cours..."
                currentTask.Text = "Configuration du chemin cible de Windows PE en cours..."
            Case 4
                allTasks.Text = "A configurar a localização de destino..."
                currentTask.Text = "A configurar a localização de destino do Windows PE..."
            Case 5
                allTasks.Text = "Impostazione percorso destinazione..."
                currentTask.Text = "Impostazione percorso destinazione di Windows PE..."
        End Select
        LogView.AppendText(CrLf & "Setting the Windows PE target path..." & CrLf &
                           "- New target path: " & Quote & peNewTargetPath & Quote)
        CommandArgs &= " /image=" & targetImage & " /set-targetpath=" & peNewTargetPath
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & "Getting error level...")
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(" Error level : 0x" & errCode)
        Else
            LogView.AppendText(" Error level : " & errCode)
        End If
        GetErrorCode(False)
    End Sub

    Private Sub SetScratchSpace(targetImage As String)
        DynaLog.LogMessage("Preparing to set the scratch space of the Windows PE image...")
        DynaLog.LogMessage("Scratch space to set: " & peNewScratchSpace & " MB")
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Setting the scratch space..."
                        currentTask.Text = "Setting the Windows PE scratch space..."
                    Case "ESN"
                        allTasks.Text = "Estableciendo el espacio temporal..."
                        currentTask.Text = "Estableciendo el espacio temporal de Windows PE..."
                    Case "FRA"
                        allTasks.Text = "Configuration de l'espace temporaire en cours..."
                        currentTask.Text = "Configuration de l'espace temporaire de Windows PE en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "A configurar o espaço temporário..."
                        currentTask.Text = "A configurar o espaço temporário do Windows PE..."
                    Case "ITA"
                        allTasks.Text = "Impostazione spazio temporaneo..."
                        currentTask.Text = "Impostazione spazio temporaneo Windows PE..."
                End Select
            Case 1
                allTasks.Text = "Setting the scratch space..."
                currentTask.Text = "Setting the Windows PE scratch space..."
            Case 2
                allTasks.Text = "Estableciendo el espacio temporal..."
                currentTask.Text = "Estableciendo el espacio temporal de Windows PE..."
            Case 3
                allTasks.Text = "Configuration de l'espace temporaire en cours..."
                currentTask.Text = "Configuration de l'espace temporaire de Windows PE en cours..."
            Case 4
                allTasks.Text = "A configurar o espaço temporário..."
                currentTask.Text = "A configurar o espaço temporário do Windows PE..."
            Case 5
                allTasks.Text = "Impostazione dello spazio temporaneo..."
                currentTask.Text = "Impostazione dello spazio temporaneo di Windows PE..."
        End Select
        LogView.AppendText(CrLf & "Setting the Windows PE scratch space..." & CrLf &
                           "- New scratch space amount: " & peNewScratchSpace & " MB")
        CommandArgs &= " /image=" & targetImage & " /set-scratchspace=" & peNewScratchSpace
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & "Getting error level...")
        If Hex(DismExitCode).Length < 8 Then
            errCode = DismExitCode
        Else
            errCode = Hex(DismExitCode)
        End If
        If errCode.Length >= 8 Then
            LogView.AppendText(" Error level : 0x" & errCode)
        Else
            LogView.AppendText(" Error level : " & errCode)
        End If
        GetErrorCode(False)
    End Sub

#End Region

#Region "OS Uninstall Management Tasks"

    Private Sub SetOSUnistallWindow()
        DynaLog.LogMessage("Preparing to set the OS rollback window...")
        DynaLog.LogMessage("New window: " & osUninstDayCount & " day(s)")
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Setting the uninstall window..."
                        currentTask.Text = "Setting the amount of days in which an uninstall can happen..."
                    Case "ESN"
                        allTasks.Text = "Estableciendo el margen de desinstalación..."
                        currentTask.Text = "Estableciendo el número de días en los que puede ocurrir una desinstalación..."
                    Case "FRA"
                        allTasks.Text = "Définition de la créneau de désinstallation en cours..."
                        currentTask.Text = "Définition du nombre de jours au cours desquels une désinstallation peut avoir lieu en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "A configurar a janela de desinstalação..."
                        currentTask.Text = "A configurar o número de dias em que uma desinstalação pode ocorrer..."
                    Case "ITA"
                        allTasks.Text = "Impostazione finestra disinstallazione..."
                        currentTask.Text = "Impostazione numero di giorni in cui può avvenire la disinstallazione..."
                End Select
            Case 1
                allTasks.Text = "Setting the uninstall window..."
                currentTask.Text = "Setting the amount of days in which an uninstall can happen..."
            Case 2
                allTasks.Text = "Estableciendo el margen de desinstalación..."
                currentTask.Text = "Estableciendo el número de días en los que puede ocurrir una desinstalación..."
            Case 3
                allTasks.Text = "Définition de la créneau de désinstallation en cours..."
                currentTask.Text = "Définition du nombre de jours au cours desquels une désinstallation peut avoir lieu en cours..."
            Case 4
                allTasks.Text = "A configurar a janela de desinstalação..."
                currentTask.Text = "A configurar o número de dias em que uma desinstalação pode ocorrer..."
            Case 5
                allTasks.Text = "Impostazione della finestra di disinstallazione..."
                currentTask.Text = "Impostazione del numero di giorni in cui può avvenire la disinstallazione..."
        End Select
        LogView.AppendText(CrLf & "Setting the amount of days an uninstall can happen..." & CrLf &
                           "Number of days: " & osUninstDayCount)
        CommandArgs &= " /online /set-osuninstallwindow /value:" & osUninstDayCount
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        End If
    End Sub

    Private Sub RemoveOSUnistall()
        DynaLog.LogMessage("Preparing to remove the OS rollback...")
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Removing OS rollback ability..."
                        currentTask.Text = "Removing the ability to revert to an old installation of Windows..."
                    Case "ESN"
                        allTasks.Text = "Eliminando la habilidad de desinstalación..."
                        currentTask.Text = "Eliminando la habilidad para revertir a una instalación anterior de Windows..."
                    Case "FRA"
                        allTasks.Text = "Suppression de la possibilité de retour en arrière du système d'exploitation en cours..."
                        currentTask.Text = "Suppression de la possibilité de revenir à une ancienne installation de Windows en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Remover a capacidade de reversão do SO..."
                        currentTask.Text = "Remover a capacidade de reverter para uma instalação antiga do Windows..."
                    Case "ITA"
                        allTasks.Text = "Rimozione possibilità rollback sistema operativo..."
                        currentTask.Text = "Rimozione possibilità tornare alla vecchia installazione di Windows..."
                End Select
            Case 1
                allTasks.Text = "Removing OS rollback ability..."
                currentTask.Text = "Removing the ability to revert to an old installation of Windows..."
            Case 2
                allTasks.Text = "Eliminando la habilidad de desinstalación..."
                currentTask.Text = "Eliminando la habilidad para revertir a una instalación anterior de Windows..."
            Case 3
                allTasks.Text = "Suppression de la possibilité de retour en arrière du système d'exploitation en cours..."
                currentTask.Text = "Suppression de la possibilité de revenir à une ancienne installation de Windows en cours..."
            Case 4
                allTasks.Text = "Remover a capacidade de reversão do SO..."
                currentTask.Text = "Remover a capacidade de reverter para uma instalação antiga do Windows..."
            Case 5
                allTasks.Text = "Rimozione opzione fallback al sistema operativo precedente..."
                currentTask.Text = "Rimozione opzione fallback ad una vecchia installazione di Windows..."
        End Select
        LogView.AppendText(CrLf & "Removing the ability to revert to an old installation of Windows...")
        CommandArgs &= " /online /remove-osuninstall"
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        End If
    End Sub

    Private Sub InitiateOSUnistall()
        DynaLog.LogMessage("Preparing to initiate the OS rollback...")
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Uninstalling this version of Windows..."
                        currentTask.Text = "Preparing operating system rollback..."
                    Case "ESN"
                        allTasks.Text = "Desinstalando esta versión de Windows..."
                        currentTask.Text = "Preparando la desinstalación del sistema operativo..."
                    Case "FRA"
                        allTasks.Text = "Désinstallation de cette version de Windows en cours..."
                        currentTask.Text = "Préparation du retour en arrière du système d'exploitation en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Desinstalar esta versão do Windows..."
                        currentTask.Text = "Preparar a reversão do sistema operativo..."
                    Case "ITA"
                        allTasks.Text = "Disinstallazione di questa versione di Windows..."
                        currentTask.Text = "Preparazione rollback sistema operativo..."
                End Select
            Case 1
                allTasks.Text = "Uninstalling this version of Windows..."
                currentTask.Text = "Preparing operating system rollback..."
            Case 2
                allTasks.Text = "Desinstalando esta versión de Windows..."
                currentTask.Text = "Preparando la desinstalación del sistema operativo..."
            Case 3
                allTasks.Text = "Désinstallation de cette version de Windows en cours..."
                currentTask.Text = "Préparation du retour en arrière du système d'exploitation en cours..."
            Case 4
                allTasks.Text = "Desinstalar esta versão do Windows..."
                currentTask.Text = "Preparar a reversão do sistema operativo..."
            Case 5
                allTasks.Text = "Disinstallazione di questa versione di Windows..."
                currentTask.Text = "Preparazione del ripristino del sistema operativo..."
        End Select
        LogView.AppendText(CrLf & "Preparing operating system rollback...")
        CommandArgs = " /online /norestart /initiate-osuninstall"
        RunProcess(DismProgram, CommandArgs)
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Converting image..."
                        currentTask.Text = "Converting specified image..."
                    Case "ESN"
                        allTasks.Text = "Convirtiendo imagen..."
                        currentTask.Text = "Convirtiendo imagen especificada"
                    Case "FRA"
                        allTasks.Text = "Conversion de l'image en cours..."
                        currentTask.Text = "Conversion de l'image spécifiée en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "A converter imagem..."
                        currentTask.Text = "A converter a imagem especificada..."
                    Case "ITA"
                        allTasks.Text = "Conversione immagine..."
                        currentTask.Text = "Conversione immagine specificata..."
                End Select
            Case 1
                allTasks.Text = "Converting image..."
                currentTask.Text = "Converting specified image..."
            Case 2
                allTasks.Text = "Convirtiendo imagen..."
                currentTask.Text = "Convirtiendo imagen especificada"
            Case 3
                allTasks.Text = "Conversion de l'image en cours..."
                currentTask.Text = "Conversion de l'image spécifiée en cours..."
            Case 4
                allTasks.Text = "A converter imagem..."
                currentTask.Text = "A converter a imagem especificada..."
            Case 5
                allTasks.Text = "Conversione immagine..."
                currentTask.Text = "Conversione dell'immagine specificata..."
        End Select
        LogView.AppendText(CrLf & "Converting image..." & CrLf &
                           "Options:" & CrLf)

        ' Gather options
        LogView.AppendText("- Source image file: " & imgSrcFile & CrLf &
                           "- Index to convert: " & imgConversionIndex & CrLf &
                           "- Destination image file: " & imgDestFile & CrLf)
        If imgConversionMode = 0 Then
            LogView.AppendText("- Image conversion mode: Windows Imaging (WIM) --> Electronic Software Distribution (ESD)")
        ElseIf imgConversionMode = 1 Then
            LogView.AppendText("- Image conversion mode: Electronic Software Distribution (ESD) --> Windows Imaging (WIM)")
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta del livello di errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        End If
    End Sub

    Private Sub MergeSWM()
        DynaLog.LogMessage("Preparing to merge SWM files...")
        DynaLog.LogMessage("- Source image file: " & Quote & imgSwmSource & Quote)
        DynaLog.LogMessage("- Source image index: " & imgMergerIndex)
        DynaLog.LogMessage("- Destination image file: " & Quote & imgWimDestination & Quote)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Merging SWM files..."
                        currentTask.Text = "Merging SWM files into a WIM file..."
                    Case "ESN"
                        allTasks.Text = "Combinando archivos SWM..."
                        currentTask.Text = "Combinando archivos SWM en un archivo WIM..."
                    Case "FRA"
                        allTasks.Text = "Fusion des fichiers SWM en cours..."
                        currentTask.Text = "Fusion des fichiers SWM dans un fichier WIM en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Combinando ficheiros SWM..."
                        currentTask.Text = "Combinar ficheiros SWM num ficheiro WIM..."
                    Case "ITA"
                        allTasks.Text = "Unione file SWM..."
                        currentTask.Text = "Unione file SWM in un file WIM..."
                End Select
            Case 1
                allTasks.Text = "Merging SWM files..."
                currentTask.Text = "Merging SWM files into a WIM file..."
            Case 2
                allTasks.Text = "Combinando archivos SWM..."
                currentTask.Text = "Combinando archivos SWM en un archivo WIM..."
            Case 3
                allTasks.Text = "Fusion des fichiers SWM en cours..."
                currentTask.Text = "Fusion des fichiers SWM dans un fichier WIM en cours..."
            Case 4
                allTasks.Text = "Combinando ficheiros SWM..."
                currentTask.Text = "Combinar ficheiros SWM num ficheiro WIM..."
            Case 5
                allTasks.Text = "Unione dei file SWM..."
                currentTask.Text = "Unione dei file SWM in un file WIM..."
        End Select
        LogView.AppendText(CrLf & "Merging SWM files into a WIM file..." & CrLf &
                           "Options:" & CrLf)
        ' Gather options
        LogView.AppendText("- Source image file: " & imgSwmSource & CrLf &
                           "- Target index: " & imgMergerIndex & CrLf &
                           "- Destination image file: " & imgWimDestination & CrLf)

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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta livello errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        End If
    End Sub

    Private Sub SwitchIndexes()
        DynaLog.LogMessage("Preparing to switch image indexes...")
        DynaLog.LogMessage("- Source image file: " & Quote & SwitchSourceImg & Quote)
        DynaLog.LogMessage("- Source image index: " & SwitchSourceIndex)
        DynaLog.LogMessage("- Target image index: " & SwitchTargetIndex)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        allTasks.Text = "Switching image indexes..."
                        currentTask.Text = "Unmounting source index..."
                    Case "ESN"
                        allTasks.Text = "Cambiando índices de imagen..."
                        currentTask.Text = "Desmontando índice de origen..."
                    Case "FRA"
                        allTasks.Text = "Changement d'index de l'image en cours..."
                        currentTask.Text = "Démontage de l'index original en cours..."
                    Case "PTB", "PTG"
                        allTasks.Text = "Alternar índices de imagem..."
                        currentTask.Text = "Desmontar índice de origem..."
                    Case "ITA"
                        allTasks.Text = "Modifica indici immagine..."
                        currentTask.Text = "Smontaggio indice sorgente..."
                End Select
            Case 1
                allTasks.Text = "Switching image indexes..."
                currentTask.Text = "Unmounting source index..."
            Case 2
                allTasks.Text = "Cambiando índices de imagen..."
                currentTask.Text = "Desmontando índice de origen..."
            Case 3
                allTasks.Text = "Changement d'index de l'image en cours..."
                currentTask.Text = "Démontage de l'index original en cours..."
            Case 4
                allTasks.Text = "Alternar índices de imagem..."
                currentTask.Text = "Desmontar índice de origem..."
            Case 5
                allTasks.Text = "Modifica indici immagine..."
                currentTask.Text = "Smontaggio indice sorgente..."
        End Select
        LogView.AppendText(CrLf & "Switching image indexes..." & CrLf &
                           "Options:" & CrLf)
        ' Gather options
        LogView.AppendText("- Target mount directory: " & SwitchTarget & CrLf &
                           "- Source image index: " & SwitchSourceIndex & CrLf &
                           "- Target image index: " & SwitchTargetIndex & " (" & SwitchTargetIndexName & ")")
        If SwitchCommitSourceIndex Then
            LogView.AppendText(CrLf & "- Commit source index? Yes")
        Else
            LogView.AppendText(CrLf & "- Commit source index? No")
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta del livello di errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        End If
        If Decimal.ToInt32(DismExitCode) <> 0 Then
            DynaLog.LogMessage("Could not save changes to the image. Unmounting image whilst discarding changes...")
            LogView.AppendText(CrLf & CrLf & "Could not commit changes to the image. Discarding changes...")
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            currentTask.Text = "Unmounting source index..."
                        Case "ESN"
                            currentTask.Text = "Desmontando índice de origen..."
                        Case "FRA"
                            currentTask.Text = "Démontage de l'index original en cours..."
                        Case "PTB", "PTG"
                            currentTask.Text = "Desmontar índice de origem..."
                        Case "ITA"
                            currentTask.Text = "Smontaggio indice sorgente..."
                    End Select
                Case 1
                    currentTask.Text = "Unmounting source index..."
                Case 2
                    currentTask.Text = "Desmontando índice de origen..."
                Case 3
                    currentTask.Text = "Démontage de l'index original en cours..."
                Case 4
                    currentTask.Text = "Desmontar índice de origem..."
                Case 5
                    currentTask.Text = "Smontaggio indice sorgente..."
            End Select
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
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            currentTask.Text = "Gathering error level..."
                        Case "ESN"
                            currentTask.Text = "Recopilando nivel de error..."
                        Case "FRA"
                            currentTask.Text = "Recueil du niveau d'erreur en cours..."
                        Case "PTB", "PTG"
                            currentTask.Text = "A recolher o nível de erro..."
                        Case "ITA"
                            currentTask.Text = "Raccolta livello errore..."
                    End Select
                Case 1
                    currentTask.Text = "Gathering error level..."
                Case 2
                    currentTask.Text = "Recopilando nivel de error..."
                Case 3
                    currentTask.Text = "Recueil du niveau d'erreur en cours..."
                Case 4
                    currentTask.Text = "A recolher o nível de erro..."
                Case 5
                    currentTask.Text = "Raccolta livello errore..."
            End Select
            LogView.AppendText(CrLf & "Gathering error level...")
            GetErrorCode(False)
            If errCode.Length >= 8 Then
                LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
            Else
                LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
            End If
            If Decimal.ToInt32(DismExitCode) <> 0 Then
                DynaLog.LogMessage("Could not unmount the image.")
                Return
            End If
        End If
        AllPB.Value = AllPB.Maximum / taskCount
        currentTCont += 1
        DynaLog.LogMessage("Mounting Windows image...")
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                        currentTask.Text = "Mounting target index..."
                    Case "ESN"
                        taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                        currentTask.Text = "Montando índice de destino..."
                    Case "FRA"
                        taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                        currentTask.Text = "Montage de l'index de ciblage en cours..."
                    Case "PTB", "PTG"
                        taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                        currentTask.Text = "A montar o índice de destino..."
                    Case "ITA"
                        taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
                        currentTask.Text = "Montaggio indice destinazione..."
                End Select
            Case 1
                taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                currentTask.Text = "Mounting target index..."
            Case 2
                taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                currentTask.Text = "Montando índice de destino..."
            Case 3
                taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                currentTask.Text = "Montage de l'index de ciblage en cours..."
            Case 4
                taskCountLbl.Text = "Tarefas: " & currentTCont & "/" & taskCount
                currentTask.Text = "A montar o índice de destino..."
            Case 5
                taskCountLbl.Text = "Attività: " & currentTCont & "/" & TaskList.Count
                currentTask.Text = "Montaggio indice destinazione..."
        End Select
        LogView.AppendText(CrLf & "Mounting image (index: " & SwitchTargetIndex & ")...")
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        currentTask.Text = "Gathering error level..."
                    Case "ESN"
                        currentTask.Text = "Recopilando nivel de error..."
                    Case "FRA"
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
                    Case "PTB", "PTG"
                        currentTask.Text = "A recolher o nível de erro..."
                    Case "ITA"
                        currentTask.Text = "Raccolta livello errore..."
                End Select
            Case 1
                currentTask.Text = "Gathering error level..."
            Case 2
                currentTask.Text = "Recopilando nivel de error..."
            Case 3
                currentTask.Text = "Recueil du niveau d'erreur en cours..."
            Case 4
                currentTask.Text = "A recolher o nível de erro..."
            Case 5
                currentTask.Text = "Raccolta livello errore..."
        End Select
        LogView.AppendText(CrLf & "Gathering error level...")
        GetErrorCode(False)
        If errCode.Length >= 8 Then
            LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
        Else
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        End If
    End Sub

    Private Sub ReplaceFfuFile()
        DynaLog.LogMessage("Preparing to replace FFU files...")
        DynaLog.LogMessage("- Source file: " & Quote & FFUReplaceSourceFFU & Quote)
        DynaLog.LogMessage("- Destination file: " & Quote & FFUReplaceDestinationFFU & Quote)
        allTasks.Text = "Replacing FFU files..."
        currentTask.Text = "Replacing original FFU file with modified FFU file..."
        LogView.AppendText(CrLf & "Replacing FFU file " & Quote & FFUReplaceDestinationFFU & Quote & " with " & Quote & FFUReplaceSourceFFU & Quote & "...")
        Try
            If Not File.Exists(FFUReplaceSourceFFU) Or Not File.Exists(FFUReplaceDestinationFFU) Then Throw New Exception("One or both FFU files do not exist.")
            File.Delete(FFUReplaceDestinationFFU)
            File.Move(FFUReplaceSourceFFU, FFUReplaceDestinationFFU)
            IsSuccessful = True
            LogView.AppendText(CrLf & "The FFU file has been successfully replaced.")
        Catch ex As Exception
            DynaLog.LogMessage("FFU files could not be replaced. Error message: " & ex.Message)
            IsSuccessful = False
            LogView.AppendText(CrLf & "The FFU file could not be replaced: " & ex.Message)
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
                                   "Warning: the contents of the log window could not be saved to the log file. Reason: " & ex.Message)
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
                                       "Warning: the contents of the log window could not be saved to the log file. Reason: " & ex.Message)
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
                               "The volume images have been deleted. If you want to remount this image into a DISMTools project, choose the " & Quote & "Mount image" & Quote & " option, or use this command if you want to mount it elsewhere:" & CrLf &
                               "  dism /mount-image /imagefile:" & Quote & imgIndexDeletionSourceImg & Quote & " /index:<preferred index> /mountdir:<preferred mountpoint>")
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
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MainForm.MenuDesc.Text = "Ready"
                        Case "ESN"
                            MainForm.MenuDesc.Text = "Listo"
                        Case "FRA"
                            MainForm.MenuDesc.Text = "Prêt"
                        Case "PTB", "PTG"
                            MainForm.MenuDesc.Text = "Pronto"
                        Case "ITA"
                            MainForm.MenuDesc.Text = "Pronto"
                    End Select
                Case 1
                    MainForm.MenuDesc.Text = "Ready"
                Case 2
                    MainForm.MenuDesc.Text = "Listo"
                Case 3
                    MainForm.MenuDesc.Text = "Prêt"
                Case 4
                    MainForm.MenuDesc.Text = "Pronto"
                Case 5
                    MainForm.MenuDesc.Text = "Pronto"
            End Select
            TaskList.Clear()
            MainForm.StatusStrip.BackColor = CurrentTheme.AccentColors(1)
            MainForm.StartMountedImageDetector()
            Close()
        Else
            DynaLog.LogMessage("Tasks have not been successful.")
            Cancel_Button.Visible = True
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label1.Text = "Could not perform image operations"
                            Label2.Text = "An error has occurred, which stopped the image operations. Please read the log below for more information."
                        Case "ESN"
                            Label1.Text = "No se pudieron realizar las operaciones"
                            Label2.Text = "Ha ocurrido un error, el cual detuvo las operaciones. Lea el registro debajo para más información."
                        Case "FRA"
                            Label1.Text = "Impossible d'effectuer des opérations de l'image"
                            Label2.Text = "Une erreur s'est produite, qui a interrompu les opérations sur l'image. Veuillez lire le journal ci-dessous pour plus d'informations."
                        Case "PTB", "PTG"
                            Label1.Text = "Não foi possível efetuar operações de imagem"
                            Label2.Text = "Ocorreu um erro que interrompeu as operações de imagem. Leia o registo abaixo para obter mais informações."
                        Case "ITA"
                            Label1.Text = "Non è stato possibile eseguire operazioni sull'immagine"
                            Label2.Text = "Si è verificato un errore che ha interrotto le operazioni sull'immagine. Per ulteriori informazioni, consulta il registro sottostante."
                    End Select
                Case 1
                    Label1.Text = "Could not perform image operations"
                    Label2.Text = "An error has occurred, which stopped the image operations. Please read the log below for more information."
                Case 2
                    Label1.Text = "No se pudieron realizar las operaciones"
                    Label2.Text = "Ha ocurrido un error, el cual detuvo las operaciones. Lea el registro debajo para más información."
                Case 3
                    Label1.Text = "Impossible d'effectuer des opérations de l'image"
                    Label2.Text = "Une erreur s'est produite, qui a interrompu les opérations sur l'image. Veuillez lire le journal ci-dessous pour plus d'informations."
                Case 4
                    Label1.Text = "Não foi possível efetuar operações de imagem"
                    Label2.Text = "Ocorreu um erro que interrompeu as operações de imagem. Leia o registo abaixo para obter mais informações."
                Case 5
                    Label1.Text = "Non è stato possibile eseguire operazioni sull'immagine"
                    Label2.Text = "Si è verificato un errore che ha interrotto le operazioni sull'immagine. Per ulteriori informazioni, consulta il registro sottostante."
            End Select
            CurrentPB.Value = CurrentPB.Maximum
            AllPB.Value = AllPB.Maximum
            If Not IsExpanded Then
                LogButton.PerformClick()
            End If
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Cancel_Button.Text = "OK"
                        Case "ESN"
                            Cancel_Button.Text = "Aceptar"
                        Case "FRA"
                            Cancel_Button.Text = "OK"
                        Case "PTB", "PTG"
                            Cancel_Button.Text = "OK"
                        Case "ITA"
                            Cancel_Button.Text = "OK"
                    End Select
                Case 1
                    Cancel_Button.Text = "OK"
                Case 2
                    Cancel_Button.Text = "Aceptar"
                Case 3
                    Cancel_Button.Text = "OK"
                Case 4
                    Cancel_Button.Text = "OK"
                Case 5
                    Cancel_Button.Text = "OK"
            End Select
            LinkLabel1.Visible = True
            ' Add details for error codes
            DynaLog.LogMessage("Error code: " & errCode)
            If errCode = "C1420126" Then
                ' An image that was selected for mounting is already mounted
                LogView.AppendText(CrLf & "The specified image is already mounted. This command works for " & Quote & "orphaned" & Quote & " images")
            ElseIf errCode = "C142010C" Then
                ' The image, with read-only permissions, was attempted to be written
                LogView.AppendText(CrLf & "The program tried to save changes to an image that was mounted as read-only. " & CrLf &
                                          "To solve this, close this dialog, and click " & Quote & "Tools > Remount image with write permissions" & Quote & CrLf &
                                          "Do note that, if the image came from an installation medium, you may need to copy the source file to perform modifications to it.")
            ElseIf errCode = "C1420117" Then
                ' Some applications (or hidden processes) have open handles on the mount dir
                LogView.AppendText(CrLf & "The program tried to unmount the image, but some applications or processes have opened files or directories of the image." & CrLf &
                                          "Make sure no application or process is using the directories or files of the image." & CrLf &
                                          "If the error occurred at the end of the operation (e.g., at 100%), and you were trying to save the changes; they might already be saved, and can be safe to continue discarding changes.")
            ElseIf errCode = "C142011D" Then
                ' A partial unmount or an in-progress mount operation happened
                LogView.AppendText(CrLf & "The mounted image cannot be committed back into the source file." & CrLf &
                                          "A partial unmount might have happened, or the image was still being mounted." & CrLf &
                                          "If the image was unmounted whilst saving changes, the commit probably succeeded. Please validate this. If this is the case, proceed with unmounting the image discarding changes.")
            ElseIf errCode = "C1510111" Then
                ' The specified image, that was marked to mount with read-write permissions, came from a read-only source (e.g., a Windows installation disc)
                LogView.AppendText(CrLf & "The source file comes from a read-only source. You cannot mount it with read-write permissions." & CrLf &
                                          "Please re-specify the image in the mount dialog whilst checking the " & Quote & "Read-only" & Quote & " check box. You can also try copying the source image to a folder with read-write permissions.")
            ElseIf errCode = "00000087" Then
                ' Internal errors
                LogView.AppendText(CrLf & "There is essential data that was not picked internally by the operation. This may be a bug in the software or a feature may be incomplete.")
            ElseIf OperationNum = 26 Then
                ' No packages have been added successfully
                LogView.AppendText(CrLf & "No packages have been added successfully. Try looking up the error codes on the Internet")
            ElseIf OperationNum = 27 Then
                ' No packages have been removed successfully
                LogView.AppendText(CrLf & "No packages have been removed successfully. Try looking up the error codes on the Internet")
            ElseIf OperationNum = 30 Then
                ' No features have been enabled successfully
                LogView.AppendText(CrLf & "No features have been enabled successfully. Try looking up the error codes on the Internet")
            ElseIf OperationNum = 31 Then
                ' No features have been disabled successfully
                LogView.AppendText(CrLf & "No features have been disabled successfully. Try looking up the error codes on the Internet")
            ElseIf OperationNum = 78 Then
                ' Cause is undetermined
                LogView.AppendText(CrLf & "Either this operation has failed or some drivers were not installed. Consider reloading this project or mode to see whether there are driver changes." & CrLf & CrLf &
                                   "If there are driver changes, consider reading the driver installation logs, stored in the INF directory of the target image. Otherwise, export the drivers you want to add from the source image and add them to the target image manually." & CrLf & CrLf &
                                   "You can also manually customize the export directory by deleting the drivers you don't need. This may be another way to fix this problem, but you will need to temporarily pause the driver addition procedure before it scans the export directory (this can be done by selecting anything from the DISM command prompt window that appears when performing an operation)")
            ElseIf errCode = "00000001" Then

            ElseIf errCode = "C000013A" Then
                ' Keyboard interrupt (Ctrl-C) or forced program closure. The former may not trigger this error, as it may trigger error 1223
                LogView.AppendText(CrLf & "The program has suffered a keyboard interrupt, or a forced program closure. The operation has been cancelled. If you have done it accidentally, you may run it again")
            ElseIf errCode = "C2FE0101" Then
                ' This happens on operation numbers 90, 91, and 92; related to Microsoft Edge servicing, if the components have already been installed.
                ' Since these operation numbers are meant for different things, detect them
                If OperationNum = 90 Then
                    LogView.AppendText(CrLf & "The Microsoft Edge components have already been installed in this image. There isn't anything to do here.")
                ElseIf OperationNum = 91 Then
                    LogView.AppendText(CrLf & "The Microsoft Edge browser has already been installed in this image. There isn't anything to do here.")
                ElseIf OperationNum = 92 Then
                    LogView.AppendText(CrLf & "The Microsoft Edge WebView2 component has already been installed in this image. There isn't anything to do here.")
                End If
            ElseIf errCode = "800F0806" Then
                ' There are pending image operations
                LogView.AppendText(CrLf & "The operation could not be performed because this image has pending online operations. Applying and booting up the image might fix this issue.")
            ElseIf errCode = "BC2" Then
                DynaLog.LogMessage("The task has succeded but requires a restart...")
                If OperationNum = 86 Then
                    DynaLog.LogMessage("Rollback initiated. Restarting system automatically in 10 seconds...")
                    LogView.AppendText(CrLf & "The rollback process has started. Your system needs to be restarted in order to continue. It will restart automatically in 10 seconds. Make sure you have saved your work.")
                    Dim restartProc As New Process()
                    restartProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\shutdown.exe"
                    restartProc.StartInfo.Arguments = "/r /t 10 /c " & Quote & "Shutdown initiated by DISMTools" & Quote
                    restartProc.StartInfo.CreateNoWindow = True
                    restartProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    restartProc.Start()
                Else
                    LogView.AppendText(CrLf & "The specified operation completed successfully, but requires a restart in order to be fully applied. Save your work and restart when ready")
                End If
            Else
                Try
                    Dim exitDesc As New Win32Exception(Int32.Parse(errCode, Globalization.NumberStyles.HexNumber))
                    LogView.AppendText(CrLf & CrLf & exitDesc.Message)
                Catch ex As Exception
                    ' Errors that weren't added to the database
                    LogView.AppendText(CrLf & "This error has not yet been added to the database, so a useful description can't be shown now. Try running the command manually and, if you see the same error, try looking it up on the Internet.")
                End Try
            End If
            LogView.AppendText(CrLf & CrLf & "For detailed information, consider reading the DISM operation logs.")
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MainForm.MenuDesc.Text = "Ready"
                        Case "ESN"
                            MainForm.MenuDesc.Text = "Listo"
                        Case "FRA"
                            MainForm.MenuDesc.Text = "Prêt"
                        Case "PTB", "PTG"
                            MainForm.MenuDesc.Text = "Pronto"
                        Case "ITA"
                            MainForm.MenuDesc.Text = "Pronto"
                    End Select
                Case 1
                    MainForm.MenuDesc.Text = "Ready"
                Case 2
                    MainForm.MenuDesc.Text = "Listo"
                Case 3
                    MainForm.MenuDesc.Text = "Prêt"
                Case 4
                    MainForm.MenuDesc.Text = "Pronto"
                Case 5
                    MainForm.MenuDesc.Text = "Pronto"
            End Select
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
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Progress"
                        Label1.Text = "Image operations in progress..."
                        Label2.Text = "Please wait while the following tasks are done. This may take some time."
                        Cancel_Button.Text = "Cancel"
                        LogButton.Text = If(Not IsExpanded, "Show log", "Hide log")
                        LinkLabel1.Text = "Show DISM log file (advanced)"
                        allTasks.Text = "Please wait..."
                        currentTask.Text = "Please wait..."
                    Case "ESN"
                        Text = "Progreso"
                        Label1.Text = "Operaciones en progreso..."
                        Label2.Text = "Espere mientras las siguientes tareas se realizan. Esto puede llevar algo de tiempo."
                        Cancel_Button.Text = "Cancelar"
                        LogButton.Text = If(Not IsExpanded, "Mostrar registro", "Ocultar registro")
                        LinkLabel1.Text = "Mostrar archivo de registro de DISM (avanzado)"
                        allTasks.Text = "Por favor, espere..."
                        currentTask.Text = "Por favor, espere..."
                    Case "FRA"
                        Text = "Avancement"
                        Label1.Text = "Opérations de l'image en cours..."
                        Label2.Text = "Veuillez patienter pendant que les tâches suivantes sont effectuées. Cela peut prendre un certain temps."
                        Cancel_Button.Text = "Annuler"
                        LogButton.Text = If(Not IsExpanded, "Afficher le journal", "Cacher le journal")
                        LinkLabel1.Text = "Afficher le fichier journal DISM (avancé)"
                        allTasks.Text = "Veuillez patienter..."
                        currentTask.Text = "Veuillez patienter..."
                    Case "PTB", "PTG"
                        Text = "Progresso"
                        Label1.Text = "Operações de imagem em curso..."
                        Label2.Text = "Aguarde enquanto as seguintes tarefas são efectuadas. Isto pode demorar algum tempo"
                        Cancel_Button.Text = "Cancelar"
                        LogButton.Text = If(Not IsExpanded, " Mostrar registo", "Ocultar registo")
                        LinkLabel1.Text = "Mostrar ficheiro de registo DISM (avançado)"
                        allTasks.Text = "Aguarde..."
                        currentTask.Text = "Por favor, aguarde..."
                    Case "ITA"
                        Text = "Progresso"
                        Label1.Text = "Operazioni immagine..."
                        Label2.Text = "Attendi mentre vengono eseguite le operazioni. L'operazione potrebbe richiedere del tempo"
                        Cancel_Button.Text = "Annulla"
                        LogButton.Text = If(Not IsExpanded, " Visualizza registro", "Nascondi registro")
                        LinkLabel1.Text = "Visualizza il file registro DISM (avanzato)"
                        allTasks.Text = "Attendi..."
                        currentTask.Text = "Attendi..."
                End Select
            Case 1
                Text = "Progress"
                Label1.Text = "Image operations in progress..."
                Label2.Text = "Please wait while the following tasks are done. This may take some time."
                Cancel_Button.Text = "Cancel"
                LogButton.Text = If(Not IsExpanded, "Show log", "Hide log")
                LinkLabel1.Text = "Show DISM log file (advanced)"
                allTasks.Text = "Please wait..."
                currentTask.Text = "Please wait..."
            Case 2
                Text = "Progreso"
                Label1.Text = "Operaciones en progreso..."
                Label2.Text = "Espere mientras las siguientes tareas se realizan. Esto puede llevar algo de tiempo."
                Cancel_Button.Text = "Cancelar"
                LogButton.Text = If(Not IsExpanded, "Mostrar registro", "Ocultar registro")
                LinkLabel1.Text = "Mostrar archivo de registro de DISM (avanzado)"
                allTasks.Text = "Por favor, espere..."
                currentTask.Text = "Por favor, espere..."
            Case 3
                Text = "Avancement"
                Label1.Text = "Opérations de l'image en cours..."
                Label2.Text = "Veuillez patienter pendant que les tâches suivantes sont effectuées. Cela peut prendre un certain temps."
                Cancel_Button.Text = "Annuler"
                LogButton.Text = If(Not IsExpanded, "Afficher le journal", "Cacher le journal")
                LinkLabel1.Text = "Afficher le fichier journal DISM (avancé)"
                allTasks.Text = "Veuillez patienter..."
                currentTask.Text = "Veuillez patienter..."
            Case 4
                Text = "Progresso"
                Label1.Text = "Operações de imagem em curso..."
                Label2.Text = "Aguarde enquanto as seguintes tarefas são efectuadas. Isto pode demorar algum tempo"
                Cancel_Button.Text = "Cancelar"
                LogButton.Text = If(Not IsExpanded, " Mostrar registo", "Ocultar registo")
                LinkLabel1.Text = "Mostrar ficheiro de registo DISM (avançado)"
                allTasks.Text = "Aguarde..."
                currentTask.Text = "Por favor, aguarde..."
            Case 5
                Text = "Progresso"
                Label1.Text = "Operazioni immagine..."
                Label2.Text = "Attendi mentre vengono eseguite le operazioni. L'operazione potrebbe richiedere del tempo"
                Cancel_Button.Text = "Annulla"
                LogButton.Text = If(Not IsExpanded, " Visualizza registro", "Nascondi registro")
                LinkLabel1.Text = "Visualizza il file registro DISM (avanzato)"
                allTasks.Text = "Attendi..."
                currentTask.Text = "Attendi..."
        End Select
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
        Language = MainForm.Language
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
            LogView.AppendText("Cancelling background processes...")
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
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        MainForm.MenuDesc.Text = "Performing image operations. Please wait..."
                    Case "ESN"
                        MainForm.MenuDesc.Text = "Realizando operaciones con la imagen. Espere..."
                    Case "FRA"
                        MainForm.MenuDesc.Text = "Exécution d'opérations sur les images en cours. Veuillez patienter..."
                    Case "PTB", "PTG"
                        MainForm.MenuDesc.Text = "Realização de operações de imagem. Por favor, aguarde..."
                    Case "ITA"
                        MainForm.MenuDesc.Text = "Esecuzione operazioni sulle immagini..."
                End Select
            Case 1
                MainForm.MenuDesc.Text = "Performing image operations. Please wait..."
            Case 2
                MainForm.MenuDesc.Text = "Realizando operaciones con la imagen. Espere..."
            Case 3
                MainForm.MenuDesc.Text = "Exécution d'opérations sur les images en cours. Veuillez patienter..."
            Case 4
                MainForm.MenuDesc.Text = "Realização de operações de imagem. Por favor, aguarde..."
            Case 5
                MainForm.MenuDesc.Text = "Esecuzione operazioni sulle immagini..."
        End Select
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
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            taskCountLbl.Text = "Tasks: 1/" & TaskList.Count
                        Case "ESN"
                            taskCountLbl.Text = "Tareas: 1/" & TaskList.Count
                        Case "FRA"
                            taskCountLbl.Text = "Tâches : 1/" & TaskList.Count
                        Case "PTB", "PTG"
                            taskCountLbl.Text = "Tarefas: 1/" & TaskList.Count
                        Case "ITA"
                            taskCountLbl.Text = "Attività: 1/" & TaskList.Count
                    End Select
                Case 1
                    taskCountLbl.Text = "Tasks: 1/" & TaskList.Count
                Case 2
                    taskCountLbl.Text = "Tareas: 1/" & TaskList.Count
                Case 3
                    taskCountLbl.Text = "Tâches : 1/" & TaskList.Count
                Case 4
                    taskCountLbl.Text = "Tarefas: 1/" & TaskList.Count
                Case 5
                    taskCountLbl.Text = "Attività: 1/" & TaskList.Count
            End Select
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
                LogView.AppendText(CrLf & "The image registry hives need to be unloaded before continuing to perform the task.")
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
                LogView.AppendText(CrLf & "System editor was not found")
            ElseIf Not File.Exists(Application.StartupPath & "\logs\" & dateStr) Or Not File.Exists(LogPath) Then
                DynaLog.LogMessage("The log file is not found on this system.")
                LogView.AppendText(CrLf & "The log file was not found")
            End If
        End Try
    End Sub

    Private Sub BodyPanel_Paint(sender As Object, e As PaintEventArgs) Handles BodyPanel.Paint
        ControlPaint.DrawBorder(e.Graphics, BodyPanel.ClientRectangle, CurrentTheme.AccentColors(1), ButtonBorderStyle.Solid)
    End Sub

    Private Sub ProgressPanel_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        MainForm.MenuDesc.Text = "Ready"
                    Case "ESN"
                        MainForm.MenuDesc.Text = "Listo"
                    Case "FRA"
                        MainForm.MenuDesc.Text = "Prêt"
                    Case "PTB", "PTG"
                        MainForm.MenuDesc.Text = "Pronto"
                    Case "ITA"
                        MainForm.MenuDesc.Text = "Pronto"
                End Select
            Case 1
                MainForm.MenuDesc.Text = "Ready"
            Case 2
                MainForm.MenuDesc.Text = "Listo"
            Case 3
                MainForm.MenuDesc.Text = "Prêt"
            Case 4
                MainForm.MenuDesc.Text = "Pronto"
            Case 5
                MainForm.MenuDesc.Text = "Pronto"
        End Select
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
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        olcText = "Operation Logs"
                    Case "ESN"
                        olcText = "Registros de operación"
                    Case "FRA"
                        olcText = "Journal des opérations"
                    Case "PTB", "PTG"
                        olcText = "Registos de operações"
                    Case "ITA"
                        olcText = "Registri operazioni"
                End Select
            Case 1
                olcText = "Operation Logs"
            Case 2
                olcText = "Registros de operación"
            Case 3
                olcText = "Journal des opérations"
            Case 4
                olcText = "Registos de operações"
            Case 5
                olcText = "Registri operazioni"
        End Select
        WindowHelper.DisplayToolTip(sender, olcText)
    End Sub

    Private Sub LogSwitcherPic2_MouseHover(sender As Object, e As EventArgs) Handles LogSwitcherPic2.MouseHover
        Dim olcText As String = ""
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        olcText = "DISM Output"
                    Case "ESN"
                        olcText = "Salida de DISM"
                    Case "FRA"
                        olcText = "Sortie DISM"
                    Case "PTB", "PTG"
                        olcText = "Saída DISM"
                    Case "ITA"
                        olcText = "Output DISM"
                End Select
            Case 1
                olcText = "DISM Output"
            Case 2
                olcText = "Salida de DISM"
            Case 3
                olcText = "Sortie DISM"
            Case 4
                olcText = "Saída DISM"
            Case 5
                olcText = "Uscita DISM"
        End Select
        WindowHelper.DisplayToolTip(sender, olcText)
    End Sub

    Private Sub ProgressPanel_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If WindowState = FormWindowState.Maximized Then
            WindowState = FormWindowState.Normal
        End If
    End Sub
End Class