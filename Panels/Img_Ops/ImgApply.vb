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
            ProgressPanel.ApplicationSWMPattern = "install*.swm"
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
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
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
End Class
