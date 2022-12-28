Public Class AddPackageReport

    Public pkgSuccessFailureRatio As Single

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Close()
    End Sub

    Private Sub AddPackageReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            Panel1.BackColor = Color.FromArgb(31, 31, 31)
            GroupBox1.ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            Panel1.BackColor = Color.FromArgb(238, 238, 242)
            GroupBox1.ForeColor = Color.Black
        End If
    End Sub
End Class