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


Imports Microsoft.VisualBasic.ControlChars
Imports System.Threading
Imports System.IO
Imports System.Net
Imports System.Text.Encoding


Public Class ProgressPanel

    Public taskCount As Long
    Dim currentTCont As Integer = 1
    Public OperationNum As Long

    Public IsSuccessful As Boolean
    Public IsDebugged As Boolean

    Public errCode As Integer

    Public CommandArgs As String = ""           ' Ubiquitous accross OperationNums. DO NOT DELETE !!!

    ' OperationNum: 0
    Public projName As String
    Public projPath As String
    Public MountAfterCreation As Boolean

    ' OperationNum: 3
    Public ApplicationSourceImg As String       ' String which determines which image to apply
    Public ApplicationIndex As Integer          ' Index to apply to destination
    Public ApplicationDestDir As String         ' Destination directory to apply image to
    Public ApplicationCheckInt As Boolean       ' Determine whether to check image corruption before applying
    Public ApplicationVerify As Boolean         ' Determine whether to check for file duplication and errors
    Public ApplicationReparsePt As Boolean      ' Determine whether to use reparse points
    Public ApplicationSWMPattern As String      ' Spanned/Split WIM (SWM) file pattern string. Usually "install*.swm", so don't use an array
    Public ApplicationValidateForTD As Boolean  ' Determine whether to validate image for Trusted Desktop (WinPE 4.0+ only)
    Public ApplicationUseWimBoot As Boolean     ' Determine whether to append image with WIMBoot configuration
    Public ApplicationCompactMode As Boolean    ' Determine whether to apply image in Compact mode (Win10+ only)
    Public ApplicationUseExtAttr As Boolean     ' Determine whether to apply extended attributes (Win10 1607+ only)
    Public ApplicationDestDrive As String       ' Gather destination disk ID

    ' OperationNum: 6
    Public CaptureSourceDir As String           ' Source directory to be captured
    Public CaptureDestinationImage As String    ' Destination image
    Public CaptureName As String                ' Captured image name
    Public CaptureDescription As String         ' Captured image description (optional)
    Public CaptureWimScriptConfig As String     ' Path for WimScript.ini
    Public CaptureCompressType As Integer       ' Compression used for the capture (0: none; 1: fast; 2: max)
    Public CaptureBootable As Boolean           ' Make captured image bootable (WinPE only)
    Public CaptureCheckIntegrity As Boolean     ' Check integrity of WIM file
    Public CaptureVerify As Boolean             ' Check for errors and file duplication
    Public CaptureReparsePt As Boolean          ' Determine whether to use the reparse point tag fix
    Public CaptureUseWimBoot As Boolean         ' Determine whether to append image with WIMBoot configuration
    Public CaptureExtendedAttributes As Boolean ' Determine whether to capture extended attributes (Win10 1607+ only)
    Public CaptureMountDestImg As Boolean       ' Determine whether to unmount the source VHD(X) file and mount the destination image (still experimental)

    ' OperationNum: 11
    Public GetFromMountedImg As Boolean         ' Get information from mounted image
    Public GetSpecificIndexInfo As Boolean      ' Get information from specific image index
    Public GetFromMountedIndex As Boolean       ' Get information from mounted image index
    Public InfoFromSourceImg As String          ' Source image information string
    Public InfoFromSpecificImg As String        ' Specific image information string
    Public InfoFromSourceIndex As Integer       ' Source image index information int
    Public InfoFromSpecificIndex As Integer     ' Specific image index information int

    ' OperationNum: 15
    Public SourceImg As String                  ' Mandatory
    Public ImgIndex As Integer                  ' Mandatory
    Public MountDir As String                   ' Mandatory
    Public isReadOnly As Boolean                ' Determine whether image will be mounted with read-only permissions
    Public isOptimized As Boolean               ' Determine whether image will be optimized to mount in a shorter time
    Public isIntegrityTested As Boolean         ' Determine whether the integrity of the image should be tested before mounting the image

    ' OperationNum: 18
    Public remountisReadOnly As Boolean         ' Determine whether the remount happened because of a read-only mounted image
    Public isTriggeredByPropertyDialog As Boolean = False

    ' OperationNum: 21
    Public UMountImgIndex As Integer
    Public ProgramIsBeingClosed As Boolean
    Public UMountLocalDir As Boolean
    Public UMountOp As Integer                  ' 0: commit, then unmount; 1: unmount without saving
    Public RandomMountDir As String             ' Don't know about that mount dir, other that it was not loaded
    Public CheckImgIntegrity As Boolean
    Public SaveToNewIndex As Boolean

    ' OperationNum: 26
    Public pkgSource As String                  ' Determine where the packages came from
    Dim pkgName As String                       ' Determine how the package is called
    Dim pkgDesc As String                       ' Determine package description (e.g., "Fix for KB5014113")
    Dim pkgApplicabilityStatus As String        ' Determine whether or not package is applicable
    Dim pkgInstallationState As String          ' Determine whether or not package was installed
    Dim pkgPossibleMsuFile As String            ' Determine whether package is a MSU file
    Public pkgs(65535) As String                ' Array used to determine package locations. DO NOT DELETE !!!
    Public pkgLastCheckedPackageName As String  ' Last index name of the aforementioned array. DO NOT DELETE !!!
    Public pkgIsApplicable As Boolean           ' Using data from pkgApplicabilityStatus, determine whether package is applicable
    Public pkgIsAlreadyAdded As Boolean         ' Using data from pkgInstallationState, determine whether package is installed
    Public pkgIgnoreApplicabilityChecks As Boolean ' If option is checked, ignore applicability checks
    Public pkgPreventIfPendingOnline As Boolean ' If option is checked, ignore package if online actions are required on the image
    Public imgCommitAfterOps As Boolean         ' If option is checked, commit image after operations are done
    Public pkgAdditionOp As Integer             ' 0: recursive operation; 1: selective operation
    Public pkgCount As Integer                  ' Gather package count
    Public pkgCurrentNum As Integer             ' Current package number
    Public pkgSuccessfulAdditions As Integer    ' Determine successful package additions
    Public pkgFailedAdditions As Integer        ' Determine failed package additions

    ' <Space for other OperationNums>
    ' OperationNum: 87
    Public osUninstDayCount As Integer          ' Number of days the user has to uninstall an OS upgrade

    ' OperationNum: 991
    Public imgSrcFile As String                 ' Source image file for conversion
    Public imgDestFile As String                ' Destination image file for conversion
    Public imgConversionMode As Integer         ' 0: WIM -> ESD; 1: WIM <- ESD

    ' OperationNum: 992
    Public imgSwmSource As String               ' Source SWM file to merge its pattern to WIM
    Public imgWimDestination As String          ' Destination WIM file to merge SWM files to

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        If Cancel_Button.Text = "Cancel" Then
            ProgressBW.CancelAsync()
        ElseIf Cancel_Button.Text = "OK" Then
            MainForm.ToolStripButton4.Visible = False
            Dispose()
            Close()
        End If
    End Sub

    Private Sub LogButton_Click(sender As Object, e As EventArgs) Handles LogButton.Click
        If LogButton.Text = "Show log" Then
            LogButton.Text = "Hide log"
            Height = 420
        ElseIf LogButton.Text = "Hide log" Then
            LogButton.Text = "Show log"
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
        ElseIf opNum = 15 Then
            taskCount = 1
        ElseIf opNum = 18 Then
            taskCount = 1
        ElseIf opNum = 21 Then
            taskCount = 1
        ElseIf opNum = 26 Then
            If imgCommitAfterOps Then
                taskCount = 2
            Else
                taskCount = 1
            End If
        ElseIf opNum = 87 Then
            taskCount = 1
        ElseIf opNum = 991 Then
            taskCount = 1
        ElseIf opNum = 992 Then
            taskCount = 1
        End If
        AllPB.Maximum = taskCount * 100
        taskCountLbl.Text = "Tasks: 1/" & taskCount
        CenterToParent()
    End Sub

    Sub GatherInitialSwitches()
        ' This procedure is not yet called. Use default settings for now
    End Sub

    Sub ChangeLogViewFont(Font As String)
        ' This procedure is not yet called. Use default font for now
    End Sub

    Sub RunOps(opNum As Integer)
        CurrentPB.Value = 0
        If opNum = 0 Then
            allTasks.Text = "Creating project: " & Quote & projName & Quote
            currentTask.Text = "Creating DISMTools project structure..."
            LogView.AppendText(CrLf & "Creating project structure...")
            Try
                Directory.CreateDirectory(projPath & projName)
                CurrentPB.Value = 16.66
                Thread.Sleep(125)
                AllPB.Value = CurrentPB.Value
                Directory.CreateDirectory(projPath & projName & "\" & "settings")
                CurrentPB.Value = 33.33
                Thread.Sleep(125)
                AllPB.Value = CurrentPB.Value
                Directory.CreateDirectory(projPath & projName & "\" & "mount")
                CurrentPB.Value = 50
                Thread.Sleep(125)
                AllPB.Value = CurrentPB.Value
                Directory.CreateDirectory(projPath & projName & "\" & "scr_temp")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win51_cons")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win60_cons")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win61_cons")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win62_cons")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win62_cons\mbr")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win62_cons\uefi")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win63_cons")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win63_cons\mbr")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win63_cons\uefi")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win10_cons")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win10_cons\mbr")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win10_cons\uefi")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win60_serv")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win61_serv")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win62_serv")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win63_serv")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\win10_serv")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\sbs08")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\sbs11")
                Directory.CreateDirectory(projPath & projName & "\" & "unattend_xml\whs11")
                Directory.CreateDirectory(projPath & projName & "\" & "reports")
                Directory.CreateDirectory(projPath & projName & "\" & "DandI")
                Directory.CreateDirectory(projPath & projName & "\" & "DandI\x86")
                Directory.CreateDirectory(projPath & projName & "\" & "DandI\amd64")
                Directory.CreateDirectory(projPath & projName & "\" & "DandI\arm")
                Directory.CreateDirectory(projPath & projName & "\" & "DandI\arm64")
                CurrentPB.Value = 66.66
                Thread.Sleep(125)
                AllPB.Value = CurrentPB.Value
                File.WriteAllText(projPath & projName & "\" & "settings\project.ini", _
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
                File.WriteAllText(projPath & projName & "\" & projName & ".dtproj", _
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
            allTasks.Text = "Applying image..."
            currentTask.Text = "Applying specified image to the specified destination..."
            LogView.AppendText(CrLf & "Applying image..." & CrLf & "Options:" & CrLf & _
                               "- Source image file: " & ApplicationSourceImg & CrLf & _
                               "- Index to apply: " & ApplicationIndex & CrLf & _
                               "- Target directory: " & ApplicationDestDir & CrLf)
            DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
            CommandArgs = "/english /apply-image /imagefile=" & ApplicationSourceImg & " /index=" & ApplicationIndex
            ' Detect additional options and set CommandArgs
            If ApplicationDestDrive = "" Then
                CommandArgs = CommandArgs & " /applydir=" & ApplicationDestDir
            Else
                CommandArgs = CommandArgs & " /applydrive=" & ApplicationDestDrive
            End If
            If ApplicationCheckInt Then
                LogView.AppendText("- Verify image integrity? Yes" & CrLf)
                CommandArgs = CommandArgs & " /checkintegrity"
            Else
                LogView.AppendText("- Verify image integrity? No" & CrLf)
            End If
            If ApplicationVerify Then
                LogView.AppendText("- Check for file errors? Yes" & CrLf)
                CommandArgs = CommandArgs & " /verify"
            Else
                LogView.AppendText("- Check for file errors? No" & CrLf)
            End If
            If ApplicationReparsePt Then
                LogView.AppendText("- Use reparse point tag fix? Yes" & CrLf)
            Else
                LogView.AppendText("- Use reparse point tag fix? No" & CrLf)
                CommandArgs = CommandArgs & " /norpfix"
            End If
            If ApplicationSWMPattern = "" Then
                LogView.AppendText("- Split WIM (SWM) file pattern: not specified/not using SWM file" & CrLf)
            Else
                LogView.AppendText("- Split WIM (SWM) file pattern: " & ApplicationSWMPattern & CrLf)
                CommandArgs = CommandArgs & " /swmfile=" & ApplicationSWMPattern
            End If
            If ApplicationValidateForTD Then
                LogView.AppendText("- Validate for Trusted Desktop? Yes" & CrLf)
                CommandArgs = CommandArgs & " /confirmtrustedfile"
            Else
                LogView.AppendText("- Validate for Trusted Desktop? No/Not supported" & CrLf)
            End If
            If ApplicationUseWimBoot Then
                LogView.AppendText("- Apply using WIMBoot configuration? Yes" & CrLf)
                CommandArgs = CommandArgs & " /wimboot"
            Else
                LogView.AppendText("- Apply using WIMBoot configuration? No" & CrLf)
            End If
            If ApplicationCompactMode Then
                LogView.AppendText("- Use Compact mode? Yes" & CrLf)
                CommandArgs = CommandArgs & " /compact"
            Else
                LogView.AppendText("- Use Compact mode? No" & CrLf)
            End If
            If ApplicationUseExtAttr Then
                LogView.AppendText("- Apply using extended attributes? Yes" & CrLf)
                CommandArgs = CommandArgs & " /ea"
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
            Do Until DISMProc.HasExited
                If DISMProc.HasExited Then
                    Exit Do
                End If
            Loop
            currentTask.Text = "Gathering error level..."
            LogView.AppendText(CrLf & "Gathering error level...")
            GetErrorCode(False)
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        ElseIf opNum = 6 Then
            allTasks.Text = "Capturing image..."
            currentTask.Text = "Capturing specified directory into a new image..."
            LogView.AppendText(CrLf & "Capturing directory..." & CrLf & "Options:" & CrLf & _
                               "- Source directory: " & CaptureSourceDir & CrLf & _
                               "- Destination image: " & CaptureDestinationImage & CrLf & _
                               "- Captured image name: " & CaptureName & CrLf)
            DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
            CommandArgs = "/English /capture-image /imagefile=" & CaptureDestinationImage & " /capturedir=" & CaptureSourceDir & " /name=" & Quote & CaptureName & Quote
            ' Get additional options
            If CaptureDescription = "" Then
                LogView.AppendText("- Captured image description: none specified" & CrLf)
            Else
                LogView.AppendText("- Captured image description: " & Quote & CaptureDescription & Quote & CrLf)
                CommandArgs = CommandArgs & " /description=" & Quote & CaptureDescription & Quote
            End If
            If CaptureWimScriptConfig = "" Then
                LogView.AppendText("- " & Quote & "WimScript.ini" & Quote & " configuration file: not specified" & CrLf)
            Else
                LogView.AppendText("- " & Quote & "WimScript.ini" & Quote & " configuration file: " & CaptureWimScriptConfig & CrLf)
                ' Possibly, the file may have been deleted after being specified. Determine whether it still exists
                If File.Exists(CaptureWimScriptConfig) Then
                    CommandArgs = CommandArgs & " /configfile=" & Quote & CaptureWimScriptConfig & Quote
                Else
                    LogView.AppendText("   WARNING: the following file does not exist in the file system. Skipping file..." & CrLf)
                End If
            End If
            If CaptureCompressType = 0 Then
                LogView.AppendText("- Compression type: none" & CrLf)
                CommandArgs = CommandArgs & " /compress=none"
            ElseIf CaptureCompressType = 1 Then
                LogView.AppendText("- Compression type: fast" & CrLf)
                CommandArgs = CommandArgs & " /compress=fast"
            ElseIf CaptureCompressType = 2 Then
                LogView.AppendText("- Compression type: maximum" & CrLf)
                CommandArgs = CommandArgs & " /compress=max"
            End If
            If CaptureBootable Then
                LogView.AppendText("- Mark image as bootable? Yes" & CrLf)
                CommandArgs = CommandArgs & " /bootable"
            Else
                LogView.AppendText("- Mark image as bootable? No" & CrLf)
            End If
            If CaptureCheckIntegrity Then
                LogView.AppendText("- Check image integrity? Yes" & CrLf)
                CommandArgs = CommandArgs & " /checkintegrity"
            Else
                LogView.AppendText("- Check image integrity? No" & CrLf)
            End If
            If CaptureVerify Then
                LogView.AppendText("- Verify file errors? Yes" & CrLf)
                CommandArgs = CommandArgs & " /verify"
            Else
                LogView.AppendText("- Verify file errors? No" & CrLf)
            End If
            If CaptureReparsePt Then
                LogView.AppendText("- Use the Reparse Point tag fix? Yes" & CrLf)
            Else
                LogView.AppendText("- Use the Reparse Point tag fix? No" & CrLf)
                CommandArgs = CommandArgs & " /norpfix"
            End If
            If CaptureUseWimBoot Then
                LogView.AppendText("- Append with WIMBoot configuration? Yes" & CrLf)
                CommandArgs = CommandArgs & " /wimboot"
            Else
                LogView.AppendText("- Append with WIMBoot configuration? No" & CrLf)
            End If
            If CaptureExtendedAttributes Then
                LogView.AppendText("- Capture extended attributes? Yes" & CrLf)
                CommandArgs = CommandArgs & " /ea"
            Else
                LogView.AppendText("- Capture extended attributes? No" & CrLf)
            End If
            LogView.AppendText(CrLf & "Capturing image...")
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            Do Until DISMProc.HasExited
                If DISMProc.HasExited Then
                    Exit Do
                End If
            Loop
            currentTask.Text = "Gathering error level..."
            LogView.AppendText(CrLf & "Gathering error level...")
            GetErrorCode(False)
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
            If CaptureMountDestImg Then
                AllPB.Value = AllPB.Value + (AllPB.Maximum / taskCount)
                currentTCont += 1
                taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                If ImgIndex = 0 Then
                    ImgIndex = 1
                    UMountImgIndex = ImgIndex
                End If
                RunOps(21)
                AllPB.Value = AllPB.Value + (AllPB.Maximum / taskCount)
                currentTCont += 1
                taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                RunOps(15)
                'MainForm.UpdateProjProperties(False, False)
                'MainForm.SaveDTProj()
            End If
        ElseIf opNum = 8 Then
            allTasks.Text = "Committing image..."
            currentTask.Text = "Saving changes to the image..."
            LogView.AppendText(CrLf & "Saving changes..." & CrLf & "Options:" & CrLf & _
                               "- Mount directory: " & MountDir)

            DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
            CommandArgs = "/english /commit-image /mountdir=" & MountDir
            ' TODO: Add additional options later
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            Do Until DISMProc.HasExited
                If DISMProc.HasExited Then
                    Exit Do
                End If
            Loop
            currentTask.Text = "Gathering error level..."
            LogView.AppendText(CrLf & "Gathering error level...")
            GetErrorCode(False)
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        ElseIf opNum = 11 Then
            CommandArgs = "/English /Get-ImageInfo"
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
                CommandArgs = CommandArgs & " /ImageFile:" & InfoFromSourceImg
                If GetSpecificIndexInfo Then
                    If GetFromMountedIndex Then
                        LogView.AppendText(CrLf & "- Image index: " & InfoFromSourceIndex & " (mounted index)")
                        CommandArgs = CommandArgs & " /Index:" & InfoFromSourceIndex
                    Else
                        LogView.AppendText(CrLf & "- Image index: " & InfoFromSpecificIndex)
                        CommandArgs = CommandArgs & " /Index:" & InfoFromSpecificIndex
                    End If
                End If
            Else
                LogView.AppendText(CrLf & "- Image file: " & InfoFromSpecificImg)
                CommandArgs = CommandArgs & " /ImageFile:" & InfoFromSpecificImg
                If GetSpecificIndexInfo Then
                    ' This version still does not support getting image information from its mounted index if it is an external image
                    LogView.AppendText(CrLf & "- Image index: " & InfoFromSpecificIndex)
                    CommandArgs = CommandArgs & " /Index:" & InfoFromSpecificIndex
                End If
            End If
            ' Run process. Right now, use an external process to do all the job
            File.WriteAllText(".\bin\exthelpers\temp.bat", _
                              "@echo off" & CrLf & _
                              "dism " & CommandArgs & " > ", _
                              ASCII)
        ElseIf opNum = 15 Then
            allTasks.Text = "Mounting image..."
            currentTask.Text = "Mounting specified image..."
            LogView.AppendText(CrLf & "Mounting image..." & CrLf & "Options:" & CrLf & _
                               "- Image file: " & SourceImg & CrLf & _
                               "- Image index: " & ImgIndex & CrLf & _
                               "- Mount point: " & MountDir)
            DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
            CommandArgs = "/English /mount-image /imagefile=" & SourceImg & " /index=" & ImgIndex & " /mountdir=" & MountDir
            If isReadOnly Then
                LogView.AppendText(CrLf & "- Mount image with read-only permissions? Yes")
                CommandArgs = CommandArgs & " /readonly"
            Else
                LogView.AppendText(CrLf & "- Mount image with read-only permissions? No")
            End If
            If isOptimized Then
                LogView.AppendText(CrLf & "- Optimize mount time? Yes")
                CommandArgs = CommandArgs & " /optimize"
            Else
                LogView.AppendText(CrLf & "- Optimize mount time? No")
            End If
            If isIntegrityTested Then
                LogView.AppendText(CrLf & "- Check image integrity? Yes")
                CommandArgs = CommandArgs & " /checkintegrity"
            Else
                LogView.AppendText(CrLf & "- Check image integrity? No")
            End If
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            Do Until DISMProc.HasExited
                If DISMProc.HasExited Then
                    Exit Do
                End If
            Loop
            currentTask.Text = "Gathering error level..."
            LogView.AppendText(CrLf & "Gathering error level...")
            GetErrorCode(False)
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        ElseIf opNum = 18 Then
            allTasks.Text = "Remounting image..."
            currentTask.Text = "Reloading servicing session for mounted image..."
            LogView.AppendText(CrLf & "Reloading servicing session..." & CrLf & _
                               "- Mount directory: " & MountDir)
            DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
            CommandArgs = "/English /remount-image /mountdir=" & MountDir
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            Do Until DISMProc.HasExited
                If DISMProc.HasExited Then
                    Exit Do
                End If
            Loop
            CurrentPB.Value = 50
            AllPB.Value = CurrentPB.Value
            currentTask.Text = "Gathering error level..."
            LogView.AppendText(CrLf & "Gathering error level...")
            GetErrorCode(False)
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        ElseIf opNum = 21 Then
            If UMountLocalDir Then
                'MountDir = MainForm.MountDir
                allTasks.Text = "Unmounting image..."
                currentTask.Text = "Unmounting image file..."
                LogView.AppendText(CrLf & "Unmounting image file from mount point..." & CrLf & _
                                   "- Mount directory: " & MountDir & CrLf & _
                                   "- Image index: " & UMountImgIndex)
            Else
                MountDir = RandomMountDir
                allTasks.Text = "Unmountimg image..."
                currentTask.Text = "Unmounting image file..."
                LogView.AppendText(CrLf & "Unmounting image file from mount point..." & CrLf & _
                                   "- Mount directory: " & MountDir & CrLf & _
                                   "- Image index: " & UMountImgIndex)
            End If
            If ProgramIsBeingClosed Then
                LogView.AppendText(CrLf & "- Unmount operation: Commit")
                ' Commit the image and unmount it
                Try
                    DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
                    CommandArgs = "/English /unmount-image /mountdir=" & MountDir & " /commit"
                    DISMProc.StartInfo.Arguments = CommandArgs
                    DISMProc.Start()
                    Do Until DISMProc.HasExited
                        If DISMProc.HasExited Then
                            Exit Do
                        End If
                    Loop
                    If DISMProc.ExitCode = Decimal.ToInt32(-1052638964) Then
                        LogView.AppendText(CrLf & CrLf & "Saving changes to the image has failed. Discarding changes...")
                        ' It mostly came from a read-only source. Discard changes
                        DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
                        CommandArgs = "/English /unmount-image /mountdir=" & MountDir & " /discard"
                        DISMProc.StartInfo.Arguments = CommandArgs
                        DISMProc.Start()
                        Do Until DISMProc.HasExited
                            If DISMProc.HasExited Then
                                Exit Do
                            End If
                        Loop
                    End If
                Catch ex As Exception
                    File.WriteAllText(".\bin\exthelpers\temp.bat", _
                                      "@echo off" & CrLf & _
                                      "dism /English /unmount-image /mountdir=" & MountDir, _
                                      ASCII)
                    Process.Start(".\bin\exthelpers\temp.bat").WaitForExit()
                End Try
                currentTask.Text = "Gathering error level..."
                LogView.AppendText(CrLf & "Gathering error level...")
                GetErrorCode(False)
                LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
            Else
                Try
                    DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
                    If UMountLocalDir Then
                        CommandArgs = "/English /unmount-image /mountdir=" & MountDir
                    Else
                        CommandArgs = "/English /unmount-image /mountdir=" & RandomMountDir
                    End If
                    If UMountOp = 0 Then
                        LogView.AppendText(CrLf & "- Unmount operation: Commit")
                        CommandArgs = CommandArgs & " /commit"
                    ElseIf UMountOp = 1 Then
                        LogView.AppendText(CrLf & "- Unmount operation: Discard")
                        CommandArgs = CommandArgs & " /discard"
                    End If
                    LogView.AppendText(CrLf & "Additional options:")
                    If CheckImgIntegrity Then
                        LogView.AppendText(" img_checkint")
                        CommandArgs = CommandArgs & " /checkintegrity"
                    End If
                    If SaveToNewIndex Then
                        LogView.AppendText(", img_append")
                        CommandArgs = CommandArgs & " /append"
                    End If
                    If Not CheckImgIntegrity And Not SaveToNewIndex Then
                        LogView.AppendText(" none")
                    End If
                    DISMProc.StartInfo.Arguments = CommandArgs
                    DISMProc.Start()
                    Do Until DISMProc.HasExited
                        If DISMProc.HasExited Then
                            Exit Do
                        End If
                    Loop
                Catch ex As Exception
                    ' Let's try this before setting things up here
                End Try
                currentTask.Text = "Gathering error level..."
                LogView.AppendText(CrLf & "Gathering error level...")
                GetErrorCode(False)
                LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
            End If
        ElseIf opNum = 26 Then
            'If IsDebugged Then
            '    PkgErrorText.Show()
            'Else
            '    PkgErrorText.Validate()
            'End If
            ' Reset internal integers
            pkgCurrentNum = 0
            allTasks.Text = "Adding packages..."
            currentTask.Text = "Preparing to add packages..."
            LogView.AppendText(CrLf & "Adding packages to mounted image..." & CrLf & _
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
                LogView.AppendText(CrLf & "- Prevent package addition if online actions need to be performed? Yes" & CrLf & _
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
            currentTask.Text = "Adding " & pkgCount & " packages..."
            CurrentPB.Style = ProgressBarStyle.Blocks
            LogView.AppendText(CrLf & CrLf & _
                               "Processing " & pkgCount & " packages..." & CrLf)
            If pkgAdditionOp = 0 Then
                DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
                CommandArgs = "/English /image=" & MountDir & " /add-package /packagepath=" & pkgSource
                If pkgIgnoreApplicabilityChecks Then
                    CommandArgs = CommandArgs & " /ignorecheck"
                End If
                If pkgPreventIfPendingOnline Then
                    CommandArgs = CommandArgs & " /preventpending"
                End If
                DISMProc.StartInfo.Arguments = CommandArgs
                DISMProc.Start()
                Do Until DISMProc.HasExited
                    If DISMProc.HasExited Then
                        Exit Do
                    End If
                Loop
                currentTask.Text = "Gathering error level..."
                LogView.AppendText(CrLf & "Gathering error level...")
                GetErrorCode(False)
                LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
            ElseIf pkgAdditionOp = 1 Then
                CurrentPB.Maximum = pkgCount
                For x = 0 To Array.LastIndexOf(pkgs, pkgLastCheckedPackageName)
                    currentTask.Text = "Adding package " & (x + 1) & " of " & pkgCount & "..."
                    CurrentPB.Value = x + 1
                    LogView.AppendText(CrLf & _
                                       "Package " & (x + 1) & " of " & pkgCount)        ' You don't want to see "Package 0 of 407", right?
                    Directory.CreateDirectory(".\tempinfo")
                    pkgPossibleMsuFile = Path.GetFileName(pkgs(x)).ToString()
                    If pkgPossibleMsuFile.EndsWith(".msu") Then
                        LogView.AppendText(CrLf & "WARNING: the package currently about to be processed is a MSU file. Proceeding with special addition mode...")
                        AddFromMsu(pkgs(x))
                        Continue For
                    End If
                    File.WriteAllText(".\bin\exthelpers\pkginfo.bat", _
                                      "@echo off" & CrLf & _
                                      "dism /English /image=" & MountDir & " /get-packageinfo /packagepath=" & pkgs(x) & " | findstr /c:" & Quote & "Package Identity" & Quote & " > .\tempinfo\pkgname" & CrLf & _
                                      "dism /English /image=" & MountDir & " /get-packageinfo /packagepath=" & pkgs(x) & " | findstr /c:" & Quote & "Description" & Quote & " > .\tempinfo\pkgdesc" & CrLf & _
                                      "dism /English /image=" & MountDir & " /get-packageinfo /packagepath=" & pkgs(x) & " | findstr /c:" & Quote & "Applicable" & Quote & " > .\tempinfo\pkgapplicability" & CrLf & _
                                      "dism /English /image=" & MountDir & " /get-packageinfo /packagepath=" & pkgs(x) & " | findstr /c:" & Quote & "State" & Quote & " > .\tempinfo\pkgstate", _
                                      ASCII)
                    If IsDebugged Then
                        Process.Start("\Windows\system32\notepad.exe", ".\bin\exthelpers\pkginfo.bat").WaitForExit()
                    End If
                    Process.Start(".\bin\exthelpers\pkginfo.bat").WaitForExit()
                    pkgName = My.Computer.FileSystem.ReadAllText(".\tempinfo\pkgname", ASCII).Replace("Package Identity : ", "").Trim()
                    pkgDesc = My.Computer.FileSystem.ReadAllText(".\tempinfo\pkgdesc", ASCII).Replace("Description : ", "").Trim()
                    pkgApplicabilityStatus = My.Computer.FileSystem.ReadAllText(".\tempinfo\pkgapplicability", ASCII).Replace("Applicable : ", "").Trim()
                    pkgInstallationState = My.Computer.FileSystem.ReadAllText(".\tempinfo\pkgstate", ASCII).Replace("State : ", "").Trim()
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
                        Directory.Delete(".\tempinfo", True)
                    Catch ex As Exception

                    End Try

                    ' Preparing to add pkg
                    If pkgIsApplicable Then
                        ' Determine whether package is already added (either Install Pending or Installed)
                        If pkgIsAlreadyAdded Then
                            LogView.AppendText(CrLf & "Package is already added. Skipping installation of this package...")
                            pkgFailedAdditions += 1
                            'CurrentPB.Value = x + 1
                            Continue For
                        Else
                            LogView.AppendText(CrLf & "Processing package...")
                            DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
                            CommandArgs = "/English /image=" & MountDir & " /add-package /packagepath=" & pkgs(x)
                            If pkgIgnoreApplicabilityChecks Then
                                CommandArgs = CommandArgs & " /ignorecheck"
                            End If
                            If pkgPreventIfPendingOnline Then
                                CommandArgs = CommandArgs & " /preventpending"
                            End If
                            DISMProc.StartInfo.Arguments = CommandArgs
                            DISMProc.Start()
                            Do Until DISMProc.HasExited
                                If DISMProc.HasExited Then
                                    Exit Do
                                End If
                            Loop
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
                            CommandArgs = "/English /image=" & MountDir & " /add-package /packagepath=" & pkgs(x) & " /ignorecheck"
                            If pkgPreventIfPendingOnline Then
                                CommandArgs = CommandArgs & " /preventpending"
                            End If
                            DISMProc.StartInfo.Arguments = CommandArgs
                            DISMProc.Start()
                            Do Until DISMProc.HasExited
                                If DISMProc.HasExited Then
                                    Exit Do
                                End If
                            Loop
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
                                PkgErrorText.RichTextBox1.AppendText("-2146498525")
                            Else
                                PkgErrorText.RichTextBox1.AppendText(CrLf & "-2146498525")
                            End If
                            pkgFailedAdditions += 1
                            'CurrentPB.Value = x + 1
                            Continue For
                        End If
                    End If
                    'CurrentPB.Value = x + 1
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
                taskCountLbl.Text = "Tasks: " & currentTCont & "/" & taskCount
                RunOps(8)
            Else
                AllPB.Value = 100
            End If
            If pkgAdditionOp = 0 Then
                GetErrorCode(False)
            ElseIf pkgAdditionOp = 1 Then
                GetErrorCode(True)
            End If

        ElseIf opNum = 87 Then
            allTasks.Text = "Setting the uninstall window..."
            currentTask.Text = "Setting the amount of days an uninstall can happen..."
            LogView.AppendText(CrLf & "Setting the amount of days an uninstall can happen..." & CrLf & _
                               "Number of days: " & osUninstDayCount)
            DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
            CommandArgs = "/English /online /set-osuninstallwindow /value:" & osUninstDayCount
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            Do Until DISMProc.HasExited
                If DISMProc.HasExited Then
                    Exit Do
                End If
            Loop
            currentTask.Text = "Gathering error level..."
            LogView.AppendText(CrLf & "Gathering error level...")
            GetErrorCode(False)
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        ElseIf opNum = 991 Then
            ' Convert image formats. Right now, only index 1 will be converted (exported) to a WIM/ESD file
            allTasks.Text = "Converting image..."
            currentTask.Text = "Converting specified image..."
            LogView.AppendText(CrLf & "Converting image..." & CrLf & _
                               "Options:" & CrLf)

            ' Gather options
            LogView.AppendText("- Source image file: " & imgSrcFile & CrLf & _
                               "- Destination image file: " & imgDestFile & CrLf)
            If imgConversionMode = 0 Then
                LogView.AppendText("- Image conversion mode: Windows Imaging (WIM) --> Electronic Software Distribution (ESD)")
            ElseIf imgConversionMode = 1 Then
                LogView.AppendText("- Image conversion mode: Electronic Software Distribution (ESD) --> Windows Imaging (WIM)")
            End If

            ' Run commands
            DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
            CommandArgs = "/English /export-image /sourceimagefile=" & imgSrcFile & " /sourceindex=1 /destinationimagefile=" & imgDestFile
            If imgConversionMode = 0 Then
                CommandArgs = CommandArgs & " /compress:recovery"
            ElseIf imgConversionMode = 1 Then
                CommandArgs = CommandArgs & " /compress:max"
            End If
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            Do Until DISMProc.HasExited
                If DISMProc.HasExited Then
                    Exit Do
                End If
            Loop
            currentTask.Text = "Gathering error level..."
            LogView.AppendText(CrLf & "Gathering error level...")
            GetErrorCode(False)
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
        ElseIf opNum = 992 Then
            allTasks.Text = "Merging SWM files..."
            currentTask.Text = "Merging SWM files into a WIM file..."
            LogView.AppendText(CrLf & "Merging SWM files into a WIM file..." & CrLf & _
                               "Options:" & CrLf)
            ' Gather options
            LogView.AppendText("- Source image file: " & imgSwmSource & CrLf & _
                               "- Destination image file: " & imgWimDestination & CrLf)

            ' Run commands
            DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
            CommandArgs = "/English /export-image /sourceimagefile=" & imgSwmSource & " /swmfile=" & Path.GetDirectoryName(imgSwmSource) & "\" & Path.GetFileNameWithoutExtension(imgSwmSource) & "*.swm /sourceindex=1 /destinationimagefile=" & imgWimDestination & " /compress=max /checkintegrity"
            DISMProc.StartInfo.Arguments = CommandArgs
            DISMProc.Start()
            Do Until DISMProc.HasExited
                If DISMProc.HasExited Then
                    Exit Do
                End If
            Loop
            currentTask.Text = "Gathering error level..."
            LogView.AppendText(CrLf & "Gathering error level...")
            GetErrorCode(False)
            LogView.AppendText(CrLf & CrLf & "    Error level : " & errCode)
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
        File.WriteAllText(".\bin\exthelpers\temp.bat", _
                          "@echo off" & CrLf & _
                          ".\bin\utils\7z x " & MsuFile & " -o" & pkgSource & "\MsuExtract\" & MsuName, ASCII)
        Process.Start(".\bin\exthelpers\temp.bat").WaitForExit()
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
        CabFile = CabFile & ".cab"
        pkgToAdd = pkgSource & "\MsuExtract\" & MsuName & "\" & CabFile
        ' Add MSU file
        File.WriteAllText(".\bin\exthelpers\pkginfo.bat", _
                          "@echo off" & CrLf & _
                          "dism /English /image=" & MountDir & " /get-packageinfo /packagepath=" & pkgToAdd & " | findstr /c:" & Quote & "Package Identity" & Quote & " > .\tempinfo\pkgname" & CrLf & _
                          "dism /English /image=" & MountDir & " /get-packageinfo /packagepath=" & pkgToAdd & " | findstr /c:" & Quote & "Description" & Quote & " > .\tempinfo\pkgdesc" & CrLf & _
                          "dism /English /image=" & MountDir & " /get-packageinfo /packagepath=" & pkgToAdd & " | findstr /c:" & Quote & "Applicable" & Quote & " > .\tempinfo\pkgapplicability" & CrLf & _
                          "dism /English /image=" & MountDir & " /get-packageinfo /packagepath=" & pkgToAdd & " | findstr /c:" & Quote & "State" & Quote & " > .\tempinfo\pkgstate", _
                          ASCII)
        If IsDebugged Then
            Process.Start("\Windows\system32\notepad.exe", ".\bin\exthelpers\pkginfo.bat").WaitForExit()
        End If
        Process.Start(".\bin\exthelpers\pkginfo.bat").WaitForExit()
        pkgName = My.Computer.FileSystem.ReadAllText(".\tempinfo\pkgname", ASCII).Replace("Package Identity : ", "").Trim()
        pkgDesc = My.Computer.FileSystem.ReadAllText(".\tempinfo\pkgdesc", ASCII).Replace("Description : ", "").Trim()
        pkgApplicabilityStatus = My.Computer.FileSystem.ReadAllText(".\tempinfo\pkgapplicability", ASCII).Replace("Applicable : ", "").Trim()
        pkgInstallationState = My.Computer.FileSystem.ReadAllText(".\tempinfo\pkgstate", ASCII).Replace("State : ", "").Trim()
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
            Directory.Delete(".\tempinfo", True)
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
                CommandArgs = "/English /image=" & MountDir & " /add-package /packagepath=" & pkgToAdd
                If pkgIgnoreApplicabilityChecks Then
                    CommandArgs = CommandArgs & " /ignorecheck"
                End If
                If pkgPreventIfPendingOnline Then
                    CommandArgs = CommandArgs & " /preventpending"
                End If
                DISMProc.StartInfo.Arguments = CommandArgs
                DISMProc.Start()
                Do Until DISMProc.HasExited
                    If DISMProc.HasExited Then
                        Exit Do
                    End If
                Loop
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
                CommandArgs = "/English /image=" & MountDir & " /add-package /packagepath=" & pkgToAdd & " /ignorecheck"
                If pkgPreventIfPendingOnline Then
                    CommandArgs = CommandArgs & " /preventpending"
                End If
                DISMProc.StartInfo.Arguments = CommandArgs
                DISMProc.Start()
                Do Until DISMProc.HasExited
                    If DISMProc.HasExited Then
                        Exit Do
                    End If
                Loop
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
                    PkgErrorText.RichTextBox1.AppendText("-2146498525")
                Else
                    PkgErrorText.RichTextBox1.AppendText(CrLf & "-2146498525")
                End If
                pkgFailedAdditions += 1
                'CurrentPB.Value = x + 1
                Exit Sub
            End If
        End If
    End Sub

    Sub GetPkgErrorLevel()
        errCode = Decimal.ToInt32(DISMProc.ExitCode)
        Select Case errCode
            Case 0
                pkgSuccessfulAdditions += 1
            Case 1
                pkgFailedAdditions += 1
        End Select
    End Sub

    Private Sub ProgressBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ProgressBW.DoWork
        RunOps(OperationNum)
    End Sub

    Private Sub ProgressBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ProgressBW.RunWorkerCompleted
        If IsSuccessful Then
            'Visible = False
            Try
                CurrentPB.Value = 100
            Catch ex As Exception
                ' Continue
            End Try
            AllPB.Value = AllPB.Maximum
            Refresh()
            MainForm.isModified = True
            Thread.Sleep(2000)
            If OperationNum = 0 Then
                MainForm.LoadDTProj(projPath & projName & "\" & projName & ".dtproj", projName, True)
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
            ElseIf OperationNum = 15 Then
                MainForm.SourceImg = SourceImg
                MainForm.ImgIndex = ImgIndex
                MainForm.MountDir = MountDir
                If isReadOnly Then
                    MainForm.UpdateProjProperties(True, True)
                Else
                    MainForm.UpdateProjProperties(True, False)
                End If
                ' This is a crucial change, so save things immediately
                MainForm.SaveDTProj()
            ElseIf OperationNum = 18 Then
                If ProjProperties.Visible Then
                    isTriggeredByPropertyDialog = True
                    ProjProperties.Close()
                End If
                If remountisReadOnly Then
                    MainForm.UpdateProjProperties(True, True)
                Else
                    MainForm.UpdateProjProperties(True, False)
                End If
                If isTriggeredByPropertyDialog Then
                    ProjProperties.TabControl1.SelectedIndex = 1
                    ProjProperties.ShowDialog(MainForm)
                End If
                MainForm.isModified = False
            ElseIf OperationNum = 21 Then
                MainForm.UpdateProjProperties(False, False)
                MainForm.MountDir = "N/A"
                ' This is a crucial change, so save things immediately
                MainForm.SaveDTProj()
                ImgMount.TextBox1.Text = ""     ' The program has a bug where mounting the same image after doing this results in the image file being ""
            ElseIf OperationNum = 26 Then
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
            ElseIf OperationNum = 991 Then
                Visible = False
                ImgConversionSuccessDialog.ShowDialog(MainForm)
                If ImgConversionSuccessDialog.DialogResult = Windows.Forms.DialogResult.OK Then
                    Process.Start("\Windows\explorer.exe", Path.GetDirectoryName(imgDestFile))
                End If
            End If
            MainForm.ToolStripButton4.Visible = False
            Dispose()
            Close()
        Else
            Label1.Text = "Could not perform image operations"
            Label2.Text = "An error has occurred, which stopped the image operations. Please read the log below for more information."
            CurrentPB.Value = CurrentPB.Maximum
            AllPB.Value = AllPB.Maximum
            If Height <> 420 Then
                LogButton.PerformClick()
            End If
            Cancel_Button.Text = "OK"
            LinkLabel1.Visible = True
            ' Add details for error codes
            If errCode = -1052638938 Then
                ' An image that was selected for mounting is already mounted
                LogView.AppendText(CrLf & "The specified image is already mounted. This command works for " & Quote & "orphaned" & Quote & " images")
            ElseIf errCode = -1052638964 Then
                ' The image, with read-only permissions, was attempted to be written
                LogView.AppendText(CrLf & "The program tried to save changes to an image that was mounted as read-only. " & CrLf & _
                                          "To solve this, close this dialog, and click " & Quote & "Tools > Remount image with write permissions" & Quote & CrLf & _
                                          "Do note that, if the image came from an installation media, you may need to copy the source file to perform modifications to it.")
            ElseIf errCode = -1052638953 Then
                ' Some applications (or hidden processes) have open handles on the mount dir
                LogView.AppendText(CrLf & "The program tried to unmount the image, but some applications or processes have opened files or directories of the image." & CrLf & _
                                          "Make sure no application or process is using the directories or files of the image." & CrLf & _
                                          "If the error occurred at the end of the operation (e.g., at 100%), and you were trying to save the changes; they might already be saved, and can be safe to continue discarding changes.")
            ElseIf errCode = -1052638947 Then
                ' A partial unmount or an in-progress mount operation happened
                LogView.AppendText(CrLf & "The mounted image cannot be committed back into the source file." & CrLf & _
                                          "A partial unmount might have happened, or the image was still being mounted." & CrLf & _
                                          "If the image was unmounted whilst saving changes, the commit probably succeeded. Please validate this. If this is the case, proceed with unmounting the image discarding changes.")
            ElseIf errCode = -1051655919 Then
                ' The specified image, that was marked to mount with read-write permissions, came from a read-only source (e.g., a Windows installation disc)
                LogView.AppendText(CrLf & "The source file comes from a read-only source. You cannot mount it with read-write permissions." & CrLf & _
                                          "Please re-specify the image in the mount dialog whilst checking the " & Quote & "Read-only" & Quote & " check box. You can also try copying the source image to a folder with read-write permissions.")
            ElseIf errCode = 87 Then
                ' Internal errors
                LogView.AppendText(CrLf & "There is essential data that was not picked internally by the operation. This may be a bug in the software or a feature may be incomplete.")
            End If
        End If
    End Sub

    Sub GetErrorCode(Bypass As Boolean)
        If Bypass Then
            errCode = 0
        Else
            errCode = Decimal.ToInt32(DISMProc.ExitCode)
        End If
        Select Case errCode
            Case 0
                IsSuccessful = True
            Case Else
                IsSuccessful = False
        End Select
    End Sub

    Private Sub ProgressPanel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Determine program colors
        If MainForm.BackColor = Color.Black Then
            BodyPanel.BackColor = Color.Black
            BodyPanel.ForeColor = Color.White
            GroupBox1.BackColor = Color.Black
            GroupBox1.ForeColor = Color.White
            LogView.BackColor = Color.Black
            LogView.ForeColor = Color.White
            Panel2.BackColor = Color.Black
            Panel2.ForeColor = Color.White
            logActions.BackColor = Color.Black
            logActions.ForeColor = Color.White
            ToolStrip1.BackColor = Color.Black
            ToolStrip1.ForeColor = Color.White
            ToolStrip2.BackColor = Color.Black
            ToolStrip2.ForeColor = Color.White
            ToolStripButton1.Image = New Bitmap(My.Resources.save_glyph_dark)
            ToolStripButton2.Image = New Bitmap(My.Resources.hide_glyph_dark)

        ElseIf MainForm.BackColor = Color.White Then

        End If
        If Debugger.IsAttached Then
            IsDebugged = True
        Else
            IsDebugged = False
        End If
        MainForm.ToolStripButton4.Visible = True
        Control.CheckForIllegalCrossThreadCalls = False
        LinkLabel1.Visible = False
        GetTasks(OperationNum)
        ProgressBW.RunWorkerAsync()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\notepad.exe", Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\Logs\DISM\DISM.log")
    End Sub

    Private Sub BodyPanel_Paint(sender As Object, e As PaintEventArgs) Handles BodyPanel.Paint
        If MainForm.BackColor = Color.Black Then
            ControlPaint.DrawBorder(e.Graphics, BodyPanel.ClientRectangle, Color.White, ButtonBorderStyle.Solid)
        ElseIf MainForm.BackColor = Color.White Then
            ControlPaint.DrawBorder(e.Graphics, BodyPanel.ClientRectangle, Color.Black, ButtonBorderStyle.Solid)
        End If
    End Sub
End Class