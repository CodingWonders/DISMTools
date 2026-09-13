Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class InvalidSettingsDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub InvalidSettingsDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("Settings.Dialog")("Detected.Label")
        Label1.Text = LocalizationService.ForSection("InvalidSettings")("Found.Label")
        Label2.Text = LocalizationService.ForSection("Settings.Dialog")("Reset.Default.Message")
        Button1.Text = LocalizationService.ForSection("Settings.Dialog")("Ok.Button")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        If MainForm.isExeProblematic Then
            Label3.Text = LocalizationService.ForSection("Settings.Dialog").Format("Dismexecutable.Exist.Item", MainForm.ProblematicStrings(0))
        Else
            Label3.Text = LocalizationService.ForSection("Settings.Dialog")("DISM.Executable.Label")
        End If
        If MainForm.isLogFontProblematic Then
            Label4.Text = LocalizationService.ForSection("Settings.Dialog").Format("Log.Font.Exist.Item", MainForm.ProblematicStrings(1))
        Else
            Label4.Text = LocalizationService.ForSection("Settings.Dialog")("Log.Font.Setting.Label")
        End If
        If MainForm.isLogFileProblematic Then
            Label5.Text = LocalizationService.ForSection("Settings.Dialog").Format("Log.File.Exist.Item", MainForm.ProblematicStrings(2))
        Else
            Label5.Text = LocalizationService.ForSection("Settings.Dialog")("Log.File.Setting.Label")
        End If
        If MainForm.isScratchDirProblematic Then
            Label6.Text = LocalizationService.ForSection("Settings.Dialog").Format("Scratch.Dir.Exist.Item", MainForm.ProblematicStrings(3))
        Else
            Label6.Text = LocalizationService.ForSection("Settings.Dialog")("Scratch.Dir.Set.Label")
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
    End Sub
End Class
