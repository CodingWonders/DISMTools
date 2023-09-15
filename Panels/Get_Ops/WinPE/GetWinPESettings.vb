Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Win32

Public Class GetWinPESettings

    Sub GetPESettings()
        ' Mount the SOFTWARE and SYSTEM keys
        Using reg As New Process
            reg.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe"
            reg.StartInfo.Arguments = "load HKLM\PE_SOFT " & Quote & MainForm.MountDir & "\Windows\system32\config\SOFTWARE" & Quote
            reg.StartInfo.CreateNoWindow = True
            reg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            reg.Start()
            reg.WaitForExit()
            If reg.ExitCode <> 0 Then
                Label5.Text = "Could not get value"
                Button1.Visible = False
            End If
            reg.StartInfo.Arguments = "load HKLM\PE_SYS " & Quote & MainForm.MountDir & "\Windows\system32\config\SYSTEM" & Quote
            reg.Start()
            reg.WaitForExit()
            If reg.ExitCode <> 0 Then
                Label6.Text = "Could not get value"
                Button2.Visible = False
            End If
            Try
                ' Get target path first
                Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("PE_SOFT\Microsoft\Windows NT\CurrentVersion\WinPE", False)
                Label5.Text = regKey.GetValue("InstRoot", "Could not get value").ToString()
                regKey.Close()
                regKey = Registry.LocalMachine.OpenSubKey("PE_SYS\ControlSet001\Services\FBWF", False)
                Label6.Text = regKey.GetValue("WinPECacheThreshold", "Could not get value").ToString() & " MB"
                regKey.Close()
            Catch ex As Exception

            End Try
            ' Unload registry hives
            reg.StartInfo.Arguments = "unload HKLM\PE_SOFT"
            reg.Start()
            reg.WaitForExit()
            reg.StartInfo.Arguments = "unload HKLM\PE_SYS"
            reg.Start()
            reg.WaitForExit()
        End Using
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click, Button2.Click, Button1.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub GetWinPESettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If

        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))

        Button1.Visible = True
        Button2.Visible = True

        ' Get Windows PE settings
        GetPESettings()
    End Sub
End Class
