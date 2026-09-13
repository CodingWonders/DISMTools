Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism

Public Class ImgUMount

    Dim UMountOperations() As String = New String(1) {"", ""}

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        If RadioButton1.Checked = True Then
            DynaLog.LogMessage("The image mounted in this project will be unmounted.")
            ProgressPanel.UMountLocalDir = True
            ProgressPanel.RandomMountDir = ""   ' Hope there isn't anything to set here
            ProgressPanel.MountDir = MainForm.MountDir
        Else
            DynaLog.LogMessage("An image mounted in a different folder will be unmounted.")
            DynaLog.LogMessage("- Provided mount directory: " & Quote & TextBox1.Text & Quote)
            ProgressPanel.UMountLocalDir = False
            ' Determine if given mount dir exists
            DynaLog.LogMessage("Checking if the provided mount directory exists...")
            If Directory.Exists(TextBox1.Text) Then
                DynaLog.LogMessage("The provided mount directory exists. Checking if an image is mounted there...")
                ' Detect whether the mount dir has an image mounted (I don't believe on what users claim, just to be sure)
                If MainForm.MountedImageList.Select(Function(image) image.ImageMountDirectory).Contains(TextBox1.Text) Then
                    DynaLog.LogMessage("An image is mounted there. This is a valid mount directory.")
                    ProgressPanel.RandomMountDir = TextBox1.Text
                Else
                    DynaLog.LogMessage("No image is mounted there. This is not a valid mount directory.")
                    MsgBox(LocalizationService.ForSection("ImgUMount.Validation")("Dir.Invalid.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    Exit Sub
                End If
            Else
                DynaLog.LogMessage("The provided mount directory does not exist.")
                MsgBox(LocalizationService.ForSection("ImgUMount.Validation")("Dir.Missing.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Exit Sub
            End If
        End If
        If ComboBox1.SelectedIndex = 0 Then
            ProgressPanel.UMountOp = 0
        ElseIf ComboBox1.SelectedIndex = 1 Then
            ProgressPanel.UMountOp = 1
        End If
        If CheckBox1.Checked Then
            ProgressPanel.CheckImgIntegrity = True
        Else
            ProgressPanel.CheckImgIntegrity = False
        End If
        If CheckBox2.Checked Then
            ProgressPanel.SaveToNewIndex = True
        Else
            ProgressPanel.SaveToNewIndex = False
        End If
        If MainForm.isProjectLoaded Then
            ProgressPanel.UMountImgIndex = MainForm.ImgIndex
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 21
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgUMount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedText = ""
        ComboBox1.Items.Clear()
        Text = LocalizationService.ForSection("ImgUMount")("UnmountImage.Label")
        ImageTaskHeader1.ItemText = Text
        Label2.Text = LocalizationService.ForSection("ImgUMount")("Options.Required.Label")
        Label3.Text = LocalizationService.ForSection("ImgUMount")("Dir.Label")
        Label4.Text = LocalizationService.ForSection("ImgUMount")("MountDirectory.Label")
        Label7.Text = LocalizationService.ForSection("ImgUMount")("UnmountOperation.Label")
        CheckBox1.Text = LocalizationService.ForSection("ImgUMount")("Integrity.CheckBox")
        CheckBox2.Text = LocalizationService.ForSection("ImgUMount")("Append.Changes.CheckBox")
        Button1.Text = LocalizationService.ForSection("ImgUMount")("Pick.Button")
        OK_Button.Text = LocalizationService.ForSection("ImgUMount")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ImgUMount")("Cancel.Button")
        FolderBrowserDialog1.Description = LocalizationService.ForSection("ImgUMount")("Dir.Required.Description")
        RadioButton1.Text = LocalizationService.ForSection("ImgUMount")("LoadedProject.RadioButton")
        RadioButton2.Text = LocalizationService.ForSection("ImgUMount")("LocatedSomewhere.RadioButton")
        UMountOperations(0) = LocalizationService.ForSection("ImgUMount")("Save.Changes.Unmount.Item")
        UMountOperations(1) = LocalizationService.ForSection("ImgUMount")("Discard.Changes.Unmount.Item")
        GroupBox1.Text = LocalizationService.ForSection("ImgUMount")("MountDirectory.Group")
        GroupBox2.Text = LocalizationService.ForSection("ImgUMount")("Additional.Options.Group")
        ComboBox1.Items.AddRange(UMountOperations)
        ComboBox1.SelectedIndex = 0
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        If RadioButton1.Checked Then
            Label4.Enabled = False
            TextBox1.Enabled = False
            Button1.Enabled = False
        Else
            Label4.Enabled = True
            TextBox1.Enabled = True
            Button1.Enabled = True
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            Label4.Enabled = False
            TextBox1.Enabled = False
            Button1.Enabled = False
        Else
            Label4.Enabled = True
            TextBox1.Enabled = True
            Button1.Enabled = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim selectedImage As WindowsImage = PopupMountedImagePicker.PickImage()
        If selectedImage IsNot Nothing Then
            TextBox1.Text = selectedImage.ImageMountDirectory
            DynaLog.LogMessage("Checking if selected item is the mount directory of the project...")
            If TextBox1.Text = MainForm.MountDir Then
                DynaLog.LogMessage("The selected item is the mount directory of the project.")
                TextBox1.Text = ""
                RadioButton1.Checked = True
                RadioButton2.Checked = False
            End If
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Select Case ComboBox1.SelectedIndex
            Case 0
                CheckBox1.Enabled = True
                CheckBox2.Enabled = True
            Case 1
                CheckBox1.Enabled = False
                CheckBox2.Enabled = False
        End Select
    End Sub
End Class
