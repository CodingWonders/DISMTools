Imports System.Windows.Forms
Imports StarterScriptEditor.Classes.ColorUtilities

Public Class CryptographicProgressDialog

    Private CurrentColorMode As ColorThemeMode

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CryptographicProgressDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CurrentColorMode = MainForm.CurrentColorMode
        SetColorMode()
    End Sub

    Private Sub SetColorMode()
        Select Case CurrentColorMode
            Case ColorThemeMode.Light
                WindowHelper.ToggleDarkTitleBar(Handle, False)

                BackColor = Color.FromArgb(239, 239, 242)
                ForeColor = Color.Black
            Case ColorThemeMode.Dark
                WindowHelper.ToggleDarkTitleBar(Handle, True)

                BackColor = Color.FromArgb(32, 32, 32)
                ForeColor = Color.White
        End Select
    End Sub
End Class
