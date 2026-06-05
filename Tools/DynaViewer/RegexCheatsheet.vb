Imports System.Windows.Forms
Imports DynaViewer.Classes.ColorUtilities

Public Class RegexCheatsheet

    Private Sub SetColorMode(ByVal NewColorMode As ColorThemeMode)
        Select Case NewColorMode
            Case ColorThemeMode.Light
                WindowHelper.ToggleDarkTitleBar(Handle, False)

                BackColor = Color.FromArgb(239, 239, 242)
                ForeColor = Color.Black
            Case ColorThemeMode.Dark
                WindowHelper.ToggleDarkTitleBar(Handle, True)

                BackColor = Color.FromArgb(32, 32, 32)
                ForeColor = Color.White
        End Select

        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        TopMost = CheckBox1.Checked
    End Sub

    Private Sub RegexCheatsheet_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
        If Visible Then
            SetColorMode(MainForm.CurrentColorMode)
        End If
    End Sub
End Class
