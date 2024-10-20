Imports System.IO
Imports Microsoft.Win32
Imports Microsoft.VisualBasic.ControlChars

Public Class RegistryControlPanel

    Dim PreviouslyLoadedKey As String       ' The previous key regedit had opened
    Dim RegHiveLocation As String

    Dim LoadButtonText As String = "Load"
    Dim UnloadButtonText As String = "Unload"

    Dim LoadedHives As Integer = 0

    Dim CustomHiveLoaded As Boolean

    Private Sub RegistryControlPanel_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Language translations will be added later
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        TextBox1.BackColor = BackColor
        TextBox2.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        Try
            Dim LastKeyReg As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Applets\Regedit")
            PreviouslyLoadedKey = LastKeyReg.GetValue("LastKey")
            LastKeyReg.Close()
        Catch ex As Exception
            ' Could not grab Last Key
        End Try
        RegHiveLocation = Path.Combine(MainForm.MountDir, "Windows\system32\config")
        If RegHiveLocation <> "" Then
            OpenFileDialog1.InitialDirectory = RegHiveLocation
        End If
    End Sub

    Function ModifyRegistryHives(Load As Boolean, HiveLocation As String) As Boolean
        Dim regLoaderProc As New Process()
        regLoaderProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe"
        Dim regName As String = "z" & Path.GetFileNameWithoutExtension(HiveLocation)
        If Load Then
            regLoaderProc.StartInfo.Arguments = "load HKLM\" & regName & " " & Quote & HiveLocation & Quote
        Else
            regLoaderProc.StartInfo.Arguments = "unload HKLM\" & regName
        End If
        regLoaderProc.StartInfo.CreateNoWindow = True
        regLoaderProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        regLoaderProc.Start()
        regLoaderProc.WaitForExit()
        If regLoaderProc.ExitCode = 0 Then
            Return True
        Else
            MsgBox("The registry process failed with exit code " & Hex(regLoaderProc.ExitCode), vbOKOnly + vbCritical, Text)
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
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe", "add HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit /v LastKey /t REG_SZ /d " & Quote & "HKEY_LOCAL_MACHINE\zSOFTWARE" & Quote & " /f").WaitForExit()
        Catch ex As Exception

        End Try
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\regedit.exe", "/m")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe", "add HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit /v LastKey /t REG_SZ /d " & Quote & "HKEY_LOCAL_MACHINE\zSYSTEM" & Quote & " /f").WaitForExit()
        Catch ex As Exception

        End Try
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\regedit.exe", "/m")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe", "add HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit /v LastKey /t REG_SZ /d " & Quote & "HKEY_LOCAL_MACHINE\zDEFAULT" & Quote & " /f").WaitForExit()
        Catch ex As Exception

        End Try
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\regedit.exe", "/m")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe", "add HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit /v LastKey /t REG_SZ /d " & Quote & "HKEY_LOCAL_MACHINE\zNTUSER" & Quote & " /f").WaitForExit()
        Catch ex As Exception

        End Try
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\regedit.exe", "/m")
    End Sub

    Private Sub RegistryControlPanel_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim msg As String = "The registry hives need to be unloaded to close this window. Do you want to unload them now?"
        If LoadedHives > 0 Or CustomHiveLoaded Then
            If MsgBox(msg, vbYesNo + vbQuestion, Text) = MsgBoxResult.Yes Then
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
                ' Some hives could not be unloaded
                MsgBox("Some hives could not be unloaded. Please unload them before closing this window.", vbOKOnly + vbCritical, Text)
                e.Cancel = True
                Exit Sub
            End If
        End If

        ' Set last key back
        If PreviouslyLoadedKey <> "" Then
            Try
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe", "add HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit /v LastKey /t REG_SZ /d " & Quote & PreviouslyLoadedKey & Quote & " /f")
            Catch ex As Exception
                ' Could not grab Last Key
            End Try
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
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
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe", "add HKCU\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit /v LastKey /t REG_SZ /d " & Quote & TextBox2.Text & Quote & " /f").WaitForExit()
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