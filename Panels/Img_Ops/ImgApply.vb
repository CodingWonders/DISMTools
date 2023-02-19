Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding

Public Class ImgApply

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ProgressPanel.ApplicationSourceImg = TextBox1.Text
        ProgressPanel.ApplicationIndex = NumericUpDown1.Value
        If RadioButton1.Checked Then
            ProgressPanel.ApplicationDestDir = TextBox2.Text
            ProgressPanel.ApplicationDestDrive = ""
        Else
            ProgressPanel.ApplicationDestDir = ""
            ProgressPanel.ApplicationDestDrive = TextBox3.Text
        End If
        If CheckBox1.Checked Then
            ProgressPanel.ApplicationCheckInt = True
        Else
            ProgressPanel.ApplicationCheckInt = False
        End If
        If CheckBox2.Checked Then
            ProgressPanel.ApplicationVerify = True
        Else
            ProgressPanel.ApplicationVerify = False
        End If
        If CheckBox3.Checked Then
            ProgressPanel.ApplicationReparsePt = True
        Else
            ProgressPanel.ApplicationReparsePt = False
        End If
        If CheckBox4.Checked Then
            ProgressPanel.ApplicationSWMPattern = Path.GetDirectoryName(TextBox1.Text) & "\" & TextBox4.Text & "*.swm"
        Else
            ProgressPanel.ApplicationSWMPattern = ""
        End If
        If CheckBox5.Checked Then
            ProgressPanel.ApplicationValidateForTD = True
        Else
            ProgressPanel.ApplicationValidateForTD = False
        End If
        If CheckBox6.Checked Then
            ProgressPanel.ApplicationUseWimBoot = True
        Else
            ProgressPanel.ApplicationUseWimBoot = False
        End If
        If CheckBox7.Checked Then
            ProgressPanel.ApplicationCompactMode = True
        Else
            ProgressPanel.ApplicationCompactMode = False
        End If
        If CheckBox8.Checked Then
            ProgressPanel.ApplicationUseExtAttr = True
        Else
            ProgressPanel.ApplicationUseExtAttr = False
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 3
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ApplicationDriveSpecifier.ShowDialog()
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            TextBox2.Enabled = True
            Button2.Enabled = True
            Button3.Enabled = False
        Else
            TextBox2.Enabled = False
            Button2.Enabled = False
            Button3.Enabled = True
        End If
    End Sub

    Private Sub ImgApply_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            TextBox3.BackColor = Color.FromArgb(31, 31, 31)
            TextBox4.BackColor = Color.FromArgb(31, 31, 31)
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
            GroupBox3.ForeColor = Color.White
            GroupBox4.ForeColor = Color.White
            ListBox1.BackColor = Color.FromArgb(31, 31, 31)
            StatusStrip1.BackColor = Color.FromArgb(31, 31, 31)
            NumericUpDown1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            TextBox3.BackColor = Color.FromArgb(238, 238, 242)
            TextBox4.BackColor = Color.FromArgb(238, 238, 242)
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
            GroupBox3.ForeColor = Color.Black
            GroupBox4.ForeColor = Color.Black
            NumericUpDown1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        NumericUpDown1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        ListBox1.ForeColor = ForeColor
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
        ToolStripStatusLabel1.Text = "Please specify the naming pattern of the SWM files"
        If MainForm.SourceImg = "N/A" Then
            UseMountedImgBtn.Enabled = False
        Else
            UseMountedImgBtn.Enabled = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        GetMaxIndexCount(TextBox1.Text)
        If TextBox1.Text.EndsWith(".swm") Then
            CheckBox4.Checked = True
            Button4.PerformClick()
        End If
    End Sub

    Sub GetMaxIndexCount(ImgFile As String)
        File.WriteAllText(".\bin\exthelpers\temp.bat", _
                          "@echo off" & CrLf & _
                          "dism /English /get-imageinfo /imagefile=" & ImgFile & " | find /c " & Quote & "Index" & Quote & " > .\indexcount", ASCII)
        Process.Start(".\bin\exthelpers\temp.bat").WaitForExit()
        MainForm.imgIndexCount = CInt(My.Computer.FileSystem.ReadAllText(".\indexcount"))
        NumericUpDown1.Maximum = MainForm.imgIndexCount
        File.Delete(".\indexcount")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        FolderBrowserDialog1.ShowDialog()
        If DialogResult.OK Then
            TextBox2.Text = FolderBrowserDialog1.SelectedPath
        Else
            TextBox2.Text = ""
        End If
    End Sub

    Private Sub UseMountedImgBtn_Click(sender As Object, e As EventArgs) Handles UseMountedImgBtn.Click
        TextBox1.Text = MainForm.SourceImg
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        TextBox4.Text = Path.GetFileNameWithoutExtension(TextBox1.Text)
        ScanSwmPattern(TextBox4.Text)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ScanSwmPattern(TextBox4.Text)
    End Sub

    Sub ScanSwmPattern(PatternName As String)
        ListBox1.Items.Clear()
        If TextBox1.Text = "" Or PatternName = "" Then
            MsgBox("Please specify a source WIM file. This will let you use the SWM files for later image application", vbOKOnly + vbCritical, "Apply an image")
            ToolStripStatusLabel1.Text = "This naming pattern returns " & ListBox1.Items.Count & " SWM files"
            Beep()
            Exit Sub
        End If
        For Each swmFile In My.Computer.FileSystem.GetFiles(Path.GetDirectoryName(TextBox1.Text), FileIO.SearchOption.SearchTopLevelOnly, "*.swm")
            If Path.GetFileNameWithoutExtension(swmFile).StartsWith(PatternName) Then
                ListBox1.Items.Add(Path.GetFileName(swmFile))
            End If
        Next
        ToolStripStatusLabel1.Text = "This naming pattern returns " & ListBox1.Items.Count & " SWM files"
        If ListBox1.Items.Count <= 0 Then Beep()
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        SWMFilePanel.Enabled = CheckBox4.Checked = True
    End Sub
End Class
