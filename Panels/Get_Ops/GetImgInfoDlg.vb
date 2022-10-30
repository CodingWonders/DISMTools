Imports System.Windows.Forms
Imports System.IO

Public Class GetImgInfoDlg

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If RadioButton1.Checked Then
            ProgressPanel.GetFromMountedImg = True
            ProgressPanel.InfoFromSourceImg = MainForm.SourceImg
            ProgressPanel.InfoFromSpecificImg = ""
        Else
            ProgressPanel.GetFromMountedImg = False
            If File.Exists(TextBox1.Text) Then
                ProgressPanel.InfoFromSourceImg = ""
                ProgressPanel.InfoFromSpecificImg = TextBox1.Text
            Else
                Do Until File.Exists(TextBox1.Text)
                    Button1.PerformClick()
                Loop
                ProgressPanel.InfoFromSourceImg = ""
                ProgressPanel.InfoFromSpecificImg = TextBox1.Text
            End If
        End If
        If CheckBox1.Checked Then
            ProgressPanel.GetSpecificIndexInfo = True
            If RadioButton3.Checked Then
                ProgressPanel.GetFromMountedIndex = True
                ProgressPanel.InfoFromSourceIndex = MainForm.ImgIndex
                ProgressPanel.InfoFromSpecificIndex = 0
            Else
                ProgressPanel.GetFromMountedIndex = False
                ProgressPanel.InfoFromSourceIndex = 0
                ProgressPanel.InfoFromSpecificIndex = NumericUpDown1.Value
            End If
        Else
            ProgressPanel.GetSpecificIndexInfo = False
        End If
        ProgressPanel.OperationNum = 11
        ProgressPanel.ShowDialog(MainForm)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            Label3.Enabled = False
            TextBox1.Enabled = False
            Button1.Enabled = False
        Else
            Label3.Enabled = True
            TextBox1.Enabled = True
            Button1.Enabled = True
        End If
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        If RadioButton3.Checked Then
            NumericUpDown1.Enabled = False
        Else
            NumericUpDown1.Enabled = True
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Label4.Enabled = True
            RadioButton3.Enabled = True
            RadioButton4.Enabled = True
            If RadioButton3.Checked Then
                NumericUpDown1.Enabled = False
            Else
                NumericUpDown1.Enabled = True
            End If
        Else
            Label4.Enabled = False
            RadioButton3.Enabled = False
            RadioButton4.Enabled = False
            NumericUpDown1.Enabled = False
        End If
    End Sub

    Private Sub GetImgInfoDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
        NumericUpDown1.Maximum = MainForm.imgIndexCount
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub
End Class
