Imports System.Windows.Forms
Imports StarterScript.Classes.ColorUtilities

Public Class ScriptVersionChooser

    Public IsInfinityScript As Boolean

    Private CurrentColorMode As ColorThemeMode

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        IsInfinityScript = RadioButton1.Checked
    End Sub

    Private Sub ScriptVersionChooser_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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

        RadioButton1.BackColor = BackColor
        RadioButton1.ForeColor = ForeColor
        RadioButton2.BackColor = BackColor
        RadioButton2.ForeColor = ForeColor
    End Sub
End Class
