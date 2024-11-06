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

Public Class ProgressPanel

    Public taskCount As Long
    Dim currentTCont As Integer = 1
    Public OperationNum As Long

    Public IsSuccessful As Boolean
    Public IsDebugged As Boolean

    Public errCode As String

    Public CommandArgs As String = ""                       ' Ubiquitous across OperationNums. DO NOT DELETE !!!
    Public DismVersionChecker As FileVersionInfo
    Public DismProgram As String

    Dim dateStr As String = "DISMTools-"

    Dim Language As Integer = 0                             ' Form language, taken from MainForm

    Dim imgSession As DismSession = Nothing                 ' Image session for the DISM API
    Dim mntString As String = ""                            ' Mount directory, necessary for the DISM API

    Dim OnlineMgmt As Boolean                               ' Determine whether to perform actions to the active installation or the mounted Windows image

    Public TaskList As New List(Of Integer)                 ' Task list

    Dim AllDrivers As Boolean                               ' Detects whether the program should detect all image drivers, taken from MainForm

    Public ActionRunning As Boolean                         ' Detects whether an Action file is being run
    Public IsInValidationMode As Boolean                    ' Detects whether an Action file is being validated

    Public Actions_ImageFile As String
    Public Actions_ImageIndex As Integer

    Public ActionFile As String

    Public ActionParameters As New List(Of String)

    Dim ImgVersion As Version

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
    Public ApplicationDestDrive As String                   ' Gather destination disk ID

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

    ' OperationNum: 18
    Public remountisReadOnly As Boolean                     ' Determine whether the remount happened because of a read-only mounted image

    ' OperationNum: 20
    Public SWMSplitSourceFile As String                     ' Source image file to be split into SWM files
    Public SWMSplitFileSize As Integer                      ' The maximum size in MB for each created image
    Public SWMSplitTargetFile As String                     ' The path of the SWM files
    Public SWMSplitCheckIntegrity As Boolean                ' Checks the integrity of the source image before splitting it

    ' OperationNum: 21
    Public UMountImgIndex As Integer
    Public ProgramIsBeingClosed As Boolean
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
    Public imgCommitAfterOps As Boolean                     ' If option is checked, commit image after operations are done
    Public pkgAdditionOp As Integer                         ' 0: recursive operation; 1: selective operation
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
    Public featCommitAfterEnablement As Boolean             ' Determine whether to commit image after enabling features
    Public featEnablementCount As Integer                   ' Count number of features to enable
    Public featCanContactWU As Boolean                      ' Determine whether program can contact Windows Update
    Dim featSuccessfulEnablements As Integer                ' Successful feature enablement count
    Dim featFailedEnablements As Integer                    ' Failed feature enablement count

    ' OperationNum: 31
    Public featDisablementNames(65535) As String            ' Array used to determine which features need to be disabled
    Public featDisablementLastName As String                ' Last feature entry checked
    Public featDisablementParentPkgUsed As Boolean          ' Determine whether to specify the parent package name for the features
    Public featDisablementParentPkg As String               ' Parent package name to use when disabling features
    Public featRemoveManifest As Boolean                    ' Remove feature without removing manifest
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

    ' OperationNum: 78
    Public ImportSourceInt As Integer                       ' The import source
    ' ImportSourceInt = 0
    Public DrvImport_SourceImage As String                  ' The mounted image that will act as the source for the driver import
    ' ImportSourceInt = 2
    Public DrvImport_SourceDisk As String                   ' The disk drive that will act as the source for the driver import

    ' OperationNum: 79
    Public UnattendedFile As String                         ' The path of the unattended answer file

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

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        If Cancel_Button.Text = "Cancel" Or Cancel_Button.Text = "Cancelar" Or Cancel_Button.Text = "Annulla" Then
            ProgressBW.CancelAsync()
        ElseIf Cancel_Button.Text = "OK" Or Cancel_Button.Text = "Aceptar" Then
            MainForm.ToolStripButton4.Visible = False
            Close()
        End If
    End Sub

    Private Sub LogButton_Click(sender As Object, e As EventArgs) Handles LogButton.Click
        If Height = 240 Then
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
            Height = 420
        ElseIf Height = 420 Then
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
                            LogButton.Text = "Mostra registro"
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
                    LogButton.Text = "Mostra registro"
            End Select
            Height = 240
        End If
        BodyPanel.Refresh()
        CenterToParent()
    End Sub

    Sub GetTasks(opNum As Integer)
        If opNum = 0 Then
            taskCount = 1
        ElseIf opNum = 1 Then
            taskCount = 1
        ElseIf opNum = 3 Then
            taskCount = 1
        ElseIf opNum = 6 Then
            If CaptureMountDestImg Then
                taskCount = 3
            Else
                taskCount = 1
            End If
        ElseIf opNum = 7 Then
            taskCount = 1
        ElseIf opNum = 8 Then
            taskCount = 1
        ElseIf opNum = 9 Then
            If imgIndexDeletionUnmount Then
                taskCount = 2
            Else
                taskCount = 1
            End If
        ElseIf opNum = 10 Then
            taskCount = 1
        ElseIf opNum = 15 Then
            taskCount = 1
        ElseIf opNum = 18 Then
            taskCount = 1
        ElseIf opNum = 20 Then
            taskCount = 1
        ElseIf opNum = 21 Then
            taskCount = 1
        ElseIf opNum = 26 Then
            If imgCommitAfterOps Then
                taskCount = 2
            Else
                taskCount = 1
            End If
        ElseIf opNum = 27 Then
            taskCount = 1
        ElseIf opNum = 30 Then
            If featCommitAfterEnablement Then
                taskCount = 2
            Else
                taskCount = 1
            End If
        ElseIf opNum = 31 Then
            taskCount = 1
        ElseIf opNum = 32 Then
            taskCount = 1
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
        ElseIf opNum = 38 Then
            taskCount = 1
        ElseIf opNum = 60 Then
            taskCount = 1
        ElseIf opNum = 64 Then
            If capAdditionCommit Then
                taskCount = 2
            Else
                taskCount = 1
            End If
        ElseIf opNum = 68 Then
            taskCount = 1
        ElseIf opNum = 75 Then
            If drvAdditionCommit Then
                taskCount = 2
            Else
                taskCount = 1
            End If
        ElseIf opNum = 76 Then
            taskCount = 1
        ElseIf opNum = 77 Then
            taskCount = 1
        ElseIf opNum = 78 Then
            taskCount = 1
        ElseIf opNum = 79 Then
            taskCount = 1
        ElseIf opNum = 83 Then
            taskCount = 1
        ElseIf opNum = 84 Then
            taskCount = 1
        ElseIf opNum = 86 Then
            taskCount = 1
        ElseIf opNum = 87 Then
            taskCount = 1
        ElseIf opNum = 88 Then
            taskCount = 1
        ElseIf opNum = 991 Then
            taskCount = 1
        ElseIf opNum = 992 Then
            taskCount = 1
        ElseIf opNum = 996 Then
            taskCount = 2
        End If
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
        CommandArgs = "/logpath=" & Quote & If(AutoLogs, Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now), LogPath) & Quote & " /loglevel=" & LogLevel & If(UseScratchDir, If(AutoScratch, If(OnlineMgmt, " /scratchdir=" & Quote & Application.StartupPath & "\scratch" & Quote, " /scratchdir=" & Quote & projPath & "\scr_temp"), If(ScratchDirPath <> "", " /scratchdir=" & Quote & ScratchDirPath & Quote, "")), "") & If(EnglishOut, " /english", "")
        BckArgs = CommandArgs
    End Sub

    ''' <summary>
    ''' Sets the name of the log file using the current date and time
    ''' </summary>
    ''' <param name="CurrentDate">The date to add. It is always "Now"</param>
    ''' <returns>This function returns a file name that can be used in log files, file-system friendly on both Unix and Windows</returns>
    ''' <remarks></remarks>
    Function GetCurrentDateAndTime(CurrentDate As Date) As String
        dateStr = "DISMTools-" & CurrentDate.ToString()
        ' Make sure the file with the name is file-system friendly
        If dateStr.Contains("/") Or dateStr.Contains(":") Then
            dateStr = dateStr.Replace("/", "-").Trim().Replace(":", "-").Trim()
        End If
        dateStr &= ".log"
        Return dateStr
    End Function

    Sub RunTaskList(taskList As List(Of Integer))
        Dim successfulTasks As Integer = 0
        Dim failedTasks As Integer = 0
        Dim prevValue As Integer = 0
        For Each Task In taskList
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
            If IsSuccessful Then successfulTasks += 1 Else failedTasks += 1
        Next
        If successfulTasks > failedTasks Then IsSuccessful = True Else IsSuccessful = False
    End Sub

    Sub RunOps(opNum As Integer)
        If DismProgram = "" Then DismProgram = MainForm.DismExe
        If Not File.Exists(DismProgram) Then DismProgram = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
        DismVersionChecker = FileVersionInfo.GetVersionInfo(DismProgram)
        CurrentPB.Value = 0
        PkgErrorText.RichTextBox1.Clear()
        FeatErrorText.RichTextBox1.Clear()
        DismApi.Initialize(DismLogLevel.LogErrors)
        Dim OperationUseQuotes As Boolean
        Dim targetImage As String = ""
        If MountDir <> "" Then
            OperationUseQuotes = Not Path.GetPathRoot(MountDir) = MountDir
            targetImage = If(OperationUseQuotes, Quote & MountDir & Quote, MountDir)
        End If
        If opNum = 0 Then
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
                Directory.CreateDirectory(projPath & "\" & projName)
                CurrentPB.Value = 16.66
                Thread.Sleep(125)
                AllPB.Value = CurrentPB.Value
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "settings")
                CurrentPB.Value = 33.33
                Thread.Sleep(125)
                AllPB.Value = CurrentPB.Value
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "mount")
                CurrentPB.Value = 50
                Thread.Sleep(125)
                AllPB.Value = CurrentPB.Value
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "scr_temp")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "reports")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "DandI")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "DandI\x86")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "DandI\amd64")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "DandI\arm")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "DandI\arm64")
                CurrentPB.Value = 66.66
                Thread.Sleep(125)
                AllPB.Value = CurrentPB.Value
                File.WriteAllText(projPath & "\" & projName & "\" & "settings\project.ini", _
                                  "[ProjOptions]" & CrLf & _
                                  "Name=" & Quote & projName & Quote & CrLf & _
                                  "Location=" & projPath & CrLf & _
                                  "EpochCreationTime=" & DateTimeOffset.Now.ToUnixTimeSeconds().ToString() & CrLf & CrLf & _
                                  "[ImageOptions]" & CrLf & _
                                  "ImageFile=N/A" & CrLf & _
                                  "ImageIndex=N/A" & CrLf & _
                                  "ImageMountPoint=N/A" & CrLf & _
                                  "ImageVersion=N/A" & CrLf & _
                                  "ImageName=N/A" & CrLf & _
                                  "ImageDescription=N/A" & CrLf & _
                                  "ImageWIMBoot=N/A" & CrLf & _
                                  "ImageArch=N/A" & CrLf & _
                                  "ImageHal=N/A" & CrLf & _
                                  "ImageSPBuild=N/A" & CrLf & _
                                  "ImageSPLevel=N/A" & CrLf & _
                                  "ImageEdition=N/A" & CrLf & _
                                  "ImagePType=N/A" & CrLf & _
                                  "ImagePSuite=N/A" & CrLf & _
                                  "ImageSysRoot=N/A" & CrLf & _
                                  "ImageDirCount=N/A" & CrLf & _
                                  "ImageFileCount=N/A" & CrLf & _
                                  "ImageEpochCreate=N/A" & CrLf & _
                                  "ImageEpochModify=N/A" & CrLf & _
                                  "ImageLang=N/A" & CrLf & CrLf & _
                                  "[Params]" & CrLf & _
                                  "ImageReadWrite=N/A", ASCII)
                CurrentPB.Value = 83.33
                Thread.Sleep(125)
                AllPB.Value = CurrentPB.Value
                File.WriteAllText(projPath & "\" & projName & "\" & projName & ".dtproj", _
                                  "# DISMTools project file. File version: 0.1" & CrLf & _
                                  "[Settings]" & CrLf & _
                                  "SettingsInclude=\settings\project.ini" & CrLf & CrLf & _
                                  "[Project]" & CrLf & _
                                  "ProjName=" & projName & CrLf & _
                                  "ProjGuid=" & Guid.NewGuid().ToString(), ASCII)
                CurrentPB.Value = 100
                Thread.Sleep(125)
                AllPB.Value = CurrentPB.Value
                LogView.AppendText(CrLf & "Project created successfully.")
                CurrentPB.Value = CurrentPB.Maximum
                AllPB.Value = AllPB.Maximum
                IsSuccessful = True
            Catch ex As Exception
                LogView.AppendText(CrLf & "An error has occurred. Please read the details below: " & CrLf & ex.GetType().ToString() & ": " & Err.Description)
                If IsDebugged Then
                    LogView.AppendText(CrLf & "Debugging information: " & ex.StackTrace)
                End If
                IsSuccessful = False
            End Try
        ElseIf opNum = 1 Then
            ' This variable tells the program to use quotes when appending a mount directory in a drive.
            ' This is false when we want to append an entire drive.
            Dim AppendixUseQuotes As Boolean = Not Path.GetPathRoot(AppendixSourceDir) = AppendixSourceDir
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
                            currentTask.Text = "Applicazione della cartella di montaggio specificata all'immagine di destinazione specificata..."
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
                    currentTask.Text = "Applicazione della cartella di montaggio specificata all'immagine di destinazione specificata..."
            End Select
            LogView.AppendText(CrLf & "Appending mount directory to specified target image..." & CrLf & "Options:" & CrLf &
                               "- Source image directory: " & AppendixSourceDir & CrLf &
                               "- Destination image file: " & AppendixDestinationImage & CrLf &
                               "- Destination image name: " & AppendixName & CrLf &
                               "- Destination image description: " & If(AppendixDescription = "", "(none specified)", AppendixDescription) & CrLf)
            If AppendixWimScriptConfig = "" Then
                LogView.AppendText("- Configuration list file: not specified" & CrLf)
            Else
                LogView.AppendText("- Configuration list file: " & Quote & AppendixWimScriptConfig & Quote & CrLf)
                If Not File.Exists(AppendixWimScriptConfig) Then
                    LogView.AppendText("   WARNING: the configuration list file does not exist in the file system. Skipping file..." & CrLf)
                End If
            End If
            LogView.AppendText("- Append image with WIMBoot configuration? " & If(AppendixUseWimBoot, "Yes", "No") & CrLf &
                               "- Make image bootable? " & If(AppendixBootable, "Yes", "No") & CrLf &
                               "- Verify image integrity? " & If(AppendixCheckIntegrity, "Yes", "No") & CrLf &
                               "- Check for file errors? " & If(AppendixVerify, "Yes", "No") & CrLf &
                               "- Use the reparse point tag fix? " & If(AppendixReparsePt, "Yes", "No") & CrLf &
                               "- Capture extended attributes? " & If(AppendixCaptureExtendedAttribs, "Yes", "No"))
            DISMProc.StartInfo.FileName = DismProgram
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
                CommandArgs &= " /description=" & Quote & AppendixDescription & Quote
            End If
            If AppendixWimScriptConfig <> "" AndAlso File.Exists(AppendixWimScriptConfig) Then
                CommandArgs &= " /configfile=" & Quote & AppendixWimScriptConfig & Quote
            End If
            If AppendixBootable Then CommandArgs &= " /bootable"
            If AppendixUseWimBoot Then CommandArgs &= " /wimboot"
            If AppendixCheckIntegrity Then CommandArgs &= " /checkintegrity"
            If AppendixVerify Then CommandArgs &= " /verify"
            If Not AppendixReparsePt Then CommandArgs &= " /norpfix"
            If AppendixCaptureExtendedAttribs Then CommandArgs &= " /EA"
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
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
                            currentTask.Text = "Raccolta del livello di errore..."
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
        ElseIf opNum = 3 Then
            ' My love with DISM came from this very YouTube video:
            ' https://www.youtube.com/watch?v=JxJ6a-PY1KA (Enderman - Manually installing Windows 10)
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
                            currentTask.Text = "Applicazione dell'immagine specificata alla destinazione specificata..."
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
            LogView.AppendText(CrLf & "Applying image..." & CrLf & "Options:" & CrLf & _
                               "- Source image file: " & ApplicationSourceImg & CrLf & _
                               "- Index to apply: " & ApplicationIndex & CrLf & _
                               "- Target directory: " & ApplicationDestDir & CrLf)
            DISMProc.StartInfo.FileName = DismProgram
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
            If ApplicationDestDrive = "" Then
                CommandArgs &= " /applydir=" & Quote & ApplicationDestDir & Quote
            Else
                CommandArgs &= " /applydrive=" & ApplicationDestDrive
            End If
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
            If ApplicationDestDrive = "" Then
                LogView.AppendText("- Destination drive ID: not specified/not applying on drive" & CrLf)
            Else
                LogView.AppendText("- Destination drive ID: " & ApplicationDestDrive & CrLf)
            End If
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
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
                            currentTask.Text = "Raccolta del livello di errore..."
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
        ElseIf opNum = 6 Then
            ' This variable tells the program to use quotes when capturing a mount directory in a drive.
            ' This is false when we want to capture an entire drive.
            Dim UseQuotes As Boolean = Not Path.GetPathRoot(CaptureSourceDir) = CaptureSourceDir
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
                            allTasks.Text = "Cattura dell'immagine..."
                            currentTask.Text = "Cattura della cartella specificata in una nuova immagine..."
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
                    allTasks.Text = "Cattura dell'immagine..."
                    currentTask.Text = "Cattura della cartella specificata in una nuova immagine..."
            End Select
            LogView.AppendText(CrLf & "Capturing directory..." & CrLf & "Options:" & CrLf &
                               "- Source directory: " & CaptureSourceDir & CrLf &
                               "- Destination image: " & CaptureDestinationImage & CrLf &
                               "- Captured image name: " & CaptureName & CrLf)
            DISMProc.StartInfo.FileName = DismProgram
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
                LogView.AppendText("- Captured image description: " & Quote & CaptureDescription & Quote & CrLf)
                CommandArgs &= " /description=" & Quote & CaptureDescription & Quote
            End If
            If CaptureWimScriptConfig = "" Then
                LogView.AppendText("- " & Quote & "WimScript.ini" & Quote & " configuration file: not specified" & CrLf)
            Else
                LogView.AppendText("- " & Quote & "WimScript.ini" & Quote & " configuration file: " & CaptureWimScriptConfig & CrLf)
                ' Possibly, the file may have been deleted after being specified. Determine whether it still exists
                If File.Exists(CaptureWimScriptConfig) Then
                    CommandArgs &= " /configfile=" & Quote & CaptureWimScriptConfig & Quote
                Else
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
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
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
                            currentTask.Text = "Raccolta del livello di errore..."
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
        ElseIf opNum = 7 Then
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
                            allTasks.Text = "Pulizia dei punti di montaggio..."
                            currentTask.Text = "Eliminazione di risorse da immagini vecchie o corrotte..."
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
                    allTasks.Text = "Pulizia dei punti di montaggio..."
                    currentTask.Text = "Eliminazione di risorse da immagini vecchie o corrotte..."
            End Select
            LogView.AppendText(CrLf & "Cleaning up mount points..." & CrLf & CrLf &
                               "This can take some time, depending on the drives connected to this system.")
            Try
                DismApi.Initialize(If(LogLevel = 1, DismLogLevel.LogErrors, If(LogLevel = 2, DismLogLevel.LogErrorsWarnings, If(LogLevel = 3, DismLogLevel.LogErrorsWarningsInfo, DismLogLevel.LogErrorsWarningsInfo))), If(AutoLogs, Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now), LogPath))
                DismApi.CleanupMountpoints()
            Catch ex As DismException
                errCode = Hex(ex.ErrorCode)
            Finally
                DismApi.Shutdown()
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
                            currentTask.Text = "Raccolta del livello di errore..."
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
            If errCode Is Nothing Then
                errCode = 0
                IsSuccessful = True
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
            Else
                LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
            End If
        ElseIf opNum = 8 Then
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
                            allTasks.Text = "Commettere l'immagine..."
                            currentTask.Text = "Salvataggio delle modifiche all'immagine..."
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
                    allTasks.Text = "Commettere l'immagine..."
                    currentTask.Text = "Salvataggio delle modifiche all'immagine..."
            End Select
            LogView.AppendText(CrLf & "Saving changes..." & CrLf & "Options:" & CrLf &
                               "- Mount directory: " & MountDir)

            DISMProc.StartInfo.FileName = DismProgram
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
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
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
                            currentTask.Text = "Raccolta del livello di errore..."
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
        ElseIf opNum = 9 Then
            If imgIndexDeletionUnmount Then
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
                            allTasks.Text = "Eliminazione delle immagini..."
                            currentTask.Text = "Preparazione alla rimozione delle immagini del volume..."
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
                    allTasks.Text = "Eliminazione delle immagini..."
                    currentTask.Text = "Preparazione alla rimozione delle immagini del volume..."
            End Select
            LogView.AppendText(CrLf & "Removing volume images from file..." & CrLf & _
                               "Options:" & CrLf & _
                               "- Source image: " & imgIndexDeletionSourceImg & CrLf)
            If imgIndexDeletionIntCheck Then
                LogView.AppendText("- Check image integrity? Yes")
            Else
                LogView.AppendText("- Check image integrity? No")
            End If
            CurrentPB.Maximum = imgIndexDeletionCount
            ' Removing volume images
            LogView.AppendText(CrLf & _
                               "Removing volume images..." & CrLf)
            For x = 0 To Array.LastIndexOf(imgIndexDeletionNames, imgIndexDeletionLastName)
                If x + 1 > CurrentPB.Maximum Then Exit For
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
                                currentTask.Text = "Rimozione dell'immagine del volume " & Quote & imgIndexDeletionNames(x) & Quote & "..."
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
                        currentTask.Text = "Rimozione dell'immagine del volume " & Quote & imgIndexDeletionNames(x) & Quote & "..."
                End Select
                LogView.AppendText(CrLf & _
                                   "- " & imgIndexDeletionNames(x) & "...")
                DISMProc.StartInfo.FileName = DismProgram
                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /delete-image /imagefile=" & Quote & imgIndexDeletionSourceImg & Quote & " /name=" & Quote & imgIndexDeletionNames(x) & Quote
                If imgIndexDeletionIntCheck Then
                    CommandArgs &= " /checkintegrity"
                End If
                DISMProc.StartInfo.Arguments = CommandArgs
                DISMProc.Start()
                DISMProc.WaitForExit()
                If Hex(DISMProc.ExitCode).Length < 8 Then
                    LogView.AppendText(" Error level : " & DISMProc.ExitCode)
                Else
                    LogView.AppendText(" Error level : 0x" & Hex(DISMProc.ExitCode))
                End If
            Next
            CurrentPB.Value = CurrentPB.Maximum
            AllPB.Value = 100
            GetErrorCode(False)
        ElseIf opNum = 10 Then
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
                            currentTask.Text = "Esportazione dell'immagine specificata..."
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
                    currentTask.Text = "Esportazione dell'immagine specificata..."
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
            If Path.GetExtension(imgExportSourceImage).EndsWith("swm", StringComparison.OrdinalIgnoreCase) Then
                LogView.AppendText(CrLf & CrLf & "NOTE: the source image contains an asterisk sign (*) in the file name to merge all SWM files")
            End If
            DISMProc.StartInfo.FileName = DismProgram
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
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
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
                            currentTask.Text = "Raccolta del livello di errore..."
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
        ElseIf opNum = 11 Then
            ' Operation handled by the image file information dialog - Redundant OpNum
        ElseIf opNum = 15 Then
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
                            allTasks.Text = "Montaggio dell'immagine..."
                            currentTask.Text = "Montaggio dell'immagine specificata..."
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
                    allTasks.Text = "Montaggio dell'immagine..."
                    currentTask.Text = "Montaggio dell'immagine specificata..."
            End Select
            LogView.AppendText(CrLf & "Mounting image..." & CrLf & "Options:" & CrLf &
                               "- Image file: " & SourceImg & CrLf &
                               "- Image index: " & ImgIndex & CrLf &
                               "- Mount point: " & MountDir)
            DISMProc.StartInfo.FileName = DismProgram
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
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
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
                            currentTask.Text = "Raccolta del livello di errore..."
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
        ElseIf opNum = 18 Then
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
                            allTasks.Text = "Rimontaggio dell'immagine..."
                            currentTask.Text = "Ricaricamento della sessione di assistenza per l'immagine montata..."
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
                    allTasks.Text = "Rimontaggio dell'immagine..."
                    currentTask.Text = "Ricaricamento della sessione di assistenza per l'immagine montata..."
            End Select
            LogView.AppendText(CrLf & "Reloading servicing session..." & CrLf &
                               "- Mount directory: " & MountDir)
            Try
                DismApi.Initialize(If(LogLevel = 1, DismLogLevel.LogErrors, If(LogLevel = 2, DismLogLevel.LogErrorsWarnings, If(LogLevel = 3, DismLogLevel.LogErrorsWarningsInfo, DismLogLevel.LogErrorsWarningsInfo))), If(AutoLogs, Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now), LogPath))
                DismApi.RemountImage(MountDir)
            Catch ex As DismException
                errCode = Hex(ex.ErrorCode)
                IsSuccessful = False
            Finally
                DismApi.Shutdown()
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
                            currentTask.Text = "Raccolta del livello di errore..."
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
            If errCode Is Nothing Then
                errCode = 0
                IsSuccessful = True
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
            Else
                LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
            End If
        ElseIf opNum = 20 Then
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
                            allTasks.Text = "Divisione dell'immagine..."
                            currentTask.Text = "Divisione del file WIM..."
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
                    allTasks.Text = "Divisione dell'immagine..."
                    currentTask.Text = "Divisione del file WIM..."
            End Select
            LogView.AppendText(CrLf & "Splitting WIM file into SWM files..." & CrLf & _
                               "- Source image file to split: " & Quote & SWMSplitSourceFile & Quote & CrLf & _
                               "- Maximum size of the split images (in MB): " & SWMSplitFileSize & " MB" & CrLf & _
                               "- Name and path of the target SWM file: " & Quote & SWMSplitTargetFile & Quote & CrLf & _
                               "- Check integrity before splitting this image? " & If(SWMSplitCheckIntegrity, "Yes", "No") & CrLf & CrLf & _
                               "Do note that, if the image contains a large file that can't fit within the maximum size, a SWM file may be larger than the rest, to accommodate it." & CrLf)
            ' Check the DISM version, as the Windows 7 version doesn't allow this action
            DISMProc.StartInfo.FileName = DismProgram
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
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
            LogView.AppendText(CrLf & "Getting error level...")
            If Hex(DISMProc.ExitCode).Length < 8 Then
                errCode = DISMProc.ExitCode
            Else
                errCode = Hex(DISMProc.ExitCode)
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(" Error level : 0x" & errCode)
            Else
                LogView.AppendText(" Error level : " & errCode)
            End If
            GetErrorCode(False)
        ElseIf opNum = 21 Then
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
                            currentTask.Text = "Smontaggio del file immagine..."
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
                    currentTask.Text = "Smontaggio del file immagine..."
            End Select
            If UMountLocalDir Then
                LogView.AppendText(CrLf & "Unmounting image file from mount point..." & CrLf &
                                   "- Mount directory: " & MountDir & CrLf &
                                   "- Image index: " & UMountImgIndex)
            Else
                MountDir = RandomMountDir
                LogView.AppendText(CrLf & "Unmounting image file from mount point..." & CrLf &
                                   "- Mount directory: " & MountDir & CrLf &
                                   "- Image index: " & UMountImgIndex)
            End If
            If ProgramIsBeingClosed Then
                LogView.AppendText(CrLf & "- Unmount operation: Commit")
                ' Commit the image and unmount it
                Try
                    DISMProc.StartInfo.FileName = DismProgram
                    Select Case DismVersionChecker.ProductMajorPart
                        Case 6
                            Select Case DismVersionChecker.ProductMinorPart
                                Case 1
                                    CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-wim /mountdir=" & Quote & MountDir & Quote & " /commit"
                                Case Is >= 2
                                    CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-image /mountdir=" & Quote & MountDir & Quote & " /commit"
                            End Select
                        Case 10
                            CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-image /mountdir=" & Quote & MountDir & Quote & " /commit"
                    End Select
                    DISMProc.StartInfo.Arguments = CommandArgs
                    DISMProc.Start()
                    DISMProc.WaitForExit()
                    If DISMProc.ExitCode = Decimal.ToInt32(-1052638964) Then
                        LogView.AppendText(CrLf & CrLf & "Saving changes to the image has failed. Discarding changes...")
                        ' It mostly came from a read-only source. Discard changes
                        DISMProc.StartInfo.FileName = DismProgram
                        Select Case DismVersionChecker.ProductMajorPart
                            Case 6
                                Select Case DismVersionChecker.ProductMinorPart
                                    Case 1
                                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-wim /mountdir=" & Quote & MountDir & Quote & " /discard"
                                    Case Is >= 2
                                        CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-image /mountdir=" & Quote & MountDir & Quote & " /discard"
                                End Select
                            Case 10
                                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /unmount-image /mountdir=" & Quote & MountDir & Quote & " /discard"
                        End Select
                        DISMProc.StartInfo.Arguments = CommandArgs
                        DISMProc.Start()
                        DISMProc.WaitForExit()
                    End If
                Catch ex As Exception
                    File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat",
                                      "@echo off" & CrLf &
                                      "dism /English /unmount-image /mountdir=" & MountDir,
                                      ASCII)
                    Process.Start(Application.StartupPath & "\bin\exthelpers\temp.bat").WaitForExit()
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
                                currentTask.Text = "Raccolta del livello di errore..."
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
            Else
                Try
                    DISMProc.StartInfo.FileName = DismProgram
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
                    DISMProc.StartInfo.Arguments = CommandArgs
                    DISMProc.Start()
                    DISMProc.WaitForExit()
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
                                currentTask.Text = "Raccolta del livello di errore..."
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
            End If
        ElseIf opNum = 26 Then
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
                            allTasks.Text = "Aggiunta di pacchetti..."
                            currentTask.Text = "Preparazione all'aggiunta di pacchetti..."
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
                    allTasks.Text = "Aggiunta di pacchetti..."
                    currentTask.Text = "Preparazione all'aggiunta di pacchetti..."
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
            If imgCommitAfterOps Then
                LogView.AppendText(CrLf & "- Commit image after operations are done? Yes")
            Else
                LogView.AppendText(CrLf & "- Commit image after operations are done? No")
            End If

            ' Perform package enumeration
            LogView.AppendText(CrLf & "Enumerating packages to add. Please wait...")
            If pkgAdditionOp = 0 Then
                Try
                    For Each CabPkg In My.Computer.FileSystem.GetFiles(pkgSource, FileIO.SearchOption.SearchAllSubDirectories, "*.cab")
                        pkgCount += 1
                    Next
                    For Each MsuPkg In My.Computer.FileSystem.GetFiles(pkgSource, FileIO.SearchOption.SearchAllSubDirectories, "*.msu")
                        pkgCount += 1
                    Next
                    LogView.AppendText(CrLf & "Total number of packages: " & pkgCount)
                Catch ex As Exception
                    LogView.AppendText(CrLf & "Exception " & ex.GetType().ToString() & " has occurred while enumerating packages. Enumerating packages in the top folder...")
                    For Each CabPkg In My.Computer.FileSystem.GetFiles(pkgSource, FileIO.SearchOption.SearchTopLevelOnly, "*.cab")
                        pkgCount += 1
                    Next
                    For Each MsuPkg In My.Computer.FileSystem.GetFiles(pkgSource, FileIO.SearchOption.SearchTopLevelOnly, "*.msu")
                        pkgCount += 1
                    Next
                    LogView.AppendText(CrLf & "Total number of packages: " & pkgCount)
                End Try
            ElseIf pkgAdditionOp = 1 Then
                LogView.AppendText(CrLf & "Total number of packages: " & pkgCount)
            ElseIf pkgAdditionOp = 2 Then
                LogView.AppendText(CrLf & "Total number of packages: 1")
            End If
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
                DISMProc.StartInfo.FileName = DismProgram
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /add-package /packagepath=" & Quote & pkgSource & Quote
                If pkgIgnoreApplicabilityChecks Then
                    CommandArgs &= " /ignorecheck"
                End If
                If pkgPreventIfPendingOnline Then
                    CommandArgs &= " /preventpending"
                End If
                DISMProc.StartInfo.Arguments = CommandArgs
                DISMProc.Start()
                DISMProc.WaitForExit()
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
                                currentTask.Text = "Raccolta del livello di errore..."
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
                LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
            ElseIf pkgAdditionOp = 1 Then
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
                    Dim pkgIsApplicable As Boolean
                    Dim pkgIsInstalled As Boolean
                    Try
                        If Not Path.GetExtension(pkgs(x)).EndsWith("msu", StringComparison.OrdinalIgnoreCase) Then
                            DismApi.Initialize(DismLogLevel.LogErrors)
                            Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                                Dim pkgInfo As DismPackageInfo = DismApi.GetPackageInfoByPath(imgSession, pkgs(x))
                                LogView.AppendText(CrLf & CrLf & _
                                                   "- Package name: " & pkgInfo.PackageName & CrLf & _
                                                   "- Package description: " & pkgInfo.Description & CrLf & _
                                                   "- Package release type: " & Casters.CastDismReleaseType(pkgInfo.ReleaseType) & CrLf & _
                                                   "- Package is applicable to this image? " & If(pkgInfo.Applicable, "Yes", "No") & CrLf & _
                                                   "- Package is already installed? " & If(pkgInfo.PackageState = DismPackageFeatureState.Installed Or pkgInfo.PackageState = DismPackageFeatureState.InstallPending, "Yes", "No") & CrLf)
                                pkgIsApplicable = pkgInfo.Applicable
                                If pkgInfo.PackageState = DismPackageFeatureState.Installed Or pkgInfo.PackageState = DismPackageFeatureState.InstallPending Then pkgIsInstalled = True Else pkgIsInstalled = False
                                If pkgInfo.Applicable Then
                                    If pkgInfo.PackageState = DismPackageFeatureState.Installed Or pkgInfo.PackageState = DismPackageFeatureState.InstallPending Then
                                        LogView.AppendText(CrLf & "Package is already added. Skipping installation of this package...")
                                        pkgFailedAdditions += 1
                                    End If
                                Else
                                    If Not pkgIgnoreApplicabilityChecks Then
                                        LogView.AppendText(CrLf & "Package is not applicable to this image. Skipping installation of this package...")
                                        If PkgErrorText.RichTextBox1.Text = "" Then
                                            PkgErrorText.RichTextBox1.AppendText("0x800F8023")
                                        Else
                                            PkgErrorText.RichTextBox1.AppendText(CrLf & "0x800F8023")
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
                        LogView.AppendText(CrLf & ex.Message)
                        If PkgErrorText.RichTextBox1.Text = "" Then
                            PkgErrorText.RichTextBox1.AppendText(If(Hex(ex.HResult).Length >= 8, "0x" & Hex(ex.HResult), Hex(ex.HResult)))
                        Else
                            PkgErrorText.RichTextBox1.AppendText(CrLf & If(Hex(ex.HResult).Length >= 8, "0x" & Hex(ex.HResult), Hex(ex.HResult)))
                        End If
                        pkgFailedAdditions += 1
                        pkgIsApplicable = False
                    Finally
                        DismApi.Shutdown()
                    End Try
                    If Not pkgIsApplicable Or pkgIsInstalled Then Continue For
                    LogView.AppendText(CrLf & "Processing package...")
                    DISMProc.StartInfo.FileName = DismProgram
                    CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /add-package /packagepath=" & Quote & pkgs(x) & Quote
                    If pkgIgnoreApplicabilityChecks Then
                        CommandArgs &= " /ignorecheck"
                    End If
                    If pkgPreventIfPendingOnline Then
                        CommandArgs &= " /preventpending"
                    End If
                    DISMProc.StartInfo.Arguments = CommandArgs
                    DISMProc.Start()
                    DISMProc.WaitForExit()
                    LogView.AppendText(CrLf & "Getting error level...")
                    GetPkgErrorLevel()
                    LogView.AppendText(" Error level: " & errCode)
                    If PkgErrorText.RichTextBox1.Text = "" Then
                        PkgErrorText.RichTextBox1.AppendText(errCode)
                    Else
                        PkgErrorText.RichTextBox1.AppendText(CrLf & errCode)
                    End If
                Next
                CurrentPB.Value = CurrentPB.Maximum
                LogView.AppendText(CrLf & "Gathering error level for selected packages..." & CrLf)
                For x = 0 To PkgErrorText.RichTextBox1.Lines.Count - 1
                    LogView.AppendText(CrLf & "- Package no. " & (x + 1) & ": " & PkgErrorText.RichTextBox1.Lines(x))
                Next
            ElseIf pkgAdditionOp = 2 Then
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
                DISMProc.StartInfo.FileName = DismProgram
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /add-package /packagepath=" & Quote & pkgs(0) & Quote
                If pkgIgnoreApplicabilityChecks Then
                    CommandArgs &= " /ignorecheck"
                End If
                If pkgPreventIfPendingOnline Then
                    CommandArgs &= " /preventpending"
                End If
                DISMProc.StartInfo.Arguments = CommandArgs
                DISMProc.Start()
                DISMProc.WaitForExit()
                LogView.AppendText(CrLf & "Getting error level...")
                GetPkgErrorLevel()
                LogView.AppendText(" Error level: " & errCode)
                If PkgErrorText.RichTextBox1.Text = "" Then
                    PkgErrorText.RichTextBox1.AppendText(errCode)
                Else
                    PkgErrorText.RichTextBox1.AppendText(CrLf & errCode)
                End If
                CurrentPB.Value = CurrentPB.Maximum
                LogView.AppendText(CrLf & "Gathering error level for selected packages..." & CrLf)
                For x = 0 To PkgErrorText.RichTextBox1.Lines.Count - 1
                    LogView.AppendText(CrLf & "- Package no. " & (x + 1) & ": " & PkgErrorText.RichTextBox1.Lines(x))
                Next
            End If
            Thread.Sleep(2000)
            If imgCommitAfterOps Then
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
            If PkgErrorText.RichTextBox1.Text.Contains("BC2") Then
                LogView.AppendText(CrLf & "Some packages require a system restart to be fully processed. Save your work, close your programs, and restart when ready")
            End If
        ElseIf opNum = 27 Then
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
                            allTasks.Text = "Rimozione dei pacchetti..."
                            currentTask.Text = "Preparazione alla rimozione dei pacchetti..."
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
                    allTasks.Text = "Rimozione dei pacchetti..."
                    currentTask.Text = "Preparazione alla rimozione dei pacchetti..."
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
                            currentTask.Text = "Rimozione dei pacchetti..."
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
                    currentTask.Text = "Rimozione dei pacchetti..."
            End Select
            CurrentPB.Maximum = pkgRemovalCount
            If pkgRemovalOp = 0 Then
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

                    Dim pkgIsRemovable As Boolean
                    Try
                        DismApi.Initialize(DismLogLevel.LogErrors)
                        Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                            Dim pkgInfo As DismPackageInfo = DismApi.GetPackageInfoByName(imgSession, pkgRemovalNames(x))
                            LogView.AppendText(CrLf & CrLf & _
                                               "- Package name: " & pkgInfo.PackageName & CrLf)
                            If pkgInfo.PackageState = DismPackageFeatureState.Installed Then
                                LogView.AppendText("- Package state: installed" & CrLf)
                            ElseIf pkgInfo.PackageState = DismPackageFeatureState.UninstallPending Then
                                LogView.AppendText("- Package state: an uninstall is pending" & CrLf)
                            ElseIf pkgInfo.PackageState = DismPackageFeatureState.InstallPending Then
                                LogView.AppendText("- Package state: an install is pending" & CrLf)
                            End If
                            If pkgInfo.PackageState = DismPackageFeatureState.Installed Or pkgInfo.PackageState = DismPackageFeatureState.InstallPending Then
                                pkgIsReadyForRemoval = True
                            Else
                                pkgIsReadyForRemoval = False
                            End If
                        End Using
                        pkgIsRemovable = True
                    Catch ex As Exception
                        LogView.AppendText(CrLf & ex.Message)
                        If PkgErrorText.RichTextBox1.Text = "" Then
                            PkgErrorText.RichTextBox1.AppendText(If(Hex(ex.HResult).Length >= 8, "0x" & Hex(ex.HResult), Hex(ex.HResult)))
                        Else
                            PkgErrorText.RichTextBox1.AppendText(CrLf & If(Hex(ex.HResult).Length >= 8, "0x" & Hex(ex.HResult), Hex(ex.HResult)))
                        End If
                        pkgFailedRemovals += 1
                        pkgIsRemovable = False
                    Finally
                        DismApi.Shutdown()
                    End Try
                    If Not pkgIsRemovable Then Continue For
                    If pkgIsReadyForRemoval Then
                        LogView.AppendText(CrLf & "Processing package removal...")
                        DISMProc.StartInfo.FileName = DismProgram
                        CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /remove-package /packagename=" & pkgRemovalNames(x)
                        DISMProc.StartInfo.Arguments = CommandArgs
                        DISMProc.Start()
                        DISMProc.WaitForExit()
                        LogView.AppendText(CrLf & "Getting error level...")
                        errCode = Hex(Decimal.ToInt32(DISMProc.ExitCode))
                        If DISMProc.ExitCode = 0 Then
                            pkgSuccessfulRemovals += 1
                        Else
                            pkgFailedRemovals += 1
                        End If
                        'GetPkgErrorLevel()
                        If errCode.Length >= 8 Then
                            LogView.AppendText(CrLf & CrLf & " Error level : 0x" & errCode)
                        Else
                            LogView.AppendText(CrLf & CrLf & " Error level : " & errCode)
                        End If
                        If PkgErrorText.RichTextBox1.Text = "" Then
                            If errCode.Length >= 8 Then
                                PkgErrorText.RichTextBox1.AppendText("0x" & errCode)
                            Else
                                PkgErrorText.RichTextBox1.AppendText(errCode)
                            End If
                        Else
                            If errCode.Length >= 8 Then
                                PkgErrorText.RichTextBox1.AppendText(CrLf & "0x" & errCode)
                            Else
                                PkgErrorText.RichTextBox1.AppendText(CrLf & errCode)
                            End If
                        End If
                    Else
                        LogView.AppendText(CrLf & "This package can't be removed. Skipping removal of this package...")
                        pkgFailedRemovals += 1
                        Continue For
                    End If
                Next
            ElseIf pkgRemovalOp = 1 Then
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
                    Dim pkgIsRemovable As Boolean
                    Try
                        DismApi.Initialize(DismLogLevel.LogErrors)
                        Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                            Dim pkgInfo As DismPackageInfo = DismApi.GetPackageInfoByPath(imgSession, pkgRemovalFiles(x))
                            LogView.AppendText(CrLf & CrLf & _
                                               "- Package name: " & pkgInfo.PackageName & CrLf)
                            If pkgInfo.PackageState = DismPackageFeatureState.Installed Then
                                LogView.AppendText("- Package state: installed" & CrLf)
                            ElseIf pkgInfo.PackageState = DismPackageFeatureState.UninstallPending Then
                                LogView.AppendText("- Package state: an uninstall is pending" & CrLf)
                            ElseIf pkgInfo.PackageState = DismPackageFeatureState.InstallPending Then
                                LogView.AppendText("- Package state: an install is pending" & CrLf)
                            End If
                            If pkgInfo.PackageState = DismPackageFeatureState.Installed Or pkgInfo.PackageState = DismPackageFeatureState.InstallPending Then
                                pkgIsReadyForRemoval = True
                            Else
                                pkgIsReadyForRemoval = False
                            End If
                        End Using
                        pkgIsRemovable = True
                    Catch ex As Exception
                        LogView.AppendText(CrLf & ex.Message)
                        If PkgErrorText.RichTextBox1.Text = "" Then
                            PkgErrorText.RichTextBox1.AppendText(If(Hex(ex.HResult).Length >= 8, "0x" & Hex(ex.HResult), Hex(ex.HResult)))
                        Else
                            PkgErrorText.RichTextBox1.AppendText(CrLf & If(Hex(ex.HResult).Length >= 8, "0x" & Hex(ex.HResult), Hex(ex.HResult)))
                        End If
                        pkgFailedRemovals += 1
                        pkgIsRemovable = False
                    Finally
                        DismApi.Shutdown()
                    End Try
                    If Not pkgIsRemovable Then Continue For
                    If pkgIsReadyForRemoval Then
                        LogView.AppendText(CrLf & "Processing package removal...")
                        DISMProc.StartInfo.FileName = DismProgram
                        CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /remove-package /packagepath=" & pkgRemovalFiles(x)
                        DISMProc.StartInfo.Arguments = CommandArgs
                        DISMProc.Start()
                        DISMProc.WaitForExit()
                        LogView.AppendText(CrLf & "Getting error level...")
                        errCode = Hex(Decimal.ToInt32(DISMProc.ExitCode))
                        If DISMProc.ExitCode = 0 Then
                            pkgSuccessfulRemovals += 1
                        Else
                            pkgFailedRemovals += 1
                        End If
                        'GetPkgErrorLevel()
                        If errCode.Length >= 8 Then
                            LogView.AppendText(CrLf & CrLf & " Error level : 0x" & errCode)
                        Else
                            LogView.AppendText(CrLf & CrLf & " Error level : " & errCode)
                        End If
                        If PkgErrorText.RichTextBox1.Text = "" Then
                            If errCode.Length >= 8 Then
                                PkgErrorText.RichTextBox1.AppendText("0x" & errCode)
                            Else
                                PkgErrorText.RichTextBox1.AppendText(errCode)
                            End If
                        Else
                            If errCode.Length >= 8 Then
                                PkgErrorText.RichTextBox1.AppendText(CrLf & "0x" & errCode)
                            Else
                                PkgErrorText.RichTextBox1.AppendText(CrLf & errCode)
                            End If
                        End If
                    Else
                        LogView.AppendText(CrLf & "This package can't be removed. Skipping removal of this package...")
                        pkgFailedRemovals += 1
                        Continue For
                    End If
                Next
            End If
            Directory.Delete(Application.StartupPath & "\tempinfo", True)
            CurrentPB.Value = CurrentPB.Maximum
            LogView.AppendText(CrLf & "Gathering error level for selected packages..." & CrLf)
            For x = 0 To PkgErrorText.RichTextBox1.Lines.Count - 1
                LogView.AppendText(CrLf & "- Package no. " & (x + 1) & ": " & PkgErrorText.RichTextBox1.Lines(x))
            Next
            Thread.Sleep(2000)
            AllPB.Value = 100
            If pkgSuccessfulRemovals > 0 Then
                GetErrorCode(True)
            ElseIf pkgSuccessfulRemovals <= 0 Then
                GetErrorCode(False)
            End If
            If PkgErrorText.RichTextBox1.Text.Contains("BC2") Then
                LogView.AppendText(CrLf & "Some packages require a system restart to be fully processed. Save your work, close your programs, and restart when ready")
            End If
        ElseIf opNum = 30 Then
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
                            allTasks.Text = "Abilitazione delle caratteristiche..."
                            currentTask.Text = "Preparazione all'abilitazione delle caratteristiche..."
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
                    allTasks.Text = "Abilitazione delle caratteristiche..."
                    currentTask.Text = "Preparazione all'abilitazione delle caratteristiche..."
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
            If featContactWindowsUpdate And OnlineMgmt Then
                LogView.AppendText(CrLf & "- Contact Windows Update? Yes")
            ElseIf featContactWindowsUpdate And OnlineMgmt And SystemInformation.BootMode = BootMode.FailSafe Then
                LogView.AppendText(CrLf & "- Contact Windows Update? No, the system is in Safe Mode")
            ElseIf featContactWindowsUpdate And OnlineMgmt = False Then
                LogView.AppendText(CrLf & "- Contact Windows Update? No, this is not an online installation")
            Else
                LogView.AppendText(CrLf & "- Contact Windows Update? No")
            End If
            If featCommitAfterEnablement Then
                LogView.AppendText(CrLf & "- Commit image after enabling features? Yes")
            Else
                LogView.AppendText(CrLf & "- Commit image after enabling features? No")
            End If
            LogView.AppendText(CrLf & CrLf & "Enumerating features to enable...")
            Thread.Sleep(500)
            LogView.AppendText(CrLf & "Total number of features to enable: " & featEnablementCount)
            ' Get command ready
            DISMProc.StartInfo.FileName = DismProgram
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
                            currentTask.Text = "Abilitazione delle caratteristiche..."
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
                    currentTask.Text = "Abilitazione delle caratteristiche..."
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
                                currentTask.Text = "Abilitazione della caratteristica " & (x + 1) & " di " & featEnablementCount & "..."
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
                        currentTask.Text = "Abilitazione della caratteristica " & (x + 1) & " di " & featEnablementCount & "..."
                End Select
                LogView.AppendText(CrLf &
                                   "Feature " & (x + 1) & " of " & featEnablementCount)
                CurrentPB.Value = x + 1
                Try
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                        Dim featInfo As DismFeatureInfo = DismApi.GetFeatureInfo(imgSession, featEnablementNames(x).Replace("ListViewItem: ", "").Trim().Replace("{", "").Trim().Replace("}", "").Trim())
                        LogView.AppendText(CrLf & CrLf &
                                           "- Feature name: " & featInfo.FeatureName & CrLf &
                                           "- Feature description: " & featInfo.Description & CrLf)
                    End Using
                Finally
                    DismApi.Shutdown()
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
                DISMProc.StartInfo.Arguments = CommandArgs
                DISMProc.Start()
                DISMProc.WaitForExit()
                LogView.AppendText(CrLf & "Getting error level...")
                GetFeatErrorLevel()
                If errCode.Length >= 8 Then
                    LogView.AppendText(" Error level : 0x" & errCode)
                Else
                    LogView.AppendText(" Error level : " & errCode)
                End If
                If FeatErrorText.RichTextBox1.Text = "" Then
                    If errCode.Length >= 8 Then
                        FeatErrorText.RichTextBox1.AppendText("0x" & errCode)
                    Else
                        FeatErrorText.RichTextBox1.AppendText(errCode)
                    End If
                Else
                    If errCode.Length >= 8 Then
                        FeatErrorText.RichTextBox1.AppendText(CrLf & "0x" & errCode)
                    Else
                        FeatErrorText.RichTextBox1.AppendText(CrLf & errCode)
                    End If
                End If
            Next
            CurrentPB.Value = CurrentPB.Maximum
            LogView.AppendText(CrLf & "Gathering error level for selected features..." & CrLf)
            For x = 0 To FeatErrorText.RichTextBox1.Lines.Count - 1
                LogView.AppendText(CrLf & "- Feature no. " & (x + 1) & ": " & FeatErrorText.RichTextBox1.Lines(x))
            Next
            Thread.Sleep(2000)
            If featCommitAfterEnablement Then
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
            If FeatErrorText.RichTextBox1.Text.Contains("BC2") Then
                LogView.AppendText(CrLf & "Some features require a system restart to be fully processed. Save your work, close your programs, and restart when ready")
            End If
        ElseIf opNum = 31 Then
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
                            allTasks.Text = "Disabilitazione delle caratteristiche..."
                            currentTask.Text = "Preparazione alla disabilitazione delle caratteristiche..."
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
                    allTasks.Text = "Disabilitazione delle caratteristiche..."
                    currentTask.Text = "Preparazione alla disabilitazione delle caratteristiche..."
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
            If featRemoveManifest Then
                LogView.AppendText(CrLf & "- Remove feature manifest? Yes")
            Else
                LogView.AppendText(CrLf & "- Remove feature manifest? No")
            End If
            LogView.AppendText(CrLf & CrLf & "Enumerating features to disable...")
            Thread.Sleep(500)
            LogView.AppendText(CrLf & "Total number of features to disable: " & featDisablementCount)
            ' Get command ready
            DISMProc.StartInfo.FileName = DismProgram
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
                            currentTask.Text = "Disabilitazione delle caratteristiche..."
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
                    currentTask.Text = "Disabilitazione delle caratteristiche..."
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
                                currentTask.Text = "Disabilitazione della caratteristica " & (x + 1) & " di " & featDisablementCount & "..."
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
                        currentTask.Text = "Disabilitazione della caratteristica " & (x + 1) & " di " & featDisablementCount & "..."
                End Select
                LogView.AppendText(CrLf &
                                   "Feature " & (x + 1) & " of " & featDisablementCount)
                CurrentPB.Value = x + 1
                Try
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                        Dim featInfo As DismFeatureInfo = DismApi.GetFeatureInfo(imgSession, featDisablementNames(x).Replace("ListViewItem: ", "").Trim().Replace("{", "").Trim().Replace("}", "").Trim())
                        LogView.AppendText(CrLf & CrLf &
                                           "- Feature name: " & featInfo.FeatureName & CrLf &
                                           "- Feature description: " & featInfo.Description & CrLf)

                    End Using
                Finally
                    DismApi.Shutdown()
                End Try
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /disable-feature /featurename=" & featDisablementNames(x).Replace("ListViewItem: ", "").Trim().Replace("{", "").Trim().Replace("}", "").Trim()
                If featDisablementParentPkgUsed And featDisablementParentPkg <> "" Then
                    CommandArgs &= " /packagename=" & featParentPkgName
                End If
                If Not featRemoveManifest Then
                    CommandArgs &= " /remove"
                End If
                DISMProc.StartInfo.Arguments = CommandArgs
                DISMProc.Start()
                DISMProc.WaitForExit()
                LogView.AppendText(CrLf & "Getting error level...")
                'GetFeatErrorLevel()
                errCode = Hex(Decimal.ToInt32(DISMProc.ExitCode))
                If DISMProc.ExitCode = 0 Then
                    featSuccessfulDisablements += 1
                Else
                    featFailedDisablements += 1
                End If
                If errCode.Length >= 8 Then
                    LogView.AppendText(" Error level : 0x" & errCode)
                Else
                    LogView.AppendText(" Error level : " & errCode)
                End If
                If FeatErrorText.RichTextBox1.Text = "" Then
                    If errCode.Length >= 8 Then
                        FeatErrorText.RichTextBox1.AppendText("0x" & errCode)
                    Else
                        FeatErrorText.RichTextBox1.AppendText(errCode)
                    End If
                Else
                    If errCode.Length >= 8 Then
                        FeatErrorText.RichTextBox1.AppendText(CrLf & "0x" & errCode)
                    Else
                        FeatErrorText.RichTextBox1.AppendText(CrLf & errCode)
                    End If
                End If
            Next
            CurrentPB.Value = CurrentPB.Maximum
            LogView.AppendText(CrLf & "Gathering error level for selected features..." & CrLf)
            For x = 0 To FeatErrorText.RichTextBox1.Lines.Count - 1
                LogView.AppendText(CrLf & "- Feature no. " & (x + 1) & ": " & FeatErrorText.RichTextBox1.Lines(x))
            Next
            Thread.Sleep(2000)
            If featSuccessfulDisablements > 0 Then
                GetErrorCode(True)
            ElseIf featSuccessfulDisablements <= 0 Then
                GetErrorCode(False)
            End If
            If FeatErrorText.RichTextBox1.Text.Contains("BC2") Then
                LogView.AppendText(CrLf & "Some features require a system restart to be fully processed. Save your work, close your programs, and restart when ready")
            End If
        ElseIf opNum = 32 Then
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
                            allTasks.Text = "Pulire l'immagine..."
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
                    allTasks.Text = "Pulire l'immagine..."
            End Select
            ' Initialize command
            DISMProc.StartInfo.FileName = DismProgram
            CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /cleanup-image"
            Select Case CleanupTask
                Case 0
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
                                    currentTask.Text = "Ripristino delle azioni di assistenza in sospeso..."
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
                            currentTask.Text = "Ripristino delle azioni di assistenza in sospeso..."
                    End Select
                    LogView.AppendText(CrLf & _
                                       "Reverting pending servicing actions...")
                    CommandArgs &= " /revertpendingactions"
                Case 1
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
                                    currentTask.Text = "Pulizia dei file di backup del Service Pack..."
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
                            currentTask.Text = "Pulizia dei file di backup del Service Pack..."
                    End Select
                    LogView.AppendText(CrLf & _
                                       "Cleaning up Service Pack backup files..." & CrLf & _
                                       "Options:" & CrLf & _
                                       "- Hide Service Packs from the Installed Updates list? " & If(CleanupHideSP, "Yes", "No"))
                    CommandArgs &= " /spsuperseded" & If(CleanupHideSP, " /hidesp", "")
                Case 2
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
                                    currentTask.Text = "Pulizia dell'archivio dei componenti..."
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
                            currentTask.Text = "Pulizia dell'archivio dei componenti..."
                    End Select
                    LogView.AppendText(CrLf & _
                                       "Cleaning up the component store..." & CrLf & _
                                       "Options:" & CrLf & _
                                       "- Perform superseded component base reset? " & If(ResetCompBase, "Yes", "No") & CrLf & _
                                       "- Defer long-running operations? " & If(DeferCleanupOps, "Yes", "No"))
                    CommandArgs &= " /startcomponentcleanup" & If(ResetCompBase, " /resetbase", "") & If(ResetCompBase And DeferCleanupOps, " /defer", "")
                Case 3
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
                                    currentTask.Text = "Analisi dell'archivio dei componenti..."
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
                            currentTask.Text = "Analisi dell'archivio dei componenti..."
                    End Select
                    LogView.AppendText(CrLf & _
                                       "Analyzing the component store...")
                    CommandArgs &= " /analyzecomponentstore"
                Case 4
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
                                    currentTask.Text = "Controllo dello stato di salute dell'archivio componenti..."
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
                            currentTask.Text = "Controllo dello stato di salute dell'archivio componenti..."
                    End Select
                    LogView.AppendText(CrLf & _
                                       "Checking the component store health...")
                    CommandArgs &= " /checkhealth"
                Case 5
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
                                    currentTask.Text = "Scansione dell'archivio componenti..."
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
                            currentTask.Text = "Scansione dell'archivio componenti..."
                    End Select
                    LogView.AppendText(CrLf & _
                                       "Scanning the component store...")
                    CommandArgs &= " /scanhealth"
                Case 6
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
                                    currentTask.Text = "Riparazione dell'archivio componenti..."
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
                            currentTask.Text = "Riparazione dell'archivio componenti..."
                    End Select
                    LogView.AppendText(CrLf & _
                                       "Repairing the component store..." & CrLf & _
                                       "Options:" & CrLf & _
                                       "- Use different source? " & If(UseCompRepairSource, "Yes (" & Quote & ComponentRepairSource & Quote & ")", "No") & CrLf & _
                                       "- Limit Windows Update access? " & If(LimitWUAccess And OnlineMgmt, "Yes", If(LimitWUAccess And Not OnlineMgmt, "No, this is not an online installation", "No")) & _
                                       If(Not LimitWUAccess And OnlineMgmt And SystemInformation.BootMode = BootMode.FailSafe, ", the system is in Safe Mode", ""))
                    CommandArgs &= " /restorehealth" & If(UseCompRepairSource And File.Exists(ComponentRepairSource), " /source=" & Quote & ComponentRepairSource & Quote, "") & If(LimitWUAccess And OnlineMgmt, " /limitaccess", "")
            End Select
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
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
                            currentTask.Text = "Raccolta del livello di errore..."
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
        ElseIf opNum = 33 Then
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
                            allTasks.Text = "Aggiunta del pacchetto di approvvigionamento..."
                            currentTask.Text = "Aggiunta del pacchetto di approvvigionamento all'immagine..."
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
                    allTasks.Text = "Aggiunta del pacchetto di approvvigionamento..."
                    currentTask.Text = "Aggiunta del pacchetto di approvvigionamento all'immagine..."
            End Select
            LogView.AppendText("Adding provisioning package to the image..." & CrLf & _
                               "Options:" & CrLf & CrLf & _
                               "- Provisioning package: " & Quote & ppkgAdditionPackagePath & Quote & CrLf & _
                               "- Catalog file: " & If(ppkgAdditionCatalogPath = "", "none specified", Quote & ppkgAdditionCatalogPath & Quote) & CrLf & _
                               "- Commit image after adding provisioning package? " & If(ppkgAdditionCommit, "Yes", "No"))
            DISMProc.StartInfo.FileName = DismProgram
            CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /add-provisioningpackage /packagepath=" & Quote & ppkgAdditionPackagePath & Quote & If(ppkgAdditionCatalogPath <> "" And File.Exists(ppkgAdditionCatalogPath), " /catalogpath=" & Quote & ppkgAdditionCatalogPath & Quote, "")
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
            LogView.AppendText(CrLf & "Getting error level...")
            If Hex(DISMProc.ExitCode).Length < 8 Then
                errCode = DISMProc.ExitCode
            Else
                errCode = Hex(DISMProc.ExitCode)
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(" Error level : 0x" & errCode)
            Else
                LogView.AppendText(" Error level : " & errCode)
            End If
            If ppkgAdditionCommit Then
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
        ElseIf opNum = 37 Then
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
                            allTasks.Text = "Aggiunta di pacchetti AppX..."
                            currentTask.Text = "Preparazione all'aggiunta di pacchetti AppX approvvigionati..."
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
                    allTasks.Text = "Aggiunta di pacchetti AppX..."
                    currentTask.Text = "Preparazione all'aggiunta di pacchetti AppX approvvigionati..."
            End Select
            LogView.AppendText(CrLf & "Adding provisioned AppX packages..." & CrLf & _
                               "Options:" & CrLf)
            If appxAdditionUseLicenseFile Then
                LogView.AppendText("- Use a license file for AppX packages? Yes" & CrLf & _
                                   "- License file: " & appxAdditionLicenseFile & CrLf)
            Else
                LogView.AppendText("- Use a license file for AppX packages? No" & CrLf & _
                                   "- License file: not using" & CrLf)
            End If
            If appxAdditionUseCustomDataFile Then
                LogView.AppendText("- Use a custom data file for AppX packages? Yes" & CrLf & _
                                   "- Custom data file: " & appxAdditionCustomDataFile & CrLf)
            Else
                LogView.AppendText("- Use a custom data file for AppX packages? No" & CrLf & _
                                   "- Custom data file: not using" & CrLf)
            End If
            If appxAdditionUseAllRegions Then
                LogView.AppendText("- Use all regions for AppX packages? Yes" & CrLf & _
                                   "- Package regions: all" & CrLf)
            Else
                LogView.AppendText("- Use all regions for AppX packages? No" & CrLf & _
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
                            currentTask.Text = "Aggiunta di pacchetti AppX..."
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
                    currentTask.Text = "Aggiunta di pacchetti AppX..."
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
                                currentTask.Text = "Aggiunta del pacchetto " & (x + 1) & " di " & appxAdditionCount & "..."
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
                        currentTask.Text = "Aggiunta del pacchetto " & (x + 1) & " di " & appxAdditionCount & "..."
                End Select
                LogView.AppendText(CrLf & _
                                   "Package " & (x + 1) & " of " & appxAdditionCount)
                CurrentPB.Value = x + 1
                LogView.AppendText(CrLf & _
                                   "- AppX package file: " & appxAdditionPackageList(x).PackageFile & CrLf & _
                                   "- Application name: " & appxAdditionPackageList(x).PackageName & CrLf & _
                                   "- Application publisher: " & appxAdditionPackageList(x).PackagePublisher & CrLf & _
                                   "- Application version: " & appxAdditionPackageList(x).PackageVersion & CrLf)
                ' Detect if it is an encrypted application
                If Path.GetExtension(appxAdditionPackageList(x).PackageFile).Replace(".", "").Trim().StartsWith("e", StringComparison.OrdinalIgnoreCase) AndAlso OnlineMgmt Then
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
                    If PkgErrorText.RichTextBox1.Text = "" Then
                        If errCode.Length >= 8 Then
                            PkgErrorText.RichTextBox1.AppendText("0x" & errCode)
                        Else
                            PkgErrorText.RichTextBox1.AppendText(errCode)
                        End If
                    Else
                        If errCode.Length >= 8 Then
                            PkgErrorText.RichTextBox1.AppendText(CrLf & "0x" & errCode)
                        Else
                            PkgErrorText.RichTextBox1.AppendText(CrLf & errCode)
                        End If
                    End If
                    Continue For
                ElseIf Path.GetExtension(appxAdditionPackageList(x).PackageFile).Replace(".", "").Trim().StartsWith("e", StringComparison.OrdinalIgnoreCase) AndAlso Not OnlineMgmt Then
                    ' Continue loop without installing application
                    LogView.AppendText(CrLf & "The application about to be added is an encrypted file. Encrypted packages can only be added to active installations. Skipping this package..." & CrLf)
                    Continue For
                Else
                    ' Initialize command
                    DISMProc.StartInfo.FileName = DismProgram
                    CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /add-provisionedappxpackage "
                    If File.GetAttributes(appxAdditionPackageList(x).PackageFile) = FileAttributes.Directory Then
                        CommandArgs &= "/folderpath=" & Quote & appxAdditionPackageList(x).PackageFile & Quote
                    Else
                        CommandArgs &= "/packagepath=" & Quote & appxAdditionPackageList(x).PackageFile & Quote
                    End If
                    If appxAdditionPackageList(x).PackageLicenseFile <> "" And File.Exists(appxAdditionPackageList(x).PackageLicenseFile) Then
                        CommandArgs &= " /licensepath=" & Quote & appxAdditionPackageList(x).PackageLicenseFile & Quote
                    Else
                        If appxAdditionPackageList(x).PackageLicenseFile <> "" Then
                            LogView.AppendText(CrLf & _
                                               "Warning: the license file does not exist. Continuing without one..." & CrLf & _
                                               "         Do note that, if this app requires a license file, it may fail addition." & CrLf & _
                                               "         Also, this may compromise the image.")
                        End If
                        CommandArgs &= " /skiplicense"
                    End If
                    ' Inform user that a package will be installed with dependencies
                    If appxAdditionPackageList(x).PackageSpecifiedDependencies.Count > 0 Then
                        LogView.AppendText("- The following dependency packages will be installed alongside this application:" & CrLf)
                    End If
                    ' Add dependencies
                    For Each Dependency As AppxDependency In appxAdditionPackageList(x).PackageSpecifiedDependencies
                        If File.Exists(Dependency.DependencyFile) Then
                            LogView.AppendText("    - Dependency: " & Quote & Path.GetFileName(Dependency.DependencyFile) & Quote & CrLf)
                            CommandArgs &= " /dependencypackagepath=" & Quote & Dependency.DependencyFile & Quote
                        Else
                            LogView.AppendText(CrLf & _
                                               "Warning: the dependency" & CrLf & _
                                               Quote & Dependency.DependencyFile & Quote & CrLf & _
                                               "does not exist in the file system. Skipping dependency...")
                            Continue For
                        End If
                    Next
                    If appxAdditionPackageList(x).PackageCustomDataFile <> "" And File.Exists(appxAdditionPackageList(x).PackageCustomDataFile) Then
                        CommandArgs &= " /customdatapath=" & Quote & appxAdditionCustomDataFile & Quote
                    ElseIf appxAdditionPackageList(x).PackageCustomDataFile <> "" And Not File.Exists(appxAdditionPackageList(x).PackageCustomDataFile) Then
                        LogView.AppendText(CrLf & _
                                           "Warning: the custom data file does not exist. Continuing without one...")
                    End If
                    If (FileVersionInfo.GetVersionInfo(DismProgram).ProductMajorPart = 10 And FileVersionInfo.GetVersionInfo(DismProgram).ProductBuildPart >= 17134) And
                        (ImgVersion.Major = 10 And ImgVersion.Build >= 17134) Then
                        If appxAdditionPackageList(x).PackageRegions = "" Then
                            CommandArgs &= " /region:all"
                        Else
                            CommandArgs &= " /region:" & Quote & appxAdditionPackageList(x).PackageRegions & Quote
                        End If
                    End If
                    If (FileVersionInfo.GetVersionInfo(DismProgram).ProductMajorPart >= 10 And ImgVersion.Major >= 10) And appxAdditionPackageList(x).SupportsStub Then
                        Select Case appxAdditionPackageList(x).StubPackageOption
                            Case StubPreference.NoPreference
                                ' Don't add stub package option flag
                            Case StubPreference.StubOnly
                                CommandArgs &= " /stubpackageoption:installstub"
                            Case StubPreference.FullPackage
                                CommandArgs &= " /stubpackageoption:installfull"
                        End Select
                    End If
                    DISMProc.StartInfo.Arguments = CommandArgs
                    DISMProc.Start()
                    DISMProc.WaitForExit()
                End If
                LogView.AppendText(CrLf & "Getting error level...")
                If Hex(DISMProc.ExitCode).Length < 8 Then
                    errCode = DISMProc.ExitCode
                Else
                    errCode = Hex(DISMProc.ExitCode)
                End If
                If DISMProc.ExitCode = 0 Then
                    appxSuccessfulAdditions += 1
                Else
                    appxFailedAdditions += 1
                End If
                If errCode.Length >= 8 Then
                    LogView.AppendText(" Error level : 0x" & errCode)
                Else
                    LogView.AppendText(" Error level : " & errCode)
                End If
                If PkgErrorText.RichTextBox1.Text = "" Then
                    If errCode.Length >= 8 Then
                        PkgErrorText.RichTextBox1.AppendText("0x" & errCode)
                    Else
                        PkgErrorText.RichTextBox1.AppendText(errCode)
                    End If
                Else
                    If errCode.Length >= 8 Then
                        PkgErrorText.RichTextBox1.AppendText(CrLf & "0x" & errCode)
                    Else
                        PkgErrorText.RichTextBox1.AppendText(CrLf & errCode)
                    End If
                End If
            Next
            CurrentPB.Value = CurrentPB.Maximum
            LogView.AppendText(CrLf & "Gathering error level for selected AppX packages..." & CrLf)
            For x = 0 To PkgErrorText.RichTextBox1.Lines.Count - 1
                LogView.AppendText(CrLf & "- Package no. " & (x + 1) & ": " & PkgErrorText.RichTextBox1.Lines(x))
            Next
            Thread.Sleep(2000)
            If appxAdditionCommit Then
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
        ElseIf opNum = 38 Then
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
                            allTasks.Text = "Rimozione dei pacchetti AppX..."
                            currentTask.Text = "Preparazione alla rimozione dei pacchetti AppX approvvigionati..."
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
                    allTasks.Text = "Rimozione dei pacchetti AppX..."
                    currentTask.Text = "Preparazione alla rimozione dei pacchetti AppX approvvigionati..."
            End Select
            LogView.AppendText(CrLf & "Removing provisioned AppX packages..." & CrLf & CrLf & _
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
                            currentTask.Text = "Rimozione dei pacchetti AppX..."
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
                    currentTask.Text = "Rimozione dei pacchetti AppX..."
            End Select
            CurrentPB.Maximum = appxRemovalCount
            For x = 0 To Array.LastIndexOf(appxRemovalPackages, appxRemovalLastPackage)
                If x + 1 > CurrentPB.Maximum Then Exit For
                CommandArgs = BckArgs
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
                                currentTask.Text = "Rimozione del pacchetto " & (x + 1) & " di " & appxRemovalCount & "..."
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
                        currentTask.Text = "Rimozione del pacchetto " & (x + 1) & " di " & appxRemovalCount & "..."
                End Select
                LogView.AppendText(CrLf & _
                                   "Package " & (x + 1) & " of " & appxRemovalCount)
                CurrentPB.Value = x + 1
                ' Display package name and DisplayName
                LogView.AppendText(CrLf & _
                                   "- Package name: " & appxRemovalPackages(x) & CrLf & _
                                   "- Display name: " & appxRemovalPkgNames(x))
                ' Display whether an application is registered to a user
                If Directory.Exists(MountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & appxRemovalPackages(x)) Then
                    If My.Computer.FileSystem.GetFiles(MountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & appxRemovalPackages(x), FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").Count = 0 Then
                        ' Application is not registered to any user
                        LogView.AppendText(CrLf & _
                                           "- Application is registered to a user? No")
                    Else
                        ' Application is registered to a user
                        LogView.AppendText(CrLf & _
                                           "- Application is registered to a user? Yes" & CrLf & _
                                           "  The removal of this application may require you to use PowerShell to completely remove it")
                    End If
                Else
                    ' Application is not registered to any user
                    LogView.AppendText(CrLf & _
                                       "- Application is registered to a user? No")
                End If
                ' Initialize command. Its syntax is simple, so don't spend too much time determining options
                LogView.AppendText(CrLf & CrLf & _
                                   "Processing package...")
                DISMProc.StartInfo.FileName = DismProgram
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /remove-provisionedappxpackage /packagename=" & appxRemovalPackages(x)
                DISMProc.StartInfo.Arguments = CommandArgs
                DISMProc.Start()
                DISMProc.WaitForExit()
                LogView.AppendText(CrLf & "Getting error level...")
                If Hex(DISMProc.ExitCode).Length < 8 Then
                    errCode = DISMProc.ExitCode
                Else
                    errCode = Hex(DISMProc.ExitCode)
                End If
                If DISMProc.ExitCode = 0 Then
                    appxSuccessfulRemovals += 1
                Else
                    appxFailedRemovals += 1
                End If
                If errCode.Length >= 8 Then
                    LogView.AppendText(" Error level : 0x" & errCode)
                Else
                    LogView.AppendText(" Error level : " & errCode)
                End If
                If PkgErrorText.RichTextBox1.Text = "" Then
                    If errCode.Length >= 8 Then
                        PkgErrorText.RichTextBox1.AppendText("0x" & errCode)
                    Else
                        PkgErrorText.RichTextBox1.AppendText(errCode)
                    End If
                Else
                    If errCode.Length >= 8 Then
                        PkgErrorText.RichTextBox1.AppendText(CrLf & "0x" & errCode)
                    Else
                        PkgErrorText.RichTextBox1.AppendText(CrLf & errCode)
                    End If
                End If
            Next
            CurrentPB.Value = CurrentPB.Maximum
            LogView.AppendText(CrLf & "Gathering error level for selected AppX packages..." & CrLf)
            For x = 0 To PkgErrorText.RichTextBox1.Lines.Count - 1
                LogView.AppendText(CrLf & "- Package no. " & (x + 1) & ": " & PkgErrorText.RichTextBox1.Lines(x))
            Next
            Thread.Sleep(2000)
            AllPB.Value = 100
            If appxSuccessfulRemovals > 0 Then
                GetErrorCode(True)
            ElseIf appxSuccessfulRemovals <= 0 Then
                GetErrorCode(False)
            End If
        ElseIf opNum = 60 Then
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
                            allTasks.Text = "Impostazione del driver stratificato..."
                            currentTask.Text = "Impostazione del driver a strati per la tastiera..."
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
                    allTasks.Text = "Impostazione del driver stratificato..."
                    currentTask.Text = "Impostazione del driver a strati per la tastiera..."
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
            LogView.AppendText(CrLf & "Setting the keyboard layered driver..." & CrLf & _
                               "- Current keyboard layered driver: " & currentLayout & CrLf & _
                               "- New keyboard layered driver: " & newLayout & CrLf)
            DISMProc.StartInfo.FileName = DismProgram
            CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /set-layereddriver:" & KeyboardLayeredDriverType
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
            LogView.AppendText(CrLf & "Getting error level...")
            If Hex(DISMProc.ExitCode).Length < 8 Then
                errCode = DISMProc.ExitCode
            Else
                errCode = Hex(DISMProc.ExitCode)
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(" Error level : 0x" & errCode)
            Else
                LogView.AppendText(" Error level : " & errCode)
            End If
            GetErrorCode(False)
        ElseIf opNum = 64 Then
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
                            allTasks.Text = "Aggiunta di capacità..."
                            currentTask.Text = "Preparazione all'aggiunta di capacità..."
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
                    allTasks.Text = "Aggiunta di capacità..."
                    currentTask.Text = "Preparazione all'aggiunta di capacità..."
            End Select
            LogView.AppendText(CrLf & "Adding capabilities to mounted image..." & CrLf & _
                               "Options:" & CrLf & _
                               "- Use a source for capability addition? " & If(capAdditionUseSource, "Yes", "No") & CrLf & _
                               "- Capability source: " & If(capAdditionUseSource, Quote & capAdditionSource & Quote, "No source has been provided") & CrLf & _
                               "- Limit access to Windows Update? " & If(capAdditionLimitWUAccess And OnlineMgmt, "Yes", If(capAdditionLimitWUAccess And Not OnlineMgmt, "No, this is not an online installation", "No")) & If(Not capAdditionLimitWUAccess And OnlineMgmt And SystemInformation.BootMode = BootMode.FailSafe, ", the system is in Safe Mode", "") & CrLf & _
                               "- Commit image after adding capabilities? " & If(capAdditionCommit, "Yes", "No") & CrLf)
            If capAdditionUseSource And Not Directory.Exists(capAdditionSource) Then
                LogView.AppendText(CrLf & _
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
                            currentTask.Text = "Aggiunta di capacità..."
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
                    currentTask.Text = "Aggiunta di capacità..."
            End Select
            LogView.AppendText(CrLf & "Enumerating capabilities to add. Please wait..." & CrLf & _
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
                                currentTask.Text = "Aggiunta della capacità " & (x + 1) & " di " & capAdditionCount & "..."
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
                        currentTask.Text = "Aggiunta della capacità " & (x + 1) & " di " & capAdditionCount & "..."
                End Select
                CurrentPB.Value = x + 1
                LogView.AppendText(CrLf & _
                                   "Capability " & (x + 1) & " of " & capAdditionCount)
                ' Get capability information
                ' Try opening the session. If API is not initialized, initialize it
                Try
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                        ' Get capability information
                        Dim capInfo As DismCapabilityInfo = DismApi.GetCapabilityInfo(imgSession, capAdditionIds(x))
                        LogView.AppendText(CrLf & CrLf & _
                                           "- Capability identity: " & capInfo.Name & CrLf & _
                                           "- Capability name: " & capInfo.DisplayName & CrLf & _
                                           "- Capability description: " & capInfo.Description & CrLf)
                    End Using
                Finally
                    DismApi.Shutdown()
                End Try
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /add-capability /capabilityname=" & capAdditionIds(x)
                If capAdditionUseSource And Directory.Exists(capAdditionSource) Then
                    CommandArgs &= " /source=" & Quote & capAdditionSource & Quote
                End If
                If capAdditionLimitWUAccess And OnlineMgmt Then CommandArgs &= " /limitaccess"
                DISMProc.StartInfo.FileName = DismProgram
                DISMProc.StartInfo.Arguments = CommandArgs
                DISMProc.Start()
                DISMProc.WaitForExit()
                LogView.AppendText(CrLf & "Getting error level...")
                errCode = Hex(Decimal.ToInt32(DISMProc.ExitCode))
                If DISMProc.ExitCode = 0 Then
                    capSuccessfulAdditions += 1
                Else
                    capFailedAdditions += 1
                End If
                If errCode.Length >= 8 Then
                    LogView.AppendText(" Error level : 0x" & errCode)
                Else
                    LogView.AppendText(" Error level : " & errCode)
                End If
                If FeatErrorText.RichTextBox1.Text = "" Then
                    If errCode.Length >= 8 Then
                        FeatErrorText.RichTextBox1.AppendText("0x" & errCode)
                    Else
                        FeatErrorText.RichTextBox1.AppendText(errCode)
                    End If
                Else
                    If errCode.Length >= 8 Then
                        FeatErrorText.RichTextBox1.AppendText(CrLf & "0x" & errCode)
                    Else
                        FeatErrorText.RichTextBox1.AppendText(CrLf & errCode)
                    End If
                End If
            Next
            CurrentPB.Value = CurrentPB.Maximum
            LogView.AppendText(CrLf & "Gathering error level for selected capabilities..." & CrLf)
            For x = 0 To FeatErrorText.RichTextBox1.Lines.Count - 1
                LogView.AppendText(CrLf & "- Capability no. " & (x + 1) & ": " & FeatErrorText.RichTextBox1.Lines(x))
            Next
            Thread.Sleep(2000)
            If capAdditionCommit Then
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
            If FeatErrorText.RichTextBox1.Text.Contains("BC2") Then
                LogView.AppendText(CrLf & "Some capabilities require a system restart to be fully processed. Save your work, close your programs, and restart when ready")
            End If
        ElseIf opNum = 68 Then
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
                            allTasks.Text = "Rimozione delle capacità..."
                            currentTask.Text = "Preparazione alla rimozione delle capacità..."
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
                    allTasks.Text = "Rimozione delle capacità..."
                    currentTask.Text = "Preparazione alla rimozione delle capacità..."
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
                            currentTask.Text = "Rimozione delle capacità..."
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
                    currentTask.Text = "Rimozione delle capacità..."
            End Select
            LogView.AppendText(CrLf & "Enumerating capabilities to remove. Please wait..." & CrLf & _
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
                                currentTask.Text = "Rimozione della capacità " & (x + 1) & " di " & capRemovalCount & "..."
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
                        currentTask.Text = "Rimozione della capacità " & (x + 1) & " di " & capRemovalCount & "..."
                End Select
                CurrentPB.Value = x + 1
                LogView.AppendText(CrLf & _
                                   "Capability " & (x + 1) & " of " & capRemovalCount)
                Try
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                        Dim capInfo As DismCapabilityInfo = DismApi.GetCapabilityInfo(imgSession, capRemovalIds(x))
                        LogView.AppendText(CrLf & CrLf & _
                                           "- Capability identity: " & capInfo.Name & CrLf & _
                                           "- Capability name: " & capInfo.DisplayName & CrLf & _
                                           "- Capability description: " & capInfo.Description & CrLf)
                    End Using
                Finally
                    DismApi.Shutdown()
                End Try
                DISMProc.StartInfo.FileName = DismProgram
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /norestart /remove-capability /capabilityname=" & capRemovalIds(x)
                DISMProc.StartInfo.Arguments = CommandArgs
                DISMProc.Start()
                DISMProc.WaitForExit()
                LogView.AppendText(CrLf & "Getting error level...")
                errCode = Hex(Decimal.ToInt32(DISMProc.ExitCode))
                If DISMProc.ExitCode = 0 Then
                    capSuccessfulRemovals += 1
                Else
                    capFailedRemovals += 1
                End If
                If errCode.Length >= 8 Then
                    LogView.AppendText(" Error level : 0x" & errCode)
                Else
                    LogView.AppendText(" Error level : " & errCode)
                End If
                If FeatErrorText.RichTextBox1.Text = "" Then
                    If errCode.Length >= 8 Then
                        FeatErrorText.RichTextBox1.AppendText("0x" & errCode)
                    Else
                        FeatErrorText.RichTextBox1.AppendText(errCode)
                    End If
                Else
                    If errCode.Length >= 8 Then
                        FeatErrorText.RichTextBox1.AppendText(CrLf & "0x" & errCode)
                    Else
                        FeatErrorText.RichTextBox1.AppendText(CrLf & errCode)
                    End If
                End If
            Next
            CurrentPB.Value = CurrentPB.Maximum
            LogView.AppendText(CrLf & "Gathering error level for selected capabilities..." & CrLf)
            For x = 0 To FeatErrorText.RichTextBox1.Lines.Count - 1
                LogView.AppendText(CrLf & "- Capability no. " & (x + 1) & ": " & FeatErrorText.RichTextBox1.Lines(x))
            Next
            Thread.Sleep(2000)
            If capSuccessfulRemovals > 0 Then
                GetErrorCode(True)
            ElseIf capSuccessfulRemovals <= 0 Then
                GetErrorCode(False)
            End If
            If FeatErrorText.RichTextBox1.Text.Contains("BC2") Then
                LogView.AppendText(CrLf & "Some capabilities require a system restart to be fully processed. Save your work, close your programs, and restart when ready")
            End If
        ElseIf opNum = 75 Then
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
                            allTasks.Text = "Aggiunta di driver..."
                            currentTask.Text = "Preparazione all'aggiunta dei driver..."
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
                    allTasks.Text = "Aggiunta di driver..."
                    currentTask.Text = "Preparazione all'aggiunta dei driver..."
            End Select
            LogView.AppendText(CrLf & "Adding driver packages to mounted image..." & CrLf & _
                               "Options:" & CrLf & _
                               "- Force installation of unsigned drivers? " & If(drvAdditionForceUnsigned, "Yes", "No") & CrLf & _
                               "- Commit image after adding driver packages? " & If(drvAdditionCommit, "Yes", "No") & CrLf)
            If drvAdditionForceUnsigned Then
                LogView.AppendText(CrLf & _
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
                            currentTask.Text = "Aggiunta di driver..."
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
                    currentTask.Text = "Aggiunta di driver..."
            End Select
            LogView.AppendText(CrLf & "Enumerating drivers to add. Please wait..." & CrLf & _
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
                                currentTask.Text = "Aggiunta del driver " & (x + 1) & " di " & drvAdditionCount & "..."
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
                        currentTask.Text = "Aggiunta del driver " & (x + 1) & " di " & drvAdditionCount & "..."
                End Select
                CurrentPB.Value = x + 1
                LogView.AppendText(CrLf & _
                                   "Driver " & (x + 1) & " of " & drvAdditionCount)
                ' Get driver information
                If Not File.GetAttributes(drvAdditionPkgs(x)) = FileAttributes.Directory Then
                    Try
                        DismApi.Initialize(DismLogLevel.LogErrors)
                        Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                            Dim drvInfoCollection As DismDriverCollection = DismApi.GetDriverInfo(imgSession, drvAdditionPkgs(x))
                            If drvInfoCollection.Count > 0 And drvInfoCollection.Count <= 10 Then
                                For Each drvInfo As DismDriver In drvInfoCollection
                                    LogView.AppendText(CrLf & CrLf & _
                                                       "- Hardware description: " & drvInfo.HardwareDescription & CrLf & _
                                                       "- Hardware ID: " & drvInfo.HardwareId & CrLf & _
                                                       "- Additional IDs" & CrLf & _
                                                       "  - Compatible IDs: " & drvInfo.CompatibleIds & CrLf & _
                                                       "  - Excluded IDs: " & drvInfo.ExcludeIds & CrLf & _
                                                       "- Hardware manufacturer: " & drvInfo.ManufacturerName & CrLf & _
                                                       "- Hardware architecture: " & Casters.CastDismArchitecture(drvInfo.Architecture))
                                Next
                            ElseIf drvInfoCollection.Count > 10 Then
                                LogView.AppendText(CrLf & CrLf & _
                                                   "This driver file targets more than 10 devices. To avoid creating log files large in size, we will not show information of this driver package, and will proceed anyway." & CrLf & _
                                                   "If you want to get information of this driver package, go to Commands > Drivers > Get driver information > I want to get information about driver files, and specify this driver file:" & CrLf & CrLf & _
                                                   "    " & Path.GetFileName(drvAdditionPkgs(x)))
                            Else
                                LogView.AppendText(CrLf & CrLf & _
                                                   "We couldn't get information of this driver package. Proceeding anyway...")
                            End If
                        End Using
                    Finally
                        DismApi.Shutdown()
                    End Try
                Else
                    LogView.AppendText(CrLf & CrLf & _
                                       "The driver package currently about to be processed is a folder, so information about it can't be obtained. Proceeding anyway...")
                End If
                DISMProc.StartInfo.FileName = DismProgram
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /add-driver /driver=" & Quote & drvAdditionPkgs(x) & Quote
                If drvAdditionForceUnsigned Then
                    CommandArgs &= " /forceunsigned"
                End If
                If File.GetAttributes(drvAdditionPkgs(x)) = FileAttributes.Directory And drvAdditionFolderRecursiveScan.Contains(drvAdditionPkgs(x)) Then
                    LogView.AppendText(CrLf & "This folder will be scanned recursively. Driver addition may take a longer time...")
                    CommandArgs &= " /recurse"
                End If
                DISMProc.StartInfo.Arguments = CommandArgs
                DISMProc.Start()
                DISMProc.WaitForExit()
                LogView.AppendText(CrLf & "Getting error level...")
                errCode = Hex(Decimal.ToInt32(DISMProc.ExitCode))
                If DISMProc.ExitCode = 0 Then
                    drvSuccessfulAdditions += 1
                Else
                    drvFailedAdditions += 1
                End If
                If errCode.Length >= 8 Then
                    LogView.AppendText(" Error level : 0x" & errCode)
                Else
                    LogView.AppendText(" Error level : " & errCode)
                End If
                If PkgErrorText.RichTextBox1.Text = "" Then
                    If errCode.Length >= 8 Then
                        PkgErrorText.RichTextBox1.AppendText("0x" & errCode)
                    Else
                        PkgErrorText.RichTextBox1.AppendText(errCode)
                    End If
                Else
                    If errCode.Length >= 8 Then
                        PkgErrorText.RichTextBox1.AppendText(CrLf & "0x" & errCode)
                    Else
                        PkgErrorText.RichTextBox1.AppendText(CrLf & errCode)
                    End If
                End If
            Next
            CurrentPB.Value = CurrentPB.Maximum
            LogView.AppendText(CrLf & "Gathering error level for selected drivers..." & CrLf)
            For x = 0 To PkgErrorText.RichTextBox1.Lines.Count - 1
                LogView.AppendText(CrLf & "- Driver no. " & (x + 1) & ": " & PkgErrorText.RichTextBox1.Lines(x))
            Next
            Thread.Sleep(2000)
            If drvAdditionCommit Then
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
        ElseIf opNum = 76 Then
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
                            allTasks.Text = "Rimozione dei driver..."
                            currentTask.Text = "Preparazione alla rimozione dei driver..."
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
                    allTasks.Text = "Rimozione dei driver..."
                    currentTask.Text = "Preparazione alla rimozione dei driver..."
            End Select
            LogView.AppendText(CrLf & "Removing driver packages from mounted image..." & CrLf)
            ' Get all driver packages
            LogView.AppendText(CrLf & "Getting image drivers. This may take some time..." & CrLf)
            Try
                DismApi.Initialize(DismLogLevel.LogErrors)
                Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                    drvCollection = DismApi.GetDrivers(imgSession, AllDrivers)
                End Using
            Finally
                DismApi.Shutdown()
            End Try
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
                            currentTask.Text = "Rimozione dei driver..."
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
                    currentTask.Text = "Rimozione dei driver..."
            End Select
            LogView.AppendText(CrLf & "Enumerating drivers to remove. Please wait..." & CrLf & _
                               "Total number of drivers: " & drvRemovalCount)
            CurrentPB.Maximum = drvRemovalCount
            For x = 0 To Array.LastIndexOf(drvRemovalPkgs, drvRemovalLastPkg)
                If x + 1 > CurrentPB.Maximum Then Exit For
                CommandArgs = BckArgs
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
                                currentTask.Text = "Rimozione del driver " & (x + 1) & " di " & drvRemovalCount & "..."
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
                        currentTask.Text = "Rimozione del driver " & (x + 1) & " di " & drvRemovalCount & "..."
                End Select
                CurrentPB.Value = x + 1
                LogView.AppendText(CrLf & _
                                   "Driver " & (x + 1) & " of " & drvRemovalCount)
                ' Get driver information
                Try
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                        For Each drv As DismDriverPackage In drvCollection
                            If drv.PublishedName = drvRemovalPkgs(x) Then
                                LogView.AppendText(CrLf & CrLf & _
                                                   "- Published name: " & drv.PublishedName & CrLf & _
                                                   "- Provider name: " & drv.ProviderName & CrLf & _
                                                   "- Class name: " & drv.ClassName & CrLf & _
                                                   "- Class description: " & drv.ClassDescription & CrLf & _
                                                   "- Class GUID: " & drv.ClassGuid & CrLf & _
                                                   "- Version and date: " & drv.Version.ToString() & " / " & drv.Date.ToString() & CrLf & _
                                                   "- Is part of the Windows distribution? " & If(drv.InBox, "Yes", "No") & CrLf & _
                                                   "- Is critical to the boot process? " & If(drv.BootCritical, "Yes", "No"))
                                If drv.InBox Then
                                    LogView.AppendText(CrLf & CrLf & _
                                                       "Warning: this driver package is part of the Windows distribution. Some areas may no longer work after this driver has been removed")
                                End If
                                If drv.BootCritical Then
                                    LogView.AppendText(CrLf & CrLf & _
                                                       "Warning: this driver package is critical to the boot process. The target image may no longer boot or work correctly after this driver has been removed")
                                End If
                                Exit For
                            End If
                        Next
                    End Using
                Finally
                    DismApi.Shutdown()
                End Try
                DISMProc.StartInfo.FileName = DismProgram
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /remove-driver /driver=" & Quote & drvRemovalPkgs(x) & Quote
                DISMProc.StartInfo.Arguments = CommandArgs
                DISMProc.Start()
                DISMProc.WaitForExit()
                LogView.AppendText(CrLf & "Getting error level...")
                errCode = Hex(Decimal.ToInt32(DISMProc.ExitCode))
                If DISMProc.ExitCode = 0 Then
                    drvSuccessfulRemovals += 1
                Else
                    drvFailedRemovals += 1
                End If
                If errCode.Length >= 8 Then
                    LogView.AppendText(" Error level : 0x" & errCode)
                Else
                    LogView.AppendText(" Error level : " & errCode)
                End If
                If PkgErrorText.RichTextBox1.Text = "" Then
                    If errCode.Length >= 8 Then
                        PkgErrorText.RichTextBox1.AppendText("0x" & errCode)
                    Else
                        PkgErrorText.RichTextBox1.AppendText(errCode)
                    End If
                Else
                    If errCode.Length >= 8 Then
                        PkgErrorText.RichTextBox1.AppendText(CrLf & "0x" & errCode)
                    Else
                        PkgErrorText.RichTextBox1.AppendText(CrLf & errCode)
                    End If
                End If
            Next
            CurrentPB.Value = CurrentPB.Maximum
            LogView.AppendText(CrLf & "Gathering error level for selected drivers..." & CrLf)
            For x = 0 To PkgErrorText.RichTextBox1.Lines.Count - 1
                LogView.AppendText(CrLf & "- Driver no. " & (x + 1) & ": " & PkgErrorText.RichTextBox1.Lines(x))
            Next
            Thread.Sleep(2000)
            If drvSuccessfulRemovals > 0 Then
                GetErrorCode(True)
            ElseIf drvSuccessfulRemovals <= 0 Then
                GetErrorCode(False)
            End If
        ElseIf opNum = 77 Then
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
                            allTasks.Text = "Esportazione dei driver..."
                            currentTask.Text = "Esportazione di driver di terze parti nella cartella specificata..."
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
                    allTasks.Text = "Esportazione dei driver..."
                    currentTask.Text = "Esportazione di driver di terze parti nella cartella specificata..."
            End Select
            LogView.AppendText(CrLf & "Exporting drivers to specified folder..." & CrLf & _
                               "- Export target: " & Quote & drvExportTarget & Quote)
            ' Check the DISM version, as the Windows 7 version doesn't allow this action
            DISMProc.StartInfo.FileName = DismProgram
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
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
            LogView.AppendText(CrLf & "Getting error level...")
            If Hex(DISMProc.ExitCode).Length < 8 Then
                errCode = DISMProc.ExitCode
            Else
                errCode = Hex(DISMProc.ExitCode)
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(" Error level : 0x" & errCode)
            Else
                LogView.AppendText(" Error level : " & errCode)
            End If
            GetErrorCode(False)
        ElseIf opNum = 78 Then
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
                            allTasks.Text = "Importazione dei driver..."
                            currentTask.Text = "Preparazione all'importazione di driver di terze parti..."
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
                            currentTask.Text = "Esportazione di driver di terze parti dall'origine di importazione dei driver..."
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
                Directory.CreateDirectory(Application.StartupPath & "\export_temp")
            Catch ex As Exception
                LogView.AppendText(CrLf & "The temporary folder could not be created. See below for reasons why:" & CrLf & CrLf & ex.ToString() & "-" & ex.Message)
            End Try
            If Directory.Exists(Application.StartupPath & "\export_temp") Then
                LogView.AppendText(CrLf & "Exporting third-party drivers from import source..." & CrLf)
                CommandArgs &= If(ImportSourceInt = 1, " /online", " /image=" & targetImage) & " /export-driver /destination=" & Quote & Application.StartupPath & "\export_temp" & Quote
                DISMProc.StartInfo.FileName = DismProgram
                DISMProc.StartInfo.Arguments = CommandArgs
                DISMProc.Start()
                DISMProc.WaitForExit()
                LogView.AppendText(CrLf & "Getting error level...")
                If Hex(DISMProc.ExitCode).Length < 8 Then
                    errCode = DISMProc.ExitCode
                Else
                    errCode = Hex(DISMProc.ExitCode)
                End If
                If errCode.Length >= 8 Then
                    LogView.AppendText(" Error level : 0x" & errCode)
                Else
                    LogView.AppendText(" Error level : " & errCode)
                End If
                If DISMProc.ExitCode = 0 Then
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
                                    currentTask.Text = "Importazione di driver di terze parti nell'immagine di destinazione..."
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
                            currentTask.Text = "Importazione di driver di terze parti nell'immagine di destinazione..."
                    End Select
                    LogView.AppendText(CrLf & "Importing third-party drivers from the temporary export directory to the destination image...")
                    CommandArgs = BckArgs
                    CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /add-driver /driver=" & Quote & Application.StartupPath & "\export_temp" & Quote & " /recurse"
                    DISMProc.StartInfo.Arguments = CommandArgs
                    DISMProc.Start()
                    DISMProc.WaitForExit()
                    If Hex(DISMProc.ExitCode).Length < 8 Then
                        errCode = DISMProc.ExitCode
                    Else
                        errCode = Hex(DISMProc.ExitCode)
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
                    Directory.Delete(Application.StartupPath & "\export_temp", True)
                Catch ex As Exception
                    LogView.AppendText(CrLf & "We couldn't delete the temporary export directory. You'll need to delete the " & Quote & "export_temp" & Quote & " directory manually.")
                End Try
            End If
        ElseIf opNum = 79 Then
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
                            allTasks.Text = "Applicazione del file di risposta non presidiato..."
                            currentTask.Text = "Applicazione del file di risposta non presidiato specificato all'immagine di destinazione..."
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
                    currentTask.Text = "Applicazione del file di risposta non presidiato specificato all'immagine di destinazione..."
            End Select
            LogView.AppendText(CrLf & "Applying unattended answer file. Options:" & CrLf & _
                               "- Unattended answer file: " & UnattendedFile)

            ' Initialize command
            DISMProc.StartInfo.FileName = DismProgram
            CommandArgs &= If(OnlineMgmt, " /online", " /image=" & targetImage) & " /apply-unattend=" & Quote & UnattendedFile & Quote
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
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
                            currentTask.Text = "Raccolta del livello di errore..."
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
        ElseIf opNum = 83 Then
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
                            allTasks.Text = "Impostazione dello spazio temporaneo..."
                            currentTask.Text = "Impostazione dello spazio temporaneo di Windows PE..."
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
            LogView.AppendText(CrLf & "Setting the Windows PE scratch space..." & CrLf & _
                               "- New scratch space amount: " & peNewScratchSpace & " MB")
            CommandArgs &= " /image=" & targetImage & " /set-scratchspace=" & peNewScratchSpace
            DISMProc.StartInfo.FileName = DismProgram
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
            LogView.AppendText(CrLf & "Getting error level...")
            If Hex(DISMProc.ExitCode).Length < 8 Then
                errCode = DISMProc.ExitCode
            Else
                errCode = Hex(DISMProc.ExitCode)
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(" Error level : 0x" & errCode)
            Else
                LogView.AppendText(" Error level : " & errCode)
            End If
            GetErrorCode(False)
        ElseIf opNum = 84 Then
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
                            allTasks.Text = "Impostazione del percorso di destinazione..."
                            currentTask.Text = "Impostazione del percorso di destinazione di Windows PE..."
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
                    allTasks.Text = "Impostazione del percorso di destinazione..."
                    currentTask.Text = "Impostazione del percorso di destinazione di Windows PE..."
            End Select
            LogView.AppendText(CrLf & "Setting the Windows PE target path..." & CrLf & _
                               "- New target path: " & Quote & peNewTargetPath & Quote)
            CommandArgs &= " /image=" & targetImage & " /set-targetpath=" & peNewTargetPath
            DISMProc.StartInfo.FileName = DismProgram
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
            LogView.AppendText(CrLf & "Getting error level...")
            If Hex(DISMProc.ExitCode).Length < 8 Then
                errCode = DISMProc.ExitCode
            Else
                errCode = Hex(DISMProc.ExitCode)
            End If
            If errCode.Length >= 8 Then
                LogView.AppendText(" Error level : 0x" & errCode)
            Else
                LogView.AppendText(" Error level : " & errCode)
            End If
            GetErrorCode(False)
        ElseIf opNum = 86 Then
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
                            currentTask.Text = "Preparazione del ripristino del sistema operativo..."
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
            DISMProc.StartInfo.FileName = DismProgram
            CommandArgs = " /online /norestart /initiate-osuninstall"
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
            LogView.AppendText(CrLf & "Gathering error level...")
            GetErrorCode(False)
            If errCode.Length >= 8 Then
                LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
            Else
                LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
            End If
        ElseIf opNum = 87 Then
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
                            allTasks.Text = "Rimozione della possibilità di ritorno al sistema operativo..."
                            currentTask.Text = "Rimozione della possibilità di tornare a una vecchia installazione di Windows..."
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
                    allTasks.Text = "Rimozione della possibilità di ritorno al sistema operativo..."
                    currentTask.Text = "Rimozione della possibilità di tornare a una vecchia installazione di Windows..."
            End Select
            LogView.AppendText(CrLf & "Removing the ability to revert to an old installation of Windows...")
            DISMProc.StartInfo.FileName = DismProgram
            CommandArgs &= " /online /remove-osuninstall"
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
            LogView.AppendText(CrLf & "Gathering error level...")
            GetErrorCode(False)
            If errCode.Length >= 8 Then
                LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
            Else
                LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
            End If
        ElseIf opNum = 88 Then
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
                            allTasks.Text = "Impostazione della finestra di disinstallazione..."
                            currentTask.Text = "Impostazione del numero di giorni in cui può avvenire la disinstallazione..."
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
            DISMProc.StartInfo.FileName = DismProgram
            CommandArgs &= " /online /set-osuninstallwindow /value:" & osUninstDayCount
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
            LogView.AppendText(CrLf & "Gathering error level...")
            GetErrorCode(False)
            If errCode.Length >= 8 Then
                LogView.AppendText(CrLf & CrLf & "    Error level : 0x" & errCode)
            Else
                LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
            End If
        ElseIf opNum = 991 Then
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
                            currentTask.Text = "Conversione dell'immagine specificata..."
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
            LogView.AppendText("- Source image file: " & imgSrcFile & CrLf & _
                               "- Index to convert: " & imgConversionIndex & CrLf & _
                               "- Destination image file: " & imgDestFile & CrLf)
            If imgConversionMode = 0 Then
                LogView.AppendText("- Image conversion mode: Windows Imaging (WIM) --> Electronic Software Distribution (ESD)")
            ElseIf imgConversionMode = 1 Then
                LogView.AppendText("- Image conversion mode: Electronic Software Distribution (ESD) --> Windows Imaging (WIM)")
            End If

            ' Run commands
            DISMProc.StartInfo.FileName = DismProgram
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
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
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
                            currentTask.Text = "Raccolta del livello di errore..."
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
        ElseIf opNum = 992 Then
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
                            allTasks.Text = "Unione dei file SWM..."
                            currentTask.Text = "Unione dei file SWM in un file WIM..."
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
            LogView.AppendText(CrLf & "Merging SWM files into a WIM file..." & CrLf & _
                               "Options:" & CrLf)
            ' Gather options
            LogView.AppendText("- Source image file: " & imgSwmSource & CrLf & _
                               "- Target index: " & imgMergerIndex & CrLf & _
                               "- Destination image file: " & imgWimDestination & CrLf)

            ' Run commands
            DISMProc.StartInfo.FileName = DismProgram
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
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
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
                            currentTask.Text = "Raccolta del livello di errore..."
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
        ElseIf opNum = 996 Then
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
                            allTasks.Text = "Cambio degli indici delle immagini..."
                            currentTask.Text = "Smontaggio dell'indice di origine..."
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
                    allTasks.Text = "Cambio degli indici delle immagini..."
                    currentTask.Text = "Smontaggio dell'indice di origine..."
            End Select
            LogView.AppendText(CrLf & "Switching image indexes..." & CrLf & _
                               "Options:" & CrLf)
            ' Gather options
            LogView.AppendText("- Target mount directory: " & SwitchTarget & CrLf & _
                               "- Source image index: " & SwitchSourceIndex & CrLf & _
                               "- Target image index: " & SwitchTargetIndex & " (" & SwitchTargetIndexName & ")")
            If SwitchCommitSourceIndex Then
                LogView.AppendText(CrLf & "- Commit source index? Yes")
            Else
                LogView.AppendText(CrLf & "- Commit source index? No")
            End If
            ' Run commands
            DISMProc.StartInfo.FileName = DismProgram
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
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
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
                            currentTask.Text = "Raccolta del livello di errore..."
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
            If Decimal.ToInt32(DISMProc.ExitCode) <> 0 Then
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
                                currentTask.Text = "Smontaggio dell'indice di origine..."
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
                        currentTask.Text = "Smontaggio dell'indice di origine..."
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
                DISMProc.StartInfo.Arguments = CommandArgs
                DISMProc.Start()
                DISMProc.WaitForExit()
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
                                currentTask.Text = "Raccolta del livello di errore..."
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
                If Decimal.ToInt32(DISMProc.ExitCode) <> 0 Then
                    Exit Sub
                End If
            End If
            AllPB.Value = AllPB.Maximum / taskCount
            currentTCont += 1
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
                            currentTask.Text = "Montaggio indice di destinazione..."
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
                    currentTask.Text = "Montaggio indice di destinazione..."
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
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
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
                            currentTask.Text = "Raccolta del livello di errore..."
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
        End If
        CurrentPB.Value = CurrentPB.Maximum
        AllPB.Value = AllPB.Maximum
        Thread.Sleep(1000)
    End Sub

    Sub GetPkgErrorLevel()
        errCode = Hex(Decimal.ToInt32(DISMProc.ExitCode))
        Select Case errCode
            Case 0
                pkgSuccessfulAdditions += 1
            Case Else
                pkgFailedAdditions += 1
        End Select
    End Sub

    Sub GetFeatErrorLevel()
        errCode = Hex(Decimal.ToInt32(DISMProc.ExitCode))
        Select Case errCode
            Case 0
                featSuccessfulEnablements += 1
            Case Else
                featFailedEnablements += 1
        End Select
    End Sub

    Private Sub ProgressBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ProgressBW.DoWork
        If TaskList.Count >= 2 Or (ActionRunning And TaskList.Count >= 1) Then
            RunTaskList(TaskList)
        Else
            RunOps(OperationNum)
        End If
    End Sub

    Sub SaveLog(LogFile As String)
        If Not File.Exists(LogFile) Then
            ' Create file
            Try
                File.WriteAllText(LogFile, String.Empty)
            Catch ex As Exception
                LogView.AppendText(CrLf & _
                                   "Warning: the contents of the log window could not be saved to the log file. Reason: " & CrLf & ex.ToString())
                Exit Sub
            End Try
        End If
        Dim logWindowReader As New RichTextBox() With {
            .Text = My.Computer.FileSystem.ReadAllText(LogFile, ASCII)
        }
        If logWindowReader.Text <> "" Then
            logWindowReader.AppendText(CrLf & "==================== DISMTools Log Window Contents (" & DateTime.Now.ToString() & ") ====================")
        Else
            logWindowReader.AppendText("======================== DISMTools Log File ========================" & CrLf & _
                                       "This is an automatically generated log file created by DISMTools." & CrLf & _
                                       "This file can be viewed at any time to view successful and/or" & CrLf & _
                                       "failed tasks." & CrLf & CrLf & _
                                       "This log file is updated every time an operation is performed." & CrLf & _
                                       "However, it does not contain the actual DISM log file, which is" & CrLf & _
                                       "also automatically generated each time DISM is run from this" & CrLf & _
                                       "program. These log files are named: " & CrLf & _
                                       "                    " & Quote & "DISMTools-<date/time>.log" & Quote & "                    " & CrLf & _
                                       "====================================================================")
        End If
        logWindowReader.AppendText(CrLf & LogView.Text)
        File.WriteAllText(LogFile, logWindowReader.Text, ASCII)
    End Sub

    Private Sub ProgressBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ProgressBW.RunWorkerCompleted
        If Not ActionRunning Then TaskList.Clear()
        If IsSuccessful Then
            If OperationNum = 9 Then LogView.AppendText(CrLf & _
                               "The volume images have been deleted. If you want to remount this image into a DISMTools project, choose the " & Quote & "Mount image" & Quote & " option, or use this command if you want to mount it elsewhere:" & CrLf & _
                               "  dism /mount-image /imagefile:" & Quote & imgIndexDeletionSourceImg & Quote & " /index:<preferred index> /mountdir:<preferred mountpoint>")
            'Visible = False
            SaveLog(Application.StartupPath & "\logs\DISMTools.log")
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
                MainForm.LoadDTProj(projPath & "\" & projName & "\" & projName & ".dtproj", projName, True, False)
            ElseIf OperationNum = 6 Then
                If CaptureMountDestImg Then
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
                MainForm.SaveDTProj()
            ElseIf OperationNum = 9 Then
                If imgIndexDeletionUnmount Then
                    ' Detect mounted images if the program needed to unmount the source image
                    MainForm.DetectMountedImages(False)
                    If UMountLocalDir Then
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
                MainForm.DetectMountedImages(False)
            ElseIf OperationNum = 26 Then
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                If Not MainForm.RunAllProcs Then MainForm.bwBackgroundProcessAction = 1
                MainForm.UpdateProjProperties(True, False)
                AddPackageReport.Label4.Text = MountDir
                AddPackageReport.Label6.Text = pkgSource
                ' The program may fail to count (possibly due to the pkgFailedAdditions variable)!
                If (pkgSuccessfulAdditions + pkgFailedAdditions) <> pkgCount Then
                    Do Until (pkgSuccessfulAdditions + pkgFailedAdditions) = pkgCount
                        If (pkgSuccessfulAdditions + pkgFailedAdditions) > pkgCount Then
                            pkgFailedAdditions -= 1
                        ElseIf (pkgSuccessfulAdditions + pkgFailedAdditions) < pkgCount Then
                            pkgFailedAdditions += 1
                        End If
                    Loop
                End If
                AddPackageReport.Label14.Text = pkgSuccessfulAdditions
                AddPackageReport.Label15.Text = pkgCount - pkgSuccessfulAdditions
                AddPackageReport.ProgressBar1.Maximum = pkgCount
                AddPackageReport.ProgressBar1.Value = pkgSuccessfulAdditions
                'AddPackageReport.pkgSuccessFailureRatio = CSng(pkgSuccessfulAdditions / pkgCount)
                AddPackageReport.Label11.Text = "Out of " & pkgCount & " packages..."
                AddPackageReport.Label12.Text = pkgSuccessfulAdditions & " were added successfully"
                AddPackageReport.Label13.Text = pkgFailedAdditions & " were not added"
                'AddPackageReport.ProgressBar1.Value = AddPackageReport.pkgSuccessFailureRatio
                AddPackageReport.Show()
            ElseIf OperationNum = 27 Then
                If Not MainForm.RunAllProcs Then MainForm.bwBackgroundProcessAction = 1
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 30 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 2
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 31 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 2
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 33 Then
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 37 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 3
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 38 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 3
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 64 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 4
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 68 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 4
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 75 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 5
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 76 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 5
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 78 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 5
                End If
                If Not MainForm.OnlineManagement And Not MainForm.OfflineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 991 Then
                Visible = False
                ImgConversionSuccessDialog.ShowDialog(MainForm)
                If ImgConversionSuccessDialog.DialogResult = Windows.Forms.DialogResult.OK Then
                    Process.Start("\Windows\explorer.exe", "/select," & Quote & imgDestFile & Quote)
                End If
            ElseIf OperationNum = 996 Then
                MainForm.DetectMountedImages(False)
                MainForm.ImgIndex = SwitchTargetIndex
                MainForm.imgMountedName = SwitchTargetIndexName
                MainForm.SaveDTProj()
                If SwitchMountAsReadOnly Then
                    MainForm.UpdateProjProperties(True, True)
                Else
                    MainForm.UpdateProjProperties(True, False)
                End If
                ' This is a crucial change, so save things immediately
                MainForm.SaveDTProj()
            ElseIf OperationNum = 1000 Then
                If TaskList.Count > 0 Then
                    If TaskList.Last = 21 Then
                        If MainForm.isProjectLoaded And MountDir = MainForm.MountDir Or RandomMountDir = MainForm.MountDir Then
                            MainForm.UpdateProjProperties(False, False)
                            MainForm.MountDir = "N/A"
                            ' This is a crucial change, so save things immediately
                            MainForm.SaveDTProj()
                            ImgMount.TextBox1.Text = ""     ' The program has a bug where mounting the same image after doing this results in the image file being ""
                            If MainForm.imgCommitOperation <> -1 Then
                                MainForm.imgCommitOperation = -1    ' Let program close on later occassions
                            End If
                        End If
                        MainForm.DetectMountedImages(False)
                    ElseIf TaskList.Last = 15 Then
                        If ActionRunning And MountDir.Contains(projPath) Then
                            MainForm.LoadDTProj(projPath & "\" & projName & "\" & projName & ".dtproj", projName, True, True)
                        End If
                        MainForm.bwBackgroundProcessAction = 0
                        MainForm.SourceImg = SourceImg
                        MainForm.ImgIndex = ImgIndex
                        MainForm.MountDir = MountDir
                        MainForm.DetectMountedImages(False)
                        If isReadOnly Then
                            MainForm.UpdateProjProperties(True, True, ActionRunning)
                        Else
                            MainForm.UpdateProjProperties(True, False, ActionRunning)
                        End If
                        ' This is a crucial change, so save things immediately
                        MainForm.SaveDTProj()
                        If ActionRunning And MountDir.Contains(projPath) Then
                            MainForm.UnloadDTProj(False, True, False)
                        End If
                    End If
                End If
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
            ActionRunning = False
            TaskList.Clear()
            MainForm.StatusStrip.BackColor = If(MainForm.ColorSchemes = 0, Color.FromArgb(53, 153, 41), Color.FromArgb(0, 122, 204))
            MainForm.ToolStripButton4.Visible = False
            If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
            MainForm.WatcherTimer.Enabled = True
            Close()
        Else
            MainForm.ToolStripButton4.Visible = False
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
                            Label2.Text = "Si è verificato un errore che ha interrotto le operazioni di immagine. Per ulteriori informazioni, leggere il registro sottostante."
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
                    Label2.Text = "Si è verificato un errore che ha interrotto le operazioni di immagine. Per ulteriori informazioni, leggere il registro sottostante."
            End Select
            CurrentPB.Value = CurrentPB.Maximum
            AllPB.Value = AllPB.Maximum
            If Height <> 420 Then
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
            If errCode = "C1420126" Then
                ' An image that was selected for mounting is already mounted
                LogView.AppendText(CrLf & "The specified image is already mounted. This command works for " & Quote & "orphaned" & Quote & " images")
            ElseIf errCode = "C142010C" Then
                ' The image, with read-only permissions, was attempted to be written
                LogView.AppendText(CrLf & "The program tried to save changes to an image that was mounted as read-only. " & CrLf & _
                                          "To solve this, close this dialog, and click " & Quote & "Tools > Remount image with write permissions" & Quote & CrLf & _
                                          "Do note that, if the image came from an installation medium, you may need to copy the source file to perform modifications to it.")
            ElseIf errCode = "C1420117" Then
                ' Some applications (or hidden processes) have open handles on the mount dir
                LogView.AppendText(CrLf & "The program tried to unmount the image, but some applications or processes have opened files or directories of the image." & CrLf & _
                                          "Make sure no application or process is using the directories or files of the image." & CrLf & _
                                          "If the error occurred at the end of the operation (e.g., at 100%), and you were trying to save the changes; they might already be saved, and can be safe to continue discarding changes.")
            ElseIf errCode = "C142011D" Then
                ' A partial unmount or an in-progress mount operation happened
                LogView.AppendText(CrLf & "The mounted image cannot be committed back into the source file." & CrLf & _
                                          "A partial unmount might have happened, or the image was still being mounted." & CrLf & _
                                          "If the image was unmounted whilst saving changes, the commit probably succeeded. Please validate this. If this is the case, proceed with unmounting the image discarding changes.")
            ElseIf errCode = "C1510111" Then
                ' The specified image, that was marked to mount with read-write permissions, came from a read-only source (e.g., a Windows installation disc)
                LogView.AppendText(CrLf & "The source file comes from a read-only source. You cannot mount it with read-write permissions." & CrLf & _
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
                LogView.AppendText(CrLf & "Either this operation has failed or some drivers were not installed. Consider reloading this project or mode to see whether there are driver changes." & CrLf & CrLf & _
                                   "If there are driver changes, consider reading the driver installation logs, stored in the INF directory of the target image. Otherwise, export the drivers you want to add from the source image and add them to the target image manually." & CrLf & CrLf & _
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
                If OperationNum = 86 Then
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
            MainForm.StatusStrip.BackColor = If(MainForm.ColorSchemes = 0, Color.FromArgb(53, 153, 41), Color.FromArgb(0, 122, 204))
            SaveLog(Application.StartupPath & "\logs\DISMTools.log")
        End If
    End Sub

    Sub GetErrorCode(Bypass As Boolean)
        If Bypass Then
            errCode = 0
        Else
            errCode = Hex(Decimal.ToInt32(DISMProc.ExitCode))
        End If
        Select Case errCode
            Case 0
                IsSuccessful = True
            Case Else
                IsSuccessful = False
        End Select
    End Sub

    Private Sub ProgressPanel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Progress"
                        Label1.Text = "Image operations in progress..."
                        Label2.Text = "Please wait while the following tasks are done. This may take some time."
                        Cancel_Button.Text = "Cancel"
                        LogButton.Text = If(Height = 240, "Show log", "Hide log")
                        LinkLabel1.Text = "Show DISM log file (advanced)"
                        GroupBox1.Text = "Log"
                        allTasks.Text = "Please wait..."
                        currentTask.Text = "Please wait..."
                    Case "ESN"
                        Text = "Progreso"
                        Label1.Text = "Operaciones en progreso..."
                        Label2.Text = "Espere mientras las siguientes tareas se realizan. Esto puede llevar algo de tiempo."
                        Cancel_Button.Text = "Cancelar"
                        LogButton.Text = If(Height = 240, "Mostrar registro", "Ocultar registro")
                        LinkLabel1.Text = "Mostrar archivo de registro de DISM (avanzado)"
                        GroupBox1.Text = "Registro"
                        allTasks.Text = "Por favor, espere..."
                        currentTask.Text = "Por favor, espere..."
                    Case "FRA"
                        Text = "Avancement"
                        Label1.Text = "Opérations de l'image en cours..."
                        Label2.Text = "Veuillez patienter pendant que les tâches suivantes sont effectuées. Cela peut prendre un certain temps."
                        Cancel_Button.Text = "Annuler"
                        LogButton.Text = If(Height = 240, "Afficher le journal", "Cacher le journal")
                        LinkLabel1.Text = "Afficher le fichier journal DISM (avancé)"
                        GroupBox1.Text = "Journal"
                        allTasks.Text = "Veuillez patienter..."
                        currentTask.Text = "Veuillez patienter..."
                    Case "PTB", "PTG"
                        Text = "Progresso"
                        Label1.Text = "Operações de imagem em curso..."
                        Label2.Text = "Aguarde enquanto as seguintes tarefas são efectuadas. Isto pode demorar algum tempo"
                        Cancel_Button.Text = "Cancelar"
                        LogButton.Text = If(Height = 240, " Mostrar registo", "Ocultar registo")
                        LinkLabel1.Text = "Mostrar ficheiro de registo DISM (avançado)"
                        GroupBox1.Text = "Registo"
                        allTasks.Text = "Aguarde..."
                        currentTask.Text = "Por favor, aguarde..."
                    Case "ITA"
                        Text = "Progresso"
                        Label1.Text = "Operazioni di immagine in corso..."
                        Label2.Text = "Attendere mentre vengono eseguite le operazioni seguenti. L'operazione potrebbe richiedere del tempo"
                        Cancel_Button.Text = "Annullare"
                        LogButton.Text = If(Height = 240, " Mostra registro", "Nascondi registro")
                        LinkLabel1.Text = "Mostra il file di registro DISM (avanzato)"
                        GroupBox1.Text = "Log"
                        allTasks.Text = "Attendere..."
                        currentTask.Text = "Attendere..."
                End Select
            Case 1
                Text = "Progress"
                Label1.Text = "Image operations in progress..."
                Label2.Text = "Please wait while the following tasks are done. This may take some time."
                Cancel_Button.Text = "Cancel"
                LogButton.Text = If(Height = 240, "Show log", "Hide log")
                LinkLabel1.Text = "Show DISM log file (advanced)"
                GroupBox1.Text = "Log"
                allTasks.Text = "Please wait..."
                currentTask.Text = "Please wait..."
            Case 2
                Text = "Progreso"
                Label1.Text = "Operaciones en progreso..."
                Label2.Text = "Espere mientras las siguientes tareas se realizan. Esto puede llevar algo de tiempo."
                Cancel_Button.Text = "Cancelar"
                LogButton.Text = If(Height = 240, "Mostrar registro", "Ocultar registro")
                LinkLabel1.Text = "Mostrar archivo de registro de DISM (avanzado)"
                GroupBox1.Text = "Registro"
                allTasks.Text = "Por favor, espere..."
                currentTask.Text = "Por favor, espere..."
            Case 3
                Text = "Avancement"
                Label1.Text = "Opérations de l'image en cours..."
                Label2.Text = "Veuillez patienter pendant que les tâches suivantes sont effectuées. Cela peut prendre un certain temps."
                Cancel_Button.Text = "Annuler"
                LogButton.Text = If(Height = 240, "Afficher le journal", "Cacher le journal")
                LinkLabel1.Text = "Afficher le fichier journal DISM (avancé)"
                GroupBox1.Text = "Journal"
                allTasks.Text = "Veuillez patienter..."
                currentTask.Text = "Veuillez patienter..."
            Case 4
                Text = "Progresso"
                Label1.Text = "Operações de imagem em curso..."
                Label2.Text = "Aguarde enquanto as seguintes tarefas são efectuadas. Isto pode demorar algum tempo"
                Cancel_Button.Text = "Cancelar"
                LogButton.Text = If(Height = 240, " Mostrar registo", "Ocultar registo")
                LinkLabel1.Text = "Mostrar ficheiro de registo DISM (avançado)"
                GroupBox1.Text = "Registo"
                allTasks.Text = "Aguarde..."
                currentTask.Text = "Por favor, aguarde..."
            Case 5
                Text = "Progresso"
                Label1.Text = "Operazioni di immagine in corso..."
                Label2.Text = "Attendere mentre vengono eseguite le operazioni seguenti. L'operazione potrebbe richiedere del tempo"
                Cancel_Button.Text = "Annullare"
                LogButton.Text = If(Height = 240, " Mostra registro", "Nascondi registro")
                LinkLabel1.Text = "Mostra il file di registro DISM (avanzato)"
                GroupBox1.Text = "Log"
                allTasks.Text = "Attendere..."
                currentTask.Text = "Attendere..."
        End Select
        If MainForm.ExpandedProgressPanel AndAlso Height = 240 Then
            LogButton.PerformClick()
        End If
        taskCountLbl.Visible = False
        MainForm.bwBackgroundProcessAction = 0
        MainForm.bwGetImageInfo = True
        MainForm.bwGetAdvImgInfo = True
        Language = MainForm.Language
        AllDrivers = MainForm.AllDrivers
        BodyPanel.BorderStyle = BorderStyle.None
        ImgVersion = MainForm.imgVersionInfo
        ' Determine program colors
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BodyPanel.BackColor = Color.FromArgb(37, 37, 38)
            BodyPanel.ForeColor = Color.White
            GroupBox1.BackColor = Color.FromArgb(37, 37, 38)
            GroupBox1.ForeColor = Color.White
            LogView.BackColor = Color.FromArgb(37, 37, 38)
            LogView.ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BodyPanel.BackColor = Color.FromArgb(246, 246, 246)
            BodyPanel.ForeColor = Color.Black
            GroupBox1.BackColor = Color.FromArgb(246, 246, 246)
            GroupBox1.ForeColor = Color.Black
            LogView.BackColor = Color.FromArgb(246, 246, 246)
            LogView.ForeColor = Color.Black
        End If
        CurrentPB.Value = 0
        AllPB.Value = 0
        If LogView.Text <> "" Then LogView.Clear()
        ' If running, cancel background processes
        If MainForm.ImgBW.IsBusy Then
            ' Make form visible sooner. We may have to set more things up here,
            ' but we'll see
            Visible = True
            LogView.AppendText("Cancelling background processes...")
            MainForm.ImgBW.CancelAsync()
            While MainForm.ImgBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(100)
            End While
        End If
        ' Cancel detector background worker which can interfere with image operations and cause crashes due to access violations
        MainForm.MountedImageDetectorBWRestarterTimer.Enabled = False
        MainForm.MountedImageDetectorBW.CancelAsync()
        While MainForm.MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        MainForm.WatcherTimer.Enabled = False
        If MainForm.WatcherBW.IsBusy Then MainForm.WatcherBW.CancelAsync()
        While MainForm.WatcherBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        If MainForm.MountedImageMountDirs.Count > 0 Then
            ' Go through all mounted images to determine which one to get info from with the DISM API,
            ' if a project has been loaded and if that project has a mounted image
            If MainForm.isProjectLoaded And MainForm.IsImageMounted Then
                For x = 0 To Array.LastIndexOf(MainForm.MountedImageMountDirs, MainForm.MountedImageMountDirs.Last)
                    If MainForm.MountedImageMountDirs(x) = MainForm.MountDir Then
                        mntString = MainForm.MountedImageMountDirs(x)
                    End If
                Next
            End If
        End If
        If MainForm.OfflineManagement Then mntString = MainForm.MountDir
        DismProgram = MainForm.DismExe
        If MountDir = "" Then MountDir = MainForm.MountDir
        DISMProc.StartInfo.CreateNoWindow = False
        Try
            If MainForm.LogFontIsBold Then
                LogView.Font = New Font(MainForm.LogFont, MainForm.LogFontSize, FontStyle.Bold)
            Else
                LogView.Font = New Font(MainForm.LogFont, MainForm.LogFontSize)
            End If
        Catch ex As Exception
            LogView.Font = New Font("Consolas", 11.25)
        End Try
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
                        MainForm.MenuDesc.Text = "Esecuzione di operazioni sulle immagini. Attendere..."
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
                MainForm.MenuDesc.Text = "Esecuzione di operazioni sulle immagini. Attendere..."
        End Select
        MainForm.StatusStrip.BackColor = If(MainForm.ColorSchemes = 0, Color.FromArgb(18, 51, 14), Color.FromArgb(14, 99, 156))
        If Debugger.IsAttached Then
            IsDebugged = True
        Else
            IsDebugged = False
        End If
        MainForm.ToolStripButton4.Visible = True
        Control.CheckForIllegalCrossThreadCalls = False
        LinkLabel1.Visible = False
        If Not Directory.Exists(Application.StartupPath & "\logs") Then Directory.CreateDirectory(Application.StartupPath & "\logs")
        ' Detect settings
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
        If UseScratchDir And AutoScratch And OnlineMgmt And Not Directory.Exists(Application.StartupPath & "\scratch") Then Directory.CreateDirectory(Application.StartupPath & "\scratch")
        GatherInitialSwitches()
        If ActionRunning Then
            OperationNum = 1000
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
            InitializeActionRuntime(IsInValidationMode)
            ReadActionFile(ActionFile)
        Else
            If TaskList.Count >= 2 Then
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
                GetTasks(OperationNum)
            End If
        End If
        taskCountLbl.Visible = True
        If RegistryControlPanel.Visible Then
            RegistryControlPanel.Close()
            If RegistryControlPanel.Visible Then
                LogView.AppendText(CrLf & "The image registry hives need to be unloaded before continuing to perform the task.")
            End If
        End If
        If Not RegistryControlPanel.Visible Then
            ProgressBW.RunWorkerAsync()
        Else
            Visible = True
            Application.DoEvents()
            Thread.Sleep(2000)
            Close()
        End If
    End Sub

#Region "Actions"

    ' This is temporary stuff. This will become improved

    Sub InitializeActionRuntime(Validate As Boolean)
        LogView.AppendText("Running the Action file " & If(Validate, "with validation", "") & "...")
        If Validate Then ValidationForm.Show()
    End Sub

    Sub ReadActionFile(ActionFile As String)
        Dim Reader As New RichTextBox With {.Text = File.ReadAllText(ActionFile)}
        For x = 0 To Reader.Lines.Count - 1
            If String.IsNullOrWhiteSpace(Reader.Lines(x)) Then
                Continue For
            ElseIf Reader.Lines(x).StartsWith("Section Properties()") And IsInValidationMode Then
                GetTestImageInfo(x, ActionFile)
            ElseIf Reader.Lines(x).StartsWith("Section Main()") Then
                GetTasks(x, ActionFile)
            End If
        Next
    End Sub

    Sub GetTasks(StarterLine As Integer, ActionFile As String)
        Dim Reader As New RichTextBox With {.Text = File.ReadAllText(ActionFile)}
        For x = StarterLine To Reader.Lines.Count - 1
            If String.IsNullOrWhiteSpace(Reader.Lines(x)) Or Reader.Lines(x).StartsWith("'") Then
                Continue For
            Else
                If Reader.Lines(x).Replace(" ", "").Trim().StartsWith("Image.Mount", StringComparison.OrdinalIgnoreCase) Then
                    TaskList.Add(15)
                    ParseParameters(Reader.Lines(x).Replace("Image.Mount", "").Trim())
                    ValidationForm.ListView1.Items.Add(New ListViewItem(New String() {"Mount image: " & ActionParameters(0), "Pending"}))
                    SourceImg = ActionParameters(0).Replace(Quote, "").Trim()
                    ImgIndex = CInt(ActionParameters(1).Remove(0, 1).Replace(Quote, "").Trim())
                    MountDir = ActionParameters(2).Remove(0, 1).Replace(Quote, "").Trim()
                    isReadOnly = False
                    isOptimized = False
                    isIntegrityTested = False
                    Select Case ActionParameters.Count
                        Case 4
                            isReadOnly = If(ActionParameters(3).Equals("True", StringComparison.OrdinalIgnoreCase), True, False)
                        Case 5
                            isReadOnly = If(ActionParameters(3).Equals("True", StringComparison.OrdinalIgnoreCase), True, False)
                            isOptimized = If(ActionParameters(4).Equals("True", StringComparison.OrdinalIgnoreCase), True, False)
                        Case 6
                            isReadOnly = If(ActionParameters(3).Equals("True", StringComparison.OrdinalIgnoreCase), True, False)
                            isOptimized = If(ActionParameters(4).Equals("True", StringComparison.OrdinalIgnoreCase), True, False)
                            isIntegrityTested = If(ActionParameters(5).Equals("True", StringComparison.OrdinalIgnoreCase), True, False)
                    End Select
                ElseIf Reader.Lines(x).Replace(" ", "").Trim().StartsWith("Image.Remount", StringComparison.OrdinalIgnoreCase) Then
                    TaskList.Add(18)
                    ParseParameters(Reader.Lines(x).Replace("Image.Remount", "").Trim())
                    ValidationForm.ListView1.Items.Add(New ListViewItem(New String() {"Remount image: " & ActionParameters(0), "Pending"}))
                    MountDir = ActionParameters(0).Replace(Quote, "").Trim()
                ElseIf Reader.Lines(x).Replace(" ", "").Trim().StartsWith("Image.Unmount", StringComparison.OrdinalIgnoreCase) Then
                    TaskList.Add(21)
                    ParseParameters(Reader.Lines(x).Replace("Image.Unmount", "").Trim())
                    ValidationForm.ListView1.Items.Add(New ListViewItem(New String() {"Unmount image on mount directory: " & ActionParameters(0), "Pending"}))
                    UMountLocalDir = False
                    RandomMountDir = ActionParameters(0).Replace(Quote, "").Trim()
                    UMountOp = If(ActionParameters(1).Remove(0, 1).Replace(Quote, "").Trim().Equals("True", StringComparison.OrdinalIgnoreCase), 0, 1)
                    If UMountOp = 0 Then
                        Select Case ActionParameters.Count
                            Case 2
                                CheckImgIntegrity = If(ActionParameters(2).Remove(0, 1).Replace(Quote, "").Trim().Equals("True", StringComparison.OrdinalIgnoreCase), True, False)
                            Case 3
                                CheckImgIntegrity = If(ActionParameters(2).Remove(0, 1).Replace(Quote, "").Trim().Equals("True", StringComparison.OrdinalIgnoreCase), True, False)
                                SaveToNewIndex = If(ActionParameters(3).Remove(0, 1).Replace(Quote, "").Trim().Equals("True", StringComparison.OrdinalIgnoreCase), True, False)
                        End Select
                    End If
                ElseIf Reader.Lines(x).Replace(" ", "").Trim().StartsWith("Project.Create", StringComparison.OrdinalIgnoreCase) Then
                    TaskList.Add(0)
                    ParseParameters(Reader.Lines(x).Replace("Project.Create", "").Trim())
                    ValidationForm.ListView1.Items.Add(New ListViewItem(New String() {"Create project: " & ActionParameters(0), "Pending"}))
                    projName = ActionParameters(0).Replace(Quote, "").Trim()
                    projPath = ActionParameters(1).Remove(0, 1).Replace(Quote, "").Trim()
                ElseIf Reader.Lines(x).Equals("End Section", StringComparison.OrdinalIgnoreCase) Then
                    Exit For
                End If
            End If
        Next
    End Sub

    Sub GetTestImageInfo(StarterLine As Integer, ActionFile As String)
        Dim Reader As New RichTextBox With {.Text = File.ReadAllText(ActionFile)}
        For x = StarterLine To Reader.Lines.Count - 1
            If Reader.Lines(x).Replace(" ", "").Trim().StartsWith("Action.TestImage.FileName", StringComparison.OrdinalIgnoreCase) Then
                Actions_ImageFile = Reader.Lines(x).Substring(Reader.Lines(x).IndexOf("=")).Replace("=", "").Trim().Replace(Quote, "").Trim()
            ElseIf Reader.Lines(x).Replace(" ", "").Trim().StartsWith("Action.TestImage.ImageIndex", StringComparison.OrdinalIgnoreCase) Then
                Actions_ImageIndex = CInt(Reader.Lines(x).Substring(Reader.Lines(x).IndexOf("=")).Replace("=", "").Trim().Replace(Quote, "").Trim())
            ElseIf Reader.Lines(x).Equals("End Section", StringComparison.OrdinalIgnoreCase) Then
                Exit For
            End If
        Next
    End Sub

    Sub ParseParameters(Line As String)
        Dim newLine As String = Line.Trim(New Char() {"("c, ")"c})
        Dim regex As New Regex(",(?=(?:[^""]*""[^""]*"")*[^""]*$)")
        ActionParameters = regex.Split(newLine).ToList()
    End Sub

#End Region

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Try
            If File.Exists(Application.StartupPath & "\logs\" & dateStr) Then
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\notepad.exe", Application.StartupPath & "\logs\" & dateStr)
            ElseIf File.Exists(LogPath) Then
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\notepad.exe", LogPath)
            End If
        Catch ex As Exception
            If Not File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\notepad.exe") Then
                LogView.AppendText(CrLf & "Notepad was not found")
            ElseIf Not File.Exists(Application.StartupPath & "\logs\" & dateStr) Or Not File.Exists(LogPath) Then
                LogView.AppendText(CrLf & "The log file was not found")
            End If
        End Try
    End Sub

    Private Sub BodyPanel_Paint(sender As Object, e As PaintEventArgs) Handles BodyPanel.Paint
        ControlPaint.DrawBorder(e.Graphics, BodyPanel.ClientRectangle, If(MainForm.ColorSchemes = 0, Color.FromArgb(53, 153, 41), Color.FromArgb(0, 122, 204)), ButtonBorderStyle.Solid)
    End Sub

    Private Sub ProgressPanel_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If ValidationForm.Visible Then ValidationForm.Close()
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
        ActionRunning = False
        MainForm.StatusStrip.BackColor = If(MainForm.ColorSchemes = 0, Color.FromArgb(53, 153, 41), Color.FromArgb(0, 122, 204))
        MainForm.ToolStripButton4.Visible = False
        If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
        MainForm.WatcherTimer.Enabled = True
    End Sub
End Class