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

' OperationNums for unattended servicing
' --------------------------------------
' 78                    Apply-Unattend

' OperationNums for Windows PE servicing
' --------------------------------------
' 79                    Get-PESettings
' 80                    Get-ScratchSpace
' 81                    Get-TargetPath
' 82                    Set-ScratchSpace
' 83                    Set-TargetPath

' OperationNums for operating system uninstall
' --------------------------------------------
' 84                    Get-OSUninstallWindow
' 85                    Initiate-OSUninstall
' 86                    Remove-OSUninstall
' 87                    Set-OSUninstallWindow

' OperationNums for reserved storage
' ----------------------------------
' 88                    Set-ReservedStorageState
' 89                    Get-ReservedStorageState

' OperationNums for Microsoft Edge servicing
' ------------------------------------------
' 90                    Add-Edge
' 91                    Add-EdgeBrowser
' 92                    Add-EdgeWebView

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
    Dim pkgPossibleMsuFile As String                        ' Determine whether package is a MSU file
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

    ' OperationNum: 82
    Public peNewScratchSpace As Integer                     ' New scratch space amount to apply to the Windows PE image

    ' OperationNum: 83
    Public peNewTargetPath As String                        ' New target path to apply to the Windows PE image

    ' <Space for other OperationNums>
    ' OperationNum: 87
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
        If Cancel_Button.Text = "Cancel" Or Cancel_Button.Text = "Cancelar" Then
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
                    End Select
                Case 1
                    LogButton.Text = "Hide log"
                Case 2
                    LogButton.Text = "Ocultar registro"
                Case 3
                    LogButton.Text = "Cacher le journal"
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
                    End Select
                Case 1
                    LogButton.Text = "Show log"
                Case 2
                    LogButton.Text = "Mostrar registro"
                Case 3
                    LogButton.Text = "Afficher le journal"
            End Select
            Height = 240
        End If
        BodyPanel.Refresh()
        CenterToParent()
    End Sub

    Sub GetTasks(opNum As Integer)
        If opNum = 0 Then
            taskCount = 1
        ElseIf opNum = 3 Then
            taskCount = 1
        ElseIf opNum = 6 Then
            If CaptureMountDestImg Then
                taskCount = 3
            Else
                taskCount = 1
            End If
        ElseIf opNum = 8 Then
            taskCount = 1
        ElseIf opNum = 9 Then
            If imgIndexDeletionUnmount Then
                taskCount = 2
            Else
                taskCount = 1
            End If
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
        ElseIf opNum = 82 Then
            taskCount = 1
        ElseIf opNum = 83 Then
            taskCount = 1
        ElseIf opNum = 87 Then
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
                End Select
            Case 1
                taskCountLbl.Text = "Tasks: 1/" & taskCount
            Case 2
                taskCountLbl.Text = "Tareas: 1/" & taskCount
            Case 3
                taskCountLbl.Text = "Tâches : 1/" & taskCount
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
                    End Select
                Case 1
                    taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskList.Count
                Case 2
                    taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskList.Count
                Case 3
                    taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskList.Count
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
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win51_cons")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win60_cons")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win61_cons")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win62_cons")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win62_cons\mbr")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win62_cons\uefi")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win63_cons")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win63_cons\mbr")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win63_cons\uefi")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win10_cons")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win10_cons\mbr")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win10_cons\uefi")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win60_serv")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win61_serv")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win62_serv")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win63_serv")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\win10_serv")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\sbs08")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\sbs11")
                Directory.CreateDirectory(projPath & "\" & projName & "\" & "unattend_xml\whs11")
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
                    End Select
                Case 1
                    currentTask.Text = "Gathering error level..."
                Case 2
                    currentTask.Text = "Recopilando nivel de error..."
                Case 3
                    currentTask.Text = "Recueil du niveau d'erreur en cours..."
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
                    LogView.AppendText("   WARNING: the following file does not exist in the file system. Skipping file..." & CrLf)
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
                    End Select
                Case 1
                    currentTask.Text = "Gathering error level..."
                Case 2
                    currentTask.Text = "Recopilando nivel de error..."
                Case 3
                    currentTask.Text = "Recueil du niveau d'erreur en cours..."
            End Select
            LogView.AppendText(CrLf & "Gathering error level...")
            GetErrorCode(False)
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
                    End Select
                Case 1
                    currentTask.Text = "Gathering error level..."
                Case 2
                    currentTask.Text = "Recopilando nivel de error..."
                Case 3
                    currentTask.Text = "Recueil du niveau d'erreur en cours..."
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
                        End Select
                    Case 1
                        taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                    Case 2
                        taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                    Case 3
                        taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
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
                        End Select
                    Case 1
                        currentTask.Text = "Removing volume image " & Quote & imgIndexDeletionNames(x) & Quote & "..."
                    Case 2
                        currentTask.Text = "Eliminando imagen de volumen " & Quote & imgIndexDeletionNames(x) & Quote & "..."
                    Case 3
                        currentTask.Text = "Suppression de l'image de volume " & Quote & imgIndexDeletionNames(x) & Quote & " en cours..."
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
        ElseIf opNum = 11 Then
            CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /Get-ImageInfo"
            allTasks.Text = "Getting information..."
            currentTask.Text = "Getting image information..."
            LogView.AppendText(CrLf & "Getting information from image..." & CrLf & "Options:" & CrLf)
            If GetFromMountedImg Then
                LogView.AppendText("- Get information from mounted image? Yes")
            Else
                LogView.AppendText("- Get information from mounted image? No")
            End If
            ' Get detailed information printed on LogView
            If GetFromMountedImg Then
                LogView.AppendText(CrLf & "- Image file: " & InfoFromSourceImg)
                CommandArgs &= " /ImageFile:" & InfoFromSourceImg
                If GetSpecificIndexInfo Then
                    If GetFromMountedIndex Then
                        LogView.AppendText(CrLf & "- Image index: " & InfoFromSourceIndex & " (mounted index)")
                        CommandArgs &= " /Index:" & InfoFromSourceIndex
                    Else
                        LogView.AppendText(CrLf & "- Image index: " & InfoFromSpecificIndex)
                        CommandArgs &= " /Index:" & InfoFromSpecificIndex
                    End If
                End If
            Else
                LogView.AppendText(CrLf & "- Image file: " & InfoFromSpecificImg)
                CommandArgs &= " /ImageFile:" & InfoFromSpecificImg
                If GetSpecificIndexInfo Then
                    ' This version still does not support getting image information from its mounted index if it is an external image
                    LogView.AppendText(CrLf & "- Image index: " & InfoFromSpecificIndex)
                    CommandArgs &= " /Index:" & InfoFromSpecificIndex
                End If
            End If
            ' Run process. Right now, use an external process to do all the job
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat",
                              "@echo off" & CrLf &
                              "dism " & CommandArgs & " > ",
                              ASCII)
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
                    End Select
                Case 1
                    currentTask.Text = "Gathering error level..."
                Case 2
                    currentTask.Text = "Recopilando nivel de error..."
                Case 3
                    currentTask.Text = "Recueil du niveau d'erreur en cours..."
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
                    End Select
                Case 1
                    currentTask.Text = "Gathering error level..."
                Case 2
                    currentTask.Text = "Recopilando nivel de error..."
                Case 3
                    currentTask.Text = "Recueil du niveau d'erreur en cours..."
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
                        End Select
                    Case 1
                        currentTask.Text = "Gathering error level..."
                    Case 2
                        currentTask.Text = "Recopilando nivel de error..."
                    Case 3
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
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
                        End Select
                    Case 1
                        currentTask.Text = "Gathering error level..."
                    Case 2
                        currentTask.Text = "Recopilando nivel de error..."
                    Case 3
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
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
                    End Select
                Case 1
                    currentTask.Text = "Adding " & pkgCount & " packages..."
                Case 2
                    currentTask.Text = "Añadiendo " & pkgCount & " paquetes..."
                Case 3
                    currentTask.Text = "Ajout de " & pkgCount & " paquets en cours..."
            End Select
            CurrentPB.Style = ProgressBarStyle.Blocks
            LogView.AppendText(CrLf & CrLf &
                               "Processing " & pkgCount & " packages..." & CrLf)
            If pkgAdditionOp = 0 Then
                DISMProc.StartInfo.FileName = DismProgram
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /add-package /packagepath=" & Quote & pkgSource & Quote
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
                        End Select
                    Case 1
                        currentTask.Text = "Gathering error level..."
                    Case 2
                        currentTask.Text = "Recopilando nivel de error..."
                    Case 3
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
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
                            End Select
                        Case 1
                            currentTask.Text = "Adding package " & (x + 1) & " of " & pkgCount & "..."
                        Case 2
                            currentTask.Text = "Añadiendo paquete " & (x + 1) & " de " & pkgCount & "..."
                        Case 3
                            currentTask.Text = "Ajout du paquet " & (x + 1) & " de " & pkgCount & " en cours..."
                    End Select
                    CurrentPB.Value = x + 1
                    LogView.AppendText(CrLf &
                                       "Package " & (x + 1) & " of " & pkgCount)        ' You don't want to see "Package 0 of 407", right?
                    Directory.CreateDirectory(Application.StartupPath & "\tempinfo")
                    pkgPossibleMsuFile = Path.GetFileName(pkgs(x)).ToString()
                    If pkgPossibleMsuFile.EndsWith(".msu") Then
                        LogView.AppendText(CrLf & "WARNING: the package currently about to be processed is a MSU file. Proceeding with special addition mode...")
                        AddFromMsu(pkgs(x))
                        Continue For
                    End If

                    ' Get package information with the DISM API
                    Dim pkgIsApplicable As Boolean
                    Dim pkgIsInstalled As Boolean
                    Try
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
                    CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /add-package /packagepath=" & Quote & pkgs(x) & Quote
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
                        End Select
                    Case 1
                        taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                    Case 2
                        taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                    Case 3
                        taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                End Select
                RunOps(8)
            Else
                AllPB.Value = 100
            End If
            If pkgAdditionOp = 0 Then
                GetErrorCode(False)
            ElseIf pkgAdditionOp = 1 And pkgSuccessfulAdditions > 0 Then
                GetErrorCode(True)
            ElseIf pkgAdditionOp = 1 And pkgSuccessfulAdditions <= 0 Then
                GetErrorCode(False)
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
                    End Select
                Case 1
                    currentTask.Text = "Removing packages..."
                Case 2
                    currentTask.Text = "Eliminando paquetes..."
                Case 3
                    currentTask.Text = "Suppression des paquets en cours..."
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
                            End Select
                        Case 1
                            currentTask.Text = "Removing package " & (x + 1) & " of " & pkgRemovalCount & "..."
                        Case 2
                            currentTask.Text = "Eliminando paquete " & (x + 1) & " de " & pkgRemovalCount & "..."
                        Case 3
                            currentTask.Text = "Suppression du paquet " & (x + 1) & " de " & pkgRemovalCount & " en cours..."
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
                        CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /remove-package /packagename=" & pkgRemovalNames(x)
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
                            End Select
                        Case 1
                            currentTask.Text = "Removing package " & (x + 1) & " of " & pkgRemovalCount & "..."
                        Case 2
                            currentTask.Text = "Eliminando paquete " & (x + 1) & " de " & pkgRemovalCount & "..."
                        Case 3
                            currentTask.Text = "Suppression du paquet " & (x + 1) & " de " & pkgRemovalCount & " en cours..."
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
                        CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /remove-package /packagepath=" & pkgRemovalFiles(x)
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
                    End Select
                Case 1
                    currentTask.Text = "Enabling features..."
                Case 2
                    currentTask.Text = "Habilitando características..."
                Case 3
                    currentTask.Text = "Activation des caractéristiques en cours..."
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
                        End Select
                    Case 1
                        currentTask.Text = "Enabling feature " & (x + 1) & " of " & featEnablementCount & "..."
                    Case 2
                        currentTask.Text = "Habilitando característica " & (x + 1) & " de " & featEnablementCount & "..."
                    Case 3
                        currentTask.Text = "Activation de la caractéristique " & (x + 1) & " de " & featEnablementCount & " en cours..."
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
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /enable-feature /featurename=" & featEnablementNames(x).Replace("ListViewItem: ", "").Trim().Replace("{", "").Trim().Replace("}", "").Trim()
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
                taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                RunOps(8)
            Else
                AllPB.Value = 100
            End If
            If featSuccessfulEnablements > 0 Then
                GetErrorCode(True)
            ElseIf featSuccessfulEnablements <= 0 Then
                GetErrorCode(False)
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
                    End Select
                Case 1
                    currentTask.Text = "Disabling features..."
                Case 2
                    currentTask.Text = "Deshabilitando características..."
                Case 3
                    currentTask.Text = "Désactivation des caractéristiques en cours..."
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
                        End Select
                    Case 1
                        currentTask.Text = "Disabling feature " & (x + 1) & " of " & featDisablementCount & "..."
                    Case 2
                        currentTask.Text = "Deshabilitando característica " & (x + 1) & " de " & featDisablementCount & "..."
                    Case 3
                        currentTask.Text = "Désactivation de la caractéristique " & (x + 1) & " de " & featDisablementCount & " en cours..."
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
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /disable-feature /featurename=" & featDisablementNames(x).Replace("ListViewItem: ", "").Trim().Replace("{", "").Trim().Replace("}", "").Trim()
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
                    End Select
                Case 1
                    allTasks.Text = "Cleaning up the image..."
                Case 2
                    allTasks.Text = "Limpiando la imagen..."
                Case 3
                    allTasks.Text = "Nettoyage de l'image en cours..."
            End Select
            ' Initialize command
            DISMProc.StartInfo.FileName = DismProgram
            CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /cleanup-image"
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
                            End Select
                        Case 1
                            currentTask.Text = "Reverting pending servicing actions..."
                        Case 2
                            currentTask.Text = "Revirtiendo acciones de servicio pendientes..."
                        Case 3
                            currentTask.Text = "Annulation des actions de maintenance en cours..."
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
                            End Select
                        Case 1
                            currentTask.Text = "Cleaning up Service Pack backup files..."
                        Case 2
                            currentTask.Text = "Limpiando archivos de copia de seguridad del Service Pack..."
                        Case 3
                            currentTask.Text = "Nettoyage des fichiers de sauvegarde du Service Pack en cours..."
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
                            End Select
                        Case 1
                            currentTask.Text = "Cleaning up the component store..."
                        Case 2
                            currentTask.Text = "Limpiando el almacén de componentes..."
                        Case 3
                            currentTask.Text = "Nettoyage du stock de composants en cours..."
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
                            End Select
                        Case 1
                            currentTask.Text = "Analyzing the component store..."
                        Case 2
                            currentTask.Text = "Analizando el almacén de componentes..."
                        Case 3
                            currentTask.Text = "Analyse du stock de composants en cours..."
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
                            End Select
                        Case 1
                            currentTask.Text = "Checking the component store health..."
                        Case 2
                            currentTask.Text = "Comprobando la salud del almacén de componentes..."
                        Case 3
                            currentTask.Text = "Vérification de l'état de santé du stock de composants en cours..."
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
                            End Select
                        Case 1
                            currentTask.Text = "Scanning the component store..."
                        Case 2
                            currentTask.Text = "Escaneando el almacén de componentes..."
                        Case 3
                            currentTask.Text = "Analyse du stock de composants en cours..."
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
                            End Select
                        Case 1
                            currentTask.Text = "Repairing the component store..."
                        Case 2
                            currentTask.Text = "Reparando el almacén de componentes..."
                        Case 3
                            currentTask.Text = "Réparation du stock de composants en cours..."
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
                    End Select
                Case 1
                    currentTask.Text = "Gathering error level..."
                Case 2
                    currentTask.Text = "Recopilando nivel de error..."
                Case 3
                    currentTask.Text = "Recueil du niveau d'erreur en cours..."
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
            End Select
            LogView.AppendText("Adding provisioning package to the image..." & CrLf & _
                               "Options:" & CrLf & CrLf & _
                               "- Provisioning package: " & Quote & ppkgAdditionPackagePath & Quote & CrLf & _
                               "- Catalog file: " & If(ppkgAdditionCatalogPath = "", "none specified", Quote & ppkgAdditionCatalogPath & Quote) & CrLf & _
                               "- Commit image after adding provisioning package? " & If(ppkgAdditionCommit, "Yes", "No"))
            DISMProc.StartInfo.FileName = DismProgram
            CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /add-provisioningpackage /packagepath=" & Quote & ppkgAdditionPackagePath & Quote & If(ppkgAdditionCatalogPath <> "" And File.Exists(ppkgAdditionCatalogPath), " /catalogpath=" & Quote & ppkgAdditionCatalogPath & Quote, "")
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
                        End Select
                    Case 1
                        taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                    Case 2
                        taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                    Case 3
                        taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
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
                    End Select
                Case 1
                    currentTask.Text = "Adding AppX packages..."
                Case 2
                    currentTask.Text = "Añadiendo paquetes AppX..."
                Case 3
                    currentTask.Text = "Ajout de paquets AppX en cours..."
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
                        End Select
                    Case 1
                        currentTask.Text = "Adding package " & (x + 1) & " of " & appxAdditionCount & "..."
                    Case 2
                        currentTask.Text = "Añadiendo paquete " & (x + 1) & " de " & appxAdditionCount & "..."
                    Case 3
                        currentTask.Text = "Ajout du paquet " & (x + 1) & " de " & appxAdditionCount & " en cours..."
                End Select
                LogView.AppendText(CrLf & _
                                   "Package " & (x + 1) & " of " & appxAdditionCount)
                CurrentPB.Value = x + 1
                LogView.AppendText(CrLf & _
                                   "- AppX package file: " & appxAdditionPackageList(x).PackageFile & CrLf & _
                                   "- Application name: " & appxAdditionPackageList(x).PackageName & CrLf & _
                                   "- Application publisher: " & appxAdditionPackageList(x).PackagePublisher & CrLf & _
                                   "- Application version: " & appxAdditionPackageList(x).PackageVersion & CrLf)

                ' Initialize command
                DISMProc.StartInfo.FileName = DismProgram
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /add-provisionedappxpackage "
                If File.GetAttributes(appxAdditionPackageList(x).PackageFile) = FileAttributes.Directory Then
                    CommandArgs &= "/folderpath=" & Quote & appxAdditionPackageList(x).PackageFile & Quote
                Else
                    CommandArgs &= "/packagepath=" & Quote & appxAdditionPackageList(x).PackageFile & Quote
                End If
                If appxAdditionPackageList(x).PackageLicenseFile <> "" And File.Exists(appxAdditionPackageList(x).PackageLicenseFile) Then
                    CommandArgs &= " /licensefile=" & Quote & appxAdditionPackageList(x).PackageLicenseFile & Quote
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
                If FileVersionInfo.GetVersionInfo(DismProgram).ProductMajorPart = 10 And ImgVersion.Major = 10 Then
                    If appxAdditionPackageList(x).PackageRegions = "" Then
                        CommandArgs &= " /region:all"
                    Else
                        CommandArgs &= " /region:" & Quote & appxAdditionPackageList(x).PackageRegions & Quote
                    End If
                End If
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
                        End Select
                    Case 1
                        taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                    Case 2
                        taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                    Case 3
                        taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
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
                    End Select
                Case 1
                    currentTask.Text = "Removing AppX packages..."
                Case 2
                    currentTask.Text = "Eliminando paquetes AppX..."
                Case 3
                    currentTask.Text = "Suppression des paquets AppX en cours..."
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
                        End Select
                    Case 1
                        currentTask.Text = "Removing package " & (x + 1) & " of " & appxRemovalCount & "..."
                    Case 2
                        currentTask.Text = "Eliminando paquete " & (x + 1) & " de " & appxRemovalCount & "..."
                    Case 3
                        currentTask.Text = "Suppression du paquet " & (x + 1) & " de " & appxRemovalCount & " en cours..."
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
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /remove-provisionedappxpackage /packagename=" & appxRemovalPackages(x)
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
                    End Select
                Case 1
                    currentTask.Text = "Adding capabilities..."
                Case 2
                    currentTask.Text = "Añadiendo funcionalidades..."
                Case 3
                    currentTask.Text = "Ajout des capacités en cours..."
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
                        End Select
                    Case 1
                        currentTask.Text = "Adding capability " & (x + 1) & " of " & capAdditionCount & "..."
                    Case 2
                        currentTask.Text = "Añadiendo funcionalidad " & (x + 1) & " de " & capAdditionCount & "..."
                    Case 3
                        currentTask.Text = "Ajout de la capacité " & (x + 1) & " de " & capAdditionCount & " en cours..."
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
                                           "- Capability identity: " & capInfo.DisplayName & CrLf & _
                                           "- Capability description: " & capInfo.Description & CrLf)
                    End Using
                Finally
                    DismApi.Shutdown()
                End Try
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /add-capability /capabilityname=" & capAdditionIds(x)
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
                        End Select
                    Case 1
                        taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                    Case 2
                        taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                    Case 3
                        taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
                End Select
                RunOps(8)
            End If
            If capSuccessfulAdditions > 0 Then
                GetErrorCode(True)
            ElseIf capSuccessfulAdditions <= 0 Then
                GetErrorCode(False)
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
                    End Select
                Case 1
                    currentTask.Text = "Removing capabilities..."
                Case 2
                    currentTask.Text = "Eliminando funcionalidades..."
                Case 3
                    currentTask.Text = "Suppression des capacités en cours..."
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
                        End Select
                    Case 1
                        currentTask.Text = "Removing capability " & (x + 1) & " of " & capRemovalCount & "..."
                    Case 2
                        currentTask.Text = "Eliminando funcionalidad " & (x + 1) & " de " & capRemovalCount & "..."
                    Case 3
                        currentTask.Text = "Suppression de la capacité " & (x + 1) & " de " & capRemovalCount & " en cours..."
                End Select
                CurrentPB.Value = x + 1
                LogView.AppendText(CrLf & _
                                   "Capability " & (x + 1) & " of " & capRemovalCount)
                Try
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    Using imgSession As DismSession = If(OnlineMgmt, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(mntString))
                        Dim capInfo As DismCapabilityInfo = DismApi.GetCapabilityInfo(imgSession, capRemovalIds(x))
                        LogView.AppendText(CrLf & CrLf & _
                                           "- Capability identity: " & capInfo.DisplayName & CrLf & _
                                           "- Capability description: " & capInfo.Description & CrLf)
                    End Using
                Finally
                    DismApi.Shutdown()
                End Try
                DISMProc.StartInfo.FileName = DismProgram
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /remove-capability /capabilityname=" & capRemovalIds(x)
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
                    End Select
                Case 1
                    currentTask.Text = "Adding drivers..."
                Case 2
                    currentTask.Text = "Añadiendo controladores..."
                Case 3
                    currentTask.Text = "Ajout des pilotes en cours..."
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
                        End Select
                    Case 1
                        currentTask.Text = "Adding driver " & (x + 1) & " of " & drvAdditionCount & "..."
                    Case 2
                        currentTask.Text = "Añadiendo controlador " & (x + 1) & " de " & drvAdditionCount & "..."
                    Case 3
                        currentTask.Text = "Ajout du pilote " & (x + 1) & " de " & drvAdditionCount & " en cours..."
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
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /add-driver /driver=" & Quote & drvAdditionPkgs(x) & Quote
                If drvAdditionForceUnsigned Then
                    CommandArgs &= " /forceunsigned"
                End If
                If File.GetAttributes(drvAdditionPkgs(x)) = FileAttributes.Directory And drvAdditionFolderRecursiveScan.Contains(drvAdditionPkgs(x)) Then
                    LogView.AppendText(CrLf & "This folder will be scanned recursively. Driver addition may take a longer time...")
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
                        End Select
                    Case 1
                        taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                    Case 2
                        taskCountLbl.Text = "Tareas: " & currentTCont & "/" & taskCount
                    Case 3
                        taskCountLbl.Text = "Tâches : " & currentTCont & "/" & taskCount
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
                    End Select
                Case 1
                    currentTask.Text = "Removing drivers..."
                Case 2
                    currentTask.Text = "Eliminando controladores..."
                Case 3
                    currentTask.Text = "Suppression des pilotes en cours..."
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
                        End Select
                    Case 1
                        currentTask.Text = "Removing driver " & (x + 1) & " of " & drvRemovalCount & "..."
                    Case 2
                        currentTask.Text = "Eliminando controlador " & (x + 1) & " de " & drvRemovalCount & "..."
                    Case 3
                        currentTask.Text = "Suppression du pilote " & (x + 1) & " de " & drvRemovalCount & " en cours..."
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
                                                   "- Version and date: " & drv.Version.ToString() & "/" & drv.Date.ToString() & CrLf & _
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
                CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /remove-driver /driver=" & Quote & drvRemovalPkgs(x) & Quote
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
                            CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /export-driver /destination=" & Quote & drvExportTarget & Quote
                    End Select
                Case 10
                    CommandArgs &= If(OnlineMgmt, " /online", " /image=" & Quote & MountDir & Quote) & " /export-driver /destination=" & Quote & drvExportTarget & Quote
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
        ElseIf opNum = 82 Then
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
            End Select
            LogView.AppendText(CrLf & "Setting the Windows PE scratch space..." & CrLf & _
                               "- New scratch space amount: " & peNewScratchSpace & " MB")
            CommandArgs &= " /image=" & Quote & MountDir & Quote & " /set-scratchspace=" & peNewScratchSpace
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
        ElseIf opNum = 83 Then
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
            End Select
            LogView.AppendText(CrLf & "Setting the Windows PE target path..." & CrLf & _
                               "- New target path: " & Quote & peNewTargetPath & Quote)
            CommandArgs &= " /image=" & Quote & MountDir & Quote & " /set-targetpath=" & peNewTargetPath
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
        ElseIf opNum = 87 Then
            allTasks.Text = "Setting the uninstall window..."
            currentTask.Text = "Setting the amount of days an uninstall can happen..."
            LogView.AppendText(CrLf & "Setting the amount of days an uninstall can happen..." & CrLf &
                               "Number of days: " & osUninstDayCount)
            DISMProc.StartInfo.FileName = DismProgram
            CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /online /set-osuninstallwindow /value:" & osUninstDayCount
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            DISMProc.WaitForExit()
            currentTask.Text = "Gathering error level..."
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
                    End Select
                Case 1
                    currentTask.Text = "Gathering error level..."
                Case 2
                    currentTask.Text = "Recopilando nivel de error..."
                Case 3
                    currentTask.Text = "Recueil du niveau d'erreur en cours..."
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
                    End Select
                Case 1
                    currentTask.Text = "Gathering error level..."
                Case 2
                    currentTask.Text = "Recopilando nivel de error..."
                Case 3
                    currentTask.Text = "Recueil du niveau d'erreur en cours..."
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
                    End Select
                Case 1
                    currentTask.Text = "Gathering error level..."
                Case 2
                    currentTask.Text = "Recopilando nivel de error..."
                Case 3
                    currentTask.Text = "Recueil du niveau d'erreur en cours..."
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
                        End Select
                    Case 1
                        currentTask.Text = "Unmounting source index..."
                    Case 2
                        currentTask.Text = "Desmontando índice de origen..."
                    Case 3
                        currentTask.Text = "Démontage de l'index original en cours..."
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
                        End Select
                    Case 1
                        currentTask.Text = "Gathering error level..."
                    Case 2
                        currentTask.Text = "Recopilando nivel de error..."
                    Case 3
                        currentTask.Text = "Recueil du niveau d'erreur en cours..."
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
                    End Select
                Case 1
                    currentTask.Text = "Gathering error level..."
                Case 2
                    currentTask.Text = "Recopilando nivel de error..."
                Case 3
                    currentTask.Text = "Recueil du niveau d'erreur en cours..."
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

    Sub AddFromMsu(MsuFile As String)
        Dim MsuName As String = Path.GetFileNameWithoutExtension(MsuFile)
        Dim CabFile As String = ""
        Dim pkgToAdd As String
        If Not Directory.Exists(pkgSource & "\MsuExtract") Then
            Directory.CreateDirectory(pkgSource & "\MsuExtract")
        End If
        File.WriteAllText(pkgSource & "\MsuExtract\readme.txt", _
                          "DISMTools Program Help - The " & Quote & "MsuExtract" & Quote & " folder" & CrLf & _
                          "------------------------------------------------" & CrLf & _
                          "The " & Quote & "MsuExtract" & Quote & " folder is created by DISMTools to extract the contents of MSU (Microsoft Update Package) files to independent folders." & CrLf & _
                          "You should not modify any of this folder's contents during addition, as they will be used by this program to:" & CrLf & _
                          "    a) Get package information. Neither this program nor DISM can obtain information from a standard (compressed) MSU package, and must be extracted (yes, they behave like ZIP files)" & CrLf & _
                          "    b) Attempt to add the package. The program will fail addition of a compressed MSU file. However, MSU files have an installation CAB file; which is valid for addition" & CrLf & _
                          "However, after adding the selected MSU files (or after the operation completes), you can feel free to delete this directory." & CrLf & _
                          "Also, you can check the Help documentation later to learn more.", ASCII)
        If Directory.Exists(pkgSource & "\MsuExtract\" & MsuName) Then
            Directory.Delete(pkgSource & "\MsuExtract\" & MsuName, True)
        End If
        Directory.CreateDirectory(pkgSource & "\MsuExtract\" & MsuName)
        If Environment.Is64BitOperatingSystem Then
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", _
                              "@echo off" & CrLf & _
                              Application.StartupPath & "\bin\utils\x64\7z.exe x " & MsuFile & " -o" & pkgSource & "\MsuExtract\" & MsuName, ASCII)
        Else
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", _
                              "@echo off" & CrLf & _
                              Application.StartupPath & "\bin\utils\x86\7z.exe x " & MsuFile & " -o" & pkgSource & "\MsuExtract\" & MsuName, ASCII)
        End If
        Process.Start(Application.StartupPath & "\bin\exthelpers\temp.bat").WaitForExit()
        If MsuName.StartsWith("windows6.1") Then    ' Windows 7
            If MsuName.Contains("-v") Then
                ' There may be no hotfix for Windows 7 that has "-v<n>"
            Else
                CabFile = MsuName.Remove(25)
            End If
        ElseIf MsuName.StartsWith("windows8-rt") Then   ' Windows 8
            If MsuName.Contains("-v") Then
                ' There may be no hotfix for Windows 8 that has "-v<n>"
            Else
                CabFile = MsuName.Remove(26)
            End If
        ElseIf MsuName.StartsWith("windows8.1") Then    ' Windows 8.1
            If MsuName.Contains("-v") Then
                ' There may be no hotfix for Windows 8.1 that has "-v<n>"
            Else
                CabFile = MsuName.Remove(25)
            End If
        ElseIf MsuName.StartsWith("windows10.0") Or MsuName.StartsWith("windows11.0") Then  ' Windows 10/11
            If MsuName.Contains("-v") Then
                If MsuName.Contains("x86") Then
                    CabFile = MsuName.Remove(28)
                ElseIf MsuName.Contains("x64") Then
                    CabFile = MsuName.Remove(28)
                ElseIf MsuName.Contains("arm64") Then
                    CabFile = MsuName.Remove(30)
                End If
            Else
                If MsuName.Contains("x86") Then
                    CabFile = MsuName.Remove(25)
                ElseIf MsuName.Contains("x64") Then
                    CabFile = MsuName.Remove(25)
                ElseIf MsuName.Contains("arm64") Then
                    CabFile = MsuName.Remove(25)
                End If
            End If
        End If
        CabFile &= ".cab"
        pkgToAdd = pkgSource & "\MsuExtract\" & MsuName & "\" & CabFile
        ' Add MSU file
        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\pkginfo.bat", _
                          "@echo off" & CrLf & _
                          "dism /English /image=" & MountDir & " /get-packageinfo /packagepath=" & pkgToAdd & " | findstr /c:" & Quote & "Package Identity" & Quote & " > .\tempinfo\pkgname" & CrLf & _
                          "dism /English /image=" & MountDir & " /get-packageinfo /packagepath=" & pkgToAdd & " | findstr /c:" & Quote & "Description" & Quote & " > .\tempinfo\pkgdesc" & CrLf & _
                          "dism /English /image=" & MountDir & " /get-packageinfo /packagepath=" & pkgToAdd & " | findstr /c:" & Quote & "Applicable" & Quote & " > .\tempinfo\pkgapplicability" & CrLf & _
                          "dism /English /image=" & MountDir & " /get-packageinfo /packagepath=" & pkgToAdd & " | findstr /c:" & Quote & "State" & Quote & " > .\tempinfo\pkgstate", _
                          ASCII)
        If IsDebugged Then
            Process.Start("\Windows\system32\notepad.exe", Application.StartupPath & "\bin\exthelpers\pkginfo.bat").WaitForExit()
        End If
        Process.Start(Application.StartupPath & "\bin\exthelpers\pkginfo.bat").WaitForExit()
        pkgName = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\tempinfo\pkgname", ASCII).Replace("Package Identity : ", "").Trim()
        pkgDesc = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\tempinfo\pkgdesc", ASCII).Replace("Description : ", "").Trim()
        pkgApplicabilityStatus = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\tempinfo\pkgapplicability", ASCII).Replace("Applicable : ", "").Trim()
        pkgInstallationState = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\tempinfo\pkgstate", ASCII).Replace("State : ", "").Trim()
        LogView.AppendText(CrLf & CrLf & _
                           "- Package name: " & pkgName & CrLf & _
                           "- Package description: " & pkgDesc & CrLf)
        If pkgApplicabilityStatus = "Yes" Then
            pkgIsApplicable = True
            LogView.AppendText("- Package is applicable to this image? Yes" & CrLf)
        Else
            pkgIsApplicable = False
            LogView.AppendText("- Package is applicable to this image? No" & CrLf)
        End If
        If pkgInstallationState = "Not Present" Or pkgInstallationState = "" Then
            pkgIsAlreadyAdded = False
            LogView.AppendText("- Package is already added? No" & CrLf)
        ElseIf pkgInstallationState = "Install Pending" Then
            pkgIsAlreadyAdded = True
            LogView.AppendText("- Package is already added? Yes" & CrLf)
        ElseIf pkgInstallationState = "Installed" Then
            pkgIsAlreadyAdded = True
            LogView.AppendText("- Package is already added? Yes" & CrLf)
        End If
        Try
            Directory.Delete(Application.StartupPath & "\tempinfo", True)
        Catch ex As Exception

        End Try

        ' Preparing to add pkg
        If pkgIsApplicable Then
            ' Determine whether package is already added (either Install Pending or Installed)
            If pkgIsAlreadyAdded Then
                LogView.AppendText(CrLf & "Package is already added. Skipping installation of this package...")
                pkgFailedAdditions += 1
                Exit Sub
            Else
                LogView.AppendText(CrLf & "Processing package...")
                DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /image=" & MountDir & " /add-package /packagepath=" & pkgToAdd
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
            End If
        Else
            If pkgIgnoreApplicabilityChecks Then
                LogView.AppendText(CrLf & "Trying to process package...")
                DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
                CommandArgs = "/logpath=" & Quote & Application.StartupPath & "\logs\" & GetCurrentDateAndTime(Now) & Quote & " /english /image=" & MountDir & " /add-package /packagepath=" & pkgToAdd & " /ignorecheck"
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
            Else
                LogView.AppendText(CrLf & "Package is not applicable to this image. Skipping installation of this package...")
                If PkgErrorText.RichTextBox1.Text = "" Then
                    PkgErrorText.RichTextBox1.AppendText("0x800F8023")
                Else
                    PkgErrorText.RichTextBox1.AppendText(CrLf & "0x800F8023")
                End If
                pkgFailedAdditions += 1
                'CurrentPB.Value = x + 1
                Exit Sub
            End If
        End If
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
            If OperationNum < 993 Then
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
                If Not MainForm.OnlineManagement Then MainForm.SaveDTProj()
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
                If Not MainForm.OnlineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 30 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 2
                End If
                If Not MainForm.OnlineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 31 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 2
                End If
                If Not MainForm.OnlineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 33 Then
                If Not MainForm.OnlineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 37 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 3
                End If
                If Not MainForm.OnlineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 38 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 3
                End If
                If Not MainForm.OnlineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 64 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 4
                End If
                If Not MainForm.OnlineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 68 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 4
                End If
                If Not MainForm.OnlineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 75 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 5
                End If
                If Not MainForm.OnlineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 76 Then
                If Not MainForm.RunAllProcs Then
                    MainForm.bwGetImageInfo = False
                    MainForm.bwGetAdvImgInfo = False
                    MainForm.bwBackgroundProcessAction = 5
                End If
                If Not MainForm.OnlineManagement Then MainForm.SaveDTProj()
                MainForm.UpdateProjProperties(True, False)
            ElseIf OperationNum = 991 Then
                Visible = False
                ImgConversionSuccessDialog.ShowDialog(MainForm)
                If ImgConversionSuccessDialog.DialogResult = Windows.Forms.DialogResult.OK Then
                    Process.Start("\Windows\explorer.exe", Path.GetDirectoryName(imgDestFile))
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
                    End Select
                Case 1
                    MainForm.MenuDesc.Text = "Ready"
                Case 2
                    MainForm.MenuDesc.Text = "Listo"
                Case 3
                    MainForm.MenuDesc.Text = "Prêt"
            End Select
            ActionRunning = False
            TaskList.Clear()
            MainForm.StatusStrip.BackColor = Color.FromArgb(0, 122, 204)
            MainForm.ToolStripButton4.Visible = False
            If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
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
                    End Select
                Case 1
                    Cancel_Button.Text = "OK"
                Case 2
                    Cancel_Button.Text = "Aceptar"
                Case 3
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
                                          "Do note that, if the image came from an installation media, you may need to copy the source file to perform modifications to it.")
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
                LogView.AppendText(CrLf & "The specified operation completed successfully, but requires a restart in order to be fully applied. Save your work and restart when ready")
            Else
                ' Errors that weren't added to the database
                LogView.AppendText(CrLf & "This error has not yet been added to the database, so a useful description can't be shown now. Try running the command manually and, if you see the same error, try looking it up on the Internet.")
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
                    End Select
                Case 1
                    MainForm.MenuDesc.Text = "Ready"
                Case 2
                    MainForm.MenuDesc.Text = "Listo"
                Case 3
                    MainForm.MenuDesc.Text = "Prêt"
            End Select
            MainForm.StatusStrip.BackColor = Color.FromArgb(0, 122, 204)
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
        End Select
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
        MainForm.MountedImageDetectorBW.CancelAsync()
        While MainForm.MountedImageDetectorBW.IsBusy
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
            LogView.Font = New Font("Courier New", 9.75)
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
                End Select
            Case 1
                MainForm.MenuDesc.Text = "Performing image operations. Please wait..."
            Case 2
                MainForm.MenuDesc.Text = "Realizando operaciones con la imagen. Espere..."
            Case 3
                MainForm.MenuDesc.Text = "Exécution d'opérations sur les images en cours. Veuillez patienter..."
        End Select
        MainForm.StatusStrip.BackColor = Color.FromArgb(14, 99, 156)
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
                    End Select
                Case 1
                    taskCountLbl.Text = "Tasks: 1/" & TaskList.Count
                Case 2
                    taskCountLbl.Text = "Tareas: 1/" & TaskList.Count
                Case 3
                    taskCountLbl.Text = "Tâches : 1/" & TaskList.Count
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
                        End Select
                    Case 1
                        taskCountLbl.Text = "Tasks: 1/" & TaskList.Count
                    Case 2
                        taskCountLbl.Text = "Tareas: 1/" & TaskList.Count
                    Case 3
                        taskCountLbl.Text = "Tâches : 1/" & TaskList.Count
                End Select
                OperationNum = 1000
            Else
                GetTasks(OperationNum)
            End If
        End If
        taskCountLbl.Visible = True
        ProgressBW.RunWorkerAsync()
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
        ControlPaint.DrawBorder(e.Graphics, BodyPanel.ClientRectangle, Color.FromArgb(0, 122, 204), ButtonBorderStyle.Solid)
    End Sub

    Private Sub ProgressPanel_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If ValidationForm.Visible Then ValidationForm.Close()
    End Sub
End Class