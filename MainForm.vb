Imports System.Net
Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports Microsoft.Win32

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

    ' Var used to detect whether the image is orphaned (needs servicing session reload)
    Public isOrphaned As Boolean    ' This variable is true when the host system is shut down or restarted (the servicing session stops abruptly)
    Public mountedImgStatus As String

    Public imgIndexCount As Integer

    Public reloadDlg As Boolean = False

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
    Public LogLevel As Integer
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

    ' Arrays and other variables used on background processes
    Public imgPackageNames(65535) As String
    Public imgPackageState(65535) As String
    Public imgFeatureNames(65535) As String
    Public imgFeatureState(65535) As String

    ' Perform image unmount operations when pressing on buttons
    Public imgCommitOperation As Integer = -1 ' 0: commit; 1: discard

    Dim DismVersionChecker As FileVersionInfo

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
        BranchTSMI.Text = dtBranch
        If Debugger.IsAttached Then
            BranchTSMI.Visible = True
            Text &= " (debug mode)"
        End If
        LoadDTSettings(1)
        imgStatus = 0
        ChangeImgStatus()
        If DismExe <> "" Then
            DismVersionChecker = FileVersionInfo.GetVersionInfo(DismExe)
        End If
    End Sub

    Sub ChangeImgStatus()
        If imgStatus = 0 Then
            Label5.Text = "No"
            LinkLabel1.Visible = True
        Else
            Label5.Text = "Yes"
            LinkLabel1.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' Set colors on any surface with the "Professional" RenderMode in dark mode
    ''' </summary>
    ''' <remarks></remarks>
    Private Class DarkModeColorTable
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
    Private Class LightModeColorTable
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
        Debug.WriteLine("DISMTools, version " & My.Application.Info.Version.ToString() & " (" & dtBranch & ")" & CrLf & _
                        "Loading program settings..." & CrLf)
        ' LoadMode = 0; load from registry
        ' LoadMode = 1; load from INI file
        If LoadMode = 0 Then
            Try

            Catch ex As Exception

            End Try
        ElseIf LoadMode = 1 Then
            If File.Exists(".\settings.ini") Then
                DTSettingForm.RichTextBox1.Text = My.Computer.FileSystem.ReadAllText(".\settings.ini", UTF8)
                ' Perform the Volatile mode check before applying any settings
                If DTSettingForm.RichTextBox1.Text.Contains("Volatile=0") Then
                    VolatileMode = False
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("Volatile=1") Then
                    VolatileMode = True
                    ' Cancel setting application
                    Exit Sub
                End If
                DismExe = DTSettingForm.RichTextBox1.Lines(3).Replace("DismExe=", "").Trim().Replace(Quote, "").Trim()
                If DTSettingForm.RichTextBox1.Text.Contains("SaveOnSettingsIni=0") Then
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
                ' Do note that this version does not support languages other than English so, if INI contains
                ' "Language=2" or something else, it will ignore it and use English
                If DTSettingForm.RichTextBox1.Text.Contains("Language=0") Then
                    ' The note above also applies to the Automatic language setting
                    Language = 1
                Else
                    Language = 1
                End If
                ' Apply language settings immediately
                ChangeLangs(Language)
                ' Detect log font setting. Do note that, if a system does not contain the font set in this program,
                ' it will revert to "Courier New"
                LogFont = DTSettingForm.RichTextBox1.Lines(10).Replace("LogFont=", "").Trim().Replace(Quote, "").Trim()
                LogFontSize = CInt(DTSettingForm.RichTextBox1.Lines(11).Replace("LogFontSi=", "").Trim())
                If DTSettingForm.RichTextBox1.Text.Contains("LogFontBold=0") Then
                    LogFontIsBold = False
                ElseIf DTSettingForm.RichTextBox1.Text.Contains("LogFontBold=1") Then
                    LogFontIsBold = True
                End If
                ' Detect log file path. If file does not exist, create one
                LogFile = DTSettingForm.RichTextBox1.Lines(15).Replace("LogFile=", "").Trim().Replace(Quote, "").Trim()
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
                    ' Detect scratch directory
                    ScratchDir = DTSettingForm.RichTextBox1.Lines(23).Replace("ScratchDirLocation=", "").Trim().Replace(Quote, "").Trim()
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
                            "ReportView           =    " & ReportView)
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

