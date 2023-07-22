Imports System.Windows.Forms

Public Class Help_RegisteredAppxPkgsDlg

    Private Sub Help_RegisteredAppxPkgsDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = RemProvAppxPackage.LinkLabel1.Text
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        RichTextBox1.BackColor = BackColor
        RichTextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class
