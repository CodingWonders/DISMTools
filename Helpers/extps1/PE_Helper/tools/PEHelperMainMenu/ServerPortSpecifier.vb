Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class ServerPortSpecifier

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
        NumericUpDown1.Value = 8080
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
                MessageBox.Show(LocalizationService.ForSection("PEHelper.ServerPort").Format("Already.Message", NumericUpDown1.Value), Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                ' This port is free
                MessageBox.Show(LocalizationService.ForSection("PEHelper.ServerPort").Format("InvalidPort.Message", NumericUpDown1.Value), Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End Using
    End Sub
End Class
