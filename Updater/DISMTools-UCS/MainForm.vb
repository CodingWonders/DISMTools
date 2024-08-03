Imports System.IO
Imports System.Net
Imports Microsoft.VisualBasic.ControlChars
Imports System.IO.Compression
Imports Microsoft.Win32

Public Class MainForm

    Dim btnToolTip As New ToolTip()
    Private isMouseDown As Boolean = False
    Private mouseOffset As Point
    Dim pageInt As Integer = 0

    ' Argument variables
    Dim branch As String
    Dim pid As Integer = -1

    ' Progress variables
    Dim msg As String

    ' Necessary variables
    Dim latestVer As String
    Dim relTag As String
    Dim needsMigration As Boolean
    Dim minNoMig As Version

    Dim ReleaseDownloader As New WebClient()

    Dim IsPortable As Boolean

    Dim FileCount As Integer = 0
    Dim CopiedFiles As Integer = 0
    Dim BackupOp As Boolean = True

    Private Sub minBox_MouseEnter(sender As Object, e As EventArgs) Handles minBox.MouseEnter
        minBox.Image = My.Resources.minBox_focus
    End Sub

    Private Sub minBox_MouseLeave(sender As Object, e As EventArgs) Handles minBox.MouseLeave
        minBox.Image = My.Resources.minBox
    End Sub

    Private Sub minBox_MouseDown(sender As Object, e As MouseEventArgs) Handles minBox.MouseDown
        minBox.Image = My.Resources.minBox_down
    End Sub

    Private Sub minBox_MouseUp(sender As Object, e As MouseEventArgs) Handles minBox.MouseUp
        minBox.Image = My.Resources.minBox_focus
    End Sub

    Private Sub minBox_MouseHover(sender As Object, e As EventArgs) Handles minBox.MouseHover
        btnToolTip.SetToolTip(sender, "Minimize")
    End Sub

    Private Sub minBox_Click(sender As Object, e As EventArgs) Handles minBox.Click
        WindowState = FormWindowState.Minimized
    End Sub

    Private Sub closeBox_MouseEnter(sender As Object, e As EventArgs) Handles closeBox.MouseEnter
        closeBox.Image = My.Resources.closebox_focus
    End Sub

    Private Sub closeBox_MouseLeave(sender As Object, e As EventArgs) Handles closeBox.MouseLeave
        closeBox.Image = My.Resources.closebox
    End Sub

    Private Sub closeBox_MouseDown(sender As Object, e As MouseEventArgs) Handles closeBox.MouseDown
        closeBox.Image = My.Resources.closebox_down
    End Sub

    Private Sub closeBox_MouseUp(sender As Object, e As MouseEventArgs) Handles closeBox.MouseUp
        closeBox.Image = My.Resources.closebox_focus
    End Sub

    Private Sub closeBox_MouseHover(sender As Object, e As EventArgs) Handles closeBox.MouseHover
        btnToolTip.SetToolTip(sender, "Close")
    End Sub

    Private Sub closeBox_Click(sender As Object, e As EventArgs) Handles closeBox.Click
        Close()
    End Sub

    Private Sub wndControlPanel_MouseDown(sender As Object, e As MouseEventArgs) Handles wndControlPanel.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            ' Get the new position
            mouseOffset = New Point(-e.X, -e.Y)
            ' Set that left button is pressed
            isMouseDown = True
        End If
    End Sub

    Private Sub wndControlPanel_MouseMove(sender As Object, e As MouseEventArgs) Handles wndControlPanel.MouseMove
        If isMouseDown Then
            Dim mousePos As Point = Control.MousePosition
            ' Get the new form position
            mousePos.Offset(mouseOffset.X, mouseOffset.Y)
            Location = mousePos
        End If
    End Sub

    Private Sub wndControlPanel_MouseUp(sender As Object, e As MouseEventArgs) Handles wndControlPanel.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            isMouseDown = False
        End If
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
        If File.Exists(Application.StartupPath & "\portable") Then IsPortable = True
        Label1.Text = "DISMTools Update Check System - Version " & Application.ProductVersion
        If Directory.Exists(Application.StartupPath & "\new") Then Directory.Delete(Application.StartupPath & "\new", True)
        GetArguments()
        Try
            Dim ColorModeRk As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize")
            Dim ColorMode As String = ColorModeRk.GetValue("AppsUseLightTheme").ToString()
            ColorModeRk.Close()
            If ColorMode = "0" Then
                ForeColor = Color.White
                PictureBox1.Image = My.Resources.check_dark
                PictureBox2.Image = My.Resources.check_dark
                PictureBox3.Image = My.Resources.check_dark
                PictureBox4.Image = My.Resources.check_dark
                WelcomePanel.BackColor = Color.FromArgb(48, 48, 48)
                UpdatePanel.BackColor = Color.FromArgb(48, 48, 48)
                FinishPanel.BackColor = Color.FromArgb(48, 48, 48)
                WelcomePanel.ForeColor = Color.White
                UpdatePanel.ForeColor = Color.White
                FinishPanel.ForeColor = Color.White
            ElseIf ColorMode = "1" Then
                ForeColor = Color.Black
                PictureBox1.Image = My.Resources.check
                PictureBox2.Image = My.Resources.check
                PictureBox3.Image = My.Resources.check
                PictureBox4.Image = My.Resources.check
                WelcomePanel.BackColor = Color.FromArgb(239, 239, 242)
                UpdatePanel.BackColor = Color.FromArgb(239, 239, 242)
                FinishPanel.BackColor = Color.FromArgb(239, 239, 242)
                WelcomePanel.ForeColor = Color.Black
                UpdatePanel.ForeColor = Color.Black
                FinishPanel.ForeColor = Color.Black
            End If
        Catch ex As Exception
            ForeColor = Color.Black
            PictureBox1.Image = My.Resources.check
            PictureBox2.Image = My.Resources.check
            PictureBox3.Image = My.Resources.check
            PictureBox4.Image = My.Resources.check
            WelcomePanel.BackColor = Color.FromArgb(239, 239, 242)
            UpdatePanel.BackColor = Color.FromArgb(239, 239, 242)
            FinishPanel.BackColor = Color.FromArgb(239, 239, 242)
            WelcomePanel.ForeColor = Color.Black
            UpdatePanel.ForeColor = Color.Black
            FinishPanel.ForeColor = Color.Black
        End Try

        If Not Environment.OSVersion.Version.Major >= 10 Or Not (DetectFont("Segoe UI Variable Display Semib") Or DetectFont("Segoe UI Variable Semib")) Then
            Label3.Font = New Font("Segoe UI", Label3.Font.Size, FontStyle.Regular)
            Label8.Font = New Font("Segoe UI", Label8.Font.Size, FontStyle.Regular)
            Label15.Font = New Font("Segoe UI", Label15.Font.Size, FontStyle.Regular)
        End If
        ReleaseFetcherBW.RunWorkerAsync()
    End Sub

    Function DetectFont(FontName As String) As Boolean
        For Each fntFamily As FontFamily In FontFamily.Families
            If fntFamily.Name = FontName Then
                Return True
            End If
        Next
        Return False
    End Function

    Sub GetArguments()
        If Environment.GetCommandLineArgs.Length = 1 Then
            MsgBox("The branch parameter is necessary to be able to check for updates", vbOKOnly + vbCritical, Text)
            Environment.Exit(1)
        Else
            Dim args() As String = Environment.GetCommandLineArgs()
            branch = args(1).Replace("/", "").Trim()
            If Environment.GetCommandLineArgs.Length >= 3 Then
                If args(2).StartsWith("/pid=", StringComparison.OrdinalIgnoreCase) Then pid = args(2).Replace("/pid=", "").Trim()
            End If
        End If
    End Sub

    Private Sub ReleaseFetcherBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ReleaseFetcherBW.DoWork
        msg = "Fetching update server..."
        Threading.Thread.Sleep(3000)
        ReleaseFetcherBW.ReportProgress(0)
        FetchUpdates()
        msg = "Comparing versions..."
        ReleaseFetcherBW.ReportProgress(50)
        CompareVersions()
    End Sub

    Private Sub ReleaseFetcherBW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles ReleaseFetcherBW.ProgressChanged
        ProgressBar1.Style = ProgressBarStyle.Blocks
        ProgressBar1.Value = e.ProgressPercentage
        Label4.Text = msg
        If e.ProgressPercentage = 100 Then
            Label3.Text = "Update information"
            Label6.Text = FileVersionInfo.GetVersionInfo(Application.StartupPath & "\DISMTools.exe").FileVersion.ToString() & " → " & latestVer
            Panel1.Visible = True
            Label4.Visible = False
            ProgressBar1.Visible = False
        End If
    End Sub

    Sub FetchUpdates()
        Using client As New WebClient()
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            Try
                client.DownloadFile("https://raw.githubusercontent.com/CodingWonders/DISMTools/" & branch & "/Updater/DISMTools-UCS/update-bin/" & If(branch.Contains("pre"), "preview.ini", "stable.ini"), Application.StartupPath & "\info.ini")
            Catch ex As WebException
                MsgBox("We couldn't fetch the necessary update information. Reason:" & CrLf & ex.Status.ToString(), vbOKOnly + vbCritical, Text)
                Environment.Exit(1)
            End Try
            If File.Exists(Application.StartupPath & "\info.ini") Then
                Dim infoRTB As New RichTextBox With {
                    .Text = File.ReadAllText(Application.StartupPath & "\info.ini")
                }
                For Each Line In infoRTB.Lines
                    If Line.StartsWith("LatestVer") Then
                        latestVer = Line.Replace("LatestVer = ", "").Trim()
                    ElseIf Line.StartsWith("ReleaseTag") Then
                        relTag = Line.Replace("ReleaseTag = ", "").Trim()
                    ElseIf Line.StartsWith("MigrateSettings") Then
                        needsMigration = If(Line.Replace("MigrateSettings = ", "").Trim() = "True", True, False)
                    ElseIf Line.StartsWith("MinNoMig") Then
                        Try
                            Dim minNoMigStr As String = Line.Replace("MinNoMig = ", "").Trim()
                            minNoMig = New Version(minNoMigStr)
                            Dim fv As String = FileVersionInfo.GetVersionInfo(Application.StartupPath & "\DISMTools.exe").ProductVersion.ToString()
                            needsMigration = Not (New Version(fv) >= minNoMig)
                        Catch ex As Exception
                            Debug.WriteLine("Could not get minimum version for no migrations")
                        End Try
                    End If
                Next
                Label17.Visible = needsMigration
                File.Delete(Application.StartupPath & "\info.ini")
            End If
        End Using
    End Sub

    Sub CompareVersions()
        If File.Exists(Application.StartupPath & "\DISMTools.exe") Then
            Dim fv As String = FileVersionInfo.GetVersionInfo(Application.StartupPath & "\DISMTools.exe").ProductVersion.ToString()
            If fv = latestVer Or New Version(fv) > New Version(latestVer) Then
                MsgBox("There aren't any updates available", vbOKOnly + vbInformation, Text)
                End
            Else
                ReleaseFetcherBW.ReportProgress(100)
            End If
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://github.com/CodingWonders/DISMTools/releases/tag/" & relTag)
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If UpdaterBW.IsBusy Then
            e.Cancel = True
            Beep()
            Exit Sub
        End If
    End Sub

    Private Sub UpdaterBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles UpdaterBW.DoWork
        PictureBox1.Visible = False
        PictureBox2.Visible = False
        PictureBox3.Visible = False
        PictureBox4.Visible = False
        msg = "Downloading the latest release..."
        Label10.ForeColor = ForeColor
        Label11.ForeColor = Color.Gray
        Label12.ForeColor = Color.Gray
        Label13.ForeColor = Color.Gray
        Label10.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        Label11.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        Label12.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        Label13.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        UpdaterBW.ReportProgress(5)
        DownloadAsync().Wait()
        Threading.Thread.Sleep(500)
        Label10.Text = "Downloading the update"
        Label10.ForeColor = Color.Gray
        Label11.ForeColor = ForeColor
        Label12.ForeColor = Color.Gray
        Label13.ForeColor = Color.Gray
        Label10.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        Label11.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        Label12.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        Label13.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        PictureBox1.Visible = True
        PictureBox2.Visible = False
        PictureBox3.Visible = False
        PictureBox4.Visible = False
        PrepareUpdateInstallation()
        Label11.Text = "Preparing update installation"
        Label10.ForeColor = Color.Gray
        Label11.ForeColor = Color.Gray
        Label12.ForeColor = ForeColor
        Label13.ForeColor = Color.Gray
        Label10.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        Label11.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        Label12.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        Label13.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        PictureBox1.Visible = True
        PictureBox2.Visible = True
        PictureBox3.Visible = False
        PictureBox4.Visible = False
        InstallUpdate()
        Label12.Text = "Installing the update"
        PictureBox1.Visible = True
        PictureBox2.Visible = True
        PictureBox3.Visible = True
        PictureBox4.Visible = False
        Label10.ForeColor = Color.Gray
        Label11.ForeColor = Color.Gray
        Label12.ForeColor = Color.Gray
        Label13.ForeColor = ForeColor
        Label10.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        Label11.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        Label12.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        Label13.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        msg = "Deleting backup files..."
        UpdaterBW.ReportProgress(87.5)
        CleanBackupFiles()
        msg = "Done."
        Label10.ForeColor = Color.Gray
        Label11.ForeColor = Color.Gray
        Label12.ForeColor = Color.Gray
        Label13.ForeColor = Color.Gray
        Label10.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        Label11.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        Label12.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        Label13.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        PictureBox1.Visible = True
        PictureBox2.Visible = True
        PictureBox3.Visible = True
        PictureBox4.Visible = True
        UpdaterBW.ReportProgress(100)
        Threading.Thread.Sleep(1000)
    End Sub

    Private Sub UpdaterBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles UpdaterBW.RunWorkerCompleted
        If CheckBox1.Checked Then
            Process.Start(Application.StartupPath & "\DISMTools.exe", If(needsMigration, "/migrate", ""))
            Environment.Exit(0)
        Else
            WelcomePanel.Visible = False
            UpdatePanel.Visible = False
            FinishPanel.Visible = True
        End If
    End Sub

    Sub DownloadRelease(ReleaseTag As String)
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        If Not Directory.Exists(Application.StartupPath & "\new") Then Directory.CreateDirectory(Application.StartupPath & "\new")
        ReleaseDownloader.DownloadFile("https://github.com/CodingWonders/DISMTools/releases/download/" & ReleaseTag & "/" & If(IsPortable, "DISMTools.zip", "dt_setup.exe"), Application.StartupPath & "\new\" & If(IsPortable, "DISMTools.zip", "dt_setup.exe"))
    End Sub

    Async Function DownloadReleaseAsync(url As String, path As String, worker As System.ComponentModel.BackgroundWorker) As Task(Of Integer)
        AddHandler ReleaseDownloader.DownloadProgressChanged, Sub(sender, e)
                                                                  Label10.Text = "Downloading the update (" & e.ProgressPercentage & "%)"
                                                                  worker.ReportProgress(e.ProgressPercentage)
                                                              End Sub
        Dim data As Byte() = Await ReleaseDownloader.DownloadDataTaskAsync(url)
        File.WriteAllBytes(path, data)
        Return data.Length
    End Function

    Async Function DownloadAsync() As Task
        Dim dwWorker As New System.ComponentModel.BackgroundWorker()
        dwWorker.WorkerReportsProgress = True
        AddHandler dwWorker.ProgressChanged, Sub(sender, e)
                                                 msg = "Downloading updated program files..."
                                                 UpdaterBW.ReportProgress(0)
                                             End Sub
        AddHandler dwWorker.RunWorkerCompleted, Sub(sender, e)
                                                    If e.Error IsNot Nothing Then
                                                        ChangeSteps(2, False)
                                                    Else
                                                        ChangeSteps(2, True)
                                                    End If
                                                End Sub
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        If Not Directory.Exists(Application.StartupPath & "\new") Then Directory.CreateDirectory(Application.StartupPath & "\new")
        Dim bytes As Integer = Await DownloadReleaseAsync("https://github.com/CodingWonders/DISMTools/releases/download/" & relTag & "/" & If(IsPortable, "DISMTools.zip", "dt_setup.exe"), Application.StartupPath & "\new\" & If(IsPortable, "DISMTools.zip", "dt_setup.exe"), dwWorker)
    End Function

    Sub PrepareUpdateInstallation()
        If IsPortable Then
            msg = "Extracting the release..."
            UpdaterBW.ReportProgress(25)
            ExpandContents()
        End If
        Label11.Text = "Preparing update installation (80%)"
        msg = "Waiting for processes to close..."
        UpdaterBW.ReportProgress(47.5)
        CloseMainProcess()
        Threading.Thread.Sleep(500)
    End Sub

    Sub InstallUpdate()
        msg = "Backing up old installation..."
        UpdaterBW.ReportProgress(50)
        BackupInstallation()
        msg = "Deleting files..."
        UpdaterBW.ReportProgress(62.5)
        DeleteInstallation()
        Threading.Thread.Sleep(1000)
        msg = "Copying files..."
        UpdaterBW.ReportProgress(70)
        InstallNewVersion()
        Threading.Thread.Sleep(500)
    End Sub

    Sub ExpandContents()
        If Not IsPortable Then Exit Sub
        Try
            Using archive As ZipArchive = ZipFile.OpenRead(Application.StartupPath & "\new\DISMTools.zip")
                Dim TotalEntries As Integer = archive.Entries.Count
                Dim ExtractedEntries As Integer = 0
                For Each entry As ZipArchiveEntry In archive.Entries
                    Dim dest As String = Path.Combine(Application.StartupPath & "\new", entry.FullName.Replace("/", "\").Trim())
                    Debug.WriteLine(dest)
                    Directory.CreateDirectory(Path.GetDirectoryName(dest))
                    If Not entry.FullName.EndsWith("/", StringComparison.OrdinalIgnoreCase) Then
                        ZipFileExtensions.ExtractToFile(entry, dest)
                    End If
                    ExtractedEntries += 1
                    Dim progress As Double = CDbl(ExtractedEntries / TotalEntries)
                    Label11.Text = "Preparing update installation (" & Math.Round(80 * progress, 0) & "%)"
                Next
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Sub CloseMainProcess()
        ' New method
        If pid <> -1 Then
            Dim Procs As Process = Process.GetProcessById(pid)
            Procs.CloseMainWindow()
            Do Until Procs.HasExited
                Application.DoEvents()
                Threading.Thread.Sleep(500)
            Loop
            Label11.Text = "Preparing update installation (100%)"
            Exit Sub
        Else
            Dim Procs() As Process = Process.GetProcessesByName("DISMTools")
            For Each Proc As Process In Procs
                Proc.CloseMainWindow()
                Do Until Proc.HasExited
                    Application.DoEvents()
                    Threading.Thread.Sleep(500)
                Loop
            Next
            Label11.Text = "Preparing update installation (100%)"
            Exit Sub
        End If
    End Sub

    Sub BackupInstallation()
        Label12.Text = "Installing the update (0%)"
        FileCount = Directory.GetFiles(Application.StartupPath, "*", SearchOption.AllDirectories).Length
        CopiedFiles = 0
        FileCount -= (1 + If(Directory.Exists(Application.StartupPath & "\logs"), Directory.GetFiles(Application.StartupPath & "\logs", "*", SearchOption.AllDirectories).Length, 0) + _
                      If(Directory.Exists(Application.StartupPath & "\new"), Directory.GetFiles(Application.StartupPath & "\new", "*", SearchOption.AllDirectories).Length, 0))
        If Not Directory.Exists(Application.StartupPath & "\old") Then Directory.CreateDirectory(Application.StartupPath & "\old")
        Directory.CreateDirectory(Application.StartupPath & "\old\Resources")
        Directory.CreateDirectory(Application.StartupPath & "\old\bin")
        For Each DLLFile In Directory.GetFiles(Application.StartupPath, "*.dll")
            File.Copy(DLLFile, Path.Combine(Application.StartupPath, "old", Path.GetFileName(DLLFile)), True)
            CopiedFiles += 1
            Label12.Text = "Installing the update (" & Math.Round(30 * (CopiedFiles / FileCount), 0) & "%)"
        Next
        DirCopy(Application.StartupPath & "\AutoUnattend", Application.StartupPath & "\old\AutoUnattend", True, False)
        DirCopy(Application.StartupPath & "\Resources", Application.StartupPath & "\old\Resources", True, False)
        DirCopy(Application.StartupPath & "\bin", Application.StartupPath & "\old\bin", True, False)
        DirCopy(Application.StartupPath & "\docs", Application.StartupPath & "\old\docs", True, False)
        DirCopy(Application.StartupPath & "\tools", Application.StartupPath & "\old\tools", True, False)
        DirCopy(Application.StartupPath & "\runtimes", Application.StartupPath & "\old\runtimes", True, False)
        DirCopy(Application.StartupPath & "\videos", Application.StartupPath & "\old\videos", True, False)
        File.Copy(Application.StartupPath & "\LICENSE", Application.StartupPath & "\old\LICENSE")
        File.Copy(Application.StartupPath & "\DISMTools.exe", Application.StartupPath & "\old\DISMTools.exe")
        CopiedFiles += 2
        Label12.Text = "Installing the update (" & Math.Round(30 * (CopiedFiles / FileCount), 0) & "%)"
    End Sub

    Sub DirCopy(sourceDir As String, destDir As String, ovr As Boolean, Backup As Boolean)
        Try
            Dim dir As DirectoryInfo = New DirectoryInfo(sourceDir)
            If Not dir.Exists Then
                Throw New DirectoryNotFoundException("The directory could not be found")
            End If
            Dim files As FileInfo() = dir.GetFiles()
            If Not Directory.Exists(destDir) Then
                Directory.CreateDirectory(destDir)
            End If
            For Each DirFile As FileInfo In files
                Dim tempPath As String = Path.Combine(destDir, DirFile.Name)
                DirFile.CopyTo(tempPath, ovr)
                CopiedFiles += 1
                Label12.Text = "Installing the update (" & If(Backup, Math.Round(30 * (CopiedFiles / FileCount), 0), 30 + Math.Round(70 * (CopiedFiles / FileCount), 0)) & "%)"
            Next
            Dim dirs As DirectoryInfo() = dir.GetDirectories()
            For Each subDir As DirectoryInfo In dirs
                Dim tempPath As String = Path.Combine(destDir, subDir.Name)
                DirCopy(subDir.FullName, tempPath, ovr, BackupOp)
            Next
        Catch ex As Exception

        End Try
    End Sub

    Sub DeleteInstallation()
        For Each dll In My.Computer.FileSystem.GetFiles(Application.StartupPath, FileIO.SearchOption.SearchTopLevelOnly, "*.dll")
            File.Delete(dll)
        Next
        Directory.Delete(Application.StartupPath & "\bin", True)
        If Directory.Exists(Application.StartupPath & "\Resources") Then Directory.Delete(Application.StartupPath & "\Resources", True)
        File.Delete(Application.StartupPath & "\DISMTools.exe")
        File.Delete(Application.StartupPath & "\LICENSE")
    End Sub

    Sub InstallNewVersion()
        BackupOp = False
        If Not IsPortable And File.Exists(Application.StartupPath & "\new\dt_setup.exe") Then
            Dim Installer As New Process()
            Installer.StartInfo.FileName = Application.StartupPath & "\new\dt_setup.exe"
            Installer.StartInfo.Arguments = "/VERYSILENT /SUPPRESSMSGBOXES"
            Installer.StartInfo.CreateNoWindow = True
            Installer.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            Installer.Start()
            Installer.WaitForExit()
            If Installer.ExitCode <> 0 Then
                Debug.WriteLine("An error occured installing the new version.")
            End If
            Label12.Text = "Installing the update (100%)"
            Exit Sub
        End If
        ' Count everything in there except for DISMTools.zip and the portable file
        FileCount = Directory.GetFiles(Application.StartupPath & "\new", "*", SearchOption.AllDirectories).Length - 2
        CopiedFiles = 0
        For Each rootFile In Directory.GetFiles(Application.StartupPath & "\new", "*", SearchOption.TopDirectoryOnly)
            If Path.GetFileName(rootFile) <> "DISMTools.zip" And Path.GetFileName(rootFile) <> "portable" Then
                File.Copy(rootFile, Path.Combine(Application.StartupPath, Path.GetFileName(rootFile)), True)
                CopiedFiles += 1
                Label12.Text = "Installing the update (" & 30 + Math.Round(70 * (CopiedFiles / FileCount), 0) & "%)"
            End If
        Next
        DirCopy(Application.StartupPath & "\new\AutoUnattend", Application.StartupPath & "\AutoUnattend", True, False)
        DirCopy(Application.StartupPath & "\new\Resources", Application.StartupPath & "\Resources", True, False)
        DirCopy(Application.StartupPath & "\new\bin", Application.StartupPath & "\bin", True, False)
        DirCopy(Application.StartupPath & "\new\docs", Application.StartupPath & "\docs", True, False)
        DirCopy(Application.StartupPath & "\new\tools", Application.StartupPath & "\tools", True, False)
        DirCopy(Application.StartupPath & "\new\runtimes", Application.StartupPath & "\runtimes", True, False)
        DirCopy(Application.StartupPath & "\new\videos", Application.StartupPath & "\videos", True, False)
        If IsPortable And Not File.Exists(Application.StartupPath & "\portable") Then File.Create(Application.StartupPath & "\portable")
    End Sub

    Sub CleanBackupFiles()
        Directory.Delete(Application.StartupPath & "\new", True)
        Directory.Delete(Application.StartupPath & "\old", True)
        If Not IsPortable And File.Exists(Application.StartupPath & "\portable") Then
            File.SetAttributes(Application.StartupPath & "\portable", FileAttributes.Normal)
            File.Delete(Application.StartupPath & "\portable")
        Else
            If Not File.Exists(Application.StartupPath & "\portable") Then Exit Sub
            If Not File.GetAttributes(Application.StartupPath & "\portable") = FileAttributes.Hidden Then
                File.SetAttributes(Application.StartupPath & "\portable", FileAttributes.Hidden)
            End If
        End If
    End Sub

    Private Sub UpdaterBW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles UpdaterBW.ProgressChanged
        Label14.Text = msg
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        WelcomePanel.Visible = False
        UpdatePanel.Visible = True
        FinishPanel.Visible = False
        UpdaterBW.RunWorkerAsync()
    End Sub

    Sub ChangeSteps(Steps As Integer, Success As Boolean)
        Select Case Steps
            Case 1
                PictureBox1.Visible = False
                PictureBox2.Visible = False
                PictureBox3.Visible = False
                PictureBox4.Visible = False
                Label10.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                Label11.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label12.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label13.Font = New Font("Segoe UI", 9, FontStyle.Regular)
            Case 2
                PictureBox1.Visible = True
                PictureBox2.Visible = False
                PictureBox3.Visible = False
                PictureBox4.Visible = False
                Label10.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label11.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                Label12.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label13.Font = New Font("Segoe UI", 9, FontStyle.Regular)
            Case 3
                PictureBox1.Visible = True
                PictureBox2.Visible = True
                PictureBox3.Visible = False
                PictureBox4.Visible = False
                Label10.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label11.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label12.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                Label13.Font = New Font("Segoe UI", 9, FontStyle.Regular)
            Case 4
                PictureBox1.Visible = True
                PictureBox2.Visible = True
                PictureBox3.Visible = True
                PictureBox4.Visible = False
                Label10.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label11.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label12.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label13.Font = New Font("Segoe UI", 9, FontStyle.Bold)
            Case 5
                PictureBox1.Visible = True
                PictureBox2.Visible = True
                PictureBox3.Visible = True
                PictureBox4.Visible = True
                Label10.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label11.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label12.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label13.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        End Select
        Refresh()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Process.Start(Application.StartupPath & "\DISMTools.exe", If(needsMigration, "/migrate", ""))
        Environment.Exit(0)
    End Sub
End Class
