Imports System.Windows.Forms
Imports Microsoft.Win32
Imports Microsoft.VisualBasic.ControlChars
Imports System.IO

Public Class SetPEScratchSpace
    Implements IImageTaskDialog

    Sub GetScratchSpace()
        DynaLog.LogMessage("Preparing to get Windows PE settings...")
        DynaLog.LogMessage("Loading SYSTEM hive of WinPE image...")
        Dim regExitCode As Integer = RegistryHelper.LoadRegistryHive(Path.Combine(MainForm.MountDir, "Windows", "system32", "config", "SYSTEM"), "HKLM\PE_SYS")
        DynaLog.LogMessage("REG hive exit code: " & Hex(regExitCode))
        Try
            DynaLog.LogMessage("Getting scratch space...")
            Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("PE_SYS\ControlSet001\Services\FBWF", False)
            DynaLog.LogMessage("Scratch space: " & regKey.GetValue("WinPECacheThreshold", 0) & " MB")
            If regKey.GetValue("WinPECacheThreshold", "").ToString() <> "" Then
                If Not ComboBox1.Items.Contains(regKey.GetValue("WinPECacheThreshold", "").ToString()) Then
                    Label5.Visible = True
                End If
            End If
            ComboBox1.SelectedText = regKey.GetValue("WinPECacheThreshold", "").ToString()
            regKey.Close()
        Catch ex As Exception

        End Try
        DynaLog.LogMessage("Unloading hives...")
        ' Unload registry hives
        RegistryHelper.UnloadRegistryHive("HKLM\PE_SYS")
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.peNewScratchSpace = ComboBox1.SelectedItem
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 83
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        DynaLog.LogMessage("Opening scratch space configuration dialog...")
        If MainForm.ImgBW.IsBusy Then
            DynaLog.LogMessage("Background processes are still busy.")
            BGProcsBusyDialog.ShowDialog(Me)
            Return False
        End If
        Return True
    End Function

    Private Sub SetPEScratchSpace_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Text = LocalizationService.ForSection("PE.Scratch")("Window.Title")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("PE.Scratch").Format("Header.Title", Text)
        Label2.Text = LocalizationService.ForSection("PE.Scratch")("Description.Message")
        Label3.Text = LocalizationService.ForSection("PE.Scratch")("Space.Label")
        OK_Button.Text = LocalizationService.ForSection("PE.Scratch")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("PE.Scratch")("Cancel.Button")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.ForeColor = ForeColor
        Label5.Visible = False
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        GetScratchSpace()
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub
End Class
