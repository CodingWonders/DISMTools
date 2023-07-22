Imports System.Windows.Forms
Imports System.IO

Public Class ImgCapture

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ProgressPanel.CaptureSourceDir = TextBox1.Text
        ProgressPanel.CaptureDestinationImage = TextBox2.Text
        ProgressPanel.CaptureName = TextBox3.Text
        ProgressPanel.CaptureDescription = TextBox4.Text
        If CheckBox1.Checked Then
            ProgressPanel.CaptureWimScriptConfig = TextBox5.Text
        Else
            ProgressPanel.CaptureWimScriptConfig = ""
        End If
        Select Case ComboBox1.SelectedIndex
            Case 0
                ProgressPanel.CaptureCompressType = 0
            Case 1
                ProgressPanel.CaptureCompressType = 1
            Case 2
                ProgressPanel.CaptureCompressType = 2
        End Select
        If CheckBox2.Checked Then
            ProgressPanel.CaptureBootable = True
        Else
            ProgressPanel.CaptureBootable = False
        End If
        If CheckBox3.Checked Then
            ProgressPanel.CaptureCheckIntegrity = True
        Else
            ProgressPanel.CaptureCheckIntegrity = False
        End If
        If CheckBox4.Checked Then
            ProgressPanel.CaptureVerify = True
        Else
            ProgressPanel.CaptureVerify = False
        End If
        If CheckBox5.Checked Then
            ProgressPanel.CaptureReparsePt = True
        Else
            ProgressPanel.CaptureReparsePt = False
        End If
        If CheckBox6.Checked Then
            ProgressPanel.CaptureUseWimBoot = True
        Else
            ProgressPanel.CaptureUseWimBoot = False
        End If
        If CheckBox7.Checked Then
            ProgressPanel.CaptureExtendedAttributes = True
        Else
            ProgressPanel.CaptureExtendedAttributes = False
        End If
        If CheckBox8.Checked Then
            ProgressPanel.CaptureMountDestImg = True
            ' Since ProgressPanel doesn't consider what other form variables contain, set them to ProgressPanel variables
            ProgressPanel.UMountLocalDir = True
            ProgressPanel.RandomMountDir = ""
            ProgressPanel.MountDir = MainForm.MountDir
            ProgressPanel.UMountOp = 1
            ProgressPanel.CheckImgIntegrity = False
            ProgressPanel.SaveToNewIndex = False
            ProgressPanel.SourceImg = ProgressPanel.CaptureDestinationImage
            ProgressPanel.isOptimized = False
            ProgressPanel.isIntegrityTested = False
            ProgressPanel.TaskList.AddRange({6, 21, 15})
        Else
            ProgressPanel.CaptureMountDestImg = False
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 6
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgCapture_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            TextBox3.BackColor = Color.FromArgb(31, 31, 31)
            TextBox4.BackColor = Color.FromArgb(31, 31, 31)
            TextBox5.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox1.BackColor = Color.FromArgb(31, 31, 31)
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            TextBox3.BackColor = Color.FromArgb(238, 238, 242)
            TextBox4.BackColor = Color.FromArgb(238, 238, 242)
            TextBox5.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox1.BackColor = Color.FromArgb(238, 238, 242)
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
        End If
        ComboBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        TextBox5.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog()
        If DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        Else
            TextBox1.Text = ""
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SaveFileDialog1.ShowDialog()
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        TextBox2.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Label6.Enabled = True
            TextBox5.Enabled = True
            Button3.Enabled = True
        Else
            Label6.Enabled = False
            TextBox5.Enabled = False
            Button3.Enabled = False
        End If
        GatherValidFields()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox5.Text = OpenFileDialog1.FileName
    End Sub

    Sub GatherValidFields()
        If CheckBox1.Checked Then
            If Directory.Exists(TextBox1.Text) And TextBox2.Text IsNot "" And TextBox3.Text IsNot "" And TextBox5.Text IsNot "" Then
                OK_Button.Enabled = True
            Else
                OK_Button.Enabled = False
            End If
        Else
            If Directory.Exists(TextBox1.Text) And TextBox2.Text IsNot "" And TextBox3.Text IsNot "" Then
                OK_Button.Enabled = True
            Else
                OK_Button.Enabled = False
            End If
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        GatherValidFields()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        GatherValidFields()
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        GatherValidFields()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "none" Then
            Label8.Text = "No compression will be applied to the destination image."
        ElseIf ComboBox1.SelectedItem = "fast" Then
            Label8.Text = "Fast compression will be applied. This is the default option."
        ElseIf ComboBox1.SelectedItem = "maximum" Then
            Label8.Text = "Maximum compression will be applied. This will take the most time, but will result in a smaller image."
        End If
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        GatherValidFields()
    End Sub
End Class
