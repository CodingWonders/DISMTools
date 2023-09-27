Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class InvalidSettingsDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub InvalidSettingsDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            Label1.ForeColor = Color.FromArgb(0, 122, 204)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            Label1.ForeColor = Color.FromArgb(0, 51, 153)
        End If
        If MainForm.isExeProblematic Then
            Label3.Text = "The specified DISM executable does not exist:" & CrLf & Quote & MainForm.ProblematicStrings(0) & Quote
        Else
            Label3.Text = "The DISM executable setting seems to be in order"
        End If
        If MainForm.isLogFontProblematic Then
            Label4.Text = "The specified log font does not exist in this system:" & CrLf & Quote & MainForm.ProblematicStrings(1) & Quote
        Else
            Label4.Text = "The log font setting seems to be in order"
        End If
        If MainForm.isLogFileProblematic Then
            Label5.Text = "The specified log file does not exist:" & CrLf & Quote & MainForm.ProblematicStrings(2) & Quote
        Else
            Label5.Text = "The log file setting seems to be in order"
        End If
        If MainForm.isScratchDirProblematic Then
            Label6.Text = "The specified scratch directory does not exist:" & CrLf & Quote & MainForm.ProblematicStrings(3) & Quote
        Else
            Label6.Text = "The scratch directory setting seems to be in order"
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class
