Imports Microsoft.Dism

Public Class WDSInstallImageCopy

    Dim ImageInfoCollection As DismImageInfoCollection
    Dim SelectedIndexes As Integer()
    Dim UploadAllImages As Boolean
    Dim success As Boolean

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Checking provided information...")
        DynaLog.LogMessage("- Source image to add to WDS: " & Quote & TextBox1.Text & Quote)
        DynaLog.LogMessage("- Group to add image to: " & Quote & TextBox2.Text & Quote)
        If TextBox1.Text = "" OrElse Not File.Exists(TextBox1.Text) Then
            MsgBox("Either the source image file does not exist or you haven't provided any image file. Please specify a valid image file and try again.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        If TextBox2.Text = "" Then
            MsgBox("No group has been specified. Please specify a new or an existing WDS group and try again.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        If ListView1.CheckedItems.Count < 1 Then
            MsgBox("No images have been selected for addition to your WDS server.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        If MsgBox("Make sure that the image you are adding has been prepared with Sysprep." & CrLf & CrLf &
                  "If you have not done so, click No, prepare the image, and start the process again. You don't have to close this window." & CrLf & CrLf &
                  "Do you want to add this image to your WDS server?", vbYesNo + vbQuestion, ImageTaskHeader1.ItemText) = MsgBoxResult.No Then
            Exit Sub
        End If
        OK_Button.Enabled = False
        Cancel_Button.Enabled = False
        OptionsPanel.Enabled = False

        UploadAllImages = ListView1.CheckedItems.Count = ListView1.Items.Count
        SelectedIndexes = ListView1.CheckedIndices.Cast(Of Integer)().ToArray()

        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Close()
    End Sub

    Private Sub WDSInstallImageCopy_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If WindowsServiceHelper.GetOnlineSystemServiceInformationByName("WDSServer") Is Nothing Then
            ' We are either not running this on Windows Server, or we are, but without the WDS role
            MsgBox("This wizard does not support this computer. Make sure that this computer is running Windows Server and that it has the Windows Deployment Services role installed.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
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
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        If MainForm.SourceImg = "N/A" Or Not File.Exists(MainForm.SourceImg) Or MainForm.OnlineManagement Or MainForm.OfflineManagement Then
            Button3.Enabled = False
        Else
            Button3.Enabled = True
        End If
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)

        ' Set disabled ListView's backcolor. Source: https://stackoverflow.com/questions/17461902/changing-background-color-of-listview-c-sharp-when-disabled
        Dim bm As New Bitmap(ListView1.ClientSize.Width, ListView1.ClientSize.Height)
        Graphics.FromImage(bm).Clear(ListView1.BackColor)
        ListView1.BackgroundImage = bm

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
            TextBox1.Text = selectedImage.ImageFile
        End If
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
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

                ' By default check them all
                WindowHelper.CheckAllItems(ListView1)

                ' Block all boot images (or at least warn)
                If ImageInfoCollection.Any(Function(ImageInfo) ImageInfo.EditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase)) Then
                    MsgBox("A boot image has been detected. You should not use these as installation images in your server.", vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
                End If
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
            Dim msg As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "Could not gather information of this image file. Reason:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case "ESN"
                            msg = "No pudimos obtener información de este archivo de imagen. Razón:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case "FRA"
                            msg = "Impossible de recueillir des informations sur ce fichier de l'image. Raison :" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case "PTB", "PTG"
                            msg = "Não foi possível recolher informações sobre este ficheiro de imagem. Motivo:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case "ITA"
                            msg = "Impossibile raccogliere informazioni sull'immagine. Motivo:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                    End Select
                Case 1
                    msg = "Could not gather information of this image file. Reason:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 2
                    msg = "No pudimos obtener información de este archivo de imagen. Razón:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 3
                    msg = "Impossible de recueillir des informations sur ce fichier de l'image. Raison :" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 4
                    msg = "Não foi possível recolher informações sobre este ficheiro de imagem. Motivo:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 5
                    msg = "Impossibile raccogliere informazioni sull'immagine. Motivo:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
            End Select
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

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("The specified file exists. Getting information...")
            GetImageInfo(TextBox1.Text)
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        WindowHelper.CheckAllItems(ListView1)
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        WindowHelper.UncheckAllItems(ListView1)
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        BackgroundWorker1.ReportProgress(0)
        DynaLog.LogMessage("Starting WDS image addition...")
        Dim WDSUploader As New Process()
        WDSUploader.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
        WDSUploader.StartInfo.WorkingDirectory = Application.StartupPath & "\bin\extps1\PE_Helper\files"
        If UploadAllImages Then
            WDSUploader.StartInfo.Arguments = "-noprofile -nologo -executionpolicy unrestricted -file " & Quote & Application.StartupPath & "\bin\extps1\PE_Helper\files\install_image_to_wds.ps1" & Quote &
                " -imageGroup " & Quote & TextBox2.Text & Quote & " -installImagePath " & Quote & TextBox1.Text & Quote
            WDSUploader.Start()
            WDSUploader.WaitForExit()
            DynaLog.LogMessage("The process finished with exit code " & Hex(WDSUploader.ExitCode))
            success = (WDSUploader.ExitCode = 0)
        Else
            Dim successfulUploads As Integer = 0,
                failedUploads As Integer = 0

            For i = 0 To SelectedIndexes.Count - 1
                WDSUploader.StartInfo.Arguments = "-noprofile -nologo -executionpolicy unrestricted -file " & Quote & Application.StartupPath & "\bin\extps1\PE_Helper\files\install_image_to_wds.ps1" & Quote &
                    " -imageGroup " & Quote & TextBox2.Text & Quote & " -installImagePath " & Quote & TextBox1.Text & Quote & " -installImageIndex " & SelectedIndexes(i) + 1
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
            Label8.Text = "Uploading images..."
            ProgressBar1.Style = ProgressBarStyle.Marquee
            TaskbarHelper.SetIndicatorState(0, Windows.Shell.TaskbarItemProgressState.Indeterminate, MainForm.Handle)
        Else
            WindowHelper.EnableCloseCapability(Handle)
            If success Then Label8.Text = "Image uploads done."
            ProgressBar1.Style = ProgressBarStyle.Blocks
            TaskbarHelper.SetIndicatorState(0, Windows.Shell.TaskbarItemProgressState.None, MainForm.Handle)
        End If
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        DynaLog.LogMessage("The process has finished.")
        DynaLog.LogMessage("- Did it succeed? " & If(success, "Yes", "No"))
        MsgBox(If(success, "The images were uploaded successfully.", "The images were not uploaded successfully."),
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
            Dim bm As New Bitmap(ListView1.ClientSize.Width, ListView1.ClientSize.Height)
            Graphics.FromImage(bm).Clear(ListView1.BackColor)
            ListView1.BackgroundImage = bm
        End If
        If BackgroundWorker1.IsBusy Then
            WindowHelper.DisableCloseCapability(Handle)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        TextBox1.Text = MainForm.SourceImg
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If WDSImageGroupSpecifier.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            TextBox2.Text = WDSImageGroupSpecifier.SpecifiedImageGroup
        End If
    End Sub
End Class