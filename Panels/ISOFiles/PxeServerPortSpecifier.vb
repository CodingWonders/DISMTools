Imports System.Windows.Forms

Public Class PxeServerPortSpecifier

    Public ServerPort As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        ServerPort = NumericUpDown1.Value
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        NumericUpDown1.Value = MainForm.PXEServerPort
    End Sub

    Private Sub PxeServerPortSpecifier_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        NumericUpDown1.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Using netstatProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "cmd.exe"),
                .Arguments = String.Format("/c netstat -ano | findstr /R {0}:{1}{0}", Quote, NumericUpDown1.Value),
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden
            }
        }
            netstatProc.Start()
            netstatProc.WaitForExit()
            If netstatProc.ExitCode = 0 Then
                ' This port is in use
                MessageBox.Show(LocalizationService.ForSection("ISOFiles.PXEServerPort").Format("Already.Label", NumericUpDown1.Value), Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                ' This port is free
                MessageBox.Show(LocalizationService.ForSection("ISOFiles.PXEServerPort").Format("Port.Label", NumericUpDown1.Value), Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub
End Class
