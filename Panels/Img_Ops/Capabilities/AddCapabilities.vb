Imports System.Windows.Forms
Imports Microsoft.Win32
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class AddCapabilities

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub AddCapability_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Label4.Text &= " Only not installed capabilities (" & ListView1.Items.Count & ") are shown"
                    Case "ESN"
                        Label4.Text &= " Solo las funcionalidades no instaladas (" & ListView1.Items.Count & ") son mostradas"
                End Select
            Case 1
                Label4.Text &= " Only not installed capabilities (" & ListView1.Items.Count & ") are shown"
            Case 2
                Label4.Text &= " Solo las funcionalidades no instaladas (" & ListView1.Items.Count & ") son mostradas"
        End Select
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Label2.Enabled = True
            RichTextBox1.Enabled = True
            Button1.Enabled = True
            Button4.Enabled = True
        Else
            Label2.Enabled = False
            RichTextBox1.Enabled = False
            Button1.Enabled = False
            Button4.Enabled = False
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            ' It is actually getting stuff from the registry, as changes in this group policy add/edit a registry key. Change this if it's not accurate,
            ' as the documentation doesn't specify which group policy is detected
            Dim capGPOSourceRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Servicing", False)
            Dim capGPOSource As String = capGPOSourceRk.GetValue("LocalSourcePath").ToString()
            capGPOSourceRk.Close()
            If capGPOSource.StartsWith("wim:\", StringComparison.OrdinalIgnoreCase) Then
                TextBoxSourcePanel.Visible = False
                WimFileSourcePanel.Visible = True
                Dim parts() As String = capGPOSource.Split(":")
                Label3.Text = parts(parts.Length - 1)
                Label5.Text = parts(1).Replace("\", "").Trim() & ":" & parts(2)
                If Label5.Text.EndsWith(":" & parts(parts.Length - 1)) Then Label5.Text = Label5.Text.Replace(":" & parts(parts.Length - 1), "").Trim()
            End If
        Catch ex As Exception
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            MsgBox("Could not gather source from group policy. Reason:" & CrLf & CrLf & ex.ToString(), vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("No se pudo recopilar el origen de las políticas de grupo. Razón:" & CrLf & CrLf & ex.ToString(), vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("Could not gather source from group policy. Reason:" & CrLf & CrLf & ex.ToString(), vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("No se pudo recopilar el origen de las políticas de grupo. Razón:" & CrLf & CrLf & ex.ToString(), vbOKOnly + vbCritical, Text)
            End Select
        End Try
    End Sub
End Class
