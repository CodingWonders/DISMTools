Imports System.Windows.Forms
Imports Microsoft.Dism
Imports System.IO
Imports System.Threading

Public Class ImgWim2Esd

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.imgSrcFile = TextBox1.Text
        ProgressPanel.imgConversionIndex = NumericUpDown1.Value
        ProgressPanel.imgDestFile = TextBox2.Text
        If ComboBox1.SelectedIndex = 0 Then
            ProgressPanel.imgConversionMode = 1
        ElseIf ComboBox1.SelectedIndex = 1 Then
            ProgressPanel.imgConversionMode = 0
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 991
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        TextBox2.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
        DynaLog.LogMessage("Source image file to convert: " & OpenFileDialog1.FileName)
        If OpenFileDialog1.FileName.EndsWith("wim", StringComparison.OrdinalIgnoreCase) Then
            ComboBox1.SelectedIndex = 1
        ElseIf OpenFileDialog1.FileName.EndsWith("esd", StringComparison.OrdinalIgnoreCase) Then
            ComboBox1.SelectedIndex = 0
        End If
    End Sub

    Private Sub ImgWim2Esd_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("Img.WIM")("ConvertImage.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("Img.WIM").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("Img.WIM")("SourceImageFile.Label")
        Label3.Text = LocalizationService.ForSection("Img.WIM")("Format.Converted.Image.Label")
        Label5.Text = LocalizationService.ForSection("Img.WIM")("Destination.ImageFile.Label")
        Label7.Text = LocalizationService.ForSection("Img.WIM")("Index.Label")
        LinkLabel1.Text = LocalizationService.ForSection("Img.WIM")("Format.Ichoose.Link")
        Button1.Text = LocalizationService.ForSection("Img.WIM")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("Img.WIM")("Browse.Button")
        OK_Button.Text = LocalizationService.ForSection("Img.WIM")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("Img.WIM")("Cancel.Button")
        ListView1.Columns(0).Text = LocalizationService.ForSection("Img.WIM")("Index.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("Img.WIM")("ImageName.Column")
        ListView1.Columns(2).Text = LocalizationService.ForSection("Img.WIM")("ImageDescription.Column")
        ListView1.Columns(3).Text = LocalizationService.ForSection("Img.WIM")("ImageVersion.Column")
        GroupBox1.Text = LocalizationService.ForSection("Img.WIM")("Source.Group")
        GroupBox2.Text = LocalizationService.ForSection("Img.WIM")("Options.Group")
        GroupBox3.Text = LocalizationService.ForSection("Img.WIM")("Destination.Group")
        OpenFileDialog1.Title = LocalizationService.ForSection("Img.WIM")("Source.ImageFile.Title")
        SaveFileDialog1.Title = LocalizationService.ForSection("Img.WIM")("Target.Image.Stored.Title")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        GroupBox3.ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        NumericUpDown1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ColumnHeader1.Width = WindowHelper.ScaleLogical(44)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(256)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(256)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(128)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SaveFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        SaveFileDialog1.Filter = LocalizationService.ForSection("Panels.ImageOps.WimToEsd")("Files.Filter")
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("Getting image file information...")
            MainForm.StopMountedImageDetector()
            Try
                ListView1.Items.Clear()
                DynaLog.LogMessage("Initializing API...")
                DismApi.Initialize(DismLogLevel.LogErrors)
                Dim imgInfoCollection As DismImageInfoCollection = DismApi.GetImageInfo(TextBox1.Text)
                NumericUpDown1.Maximum = imgInfoCollection.Count
                DynaLog.LogMessage("Information collection count: " & imgInfoCollection.Count)
                For Each imgInfo As DismImageInfo In imgInfoCollection
                    ListView1.Items.Add(New ListViewItem(New String() {imgInfo.ImageIndex, imgInfo.ImageName, imgInfo.ImageDescription, imgInfo.ProductVersion.ToString()}))
                Next
            Catch ex As Exception
                MsgBox(LocalizationService.ForSection("ImageConversion.WimToEsd")("Get.Index.Image.Label"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Finally
                DynaLog.LogMessage("Shutting down API...")
                Try
                    DismApi.Shutdown()
                Catch ex As Exception

                End Try
            End Try
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim qhMessage As String = "Choose the WIM format to work with the broadest range of Windows images and to modify them. Choose the ESD format if you prefer a smaller Windows image, with the disadvantage of not being modifiable."
        QuickHelpModule.ShowQuickHelp(qhMessage)
    End Sub
End Class
