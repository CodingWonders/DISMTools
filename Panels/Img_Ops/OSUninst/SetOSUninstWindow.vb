Imports System.Windows.Forms
Imports Microsoft.Win32
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class SetOSUninstWindow
    Implements IImageTaskDialog

    Dim uninstWindow As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
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

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        If MainForm.OnlineManagement Then
            DynaLog.LogMessage("The active installation is being managed right now. Checking if it can uninstall an OS...")
            If Not MainForm.CheckOSUninstallCapability() Then
                DynaLog.LogMessage("No rollbacks/uninstallations can be performed.")
                OSNoRollbackErrorDlg.ShowDialog(MainForm)
                Return False
            End If
        Else
            DynaLog.LogMessage("The active installation is not being managed right now.")
            MsgBox(LocalizationService.ForSection("RollbackWindow.Init")("OnlineOnly.Message"), vbOKOnly + vbCritical, Text)
            Return False
        End If
        Return True
    End Function

    Private Sub SetOSUninstWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
            Exit Sub
        End If
        Text = LocalizationService.ForSection("OSRollback")("OSUninstall.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("OSRollback").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("OSRollback")("Default.OS.Message")
        Label3.Text = LocalizationService.ForSection("OSRollback")("Amount.Days.Revert.Label")
        OK_Button.Text = LocalizationService.ForSection("OSRollback")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("OSRollback")("Cancel.Button")
        ' Get the uninstall window from the registry first
        Try
            DynaLog.LogMessage("Getting OS uninstall window...")
            Dim osUninstReg As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\Setup")
            uninstWindow = CInt(osUninstReg.GetValue("UninstallWindow").ToString())
            osUninstReg.Close()
        Catch ex As Exception
            MsgBox(ex.ToString() & " - " & ex.Message & String.Format(LocalizationService.ForSection("OSRollback.Messages")("Hresult.Label"), ex.HResult), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Close()
        End Try
        DynaLog.LogMessage("Uninstall window: " & uninstWindow)
        DynaLog.LogMessage("Checking value...")
        If (uninstWindow >= 2 And uninstWindow <= 60) Then NumericUpDown1.Value = uninstWindow
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub
End Class
