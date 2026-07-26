Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports DISMTools.Utilities
Imports System.Net
Imports Microsoft.Win32
Imports System.Threading.Tasks
Imports DISMTools.Elements.ISOCreation

Public Class ISOCreator

    Dim ImageInfoCollection As DismImageInfoCollection
    Dim ISOMsg As String = ""
    Dim success As Boolean
    Dim architectures() As String = New String(2) {"x86", "amd64", "arm64"}
    Dim adkDownloadLocations() As String = New String(1) {"https://download.microsoft.com/download/615540bc-be0b-433a-b91b-1f2b0642bb24/adk/adksetup.exe", "https://download.microsoft.com/download/2472e9a0-7c74-4ffd-a3e4-27ed1fa30d30/adkwinpeaddons/adkwinpesetup.exe"}
    Dim adkDownloadSuccess As Boolean

    ' Job manager for concurrent ISO creation tasks
    Private Shared _jobManager As IsoCreationJobManager = New IsoCreationJobManager(maxConcurrentTasks:=MainForm.PEHelper_MaxConcurrentISO)
    Private _currentJobId As Integer = -1

    Private Sub ISOCreator_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("ISOCreator")("CreateIsofile.Label")
        ImageTaskHeader1.ItemText = Text
        Label2.Text = LocalizationService.ForSection("ISOCreator")("ISO.File.Message")
        Label3.Text = LocalizationService.ForSection("ISOCreator")("Re.Ready.Create.Label")
        Label4.Text = LocalizationService.ForSection("ISOCreator")("ImageFile.Add.Label")
        Label6.Text = LocalizationService.ForSection("ISOCreator")("Architecture.Label")
        Label7.Text = LocalizationService.ForSection("ISOCreator")("Target.Isolocation.Label")
        Label8.Text = LocalizationService.ForSection("ISOCreator")("Jobs.Label")
        Button1.Text = LocalizationService.ForSection("ISOCreator")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("ISOCreator")("Pick.Button")
        Button3.Text = LocalizationService.ForSection("ISOCreator")("Browse.Button")
        Button4.Text = LocalizationService.ForSection("ISOCreator")("Mounted.Image.Button")
        Button5.Text = LocalizationService.ForSection("ISOCreator")("Browse.Button")
        Button6.Text = LocalizationService.ForSection("ISOCreator")("Customize.Environment.Button")
        OK_Button.Text = LocalizationService.ForSection("ISOCreator")("Create.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ISOCreator")("Cancel.Button")
        GroupBox1.Text = LocalizationService.ForSection("ISOCreator")("Options.Group")
        GroupBox2.Text = LocalizationService.ForSection("ISOCreator")("Progress.Group")
        LinkLabel1.Text = LocalizationService.ForSection("ISOCreator")("Download.Windows.ADK.Link")
        ColumnHeader2.Text = LocalizationService.ForSection("ISOCreator")("ImageName.Column")
        ColumnHeader3.Text = LocalizationService.ForSection("ISOCreator")("ImageDescription.Column")
        ColumnHeader4.Text = LocalizationService.ForSection("ISOCreator")("ImageVersion.Column")
        ColumnHeader5.Text = LocalizationService.ForSection("ISOCreator")("Image.Architecture.Column")
        ColumnHeader6.Text = LocalizationService.ForSection("ISOCreator")("JobId.Column")
        ColumnHeader7.Text = LocalizationService.ForSection("ISOCreator")("DestinationFile.Column")
        ColumnHeader8.Text = LocalizationService.ForSection("ISOCreator")("Status.Column")
        CheckBox1.Text = LocalizationService.ForSection("ISOCreator")("Unattended.CheckBox")
        CheckBox2.Text = LocalizationService.ForSection("ISOCreator")("Copy.Ventoy.Drives.CheckBox")
        CheckBox3.Text = LocalizationService.ForSection("ISOCreator")("Newly.Signed.Boot.CheckBox")
        CheckBox4.Text = LocalizationService.ForSection("ISOCreator")("Include.Essential.CheckBox")
        ImageTaskHeader1.SetColors()
        ImageTaskHeader1.HideWindowTitle(Me.Handle)
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        CreationJobsLV.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox3.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox4.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        CreationJobsLV.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        GroupBox2.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        If MainForm.SourceImg = "N/A" Or Not File.Exists(MainForm.SourceImg) Or MainForm.OnlineManagement Or MainForm.OfflineManagement Then
            Button4.Enabled = False
        Else
            Button4.Enabled = True
        End If
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ' Set disabled ListView's backcolor. Source: https://stackoverflow.com/questions/17461902/changing-background-color-of-listview-c-sharp-when-disabled
        Dim bm As New Bitmap(ListView1.ClientSize.Width, ListView1.ClientSize.Height)
        Graphics.FromImage(bm).Clear(ListView1.BackColor)
        ListView1.BackgroundImage = bm

        ' Declare path constant for Windows ADK
        Dim ADKPath As String = Path.Combine(If(Environment.Is64BitOperatingSystem,
                                                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                                                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)), "Windows Kits", "10",
                                                "Assessment and Deployment Kit")
        ' Check ADK status
        If Not Directory.Exists(ADKPath) Then
            DynaLog.LogMessage("ADK installation directory " & Quote & ADKPath & Quote & " is not found in this system. Either it has not been installed or it has been installed somewhere else.")
            If MsgBox(LocalizationService.ForSection("ISOFiles.Creator.Messages")("Windows.Message"), vbYesNo + vbQuestion, "") = MsgBoxResult.Yes Then
                Visible = True
                ADKDownloaderBW.RunWorkerAsync()
                Do Until Not ADKDownloaderBW.IsBusy
                    Application.DoEvents()
                    Thread.Sleep(100)
                Loop
                If Not adkDownloadSuccess Then
                    Process.Start(LocalizationService.ForSection("ISOCreator.Links")("Https.Learn.Message"))
                    Close()
                End If
            Else
                Close()
            End If

        End If

        ' Restore combobox architecture items
        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange(architectures)
        ' Remove architectures incompatible with the system ADK
        For Each architecture In architectures
            Dim WimPath As String = Path.Combine(ADKPath, "Windows Preinstallation Environment", architecture, "en-us", "winpe.wim")
            DynaLog.LogMessage("Testing if architecture " & architecture & " is supported by the ADK installed in this system...")
            If Not File.Exists(WimPath) Then
                DynaLog.LogMessage("- Windows PE WIM " & Quote & WimPath & Quote & " is not present. Removing architecture option...")
                ComboBox1.Items.Remove(architecture)
            End If
        Next
        ' If we are left with no architectures, add them back
        If ComboBox1.Items.Count = 0 Then
            DynaLog.LogMessage("For some reason we excluded all of them. This could be because of incorrect detections. Adding back...")
            ComboBox1.Items.AddRange(architectures)
        End If
        ComboBox1.SelectedIndex = 0

        ' Apply PE Helper settings
        DynaLog.LogMessage("Getting ISO creation settings...")
        DynaLog.LogMessage("- Unattended answer file (overrides existing answer files in an image): " & MainForm.PEHelper_UnattendedFile)
        DynaLog.LogMessage("- Copy to Ventoy? " & MainForm.PEHelper_CopyToVentoy)
        DynaLog.LogMessage("- Use new EFI boot binaries? " & MainForm.PEHelper_Use2023EFI)
        DynaLog.LogMessage("- Include System Drivers? " & MainForm.PEHelper_IncludeSysDrvs)
        DynaLog.LogMessage("- " & MainForm.PEHelper_MaxConcurrentISO & " ISO file(s) can be created at the same time")
        _jobManager = New IsoCreationJobManager(MainForm.PEHelper_MaxConcurrentISO)

        ' get build time to show on watermark
        Try
            Dim buildTime As String = BuildGetter.RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm")
            File.WriteAllText(Path.Combine(Application.StartupPath, "bin", "extps1", "PE_Helper", "version"), buildTime)
        Catch ex As Exception

        End Try

        If MainForm.PEHelper_UnattendedFile <> "" AndAlso File.Exists(MainForm.PEHelper_UnattendedFile) Then
            DynaLog.LogMessage("Unattended answer file has been specified and exists. Using it...")
            CheckBox1.Checked = True
            TextBox4.Text = MainForm.PEHelper_UnattendedFile
        Else
            DynaLog.LogMessage("Either no answer file was specified or it was specified, but doesn't exist...")
            CheckBox1.Checked = False
            TextBox4.Text = ""
        End If
        CheckBox2.Checked = MainForm.PEHelper_CopyToVentoy
        CheckBox3.Checked = MainForm.PEHelper_Use2023EFI
        CheckBox4.Checked = MainForm.PEHelper_IncludeSysDrvs

        AddHandler CheckBox3.CheckedChanged, AddressOf CheckBox3_CheckedChanged

        ColumnHeader1.Width = WindowHelper.ScaleLogical(29)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(265)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(343)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(103)
        ColumnHeader5.Width = WindowHelper.ScaleLogical(130)
        ColumnHeader6.Width = WindowHelper.ScaleLogical(64)
        ColumnHeader7.Width = WindowHelper.ScaleLogical(960)
        ColumnHeader8.Width = WindowHelper.ScaleLogical(128)

        ' Hook into job manager events
        AddHandler _jobManager.JobStatusChanged, AddressOf JobManager_JobStatusChanged
        AddHandler _jobManager.JobProgressChanged, AddressOf JobManager_JobProgressChanged
    End Sub

    Private Sub DownloadADK()
        Try
            ProgressReporter.SetMessage(LocalizationService.ForSection("ISOCreator")("Preparing.ADK.Download.Message"))
            ADKDownloaderBW.ReportProgress(0)
            Dim FileNames As New List(Of String)
            For Each downloadLocation In adkDownloadLocations
                FileNames.Add(Path.GetFileName(downloadLocation))
                Dim current As Integer = adkDownloadLocations.ToList().IndexOf(downloadLocation)
                Dim count As Integer = adkDownloadLocations.Count
                ProgressReporter.SetMessage(LocalizationService.ForSection("ISOCreator").Format("Downloading.ADK.Component.Message", current + 1, count))
                ADKDownloaderBW.ReportProgress(50 * (current / count))
                Using client As New WebClient()
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                    client.DownloadFile(downloadLocation, Path.Combine(Application.StartupPath, Path.GetFileName(downloadLocation)))
                End Using
            Next
            Dim currentProgress As Integer = 50
            For Each FileName In FileNames
                Dim current As Integer = FileNames.IndexOf(FileName)
                Dim count As Integer = FileNames.Count
                ProgressReporter.SetMessage(LocalizationService.ForSection("ISOCreator").Format("Installing.ADK.Component.Message", current + 1, count))
                ADKDownloaderBW.ReportProgress(currentProgress)
                Dim InstallerProcess As New Process()
                InstallerProcess.StartInfo.WorkingDirectory = Application.StartupPath
                If File.Exists(Path.Combine(Application.StartupPath, FileName)) Then
                    InstallerProcess.StartInfo.FileName = FileName
                    ' Guess command-line options. Source of necessary options comes from remediation script Microsoft published
                    ' during the CrowdStrike incident.
                    InstallerProcess.StartInfo.Arguments = String.Format("/features {0} /q /ceip off",
                                                                         If(FileName.Contains("winpe"),
                                                                            "OptionId.WindowsPreinstallationEnvironment",
                                                                            "OptionId.DeploymentTools")
                                                                        )
                    InstallerProcess.Start()
                    InstallerProcess.WaitForExit()
                    If Not InstallerProcess.ExitCode = 0 Then
                        Throw New Exception(LocalizationService.ForSection("ISOCreator").Format("ADK.Installer.ExitCode.Message", InstallerProcess.ExitCode))
                    End If
                End If
                currentProgress += 25
            Next
            Try
                ProgressReporter.SetMessage(LocalizationService.ForSection("ISOCreator")("Deleting.Temp.Files.Message"))
                ADKDownloaderBW.ReportProgress(100)
                For Each FileName In FileNames
                    File.Delete(Path.Combine(Application.StartupPath, FileName))
                Next
            Catch ex As Exception

            End Try
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        SaveFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        DynaLog.LogMessage("Source image file to test: " & Quote & OpenFileDialog1.FileName & Quote)
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Sub GetImageInfo(ImageFile As String)
        DynaLog.LogMessage("Image file to get information about: " & Quote & ImageFile & Quote)
        DynaLog.LogMessage("Checking if mounted image detector is busy...")
        ListView1.Items.Clear()
        MainForm.StopMountedImageDetector()
        Try
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            ImageInfoCollection = DismApi.GetImageInfo(ImageFile)
            DynaLog.LogMessage("Information collection count: " & ImageInfoCollection.Count)
            If ImageInfoCollection.Count > 0 Then
                DynaLog.LogMessage("This file has images. Updating lists...")
                ListView1.Items.AddRange(ImageInfoCollection.Select(Function(ImageInfo) New ListViewItem(New String() {(ImageInfoCollection.IndexOf(ImageInfo) + 1),
                                                                                                                       ImageInfo.ImageName,
                                                                                                                       ImageInfo.ImageDescription,
                                                                                                                       ImageInfo.ProductVersion.ToString(),
                                                                                                                       Casters.CastDismArchitecture(ImageInfo.Architecture)})).ToArray())
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
            Dim msg As String = LocalizationService.ForSection("ISOCreator.GetImageInfo").Format("Gather.ImageFile.Message", ex.ToString(), ex.Message, Hex(ex.HResult))
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
        Finally
            DynaLog.LogMessage("Shutting down API...")
            Try
                DismApi.Shutdown()
            Catch ex As Exception
                ' Don't do anything
            End Try
        End Try
        DynaLog.LogMessage("This process has finished.")
        MainForm.StartMountedImageDetector()
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        DynaLog.LogMessage("Specified destination: " & Quote & SaveFileDialog1.FileName & Quote)
        TextBox3.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Checking provided information...")
        DynaLog.LogMessage("- Source image to add to ISO file: " & Quote & TextBox1.Text & Quote)
        DynaLog.LogMessage("- Destination ISO file: " & Quote & TextBox3.Text & Quote)
        If TextBox1.Text = "" OrElse Not File.Exists(TextBox1.Text) Then
            ISOMsg = LocalizationService.ForSection("ISOCreator.Validation")("Either.Source.Message")
            MsgBox(ISOMsg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        If TextBox3.Text = "" Then
            If SaveFileDialog1.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then
                ISOMsg = LocalizationService.ForSection("ISOCreator.Validation")("TargetISO.Required.Message")
                MsgBox(ISOMsg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Exit Sub
            End If
        End If
        ISOMsg = LocalizationService.ForSection("ISOCreator.Validation")("Saved.Message")
        If MsgBox(ISOMsg, vbYesNo + vbQuestion, ImageTaskHeader1.ItemText) = MsgBoxResult.No Then
            Exit Sub
        End If

        ' If we have already started a job targeting the specified destination, we cancel
        If _jobManager.JobQueue.Any(Function(job) job.Value.DestinationIsoFile.Equals(TextBox3.Text, StringComparison.OrdinalIgnoreCase)) Or
            _jobManager.ActiveTasks.Any(Function(job) job.Value.DestinationIsoFile.Equals(TextBox3.Text, StringComparison.OrdinalIgnoreCase)) Then
            MessageBox.Show(LocalizationService.ForSection("ISOCreator.Validation")("Duplicate.Job.Message"), ImageTaskHeader1.ItemText, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If File.Exists(TextBox3.Text) Then
            ISOMsg = LocalizationService.ForSection("ISOCreator.Validation")("Target.ISO.Message")
            If MsgBox(ISOMsg, vbYesNo + vbQuestion, ImageTaskHeader1.ItemText) = MsgBoxResult.Yes Then
                Try
                    File.Delete(TextBox3.Text)
                Catch ex As Exception
                    ' Could not delete ISO
                End Try
            Else
                Exit Sub
            End If
        End If
        Cancel_Button.Enabled = False

        ' Queue the ISO creation job in the job manager
        Dim architecture As IsoArchitecture = GetArchitectureFromString(ComboBox1.SelectedItem.ToString())
        Dim unattFile As String = If(CheckBox1.Checked, TextBox4.Text, "")

        _currentJobId = _jobManager.QueueJob(TextBox1.Text, TextBox3.Text, architecture, unattFile, CheckBox2.Checked, CheckBox3.Checked, CheckBox4.Checked)

        DynaLog.LogMessage("ISO creation job queued with ID: " & _currentJobId)
    End Sub
    Private Function GetArchitectureFromString(architectureString As String) As IsoArchitecture
        Select Case architectureString.ToLower()
            Case "x86" : Return IsoArchitecture.X86
            Case "amd64" : Return IsoArchitecture.AMD64
            Case "arm64" : Return IsoArchitecture.ARM64
            Case Else : Return IsoArchitecture.X86
        End Select
    End Function

    Private Sub ISOCreator_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If _jobManager.GetActiveTaskCount() > 0 Then
            DynaLog.LogMessage("The PE Helper is busy. Cancelling exit...")
            e.Cancel = True
            Beep()
            Exit Sub
        End If
        DynaLog.LogMessage("Saving settings...")
        If CheckBox1.Checked Then
            MainForm.PEHelper_UnattendedFile = TextBox4.Text
        Else
            MainForm.PEHelper_UnattendedFile = ""
        End If
        MainForm.PEHelper_CopyToVentoy = CheckBox2.Checked
        MainForm.PEHelper_Use2023EFI = CheckBox3.Checked
        MainForm.PEHelper_IncludeSysDrvs = CheckBox4.Checked

        Dim customPolicyPath As String = Path.Combine(Application.StartupPath, "bin", "extps1", "PE_Helper", "files", "CustomPolicy.reg")
        If File.Exists(customPolicyPath) Then
            Try
                File.Delete(customPolicyPath)
            Catch ex As Exception

            End Try
        End If

        RemoveHandler CheckBox3.CheckedChanged, AddressOf CheckBox3_CheckedChanged
        RemoveHandler _jobManager.JobStatusChanged, AddressOf JobManager_JobStatusChanged
        RemoveHandler _jobManager.JobProgressChanged, AddressOf JobManager_JobProgressChanged
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim selectedImage As WindowsImage = PopupMountedImagePicker.PickImage(".wim")
        If selectedImage IsNot Nothing Then
            DynaLog.LogMessage("Selected image: " & selectedImage.ImageFile)
            TextBox1.Text = selectedImage.ImageFile
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("The specified file exists. Getting information...")
            GetImageInfo(TextBox1.Text)
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start(LocalizationService.ForSection("ISOCreator.Links")("Https.Learn.Message"))
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        TextBox1.Text = MainForm.SourceImg
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Close()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Panel2.Enabled = CheckBox1.Checked
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        OpenFileDialog2.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog2_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog2.FileOk
        DynaLog.LogMessage("Unattended answer file to test: " & Quote & OpenFileDialog2.FileName & Quote)
        TextBox4.Text = OpenFileDialog2.FileName
    End Sub

    Private Sub ISOCreator_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If Visible And WindowState <> FormWindowState.Minimized Then
            ' Set disabled ListView's backcolor. Source: https://stackoverflow.com/questions/17461902/changing-background-color-of-listview-c-sharp-when-disabled
            Dim bm As New Bitmap(ListView1.ClientSize.Width, ListView1.ClientSize.Height)
            Graphics.FromImage(bm).Clear(ListView1.BackColor)
            ListView1.BackgroundImage = bm
        End If
        If _jobManager.GetActiveTaskCount() > 0 Then
            WindowHelper.DisableCloseCapability(Handle)
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs)
        Dim uefiSection = LocalizationService.ForSection("ISOCreator")
        Dim uefiCA2023_Message As String = uefiSection("Create.ISO.Message") &
            uefiSection("Computers.UEFI.Message") & CrLf & CrLf &
            uefiSection("Run.Power.Shell.Message") & CrLf & CrLf &
            uefiSection("Doubts.Recommend.Message")
        Dim uefiCA2023_Title As String = uefiSection("Windows.Title")
        Dim uefiCA2023_NotSupportedOnCurrentSystemMessage As String = uefiSection("Have.Detected.Message")
        If CheckBox3.Checked Then
            MsgBox(uefiCA2023_Message, vbOKOnly + vbInformation, uefiCA2023_Title)

            ' Detect if we support UEFI CA 2023 binaries on the current system, just to have an idea (on the current system, at least)
            ' https://techcommunity.microsoft.com/blog/Windows-ITPro-blog/secure-boot-playbook-for-certificates-expiring-in-2026/4469235
            Try
                ' we don't REALLY need to check on BIOS systems
                If Not Environment.GetEnvironmentVariable("FIRMWARE_TYPE").Equals("UEFI") Then Exit Try

                ' Before checking the system for CA 2023 certs, we'll check if Secure Boot is enabled.
                DynaLog.LogMessage("Detecting current Secure Boot status...")

                Dim sbStateRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\SecureBoot\State", False)
                Dim sbState As Integer = sbStateRk.GetValue("UEFISecureBootEnabled")
                sbStateRk.Close()

                DynaLog.LogMessage("Secure Boot Status: " & sbState)

                ' If we have 0 then we know secure boot is disabled on the system.
                If sbState = 0 Then Exit Try

                DynaLog.LogMessage("Detecting if current system is compatible with UEFI CA 2023...")

                Dim sbUefiBinStatusRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\SecureBoot\Servicing", False)
                Dim sbUefiBinStatus As String = sbUefiBinStatusRk.GetValue("UEFICA2023Status", "")
                sbUefiBinStatusRk.Close()

                DynaLog.LogMessage("UEFI CA 2023 Status: " & sbUefiBinStatus)

                ' If the status value is "Updated", it means that the system has already applied Secure Boot DBX updates
                ' to enable support for UEFI CA 2023 binaries. If it is "NotStarted" or something else though, then
                ' the system hasn't initiated any DBX updates.
                If Not sbUefiBinStatus.Equals("updated", StringComparison.InvariantCultureIgnoreCase) Then
                    DynaLog.LogMessage("UEFI CA 2023 Status is not Updated. We are not running with UEFI CA 2023-supported SecureBoot")

                    MsgBox(uefiCA2023_NotSupportedOnCurrentSystemMessage, vbOKOnly + vbExclamation, uefiCA2023_Title)
                End If

            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub UpdateJobCountDisplay()
        ' Update the display to show active and queued job counts
        Dim activeCount = _jobManager.GetActiveTaskCount()
        Dim queuedCount = _jobManager.GetQueuedTaskCount()

        If activeCount > 0 OrElse queuedCount > 0 Then
            Label8.Text = LocalizationService.ForSection("ISOCreator").Format("Jobs.Status.Message", activeCount, queuedCount)
            DynaLog.LogMessage(String.Format("Job status: {0} active, {1} queued", activeCount, queuedCount))

            ' show in lists
            Dim CurrentJobQueue As List(Of KeyValuePair(Of Integer, IsoCreationTask)) = _jobManager.JobQueue,
                ActiveJobQueue As List(Of KeyValuePair(Of Integer, IsoCreationTask)) = _jobManager.ActiveTasks

            CreationJobsLV.Items.Clear()
            CreationJobsLV.Items.AddRange(CurrentJobQueue.Select(Function(job) New ListViewItem(New String() {job.Key, job.Value.DestinationIsoFile, LocalizationService.ForSection("ISOCreator")("Queued.Status")})).ToArray())
            CreationJobsLV.Items.AddRange(ActiveJobQueue.Select(Function(job) New ListViewItem(New String() {job.Key, job.Value.DestinationIsoFile, LocalizationService.ForSection("ISOCreator")("Running.Status")})).ToArray())
        Else
            IdlePanel.Visible = True
            ISOProgressPanel.Visible = False

            CreationJobsLV.Items.Clear()
        End If
    End Sub

    Private Sub JobManager_JobStatusChanged(jobId As Integer, status As JobStatus)
        ' This event is fired when a job status changes
        DynaLog.LogMessage("Job " & jobId & " status changed to: " & status.ToString())

        ' Update job count display
        UpdateJobCountDisplay()

        Select Case status
            Case JobStatus.Queued
                DynaLog.LogMessage("ISO creation task is queued")
            Case JobStatus.Running
                DynaLog.LogMessage("ISO creation task is now running")
                IdlePanel.Visible = False
                ISOProgressPanel.Visible = True
                WindowHelper.DisableCloseCapability(Handle)

                ' Disable close if any tasks are running
                If _jobManager.GetActiveTaskCount() > 0 Then
                    WindowHelper.DisableCloseCapability(Handle)
                End If
            Case JobStatus.Completed
                DynaLog.LogMessage("ISO creation task completed successfully")
                success = True

                ' Get ISO path from job metadata
                Dim metadata = _jobManager.GetJobMetadata(jobId)
                ShowJobCompletionMessage(True, If(metadata IsNot Nothing, metadata.DestinationIsoFile, ""))

                ' Enable close only if no more tasks are running
                If _jobManager.GetActiveTaskCount() = 0 Then
                    WindowHelper.EnableCloseCapability(Handle)
                End If
            Case JobStatus.Failed
                DynaLog.LogMessage("ISO creation task failed")
                success = False

                ' Get ISO path from job metadata
                Dim metadata = _jobManager.GetJobMetadata(jobId)
                ShowJobCompletionMessage(False, If(metadata IsNot Nothing, metadata.DestinationIsoFile, ""))

                ' Enable close only if no more tasks are running
                If _jobManager.GetActiveTaskCount() = 0 Then
                    WindowHelper.EnableCloseCapability(Handle)
                End If
        End Select
    End Sub

    Private Sub JobManager_JobProgressChanged(jobId As Integer, isRunning As Boolean)
        If Not isRunning Then
            ' Job has finished, re-enable buttons only if no other jobs are running
            If _jobManager.GetActiveTaskCount() = 0 Then
                OK_Button.Enabled = True
                Cancel_Button.Enabled = True
                GroupBox1.Enabled = True
                IdlePanel.Visible = True
                ISOProgressPanel.Visible = False

                ' Delete ISOTASKS
                Try
                    Directory.Delete(String.Format("{0}\ISOTASKS", Environment.GetEnvironmentVariable("SYSTEMDRIVE")), True)
                Catch ex As Exception

                End Try
            End If
        End If
    End Sub

    Private Sub ShowJobCompletionMessage(isSuccess As Boolean, isoFilePath As String)
        Dim msg As String = If(isSuccess,
                               LocalizationService.ForSection("ISOCreator.Background").Format("Isofile.Created.Named.Message", Path.GetFileName(isoFilePath)),
                               LocalizationService.ForSection("ISOCreator.Background")("Failed.Create.Message"))
        WindowHelper.DisplayNotificationBalloon(If(isSuccess, ToolTipIcon.Info, ToolTipIcon.Warning), ImageTaskHeader1.ItemText, msg)
    End Sub
    Private Sub ADKDownloaderBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ADKDownloaderBW.DoWork
        DownloadADK()
    End Sub

    Private Sub ADKDownloaderBW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles ADKDownloaderBW.ProgressChanged
        ProgressReporter.ReportProgress(Me, e.ProgressPercentage)
    End Sub

    Private Sub ADKDownloaderBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ADKDownloaderBW.RunWorkerCompleted
        ProgressReporter.Hide()
        adkDownloadSuccess = e.Error Is Nothing
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        PECustomizerDialog.ShowDialog(Me)
    End Sub

    Private Sub CheckBox4_MouseHover(sender As Object, e As EventArgs) Handles CheckBox4.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("ISOCreator")("Check.Option.Storage.Message"))
    End Sub

    Private Sub CheckBox3_MouseHover(sender As Object, e As EventArgs) Handles CheckBox3.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("ISOCreator")("AvailableADK.Message"))
    End Sub
End Class
