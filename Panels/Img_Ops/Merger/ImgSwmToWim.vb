Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.Dism
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars

Public Class ImgSwmToWim

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.imgSwmSource = TextBox1.Text
        ProgressPanel.imgMergerIndex = NumericUpDown1.Value
        ProgressPanel.imgWimDestination = TextBox2.Text
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 992
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SaveFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        TextBox2.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub ImgSwmToWim_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("Img.Swm")("MergeSwmfiles.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("Img.Swm").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("Img.Swm")("SourceSwmfile.Label")
        Label3.Text = LocalizationService.ForSection("Img.Swm")("Notewhen.Specifying.Message")
        Label4.Text = LocalizationService.ForSection("Img.Swm")("Destination.WIM.File.Label")
        Label5.Text = LocalizationService.ForSection("Img.Swm")("Index.Label")
        Button1.Text = LocalizationService.ForSection("Img.Swm")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("Img.Swm")("Browse.Button")
        OK_Button.Text = LocalizationService.ForSection("Img.Swm")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("Img.Swm")("Cancel.Button")
        LinkLabel1.Text = LocalizationService.ForSection("Img.Swm")("LearnHow.Link")
        ListView1.Columns(0).Text = LocalizationService.ForSection("Img.Swm")("Index.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("Img.Swm")("ImageName.Column")
        ListView1.Columns(2).Text = LocalizationService.ForSection("Img.Swm")("ImageDescription.Column")
        ListView1.Columns(3).Text = LocalizationService.ForSection("Img.Swm")("ImageVersion.Column")
        OpenFileDialog1.Title = LocalizationService.ForSection("Img.Swm")("Source.Swmfile.Title")
        SaveFileDialog1.Title = LocalizationService.ForSection("Img.Swm")("Dest.WIM.File.Title")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        GroupBox3.ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
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

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://linustechtips.com/topic/1318158-merge-two-swm-files/")
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("Getting and displaying information of specified image file...")
            DynaLog.LogMessage("Image file to get information about: " & Quote & TextBox1.Text & Quote)
            MainForm.StopMountedImageDetector()
            Try
                DynaLog.LogMessage("Getting information about the image file...")
                ListView1.Items.Clear()
                DynaLog.LogMessage("Initializing API...")
                DismApi.Initialize(DismLogLevel.LogErrors)
                Dim imgInfoCollection As DismImageInfoCollection = DismApi.GetImageInfo(TextBox1.Text)
                DynaLog.LogMessage("Information collection count: " & imgInfoCollection.Count)
                NumericUpDown1.Maximum = imgInfoCollection.Count
                ListView1.Items.AddRange(imgInfoCollection.Select(Function(imgInfo) New ListViewItem(New String() {imgInfo.ImageIndex, imgInfo.ImageName, imgInfo.ImageDescription, imgInfo.ProductVersion.ToString()})).ToArray())
            Catch ex As Exception
                DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
                MsgBox(LocalizationService.ForSection("ImageConversion.SwmToWim")("Get.Index.Image.Label"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Finally
                DynaLog.LogMessage("Shutting down API...")
                Try
                    DismApi.Shutdown()
                Catch ex As Exception
                    ' Don't do anything
                End Try
            End Try
            DynaLog.LogMessage("This process has finished.")
        End If
    End Sub
End Class
