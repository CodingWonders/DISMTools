Imports System.Windows.Forms
Imports Microsoft.Win32
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class SetOSUninstWindow

    Dim uninstWindow As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If NumericUpDown1.Value = uninstWindow Then Exit Sub
        ProgressPanel.osUninstDayCount = NumericUpDown1.Value
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 88
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SetOSUninstWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Get the uninstall window from the registry first
        Try
            Dim osUninstReg As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\Setup")
            uninstWindow = CInt(osUninstReg.GetValue("UninstallWindow").ToString())
            osUninstReg.Close()
        Catch ex As Exception
            MsgBox(ex.ToString() & " - " & ex.Message & "(HRESULT " & ex.HResult & ")", vbOKOnly + vbCritical, Label1.Text)
            Close()
        End Try
        If (uninstWindow >= 2 And uninstWindow <= 60) Then NumericUpDown1.Value = uninstWindow
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
    End Sub
End Class
