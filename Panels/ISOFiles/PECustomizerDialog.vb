Imports System.Windows.Forms
Imports Microsoft.Win32
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class PECustomizerDialog

    Private Function SavePolicies() As Boolean
        ' First let's get the wallpaper out of the way
        Try
            If TextBox1.Text <> "" AndAlso File.Exists(TextBox1.Text) Then
                File.Copy(TextBox1.Text, Path.Combine(Application.StartupPath, "bin", "extps1", "PE_Helper", "backgrounds", "wallpaper.jpg"), True)
            End If
        Catch ex As Exception

        End Try

        ' Now let's deal with the policies
        Dim PartTableOverridePreference As String = ""
        Select Case ComboBox1.SelectedIndex
            Case 0
                PartTableOverridePreference = "NoOverride"
            Case 1
                PartTableOverridePreference = "AlwaysMBR"
            Case 2
                PartTableOverridePreference = "AlwaysGPT"
        End Select
        Dim UEFICA23Preference As String = ""
        Select Case ComboBox2.SelectedIndex
            Case 0
                UEFICA23Preference = "AskUser"
            Case 1
                UEFICA23Preference = "UseNever"
            Case 2
                UEFICA23Preference = "UseAlways"
        End Select

        Dim regContents As String = String.Format("Windows Registry Editor Version 5.00{0}{0}" &
                                                  "[HKEY_LOCAL_MACHINE\WINPESOFT\DISMTools\Preinstallation Environment\Policies]{0}" &
                                                  "{1}ShowWatermark{1}=dword:0000000{2}{0}" &
                                                  "{1}UEFICA23Preference{1}={1}{3}{1}{0}" &
                                                  "{1}PartTableOverridePreference{1}={1}{4}{1}{0}" &
                                                  "{1}WDSHCConnAttempts{1}=dword:{5}{0}" &
                                                  "{1}WDSHCGraphoView{1}=dword:0000000{6}{0}" &
                                                  "{1}DTDimShowPnputilOut{1}=dword:0000000{7}{0}{0}",
                                                  CrLf, Quote, If(CheckBox2.Checked, 1, 0), UEFICA23Preference, PartTableOverridePreference,
                                                  Hex(NumericUpDown1.Value).PadLeft(8, "0"c).ToLowerInvariant(), If(CheckBox3.Checked, 1, 0), If(CheckBox4.Checked, 1, 0))
        Try
            File.WriteAllText(Path.Combine(Application.StartupPath, "bin", "extps1", "PE_Helper", "files", "CustomPolicy.reg"), regContents)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not SavePolicies() Then
            MessageBox.Show(Me, "Policies could not be saved.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub PECustomizerDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load from defined policies
        CheckBox2.Checked = MainForm.ShowWatermark
        CheckBox3.Checked = MainForm.WDSHCGraphoView
        CheckBox4.Checked = MainForm.DTDimShowPnputilOut
        ComboBox1.SelectedIndex = MainForm.PartTableOverridePreference
        ComboBox2.SelectedIndex = MainForm.UEFICA23Preference
        NumericUpDown1.Value = MainForm.WDSHCConnAttempts

        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox2.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        ComboBox2.ForeColor = ForeColor
        NumericUpDown1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Button1.Enabled = Not CheckBox1.Checked
        If Not CheckBox1.Checked Then Exit Sub
        Try
            Dim wallpaperRk As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\Desktop", False)
            TextBox1.Text = wallpaperRk.GetValue("WallPaper", "")
            wallpaperRk.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" Then
            If Not File.Exists(TextBox1.Text) Then
                MessageBox.Show(Me, "The specified wallpaper does not exist.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                TextBox1.Text = ""
                Exit Sub
            End If

            If Not Path.GetExtension(TextBox1.Text).Equals(".jpg", StringComparison.OrdinalIgnoreCase) Then
                MessageBox.Show(Me, "The specified wallpaper is not supported. Only JPG files are supported.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                TextBox1.Text = ""
                Exit Sub
            End If

            MessageBox.Show(Me, "By continuing with this wallpaper you will be overriding a background you may have already stored in your user data folder. That background will be reused the next time you launch DISMTools.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub
End Class
