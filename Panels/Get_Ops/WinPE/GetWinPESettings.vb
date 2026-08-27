Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Win32
Imports System.ComponentModel
Imports System.IO

Public Class GetWinPESettings
    Implements IImageTaskDialog

    Sub GetPESettings()
        ' Mount the SOFTWARE and SYSTEM keys
        DynaLog.LogMessage("Preparing to get Windows PE settings...")
        DynaLog.LogMessage("Loading SOFTWARE hive of WinPE image...")
        Dim regExitCode As Integer = RegistryHelper.LoadRegistryHive(Path.Combine(MainForm.MountDir, "Windows", "system32", "config", "SOFTWARE"), "HKLM\PE_SOFT")
        DynaLog.LogMessage("REG hive exit code: " & Hex(regExitCode))
        If regExitCode <> 0 Then
            DynaLog.LogMessage("Could not load the hive.")
            DynaLog.LogMessage("Error message: " & New Win32Exception(regExitCode).Message)
            Label5.Text = LocalizationService.ForSection("WinPESettings.GetPESettings")("GetValue.Label")
            Button1.Visible = False
        End If
        DynaLog.LogMessage("Loading SYSTEM hive of WinPE image...")
        regExitCode = RegistryHelper.LoadRegistryHive(Path.Combine(MainForm.MountDir, "Windows", "system32", "config", "SYSTEM"), "HKLM\PE_SYS")
        DynaLog.LogMessage("REG hive exit code: " & Hex(regExitCode))
        If regExitCode <> 0 Then
            DynaLog.LogMessage("Could not load the hive.")
            DynaLog.LogMessage("Error message: " & New Win32Exception(regExitCode).Message)
            Label6.Text = LocalizationService.ForSection("WinPESettings.GetPESettings")("GetValue.Label")
            Button2.Visible = False
        End If
        Try
            Dim msg As String = ""
            msg = LocalizationService.ForSection("WinPESettings.GetPESettings")("GetValue.Message")
            DynaLog.LogMessage("Getting target path...")
            ' Get target path first
            Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("PE_SOFT\Microsoft\Windows NT\CurrentVersion\WinPE", False)
            DynaLog.LogMessage("Target path: " & Quote & regKey.GetValue("InstRoot", "") & Quote)
            Label5.Text = regKey.GetValue("InstRoot", msg).ToString()
            regKey.Close()
            DynaLog.LogMessage("Getting scratch space...")
            regKey = Registry.LocalMachine.OpenSubKey("PE_SYS\ControlSet001\Services\FBWF", False)
            DynaLog.LogMessage("Scratch space: " & regKey.GetValue("WinPECacheThreshold", 0) & " MB")
            Dim scSize As String = regKey.GetValue("WinPECacheThreshold", "").ToString()
            Label6.Text = If(Not scSize = "", scSize & " MB", msg)
            regKey.Close()
        Catch ex As Exception

        End Try
        DynaLog.LogMessage("Unloading hives...")
        ' Unload registry hives
        RegistryHelper.UnloadRegistryHive("HKLM\PE_SOFT")
        RegistryHelper.UnloadRegistryHive("HKLM\PE_SYS")
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        DynaLog.LogMessage("Opening WinPE configuration observation dialog...")
        If MainForm.ImgBW.IsBusy Then
            DynaLog.LogMessage("Background processes are still busy.")
            BGProcsBusyDialog.ShowDialog(Me)
            Return False
        End If
        Return True
    End Function

    Private Sub GetWinPESettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
            Exit Sub
        End If
        Text = LocalizationService.ForSection("WinPESettings")("Get.Windows.Pesettings.Label")
        ImageTaskHeader1.ItemText = Text
        Label2.Text = LocalizationService.ForSection("WinPESettings")("Windows.Label")
        Label3.Text = LocalizationService.ForSection("WinPESettings")("TargetPath.Label")
        Label4.Text = LocalizationService.ForSection("WinPESettings")("ScratchSpace.Label")
        Button1.Text = LocalizationService.ForSection("WinPESettings")("Change.Button")
        Button2.Text = LocalizationService.ForSection("WinPESettings")("Change.Button")
        OK_Button.Text = LocalizationService.ForSection("WinPESettings")("Ok.Button")
        Button4.Text = LocalizationService.ForSection("WinPESettings")("Save.Button")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor

        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        Button1.Visible = True
        Button2.Visible = True

        ' Get Windows PE settings
        GetPESettings()
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DynaLog.LogMessage("Preparing to configure target path...")
        Visible = False
        If SetPETargetPath.ShowDialog(MainForm) = Windows.Forms.DialogResult.OK Then GetPESettings()
        Visible = True
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DynaLog.LogMessage("Preparing to configure scratch space...")
        Visible = False
        If SetPEScratchSpace.ShowDialog(MainForm) = Windows.Forms.DialogResult.OK Then GetPESettings()
        Visible = True
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If MainForm.ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Saving Windows PE configuration information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = MainForm.SourceImg
            ImgInfoSaveDlg.SaveTarget = MainForm.ImgInfoSFD.FileName
            ImgInfoSaveDlg.ImgMountDir = If(Not MainForm.OnlineManagement, MainForm.MountDir, "")
            ImgInfoSaveDlg.OnlineMode = MainForm.OnlineManagement
            ImgInfoSaveDlg.SkipQuestions = MainForm.SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = MainForm.AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = 9
            ImgInfoSaveDlg.ImageToGetInfoFrom = MainForm.CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub
End Class
