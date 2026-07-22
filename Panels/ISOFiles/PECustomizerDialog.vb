Imports System.Windows.Forms
Imports Microsoft.Win32
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class PECustomizerDialog

    Private KeyboardLayoutDictionary As New Dictionary(Of String, String)
    Private SelectedKeyboardLayoutCode As String

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
        Dim AnswerFileConflictResponse As String = ""
        Select Case ComboBox3.SelectedIndex
            Case 0
                AnswerFileConflictResponse = "AskUser"
            Case 1
                AnswerFileConflictResponse = "PreferISO"
            Case 2
                AnswerFileConflictResponse = "PreferWIM"
        End Select

        ' Control the selected layout; if it is invalid then fall back to US
        If SelectedKeyboardLayoutCode = "" OrElse Not KeyboardLayoutDictionary.ContainsKey(SelectedKeyboardLayoutCode) Then
            SelectedKeyboardLayoutCode = "00000409"
        End If

        Dim regContents As String = String.Format("Windows Registry Editor Version 5.00{0}{0}" &
                                                  "[HKEY_LOCAL_MACHINE\WINPESOFT\DISMTools\Preinstallation Environment\Policies]{0}" &
                                                  "{1}ShowWatermark{1}=dword:0000000{2}{0}" &
                                                  "{1}UEFICA23Preference{1}={1}{3}{1}{0}" &
                                                  "{1}PartTableOverridePreference{1}={1}{4}{1}{0}" &
                                                  "{1}WDSHCConnAttempts{1}=dword:{5}{0}" &
                                                  "{1}WDSHCGraphoView{1}=dword:0000000{6}{0}" &
                                                  "{1}DTDimShowPnputilOut{1}=dword:0000000{7}{0}" &
                                                  "{1}AutoUnattendCopytoSysprep{1}=dword:0000000{8}{0}" &
                                                  "{1}PXEServerPort{1}=dword:{9}{0}" &
                                                  "{1}KeyboardLayoutCode{1}={1}{10}{1}{0}" &
                                                  "{1}KeyboardLayoutOverrideExistingLayout{1}=dword:0000000{11}{0}" &
                                                  "{1}AnswerFileConflictResponse{1}={1}{12}{1}{0}",
                                                  CrLf, Quote, If(CheckBox2.Checked, 1, 0), UEFICA23Preference, PartTableOverridePreference,
                                                  Hex(NumericUpDown1.Value).PadLeft(8, "0"c).ToLowerInvariant(), If(CheckBox3.Checked, 1, 0), If(CheckBox4.Checked, 1, 0),
                                                  If(CheckBox5.Checked, 1, 0), Hex(NumericUpDown2.Value).PadLeft(8, "0"c).ToLowerInvariant(), SelectedKeyboardLayoutCode, If(CheckBox6.Checked, 1, 0), AnswerFileConflictResponse)
        Try
            File.WriteAllText(Path.Combine(Application.StartupPath, "bin", "extps1", "PE_Helper", "files", "CustomPolicy.reg"), regContents)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Private Function SaveDefaultPolicies() As Boolean
        Try
            ' First let's get the wallpaper out of the way
            Try
                If TextBox1.Text <> "" AndAlso File.Exists(TextBox1.Text) Then
                    File.Copy(TextBox1.Text, Path.Combine(Application.StartupPath, "bin", "extps1", "PE_Helper", "backgrounds", "wallpaper.jpg"), True)
                    File.Copy(TextBox1.Text, Path.Combine(Application.StartupPath, "userdata", "dtpe_backgrounds", "wallpaper.jpg"), True)
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
            Dim AnswerFileConflictResponse As String = ""
            Select Case ComboBox3.SelectedIndex
                Case 0
                    AnswerFileConflictResponse = "AskUser"
                Case 1
                    AnswerFileConflictResponse = "PreferISO"
                Case 2
                    AnswerFileConflictResponse = "PreferWIM"
            End Select

            ' Control the selected layout; if it is invalid then fall back to US
            If SelectedKeyboardLayoutCode = "" OrElse Not KeyboardLayoutDictionary.ContainsKey(SelectedKeyboardLayoutCode) Then
                SelectedKeyboardLayoutCode = "00000409"
            End If

            MainForm.ShowWatermark = CheckBox2.Checked
            MainForm.UEFICA23Preference = ComboBox2.SelectedIndex
            MainForm.PartTableOverridePreference = ComboBox1.SelectedIndex
            MainForm.WDSHCConnAttempts = NumericUpDown1.Value
            MainForm.WDSHCGraphoView = CheckBox3.Checked
            MainForm.DTDimShowPnputilOut = CheckBox4.Checked
            MainForm.AutoUnattendCopytoSysprep = CheckBox5.Checked
            MainForm.PXEServerPort = NumericUpDown2.Value
            MainForm.KeyboardLayoutCode = SelectedKeyboardLayoutCode
            MainForm.KeyboardLayoutOverrideExistingLayout = CheckBox6.Checked
            MainForm.AnswerFileConflictResponse = ComboBox3.SelectedIndex
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not SavePolicies() Then
            MessageBox.Show(Me, LocalizationService.ForSection("ISOFiles.PECustomizer")("PoliciesSaved.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
        CheckBox5.Checked = MainForm.AutoUnattendCopytoSysprep
        CheckBox6.Checked = MainForm.KeyboardLayoutOverrideExistingLayout
        ComboBox1.SelectedIndex = MainForm.PartTableOverridePreference
        ComboBox2.SelectedIndex = MainForm.UEFICA23Preference
        ComboBox3.SelectedIndex = MainForm.AnswerFileConflictResponse
        NumericUpDown1.Value = MainForm.WDSHCConnAttempts
        NumericUpDown2.Value = MainForm.PXEServerPort

        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox2.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox3.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown1.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown2.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        TabPage1.BackColor = CurrentTheme.SectionBackgroundColor
        TabPage2.BackColor = CurrentTheme.SectionBackgroundColor
        TabPage3.BackColor = CurrentTheme.SectionBackgroundColor
        TabPage4.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        ComboBox2.ForeColor = ForeColor
        ComboBox3.ForeColor = ForeColor
        NumericUpDown1.ForeColor = ForeColor
        NumericUpDown2.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        TabPage1.ForeColor = ForeColor
        TabPage2.ForeColor = ForeColor
        TabPage3.ForeColor = ForeColor
        TabPage4.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ColumnHeader1.Width = WindowHelper.ScaleLogical(96)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(384)

        ListView1.Items.Clear()

        ' Get the keyboard layouts
        Dim LayoutRk As RegistryKey = Nothing
        Try
            LayoutRk = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Control\Keyboard Layouts", False)
            For Each LayoutCode In LayoutRk.GetSubKeyNames()
                Dim LayoutNameRk As RegistryKey = LayoutRk.OpenSubKey(LayoutCode, False)
                Dim LayoutName As String = LayoutNameRk.GetValue("Layout Text", "")
                LayoutNameRk.Close()

                If Not KeyboardLayoutDictionary.ContainsKey(LayoutCode) Then
                    KeyboardLayoutDictionary.Add(LayoutCode, LayoutName)
                Else
                    KeyboardLayoutDictionary(LayoutCode) = LayoutName
                End If
            Next
            ListView1.Items.AddRange(KeyboardLayoutDictionary.ToList().Select(Function(kvp) New ListViewItem(New String() {kvp.Key, kvp.Value})).ToArray())

            Dim DefaultLayoutIndex As Integer = KeyboardLayoutDictionary.Keys.ToList().IndexOf(MainForm.KeyboardLayoutCode)
            If DefaultLayoutIndex > -1 Then
                ListView1.Items(DefaultLayoutIndex).Selected = True
                ListView1.Select()
            End If
            TextBox2.Text = MainForm.KeyboardLayoutCode
        Catch ex As Exception

        Finally
            If LayoutRk IsNot Nothing Then LayoutRk.Close()
        End Try
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Button1.Enabled = Not CheckBox1.Checked
        If Not CheckBox1.Checked Then Exit Sub

        Dim WallpaperPath As String = ""
        ' Wallpaper may be defined by group policy; check there first
        Try
            Dim WallpaperPolicyRk As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Policies\System")
            WallpaperPath = WallpaperPolicyRk.GetValue("Wallpaper", "")
            WallpaperPolicyRk.Close()
            If WallpaperPath = "" OrElse Not File.Exists(WallpaperPath) Then Throw New Exception()
        Catch ex As Exception
            ' Ignore and use general wallpaper
            Dim WallpaperRk As RegistryKey = Registry.CurrentUser.OpenSubKey("Control Panel\Desktop", False)
            WallpaperPath = WallpaperRk.GetValue("WallPaper", "")
            WallpaperRk.Close()
        End Try
        TextBox1.Text = WallpaperPath
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" Then
            If Not File.Exists(TextBox1.Text) Then
                MessageBox.Show(Me, LocalizationService.ForSection("ISOFiles.PECustomizer")("Wallpaper.Exist.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                TextBox1.Text = ""
                Exit Sub
            End If

            If Not Path.GetExtension(TextBox1.Text).Equals(".jpg", StringComparison.OrdinalIgnoreCase) Then
                MessageBox.Show(Me, LocalizationService.ForSection("ISOFiles.PECustomizer")("Wallpaper.Supported.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                TextBox1.Text = ""
                Exit Sub
            End If

            MessageBox.Show(Me, LocalizationService.ForSection("ISOFiles.PECustomizer")("WallpaperOverride.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            TextBox2.Text = If(ListView1.SelectedItems.Count = 1, ListView1.FocusedItem.Text, "")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        SelectedKeyboardLayoutCode = TextBox2.Text
    End Sub

    Private Sub DefaultPolicySaveButton_MouseHover(sender As Object, e As EventArgs) Handles DefaultPolicySaveButton.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("PECustomizer.Tooltips")("DefaultPolicies.Message"))
    End Sub

    Private Sub DefaultPolicySaveButton_Click(sender As Object, e As EventArgs) Handles DefaultPolicySaveButton.Click
        If SaveDefaultPolicies() Then
            MainForm.WriteDefaultPEPolicy()
            MessageBox.Show(LocalizationService.ForSection("PECustomizer.Messages")("Default.Policies.Saved.Label"), Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show(LocalizationService.ForSection("PECustomizer.Messages")("Policies.SaveFailed.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        Label13.Visible = ComboBox3.SelectedIndex > 0
    End Sub
End Class
