Imports System.Windows.Forms

Public Class ImgIndexSwitch
    Implements IImageTaskDialog

    Public indexNames(1024) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        DynaLog.LogMessage("Preparing to switch image indexes...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.SwitchSourceImg = MainForm.SourceImg
        ProgressPanel.SwitchTarget = MainForm.MountDir
        ProgressPanel.SwitchSourceIndex = MainForm.ImgIndex
        ProgressPanel.SwitchTargetIndex = NumericUpDown1.Value
        ProgressPanel.SwitchTargetIndexName = Label5.Text
        If RadioButton1.Checked Then
            ProgressPanel.SwitchCommitSourceIndex = True
        Else
            ProgressPanel.SwitchCommitSourceIndex = False
        End If
        If MainForm.isReadOnly Then
            ProgressPanel.SwitchMountAsReadOnly = True
        Else
            ProgressPanel.SwitchMountAsReadOnly = False
        End If
        ProgressPanel.OperationNum = 996
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        DynaLog.LogMessage("Opening image index switch dialog...")
        DynaLog.LogMessage("Stopping mounted image detector...")
        MainForm.StopMountedImageDetector()
        DynaLog.LogMessage("Getting image indexes...")
        ProgressPanel.OperationNum = 995
        PleaseWaitDialog.indexesSourceImg = MainForm.SourceImg
        PleaseWaitDialog.Label2.Text = LocalizationService.ForSection("ImageIndexSwitch.Initialize")("Getting.Image.Indexes.Label")
        PleaseWaitDialog.ShowDialog(Me)
        MainForm.StartMountedImageDetector()
        Return (PleaseWaitDialog.imgIndexes > 1)
    End Function

    Private Sub ImgIndexSwitch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Text = LocalizationService.ForSection("ImageIndexSwitch")("Image.Indexes.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("ImageIndexSwitch").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("ImageIndexSwitch")("Image.Label")
        Label3.Text = LocalizationService.ForSection("ImageIndexSwitch")("Unmounting.Source.Label")
        Label4.Text = LocalizationService.ForSection("ImageIndexSwitch")("Destination.Mount.Label")
        Label6.Text = LocalizationService.ForSection("ImageIndexSwitch")("Already.Mounted.Label")
        OK_Button.Text = LocalizationService.ForSection("ImageIndexSwitch")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ImageIndexSwitch")("Cancel.Button")
        GroupBox1.Text = LocalizationService.ForSection("ImageIndexSwitch")("Indexes.Group")
        RadioButton1.Text = LocalizationService.ForSection("ImageIndexSwitch")("Save.Changes.RadioButton")
        RadioButton2.Text = LocalizationService.ForSection("ImageIndexSwitch")("DiscardChanges.RadioButton")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        NumericUpDown1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        Label5.Text = indexNames(NumericUpDown1.Value - 1)
        If Label5.Text = MainForm.CurrentImage.ImageName Then
            Label6.Visible = True
            OK_Button.Enabled = False
        Else
            Label6.Visible = False
            OK_Button.Enabled = True
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        Label5.Text = indexNames(NumericUpDown1.Value - 1)
        If Label5.Text = MainForm.CurrentImage.ImageName Then
            DynaLog.LogMessage("The index target is already mounted.")
            Label6.Visible = True
            OK_Button.Enabled = False
        Else
            Label6.Visible = False
            OK_Button.Enabled = True
        End If
    End Sub
End Class
