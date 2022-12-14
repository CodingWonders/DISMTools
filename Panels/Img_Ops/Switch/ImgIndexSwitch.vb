Imports System.Windows.Forms

Public Class ImgIndexSwitch

    Public indexNames(1024) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
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

    Private Sub ImgIndexSwitch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
        Label5.Text = indexNames(NumericUpDown1.Value - 1)
        If Label5.Text = MainForm.imgMountedName Then
            Label6.Visible = True
            OK_Button.Enabled = False
        Else
            Label6.Visible = False
            OK_Button.Enabled = True
        End If
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        Label5.Text = indexNames(NumericUpDown1.Value - 1)
        If Label5.Text = MainForm.imgMountedName Then
            Label6.Visible = True
            OK_Button.Enabled = False
        Else
            Label6.Visible = False
            OK_Button.Enabled = True
        End If
    End Sub
End Class