#Region "Background Processes"  ' These will be called later on

    ''' <summary>
    ''' Gets installed packages in an image and puts them in separate arrays
    ''' </summary>
    Sub GetImagePackages()
        ' Get image packages and their state
        File.WriteAllText(".\bin\exthelpers\pkginfo.bat", _
                          "@echo off" & CrLf & _
                          "dism /English /image=" & MountDir & " /get-packages | findstr /c:" & Quote & "Package Identity : " & Quote & " > .\imgpkgnames" & CrLf & _
                          "dism /English /image=" & MountDir & " /get-packages | findstr /c:" & Quote & "State : " & Quote & " > .\imgpkgstate")
        ImgProcesses.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
        ImgProcesses.StartInfo.Arguments = "/c " & Directory.GetCurrentDirectory() & "\bin\exthelpers\pkginfo.bat"
        ImgProcesses.StartInfo.CreateNoWindow = True
        ImgProcesses.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        ImgProcesses.Start()
        Do Until ImgProcesses.HasExited
            If ImgProcesses.HasExited Then
                Exit Do
            End If
        Loop
        If ImgProcesses.ExitCode = 0 Then
            Dim pkgNameRTB As New RichTextBox With {
                .Text = My.Computer.FileSystem.ReadAllText(".\imgpkgnames")
            }
            Dim pkgStateRTB As New RichTextBox With {
                .Text = My.Computer.FileSystem.ReadAllText(".\imgpkgstate")
            }
            For x = 0 To pkgNameRTB.Lines.Count - 1
                imgPackageNames(x) = pkgNameRTB.Lines(x).ToString()
            Next
            For x = 0 To pkgStateRTB.Lines.Count - 1
                imgPackageState(x) = pkgStateRTB.Lines(x).ToString()
            Next
        Else

        End If
    End Sub

    ''' <summary>
    ''' Gets present features in an image and puts them in separate arrays
    ''' </summary>
    Sub GetImageFeatures()
        ' Get image features and their state
        File.WriteAllText(".\bin\exthelpers\featinfo.bat", _
                          "@echo off" & CrLf & _
                          "dism /English /image=" & MountDir & " /get-features | findstr /c:" & Quote & "Feature Name : " & Quote & " > .\imgfeatnames" & CrLf & _
                          "dism /English /image=" & MountDir & " /get-features | findstr /c:" & Quote & "State : " & Quote & " > .\imgfeatstate")
        ImgProcesses.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
        ImgProcesses.StartInfo.Arguments = "/c " & Directory.GetCurrentDirectory() & "\bin\exthelpers\featinfo.bat"
        ImgProcesses.StartInfo.CreateNoWindow = True
        ImgProcesses.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        ImgProcesses.Start()
        Do Until ImgProcesses.HasExited
            If ImgProcesses.HasExited Then
                Exit Do
            End If
        Loop
        If ImgProcesses.ExitCode = 0 Then
            Dim featureNameRTB As New RichTextBox With {
                .Text = My.Computer.FileSystem.ReadAllText(".\imgfeatnames")
            }
            Dim featureStateRTB As New RichTextBox With {
                .Text = My.Computer.FileSystem.ReadAllText(".\imgfeatstate")
            }
            For x = 0 To featureNameRTB.Lines.Count - 1
                imgFeatureNames(x) = featureNameRTB.Lines(x).ToString()
            Next
            For x = 0 To featureStateRTB.Lines.Count - 1
                imgFeatureState(x) = featureStateRTB.Lines(x).ToString()
            Next
        Else

        End If
    End Sub

    ''' <summary>
    ''' Gets installed provisioned APPX packages in an image and puts them in separate arrays. This is only for Windows 8 and newer
    ''' </summary>
    Sub GetImageAppxPackages()

    End Sub

    ''' <summary>
    ''' Gets installed Features on Demand (capabilities) in an image and puts them in separate arrays. This is only for Windows 10 or newer
    ''' </summary>
    Sub GetImageCapabilities()

    End Sub

    ''' <summary>
    ''' Gets installed drivers in an image and puts them in separate arrays
    ''' </summary>
    Sub GetImageDrivers()

    End Sub

#End Region

    Sub GenerateDTSettings()
        DTSettingForm.RichTextBox2.AppendText("# DISMTools (version 0.1) configuration file" & CrLf & CrLf & "[Program]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("DismExe=" & Quote & "\Windows\system32\dism.exe" & Quote)
        DTSettingForm.RichTextBox2.AppendText(CrLf & "SaveOnSettingsIni=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "Volatile=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[Personalization]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("ColorMode=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "Language=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "LogFont=" & Quote & "Courier New" & Quote)
        DTSettingForm.RichTextBox2.AppendText(CrLf & "LogFontSi=10")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "LogFontBold=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[Logs]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("LogFile=" & Quote & "\Windows\Logs\DISM\DISM.log" & Quote)
        DTSettingForm.RichTextBox2.AppendText(CrLf & "LogLevel=3")
        DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[ImgOps]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("ImgOperationMode=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "Quiet=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "NoRestart=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[ScratchDir]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("UseScratch=0")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "ScratchDirLocation=" & Quote & "" & Quote)
        DTSettingForm.RichTextBox2.AppendText(CrLf & CrLf & "[Output]" & CrLf)
        DTSettingForm.RichTextBox2.AppendText("EnglishOutput=1")
        DTSettingForm.RichTextBox2.AppendText(CrLf & "ReportView=0")
        File.WriteAllText(".\settings.ini", DTSettingForm.RichTextBox2.Text, ASCII)
    End Sub

    Sub SaveDTSettings()
        If VolatileMode Then
            Exit Sub
        Else
            If SaveOnSettingsIni Then
                If File.Exists(".\settings.ini") Then
                    File.Delete(".\settings.ini")
                End If
                DTSettingForm.RichTextBox2.Clear()
                DTSettingForm.RichTextBox2.AppendText("# DISMTools (version 0.1) configuration file" & CrLf & CrLf & "[Program]" & CrLf)
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
                ' This version does not support additional languages
                DTSettingForm.RichTextBox2.AppendText(CrLf & "Language=1")
                DTSettingForm.RichTextBox2.AppendText(CrLf & "LogFont=" & Quote & LogFont & Quote)
                DTSettingForm.RichTextBox2.AppendText(CrLf & "LogFontSi=" & LogFontSize)
                If LogFontIsBold Then
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "LogFontBold=1")
                Else
                    DTSettingForm.RichTextBox2.AppendText(CrLf & "LogFontBold=0")
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
                File.WriteAllText(".\settings.ini", DTSettingForm.RichTextBox2.Text, ASCII)
            Else
                ' This procedure should not be called yet
                Try
                    Registry.CurrentUser.OpenSubKey("Software\DISMTools", True)
                Catch RegNotFound As NullReferenceException
                    Registry.CurrentUser.CreateSubKey("Software\DISMTools", True)
                    Registry.CurrentUser.OpenSubKey("Software\DISMTools", True)
                End Try

            End If
        End If

    End Sub

    Sub ResetDTSettings()

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
                        FeatureInfoCMS.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                        PkgInfoCMS.ForeColor = Color.White
                        FeatureInfoCMS.ForeColor = Color.White
                        InvalidSettingsTSMI.Image = New Bitmap(My.Resources.setting_error_glyph_dark)
                        BranchTSMI.Image = New Bitmap(My.Resources.branch_dark)
                    ElseIf ColorMode = "1" Then
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
                        FeatureInfoCMS.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                        PkgInfoCMS.ForeColor = Color.Black
                        FeatureInfoCMS.ForeColor = Color.Black
                        InvalidSettingsTSMI.Image = New Bitmap(My.Resources.setting_error_glyph)
                        BranchTSMI.Image = New Bitmap(My.Resources.branch)
                    End If
                Catch ex As Exception
                    ChangePrgColors(1)
                End Try
            Case 1
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
                FeatureInfoCMS.Renderer = New ToolStripProfessionalRenderer(New LightModeColorTable())
                PkgInfoCMS.ForeColor = Color.Black
                FeatureInfoCMS.ForeColor = Color.Black
                InvalidSettingsTSMI.Image = New Bitmap(My.Resources.setting_error_glyph)
                BranchTSMI.Image = New Bitmap(My.Resources.branch)
            Case 2
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
                FeatureInfoCMS.Renderer = New ToolStripProfessionalRenderer(New DarkModeColorTable())
                PkgInfoCMS.ForeColor = Color.White
                FeatureInfoCMS.ForeColor = Color.White
                InvalidSettingsTSMI.Image = New Bitmap(My.Resources.setting_error_glyph_dark)
                BranchTSMI.Image = New Bitmap(My.Resources.branch_dark)
        End Select
    End Sub

    Sub ChangeLangs(LangCode As Integer)
        Select Case LangCode
            Case 0

            Case 1

            Case 2

        End Select
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

    Sub LoadDTProj(DTProjPath As String, DTProjFileName As String, BypassFileDialog As Boolean)
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
                PleaseWaitDialog.Label2.Text = "Loading project: " & Quote & prjName & Quote
                PleaseWaitDialog.ShowDialog(Me)
                projName.Text = prjName
                Label3.Text = DTProjPath
                projPath = DTProjPath
                projPath = projPath.Replace("\" & DTProjFileName & ".dtproj", "").Trim()
                If IsImageMounted Then
                    ImageNotMountedPanel.Visible = False
                    ImagePanel.Visible = True
                End If
                PopulateProjectTree(prjName)
                isProjectLoaded = True
            Else
                If OpenFileDialog1.FileName = "" Then
                    If BypassFileDialog = False Then
                        Exit Sub
                    End If
                Else
                    prjName = Path.GetFileNameWithoutExtension(DTProjPath)
                    Text = prjName & " - DISMTools"
                    If Debugger.IsAttached Then
                        Text &= " (debug mode)"
                    End If
                    Label3.Text = DTProjPath
                    projPath = DTProjPath
                    projPath = projPath.Replace("\" & DTProjFileName & ".dtproj", "").Trim()
                    PleaseWaitDialog.Label2.Text = "Loading project: " & Quote & prjName & Quote
                    PleaseWaitDialog.ShowDialog(Me)
                    projName.Text = prjName
                    If IsImageMounted Then
                        ImageNotMountedPanel.Visible = False
                        ImagePanel.Visible = True
                    Else
                        ImageNotMountedPanel.Visible = True
                        ImagePanel.Visible = False
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
                    imgRW = ProjectValueLoadForm.RichTextBox25.Text

                    ' Detect individual stuff
                    If Directory.Exists(projPath & "\mount" & "\Windows") Then
                        ' Detect whether image is mounted by checking its Windows directory.
                        ' This will be changed in the future but, because this is in alpha, scan
                        ' whether the image's Windows folder exists
                        IsImageMounted = True
                        If imgRW = "Yes" Then
                            UpdateProjProperties(True, False)
                        ElseIf imgRW = "No" Then
                            UpdateProjProperties(True, True)
                        Else
                            ' Assume it has read-write permissions
                            UpdateProjProperties(True, True)
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
                            UpdateProjProperties(True, True)
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
                    End If
                End If
            End If
        Else
            MessageBox.Show("Cannot load the project. Reason: the project was not found. It may have been moved or its folder may have been deleted.", "Project load error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            If DialogResult.OK Then
                Exit Sub
            End If
        End If
    End Sub

    Sub SaveDTProj()
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
                                                      "ImageLang=" & ProjectValueLoadForm.RichTextBox24.Text & CrLf & CrLf & _
                                                      "[Params]" & CrLf & _
                                                      "ImageReadWrite=" & ProjectValueLoadForm.RichTextBox25.Text)
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

    Sub UnloadDTProj(IsBeingClosed As Boolean, SaveProject As Boolean, UnmountImg As Boolean)
        If imgCommitOperation = 0 Then
            ProgressPanel.OperationNum = 21
            ProgressPanel.UMountLocalDir = True
            ProgressPanel.RandomMountDir = ""   ' Hope there isn't anything to set here
            ProgressPanel.UMountImgIndex = ImgIndex
            ProgressPanel.MountDir = MountDir
            ProgressPanel.UMountOp = 0
            ProgressPanel.ShowDialog()
            Exit Sub
        ElseIf imgCommitOperation = 1 Then
            ProgressPanel.OperationNum = 21
            ProgressPanel.UMountLocalDir = True
            ProgressPanel.RandomMountDir = ""   ' Hope there isn't anything to set here
            ProgressPanel.UMountImgIndex = ImgIndex
            ProgressPanel.MountDir = MountDir
            ProgressPanel.UMountOp = 1
            ProgressPanel.ShowDialog()
            Exit Sub
        End If
        If SaveProject Then
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
    End Sub

    Sub UpdateProjProperties(WasImageMounted As Boolean, IsReadOnly As Boolean)
        If WasImageMounted Then
            Label5.Text = "Yes"
            LinkLabel1.Visible = False
            ImageNotMountedPanel.Visible = False
            ImagePanel.Visible = True
            IsImageMounted = True
        Else
            Label5.Text = "No"
            LinkLabel1.Visible = True
            ImageNotMountedPanel.Visible = True
            ImagePanel.Visible = False
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

        ' Set image properties
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
                                File.WriteAllText(".\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " > imgname", _
                                                  ASCII)
                            Case Is >= 2
                                File.WriteAllText(".\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " > imgname", _
                                                  ASCII)
                        End Select
                    Case 10
                        File.WriteAllText(".\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " > imgname", _
                                          ASCII)
                End Select
                Process.Start(".\bin\exthelpers\temp.bat").WaitForExit()
                Label18.Text = My.Computer.FileSystem.ReadAllText(".\imgname").Replace("Name : ", "").Trim()
                File.Delete(".\imgname")
                File.Delete(".\bin\exthelpers\temp.bat")
                Select Case DismVersionChecker.ProductMajorPart
                    Case 6
                        Select Case DismVersionChecker.ProductMinorPart
                            Case 1
                                File.WriteAllText(".\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                                  ASCII)
                            Case Is >= 2
                                File.WriteAllText(".\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                                  ASCII)
                        End Select
                    Case 10
                        File.WriteAllText(".\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & ProgressPanel.SourceImg & " /index=" & ProgressPanel.ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                          ASCII)
                End Select
                Process.Start(".\bin\exthelpers\temp.bat").WaitForExit()
                Label20.Text = My.Computer.FileSystem.ReadAllText(".\imgdesc").Replace("Description : ", "").Trim()
                File.Delete(".\imgdesc")
                File.Delete(".\bin\exthelpers\temp.bat")
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
                                File.WriteAllText(".\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Name" & Quote & " > imgname", _
                                                  ASCII)
                            Case Is >= 2
                                File.WriteAllText(".\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Name" & Quote & " > imgname", _
                                                  ASCII)
                        End Select
                    Case 10
                        File.WriteAllText(".\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Name" & Quote & " > imgname", _
                                          ASCII)
                End Select
                Process.Start(".\bin\exthelpers\temp.bat").WaitForExit()
                Label18.Text = My.Computer.FileSystem.ReadAllText(".\imgname").Replace("Name : ", "").Trim()
                File.Delete(".\imgname")
                File.Delete(".\bin\exthelpers\temp.bat")
                Select Case DismVersionChecker.ProductMajorPart
                    Case 6
                        Select Case DismVersionChecker.ProductMinorPart
                            Case 1
                                File.WriteAllText(".\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                                  ASCII)
                            Case Is >= 2
                                File.WriteAllText(".\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                                  ASCII)
                        End Select
                    Case 10
                        File.WriteAllText(".\bin\exthelpers\temp.bat", "@echo off" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " /index=" & ImgIndex & " | findstr " & Quote & "Description" & Quote & " > imgdesc", _
                                          ASCII)
                End Select
                Process.Start(".\bin\exthelpers\temp.bat").WaitForExit()
                Label20.Text = My.Computer.FileSystem.ReadAllText(".\imgdesc").Replace("Description : ", "").Trim()
                File.Delete(".\imgdesc")
                File.Delete(".\bin\exthelpers\temp.bat")
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
                        File.WriteAllText(".\bin\exthelpers\imginfo.bat", _
                                          "@echo off" & CrLf & _
                                          "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & projPath & "\tempinfo\imgmountedstatus", ASCII)
                    Case Is >= 2
                        File.WriteAllText(".\bin\exthelpers\imginfo.bat", _
                                          "@echo off" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & projPath & "\tempinfo\imgmountedstatus", ASCII)
                End Select
            Case 10
                File.WriteAllText(".\bin\exthelpers\imginfo.bat", _
                                  "@echo off" & CrLf & _
                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & projPath & "\tempinfo\imgmountedstatus", ASCII)
        End Select
        Process.Start(".\bin\exthelpers\imginfo.bat").WaitForExit()
        mountedImgStatus = My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\imgmountedstatus", ASCII).Replace("Status : ", "").Trim()
        File.Delete(".\bin\exthelpers\imginfo.bat")
        Select Case DismVersionChecker.ProductMajorPart
            Case 6
                Select Case DismVersionChecker.ProductMinorPart
                    Case 1
                        File.WriteAllText(".\bin\exthelpers\imginfo.bat", _
                                          "@echo off" & CrLf & _
                                          "dism /English /get-wiminfo /wimfile=" & SourceImg & " | find /c " & Quote & "Index" & Quote & " > " & projPath & "\tempinfo\indexcount", ASCII)
                    Case Is >= 2
                        File.WriteAllText(".\bin\exthelpers\imginfo.bat", _
                                          "@echo off" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & SourceImg & " | find /c " & Quote & "Index" & Quote & " > " & projPath & "\tempinfo\indexcount", ASCII)
                End Select
            Case 10
                File.WriteAllText(".\bin\exthelpers\imginfo.bat", _
                                  "@echo off" & CrLf & _
                                  "dism /English /get-imageinfo /imagefile=" & SourceImg & " | find /c " & Quote & "Index" & Quote & " > " & projPath & "\tempinfo\indexcount", ASCII)
        End Select
        Process.Start(".\bin\exthelpers\imginfo.bat").WaitForExit()
        imgIndexCount = CInt(My.Computer.FileSystem.ReadAllText(projPath & "\tempinfo\indexcount", ASCII))
        File.Delete(".\bin\exthelpers\imginfo.bat")
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
                                File.WriteAllText(".\bin\exthelpers\imginfo.bat", _
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
                                File.WriteAllText(".\bin\exthelpers\imginfo.bat", _
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
                        File.WriteAllText(".\bin\exthelpers\imginfo.bat", _
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
                    Process.Start("\Windows\system32\notepad.exe", ".\bin\exthelpers\imginfo.bat").WaitForExit()
                End If
                Process.Start(".\bin\exthelpers\imginfo.bat").WaitForExit()
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
                    File.Delete(".\bin\exthelpers\imginfo.bat")
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
        End If
    End Sub

    Sub DetectNTVersion(NTKeExe As String)
        Try
            Dim NTKeVerInfo As FileVersionInfo
            NTKeVerInfo = FileVersionInfo.GetVersionInfo(NTKeExe)
            If NTKeVerInfo.ProductMajorPart = 6 Then
                If NTKeVerInfo.ProductMinorPart = 0 Then        ' Windows Vista / WinPE 2.x
                    ' Let the user know the incompatibility
                    If Not ProgressPanel.IsDisposed Then
                        ToolStripButton4.Visible = False
                        ProgressPanel.Dispose()
                        ProgressPanel.Close()
                    End If
                    ImgWinVistaIncompatibilityDialog.ShowDialog(Me)
                    If ImgWinVistaIncompatibilityDialog.DialogResult = Windows.Forms.DialogResult.OK Then
                        ' Disable every option

                    ElseIf ImgWinVistaIncompatibilityDialog.DialogResult = Windows.Forms.DialogResult.Cancel Then
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
                ElseIf NTKeVerInfo.ProductMinorPart = 1 Then    ' Windows 7 / WinPE 3.x

                ElseIf NTKeVerInfo.ProductMinorPart = 2 Then    ' Windows 8 / WinPE 4.0

                ElseIf NTKeVerInfo.ProductMinorPart = 3 Then    ' Windows 8.1 / WinPE 5.x

                ElseIf NTKeVerInfo.ProductMinorPart = 4 Then    ' Windows 10 (Technical Preview)

                End If
            ElseIf NTKeVerInfo.ProductMajorPart = 10 Then
                Select Case NTKeVerInfo.ProductBuildPart
                    Case 9888 To 21390                          ' Windows 10 / Server 2016,2019,2022 / Cobalt_SunValley / Win10X / WinPE 10.0

                    Case Is >= 21996                            ' Windows 11 / Cobalt_Refresh / Nickel / Copper / WinPE 10.0

                End Select
            End If
        Catch ex As Exception

        End Try
    End Sub

    Sub PopulateProjectTree(MainProjNameNode As String)
        prjTreeStatus.Visible = True
        Try
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
                    MenuDesc.Text = "Displays information about all packages in the image"
                Case 25
                    MenuDesc.Text = "Displays information about a package provided as a .cab file"
                Case 26
                    MenuDesc.Text = "Installs a .cab or .msu package in the image"
                Case 27
                    MenuDesc.Text = "Removes a .cab file package from the image"
                Case 28
                    MenuDesc.Text = "Displays information about all features in a package"
                Case 29
                    MenuDesc.Text = "Displays information about a feature"
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
                    MenuDesc.Text = "Gets a list of capabilities and their install status in the image"
                Case 66
                    MenuDesc.Text = "Gets information about a specific capability"
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
                    MenuDesc.Text = "Displays information about all driver packages in the image"
                Case 73
                    MenuDesc.Text = "Displays information about a specific driver package"
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
                    MenuDesc.Text = "Saves the changes of this project"
                Case 4
                    MenuDesc.Text = "Saves this project on another location"
                Case 5
                    MenuDesc.Text = "Closes the program. If a project is loaded, you will be asked whether or not you would like to save it"
                Case 6
                    MenuDesc.Text = "Opens the File Explorer to view the project files"
                Case 7
                    MenuDesc.Text = "Unloads this project. If changes were made, you will be asked whether or not you would like to save it"
                Case 8
                    MenuDesc.Text = "Switches the mounted image index"
                Case 9
                    MenuDesc.Text = "Launches the project section of the project properties dialog"
                Case 10
                    MenuDesc.Text = "Launches the image section of the project properties dialog"
                Case 11
                    MenuDesc.Text = "Performs image format conversion from WIM to ESD and vice versa"
                Case 12
                    MenuDesc.Text = "Merges two or more SWM files into a single WIM file"
                Case 13
                    MenuDesc.Text = "Remounts the image with read-write permissions to allow making modifications to it"
                Case 14
                    MenuDesc.Text = "Opens the Command Console"
                Case 15
                    MenuDesc.Text = "Lets you manage unattended answer files for this project"
                Case 16
                    MenuDesc.Text = "Lets you manage project reports"
                Case 17
                    MenuDesc.Text = "Configures settings for the program"
                Case 18
                    MenuDesc.Text = "Opens the help topics for this program"
                Case 19
                    MenuDesc.Text = "Opens the glossary, if you don't understand a concept"
                Case 20
                    MenuDesc.Text = "Shows the Command Help, letting you use commands to perform the same actions"
                Case 21
                    MenuDesc.Text = "Shows program information"
            End Select
        End If
    End Sub

    Sub HideParentDesc()
        MenuDesc.Text = "Ready"
    End Sub

    Sub HideChildDescs()
        MenuDesc.Text = "Ready"
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

    Private Sub HideChildDescsTrigger(sender As Object, e As EventArgs) Handles AppendImage.MouseLeave, ApplyFFU.MouseLeave, ApplyImage.MouseLeave, CaptureCustomImage.MouseLeave, CaptureFFU.MouseLeave, CaptureImage.MouseLeave, CleanupMountpoints.MouseLeave, CommitImage.MouseLeave, DeleteImage.MouseLeave, ExportImage.MouseLeave, GetImageInfo.MouseLeave, GetMountedImageInfo.MouseLeave, GetWIMBootEntry.MouseLeave, ListImage.MouseLeave, MountImage.MouseLeave, OptimizeFFU.MouseLeave, OptimizeImage.MouseLeave, RemountImage.MouseLeave, SplitFFU.MouseLeave, SplitImage.MouseLeave, UnmountImage.MouseLeave, UpdateWIMBootEntry.MouseLeave, ApplySiloedPackage.MouseLeave, GetPackages.MouseLeave, GetPackageInfo.MouseLeave, AddPackage.MouseLeave, RemovePackage.MouseLeave, GetFeatures.MouseLeave, GetFeatureInfo.MouseLeave, EnableFeature.MouseLeave, DisableFeature.MouseLeave, CleanupImage.MouseLeave, AddProvisionedAppxPackage.MouseLeave, GetProvisioningPackageInfo.MouseLeave, ApplyCustomDataImage.MouseLeave, GetProvisionedAppxPackages.MouseLeave, AddProvisionedAppxPackage.MouseLeave, RemoveProvisionedAppxPackage.MouseLeave, OptimizeProvisionedAppxPackages.MouseLeave, SetProvisionedAppxDataFile.MouseLeave, CheckAppPatch.MouseLeave, GetAppPatchInfo.MouseLeave, GetAppPatches.MouseLeave, GetAppInfo.MouseLeave, GetApps.MouseLeave, ExportDefaultAppAssociations.MouseLeave, GetDefaultAppAssociations.MouseLeave, ImportDefaultAppAssociations.MouseLeave, RemoveDefaultAppAssociations.MouseLeave, GetIntl.MouseLeave, SetUILangFallback.MouseLeave, SetSysUILang.MouseLeave, SetSysLocale.MouseLeave, SetUserLocale.MouseLeave, SetInputLocale.MouseLeave, SetAllIntl.MouseLeave, SetTimeZone.MouseLeave, SetSKUIntlDefaults.MouseLeave, SetLayeredDriver.MouseLeave, GenLangINI.MouseLeave, SetSetupUILang.MouseLeave, AddCapability.MouseLeave, ExportSource.MouseLeave, GetCapabilities.MouseLeave, GetCapabilityInfo.MouseLeave, RemoveCapability.MouseLeave, GetCurrentEdition.MouseLeave, GetTargetEditions.MouseLeave, SetEdition.MouseLeave, SetProductKey.MouseLeave, GetDrivers.MouseLeave, GetDriverInfo.MouseLeave, AddDriver.MouseLeave, RemoveDriver.MouseLeave, ExportDriver.MouseLeave, ApplyUnattend.MouseLeave, GetPESettings.MouseLeave, GetTargetPath.MouseLeave, GetScratchSpace.MouseLeave, SetScratchSpace.MouseLeave, SetTargetPath.MouseLeave, GetOSUninstallWindow.MouseLeave, InitiateOSUninstall.MouseLeave, RemoveOSUninstall.MouseLeave, SetOSUninstallWindow.MouseLeave, SetReservedStorageState.MouseLeave, GetReservedStorageState.MouseLeave, NewProjectToolStripMenuItem.MouseLeave, OpenExistingProjectToolStripMenuItem.MouseLeave, SaveProjectToolStripMenuItem.MouseLeave, SaveProjectasToolStripMenuItem.MouseLeave, ExitToolStripMenuItem.MouseLeave, ViewProjectFilesInFileExplorerToolStripMenuItem.MouseLeave, UnloadProjectToolStripMenuItem.MouseLeave, SwitchImageIndexesToolStripMenuItem.MouseLeave, ProjectPropertiesToolStripMenuItem.MouseLeave, ImagePropertiesToolStripMenuItem.MouseLeave, ImageManagementToolStripMenuItem.MouseLeave, OSPackagesToolStripMenuItem.MouseLeave, ProvisioningPackagesToolStripMenuItem.MouseLeave, AppPackagesToolStripMenuItem.MouseLeave, AppPatchesToolStripMenuItem.MouseLeave, DefaultAppAssociationsToolStripMenuItem.MouseLeave, LanguagesAndRegionSettingsToolStripMenuItem.MouseLeave, CapabilitiesToolStripMenuItem.MouseLeave, WindowsEditionsToolStripMenuItem.MouseLeave, DriversToolStripMenuItem.MouseLeave, UnattendedAnswerFilesToolStripMenuItem.MouseLeave, WindowsPEServicingToolStripMenuItem.MouseLeave, OSUninstallToolStripMenuItem.MouseLeave, ReservedStorageToolStripMenuItem.MouseLeave, ImageConversionToolStripMenuItem.MouseLeave, WIMESDToolStripMenuItem.MouseLeave, RemountImageWithWritePermissionsToolStripMenuItem.MouseLeave, CommandShellToolStripMenuItem.MouseLeave, OptionsToolStripMenuItem.MouseLeave, HelpTopicsToolStripMenuItem.MouseLeave, GlossaryToolStripMenuItem.MouseLeave, CommandHelpToolStripMenuItem.MouseLeave, AboutDISMToolsToolStripMenuItem.MouseLeave, UnattendedAnswerFileManagerToolStripMenuItem.MouseLeave, AddEdge.MouseLeave, AddEdgeBrowser.MouseLeave, AddEdgeWebView.MouseLeave, ReportManagerToolStripMenuItem.MouseLeave, MergeSWM.MouseLeave
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

    Private Sub GetMountedImageInfo_MouseEnter(sender As Object, e As EventArgs) Handles GetMountedImageInfo.MouseEnter
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

    Private Sub GetPackageInfo_MouseEnter(sender As Object, e As EventArgs) Handles GetPackageInfo.MouseEnter
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

    Private Sub GetFeatureInfo_MouseEnter(sender As Object, e As EventArgs) Handles GetFeatureInfo.MouseEnter
        ShowChildDescs(True, 29)
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

    Private Sub GetCapabilityInfo_MouseEnter(sender As Object, e As EventArgs) Handles GetCapabilityInfo.MouseEnter
        ShowChildDescs(True, 66)
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

    Private Sub GetDriverInfo_MouseEnter(sender As Object, e As EventArgs) Handles GetDriverInfo.MouseEnter
        ShowChildDescs(True, 73)
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

    Private Sub GetScratchSpace_MouseEnter(sender As Object, e As EventArgs) Handles GetScratchSpace.MouseEnter
        ShowChildDescs(True, 79)
    End Sub

    Private Sub GetTargetPath_MouseEnter(sender As Object, e As EventArgs) Handles GetTargetPath.MouseEnter
        ShowChildDescs(True, 80)
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

    Private Sub NewProject_MouseEnter(sender As Object, e As EventArgs) Handles NewProjectToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 1)
    End Sub

    Private Sub OpenProject_MouseEnter(sender As Object, e As EventArgs) Handles OpenExistingProjectToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 2)
    End Sub

    Private Sub SaveProject_MouseEnter(sender As Object, e As EventArgs) Handles SaveProjectToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 3)
    End Sub

    Private Sub SaveProjAs_MouseEnter(sender As Object, e As EventArgs) Handles SaveProjectasToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 4)
    End Sub

    Private Sub ExitProg_MouseEnter(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 5)
    End Sub

    Private Sub ProjectInExplorer_MouseEnter(sender As Object, e As EventArgs) Handles ViewProjectFilesInFileExplorerToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 6)
    End Sub

    Private Sub UnloadProject_MouseEnter(sender As Object, e As EventArgs) Handles UnloadProjectToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 7)
    End Sub

    Private Sub SwitchIndexes_MouseEnter(sender As Object, e As EventArgs) Handles SwitchImageIndexesToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 8)
    End Sub

    Private Sub ProjProps_MouseEnter(sender As Object, e As EventArgs) Handles ProjectPropertiesToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 9)
    End Sub

    Private Sub ImgProps_MouseEnter(sender As Object, e As EventArgs) Handles ImagePropertiesToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 10)
    End Sub

    Private Sub ImgConversion_MouseEnter(sender As Object, e As EventArgs) Handles ImageConversionToolStripMenuItem.MouseEnter, WIMESDToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 11)
    End Sub

    Private Sub MergeSWM_MouseEnter(sender As Object, e As EventArgs) Handles MergeSWM.MouseEnter
        ShowChildDescs(False, 12)
    End Sub

    Private Sub RemountImg_MouseEnter(sender As Object, e As EventArgs) Handles RemountImageWithWritePermissionsToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 13)
    End Sub

    Private Sub CmdConsole_MouseEnter(sender As Object, e As EventArgs) Handles CommandShellToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 14)
    End Sub

    Private Sub UAFileMan_MouseEnter(sender As Object, e As EventArgs) Handles UnattendedAnswerFileManagerToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 15)
    End Sub

    Private Sub ReportManagerToolStripMenuItem_MouseEnter(sender As Object, e As EventArgs) Handles ReportManagerToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 16)
    End Sub

    Private Sub ProgSettings_MouseEnter(sender As Object, e As EventArgs) Handles OptionsToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 17)
    End Sub

    Private Sub HelpTopics_MouseEnter(sender As Object, e As EventArgs) Handles HelpTopicsToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 18)
    End Sub

    Private Sub Glossary_MouseEnter(sender As Object, e As EventArgs) Handles GlossaryToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 19)
    End Sub

    Private Sub CmdHelp_MouseEnter(sender As Object, e As EventArgs) Handles CommandHelpToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 20)
    End Sub

    Private Sub ProgInfo_MouseEnter(sender As Object, e As EventArgs) Handles AboutDISMToolsToolStripMenuItem.MouseEnter
        ShowChildDescs(False, 21)
    End Sub
#End Region

    Private Sub NewProjLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles NewProjLink.LinkClicked
        NewProj.ShowDialog()
        If NewProj.DialogResult = Windows.Forms.DialogResult.OK Then
            ProgressPanel.OperationNum = 0
            ProgressPanel.ShowDialog()
        End If
    End Sub

    Private Sub ExistingProjLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles ExistingProjLink.LinkClicked
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If File.Exists(OpenFileDialog1.FileName) Then
                ProgressPanel.OperationNum = 990
                LoadDTProj(OpenFileDialog1.FileName, Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName), False)
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
        PkgInfoCMS.Show(Button6, New Point(24, Button6.Height * 0.75))
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        FeatureInfoCMS.Show(Button8, New Point(24, Button8.Height * 0.75))
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click, ProjectPropertiesToolStripMenuItem.Click
        ProjProperties.TabControl1.SelectedIndex = 0
        ProjProperties.Label1.Text = ProjProperties.TabControl1.SelectedTab.Text & " properties"
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            ProjProperties.Text = ""
        Else
            ProjProperties.Text = ProjProperties.Label1.Text
        End If
        ProjProperties.ShowDialog()
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click, ImagePropertiesToolStripMenuItem.Click
        ProjProperties.TabControl1.SelectedIndex = 1
        ProjProperties.Label1.Text = ProjProperties.TabControl1.SelectedTab.Text & " properties"
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
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
        'If isModified Then

        'Else
        '    UnloadDTProj(False, False)
        'End If
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If isProjectLoaded Then
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
        If Not VolatileMode Then
            SaveDTSettings()
        End If
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        SaveDTProj()
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
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
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe", "/k .\bin\dthelper.bat /sh")
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        ImgUMount.ShowDialog()
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OptionsToolStripMenuItem.Click
        Options.ShowDialog()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ImgMount.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ProgressPanel.MountDir = MountDir
        ' TODO: Add additional options later
        ProgressPanel.OperationNum = 8
        ProgressPanel.ShowDialog(Me)
    End Sub

    Private Sub ExplorerView_Click(sender As Object, e As EventArgs) Handles ExplorerView.Click
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", projPath)
    End Sub

    Private Sub GetImageInfo_Click(sender As Object, e As EventArgs) Handles GetImageInfo.Click
        GetImgInfoDlg.ShowDialog()
    End Sub

    Private Sub prjTreeView_AfterExpand(sender As Object, e As TreeViewEventArgs) Handles prjTreeView.AfterExpand
        Try
            If prjTreeView.SelectedNode.IsExpanded Then
                ExpandCollapseTSB.Text = "Collapse"
                If BackColor = Color.FromArgb(48, 48, 48) Then
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph_dark)
                ElseIf BackColor = Color.White Then
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph)
                End If
            Else
                ExpandCollapseTSB.Text = "Expand"
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
                ExpandCollapseTSB.Text = "Collapse"
                If BackColor = Color.FromArgb(48, 48, 48) Then
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph_dark)
                ElseIf BackColor = Color.White Then
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph)
                End If
            Else
                ExpandCollapseTSB.Text = "Expand"
                If BackColor = Color.FromArgb(48, 48, 48) Then
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph_dark)
                ElseIf BackColor = Color.White Then
                    ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph)
                End If
            End If
        Catch ex As Exception
            ExpandCollapseTSB.Text = "Expand"
            If BackColor = Color.FromArgb(48, 48, 48) Then
                ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph_dark)
            ElseIf BackColor = Color.White Then
                ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph)
            End If
        End Try
    End Sub

    Private Sub prjTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles prjTreeView.AfterSelect
        If prjTreeView.SelectedNode.IsExpanded Then
            ExpandCollapseTSB.Text = "Collapse"
            If BackColor = Color.FromArgb(48, 48, 48) Then
                ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph_dark)
            ElseIf BackColor = Color.White Then
                ExpandCollapseTSB.Image = New Bitmap(My.Resources.collapse_glyph)
            End If
        Else
            ExpandCollapseTSB.Text = "Expand"
            If BackColor = Color.FromArgb(48, 48, 48) Then
                ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph_dark)
            ElseIf BackColor = Color.White Then
                ExpandCollapseTSB.Image = New Bitmap(My.Resources.expand_glyph)
            End If
        End If
        If prjTreeView.SelectedNode.Nodes.Count = 0 Then
            ExpandCollapseTSB.Enabled = False
        Else
            ExpandCollapseTSB.Enabled = True
        End If
    End Sub

    Private Sub ExpandCollapseTSB_Click(sender As Object, e As EventArgs) Handles ExpandCollapseTSB.Click
        If ExpandCollapseTSB.Text = "Expand" Then
            Try
                prjTreeView.SelectedNode.Expand()
            Catch ex As Exception

            End Try
        ElseIf ExpandCollapseTSB.Text = "Collapse" Then
            Try
                prjTreeView.SelectedNode.Collapse()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub AddPackage_Click(sender As Object, e As EventArgs) Handles AddPackage.Click
        AddPackageDlg.ShowDialog()
        Do Until reloadDlg = False
            AddPackageDlg.ShowDialog()
        Loop
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        AddPackageDlg.ShowDialog()
    End Sub

    Private Sub AboutDISMToolsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutDISMToolsToolStripMenuItem.Click
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
        ProgressPanel.OperationNum = 993
        PleaseWaitDialog.pkgSourceImgStr = MountDir
        PleaseWaitDialog.Label2.Text = "Getting package names..."
        PleaseWaitDialog.ShowDialog(Me)
        RemPackage.ShowDialog()
    End Sub

    Private Sub ImgBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ImgBW.DoWork

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        ProgressPanel.OperationNum = 994
        PleaseWaitDialog.featOpType = 0
        PleaseWaitDialog.featSourceImg = MountDir
        PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
        PleaseWaitDialog.ShowDialog(Me)
        EnableFeat.ShowDialog()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        ProgressPanel.OperationNum = 994
        PleaseWaitDialog.featOpType = 1
        PleaseWaitDialog.featSourceImg = MountDir
        PleaseWaitDialog.Label2.Text = "Getting feature names and their state..."
        PleaseWaitDialog.ShowDialog(Me)
        DisableFeat.ShowDialog()
    End Sub

    Private Sub SplitPanels_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles SplitPanels.SplitterMoved
        If SplitPanels.SplitterDistance >= 384 And GroupBox1.Left >= 0 Then
            SplitPanels.SplitterDistance = 384
        ElseIf GroupBox1.Left < 0 Then
            SplitPanels.SplitterDistance = 264
        End If
    End Sub

    Private Sub MainForm_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If GroupBox1.Left < 0 Then
            SplitPanels.SplitterDistance = 264
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        imgCommitOperation = 0
        UnloadDTProj(False, True, True)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        imgCommitOperation = 1
        UnloadDTProj(False, True, True)
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        ProgressPanel.OperationNum = 995
        PleaseWaitDialog.indexesSourceImg = SourceImg
        PleaseWaitDialog.Label2.Text = "Getting image indexes..."
        PleaseWaitDialog.ShowDialog(Me)
        If PleaseWaitDialog.imgIndexes > 1 Then
            ImgIndexSwitch.ShowDialog()
        End If
    End Sub

    Private Sub UnloadBtn_Click(sender As Object, e As EventArgs) Handles UnloadBtn.Click
        ToolStripButton3.PerformClick()
    End Sub
End Class
