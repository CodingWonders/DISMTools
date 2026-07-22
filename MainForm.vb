Imports System.Net
Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports Microsoft.Win32
Imports Microsoft.Dism
Imports System.Runtime.InteropServices
Imports System.Xml
Imports System.Xml.Serialization
Imports System.ServiceModel.Syndication
Imports DISMTools.Utilities
Imports DISMTools.Elements
Imports DISMTools.Elements.Contemporaneus
Imports DISMTools.Elements.IniParserConfigurators
Imports System.ComponentModel
Imports IniParser
Imports IniParser.Parser
Imports IniParser.Model
Imports System.Management
Imports System.Threading.Tasks
Imports System.Globalization
Imports DISMTools.Elements.InfinityHome
Imports System.Text.RegularExpressions

Public Class MainForm

    Public IsInEditMode As Boolean
    Public imgStatus As Integer
    Public prjName As String
    Public IsImageMounted As Boolean
    Public projPath As String
    Public isReadOnly As Boolean
    Public isProjectLoaded As Boolean
    Public isModified As Boolean

    ' Image info
    Public SourceImg As String
    Public ImgIndex As Integer
    Public MountDir As String       ' ProjProperties.imgMountDir

    ' Var used to detect whether the image is orphaned (needs servicing session reload)
    Public isOrphaned As Boolean    ' This variable is true when the host system is shut down or restarted (the servicing session stops abruptly)

    ' Program settings
    ' This is the initial batch of settings for this version (0.1). As the program continues development,
    ' more settings will be added below this initial batch
    Public DismExe As String
    Public SaveOnSettingsIni As Boolean
    Public ColorMode As Integer
    Public LanguageCode As String = LocalizationService.CurrentCultureCode
    Public LogFont As String
    Public LogFile As String
    Public LogLevel As Integer = 3
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
    Public dtBranch As String = "stable"
    Public dt_codeName As String = "Infinity"

    ' Arrays and other variables used on background processes
    Public areBackgroundProcessesDone As Boolean

    Dim pbOpNums As Integer
    Dim progressMin As Integer = 0
    Dim progressDivs As Double
    Dim progressLabel As String
    Dim regJumps As Boolean
    Dim irregVal As Integer = 0

    Public pinState As Integer

    ' Perform image unmount operations when pressing on buttons
    Public imgCommitOperation As Integer = -1 ' 0: commit; 1: discard

    Dim DismVersionChecker As FileVersionInfo
    Dim argProjPath As String = ""                                       ' String used to know which project to load if it's specified in an argument
    Dim argOnline As Boolean                                             ' Determine if program will be launched in online installation mode
    Dim argOffline As Boolean                                            ' Determine if program will be launched in offline installation mode

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
    Dim FailedBGProcResultDic As New Dictionary(Of String, Exception)

    Dim HasRemounted As Boolean

    Dim IsCompatible As Boolean = True

    Dim SysVer As Version

    Dim NoMigration As Boolean                                           ' Set this variable to true ONLY if the IDE started the program
    Public SkipUpdates As Boolean                                        ' Same for this one

    Public drivePath As String = ""

    Public EnableExperiments As Boolean

    Public SkipQuestions As Boolean             ' Skips questions in the info saver
    Public AutoCompleteInfo(4) As Boolean       ' Skips questions for specific info categories

    Public AutoCleanMounts As Boolean

    Public ExpandedProgressPanel As Boolean      ' Determine whether to show the progress panel in its expanded form

    Public SystemEditor As String               ' System editor specified by the user

    Public EnableDynaLog As Boolean = True             ' Determine whether to enable DynaLog logging

    Dim FeedContents As New SyndicationFeed()
    Dim FeedLinks As New List(Of Uri)
    Dim FeedEx As Exception

    Dim ImageStatus As ImageWatcher.Status

    Public RecentList As New List(Of Recents)
    Public VideoList As New List(Of Video)
    Dim thumbnailList As ImageList = New ImageList()
    Dim VideoEx As Exception

    Dim AdkCopyEx As Exception

    Dim OriginalWindowBounds As Rectangle           ' Window bounds before full-screen
    Dim OriginalWindowState As FormWindowState      ' Window state before full-screen

    Public DarkThemeIndex As Integer = 0            ' Color theme index for dark color scheme
    Public LightThemeIndex As Integer = 1           ' Color theme index for light color scheme
    Public ShowDateAndTime As Boolean = True        ' Whether to show the date and time on the project view

    Public NoNTSamMappings As Boolean = False       ' Whether to map AppX pckgdep SIDs with SIDs from system's SAM file

    Public AppxDisplayNameFormatOnRemoval As Integer = 1        ' The format to use when showing disply names in the appx removal dialog. 0: display name only; 1: disp name + friendly disp name; 2: friendly disp name only

    Public IsFirstTime As Boolean = False           ' Whether the user has launched this software for the first time

    ' Preinstallation Environment Helper Settings
    Public PEHelper_UnattendedFile As String = ""       ' A default unattended answer file for new ISOs
    Public PEHelper_CopyToVentoy As Boolean = False     ' Whether to copy new ISO files to Ventoy drives automatically
    Public PEHelper_Use2023EFI As Boolean = False       ' Whether to use Windows UEFI CA 2023-signed boot binaries (EFI ONLY)
    Public PEHelper_IncludeSysDrvs As Boolean = True    ' Whether to include SCSI adapters and network controllers in the DTPE

    ' Web Search Settings
    Public SearchEngineName As String = "DuckDuckGo" ' The name of the selected search engine
    Public SearchEngineAITolerance As Integer = 1    ' The amount of tolerance of AI in search engines

    ' Tour server
    Public ReadOnly tourServer As DTHttpServer = New DTHttpServer(Path.Combine(Application.StartupPath, "docs", "tour"), 2022)
    Private ReadOnly videoServer As New DTHttpServer(Path.Combine(Application.StartupPath, "videos"), 2026)

    ' Contemporaneus Preview
    Public MountedImageList As New List(Of WindowsImage)
    Public CurrentImage As New WindowsImage()

    ' Default PE Policy settings
    Public ShowWatermark As Boolean = False
    Public WDSHCGraphoView As Boolean = True
    Public DTDimShowPnputilOut As Boolean = True
    Public AutoUnattendCopytoSysprep As Boolean = False
    Public PartTableOverridePreference As Integer = 0
    Public UEFICA23Preference As Integer = 0
    Public WDSHCConnAttempts As Integer = 5
    Public PXEServerPort As Integer = 8080
    Public KeyboardLayoutCode As String = "00000409"
    Public KeyboardLayoutOverrideExistingLayout As Boolean = False
    Public AnswerFileConflictResponse As Integer = 0

    ' INFINITY settings
    Public PreventSystemFromSleeping As Boolean = True      ' Whether to call system APIs to prevent the machine from sleeping during image operations
    Public HumanizeDates As Boolean = True                  ' Whether to display all date fields in a human-readable format

    Public ReinitializeCurImage As Boolean = True

    Private NewsFeedWebContent As WebBrowser
    Private NewsFeedContent As String
    Private NewsLastUpdateDate As Date

    ' Infinity Home
    Private InfinityHomeFacts As New List(Of InfinityFact)

    Sub GetArguments()
        Dim args() As String = Environment.GetCommandLineArgs()
        DynaLog.LogMessage("Command-line arguments that have been passed to the program: " & String.Join(" ", args))
        If args.Length = 1 Then
            DynaLog.LogMessage("DISMTools has been called as is - without any arguments. Exiting...")
            Exit Sub
        ElseIf args.Length = 2 And args(1) = "/?" Then
            DynaLog.LogMessage("Help has been requested by the user. Showing help message...")
            ' Show command-line argument help
            MsgBox(LocalizationService.ForSection("Main.CommandLineHelp")("Pass.Arguments.Message"), vbOKOnly + vbInformation, LocalizationService.ForSection("Main.CommandLineHelp")("DISM.Tools.Title"))
            DynaLog.LogMessage("User accepted the dialog. Continuing startup...")
        Else
            DynaLog.LogMessage("Parsing command-line arguments...")
            For Each arg In args
                DynaLog.LogMessage("Command-line argument: " & arg)
                If arg.StartsWith("/setup", StringComparison.OrdinalIgnoreCase) Then
                    DynaLog.LogMessage("Setting reconfiguration has been requested by the user. Showing initial setup wizard (ISW)...")
                    SplashScreen.Hide()
                    PrgSetup.ShowDialog()
                ElseIf arg.StartsWith("/load", StringComparison.OrdinalIgnoreCase) Then
                    DynaLog.LogMessage("Determining if specified project can be loaded...")
                    Dim requestedProjectPath As String = arg.Substring(arg.IndexOf("="c) + 1).Trim().Trim(Quote)
                    If File.Exists(requestedProjectPath) AndAlso Directory.Exists(Path.GetDirectoryName(requestedProjectPath)) Then
                        DynaLog.LogMessage("Specified project satisfies all requirements (projfile exists, dir exists). Passing to load...")
                        argProjPath = requestedProjectPath
                    Else
                        DynaLog.LogMessage("Specified project does NOT satisfy all requirements (either projfile or dir doesn't exist). Cannot continue loading project. Path: " & Quote & requestedProjectPath & Quote)
                        MsgBox(LocalizationService.ForSection("Main.GetArguments").Format("Project.Message", requestedProjectPath), vbOKOnly + vbCritical, Text)
                    End If
                ElseIf arg.StartsWith("/online", StringComparison.OrdinalIgnoreCase) Then
                    DynaLog.LogMessage("Detecting if no projects had been passed by the /load flag...")
                    If argProjPath = "" Then
                        DynaLog.LogMessage("No projects have been passed. Passing to load online inst management mode...")
                        argOnline = True
                    Else
                        DynaLog.LogMessage("A project has already been specified by the /load flag. Skipping...")
                        ' Add warning later
                    End If
                ElseIf arg.StartsWith("/offline", StringComparison.OrdinalIgnoreCase) Then
                    DynaLog.LogMessage("Detecting if no projects had been passed by the /load flag...")
                    If argProjPath = "" Then
                        DynaLog.LogMessage("No projects have been passed. Passing to load offline inst management mode...")
                        If arg.Replace("/offline:", "").Trim() <> "" Then
                            DynaLog.LogMessage("Getting all disks...")
                            Dim diList As New List(Of DriveInfo)
                            diList = DriveInfo.GetDrives().Where(Function(disk) disk.IsReady).ToList()
                            Dim diPaths As New List(Of String)
                            DynaLog.LogMessage("Disks have been obtained. Preparing list...")
                            For Each di As DriveInfo In diList
                                DynaLog.LogMessage("Essential disk information:" & CrLf &
                                                   "- Is disk ready? " & If(di.IsReady, "Yes", "No. Skipping...") & CrLf &
                                                   "- Drive letter: " & If(di.IsReady, di.Name, "Disk is not ready"))
                                diPaths.Add(di.Name)
                            Next
                            If Path.GetPathRoot(arg.Replace("/offline:", "").Trim()) = arg.Replace("/offline:", "").Trim() And diPaths.Contains(arg.Replace("/offline:", "").Trim()) Then
                                DynaLog.LogMessage("Disk specified satisfies all requirements (disk is ready, path is only path root). Passing to load...")
                                drivePath = arg.Replace("/offline:", "").Trim()
                                argOffline = True
                            End If
                        End If
                    Else
                        ' Add warning later
                    End If
                ElseIf arg.StartsWith("/migrate", StringComparison.OrdinalIgnoreCase) Then
                    DynaLog.LogMessage("Setting migration has been requested by the user or by DTUCS. Migrating settings...")
                    MigrationForm.ShowDialog()
                    Thread.Sleep(1500)
                ElseIf arg.StartsWith("/nomig", StringComparison.OrdinalIgnoreCase) Then
                    DynaLog.LogMessage("Setting migration has been disabled. Unless you are testing a build straight out of the build process, a configuration file may be incompatible")
                    NoMigration = True
                ElseIf arg.StartsWith("/noupd", StringComparison.OrdinalIgnoreCase) Then
                    DynaLog.LogMessage("Update checks have been disabled. Unless you are testing a build straight out of the build process, you will be missing out on new features and fixes")
                    SkipUpdates = True
                ElseIf arg.StartsWith("/exp", StringComparison.OrdinalIgnoreCase) Then
                    DynaLog.LogMessage("Experiments have been enabled. Now, whether or not this build contains experiments is up to decide by CW")
                    EnableExperiments = True
                End If
            Next
        End If
    End Sub

    Function LoadRecents(filePath As String) As List(Of Recents)
        Dim recList As New List(Of Recents)
        Try
            DynaLog.LogMessage("Grabbing recents list elements from XML file " & Quote & filePath & Quote & "...")
            Using fs As FileStream = New FileStream(filePath, FileMode.Open)
                Dim xs As New XmlReaderSettings()
                xs.IgnoreWhitespace = True
                Using reader As XmlReader = XmlReader.Create(fs, xs)
                    While reader.Read()
                        If reader.NodeType = XmlNodeType.Element AndAlso reader.Name = "Recents" Then
                            DynaLog.LogMessage("We are dealing with a recents list element (XML node type is element and is " & Quote & "Recents" & Quote & ".) Parsing...")
                            Dim rec As New Recents()
                            rec.ProjPath = reader.GetAttribute("Path")
                            rec.ProjName = reader.GetAttribute("Name")
                            rec.Order = Integer.Parse(reader.GetAttribute("Order"))
                            DynaLog.LogMessage(rec.ToString())
                            recList.Add(rec)
                        End If
                    End While
                End Using
            End Using
            DynaLog.LogMessage("Recents list has " & recList.Count & " item(s). Good job")
            Return recList
        Catch ex As Exception
            Return Nothing
        End Try
        Return Nothing
    End Function

    Sub SaveRecents(Of T)(obj As T, filePath As String)
        Try
            DynaLog.LogMessage("Checking if XML files exist, deleting them if they do, and recreating backups...")
            If File.Exists(Application.StartupPath & "\recents.xml.old") Then File.Delete(Application.StartupPath & "\recents.xml.old")
            If File.Exists(Application.StartupPath & "\recents.xml") Then File.Move(Application.StartupPath & "\recents.xml", Application.StartupPath & "\recents.xml.old")
            DynaLog.LogMessage("Serializing items to XML file...")
            Dim serial As New XmlSerializer(GetType(T))
            Using writer As New StreamWriter(filePath)
                serial.Serialize(writer, obj)
            End Using
            DynaLog.LogMessage("Serialization complete. Deleting backups...")
            If File.Exists(Application.StartupPath & "\recents.xml.old") Then File.Delete(Application.StartupPath & "\recents.xml.old")
        Catch ex As Exception
            DynaLog.LogMessage("An error occurred saving the recents list. Trying to reinstate old recents list..." & CrLf &
                               "Error information: " & ex.Message)
            If File.Exists(Application.StartupPath & "\recents.xml.old") Then File.Move(Application.StartupPath & "\recents.xml.old", Application.StartupPath & "\recents.xml")
        End Try
    End Sub

    Sub ChangeRecentListOrder(Project As Recents, itmIndex As Integer)
        DynaLog.LogMessage("Beginning to reorder recents list...")
        DynaLog.LogMessage("- Project to reorder: " & Project.ToString())
        DynaLog.LogMessage("- Place to move item from: " & itmIndex)
        ' Update listings
        RecentsLV.Items.Clear()
        RecentList.RemoveAt(itmIndex)
        RecentList.Insert(0, Project)
        DynaLog.LogMessage("Reconfiguring project order for recents list...")
        For Each recentProject In RecentList
            recentProject.Order = RecentList.IndexOf(recentProject)
            RecentsLV.Items.Add(If(recentProject.ProjName <> "", recentProject.ProjName, Path.GetFileNameWithoutExtension(recentProject.ProjPath)))
        Next
        Try
            DynaLog.LogMessage("Changing paths of recents submenu items to reflect new list...")
            RecentProject1ToolStripMenuItem.Text = " "
            RecentProject2ToolStripMenuItem.Text = " "
            RecentProject3ToolStripMenuItem.Text = " "
            RecentProject4ToolStripMenuItem.Text = " "
            RecentProject5ToolStripMenuItem.Text = " "
            RecentProject6ToolStripMenuItem.Text = " "
            RecentProject7ToolStripMenuItem.Text = " "
            RecentProject8ToolStripMenuItem.Text = " "
            RecentProject9ToolStripMenuItem.Text = " "
            RecentProject10ToolStripMenuItem.Text = " "

            ' Reconfigure text
            DynaLog.LogMessage("Relying on mother nature of software to determine which ones are visible...")
            RecentProject1ToolStripMenuItem.Text = RecentList(0).ProjPath
            RecentProject1ToolStripMenuItem.Visible = True
            RecentProject2ToolStripMenuItem.Text = RecentList(1).ProjPath
            RecentProject2ToolStripMenuItem.Visible = True
            RecentProject3ToolStripMenuItem.Text = RecentList(2).ProjPath
            RecentProject3ToolStripMenuItem.Visible = True
            RecentProject4ToolStripMenuItem.Text = RecentList(3).ProjPath
            RecentProject4ToolStripMenuItem.Visible = True
            RecentProject5ToolStripMenuItem.Text = RecentList(4).ProjPath
            RecentProject5ToolStripMenuItem.Visible = True
            RecentProject6ToolStripMenuItem.Text = RecentList(5).ProjPath
            RecentProject6ToolStripMenuItem.Visible = True
            RecentProject7ToolStripMenuItem.Text = RecentList(6).ProjPath
            RecentProject7ToolStripMenuItem.Visible = True
            RecentProject8ToolStripMenuItem.Text = RecentList(7).ProjPath
            RecentProject8ToolStripMenuItem.Visible = True
            RecentProject9ToolStripMenuItem.Text = RecentList(8).ProjPath
            RecentProject9ToolStripMenuItem.Visible = True
            RecentProject10ToolStripMenuItem.Text = RecentList(9).ProjPath
            RecentProject10ToolStripMenuItem.Visible = True
        Catch ex As Exception
            ' Don't do anything special here
        End Try
    End Sub

    Function LoadVideos(filePath As String) As List(Of Video)
        Dim vidList As New List(Of Video)
        Try
            DynaLog.LogMessage("Grabbing CodingWonders' DT videos from XML file " & Quote & filePath & Quote & "...")
            Using fs As FileStream = New FileStream(filePath, FileMode.Open)
                Dim xs As New XmlReaderSettings()
                xs.IgnoreWhitespace = True
                Using reader As XmlReader = XmlReader.Create(fs, xs)
                    While reader.Read()
                        If reader.NodeType = XmlNodeType.Element AndAlso reader.Name = "Video" Then
                            DynaLog.LogMessage("We are dealing with a video (XML node type is element and is " & Quote & "Video" & Quote & ".) Parsing...")
                            Dim vid As New Video()
                            vid.YT_ID = reader.GetAttribute("ID")
                            vid.VideoName = reader.GetAttribute("Name")
                            vid.VideoDesc = reader.GetAttribute("Description")
                            DynaLog.LogMessage(vid.ToString())
                            vidList.Add(vid)
                        End If
                    End While
                End Using
            End Using
            DynaLog.LogMessage("Video list has " & vidList.Count & " item(s). Good job")
            Return vidList
        Catch ex As Exception
            Return Nothing
        End Try
        Return Nothing
    End Function

    Private Function GetKitsRoot(IsWOW6432Environment As Boolean) As String
        DynaLog.LogMessage("Getting ADK root for Windows 10+ ADK...")
        DynaLog.LogMessage("Detect in a WOW64 compatibility (32-bit) environment? " & If(IsWOW6432Environment, "Yes", "No"))

        Dim Adk10KitsRoot As String = ""

        ' if we set the wow64 bit on and we're on a 32-bit system, then we prematurely return the value
        If IsWOW6432Environment AndAlso Not Environment.Is64BitOperatingSystem Then
            DynaLog.LogMessage("A 32-bit environment has been detected and we're checking values in WOW64. We're already on 32-bit, so we don't check...")
            Return Adk10KitsRoot
        End If

        Dim kitsRootRegistryPath As String = String.Format("SOFTWARE{0}\Microsoft\Windows Kits\Installed Roots", If(IsWOW6432Environment, "\WOW6432Node", ""))

        Try
            Using KitsRootRk As RegistryKey = Registry.LocalMachine.OpenSubKey(kitsRootRegistryPath, False)
                If KitsRootRk Is Nothing Then
                    DynaLog.LogMessage("ADK kits root registry key was not found: HKLM\" & kitsRootRegistryPath)
                    Return Adk10KitsRoot
                End If

                Dim kitsRootValue As Object = KitsRootRk.GetValue("KitsRoot10")
                If kitsRootValue IsNot Nothing Then
                    Adk10KitsRoot = kitsRootValue.ToString()
                End If
            End Using
        Catch ex As Exception
            DynaLog.LogMessage("Could not check kits root. Error message: " & ex.Message)
        End Try

        DynaLog.LogMessage("Detected Kits Root: " & Adk10KitsRoot)

        Return Adk10KitsRoot
    End Function

    Function DetectPossibleADKs() As Integer
        DynaLog.LogMessage("Detecting possible ADKs...")

        Dim AdkKitsRoot As String = GetKitsRoot(False),
            AdkKitsRoot_WOW64Environ As String = GetKitsRoot(True)

        Dim expectedADKPath As String = Path.Combine(AdkKitsRoot, "Assessment and Deployment Kit"),
            expectedADKPath_WOW64Environ As String = Path.Combine(AdkKitsRoot_WOW64Environ, "Assessment and Deployment Kit")

        DynaLog.LogMessage("- Kits root for Win64 environment: " & AdkKitsRoot)
        DynaLog.LogMessage("- Kits root for WOW64 compatibility environment: " & AdkKitsRoot_WOW64Environ)

        DynaLog.LogMessage("Expected ADK locations are:")
        DynaLog.LogMessage("- For native/Win64 environments: " & expectedADKPath)
        DynaLog.LogMessage("- For WOW64 compatibility environments: " & expectedADKPath_WOW64Environ)

        Dim DefinedADKInstallation As Boolean
        Try
            ' We'll check if the expected ADK paths exist. If at least one exists, then we know we have the ADK.
            For Each adkPath In {expectedADKPath, expectedADKPath_WOW64Environ}
                DynaLog.LogMessage("Determining if " & adkPath & " exists...")
                If Directory.Exists(adkPath) Then
                    DynaLog.LogMessage(Quote & adkPath & Quote & " exists. We have an ADK...")
                    Return 2
                Else
                    DynaLog.LogMessage(Quote & adkPath & Quote & " does not exist.")
                End If
            Next
        Catch ex As Exception
            DynaLog.LogMessage("Could not grab ADK installation. " & ex.Message)
            DefinedADKInstallation = False
        End Try
        If Not DefinedADKInstallation Then
            DynaLog.LogMessage("Checking if ADKs are found in standard locations to perform fixups...")
            Dim folderPath As String = ""
            If Environment.Is64BitOperatingSystem Then
                folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\Windows Kits\10\Assessment and Deployment Kit"
            Else
                folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Windows Kits\10\Assessment and Deployment Kit"
            End If
            If Directory.Exists(folderPath) Then
                DynaLog.LogMessage("ADK exists in standard location, but DT does not detect it. Reporting...")
                Return 1
            Else
                DynaLog.LogMessage("Either no ADK is installed or is installed somewhere else")
                Return 0
            End If
        End If
        Return 0
    End Function

    Function GetCopyrightTimespan(ByVal start As Integer, ByVal current As Integer) As String
        If current <= start Then
            Return current.ToString()
        Else
            Return start.ToString() & "-" & current.ToString()
        End If
    End Function

    Sub InitDynaLog()
        DynaLog.CheckLogAge()
        DynaLog.LogMessage("DISMTools - Version " & My.Application.Info.Version.ToString() & " (" & dt_codeName & "), build timestamp: " & RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm"))
        ' Display copyright/author information for every component
        DynaLog.LogMessage("Components:")
        DynaLog.LogMessage("- Program: " & My.Application.Info.Copyright.Replace("©", "(c)"))
        DynaLog.LogMessage("- ExtAppx.ps1/MImgMgr.ps1: (c) " & GetCopyrightTimespan(2023, Date.Now.Year) & " CodingWonders Software")
        DynaLog.LogMessage("- PE Helper: (c) " & GetCopyrightTimespan(2024, Date.Now.Year) & " CodingWonders Software" & CrLf &
                           "  Compilation Preprocessor by og-mrk (https://github.com/og-mrk), modified from WinUtil: (c) " & GetCopyrightTimespan(2022, 2022) & " CT Tech Group LLC" & CrLf &
                           "  Driver Installation Module: (c) " & GetCopyrightTimespan(2024, Date.Now.Year) & " CodingWonders Software" & CrLf &
                           "  HotInstall: (c) " & GetCopyrightTimespan(2025, Date.Now.Year) & " CodingWonders Software" & CrLf &
                           "  Preboot eXecution Environment (PXE) Helpers: (c) " & GetCopyrightTimespan(2025, Date.Now.Year) & " CodingWonders Software")
        DynaLog.LogMessage("- Scintilla.NET: " &
                           "(c) " & GetCopyrightTimespan(2017, 2017) & " Jacob Slusser, " &
                           "(c) " & GetCopyrightTimespan(2020, 2022) & " VPKSoft, " &
                           "(c) " & GetCopyrightTimespan(2023, 2023) & " desjarlais")
        DynaLog.LogMessage("- ManagedDism: (c) " & GetCopyrightTimespan(2016, 2016) & " Jeff Kluge")
        DynaLog.LogMessage("- DarkUI: (c) " & GetCopyrightTimespan(2017, 2017) & " Robin Perris")
        DynaLog.LogMessage("- 7-Zip: (c) " & GetCopyrightTimespan(1999, 2025) & " Igor Pavlov" & CrLf &
                           "  LZFSE Compression Library: (c) " & GetCopyrightTimespan(2015, 2016) & " Apple Inc." & CrLf &
                           "  ZSTD Data Decompression: (c) Facebook, Inc. All rights reserved, (c) " & GetCopyrightTimespan(2023, 2025) & " Igor Pavlov" & CrLf &
                           "  XXH64 Code: (c) " & GetCopyrightTimespan(2012, 2021) & " Yann Collet, (c) " & GetCopyrightTimespan(2023, 2025) & " Igor Pavlov" & CrLf &
                           "  unRAR: (c) Alexander Roshal")        ' ugggghhhhhhh, why meta for zstd???
        DynaLog.LogMessage("- UnpEax: (c) " & GetCopyrightTimespan(2020, 2020) & " LioneL Christopher Chetty")
        DynaLog.LogMessage("- UnattendGen: " &
                           "(c) " & GetCopyrightTimespan(2024, Date.Now.Year) & " CodingWonders Software, " &
                           "(c) " & GetCopyrightTimespan(2024, Date.Now.Year) & " Christoph Schneegans")
        DynaLog.LogMessage("- Markdig: (c) " & GetCopyrightTimespan(2018, 2019) & " Alexandre Mutel")
        DynaLog.LogMessage("- Windows API Code Pack: " &
                           "(c) " & GetCopyrightTimespan(2009, 2010) & " Microsoft Corporation, " &
                           "modifications by Jacob Slusser (" & GetCopyrightTimespan(2014, 2014) & "), and by " &
                           "Peter William Wagner (" & GetCopyrightTimespan(2017, 2024) & ")")
        DynaLog.LogMessage("- INI File Parser: (c) " & GetCopyrightTimespan(2008, 2008) & " Ricardo Amores Hernández")
        DynaLog.LogMessage("- Active Directory Object Picker: Armand du Plessis, Tulpep")
        DynaLog.BeginLogging()
        DynaLog.LogMessage("-------- Powered by CONTEMPOR/\NE\/S Wave 1 PREVIEW 2 --------")
    End Sub

    Private Sub MainForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ' The Load handler performs asynchronous startup work. Request foreground activation only
        ' after WinForms has actually displayed the main window for the first time.
        BeginInvoke(New MethodInvoker(AddressOf ActivateMainWindowAfterShown))
    End Sub

    Private Sub ActivateMainWindowAfterShown()
        If IsDisposed OrElse Not Visible Then Exit Sub

        DynaLog.LogMessage("The main program window has been shown. Requesting foreground activation...")
        BringToFront()
        Dim normalZOrderRestored As Boolean = False
        Dim raisedAboveOtherWindows As Boolean = WindowHelper.RaiseWindowAndRestoreNormalZOrder(Handle, normalZOrderRestored)
        Dim foregroundRequested As Boolean = WindowHelper.RequestForegroundWindow(Handle)
        Activate()
        DynaLog.LogMessage("One-time window raise succeeded: " & raisedAboveOtherWindows &
                           "; normal z-order restored: " & normalZOrderRestored &
                           "; foreground activation request succeeded: " & foregroundRequested &
                           "; main window is foreground: " & WindowHelper.IsForegroundWindow(Handle) &
                           "; main window remains topmost: " & WindowHelper.IsTopMostWindow(Handle))
    End Sub

    Private Async Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitDynaLog()

        ' Prepare all user data
        DynaLog.LogMessage("Preparing user data...")
        UserDataManagerModule.CopyUserDataToProgramFiles()

        LoadThemes(True)
        ' Because of the DISM API, Windows 7 compatibility is out the window (no pun intended)
        If Environment.OSVersion.Version.Major = 6 And Environment.OSVersion.Version.Minor < 2 Then
            DynaLog.LogMessage("Windows 7 or an earlier version has been detected on this system. Program incompatible -- aborting any future procedures!")
            SplashScreen.Hide()
            MsgBox(LocalizationService.ForSection("Main.Messages")("Incompatible.Win7.Message"), vbOKOnly + vbCritical, "DISMTools")
            Environment.Exit(1)
        End If
        ' Detect .NET Framework version, as the program somehow runs without it
        Try
            DynaLog.LogMessage("Detecting target .NET Framework version...")
            Dim NDPCheckerReg As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full")
            Dim NDPReleaseInt As Integer = NDPCheckerReg.GetValue("Release")
            NDPCheckerReg.Close()
            DynaLog.LogMessage(".NET Framework detection results - Minimum threshold: 528040; Installed NDP version: " & NDPReleaseInt)
            ' Detect .NET Framework 4.8
            If NDPReleaseInt < 528040 Then
                DynaLog.LogMessage(NDPReleaseInt & " < 528040 - Incompatible .NET Framework Release -- aborting any future procedures!")
                SplashScreen.Hide()
                MsgBox(LocalizationService.ForSection("Main.Messages")("Requires.NET.Message"), vbOKOnly + vbCritical, "DISMTools")
                Environment.Exit(1)
            End If
        Catch ex As Exception

        End Try

        ' Detect presence of verified AME Playbooks in current system. This is NOT a way to block these projects, but a way to help isolate
        ' specific program bugs on those types of systems
        If PlaybookDetector.DetectInstalledPlaybook(PlaybookDetector.VerifiedPlaybooks.AtlasOS) Then
            DynaLog.LogMessage("Atlas OS has been detected on this system. There may be compatibility issues with DISMTools on your system", False)
        End If
        If PlaybookDetector.DetectInstalledPlaybook(PlaybookDetector.VerifiedPlaybooks.ReviOS) Then
            DynaLog.LogMessage("Revision (ReviOS) has been detected on this system. There may be compatibility issues with DISMTools on your system", False)
        End If
        If PlaybookDetector.DetectInstalledPlaybook(PlaybookDetector.VerifiedPlaybooks.AME) Then
            DynaLog.LogMessage("AME 10/11 has been detected on this system. There may be compatibility issues with DISMTools on your system", False)
        End If

        If Not Directory.Exists(Application.StartupPath & "\logs") Then
            Try
                Directory.CreateDirectory(Application.StartupPath & "\logs")
            Catch ex As Exception
                ' don't create such a folder then
            End Try
        End If
        If Not Debugger.IsAttached Then SplashScreen.Show()
        Thread.Sleep(2000)
        ' I once tested this on a computer which didn't require me to ask for admin privileges. This is a requirement of DISM. Check this
        If Not My.User.IsInRole(ApplicationServices.BuiltInRole.Administrator) Then
            DynaLog.LogMessage("This user is not part of the Administrators group/role -- aborting any future procedures!")
            SplashScreen.Hide()
            MsgBox(LocalizationService.ForSection("Main.Messages")("Run.Admin.Message"), vbOKOnly + vbCritical, "DISMTools")
            Environment.Exit(1)
        End If
        Visible = False
        DynaLog.LogMessage("Detecting program arguments...")
        GetArguments()
        ' Detect mounted images
        DetectMountedImages(True)
        DynaLog.LogMessage("Finished detecting mounted images. Continuing program startup...")
        Control.CheckForIllegalCrossThreadCalls = False
        BranchTSMI.Text = dtBranch
        If Debugger.IsAttached Then
            DynaLog.LogMessage("Debugger attached. Showing information...")
            BranchTSMI.Visible = True
            Text &= " (debug mode)"
        End If
        ' Load theme files (EXPERIMENTS ONLY)
        ThemeHelper.LoadThemes()
        ' Read settings file
        If File.Exists(Application.StartupPath & "\settings.ini") Then
            DynaLog.LogMessage("A settings file has been found. Loading settings...")
            PerformSettingFileValidation()
            Dim SettingReader As String = File.ReadAllText(Application.StartupPath & "\settings.ini", UTF8)
            If SettingReader.Contains("SaveOnSettingsIni=1") Or SettingReader.Contains("SaveOnSettingsIni = 1") Then
                DynaLog.LogMessage("Settings are stored in the settings file (INI). Looking at them...")
                LoadDTSettings(1)
            ElseIf SettingReader.Contains("SaveOnSettingsIni=0") Or SettingReader.Contains("SaveOnSettingsIni = 0") Then
                DynaLog.LogMessage("Settings are stored in the registry. Looking at them...")
                LoadDTSettings(0)
            End If
        Else
            DynaLog.LogMessage("No settings file has been found. Launching Initial Setup Wizard (ISW)...")
            SplashScreen.Hide()
            PrgSetup.ShowDialog()
            LoadDTSettings(1)
        End If
        imgStatus = 0
        TimeLabel.Visible = ShowDateAndTime
        ChangeImgStatus()
        If DismExe <> "" Then
            DynaLog.LogMessage("Checking version of DISM executable...")
            DismVersionChecker = FileVersionInfo.GetVersionInfo(DismExe)
            DynaLog.LogMessage("DISM Executable Version: " & DismVersionChecker.ProductVersion)
        End If
        If Environment.GetCommandLineArgs().Contains("/english") Then
            DynaLog.LogMessage("DISMTools is forced to use English as its language because /english has been passed. Changing language...")
            LanguageCode = LocalizationService.DefaultCultureCode
            ApplyLanguage(LanguageCode)
        End If
        UnblockPSHelpers()
        If StartupRemount Then RemountOrphanedImages() Else HasRemounted = True
        While Not HasRemounted
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        If StartupUpdateCheck And Not SkipUpdates Then
            DynaLog.LogMessage("Checking program updates...")
            UpdCheckerBW.RunWorkerAsync()
        Else
            UpdatePanel.Visible = False
        End If
        DynaLog.LogMessage("Beginning to detect mounted images in the background. No logging will be performed here")
        If Not MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.RunWorkerAsync()
        DynaLog.LogMessage("Enabling image status watchers...")
        WatcherTimer.Enabled = True
        If dtBranch.Contains("pre") And Not Debugger.IsAttached Then
            VersionTSMI.Visible = True
        Else
            VersionTSMI.Visible = False
        End If
        If Not Debugger.IsAttached Then SplashScreen.Close()
        DynaLog.LogMessage("Repositioning window according to window parameters...")
        WndWidth = Width
        WndHeight = Height
        WndLeft = Left
        WndTop = Top
        If Left < 0 And Top < 0 Then
            ' Center form
            Location = New Point((Screen.FromControl(Me).WorkingArea.Width - Width) / 2, (Screen.FromControl(Me).WorkingArea.Height - Height) / 2)
        End If
        DynaLog.LogMessage("Showing the main program window...")
        Visible = True
        If argProjPath <> "" Then
            DynaLog.LogMessage("A project path has been specified with /load. Loading project...")
            HomePanel.Visible = False
            'Visible = True
            ProgressPanel.OperationNum = 990
            LoadDTProj(argProjPath, Path.GetFileNameWithoutExtension(argProjPath), True, False)
        End If
        If argOnline Then
            DynaLog.LogMessage("The /online argument has been passed. Entering this mode...")
            BeginOnlineManagement(True)
        End If
        If argOffline And drivePath <> "" Then
            DynaLog.LogMessage("The /offline argument has been passed. Entering this mode...")
            BeginOfflineManagement(drivePath)
        End If
        Timer1.Enabled = True
        LinkLabel12.LinkColor = CurrentTheme.ForegroundColor
        LinkLabel13.LinkColor = CurrentTheme.DisabledForegroundColor

        SplitContainer1.SplitterDistance = WindowHelper.ScaleLogical(InfinityStartPanel.Width / 2)

        DynaLog.LogMessage("Getting official news...")
        FeedWorker.RunWorkerAsync()
        Timer2.Enabled = True
        If Not File.Exists(Application.StartupPath & "\recents.xml") Then
            DynaLog.LogMessage("The recents list file does not exist. Creating...")
            File.Create(Application.StartupPath & "\recents.xml")
        Else
            DynaLog.LogMessage("Loading Recents list items...")
            RecentList = LoadRecents(Application.StartupPath & "\recents.xml")
            If RecentList IsNot Nothing Then
                If RecentList.Count > 0 Then
                    DynaLog.LogMessage("Showing items...")
                    For Each Project In RecentList
                        RecentsLV.Items.Add(If(Project.ProjName <> "", Project.ProjName,
                                                                       Path.GetFileNameWithoutExtension(Project.ProjPath)))
                    Next
                    Try
                        RecentProject1ToolStripMenuItem.Text = " "
                        RecentProject2ToolStripMenuItem.Text = " "
                        RecentProject3ToolStripMenuItem.Text = " "
                        RecentProject4ToolStripMenuItem.Text = " "
                        RecentProject5ToolStripMenuItem.Text = " "
                        RecentProject6ToolStripMenuItem.Text = " "
                        RecentProject7ToolStripMenuItem.Text = " "
                        RecentProject8ToolStripMenuItem.Text = " "
                        RecentProject9ToolStripMenuItem.Text = " "
                        RecentProject10ToolStripMenuItem.Text = " "

                        ' Reconfigure text
                        RecentProject1ToolStripMenuItem.Text = RecentList(0).ProjPath
                        RecentProject1ToolStripMenuItem.Visible = True
                        RecentProject2ToolStripMenuItem.Text = RecentList(1).ProjPath
                        RecentProject2ToolStripMenuItem.Visible = True
                        RecentProject3ToolStripMenuItem.Text = RecentList(2).ProjPath
                        RecentProject3ToolStripMenuItem.Visible = True
                        RecentProject4ToolStripMenuItem.Text = RecentList(3).ProjPath
                        RecentProject4ToolStripMenuItem.Visible = True
                        RecentProject5ToolStripMenuItem.Text = RecentList(4).ProjPath
                        RecentProject5ToolStripMenuItem.Visible = True
                        RecentProject6ToolStripMenuItem.Text = RecentList(5).ProjPath
                        RecentProject6ToolStripMenuItem.Visible = True
                        RecentProject7ToolStripMenuItem.Text = RecentList(6).ProjPath
                        RecentProject7ToolStripMenuItem.Visible = True
                        RecentProject8ToolStripMenuItem.Text = RecentList(7).ProjPath
                        RecentProject8ToolStripMenuItem.Visible = True
                        RecentProject9ToolStripMenuItem.Text = RecentList(8).ProjPath
                        RecentProject9ToolStripMenuItem.Visible = True
                        RecentProject10ToolStripMenuItem.Text = RecentList(9).ProjPath
                        RecentProject10ToolStripMenuItem.Visible = True
                    Catch ex As Exception
                        ' Don't do anything special here
                    End Try
                End If
            End If
        End If

        ' Fill in INFINITY HOME information
        SplitContainer1.SplitterDistance = WindowHelper.ScaleLogical(428)
        Await Task.Run(Sub()
                           DisplayInfinityComputerInformation()
                       End Sub)

        ' Get videos
        ListView2.Items.Clear()

        DynaLog.LogMessage("Getting videos...")

        VideoGetterBW.RunWorkerAsync()

        ' Detect custom themes
        Try
            DynaLog.LogMessage("Detecting presence of custom themes...")
            Dim themeRk As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\ThemeManager")
            Dim ThemeDll As String = themeRk.GetValue("DllName")
            Dim PrePol As String = themeRk.GetValue("PrePolicy-DllName")
            themeRk.Close()
            DynaLog.LogMessage("System Theme   : " & Quote & ThemeDll & Quote)
            DynaLog.LogMessage("PrePolicy Theme: " & Quote & PrePol & Quote)
            If Not ThemeDll.Equals(PrePol, StringComparison.OrdinalIgnoreCase) Then
                DynaLog.LogMessage("System Theme and PrePolicy Theme are different.")
                DynaLog.LogMessage("A custom theme has been detected. There may be visual issues with DISMTools")
                Dim msg As String = ""
                Dim titleMsg As String = ""
                        titleMsg = LocalizationService.ForSection("Main.InitDynaLog")("Beware.Custom.Themes.Title")
                        msg = LocalizationService.ForSection("Main.InitDynaLog")("DISM.Tools.Detected.Message")
                MsgBox(msg, vbOKOnly + vbExclamation, titleMsg)
            Else
                DynaLog.LogMessage("System Theme and PrePolicy Theme are the same.")
            End If
        Catch ex As Exception

        End Try

        Try
            DynaLog.LogMessage("Getting device DPI...")
            Dim dx As Single, dy As Single
            Dim g As Graphics = CreateGraphics()

            Try
                dx = g.DpiX
                dy = g.DpiY
            Finally
                g.Dispose()
            End Try

            DynaLog.LogMessage("DPI X-axis: " & dx)
            DynaLog.LogMessage("DPI Y-axis: " & dy)

            ' 125% display scaling is equal to 120 DPI. Higher display scaling settings make
            ' some items in the program not look correctly. It is better to tell the user
            ' about this.
            If dx > 120 Or dy > 120 Then
                DynaLog.LogMessage("Display scaling is over 125%. The program may not look correctly...")
                MsgBox(LocalizationService.ForSection("Main.Messages")("DISM.Tools.Detected.Message"),
                       vbOKOnly + vbInformation, LocalizationService.ForSection("Main.Messages")("Higher.Display.Scaling.Title"))
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not check DPI settings. Error message: " & ex.Message)
        End Try

        If DetectPossibleADKs() = 1 Then
            DynaLog.LogMessage("An ADK has been installed but is not detected by DISMTools")
            Dim msg As String = ""
            msg = LocalizationService.ForSection("Main.Messages")("DISM.Tools.Found.Message")
            If MsgBox(msg, vbYesNo + vbQuestion, LocalizationService.ForSection("Main.Messages")("Possible.ADK.Title")) = MsgBoxResult.Yes Then
                Try
                    DynaLog.LogMessage("Creating keys...")
                    Dim AdkProc As New Process()
                    AdkProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\regedit.exe"
                    AdkProc.StartInfo.Arguments = "/s " & Quote & Application.StartupPath & "\DT_WinADK.reg"
                    AdkProc.StartInfo.CreateNoWindow = True
                    AdkProc.Start()
                    AdkProc.WaitForExit()
                    AdkProc.Dispose()
                Catch ex As Exception

                End Try
            End If
        End If

        DynaLog.LogMessage("Checking boot mode...")
        DynaLog.LogMessage(SystemInformation.BootMode)
        If SystemInformation.BootMode <> BootMode.Normal Then
            DynaLog.LogMessage("This system is in limp home mode. Offering choice to enter online installation management mode...")
            Dim safeModeMessage As String = LocalizationService.ForSection("Main.Messages")("SafeMode.Prompt")
            If MsgBox(safeModeMessage, vbYesNo + vbQuestion, LocalizationService.ForSection("Main.Messages")("Windows.Title")) = MsgBoxResult.Yes Then
                DynaLog.LogMessage("It is official. We are entering online installation management mode to (try to) save this installation...")
                BeginOnlineManagement(False)
            End If
        End If

        If IsFirstTime Then
            Dim tourMessage As String = LocalizationService.ForSection("Main.Messages")("Tour.Prompt")
            If MsgBox(tourMessage, vbYesNo + vbQuestion, LocalizationService.ForSection("Main.Messages")("Getting.Started.DISM.Title")) = MsgBoxResult.Yes Then
                If Directory.Exists(Path.Combine(Application.StartupPath, "docs", "tour")) Then
                    DynaLog.LogMessage("Tour directory exists. Starting the tour!")

                    Dim languageCode As String = LocalizationService.GetDocumentationLanguageCode()

                    tourServer.StartServer()
                    If tourServer.IsListenerAlive() Then
                        Process.Start(String.Format("http://localhost:2022/{0}/tour-start.html", languageCode))
                        TourActionsTSMI.Visible = True
                    End If
                End If
            End If
        End If

        ' For the PXE Helper Server menu item to be usable, we need to be on a server system.
        Dim InstallationTypeRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion", False)
        Dim InstallationType As String = InstallationTypeRk.GetValue("InstallationType", "")
        InstallationTypeRk.Close()

        PxeHelperServersTSMI.Enabled = InstallationType.ToLower().Contains("server")
        UploadThisImageToMyWDSServerToolStripMenuItem.Enabled = InstallationType.ToLower().Contains("server")

        ' For some reason, on Windows 11 it does not focus the window. Keyboard users may suffer if we don't correct this.
        Focus()

        ' On higher DPI settings listview column widths don't adapt correctly, causing stuff to be even more truncated than
        ' necessary. Scale these appropriately
        ColumnHeader3.Width = WindowHelper.ScaleLogical(163)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(375)

        If InstallationType.Equals("Server Core", StringComparison.InvariantCultureIgnoreCase) Then
            MessageBox.Show(LocalizationService.ForSection("Main.Messages")("DISM.Tools.Running.Message"),
                            LocalizationService.ForSection("Main.Messages")("ServerCore.Title"), MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        ' If the window size is lower than 720p (1280x720), then we'll make it 1280x720, since,
        ' even though we accept window sizes LOWER than 720p, we consider these either obscure
        ' window sizes or unacceptable (such as 800x600).
        If Width < WindowHelper.ScaleLogical(1280) And Height < WindowHelper.ScaleLogical(720) Then
            Size = WindowHelper.ScaleSizeLogical(1280, 720)
        End If

        ' The web browser needs to be initialized way after everything else because ActiveX objects don't like
        ' to be initialized in MTA threads (this async thread is one of those).
        NewsFeedWebContent = New WebBrowser() With {
            .Dock = DockStyle.Fill,
            .ScriptErrorsSuppressed = True,
            .AllowWebBrowserDrop = False
        }
        NewsContentPreviewerPanel.Controls.Add(NewsFeedWebContent)
        NewsFeedWebContent.BringToFront()
        AddHandler NewsFeedWebContent.DocumentCompleted, AddressOf NewsFeedWebContent_DocumentCompleted

        ' Load the facts
        Dim FactsFile As String = Path.Combine(Application.StartupPath, "bin", "facts.xml")
        If File.Exists(FactsFile) Then
            Try
                Dim factsDeserializer As New XmlSerializer(GetType(InfinityFactsDocument))
                Using fs As FileStream = File.OpenRead(FactsFile)
                    Dim document As InfinityFactsDocument = CType(factsDeserializer.Deserialize(fs), InfinityFactsDocument)
                    InfinityHomeFacts = document.Facts
                End Using

                If InfinityHomeFacts.Any() Then
                    ' Show a random one
                    FactLabel.Text = InfinityHomeFacts.ElementAt(New Random().Next(InfinityHomeFacts.Count)).Message
                End If
            Catch ex As Exception
                DynaLog.LogMessage("Could not load facts: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub DisplayInfinityComputerInformation()
        Try
            ' Wallpaper
            Try
                Dim WallpaperPath As String = ""
                ' Wallpaper may be defined by group policy; check there first
                Try
                    Dim WallpaperPolicyRk As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Policies\System")
                    WallpaperPath = WallpaperPolicyRk.GetValue("Wallpaper", "")
                    WallpaperPolicyRk.Close()
                    If WallpaperPath = "" OrElse Not File.Exists(WallpaperPath) Then Throw New Exception()
                Catch ex As Exception
                    ' Ignore and use general wallpaper
                    Dim WallpaperRk As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\Desktop", False)
                    WallpaperPath = WallpaperRk.GetValue("WallPaper", "")
                    WallpaperRk.Close()
                End Try
                ComputerWallpaperPB.Image = Image.FromFile(WallpaperPath)
            Catch ex As Exception

            End Try

            ' Localizable strings
            Dim BuildStr As String = "",
                SysMemStr As String = "",
                CurDiskStr As String = "",
                NoDomStr As String = "",
                DomainStr As String = "",
                BDCStr As String = "",
                PDCStr As String = "",
                NoIPStr As String = "",
                ManualIPStr As String = "",
                DHCPStr As String = ""

            BuildStr = LocalizationService.ForSection("Main.ComputerInfo")("Build.Label")
            SysMemStr = LocalizationService.ForSection("Main.ComputerInfo")("SystemMemory.Label")
            CurDiskStr = LocalizationService.ForSection("Main.ComputerInfo")("UsedOut.Label")
            NoDomStr = LocalizationService.ForSection("Main.ComputerInfo")("Part.Domain.Label")
            DomainStr = LocalizationService.ForSection("Main.ComputerInfo")("PartDomain.Label")
            BDCStr = LocalizationService.ForSection("Main.ComputerInfo")("Backup.Domain.Label")
            PDCStr = LocalizationService.ForSection("Main.ComputerInfo")("Primary.Domain.Label")
            NoIPStr = LocalizationService.ForSection("Main.ComputerInfo")("ConnectedNetwork.Label")
            ManualIPStr = LocalizationService.ForSection("Main.ComputerInfo")("Manual.Label")
            DHCPStr = LocalizationService.ForSection("Main.ComputerInfo")("Automatic.Assigned.Label")

            ' Computer Information
            ComputerOSLabel.Text = String.Format("{0} ({1} {2})", My.Computer.Info.OSFullName, BuildStr, Environment.OSVersion.Version.Build)
            Dim ComputerSystemMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery("SELECT Manufacturer, Model, DNSHostName, TotalPhysicalMemory, Domain, DomainRole FROM Win32_ComputerSystem")
            Dim ComputerProcMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery("SELECT Name FROM Win32_Processor")
            Dim ComputerCurrentVolMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT Label, FreeSpace, Capacity FROM Win32_Volume WHERE Name = {0}{1}{0}", Quote, WMIHelper.GetEscapedValue(Environment.GetEnvironmentVariable("SYSTEMDRIVE") & "\")))
            Dim ComputerSystemProps As Dictionary(Of String, Object) = WMIHelper.GetObjectValues(ComputerSystemMOC(0), "Manufacturer", "Model", "DNSHostName", "TotalPhysicalMemory", "Domain", "DomainRole")
            ComputerNameLabel.Text = ComputerSystemProps("DNSHostName")
            ComputerModelLabel.Text = ComputerSystemProps("Model")
            ComputerProcessorLabel.Text = WMIHelper.GetObjectValue(ComputerProcMOC(0), "Name")
            ComputerMemoryLabel.Text = String.Format("{0} {1}", Converters.BytesToReadableSize(ComputerSystemProps("TotalPhysicalMemory"),
                                                                                               LanguageCode.Equals("fr-FR", StringComparison.OrdinalIgnoreCase)), SysMemStr)
            Try
                Dim CurrentVolProps As Dictionary(Of String, Object) = WMIHelper.GetObjectValues(ComputerCurrentVolMOC(0), "Capacity", "FreeSpace", "Label"),
                    DiskCapacity As Long = CurrentVolProps("Capacity"),
                    DiskFreeSpace As Long = CurrentVolProps("FreeSpace"),
                    DiskUsedSpace As Long = DiskCapacity - DiskFreeSpace,
                    DiskVolumeLetter As String = Environment.GetEnvironmentVariable("SYSTEMDRIVE"),
                    DiskLabel As String = CurrentVolProps("Label")
                ComputerStorageLabel.Text = String.Format("{0}\{1}: {2} {3} {4} ({5}%)", DiskVolumeLetter, If(DiskLabel <> "", String.Format(" ({0})", DiskLabel), ""),
                                                                                            Converters.BytesToReadableSize(DiskUsedSpace, LanguageCode.Equals("fr-FR", StringComparison.OrdinalIgnoreCase)),
                                                                                            CurDiskStr,
                                                                                            Converters.BytesToReadableSize(DiskCapacity, LanguageCode.Equals("fr-FR", StringComparison.OrdinalIgnoreCase)),
                                                                                            Math.Round((DiskUsedSpace / DiskCapacity) * 100, 2))
            Catch ex As Exception
                DynaLog.LogMessage("Could not display disk information: " & ex.Message)
            End Try
            Select Case ComputerSystemProps("DomainRole")
                Case DomainRole.StandaloneWorkstation, DomainRole.StandaloneServer
                    ComputerDomainStatusLabel.Text = NoDomStr
                Case DomainRole.MemberWorkstation, DomainRole.MemberServer
                    ComputerDomainStatusLabel.Text = DomainStr
                Case DomainRole.BackupDomainController
                    ComputerDomainStatusLabel.Text = BDCStr
                Case DomainRole.PrimaryDomainController
                    ComputerDomainStatusLabel.Text = PDCStr
            End Select
            ComputerDomainWorkgroupLabel.Text = ComputerSystemProps("Domain")
            Try
                Dim RouteTableMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery("SELECT InterfaceIndex FROM Win32_IP4RouteTable WHERE Destination = '0.0.0.0'")
                Dim currentNetAdapterIndex As UInteger = WMIHelper.GetObjectValue(RouteTableMOC(0), "InterfaceIndex")
                Dim ComputerNetworkMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT DHCPEnabled FROM Win32_NetworkAdapterConfiguration WHERE InterfaceIndex = {0}", currentNetAdapterIndex))
                Dim DhcpEnabled As Boolean = WMIHelper.GetObjectValue(ComputerNetworkMOC(0), "DHCPEnabled")
                ComputerDhcpStatusLabel.Text = If(DhcpEnabled, DHCPStr, ManualIPStr)
            Catch ex As Exception
                ComputerDhcpStatusLabel.Text = NoIPStr
            End Try
        Catch ex As Exception
            DynaLog.LogMessage("Could not display computer info: " & ex.Message)
        End Try
    End Sub

    Function GetItemThumbnail(videoId As String) As Image
        Try
            DynaLog.LogMessage("Getting YouTube thumbnail of video ID " & videoId & " and modifying it accordingly...")
            Dim thumbnailURI As String = "https://img.youtube.com/vi/" & videoId & "/maxresdefault.jpg"
            Using client As WebClient = New WebClient()
                Dim imgBytes() As Byte = client.DownloadData(thumbnailURI)
                Using ms As MemoryStream = New MemoryStream(imgBytes)
                    Dim ogImg As Image = Image.FromStream(ms)
                    Dim thumbnail As Image = ResizeThumbnail(ogImg, 160, 90)
                    Return thumbnail
                End Using
            End Using
        Catch ex As Exception
            Return Nothing
        End Try
        Return Nothing
    End Function

    Function ResizeThumbnail(img As Image, width As Integer, height As Integer) As Image
        Try
            Dim resImg As Bitmap = New Bitmap(width, height)
            Using g As Graphics = Graphics.FromImage(resImg)
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.DrawImage(img, 0, 0, width, height)
            End Using
            Return resImg
        Catch ex As Exception
            Return Nothing
        End Try
        Return Nothing
    End Function

    Function CombineImages(thumbnail As Image) As Image
        Try
            Dim play As Image = My.Resources.video_play
            Dim combinedImage As Bitmap = New Bitmap(thumbnail.Width, thumbnail.Height)
            Using g As Graphics = Graphics.FromImage(combinedImage)
                g.DrawImage(thumbnail, 0, 0, thumbnail.Width, thumbnail.Height)
                ' Draw Play symbol
                Dim pX As Integer = (thumbnail.Width - play.Width) / 2
                Dim pY As Integer = (thumbnail.Height - play.Height) / 2
                g.DrawImage(play, pX, pY, play.Width, play.Height)
            End Using
            Return combinedImage
        Catch ex As Exception
            Return Nothing
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Detects all mounted images and their state. Calls the DISM API at program startup
    ''' </summary>
    ''' <param name="DebugLog">Check if the program should output debug information. This is always called at program startup, but never after</param>
    ''' <remarks>This yields results for the MountedImageImgFiles, MountedImageMountDirs, MountedImageImgIndexes, MountedImageMountedRewr and MountedImageImgStatuses string arrays in code</remarks>
    Sub DetectMountedImages(DebugLog As Boolean)
        If DebugLog Then DynaLog.LogMessage("Getting mounted images...")
        If MountedImageList IsNot Nothing Then
            If DebugLog Then DynaLog.LogMessage("Clearing lists...")
            MountedImageList.Clear()
        End If
        Try
            If DebugLog Then DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
            If DebugLog Then DynaLog.LogMessage("Calling API function to grab mounted images...")
            Dim MountedImgs As DismMountedImageInfoCollection = DismApi.GetMountedImages()
            If DebugLog Then DynaLog.LogMessage(MountedImgs.Count & " mounted image(s) have been detected on this system. Grabbing information...")
            For Each imageInfo As DismMountedImageInfo In MountedImgs
                If DebugLog Then DynaLog.LogMessage("Image information:")
                If DebugLog Then DynaLog.LogMessage("- Image file : " & imageInfo.ImageFilePath)
                If DebugLog Then DynaLog.LogMessage("- Image index : " & imageInfo.ImageIndex)
                If DebugLog Then DynaLog.LogMessage("- Mount directory : " & imageInfo.MountPath)
                If DebugLog Then DynaLog.LogMessage("- Mount status : " & imageInfo.MountStatus & If(imageInfo.MountStatus = DismMountStatus.Ok, " (OK)", If(imageInfo.MountStatus = DismMountStatus.NeedsRemount, " (Orphaned)", " (Invalid)")))
                If DebugLog Then DynaLog.LogMessage("- Mount mode : " & imageInfo.MountMode & If(imageInfo.MountMode = DismMountMode.ReadWrite, " (Write permissions enabled)", "(Write permissions disabled)"))

                MountedImageList.Add(New WindowsImage(imageInfo.ImageFilePath, imageInfo.ImageIndex, imageInfo.MountPath, imageInfo.MountStatus, imageInfo.MountMode))
            Next
            RaiseMountedImagesUpdated()
        Catch ex As Exception
            DynaLog.LogMessage("Could not detect mounted images. Error message: " & ex.Message)
        Finally
            Try
                If DebugLog Then DynaLog.LogMessage("Shutting down API...")
                DismApi.Shutdown()
            Catch ex As Exception
                ' ignore
            End Try
        End Try
    End Sub

    ''' <summary>
    ''' Remounts orphaned images (images in need of a servicing session reload)
    ''' </summary>
    ''' <remarks></remarks>
    Sub RemountOrphanedImages()
        DynaLog.LogMessage("Do we REALLY have to do this? Let's find out!")
        Dim NeedToRemount As Boolean = False
        NeedToRemount = MountedImageList.Any(Function(mountedImage) mountedImage.ImageMountStatus = DismMountStatus.NeedsRemount)
        DynaLog.LogMessage(If(NeedToRemount, "Remounting any orphaned images...", "There is no need to do this. Skipping..."))
        If NeedToRemount Then AutoReloadForm.ShowDialog()
        HasRemounted = True
    End Sub

    Sub CheckForUpdates(branch As String)
        DynaLog.LogMessage("Checking for program updates...")
        UpdateLink.LinkArea = New LinkArea(0, 0)
                UpdateLink.Text = LocalizationService.ForSection("Main.CheckForUpdates")("CheckingUpdates.Link")
        Dim latestVer As String = ""
        Using client As New WebClient()
            DynaLog.LogMessage("Downloading update information from DISMTools repository...")
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            Try
                client.DownloadFile("https://raw.githubusercontent.com/CodingWonders/dt-update-files/refs/heads/main/" & If(branch.Contains("pre"), "preview.ini", "stable.ini"), Application.StartupPath & "\info.ini")
            Catch ex As WebException
                DynaLog.LogMessage("Could not grab update info. Well, I guess that we can't update!")
                Debug.WriteLine("We couldn't fetch the necessary update information. Reason:" & CrLf & ex.Status.ToString())
                UpdatePanel.Visible = False
                Exit Sub
            End Try
            DynaLog.LogMessage("Reading update information...")
            If File.Exists(Application.StartupPath & "\info.ini") Then
                Dim UpdateInfoFileLines As String() = File.ReadAllLines(Application.StartupPath & "\info.ini")
                For Each Line In UpdateInfoFileLines
                    If Line.StartsWith("LatestVer") Then
                        DynaLog.LogMessage("Getting latest version...")
                        latestVer = Line.Replace("LatestVer = ", "").Trim()
                    End If
                Next
                File.Delete(Application.StartupPath & "\info.ini")
                DynaLog.LogMessage("Comparing versions...")
                Dim fv As String = My.Application.Info.Version.ToString()
                If fv = latestVer Or New Version(fv) > New Version(latestVer) Then
                    DynaLog.LogMessage("The program is up to date or is a newer build that can't be updated.")
                    UpdatePanel.Visible = False
                Else
                    DynaLog.LogMessage("The program is outdated. Recommending the user to update in a subtle way...")
                    UpdateLink.Text = LocalizationService.ForSection("Main.CheckForUpdates")("NewVersion.Available.Link")
                    UpdateLink.LinkArea = LocalizationService.GetLinkArea(UpdateLink.Text, LocalizationService.ForSection("Main.CheckForUpdates")("Learn.Link"))
                    UpdatePanel.Visible = True
                End If
            End If
        End Using
    End Sub

    Private Sub UnblockPSHelpers()
        DynaLog.LogMessage("Unblocking PowerShell scripts for them to run freely (for those who want technical terms, removing Intenet zone alternate data streams from NTFS)...")
        Dim PSUnblocker As New Process()
        PSUnblocker.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\System32\WindowsPowerShell\v1.0\powershell.exe"
        PSUnblocker.StartInfo.Arguments = "-executionpolicy unrestricted -command Unblock-File " & Quote & Application.StartupPath & "\bin\extps1\*.*" & Quote
        PSUnblocker.StartInfo.CreateNoWindow = True
        PSUnblocker.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        PSUnblocker.Start()
    End Sub

    Sub ChangeImgStatus()
        DynaLog.LogMessage("Determining if an image is mounted in the project. This is also run at startup...")
        If imgStatus = 0 Then
            DynaLog.LogMessage("Nothing/Zero/Zilch/Nada. Report so")
                    Label50.Text = LocalizationService.ForSection("Main.ChangeImgStatus")("No.Button")
            LinkLabel14.Visible = True
        Else
            DynaLog.LogMessage("Yes, we have an image mounted here...")
                    Label50.Text = LocalizationService.ForSection("Main.ChangeImgStatus")("Yes.Button")
            LinkLabel14.Visible = False
        End If
    End Sub

    Sub LoadDTSettings(LoadMode As Integer, Optional ForceINILoad As Boolean = False)
        ' LoadMode = 0; load from registry
        ' LoadMode = 1; load from INI file
        DynaLog.LogMessage("Preparing to get settings (Load Mode: " & LoadMode & ")...")
        If LoadMode = 0 Then
            DynaLog.LogMessage("Load Mode is 0 -- Getting from Registry...")
            If File.Exists(Application.StartupPath & "\portable") Then
                DynaLog.LogMessage("Portable copy marker detected. Loading from INI...")
                SaveOnSettingsIni = True
                LoadDTSettings(1)
                Exit Sub
            End If
            Try
                DynaLog.LogMessage("Preparing to grab values...")
                Dim KeyStr As String = "Software\DISMTools\" & If(dtBranch.Contains("pre"), "Preview", "Stable")
                Dim Key As RegistryKey = Registry.CurrentUser.OpenSubKey(KeyStr)
                Dim PrgKey As RegistryKey = Key.OpenSubKey("Program")
                DismExe = PrgKey.GetValue("DismExe").ToString().Replace(Quote, "").Trim()
                SaveOnSettingsIni = (CInt(PrgKey.GetValue("SaveOnSettingsIni")) = 1)
                PrgKey.Close()
                Dim PersKey As RegistryKey = Key.OpenSubKey("Personalization")
                ColorMode = PersKey.GetValue("ColorMode")
                DarkThemeIndex = PersKey.GetValue("ColorTheme_Dark")
                LightThemeIndex = PersKey.GetValue("ColorTheme_Light")
                LanguageCode = LocalizationService.ResolveStartupCultureCode(PersKey.GetValue("LanguageCode", LocalizationService.DefaultCultureCode))
                LogFont = PersKey.GetValue("LogFont").ToString()
                LogFontSize = CInt(PersKey.GetValue("LogFontSi"))
                LogFontIsBold = (CInt(PersKey.GetValue("LogFontBold")) = 1)
                ProgressPanelStyle = CInt(PersKey.GetValue("SecondaryProgressPanelStyle"))
                AllCaps = (CInt(PersKey.GetValue("AllCaps")) = 1)
                ExpandedProgressPanel = (CInt(PersKey.GetValue("ExpandedProgressPanel")) = 1)
                ShowDateAndTime = (CInt(PersKey.GetValue("ShowDateAndTime")) = 1)
                ProjectView.Visible = True
                PersKey.Close()
                Dim LogKey As RegistryKey = Key.OpenSubKey("Logs")
                LogFile = LogKey.GetValue("LogFile").ToString().Replace(Quote, "").Trim()
                LogLevel = CInt(LogKey.GetValue("LogLevel"))
                AutoLogs = (CInt(LogKey.GetValue("AutoLogs")) = 1)
                SystemEditor = LogKey.GetValue("SystemEditor").ToString().Replace(Quote, "").Trim()
                EnableDynaLog = (CInt(LogKey.GetValue("EnableDynaLog")) = 1)
                LogKey.Close()
                Dim ImgOpKey As RegistryKey = Key.OpenSubKey("ImgOps")
                QuietOperations = (CInt(ImgOpKey.GetValue("Quiet")) = 1)
                SysNoRestart = (CInt(ImgOpKey.GetValue("NoRestart")) = 1)
                NoNTSamMappings = (CInt(ImgOpKey.GetValue("NoNTSamMappings")) = 1)
                PEHelper_UnattendedFile = ImgOpKey.GetValue("PEHelper.UnattendedFile").ToString().Replace(Quote, "").Trim()
                PEHelper_CopyToVentoy = (CInt(ImgOpKey.GetValue("PEHelper.CopyToVentoy")) = 1)
                PEHelper_Use2023EFI = (CInt(ImgOpKey.GetValue("PEHelper.Use2023EFI")) = 1)
                PEHelper_IncludeSysDrvs = (CInt(ImgOpKey.GetValue("PEHelper.IncludeSysDrvs")) = 1)
                AppxDisplayNameFormatOnRemoval = CInt(ImgOpKey.GetValue("AppxRemovalDisplayNameFormat"))
                PreventSystemFromSleeping = CInt(ImgOpKey.GetValue("PreventSystemFromSleeping", 1)) = 1
                HumanizeDates = CInt(ImgOpKey.GetValue("HumanizeDates", 1)) = 1
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
                Dim ShutdownKey As RegistryKey = Key.OpenSubKey("Shutdown")
                AutoCleanMounts = (CInt(ShutdownKey.GetValue("AutoCleanMounts")) = 1)
                ShutdownKey.Close()
                Dim WndKey As RegistryKey = Key.OpenSubKey("WndParams")
                Width = WindowHelper.ScaleLogical(CInt(WndKey.GetValue("WndWidth")))
                Height = WindowHelper.ScaleLogical(CInt(WndKey.GetValue("WndHeight")))
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
                Dim SearchKey As RegistryKey = Key.OpenSubKey("SearchSettings")
                SearchEngineName = SearchKey.GetValue("EngineName").ToString().Replace(Quote, "").Trim()
                SearchEngineAITolerance = CInt(SearchKey.GetValue("AITolerance"))
                SearchKey.Close()
                Dim PEPolicyKey As RegistryKey = Key.OpenSubKey("PEPolicy")
                ShowWatermark = (CInt(PEPolicyKey.GetValue("ShowWatermark", 0)) = 1)
                WDSHCGraphoView = (CInt(PEPolicyKey.GetValue("WDSHCGraphoView", 1)) = 1)
                DTDimShowPnputilOut = (CInt(PEPolicyKey.GetValue("DTDimShowPnputilOut", 1)) = 1)
                WDSHCConnAttempts = (CInt(PEPolicyKey.GetValue("WDSHCConnAttempts", 5)))
                PartTableOverridePreference = (CInt(PEPolicyKey.GetValue("PartTableOverridePreference", 0)))
                UEFICA23Preference = (CInt(PEPolicyKey.GetValue("UEFICA23Preference", 0)))
                AutoUnattendCopytoSysprep = (CInt(PEPolicyKey.GetValue("AutoUnattendCopytoSysprep", 0)) = 1)
                PXEServerPort = PEPolicyKey.GetValue("PXEServerPort", 8080)
                KeyboardLayoutCode = PEPolicyKey.GetValue("KeyboardLayoutCode", "00000409")
                KeyboardLayoutOverrideExistingLayout = CInt(PEPolicyKey.GetValue("KeyboardLayoutOverrideExistingLayout", 0)) = 1
                AnswerFileConflictResponse = CInt(PEPolicyKey.GetValue("AnswerFileConflictResponse", 0))
                PEPolicyKey.Close()
                Key.Close()
                ' Apply program colors immediately
                ChangePrgColors(ColorMode)
                ' Apply language settings immediately
                ApplyLanguage(LanguageCode)
            Catch ex As Exception
                DynaLog.LogMessage("Could not grab settings from registry: " & ex.Message & ". Loading from INI File...")
                LoadDTSettings(1, True)
                Exit Sub
            End Try
        ElseIf LoadMode = 1 Then
            DynaLog.LogMessage("Load Mode is 1 -- Getting from INI File...")
            If File.Exists(Path.Combine(Application.StartupPath, "settings.ini")) Then
                DynaLog.LogMessage("Preparing to grab values...")
                Try
                    Dim parser As New IniDataParser(New SettingsParserConfiguration())
                    Dim settingData As IniData = parser.Parse(File.ReadAllText(Path.Combine(Application.StartupPath, "settings.ini"), UTF8))
                    DismExe = settingData("Program")("DismExe").Replace(Quote, "").Replace("{common:WinDir}", Environment.GetFolderPath(Environment.SpecialFolder.Windows)).Trim()
                    SaveOnSettingsIni = CInt(settingData("Program")("SaveOnSettingsIni")) = 1
                    If Not SaveOnSettingsIni AndAlso Not File.Exists(Path.Combine(Application.StartupPath, "portable")) Then
                        DynaLog.LogMessage("We are not forcing load with INI. Proceeding to load from registry...")
                        LoadDTSettings(0)
                        Exit Sub
                    End If
                    ColorMode = CInt(settingData("Personalization")("ColorMode"))
                    If ColorMode < 0 Then ColorMode = 0
                    If ColorMode > 2 Then ColorMode = 2
                    Dim rawLanguageSetting As String = ""
                    Try
                        rawLanguageSetting = settingData("Personalization")("LanguageCode")
                    Catch
                    End Try
                    LanguageCode = LocalizationService.ResolveStartupCultureCode(rawLanguageSetting)
                    ApplyLanguage(LanguageCode)
                    LightThemeIndex = CInt(settingData("Personalization")("ColorTheme_Light"))
                    DarkThemeIndex = CInt(settingData("Personalization")("ColorTheme_Dark"))
                    ChangePrgColors(ColorMode)
                    LogFont = settingData("Personalization")("LogFont").Replace(Quote, "").Trim()
                    LogFontSize = CInt(settingData("Personalization")("LogFontSi"))
                    LogFontIsBold = CInt(settingData("Personalization")("LogFontBold")) = 1
                    ProgressPanelStyle = CInt(settingData("Personalization")("SecondaryProgressPanelStyle"))
                    If ProgressPanelStyle < 0 Then ProgressPanelStyle = 0
                    If ProgressPanelStyle > 1 Then ProgressPanelStyle = 1
                    AllCaps = CInt(settingData("Personalization")("AllCaps")) = 1
                    If AllCaps Then
                        FileToolStripMenuItem.Text = FileToolStripMenuItem.Text.ToUpper()
                        ProjectToolStripMenuItem.Text = ProjectToolStripMenuItem.Text.ToUpper()
                        CommandsToolStripMenuItem.Text = CommandsToolStripMenuItem.Text.ToUpper()
                        ToolsToolStripMenuItem.Text = ToolsToolStripMenuItem.Text.ToUpper()
                        HelpToolStripMenuItem.Text = HelpToolStripMenuItem.Text.ToUpper()
                    End If
                    ExpandedProgressPanel = CInt(settingData("Personalization")("ExpandedProgressPanel")) = 1
                    ShowDateAndTime = CInt(settingData("Personalization")("ShowDateAndTime")) = 1
                    LogFile = settingData("Logs")("LogFile").Replace(Quote, "").Replace("{common:WinDir}", Environment.GetFolderPath(Environment.SpecialFolder.Windows)).Trim()
                    LogLevel = CInt(settingData("Logs")("LogLevel"))
                    If LogLevel < 1 Then LogLevel = 1
                    If LogLevel > 4 Then LogLevel = 4
                    AutoLogs = CInt(settingData("Logs")("AutoLogs")) = 1
                    SystemEditor = settingData("Logs")("SystemEditor").Replace(Quote, "").Replace("{common:WinDir}", Environment.GetFolderPath(Environment.SpecialFolder.Windows)).Trim()
                    EnableDynaLog = CInt(settingData("Logs")("EnableDynaLog")) = 1
                    QuietOperations = CInt(settingData("ImgOps")("Quiet")) = 1
                    SysNoRestart = CInt(settingData("ImgOps")("NoRestart")) = 1
                    NoNTSamMappings = CInt(settingData("ImgOps")("NoNTSamMappings")) = 1
                    PEHelper_UnattendedFile = settingData("ImgOps")("PEHelper.UnattendedFile").Replace(Quote, "").Trim()
                    PEHelper_CopyToVentoy = CInt(settingData("ImgOps")("PEHelper.CopyToVentoy")) = 1
                    PEHelper_Use2023EFI = CInt(settingData("ImgOps")("PEHelper.Use2023EFI")) = 1
                    PEHelper_IncludeSysDrvs = CInt(settingData("ImgOps")("PEHelper.IncludeSysDrvs")) = 1
                    AppxDisplayNameFormatOnRemoval = CInt(settingData("ImgOps")("AppxRemovalDisplayNameFormat"))
                    PreventSystemFromSleeping = CInt(settingData("ImgOps")("PreventSystemFromSleeping")) = 1
                    HumanizeDates = CInt(settingData("ImgOps")("HumanizeDates")) = 1
                    If AppxDisplayNameFormatOnRemoval < 0 Then AppxDisplayNameFormatOnRemoval = 0
                    If AppxDisplayNameFormatOnRemoval > 2 Then AppxDisplayNameFormatOnRemoval = 2
                    UseScratch = CInt(settingData("ScratchDir")("UseScratch")) = 1
                    AutoScrDir = CInt(settingData("ScratchDir")("AutoScratch")) = 1
                    ScratchDir = settingData("ScratchDir")("ScratchDirLocation").Replace(Quote, "")
                    EnglishOutput = CInt(settingData("Output")("EnglishOutput")) = 1
                    ReportView = CInt(settingData("Output")("ReportView"))
                    If ReportView < 0 Then ReportView = 0
                    If ReportView > 1 Then ReportView = 1
                    NotificationShow = CInt(settingData("BgProcesses")("ShowNotification")) = 1
                    NotificationFrequency = CInt(settingData("BgProcesses")("NotifyFrequency"))
                    If NotificationFrequency < 0 Then NotificationFrequency = 0
                    If NotificationFrequency > 1 Then NotificationFrequency = 1
                    ExtAppxGetter = CInt(settingData("AdvBgProcesses")("EnhancedAppxGetter")) = 1
                    SkipNonRemovable = CInt(settingData("AdvBgProcesses")("SkipNonRemovable")) = 1
                    AllDrivers = CInt(settingData("AdvBgProcesses")("DetectAllDrivers")) = 1
                    SkipFrameworks = CInt(settingData("AdvBgProcesses")("SkipFrameworks")) = 1
                    RunAllProcs = CInt(settingData("AdvBgProcesses")("RunAllProcs")) = 1
                    StartupRemount = CInt(settingData("Startup")("RemountImages")) = 1
                    StartupUpdateCheck = CInt(settingData("Startup")("CheckForUpdates")) = 1
                    AutoCleanMounts = CInt(settingData("Shutdown")("AutoCleanMounts")) = 1
                    Width = WindowHelper.ScaleLogical(CInt(settingData("WndParams")("WndWidth")))
                    Height = WindowHelper.ScaleLogical(CInt(settingData("WndParams")("WndHeight")))
                    StartPosition = If(CInt(settingData("WndParams")("WndCenter")) = 1, FormStartPosition.CenterScreen, FormStartPosition.Manual)
                    If StartPosition = FormStartPosition.CenterScreen Then
                        Location = New Point((Screen.FromControl(Me).WorkingArea.Width - Width) / 2, (Screen.FromControl(Me).WorkingArea.Height - Height) / 2)
                    Else
                        Left = CInt(settingData("WndParams")("WndLeft"))
                        Top = CInt(settingData("WndParams")("WndTop"))
                    End If
                    WindowState = If(CInt(settingData("WndParams")("WndMaximized")) = 1, FormWindowState.Maximized, FormWindowState.Normal)
                    SkipQuestions = CInt(settingData("InfoSaver")("SkipQuestions")) = 1
                    AutoCompleteInfo(0) = CInt(settingData("InfoSaver")("Pkg_CompleteInfo")) = 1
                    AutoCompleteInfo(1) = CInt(settingData("InfoSaver")("Feat_CompleteInfo")) = 1
                    AutoCompleteInfo(2) = CInt(settingData("InfoSaver")("AppX_CompleteInfo")) = 1
                    AutoCompleteInfo(3) = CInt(settingData("InfoSaver")("Cap_CompleteInfo")) = 1
                    AutoCompleteInfo(4) = CInt(settingData("InfoSaver")("Drv_CompleteInfo")) = 1
                    SearchEngineName = settingData("SearchSettings")("EngineName").Replace(Quote, "")
                    SearchEngineAITolerance = CInt(settingData("SearchSettings")("AITolerance"))
                    If SearchEngineAITolerance < 0 Then SearchEngineAITolerance = 0
                    If SearchEngineAITolerance > 2 Then SearchEngineAITolerance = 2
                    ShowWatermark = CInt(settingData("PEPolicy")("ShowWatermark")) = 1
                    WDSHCGraphoView = CInt(settingData("PEPolicy")("WDSHCGraphoView")) = 1
                    DTDimShowPnputilOut = CInt(settingData("PEPolicy")("DTDimShowPnputilOut")) = 1
                    WDSHCConnAttempts = CInt(settingData("PEPolicy")("WDSHCConnAttempts"))
                    PartTableOverridePreference = CInt(settingData("PEPolicy")("PartTableOverridePreference"))
                    UEFICA23Preference = CInt(settingData("PEPolicy")("UEFICA23Preference"))
                    AutoUnattendCopytoSysprep = CInt(settingData("PEPolicy")("AutoUnattendCopyToSysprep")) = 1
                    PXEServerPort = CInt(settingData("PEPolicy")("PXEServerPort"))
                    KeyboardLayoutCode = settingData("PEPolicy")("KeyboardLayoutCode").Replace(Quote, "")
                    KeyboardLayoutOverrideExistingLayout = CInt(settingData("PEPolicy")("KeyboardLayoutOverrideExistingLayout")) = 1
                    AnswerFileConflictResponse = CInt(settingData("PEPolicy")("AnswerFileConflictResponse"))
                Catch ex As Exception
                    DynaLog.LogMessage("Settings could not be loaded. Error message: " & ex.Message)
                End Try
                ProjectView.Visible = True
            Else
                DynaLog.LogMessage("Settings file not found. Launching Initial Setup Wizard (ISW) and reloading settings...")
                GenerateDTSettings()
                LoadDTSettings(1)
                Exit Sub
            End If
        End If
        StatusStrip.BackColor = CurrentTheme.AccentColors(1)
        ShowDTSettings()
        If Not EnableDynaLog Then
            DynaLog.LogMessage("DynaLog Logger will be ultimately disabled")
            DynaLog.DisableLogging()
        End If
        ' Test setting validity
        If Not File.Exists(DismExe) Then
            DynaLog.LogMessage("Specified DISM Executable not found. Falling back to default program and reporting invalid setting...")
            ProblematicStrings(0) = DismExe
            isExeProblematic = True
            DismExe = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
        End If
        Dim TestingFontName As String = LogFont
        Dim siSize As Single = 12
        Using fontTester As New Font(TestingFontName, siSize, FontStyle.Regular, GraphicsUnit.Pixel)
            If Not fontTester.Name = TestingFontName Then
                DynaLog.LogMessage("Specified font not found. Falling back to default font and reporting invalid setting...")
                ProblematicStrings(1) = LogFont
                isLogFontProblematic = True
                LogFont = "Consolas"
            End If
        End Using
        If Not File.Exists(LogFile) Then
            Try
                File.Create(LogFile)
            Catch ex As Exception
                DynaLog.LogMessage("Specified log file not found. Falling back to default log file and reporting invalid setting...")
                ProblematicStrings(2) = LogFile
                LogFile = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\Logs\DISM\DISM.log"
            End Try
        End If
        If UseScratch Then
            If Not Directory.Exists(ScratchDir) Then
                Try
                    Directory.CreateDirectory(ScratchDir)
                Catch ex As Exception
                    DynaLog.LogMessage("Specified scratch directory file not found. Falling back to default settings and reporting invalid setting...")
                    ProblematicStrings(3) = ScratchDir
                    isScratchDirProblematic = True
                    ScratchDir = ""
                    UseScratch = False
                End Try
            End If
        End If
        If AppxDisplayNameFormatOnRemoval < 0 OrElse AppxDisplayNameFormatOnRemoval > 2 Then
            AppxDisplayNameFormatOnRemoval = 1
        End If
        If isExeProblematic Or isLogFontProblematic Or isLogFileProblematic Or isScratchDirProblematic Then
            InvalidSettingsTSMI.Visible = True
        End If
        If PartTableOverridePreference < 0 OrElse PartTableOverridePreference > 2 Then PartTableOverridePreference = 0
        If UEFICA23Preference < 0 OrElse UEFICA23Preference > 2 Then UEFICA23Preference = 0
        If WDSHCConnAttempts < 2 OrElse WDSHCConnAttempts > 16 Then WDSHCConnAttempts = 5
        If PXEServerPort < 80 OrElse PXEServerPort > 65535 Then PXEServerPort = 8080
        If AnswerFileConflictResponse < 0 OrElse AnswerFileConflictResponse > 2 Then AnswerFileConflictResponse = 0
        Try
            Dim KeyboardLayoutRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\Keyboard Layouts", False)
            Dim KeyboardLayoutCodes As String() = KeyboardLayoutRk.GetSubKeyNames()
            KeyboardLayoutRk.Close()
            If Not KeyboardLayoutCodes.Contains(KeyboardLayoutCode) Then KeyboardLayoutCode = "00000409"
        Catch ex As Exception

        End Try
        WriteDefaultPEPolicy()
    End Sub

    Public Sub WriteDefaultPEPolicy()
        Dim PartTableOverridePreferenceStr As String = "NoOverride"
        Select Case PartTableOverridePreference
            Case 0
                PartTableOverridePreferenceStr = "NoOverride"
            Case 1
                PartTableOverridePreferenceStr = "AlwaysMBR"
            Case 2
                PartTableOverridePreferenceStr = "AlwaysGPT"
        End Select
        Dim UEFICA23PreferenceStr As String = "AskUser"
        Select Case UEFICA23Preference
            Case 0
                UEFICA23PreferenceStr = "AskUser"
            Case 1
                UEFICA23PreferenceStr = "UseNever"
            Case 2
                UEFICA23PreferenceStr = "UseAlways"
        End Select
        Dim AnswerFileConflictResponseStr As String = "AskUser"
        Select Case AnswerFileConflictResponse
            Case 0
                AnswerFileConflictResponseStr = "AskUser"
            Case 1
                AnswerFileConflictResponseStr = "PreferISO"
            Case 2
                AnswerFileConflictResponseStr = "PreferWIM"
        End Select

        Dim regContents As String = String.Format("Windows Registry Editor Version 5.00{0}{0}" &
                                                  "[HKEY_LOCAL_MACHINE\WINPESOFT\DISMTools\Preinstallation Environment\Policies]{0}" &
                                                  "{1}ShowWatermark{1}=dword:0000000{2}{0}" &
                                                  "{1}UEFICA23Preference{1}={1}{3}{1}{0}" &
                                                  "{1}PartTableOverridePreference{1}={1}{4}{1}{0}" &
                                                  "{1}WDSHCConnAttempts{1}=dword:{5}{0}" &
                                                  "{1}WDSHCGraphoView{1}=dword:0000000{6}{0}" &
                                                  "{1}DTDimShowPnputilOut{1}=dword:0000000{7}{0}" &
                                                  "{1}AutoUnattendCopytoSysprep{1}=dword:0000000{8}{0}" &
                                                  "{1}PXEServerPort{1}=dword:{9}{0}" &
                                                  "{1}KeyboardLayoutCode{1}={1}{10}{1}{0}" &
                                                  "{1}KeyboardLayoutOverrideExistingLayout{1}=dword:0000000{11}{0}" &
                                                  "{1}AnswerFileConflictResponse{1}={1}{12}{1}{0}",
                                                  CrLf, Quote, If(ShowWatermark, 1, 0), UEFICA23PreferenceStr, PartTableOverridePreferenceStr,
                                                  Hex(WDSHCConnAttempts).PadLeft(8, "0"c).ToLowerInvariant(), If(WDSHCGraphoView, 1, 0), If(DTDimShowPnputilOut, 1, 0),
                                                  If(AutoUnattendCopytoSysprep, 1, 0), Hex(PXEServerPort).PadLeft(8, "0"c).ToLowerInvariant(), KeyboardLayoutCode, If(KeyboardLayoutOverrideExistingLayout, 1, 0), AnswerFileConflictResponseStr)
        Try
            File.WriteAllText(Path.Combine(Application.StartupPath, "bin", "extps1", "PE_Helper", "files", "DefaultPolicy.reg"), regContents)
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    ''' <summary>
    ''' Shows the DISMTools Settings with DynaLog
    ''' </summary>
    ''' <remarks></remarks>
    Sub ShowDTSettings()
        DynaLog.LogMessage("Program Settings:" & CrLf &
                           "DISMExe                             =    " & Quote & DismExe & Quote & CrLf &
                           "SaveOnSettingsIni                   =    " & SaveOnSettingsIni & CrLf &
                           "ColorMode                           =    " & ColorMode & CrLf &
                           "ColorTheme_Light                    =    " & LightThemeIndex & CrLf &
                           "ColorTheme_Dark                     =    " & DarkThemeIndex & CrLf &
                           "LanguageCode                        =    " & Quote & LanguageCode & Quote & CrLf &
                           "LogFont                             =    " & Quote & LogFont & Quote & CrLf &
                           "LogFontSi                           =    " & LogFontSize & CrLf &
                           "LogFontBold                         =    " & LogFontIsBold & CrLf &
                           "SecondaryProgressPanelStyle         =    " & ProgressPanelStyle & CrLf &
                           "AllCaps                             =    " & AllCaps & CrLf &
                           "ExpandedProgressPanel               =    " & ExpandedProgressPanel & CrLf &
                           "ShowDateAndTime                     =    " & ShowDateAndTime & CrLf &
                           "LogFile                             =    " & Quote & LogFile & Quote & CrLf &
                           "LogLevel                            =    " & LogLevel & CrLf &
                           "AutoLogs                            =    " & AutoLogs & CrLf &
                           "SystemEditor                        =    " & Quote & SystemEditor & Quote & CrLf &
                           "EnableDynaLog                       =    " & EnableDynaLog & CrLf &
                           "Quiet                               =    " & QuietOperations & CrLf &
                           "NoNTSamMappings                     =    " & NoNTSamMappings & CrLf &
                           "WebSearchEngineName                 =    " & SearchEngineName & CrLf &
                           "WebSearchAITolerance                =    " & SearchEngineAITolerance & CrLf &
                           "PEHelper_UnattendedFile             =    " & Quote & PEHelper_UnattendedFile & Quote & CrLf &
                           "PEHelper_CopyToVentoy               =    " & PEHelper_CopyToVentoy & CrLf &
                           "PEHelper_Use2023EFI                 =    " & PEHelper_Use2023EFI & CrLf &
                           "PEHelper_IncludeSysDrvs             =    " & PEHelper_IncludeSysDrvs & CrLf &
                           "NoRestart                           =    " & SysNoRestart & CrLf &
                           "AppxRemovalDisplayNameFrmt          =    " & AppxDisplayNameFormatOnRemoval & CrLf &
                           "PreventSystemFromSleeping           =    " & PreventSystemFromSleeping & CrLf &
                           "HumanizeDates                       =    " & HumanizeDates & CrLf &
                           "UseScratch                          =    " & UseScratch & CrLf &
                           "AutoScratch                         =    " & AutoScrDir & CrLf &
                           "ScratchDirLocation                  =    " & Quote & ScratchDir & Quote & CrLf &
                           "EnglishOutput                       =    " & EnglishOutput & CrLf &
                           "ReportView                          =    " & ReportView & CrLf &
                           "ShowNotification                    =    " & NotificationShow & CrLf &
                           "NotifyFrequency                     =    " & NotificationFrequency & CrLf &
                           "EnhancedAppxGetter                  =    " & ExtAppxGetter & CrLf &
                           "SkipNonRemovable                    =    " & SkipNonRemovable & CrLf &
                           "DetectAllDrivers                    =    " & AllDrivers & CrLf &
                           "SkipFrameworks                      =    " & SkipFrameworks & CrLf &
                           "RunAllProcs                         =    " & RunAllProcs & CrLf &
                           "RemountImages                       =    " & StartupRemount & CrLf &
                           "CheckForUpdates                     =    " & StartupUpdateCheck & CrLf &
                           "AutoCleanMounts                     =    " & AutoCleanMounts & CrLf &
                           "WndWidth                            =    " & WndWidth & CrLf &
                           "WndHeight                           =    " & WndHeight & CrLf &
                           "WndCenter                           =    " & (StartPosition = FormStartPosition.CenterScreen) & CrLf &
                           "WndLeft                             =    " & WndLeft & CrLf &
                           "WndTop                              =    " & WndTop & CrLf &
                           "WndMaximized                        =    " & (WindowState = FormWindowState.Maximized) & CrLf &
                           "SkipQuestions                       =    " & SkipQuestions & CrLf &
                           "Pkg_CompleteInfo                    =    " & AutoCompleteInfo(0) & CrLf &
                           "Feat_CompleteInfo                   =    " & AutoCompleteInfo(1) & CrLf &
                           "AppX_CompleteInfo                   =    " & AutoCompleteInfo(2) & CrLf &
                           "Cap_CompleteInfo                    =    " & AutoCompleteInfo(3) & CrLf &
                           "Drv_CompleteInfo                    =    " & AutoCompleteInfo(4) & CrLf &
                           "ShowWatermark                       =    " & ShowWatermark & CrLf &
                           "WDSHCGraphoView                     =    " & WDSHCGraphoView & CrLf &
                           "DTDimShowPnputilOut                 =    " & DTDimShowPnputilOut & CrLf &
                           "WDSHCConnAttempts                   =    " & WDSHCConnAttempts & CrLf &
                           "PartTableOverridePreference         =    " & PartTableOverridePreference & CrLf &
                           "UEFICA23Preference                  =    " & UEFICA23Preference & CrLf &
                           "AutoUnattendCopytoSysprep           =    " & AutoUnattendCopytoSysprep & CrLf &
                           "PXEServerPort                       =    " & PXEServerPort & CrLf &
                           "KeyboardLayoutCode                  =    " & KeyboardLayoutCode & CrLf &
                           "KeyboardLayoutOverrideExistingLayout=    " & KeyboardLayoutOverrideExistingLayout & CrLf &
                           "AnswerFileConflictResponse          =    " & AnswerFileConflictResponse)
    End Sub

#Region "Background Processes"

    ''' <summary>
    ''' Runs specified background processes. The program refers to the processes that gather image information as "background processes", due to the way they are run (in the background ;))
    ''' </summary>
    ''' <param name="bgProcOptn">Which processes are run to get image information</param>
    ''' <param name="GatherBasicInfo">When true, this procedure gets the basic image information (image file, index, mountpoint and status)</param>
    ''' <param name="GatherAdvancedInfo">When true, this procedure gets all image information unrelated to packages, features, capabilities, or drivers</param>
    ''' <param name="OnlineMode">(Optional) Detects properties of an active Windows installation if this value is True. Otherwise, if it is False or is not set, it won't pass this option</param>
    ''' <remarks>Depending on the parameter of bgProcOptn, and on the power of the system, the background processes may take a longer time to finish</remarks>
    Sub RunBackgroundProcesses(bgProcOptn As Integer, GatherBasicInfo As Boolean, GatherAdvancedInfo As Boolean, Optional OnlineMode As Boolean = False, Optional OfflineMode As Boolean = False)
        IsCompatible = True
        DynaLog.LogMessage("Preparing to run background processes...")
        BWFailPanel.Visible = False
        FailedBGProcResultDic.Clear()
        If Not IsImageMounted Then
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
            DynaLog.LogMessage("Background processes have stopped because no images have been mounted. Exiting...")
            Exit Sub
        End If
        DynaLog.LogMessage("Clearing completion state...")
        Array.Clear(CompletedTasks, 0, CompletedTasks.Length)
        ' Let user know things are working
        BackgroundProcessesButton.Visible = False
        BackgroundProcessesButton.Image = GetGlyphResource("bg_ops")
        BackgroundProcessesButton.Visible = True
        DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
        areBackgroundProcessesDone = False
        regJumps = False
        irregVal = 0
        pbOpNums = 0
        Dim session As DismSession = Nothing
        If Not OnlineMode And Not OfflineMode Then
            ' Fix up paths, which, in some cases, may begin with <letter>:\\. The filters then fail and return nothing
            SourceImg = SourceImg.Replace("\\", "\")
            MountDir = MountDir.Replace("\\", "\")

            DynaLog.LogMessage("Creating image session...")
            Try
                Dim imageToProcess As WindowsImage = MountedImageList.FirstOrDefault(Function(image) image.ImageMountDirectory = MountDir)
                If imageToProcess IsNot Nothing Then
                    sessionMntDir = imageToProcess.ImageMountDirectory
                End If
                DynaLog.LogMessage("We have the necessary bits for the image session. Mount directory: " & Quote & sessionMntDir & Quote)
            Catch ex As Exception

            End Try
        End If
        If OfflineMode Then sessionMntDir = MountDir
        ' Determine which actions are being done
        DynaLog.LogMessage("Calculating steps to perform...")
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
        DynaLog.LogMessage("Amount of steps: " & pbOpNums)
        If pbOpNums > 1 Then progressDivs = 100 / pbOpNums Else progressDivs = 0
        progressLabel = LocalizationService.ForSection("Main.Run.BgProcesses")("RunningProcesses.Label")
        ImgBW.ReportProgress(0)
        If GatherBasicInfo Then
            DynaLog.LogMessage("Beginning background process work by getting standard image info...")
            progressLabel = LocalizationService.ForSection("Main.Run.BgProcesses")("Getting.Basic.Image.Label")
            ImgBW.ReportProgress(progressMin + progressDivs)
            GetBasicImageInfo(OnlineMode, OfflineMode)
            If isOrphaned Then
                'If session IsNot Nothing Then DismApi.CloseSession(session)
                DynaLog.LogMessage("The image we tried to get info about is orphaned. Exiting...")
                Exit Sub
            End If
            If ImgBW.CancellationPending Then
                'If session IsNot Nothing Then DismApi.CloseSession(session)
                DynaLog.LogMessage("The user is cancelling these processes. Exiting...")
                Exit Sub
            End If
            DynaLog.LogMessage("Detecting whether or not the Windows image is compatible with DISMTools/DISM...")
            DetectNTVersion(If(OnlineMode, Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\ntoskrnl.exe", MountDir & "\Windows\system32\ntoskrnl.exe"))
            DynaLog.LogMessage("Determining whether or not to continue...")
            ' If DetectNTVersion flags this image as incompatible, don't go any further
            If Not IsCompatible Then Exit Sub
            If GatherAdvancedInfo Then
                DynaLog.LogMessage("Getting the remaining bits of information...")
                progressLabel = LocalizationService.ForSection("Main.Run.BgProcesses")("AdvancedImageInfo.Label")
                ImgBW.ReportProgress(progressMin + progressDivs)
                GetAdvancedImageInfo(OnlineMode, OfflineMode)
            End If
        End If
        DynaLog.LogMessage("Creating temporary directory for information...")
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
                DynaLog.LogMessage("Package information processes are being run")
                CompletedTasks(0) = False
                CompletedTasks(1) = True
                CompletedTasks(2) = True
                CompletedTasks(3) = True
                CompletedTasks(4) = True
                ' Set pending task
                PendingTasks(0) = True
            Case 2
                DynaLog.LogMessage("Feature information processes are being run")
                CompletedTasks(0) = True
                CompletedTasks(1) = False
                CompletedTasks(2) = True
                CompletedTasks(3) = True
                CompletedTasks(4) = True
                ' Set pending task
                PendingTasks(1) = True
            Case 3
                DynaLog.LogMessage("AppX package information processes are being run")
                CompletedTasks(0) = True
                CompletedTasks(1) = True
                CompletedTasks(2) = False
                CompletedTasks(3) = True
                CompletedTasks(4) = True
                ' Set pending task
                PendingTasks(2) = True
            Case 4
                DynaLog.LogMessage("Capability information processes are being run")
                CompletedTasks(0) = True
                CompletedTasks(1) = True
                CompletedTasks(2) = True
                CompletedTasks(3) = False
                CompletedTasks(4) = True
                ' Set pending task
                PendingTasks(3) = True
            Case 5
                DynaLog.LogMessage("Driver information processes are being run")
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
                progressLabel = LocalizationService.ForSection("Main.Run.BgProcesses")("Getting.Image.Packages.Label")
                ImgBW.ReportProgress(20)
                GetImagePackages(OnlineMode)
                If ImgBW.CancellationPending Then
                    DynaLog.LogMessage("The user is cancelling these processes. Exiting...")
                    If session IsNot Nothing Then DismApi.CloseSession(session)
                    Exit Sub
                End If
                progressLabel = LocalizationService.ForSection("Main.Run.BgProcesses")("Getting.Image.Features.Label")
                ImgBW.ReportProgress(progressMin + progressDivs)
                GetImageFeatures(OnlineMode)
                If ImgBW.CancellationPending Then
                    DynaLog.LogMessage("The user is cancelling these processes. Exiting...")
                    If session IsNot Nothing Then DismApi.CloseSession(session)
                    Exit Sub
                End If
                If IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
                    If Not (CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise) And Not (CurrentImage.ImageInstallationType.Contains("Nano") Or CurrentImage.ImageInstallationType.Contains("Core")) Then
                        DynaLog.LogMessage("Windows 8 or later")
                        pbOpNums += 1
                        progressLabel = LocalizationService.ForSection("Main.Run.BgProcesses")("Get.Image.Provisioned.Label")
                        ImgBW.ReportProgress(progressMin + progressDivs)
                        GetImageAppxPackages(OnlineMode)
                        If ImgBW.CancellationPending Then
                            DynaLog.LogMessage("The user is cancelling these processes. Exiting...")
                            If session IsNot Nothing Then DismApi.CloseSession(session)
                            Exit Sub
                        End If
                    Else
                        DynaLog.LogMessage("Not Windows 8 or later")
                    End If
                Else
                    DynaLog.LogMessage("Not Windows 8 or later")
                End If
                If IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") And Not (CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise) Then
                    If Not (CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise) And Not CurrentImage.ImageInstallationType.Contains("Nano") Then
                        DynaLog.LogMessage("Windows 10 or later")
                        pbOpNums += 1
                        progressLabel = LocalizationService.ForSection("Main.Run.BgProcesses")("Get.Image.Features.Label")
                        ImgBW.ReportProgress(progressMin + progressDivs)
                        GetImageCapabilities(OnlineMode)
                        If ImgBW.CancellationPending Then
                            DynaLog.LogMessage("The user is cancelling these processes. Exiting...")
                            If session IsNot Nothing Then DismApi.CloseSession(session)
                            Exit Sub
                        End If
                    Else
                        DynaLog.LogMessage("Not Windows 10 or later")
                    End If
                Else
                    DynaLog.LogMessage("Not Windows 10 or later")
                End If
                progressLabel = LocalizationService.ForSection("Main.Run.BgProcesses")("Getting.Image.Drivers.Label")
                ImgBW.ReportProgress(progressMin + progressDivs)
                GetImageDrivers(OnlineMode)
                If ImgBW.CancellationPending Then
                    DynaLog.LogMessage("The user is cancelling these processes. Exiting...")
                    If session IsNot Nothing Then DismApi.CloseSession(session)
                    Exit Sub
                End If
            Case 1
                DynaLog.LogMessage("Updating recorded OS package information...")
                progressLabel = LocalizationService.ForSection("Main.Run.BgProcesses")("Getting.Image.Packages.Label")
                ImgBW.ReportProgress(20)
                GetImagePackages(OnlineMode)
            Case 2
                DynaLog.LogMessage("Updating recorded feature information...")
                progressLabel = LocalizationService.ForSection("Main.Run.BgProcesses")("Getting.Image.Features.Label")
                ImgBW.ReportProgress(progressMin + progressDivs)
                GetImageFeatures(OnlineMode)
            Case 3
                DynaLog.LogMessage("Updating recorded AppX package information...")
                If IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") = True Then
                    DynaLog.LogMessage("Windows 8 or later")
                    pbOpNums += 1
                    progressLabel = LocalizationService.ForSection("Main.Run.BgProcesses")("Get.Image.Provisioned.Label")
                    ImgBW.ReportProgress(progressMin + progressDivs)
                    GetImageAppxPackages(OnlineMode)
                Else
                    DynaLog.LogMessage("Not Windows 8 or later")
                    PendingTasks(2) = False
                End If
            Case 4
                DynaLog.LogMessage("Updating recorded capability information...")
                If IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") = True Then
                    DynaLog.LogMessage("Windows 10 or later")
                    pbOpNums += 1
                    progressLabel = LocalizationService.ForSection("Main.Run.BgProcesses")("Get.Image.Features.Label")
                    ImgBW.ReportProgress(progressMin + progressDivs)
                    GetImageCapabilities(OnlineMode)
                Else
                    DynaLog.LogMessage("Not Windows 10 or later")
                    PendingTasks(3) = False
                End If
            Case 5
                DynaLog.LogMessage("Updating recorded driver information...")
                progressLabel = LocalizationService.ForSection("Main.Run.BgProcesses")("Getting.Image.Drivers.Label")
                ImgBW.ReportProgress(progressMin + progressDivs)
                GetImageDrivers(OnlineMode)
        End Select
        If bgProcOptn <> 0 And PendingTasks.Contains(True) Then
            DynaLog.LogMessage("Some tasks need to be finished before we're happy. Finishing them...")
            progressLabel = LocalizationService.ForSection("Main.Run.BgProcesses")("Running.Pending.Tasks.Label")
            ImgBW.ReportProgress(99)
            DynaLog.LogMessage("Determining whether or not OS package information processes remain. Do them if they do remain...")
            If PendingTasks(0) Then GetImagePackages(OnlineMode)
            DynaLog.LogMessage("Determining whether or not feature information processes remain. Do them if they do remain...")
            If PendingTasks(1) Then GetImageFeatures(OnlineMode)
            DynaLog.LogMessage("Determining whether or not AppX package information processes remain. Do them if they do remain...")
            If PendingTasks(2) Then
                If IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
                    If Not (CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise) And Not (CurrentImage.ImageInstallationType.Contains("Nano") Or CurrentImage.ImageInstallationType.Contains("Core")) Then
                        GetImageAppxPackages(OnlineMode)
                    End If
                End If
            End If
            DynaLog.LogMessage("Determining whether or not capability information processes remain. Do them if they do remain...")
            If PendingTasks(3) Then
                If IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") And Not (CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise) Then
                    If Not (CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise) And Not CurrentImage.ImageInstallationType.Contains("Nano") Then
                        GetImageCapabilities(OnlineMode)
                    End If
                End If
            End If
            DynaLog.LogMessage("Determining whether or not driver information processes remain. Do them if they do remain...")
            If PendingTasks(4) Then GetImageDrivers(OnlineMode)
            DynaLog.LogMessage("Deleting temporary files...")
            DeleteTempFiles()
            If session IsNot Nothing Then
                DynaLog.LogMessage("Closing sessions...")
                DismApi.CloseSession(session)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Gets basic image information, such as its index, its file path, or its mount dir
    ''' </summary>
    ''' <remarks>Depending on the GatherBasicInfo flag in RunBackgroundProcesses, this function will run or not</remarks>
    Sub GetBasicImageInfo(Optional OnlineMode As Boolean = False, Optional OfflineMode As Boolean = False)
        ' Set image properties
        Label41.Text = ProgressPanel.ImgIndex
        Label44.Text = ProgressPanel.MountDir
        ' Loading the project directly with an image already mounted makes the two labels above be wrong.
        ' Check them and use local vars
        If Label41.Text = "0" Or Label41.Text = "" Then     ' Label41 (index preview label) returns 0 and Label44 (mount dir preview) returns blank
            Label41.Text = ImgIndex
            Label44.Text = MountDir
        End If
        If OnlineMode Then
            DynaLog.LogMessage("Getting information about the active installation...")
            ' Revision number may not be the one that we're actually on when getting info about ntoskrnl; use UBR if we can
            Dim revisionNumber As Integer
            Try
                Dim ubrRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion", False)
                revisionNumber = ubrRk.GetValue("UBR")
                ubrRk.Close()
            Catch ex As Exception
                revisionNumber = FileVersionInfo.GetVersionInfo(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\ntoskrnl.exe").ProductPrivatePart
            End Try

            Label48.Text = Environment.OSVersion.Version.Major & "." & Environment.OSVersion.Version.Minor & "." & Environment.OSVersion.Version.Build & "." & revisionNumber
            CurrentImage.ImageVersion = Environment.OSVersion.Version
                    Label41.Text = LocalizationService.ForSection("Main.Get.Basic")("Online.Install.Label")
                    Label47.Text = LocalizationService.ForSection("Main.Get.Basic")("Online.Install.Label")
                    Label49.Text = LocalizationService.ForSection("Main.Get.Basic")("Online.Install.Label")
            Label46.Text = My.Computer.Info.OSFullName
            Label44.Text = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows))
            Label52.Text = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows))
            Label49.Text = Label49.Text
            ' Disable tasks in the new design accordingly
            Button24.Enabled = False
            Button25.Enabled = False
            Button26.Enabled = False
            Button27.Enabled = False
            Button28.Enabled = False
            Button29.Enabled = False
            DynaLog.LogMessage("Online installation information:")
            DynaLog.LogMessage("- Mount point: " & Label52.Text)
            DynaLog.LogMessage("- Image name: " & Label46.Text)
            DynaLog.LogMessage("- Image version: " & Label48.Text)
        ElseIf OfflineMode Then
            DynaLog.LogMessage("Getting information about the offline installation...")
            Label48.Text = FileVersionInfo.GetVersionInfo(MountDir & "\Windows\system32\ntoskrnl.exe").ProductVersion
            CurrentImage.ImageVersion = New Version(FileVersionInfo.GetVersionInfo(MountDir & "\Windows\system32\ntoskrnl.exe").ProductVersion)
                    Label41.Text = LocalizationService.ForSection("Main.Get.Basic")("Offline.Install.Item")
                    Label46.Text = LocalizationService.ForSection("Main.Get.Basic")("Offline.Install.Label")
                    Label47.Text = LocalizationService.ForSection("Main.Get.Basic")("Offline.Install.Item")
                    Label49.Text = LocalizationService.ForSection("Main.Get.Basic")("Offline.Install.Item")
            Label41.Text = MountDir
            Label44.Text = MountDir
            Label52.Text = MountDir
            ' Disable tasks in the new design accordingly
            Button24.Enabled = False
            Button25.Enabled = False
            Button26.Enabled = False
            Button27.Enabled = False
            Button28.Enabled = False
            Button29.Enabled = False
            DynaLog.LogMessage("Getting Edition ID and installation type...")
            GetOfflineEditionAndInstIdFromRegistry()
            DynaLog.LogMessage("Offline installation information:")
            DynaLog.LogMessage("- Mount point: " & Label52.Text)
            DynaLog.LogMessage("- Image version: " & Label48.Text)
        Else
            Try
                If ReinitializeCurImage Then
                    CurrentImage = MountedImageList.FirstOrDefault(Function(image) image.ImageFile = SourceImg)
                End If
                ReinitializeCurImage = True
                If CurrentImage IsNot Nothing Then
                    Label41.Text = CurrentImage.ImageIndex
                    Label44.Text = CurrentImage.ImageMountDirectory
                    isOrphaned = (CurrentImage.ImageMountStatus = DismMountStatus.NeedsRemount)
                    Dim ImageInformation As DismImageInfo = DismApi.GetImageInfo(CurrentImage.ImageFile).ElementAtOrDefault(CurrentImage.ImageIndex - 1)
                    If ImageInformation IsNot Nothing Then
                        CurrentImage.ImageMountGuid = CurrentImage.GetImageMountGuid()
                        CurrentImage.ImageVersion = ImageInformation.ProductVersion
                        CurrentImage.ImageName = ImageInformation.ImageName
                        CurrentImage.ImageDescription = ImageInformation.ImageDescription

                        Label48.Text = ImageInformation.ProductVersion.ToString()
                        Label46.Text = ImageInformation.ImageName
                        Label47.Text = ImageInformation.ImageDescription
                    End If
                    RemountImageWithWritePermissionsToolStripMenuItem.Enabled = (CurrentImage.ImageMountMode = DismMountMode.ReadOnly)
                    Button27.Enabled = (CurrentImage.ImageMountMode = DismMountMode.ReadWrite)
                    Button28.Enabled = (CurrentImage.ImageMountMode = DismMountMode.ReadWrite)
                    Button29.Enabled = True
                    DynaLog.LogMessage("Basic image information:")
                    DynaLog.LogMessage("- Image Index: " & CurrentImage.ImageIndex)
                    DynaLog.LogMessage("- Image Mount Point: " & CurrentImage.ImageMountDirectory)
                    DynaLog.LogMessage("- Image Version: " & CurrentImage.ImageVersion.ToString())
                    DynaLog.LogMessage("- Image Name: " & CurrentImage.ImageName)
                    DynaLog.LogMessage("- Image Description: " & CurrentImage.ImageDescription)
                    DynaLog.LogMessage("- Does image have read/write permissions? " & If(CurrentImage.ImageMountMode = DismMountMode.ReadWrite, "Yes", "No"))
                End If
            Catch ex As Exception

            End Try
        End If
        Exit Sub
    End Sub

    Sub GetOfflineEditionAndInstIdFromRegistry()
        DynaLog.LogMessage("Loading installation registry...")
        Dim regExitCode As Integer = RegistryHelper.LoadRegistryHive(Path.Combine(MountDir, "Windows", "system32", "config", "SOFTWARE"), "HKLM\IMG_SOFT")
        If regExitCode <> 0 Then
            DynaLog.LogMessage("The edition could not be grabbed. Process exit code: " & Hex(regExitCode))
            CurrentImage.ImageEditionId = ""
        Else
            DynaLog.LogMessage("Getting values...")
            Dim edReg As RegistryKey = Registry.LocalMachine.OpenSubKey("IMG_SOFT\Microsoft\Windows NT\CurrentVersion", False)
            CurrentImage.ImageEditionId = edReg.GetValue("EditionID", "").ToString()
            CurrentImage.ImageInstallationType = edReg.GetValue("InstallationType", "").ToString()
            DynaLog.LogMessage("Edition: " & CurrentImage.ImageEditionId)
            DynaLog.LogMessage("Installation type: " & CurrentImage.ImageInstallationType)
            edReg.Close()
        End If
        DynaLog.LogMessage("Unloading installation registry...")
        RegistryHelper.UnloadRegistryHive("HKLM\IMG_SOFT")
    End Sub

    Private Function IsWinPeInDisguise(MountDirectory As String) As Boolean
        DynaLog.LogMessage("Checking if mounted Windows image is WinPE in disguise...")
        DynaLog.LogMessage("Mount directory: " & MountDirectory)
        If Not Directory.Exists(MountDirectory) Then Return False

        Dim disguised As Boolean = False

        ' Windows PE images in disguise, while they modify the EditionID, they don't seem to modify the registry keys. So, by just
        ' checking for a WinPE key, we can determine this.
        Dim softwareHivePath As String = Path.Combine(MountDirectory, "Windows", "system32", "config", "SOFTWARE")
        If Not File.Exists(softwareHivePath) Then Return False

        If Not RegistryHelper.LoadRegistryHive(softwareHivePath, "HKLM\zSOFT") = 0 Then Return False
        Try
            Dim WinPeSoftwareKey As RegistryKey = Registry.LocalMachine.OpenSubKey("zSOFT\Microsoft\Windows NT\CurrentVersion")
            disguised = WinPeSoftwareKey.GetSubKeyNames().Any(Function(key) key.Equals("WinPE", StringComparison.OrdinalIgnoreCase))
            WinPeSoftwareKey.Close()
        Catch ex As Exception
            ' ignore
        Finally
            RegistryHelper.UnloadRegistryHive("HKLM\zSOFT")
        End Try

        Return disguised
    End Function

    Private Sub GetFFUInformation(ByRef ImageFile As WindowsImage)
        If ImageFile Is Nothing Then Exit Sub
        Dim MountedFFURk As RegistryKey = Nothing

        Try
            MountedFFURk = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\DISM\Mounted FFUs", False)
            ' Since the information is stored in subkeys of the aforementioned subkey, we'll have to iterate over them to get the one that
            ' we wanted.
            For Each MountedFFUVolume In MountedFFURk.GetSubKeyNames()
                Dim MountedFFUVolumeRk As RegistryKey = MountedFFURk.OpenSubKey(MountedFFUVolume, False)
                Dim MountedFFUMountPath As String = MountedFFUVolumeRk.GetValue("Mount Path", "")

                If MountedFFUMountPath.Equals(ImageFile.ImageMountDirectory, StringComparison.OrdinalIgnoreCase) Then
                    ' Then it's this one
                    Dim IniManifestContents As Byte() = MountedFFUVolumeRk.GetValue("Manifest", {})
                    ImageFile.FFUInfo.IniManifest = ASCII.GetString(IniManifestContents)
                    ImageFile.FFUInfo.VhdPath = MountedFFUVolumeRk.GetValue("VHD Path", "")
                    ImageFile.FFUInfo.VhdId = MountedFFUVolumeRk.GetValue("VHD Id", "")
                    ImageFile.FFUInfo.VhdStorageDeviceId = MountedFFUVolumeRk.GetValue("VHD Storage Device Id", 0)
                    ImageFile.FFUInfo.MountDiskPath = MountedFFUVolumeRk.GetValue("Mount Disk Path", "")
                    ImageFile.FFUInfo.FullFlashVersionInfo = New Version(MountedFFUVolumeRk.GetValue("Full Flash Major Version", 0),
                                                                         MountedFFUVolumeRk.GetValue("Full Flash Minor Version", 0))
                    ImageFile.FFUInfo.VersionInfo = New Version(MountedFFUVolumeRk.GetValue("Major Version", 0), MountedFFUVolumeRk.GetValue("Minor Version", 0))
                    ImageFile.FFUInfo.MountVersion = MountedFFUVolumeRk.GetValue("Mount Version", 0)
                    ImageFile.FFUInfo.Compression = MountedFFUVolumeRk.GetValue("Compression", 0)
                    ImageFile.FFUInfo.OptimizedPartitionNumber = MountedFFUVolumeRk.GetValue("Optimized Partition Number", 0)

                    MountedFFUVolumeRk.Close()

                    ' Try processing the ini manifest so we can fill in the information that we couldn't
                    Try
                        Dim parser As New IniDataParser(New FfuIniParserConfiguration())
                        Dim ffuData As IniData = parser.Parse(ImageFile.FFUInfo.IniManifest)

                        ImageFile.ImageArchitecture = CInt(ffuData("FullFlash")("Architecture"))
                        ImageFile.ImageCreationDate = DateTimeOffset.FromFileTime(CLng(ffuData("FullFlash")("CreationTime"))).DateTime
                        ImageFile.ImageModificationDate = DateTimeOffset.FromFileTime(CLng(ffuData("FullFlash")("LastModificationTime"))).DateTime
                    Catch ex As Exception
                        ' Don't get that data then
                    End Try

                    ' Use the size of the entire virtual disk as the expanded size of our FFU.
                    Dim sizeMO As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT Size FROM Win32_DiskDrive WHERE DeviceID LIKE {0}{1}{0}", Quote, WMIHelper.GetEscapedValue(ImageFile.FFUInfo.MountDiskPath)))
                    If sizeMO IsNot Nothing Then ImageFile.ImageSize = WMIHelper.GetObjectValue(sizeMO(0), "Size")

                    Exit For
                End If

                MountedFFUVolumeRk.Close()
            Next
        Catch ex As Exception

        Finally
            If MountedFFURk IsNot Nothing Then MountedFFURk.Close()
        End Try
    End Sub

    ''' <summary>
    ''' Gets advanced image information, such as number of files and directories, image name, and more
    ''' </summary>
    ''' <remarks>This is called when bgGetAdvImgInfo is True</remarks>
    Sub GetAdvancedImageInfo(Optional OnlineMode As Boolean = False, Optional OfflineMode As Boolean = False)
        LinkLabel20.Enabled = True
        LinkLabel19.Enabled = True
        LinkLabel15.Enabled = True
        LinkLabel16.Enabled = True

        If CurrentImage Is Nothing Then CurrentImage = New WindowsImage()

        If OnlineMode Then
            DynaLog.LogMessage("Getting information about the active installation...")
            LinkLabel20.Enabled = False
            LinkLabel19.Enabled = False
            LinkLabel15.Enabled = False
            LinkLabel16.Enabled = False
            ' Set edition variable according to the EditionID registry value
            CurrentImage.ImageEditionId = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion").GetValue("EditionID")

            ' Set installation type variable according to the InstallationType registry value
            CurrentImage.ImageInstallationType = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion").GetValue("InstallationType")

            Button24.Enabled = False
            Button25.Enabled = False
            Button26.Enabled = False
            Button27.Enabled = False
            Button28.Enabled = False
            Button29.Enabled = False

            DynaLog.LogMessage("- Edition ID: " & CurrentImage.ImageEditionId)
            DynaLog.LogMessage("- Installation type: " & CurrentImage.ImageInstallationType)

            DynaLog.LogMessage("Comparing versions to determine the tasks you can do...")
            DetectVersions(FileVersionInfo.GetVersionInfo(DismExe), CurrentImage.ImageVersion)
            Exit Sub
        ElseIf OfflineMode Then
            DynaLog.LogMessage("Getting information about the offline installation...")
            LinkLabel20.Enabled = False
            LinkLabel19.Enabled = False
            LinkLabel15.Enabled = False
            LinkLabel16.Enabled = False
            Button24.Enabled = False
            Button25.Enabled = False
            Button26.Enabled = False
            Button27.Enabled = False
            Button28.Enabled = False
            Button29.Enabled = False

            DynaLog.LogMessage("Comparing versions to determine the tasks you can do...")
            DetectVersions(FileVersionInfo.GetVersionInfo(DismExe), CurrentImage.ImageVersion)
            Exit Sub
        Else
            If IsImageMounted Then
                DynaLog.LogMessage("The image is mounted. Continuing...")
                Try
                    Dim ImageInformation As DismImageInfo = DismApi.GetImageInfo(CurrentImage.ImageFile).ElementAtOrDefault(CurrentImage.ImageIndex - 1)
                    If ImageInformation IsNot Nothing Then
                        CurrentImage.ImageName = ImageInformation.ImageName
                        CurrentImage.ImageDescription = ImageInformation.ImageDescription
                        CurrentImage.ImageHal = ImageInformation.Hal
                        CurrentImage.ImageArchitecture = ImageInformation.Architecture
                        CurrentImage.ImageSpBuild = ImageInformation.ProductVersion.Revision
                        CurrentImage.ImageSpLevel = ImageInformation.SpLevel
                        CurrentImage.ImageEditionId = ImageInformation.EditionId

                        ' HACK: The edition ID might not be the correct one in an image. There are certain
                        ' WinPE images (like Sergei Strelec WinPE) that use a different EditionID. Images such
                        ' as Sergei Strelec WinPE are not recommended to be used in this program because they are
                        ' heavily modified, to the point even DISM can't service them. Detect these WinPE images in disguise.
                        If ImageInformation.EditionId <> "WindowsPE" Then
                            CurrentImage.WinPeInDisguise = IsWinPeInDisguise(MountDir)
                        End If

                        CurrentImage.ImageInstallationType = ImageInformation.InstallationType
                        CurrentImage.ImageProductType = ImageInformation.ProductType
                        CurrentImage.ImageProductSuite = ImageInformation.ProductSuite
                        CurrentImage.ImageSystemRoot = ImageInformation.SystemRoot
                        CurrentImage.ImageLanguages = ImageInformation.Languages
                        CurrentImage.ImageDefaultLanguage = ImageInformation.DefaultLanguage
                        If ImageInformation.CustomizedInfo IsNot Nothing Then
                            CurrentImage.ImageFileCount = ImageInformation.CustomizedInfo.FileCount
                            CurrentImage.ImageDirectoryCount = ImageInformation.CustomizedInfo.DirectoryCount
                            CurrentImage.ImageCreationDate = ImageInformation.CustomizedInfo.CreatedTime
                            CurrentImage.ImageModificationDate = ImageInformation.CustomizedInfo.ModifiedTime
                        Else
                            ' Either this is a FFU file or it's a badly made WIM.
                            CurrentImage.ImageFileCount = 0
                            CurrentImage.ImageDirectoryCount = 0
                            CurrentImage.ImageCreationDate = Date.MinValue
                            CurrentImage.ImageModificationDate = Date.MinValue
                        End If
                        CurrentImage.ImageSize = ImageInformation.ImageSize
                        DynaLog.LogMessage("Getting WIMBoot information")
                        Dim args As String = "/English",
                            out As String = ""
                        If DismVersionChecker.ProductMajorPart = 6 AndAlso DismVersionChecker.FileMinorPart = 1 Then
                            args &= String.Format(" /get-wiminfo /wimfile={0} ", Quote & CurrentImage.ImageFile & Quote)
                        Else
                            args &= String.Format(" /get-imageinfo /imagefile={0} ", Quote & CurrentImage.ImageFile & Quote)
                        End If
                        args &= String.Format(" /index={0}", CurrentImage.ImageIndex)
                        Using WIMBootProc As New Process() With {
                            .StartInfo = New ProcessStartInfo() With {
                                .FileName = DismExe,
                                .Arguments = args,
                                .UseShellExecute = False,
                                .CreateNoWindow = True,
                                .RedirectStandardOutput = True,
                                .RedirectStandardError = True,
                                .WindowStyle = ProcessWindowStyle.Hidden
                            }
                        }
                            WIMBootProc.Start()
                            out = WIMBootProc.StandardOutput.ReadToEnd()
                            WIMBootProc.WaitForExit()

                            If WIMBootProc.ExitCode = 0 Then
                                CurrentImage.ImageWimBootCompatible = out.ToLower().Contains("wim bootable : yes")
                            End If
                        End Using
                        If Path.GetExtension(CurrentImage.ImageFile).EndsWith("ffu", StringComparison.OrdinalIgnoreCase) Then
                            GetFFUInformation(CurrentImage)
                        End If
                        DynaLog.LogMessage(CurrentImage.ToString())
                        DetectVersions(FileVersionInfo.GetVersionInfo(DismExe), CurrentImage.ImageVersion)
                    End If
                Catch ex As Exception
                    Exit Try
                End Try
                DynaLog.LogMessage("Updating buttons...")
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
                DynaLog.LogMessage("Updating buttons...")
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
        End If
    End Sub

    Sub DetectVersions(DismVer As FileVersionInfo, NTVer As Version)
        Try
            ' Restore enabled properties of each menu item and group in the new design
            DynaLog.LogMessage("Restoring state of items...")
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

            DynaLog.LogMessage("Provided version information:")
            DynaLog.LogMessage("- Information of ntoskrnl: " & NTVer.ToString())
            DynaLog.LogMessage("- Information of DISM: " & DismVer.ProductVersion)

            ' Detect if an image has been mounted, and act accordingly
            If IsImageMounted Then
                DynaLog.LogMessage("An image has been mounted. Comparing ntoskrnl versions...")
                ' Now, detect the Windows version
                Select Case NTVer.Major
                    Case 6
                        Select Case NTVer.Minor
                            Case 1
                                DynaLog.LogMessage("The Windows image contains Windows 7. Disabling AppX, WIMBoot and capability-related actions...")
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
                                ReservedStorageToolStripMenuItem.Enabled = False
                                SetSysUILang.Enabled = False
                                ProvisioningPackagesToolStripMenuItem.Enabled = False
                                OSUninstallToolStripMenuItem.Enabled = False
                            Case 2
                                Select Case NTVer.Build
                                    Case Is >= 8102
                                        DynaLog.LogMessage("The Windows image contains Windows Developer Preview or higher. Disabling WIMBoot and capability-related actions...")
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
                                        DynaLog.LogMessage("The Windows image contains an earlier beta build of Windows 8. Disabling AppX, WIMBoot and capability-related actions...")
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
                                DynaLog.LogMessage("The Windows image contains Windows 8.1. Disabling capability-related actions...")
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
                                DynaLog.LogMessage("The Windows image contains Windows 10")
                                ' Microsoft Edge stuff only affects Windows 11
                                MicrosoftEdgeToolStripMenuItem.Enabled = False
                        End Select
                End Select

                ' Disable Windows PE stuff when not working with a Windows PE image
                WindowsPEServicingToolStripMenuItem.Enabled = CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise
                GroupBox10.Enabled = CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise
                ' Disable AppX and capability stuff when working with a Windows PE image
                AppPackagesToolStripMenuItem.Enabled = (Not (CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise) And IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe"))
                CapabilitiesToolStripMenuItem.Enabled = (Not (CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise) And IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe"))
                GroupBox7.Enabled = (Not (CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise) And IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe"))
                GroupBox8.Enabled = (Not (CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise) And IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe"))

                ' Next, detect the DISM version, so that we can determine which things are applicable
                DynaLog.LogMessage("Comparing DISM versions...")
                Select Case DismVer.ProductMajorPart
                    Case 6
                        Select Case DismVer.ProductMinorPart
                            Case 1
                                DynaLog.LogMessage("Provided DISM is from Windows 7")
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
                                DynaLog.LogMessage("Provided DISM is from Windows 8")
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
                                DynaLog.LogMessage("Provided DISM is from Windows 8.1")
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
                        DynaLog.LogMessage("Provided DISM is from Windows 10 or newer")
                        ' Everything is enabled
                End Select
            Else

            End If
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Detects the image's version by gathering the product version from its "ntoskrnl.exe" file in MountDir\Windows\System32
    ''' </summary>
    ''' <param name="NTKeExe">The path of the "NT Kernel &amp; System" (ntoskrnl.exe) file</param>
    ''' <remarks>The program, depending on the NT version of the image, will enable and disable certain features. This happens because the image may not be applicable for such actions. Also, it is called to detect whether a Windows Vista/Server 2008 image has been mounted; as DISM can mount Vista (NT 6.0) images, but cannot service them. In that case, you would need to use separate tools, like ImageX</remarks>
    Sub DetectNTVersion(NTKeExe As String)
        Try
            DynaLog.LogMessage("Getting version information with provided ntoskrnl (" & Quote & NTKeExe & Quote & ")...")
            Dim NTKeVerInfo As FileVersionInfo
            NTKeVerInfo = FileVersionInfo.GetVersionInfo(NTKeExe)
            DynaLog.LogMessage("Version of provided ntoskrnl: " & NTKeVerInfo.ProductVersion)
            If NTKeVerInfo.ProductMajorPart >= 6 And NTKeVerInfo.ProductBuildPart >= 6000 Then
                DynaLog.LogMessage("The image contains Windows Vista/Server 2008 or a newer OS")
                If NTKeVerInfo.ProductMajorPart = 6 And NTKeVerInfo.ProductMinorPart = 0 Then
                    DynaLog.LogMessage("Correction: the image contains Windows Vista/Server 2008. These are unsupported by both DISMTools and DISM")
                    IsCompatible = False
                Else
                    DynaLog.LogMessage("The image contains a version of Windows newer than Windows Vista/Server 2008. It becomes compatible to a certain degree - this depends on the Windows image")
                    IsCompatible = True
                End If
            Else
                DynaLog.LogMessage("Someone just tossed Windows XP or earlier onto a WIM file. Why would you want to do this? Panther does not support WINNT/WINNT32")
                IsCompatible = False
            End If
        Catch ex As Exception
            DynaLog.LogMessage("We could not grab information about ntoskrnl. It's therefore something wrong with the Windows image or a PoS Win9x image")
            If IsImageMounted Then IsCompatible = False
        End Try
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
        Try
            If Not File.Exists(NTKeExe) Then Return False
            DynaLog.LogMessage("Detecting ntoskrnl version...")
            Dim KeFVI As FileVersionInfo = FileVersionInfo.GetVersionInfo(NTKeExe)
            DynaLog.LogMessage("Provided ntoskrnl version: " & KeFVI.ProductVersion)
            Select Case KeFVI.ProductMajorPart
                Case 6
                    Select Case KeFVI.ProductMinorPart
                        Case 1
                            DynaLog.LogMessage("Image is running a Windows version older than Windows 8")
                            Return False
                        Case Is >= 2
                            Select Case KeFVI.ProductBuildPart
                                Case Is >= 8102
                                    DynaLog.LogMessage("Image is running Windows Developer Preview or later")
                                    Return True
                                Case Else
                                    DynaLog.LogMessage("Image is not running Windows Developer Preview or later")
                                    Return False
                            End Select
                    End Select
                Case 10
                    DynaLog.LogMessage("Image is running a Windows version newer than Windows 8")
                    Return True
                Case Else
                    Return False
            End Select
            Return False
        Catch ex As Exception
            DynaLog.LogMessage("Could not detect ntoskrnl version. Error message: " & ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Determines whether the loaded image is running a Windows version newer than Windows 10
    ''' </summary>
    ''' <param name="NTKeExe">The path of the "NT Kernel &amp; System" file</param>
    ''' <returns>The function returns True when image is running Windows 10 or newer (Image = 10.0)</returns>
    ''' <remarks></remarks>
    Function IsWindows10OrHigher(NTKeExe As String) As Boolean
        Try
            If Not File.Exists(NTKeExe) Then Return False
            DynaLog.LogMessage("Detecting ntoskrnl version...")
            Dim KeFVI As FileVersionInfo = FileVersionInfo.GetVersionInfo(NTKeExe)
            DynaLog.LogMessage("Provided ntoskrnl version: " & KeFVI.ProductVersion)
            Select Case KeFVI.ProductMajorPart
                Case Is <= 6
                    DynaLog.LogMessage("Image is running a Windows version older than Windows 10")
                    Return False
                Case 10
                    DynaLog.LogMessage("Image is running Windows 10 or Windows 11")
                    Return True
            End Select
            Return False
        Catch ex As Exception
            DynaLog.LogMessage("Could not detect ntoskrnl version. Error message: " & ex.Message)
            Return False
        End Try
    End Function

    Public Event APIExceptionThrown(errorEx As Exception, bgProcTitle As String)

    Private Sub APIExceptionHandler(errorEx As Exception, bgProcTitle As String) Handles Me.APIExceptionThrown
        DynaLog.LogMessage("An error occurred with the DISM API. Error message: " & errorEx.Message)
        FailedBGProcResultDic.Add(bgProcTitle, errorEx)
    End Sub

    Sub ThrowAPIException(ProcessTitle As String, Optional APIException As DismException = Nothing, Optional GeneralException As Exception = Nothing)
        Dim errorEx As Exception = Nothing
        If APIException IsNot Nothing Then errorEx = New Exception(String.Format("DISM API Task Error: {0}", New Win32Exception(APIException.HResult).Message), APIException)
        If GeneralException IsNot Nothing Then errorEx = New Exception(String.Format("DISM Task Error: {0}", New Win32Exception(GeneralException.HResult).Message), GeneralException)
        DynaLog.LogMessage("Raising awareness to the event handler")
        RaiseEvent APIExceptionThrown(errorEx, ProcessTitle)
    End Sub

    ''' <summary>
    ''' Gets installed packages in an image and puts them in separate arrays
    ''' </summary>
    Sub GetImagePackages(Optional OnlineMode As Boolean = False)
        DynaLog.LogMessage("Getting OS package information...")
        Try
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
            DynaLog.LogMessage("Creating session...")
            Using session As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(sessionMntDir))
                Dim imgPackageNameList As New List(Of String)
                Dim imgPackageStateList As New List(Of String)
                Dim imgPackageRelTypeList As New List(Of String)
                Dim imgPackageInstTimeList As New List(Of String)
                Dim PackageCollection As DismPackageCollection = DismApi.GetPackages(session)
                DynaLog.LogMessage("Total amount of packages obtained: " & PackageCollection.Count)
                DynaLog.LogMessage("Package information will not be listed here")
                If CurrentImage IsNot Nothing Then CurrentImage.ImagePackages = PackageCollection
                If ImgBW.CancellationPending Then
                    DynaLog.LogMessage("The user is cancelling these processes. Exiting...")
                    If session IsNot Nothing Then DismApi.CloseSession(session)
                    CompletedTasks(0) = False
                    PendingTasks(0) = True
                    Exit Sub
                End If
            End Using
        Catch ex As Exception
            DynaLog.LogMessage("Could not get package information with DISM API. Error: " & ex.Message)
            ThrowAPIException("Package Information", ex)
            DynaLog.LogMessage("Getting package information with DISM executable...")
            If Not GetImagePackagesWithExecutable(OnlineMode) Then
                DynaLog.LogMessage("Package information could not be obtained with DISM executable.")
            End If
        Finally
            DynaLog.LogMessage("Shutting down API...")
            DismApi.Shutdown()
        End Try
        DynaLog.LogMessage("Signaling completion of task...")
        CompletedTasks(0) = True
        PendingTasks(0) = False
    End Sub

    Function GetImagePackagesWithExecutable(Optional OnlineMode As Boolean = False) As Boolean
        Dim args As String = String.Format("/English {0} /get-packages", If(OnlineMode, "/online", String.Format("/image={0}", Quote & MountDir & Quote)))

        Using pkgProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = DismExe,
                .Arguments = args,
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden,
                .UseShellExecute = False,
                .RedirectStandardOutput = True,
                .RedirectStandardError = True
            }
        }
            pkgProc.Start()
            Dim output As String = pkgProc.StandardOutput.ReadToEnd()
            pkgProc.WaitForExit()
            If pkgProc.ExitCode <> 0 Then
                ThrowAPIException("Package Information (DISM Executable)", GeneralException:=New Win32Exception(pkgProc.ExitCode))
                Return False
            End If

            ' Parse the output
            Dim outputLines As String() = output.Split({vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries).SkipWhile(Function(line) Not line.StartsWith("Package Identity : ", StringComparison.InvariantCultureIgnoreCase)).ToArray()
            Dim pkgNameString As String = "",
                pkgStateString As String = "",
                pkgReleaseTypeString As String = "",
                pkgInstallTimeString As String = ""
            For Each outputLine In outputLines
                If outputLine.StartsWith("Package Identity : ") Then
                    pkgNameString = outputLine.Replace("Package Identity : ", "")
                ElseIf outputLine.StartsWith("State : ") Then
                    pkgStateString = outputLine.Replace("State : ", "")
                ElseIf outputLine.StartsWith("Release Type : ") Then
                    pkgReleaseTypeString = outputLine.Replace("Release Type : ", "")
                ElseIf outputLine.StartsWith("Install Time : ") Then
                    pkgInstallTimeString = outputLine.Replace("Install Time : ", "")
                End If

                ' If we've grabbed everything at this point, we add it to our list,
                ' then clear everything and move on.
                If pkgNameString <> "" AndAlso
                        pkgStateString <> "" AndAlso
                        pkgReleaseTypeString <> "" AndAlso
                        pkgInstallTimeString <> "" Then

                    CurrentImage.ImagePackages_Backup.Add(New ImagePackage(pkgNameString,
                                                                                 Casters.CastDismPackageStateString(pkgStateString),
                                                                                 New Date(pkgInstallTimeString),
                                                                                 Casters.CastDismReleaseTypeString(pkgReleaseTypeString)))
                    pkgNameString = ""
                    pkgStateString = ""
                    pkgReleaseTypeString = ""
                    pkgInstallTimeString = ""
                End If
            Next
        End Using
        Return True
    End Function

    ''' <summary>
    ''' Gets present features in an image and puts them in separate arrays
    ''' </summary>
    Sub GetImageFeatures(Optional OnlineMode As Boolean = False)
        DynaLog.LogMessage("Getting feature information...")
        Try
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
            DynaLog.LogMessage("Creating session...")
            Using session As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(sessionMntDir))
                Dim imgFeatureNameList As New List(Of String)
                Dim imgFeatureStateList As New List(Of String)
                Dim FeatureCollection As DismFeatureCollection = DismApi.GetFeatures(session)
                DynaLog.LogMessage("Total amount of features obtained: " & FeatureCollection.Count & ". States will be parsed without logging.")
                If CurrentImage IsNot Nothing Then CurrentImage.ImageFeatures = FeatureCollection
                If ImgBW.CancellationPending Then
                    DynaLog.LogMessage("The user is cancelling these processes. Exiting...")
                    If session IsNot Nothing Then DismApi.CloseSession(session)
                    CompletedTasks(1) = False
                    PendingTasks(1) = True
                    Exit Sub
                End If
            End Using
        Catch ex As Exception
            DynaLog.LogMessage("Could not get feature information. Error: " & ex.Message)
            DynaLog.LogMessage("Getting feature information with DISM executable...")
            ThrowAPIException("Feature Information", ex)
            If Not GetImageFeaturesWithExecutable(OnlineMode) Then
                DynaLog.LogMessage("Feature information could not be obtained with DISM executable.")
            End If
        Finally
            DynaLog.LogMessage("Shutting down API...")
            DismApi.Shutdown()
        End Try
        DynaLog.LogMessage("Signaling completion of task...")
        CompletedTasks(1) = True
        PendingTasks(1) = False
    End Sub

    Function GetImageFeaturesWithExecutable(Optional OnlineMode As Boolean = False) As Boolean
        Dim args As String = String.Format("/English {0} /get-features", If(OnlineMode, "/online", String.Format("/image={0}", Quote & MountDir & Quote)))

        Using featProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = DismExe,
                .Arguments = args,
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden,
                .UseShellExecute = False,
                .RedirectStandardOutput = True,
                .RedirectStandardError = True
            }
        }
            featProc.Start()
            Dim output As String = featProc.StandardOutput.ReadToEnd()
            featProc.WaitForExit()
            If featProc.ExitCode <> 0 Then
                ThrowAPIException("Feature Information (DISM Executable)", GeneralException:=New Win32Exception(featProc.ExitCode))
                Return False
            End If

            ' Parse the output
            Dim outputLines As String() = output.Split({vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries).SkipWhile(Function(line) Not line.StartsWith("Feature Name : ", StringComparison.InvariantCultureIgnoreCase)).ToArray()
            Dim featNameString As String = "",
                featStateString As String = ""
            For Each outputLine In outputLines
                If outputLine.StartsWith("Feature Name : ") Then
                    featNameString = outputLine.Replace("Feature Name : ", "")
                ElseIf outputLine.StartsWith("State : ") Then
                    featStateString = outputLine.Replace("State : ", "")
                End If

                ' If we've grabbed everything at this point, we add it to our list,
                ' then clear everything and move on.
                If featNameString <> "" AndAlso
                    featStateString <> "" Then
                    CurrentImage.ImageFeatures_Backup.Add(New ImageFeature(featNameString, Casters.CastDismPackageStateString(featStateString)))

                    featNameString = ""
                    featStateString = ""
                End If
            Next
        End Using
        Return True
    End Function

    ''' <summary>
    ''' Gets installed provisioned APPX packages in an image and puts them in separate arrays
    ''' </summary>
    ''' <remarks>This is only for Windows 8 and newer</remarks>
    Sub GetImageAppxPackages(Optional OnlineMode As Boolean = False)
        DynaLog.LogMessage("Getting AppX package information...")

        Dim imgAppxDisplayNameList As New List(Of String)
        Dim imgAppxPackageNameList As New List(Of String)
        Dim imgAppxVersionList As New List(Of String)
        Dim imgAppxArchitectureList As New List(Of String)
        Dim imgAppxResourceIdList As New List(Of String)

        If Environment.OSVersion.Version.Major > 6 Then
            DynaLog.LogMessage("Host system is running Windows 10 or newer. We have the benefit of using the API")
            Try
                DynaLog.LogMessage("Initializing API...")
                DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
                DynaLog.LogMessage("Creating session...")
                Using session As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(sessionMntDir))
                    Dim imgAppxRegionList As New List(Of String)
                    Dim AppxPackageCollection As DismAppxPackageCollection = DismApi.GetProvisionedAppxPackages(session)
                    DynaLog.LogMessage("Total amount of AppX packages obtained: " & AppxPackageCollection.Count & ". Architectures will be parsed without logging.")
                    If CurrentImage IsNot Nothing Then CurrentImage.ImageAppxPackages = AppxPackageCollection
                    If ImgBW.CancellationPending Then
                        DynaLog.LogMessage("The user is cancelling these processes. Exiting...")
                        If session IsNot Nothing Then DismApi.CloseSession(session)
                        CompletedTasks(2) = False
                        PendingTasks(2) = True
                        Exit Sub
                    End If
                    If OnlineMode And ExtAppxGetter Then
                        DynaLog.LogMessage("Calling helper script...")
                        Dim PSExtAppxGetterOutput As String = GetPSExtAppxGetterOutput()

                        If PSExtAppxGetterOutput <> "" Then
                            Dim deserializer As New XmlSerializer(GetType(PSInterop.PsObjects))
                            Dim objectsCollection As New PSInterop.PsObjects()
                            Using reader As New StringReader(PSExtAppxGetterOutput)
                                objectsCollection = CType(deserializer.Deserialize(reader), PSInterop.PsObjects)
                            End Using
                            If objectsCollection.Items.Count > 0 Then
                                For Each item In objectsCollection.Items
                                    Dim propertyDictionary As Dictionary(Of String, String) = item.Properties.ToDictionary(Function(prop) prop.Name,
                                                                                                               Function(prop) prop.Value)
                                    CurrentImage.ImageAppxPackages_Backup.Add(New ImageAppxPackage(propertyDictionary("Name"),
                                                                                                 propertyDictionary("PackageFullName"),
                                                                                                 Casters.CastDismArchitectureString(propertyDictionary("Architecture")),
                                                                                                 propertyDictionary("ResourceId"),
                                                                                                 New Version(propertyDictionary("Version"))))
                                Next
                            End If
                        End If
                    End If
                End Using
            Catch ex As DismException
                DynaLog.LogMessage("Could not get package information. Error: " & ex.Message)
                ThrowAPIException("AppX Package Information", ex)
            Finally
                DynaLog.LogMessage("Shutting down API...")
                DismApi.Shutdown()
            End Try
            DynaLog.LogMessage("Signaling completion of task...")
            CompletedTasks(2) = True
            PendingTasks(2) = False
            Exit Sub
        End If
        DynaLog.LogMessage("Host system is running Windows 8. Use DISM executable. DISM API when processing this info on Windows 8 crashes the program")
        ' The mounted image may be Windows 8 or later, but DISM may be from Windows 7. Get this information before running this procedure
        Dim FileVersion As FileVersionInfo = FileVersionInfo.GetVersionInfo(DismExe)
        If FileVersion.ProductMajorPart = 6 AndAlso FileVersion.ProductMinorPart = 1 Then
            DynaLog.LogMessage("The image is Windows 8 or later, but this version of DISM does not support this command. Exiting...")
            DynaLog.LogMessage("Signaling completion of task...")
            CompletedTasks(2) = False
            PendingTasks(2) = True
            Exit Sub
        End If
        CurrentImage.ImageAppxPackages_Backup.Clear()
        ' Run DISM and parse the output in one go.
        Dim args As String = String.Format("/English {0} /get-provisionedappxpackages", If(OnlineMode, "/online", String.Format("/image={0}", Quote & MountDir & Quote)))
        Using appxProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = DismExe,
                .Arguments = args,
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden,
                .UseShellExecute = False,
                .RedirectStandardOutput = True,
                .RedirectStandardError = True
            }
        }
            appxProc.Start()
            Dim output As String = appxProc.StandardOutput.ReadToEnd()
            appxProc.WaitForExit()
            If appxProc.ExitCode = 0 Then
                ' Parse the output.
                Dim outputLines As String() = output.Split({vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries).SkipWhile(Function(line) Not line.StartsWith("DisplayName : ", StringComparison.InvariantCultureIgnoreCase)).ToArray()
                Dim appxDisplayNameString As String = "",
                    appxVersionString As String = "",
                    appxArchitectureString As String = "",
                    appxResourceIdString As String = "",
                    appxPackageNameString As String = ""
                For Each outputLine In outputLines
                    If outputLine.StartsWith("DisplayName : ") Then
                        appxDisplayNameString = outputLine.Replace("DisplayName : ", "")
                    ElseIf outputLine.StartsWith("Version : ") Then
                        appxVersionString = outputLine.Replace("Version : ", "")
                    ElseIf outputLine.StartsWith("Architecture : ") Then
                        appxArchitectureString = outputLine.Replace("Architecture : ", "")
                    ElseIf outputLine.StartsWith("ResourceId : ") Then
                        appxResourceIdString = outputLine.Replace("ResourceId : ", "")
                    ElseIf outputLine.StartsWith("PackageName : ") Then
                        appxPackageNameString = outputLine.Replace("PackageName : ", "")
                    End If

                    ' If we've grabbed everything at this point, we add it to our list,
                    ' then clear everything and move on.
                    If appxDisplayNameString <> "" AndAlso
                            appxVersionString <> "" AndAlso
                            appxArchitectureString <> "" AndAlso
                            appxResourceIdString <> "" AndAlso
                            appxPackageNameString <> "" Then
                        CurrentImage.ImageAppxPackages_Backup.Add(New ImageAppxPackage(appxDisplayNameString,
                                                                                     appxPackageNameString,
                                                                                     Casters.CastDismArchitectureString(appxArchitectureString),
                                                                                     appxResourceIdString,
                                                                                     New Version(appxVersionString)))
                        appxDisplayNameString = ""
                        appxVersionString = ""
                        appxArchitectureString = ""
                        appxResourceIdString = ""
                        appxPackageNameString = ""
                    End If
                Next
            Else
                ThrowAPIException("AppX Package Information", GeneralException:=New Win32Exception(appxProc.ExitCode))
            End If
        End Using
        If OnlineMode And ExtAppxGetter Then
            Dim PSExtAppxGetterOutput As String = GetPSExtAppxGetterOutput()

            If PSExtAppxGetterOutput <> "" Then
                Dim deserializer As New XmlSerializer(GetType(PSInterop.PsObjects))
                Dim objectsCollection As New PSInterop.PsObjects()
                Using reader As New StringReader(PSExtAppxGetterOutput)
                    objectsCollection = CType(deserializer.Deserialize(reader), PSInterop.PsObjects)
                End Using
                If objectsCollection.Items.Count > 0 Then
                    For Each item In objectsCollection.Items
                        Dim propertyDictionary As Dictionary(Of String, String) = item.Properties.ToDictionary(Function(prop) prop.Name,
                                                                                                               Function(prop) prop.Value)
                        CurrentImage.ImageAppxPackages_Backup.Add(New ImageAppxPackage(propertyDictionary("Name"),
                                                                                                 propertyDictionary("PackageFullName"),
                                                                                                 Casters.CastDismArchitectureString(propertyDictionary("Architecture")),
                                                                                                 propertyDictionary("ResourceId"),
                                                                                                 New Version(propertyDictionary("Version"))))
                    Next
                End If
            End If
        End If
        DynaLog.LogMessage("Signaling completion of task...")
        CompletedTasks(2) = True
        PendingTasks(2) = False
        'ImgBW.ReportProgress(progressMin + progressDivs)
    End Sub

    Function GetPSExtAppxGetterOutput() As String
        Dim output As String = ""
        DynaLog.LogMessage("Running PowerShell script...")
        Using PSExtAppxProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe",
                .WorkingDirectory = Application.StartupPath,
                .Arguments = "-executionpolicy unrestricted -file " & Quote & Application.StartupPath & "\bin\extps1\extappx.ps1" & Quote & String.Format("{0}{1}", If(SkipNonRemovable, " -noNonRemovable " & Quote & "true" & Quote, ""), If(SkipFrameworks, " -noFramework " & Quote & "true" & Quote, "")),
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden,
                .UseShellExecute = False,
                .RedirectStandardOutput = True
            }
        }
            Try
                PSExtAppxProc.StartInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding(Globalization.CultureInfo.CurrentCulture.TextInfo.OEMCodePage)
            Catch ex As Exception
                PSExtAppxProc.StartInfo.StandardOutputEncoding = Nothing
            End Try
            PSExtAppxProc.Start()
            output = PSExtAppxProc.StandardOutput.ReadToEnd()
            PSExtAppxProc.WaitForExit()
        End Using
        Return output
    End Function

    ''' <summary>
    ''' Gets installed Features on Demand (capabilities) in an image and puts them in separate arrays
    ''' </summary>
    ''' <remarks>This is only for Windows 10 or newer</remarks>
    Sub GetImageCapabilities(Optional OnlineMode As Boolean = False)
        DynaLog.LogMessage("Getting capability information...")
        Try
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
            DynaLog.LogMessage("Creating session...")
            Using session As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(sessionMntDir))
                Dim imgCapabilityNameList As New List(Of String)
                Dim imgCapabilityStateList As New List(Of String)
                Dim CapabilityCollection As DismCapabilityCollection = DismApi.GetCapabilities(session)
                DynaLog.LogMessage("Total amount of capabilities obtained: " & CapabilityCollection.Count & ". States will be parsed without logging.")
                If CurrentImage IsNot Nothing Then CurrentImage.ImageCapabilities = CapabilityCollection
                If ImgBW.CancellationPending Then
                    DynaLog.LogMessage("The user is cancelling these processes. Exiting...")
                    If session IsNot Nothing Then DismApi.CloseSession(session)
                    CompletedTasks(3) = False
                    PendingTasks(3) = True
                    Exit Sub
                End If
            End Using
        Catch ex As Exception
            DynaLog.LogMessage("Could not get capability information. Error: " & ex.Message)
            DynaLog.LogMessage("Getting capability information with DISM executable...")
            ThrowAPIException("Capability Information", ex)
            If Not GetImageCapabilitiesWithExecutable(OnlineMode) Then
                DynaLog.LogMessage("Capability information could not be obtained with DISM executable.")
            End If
        Finally
            DynaLog.LogMessage("Shutting down API...")
            DismApi.Shutdown()
        End Try
        DynaLog.LogMessage("Signaling completion of task...")
        CompletedTasks(3) = True
        PendingTasks(3) = False
    End Sub

    Function GetImageCapabilitiesWithExecutable(Optional OnlineMode As Boolean = False) As Boolean
        Dim args As String = String.Format("/English {0} /get-capabilities", If(OnlineMode, "/online", String.Format("/image={0}", Quote & MountDir & Quote)))

        Using capProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = DismExe,
                .Arguments = args,
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden,
                .UseShellExecute = False,
                .RedirectStandardOutput = True,
                .RedirectStandardError = True
            }
        }
            capProc.Start()
            Dim output As String = capProc.StandardOutput.ReadToEnd()
            capProc.WaitForExit()

            If capProc.ExitCode <> 0 Then
                ThrowAPIException("Capability Information (DISM Executable)", GeneralException:=New Win32Exception(capProc.ExitCode))
                Return False
            End If

            ' Parse the output
            Dim outputLines As String() = output.Split({vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries).SkipWhile(Function(line) Not line.StartsWith("Capability Identity : ", StringComparison.InvariantCultureIgnoreCase)).ToArray()
            Dim capNameString As String = "",
                capStateString As String = ""
            For Each outputLine In outputLines
                If outputLine.StartsWith("Capability Identity : ") Then
                    capNameString = outputLine.Replace("Capability Identity : ", "")
                ElseIf outputLine.StartsWith("State : ") Then
                    capStateString = outputLine.Replace("State : ", "")
                End If
            Next
        End Using
        Return True
    End Function

    ''' <summary>
    ''' Gets installed third-party drivers in an image and puts them in separate arrays
    ''' </summary>
    ''' <remarks>This procedure will detect the number of third-party drivers. If the image contains none, this procedure will end</remarks>
    Sub GetImageDrivers(Optional OnlineMode As Boolean = False)
        DynaLog.LogMessage("Getting driver information...")
        If IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            DynaLog.LogMessage("Image contains Windows 8 or later. Driver information can be obtained with the DISM API")
            Try
                DynaLog.LogMessage("Initializing API...")
                DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
                DynaLog.LogMessage("Creating session...")
                Using session As DismSession = If(OnlineMode, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(sessionMntDir))
                    If Not AllDrivers Then DynaLog.LogMessage("Not all drivers will be obtained because of a setting in background processes")
                    Dim DriverCollection As DismDriverPackageCollection = DismApi.GetDrivers(session, AllDrivers)
                    DynaLog.LogMessage("Total amount of drivers obtained: " & DriverCollection.Count)
                    If CurrentImage IsNot Nothing Then CurrentImage.ImageDrivers = DriverCollection
                    If ImgBW.CancellationPending Then
                        DynaLog.LogMessage("The user is cancelling these processes. Exiting...")
                        If session IsNot Nothing Then DismApi.CloseSession(session)
                        CompletedTasks(4) = False
                        PendingTasks(4) = True
                        Exit Sub
                    End If
                End Using
            Catch ex As DismException
                DynaLog.LogMessage("Could not get package information. Error: " & ex.Message)
                ThrowAPIException("Driver Information", ex)
            Finally
                DynaLog.LogMessage("Shutting down API...")
                DismApi.Shutdown()
            End Try
            DynaLog.LogMessage("Signaling completion of task...")
            CompletedTasks(4) = True
            PendingTasks(4) = False
            Exit Sub
        End If
        CurrentImage.ImageDrivers_Backup.Clear()
        DynaLog.LogMessage("Running function...")
        DynaLog.LogMessage("Determining whether there are third-party drivers in image...")
        ' Run DISM and parse the output in one go.
        Using DriverEnumerationProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = DismExe,
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
                        CurrentImage.ImageDrivers_Backup.Add(New ImageDriver(drvPublishedNameString,
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
                ThrowAPIException("Driver Information (DISM Executable)", GeneralException:=New Win32Exception(DriverEnumerationProc.ExitCode))
            End If
        End Using
        DynaLog.LogMessage("Signaling completion of task...")
        CompletedTasks(4) = True
        PendingTasks(4) = False
        'ImgBW.ReportProgress(progressMin + progressDivs)
    End Sub

    ''' <summary>
    ''' Deletes temporary files created by the background processes
    ''' </summary>
    Sub DeleteTempFiles()
        If MountedImageDetectorBW.IsBusy Then
            DynaLog.LogMessage("The mounted image detector is busy and uses this folder. Skipping...")
            Exit Sub
        End If
        Try
            DynaLog.LogMessage("Deleting temporary files...")
            Directory.Delete(Application.StartupPath & "\tempinfo", True)
            ' Keep the "exthelpers" folder in case background processes need to be run again
            For Each TempFile In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\bin\exthelpers", FileIO.SearchOption.SearchTopLevelOnly)
                File.Delete(TempFile)
            Next
        Catch ex As Exception
            DynaLog.LogMessage("Could not delete temporary files. Reason: " & ex.Message)
        End Try
    End Sub

#End Region

    Sub GenerateDTSettings()
        DynaLog.LogMessage("Generating new settings file...")
        Dim parser As New FileIniDataParser(),
            settingsData As New IniData()
        settingsData.Sections.AddSection("Program")
        settingsData("Program").AddKey("DismExe", Quote & "{common:WinDir}\system32\dism.exe" & Quote)
        settingsData("Program").AddKey("SaveOnSettingsIni", 1)
        settingsData("Program").AddKey("Volatile", 0)
        settingsData.Sections.AddSection("Personalization")
        Try
            Dim ColorModeRk As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", False)
            Dim ColorMode As String = ColorModeRk.GetValue("AppsUseLightTheme").ToString()
            ColorModeRk.Close()
            DynaLog.LogMessage("Auto Coloring from System is supported (Windows 10+). Enabling system color mode...")
            settingsData("Personalization").AddKey("ColorMode", 0)
        Catch ex As Exception
            ' Rollback to light theme
            DynaLog.LogMessage("Auto Coloring from System is not supported. Falling back to light mode (hopefully you're not using this at night)...")
            settingsData("Personalization").AddKey("ColorMode", 1)
        End Try
        settingsData("Personalization").AddKey("ColorTheme_Light", 1)
        settingsData("Personalization").AddKey("ColorTheme_Dark", 0)
        settingsData("Personalization").AddKey("LanguageCode", Quote & LocalizationService.CurrentCultureCode & Quote)
        settingsData("Personalization").AddKey("LogFont", Quote & "Consolas" & Quote)
        settingsData("Personalization").AddKey("LogFontSi", 11)
        settingsData("Personalization").AddKey("LogFontBold", 0)
        settingsData("Personalization").AddKey("SecondaryProgressPanelStyle", 1)
        settingsData("Personalization").AddKey("AllCaps", 0)
        settingsData("Personalization").AddKey("ExpandedProgressPanel", 1)
        settingsData("Personalization").AddKey("ShowDateAndTime", 1)
        settingsData.Sections.AddSection("Logs")
        settingsData("Logs").AddKey("LogFile", Quote & "{common:WinDir}\Logs\DISM\DISM.log" & Quote)
        settingsData("Logs").AddKey("LogLevel", 3)
        settingsData("Logs").AddKey("AutoLogs", 1)
        settingsData("Logs").AddKey("SystemEditor", Quote & "{common:WinDir}\system32\notepad.exe" & Quote)
        settingsData("Logs").AddKey("EnableDynaLog", 1)
        settingsData.Sections.AddSection("ImgOps")
        settingsData("ImgOps").AddKey("Quiet", 0)
        settingsData("ImgOps").AddKey("NoRestart", 0)
        settingsData("ImgOps").AddKey("NoNTSamMappings", 0)
        settingsData("ImgOps").AddKey("PEHelper.UnattendedFile", Quote & Quote)
        settingsData("ImgOps").AddKey("PEHelper.CopyToVentoy", 0)
        settingsData("ImgOps").AddKey("PEHelper.Use2023EFI", 0)
        settingsData("ImgOps").AddKey("AppxRemovalDisplayNameFormat", 1)
        settingsData("ImgOps").AddKey("PreventSystemFromSleeping", 1)
        settingsData("ImgOps").AddKey("HumanizeDates", 1)
        settingsData.Sections.AddSection("ScratchDir")
        settingsData("ScratchDir").AddKey("UseScratch", 0)
        settingsData("ScratchDir").AddKey("AutoScratch", 1)
        settingsData("ScratchDir").AddKey("ScratchDirLocation", Quote & Quote)
        settingsData.Sections.AddSection("Output")
        settingsData("Output").AddKey("EnglishOutput", 1)
        settingsData("Output").AddKey("ReportView", 0)
        settingsData.Sections.AddSection("BgProcesses")
        settingsData("BgProcesses").AddKey("ShowNotification", 1)
        settingsData("BgProcesses").AddKey("NotifyFrequency", 1)
        settingsData.Sections.AddSection("AdvBgProcesses")
        settingsData("AdvBgProcesses").AddKey("EnhancedAppxGetter", 1)
        settingsData("AdvBgProcesses").AddKey("SkipNonRemovable", 1)
        settingsData("AdvBgProcesses").AddKey("DetectAllDrivers", 0)
        settingsData("AdvBgProcesses").AddKey("SkipFrameworks", 1)
        settingsData("AdvBgProcesses").AddKey("RunAllProcs", 0)
        settingsData.Sections.AddSection("Startup")
        settingsData("Startup").AddKey("RemountImages", 1)
        settingsData("Startup").AddKey("CheckForUpdates", 1)
        settingsData.Sections.AddSection("Shutdown")
        settingsData("Shutdown").AddKey("AutoCleanMounts", 0)
        settingsData.Sections.AddSection("WndParams")
        settingsData("WndParams").AddKey("WndWidth", 1280)
        settingsData("WndParams").AddKey("WndHeight", 720)
        settingsData("WndParams").AddKey("WndCenter", 1)
        settingsData("WndParams").AddKey("WndLeft", 0)
        settingsData("WndParams").AddKey("WndTop", 0)
        settingsData("WndParams").AddKey("WndMaximized", 0)
        settingsData.Sections.AddSection("InfoSaver")
        settingsData("InfoSaver").AddKey("SkipQuestions", 1)
        settingsData("InfoSaver").AddKey("Pkg_CompleteInfo", 1)
        settingsData("InfoSaver").AddKey("Feat_CompleteInfo", 1)
        settingsData("InfoSaver").AddKey("AppX_CompleteInfo", 1)
        settingsData("InfoSaver").AddKey("Cap_CompleteInfo", 1)
        settingsData("InfoSaver").AddKey("Drv_CompleteInfo", 1)
        settingsData.Sections.AddSection("SearchSettings")
        settingsData("SearchSettings").AddKey("EngineName", Quote & "DuckDuckGo" & Quote)
        settingsData("SearchSettings").AddKey("AITolerance", 1)
        settingsData.Sections.AddSection("PEPolicy")
        settingsData("PEPolicy").AddKey("ShowWatermark", 0)
        settingsData("PEPolicy").AddKey("WDSHCGraphoView", 1)
        settingsData("PEPolicy").AddKey("DTDimShowPnputilOut", 1)
        settingsData("PEPolicy").AddKey("WDSHCConnAttempts", 5)
        settingsData("PEPolicy").AddKey("PartTableOverridePreference", 0)
        settingsData("PEPolicy").AddKey("UEFICA23Preference", 0)
        settingsData("PEPolicy").AddKey("AutoUnattendCopytoSysprep", 0)
        settingsData("PEPolicy").AddKey("PXEServerPort", 8080)
        settingsData("PEPolicy").AddKey("KeyboardLayoutCode", Quote & KeyboardLayoutCode & Quote)
        settingsData("PEPolicy").AddKey("KeyboardLayoutOverrideExistingLayout", 0)
        parser.WriteFile(Path.Combine(Application.StartupPath, "settings.ini"), settingsData, UTF8)
        If File.Exists(Application.StartupPath & "\portable") Then Exit Sub
        DynaLog.LogMessage("Portable marker does not exist. Configuring settings in registry...")
        Dim KeyStr As String = "Software\DISMTools\" & If(dtBranch.Contains("pre"), "Preview", "Stable")
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
        PersKey.SetValue("ColorTheme_Light", 1, RegistryValueKind.DWord)
        PersKey.SetValue("ColorTheme_Dark", 0, RegistryValueKind.DWord)
        PersKey.SetValue("LanguageCode", LocalizationService.DefaultCultureCode, RegistryValueKind.String)
        PersKey.SetValue("LogFont", "Consolas", RegistryValueKind.String)
        PersKey.SetValue("LogFontSi", 11, RegistryValueKind.DWord)
        PersKey.SetValue("LogFontBold", 0, RegistryValueKind.DWord)
        PersKey.SetValue("SecondaryProgressPanelStyle", 1, RegistryValueKind.DWord)
        PersKey.SetValue("AllCaps", 0, RegistryValueKind.DWord)
        PersKey.SetValue("ColorSchemes", 0, RegistryValueKind.DWord)
        PersKey.SetValue("ExpandedProgressPanel", 1, RegistryValueKind.DWord)
        PersKey.SetValue("ShowDateAndTime", 1, RegistryValueKind.DWord)
        PersKey.Close()
        Dim LogKey As RegistryKey = Key.CreateSubKey("Logs")
        LogKey.SetValue("LogFile", Quote & Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\logs\DISM\DISM.log" & Quote, RegistryValueKind.ExpandString)
        LogKey.SetValue("LogLevel", 3, RegistryValueKind.DWord)
        LogKey.SetValue("AutoLogs", 1, RegistryValueKind.DWord)
        LogKey.SetValue("SystemEditor", Quote & Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\notepad.exe" & Quote, RegistryValueKind.ExpandString)
        LogKey.SetValue("EnableDynaLog", 1, RegistryValueKind.DWord)
        LogKey.Close()
        Dim ImgOpKey As RegistryKey = Key.CreateSubKey("ImgOps")
        ImgOpKey.SetValue("Quiet", 0, RegistryValueKind.DWord)
        ImgOpKey.SetValue("NoRestart", 0, RegistryValueKind.DWord)
        ImgOpKey.SetValue("NoNTSamMappings", 0, RegistryValueKind.DWord)
        ImgOpKey.SetValue("PEHelper.UnattendedFile", "", RegistryValueKind.String)
        ImgOpKey.SetValue("PEHelper.CopyToVentoy", 0, RegistryValueKind.DWord)
        ImgOpKey.SetValue("PEHelper.Use2023EFI", 0, RegistryValueKind.DWord)
        ImgOpKey.SetValue("AppxRemovalDisplayNameFormat", 1, RegistryValueKind.DWord)
        ImgOpKey.SetValue("PreventSystemFromSleeping", 1, RegistryValueKind.DWord)
        ImgOpKey.SetValue("HumanizeDates", 1, RegistryValueKind.DWord)
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
        Dim ShutdownKey As RegistryKey = Key.CreateSubKey("Startup")
        ShutdownKey.SetValue("AutoCleanMounts", 0, RegistryValueKind.DWord)
        ShutdownKey.Close()
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
        Dim SearchKey As RegistryKey = Key.CreateSubKey("SearchSettings")
        SearchKey.SetValue("EngineName", "DuckDuckGo", RegistryValueKind.String)
        SearchKey.SetValue("AITolerance", 1, RegistryValueKind.DWord)
        SearchKey.Close()
        Dim PEPolicyKey As RegistryKey = Key.CreateSubKey("PEPolicy")
        PEPolicyKey.SetValue("ShowWatermark", 0, RegistryValueKind.DWord)
        PEPolicyKey.SetValue("WDSHCGraphoView", 1, RegistryValueKind.DWord)
        PEPolicyKey.SetValue("DTDimShowPnputilOut", 1, RegistryValueKind.DWord)
        PEPolicyKey.SetValue("WDSHCConnAttempts", 5, RegistryValueKind.DWord)
        PEPolicyKey.SetValue("PartTableOverridePreference", 0, RegistryValueKind.DWord)
        PEPolicyKey.SetValue("UEFICA23Preference", 0, RegistryValueKind.DWord)
        PEPolicyKey.SetValue("AutoUnattendCopytoSysprep", 0, RegistryValueKind.DWord)
        PEPolicyKey.SetValue("KeyboardLayoutCode", KeyboardLayoutCode, RegistryValueKind.String)
        PEPolicyKey.SetValue("KeyboardLayoutOverrideExistingLayout", 0, RegistryValueKind.DWord)
        PEPolicyKey.Close()
        Key.Close()
    End Sub

    Sub SaveDTSettings()
        DynaLog.LogMessage("Determining volatile mode status...")
        
        ShowDTSettings()
        If SaveOnSettingsIni Then
            DynaLog.LogMessage("Checking state of INI File...")
            If File.Exists(Application.StartupPath & "\settings.ini") Then
                DynaLog.LogMessage("Deleting existing INI File...")
                File.Delete(Application.StartupPath & "\settings.ini")
            End If
            DynaLog.LogMessage("Writing to INI...")
            Dim parser As New FileIniDataParser(),
                settingsData As New IniData()
            settingsData.Sections.AddSection("Program")
            settingsData("Program").AddKey("DismExe", Quote & DismExe & Quote)
            settingsData("Program").AddKey("SaveOnSettingsIni", If(SaveOnSettingsIni, 1, 0))
            settingsData.Sections.AddSection("Personalization")
            settingsData("Personalization").AddKey("ColorMode", ColorMode)
            settingsData("Personalization").AddKey("ColorTheme_Light", LightThemeIndex)
            settingsData("Personalization").AddKey("ColorTheme_Dark", DarkThemeIndex)
            LanguageCode = LocalizationService.NormalizeCultureCode(LanguageCode)
            settingsData("Personalization").AddKey("LanguageCode", Quote & LanguageCode & Quote)
            settingsData("Personalization").AddKey("LogFont", Quote & LogFont & Quote)
            settingsData("Personalization").AddKey("LogFontSi", LogFontSize)
            settingsData("Personalization").AddKey("LogFontBold", If(LogFontIsBold, 1, 0))
            settingsData("Personalization").AddKey("SecondaryProgressPanelStyle", ProgressPanelStyle)
            settingsData("Personalization").AddKey("AllCaps", If(AllCaps, 1, 0))
            settingsData("Personalization").AddKey("ExpandedProgressPanel", If(ExpandedProgressPanel, 1, 0))
            settingsData("Personalization").AddKey("ShowDateAndTime", If(ShowDateAndTime, 1, 0))
            settingsData.Sections.AddSection("Logs")
            settingsData("Logs").AddKey("LogFile", Quote & LogFile & Quote)
            settingsData("Logs").AddKey("LogLevel", LogLevel)
            settingsData("Logs").AddKey("AutoLogs", If(AutoLogs, 1, 0))
            settingsData("Logs").AddKey("SystemEditor", Quote & SystemEditor & Quote)
            settingsData("Logs").AddKey("EnableDynaLog", If(EnableDynaLog, 1, 0))
            settingsData.Sections.AddSection("ImgOps")
            settingsData("ImgOps").AddKey("Quiet", If(QuietOperations, 1, 0))
            settingsData("ImgOps").AddKey("NoRestart", If(SysNoRestart, 1, 0))
            settingsData("ImgOps").AddKey("NoNTSamMappings", If(NoNTSamMappings, 1, 0))
            settingsData("ImgOps").AddKey("PEHelper.UnattendedFile", Quote & PEHelper_UnattendedFile & Quote)
            settingsData("ImgOps").AddKey("PEHelper.CopyToVentoy", If(PEHelper_CopyToVentoy, 1, 0))
            settingsData("ImgOps").AddKey("PEHelper.Use2023EFI", If(PEHelper_Use2023EFI, 1, 0))
            settingsData("ImgOps").AddKey("PEHelper.IncludeSysDrvs", If(PEHelper_IncludeSysDrvs, 1, 0))
            settingsData("ImgOps").AddKey("AppxRemovalDisplayNameFormat", AppxDisplayNameFormatOnRemoval)
            settingsData("ImgOps").AddKey("PreventSystemFromSleeping", If(PreventSystemFromSleeping, 1, 0))
            settingsData("ImgOps").AddKey("HumanizeDates", If(HumanizeDates, 1, 0))
            settingsData.Sections.AddSection("ScratchDir")
            settingsData("ScratchDir").AddKey("UseScratch", If(UseScratch, 1, 0))
            settingsData("ScratchDir").AddKey("AutoScratch", If(AutoScrDir, 1, 0))
            settingsData("ScratchDir").AddKey("ScratchDirLocation", Quote & ScratchDir & Quote)
            settingsData.Sections.AddSection("Output")
            settingsData("Output").AddKey("EnglishOutput", If(EnglishOutput, 1, 0))
            settingsData("Output").AddKey("ReportView", ReportView)
            settingsData.Sections.AddSection("BgProcesses")
            settingsData("BgProcesses").AddKey("ShowNotification", If(NotificationShow, 1, 0))
            settingsData("BgProcesses").AddKey("NotifyFrequency", NotificationFrequency)
            settingsData.Sections.AddSection("AdvBgProcesses")
            settingsData("AdvBgProcesses").AddKey("EnhancedAppxGetter", If(ExtAppxGetter, 1, 0))
            settingsData("AdvBgProcesses").AddKey("SkipNonRemovable", If(SkipNonRemovable, 1, 0))
            settingsData("AdvBgProcesses").AddKey("DetectAllDrivers", If(AllDrivers, 1, 0))
            settingsData("AdvBgProcesses").AddKey("SkipFrameworks", If(SkipFrameworks, 1, 0))
            settingsData("AdvBgProcesses").AddKey("RunAllProcs", If(RunAllProcs, 1, 0))
            settingsData.Sections.AddSection("Startup")
            settingsData("Startup").AddKey("RemountImages", If(StartupRemount, 1, 0))
            settingsData("Startup").AddKey("CheckForUpdates", If(StartupUpdateCheck, 1, 0))
            settingsData.Sections.AddSection("Shutdown")
            settingsData("Shutdown").AddKey("AutoCleanMounts", If(AutoCleanMounts, 1, 0))
            settingsData.Sections.AddSection("WndParams")
            settingsData("WndParams").AddKey("WndWidth", Math.Round(WndWidth / (WindowHelper.GetSystemDpi() / 100), 0))
            settingsData("WndParams").AddKey("WndHeight", Math.Round(WndHeight / (WindowHelper.GetSystemDpi() / 100), 0))
            settingsData("WndParams").AddKey("WndCenter", If(Location = New Point((Screen.FromControl(Me).WorkingArea.Width - Width) / 2, (Screen.FromControl(Me).WorkingArea.Height - Height) / 2), 1, 0))
            settingsData("WndParams").AddKey("WndLeft", WndLeft)
            settingsData("WndParams").AddKey("WndTop", WndTop)
            settingsData("WndParams").AddKey("WndMaximized", If(WindowState = FormWindowState.Maximized, 1, 0))
            settingsData.Sections.AddSection("InfoSaver")
            settingsData("InfoSaver").AddKey("SkipQuestions", If(SkipQuestions, 1, 0))
            settingsData("InfoSaver").AddKey("Pkg_CompleteInfo", If(AutoCompleteInfo(0), 1, 0))
            settingsData("InfoSaver").AddKey("Feat_CompleteInfo", If(AutoCompleteInfo(1), 1, 0))
            settingsData("InfoSaver").AddKey("AppX_CompleteInfo", If(AutoCompleteInfo(2), 1, 0))
            settingsData("InfoSaver").AddKey("Cap_CompleteInfo", If(AutoCompleteInfo(3), 1, 0))
            settingsData("InfoSaver").AddKey("Drv_CompleteInfo", If(AutoCompleteInfo(4), 1, 0))
            settingsData.Sections.AddSection("SearchSettings")
            settingsData("SearchSettings").AddKey("EngineName", Quote & SearchEngineName & Quote)
            settingsData("SearchSettings").AddKey("AITolerance", SearchEngineAITolerance)
            settingsData.Sections.AddSection("PEPolicy")
            settingsData("PEPolicy").AddKey("ShowWatermark", If(ShowWatermark, 1, 0))
            settingsData("PEPolicy").AddKey("WDSHCGraphoView", If(WDSHCGraphoView, 1, 0))
            settingsData("PEPolicy").AddKey("DTDimShowPnputilOut", If(DTDimShowPnputilOut, 1, 0))
            settingsData("PEPolicy").AddKey("WDSHCConnAttempts", WDSHCConnAttempts)
            settingsData("PEPolicy").AddKey("PartTableOverridePreference", PartTableOverridePreference)
            settingsData("PEPolicy").AddKey("UEFICA23Preference", UEFICA23Preference)
            settingsData("PEPolicy").AddKey("AutoUnattendCopytoSysprep", If(AutoUnattendCopytoSysprep, 1, 0))
            settingsData("PEPolicy").AddKey("PXEServerPort", PXEServerPort)
            settingsData("PEPolicy").AddKey("KeyboardLayoutCode", Quote & KeyboardLayoutCode & Quote)
            settingsData("PEPolicy").AddKey("KeyboardLayoutOverrideExistingLayout", If(KeyboardLayoutOverrideExistingLayout, 1, 0))
            settingsData("PEPolicy").AddKey("AnswerFileConflictResponse", AnswerFileConflictResponse)
            parser.WriteFile(Path.Combine(Application.StartupPath, "settings.ini"), settingsData, UTF8)
        Else
            DynaLog.LogMessage("Attempting to write to registry...")
            Try
                ' Tell settings file to use this method
                DynaLog.LogMessage("Forcing save to registry in INI File...")
                Dim SettingRtb As New RichTextBox() With {
                    .Text = File.ReadAllText(Application.StartupPath & "\settings.ini", UTF8)
                }
                SettingRtb.Text = SettingRtb.Text.Replace("SaveOnSettingsIni=1", "SaveOnSettingsIni=0").Replace("SaveOnSettingsIni = 1", "SaveOnSettingsIni = 0").Trim()
                File.WriteAllText(Application.StartupPath & "\settings.ini", SettingRtb.Text, ASCII)
                DynaLog.LogMessage("Setting key values...")
                Dim KeyStr As String = "Software\DISMTools\" & If(dtBranch.Contains("pre"), "Preview", "Stable")
                DynaLog.LogMessage("Destination path in registry: HKCU\" & KeyStr)
                Dim Key As RegistryKey = Registry.CurrentUser.CreateSubKey(KeyStr)
                DynaLog.LogMessage("Configuring program settings...")
                Dim PrgKey As RegistryKey = Key.CreateSubKey("Program")
                PrgKey.SetValue("DismExe", Quote & DismExe & Quote, RegistryValueKind.ExpandString)
                PrgKey.SetValue("SaveOnSettingsIni", If(SaveOnSettingsIni, 1, 0), RegistryValueKind.DWord)
                PrgKey.Close()
                DynaLog.LogMessage("Configuring personalization settings...")
                Dim PersKey As RegistryKey = Key.CreateSubKey("Personalization")
                PersKey.SetValue("ColorMode", ColorMode, RegistryValueKind.DWord)
                PersKey.SetValue("ColorTheme_Light", LightThemeIndex, RegistryValueKind.DWord)
                PersKey.SetValue("ColorTheme_Dark", DarkThemeIndex, RegistryValueKind.DWord)
                LanguageCode = LocalizationService.NormalizeCultureCode(LanguageCode)
                PersKey.SetValue("LanguageCode", LanguageCode, RegistryValueKind.String)
                PersKey.SetValue("LogFont", LogFont, RegistryValueKind.String)
                PersKey.SetValue("LogFontSi", LogFontSize, RegistryValueKind.DWord)
                PersKey.SetValue("LogFontBold", If(LogFontIsBold, 1, 0), RegistryValueKind.DWord)
                PersKey.SetValue("SecondaryProgressPanelStyle", ProgressPanelStyle, RegistryValueKind.DWord)
                PersKey.SetValue("AllCaps", If(AllCaps, 1, 0), RegistryValueKind.DWord)
                PersKey.SetValue("ExpandedProgressPanel", If(ExpandedProgressPanel, 1, 0), RegistryValueKind.DWord)
                PersKey.SetValue("ShowDateAndTime", If(ShowDateAndTime, 1, 0), RegistryValueKind.DWord)
                PersKey.Close()
                DynaLog.LogMessage("Configuring log settings...")
                Dim LogKey As RegistryKey = Key.CreateSubKey("Logs")
                LogKey.SetValue("LogFile", LogFile, RegistryValueKind.ExpandString)
                LogKey.SetValue("LogLevel", LogLevel, RegistryValueKind.DWord)
                LogKey.SetValue("AutoLogs", If(AutoLogs, 1, 0), RegistryValueKind.DWord)
                LogKey.SetValue("SystemEditor", SystemEditor, RegistryValueKind.ExpandString)
                LogKey.SetValue("EnableDynaLog", If(EnableDynaLog, 1, 0), RegistryValueKind.DWord)
                LogKey.Close()
                DynaLog.LogMessage("Configuring image operation settings...")
                Dim ImgOpKey As RegistryKey = Key.CreateSubKey("ImgOps")
                ImgOpKey.SetValue("Quiet", If(QuietOperations, 1, 0), RegistryValueKind.DWord)
                ImgOpKey.SetValue("NoRestart", If(SysNoRestart, 1, 0), RegistryValueKind.DWord)
                ImgOpKey.SetValue("NoNTSamMappings", If(NoNTSamMappings, 1, 0), RegistryValueKind.DWord)
                ImgOpKey.SetValue("PEHelper.UnattendedFile", PEHelper_UnattendedFile, RegistryValueKind.String)
                ImgOpKey.SetValue("PEHelper.CopyToVentoy", PEHelper_CopyToVentoy, RegistryValueKind.DWord)
                ImgOpKey.SetValue("PEHelper.Use2023EFI", PEHelper_Use2023EFI, RegistryValueKind.DWord)
                ImgOpKey.SetValue("PEHelper.IncludeSysDrvs", PEHelper_IncludeSysDrvs, RegistryValueKind.DWord)
                ImgOpKey.SetValue("AppxRemovalDisplayNameFormat", AppxDisplayNameFormatOnRemoval, RegistryValueKind.DWord)
                ImgOpKey.SetValue("PreventSystemFromSleeping", PreventSystemFromSleeping, RegistryValueKind.DWord)
                ImgOpKey.SetValue("HumanizeDates", HumanizeDates, RegistryValueKind.DWord)
                ImgOpKey.Close()
                DynaLog.LogMessage("Configuring scratch directory settings...")
                Dim ScrDirKey As RegistryKey = Key.CreateSubKey("ScratchDir")
                ScrDirKey.SetValue("UseScratch", If(UseScratch, 1, 0), RegistryValueKind.DWord)
                ScrDirKey.SetValue("AutoScratch", If(AutoScrDir, 1, 0), RegistryValueKind.DWord)
                ScrDirKey.SetValue("ScratchDirLocation", ScratchDir, RegistryValueKind.ExpandString)
                ScrDirKey.Close()
                DynaLog.LogMessage("Configuring output settings...")
                Dim OutKey As RegistryKey = Key.CreateSubKey("Output")
                OutKey.SetValue("EnglishOutput", If(EnglishOutput, 1, 0), RegistryValueKind.DWord)
                OutKey.SetValue("ReportView", ReportView, RegistryValueKind.DWord)
                OutKey.Close()
                DynaLog.LogMessage("Configuring background process settings...")
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
                DynaLog.LogMessage("Configuring startup settings...")
                Dim StartupKey As RegistryKey = Key.CreateSubKey("Startup")
                StartupKey.SetValue("RemountImages", If(StartupRemount, 1, 0), RegistryValueKind.DWord)
                StartupKey.SetValue("CheckForUpdates", If(StartupUpdateCheck, 1, 0), RegistryValueKind.DWord)
                StartupKey.Close()
                DynaLog.LogMessage("Configuring shutdown settings...")
                Dim ShutdownKey As RegistryKey = Key.CreateSubKey("Shutdown")
                ShutdownKey.SetValue("AutoCleanMounts", If(AutoCleanMounts, 1, 0), RegistryValueKind.DWord)
                ShutdownKey.Close()
                DynaLog.LogMessage("Configuring window parameters...")
                Dim WndKey As RegistryKey = Key.CreateSubKey("WndParams")
                WndKey.SetValue("WndWidth", Math.Round(WndWidth / (WindowHelper.GetSystemDpi() / 100), 0), RegistryValueKind.DWord)
                WndKey.SetValue("WndHeight", Math.Round(WndHeight / (WindowHelper.GetSystemDpi() / 100), 0), RegistryValueKind.DWord)
                If Location = New Point((Screen.FromControl(Me).WorkingArea.Width - Width) / 2, (Screen.FromControl(Me).WorkingArea.Height - Height) / 2) Then
                    WndKey.SetValue("WndCenter", 1, RegistryValueKind.DWord)
                Else
                    WndKey.SetValue("WndCenter", 0, RegistryValueKind.DWord)
                End If
                WndKey.SetValue("WndLeft", WndLeft, RegistryValueKind.DWord)
                WndKey.SetValue("WndTop", WndTop, RegistryValueKind.DWord)
                WndKey.SetValue("WndMaximized", If(WindowState = FormWindowState.Maximized, 1, 0), RegistryValueKind.DWord)
                WndKey.Close()
                DynaLog.LogMessage("Configuring Information Saver settings...")
                Dim InfoSaverKey As RegistryKey = Key.CreateSubKey("InfoSaver")
                InfoSaverKey.SetValue("SkipQuestions", If(SkipQuestions, 1, 0), RegistryValueKind.DWord)
                InfoSaverKey.SetValue("Pkg_CompleteInfo", If(AutoCompleteInfo(0), 1, 0), RegistryValueKind.DWord)
                InfoSaverKey.SetValue("Feat_CompleteInfo", If(AutoCompleteInfo(1), 1, 0), RegistryValueKind.DWord)
                InfoSaverKey.SetValue("AppX_CompleteInfo", If(AutoCompleteInfo(2), 1, 0), RegistryValueKind.DWord)
                InfoSaverKey.SetValue("Cap_CompleteInfo", If(AutoCompleteInfo(3), 1, 0), RegistryValueKind.DWord)
                InfoSaverKey.SetValue("Drv_CompleteInfo", If(AutoCompleteInfo(4), 1, 0), RegistryValueKind.DWord)
                InfoSaverKey.Close()
                Dim SearchKey As RegistryKey = Key.CreateSubKey("SearchSettings")
                SearchKey.SetValue("EngineName", SearchEngineName, RegistryValueKind.String)
                SearchKey.SetValue("AITolerance", SearchEngineAITolerance, RegistryValueKind.DWord)
                SearchKey.Close()
                Dim PEPolicyKey As RegistryKey = Key.CreateSubKey("PEPolicy")
                PEPolicyKey.SetValue("ShowWatermark", If(ShowWatermark, 1, 0), RegistryValueKind.DWord)
                PEPolicyKey.SetValue("WDSHCGraphoView", If(WDSHCGraphoView, 1, 0), RegistryValueKind.DWord)
                PEPolicyKey.SetValue("DTDimShowPnputilOut", If(DTDimShowPnputilOut, 1, 0), RegistryValueKind.DWord)
                PEPolicyKey.SetValue("WDSHCConnAttempts", WDSHCConnAttempts, RegistryValueKind.DWord)
                PEPolicyKey.SetValue("PartTableOverridePreference", PartTableOverridePreference, RegistryValueKind.DWord)
                PEPolicyKey.SetValue("UEFICA23Preference", UEFICA23Preference, RegistryValueKind.DWord)
                PEPolicyKey.SetValue("AutoUnattendCopytoSysprep", AutoUnattendCopytoSysprep, RegistryValueKind.DWord)
                PEPolicyKey.SetValue("PXEServerPort", PXEServerPort, RegistryValueKind.DWord)
                PEPolicyKey.SetValue("KeyboardLayoutCode", KeyboardLayoutCode, RegistryValueKind.String)
                PEPolicyKey.SetValue("KeyboardLayoutOverrideExistingLayout", KeyboardLayoutOverrideExistingLayout, RegistryValueKind.DWord)
                PEPolicyKey.SetValue("AnswerFileConflictResponse", AnswerFileConflictResponse, RegistryValueKind.DWord)
                PEPolicyKey.Close()
                Key.Close()
            Catch ex As Exception
                DynaLog.LogMessage("An error occurred while saving settings to registry. Error message: " & ex.Message)
                DynaLog.LogMessage("Forcing save to INI...")
                ' Fallback to INI method and force save
                SaveOnSettingsIni = True
                SaveDTSettings()
            End Try
        End If

    End Sub

    Sub ResetDTSettings()
        DynaLog.LogMessage("Regenerating DISMTools Settings...")
        GenerateDTSettings()
        DynaLog.LogMessage("Loading Settings...")
        LoadDTSettings(If(SaveOnSettingsIni, 1, 0))
    End Sub

    ''' <summary>
    ''' Detects properties of the file to determine whether automatic migration needs to be performed
    ''' </summary>
    ''' <remarks>If the file does not exist, the initial setup wizard launches</remarks>
    Sub PerformSettingFileValidation()
        DynaLog.LogMessage("Checking age of settings file... (Exiting from procedure if migration is disabled)")
        If File.Exists(Application.StartupPath & "\settings.ini") Then
            If NoMigration Then Exit Sub
            DynaLog.LogMessage("Checking build date...")
            Dim bldDate As Date = RetrieveLinkerTimestamp()
            DynaLog.LogMessage("Comparing dates...")
            If File.GetLastWriteTime(Application.StartupPath & "\settings.ini") < bldDate Then
                DynaLog.LogMessage("Settings file was last modified at a date older than build date. Migrating settings...")
                ' Perform setting file migration
                MigrationForm.ShowDialog()
            End If
        Else
            DynaLog.LogMessage("Settings file not found. Launching Initial Setup Wizard (ISW)...")
            ' Show setup window
            SplashScreen.Hide()
            PrgSetup.ShowDialog()
        End If
    End Sub

    Sub ChangeMenuItemColors(bgColor As Color, fgColor As Color, itemCollection As ToolStripItemCollection)
        For Each tsi As ToolStripItem In itemCollection
            If TypeOf tsi Is ToolStripDropDownItem Then
                Dim item As ToolStripDropDownItem = CType(tsi, ToolStripDropDownItem)
                Try
                    item.DropDown.BackColor = bgColor
                    item.DropDown.ForeColor = fgColor
                    If item.DropDownItems.Count > 0 Then
                        DynaLog.LogMessage("Item has " & item.DropDownItems.Count & " drop-down item(s). Changing colors of submenus...")
                        ChangeMenuItemColors(bgColor, fgColor, item.DropDownItems)
                    End If
                Catch ex As Exception
                    DynaLog.LogMessage("Item " & tsi.Name & " of type " & tsi.GetType().ToString() & " could not be modified")
                    Continue For
                End Try
            End If
        Next
    End Sub

    ''' <summary>
    ''' Change program colors accordingly. Due to developer's preference, match those of VS2012
    ''' </summary>
    ''' <param name="ColorCode"></param>
    ''' <remarks></remarks>
    Sub ChangePrgColors(ColorCode As Integer)
        DynaLog.LogMessage("Changing program colors. Color Code: " & ColorCode)
        Select Case ColorCode
            Case 0
                DynaLog.LogMessage("Color Code is 0. Attempting to get values from host system...")
                Try
                    Dim ColorModeRk As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", False)
                    Dim ColorMode As String = ColorModeRk.GetValue("AppsUseLightTheme").ToString()
                    ColorModeRk.Close()
                    DynaLog.LogMessage("Color Mode is " & CInt(ColorMode) & ". Changing accordingly...")
                    If ColorMode = "0" Then
                        WindowHelper.ToggleDarkTitleBar(Handle, True)
                        ThemeHelper.ChangeCurrentTheme(DarkThemeIndex, True)
                    ElseIf ColorMode = "1" Then
                        WindowHelper.ToggleDarkTitleBar(Handle, False)
                        ThemeHelper.ChangeCurrentTheme(LightThemeIndex, False)
                    End If
                Catch ex As Exception
                    ChangePrgColors(1)
                    Exit Sub
                End Try
            Case 1
                DynaLog.LogMessage("Color Code is 1. Switching to light theme...")
                WindowHelper.ToggleDarkTitleBar(Handle, False)
                ThemeHelper.ChangeCurrentTheme(LightThemeIndex, False)
            Case 2
                DynaLog.LogMessage("Color Code is 1. Switching to dark theme...")
                WindowHelper.ToggleDarkTitleBar(Handle, True)
                ThemeHelper.ChangeCurrentTheme(DarkThemeIndex, True)
        End Select
        BackColor = CurrentTheme.BackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        HomePanel.BackColor = CurrentTheme.BackgroundColor
        HomePanel.ForeColor = CurrentTheme.ForegroundColor
        SidePanel.BackColor = CurrentTheme.SectionBackgroundColor
        SidePanel.ForeColor = CurrentTheme.ForegroundColor
        WelcomePanel.BackColor = CurrentTheme.BackgroundColor
        WelcomePanel.ForeColor = CurrentTheme.ForegroundColor
        PrjPanel.BackColor = CurrentTheme.BackgroundColor
        PrjPanel.ForeColor = CurrentTheme.ForegroundColor
        MenuStrip1.BackColor = CurrentTheme.SectionBackgroundColor
        MenuStrip1.ForeColor = CurrentTheme.ForegroundColor
        ChangeMenuItemColors(CurrentTheme.SectionBackgroundColor, CurrentTheme.ForegroundColor, MenuStrip1.Items)
        PictureBox5.Image = GetGlyphResource("logo_aboutdlg")
        ToolStrip1.BackColor = CurrentTheme.SectionBackgroundColor
        ToolStrip1.ForeColor = CurrentTheme.ForegroundColor
        ToolStrip2.BackColor = CurrentTheme.SectionBackgroundColor
        ToolStrip2.ForeColor = CurrentTheme.ForegroundColor
        prjTreeView.BackColor = CurrentTheme.SectionBackgroundColor
        prjTreeView.ForeColor = CurrentTheme.ForegroundColor
        ToolStripButton2.Image = GetGlyphResource("save_glyph")
        ToolStripButton3.Image = GetGlyphResource("prj_unload_glyph")
        ToolStripButton4.Image = GetGlyphResource("progress_window")
        RefreshViewTSB.Image = GetGlyphResource("refresh_glyph")
        MenuToggle.Image = GetGlyphResource("menu")
        Try
            If prjTreeView.SelectedNode.IsExpanded Then
                ExpandCollapseTSB.Image = GetGlyphResource("collapse_glyph")
            Else
                ExpandCollapseTSB.Image = GetGlyphResource("expand_glyph")
            End If
        Catch ex As Exception
            ExpandCollapseTSB.Image = GetGlyphResource("expand_glyph")
        End Try
        MenuStrip1.RenderMode = ToolStripRenderMode.Professional
        MenuStrip1.Renderer = GetProfessionalRenderer()
        ToolStrip1.Renderer = GetProfessionalRenderer()
        ToolStrip2.Renderer = GetProfessionalRenderer()
        PkgInfoCMS.Renderer = GetProfessionalRenderer()
        ImgUMountPopupCMS.Renderer = GetProfessionalRenderer()
        AppxPackagePopupCMS.Renderer = GetProfessionalRenderer()
        AppxRelatedLinksCMS.Renderer = GetProfessionalRenderer()
        TreeViewCMS.Renderer = GetProfessionalRenderer()
        AppxResCMS.Renderer = GetProfessionalRenderer()
        ImgSpecialToolsCMS.Renderer = GetProfessionalRenderer()
        ImgApplyModeCMS.Renderer = GetProfessionalRenderer()
        ImgCaptureModeCMS.Renderer = GetProfessionalRenderer()
        PkgInfoCMS.ForeColor = CurrentTheme.ForegroundColor
        ImgUMountPopupCMS.ForeColor = CurrentTheme.ForegroundColor
        AppxPackagePopupCMS.ForeColor = CurrentTheme.ForegroundColor
        AppxRelatedLinksCMS.ForeColor = CurrentTheme.ForegroundColor
        TreeViewCMS.ForeColor = CurrentTheme.ForegroundColor
        AppxResCMS.ForeColor = CurrentTheme.ForegroundColor
        ImgSpecialToolsCMS.ForeColor = CurrentTheme.ForegroundColor
        ImgApplyModeCMS.ForeColor = CurrentTheme.ForegroundColor
        ImgCaptureModeCMS.ForeColor = CurrentTheme.ForegroundColor
        ChangeMenuItemColors(CurrentTheme.SectionBackgroundColor, CurrentTheme.ForegroundColor, TreeViewCMS.Items)
        InvalidSettingsTSMI.Image = GetGlyphResource("setting_error_glyph")
        ExitFullScreenTSMI.Image = GetGlyphResource("exit_full_screen_glyph")
        BranchTSMI.Image = GetGlyphResource("branch")
        TourActionsTSMI.Image = GetGlyphResource("tour_glyph")
        ' New design stuff
        FlowLayoutPanel1.BackColor = CurrentTheme.BackgroundColor
        GroupBox4.ForeColor = CurrentTheme.ForegroundColor
        GroupBox5.ForeColor = CurrentTheme.ForegroundColor
        GroupBox6.ForeColor = CurrentTheme.ForegroundColor
        GroupBox7.ForeColor = CurrentTheme.ForegroundColor
        GroupBox8.ForeColor = CurrentTheme.ForegroundColor
        GroupBox9.ForeColor = CurrentTheme.ForegroundColor
        GroupBox10.ForeColor = CurrentTheme.ForegroundColor
        ListView2.BackColor = CurrentTheme.BackgroundColor
        ListView2.ForeColor = CurrentTheme.ForegroundColor
        RecentsLV.BackColor = SidePanel.BackColor
        ' New project view header and side panel tints
        ProjectViewHeader.BackColor = CurrentTheme.AccentColors(0)
        ProjectSidePanel.BackColor = CurrentTheme.AccentColors(0)
        If SidePanel_ProjectView.Visible Then LinkLabel12.LinkColor = CurrentTheme.ForegroundColor
        If SidePanel_ImageView.Visible Then LinkLabel13.LinkColor = CurrentTheme.ForegroundColor
        PictureBox9.Image = GetGlyphResource("info_glyph")
        PictureBox10.Image = GetGlyphResource("explorer_view_glyph")
        PictureBox11.Image = GetGlyphResource("prj_unload_glyph")
        PictureBox14.Image = GetGlyphResource("info_glyph")
        PictureBox15.Image = GetGlyphResource("openfile")
        ProjectViewHeader.ForeColor = ForeColor
        ProjectSidePanel.ForeColor = ForeColor
        StatusStrip.BackColor = CurrentTheme.AccentColors(1)
        StatusStrip.ForeColor = CurrentTheme.ForegroundColor
        If ImgBW.IsBusy Then
            BackgroundProcessesButton.Image = GetGlyphResource("bg_ops")
        Else
            BackgroundProcessesButton.Image = GetGlyphResource("bg_ops_complete")
        End If
        ' Infinity Home
        ComputerInfoPanel.BackColor = CurrentTheme.SectionBackgroundColor

        ' Set link label link controls
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ThemeHelper.UpdateLinkLabelColors(ImgTasks, ForeColor, CurrentTheme.AccentColors(1))
        ThemeHelper.UpdateLinkLabelColors(PrjTasks, ForeColor, CurrentTheme.AccentColors(1))
        ThemeHelper.UpdateLinkLabelColors(TableLayoutPanel7, ForeColor, CurrentTheme.AccentColors(1))
        ThemeHelper.UpdateLinkLabelColors(TableLayoutPanel4, ForeColor, CurrentTheme.AccentColors(1))

        For Each NewsCard As NewsFeedItemCard In NewsItemCardContainerPanel.Controls.OfType(Of NewsFeedItemCard)()
            NewsCard.SetColors()
        Next
    End Sub

    Sub ApplyLanguage(cultureCode As String)
        Dim requestedCultureCode As String = LocalizationService.NormalizeCultureCode(cultureCode)
        Dim validationMessage As String = ""
        If Not LocalizationService.ValidateLanguage(requestedCultureCode, validationMessage) Then
            DynaLog.LogMessage("The requested language file failed validation. Keeping the default language.")
            MessageBox.Show(validationMessage,
                            "Incompatible or invalid DISMTools language file",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            requestedCultureCode = LocalizationService.DefaultCultureCode
        End If

        LanguageCode = requestedCultureCode
        LocalizationService.SetLanguageByCultureCode(LanguageCode)
        DynaLog.LogMessage("Changing program language... (culture code: " & LanguageCode & ")")
        DynaLog.LogMessage("Language culture is " & LanguageCode & ". Applying localization resources...")
        FileToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface").Upper("File.Label", AllCaps)
        ProjectToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface").Upper("Project.Label", AllCaps)
        CommandsToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface").Upper("Commands.Label", AllCaps)
        ToolsToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface").Upper("Tools.Label", AllCaps)
        HelpToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface").Upper("Help.Label", AllCaps)
        InvalidSettingsTSMI.Text = LocalizationService.ForSection("Main.Interface")("Settings.Detected.Label")
        NewProjectToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("NewProject.Button")
        OpenExistingProjectToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Open.Existing.Project.Label")
        ManageOnlineInstallationToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Manage.Online.Install.Label")
        ManageOfflineInstallationToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Manage.Ffline.Button")
        RecentProjectsListMenu.Text = LocalizationService.ForSection("Main.Interface")("RecentProjects.Label")
        SaveProjectToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("SaveProject.Button")
        SaveProjectasToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("SaveProjectas.Button")
        ExitToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Exit.Label")
        ViewProjectFilesInFileExplorerToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("View.Project.Files.Label")
        UnloadProjectToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("UnloadProject.Button")
        SwitchImageIndexesToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Switch.Image.Indexes.Button")
        ProjectPropertiesToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("ProjectProps.Label")
        ImagePropertiesToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("ImageProps.Label")
        ImageManagementToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("ImageManagement.Label")
        OSPackagesToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("OSPackages.Label")
        ProvisioningPackagesToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("ProvPackages.Label")
        AppPackagesToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("AppxPackages.Label")
        AppPatchesToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("AppMspservicing.Label")
        DefaultAppAssociationsToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("DefaultApp.Assoc.Label")
        LanguagesAndRegionSettingsToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Languages.Regional.Label")
        CapabilitiesToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Capabilities.Label")
        WindowsEditionsToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Windows.Label")
        DriversToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Drivers.Label")
        UnattendedAnswerFilesToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Unattended.Answer.Label")
        WindowsPEServicingToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("WindowsPE.Label")
        OSUninstallToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("OSUninstall.Label")
        ReservedStorageToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("ReservedStorage.Label")
        AppendImage.Text = LocalizationService.ForSection("Main.Interface")("Append.Capture.Dir.Button")
        ApplyFFU.Text = LocalizationService.ForSection("Main.Interface")("ApplyFfusfufile.Button")
        ApplyImage.Text = LocalizationService.ForSection("Main.Interface")("ApplyWimswmfile.Button")
        CaptureCustomImage.Text = LocalizationService.ForSection("Main.Interface")("Capture.Incremental.Button")
        CaptureFFU.Text = LocalizationService.ForSection("Main.Interface")("Capture.Partitions.Button")
        CaptureImage.Text = LocalizationService.ForSection("Main.Interface")("Capture.Image.Drive.Button")
        CleanupMountpoints.Text = LocalizationService.ForSection("Main.Interface")("Delete.Resources.Button")
        CommitImage.Text = LocalizationService.ForSection("Main.Interface")("Apply.Changes.Image.Button")
        DeleteImage.Text = LocalizationService.ForSection("Main.Interface")("Delete.VolumeImages.Button")
        ExportImage.Text = LocalizationService.ForSection("Main.Interface")("ExportImage.Button")
        GetImageInfo.Text = LocalizationService.ForSection("Main.Interface")("Get.Image.Button")
        GetWIMBootEntry.Text = LocalizationService.ForSection("Main.Interface")("Get.WIM.Boot.Button")
        ListImage.Text = LocalizationService.ForSection("Main.Interface")("List.Files.Dirs.Button")
        MountImage.Text = LocalizationService.ForSection("Main.Interface")("MountImage.Button")
        OptimizeFFU.Text = LocalizationService.ForSection("Main.Interface")("Optimize.FFU.File.Button")
        OptimizeImage.Text = LocalizationService.ForSection("Main.Interface")("OptimizeImage.Button")
        RemountImage.Text = LocalizationService.ForSection("Main.Interface")("Remount.Image.Button")
        SplitFFU.Text = LocalizationService.ForSection("Main.Interface")("Split.FFU.File.Button")
        SplitImage.Text = LocalizationService.ForSection("Main.Interface")("Split.WIM.File.Button")
        UnmountImage.Text = LocalizationService.ForSection("Main.Interface")("UnmountImage.Button")
        UpdateWIMBootEntry.Text = LocalizationService.ForSection("Main.Interface")("Update.WIM.Boot.Button")
        ApplySiloedPackage.Text = LocalizationService.ForSection("Main.Interface")("Apply.Siloed.Prov.Button")
        SaveImageInformationToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Save.Image.Button")
        GetPackages.Text = LocalizationService.ForSection("Main.Interface")("GetPackages.Button")
        AddPackage.Text = LocalizationService.ForSection("Main.Interface")("AddPackage.Button")
        RemovePackage.Text = LocalizationService.ForSection("Main.Interface")("RemovePackage.Button")
        GetFeatures.Text = LocalizationService.ForSection("Main.Interface")("GetFeatures.Button")
        EnableFeature.Text = LocalizationService.ForSection("Main.Interface")("EnableFeature.Button")
        DisableFeature.Text = LocalizationService.ForSection("Main.Interface")("DisableFeature.Button")
        CleanupImage.Text = LocalizationService.ForSection("Main.Interface")("CleanupRecovery.Button")
        AddProvisioningPackage.Text = LocalizationService.ForSection("Main.Interface")("Add.Prov.Package.Button")
        GetProvisioningPackageInfo.Text = LocalizationService.ForSection("Main.Interface")("Get.Prov.Package.Button")
        ApplyCustomDataImage.Text = LocalizationService.ForSection("Main.Interface")("Apply.CustomData.Button")
        GetProvisionedAppxPackages.Text = LocalizationService.ForSection("Main.Interface")("Get.App.Package.Button")
        AddProvisionedAppxPackage.Text = LocalizationService.ForSection("Main.Interface")("Add.Provisioned.App.Button")
        RemoveProvisionedAppxPackage.Text = LocalizationService.ForSection("Main.Interface")("Remove.Prov.App.Button")
        OptimizeProvisionedAppxPackages.Text = LocalizationService.ForSection("Main.Interface")("Optimize.Provisioned.Button")
        SetProvisionedAppxDataFile.Text = LocalizationService.ForSection("Main.Interface")("Add.CustomData.File.Button")
        CheckAppPatch.Text = LocalizationService.ForSection("Main.Interface")("Get.App.Patch.Button")
        GetAppPatchInfo.Text = LocalizationService.ForSection("Main.Interface")("Detailed.App.Patch.Button")
        GetAppPatches.Text = LocalizationService.ForSection("Main.Interface")("Basic.Installed.App.Button")
        GetAppInfo.Text = LocalizationService.ForSection("Main.Interface")("Get.Detailed.Button")
        GetApps.Text = LocalizationService.ForSection("Main.Interface")("Get.Basic.Windows.Button")
        ExportDefaultAppAssociations.Text = LocalizationService.ForSection("Main.Interface")("Export.Default.Button")
        GetDefaultAppAssociations.Text = LocalizationService.ForSection("Main.Interface")("DefaultApp.Assoc.Button")
        ImportDefaultAppAssociations.Text = LocalizationService.ForSection("Main.Interface")("Import.Default.Button")
        RemoveDefaultAppAssociations.Text = LocalizationService.ForSection("Main.Interface")("Remove.Default.Button")
        GetIntl.Text = LocalizationService.ForSection("Main.Interface")("Intl.Settings.Button")
        SetUILang.Text = LocalizationService.ForSection("Main.Interface")("SetUilanguage.Button")
        SetUILangFallback.Text = LocalizationService.ForSection("Main.Interface")("Set.Default.Button")
        SetSysUILang.Text = LocalizationService.ForSection("Main.Interface")("Set.System.Preferred.Button")
        SetSysLocale.Text = LocalizationService.ForSection("Main.Interface")("Set.System.Locale.Button")
        SetUserLocale.Text = LocalizationService.ForSection("Main.Interface")("Set.User.Locale.Button")
        SetInputLocale.Text = LocalizationService.ForSection("Main.Interface")("Set.Input.Locale.Button")
        SetAllIntl.Text = LocalizationService.ForSection("Main.Interface")("Set.UI.Button")
        SetTimeZone.Text = LocalizationService.ForSection("Main.Interface")("Set.Default.Time.Button")
        SetSKUIntlDefaults.Text = LocalizationService.ForSection("Main.Interface")("Set.Default.Languages.Button")
        SetLayeredDriver.Text = LocalizationService.ForSection("Main.Interface")("Set.Layered.Driver.Button")
        GenLangINI.Text = LocalizationService.ForSection("Main.Interface")("Generate.Lang.Ini.Button")
        SetSetupUILang.Text = LocalizationService.ForSection("Main.Interface")("Set.Default.Setup.Button")
        AddCapability.Text = LocalizationService.ForSection("Main.Interface")("AddCapability.Button")
        ExportSource.Text = LocalizationService.ForSection("Main.Interface")("Export.Capabilities.Button")
        GetCapabilities.Text = LocalizationService.ForSection("Main.Interface")("GetCapabilities.Button")
        RemoveCapability.Text = LocalizationService.ForSection("Main.Interface")("RemoveCapability.Button")
        GetCurrentEdition.Text = LocalizationService.ForSection("Main.Interface")("Get.Edition.Button")
        GetTargetEditions.Text = LocalizationService.ForSection("Main.Interface")("Get.Upgrade.Targets.Button")
        SetEdition.Text = LocalizationService.ForSection("Main.Interface")("UpgradeImage.Button")
        SetProductKey.Text = LocalizationService.ForSection("Main.Interface")("SetProductKey.Button")
        GetDrivers.Text = LocalizationService.ForSection("Main.Interface")("GetDrivers.Button")
        AddDriver.Text = LocalizationService.ForSection("Main.Interface")("AddDriver.Button")
        RemoveDriver.Text = LocalizationService.ForSection("Main.Interface")("RemoveDriver.Button")
        ExportDriver.Text = LocalizationService.ForSection("Main.Interface")("Export.DriverPackages.Button")
        ImportDriver.Text = LocalizationService.ForSection("Main.Interface")("Import.DriverPackages.Button")
        ApplyUnattend.Text = LocalizationService.ForSection("Main.Interface")("Apply.Unattended.Button")
        GetPESettings.Text = LocalizationService.ForSection("Main.Interface")("GetSettings.Button")
        SetScratchSpace.Text = LocalizationService.ForSection("Main.Interface")("SetScratchSpace.Button")
        SetTargetPath.Text = LocalizationService.ForSection("Main.Interface")("Set.Target.Path.Button")
        GetOSUninstallWindow.Text = LocalizationService.ForSection("Main.Interface")("Get.Uninstall.Window.Button")
        InitiateOSUninstall.Text = LocalizationService.ForSection("Main.Interface")("Initiate.Uninstall.Button")
        RemoveOSUninstall.Text = LocalizationService.ForSection("Main.Interface")("Remove.Roll.Back.Button")
        SetOSUninstallWindow.Text = LocalizationService.ForSection("Main.Interface")("Set.Uninstall.Window.Button")
        SetReservedStorageState.Text = LocalizationService.ForSection("Main.Interface")("Set.Reserved.Storage.Button")
        GetReservedStorageState.Text = LocalizationService.ForSection("Main.Interface")("Get.Reserved.Storage.Button")
        AddEdge.Text = LocalizationService.ForSection("Main.Interface")("AddEdge.Button")
        AddEdgeBrowser.Text = LocalizationService.ForSection("Main.Interface")("Add.Edge.Browser.Button")
        AddEdgeWebView.Text = LocalizationService.ForSection("Main.Interface")("Add.Edge.Web.Button")
        ImageConversionToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("ImageConversion.Label")
        MergeSWM.Text = LocalizationService.ForSection("Main.Interface")("MergeSwmfiles.Button")
        RemountImageWithWritePermissionsToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Remount.Image.Write.Label")
        CommandShellToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("CommandConsole.Label")
        UnattendedAnswerFileManagerToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Unattended.AnswerFile.Label")
        UnattendedAnswerFileCreatorToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Unattended.Creator.Label")
        RegCplToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Manage.Image.Registry.Button")
        WebResourcesToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("WebResources.Label")
        LanguagesAndOptionalFeaturesISOToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Download.Languages.Button")
        LanguagesAndFODWin10ToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Download.FOD.Button")
        ReportManagerToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("ReportManager.Label")
        MountedImageManagerTSMI.Text = LocalizationService.ForSection("Main.Interface")("Mounted.Image.Manager.Label")
        CreateDiscImageToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Create.Disc.Image.Button")
        CreateTestingEnvironmentToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Create.Testing.Button")
        WimScriptEditorCommand.Text = LocalizationService.ForSection("Main.Interface")("Config.List.Editor.Label")
        OptionsToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Options.Label")
        HelpTopicsToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("HelpTopics.Label")
        AboutDISMToolsToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("DISM.Tools.Label")
        ISFix.Text = LocalizationService.ForSection("Main.Interface")("MoreInfo.Label")
        ISHelp.Text = LocalizationService.ForSection("Main.Interface")("WhatsThis.Label")
        ReportFeedbackToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Report.Feedback.Opens.Label")
        ContributeToTheHelpSystemToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Contribute.Help.System.Label")
        TourActionsTSMI.Text = LocalizationService.ForSection("Main.Interface")("TourActions.Label")
        ServerStatusTSMI.Text = LocalizationService.ForSection("Main.Interface").Format("Tour.Server.Active.Label", tourServer.GetTcpPort())
        RestartDTTourTSMI.Text = LocalizationService.ForSection("Main.Interface")("RestartTour.Label")
        StopDTTourServerTSMI.Text = LocalizationService.ForSection("Main.Interface")("Stop.Tour.Server.Label")
        LabelHeader1.Text = LocalizationService.ForSection("Main.Interface")("Begin.Label")
        Label10.Text = LocalizationService.ForSection("Main.Interface")("RecentProjects.Label")
        NewProjLink.Text = LocalizationService.ForSection("Main.Interface")("NewProject.Link")
        ExistingProjLink.Text = LocalizationService.ForSection("Main.Interface")("Open.Existing.Project.Link")
        OnlineInstMgmt.Text = LocalizationService.ForSection("Main.Interface")("Manage.Online.Install.Link")
        OfflineInstMgmt.Text = LocalizationService.ForSection("Main.Interface")("Manage.Offline.Button.Button")
        RecentRemoveLink.Text = LocalizationService.ForSection("Main.Interface")("RemoveEntry.Link")
        ToolStripButton1.Text = LocalizationService.ForSection("Main.Interface")("CloseTab.Label")
        ToolStripButton2.Text = LocalizationService.ForSection("Main.Interface")("SaveProject.Label")
        ToolStripButton3.Text = LocalizationService.ForSection("Main.Interface")("UnloadProject.Label")
        ToolStripButton3.ToolTipText = LocalizationService.ForSection("Main.Interface")("Unload.Project.Tooltip")
        ToolStripButton4.Text = LocalizationService.ForSection("Main.Interface")("Show.Progress.Window.Label")
        RefreshViewTSB.Text = LocalizationService.ForSection("Main.Interface")("RefreshView.Label")
        ExpandCollapseTSB.Text = LocalizationService.ForSection("Main.Interface")("Expand.Label")
        UpdateLink.Text = LocalizationService.ForSection("Main.Interface")("NewVersion.Available.Link")
        UpdateLink.LinkArea = LocalizationService.GetLinkArea(UpdateLink.Text, LocalizationService.ForSection("Main.CheckForUpdates")("Learn.Link"))
        PkgBasicInfo.Text = LocalizationService.ForSection("Main.Interface")("Get.Basic.Label")
        PkgDetailedInfo.Text = LocalizationService.ForSection("Main.Interface")("Get.Detailed.Specific.Label")
        CommitAndUnmountTSMI.Text = LocalizationService.ForSection("Main.Interface")("CommitImage.Label")
        DiscardAndUnmountTSMI.Text = LocalizationService.ForSection("Main.Interface")("Discard.Changes.Label")
        UnmountSettingsToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("UnmountSettings.Button")
        ViewPackageDirectoryToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("View.Package.Dir.Label")
        GetImageFileInformationToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Get.ImageFile.Button")
        SaveCompleteImageInformationToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Save.Complete.Image.Button")
        CreateDiscImageWithThisFileToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Create.Disc.ImageFile.Button")
        OpenFileDialog1.Title = LocalizationService.ForSection("Main.Interface")("Project.File.Load.Title")
        LocalMountDirFBD.Description = LocalizationService.ForSection("Main.Interface")("MountDir.Description")
        If Not ImgBW.IsBusy And areBackgroundProcessesDone Then
            BGProcDetails.Label2.Text = LocalizationService.ForSection("Main.Interface")("Image.Processes.Label")
        End If
        MenuDesc.Text = LocalizationService.ForSection("Main.Interface")("Ready.Label")
        AccessDirectoryToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("AccessDirectory.Label")
        UnloadProjectToolStripMenuItem1.Text = LocalizationService.ForSection("Main.Interface")("UnloadProject.Label")
        CopyDeploymentToolsToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Copy.Deployment.Tools.Label")
        OfAllArchitecturesToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("AllArchitectures.Label")
        OfSelectedArchitectureToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Selected.Architecture.Label")
        ForX86ArchitectureToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Xarchitecture.Label")
        ForAmd64ArchitectureToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Amarkdown.Architecture.Label")
        ForARMArchitectureToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("ARM.Label")
        ForARM64ArchitectureToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("ARM64.Label")
        ImageOperationsToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("ImageOperations.Label")
        MountImageToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("MountImage.Button")
        UnmountImageToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("UnmountImage.Button")
        RemoveVolumeImagesToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Remove.VolumeImages.Button")
        SwitchImageIndexesToolStripMenuItem1.Text = LocalizationService.ForSection("Main.Interface")("Switch.Image.Indexes.Button")
        UnattendedAnswerFilesToolStripMenuItem1.Text = LocalizationService.ForSection("Main.Interface")("Unattended.Answer.Label")
        ManageToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Manage.Label")
        CreationWizardToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Create.Label")
        ScratchDirectorySettingsToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Configure.Scratch.Dir.Label")
        ManageReportsToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("ManageReports.Label")
        AddToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Add.Button")
        NewFileToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("NewFile.Button")
        ExistingFileToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("ExistingFile.Button")
        SaveResourceToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("SaveResource.Button")
        CopyToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("CopyResource.Label")
        MicrosoftAppsToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Visit.Microsoft.Apps.Label")
        MicrosoftStoreGenerationProjectToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Visit.Microsoft.Label")
        AppxDownloadHelpToolStripMenuItem.Text = LocalizationService.ForSection("Main.Interface")("Iget.Apps.Label")
        GreetingLabel.Text = LocalizationService.ForSection("Main.Interface")("Welcome.Servicing.Label")
        LinkLabel12.Text = LocalizationService.ForSection("Main.Interface")("Project.Link")
        LinkLabel13.Text = LocalizationService.ForSection("Main.Interface")("Image.Link")
        Label54.Text = LocalizationService.ForSection("Main.Interface")("Name.Label")
        Label51.Text = LocalizationService.ForSection("Main.Interface")("Location.Label")
        Label53.Text = LocalizationService.ForSection("Main.Interface")("ImagesMounted.Label")
        LinkLabel14.Text = LocalizationService.ForSection("Main.Interface")("Mount.Image.Link")
        Label55.Text = LocalizationService.ForSection("Main.Interface")("ProjectTasks.Label")
        LinkLabel15.Text = LocalizationService.ForSection("Main.Interface")("View.Project.Props.Link")
        LinkLabel16.Text = LocalizationService.ForSection("Main.Interface")("Open.File.Explorer.Link")
        LinkLabel17.Text = LocalizationService.ForSection("Main.Interface")("UnloadProject.Link")
        Label59.Text = LocalizationService.ForSection("Main.Interface")("ImageMounted.Label")
        Label58.Text = LocalizationService.ForSection("Main.Interface")("Mount.Image.Order.Label")
        Label57.Text = LocalizationService.ForSection("Main.Interface")("Choices.Label")
        LinkLabel21.Text = LocalizationService.ForSection("Main.Interface")("MountImage.Link")
        LinkLabel18.Text = LocalizationService.ForSection("Main.Interface")("Pick.Mounted.Image.Link")
        Label39.Text = LocalizationService.ForSection("Main.Interface")("ImageIndex.Label")
        Label43.Text = LocalizationService.ForSection("Main.Interface")("MountPoint.Label")
        Label45.Text = LocalizationService.ForSection("Main.Interface")("Version.Label")
        Label42.Text = LocalizationService.ForSection("Main.Interface")("Name.Label")
        Label40.Text = LocalizationService.ForSection("Main.Interface")("Description.Label")
        Label56.Text = LocalizationService.ForSection("Main.Interface")("ImageTasks.Label")
        LinkLabel20.Text = LocalizationService.ForSection("Main.Interface")("View.Image.Props.Link")
        LinkLabel19.Text = LocalizationService.ForSection("Main.Interface")("UnmountImage.Link")
        GroupBox4.Text = LocalizationService.ForSection("Main.Interface")("ImageOperations.Group")
        Button26.Text = LocalizationService.ForSection("Main.Interface")("MountImage.Button")
        Button27.Text = LocalizationService.ForSection("Main.Interface")("Commit.Changes.Button")
        Button28.Text = LocalizationService.ForSection("Main.Interface")("CommitImage.Button")
        Button29.Text = LocalizationService.ForSection("Main.Interface")("Unmount.Image.Button")
        Button25.Text = LocalizationService.ForSection("Main.Interface")("Reload.Servicing.Button")
        Button24.Text = LocalizationService.ForSection("Main.Interface")("Switch.Image.Indexes.Button")
        Button30.Text = LocalizationService.ForSection("Main.Interface")("ApplyImage.Button")
        Button31.Text = LocalizationService.ForSection("Main.Interface")("CaptureImage.Button")
        Button32.Text = LocalizationService.ForSection("Main.Interface")("Remove.VolumeImages.Button")
        Button33.Text = LocalizationService.ForSection("Main.Interface")("Save.Complete.Image.Button")
        GroupBox5.Text = LocalizationService.ForSection("Main.Interface")("Package.Operations.Group")
        Button36.Text = LocalizationService.ForSection("Main.Interface")("AddPackage.Button")
        Button34.Text = LocalizationService.ForSection("Main.Interface")("Get.Package.Button")
        Button38.Text = LocalizationService.ForSection("Main.Interface")("Save.Installed.Button")
        Button35.Text = LocalizationService.ForSection("Main.Interface")("RemovePackage.Button")
        Button37.Text = LocalizationService.ForSection("Main.Interface")("Component.Store.Maint.Button")
        GroupBox6.Text = LocalizationService.ForSection("Main.Interface")("Feature.Operations.Group")
        Button41.Text = LocalizationService.ForSection("Main.Interface")("EnableFeature.Button")
        Button39.Text = LocalizationService.ForSection("Main.Interface")("Get.Feature.Button")
        Button42.Text = LocalizationService.ForSection("Main.Interface")("Save.Feature.Button")
        Button40.Text = LocalizationService.ForSection("Main.Interface")("DisableFeature.Button")
        GroupBox7.Text = LocalizationService.ForSection("Main.Interface")("AppX.Package.Operations")
        Button44.Text = LocalizationService.ForSection("Main.Interface")("Add.AppX.Package.Button")
        Button45.Text = LocalizationService.ForSection("Main.Interface")("Get.App.Button")
        Button46.Text = LocalizationService.ForSection("Main.Interface")("Save.Installed.AppX.Button")
        Button43.Text = LocalizationService.ForSection("Main.Interface")("Remove.AppX.Package.Button")
        GroupBox8.Text = LocalizationService.ForSection("Main.Interface")("Capability.Operations.Group")
        Button48.Text = LocalizationService.ForSection("Main.Interface")("AddCapability.Button")
        Button49.Text = LocalizationService.ForSection("Main.Interface")("Get.Capability.Button")
        Button50.Text = LocalizationService.ForSection("Main.Interface")("Save.Capability.Button")
        Button47.Text = LocalizationService.ForSection("Main.Interface")("RemoveCapability.Button")
        GroupBox9.Text = LocalizationService.ForSection("Main.Interface")("DriverOperations.Group")
        Button53.Text = LocalizationService.ForSection("Main.Interface")("AddDriverPackage.Button")
        Button52.Text = LocalizationService.ForSection("Main.Interface")("Get.Driver.Button")
        Button54.Text = LocalizationService.ForSection("Main.Interface")("Save.Installed.Driver.Button")
        Button51.Text = LocalizationService.ForSection("Main.Interface")("RemoveDriver.Button")
        GroupBox10.Text = LocalizationService.ForSection("Main.Interface")("Windows.Group")
        Button55.Text = LocalizationService.ForSection("Main.Interface")("GetConfig.Button")
        Button56.Text = LocalizationService.ForSection("Main.Interface")("SaveConfig.Button")
        Button57.Text = LocalizationService.ForSection("Main.Interface")("Set.Target.Path.Button")
        Button58.Text = LocalizationService.ForSection("Main.Interface")("SetScratchSpace.Button")

        If OnlineManagement Then
            Dim onlineMountedText As String = LocalizationService.ForSection("Main.Interface")("Yes.Button")
            Dim onlineNotMountedText As String = LocalizationService.ForSection("Main.Interface")("No.Button")
            Label50.Text = If(IsImageMounted, onlineMountedText, onlineNotMountedText)
            Text = LocalizationService.ForSection("Main.OnlineManagement.Start")("Install.DISM.Tools.Label")
            Label41.Text = LocalizationService.ForSection("Main.OnlineManagement.Start")("Install.Label")
            Label47.Text = LocalizationService.ForSection("Main.OnlineManagement.Start")("Install.Label")
            Label49.Text = LocalizationService.ForSection("Main.OnlineManagement.Start")("Install.Label")
        ElseIf OfflineManagement Then
            Dim offlineMountedText As String = LocalizationService.ForSection("Main.Interface")("Offline.Management.Button")
            Dim offlineNotMountedText As String = LocalizationService.ForSection("Main.Interface")("No.Button")
            Label50.Text = If(IsImageMounted, offlineMountedText, offlineNotMountedText)
            Text = LocalizationService.ForSection("Main.OfflineManagement")("OfflineInstall.Label")
            Label41.Text = LocalizationService.ForSection("Main.OfflineManagement")("Install.Label")
            Label46.Text = LocalizationService.ForSection("Main.OfflineManagement")("Install.Label")
            Label47.Text = LocalizationService.ForSection("Main.OfflineManagement")("Install.Label")
            Label49.Text = LocalizationService.ForSection("Main.OfflineManagement")("Install.Label")
        End If

        ' Infinity Home
                ChangeComputerNameLink.Text = LocalizationService.ForSection("Main.Interface")("Rename.Link")
                Label1.Text = LocalizationService.ForSection("Main.Interface")("DomainMembership.Label")
                Label2.Text = LocalizationService.ForSection("Main.Interface")("WorkgroupDomain.Label")
                Label3.Text = LocalizationService.ForSection("Main.Interface")("IP.Address.Config.Label")
                Label4.Text = LocalizationService.ForSection("Main.Interface")("Explore.Get.Started.Label")
                Label5.Text = LocalizationService.ForSection("Main.Interface")("Stay.Up.Date.Label")
                Label9.Text = LocalizationService.ForSection("Main.Interface")("FactDay.Label")
                LinkLabel27.Text = LocalizationService.ForSection("Main.Interface")("Learn.Snew.Link")
                LinkLabel28.Text = LocalizationService.ForSection("Main.Interface")("Get.Started.DISM.Link")
                LinkLabel29.Text = LocalizationService.ForSection("Main.Interface")("Manage.Install.Link")
                LinkLabel30.Text = LocalizationService.ForSection("Main.Interface")("Manage.External.Link")
                Label12.Text = LocalizationService.ForSection("Main.Interface")("Learn.Watching.Videos.Label")
                Label6.Text = LocalizationService.ForSection("Main.Interface")("Video.Content.Loaded.Label")
                Label7.Text = LocalizationService.ForSection("Main.Interface")("News.Feed.Loaded.Label")
                LinkLabel31.Text = LocalizationService.ForSection("Main.Interface")("LearnMore.Link")
                LinkLabel32.Text = LocalizationService.ForSection("Main.Interface")("Retry.Button")
                LinkLabel33.Text = LocalizationService.ForSection("Main.News.Load")("Retry.Button")
                LinkLabel34.Text = LocalizationService.ForSection("Main.News")("LearnMore.Link")

        RefreshInfinityHomeLocalizedInformation()
        RefreshNewsFeedLocalizedInformation()
    End Sub

    Private Sub RefreshNewsFeedLocalizedInformation()
        If Not IsHandleCreated Then Exit Sub

        Try
            If HasFeedItems(FeedContents) Then RenderNewsFeed()
        Catch ex As Exception
            DynaLog.LogMessage("Could not refresh localized news feed information: " & ex.Message)
        End Try
    End Sub

    Private Sub RefreshInfinityHomeLocalizedInformation()
        If Not IsHandleCreated Then Exit Sub

        Try
            DisplayInfinityComputerInformation()
        Catch ex As Exception
            DynaLog.LogMessage("Could not refresh Infinity Home localized information: " & ex.Message)
        End Try
    End Sub

    Sub CheckDTProjHeaders(DTFileName As String)
        DynaLog.LogMessage("Getting header of project file " & Quote & DTFileName & Quote & "...")
        Dim ProjHeaderTest As String
        ProjHeaderTest = File.ReadAllText(DTFileName)
        If ProjHeaderTest.StartsWith("<?xml") Then
            DynaLog.LogMessage("Header of project begins with XML initializer. This is a Microsoft SQL Server Data Tools project, not a DISMTools project.")
            ' SQL Server project
            isSqlServerDTProj = True
        ElseIf ProjHeaderTest.StartsWith("# DISMTools project file") Then
            DynaLog.LogMessage("Header of project begins with DISMTools initializer. This is a DISMTools project.")
            ' DISMTools project
            isSqlServerDTProj = False
        End If
    End Sub

    Sub LoadDTProj(DTProjPath As String, DTProjFileName As String, BypassFileDialog As Boolean, SkipBGProcs As Boolean)
        BWFailPanel.Visible = False
        DynaLog.LogMessage("Getting state of image registry control panel...")
        If RegistryControlPanel.Visible Then
            DynaLog.LogMessage("Image registry control panel is open. Attempting to close...")
            RegistryControlPanel.Close()
            If RegistryControlPanel.Visible Then
                DynaLog.LogMessage("Second check determined the image registry control panel is still open. Cannot continue loading project until it's closed")
                Dim msg As String = ""
                        msg = LocalizationService.ForSection("Main.Project.Load.Guard")("Image.Registry.Message")
                MsgBox(msg, vbOKOnly + vbExclamation, Text)
                Exit Sub
            End If
        End If
        DynaLog.LogMessage("Either the control panel was closed successfully or wasn't opened in the first place. Continuing project load...")
        If File.Exists(DTProjPath) Then
            DynaLog.LogMessage("Project file exists. Checking header for possible conflicts with SQL Server...")
            CheckDTProjHeaders(DTProjPath)
            If isSqlServerDTProj Then
                DynaLog.LogMessage("We are dealing with a SQL Server Data Tools project. Cancelling project load...")
                MessageBox.Show(LocalizationService.ForSection("Main.Messages")("Project.DISM.Tools.Label"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            SaveProjectToolStripMenuItem.Enabled = True
            SaveProjectasToolStripMenuItem.Enabled = True
            If ProgressPanel.OperationNum = 0 Then
                DynaLog.LogMessage("Loading newly created project...")
                prjName = NewProj.TextBox1.Text
                Text = prjName & " - DISMTools"
                If Debugger.IsAttached Then
                    Text &= " (debug mode)"
                End If
                DynaLog.LogMessage("Project name: " & prjName)
                PleaseWaitDialog.Label2.Text = LocalizationService.ForSection("Main.Project.Load").Format("LoadingProject.Label", prjName)
                PleaseWaitDialog.ShowDialog(Me)
                Label49.Text = prjName
                Label52.Text = DTProjPath
                projPath = DTProjPath
                projPath = projPath.Replace("\" & DTProjFileName & ".dtproj", "").Trim()
                DynaLog.LogMessage("Detecting if images are mounted here (least likely outcome but anyway)...")
                If IsImageMounted Then
                    DynaLog.LogMessage("An image is mounted")
                    ImageView_NoImage.Visible = False
                    ImageView_BasicInfo.Visible = True
                End If
                DynaLog.LogMessage("Populating project tree view with detailed structure...")
                PopulateProjectTree(prjName)
                isProjectLoaded = True
                IsImageMounted = False
                DynaLog.LogMessage("Updating project properties...")
                UpdateProjProperties(False, False, SkipBGProcs)
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
                DynaLog.LogMessage("Reordering Recents list...")
                Dim Project As New Recents()
                Project.ProjPath = DTProjPath
                Project.ProjName = DTProjFileName
                Project.Order = 0
                DynaLog.LogMessage(Project.ToString())
                If RecentList IsNot Nothing Then
                    RecentsLV.Items.Clear()
                    RecentList.Insert(0, Project)
                    For Each recentProject In RecentList
                        recentProject.Order = RecentList.IndexOf(recentProject)
                        RecentsLV.Items.Add(If(recentProject.ProjName <> "", recentProject.ProjName, Path.GetFileNameWithoutExtension(recentProject.ProjPath)))
                    Next
                End If
            Else
                DynaLog.LogMessage("Loading existing project...")
                If OpenFileDialog1.FileName = "" Then
                    If BypassFileDialog = False Then
                        Exit Sub
                    Else
                        prjName = Path.GetFileNameWithoutExtension(DTProjPath)
                        Text = prjName & " - DISMTools"
                        If Debugger.IsAttached Then
                            Text &= " (debug mode)"
                        End If
                        Label52.Text = DTProjPath
                        projPath = DTProjPath
                        projPath = projPath.Replace("\" & DTProjFileName & ".dtproj", "").Trim()
                        DynaLog.LogMessage("Project name: " & prjName)
                        PleaseWaitDialog.Label2.Text = LocalizationService.ForSection("Main.Project.Load").Format("LoadingProject.Label", prjName)
                        PleaseWaitDialog.ShowDialog(Me)
                        Label49.Text = prjName
                        DynaLog.LogMessage("Detecting if images are mounted here...")
                        If IsImageMounted Then
                            DynaLog.LogMessage("An image is mounted")
                            ImageView_NoImage.Visible = False
                            ImageView_BasicInfo.Visible = True
                        Else
                            DynaLog.LogMessage("An image is not mounted")
                            ImageView_NoImage.Visible = True
                            ImageView_BasicInfo.Visible = False
                        End If
                        DynaLog.LogMessage("Populating project tree view with detailed structure...")
                        PopulateProjectTree(prjName)
                        isProjectLoaded = True

                        DynaLog.LogMessage("Loading project information...")
                        ' Load values (use same code as saving, but reversed)
                        SourceImg = ProjectValueLoadForm.RichTextBox5.Text
                        Try
                            ImgIndex = CInt(ProjectValueLoadForm.RichTextBox6.Text)
                        Catch ex As Exception
                            ' The conversion could not be possible. Maybe because it's "N/A" on the RTB?
                        End Try
                        MountDir = ProjectValueLoadForm.RichTextBox7.Text

                        DynaLog.LogMessage("Project settings:" & CrLf &
                                           "- Source image: " & SourceImg & CrLf &
                                           "- Image index: " & ImgIndex & CrLf &
                                           "- Mount directory: " & MountDir & CrLf)


                        DynaLog.LogMessage("Preparing work of background processes...")

                        ' Set initial settings for background processes
                        bwGetImageInfo = True
                        bwGetAdvImgInfo = True
                        bwBackgroundProcessAction = 0

                        DynaLog.LogMessage("Beginning to update project properties to run bg processes...")

                        ' Detect individual stuff
                        If Directory.Exists(projPath & "\mount" & "\Windows") Then
                            DynaLog.LogMessage("Mount directory is in project directory")
                            ' Detect whether image is mounted by checking its Windows directory.
                            ' This will be changed in the future but, because this is in alpha, scan
                            ' whether the image's Windows folder exists
                            IsImageMounted = True
                            UpdateProjProperties(True, False, SkipBGProcs)
                        ElseIf Directory.Exists(MountDir & "\Windows") Then
                            DynaLog.LogMessage("An image was mounted somewhere else")
                            ' This is for these cases where image was mounted to outside the project
                            IsImageMounted = True
                            UpdateProjProperties(True, False, SkipBGProcs)
                        Else
                            DynaLog.LogMessage("This image is bad.")
                            IsImageMounted = False
                            UpdateProjProperties(False, False, SkipBGProcs)
                        End If
                        If IsImageMounted Then
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
                    Label52.Text = DTProjPath
                    projPath = DTProjPath
                    projPath = projPath.Replace("\" & DTProjFileName & ".dtproj", "").Trim()
                    DynaLog.LogMessage("Project name: " & prjName)
                    PleaseWaitDialog.Label2.Text = LocalizationService.ForSection("Main.Project.Load").Format("LoadingProject.Label", prjName)
                    PleaseWaitDialog.ShowDialog(Me)
                    Label49.Text = prjName
                    DynaLog.LogMessage("Detecting if images are mounted here...")
                    If IsImageMounted Then
                        DynaLog.LogMessage("An image is mounted")
                        ImageView_NoImage.Visible = False
                        ImageView_BasicInfo.Visible = True
                    Else
                        DynaLog.LogMessage("An image is not mounted")
                        ImageView_NoImage.Visible = True
                        ImageView_BasicInfo.Visible = False
                    End If
                    DynaLog.LogMessage("Populating project tree view with detailed structure...")
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

                    DynaLog.LogMessage("Project settings:" & CrLf &
                                       "- Source image: " & SourceImg & CrLf &
                                       "- Image index: " & ImgIndex & CrLf &
                                       "- Mount directory: " & MountDir)


                    DynaLog.LogMessage("Preparing work of background processes...")

                    ' Set initial settings for background processes
                    bwGetImageInfo = True
                    bwGetAdvImgInfo = True
                    bwBackgroundProcessAction = 0

                    DynaLog.LogMessage("Beginning to update project properties to run bg processes...")

                    ' Detect individual stuff
                    If Directory.Exists(projPath & "\mount" & "\Windows") Then
                        DynaLog.LogMessage("Mount directory is in project directory")
                        ' Detect whether image is mounted by checking its Windows directory.
                        ' This will be changed in the future but, because this is in alpha, scan
                        ' whether the image's Windows folder exists
                        IsImageMounted = True
                        UpdateProjProperties(True, False, SkipBGProcs)
                    ElseIf Directory.Exists(MountDir & "\Windows") Then
                        DynaLog.LogMessage("An image was mounted somewhere else")
                        ' This is for these cases where image was mounted to outside the project
                        IsImageMounted = True
                        UpdateProjProperties(True, False)
                    Else
                        DynaLog.LogMessage("This image is bad.")
                        IsImageMounted = False
                        UpdateProjProperties(False, False)
                    End If
                    If IsImageMounted Then
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
            DynaLog.LogMessage("Showing background process notification...")
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
                Focus()
            End If
            BackgroundProcessesButton.Image = GetGlyphResource("bg_ops_complete")
        Else
            DynaLog.LogMessage("Project file doesn't exist.")
            MessageBox.Show(LocalizationService.ForSection("Main.Messages.Validation")("Cannot.Load.Project.Message"), LocalizationService.ForSection("Main.Messages.Validation")("Project.Load.Error.Title"), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            If DialogResult.OK Then
                Exit Sub
            End If
        End If
    End Sub

    Sub SaveDTProj()
        DynaLog.LogMessage("Checking if project files exist...")
        If Not File.Exists(projPath & "\settings\project.ini") Then
            DynaLog.LogMessage("No project settings file exists. Exiting...")
            Exit Sub
        End If
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

        DynaLog.LogMessage("Saving project settings to configuration files...")

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
        ProjectValueLoadForm.RichTextBox26.AppendText("[ProjOptions]" & CrLf & ProjectValueLoadForm.RichTextBox1.Lines(1) & CrLf & ProjectValueLoadForm.RichTextBox1.Lines(2) & CrLf & ProjectValueLoadForm.RichTextBox1.Lines(3) & CrLf & CrLf &
                                                      "[ImageOptions]" & CrLf &
                                                      "ImageFile=" & ProjectValueLoadForm.RichTextBox5.Text & CrLf &
                                                      "ImageIndex=" & ProjectValueLoadForm.RichTextBox6.Text & CrLf &
                                                      "ImageMountPoint=" & ProjectValueLoadForm.RichTextBox7.Text & CrLf &
                                                      "ImageVersion=" & ProjectValueLoadForm.RichTextBox8.Text & CrLf &
                                                      "ImageName=" & ProjectValueLoadForm.RichTextBox9.Text & CrLf &
                                                      "ImageDescription=" & ProjectValueLoadForm.RichTextBox10.Text & CrLf &
                                                      "ImageWIMBoot=" & ProjectValueLoadForm.RichTextBox11.Text & CrLf &
                                                      "ImageArch=" & ProjectValueLoadForm.RichTextBox12.Text & CrLf &
                                                      "ImageHal=" & ProjectValueLoadForm.RichTextBox13.Text & CrLf &
                                                      "ImageSPBuild=" & ProjectValueLoadForm.RichTextBox14.Text & CrLf &
                                                      "ImageSPLevel=" & ProjectValueLoadForm.RichTextBox15.Text & CrLf &
                                                      "ImageEdition=" & ProjectValueLoadForm.RichTextBox16.Text & CrLf &
                                                      "ImagePType=" & ProjectValueLoadForm.RichTextBox17.Text & CrLf &
                                                      "ImagePSuite=" & ProjectValueLoadForm.RichTextBox18.Text & CrLf &
                                                      "ImageSysRoot=" & ProjectValueLoadForm.RichTextBox19.Text & CrLf &
                                                      "ImageDirCount=" & ProjectValueLoadForm.RichTextBox20.Text & CrLf &
                                                      "ImageFileCount=" & ProjectValueLoadForm.RichTextBox21.Text & CrLf &
                                                      "ImageEpochCreate=" & ProjectValueLoadForm.RichTextBox22.Text & CrLf &
                                                      "ImageEpochModify=" & ProjectValueLoadForm.RichTextBox23.Text & CrLf &
                                                      "ImageLang=" & ProjectValueLoadForm.RichTextBox24.Text)
        Try
            ProjectValueLoadForm.EpochRTB2.Text = DateTimeOffset.FromUnixTimeSeconds(CType(ProjectValueLoadForm.RichTextBox22.Text, Long)).ToString().Replace(" +00:00", "").Trim()
            ProjectValueLoadForm.EpochRTB3.Text = DateTimeOffset.FromUnixTimeSeconds(CType(ProjectValueLoadForm.RichTextBox23.Text, Long)).ToString().Replace(" +00:00", "").Trim()
        Catch ex As Exception
            ProjectValueLoadForm.EpochRTB2.Text = LocalizationService.ForSection("Wait")("NotAvailable.Label")
            ProjectValueLoadForm.EpochRTB3.Text = LocalizationService.ForSection("Wait")("ProjectValue.Label")
        End Try
        DynaLog.LogMessage("Configured project settings:" & CrLf & ProjectValueLoadForm.RichTextBox26.Text)
        If Debugger.IsAttached Then
            ProjectValueLoadForm.ShowDialog()
        End If
        Try
            DynaLog.LogMessage("Writing to project INI...")
            File.WriteAllText(projPath & "\" & "settings\project.ini", ProjectValueLoadForm.RichTextBox26.Text)
        Catch ex As Exception

        End Try
        DynaLog.LogMessage("Removing modified mark...")
        isModified = False
    End Sub

    ''' <summary>
    ''' Unloads the DISMTools project
    ''' </summary>
    ''' <param name="IsBeingClosed">Determines whether the program is being closed</param>
    ''' <param name="SaveProject">Determines whether the program should save the project</param>
    ''' <remarks>The program, attending to the parameters shown above, will unload the project</remarks>
    Sub UnloadDTProj(IsBeingClosed As Boolean, SaveProject As Boolean)
        DynaLog.LogMessage("Preparing to unload project...")
        DynaLog.LogMessage("- Is the program being closed? " & If(IsBeingClosed, "Yes", "No"))
        DynaLog.LogMessage("- Will the project be saved? " & If(SaveProject, "Yes", "No"))
        If ImgBW.IsBusy Then
            DynaLog.LogMessage("Background processes are busy. Ask the user what they want to do")
            Dim msg As String = ""
                    msg = LocalizationService.ForSection("Main.Project.Unload")("Bg.Procs.Still.Message")
            If MsgBox(msg, vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                DynaLog.LogMessage("Cancelling background processes...")
                ImgBW.CancelAsync()
            Else
                DynaLog.LogMessage("User decided not to cancel background processes. Exiting procedure...")
                Exit Sub
            End If
                    MenuDesc.Text = LocalizationService.ForSection("Main.Project.Unload")("Cancelling.Bg.Procs.Button")
            While ImgBW.IsBusy()
                ToolStripButton3.Enabled = False
                Application.DoEvents()
                Thread.Sleep(100)
            End While
            ToolStripButton3.Enabled = True
                    MenuDesc.Text = LocalizationService.ForSection("Main.Project.Unload")("Ready.Item")
        End If
        bwBackgroundProcessAction = 0
        bwGetImageInfo = True
        bwGetAdvImgInfo = True
        If imgCommitOperation = 0 Then
            Dim IsInFfuMode As Boolean
            DynaLog.LogMessage("The image will be unmounted committing changes...")
            If Path.GetExtension(CurrentImage.ImageFile).Equals(".ffu", StringComparison.OrdinalIgnoreCase) Then
                IsInFfuMode = True

                ' We have to do all of this because FFUs can't be saved normally. The workaround is to capture it into a new file,
                ' unmount the old FFU, replace it with the new one, and mount that one... it will be considered a "new" FFU file,
                ' but at least we save the changes...

                Dim tempFfuPath As String = String.Format("capturedFFU_{0}.ffu", New Random().Next(Integer.MaxValue))

                ' Options for capture task
                ProgressPanel.FFUCaptureSourceDrive = CurrentImage.FFUInfo.MountDiskPath
                ProgressPanel.FFUCaptureDestinationFfuImage = Path.Combine(Path.GetTempPath(), tempFfuPath)
                ProgressPanel.FFUCaptureName = CurrentImage.ImageName
                ProgressPanel.FFUCaptureDescription = CurrentImage.ImageDescription
                ProgressPanel.FFUCaptureCompressType = 1

                ' Options for unmount task
                ProgressPanel.MountDir = MountDir
                ProgressPanel.UMountOp = 1
                ProgressPanel.UMountLocalDir = True
                ProgressPanel.RandomMountDir = ""
                ProgressPanel.CheckImgIntegrity = False
                ProgressPanel.SaveToNewIndex = False
                ProgressPanel.UMountImgIndex = 1

                ' Options for replace task
                ProgressPanel.FFUReplaceSourceFFU = Path.Combine(Path.GetTempPath(), tempFfuPath)
                ProgressPanel.FFUReplaceDestinationFFU = CurrentImage.ImageFile

                ProgressPanel.TaskList.AddRange({5, 21, 998})
            Else
                IsInFfuMode = False

                ProgressPanel.OperationNum = 21
                ProgressPanel.UMountLocalDir = True
                ProgressPanel.RandomMountDir = ""   ' Hope there isn't anything to set here
                ProgressPanel.UMountImgIndex = ImgIndex
                ProgressPanel.MountDir = MountDir
                ProgressPanel.UMountOp = 0
            End If
            ProgressPanel.ShowDialog(Me)
            If IsInFfuMode Then
                UpdateProjProperties(False, False)
                SaveDTProj()
            End If
            Exit Sub
        ElseIf imgCommitOperation = 1 Then
            DynaLog.LogMessage("The image will be unmounted discarding changes...")
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
            DynaLog.LogMessage("Saving project configuration...")
            If File.Exists(projPath & "\settings\project.ini") Then
                SaveDTProj()
            End If
        End If
        Text = "DISMTools"
        If Debugger.IsAttached Then
            Text &= " (debug mode)"
        End If
        DynaLog.LogMessage("Removing items from project tree view...")
        UnpopulateProjectTree()
        ProjectToolStripMenuItem.Visible = False
        CommandsToolStripMenuItem.Visible = False
        Refresh()
        HomePanel.Visible = True
        PrjPanel.Visible = False
        isProjectLoaded = False
        SaveProjectToolStripMenuItem.Enabled = False
        SaveProjectasToolStripMenuItem.Enabled = False
        BGProcDetails.Hide()
        DynaLog.LogMessage("Clearing completion state of background processes...")
        Array.Clear(CompletedTasks, 0, CompletedTasks.Length)
        PendingTasks = Enumerable.Repeat(True, PendingTasks.Length).ToArray()
        If RegistryControlPanel IsNot Nothing Then
            DynaLog.LogMessage("Attempting to close the image registry control panel...")
            RegistryControlPanel.Close()
        End If
        DynaLog.LogMessage("Ending special management modes if user is in one of them...")
        If OnlineManagement Then EndOnlineManagement()
        If OfflineManagement Then EndOfflineManagement()
        ' Set this to its default state
        CurrentImage = New WindowsImage()
    End Sub

    Sub BeginOnlineManagement(ShowDialog As Boolean)
        DynaLog.LogMessage("Beginning active installation management. Show warning? " & If(ShowDialog, "Yes", "No"))
        If ShowDialog Then
            If ActiveInstAccessWarn.ShowDialog(Me) = Windows.Forms.DialogResult.Cancel Then Exit Sub
        End If
        IsImageMounted = True
        isProjectLoaded = True
                Text = LocalizationService.ForSection("Main.OnlineManagement.Start")("Install.DISM.Tools.Label")
        OnlineManagement = True
        ' Initialize background processes
        bwGetImageInfo = True
        bwGetAdvImgInfo = True
        bwBackgroundProcessAction = 0
                Label50.Text = LocalizationService.ForSection("Main.OnlineManagement.Start")("Yes.Button")
        DynaLog.LogMessage("Clearing items in project tree. We don't need them")
        UnpopulateProjectTree()
        HomePanel.Visible = False
        PrjPanel.Visible = True
        RemountImageWithWritePermissionsToolStripMenuItem.Enabled = False
        SaveProjectToolStripMenuItem.Enabled = False
        SaveProjectasToolStripMenuItem.Enabled = False
        LinkLabel14.Visible = False
        ImageView_NoImage.Visible = False
        ImageView_BasicInfo.Visible = True
        CommandsToolStripMenuItem.Visible = True
        Refresh()
        ' Saving a project is not possible in online mode
        ToolStripButton2.Enabled = False
                Label41.Text = LocalizationService.ForSection("Main.OnlineManagement.Start")("Install.Label")
                Label44.Text = LocalizationService.ForSection("Main.OnlineManagement.Start")("Install.Label")
        Panel2.Visible = False
        ManageOnlineInstallationToolStripMenuItem.Enabled = False
        DynaLog.LogMessage("Setting mount directory to disk root...")
        MountDir = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows))
        DynaLog.LogMessage("Beginning background processes...")
        ImgBW.RunWorkerAsync()
        Exit Sub
    End Sub

    Sub BeginOfflineManagement(ImageDrive As String)
        DynaLog.LogMessage("Beginning active installation management. Drive to service: " & ImageDrive)
        DynaLog.LogMessage("Getting state of image registry control panel...")
        If RegistryControlPanel.Visible Then
            DynaLog.LogMessage("Image registry control panel is open. Attempting to close...")
            RegistryControlPanel.Close()
            If RegistryControlPanel.Visible Then
                DynaLog.LogMessage("Second check determined the image registry control panel is still open. Cannot continue loading project until it's closed")
                Dim msg As String = ""
                        msg = LocalizationService.ForSection("Main.OfflineManagement")("Image.Registry.Message")
                MsgBox(msg, vbOKOnly + vbExclamation, Text)
                Exit Sub
            End If
        End If
        DynaLog.LogMessage("Either the control panel was closed successfully or wasn't opened in the first place. Continuing project load...")
        IsImageMounted = True
        isProjectLoaded = True
                Text = LocalizationService.ForSection("Main.OfflineManagement")("OfflineInstall.Label")
        OfflineManagement = True
        ' Initialize background processes
        bwGetImageInfo = True
        bwGetAdvImgInfo = True
        bwBackgroundProcessAction = 0
                Label50.Text = LocalizationService.ForSection("Main.OfflineManagement")("Yes.Button")
        DynaLog.LogMessage("Clearing items in project tree. We don't need them")
        UnpopulateProjectTree()
        HomePanel.Visible = False
        PrjPanel.Visible = True
        RemountImageWithWritePermissionsToolStripMenuItem.Enabled = False
        SaveProjectToolStripMenuItem.Enabled = False
        SaveProjectasToolStripMenuItem.Enabled = False
        LinkLabel14.Visible = False
        ImageView_NoImage.Visible = False
        ImageView_BasicInfo.Visible = True
        CommandsToolStripMenuItem.Visible = True
        Refresh()
        ' Saving a project is not possible in offline mode either
        ToolStripButton2.Enabled = False
                Label41.Text = LocalizationService.ForSection("Main.OfflineManagement")("Install.Label")
                Label44.Text = LocalizationService.ForSection("Main.OfflineManagement")("Install.Label")
        Panel2.Visible = False
        ManageOfflineInstallationToolStripMenuItem.Enabled = False
        DynaLog.LogMessage("Setting mount directory to disk...")
        MountDir = ImageDrive
        DynaLog.LogMessage("Beginning background processes...")
        ImgBW.RunWorkerAsync()
        Exit Sub
    End Sub

    Sub EndOfflineManagement()
        If ImgBW.IsBusy Then
            DynaLog.LogMessage("Background processes are busy. Ask the user what they want to do")
            Dim msg As String = ""
                    msg = LocalizationService.ForSection("Main.EndOfflineMgmt")("Bg.Procs.Still.Message")
            If MsgBox(msg, vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                DynaLog.LogMessage("Cancelling background processes...")
                ImgBW.CancelAsync()
            Else
                DynaLog.LogMessage("User decided not to cancel background processes. Exiting procedure...")
                Exit Sub
            End If
                    MenuDesc.Text = LocalizationService.ForSection("Main.EndOfflineMgmt")("Cancelling.Bg.Procs.Button")
            While ImgBW.IsBusy()
                ToolStripButton3.Enabled = False
                Application.DoEvents()
                Thread.Sleep(100)
            End While
            ToolStripButton3.Enabled = True
                    MenuDesc.Text = LocalizationService.ForSection("Main.EndOffline")("Ready.Item")
        End If
        bwBackgroundProcessAction = 0
        bwGetImageInfo = True
        bwGetAdvImgInfo = True
        IsImageMounted = False
        isProjectLoaded = False
        Text = "DISMTools"
        OfflineManagement = False
                Label50.Text = LocalizationService.ForSection("Main.EndOffline")("Yes.Button")
        HomePanel.Visible = True
        PrjPanel.Visible = False
        RemountImageWithWritePermissionsToolStripMenuItem.Enabled = False
        LinkLabel14.Visible = False
        ImageView_NoImage.Visible = False
        ImageView_BasicInfo.Visible = True
        CommandsToolStripMenuItem.Visible = False
        ProjectToolStripMenuItem.Visible = False
        Refresh()
        ToolStripButton2.Enabled = True
        ' Enable tasks in the new design accordingly
        Button24.Enabled = True
        Button25.Enabled = True
        Button26.Enabled = True
        Button27.Enabled = True
        Button28.Enabled = True
        Button29.Enabled = True
        Panel2.Visible = True
        BGProcDetails.Hide()
        ManageOfflineInstallationToolStripMenuItem.Enabled = True
        DynaLog.LogMessage("Clearing completion state of background processes...")
        Array.Clear(CompletedTasks, 0, CompletedTasks.Length)
        PendingTasks = Enumerable.Repeat(True, PendingTasks.Count).ToArray()
        MountDir = ""
    End Sub

    Sub EndOnlineManagement()
        If ImgBW.IsBusy Then
            DynaLog.LogMessage("Background processes are busy. Ask the user what they want to do")
            Dim msg As String = ""
                    msg = LocalizationService.ForSection("Main.EndOnlineMgmt")("Bg.Procs.Still.Message")
            If MsgBox(msg, vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                DynaLog.LogMessage("Cancelling background processes...")
                ImgBW.CancelAsync()
            Else
                DynaLog.LogMessage("User decided not to cancel background processes. Exiting procedure...")
                Exit Sub
            End If
                    MenuDesc.Text = LocalizationService.ForSection("Main.EndOnlineMgmt")("Cancelling.Bg.Procs.Button")
            While ImgBW.IsBusy()
                ToolStripButton3.Enabled = False
                Application.DoEvents()
                Thread.Sleep(100)
            End While
            ToolStripButton3.Enabled = True
                    MenuDesc.Text = LocalizationService.ForSection("Main.EndOnline")("Ready.Item")
        End If
        bwBackgroundProcessAction = 0
        bwGetImageInfo = True
        bwGetAdvImgInfo = True
        IsImageMounted = False
        isProjectLoaded = False
        Text = "DISMTools"
        OnlineManagement = False
                Label50.Text = LocalizationService.ForSection("Main.EndOnline")("Yes.Button")
        HomePanel.Visible = True
        PrjPanel.Visible = False
        RemountImageWithWritePermissionsToolStripMenuItem.Enabled = False
        LinkLabel14.Visible = False
        ImageView_NoImage.Visible = False
        ImageView_BasicInfo.Visible = True
        CommandsToolStripMenuItem.Visible = False
        ProjectToolStripMenuItem.Visible = False
        Refresh()
        ToolStripButton2.Enabled = True
        ' Enable tasks in the new design accordingly
        Button24.Enabled = True
        Button25.Enabled = True
        Button26.Enabled = True
        Button27.Enabled = True
        Button28.Enabled = True
        Button29.Enabled = True
        Panel2.Visible = True
        ManageOnlineInstallationToolStripMenuItem.Enabled = True
        BGProcDetails.Hide()
        DynaLog.LogMessage("Clearing completion state of background processes...")
        Array.Clear(CompletedTasks, 0, CompletedTasks.Length)
        PendingTasks = Enumerable.Repeat(True, PendingTasks.Count).ToArray()
        MountDir = ""
    End Sub

    Sub UpdateProjProperties(WasImageMounted As Boolean, IsReadOnly As Boolean, Optional SkipBGProcs As Boolean = False)
        DynaLog.LogMessage("Updating project properties...")
        DynaLog.LogMessage("- Is an image mounted here? " & If(WasImageMounted, "Yes", "No"))
        DynaLog.LogMessage("- Is the mounted image read-only? " & If(IsReadOnly, "Yes", "No"))
        DynaLog.LogMessage("- Skip background processes? " & If(SkipBGProcs, "Yes", "No"))
        If WasImageMounted Then
                    Label50.Text = LocalizationService.ForSection("Main.UpdateProjProps")("Yes.Button")
            LinkLabel14.Visible = False
            ImageView_NoImage.Visible = False
            ImageView_BasicInfo.Visible = True
            IsImageMounted = True
        Else
            Label50.Text = LocalizationService.ForSection("Main.UpdateProjProps")("No.Button")
            LinkLabel14.Visible = True
            ImageView_NoImage.Visible = True
            ImageView_BasicInfo.Visible = False
            IsImageMounted = False
            SourceImg = "N/A"
            ImgIndex = 0
            MountDir = "N/A"
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
            RemountImageWithWritePermissionsToolStripMenuItem.Enabled = False
            Exit Sub
        End If
        If IsReadOnly Then
            RemountImageWithWritePermissionsToolStripMenuItem.Enabled = True
        Else
            RemountImageWithWritePermissionsToolStripMenuItem.Enabled = False
        End If
        If SkipBGProcs Then Exit Sub
        DynaLog.LogMessage("Checking if background processes are busy...")
        ' Set image properties
        If Not ImgBW.IsBusy Then
            DynaLog.LogMessage("Background processes are not busy. Starting them...")
            ImgBW.RunWorkerAsync()
        Else
            DynaLog.LogMessage("Background processes are busy.")
        End If
    End Sub

    Sub PopulateProjectTree(MainProjNameNode As String)
        prjTreeStatus.Visible = True
        DynaLog.LogMessage("Adding tree nodes...")
        Try
            prjTreeView.Nodes.Add("parent", LocalizationService.ForSection("Main.Project.Load").Format("Project.Label", MainProjNameNode))
            prjTreeView.Nodes("parent").Nodes.Add("dandi", LocalizationService.ForSection("Main.Project.Load")("Adkdeployment.Tools.Label"))
            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_x86", LocalizationService.ForSection("Main.Project.Load")("DeploymentTools.X86.Label"))
            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_amd64", LocalizationService.ForSection("Main.Project.Load")("Deployment.Tools.Label"))
            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_arm", LocalizationService.ForSection("Main.Project.Load")("DeploymentTools.ARM.Label"))
            prjTreeView.Nodes("parent").Nodes("dandi").Nodes.Add("dandi_arm64", LocalizationService.ForSection("Main.Project.Load")("DeploymentTools.ARM64.Label"))
            prjTreeView.Nodes("parent").Nodes.Add("mount", LocalizationService.ForSection("Main.Project.Load")("MountPoint.Label"))
            prjTreeView.Nodes("parent").Nodes.Add("unattend_xml", LocalizationService.ForSection("Main.Project.Load")("Unattended.Answer.Label"))
            prjTreeView.Nodes("parent").Nodes.Add("scr_temp", LocalizationService.ForSection("Main.Project.Load")("ScratchDirectory.Label"))
            prjTreeView.Nodes("parent").Nodes.Add("reports", LocalizationService.ForSection("Main.Project.Load")("ProjectReports.Label"))
            prjTreeView.ExpandAll()
        Catch ex As Exception

        End Try
        prjTreeStatus.Visible = False
    End Sub

    Sub UnpopulateProjectTree()
        DynaLog.LogMessage("Clearing tree nodes...")
        prjTreeView.Nodes.Clear()
    End Sub

#Region "MenuStrip entries"
    Sub ShowParentDesc(ParentDescMode As Integer)
        Select Case ParentDescMode
            Case 1
                MenuDesc.Text = LocalizationService.ForSection("Main.ShowParentDesc")("View.Options.Related.Item")
            Case 2
                MenuDesc.Text = LocalizationService.ForSection("Main.ShowParentDesc")("View.Options.Project.Item")
            Case 3
                MenuDesc.Text = LocalizationService.ForSection("Main.ShowParentDesc")("View.Options.Image.Item")
            Case 4
                MenuDesc.Text = LocalizationService.ForSection("Main.ShowParentDesc")("View.Options.Additional.Item")
            Case 5
                MenuDesc.Text = LocalizationService.ForSection("Main.ShowParentDesc")("View.Options.Help.Item")
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
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Adds.Additional.Item")
                Case 2
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Applies.Full.Flash.Item")
                Case 3
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Applies.Windows.Image.Item")
                Case 4
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Captures.Incremen.File.Item")
                Case 5
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Captures.Image.Drive.Item")
                Case 6
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Captures.Image.New.Item")
                Case 7
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Deletes.Resources.Item")
                Case 8
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Applies.Changes.Made.Item")
                Case 9
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Deletes.Volume.Image.Item")
                Case 10
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Exports.Copy.Image.Item")
                Case 11
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.Images.Item")
                Case 12
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.List.Wimffu.Item")
                Case 13
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.WIM.Boot.Item")
                Case 14
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.List.Files.Item")
                Case 15
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Mounts.Image.Wimffu.Item")
                Case 16
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Optimizes.Ffuimage.Item")
                Case 17
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Optimizes.Image.Faster.Item")
                Case 18
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Remounts.Mounted.Image.Item")
                Case 19
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Splits.Full.Flash.Item")
                Case 20
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Splits.Existing.WIM.Item")
                Case 21
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Unmounts.Wimffuvhd.Item")
                Case 22
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Updates.WIM.Boot.Item")
                Case 23
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Applies.Siloed.Prov.Item")
                Case 24
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.Message")
                Case 26
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Installs.Cabmsu.Package.Item")
                Case 27
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Removes.Cabfile.Package.Item")
                Case 28
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.Installed.Item")
                Case 30
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Enables.Updates.Feature.Item")
                Case 31
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Disables.Feature.Image.Item")
                Case 32
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Performs.Cleanup.Item")
                Case 33
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Adds.Applicable.Item")
                Case 34
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Gets.Infomation.Prov.Item")
                Case 35
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Dehydrat.Files.Containe.Item")
                Case 36
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.App.Item")
                Case 37
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Addsone.App.Item")
                Case 38
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Removes.Prov.App.Item")
                Case 39
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Optimizes.Total.Size.Item")
                Case 40
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Addscustom.Data.File.Item")
                Case 41
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.Msppatches.Item")
                Case 42
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Command42.Item")
                Case 43
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.Item")
                Case 44
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.Specific.Item")
                Case 45
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Command45.Item")
                Case 46
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Exports.Default.Item")
                Case 47
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.List.Item")
                Case 48
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Imports.Set.DefaultApp.Item")
                Case 49
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Removes.Default.Item")
                Case 50
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.Intl.Item")
                Case 51
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Sets.Default.Uilanguage.Item")
                Case 52
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Sets.Fallback.Default.Item")
                Case 53
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Sets.System.Preferred.Item")
                Case 54
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Sets.Language.Non.Item")
                Case 55
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Sets.Standards.Formats.Item")
                Case 56
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Sets.Input.Locales.Item")
                Case 57
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Sets.Default.System.Message")
                Case 58
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Sets.Default.Time.Item")
                Case 59
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Sets.Default.Message")
                Case 60
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Specifies.Keyboard.Item")
                Case 61
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Generates.Lang.Ini.Item")
                Case 62
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Defines.Default.Item")
                Case 63
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Addscapability.Image.Item")
                Case 64
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Exports.Set.Caps.Item")
                Case 65
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Gets.Installed.Item")
                Case 67
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Removes.Capability.Item")
                Case 68
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.Edition.Image.Item")
                Case 69
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.Editions.Image.Item")
                Case 70
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Changes.Image.Higher.Item")
                Case 71
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Enters.ProductKey.Item")
                Case 72
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.Driver.Message")
                Case 74
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Addsthird.Party.Driver.Item")
                Case 75
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Removes.ThirdParty.Item")
                Case 76
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Exports.ThirdParty.Item")
                Case 77
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Imports.ThirdParty.Message")
                Case 78
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Applies.Unattend.Item")
                Case 79
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Displays.List.Windows.Item")
                Case 80
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Retrieves.Configured.Item")
                Case 81
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Retrieves.Target.Path.Item")
                Case 82
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Sets.Available.Item")
                Case 83
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Sets.Location.Win.Item")
                Case 84
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Gets.Number.Days.Item")
                Case 85
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Reverts.PC.Item")
                Case 86
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Removes.Ability.Roll.Item")
                Case 87
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Sets.Number.Days.Item")
                Case 88
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Gets.State.Reserved.Item")
                Case 89
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Sets.State.Reserved.Item")
                Case 90             ' Edge can also be deployed
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Adds.Microsoft.Item")
                Case 91
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Command91.Item")
                Case 92
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Addsmicrosoft.Edge.Web.Item")
                Case 93
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Saves.Complete.Image.Message")
                Case Else
                    ' Do not show anything
            End Select
        Else
            Select Case ChildDescMode
                Case 1
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Creates.New.DISM.Item")
                Case 2
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Opens.Existing.DISM.Item")
                Case 3
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Enters.Online.Install.Item")
                Case 4
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Saves.Changes.Project.Item")
                Case 5
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Saves.Project.Another.Item")
                Case 6
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Closes.Project.Message")
                Case 7
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Opens.File.Explorer.Item")
                Case 8
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Unloads.Project.Message")
                Case 9
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Switches.Mounted.Image.Item")
                Case 10
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Launches.Project.Item")
                Case 11
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Launches.Image.Section.Item")
                Case 12
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("ImageFormat.Item")
                Case 13
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Merges.Two.SWM.Item")
                Case 14
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Remounts.Image.Read.Item")
                Case 15
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Opens.Command.Console.Item")
                Case 16
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Lets.Manage.Item")
                Case 17
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Lets.Manage.Project.Item")
                Case 18
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Shows.Overview.Mounted.Item")
                Case 19
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Configures.Settings.Item")
                Case 20
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Opens.Help.Topics.Item")
                Case 21
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Opens.Glossary.Don.Item")
                Case 22
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Shows.Command.Help.Item")
                Case 23
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Shows.Item")
                Case 24
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Lets.Report.Feedback.Item")
                Case 25
                    MenuDesc.Text = LocalizationService.ForSection("Main.ShowChildDescs")("Opens.Git.Hub.Message")
            End Select
        End If
    End Sub

    Sub HideParentDesc()
                MenuDesc.Text = LocalizationService.ForSection("Main.HideParentDesc")("Ready.Label")
        If ImgBW.CancellationPending Then
                    MenuDesc.Text = LocalizationService.ForSection("Main.HideParentDesc")("Cancelling.Bg.Procs.Item")
        End If
    End Sub

    Sub HideChildDescs()
                MenuDesc.Text = LocalizationService.ForSection("Main.HideChildDescs")("Ready.Label")
        If ImgBW.CancellationPending Then
                    MenuDesc.Text = LocalizationService.ForSection("Main.HideChildDescs")("Cancelling.Bg.Procs.Item")
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

    Private Sub HideChildDescsTrigger(sender As Object, e As EventArgs) Handles AppendImage.MouseLeave, ApplyFFU.MouseLeave, ApplyImage.MouseLeave, CaptureCustomImage.MouseLeave, CaptureFFU.MouseLeave, CaptureImage.MouseLeave, CleanupMountpoints.MouseLeave, CommitImage.MouseLeave, DeleteImage.MouseLeave, ExportImage.MouseLeave, GetImageInfo.MouseLeave, GetWIMBootEntry.MouseLeave, ListImage.MouseLeave, MountImage.MouseLeave, OptimizeFFU.MouseLeave, OptimizeImage.MouseLeave, RemountImage.MouseLeave, SplitFFU.MouseLeave, SplitImage.MouseLeave, UnmountImage.MouseLeave, UpdateWIMBootEntry.MouseLeave, ApplySiloedPackage.MouseLeave, GetPackages.MouseLeave, AddPackage.MouseLeave, RemovePackage.MouseLeave, GetFeatures.MouseLeave, EnableFeature.MouseLeave, DisableFeature.MouseLeave, CleanupImage.MouseLeave, AddProvisionedAppxPackage.MouseLeave, GetProvisioningPackageInfo.MouseLeave, ApplyCustomDataImage.MouseLeave, GetProvisionedAppxPackages.MouseLeave, AddProvisionedAppxPackage.MouseLeave, RemoveProvisionedAppxPackage.MouseLeave, OptimizeProvisionedAppxPackages.MouseLeave, SetProvisionedAppxDataFile.MouseLeave, CheckAppPatch.MouseLeave, GetAppPatchInfo.MouseLeave, GetAppPatches.MouseLeave, GetAppInfo.MouseLeave, GetApps.MouseLeave, ExportDefaultAppAssociations.MouseLeave, GetDefaultAppAssociations.MouseLeave, ImportDefaultAppAssociations.MouseLeave, RemoveDefaultAppAssociations.MouseLeave, GetIntl.MouseLeave, SetUILangFallback.MouseLeave, SetSysUILang.MouseLeave, SetSysLocale.MouseLeave, SetUserLocale.MouseLeave, SetInputLocale.MouseLeave, SetAllIntl.MouseLeave, SetTimeZone.MouseLeave, SetSKUIntlDefaults.MouseLeave, SetLayeredDriver.MouseLeave, GenLangINI.MouseLeave, SetSetupUILang.MouseLeave, AddCapability.MouseLeave, ExportSource.MouseLeave, GetCapabilities.MouseLeave, RemoveCapability.MouseLeave, GetCurrentEdition.MouseLeave, GetTargetEditions.MouseLeave, SetEdition.MouseLeave, SetProductKey.MouseLeave, GetDrivers.MouseLeave, AddDriver.MouseLeave, RemoveDriver.MouseLeave, ExportDriver.MouseLeave, ApplyUnattend.MouseLeave, GetPESettings.MouseLeave, SetScratchSpace.MouseLeave, SetTargetPath.MouseLeave, GetOSUninstallWindow.MouseLeave, InitiateOSUninstall.MouseLeave, RemoveOSUninstall.MouseLeave, SetOSUninstallWindow.MouseLeave, SetReservedStorageState.MouseLeave, GetReservedStorageState.MouseLeave, NewProjectToolStripMenuItem.MouseLeave, OpenExistingProjectToolStripMenuItem.MouseLeave, SaveProjectToolStripMenuItem.MouseLeave, SaveProjectasToolStripMenuItem.MouseLeave, ExitToolStripMenuItem.MouseLeave, ViewProjectFilesInFileExplorerToolStripMenuItem.MouseLeave, UnloadProjectToolStripMenuItem.MouseLeave, SwitchImageIndexesToolStripMenuItem.MouseLeave, ProjectPropertiesToolStripMenuItem.MouseLeave, ImagePropertiesToolStripMenuItem.MouseLeave, ImageManagementToolStripMenuItem.MouseLeave, OSPackagesToolStripMenuItem.MouseLeave, ProvisioningPackagesToolStripMenuItem.MouseLeave, AppPackagesToolStripMenuItem.MouseLeave, AppPatchesToolStripMenuItem.MouseLeave, DefaultAppAssociationsToolStripMenuItem.MouseLeave, LanguagesAndRegionSettingsToolStripMenuItem.MouseLeave, CapabilitiesToolStripMenuItem.MouseLeave, WindowsEditionsToolStripMenuItem.MouseLeave, DriversToolStripMenuItem.MouseLeave, UnattendedAnswerFilesToolStripMenuItem.MouseLeave, WindowsPEServicingToolStripMenuItem.MouseLeave, OSUninstallToolStripMenuItem.MouseLeave, ReservedStorageToolStripMenuItem.MouseLeave, ImageConversionToolStripMenuItem.MouseLeave, WIMESDToolStripMenuItem.MouseLeave, RemountImageWithWritePermissionsToolStripMenuItem.MouseLeave, CommandShellToolStripMenuItem.MouseLeave, OptionsToolStripMenuItem.MouseLeave, HelpTopicsToolStripMenuItem.MouseLeave, AboutDISMToolsToolStripMenuItem.MouseLeave, UnattendedAnswerFileManagerToolStripMenuItem.MouseLeave, AddEdge.MouseLeave, AddEdgeBrowser.MouseLeave, AddEdgeWebView.MouseLeave, ReportManagerToolStripMenuItem.MouseLeave, MergeSWM.MouseLeave, MountedImageManagerTSMI.MouseLeave, ReportFeedbackToolStripMenuItem.MouseLeave, ManageOnlineInstallationToolStripMenuItem.MouseLeave, AddProvisioningPackage.MouseLeave, SaveImageInformationToolStripMenuItem.MouseLeave, ContributeToTheHelpSystemToolStripMenuItem.MouseLeave, ImportDriver.MouseLeave
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

    Private Sub ImportDriver_MouseEnter(sender As Object, e As EventArgs) Handles ImportDriver.MouseEnter
        ShowChildDescs(True, 77)
    End Sub

    Private Sub ApplyUnattend_MouseEnter(sender As Object, e As EventArgs) Handles ApplyUnattend.MouseEnter
        ShowChildDescs(True, 78)
    End Sub

    Private Sub GetPESettings_MouseEnter(sender As Object, e As EventArgs) Handles GetPESettings.MouseEnter
        ShowChildDescs(True, 79)
    End Sub

    Private Sub SetScratchSpace_MouseEnter(sender As Object, e As EventArgs) Handles SetScratchSpace.MouseEnter
        ShowChildDescs(True, 82)
    End Sub

    Private Sub SetTargetPath_MouseEnter(sender As Object, e As EventArgs) Handles SetTargetPath.MouseEnter
        ShowChildDescs(True, 83)
    End Sub

    Private Sub GetOSUninstallWindow_MouseEnter(sender As Object, e As EventArgs) Handles GetOSUninstallWindow.MouseEnter
        ShowChildDescs(True, 84)
    End Sub

    Private Sub InitiateOSUninstall_MouseEnter(sender As Object, e As EventArgs) Handles InitiateOSUninstall.MouseEnter
        ShowChildDescs(True, 85)
    End Sub

    Private Sub RemoveOSUninstall_MouseEnter(sender As Object, e As EventArgs) Handles RemoveOSUninstall.MouseEnter
        ShowChildDescs(True, 86)
    End Sub

    Private Sub SetOSUninstallWindow_MouseEnter(sender As Object, e As EventArgs) Handles SetOSUninstallWindow.MouseEnter
        ShowChildDescs(True, 87)
    End Sub

    Private Sub GetReservedStorageState_MouseEnter(sender As Object, e As EventArgs) Handles GetReservedStorageState.MouseEnter
        ShowChildDescs(True, 88)
    End Sub

    Private Sub SetReservedStorageState_MouseEnter(sender As Object, e As EventArgs) Handles SetReservedStorageState.MouseEnter
        ShowChildDescs(True, 89)
    End Sub

    Private Sub AddEdge_MouseEnter(sender As Object, e As EventArgs) Handles AddEdge.MouseEnter
        ShowChildDescs(True, 90)
    End Sub

    Private Sub AddEdgeBrowser_MouseEnter(sender As Object, e As EventArgs) Handles AddEdgeBrowser.MouseEnter
        ShowChildDescs(True, 91)
    End Sub

    Private Sub AddEdgeWebView_MouseEnter(sender As Object, e As EventArgs) Handles AddEdgeWebView.MouseEnter
        ShowChildDescs(True, 92)
    End Sub

    Private Sub SaveImageInformationToolStripMenuItem_MouseEnter(sender As Object, e As EventArgs) Handles SaveImageInformationToolStripMenuItem.MouseEnter
        ShowChildDescs(True, 93)
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

    Private Sub Glossary_MouseEnter(sender As Object, e As EventArgs)
        ShowChildDescs(False, 21)
    End Sub

    Private Sub CmdHelp_MouseEnter(sender As Object, e As EventArgs)
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
        DynaLog.LogMessage("Opening new project panel...")
        NewProj.ShowDialog(Me)
    End Sub

    Private Sub ExistingProjLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles ExistingProjLink.LinkClicked
        If Not HomePanel.Visible Then Exit Sub
        DynaLog.LogMessage("Opening project OFD...")
        If OpenFileDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("File specified in OFD: " & OpenFileDialog1.FileName)
            If File.Exists(OpenFileDialog1.FileName) Then
                DynaLog.LogMessage("Project file exists")
                Dim Project As New Recents()
                Project.ProjPath = OpenFileDialog1.FileName
                Project.ProjName = Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName)
                Project.Order = 0
                DynaLog.LogMessage(Project.ToString())
                If RecentList IsNot Nothing Then
                    DynaLog.LogMessage("Reordering recents...")
                    Dim LVItems As ListView.ListViewItemCollection = RecentsLV.Items
                    Dim ProjNames As New List(Of String)
                    For Each LVI As ListViewItem In LVItems
                        ProjNames.Add(LVI.SubItems(0).Text)
                    Next
                    Dim itmOrder As Integer = 0
                    For Each Proj In ProjNames
                        If Proj = Project.ProjName Then
                            itmOrder = ProjNames.IndexOf(Proj)
                            Exit For
                        End If
                    Next
                    RecentsLV.Items.Clear()
                    If ProjNames.Contains(Project.ProjName) Then
                        ChangeRecentListOrder(Project, itmOrder)
                    Else
                        RecentList.Insert(0, Project)
                        For Each recentProject In RecentList
                            recentProject.Order = RecentList.IndexOf(recentProject)
                            RecentsLV.Items.Add(If(recentProject.ProjName <> "", recentProject.ProjName, Path.GetFileNameWithoutExtension(recentProject.ProjPath)))
                        Next
                    End If
                    Try
                        RecentProject1ToolStripMenuItem.Text = " "
                        RecentProject2ToolStripMenuItem.Text = " "
                        RecentProject3ToolStripMenuItem.Text = " "
                        RecentProject4ToolStripMenuItem.Text = " "
                        RecentProject5ToolStripMenuItem.Text = " "
                        RecentProject6ToolStripMenuItem.Text = " "
                        RecentProject7ToolStripMenuItem.Text = " "
                        RecentProject8ToolStripMenuItem.Text = " "
                        RecentProject9ToolStripMenuItem.Text = " "
                        RecentProject10ToolStripMenuItem.Text = " "

                        ' Reconfigure text
                        RecentProject1ToolStripMenuItem.Text = RecentList(0).ProjPath
                        RecentProject1ToolStripMenuItem.Visible = True
                        RecentProject2ToolStripMenuItem.Text = RecentList(1).ProjPath
                        RecentProject2ToolStripMenuItem.Visible = True
                        RecentProject3ToolStripMenuItem.Text = RecentList(2).ProjPath
                        RecentProject3ToolStripMenuItem.Visible = True
                        RecentProject4ToolStripMenuItem.Text = RecentList(3).ProjPath
                        RecentProject4ToolStripMenuItem.Visible = True
                        RecentProject5ToolStripMenuItem.Text = RecentList(4).ProjPath
                        RecentProject5ToolStripMenuItem.Visible = True
                        RecentProject6ToolStripMenuItem.Text = RecentList(5).ProjPath
                        RecentProject6ToolStripMenuItem.Visible = True
                        RecentProject7ToolStripMenuItem.Text = RecentList(6).ProjPath
                        RecentProject7ToolStripMenuItem.Visible = True
                        RecentProject8ToolStripMenuItem.Text = RecentList(7).ProjPath
                        RecentProject8ToolStripMenuItem.Visible = True
                        RecentProject9ToolStripMenuItem.Text = RecentList(8).ProjPath
                        RecentProject9ToolStripMenuItem.Visible = True
                        RecentProject10ToolStripMenuItem.Text = RecentList(9).ProjPath
                        RecentProject10ToolStripMenuItem.Visible = True
                    Catch ex As Exception
                        ' Don't do anything special here
                    End Try
                End If
                ProgressPanel.OperationNum = 990
                DynaLog.LogMessage("Loading project...")
                LoadDTProj(OpenFileDialog1.FileName, Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName), False, False)
            End If
        End If
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles ProjectPropertiesToolStripMenuItem.Click, Button23.Click
                ProjProperties.ImageTaskHeader1.ItemText = LocalizationService.ForSection("Main")("Props.Label")
        If Environment.OSVersion.Version.Major = 10 Then
            ProjProperties.Text = ""
        Else
            ProjProperties.Text = ProjProperties.ImageTaskHeader1.ItemText
        End If
        DynaLog.LogMessage("Showing project/image properties...")
        ProjProperties.ShowDialog(Me)
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles ImagePropertiesToolStripMenuItem.Click

                ProjProperties.ImageTaskHeader1.ItemText = LocalizationService.ForSection("Main")("ProjProps.Label")
        If Environment.OSVersion.Version.Major = 10 Then
            ProjProperties.Text = ""
        Else
            ProjProperties.Text = ProjProperties.ImageTaskHeader1.ItemText
        End If
        DynaLog.LogMessage("Showing project/image properties...")
        ProjProperties.ShowDialog(Me)
    End Sub

    Private Sub UnloadProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnloadProjectToolStripMenuItem.Click
        DynaLog.LogMessage("Showing save question...")
        SaveProjectQuestionDialog.ShowDialog(Me)
        If SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.Yes Then
            DynaLog.LogMessage("Saving project...")
            UnloadDTProj(False, True)
        ElseIf SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.No Then
            DynaLog.LogMessage("Discarding project changes...")
            UnloadDTProj(False, False)
        ElseIf SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.Cancel Then
            DynaLog.LogMessage("Nothing happened here")
            Exit Sub
        End If
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If OnlineManagement Then
            DynaLog.LogMessage("DISMTools is managing the active installation at this time. Ending this mode...")
            EndOnlineManagement()
            StopMountedImageDetector()
        End If
        If OfflineManagement Then
            DynaLog.LogMessage("DISMTools is managing an offline installation at this time. Ending this mode...")
            EndOfflineManagement()
            StopMountedImageDetector()
        End If
        If isProjectLoaded And (Not OnlineManagement Or Not OfflineManagement) Then
            DynaLog.LogMessage("DISMTools is managing a project at this time. Unloading project...")
            If isModified Then
                DynaLog.LogMessage("The image this project contains has been modified")
                SaveProjectQuestionDialog.ShowDialog(Me)
                If SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.Yes Then
                    DynaLog.LogMessage("Saving project...")
                    UnloadDTProj(True, True)
                ElseIf SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.No Then
                    DynaLog.LogMessage("Discarding project changes...")
                    UnloadDTProj(True, False)
                ElseIf SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.Cancel Then
                    DynaLog.LogMessage("Nothing happened here. Cancelling closure...")
                    e.Cancel = True
                End If
            Else
                imgCommitOperation = -1
                DynaLog.LogMessage("No (unsaved) changes have been detected in this project. Unloading it...")
                UnloadDTProj(True, False)
            End If
        End If
        If ImgBW.IsBusy Then
            DynaLog.LogMessage("Background processes are still running. Cannot continue closure")
            e.Cancel = True
            Beep()
            Exit Sub
        End If
        If MountedImgMgr.Visible Then
            DynaLog.LogMessage("The mounted image manager is still open. Attempting closure...")
            MountedImgMgr.Close()
            If MountedImgMgr.Visible Then
                DynaLog.LogMessage("The mounted image manager is still open. Cannot continue closure")
                e.Cancel = True
                Exit Sub
            End If
        End If
        If RegistryControlPanel.Visible Then
            DynaLog.LogMessage("The image registry control panel is still open. Cannot continue closure")
            e.Cancel = True
            Exit Sub
        End If
        If WimScriptEditor.Visible Then
            DynaLog.LogMessage("The configuration list editor is open. Attempting closure...")
            WimScriptEditor.Close()
            If WimScriptEditor.Visible Then
                DynaLog.LogMessage("The configuration list editor is still open. Cannot continue closure")
                e.Cancel = True
                Exit Sub
            End If
        End If
        If InfoSaveResults.Visible Then
            DynaLog.LogMessage("The info saver result viewer is open. Attempting closure...")
            InfoSaveResults.Close()
            If InfoSaveResults.Visible Then
                DynaLog.LogMessage("The info saver result viewer is still open. Cannot continue closure")
                e.Cancel = True
                Exit Sub
            End If
        End If
        If FormBorderStyle = Windows.Forms.FormBorderStyle.None Then
            ToggleFullScreenMode()
        End If
        SaveDTSettings()
        If Not EnableDynaLog Then
            ' Settings have already been saved. Re-enable DynaLog for ending
            EnableDynaLog = True
            DynaLog.EnableLogging()
        End If
        If tourServer.IsListenerAlive() Then
            DynaLog.LogMessage("Tour is active. Attempting to shut down server...")
            tourServer.StopServer()
            TourActionsTSMI.Visible = False
        End If
        DynaLog.LogMessage("Stopping mounted image detector...")
        StopMountedImageDetector()
        DynaLog.LogMessage("Stopping detection of news...")
        If FeedWorker.IsBusy Then FeedWorker.CancelAsync()
        While FeedWorker.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        Timer2.Enabled = False
        Try
            DynaLog.LogMessage("Attempting to save elements in recents list...")
            If (RecentList IsNot Nothing And RecentList.Count >= 0) Then
                SaveRecents(RecentList, Application.StartupPath & "\recents.xml")
            End If
        Catch ex As Exception
            ' Don't save the recent item. The recent list may not have been initialized
        End Try
        If AutoCleanMounts Then
            DynaLog.LogMessage("DISMTools has been configured to automatically clean up mount points. Launching DISM to do the job (this doesn't slow down program closure)...")
            ' Clean up corrupted mount points. Use the DISM executable to avoid slowing down program closure
            Dim DismProc As New Process()
            DismProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
            DismProc.StartInfo.Arguments = "/cleanup-mountpoints"
            DismProc.Start()
        End If
        DynaLog.LogMessage("We Are Done")
        DynaLog.EndLogging()
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        DynaLog.LogMessage("Saving project...")
        SaveDTProj()
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        If OnlineManagement Then
            DynaLog.LogMessage("DISMTools is managing the active installation at this time. Ending this mode...")
            EndOnlineManagement()
            Exit Sub
        End If
        If OfflineManagement Then
            DynaLog.LogMessage("DISMTools is managing an offline installation at this time. Ending this mode...")
            EndOfflineManagement()
            Exit Sub
        End If
        DynaLog.LogMessage("DISMTools is managing a project at this time. Unloading project...")
        If isModified Then
            DynaLog.LogMessage("The image this project contains has been modified")
            SaveProjectQuestionDialog.ShowDialog(Me)
            If SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.Yes Then
                DynaLog.LogMessage("Saving project...")
                UnloadDTProj(False, True)
            ElseIf SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.No Then
                DynaLog.LogMessage("Discarding project changes...")
                UnloadDTProj(False, False)
            ElseIf SaveProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.Cancel Then
                DynaLog.LogMessage("Nothing happened here.")
                Exit Sub
            End If
        Else
            DynaLog.LogMessage("No (unsaved) changes have been detected in this project. Unloading it...")
            UnloadDTProj(False, False)
        End If
    End Sub

    Private Sub CommandShellToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CommandShellToolStripMenuItem.Click
        DynaLog.LogMessage("Launching Command Console...")
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe", "/k " & Quote & Application.StartupPath & "\bin\dthelper.bat" & Quote & " /sh")
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OptionsToolStripMenuItem.Click
        DynaLog.LogMessage("Launching Options Panel...")
        Options.PrefReset.Enabled = True
        Options.ShowDialog(Me)
    End Sub

    Private Sub ExplorerView_Click(sender As Object, e As EventArgs) Handles Button22.Click
        DynaLog.LogMessage("Opening project in File Explorer...")
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", "/select," & Quote & projPath & "\" & Label49.Text & ".dtproj" & Quote)
    End Sub

    Private Sub GetImageInfo_Click(sender As Object, e As EventArgs) Handles GetImageInfo.Click
        If ImgBW.IsBusy Then
            DynaLog.LogMessage("Notifying user about background process being busy...")
            BGProcsBusyDialog.ShowDialog(Me)
            Exit Sub
        End If
        DynaLog.LogMessage("Opening image file information dialog...")
        GetImgInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub prjTreeView_AfterExpand(sender As Object, e As TreeViewEventArgs) Handles prjTreeView.AfterExpand
        Try
            If prjTreeView.SelectedNode.IsExpanded Then
                        ExpandCollapseTSB.Text = LocalizationService.ForSection("Main.ProjectTree.AfterExpand")("Collapse.Label")
                        ExpandToolStripMenuItem.Text = LocalizationService.ForSection("Main.ProjectTree.AfterExpand")("CollapseItem.Label")
                ExpandCollapseTSB.Image = GetGlyphResource("collapse_glyph")
            Else
                        ExpandCollapseTSB.Text = LocalizationService.ForSection("Main.ProjectTree.AfterExpand")("Expand.Item")
                        ExpandToolStripMenuItem.Text = LocalizationService.ForSection("Main.ProjectTree.AfterExpand")("ExpandItem")
                ExpandCollapseTSB.Image = GetGlyphResource("expand_glyph")
            End If
        Catch ex As Exception
            ExpandCollapseTSB.Enabled = False
            Exit Sub
        End Try
    End Sub

    Private Sub prjTreeView_AfterCollapse(sender As Object, e As TreeViewEventArgs) Handles prjTreeView.AfterCollapse
        Try
            If prjTreeView.SelectedNode.IsExpanded Then
                        ExpandCollapseTSB.Text = LocalizationService.ForSection("Main.ProjectTree.AfterCollapse")("Collapse.Label")
                        ExpandToolStripMenuItem.Text = LocalizationService.ForSection("Main.ProjectTree.AfterCollapse")("CollapseItem.Label")
                ExpandCollapseTSB.Image = GetGlyphResource("collapse_glyph")
            Else
                        ExpandCollapseTSB.Text = LocalizationService.ForSection("Main.ProjectTree.AfterCollapse")("Expand.Item")
                        ExpandToolStripMenuItem.Text = LocalizationService.ForSection("Main.ProjectTree.AfterCollapse")("ExpandItem")
                ExpandCollapseTSB.Image = GetGlyphResource("expand_glyph")
            End If
        Catch ex As Exception
                    ExpandCollapseTSB.Text = LocalizationService.ForSection("Main.ProjectTree.AfterCollapse")("ExpandCollapse.Item")
                    ExpandToolStripMenuItem.Text = LocalizationService.ForSection("Main.ProjectTree.AfterCollapse")("ExpandTool.ExpandItem")
            ExpandCollapseTSB.Image = GetGlyphResource("expand_glyph")
        End Try
    End Sub

    Private Sub prjTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles prjTreeView.AfterSelect
        If prjTreeView.SelectedNode.IsExpanded Then
                    ExpandCollapseTSB.Text = LocalizationService.ForSection("Main.ProjectTree.AfterSelect")("Collapse.Label")
                    ExpandToolStripMenuItem.Text = LocalizationService.ForSection("Main.ProjectTree.AfterSelect")("CollapseItem.Label")
            ExpandCollapseTSB.Image = GetGlyphResource("collapse_glyph")
        Else
                    ExpandCollapseTSB.Text = LocalizationService.ForSection("Main.ProjectTree.AfterSelect")("Expand.Item")
                    ExpandToolStripMenuItem.Text = LocalizationService.ForSection("Main.ProjectTree.AfterSelect")("ExpandItem")
            ExpandCollapseTSB.Image = GetGlyphResource("expand_glyph")
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
        If prjTreeView.SelectedNode Is Nothing Then Exit Sub
        Try
            If prjTreeView.SelectedNode.IsExpanded Then
                prjTreeView.SelectedNode.Collapse()
            Else
                prjTreeView.SelectedNode.Expand()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub RefreshViewTSB_Click(sender As Object, e As EventArgs) Handles RefreshViewTSB.Click
        If Not isProjectLoaded Then Exit Sub
        DynaLog.LogMessage("Refreshing the project tree...")
        UnpopulateProjectTree()
        PopulateProjectTree(prjName)
    End Sub

    Private Sub AddPackage_Click(sender As Object, e As EventArgs) Handles AddPackage.Click
        DynaLog.LogMessage("Opening package addition dialog...")
        AddPackageDlg.ShowDialog(Me)
    End Sub

    Private Sub AboutDISMToolsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutDISMToolsToolStripMenuItem.Click, VersionTSMI.Click
        DynaLog.LogMessage("Showing program information...")
        PrgAbout.ShowDialog(Me)
    End Sub

    Private Sub WIMESDToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WIMESDToolStripMenuItem.Click
        DynaLog.LogMessage("Opening image conversion dialog...")
        ImgWim2Esd.ShowDialog(Me)
    End Sub

    Private Sub CaptureImage_Click(sender As Object, e As EventArgs) Handles CaptureImage.Click
        DynaLog.LogMessage("Opening image capture dialog...")
        ImgCapture.ShowDialog(Me)
    End Sub

    Private Sub MergeSWM_Click(sender As Object, e As EventArgs) Handles MergeSWM.Click
        DynaLog.LogMessage("Opening image merger dialog...")
        ImgSwmToWim.ShowDialog(Me)
    End Sub

    Private Sub ApplyImage_Click(sender As Object, e As EventArgs) Handles ApplyImage.Click
        DynaLog.LogMessage("Opening image application dialog...")
        ImgApply.ShowDialog(Me)
    End Sub

    Private Sub SaveProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveProjectToolStripMenuItem.Click
        DynaLog.LogMessage("Saving project...")
        SaveDTProj()
    End Sub

    Private Sub SaveProjectasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveProjectasToolStripMenuItem.Click
        DynaLog.LogMessage("Save Project As menu command received." & CrLf &
                           "- Is a project loaded? " & If(isProjectLoaded, "Yes", "No") & CrLf &
                           "- Online management mode? " & If(OnlineManagement, "Yes", "No") & CrLf &
                           "- Offline management mode? " & If(OfflineManagement, "Yes", "No") & CrLf &
                           "- Project path: " & Quote & projPath & Quote)
        If Not isProjectLoaded OrElse OnlineManagement OrElse OfflineManagement Then
            DynaLog.LogMessage("Save Project As cannot continue because no regular DISMTools project is loaded.")
            MessageBox.Show(LocalizationService.ForSection("Main.SaveProjectAs")("Unavailable.Message"),
                            LocalizationService.ForSection("Main.SaveProjectAs")("Error.Title"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim sourceParent As DirectoryInfo = Directory.GetParent(Path.GetFullPath(projPath))
        Using saveAsDialog As New NewProj()
            saveAsDialog.SaveAsMode = True
            saveAsDialog.TextBox1.Text = prjName & " - Copy"
            saveAsDialog.TextBox2.Text = If(sourceParent Is Nothing, projPath, sourceParent.FullName)
            DynaLog.LogMessage("Opening the Save Project As dialog...")
            Dim saveAsResult = saveAsDialog.ShowDialog(Me)
            DynaLog.LogMessage("The Save Project As dialog returned: " & saveAsResult.ToString())
            If saveAsResult <> Windows.Forms.DialogResult.OK Then Exit Sub

            DynaLog.LogMessage("The user confirmed Save Project As. Beginning the copy operation...")
            SaveProjectAsCopy(saveAsDialog.TextBox1.Text.Trim(), saveAsDialog.TextBox2.Text.Trim())
        End Using
    End Sub

    Private Sub SaveProjectAsCopy(newProjectName As String, destinationParent As String)
        Dim targetRoot As String = destinationParent

        Try
            Dim sourceRoot = Path.GetFullPath(projPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            Dim destinationRoot = Path.GetFullPath(destinationParent).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            targetRoot = Path.GetFullPath(Path.Combine(destinationRoot, newProjectName)).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            DynaLog.LogMessage("Preparing to save the project as a copy." & CrLf &
                               "- Source: " & Quote & sourceRoot & Quote & CrLf &
                               "- Destination: " & Quote & targetRoot & Quote)

            If targetRoot.Equals(sourceRoot, StringComparison.OrdinalIgnoreCase) OrElse
               targetRoot.StartsWith(sourceRoot & Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) Then
                MessageBox.Show(LocalizationService.ForSection("Main.SaveProjectAs")("InvalidDestination.Message"), LocalizationService.ForSection("Main.SaveProjectAs")("Error.Title"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            If Directory.Exists(targetRoot) AndAlso Directory.GetFileSystemEntries(targetRoot).Length > 0 Then
                MessageBox.Show(LocalizationService.ForSection("Main.SaveProjectAs").Format("DestinationExists.Message", targetRoot), LocalizationService.ForSection("Main.SaveProjectAs")("Error.Title"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            SaveDTProj()
            CopyProjectDirectory(sourceRoot, targetRoot, sourceRoot)

            Dim targetSettingsPath = Path.Combine(targetRoot, "settings", "project.ini")
            Dim settingsLines = File.ReadAllLines(targetSettingsPath, ASCII)
            For lineIndex = 0 To settingsLines.Length - 1
                If settingsLines(lineIndex).StartsWith("Name=", StringComparison.OrdinalIgnoreCase) Then
                    settingsLines(lineIndex) = "Name=" & Quote & newProjectName & Quote
                ElseIf settingsLines(lineIndex).StartsWith("Location=", StringComparison.OrdinalIgnoreCase) Then
                    settingsLines(lineIndex) = "Location=" & destinationRoot
                End If
            Next
            File.WriteAllLines(targetSettingsPath, settingsLines, ASCII)

            Dim targetProjectFile = Path.Combine(targetRoot, newProjectName & ".dtproj")
            File.WriteAllText(targetProjectFile,
                              "# DISMTools project file. File version: 0.1" & CrLf &
                              "[Settings]" & CrLf &
                              "SettingsInclude=\settings\project.ini" & CrLf & CrLf &
                              "[Project]" & CrLf &
                              "ProjName=" & newProjectName & CrLf &
                              "ProjGuid=" & Guid.NewGuid().ToString(), ASCII)

            Dim previousCommitOperation = imgCommitOperation
            Try
                imgCommitOperation = -1
                UnloadDTProj(False, False)
            Finally
                imgCommitOperation = previousCommitOperation
            End Try

            ProgressPanel.OperationNum = 990
            LoadDTProj(targetProjectFile, newProjectName, True, False)
            If Not isProjectLoaded OrElse
               Not Path.GetFullPath(projPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Equals(targetRoot, StringComparison.OrdinalIgnoreCase) Then
                Throw New InvalidOperationException(LocalizationService.ForSection("Main.SaveProjectAs").Format("OpenCopyFailed.Message", targetProjectFile))
            End If
            DynaLog.LogMessage("Save Project As completed successfully. The copied project is loaded.")
        Catch ex As Exception
            DynaLog.LogMessage("Could not save the project as a copy. Error message: " & ex.Message)
            MessageBox.Show(LocalizationService.ForSection("Main.SaveProjectAs").Format("Error.Message", targetRoot, ex.Message), LocalizationService.ForSection("Main.SaveProjectAs")("Error.Title"), MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CopyProjectDirectory(sourceDirectory As String, targetDirectory As String, sourceRoot As String)
        Directory.CreateDirectory(targetDirectory)

        For Each sourceFile In Directory.GetFiles(sourceDirectory)
            If sourceDirectory.Equals(sourceRoot, StringComparison.OrdinalIgnoreCase) AndAlso
               Path.GetExtension(sourceFile).Equals(".dtproj", StringComparison.OrdinalIgnoreCase) Then Continue For
            File.Copy(sourceFile, Path.Combine(targetDirectory, Path.GetFileName(sourceFile)), True)
        Next

        For Each sourceChildDirectory In Directory.GetDirectories(sourceDirectory)
            Dim relativePath = sourceChildDirectory.Substring(sourceRoot.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            Dim targetChildDirectory = Path.Combine(targetDirectory, Path.GetFileName(sourceChildDirectory))
            Directory.CreateDirectory(targetChildDirectory)
            If IsTemporaryProjectPath(relativePath) Then Continue For
            CopyProjectDirectory(sourceChildDirectory, targetChildDirectory, sourceRoot)
        Next
    End Sub

    Private Function IsTemporaryProjectPath(relativePath As String) As Boolean
        Dim pathParts = relativePath.Split({Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar}, StringSplitOptions.RemoveEmptyEntries)
        If pathParts.Length = 0 Then Return False
        Return pathParts(0).Equals("mount", StringComparison.OrdinalIgnoreCase) OrElse
               pathParts(0).Equals("scr_temp", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Sub ImgBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ImgBW.DoWork
        DynaLog.LogMessage("Preparing background processes...")
        DynaLog.LogMessage("- Background process action: " & bwBackgroundProcessAction)
        DynaLog.LogMessage("- Get basic image information? " & If(bwGetImageInfo, "Yes", "No"))
        DynaLog.LogMessage("- Get advanced image information? " & If(bwGetAdvImgInfo, "Yes", "No"))
        DynaLog.LogMessage("Stopping mounted image detector...")
        StopMountedImageDetector()
        DynaLog.LogMessage("Starting background processes...")
        RunBackgroundProcesses(bwBackgroundProcessAction, bwGetImageInfo, bwGetAdvImgInfo, OnlineManagement, OfflineManagement)
    End Sub

    Private Sub UnloadBtn_Click(sender As Object, e As EventArgs) Handles Button21.Click
        DynaLog.LogMessage("Unloading project/mode...")
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
                    BGProcNotify.Location = New Point(Left + 8, Top + StatusStrip.Top - StatusStrip.Height - 7)
                End If
            End If
        ElseIf BGProcDetails.Visible And pinState = 0 Then
            If Environment.OSVersion.Version.Major = 10 Then    ' The Left property also includes the window shadows on Windows 10 and 11
                BGProcDetails.Location = New Point(Left + 8, Top + StatusStrip.Top - (75 + StatusStrip.Height))
            ElseIf Environment.OSVersion.Version.Major = 6 Then
                If Environment.OSVersion.Version.Minor = 1 Then ' The same also applies to Windows 7
                    BGProcDetails.Location = New Point(Left + 8, Top + StatusStrip.Top - (75 + StatusStrip.Height))
                Else
                    BGProcDetails.Location = New Point(Left + 8, Top + StatusStrip.Top - StatusStrip.Height - 75)
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
        DynaLog.LogMessage("Reporting new progress...")
        DynaLog.LogMessage("Progress message (translated): " & Quote & progressLabel & Quote)
        BGProcDetails.Label2.Text = progressLabel
        If bwBackgroundProcessAction <> 0 Then BGProcDetails.ProgressBar1.Style = ProgressBarStyle.Marquee Else BGProcDetails.ProgressBar1.Style = ProgressBarStyle.Blocks
        If regJumps Then
            DynaLog.LogMessage("Regular progress bar value jumps are being done")
            BGProcDetails.ProgressBar1.Value = e.ProgressPercentage
        Else
            DynaLog.LogMessage("Regular progress bar value jumps are not being done")
            BGProcDetails.ProgressBar1.Value = irregVal
        End If
        progressMin = BGProcDetails.ProgressBar1.Value
    End Sub

    Private Sub ImgBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ImgBW.RunWorkerCompleted
        DynaLog.LogMessage("Background processes have finished")
        If FailedBGProcResultDic.Count > 0 Then
            DynaLog.LogMessage("One or more background processes has failed.")
            BWFailPanel.Visible = True
            Beep()
        End If
        CompletedTasks = Enumerable.Repeat(True, CompletedTasks.Length).ToArray()
        BGProcDetails.ProgressBar1.Style = ProgressBarStyle.Blocks
        If Not MountedImageDetectorBW.IsBusy Then Call MountedImageDetectorBW.RunWorkerAsync()
        WatcherTimer.Enabled = True
        areBackgroundProcessesDone = True
        BackgroundProcessesButton.Image = GetGlyphResource("bg_ops_complete")
        progressLabel = LocalizationService.ForSection("Main.BgProcesses")("ImageCompleted.Label")
        BGProcDetails.Label2.Text = progressLabel
        BGProcDetails.ProgressBar1.Value = BGProcDetails.ProgressBar1.Maximum
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed And Not ProgressPanel.Visible Then ProgressPanel.Dispose()
        If isOrphaned Then
            DynaLog.LogMessage("Background processes have stopped because the image requires a servicing session reload")
            WatcherTimer.Enabled = False
            DynaLog.LogMessage("Stopping watcher...")
            If WatcherBW.IsBusy Then WatcherBW.CancelAsync()
            While WatcherBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(100)
            End While
            If BGProcDetails.Visible Then
                BGProcDetails.ProgressBar1.Value = 0
            End If
            If Not OrphanedMountedImgDialog.IsDisposed Then OrphanedMountedImgDialog.Dispose()
            OrphanedMountedImgDialog.ShowDialog(Me)
            If OrphanedMountedImgDialog.DialogResult = Windows.Forms.DialogResult.OK Then
                DynaLog.LogMessage("User agreed to reload the servicing session")
                ProgressPanel.Validate()
                ProgressPanel.MountDir = MountDir
                ProgressPanel.OperationNum = 18
                ProgressPanel.ShowDialog(Me)
            ElseIf OrphanedMountedImgDialog.DialogResult = Windows.Forms.DialogResult.Cancel Then
                DynaLog.LogMessage("User decided not to reload the servicing session. Unloading project...")
                UnloadDTProj(False, False)
                ImgBW.CancelAsync()
            End If
        End If
        If Not IsCompatible Then
            DynaLog.LogMessage("The image was reported to have incompatibilities. Studying incompatibility...")
            If SysVer.Major = 6 And SysVer.Build >= 6000 Then
                If SysVer.Minor = 0 Then        ' Windows Vista / WinPE 2.x
                    DynaLog.LogMessage("Incompatibility studied. This is a Windows Vista/Server 2008 image")
                    ' Let the user know about the incompatibility
                    If Not ProgressPanel.IsDisposed Then
                        ProgressPanel.Dispose()
                        ProgressPanel.Close()
                    End If
                    ImgWinVistaIncompatibilityDialog.ShowDialog(Me)
                    If ImgWinVistaIncompatibilityDialog.DialogResult = Windows.Forms.DialogResult.OK Then
                        DynaLog.LogMessage("User decided not to unmount the image")
                        ' Disable every option
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
                        DynaLog.LogMessage("User decided to unmount the image")
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
                        ProgressPanel.ShowDialog(Me)
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
                DynaLog.LogMessage("Incompatibility studied. Someone really put a Longhorn beta or an earlier version of Windows in WIM form to the project.")
                DynaLog.LogMessage("Unmounting the image without asking first. This is so known that it's rejected.")
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
                ProgressPanel.ShowDialog(Me)
            End If
        End If
    End Sub

    Private Sub RemovePackage_Click(sender As Object, e As EventArgs) Handles RemovePackage.Click
        RemPackage.ShowDialog(Me)
    End Sub

    Private Sub EnableFeature_Click(sender As Object, e As EventArgs) Handles EnableFeature.Click
        EnableFeat.ShowDialog(Me)
    End Sub

    Private Sub DisableFeature_Click(sender As Object, e As EventArgs) Handles DisableFeature.Click
        DisableFeat.ShowDialog(Me)
    End Sub

    Private Sub AddProvisionedAppxPackage_Click(sender As Object, e As EventArgs) Handles AddProvisionedAppxPackage.Click
        AddProvAppxPackage.ShowDialog(Me)
    End Sub

    Private Sub RemoveProvisionedAppxPackage_Click(sender As Object, e As EventArgs) Handles RemoveProvisionedAppxPackage.Click
        RemProvAppxPackage.ShowDialog(Me)
    End Sub

    Private Sub DeleteImage_Click(sender As Object, e As EventArgs) Handles DeleteImage.Click
        DynaLog.LogMessage("Opening image index removal dialog...")
        ImgIndexDelete.ShowDialog(Me)
    End Sub

    Private Sub MountedImageDetectorBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles MountedImageDetectorBW.DoWork
        DynaLog.LogMessage("Images will be detected in a loop with a 1s break. No logging will be performed here...")
        Do
            If MountedImageDetectorBW.CancellationPending Or ImgBW.IsBusy Then Exit Do
            Try
                DetectMountedImages(False)
            Catch ex As AccessViolationException
                DynaLog.LogMessage("Procedure failed due to handled access violation.")
            End Try
            Thread.Sleep(1000)
        Loop
    End Sub

    Private Sub MountedImageDetectorBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles MountedImageDetectorBW.RunWorkerCompleted
        DynaLog.LogMessage("The mounted image detector is no longer busy.")
    End Sub

    Private Sub MountedImageManagerTSMI_Click(sender As Object, e As EventArgs) Handles MountedImageManagerTSMI.Click
        DynaLog.LogMessage("Opening the mounted image manager...")
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

    Private Sub ReportManagerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportManagerToolStripMenuItem.Click, ManageReportsToolStripMenuItem.Click
        If Not isProjectLoaded OrElse OnlineManagement OrElse OfflineManagement OrElse String.IsNullOrWhiteSpace(projPath) Then
            DynaLog.LogMessage("The report manager cannot be opened because no DISMTools project is loaded.")
            MessageBox.Show(LocalizationService.ForSection("Main.ReportManager")("ProjectRequired.Message"),
                            LocalizationService.ForSection("Main.ReportManager")("Error.Title"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim reportsPath As String = Path.Combine(projPath, "reports")
        Try
            If Not Directory.Exists(reportsPath) Then Directory.CreateDirectory(reportsPath)
            DynaLog.LogMessage("Opening project reports directory: " & Quote & reportsPath & Quote)
            Process.Start(reportsPath)
        Catch ex As Exception
            DynaLog.LogMessage("Could not open the project reports directory. Path: " & Quote & reportsPath & Quote & "; reason: " & ex.Message)
            MessageBox.Show(LocalizationService.ForSection("Main.ReportManager").Format("OpenFailed.Message", reportsPath, ex.Message),
                            LocalizationService.ForSection("Main.ReportManager")("Error.Title"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ReportFeedbackToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportFeedbackToolStripMenuItem.Click
        DynaLog.LogMessage("Launching page to report feedback...")
        Process.Start("https://github.com/CodingWonders/DISMTools/issues/new/choose")
    End Sub

    Private Sub Discord_Click(sender As Object, e As EventArgs) Handles Discord.Click
        DynaLog.LogMessage("Launching discord join link...")
        Process.Start("https://discord.gg/5TxEmKXNwu")
    End Sub

    Private Sub UnmountImage_Click(sender As Object, e As EventArgs) Handles UnmountImage.Click, UnmountSettingsToolStripMenuItem.Click
        DynaLog.LogMessage("Opening image unmount dialog...")
        If isProjectLoaded And MountDir = MountedImgMgr.ListView1.FocusedItem.SubItems(2).Text Then
            DynaLog.LogMessage("This is the image the user is managing here")
            ImgUMount.RadioButton1.Checked = True
            ImgUMount.RadioButton2.Checked = False
            ImgUMount.TextBox1.Text = ""
        Else
            DynaLog.LogMessage("This is an image different from the one the user is managing here")
            ImgUMount.RadioButton1.Checked = False
            ImgUMount.RadioButton2.Checked = True
            ImgUMount.TextBox1.Text = MountedImgMgr.ListView1.FocusedItem.SubItems(2).Text
            ProgressPanel.UMountImgIndex = MountedImgMgr.ListView1.FocusedItem.SubItems(1).Text
        End If
        ImgUMount.ShowDialog(Me)
    End Sub

    Private Sub CommitAndUnmountTSMI_Click(sender As Object, e As EventArgs) Handles CommitAndUnmountTSMI.Click
        DynaLog.LogMessage("Unmounting the Windows image whilst committing changes...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.OperationNum = 21
        ProgressPanel.UMountLocalDir = False
        ProgressPanel.RandomMountDir = MountedImgMgr.ListView1.FocusedItem.SubItems(2).Text   ' Hope there isn't anything to set here
        ProgressPanel.UMountImgIndex = MountedImgMgr.ListView1.FocusedItem.SubItems(1).Text
        ProgressPanel.MountDir = ""
        ProgressPanel.UMountOp = 0
        ProgressPanel.ShowDialog(Me)
    End Sub

    Private Sub DiscardAndUnmountTSMI_Click(sender As Object, e As EventArgs) Handles DiscardAndUnmountTSMI.Click
        DynaLog.LogMessage("Unmounting the Windows image whilst discarding changes...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.OperationNum = 21
        ProgressPanel.UMountLocalDir = False
        ProgressPanel.RandomMountDir = MountedImgMgr.ListView1.FocusedItem.SubItems(2).Text   ' Hope there isn't anything to set here
        ProgressPanel.UMountImgIndex = MountedImgMgr.ListView1.FocusedItem.SubItems(1).Text
        ProgressPanel.MountDir = ""
        ProgressPanel.UMountOp = 1
        ProgressPanel.ShowDialog(Me)
    End Sub

    Private Sub CleanupImage_Click(sender As Object, e As EventArgs) Handles CleanupImage.Click
        DynaLog.LogMessage("Opening image cleanup dialog...")
        ImgCleanup.ShowDialog(Me)
    End Sub

    Private Sub NewProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewProjectToolStripMenuItem.Click
        DynaLog.LogMessage("Opening project creation dialog...")
        NewProj.ShowDialog(Me)
    End Sub

    Private Sub OpenExistingProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenExistingProjectToolStripMenuItem.Click
        DynaLog.LogMessage("Opening project OFD...")
        If OpenFileDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("File specified in OFD: " & OpenFileDialog1.FileName)
            If File.Exists(OpenFileDialog1.FileName) Then
                DynaLog.LogMessage("Project file exists")
                If isProjectLoaded Then UnloadDTProj(False, If(OnlineManagement Or OfflineManagement, False, True))
                If ImgBW.IsBusy Then Exit Sub
                Dim Project As New Recents()
                Project.ProjPath = OpenFileDialog1.FileName
                Project.ProjName = Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName)
                Project.Order = 0
                DynaLog.LogMessage(Project.ToString())
                If RecentList IsNot Nothing Then
                    DynaLog.LogMessage("Reordering recents...")
                    Dim LVItems As ListView.ListViewItemCollection = RecentsLV.Items
                    Dim ProjNames As New List(Of String)
                    For Each LVI As ListViewItem In LVItems
                        ProjNames.Add(LVI.SubItems(0).Text)
                    Next
                    Dim itmOrder As Integer = 0
                    For Each Proj In ProjNames
                        If Proj = Project.ProjName Then
                            itmOrder = ProjNames.IndexOf(Proj)
                            Exit For
                        End If
                    Next
                    RecentsLV.Items.Clear()
                    If ProjNames.Contains(Project.ProjName) Then
                        ChangeRecentListOrder(Project, itmOrder)
                    Else
                        RecentList.Insert(0, Project)
                        For Each recentProject In RecentList
                            recentProject.Order = RecentList.IndexOf(recentProject)
                            RecentsLV.Items.Add(If(recentProject.ProjName <> "", recentProject.ProjName, Path.GetFileNameWithoutExtension(recentProject.ProjPath)))
                        Next
                    End If
                    Try
                        RecentProject1ToolStripMenuItem.Text = " "
                        RecentProject2ToolStripMenuItem.Text = " "
                        RecentProject3ToolStripMenuItem.Text = " "
                        RecentProject4ToolStripMenuItem.Text = " "
                        RecentProject5ToolStripMenuItem.Text = " "
                        RecentProject6ToolStripMenuItem.Text = " "
                        RecentProject7ToolStripMenuItem.Text = " "
                        RecentProject8ToolStripMenuItem.Text = " "
                        RecentProject9ToolStripMenuItem.Text = " "
                        RecentProject10ToolStripMenuItem.Text = " "

                        ' Reconfigure text
                        RecentProject1ToolStripMenuItem.Text = RecentList(0).ProjPath
                        RecentProject1ToolStripMenuItem.Visible = True
                        RecentProject2ToolStripMenuItem.Text = RecentList(1).ProjPath
                        RecentProject2ToolStripMenuItem.Visible = True
                        RecentProject3ToolStripMenuItem.Text = RecentList(2).ProjPath
                        RecentProject3ToolStripMenuItem.Visible = True
                        RecentProject4ToolStripMenuItem.Text = RecentList(3).ProjPath
                        RecentProject4ToolStripMenuItem.Visible = True
                        RecentProject5ToolStripMenuItem.Text = RecentList(4).ProjPath
                        RecentProject5ToolStripMenuItem.Visible = True
                        RecentProject6ToolStripMenuItem.Text = RecentList(5).ProjPath
                        RecentProject6ToolStripMenuItem.Visible = True
                        RecentProject7ToolStripMenuItem.Text = RecentList(6).ProjPath
                        RecentProject7ToolStripMenuItem.Visible = True
                        RecentProject8ToolStripMenuItem.Text = RecentList(7).ProjPath
                        RecentProject8ToolStripMenuItem.Visible = True
                        RecentProject9ToolStripMenuItem.Text = RecentList(8).ProjPath
                        RecentProject9ToolStripMenuItem.Visible = True
                        RecentProject10ToolStripMenuItem.Text = RecentList(9).ProjPath
                        RecentProject10ToolStripMenuItem.Visible = True
                    Catch ex As Exception
                        ' Don't do anything special here
                    End Try
                End If
                ProgressPanel.OperationNum = 990
                DynaLog.LogMessage("Loading project...")
                LoadDTProj(OpenFileDialog1.FileName, Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName), False, False)
            End If
        End If
    End Sub

    Private Sub AddCapability_Click(sender As Object, e As EventArgs) Handles AddCapability.Click
        AddCapabilities.ShowDialog(Me)
    End Sub

    Private Sub RemoveCapability_Click(sender As Object, e As EventArgs) Handles RemoveCapability.Click
        RemCapabilities.ShowDialog(Me)
    End Sub

    Private Sub AddDriver_Click(sender As Object, e As EventArgs) Handles AddDriver.Click
        AddDrivers.ShowDialog(Me)
    End Sub

    Private Sub RemoveDriver_Click(sender As Object, e As EventArgs) Handles RemoveDriver.Click
        RemDrivers.ShowDialog(Me)
    End Sub

    Private Sub AddProvisioningPackage_Click(sender As Object, e As EventArgs) Handles AddProvisioningPackage.Click
        DynaLog.LogMessage("Opening provisioned package addition dialog...")
        AddProvisioningPkg.ShowDialog(Me)
    End Sub

    Private Sub OnlineInstMgmt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles OnlineInstMgmt.LinkClicked
        If Not HomePanel.Visible Then Exit Sub
        DynaLog.LogMessage("Accessing online installation management mode...")
        ActiveInstAccessWarn.Label2.Visible = False
        BeginOnlineManagement(True)
    End Sub

    Function GetSuitablePackageFolder(PackageName As String)
        DynaLog.LogMessage("Beginning detection of package folders to find the one that suits best...")
        DynaLog.LogMessage("Package name specified for scan: " & PackageName)
        Try
            If Directory.GetDirectories(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps", PackageName & "*", SearchOption.TopDirectoryOnly).Count > 1 Then
                Dim pkgDirs() As String = Directory.GetDirectories(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps", PackageName & "*", SearchOption.TopDirectoryOnly)
                DynaLog.LogMessage("Total amount of directories: " & pkgDirs.Count())
                For Each folder In pkgDirs
                    If Not folder.Contains("neutral") Then
                        DynaLog.LogMessage("Folder " & Quote & folder & Quote & " is suitable.")
                        Return folder
                    End If
                Next
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not grab suitable folder for AppX packages. Error message: " & ex.Message)
            Return Nothing
        End Try
        DynaLog.LogMessage("There are no suitable folders")
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets the path of the main logo of an installed provisioned AppX package
    ''' </summary>
    ''' <param name="PackageName">The name of the AppX package</param>
    ''' <returns>This function returns a path to the logo asset of an application</returns>
    ''' <remarks>This can be a little wonky and may not show the main asset. However, since this allows the program to launch an image viewer afterwards, you can browse other assets</remarks>
    Function GetStoreAppMainLogo(PackageName As String)
        DynaLog.LogMessage("Beginning detection of logo assets to find the main one...")
        Try
            If File.Exists(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName & "\AppxManifest.xml") Then
                DynaLog.LogMessage("An AppX manifest file exists in the main directory. There are no variations of any kind")
                ' Read from manifest
                DynaLog.LogMessage("Reading AppX manifest...")
                Dim ManFile As New RichTextBox() With {
                    .Text = File.ReadAllText(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName & "\AppxManifest.xml")
                }
                For Each line In ManFile.Lines
                    If line.Contains("Logo") Then
                        DynaLog.LogMessage("We have a possible logo...")
                        Dim SplitPaths As New List(Of String)
                        SplitPaths = line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim().Split("\").ToList()
                        SplitPaths.RemoveAt(SplitPaths.Count - 1)
                        Dim newPath As String = String.Join("\", SplitPaths)
                        DynaLog.LogMessage("New path after joining splits: " & newPath)
                        If Directory.GetFiles(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName & "\" & newPath, "*.png").Count > 1 Then
                            Dim logoFiles() As String = Directory.GetFiles(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName & "\" & newPath, "*.png")
                            DynaLog.LogMessage("Logo files found: " & logoFiles.Count())
                            DynaLog.LogMessage("Last logo is " & Quote & logoFiles.Last() & Quote & ". Returning this one, as it could be the largest...")
                            ' Choose the largest one
                            Return logoFiles.Last
                        Else
                            If File.Exists(Path.Combine(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName, line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim())) Then
                                DynaLog.LogMessage("The logo specified in the AppX manifest exists there. Returning it...")
                                Return Path.Combine(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName, line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim())
                            Else
                                DynaLog.LogMessage("The logo specified in the AppX manifest exists there. Returning it...")
                                ' There may be 1 asset in the folder we're looking on. Open it
                                Dim logoFiles() As String = Directory.GetFiles(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps\" & PackageName & "\" & newPath, "*.png")
                                DynaLog.LogMessage("Logo files found: " & logoFiles.Count())
                                DynaLog.LogMessage("Last logo is " & Quote & logoFiles.Last() & Quote & ". Returning this one...")
                                Return logoFiles.Last
                            End If
                        End If
                    End If
                Next
            ElseIf Directory.GetDirectories(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps", PackageName & "*", SearchOption.TopDirectoryOnly).Count > 1 Then
                DynaLog.LogMessage("No AppX manifests exist in the main directory, but there are multiple variations of the package (some architecture-neutral, others architecture-specific)")
                DynaLog.LogMessage("We will take into account the architecture-specific package...")
                DynaLog.LogMessage("Getting directories...")
                Dim pkgDirs() As String = Directory.GetDirectories(If(OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MountDir) & "\Program Files\WindowsApps", PackageName & "*", SearchOption.TopDirectoryOnly)
                DynaLog.LogMessage("Total amount of directories: " & pkgDirs.Count())
                For Each folder In pkgDirs
                    If Not folder.Contains("neutral") Then
                        DynaLog.LogMessage("We have a possible folder candidate. Reading manifest...")
                        ' Read from manifest
                        Dim ManFile As New RichTextBox() With {
                            .Text = File.ReadAllText(folder & "AppxManifest.xml")
                        }
                        For Each line In ManFile.Lines
                            If line.Contains("Logo") Then
                                DynaLog.LogMessage("Returning logo...")
                                Return Path.Combine(folder, line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim())
                            End If
                        Next
                    End If
                Next
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not grab AppX package main logo asset. Error message: " & ex.Message)
            Return Nothing
        End Try
        Return Nothing
    End Function

    Private Sub ViewPackageDirectoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewPackageDirectoryToolStripMenuItem.Click
        DynaLog.LogMessage("Accessing most suitable package directory...")
        Dim suitableFolderName As String = ""
        Try
            suitableFolderName = GetSuitablePackageFolder(RemProvAppxPackage.ListView1.FocusedItem.SubItems(1).Text.Replace(" (Cortana)", "").Trim())
        Catch ex As Exception
            ' Continue
        End Try
        DynaLog.LogMessage("The suitable folder path is: " & suitableFolderName)
        If suitableFolderName <> "" Then
            DynaLog.LogMessage("The suitable folder path can be used with File Explorer")
            Process.Start(suitableFolderName)
            Exit Sub
        End If
        DynaLog.LogMessage("The suitable folder path cannot be used with File Explorer. Using alternate method...")
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
        DynaLog.LogMessage("Getting main Store logo asset...")
        Dim MainLogo As String = GetStoreAppMainLogo(RemProvAppxPackage.ListView1.FocusedItem.SubItems(0).Text)
        DynaLog.LogMessage("Main logo obtained with function: " & MainLogo)
        If MainLogo <> "" And File.Exists(MainLogo) Then
            DynaLog.LogMessage("Main logo exists. Opening it...")
            Process.Start(MainLogo)
            Exit Sub
        End If
        DynaLog.LogMessage("Getting suitable folder...")
        Dim suitableFolderName As String = ""
        Try
            suitableFolderName = GetSuitablePackageFolder(RemProvAppxPackage.ListView1.FocusedItem.SubItems(1).Text.Replace(" (Cortana)", "").Trim())
        Catch ex As Exception
            ' Continue
        End Try
        DynaLog.LogMessage("Suitable folder name: " & suitableFolderName)
        If suitableFolderName <> "" Then
            DynaLog.LogMessage("Checking if AppX manifest exists...")
            If File.Exists(suitableFolderName & "\AppxManifest.xml") Then
                DynaLog.LogMessage("Reading AppX manifest...")
                Dim ManFile As New RichTextBox() With {
                    .Text = File.ReadAllText(suitableFolderName & "\AppxManifest.xml")
                }
                For Each line In ManFile.Lines
                    If line.Contains("<Logo>") Then
                        Dim SplitPaths As New List(Of String)
                        SplitPaths = line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim().Split("\").ToList()
                        SplitPaths.RemoveAt(SplitPaths.Count - 1)
                        Dim newPath As String = String.Join("\", SplitPaths)
                        DynaLog.LogMessage("Full path for logo asset: " & Path.Combine(suitableFolderName, newPath))
                        Process.Start(suitableFolderName & "\" & newPath)
                        Exit For
                    End If
                Next
            End If
            Exit Sub
        End If
        DynaLog.LogMessage("The suitable folder path cannot be used with File Explorer. Using alternate method...")
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
        DynaLog.LogMessage("Beginning download of Update System...")
        Try
            If File.Exists(Application.StartupPath & "\update.exe") Then File.Delete(Application.StartupPath & "\update.exe")
        Catch ex As Exception
            DynaLog.LogMessage("Could not delete existing update downloader...")
            Exit Sub
        End Try
        Try
            DynaLog.LogMessage("Downloading " & Quote & "update.exe" & Quote & " from DISMTools repository...")
            Using client As New WebClient()
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                client.DownloadFile("https://github.com/CodingWonders/DISMTools/raw/stable/Updater/DISMTools-UCS/update-bin/update.exe", Application.StartupPath & "\update.exe")
            End Using
        Catch ex As WebException
            DynaLog.LogMessage("Could not get updater. Error message: " & ex.Status.ToString())
            MsgBox(LocalizationService.ForSection("Main.UpdateChecker").Format("Couldn.Tdownload.Message", ex.Status.ToString()), vbOKOnly + vbCritical, LocalizationService.ForSection("Main.UpdateChecker")("CheckUpdates.Title"))
            Exit Sub
        End Try
        DynaLog.LogMessage("Information to pass to updater:")
        DynaLog.LogMessage("- Branch: " & dtBranch)
        DynaLog.LogMessage("- Process ID (PID): " & Process.GetCurrentProcess().Id)
        If File.Exists(Application.StartupPath & "\update.exe") Then Process.Start(Application.StartupPath & "\update.exe", "/" & dtBranch & " /pid=" & Process.GetCurrentProcess().Id & " " & LocalizationService.GetLanguageCommandLineArgument())
    End Sub

    Private Sub prjTreeView_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles prjTreeView.NodeMouseClick
        DynaLog.LogMessage("Has the end-user right-clicked? " & If(e.Button = Windows.Forms.MouseButtons.Right, "Yes", "No"))
        If e.Button = Windows.Forms.MouseButtons.Right Then
            DynaLog.LogMessage("Name of clicked node: " & e.Node.Name)
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
        DynaLog.LogMessage("Preparing to copy ADK files...")
        DynaLog.LogMessage("Path of selected node: " & prjTreeView.SelectedNode.FullPath)
        If prjTreeView.SelectedNode.Name.StartsWith("dandi") Then
            DynaLog.LogMessage("ADK nodes are selected. Continuing...")
            Try
                If DetectPossibleADKs() = 2 Then
                    DynaLog.LogMessage("The ADK is installed")
                    DynaLog.LogMessage("Copy mode for ADK files: " & adkCopyArg)
                    ' Copy deployment tools. This will default to "Program Files\Windows Kits\10"
                    Select Case adkCopyArg
                        Case 0
                            DynaLog.LogMessage("Copying ADK files of all architectures...")
                            ' Copy all architectures
                            If Directory.Exists(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools") Then
                                Dim arches() As String = New String(3) {"x86", "amd64", "arm", "arm64"}
                                For x = 0 To Array.LastIndexOf(arches, arches.Last)
                                    archIntg = x + 1
                                    currentArch = arches(x)
                                    ' Count files
                                    fileCount = My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\" & arches(x), FileIO.SearchOption.SearchAllSubDirectories).Count
                                    DynaLog.LogMessage("Count of ADK files for " & currentArch & ": " & fileCount)
                                    MenuDesc.Text = LocalizationService.ForSection("Main.ADKCopy").Format("Prepare.Deploy.Tools.Label", If(adkCopyArg = 0, LocalizationService.ForSection("Main.ADKCopy").Format("Architecture.Label", archIntg), ""))
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
                            DynaLog.LogMessage("Copying ADK files for x86...")
                            Dim fileCount As Integer = My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\x86", FileIO.SearchOption.SearchAllSubDirectories).Count
                            MenuDesc.Text = LocalizationService.ForSection("Main.ADKCopy").Format("Prepare.Deploy.Tools.Label", If(adkCopyArg = 0, LocalizationService.ForSection("Main.ADKCopy").Format("Architecture.Label", archIntg), ""))
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
                            DynaLog.LogMessage("Copying ADK files for AMD64...")
                            Dim fileCount As Integer = My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\amd64", FileIO.SearchOption.SearchAllSubDirectories).Count
                            MenuDesc.Text = LocalizationService.ForSection("Main.ADKCopy").Format("Prepare.Deploy.Tools.Label", If(adkCopyArg = 0, LocalizationService.ForSection("Main.ADKCopy").Format("Architecture.Label", archIntg), ""))
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
                            DynaLog.LogMessage("Copying ADK files for ARM...")
                            Dim fileCount As Integer = My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\arm", FileIO.SearchOption.SearchAllSubDirectories).Count
                            MenuDesc.Text = LocalizationService.ForSection("Main.ADKCopy").Format("Prepare.Deploy.Tools.Label", If(adkCopyArg = 0, LocalizationService.ForSection("Main.ADKCopy").Format("Architecture.Label", archIntg), ""))
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
                            DynaLog.LogMessage("Copying ADK files for ARM64...")
                            Dim fileCount As Integer = My.Computer.FileSystem.GetFiles(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)) & "\Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\arm64", FileIO.SearchOption.SearchAllSubDirectories).Count
                            MenuDesc.Text = LocalizationService.ForSection("Main.ADKCopy").Format("Prepare.Deploy.Tools.Label", If(adkCopyArg = 0, LocalizationService.ForSection("Main.ADKCopy").Format("Architecture.Label", archIntg), ""))
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
                DynaLog.LogMessage("Could not copy ADK deployment tools. Error message: " & ex.Message)
                AdkCopyEx = ex
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
            If DetectPossibleADKs() = 2 Then
                        MenuDesc.Text = LocalizationService.ForSection("Main.ADKCopierBW.Background")("ToolsCopied.Label")
            Else
                        MenuDesc.Text = LocalizationService.ForSection("Main.ADKCopierBW.Background")("Deployment.Tools.Aren.Item")
            End If
        Catch ex As Exception
                    MenuDesc.Text = LocalizationService.ForSection("Main.ADKCopierBW.Background")("Deployment.Tools.Copied.Item")
            If AdkCopyEx IsNot Nothing Then
                MenuDesc.Text &= " (" & AdkCopyEx.Message & ")"
            End If
        End Try
    End Sub

    Private Sub ADKCopierBW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles ADKCopierBW.ProgressChanged
        Select Case adkCopyArg
            Case 0
                MenuDesc.Text = LocalizationService.ForSection("Main.ADKCopy").Format("Copying.Deployment.Label", currentArch, e.ProgressPercentage, If(adkCopyArg = 0, LocalizationService.ForSection("Main.ADKCopy").Format("Progress.Architecture.Label", archIntg), ""))
            Case 1
                MenuDesc.Text = LocalizationService.ForSection("Main.ADKCopy").Format("Copying.Deployment.Label", "x86", e.ProgressPercentage, If(adkCopyArg = 0, LocalizationService.ForSection("Main.ADKCopy").Format("Progress.Architecture.Label", archIntg), ""))
            Case 2
                MenuDesc.Text = LocalizationService.ForSection("Main.ADKCopy").Format("Copying.Deployment.Label", "amd64", e.ProgressPercentage, If(adkCopyArg = 0, LocalizationService.ForSection("Main.ADKCopy").Format("Progress.Architecture.Label", archIntg), ""))
            Case 3
                MenuDesc.Text = LocalizationService.ForSection("Main.ADKCopy").Format("Copying.Deployment.Label", "arm", e.ProgressPercentage, If(adkCopyArg = 0, LocalizationService.ForSection("Main.ADKCopy").Format("Progress.Architecture.Label", archIntg), ""))
            Case 4
                MenuDesc.Text = LocalizationService.ForSection("Main.ADKCopy").Format("Copying.Deployment.Label", "arm64", e.ProgressPercentage, If(adkCopyArg = 0, LocalizationService.ForSection("Main.ADKCopy").Format("Progress.Architecture.Label", archIntg), ""))
        End Select
    End Sub

    Private Sub ExpandToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExpandToolStripMenuItem.Click
        ExpandCollapseTSB.PerformClick()
    End Sub

    Private Sub AccessDirectoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AccessDirectoryToolStripMenuItem.Click
        Try
            DynaLog.LogMessage("Path of selected node: " & prjTreeView.SelectedNode.FullPath)
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
                If Not MountDir = (projPath & "\mount") Then
                    Process.Start(MountDir)
                Else
                    Process.Start(projPath & "\mount")
                End If
            ElseIf prjTreeView.SelectedNode.Name = "unattend_xml" Then
                Process.Start(projPath & "\unattend_xml")
            ElseIf prjTreeView.SelectedNode.Name = "scr_temp" Then
                Process.Start(projPath & "\scr_temp")
            ElseIf prjTreeView.SelectedNode.Name = "reports" Then
                Process.Start(projPath & "\reports")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub UnloadProjectToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles UnloadProjectToolStripMenuItem1.Click
        ToolStripButton3.PerformClick()
    End Sub

    Private Sub ScratchDirectorySettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScratchDirectorySettingsToolStripMenuItem.Click
        DynaLog.LogMessage("Opening options dialog...")
        Options.SectionNum = 3
        Options.ShowDialog(Me)
    End Sub

    Private Sub ManageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageToolStripMenuItem.Click
        DynaLog.LogMessage("Opening unattended answer file manager...")
        If isProjectLoaded And Not (OnlineManagement Or OfflineManagement) Then
            UnattendMgr.TextBox1.Text = Path.Combine(projPath, "unattend_xml")
        End If
        UnattendMgr.Show()
    End Sub

    Private Sub CreationWizardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreationWizardToolStripMenuItem.Click
        DynaLog.LogMessage("Opening unattended answer file creator...")
        NewUnattendWiz.Show()
    End Sub

    Private Sub MountImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MountImageToolStripMenuItem.Click
        DynaLog.LogMessage("Opening image mount dialog...")
        DynaLog.LogMessage("Stopping mounted image detector...")
        StopMountedImageDetector()
        ImgMount.ShowDialog(Me)
    End Sub

    Private Sub UnmountImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnmountImageToolStripMenuItem.Click
        DynaLog.LogMessage("Opening image unmount dialog...")
        ImgUMount.RadioButton1.Checked = True
        ImgUMount.RadioButton2.Checked = False
        ImgUMount.ShowDialog(Me)
    End Sub

    Private Sub RemoveVolumeImagesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveVolumeImagesToolStripMenuItem.Click
        ImgIndexDelete.ShowDialog(Me)
    End Sub

    Private Sub SwitchImageIndexesToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SwitchImageIndexesToolStripMenuItem1.Click, SwitchImageIndexesToolStripMenuItem.Click
        ImgIndexSwitch.ShowDialog(Me)
    End Sub

    Private Sub ManageOnlineInstallationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageOnlineInstallationToolStripMenuItem.Click
        DynaLog.LogMessage("Beginning online installation management...")
        Dim showMessage As Boolean = isProjectLoaded
        DynaLog.LogMessage("Will message be shown a second time? " & If(showMessage, "Yes", "No"))
        If isProjectLoaded Then
            DynaLog.LogMessage("Showing warning and proceeding to unload project...")
            ActiveInstAccessWarn.Label2.Visible = True
            If ActiveInstAccessWarn.ShowDialog(Me) = Windows.Forms.DialogResult.Cancel Then Exit Sub
            If ActiveInstAccessWarn.DialogResult = Windows.Forms.DialogResult.OK Then UnloadDTProj(False, True)
            If ImgBW.IsBusy Then Exit Sub
        End If
        ActiveInstAccessWarn.Label2.Visible = False
        BeginOnlineManagement(Not showMessage)
    End Sub

    Private Sub ManageOfflineInstallationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageOfflineInstallationToolStripMenuItem.Click
        DynaLog.LogMessage("Beginning offline installation management...")
        If OfflineInstDriveLister.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Selected drive path: " & drivePath)
            DynaLog.LogMessage("User has accepted the disk chooser popup.")
            If drivePath = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) Then
                DynaLog.LogMessage("Selected drive is the same as the drive currently booted to.")
                Exit Sub
            End If
            If isProjectLoaded Then
                DynaLog.LogMessage("Unloading project...")
                UnloadDTProj(False, True)
                If ImgBW.IsBusy Then Exit Sub
            End If
            BeginOfflineManagement(drivePath)
        End If
    End Sub

    Private Sub UpdCheckerBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles UpdCheckerBW.DoWork
        DynaLog.LogMessage("Beginning update checks...")
        CheckForUpdates(dtBranch)
    End Sub

    Private Sub RemountImageWithWritePermissionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemountImageWithWritePermissionsToolStripMenuItem.Click
        DynaLog.LogMessage("Preparing to remount the image with write permissions...")
        If CurrentImage Is Nothing Then
            CurrentImage = MountedImageList.FirstOrDefault(Function(mountedImage) mountedImage.ImageMountDirectory = MountDir)
        End If
        If CurrentImage Is Nothing Then
            Exit Sub
        End If
        EnableWritePermissions(CurrentImage.ImageFile, CurrentImage.ImageIndex, CurrentImage.ImageMountDirectory)
    End Sub

    Sub EnableWritePermissions(SourceImage As String, SourceIndex As Integer, DestinationPath As String)
        DynaLog.LogMessage("Enabling write permissions of the image:")
        DynaLog.LogMessage("- Image file: " & Quote & SourceImage & Quote)
        DynaLog.LogMessage("- Image index: " & SourceIndex)
        DynaLog.LogMessage("- Mount directory: " & Quote & DestinationPath & Quote)
        DynaLog.LogMessage("Checking if source image " & Quote & SourceImage & Quote & " exists...")
        If File.Exists(SourceImage) Then
            DynaLog.LogMessage("The source image exists in the file system. Preparing to remount it...")
            If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
            ' Configure settings to remount image with write permissions

            DynaLog.LogMessage("Configuring progress panel tasks...")

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
                DynaLog.LogMessage("Updating project properties...")
                UpdateProjProperties(True, False, False)
            Else
                DynaLog.LogMessage("Restarting mounted image detector and watcher...")
                If Not MountedImageDetectorBW.IsBusy Then Call MountedImageDetectorBW.RunWorkerAsync()
                WatcherTimer.Enabled = True
            End If
        End If
    End Sub

    Private Sub GetDrivers_Click(sender As Object, e As EventArgs) Handles GetDrivers.Click
        DynaLog.LogMessage("Opening driver information dialog...")
        ProgressPanel.OperationNum = 994
                PleaseWaitDialog.Label2.Text = LocalizationService.ForSection("Main.GetDrivers")("Loading.DriverPackages.Label")
        If Not CompletedTasks(4) Then
            DynaLog.LogMessage("Device driver background processes haven't completed.")
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        StopMountedImageDetector()
        GetDriverInfo.ShowDialog(Me)
    End Sub

    Private Sub ViewProjectFilesInFileExplorerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewProjectFilesInFileExplorerToolStripMenuItem.Click
        DynaLog.LogMessage("Opening project location in File Explorer...")
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", "/select," & Quote & projPath & "\" & Label49.Text & ".dtproj" & Quote)
    End Sub

    Private Sub WimScriptEditorCommand_Click(sender As Object, e As EventArgs) Handles WimScriptEditorCommand.Click
        DynaLog.LogMessage("Opening WIMScript Configuration Editor...")
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
        DynaLog.LogMessage("Opening feature information dialog...")
        ProgressPanel.OperationNum = 994
                PleaseWaitDialog.Label2.Text = LocalizationService.ForSection("Main.GetFeatures")("Getting.Feature.Names.Label")
        If Not CompletedTasks(1) Then
            DynaLog.LogMessage("Feature background processes haven't completed.")
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        StopMountedImageDetector()
        GetFeatureInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub GetCapabilities_Click(sender As Object, e As EventArgs) Handles GetCapabilities.Click
        DynaLog.LogMessage("Checking edition and version information for any unmet requirements...")
        If (CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise) Or Not IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            DynaLog.LogMessage("The image is not supported")
                    MsgBox(LocalizationService.ForSection("Main.GetCapabilities.Actions")("UnsupportedImage.Message"), vbOKOnly + vbCritical, Text)
            Exit Sub
        End If
        DynaLog.LogMessage("All requirements are met. Continuing with the task...")
        DynaLog.LogMessage("Opening capability information dialog...")
        ProgressPanel.OperationNum = 994
                PleaseWaitDialog.Label2.Text = LocalizationService.ForSection("Main.GetCapabilities")("Cap.Names.Their.Label")
        If Not CompletedTasks(3) Then
            DynaLog.LogMessage("Capability background processes haven't completed.")
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        StopMountedImageDetector()
        GetCapabilityInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub GetPackages_Click(sender As Object, e As EventArgs) Handles GetPackages.Click
        DynaLog.LogMessage("Opening OS package information dialog...")
        ProgressPanel.OperationNum = 993
                PleaseWaitDialog.Label2.Text = LocalizationService.ForSection("Main.GetPackages")("Getting.Package.Names.Label")
        If Not CompletedTasks(0) Then
            DynaLog.LogMessage("OS package background processes haven't completed.")
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        StopMountedImageDetector()
        GetPkgInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub GetProvisionedAppxPackages_Click(sender As Object, e As EventArgs) Handles GetProvisionedAppxPackages.Click
        DynaLog.LogMessage("Checking edition and version information for any unmet requirements...")
        If (CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise) Or Not IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            DynaLog.LogMessage("The image is not supported")
                    MsgBox(LocalizationService.ForSection("Main.ProvAppx")("UnsupportedImage.Message"), vbOKOnly + vbCritical, Text)
            Exit Sub
        End If
        DynaLog.LogMessage("All requirements are met. Continuing with the task...")
        DynaLog.LogMessage("Opening AppX package information dialog...")
        ProgressPanel.OperationNum = 993
                PleaseWaitDialog.Label2.Text = LocalizationService.ForSection("Main.GetProvAppx")("Getting.Package.Names.Label")
        If Not CompletedTasks(2) Then
            DynaLog.LogMessage("AppX package background processes haven't completed.")
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        GetAppxPkgInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub SaveResourceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveResourceToolStripMenuItem.Click
        DynaLog.LogMessage("Triggering logo asset resource save...")
        If OnlineManagement Then AppxResSFD.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) Else AppxResSFD.InitialDirectory = projPath
        AppxResSFD.FileName = If(GetAppxPkgInfoDlg.displayName <> "", GetAppxPkgInfoDlg.displayName, GetAppxPkgInfoDlg.Label25.Text)
        AppxResSFD.ShowDialog(Me)
    End Sub

    Private Sub AppxResSFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles AppxResSFD.FileOk
        Try
            DynaLog.LogMessage("Saving image...")
            DynaLog.LogMessage("Destination: " & AppxResSFD.FileName)
            GetAppxPkgInfoDlg.PictureBox2.Image.Save(AppxResSFD.FileName, Imaging.ImageFormat.Png)
            Notifications.Visible = True
            Notifications.Icon = Icon
            Notifications.BalloonTipText = LocalizationService.ForSection("Main.SaveAsset")("Saved.Location.Label")
            Notifications.BalloonTipTitle = LocalizationService.ForSection("Main.SaveAsset")("SaveSuccessful.Title")
            Notifications.ShowBalloonTip(3000)
        Catch ex As Exception
            DynaLog.LogMessage("Could not save logo asset. Error message: " & ex.Message)
        End Try
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        Try
            DynaLog.LogMessage("Copying image to system clipboard...")
            Dim data As New DataObject()
            data.SetImage(GetAppxPkgInfoDlg.PictureBox2.Image)
            Clipboard.SetDataObject(data, True)
            Notifications.Visible = True
            Notifications.Icon = Icon
            Notifications.BalloonTipText = LocalizationService.ForSection("Main.CopyAsset")("Copied.Clipboard.Label")
            Notifications.BalloonTipTitle = LocalizationService.ForSection("Main.CopyAsset")("CopySuccessful.Title")
            Notifications.ShowBalloonTip(3000)
        Catch ex As Exception
            DynaLog.LogMessage("Could not copy logo asset. Error message: " & ex.Message)
        End Try
    End Sub

    Private Sub Notifications_BalloonTipClosed(sender As Object, e As EventArgs) Handles Notifications.BalloonTipClosed
        Notifications.Visible = False
    End Sub

    Private Sub SplitImage_Click(sender As Object, e As EventArgs) Handles SplitImage.Click
        DynaLog.LogMessage("Opening image split dialog...")
        ImgSplit.ShowDialog(Me)
    End Sub

    Private Sub Notifications_BalloonTipClicked(sender As Object, e As EventArgs) Handles Notifications.BalloonTipClicked
        If Notifications.BalloonTipText.Contains("saved") Or Notifications.BalloonTipText.Contains("guardado") Or Notifications.BalloonTipText.Contains("sauvegardé") Or Notifications.BalloonTipText.Contains("salvata") Then
            DynaLog.LogMessage("Opening saved asset...")
            If File.Exists(AppxResSFD.FileName) Then
                Process.Start(AppxResSFD.FileName)
            End If
        End If
    End Sub

    Private Sub ExportDriver_Click(sender As Object, e As EventArgs) Handles ExportDriver.Click
        DynaLog.LogMessage("Opening device driver export dialog...")
        ExportDrivers.ShowDialog(Me)
    End Sub

    Private Sub GetPESettings_Click(sender As Object, e As EventArgs) Handles GetPESettings.Click
        GetWinPESettings.ShowDialog(Me)
    End Sub

    Private Sub SetTargetPath_Click(sender As Object, e As EventArgs) Handles SetTargetPath.Click
        SetPETargetPath.ShowDialog(Me)
    End Sub

    Private Sub SetScratchSpace_Click(sender As Object, e As EventArgs) Handles SetScratchSpace.Click
        SetPEScratchSpace.ShowDialog(Me)
    End Sub

    Private Sub ISFix_Click(sender As Object, e As EventArgs) Handles ISFix.Click
        DynaLog.LogMessage("Showing dialog for invalid settings...")
        InvalidSettingsDialog.ShowDialog(Me)
    End Sub

    Private Sub MicrosoftAppsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MicrosoftAppsToolStripMenuItem.Click
        Process.Start("https://apps.microsoft.com")
    End Sub

    Private Sub MicrosoftStoreGenerationProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MicrosoftStoreGenerationProjectToolStripMenuItem.Click
        Process.Start("https://store.rg-adguard.net")
    End Sub

    Private Sub SaveImageInformationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveImageInformationToolStripMenuItem.Click
        If ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Preparing to save image information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            If CurrentImage Is Nothing Then
                CurrentImage = MountedImageList.FirstOrDefault(Function(mountedImage) mountedImage.ImageMountDirectory = MountDir)
            End If
            ' If it's still nothing then we give up.
            If CurrentImage Is Nothing Then Exit Sub
            DynaLog.LogMessage("Image to get information about: " & CurrentImage.ImageFile)
            ImgInfoSaveDlg.SourceImage = CurrentImage.ImageFile
            ImgInfoSaveDlg.ImgMountDir = If(Not OnlineManagement, MountDir, "")
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.OfflineMode = OfflineManagement
            ImgInfoSaveDlg.AllDrivers = AllDrivers
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = 0
            ImgInfoSaveDlg.ImageToGetInfoFrom = CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Private Sub OfflineInstMgmt_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles OfflineInstMgmt.LinkClicked
        DynaLog.LogMessage("Beginning offline installation management...")
        If OfflineInstDriveLister.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Selected drive path: " & drivePath)
            DynaLog.LogMessage("User has accepted the disk chooser popup.")
            If drivePath = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) Then
                DynaLog.LogMessage("Selected drive is the same as the drive currently booted to. Entering online installation management mode instead...")
                ActiveInstAccessWarn.Label2.Visible = False
                BeginOnlineManagement(True)
                Exit Sub
            End If
            DynaLog.LogMessage("Entering offline installation management mode...")
            BeginOfflineManagement(drivePath)
        End If
    End Sub

    Private Sub ContributeToTheHelpSystemToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ContributeToTheHelpSystemToolStripMenuItem.Click
        Process.Start("https://github.com/CodingWonders/dt_help")
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        TimeLabel.Text = DateTime.Now.ToString("D") & " - " & DateTime.Now.ToString("HH:mm")
    End Sub

    Private Sub LinkLabel12_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel12.LinkClicked
        LinkLabel12.LinkColor = CurrentTheme.ForegroundColor
        LinkLabel13.LinkColor = CurrentTheme.DisabledForegroundColor
        SidePanel_ProjectView.Visible = True
        SidePanel_ImageView.Visible = False
    End Sub

    Private Sub LinkLabel13_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel13.LinkClicked
        LinkLabel12.LinkColor = CurrentTheme.DisabledForegroundColor
        LinkLabel13.LinkColor = CurrentTheme.ForegroundColor
        SidePanel_ProjectView.Visible = False
        SidePanel_ImageView.Visible = True
    End Sub

#Region "Task Links"

    Private Sub LinkLabel14_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel14.LinkClicked
        Button26.PerformClick()
    End Sub

    Private Sub LinkLabel15_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel15.LinkClicked
                ProjProperties.ImageTaskHeader1.ItemText = LocalizationService.ForSection("Main.Links")("Props.Label")
        If Environment.OSVersion.Version.Major = 10 Then
            ProjProperties.Text = ""
        Else
            ProjProperties.Text = ProjProperties.ImageTaskHeader1.ItemText
        End If
        DynaLog.LogMessage("Showing project/image properties...")
        ProjProperties.ShowDialog(Me)
    End Sub

    Private Sub LinkLabel16_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel16.LinkClicked
        DynaLog.LogMessage("Opening project location in File Explorer...")
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", "/select," & Quote & projPath & "\" & Label49.Text & ".dtproj" & Quote)
    End Sub

    Private Sub LinkLabel17_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel17.LinkClicked
        ToolStripButton3.PerformClick()
    End Sub

    Private Sub LinkLabel18_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel18.LinkClicked
        DynaLog.LogMessage("Opening popup mounted image picker...")
        Dim selectedImage As WindowsImage = PopupMountedImagePicker.PickImage()
        If selectedImage IsNot Nothing Then
            DynaLog.LogMessage("User accepted the popup.")
            MountDir = selectedImage.ImageMountDirectory
            Dim ImageToLoad As WindowsImage = MountedImageList.FirstOrDefault(Function(image) image.ImageMountDirectory = MountDir)
            If ImageToLoad IsNot Nothing Then
                SourceImg = ImageToLoad.ImageFile
                ImgIndex = ImageToLoad.ImageIndex
                isReadOnly = ImageToLoad.ImageMountMode = DismMountMode.ReadOnly
            End If
            DynaLog.LogMessage("Information obtained about the image to load here:")
            DynaLog.LogMessage("- Image file: " & SourceImg)
            DynaLog.LogMessage("- Image index: " & ImgIndex)
            DynaLog.LogMessage("- Mount directory: " & MountDir)
            DynaLog.LogMessage("- Loaded with read/write privileges? " & If(isReadOnly, "No", "Yes"))
            UpdateProjProperties(True, isReadOnly)
            SaveDTProj()
        End If
    End Sub

    Private Sub LinkLabel19_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel19.LinkClicked
        ' If it's a read only image, directly unmount it discarding changes
        If CurrentImage.ImageMountMode = DismMountMode.ReadOnly Then
            DynaLog.LogMessage("The image that is about to be unmounted is mounted with read-only permissions. Committing changes to this image makes no sense.")
            DynaLog.LogMessage("Unmounting image directly...")
            If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
            imgCommitOperation = 1
            UnloadDTProj(False, True)
            Exit Sub
        End If
        DynaLog.LogMessage("Opening image unmount dialog...")
        ImgUMount.RadioButton1.Checked = True
        ImgUMount.RadioButton2.Checked = False
        ImgUMount.ShowDialog(Me)
    End Sub

    Private Sub LinkLabel20_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel20.LinkClicked
                ProjProperties.ImageTaskHeader1.ItemText = LocalizationService.ForSection("Main.Links")("ProjProps.Label")
        If Environment.OSVersion.Version.Major = 10 Then
            ProjProperties.Text = ""
        Else
            ProjProperties.Text = ProjProperties.ImageTaskHeader1.ItemText
        End If
        DynaLog.LogMessage("Showing project/image properties...")
        ProjProperties.ShowDialog(Me)
    End Sub

    Private Sub LinkLabel21_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel21.LinkClicked
        DynaLog.LogMessage("Opening image mount dialog...")
        ImgMount.ShowDialog(Me)
    End Sub

#End Region

#Region "Common Task button functionality in new design"

    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles Button24.Click
        ImgIndexSwitch.ShowDialog(Me)
    End Sub

    Private Sub Button25_Click(sender As Object, e As EventArgs) Handles Button25.Click
        DynaLog.LogMessage("Reloading servicing session of image...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MountDir
        ProgressPanel.OperationNum = 18
        ProgressPanel.ShowDialog(Me)
    End Sub

    Private Sub Button26_Click(sender As Object, e As EventArgs) Handles Button26.Click
        DynaLog.LogMessage("Opening image mount dialog...")
        StopMountedImageDetector()
        ImgMount.ShowDialog(Me)
    End Sub

    Private Sub Button27_Click(sender As Object, e As EventArgs) Handles Button27.Click
        DynaLog.LogMessage("Committing changes to the image...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        Dim IsInFfuMode As Boolean

        If Path.GetExtension(CurrentImage.ImageFile).Equals(".ffu", StringComparison.OrdinalIgnoreCase) Then
            IsInFfuMode = True

            ' We have to do all of this because FFUs can't be saved normally. The workaround is to capture it into a new file,
            ' unmount the old FFU, replace it with the new one, and mount that one... it will be considered a "new" FFU file,
            ' but at least we save the changes...

            Dim tempFfuPath As String = String.Format("capturedFFU_{0}.ffu", New Random().Next(Integer.MaxValue))

            ' Options for capture task
            ProgressPanel.FFUCaptureSourceDrive = CurrentImage.FFUInfo.MountDiskPath
            ProgressPanel.FFUCaptureDestinationFfuImage = Path.Combine(Path.GetTempPath(), tempFfuPath)
            ProgressPanel.FFUCaptureName = CurrentImage.ImageName
            ProgressPanel.FFUCaptureDescription = CurrentImage.ImageDescription
            ProgressPanel.FFUCaptureCompressType = 1

            ' Options for unmount task
            ProgressPanel.MountDir = MountDir
            ProgressPanel.UMountOp = 1
            ProgressPanel.UMountLocalDir = True
            ProgressPanel.RandomMountDir = ""
            ProgressPanel.CheckImgIntegrity = False
            ProgressPanel.SaveToNewIndex = False
            ProgressPanel.UMountImgIndex = 1

            ' Options for replace task
            ProgressPanel.FFUReplaceSourceFFU = Path.Combine(Path.GetTempPath(), tempFfuPath)
            ProgressPanel.FFUReplaceDestinationFFU = CurrentImage.ImageFile

            ' Options for mount task
            ProgressPanel.SourceImg = CurrentImage.ImageFile
            ProgressPanel.ImgIndex = 1
            ProgressPanel.isReadOnly = False
            ProgressPanel.isOptimized = False
            ProgressPanel.isIntegrityTested = False

            ProgressPanel.TaskList.AddRange({5, 21, 998, 15})
        Else
            IsInFfuMode = False

            ProgressPanel.MountDir = MountDir
            ' TODO: Add additional options later
            ProgressPanel.OperationNum = 8
        End If
        ProgressPanel.ShowDialog(Me)
        If IsInFfuMode Then
            UpdateProjProperties(True, False)
            SaveDTProj()
        End If
    End Sub

    Private Sub Button28_Click(sender As Object, e As EventArgs) Handles Button28.Click
        DynaLog.LogMessage("Unmounting the Windows image whilst committing changes...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        imgCommitOperation = 0
        UnloadDTProj(False, True)
    End Sub

    Private Sub Button29_Click(sender As Object, e As EventArgs) Handles Button29.Click
        DynaLog.LogMessage("Unmounting the Windows image whilst discarding changes...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        imgCommitOperation = 1
        UnloadDTProj(False, True)
    End Sub

    Private Sub Button30_Click(sender As Object, e As EventArgs) Handles Button30.Click
        DynaLog.LogMessage("Opening image application dialog...")
        Dim cmsPos As Point = Button30.PointToScreen(Point.Empty)
        cmsPos.Offset(0, Button30.Height)
        ImgApplyModeCMS.Show(cmsPos)
    End Sub

    Private Sub Button31_Click(sender As Object, e As EventArgs) Handles Button31.Click
        DynaLog.LogMessage("Opening image capture dialog...")
        Dim cmsPos As Point = Button31.PointToScreen(Point.Empty)
        cmsPos.Offset(0, Button31.Height)
        ImgCaptureModeCMS.Show(cmsPos)
    End Sub

    Private Sub Button32_Click(sender As Object, e As EventArgs) Handles Button32.Click
        DynaLog.LogMessage("Opening volume image removal dialog...")
        ImgIndexDelete.ShowDialog(Me)
    End Sub

    Private Sub Button33_Click(sender As Object, e As EventArgs) Handles Button33.Click
        If ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Preparing to save image information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            If CurrentImage Is Nothing Then
                CurrentImage = MountedImageList.FirstOrDefault(Function(mountedImage) mountedImage.ImageMountDirectory = MountDir)
            End If
            ' If it's still nothing then we give up.
            If CurrentImage Is Nothing Then Exit Sub
            DynaLog.LogMessage("Image to get information about: " & CurrentImage.ImageFile)
            ImgInfoSaveDlg.SourceImage = CurrentImage.ImageFile
            ImgInfoSaveDlg.ImgMountDir = If(Not OnlineManagement, MountDir, "")
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.OfflineMode = OfflineManagement
            ImgInfoSaveDlg.AllDrivers = AllDrivers
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = 0
            ImgInfoSaveDlg.ImageToGetInfoFrom = CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Private Sub Button34_Click(sender As Object, e As EventArgs) Handles Button34.Click
        DynaLog.LogMessage("Opening OS package information dialog...")
        ProgressPanel.OperationNum = 993
                PleaseWaitDialog.Label2.Text = LocalizationService.ForSection("Main")("Getting.Package.Names.Label")
        If Not CompletedTasks(0) Then
            DynaLog.LogMessage("OS package background processes haven't completed.")
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        StopMountedImageDetector()
        GetPkgInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub Button35_Click(sender As Object, e As EventArgs) Handles Button35.Click
        RemPackage.ShowDialog(Me)
    End Sub

    Private Sub Button36_Click(sender As Object, e As EventArgs) Handles Button36.Click
        DynaLog.LogMessage("Opening package addition dialog...")
        AddPackageDlg.ShowDialog(Me)
    End Sub

    Private Sub Button37_Click(sender As Object, e As EventArgs) Handles Button37.Click
        DynaLog.LogMessage("Opening image cleanup dialog...")
        ImgCleanup.ShowDialog(Me)
    End Sub

    Private Sub Button38_Click(sender As Object, e As EventArgs) Handles Button38.Click
        If ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Saving installed package information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            If ImgInfoSaveDlg.PackageFiles.Count > 0 Then ImgInfoSaveDlg.PackageFiles.Clear()
            ImgInfoSaveDlg.SourceImage = SourceImg
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            ImgInfoSaveDlg.ImgMountDir = If(Not OnlineManagement, MountDir, "")
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = 2
            ImgInfoSaveDlg.ImageToGetInfoFrom = CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Private Sub Button39_Click(sender As Object, e As EventArgs) Handles Button39.Click
        If Not IsImageMounted Then Exit Sub
        DynaLog.LogMessage("Opening feature information dialog...")
        ProgressPanel.OperationNum = 994
                PleaseWaitDialog.Label2.Text = LocalizationService.ForSection("Main")("Getting.Feature.Names.Label")
        If Not CompletedTasks(1) Then
            DynaLog.LogMessage("Feature background processes haven't completed.")
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        StopMountedImageDetector()
        GetFeatureInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub Button40_Click(sender As Object, e As EventArgs) Handles Button40.Click
        DisableFeat.ShowDialog(Me)
    End Sub

    Private Sub Button41_Click(sender As Object, e As EventArgs) Handles Button41.Click
        EnableFeat.ShowDialog(Me)
    End Sub

    Private Sub Button42_Click(sender As Object, e As EventArgs) Handles Button42.Click
        If ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Saving feature information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = SourceImg
            ImgInfoSaveDlg.ImgMountDir = If(Not OnlineManagement, MountDir, "")
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.OfflineMode = OfflineManagement
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = 4
            ImgInfoSaveDlg.ImageToGetInfoFrom = CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Private Sub Button43_Click(sender As Object, e As EventArgs) Handles Button43.Click
        RemProvAppxPackage.ShowDialog(Me)
    End Sub

    Private Sub Button44_Click(sender As Object, e As EventArgs) Handles Button44.Click
        AddProvAppxPackage.ShowDialog(Me)
    End Sub

    Private Sub Button45_Click(sender As Object, e As EventArgs) Handles Button45.Click
        DynaLog.LogMessage("Checking edition and version information for any unmet requirements...")
        If (CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise) Or Not IsWindows8OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            DynaLog.LogMessage("The image is not supported")
                    MsgBox(LocalizationService.ForSection("Main.Actions")("UnsupportedImage.Message"), vbOKOnly + vbCritical, Text)
            Exit Sub
        End If
        DynaLog.LogMessage("All requirements are met. Continuing with the task...")
        DynaLog.LogMessage("Opening AppX package information dialog...")
        ProgressPanel.OperationNum = 993
                PleaseWaitDialog.Label2.Text = LocalizationService.ForSection("Main")("Wait.Label")
        If Not CompletedTasks(2) Then
            DynaLog.LogMessage("AppX package background processes haven't completed.")
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        GetAppxPkgInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub Button46_Click(sender As Object, e As EventArgs) Handles Button46.Click
        If ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Saving installed AppX package information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = SourceImg
            ImgInfoSaveDlg.ImgMountDir = If(Not OnlineManagement, MountDir, "")
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.OfflineMode = OfflineManagement
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = 5
            ImgInfoSaveDlg.ImageToGetInfoFrom = CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Private Sub Button47_Click(sender As Object, e As EventArgs) Handles Button47.Click
        RemCapabilities.ShowDialog(Me)
    End Sub

    Private Sub Button48_Click(sender As Object, e As EventArgs) Handles Button48.Click
        AddCapabilities.ShowDialog(Me)
    End Sub

    Private Sub Button49_Click(sender As Object, e As EventArgs) Handles Button49.Click
        DynaLog.LogMessage("Checking edition and version information for any unmet requirements...")
        If (CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise) Or Not IsWindows10OrHigher(MountDir & "\Windows\system32\ntoskrnl.exe") Then
            DynaLog.LogMessage("The image is not supported")
                    MsgBox(LocalizationService.ForSection("Main.Actions")("UnsupportedImage.Message"), vbOKOnly + vbCritical, Text)
            Exit Sub
        End If
        DynaLog.LogMessage("All requirements are met. Continuing with the task...")
        DynaLog.LogMessage("Opening capability information dialog...")
        ProgressPanel.OperationNum = 994
                PleaseWaitDialog.Label2.Text = LocalizationService.ForSection("Main")("Get.Cap.Names.Label")
        If Not CompletedTasks(3) Then
            DynaLog.LogMessage("Capability background processes haven't completed.")
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        StopMountedImageDetector()
        GetCapabilityInfoDlg.ShowDialog(Me)
    End Sub

    Private Sub Button50_Click(sender As Object, e As EventArgs) Handles Button50.Click
        If ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Saving capability information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = SourceImg
            ImgInfoSaveDlg.ImgMountDir = If(Not OnlineManagement, MountDir, "")
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.OfflineMode = OfflineManagement
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = 6
            ImgInfoSaveDlg.ImageToGetInfoFrom = CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Private Sub Button51_Click(sender As Object, e As EventArgs) Handles Button51.Click
        RemDrivers.ShowDialog(Me)
    End Sub

    Private Sub Button52_Click(sender As Object, e As EventArgs) Handles Button52.Click
        DynaLog.LogMessage("Opening driver information dialog...")
        ProgressPanel.OperationNum = 994
                PleaseWaitDialog.Label2.Text = LocalizationService.ForSection("Main")("Loading.DriverPackages.Label")
        If Not CompletedTasks(4) Then
            DynaLog.LogMessage("Device driver background processes haven't completed.")
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        StopMountedImageDetector()
        GetDriverInfo.ShowDialog(Me)
    End Sub

    Private Sub Button53_Click(sender As Object, e As EventArgs) Handles Button53.Click
        AddDrivers.ShowDialog(Me)
    End Sub

    Private Sub Button54_Click(sender As Object, e As EventArgs) Handles Button54.Click
        If ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Saving installed device driver information...")
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
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = 7
            ImgInfoSaveDlg.ImageToGetInfoFrom = CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Private Sub Button55_Click(sender As Object, e As EventArgs) Handles Button55.Click
        GetWinPESettings.ShowDialog(Me)
    End Sub

    Private Sub Button56_Click(sender As Object, e As EventArgs) Handles Button56.Click
        If ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Saving Windows PE configuration information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = SourceImg
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            ImgInfoSaveDlg.ImgMountDir = If(Not OnlineManagement, MountDir, "")
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = 9
            ImgInfoSaveDlg.ImageToGetInfoFrom = CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Private Sub Button57_Click(sender As Object, e As EventArgs) Handles Button57.Click
        SetPETargetPath.ShowDialog(Me)
    End Sub

    Private Sub Button58_Click(sender As Object, e As EventArgs) Handles Button58.Click
        SetPEScratchSpace.ShowDialog(Me)
    End Sub

#End Region

    Private Function LoadNewsFeedFromUrl(rssUrl As String) As SyndicationFeed
        Dim rssContent As String = ""

        Using client As New WebClient()
            client.Headers(HttpRequestHeader.UserAgent) = "DISMTools/0.8 (+https://github.com/CodingWonders/DISMTools)"
            client.Headers(HttpRequestHeader.Accept) = "application/rss+xml, application/xml;q=0.9, text/xml;q=0.8, */*;q=0.7"
            client.Headers(HttpRequestHeader.AcceptLanguage) = "en-US,en;q=0.9"
            rssContent = client.DownloadString(rssUrl)
        End Using

        If String.IsNullOrWhiteSpace(rssContent) Then Throw New InvalidDataException("The feed response was empty.")

        DynaLog.LogMessage("RSS Feed Content is not nothing. Attempting to create XML reader from content...")
        Using reader As XmlReader = XmlReader.Create(New StringReader(rssContent))
            DynaLog.LogMessage("Loading reader...")
            Return SyndicationFeed.Load(reader)
        End Using
    End Function

    Private Function HasFeedItems(feed As SyndicationFeed) As Boolean
        Return feed IsNot Nothing AndAlso feed.Items IsNot Nothing AndAlso feed.Items.Any()
    End Function

    Private Function GetNewsLastUpdatedText() As String
        Dim currentOSCulture As CultureInfo = CultureInfo.CurrentCulture
        Dim dateText As String = If(HumanizeDates,
                                    String.Format("{0}, {1}", NewsLastUpdateDate.ToString(currentOSCulture.DateTimeFormat.LongDatePattern, currentOSCulture),
                                                              NewsLastUpdateDate.ToString(currentOSCulture.DateTimeFormat.LongTimePattern, currentOSCulture)),
                                    NewsLastUpdateDate.ToString("MM/dd/yyyy HH:mm:ss"))

        Dim lastUpdatedPrefix As String = LocalizationService.ForSection("Main.News")("Last.Updated.Label")

        Return String.Format("{0} {1}", lastUpdatedPrefix.TrimEnd(), dateText)
    End Function

    Private Sub RenderNewsFeed()
        DynaLog.LogMessage("Refreshing news feed...")

        If HasFeedItems(FeedContents) Then
            Label8.Text = GetNewsLastUpdatedText()
            NewsItemCardContainerPanel.Controls.Clear()
            FeedLinks.Clear()

            Dim ValueAddedTop As Integer = WindowHelper.ScaleLogical(8),
                PreviousTop As Integer
            Dim FirstCard As Boolean = True
            Dim sortedArticles As IOrderedEnumerable(Of SyndicationItem) = FeedContents.Items.OrderByDescending(Function(article) article.PublishDate)
            Dim ItemCardControls As New List(Of NewsFeedItemCard)

            For Each Article In sortedArticles
                Dim newsCard As New NewsFeedItemCard()
                newsCard.SetColors()
                newsCard.FeedItemText = Article.Title.Text
                newsCard.FeedItemDate = TimeZoneInfo.ConvertTime(Article.PublishDate.DateTime, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"), TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"))
                newsCard.FeedItemLink = Article.Links(0).Uri.AbsoluteUri
                newsCard.FeedItemContents = CType(Article.Content, TextSyndicationContent).Text
                newsCard.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
                newsCard.Left = WindowHelper.ScaleLogical(8)
                newsCard.Top = If(FirstCard, ValueAddedTop, PreviousTop + newsCard.Height + ValueAddedTop)
                newsCard.Width = NewsItemCardContainerPanel.Width - 32
                FirstCard = False
                PreviousTop = newsCard.Top
                AddHandler newsCard.LinkContentsEvent, AddressOf DisplayFeedItemCardContent
                ItemCardControls.Add(newsCard)
            Next

            NewsItemCardContainerPanel.Controls.AddRange(ItemCardControls.ToArray())
            Panel12.Visible = False
        Else
            If FeedEx IsNot Nothing Then
                DynaLog.LogMessage("Could not get feed news. Error message: " & FeedEx.Message)
            Else
                DynaLog.LogMessage("Could not get feed news. No feed items were returned.")
            End If
            Panel12.Visible = True
        End If
    End Sub

    Sub GetFeedNews()
        DynaLog.LogMessage("Pulling news feed from DISMTools subreddit...")
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        FeedEx = Nothing

        Dim previousFeed As SyndicationFeed = FeedContents
        Dim feedUrls() As String = {
            "https://www.reddit.com/r/DISMTools/.rss",
            "https://old.reddit.com/r/DISMTools/.rss",
            "https://reddit.com/r/DISMTools.rss"
        }
        Dim feedErrors As New List(Of String)
        Dim lastException As Exception = Nothing

        For Each rssUrl As String In feedUrls
            Try
                DynaLog.LogMessage("Trying news feed URL: " & rssUrl)
                Dim loadedFeed As SyndicationFeed = LoadNewsFeedFromUrl(rssUrl)
                If HasFeedItems(loadedFeed) Then
                    FeedContents = loadedFeed
                    NewsLastUpdateDate = Date.Now
                    DynaLog.LogMessage("News feed loaded successfully from: " & rssUrl)
                    Return
                End If
                Dim emptyFeedException As New InvalidDataException("The feed did not contain any items.")
                lastException = emptyFeedException
                feedErrors.Add(String.Format("{0}: {1}", rssUrl, emptyFeedException.Message))
                DynaLog.LogMessage("News feed URL returned no items: " & rssUrl)
            Catch ex As Exception
                lastException = ex
                feedErrors.Add(String.Format("{0}: {1}", rssUrl, ex.Message))
                DynaLog.LogMessage("News feed URL failed: " & rssUrl & ". Error message: " & ex.Message)
            End Try
        Next

        If HasFeedItems(previousFeed) Then FeedContents = previousFeed Else FeedContents = New SyndicationFeed()

        Dim errorDetails As String = If(feedErrors.Any(), String.Join(" | ", feedErrors), "No feed URL returned usable content.")
        FeedEx = New Exception("Could not load DISMTools news feed. " & errorDetails, lastException)
        DynaLog.LogMessage("Failed to get feed news. Error message: " & FeedEx.Message)
    End Sub

    Private Sub HelpTopicsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HelpTopicsToolStripMenuItem.Click
        HelpDocsModule.DisplayHelpDocumentation("docs\index.html")
    End Sub

    Private Sub LinkLabel12_MouseLeave(sender As Object, e As EventArgs) Handles LinkLabel12.MouseLeave
        If SidePanel_ProjectView.Visible Then
            LinkLabel12.LinkColor = CurrentTheme.ForegroundColor
        Else
            LinkLabel12.LinkColor = CurrentTheme.DisabledForegroundColor
        End If
    End Sub

    Private Sub LinkLabel13_MouseLeave(sender As Object, e As EventArgs) Handles LinkLabel13.MouseLeave
        If SidePanel_ImageView.Visible Then
            LinkLabel13.LinkColor = CurrentTheme.ForegroundColor
        Else
            LinkLabel13.LinkColor = CurrentTheme.DisabledForegroundColor
        End If
    End Sub

    Private Sub LinkLabel12_MouseEnter(sender As Object, e As EventArgs) Handles LinkLabel12.MouseEnter
        If LinkLabel12.LinkColor = CurrentTheme.ForegroundColor Then
            Cursor = Cursors.Arrow
            Exit Sub
        Else
            LinkLabel12.LinkColor = CurrentTheme.AccentColors(2)
        End If
    End Sub

    Private Sub LinkLabel13_MouseEnter(sender As Object, e As EventArgs) Handles LinkLabel13.MouseEnter
        If LinkLabel13.LinkColor = CurrentTheme.ForegroundColor Then
            Cursor = Cursors.Arrow
            Exit Sub
        Else
            LinkLabel13.LinkColor = CurrentTheme.AccentColors(2)
        End If
    End Sub

    Private Sub FeedWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles FeedWorker.DoWork
        DynaLog.LogMessage("Detecting if worker needs to be cancelled...")
        If FeedWorker.CancellationPending Then Exit Sub
        DynaLog.LogMessage("Getting feed news...")
        GetFeedNews()
        DynaLog.LogMessage("Reporting feed result to UI.")
        FeedWorker.ReportProgress(0)
        If Not FeedWorker.CancellationPending Then Thread.Sleep(2000)
    End Sub

    Private Sub DisplayFeedItemCardContent(sender As Object, e As NewsFeedItemCardLinkClickedEventArgs)
        NewsFeedTextLabel.Text = e.Title
        Dim currentOSCulture As CultureInfo = CultureInfo.CurrentCulture
        NewsFeedDateLabel.Text = String.Format("{0}, {1}", e.PublishDate.ToString(currentOSCulture.DateTimeFormat.LongDatePattern, currentOSCulture),
                                                           e.PublishDate.ToString(currentOSCulture.DateTimeFormat.LongTimePattern, currentOSCulture))
        ' Do it like this because the IE webbrowser is quirky and doesn't want to change text using its property;
        ' we need to navigate to the blank page. https://stackoverflow.com/a/174483. But, as we pull stuff from
        ' the subreddit, we find that images just show as links to such -- not a good look. Change these too. Additionally,
        ' we'll spice the look up *just* a bit.
        Dim contentStyle As String = "<style>" & CrLf &
                                     "    * {" & CrLf &
                                     "        background-color: " & ColorTranslator.ToHtml(CurrentTheme.BackgroundColor) & ";" & CrLf &
                                     "        color: " & ColorTranslator.ToHtml(CurrentTheme.ForegroundColor) & ";" & CrLf &
                                     "        font-family: " & Quote & "Segoe UI" & Quote & ", Tahoma, Verdana, Arial, Helvetica, sans-serif;" & CrLf &
                                     "    }" & CrLf & CrLf &
                                     "    body {" & CrLf &
                                     "        margin: 8px;" & CrLf &
                                     "    }" & CrLf & CrLf &
                                     "    code {" & CrLf &
                                     "        font-family: " & Quote & LogFont.Replace(Quote, "") & Quote & ", Consolas, " & Quote & "Courier New" & Quote & ";" & CrLf &
                                     "        font-size: " & If(LogFontSize <= 16, LogFontSize, 11) & "pt;" & CrLf &
                                     "    }" & CrLf & CrLf &
                                     "    a {" & CrLf &
                                     "        color: #1E90FF;" & CrLf &
                                     "    }" & CrLf & CrLf &
                                     "    img {" & CrLf &
                                     "        max-width: 70%;" & CrLf &
                                     "    }" & CrLf & CrLf &
                                     "    table {" & CrLf &
                                     "        table-layout: fixed;" & CrLf &
                                     "        width: 100%;" & CrLf &
                                     "    }" & CrLf &
                                     "</style>" & CrLf
        Try
            ' Quotes don't like to be displayed as such by default; we'll help.
            Dim baseContents As String = UTF8.GetString(GetEncoding(1252).GetBytes(e.Contents))

            ' If the post has pictures a column with the first picture will show up. We don't want this.
            If baseContents.StartsWith("<table> <tr><td> <a href=", StringComparison.OrdinalIgnoreCase) Then
                baseContents = baseContents.Replace("<table> <tr><td> <a href=", "<table> <tr><td style=" & Quote & "width: 0px" & Quote & "> <a href=")
            End If

            Dim parsedContents As String = Regex.Replace(baseContents, "<p><a href=" & Quote & "(https?://preview\.redd\.it/[^" & Quote & "]+)" & Quote & ">\1</a></p>", "<p align=" & Quote & "center" & Quote & "><img src=" & Quote & "$1" & Quote & " /></p>")
            NewsFeedContent = contentStyle & parsedContents
        Catch ex As Exception
            NewsFeedContent = contentStyle & e.Contents
        End Try
        NewsFeedWebContent.Navigate("about:blank")
        NewsContentPreviewerPanel.Visible = True
    End Sub

    Private Sub NewsFeedWebContent_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs)
        If e.Url.ToString() <> "about:blank" Then
            Process.Start(e.Url.AbsoluteUri)
            NewsFeedWebContent.Navigate("about:blank")
            NewsFeedWebContent.Document.OpenNew(True)
            Exit Sub
        End If

        NewsFeedWebContent.Document.OpenNew(True)
        NewsFeedWebContent.Document.Write(NewsFeedContent)
    End Sub

    Private Sub FeedWorker_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles FeedWorker.ProgressChanged
        Try
            DynaLog.LogMessage("Items in feed: " & If(FeedContents IsNot Nothing AndAlso FeedContents.Items IsNot Nothing, FeedContents.Items.Count(), 0))
            RenderNewsFeed()
        Catch ex As Exception
            DynaLog.LogMessage("Could not get feed news. Error message: " & ex.Message)
            FeedEx = ex
            Panel12.Visible = True
        End Try
    End Sub

    Private Sub LinkLabel6_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        HelpDocsModule.DisplayHelpDocumentation("docs\getting_started\new_to_servicing.html")
    End Sub

    Private Sub LinkLabel7_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        HelpDocsModule.DisplayHelpDocumentation("docs\getting_started\start.html", "first-steps")
    End Sub

    Private Sub LinkLabel8_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        HelpDocsModule.DisplayHelpDocumentation("docs\getting_started\start.html")
    End Sub

    Private Sub LinkLabel9_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        HelpDocsModule.DisplayHelpDocumentation("docs\getting_started\start.html", "best-practices")
    End Sub

    Private Sub LinkLabel10_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        HelpDocsModule.DisplayHelpDocumentation("docs\img_tasks\info\infodlgs.html")
    End Sub

    Private Sub LinkLabel11_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        HelpDocsModule.DisplayHelpDocumentation("docs\img_tasks\info\infodlgs.html", "saving-image-information")
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        DynaLog.LogMessage("Refreshing news feed...")
        If Not FeedWorker.IsBusy Then FeedWorker.RunWorkerAsync()
    End Sub

    Private Sub WatcherBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles WatcherBW.DoWork
        ImageStatus = ImageWatcher.WatchStatus(SourceImg, MountedImageList)
    End Sub

    Private Sub WatcherBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles WatcherBW.RunWorkerCompleted
        Debug.WriteLine("Image status watcher has finished.")
        Debug.WriteLine("Detected image status: " & ImageStatus.ToString())
        If Not ImageStatus = ImageWatcher.Status.OK Then
            DynaLog.LogMessage("Image status is not OK. Stopping mounted image detector to step in...")
            StopMountedImageDetector()
        End If
        Select Case ImageStatus
            Case ImageWatcher.Status.NeedsRemount
                DynaLog.LogMessage("The image is in need of a servicing session reload.")
                If Not OrphanedMountedImgDialog.IsDisposed Then OrphanedMountedImgDialog.Dispose()
                OrphanedMountedImgDialog.ShowDialog(Me)
                If OrphanedMountedImgDialog.DialogResult = Windows.Forms.DialogResult.OK Then
                    DynaLog.LogMessage("Ready to reload. Doing so...")
                    ProgressPanel.Validate()
                    ProgressPanel.MountDir = MountDir
                    ProgressPanel.OperationNum = 18
                    ProgressPanel.ShowDialog(Me)
                    If ProgressPanel.IsSuccessful Then ImageStatus = ImageWatcher.Status.OK
                ElseIf OrphanedMountedImgDialog.DialogResult = Windows.Forms.DialogResult.Cancel Then
                    DynaLog.LogMessage("Not ready to reload. The image needs to be reloading before using this project again. Unloading project...")
                    UnloadDTProj(False, False)
                    If ImgBW.IsBusy Then ImgBW.CancelAsync()
                End If
            Case ImageWatcher.Status.NotMounted
                If IsImageMounted Then
                    DynaLog.LogMessage("The image is no longer mounted. The project needs to be reconfigured")
                    If Not ReloadProjectQuestionDialog.IsDisposed Then ReloadProjectQuestionDialog.Dispose()
                    ReloadProjectQuestionDialog.ShowDialog(Me)
                    If ReloadProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.OK Then
                        DynaLog.LogMessage("Ready to reconfigure. Doing so...")
                        UpdateProjProperties(False, False)
                    ElseIf ReloadProjectQuestionDialog.DialogResult = Windows.Forms.DialogResult.Cancel Then
                        DynaLog.LogMessage("Not ready to reconfigure. Unloading project...")
                        UnloadDTProj(False, False)
                        If ImgBW.IsBusy Then ImgBW.CancelAsync()
                    End If
                End If
        End Select
    End Sub

    Private Sub WatcherTimer_Tick(sender As Object, e As EventArgs) Handles WatcherTimer.Tick
        If isProjectLoaded And IsImageMounted And Not OnlineManagement And Not OfflineManagement Then
            If Not WatcherBW.IsBusy Then WatcherBW.RunWorkerAsync()
        End If
    End Sub

    Private Sub ImportDriver_Click(sender As Object, e As EventArgs) Handles ImportDriver.Click
        ImportDrivers.ShowDialog(Me)
    End Sub

    Private Sub AppxDownloadHelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AppxDownloadHelpToolStripMenuItem.Click
        HelpDocsModule.DisplayHelpDocumentation("docs\img_tasks\appx\add_provisionedappxpackage.html", "questions")
    End Sub

    Function CheckOSUninstallCapability() As Boolean
        DynaLog.LogMessage("Getting OS uninstallation capabilities...")
        Try
            Dim uninstReg As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\Setup")
            Dim uninstFlag As Integer = CInt(uninstReg.GetValue("UninstallActive").ToString())
            uninstReg.Close()
            DynaLog.LogMessage("Uninstallation is active: " & uninstFlag)
            Return (uninstFlag = 1)
        Catch ex As Exception
            DynaLog.LogMessage("Could not get uninstallation capabilities. Error message: " & ex.Message)
            Return False
        End Try
        Return False
    End Function

    Private Sub SetOSUninstallWindow_Click(sender As Object, e As EventArgs) Handles SetOSUninstallWindow.Click
        SetOSUninstWindow.ShowDialog(Me)
    End Sub

    Private Sub GetOSUninstallWindow_Click(sender As Object, e As EventArgs) Handles GetOSUninstallWindow.Click
        If OnlineManagement Then
            DynaLog.LogMessage("The active installation is being managed right now. Checking if it can uninstall an OS...")
            Try
                If Not CheckOSUninstallCapability() Then
                    DynaLog.LogMessage("No rollbacks/uninstallations can be performed.")
                    OSNoRollbackErrorDlg.ShowDialog(Me)
                    Exit Sub
                End If
                Dim osUninstReg As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\Setup")
                Dim RollbackDays As Integer = CInt(osUninstReg.GetValue("UninstallWindow").ToString())
                osUninstReg.Close()
                DynaLog.LogMessage("OS Uninstallation Window: " & RollbackDays & " day(s)")
                Dim msg As String = ""
                msg = LocalizationService.ForSection("Main.Get.OS").Format("Days.Go.Back.Message", RollbackDays)
                MsgBox(msg, vbOKOnly + vbInformation, Text)
            Catch ex As Exception
                Exit Sub
            End Try
        Else
            DynaLog.LogMessage("The active installation is not being managed right now.")
                    MsgBox(LocalizationService.ForSection("Main.OSUninstallWindow")("OnlineOnly.Message"), vbOKOnly + vbCritical, Text)
        End If
    End Sub

    Private Sub InitiateOSUninstall_Click(sender As Object, e As EventArgs) Handles InitiateOSUninstall.Click
        If OnlineManagement Then
            DynaLog.LogMessage("The active installation is being managed right now. Checking if it can uninstall an OS...")
            Try
                If Not CheckOSUninstallCapability() Then
                    DynaLog.LogMessage("No rollbacks/uninstallations can be performed.")
                    OSNoRollbackErrorDlg.ShowDialog(Me)
                    Exit Sub
                End If
                Dim msg As String = ""
                msg = LocalizationService.ForSection("Main.StartOSUninstall").Format("ReadCarefully.Message", Environment.UserName)
                If MsgBox(msg, vbYesNo + vbExclamation, Text) = MsgBoxResult.Yes Then
                    DynaLog.LogMessage("User accepted the question. Proceeding with OS uninstallation...")
                    If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
                    ProgressPanel.OperationNum = 86
                    ProgressPanel.ShowDialog(Me)
                    Close()
                Else
                    DynaLog.LogMessage("User didn't accept the question. Exiting...")
                    Exit Sub
                End If
            Catch ex As Exception
                DynaLog.LogMessage("An error occurred. Error message: " & ex.Message)
                Exit Sub
            End Try
        Else
            DynaLog.LogMessage("The active installation is not being managed right now.")
                    MsgBox(LocalizationService.ForSection("Main.OSUninstall")("OnlineOnly.Message"), vbOKOnly + vbCritical, Text)
        End If
    End Sub

    Private Sub RemoveOSUninstall_Click(sender As Object, e As EventArgs) Handles RemoveOSUninstall.Click
        If OnlineManagement Then
            DynaLog.LogMessage("The active installation is being managed right now. Checking if it can uninstall an OS...")
            Try
                If Not CheckOSUninstallCapability() Then
                    DynaLog.LogMessage("No rollbacks/uninstallations can be performed.")
                    OSNoRollbackErrorDlg.ShowDialog(Me)
                    Exit Sub
                End If
                Dim msg As String = ""
                msg = LocalizationService.ForSection("Main.RemoveOSUninstall").Format("ReadCarefully.Message", Environment.UserName)
                If MsgBox(msg, vbYesNo + vbExclamation, Text) = MsgBoxResult.Yes Then
                    DynaLog.LogMessage("User accepted the question. Proceeding with removal of OS uninstallation capability...")
                    If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
                    ProgressPanel.OperationNum = 87
                    ProgressPanel.ShowDialog(Me)
                Else
                    DynaLog.LogMessage("User didn't accept the question. Exiting...")
                    Exit Sub
                End If
            Catch ex As Exception
                DynaLog.LogMessage("An error occurred. Error message: " & ex.Message)
                Exit Sub
            End Try
        Else
            DynaLog.LogMessage("The active installation is not being managed right now.")
                    MsgBox(LocalizationService.ForSection("Main.RemoveOSUninstall")("OnlineOnly.Message"), vbOKOnly + vbCritical, Text)
        End If
    End Sub

    Private Sub RecentsLV_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles RecentsLV.MouseDoubleClick
        DynaLog.LogMessage("Selected items: " & RecentsLV.SelectedItems.Count)
        If RecentsLV.SelectedItems.Count = 1 Then
            DynaLog.LogMessage("Selected item count is 1. Continuing...")
            Dim itmOrder As Integer = 0
            If RecentList(RecentsLV.FocusedItem.Index).ProjPath <> "" And File.Exists(RecentList(RecentsLV.FocusedItem.Index).ProjPath) Then
                DynaLog.LogMessage("Selected item is not bogus and exists. Loading project...")
                itmOrder = RecentsLV.FocusedItem.Index
                Dim recentProj As Recents = RecentList(itmOrder)
                DynaLog.LogMessage("Selected project: " & recentProj.ToString())
                DynaLog.LogMessage("Reordering projects in recents list...")
                ChangeRecentListOrder(recentProj, itmOrder)
                ProgressPanel.OperationNum = 990
                LoadDTProj(recentProj.ProjPath,
                           If(recentProj.ProjName <> "",
                              recentProj.ProjName,
                              Path.GetFileNameWithoutExtension(recentProj.ProjPath)),
                           True, False)
            End If
            RecentRemoveLink.Visible = False
            Try
                RecentProject1ToolStripMenuItem.Text = " "
                RecentProject2ToolStripMenuItem.Text = " "
                RecentProject3ToolStripMenuItem.Text = " "
                RecentProject4ToolStripMenuItem.Text = " "
                RecentProject5ToolStripMenuItem.Text = " "
                RecentProject6ToolStripMenuItem.Text = " "
                RecentProject7ToolStripMenuItem.Text = " "
                RecentProject8ToolStripMenuItem.Text = " "
                RecentProject9ToolStripMenuItem.Text = " "
                RecentProject10ToolStripMenuItem.Text = " "

                ' Reconfigure text
                RecentProject1ToolStripMenuItem.Text = RecentList(0).ProjPath
                RecentProject1ToolStripMenuItem.Visible = True
                RecentProject2ToolStripMenuItem.Text = RecentList(1).ProjPath
                RecentProject2ToolStripMenuItem.Visible = True
                RecentProject3ToolStripMenuItem.Text = RecentList(2).ProjPath
                RecentProject3ToolStripMenuItem.Visible = True
                RecentProject4ToolStripMenuItem.Text = RecentList(3).ProjPath
                RecentProject4ToolStripMenuItem.Visible = True
                RecentProject5ToolStripMenuItem.Text = RecentList(4).ProjPath
                RecentProject5ToolStripMenuItem.Visible = True
                RecentProject6ToolStripMenuItem.Text = RecentList(5).ProjPath
                RecentProject6ToolStripMenuItem.Visible = True
                RecentProject7ToolStripMenuItem.Text = RecentList(6).ProjPath
                RecentProject7ToolStripMenuItem.Visible = True
                RecentProject8ToolStripMenuItem.Text = RecentList(7).ProjPath
                RecentProject8ToolStripMenuItem.Visible = True
                RecentProject9ToolStripMenuItem.Text = RecentList(8).ProjPath
                RecentProject9ToolStripMenuItem.Visible = True
                RecentProject10ToolStripMenuItem.Text = RecentList(9).ProjPath
                RecentProject10ToolStripMenuItem.Visible = True
            Catch ex As Exception
                ' Don't do anything special here
            End Try
        End If
    End Sub

    Private Sub RecentsLV_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RecentsLV.SelectedIndexChanged
        RecentRemoveLink.Visible = RecentsLV.SelectedItems.Count = 1
    End Sub

    Private Sub RecentRemoveLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles RecentRemoveLink.LinkClicked
        Dim itmOrder As Integer = 0
        itmOrder = RecentsLV.FocusedItem.Index
        DynaLog.LogMessage("Removing entry at index " & itmOrder & "...")
        RecentsLV.Items.Clear()
        RecentList.RemoveAt(itmOrder)
        For Each recentProject In RecentList
            recentProject.Order = RecentList.IndexOf(recentProject)
            RecentsLV.Items.Add(If(recentProject.ProjName <> "", recentProject.ProjName, Path.GetFileNameWithoutExtension(recentProject.ProjPath)))
        Next
        RecentRemoveLink.Visible = False
        Try
            RecentProject1ToolStripMenuItem.Text = " "
            RecentProject2ToolStripMenuItem.Text = " "
            RecentProject3ToolStripMenuItem.Text = " "
            RecentProject4ToolStripMenuItem.Text = " "
            RecentProject5ToolStripMenuItem.Text = " "
            RecentProject6ToolStripMenuItem.Text = " "
            RecentProject7ToolStripMenuItem.Text = " "
            RecentProject8ToolStripMenuItem.Text = " "
            RecentProject9ToolStripMenuItem.Text = " "
            RecentProject10ToolStripMenuItem.Text = " "

            ' Reconfigure text
            RecentProject1ToolStripMenuItem.Text = RecentList(0).ProjPath
            RecentProject1ToolStripMenuItem.Visible = True
            RecentProject2ToolStripMenuItem.Text = RecentList(1).ProjPath
            RecentProject2ToolStripMenuItem.Visible = True
            RecentProject3ToolStripMenuItem.Text = RecentList(2).ProjPath
            RecentProject3ToolStripMenuItem.Visible = True
            RecentProject4ToolStripMenuItem.Text = RecentList(3).ProjPath
            RecentProject4ToolStripMenuItem.Visible = True
            RecentProject5ToolStripMenuItem.Text = RecentList(4).ProjPath
            RecentProject5ToolStripMenuItem.Visible = True
            RecentProject6ToolStripMenuItem.Text = RecentList(5).ProjPath
            RecentProject6ToolStripMenuItem.Visible = True
            RecentProject7ToolStripMenuItem.Text = RecentList(6).ProjPath
            RecentProject7ToolStripMenuItem.Visible = True
            RecentProject8ToolStripMenuItem.Text = RecentList(7).ProjPath
            RecentProject8ToolStripMenuItem.Visible = True
            RecentProject9ToolStripMenuItem.Text = RecentList(8).ProjPath
            RecentProject9ToolStripMenuItem.Visible = True
            RecentProject10ToolStripMenuItem.Text = RecentList(9).ProjPath
            RecentProject10ToolStripMenuItem.Visible = True
        Catch ex As Exception
            ' Don't do anything special here
        End Try

        ' An empty item will appear at the end. Make it hidden
        Dim recItems = RecentProjectsListMenu.DropDownItems
        Dim remItems As IEnumerable(Of ToolStripMenuItem) = Enumerable.OfType(Of ToolStripMenuItem)(recItems)
        Try
            For Each dropDownItem As ToolStripDropDownItem In remItems
                If dropDownItem.Text = " " Then
                    dropDownItem.Visible = False
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ExportImage_Click(sender As Object, e As EventArgs) Handles ExportImage.Click
        DynaLog.LogMessage("Opening image export dialog...")
        ImgExport.ShowDialog(Me)
    End Sub

    Private Sub CleanupMountpoints_Click(sender As Object, e As EventArgs) Handles CleanupMountpoints.Click
        DynaLog.LogMessage("Preparing to clean up mount points...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.OperationNum = 7
        ProgressPanel.ShowDialog(Me)
    End Sub

    Sub LoadVideo(ID As String, Name As String, Description As String)
        DynaLog.LogMessage("Deleting existing video player HTML file (if it exists) and recreating from template...")
        If File.Exists(Application.StartupPath & "\videos\videoplay.html") Then File.Delete(Application.StartupPath & "\videos\videoplay.html")
        If File.Exists(Application.StartupPath & "\videos\videoplay_tmp.html") Then
            DynaLog.LogMessage("Reading HTML...")
            Dim videoPlayerContents As String = File.ReadAllText(Application.StartupPath & "\videos\videoplay_tmp.html")
            DynaLog.LogMessage("Modifying HTML according following values:")
            DynaLog.LogMessage("- Video ID: " & ID)
            DynaLog.LogMessage("- Video Name: " & Name)
            DynaLog.LogMessage("- Video Description: " & Description)
            videoPlayerContents = videoPlayerContents.Replace("{#REPLACEME}", ID).Trim().Replace("{#NAME}", Name).Trim().Replace("{#DESCRIPTION}", Description).Trim()
            ' Set appropriate color mode in light theme
            DynaLog.LogMessage("Setting colors...")
            If Not CurrentTheme.IsDark Then
                videoPlayerContents = videoPlayerContents.Replace("<body class=" & Quote & "pagebody-dark" & Quote & ">", "<body class=" & Quote & "pagebody" & Quote & ">").Trim()
            End If
            File.WriteAllText(Application.StartupPath & "\videos\videoplay.html", videoPlayerContents, UTF8)
            ' Check emulation mode settings of IE for DISMTools and set them to IE11 (+Edge) (if not detected)
            DynaLog.LogMessage("Checking Internet Explorer browser emulation settings (necessary step for you to watch videos on web browser controls)...")
            Try
                Dim IECompatRk As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", True)
                Dim IECompatInt As Integer = IECompatRk.GetValue("DISMTools.exe", -1)
                DynaLog.LogMessage("Browser emulation setting level: " & IECompatInt)
                If IECompatInt <> 11001 Then
                    DynaLog.LogMessage("Browser emulation setting level < 11001 (IE 11+Edge). Setting value...")
                    IECompatRk.SetValue("DISMTools.exe", 11001, RegistryValueKind.DWord)
                    DynaLog.LogMessage("Value set. A program restart is necessary.")
                    MsgBox(LocalizationService.ForSection("Main.Messages")("IE.Emulation.Changed.Message"), vbOKOnly + vbInformation, "DISMTools")
                    IECompatRk.Close()
                    Exit Sub
                End If
                IECompatRk.Close()
            Catch ex As Exception
                DynaLog.LogMessage("Could not detect/modify IE browser emulation settings. Error message: " & ex.Message)
                MsgBox(LocalizationService.ForSection("Main.Messages")("DISM.Tools.Modify.Message"), vbOKOnly + vbCritical, "DISMTools")
                Exit Sub
            End Try
            If Not videoServer.IsListenerAlive Then videoServer.StartServer()
            If videoServer.IsListenerAlive() Then
                Process.Start("http://localhost:2026/videoplay.html")
            End If
        End If
    End Sub

    Private Sub ListView2_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView2.MouseDoubleClick
        LoadVideo(VideoList(ListView2.FocusedItem.Index).YT_ID,
                  VideoList(ListView2.FocusedItem.Index).VideoName,
                  VideoList(ListView2.FocusedItem.Index).VideoDesc)
    End Sub

    Private Sub MainForm_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' Alt-B (Background process panel)
        If e.KeyCode = Keys.B And e.Alt Then
            If Not HomePanel.Visible Then
                DynaLog.LogMessage("Toggling background processes details panel...")
                BackgroundProcessesButton.PerformClick()
                Focus()
            End If
        ElseIf e.KeyCode = Keys.U And e.Alt Then
            If Not ImgBW.IsBusy Then
                bwBackgroundProcessAction = 0
                bwGetImageInfo = True
                bwGetAdvImgInfo = True
                DynaLog.LogMessage("Triggering background processes...")
                ImgBW.RunWorkerAsync()
            End If
        ElseIf e.KeyCode = Keys.F11 Then
            ToggleFullScreenMode()
        End If
    End Sub

    Private Sub AppendImage_Click(sender As Object, e As EventArgs) Handles AppendImage.Click
        DynaLog.LogMessage("Opening image append dialog...")
        ImgAppend.ShowDialog(Me)
    End Sub

    Sub LoadRecentsFromMenu(itemOrder As Integer)
        Dim itmOrder As Integer = 0
        DynaLog.LogMessage("Items in recents list: " & RecentList.Count)
        If RecentList.Count <= 0 Then
            DynaLog.LogMessage("No items are present in the recents list. Exiting...")
            MsgBox(LocalizationService.ForSection("Main.Messages")("Items.Present.None.Label"))
            Exit Sub
        End If
        If (itemOrder + 1) > RecentList.Count Then
            DynaLog.LogMessage("Item with index " & itemOrder & " is not yet declared in the Recents list. Exiting...")
            Exit Sub
        End If
        If RecentList(itemOrder).ProjPath <> "" And File.Exists(RecentList(itemOrder).ProjPath) Then
            DynaLog.LogMessage("Selected item is not bogus and exists. Loading project...")
            If isProjectLoaded Then UnloadDTProj(False, If(OnlineManagement Or OfflineManagement, False, True))
            If ImgBW.IsBusy Then Exit Sub
            itmOrder = itemOrder
            Dim recentProj As Recents = RecentList(itmOrder)
            DynaLog.LogMessage("Selected project: " & recentProj.ToString())
            DynaLog.LogMessage("Reordering projects in recents list...")
            ChangeRecentListOrder(recentProj, itmOrder)
            ProgressPanel.OperationNum = 990
            LoadDTProj(recentProj.ProjPath,
                       If(recentProj.ProjName <> "",
                          recentProj.ProjName,
                          Path.GetFileNameWithoutExtension(recentProj.ProjPath)),
                       True, False)
        End If
        RecentRemoveLink.Visible = False
        Try
            RecentProject1ToolStripMenuItem.Text = " "
            RecentProject2ToolStripMenuItem.Text = " "
            RecentProject3ToolStripMenuItem.Text = " "
            RecentProject4ToolStripMenuItem.Text = " "
            RecentProject5ToolStripMenuItem.Text = " "
            RecentProject6ToolStripMenuItem.Text = " "
            RecentProject7ToolStripMenuItem.Text = " "
            RecentProject8ToolStripMenuItem.Text = " "
            RecentProject9ToolStripMenuItem.Text = " "
            RecentProject10ToolStripMenuItem.Text = " "

            ' Reconfigure text
            RecentProject1ToolStripMenuItem.Text = RecentList(0).ProjPath
            RecentProject1ToolStripMenuItem.Visible = True
            RecentProject2ToolStripMenuItem.Text = RecentList(1).ProjPath
            RecentProject2ToolStripMenuItem.Visible = True
            RecentProject3ToolStripMenuItem.Text = RecentList(2).ProjPath
            RecentProject3ToolStripMenuItem.Visible = True
            RecentProject4ToolStripMenuItem.Text = RecentList(3).ProjPath
            RecentProject4ToolStripMenuItem.Visible = True
            RecentProject5ToolStripMenuItem.Text = RecentList(4).ProjPath
            RecentProject5ToolStripMenuItem.Visible = True
            RecentProject6ToolStripMenuItem.Text = RecentList(5).ProjPath
            RecentProject6ToolStripMenuItem.Visible = True
            RecentProject7ToolStripMenuItem.Text = RecentList(6).ProjPath
            RecentProject7ToolStripMenuItem.Visible = True
            RecentProject8ToolStripMenuItem.Text = RecentList(7).ProjPath
            RecentProject8ToolStripMenuItem.Visible = True
            RecentProject9ToolStripMenuItem.Text = RecentList(8).ProjPath
            RecentProject9ToolStripMenuItem.Visible = True
            RecentProject10ToolStripMenuItem.Text = RecentList(9).ProjPath
            RecentProject10ToolStripMenuItem.Visible = True
        Catch ex As Exception
            ' Don't do anything special here
        End Try
    End Sub

    Private Sub RecentProject10ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecentProject10ToolStripMenuItem.Click
        LoadRecentsFromMenu(9)
    End Sub

    Private Sub RecentProject9ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecentProject9ToolStripMenuItem.Click
        LoadRecentsFromMenu(8)
    End Sub

    Private Sub RecentProject8ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecentProject8ToolStripMenuItem.Click
        LoadRecentsFromMenu(7)
    End Sub

    Private Sub RecentProject7ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecentProject7ToolStripMenuItem.Click
        LoadRecentsFromMenu(6)
    End Sub

    Private Sub RecentProject6ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecentProject6ToolStripMenuItem.Click
        LoadRecentsFromMenu(5)
    End Sub

    Private Sub RecentProject5ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecentProject5ToolStripMenuItem.Click
        LoadRecentsFromMenu(4)
    End Sub

    Private Sub RecentProject4ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecentProject4ToolStripMenuItem.Click
        LoadRecentsFromMenu(3)
    End Sub

    Private Sub RecentProject3ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecentProject3ToolStripMenuItem.Click
        LoadRecentsFromMenu(2)
    End Sub

    Private Sub RecentProject2ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecentProject2ToolStripMenuItem.Click
        LoadRecentsFromMenu(1)
    End Sub

    Private Sub RecentProject1ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecentProject1ToolStripMenuItem.Click
        LoadRecentsFromMenu(0)
    End Sub

    Private Sub CreateDiscImageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateDiscImageToolStripMenuItem.Click
        ISOCreator.Show()
    End Sub

    Private Sub GetImageFileInformationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GetImageFileInformationToolStripMenuItem.Click
        GetImgInfoDlg.RadioButton1.Checked = False
        GetImgInfoDlg.RadioButton2.Checked = True
        GetImgInfoDlg.TextBox1.Text = MountedImgMgr.ListView1.FocusedItem.SubItems(0).Text
        GetImgInfoDlg.ShowDialog(MountedImgMgr)
    End Sub

    Private Sub SaveCompleteImageInformationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveCompleteImageInformationToolStripMenuItem.Click
        If ImgInfoSFD.ShowDialog(MountedImgMgr) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Saving complete image information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SaveTarget = ImgInfoSFD.FileName
            ImgInfoSaveDlg.SourceImage = MountedImgMgr.ListView1.FocusedItem.SubItems(0).Text
            ImgInfoSaveDlg.ImgMountDir = MountedImgMgr.ListView1.FocusedItem.SubItems(2).Text
            ImgInfoSaveDlg.OnlineMode = OnlineManagement
            ImgInfoSaveDlg.OfflineMode = OfflineManagement
            ImgInfoSaveDlg.AllDrivers = AllDrivers
            ImgInfoSaveDlg.SkipQuestions = SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = True
            ImgInfoSaveDlg.SaveTask = 0
            ImgInfoSaveDlg.ImageToGetInfoFrom = CurrentImage
            ImgInfoSaveDlg.ShowDialog(MountedImgMgr)
            InfoSaveResults.Show()
        End If
    End Sub

    Private Sub CreateDiscImageWithThisFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateDiscImageWithThisFileToolStripMenuItem.Click
        If ISOCreator.BackgroundWorker1.IsBusy Then Exit Sub
        DynaLog.LogMessage("Opening ISO creator...")
        ISOCreator.TextBox1.Text = MountedImgMgr.ListView1.FocusedItem.SubItems(0).Text
        If ISOCreator.Visible Then
            If ISOCreator.WindowState = FormWindowState.Minimized Then
                ISOCreator.WindowState = FormWindowState.Normal
            Else
                ISOCreator.BringToFront()
            End If
            ISOCreator.Focus()
        Else
            ISOCreator.Show()
        End If
    End Sub

    Private Sub CreateTestingEnvironmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateTestingEnvironmentToolStripMenuItem.Click
        DynaLog.LogMessage("Opening test environment creator...")
        NewTestingEnv.Show()
    End Sub

    Private Sub ListImage_Click(sender As Object, e As EventArgs) Handles ListImage.Click
        If Not WIEDownloaderBW.IsBusy Then
            DynaLog.LogMessage("Downloading the WIM Explorer...")
            WIEDownloaderBW.RunWorkerAsync()
        End If
    End Sub

    Private Sub WIEDownloaderBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles WIEDownloaderBW.DoWork
        Try
            ' Download the WIM Explorer and run it while passing the image as an argument
            If Not Directory.Exists(Application.StartupPath & "\bin\utils\WIM-Explorer") Then
                Directory.CreateDirectory(Application.StartupPath & "\bin\utils\WIM-Explorer")
            End If
            DynaLog.LogMessage("Downloading WIM Explorer installer script from GitHub repository...")
            Using WIMExpClient As New WebClient()
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                Dim contents As String = ""
                Try
                    contents = WIMExpClient.DownloadString("https://raw.githubusercontent.com/CodingWonders/WIM-Explorer/main/DISMTools-Install.ps1")
                Catch ex As WebException
                    DynaLog.LogMessage("Could not download WIM Explorer Setup. Error message: " & ex.Status.ToString())
                    MessageBox.Show(LocalizationService.ForSection("Main.Messages").Format("DownloadFailed.Label", ex.Status.ToString()))
                    Exit Sub
                End Try
                If contents <> "" Then
                    DynaLog.LogMessage("Writing contents...")
                    File.WriteAllText(Application.StartupPath & "\bin\utils\WIM-Explorer\setup.ps1", contents, UTF8)
                End If
            End Using
            If File.Exists(Application.StartupPath & "\bin\utils\WIM-Explorer\setup.ps1") Then
                DynaLog.LogMessage("Running script...")
                ' Run installer
                Dim WEProc As New Process()
                WEProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
                WEProc.StartInfo.WorkingDirectory = Application.StartupPath & "\bin\utils\WIM-Explorer"
                WEProc.StartInfo.Arguments = "-executionpolicy unrestricted -file " & Quote & Application.StartupPath & "\bin\utils\WIM-Explorer\setup.ps1" & Quote
                If Not Debugger.IsAttached Then
                    WEProc.StartInfo.CreateNoWindow = True
                    WEProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                End If
                WEProc.Start()
                WEProc.WaitForExit()
            End If
            If File.Exists(Application.StartupPath & "\bin\utils\WIM-Explorer\WIMExplorer.exe") Then
                DynaLog.LogMessage("Loading WIM Explorer...")
                DynaLog.LogMessage("Source image: " & SourceImg)
                ' Delete temporary files
                Directory.Delete(Application.StartupPath & "\bin\utils\WIM-Explorer\temp", True)
                File.Delete(Application.StartupPath & "\bin\utils\WIM-Explorer\setup.ps1")
                Dim WimExplorer As New Process()
                WimExplorer.StartInfo.FileName = Application.StartupPath & "\bin\utils\WIM-Explorer\WIMExplorer.exe"
                WimExplorer.StartInfo.WorkingDirectory = Application.StartupPath & "\bin\utils\WIM-Explorer"
                If (Not OnlineManagement) And (Not OfflineManagement) Then
                    WimExplorer.StartInfo.Arguments = "/image=" & Quote & SourceImg & Quote
                End If
                WimExplorer.Start()
            End If
        Catch ex As Exception
            MessageBox.Show(LocalizationService.ForSection("Main.Messages").Format("PrepareFailed.Label", ex.Message))
            Exit Sub
        End Try
    End Sub

    Private Sub SetLayeredDriver_Click(sender As Object, e As EventArgs) Handles SetLayeredDriver.Click
        DynaLog.LogMessage("Opening layered driver configuration dialog...")
        SetLayeredDriverDialog.ShowDialog(Me)
    End Sub

    Private Sub UnattendedAnswerFileManagerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnattendedAnswerFileManagerToolStripMenuItem.Click
        DynaLog.LogMessage("Opening unattended answer file manager...")
        If isProjectLoaded And Not (OnlineManagement Or OfflineManagement) Then
            UnattendMgr.TextBox1.Text = Path.Combine(projPath, "unattend_xml")
        End If
        UnattendMgr.Show()
    End Sub

    Private Sub UnattendedAnswerFileCreatorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnattendedAnswerFileCreatorToolStripMenuItem.Click
        DynaLog.LogMessage("Opening unattended answer file creator...")
        NewUnattendWiz.Show()
    End Sub

    Private Sub ApplyUnattend_Click(sender As Object, e As EventArgs) Handles ApplyUnattend.Click
        DynaLog.LogMessage("Opening unattended answer file application dialog...")
        ApplyUnattendFile.ShowDialog(Me)
    End Sub

    Private Sub VideoGetterBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles VideoGetterBW.DoWork
        Try
            DynaLog.LogMessage("Getting videos...")
            Dim videoEx As Exception = New Exception()
            If File.Exists(Application.StartupPath & "\videos.xml") Then File.Move(Application.StartupPath & "\videos.xml", Application.StartupPath & "\videos.xml.old")
            Using client As New WebClient()
                DynaLog.LogMessage("Downloading XML...")
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                Try
                    client.DownloadFile("https://raw.githubusercontent.com/CodingWonders/dt_videos/main/videos.xml", Application.StartupPath & "\videos.xml")
                Catch ex As Exception
                    videoEx = ex
                    Throw New Exception(If(videoEx IsNot Nothing, videoEx, "Could not get video feed"))
                    Debug.WriteLine("Could not download video list")
                End Try
            End Using
            Try
                If File.Exists(Application.StartupPath & "\videos.xml") Then
                    VideoList = LoadVideos(Application.StartupPath & "\videos.xml")
                    File.Delete(Application.StartupPath & "\videos.xml.old")
                End If
            Catch ex As Exception
                videoEx = ex
                If File.Exists(Application.StartupPath & "\videos.xml.old") Then File.Move(Application.StartupPath & "\videos.xml.old", Application.StartupPath & "\videos.xml")
                VideoList = LoadVideos(Application.StartupPath & "\videos.xml")
            End Try
            If VideoList IsNot Nothing Then
                If VideoList.Count > 0 Then
                    For Each VideoLink As Video In VideoList
                        Dim thumbnail As Image = GetItemThumbnail(VideoLink.YT_ID)
                        If thumbnail IsNot Nothing Then
                            Dim newThumb As Image = CombineImages(thumbnail)
                            thumbnailList.Images.Add(newThumb)
                        End If
                    Next
                Else
                    Throw New Exception(If(videoEx IsNot Nothing, videoEx, "Could not get video feed"))
                End If
            Else
                Throw New Exception(If(videoEx IsNot Nothing, videoEx, "Could not get video feed"))
            End If
            Panel9.Visible = Not VideoList.Any()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub VideoGetterBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles VideoGetterBW.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            DynaLog.LogMessage("Could not get video feed. Error message: " & e.Error.Message)
            Panel9.Visible = True
            VideoEx = e.Error
            Exit Sub
        End If
        ListView2.LargeImageList = thumbnailList
        thumbnailList.ImageSize = New Size(160, 90)
        thumbnailList.ColorDepth = ColorDepth.Depth32Bit
        If VideoList.Count > 0 Then
            For Each VideoLink As Video In VideoList
                Dim listItem As ListViewItem = New ListViewItem()
                listItem.ImageIndex = VideoList.IndexOf(VideoLink)
                listItem.Text = VideoLink.VideoName
                ListView2.Items.Add(listItem)
            Next
        End If
    End Sub

    Private Sub MainForm_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If WindowState <> FormWindowState.Maximized Then
            WndWidth = Width
            WndHeight = Height
        End If
        If BGProcNotify.Visible Then
            If Environment.OSVersion.Version.Major = 10 Then    ' The Left property also includes the window shadows on Windows 10 and 11
                BGProcNotify.Location = New Point(Left + 8, Top + StatusStrip.Top - (7 + StatusStrip.Height))
            ElseIf Environment.OSVersion.Version.Major = 6 Then
                If Environment.OSVersion.Version.Minor = 1 Then ' The same also applies to Windows 7
                    BGProcNotify.Location = New Point(Left + 8, Top + StatusStrip.Top - (7 + StatusStrip.Height))
                Else
                    BGProcNotify.Location = New Point(Left + 8, Top + StatusStrip.Top - StatusStrip.Height - 7)
                End If
            End If
        ElseIf BGProcDetails.Visible And pinState = 0 Then
            If Environment.OSVersion.Version.Major = 10 Then    ' The Left property also includes the window shadows on Windows 10 and 11
                BGProcDetails.Location = New Point(Left + 8, Top + StatusStrip.Top - (75 + StatusStrip.Height))
            ElseIf Environment.OSVersion.Version.Major = 6 Then
                If Environment.OSVersion.Version.Minor = 1 Then ' The same also applies to Windows 7
                    BGProcDetails.Location = New Point(Left + 8, Top + StatusStrip.Top - (75 + StatusStrip.Height))
                Else
                    BGProcDetails.Location = New Point(Left + 8, Top + StatusStrip.Top - StatusStrip.Height - 75)
                End If
            End If
        End If
        ' Toggle Menu button and side panel visibility
        MenuToggle.Visible = Width <= WindowHelper.ScaleLogical(1024)
        ProjectSidePanel.Visible = Width > WindowHelper.ScaleLogical(1024)
    End Sub

    Private Sub RegCplToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RegCplToolStripMenuItem.Click
        DynaLog.LogMessage("Opening image registry control panel...")
        Dim msg As String = ""
        If isProjectLoaded Then
            DynaLog.LogMessage("A project has been loaded.")
            If IsImageMounted And Not OnlineManagement Then
                DynaLog.LogMessage("All requirements are met (an image is mounted, and no active installation is being managed). Continuing...")
                RegistryControlPanel.Show()
            ElseIf IsImageMounted And OnlineManagement Then
                DynaLog.LogMessage("The active installation is being managed right now. The image is not supported.")
                        msg = LocalizationService.ForSection("Main.RegistryPanel")("Control.Active.Message")
                MsgBox(msg, vbOKOnly + vbCritical, Text)
            End If
        Else
            DynaLog.LogMessage("No project has been loaded.")
                    msg = LocalizationService.ForSection("Main.Registry.Actions")("Load.Project.Mode.Message")
            MsgBox(msg, vbOKOnly + vbExclamation, Text)
        End If
    End Sub

    Sub RestartDetector()
        Debug.WriteLine("Restarting mounted image detector...")
        Try
            If Not MountedImageDetectorBW.IsBusy Then
                Debug.WriteLine("The detector is not busy. Calling it...")
                Call MountedImageDetectorBW.RunWorkerAsync()
            Else
                Debug.WriteLine("The detector is busy.")
                Exit Sub
            End If
            Debug.WriteLine("Mounted Image Detector Restart Successful")
            MountedImageDetectorBWRestarterTimer.Enabled = False
        Catch ex As Exception
            Debug.WriteLine("Mounted Image Detector Restart Not Successful")
        End Try
    End Sub

    Private Sub MountedImageDetectorBWRestarterTimer_Tick(sender As Object, e As EventArgs) Handles MountedImageDetectorBWRestarterTimer.Tick
        Debug.WriteLine("An event that requires the mounted image detector to be restarted has been triggered.")
        RestartDetector()
    End Sub

    Private Sub LanguagesAndOptionalFeaturesISOToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LanguagesAndOptionalFeaturesISOToolStripMenuItem.Click
        Process.Start(String.Format("https://learn.microsoft.com/{0}/azure/virtual-desktop/windows-11-language-packs#prerequisites", LocalizationService.GetMicrosoftLearnCultureCode()))
    End Sub

    Private Sub LanguagesAndFODWin10ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LanguagesAndFODWin10ToolStripMenuItem.Click
        Process.Start(String.Format("https://learn.microsoft.com/{0}/azure/virtual-desktop/language-packs#prerequisites", LocalizationService.GetMicrosoftLearnCultureCode()))
    End Sub

    Private Sub GetCurrentEdition_Click(sender As Object, e As EventArgs) Handles GetCurrentEdition.Click
        DynaLog.LogMessage("Getting current image edition...")
        DynaLog.LogMessage("Image edition: " & Quote & CurrentImage.ImageEditionId & Quote)
        If CurrentImage.ImageEditionId <> "" Then
            DynaLog.LogMessage("Image edition field has been populated. Showing and checking...")
            Dim msg As String = ""
            msg = LocalizationService.ForSection("Main.GetTargetEditions").Format("CurrentEdition.Message", CurrentImage.ImageEditionId)
            If CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise Then
                DynaLog.LogMessage("Image edition is WindowsPE. This is a Windows PE image.")
                msg &= CrLf & LocalizationService.ForSection("Main.GetTargetEditions")("Windows.Message")
            Else
                DynaLog.LogMessage("Image edition is not WindowsPE. This is not a Windows PE image.")
                msg &= CrLf & LocalizationService.ForSection("Main.GetTargetEditions")("ProductKey.Upgrade.Message")
            End If
            MsgBox(msg, vbOKOnly + vbInformation, Text)
        End If
    End Sub

    Private Sub GetTargetEditions_Click(sender As Object, e As EventArgs) Handles GetTargetEditions.Click
        DynaLog.LogMessage("Preparing to get target editions...")
        StopMountedImageDetector()
        DynaLog.LogMessage("Getting target editions...")
        Dim msg As String = ""
        Dim msgSuccess As Boolean
        Try
            DynaLog.LogMessage("Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            DynaLog.LogMessage("Creating session...")
            Using imgSession As DismSession = If(OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MountDir))
                Dim targetEditions As DismEditionCollection = DismApi.GetTargetEditions(imgSession)
                DynaLog.LogMessage("Amount of target editions: " & targetEditions.Count)
                msgSuccess = True
                If targetEditions.Count > 0 Then
                    ' This image hasn't been upgraded to its highest edition
                    DynaLog.LogMessage("There are target editions. This image can give a little more")
                            msg = LocalizationService.ForSection("Main.GetTargetEditions")("Suitable.ProductKey.Message")
                    For Each targetEdition In targetEditions
                        msg &= "- " & targetEdition & CrLf
                    Next
                Else
                    ' This image has been upgraded to its highest edition
                    DynaLog.LogMessage("There are no target editions. This image is already rocking the best edition")
                            msg = LocalizationService.ForSection("Main.GetTargetEditions")("Image.Cannot.Message")
                End If
            End Using
        Catch ex As Exception
            DynaLog.LogMessage("Could not grab edition targets. Error message: " & ex.Message)
            msgSuccess = False
            If CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) OrElse CurrentImage.WinPeInDisguise Then
                DynaLog.LogMessage("Image edition is WindowsPE. This is a Windows PE image.")
                        msg = LocalizationService.ForSection("Main.GetTargetEditions")("Windows.Message")
            Else
                msg = ex.ToString()
            End If
        Finally
            Try
                DismApi.Shutdown()
            Catch ex As Exception
                ' Don't do anything
            End Try
        End Try
        MsgBox(msg, vbOKOnly + If(msgSuccess, vbInformation, vbExclamation), Text)
        DynaLog.LogMessage("Restarting mounted image detector...")
        If Not MountedImageDetectorBW.IsBusy Then Call MountedImageDetectorBW.RunWorkerAsync()
        WatcherTimer.Enabled = True
    End Sub

    Private Sub SetProductKey_Click(sender As Object, e As EventArgs) Handles SetProductKey.Click
        StopMountedImageDetector()
        SetImageKey.ShowDialog(Me)
        StartMountedImageDetector()
    End Sub

    Private Sub SetEdition_Click(sender As Object, e As EventArgs) Handles SetEdition.Click
        StopMountedImageDetector()
        SetImageEdition.ShowDialog(Me)
        StartMountedImageDetector()
    End Sub

    Sub ToggleFullScreenMode()
        If FormBorderStyle = Windows.Forms.FormBorderStyle.None Then
            DynaLog.LogMessage("Exiting full-screen mode...")
            StatusStrip.SizingGrip = True
            FormBorderStyle = Windows.Forms.FormBorderStyle.Sizable
            Bounds = OriginalWindowBounds
            WindowState = OriginalWindowState
        Else
            DynaLog.LogMessage("Entering full-screen mode...")
            StatusStrip.SizingGrip = False
            FormBorderStyle = Windows.Forms.FormBorderStyle.None
            OriginalWindowState = WindowState
            WindowState = FormWindowState.Normal
            OriginalWindowBounds = Bounds
            Bounds = Screen.FromControl(Me).Bounds
        End If
        ExitFullScreenTSMI.Visible = (FormBorderStyle = Windows.Forms.FormBorderStyle.None)
    End Sub

    Private Sub ExitFullScreenTSMI_Click(sender As Object, e As EventArgs) Handles ExitFullScreenTSMI.Click
        ToggleFullScreenMode()
    End Sub

    Sub StopMountedImageDetector()
        DynaLog.LogMessage("Stopping mounted image detector...")
        MountedImageDetectorBWRestarterTimer.Enabled = False
        If MountedImageDetectorBW.IsBusy Then MountedImageDetectorBW.CancelAsync()
        While MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(500)
        End While
        DynaLog.LogMessage("Stopping image status watchers...")
        WatcherTimer.Enabled = False
        If WatcherBW.IsBusy Then WatcherBW.CancelAsync()
        While WatcherBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
    End Sub

    Sub StartMountedImageDetector()
        DynaLog.LogMessage("Restarting mounted image detector and watchers...")
        If Not MountedImageDetectorBW.IsBusy Then Call MountedImageDetectorBW.RunWorkerAsync()
        WatcherTimer.Enabled = True
    End Sub

    Private Sub DISMToolsTourToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DISMToolsTourToolStripMenuItem.Click, LinkLabel28.LinkClicked
        If Directory.Exists(Path.Combine(Application.StartupPath, "docs", "tour")) Then
            DynaLog.LogMessage("Tour directory exists. Starting the tour!")

            Dim languageCode As String = LocalizationService.GetDocumentationLanguageCode()

            tourServer.StartServer()
            If tourServer.IsListenerAlive() Then
                Process.Start(String.Format("http://localhost:2022/{0}/tour-start.html", languageCode))
                TourActionsTSMI.Visible = True
            End If
        End If
    End Sub

    Private Sub RemoveAppliedAnswerFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveAppliedAnswerFileToolStripMenuItem.Click
        Dim pantherXml As String = Path.Combine(MountDir, "Windows", "Panther", "unattend.xml")
        Dim sysprepXml As String = Path.Combine(MountDir, "Windows", "System32", "sysprep", "unattend.xml")
        Dim nonExistentFiles As Integer = 0
        Cursor = Cursors.WaitCursor
        Refresh()
        Try
            If Not File.Exists(pantherXml) Then nonExistentFiles += 1
            If Not File.Exists(sysprepXml) Then nonExistentFiles += 1
            DynaLog.LogMessage("Removing existing answer files...")
            DynaLog.LogMessage("Removing answer file from Panther directory...")
            If File.Exists(pantherXml) Then File.Delete(pantherXml)
            DynaLog.LogMessage("Removing answer file from Sysprep directory...")
            If File.Exists(sysprepXml) Then File.Delete(sysprepXml)
            If nonExistentFiles >= 2 Then
                Throw New Exception("No answer files have been detected in the mounted image.")
            End If
            MsgBox(LocalizationService.ForSection("Main.Messages")("AnswerFile.Removed.Label"), vbOKOnly + vbInformation, "")
        Catch ex As Exception
            DynaLog.LogMessage("Could not remove answer files. Reason: " & ex.Message)
            MsgBox(ex.Message, vbOKOnly + vbExclamation, "")
        End Try
        Cursor = Cursors.Arrow
    End Sub

    Private Sub CommitImage_Click(sender As Object, e As EventArgs) Handles CommitImage.Click
        Button27.PerformClick()
    End Sub

    Private Sub MountImage_Click(sender As Object, e As EventArgs) Handles MountImage.Click
        Button26.PerformClick()
    End Sub

    Private Sub RemountImage_Click(sender As Object, e As EventArgs) Handles RemountImage.Click
        Button25.PerformClick()
    End Sub

    Private Sub OpenDiagnosticLogsInLogViewerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenDiagnosticLogsInLogViewerToolStripMenuItem.Click
        Dim viewerPath As String = Path.Combine(Application.StartupPath, "tools", "DynaViewer", "DynaViewer.exe")
        TryLaunchExternalTool(viewerPath,
                              OpenDiagnosticLogsInLogViewerToolStripMenuItem.Text,
                              Quote & Path.Combine(Application.StartupPath, "logs", "DT_DynaLog.log") & Quote & " " & LocalizationService.GetLanguageCommandLineArgument())
    End Sub

    Private Sub ManageSystemServicesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageSystemServicesToolStripMenuItem.Click
        If isProjectLoaded Then
            If IsImageMounted AndAlso OnlineManagement Then
                Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "services.msc"))
                Exit Sub
            End If
        Else
            Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "services.msc"))
            Exit Sub
        End If
        If ImgBW.IsBusy Then
            MsgBox(LocalizationService.ForSection("Main.Messages")("BackgroundBusy.Message"), vbOKOnly + vbExclamation)
            Exit Sub
        End If
        ServiceManagementForm.Show()
    End Sub

    Private Sub ManageSystemEnvironmentVariablesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageSystemEnvironmentVariablesToolStripMenuItem.Click
        If isProjectLoaded Then
            If IsImageMounted AndAlso OnlineManagement Then
                Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "SystemPropertiesAdvanced.exe"))
                Exit Sub
            End If
        Else
            Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "SystemPropertiesAdvanced.exe"))
            Exit Sub
        End If
        If ImgBW.IsBusy Then
            MsgBox(LocalizationService.ForSection("Main.Messages")("Background.Finish.Message"), vbOKOnly + vbExclamation)
            Exit Sub
        End If
        EnvVarManagementForm.Show()
    End Sub

    Private Sub StopDTTourServerTSMI_Click(sender As Object, e As EventArgs) Handles StopDTTourServerTSMI.Click
        tourServer.StopServer()
        TourActionsTSMI.Visible = False
    End Sub

    Private Sub RestartDTTourTSMI_Click(sender As Object, e As EventArgs) Handles RestartDTTourTSMI.Click
        Dim languageCode As String = LocalizationService.GetDocumentationLanguageCode()

        Process.Start(String.Format("http://localhost:2022/{0}/tour-start.html", languageCode))
    End Sub

    Private Sub RunProcess(FilePath As String, Optional Arguments As String = "")
        Try
            Dim proc As New Process() With {
                .StartInfo = New ProcessStartInfo() With {
                    .FileName = FilePath,
                    .Arguments = Arguments,
                    .WorkingDirectory = Path.GetDirectoryName(FilePath),
                    .CreateNoWindow = True
                }
            }
            proc.Start()
        Catch ignored As Exception

        End Try
    End Sub

    Private Sub StartWdsHelperTSMI_Click(sender As Object, e As EventArgs) Handles StartWdsHelperTSMI.Click
        Dim wdshsPath As String = Path.Combine(Application.StartupPath, "bin", "extps1", "PE_Helper", "pxehelpers", "wds", "wdshelper_server.ps1")
        Dim serverPort As Integer = PXEServerPort
        If File.Exists(wdshsPath) Then
            DynaLog.LogMessage("WDSHS Script exists. Launching...")
            If ModifierKeys.HasFlag(Keys.Shift) AndAlso PxeServerPortSpecifier.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                serverPort = PxeServerPortSpecifier.ServerPort
            End If
            RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "WindowsPowerShell", "v1.0", "powershell.exe"),
                       "-Executionpolicy Bypass -File " & Quote & wdshsPath & Quote & " -sPort " & serverPort)
        Else
            DynaLog.LogMessage("WDSHS Script does not exist.")
        End If
    End Sub

    Private Sub StartFogHelperTSMI_Click(sender As Object, e As EventArgs) Handles StartFogHelperTSMI.Click
        Dim foghsPath As String = Path.Combine(Application.StartupPath, "bin", "extps1", "PE_Helper", "pxehelpers", "fog", "foghelper_server.ps1")
        Dim serverPort As Integer = PXEServerPort
        If File.Exists(foghsPath) Then
            DynaLog.LogMessage("FOGHS Script exists. Launching...")
            If ModifierKeys.HasFlag(Keys.Shift) AndAlso PxeServerPortSpecifier.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                serverPort = PxeServerPortSpecifier.ServerPort
            End If
            RunProcess(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "WindowsPowerShell", "v1.0", "powershell.exe"),
                       "-Executionpolicy Bypass -File " & Quote & foghsPath & Quote & " -sPort " & serverPort)
        Else
            DynaLog.LogMessage("FOGHS Script does not exist.")
        End If
    End Sub

    Private Sub UnixFogInstructionTSMI_Click(sender As Object, e As EventArgs) Handles UnixFogInstructionTSMI.Click
        Dim foghsUnixNotesPath As String = Path.Combine(Application.StartupPath, "bin", "extps1", "PE_Helper", "pxehelpers", "fog", "foghs_unix_notes.txt")
        If File.Exists(foghsUnixNotesPath) Then
            DynaLog.LogMessage("FOGHS UNIX Notes exist. Launching...")
            RunProcess(SystemEditor, Quote & foghsUnixNotesPath & Quote)
        Else
            DynaLog.LogMessage("FOGHS UNIX notes do not exist.")
        End If
    End Sub

    Private Sub BWFailLearnMoreBtn_Click(sender As Object, e As EventArgs) Handles BWFailLearnMoreBtn.Click
        BGProcFailureDialog.FailedTasks = FailedBGProcResultDic
        BGProcFailureDialog.ImageInQuestion = CurrentImage
        BGProcFailureDialog.ShowDialog(Me)
    End Sub

    Private Enum SecureBootCA23Status As Integer
        Unknown = -1
        NotAvailable = 0
        InProgress = 1
        Available = 2
        AvailableEnforced = 3
    End Enum

    Private Sub EvaluateWindowsUEFICA2023ReadinessToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EvaluateWindowsUEFICA2023ReadinessToolStripMenuItem.Click
        DynaLog.LogMessage("Preparing to evaluate readiness...")
        Dim SecureBootKey As RegistryKey = Nothing
        Dim SecureBootStatus As SecureBootCA23Status = SecureBootCA23Status.Unknown
        Try
            SecureBootKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\SecureBoot")

            Dim SBStateKey As RegistryKey = SecureBootKey.OpenSubKey("State")
            Dim SecureBootEnabled As Boolean = SBStateKey.GetValue("UEFISecureBootEnabled", False)
            SBStateKey.Close()

            DynaLog.LogMessage("State of SecureBoot: " & SecureBootEnabled)

            If Not SecureBootEnabled Then
                MessageBox.Show(LocalizationService.ForSection("Main.Messages")("Secure.Boot.Enabled.Label"), LocalizationService.ForSection("Main.Messages")("Secure.Boot.Status.Title"), MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Try
            End If

            Dim SBServicingKey As RegistryKey = SecureBootKey.OpenSubKey("Servicing")
            Dim CA23UpdateStatus As Integer = SBServicingKey.GetValue("WindowsUEFICA2023Capable", 0)
            Dim CA23Updated As String = SBServicingKey.GetValue("UEFICA2023Status", "")
            SBServicingKey.Close()

            DynaLog.LogMessage("UEFI CA 2023 Capable: " & CA23UpdateStatus)
            DynaLog.LogMessage("UEFI CA 2023 Status: " & CA23Updated)

            Select Case CA23UpdateStatus
                Case 0 : SecureBootStatus = SecureBootCA23Status.NotAvailable
                Case 1 : SecureBootStatus = SecureBootCA23Status.Available
                Case 2 : SecureBootStatus = SecureBootCA23Status.AvailableEnforced
            End Select

            Select Case CA23Updated
                Case "NotStarted" : If SecureBootStatus = SecureBootCA23Status.Unknown Then SecureBootStatus = SecureBootCA23Status.NotAvailable
                Case "InProgress" : SecureBootStatus = SecureBootCA23Status.InProgress
                Case "Updated" : If SecureBootStatus < SecureBootCA23Status.Available Then SecureBootStatus = SecureBootCA23Status.Available
            End Select

            Select Case SecureBootStatus
                Case SecureBootCA23Status.NotAvailable
                    MessageBox.Show(LocalizationService.ForSection("Main.Messages")("Secure.Boot"), LocalizationService.ForSection("Main.Messages")("Secure.Boot.Status.Title"), MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Case SecureBootCA23Status.InProgress
                    MessageBox.Show(LocalizationService.ForSection("Main.Messages")("Update.Secure.Boot.Message"), LocalizationService.ForSection("Main.Messages")("Secure.Boot.Status.Title"), MessageBoxButtons.OK, MessageBoxIcon.Information)
                Case SecureBootCA23Status.Available, SecureBootCA23Status.AvailableEnforced
                    MessageBox.Show(LocalizationService.ForSection("Main.Messages")("Secure.Enabled"), LocalizationService.ForSection("Main.Messages")("Secure.Boot.Status.Title"), MessageBoxButtons.OK, MessageBoxIcon.Information)
                Case SecureBootCA23Status.Unknown
                    MessageBox.Show(LocalizationService.ForSection("Main.Messages")("Determine.Status.Label"), LocalizationService.ForSection("Main.Messages")("Secure.Boot.Status.Title"), MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Select
        Catch ex As Exception

        Finally
            If SecureBootKey IsNot Nothing Then SecureBootKey.Close()
        End Try
    End Sub

    Private Sub ApplyFFU_Click(sender As Object, e As EventArgs) Handles ApplyFFU.Click
        DynaLog.LogMessage("Opening image application dialog...")
        FfuApply.ShowDialog(Me)
    End Sub

    Private Sub CaptureFFU_Click(sender As Object, e As EventArgs) Handles CaptureFFU.Click
        DynaLog.LogMessage("Opening image capture dialog...")
        FfuCapture.ShowDialog(Me)
    End Sub

    Private Sub SplitFFU_Click(sender As Object, e As EventArgs) Handles SplitFFU.Click
        DynaLog.LogMessage("Opening image split dialog...")
        FfuSplit.ShowDialog(Me)
    End Sub

    Private Sub OptimizeImage_Click(sender As Object, e As EventArgs) Handles OptimizeImage.Click
        DynaLog.LogMessage("Opening image optimization dialog...")
        ImgOptimize.ShowDialog(Me)
    End Sub

    Private Sub OptimizeFFU_Click(sender As Object, e As EventArgs) Handles OptimizeFFU.Click
        DynaLog.LogMessage("Opening image optimization dialog...")
        FfuOptimize.ShowDialog(Me)
    End Sub

    Private Sub CopyImageToWdsServerTSMI_Click(sender As Object, e As EventArgs) Handles CopyImageToWdsServerTSMI.Click
        WDSInstallImageCopy.Show()
    End Sub

    Private Sub AuditModeTSMI_Click(sender As Object, e As EventArgs) Handles AuditModeTSMI.Click
        ' Create a new answer file with default options for entering audit mode, then copy that file to the system
        Dim auditFile As String = Path.Combine(Path.GetTempPath(), "sysprep_audit_unatt.xml")

        Try
            File.WriteAllText(auditFile, My.Resources.DefaultUnattended_AuditMode, UTF8)
            If File.Exists(auditFile) Then
                If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
                ProgressPanel.UnattendedFile = auditFile
                ' Just copying our custom answer file to the sysprep folder of the target system seems to make it enter an infinite loop
                ' where it can generalize, but won't go back to OOBE; so it keeps entering audit mode. This time do NOT copy the file to
                ' sysprep.
                ProgressPanel.UnattendedCopyToSysprep = False
                ProgressPanel.OperationNum = 79
                ProgressPanel.ShowDialog(Me)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ApplyWimTSMI_Click(sender As Object, e As EventArgs) Handles ApplyWimTSMI.Click
        ImgApply.ShowDialog(Me)
    End Sub

    Private Sub ApplyFfuTSMI_Click(sender As Object, e As EventArgs) Handles ApplyFfuTSMI.Click
        FfuApply.ShowDialog(Me)
    End Sub

    Private Sub CaptureWimTSMI_Click(sender As Object, e As EventArgs) Handles CaptureWimTSMI.Click
        ImgCapture.ShowDialog(Me)
    End Sub

    Private Sub CaptureFfuTSMI_Click(sender As Object, e As EventArgs) Handles CaptureFfuTSMI.Click
        FfuCapture.ShowDialog(Me)
    End Sub

    Private Sub UploadThisImageToMyWDSServerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UploadThisImageToMyWDSServerToolStripMenuItem.Click
        If WDSInstallImageCopy.BackgroundWorker1.IsBusy Then Exit Sub
        DynaLog.LogMessage("Opening WDS upload wizard...")
        WDSInstallImageCopy.TextBox1.Text = MountedImgMgr.ListView1.FocusedItem.SubItems(0).Text
        If WDSInstallImageCopy.Visible Then
            If WDSInstallImageCopy.WindowState = FormWindowState.Minimized Then
                WDSInstallImageCopy.WindowState = FormWindowState.Normal
            Else
                WDSInstallImageCopy.BringToFront()
            End If
            WDSInstallImageCopy.Focus()
        Else
            WDSInstallImageCopy.Show()
        End If
    End Sub

    Private Sub SSE_TSMI_Click(sender As Object, e As EventArgs) Handles SSE_TSMI.Click
        Dim SSEPath As String = Path.Combine(Application.StartupPath, "tools", "StarterScriptEditor", "StarterScriptEditor.exe")
        If TryLaunchExternalTool(SSEPath,
                                 SSE_TSMI.Text,
                                 String.Format("/userdata={0} {1}", Quote & Path.Combine(Application.StartupPath, "userdata", "starter_scripts") & Quote, LocalizationService.GetLanguageCommandLineArgument())) Then
            SSETimer.Enabled = True
        End If
    End Sub

    Private Sub SSETimer_Tick(sender As Object, e As EventArgs) Handles SSETimer.Tick
        If Not Process.GetProcessesByName("StarterScriptEditor").Any() Then
            UserDataManagerModule.CopyUserDataToProgramFiles("starter_scripts")
            SSETimer.Enabled = False
        End If
    End Sub

    Private Sub ThemeDesigner_TSMI_Click(sender As Object, e As EventArgs) Handles ThemeDesigner_TSMI.Click
        Dim TDPath As String = Path.Combine(Application.StartupPath, "tools", "ThemeDesigner", "DT_ThemeDesigner.exe")
        If TryLaunchExternalTool(TDPath,
                                 ThemeDesigner_TSMI.Text,
                                 String.Format("/userdata={0} {1}", Quote & Path.Combine(Application.StartupPath, "userdata", "themes") & Quote, LocalizationService.GetLanguageCommandLineArgument())) Then
            ThemeDesignerTimer.Enabled = True
        End If
    End Sub

    Public Function TryLaunchExternalTool(executablePath As String, displayName As String, Optional arguments As String = "") As Boolean
        Dim resolvedDisplayName As String = If(String.IsNullOrWhiteSpace(displayName), Path.GetFileNameWithoutExtension(executablePath), displayName.Replace("&", "").Trim())

        If Not File.Exists(executablePath) Then
            DynaLog.LogMessage("Could not start external tool because its executable was not found. Tool: " & resolvedDisplayName & "; path: " & Quote & executablePath & Quote)
            MessageBox.Show(LocalizationService.ForSection("Main.ExternalTools").Format("NotFound.Message", resolvedDisplayName, executablePath),
                            LocalizationService.ForSection("Main.ExternalTools")("Error.Title"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return False
        End If

        Try
            If String.IsNullOrWhiteSpace(arguments) Then
                Process.Start(executablePath)
            Else
                Process.Start(executablePath, arguments)
            End If
            Return True
        Catch ex As Exception
            DynaLog.LogMessage("Could not start external tool. Tool: " & resolvedDisplayName & "; path: " & Quote & executablePath & Quote & "; reason: " & ex.Message)
            MessageBox.Show(LocalizationService.ForSection("Main.ExternalTools").Format("StartFailed.Message", resolvedDisplayName, ex.Message),
                            LocalizationService.ForSection("Main.ExternalTools")("Error.Title"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Sub ThemeDesignerTimer_Tick(sender As Object, e As EventArgs) Handles ThemeDesignerTimer.Tick
        If Not Process.GetProcessesByName("DT_ThemeDesigner").Any() Then
            UserDataManagerModule.CopyUserDataToProgramFiles("themes")
            ThemeDesignerTimer.Enabled = False
        End If
    End Sub

    Private Sub MenuToggle_Click(sender As Object, e As EventArgs) Handles MenuToggle.Click
        ProjectSidePanel.Visible = Not ProjectSidePanel.Visible
    End Sub

    Private Sub ChangeComputerNameLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles ChangeComputerNameLink.LinkClicked
        Try
            Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "systempropertiescomputername.exe"))
        Catch ex As Exception
            ' Ignored
        End Try
    End Sub

    Private Async Sub RefreshComputerInfoBtn_Click(sender As Object, e As EventArgs) Handles RefreshComputerInfoBtn.Click
        Cursor = Cursors.WaitCursor
        Await Task.Run(Sub()
                           DisplayInfinityComputerInformation()
                       End Sub)
        Cursor = Cursors.Arrow
    End Sub

    Private Sub ChangeNetworkConfigBtn_Click(sender As Object, e As EventArgs) Handles ChangeNetworkConfigBtn.Click
        Try
            Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "ncpa.cpl"))
        Catch ex As Exception
            ' Ignored
        End Try
    End Sub

    Private Sub AdminToolsBtn_Click(sender As Object, e As EventArgs) Handles AdminToolsBtn.Click
        Try
            Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe"), Quote & "shell:::{D20EA4E1-3957-11d2-A40B-0C5020524153}" & Quote)
        Catch ex As Exception
            ' Ignored
        End Try
    End Sub

    Private Sub RefreshComputerInfoBtn_MouseHover(sender As Object, e As EventArgs) Handles RefreshComputerInfoBtn.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("Main.Tooltips")("RefreshInfo.Label"))
    End Sub

    Private Sub ChangeNetworkConfigBtn_MouseHover(sender As Object, e As EventArgs) Handles ChangeNetworkConfigBtn.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("Main.Tooltips")("Change.Network.Config.Label"))
    End Sub

    Private Sub AdminToolsBtn_MouseHover(sender As Object, e As EventArgs) Handles AdminToolsBtn.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("Main.Tooltips")("Other.Win.Administ.Label"))
    End Sub

    Private Sub ComputerWallpaperPB_MouseHover(sender As Object, e As EventArgs) Handles ComputerWallpaperPB.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("Main.Tooltips")("Change.Wallpaper.Label"))
    End Sub

    Private Sub ComputerWallpaperPB_Click(sender As Object, e As EventArgs) Handles ComputerWallpaperPB.Click
        Try
            If Environment.OSVersion.Version.Major = 10 Then
                Process.Start("ms-settings:personalization-background")
            Else
                Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "desk.cpl"))
            End If
        Catch ex As Exception
            ' Ignored
        End Try
    End Sub

    Private Sub LinkLabel27_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel27.LinkClicked
        HelpDocsModule.DisplayHelpDocumentation("docs\whats_new\highlights.html")
    End Sub

    Private Sub LinkLabel29_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel29.LinkClicked
        HelpDocsModule.DisplayHelpDocumentation("docs\img_tasks\online_inst_mgmt.html")
    End Sub

    Private Sub LinkLabel30_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel30.LinkClicked
        HelpDocsModule.DisplayHelpDocumentation("docs\img_tasks\offline_inst_mgmt.html")
    End Sub

    Private Sub InfinityStartPanel_SizeChanged(sender As Object, e As EventArgs) Handles InfinityStartPanel.SizeChanged
        SplitContainer1.SplitterDistance = WindowHelper.ScaleLogical(428)
    End Sub

    Private Sub LinkLabel32_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel32.LinkClicked
        Try
            DynaLog.LogMessage("Getting videos...")
            Dim videoEx As Exception = New Exception()
            If File.Exists(Application.StartupPath & "\videos.xml") Then File.Move(Application.StartupPath & "\videos.xml", Application.StartupPath & "\videos.xml.old")
            Using client As New WebClient()
                DynaLog.LogMessage("Downloading XML...")
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                Try
                    client.DownloadFile("https://raw.githubusercontent.com/CodingWonders/dt_videos/main/videos.xml", Application.StartupPath & "\videos.xml")
                Catch ex As Exception
                    videoEx = ex
                    Throw New Exception(If(videoEx IsNot Nothing, videoEx, "Could not get video feed"))
                    Debug.WriteLine("Could not download video list")
                End Try
            End Using
            Try
                If File.Exists(Application.StartupPath & "\videos.xml") Then
                    VideoList = LoadVideos(Application.StartupPath & "\videos.xml")
                    File.Delete(Application.StartupPath & "\videos.xml.old")
                End If
            Catch ex As Exception
                videoEx = ex
                If File.Exists(Application.StartupPath & "\videos.xml.old") Then File.Move(Application.StartupPath & "\videos.xml.old", Application.StartupPath & "\videos.xml")
                VideoList = LoadVideos(Application.StartupPath & "\videos.xml")
            End Try
            ListView2.Items.Clear()
            Dim thumbnailList As ImageList = New ImageList()
            thumbnailList.ImageSize = New Size(160, 90)
            thumbnailList.ColorDepth = ColorDepth.Depth32Bit
            ListView2.View = View.LargeIcon
            ListView2.LargeImageList = thumbnailList
            If VideoList IsNot Nothing Then
                If VideoList.Count > 0 Then
                    For Each VideoLink As Video In VideoList
                        Dim thumbnail As Image = GetItemThumbnail(VideoLink.YT_ID)
                        If thumbnail IsNot Nothing Then
                            Dim newThumb As Image = CombineImages(thumbnail)
                            thumbnailList.Images.Add(newThumb)
                        End If
                        Dim listItem As ListViewItem = New ListViewItem()
                        listItem.ImageIndex = VideoList.IndexOf(VideoLink)
                        listItem.Text = VideoLink.VideoName
                        ListView2.Items.Add(listItem)
                    Next
                End If
            ElseIf VideoList Is Nothing OrElse VideoList.Count = 0 Then
                Throw New Exception(If(videoEx IsNot Nothing, videoEx, "Could not get video feed"))
            End If
            Panel9.Visible = Not VideoList.Any()
        Catch ex As Exception
            DynaLog.LogMessage("Could not get video feed. Error message: " & ex.Message)
            Panel9.Visible = True
            VideoEx = ex
        End Try
    End Sub

    Private Sub LinkLabel33_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel33.LinkClicked
        DynaLog.LogMessage("Refreshing news feed...")
        GetFeedNews()
        DynaLog.LogMessage("Items in feed: " & If(FeedContents IsNot Nothing AndAlso FeedContents.Items IsNot Nothing, FeedContents.Items.Count(), 0))
        RenderNewsFeed()
    End Sub

    Private Sub LinkLabel31_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel31.LinkClicked
        MessageBox.Show(VideoEx.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub LinkLabel34_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel34.LinkClicked
        Dim feedErrorMessage As String = ""
        If FeedEx IsNot Nothing Then feedErrorMessage = FeedEx.Message

        If String.IsNullOrWhiteSpace(feedErrorMessage) Then
            feedErrorMessage = LocalizationService.ForSection("Main.News.Error")("NoDetails.Message")
        End If

        MessageBox.Show(feedErrorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub ComputerNameLabel_MouseHover(sender As Object, e As EventArgs) Handles ComputerNameLabel.MouseHover
        WindowHelper.DisplayToolTip(sender, String.Format(LocalizationService.ForSection("Main.Tooltips")("NetBiosname.Label"), My.Computer.Name))
    End Sub

    Private Sub NewsFeedCloseBtn_Click(sender As Object, e As EventArgs) Handles NewsFeedCloseBtn.Click
        NewsContentPreviewerPanel.Visible = False
    End Sub

    Private Sub RefreshFactButton_Click(sender As Object, e As EventArgs) Handles RefreshFactButton.Click
        If InfinityHomeFacts.Any() Then
            ' Show a random one
            FactLabel.Text = InfinityHomeFacts.ElementAt(New Random().Next(InfinityHomeFacts.Count)).Message
        End If
    End Sub

    Private Sub RefreshFactButton_MouseHover(sender As Object, e As EventArgs) Handles RefreshFactButton.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("Main.Tooltips")("Show.New.Fact.Label"))
    End Sub
End Class
