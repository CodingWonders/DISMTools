﻿Imports System.Net
Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports Microsoft.Win32
Imports Microsoft.Dism
Imports System.Runtime.InteropServices
Imports System.Xml
Imports System.ServiceModel.Syndication

Public Class MainForm

    Public IsInEditMode As Boolean
    Public imgStatus As Integer
    Public prjName As String
    Public IsImageMounted As Boolean
    Public projPath As String
    Public isReadOnly As Boolean
    Public isOptimized As Boolean
    Public isIntegrityTested As Boolean
    Public isProjectLoaded As Boolean
    Public isModified As Boolean

    ' Image info
    Public SourceImg As String
    Public ImgIndex As Integer
    Public MountDir As String       ' ProjProperties.imgMountDir
    Public imgMountedStatus As String
    Public imgVersion As String
    Public imgMountedName As String
    Public imgMountedDesc As String
    Public imgSize As String
    Public imgWimBootStatus As String
    Public imgArch As String
    Public imgHal As String
    Public imgSPBuild As String
    Public imgSPLvl As String
    Public imgEdition As String
    Public imgPType As String
    Public imgPSuite As String
    Public imgSysRoot As String
    Public imgDirs As Integer
    Public imgFiles As Integer
    Public imgCreation As String
    Public imgModification As String
    Public imgLangs As String
    Public imgFormat As String
    Public imgRW As String

    Public CreationTime As String
    Public ModifyTime As String

    Public imgInstType As String       ' Image installation type, used to determine whether an image contains a Server Core/Nano Server installation

    ' Var used to detect whether the image is orphaned (needs servicing session reload)
    Public isOrphaned As Boolean    ' This variable is true when the host system is shut down or restarted (the servicing session stops abruptly)
    Public mountedImgStatus As String

    Public imgIndexCount As Integer

    ' Program settings
    ' This is the initial batch of settings for this version (0.1). As the program continues development,
    ' more settings will be added below this initial batch
    Public DismExe As String
    Public SaveOnSettingsIni As Boolean
    Public VolatileMode As Boolean
    Public ColorMode As Integer
    Public Language As Integer
    Public LogFont As String
    Public LogFile As String
    Public LogLevel As Integer = 3
    Public ImgOperationMode As Integer
    Public QuietOperations As Boolean
    Public SysNoRestart As Boolean
    Public UseScratch As Boolean
    Public ScratchDir As String
    Public EnglishOutput As Boolean
    Public ReportView As Integer
    ' 0.1.1 settings
    Public LogFontIsBold As Boolean
    Public LogFontSize As Integer
    ' 0.2 settings
    Public expBackgroundProcesses As Boolean = True     ' Experimental setting used during development. Everything now depends on it. This WILL be removed in the future
    Public NotificationShow As Boolean
    Public NotificationFrequency As Integer
    Public NotificationTimes As Integer = 0
    ' 0.3 settings
    ' - Background processes -
    Public ExtAppxGetter As Boolean = True
    Public SkipNonRemovable As Boolean = True
    Public AllDrivers As Boolean
    Public SkipFrameworks As Boolean = True
    Public RunAllProcs As Boolean
    ' - Startup -
    Public StartupRemount As Boolean
    Public StartupUpdateCheck As Boolean
    ' - Secondary progress panel -
    Public ProgressPanelStyle As Integer = 1        ' 0 (Legacy, 0.1 - 0.2.2), 1 (Modern, >= 0.3)
    ' - Progress panel -
    Public AutoLogs As Boolean
    Public AutoScrDir As Boolean
    ' - Appearance -
    Public AllCaps As Boolean

    ' Background process initiator settings
    Public bwBackgroundProcessAction As Integer
    Public bwAllBackgroundProcesses As Boolean
    Public bwGetImageInfo As Boolean
    Public bwGetAdvImgInfo As Boolean

    ' Variable used for online installation management
    Public OnlineManagement As Boolean
    ' Variable used for offline installation management
    Public OfflineManagement As Boolean

    ' These are the variables that need to change when testing setting validity
    Public isExeProblematic As Boolean
    Public isLogFontProblematic As Boolean
    Public isLogFileProblematic As Boolean
    Public isScratchDirProblematic As Boolean
    Public ProblematicStrings(4) As String      ' 0 (DismExe), 1 (LogFont), 2 (LogFile), 3 (ScratchDir)

    ' Detect whether project is a SQL Server project or a DISMTools project
    Public isSqlServerDTProj As Boolean

    ' Set branch name and codenames
    Public dtBranch As String = "dt_preview"

    ' Arrays and other variables used on background processes
    Public imgPackageNames(65535) As String
    Public imgPackageState(65535) As String
    Public imgPackageRelType(65535) As String
    Public imgPackageInstTime(65535) As String

    Public imgFeatureNames(65535) As String
    Public imgFeatureState(65535) As String

    Public imgAppxDisplayNames(65535) As String
    Public imgAppxPackageNames(65535) As String
    Public imgAppxVersions(65535) As String
    Public imgAppxArchitectures(65535) As String
    Public imgAppxResourceIds(65535) As String
    Public imgAppxRegions(65535) As String

    Public imgCapabilityIds(65535) As String
    Public imgCapabilityState(65535) As String

    Public imgDrvPublishedNames(65535) As String
    Public imgDrvOGFileNames(65535) As String
    Public imgDrvInbox(65535) As String
    Public imgDrvClassNames(65535) As String
    Public imgDrvProviderNames(65535) As String
    Public imgDrvDates(65535) As String
    Public imgDrvVersions(65535) As String
    Public imgDrvBootCriticalStatus(65535) As Boolean

    Public imgPackageNameLastEntry As String

    Public areBackgroundProcessesDone As Boolean

    Dim pbOpNums As Integer
    Dim progressMax As Integer = 100
    Dim progressMin As Integer = 0
    Dim progressDivs As Double
    Dim progressLabel As String
    Dim regJumps As Boolean
    Dim irregVal As Integer = 0

    Dim ElementCount As Integer = 0

    Public pinState As Integer

    Public MountedImageImgFiles(65535) As String
    Public MountedImageMountDirs(65535) As String
    Public MountedImageImgIndexes(65535) As String
    Public MountedImageMountedReWr(65535) As String
    Public MountedImageImgStatuses(65535) As String
    ' New variables for 0.3
    Public MountedImageImgVersions(65535) As String
    ' Private lists for DetectMountedImages function
    Dim MountedImageImgFileList As New List(Of String)
    Dim MountedImageImgIndexList As New List(Of String)
    Dim MountedImageMountDirList As New List(Of String)
    Dim MountedImageImgStatusList As New List(Of String)
    Dim MountedImageReWrList As New List(Of String)
    Dim MountedImageImgVersionList As New List(Of String)

    ' Perform image unmount operations when pressing on buttons
    Public imgCommitOperation As Integer = -1 ' 0: commit; 1: discard

    Dim DismVersionChecker As FileVersionInfo
    Dim argProjPath As String = ""                                       ' String used to know which project to load if it's specified in an argument
    Dim argOnline As Boolean                                             ' Determine if program will be launched in online installation mode

    Dim sessionMntDir As String = ""

    ' ADK copier variables
    Dim adkCopyArg As Integer
    Dim currentArch As String
    Dim archIntg As Integer
    Dim fileCount As Integer
    Dim CurrentFileInt As Integer

    ' Window parameters
    Dim WndWidth As Integer
    Dim WndHeight As Integer
    Dim WndLeft As Integer
    Dim WndTop As Integer

    Public CompletedTasks(4) As Boolean
    Public PendingTasks(4) As Boolean

    Dim HasRemounted As Boolean

    Dim IsCompatible As Boolean = True

    Dim SysVer As Version

    ' Lists for information dialogs
    Dim PackageInfoList As DismPackageCollection
    Dim FeatureInfoList As DismFeatureCollection
    Dim AppxPackageInfoList As DismAppxPackageCollection
    Dim CapabilityInfoList As DismCapabilityCollection
    Dim DriverInfoList As DismDriverPackageCollection

    Public imgVersionInfo As Version = Nothing

    Dim NoMigration As Boolean                                           ' Set this variable to true ONLY if the IDE started the program

    Public drivePath As String = ""

    Public EnableExperiments As Boolean

    Public SkipQuestions As Boolean             ' Skips questions in the info saver
    Public AutoCompleteInfo(4) As Boolean       ' Skips questions for specific info categories

    Public GoToNewView As Boolean

    Dim FeedLinks As New List(Of Uri)

    Friend NotInheritable Class NativeMethods

        Private Sub New()
        End Sub

        <DllImport("dwmapi.dll")>
        Shared Function DwmSetWindowAttribute(hwnd As IntPtr, attr As Integer, ByRef attrValue As Integer, attrSize As Integer) As Integer
        End Function

    End Class

    Const DWMWA_USE_IMMERSIVE_DARK_MODE As Integer = 20
    Const WS_EX_COMPOSITED As Integer = &H2000000
    Const GWL_EXSTYLE As Integer = -20

    Shared Sub EnableDarkTitleBar(hwnd As IntPtr, isDarkMode As Boolean)
        Dim attribute As Integer = If(isDarkMode, 1, 0)
        Dim result As Integer = NativeMethods.DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, attribute, 4)
    End Sub

    Function GetWindowHandle(ctrl As Control) As IntPtr
        Return ctrl.Handle
    End Function

    Function IsWindowsVersionOrGreater(majorVersion As Integer, minorVersion As Integer, buildNumber As Integer) As Boolean
        Dim version = Environment.OSVersion.Version
        Return version.Major > majorVersion OrElse (version.Major = majorVersion AndAlso version.Minor > minorVersion) OrElse (version.Major = majorVersion AndAlso version.Minor = minorVersion AndAlso version.Build >= buildNumber)
    End Function

    Sub GetArguments()
        Dim args() As String = Environment.GetCommandLineArgs()
        If args.Length = 1 Then
            Exit Sub
        Else
            For Each arg In args
                If arg.StartsWith("/setup", StringComparison.OrdinalIgnoreCase) Then
                    SplashScreen.Hide()
                    PrgSetup.ShowDialog()
                ElseIf arg.StartsWith("/load", StringComparison.OrdinalIgnoreCase) Then
                    If File.Exists(arg.Replace("/load=", "").Trim()) And Directory.Exists(Path.GetDirectoryName(arg.Replace("/load=", "").Trim())) Then
                        argProjPath = arg.Replace("/load=", "").Trim()
                    Else
                        Select Case Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        MsgBox("An invalid project has been specified", vbOKOnly + vbCritical, Text)
                                    Case "ESN"
                                        MsgBox("Se ha especificado un proyecto no válido", vbOKOnly + vbCritical, Text)
                                    Case "FRA"
                                        MsgBox("Un projet non valide a été spécifié", vbOKOnly + vbCritical, Text)
                                End Select
                            Case 1
                                MsgBox("An invalid project has been specified", vbOKOnly + vbCritical, Text)
                            Case 2
                                MsgBox("Se ha especificado un proyecto no válido", vbOKOnly + vbCritical, Text)
                            Case 3
                                MsgBox("Un projet non valide a été spécifié", vbOKOnly + vbCritical, Text)
                        End Select
                    End If
                ElseIf arg.StartsWith("/online", StringComparison.OrdinalIgnoreCase) Then
                    If argProjPath = "" Then
                        argOnline = True
                    Else
                        ' Add warning later
                    End If
                ElseIf arg.StartsWith("/migrate", StringComparison.OrdinalIgnoreCase) Then
                    MigrationForm.ShowDialog()
                    Thread.Sleep(1500)
                ElseIf arg.StartsWith("/nomig", StringComparison.OrdinalIgnoreCase) Then
                    NoMigration = True
                ElseIf arg.StartsWith("/exp", StringComparison.OrdinalIgnoreCase) Then
                    EnableExperiments = True
                End If
            Next
        End If
    End Sub
    
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Because of the DISM API, Windows 7 compatibility is out the window (no pun intended)
        If Environment.OSVersion.Version.Major = 6 And Environment.OSVersion.Version.Minor < 2 Then
            MsgBox("This program is incompatible with Windows 7 and Server 2008 R2." & CrLf & "This program uses the DISM API, which requires files from the Assessment and Deployment Kit (ADK). However, support for Windows 7 is not included." & CrLf & CrLf & "The program will be closed.", vbOKOnly + vbCritical, "DISMTools")
            Environment.Exit(1)
        End If
        If Not Directory.Exists(Application.StartupPath & "\logs") Then Directory.CreateDirectory(Application.StartupPath & "\logs")
        If Not Debugger.IsAttached Then SplashScreen.Show()
        Thread.Sleep(2000)
        ' I once tested this on a computer which didn't require me to ask for admin privileges. This is a requirement of DISM. Check this
        If Not My.User.IsInRole(ApplicationServices.BuiltInRole.Administrator) Then
            MsgBox("This program must be run as an administrator." & CrLf & "There are certain software configurations in which Windows will run this program without admin privileges, so you must ask for them manually." & CrLf & CrLf & "Right-click the executable, and select " & Quote & "Run as administrator" & Quote, vbOKOnly + vbCritical, "DISMTools")
            Environment.Exit(1)
        End If
        Visible = False
        Debug.WriteLine("DISMTools, version " & My.Application.Info.Version.ToString() & " (" & dtBranch & ")" & CrLf & _
                        "Loading program settings..." & CrLf)
        GetArguments()
        ' Detect mounted images
        DetectMountedImages(True)
        Debug.WriteLine(CrLf & "Finished detecting mounted images. Continuing program startup..." & CrLf)
        Control.CheckForIllegalCrossThreadCalls = False
        BranchTSMI.Text = dtBranch
        If Debugger.IsAttached Then
            BranchTSMI.Visible = True
            Text &= " (debug mode)"
        End If
        ' Read settings file
        If File.Exists(Application.StartupPath & "\settings.ini") Then
            PerformSettingFileValidation()
            Dim SettingReader As New RichTextBox() With {.Text = File.ReadAllText(Application.StartupPath & "\settings.ini", UTF8)}
            If SettingReader.Text.Contains("SaveOnSettingsIni=1") Then
                LoadDTSettings(1)
            ElseIf SettingReader.Text.Contains("SaveOnSettingsIni=0") Then
                LoadDTSettings(0)
            End If
        Else
            'GenerateDTSettings()
            'LoadDTSettings(1)
            SplashScreen.Hide()
            PrgSetup.ShowDialog()
            LoadDTSettings(1)
        End If
        imgStatus = 0
        ChangeImgStatus()
        If DismExe <> "" Then
            DismVersionChecker = FileVersionInfo.GetVersionInfo(DismExe)
        End If
        UnblockPSHelpers()
        If StartupRemount Then RemountOrphanedImages() Else HasRemounted = True
        While Not HasRemounted
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        If StartupUpdateCheck Then
            UpdCheckerBW.RunWorkerAsync()
        Else
            UpdatePanel.Visible = False
        End If
        MountedImageDetectorBW.RunWorkerAsync()
        If dtBranch.Contains("preview") And Not Debugger.IsAttached Then
            VersionTSMI.Visible = True
        Else
            VersionTSMI.Visible = False
        End If
        If Not Debugger.IsAttached Then SplashScreen.Close()
        WndWidth = Width
        WndHeight = Height
        WndLeft = Left
        WndTop = Top
        If Left < 0 And Top < 0 Then
            ' Center form
            Location = New Point((Screen.FromControl(Me).WorkingArea.Width - Width) / 2, (Screen.FromControl(Me).WorkingArea.Height - Height) / 2)
        End If
        Visible = True
        If argProjPath <> "" Then
            HomePanel.Visible = False
            'Visible = True
            ProgressPanel.OperationNum = 990
            LoadDTProj(argProjPath, Path.GetFileNameWithoutExtension(argProjPath), True, False)
        End If
        If argOnline Then
            BeginOnlineManagement(True)
        End If
        Timer1.Enabled = True
        LinkLabel12.LinkColor = Color.FromArgb(241, 241, 241)
        LinkLabel13.LinkColor = Color.FromArgb(153, 153, 153)
        Button17.Visible = EnableExperiments
        If EnableExperiments Then GetFeedNews()
    End Sub

    ''' <summary>
    ''' Detects all mounted images and their state. Calls the DISM API at program startup
    ''' </summary>
    ''' <param name="DebugLog">Check if the program should output debug information. This is always called at program startup, but never after</param>
    ''' <remarks>This yields results for the MountedImageImgFiles, MountedImageMountDirs, MountedImageImgIndexes, MountedImageMountedRewr and MountedImageImgStatuses string arrays in code</remarks>
    Sub DetectMountedImages(DebugLog As Boolean)
        If DebugLog Then Debug.WriteLine("[DetectMountedImages] Running function...")
        If MountedImageImgFileList IsNot Nothing And MountedImageImgIndexList IsNot Nothing And MountedImageMountDirList IsNot Nothing And MountedImageImgStatuses IsNot Nothing And MountedImageMountedReWr IsNot Nothing Then
            MountedImageImgFileList.Clear()
            MountedImageImgIndexList.Clear()
            MountedImageMountDirList.Clear()
            MountedImageImgStatusList.Clear()
            MountedImageReWrList.Clear()
            MountedImageImgVersionList.Clear()
        End If
        DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
        Dim MountedImgs As DismMountedImageInfoCollection = DismApi.GetMountedImages()
        For Each imageInfo As DismMountedImageInfo In MountedImgs
            If DebugLog Then Debug.WriteLine("- Image file : " & imageInfo.ImageFilePath)
            If DebugLog Then Debug.WriteLine("- Image index : " & imageInfo.ImageIndex)
            If DebugLog Then Debug.WriteLine("- Mount directory : " & imageInfo.MountPath)
            If DebugLog Then Debug.WriteLine("- Mount status : " & imageInfo.MountStatus & If(imageInfo.MountStatus = DismMountStatus.Ok, " (OK)", If(imageInfo.MountStatus = DismMountStatus.NeedsRemount, " (Orphaned)", " (Invalid)")))
            If DebugLog Then Debug.WriteLine("- Mount mode : " & imageInfo.MountMode & If(imageInfo.MountMode = DismMountMode.ReadWrite, " (Write permissions enabled)", "(Write permissions disabled)"))
            If DebugLog Then Debug.WriteLineIf(MountedImgs.Count > 1, "---------------------------------------------------")
            MountedImageImgFileList.Add(imageInfo.ImageFilePath)
            MountedImageImgIndexList.Add(imageInfo.ImageIndex)
            MountedImageMountDirList.Add(imageInfo.MountPath)
            MountedImageImgStatusList.Add(imageInfo.MountStatus)
            MountedImageReWrList.Add(imageInfo.MountMode)
        Next
        MountedImageImgFiles = MountedImageImgFileList.ToArray()
        MountedImageImgIndexes = MountedImageImgIndexList.ToArray()
        MountedImageMountDirs = MountedImageMountDirList.ToArray()
        MountedImageImgStatuses = MountedImageImgStatusList.ToArray()
        MountedImageMountedReWr = MountedImageReWrList.ToArray()
        If MountedImageImgFileList.Count > 0 Then
            For x = 0 To Array.LastIndexOf(MountedImageImgFiles, MountedImageImgFiles.Last)
                Try
                    Dim infoCollection As DismImageInfoCollection = DismApi.GetImageInfo(MountedImageImgFiles(x))
                    For Each imageInfo As DismImageInfo In infoCollection
                        If imageInfo.ImageIndex = MountedImageImgIndexes(x) Then
                            MountedImageImgVersionList.Add(imageInfo.ProductVersion.ToString())
                        End If
                    Next
                Catch ex As Exception
                    If DebugLog Then Debug.WriteLine("[DetectMountedImages] Exception: " & ex.Message & " has occurred when detecting the image version. Proceeding with detecting image version with ntoskrnl...")
                    MountedImageImgVersionList.Add(FileVersionInfo.GetVersionInfo(MountedImageMountDirs(x) & "\Windows\system32\ntoskrnl.exe").ProductVersion)
                End Try
            Next
        End If
        DismApi.Shutdown()
        MountedImageImgVersions = MountedImageImgVersionList.ToArray()
    End Sub

    ''' <summary>
    ''' Remounts orphaned images (images in need of a servicing session reload)
    ''' </summary>
    ''' <remarks></remarks>
    Sub RemountOrphanedImages()
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        If MountedImageMountDirs.Count > 0 Then
            Try
                DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
                For x = 0 To Array.LastIndexOf(MountedImageMountDirs, MountedImageMountDirs.Last)
                    If MountedImageImgStatuses(x) = 1 Then
                        DismApi.RemountImage(MountedImageMountDirs(x))
                    End If
                Next
            Catch ex As Exception
                Debug.WriteLine("Could not remount all orphaned images. Reason:" & CrLf & ex.ToString())
            Finally
                DismApi.Shutdown()
            End Try
        End If
        HasRemounted = True
    End Sub

    Sub CheckForUpdates(branch As String)
        UpdateLink.LinkArea = New LinkArea(0, 0)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        UpdateLink.Text = "Checking for updates..."
                    Case "ESN"
                        UpdateLink.Text = "Comprobando actualizaciones..."
                    Case "FRA"
                        UpdateLink.Text = "Vérification des mises à jour en cours..."
                End Select
            Case 1
                UpdateLink.Text = "Checking for updates..."
            Case 2
                UpdateLink.Text = "Comprobando actualizaciones..."
            Case 3
                UpdateLink.Text = "Vérification des mises à jour en cours..."
        End Select
        Dim latestVer As String = ""
        Using client As New WebClient()
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            Try
                client.DownloadFile("https://raw.githubusercontent.com/CodingWonders/DISMTools/" & branch & "/Updater/DISMTools-UCS/update-bin/" & If(branch.Contains("preview"), "preview.ini", "stable.ini"), Application.StartupPath & "\info.ini")
            Catch ex As WebException
                Debug.WriteLine("We couldn't fetch the necessary update information. Reason:" & CrLf & ex.Status.ToString())
                UpdatePanel.Visible = False
                Exit Sub
            End Try
            Debug.WriteLine("Reading update information...")
            If File.Exists(Application.StartupPath & "\info.ini") Then
                Dim infoRTB As New RichTextBox With {
                    .Text = File.ReadAllText(Application.StartupPath & "\info.ini")
                }
                For Each Line In infoRTB.Lines
                    If Line.StartsWith("LatestVer") Then
                        latestVer = Line.Replace("LatestVer = ", "").Trim()
                    End If
                Next
                File.Delete(Application.StartupPath & "\info.ini")
                Debug.WriteLine("Comparing versions...")
                Dim fv As String = My.Application.Info.Version.ToString()
                If fv = latestVer Then
                    Debug.WriteLine("There aren't any updates available")
                    UpdatePanel.Visible = False
                Else
                    Debug.WriteLine("There are updates available. Showing update recommendation...")
                    Select Case Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    UpdateLink.Text = "A new version is available for download and installation. Click here to learn more"
                                    UpdateLink.LinkArea = New LinkArea(58, 24)
                                Case "ESN"
                                    UpdateLink.Text = "Hay una nueva versión disponible para su descarga e instalación. Haga clic aquí para saber más"
                                    UpdateLink.LinkArea = New LinkArea(65, 29)
                                Case "FRA"
                                    UpdateLink.Text = "Une nouvelle version est disponible pour le téléchargement et l'installation. Cliquez ici pour en savoir plus"
                                    UpdateLink.LinkArea = New LinkArea(78, 31)
                            End Select
                        Case 1
                            UpdateLink.Text = "A new version is available for download and installation. Click here to learn more"
                            UpdateLink.LinkArea = New LinkArea(58, 24)
                        Case 2
                            UpdateLink.Text = "Hay una nueva versión disponible para su descarga e instalación. Haga clic aquí para saber más"
                            UpdateLink.LinkArea = New LinkArea(65, 29)
                        Case 3
                            UpdateLink.Text = "Une nouvelle version est disponible pour le téléchargement et l'installation. Cliquez ici pour en savoir plus"
                            UpdateLink.LinkArea = New LinkArea(78, 31)
                    End Select
                    UpdatePanel.Visible = True
                End If
            End If
        End Using
    End Sub

    Sub UnblockPSHelpers()
        Dim PSUnblocker As New Process()
        PSUnblocker.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\System32\WindowsPowerShell\v1.0\powershell.exe"
        PSUnblocker.StartInfo.Arguments = "-executionpolicy unrestricted -command Unblock-File " & Quote & Application.StartupPath & "\bin\extps1\*.*" & Quote
        PSUnblocker.StartInfo.CreateNoWindow = True
        PSUnblocker.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        PSUnblocker.Start()
        PSUnblocker.WaitForExit()
    End Sub

    Sub ChangeImgStatus()
        If imgStatus = 0 Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label5.Text = "No"
                        Case "ESN"
                            Label5.Text = "No"
                        Case "FRA"
                            Label5.Text = "Non"
                    End Select
                Case 1
                    Label5.Text = "No"
                Case 2
                    Label5.Text = "No"
                Case 3
                    Label5.Text = "Non"
            End Select
            LinkLabel1.Visible = True
            LinkLabel14.Visible = True
        Else
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label5.Text = "Yes"
                        Case "ESN"
                            Label5.Text = "Sí"
                        Case "FRA"
                            Label5.Text = "Oui"
                    End Select
                Case 1
                    Label5.Text = "Yes"
                Case 2
                    Label5.Text = "Sí"
                Case 3
                    Label5.Text = "Oui"
            End Select
            LinkLabel1.Visible = False
            LinkLabel14.Visible = False
        End If
        Label50.Text = Label5.Text
    End Sub

    ''' <summary>
    ''' Set colors on any surface with the "Professional" RenderMode in dark mode
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DarkModeColorTable
        Inherits ProfessionalColorTable

        Public Overrides ReadOnly Property ToolStripBorder As Color
            Get
                Return Color.FromArgb(32, 32, 32)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
            Get
                Return Color.FromArgb(32, 32, 32)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientBegin As Color
            Get
                Return Color.FromArgb(32, 32, 32)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientMiddle As Color
            Get
                Return Color.FromArgb(32, 32, 32)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientEnd As Color
            Get
                Return Color.FromArgb(32, 32, 32)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelected As Color
            Get
                Return Color.FromArgb(39, 39, 39)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemBorder As Color
            Get
                Return Color.FromArgb(39, 39, 39)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuBorder As Color
            Get
                Return Color.FromArgb(39, 39, 39)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientBegin As Color
            Get
                Return Color.FromArgb(62, 62, 64)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientEnd As Color
            Get
                Return Color.FromArgb(62, 62, 64)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientBegin As Color
            Get
                Return Color.FromArgb(27, 27, 28)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientEnd As Color
            Get
                Return Color.FromArgb(27, 27, 28)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientMiddle As Color
            Get
                Return Color.FromArgb(27, 27, 28)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientBegin As Color
            Get
                Return Color.FromArgb(27, 27, 28)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientEnd As Color
            Get
                Return Color.FromArgb(27, 27, 28)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
            Get
                Return Color.FromArgb(27, 27, 28)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
            Get
                Return Color.FromArgb(27, 27, 28)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
            Get
                Return Color.FromArgb(27, 27, 28)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripPanelGradientBegin As Color
            Get
                Return Color.FromArgb(48, 48, 48)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientBegin As Color
            Get
                Return Color.FromArgb(62, 62, 64)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientMiddle As Color
            Get
                Return Color.FromArgb(62, 62, 64)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientEnd As Color
            Get
                Return Color.FromArgb(62, 62, 64)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedBorder As Color
            Get
                Return Color.FromArgb(62, 62, 64)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientBegin As Color
            Get
                Return Color.FromArgb(0, 122, 204)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientMiddle As Color
            Get
                Return Color.FromArgb(0, 122, 204)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientEnd As Color
            Get
                Return Color.FromArgb(0, 122, 204)
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Set colors on any surface with the "Professional" RenderMode in light mode
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LightModeColorTable
        Inherits ProfessionalColorTable

        Public Overrides ReadOnly Property ToolStripBorder As Color
            Get
                Return Color.FromArgb(239, 239, 242)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
            Get
                Return Color.FromArgb(239, 239, 242)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientBegin As Color
            Get
                Return Color.FromArgb(239, 239, 242)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientMiddle As Color
            Get
                Return Color.FromArgb(239, 239, 242)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientEnd As Color
            Get
                Return Color.FromArgb(239, 239, 242)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelected As Color
            Get
                Return Color.FromArgb(254, 254, 254)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemBorder As Color
            Get
                Return Color.FromArgb(239, 239, 239)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuBorder As Color
            Get
                Return Color.FromArgb(239, 239, 239)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientBegin As Color
            Get
                Return Color.FromArgb(254, 254, 254)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientEnd As Color
            Get
                Return Color.FromArgb(254, 254, 254)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientBegin As Color
            Get
                Return Color.FromArgb(231, 232, 236)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientEnd As Color
            Get
                Return Color.FromArgb(231, 232, 236)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientMiddle As Color
            Get
                Return Color.FromArgb(231, 232, 236)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientBegin As Color
            Get
                Return Color.FromArgb(231, 232, 236)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientEnd As Color
            Get
                Return Color.FromArgb(231, 232, 236)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
            Get
                Return Color.FromArgb(231, 232, 236)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
            Get
                Return Color.FromArgb(231, 232, 236)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
            Get
                Return Color.FromArgb(231, 232, 236)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientBegin As Color
            Get
                Return Color.FromArgb(254, 254, 254)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientMiddle As Color
            Get
                Return Color.FromArgb(254, 254, 254)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientEnd As Color
            Get
                Return Color.FromArgb(254, 254, 254)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedBorder As Color
            Get
                Return Color.FromArgb(254, 254, 254)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientBegin As Color
            Get
                Return Color.FromArgb(0, 122, 204)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientMiddle As Color
            Get
                Return Color.FromArgb(0, 122, 204)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientEnd As Color
            Get
                Return Color.FromArgb(0, 122, 204)
            End Get
        End Property
    End Class

    Sub LoadDTSettings(LoadMode As Integer)
        ' LoadMode = 0; load from registry
        ' LoadMode = 1; load from INI file
        If LoadMode = 0 Then
            If File.Exists(Application.StartupPath & "\portable") Then
                SaveOnSettingsIni = True
                LoadDTSettings(1)
                Exit Sub
            End If
            Try
                Dim KeyStr As String = "Software\DISMTools\" & If(dtBranch.Contains("preview"), "Preview", "Stable")
                Dim Key As RegistryKey = Registry.CurrentUser.OpenSubKey(KeyStr)
                Dim PrgKey As RegistryKey = Key.OpenSubKey("Program")
                If CInt(PrgKey.GetValue("Volatile")) = 1 Then
                    VolatileMode = True
                    Exit Sub
                Else
                    VolatileMode = False
                End If
                DismExe = PrgKey.GetValue("DismExe").ToString().Replace(Quote, "").Trim()
                SaveOnSettingsIni = (CInt(PrgKey.GetValue("SaveOnSettingsIni")) = 1)
                PrgKey.Close()
                Dim PersKey As RegistryKey = Key.OpenSubKey("Personalization")
                ColorMode = PersKey.GetValue("ColorMode")
                Language = PersKey.GetValue("Language")
                LogFont = PersKey.GetValue("LogFont").ToString()
                LogFontSize = CInt(PersKey.GetValue("LogFontSi"))
                LogFontIsBold = (CInt(PersKey.GetValue("LogFontBold")) = 1)
                ProgressPanelStyle = CInt(PersKey.GetValue("SecondaryProgressPanelStyle"))
                AllCaps = (CInt(PersKey.GetValue("AllCaps")) = 1)
                PersKey.Close()
                Dim LogKey As RegistryKey = Key.OpenSubKey("Logs")
                LogFile = LogKey.GetValue("LogFile").ToString().Replace(Quote, "").Trim()
                LogLevel = CInt(LogKey.GetValue("LogLevel"))
                AutoLogs = (CInt(LogKey.GetValue("AutoLogs")) = 1)
                LogKey.Close()
                Dim ImgOpKey As RegistryKey = Key.OpenSubKey("ImgOps")
                QuietOperations = (CInt(ImgOpKey.GetValue("Quiet")) = 1)
                SysNoRestart = (CInt(ImgOpKey.GetValue("NoRestart")) = 1)
                ImgOpKey.Close()
                Dim ScrDirKey As RegistryKey = Key.OpenSubKey("ScratchDir")
                UseScratch = (CInt(ScrDirKey.GetValue("UseScratch")) = 1)
                AutoScrDir = (CInt(ScrDirKey.GetValue("AutoScratch")) = 1)
                ScratchDir = ScrDirKey.GetValue("ScratchDirLocation").ToString().Replace(Quote, "").Trim()
                ScrDirKey.Close()
                Dim OutKey As RegistryKey = Key.OpenSubKey("Output")
                EnglishOutput = (CInt(OutKey.GetValue("EnglishOutput")) = 1)
                ReportView = CInt(OutKey.GetValue("ReportView"))
                OutKey.Close()
                Dim BGKey As RegistryKey = Key.OpenSubKey("BgProcesses")
                NotificationShow = (CInt(BGKey.GetValue("ShowNotification")) = 1)
                NotificationFrequency = CInt(BGKey.GetValue("NotifyFrequency"))
                BGKey.Close()
                Dim AdvBGKey As RegistryKey = Key.OpenSubKey("AdvBgProcesses")
                ExtAppxGetter = (CInt(AdvBGKey.GetValue("EnhancedAppxGetter")) = 1)
                SkipNonRemovable = (CInt(AdvBGKey.GetValue("SkipNonRemovable")) = 1)
                AllDrivers = (CInt(AdvBGKey.GetValue("DetectAllDrivers")) = 1)
                SkipFrameworks = (CInt(AdvBGKey.GetValue("SkipFrameworks")) = 1)
                RunAllProcs = (CInt(AdvBGKey.GetValue("RunAllProcs")) = 1)
                AdvBGKey.Close()
                Dim StartupKey As RegistryKey = Key.OpenSubKey("Startup")
                StartupRemount = (CInt(StartupKey.GetValue("RemountImages")) = 1)
                StartupUpdateCheck = (CInt(StartupKey.GetValue("CheckForUpdates")) = 1)
                StartupKey.Close()
                Dim WndKey As RegistryKey = Key.OpenSubKey("WndParams")
                Width = CInt(WndKey.GetValue("WndWidth"))
                Height = CInt(WndKey.GetValue("WndHeight"))
                StartPosition = If(CInt(WndKey.GetValue("WndCenter")) = 1, FormStartPosition.CenterScreen, FormStartPosition.Manual)
                If StartPosition = FormStartPosition.CenterScreen Then Location = New Point((Screen.FromControl(Me).WorkingArea.Width - Width) / 2, (Screen.FromControl(Me).WorkingArea.Height - Height) / 2)
                If StartPosition <> FormStartPosition.CenterScreen Then
                    Left = CInt(WndKey.GetValue("WndLeft"))
                    Top = CInt(WndKey.GetValue("WndTop"))
                End If
                WindowState = If(CInt(WndKey.GetValue("WndMaximized")) = 1, FormWindowState.Maximized, FormWindowState.Normal)
                WndKey.Close()
                Dim InfoSaverKey As RegistryKey = Key.OpenSubKey("InfoSaver")
                SkipQuestions = (CInt(InfoSaverKey.GetValue("SkipQuestions")) = 1)
                AutoCompleteInfo(0) = (CInt(InfoSaverKey.GetValue("Pkg_CompleteInfo")) = 1)
                AutoCompleteInfo(1) = (CInt(InfoSaverKey.GetValue("Feat_CompleteInfo")) = 1)
                AutoCompleteInfo(2) = (CInt(InfoSaverKey.GetValue("AppX_CompleteInfo")) = 1)
                AutoCompleteInfo(3) = (CInt(InfoSaverKey.GetValue("Cap_CompleteInfo")) = 1)
                AutoCompleteInfo(4) = (CInt(InfoSaverKey.GetValue("Drv_CompleteInfo")) = 1)
                InfoSaverKey.Close()
                Key.Close()
                ' Apply program colors immediately
                ChangePrgColors(ColorMode)
                ' Apply language settings immediately
                ChangeLangs(Language)
            Catch ex As Exception
                LoadDTSettings(1)
                Exit Sub
            End Try
        ElseIf LoadMode = 1 Then
            If File.Exists(Application.StartupPath & "\" & "settings.ini") Then
                DTSettingForm.RichTextBox1.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\settings.ini", UTF8)
                ' Perform the Volatile mode check before applying any settings
                If DTSettingForm.RichTextBox1.Text.Contains("Volatile=0") Then
                    VolatileMode = False
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("Volatile=1") Then
                    VolatileMode = True
                    ' Cancel setting application
                    Exit Sub
                End If
                DismExe = DTSettingForm.RichTextBox1.Lines(3).Replace("DismExe=", "").Trim().Replace(Quote, "").Trim()
                If DismExe.StartsWith("{common:WinDir}", StringComparison.OrdinalIgnoreCase) Then DismExe = DismExe.Replace("{common:WinDir}", Environment.GetFolderPath(Environment.SpecialFolder.Windows)).Trim()
                If DTSettingForm.RichTextBox1.Text.Contains("SaveOnSettingsIni=0") And Not File.Exists(Application.StartupPath & "\portable") Then
                    SaveOnSettingsIni = False
                    LoadDTSettings(0)
                    Exit Sub
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("SaveOnSettingsIni=1") Then
                    SaveOnSettingsIni = True
                End If
                ' Detect program color settings: 0 - Detect system settings
                '                                1 - Light mode
                '                                2 - Dark mode
                If DTSettingForm.RichTextBox1.Text.Contains("ColorMode=0") Then
                    ColorMode = 0
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("ColorMode=1") Then
                    ColorMode = 1
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("ColorMode=2") Then
                    ColorMode = 2
                End If
                ' Apply program colors immediately
                ChangePrgColors(ColorMode)
                ' Detect language settings: 0 - Detect system language (using "ThreeLetterWindowsLanguageName")
                '                         nnn - Apply specific language
                If DTSettingForm.RichTextBox1.Text.Contains("Language=0") Then
                    ' The note above also applies to the Automatic language setting
                    Language = 0
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("Language=1") Then
                    Language = 1
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("Language=2") Then
                    Language = 2
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("Language=3") Then
                    Language = 3
                End If
                ' Apply language settings immediately
                ChangeLangs(Language)
                ' Detect log font setting. Do note that, if a system does not contain the font set in this program,
                ' it will revert to "Courier New"
                For Each line In DTSettingForm.RichTextBox1.Lines
                    If line.StartsWith("LogFont=", StringComparison.OrdinalIgnoreCase) Then
                        LogFont = line.Replace("LogFont=", "").Trim().Replace(Quote, "").Trim()
                    ElseIf line.StartsWith("LogFontSi=", StringComparison.OrdinalIgnoreCase) Then
                        LogFontSize = CInt(line.Replace("LogFontSi=", "").Trim())
                    ElseIf line.StartsWith("LogFile=", StringComparison.OrdinalIgnoreCase) Then
                        ' Detect log file path. If file does not exist, create one
                        LogFile = line.Replace("LogFile=", "").Trim().Replace(Quote, "").Trim()
                        If LogFile.StartsWith("{common:WinDir", StringComparison.OrdinalIgnoreCase) Then LogFile = LogFile.Replace("{common:WinDir}", Environment.GetFolderPath(Environment.SpecialFolder.Windows)).Trim()
                    ElseIf line.StartsWith("ScratchDirLocation=", StringComparison.OrdinalIgnoreCase) Then
                        ScratchDir = line.Replace("ScratchDirLocation=", "").Trim().Replace(Quote, "").Trim()
                    ElseIf line.StartsWith("WndWidth=", StringComparison.OrdinalIgnoreCase) Then
                        Width = CInt(line.Replace("WndWidth=", "").Trim())
                    ElseIf line.StartsWith("WndHeight=", StringComparison.OrdinalIgnoreCase) Then
                        Height = CInt(line.Replace("WndHeight=", "").Trim())
                    ElseIf line.StartsWith("WndCenter=", StringComparison.OrdinalIgnoreCase) Then
                        StartPosition = If(CInt(line.Replace("WndCenter=", "").Trim()) = 1, FormStartPosition.CenterScreen, FormStartPosition.Manual)
                        If StartPosition = FormStartPosition.CenterScreen Then Location = New Point((Screen.FromControl(Me).WorkingArea.Width - Width) / 2, (Screen.FromControl(Me).WorkingArea.Height - Height) / 2)
                    ElseIf line.StartsWith("WndLeft=", StringComparison.OrdinalIgnoreCase) Then
                        If StartPosition <> FormStartPosition.CenterScreen Then Left = CInt(line.Replace("WndLeft=", "").Trim())
                    ElseIf line.StartsWith("WndTop=", StringComparison.OrdinalIgnoreCase) Then
                        If StartPosition <> FormStartPosition.CenterScreen Then Top = CInt(line.Replace("WndTop=", "").Trim())
                    End If
                Next
                If DTSettingForm.RichTextBox1.Text.Contains("LogFontBold=0") Then
                    LogFontIsBold = False
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("LogFontBold=1") Then
                    LogFontIsBold = True
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("SecondaryProgressPanelStyle=0") Then
                    ProgressPanelStyle = 0
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("SecondaryProgressPanelStyle=1") Then
                    ProgressPanelStyle = 1
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("AllCaps=0") Then
                    AllCaps = False
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("AllCaps=1") Then
                    AllCaps = True
                    FileToolStripMenuItem.Text = FileToolStripMenuItem.Text.ToUpper()
                    ProjectToolStripMenuItem.Text = ProjectToolStripMenuItem.Text.ToUpper()
                    CommandsToolStripMenuItem.Text = CommandsToolStripMenuItem.Text.ToUpper()
                    ToolsToolStripMenuItem.Text = ToolsToolStripMenuItem.Text.ToUpper()
                    HelpToolStripMenuItem.Text = HelpToolStripMenuItem.Text.ToUpper()
                End If
                ' Detect log file level: 1 - Errors only
                '                        2 - Errors and warnings
                '                        3 - Errors, warnings and informations
                '                        4 - Errors, warnings, informations and debug messages
                If DTSettingForm.RichTextBox1.Text.Contains("LogLevel=1") Then
                    LogLevel = 1
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("LogLevel=2") Then
                    LogLevel = 2
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("LogLevel=3") Then
                    LogLevel = 3
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("LogLevel=4") Then
                    LogLevel = 4
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("AutoLogs=0") Then
                    AutoLogs = 0
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("AutoLogs=1") Then
                    AutoLogs = 1
                End If
                ' Detect image operation mode: 0 - Offline mode (mounted Windows image)
                '                              1 - Online mode
                ' Do note that online mode is not ready yet
                If DTSettingForm.RichTextBox1.Text.Contains("ImgOperationMode=0") Then
                    ImgOperationMode = 0
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("ImgOperationMode=1") Then
                    ImgOperationMode = 1
                End If
                ' Detect whether operations are performed quietly
                If DTSettingForm.RichTextBox1.Text.Contains("Quiet=0") Then
                    QuietOperations = False
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("Quiet=1") Then
                    QuietOperations = True
                End If
                ' Detect whether system should be restarted automatically
                If DTSettingForm.RichTextBox1.Text.Contains("NoRestart=0") Then
                    SysNoRestart = False
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("NoRestart=1") Then
                    SysNoRestart = True
                End If
                ' Detect whether to use scratch directory
                If DTSettingForm.RichTextBox1.Text.Contains("UseScratch=0") Then
                    UseScratch = False
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("UseScratch=1") Then
                    UseScratch = True
                End If
                ' Detect scratch directory
                If DTSettingForm.RichTextBox1.Text.Contains("AutoScratch=1") Then
                    AutoScrDir = True
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("AutoScratch=0") Then
                    AutoScrDir = False
                End If
                ' Detect whether output should be in English
                If DTSettingForm.RichTextBox1.Text.Contains("EnglishOutput=0") Then
                    EnglishOutput = False
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("EnglishOutput=1") Then
                    EnglishOutput = True
                End If
                ' Detect report view: 0 - List
                '                     1 - Table
                If DTSettingForm.RichTextBox1.Text.Contains("ReportView=0") Then
                    ReportView = 0
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("ReportView=1") Then
                    ReportView = 1
                End If
                ' Show notification: 1 - True
                '                    0 - False
                If DTSettingForm.RichTextBox1.Text.Contains("ShowNotification=1") Then
                    NotificationShow = True
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("ShowNotification=0") Then
                    NotificationShow = False
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("NotifyFrequency=0") Then
                    NotificationFrequency = 0
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("NotifyFrequency=1") Then
                    NotificationFrequency = 1
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("EnhancedAppxGetter=1") Then
                    ExtAppxGetter = True
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("EnhancedAppxGetter=0") Then
                    ExtAppxGetter = False
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("SkipNonRemovable=1") Then
                    SkipNonRemovable = True
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("SkipNonRemovable=0") Then
                    SkipNonRemovable = False
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("DetectAllDrivers=1") Then
                    AllDrivers = True
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("DetectAllDrivers=0") Then
                    AllDrivers = False
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("SkipFrameworks=1") Then
                    SkipFrameworks = True
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("SkipFrameworks=0") Then
                    SkipFrameworks = False
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("RunAllProcs=1") Then
                    RunAllProcs = True
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("RunAllProcs=0") Then
                    RunAllProcs = False
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("RemountImages=1") Then
                    StartupRemount = True
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("RemountImages=0") Then
                    StartupRemount = False
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("CheckForUpdates=1") Then
                    StartupUpdateCheck = True
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("CheckForUpdates=0") Then
                    StartupUpdateCheck = False
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("WndMaximized=1") Then
                    WindowState = FormWindowState.Maximized
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("WndMaximized=0") Then
                    WindowState = FormWindowState.Normal
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("SkipQuestions=1") Then
                    SkipQuestions = True
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("SkipQuestions=0") Then
                    SkipQuestions = False
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("Pkg_CompleteInfo=1") Then
                    AutoCompleteInfo(0) = True
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("Pkg_CompleteInfo=0") Then
                    AutoCompleteInfo(0) = False
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("Feat_CompleteInfo=1") Then
                    AutoCompleteInfo(1) = True
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("Feat_CompleteInfo=0") Then
                    AutoCompleteInfo(1) = False
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("AppX_CompleteInfo=1") Then
                    AutoCompleteInfo(2) = True
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("AppX_CompleteInfo=0") Then
                    AutoCompleteInfo(2) = False
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("Cap_CompleteInfo=1") Then
                    AutoCompleteInfo(3) = True
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("Cap_CompleteInfo=0") Then
                    AutoCompleteInfo(3) = False
                End If
                If DTSettingForm.RichTextBox1.Text.Contains("Drv_CompleteInfo=1") Then
                    AutoCompleteInfo(4) = True
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("Drv_CompleteInfo=0") Then
                    AutoCompleteInfo(4) = False
                End If
            Else
                GenerateDTSettings()
                LoadDTSettings(1)
                Exit Sub
            End If
        End If
        If Debugger.IsAttached Then
            Debug.WriteLine("Settings:" & CrLf & _
                            "DISMExe              =    " & Quote & DismExe & Quote & CrLf & _
                            "SaveOnSettingsIni    =    " & SaveOnSettingsIni & CrLf & _
                            "VolatileMode         =    " & VolatileMode & CrLf & _
                            "ColorMode            =    " & ColorMode & CrLf & _
                            "Language             =    " & Language & CrLf & _
                            "LogFont              =    " & Quote & LogFont & Quote & CrLf & _
                            "LogFontSize          =    " & LogFontSize & CrLf & _
                            "LogFontIsBold        =    " & LogFontIsBold & CrLf & _
                            "LogLevel             =    " & LogLevel & CrLf & _
                            "ImgOperationMode     =    " & ImgOperationMode & CrLf & _
                            "QuietOperations      =    " & QuietOperations & CrLf & _
                            "SysNoRestart         =    " & SysNoRestart & CrLf & _
                            "UseScratch           =    " & UseScratch & CrLf & _
                            "ScratchDir           =    " & Quote & ScratchDir & Quote & CrLf & _
                            "EnglishOutput        =    " & EnglishOutput & CrLf & _
                            "ReportView           =    " & ReportView & CrLf & _
                            "NotificationShow     =    " & NotificationShow & CrLf & _
                            "NotificationFrequency=    " & NotificationFrequency & CrLf & _
                            "ExtAppxGetter        =    " & ExtAppxGetter & CrLf & _
                            "SkipNonRemovable     =    " & SkipNonRemovable & CrLf & _
                            "AllDrivers           =    " & AllDrivers & CrLf & _
                            "StartupRemount       =    " & StartupRemount)
        End If
        ' Test setting validity
        If Not File.Exists(DismExe) Then
            ProblematicStrings(0) = DismExe
            isExeProblematic = True
            DismExe = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
        End If
        Dim TestingFontName As String = LogFont
        Dim siSize As Single = 12
        Using fontTester As New Font(TestingFontName, siSize, FontStyle.Regular, GraphicsUnit.Pixel)
            If Not fontTester.Name = TestingFontName Then
                ProblematicStrings(1) = LogFont
                isLogFontProblematic = True
                LogFont = "Courier New"
            End If
        End Using
        If Not File.Exists(LogFile) Then
            Try
                File.Create(LogFile)
            Catch ex As Exception
                ProblematicStrings(2) = LogFile
                LogFile = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\Logs\DISM\DISM.log"
            End Try
        End If
        If UseScratch Then
            If Not Directory.Exists(ScratchDir) Then
                Try
                    Directory.CreateDirectory(ScratchDir)
                Catch ex As Exception
                    ProblematicStrings(3) = ScratchDir
                    isScratchDirProblematic = True
                    ScratchDir = ""
                    UseScratch = False
                End Try
            End If
        End If
        If isExeProblematic Or isLogFontProblematic Or isLogFileProblematic Or isScratchDirProblematic Then
            InvalidSettingsTSMI.Visible = True
        End If
    End Sub

#Region "Background Processes"

    ''' <summary>
    ''' Runs specified background processes. The program refers to the processes that gather image information as "background processes", due to the way they are run (in the background ;))
    ''' </summary>
    ''' <param name="bgProcOptn">Which processes are run to get image information</param>
    ''' <param name="GatherBasicInfo">When true, this procedure gets the basic image information (image file, index, mountpoint and status)</param>
    ''' <param name="GatherAdvancedInfo">When true, this procedure gets all image information unrelated to packages, features, capabilities, or drivers</param>
    ''' <param name="UseApi">(Optional) Uses the DISM API to get image information and to reduce the time these processes take</param>
    ''' <param name="OnlineMode">(Optional) Detects properties of an active Windows installation if this value is True. Otherwise, if it is False or is not set, it won't pass this option</param>
    ''' <remarks>Depending on the parameter of bgProcOptn, and on the power of the system, the background processes may take a longer time to finish</remarks>
    Sub RunBackgroundProcesses(bgProcOptn As Integer, GatherBasicInfo As Boolean, GatherAdvancedInfo As Boolean, Optional UseApi As Boolean = False, Optional OnlineMode As Boolean = False, Optional OfflineMode As Boolean = False)
        IsCompatible = True
        If isProjectLoaded And GoToNewView Then
            ProjectView.Visible = True
            SplitPanels.Visible = False
        ElseIf isProjectLoaded And Not GoToNewView Then
            ProjectView.Visible = False
            SplitPanels.Visible = True
        End If
        If Not IsImageMounted Then
            Button1.Enabled = True
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            Button5.Enabled = False
            Button6.Enabled = False
            Button7.Enabled = False
            Button8.Enabled = False
            Button9.Enabled = False
            Button10.Enabled = False
            Button11.Enabled = False
            Button12.Enabled = False
            Button13.Enabled = False
            ' Update the buttons in the new design accordingly
            Button26.Enabled = True
            Button27.Enabled = False
            Button28.Enabled = False
            Button29.Enabled = False
            Button24.Enabled = False
            Button25.Enabled = False
            Button30.Enabled = False
            Button31.Enabled = False
            Button32.Enabled = False
            Button33.Enabled = False
            Button34.Enabled = False
            Button35.Enabled = False
            Button36.Enabled = False
            Button37.Enabled = False
            Button38.Enabled = False
            Button39.Enabled = False
            Button40.Enabled = False
            Button41.Enabled = False
            Button42.Enabled = False
            Button43.Enabled = False
            Button44.Enabled = False
            Button45.Enabled = False
            Button46.Enabled = False
            Button47.Enabled = False
            Button48.Enabled = False
            Button49.Enabled = False
            Button50.Enabled = False
            Button51.Enabled = False
            Button52.Enabled = False
            Button53.Enabled = False
            Button54.Enabled = False
            Button55.Enabled = False
            Button56.Enabled = False
            Button57.Enabled = False
            Button58.Enabled = False
            Exit Sub
        End If
        Array.Clear(CompletedTasks, 0, CompletedTasks.Length)
        ' Let user know things are working
        BackgroundProcessesButton.Visible = False
        BackgroundProcessesButton.Image = My.Resources.bg_ops
        BackgroundProcessesButton.Visible = True
        If UseApi Then DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
        areBackgroundProcessesDone = False
        regJumps = False
        irregVal = 0
        pbOpNums = 0
        Dim session As DismSession = Nothing
        If UseApi Then
            If Not OnlineMode And Not OfflineMode Then
                Try
                    For x = 0 To Array.LastIndexOf(MountedImageMountDirs, MountedImageMountDirs.Last)
                        If MountedImageMountDirs(x) = MountDir Then
                            Select Case Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            progressLabel = "Creating session for this image..."
                                        Case "ESN"
                                            progressLabel = "Creando sesión para esta imagen..."
                                        Case "FRA"
                                            progressLabel = "Création d'une session pour cette image en cours..."
                                    End Select
                                Case 1
                                    progressLabel = "Creating session for this image..."
                                Case 2
                                    progressLabel = "Creando sesión para esta imagen..."
                                Case 3
                                    progressLabel = "Création d'une session pour cette image en cours..."
                            End Select
                            ImgBW.ReportProgress(0)
                            sessionMntDir = MountedImageMountDirs(x)
                            Exit For
                        End If
                    Next
                Catch ex As Exception

                End Try
            End If
        End If
        If OfflineMode Then sessionMntDir = MountDir
        ' Determine which actions are being done
        If GatherBasicInfo Then
            If GatherAdvancedInfo Then
                pbOpNums = 2
            Else
                pbOpNums = 1
            End If
        End If
        Select Case bgProcOptn
            Case 0
                pbOpNums += 3
            Case 1
                pbOpNums += 1
            Case 2
                pbOpNums += 1
            Case 3
                pbOpNums += 1
            Case 4
                pbOpNums += 1
            Case 5
                pbOpNums += 1
        End Select
        If pbOpNums > 1 Then progressDivs = 100 / pbOpNums Else progressDivs = 0
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        progressLabel = "Running processes"
                    Case "ESN"
                        progressLabel = "Ejecutando procesos"
                    Case "FRA"
                        progressLabel = "Processus en cours"
                End Select
            Case 1
                progressLabel = "Running processes"
            Case 2
                progressLabel = "Ejecutando procesos"
            Case 3
                progressLabel = "Processus en cours"
        End Select
        ImgBW.ReportProgress(0)
        If GatherBasicInfo Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            progressLabel = "Getting basic image information..."
                        Case "ESN"
                            progressLabel = "Obteniendo información básica de la imagen..."
                        Case "FRA"
                            progressLabel = "Obtention des informations basiques sur l'image en cours..."
                    End Select
                Case 1
                    progressLabel = "Getting basic image information..."
                Case 2
                    progressLabel = "Obteniendo información básica de la imagen..."
                Case 3
                    progressLabel = "Obtention des informations basiques sur l'image en cours..."
            End Select
            ImgBW.ReportProgress(progressMin + progressDivs)
            GetBasicImageInfo(True, OnlineMode, OfflineMode)
            If isOrphaned Then
                'If UseApi And session IsNot Nothing Then DismApi.CloseSession(session)
                Exit Sub
            End If
            If ImgBW.CancellationPending Then
                'If UseApi And session IsNot Nothing Then DismApi.CloseSession(session)
                Exit Sub
            End If
            DetectNTVersion(If(OnlineMode, Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\ntoskrnl.exe", MountDir & "\Windows\system32\ntoskrnl.exe"))
            ' If DetectNTVersion flags this image as incompatible, don't go any further
            If Not IsCompatible Then Exit Sub
            If GatherAdvancedInfo Then
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                progressLabel = "Getting advanced image information..."
                            Case "ESN"
                                progressLabel = "Obteniendo información avanzada de la imagen..."
                            Case "FRA"
                                progressLabel = "Obtention des informations avancées sur l'image en cours..."
                        End Select
                    Case 1
                        progressLabel = "Getting advanced image information..."
                    Case 2
                        progressLabel = "Obteniendo información avanzada de la imagen..."
                    Case 3
                        progressLabel = "Obtention des informations avancées sur l'image en cours..."
                End Select
                ImgBW.ReportProgress(progressMin + progressDivs)
                GetAdvancedImageInfo(True, OnlineMode, OfflineMode)
            End If
        End If
        Directory.CreateDirectory(Application.StartupPath & "\tempinfo")
        ' Parameters for bgProcOptn:
        ' 0 (meta-optn): run every background process
        ' 1: run package background processes
        ' 2: run feature background processes
        ' 3: run AppX package background processes
        ' 4: run FoD background processes
        ' 5: run driver background processes
        Select Case bgProcOptn
            Case 1
                CompletedTasks(0) = False
                CompletedTasks(1) = True
                CompletedTasks(2) = True
                CompletedTasks(3) = True
                CompletedTasks(4) = True
                ' Set pending task
                PendingTasks(0) = True
            Case 2
                CompletedTasks(0) = True
                CompletedTasks(1) = False
                CompletedTasks(2) = True
                CompletedTasks(3) = True
                CompletedTasks(4) = True
                ' Set pending task
                PendingTasks(1) = True
            Case 3
                CompletedTasks(0) = True
                CompletedTasks(1) = True
                CompletedTasks(2) = False
                CompletedTasks(3) = True
                CompletedTasks(4) = True
                ' Set pending task
                PendingTasks(2) = True
            Case 4
                CompletedTasks(0) = True
                CompletedTasks(1) = True
                CompletedTasks(2) = True
                CompletedTasks(3) = False
                CompletedTasks(4) = True
                ' Set pending task
                PendingTasks(3) = True
            Case 5
                CompletedTasks(0) = True
                CompletedTasks(1) = True
                CompletedTasks(2) = True
                CompletedTasks(3) = True
                CompletedTasks(4) = False
                ' Set pending task
                PendingTasks(4) = True
        End Select
        regJumps = True
        progressMin = 20
        Select Case bgProcOptn
            Case 0
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                progressLabel = "Getting image packages..."
                            Case "ESN"
                                progressLabel = "Obteniendo paquetes de la imagen..."
                            Case "FRA"
                                progressLabel = "Obtention des paquets de l'image en cours..."
                        End Select
                    Case 1
                        progressLabel = "Getting image packages..."
                    Case 2
                        progressLabel = "Obteniendo paquetes de la imagen..."
                    Case 3
                        progressLabel = "Obtention des paquets de l'image en cours..."
                End Select
                ImgBW.ReportProgress(20)
                GetImagePackages(True, OnlineMode)
                If ImgBW.CancellationPending Then
                    If UseApi And session IsNot Nothing Then DismApi.CloseSession(session)
                    Exit Sub
                End If
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                progressLabel = "Getting image features..."
                            Case "ESN"
                                progressLabel = "Obteniendo características de la imagen..."
                            Case "FRA"
                                progressLabel = "Obtention des caractéristiques de l'image en cours..."
                        End Select
                    Case 1
                        progressLabel = "Getting image features..."
                    Case 2
                        progressLabel = "Obteniendo características de la imagen..."
                    Case 3
                        progressLabel = "Obtention des caractéristiques de l'image en cours..."
                End Select
                ImgBW.ReportProgress(progressMin + progressDivs)
                GetImageFeatures(True, OnlineMode)
                If ImgBW.CancellationPending Then
                    If UseApi And session IsNot Nothing Then DismApi.CloseSession(session)
                    Exit Sub
                End If
                If imgEdition Is Nothing Then imgEdition = ""
                If IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
                    If Not imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) And Not (imgInstType.Contains("Nano") Or imgInstType.Contains("Core")) Then
                        Debug.WriteLine("[IsWindows8OrHigher] Returned True")
                        pbOpNums += 1
                        Select Case Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        progressLabel = "Getting image provisioned AppX packages (Metro-style applications)..."
                                    Case "ESN"
                                        progressLabel = "Obteniendo paquetes aprovisionados AppX de la imagen (aplicaciones estilo Metro)..."
                                    Case "FRA"
                                        progressLabel = "Obtention des paquets AppX (applications de style Metro) provisionnés de l'image en cours..."
                                End Select
                            Case 1
                                progressLabel = "Getting image provisioned AppX packages (Metro-style applications)..."
                            Case 2
                                progressLabel = "Obteniendo paquetes aprovisionados AppX de la imagen (aplicaciones estilo Metro)..."
                            Case 3
                                progressLabel = "Obtention des paquets AppX (applications de style Metro) provisionnés de l'image en cours..."
                        End Select
                        ImgBW.ReportProgress(progressMin + progressDivs)
                        GetImageAppxPackages(True, OnlineMode)
                        If ImgBW.CancellationPending Then
                            If UseApi And session IsNot Nothing Then DismApi.CloseSession(session)
                            Exit Sub
                        End If
                    Else
                        Debug.WriteLine("[IsWindows8OrHigher] Returned False")
                    End If
                Else
                    Debug.WriteLine("[IsWindows8OrHigher] Returned False")
                End If
                If IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") And Not imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Then
                    If Not imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) And Not imgInstType.Contains("Nano") Then
                        Debug.WriteLine("[IsWindows10OrHigher] Returned True")
                        pbOpNums += 1
                        Select Case Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        progressLabel = "Getting image Features on Demand (capabilities)..."
                                    Case "ESN"
                                        progressLabel = "Obteniendo características opcionales de la imagen (funcionalidades)..."
                                    Case "FRA"
                                        progressLabel = "Obtention de caractéristiques de l'image à la demande (capacités) en cours..."
                                End Select
                            Case 1
                                progressLabel = "Getting image Features on Demand (capabilities)..."
                            Case 2
                                progressLabel = "Obteniendo características opcionales de la imagen (funcionalidades)..."
                            Case 3
                                progressLabel = "Obtention de caractéristiques de l'image à la demande (capacités) en cours..."
                        End Select
                        ImgBW.ReportProgress(progressMin + progressDivs)
                        GetImageCapabilities(True, OnlineMode)
                        If ImgBW.CancellationPending Then
                            If UseApi And session IsNot Nothing Then DismApi.CloseSession(session)
                            Exit Sub
                        End If
                    Else
                        Debug.WriteLine("[IsWindows10OrHigher] Returned False")
                    End If
                Else
                    Debug.WriteLine("[IsWindows10OrHigher] Returned False")
                End If
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                progressLabel = "Getting image drivers..."
                            Case "ESN"
                                progressLabel = "Obteniendo controladores de la imagen..."
                            Case "FRA"
                                progressLabel = "Obtention des pilotes de l'image en cours..."
                        End Select
                    Case 1
                        progressLabel = "Getting image drivers..."
                    Case 2
                        progressLabel = "Obteniendo controladores de la imagen..."
                    Case 3
                        progressLabel = "Obtention des pilotes de l'image en cours..."
                End Select
                ImgBW.ReportProgress(progressMin + progressDivs)
                GetImageDrivers(True, OnlineMode)
                If ImgBW.CancellationPending Then
                    If UseApi And session IsNot Nothing Then DismApi.CloseSession(session)
                    Exit Sub
                End If
            Case 1
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                progressLabel = "Getting image packages..."
                            Case "ESN"
                                progressLabel = "Obteniendo paquetes de la imagen..."
                            Case "FRA"
                                progressLabel = "Obtention des paquets de l'image en cours..."
                        End Select
                    Case 1
                        progressLabel = "Getting image packages..."
                    Case 2
                        progressLabel = "Obteniendo paquetes de la imagen..."
                    Case 3
                        progressLabel = "Obtention des paquets de l'image en cours..."
                End Select
                ImgBW.ReportProgress(20)
                GetImagePackages(True, OnlineMode)
            Case 2
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                progressLabel = "Getting image features..."
                            Case "ESN"
                                progressLabel = "Obteniendo características de la imagen..."
                            Case "FRA"
                                progressLabel = "Obtention des caractéristiques de l'image en cours..."
                        End Select
                    Case 1
                        progressLabel = "Getting image features..."
                    Case 2
                        progressLabel = "Obteniendo características de la imagen..."
                    Case 3
                        progressLabel = "Obtention des caractéristiques de l'image en cours..."
                End Select
                ImgBW.ReportProgress(progressMin + progressDivs)
                GetImageFeatures(True, OnlineMode)
            Case 3
                If IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") = True Then
                    Debug.WriteLine("[IsWindows8OrHigher] Returned True")
                    pbOpNums += 1
                    Select Case Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    progressLabel = "Getting image provisioned AppX packages (Metro-style applications)..."
                                Case "ESN"
                                    progressLabel = "Obteniendo paquetes aprovisionados AppX de la imagen (aplicaciones estilo Metro)..."
                                Case "FRA"
                                    progressLabel = "Obtention des paquets AppX (applications de style Metro) provisionnés de l'image en cours..."
                            End Select
                        Case 1
                            progressLabel = "Getting image provisioned AppX packages (Metro-style applications)..."
                        Case 2
                            progressLabel = "Obteniendo paquetes aprovisionados AppX de la imagen (aplicaciones estilo Metro)..."
                        Case 3
                            progressLabel = "Obtention des paquets AppX (applications de style Metro) provisionnés de l'image en cours..."
                    End Select
                    ImgBW.ReportProgress(progressMin + progressDivs)
                    GetImageAppxPackages(True, OnlineMode)
                Else
                    Debug.WriteLine("[IsWindows8OrHigher] Returned False")
                    PendingTasks(2) = False
                End If
            Case 4
                If IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") = True Then
                    Debug.WriteLine("[IsWindows10OrHigher] Returned True")
                    pbOpNums += 1
                    Select Case Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    progressLabel = "Getting image Features on Demand (capabilities)..."
                                Case "ESN"
                                    progressLabel = "Obteniendo características opcionales de la imagen (funcionalidades)..."
                                Case "FRA"
                                    progressLabel = "Obtention de caractéristiques de l'image à la demande (capacités) en cours..."
                            End Select
                        Case 1
                            progressLabel = "Getting image Features on Demand (capabilities)..."
                        Case 2
                            progressLabel = "Obteniendo características opcionales de la imagen (funcionalidades)..."
                        Case 3
                            progressLabel = "Obtention de caractéristiques de l'image à la demande (capacités) en cours..."
                    End Select
                    ImgBW.ReportProgress(progressMin + progressDivs)
                    GetImageCapabilities(True, OnlineMode)
                Else
                    Debug.WriteLine("[IsWindows10OrHigher] Returned False")
                    PendingTasks(3) = False
                End If
            Case 5
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                progressLabel = "Getting image drivers..."
                            Case "ESN"
                                progressLabel = "Obteniendo controladores de la imagen..."
                            Case "FRA"
                                progressLabel = "Obtention des pilotes de l'image en cours..."
                        End Select
                    Case 1
                        progressLabel = "Getting image drivers..."
                    Case 2
                        progressLabel = "Obteniendo controladores de la imagen..."
                    Case 3
                        progressLabel = "Obtention des pilotes de l'image en cours..."
                End Select
                ImgBW.ReportProgress(progressMin + progressDivs)
                GetImageDrivers(True, OnlineMode)
        End Select
        If bgProcOptn <> 0 And PendingTasks.Contains(True) Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            progressLabel = "Running pending tasks. This may take some time..."
                        Case "ESN"
                            progressLabel = "Ejecutando tareas pendientes. Esto puede llevar algo de tiempo..."
                        Case "FRA"
                            progressLabel = "Exécution des tâches en cours. Cela peut prendre un certain temps ..."
                    End Select
                Case 1
                    progressLabel = "Running pending tasks. This may take some time..."
                Case 2
                    progressLabel = "Ejecutando tareas pendientes. Esto puede llevar algo de tiempo..."
                Case 3
                    progressLabel = "Exécution des tâches en cours. Cela peut prendre un certain temps ..."
            End Select
            ImgBW.ReportProgress(99)
            If PendingTasks(0) Then GetImagePackages(True, OnlineMode)
            If PendingTasks(1) Then GetImageFeatures(True, OnlineMode)
            If PendingTasks(2) Then GetImageAppxPackages(True, OnlineMode)
            If PendingTasks(3) Then GetImageCapabilities(True, OnlineMode)
            If PendingTasks(4) Then GetImageDrivers(True, OnlineMode)
        End If
        DeleteTempFiles()
        If UseApi And session IsNot Nothing Then
            DismApi.CloseSession(session)
        End If
    End Sub

    ''' <summary>
    ''' Gets basic image information, such as its index, its file path, or its mount dir
    ''' </summary>
    ''' <remarks>Depending on the GatherBasicInfo flag in RunBackgroundProcesses, this function will run or not</remarks>
    Sub GetBasicImageInfo(Optional Streamlined As Boolean = False, Optional OnlineMode As Boolean = False, Optional OfflineMode As Boolean = False)
        ' Set image properties
        Label14.Text = ProgressPanel.ImgIndex
        Label12.Text = ProgressPanel.MountDir
        Label41.Text = Label14.Text
        Label44.Text = Label12.Text
        ' Loading the project directly with an image already mounted makes the two labels above be wrong.
        ' Check them and use local vars
        If Label14.Text = "0" Or Label12.Text = "" Then     ' Label14 (index preview label) returns 0 and Label12 (mount dir preview) returns blank
            Label14.Text = ImgIndex
            Label12.Text = MountDir
            Label41.Text = Label14.Text
            Label44.Text = Label12.Text
        End If
        If Streamlined Then
            If OnlineMode Then
                Label17.Text = Environment.OSVersion.Version.Major & "." & Environment.OSVersion.Version.Minor & "." & Environment.OSVersion.Version.Build & "." & FileVersionInfo.GetVersionInfo(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\ntoskrnl.exe").ProductPrivatePart
                imgVersionInfo = Environment.OSVersion.Version
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                Label14.Text = "(Online installation)"
                                Label20.Text = "(Online installation)"
                                projName.Text = "(Online installation)"
                            Case "ESN"
                                Label14.Text = "(Instalación activa)"
                                Label20.Text = "(Instalación activa)"
                                projName.Text = "(Instalación activa)"
                            Case "FRA"
                                Label14.Text = "(Installation en ligne)"
                                Label20.Text = "(Installation en ligne)"
                                projName.Text = "(Installation en ligne)"
                        End Select
                    Case 1
                        Label14.Text = "(Online installation)"
                        Label20.Text = "(Online installation)"
                        projName.Text = "(Online installation)"
                    Case 2
                        Label14.Text = "(Instalación activa)"
                        Label20.Text = "(Instalación activa)"
                        projName.Text = "(Instalación activa)"
                    Case 3
                        Label14.Text = "(Installation en ligne)"
                        Label20.Text = "(Installation en ligne)"
                        projName.Text = "(Installation en ligne)"
                End Select
                Label18.Text = My.Computer.Info.OSFullName
                Label12.Text = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows))
                Label3.Text = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows))
                Label49.Text = projName.Text
                Label52.Text = Label3.Text
                Label44.Text = Label12.Text
                Label46.Text = Label18.Text
                Label41.Text = Label14.Text
                Label48.Text = Label17.Text
                Label47.Text = Label20.Text
                ' Disable tasks in the new design accordingly
                Button24.Enabled = False
                Button25.Enabled = False
                Button26.Enabled = False
                Button27.Enabled = False
                Button28.Enabled = False
                Button29.Enabled = False
            ElseIf OfflineMode Then
                Label17.Text = FileVersionInfo.GetVersionInfo(MountDir & "\Windows\system32\ntoskrnl.exe").ProductVersion
                imgVersionInfo = New Version(FileVersionInfo.GetVersionInfo(MountDir & "\Windows\system32\ntoskrnl.exe").ProductVersion)
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                Label14.Text = "(Offline installation)"
                                Label18.Text = "(Offline installation)"
                                Label20.Text = "(Offline installation)"
                                projName.Text = "(Offline installation)"
                            Case "ESN"
                                Label14.Text = "(Instalación fuera de línea)"
                                Label18.Text = "(Instalación fuera de línea)"
                                Label20.Text = "(Instalación fuera de línea)"
                                projName.Text = "(Instalación fuera de línea)"
                            Case "FRA"
                                Label14.Text = "(Installation hors ligne)"
                                Label18.Text = "(Installation hors ligne)"
                                Label20.Text = "(Installation hors ligne)"
                                projName.Text = "(Installation hors ligne)"
                        End Select
                    Case 1
                        Label14.Text = "(Offline installation)"
                        Label18.Text = "(Offline installation)"
                        Label20.Text = "(Offline installation)"
                        projName.Text = "(Offline installation)"
                    Case 2
                        Label14.Text = "(Instalación fuera de línea)"
                        Label18.Text = "(Instalación fuera de línea)"
                        Label20.Text = "(Instalación fuera de línea)"
                        projName.Text = "(Instalación fuera de línea)"
                    Case 3
                        Label14.Text = "(Installation hors ligne)"
                        Label18.Text = "(Installation hors ligne)"
                        Label20.Text = "(Installation hors ligne)"
                        projName.Text = "(Installation hors ligne)"
                End Select
                Label12.Text = MountDir
                Label44.Text = MountDir
                Label41.Text = Label14.Text
                Label46.Text = Label18.Text
                Label47.Text = Label20.Text
                Label48.Text = Label17.Text
                Label49.Text = projName.Text
                Label3.Text = MountDir
                Label52.Text = Label3.Text
                ' Disable tasks in the new design accordingly
                Button24.Enabled = False
                Button25.Enabled = False
                Button26.Enabled = False
                Button27.Enabled = False
                Button28.Enabled = False
                Button29.Enabled = False
                GetOfflineEditionAndInstIdFromRegistry()
            Else
                Try
                    For x = 0 To Array.LastIndexOf(MountedImageImgFiles, MountedImageImgFiles.Last)
                        If MountedImageImgFiles(x) = SourceImg Then
                            Label14.Text = MountedImageImgIndexes(x)
                            Label12.Text = MountedImageMountDirs(x)

                            Label44.Text = Label12.Text
                            Label41.Text = Label14.Text
                            If MountedImageImgStatuses(x) = 0 Then
                                isOrphaned = False
                            ElseIf MountedImageImgStatuses(x) = 1 Then
                                isOrphaned = True
                            End If
                            Dim ImageInfoCollection As DismImageInfoCollection = DismApi.GetImageInfo(MountedImageImgFiles(x))
                            For Each imageInfo As DismImageInfo In ImageInfoCollection
                                If imageInfo.ImageIndex = MountedImageImgIndexes(x) Then
                                    SysVer = imageInfo.ProductVersion
                                    Label17.Text = imageInfo.ProductVersion.ToString()
                                    Label18.Text = imageInfo.ImageName
                                    Label20.Text = imageInfo.ImageDescription

                                    Label48.Text = imageInfo.ProductVersion.ToString()
                                    Label46.Text = imageInfo.ImageName
                                    Label47.Text = imageInfo.ImageDescription
                                End If
                            Next
                            RemountImageWithWritePermissionsToolStripMenuItem.Enabled = If(MountedImageMountedReWr(x) = 0, False, True)
                            Button2.Enabled = If(MountedImageMountedReWr(x) = 0, True, False)
                            Button3.Enabled = If(MountedImageMountedReWr(x) = 0, True, False)
                            Button4.Enabled = True
                            Button27.Enabled = If(MountedImageMountedReWr(x) = 0, True, False)
                            Button28.Enabled = If(MountedImageMountedReWr(x) = 0, True, False)
                            Button29.Enabled = True
                            Exit For
                        End If
                    Next
                Catch ex As Exception

                End Try
            End If
            Exit Sub
        End If
        Try
            If ProgressPanel.MountDir = "" Then
                Throw New Exception
            Else
                Dim KeVerInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(ProgressPanel.MountDir & "\Windows\system32\ntoskrnl.exe")    ' Get version info from ntoskrnl.exe
                Dim KeVerStr As String = KeVerInfo.ProductVersion
                Label17.Text = KeVerStr
                Select Case DismVersionChecker.ProductMajorPart
                    Case 6
                        Select Case DismVersionChecker.ProductMinorPart
                            Case 1
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " > imgname", _
                                                  ASCII)
                            Case Is >= 2
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " > imgname", _
                                                  ASCII)
                        End Select
                    Case 10
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " > imgname", _
                                          ASCII)
                End Select
                Process.Start(Application.StartupPath & "\bin\exthelpers\temp.bat").WaitForExit()
                Label18.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\imgname").Replace("Name : ", "").Trim()
                File.Delete(Application.StartupPath & "\imgname")
                File.Delete(Application.StartupPath & "\bin\exthelpers\temp.bat")
                Select Case DismVersionChecker.ProductMajorPart
                    Case 6
                        Select Case DismVersionChecker.ProductMinorPart
                            Case 1
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                                  ASCII)
                            Case Is >= 2
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                                  ASCII)
                        End Select
                    Case 10
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                          ASCII)
                End Select
                Process.Start(Application.StartupPath & "\bin\exthelpers\temp.bat").WaitForExit()
                Label20.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\imgdesc").Replace("Description : ", "").Trim()
                File.Delete(Application.StartupPath & "\imgdesc")
                File.Delete(Application.StartupPath & "\bin\exthelpers\temp.bat")
                If Label18.Text = "" Or Label20.Text = "" Then
                    Label18.Text = imgMountedName
                    Label20.Text = imgMountedDesc
                End If
            End If
        Catch ex As Exception
            ' Maybe it was loaded directly. Check local vars
            Try
                Dim KeVerInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(MountDir & "\Windows\system32\ntoskrnl.exe")    ' Get version info from ntoskrnl.exe
                Dim KeVerStr As String = KeVerInfo.ProductVersion
                Label17.Text = KeVerStr
                Select Case DismVersionChecker.ProductMajorPart
                    Case 6
                        Select Case DismVersionChecker.ProductMinorPart
                            Case 1
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Name" & Quote & " > imgname", _
                                                  ASCII)
                            Case Is >= 2
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Name" & Quote & " > imgname", _
                                                  ASCII)
                        End Select
                    Case 10
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Name" & Quote & " > imgname", _
                                          ASCII)
                End Select
                Process.Start(Application.StartupPath & "\bin\exthelpers\temp.bat").WaitForExit()
                Label18.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\imgname").Replace("Name : ", "").Trim()
                File.Delete(Application.StartupPath & "\imgname")
                File.Delete(Application.StartupPath & "\bin\exthelpers\temp.bat")
                Select Case DismVersionChecker.ProductMajorPart
                    Case 6
                        Select Case DismVersionChecker.ProductMinorPart
                            Case 1
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                                  ASCII)
                            Case Is >= 2
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                                  ASCII)
                        End Select
                    Case 10
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                          ASCII)
                End Select
                Process.Start(Application.StartupPath & "\bin\exthelpers\temp.bat").WaitForExit()
                Label20.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\imgdesc").Replace("Description : ", "").Trim()
                File.Delete(Application.StartupPath & "\imgdesc")
                File.Delete(Application.StartupPath & "\bin\exthelpers\temp.bat")
                If Label18.Text = "" Or Label20.Text = "" Then
                    Label18.Text = imgMountedName
                    Label20.Text = imgMountedDesc
                End If
            Catch ex2 As Exception      ' It is clear that something went seriously wrong. Assume the image was unmounted before loading the proj in first place
                UpdateImgProps()        ' and exit the sub (a.k.a., give up)
                Exit Sub
            End Try
        End Try
        DetectNTVersion(MountDir & "\Windows\system32\ntoskrnl.exe")
        ' Detect whether the image needs a servicing session reload
        Directory.CreateDirectory(projPath & "\tempinfo")
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                          "@echo off" & CrLf & _
                                          "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & projPath & "\tempinfo\imgmountedstatus", ASCII)
                    Case Is >= 2
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                          "@echo off" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & projPath & "\tempinfo\imgmountedstatus", ASCII)
                End Select
            Case 10
                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                  "@echo off" & CrLf & _
                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & projPath & "\tempinfo\imgmountedstatus", ASCII)
        End Select
        Process.Start(Application.StartupPath & "\bin\exthelpers\imginfo.bat").WaitForExit()
        mountedImgStatus = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgmountedstatus", ASCII).Replace("Status : ", "").Trim()
        File.Delete(Application.StartupPath & "\bin\exthelpers\imginfo.bat")
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                          "@echo off" & CrLf & _
                                          "dism /English /get-wiminfo /wimfile=" & SourceImg & " | find /c " & Quote & "Index" & Quote & " > " & projPath & "\tempinfo\indexcount", ASCII)
                    Case Is >= 2
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                          "@echo off" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " | find /c " & Quote & "Index" & Quote & " > " & projPath & "\tempinfo\indexcount", ASCII)
                End Select
            Case 10
                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                  "@echo off" & CrLf & _
                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " | find /c " & Quote & "Index" & Quote & " > " & projPath & "\tempinfo\indexcount", ASCII)
        End Select
        Process.Start(Application.StartupPath & "\bin\exthelpers\imginfo.bat").WaitForExit()
        imgIndexCount = CInt(My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\indexcount", ASCII))
        File.Delete(Application.StartupPath & "\bin\exthelpers\imginfo.bat")
        For Each FoundFile In My.Computer.FileSystem.GetFiles(projPath & "\tempinfo", FileIO.SearchOption.SearchTopLevelOnly)
            File.Delete(FoundFile)
        Next
        Directory.Delete(projPath & "\tempinfo")
        If mountedImgStatus = "Ok" Then
            isOrphaned = False
        ElseIf mountedImgStatus = "Needs Remount" Then
            isOrphaned = True
        End If
        If isOrphaned Then
            Exit Sub
        End If
        irregVal = 5
        'ImgBW.ReportProgress(irregVal)
    End Sub

    Sub GetOfflineEditionAndInstIdFromRegistry()
        Using reg As New Process
            reg.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe"
            reg.StartInfo.Arguments = "load HKLM\IMG_SOFT " & Quote & MountDir & "\Windows\system32\config\SOFTWARE" & Quote
            reg.StartInfo.CreateNoWindow = True
            reg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            reg.Start()
            reg.WaitForExit()
            If reg.ExitCode <> 0 Then
                imgEdition = ""
            Else
                Dim edReg As RegistryKey = Registry.LocalMachine.OpenSubKey("IMG_SOFT\Microsoft\Windows NT\CurrentVersion", False)
                imgEdition = edReg.GetValue("EditionID", "").ToString()
                imgInstType = edReg.GetValue("InstallationType", "").ToString()
                edReg.Close()
            End If
            reg.StartInfo.Arguments = "unload HKLM\IMG_SOFT"
            reg.StartInfo.CreateNoWindow = True
            reg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            reg.Start()
            reg.WaitForExit()
        End Using
    End Sub

    ''' <summary>
    ''' Gets advanced image information, such as number of files and directories, image name, and more
    ''' </summary>
    ''' <remarks>This is called when bgGetAdvImgInfo is True</remarks>
    Sub GetAdvancedImageInfo(Optional UseApi As Boolean = False, Optional OnlineMode As Boolean = False, Optional OfflineMode As Boolean = False)
        Button14.Enabled = True
        Button15.Enabled = True
        LinkLabel20.Enabled = True
        Button16.Enabled = True
        LinkLabel19.Enabled = True
        ExplorerView.Enabled = True
        LinkLabel15.Enabled = True
        LinkLabel16.Enabled = True
        ProjNameEditBtn.Visible = True
        If UseApi Then
            If OnlineMode Then
                Button14.Enabled = False
                Button15.Enabled = False
                LinkLabel20.Enabled = False
                Button16.Enabled = False
                LinkLabel19.Enabled = False
                ExplorerView.Enabled = False
                ProjNameEditBtn.Visible = False
                LinkLabel15.Enabled = False
                LinkLabel16.Enabled = False
                ' Set edition variable according to the EditionID registry value
                imgEdition = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion").GetValue("EditionID")

                ' Set installation type variable according to the InstallationType registry value
                imgInstType = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion").GetValue("InstallationType")

                Button24.Enabled = False
                Button25.Enabled = False
                Button26.Enabled = False
                Button27.Enabled = False
                Button28.Enabled = False
                Button29.Enabled = False

                DetectVersions(FileVersionInfo.GetVersionInfo(DismExe), imgVersionInfo)
                Exit Sub
            ElseIf OfflineMode Then
                Button14.Enabled = False
                Button15.Enabled = False
                LinkLabel20.Enabled = False
                Button16.Enabled = False
                LinkLabel19.Enabled = False
                ExplorerView.Enabled = False
                ProjNameEditBtn.Visible = False
                LinkLabel15.Enabled = False
                LinkLabel16.Enabled = False
                Button24.Enabled = False
                Button25.Enabled = False
                Button26.Enabled = False
                Button27.Enabled = False
                Button28.Enabled = False
                Button29.Enabled = False
                DetectVersions(FileVersionInfo.GetVersionInfo(DismExe), imgVersionInfo)
                Exit Sub
            Else
                If IsImageMounted Then
                    Try
                        For x = 0 To Array.LastIndexOf(MountedImageImgFiles, MountedImageImgFiles.Last)
                            If MountedImageMountDirs(x) = MountDir Then
                                Dim ImageInfoCollection As DismImageInfoCollection = DismApi.GetImageInfo(MountedImageImgFiles(x))
                                For Each imageInfo As DismImageInfo In ImageInfoCollection
                                    If imageInfo.ImageIndex = MountedImageImgIndexes(x) Then
                                        imgVersionInfo = imageInfo.ProductVersion
                                        imgMountedName = imageInfo.ImageName
                                        imgMountedDesc = imageInfo.ImageDescription
                                        imgHal = If(Not imageInfo.Hal = "", imageInfo.Hal, "Undefined by the image")
                                        imgSPBuild = imageInfo.ProductVersion.Revision
                                        imgSPLvl = imageInfo.SpLevel
                                        imgEdition = imageInfo.EditionId
                                        imgPType = imageInfo.ProductType
                                        imgPSuite = imageInfo.ProductSuite
                                        imgSysRoot = imageInfo.SystemRoot
                                        If imgLangs <> "" Then imgLangs = ""
                                        For Each imageLang In imageInfo.Languages
                                            imgLangs &= imageLang.Name & If(imageInfo.DefaultLanguage.Name = imageLang.Name, " (default)", "") & ", "
                                        Next
                                        Dim langArr() As Char = imgLangs.ToCharArray()
                                        langArr(langArr.Count - 2) = ""
                                        imgLangs = New String(langArr)
                                        imgFormat = Path.GetExtension(MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper() & " file"
                                        imgRW = If(MountedImageMountedReWr(x) = 0, "Yes", "No")
                                        imgDirs = imageInfo.CustomizedInfo.DirectoryCount
                                        imgFiles = imageInfo.CustomizedInfo.FileCount
                                        imgCreation = imageInfo.CustomizedInfo.CreatedTime
                                        imgModification = imageInfo.CustomizedInfo.ModifiedTime
                                        imgInstType = imageInfo.InstallationType
                                    End If
                                Next
                            End If
                        Next
                    Catch ex As Exception
                        Exit Try
                    End Try
                    ' Time to use the DISM executable
                    Try     ' Try getting image properties
                        If Not Directory.Exists(projPath & "\tempinfo") Then
                            Directory.CreateDirectory(projPath & "\tempinfo").Attributes = FileAttributes.Hidden
                        End If
                        Select Case DismVersionChecker.ProductMajorPart
                            Case 6
                                Select Case DismVersionChecker.ProductMinorPart
                                    Case 1
                                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat",
                                                          "@echo off" & CrLf &
                                                          "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & projPath & "\tempinfo\imgwimboot", ASCII)
                                    Case Is >= 2
                                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat",
                                                          "@echo off" & CrLf &
                                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & projPath & "\tempinfo\imgwimboot", ASCII)
                                End Select
                            Case 10
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat",
                                                  "@echo off" & CrLf &
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & projPath & "\tempinfo\imgwimboot", ASCII)
                        End Select
                        If Debugger.IsAttached Then
                            Process.Start("\Windows\system32\notepad.exe", Application.StartupPath & "\bin\exthelpers\imginfo.bat").WaitForExit()
                        End If
                        Using WIMBootProc As New Process()
                            WIMBootProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
                            WIMBootProc.StartInfo.Arguments = "/c " & Quote & Application.StartupPath & "\bin\exthelpers\imginfo.bat" & Quote
                            WIMBootProc.StartInfo.CreateNoWindow = True
                            WIMBootProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                            WIMBootProc.Start()
                            WIMBootProc.WaitForExit()
                        End Using
                        Try
                            imgWimBootStatus = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgwimboot", ASCII).Replace("WIM Bootable : ", "").Trim()
                            If Not ImgBW.IsBusy Then
                                For Each foundFile In My.Computer.FileSystem.GetFiles(projPath & "\tempinfo", FileIO.SearchOption.SearchTopLevelOnly)
                                    File.Delete(foundFile)
                                Next
                                Directory.Delete(projPath & "\tempinfo")
                            End If
                            File.Delete(Application.StartupPath & "\bin\exthelpers\imginfo.bat")
                        Catch ex As Exception

                        End Try
                    Catch ex As Exception
                        Exit Try
                    End Try
                    Button1.Enabled = False
                    'Button2.Enabled = True
                    'Button3.Enabled = True
                    'Button4.Enabled = True
                    Button5.Enabled = True
                    Button6.Enabled = True
                    Button7.Enabled = True
                    Button8.Enabled = True
                    Button9.Enabled = True
                    Button10.Enabled = True
                    Button11.Enabled = True
                    Button12.Enabled = True
                    Button13.Enabled = True
                    ' Update the buttons in the new design accordingly
                    Button26.Enabled = False
                    'Button27.Enabled = True
                    'Button28.Enabled = True
                    'Button29.Enabled = True
                    Button24.Enabled = True
                    Button25.Enabled = True
                    Button30.Enabled = True
                    Button31.Enabled = True
                    Button32.Enabled = True
                    Button33.Enabled = True
                    Button34.Enabled = True
                    Button35.Enabled = True
                    Button36.Enabled = True
                    Button37.Enabled = True
                    Button38.Enabled = True
                    Button39.Enabled = True
                    Button40.Enabled = True
                    Button41.Enabled = True
                    Button42.Enabled = True
                    Button43.Enabled = True
                    Button44.Enabled = True
                    Button45.Enabled = True
                    Button46.Enabled = True
                    Button47.Enabled = True
                    Button48.Enabled = True
                    Button49.Enabled = True
                    Button50.Enabled = True
                    Button51.Enabled = True
                    Button52.Enabled = True
                    Button53.Enabled = True
                    Button54.Enabled = True
                    Button55.Enabled = True
                    Button56.Enabled = True
                    Button57.Enabled = True
                    Button58.Enabled = True
                    MountImageToolStripMenuItem.Enabled = False
                    UnmountImageToolStripMenuItem.Enabled = True
                Else
                    Button1.Enabled = True
                    Button2.Enabled = False
                    Button3.Enabled = False
                    Button4.Enabled = False
                    Button5.Enabled = False
                    Button6.Enabled = False
                    Button7.Enabled = False
                    Button8.Enabled = False
                    Button9.Enabled = False
                    Button10.Enabled = False
                    Button11.Enabled = False
                    Button12.Enabled = False
                    Button13.Enabled = False
                    ' Update the buttons in the new design accordingly
                    Button26.Enabled = True
                    Button27.Enabled = False
                    Button28.Enabled = False
                    Button29.Enabled = False
                    Button24.Enabled = False
                    Button25.Enabled = False
                    Button30.Enabled = False
                    Button31.Enabled = False
                    Button32.Enabled = False
                    Button33.Enabled = False
                    Button34.Enabled = False
                    Button35.Enabled = False
                    Button36.Enabled = False
                    Button37.Enabled = False
                    Button38.Enabled = False
                    Button39.Enabled = False
                    Button40.Enabled = False
                    Button41.Enabled = False
                    Button42.Enabled = False
                    Button43.Enabled = False
                    Button44.Enabled = False
                    Button45.Enabled = False
                    Button46.Enabled = False
                    Button47.Enabled = False
                    Button48.Enabled = False
                    Button49.Enabled = False
                    Button50.Enabled = False
                    Button51.Enabled = False
                    Button52.Enabled = False
                    Button53.Enabled = False
                    Button54.Enabled = False
                    Button55.Enabled = False
                    Button56.Enabled = False
                    Button57.Enabled = False
                    Button58.Enabled = False
                    MountImageToolStripMenuItem.Enabled = True
                    UnmountImageToolStripMenuItem.Enabled = False
                End If
                DetectVersions(FileVersionInfo.GetVersionInfo(DismExe), imgVersionInfo)
                Exit Sub
            End If
        End If
        If IsImageMounted Then
            Try     ' Try getting image properties
                If Not Directory.Exists(projPath & "\tempinfo") Then
                    Directory.CreateDirectory(projPath & "\tempinfo").Attributes = FileAttributes.Hidden
                End If
                Select Case DismVersionChecker.ProductMajorPart
                    Case 6
                        Select Case DismVersionChecker.ProductMinorPart
                            Case 1
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                                  "@echo off" & CrLf & _
                                                  "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Mount Dir" & Quote & " /b > " & projPath & "\tempinfo\mountdir" & CrLf & _
                                                  "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Image File" & Quote & " /b > " & projPath & "\tempinfo\imgfile" & CrLf & _
                                                  "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Image Index" & Quote & " /b > " & projPath & "\tempinfo\imgindex" & CrLf & _
                                                  "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Mounted Read/Write" & Quote & " /b > " & projPath & "\tempinfo\imgrw" & CrLf & _
                                                  "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & projPath & "\tempinfo\imgmountedstatus" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " /b > " & projPath & "\tempinfo\imgmountedname" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Description" & Quote & " /b > " & projPath & "\tempinfo\imgmounteddesc" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Size" & Quote & " /b > " & projPath & "\tempinfo\imgsize" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & projPath & "\tempinfo\imgwimboot" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Architecture" & Quote & " /b > " & projPath & "\tempinfo\imgarch" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Hal" & Quote & " /b > " & projPath & "\tempinfo\imghal" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ServicePack Build" & Quote & " /b > " & projPath & "\tempinfo\imgspbuild" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ServicePack Level" & Quote & " /b > " & projPath & "\tempinfo\imgsplevel" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Edition" & Quote & " /b > " & projPath & "\tempinfo\imgedition" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Installation" & Quote & " /b > " & projPath & "\tempinfo\imginst" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ProductType" & Quote & " /b > " & projPath & "\tempinfo\imgptype" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ProductSuite" & Quote & " /b > " & projPath & "\tempinfo\imgpsuite" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "System Root" & Quote & " /b > " & projPath & "\tempinfo\imgsysroot" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Directories" & Quote & " /b > " & projPath & "\tempinfo\imgdirs" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Files" & Quote & " /b > " & projPath & "\tempinfo\imgfiles" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Created" & Quote & " /b > " & projPath & "\tempinfo\imgcreation" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Modified" & Quote & " /b > " & projPath & "\tempinfo\imgmodification" & CrLf & _
                                                  "dism /English /image=" & Quote & MountDir & Quote & " /get-intl | findstr /c:" & Quote & "Installed language(s):" & Quote & " /b > " & projPath & "\tempinfo\imglangs", ASCII)
                            Case Is >= 2
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                                  "@echo off" & CrLf & _
                                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mount Dir" & Quote & " /b > " & projPath & "\tempinfo\mountdir" & CrLf & _
                                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image File" & Quote & " /b > " & projPath & "\tempinfo\imgfile" & CrLf & _
                                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image Index" & Quote & " /b > " & projPath & "\tempinfo\imgindex" & CrLf & _
                                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mounted Read/Write" & Quote & " /b > " & projPath & "\tempinfo\imgrw" & CrLf & _
                                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & projPath & "\tempinfo\imgmountedstatus" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " /b > " & projPath & "\tempinfo\imgmountedname" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Description" & Quote & " /b > " & projPath & "\tempinfo\imgmounteddesc" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Size" & Quote & " /b > " & projPath & "\tempinfo\imgsize" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & projPath & "\tempinfo\imgwimboot" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Architecture" & Quote & " /b > " & projPath & "\tempinfo\imgarch" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Hal" & Quote & " /b > " & projPath & "\tempinfo\imghal" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ServicePack Build" & Quote & " /b > " & projPath & "\tempinfo\imgspbuild" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ServicePack Level" & Quote & " /b > " & projPath & "\tempinfo\imgsplevel" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Edition" & Quote & " /b > " & projPath & "\tempinfo\imgedition" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Installation" & Quote & " /b > " & projPath & "\tempinfo\imginst" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ProductType" & Quote & " /b > " & projPath & "\tempinfo\imgptype" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ProductSuite" & Quote & " /b > " & projPath & "\tempinfo\imgpsuite" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "System Root" & Quote & " /b > " & projPath & "\tempinfo\imgsysroot" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Directories" & Quote & " /b > " & projPath & "\tempinfo\imgdirs" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Files" & Quote & " /b > " & projPath & "\tempinfo\imgfiles" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Created" & Quote & " /b > " & projPath & "\tempinfo\imgcreation" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Modified" & Quote & " /b > " & projPath & "\tempinfo\imgmodification" & CrLf & _
                                                  "dism /English /image=" & Quote & MountDir & Quote & " /get-intl | findstr /c:" & Quote & "Installed language(s):" & Quote & " /b > " & projPath & "\tempinfo\imglangs", ASCII)
                        End Select
                    Case 10
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                          "@echo off" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mount Dir" & Quote & " /b > " & projPath & "\tempinfo\mountdir" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image File" & Quote & " /b > " & projPath & "\tempinfo\imgfile" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image Index" & Quote & " /b > " & projPath & "\tempinfo\imgindex" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mounted Read/Write" & Quote & " /b > " & projPath & "\tempinfo\imgrw" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & projPath & "\tempinfo\imgmountedstatus" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " /b > " & projPath & "\tempinfo\imgmountedname" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Description" & Quote & " /b > " & projPath & "\tempinfo\imgmounteddesc" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Size" & Quote & " /b > " & projPath & "\tempinfo\imgsize" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & projPath & "\tempinfo\imgwimboot" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Architecture" & Quote & " /b > " & projPath & "\tempinfo\imgarch" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Hal" & Quote & " /b > " & projPath & "\tempinfo\imghal" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ServicePack Build" & Quote & " /b > " & projPath & "\tempinfo\imgspbuild" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ServicePack Level" & Quote & " /b > " & projPath & "\tempinfo\imgsplevel" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Edition" & Quote & " /b > " & projPath & "\tempinfo\imgedition" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Installation" & Quote & " /b > " & projPath & "\tempinfo\imginst" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ProductType" & Quote & " /b > " & projPath & "\tempinfo\imgptype" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ProductSuite" & Quote & " /b > " & projPath & "\tempinfo\imgpsuite" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "System Root" & Quote & " /b > " & projPath & "\tempinfo\imgsysroot" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Directories" & Quote & " /b > " & projPath & "\tempinfo\imgdirs" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Files" & Quote & " /b > " & projPath & "\tempinfo\imgfiles" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Created" & Quote & " /b > " & projPath & "\tempinfo\imgcreation" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & Quote & SourceImg & Quote & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Modified" & Quote & " /b > " & projPath & "\tempinfo\imgmodification" & CrLf & _
                                          "dism /English /image=" & Quote & MountDir & Quote & " /get-intl | findstr /c:" & Quote & "Installed language(s):" & Quote & " /b > " & projPath & "\tempinfo\imglangs", ASCII)
                End Select

                If Debugger.IsAttached Then
                    Process.Start("\Windows\system32\notepad.exe", Application.StartupPath & "\bin\exthelpers\imginfo.bat").WaitForExit()
                End If
                Process.Start(Application.StartupPath & "\bin\exthelpers\imginfo.bat").WaitForExit()
                'imgName = SourceImg
                'ImgIndex = ImgIndex
                'imgMountDir = MountDir
                imgMountedStatus = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgmountedstatus", ASCII).Replace("Status : ", "").Trim()
                Try
                    Dim KeVerInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(MountDir & "\Windows\system32\ntoskrnl.exe")
                    Dim KeVerStr As String = KeVerInfo.ProductVersion
                    imgVersion = KeVerStr
                    imgMountedName = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgmountedname", ASCII).Replace("Name : ", "").Trim()
                    imgMountedDesc = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgmounteddesc", ASCII).Replace("Description : ", "").Trim()
                    Dim ImgSizeDbl As Double
                    ImgSizeDbl = CDbl(My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgsize", ASCII).Replace("Size : ", "").Trim().Replace(" bytes", "").Trim().Replace(".", "").Trim()) / (1024 ^ 3)
                    Dim ImgSizeStr As String
                    ImgSizeStr = Math.Round(ImgSizeDbl, 2)
                    imgSize = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgsize", ASCII).Replace("Size : ", "").Trim()
                    imgWimBootStatus = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgwimboot", ASCII).Replace("WIM Bootable : ", "").Trim()
                    imgArch = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgarch", ASCII).Replace("Architecture : ", "").Trim()
                    imgHal = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imghal", ASCII).Replace("Hal : ", "").Trim()
                    imgSPBuild = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgspbuild", ASCII).Replace("ServicePack Build : ", "").Trim()
                    imgSPLvl = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgsplevel", ASCII).Replace("ServicePack Level : ", "").Trim()
                    imgEdition = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgedition", ASCII).Replace("Edition : ", "").Trim()
                    imgPType = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgptype", ASCII).Replace("ProductType : ", "").Trim()
                    imgPSuite = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgpsuite", ASCII).Replace("ProductSuite : ", "").Trim()
                    imgSysRoot = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgsysroot", ASCII).Replace("System Root : ", "").Trim()
                    imgDirs = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgdirs", ASCII).Replace("Directories : ", "").Trim()
                    imgFiles = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgfiles", ASCII).Replace("Files : ", "").Trim()
                    imgCreation = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgcreation", ASCII).Replace("Created : ", "").Trim()
                    imgModification = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgmodification", ASCII).Replace("Modified : ", "").Trim()
                    imgLangs = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imglangs", ASCII).Replace("Installed language(s): ", "").Trim()
                    imgFormat = Path.GetExtension(SourceImg).Replace(".", "").Trim().ToUpper() & " file"
                    imgRW = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgrw", ASCII).Replace("Mounted Read/Write : ", "").Trim()
                    'If imgRW = "Yes" Then
                    '    RWRemountBtn.Visible = False
                    'ElseIf imgRW = "No" Then
                    '    RWRemountBtn.Visible = True
                    'End If
                    For Each foundFile In My.Computer.FileSystem.GetFiles(projPath & "\tempinfo", FileIO.SearchOption.SearchTopLevelOnly)
                        File.Delete(foundFile)
                    Next
                    Directory.Delete(projPath & "\tempinfo")
                    File.Delete(Application.StartupPath & "\bin\exthelpers\imginfo.bat")
                Catch ex As Exception

                End Try
                imgVersion = imgVersion
                imgMountedStatus = imgMountedStatus
                imgMountedName = imgMountedName
                imgMountedDesc = imgMountedDesc
                imgWimBootStatus = imgWimBootStatus
                imgArch = imgArch
                imgHal = imgHal
                imgSPBuild = imgSPBuild
                imgSPLvl = imgSPLvl
                imgEdition = imgEdition
                imgPType = imgPType
                imgPSuite = imgPSuite
                imgSysRoot = imgSysRoot
                imgDirs = CInt(imgDirs)
                imgFiles = CInt(imgFiles)
                imgCreation = imgCreation
                CreationTime = imgCreation.Replace(" - ", " ")
                imgModification = imgModification
                ModifyTime = imgModification.Replace(" - ", " ")
                'imgLangs = imgLangText
                imgRW = imgRW
                Button1.Enabled = False
                Button2.Enabled = True
                Button3.Enabled = True
                Button4.Enabled = True
                Button5.Enabled = True
                Button6.Enabled = True
                Button7.Enabled = True
                Button8.Enabled = True
                Button9.Enabled = True
                Button10.Enabled = True
                Button11.Enabled = True
                Button12.Enabled = True
                Button13.Enabled = True
                ' Update the buttons in the new design accordingly
                Button26.Enabled = False
                Button27.Enabled = True
                Button28.Enabled = True
                Button29.Enabled = True
                Button24.Enabled = True
                Button25.Enabled = True
                Button30.Enabled = True
                Button31.Enabled = True
                Button32.Enabled = True
                Button33.Enabled = True
                Button34.Enabled = True
                Button35.Enabled = True
                Button36.Enabled = True
                Button37.Enabled = True
                Button38.Enabled = True
                Button39.Enabled = True
                Button40.Enabled = True
                Button41.Enabled = True
                Button42.Enabled = True
                Button43.Enabled = True
                Button44.Enabled = True
                Button45.Enabled = True
                Button46.Enabled = True
                Button47.Enabled = True
                Button48.Enabled = True
                Button49.Enabled = True
                Button50.Enabled = True
                Button51.Enabled = True
                Button52.Enabled = True
                Button53.Enabled = True
                Button54.Enabled = True
                Button55.Enabled = True
                Button56.Enabled = True
                Button57.Enabled = True
                Button58.Enabled = True
                DetectNTVersion(MountDir & "\Windows\system32\ntoskrnl.exe")
            Catch ex As Exception

            End Try
            irregVal = 10
            'ImgBW.ReportProgress(irregVal)
            'Label4.Visible = False
        Else
            MountDir = "N/A"
            'Label19 = "No"
            'imgMountDir = "Not available"
            'ImgIndex = "Not available"
            'imgName = "Not available"
            'imgMountedStatus = "Not available"
            'imgVersion = "Not available"
            'imgMountedName = "Not available"
            'imgMountedDesc = "Not available"
            'imgSize = "Not available"
            'imgWimBootStatus = "Not available"
            'imgArch = "Not available"
            'imgHal = "Not available"
            'imgSPBuild = "Not available"
            'imgSPLvl = "Not available"
            'imgEdition = "Not available"
            'imgPType = "Not available"
            'imgPSuite = "Not available"
            'imgSysRoot = "Not available"
            'imgDirs = "Not available"
            'imgFiles = "Not available"
            'imgCreation = "Not available"
            'imgModification = "Not available"
            'imgFormat = "Not available"
            'imgRW = "Not available"
            'Panel3.Visible = True
            'Label4.Visible = False
            Button1.Enabled = True
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            Button5.Enabled = False
            Button6.Enabled = False
            Button7.Enabled = False
            Button8.Enabled = False
            Button9.Enabled = False
            Button10.Enabled = False
            Button11.Enabled = False
            Button12.Enabled = False
            Button13.Enabled = False
            ' Update the buttons in the new design accordingly
            Button26.Enabled = True
            Button27.Enabled = False
            Button28.Enabled = False
            Button29.Enabled = False
            Button24.Enabled = False
            Button25.Enabled = False
            Button30.Enabled = False
            Button31.Enabled = False
            Button32.Enabled = False
            Button33.Enabled = False
            Button34.Enabled = False
            Button35.Enabled = False
            Button36.Enabled = False
            Button37.Enabled = False
            Button38.Enabled = False
            Button39.Enabled = False
            Button40.Enabled = False
            Button41.Enabled = False
            Button42.Enabled = False
            Button43.Enabled = False
            Button44.Enabled = False
            Button45.Enabled = False
            Button46.Enabled = False
            Button47.Enabled = False
            Button48.Enabled = False
            Button49.Enabled = False
            Button50.Enabled = False
            Button51.Enabled = False
            Button52.Enabled = False
            Button53.Enabled = False
            Button54.Enabled = False
            Button55.Enabled = False
            Button56.Enabled = False
            Button57.Enabled = False
            Button58.Enabled = False
        End If
    End Sub

    Sub DetectVersions(DismVer As FileVersionInfo, NTVer As Version)
        ' Restore enabled properties of each menu item and group in the new design
        For Each Item As ToolStripDropDownItem In CommandsToolStripMenuItem.DropDownItems
            Item.Enabled = True
            Try
                For Each DropDownItem As ToolStripDropDownItem In Item.DropDownItems
                    DropDownItem.Enabled = True
                Next
            Catch ex As Exception
                Continue For
            End Try
        Next
        GroupBox7.Enabled = True    ' AppX package group
        GroupBox8.Enabled = True    ' Capability group
        GroupBox10.Enabled = True   ' Windows PE settings group

        ' Detect if an image has been mounted, and act accordingly
        If IsImageMounted Then
            ' Now, detect the Windows version
            Select Case NTVer.Major
                Case 6
                    Select Case NTVer.Minor
                        Case 1
                            ' All AppX and capability stuff goes away
                            AppPackagesToolStripMenuItem.Enabled = False
                            CapabilitiesToolStripMenuItem.Enabled = False
                            GroupBox7.Enabled = False
                            GroupBox8.Enabled = False

                            ' WIMBoot also goes away
                            GetWIMBootEntry.Enabled = False
                            UpdateWIMBootEntry.Enabled = False

                            ' Microsoft Edge stuff, you know what I mean...
                            MicrosoftEdgeToolStripMenuItem.Enabled = False

                            ' Disable other stuff
                            ExportDriver.Enabled = False
                            ReservedStorageToolStripMenuItem.Enabled = False
                            SetSysUILang.Enabled = False
                            ProvisioningPackagesToolStripMenuItem.Enabled = False
                            OSUninstallToolStripMenuItem.Enabled = False
                        Case 2
                            Select Case NTVer.Build
                                Case Is >= 8102
                                    CapabilitiesToolStripMenuItem.Enabled = False
                                    GroupBox8.Enabled = False
                                    GetWIMBootEntry.Enabled = False
                                    UpdateWIMBootEntry.Enabled = False
                                    MicrosoftEdgeToolStripMenuItem.Enabled = False
                                    ReservedStorageToolStripMenuItem.Enabled = False
                                    SetSysUILang.Enabled = False
                                    ProvisioningPackagesToolStripMenuItem.Enabled = False
                                    OSUninstallToolStripMenuItem.Enabled = False
                                Case Else
                                    AppPackagesToolStripMenuItem.Enabled = False
                                    CapabilitiesToolStripMenuItem.Enabled = False
                                    GroupBox7.Enabled = False
                                    GroupBox8.Enabled = False
                                    GetWIMBootEntry.Enabled = False
                                    UpdateWIMBootEntry.Enabled = False
                                    MicrosoftEdgeToolStripMenuItem.Enabled = False
                                    ReservedStorageToolStripMenuItem.Enabled = False
                                    SetSysUILang.Enabled = False
                                    ProvisioningPackagesToolStripMenuItem.Enabled = False
                                    OSUninstallToolStripMenuItem.Enabled = False
                            End Select
                        Case 3
                            CapabilitiesToolStripMenuItem.Enabled = False
                            GroupBox8.Enabled = False
                            MicrosoftEdgeToolStripMenuItem.Enabled = False
                            ReservedStorageToolStripMenuItem.Enabled = False
                            SetSysUILang.Enabled = False
                            ProvisioningPackagesToolStripMenuItem.Enabled = False
                            OSUninstallToolStripMenuItem.Enabled = False
                    End Select
                Case 10
                    Select Case NTVer.Build
                        Case Is < 21996
                            ' Microsoft Edge stuff only affects Windows 11
                            MicrosoftEdgeToolStripMenuItem.Enabled = False
                    End Select
            End Select

            ' Disable Windows PE stuff when not working with a Windows PE image
            WindowsPEServicingToolStripMenuItem.Enabled = imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase)
            GroupBox10.Enabled = imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase)
            ' Disable AppX and capability stuff when working with a Windows PE image
            AppPackagesToolStripMenuItem.Enabled = (Not imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) And IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe"))
            CapabilitiesToolStripMenuItem.Enabled = (Not imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) And IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe"))
            GroupBox7.Enabled = (Not imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) And IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe"))
            GroupBox8.Enabled = (Not imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) And IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe"))

            ' Next, detect the DISM version, so that we can determine which things are applicable
            Select Case DismVer.ProductMajorPart
                Case 6
                    Select Case DismVer.ProductMinorPart
                        Case 1
                            AppendImage.Enabled = False
                            ApplyFFU.Enabled = False
                            ApplyImage.Enabled = False
                            CaptureCustomImage.Enabled = False
                            CaptureFFU.Enabled = False
                            CaptureImage.Enabled = False
                            CleanupMountpoints.Enabled = False
                            DeleteImage.Enabled = False
                            ExportImage.Enabled = False
                            GetWIMBootEntry.Enabled = False
                            ListImage.Enabled = False
                            OptimizeFFU.Enabled = False
                            OptimizeImage.Enabled = False
                            SplitFFU.Enabled = False
                            SplitImage.Enabled = False
                            UpdateWIMBootEntry.Enabled = False
                            ApplySiloedPackage.Enabled = False
                            ProvisioningPackagesToolStripMenuItem.Enabled = False
                            AddProvisionedAppxPackage.Enabled = False
                            RemoveProvisionedAppxPackage.Enabled = False
                            OptimizeProvisionedAppxPackages.Enabled = False
                            SetProvisionedAppxDataFile.Enabled = False
                            ExportDefaultAppAssociations.Enabled = False
                            GetDefaultAppAssociations.Enabled = False
                            ImportDefaultAppAssociations.Enabled = False
                            RemoveDefaultAppAssociations.Enabled = False
                            AddCapability.Enabled = False
                            ExportSource.Enabled = False
                            RemoveCapability.Enabled = False
                            ExportDriver.Enabled = False
                            GetOSUninstallWindow.Enabled = False
                            InitiateOSUninstall.Enabled = False
                            RemoveOSUninstall.Enabled = False
                            SetOSUninstallWindow.Enabled = False
                            ReservedStorageToolStripMenuItem.Enabled = False
                            MicrosoftEdgeToolStripMenuItem.Enabled = False
                            SetSysUILang.Enabled = False
                            GroupBox7.Enabled = False
                            GroupBox8.Enabled = False
                        Case 2
                            CaptureFFU.Enabled = False
                            GetWIMBootEntry.Enabled = False
                            OptimizeFFU.Enabled = False
                            OptimizeImage.Enabled = False
                            SplitFFU.Enabled = False
                            UpdateWIMBootEntry.Enabled = False
                            ApplySiloedPackage.Enabled = False
                            ProvisioningPackagesToolStripMenuItem.Enabled = False
                            OptimizeProvisionedAppxPackages.Enabled = False
                            AddCapability.Enabled = False
                            ExportSource.Enabled = False
                            RemoveCapability.Enabled = False
                            GetOSUninstallWindow.Enabled = False
                            InitiateOSUninstall.Enabled = False
                            RemoveOSUninstall.Enabled = False
                            SetOSUninstallWindow.Enabled = False
                            ReservedStorageToolStripMenuItem.Enabled = False
                            MicrosoftEdgeToolStripMenuItem.Enabled = False
                            SetSysUILang.Enabled = False
                            GroupBox8.Enabled = False
                        Case 3
                            CaptureFFU.Enabled = False
                            OptimizeFFU.Enabled = False
                            OptimizeImage.Enabled = False
                            SplitFFU.Enabled = False
                            ApplySiloedPackage.Enabled = False
                            ProvisioningPackagesToolStripMenuItem.Enabled = False
                            OptimizeProvisionedAppxPackages.Enabled = False
                            AddCapability.Enabled = False
                            ExportSource.Enabled = False
                            RemoveCapability.Enabled = False
                            GetOSUninstallWindow.Enabled = False
                            InitiateOSUninstall.Enabled = False
                            RemoveOSUninstall.Enabled = False
                            SetOSUninstallWindow.Enabled = False
                            ReservedStorageToolStripMenuItem.Enabled = False
                            MicrosoftEdgeToolStripMenuItem.Enabled = False
                            SetSysUILang.Enabled = False
                            GroupBox8.Enabled = False
                    End Select
                Case 10
                    ' Everything is enabled
            End Select
        Else

        End If
    End Sub

    ''' <summary>
    ''' Detects the image's version by gathering the product version from its "ntoskrnl.exe" file in MountDir\Windows\System32
    ''' </summary>
    ''' <param name="NTKeExe">The path of the "NT Kernel &amp; System" (ntoskrnl.exe) file</param>
    ''' <remarks>The program, depending on the NT version of the image, will enable and disable certain features. This happens because the image may not be applicable for such actions. Also, it is called to detect whether a Windows Vista/Server 2008 image has been mounted; as DISM can mount Vista (NT 6.0) images, but cannot service them. In that case, you would need to use separate tools, like ImageX</remarks>
    Sub DetectNTVersion(NTKeExe As String)
        Try
            Dim NTKeVerInfo As FileVersionInfo
            NTKeVerInfo = FileVersionInfo.GetVersionInfo(NTKeExe)
            If NTKeVerInfo.ProductMajorPart >= 6 And NTKeVerInfo.ProductBuildPart >= 6000 Then
                If NTKeVerInfo.ProductMajorPart = 6 And NTKeVerInfo.ProductMinorPart = 0 Then
                    IsCompatible = False
                Else
                    IsCompatible = True
                End If
            Else
                IsCompatible = False
            End If
        Catch ex As Exception
            If IsImageMounted Then IsCompatible = False
        End Try
        Button1.Enabled = False
        'Button2.Enabled = True
        'Button3.Enabled = True
        'Button4.Enabled = True
        Button5.Enabled = True
        Button6.Enabled = True
        Button7.Enabled = True
        Button8.Enabled = True
        Button9.Enabled = True
        Button10.Enabled = True
        Button11.Enabled = True
        Button12.Enabled = True
        Button13.Enabled = True
        ' Update the buttons in the new design accordingly
        Button26.Enabled = False
        'Button27.Enabled = True
        'Button28.Enabled = True
        'Button29.Enabled = True
        Button24.Enabled = True
        Button25.Enabled = True
        Button30.Enabled = True
        Button31.Enabled = True
        Button32.Enabled = True
        Button33.Enabled = True
        Button34.Enabled = True
        Button35.Enabled = True
        Button36.Enabled = True
        Button37.Enabled = True
        Button38.Enabled = True
        Button39.Enabled = True
        Button40.Enabled = True
        Button41.Enabled = True
        Button42.Enabled = True
        Button43.Enabled = True
        Button44.Enabled = True
        Button45.Enabled = True
        Button46.Enabled = True
        Button47.Enabled = True
        Button48.Enabled = True
        Button49.Enabled = True
        Button50.Enabled = True
        Button51.Enabled = True
        Button52.Enabled = True
        Button53.Enabled = True
        Button54.Enabled = True
        Button55.Enabled = True
        Button56.Enabled = True
        Button57.Enabled = True
        Button58.Enabled = True
    End Sub

    ''' <summary>
    ''' Determines whether the loaded image is running a Windows version newer than Windows 8
    ''' </summary>
    ''' <param name="NTKeExe">The path of the "NT Kernel &amp; System" file</param>
    ''' <returns>The function returns True when image is running Windows 8 or newer (Image >= 6.2)</returns>
    ''' <remarks>This is done to determine whether to scan for AppX packages</remarks>
    Function IsWindows8OrHigher(NTKeExe As String) As Boolean
        Debug.WriteLine("[IsWindows8OrHigher] Running function...")
        Dim KeFVI As FileVersionInfo = FileVersionInfo.GetVersionInfo(NTKeExe)
        Select Case KeFVI.ProductMajorPart
            Case 6
                Select Case KeFVI.ProductMinorPart
                    Case 1
                        Debug.WriteLine("[IsWindows8OrHigher] 6.1 >= 6.2 -> False" & CrLf & _
                                        "                     Image is running a Windows version older than Windows 8")
                        Return False
                    Case Is >= 2
                        Debug.WriteLine("[IsWindows8OrHigher] 6.2 >= 6.2 -> True")
                        Select Case KeFVI.ProductBuildPart
                            Case Is >= 8102
                                Debug.WriteLine("                     Image is running Windows Developer Preview or later")
                                Return True
                            Case Else
                                Debug.WriteLine("                     Image is not running Windows Developer Preview or later")
                                Return False
                        End Select
                End Select
            Case 10
                Debug.WriteLine("[IsWindows8OrHigher] " & KeFVI.ProductMajorPart & "." & KeFVI.ProductMinorPart & " >= 6.2 -> True" & CrLf & _
                                "                     Image is running a Windows version newer than Windows 8")
                Return True
            Case Else
                Return False
        End Select
        Return False
    End Function

    ''' <summary>
    ''' Determines whether the loaded image is running a Windows version newer than Windows 10
    ''' </summary>
    ''' <param name="NTKeExe">The path of the "NT Kernel &amp; System" file</param>
    ''' <returns>The function returns True when image is running Windows 10 or newer (Image = 10.0)</returns>
    ''' <remarks></remarks>
    Function IsWindows10OrHigher(NTKeExe As String) As Boolean
        Dim KeFVI As FileVersionInfo = FileVersionInfo.GetVersionInfo(NTKeExe)
        Select Case KeFVI.ProductMajorPart
            Case Is <= 6
                Debug.WriteLine("[IsWindows10OrHigher] 6.x == 10.0 => False" & CrLf & _
                                "                      Image is running a Windows version older than Windows 10")
                Return False
            Case 10
                Debug.WriteLine("[IsWindows10OrHigher] 10.0 == 10.0 => True" & CrLf & _
                                "                      Image is running Windows 10 or Windows 11")
                Return True
        End Select
        Return False
    End Function

    ''' <summary>
    ''' Gets installed packages in an image and puts them in separate arrays
    ''' </summary>
    Sub GetImagePackages(Optional UseApi As Boolean = False, Optional OnlineMode As Boolean = False)
        If UseApi Then
            Try
                DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
                Using session As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(sessionMntDir))
                    Dim imgPackageNameList As New List(Of String)
                    Dim imgPackageStateList As New List(Of String)
                    Dim imgPackageRelTypeList As New List(Of String)
                    Dim imgPackageInstTimeList As New List(Of String)
                    Dim PackageCollection As DismPackageCollection = DismApi.GetPackages(session)
                    PackageInfoList = PackageCollection
                    For Each package As DismPackage In PackageCollection
                        If ImgBW.CancellationPending Then
                            If UseApi And session IsNot Nothing Then DismApi.CloseSession(session)
                            CompletedTasks(0) = False
                            PendingTasks(0) = True
                            Exit Sub
                        End If
                        imgPackageNameList.Add(package.PackageName)
                        imgPackageStateList.Add(package.PackageState)
                        imgPackageRelTypeList.Add(package.ReleaseType)
                        imgPackageInstTimeList.Add(package.InstallTime.ToString())
                    Next
                    imgPackageNames = imgPackageNameList.ToArray()
                    imgPackageState = imgPackageStateList.ToArray()
                    imgPackageRelType = imgPackageRelTypeList.ToArray()
                    imgPackageInstTime = imgPackageInstTimeList.ToArray()
                End Using
            Finally
                DismApi.Shutdown()
            End Try
            CompletedTasks(0) = True
            PendingTasks(0) = False
            Exit Sub
            'Try

            '    'If session IsNot Nothing Then
            '    '    Dim imgPackageNameList As New List(Of String)
            '    '    Dim imgPackageStateList As New List(Of String)
            '    '    Dim imgPackageRelTypeList As New List(Of String)
            '    '    Dim imgPackageInstTimeList As New List(Of String)
            '    '    Dim PackageCollection As DismPackageCollection = DismApi.GetPackages(session)
            '    '    For Each package As DismPackage In PackageCollection
            '    '        If ImgBW.CancellationPending Then
            '    '            If UseApi And session IsNot Nothing Then DismApi.CloseSession(session)
            '    '            Exit Sub
            '    '        End If
            '    '        imgPackageNameList.Add(package.PackageName)
            '    '        imgPackageStateList.Add(package.PackageState)
            '    '        imgPackageRelTypeList.Add(package.ReleaseType)
            '    '        imgPackageInstTimeList.Add(package.InstallTime.ToString())
            '    '    Next
            '    '    imgPackageNames = imgPackageNameList.ToArray()
            '    '    imgPackageState = imgPackageStateList.ToArray()
            '    '    imgPackageRelType = imgPackageRelTypeList.ToArray()
            '    '    imgPackageInstTime = imgPackageInstTimeList.ToArray()
            '    '    Exit Sub
            '    'Else
            '    '    Throw New Exception("No valid DISM sesion has been provided")
            '    'End If
            'Catch ex As Exception
            '    DismApi.CloseSession(session)
            '    ' Run backup function
            '    Exit Try
            'End Try
        End If
        Debug.WriteLine("[GetImagePackages] Running function...")
        Debug.WriteLine("[GetImagePackages] Writing getter scripts...")
        Try
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\pkgnames.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-packages | findstr /c:" & Quote & "Package Identity : " & Quote & " > .\tempinfo\pkgnames", _
                              ASCII)
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\pkgstate.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-packages | findstr /c:" & Quote & "State : " & Quote & " > .\tempinfo\pkgstate", _
                              ASCII)
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\pkgreltype.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-packages | findstr /c:" & Quote & "Release Type : " & Quote & " > .\tempinfo\pkgreltype", _
                              ASCII)
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\pkginsttime.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-packages | findstr /c:" & Quote & "Install Time : " & Quote & " > .\tempinfo\pkginsttime", _
                              ASCII)
        Catch ex As Exception
            Debug.WriteLine("[GetImagePackages] Failed writing getter scripts. Reason: " & ex.Message)
            CompletedTasks(0) = False
            Exit Sub
        End Try
        Debug.WriteLine("[GetImagePackages] Finished writing getter scripts. Executing them...")
        ImgProcesses.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
        ImgProcesses.StartInfo.CreateNoWindow = True
        ImgProcesses.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        For Each pkgScript In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\bin\exthelpers", FileIO.SearchOption.SearchTopLevelOnly, "*.bat")
            If Path.GetFileName(pkgScript).StartsWith("pkg") Then
                Debug.WriteLine("[GetImagePackages] RunCommand -> " & Path.GetFileName(pkgScript))
                ImgProcesses.StartInfo.Arguments = "/c " & pkgScript
                ImgProcesses.Start()
                ImgProcesses.WaitForExit()
                If ImgProcesses.ExitCode = 0 Then
                    Continue For
                End If
            Else
                Continue For
            End If
        Next
        Debug.WriteLine("[GetImagePackages] Finished running getter scripts. Filling arrays...")
        Dim FileGetterRTB As New RichTextBox()
        Dim TypeLookups() As String = New String(3) {"Package Identity : ", "State : ", "Release Type : ", "Install Time : "}
        Dim lineToAppend As String = ""
        For Each pkgFile In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\tempinfo", FileIO.SearchOption.SearchTopLevelOnly)
            If Path.GetFileName(pkgFile).StartsWith("pkg") Then
                Debug.WriteLine("[GetImagePackages] FillArray -> (values_from: " & Path.GetFileName(pkgFile) & ")")
                FileGetterRTB.Clear()
                FileGetterRTB.Text = My.Computer.FileSystem.ReadAllText(pkgFile)
                For x = 0 To FileGetterRTB.Lines.Count - 1
                    If FileGetterRTB.Lines(x) = "" Then
                        Continue For
                    Else
                        If FileGetterRTB.Lines(x).StartsWith(TypeLookups(0)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("Package Identity : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgPackageNames(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(1)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("State : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgPackageState(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(2)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("Release Type : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgPackageRelType(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(3)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("Install Time : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgPackageInstTime(x) = lineToAppend
                        End If
                    End If
                Next
            Else
                Continue For
            End If
        Next
        CompletedTasks(0) = True
        PendingTasks(0) = False
        'imgPackageNameLastEntry = UBound(imgPackageNames)
        'ImgBW.ReportProgress(progressMin + progressDivs)
    End Sub

    ''' <summary>
    ''' Gets present features in an image and puts them in separate arrays
    ''' </summary>
    Sub GetImageFeatures(Optional UseApi As Boolean = False, Optional OnlineMode As Boolean = False)
        If UseApi Then
            Try
                DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
                Using session As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(sessionMntDir))
                    Dim imgFeatureNameList As New List(Of String)
                    Dim imgFeatureStateList As New List(Of String)
                    Dim FeatureCollection As DismFeatureCollection = DismApi.GetFeatures(session)
                    FeatureInfoList = FeatureCollection
                    For Each feature As DismFeature In FeatureCollection
                        If ImgBW.CancellationPending Then
                            If UseApi And session IsNot Nothing Then DismApi.CloseSession(session)
                            CompletedTasks(1) = False
                            PendingTasks(1) = True
                            Exit Sub
                        End If
                        imgFeatureNameList.Add(feature.FeatureName)
                        Select Case feature.State
                            Case DismPackageFeatureState.NotPresent
                                imgFeatureStateList.Add("Not present")
                            Case DismPackageFeatureState.UninstallPending
                                imgFeatureStateList.Add("Disable Pending")
                            Case DismPackageFeatureState.Staged
                                imgFeatureStateList.Add("Disabled")
                            Case DismPackageFeatureState.Removed Or DismPackageFeatureState.Resolved
                                imgFeatureStateList.Add("Removed")
                            Case DismPackageFeatureState.Installed
                                imgFeatureStateList.Add("Enabled")
                            Case DismPackageFeatureState.InstallPending
                                imgFeatureStateList.Add("Enable Pending")
                            Case DismPackageFeatureState.Superseded
                                imgFeatureStateList.Add("Superseded")
                            Case DismPackageFeatureState.PartiallyInstalled
                                imgFeatureStateList.Add("Partially Installed")
                        End Select
                    Next
                    imgFeatureNames = imgFeatureNameList.ToArray()
                    imgFeatureState = imgFeatureStateList.ToArray()
                End Using
            Finally
                DismApi.Shutdown()
            End Try
            CompletedTasks(1) = True
            PendingTasks(1) = False
            Exit Sub
            'Try
            '    If session IsNot Nothing Then

            '        Exit Sub
            '    Else
            '        Throw New Exception("No valid DISM session has been provided")
            '    End If
            'Catch ex As Exception
            '    DismApi.CloseSession(session)
            '    Exit Try
            'End Try
        End If
        Debug.WriteLine("[GetImageFeatures] Running function...")
        Debug.WriteLine("[GetImageFeatures] Writing getter scripts...")
        Try
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\featnames.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-features | findstr /c:" & Quote & "Feature Name : " & Quote & " > .\tempinfo\featnames", _
                              ASCII)
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\featstate.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-features | findstr /c:" & Quote & "State : " & Quote & " > .\tempinfo\featstate", _
                              ASCII)
        Catch ex As Exception
            Debug.WriteLine("[GetImageFeatures] Failed writing getter scripts. Reason: " & ex.Message)
            CompletedTasks(1) = False
            Exit Sub
        End Try
        Debug.WriteLine("[GetImageFeatures] Finished writing getter scripts. Executing them...")
        ImgProcesses.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
        ImgProcesses.StartInfo.CreateNoWindow = True
        ImgProcesses.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        For Each featScript In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\bin\exthelpers", FileIO.SearchOption.SearchTopLevelOnly, "*.bat")
            If Path.GetFileName(featScript).StartsWith("feat") Then
                Debug.WriteLine("[GetImageFeatures] RunCommand -> " & Path.GetFileName(featScript))
                ImgProcesses.StartInfo.Arguments = "/c " & featScript
                ImgProcesses.Start()
                ImgProcesses.WaitForExit()
                If ImgProcesses.ExitCode = 0 Then
                    Continue For
                End If
            Else
                Continue For
            End If
        Next
        Debug.WriteLine("[GetImageFeatures] Finished running getter scripts. Filling arrays...")
        Dim FileGetterRTB As New RichTextBox()
        Dim TypeLookups() As String = New String(1) {"Feature Name : ", "State : "}
        Dim lineToAppend As String = ""
        For Each featFile In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\tempinfo", FileIO.SearchOption.SearchTopLevelOnly)
            If Path.GetFileName(featFile).StartsWith("feat") Then
                Debug.WriteLine("[GetImageFeatures] FillArray -> (values_from: " & Path.GetFileName(featFile) & ")")
                FileGetterRTB.Clear()
                FileGetterRTB.Text = My.Computer.FileSystem.ReadAllText(featFile)
                For x = 0 To FileGetterRTB.Lines.Count - 1
                    If FileGetterRTB.Lines(x) = "" Then
                        Continue For
                    Else
                        If FileGetterRTB.Lines(x).StartsWith(TypeLookups(0)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("Feature Name : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgFeatureNames(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(1)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("State : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgFeatureState(x) = lineToAppend
                        Else
                            Continue For
                        End If
                    End If
                Next
            Else
                Continue For
            End If
        Next
        CompletedTasks(1) = True
        PendingTasks(1) = False
        'ImgBW.ReportProgress(progressMin + progressDivs)
    End Sub

    ''' <summary>
    ''' Gets installed provisioned APPX packages in an image and puts them in separate arrays
    ''' </summary>
    ''' <remarks>This is only for Windows 8 and newer</remarks>
    Sub GetImageAppxPackages(Optional UseApi As Boolean = False, Optional OnlineMode As Boolean = False)
        If UseApi And Environment.OSVersion.Version.Major > 6 Then
            Try
                DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
                Using session As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(sessionMntDir))
                    Dim imgAppxDisplayNameList As New List(Of String)
                    Dim imgAppxPackageNameList As New List(Of String)
                    Dim imgAppxVersionList As New List(Of String)
                    Dim imgAppxArchitectureList As New List(Of String)
                    Dim imgAppxResourceIdList As New List(Of String)
                    Dim imgAppxRegionList As New List(Of String)
                    Dim AppxPackageCollection As DismAppxPackageCollection = DismApi.GetProvisionedAppxPackages(session)
                    AppxPackageInfoList = AppxPackageCollection
                    For Each AppxPackage As DismAppxPackage In AppxPackageCollection
                        If ImgBW.CancellationPending Then
                            If UseApi And session IsNot Nothing Then DismApi.CloseSession(session)
                            CompletedTasks(2) = False
                            PendingTasks(2) = True
                            Exit Sub
                        End If
                        Select Case AppxPackage.Architecture
                            Case DismProcessorArchitecture.None
                                imgAppxArchitectureList.Add("Unknown")
                            Case DismProcessorArchitecture.Intel
                                imgAppxArchitectureList.Add("x86")
                            Case DismProcessorArchitecture.ARM
                                imgAppxArchitectureList.Add("ARM")
                            Case DismProcessorArchitecture.IA64
                                imgAppxArchitectureList.Add("IA64")
                            Case DismProcessorArchitecture.AMD64
                                imgAppxArchitectureList.Add("x64")
                            Case DismProcessorArchitecture.Neutral
                                imgAppxArchitectureList.Add("Neutral")
                            Case DismProcessorArchitecture.ARM64
                                imgAppxArchitectureList.Add("ARM64")
                        End Select
                        imgAppxDisplayNameList.Add(AppxPackage.DisplayName)
                        imgAppxPackageNameList.Add(AppxPackage.PackageName)
                        imgAppxResourceIdList.Add(AppxPackage.ResourceId)
                        imgAppxVersionList.Add(AppxPackage.Version.ToString())
                    Next
                    If OnlineMode And ExtAppxGetter Then
                        PSExtAppxGetter()
                        If Directory.Exists(Application.StartupPath & "\bin\extps1\out") And My.Computer.FileSystem.GetFiles(Application.StartupPath & "\bin\extps1\out").Count > 0 Then
                            Dim appxPkgNameRTB As New RichTextBox()
                            Dim appxPkgFullNameRTB As New RichTextBox()
                            Dim appxArchRTB As New RichTextBox()
                            Dim appxResIdRTB As New RichTextBox()
                            Dim appxVerRTB As New RichTextBox()
                            Dim appxNonRemPolRTB As New RichTextBox()
                            Dim appxFrameworkRTB As New RichTextBox()
                            appxPkgNameRTB.Text = File.ReadAllText(Application.StartupPath & "\bin\extps1\out\appxpkgnames")
                            appxPkgFullNameRTB.Text = File.ReadAllText(Application.StartupPath & "\bin\extps1\out\appxpkgfullnames")
                            appxArchRTB.Text = File.ReadAllText(Application.StartupPath & "\bin\extps1\out\appxarch")
                            appxResIdRTB.Text = File.ReadAllText(Application.StartupPath & "\bin\extps1\out\appxresid")
                            appxVerRTB.Text = File.ReadAllText(Application.StartupPath & "\bin\extps1\out\appxver")
                            If File.Exists(Application.StartupPath & "\bin\extps1\out\appxnonrempolicy") Then appxNonRemPolRTB.Text = File.ReadAllText(Application.StartupPath & "\bin\extps1\out\appxnonrempolicy")
                            If File.Exists(Application.StartupPath & "\bin\extps1\out\appxframework") Then appxFrameworkRTB.Text = File.ReadAllText(Application.StartupPath & "\bin\extps1\out\appxframework")
                            For x = 0 To appxPkgFullNameRTB.Lines.Count - 1
                                If imgAppxPackageNameList.Contains(appxPkgFullNameRTB.Lines(x)) Then
                                    Continue For
                                Else
                                    If SkipNonRemovable Or (SkipFrameworks And appxFrameworkRTB.Text <> "") Then
                                        If appxNonRemPolRTB.Lines(x) = "True" Or (SkipFrameworks And appxFrameworkRTB.Lines(x) = "True") Then
                                            Continue For
                                        Else
                                            imgAppxDisplayNameList.Add(appxPkgNameRTB.Lines(x))
                                            imgAppxPackageNameList.Add(appxPkgFullNameRTB.Lines(x))
                                            imgAppxArchitectureList.Add(appxArchRTB.Lines(x))
                                            imgAppxResourceIdList.Add(appxResIdRTB.Lines(x))
                                            imgAppxVersionList.Add(appxVerRTB.Lines(x))
                                        End If
                                    Else
                                        imgAppxDisplayNameList.Add(appxPkgNameRTB.Lines(x))
                                        imgAppxPackageNameList.Add(appxPkgFullNameRTB.Lines(x))
                                        imgAppxArchitectureList.Add(appxArchRTB.Lines(x))
                                        imgAppxResourceIdList.Add(appxResIdRTB.Lines(x))
                                        imgAppxVersionList.Add(appxVerRTB.Lines(x))
                                    End If
                                End If
                            Next
                            Try
                                Directory.Delete(Application.StartupPath & "\bin\extps1\out", True)
                            Catch ex As Exception
                                ' Leave directory for later
                            End Try
                        End If
                    End If
                    imgAppxArchitectures = imgAppxArchitectureList.ToArray()
                    imgAppxDisplayNames = imgAppxDisplayNameList.ToArray()
                    imgAppxPackageNames = imgAppxPackageNameList.ToArray()
                    imgAppxResourceIds = imgAppxResourceIdList.ToArray()
                    imgAppxVersions = imgAppxVersionList.ToArray()
                End Using
            Finally
                DismApi.Shutdown()
            End Try
            CompletedTasks(2) = True
            PendingTasks(2) = False
            Exit Sub
        End If
        Debug.WriteLine("[GetImageAppxPackages] Running function...")
        ' The mounted image may be Windows 8 or later, but DISM may be from Windows 7. Get this information before running this procedure
        Dim FileVersion As FileVersionInfo = FileVersionInfo.GetVersionInfo(DismExe)
        Select Case FileVersion.ProductMajorPart
            Case 6
                ' Detect if it is Windows 7
                Select Case FileVersion.ProductMinorPart
                    Case 1
                        Debug.WriteLine("[GetImageAppxPackages] The image is Windows 8 or later, but this version of DISM does not support this command. Exiting...")
                        CompletedTasks(2) = False
                        PendingTasks(2) = True
                        Exit Sub
                End Select
        End Select
        Debug.WriteLine("[GetImageAppxPackages] Writing getter scripts...")
        Try
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\appxnames.bat", _
                              "@echo off" & CrLf & _
                              "dism /English " & If(OnlineMode, " /online", " /image=" & Quote & MountDir & Quote) & " /get-provisionedappxpackages | findstr /c:" & Quote & "DisplayName : " & Quote & " > .\tempinfo\appxdisplaynames" & CrLf & _
                              "dism /English " & If(OnlineMode, " /online", " /image=" & Quote & MountDir & Quote) & " /get-provisionedappxpackages | findstr /c:" & Quote & "PackageName : " & Quote & " > .\tempinfo\appxpackagenames", _
                              ASCII)
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\appxversions.bat", _
                              "dism /English " & If(OnlineMode, " /online", " /image=" & Quote & MountDir & Quote) & " /get-provisionedappxpackages | findstr /c:" & Quote & "Version : " & Quote & " > .\tempinfo\appxversions", _
                              ASCII)
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\appxarches.bat", _
                              "dism /English " & If(OnlineMode, " /online", " /image=" & Quote & MountDir & Quote) & " /get-provisionedappxpackages | findstr /c:" & Quote & "Architecture : " & Quote & " > .\tempinfo\appxarchitectures", _
                              ASCII)
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\appxresids.bat", _
                              "dism /English " & If(OnlineMode, " /online", " /image=" & Quote & MountDir & Quote) & " /get-provisionedappxpackages | findstr /c:" & Quote & "ResourceId : " & Quote & " > .\tempinfo\appxresids", _
                              ASCII)
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\appxregions.bat", _
                              "dism /English " & If(OnlineMode, " /online", " /image=" & Quote & MountDir & Quote) & " /get-provisionedappxpackages | findstr /c:" & Quote & "Regions : " & Quote & " > .\tempinfo\appxregions", _
                              ASCII)
        Catch ex As Exception
            Debug.WriteLine("[GetImageAppxPackages] Failed writing getter scripts. Reason: " & ex.Message)
            CompletedTasks(2) = False
            PendingTasks(2) = True
            Exit Sub
        End Try
        Debug.WriteLine("[GetImageAppxPackages] Finished writing getter scripts. Executing them...")
        ImgProcesses.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
        ImgProcesses.StartInfo.CreateNoWindow = True
        ImgProcesses.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        For Each appxScript In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\bin\exthelpers", FileIO.SearchOption.SearchTopLevelOnly, "*.bat")
            If Path.GetFileName(appxScript).StartsWith("appx") Then
                Debug.WriteLine("[GetImageAppxPackages] RunCommand -> " & Path.GetFileName(appxScript))
                ImgProcesses.StartInfo.Arguments = "/c " & appxScript
                ImgProcesses.Start()
                ImgProcesses.WaitForExit()
                If ImgProcesses.ExitCode = 0 Then
                    Continue For
                End If
            Else
                Continue For
            End If
        Next
        Debug.WriteLine("[GetImageAppxPackages] Finished running getter scripts. Filling arrays...")
        Dim FileGetterRTB As New RichTextBox()
        Dim TypeLookups() As String = New String(5) {"DisplayName : ", "PackageName : ", "Version : ", "Architecture : ", "ResourceId : ", "Regions : "}
        Dim lineToAppend As String = ""
        For Each appxFile In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\tempinfo", FileIO.SearchOption.SearchTopLevelOnly)
            If Path.GetFileName(appxFile).StartsWith("appx") Then
                Debug.WriteLine("[GetImageAppxPackages] FillArray -> (values_from: " & Path.GetFileName(appxFile) & ")")
                FileGetterRTB.Clear()
                FileGetterRTB.Text = My.Computer.FileSystem.ReadAllText(appxFile)
                For x = 0 To FileGetterRTB.Lines.Count - 1
                    If FileGetterRTB.Lines(x) = "" Then
                        Continue For
                    Else
                        If FileGetterRTB.Lines(x).StartsWith(TypeLookups(0)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("DisplayName : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgAppxDisplayNames(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(1)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("PackageName : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgAppxPackageNames(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(2)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("Version : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgAppxVersions(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(3)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("Architecture : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgAppxArchitectures(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(4)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("ResourceId : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgAppxResourceIds(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(5)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("Regions : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgAppxRegions(x) = lineToAppend
                        Else
                            Continue For
                        End If
                    End If
                Next
            Else
                Continue For
            End If
        Next
        If OnlineMode And ExtAppxGetter Then
            Dim imgAppxDisplayNameList As New List(Of String)
            Dim imgAppxPackageNameList As New List(Of String)
            Dim imgAppxVersionList As New List(Of String)
            Dim imgAppxArchitectureList As New List(Of String)
            Dim imgAppxResourceIdList As New List(Of String)
            imgAppxDisplayNameList = imgAppxDisplayNames.ToList()
            imgAppxPackageNameList = imgAppxPackageNames.ToList()
            imgAppxVersionList = imgAppxVersions.ToList()
            imgAppxArchitectureList = imgAppxArchitectures.ToList()
            imgAppxResourceIdList = imgAppxResourceIds.ToList()
            PSExtAppxGetter()
            If Directory.Exists(Application.StartupPath & "\bin\extps1\out") And My.Computer.FileSystem.GetFiles(Application.StartupPath & "\bin\extps1\out").Count > 0 Then
                Dim appxPkgNameRTB As New RichTextBox()
                Dim appxPkgFullNameRTB As New RichTextBox()
                Dim appxArchRTB As New RichTextBox()
                Dim appxResIdRTB As New RichTextBox()
                Dim appxVerRTB As New RichTextBox()
                Dim appxNonRemPolRTB As New RichTextBox()
                appxPkgNameRTB.Text = File.ReadAllText(Application.StartupPath & "\bin\extps1\out\appxpkgnames")
                appxPkgFullNameRTB.Text = File.ReadAllText(Application.StartupPath & "\bin\extps1\out\appxpkgfullnames")
                appxArchRTB.Text = File.ReadAllText(Application.StartupPath & "\bin\extps1\out\appxarch")
                appxResIdRTB.Text = File.ReadAllText(Application.StartupPath & "\bin\extps1\out\appxresid")
                appxVerRTB.Text = File.ReadAllText(Application.StartupPath & "\bin\extps1\out\appxver")
                If File.Exists(Application.StartupPath & "\bin\extps1\out\appxnonrempolicy") Then appxNonRemPolRTB.Text = File.ReadAllText(Application.StartupPath & "\bin\extps1\out\appxnonrempolicy")
                For x = 0 To appxPkgFullNameRTB.Lines.Count - 1
                    If imgAppxPackageNameList.Contains(appxPkgFullNameRTB.Lines(x)) Then
                        Continue For
                    Else
                        If SkipNonRemovable And File.Exists(Application.StartupPath & "\bin\extps1\out\appxnonrempolicy") Then
                            If appxNonRemPolRTB.Lines(x) = "True" Then
                                Continue For
                            Else
                                imgAppxDisplayNameList.Add(appxPkgNameRTB.Lines(x))
                                imgAppxPackageNameList.Add(appxPkgFullNameRTB.Lines(x))
                                imgAppxArchitectureList.Add(appxArchRTB.Lines(x))
                                imgAppxResourceIdList.Add(appxResIdRTB.Lines(x))
                                imgAppxVersionList.Add(appxVerRTB.Lines(x))
                            End If
                        Else
                            imgAppxDisplayNameList.Add(appxPkgNameRTB.Lines(x))
                            imgAppxPackageNameList.Add(appxPkgFullNameRTB.Lines(x))
                            imgAppxArchitectureList.Add(appxArchRTB.Lines(x))
                            imgAppxResourceIdList.Add(appxResIdRTB.Lines(x))
                            imgAppxVersionList.Add(appxVerRTB.Lines(x))
                        End If
                    End If
                Next
                Try
                    Directory.Delete(Application.StartupPath & "\bin\extps1\out", True)
                Catch ex As Exception
                    ' Leave directory for later
                End Try
            End If
            imgAppxDisplayNames = imgAppxDisplayNameList.ToArray()
            imgAppxPackageNames = imgAppxPackageNameList.ToArray()
            imgAppxVersions = imgAppxVersionList.ToArray()
            imgAppxArchitectures = imgAppxArchitectureList.ToArray()
            imgAppxResourceIds = imgAppxResourceIdList.ToArray()
        End If
        CompletedTasks(2) = True
        PendingTasks(2) = False
        'ImgBW.ReportProgress(progressMin + progressDivs)
    End Sub

    Sub PSExtAppxGetter()
        Dim PSExtAppxProc As New Process
        PSExtAppxProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
        PSExtAppxProc.StartInfo.WorkingDirectory = Application.StartupPath
        ' The "executionpolicy" argument is passed to PowerShell as a temporary execution policy setting that happens once.
        ' More on that here: https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_execution_policies?view=powershell-7.3#set-a-different-policy-for-one-session
        PSExtAppxProc.StartInfo.Arguments = "-executionpolicy unrestricted -file " & Quote & Application.StartupPath & "\bin\extps1\extappx.ps1" & Quote
        If Not Debugger.IsAttached Then
            PSExtAppxProc.StartInfo.CreateNoWindow = True
            PSExtAppxProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        End If
        PSExtAppxProc.Start()
        PSExtAppxProc.WaitForExit()
        If PSExtAppxProc.ExitCode = 0 Then

        End If
    End Sub

    ''' <summary>
    ''' Gets installed Features on Demand (capabilities) in an image and puts them in separate arrays
    ''' </summary>
    ''' <remarks>This is only for Windows 10 or newer</remarks>
    Sub GetImageCapabilities(Optional UseApi As Boolean = False, Optional OnlineMode As Boolean = False)
        If UseApi Then
            Try
                DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
                Using session As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(sessionMntDir))
                    Dim imgCapabilityNameList As New List(Of String)
                    Dim imgCapabilityStateList As New List(Of String)
                    Dim CapabilityCollection As DismCapabilityCollection = DismApi.GetCapabilities(session)
                    CapabilityInfoList = CapabilityCollection
                    For Each capability As DismCapability In CapabilityCollection
                        If ImgBW.CancellationPending Then
                            If UseApi And session IsNot Nothing Then DismApi.CloseSession(session)
                            CompletedTasks(3) = False
                            PendingTasks(3) = True
                            Exit Sub
                        End If
                        imgCapabilityNameList.Add(capability.Name)
                        Select Case capability.State
                            Case DismPackageFeatureState.NotPresent
                                imgCapabilityStateList.Add("Not present")
                            Case DismPackageFeatureState.UninstallPending
                                imgCapabilityStateList.Add("Uninstall pending")
                            Case DismPackageFeatureState.Staged
                                imgCapabilityStateList.Add("Uninstalled")
                            Case DismPackageFeatureState.Removed Or DismPackageFeatureState.Resolved
                                imgCapabilityStateList.Add("Removed")
                            Case DismPackageFeatureState.Installed
                                imgCapabilityStateList.Add("Installed")
                            Case DismPackageFeatureState.InstallPending
                                imgCapabilityStateList.Add("Install Pending")
                            Case DismPackageFeatureState.Superseded
                                imgCapabilityStateList.Add("Superseded")
                            Case DismPackageFeatureState.PartiallyInstalled
                                imgCapabilityStateList.Add("Partially Installed")
                        End Select
                    Next
                    imgCapabilityIds = imgCapabilityNameList.ToArray()
                    imgCapabilityState = imgCapabilityStateList.ToArray()
                End Using
            Finally
                DismApi.Shutdown()
            End Try
            CompletedTasks(3) = True
            PendingTasks(3) = False
            Exit Sub
            'Try

            '    If session IsNot Nothing Then
            '        Dim imgCapabilityNameList As New List(Of String)
            '        Dim imgCapabilityStateList As New List(Of String)
            '        Dim CapabilityCollection As DismCapabilityCollection = DismApi.GetCapabilities(session)
            '        For Each capability As DismCapability In CapabilityCollection
            '            If ImgBW.CancellationPending Then
            '                If UseApi And session IsNot Nothing Then DismApi.CloseSession(session)
            '                Exit Sub
            '            End If
            '            imgCapabilityNameList.Add(capability.Name)
            '            Select Case capability.State
            '                Case DismPackageFeatureState.NotPresent
            '                    imgCapabilityStateList.Add("Not present")
            '                Case DismPackageFeatureState.UninstallPending
            '                    imgCapabilityStateList.Add("Uninstall pending")
            '                Case DismPackageFeatureState.Staged
            '                    imgCapabilityStateList.Add("Uninstalled")
            '                Case DismPackageFeatureState.Removed Or DismPackageFeatureState.Resolved
            '                    imgCapabilityStateList.Add("Removed")
            '                Case DismPackageFeatureState.Installed
            '                    imgCapabilityStateList.Add("Installed")
            '                Case DismPackageFeatureState.InstallPending
            '                    imgCapabilityStateList.Add("Install Pending")
            '                Case DismPackageFeatureState.Superseded
            '                    imgCapabilityStateList.Add("Superseded")
            '                Case DismPackageFeatureState.PartiallyInstalled
            '                    imgCapabilityStateList.Add("Partially Installed")
            '            End Select
            '        Next
            '        imgCapabilityIds = imgCapabilityNameList.ToArray()
            '        imgCapabilityState = imgCapabilityStateList.ToArray()
            '        Exit Sub
            '    Else
            '        Throw New Exception("No valid DISM session has been provided")
            '    End If
            'Catch ex As Exception
            '    DismApi.CloseSession(session)
            'End Try
        End If
        Debug.WriteLine("[GetImageCapabilities] Running function...")
        ' The image may be Windows 10/11, but DISM may not be from Windows 10/11. Get this information before running this procedure
        Dim FileVersion As FileVersionInfo = FileVersionInfo.GetVersionInfo(DismExe)
        Select Case FileVersion.ProductMajorPart
            Case 6
                ' Exit procedure
                Debug.WriteLine("[GetImageCapabilities] The image is Windows 10 or 11, but this version of DISM does not support this command. Exiting...")
                CompletedTasks(3) = False
        End Select
        Debug.WriteLine("[GetImageCapabilities] Writing getter scripts...")
        Try
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\capids.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-capabilities | findstr /c:" & Quote & "Capability Identity : " & Quote & " > .\tempinfo\capids", _
                              ASCII)
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\capstate.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-capabilities | findstr /c:" & Quote & "State : " & Quote & " > .\tempinfo\capstate", _
                              ASCII)
        Catch ex As Exception
            Debug.WriteLine("[GetImageCapabilities] Failed writing getter scripts. Reason: " & ex.Message)
            CompletedTasks(3) = False
            Exit Sub
        End Try
        Debug.WriteLine("[GetImageCapabilities] Finished writing getter scripts. Executing them...")
        ImgProcesses.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
        ImgProcesses.StartInfo.CreateNoWindow = True
        ImgProcesses.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        For Each capScript In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\bin\exthelpers", FileIO.SearchOption.SearchTopLevelOnly, "*.bat")
            If Path.GetFileName(capScript).StartsWith("cap") Then
                Debug.WriteLine("[GetImageCapabilities] RunCommand -> " & Path.GetFileName(capScript))
                ImgProcesses.StartInfo.Arguments = "/c " & capScript
                ImgProcesses.Start()
                ImgProcesses.WaitForExit()
                If ImgProcesses.ExitCode = 0 Then
                    Continue For
                End If
            Else
                Continue For
            End If
        Next
        Debug.WriteLine("[GetImageCapabilities] Finished running getter scripts. Filling arrays...")
        Dim FileGetterRTB As New RichTextBox()
        Dim TypeLookups() As String = New String(1) {"Capability Identity : ", "State : "}
        Dim lineToAppend As String = ""
        For Each capFile In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\tempinfo", FileIO.SearchOption.SearchTopLevelOnly)
            If Path.GetFileName(capFile).StartsWith("cap") Then
                Debug.WriteLine("[GetImageCapabilities] FillArray -> (values_from: " & Path.GetFileName(capFile) & ")")
                FileGetterRTB.Clear()
                FileGetterRTB.Text = My.Computer.FileSystem.ReadAllText(capFile)
                For x = 0 To FileGetterRTB.Lines.Count - 1
                    If FileGetterRTB.Lines(x) = "" Then
                        Continue For
                    Else
                        If FileGetterRTB.Lines(x).StartsWith(TypeLookups(0)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("Capability Identity : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgCapabilityIds(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(1)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("State : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgCapabilityState(x) = lineToAppend
                        Else
                            Continue For
                        End If
                    End If
                Next
            Else
                Continue For
            End If
        Next
        CompletedTasks(3) = True
        PendingTasks(3) = False
        'ImgBW.ReportProgress(progressMin + progressDivs)
    End Sub

    ''' <summary>
    ''' Gets installed third-party drivers in an image and puts them in separate arrays
    ''' </summary>
    ''' <remarks>This procedure will detect the number of third-party drivers. If the image contains none, this procedure will end</remarks>
    Sub GetImageDrivers(Optional UseApi As Boolean = False, Optional OnlineMode As Boolean = False)
        If UseApi Then
            Try
                DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
                Using session As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(sessionMntDir))
                    Dim imgDrvPublishedNameList As New List(Of String)
                    Dim imgDrvOGFileNameList As New List(Of String)
                    Dim imgDrvInboxList As New List(Of String)
                    Dim imgDrvClassNameList As New List(Of String)
                    Dim imgDrvProviderNameList As New List(Of String)
                    Dim imgDrvDateList As New List(Of String)
                    Dim imgDrvVersionList As New List(Of String)
                    Dim imgDrvBootCriticalStatusList As New List(Of Boolean)
                    Dim DriverCollection As DismDriverPackageCollection = DismApi.GetDrivers(session, AllDrivers)
                    DriverInfoList = DriverCollection
                    For Each driver As DismDriverPackage In DriverCollection
                        If ImgBW.CancellationPending Then
                            If UseApi And session IsNot Nothing Then DismApi.CloseSession(session)
                            CompletedTasks(4) = False
                            PendingTasks(4) = True
                            Exit Sub
                        End If
                        imgDrvPublishedNameList.Add(driver.PublishedName)
                        imgDrvOGFileNameList.Add(driver.OriginalFileName)
                        imgDrvInboxList.Add(driver.InBox)
                        imgDrvClassNameList.Add(driver.ClassName)
                        imgDrvProviderNameList.Add(driver.ProviderName)
                        imgDrvDateList.Add(driver.Date.ToString())
                        imgDrvVersionList.Add(driver.Version.ToString())
                        imgDrvBootCriticalStatusList.Add(driver.BootCritical)
                    Next
                    imgDrvPublishedNames = imgDrvPublishedNameList.ToArray()
                    imgDrvOGFileNames = imgDrvOGFileNameList.ToArray()
                    imgDrvInbox = imgDrvInboxList.ToArray()
                    imgDrvClassNames = imgDrvClassNameList.ToArray()
                    imgDrvProviderNames = imgDrvProviderNameList.ToArray()
                    imgDrvDates = imgDrvDateList.ToArray()
                    imgDrvVersions = imgDrvVersionList.ToArray()
                    imgDrvBootCriticalStatus = imgDrvBootCriticalStatusList.ToArray()
                End Using
            Finally
                DismApi.Shutdown()
            End Try
            CompletedTasks(4) = True
            PendingTasks(4) = False
            Exit Sub
            'Try
            '    If session IsNot Nothing Then
            '        Dim imgDrvPublishedNameList As New List(Of String)
            '        Dim imgDrvOGFileNameList As New List(Of String)
            '        Dim imgDrvInboxList As New List(Of String)
            '        Dim imgDrvClassNameList As New List(Of String)
            '        Dim imgDrvProviderNameList As New List(Of String)
            '        Dim imgDrvDateList As New List(Of String)
            '        Dim imgDrvVersionList As New List(Of String)
            '        Dim DriverCollection As DismDriverPackageCollection = DismApi.GetDrivers(session, True)
            '        For Each driver As DismDriverPackage In DriverCollection
            '            If ImgBW.CancellationPending Then
            '                If UseApi And session IsNot Nothing Then DismApi.CloseSession(session)
            '                Exit Sub
            '            End If
            '            imgDrvPublishedNameList.Add(driver.PublishedName)
            '            imgDrvOGFileNameList.Add(driver.OriginalFileName)
            '            imgDrvInboxList.Add(driver.InBox)
            '            imgDrvClassNameList.Add(driver.ClassName)
            '            imgDrvProviderNameList.Add(driver.ProviderName)
            '            imgDrvDateList.Add(driver.Date.ToString())
            '            imgDrvVersionList.Add(driver.Version.ToString())
            '        Next
            '        imgDrvPublishedNames = imgDrvPublishedNameList.ToArray()
            '        imgDrvOGFileNames = imgDrvOGFileNameList.ToArray()
            '        imgDrvInbox = imgDrvInboxList.ToArray()
            '        imgDrvClassNames = imgDrvClassNameList.ToArray()
            '        imgDrvProviderNames = imgDrvProviderNameList.ToArray()
            '        imgDrvDates = imgDrvDateList.ToArray()
            '        imgDrvVersions = imgDrvVersionList.ToArray()
            '        Exit Sub
            '    Else
            '        Throw New Exception("No valid DISM session has been provided")
            '    End If
            'Catch ex As Exception
            '    DismApi.CloseSession(session)
            '    Exit Try
            'End Try
        End If
        Debug.WriteLine("[GetImageDrivers] Running function...")
        Debug.WriteLine("[GetImageDrivers] Determining whether there are third-party drivers in image...")
        Try
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\drvnums.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-drivers | find /c " & Quote & "Published Name : " & Quote & " > .\tempinfo\drvnums", _
                              ASCII)
        Catch ex As Exception
            Debug.WriteLine("[GetImageDrivers] Failed writing getter scripts. Reason: " & ex.Message)
            CompletedTasks(4) = False
            Exit Sub
        End Try
        ImgProcesses.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
        ImgProcesses.StartInfo.CreateNoWindow = True
        ImgProcesses.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        ImgProcesses.StartInfo.Arguments = "/c " & Application.StartupPath & "\bin\exthelpers\drvnums.bat"
        ImgProcesses.Start()
        ImgProcesses.WaitForExit()
        File.Delete(Application.StartupPath & "\bin\exthelpers\drvnums.bat")
        If ImgProcesses.ExitCode = 0 Then
            Dim drvCount As Integer = CInt(My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\tempinfo\drvnums"))
            If drvCount = 0 Then
                Debug.WriteLine("[GetImageDrivers] There are no available third-party drivers in this image. Exiting function...")
                Exit Sub
            End If
        End If
        Debug.WriteLine("[GetImageDrivers] Writing getter scripts...")
        Try
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\drvpublishednames.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-drivers | findstr /c:" & Quote & "Published Name : " & Quote & " > .\tempinfo\drvpublishednames", _
                              ASCII)
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\drvogfilenames.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-drivers | findstr /c:" & Quote & "Original File Name : " & Quote & " > .\tempinfo\drvogfilenames", _
                              ASCII)
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\drvinbox.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-drivers | findstr /c:" & Quote & "Inbox : " & Quote & " > .\tempinfo\drvinbox", _
                              ASCII)
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\drvclassnames.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-drivers | findstr /c:" & Quote & "Class Name : " & Quote & " > .\tempinfo\drvclassnames", _
                              ASCII)
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\drvprovnames.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-drivers | findstr /c:" & Quote & "Provider Name : " & Quote & " > .\tempinfo\drvprovnames", _
                              ASCII)
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\drvdates.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-drivers | findstr /c:" & Quote & "Date : " & Quote & " > .\tempinfo\drvdates", _
                              ASCII)
            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\drvversions.bat", _
                              "@echo off" & CrLf & _
                              "dism /English /image=" & Quote & MountDir & Quote & " /get-drivers | findstr /c:" & Quote & "Version : " & Quote & " > .\tempinfo\drvversions", _
                              ASCII)
        Catch ex As Exception
            Debug.WriteLine("[GetImageDrivers] Failed writing getter scripts. Reason: " & ex.Message)
            Exit Sub
        End Try
        Debug.WriteLine("[GetImageDrivers] Finished writing getter scripts. Executing them...")
        ImgProcesses.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
        ImgProcesses.StartInfo.CreateNoWindow = True
        ImgProcesses.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        For Each drvScript In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\bin\exthelpers", FileIO.SearchOption.SearchTopLevelOnly, "*.bat")
            If Path.GetFileName(drvScript).StartsWith("drv") Then
                Debug.WriteLine("[GetImageDrivers] RunCommand -> " & Path.GetFileName(drvScript))
                ImgProcesses.StartInfo.Arguments = "/c " & drvScript
                ImgProcesses.Start()
                ImgProcesses.WaitForExit()
                If ImgProcesses.ExitCode = 0 Then
                    Continue For
                End If
            Else
                Continue For
            End If
        Next
        Debug.WriteLine("[GetImageDrivers] Finished running getter scripts. Filling arrays...")
        Dim FileGetterRTB As New RichTextBox()
        Dim TypeLookups() As String = New String(6) {"Published Name : ", "Original File Name : ", "Inbox : ", "Class Name : ", "Provider Name : ", "Date : ", "Version : "}
        Dim lineToAppend As String = ""
        For Each drvFile In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\tempinfo", FileIO.SearchOption.SearchTopLevelOnly)
            If Path.GetFileName(drvFile).StartsWith("drv") Then
                Debug.WriteLine("[GetImageDrivers] FillArray -> (values_from: " & Path.GetFileName(drvFile) & ")")
                FileGetterRTB.Clear()
                FileGetterRTB.Text = My.Computer.FileSystem.ReadAllText(drvFile)
                For x = 0 To FileGetterRTB.Lines.Count - 1
                    If FileGetterRTB.Lines(x) = "" Then
                        Continue For
                    Else
                        If FileGetterRTB.Lines(x).StartsWith(TypeLookups(0)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("Published Name : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgDrvPublishedNames(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(1)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("Original File Name : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgDrvOGFileNames(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(2)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("Inbox : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgDrvInbox(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(3)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("Class Name : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgDrvClassNames(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(4)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("Provider Name : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgDrvProviderNames(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(5)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("Date : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgDrvDates(x) = lineToAppend
                        ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(6)) Then
                            lineToAppend = FileGetterRTB.Lines(x).Replace("Version : ", "").Trim()
                            If lineToAppend = "" Then lineToAppend = "Nothing"
                            imgDrvVersions(x) = lineToAppend
                        Else
                            Continue For
                        End If
                    End If
                Next
            Else
                Continue For
            End If
        Next
        CompletedTasks(4) = True
        PendingTasks(4) = False
        'ImgBW.ReportProgress(progressMin + progressDivs)
    End Sub

    ''' <summary>
    ''' Deletes temporary files created by the background processes
    ''' </summary>
    Sub DeleteTempFiles()
        If MountedImageDetectorBW.IsBusy Then
            Exit Sub
        End If
        Try
            Directory.Delete(Application.StartupPath & "\tempinfo", True)
            ' Keep the "exthelpers" folder in case background processes need to be run again
            For Each TempFile In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\bin\exthelpers", FileIO.SearchOption.SearchTopLevelOnly)
                File.Delete(TempFile)
            Next
        Catch ex As Exception
            Debug.WriteLine("[DeleteTempFiles] Could not delete temporary files. Reason: " & ex.Message)
        End Try
    End Sub

#End Region

    Sub GenerateDTSettings()
        DTSettingForm.RichTextBox2.AppendText("# DISMTools (version 0.4) configuration file" & CrLf & CrLf & "[Program]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("DismExe=" & Quote & "{common:WinDir}\system32\dism.exe" & Quote)
        DTSettingForm.RichTextBox2.AppendText(CrLf & "SaveOnSettingsIni=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "Volatile=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[Personalization]" & CrLf)
        Try
            Dim ColorModeRk As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", False)
            Dim ColorMode As String = ColorModeRk.GetValue("AppsUseLightTheme").ToString()
            ColorModeRk.Close()
            DTSettingForm.RichTextBox2.AppendText("ColorMode=0")
        Catch ex As Exception
            ' Rollback to light theme
            DTSettingForm.RichTextBox2.AppendText("ColorMode=1")
        End Try
        DTSettingForm.RichTextBox2.AppendText(CrLf & "Language=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "LogFont=" & Quote & "Courier New" & Quote)
        DTSettingForm.RichTextBox2.AppendText(CrLf & "LogFontSi=10")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "LogFontBold=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "SecondaryProgressPanelStyle=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "AllCaps=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[Logs]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("LogFile=" & Quote & "{common:WinDir}\Logs\DISM\DISM.log" & Quote)
        DTSettingForm.RichTextBox2.AppendText(CrLf & "LogLevel=3")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "AutoLogs=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[ImgOps]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("ImgOperationMode=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "Quiet=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "NoRestart=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[ScratchDir]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("UseScratch=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "AutoScratch=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "ScratchDirLocation=" & Quote & "" & Quote)
        DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[Output]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("EnglishOutput=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "ReportView=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[BgProcesses]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("ShowNotification=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "NotifyFrequency=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[AdvBgProcesses]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("EnhancedAppxGetter=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "SkipNonRemovable=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "DetectAllDrivers=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "SkipFrameworks=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "RunAllProcs=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[Startup]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("RemountImages=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "CheckForUpdates=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[WndParams]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("WndWidth=1280")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "WndHeight=720")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "WndCenter=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "WndLeft=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "WndTop=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "WndMaximized=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[InfoSaver]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("SkipQuestions=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "Pkg_CompleteInfo=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "Feat_CompleteInfo=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "AppX_CompleteInfo=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "Cap_CompleteInfo=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "Drv_CompleteInfo=1")
        File.WriteAllText(Application.StartupPath & "\settings.ini", DTSettingForm.RichTextBox2.Text, ASCII)
        If File.Exists(Application.StartupPath & "\portable") Then Exit Sub
        Dim KeyStr As String = "Software\DISMTools\" & If(dtBranch.Contains("preview"), "Preview", "Stable")
        Dim Key As RegistryKey = Registry.CurrentUser.CreateSubKey(KeyStr)
        Dim PrgKey As RegistryKey = Key.CreateSubKey("Program")
        PrgKey.SetValue("DismExe", Quote & Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe" & Quote, RegistryValueKind.ExpandString)
        PrgKey.SetValue("SaveOnSettingsIni", 1, RegistryValueKind.DWord)
        PrgKey.SetValue("Volatile", 0, RegistryValueKind.DWord)
        PrgKey.Close()
        Dim PersKey As RegistryKey = Key.CreateSubKey("Personalization")
        Try
            Dim ColorModeRk As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", False)
            Dim ColorMode As String = ColorModeRk.GetValue("AppsUseLightTheme").ToString()
            ColorModeRk.Close()
            PersKey.SetValue("ColorMode", 0, RegistryValueKind.DWord)
        Catch ex As Exception
            ' Rollback to light theme
            PersKey.SetValue("ColorMode", 1, RegistryValueKind.DWord)
        End Try
        PersKey.SetValue("Language", 0, RegistryValueKind.DWord)
        PersKey.SetValue("LogFont", "Courier New", RegistryValueKind.String)
        PersKey.SetValue("LogFontSi", 10, RegistryValueKind.DWord)
        PersKey.SetValue("LogFontBold", 0, RegistryValueKind.DWord)
        PersKey.SetValue("SecondaryProgressPanelStyle", 1, RegistryValueKind.DWord)
        PersKey.SetValue("AllCaps", 0, RegistryValueKind.DWord)
        PersKey.Close()
        Dim LogKey As RegistryKey = Key.CreateSubKey("Logs")
        LogKey.SetValue("LogFile", Quote & Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\logs\DISM\DISM.log" & Quote, RegistryValueKind.ExpandString)
        LogKey.SetValue("LogLevel", 3, RegistryValueKind.DWord)
        LogKey.SetValue("AutoLogs", 1, RegistryValueKind.DWord)
        LogKey.Close()
        Dim ImgOpKey As RegistryKey = Key.CreateSubKey("ImgOps")
        ImgOpKey.SetValue("Quiet", 0, RegistryValueKind.DWord)
        ImgOpKey.SetValue("NoRestart", 0, RegistryValueKind.DWord)
        ImgOpKey.Close()
        Dim ScrDirKey As RegistryKey = Key.CreateSubKey("ScratchDir")
        ScrDirKey.SetValue("UseScratch", 0, RegistryValueKind.DWord)
        ScrDirKey.SetValue("AutoScratch", 1, RegistryValueKind.DWord)
        ScrDirKey.SetValue("ScratchDirLocation", "", RegistryValueKind.ExpandString)
        ScrDirKey.Close()
        Dim OutKey As RegistryKey = Key.CreateSubKey("Output")
        OutKey.SetValue("EnglishOutput", 1, RegistryValueKind.DWord)
        OutKey.SetValue("ReportView", 0, RegistryValueKind.DWord)
        OutKey.Close()
        Dim BGKey As RegistryKey = Key.CreateSubKey("BgProcesses")
        BGKey.SetValue("ShowNotification", 1, RegistryValueKind.DWord)
        BGKey.SetValue("NotifyFrequency", 1, RegistryValueKind.DWord)
        BGKey.Close()
        Dim AdvBGKey As RegistryKey = Key.CreateSubKey("AdvBgProcesses")
        AdvBGKey.SetValue("EnhancedAppxGetter", 1, RegistryValueKind.DWord)
        AdvBGKey.SetValue("SkipNonRemovable", 1, RegistryValueKind.DWord)
        AdvBGKey.SetValue("DetectAllDrivers", 0, RegistryValueKind.DWord)
        AdvBGKey.SetValue("SkipFrameworks", 1, RegistryValueKind.DWord)
        AdvBGKey.SetValue("RunAllProcs", 0, RegistryValueKind.DWord)
        AdvBGKey.Close()
        Dim StartupKey As RegistryKey = Key.CreateSubKey("Startup")
        StartupKey.SetValue("RemountImages", 1, RegistryValueKind.DWord)
        StartupKey.SetValue("CheckForUpdates", 1, RegistryValueKind.DWord)
        StartupKey.Close()
        Dim WndKey As RegistryKey = Key.CreateSubKey("WndParams")
        WndKey.SetValue("WndWidth", 1280, RegistryValueKind.DWord)
        WndKey.SetValue("WndHeight", 720, RegistryValueKind.DWord)
        WndKey.SetValue("WndCenter", 1, RegistryValueKind.DWord)
        WndKey.SetValue("WndLeft", 0, RegistryValueKind.DWord)
        WndKey.SetValue("WndTop", 0, RegistryValueKind.DWord)
        WndKey.SetValue("WndMaximized", 0, RegistryValueKind.DWord)
        WndKey.Close()
        Dim InfoSaverKey As RegistryKey = Key.CreateSubKey("InfoSaver")
        InfoSaverKey.SetValue("SkipQuestions", 1, RegistryValueKind.DWord)
        InfoSaverKey.SetValue("Pkg_CompleteInfo", 1, RegistryValueKind.DWord)
        InfoSaverKey.SetValue("Feat_CompleteInfo", 1, RegistryValueKind.DWord)
        InfoSaverKey.SetValue("AppX_CompleteInfo", 1, RegistryValueKind.DWord)
        InfoSaverKey.SetValue("Cap_CompleteInfo", 1, RegistryValueKind.DWord)
        InfoSaverKey.SetValue("Drv_CompleteInfo", 1, RegistryValueKind.DWord)
        InfoSaverKey.Close()
        Key.Close()
    End Sub

    Sub SaveDTSettings()
        If VolatileMode Then
            Exit Sub
        Else
            If SaveOnSettingsIni Then
                If File.Exists(Application.StartupPath & "\settings.ini") Then
                    File.Delete(Application.StartupPath & "\settings.ini")
                End If
                DTSettingForm.RichTextBox2.Clear()
                DTSettingForm.RichTextBox2.AppendText("# DISMTools (version 0.4) configuration file" & CrLf & CrLf & "[Program]" & CrLf)
                DTSettingForm.RichTextBox2.AppendText("DismExe=" & Quote & DismExe & Quote)
                If SaveOnSettingsIni Then
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "SaveOnSettingsIni=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "SaveOnSettingsIni=0")
                End If
                If VolatileMode Then
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "Volatile=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "Volatile=0")
                End If
                DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[Personalization]" & CrLf)
                Select Case ColorMode
                    Case 0
                        DTSettingForm.RichTextBox2.AppendText("ColorMode=0")
                    Case 1
                        DTSettingForm.RichTextBox2.AppendText("ColorMode=1")
                    Case 2
                        DTSettingForm.RichTextBox2.AppendText("ColorMode=2")
                End Select
                DTSettingForm.RichTextBox2.AppendText(CrLf & "Language=" & Language)
                DTSettingForm.RichTextBox2.AppendText(CrLf & "LogFont=" & Quote & LogFont & Quote)
                DTSettingForm.RichTextBox2.AppendText(CrLf & "LogFontSi=" & LogFontSize)
                If LogFontIsBold Then
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "LogFontBold=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "LogFontBold=0")
                End If
                Select Case ProgressPanelStyle
                    Case 0
                        DTSettingForm.RichTextBox2.AppendText(CrLf & "SecondaryProgressPanelStyle=0")
                    Case 1
                        DTSettingForm.RichTextBox2.AppendText(CrLf & "SecondaryProgressPanelStyle=1")
                End Select
                If AllCaps Then
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "AllCaps=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "AllCaps=0")
                End If
                DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[Logs]" & CrLf)
                DTSettingForm.RichTextBox2.AppendText("LogFile=" & Quote & LogFile & Quote)
                Select Case LogLevel
                    Case 1
                        DTSettingForm.RichTextBox2.AppendText(CrLf & "LogLevel=1")
                    Case 2
                        DTSettingForm.RichTextBox2.AppendText(CrLf & "LogLevel=2")
                    Case 3
                        DTSettingForm.RichTextBox2.AppendText(CrLf & "LogLevel=3")
                    Case 4
                        DTSettingForm.RichTextBox2.AppendText(CrLf & "LogLevel=4")
                End Select
                If AutoLogs Then
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "AutoLogs=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "AutoLogs=0")
                End If
                DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[ImgOps]" & CrLf)
                Select Case ImgOperationMode
                    Case 0
                        DTSettingForm.RichTextBox2.AppendText("ImgOperationMode=0")
                    Case 1
                        DTSettingForm.RichTextBox2.AppendText("ImgOperationMode=1")
                End Select
                If QuietOperations Then
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "Quiet=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "Quiet=0")
                End If
                If SysNoRestart Then
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "NoRestart=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "NoRestart=0")
                End If
                DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[ScratchDir]" & CrLf)
                If UseScratch Then
                    DTSettingForm.RichTextBox2.AppendText("UseScratch=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText("UseScratch=0")
                End If
                If AutoScrDir Then
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "AutoScratch=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "AutoScratch=0")
                End If
                DTSettingForm.RichTextBox2.AppendText(CrLf & "ScratchDirLocation=" & Quote & ScratchDir & Quote)
                DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[Output]" & CrLf)
                If EnglishOutput Then
                    DTSettingForm.RichTextBox2.AppendText("EnglishOutput=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText("EnglishOutput=0")
                End If
                Select Case ReportView
                    Case 0
                        DTSettingForm.RichTextBox2.AppendText(CrLf & "ReportView=0")
                    Case 1
                        DTSettingForm.RichTextBox2.AppendText(CrLf & "ReportView=1")
                End Select
                DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[BgProcesses]" & CrLf)
                If NotificationShow Then
                    DTSettingForm.RichTextBox2.AppendText("ShowNotification=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText("ShowNotification=0")
                End If
                Select Case NotificationFrequency
                    Case 0
                        DTSettingForm.RichTextBox2.AppendText(CrLf & "NotifyFrequency=0")
                    Case 1
                        DTSettingForm.RichTextBox2.AppendText(CrLf & "NotifyFrequency=1")
                End Select
                DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[AdvBgProcesses]" & CrLf)
                If ExtAppxGetter Then
                    DTSettingForm.RichTextBox2.AppendText("EnhancedAppxGetter=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText("EnhancedAppxGetter=0")
                End If
                If SkipNonRemovable Then
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "SkipNonRemovable=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "SkipNonRemovable=0")
                End If
                If AllDrivers Then
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "DetectAllDrivers=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "DetectAllDrivers=0")
                End If
                If SkipFrameworks Then
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "SkipFrameworks=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "SkipFrameworks=0")
                End If
                If RunAllProcs Then
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "RunAllProcs=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "RunAllProcs=0")
                End If
                DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[Startup]" & CrLf)
                If StartupRemount Then
                    DTSettingForm.RichTextBox2.AppendText("RemountImages=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText("RemountImages=0")
                End If
                If StartupUpdateCheck Then
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "CheckForUpdates=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "CheckForUpdates=0")
                End If
                DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[WndParams]" & CrLf)
                DTSettingForm.RichTextBox2.AppendText("WndWidth=" & WndWidth)
                DTSettingForm.RichTextBox2.AppendText(CrLf & "WndHeight=" & WndHeight)
                If Location = New Point((Screen.FromControl(Me).WorkingArea.Width - Width) / 2, (Screen.FromControl(Me).WorkingArea.Height - Height) / 2) Then
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "WndCenter=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "WndCenter=0")
                End If
                DTSettingForm.RichTextBox2.AppendText(CrLf & "WndLeft=" & WndLeft)
                DTSettingForm.RichTextBox2.AppendText(CrLf & "WndTop=" & WndTop)
                DTSettingForm.RichTextBox2.AppendText(CrLf & "WndMaximized=" & If(WindowState = FormWindowState.Maximized, "1", "0"))
                DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[InfoSaver]" & CrLf)
                DTSettingForm.RichTextBox2.AppendText("SkipQuestions=" & If(SkipQuestions, "1", "0"))
                DTSettingForm.RichTextBox2.AppendText(CrLf & "Pkg_CompleteInfo=" & If(AutoCompleteInfo(0), "1", "0"))
                DTSettingForm.RichTextBox2.AppendText(CrLf & "Feat_CompleteInfo=" & If(AutoCompleteInfo(1), "1", "0"))
                DTSettingForm.RichTextBox2.AppendText(CrLf & "AppX_CompleteInfo=" & If(AutoCompleteInfo(2), "1", "0"))
                DTSettingForm.RichTextBox2.AppendText(CrLf & "Cap_CompleteInfo=" & If(AutoCompleteInfo(3), "1", "0"))
                DTSettingForm.RichTextBox2.AppendText(CrLf & "Drv_CompleteInfo=" & If(AutoCompleteInfo(4), "1", "0"))
                File.WriteAllText(Application.StartupPath & "\settings.ini", DTSettingForm.RichTextBox2.Text, ASCII)
            Else
                ' Tell settings file to use this method
                Dim SettingRtb As New RichTextBox() With {
                    .Text = File.ReadAllText(Application.StartupPath & "\settings.ini", UTF8)
                }
                SettingRtb.Text = SettingRtb.Text.Replace("SaveOnSettingsIni=1", "SaveOnSettingsIni=0").Trim()
                File.WriteAllText(Application.StartupPath & "\settings.ini", SettingRtb.Text, ASCII)
                Dim KeyStr As String = "Software\DISMTools\" & If(dtBranch.Contains("preview"), "Preview", "Stable")
                Dim Key As RegistryKey = Registry.CurrentUser.CreateSubKey(KeyStr)
                Dim PrgKey As RegistryKey = Key.CreateSubKey("Program")
                PrgKey.SetValue("DismExe", Quote & DismExe & Quote, RegistryValueKind.ExpandString)
                PrgKey.SetValue("SaveOnSettingsIni", If(SaveOnSettingsIni, 1, 0), RegistryValueKind.DWord)
                PrgKey.SetValue("Volatile", If(VolatileMode, 1, 0), RegistryValueKind.DWord)
                PrgKey.Close()
                Dim PersKey As RegistryKey = Key.CreateSubKey("Personalization")
                PersKey.SetValue("ColorMode", ColorMode, RegistryValueKind.DWord)
                PersKey.SetValue("Language", Language, RegistryValueKind.DWord)
                PersKey.SetValue("LogFont", LogFont, RegistryValueKind.String)
                PersKey.SetValue("LogFontSi", LogFontSize, RegistryValueKind.DWord)
                PersKey.SetValue("LogFontBold", If(LogFontIsBold, 1, 0), RegistryValueKind.DWord)
                PersKey.SetValue("SecondaryProgressPanelStyle", ProgressPanelStyle, RegistryValueKind.DWord)
                PersKey.SetValue("AllCaps", If(AllCaps, 1, 0), RegistryValueKind.DWord)
                PersKey.Close()
                Dim LogKey As RegistryKey = Key.CreateSubKey("Logs")
                LogKey.SetValue("LogFile", LogFile, RegistryValueKind.ExpandString)
                LogKey.SetValue("LogLevel", LogLevel, RegistryValueKind.DWord)
                LogKey.SetValue("AutoLogs", If(AutoLogs, 1, 0), RegistryValueKind.DWord)
                LogKey.Close()
                Dim ImgOpKey As RegistryKey = Key.CreateSubKey("ImgOps")
                ImgOpKey.SetValue("Quiet", If(QuietOperations, 1, 0), RegistryValueKind.DWord)
                ImgOpKey.SetValue("NoRestart", If(SysNoRestart, 1, 0), RegistryValueKind.DWord)
                ImgOpKey.Close()
                Dim ScrDirKey As RegistryKey = Key.CreateSubKey("ScratchDir")
                ScrDirKey.SetValue("UseScratch", If(UseScratch, 1, 0), RegistryValueKind.DWord)
                ScrDirKey.SetValue("AutoScratch", If(AutoScrDir, 1, 0), RegistryValueKind.DWord)
                ScrDirKey.SetValue("ScratchDirLocation", ScratchDir, RegistryValueKind.ExpandString)
                ScrDirKey.Close()
                Dim OutKey As RegistryKey = Key.CreateSubKey("Output")
                OutKey.SetValue("EnglishOutput", If(EnglishOutput, 1, 0), RegistryValueKind.DWord)
                OutKey.SetValue("ReportView", ReportView, RegistryValueKind.DWord)
                OutKey.Close()
                Dim BGKey As RegistryKey = Key.CreateSubKey("BgProcesses")
                BGKey.SetValue("ShowNotification", If(NotificationShow, 1, 0), RegistryValueKind.DWord)
                BGKey.SetValue("NotifyFrequency", NotificationFrequency, RegistryValueKind.DWord)
                BGKey.Close()
                Dim AdvBGKey As RegistryKey = Key.CreateSubKey("AdvBgProcesses")
                AdvBGKey.SetValue("EnhancedAppxGetter", If(ExtAppxGetter, 1, 0), RegistryValueKind.DWord)
                AdvBGKey.SetValue("SkipNonRemovable", If(SkipNonRemovable, 1, 0), RegistryValueKind.DWord)
                AdvBGKey.SetValue("DetectAllDrivers", If(AllDrivers, 1, 0), RegistryValueKind.DWord)
                AdvBGKey.SetValue("SkipFrameworks", If(SkipFrameworks, 1, 0), RegistryValueKind.DWord)
                AdvBGKey.SetValue("RunAllProcs", If(RunAllProcs, 1, 0), RegistryValueKind.DWord)
                AdvBGKey.Close()
                Dim StartupKey As RegistryKey = Key.CreateSubKey("Startup")
                StartupKey.SetValue("RemountImages", If(StartupRemount, 1, 0), RegistryValueKind.DWord)
                StartupKey.SetValue("CheckForUpdates", If(StartupUpdateCheck, 1, 0), RegistryValueKind.DWord)
                StartupKey.Close()
                Dim WndKey As RegistryKey = Key.CreateSubKey("WndParams")
                WndKey.SetValue("WndWidth", WndWidth, RegistryValueKind.DWord)
                WndKey.SetValue("WndHeight", WndHeight, RegistryValueKind.DWord)
                If Location = New Point((Screen.FromControl(Me).WorkingArea.Width - Width) / 2, (Screen.FromControl(Me).WorkingArea.Height - Height) / 2) Then
                    WndKey.SetValue("WndCenter", 1, RegistryValueKind.DWord)
                Else
                    WndKey.SetValue("WndCenter", 0, RegistryValueKind.DWord)
                End If
                WndKey.SetValue("WndLeft", WndLeft, RegistryValueKind.DWord)
                WndKey.SetValue("WndTop", WndTop, RegistryValueKind.DWord)
                WndKey.SetValue("WndMaximized", If(WindowState = FormWindowState.Maximized, 1, 0), RegistryValueKind.DWord)
                WndKey.Close()
                Dim InfoSaverKey As RegistryKey = Key.CreateSubKey("InfoSaver")
                InfoSaverKey.SetValue("SkipQuestions", If(SkipQuestions, 1, 0), RegistryValueKind.DWord)
                InfoSaverKey.SetValue("Pkg_CompleteInfo", If(AutoCompleteInfo(0), 1, 0), RegistryValueKind.DWord)
                InfoSaverKey.SetValue("Feat_CompleteInfo", If(AutoCompleteInfo(1), 1, 0), RegistryValueKind.DWord)
                InfoSaverKey.SetValue("AppX_CompleteInfo", If(AutoCompleteInfo(2), 1, 0), RegistryValueKind.DWord)
                InfoSaverKey.SetValue("Cap_CompleteInfo", If(AutoCompleteInfo(3), 1, 0), RegistryValueKind.DWord)
                InfoSaverKey.SetValue("Drv_CompleteInfo", If(AutoCompleteInfo(4), 1, 0), RegistryValueKind.DWord)
                InfoSaverKey.Close()
                Key.Close()
            End If
        End If

    End Sub

    Sub ResetDTSettings()
        GenerateDTSettings()
        LoadDTSettings(If(SaveOnSettingsIni, 1, 0))
    End Sub

    ''' <summary>
    ''' Detects properties of the file to determine whether automatic migration needs to be performed
    ''' </summary>
    ''' <remarks>If the file does not exist, the initial setup wizard launches</remarks>
    Sub PerformSettingFileValidation()
        If File.Exists(Application.StartupPath & "\settings.ini") Then
            If NoMigration Then Exit Sub
            Dim bldDate As Date = PrgAbout.RetrieveLinkerTimestamp(Application.StartupPath & "\DISMTools.exe")
            If File.GetLastWriteTime(Application.StartupPath & "\settings.ini") < bldDate Then
                ' Perform setting file migration
                MigrationForm.ShowDialog()
                Thread.Sleep(1500)
            End If
        Else
            ' Show setup window
            SplashScreen.Hide()
            PrgSetup.ShowDialog()
        End If
    End Sub

    ''' <summary>
    ''' Change program colors accordingly. Due to developer's preference, match those of VS2012
    ''' </summary>
    ''' <param name="ColorCode"></param>
    ''' <remarks></remarks>
    Sub ChangePrgColors(ColorCode As Integer)
        Select Case ColorCode
            Case 0
                Try
                    Dim ColorModeRk As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", False)
                    Dim ColorMode As String = ColorModeRk.GetValue("AppsUseLightTheme").ToString()
                    ColorModeRk.Close()
                    If ColorMode = "0" Then
                        If IsWindowsVersionOrGreater(10, 0, 18362) Then EnableDarkTitleBar(Handle, True)
                        BackColor = Color.FromArgb(48, 48, 48)
                        ForeColor = Color.White
                        HomePanel.BackColor = Color.FromArgb(40, 40, 43)
                        HomePanel.ForeColor = Color.White
                        SidePanel.BackColor = Color.FromArgb(31, 31, 34)
                        SidePanel.ForeColor = Color.White
                        WelcomePanel.BackColor = Color.FromArgb(40, 40, 43)
                        WelcomePanel.ForeColor = Color.White
                        PrjPanel.BackColor = Color.FromArgb(40, 40, 43)
                        PrjPanel.ForeColor = Color.White
                        MenuStrip1.BackColor = Color.FromArgb(48, 48, 48)
                        MenuStrip1.ForeColor = Color.White
                        For Each item As ToolStripDropDownItem In MenuStrip1.Items
                            item.DropDown.BackColor = Color.FromArgb(27, 27, 28)
                            item.DropDown.ForeColor = Color.White
                            Try
                                For Each dropDownItem As ToolStripDropDownItem In item.DropDownItems
                                    dropDownItem.DropDown.BackColor = Color.FromArgb(27, 27, 28)
                                    dropDownItem.DropDown.ForeColor = Color.White
                                Next
                            Catch ex As Exception
                                Continue For
                            End Try
                        Next
                        StatusStrip.BackColor = Color.FromArgb(0, 122, 204)
                        StatusStrip.ForeColor = Color.White
                        TabPage1.BackColor = Color.FromArgb(40, 40, 43)
                        TabPage1.ForeColor = Color.White
                        TabPage2.BackColor = Color.FromArgb(40, 40, 43)
                        TabPage2.ForeColor = Color.White
                        TabPage3.BackColor = Color.FromArgb(40, 40, 43)
                        TabPage3.ForeColor = Color.White
                        WelcomeTab.BackColor = Color.FromArgb(40, 40, 43)
                        WelcomeTab.ForeColor = Color.White
                        NewsFeedTab.BackColor = Color.FromArgb(40, 40, 43)
                        NewsFeedTab.ForeColor = Color.White
                        VideosTab.BackColor = Color.FromArgb(40, 40, 43)
                        VideosTab.ForeColor = Color.White
                        PictureBox5.Image = New Bitmap(My.Resources.logo_mainscr_dark)
                        ToolStrip1.BackColor = Color.FromArgb(48, 48, 48)
                        ToolStrip1.ForeColor = Color.White
                        ToolStrip2.BackColor = Color.FromArgb(48, 48, 48)
                        ToolStrip2.ForeColor = Color.White
                        prjTreeView.BackColor = Color.FromArgb(37, 37, 38)
                        prjTreeView.ForeColor = Color.White
                        GroupBox1.BackColor = Color.FromArgb(40, 40, 43)
                        GroupBox1.ForeColor = Color.White
                        GroupBox2.BackColor = Color.FromArgb(40, 40, 43)
                        GroupBox2.ForeColor = Color.White
                        GroupBox3.BackColor = Color.FromArgb(40, 40, 43)
                        GroupBox3.ForeColor = Color.White
                        Button1.FlatStyle = FlatStyle.Flat
                        Button2.FlatStyle = FlatStyle.Flat
                        Button3.FlatStyle = FlatStyle.Flat
                        Button4.FlatStyle = FlatStyle.Flat
                        Button5.FlatStyle = FlatStyle.Flat
                        Button6.FlatStyle = FlatStyle.Flat
                        Button7.FlatStyle = FlatStyle.Flat
                        Button8.FlatStyle = FlatStyle.Flat
                        Button9.FlatStyle = FlatStyle.Flat
                        Button10.FlatStyle = FlatStyle.Flat
                        Button11.FlatStyle = FlatStyle.Flat
                        Button12.FlatStyle = FlatStyle.Flat
                        Button13.FlatStyle = FlatStyle.Flat
                        ToolStripButton2.Image = New Bitmap(My.Resources.save_glyph_dark)
                        ToolStripButton3.Image = New Bitmap(My.Resources.prj_unload_glyph_dark)
                        ToolStripButton4.Image = New Bitmap(My.Resources.progress_window_dark)
                        RefreshViewTSB.Image = New Bitmap(My.Resources.refresh_glyph_dark)
                        Try
                            If prjTreeView.SelectedNode.IsExpanded Then
                                ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph_dark)
                            Else
                                ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph_dark)
                            End If
                        Catch ex As Exception
                            ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph_dark)
                        End Try
                        MenuStrip1.RenderMode = ToolStripRenderMode.Professional
                        MenuStrip1.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                        ToolStrip1.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                        ToolStrip2.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                        PkgInfoCMS.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                        ImgUMountPopupCMS.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                        AppxPackagePopupCMS.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                        AppxRelatedLinksCMS.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                        TreeViewCMS.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                        AppxResCMS.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                        PkgInfoCMS.ForeColor = Color.White
                        ImgUMountPopupCMS.ForeColor = Color.White
                        AppxPackagePopupCMS.ForeColor = Color.White
                        AppxRelatedLinksCMS.ForeColor = Color.White
                        TreeViewCMS.ForeColor = Color.White
                        AppxResCMS.ForeColor = Color.White
                        Dim items = TreeViewCMS.Items
                        Dim mItem As IEnumerable(Of ToolStripMenuItem) = Enumerable.OfType(Of ToolStripMenuItem)(items)
                        For Each item As ToolStripDropDownItem In mItem
                            If item.DropDownItems.Count > 0 Then
                                Dim ditems = item.DropDownItems
                                Dim dmItem As IEnumerable(Of ToolStripMenuItem) = Enumerable.OfType(Of ToolStripMenuItem)(ditems)
                                Try
                                    For Each dropDownItem As ToolStripDropDownItem In dmItem
                                        dropDownItem.BackColor = Color.FromArgb(27, 27, 28)
                                        dropDownItem.ForeColor = Color.White
                                    Next
                                Catch ex As Exception
                                    Continue For
                                End Try
                            End If
                        Next
                        InvalidSettingsTSMI.Image = New Bitmap(My.Resources.setting_error_glyph_dark)
                        BranchTSMI.Image = New Bitmap(My.Resources.branch_dark)
                        ' New design stuff
                        FlowLayoutPanel1.BackColor = Color.FromArgb(48, 48, 48)
                        GroupBox4.ForeColor = Color.White
                        GroupBox5.ForeColor = Color.White
                        GroupBox6.ForeColor = Color.White
                        GroupBox7.ForeColor = Color.White
                        GroupBox8.ForeColor = Color.White
                        GroupBox9.ForeColor = Color.White
                        GroupBox10.ForeColor = Color.White
                    ElseIf ColorMode = "1" Then
                        If IsWindowsVersionOrGreater(10, 0, 18362) Then EnableDarkTitleBar(Handle, False)
                        BackColor = Color.FromArgb(239, 239, 242)
                        ForeColor = Color.Black
                        HomePanel.BackColor = Color.White
                        HomePanel.ForeColor = Color.Black
                        SidePanel.BackColor = Color.FromArgb(230, 230, 230)
                        SidePanel.ForeColor = Color.Black
                        WelcomePanel.BackColor = Color.White
                        WelcomePanel.ForeColor = Color.Black
                        PrjPanel.BackColor = Color.FromArgb(240, 240, 240)
                        PrjPanel.ForeColor = Color.Black
                        MenuStrip1.BackColor = Color.FromArgb(239, 239, 242)
                        MenuStrip1.ForeColor = Color.Black
                        For Each item As ToolStripDropDownItem In MenuStrip1.Items
                            item.DropDown.BackColor = Color.FromArgb(231, 232, 236)
                            item.DropDown.ForeColor = Color.Black
                            Try
                                For Each dropDownItem As ToolStripDropDownItem In item.DropDownItems
                                    dropDownItem.DropDown.BackColor = Color.FromArgb(231, 232, 236)
                                    dropDownItem.DropDown.ForeColor = Color.Black
                                Next
                            Catch ex As Exception
                                Continue For
                            End Try
                        Next
                        StatusStrip.BackColor = Color.FromArgb(0, 122, 204)
                        StatusStrip.ForeColor = Color.White
                        TabPage1.BackColor = Color.White
                        TabPage1.ForeColor = Color.Black
                        TabPage2.BackColor = Color.White
                        TabPage2.ForeColor = Color.Black
                        TabPage3.BackColor = Color.White
                        TabPage3.ForeColor = Color.Black
                        WelcomeTab.BackColor = Color.White
                        WelcomeTab.ForeColor = Color.Black
                        NewsFeedTab.BackColor = Color.White
                        NewsFeedTab.ForeColor = Color.Black
                        VideosTab.BackColor = Color.White
                        VideosTab.ForeColor = Color.Black
                        PictureBox5.Image = New Bitmap(My.Resources.logo_mainscr_light)
                        ToolStrip1.BackColor = Color.FromArgb(239, 239, 242)
                        ToolStrip1.ForeColor = Color.Black
                        ToolStrip2.BackColor = Color.FromArgb(239, 239, 242)
                        ToolStrip2.ForeColor = Color.Black
                        prjTreeView.BackColor = Color.FromArgb(246, 246, 246)
                        prjTreeView.ForeColor = Color.Black
                        GroupBox1.BackColor = Color.White
                        GroupBox1.ForeColor = Color.Black
                        GroupBox2.BackColor = Color.White
                        GroupBox2.ForeColor = Color.Black
                        GroupBox3.BackColor = Color.White
                        GroupBox3.ForeColor = Color.Black
                        Button1.FlatStyle = FlatStyle.Standard
                        Button2.FlatStyle = FlatStyle.Standard
                        Button3.FlatStyle = FlatStyle.Standard
                        Button4.FlatStyle = FlatStyle.Standard
                        Button5.FlatStyle = FlatStyle.Standard
                        Button6.FlatStyle = FlatStyle.Standard
                        Button7.FlatStyle = FlatStyle.Standard
                        Button8.FlatStyle = FlatStyle.Standard
                        Button9.FlatStyle = FlatStyle.Standard
                        Button10.FlatStyle = FlatStyle.Standard
                        Button11.FlatStyle = FlatStyle.Standard
                        Button12.FlatStyle = FlatStyle.Standard
                        Button13.FlatStyle = FlatStyle.Standard
                        ToolStripButton2.Image = New Bitmap(My.Resources.save_glyph)
                        ToolStripButton3.Image = New Bitmap(My.Resources.prj_unload_glyph)
                        ToolStripButton4.Image = New Bitmap(My.Resources.progress_window)
                        RefreshViewTSB.Image = New Bitmap(My.Resources.refresh_glyph)
                        Try
                            If prjTreeView.SelectedNode.IsExpanded Then
                                ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph)
                            Else
                                ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph)
                            End If
                        Catch ex As Exception
                            ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph)
                        End Try
                        MenuStrip1.RenderMode = ToolStripRenderMode.Professional
                        MenuStrip1.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                        ToolStrip1.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                        ToolStrip2.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                        PkgInfoCMS.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                        ImgUMountPopupCMS.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                        AppxPackagePopupCMS.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                        AppxRelatedLinksCMS.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                        TreeViewCMS.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                        AppxResCMS.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                        PkgInfoCMS.ForeColor = Color.Black
                        ImgUMountPopupCMS.ForeColor = Color.Black
                        AppxPackagePopupCMS.ForeColor = Color.Black
                        AppxRelatedLinksCMS.ForeColor = Color.Black
                        TreeViewCMS.ForeColor = Color.Black
                        AppxResCMS.ForeColor = Color.Black
                        Dim items = TreeViewCMS.Items
                        Dim mItem As IEnumerable(Of ToolStripMenuItem) = Enumerable.OfType(Of ToolStripMenuItem)(items)
                        For Each item As ToolStripDropDownItem In mItem
                            If item.DropDownItems.Count > 0 Then
                                Dim ditems = item.DropDownItems
                                Dim dmItem As IEnumerable(Of ToolStripMenuItem) = Enumerable.OfType(Of ToolStripMenuItem)(ditems)
                                Try
                                    For Each dropDownItem As ToolStripDropDownItem In dmItem
                                        dropDownItem.BackColor = Color.FromArgb(231, 232, 236)
                                        dropDownItem.ForeColor = Color.Black
                                    Next
                                Catch ex As Exception
                                    Continue For
                                End Try
                            End If
                        Next
                        InvalidSettingsTSMI.Image = New Bitmap(My.Resources.setting_error_glyph)
                        BranchTSMI.Image = New Bitmap(My.Resources.branch)
                        ' New design stuff
                        FlowLayoutPanel1.BackColor = Color.FromArgb(239, 239, 242)
                        GroupBox4.ForeColor = Color.Black
                        GroupBox5.ForeColor = Color.Black
                        GroupBox6.ForeColor = Color.Black
                        GroupBox7.ForeColor = Color.Black
                        GroupBox8.ForeColor = Color.Black
                        GroupBox9.ForeColor = Color.Black
                        GroupBox10.ForeColor = Color.Black
                    End If
                Catch ex As Exception
                    ChangePrgColors(1)
                End Try
            Case 1
                If IsWindowsVersionOrGreater(10, 0, 18362) Then EnableDarkTitleBar(Handle, False)
                BackColor = Color.FromArgb(239, 239, 242)
                ForeColor = Color.Black
                HomePanel.BackColor = Color.White
                HomePanel.ForeColor = Color.Black
                SidePanel.BackColor = Color.FromArgb(230, 230, 230)
                SidePanel.ForeColor = Color.Black
                WelcomePanel.BackColor = Color.White
                WelcomePanel.ForeColor = Color.Black
                PrjPanel.BackColor = Color.FromArgb(240, 240, 240)
                PrjPanel.ForeColor = Color.Black
                MenuStrip1.BackColor = Color.FromArgb(239, 239, 242)
                MenuStrip1.ForeColor = Color.Black
                For Each item As ToolStripDropDownItem In MenuStrip1.Items
                    item.DropDown.BackColor = Color.FromArgb(231, 232, 236)
                    item.DropDown.ForeColor = Color.Black
                    Try
                        For Each dropDownItem As ToolStripDropDownItem In item.DropDownItems
                            dropDownItem.DropDown.BackColor = Color.FromArgb(231, 232, 236)
                            dropDownItem.DropDown.ForeColor = Color.Black
                        Next
                    Catch ex As Exception
                        Continue For
                    End Try
                Next
                StatusStrip.BackColor = Color.FromArgb(0, 122, 204)
                StatusStrip.ForeColor = Color.White
                TabPage1.BackColor = Color.White
                TabPage1.ForeColor = Color.Black
                TabPage2.BackColor = Color.White
                TabPage2.ForeColor = Color.Black
                TabPage3.BackColor = Color.White
                TabPage3.ForeColor = Color.Black
                WelcomeTab.BackColor = Color.White
                WelcomeTab.ForeColor = Color.Black
                NewsFeedTab.BackColor = Color.White
                NewsFeedTab.ForeColor = Color.Black
                VideosTab.BackColor = Color.White
                VideosTab.ForeColor = Color.Black
                PictureBox5.Image = New Bitmap(My.Resources.logo_mainscr_light)
                ToolStrip1.BackColor = Color.FromArgb(239, 239, 242)
                ToolStrip1.ForeColor = Color.Black
                ToolStrip2.BackColor = Color.FromArgb(239, 239, 242)
                ToolStrip2.ForeColor = Color.Black
                prjTreeView.BackColor = Color.FromArgb(246, 246, 246)
                prjTreeView.ForeColor = Color.Black
                GroupBox1.BackColor = Color.White
                GroupBox1.ForeColor = Color.Black
                GroupBox2.BackColor = Color.White
                GroupBox2.ForeColor = Color.Black
                GroupBox3.BackColor = Color.White
                GroupBox3.ForeColor = Color.Black
                Button1.FlatStyle = FlatStyle.Standard
                Button2.FlatStyle = FlatStyle.Standard
                Button3.FlatStyle = FlatStyle.Standard
                Button4.FlatStyle = FlatStyle.Standard
                Button5.FlatStyle = FlatStyle.Standard
                Button6.FlatStyle = FlatStyle.Standard
                Button7.FlatStyle = FlatStyle.Standard
                Button8.FlatStyle = FlatStyle.Standard
                Button9.FlatStyle = FlatStyle.Standard
                Button10.FlatStyle = FlatStyle.Standard
                Button11.FlatStyle = FlatStyle.Standard
                Button12.FlatStyle = FlatStyle.Standard
                Button13.FlatStyle = FlatStyle.Standard
                ToolStripButton2.Image = New Bitmap(My.Resources.save_glyph)
                ToolStripButton3.Image = New Bitmap(My.Resources.prj_unload_glyph)
                ToolStripButton4.Image = New Bitmap(My.Resources.progress_window)
                RefreshViewTSB.Image = New Bitmap(My.Resources.refresh_glyph)
                Try
                    If prjTreeView.SelectedNode.IsExpanded Then
                        ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph)
                    Else
                        ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph)
                    End If
                Catch ex As Exception
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph)
                End Try
                MenuStrip1.RenderMode = ToolStripRenderMode.Professional
                MenuStrip1.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                ToolStrip1.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                ToolStrip2.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                PkgInfoCMS.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                ImgUMountPopupCMS.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                AppxPackagePopupCMS.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                AppxRelatedLinksCMS.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                TreeViewCMS.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                AppxResCMS.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                PkgInfoCMS.ForeColor = Color.Black
                ImgUMountPopupCMS.ForeColor = Color.Black
                AppxPackagePopupCMS.ForeColor = Color.Black
                AppxRelatedLinksCMS.ForeColor = Color.Black
                TreeViewCMS.ForeColor = Color.Black
                AppxResCMS.ForeColor = Color.Black
                Dim items = TreeViewCMS.Items
                Dim mItem As IEnumerable(Of ToolStripMenuItem) = Enumerable.OfType(Of ToolStripMenuItem)(items)
                For Each item As ToolStripDropDownItem In mItem
                    If item.DropDownItems.Count > 0 Then
                        Dim ditems = item.DropDownItems
                        Dim dmItem As IEnumerable(Of ToolStripMenuItem) = Enumerable.OfType(Of ToolStripMenuItem)(ditems)
                        Try
                            For Each dropDownItem As ToolStripDropDownItem In dmItem
                                dropDownItem.BackColor = Color.FromArgb(231, 232, 236)
                                dropDownItem.ForeColor = Color.Black
                            Next
                        Catch ex As Exception
                            Continue For
                        End Try
                    End If
                Next
                InvalidSettingsTSMI.Image = New Bitmap(My.Resources.setting_error_glyph)
                BranchTSMI.Image = New Bitmap(My.Resources.branch)
                ' New design stuff
                FlowLayoutPanel1.BackColor = Color.FromArgb(239, 239, 242)
                GroupBox4.ForeColor = Color.Black
                GroupBox5.ForeColor = Color.Black
                GroupBox6.ForeColor = Color.Black
                GroupBox7.ForeColor = Color.Black
                GroupBox8.ForeColor = Color.Black
                GroupBox9.ForeColor = Color.Black
                GroupBox10.ForeColor = Color.Black
            Case 2
                If IsWindowsVersionOrGreater(10, 0, 18362) Then EnableDarkTitleBar(Handle, True)
                BackColor = Color.FromArgb(48, 48, 48)
                ForeColor = Color.White
                HomePanel.BackColor = Color.FromArgb(40, 40, 43)
                HomePanel.ForeColor = Color.White
                SidePanel.BackColor = Color.FromArgb(31, 31, 34)
                SidePanel.ForeColor = Color.White
                WelcomePanel.BackColor = Color.FromArgb(40, 40, 43)
                WelcomePanel.ForeColor = Color.White
                PrjPanel.BackColor = Color.FromArgb(40, 40, 43)
                PrjPanel.ForeColor = Color.White
                MenuStrip1.BackColor = Color.FromArgb(48, 48, 48)
                MenuStrip1.ForeColor = Color.White
                For Each item As ToolStripDropDownItem In MenuStrip1.Items
                    item.DropDown.BackColor = Color.FromArgb(27, 27, 28)
                    item.DropDown.ForeColor = Color.White
                    Try
                        For Each dropDownItem As ToolStripDropDownItem In item.DropDownItems
                            dropDownItem.DropDown.BackColor = Color.FromArgb(27, 27, 28)
                            dropDownItem.DropDown.ForeColor = Color.White
                        Next
                    Catch ex As Exception
                        Continue For
                    End Try
                Next
                StatusStrip.BackColor = Color.FromArgb(0, 122, 204)
                StatusStrip.ForeColor = Color.White
                TabPage1.BackColor = Color.FromArgb(40, 40, 43)
                TabPage1.ForeColor = Color.White
                TabPage2.BackColor = Color.FromArgb(40, 40, 43)
                TabPage2.ForeColor = Color.White
                TabPage3.BackColor = Color.FromArgb(40, 40, 43)
                TabPage3.ForeColor = Color.White
                WelcomeTab.BackColor = Color.FromArgb(40, 40, 43)
                WelcomeTab.ForeColor = Color.White
                NewsFeedTab.BackColor = Color.FromArgb(40, 40, 43)
                NewsFeedTab.ForeColor = Color.White
                VideosTab.BackColor = Color.FromArgb(40, 40, 43)
                VideosTab.ForeColor = Color.White
                PictureBox5.Image = New Bitmap(My.Resources.logo_mainscr_dark)
                ToolStrip1.BackColor = Color.FromArgb(48, 48, 48)
                ToolStrip1.ForeColor = Color.White
                ToolStrip2.BackColor = Color.FromArgb(48, 48, 48)
                ToolStrip2.ForeColor = Color.White
                prjTreeView.BackColor = Color.FromArgb(37, 37, 38)
                prjTreeView.ForeColor = Color.White
                GroupBox1.BackColor = Color.FromArgb(40, 40, 43)
                GroupBox1.ForeColor = Color.White
                GroupBox2.BackColor = Color.FromArgb(40, 40, 43)
                GroupBox2.ForeColor = Color.White
                GroupBox3.BackColor = Color.FromArgb(40, 40, 43)
                GroupBox3.ForeColor = Color.White
                Button1.FlatStyle = FlatStyle.Flat
                Button2.FlatStyle = FlatStyle.Flat
                Button3.FlatStyle = FlatStyle.Flat
                Button4.FlatStyle = FlatStyle.Flat
                Button5.FlatStyle = FlatStyle.Flat
                Button6.FlatStyle = FlatStyle.Flat
                Button7.FlatStyle = FlatStyle.Flat
                Button8.FlatStyle = FlatStyle.Flat
                Button9.FlatStyle = FlatStyle.Flat
                Button10.FlatStyle = FlatStyle.Flat
                Button11.FlatStyle = FlatStyle.Flat
                Button12.FlatStyle = FlatStyle.Flat
                Button13.FlatStyle = FlatStyle.Flat
                ToolStripButton2.Image = New Bitmap(My.Resources.save_glyph_dark)
                ToolStripButton3.Image = New Bitmap(My.Resources.prj_unload_glyph_dark)
                ToolStripButton4.Image = New Bitmap(My.Resources.progress_window_dark)
                RefreshViewTSB.Image = New Bitmap(My.Resources.refresh_glyph_dark)
                Try
                    If prjTreeView.SelectedNode.IsExpanded Then
                        ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph_dark)
                    Else
                        ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph_dark)
                    End If
                Catch ex As Exception
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph_dark)
                End Try
                MenuStrip1.RenderMode = ToolStripRenderMode.Professional
                MenuStrip1.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                ToolStrip1.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                ToolStrip2.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                PkgInfoCMS.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                ImgUMountPopupCMS.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                AppxPackagePopupCMS.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                AppxRelatedLinksCMS.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                TreeViewCMS.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                AppxResCMS.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                PkgInfoCMS.ForeColor = Color.White
                ImgUMountPopupCMS.ForeColor = Color.White
                AppxPackagePopupCMS.ForeColor = Color.White
                AppxRelatedLinksCMS.ForeColor = Color.White
                TreeViewCMS.ForeColor = Color.White
                AppxResCMS.ForeColor = Color.White
                Dim items = TreeViewCMS.Items
                Dim mItem As IEnumerable(Of ToolStripMenuItem) = Enumerable.OfType(Of ToolStripMenuItem)(items)
                For Each item As ToolStripDropDownItem In mItem
                    If item.DropDownItems.Count > 0 Then
                        Dim ditems = item.DropDownItems
                        Dim dmItem As IEnumerable(Of ToolStripMenuItem) = Enumerable.OfType(Of ToolStripMenuItem)(ditems)
                        Try
                            For Each dropDownItem As ToolStripDropDownItem In dmItem
                                dropDownItem.BackColor = Color.FromArgb(27, 27, 28)
                                dropDownItem.ForeColor = Color.White
                            Next
                        Catch ex As Exception
                            Continue For
                        End Try
                    End If
                Next
                InvalidSettingsTSMI.Image = New Bitmap(My.Resources.setting_error_glyph_dark)
                BranchTSMI.Image = New Bitmap(My.Resources.branch_dark)
                ' New design stuff
                FlowLayoutPanel1.BackColor = Color.FromArgb(48, 48, 48)
                GroupBox4.ForeColor = Color.White
                GroupBox5.ForeColor = Color.White
                GroupBox6.ForeColor = Color.White
                GroupBox7.ForeColor = Color.White
                GroupBox8.ForeColor = Color.White
                GroupBox9.ForeColor = Color.White
                GroupBox10.ForeColor = Color.White
        End Select
        If EnableExperiments Then
            If GetStartedPanel.Visible Then
                LinkLabel22.LinkColor = ForeColor
                LinkLabel23.LinkColor = Color.FromArgb(153, 153, 153)
                LinkLabel24.LinkColor = Color.FromArgb(153, 153, 153)
            ElseIf LatestNewsPanel.Visible Then
                LinkLabel22.LinkColor = Color.FromArgb(153, 153, 153)
                LinkLabel23.LinkColor = ForeColor
                LinkLabel24.LinkColor = Color.FromArgb(153, 153, 153)
            End If
            ListView1.BackColor = BackColor
            ListView1.ForeColor = ForeColor
        End If
    End Sub

    Sub ChangeLangs(LangCode As Integer)
        Select Case LangCode
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        ' Top-level menu items
                        FileToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&File".ToUpper(), "&File")
                        ProjectToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Project".ToUpper(), "&Project")
                        CommandsToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "Com&mands".ToUpper(), "Com&mands")
                        ToolsToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Tools".ToUpper(), "&Tools")
                        HelpToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Help".ToUpper(), "&Help")
                        InvalidSettingsTSMI.Text = "Invalid settings have been detected"
                        ' Submenu items
                        ' Menu - File
                        NewProjectToolStripMenuItem.Text = "&New project..."
                        OpenExistingProjectToolStripMenuItem.Text = "&Open existing project"
                        ManageOnlineInstallationToolStripMenuItem.Text = "&Manage online installation"
                        ManageOfflineInstallationToolStripMenuItem.Text = "Manage o&ffline installation..."
                        SaveProjectToolStripMenuItem.Text = "&Save project..."
                        SaveProjectasToolStripMenuItem.Text = "Save project &as..."
                        ExitToolStripMenuItem.Text = "E&xit"
                        ' Menu - Project
                        ViewProjectFilesInFileExplorerToolStripMenuItem.Text = "View project files in File Explorer"
                        UnloadProjectToolStripMenuItem.Text = "Unload project..."
                        SwitchImageIndexesToolStripMenuItem.Text = "Switch image indexes..."
                        ProjectPropertiesToolStripMenuItem.Text = "Project properties"
                        ImagePropertiesToolStripMenuItem.Text = "Image properties"
                        ' Menu - Commands
                        ImageManagementToolStripMenuItem.Text = "Image management"
                        OSPackagesToolStripMenuItem.Text = "OS packages"
                        ProvisioningPackagesToolStripMenuItem.Text = "Provisioning packages"
                        AppPackagesToolStripMenuItem.Text = "App packages"
                        AppPatchesToolStripMenuItem.Text = "App (MSP) servicing"
                        DefaultAppAssociationsToolStripMenuItem.Text = "Default app associations"
                        LanguagesAndRegionSettingsToolStripMenuItem.Text = "Languages and regional settings"
                        CapabilitiesToolStripMenuItem.Text = "Capabilities"
                        WindowsEditionsToolStripMenuItem.Text = "Windows editions"
                        DriversToolStripMenuItem.Text = "Drivers"
                        UnattendedAnswerFilesToolStripMenuItem.Text = "Unattended answer files"
                        WindowsPEServicingToolStripMenuItem.Text = "Windows PE servicing"
                        OSUninstallToolStripMenuItem.Text = "OS uninstall"
                        ReservedStorageToolStripMenuItem.Text = "Reserved storage"
                        ' Menu - Commands - Image management
                        AppendImage.Text = "Append capture directory to image..."
                        ApplyFFU.Text = "Apply FFU or SFU file..."
                        ApplyImage.Text = "Apply WIM or SWM file..."
                        CaptureCustomImage.Text = "Capture incremental changes to file..."
                        CaptureFFU.Text = "Capture partitions to FFU file..."
                        CaptureImage.Text = "Capture image of a drive to WIM file..."
                        CleanupMountpoints.Text = "Delete resources from corrupted image..."
                        CommitImage.Text = "Apply changes to image..."
                        DeleteImage.Text = "Delete volume images from WIM file..."
                        ExportImage.Text = "Export image..."
                        GetImageInfo.Text = "Get image information..."
                        GetWIMBootEntry.Text = "Get WIMBoot configuration entries..."
                        ListImage.Text = "List files and directories in image..."
                        MountImage.Text = "Mount image..."
                        OptimizeFFU.Text = "Optimize FFU file..."
                        OptimizeImage.Text = "Optimize image..."
                        RemountImage.Text = "Remount image for servicing..."
                        SplitFFU.Text = "Split FFU file into SFU files..."
                        SplitImage.Text = "Split WIM file into SWM files..."
                        UnmountImage.Text = "Unmount image..."
                        UpdateWIMBootEntry.Text = "Update WIMBoot configuration entry..."
                        ApplySiloedPackage.Text = "Apply siloed provisioning package..."
                        ' Menu - Commands - OS packages
                        GetPackages.Text = "Get package information..."
                        AddPackage.Text = "Add package..."
                        RemovePackage.Text = "Remove package..."
                        GetFeatures.Text = "Get feature information..."
                        EnableFeature.Text = "Enable feature..."
                        DisableFeature.Text = "Disable feature..."
                        CleanupImage.Text = "Perform cleanup or recovery operations..."
                        SaveImageInformationToolStripMenuItem.Text = "Save image information..."
                        ' Menu - Commands - Provisioning packages
                        AddProvisioningPackage.Text = "Add provisioning package..."
                        GetProvisioningPackageInfo.Text = "Get provisioning package information..."
                        ApplyCustomDataImage.Text = "Apply custom data image..."
                        ' Menu - Commands - App packages
                        GetProvisionedAppxPackages.Text = "Get app package information..."
                        AddProvisionedAppxPackage.Text = "Add provisioned app package..."
                        RemoveProvisionedAppxPackage.Text = "Remove provisioning for app package..."
                        OptimizeProvisionedAppxPackages.Text = "Optimize provisioned packages..."
                        SetProvisionedAppxDataFile.Text = "Add custom data file into app package..."
                        ' Menu - Commands - App (MSP) servicing
                        CheckAppPatch.Text = "Get application patch information..."
                        GetAppPatchInfo.Text = "Get detailed application patch information..."
                        GetAppPatches.Text = "Get basic installed application patch information..."
                        GetAppInfo.Text = "Get detailed Windows Installer (*.msi) application information..."
                        GetApps.Text = "Get basic Windows Installer (*.msi) application information..."
                        ' Menu - Commands - Default app associations
                        ExportDefaultAppAssociations.Text = "Export default application associations..."
                        GetDefaultAppAssociations.Text = "Get default application association information..."
                        ImportDefaultAppAssociations.Text = "Import default application associations..."
                        RemoveDefaultAppAssociations.Text = "Remove default application associations..."
                        ' Menu - Commands - Languages and regional settings
                        GetIntl.Text = "Get international settings and languages..."
                        SetUILang.Text = "Set UI language..."
                        SetUILangFallback.Text = "Set default UI fallback language..."
                        SetSysUILang.Text = "Set system preferred UI language..."
                        SetSysLocale.Text = "Set system locale..."
                        SetUserLocale.Text = "Set user locale..."
                        SetInputLocale.Text = "Set input locale..."
                        SetAllIntl.Text = "Set UI language and locales..."
                        SetTimeZone.Text = "Set default time zone..."
                        SetSKUIntlDefaults.Text = "Set default languages and locales..."
                        SetLayeredDriver.Text = "Set layered driver..."
                        GenLangINI.Text = "Generate Lang.ini file..."
                        SetSetupUILang.Text = "Set default Setup language..."
                        ' Menu - Commands - Capabilities
                        AddCapability.Text = "Add capability..."
                        ExportSource.Text = "Export capabilities into repository..."
                        GetCapabilities.Text = "Get capability information..."
                        RemoveCapability.Text = "Remove capability..."
                        ' Menu - Commands - Windows editions
                        GetCurrentEdition.Text = "Get current edition..."
                        GetTargetEditions.Text = "Get upgrade targets..."
                        SetEdition.Text = "Upgrade image..."
                        SetProductKey.Text = "Set product key..."
                        ' Menu - Commands - Drivers
                        GetDrivers.Text = "Get driver information..."
                        AddDriver.Text = "Add driver..."
                        RemoveDriver.Text = "Remove driver..."
                        ExportDriver.Text = "Export driver packages..."
                        ' Menu - Commands - Unattended answer files
                        ApplyUnattend.Text = "Apply unattended answer file..."
                        ' Menu - Commands - Windows PE servicing
                        GetPESettings.Text = "Get settings..."
                        SetScratchSpace.Text = "Set scratch space..."
                        SetTargetPath.Text = "Set target path..."
                        ' Menu - Commands - OS uninstall
                        GetOSUninstallWindow.Text = "Get uninstall window..."
                        InitiateOSUninstall.Text = "Initiate uninstall..."
                        RemoveOSUninstall.Text = "Remove roll back ability..."
                        SetOSUninstallWindow.Text = "Set uninstall window..."
                        ' Menu - Commands - Reserved storage
                        SetReservedStorageState.Text = "Set reserved storage state..."
                        GetReservedStorageState.Text = "Get reserved storage state..."
                        ' Menu - Commands - Microsoft Edge
                        AddEdge.Text = "Add Edge..."
                        AddEdgeBrowser.Text = "Add Edge browser..."
                        AddEdgeWebView.Text = "Add Edge WebView..."
                        ' Menu - Tools
                        ImageConversionToolStripMenuItem.Text = "Image conversion"
                        MergeSWM.Text = "Merge SWM files..."
                        RemountImageWithWritePermissionsToolStripMenuItem.Text = "Remount image with write permissions"
                        CommandShellToolStripMenuItem.Text = "Command Console"
                        UnattendedAnswerFileManagerToolStripMenuItem.Text = "Unattended answer file manager"
                        ReportManagerToolStripMenuItem.Text = "Report manager"
                        MountedImageManagerTSMI.Text = "Mounted image manager"
                        WimScriptEditorCommand.Text = "Configuration list editor"
                        ActionEditorToolStripMenuItem.Text = "Action editor"
                        OptionsToolStripMenuItem.Text = "Options"
                        ' Menu - Help
                        HelpTopicsToolStripMenuItem.Text = "Help Topics"
                        GlossaryToolStripMenuItem.Text = "Glossary"
                        CommandHelpToolStripMenuItem.Text = "Command help..."
                        AboutDISMToolsToolStripMenuItem.Text = "About DISMTools"
                        ' Menu - Invalid settings
                        ISFix.Text = "More information"
                        ISHelp.Text = "What's this?"
                        ' Menu - DevState
                        ReportFeedbackToolStripMenuItem.Text = "Report feedback (opens in web browser)"
                        ' Menu - Contributions
                        ContributeToTheHelpSystemToolStripMenuItem.Text = "Contribute to the help system"
                        ' Start Panel
                        LabelHeader1.Text = "Begin"
                        Label10.Text = "Recent projects"
                        Label11.Text = "Coming soon!"
                        NewProjLink.Text = "New project..."
                        ExistingProjLink.Text = "Open existing project..."
                        OnlineInstMgmt.Text = "Manage online installation"
                        OfflineInstMgmt.Text = "Manage offline installation..."
                        ' Start Panel tabs
                        WelcomeTab.Text = "Welcome"
                        NewsFeedTab.Text = "Latest news"
                        VideosTab.Text = "Tutorial videos"
                        ' Welcome tab contents
                        Label24.Text = "Welcome to DISMTools"
                        Label25.Text = "The graphical front-end to perform DISM operations."
                        Label26.Text = "This is beta software"
                        Label27.Text = "Currently, this program is in beta. This means lots of things will not work as expected. There will also be lots of bugs, and, generally, the program is incomplete (as you can see right now)"
                        Label28.Text = "This program is open-source"
                        Label29.Text = "This program is open-source, meaning you can take a look at how it works and understand it better."
                        Label30.Text = "Be sure to know what you are doing"
                        Label31.Text = "Please be careful over what you do in this program. Although it may look simple and easy to use, it is targeted to system administrators and people in the IT department. If you carelessly perform operations to an image or an active installation, it may not work correctly or may refuse to work."
                        Label32.Text = "Getting started"
                        Label33.Text = "- Did you get lost in this program? We recommend checking the help topics to get around" & CrLf & _
                            "- Don't understand a specific term? We recommend checking the glossary. The terms are alphabetically sorted and provide explanations as detailed as possible" & CrLf & _
                            "- Want to know how to do it in the command line? We recommmend checking the command help" & CrLf & _
                            "These options, and the program information, can be found by opening the Help menu."
                        ' ToolStrip buttons
                        ToolStripButton1.Text = "Close tab"
                        ToolStripButton2.Text = "Save project"
                        ToolStripButton3.Text = "Unload project"
                        ToolStripButton3.ToolTipText = "Unload project from this program"
                        ToolStripButton4.Text = "Show progress window"
                        RefreshViewTSB.Text = "Refresh view"
                        ExpandCollapseTSB.Text = "Expand"
                        ' TabPages
                        TabPage1.Text = "Project"
                        TabPage2.Text = "Image"
                        TabPage3.Text = "Actions"
                        ' TabPage controls
                        UnloadBtn.Text = "Unload project"
                        ExplorerView.Text = "View in File Explorer"
                        Button14.Text = "View project properties"
                        Button15.Text = "View image properties"
                        Button16.Text = "Unmount image..."
                        TabPageTitle1.Text = "Project"
                        TabPageTitle2.Text = "Image"
                        TabPageDescription1.Text = "View project information"
                        TabPageDescription2.Text = "View image information"
                        Label1.Text = "Name:"
                        Label2.Text = "Location:"
                        Label4.Text = "Images mounted?"
                        Label5.Text = If(IsImageMounted, "Yes", "No")
                        LinkLabel1.Text = "Click here to mount an image"
                        Label23.Text = "No image has been mounted"
                        LinkLabel2.Text = "You need to mount an image in order to view its information here. Click here to mount an image."
                        LinkLabel2.LinkArea = New LinkArea(72, 4)
                        LinkLabel3.Text = "Or, if you have a mounted image, open an existing mount directory"
                        LinkLabel3.LinkArea = New LinkArea(33, 32)
                        UpdateLink.Text = "A new version is available for download and installation. Click here to learn more"
                        UpdateLink.LinkArea = New LinkArea(58, 24)
                        Label15.Text = "Image index:"
                        Label13.Text = "Mount point:"
                        Label16.Text = "Version:"
                        Label19.Text = "Name:"
                        Label21.Text = "Description:"
                        ' Actions
                        GroupBox1.Text = "Image operations"
                        GroupBox2.Text = "Package operations"
                        GroupBox3.Text = "Feature operations"
                        Button1.Text = "Mount image..."
                        Button2.Text = "Commit current changes"
                        Button3.Text = "Commit and unmount image"
                        Button4.Text = "Unmount image discarding changes"
                        Button5.Text = "Add package..."
                        Button6.Text = "Get package information..."
                        Button7.Text = "Remove package..."
                        Button8.Text = "Get feature information..."
                        Button9.Text = "Disable feature..."
                        Button10.Text = "Enable feature..."
                        Button11.Text = "Reload servicing session..."
                        Button12.Text = "Perform component cleanup and/or repair..."
                        Button13.Text = "Switch indexes..."
                        Button19.Text = "Preview the new design"
                        Button20.Text = "Go back to the old design"
                        ' Pop-up context menus
                        PkgBasicInfo.Text = "Get basic information (all packages)"
                        PkgDetailedInfo.Text = "Get detailed information (specific package)"
                        CommitAndUnmountTSMI.Text = "Commit changes and unmount image"
                        DiscardAndUnmountTSMI.Text = "Discard changes and unmount image"
                        UnmountSettingsToolStripMenuItem.Text = "Unmount settings..."
                        ViewPackageDirectoryToolStripMenuItem.Text = "View package directory"
                        ' OpenFileDialogs and FolderBrowsers
                        OpenFileDialog1.Title = "Specify the project file to load"
                        LocalMountDirFBD.Description = "Please specify the mount directory you want to load into this project:"
                        If Not ImgBW.IsBusy And areBackgroundProcessesDone Then
                            BGProcDetails.Label2.Text = "Image processes have completed"
                        End If
                        MenuDesc.Text = "Ready"
                        ' Tree view context menu
                        AccessDirectoryToolStripMenuItem.Text = "Access directory"
                        UnloadProjectToolStripMenuItem1.Text = "Unload project"
                        CopyDeploymentToolsToolStripMenuItem.Text = "Copy deployment tools"
                        OfAllArchitecturesToolStripMenuItem.Text = "Of all architectures"
                        OfSelectedArchitectureToolStripMenuItem.Text = "Of selected architecture"
                        ForX86ArchitectureToolStripMenuItem.Text = "For x86 architecture"
                        ForAmd64ArchitectureToolStripMenuItem.Text = "For AMD64 architecture"
                        ForARMArchitectureToolStripMenuItem.Text = "For ARM architecture"
                        ForARM64ArchitectureToolStripMenuItem.Text = "For ARM64 architecture"
                        ImageOperationsToolStripMenuItem.Text = "Image operations"
                        MountImageToolStripMenuItem.Text = "Mount image..."
                        UnmountImageToolStripMenuItem.Text = "Unmount image..."
                        RemoveVolumeImagesToolStripMenuItem.Text = "Remove volume images..."
                        SwitchImageIndexesToolStripMenuItem1.Text = "Switch image indexes..."
                        UnattendedAnswerFilesToolStripMenuItem1.Text = "Unattended answer files"
                        ManageToolStripMenuItem.Text = "Manage"
                        CreationWizardToolStripMenuItem.Text = "Create"
                        ScratchDirectorySettingsToolStripMenuItem.Text = "Configure scratch directory"
                        ManageReportsToolStripMenuItem.Text = "Manage reports"
                        AddToolStripMenuItem.Text = "Add"
                        NewFileToolStripMenuItem.Text = "New file..."
                        ExistingFileToolStripMenuItem.Text = "Existing file..."
                        ' Context menu of AppX information dialog
                        SaveResourceToolStripMenuItem.Text = "Save resource..."
                        CopyToolStripMenuItem.Text = "Copy resource"
                        ' Context menu of AppX addition dialog
                        MicrosoftAppsToolStripMenuItem.Text = "Visit the Microsoft Apps website"
                        MicrosoftStoreGenerationProjectToolStripMenuItem.Text = "Visit the Microsoft Store Generation Project website"
                        ' New design
                        GreetingLabel.Text = "Welcome to this servicing session"
                        LinkLabel12.Text = "PROJECT"
                        LinkLabel13.Text = "IMAGE"
                        Label54.Text = "Name:"
                        Label51.Text = "Location:"
                        Label53.Text = "Images mounted?"
                        LinkLabel14.Text = "Click here to mount an image"
                        Label55.Text = "Project Tasks"
                        LinkLabel15.Text = "View project properties"
                        LinkLabel16.Text = "Open in File Explorer"
                        LinkLabel17.Text = "Unload project"
                        Label59.Text = "No image has been mounted"
                        Label58.Text = "You need to mount an image in order to view its information"
                        Label57.Text = "Choices"
                        LinkLabel21.Text = "Mount an image..."
                        LinkLabel18.Text = "Pick a mounted image..."
                        Label39.Text = "Image index:"
                        Label43.Text = "Mount point:"
                        Label45.Text = "Version:"
                        Label42.Text = "Name:"
                        Label40.Text = "Description:"
                        Label56.Text = "Image Tasks"
                        LinkLabel20.Text = "View image properties"
                        LinkLabel19.Text = "Unmount image"
                        GroupBox4.Text = "Image operations"
                        Button26.Text = "Mount image..."
                        Button27.Text = "Commit current changes"
                        Button28.Text = "Commit and unmount image"
                        Button29.Text = "Unmount image discarding changes"
                        Button25.Text = "Reload servicing session"
                        Button24.Text = "Switch image indexes..."
                        Button30.Text = "Apply image..."
                        Button31.Text = "Capture image..."
                        Button32.Text = "Remove volume images..."
                        Button33.Text = "Save complete image information..."
                        GroupBox5.Text = "Package operations"
                        Button36.Text = "Add package..."
                        Button34.Text = "Get package information..."
                        Button38.Text = "Save installed package information..."
                        Button35.Text = "Remove package..."
                        Button37.Text = "Perform component store maintenance and cleanup..."
                        GroupBox6.Text = "Feature operations"
                        Button41.Text = "Enable feature..."
                        Button39.Text = "Get feature information..."
                        Button42.Text = "Save feature information..."
                        Button40.Text = "Disable feature..."
                        GroupBox7.Text = "AppX package operations"
                        Button44.Text = "Add AppX package..."
                        Button45.Text = "Get app information..."
                        Button46.Text = "Save installed AppX package information..."
                        Button43.Text = "Remove AppX package..."
                        GroupBox8.Text = "Capability operations"
                        Button48.Text = "Add capability..."
                        Button49.Text = "Get capability information..."
                        Button50.Text = "Save capability information..."
                        Button47.Text = "Remove capability..."
                        GroupBox9.Text = "Driver operations"
                        Button53.Text = "Add driver package..."
                        Button52.Text = "Get driver information..."
                        Button54.Text = "Save installed driver information..."
                        Button51.Text = "Remove driver..."
                        GroupBox10.Text = "Windows PE operations"
                        Button55.Text = "Get configuration"
                        Button56.Text = "Save configuration..."
                        Button57.Text = "Set target path..."
                        Button58.Text = "Set scratch space..."
                    Case "ESN"
                        ' Top-level menu items
                        FileToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Archivo".ToUpper(), "&Archivo")
                        ProjectToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Proyecto".ToUpper(), "&Proyecto")
                        CommandsToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "Co&mandos".ToUpper(), "Co&mandos")
                        ToolsToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "Her&ramientas".ToUpper(), "Her&ramientas")
                        HelpToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "Ay&uda".ToUpper(), "Ay&uda")
                        InvalidSettingsTSMI.Text = "Se han detectado configuraciones inválidas"
                        ' Submenu items
                        ' Menu - File
                        NewProjectToolStripMenuItem.Text = "&Nuevo proyecto..."
                        OpenExistingProjectToolStripMenuItem.Text = "&Abrir proyecto existente"
                        ManageOnlineInstallationToolStripMenuItem.Text = "Administrar &instalación activa"
                        ManageOfflineInstallationToolStripMenuItem.Text = "Administrar instalación &fuera de línea..."
                        SaveProjectToolStripMenuItem.Text = "&Guardar proyecto..."
                        SaveProjectasToolStripMenuItem.Text = "Guardar proyecto &como..."
                        ExitToolStripMenuItem.Text = "Sa&lir"
                        ' Menu - Project
                        ViewProjectFilesInFileExplorerToolStripMenuItem.Text = "Ver archivos del proyecto en el Explorador de archivos"
                        UnloadProjectToolStripMenuItem.Text = "Descargar proyecto..."
                        SwitchImageIndexesToolStripMenuItem.Text = "Cambiar índices de imagen..."
                        ProjectPropertiesToolStripMenuItem.Text = "Propiedades del proyecto"
                        ImagePropertiesToolStripMenuItem.Text = "Propiedades de la imagen"
                        ' Menu - Commands
                        ImageManagementToolStripMenuItem.Text = "Administración de la imagen"
                        OSPackagesToolStripMenuItem.Text = "Paquetes del sistema operativo"
                        ProvisioningPackagesToolStripMenuItem.Text = "Paquetes de aprovisionamiento"
                        AppPackagesToolStripMenuItem.Text = "Paquetes de aplicación"
                        AppPatchesToolStripMenuItem.Text = "Servicio de aplicaciones (MSP)"
                        DefaultAppAssociationsToolStripMenuItem.Text = "Asociaciones predeterminadas de aplicaciones"
                        LanguagesAndRegionSettingsToolStripMenuItem.Text = "Configuración de idiomas y regiones"
                        CapabilitiesToolStripMenuItem.Text = "Funcionalidades"
                        WindowsEditionsToolStripMenuItem.Text = "Ediciones de Windows"
                        DriversToolStripMenuItem.Text = "Controladores"
                        UnattendedAnswerFilesToolStripMenuItem.Text = "Archivos de respuesta desatendida"
                        WindowsPEServicingToolStripMenuItem.Text = "Servicio de Windows PE"
                        OSUninstallToolStripMenuItem.Text = "Desinstalación del sistema operativo"
                        ReservedStorageToolStripMenuItem.Text = "Almacenamiento reservado"
                        ' Menu - Commands - Image management
                        AppendImage.Text = "Anexar directorio de captura a imagen..."
                        ApplyFFU.Text = "Aplicar archivo FFU o SFU..."
                        ApplyImage.Text = "Aplicar archivo WIM o SWM..."
                        CaptureCustomImage.Text = "Capturar cambios incrementales a un archivo..."
                        CaptureFFU.Text = "Capturar particiones a un archivo FFU..."
                        CaptureImage.Text = "Capturar imagen de un disco a un archivo WIM..."
                        CleanupMountpoints.Text = "Eliminar recursos de una imagen corrupta..."
                        CommitImage.Text = "Aplicar cambios a la imagen..."
                        DeleteImage.Text = "Eliminar imágenes de volumen de un archivo WIM..."
                        ExportImage.Text = "Exportar imagen..."
                        GetImageInfo.Text = "Obtener información de imagen..."
                        GetWIMBootEntry.Text = "Obtener entradas de configuración WIMBoot..."
                        ListImage.Text = "Enumerar archivos y directorios de un archivo WIM..."
                        MountImage.Text = "Montar imagen..."
                        OptimizeFFU.Text = "Optimizar archivo FFU..."
                        OptimizeImage.Text = "Optimizar imagen..."
                        RemountImage.Text = "Remontar imagen para su servicio..."
                        SplitFFU.Text = "Dividir archivo FFU en archivos SFU..."
                        SplitImage.Text = "Dividir archivo WIM en archivos SWM..."
                        UnmountImage.Text = "Desmontar imagen..."
                        UpdateWIMBootEntry.Text = "Actualizar entradas de configuración WIMBoot..."
                        ApplySiloedPackage.Text = "Aplicar paquete de aprovisionamiento en silos..."
                        SaveImageInformationToolStripMenuItem.Text = "Guardar información de la imagen..."
                        ' Menu - Commands - OS packages
                        GetPackages.Text = "Obtener información de paquetes..."
                        AddPackage.Text = "Añadir paquete..."
                        RemovePackage.Text = "Eliminar paquete..."
                        GetFeatures.Text = "Obtener información de características..."
                        EnableFeature.Text = "Habilitar característica..."
                        DisableFeature.Text = "Deshabilitar característica..."
                        CleanupImage.Text = "Realizar operaciones de limpieza o recuperación..."
                        ' Menu - Commands - Provisioning packages
                        AddProvisioningPackage.Text = "Añadir paquete de aprovisionamiento..."
                        GetProvisioningPackageInfo.Text = "Obtener información de paquete de aprovisionamiento..."
                        ApplyCustomDataImage.Text = "Aplicar imagen de datos personalizada..."
                        ' Menu - Commands - App packages
                        GetProvisionedAppxPackages.Text = "Obtener información de paquete de aplicación..."
                        AddProvisionedAppxPackage.Text = "Añadir paquete de aplicación aprovisionada..."
                        RemoveProvisionedAppxPackage.Text = "Eliminar aprovisionamiento para un paquete de aplicación..."
                        OptimizeProvisionedAppxPackages.Text = "Optimizar paquete de aprovisionamiento..."
                        SetProvisionedAppxDataFile.Text = "Añadir archivo de datos personalizado en paquete de aplicación..."
                        ' Menu - Commands - App (MSP) servicing
                        CheckAppPatch.Text = "Obtener información de parche de aplicación..."
                        GetAppPatchInfo.Text = "Obtener información detallada de parches de aplicación instalados..."
                        GetAppPatches.Text = "Obtener información básica de parches de aplicación instalados..."
                        GetAppInfo.Text = "Obtener información detallada de aplicaciones de Windows Installer (*.msi)..."
                        GetApps.Text = "Obtener información básica de aplicaciones de Windows Installer (*.msi)..."
                        ' Menu - Commands - Default app associations
                        ExportDefaultAppAssociations.Text = "Exportar asociaciones de aplicaciones predeterminadas..."
                        GetDefaultAppAssociations.Text = "Obtener información de asociaciones de aplicaciones predeterminadas..."
                        ImportDefaultAppAssociations.Text = "Importar asociaciones de aplicaciones predeterminadas..."
                        RemoveDefaultAppAssociations.Text = "Eliminar asociaciones de aplicaciones predeterminadas..."
                        ' Menu - Commands - Languages and regional settings
                        GetIntl.Text = "Obtener configuraciones e idiomas internacionales..."
                        SetUILang.Text = "Establecer idioma de la interfaz de usuario..."
                        SetUILangFallback.Text = "Establecer idioma predeterminado de la interfaz de usuario de último recurso..."
                        SetSysUILang.Text = "Estabñecer idioma de la interfaz de usuario preferido para el sistema..."
                        SetSysLocale.Text = "Establecer zona del sistema..."
                        SetUserLocale.Text = "Establecer zona del usuario..."
                        SetInputLocale.Text = "Establecer zona de entrada..."
                        SetAllIntl.Text = "Establecer idioma de la interfaz de usuario y zonas..."
                        SetTimeZone.Text = "Establecer zona horaria predeterminada..."
                        SetSKUIntlDefaults.Text = "Establecer lenguajes y zonas predeterminadas..."
                        SetLayeredDriver.Text = "Establecer controlador en capas..."
                        GenLangINI.Text = "Generar archivo Lang.ini..."
                        SetSetupUILang.Text = "Establecer idioma predeterminado del programa de instalación..."
                        ' Menu - Commands - Capabilities
                        AddCapability.Text = "Añadir funcionalidad..."
                        ExportSource.Text = "Exportar funcionalidades en un repositorio..."
                        GetCapabilities.Text = "Obtener información de funcionalidades..."
                        RemoveCapability.Text = "Eliminar funcionalidad..."
                        ' Menu - Commands - Windows editions
                        GetCurrentEdition.Text = "Obtener edición actual..."
                        GetTargetEditions.Text = "Obtener destinos de actualización..."
                        SetEdition.Text = "Actualizar imagen..."
                        SetProductKey.Text = "Establecer clave de producto..."
                        ' Menu - Commands - Drivers
                        GetDrivers.Text = "Obtener información de controladores..."
                        AddDriver.Text = "Añadir controlador..."
                        RemoveDriver.Text = "Eliminar controlador..."
                        ExportDriver.Text = "Exportar paquetes de controlador..."
                        ' Menu - Commands - Unattended answer files
                        ApplyUnattend.Text = "Aplicar archivo de respuesta desatendida..."
                        ' Menu - Commands - Windows PE servicing
                        GetPESettings.Text = "Obtener configuración..."
                        SetScratchSpace.Text = "Establecer espacio temporal..."
                        SetTargetPath.Text = "Establecer ruta de destino..."
                        ' Menu - Commands - OS uninstall
                        GetOSUninstallWindow.Text = "Obtener margen de desinstalación..."
                        InitiateOSUninstall.Text = "Iniciar desinstalación..."
                        RemoveOSUninstall.Text = "Eliminar habilidad de desinstalación..."
                        SetOSUninstallWindow.Text = "Establecer margen de desinstalación..."
                        ' Menu - Commands - Reserved storage
                        SetReservedStorageState.Text = "Establecer estado de almacenamiento reservado..."
                        GetReservedStorageState.Text = "Obtener estado de almacenamiento reservado..."
                        ' Menu - Commands - Microsoft Edge
                        AddEdge.Text = "Añadir Edge..."
                        AddEdgeBrowser.Text = "Añadir navegador Edge..."
                        AddEdgeWebView.Text = "Añadir Edge WebView..."
                        ' Menu - Tools
                        ImageConversionToolStripMenuItem.Text = "Conversión de imágenes"
                        MergeSWM.Text = "Combinar archivos SWM..."
                        RemountImageWithWritePermissionsToolStripMenuItem.Text = "Remontar imagen con permisos de escritura"
                        CommandShellToolStripMenuItem.Text = "Consola de comandos"
                        UnattendedAnswerFileManagerToolStripMenuItem.Text = "Administrador de archivos de respuesta desatendida"
                        ReportManagerToolStripMenuItem.Text = "Administrador de informes"
                        MountedImageManagerTSMI.Text = "Administrador de imágenes montadas"
                        WimScriptEditorCommand.Text = "Editor de lista de configuraciones"
                        ActionEditorToolStripMenuItem.Text = "Editor de acciones"
                        OptionsToolStripMenuItem.Text = "Opciones"
                        ' Menu - Help
                        HelpTopicsToolStripMenuItem.Text = "Ver la ayuda"
                        GlossaryToolStripMenuItem.Text = "Glosario"
                        CommandHelpToolStripMenuItem.Text = "Ayuda de comandos..."
                        AboutDISMToolsToolStripMenuItem.Text = "Acerca de DISMTools"
                        ' Menu - Invalid settings
                        ISFix.Text = "Más información"
                        ISHelp.Text = "¿Qué es esto?"
                        ' Menu - DevState
                        ReportFeedbackToolStripMenuItem.Text = "Enviar comentarios (se abre en navegador web)"
                        ' Menu - Contributions
                        ContributeToTheHelpSystemToolStripMenuItem.Text = "Contribuir al sistema de ayuda"
                        ' Start Panel
                        LabelHeader1.Text = "Comenzar"
                        Label10.Text = "Proyectos recientes"
                        Label11.Text = "¡Próximamente!"
                        NewProjLink.Text = "Nuevo proyecto..."
                        ExistingProjLink.Text = "Abrir proyecto existente..."
                        OnlineInstMgmt.Text = "Administrar instalación activa"
                        OfflineInstMgmt.Text = "Administrar instalación fuera de línea..."
                        ' Start Panel tabs
                        WelcomeTab.Text = "Bienvenido"
                        NewsFeedTab.Text = "Últimas noticias"
                        VideosTab.Text = "Tutoriales"
                        ' Welcome tab contents
                        Label24.Text = "Bienvenido a DISMTools"
                        Label25.Text = "La interfaz gráfica para realizar operaciones de DISM."
                        Label26.Text = "Este es un software en beta"
                        Label27.Text = "Ahora mismo este programa está en beta. Esto significa que muchas cosas no funcionarán como esperado. También habrán muchos errores y, en general, este programa no está completo (como puede ver ahora mismo)"
                        Label28.Text = "Este programa es de código abierto"
                        Label29.Text = "Este programa es de código abierto, lo que le permite observar cómo funciona y entenderlo mejor."
                        Label30.Text = "Asegúrese de saber lo que hace"
                        Label31.Text = "Por favor, tenga cuidado con lo que hace en este programa. Aunque parezca simple y fácil de usar, está destinado a administradores del sistema y gente en el departamento de TI. Si realiza operaciones sin cuidado a una imagen o instalación activa, podría no funcionar correctamente o dejar de funcionar."
                        Label32.Text = "Comencemos"
                        Label33.Text = "- ¿Está perdido en este programa? Le recomendamos que vea la ayuda para manejarse" & CrLf & _
                            "- ¿No entiende un término en específico? Le recomendamos comprobar el glosario. Los términos están ordenados alfabéticamente y ofrecen explicaciones lo más detalladas posible" & CrLf & _
                            "- ¿Desea saber cómo hacerlo en la línea de comandos? Le recomendamos comprobar la ayuda de comandos" & CrLf & _
                            "Estas opciones, y la información del programa, pueden ser encontradas en el menú Ayuda."
                        ' ToolStrip buttons
                        ToolStripButton1.Text = "Cerrar pestaña"
                        ToolStripButton2.Text = "Guardar proyecto"
                        ToolStripButton3.Text = "Descargar proyecto"
                        ToolStripButton3.ToolTipText = "Descargar proyecto de este programa"
                        ToolStripButton4.Text = "Mostrar ventana de progreso"
                        RefreshViewTSB.Text = "Actualizar vista"
                        ExpandCollapseTSB.Text = "Expandir"
                        ' TabPages
                        TabPage1.Text = "Proyecto"
                        TabPage2.Text = "Imagen"
                        TabPage3.Text = "Acciones"
                        ' TabPage controls
                        UnloadBtn.Text = "Descargar proyecto"
                        ExplorerView.Text = "Ver en Explorador de archivos"
                        Button14.Text = "Ver propiedades del proyecto"
                        Button15.Text = "Ver propiedades de la imagen"
                        Button16.Text = "Desmontar imagen..."
                        TabPageTitle1.Text = "Proyecto"
                        TabPageTitle2.Text = "Imagen"
                        TabPageDescription1.Text = "Ver información del proyecto"
                        TabPageDescription2.Text = "Ver información de la imagen"
                        Label1.Text = "Nombre:"
                        Label2.Text = "Ubicación:"
                        Label4.Text = "¿Hay imágenes montadas?"
                        Label5.Text = If(IsImageMounted, "Sí", "No")
                        LinkLabel1.Text = "Haga clic aquí para montar una imagen"
                        Label23.Text = "No se ha montado una imagen"
                        LinkLabel2.Text = "Necesita montar una imagen para ver su información aquí. Haga clic aquí para montar una imagen."
                        LinkLabel2.LinkArea = New LinkArea(67, 4)
                        LinkLabel3.Text = "O, si tiene una imagen montada, abra un directorio de montaje existente"
                        LinkLabel3.LinkArea = New LinkArea(32, 40)
                        UpdateLink.Text = "Hay una nueva versión disponible para su descarga e instalación. Haga clic aquí para saber más"
                        UpdateLink.LinkArea = New LinkArea(65, 29)
                        Label15.Text = "Índice:"
                        Label13.Text = "Punto de montaje:"
                        Label16.Text = "Versión:"
                        Label19.Text = "Nombre:"
                        Label21.Text = "Descripción:"
                        ' Actions
                        GroupBox1.Text = "Operaciones de la imagen"
                        GroupBox2.Text = "Operaciones de paquetes"
                        GroupBox3.Text = "Operaciones de características"
                        Button1.Text = "Montar imagen..."
                        Button2.Text = "Guardar cambios"
                        Button3.Text = "Guardar y desmontar imagen"
                        Button4.Text = "Descartar y desmontar imagen"
                        Button5.Text = "Añadir paquete..."
                        Button6.Text = "Obtener información de paquetes..."
                        Button7.Text = "Eliminar paquete..."
                        Button8.Text = "Obtener información de características..."
                        Button9.Text = "Deshabilitar característica..."
                        Button10.Text = "Habilitar característica..."
                        Button11.Text = "Recargar sesión de servicio..."
                        Button12.Text = "Realizar limpieza y/o reparación de componentes..."
                        Button13.Text = "Cambiar índices..."
                        Button19.Text = "Ver el nuevo diseño"
                        Button20.Text = "Regresar al diseño antiguo"
                        ' Pop-up context menus
                        PkgBasicInfo.Text = "Obtener información básica (todos los paquetes)"
                        PkgDetailedInfo.Text = "Obtener información detallada (paquete específico)"
                        CommitAndUnmountTSMI.Text = "Guardar cambios y desmontar imagen"
                        DiscardAndUnmountTSMI.Text = "Descartar cambios y desmontar imagen"
                        UnmountSettingsToolStripMenuItem.Text = "Configuración de desmontaje..."
                        ViewPackageDirectoryToolStripMenuItem.Text = "Ver directorio del paquete"
                        ' OpenFileDialogs and FolderBrowsers
                        OpenFileDialog1.Title = "Especifique el archivo de proyecto a cargar"
                        LocalMountDirFBD.Description = "Especifique el directorio de montaje que desea cargar en este proyecto:"
                        If Not ImgBW.IsBusy And areBackgroundProcessesDone Then
                            BGProcDetails.Label2.Text = "Los procesos de la imagen han completado"
                        End If
                        MenuDesc.Text = "Listo"
                        ' Tree view context menu
                        AccessDirectoryToolStripMenuItem.Text = "Acceder directorio"
                        UnloadProjectToolStripMenuItem1.Text = "Descargar proyecto"
                        CopyDeploymentToolsToolStripMenuItem.Text = "Copiar herramientas de implementación"
                        OfAllArchitecturesToolStripMenuItem.Text = "De todas las arquitecturas"
                        OfSelectedArchitectureToolStripMenuItem.Text = "De la arquitectura seleccionada"
                        ForX86ArchitectureToolStripMenuItem.Text = "Para arquitectura x86"
                        ForAmd64ArchitectureToolStripMenuItem.Text = "Para arquitectura AMD64"
                        ForARMArchitectureToolStripMenuItem.Text = "Para arquitectura ARM"
                        ForARM64ArchitectureToolStripMenuItem.Text = "Para arquitectura ARM64"
                        ImageOperationsToolStripMenuItem.Text = "Operaciones de la imagen"
                        MountImageToolStripMenuItem.Text = "Montar imagen..."
                        UnmountImageToolStripMenuItem.Text = "Desmontar imagen..."
                        RemoveVolumeImagesToolStripMenuItem.Text = "Eliminar imágenes de volumen..."
                        SwitchImageIndexesToolStripMenuItem1.Text = "Cambiar índices de imagen..."
                        UnattendedAnswerFilesToolStripMenuItem1.Text = "Archivos de respuesta desatendida"
                        ManageToolStripMenuItem.Text = "Administrar"
                        CreationWizardToolStripMenuItem.Text = "Crear"
                        ScratchDirectorySettingsToolStripMenuItem.Text = "Configurar directorio temporal"
                        ManageReportsToolStripMenuItem.Text = "Administrar informes"
                        AddToolStripMenuItem.Text = "Añadir"
                        NewFileToolStripMenuItem.Text = "Nuevo archivo..."
                        ExistingFileToolStripMenuItem.Text = "Archivo existente..."
                        ' Context menu of AppX information dialog
                        SaveResourceToolStripMenuItem.Text = "Guardar recurso..."
                        CopyToolStripMenuItem.Text = "Copiar recurso"
                        ' Context menu of AppX addition dialog
                        MicrosoftAppsToolStripMenuItem.Text = "Visitar el sitio web de Aplicaciones de Microsoft"
                        MicrosoftStoreGenerationProjectToolStripMenuItem.Text = "Visitar el sitio web del proyecto de generación de Microsoft Store"
                        ' New design
                        GreetingLabel.Text = "Le damos la bienvenida a esta sesión de servicio"
                        LinkLabel12.Text = "PROYECTO"
                        LinkLabel13.Text = "IMAGEN"
                        Label54.Text = "Nombre:"
                        Label51.Text = "Ubicación:"
                        Label53.Text = "¿Hay imágenes montadas?"
                        LinkLabel14.Text = "Haga clic aquí para montar una imagen"
                        Label55.Text = "Tareas del proyecto"
                        LinkLabel15.Text = "Ver propiedades del proyecto"
                        LinkLabel16.Text = "Abrir en el Explorador de Archivos"
                        LinkLabel17.Text = "Descargar proyecto"
                        Label59.Text = "No se ha montado una imagen"
                        Label58.Text = "Debe montar una imagen para poder ver su información"
                        Label57.Text = "Elecciones"
                        LinkLabel21.Text = "Montar una imagen..."
                        LinkLabel18.Text = "Escoger una imagen montada..."
                        Label39.Text = "Índice de la imagen:"
                        Label43.Text = "Punto de montaje:"
                        Label45.Text = "Versión:"
                        Label42.Text = "Nombre:"
                        Label40.Text = "Descripción:"
                        Label56.Text = "Tareas de la imagen"
                        LinkLabel20.Text = "Ver propiedades de la imagen"
                        LinkLabel19.Text = "Desmontar imagen"
                        GroupBox4.Text = "Operaciones de la imagen"
                        Button26.Text = "Montar imagen..."
                        Button27.Text = "Guardar cambios actuales"
                        Button28.Text = "Guardar cambios y desmontar imagen"
                        Button29.Text = "Desmontar imagen descartando cambios"
                        Button25.Text = "Recargar sesión de servicio"
                        Button24.Text = "Cambiar índices de la imagen..."
                        Button30.Text = "Aplicar imagen..."
                        Button31.Text = "Capturar imagen..."
                        Button32.Text = "Eliminar imágenes de volumen..."
                        Button33.Text = "Guardar información completa de la imagen..."
                        GroupBox5.Text = "Operaciones de paquetes"
                        Button36.Text = "Añadir paquete..."
                        Button34.Text = "Obtener información de paquetes..."
                        Button38.Text = "Guardar información de paquetes instalados..."
                        Button35.Text = "Eliminar paquete..."
                        Button37.Text = "Realizar mantenimiento y limpieza del almacén de componentes..."
                        GroupBox6.Text = "Operaciones de características"
                        Button41.Text = "Habilitar característica..."
                        Button39.Text = "Obtener información de características..."
                        Button42.Text = "Guardar información de características..."
                        Button40.Text = "Deshabilitar característica..."
                        GroupBox7.Text = "Operaciones de paquetes AppX"
                        Button44.Text = "Añadir paquete AppX..."
                        Button45.Text = "Obtener información de aplicaciones..."
                        Button46.Text = "Guardar información de paquetes AppX instalados..."
                        Button43.Text = "Eliminar paquete AppX..."
                        GroupBox8.Text = "Operaciones de funcionalidades"
                        Button48.Text = "Añadir funcionalidad..."
                        Button49.Text = "Obtener información de funcionalidades..."
                        Button50.Text = "Guardar información de funcionalidades..."
                        Button47.Text = "Eliminar funcionalidades..."
                        GroupBox9.Text = "Operaciones de controladores"
                        Button53.Text = "Añadir controlador..."
                        Button52.Text = "Obtener información de controladores..."
                        Button54.Text = "Guardar información de controladores instalados..."
                        Button51.Text = "Eliminar controlador..."
                        GroupBox10.Text = "Operaciones de Windows PE"
                        Button55.Text = "Obtener configuración"
                        Button56.Text = "Guardar configuración..."
                        Button57.Text = "Establecer ruta de destino..."
                        Button58.Text = "Establecer espacio temporal..."
                    Case "FRA"
                        ' Top-level menu items
                        FileToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Fichier".ToUpper(), "&Fichier")
                        ProjectToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Projet".ToUpper(), "&Projet")
                        CommandsToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "Com&mandes".ToUpper(), "Com&mandes")
                        ToolsToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "Ou&tils".ToUpper(), "Ou&tils")
                        HelpToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Aide".ToUpper(), "&Aide")
                        InvalidSettingsTSMI.Text = "Des paramètres non valides ont été détectés"
                        ' Submenu items
                        ' Menu - File
                        NewProjectToolStripMenuItem.Text = "&Nouveau projet..."
                        OpenExistingProjectToolStripMenuItem.Text = "&Ouvrir un projet existant"
                        ManageOnlineInstallationToolStripMenuItem.Text = "&Gérer l'installation en ligne"
                        ManageOfflineInstallationToolStripMenuItem.Text = "Gérer l'installation &hors ligne..."
                        SaveProjectToolStripMenuItem.Text = "&Sauvegarder le projet..."
                        SaveProjectasToolStripMenuItem.Text = "Sauvegarder le projet so&us..."
                        ExitToolStripMenuItem.Text = "Sor&tir"
                        ' Menu - Project
                        ViewProjectFilesInFileExplorerToolStripMenuItem.Text = "Visualiser les fichiers du projet dans l'explorateur de fichiers"
                        UnloadProjectToolStripMenuItem.Text = "Décharget le projet..."
                        SwitchImageIndexesToolStripMenuItem.Text = "Changer d'index de l'image..."
                        ProjectPropertiesToolStripMenuItem.Text = "Propriétés du projet"
                        ImagePropertiesToolStripMenuItem.Text = "Propriétés de l'image"
                        ' Menu - Commands
                        ImageManagementToolStripMenuItem.Text = "Gestion des images"
                        OSPackagesToolStripMenuItem.Text = "Paquets de systèmes d'exploitation"
                        ProvisioningPackagesToolStripMenuItem.Text = "Paquets de provisionnement"
                        AppPackagesToolStripMenuItem.Text = "Paquets d'applications"
                        AppPatchesToolStripMenuItem.Text = "Maintenance des applications (MSP)"
                        DefaultAppAssociationsToolStripMenuItem.Text = "Associations d'applications par défaut"
                        LanguagesAndRegionSettingsToolStripMenuItem.Text = "Langues et paramètres régionaux"
                        CapabilitiesToolStripMenuItem.Text = "Capacités"
                        WindowsEditionsToolStripMenuItem.Text = "Éditions Windows"
                        DriversToolStripMenuItem.Text = "Pilotes"
                        UnattendedAnswerFilesToolStripMenuItem.Text = "Fichiers de réponse non surveillés"
                        WindowsPEServicingToolStripMenuItem.Text = "Maintenance de Windows PE"
                        OSUninstallToolStripMenuItem.Text = "Désinstallation du système d'exploitation"
                        ReservedStorageToolStripMenuItem.Text = "Stockage réservé"
                        ' Menu - Commands - Image management
                        AppendImage.Text = "Ajouter le répertoire de capture à l'image..."
                        ApplyFFU.Text = "Appliquer le fichier FFU ou SFU..."
                        ApplyImage.Text = "Appliquer le fichier WIM ou SWM..."
                        CaptureCustomImage.Text = "Capturer les modifications incrémentales d'un fichier..."
                        CaptureFFU.Text = "Capturer des partitions dans un fichier FFU..."
                        CaptureImage.Text = "Capturer l'image d'un lecteur dans un fichier WIM..."
                        CleanupMountpoints.Text = "Supprimer les resources d'une image corrompue..."
                        CommitImage.Text = "Appliquer les modifications à l'image..."
                        DeleteImage.Text = "Supprimer les images de volume du fichier WIM..."
                        ExportImage.Text = "Exporter l'image..."
                        GetImageInfo.Text = "Obtenir des informations sur l'image..."
                        GetWIMBootEntry.Text = "Obtenir les entrées de configuration WIMBoot..."
                        ListImage.Text = "Lister des fichiers et répertoires dans l'image..."
                        MountImage.Text = "Monter l'image..."
                        OptimizeFFU.Text = "Optimiser le fichier FFU..."
                        OptimizeImage.Text = "Optimiser l'image..."
                        RemountImage.Text = "Remonter l'image pour la maintenance..."
                        SplitFFU.Text = "Diviser un fichier FFU en fichiers SFU..."
                        SplitImage.Text = "Diviser un fichier WIM en fichiers SWM..."
                        UnmountImage.Text = "Démonter l'image..."
                        UpdateWIMBootEntry.Text = "Mettre à jour de l'entrée de configuration de WIMBoot..."
                        ApplySiloedPackage.Text = "Appliquer un package de provisionnement en silo..."
                        SaveImageInformationToolStripMenuItem.Text = "Sauvegarder les informations de l'image..."
                        ' Menu - Commands - OS packages
                        GetPackages.Text = "Obtenir des informations sur le paquet..."
                        AddPackage.Text = "Ajouter un paquet..."
                        RemovePackage.Text = "Supprimer le paquet..."
                        GetFeatures.Text = "Obtenir des informations sur les caractéristiques..."
                        EnableFeature.Text = "Activer la caractéristique..."
                        DisableFeature.Text = "Désactiver la caractéristique..."
                        CleanupImage.Text = "Effectuer des opérations de nettoyage ou de récupération..."
                        ' Menu - Commands - Provisioning packages
                        AddProvisioningPackage.Text = "Ajouter un paquet de provisionnement..."
                        GetProvisioningPackageInfo.Text = "Obtenir des informations sur le paquet de provisionnement..."
                        ApplyCustomDataImage.Text = "Appliquer une image de données personnalisée..."
                        ' Menu - Commands - App packages
                        GetProvisionedAppxPackages.Text = "Obtenir des informations sur le paquet d'applications..."
                        AddProvisionedAppxPackage.Text = "Ajouter un paquet d'applications provisionnées..."
                        RemoveProvisionedAppxPackage.Text = "Supprimer le provisionnement pour les paquets d'applications..."
                        OptimizeProvisionedAppxPackages.Text = "Optimiser les paquets provisionnés..."
                        SetProvisionedAppxDataFile.Text = "Ajouter un fichier de données personnalisé dans le paquet d'applications..."
                        ' Menu - Commands - App (MSP) servicing
                        CheckAppPatch.Text = "Obtenir des informations sur les correctifs de l'application..."
                        GetAppPatchInfo.Text = "Obtenir des informations détaillées sur les correctifs des applications..."
                        GetAppPatches.Text = "Obtenir des informations basiques sur les correctifs des applications installées..."
                        GetAppInfo.Text = "Obtenir des informations détaillées sur l'application Windows Installer (*.msi)..."
                        GetApps.Text = "Obtenir des informations basiques sur l'application Windows Installer (*.msi)..."
                        ' Menu - Commands - Default app associations
                        ExportDefaultAppAssociations.Text = "Exporter les associations d'applications par défaut..."
                        GetDefaultAppAssociations.Text = "Obtenir des informations sur l'association d'applications par défaut..."
                        ImportDefaultAppAssociations.Text = "Importer les associations d'applications par défaut..."
                        RemoveDefaultAppAssociations.Text = "Supprimer les associations d'applications par défaut..."
                        ' Menu - Commands - Languages and regional settings
                        GetIntl.Text = "Obtenir des paramètres et des langues internationaux..."
                        SetUILang.Text = "Définir la langue de l'interface utilisateur..."
                        SetUILangFallback.Text = "Définir la langue par défaut de l'interface utilisateur..."
                        SetSysUILang.Text = "Définir la langue préférée de l'interface utilisateur du système..."
                        SetSysLocale.Text = "Définir les paramètres linguistiques du système..."
                        SetUserLocale.Text = "Définir les paramètres linguistiques de l'utilisateur..."
                        SetInputLocale.Text = "Définir la langue d'entrée..."
                        SetAllIntl.Text = "Définir la langue de l'interface utilisateur et les paramètres locaux..."
                        SetTimeZone.Text = "Définir le fuseau horaire par défaut..."
                        SetSKUIntlDefaults.Text = "Définir les langues et les locales par défaut..."
                        SetLayeredDriver.Text = "Régler le pilote en couches..."
                        GenLangINI.Text = "Générer le fichier Lang.ini..."
                        SetSetupUILang.Text = "Définir la langue d'installation par défaut..."
                        ' Menu - Commands - Capabilities
                        AddCapability.Text = "Ajouter une capacité..."
                        ExportSource.Text = "Exporter les capacités dans le référentiel..."
                        GetCapabilities.Text = "Obtenir des informations sur les capacités..."
                        RemoveCapability.Text = "Supprimer la capacité..."
                        ' Menu - Commands - Windows editions
                        GetCurrentEdition.Text = "Obtenir l'édition actuelle..."
                        GetTargetEditions.Text = "Obtenir des objectifs de mise à niveau..."
                        SetEdition.Text = "Mettre à jour l'image..."
                        SetProductKey.Text = "Définir la clé de produit..."
                        ' Menu - Commands - Drivers
                        GetDrivers.Text = "Obtenir des informations sur le pilote..."
                        AddDriver.Text = "Ajouter un pilote..."
                        RemoveDriver.Text = "Retirer le pilote..."
                        ExportDriver.Text = "Exporter des paquets de pilotes..."
                        ' Menu - Commands - Unattended answer files
                        ApplyUnattend.Text = "Appliquer un fichier de réponse non surveillé..."
                        ' Menu - Commands - Windows PE servicing
                        GetPESettings.Text = "Obtenir des paramètres..."
                        SetScratchSpace.Text = "Définir l'espace temporaire..."
                        SetTargetPath.Text = "Définir le chemin cible..."
                        ' Menu - Commands - OS uninstall
                        GetOSUninstallWindow.Text = "Obtenir la créneau de désinstallation..."
                        InitiateOSUninstall.Text = "Démarrer la désinstallation..."
                        RemoveOSUninstall.Text = "Supprimer la possibilité de revenir en arrière..."
                        SetOSUninstallWindow.Text = "Définir la créneau de désinstallation..."
                        ' Menu - Commands - Reserved storage
                        SetReservedStorageState.Text = "Définir l'état du stockage réservé..."
                        GetReservedStorageState.Text = "Obtenir l'état du stockage réservé..."
                        ' Menu - Commands - Microsoft Edge
                        AddEdge.Text = "Ajouter Edge..."
                        AddEdgeBrowser.Text = "Ajouter le navigateur Edge..."
                        AddEdgeWebView.Text = "Ajouter Edge WebView..."
                        ' Menu - Tools
                        ImageConversionToolStripMenuItem.Text = "Conversion des images"
                        MergeSWM.Text = "Fusionner des fichiers SWM..."
                        RemountImageWithWritePermissionsToolStripMenuItem.Text = "Remonter l'image avec les droits d'écriture"
                        CommandShellToolStripMenuItem.Text = "Console de commande"
                        UnattendedAnswerFileManagerToolStripMenuItem.Text = "Gestionnaire de fichiers de réponse sans surveillance"
                        ReportManagerToolStripMenuItem.Text = "Gestionnaire de rapports"
                        MountedImageManagerTSMI.Text = "Gestionnaire des images montées"
                        WimScriptEditorCommand.Text = "Éditeur de listes de configuration"
                        ActionEditorToolStripMenuItem.Text = "Éditeur des actions"
                        OptionsToolStripMenuItem.Text = "Paramètres"
                        ' Menu - Help
                        HelpTopicsToolStripMenuItem.Text = "Rubriques d'aide"
                        GlossaryToolStripMenuItem.Text = "Glossaire"
                        CommandHelpToolStripMenuItem.Text = "Aide à la commande..."
                        AboutDISMToolsToolStripMenuItem.Text = "À propos de DISMTools"
                        ' Menu - Invalid settings
                        ISFix.Text = "Plus d'informations"
                        ISHelp.Text = "Qu'est-ce que c'est ?"
                        ' Menu - DevState
                        ReportFeedbackToolStripMenuItem.Text = "Rapport de rétroaction (s'ouvre dans un navigateur web)"
                        ' Menu - Contributions
                        ContributeToTheHelpSystemToolStripMenuItem.Text = "Contribuer au système d'aide"
                        ' Start Panel
                        LabelHeader1.Text = "Commencer"
                        Label10.Text = "Projets récents"
                        Label11.Text = "A venir !"
                        NewProjLink.Text = "Nouveau projet..."
                        ExistingProjLink.Text = "Ouvrir un projet existant..."
                        OnlineInstMgmt.Text = "Gérer l'installation en ligne"
                        OfflineInstMgmt.Text = "Gérer l'installation hors ligne..."
                        ' Start Panel tabs
                        WelcomeTab.Text = "Bienvenue"
                        NewsFeedTab.Text = "Dernières nouvelles"
                        VideosTab.Text = "Vidéos tutorielles"
                        ' Welcome tab contents
                        Label24.Text = "Bienvenue à DISMTools"
                        Label25.Text = "L'interface graphique pour effectuer les opérations DISM."
                        Label26.Text = "Il s'agit d'une version bêta du logiciel"
                        Label27.Text = "Ce programme est actuellement en version bêta. Cela signifie que beaucoup de choses ne fonctionneront pas comme prévu. Il y aura également de nombreux bugs et, de manière générale, le programme est incomplet (comme vous pouvez le constater dès à présent)."
                        Label28.Text = "Ce programme est open-source"
                        Label29.Text = "Ce programme est open-source, ce qui signifie que vous pouvez jeter un coup d'œil à son fonctionnement et mieux le comprendre."
                        Label30.Text = "Assurez-vous de savoir ce que vous faites"
                        Label31.Text = "Faites attention à ce que vous faites dans ce programme. Bien qu'il puisse paraître simple et facile à utiliser, il est destiné aux administrateurs système et aux personnes travaillant dans le département informatique. Si vous effectuez sans précaution des opérations sur une image ou une installation active, il se peut qu'elle ne fonctionne pas correctement ou qu'elle refuse de fonctionner."
                        Label32.Text = "Pour commencer"
                        Label33.Text = "- Vous vous êtes perdu dans ce programme ? Nous vous recommandons de consulter les rubriques d'aide pour vous y retrouver" & CrLf & _
                            "- Vous ne comprenez pas un terme spécifique ? Nous vous recommandons de consulter le glossaire. Les termes sont classés par ordre alphabétique et fournissent des explications aussi détaillées que possible" & CrLf & _
                            "- Vous voulez savoir comment le faire en ligne de commande ? Nous vous recommandons de consulter la commande help" & CrLf & _
                            "Ces options, ainsi que les informations sur le programme, peuvent être trouvées en ouvrant le menu Aide."
                        ' ToolStrip buttons
                        ToolStripButton1.Text = "Fermer l'onglet"
                        ToolStripButton2.Text = "Sauvegarder le projet"
                        ToolStripButton3.Text = "Décharger le projet"
                        ToolStripButton3.ToolTipText = "Décharger le projet de ce programme"
                        ToolStripButton4.Text = "Afficher la fenêtre de progression"
                        RefreshViewTSB.Text = "Rafraîchir la vue"
                        ExpandCollapseTSB.Text = "Élargir"
                        ' TabPages
                        TabPage1.Text = "Projet"
                        TabPage2.Text = "Image"
                        TabPage3.Text = "Actions"
                        ' TabPage controls
                        UnloadBtn.Text = "Décharger le projet"
                        ExplorerView.Text = "Voir dans l'explorateur de fichiers"
                        Button14.Text = "Voir les propriétés du projet"
                        Button15.Text = "Voir les propriétés de l'image"
                        Button16.Text = "Démonter l'image..."
                        TabPageTitle1.Text = "Projet"
                        TabPageTitle2.Text = "Image"
                        TabPageDescription1.Text = "Voir les informations sur le projet"
                        TabPageDescription2.Text = "Voir les informations sur l'image"
                        Label1.Text = "Nom:"
                        Label2.Text = "Emplacement:"
                        Label4.Text = "Images montées?"
                        Label5.Text = If(IsImageMounted, "Oui", "Non")
                        LinkLabel1.Text = "Cliquez ici pour monter une image"
                        Label23.Text = "Aucune image n'a été montée"
                        LinkLabel2.Text = "Vous devez monter une image pour pouvoir afficher ses informations ici. Cliquez ici pour monter une image."
                        LinkLabel2.LinkArea = New LinkArea(80, 3)
                        LinkLabel3.Text = "Ou, si vous avez une image montée, ouvrez un répertoire de montage existant"
                        LinkLabel3.LinkArea = New LinkArea(35, 40)
                        UpdateLink.Text = "Une nouvelle version est disponible pour le téléchargement et l'installation. Cliquez ici pour en savoir plus"
                        UpdateLink.LinkArea = New LinkArea(78, 31)
                        Label15.Text = "Index de l'image:"
                        Label13.Text = "Point de montage:"
                        Label16.Text = "Version:"
                        Label19.Text = "Nom:"
                        Label21.Text = "Description:"
                        ' Actions
                        GroupBox1.Text = "Opérations sur les images"
                        GroupBox2.Text = "Opérations sur les paquets"
                        GroupBox3.Text = "Opérations sur les caractéristiques"
                        Button1.Text = "Monter l'image..."
                        Button2.Text = "Appliquer les modifications en cours"
                        Button3.Text = "Appliquer les modifications et démonter l'image"
                        Button4.Text = "Démonter l'image en ignorant les modifications"
                        Button5.Text = "Ajouter un paquet..."
                        Button6.Text = "Obtenir des informations sur le paquet..."
                        Button7.Text = "Supprimer le paquet..."
                        Button8.Text = "Obtenir des informations sur les caractéristiques..."
                        Button9.Text = "Désactiver la caractéristique..."
                        Button10.Text = "Activer la caractéristique..."
                        Button11.Text = "Recharger la session de maintenance..."
                        Button12.Text = "Effectuer le nettoyage et/ou la réparation des composants..."
                        Button13.Text = "Changer d'index de l'image..."
                        Button19.Text = "Prévisualiser le nouveau design"
                        Button20.Text = "Revenir à l'ancien design"
                        ' Pop-up context menus
                        PkgBasicInfo.Text = "Obtenir des informations basiques (tous les paquets)"
                        PkgDetailedInfo.Text = "Obtenir des informations détaillées (paquet spécifique)"
                        CommitAndUnmountTSMI.Text = "Valider les modifications et démonter l'image"
                        DiscardAndUnmountTSMI.Text = "Annuler les modifications et démonter l'image"
                        UnmountSettingsToolStripMenuItem.Text = "Configurer les paramètres de démontage......"
                        ViewPackageDirectoryToolStripMenuItem.Text = "Afficher le répertoire des paquets"
                        ' OpenFileDialogs and FolderBrowsers
                        OpenFileDialog1.Title = "Spécifier le fichier de projet à charger"
                        LocalMountDirFBD.Description = "Veuillez spécifier le répertoire de montage que vous souhaitez charger dans ce projet:"
                        If Not ImgBW.IsBusy And areBackgroundProcessesDone Then
                            BGProcDetails.Label2.Text = "Les processus de l'image sont terminés"
                        End If
                        MenuDesc.Text = "Prêt"
                        ' Tree view context menu
                        AccessDirectoryToolStripMenuItem.Text = "Accéder à ce répertoire"
                        UnloadProjectToolStripMenuItem1.Text = "Décharger le projet"
                        CopyDeploymentToolsToolStripMenuItem.Text = "Copier les outils de déploiement"
                        OfAllArchitecturesToolStripMenuItem.Text = "De toutes les architectures"
                        OfSelectedArchitectureToolStripMenuItem.Text = "De l'architecture sélectionnée"
                        ForX86ArchitectureToolStripMenuItem.Text = "Pour l'architecture x86"
                        ForAmd64ArchitectureToolStripMenuItem.Text = "Pour l'architecture AMD64"
                        ForARMArchitectureToolStripMenuItem.Text = "Pour l'architecture ARM"
                        ForARM64ArchitectureToolStripMenuItem.Text = "Pour l'architecture ARM64"
                        ImageOperationsToolStripMenuItem.Text = "Opérations sur les images"
                        MountImageToolStripMenuItem.Text = "Monter l'image..."
                        UnmountImageToolStripMenuItem.Text = "Démonter l'image..."
                        RemoveVolumeImagesToolStripMenuItem.Text = "Supprimer les images de volume..."
                        SwitchImageIndexesToolStripMenuItem1.Text = "Changer d'index de l'image..."
                        UnattendedAnswerFilesToolStripMenuItem1.Text = "Fichiers de réponse non surveillés"
                        ManageToolStripMenuItem.Text = "Gérer"
                        CreationWizardToolStripMenuItem.Text = "Créer"
                        ScratchDirectorySettingsToolStripMenuItem.Text = "Configurer le répertoire temporaire"
                        ManageReportsToolStripMenuItem.Text = "Gérer les rapports"
                        AddToolStripMenuItem.Text = "Ajouter"
                        NewFileToolStripMenuItem.Text = "Nouveau fichier..."
                        ExistingFileToolStripMenuItem.Text = "Fichier existant..."
                        ' Context menu of AppX information dialog
                        SaveResourceToolStripMenuItem.Text = "Sauvegarder les ressources..."
                        CopyToolStripMenuItem.Text = "Copier la ressource"
                        ' Context menu of AppX addition dialog
                        MicrosoftAppsToolStripMenuItem.Text = "Visiter le site web de Microsoft Apps"
                        MicrosoftStoreGenerationProjectToolStripMenuItem.Text = "Visiter le site web du projet Microsoft Store Generation"
                        ' New design
                        GreetingLabel.Text = "Bienvenue à cette session de service"
                        LinkLabel12.Text = "PROJET"
                        LinkLabel13.Text = "IMAGE"
                        Label54.Text = "Nom :"
                        Label51.Text = "Lieu :"
                        Label53.Text = "Images montées ?"
                        LinkLabel14.Text = "Cliquez ici pour monter une image"
                        Label55.Text = "Tâches du projet"
                        LinkLabel15.Text = "Voir les propriétés du projet"
                        LinkLabel16.Text = "Ouvrir dans l'explorateur de fichiers"
                        LinkLabel17.Text = "Décharger le projet"
                        Label59.Text = "Aucune image n'a été montée"
                        Label58.Text = "Vous devez monter une image pour pouvoir consulter ses informations."
                        Label57.Text = "Choix"
                        LinkLabel21.Text = "Monter une image..."
                        LinkLabel18.Text = "Choisir une image montée..."
                        Label39.Text = "Index de l'image :"
                        Label43.Text = "Répertoire de montage :"
                        Label45.Text = "Version :"
                        Label42.Text = "Nom :"
                        Label40.Text = "Description :"
                        Label56.Text = "Tâches de l'image"
                        LinkLabel20.Text = "Voir les propriétés de l'image"
                        LinkLabel19.Text = "Démonter l'image"
                        GroupBox4.Text = "Opérations sur les images"
                        Button26.Text = "Monter une image..."
                        Button27.Text = "Sauvegarder les modifications pendants"
                        Button28.Text = "Sauvegarder modifications et démonter l'image"
                        Button29.Text = "Démonter l'image en supprimant les modifications"
                        Button25.Text = "Recharger la session de service"
                        Button24.Text = "Changer d'index de l'image..."
                        Button30.Text = "Appliquer l'image..."
                        Button31.Text = "Capturer image..."
                        Button32.Text = "Supprimer les images de volume..."
                        Button33.Text = "Sauvegarder les informations complètes de l'image..."
                        GroupBox5.Text = "Opérations sur les paquets"
                        Button36.Text = "Ajouter des paquets..."
                        Button34.Text = "Obtenir des informations sur le paquet..."
                        Button38.Text = "Sauvegarder les informations sur les paquets installés..."
                        Button35.Text = "Supprimer des paquets..."
                        Button37.Text = "Effectuer la maintenance et le nettoyage du stock de composants..."
                        GroupBox6.Text = "Opérations sur les caractéristiques"
                        Button41.Text = "Activer des caractéristiques..."
                        Button39.Text = "Obtenir des informations sur les caractéristiques..."
                        Button42.Text = "Sauvegarder les caractéristiques..."
                        Button40.Text = "Désactiver des caractéristiques..."
                        GroupBox7.Text = "Opérations sur les paquets AppX"
                        Button44.Text = "Ajouter des paquets AppX..."
                        Button45.Text = "Obtenir des informations sur les applications..."
                        Button46.Text = "Sauvegarder les informations sur les paquets AppX installés..."
                        Button43.Text = "Supprimer des paquets AppX..."
                        GroupBox8.Text = "Opérations sur les capacités"
                        Button48.Text = "Ajouter des capacités..."
                        Button49.Text = "Obtenir des informations sur les capacités..."
                        Button50.Text = "Sauvegarder les informations sur les capacités..."
                        Button47.Text = "Supprimer des capacités..."
                        GroupBox9.Text = "Opérations sur les pilotes"
                        Button53.Text = "Ajouter des paquets de pilotes..."
                        Button52.Text = "Obtenir des informations sur les pilotes..."
                        Button54.Text = "Sauvegarder les informations sur les pilotes installés..."
                        Button51.Text = "Supprimer des pilotes..."
                        GroupBox10.Text = "Opérations de Windows PE"
                        Button55.Text = "Obtenir des paramètres..."
                        Button56.Text = "Sauvegarder les paramètres..."
                        Button57.Text = "Configurer le chemin d'accès..."
                        Button58.Text = "Configurer l'espace temporaire..."
                    Case Else
                        Language = 1
                        ChangeLangs(Language)
                        Exit Sub
                End Select
            Case 1
                ' Top-level menu items
                FileToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&File".ToUpper(), "&File")
                ProjectToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Project".ToUpper(), "&Project")
                CommandsToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "Com&mands".ToUpper(), "Com&mands")
                ToolsToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Tools".ToUpper(), "&Tools")
                HelpToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Help".ToUpper(), "&Help")
                InvalidSettingsTSMI.Text = "Invalid settings have been detected"
                ' Submenu items
                ' Menu - File
                NewProjectToolStripMenuItem.Text = "&New project..."
                OpenExistingProjectToolStripMenuItem.Text = "&Open existing project"
                ManageOnlineInstallationToolStripMenuItem.Text = "&Manage online installation"
                ManageOfflineInstallationToolStripMenuItem.Text = "Manage o&ffline installation..."
                SaveProjectToolStripMenuItem.Text = "&Save project..."
                SaveProjectasToolStripMenuItem.Text = "Save project &as..."
                ExitToolStripMenuItem.Text = "E&xit"
                ' Menu - Project
                ViewProjectFilesInFileExplorerToolStripMenuItem.Text = "View project files in File Explorer"
                UnloadProjectToolStripMenuItem.Text = "Unload project..."
                SwitchImageIndexesToolStripMenuItem.Text = "Switch image indexes..."
                ProjectPropertiesToolStripMenuItem.Text = "Project properties"
                ImagePropertiesToolStripMenuItem.Text = "Image properties"
                ' Menu - Commands
                ImageManagementToolStripMenuItem.Text = "Image management"
                OSPackagesToolStripMenuItem.Text = "OS packages"
                ProvisioningPackagesToolStripMenuItem.Text = "Provisioning packages"
                AppPackagesToolStripMenuItem.Text = "App packages"
                AppPatchesToolStripMenuItem.Text = "App (MSP) servicing"
                DefaultAppAssociationsToolStripMenuItem.Text = "Default app associations"
                LanguagesAndRegionSettingsToolStripMenuItem.Text = "Languages and regional settings"
                CapabilitiesToolStripMenuItem.Text = "Capabilities"
                WindowsEditionsToolStripMenuItem.Text = "Windows editions"
                DriversToolStripMenuItem.Text = "Drivers"
                UnattendedAnswerFilesToolStripMenuItem.Text = "Unattended answer files"
                WindowsPEServicingToolStripMenuItem.Text = "Windows PE servicing"
                OSUninstallToolStripMenuItem.Text = "OS uninstall"
                ReservedStorageToolStripMenuItem.Text = "Reserved storage"
                ' Menu - Commands - Image management
                AppendImage.Text = "Append capture directory to image..."
                ApplyFFU.Text = "Apply FFU or SFU file..."
                ApplyImage.Text = "Apply WIM or SWM file..."
                CaptureCustomImage.Text = "Capture incremental changes to file..."
                CaptureFFU.Text = "Capture partitions to FFU file..."
                CaptureImage.Text = "Capture image of a drive to WIM file..."
                CleanupMountpoints.Text = "Delete resources from corrupted image..."
                CommitImage.Text = "Apply changes to image..."
                DeleteImage.Text = "Delete volume images from WIM file..."
                ExportImage.Text = "Export image..."
                GetImageInfo.Text = "Get image information..."
                GetWIMBootEntry.Text = "Get WIMBoot configuration entries..."
                ListImage.Text = "List files and directories in image..."
                MountImage.Text = "Mount image..."
                OptimizeFFU.Text = "Optimize FFU file..."
                OptimizeImage.Text = "Optimize image..."
                RemountImage.Text = "Remount image for servicing..."
                SplitFFU.Text = "Split FFU file into SFU files..."
                SplitImage.Text = "Split WIM file into SWM files..."
                UnmountImage.Text = "Unmount image..."
                UpdateWIMBootEntry.Text = "Update WIMBoot configuration entry..."
                ApplySiloedPackage.Text = "Apply siloed provisioning package..."
                SaveImageInformationToolStripMenuItem.Text = "Save image information..."
                ' Menu - Commands - OS packages
                GetPackages.Text = "Get package information..."
                AddPackage.Text = "Add package..."
                RemovePackage.Text = "Remove package..."
                GetFeatures.Text = "Get feature information..."
                EnableFeature.Text = "Enable feature..."
                DisableFeature.Text = "Disable feature..."
                CleanupImage.Text = "Perform cleanup or recovery operations..."
                ' Menu - Commands - Provisioning packages
                AddProvisioningPackage.Text = "Add provisioning package..."
                GetProvisioningPackageInfo.Text = "Get provisioning package information..."
                ApplyCustomDataImage.Text = "Apply custom data image..."
                ' Menu - Commands - App packages
                GetProvisionedAppxPackages.Text = "Get app package information..."
                AddProvisionedAppxPackage.Text = "Add provisioned app package..."
                RemoveProvisionedAppxPackage.Text = "Remove provisioning for app package..."
                OptimizeProvisionedAppxPackages.Text = "Optimize provisioned packages..."
                SetProvisionedAppxDataFile.Text = "Add custom data file into app package..."
                ' Menu - Commands - App (MSP) servicing
                CheckAppPatch.Text = "Get application patch information..."
                GetAppPatchInfo.Text = "Get detailed application patch information..."
                GetAppPatches.Text = "Get basic installed application patch information..."
                GetAppInfo.Text = "Get detailed Windows Installer (*.msi) application information..."
                GetApps.Text = "Get basic Windows Installer (*.msi) application information..."
                ' Menu - Commands - Default app associations
                ExportDefaultAppAssociations.Text = "Export default application associations..."
                GetDefaultAppAssociations.Text = "Get default application association information..."
                ImportDefaultAppAssociations.Text = "Import default application associations..."
                RemoveDefaultAppAssociations.Text = "Remove default application associations..."
                ' Menu - Commands - Languages and regional settings
                GetIntl.Text = "Get international settings and languages..."
                SetUILang.Text = "Set UI language..."
                SetUILangFallback.Text = "Set default UI fallback language..."
                SetSysUILang.Text = "Set system preferred UI language..."
                SetSysLocale.Text = "Set system locale..."
                SetUserLocale.Text = "Set user locale..."
                SetInputLocale.Text = "Set input locale..."
                SetAllIntl.Text = "Set UI language and locales..."
                SetTimeZone.Text = "Set default time zone..."
                SetSKUIntlDefaults.Text = "Set default languages and locales..."
                SetLayeredDriver.Text = "Set layered driver..."
                GenLangINI.Text = "Generate Lang.ini file..."
                SetSetupUILang.Text = "Set default Setup language..."
                ' Menu - Commands - Capabilities
                AddCapability.Text = "Add capability..."
                ExportSource.Text = "Export capabilities into repository..."
                GetCapabilities.Text = "Get capability information..."
                RemoveCapability.Text = "Remove capability..."
                ' Menu - Commands - Windows editions
                GetCurrentEdition.Text = "Get current edition..."
                GetTargetEditions.Text = "Get upgrade targets..."
                SetEdition.Text = "Upgrade image..."
                SetProductKey.Text = "Set product key..."
                ' Menu - Commands - Drivers
                GetDrivers.Text = "Get driver information..."
                AddDriver.Text = "Add driver..."
                RemoveDriver.Text = "Remove driver..."
                ExportDriver.Text = "Export driver packages..."
                ' Menu - Commands - Unattended answer files
                ApplyUnattend.Text = "Apply unattended answer file..."
                ' Menu - Commands - Windows PE servicing
                GetPESettings.Text = "Get settings..."
                SetScratchSpace.Text = "Set scratch space..."
                SetTargetPath.Text = "Set target path..."
                ' Menu - Commands - OS uninstall
                GetOSUninstallWindow.Text = "Get uninstall window..."
                InitiateOSUninstall.Text = "Initiate uninstall..."
                RemoveOSUninstall.Text = "Remove roll back ability..."
                SetOSUninstallWindow.Text = "Set uninstall window..."
                ' Menu - Commands - Reserved storage
                SetReservedStorageState.Text = "Set reserved storage state..."
                GetReservedStorageState.Text = "Get reserved storage state..."
                ' Menu - Commands - Microsoft Edge
                AddEdge.Text = "Add Edge..."
                AddEdgeBrowser.Text = "Add Edge browser..."
                AddEdgeWebView.Text = "Add Edge WebView..."
                ' Menu - Tools
                ImageConversionToolStripMenuItem.Text = "Image conversion"
                MergeSWM.Text = "Merge SWM files..."
                RemountImageWithWritePermissionsToolStripMenuItem.Text = "Remount image with write permissions"
                CommandShellToolStripMenuItem.Text = "Command Console"
                UnattendedAnswerFileManagerToolStripMenuItem.Text = "Unattended answer file manager"
                ReportManagerToolStripMenuItem.Text = "Report manager"
                MountedImageManagerTSMI.Text = "Mounted image manager"
                WimScriptEditorCommand.Text = "Configuration list editor"
                ActionEditorToolStripMenuItem.Text = "Action editor"
                OptionsToolStripMenuItem.Text = "Options"
                ' Menu - Help
                HelpTopicsToolStripMenuItem.Text = "Help Topics"
                GlossaryToolStripMenuItem.Text = "Glossary"
                CommandHelpToolStripMenuItem.Text = "Command help..."
                AboutDISMToolsToolStripMenuItem.Text = "About DISMTools"
                ' Menu - Invalid settings
                ISFix.Text = "More information"
                ISHelp.Text = "What's this?"
                ' Menu - DevState
                ReportFeedbackToolStripMenuItem.Text = "Report feedback (opens in web browser)"
                ' Menu - Contributions
                ContributeToTheHelpSystemToolStripMenuItem.Text = "Contribute to the help system"
                ' Start Panel
                LabelHeader1.Text = "Begin"
                Label10.Text = "Recent projects"
                Label11.Text = "Coming soon!"
                NewProjLink.Text = "New project..."
                ExistingProjLink.Text = "Open existing project..."
                OnlineInstMgmt.Text = "Manage online installation"
                OfflineInstMgmt.Text = "Manage offline installation..."
                ' Start Panel tabs
                WelcomeTab.Text = "Welcome"
                NewsFeedTab.Text = "Latest news"
                VideosTab.Text = "Tutorial videos"
                ' Welcome tab contents
                Label24.Text = "Welcome to DISMTools"
                Label25.Text = "The graphical front-end to perform DISM operations."
                Label26.Text = "This is beta software"
                Label27.Text = "Currently, this program is in beta. This means lots of things will not work as expected. There will also be lots of bugs, and, generally, the program is incomplete (as you can see right now)"
                Label28.Text = "This program is open-source"
                Label29.Text = "This program is open-source, meaning you can take a look at how it works and understand it better."
                Label30.Text = "Be sure to know what you are doing"
                Label31.Text = "Please be careful over what you do in this program. Although it may look simple and easy to use, it is targeted to system administrators and people in the IT department. If you carelessly perform operations to an image or an active installation, it may not work correctly or may refuse to work."
                Label32.Text = "Getting started"
                Label33.Text = "- Did you get lost in this program? We recommend checking the help topics to get around" & CrLf & _
                    "- Don't understand a specific term? We recommend checking the glossary. The terms are alphabetically sorted and provide explanations as detailed as possible" & CrLf & _
                    "- Want to know how to do it in the command line? We recommmend checking the command help" & CrLf & _
                    "These options, and the program information, can be found by opening the Help menu."
                ' ToolStrip buttons
                ToolStripButton1.Text = "Close tab"
                ToolStripButton2.Text = "Save project"
                ToolStripButton3.Text = "Unload project"
                ToolStripButton3.ToolTipText = "Unload project from this program"
                ToolStripButton4.Text = "Show progress window"
                RefreshViewTSB.Text = "Refresh view"
                ExpandCollapseTSB.Text = "Expand"
                ' TabPages
                TabPage1.Text = "Project"
                TabPage2.Text = "Image"
                TabPage3.Text = "Actions"
                ' TabPage controls
                UnloadBtn.Text = "Unload project"
                ExplorerView.Text = "View in File Explorer"
                Button14.Text = "View project properties"
                Button15.Text = "View image properties"
                Button16.Text = "Unmount image..."
                TabPageTitle1.Text = "Project"
                TabPageTitle2.Text = "Image"
                TabPageDescription1.Text = "View project information"
                TabPageDescription2.Text = "View image information"
                Label1.Text = "Name:"
                Label2.Text = "Location:"
                Label4.Text = "Images mounted?"
                Label5.Text = If(IsImageMounted, "Yes", "No")
                LinkLabel1.Text = "Click here to mount an image"
                Label23.Text = "No image has been mounted"
                LinkLabel2.Text = "You need to mount an image in order to view its information here. Click here to mount an image."
                LinkLabel2.LinkArea = New LinkArea(72, 4)
                LinkLabel3.Text = "Or, if you have a mounted image, open an existing mount directory"
                LinkLabel3.LinkArea = New LinkArea(33, 32)
                UpdateLink.Text = "A new version is available for download and installation. Click here to learn more"
                UpdateLink.LinkArea = New LinkArea(58, 24)
                Label15.Text = "Image index:"
                Label13.Text = "Mount point:"
                Label16.Text = "Version:"
                Label19.Text = "Name:"
                Label21.Text = "Description:"
                ' Actions
                GroupBox1.Text = "Image operations"
                GroupBox2.Text = "Package operations"
                GroupBox3.Text = "Feature operations"
                Button1.Text = "Mount image..."
                Button2.Text = "Commit current changes"
                Button3.Text = "Commit and unmount image"
                Button4.Text = "Unmount image discarding changes"
                Button5.Text = "Add package..."
                Button6.Text = "Get package information..."
                Button7.Text = "Remove package..."
                Button8.Text = "Get feature information..."
                Button9.Text = "Disable feature..."
                Button10.Text = "Enable feature..."
                Button11.Text = "Reload servicing session..."
                Button12.Text = "Perform component cleanup and/or repair..."
                Button13.Text = "Switch indexes..."
                Button19.Text = "Preview the new design"
                Button20.Text = "Go back to the old design"
                ' Pop-up context menus
                PkgBasicInfo.Text = "Get basic information (all packages)"
                PkgDetailedInfo.Text = "Get detailed information (specific package)"
                CommitAndUnmountTSMI.Text = "Commit changes and unmount image"
                DiscardAndUnmountTSMI.Text = "Discard changes and unmount image"
                UnmountSettingsToolStripMenuItem.Text = "Unmount settings..."
                ViewPackageDirectoryToolStripMenuItem.Text = "View package directory"
                ' OpenFileDialogs and FolderBrowsers
                OpenFileDialog1.Title = "Specify the project file to load"
                LocalMountDirFBD.Description = "Please specify the mount directory you want to load into this project:"
                If Not ImgBW.IsBusy And areBackgroundProcessesDone Then
                    BGProcDetails.Label2.Text = "Image processes have completed"
                End If
                MenuDesc.Text = "Ready"
                ' Tree view context menu
                AccessDirectoryToolStripMenuItem.Text = "Access directory"
                UnloadProjectToolStripMenuItem1.Text = "Unload project"
                CopyDeploymentToolsToolStripMenuItem.Text = "Copy deployment tools"
                OfAllArchitecturesToolStripMenuItem.Text = "Of all architectures"
                OfSelectedArchitectureToolStripMenuItem.Text = "Of selected architecture"
                ForX86ArchitectureToolStripMenuItem.Text = "For x86 architecture"
                ForAmd64ArchitectureToolStripMenuItem.Text = "For AMD64 architecture"
                ForARMArchitectureToolStripMenuItem.Text = "For ARM architecture"
                ForARM64ArchitectureToolStripMenuItem.Text = "For ARM64 architecture"
                ImageOperationsToolStripMenuItem.Text = "Image operations"
                MountImageToolStripMenuItem.Text = "Mount image..."
                UnmountImageToolStripMenuItem.Text = "Unmount image..."
                RemoveVolumeImagesToolStripMenuItem.Text = "Remove volume images..."
                SwitchImageIndexesToolStripMenuItem1.Text = "Switch image indexes..."
                UnattendedAnswerFilesToolStripMenuItem1.Text = "Unattended answer files"
                ManageToolStripMenuItem.Text = "Manage"
                CreationWizardToolStripMenuItem.Text = "Create"
                ScratchDirectorySettingsToolStripMenuItem.Text = "Configure scratch directory"
                ManageReportsToolStripMenuItem.Text = "Manage reports"
                AddToolStripMenuItem.Text = "Add"
                NewFileToolStripMenuItem.Text = "New file..."
                ExistingFileToolStripMenuItem.Text = "Existing file..."
                ' Context menu of AppX information dialog
                SaveResourceToolStripMenuItem.Text = "Save resource..."
                CopyToolStripMenuItem.Text = "Copy resource"
                ' Context menu of AppX addition dialog
                MicrosoftAppsToolStripMenuItem.Text = "Visit the Microsoft Apps website"
                MicrosoftStoreGenerationProjectToolStripMenuItem.Text = "Visit the Microsoft Store Generation Project website"
                ' New design
                GreetingLabel.Text = "Welcome to this servicing session"
                LinkLabel12.Text = "PROJECT"
                LinkLabel13.Text = "IMAGE"
                Label54.Text = "Name:"
                Label51.Text = "Location:"
                Label53.Text = "Images mounted?"
                LinkLabel14.Text = "Click here to mount an image"
                Label55.Text = "Project Tasks"
                LinkLabel15.Text = "View project properties"
                LinkLabel16.Text = "Open in File Explorer"
                LinkLabel17.Text = "Unload project"
                Label59.Text = "No image has been mounted"
                Label58.Text = "You need to mount an image in order to view its information"
                Label57.Text = "Choices"
                LinkLabel21.Text = "Mount an image..."
                LinkLabel18.Text = "Pick a mounted image..."
                Label39.Text = "Image index:"
                Label43.Text = "Mount point:"
                Label45.Text = "Version:"
                Label42.Text = "Name:"
                Label40.Text = "Description:"
                Label56.Text = "Image Tasks"
                LinkLabel20.Text = "View image properties"
                LinkLabel19.Text = "Unmount image"
                GroupBox4.Text = "Image operations"
                Button26.Text = "Mount image..."
                Button27.Text = "Commit current changes"
                Button28.Text = "Commit and unmount image"
                Button29.Text = "Unmount image discarding changes"
                Button25.Text = "Reload servicing session"
                Button24.Text = "Switch image indexes..."
                Button30.Text = "Apply image..."
                Button31.Text = "Capture image..."
                Button32.Text = "Remove volume images..."
                Button33.Text = "Save complete image information..."
                GroupBox5.Text = "Package operations"
                Button36.Text = "Add package..."
                Button34.Text = "Get package information..."
                Button38.Text = "Save installed package information..."
                Button35.Text = "Remove package..."
                Button37.Text = "Perform component store maintenance and cleanup..."
                GroupBox6.Text = "Feature operations"
                Button41.Text = "Enable feature..."
                Button39.Text = "Get feature information..."
                Button42.Text = "Save feature information..."
                Button40.Text = "Disable feature..."
                GroupBox7.Text = "AppX package operations"
                Button44.Text = "Add AppX package..."
                Button45.Text = "Get app information..."
                Button46.Text = "Save installed AppX package information..."
                Button43.Text = "Remove AppX package..."
                GroupBox8.Text = "Capability operations"
                Button48.Text = "Add capability..."
                Button49.Text = "Get capability information..."
                Button50.Text = "Save capability information..."
                Button47.Text = "Remove capability..."
                GroupBox9.Text = "Driver operations"
                Button53.Text = "Add driver package..."
                Button52.Text = "Get driver information..."
                Button54.Text = "Save installed driver information..."
                Button51.Text = "Remove driver..."
                GroupBox10.Text = "Windows PE operations"
                Button55.Text = "Get configuration"
                Button56.Text = "Save configuration..."
                Button57.Text = "Set target path..."
                Button58.Text = "Set scratch space..."
            Case 2
                ' Top-level menu items
                FileToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Archivo".ToUpper(), "&Archivo")
                ProjectToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Proyecto".ToUpper(), "&Proyecto")
                CommandsToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "Co&mandos".ToUpper(), "Co&mandos")
                ToolsToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "Her&ramientas".ToUpper(), "Her&ramientas")
                HelpToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "Ay&uda".ToUpper(), "Ay&uda")
                InvalidSettingsTSMI.Text = "Se han detectado configuraciones inválidas"
                ' Submenu items
                ' Menu - File
                NewProjectToolStripMenuItem.Text = "&Nuevo proyecto..."
                OpenExistingProjectToolStripMenuItem.Text = "&Abrir proyecto existente"
                ManageOnlineInstallationToolStripMenuItem.Text = "Administrar &instalación activa"
                ManageOfflineInstallationToolStripMenuItem.Text = "Administrar instalación &fuera de línea..."
                SaveProjectToolStripMenuItem.Text = "&Guardar proyecto..."
                SaveProjectasToolStripMenuItem.Text = "Guardar proyecto &como..."
                ExitToolStripMenuItem.Text = "Sa&lir"
                ' Menu - Project
                ViewProjectFilesInFileExplorerToolStripMenuItem.Text = "Ver archivos del proyecto en el Explorador de archivos"
                UnloadProjectToolStripMenuItem.Text = "Descargar proyecto..."
                SwitchImageIndexesToolStripMenuItem.Text = "Cambiar índices de imagen..."
                ProjectPropertiesToolStripMenuItem.Text = "Propiedades del proyecto"
                ImagePropertiesToolStripMenuItem.Text = "Propiedades de la imagen"
                ' Menu - Commands
                ImageManagementToolStripMenuItem.Text = "Administración de la imagen"
                OSPackagesToolStripMenuItem.Text = "Paquetes del sistema operativo"
                ProvisioningPackagesToolStripMenuItem.Text = "Paquetes de aprovisionamiento"
                AppPackagesToolStripMenuItem.Text = "Paquetes de aplicación"
                AppPatchesToolStripMenuItem.Text = "Servicio de aplicaciones (MSP)"
                DefaultAppAssociationsToolStripMenuItem.Text = "Asociaciones predeterminadas de aplicaciones"
                LanguagesAndRegionSettingsToolStripMenuItem.Text = "Configuración de idiomas y regiones"
                CapabilitiesToolStripMenuItem.Text = "Funcionalidades"
                WindowsEditionsToolStripMenuItem.Text = "Ediciones de Windows"
                DriversToolStripMenuItem.Text = "Controladores"
                UnattendedAnswerFilesToolStripMenuItem.Text = "Archivos de respuesta desatendida"
                WindowsPEServicingToolStripMenuItem.Text = "Servicio de Windows PE"
                OSUninstallToolStripMenuItem.Text = "Desinstalación del sistema operativo"
                ReservedStorageToolStripMenuItem.Text = "Almacenamiento reservado"
                ' Menu - Commands - Image management
                AppendImage.Text = "Anexar directorio de captura a imagen..."
                ApplyFFU.Text = "Aplicar archivo FFU o SFU..."
                ApplyImage.Text = "Aplicar archivo WIM o SWM..."
                CaptureCustomImage.Text = "Capturar cambios incrementales a un archivo..."
                CaptureFFU.Text = "Capturar particiones a un archivo FFU..."
                CaptureImage.Text = "Capturar imagen de un disco a un archivo WIM..."
                CleanupMountpoints.Text = "Eliminar recursos de una imagen corrupta..."
                CommitImage.Text = "Aplicar cambios a la imagen..."
                DeleteImage.Text = "Eliminar imágenes de volumen de un archivo WIM..."
                ExportImage.Text = "Exportar imagen..."
                GetImageInfo.Text = "Obtener información de imagen..."
                GetWIMBootEntry.Text = "Obtener entradas de configuración WIMBoot..."
                ListImage.Text = "Enumerar archivos y directorios de un archivo WIM..."
                MountImage.Text = "Montar imagen..."
                OptimizeFFU.Text = "Optimizar archivo FFU..."
                OptimizeImage.Text = "Optimizar imagen..."
                RemountImage.Text = "Remontar imagen para su servicio..."
                SplitFFU.Text = "Dividir archivo FFU en archivos SFU..."
                SplitImage.Text = "Dividir archivo WIM en archivos SWM..."
                UnmountImage.Text = "Desmontar imagen..."
                UpdateWIMBootEntry.Text = "Actualizar entradas de configuración WIMBoot..."
                ApplySiloedPackage.Text = "Aplicar paquete de aprovisionamiento en silos..."
                SaveImageInformationToolStripMenuItem.Text = "Guardar información de la imagen..."
                ' Menu - Commands - OS packages
                GetPackages.Text = "Obtener información de paquetes..."
                AddPackage.Text = "Añadir paquete..."
                RemovePackage.Text = "Eliminar paquete..."
                GetFeatures.Text = "Obtener información de características..."
                EnableFeature.Text = "Habilitar característica..."
                DisableFeature.Text = "Deshabilitar característica..."
                CleanupImage.Text = "Realizar operaciones de limpieza o recuperación..."
                ' Menu - Commands - Provisioning packages
                AddProvisioningPackage.Text = "Añadir paquete de aprovisionamiento..."
                GetProvisioningPackageInfo.Text = "Obtener información de paquete de aprovisionamiento..."
                ApplyCustomDataImage.Text = "Aplicar imagen de datos personalizada..."
                ' Menu - Commands - App packages
                GetProvisionedAppxPackages.Text = "Obtener información de paquete de aplicación..."
                AddProvisionedAppxPackage.Text = "Añadir paquete de aplicación aprovisionada..."
                RemoveProvisionedAppxPackage.Text = "Eliminar aprovisionamiento para un paquete de aplicación..."
                OptimizeProvisionedAppxPackages.Text = "Optimizar paquete de aprovisionamiento..."
                SetProvisionedAppxDataFile.Text = "Añadir archivo de datos personalizado en paquete de aplicación..."
                ' Menu - Commands - App (MSP) servicing
                CheckAppPatch.Text = "Obtener información de parche de aplicación..."
                GetAppPatchInfo.Text = "Obtener información detallada de parches de aplicación instalados..."
                GetAppPatches.Text = "Obtener información básica de parches de aplicación instalados..."
                GetAppInfo.Text = "Obtener información detallada de aplicaciones de Windows Installer (*.msi)..."
                GetApps.Text = "Obtener información básica de aplicaciones de Windows Installer (*.msi)..."
                ' Menu - Commands - Default app associations
                ExportDefaultAppAssociations.Text = "Exportar asociaciones de aplicaciones predeterminadas..."
                GetDefaultAppAssociations.Text = "Obtener información de asociaciones de aplicaciones predeterminadas..."
                ImportDefaultAppAssociations.Text = "Importar asociaciones de aplicaciones predeterminadas..."
                RemoveDefaultAppAssociations.Text = "Eliminar asociaciones de aplicaciones predeterminadas..."
                ' Menu - Commands - Languages and regional settings
                GetIntl.Text = "Obtener configuraciones e idiomas internacionales..."
                SetUILang.Text = "Establecer idioma de la interfaz de usuario..."
                SetUILangFallback.Text = "Establecer idioma predeterminado de la interfaz de usuario de último recurso..."
                SetSysUILang.Text = "Estabñecer idioma de la interfaz de usuario preferido para el sistema..."
                SetSysLocale.Text = "Establecer zona del sistema..."
                SetUserLocale.Text = "Establecer zona del usuario..."
                SetInputLocale.Text = "Establecer zona de entrada..."
                SetAllIntl.Text = "Establecer idioma de la interfaz de usuario y zonas..."
                SetTimeZone.Text = "Establecer zona horaria predeterminada..."
                SetSKUIntlDefaults.Text = "Establecer lenguajes y zonas predeterminadas..."
                SetLayeredDriver.Text = "Establecer controlador en capas..."
                GenLangINI.Text = "Generar archivo Lang.ini..."
                SetSetupUILang.Text = "Establecer idioma predeterminado del programa de instalación..."
                ' Menu - Commands - Capabilities
                AddCapability.Text = "Añadir funcionalidad..."
                ExportSource.Text = "Exportar funcionalidades en un repositorio..."
                GetCapabilities.Text = "Obtener información de funcionalidades..."
                RemoveCapability.Text = "Eliminar funcionalidad..."
                ' Menu - Commands - Windows editions
                GetCurrentEdition.Text = "Obtener edición actual..."
                GetTargetEditions.Text = "Obtener destinos de actualización..."
                SetEdition.Text = "Actualizar imagen..."
                SetProductKey.Text = "Establecer clave de producto..."
                ' Menu - Commands - Drivers
                GetDrivers.Text = "Obtener información de controladores..."
                AddDriver.Text = "Añadir controlador..."
                RemoveDriver.Text = "Eliminar controlador..."
                ExportDriver.Text = "Exportar paquetes de controlador..."
                ' Menu - Commands - Unattended answer files
                ApplyUnattend.Text = "Aplicar archivo de respuesta desatendida..."
                ' Menu - Commands - Windows PE servicing
                GetPESettings.Text = "Obtener configuración..."
                SetScratchSpace.Text = "Establecer espacio temporal..."
                SetTargetPath.Text = "Establecer ruta de destino..."
                ' Menu - Commands - OS uninstall
                GetOSUninstallWindow.Text = "Obtener margen de desinstalación..."
                InitiateOSUninstall.Text = "Iniciar desinstalación..."
                RemoveOSUninstall.Text = "Eliminar habilidad de desinstalación..."
                SetOSUninstallWindow.Text = "Establecer margen de desinstalación..."
                ' Menu - Commands - Reserved storage
                SetReservedStorageState.Text = "Establecer estado de almacenamiento reservado..."
                GetReservedStorageState.Text = "Obtener estado de almacenamiento reservado..."
                ' Menu - Commands - Microsoft Edge
                AddEdge.Text = "Añadir Edge..."
                AddEdgeBrowser.Text = "Añadir navegador Edge..."
                AddEdgeWebView.Text = "Añadir Edge WebView..."
                ' Menu - Tools
                ImageConversionToolStripMenuItem.Text = "Conversión de imágenes"
                MergeSWM.Text = "Combinar archivos SWM..."
                RemountImageWithWritePermissionsToolStripMenuItem.Text = "Remontar imagen con permisos de escritura"
                CommandShellToolStripMenuItem.Text = "Consola de comandos"
                UnattendedAnswerFileManagerToolStripMenuItem.Text = "Administrador de archivos de respuesta desatendida"
                ReportManagerToolStripMenuItem.Text = "Administrador de informes"
                MountedImageManagerTSMI.Text = "Administrador de imágenes montadas"
                WimScriptEditorCommand.Text = "Editor de lista de configuraciones"
                ActionEditorToolStripMenuItem.Text = "Editor de acciones"
                OptionsToolStripMenuItem.Text = "Opciones"
                ' Menu - Help
                HelpTopicsToolStripMenuItem.Text = "Ver la ayuda"
                GlossaryToolStripMenuItem.Text = "Glosario"
                CommandHelpToolStripMenuItem.Text = "Ayuda de comandos..."
                AboutDISMToolsToolStripMenuItem.Text = "Acerca de DISMTools"
                ' Menu - Invalid settings
                ISFix.Text = "Más información"
                ISHelp.Text = "¿Qué es esto?"
                ' Menu - DevState
                ReportFeedbackToolStripMenuItem.Text = "Enviar comentarios (se abre en navegador web)"
                ' Menu - Contributions
                ContributeToTheHelpSystemToolStripMenuItem.Text = "Contribuir al sistema de ayuda"
                ' Start Panel
                LabelHeader1.Text = "Comenzar"
                Label10.Text = "Proyectos recientes"
                Label11.Text = "¡Próximamente!"
                NewProjLink.Text = "Nuevo proyecto..."
                ExistingProjLink.Text = "Abrir proyecto existente..."
                OnlineInstMgmt.Text = "Administrar instalación activa"
                OfflineInstMgmt.Text = "Administrar instalación fuera de línea..."
                ' Start Panel tabs
                WelcomeTab.Text = "Bienvenido"
                NewsFeedTab.Text = "Últimas noticias"
                VideosTab.Text = "Tutoriales"
                ' Welcome tab contents
                Label24.Text = "Bienvenido a DISMTools"
                Label25.Text = "La interfaz gráfica para realizar operaciones de DISM."
                Label26.Text = "Este es un software en beta"
                Label27.Text = "Ahora mismo este programa está en beta. Esto significa que muchas cosas no funcionarán como esperado. También habrán muchos errores y, en general, este programa no está completo (como puede ver ahora mismo)"
                Label28.Text = "Este programa es de código abierto"
                Label29.Text = "Este programa es de código abierto, lo que le permite observar cómo funciona y entenderlo mejor."
                Label30.Text = "Asegúrese de saber lo que hace"
                Label31.Text = "Por favor, tenga cuidado con lo que hace en este programa. Aunque parezca simple y fácil de usar, está destinado a administradores del sistema y gente en el departamento de TI. Si realiza operaciones sin cuidado a una imagen o instalación activa, podría no funcionar correctamente o dejar de funcionar."
                Label32.Text = "Comencemos"
                Label33.Text = "- ¿Está perdido en este programa? Le recomendamos que vea la ayuda para manejarse" & CrLf & _
                    "- ¿No entiende un término en específico? Le recomendamos comprobar el glosario. Los términos están ordenados alfabéticamente y ofrecen explicaciones lo más detalladas posible" & CrLf & _
                    "- ¿Desea saber cómo hacerlo en la línea de comandos? Le recomendamos comprobar la ayuda de comandos" & CrLf & _
                    "Estas opciones, y la información del programa, pueden ser encontradas en el menú Ayuda."
                ' ToolStrip buttons
                ToolStripButton1.Text = "Cerrar pestaña"
                ToolStripButton2.Text = "Guardar proyecto"
                ToolStripButton3.Text = "Descargar proyecto"
                ToolStripButton3.ToolTipText = "Descargar proyecto de este programa"
                ToolStripButton4.Text = "Mostrar ventana de progreso"
                RefreshViewTSB.Text = "Actualizar vista"
                ExpandCollapseTSB.Text = "Expandir"
                ' TabPages
                TabPage1.Text = "Proyecto"
                TabPage2.Text = "Imagen"
                TabPage3.Text = "Acciones"
                ' TabPage controls
                UnloadBtn.Text = "Descargar proyecto"
                ExplorerView.Text = "Ver en Explorador de archivos"
                Button14.Text = "Ver propiedades del proyecto"
                Button15.Text = "Ver propiedades de la imagen"
                Button16.Text = "Desmontar imagen..."
                TabPageTitle1.Text = "Proyecto"
                TabPageTitle2.Text = "Imagen"
                TabPageDescription1.Text = "Ver información del proyecto"
                TabPageDescription2.Text = "Ver información de la imagen"
                Label1.Text = "Nombre:"
                Label2.Text = "Ubicación:"
                Label4.Text = "¿Hay imágenes montadas?"
                Label5.Text = If(IsImageMounted, "Sí", "No")
                LinkLabel1.Text = "Haga clic aquí para montar una imagen"
                Label23.Text = "No se ha montado una imagen"
                LinkLabel2.Text = "Necesita montar una imagen para ver su información aquí. Haga clic aquí para montar una imagen."
                LinkLabel2.LinkArea = New LinkArea(67, 4)
                LinkLabel3.Text = "O, si tiene una imagen montada, abra un directorio de montaje existente"
                LinkLabel3.LinkArea = New LinkArea(32, 40)
                UpdateLink.Text = "Hay una nueva versión disponible para su descarga e instalación. Haga clic aquí para saber más"
                UpdateLink.LinkArea = New LinkArea(65, 29)
                Label15.Text = "Índice:"
                Label13.Text = "Punto de montaje:"
                Label16.Text = "Versión:"
                Label19.Text = "Nombre:"
                Label21.Text = "Descripción:"
                ' Actions
                GroupBox1.Text = "Operaciones de la imagen"
                GroupBox2.Text = "Operaciones de paquetes"
                GroupBox3.Text = "Operaciones de características"
                Button1.Text = "Montar imagen..."
                Button2.Text = "Guardar cambios"
                Button3.Text = "Guardar y desmontar imagen"
                Button4.Text = "Descartar y desmontar imagen"
                Button5.Text = "Añadir paquete..."
                Button6.Text = "Obtener información de paquetes..."
                Button7.Text = "Eliminar paquete..."
                Button8.Text = "Obtener información de características..."
                Button9.Text = "Deshabilitar característica..."
                Button10.Text = "Habilitar característica..."
                Button11.Text = "Recargar sesión de servicio..."
                Button12.Text = "Realizar limpieza y/o reparación de componentes..."
                Button13.Text = "Cambiar índices..."
                Button19.Text = "Ver el nuevo diseño"
                Button20.Text = "Regresar al diseño antiguo"
                ' Pop-up context menus
                PkgBasicInfo.Text = "Obtener información básica (todos los paquetes)"
                PkgDetailedInfo.Text = "Obtener información detallada (paquete específico)"
                CommitAndUnmountTSMI.Text = "Guardar cambios y desmontar imagen"
                DiscardAndUnmountTSMI.Text = "Descartar cambios y desmontar imagen"
                UnmountSettingsToolStripMenuItem.Text = "Configuración de desmontaje..."
                ViewPackageDirectoryToolStripMenuItem.Text = "Ver directorio del paquete"
                ' OpenFileDialogs and FolderBrowsers
                OpenFileDialog1.Title = "Especifique el archivo de proyecto a cargar"
                LocalMountDirFBD.Description = "Especifique el directorio de montaje que desea cargar en este proyecto:"
                If Not ImgBW.IsBusy And areBackgroundProcessesDone Then
                    BGProcDetails.Label2.Text = "Los procesos de la imagen han completado"
                End If
                MenuDesc.Text = "Listo"
                ' Tree view context menu
                AccessDirectoryToolStripMenuItem.Text = "Acceder directorio"
                UnloadProjectToolStripMenuItem1.Text = "Descargar proyecto"
                CopyDeploymentToolsToolStripMenuItem.Text = "Copiar herramientas de implementación"
                OfAllArchitecturesToolStripMenuItem.Text = "De todas las arquitecturas"
                OfSelectedArchitectureToolStripMenuItem.Text = "De la arquitectura seleccionada"
                ForX86ArchitectureToolStripMenuItem.Text = "Para arquitectura x86"
                ForAmd64ArchitectureToolStripMenuItem.Text = "Para arquitectura AMD64"
                ForARMArchitectureToolStripMenuItem.Text = "Para arquitectura ARM"
                ForARM64ArchitectureToolStripMenuItem.Text = "Para arquitectura ARM64"
                ImageOperationsToolStripMenuItem.Text = "Operaciones de la imagen"
                MountImageToolStripMenuItem.Text = "Montar imagen..."
                UnmountImageToolStripMenuItem.Text = "Desmontar imagen..."
                RemoveVolumeImagesToolStripMenuItem.Text = "Eliminar imágenes de volumen..."
                SwitchImageIndexesToolStripMenuItem1.Text = "Cambiar índices de imagen..."
                UnattendedAnswerFilesToolStripMenuItem1.Text = "Archivos de respuesta desatendida"
                ManageToolStripMenuItem.Text = "Administrar"
                CreationWizardToolStripMenuItem.Text = "Crear"
                ScratchDirectorySettingsToolStripMenuItem.Text = "Configurar directorio temporal"
                ManageReportsToolStripMenuItem.Text = "Administrar informes"
                AddToolStripMenuItem.Text = "Añadir"
                NewFileToolStripMenuItem.Text = "Nuevo archivo..."
                ExistingFileToolStripMenuItem.Text = "Archivo existente..."
                SaveResourceToolStripMenuItem.Text = "Guardar recurso..."
                CopyToolStripMenuItem.Text = "Copiar recurso"
                ' Context menu of AppX addition dialog
                MicrosoftAppsToolStripMenuItem.Text = "Visitar el sitio web de Aplicaciones de Microsoft"
                MicrosoftStoreGenerationProjectToolStripMenuItem.Text = "Visitar el sitio web del proyecto de generación de Microsoft Store"
                ' New design
                GreetingLabel.Text = "Le damos la bienvenida a esta sesión de servicio"
                LinkLabel12.Text = "PROYECTO"
                LinkLabel13.Text = "IMAGEN"
                Label54.Text = "Nombre:"
                Label51.Text = "Ubicación:"
                Label53.Text = "¿Hay imágenes montadas?"
                LinkLabel14.Text = "Haga clic aquí para montar una imagen"
                Label55.Text = "Tareas del proyecto"
                LinkLabel15.Text = "Ver propiedades del proyecto"
                LinkLabel16.Text = "Abrir en el Explorador de Archivos"
                LinkLabel17.Text = "Descargar proyecto"
                Label59.Text = "No se ha montado una imagen"
                Label58.Text = "Debe montar una imagen para poder ver su información"
                Label57.Text = "Elecciones"
                LinkLabel21.Text = "Montar una imagen..."
                LinkLabel18.Text = "Escoger una imagen montada..."
                Label39.Text = "Índice de la imagen:"
                Label43.Text = "Punto de montaje:"
                Label45.Text = "Versión:"
                Label42.Text = "Nombre:"
                Label40.Text = "Descripción:"
                Label56.Text = "Tareas de la imagen"
                LinkLabel20.Text = "Ver propiedades de la imagen"
                LinkLabel19.Text = "Desmontar imagen"
                GroupBox4.Text = "Operaciones de la imagen"
                Button26.Text = "Montar imagen..."
                Button27.Text = "Guardar cambios actuales"
                Button28.Text = "Guardar cambios y desmontar imagen"
                Button29.Text = "Desmontar imagen descartando cambios"
                Button25.Text = "Recargar sesión de servicio"
                Button24.Text = "Cambiar índices de la imagen..."
                Button30.Text = "Aplicar imagen..."
                Button31.Text = "Capturar imagen..."
                Button32.Text = "Eliminar imágenes de volumen..."
                Button33.Text = "Guardar información completa de la imagen..."
                GroupBox5.Text = "Operaciones de paquetes"
                Button36.Text = "Añadir paquete..."
                Button34.Text = "Obtener información de paquetes..."
                Button38.Text = "Guardar información de paquetes instalados..."
                Button35.Text = "Eliminar paquete..."
                Button37.Text = "Realizar mantenimiento y limpieza del almacén de componentes..."
                GroupBox6.Text = "Operaciones de características"
                Button41.Text = "Habilitar característica..."
                Button39.Text = "Obtener información de características..."
                Button42.Text = "Guardar información de características..."
                Button40.Text = "Deshabilitar característica..."
                GroupBox7.Text = "Operaciones de paquetes AppX"
                Button44.Text = "Añadir paquete AppX..."
                Button45.Text = "Obtener información de aplicaciones..."
                Button46.Text = "Guardar información de paquetes AppX instalados..."
                Button43.Text = "Eliminar paquete AppX..."
                GroupBox8.Text = "Operaciones de funcionalidades"
                Button48.Text = "Añadir funcionalidad..."
                Button49.Text = "Obtener información de funcionalidades..."
                Button50.Text = "Guardar información de funcionalidades..."
                Button47.Text = "Eliminar funcionalidades..."
                GroupBox9.Text = "Operaciones de controladores"
                Button53.Text = "Añadir controlador..."
                Button52.Text = "Obtener información de controladores..."
                Button54.Text = "Guardar información de controladores instalados..."
                Button51.Text = "Eliminar controlador..."
                GroupBox10.Text = "Operaciones de Windows PE"
                Button55.Text = "Obtener configuración"
                Button56.Text = "Guardar configuración..."
                Button57.Text = "Establecer ruta de destino..."
                Button58.Text = "Establecer espacio temporal..."
            Case 3
                ' Top-level menu items
                FileToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Fichier".ToUpper(), "&Fichier")
                ProjectToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Projet".ToUpper(), "&Projet")
                CommandsToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "Com&mandes".ToUpper(), "Com&mandes")
                ToolsToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "Ou&tils".ToUpper(), "Ou&tils")
                HelpToolStripMenuItem.Text = If(Options.CheckBox9.Checked, "&Aide".ToUpper(), "&Aide")
                InvalidSettingsTSMI.Text = "Des paramètres non valides ont été détectés"
                ' Submenu items
                ' Menu - File
                NewProjectToolStripMenuItem.Text = "&Nouveau projet..."
                OpenExistingProjectToolStripMenuItem.Text = "&Ouvrir un projet existant"
                ManageOnlineInstallationToolStripMenuItem.Text = "&Gérer l'installation en ligne"
                ManageOfflineInstallationToolStripMenuItem.Text = "Gérer l'installation &hors ligne..."
                SaveProjectToolStripMenuItem.Text = "&Sauvegarder le projet..."
                SaveProjectasToolStripMenuItem.Text = "Sauvegarder le projet so&us..."
                ExitToolStripMenuItem.Text = "Sor&tir"
                ' Menu - Project
                ViewProjectFilesInFileExplorerToolStripMenuItem.Text = "Visualiser les fichiers du projet dans l'explorateur de fichiers"
                UnloadProjectToolStripMenuItem.Text = "Décharget le projet..."
                SwitchImageIndexesToolStripMenuItem.Text = "Changer d'index de l'image..."
                ProjectPropertiesToolStripMenuItem.Text = "Propriétés du projet"
                ImagePropertiesToolStripMenuItem.Text = "Propriétés de l'image"
                ' Menu - Commands
                ImageManagementToolStripMenuItem.Text = "Gestion des images"
                OSPackagesToolStripMenuItem.Text = "Paquets de systèmes d'exploitation"
                ProvisioningPackagesToolStripMenuItem.Text = "Paquets de provisionnement"
                AppPackagesToolStripMenuItem.Text = "Paquets d'applications"
                AppPatchesToolStripMenuItem.Text = "Maintenance des applications (MSP)"
                DefaultAppAssociationsToolStripMenuItem.Text = "Associations d'applications par défaut"
                LanguagesAndRegionSettingsToolStripMenuItem.Text = "Langues et paramètres régionaux"
                CapabilitiesToolStripMenuItem.Text = "Capacités"
                WindowsEditionsToolStripMenuItem.Text = "Éditions Windows"
                DriversToolStripMenuItem.Text = "Pilotes"
                UnattendedAnswerFilesToolStripMenuItem.Text = "Fichiers de réponse non surveillés"
                WindowsPEServicingToolStripMenuItem.Text = "Maintenance de Windows PE"
                OSUninstallToolStripMenuItem.Text = "Désinstallation du système d'exploitation"
                ReservedStorageToolStripMenuItem.Text = "Stockage réservé"
                ' Menu - Commands - Image management
                AppendImage.Text = "Ajouter le répertoire de capture à l'image..."
                ApplyFFU.Text = "Appliquer le fichier FFU ou SFU..."
                ApplyImage.Text = "Appliquer le fichier WIM ou SWM..."
                CaptureCustomImage.Text = "Capturer les modifications incrémentales d'un fichier..."
                CaptureFFU.Text = "Capturer des partitions dans un fichier FFU..."
                CaptureImage.Text = "Capturer l'image d'un lecteur dans un fichier WIM..."
                CleanupMountpoints.Text = "Supprimer les resources d'une image corrompue..."
                CommitImage.Text = "Appliquer les modifications à l'image..."
                DeleteImage.Text = "Supprimer les images de volume du fichier WIM..."
                ExportImage.Text = "Exporter l'image..."
                GetImageInfo.Text = "Obtenir des informations sur l'image..."
                GetWIMBootEntry.Text = "Obtenir les entrées de configuration WIMBoot..."
                ListImage.Text = "Lister des fichiers et répertoires dans l'image..."
                MountImage.Text = "Monter l'image..."
                OptimizeFFU.Text = "Optimiser le fichier FFU..."
                OptimizeImage.Text = "Optimiser l'image..."
                RemountImage.Text = "Remonter l'image pour la maintenance..."
                SplitFFU.Text = "Diviser un fichier FFU en fichiers SFU..."
                SplitImage.Text = "Diviser un fichier WIM en fichiers SWM..."
                UnmountImage.Text = "Démonter l'image..."
                UpdateWIMBootEntry.Text = "Mettre à jour de l'entrée de configuration de WIMBoot..."
                ApplySiloedPackage.Text = "Appliquer un package de provisionnement en silo..."
                SaveImageInformationToolStripMenuItem.Text = "Sauvegarder les informations de l'image..."
                ' Menu - Commands - OS packages
                GetPackages.Text = "Obtenir des informations sur le paquet..."
                AddPackage.Text = "Ajouter un paquet..."
                RemovePackage.Text = "Supprimer le paquet..."
                GetFeatures.Text = "Obtenir des informations sur les caractéristiques..."
                EnableFeature.Text = "Activer la caractéristique..."
                DisableFeature.Text = "Désactiver la caractéristique..."
                CleanupImage.Text = "Effectuer des opérations de nettoyage ou de récupération..."
                ' Menu - Commands - Provisioning packages
                AddProvisioningPackage.Text = "Ajouter un paquet de provisionnement..."
                GetProvisioningPackageInfo.Text = "Obtenir des informations sur le paquet de provisionnement..."
                ApplyCustomDataImage.Text = "Appliquer une image de données personnalisée..."
                ' Menu - Commands - App packages
                GetProvisionedAppxPackages.Text = "Obtenir des informations sur le paquet d'applications..."
                AddProvisionedAppxPackage.Text = "Ajouter un paquet d'applications provisionnées..."
                RemoveProvisionedAppxPackage.Text = "Supprimer le provisionnement pour les paquets d'applications..."
                OptimizeProvisionedAppxPackages.Text = "Optimiser les paquets provisionnés..."
                SetProvisionedAppxDataFile.Text = "Ajouter un fichier de données personnalisé dans le paquet d'applications..."
                ' Menu - Commands - App (MSP) servicing
                CheckAppPatch.Text = "Obtenir des informations sur les correctifs de l'application..."
                GetAppPatchInfo.Text = "Obtenir des informations détaillées sur les correctifs des applications..."
                GetAppPatches.Text = "Obtenir des informations basiques sur les correctifs des applications installées..."
                GetAppInfo.Text = "Obtenir des informations détaillées sur l'application Windows Installer (*.msi)..."
                GetApps.Text = "Obtenir des informations basiques sur l'application Windows Installer (*.msi)..."
                ' Menu - Commands - Default app associations
                ExportDefaultAppAssociations.Text = "Exporter les associations d'applications par défaut..."
                GetDefaultAppAssociations.Text = "Obtenir des informations sur l'association d'applications par défaut..."
                ImportDefaultAppAssociations.Text = "Importer les associations d'applications par défaut..."
                RemoveDefaultAppAssociations.Text = "Supprimer les associations d'applications par défaut..."
                ' Menu - Commands - Languages and regional settings
                GetIntl.Text = "Obtenir des paramètres et des langues internationaux..."
                SetUILang.Text = "Définir la langue de l'interface utilisateur..."
                SetUILangFallback.Text = "Définir la langue par défaut de l'interface utilisateur..."
                SetSysUILang.Text = "Définir la langue préférée de l'interface utilisateur du système..."
                SetSysLocale.Text = "Définir les paramètres linguistiques du système..."
                SetUserLocale.Text = "Définir les paramètres linguistiques de l'utilisateur..."
                SetInputLocale.Text = "Définir la langue d'entrée..."
                SetAllIntl.Text = "Définir la langue de l'interface utilisateur et les paramètres locaux..."
                SetTimeZone.Text = "Définir le fuseau horaire par défaut..."
                SetSKUIntlDefaults.Text = "Définir les langues et les locales par défaut..."
                SetLayeredDriver.Text = "Régler le pilote en couches..."
                GenLangINI.Text = "Générer le fichier Lang.ini..."
                SetSetupUILang.Text = "Définir la langue d'installation par défaut..."
                ' Menu - Commands - Capabilities
                AddCapability.Text = "Ajouter une capacité..."
                ExportSource.Text = "Exporter les capacités dans le référentiel..."
                GetCapabilities.Text = "Obtenir des informations sur les capacités..."
                RemoveCapability.Text = "Supprimer la capacité..."
                ' Menu - Commands - Windows editions
                GetCurrentEdition.Text = "Obtenir l'édition actuelle..."
                GetTargetEditions.Text = "Obtenir des objectifs de mise à niveau..."
                SetEdition.Text = "Mettre à jour l'image..."
                SetProductKey.Text = "Définir la clé de produit..."
                ' Menu - Commands - Drivers
                GetDrivers.Text = "Obtenir des informations sur le pilote..."
                AddDriver.Text = "Ajouter un pilote..."
                RemoveDriver.Text = "Retirer le pilote..."
                ExportDriver.Text = "Exporter des paquets de pilotes..."
                ' Menu - Commands - Unattended answer files
                ApplyUnattend.Text = "Appliquer un fichier de réponse non surveillé..."
                ' Menu - Commands - Windows PE servicing
                GetPESettings.Text = "Obtenir des paramètres..."
                SetScratchSpace.Text = "Définir l'espace temporaire..."
                SetTargetPath.Text = "Définir le chemin cible..."
                ' Menu - Commands - OS uninstall
                GetOSUninstallWindow.Text = "Obtenir la créneau de désinstallation..."
                InitiateOSUninstall.Text = "Démarrer la désinstallation..."
                RemoveOSUninstall.Text = "Supprimer la possibilité de revenir en arrière..."
                SetOSUninstallWindow.Text = "Définir la créneau de désinstallation..."
                ' Menu - Commands - Reserved storage
                SetReservedStorageState.Text = "Définir l'état du stockage réservé..."
                GetReservedStorageState.Text = "Obtenir l'état du stockage réservé..."
                ' Menu - Commands - Microsoft Edge
                AddEdge.Text = "Ajouter Edge..."
                AddEdgeBrowser.Text = "Ajouter le navigateur Edge..."
                AddEdgeWebView.Text = "Ajouter Edge WebView..."
                ' Menu - Tools
                ImageConversionToolStripMenuItem.Text = "Conversion des images"
                MergeSWM.Text = "Fusionner des fichiers SWM..."
                RemountImageWithWritePermissionsToolStripMenuItem.Text = "Remonter l'image avec les droits d'écriture"
                CommandShellToolStripMenuItem.Text = "Console de commande"
                UnattendedAnswerFileManagerToolStripMenuItem.Text = "Gestionnaire de fichiers de réponse sans surveillance"
                ReportManagerToolStripMenuItem.Text = "Gestionnaire de rapports"
                MountedImageManagerTSMI.Text = "Gestionnaire des images montées"
                WimScriptEditorCommand.Text = "Éditeur de listes de configuration"
                ActionEditorToolStripMenuItem.Text = "Éditeur des actions"
                OptionsToolStripMenuItem.Text = "Paramètres"
                ' Menu - Help
                HelpTopicsToolStripMenuItem.Text = "Rubriques d'aide"
                GlossaryToolStripMenuItem.Text = "Glossaire"
                CommandHelpToolStripMenuItem.Text = "Aide à la commande..."
                AboutDISMToolsToolStripMenuItem.Text = "À propos de DISMTools"
                ' Menu - Invalid settings
                ISFix.Text = "Plus d'informations"
                ISHelp.Text = "Qu'est-ce que c'est ?"
                ' Menu - DevState
                ReportFeedbackToolStripMenuItem.Text = "Rapport de rétroaction (s'ouvre dans un navigateur web)"
                ' Menu - Contributions
                ContributeToTheHelpSystemToolStripMenuItem.Text = "Contribuer au système d'aide"
                ' Start Panel
                LabelHeader1.Text = "Commencer"
                Label10.Text = "Projets récents"
                Label11.Text = "A venir !"
                NewProjLink.Text = "Nouveau projet..."
                ExistingProjLink.Text = "Ouvrir un projet existant..."
                OnlineInstMgmt.Text = "Gérer l'installation en ligne"
                OfflineInstMgmt.Text = "Gérer l'installation hors ligne..."
                ' Start Panel tabs
                WelcomeTab.Text = "Bienvenue"
                NewsFeedTab.Text = "Dernières nouvelles"
                VideosTab.Text = "Vidéos tutorielles"
                ' Welcome tab contents
                Label24.Text = "Bienvenue à DISMTools"
                Label25.Text = "L'interface graphique pour effectuer les opérations DISM."
                Label26.Text = "Il s'agit d'une version bêta du logiciel"
                Label27.Text = "Ce programme est actuellement en version bêta. Cela signifie que beaucoup de choses ne fonctionneront pas comme prévu. Il y aura également de nombreux bugs et, de manière générale, le programme est incomplet (comme vous pouvez le constater dès à présent)."
                Label28.Text = "Ce programme est open-source"
                Label29.Text = "Ce programme est open-source, ce qui signifie que vous pouvez jeter un coup d'œil à son fonctionnement et mieux le comprendre."
                Label30.Text = "Assurez-vous de savoir ce que vous faites"
                Label31.Text = "Faites attention à ce que vous faites dans ce programme. Bien qu'il puisse paraître simple et facile à utiliser, il est destiné aux administrateurs système et aux personnes travaillant dans le département informatique. Si vous effectuez sans précaution des opérations sur une image ou une installation active, il se peut qu'elle ne fonctionne pas correctement ou qu'elle refuse de fonctionner."
                Label32.Text = "Pour commencer"
                Label33.Text = "- Vous vous êtes perdu dans ce programme ? Nous vous recommandons de consulter les rubriques d'aide pour vous y retrouver" & CrLf & _
                    "- Vous ne comprenez pas un terme spécifique ? Nous vous recommandons de consulter le glossaire. Les termes sont classés par ordre alphabétique et fournissent des explications aussi détaillées que possible" & CrLf & _
                    "- Vous voulez savoir comment le faire en ligne de commande ? Nous vous recommandons de consulter la commande help" & CrLf & _
                    "Ces options, ainsi que les informations sur le programme, peuvent être trouvées en ouvrant le menu Aide."
                ' ToolStrip buttons
                ToolStripButton1.Text = "Fermer l'onglet"
                ToolStripButton2.Text = "Sauvegarder le projet"
                ToolStripButton3.Text = "Décharger le projet"
                ToolStripButton3.ToolTipText = "Décharger le projet de ce programme"
                ToolStripButton4.Text = "Afficher la fenêtre de progression"
                RefreshViewTSB.Text = "Rafraîchir la vue"
                ExpandCollapseTSB.Text = "Élargir"
                ' TabPages
                TabPage1.Text = "Projet"
                TabPage2.Text = "Image"
                TabPage3.Text = "Actions"
                ' TabPage controls
                UnloadBtn.Text = "Décharger le projet"
                ExplorerView.Text = "Voir dans l'explorateur de fichiers"
                Button14.Text = "Voir les propriétés du projet"
                Button15.Text = "Voir les propriétés de l'image"
                Button16.Text = "Démonter l'image..."
                TabPageTitle1.Text = "Projet"
                TabPageTitle2.Text = "Image"
                TabPageDescription1.Text = "Voir les informations sur le projet"
                TabPageDescription2.Text = "Voir les informations sur l'image"
                Label1.Text = "Nom:"
                Label2.Text = "Emplacement:"
                Label4.Text = "Images montées?"
                Label5.Text = If(IsImageMounted, "Oui", "Non")
                LinkLabel1.Text = "Cliquez ici pour monter une image"
                Label23.Text = "Aucune image n'a été montée"
                LinkLabel2.Text = "Vous devez monter une image pour pouvoir afficher ses informations ici. Cliquez ici pour monter une image."
                LinkLabel2.LinkArea = New LinkArea(80, 3)
                LinkLabel3.Text = "Ou, si vous avez une image montée, ouvrez un répertoire de montage existant"
                LinkLabel3.LinkArea = New LinkArea(35, 40)
                UpdateLink.Text = "Une nouvelle version est disponible pour le téléchargement et l'installation. Cliquez ici pour en savoir plus"
                UpdateLink.LinkArea = New LinkArea(78, 31)
                Label15.Text = "Index de l'image:"
                Label13.Text = "Point de montage:"
                Label16.Text = "Version:"
                Label19.Text = "Nom:"
                Label21.Text = "Description:"
                ' Actions
                GroupBox1.Text = "Opérations sur les images"
                GroupBox2.Text = "Opérations sur les paquets"
                GroupBox3.Text = "Opérations sur les caractéristiques"
                Button1.Text = "Monter l'image..."
                Button2.Text = "Appliquer les modifications en cours"
                Button3.Text = "Appliquer les modifications et démonter l'image"
                Button4.Text = "Démonter l'image en ignorant les modifications"
                Button5.Text = "Ajouter un paquet..."
                Button6.Text = "Obtenir des informations sur le paquet..."
                Button7.Text = "Supprimer le paquet..."
                Button8.Text = "Obtenir des informations sur les caractéristiques..."
                Button9.Text = "Désactiver la caractéristique..."
                Button10.Text = "Activer la caractéristique..."
                Button11.Text = "Recharger la session de maintenance..."
                Button12.Text = "Effectuer le nettoyage et/ou la réparation des composants..."
                Button13.Text = "Changer d'index de l'image..."
                Button19.Text = "Prévisualiser le nouveau design"
                Button20.Text = "Revenir à l'ancien design"
                ' Pop-up context menus
                PkgBasicInfo.Text = "Obtenir des informations basiques (tous les paquets)"
                PkgDetailedInfo.Text = "Obtenir des informations détaillées (paquet spécifique)"
                CommitAndUnmountTSMI.Text = "Valider les modifications et démonter l'image"
                DiscardAndUnmountTSMI.Text = "Annuler les modifications et démonter l'image"
                UnmountSettingsToolStripMenuItem.Text = "Configurer les paramètres de démontage......"
                ViewPackageDirectoryToolStripMenuItem.Text = "Afficher le répertoire des paquets"
                ' OpenFileDialogs and FolderBrowsers
                OpenFileDialog1.Title = "Spécifier le fichier de projet à charger"
                LocalMountDirFBD.Description = "Veuillez spécifier le répertoire de montage que vous souhaitez charger dans ce projet:"
                If Not ImgBW.IsBusy And areBackgroundProcessesDone Then
                    BGProcDetails.Label2.Text = "Les processus de l'image sont terminés"
                End If
                MenuDesc.Text = "Prêt"
                ' Tree view context menu
                AccessDirectoryToolStripMenuItem.Text = "Accéder à ce répertoire"
                UnloadProjectToolStripMenuItem1.Text = "Décharger le projet"
                CopyDeploymentToolsToolStripMenuItem.Text = "Copier les outils de déploiement"
                OfAllArchitecturesToolStripMenuItem.Text = "De toutes les architectures"
                OfSelectedArchitectureToolStripMenuItem.Text = "De l'architecture sélectionnée"
                ForX86ArchitectureToolStripMenuItem.Text = "Pour l'architecture x86"
                ForAmd64ArchitectureToolStripMenuItem.Text = "Pour l'architecture AMD64"
                ForARMArchitectureToolStripMenuItem.Text = "Pour l'architecture ARM"
                ForARM64ArchitectureToolStripMenuItem.Text = "Pour l'architecture ARM64"
                ImageOperationsToolStripMenuItem.Text = "Opérations sur les images"
                MountImageToolStripMenuItem.Text = "Monter l'image..."
                UnmountImageToolStripMenuItem.Text = "Démonter l'image..."
                RemoveVolumeImagesToolStripMenuItem.Text = "Supprimer les images de volume..."
                SwitchImageIndexesToolStripMenuItem1.Text = "Changer d'index de l'image..."
                UnattendedAnswerFilesToolStripMenuItem1.Text = "Fichiers de réponse non surveillés"
                ManageToolStripMenuItem.Text = "Gérer"
                CreationWizardToolStripMenuItem.Text = "Créer"
                ScratchDirectorySettingsToolStripMenuItem.Text = "Configurer le répertoire temporaire"
                ManageReportsToolStripMenuItem.Text = "Gérer les rapports"
                AddToolStripMenuItem.Text = "Ajouter"
                NewFileToolStripMenuItem.Text = "Nouveau fichier..."
                ExistingFileToolStripMenuItem.Text = "Fichier existant..."
                ' Context menu of AppX information dialog
                SaveResourceToolStripMenuItem.Text = "Sauvegarder les ressources..."
                CopyToolStripMenuItem.Text = "Copier la ressource"
                ' Context menu of AppX addition dialog
                MicrosoftAppsToolStripMenuItem.Text = "Visiter le site web de Microsoft Apps"
                MicrosoftStoreGenerationProjectToolStripMenuItem.Text = "Visiter le site web du projet Microsoft Store Generation"
                ' New design
                GreetingLabel.Text = "Bienvenue à cette session de service"
                LinkLabel12.Text = "PROJET"
                LinkLabel13.Text = "IMAGE"
                Label54.Text = "Nom :"
                Label51.Text = "Lieu :"
                Label53.Text = "Images montées ?"
                LinkLabel14.Text = "Cliquez ici pour monter une image"
                Label55.Text = "Tâches du projet"
                LinkLabel15.Text = "Voir les propriétés du projet"
                LinkLabel16.Text = "Ouvrir dans l'explorateur de fichiers"
                LinkLabel17.Text = "Décharger le projet"
                Label59.Text = "Aucune image n'a été montée"
                Label58.Text = "Vous devez monter une image pour pouvoir consulter ses informations."
                Label57.Text = "Choix"
                LinkLabel21.Text = "Monter une image..."
                LinkLabel18.Text = "Choisir une image montée..."
                Label39.Text = "Index de l'image :"
                Label43.Text = "Répertoire de montage :"
                Label45.Text = "Version :"
                Label42.Text = "Nom :"
                Label40.Text = "Description :"
                Label56.Text = "Tâches de l'image"
                LinkLabel20.Text = "Voir les propriétés de l'image"
                LinkLabel19.Text = "Démonter l'image"
                GroupBox4.Text = "Opérations sur les images"
                Button26.Text = "Monter une image..."
                Button27.Text = "Sauvegarder les modifications pendants"
                Button28.Text = "Sauvegarder modifications et démonter l'image"
                Button29.Text = "Démonter l'image en supprimant les modifications"
                Button25.Text = "Recharger la session de service"
                Button24.Text = "Changer d'index de l'image..."
                Button30.Text = "Appliquer l'image..."
                Button31.Text = "Capturer image..."
                Button32.Text = "Supprimer les images de volume..."
                Button33.Text = "Sauvegarder les informations complètes de l'image..."
                GroupBox5.Text = "Opérations sur les paquets"
                Button36.Text = "Ajouter des paquets..."
                Button34.Text = "Obtenir des informations sur le paquet..."
                Button38.Text = "Sauvegarder les informations sur les paquets installés..."
                Button35.Text = "Supprimer des paquets..."
                Button37.Text = "Effectuer la maintenance et le nettoyage du stock de composants..."
                GroupBox6.Text = "Opérations sur les caractéristiques"
                Button41.Text = "Activer des caractéristiques..."
                Button39.Text = "Obtenir des informations sur les caractéristiques..."
                Button42.Text = "Sauvegarder les caractéristiques..."
                Button40.Text = "Désactiver des caractéristiques..."
                GroupBox7.Text = "Opérations sur les paquets AppX"
                Button44.Text = "Ajouter des paquets AppX..."
                Button45.Text = "Obtenir des informations sur les applications..."
                Button46.Text = "Sauvegarder les informations sur les paquets AppX installés..."
                Button43.Text = "Supprimer des paquets AppX..."
                GroupBox8.Text = "Opérations sur les capacités"
                Button48.Text = "Ajouter des capacités..."
                Button49.Text = "Obtenir des informations sur les capacités..."
                Button50.Text = "Sauvegarder les informations sur les capacités..."
                Button47.Text = "Supprimer des capacités..."
                GroupBox9.Text = "Opérations sur les pilotes"
                Button53.Text = "Ajouter des paquets de pilotes..."
                Button52.Text = "Obtenir des informations sur les pilotes..."
                Button54.Text = "Sauvegarder les informations sur les pilotes installés..."
                Button51.Text = "Supprimer des pilotes..."
                GroupBox10.Text = "Opérations de Windows PE"
                Button55.Text = "Obtenir des paramètres..."
                Button56.Text = "Sauvegarder les paramètres..."
                Button57.Text = "Configurer le chemin d'accès..."
                Button58.Text = "Configurer l'espace temporaire..."
        End Select

        If OnlineManagement Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label5.Text = If(IsImageMounted, "Yes", "No")
                            Text = "Online installation - DISMTools"
                            Label14.Text = "(Online installation)"
                            Label20.Text = "(Online installation)"
                            projName.Text = "(Online installation)"
                            Label50.Text = If(IsImageMounted, "Yes", "No")
                            Label41.Text = "(Online installation)"
                            Label47.Text = "(Online installation)"
                            Label49.Text = "(Online installation)"
                        Case "ESN"
                            Label5.Text = If(IsImageMounted, "Sí", "No")
                            Text = "Instalación activa - DISMTools"
                            Label14.Text = "(Instalación activa)"
                            Label20.Text = "(Instalación activa)"
                            projName.Text = "(Instalación activa)"
                            Label50.Text = If(IsImageMounted, "Sí", "No")
                            Label41.Text = "(Instalación activa)"
                            Label47.Text = "(Instalación activa)"
                            Label49.Text = "(Instalación activa)"
                        Case "FRA"
                            Label5.Text = If(IsImageMounted, "Oui", "Non")
                            Text = "Installation en ligne - DISMTools"
                            Label14.Text = "(Installation en ligne)"
                            Label20.Text = "(Installation en ligne)"
                            projName.Text = "(Installation en ligne)"
                            Label50.Text = If(IsImageMounted, "Oui", "Non")
                            Label41.Text = "(Installation en ligne)"
                            Label47.Text = "(Installation en ligne)"
                            Label49.Text = "(Installation en ligne)"
                    End Select
                Case 1
                    Label5.Text = If(IsImageMounted, "Yes", "No")
                    Text = "Online installation - DISMTools"
                    Label14.Text = "(Online installation)"
                    Label20.Text = "(Online installation)"
                    projName.Text = "(Online installation)"
                    Label50.Text = If(IsImageMounted, "Yes", "No")
                    Label41.Text = "(Online installation)"
                    Label47.Text = "(Online installation)"
                    Label49.Text = "(Online installation)"
                Case 2
                    Label5.Text = If(IsImageMounted, "Sí", "No")
                    Text = "Instalación activa - DISMTools"
                    Label14.Text = "(Instalación activa)"
                    Label20.Text = "(Instalación activa)"
                    projName.Text = "(Instalación activa)"
                    Label50.Text = If(IsImageMounted, "Sí", "No")
                    Label41.Text = "(Instalación activa)"
                    Label47.Text = "(Instalación activa)"
                    Label49.Text = "(Instalación activa)"
                Case 3
                    Label5.Text = If(IsImageMounted, "Oui", "Non")
                    Text = "Installation en ligne - DISMTools"
                    Label14.Text = "(Installation en ligne)"
                    Label20.Text = "(Installation en ligne)"
                    projName.Text = "(Installation en ligne)"
                    Label50.Text = If(IsImageMounted, "Oui", "Non")
                    Label41.Text = "(Installation en ligne)"
                    Label47.Text = "(Installation en ligne)"
                    Label49.Text = "(Installation en ligne)"
            End Select
            Label49.Text = projName.Text
        ElseIf OfflineManagement Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label5.Text = If(IsImageMounted, "Yes", "No")
                            Text = "Offline installation - DISMTools"
                            Label14.Text = "(Offline installation)"
                            Label18.Text = "(Offline installation)"
                            Label20.Text = "(Offline installation)"
                            projName.Text = "(Offline installation)"
                            Label50.Text = If(IsImageMounted, "Yes", "No")
                            Label41.Text = "(Offline installation)"
                            Label46.Text = "(Offline installation)"
                            Label47.Text = "(Offline installation)"
                            Label49.Text = "(Offline installation)"
                        Case "ESN"
                            Label5.Text = If(IsImageMounted, "Sí", "No")
                            Text = "Instalación fuera de línea - DISMTools"
                            Label14.Text = "(Instalación fuera de línea)"
                            Label18.Text = "(Instalación fuera de línea)"
                            Label20.Text = "(Instalación fuera de línea)"
                            projName.Text = "(Instalación fuera de línea)"
                            Label50.Text = If(IsImageMounted, "Sí", "No")
                            Label41.Text = "(Instalación fuera de línea)"
                            Label46.Text = "(Instalación fuera de línea)"
                            Label47.Text = "(Instalación fuera de línea)"
                            Label49.Text = "(Instalación fuera de línea)"
                        Case "FRA"
                            Label5.Text = If(IsImageMounted, "Oui", "Non")
                            Text = "Installation hors ligne - DISMTools"
                            Label14.Text = "(Installation hors ligne)"
                            Label18.Text = "(Installation hors ligne)"
                            Label20.Text = "(Installation hors ligne)"
                            projName.Text = "(Installation hors ligne)"
                            Label50.Text = If(IsImageMounted, "Oui", "Non")
                            Label41.Text = "(Installation hors ligne)"
                            Label46.Text = "(Installation hors ligne)"
                            Label47.Text = "(Installation hors ligne)"
                            Label49.Text = "(Installation hors ligne)"
                    End Select
                Case 1
                    Label5.Text = If(IsImageMounted, "Yes", "No")
                    Text = "Online installation - DISMTools"
                    Label14.Text = "(Offline installation)"
                    Label18.Text = "(Offline installation)"
                    Label20.Text = "(Offline installation)"
                    projName.Text = "(Offline installation)"
                    Label50.Text = If(IsImageMounted, "Yes", "No")
                    Label41.Text = "(Offline installation)"
                    Label46.Text = "(Offline installation)"
                    Label47.Text = "(Offline installation)"
                    Label49.Text = "(Offline installation)"
                Case 2
                    Label5.Text = If(IsImageMounted, "Sí", "No")
                    Text = "Instalación fuera de línea - DISMTools"
                    Label14.Text = "(Instalación fuera de línea)"
                    Label18.Text = "(Instalación fuera de línea)"
                    Label20.Text = "(Instalación fuera de línea)"
                    projName.Text = "(Instalación fuera de línea)"
                    Label50.Text = If(IsImageMounted, "Sí", "No")
                    Label41.Text = "(Instalación fuera de línea)"
                    Label46.Text = "(Instalación fuera de línea)"
                    Label47.Text = "(Instalación fuera de línea)"
                    Label49.Text = "(Instalación fuera de línea)"
                Case 3
                    Label5.Text = If(IsImageMounted, "Oui", "Non")
                    Text = "Installation hors ligne - DISMTools"
                    Label14.Text = "(Installation hors ligne)"
                    Label18.Text = "(Installation hors ligne)"
                    Label20.Text = "(Installation hors ligne)"
                    projName.Text = "(Installation hors ligne)"
                    Label50.Text = If(IsImageMounted, "Oui", "Non")
                    Label41.Text = "(Installation hors ligne)"
                    Label46.Text = "(Installation hors ligne)"
                    Label47.Text = "(Installation hors ligne)"
                    Label49.Text = "(Installation hors ligne)"
            End Select
            Label49.Text = projName.Text
        End If
    End Sub

    'Sub GenReportTab(ReportType As Integer, TableFormat As Integer)            ' Hold this for a future release
    '    ' Create new tab and switch to it
    '    TabControl2.TabPages.Add("Operation report")
    '    TabControl2.SelectedIndex = TabControl2.TabPages.Count - 1

    '    ' Add stuff to the selected tab
    '    Dim HeaderPanel As New Panel()
    '    With HeaderPanel
    '        .BorderStyle = BorderStyle.Fixed3D
    '        .Parent = TabControl2.SelectedTab
    '        .Dock = DockStyle.Top
    '        Dim HeaderPic As New PictureBox()
    '        With HeaderPic
    '            .Location = New Point(12, 12)
    '            .SizeMode = PictureBoxSizeMode.AutoSize
    '            .Image = New Bitmap(My.Resources.report_tab_header_pic)
    '            .Parent = HeaderPanel
    '        End With
    '        Dim HeaderLabel As New Label()
    '        With HeaderLabel
    '            .Location = New Point((HeaderPanel.Left + HeaderPic.Left + HeaderPic.Width + 4), 12)
    '            .Font = New Font("Tahoma", 14.25)
    '            .Text = "Operation report"
    '            .Parent = HeaderPanel
    '        End With
    '        Dim DescriptionLabel As New Label()
    '        With DescriptionLabel
    '            .Location = New Point((HeaderPanel.Left + HeaderPic.Left + HeaderPic.Width + 4), (HeaderPanel.Top + HeaderLabel.Top + HeaderLabel.Height + 4))
    '            .Font = New Font("Tahoma", 8.25)
    '            .Text = "This is an automatically generated report using the data obtained from the action's results."
    '            .Parent = HeaderPanel
    '        End With
    '    End With

    'End Sub

    Sub CheckDTProjHeaders(DTFileName As String)
        Dim ProjHeaderTest As String
        ProjHeaderTest = File.ReadAllText(DTFileName)
        If ProjHeaderTest.StartsWith("<?xml") Then
            ' SQL Server project
            isSqlServerDTProj = True
        ElseIf ProjHeaderTest.StartsWith("# DISMTools project file") Then
            ' DISMTools project
            isSqlServerDTProj = False
        End If
    End Sub

    Sub LoadDTProj(DTProjPath As String, DTProjFileName As String, BypassFileDialog As Boolean, SkipBGProcs As Boolean)
        If File.Exists(DTProjPath) Then
            CheckDTProjHeaders(DTProjPath)
            If isSqlServerDTProj Then
                SqlServerProjectErrorDlg.ShowDialog()
                Exit Sub
            End If
            SaveProjectToolStripMenuItem.Enabled = True
            SaveProjectasToolStripMenuItem.Enabled = True
            If ProgressPanel.OperationNum = 0 Then
                prjName = NewProj.TextBox1.Text
                Text = prjName & " - DISMTools"
                If Debugger.IsAttached Then
                    Text &= " (debug mode)"
                End If
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                PleaseWaitDialog.Label2.Text = "Loading project: " & Quote & prjName & Quote
                            Case "ESN"
                                PleaseWaitDialog.Label2.Text = "Cargando proyecto: " & Quote & prjName & Quote
                            Case "FRA"
                                PleaseWaitDialog.Label2.Text = "Chargement du projet en cours : " & Quote & prjName & Quote
                        End Select
                    Case 1
                        PleaseWaitDialog.Label2.Text = "Loading project: " & Quote & prjName & Quote
                    Case 2
                        PleaseWaitDialog.Label2.Text = "Cargando proyecto: " & Quote & prjName & Quote
                    Case 3
                        PleaseWaitDialog.Label2.Text = "Chargement du projet en cours : " & Quote & prjName & Quote
                End Select
                PleaseWaitDialog.ShowDialog(Me)
                projName.Text = prjName
                Label49.Text = projName.Text
                Label3.Text = DTProjPath
                Label52.Text = Label3.Text
                projPath = DTProjPath
                projPath = projPath.Replace("\" & DTProjFileName & ".dtproj", "").Trim()
                If IsImageMounted Then
                    ImageNotMountedPanel.Visible = False
                    ImagePanel.Visible = True
                    ImageView_NoImage.Visible = False
                    ImageView_BasicInfo.Visible = True
                End If
                PopulateProjectTree(prjName)
                isProjectLoaded = True
                IsImageMounted = False
                UpdateProjProperties(False, False, SkipBGProcs)
                Button1.Enabled = True
                Button2.Enabled = False
                Button3.Enabled = False
                Button4.Enabled = False
                Button5.Enabled = False
                Button6.Enabled = False
                Button7.Enabled = False
                Button8.Enabled = False
                Button9.Enabled = False
                Button10.Enabled = False
                Button11.Enabled = False
                Button12.Enabled = False
                Button13.Enabled = False
                ' Update the buttons in the new design accordingly
                Button26.Enabled = True
                Button27.Enabled = False
                Button28.Enabled = False
                Button29.Enabled = False
                Button24.Enabled = False
                Button25.Enabled = False
                Button30.Enabled = False
                Button31.Enabled = False
                Button32.Enabled = False
                Button33.Enabled = False
                Button34.Enabled = False
                Button35.Enabled = False
                Button36.Enabled = False
                Button37.Enabled = False
                Button38.Enabled = False
                Button39.Enabled = False
                Button40.Enabled = False
                Button41.Enabled = False
                Button42.Enabled = False
                Button43.Enabled = False
                Button44.Enabled = False
                Button45.Enabled = False
                Button46.Enabled = False
                Button47.Enabled = False
                Button48.Enabled = False
                Button49.Enabled = False
                Button50.Enabled = False
                Button51.Enabled = False
                Button52.Enabled = False
                Button53.Enabled = False
                Button54.Enabled = False
                Button55.Enabled = False
                Button56.Enabled = False
                Button57.Enabled = False
                Button58.Enabled = False
            Else
                If OpenFileDialog1.FileName = "" Then
                    If BypassFileDialog = False Then
                        Exit Sub
                    Else
                        prjName = Path.GetFileNameWithoutExtension(DTProjPath)
                        Text = prjName & " - DISMTools"
                        If Debugger.IsAttached Then
                            Text &= " (debug mode)"
                        End If
                        Label3.Text = DTProjPath
                        Label52.Text = Label3.Text
                        projPath = DTProjPath
                        projPath = projPath.Replace("\" & DTProjFileName & ".dtproj", "").Trim()
                        Select Case Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        PleaseWaitDialog.Label2.Text = "Loading project: " & Quote & prjName & Quote
                                    Case "ESN"
                                        PleaseWaitDialog.Label2.Text = "Cargando proyecto: " & Quote & prjName & Quote
                                    Case "FRA"
                                        PleaseWaitDialog.Label2.Text = "Chargement du projet en cours : " & Quote & prjName & Quote
                                End Select
                            Case 1
                                PleaseWaitDialog.Label2.Text = "Loading project: " & Quote & prjName & Quote
                            Case 2
                                PleaseWaitDialog.Label2.Text = "Cargando proyecto: " & Quote & prjName & Quote
                            Case 3
                                PleaseWaitDialog.Label2.Text = "Chargement du projet en cours : " & Quote & prjName & Quote
                        End Select
                        'PleaseWaitDialog.Label2.Text = "Loading project: " & Quote & prjName & Quote
                        PleaseWaitDialog.ShowDialog(Me)
                        projName.Text = prjName
                        Label49.Text = projName.Text
                        If IsImageMounted Then
                            ImageNotMountedPanel.Visible = False
                            ImagePanel.Visible = True
                            ImageView_NoImage.Visible = False
                            ImageView_BasicInfo.Visible = True
                        Else
                            ImageNotMountedPanel.Visible = True
                            ImagePanel.Visible = False
                            ImageView_NoImage.Visible = True
                            ImageView_BasicInfo.Visible = False
                        End If
                        PopulateProjectTree(prjName)
                        isProjectLoaded = True

                        ' Load values (use same code as saving, but reversed)
                        SourceImg = ProjectValueLoadForm.RichTextBox5.Text
                        Try
                            ImgIndex = CInt(ProjectValueLoadForm.RichTextBox6.Text)
                        Catch ex As Exception
                            ' The conversion could not be possible. Maybe because it's "N/A" on the RTB?
                        End Try
                        MountDir = ProjectValueLoadForm.RichTextBox7.Text
                        imgVersion = ProjectValueLoadForm.RichTextBox8.Text
                        imgMountedName = ProjectValueLoadForm.RichTextBox9.Text
                        imgMountedDesc = ProjectValueLoadForm.RichTextBox10.Text
                        imgWimBootStatus = ProjectValueLoadForm.RichTextBox11.Text
                        imgArch = ProjectValueLoadForm.RichTextBox12.Text
                        imgHal = ProjectValueLoadForm.RichTextBox13.Text
                        imgSPBuild = ProjectValueLoadForm.RichTextBox14.Text
                        imgSPLvl = ProjectValueLoadForm.RichTextBox15.Text
                        imgEdition = ProjectValueLoadForm.RichTextBox16.Text
                        imgPType = ProjectValueLoadForm.RichTextBox17.Text
                        imgPSuite = ProjectValueLoadForm.RichTextBox18.Text
                        imgSysRoot = ProjectValueLoadForm.RichTextBox19.Text
                        Try
                            imgDirs = ProjectValueLoadForm.RichTextBox20.Text
                        Catch ex As Exception
                            ' Like before, the conversion could not be possible
                        End Try
                        Try
                            imgFiles = ProjectValueLoadForm.RichTextBox21.Text
                        Catch ex As Exception
                            ' Like before, the conversion could not be possible
                        End Try
                        Try
                            CreationTime = DateTimeOffset.FromUnixTimeSeconds(CType(ProjectValueLoadForm.RichTextBox22.Text, Long)).ToString().Replace(" +00:00", "").Trim()
                            ModifyTime = DateTimeOffset.FromUnixTimeSeconds(CType(ProjectValueLoadForm.RichTextBox23.Text, Long)).ToString().Replace(" +00:00", "").Trim()
                        Catch ex As Exception
                            ' Like before, the conversion could not be possible
                        End Try
                        imgLangs = ProjectValueLoadForm.RichTextBox24.Text
                        imgRW = ""

                        ' Set initial settings for background processes
                        bwAllBackgroundProcesses = True
                        bwGetImageInfo = True
                        bwGetAdvImgInfo = True
                        bwBackgroundProcessAction = 0

                        ' Detect individual stuff
                        If Directory.Exists(projPath & "\mount" & "\Windows") Then
                            ' Detect whether image is mounted by checking its Windows directory.
                            ' This will be changed in the future but, because this is in alpha, scan
                            ' whether the image's Windows folder exists
                            IsImageMounted = True
                            If imgRW = "Yes" Then
                                UpdateProjProperties(True, False, SkipBGProcs)
                            ElseIf imgRW = "No" Then
                                UpdateProjProperties(True, True, SkipBGProcs)
                            Else
                                ' Assume it has read-write permissions
                                UpdateProjProperties(True, False, SkipBGProcs)
                            End If
                        ElseIf Directory.Exists(MountDir & "\Windows") Then
                            ' This is for these cases where image was mounted to outside the project
                            IsImageMounted = True
                            If imgRW = "Yes" Then
                                UpdateProjProperties(True, False, SkipBGProcs)
                            ElseIf imgRW = "No" Then
                                UpdateProjProperties(True, True, SkipBGProcs)
                            Else
                                ' Assume it has read-write permissions
                                UpdateProjProperties(True, False, SkipBGProcs)
                            End If
                        Else
                            IsImageMounted = False
                            UpdateProjProperties(False, False, SkipBGProcs)
                        End If
                        If IsImageMounted Then
                            Button1.Enabled = False
                            Button2.Enabled = True
                            Button3.Enabled = True
                            Button4.Enabled = True
                            Button5.Enabled = True
                            Button6.Enabled = True
                            Button7.Enabled = True
                            Button8.Enabled = True
                            Button9.Enabled = True
                            Button10.Enabled = True
                            Button11.Enabled = True
                            Button12.Enabled = True
                            Button13.Enabled = True
                            ' Update the buttons in the new design accordingly
                            Button26.Enabled = False
                            Button27.Enabled = True
                            Button28.Enabled = True
                            Button29.Enabled = True
                            Button24.Enabled = True
                            Button25.Enabled = True
                            Button30.Enabled = True
                            Button31.Enabled = True
                            Button32.Enabled = True
                            Button33.Enabled = True
                            Button34.Enabled = True
                            Button35.Enabled = True
                            Button36.Enabled = True
                            Button37.Enabled = True
                            Button38.Enabled = True
                            Button39.Enabled = True
                            Button40.Enabled = True
                            Button41.Enabled = True
                            Button42.Enabled = True
                            Button43.Enabled = True
                            Button44.Enabled = True
                            Button45.Enabled = True
                            Button46.Enabled = True
                            Button47.Enabled = True
                            Button48.Enabled = True
                            Button49.Enabled = True
                            Button50.Enabled = True
                            Button51.Enabled = True
                            Button52.Enabled = True
                            Button53.Enabled = True
                            Button54.Enabled = True
                            Button55.Enabled = True
                            Button56.Enabled = True
                            Button57.Enabled = True
                            Button58.Enabled = True
                        Else
                            Button1.Enabled = True
                            Button2.Enabled = False
                            Button3.Enabled = False
                            Button4.Enabled = False
                            Button5.Enabled = False
                            Button6.Enabled = False
                            Button7.Enabled = False
                            Button8.Enabled = False
                            Button9.Enabled = False
                            Button10.Enabled = False
                            Button11.Enabled = False
                            Button12.Enabled = False
                            Button13.Enabled = False
                            ' Update the buttons in the new design accordingly
                            Button26.Enabled = True
                            Button27.Enabled = False
                            Button28.Enabled = False
                            Button29.Enabled = False
                            Button24.Enabled = False
                            Button25.Enabled = False
                            Button30.Enabled = False
                            Button31.Enabled = False
                            Button32.Enabled = False
                            Button33.Enabled = False
                            Button34.Enabled = False
                            Button35.Enabled = False
                            Button36.Enabled = False
                            Button37.Enabled = False
                            Button38.Enabled = False
                            Button39.Enabled = False
                            Button40.Enabled = False
                            Button41.Enabled = False
                            Button42.Enabled = False
                            Button43.Enabled = False
                            Button44.Enabled = False
                            Button45.Enabled = False
                            Button46.Enabled = False
                            Button47.Enabled = False
                            Button48.Enabled = False
                            Button49.Enabled = False
                            Button50.Enabled = False
                            Button51.Enabled = False
                            Button52.Enabled = False
                            Button53.Enabled = False
                            Button54.Enabled = False
                            Button55.Enabled = False
                            Button56.Enabled = False
                            Button57.Enabled = False
                            Button58.Enabled = False
                        End If
                    End If
                Else
                    prjName = Path.GetFileNameWithoutExtension(DTProjPath)
                    Text = prjName & " - DISMTools"
                    If Debugger.IsAttached Then
                        Text &= " (debug mode)"
                    End If
                    Label3.Text = DTProjPath
                    Label52.Text = Label3.Text
                    projPath = DTProjPath
                    projPath = projPath.Replace("\" & DTProjFileName & ".dtproj", "").Trim()
                    Select Case Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    PleaseWaitDialog.Label2.Text = "Loading project: " & Quote & prjName & Quote
                                Case "ESN"
                                    PleaseWaitDialog.Label2.Text = "Cargando proyecto: " & Quote & prjName & Quote
                                Case "FRA"
                                    PleaseWaitDialog.Label2.Text = "Chargement du projet en cours : " & Quote & prjName & Quote
                            End Select
                        Case 1
                            PleaseWaitDialog.Label2.Text = "Loading project: " & Quote & prjName & Quote
                        Case 2
                            PleaseWaitDialog.Label2.Text = "Cargando proyecto: " & Quote & prjName & Quote
                        Case 3
                            PleaseWaitDialog.Label2.Text = "Chargement du projet en cours : " & Quote & prjName & Quote
                    End Select
                    'PleaseWaitDialog.Label2.Text = "Loading project: " & Quote & prjName & Quote
                    PleaseWaitDialog.ShowDialog(Me)
                    projName.Text = prjName
                    Label49.Text = projName.Text
                    If IsImageMounted Then
                        ImageNotMountedPanel.Visible = False
                        ImagePanel.Visible = True
                        ImageView_NoImage.Visible = False
                        ImageView_BasicInfo.Visible = True
                    Else
                        ImageNotMountedPanel.Visible = True
                        ImagePanel.Visible = False
                        ImageView_NoImage.Visible = True
                        ImageView_BasicInfo.Visible = False
                    End If
                    PopulateProjectTree(prjName)
                    isProjectLoaded = True

                    ' Load values (use same code as saving, but reversed)
                    SourceImg = ProjectValueLoadForm.RichTextBox5.Text
                    Try
                        ImgIndex = CInt(ProjectValueLoadForm.RichTextBox6.Text)
                    Catch ex As Exception
                        ' The conversion could not be possible. Maybe because it's "N/A" on the RTB?
                    End Try
                    MountDir = ProjectValueLoadForm.RichTextBox7.Text
                    imgVersion = ProjectValueLoadForm.RichTextBox8.Text
                    imgMountedName = ProjectValueLoadForm.RichTextBox9.Text
                    imgMountedDesc = ProjectValueLoadForm.RichTextBox10.Text
                    imgWimBootStatus = ProjectValueLoadForm.RichTextBox11.Text
                    imgArch = ProjectValueLoadForm.RichTextBox12.Text
                    imgHal = ProjectValueLoadForm.RichTextBox13.Text
                    imgSPBuild = ProjectValueLoadForm.RichTextBox14.Text
                    imgSPLvl = ProjectValueLoadForm.RichTextBox15.Text
                    imgEdition = ProjectValueLoadForm.RichTextBox16.Text
                    imgPType = ProjectValueLoadForm.RichTextBox17.Text
                    imgPSuite = ProjectValueLoadForm.RichTextBox18.Text
                    imgSysRoot = ProjectValueLoadForm.RichTextBox19.Text
                    Try
                        imgDirs = ProjectValueLoadForm.RichTextBox20.Text
                    Catch ex As Exception
                        ' Like before, the conversion could not be possible
                    End Try
                    Try
                        imgFiles = ProjectValueLoadForm.RichTextBox21.Text
                    Catch ex As Exception
                        ' Like before, the conversion could not be possible
                    End Try
                    Try
                        CreationTime = DateTimeOffset.FromUnixTimeSeconds(CType(ProjectValueLoadForm.RichTextBox22.Text, Long)).ToString().Replace(" +00:00", "").Trim()
                        ModifyTime = DateTimeOffset.FromUnixTimeSeconds(CType(ProjectValueLoadForm.RichTextBox23.Text, Long)).ToString().Replace(" +00:00", "").Trim()
                    Catch ex As Exception
                        ' Like before, the conversion could not be possible
                    End Try
                    imgLangs = ProjectValueLoadForm.RichTextBox24.Text
                    imgRW = ""

                    ' Set initial settings for background processes
                    bwAllBackgroundProcesses = True
                    bwGetImageInfo = True
                    bwGetAdvImgInfo = True
                    bwBackgroundProcessAction = 0

                    ' Detect individual stuff
                    If Directory.Exists(projPath & "\mount" & "\Windows") Then
                        ' Detect whether image is mounted by checking its Windows directory.
                        ' This will be changed in the future but, because this is in alpha, scan
                        ' whether the image's Windows folder exists
                        IsImageMounted = True
                        If imgRW = "Yes" Then
                            UpdateProjProperties(True, False, SkipBGProcs)
                        ElseIf imgRW = "No" Then
                            UpdateProjProperties(True, True, SkipBGProcs)
                        Else
                            ' Assume it has read-write permissions
                            UpdateProjProperties(True, False, SkipBGProcs)
                        End If
                    ElseIf Directory.Exists(MountDir & "\Windows") Then
                        ' This is for these cases where image was mounted to outside the project
                        IsImageMounted = True
                        If imgRW = "Yes" Then
                            UpdateProjProperties(True, False)
                        ElseIf imgRW = "No" Then
                            UpdateProjProperties(True, True)
                        Else
                            ' Assume it has read-write permissions
                            UpdateProjProperties(True, False)
                        End If
                    Else
                        IsImageMounted = False
                        UpdateProjProperties(False, False)
                    End If
                    If IsImageMounted Then
                        Button1.Enabled = False
                        Button2.Enabled = True
                        Button3.Enabled = True
                        Button4.Enabled = True
                        Button5.Enabled = True
                        Button6.Enabled = True
                        Button7.Enabled = True
                        Button8.Enabled = True
                        Button9.Enabled = True
                        Button10.Enabled = True
                        Button11.Enabled = True
                        Button12.Enabled = True
                        Button13.Enabled = True
                        ' Update the buttons in the new design accordingly
                        Button26.Enabled = False
                        Button27.Enabled = True
                        Button28.Enabled = True
                        Button29.Enabled = True
                        Button24.Enabled = True
                        Button25.Enabled = True
                        Button30.Enabled = True
                        Button31.Enabled = True
                        Button32.Enabled = True
                        Button33.Enabled = True
                        Button34.Enabled = True
                        Button35.Enabled = True
                        Button36.Enabled = True
                        Button37.Enabled = True
                        Button38.Enabled = True
                        Button39.Enabled = True
                        Button40.Enabled = True
                        Button41.Enabled = True
                        Button42.Enabled = True
                        Button43.Enabled = True
                        Button44.Enabled = True
                        Button45.Enabled = True
                        Button46.Enabled = True
                        Button47.Enabled = True
                        Button48.Enabled = True
                        Button49.Enabled = True
                        Button50.Enabled = True
                        Button51.Enabled = True
                        Button52.Enabled = True
                        Button53.Enabled = True
                        Button54.Enabled = True
                        Button55.Enabled = True
                        Button56.Enabled = True
                        Button57.Enabled = True
                        Button58.Enabled = True
                    Else
                        Button1.Enabled = True
                        Button2.Enabled = False
                        Button3.Enabled = False
                        Button4.Enabled = False
                        Button5.Enabled = False
                        Button6.Enabled = False
                        Button7.Enabled = False
                        Button8.Enabled = False
                        Button9.Enabled = False
                        Button10.Enabled = False
                        Button11.Enabled = False
                        Button12.Enabled = False
                        Button13.Enabled = False
                        ' Update the buttons in the new design accordingly
                        Button26.Enabled = True
                        Button27.Enabled = False
                        Button28.Enabled = False
                        Button29.Enabled = False
                        Button24.Enabled = False
                        Button25.Enabled = False
                        Button30.Enabled = False
                        Button31.Enabled = False
                        Button32.Enabled = False
                        Button33.Enabled = False
                        Button34.Enabled = False
                        Button35.Enabled = False
                        Button36.Enabled = False
                        Button37.Enabled = False
                        Button38.Enabled = False
                        Button39.Enabled = False
                        Button40.Enabled = False
                        Button41.Enabled = False
                        Button42.Enabled = False
                        Button43.Enabled = False
                        Button44.Enabled = False
                        Button45.Enabled = False
                        Button46.Enabled = False
                        Button47.Enabled = False
                        Button48.Enabled = False
                        Button49.Enabled = False
                        Button50.Enabled = False
                        Button51.Enabled = False
                        Button52.Enabled = False
                        Button53.Enabled = False
                        Button54.Enabled = False
                        Button55.Enabled = False
                        Button56.Enabled = False
                        Button57.Enabled = False
                        Button58.Enabled = False
                    End If
                End If
            End If
            If IsImageMounted Then BGProcNotify.Label2.Visible = True Else BGProcNotify.Label2.Visible = False
            BGProcNotify.Opacity = 100
            If NotificationShow Then
                Select Case NotificationFrequency
                    Case 0
                        BGProcNotify.Show()
                    Case 1
                        If NotificationTimes < 1 Then
                            NotificationTimes += 1
                            BGProcNotify.Show()
                        End If
                End Select
            End If
            BackgroundProcessesButton.Image = New Bitmap(My.Resources.bg_ops_complete)
        Else
            MessageBox.Show("Cannot load the project. Reason: the project was not found. It may have been moved or its folder may have been deleted.", "Project load error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            If DialogResult.OK Then
                Exit Sub
            End If
        End If
    End Sub

    Sub SaveDTProj()
        If ProjectValueLoadForm.RichTextBox1.Text = "" Then ProjectValueLoadForm.RichTextBox1.Text = File.ReadAllText(projPath & "\settings\project.ini", UTF8)
        ' Clear Rich Text Boxes
        'ProjectValueLoadForm.RichTextBox2.Text = ""
        'ProjectValueLoadForm.RichTextBox3.Text = ""
        'ProjectValueLoadForm.RichTextBox4.Text = ""
        ProjectValueLoadForm.RichTextBox5.Text = ""
        ProjectValueLoadForm.RichTextBox6.Text = ""
        ProjectValueLoadForm.RichTextBox7.Text = ""
        ProjectValueLoadForm.RichTextBox8.Text = ""
        ProjectValueLoadForm.RichTextBox9.Text = ""
        ProjectValueLoadForm.RichTextBox10.Text = ""
        ProjectValueLoadForm.RichTextBox11.Text = ""
        ProjectValueLoadForm.RichTextBox12.Text = ""
        ProjectValueLoadForm.RichTextBox13.Text = ""
        ProjectValueLoadForm.RichTextBox14.Text = ""
        ProjectValueLoadForm.RichTextBox15.Text = ""
        ProjectValueLoadForm.RichTextBox16.Text = ""
        ProjectValueLoadForm.RichTextBox17.Text = ""
        ProjectValueLoadForm.RichTextBox18.Text = ""
        ProjectValueLoadForm.RichTextBox19.Text = ""
        ProjectValueLoadForm.RichTextBox20.Text = ""
        ProjectValueLoadForm.RichTextBox21.Text = ""
        ProjectValueLoadForm.RichTextBox22.Text = ""
        ProjectValueLoadForm.RichTextBox23.Text = ""
        ProjectValueLoadForm.RichTextBox24.Text = ""
        ProjectValueLoadForm.RichTextBox25.Text = ""
        ProjectValueLoadForm.RichTextBox26.Text = ""

        ' Add new values
        'ProjectValueLoadForm.RichTextBox2.Text = ""
        'ProjectValueLoadForm.RichTextBox3.Text = ""
        'ProjectValueLoadForm.RichTextBox4.Text = ""
        If IsImageMounted Then
            ProjectValueLoadForm.RichTextBox5.Text = SourceImg
        Else
            ProjectValueLoadForm.RichTextBox5.Text = "N/A"
        End If
        If ImgIndex = 0 Then
            ProjectValueLoadForm.RichTextBox6.Text = "N/A"
        Else
            ProjectValueLoadForm.RichTextBox6.Text = CStr(ImgIndex)
        End If
        ProjectValueLoadForm.RichTextBox7.Text = MountDir
        ProjectValueLoadForm.RichTextBox8.Text = imgVersion
        ProjectValueLoadForm.RichTextBox9.Text = imgMountedName
        ProjectValueLoadForm.RichTextBox10.Text = imgMountedDesc
        ProjectValueLoadForm.RichTextBox11.Text = imgWimBootStatus
        ProjectValueLoadForm.RichTextBox12.Text = imgArch
        ProjectValueLoadForm.RichTextBox13.Text = imgHal
        ProjectValueLoadForm.RichTextBox14.Text = imgSPBuild
        ProjectValueLoadForm.RichTextBox15.Text = imgSPLvl
        ProjectValueLoadForm.RichTextBox16.Text = imgEdition
        ProjectValueLoadForm.RichTextBox17.Text = imgPType
        ProjectValueLoadForm.RichTextBox18.Text = imgPSuite
        ProjectValueLoadForm.RichTextBox19.Text = imgSysRoot
        If imgDirs = 0 Or Not IsImageMounted Then
            ProjectValueLoadForm.RichTextBox20.Text = "N/A"
        Else
            ProjectValueLoadForm.RichTextBox20.Text = CStr(imgDirs)
        End If
        If imgFiles = 0 Or Not IsImageMounted Then
            ProjectValueLoadForm.RichTextBox21.Text = "N/A"
        Else
            ProjectValueLoadForm.RichTextBox21.Text = CStr(imgFiles)
        End If
        Try
            ProjectValueLoadForm.RichTextBox22.Text = DateTimeOffset.Parse(CreationTime).ToUnixTimeSeconds()
            ProjectValueLoadForm.RichTextBox23.Text = DateTimeOffset.Parse(ModifyTime).ToUnixTimeSeconds()
        Catch ex As Exception
            ProjectValueLoadForm.RichTextBox22.Text = "N/A"
            ProjectValueLoadForm.RichTextBox23.Text = "N/A"
        End Try

        ProjectValueLoadForm.RichTextBox24.Text = imgLangs
        ProjectValueLoadForm.RichTextBox25.Text = imgRW
        ProjectValueLoadForm.RichTextBox26.Text = ""
        ProjectValueLoadForm.RichTextBox26.AppendText("[ProjOptions]" & CrLf & ProjectValueLoadForm.RichTextBox1.Lines(1) & CrLf & ProjectValueLoadForm.RichTextBox1.Lines(2) & CrLf & ProjectValueLoadForm.RichTextBox1.Lines(3) & CrLf & CrLf & _
                                                      "[ImageOptions]" & CrLf & _
                                                      "ImageFile=" & ProjectValueLoadForm.RichTextBox5.Text & CrLf & _
                                                      "ImageIndex=" & ProjectValueLoadForm.RichTextBox6.Text & CrLf & _
                                                      "ImageMountPoint=" & ProjectValueLoadForm.RichTextBox7.Text & CrLf & _
                                                      "ImageVersion=" & ProjectValueLoadForm.RichTextBox8.Text & CrLf & _
                                                      "ImageName=" & ProjectValueLoadForm.RichTextBox9.Text & CrLf & _
                                                      "ImageDescription=" & ProjectValueLoadForm.RichTextBox10.Text & CrLf & _
                                                      "ImageWIMBoot=" & ProjectValueLoadForm.RichTextBox11.Text & CrLf & _
                                                      "ImageArch=" & ProjectValueLoadForm.RichTextBox12.Text & CrLf & _
                                                      "ImageHal=" & ProjectValueLoadForm.RichTextBox13.Text & CrLf & _
                                                      "ImageSPBuild=" & ProjectValueLoadForm.RichTextBox14.Text & CrLf & _
                                                      "ImageSPLevel=" & ProjectValueLoadForm.RichTextBox15.Text & CrLf & _
                                                      "ImageEdition=" & ProjectValueLoadForm.RichTextBox16.Text & CrLf & _
                                                      "ImagePType=" & ProjectValueLoadForm.RichTextBox17.Text & CrLf & _
                                                      "ImagePSuite=" & ProjectValueLoadForm.RichTextBox18.Text & CrLf & _
                                                      "ImageSysRoot=" & ProjectValueLoadForm.RichTextBox19.Text & CrLf & _
                                                      "ImageDirCount=" & ProjectValueLoadForm.RichTextBox20.Text & CrLf & _
                                                      "ImageFileCount=" & ProjectValueLoadForm.RichTextBox21.Text & CrLf & _
                                                      "ImageEpochCreate=" & ProjectValueLoadForm.RichTextBox22.Text & CrLf & _
                                                      "ImageEpochModify=" & ProjectValueLoadForm.RichTextBox23.Text & CrLf & _
                                                      "ImageLang=" & ProjectValueLoadForm.RichTextBox24.Text)
        Try
            ProjectValueLoadForm.EpochRTB2.Text = DateTimeOffset.FromUnixTimeSeconds(CType(ProjectValueLoadForm.RichTextBox22.Text, Long)).ToString().Replace(" +00:00", "").Trim()
            ProjectValueLoadForm.EpochRTB3.Text = DateTimeOffset.FromUnixTimeSeconds(CType(ProjectValueLoadForm.RichTextBox23.Text, Long)).ToString().Replace(" +00:00", "").Trim()
        Catch ex As Exception
            ProjectValueLoadForm.EpochRTB2.Text = "Not available"
            ProjectValueLoadForm.EpochRTB3.Text = "Not available"
        End Try
        If Debugger.IsAttached Then
            ProjectValueLoadForm.ShowDialog()
        End If
        Try
            File.WriteAllText(projPath & "\" & "settings\project.ini", ProjectValueLoadForm.RichTextBox26.Text)
        Catch ex As Exception

        End Try
        isModified = False
    End Sub

    ''' <summary>
    ''' Unloads the DISMTools project
    ''' </summary>
    ''' <param name="IsBeingClosed">Determines whether the program is being closed</param>
    ''' <param name="SaveProject">Determines whether the program should save the project</param>
    ''' <param name="UnmountImg">Determines whether the program should unmount the image before unloading the project</param>
    ''' <remarks>The program, attending to the parameters shown above, will unload the project</remarks>
    Sub UnloadDTProj(IsBeingClosed As Boolean, SaveProject As Boolean, UnmountImg As Boolean)
        If ImgBW.IsBusy Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            If MsgBox("Background processes are still gathering information about this image. Do you want to cancel them?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                                ImgBW.CancelAsync()
                            Else
                                Exit Sub
                            End If
                        Case "ESN"
                            If MsgBox("Procesos en segundo plano todavía están recopilando información de esta imagen. ¿Desea cancelarlos?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                                ImgBW.CancelAsync()
                            Else
                                Exit Sub
                            End If
                        Case "FRA"
                            If MsgBox("Les processus en arrière-plan sont encore en train de recueillir des informations sur cette image. Voulez-vous les annuler ?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                                ImgBW.CancelAsync()
                            Else
                                Exit Sub
                            End If
                    End Select
                Case 1
                    If MsgBox("Background processes are still gathering information about this image. Do you want to cancel them?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                        ImgBW.CancelAsync()
                    Else
                        Exit Sub
                    End If
                Case 2
                    If MsgBox("Procesos en segundo plano todavía están recopilando información de esta imagen. ¿Desea cancelarlos?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                        ImgBW.CancelAsync()
                    Else
                        Exit Sub
                    End If
                Case 3
                    If MsgBox("Les processus en arrière-plan sont encore en train de recueillir des informations sur cette image. Voulez-vous les annuler ?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                        ImgBW.CancelAsync()
                    Else
                        Exit Sub
                    End If
            End Select
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MenuDesc.Text = "Cancelling background processes. Please wait..."
                        Case "ESN"
                            MenuDesc.Text = "Espere mientras cancelamos los procesos en segundo plano..."
                        Case "FRA"
                            MenuDesc.Text = "Annulation des processus en arrière plan en cours. Veuillez patienter ..."
                    End Select
                Case 1
                    MenuDesc.Text = "Cancelling background processes. Please wait..."
                Case 2
                    MenuDesc.Text = "Espere mientras cancelamos los procesos en segundo plano..."
                Case 3
                    MenuDesc.Text = "Annulation des processus en arrière plan en cours. Veuillez patienter ..."
            End Select
            While ImgBW.IsBusy()
                ToolStripButton3.Enabled = False
                UnloadBtn.Enabled = False
                Application.DoEvents()
                Thread.Sleep(100)
            End While
            ToolStripButton3.Enabled = True
            UnloadBtn.Enabled = True
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MenuDesc.Text = "Ready"
                        Case "ESN"
                            MenuDesc.Text = "Listo"
                        Case "FRA"
                            MenuDesc.Text = "Prêt"
                    End Select
                Case 1
                    MenuDesc.Text = "Ready"
                Case 2
                    MenuDesc.Text = "Listo"
                Case 3
                    MenuDesc.Text = "Prêt"
            End Select
        End If
        bwBackgroundProcessAction = 0
        bwGetImageInfo = True
        bwGetAdvImgInfo = True
        If imgCommitOperation = 0 Then
            ProgressPanel.OperationNum = 21
            ProgressPanel.UMountLocalDir = True
            ProgressPanel.RandomMountDir = ""   ' Hope there isn't anything to set here
            ProgressPanel.UMountImgIndex = ImgIndex
            ProgressPanel.MountDir = MountDir
            ProgressPanel.UMountOp = 0
            ProgressPanel.ShowDialog(Me)
            Exit Sub
        ElseIf imgCommitOperation = 1 Then
            ProgressPanel.OperationNum = 21
            ProgressPanel.UMountLocalDir = True
            ProgressPanel.RandomMountDir = ""   ' Hope there isn't anything to set here
            ProgressPanel.UMountImgIndex = ImgIndex
            ProgressPanel.MountDir = MountDir
            ProgressPanel.UMountOp = 1
            ProgressPanel.ShowDialog(Me)
            Exit Sub
        End If
        If SaveProject And Not (OnlineManagement Or OfflineManagement) Then
            SaveDTProj()
        End If
        If UnmountImg Then
            ProgressPanel.OperationNum = 21
            ProgressPanel.UMountLocalDir = True
            ProgressPanel.RandomMountDir = ""   ' Hope there isn't anything to set here
            ProgressPanel.UMountImgIndex = ImgIndex
            ProgressPanel.MountDir = MountDir
            If IsBeingClosed Then
                ProgressPanel.ProgramIsBeingClosed = True
            End If
            ProgressPanel.ShowDialog()
        End If
        Text = "DISMTools"
        If Debugger.IsAttached Then
            Text &= " (debug mode)"
        End If
        UnpopulateProjectTree()
        ProjectToolStripMenuItem.Visible = False
        Thread.Sleep(250)
        Refresh()
        CommandsToolStripMenuItem.Visible = False
        Thread.Sleep(250)
        Refresh()
        HomePanel.Visible = True
        PrjPanel.Visible = False
        SplitPanels.Visible = False
        isProjectLoaded = False
        SaveProjectToolStripMenuItem.Enabled = False
        SaveProjectasToolStripMenuItem.Enabled = False
        BGProcDetails.Hide()
        Array.Clear(CompletedTasks, 0, CompletedTasks.Length)
        PendingTasks = Enumerable.Repeat(True, PendingTasks.Length).ToArray()
        If OnlineManagement Then EndOnlineManagement()
        If OfflineManagement Then EndOfflineManagement()
    End Sub

    Sub BeginOnlineManagement(ShowDialog As Boolean)
        If ShowDialog Then ActiveInstAccessWarn.ShowDialog()
        IsImageMounted = True
        isProjectLoaded = True
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Online installation - DISMTools"
                    Case "ESN"
                        Text = "Instalación activa - DISMTools"
                    Case "FRA"
                        Text = "Installation en ligne - DISMTools"
                End Select
            Case 1
                Text = "Online installation - DISMTools"
            Case 2
                Text = "Instalación activa - DISMTools"
            Case 3
                Text = "Installation en ligne - DISMTools"
        End Select
        OnlineManagement = True
        ' Initialize background processes
        bwAllBackgroundProcesses = True
        bwGetImageInfo = True
        bwGetAdvImgInfo = True
        bwBackgroundProcessAction = 0
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label5.Text = "Yes"
                    Case "ESN"
                        Label5.Text = "Sí"
                    Case "FRA"
                        Label5.Text = "Oui"
                End Select
            Case 1
                Label5.Text = "Yes"
            Case 2
                Label5.Text = "Sí"
            Case 3
                Label5.Text = "Oui"
        End Select
        Label50.Text = Label5.Text
        UnpopulateProjectTree()
        HomePanel.Visible = False
        PrjPanel.Visible = True
        SplitPanels.Visible = True
        RemountImageWithWritePermissionsToolStripMenuItem.Enabled = False
        SaveProjectToolStripMenuItem.Enabled = False
        SaveProjectasToolStripMenuItem.Enabled = False
        LinkLabel1.Visible = False
        LinkLabel14.Visible = False
        ImageNotMountedPanel.Visible = False
        ImagePanel.Visible = True
        ImageView_NoImage.Visible = False
        ImageView_BasicInfo.Visible = True
        CommandsToolStripMenuItem.Visible = True
        Thread.Sleep(250)
        Refresh()
        ' Saving a project is not possible in online mode
        ToolStripButton2.Enabled = False
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label14.Text = "(Online installation)"
                        Label12.Text = "(Online installation)"
                    Case "ESN"
                        Label14.Text = "(Instalación activa)"
                        Label12.Text = "(Instalación activa)"
                    Case "FRA"
                        Label14.Text = "(Installation en ligne)"
                        Label12.Text = "(Installation en ligne)"
                End Select
            Case 1
                Label14.Text = "(Online installation)"
                Label12.Text = "(Online installation)"
            Case 2
                Label14.Text = "(Instalación activa)"
                Label12.Text = "(Instalación activa)"
            Case 3
                Label14.Text = "(Installation en ligne)"
                Label12.Text = "(Installation en ligne)"
        End Select
        Label41.Text = Label14.Text
        Label44.Text = Label12.Text
        GroupBox1.Enabled = False
        Panel2.Visible = False
        ProjNameEditBtn.Visible = False
        TableLayoutPanel2.ColumnCount = 2
        TableLayoutPanel2.SetColumnSpan(Label5, 1)
        TableLayoutPanel2.SetColumnSpan(Label3, 1)
        ManageOnlineInstallationToolStripMenuItem.Enabled = False
        MountDir = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows))
        ImgBW.RunWorkerAsync()
        Exit Sub
    End Sub

    Sub BeginOfflineManagement(ImageDrive As String)
        IsImageMounted = True
        isProjectLoaded = True
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Offline installation - DISMTools"
                    Case "ESN"
                        Text = "Instalación fuera de línea - DISMTools"
                    Case "FRA"
                        Text = "Installation hors ligne - DISMTools"
                End Select
            Case 1
                Text = "Offline installation - DISMTools"
            Case 2
                Text = "Instalación fuera de línea - DISMTools"
            Case 3
                Text = "Installation hors ligne - DISMTools"
        End Select
        OfflineManagement = True
        ' Initialize background processes
        bwAllBackgroundProcesses = True
        bwGetImageInfo = True
        bwGetAdvImgInfo = True
        bwBackgroundProcessAction = 0
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label5.Text = "Yes"
                    Case "ESN"
                        Label5.Text = "Sí"
                    Case "FRA"
                        Label5.Text = "Oui"
                End Select
            Case 1
                Label5.Text = "Yes"
            Case 2
                Label5.Text = "Sí"
            Case 3
                Label5.Text = "Oui"
        End Select
        Label50.Text = Label5.Text
        UnpopulateProjectTree()
        HomePanel.Visible = False
        PrjPanel.Visible = True
        SplitPanels.Visible = True
        RemountImageWithWritePermissionsToolStripMenuItem.Enabled = False
        SaveProjectToolStripMenuItem.Enabled = False
        SaveProjectasToolStripMenuItem.Enabled = False
        LinkLabel1.Visible = False
        LinkLabel14.Visible = False
        ImageNotMountedPanel.Visible = False
        ImagePanel.Visible = True
        ImageView_NoImage.Visible = False
        ImageView_BasicInfo.Visible = True
        CommandsToolStripMenuItem.Visible = True
        Thread.Sleep(250)
        Refresh()
        ' Saving a project is not possible in offline mode either
        ToolStripButton2.Enabled = False
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label14.Text = "(Offline installation)"
                        Label12.Text = "(Offline installation)"
                    Case "ESN"
                        Label14.Text = "(Instalación fuera de línea)"
                        Label12.Text = "(Instalación fuera de línea)"
                    Case "FRA"
                        Label14.Text = "(Installation hors ligne)"
                        Label12.Text = "(Installation hors ligne)"
                End Select
            Case 1
                Label14.Text = "(Offline installation)"
                Label12.Text = "(Offline installation)"
            Case 2
                Label14.Text = "(Instalación fuera de línea)"
                Label12.Text = "(Instalación fuera de línea)"
            Case 3
                Label14.Text = "(Installation hors ligne)"
                Label12.Text = "(Installation hors ligne)"
        End Select
        Label41.Text = Label14.Text
        Label44.Text = Label12.Text
        GroupBox1.Enabled = False
        Panel2.Visible = False
        ProjNameEditBtn.Visible = False
        TableLayoutPanel2.ColumnCount = 2
        TableLayoutPanel2.SetColumnSpan(Label5, 1)
        TableLayoutPanel2.SetColumnSpan(Label3, 1)
        ManageOfflineInstallationToolStripMenuItem.Enabled = False
        MountDir = ImageDrive
        ImgBW.RunWorkerAsync()
        Exit Sub
    End Sub

    Sub EndOfflineManagement()
        If ImgBW.IsBusy Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            If MsgBox("Background processes are still gathering information about this image. Do you want to cancel them?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                                ImgBW.CancelAsync()
                            Else
                                Exit Sub
                            End If
                        Case "ESN"
                            If MsgBox("Procesos en segundo plano todavía están recopilando información de esta imagen. ¿Desea cancelarlos?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                                ImgBW.CancelAsync()
                            Else
                                Exit Sub
                            End If
                        Case "FRA"
                            If MsgBox("Les processus en arrière-plan sont encore en train de recueillir des informations sur cette image. Voulez-vous les annuler ?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                                ImgBW.CancelAsync()
                            Else
                                Exit Sub
                            End If
                    End Select
                Case 1
                    If MsgBox("Background processes are still gathering information about this image. Do you want to cancel them?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                        ImgBW.CancelAsync()
                    Else
                        Exit Sub
                    End If
                Case 2
                    If MsgBox("Procesos en segundo plano todavía están recopilando información de esta imagen. ¿Desea cancelarlos?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                        ImgBW.CancelAsync()
                    Else
                        Exit Sub
                    End If
                Case 3
                    If MsgBox("Les processus en arrière-plan sont encore en train de recueillir des informations sur cette image. Voulez-vous les annuler ?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                        ImgBW.CancelAsync()
                    Else
                        Exit Sub
                    End If
            End Select
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MenuDesc.Text = "Cancelling background processes. Please wait..."
                        Case "ESN"
                            MenuDesc.Text = "Espere mientras cancelamos los procesos en segundo plano..."
                        Case "FRA"
                            MenuDesc.Text = "Annulation des processus en arrière plan en cours. Veuillez patienter ..."
                    End Select
                Case 1
                    MenuDesc.Text = "Cancelling background processes. Please wait..."
                Case 2
                    MenuDesc.Text = "Espere mientras cancelamos los procesos en segundo plano..."
                Case 3
                    MenuDesc.Text = "Annulation des processus en arrière plan en cours. Veuillez patienter ..."
            End Select
            While ImgBW.IsBusy()
                ToolStripButton3.Enabled = False
                UnloadBtn.Enabled = False
                Application.DoEvents()
                Thread.Sleep(100)
            End While
            ToolStripButton3.Enabled = True
            UnloadBtn.Enabled = True
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MenuDesc.Text = "Ready"
                        Case "ESN"
                            MenuDesc.Text = "Listo"
                        Case "FRA"
                            MenuDesc.Text = "Prêt"
                    End Select
                Case 1
                    MenuDesc.Text = "Ready"
                Case 2
                    MenuDesc.Text = "Listo"
                Case 3
                    MenuDesc.Text = "Prêt"
            End Select
        End If
        bwBackgroundProcessAction = 0
        bwGetImageInfo = True
        bwGetAdvImgInfo = True
        IsImageMounted = False
        isProjectLoaded = False
        Text = "DISMTools"
        OfflineManagement = False
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label5.Text = "Yes"
                    Case "ESN"
                        Label5.Text = "Sí"
                    Case "FRA"
                        Label5.Text = "Oui"
                End Select
            Case 1
                Label5.Text = "Yes"
            Case 2
                Label5.Text = "Sí"
            Case 3
                Label5.Text = "Oui"
        End Select
        Label50.Text = Label5.Text
        HomePanel.Visible = True
        PrjPanel.Visible = False
        SplitPanels.Visible = False
        RemountImageWithWritePermissionsToolStripMenuItem.Enabled = False
        LinkLabel1.Visible = False
        LinkLabel14.Visible = False
        ImageNotMountedPanel.Visible = False
        ImagePanel.Visible = True
        ImageView_NoImage.Visible = False
        ImageView_BasicInfo.Visible = True
        CommandsToolStripMenuItem.Visible = False
        ProjectToolStripMenuItem.Visible = False
        Thread.Sleep(250)
        Refresh()
        ToolStripButton2.Enabled = True
        GroupBox1.Enabled = True
        ' Enable tasks in the new design accordingly
        Button24.Enabled = True
        Button25.Enabled = True
        Button26.Enabled = True
        Button27.Enabled = True
        Button28.Enabled = True
        Button29.Enabled = True
        Panel2.Visible = True
        ProjNameEditBtn.Visible = True
        TableLayoutPanel2.ColumnCount = 3
        TableLayoutPanel2.SetColumnSpan(Label5, 2)
        TableLayoutPanel2.SetColumnSpan(Label3, 2)
        BGProcDetails.Hide()
        ManageOfflineInstallationToolStripMenuItem.Enabled = True
        Array.Clear(CompletedTasks, 0, CompletedTasks.Length)
        PendingTasks = Enumerable.Repeat(True, PendingTasks.Count).ToArray()
        MountDir = ""
    End Sub

    Sub EndOnlineManagement()
        If ImgBW.IsBusy Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            If MsgBox("Background processes are still gathering information about this image. Do you want to cancel them?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                                ImgBW.CancelAsync()
                            Else
                                Exit Sub
                            End If
                        Case "ESN"
                            If MsgBox("Procesos en segundo plano todavía están recopilando información de esta imagen. ¿Desea cancelarlos?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                                ImgBW.CancelAsync()
                            Else
                                Exit Sub
                            End If
                        Case "FRA"
                            If MsgBox("Les processus en arrière-plan sont encore en train de recueillir des informations sur cette image. Voulez-vous les annuler ?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                                ImgBW.CancelAsync()
                            Else
                                Exit Sub
                            End If
                    End Select
                Case 1
                    If MsgBox("Background processes are still gathering information about this image. Do you want to cancel them?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                        ImgBW.CancelAsync()
                    Else
                        Exit Sub
                    End If
                Case 2
                    If MsgBox("Procesos en segundo plano todavía están recopilando información de esta imagen. ¿Desea cancelarlos?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                        ImgBW.CancelAsync()
                    Else
                        Exit Sub
                    End If
                Case 3
                    If MsgBox("Les processus en arrière-plan sont encore en train de recueillir des informations sur cette image. Voulez-vous les annuler ?", vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                        ImgBW.CancelAsync()
                    Else
                        Exit Sub
                    End If
            End Select
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MenuDesc.Text = "Cancelling background processes. Please wait..."
                        Case "ESN"
                            MenuDesc.Text = "Espere mientras cancelamos los procesos en segundo plano..."
                        Case "FRA"
                            MenuDesc.Text = "Annulation des processus en arrière plan en cours. Veuillez patienter ..."
                    End Select
                Case 1
                    MenuDesc.Text = "Cancelling background processes. Please wait..."
                Case 2
                    MenuDesc.Text = "Espere mientras cancelamos los procesos en segundo plano..."
                Case 3
                    MenuDesc.Text = "Annulation des processus en arrière plan en cours. Veuillez patienter ..."
            End Select
            While ImgBW.IsBusy()
                ToolStripButton3.Enabled = False
                UnloadBtn.Enabled = False
                Application.DoEvents()
                Thread.Sleep(100)
            End While
            ToolStripButton3.Enabled = True
            UnloadBtn.Enabled = True
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MenuDesc.Text = "Ready"
                        Case "ESN"
                            MenuDesc.Text = "Listo"
                        Case "FRA"
                            MenuDesc.Text = "Prêt"
                    End Select
                Case 1
                    MenuDesc.Text = "Ready"
                Case 2
                    MenuDesc.Text = "Listo"
                Case 3
                    MenuDesc.Text = "Prêt"
            End Select
        End If
        bwBackgroundProcessAction = 0
        bwGetImageInfo = True
        bwGetAdvImgInfo = True
        IsImageMounted = False
        isProjectLoaded = False
        Text = "DISMTools"
        OnlineManagement = False
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label5.Text = "Yes"
                    Case "ESN"
                        Label5.Text = "Sí"
                    Case "FRA"
                        Label5.Text = "Oui"
                End Select
            Case 1
                Label5.Text = "Yes"
            Case 2
                Label5.Text = "Sí"
            Case 3
                Label5.Text = "Oui"
        End Select
        Label50.Text = Label5.Text
        HomePanel.Visible = True
        PrjPanel.Visible = False
        SplitPanels.Visible = False
        RemountImageWithWritePermissionsToolStripMenuItem.Enabled = False
        LinkLabel1.Visible = False
        LinkLabel14.Visible = False
        ImageNotMountedPanel.Visible = False
        ImagePanel.Visible = True
        ImageView_NoImage.Visible = False
        ImageView_BasicInfo.Visible = True
        CommandsToolStripMenuItem.Visible = False
        ProjectToolStripMenuItem.Visible = False
        Thread.Sleep(250)
        Refresh()
        ToolStripButton2.Enabled = True
        GroupBox1.Enabled = True
        ' Enable tasks in the new design accordingly
        Button24.Enabled = True
        Button25.Enabled = True
        Button26.Enabled = True
        Button27.Enabled = True
        Button28.Enabled = True
        Button29.Enabled = True
        Panel2.Visible = True
        ProjNameEditBtn.Visible = True
        TableLayoutPanel2.ColumnCount = 3
        TableLayoutPanel2.SetColumnSpan(Label5, 2)
        TableLayoutPanel2.SetColumnSpan(Label3, 2)
        ManageOnlineInstallationToolStripMenuItem.Enabled = True
        BGProcDetails.Hide()
        Array.Clear(CompletedTasks, 0, CompletedTasks.Length)
        PendingTasks = Enumerable.Repeat(True, PendingTasks.Count).ToArray()
        MountDir = ""
    End Sub

    Sub UpdateProjProperties(WasImageMounted As Boolean, IsReadOnly As Boolean, Optional SkipBGProcs As Boolean = False)
        If WasImageMounted Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label5.Text = "Yes"
                        Case "ESN"
                            Label5.Text = "Sí"
                        Case "FRA"
                            Label5.Text = "Oui"
                    End Select
                Case 1
                    Label5.Text = "Yes"
                Case 2
                    Label5.Text = "Sí"
                Case 3
                    Label5.Text = "Oui"
            End Select
            Label50.Text = Label5.Text
            LinkLabel1.Visible = False
            LinkLabel14.Visible = False
            ImageNotMountedPanel.Visible = False
            ImagePanel.Visible = True
            ImageView_NoImage.Visible = False
            ImageView_BasicInfo.Visible = True
            IsImageMounted = True
        Else
            Label5.Text = "No"
            Label50.Text = Label5.Text
            LinkLabel1.Visible = True
            LinkLabel14.Visible = True
            ImageNotMountedPanel.Visible = True
            ImagePanel.Visible = False
            ImageView_NoImage.Visible = True
            ImageView_BasicInfo.Visible = False
            IsImageMounted = False
            SourceImg = "N/A"
            ImgIndex = 0
            MountDir = "N/A"
            imgVersion = "N/A"
            imgMountedName = "N/A"
            imgMountedDesc = "N/A"
            imgWimBootStatus = "N/A"
            imgArch = "N/A"
            imgHal = "N/A"
            imgSPBuild = "N/A"
            imgSPLvl = "N/A"
            imgEdition = "N/A"
            imgPType = "N/A"
            imgPSuite = "N/A"
            imgSysRoot = "N/A"
            imgDirs = 0
            imgFiles = 0
            CreationTime = "N/A"
            ModifyTime = "N/A"
            imgLangs = "N/A"
            imgRW = "N/A"
        End If
        If IsReadOnly Then
            RemountImageWithWritePermissionsToolStripMenuItem.Enabled = True
        Else
            RemountImageWithWritePermissionsToolStripMenuItem.Enabled = False
        End If
        If SkipBGProcs Then Exit Sub
        ' Set image properties
        If expBackgroundProcesses Then
            ImgBW.RunWorkerAsync()
            Exit Sub
        End If
        Label14.Text = ProgressPanel.ImgIndex
        Label12.Text = ProgressPanel.MountDir
        ' Loading the project directly with an image already mounted makes the two labels above be wrong.
        ' Check them and use local vars
        If Label14.Text = "0" Or Label12.Text = "" Then     ' Label14 (index preview label) returns 0 and Label12 (mount dir preview) returns blank
            Label14.Text = ImgIndex
            Label12.Text = MountDir
        End If
        Try
            If ProgressPanel.MountDir = "" Then
                Throw New Exception
            Else
                Dim KeVerInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(ProgressPanel.MountDir & "\Windows\system32\ntoskrnl.exe")    ' Get version info from ntoskrnl.exe
                Dim KeVerStr As String = KeVerInfo.ProductVersion
                Label17.Text = KeVerStr
                Select Case DismVersionChecker.ProductMajorPart
                    Case 6
                        Select Case DismVersionChecker.ProductMinorPart
                            Case 1
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " > imgname", _
                                                  ASCII)
                            Case Is >= 2
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " > imgname", _
                                                  ASCII)
                        End Select
                    Case 10
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " > imgname", _
                                          ASCII)
                End Select
                Process.Start(Application.StartupPath & "\bin\exthelpers\temp.bat").WaitForExit()
                Label18.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\imgname").Replace("Name : ", "").Trim()
                File.Delete(Application.StartupPath & "\imgname")
                File.Delete(Application.StartupPath & "\bin\exthelpers\temp.bat")
                Select Case DismVersionChecker.ProductMajorPart
                    Case 6
                        Select Case DismVersionChecker.ProductMinorPart
                            Case 1
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                                  ASCII)
                            Case Is >= 2
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                                  ASCII)
                        End Select
                    Case 10
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                          ASCII)
                End Select
                Process.Start(Application.StartupPath & "\bin\exthelpers\temp.bat").WaitForExit()
                Label20.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\imgdesc").Replace("Description : ", "").Trim()
                File.Delete(Application.StartupPath & "\imgdesc")
                File.Delete(Application.StartupPath & "\bin\exthelpers\temp.bat")
                If Label18.Text = "" Or Label20.Text = "" Then
                    Label18.Text = imgMountedName
                    Label20.Text = imgMountedDesc
                End If
            End If
        Catch ex As Exception
            ' Maybe it was loaded directly. Check local vars
            Try
                Dim KeVerInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(MountDir & "\Windows\system32\ntoskrnl.exe")    ' Get version info from ntoskrnl.exe
                Dim KeVerStr As String = KeVerInfo.ProductVersion
                Label17.Text = KeVerStr
                Select Case DismVersionChecker.ProductMajorPart
                    Case 6
                        Select Case DismVersionChecker.ProductMinorPart
                            Case 1
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Name" & Quote & " > imgname", _
                                                  ASCII)
                            Case Is >= 2
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Name" & Quote & " > imgname", _
                                                  ASCII)
                        End Select
                    Case 10
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Name" & Quote & " > imgname", _
                                          ASCII)
                End Select
                Process.Start(Application.StartupPath & "\bin\exthelpers\temp.bat").WaitForExit()
                Label18.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\imgname").Replace("Name : ", "").Trim()
                File.Delete(Application.StartupPath & "\imgname")
                File.Delete(Application.StartupPath & "\bin\exthelpers\temp.bat")
                Select Case DismVersionChecker.ProductMajorPart
                    Case 6
                        Select Case DismVersionChecker.ProductMinorPart
                            Case 1
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                                  ASCII)
                            Case Is >= 2
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                                  ASCII)
                        End Select
                    Case 10
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                          ASCII)
                End Select
                Process.Start(Application.StartupPath & "\bin\exthelpers\temp.bat").WaitForExit()
                Label20.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\imgdesc").Replace("Description : ", "").Trim()
                File.Delete(Application.StartupPath & "\imgdesc")
                File.Delete(Application.StartupPath & "\bin\exthelpers\temp.bat")
                If Label18.Text = "" Or Label20.Text = "" Then
                    Label18.Text = imgMountedName
                    Label20.Text = imgMountedDesc
                End If
            Catch ex2 As Exception      ' It is clear that something went seriously wrong. Assume the image was unmounted before loading the proj in first place
                UpdateImgProps()        ' and exit the sub (a.k.a., give up)
                Exit Sub
            End Try
        End Try
        ' Detect whether the image needs a servicing session reload
        Directory.CreateDirectory(projPath & "\tempinfo")
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                          "@echo off" & CrLf & _
                                          "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & projPath & "\tempinfo\imgmountedstatus", ASCII)
                    Case Is >= 2
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                          "@echo off" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & projPath & "\tempinfo\imgmountedstatus", ASCII)
                End Select
            Case 10
                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                  "@echo off" & CrLf & _
                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & projPath & "\tempinfo\imgmountedstatus", ASCII)
        End Select
        Process.Start(Application.StartupPath & "\bin\exthelpers\imginfo.bat").WaitForExit()
        mountedImgStatus = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgmountedstatus", ASCII).Replace("Status : ", "").Trim()
        File.Delete(Application.StartupPath & "\bin\exthelpers\imginfo.bat")
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                          "@echo off" & CrLf & _
                                          "dism /English /get-wiminfo /wimfile=" & SourceImg & " | find /c " & Quote & "Index" & Quote & " > " & projPath & "\tempinfo\indexcount", ASCII)
                    Case Is >= 2
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                          "@echo off" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " | find /c " & Quote & "Index" & Quote & " > " & projPath & "\tempinfo\indexcount", ASCII)
                End Select
            Case 10
                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                  "@echo off" & CrLf & _
                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " | find /c " & Quote & "Index" & Quote & " > " & projPath & "\tempinfo\indexcount", ASCII)
        End Select
        Process.Start(Application.StartupPath & "\bin\exthelpers\imginfo.bat").WaitForExit()
        imgIndexCount = CInt(My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\indexcount", ASCII))
        File.Delete(Application.StartupPath & "\bin\exthelpers\imginfo.bat")
        For Each FoundFile In My.Computer.FileSystem.GetFiles(projPath & "\tempinfo", FileIO.SearchOption.SearchTopLevelOnly)
            File.Delete(FoundFile)
        Next
        Directory.Delete(projPath & "\tempinfo")
        If mountedImgStatus = "Ok" Then
            isOrphaned = False
        ElseIf mountedImgStatus = "Needs Remount" Then
            isOrphaned = True
        End If
        If isOrphaned Then
            OrphanedMountedImgDialog.ShowDialog(Me)
            If OrphanedMountedImgDialog.DialogResult = Windows.Forms.DialogResult.OK Then
                ProgressPanel.Validate()
                ProgressPanel.MountDir = MountDir
                ProgressPanel.OperationNum = 18
                ProgressPanel.ShowDialog(Me)
            ElseIf OrphanedMountedImgDialog.DialogResult = Windows.Forms.DialogResult.Cancel Then
                UnloadDTProj(False, True, False)
            End If
            Exit Sub
        End If
        UpdateImgProps()
    End Sub

    Sub UpdateImgProps()
        If IsImageMounted Then
            Try     ' Try getting image properties
                If Not Directory.Exists(projPath & "\tempinfo") Then
                    Directory.CreateDirectory(projPath & "\tempinfo").Attributes = FileAttributes.Hidden
                End If
                Select Case DismVersionChecker.ProductMajorPart
                    Case 6
                        Select Case DismVersionChecker.ProductMinorPart
                            Case 1
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                                  "@echo off" & CrLf & _
                                                  "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Mount Dir" & Quote & " /b > " & projPath & "\tempinfo\mountdir" & CrLf & _
                                                  "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Image File" & Quote & " /b > " & projPath & "\tempinfo\imgfile" & CrLf & _
                                                  "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Image Index" & Quote & " /b > " & projPath & "\tempinfo\imgindex" & CrLf & _
                                                  "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Mounted Read/Write" & Quote & " /b > " & projPath & "\tempinfo\imgrw" & CrLf & _
                                                  "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & projPath & "\tempinfo\imgmountedstatus" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " /b > " & projPath & "\tempinfo\imgmountedname" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Description" & Quote & " /b > " & projPath & "\tempinfo\imgmounteddesc" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Size" & Quote & " /b > " & projPath & "\tempinfo\imgsize" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & projPath & "\tempinfo\imgwimboot" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Architecture" & Quote & " /b > " & projPath & "\tempinfo\imgarch" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Hal" & Quote & " /b > " & projPath & "\tempinfo\imghal" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ServicePack Build" & Quote & " /b > " & projPath & "\tempinfo\imgspbuild" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ServicePack Level" & Quote & " /b > " & projPath & "\tempinfo\imgsplevel" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Edition" & Quote & " /b > " & projPath & "\tempinfo\imgedition" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Installation" & Quote & " /b > " & projPath & "\tempinfo\imginst" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ProductType" & Quote & " /b > " & projPath & "\tempinfo\imgptype" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ProductSuite" & Quote & " /b > " & projPath & "\tempinfo\imgpsuite" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "System Root" & Quote & " /b > " & projPath & "\tempinfo\imgsysroot" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Directories" & Quote & " /b > " & projPath & "\tempinfo\imgdirs" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Files" & Quote & " /b > " & projPath & "\tempinfo\imgfiles" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Created" & Quote & " /b > " & projPath & "\tempinfo\imgcreation" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Modified" & Quote & " /b > " & projPath & "\tempinfo\imgmodification" & CrLf & _
                                                  "dism /English /image=" & MountDir & " /get-intl | findstr /c:" & Quote & "Installed language(s):" & Quote & " /b > " & projPath & "\tempinfo\imglangs", ASCII)
                            Case Is >= 2
                                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                                  "@echo off" & CrLf & _
                                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mount Dir" & Quote & " /b > " & projPath & "\tempinfo\mountdir" & CrLf & _
                                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image File" & Quote & " /b > " & projPath & "\tempinfo\imgfile" & CrLf & _
                                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image Index" & Quote & " /b > " & projPath & "\tempinfo\imgindex" & CrLf & _
                                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mounted Read/Write" & Quote & " /b > " & projPath & "\tempinfo\imgrw" & CrLf & _
                                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & projPath & "\tempinfo\imgmountedstatus" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " /b > " & projPath & "\tempinfo\imgmountedname" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Description" & Quote & " /b > " & projPath & "\tempinfo\imgmounteddesc" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Size" & Quote & " /b > " & projPath & "\tempinfo\imgsize" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & projPath & "\tempinfo\imgwimboot" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Architecture" & Quote & " /b > " & projPath & "\tempinfo\imgarch" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Hal" & Quote & " /b > " & projPath & "\tempinfo\imghal" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ServicePack Build" & Quote & " /b > " & projPath & "\tempinfo\imgspbuild" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ServicePack Level" & Quote & " /b > " & projPath & "\tempinfo\imgsplevel" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Edition" & Quote & " /b > " & projPath & "\tempinfo\imgedition" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Installation" & Quote & " /b > " & projPath & "\tempinfo\imginst" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ProductType" & Quote & " /b > " & projPath & "\tempinfo\imgptype" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ProductSuite" & Quote & " /b > " & projPath & "\tempinfo\imgpsuite" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "System Root" & Quote & " /b > " & projPath & "\tempinfo\imgsysroot" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Directories" & Quote & " /b > " & projPath & "\tempinfo\imgdirs" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Files" & Quote & " /b > " & projPath & "\tempinfo\imgfiles" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Created" & Quote & " /b > " & projPath & "\tempinfo\imgcreation" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Modified" & Quote & " /b > " & projPath & "\tempinfo\imgmodification" & CrLf & _
                                                  "dism /English /image=" & MountDir & " /get-intl | findstr /c:" & Quote & "Installed language(s):" & Quote & " /b > " & projPath & "\tempinfo\imglangs", ASCII)
                        End Select
                    Case 10
                        File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat", _
                                          "@echo off" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mount Dir" & Quote & " /b > " & projPath & "\tempinfo\mountdir" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image File" & Quote & " /b > " & projPath & "\tempinfo\imgfile" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image Index" & Quote & " /b > " & projPath & "\tempinfo\imgindex" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mounted Read/Write" & Quote & " /b > " & projPath & "\tempinfo\imgrw" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & projPath & "\tempinfo\imgmountedstatus" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " /b > " & projPath & "\tempinfo\imgmountedname" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Description" & Quote & " /b > " & projPath & "\tempinfo\imgmounteddesc" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Size" & Quote & " /b > " & projPath & "\tempinfo\imgsize" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & projPath & "\tempinfo\imgwimboot" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Architecture" & Quote & " /b > " & projPath & "\tempinfo\imgarch" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Hal" & Quote & " /b > " & projPath & "\tempinfo\imghal" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ServicePack Build" & Quote & " /b > " & projPath & "\tempinfo\imgspbuild" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ServicePack Level" & Quote & " /b > " & projPath & "\tempinfo\imgsplevel" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Edition" & Quote & " /b > " & projPath & "\tempinfo\imgedition" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Installation" & Quote & " /b > " & projPath & "\tempinfo\imginst" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ProductType" & Quote & " /b > " & projPath & "\tempinfo\imgptype" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "ProductSuite" & Quote & " /b > " & projPath & "\tempinfo\imgpsuite" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "System Root" & Quote & " /b > " & projPath & "\tempinfo\imgsysroot" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Directories" & Quote & " /b > " & projPath & "\tempinfo\imgdirs" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Files" & Quote & " /b > " & projPath & "\tempinfo\imgfiles" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Created" & Quote & " /b > " & projPath & "\tempinfo\imgcreation" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr /c:" & Quote & "Modified" & Quote & " /b > " & projPath & "\tempinfo\imgmodification" & CrLf & _
                                          "dism /English /image=" & MountDir & " /get-intl | findstr /c:" & Quote & "Installed language(s):" & Quote & " /b > " & projPath & "\tempinfo\imglangs", ASCII)
                End Select

                If Debugger.IsAttached Then
                    Process.Start("\Windows\system32\notepad.exe", Application.StartupPath & "\bin\exthelpers\imginfo.bat").WaitForExit()
                End If
                Process.Start(Application.StartupPath & "\bin\exthelpers\imginfo.bat").WaitForExit()
                'imgName = SourceImg
                'ImgIndex = ImgIndex
                'imgMountDir = MountDir
                imgMountedStatus = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgmountedstatus", ASCII).Replace("Status : ", "").Trim()
                Try
                    Dim KeVerInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(MountDir & "\Windows\system32\ntoskrnl.exe")
                    Dim KeVerStr As String = KeVerInfo.ProductVersion
                    imgVersion = KeVerStr
                    imgMountedName = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgmountedname", ASCII).Replace("Name : ", "").Trim()
                    imgMountedDesc = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgmounteddesc", ASCII).Replace("Description : ", "").Trim()
                    Dim ImgSizeDbl As Double
                    ImgSizeDbl = CDbl(My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgsize", ASCII).Replace("Size : ", "").Trim().Replace(" bytes", "").Trim().Replace(".", "").Trim()) / (1024 ^ 3)
                    Dim ImgSizeStr As String
                    ImgSizeStr = Math.Round(ImgSizeDbl, 2)
                    imgSize = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgsize", ASCII).Replace("Size : ", "").Trim()
                    imgWimBootStatus = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgwimboot", ASCII).Replace("WIM Bootable : ", "").Trim()
                    imgArch = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgarch", ASCII).Replace("Architecture : ", "").Trim()
                    imgHal = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imghal", ASCII).Replace("Hal : ", "").Trim()
                    imgSPBuild = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgspbuild", ASCII).Replace("ServicePack Build : ", "").Trim()
                    imgSPLvl = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgsplevel", ASCII).Replace("ServicePack Level : ", "").Trim()
                    imgEdition = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgedition", ASCII).Replace("Edition : ", "").Trim()
                    imgPType = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgptype", ASCII).Replace("ProductType : ", "").Trim()
                    imgPSuite = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgpsuite", ASCII).Replace("ProductSuite : ", "").Trim()
                    imgSysRoot = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgsysroot", ASCII).Replace("System Root : ", "").Trim()
                    imgDirs = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgdirs", ASCII).Replace("Directories : ", "").Trim()
                    imgFiles = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgfiles", ASCII).Replace("Files : ", "").Trim()
                    imgCreation = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgcreation", ASCII).Replace("Created : ", "").Trim()
                    imgModification = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgmodification", ASCII).Replace("Modified : ", "").Trim()
                    imgLangs = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imglangs", ASCII).Replace("Installed language(s): ", "").Trim()
                    imgFormat = Path.GetExtension(SourceImg).Replace(".", "").Trim().ToUpper() & " file"
                    imgRW = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgrw", ASCII).Replace("Mounted Read/Write : ", "").Trim()
                    'If imgRW = "Yes" Then
                    '    RWRemountBtn.Visible = False
                    'ElseIf imgRW = "No" Then
                    '    RWRemountBtn.Visible = True
                    'End If
                    For Each foundFile In My.Computer.FileSystem.GetFiles(projPath & "\tempinfo", FileIO.SearchOption.SearchTopLevelOnly)
                        File.Delete(foundFile)
                    Next
                    Directory.Delete(projPath & "\tempinfo")
                    File.Delete(Application.StartupPath & "\bin\exthelpers\imginfo.bat")
                Catch ex As Exception

                End Try
                imgVersion = imgVersion
                imgMountedStatus = imgMountedStatus
                imgMountedName = imgMountedName
                imgMountedDesc = imgMountedDesc
                imgWimBootStatus = imgWimBootStatus
                imgArch = imgArch
                imgHal = imgHal
                imgSPBuild = imgSPBuild
                imgSPLvl = imgSPLvl
                imgEdition = imgEdition
                imgPType = imgPType
                imgPSuite = imgPSuite
                imgSysRoot = imgSysRoot
                imgDirs = CInt(imgDirs)
                imgFiles = CInt(imgFiles)
                imgCreation = imgCreation
                CreationTime = imgCreation.Replace(" - ", " ")
                imgModification = imgModification
                ModifyTime = imgModification.Replace(" - ", " ")
                'imgLangs = imgLangText
                imgRW = imgRW
                Button1.Enabled = False
                Button2.Enabled = True
                Button3.Enabled = True
                Button4.Enabled = True
                Button5.Enabled = True
                Button6.Enabled = True
                Button7.Enabled = True
                Button8.Enabled = True
                Button9.Enabled = True
                Button10.Enabled = True
                Button11.Enabled = True
                Button12.Enabled = True
                Button13.Enabled = True
                ' Update the buttons in the new design accordingly
                Button26.Enabled = False
                Button27.Enabled = True
                Button28.Enabled = True
                Button29.Enabled = True
                Button24.Enabled = True
                Button25.Enabled = True
                Button30.Enabled = True
                Button31.Enabled = True
                Button32.Enabled = True
                Button33.Enabled = True
                Button34.Enabled = True
                Button35.Enabled = True
                Button36.Enabled = True
                Button37.Enabled = True
                Button38.Enabled = True
                Button39.Enabled = True
                Button40.Enabled = True
                Button41.Enabled = True
                Button42.Enabled = True
                Button43.Enabled = True
                Button44.Enabled = True
                Button45.Enabled = True
                Button46.Enabled = True
                Button47.Enabled = True
                Button48.Enabled = True
                Button49.Enabled = True
                Button50.Enabled = True
                Button51.Enabled = True
                Button52.Enabled = True
                Button53.Enabled = True
                Button54.Enabled = True
                Button55.Enabled = True
                Button56.Enabled = True
                Button57.Enabled = True
                Button58.Enabled = True
                DetectNTVersion(MountDir & "\Windows\system32\ntoskrnl.exe")
            Catch ex As Exception

            End Try
            'Label4.Visible = False
        Else
            MountDir = "N/A"
            'Label19 = "No"
            'imgMountDir = "Not available"
            'ImgIndex = "Not available"
            'imgName = "Not available"
            'imgMountedStatus = "Not available"
            'imgVersion = "Not available"
            'imgMountedName = "Not available"
            'imgMountedDesc = "Not available"
            'imgSize = "Not available"
            'imgWimBootStatus = "Not available"
            'imgArch = "Not available"
            'imgHal = "Not available"
            'imgSPBuild = "Not available"
            'imgSPLvl = "Not available"
            'imgEdition = "Not available"
            'imgPType = "Not available"
            'imgPSuite = "Not available"
            'imgSysRoot = "Not available"
            'imgDirs = "Not available"
            'imgFiles = "Not available"
            'imgCreation = "Not available"
            'imgModification = "Not available"
            'imgFormat = "Not available"
            'imgRW = "Not available"
            'Panel3.Visible = True
            'Label4.Visible = False
            Button1.Enabled = True
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            Button5.Enabled = False
            Button6.Enabled = False
            Button7.Enabled = False
            Button8.Enabled = False
            Button9.Enabled = False
            Button10.Enabled = False
            Button11.Enabled = False
            Button12.Enabled = False
            Button13.Enabled = False
            ' Update the buttons in the new design accordingly
            Button26.Enabled = True
            Button27.Enabled = False
            Button28.Enabled = False
            Button29.Enabled = False
            Button24.Enabled = False
            Button25.Enabled = False
            Button30.Enabled = False
            Button31.Enabled = False
            Button32.Enabled = False
            Button33.Enabled = False
            Button34.Enabled = False
            Button35.Enabled = False
            Button36.Enabled = False
            Button37.Enabled = False
            Button38.Enabled = False
            Button39.Enabled = False
            Button40.Enabled = False
            Button41.Enabled = False
            Button42.Enabled = False
            Button43.Enabled = False
            Button44.Enabled = False
            Button45.Enabled = False
            Button46.Enabled = False
            Button47.Enabled = False
            Button48.Enabled = False
            Button49.Enabled = False
            Button50.Enabled = False
            Button51.Enabled = False
            Button52.Enabled = False
            Button53.Enabled = False
            Button54.Enabled = False
            Button55.Enabled = False
            Button56.Enabled = False
            Button57.Enabled = False
            Button58.Enabled = False
        End If
    End Sub

    Sub PopulateProjectTree(MainProjNameNode As String)
        prjTreeStatus.Visible = True
        Try
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            prjTreeView.Nodes.Add("parent", "Project: " & Quote & MainProjNameNode & Quote)
                            prjTreeView.Nodes("parent").Nodes.Add("dandi", "ADK Deployment Tools")
                            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_x86", "Deployment Tools (x86)")
                            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_amd64", "Deployment Tools (AMD64)")
                            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_arm", "Deployment Tools (ARM)")
                            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_arm64", "Deployment Tools (ARM64)")
                            prjTreeView.Nodes("parent").Nodes.Add("mount", "Mount point")
                            prjTreeView.Nodes("parent").Nodes.Add("unattend_xml", "Unattended answer files")
                            prjTreeView.Nodes("parent").Nodes.Add("scr_temp", "Scratch directory")
                            prjTreeView.Nodes("parent").Nodes.Add("reports", "Project reports")
                        Case "ESN"
                            prjTreeView.Nodes.Add("parent", "Proyecto: " & Quote & MainProjNameNode & Quote)
                            prjTreeView.Nodes("parent").Nodes.Add("dandi", "Herramientas de implementación")
                            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_x86", "Herramientas de implementación (x86)")
                            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_amd64", "Herramientas de implementación (AMD64)")
                            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_arm", "Herramientas de implementación (ARM)")
                            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_arm64", "Herramientas de implementación (ARM64)")
                            prjTreeView.Nodes("parent").Nodes.Add("mount", "Punto de montaje")
                            prjTreeView.Nodes("parent").Nodes.Add("unattend_xml", "Archivos de respuesta desatendida")
                            prjTreeView.Nodes("parent").Nodes.Add("scr_temp", "Directorio temporal")
                            prjTreeView.Nodes("parent").Nodes.Add("reports", "Informes del proyecto")
                        Case "FRA"
                            prjTreeView.Nodes.Add("parent", "Projet: " & Quote & MainProjNameNode & Quote)
                            prjTreeView.Nodes("parent").Nodes.Add("dandi", "Outils de déploiement ADK")
                            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_x86", "Outils de déploiement (x86)")
                            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_amd64", "Outils de déploiement (AMD64)")
                            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_arm", "Outils de déploiement (ARM)")
                            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_arm64", "Outils de déploiement (ARM64)")
                            prjTreeView.Nodes("parent").Nodes.Add("mount", "Point de montage")
                            prjTreeView.Nodes("parent").Nodes.Add("unattend_xml", "Fichiers de réponse non surveillés")
                            prjTreeView.Nodes("parent").Nodes.Add("scr_temp", "Répertoire temporaire")
                            prjTreeView.Nodes("parent").Nodes.Add("reports", "Rapports de projet")
                    End Select
                Case 1
                    prjTreeView.Nodes.Add("parent", "Project: " & Quote & MainProjNameNode & Quote)
                    prjTreeView.Nodes("parent").Nodes.Add("dandi", "ADK Deployment Tools")
                    prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_x86", "Deployment Tools (x86)")
                    prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_amd64", "Deployment Tools (AMD64)")
                    prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_arm", "Deployment Tools (ARM)")
                    prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_arm64", "Deployment Tools (ARM64)")
                    prjTreeView.Nodes("parent").Nodes.Add("mount", "Mount point")
                    prjTreeView.Nodes("parent").Nodes.Add("unattend_xml", "Unattended answer files")
                    prjTreeView.Nodes("parent").Nodes.Add("scr_temp", "Scratch directory")
                    prjTreeView.Nodes("parent").Nodes.Add("reports", "Project reports")
                Case 2
                    prjTreeView.Nodes.Add("parent", "Proyecto: " & Quote & MainProjNameNode & Quote)
                    prjTreeView.Nodes("parent").Nodes.Add("dandi", "Herramientas de implementación")
                    prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_x86", "Herramientas de implementación (x86)")
                    prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_amd64", "Herramientas de implementación (AMD64)")
                    prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_arm", "Herramientas de implementación (ARM)")
                    prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_arm64", "Herramientas de implementación (ARM64)")
                    prjTreeView.Nodes("parent").Nodes.Add("mount", "Punto de montaje")
                    prjTreeView.Nodes("parent").Nodes.Add("unattend_xml", "Archivos de respuesta desatendida")
                    prjTreeView.Nodes("parent").Nodes.Add("scr_temp", "Directorio temporal")
                    prjTreeView.Nodes("parent").Nodes.Add("reports", "Informes del proyecto")
                Case 3
                    prjTreeView.Nodes.Add("parent", "Projet: " & Quote & MainProjNameNode & Quote)
                    prjTreeView.Nodes("parent").Nodes.Add("dandi", "Outils de déploiement ADK")
                    prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_x86", "Outils de déploiement (x86)")
                    prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_amd64", "Outils de déploiement (AMD64)")
                    prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_arm", "Outils de déploiement (ARM)")
                    prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_arm64", "Outils de déploiement (ARM64)")
                    prjTreeView.Nodes("parent").Nodes.Add("mount", "Point de montage")
                    prjTreeView.Nodes("parent").Nodes.Add("unattend_xml", "Fichiers de réponse non surveillés")
                    prjTreeView.Nodes("parent").Nodes.Add("scr_temp", "Répertoire temporaire")
                    prjTreeView.Nodes("parent").Nodes.Add("reports", "Rapports de projet")
            End Select
            prjTreeView.ExpandAll()
        Catch ex As Exception

        End Try
        prjTreeStatus.Visible = False
    End Sub

    Sub UnpopulateProjectTree()
        prjTreeView.Nodes.Clear()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked, LinkLabel2.LinkClicked
        ImgMount.ShowDialog()
    End Sub

    Private Sub ProjNameEditBtn_Click(sender As Object, e As EventArgs) Handles ProjNameEditBtn.Click
        If IsInEditMode Then
            IsInEditMode = False
            ProjNameEditBtn.Image = New Bitmap(My.Resources.proj_name_edit)
            projName.Text = projNameText.Text
            projName.Visible = True
            projNameText.Visible = False
        Else
            IsInEditMode = True
            ProjNameEditBtn.Image = New Bitmap(My.Resources.proj_name_set)
            projNameText.Text = projName.Text
            projName.Visible = False
            projNameText.Visible = True
        End If
    End Sub

#Region "MenuStrip entries"
    Sub ShowParentDesc(ParentDescMode As Integer)
        Select Case ParentDescMode
            Case 1
                MenuDesc.Text = "View options related to files, like creating or opening projects"
            Case 2
                MenuDesc.Text = "View options related to this project, like viewing its properties"
            Case 3
                MenuDesc.Text = "View options related to image management, deployment and/or servicing"
            Case 4
                MenuDesc.Text = "View options related to additional tools, like the Command Console"
            Case 5
                MenuDesc.Text = "View options related to help topics, glossary, command help and product information"
            Case Else
                ' Do not show anything
        End Select
    End Sub

    Sub ShowChildDescs(IsCmdDescription As Boolean, ChildDescMode As Integer)
        If IsCmdDescription Then
            Dim CommandDescriptionInt As Integer = ChildDescMode
            ' ChildDescMode follows the same style as ProgressPanel.OperationNum
            Select Case CommandDescriptionInt
                Case 1
                    MenuDesc.Text = "Adds an additional image to a .wim file"
                Case 2
                    MenuDesc.Text = "Applies a Full Flash Utility or split FFU to a physical drive"
                Case 3
                    MenuDesc.Text = "Applies a Windows image or split WIM to a partition"
                Case 4
                    MenuDesc.Text = "Captures incremental file changes on the specific WIM file to " & Quote & "custom.wim" & Quote
                Case 5
                    MenuDesc.Text = "Captures an image of a drive's partitions to a new FFU file"
                Case 6
                    MenuDesc.Text = "Captures an image of a drive to a new WIM file"
                Case 7
                    MenuDesc.Text = "Deletes all resources associated with a corrupted mounted image"
                Case 8
                    MenuDesc.Text = "Applies the changes made to the mounted image"
                Case 9
                    MenuDesc.Text = "Deletes a volume image from a WIM file"
                Case 10
                    MenuDesc.Text = "Exports a copy of the image to another file"
                Case 11
                    MenuDesc.Text = "Displays information about the images contained in a WIM, FFU, VHD or VHDX file"
                Case 12
                    MenuDesc.Text = "Displays a list of WIM, FFU, VHD or VHDX images that are currently mounted"
                Case 13
                    MenuDesc.Text = "Displays WIMBoot configuration entries for the specified disk volume"
                Case 14
                    MenuDesc.Text = "Displays a list of files and folders in an image"
                Case 15
                    MenuDesc.Text = "Mounts an image from a WIM, FFU, VHD or VHDX to make it available for servicing"
                Case 16
                    MenuDesc.Text = "Optimizes a FFU image to make it faster to deploy"
                Case 17
                    MenuDesc.Text = "Optimizes an image to make it faster to deploy"
                Case 18
                    MenuDesc.Text = "Remounts a mounted image that is inaccessible to make it available for servicing"
                Case 19
                    MenuDesc.Text = "Splits a Full Flash Utility (FFU) file into read-only split FFU (.sfu) files"
                Case 20
                    MenuDesc.Text = "Splits an existing WIM file into read-only split WIM (.swm) files"
                Case 21
                    MenuDesc.Text = "Unmounts the WIM, FFU, VHD or VHDX file and either commits or discards its changes"
                Case 22
                    MenuDesc.Text = "Updates the WIMBoot configuration entry"
                Case 23
                    MenuDesc.Text = "Applies siloed provisioning packages to the image"
                Case 24
                    MenuDesc.Text = "Displays information about all packages in the image or in the installation or any package file you want to add"
                Case 26
                    MenuDesc.Text = "Installs a .cab or .msu package in the image"
                Case 27
                    MenuDesc.Text = "Removes a .cab file package from the image"
                Case 28
                    MenuDesc.Text = "Displays information about the installed features in an image or an online installation"
                Case 30
                    MenuDesc.Text = "Enables or updates the specified feature in the image"
                Case 31
                    MenuDesc.Text = "Disables the specified feature in the image"
                Case 32
                    MenuDesc.Text = "Performs cleanup or recovery operations on the image"
                Case 33
                    MenuDesc.Text = "Adds an applicable payload of a provisioning package to the image"
                Case 34
                    MenuDesc.Text = "Gets infomation of a provisioning package"
                Case 35
                    MenuDesc.Text = "Dehydrates files contained in the custom data image to save space"
                Case 36
                    MenuDesc.Text = "Displays information about app packages in an image"
                Case 37
                    MenuDesc.Text = "Adds one or more app packages to the image"
                Case 38
                    MenuDesc.Text = "Removes provisioning for app packages from the image"
                Case 39
                    MenuDesc.Text = "Optimizes the total size of provisioned app packages on the image"
                Case 40
                    MenuDesc.Text = "Adds a custom data file into the specified app package"
                Case 41
                    MenuDesc.Text = "Displays information of MSP patches applicable to the offline image"
                Case 42
                    MenuDesc.Text = "Displays information about installed MSP patches"
                Case 43
                    MenuDesc.Text = "Displays information about all applied MSP patches for all applications installed on the image"
                Case 44
                    MenuDesc.Text = "Displays information about a specific installed Windows Installer application"
                Case 45
                    MenuDesc.Text = "Displays information about all Windows Installer applications in the image"
                Case 46
                    MenuDesc.Text = "Exports default application associations from a running OS to an XML file"
                Case 47
                    MenuDesc.Text = "Displays the list of default application associations set in the image"
                Case 48
                    MenuDesc.Text = "Imports a set of default application associations from an XML file to an image"
                Case 49
                    MenuDesc.Text = "Removes default application associations from the image"
                Case 50
                    MenuDesc.Text = "Displays information about international settings and languages"
                Case 51
                    MenuDesc.Text = "Sets the default UI language"
                Case 52
                    MenuDesc.Text = "Sets the fallback default language for the system UI"
                Case 53
                    MenuDesc.Text = "Sets the " & Quote & "System Preferred" & Quote & " UI language"
                Case 54
                    MenuDesc.Text = "Sets the language for non-Unicode programs and font settings in the image"
                Case 55
                    MenuDesc.Text = "Sets the " & Quote & "standards and formats" & Quote & " language (user locale) in the image"
                Case 56
                    MenuDesc.Text = "Sets the input locales and keyboard layouts to use in the image"
                Case 57
                    MenuDesc.Text = "Sets the default system UI language, the language for non-Unicode programs, the user locale, and the keyboard layouts to the language in the image"
                Case 58
                    MenuDesc.Text = "Sets the default time zone in the image"
                Case 59
                    MenuDesc.Text = "Sets the default language for the UI and non-Unicode programs, locales for the user and input, keyboard layouts and time zone values in the image"
                Case 60
                    MenuDesc.Text = "Specifies a keyboard driver for Japanese and Korean keyboards"
                Case 61
                    MenuDesc.Text = "Generates a Lang.ini file, used by Setup to define the language packs inside the image and out"
                Case 62
                    MenuDesc.Text = "Defines the default language that will be used by Setup"
                Case 63
                    MenuDesc.Text = "Adds a capability to an image"
                Case 64
                    MenuDesc.Text = "Exports a set of capabilities into a new repository"
                Case 65
                    MenuDesc.Text = "Gets information about the installed capabilities of an image or an active installation"
                Case 67
                    MenuDesc.Text = "Removes a capability from the image"
                Case 68
                    MenuDesc.Text = "Displays the edition of the image"
                Case 69
                    MenuDesc.Text = "Displays the editions the image can be upgraded to"
                Case 70
                    MenuDesc.Text = "Changes an image to a higher edition"
                Case 71
                    MenuDesc.Text = "Enters the product key for the current edition"
                Case 72
                    MenuDesc.Text = "Displays information about the driver packages you specify or the installed drivers in the image or in the installation"
                Case 74
                    MenuDesc.Text = "Adds third-party driver packages to the image"
                Case 75
                    MenuDesc.Text = "Removes third-party drivers from the image"
                Case 76
                    MenuDesc.Text = "Exports all third-party driver packages from the image to a destination path"
                Case 77
                    MenuDesc.Text = "Applies an Unattend.xml file to the image"
                Case 78
                    MenuDesc.Text = "Displays a list of Windows PE settings in the WinPE image"
                Case 79
                    MenuDesc.Text = "Retrieves the configured amount of the Windows PE system volume scratch space"
                Case 80
                    MenuDesc.Text = "Retrieves the target path of the Windows PE image"
                Case 81
                    MenuDesc.Text = "Sets the available scratch space (in MB)"
                Case 82
                    MenuDesc.Text = "Sets the location of the WinPE image on the disk (for hard disk boot scenarios)"
                Case 83
                    MenuDesc.Text = "Gets the number of days an uninstall can be initiated after an upgrade"
                Case 84
                    MenuDesc.Text = "Reverts a PC to a previous installation"
                Case 85
                    MenuDesc.Text = "Removes the ability to roll back a PC to a previous installation"
                Case 86
                    MenuDesc.Text = "Sets the number of days an uninstall can be initiated after an upgrade"
                Case 87
                    MenuDesc.Text = "Gets the current state of reserved storage"
                Case 88
                    MenuDesc.Text = "Sets the state of reserved storage"
                Case 89             ' Edge can also be deployed
                    MenuDesc.Text = "Adds the Microsoft Edge Browser and WebView2 component to the image"
                Case 90
                    MenuDesc.Text = "Adds the Microsoft Edge Browser to the image"
                Case 91
                    MenuDesc.Text = "Adds the Microsoft Edge WebView2 component to the image"
                Case 92
                    MenuDesc.Text = "Saves complete image information to the file you want. Depending on the settings you had specified, you may be asked some questions during the process"
                Case Else
                    ' Do not show anything
            End Select
        Else
            Select Case ChildDescMode
                Case 1
                    MenuDesc.Text = "Creates a new DISMTools project. The current project will be unloaded after creating it"
                Case 2
                    MenuDesc.Text = "Opens an existing DISMTools project. The current project will be unloaded"
                Case 3
                    MenuDesc.Text = "Enters online installation management mode"
                Case 4
                    MenuDesc.Text = "Saves the changes of this project"
                Case 5
                    MenuDesc.Text = "Saves this project on another location"
                Case 6
                    MenuDesc.Text = "Closes the program. If a project is loaded, you will be asked whether or not you would like to save it"
                Case 7
                    MenuDesc.Text = "Opens the File Explorer to view the project files"
                Case 8
                    MenuDesc.Text = "Unloads this project. If changes were made, you will be asked whether or not you would like to save it"
                Case 9
                    MenuDesc.Text = "Switches the mounted image index"
                Case 10
                    MenuDesc.Text = "Launches the project section of the project properties dialog"
                Case 11
                    MenuDesc.Text = "Launches the image section of the project properties dialog"
                Case 12
                    MenuDesc.Text = "Performs image format conversion from WIM to ESD and vice versa"
                Case 13
                    MenuDesc.Text = "Merges two or more SWM files into a single WIM file"
                Case 14
                    MenuDesc.Text = "Remounts the image with read-write permissions to allow making modifications to it"
                Case 15
                    MenuDesc.Text = "Opens the Command Console"
                Case 16
                    MenuDesc.Text = "Lets you manage unattended answer files for this project"
                Case 17
                    MenuDesc.Text = "Lets you manage project reports"
                Case 18
                    MenuDesc.Text = "Shows an overview of the mounted images"
                Case 19
                    MenuDesc.Text = "Configures settings for the program"
                Case 20
                    MenuDesc.Text = "Opens the help topics for this program"
                Case 21
                    MenuDesc.Text = "Opens the glossary, if you don't understand a concept"
                Case 22
                    MenuDesc.Text = "Shows the Command Help, letting you use commands to perform the same actions"
                Case 23
                    MenuDesc.Text = "Shows program information"
                Case 24
                    MenuDesc.Text = "Lets you report feedback through a new GitHub issue (a GitHub account is needed)"
                Case 25
                    MenuDesc.Text = "Opens the GitHub repository containing the help documentation contents, to which you can contribute (a GitHub account is needed)"
            End Select
        End If
    End Sub

    Sub HideParentDesc()
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        MenuDesc.Text = "Ready"
                    Case "ESN"
                        MenuDesc.Text = "Listo"
                    Case "FRA"
                        MenuDesc.Text = "Prêt"
                End Select
            Case 1
                MenuDesc.Text = "Ready"
            Case 2
                MenuDesc.Text = "Listo"
            Case 3
                MenuDesc.Text = "Prêt"
        End Select
        If ImgBW.CancellationPending Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MenuDesc.Text = "Cancelling background processes. Please wait..."
                        Case "ESN"
                            MenuDesc.Text = "Espere mientras cancelamos los procesos en segundo plano..."
                        Case "FRA"
                            MenuDesc.Text = "Annulation des processus en arrière plan en cours. Veuillez patienter ..."
                    End Select
                Case 1
                    MenuDesc.Text = "Cancelling background processes. Please wait..."
                Case 2
                    MenuDesc.Text = "Espere mientras cancelamos los procesos en segundo plano..."
                Case 3
                    MenuDesc.Text = "Annulation des processus en arrière plan en cours. Veuillez patienter ..."
            End Select
        End If
    End Sub

    Sub HideChildDescs()
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        MenuDesc.Text = "Ready"
                    Case "ESN"
                        MenuDesc.Text = "Listo"
                    Case "FRA"
                        MenuDesc.Text = "Prêt"
                End Select
            Case 1
                MenuDesc.Text = "Ready"
            Case 2
                MenuDesc.Text = "Listo"
            Case 3
                MenuDesc.Text = "Prêt"
        End Select
        If ImgBW.CancellationPending Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MenuDesc.Text = "Cancelling background processes. Please wait..."
                        Case "ESN"
                            MenuDesc.Text = "Espere mientras cancelamos los procesos en segundo plano..."
                        Case "FRA"
                            MenuDesc.Text = "Annulation des processus en arrière plan en cours. Veuillez patienter ..."
                    End Select
                Case 1
                    MenuDesc.Text = "Cancelling background processes. Please wait..."
                Case 2
                    MenuDesc.Text = "Espere mientras cancelamos los procesos en segundo plano..."
                Case 3
                    MenuDesc.Text = "Annulation des processus en arrière plan en cours. Veuillez patienter ..."
            End Select
        End If
    End Sub

    Private Sub FileToolStripMenuItem_MouseEnter(sender As Object, e As EventArgs) Handles FileToolStripMenuItem.MouseEnter
        ShowParentDesc(1)
    End Sub

    Private Sub ProjectToolStripMenuItem_MouseEnter(sender As Object, e As EventArgs) Handles ProjectToolStripMenuItem.MouseEnter
        ShowParentDesc(2)
    End Sub

    Private Sub CommandsToolStripMenuItem_MouseEnter(sender As Object, e As EventArgs) Handles CommandsToolStripMenuItem.MouseEnter
        ShowParentDesc(3)
    End Sub

    Private Sub ToolsToolStripMenuItem_MouseEnter(sender As Object, e As EventArgs) Handles ToolsToolStripMenuItem.MouseEnter
        ShowParentDesc(4)
    End Sub

    Private Sub HelpToolStripMenuItem_MouseEnter(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.MouseEnter
        ShowParentDesc(5)
    End Sub

    Private Sub FileToolStripMenuItem_MouseLeave(sender As Object, e As EventArgs) Handles FileToolStripMenuItem.MouseLeave
        HideParentDesc()
    End Sub

    Private Sub ProjectToolStripMenuItem_MouseLeave(sender As Object, e As EventArgs) Handles ProjectToolStripMenuItem.MouseLeave
        HideParentDesc()
    End Sub

    Private Sub CommandsToolStripMenuItem_MouseLeave(sender As Object, e As EventArgs) Handles CommandsToolStripMenuItem.MouseLeave
        HideParentDesc()
    End Sub

    Private Sub ToolsToolStripMenuItem_MouseLeave(sender As Object, e As EventArgs) Handles ToolsToolStripMenuItem.MouseLeave
        HideParentDesc()
    End Sub

    Private Sub HelpToolStripMenuItem_MouseLeave(sender As Object, e As EventArgs) Handles HelpToolStripMenuItem.MouseLeave
        HideParentDesc()
    End Sub

    Private Sub AppendImage_MouseEnter(sender As Object, e As EventArgs) Handles AppendImage.MouseEnter
        ShowChildDescs(True, 1)
    End Sub

    Private Sub HideChildDescsTrigger(sender As Object, e As EventArgs) Handles AppendImage.MouseLeave, ApplyFFU.MouseLeave, ApplyImage.MouseLeave, CaptureCustomImage.MouseLeave, CaptureFFU.MouseLeave, CaptureImage.MouseLeave, CleanupMountpoints.MouseLeave, CommitImage.MouseLeave, DeleteImage.MouseLeave, ExportImage.MouseLeave, GetImageInfo.MouseLeave, GetWIMBootEntry.MouseLeave, ListImage.MouseLeave, MountImage.MouseLeave, OptimizeFFU.MouseLeave, OptimizeImage.MouseLeave, RemountImage.MouseLeave, SplitFFU.MouseLeave, SplitImage.MouseLeave, UnmountImage.MouseLeave, UpdateWIMBootEntry.MouseLeave, ApplySiloedPackage.MouseLeave, GetPackages.MouseLeave, AddPackage.MouseLeave, RemovePackage.MouseLeave, GetFeatures.MouseLeave, EnableFeature.MouseLeave, DisableFeature.MouseLeave, CleanupImage.MouseLeave, AddProvisionedAppxPackage.MouseLeave, GetProvisioningPackageInfo.MouseLeave, ApplyCustomDataImage.MouseLeave, GetProvisionedAppxPackages.MouseLeave, AddProvisionedAppxPackage.MouseLeave, RemoveProvisionedAppxPackage.MouseLeave, OptimizeProvisionedAppxPackages.MouseLeave, SetProvisionedAppxDataFile.MouseLeave, CheckAppPatch.MouseLeave, GetAppPatchInfo.MouseLeave, GetAppPatches.MouseLeave, GetAppInfo.MouseLeave, GetApps.MouseLeave, ExportDefaultAppAssociations.MouseLeave, GetDefaultAppAssociations.MouseLeave, ImportDefaultAppAssociations.MouseLeave, RemoveDefaultAppAssociations.MouseLeave, GetIntl.MouseLeave, SetUILangFallback.MouseLeave, SetSysUILang.MouseLeave, SetSysLocale.MouseLeave, SetUserLocale.MouseLeave, SetInputLocale.MouseLeave, SetAllIntl.MouseLeave, SetTimeZone.MouseLeave, SetSKUIntlDefaults.MouseLeave, SetLayeredDriver.MouseLeave, GenLangINI.MouseLeave, SetSetupUILang.MouseLeave, AddCapability.MouseLeave, ExportSource.MouseLeave, GetCapabilities.MouseLeave, RemoveCapability.MouseLeave, GetCurrentEdition.MouseLeave, GetTargetEditions.MouseLeave, SetEdition.MouseLeave, SetProductKey.MouseLeave, GetDrivers.MouseLeave, AddDriver.MouseLeave, RemoveDriver.MouseLeave, ExportDriver.MouseLeave, ApplyUnattend.MouseLeave, GetPESettings.MouseLeave, SetScratchSpace.MouseLeave, SetTargetPath.MouseLeave, GetOSUninstallWindow.MouseLeave, InitiateOSUninstall.MouseLeave, RemoveOSUninstall.MouseLeave, SetOSUninstallWindow.MouseLeave, SetReservedStorageState.MouseLeave, GetReservedStorageState.MouseLeave, NewProjectToolStripMenuItem.MouseLeave, OpenExistingProjectToolStripMenuItem.MouseLeave, SaveProjectToolStripMenuItem.MouseLeave, SaveProjectasToolStripMenuItem.MouseLeave, ExitToolStripMenuItem.MouseLeave, ViewProjectFilesInFileExplorerToolStripMenuItem.MouseLeave, UnloadProjectToolStripMenuItem.MouseLeave, SwitchImageIndexesToolStripMenuItem.MouseLeave, ProjectPropertiesToolStripMenuItem.MouseLeave, ImagePropertiesToolStripMenuItem.MouseLeave, ImageManagementToolStripMenuItem.MouseLeave, OSPackagesToolStripMenuItem.MouseLeave, ProvisioningPackagesToolStripMenuItem.MouseLeave, AppPackagesToolStripMenuItem.MouseLeave, AppPatchesToolStripMenuItem.MouseLeave, DefaultAppAssociationsToolStripMenuItem.MouseLeave, LanguagesAndRegionSettingsToolStripMenuItem.MouseLeave, CapabilitiesToolStripMenuItem.MouseLeave, WindowsEditionsToolStripMenuItem.MouseLeave, DriversToolStripMenuItem.MouseLeave, UnattendedAnswerFilesToolStripMenuItem.MouseLeave, WindowsPEServicingToolStripMenuItem.MouseLeave, OSUninstallToolStripMenuItem.MouseLeave, ReservedStorageToolStripMenuItem.MouseLeave, ImageConversionToolStripMenuItem.MouseLeave, WIMESDToolStripMenuItem.MouseLeave, RemountImageWithWritePermissionsToolStripMenuItem.MouseLeave, CommandShellToolStripMenuItem.MouseLeave, OptionsToolStripMenuItem.MouseLeave, HelpTopicsToolStripMenuItem.MouseLeave, GlossaryToolStripMenuItem.MouseLeave, CommandHelpToolStripMenuItem.MouseLeave, AboutDISMToolsToolStripMenuItem.MouseLeave, UnattendedAnswerFileManagerToolStripMenuItem.MouseLeave, AddEdge.MouseLeave, AddEdgeBrowser.MouseLeave, AddEdgeWebView.MouseLeave, ReportManagerToolStripMenuItem.MouseLeave, MergeSWM.MouseLeave, MountedImageManagerTSMI.MouseLeave, ReportFeedbackToolStripMenuItem.MouseLeave, ManageOnlineInstallationToolStripMenuItem.MouseLeave, AddProvisioningPackage.MouseLeave, SaveImageInformationToolStripMenuItem.MouseLeave, ContributeToTheHelpSystemToolStripMenuItem.MouseLeave
        HideChildDescs()
    End Sub

    Private Sub ApplyFFU_MouseEnter(sender As Object, e As EventArgs) Handles ApplyFFU.MouseEnter
        ShowChildDescs(True, 2)
    End Sub

    Private Sub ApplyImage_MouseEnter(sender As Object, e As EventArgs) Handles ApplyImage.MouseEnter
        ShowChildDescs(True, 3)
    End Sub

    Private Sub CaptureCustomImage_MouseEnter(sender As Object, e As EventArgs) Handles CaptureCustomImage.MouseEnter
        ShowChildDescs(True, 4)
    End Sub

    Private Sub CaptureFFU_MouseEnter(sender As Object, e As EventArgs) Handles CaptureFFU.MouseEnter
        ShowChildDescs(True, 5)
    End Sub

    Private Sub CaptureImage_MouseEnter(sender As Object, e As EventArgs) Handles CaptureImage.MouseEnter
        ShowChildDescs(True, 6)
    End Sub

    Private Sub CleanupMountpoints_MouseEnter(sender As Object, e As EventArgs) Handles CleanupMountpoints.MouseEnter
        ShowChildDescs(True, 7)
    End Sub

    Private Sub CommitImage_MouseEnter(sender As Object, e As EventArgs) Handles CommitImage.MouseEnter
        ShowChildDescs(True, 8)
    End Sub

    Private Sub DeleteImage_MouseEnter(sender As Object, e As EventArgs) Handles DeleteImage.MouseEnter
        ShowChildDescs(True, 9)
    End Sub

    Private Sub ExportImage_MouseEnter(sender As Object, e As EventArgs) Handles ExportImage.MouseEnter
        ShowChildDescs(True, 10)
    End Sub

    Private Sub GetImageInfo_MouseEnter(sender As Object, e As EventArgs) Handles GetImageInfo.MouseEnter
        ShowChildDescs(True, 11)
    End Sub

    Private Sub GetMountedImageInfo_MouseEnter(sender As Object, e As EventArgs)
        ShowChildDescs(True, 12)
    End Sub

    Private Sub GetWIMBootEntry_MouseEnter(sender As Object, e As EventArgs) Handles GetWIMBootEntry.MouseEnter
        ShowChildDescs(True, 13)
    End Sub

    Private Sub ListImage_MouseEnter(sender As Object, e As EventArgs) Handles ListImage.MouseEnter
        ShowChildDescs(True, 14)
    End Sub

    Private Sub MountImage_MouseEnter(sender As Object, e As EventArgs) Handles MountImage.MouseEnter
        ShowChildDescs(True, 15)
    End Sub

    Private Sub OptimizeFFU_MouseEnter(sender As Object, e As EventArgs) Handles OptimizeFFU.MouseEnter
        ShowChildDescs(True, 16)
    End Sub

    Private Sub OptimizeImage_MouseEnter(sender As Object, e As EventArgs) Handles OptimizeImage.MouseEnter
        ShowChildDescs(True, 17)
    End Sub

    Private Sub RemountImage_MouseEnter(sender As Object, e As EventArgs) Handles RemountImage.MouseEnter
        ShowChildDescs(True, 18)
    End Sub

    Private Sub SplitFFU_MouseEnter(sender As Object, e As EventArgs) Handles SplitFFU.MouseEnter
        ShowChildDescs(True, 19)
    End Sub

    Private Sub SplitImage_MouseEnter(sender As Object, e As EventArgs) Handles SplitImage.MouseEnter
        ShowChildDescs(True, 20)
    End Sub

    Private Sub UnmountImage_MouseEnter(sender As Object, e As EventArgs) Handles UnmountImage.MouseEnter
        ShowChildDescs(True, 21)
    End Sub

    Private Sub UpdateWIMBootEntry_MouseEnter(sender As Object, e As EventArgs) Handles UpdateWIMBootEntry.MouseEnter
        ShowChildDescs(True, 22)
    End Sub

    Private Sub ApplySiloedPackage_MouseEnter(sender As Object, e As EventArgs) Handles ApplySiloedPackage.MouseEnter
        ShowChildDescs(True, 23)
    End Sub

    Private Sub GetPackages_MouseEnter(sender As Object, e As EventArgs) Handles GetPackages.MouseEnter
        ShowChildDescs(True, 24)
    End Sub

    Private Sub GetPackageInfo_MouseEnter(sender As Object, e As EventArgs)
        ShowChildDescs(True, 25)
    End Sub

    Private Sub AddPackage_MouseEnter(sender As Object, e As EventArgs) Handles AddPackage.MouseEnter
        ShowChildDescs(True, 26)
    End Sub

    Private Sub RemovePackage_MouseEnter(sender As Object, e As EventArgs) Handles RemovePackage.MouseEnter
        ShowChildDescs(True, 27)
    End Sub

    Private Sub GetFeatures_MouseEnter(sender As Object, e As EventArgs) Handles GetFeatures.MouseEnter
        ShowChildDescs(True, 28)
    End Sub

    Private Sub EnableFeature_MouseEnter(sender As Object, e As EventArgs) Handles EnableFeature.MouseEnter
        ShowChildDescs(True, 30)
    End Sub

    Private Sub DisableFeature_MouseEnter(sender As Object, e As EventArgs) Handles DisableFeature.MouseEnter
        ShowChildDescs(True, 31)
    End Sub

    Private Sub CleanupImage_MouseEnter(sender As Object, e As EventArgs) Handles CleanupImage.MouseEnter
        ShowChildDescs(True, 32)
    End Sub

    Private Sub AddProvisioningPackage_MouseEnter(sender As Object, e As EventArgs) Handles AddProvisioningPackage.MouseEnter
        ShowChildDescs(True, 33)
    End Sub

    Private Sub GetProvisioningPackageInfo_MouseEnter(sender As Object, e As EventArgs) Handles GetProvisioningPackageInfo.MouseEnter
        ShowChildDescs(True, 34)
    End Sub

    Private Sub ApplyCustomDataImage_MouseEnter(sender As Object, e As EventArgs) Handles ApplyCustomDataImage.MouseEnter
        ShowChildDescs(True, 35)
    End Sub

    Private Sub GetProvisionedAppxPackages_MouseEnter(sender As Object, e As EventArgs) Handles GetProvisionedAppxPackages.MouseEnter
        ShowChildDescs(True, 36)
    End Sub

    Private Sub AddProvisionedAppxPackage_MouseEnter(sender As Object, e As EventArgs) Handles AddProvisionedAppxPackage.MouseEnter
        ShowChildDescs(True, 37)
    End Sub

    Private Sub RemoveProvisionedAppxPackage_MouseEnter(sender As Object, e As EventArgs) Handles RemoveProvisionedAppxPackage.MouseEnter
        ShowChildDescs(True, 38)
    End Sub

    Private Sub OptimizeProvisionedAppxPackages_MouseEnter(sender As Object, e As EventArgs) Handles OptimizeProvisionedAppxPackages.MouseEnter
        ShowChildDescs(True, 39)
    End Sub

    Private Sub SetProvisionedAppxDataFile_MouseEnter(sender As Object, e As EventArgs) Handles SetProvisionedAppxDataFile.MouseEnter
        ShowChildDescs(True, 40)
    End Sub

    Private Sub CheckAppPatch_MouseEnter(sender As Object, e As EventArgs) Handles CheckAppPatch.MouseEnter
        ShowChildDescs(True, 41)
    End Sub

    Private Sub GetAppPatchInfo_MouseEnter(sender As Object, e As EventArgs) Handles GetAppPatchInfo.MouseEnter
        ShowChildDescs(True, 42)
    End Sub

    Private Sub GetAppPatches_MouseEnter(sender As Object, e As EventArgs) Handles GetAppPatches.MouseEnter
        ShowChildDescs(True, 43)
    End Sub

    Private Sub GetAppInfo_MouseEnter(sender As Object, e As EventArgs) Handles GetAppInfo.MouseEnter
        ShowChildDescs(True, 44)
    End Sub

    Private Sub GetApps_MouseEnter(sender As Object, e As EventArgs) Handles GetApps.MouseEnter
        ShowChildDescs(True, 45)
    End Sub

    Private Sub ExportDefaultAppAssociations_MouseEnter(sender As Object, e As EventArgs) Handles ExportDefaultAppAssociations.MouseEnter
        ShowChildDescs(True, 46)
    End Sub

    Private Sub GetDefaultAppAssociations_MouseEnter(sender As Object, e As EventArgs) Handles GetDefaultAppAssociations.MouseEnter
        ShowChildDescs(True, 47)
    End Sub

    Private Sub ImportDefaultAppAssociations_MouseEnter(sender As Object, e As EventArgs) Handles ImportDefaultAppAssociations.MouseEnter
        ShowChildDescs(True, 48)
    End Sub

    Private Sub RemoveDefaultAppAssociations_MouseEnter(sender As Object, e As EventArgs) Handles RemoveDefaultAppAssociations.MouseEnter
        ShowChildDescs(True, 49)
    End Sub

    Private Sub GetIntl_MouseEnter(sender As Object, e As EventArgs) Handles GetIntl.MouseEnter
        ShowChildDescs(True, 50)
    End Sub

    Private Sub SetUILang_MouseEnter(sender As Object, e As EventArgs) Handles SetUILang.MouseEnter
        ShowChildDescs(True, 51)
    End Sub

    Private Sub SetUILangFallback_MouseEnter(sender As Object, e As EventArgs) Handles SetUILangFallback.MouseEnter
        ShowChildDescs(True, 52)
    End Sub

    Private Sub SetSysUILang_MouseEnter(sender As Object, e As EventArgs) Handles SetSysUILang.MouseEnter
        ShowChildDescs(True, 53)
    End Sub

    Private Sub SetSysLocale_MouseEnter(sender As Object, e As EventArgs) Handles SetSysLocale.MouseEnter
        ShowChildDescs(True, 54)
    End Sub

    Private Sub SetUserLocale_MouseEnter(sender As Object, e As EventArgs) Handles SetUserLocale.MouseEnter
        ShowChildDescs(True, 55)
    End Sub

    Private Sub SetInputLocale_MouseEnter(sender As Object, e As EventArgs) Handles SetInputLocale.MouseEnter
        ShowChildDescs(True, 56)
    End Sub

    Private Sub SetAllIntl_MouseEnter(sender As Object, e As EventArgs) Handles SetAllIntl.MouseEnter
        ShowChildDescs(True, 57)
    End Sub

    Private Sub SetTimeZone_MouseEnter(sender As Object, e As EventArgs) Handles SetTimeZone.MouseEnter
        ShowChildDescs(True, 58)
    End Sub

    Private Sub SetSkuIntlDefaults_MouseEnter(sender As Object, e As EventArgs) Handles SetSKUIntlDefaults.MouseEnter
        ShowChildDescs(True, 59)
    End Sub

    Private Sub SetLayeredDriver_MouseEnter(sender As Object, e As EventArgs) Handles SetLayeredDriver.MouseEnter
        ShowChildDescs(True, 60)
    End Sub

    Private Sub GenLangIni_MouseEnter(sender As Object, e As EventArgs) Handles GenLangINI.MouseEnter
        ShowChildDescs(True, 61)
    End Sub

    Private Sub SetSetupUILang_MouseEnter(sender As Object, e As EventArgs) Handles SetSetupUILang.MouseEnter
        ShowChildDescs(True, 62)
    End Sub

    Private Sub AddCapability_MouseEnter(sender As Object, e As EventArgs) Handles AddCapability.MouseEnter
        ShowChildDescs(True, 63)
    End Sub

    Private Sub ExportSource_MouseEnter(sender As Object, e As EventArgs) Handles ExportSource.MouseEnter
        ShowChildDescs(True, 64)
    End Sub

    Private Sub GetCapabilities_MouseEnter(sender As Object, e As EventArgs) Handles GetCapabilities.MouseEnter
        ShowChildDescs(True, 65)
    End Sub

    Private Sub RemoveCapability_MouseEnter(sender As Object, e As EventArgs) Handles RemoveCapability.MouseEnter
        ShowChildDescs(True, 67)
    End Sub

    Private Sub GetCurrentEdition_MouseEnter(sender As Object, e As EventArgs) Handles GetCurrentEdition.MouseEnter
        ShowChildDescs(True, 68)
    End Sub

    Private Sub GetTargetEditions_MouseEnter(sender As Object, e As EventArgs) Handles GetTargetEditions.MouseEnter
        ShowChildDescs(True, 69)
    End Sub

    Private Sub SetEdition_MouseEnter(sender As Object, e As EventArgs) Handles SetEdition.MouseEnter
        ShowChildDescs(True, 70)
    End Sub

    Private Sub SetProductKey_MouseEnter(sender As Object, e As EventArgs) Handles SetProductKey.MouseEnter
        ShowChildDescs(True, 71)
    End Sub

    Private Sub GetDrivers_MouseEnter(sender As Object, e As EventArgs) Handles GetDrivers.MouseEnter
        ShowChildDescs(True, 72)
    End Sub

    Private Sub AddDriver_MouseEnter(sender As Object, e As EventArgs) Handles AddDriver.MouseEnter
        ShowChildDescs(True, 74)
    End Sub

    Private Sub RemoveDriver_MouseEnter(sender As Object, e As EventArgs) Handles RemoveDriver.MouseEnter
        ShowChildDescs(True, 75)
    End Sub

    Private Sub ExportDriver_MouseEnter(sender As Object, e As EventArgs) Handles ExportDriver.MouseEnter
        ShowChildDescs(True, 76)
    End Sub

    Private Sub ApplyUnattend_MouseEnter(sender As Object, e As EventArgs) Handles ApplyUnattend.MouseEnter
        ShowChildDescs(True, 77)
    End Sub

    Private Sub GetPESettings_MouseEnter(sender As Object, e As EventArgs) Handles GetPESettings.MouseEnter
        ShowChildDescs(True, 78)
    End Sub

    Private Sub SetScratchSpace_MouseEnter(sender As Object, e As EventArgs) Handles SetScratchSpace.MouseEnter
        ShowChildDescs(True, 81)
    End Sub

    Private Sub SetTargetPath_MouseEnter(sender As Object, e As EventArgs) Handles SetTargetPath.MouseEnter
        ShowChildDescs(True, 82)
    End Sub

    Private Sub GetOSUninstallWindow_MouseEnter(sender As Object, e As EventArgs) Handles GetOSUninstallWindow.MouseEnter
        ShowChildDescs(True, 83)
    End Sub

    Private Sub InitiateOSUninstall_MouseEnter(sender As Object, e As EventArgs) Handles InitiateOSUninstall.MouseEnter
        ShowChildDescs(True, 84)
    End Sub

    Private Sub RemoveOSUninstall_MouseEnter(sender As Object, e As EventArgs) Handles RemoveOSUninstall.MouseEnter
        ShowChildDescs(True, 85)
    End Sub

    Private Sub SetOSUninstallWindow_MouseEnter(sender As Object, e As EventArgs) Handles SetOSUninstallWindow.MouseEnter
        ShowChildDescs(True, 86)
    End Sub

    Private Sub GetReservedStorageState_MouseEnter(sender As Object, e As EventArgs) Handles GetReservedStorageState.MouseEnter
        ShowChildDescs(True, 87)
    End Sub

    Private Sub SetReservedStorageState_MouseEnter(sender As Object, e As EventArgs) Handles SetReservedStorageState.MouseEnter
        ShowChildDescs(True, 88)
    End Sub

    Private Sub AddEdge_MouseEnter(sender As Object, e As EventArgs) Handles AddEdge.MouseEnter
        ShowChildDescs(True, 89)
    End Sub

    Private Sub AddEdgeBrowser_MouseEnter(sender As Object, e As EventArgs) Handles AddEdgeBrowser.MouseEnter
        ShowChildDescs(True, 90)
    End Sub

    Private Sub AddEdgeWebView_MouseEnter(sender As Object, e As EventArgs) Handles AddEdgeWebView.MouseEnter
        ShowChildDescs(True, 91)
    End Sub

    Private Sub SaveImageInformationToolStripMenuItem_MouseEnter(sender As Object, e As EventArgs) Handles SaveImageInformationToolStripMenuItem.MouseEnter
        ShowChildDescs(True, 92)
    End Sub

    Private Sub NewProject_MouseEnter(sender As Object, e As EventArgs) Handles NewProjectToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 1)
    End Sub

    Private Sub OpenProject_MouseEnter(sender As Object, e As EventArgs) Handles OpenExistingProjectToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 2)
    End Sub

    Private Sub ManageOnlineInstallation_MouseEnter(sender As Object, e As EventArgs) Handles ManageOnlineInstallationToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 3)
    End Sub

    Private Sub SaveProject_MouseEnter(sender As Object, e As EventArgs) Handles SaveProjectToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 4)
    End Sub

    Private Sub SaveProjAs_MouseEnter(sender As Object, e As EventArgs) Handles SaveProjectasToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 5)
    End Sub

    Private Sub ExitProg_MouseEnter(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 6)
    End Sub

    Private Sub ProjectInExplorer_MouseEnter(sender As Object, e As EventArgs) Handles ViewProjectFilesInFileExplorerToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 7)
    End Sub

    Private Sub UnloadProject_MouseEnter(sender As Object, e As EventArgs) Handles UnloadProjectToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 8)
    End Sub

    Private Sub SwitchIndexes_MouseEnter(sender As Object, e As EventArgs) Handles SwitchImageIndexesToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 9)
    End Sub

    Private Sub ProjProps_MouseEnter(sender As Object, e As EventArgs) Handles ProjectPropertiesToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 10)
    End Sub

    Private Sub ImgProps_MouseEnter(sender As Object, e As EventArgs) Handles ImagePropertiesToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 11)
    End Sub

    Private Sub ImgConversion_MouseEnter(sender As Object, e As EventArgs) Handles ImageConversionToolStripMenuItem.MouseEnter, WIMESDToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 12)
    End Sub

    Private Sub MergeSWM_MouseEnter(sender As Object, e As EventArgs) Handles MergeSWM.MouseEnter
        ShowChildDescs(False, 13)
    End Sub

    Private Sub RemountImg_MouseEnter(sender As Object, e As EventArgs) Handles RemountImageWithWritePermissionsToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 14)
    End Sub

    Private Sub CmdConsole_MouseEnter(sender As Object, e As EventArgs) Handles CommandShellToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 15)
    End Sub

    Private Sub UAFileMan_MouseEnter(sender As Object, e As EventArgs) Handles UnattendedAnswerFileManagerToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 16)
    End Sub

    Private Sub ReportManagerToolStripMenuItem_MouseEnter(sender As Object, e As EventArgs) Handles ReportManagerToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 17)
    End Sub

    Private Sub MountedImageManagerTSMI_MouseEnter(sender As Object, e As EventArgs) Handles MountedImageManagerTSMI.MouseEnter
        ShowChildDescs(False, 18)
    End Sub

    Private Sub ProgSettings_MouseEnter(sender As Object, e As EventArgs) Handles OptionsToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 19)
    End Sub

    Private Sub HelpTopics_MouseEnter(sender As Object, e As EventArgs) Handles HelpTopicsToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 20)
    End Sub

    Private Sub Glossary_MouseEnter(sender As Object, e As EventArgs) Handles GlossaryToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 21)
    End Sub

    Private Sub CmdHelp_MouseEnter(sender As Object, e As EventArgs) Handles CommandHelpToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 22)
    End Sub

    Private Sub ProgInfo_MouseEnter(sender As Object, e As EventArgs) Handles AboutDISMToolsToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 23)
    End Sub

    Private Sub ReportFeedbackToolStripMenuItem_MouseEnter(sender As Object, e As EventArgs) Handles ReportFeedbackToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 24)
    End Sub

    Private Sub ContributeToTheHelpSystemToolStripMenuItem_MouseEnter(sender As Object, e As EventArgs) Handles ContributeToTheHelpSystemToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 25)
    End Sub
#End Region

    Private Sub NewProjLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles NewProjLink.LinkClicked
        If Not HomePanel.Visible Then Exit Sub
        NewProj.ShowDialog()
    End Sub

    Private Sub ExistingProjLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles ExistingProjLink.LinkClicked
        If Not HomePanel.Visible Then Exit Sub
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If File.Exists(OpenFileDialog1.FileName) Then
                ProgressPanel.OperationNum = 990
                LoadDTProj(OpenFileDialog1.FileName, Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName), False, False)
            End If
        End If
    End Sub

    Private Sub TabControl2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl2.SelectedIndexChanged
        If TabControl2.SelectedTab.Text = "Actions" Then
            ToolStripButton1.Enabled = False
        Else
            ToolStripButton1.Enabled = True
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        ProgressPanel.OperationNum = 993
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting package names..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting package names..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
        End Select
        If Not CompletedTasks(0) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        If MountedImageDetectorBW.IsBusy Then
            MountedImageDetectorBW.CancelAsync()
            While MountedImageDetectorBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(500)
            End While
        End If
        If PackageInfoList IsNot Nothing Then GetPkgInfoDlg.InstalledPkgInfo = PackageInfoList
        GetPkgInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        ProgressPanel.OperationNum = 994
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
        End Select
        If Not CompletedTasks(1) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(500)
        End While
        If FeatureInfoList IsNot Nothing Then GetFeatureInfoDlg.InstalledFeatureInfo = FeatureInfoList
        GetFeatureInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click, ProjectPropertiesToolStripMenuItem.Click, Button23.Click
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        ProjProperties.TabControl1.SelectedIndex = 0
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        ProjProperties.Label1.Text = ProjProperties.TabControl1.SelectedTab.Text & " properties"
                    Case "ESN"
                        ProjProperties.Label1.Text = "Propiedades " & If(ProjProperties.TabControl1.SelectedIndex = 0, "del proyecto", "de la imagen")
                    Case "FRA"
                        ProjProperties.Label1.Text = "Propriétés " & If(ProjProperties.TabControl1.SelectedIndex = 0, "du projet", "de l'image")
                End Select
            Case 1
                ProjProperties.Label1.Text = ProjProperties.TabControl1.SelectedTab.Text & " properties"
            Case 2
                ProjProperties.Label1.Text = "Propiedades " & If(ProjProperties.TabControl1.SelectedIndex = 0, "del proyecto", "de la imagen")
            Case 3
                ProjProperties.Label1.Text = "Propriétés " & If(ProjProperties.TabControl1.SelectedIndex = 0, "du projet", "de l'image")
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
            ProjProperties.Text = ""
        Else
            ProjProperties.Text = ProjProperties.Label1.Text
        End If
        ProjProperties.ShowDialog()
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles ImagePropertiesToolStripMenuItem.Click, Button15.Click
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        ProjProperties.TabControl1.SelectedIndex = 1
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        ProjProperties.Label1.Text = ProjProperties.TabControl1.SelectedTab.Text & " properties"
                    Case "ESN"
                        ProjProperties.Label1.Text = "Propiedades " & If(ProjProperties.TabControl1.SelectedIndex = 0, "del proyecto", "de la imagen")
                    Case "FRA"
                        ProjProperties.Label1.Text = "Propriétés " & If(ProjProperties.TabControl1.SelectedIndex = 0, "du projet", "de l'image")
                End Select
            Case 1
                ProjProperties.Label1.Text = ProjProperties.TabControl1.SelectedTab.Text & " properties"
            Case 2
                ProjProperties.Label1.Text = "Propiedades " & If(ProjProperties.TabControl1.SelectedIndex = 0, "del proyecto", "de la imagen")
            Case 3
                ProjProperties.Label1.Text = "Propriétés " & If(ProjProperties.TabControl1.SelectedIndex = 0, "du projet", "de l'image")
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
            ProjProperties.Text = ""
        Else
            ProjProperties.Text = ProjProperties.Label1.Text
        End If
        ProjProperties.ShowDialog()
    End Sub

    Private Sub UnloadProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnloadProjectToolStripMenuItem.Click
        SaveProjectQuestionDialog.ShowDialog()
        If SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.Yes Then
            If SaveProjectQuestionDialog.CheckBox1.Checked Then
                UnloadDTProj(False, True, True)
            Else
                UnloadDTProj(False, True, False)
            End If
        ElseIf SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.No Then
            If SaveProjectQuestionDialog.CheckBox1.Checked Then
                UnloadDTProj(False, False, True)
            Else
                UnloadDTProj(False, False, False)
            End If
        ElseIf SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If OnlineManagement Then
            EndOnlineManagement()
            MountedImageDetectorBW.CancelAsync()
            While MountedImageDetectorBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(100)
            End While
            If MountedImgMgr.DetectorBW.IsBusy Then MountedImgMgr.DetectorBW.CancelAsync()
            While MountedImgMgr.DetectorBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(100)
            End While
        End If
        If OfflineManagement Then
            EndOfflineManagement()
            MountedImageDetectorBW.CancelAsync()
            While MountedImageDetectorBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(100)
            End While
            If MountedImgMgr.DetectorBW.IsBusy Then MountedImgMgr.DetectorBW.CancelAsync()
            While MountedImgMgr.DetectorBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(100)
            End While
        End If
        If isProjectLoaded And (Not OnlineManagement Or Not OfflineManagement) Then
            If isModified Then
                SaveProjectQuestionDialog.ShowDialog()
                If SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.Yes Then
                    If SaveProjectQuestionDialog.CheckBox1.Checked Then
                        UnloadDTProj(True, True, True)
                    Else
                        UnloadDTProj(True, True, False)
                    End If
                ElseIf SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.No Then
                    If SaveProjectQuestionDialog.CheckBox1.Checked Then
                        UnloadDTProj(True, False, True)
                    Else
                        UnloadDTProj(True, False, False)
                    End If
                ElseIf SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.Cancel Then
                    e.Cancel = True
                End If
            Else
                UnloadDTProj(True, False, False)
            End If
        End If
        If ImgBW.IsBusy Then
            e.Cancel = True
            Beep()
            Exit Sub
        End If
        If WimScriptEditor.Visible Then
            WimScriptEditor.Close()
            If WimScriptEditor.Visible Then
                e.Cancel = True
                Exit Sub
            End If
        End If
        If Not VolatileMode Then
            SaveDTSettings()
        End If
        MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        If MountedImgMgr.DetectorBW.IsBusy Then MountedImgMgr.DetectorBW.CancelAsync()
        While MountedImgMgr.DetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        SaveDTProj()
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        If OnlineManagement Then
            EndOnlineManagement()
            Exit Sub
        End If
        If OfflineManagement Then
            EndOfflineManagement()
            Exit Sub
        End If
        If isModified Then
            SaveProjectQuestionDialog.ShowDialog()
            If SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.Yes Then
                If SaveProjectQuestionDialog.CheckBox1.Checked Then
                    UnloadDTProj(False, True, True)
                Else
                    UnloadDTProj(False, True, False)
                End If
            ElseIf SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.No Then
                If SaveProjectQuestionDialog.CheckBox1.Checked Then
                    UnloadDTProj(False, False, True)
                Else
                    UnloadDTProj(False, False, False)
                End If
            ElseIf SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
        Else
            UnloadDTProj(False, False, False)
        End If
    End Sub

    Private Sub CommandShellToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CommandShellToolStripMenuItem.Click
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe", "/k " & Quote & Application.StartupPath & "\bin\dthelper.bat" & Quote & " /sh")
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        ' If it's a read only image, directly unmount it discarding changes
        If MountedImageImgFiles.Count > 0 Then
            For x = 0 To Array.LastIndexOf(MountedImageImgFiles, MountedImageImgFiles.Last)
                If MountedImageMountDirs(x) = MountDir Then
                    If MountedImageMountedReWr(x) = 1 Then
                        Button4.PerformClick()
                        Exit Sub
                    End If
                End If
            Next
        End If
        ImgUMount.RadioButton1.Checked = True
        ImgUMount.RadioButton2.Checked = False
        ImgUMount.ShowDialog()
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OptionsToolStripMenuItem.Click
        Options.PrefReset.Enabled = True
        Options.ShowDialog()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        ImgMount.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MountDir
        ' TODO: Add additional options later
        ProgressPanel.OperationNum = 8
        ProgressPanel.ShowDialog(Me)
    End Sub

    Private Sub ExplorerView_Click(sender As Object, e As EventArgs) Handles ExplorerView.Click, Button22.Click
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", projPath)
    End Sub

    Private Sub GetImageInfo_Click(sender As Object, e As EventArgs) Handles GetImageInfo.Click
        If ImgBW.IsBusy Then
            BGProcsBusyDialog.ShowDialog()
            Exit Sub
        End If
        GetImgInfoDlg.ShowDialog()
    End Sub

    Private Sub prjTreeView_AfterExpand(sender As Object, e As TreeViewEventArgs) Handles prjTreeView.AfterExpand
        Try
            If prjTreeView.SelectedNode.IsExpanded Then
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                ExpandCollapseTSB.Text = "Collapse"
                                ExpandToolStripMenuItem.Text = "Collapse item"
                            Case "ESN"
                                ExpandCollapseTSB.Text = "Contraer"
                                ExpandToolStripMenuItem.Text = "Contraer objeto"
                            Case "FRA"
                                ExpandCollapseTSB.Text = "Réduire"
                                ExpandToolStripMenuItem.Text = "Réduire élément"
                        End Select
                    Case 1
                        ExpandCollapseTSB.Text = "Collapse"
                        ExpandToolStripMenuItem.Text = "Collapse item"
                    Case 2
                        ExpandCollapseTSB.Text = "Contraer"
                        ExpandToolStripMenuItem.Text = "Contraer objeto"
                    Case 3
                        ExpandCollapseTSB.Text = "Réduire"
                        ExpandToolStripMenuItem.Text = "Réduire élément"
                End Select
                If BackColor = Color.FromArgb(48, 48, 48) Then
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph_dark)
                ElseIf BackColor = Color.White Then
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph)
                End If
            Else
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                ExpandCollapseTSB.Text = "Expand"
                                ExpandToolStripMenuItem.Text = "Expand item"
                            Case "ESN"
                                ExpandCollapseTSB.Text = "Expandir"
                                ExpandToolStripMenuItem.Text = "Expandir objeto"
                            Case "FRA"
                                ExpandCollapseTSB.Text = "Agrandir"
                                ExpandToolStripMenuItem.Text = "Agrandir élément"
                        End Select
                    Case 1
                        ExpandCollapseTSB.Text = "Expand"
                        ExpandToolStripMenuItem.Text = "Expand item"
                    Case 2
                        ExpandCollapseTSB.Text = "Expandir"
                        ExpandToolStripMenuItem.Text = "Expandir objeto"
                    Case 3
                        ExpandCollapseTSB.Text = "Agrandir"
                        ExpandToolStripMenuItem.Text = "Agrandir élément"
                End Select
                If BackColor = Color.FromArgb(48, 48, 48) Then
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph_dark)
                ElseIf BackColor = Color.White Then
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph)
                End If
            End If
        Catch ex As Exception
            ExpandCollapseTSB.Enabled = False
            Exit Sub
        End Try
    End Sub

    Private Sub prjTreeView_AfterCollapse(sender As Object, e As TreeViewEventArgs) Handles prjTreeView.AfterCollapse
        Try
            If prjTreeView.SelectedNode.IsExpanded Then
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                ExpandCollapseTSB.Text = "Collapse"
                                ExpandToolStripMenuItem.Text = "Collapse item"
                            Case "ESN"
                                ExpandCollapseTSB.Text = "Contraer"
                                ExpandToolStripMenuItem.Text = "Contraer objeto"
                            Case "FRA"
                                ExpandCollapseTSB.Text = "Réduire"
                                ExpandToolStripMenuItem.Text = "Réduire élément"
                        End Select
                    Case 1
                        ExpandCollapseTSB.Text = "Collapse"
                        ExpandToolStripMenuItem.Text = "Collapse item"
                    Case 2
                        ExpandCollapseTSB.Text = "Contraer"
                        ExpandToolStripMenuItem.Text = "Contraer objeto"
                    Case 3
                        ExpandCollapseTSB.Text = "Réduire"
                        ExpandToolStripMenuItem.Text = "Réduire élément"
                End Select
                If BackColor = Color.FromArgb(48, 48, 48) Then
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph_dark)
                ElseIf BackColor = Color.White Then
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph)
                End If
            Else
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                ExpandCollapseTSB.Text = "Expand"
                                ExpandToolStripMenuItem.Text = "Expand item"
                            Case "ESN"
                                ExpandCollapseTSB.Text = "Expandir"
                                ExpandToolStripMenuItem.Text = "Expandir objeto"
                            Case "FRA"
                                ExpandCollapseTSB.Text = "Agrandir"
                                ExpandToolStripMenuItem.Text = "Agrandir élément"
                        End Select
                    Case 1
                        ExpandCollapseTSB.Text = "Expand"
                        ExpandToolStripMenuItem.Text = "Expand item"
                    Case 2
                        ExpandCollapseTSB.Text = "Expandir"
                        ExpandToolStripMenuItem.Text = "Expandir objeto"
                    Case 3
                        ExpandCollapseTSB.Text = "Agrandir"
                        ExpandToolStripMenuItem.Text = "Agrandir élément"
                End Select
                If BackColor = Color.FromArgb(48, 48, 48) Then
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph_dark)
                ElseIf BackColor = Color.White Then
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph)
                End If
            End If
        Catch ex As Exception
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            ExpandCollapseTSB.Text = "Expand"
                            ExpandToolStripMenuItem.Text = "Expand item"
                        Case "ESN"
                            ExpandCollapseTSB.Text = "Expandir"
                            ExpandToolStripMenuItem.Text = "Expandir objeto"
                        Case "FRA"
                            ExpandCollapseTSB.Text = "Agrandir"
                            ExpandToolStripMenuItem.Text = "Agrandir élément"
                    End Select
                Case 1
                    ExpandCollapseTSB.Text = "Expand"
                    ExpandToolStripMenuItem.Text = "Expand item"
                Case 2
                    ExpandCollapseTSB.Text = "Expandir"
                    ExpandToolStripMenuItem.Text = "Expandir objeto"
                Case 3
                    ExpandCollapseTSB.Text = "Agrandir"
                    ExpandToolStripMenuItem.Text = "Agrandir élément"
            End Select
            If BackColor = Color.FromArgb(48, 48, 48) Then
                ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph_dark)
            ElseIf BackColor = Color.White Then
                ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph)
            End If
        End Try
    End Sub

    Private Sub prjTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles prjTreeView.AfterSelect
        If prjTreeView.SelectedNode.IsExpanded Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            ExpandCollapseTSB.Text = "Collapse"
                            ExpandToolStripMenuItem.Text = "Collapse item"
                        Case "ESN"
                            ExpandCollapseTSB.Text = "Contraer"
                            ExpandToolStripMenuItem.Text = "Contraer objeto"
                        Case "FRA"
                            ExpandCollapseTSB.Text = "Réduire"
                            ExpandToolStripMenuItem.Text = "Réduire élément"
                    End Select
                Case 1
                    ExpandCollapseTSB.Text = "Collapse"
                    ExpandToolStripMenuItem.Text = "Collapse item"
                Case 2
                    ExpandCollapseTSB.Text = "Contraer"
                    ExpandToolStripMenuItem.Text = "Contraer objeto"
                Case 3
                    ExpandCollapseTSB.Text = "Réduire"
                    ExpandToolStripMenuItem.Text = "Réduire élément"
            End Select
            If BackColor = Color.FromArgb(48, 48, 48) Then
                ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph_dark)
            ElseIf BackColor = Color.White Then
                ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph)
            End If
        Else
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            ExpandCollapseTSB.Text = "Expand"
                            ExpandToolStripMenuItem.Text = "Expand item"
                        Case "ESN"
                            ExpandCollapseTSB.Text = "Expandir"
                            ExpandToolStripMenuItem.Text = "Expandir objeto"
                        Case "FRA"
                            ExpandCollapseTSB.Text = "Agrandir"
                            ExpandToolStripMenuItem.Text = "Agrandir élément"
                    End Select
                Case 1
                    ExpandCollapseTSB.Text = "Expand"
                    ExpandToolStripMenuItem.Text = "Expand item"
                Case 2
                    ExpandCollapseTSB.Text = "Expandir"
                    ExpandToolStripMenuItem.Text = "Expandir objeto"
                Case 3
                    ExpandCollapseTSB.Text = "Agrandir"
                    ExpandToolStripMenuItem.Text = "Agrandir élément"
            End Select
            If BackColor = Color.FromArgb(48, 48, 48) Then
                ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph_dark)
            ElseIf BackColor = Color.White Then
                ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph)
            End If
        End If
        If prjTreeView.SelectedNode.Nodes.Count = 0 Then
            ExpandCollapseTSB.Enabled = False
            ExpandToolStripMenuItem.Enabled = False
        Else
            ExpandCollapseTSB.Enabled = True
            ExpandToolStripMenuItem.Enabled = True
        End If
    End Sub

    Private Sub ExpandCollapseTSB_Click(sender As Object, e As EventArgs) Handles ExpandCollapseTSB.Click
        If ExpandCollapseTSB.Text = "Expand" Or ExpandCollapseTSB.Text = "Expandir" Or ExpandCollapseTSB.Text = "Agrandir" Then
            Try
                prjTreeView.SelectedNode.Expand()
            Catch ex As Exception

            End Try
        ElseIf ExpandCollapseTSB.Text = "Collapse" Or ExpandCollapseTSB.Text = "Contraer" Or ExpandCollapseTSB.Text = "Réduire" Then
            Try
                prjTreeView.SelectedNode.Collapse()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub AddPackage_Click(sender As Object, e As EventArgs) Handles AddPackage.Click
        AddPackageDlg.ShowDialog()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        AddPackageDlg.ShowDialog()
    End Sub

    Private Sub AboutDISMToolsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutDISMToolsToolStripMenuItem.Click, VersionTSMI.Click
        PrgAbout.ShowDialog()
    End Sub

    Private Sub WIMESDToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WIMESDToolStripMenuItem.Click
        ImgWim2Esd.ShowDialog()
    End Sub

    Private Sub UnattendedAnswerFileManagerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnattendedAnswerFileManagerToolStripMenuItem.Click
        UnattendMgr.Show()
    End Sub

    Private Sub CaptureImage_Click(sender As Object, e As EventArgs) Handles CaptureImage.Click
        ImgCapture.ShowDialog()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MountDir
        ProgressPanel.OperationNum = 18
        ProgressPanel.ShowDialog(Me)
    End Sub

    Private Sub MergeSWM_Click(sender As Object, e As EventArgs) Handles MergeSWM.Click
        ImgSwmToWim.ShowDialog()
    End Sub

    Private Sub ApplyImage_Click(sender As Object, e As EventArgs) Handles ApplyImage.Click
        ImgApply.ShowDialog()
    End Sub

    Private Sub SaveProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveProjectToolStripMenuItem.Click
        SaveDTProj()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        ElementCount = 0
        RemPackage.CheckedListBox1.Items.Clear()
        ProgressPanel.OperationNum = 993
        PleaseWaitDialog.pkgSourceImgStr = MountDir
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting package names..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting package names..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
        End Select
        If Not CompletedTasks(0) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Try
            For x = 0 To Array.LastIndexOf(imgPackageNames, imgPackageNames.Last)
                If imgPackageNames(x) = "" Then
                    Continue For
                End If
                RemPackage.CheckedListBox1.Items.Add(imgPackageNames(x))
            Next
        Catch ex As Exception
            ' We should have enough with the entries already added.
            Exit Try
        End Try
        Try
            For x = 0 To Array.LastIndexOf(imgPackageNames, imgPackageNames.Last)
                If imgPackageNames(x) = "" Then
                    Exit For
                End If
                ElementCount += 1
            Next
        Catch ex As Exception
            ' We should have enough with the entries already added.
            Exit Try
        End Try
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        RemPackage.Label2.Text = "This image contains " & ElementCount & " packages"
                    Case "ESN"
                        RemPackage.Label2.Text = "Esta imagen contiene " & ElementCount & " paquetes"
                    Case "FRA"
                        RemPackage.Label2.Text = "Cette image contient " & ElementCount & " paquets"
                End Select
            Case 1
                RemPackage.Label2.Text = "This image contains " & ElementCount & " packages"
            Case 2
                RemPackage.Label2.Text = "Esta imagen contiene " & ElementCount & " paquetes"
            Case 3
                RemPackage.Label2.Text = "Cette image contient " & ElementCount & " paquets"
        End Select
        RemPackage.ShowDialog()
    End Sub

    Private Sub ImgBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ImgBW.DoWork
        MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        If bwAllBackgroundProcesses Then
            If bwGetImageInfo Then
                If bwGetAdvImgInfo Then
                    RunBackgroundProcesses(bwBackgroundProcessAction, True, True, True, OnlineManagement, OfflineManagement)
                Else
                    RunBackgroundProcesses(bwBackgroundProcessAction, True, False, True, OnlineManagement, OfflineManagement)
                End If
            Else
                RunBackgroundProcesses(bwBackgroundProcessAction, False, False, True, OnlineManagement, OfflineManagement)
            End If
        Else

        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        ElementCount = 0
        EnableFeat.ListView1.Items.Clear()
        DisableFeat.ListView1.Items.Clear()
        ProgressPanel.OperationNum = 994
        PleaseWaitDialog.featOpType = 0
        PleaseWaitDialog.featSourceImg = MountDir
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
        End Select
        If Not CompletedTasks(1) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Select Case PleaseWaitDialog.featOpType
            Case 0
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureState(x).Contains("Enable") Or imgFeatureState(x) = "" Or imgFeatureState(x) = "Nothing" Then
                            Continue For
                        End If
                        EnableFeat.ListView1.Items.Add(imgFeatureNames(x)).SubItems.Add(imgFeatureState(x))
                    Next
                Catch ex As Exception
                    ' We should have enough with the entries already added.
                    Exit Try
                End Try
                ' Get number of available elements
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureNames(x) = "" Then
                            Exit For
                        End If
                        ElementCount += 1
                    Next
                Catch ex As Exception
                    Exit Try
                End Try
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                EnableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                            Case "ESN"
                                EnableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                            Case "FRA"
                                EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                        End Select
                    Case 1
                        EnableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                    Case 2
                        EnableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                    Case 3
                        EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                End Select
            Case 1
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureState(x).Contains("Disable") Or imgFeatureState(x) = "" Or imgFeatureState(x) = "Nothing" Then
                            Continue For
                        End If
                        DisableFeat.ListView1.Items.Add(imgFeatureNames(x)).SubItems.Add(imgFeatureState(x))
                    Next
                Catch ex As Exception
                    ' We should have enough with the entries already added.
                    Exit Try
                End Try
                ' Get number of available elements
                Dim ElementCount As Integer
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureNames(x) = "" Then
                            Exit For
                        End If
                        ElementCount += 1
                    Next
                Catch ex As Exception
                    Exit Try
                End Try
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                DisableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                            Case "ESN"
                                DisableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                            Case "FRA"
                                EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                        End Select
                    Case 1
                        DisableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                    Case 2
                        DisableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                    Case 3
                        EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                End Select
        End Select
        EnableFeat.ShowDialog()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        ElementCount = 0
        EnableFeat.ListView1.Items.Clear()
        DisableFeat.ListView1.Items.Clear()
        ProgressPanel.OperationNum = 994
        PleaseWaitDialog.featOpType = 1
        PleaseWaitDialog.featSourceImg = MountDir
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
        End Select
        If Not CompletedTasks(1) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Select Case PleaseWaitDialog.featOpType
            Case 0
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureState(x).Contains("Enable") Or imgFeatureState(x) = "" Or imgFeatureState(x) = "Nothing" Then
                            Continue For
                        End If
                        EnableFeat.ListView1.Items.Add(imgFeatureNames(x)).SubItems.Add(imgFeatureState(x))
                    Next
                Catch ex As Exception
                    ' We should have enough with the entries already added.
                    Exit Try
                End Try
                ' Get number of available elements
                Dim ElementCount As Integer = 0
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureNames(x) = "" Then
                            Exit For
                        End If
                        ElementCount += 1
                    Next
                Catch ex As Exception
                    Exit Try
                End Try
                EnableFeat.Label2.Text = "This image contains " & ElementCount & " features."
            Case 1
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureState(x).Contains("Disable") Or imgFeatureState(x) = "" Or imgFeatureState(x) = "Nothing" Or imgFeatureState(x) = "Removed" Then
                            Continue For
                        End If
                        DisableFeat.ListView1.Items.Add(imgFeatureNames(x)).SubItems.Add(imgFeatureState(x))
                    Next
                Catch ex As Exception
                    ' We should have enough with the entries already added.
                    Exit Try
                End Try
                ' Get number of available elements
                Dim ElementCount As Integer
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureNames(x) = "" Then
                            Exit For
                        End If
                        ElementCount += 1
                    Next
                Catch ex As Exception
                    Exit Try
                End Try
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                DisableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                            Case "ESN"
                                DisableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                            Case "FRA"
                                EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                        End Select
                    Case 1
                        DisableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                    Case 2
                        DisableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                    Case 3
                        EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                End Select
        End Select
        DisableFeat.ShowDialog()
    End Sub

    Private Sub SplitPanels_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles SplitPanels.SplitterMoved
        If SplitPanels.SplitterDistance >= 384 And GroupBox1.Left >= 0 Then
            SplitPanels.SplitterDistance = 384
        ElseIf GroupBox1.Left < 0 Then
            SplitPanels.SplitterDistance = 300
        End If
    End Sub

    Private Sub MainForm_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If WindowState <> FormWindowState.Maximized Then
            WndWidth = Width
            WndHeight = Height
        End If
        If Visible And ColorMode = 0 Then
            ChangePrgColors(0)
        End If
        If GroupBox1.Left < 0 Then
            SplitPanels.SplitterDistance = 264
        End If
        If BGProcNotify.Visible Then
            If Environment.OSVersion.Version.Major = 10 Then    ' The Left property also includes the window shadows on Windows 10 and 11
                BGProcNotify.Location = New Point(Left + 8, Top + StatusStrip.Top - (7 + StatusStrip.Height))
            ElseIf Environment.OSVersion.Version.Major = 6 Then
                If Environment.OSVersion.Version.Minor = 1 Then ' The same also applies to Windows 7
                    BGProcNotify.Location = New Point(Left + 8, Top + StatusStrip.Top - (7 + StatusStrip.Height))
                Else
                    BGProcNotify.Location = New Point(Left, Top + StatusStrip.Top - StatusStrip.Height)
                End If
            End If
        ElseIf BGProcDetails.Visible And pinState = 0 Then
            If Environment.OSVersion.Version.Major = 10 Then    ' The Left property also includes the window shadows on Windows 10 and 11
                BGProcDetails.Location = New Point(Left + 8, Top + StatusStrip.Top - (75 + StatusStrip.Height))
            ElseIf Environment.OSVersion.Version.Major = 6 Then
                If Environment.OSVersion.Version.Minor = 1 Then ' The same also applies to Windows 7
                    BGProcDetails.Location = New Point(Left + 8, Top + StatusStrip.Top - (75 + StatusStrip.Height))
                Else
                    BGProcDetails.Location = New Point(Left, Top + StatusStrip.Top - StatusStrip.Height)
                End If
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        imgCommitOperation = 0
        UnloadDTProj(False, True, True)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        imgCommitOperation = 1
        UnloadDTProj(False, True, True)
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        MountedImageDetectorBW.CancelAsync()
        ProgressPanel.OperationNum = 995
        PleaseWaitDialog.indexesSourceImg = SourceImg
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting image indexes..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo índices de la imagen..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des index de l'image en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting image indexes..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo índices de la imagen..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des index de l'image en cours..."
        End Select
        PleaseWaitDialog.ShowDialog(Me)
        If Not MountedImageDetectorBW.IsBusy Then Call MountedImageDetectorBW.RunWorkerAsync()
        If PleaseWaitDialog.imgIndexes > 1 Then
            ImgIndexSwitch.ShowDialog()
        End If
    End Sub

    Private Sub UnloadBtn_Click(sender As Object, e As EventArgs) Handles UnloadBtn.Click, Button21.Click
        ToolStripButton3.PerformClick()
    End Sub

    Private Sub MainForm_Move(sender As Object, e As EventArgs) Handles MyBase.Move
        If WindowState <> FormWindowState.Maximized Then
            WndLeft = Left
            WndTop = Top
        End If
        If BGProcNotify.Visible Then
            If Environment.OSVersion.Version.Major = 10 Then    ' The Left property also includes the window shadows on Windows 10 and 11
                BGProcNotify.Location = New Point(Left + 8, Top + StatusStrip.Top - (7 + StatusStrip.Height))
            ElseIf Environment.OSVersion.Version.Major = 6 Then
                If Environment.OSVersion.Version.Minor = 1 Then ' The same also applies to Windows 7
                    BGProcNotify.Location = New Point(Left + 8, Top + StatusStrip.Top - (7 + StatusStrip.Height))
                Else
                    BGProcNotify.Location = New Point(Left, Top + StatusStrip.Top - StatusStrip.Height)
                End If
            End If
        ElseIf BGProcDetails.Visible And pinState = 0 Then
            If Environment.OSVersion.Version.Major = 10 Then    ' The Left property also includes the window shadows on Windows 10 and 11
                BGProcDetails.Location = New Point(Left + 8, Top + StatusStrip.Top - (75 + StatusStrip.Height))
            ElseIf Environment.OSVersion.Version.Major = 6 Then
                If Environment.OSVersion.Version.Minor = 1 Then ' The same also applies to Windows 7
                    BGProcDetails.Location = New Point(Left + 8, Top + StatusStrip.Top - (75 + StatusStrip.Height))
                Else
                    BGProcDetails.Location = New Point(Left, Top + StatusStrip.Top - StatusStrip.Height)
                End If
            End If
        End If
    End Sub

    Private Sub BackgroundProcessesButton_Click(sender As Object, e As EventArgs) Handles BackgroundProcessesButton.Click
        If BGProcDetails.Visible Then
            BGProcDetails.Hide()
        Else
            BGProcDetails.Show()
        End If
    End Sub

    Private Sub ImgBW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles ImgBW.ProgressChanged
        BGProcDetails.Label2.Text = progressLabel
        If bwBackgroundProcessAction <> 0 Then BGProcDetails.ProgressBar1.Style = ProgressBarStyle.Marquee Else BGProcDetails.ProgressBar1.Style = ProgressBarStyle.Blocks
        If regJumps Then
            BGProcDetails.ProgressBar1.Value = e.ProgressPercentage
        Else
            BGProcDetails.ProgressBar1.Value = irregVal
        End If
        progressMin = BGProcDetails.ProgressBar1.Value
    End Sub

    Private Sub ImgBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ImgBW.RunWorkerCompleted
        CompletedTasks = Enumerable.Repeat(True, CompletedTasks.Length).ToArray()
        BGProcDetails.ProgressBar1.Style = ProgressBarStyle.Blocks
        If Not MountedImageDetectorBW.IsBusy Then Call MountedImageDetectorBW.RunWorkerAsync()
        areBackgroundProcessesDone = True
        BackgroundProcessesButton.Image = New Bitmap(My.Resources.bg_ops_complete)
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        progressLabel = "Image processes have completed"
                    Case "ESN"
                        progressLabel = "Los procesos de la imagen han completado"
                    Case "FRA"
                        progressLabel = "Les processus de l'image sont terminés"
                End Select
            Case 1
                progressLabel = "Image processes have completed"
            Case 2
                progressLabel = "Los procesos de la imagen han completado"
            Case 3
                progressLabel = "Les processus de l'image sont terminés"
        End Select
        BGProcDetails.Label2.Text = progressLabel
        BGProcDetails.ProgressBar1.Value = BGProcDetails.ProgressBar1.Maximum
        If Not ProgressPanel.IsDisposed And Not ProgressPanel.Visible Then ProgressPanel.Dispose()
        If isOrphaned Then
            If BGProcDetails.Visible Then
                BGProcDetails.ProgressBar1.Value = 0
            End If
            OrphanedMountedImgDialog.ShowDialog(Me)
            If OrphanedMountedImgDialog.DialogResult = Windows.Forms.DialogResult.OK Then
                ProgressPanel.Validate()
                ProgressPanel.MountDir = MountDir
                ProgressPanel.OperationNum = 18
                ProgressPanel.ShowDialog(Me)
            ElseIf OrphanedMountedImgDialog.DialogResult = Windows.Forms.DialogResult.Cancel Then
                UnloadDTProj(False, False, False)
                ImgBW.CancelAsync()
            End If
        End If
        If Not IsCompatible Then
            If SysVer.Major = 6 And SysVer.Build >= 6000 Then
                If SysVer.Minor = 0 Then        ' Windows Vista / WinPE 2.x
                    ' Let the user know about the incompatibility
                    If Not ProgressPanel.IsDisposed Then
                        ToolStripButton4.Visible = False
                        ProgressPanel.Dispose()
                        ProgressPanel.Close()
                    End If
                    ImgWinVistaIncompatibilityDialog.ShowDialog(Me)
                    If ImgWinVistaIncompatibilityDialog.DialogResult = Windows.Forms.DialogResult.OK Then
                        ' Disable every option
                        Button1.Enabled = False
                        Button2.Enabled = False
                        Button3.Enabled = False
                        Button4.Enabled = True
                        Button5.Enabled = False
                        Button6.Enabled = False
                        Button7.Enabled = False
                        Button8.Enabled = False
                        Button9.Enabled = False
                        Button10.Enabled = False
                        Button11.Enabled = False
                        Button12.Enabled = False
                        Button13.Enabled = False
                        ' Update the buttons in the new design accordingly
                        Button26.Enabled = False
                        Button27.Enabled = False
                        Button28.Enabled = False
                        Button29.Enabled = True
                        Button24.Enabled = False
                        Button25.Enabled = False
                        Button30.Enabled = False
                        Button31.Enabled = False
                        Button32.Enabled = False
                        Button33.Enabled = False
                        Button34.Enabled = False
                        Button35.Enabled = False
                        Button36.Enabled = False
                        Button37.Enabled = False
                        Button38.Enabled = False
                        Button39.Enabled = False
                        Button40.Enabled = False
                        Button41.Enabled = False
                        Button42.Enabled = False
                        Button43.Enabled = False
                        Button44.Enabled = False
                        Button45.Enabled = False
                        Button46.Enabled = False
                        Button47.Enabled = False
                        Button48.Enabled = False
                        Button49.Enabled = False
                        Button50.Enabled = False
                        Button51.Enabled = False
                        Button52.Enabled = False
                        Button53.Enabled = False
                        Button54.Enabled = False
                        Button55.Enabled = False
                        Button56.Enabled = False
                        Button57.Enabled = False
                        Button58.Enabled = False
                        Exit Sub
                    ElseIf ImgWinVistaIncompatibilityDialog.DialogResult = Windows.Forms.DialogResult.Cancel Then
                        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
                        ' Unmount the image
                        ProgressPanel.UMountLocalDir = True
                        ProgressPanel.RandomMountDir = ""   ' Hope there isn't anything to set here
                        ProgressPanel.MountDir = MountDir
                        ProgressPanel.UMountOp = 1
                        ProgressPanel.CheckImgIntegrity = False
                        ProgressPanel.SaveToNewIndex = False
                        ProgressPanel.UMountImgIndex = ImgIndex
                        ProgressPanel.OperationNum = 21
                        ProgressPanel.ShowDialog()
                    End If
                ElseIf SysVer.Minor = 1 Then    ' Windows 7 / WinPE 3.x

                ElseIf SysVer.Minor = 2 Then    ' Windows 8 / WinPE 4.0

                ElseIf SysVer.Minor = 3 Then    ' Windows 8.1 / WinPE 5.x

                ElseIf SysVer.Minor = 4 Then    ' Windows 10 (Technical Preview)

                End If

            ElseIf SysVer.Major = 10 Then
                Select Case SysVer.Build
                    Case 9888 To 21390                          ' Windows 10 / Server 2016,2019,2022 / Cobalt_SunValley / Win10X / WinPE 10.0

                    Case Is >= 21996                            ' Windows 11 / Cobalt_Refresh / Nickel / Copper / WinPE 10.0

                End Select
            ElseIf SysVer.Major < 6 Or (SysVer.Major = 6 And SysVer.Build < 6000) Then
                If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
                ' Windows XP/Server 2003 or older WIM files created by XP2ESD or other XP -> WIM projects. Directly unmount it
                ProgressPanel.UMountLocalDir = True
                ProgressPanel.RandomMountDir = ""   ' Hope there isn't anything to set here
                ProgressPanel.MountDir = MountDir
                ProgressPanel.UMountOp = 1
                ProgressPanel.CheckImgIntegrity = False
                ProgressPanel.SaveToNewIndex = False
                ProgressPanel.UMountImgIndex = ImgIndex
                ProgressPanel.OperationNum = 21
                ProgressPanel.ShowDialog()
            End If
        End If
    End Sub

    Private Sub RemovePackage_Click(sender As Object, e As EventArgs) Handles RemovePackage.Click
        ElementCount = 0
        RemPackage.CheckedListBox1.Items.Clear()
        ProgressPanel.OperationNum = 993
        PleaseWaitDialog.pkgSourceImgStr = MountDir
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting package names..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting package names..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
        End Select
        If Not CompletedTasks(0) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Try
            For x = 0 To Array.LastIndexOf(imgPackageNames, imgPackageNames.Last)
                If imgPackageNames(x) = "" Then
                    Continue For
                End If
                RemPackage.CheckedListBox1.Items.Add(imgPackageNames(x))
            Next
        Catch ex As Exception
            ' We should have enough with the entries already added.
            Exit Try
        End Try
        Try
            For x = 0 To Array.LastIndexOf(imgPackageNames, imgPackageNames.Last)
                If imgPackageNames(x) = "" Then
                    Exit For
                End If
                ElementCount += 1
            Next
        Catch ex As Exception
            ' We should have enough with the entries already added.
            Exit Try
        End Try
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        RemPackage.Label2.Text = "This image contains " & ElementCount & " packages"
                    Case "ESN"
                        RemPackage.Label2.Text = "Esta imagen contiene " & ElementCount & " paquetes"
                    Case "FRA"
                        RemPackage.Label2.Text = "Cette image contient " & ElementCount & " paquets"
                End Select
            Case 1
                RemPackage.Label2.Text = "This image contains " & ElementCount & " packages"
            Case 2
                RemPackage.Label2.Text = "Esta imagen contiene " & ElementCount & " paquetes"
            Case 3
                RemPackage.Label2.Text = "Cette image contient " & ElementCount & " paquets"
        End Select
        RemPackage.ShowDialog()
    End Sub

    Private Sub EnableFeature_Click(sender As Object, e As EventArgs) Handles EnableFeature.Click
        ElementCount = 0
        EnableFeat.ListView1.Items.Clear()
        DisableFeat.ListView1.Items.Clear()
        ProgressPanel.OperationNum = 994
        PleaseWaitDialog.featOpType = 0
        PleaseWaitDialog.featSourceImg = MountDir
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
        End Select
        If Not CompletedTasks(1) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Select Case PleaseWaitDialog.featOpType
            Case 0
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureState(x).Contains("Enable") Or imgFeatureState(x) = "" Or imgFeatureState(x) = "Nothing" Then
                            Continue For
                        End If
                        EnableFeat.ListView1.Items.Add(imgFeatureNames(x)).SubItems.Add(imgFeatureState(x))
                    Next
                Catch ex As Exception
                    ' We should have enough with the entries already added.
                    Exit Try
                End Try
                ' Get number of available elements
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureNames(x) = "" Then
                            Exit For
                        End If
                        ElementCount += 1
                    Next
                Catch ex As Exception
                    Exit Try
                End Try
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                EnableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                            Case "ESN"
                                EnableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                            Case "FRA"
                                EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                        End Select
                    Case 1
                        EnableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                    Case 2
                        EnableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                    Case 3
                        EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                End Select
            Case 1
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureState(x).Contains("Disable") Or imgFeatureState(x) = "" Or imgFeatureState(x) = "Nothing" Then
                            Continue For
                        End If
                        DisableFeat.ListView1.Items.Add(imgFeatureNames(x)).SubItems.Add(imgFeatureState(x))
                    Next
                Catch ex As Exception
                    ' We should have enough with the entries already added.
                    Exit Try
                End Try
                ' Get number of available elements
                Dim ElementCount As Integer
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureNames(x) = "" Then
                            Exit For
                        End If
                        ElementCount += 1
                    Next
                Catch ex As Exception
                    Exit Try
                End Try
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                DisableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                            Case "ESN"
                                DisableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                            Case "FRA"
                                EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                        End Select
                    Case 1
                        DisableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                    Case 2
                        DisableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                    Case 3
                        EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                End Select
        End Select
        EnableFeat.ShowDialog()
    End Sub

    Private Sub DisableFeature_Click(sender As Object, e As EventArgs) Handles DisableFeature.Click
        ElementCount = 0
        EnableFeat.ListView1.Items.Clear()
        DisableFeat.ListView1.Items.Clear()
        ProgressPanel.OperationNum = 994
        PleaseWaitDialog.featOpType = 1
        PleaseWaitDialog.featSourceImg = MountDir
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
        End Select
        If Not CompletedTasks(1) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Select Case PleaseWaitDialog.featOpType
            Case 0
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureState(x).Contains("Enable") Or imgFeatureState(x) = "" Or imgFeatureState(x) = "Nothing" Then
                            Continue For
                        End If
                        EnableFeat.ListView1.Items.Add(imgFeatureNames(x)).SubItems.Add(imgFeatureState(x))
                    Next
                Catch ex As Exception
                    ' We should have enough with the entries already added.
                    Exit Try
                End Try
                ' Get number of available elements
                Dim ElementCount As Integer = 0
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureNames(x) = "" Then
                            Exit For
                        End If
                        ElementCount += 1
                    Next
                Catch ex As Exception
                    Exit Try
                End Try
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                EnableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                            Case "ESN"
                                EnableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                            Case "FRA"
                                EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                        End Select
                    Case 1
                        EnableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                    Case 2
                        EnableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                    Case 3
                        EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                End Select
            Case 1
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureState(x).Contains("Disable") Or imgFeatureState(x) = "" Or imgFeatureState(x) = "Nothing" Then
                            Continue For
                        End If
                        DisableFeat.ListView1.Items.Add(imgFeatureNames(x)).SubItems.Add(imgFeatureState(x))
                    Next
                Catch ex As Exception
                    ' We should have enough with the entries already added.
                    Exit Try
                End Try
                ' Get number of available elements
                Dim ElementCount As Integer
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureNames(x) = "" Then
                            Exit For
                        End If
                        ElementCount += 1
                    Next
                Catch ex As Exception
                    Exit Try
                End Try
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                DisableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                            Case "ESN"
                                DisableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                            Case "FRA"
                                EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                        End Select
                    Case 1
                        DisableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                    Case 2
                        DisableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                    Case 3
                        EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                End Select
        End Select
        DisableFeat.ShowDialog()
    End Sub

    Private Sub AddProvisionedAppxPackage_Click(sender As Object, e As EventArgs) Handles AddProvisionedAppxPackage.Click
        If Not imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) And IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            AddProvAppxPackage.ShowDialog()
        Else
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
            End Select
        End If
    End Sub

    Private Sub RemoveProvisionedAppxPackage_Click(sender As Object, e As EventArgs) Handles RemoveProvisionedAppxPackage.Click
        If imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Or Not IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
            End Select
            Exit Sub
        End If
        ElementCount = 0
        RemProvAppxPackage.ListView1.Items.Clear()
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting provisioned AppX packages..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo paquetes aprovisionados AppX..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des paquets AppX provisionnés en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting provisioned AppX packages..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo paquetes aprovisionados AppX..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des paquets AppX provisionnés en cours..."
        End Select
        ProgressPanel.OperationNum = 994
        If Not CompletedTasks(2) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Try
            For x = 0 To Array.LastIndexOf(imgAppxPackageNames, imgAppxPackageNames.Last)
                If imgAppxPackageNames(x) = "" Or imgAppxPackageNames(x) = "Nothing" Then
                    Continue For
                Else
                    If Directory.Exists(MountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & imgAppxPackageNames(x)) Then
                        If My.Computer.FileSystem.GetFiles(MountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & imgAppxPackageNames(x), FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").Count = 0 Then
                            RemProvAppxPackage.ListView1.Items.Add(New ListViewItem(New String() {imgAppxPackageNames(x), imgAppxDisplayNames(x), imgAppxArchitectures(x), imgAppxResourceIds(x), imgAppxVersions(x), "No"}))
                        Else
                            RemProvAppxPackage.ListView1.Items.Add(New ListViewItem(New String() {imgAppxPackageNames(x), imgAppxDisplayNames(x), imgAppxArchitectures(x), imgAppxResourceIds(x), imgAppxVersions(x), "Yes"}))
                        End If
                    Else
                        RemProvAppxPackage.ListView1.Items.Add(New ListViewItem(New String() {imgAppxPackageNames(x), imgAppxDisplayNames(x), imgAppxArchitectures(x), imgAppxResourceIds(x), imgAppxVersions(x), "No"}))
                    End If
                End If
            Next
        Catch ex As Exception
            ' We should have enough with the entries already added.
            Exit Try
        End Try
        ' Begin counting
        Try
            For x = 0 To Array.LastIndexOf(imgAppxPackageNames, imgAppxPackageNames.Last)
                If imgAppxPackageNames(x) = "" Then
                    Exit For
                End If
                ElementCount += 1
            Next
        Catch ex As Exception
            Exit Try
        End Try
        If ElementCount <= 0 Then
            ElementCount = RemProvAppxPackage.ListView1.Items.Count
        End If
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        RemProvAppxPackage.Label2.Text = "This image contains " & ElementCount & " AppX packages."
                    Case "ESN"
                        RemProvAppxPackage.Label2.Text = "Esta imagen contiene " & ElementCount & " paquetes AppX."
                    Case "FRA"
                        RemProvAppxPackage.Label2.Text = "Cette image contient " & ElementCount & " paquets AppX."
                End Select
            Case 1
                RemProvAppxPackage.Label2.Text = "This image contains " & ElementCount & " AppX packages."
            Case 2
                RemProvAppxPackage.Label2.Text = "Esta imagen contiene " & ElementCount & " paquetes AppX."
            Case 3
                RemProvAppxPackage.Label2.Text = "Cette image contient " & ElementCount & " paquets AppX."
        End Select
        RemProvAppxPackage.ShowDialog()
    End Sub

    Private Sub DeleteImage_Click(sender As Object, e As EventArgs) Handles DeleteImage.Click
        ImgIndexDelete.ShowDialog()
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        PopupImageManager.Location = LinkLabel3.PointToScreen(Point.Empty)
        PopupImageManager.Top -= PopupImageManager.Height
        If PopupImageManager.ShowDialog() = DialogResult.OK Then
            If MountedImageMountDirs.Count > 0 Then
                MountDir = PopupImageManager.selectedMntDir
                If MountedImageMountDirs.Count > 0 Then
                    Try
                        For x = 0 To Array.LastIndexOf(MountedImageMountDirs, MountedImageMountDirs.Last)
                            If MountedImageMountDirs(x) = MountDir Then
                                ImgIndex = MountedImageImgIndexes(x)
                                SourceImg = MountedImageImgFiles(x)
                                IIf(MountedImageMountedReWr(x) = 1, isReadOnly = False, isReadOnly = True)
                            End If
                        Next
                    Catch ex As Exception
                        Exit Try
                    End Try
                    UpdateProjProperties(True, If(isReadOnly, True, False))
                    SaveDTProj()
                End If
            Else
                Exit Sub
            End If
        End If
    End Sub

    Private Sub MountedImageDetectorBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles MountedImageDetectorBW.DoWork
        Do
            If MountedImageDetectorBW.CancellationPending Or ImgBW.IsBusy Then Exit Do
            DetectMountedImages(False)
            Thread.Sleep(500)
        Loop
    End Sub

    Private Sub MountedImageDetectorBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles MountedImageDetectorBW.RunWorkerCompleted
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Options.Label38.Text = If(MountedImageDetectorBW.IsBusy, "running", "stopped")
                        Options.Button8.Text = If(MountedImageDetectorBW.IsBusy, "Stop", "Start")
                    Case "ESN"
                        Options.Label38.Text = If(MountedImageDetectorBW.IsBusy, "iniciado", "detenido")
                        Options.Button8.Text = If(MountedImageDetectorBW.IsBusy, "Detener", "Iniciar")
                    Case "FRA"
                        Options.Label38.Text = If(MountedImageDetectorBW.IsBusy, "démarré", "arrêté")
                        Options.Button8.Text = If(MountedImageDetectorBW.IsBusy, "Arrêter", "Démarrer")
                End Select
            Case 1
                Options.Label38.Text = If(MountedImageDetectorBW.IsBusy, "running", "stopped")
                Options.Button8.Text = If(MountedImageDetectorBW.IsBusy, "Stop", "Start")
            Case 2
                Options.Label38.Text = If(MountedImageDetectorBW.IsBusy, "iniciado", "detenido")
                Options.Button8.Text = If(MountedImageDetectorBW.IsBusy, "Detener", "Iniciar")
            Case 3
                Options.Label38.Text = If(MountedImageDetectorBW.IsBusy, "démarré", "arrêté")
                Options.Button8.Text = If(MountedImageDetectorBW.IsBusy, "Arrêter", "Démarrer")
        End Select
    End Sub

    Private Sub MountedImageManagerTSMI_Click(sender As Object, e As EventArgs) Handles MountedImageManagerTSMI.Click
        If MountedImgMgr.Visible Then
            If MountedImgMgr.WindowState = FormWindowState.Minimized Then
                MountedImgMgr.WindowState = FormWindowState.Normal
            Else
                MountedImgMgr.BringToFront()
            End If
            MountedImgMgr.Focus()
        Else
            MountedImgMgr.Show()
        End If
    End Sub

    Private Sub ReportFeedbackToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportFeedbackToolStripMenuItem.Click
        Process.Start("https://github.com/CodingWonders/DISMTools/issues/new/choose")
    End Sub

    Private Sub UnmountImage_Click(sender As Object, e As EventArgs) Handles UnmountImage.Click, UnmountSettingsToolStripMenuItem.Click
        If isProjectLoaded And MountDir = MountedImgMgr.ListView1.FocusedItem.SubItems(2).Text Then
            ImgUMount.RadioButton1.Checked = True
            ImgUMount.RadioButton2.Checked = False
            ImgUMount.TextBox1.Text = ""
        Else
            ImgUMount.RadioButton1.Checked = False
            ImgUMount.RadioButton2.Checked = True
            ImgUMount.TextBox1.Text = MountedImgMgr.ListView1.FocusedItem.SubItems(2).Text
            ProgressPanel.UMountImgIndex = MountedImgMgr.ListView1.FocusedItem.SubItems(1).Text
        End If
        ImgUMount.ShowDialog()
    End Sub

    Private Sub CommitAndUnmountTSMI_Click(sender As Object, e As EventArgs) Handles CommitAndUnmountTSMI.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.OperationNum = 21
        ProgressPanel.UMountLocalDir = False
        ProgressPanel.RandomMountDir = MountedImgMgr.ListView1.FocusedItem.SubItems(2).Text   ' Hope there isn't anything to set here
        ProgressPanel.UMountImgIndex = MountedImgMgr.ListView1.FocusedItem.SubItems(1).Text
        ProgressPanel.MountDir = ""
        ProgressPanel.UMountOp = 0
        ProgressPanel.ShowDialog()
    End Sub

    Private Sub DiscardAndUnmountTSMI_Click(sender As Object, e As EventArgs) Handles DiscardAndUnmountTSMI.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.OperationNum = 21
        ProgressPanel.UMountLocalDir = False
        ProgressPanel.RandomMountDir = MountedImgMgr.ListView1.FocusedItem.SubItems(2).Text   ' Hope there isn't anything to set here
        ProgressPanel.UMountImgIndex = MountedImgMgr.ListView1.FocusedItem.SubItems(1).Text
        ProgressPanel.MountDir = ""
        ProgressPanel.UMountOp = 1
        ProgressPanel.ShowDialog()
    End Sub

    Private Sub CleanupImage_Click(sender As Object, e As EventArgs) Handles CleanupImage.Click
        ImgCleanup.ShowDialog()
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        ImgCleanup.ShowDialog()
    End Sub

    Private Sub NewProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewProjectToolStripMenuItem.Click
        NewProj.ShowDialog()
    End Sub

    Private Sub OpenExistingProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenExistingProjectToolStripMenuItem.Click
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If File.Exists(OpenFileDialog1.FileName) Then
                If isProjectLoaded Then UnloadDTProj(False, If(OnlineManagement Or OfflineManagement, False, True), False)
                If ImgBW.IsBusy Then Exit Sub
                ProgressPanel.OperationNum = 990
                LoadDTProj(OpenFileDialog1.FileName, Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName), False, False)
            End If
        End If
    End Sub

    Private Sub AddCapability_Click(sender As Object, e As EventArgs) Handles AddCapability.Click
        If imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Or Not IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
            End Select
            Exit Sub
        End If
        ElementCount = 0
        AddCapabilities.ListView1.Items.Clear()
        ProgressPanel.OperationNum = 994
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting capability names and their state..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de funcionalidades y sus estados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms des capacités et de leur état en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting capability names and their state..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de funcionalidades y sus estados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms des capacités et de leur état en cours..."
        End Select
        If Not CompletedTasks(3) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Try
            For x = 0 To Array.LastIndexOf(imgCapabilityIds, imgCapabilityIds.Last)
                If imgCapabilityState(x) = "Installed" Or imgCapabilityState(x) = "Install Pending" Then
                    Continue For
                End If
                AddCapabilities.ListView1.Items.Add(New ListViewItem(New String() {imgCapabilityIds(x), imgCapabilityState(x)}))
            Next
        Catch ex As Exception
            Exit Try
        End Try
        Try
            For x = 0 To Array.LastIndexOf(imgCapabilityIds, imgCapabilityIds.Last)
                If imgCapabilityIds(x) = "" Then
                    Exit For
                End If
                ElementCount += 1
            Next
        Catch ex As Exception
            Exit Try
        End Try
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        AddCapabilities.Label4.Text = "This image contains " & ElementCount & " capabilities."
                    Case "ESN"
                        AddCapabilities.Label4.Text = "Esta imagen contiene " & ElementCount & " funcionalidades."
                    Case "FRA"
                        AddCapabilities.Label4.Text = "Cette image contient " & ElementCount & " capacités."
                End Select
            Case 1
                AddCapabilities.Label4.Text = "This image contains " & ElementCount & " capabilities."
            Case 2
                AddCapabilities.Label4.Text = "Esta imagen contiene " & ElementCount & " funcionalidades."
            Case 3
                AddCapabilities.Label4.Text = "Cette image contient " & ElementCount & " capacités."
        End Select
        AddCapabilities.ShowDialog()
    End Sub

    Private Sub RemoveCapability_Click(sender As Object, e As EventArgs) Handles RemoveCapability.Click
        If imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Or Not IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
            End Select
            Exit Sub
        End If
        ElementCount = 0
        RemCapabilities.ListView1.Items.Clear()
        ProgressPanel.OperationNum = 994
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting capability names and their state..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de funcionalidades y sus estados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms des capacités et de leur état en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting capability names and their state..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de funcionalidades y sus estados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms des capacités et de leur état en cours..."
        End Select
        If Not CompletedTasks(3) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Try
            For x = 0 To Array.LastIndexOf(imgCapabilityIds, imgCapabilityIds.Last)
                If imgCapabilityState(x) = "Removed" Or imgCapabilityState(x) = "Not present" Or imgCapabilityState(x) = "Uninstalled" Then
                    Continue For
                End If
                RemCapabilities.ListView1.Items.Add(New ListViewItem(New String() {imgCapabilityIds(x), imgCapabilityState(x)}))
            Next
        Catch ex As Exception
            Exit Try
        End Try
        Try
            For x = 0 To Array.LastIndexOf(imgCapabilityIds, imgCapabilityIds.Last)
                If imgCapabilityIds(x) = "" Then
                    Exit For
                End If
                ElementCount += 1
            Next
        Catch ex As Exception
            Exit Try
        End Try
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        RemCapabilities.Label2.Text = "This image contains " & ElementCount & " capabilities."
                    Case "ESN"
                        RemCapabilities.Label2.Text = "Esta imagen contiene " & ElementCount & " funcionalidades."
                    Case "FRA"
                        AddCapabilities.Label4.Text = "Cette image contient " & ElementCount & " capacités."
                End Select
            Case 1
                RemCapabilities.Label2.Text = "This image contains " & ElementCount & " capabilities."
            Case 2
                RemCapabilities.Label2.Text = "Esta imagen contiene " & ElementCount & " funcionalidades."
            Case 3
                AddCapabilities.Label4.Text = "Cette image contient " & ElementCount & " capacités."
        End Select
        RemCapabilities.ShowDialog()
    End Sub

    Private Sub AddDriver_Click(sender As Object, e As EventArgs) Handles AddDriver.Click
        If Not OnlineManagement Then
            AddDrivers.ShowDialog()
        Else
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on online installations", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en instalaciones activas", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge par les installations en ligne", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on online installations", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en instalaciones activas", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge par les installations en ligne", vbOKOnly + vbCritical, Text)
            End Select
        End If
    End Sub

    Private Sub RemoveDriver_Click(sender As Object, e As EventArgs) Handles RemoveDriver.Click
        If OnlineManagement Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on online installations", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en instalaciones activas", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge par les installations en ligne", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on online installations", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en instalaciones activas", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge par les installations en ligne", vbOKOnly + vbCritical, Text)
            End Select
            Exit Sub
        End If
        RemDrivers.ListView1.Items.Clear()
        ProgressPanel.OperationNum = 994
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting installed driver packages..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo paquetes de controladores instalados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des paquets de pilotes installés en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting installed driver packages..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo paquetes de controladores instalados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des paquets de pilotes installés en cours..."
        End Select
        If Not CompletedTasks(4) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Try
            For x = 0 To Array.LastIndexOf(imgDrvPublishedNames, imgDrvPublishedNames.Last)
                If RemDrivers.CheckBox1.Checked Then
                    If imgDrvBootCriticalStatus(x) Then Continue For
                End If
                If RemDrivers.CheckBox2.Checked Then
                    If CBool(imgDrvInbox(x)) Then Continue For
                End If
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                RemDrivers.ListView1.Items.Add(New ListViewItem(New String() {imgDrvPublishedNames(x), Path.GetFileName(imgDrvOGFileNames(x)), imgDrvProviderNames(x), imgDrvClassNames(x), If(CBool(imgDrvInbox(x)), "Yes", "No"), If(imgDrvBootCriticalStatus(x), "Yes", "No"), imgDrvVersions(x), imgDrvDates(x)}))
                            Case "ESN"
                                RemDrivers.ListView1.Items.Add(New ListViewItem(New String() {imgDrvPublishedNames(x), Path.GetFileName(imgDrvOGFileNames(x)), imgDrvProviderNames(x), imgDrvClassNames(x), If(CBool(imgDrvInbox(x)), "Sí", "No"), If(imgDrvBootCriticalStatus(x), "Sí", "No"), imgDrvVersions(x), imgDrvDates(x)}))
                            Case "FRA"
                                RemDrivers.ListView1.Items.Add(New ListViewItem(New String() {imgDrvPublishedNames(x), Path.GetFileName(imgDrvOGFileNames(x)), imgDrvProviderNames(x), imgDrvClassNames(x), If(CBool(imgDrvInbox(x)), "Oui", "Non"), If(imgDrvBootCriticalStatus(x), "Oui", "Non"), imgDrvVersions(x), imgDrvDates(x)}))
                        End Select
                    Case 1
                        RemDrivers.ListView1.Items.Add(New ListViewItem(New String() {imgDrvPublishedNames(x), Path.GetFileName(imgDrvOGFileNames(x)), imgDrvProviderNames(x), imgDrvClassNames(x), If(CBool(imgDrvInbox(x)), "Yes", "No"), If(imgDrvBootCriticalStatus(x), "Yes", "No"), imgDrvVersions(x), imgDrvDates(x)}))
                    Case 2
                        RemDrivers.ListView1.Items.Add(New ListViewItem(New String() {imgDrvPublishedNames(x), Path.GetFileName(imgDrvOGFileNames(x)), imgDrvProviderNames(x), imgDrvClassNames(x), If(CBool(imgDrvInbox(x)), "Sí", "No"), If(imgDrvBootCriticalStatus(x), "Sí", "No"), imgDrvVersions(x), imgDrvDates(x)}))
                    Case 3
                        RemDrivers.ListView1.Items.Add(New ListViewItem(New String() {imgDrvPublishedNames(x), Path.GetFileName(imgDrvOGFileNames(x)), imgDrvProviderNames(x), imgDrvClassNames(x), If(CBool(imgDrvInbox(x)), "Oui", "Non"), If(imgDrvBootCriticalStatus(x), "Oui", "Non"), imgDrvVersions(x), imgDrvDates(x)}))
                End Select
            Next
        Catch ex As Exception
            Exit Try
        End Try
        RemDrivers.ShowDialog()
    End Sub

    Private Sub ActionEditorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ActionEditorToolStripMenuItem.Click
        Actions_MainForm.Show()
    End Sub

    ''' <summary>
    ''' Detects the source for optional feature installs and component repairs from the "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Servicing" registry key
    ''' </summary>
    ''' <returns>Returns GPOSource as the aforementioned source if this function runs correctly. Otherwise, it returns Nothing</returns>
    ''' <remarks>"LocalSourcePath" is updated every time a source is specified in the group policy editor. "GPOSource" pulls the value from "LocalSourcePath", which can be a local folder, a remote server or a Windows image (if it begins with "wim:\")</remarks>
    Function GetSrcFromGPO() As String
        Try
            Dim GPOSourceRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Servicing", False)
            Dim GPOSource As String = GPOSourceRk.GetValue("LocalSourcePath").ToString()
            GPOSourceRk.Close()
            Return GPOSource
        Catch ex As Exception
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("Could not gather source from group policy. Reason:" & CrLf & CrLf & ex.ToString(), vbOKOnly + vbCritical, "Detect from group policy")
                        Case "ESN"
                            MsgBox("No se pudo recopilar el origen de las políticas de grupo. Razón:" & CrLf & CrLf & ex.ToString(), vbOKOnly + vbCritical, "Detectar políticas de grupo")
                        Case "FRA"
                            MsgBox("Impossible d'obtenir la source de la directive de groupe. Raison :" & CrLf & CrLf & ex.ToString(), vbOKOnly + vbCritical, "Détecter à partir d'une directive de groupe")
                    End Select
                Case 1
                    MsgBox("Could not gather source from group policy. Reason:" & CrLf & CrLf & ex.ToString(), vbOKOnly + vbCritical, "Detect from group policy")
                Case 2
                    MsgBox("No se pudo recopilar el origen de las políticas de grupo. Razón:" & CrLf & CrLf & ex.ToString(), vbOKOnly + vbCritical, "Detectar políticas de grupo")
                Case 3
                    MsgBox("Impossible d'obtenir la source de la directive de groupe. Raison :" & CrLf & CrLf & ex.ToString(), vbOKOnly + vbCritical, "Détecter à partir d'une directive de groupe")
            End Select
            Return Nothing
        End Try
        Return Nothing
    End Function

    Private Sub AddProvisioningPackage_Click(sender As Object, e As EventArgs) Handles AddProvisioningPackage.Click
        AddProvisioningPkg.ShowDialog()
    End Sub

    Private Sub OnlineInstMgmt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles OnlineInstMgmt.LinkClicked
        If Not HomePanel.Visible Then Exit Sub
        ActiveInstAccessWarn.Label2.Visible = False
        BeginOnlineManagement(True)
    End Sub

    ''' <summary>
    ''' Gets the application display name from the AppX package manifest
    ''' </summary>
    ''' <param name="PackageName">The package name of an application</param>
    ''' <param name="DisplayName">The display name of an application. This parameter is required when there are multiple directories with their names containing <paramref name="PackageName">the package name</paramref></param>
    ''' <returns>pkgName: the suitable package display name</returns>
    ''' <remarks>If pkgName returns Nothing, the callers will hide those options calling this function</remarks>
    Function GetPackageDisplayName(PackageName As String, Optional DisplayName As String = "")
        If File.Exists(Application.StartupPath & "\AppxManifest.xml") Then File.Delete(Application.StartupPath & "\AppxManifest.xml")
        If File.Exists(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName & "\AppxManifest.xml") Then
            ' Copy manifest to startup dir
            File.Copy(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName & "\AppxManifest.xml", Application.StartupPath & "\AppxManifest.xml")
            Dim XMLReaderRTB As New RichTextBox With {
                .Text = File.ReadAllText(Application.StartupPath & "\AppxManifest.xml")
            }
            ' Go through each line until we find the properties tag
            For x = 0 To XMLReaderRTB.Lines.Count - 1
                If XMLReaderRTB.Lines(x).EndsWith("<Properties>") Then
                    ' Go through each line until we find the display name
                    For y = x To XMLReaderRTB.Lines.Count - 1
                        If XMLReaderRTB.Lines(y).Replace("<", "").Trim().Replace(">", "").Trim().Replace(" ", "").Trim().StartsWith("DisplayName", StringComparison.OrdinalIgnoreCase) Then
                            Dim pkgName As String = XMLReaderRTB.Lines(y).Replace("<DisplayName>", "").Trim().Replace("</DisplayName>", "").Trim()
                            File.Delete(Application.StartupPath & "\AppxManifest.xml")
                            Return pkgName
                        End If
                    Next
                End If
            Next
        Else
            If Directory.GetDirectories(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps", DisplayName & "*", SearchOption.TopDirectoryOnly).Count > 1 Then
                ' Skip architecture neutral packages
                Dim pkgDirs() As String = Directory.GetDirectories(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps", DisplayName & "*", SearchOption.TopDirectoryOnly)
                For Each folder In pkgDirs
                    If Not folder.Contains("neutral") Then
                        ' Copy manifest to startup dir
                        File.Copy(folder & "\AppxManifest.xml", Application.StartupPath & "\AppxManifest.xml")
                        Dim XMLReaderRTB As New RichTextBox With {
                            .Text = File.ReadAllText(Application.StartupPath & "\AppxManifest.xml")
                        }
                        ' Go through each line until we find the properties tag
                        For x = 0 To XMLReaderRTB.Lines.Count - 1
                            If XMLReaderRTB.Lines(x).EndsWith("<Properties>") Then
                                ' Go through each line until we find the display name
                                For y = x To XMLReaderRTB.Lines.Count - 1
                                    If XMLReaderRTB.Lines(y).Replace("<", "").Trim().Replace(">", "").Trim().Replace(" ", "").Trim().StartsWith("DisplayName", StringComparison.OrdinalIgnoreCase) Then
                                        Dim pkgName As String = XMLReaderRTB.Lines(y).Replace("<DisplayName>", "").Trim().Replace("</DisplayName>", "").Trim()
                                        File.Delete(Application.StartupPath & "\AppxManifest.xml")
                                        Return pkgName
                                    End If
                                Next
                            End If
                        Next
                    End If
                Next
            End If
        End If
        Return Nothing
    End Function

    Function GetSuitablePackageFolder(PackageName As String)
        If Directory.GetDirectories(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps", PackageName & "*", SearchOption.TopDirectoryOnly).Count > 1 Then
            Dim pkgDirs() As String = Directory.GetDirectories(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps", PackageName & "*", SearchOption.TopDirectoryOnly)
            For Each folder In pkgDirs
                If Not folder.Contains("neutral") Then
                    Return folder
                End If
            Next
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets the path of the main logo of an installed provisioned AppX package
    ''' </summary>
    ''' <param name="PackageName">The name of the AppX package</param>
    ''' <returns>This function returns a path to the logo asset of an application</returns>
    ''' <remarks>This can be a little wonky and may not show the main asset. However, since this allows the program to launch an image viewer afterwards, you can browse other assets</remarks>
    Function GetStoreAppMainLogo(PackageName As String)
        Try
            If File.Exists(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName & "\AppxManifest.xml") Then
                ' Read from manifest
                Dim ManFile As New RichTextBox() With {
                    .Text = File.ReadAllText(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName & "\AppxManifest.xml")
                }
                For Each line In ManFile.Lines
                    If line.Contains("Logo") Then
                        Dim SplitPaths As New List(Of String)
                        SplitPaths = line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim().Split("\").ToList()
                        SplitPaths.RemoveAt(SplitPaths.Count - 1)
                        Dim newPath As String = String.Join("\", SplitPaths)
                        If Directory.GetFiles(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName & "\" & newPath, "*.png").Count > 1 Then
                            Dim logoFiles() As String = Directory.GetFiles(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName & "\" & newPath, "*.png")
                            ' Choose the largest one
                            Return logoFiles.Last
                        Else
                            If File.Exists(Path.Combine(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName, line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim())) Then
                                Return Path.Combine(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName, line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim())
                            Else
                                ' There may be 1 asset in the folder we're looking on. Open it
                                Dim logoFiles() As String = Directory.GetFiles(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName & "\" & newPath, "*.png")
                                Return logoFiles.Last
                            End If
                        End If
                    End If
                Next
            ElseIf Directory.GetDirectories(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps", PackageName & "*", SearchOption.TopDirectoryOnly).Count > 1 Then
                Dim pkgDirs() As String = Directory.GetDirectories(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps", PackageName & "*", SearchOption.TopDirectoryOnly)
                For Each folder In pkgDirs
                    If Not folder.Contains("neutral") Then
                        ' Read from manifest
                        Dim ManFile As New RichTextBox() With {
                            .Text = File.ReadAllText(folder & "AppxManifest.xml")
                        }
                        For Each line In ManFile.Lines
                            If line.Contains("Logo") Then
                                Return Path.Combine(folder, line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim())
                            End If
                        Next
                    End If
                Next
            End If
        Catch ex As Exception
            Return Nothing
        End Try
        Return Nothing
    End Function

    Private Sub ViewPackageDirectoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewPackageDirectoryToolStripMenuItem.Click
        Dim suitableFolderName As String = ""
        Try
            suitableFolderName = GetSuitablePackageFolder(RemProvAppxPackage.ListView1.FocusedItem.SubItems(1).Text.Replace(" (Cortana)", "").Trim())
        Catch ex As Exception
            ' Continue
        End Try
        If suitableFolderName <> "" Then
            Process.Start(suitableFolderName)
            Exit Sub
        End If
        If OnlineManagement Then
            If Directory.Exists(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\Program Files\WindowsApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text) Then
                Process.Start(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\Program Files\WindowsApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text)
            ElseIf Directory.Exists(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\Windows\SystemApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text) Then
                Process.Start(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\Windows\SystemApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text)
            End If
        Else
            If Directory.Exists(MountDir & "\Program Files\WindowsApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text) Then
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", MountDir & "\Program Files\WindowsApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text)
            ElseIf Directory.Exists(MountDir & "\Windows\SystemApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text) Then
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", MountDir & "\Windows\SystemApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text)
            End If
        End If
    End Sub

    Private Sub ResViewTSMI_Click(sender As Object, e As EventArgs) Handles ResViewTSMI.Click
        Dim MainLogo As String = GetStoreAppMainLogo(RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text)
        If MainLogo <> "" And File.Exists(MainLogo) Then
            Process.Start(MainLogo)
            Exit Sub
        End If
        Dim suitableFolderName As String = ""
        Try
            suitableFolderName = GetSuitablePackageFolder(RemProvAppxPackage.ListView1.FocusedItem.SubItems(1).Text.Replace(" (Cortana)", "").Trim())
        Catch ex As Exception
            ' Continue
        End Try
        If suitableFolderName <> "" Then
            If File.Exists(suitableFolderName & "\AppxManifest.xml") Then
                Dim ManFile As New RichTextBox() With {
                    .Text = File.ReadAllText(suitableFolderName & "\AppxManifest.xml")
                }
                For Each line In ManFile.Lines
                    If line.Contains("<Logo>") Then
                        Dim SplitPaths As New List(Of String)
                        SplitPaths = line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim().Split("\").ToList()
                        SplitPaths.RemoveAt(SplitPaths.Count - 1)
                        Dim newPath As String = String.Join("\", SplitPaths)
                        Process.Start(suitableFolderName & "\" & newPath)
                        Exit For
                    End If
                Next
            End If
            Exit Sub
        End If
        If OnlineManagement Then
            If Directory.Exists(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\Program Files\WindowsApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text & "\Assets") Then
                Process.Start(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\Program Files\WindowsApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text & "\Assets")
            ElseIf Directory.Exists(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\Windows\SystemApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text & "\Assets") Then
                Process.Start(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) & "\Windows\SystemApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text & "\Assets")
            End If
        Else
            If Directory.Exists(MountDir & "\Program Files\WindowsApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text & "\Assets") Then
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", MountDir & "\Program Files\WindowsApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text & "\Assets")
            ElseIf Directory.Exists(MountDir & "\Windows\SystemApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text & "\Assets") Then
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", MountDir & "\Windows\SystemApps\" & RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text & "\Assets")
            End If
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub UpdateLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles UpdateLink.LinkClicked
        If Not HomePanel.Visible Then Exit Sub
        If File.Exists(Application.StartupPath & "\update.exe") Then File.Delete(Application.StartupPath & "\update.exe")
        Try
            Using client As New WebClient()
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                client.DownloadFile("https://github.com/CodingWonders/DISMTools/raw/" & dtBranch & "/Updater/DISMTools-UCS/update-bin/update.exe", Application.StartupPath & "\update.exe")
            End Using
        Catch ex As WebException
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("We couldn't download the update checker. Reason:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, "Check for updates")
                        Case "ESN"
                            MsgBox("No pudimos descargar el comprobador de actualizaciones. Razón:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, "Comprobar actualizaciones")
                        Case "FRA"
                            MsgBox("Nous n'avons pas pu télécharger le vérificateur de mise à jour. Raison :" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, "Vérifier les mises à jour du programme")
                    End Select
                Case 1
                    MsgBox("We couldn't download the update checker. Reason:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, "Check for updates")
                Case 2
                    MsgBox("No pudimos descargar el comprobador de actualizaciones. Razón:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, "Comprobar actualizaciones")
                Case 3
                    MsgBox("Nous n'avons pas pu télécharger le vérificateur de mise à jour. Raison :" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, "Vérifier les mises à jour du programme")
            End Select
            Exit Sub
        End Try
        If File.Exists(Application.StartupPath & "\update.exe") Then Process.Start(Application.StartupPath & "\update.exe", "/" & dtBranch & " /pid=" & Process.GetCurrentProcess().Id)
    End Sub

    Private Sub prjTreeView_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles prjTreeView.NodeMouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            prjTreeView.SelectedNode = e.Node
            If e.Node.Name.StartsWith("dandi") Then
                OfSelectedArchitectureToolStripMenuItem.Enabled = Not e.Node.Name.Equals("dandi")
                CopyDeploymentToolsToolStripMenuItem.Enabled = True
                ImageOperationsToolStripMenuItem.Enabled = False
                UnattendedAnswerFilesToolStripMenuItem1.Enabled = False
                ScratchDirectorySettingsToolStripMenuItem.Enabled = False
                ManageReportsToolStripMenuItem.Enabled = False
            ElseIf e.Node.Name = "mount" Then
                CopyDeploymentToolsToolStripMenuItem.Enabled = False
                ImageOperationsToolStripMenuItem.Enabled = True
                UnattendedAnswerFilesToolStripMenuItem1.Enabled = False
                ScratchDirectorySettingsToolStripMenuItem.Enabled = False
                ManageReportsToolStripMenuItem.Enabled = False
            ElseIf e.Node.Name = "unattend_xml" Then
                CopyDeploymentToolsToolStripMenuItem.Enabled = False
                ImageOperationsToolStripMenuItem.Enabled = False
                UnattendedAnswerFilesToolStripMenuItem1.Enabled = True
                ScratchDirectorySettingsToolStripMenuItem.Enabled = False
                ManageReportsToolStripMenuItem.Enabled = False
            ElseIf e.Node.Name = "scr_temp" Then
                CopyDeploymentToolsToolStripMenuItem.Enabled = False
                ImageOperationsToolStripMenuItem.Enabled = False
                UnattendedAnswerFilesToolStripMenuItem1.Enabled = False
                ScratchDirectorySettingsToolStripMenuItem.Enabled = True
                ManageReportsToolStripMenuItem.Enabled = False
            ElseIf e.Node.Name = "reports" Then
                CopyDeploymentToolsToolStripMenuItem.Enabled = False
                ImageOperationsToolStripMenuItem.Enabled = False
                UnattendedAnswerFilesToolStripMenuItem1.Enabled = False
                ScratchDirectorySettingsToolStripMenuItem.Enabled = False
                ManageReportsToolStripMenuItem.Enabled = True
            Else
                CopyDeploymentToolsToolStripMenuItem.Enabled = False
                ImageOperationsToolStripMenuItem.Enabled = False
                UnattendedAnswerFilesToolStripMenuItem1.Enabled = False
                ScratchDirectorySettingsToolStripMenuItem.Enabled = False
                ManageReportsToolStripMenuItem.Enabled = False
            End If
            Dim pnt As Point = e.Location
            TreeViewCMS.Show(sender, pnt)
        End If
    End Sub

    Private Sub ADKCopierBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ADKCopierBW.DoWork
        If prjTreeView.SelectedNode.Name.StartsWith("dandi") Then
            Try
                Dim adkInst As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\WIMMount")
                Dim adk As String = adkInst.GetValue("AdkInstallation").ToString()
                adkInst.Close()
                If adk = "1" Then
                    ' Copy deployment tools. This will default to "Program Files\Windows Kits\10"
                    Select Case adkCopyArg
                        Case 0
                            ' Copy all architectures
                            If Directory.Exists(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools") Then
                                Dim arches() As String = New String(3) {"x86", "amd64", "arm", "arm64"}
                                For x = 0 To Array.LastIndexOf(arches, arches.Last)
                                    archIntg = x + 1
                                    currentArch = arches(x)
                                    ' Count files
                                    fileCount = My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\" & arches(x), FileIO.SearchOption.SearchAllSubDirectories).Count
                                    Select Case Language
                                        Case 0
                                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                                Case "ENU", "ENG"
                                                    MenuDesc.Text = "Preparing to copy deployment tools..." & If(adkCopyArg = 0, " (architecture " & archIntg & " of 4)", "")
                                                Case "ESN"
                                                    MenuDesc.Text = "Preparándonos para copiar las herramientas de implementación..." & If(adkCopyArg = 0, " (arquitectura " & archIntg & " de 4)", "")
                                                Case "FRA"
                                                    MenuDesc.Text = "Préparation de la copie des outils de déploiement en cours..." & If(adkCopyArg = 0, " (architecture " & archIntg & " de 4)", "")
                                            End Select
                                        Case 1
                                            MenuDesc.Text = "Preparing to copy deployment tools..." & If(adkCopyArg = 0, " (architecture " & archIntg & " of 4)", "")
                                        Case 2
                                            MenuDesc.Text = "Preparándonos para copiar las herramientas de implementación..." & If(adkCopyArg = 0, " (arquitectura " & archIntg & " de 4)", "")
                                        Case 3
                                            MenuDesc.Text = "Préparation de la copie des outils de déploiement en cours..." & If(adkCopyArg = 0, " (architecture " & archIntg & " de 4)", "")
                                    End Select
                                    CurrentFileInt = 0
                                    For Each folder In My.Computer.FileSystem.GetDirectories(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\" & arches(x), FileIO.SearchOption.SearchAllSubDirectories)
                                        Directory.CreateDirectory(folder.Replace(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\" & arches(x), projPath & "\DandI\" & arches(x)))
                                    Next
                                    For Each archFile In My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\" & arches(x), FileIO.SearchOption.SearchAllSubDirectories)
                                        ADKCopierBW.ReportProgress(Math.Round(CurrentFileInt / fileCount, 2) * 100)
                                        File.Copy(archFile, archFile.Replace(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\" & arches(x), projPath & "\DandI\" & arches(x)), True)
                                        CurrentFileInt += 1
                                    Next
                                Next
                            End If
                        Case 1
                            ' Copy x86 architecture
                            ' Count files
                            Dim fileCount As Integer = My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\x86", FileIO.SearchOption.SearchAllSubDirectories).Count
                            Select Case Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            MenuDesc.Text = "Preparing to copy deployment tools..." & If(adkCopyArg = 0, " (architecture " & archIntg & " of 4)", "")
                                        Case "ESN"
                                            MenuDesc.Text = "Preparándonos para copiar las herramientas de implementación..." & If(adkCopyArg = 0, " (arquitectura " & archIntg & " de 4)", "")
                                        Case "FRA"
                                            MenuDesc.Text = "Préparation de la copie des outils de déploiement en cours..." & If(adkCopyArg = 0, " (architecture " & archIntg & " de 4)", "")
                                    End Select
                                Case 1
                                    MenuDesc.Text = "Preparing to copy deployment tools..." & If(adkCopyArg = 0, " (architecture " & archIntg & " of 4)", "")
                                Case 2
                                    MenuDesc.Text = "Preparándonos para copiar las herramientas de implementación..." & If(adkCopyArg = 0, " (arquitectura " & archIntg & " de 4)", "")
                                Case 3
                                    MenuDesc.Text = "Préparation de la copie des outils de déploiement en cours..." & If(adkCopyArg = 0, " (architecture " & archIntg & " de 4)", "")
                            End Select
                            Dim CurrentFileInt As Integer = 0
                            For Each folder In My.Computer.FileSystem.GetDirectories(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\x86", FileIO.SearchOption.SearchAllSubDirectories)
                                Directory.CreateDirectory(folder.Replace(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\x86", projPath & "\DandI\x86"))
                            Next
                            For Each archFile In My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\x86", FileIO.SearchOption.SearchAllSubDirectories)
                                ADKCopierBW.ReportProgress(Math.Round(CurrentFileInt / fileCount, 2) * 100)
                                File.Copy(archFile, archFile.Replace(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\x86", projPath & "\DandI\x86"), True)
                                CurrentFileInt += 1
                            Next
                        Case 2
                            ' Copy AMD64 architecture
                            ' Count files
                            Dim fileCount As Integer = My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\amd64", FileIO.SearchOption.SearchAllSubDirectories).Count
                            Select Case Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            MenuDesc.Text = "Preparing to copy deployment tools..." & If(adkCopyArg = 0, " (architecture " & archIntg & " of 4)", "")
                                        Case "ESN"
                                            MenuDesc.Text = "Preparándonos para copiar las herramientas de implementación..." & If(adkCopyArg = 0, " (arquitectura " & archIntg & " de 4)", "")
                                        Case "FRA"
                                            MenuDesc.Text = "Préparation de la copie des outils de déploiement en cours..." & If(adkCopyArg = 0, " (architecture " & archIntg & " de 4)", "")
                                    End Select
                                Case 1
                                    MenuDesc.Text = "Preparing to copy deployment tools..." & If(adkCopyArg = 0, " (architecture " & archIntg & " of 4)", "")
                                Case 2
                                    MenuDesc.Text = "Preparándonos para copiar las herramientas de implementación..." & If(adkCopyArg = 0, " (arquitectura " & archIntg & " de 4)", "")
                                Case 3
                                    MenuDesc.Text = "Préparation de la copie des outils de déploiement en cours..." & If(adkCopyArg = 0, " (architecture " & archIntg & " de 4)", "")
                            End Select
                            Dim CurrentFileInt As Integer = 0
                            For Each folder In My.Computer.FileSystem.GetDirectories(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\amd64", FileIO.SearchOption.SearchAllSubDirectories)
                                Directory.CreateDirectory(folder.Replace(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\amd64", projPath & "\DandI\amd64"))
                            Next
                            For Each archFile In My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\amd64", FileIO.SearchOption.SearchAllSubDirectories)
                                ADKCopierBW.ReportProgress(Math.Round(CurrentFileInt / fileCount, 2) * 100)
                                File.Copy(archFile, archFile.Replace(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\amd64", projPath & "\DandI\amd64"), True)
                                CurrentFileInt += 1
                            Next
                        Case 3
                            ' Copy ARM architecture
                            ' Count files
                            Dim fileCount As Integer = My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\arm", FileIO.SearchOption.SearchAllSubDirectories).Count
                            Select Case Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            MenuDesc.Text = "Preparing to copy deployment tools..." & If(adkCopyArg = 0, " (architecture " & archIntg & " of 4)", "")
                                        Case "ESN"
                                            MenuDesc.Text = "Preparándonos para copiar las herramientas de implementación..." & If(adkCopyArg = 0, " (arquitectura " & archIntg & " de 4)", "")
                                        Case "FRA"
                                            MenuDesc.Text = "Préparation de la copie des outils de déploiement en cours..." & If(adkCopyArg = 0, " (architecture " & archIntg & " de 4)", "")
                                    End Select
                                Case 1
                                    MenuDesc.Text = "Preparing to copy deployment tools..." & If(adkCopyArg = 0, " (architecture " & archIntg & " of 4)", "")
                                Case 2
                                    MenuDesc.Text = "Preparándonos para copiar las herramientas de implementación..." & If(adkCopyArg = 0, " (arquitectura " & archIntg & " de 4)", "")
                                Case 3
                                    MenuDesc.Text = "Préparation de la copie des outils de déploiement en cours..." & If(adkCopyArg = 0, " (architecture " & archIntg & " de 4)", "")
                            End Select
                            Dim CurrentFileInt As Integer = 0
                            For Each folder In My.Computer.FileSystem.GetDirectories(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\arm", FileIO.SearchOption.SearchAllSubDirectories)
                                Directory.CreateDirectory(folder.Replace(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\arm", projPath & "\DandI\arm"))
                            Next
                            For Each archFile In My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\arm", FileIO.SearchOption.SearchAllSubDirectories)
                                ADKCopierBW.ReportProgress(Math.Round(CurrentFileInt / fileCount, 2) * 100)
                                File.Copy(archFile, archFile.Replace(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\arm", projPath & "\DandI\arm"), True)
                                CurrentFileInt += 1
                            Next
                        Case 4
                            ' Copy ARM64 architecture
                            ' Count files
                            Dim fileCount As Integer = My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\arm64", FileIO.SearchOption.SearchAllSubDirectories).Count
                            Select Case Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            MenuDesc.Text = "Preparing to copy deployment tools..." & If(adkCopyArg = 0, " (architecture " & archIntg & " of 4)", "")
                                        Case "ESN"
                                            MenuDesc.Text = "Preparándonos para copiar las herramientas de implementación..." & If(adkCopyArg = 0, " (arquitectura " & archIntg & " de 4)", "")
                                        Case "FRA"
                                            MenuDesc.Text = "Préparation de la copie des outils de déploiement en cours..." & If(adkCopyArg = 0, " (architecture " & archIntg & " de 4)", "")
                                    End Select
                                Case 1
                                    MenuDesc.Text = "Preparing to copy deployment tools..." & If(adkCopyArg = 0, " (architecture " & archIntg & " of 4)", "")
                                Case 2
                                    MenuDesc.Text = "Preparándonos para copiar las herramientas de implementación..." & If(adkCopyArg = 0, " (arquitectura " & archIntg & " de 4)", "")
                                Case 3
                                    MenuDesc.Text = "Préparation de la copie des outils de déploiement en cours..." & If(adkCopyArg = 0, " (architecture " & archIntg & " de 4)", "")
                            End Select
                            Dim CurrentFileInt As Integer = 0
                            For Each folder In My.Computer.FileSystem.GetDirectories(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\arm64", FileIO.SearchOption.SearchAllSubDirectories)
                                Directory.CreateDirectory(folder.Replace(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\arm64", projPath & "\DandI\arm64"))
                            Next
                            For Each archFile In My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\arm64", FileIO.SearchOption.SearchAllSubDirectories)
                                ADKCopierBW.ReportProgress(Math.Round(CurrentFileInt / fileCount, 2) * 100)
                                File.Copy(archFile, archFile.Replace(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\arm64", projPath & "\DandI\arm64"), True)
                                CurrentFileInt += 1
                            Next
                    End Select
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub OfAllArchitecturesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OfAllArchitecturesToolStripMenuItem.Click
        adkCopyArg = 0
        ADKCopierBW.RunWorkerAsync()
    End Sub

    Private Sub OfSelectedArchitectureToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OfSelectedArchitectureToolStripMenuItem.Click
        If prjTreeView.SelectedNode.Name.EndsWith("x86") Then
            adkCopyArg = 1
        ElseIf prjTreeView.SelectedNode.Name.EndsWith("amd64") Then
            adkCopyArg = 2
        ElseIf prjTreeView.SelectedNode.Name.EndsWith("arm") Then
            adkCopyArg = 3
        ElseIf prjTreeView.SelectedNode.Name.EndsWith("arm64") Then
            adkCopyArg = 4
        End If
        ADKCopierBW.RunWorkerAsync()
    End Sub

    Private Sub ForX86ArchitectureToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ForX86ArchitectureToolStripMenuItem.Click
        adkCopyArg = 1
        ADKCopierBW.RunWorkerAsync()
    End Sub

    Private Sub ForAmd64ArchitectureToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ForAmd64ArchitectureToolStripMenuItem.Click
        adkCopyArg = 2
        ADKCopierBW.RunWorkerAsync()
    End Sub

    Private Sub ForARMArchitectureToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ForARMArchitectureToolStripMenuItem.Click
        adkCopyArg = 3
        ADKCopierBW.RunWorkerAsync()
    End Sub

    Private Sub ForARM64ArchitectureToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ForARM64ArchitectureToolStripMenuItem.Click
        adkCopyArg = 4
        ADKCopierBW.RunWorkerAsync()
    End Sub

    Private Sub ADKCopierBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ADKCopierBW.RunWorkerCompleted
        Try
            ' Detect if ADKs are present
            Dim adkInst As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\WIMMount")
            Dim adk As String = adkInst.GetValue("AdkInstallation").ToString()
            adkInst.Close()
            If adk = "1" Then
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MenuDesc.Text = "Deployment tools were copied to the project successfully"
                            Case "ESN"
                                MenuDesc.Text = "Las herramientas de implementación fueron copiadas al proyecto satisfactoriamente"
                            Case "FRA"
                                MenuDesc.Text = "Les outils de déploiement ont été copiés dans le projet avec succès."
                        End Select
                    Case 1
                        MenuDesc.Text = "Deployment tools were copied to the project successfully"
                    Case 2
                        MenuDesc.Text = "Las herramientas de implementación fueron copiadas al proyecto satisfactoriamente"
                    Case 3
                        MenuDesc.Text = "Les outils de déploiement ont été copiés dans le projet avec succès."
                End Select
            ElseIf adk <> "1" Then
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MenuDesc.Text = "Deployment tools aren't present on this system"
                            Case "ESN"
                                MenuDesc.Text = "Las herramientas de implementación no están presentes en este sistema"
                            Case "FRA"
                                MenuDesc.Text = "Les outils de déploiement ne sont pas présents sur ce système."
                        End Select
                    Case 1
                        MenuDesc.Text = "Deployment tools aren't present on this system"
                    Case 2
                        MenuDesc.Text = "Las herramientas de implementación no están presentes en este sistema"
                    Case 3
                        MenuDesc.Text = "Les outils de déploiement ne sont pas présents sur ce système."
                End Select
            End If
        Catch ex As Exception
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MenuDesc.Text = "Deployment tools could not be copied"
                        Case "ESN"
                            MenuDesc.Text = "Las herramientas de implementación no pudieron ser copiadas"
                        Case "FRA"
                            MenuDesc.Text = "Les outils de déploiement n'ont pas pu être copiés."
                    End Select
                Case 1
                    MenuDesc.Text = "Deployment tools could not be copied"
                Case 2
                    MenuDesc.Text = "Las herramientas de implementación no pudieron ser copiadas"
                Case 3
                    MenuDesc.Text = "Les outils de déploiement n'ont pas pu être copiés."
            End Select
        End Try
    End Sub

    Private Sub ADKCopierBW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles ADKCopierBW.ProgressChanged
        Select Case adkCopyArg
            Case 0
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MenuDesc.Text = "Copying deployment tools for architecture (" & currentArch & ", " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " of 4)...", ")...")
                            Case "ESN"
                                MenuDesc.Text = "Copiando herramientas de implementación para la arquitectura (" & currentArch & ", " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", arquitectura " & archIntg & " de 4)...", ")...")
                            Case "FRA"
                                MenuDesc.Text = "Copie des outils de déploiement pour l'architecture en cours (" & currentArch & ", " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " de 4)...", ") ...")
                        End Select
                    Case 1
                        MenuDesc.Text = "Copying deployment tools for architecture (" & currentArch & ", " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " of 4)...", ")...")
                    Case 2
                        MenuDesc.Text = "Copiando herramientas de implementación para la arquitectura (" & currentArch & ", " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", arquitectura " & archIntg & " de 4)...", ")...")
                    Case 3
                        MenuDesc.Text = "Copie des outils de déploiement pour l'architecture en cours (" & currentArch & ", " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " de 4)...", ") ...")
                End Select
            Case 1
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MenuDesc.Text = "Copying deployment tools for architecture (x86, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " of 4)...", ")...")
                            Case "ESN"
                                MenuDesc.Text = "Copiando herramientas de implementación para la arquitectura (x86, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", arquitectura " & archIntg & " de 4)...", ")...")
                            Case "FRA"
                                MenuDesc.Text = "Copie des outils de déploiement pour l'architecture en cours (x86," & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " de 4)...", ") ...")
                        End Select
                    Case 1
                        MenuDesc.Text = "Copying deployment tools for architecture (x86, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " of 4)...", ")...")
                    Case 2
                        MenuDesc.Text = "Copiando herramientas de implementación para la arquitectura (x86, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", arquitectura " & archIntg & " de 4)...", ")...")
                    Case 3
                        MenuDesc.Text = "Copie des outils de déploiement pour l'architecture en cours (x86," & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " de 4)...", ") ...")
                End Select
            Case 2
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MenuDesc.Text = "Copying deployment tools for architecture (amd64, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " of 4)...", ")...")
                            Case "ESN"
                                MenuDesc.Text = "Copiando herramientas de implementación para la arquitectura (amd64, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", arquitectura " & archIntg & " de 4)...", ")...")
                            Case "FRA"
                                MenuDesc.Text = "Copie des outils de déploiement pour l'architecture en cours (amd64," & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " de 4)...", ") ...")
                        End Select
                    Case 1
                        MenuDesc.Text = "Copying deployment tools for architecture (amd64, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " of 4)...", ")...")
                    Case 2
                        MenuDesc.Text = "Copiando herramientas de implementación para la arquitectura (amd64, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", arquitectura " & archIntg & " de 4)...", ")...")
                    Case 3
                        MenuDesc.Text = "Copie des outils de déploiement pour l'architecture en cours (amd64," & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " de 4)...", ") ...")
                End Select
            Case 3
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MenuDesc.Text = "Copying deployment tools for architecture (arm, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " of 4)...", ")...")
                            Case "ESN"
                                MenuDesc.Text = "Copiando herramientas de implementación para la arquitectura (arm, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", arquitectura " & archIntg & " de 4)...", ")...")
                            Case "FRA"
                                MenuDesc.Text = "Copie des outils de déploiement pour l'architecture en cours (arm," & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " de 4)...", ") ...")
                        End Select
                    Case 1
                        MenuDesc.Text = "Copying deployment tools for architecture (arm, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " of 4)...", ")...")
                    Case 2
                        MenuDesc.Text = "Copiando herramientas de implementación para la arquitectura (arm, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", arquitectura " & archIntg & " de 4)...", ")...")
                    Case 3
                        MenuDesc.Text = "Copie des outils de déploiement pour l'architecture en cours (arm," & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " de 4)...", ") ...")
                End Select
            Case 4
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MenuDesc.Text = "Copying deployment tools for architecture (arm64, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " of 4)...", ")...")
                            Case "ESN"
                                MenuDesc.Text = "Copiando herramientas de implementación para la arquitectura (arm64, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", arquitectura " & archIntg & " de 4)...", ")...")
                            Case "FRA"
                                MenuDesc.Text = "Copie des outils de déploiement pour l'architecture en cours (arm64," & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " de 4)...", ") ...")
                        End Select
                    Case 1
                        MenuDesc.Text = "Copying deployment tools for architecture (arm64, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " of 4)...", ")...")
                    Case 2
                        MenuDesc.Text = "Copiando herramientas de implementación para la arquitectura (arm64, " & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", arquitectura " & archIntg & " de 4)...", ")...")
                    Case 3
                        MenuDesc.Text = "Copie des outils de déploiement pour l'architecture en cours (arm64," & e.ProgressPercentage & "%" & If(adkCopyArg = 0, ", architecture " & archIntg & " de 4)...", ") ...")
                End Select
        End Select
    End Sub

    Private Sub ExpandToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExpandToolStripMenuItem.Click
        ExpandCollapseTSB.PerformClick()
    End Sub

    Private Sub AccessDirectoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AccessDirectoryToolStripMenuItem.Click
        If prjTreeView.SelectedNode.Name = "parent" Then
            Process.Start(projPath)
        ElseIf prjTreeView.SelectedNode.Name = "dandi" Then
            Process.Start(projPath & "\dandi")
        ElseIf prjTreeView.SelectedNode.Name.EndsWith("x86") Then
            Process.Start(projPath & "\dandi\x86")
        ElseIf prjTreeView.SelectedNode.Name.EndsWith("amd64") Then
            Process.Start(projPath & "\dandi\amd64")
        ElseIf prjTreeView.SelectedNode.Name.EndsWith("arm") Then
            Process.Start(projPath & "\dandi\arm")
        ElseIf prjTreeView.SelectedNode.Name.EndsWith("arm64") Then
            Process.Start(projPath & "\dandi\arm64")
        ElseIf prjTreeView.SelectedNode.Name = "mount" Then
            Process.Start(projPath & "\mount")
        ElseIf prjTreeView.SelectedNode.Name = "unattend_xml" Then
            Process.Start(projPath & "\unattend_xml")
        ElseIf prjTreeView.SelectedNode.Name = "scr_temp" Then
            Process.Start(projPath & "\scr_temp")
        ElseIf prjTreeView.SelectedNode.Name = "reports" Then
            Process.Start(projPath & "\reports")
        End If
    End Sub

    Private Sub UnloadProjectToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles UnloadProjectToolStripMenuItem1.Click
        ToolStripButton3.PerformClick()
    End Sub

    Private Sub ScratchDirectorySettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScratchDirectorySettingsToolStripMenuItem.Click
        Options.TabControl1.SelectedIndex = 4
        Options.ShowDialog()
    End Sub

    Private Sub ManageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageToolStripMenuItem.Click
        UnattendMgr.Show()
    End Sub

    Private Sub CreationWizardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreationWizardToolStripMenuItem.Click
        NewUnattendWiz.Show()
    End Sub

    Private Sub MountImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MountImageToolStripMenuItem.Click
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        ImgMount.ShowDialog()
    End Sub

    Private Sub UnmountImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnmountImageToolStripMenuItem.Click
        ImgUMount.RadioButton1.Checked = True
        ImgUMount.RadioButton2.Checked = False
        ImgUMount.ShowDialog()
    End Sub

    Private Sub RemoveVolumeImagesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveVolumeImagesToolStripMenuItem.Click
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Threading.Thread.Sleep(100)
        End While
        For x = 0 To Array.LastIndexOf(MountedImageMountDirs, MountedImageMountDirs.Last)
            If MountedImageMountDirs(x) = MountDir Then
                ImgIndexDelete.TextBox1.Text = MountedImageImgFiles(x)
                Exit For
            End If
        Next
        ImgIndexDelete.ShowDialog()
    End Sub

    Private Sub SwitchImageIndexesToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SwitchImageIndexesToolStripMenuItem1.Click
        MountedImageDetectorBW.CancelAsync()
        ProgressPanel.OperationNum = 995
        PleaseWaitDialog.indexesSourceImg = SourceImg
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting image indexes..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo índices de la imagen..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des index de l'image en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting image indexes..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo índices de la imagen..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des index de l'image en cours..."
        End Select
        PleaseWaitDialog.ShowDialog(Me)
        If Not MountedImageDetectorBW.IsBusy Then Call MountedImageDetectorBW.RunWorkerAsync()
        If PleaseWaitDialog.imgIndexes > 1 Then
            ImgIndexSwitch.ShowDialog()
        End If
    End Sub

    Private Sub ManageOnlineInstallationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageOnlineInstallationToolStripMenuItem.Click
        Dim showMessage As Boolean = isProjectLoaded
        If isProjectLoaded Then
            ActiveInstAccessWarn.Label2.Visible = True
            ActiveInstAccessWarn.ShowDialog()
            If ActiveInstAccessWarn.DialogResult = Windows.Forms.DialogResult.OK Then UnloadDTProj(False, True, False)
            If ImgBW.IsBusy Then Exit Sub
        End If
        ActiveInstAccessWarn.Label2.Visible = False
        BeginOnlineManagement(Not showMessage)
    End Sub

    Private Sub ManageOfflineInstallationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageOfflineInstallationToolStripMenuItem.Click
        If OfflineInstDriveLister.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If drivePath = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) Then
                Exit Sub
            End If
            If isProjectLoaded Then
                UnloadDTProj(False, True, False)
                If ImgBW.IsBusy Then Exit Sub
            End If
            BeginOfflineManagement(drivePath)
        End If
    End Sub

    Private Sub UpdCheckupPanel_Paint(sender As Object, e As PaintEventArgs)
        ControlPaint.DrawBorder(e.Graphics, Panel1.ClientRectangle, Color.FromArgb(0, 122, 204), ButtonBorderStyle.Solid)
    End Sub

    Private Sub UpdCheckerBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles UpdCheckerBW.DoWork
        CheckForUpdates(dtBranch)
    End Sub

    Private Sub RemountImageWithWritePermissionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemountImageWithWritePermissionsToolStripMenuItem.Click
        ' Go through each mounted image until we find it
        If MountedImageMountDirs.Count > 0 Then
            If MountedImageMountDirs.Contains(MountDir) Then
                For x = 0 To Array.LastIndexOf(MountedImageMountDirs, MountedImageMountDirs.Last)
                    If MountedImageMountDirs(x) = MountDir Then
                        EnableWritePermissions(MountedImageImgFiles(x), CInt(MountedImageImgIndexes(x)), MountedImageMountDirs(x))
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Sub EnableWritePermissions(SourceImage As String, SourceIndex As Integer, DestinationPath As String)
        If File.Exists(SourceImage) Then
            If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
            ' Configure settings to remount image with write permissions

            ' Unmount image discarding changes
            ProgressPanel.UMountLocalDir = True
            ProgressPanel.MountDir = DestinationPath
            ProgressPanel.UMountImgIndex = SourceIndex
            ProgressPanel.UMountOp = 1

            ' Mount the same image to the same directory with (hopefully) write permissions
            ProgressPanel.SourceImg = SourceImage
            ProgressPanel.ImgIndex = SourceIndex
            ProgressPanel.isReadOnly = False
            ProgressPanel.isOptimized = False
            ProgressPanel.isIntegrityTested = False

            ' Add tasks to task list
            ProgressPanel.TaskList.AddRange({21, 15})
            ProgressPanel.OperationNum = 15

            If WindowState = FormWindowState.Minimized Then WindowState = FormWindowState.Normal
            ProgressPanel.ShowDialog(Me)

            If isProjectLoaded And IsImageMounted And MountDir = DestinationPath Then
                UpdateProjProperties(True, False, False)
            Else
                If Not MountedImageDetectorBW.IsBusy Then Call MountedImageDetectorBW.RunWorkerAsync()
            End If
        End If
    End Sub

    Private Sub GetDrivers_Click(sender As Object, e As EventArgs) Handles GetDrivers.Click
        ProgressPanel.OperationNum = 994
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting installed driver packages..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo paquetes de controladores instalados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des paquets de pilotes installés en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting installed driver packages..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo paquetes de controladores instalados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des paquets de pilotes installés en cours..."
        End Select
        If Not CompletedTasks(4) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(500)
        End While
        If DriverInfoList IsNot Nothing Then GetDriverInfo.InstalledDriverInfo = DriverInfoList
        GetDriverInfo.ShowDialog()
    End Sub

    Private Sub ViewProjectFilesInFileExplorerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewProjectFilesInFileExplorerToolStripMenuItem.Click
        ExplorerView.PerformClick()
    End Sub

    Private Sub WimScriptEditorCommand_Click(sender As Object, e As EventArgs) Handles WimScriptEditorCommand.Click
        WimScriptEditor.MinimizeBox = True
        WimScriptEditor.MaximizeBox = True
        If WimScriptEditor.Visible Then
            If WimScriptEditor.WindowState = FormWindowState.Minimized Then
                WimScriptEditor.WindowState = FormWindowState.Normal
            Else
                WimScriptEditor.BringToFront()
            End If
            WimScriptEditor.Focus()
        Else
            WimScriptEditor.Show()
        End If
    End Sub

    Private Sub GetFeatures_Click(sender As Object, e As EventArgs) Handles GetFeatures.Click
        If Not IsImageMounted Then Exit Sub
        ProgressPanel.OperationNum = 994
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
        End Select
        If Not CompletedTasks(1) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(500)
        End While
        If FeatureInfoList IsNot Nothing Then GetFeatureInfoDlg.InstalledFeatureInfo = FeatureInfoList
        GetFeatureInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub GetCapabilities_Click(sender As Object, e As EventArgs) Handles GetCapabilities.Click
        If imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Or Not IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
            End Select
            Exit Sub
        End If
        ProgressPanel.OperationNum = 994
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting capability names and their state..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de funcionalidades y sus estados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms des capacités et de leur état en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting capability names and their state..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de funcionalidades y sus estados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms des capacités et de leur état en cours..."
        End Select
        If Not CompletedTasks(3) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(500)
        End While
        If CapabilityInfoList IsNot Nothing Then GetCapabilityInfoDlg.InstalledCapabilityInfo = CapabilityInfoList
        GetCapabilityInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub GetPackages_Click(sender As Object, e As EventArgs) Handles GetPackages.Click
        ProgressPanel.OperationNum = 993
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting package names..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting package names..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
        End Select
        If Not CompletedTasks(0) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        If MountedImageDetectorBW.IsBusy Then
            MountedImageDetectorBW.CancelAsync()
            While MountedImageDetectorBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(500)
            End While
        End If
        If PackageInfoList IsNot Nothing Then GetPkgInfoDlg.InstalledPkgInfo = PackageInfoList
        GetPkgInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub GetProvisionedAppxPackages_Click(sender As Object, e As EventArgs) Handles GetProvisionedAppxPackages.Click
        If imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Or Not IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
            End Select
            Exit Sub
        End If
        ProgressPanel.OperationNum = 993
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting package names..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting package names..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
        End Select
        If Not CompletedTasks(2) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        If AppxPackageInfoList IsNot Nothing Then GetAppxPkgInfoDlg.InstalledAppxPkgInfo = AppxPackageInfoList
        GetAppxPkgInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub SaveResourceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveResourceToolStripMenuItem.Click
        If OnlineManagement Then AppxResSFD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) Else AppxResSFD.InitialDirectory = projPath
        AppxResSFD.FileName = If(GetAppxPkgInfoDlg.displayName <> "", GetAppxPkgInfoDlg.displayName, GetAppxPkgInfoDlg.Label25.Text)
        AppxResSFD.ShowDialog()
    End Sub

    Private Sub AppxResSFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles AppxResSFD.FileOk
        Try
            GetAppxPkgInfoDlg.PictureBox2.Image.Save(AppxResSFD.FileName, Imaging.ImageFormat.Png)
            Notifications.Visible = True
            Notifications.Icon = Icon
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Notifications.BalloonTipText = "The asset has been saved to the location you specified"
                            Notifications.BalloonTipTitle = "Save successful"
                        Case "ESN"
                            Notifications.BalloonTipText = "El recurso ha sido guardado en la ubicación que especificó"
                            Notifications.BalloonTipTitle = "Guardado satisfactorio"
                        Case "FRA"
                            Notifications.BalloonTipText = "Le fichier a été sauvegardé à l'emplacement que vous avez spécifié."
                            Notifications.BalloonTipTitle = "Sauvegarde du fichier réussie"
                    End Select
                Case 1
                    Notifications.BalloonTipText = "The asset has been saved to the location you specified"
                    Notifications.BalloonTipTitle = "Save successful"
                Case 2
                    Notifications.BalloonTipText = "El recurso ha sido guardado en la ubicación que especificó"
                    Notifications.BalloonTipTitle = "Guardado satisfactorio"
                Case 3
                    Notifications.BalloonTipText = "Le fichier a été sauvegardé à l'emplacement que vous avez spécifié."
                    Notifications.BalloonTipTitle = "Sauvegarde du fichier réussie"
            End Select
            Notifications.ShowBalloonTip(3000)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        Try
            Dim data As New DataObject()
            data.SetImage(GetAppxPkgInfoDlg.PictureBox2.Image)
            Clipboard.SetDataObject(data, True)
            Notifications.Visible = True
            Notifications.Icon = Icon
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Notifications.BalloonTipText = "The asset has been copied to the clipboard"
                            Notifications.BalloonTipTitle = "Copy successful"
                        Case "ESN"
                            Notifications.BalloonTipText = "El recurso ha sido copiado al portapapeles"
                            Notifications.BalloonTipTitle = "Copia satisfactoria"
                        Case "FRA"
                            Notifications.BalloonTipText = "Le fichier a été copié dans le presse-papiers."
                            Notifications.BalloonTipTitle = "Copie du fichier réussie"
                    End Select
                Case 1
                    Notifications.BalloonTipText = "The asset has been copied to the clipboard"
                    Notifications.BalloonTipTitle = "Copy successful"
                Case 2
                    Notifications.BalloonTipText = "El recurso ha sido copiado al portapapeles"
                    Notifications.BalloonTipTitle = "Copia satisfactoria"
                Case 3
                    Notifications.BalloonTipText = "Le fichier a été copié dans le presse-papiers."
                    Notifications.BalloonTipTitle = "Copie du fichier réussie"
            End Select
            Notifications.ShowBalloonTip(3000)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Notifications_BalloonTipClosed(sender As Object, e As EventArgs) Handles Notifications.BalloonTipClosed
        Notifications.Visible = False
    End Sub

    Private Sub SplitImage_Click(sender As Object, e As EventArgs) Handles SplitImage.Click
        ImgSplit.ShowDialog()
    End Sub

    Private Sub Notifications_BalloonTipClicked(sender As Object, e As EventArgs) Handles Notifications.BalloonTipClicked
        If Notifications.BalloonTipText.Contains("saved") Or Notifications.BalloonTipText.Contains("guardado") Or Notifications.BalloonTipText.Contains("sauvegardé") Then
            If File.Exists(AppxResSFD.FileName) Then
                Process.Start(AppxResSFD.FileName)
            End If
        End If
    End Sub

    Private Sub ExportDriver_Click(sender As Object, e As EventArgs) Handles ExportDriver.Click
        ExportDrivers.ShowDialog()
    End Sub

    Private Sub GetPESettings_Click(sender As Object, e As EventArgs) Handles GetPESettings.Click
        If ImgBW.IsBusy Then
            BGProcsBusyDialog.ShowDialog()
            Exit Sub
        End If
        GetWinPESettings.ShowDialog()
    End Sub

    Private Sub SetTargetPath_Click(sender As Object, e As EventArgs) Handles SetTargetPath.Click
        If ImgBW.IsBusy Then
            BGProcsBusyDialog.ShowDialog()
            Exit Sub
        End If
        SetPETargetPath.ShowDialog()
    End Sub

    Private Sub SetScratchSpace_Click(sender As Object, e As EventArgs) Handles SetScratchSpace.Click
        If ImgBW.IsBusy Then
            BGProcsBusyDialog.ShowDialog()
            Exit Sub
        End If
        SetPEScratchSpace.ShowDialog()
    End Sub

    Private Sub ISFix_Click(sender As Object, e As EventArgs) Handles ISFix.Click
        InvalidSettingsDialog.ShowDialog()
    End Sub

    Private Sub MicrosoftAppsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MicrosoftAppsToolStripMenuItem.Click
        Process.Start("https://apps.microsoft.com")
    End Sub

    Private Sub MicrosoftStoreGenerationProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MicrosoftStoreGenerationProjectToolStripMenuItem.Click
        Process.Start("https://store.rg-adguard.net")
    End Sub

    Private Sub SaveImageInformationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveImageInformationToolStripMenuItem.Click
        If ImgInfoSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            If MountedImageMountDirs.Count > 0 Then
                For x = 0 To Array.LastIndexOf(MountedImageMountDirs, MountedImageMountDirs.Last)
                    If MountedImageMountDirs(x) = MountDir Then
                        ImgInfoSaveDlg.SourceImage = MountedImageImgFiles(x)
                        Exit For
                    End If
                Next
            End If
            ImgInfoSaveDlg.ImgMountDir = If(Not OnlineManagement, MountDir, "")
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.OfflineMode = OfflineManagement
            ImgInfoSaveDlg.AllDrivers = AllDrivers
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.SaveTask = 0
            ImgInfoSaveDlg.ShowDialog()
        End If
    End Sub

    Private Sub OfflineInstMgmt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles OfflineInstMgmt.LinkClicked
        If OfflineInstDriveLister.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If drivePath = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) Then
                ActiveInstAccessWarn.Label2.Visible = False
                BeginOnlineManagement(True)
                Exit Sub
            End If
            BeginOfflineManagement(drivePath)
        End If
    End Sub

    Private Sub ContributeToTheHelpSystemToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ContributeToTheHelpSystemToolStripMenuItem.Click
        Process.Start("https://github.com/CodingWonders/dt_help")
    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        ProjectView.Visible = True
        SplitPanels.Visible = False
        GoToNewView = True
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        ProjectView.Visible = False
        SplitPanels.Visible = True
        GoToNewView = False
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        TimeLabel.Text = DateTime.Now.ToString("D") & " - " & DateTime.Now.ToString("HH:mm")
    End Sub

    Private Sub LinkLabel12_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel12.LinkClicked
        LinkLabel12.LinkColor = Color.FromArgb(241, 241, 241)
        LinkLabel13.LinkColor = Color.FromArgb(153, 153, 153)
        SidePanel_ProjectView.Visible = True
        SidePanel_ImageView.Visible = False
    End Sub

    Private Sub LinkLabel13_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel13.LinkClicked
        LinkLabel12.LinkColor = Color.FromArgb(153, 153, 153)
        LinkLabel13.LinkColor = Color.FromArgb(241, 241, 241)
        SidePanel_ProjectView.Visible = False
        SidePanel_ImageView.Visible = True
    End Sub

#Region "Task Links"

    Private Sub LinkLabel15_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel15.LinkClicked
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        ProjProperties.TabControl1.SelectedIndex = 0
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        ProjProperties.Label1.Text = ProjProperties.TabControl1.SelectedTab.Text & " properties"
                    Case "ESN"
                        ProjProperties.Label1.Text = "Propiedades " & If(ProjProperties.TabControl1.SelectedIndex = 0, "del proyecto", "de la imagen")
                    Case "FRA"
                        ProjProperties.Label1.Text = "Propriétés " & If(ProjProperties.TabControl1.SelectedIndex = 0, "du projet", "de l'image")
                End Select
            Case 1
                ProjProperties.Label1.Text = ProjProperties.TabControl1.SelectedTab.Text & " properties"
            Case 2
                ProjProperties.Label1.Text = "Propiedades " & If(ProjProperties.TabControl1.SelectedIndex = 0, "del proyecto", "de la imagen")
            Case 3
                ProjProperties.Label1.Text = "Propriétés " & If(ProjProperties.TabControl1.SelectedIndex = 0, "du projet", "de l'image")
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
            ProjProperties.Text = ""
        Else
            ProjProperties.Text = ProjProperties.Label1.Text
        End If
        ProjProperties.ShowDialog()
    End Sub

    Private Sub LinkLabel16_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel16.LinkClicked
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", projPath)
    End Sub

    Private Sub LinkLabel17_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel17.LinkClicked
        ToolStripButton3.PerformClick()
    End Sub

    Private Sub LinkLabel18_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel18.LinkClicked
        PopupImageManager.Location = LinkLabel18.PointToScreen(Point.Empty)
        PopupImageManager.Top -= PopupImageManager.Height
        If PopupImageManager.ShowDialog() = DialogResult.OK Then
            If MountedImageMountDirs.Count > 0 Then
                MountDir = PopupImageManager.selectedMntDir
                If MountedImageMountDirs.Count > 0 Then
                    Try
                        For x = 0 To Array.LastIndexOf(MountedImageMountDirs, MountedImageMountDirs.Last)
                            If MountedImageMountDirs(x) = MountDir Then
                                ImgIndex = MountedImageImgIndexes(x)
                                SourceImg = MountedImageImgFiles(x)
                                IIf(MountedImageMountedReWr(x) = 1, isReadOnly = False, isReadOnly = True)
                            End If
                        Next
                    Catch ex As Exception
                        Exit Try
                    End Try
                    UpdateProjProperties(True, If(isReadOnly, True, False))
                    SaveDTProj()
                End If
            Else
                Exit Sub
            End If
        End If
    End Sub

    Private Sub LinkLabel19_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel19.LinkClicked
        ' If it's a read only image, directly unmount it discarding changes
        If MountedImageImgFiles.Count > 0 Then
            For x = 0 To Array.LastIndexOf(MountedImageImgFiles, MountedImageImgFiles.Last)
                If MountedImageMountDirs(x) = MountDir Then
                    If MountedImageMountedReWr(x) = 1 Then
                        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
                        imgCommitOperation = 1
                        UnloadDTProj(False, True, True)
                        Exit Sub
                    End If
                End If
            Next
        End If
        ImgUMount.RadioButton1.Checked = True
        ImgUMount.RadioButton2.Checked = False
        ImgUMount.ShowDialog()
    End Sub

    Private Sub LinkLabel20_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel20.LinkClicked
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        ProjProperties.TabControl1.SelectedIndex = 1
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        ProjProperties.Label1.Text = ProjProperties.TabControl1.SelectedTab.Text & " properties"
                    Case "ESN"
                        ProjProperties.Label1.Text = "Propiedades " & If(ProjProperties.TabControl1.SelectedIndex = 0, "del proyecto", "de la imagen")
                    Case "FRA"
                        ProjProperties.Label1.Text = "Propriétés " & If(ProjProperties.TabControl1.SelectedIndex = 0, "du projet", "de l'image")
                End Select
            Case 1
                ProjProperties.Label1.Text = ProjProperties.TabControl1.SelectedTab.Text & " properties"
            Case 2
                ProjProperties.Label1.Text = "Propiedades " & If(ProjProperties.TabControl1.SelectedIndex = 0, "del proyecto", "de la imagen")
            Case 3
                ProjProperties.Label1.Text = "Propriétés " & If(ProjProperties.TabControl1.SelectedIndex = 0, "du projet", "de l'image")
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
            ProjProperties.Text = ""
        Else
            ProjProperties.Text = ProjProperties.Label1.Text
        End If
        ProjProperties.ShowDialog()
    End Sub

    Private Sub LinkLabel21_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel21.LinkClicked
        ImgMount.ShowDialog()
    End Sub

#End Region

#Region "Common Task button functionality in new design"

    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles Button24.Click
        MountedImageDetectorBW.CancelAsync()
        ProgressPanel.OperationNum = 995
        PleaseWaitDialog.indexesSourceImg = SourceImg
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting image indexes..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo índices de la imagen..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des index de l'image en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting image indexes..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo índices de la imagen..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des index de l'image en cours..."
        End Select
        PleaseWaitDialog.ShowDialog(Me)
        If Not MountedImageDetectorBW.IsBusy Then Call MountedImageDetectorBW.RunWorkerAsync()
        If PleaseWaitDialog.imgIndexes > 1 Then
            ImgIndexSwitch.ShowDialog()
        End If
    End Sub

    Private Sub Button25_Click(sender As Object, e As EventArgs) Handles Button25.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MountDir
        ProgressPanel.OperationNum = 18
        ProgressPanel.ShowDialog(Me)
    End Sub

    Private Sub Button26_Click(sender As Object, e As EventArgs) Handles Button26.Click
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        ImgMount.ShowDialog()
    End Sub

    Private Sub Button27_Click(sender As Object, e As EventArgs) Handles Button27.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MountDir
        ' TODO: Add additional options later
        ProgressPanel.OperationNum = 8
        ProgressPanel.ShowDialog(Me)
    End Sub

    Private Sub Button28_Click(sender As Object, e As EventArgs) Handles Button28.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        imgCommitOperation = 0
        UnloadDTProj(False, True, True)
    End Sub

    Private Sub Button29_Click(sender As Object, e As EventArgs) Handles Button29.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        imgCommitOperation = 1
        UnloadDTProj(False, True, True)
    End Sub

    Private Sub Button30_Click(sender As Object, e As EventArgs) Handles Button30.Click
        ImgApply.ShowDialog()
    End Sub

    Private Sub Button31_Click(sender As Object, e As EventArgs) Handles Button31.Click
        ImgCapture.ShowDialog()
    End Sub

    Private Sub Button32_Click(sender As Object, e As EventArgs) Handles Button32.Click
        ImgIndexDelete.ShowDialog()
    End Sub

    Private Sub Button33_Click(sender As Object, e As EventArgs) Handles Button33.Click
        If ImgInfoSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            If MountedImageMountDirs.Count > 0 Then
                For x = 0 To Array.LastIndexOf(MountedImageMountDirs, MountedImageMountDirs.Last)
                    If MountedImageMountDirs(x) = MountDir Then
                        ImgInfoSaveDlg.SourceImage = MountedImageImgFiles(x)
                        Exit For
                    End If
                Next
            End If
            ImgInfoSaveDlg.ImgMountDir = If(Not OnlineManagement, MountDir, "")
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.OfflineMode = OfflineManagement
            ImgInfoSaveDlg.AllDrivers = AllDrivers
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.SaveTask = 0
            ImgInfoSaveDlg.ShowDialog()
        End If
    End Sub

    Private Sub Button34_Click(sender As Object, e As EventArgs) Handles Button34.Click
        ProgressPanel.OperationNum = 993
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting package names..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting package names..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
        End Select
        If Not CompletedTasks(0) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        If MountedImageDetectorBW.IsBusy Then
            MountedImageDetectorBW.CancelAsync()
            While MountedImageDetectorBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(500)
            End While
        End If
        If PackageInfoList IsNot Nothing Then GetPkgInfoDlg.InstalledPkgInfo = PackageInfoList
        GetPkgInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub Button35_Click(sender As Object, e As EventArgs) Handles Button35.Click
        ElementCount = 0
        RemPackage.CheckedListBox1.Items.Clear()
        ProgressPanel.OperationNum = 993
        PleaseWaitDialog.pkgSourceImgStr = MountDir
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting package names..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting package names..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
        End Select
        If Not CompletedTasks(0) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Try
            For x = 0 To Array.LastIndexOf(imgPackageNames, imgPackageNames.Last)
                If imgPackageNames(x) = "" Then
                    Continue For
                End If
                RemPackage.CheckedListBox1.Items.Add(imgPackageNames(x))
            Next
        Catch ex As Exception
            ' We should have enough with the entries already added.
            Exit Try
        End Try
        Try
            For x = 0 To Array.LastIndexOf(imgPackageNames, imgPackageNames.Last)
                If imgPackageNames(x) = "" Then
                    Exit For
                End If
                ElementCount += 1
            Next
        Catch ex As Exception
            ' We should have enough with the entries already added.
            Exit Try
        End Try
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        RemPackage.Label2.Text = "This image contains " & ElementCount & " packages"
                    Case "ESN"
                        RemPackage.Label2.Text = "Esta imagen contiene " & ElementCount & " paquetes"
                    Case "FRA"
                        RemPackage.Label2.Text = "Cette image contient " & ElementCount & " paquets"
                End Select
            Case 1
                RemPackage.Label2.Text = "This image contains " & ElementCount & " packages"
            Case 2
                RemPackage.Label2.Text = "Esta imagen contiene " & ElementCount & " paquetes"
            Case 3
                RemPackage.Label2.Text = "Cette image contient " & ElementCount & " paquets"
        End Select
        RemPackage.ShowDialog()
    End Sub

    Private Sub Button36_Click(sender As Object, e As EventArgs) Handles Button36.Click
        AddPackageDlg.ShowDialog()
    End Sub

    Private Sub Button37_Click(sender As Object, e As EventArgs) Handles Button37.Click
        ImgCleanup.ShowDialog()
    End Sub

    Private Sub Button38_Click(sender As Object, e As EventArgs) Handles Button38.Click
        If ImgInfoSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            If ImgInfoSaveDlg.PackageFiles.Count > 0 Then ImgInfoSaveDlg.PackageFiles.Clear()
            ImgInfoSaveDlg.SourceImage = SourceImg
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            ImgInfoSaveDlg.ImgMountDir = If(Not OnlineManagement, MountDir, "")
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.SaveTask = 2
            ImgInfoSaveDlg.ShowDialog()
        End If
    End Sub

    Private Sub Button39_Click(sender As Object, e As EventArgs) Handles Button39.Click
        If Not IsImageMounted Then Exit Sub
        ProgressPanel.OperationNum = 994
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
        End Select
        If Not CompletedTasks(1) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(500)
        End While
        If FeatureInfoList IsNot Nothing Then GetFeatureInfoDlg.InstalledFeatureInfo = FeatureInfoList
        GetFeatureInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub Button40_Click(sender As Object, e As EventArgs) Handles Button40.Click
        ElementCount = 0
        EnableFeat.ListView1.Items.Clear()
        DisableFeat.ListView1.Items.Clear()
        ProgressPanel.OperationNum = 994
        PleaseWaitDialog.featOpType = 1
        PleaseWaitDialog.featSourceImg = MountDir
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
        End Select
        If Not CompletedTasks(1) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Select Case PleaseWaitDialog.featOpType
            Case 0
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureState(x).Contains("Enable") Or imgFeatureState(x) = "" Or imgFeatureState(x) = "Nothing" Then
                            Continue For
                        End If
                        EnableFeat.ListView1.Items.Add(imgFeatureNames(x)).SubItems.Add(imgFeatureState(x))
                    Next
                Catch ex As Exception
                    ' We should have enough with the entries already added.
                    Exit Try
                End Try
                ' Get number of available elements
                Dim ElementCount As Integer = 0
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureNames(x) = "" Then
                            Exit For
                        End If
                        ElementCount += 1
                    Next
                Catch ex As Exception
                    Exit Try
                End Try
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                EnableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                            Case "ESN"
                                EnableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                            Case "FRA"
                                EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                        End Select
                    Case 1
                        EnableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                    Case 2
                        EnableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                    Case 3
                        EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                End Select
            Case 1
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureState(x).Contains("Disable") Or imgFeatureState(x) = "" Or imgFeatureState(x) = "Nothing" Then
                            Continue For
                        End If
                        DisableFeat.ListView1.Items.Add(imgFeatureNames(x)).SubItems.Add(imgFeatureState(x))
                    Next
                Catch ex As Exception
                    ' We should have enough with the entries already added.
                    Exit Try
                End Try
                ' Get number of available elements
                Dim ElementCount As Integer
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureNames(x) = "" Then
                            Exit For
                        End If
                        ElementCount += 1
                    Next
                Catch ex As Exception
                    Exit Try
                End Try
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                DisableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                            Case "ESN"
                                DisableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                            Case "FRA"
                                EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                        End Select
                    Case 1
                        DisableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                    Case 2
                        DisableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                    Case 3
                        EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                End Select
        End Select
        DisableFeat.ShowDialog()
    End Sub

    Private Sub Button41_Click(sender As Object, e As EventArgs) Handles Button41.Click
        ElementCount = 0
        EnableFeat.ListView1.Items.Clear()
        DisableFeat.ListView1.Items.Clear()
        ProgressPanel.OperationNum = 994
        PleaseWaitDialog.featOpType = 0
        PleaseWaitDialog.featSourceImg = MountDir
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de características y sus estados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms des caractéristiques et de leur état en cours..."
        End Select
        If Not CompletedTasks(1) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Select Case PleaseWaitDialog.featOpType
            Case 0
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureState(x).Contains("Enable") Or imgFeatureState(x) = "" Or imgFeatureState(x) = "Nothing" Then
                            Continue For
                        End If
                        EnableFeat.ListView1.Items.Add(imgFeatureNames(x)).SubItems.Add(imgFeatureState(x))
                    Next
                Catch ex As Exception
                    ' We should have enough with the entries already added.
                    Exit Try
                End Try
                ' Get number of available elements
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureNames(x) = "" Then
                            Exit For
                        End If
                        ElementCount += 1
                    Next
                Catch ex As Exception
                    Exit Try
                End Try
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                EnableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                            Case "ESN"
                                EnableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                            Case "FRA"
                                EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                        End Select
                    Case 1
                        EnableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                    Case 2
                        EnableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                    Case 3
                        EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                End Select
            Case 1
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureState(x).Contains("Disable") Or imgFeatureState(x) = "" Or imgFeatureState(x) = "Nothing" Then
                            Continue For
                        End If
                        DisableFeat.ListView1.Items.Add(imgFeatureNames(x)).SubItems.Add(imgFeatureState(x))
                    Next
                Catch ex As Exception
                    ' We should have enough with the entries already added.
                    Exit Try
                End Try
                ' Get number of available elements
                Dim ElementCount As Integer
                Try
                    For x = 0 To Array.LastIndexOf(imgFeatureNames, imgFeatureNames.Last)
                        If imgFeatureNames(x) = "" Then
                            Exit For
                        End If
                        ElementCount += 1
                    Next
                Catch ex As Exception
                    Exit Try
                End Try
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                DisableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                            Case "ESN"
                                DisableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                            Case "FRA"
                                EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                        End Select
                    Case 1
                        DisableFeat.Label2.Text = "This image contains " & ElementCount & " features."
                    Case 2
                        DisableFeat.Label2.Text = "Esta imagen contiene " & ElementCount & " características."
                    Case 3
                        EnableFeat.Label2.Text = "Cette image contient " & ElementCount & " caractéristiques."
                End Select
        End Select
        EnableFeat.ShowDialog()
    End Sub

    Private Sub Button42_Click(sender As Object, e As EventArgs) Handles Button42.Click
        If ImgInfoSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = SourceImg
            ImgInfoSaveDlg.ImgMountDir = If(Not OnlineManagement, MountDir, "")
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.OfflineMode = OfflineManagement
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.SaveTask = 4
            ImgInfoSaveDlg.ShowDialog()
        End If
    End Sub

    Private Sub Button43_Click(sender As Object, e As EventArgs) Handles Button43.Click
        If imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Or Not IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
            End Select
            Exit Sub
        End If
        ElementCount = 0
        RemProvAppxPackage.ListView1.Items.Clear()
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting provisioned AppX packages..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo paquetes aprovisionados AppX..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des paquets AppX provisionnés en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting provisioned AppX packages..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo paquetes aprovisionados AppX..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des paquets AppX provisionnés en cours..."
        End Select
        ProgressPanel.OperationNum = 994
        If Not CompletedTasks(2) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Try
            For x = 0 To Array.LastIndexOf(imgAppxPackageNames, imgAppxPackageNames.Last)
                If imgAppxPackageNames(x) = "" Or imgAppxPackageNames(x) = "Nothing" Then
                    Continue For
                Else
                    If Directory.Exists(MountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & imgAppxPackageNames(x)) Then
                        If My.Computer.FileSystem.GetFiles(MountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & imgAppxPackageNames(x), FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").Count = 0 Then
                            RemProvAppxPackage.ListView1.Items.Add(New ListViewItem(New String() {imgAppxPackageNames(x), imgAppxDisplayNames(x), imgAppxArchitectures(x), imgAppxResourceIds(x), imgAppxVersions(x), "No"}))
                        Else
                            RemProvAppxPackage.ListView1.Items.Add(New ListViewItem(New String() {imgAppxPackageNames(x), imgAppxDisplayNames(x), imgAppxArchitectures(x), imgAppxResourceIds(x), imgAppxVersions(x), "Yes"}))
                        End If
                    Else
                        RemProvAppxPackage.ListView1.Items.Add(New ListViewItem(New String() {imgAppxPackageNames(x), imgAppxDisplayNames(x), imgAppxArchitectures(x), imgAppxResourceIds(x), imgAppxVersions(x), "No"}))
                    End If
                End If
            Next
        Catch ex As Exception
            ' We should have enough with the entries already added.
            Exit Try
        End Try
        ' Begin counting
        Try
            For x = 0 To Array.LastIndexOf(imgAppxPackageNames, imgAppxPackageNames.Last)
                If imgAppxPackageNames(x) = "" Then
                    Exit For
                End If
                ElementCount += 1
            Next
        Catch ex As Exception
            Exit Try
        End Try
        If ElementCount <= 0 Then
            ElementCount = RemProvAppxPackage.ListView1.Items.Count
        End If
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        RemProvAppxPackage.Label2.Text = "This image contains " & ElementCount & " AppX packages."
                    Case "ESN"
                        RemProvAppxPackage.Label2.Text = "Esta imagen contiene " & ElementCount & " paquetes AppX."
                    Case "FRA"
                        RemProvAppxPackage.Label2.Text = "Cette image contient " & ElementCount & " paquets AppX."
                End Select
            Case 1
                RemProvAppxPackage.Label2.Text = "This image contains " & ElementCount & " AppX packages."
            Case 2
                RemProvAppxPackage.Label2.Text = "Esta imagen contiene " & ElementCount & " paquetes AppX."
            Case 3
                RemProvAppxPackage.Label2.Text = "Cette image contient " & ElementCount & " paquets AppX."
        End Select
        RemProvAppxPackage.ShowDialog()
    End Sub

    Private Sub Button44_Click(sender As Object, e As EventArgs) Handles Button44.Click
        If Not imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) And IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            AddProvAppxPackage.ShowDialog()
        Else
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
            End Select
        End If
    End Sub

    Private Sub Button45_Click(sender As Object, e As EventArgs) Handles Button45.Click
        If imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Or Not IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
            End Select
            Exit Sub
        End If
        ProgressPanel.OperationNum = 993
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting package names..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting package names..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de paquetes..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms de paquets en cours..."
        End Select
        If Not CompletedTasks(2) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        If AppxPackageInfoList IsNot Nothing Then GetAppxPkgInfoDlg.InstalledAppxPkgInfo = AppxPackageInfoList
        GetAppxPkgInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub Button46_Click(sender As Object, e As EventArgs) Handles Button46.Click
        If ImgInfoSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = SourceImg
            ImgInfoSaveDlg.ImgMountDir = If(Not OnlineManagement, MountDir, "")
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.OfflineMode = OfflineManagement
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.SaveTask = 5
            ImgInfoSaveDlg.ShowDialog()
        End If
    End Sub

    Private Sub Button47_Click(sender As Object, e As EventArgs) Handles Button47.Click
        If imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Or Not IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
            End Select
            Exit Sub
        End If
        ElementCount = 0
        RemCapabilities.ListView1.Items.Clear()
        ProgressPanel.OperationNum = 994
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting capability names and their state..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de funcionalidades y sus estados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms des capacités et de leur état en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting capability names and their state..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de funcionalidades y sus estados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms des capacités et de leur état en cours..."
        End Select
        If Not CompletedTasks(3) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Try
            For x = 0 To Array.LastIndexOf(imgCapabilityIds, imgCapabilityIds.Last)
                If imgCapabilityState(x) = "Removed" Or imgCapabilityState(x) = "Not present" Or imgCapabilityState(x) = "Uninstalled" Then
                    Continue For
                End If
                RemCapabilities.ListView1.Items.Add(New ListViewItem(New String() {imgCapabilityIds(x), imgCapabilityState(x)}))
            Next
        Catch ex As Exception
            Exit Try
        End Try
        Try
            For x = 0 To Array.LastIndexOf(imgCapabilityIds, imgCapabilityIds.Last)
                If imgCapabilityIds(x) = "" Then
                    Exit For
                End If
                ElementCount += 1
            Next
        Catch ex As Exception
            Exit Try
        End Try
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        RemCapabilities.Label2.Text = "This image contains " & ElementCount & " capabilities."
                    Case "ESN"
                        RemCapabilities.Label2.Text = "Esta imagen contiene " & ElementCount & " funcionalidades."
                    Case "FRA"
                        AddCapabilities.Label4.Text = "Cette image contient " & ElementCount & " capacités."
                End Select
            Case 1
                RemCapabilities.Label2.Text = "This image contains " & ElementCount & " capabilities."
            Case 2
                RemCapabilities.Label2.Text = "Esta imagen contiene " & ElementCount & " funcionalidades."
            Case 3
                AddCapabilities.Label4.Text = "Cette image contient " & ElementCount & " capacités."
        End Select
        RemCapabilities.ShowDialog()
    End Sub

    Private Sub Button48_Click(sender As Object, e As EventArgs) Handles Button48.Click
        If imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Or Not IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
            End Select
            Exit Sub
        End If
        ElementCount = 0
        AddCapabilities.ListView1.Items.Clear()
        ProgressPanel.OperationNum = 994
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting capability names and their state..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de funcionalidades y sus estados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms des capacités et de leur état en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting capability names and their state..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de funcionalidades y sus estados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms des capacités et de leur état en cours..."
        End Select
        If Not CompletedTasks(3) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Try
            For x = 0 To Array.LastIndexOf(imgCapabilityIds, imgCapabilityIds.Last)
                If imgCapabilityState(x) = "Installed" Or imgCapabilityState(x) = "Install Pending" Then
                    Continue For
                End If
                AddCapabilities.ListView1.Items.Add(New ListViewItem(New String() {imgCapabilityIds(x), imgCapabilityState(x)}))
            Next
        Catch ex As Exception
            Exit Try
        End Try
        Try
            For x = 0 To Array.LastIndexOf(imgCapabilityIds, imgCapabilityIds.Last)
                If imgCapabilityIds(x) = "" Then
                    Exit For
                End If
                ElementCount += 1
            Next
        Catch ex As Exception
            Exit Try
        End Try
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        AddCapabilities.Label4.Text = "This image contains " & ElementCount & " capabilities."
                    Case "ESN"
                        AddCapabilities.Label4.Text = "Esta imagen contiene " & ElementCount & " funcionalidades."
                    Case "FRA"
                        AddCapabilities.Label4.Text = "Cette image contient " & ElementCount & " capacités."
                End Select
            Case 1
                AddCapabilities.Label4.Text = "This image contains " & ElementCount & " capabilities."
            Case 2
                AddCapabilities.Label4.Text = "Esta imagen contiene " & ElementCount & " funcionalidades."
            Case 3
                AddCapabilities.Label4.Text = "Cette image contient " & ElementCount & " capacités."
        End Select
        AddCapabilities.ShowDialog()
    End Sub

    Private Sub Button49_Click(sender As Object, e As EventArgs) Handles Button49.Click
        If imgEdition.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Or Not IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
            End Select
            Exit Sub
        End If
        ProgressPanel.OperationNum = 994
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting capability names and their state..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo nombres de funcionalidades y sus estados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des noms des capacités et de leur état en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting capability names and their state..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo nombres de funcionalidades y sus estados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des noms des capacités et de leur état en cours..."
        End Select
        If Not CompletedTasks(3) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(500)
        End While
        If CapabilityInfoList IsNot Nothing Then GetCapabilityInfoDlg.InstalledCapabilityInfo = CapabilityInfoList
        GetCapabilityInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub Button50_Click(sender As Object, e As EventArgs) Handles Button50.Click
        If ImgInfoSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = SourceImg
            ImgInfoSaveDlg.ImgMountDir = If(Not OnlineManagement, MountDir, "")
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.OfflineMode = OfflineManagement
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.SaveTask = 6
            ImgInfoSaveDlg.ShowDialog()
        End If
    End Sub

    Private Sub Button51_Click(sender As Object, e As EventArgs) Handles Button51.Click
        If OnlineManagement Then
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on online installations", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en instalaciones activas", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge par les installations en ligne", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on online installations", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en instalaciones activas", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge par les installations en ligne", vbOKOnly + vbCritical, Text)
            End Select
            Exit Sub
        End If
        RemDrivers.ListView1.Items.Clear()
        ProgressPanel.OperationNum = 994
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting installed driver packages..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo paquetes de controladores instalados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des paquets de pilotes installés en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting installed driver packages..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo paquetes de controladores instalados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des paquets de pilotes installés en cours..."
        End Select
        If Not CompletedTasks(4) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Try
            For x = 0 To Array.LastIndexOf(imgDrvPublishedNames, imgDrvPublishedNames.Last)
                If RemDrivers.CheckBox1.Checked Then
                    If imgDrvBootCriticalStatus(x) Then Continue For
                End If
                If RemDrivers.CheckBox2.Checked Then
                    If CBool(imgDrvInbox(x)) Then Continue For
                End If
                Select Case Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                RemDrivers.ListView1.Items.Add(New ListViewItem(New String() {imgDrvPublishedNames(x), Path.GetFileName(imgDrvOGFileNames(x)), imgDrvProviderNames(x), imgDrvClassNames(x), If(CBool(imgDrvInbox(x)), "Yes", "No"), If(imgDrvBootCriticalStatus(x), "Yes", "No"), imgDrvVersions(x), imgDrvDates(x)}))
                            Case "ESN"
                                RemDrivers.ListView1.Items.Add(New ListViewItem(New String() {imgDrvPublishedNames(x), Path.GetFileName(imgDrvOGFileNames(x)), imgDrvProviderNames(x), imgDrvClassNames(x), If(CBool(imgDrvInbox(x)), "Sí", "No"), If(imgDrvBootCriticalStatus(x), "Sí", "No"), imgDrvVersions(x), imgDrvDates(x)}))
                            Case "FRA"
                                RemDrivers.ListView1.Items.Add(New ListViewItem(New String() {imgDrvPublishedNames(x), Path.GetFileName(imgDrvOGFileNames(x)), imgDrvProviderNames(x), imgDrvClassNames(x), If(CBool(imgDrvInbox(x)), "Oui", "Non"), If(imgDrvBootCriticalStatus(x), "Oui", "Non"), imgDrvVersions(x), imgDrvDates(x)}))
                        End Select
                    Case 1
                        RemDrivers.ListView1.Items.Add(New ListViewItem(New String() {imgDrvPublishedNames(x), Path.GetFileName(imgDrvOGFileNames(x)), imgDrvProviderNames(x), imgDrvClassNames(x), If(CBool(imgDrvInbox(x)), "Yes", "No"), If(imgDrvBootCriticalStatus(x), "Yes", "No"), imgDrvVersions(x), imgDrvDates(x)}))
                    Case 2
                        RemDrivers.ListView1.Items.Add(New ListViewItem(New String() {imgDrvPublishedNames(x), Path.GetFileName(imgDrvOGFileNames(x)), imgDrvProviderNames(x), imgDrvClassNames(x), If(CBool(imgDrvInbox(x)), "Sí", "No"), If(imgDrvBootCriticalStatus(x), "Sí", "No"), imgDrvVersions(x), imgDrvDates(x)}))
                    Case 3
                        RemDrivers.ListView1.Items.Add(New ListViewItem(New String() {imgDrvPublishedNames(x), Path.GetFileName(imgDrvOGFileNames(x)), imgDrvProviderNames(x), imgDrvClassNames(x), If(CBool(imgDrvInbox(x)), "Oui", "Non"), If(imgDrvBootCriticalStatus(x), "Oui", "Non"), imgDrvVersions(x), imgDrvDates(x)}))
                End Select
            Next
        Catch ex As Exception
            Exit Try
        End Try
        RemDrivers.ShowDialog()
    End Sub

    Private Sub Button52_Click(sender As Object, e As EventArgs) Handles Button52.Click
        ProgressPanel.OperationNum = 994
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting installed driver packages..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo paquetes de controladores instalados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des paquets de pilotes installés en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting installed driver packages..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo paquetes de controladores instalados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des paquets de pilotes installés en cours..."
        End Select
        If Not CompletedTasks(4) Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(500)
        End While
        If DriverInfoList IsNot Nothing Then GetDriverInfo.InstalledDriverInfo = DriverInfoList
        GetDriverInfo.ShowDialog()
    End Sub

    Private Sub Button53_Click(sender As Object, e As EventArgs) Handles Button53.Click
        If Not OnlineManagement Then
            AddDrivers.ShowDialog()
        Else
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on online installations", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en instalaciones activas", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge par les installations en ligne", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on online installations", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en instalaciones activas", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge par les installations en ligne", vbOKOnly + vbCritical, Text)
            End Select
        End If
    End Sub

    Private Sub Button54_Click(sender As Object, e As EventArgs) Handles Button54.Click
        If ImgInfoSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            If ImgInfoSaveDlg.DriverPkgs.Count > 0 Then ImgInfoSaveDlg.DriverPkgs.Clear()
            ImgInfoSaveDlg.SourceImage = SourceImg
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            ImgInfoSaveDlg.ImgMountDir = If(Not OnlineManagement, MountDir, "")
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.OfflineMode = OfflineManagement
            ImgInfoSaveDlg.AllDrivers = AllDrivers
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.SaveTask = 7
            ImgInfoSaveDlg.ShowDialog()
        End If
    End Sub

    Private Sub Button55_Click(sender As Object, e As EventArgs) Handles Button55.Click
        If ImgBW.IsBusy Then
            BGProcsBusyDialog.ShowDialog()
            Exit Sub
        End If
        GetWinPESettings.ShowDialog()
    End Sub

    Private Sub Button56_Click(sender As Object, e As EventArgs) Handles Button56.Click
        If ImgInfoSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = SourceImg
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            ImgInfoSaveDlg.ImgMountDir = If(Not OnlineManagement, MountDir, "")
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.SaveTask = 9
            ImgInfoSaveDlg.ShowDialog()
        End If
    End Sub

    Private Sub Button57_Click(sender As Object, e As EventArgs) Handles Button57.Click
        If ImgBW.IsBusy Then
            BGProcsBusyDialog.ShowDialog()
            Exit Sub
        End If
        SetPETargetPath.ShowDialog()
    End Sub

    Private Sub Button58_Click(sender As Object, e As EventArgs) Handles Button58.Click
        If ImgBW.IsBusy Then
            BGProcsBusyDialog.ShowDialog()
            Exit Sub
        End If
        SetPEScratchSpace.ShowDialog()
    End Sub

#End Region

    Sub GetFeedNews()
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        Try
            Dim rssUrl As String = "https://reddit.com/r/DISMTools.rss"
            Dim rssContent As String = ""
            Using client As New WebClient()
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36")
                rssContent = client.DownloadString(rssUrl)
            End Using
            If Not String.IsNullOrWhiteSpace(rssContent) Then
                Dim reader As XmlReader = XmlReader.Create(New StringReader(rssContent))
                Dim feed As SyndicationFeed = SyndicationFeed.Load(reader)
                reader.Close()
                For Each item As SyndicationItem In feed.Items.OrderByDescending(Function(x) x.PublishDate)
                    ListView1.Items.Add(New ListViewItem(New String() {item.Title.Text, item.PublishDate.ToString()}))
                    FeedLinks.Add(item.Links(0).Uri)
                Next
            End If
        Catch ex As Exception
            FeedsPanel.Visible = False
            FeedErrorPanel.Visible = True
            TextBox1.Text = ex.ToString() & " - " & ex.Message
        End Try
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        WelcomeTabControl.Visible = False
        StartPanel.Visible = True
        Button17.Visible = False
    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        WelcomeTabControl.Visible = True
        StartPanel.Visible = False
        Button17.Visible = True
    End Sub

    Private Sub LinkLabel22_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel22.LinkClicked
        GetStartedPanel.Visible = True
        LatestNewsPanel.Visible = False
        LinkLabel22.LinkColor = ForeColor
        LinkLabel23.LinkColor = Color.FromArgb(153, 153, 153)
        LinkLabel24.LinkColor = Color.FromArgb(153, 153, 153)
    End Sub

    Private Sub LinkLabel23_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel23.LinkClicked
        GetStartedPanel.Visible = False
        LatestNewsPanel.Visible = True
        LinkLabel22.LinkColor = Color.FromArgb(153, 153, 153)
        LinkLabel23.LinkColor = ForeColor
        LinkLabel24.LinkColor = Color.FromArgb(153, 153, 153)
    End Sub

    Private Sub LinkLabel24_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel24.LinkClicked
        LinkLabel22.LinkColor = Color.FromArgb(153, 153, 153)
        LinkLabel23.LinkColor = Color.FromArgb(153, 153, 153)
        LinkLabel24.LinkColor = ForeColor
    End Sub

    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick
        If ListView1.SelectedItems.Count = 1 Then
            Process.Start(FeedLinks(ListView1.FocusedItem.Index).AbsoluteUri)
        End If
    End Sub

    Private Sub HelpTopicsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpTopicsToolStripMenuItem.Click
        HelpBrowserForm.Show()
    End Sub

    Private Sub LinkLabel12_MouseLeave(sender As Object, e As EventArgs) Handles LinkLabel12.MouseLeave
        If SidePanel_ProjectView.Visible Then
            LinkLabel12.LinkColor = Color.FromArgb(241, 241, 241)
        Else
            LinkLabel12.LinkColor = Color.FromArgb(153, 153, 153)
        End If
    End Sub

    Private Sub LinkLabel13_MouseLeave(sender As Object, e As EventArgs) Handles LinkLabel13.MouseLeave
        If SidePanel_ImageView.Visible Then
            LinkLabel13.LinkColor = Color.FromArgb(241, 241, 241)
        Else
            LinkLabel13.LinkColor = Color.FromArgb(153, 153, 153)
        End If
    End Sub

    Private Sub LinkLabel12_MouseEnter(sender As Object, e As EventArgs) Handles LinkLabel12.MouseEnter
        If LinkLabel12.LinkColor = Color.FromArgb(241, 241, 241) Then
            Cursor = Cursors.Arrow
            Exit Sub
        Else
            LinkLabel12.LinkColor = Color.FromArgb(0, 151, 251)
        End If
    End Sub

    Private Sub LinkLabel13_MouseEnter(sender As Object, e As EventArgs) Handles LinkLabel13.MouseEnter
        If LinkLabel13.LinkColor = Color.FromArgb(241, 241, 241) Then
            Cursor = Cursors.Arrow
            Exit Sub
        Else
            LinkLabel13.LinkColor = Color.FromArgb(0, 151, 251)
        End If
    End Sub
End Class