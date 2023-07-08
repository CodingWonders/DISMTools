Imports System.IO
Imports System.Net
Imports Microsoft.VisualBasic.ControlChars
Imports System.IO.Compression

Public Class MainForm

    Dim btnToolTip As New ToolTip()
    Private isMouseDown As Boolean = False
    Private mouseOffset As Point
    Dim pageInt As Integer = 0

    ' Argument variables
    Dim branch As String

    ' Progress variables
    Dim msg As String

    ' Necessary variables
    Dim latestVer As String
    Dim relTag As String
    Dim needsMigration As Boolean

    Dim ReleaseDownloader As New WebClient()

    Dim IsPortable As Boolean

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
        ReleaseFetcherBW.RunWorkerAsync()
    End Sub

    Sub GetArguments()
        If Environment.GetCommandLineArgs.Length = 1 Then
            MsgBox("The branch parameter is necessary to be able to check for updates", vbOKOnly + vbCritical, Text)
            Environment.Exit(1)
        Else
            Dim args() As String = Environment.GetCommandLineArgs()
            branch = args(1).Replace("/", "").Trim()
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
                client.DownloadFile("https://raw.githubusercontent.com/CodingWonders/DISMTools/" & branch & "/Updater/DISMTools-UCS/update-bin/" & If(branch.Contains("preview"), "preview.ini", "stable.ini"), Application.StartupPath & "\info.ini")
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
            If fv = latestVer Then
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
        DownloadRelease(relTag)
        msg = "Extracting the release..."
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
        UpdaterBW.ReportProgress(25)
        ExpandContents()
        msg = "Closing the program..."
        UpdaterBW.ReportProgress(47.5)
        CloseMainProcess()
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
        msg = "Backing up old installation..."
        UpdaterBW.ReportProgress(50)
        BackupInstallation()
        msg = "Deleting files..."
        UpdaterBW.ReportProgress(62.5)
        DeleteInstallation()
        msg = "Copying files..."
        UpdaterBW.ReportProgress(70)
        InstallNewVersion()
        Label10.ForeColor = Color.Gray
        Label11.ForeColor = Color.Gray
        Label12.ForeColor = Color.Gray
        Label13.ForeColor = ForeColor
        Label10.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        Label11.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        Label12.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        Label13.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        PictureBox1.Visible = True
        PictureBox2.Visible = True
        PictureBox3.Visible = True
        PictureBox4.Visible = False
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
        ReleaseDownloader.DownloadFile("https://github.com/CodingWonders/DISMTools/releases/download/" & ReleaseTag & "/DISMTools.zip", Application.StartupPath & "\new\DISMTools.zip")
    End Sub

    Sub ExpandContents()
        Dim Expander As New Process()
        Expander.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
        Expander.StartInfo.Arguments = "-command Expand-Archive -Path '" & Application.StartupPath & "\new\DISMTools.zip' -DestinationPath '" & Application.StartupPath & "\new'"
        Expander.StartInfo.CreateNoWindow = True
        Expander.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        Expander.Start()
        Expander.WaitForExit()
        If Expander.ExitCode <> 0 Then
            ZipFile.ExtractToDirectory(Application.StartupPath & "\new\DISMTools.zip", Application.StartupPath & "\new")
        End If
        File.Delete(Application.StartupPath & "\new\DISMTools.zip")
    End Sub

    Sub CloseMainProcess()
        Dim Closer As New Process()
        Closer.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
        Closer.StartInfo.Arguments = "-command Get-Process DISMTools | Foreach-Object { $_.CloseMainWindow() | Out-Null }"
        Closer.StartInfo.CreateNoWindow = True
        Closer.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        Closer.Start()
        Closer.WaitForExit()
    End Sub

    Sub BackupInstallation()
        Dim Backupper As New Process()
        Backupper.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
        Backupper.StartInfo.CreateNoWindow = True
        Backupper.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        If Not Directory.Exists(Application.StartupPath & "\old") Then Directory.CreateDirectory(Application.StartupPath & "\old")
        Directory.CreateDirectory(Application.StartupPath & "\old\Resources")
        Directory.CreateDirectory(Application.StartupPath & "\old\bin")
        Backupper.StartInfo.Arguments = "-command Copy-Item -Path *.dll -Destination '" & Application.StartupPath & "\old'"
        Backupper.Start()
        Backupper.WaitForExit()
        Backupper.StartInfo.Arguments = "-command Copy-Item -Path '" & Application.StartupPath & "\Resources' -Destination '" & Application.StartupPath & "\old\Resources' -Recurse -Force"
        Backupper.Start()
        Backupper.WaitForExit()
        Backupper.StartInfo.Arguments = "-command Copy-Item -Path '" & Application.StartupPath & "\bin' -Destination '" & Application.StartupPath & "\old\bin' -Recurse -Force"
        Backupper.Start()
        Backupper.WaitForExit()
        File.Copy(Application.StartupPath & "\LICENSE", Application.StartupPath & "\old\LICENSE")
        File.Copy(Application.StartupPath & "\DISMTools.exe", Application.StartupPath & "\old\DISMTools.exe")
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
        Dim Updater As New Process()
        Updater.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
        Updater.StartInfo.WorkingDirectory = Application.StartupPath
        If Not Debugger.IsAttached Then
            Updater.StartInfo.CreateNoWindow = True
            Updater.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        End If
        Updater.StartInfo.Arguments = "-command Copy-Item -Path '" & Application.StartupPath & "\new\*' -Exclude DISMTools.zip" & If(Debugger.IsAttached, " -Verbose", "")
        Updater.Start()
        Updater.WaitForExit()
        Updater.StartInfo.Arguments = "-command Copy-Item -Path '" & Application.StartupPath & "\new\Resources' -Destination '" & Application.StartupPath & "' -Recurse -Force" & If(Debugger.IsAttached, " -Verbose", "")
        Updater.Start()
        Updater.WaitForExit()
        Updater.StartInfo.Arguments = "-command Copy-Item -Path '" & Application.StartupPath & "\new\bin' -Destination '" & Application.StartupPath & "' -Recurse -Force" & If(Debugger.IsAttached, " -Verbose", "")
        Updater.Start()
        Updater.WaitForExit()
    End Sub

    Sub CleanBackupFiles()
        Directory.Delete(Application.StartupPath & "\new", True)
        Directory.Delete(Application.StartupPath & "\old", True)
        If Not IsPortable Then
            File.SetAttributes(Application.StartupPath & "\portable", FileAttributes.Normal)
            File.Delete(Application.StartupPath & "\portable")
        Else
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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Process.Start(Application.StartupPath & "\DISMTools.exe", If(needsMigration, "/migrate", ""))
        Environment.Exit(0)
    End Sub
End Class
