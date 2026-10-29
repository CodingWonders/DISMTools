Imports Microsoft.Dism
Imports WimgApi.Interop

Public Class WDSInstallImageCopy

    Dim ImageInfoCollection As DismImageInfoCollection
    Dim SelectedIndexes As Integer()
    Dim UploadAllImages As Boolean
    Dim success As Boolean

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Checking provided information...")
        DynaLog.LogMessage("- Source image to add to WDS: " & Quote & tbSourceImageFile.Text & Quote)
        DynaLog.LogMessage("- Group to add image to: " & Quote & tbDestinationWdsImageGroup.Text & Quote)
        If tbSourceImageFile.Text = "" OrElse Not File.Exists(tbSourceImageFile.Text) Then
            MsgBox(LocalizationService.ForSection("WDSImageCopy.Messages")("Either.Source.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        If tbDestinationWdsImageGroup.Text = "" Then
            MsgBox(LocalizationService.ForSection("WDSImageCopy.Messages")("Group.Has.None.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        If lvImages.CheckedItems.Count < 1 Then
            MsgBox(LocalizationService.ForSection("WDSImageCopy.Messages")("Images.Have.None.Label"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        If MsgBox(LocalizationService.ForSection("WDSImageCopy.Messages")("Image.Add.Message"), vbYesNo + vbQuestion, ImageTaskHeader1.ItemText) = MsgBoxResult.No Then
            Exit Sub
        End If
        OK_Button.Enabled = False
        Cancel_Button.Enabled = False
        OptionsPanel.Enabled = False

        UploadAllImages = lvImages.CheckedItems.Count = lvImages.Items.Count
        SelectedIndexes = lvImages.CheckedIndices.Cast(Of Integer)().ToArray()

        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Close()
    End Sub

    Private Sub WDSInstallImageCopy_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        OK_Button.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Cancel.Button")
        Button2.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Pick.Button")
        Button1.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Browse.Button")
        Label4.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("ImageFile.Server.Label")
        Button3.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Mounted.Image.Button")
        Label1.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Images.Added.Group.Label")
        ColumnHeader1.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Value.Column")
        ColumnHeader2.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("ImageName.Column")
        ColumnHeader3.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("ImageDescription.Column")
        ColumnHeader4.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("ImageVersion.Column")
        ColumnHeader5.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Image.Architecture.Column")
        Button4.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Pick.Server.Groups.Button")
        Button5.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("SelectAll.Button")
        Button6.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("ClearSelection.Button")
        GroupBox1.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Progress.Group")
        Label3.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Re.Ready.OK.Label")
        Label8.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Status.Label")
        OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.WDSImageCopy")("WIM.Files.Filter")
        Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Image.Win.Deploy.Label")
        ImageTaskHeader1.ItemText = Text

        If WindowsServiceHelper.GetOnlineSystemServiceInformationByName("WDSServer") Is Nothing Then
            ' We are either not running this on Windows Server, or we are, but without the WDS role
            MsgBox(LocalizationService.ForSection("WDSImageCopy.Messages")("Wizard.Support.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Close()
            Exit Sub
        End If
        If WindowsServiceHelper.GetOnlineServiceStartStatus("WDSServer") <> ServiceProcess.ServiceControllerStatus.Running Then
            DynaLog.LogMessage("WDS Server Service not running. Starting...")
            WindowsServiceHelper.StartOnlineService("WDSServer")
        End If

        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        tbSourceImageFile.BackColor = CurrentTheme.SectionBackgroundColor
        tbDestinationWdsImageGroup.BackColor = CurrentTheme.SectionBackgroundColor
        lvImages.BackColor = CurrentTheme.SectionBackgroundColor
        tbSourceImageFile.ForeColor = ForeColor
        tbDestinationWdsImageGroup.ForeColor = ForeColor
        lvImages.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        If MainForm.SourceImg = "N/A" Or Not File.Exists(MainForm.SourceImg) Or MainForm.OnlineManagement Or MainForm.OfflineManagement Then
            Button3.Enabled = False
        Else
            Button3.Enabled = True
        End If
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ' Set disabled ListView's backcolor. Source: https://stackoverflow.com/questions/17461902/changing-background-color-of-listview-c-sharp-when-disabled
        Dim bm As New Bitmap(lvImages.ClientSize.Width, lvImages.ClientSize.Height)
        Graphics.FromImage(bm).Clear(lvImages.BackColor)
        lvImages.BackgroundImage = bm

        ColumnHeader1.Width = WindowHelper.ScaleLogical(44)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(265)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(343)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(103)
        ColumnHeader5.Width = WindowHelper.ScaleLogical(130)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim selectedImage As WindowsImage = PopupMountedImagePicker.PickImage(".wim")
        If selectedImage IsNot Nothing Then
            DynaLog.LogMessage("Selected image: " & selectedImage.ImageFile)
            tbSourceImageFile.Text = selectedImage.ImageFile
        End If
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        tbSourceImageFile.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub GetImageInfoWithWimgApi(ImageFile As String)
        DynaLog.LogMessage("Image file to get information about: " & Quote & ImageFile & Quote)
        DynaLog.LogMessage("---> Using WIMGAPI...")
        lvImages.Items.Clear()
        Try
            If File.Exists(ImageFile) Then
                Dim imageInfo As WimFileInfo = WimReader.GetWimInfo(ImageFile)
                lvImages.Items.AddRange(imageInfo.Images.Select(Function(imgInfo) New ListViewItem(New String() {(imageInfo.Images.IndexOf(imgInfo) + 1),
                                                                                                                  imgInfo.Name,
                                                                                                                  imgInfo.Description,
                                                                                                                  imgInfo.Version,
                                                                                                                  Casters.CastDismArchitecture(imgInfo.Architecture)})).ToArray())
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Exception occurred: " & ex.Message)
            GetImageInfoWithDismApi(ImageFile)
        End Try
    End Sub

    Private Sub GetImageInfoWithDismApi(ImageFile As String)
        DynaLog.LogMessage("WIMGAPI method failed.")
        DynaLog.LogMessage("---> Using DISM API...")
        DynaLog.LogMessage("Checking if mounted image detector is busy...")
        MainForm.StopMountedImageDetector()
        Try
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            ImageInfoCollection = DismApi.GetImageInfo(ImageFile)
            DynaLog.LogMessage("Information collection count: " & ImageInfoCollection.Count)
            If ImageInfoCollection.Count > 0 Then
                DynaLog.LogMessage("This file has images. Updating lists...")
                lvImages.Items.AddRange(ImageInfoCollection.Select(Function(ImageInfo) New ListViewItem(New String() {(ImageInfoCollection.IndexOf(ImageInfo) + 1),
                                                                                                                       ImageInfo.ImageName,
                                                                                                                       ImageInfo.ImageDescription,
                                                                                                                       ImageInfo.ProductVersion.ToString(),
                                                                                                                       Casters.CastDismArchitecture(ImageInfo.Architecture)})).ToArray())

                ' By default check them all
                WindowHelper.CheckAllItems(lvImages)

                ' Block all boot images (or at least warn)
                If ImageInfoCollection.Any(Function(ImageInfo) ImageInfo.EditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase)) Then
                    MsgBox(LocalizationService.ForSection("WDSImageCopy.Messages")("Boot.Image.Detected.Message"), vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
                End If
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
            Dim msg As String = ""
            msg = LocalizationService.ForSection("WDSImageCopy.ImageInfo").Format("Gather.ImageFile.Message", ex.ToString(), ex.Message, Hex(ex.HResult))
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

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles tbSourceImageFile.TextChanged
        If tbSourceImageFile.Text <> "" And File.Exists(tbSourceImageFile.Text) Then
            DynaLog.LogMessage("The specified file exists. Getting information...")
            GetImageInfoWithWimgApi(tbSourceImageFile.Text)
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        WindowHelper.CheckAllItems(lvImages)
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        WindowHelper.UncheckAllItems(lvImages)
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        BackgroundWorker1.ReportProgress(0)
        DynaLog.LogMessage("Starting WDS image addition...")
        Dim WDSUploader As New Process()
        WDSUploader.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
        WDSUploader.StartInfo.WorkingDirectory = Application.StartupPath & "\bin\extps1\PE_Helper\files"
        If UploadAllImages Then
            WDSUploader.StartInfo.Arguments = "-noprofile -nologo -executionpolicy unrestricted -file " & Quote & Application.StartupPath & "\bin\extps1\PE_Helper\files\install_image_to_wds.ps1" & Quote &
                " -imageGroup " & Quote & tbDestinationWdsImageGroup.Text & Quote & " -installImagePath " & Quote & tbSourceImageFile.Text & Quote
            WDSUploader.Start()
            WDSUploader.WaitForExit()
            DynaLog.LogMessage("The process finished with exit code " & Hex(WDSUploader.ExitCode))
            success = (WDSUploader.ExitCode = 0)
        Else
            Dim successfulUploads As Integer = 0,
                failedUploads As Integer = 0

            For i = 0 To SelectedIndexes.Count - 1
                WDSUploader.StartInfo.Arguments = "-noprofile -nologo -executionpolicy unrestricted -file " & Quote & Application.StartupPath & "\bin\extps1\PE_Helper\files\install_image_to_wds.ps1" & Quote &
                    " -imageGroup " & Quote & tbDestinationWdsImageGroup.Text & Quote & " -installImagePath " & Quote & tbSourceImageFile.Text & Quote & " -installImageIndex " & SelectedIndexes(i) + 1
                WDSUploader.Start()
                WDSUploader.WaitForExit()
                DynaLog.LogMessage("The process finished with exit code " & Hex(WDSUploader.ExitCode))

                If WDSUploader.ExitCode = 0 Then
                    successfulUploads += 1
                Else
                    failedUploads += 1
                End If
            Next
            success = (successfulUploads >= failedUploads)
        End If
        BackgroundWorker1.ReportProgress(100)
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        IdlePanel.Visible = False
        ISOProgressPanel.Visible = True
        If e.ProgressPercentage < 100 Then
            WindowHelper.DisableCloseCapability(Handle)
            Label8.Text = LocalizationService.ForSection("WDSImageCopy.Progress")("UploadingImages.Label")
            ProgressBar1.Style = ProgressBarStyle.Marquee
            TaskbarHelper.SetIndicatorState(0, Windows.Shell.TaskbarItemProgressState.Indeterminate, MainForm.Handle)
        Else
            WindowHelper.EnableCloseCapability(Handle)
            If success Then Label8.Text = LocalizationService.ForSection("WDSImageCopy.Progress")("Image.Uploads.Done.Label")
            ProgressBar1.Style = ProgressBarStyle.Blocks
            TaskbarHelper.SetIndicatorState(0, Windows.Shell.TaskbarItemProgressState.None, MainForm.Handle)
        End If
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        DynaLog.LogMessage("The process has finished.")
        DynaLog.LogMessage("- Did it succeed? " & If(success, "Yes", "No"))
        MsgBox(If(success, LocalizationService.ForSection("WDSImageCopy.Messages")("UploadSuccessful.Label"), LocalizationService.ForSection("WDSImageCopy.Messages")("Images.Uploaded.Done.Label")),
               vbOKOnly + vbInformation, ImageTaskHeader1.ItemText)
        OK_Button.Enabled = True
        Cancel_Button.Enabled = True
        OptionsPanel.Enabled = True
        IdlePanel.Visible = True
        ISOProgressPanel.Visible = False
    End Sub

    Private Sub WDSInstallImageCopy_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If BackgroundWorker1.IsBusy Then
            DynaLog.LogMessage("The process is busy. Cancelling exit...")
            e.Cancel = True
            Beep()
            Exit Sub
        End If
    End Sub

    Private Sub WDSInstallImageCopy_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If Visible And WindowState <> FormWindowState.Minimized Then
            ' Set disabled ListView's backcolor. Source: https://stackoverflow.com/questions/17461902/changing-background-color-of-listview-c-sharp-when-disabled
            Dim bm As New Bitmap(lvImages.ClientSize.Width, lvImages.ClientSize.Height)
            Graphics.FromImage(bm).Clear(lvImages.BackColor)
            lvImages.BackgroundImage = bm
        End If
        If BackgroundWorker1.IsBusy Then
            WindowHelper.DisableCloseCapability(Handle)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        tbSourceImageFile.Text = MainForm.SourceImg
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If WDSImageGroupSpecifier.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            tbDestinationWdsImageGroup.Text = WDSImageGroupSpecifier.SpecifiedImageGroup
        End If
    End Sub
End Class
