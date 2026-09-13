Imports System.IO
Imports Microsoft.Win32
Imports Microsoft.VisualBasic.ControlChars

Public Class RegistryControlPanel

    Dim PreviouslyLoadedKey As String       ' The previous key regedit had opened
    Dim RegHiveLocation As String

    Dim LoadButtonText As String = "Load"
    Dim UnloadButtonText As String = "Unload"
    Dim OpenButtonText As String = "Open"

    Dim LoadedHives As Integer = 0

    Dim CustomHiveLoaded As Boolean

    Private Sub RegistryControlPanel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("RegistryPanel")("Image.Hives.Label")
        LoadButtonText = LocalizationService.ForSection("RegistryPanel")("Load.Label")
        UnloadButtonText = LocalizationService.ForSection("RegistryPanel")("Unload.Label")
        OpenButtonText = LocalizationService.ForSection("RegistryPanel")("Open.Button")
        Label1.Text = LocalizationService.ForSection("RegistryPanel")("Tool.Lets.Load.Message")
        Label5.Text = LocalizationService.ForSection("RegistryPanel")("Ntuserdatdefault.User.Label")
        Label6.Text = LocalizationService.ForSection("RegistryPanel")("Load.Different.Label")
        Label7.Text = LocalizationService.ForSection("RegistryPanel")("HiveLocation.Label")
        Label8.Text = LocalizationService.ForSection("RegistryPanel")("PathRegistry.Label")
        Button5.Text = LocalizationService.ForSection("RegistryPanel")("Browse.Button")
        GroupBox1.Text = LocalizationService.ForSection("RegistryPanel")("Load.Custom.Hive")
        Button1.Text = OpenButtonText
        Button2.Text = OpenButtonText
        Button3.Text = OpenButtonText
        Button4.Text = OpenButtonText
        Button6.Text = UnloadButtonText
        Button8.Text = LoadButtonText
        Button9.Text = LoadButtonText
        Button10.Text = LoadButtonText
        Button11.Text = LoadButtonText
        Button12.Text = LoadButtonText
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = BackColor
        TextBox2.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        Try
            DynaLog.LogMessage("Getting last key that was opened in the Registry Editor...")
            Dim LastKeyReg As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Applets\Regedit")
            PreviouslyLoadedKey = LastKeyReg.GetValue("LastKey", "")
            DynaLog.LogMessage("Last key opened: " & Quote & PreviouslyLoadedKey & Quote)
            LastKeyReg.Close()
        Catch ex As Exception
            ' Could not grab Last Key
        End Try
        DynaLog.LogMessage("Setting path of default registry hives in the image...")
        RegHiveLocation = Path.Combine(MainForm.MountDir, "Windows\system32\config")
        If RegHiveLocation <> "" Then
            OpenFileDialog1.InitialDirectory = RegHiveLocation
        End If
    End Sub

    Function ModifyRegistryHives(Load As Boolean, HiveLocation As String) As Boolean
        DynaLog.LogMessage("Modifying the state of the registry hives...")
        DynaLog.LogMessage("- Type of operation: " & If(Load, "load hive", "unload hive"))
        DynaLog.LogMessage("- Hive that will be affected by this operation: " & Path.GetFileName(HiveLocation))
        Dim regName As String = "z" & Path.GetFileNameWithoutExtension(HiveLocation)
        Dim regExitCode As Integer
        If Load Then
            If RegistryHelper.RegistryKeyExists(String.Format("HKLM\{0}", regName)) Then Return True
            regExitCode = RegistryHelper.LoadRegistryHive(HiveLocation, String.Format("HKLM\{0}", regName))
        Else
            If Not RegistryHelper.RegistryKeyExists(String.Format("HKLM\{0}", regName)) Then Return True
            regExitCode = RegistryHelper.UnloadRegistryHive(String.Format("HKLM\{0}", regName))
        End If
        DynaLog.LogMessage("The REG process finished with exit code " & Hex(regExitCode))
        If regExitCode = 0 Then
            Return True
        Else
            Debug.WriteLine("The registry process failed with exit code " & Hex(regExitCode))
            Return False
        End If
    End Function

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If ModifyRegistryHives(Button9.Text = LoadButtonText, RegHiveLocation & "\SOFTWARE") Then
            If Button9.Text = LoadButtonText Then
                Button9.Text = UnloadButtonText
                LoadedHives += 1
            Else
                Button9.Text = LoadButtonText
                LoadedHives -= 1
            End If
            Button1.Enabled = (Button9.Text <> LoadButtonText)
        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If ModifyRegistryHives(Button10.Text = LoadButtonText, RegHiveLocation & "\SYSTEM") Then
            If Button10.Text = LoadButtonText Then
                Button10.Text = UnloadButtonText
                LoadedHives += 1
            Else
                Button10.Text = LoadButtonText
                LoadedHives -= 1
            End If
            Button2.Enabled = (Button10.Text <> LoadButtonText)
        End If
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If ModifyRegistryHives(Button11.Text = LoadButtonText, RegHiveLocation & "\DEFAULT") Then
            If Button11.Text = LoadButtonText Then
                Button11.Text = UnloadButtonText
                LoadedHives += 1
            Else
                Button11.Text = LoadButtonText
                LoadedHives -= 1
            End If
            Button3.Enabled = (Button11.Text <> LoadButtonText)
        End If
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        If ModifyRegistryHives(Button12.Text = LoadButtonText, MainForm.MountDir & "\Users\Default\NTUSER.DAT") Then
            If Button12.Text = LoadButtonText Then
                Button12.Text = UnloadButtonText
                LoadedHives += 1
            Else
                Button12.Text = LoadButtonText
                LoadedHives -= 1
            End If
            Button4.Enabled = (Button12.Text <> LoadButtonText)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            DynaLog.LogMessage("Setting registry key to be opened by the Registry Editor...")
            RegistryHelper.AddRegistryItem(New RegistryItem("HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit", "LastKey", RegistryItem.ValueType.RegSz, Quote & "HKEY_LOCAL_MACHINE\zSOFTWARE" & Quote))
        Catch ex As Exception

        End Try
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\regedit.exe", "/m")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            DynaLog.LogMessage("Setting registry key to be opened by the Registry Editor...")
            RegistryHelper.AddRegistryItem(New RegistryItem("HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit", "LastKey", RegistryItem.ValueType.RegSz, Quote & "HKEY_LOCAL_MACHINE\zSYSTEM" & Quote))
        Catch ex As Exception

        End Try
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\regedit.exe", "/m")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            DynaLog.LogMessage("Setting registry key to be opened by the Registry Editor...")
            RegistryHelper.AddRegistryItem(New RegistryItem("HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit", "LastKey", RegistryItem.ValueType.RegSz, Quote & "HKEY_LOCAL_MACHINE\zDEFAULT" & Quote))
        Catch ex As Exception

        End Try
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\regedit.exe", "/m")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            DynaLog.LogMessage("Setting registry key to be opened by the Registry Editor...")
            RegistryHelper.AddRegistryItem(New RegistryItem("HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit", "LastKey", RegistryItem.ValueType.RegSz, Quote & "HKEY_LOCAL_MACHINE\zNTUSER" & Quote))
        Catch ex As Exception

        End Try
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\regedit.exe", "/m")
    End Sub

    Private Sub RegistryControlPanel_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim msg As String = ""
        msg = LocalizationService.ForSection("RegistryPanel.Close")("HivesNeedUnload.Message")
        If LoadedHives > 0 Or CustomHiveLoaded Then
            DynaLog.LogMessage("Registry hives were loaded and need to be unloaded before closing the control panel.")
            If MsgBox(msg, vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
                DynaLog.LogMessage("Unloading loaded hives...")
                If Button9.Text <> LoadButtonText Then Button9.PerformClick()
                If Button10.Text <> LoadButtonText Then Button10.PerformClick()
                If Button11.Text <> LoadButtonText Then Button11.PerformClick()
                If Button12.Text <> LoadButtonText Then Button12.PerformClick()
                If CustomHiveLoaded Then Button6.PerformClick()
            Else
                e.Cancel = True
                Exit Sub
            End If
            If LoadedHives > 0 Or CustomHiveLoaded Then
                DynaLog.LogMessage("Some hives are still loaded.")
                ' Some hives could not be unloaded
                msg = LocalizationService.ForSection("RegistryPanel.Close")("HivesNotUnloaded.Message")
                MsgBox(msg, vbOKOnly + vbCritical, Text)
                e.Cancel = True
                Exit Sub
            End If
        End If

        ' Set last key back
        If PreviouslyLoadedKey <> "" Then
            DynaLog.LogMessage("Setting last key that was opened in the Registry Editor to be the key to open...")
            Try
                RegistryHelper.AddRegistryItem(New RegistryItem("HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit", "LastKey", RegistryItem.ValueType.RegSz, Quote & PreviouslyLoadedKey & Quote))
            Catch ex As Exception
                ' Could not grab Last Key
            End Try
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        DynaLog.LogMessage("Hive to load: " & Quote & OpenFileDialog1.FileName & Quote)
        TextBox1.Text = OpenFileDialog1.FileName
        TextBox2.Text = "HKEY_LOCAL_MACHINE\z" & Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName)
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If TextBox1.Text = "" Then Exit Sub
        ' Load Custom Hive
        If ModifyRegistryHives(True, TextBox1.Text) Then
            TextBox1.Enabled = False
            Button8.Enabled = False
            Button7.Enabled = True
            Button6.Enabled = True
            Button5.Enabled = False
            CustomHiveLoaded = True
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        ' Launch regedit on the loaded Hive
        Try
            DynaLog.LogMessage("Setting registry key to be opened by the Registry Editor...")
            RegistryHelper.AddRegistryItem(New RegistryItem("HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit", "LastKey", RegistryItem.ValueType.RegSz, Quote & TextBox2.Text & Quote))
        Catch ex As Exception

        End Try
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\regedit.exe", "/m")
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        ' Unload Custom Hive
        If ModifyRegistryHives(False, TextBox1.Text) Then
            TextBox1.Enabled = True
            Button8.Enabled = True
            Button7.Enabled = False
            Button6.Enabled = False
            Button5.Enabled = True
            CustomHiveLoaded = False
        End If
    End Sub
End Class
