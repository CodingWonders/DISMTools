Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Win32
Imports DT_ThemeDesigner.Classes.ColorUtilities

Public Class MainForm

    Public NewTheme As Theme

    Private UserDataScriptFolder As String

    Public CurrentColorMode As ColorThemeMode

    Private roMode As Boolean
    Private SavedThemePath As String

    Private Sub ChangeMenuItemColors(ByVal bgColor As Color, ByVal fgColor As Color, ByVal itemCollection As ToolStripItemCollection)
        For Each tsi As ToolStripItem In itemCollection
            If TypeOf tsi Is ToolStripDropDownItem Then
                Dim item As ToolStripDropDownItem = CType(tsi, ToolStripDropDownItem)
                Try
                    item.DropDown.BackColor = bgColor
                    item.DropDown.ForeColor = fgColor
                    If item.DropDownItems.Count > 0 Then
                        ChangeMenuItemColors(bgColor, fgColor, item.DropDownItems)
                    End If
                Catch ex As Exception
                    Continue For
                End Try
            End If
        Next
    End Sub

    Private Sub SetColorMode(ByVal NewColorMode As ColorThemeMode)
        CurrentColorMode = NewColorMode
        Select Case NewColorMode
            Case ColorThemeMode.Light
                WindowHelper.ToggleDarkTitleBar(Handle, False)

                BackColor = Color.FromArgb(239, 239, 242)
                ForeColor = Color.Black
            Case ColorThemeMode.Dark
                WindowHelper.ToggleDarkTitleBar(Handle, True)

                BackColor = Color.FromArgb(32, 32, 32)
                ForeColor = Color.White
            Case ColorThemeMode.System
                If Environment.OSVersion.Version.Major < 10 Then SetColorMode(ColorThemeMode.Light)

                Try
                    Dim darkMode As Boolean
                    Dim ColorModeRk As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", False)
                    darkMode = ColorModeRk.GetValue("AppsUseLightTheme", 1) = 0
                    ColorModeRk.Close()

                    If darkMode Then SetColorMode(ColorThemeMode.Dark) Else SetColorMode(ColorThemeMode.Light)
                Catch ex As Exception
                    SetColorMode(ColorThemeMode.Light)
                End Try

                Exit Sub
        End Select

        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor

        If NewColorMode = ColorThemeMode.Light Then
            ToolStrip1.Renderer = New LightModeRenderer()
        ElseIf NewColorMode = ColorThemeMode.Dark Then
            ToolStrip1.Renderer = New DarkModeRenderer()
        End If
        ChangeMenuItemColors(BackColor, ForeColor, ColorModeTSDDB.DropDownItems)
    End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SystemCM_TSMI.Enabled = Environment.OSVersion.Version.Major >= 10

        SetColorMode(ColorThemeMode.System)
        GetArguments()
        SaveFileDialog1.InitialDirectory = UserDataScriptFolder
        NewTheme = GetNewTheme()
        ChangeColorPreviews()
        LoadCurrentTheme()
    End Sub

    Private Sub GetArguments()
        Dim args As String() = Environment.GetCommandLineArgs()
        If args.Length <= 1 Then Exit Sub

        For Each arg As String In args
            If arg.StartsWith("/userdata", StringComparison.OrdinalIgnoreCase) Then
                ' This parameter determines the path to a DT UserData folder.
                Dim userDataPath As String = arg.Replace("/userdata=", "")

                If Directory.Exists(userDataPath) Then
                    UserDataScriptFolder = userDataPath
                End If
            End If
        Next
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        NewTheme.Name = TextBox1.Text
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        NewTheme.IsDark = CheckBox1.Checked
        LoadCurrentTheme()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ColorDialog1.Color = NewTheme.BackgroundColor
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NewTheme.BackgroundColor = ColorDialog1.Color
            ChangeColorPreviews()
            LoadCurrentTheme()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ColorDialog1.Color = NewTheme.SectionBackgroundColor
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NewTheme.SectionBackgroundColor = ColorDialog1.Color
            ChangeColorPreviews()
            LoadCurrentTheme()
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ColorDialog1.Color = NewTheme.ForegroundColor
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NewTheme.ForegroundColor = ColorDialog1.Color
            ChangeColorPreviews()
            LoadCurrentTheme()
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        ColorDialog1.Color = NewTheme.AccentColors(0)
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NewTheme.AccentColors(0) = ColorDialog1.Color
            ChangeColorPreviews()
            LoadCurrentTheme()
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        ColorDialog1.Color = NewTheme.AccentColors(1)
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NewTheme.AccentColors(1) = ColorDialog1.Color
            ChangeColorPreviews()
            LoadCurrentTheme()
        End If
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        ColorDialog1.Color = NewTheme.AccentColors(2)
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NewTheme.AccentColors(2) = ColorDialog1.Color
            ChangeColorPreviews()
            LoadCurrentTheme()
        End If
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        ColorDialog1.Color = NewTheme.AccentColors(3)
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NewTheme.AccentColors(3) = ColorDialog1.Color
            ChangeColorPreviews()
            LoadCurrentTheme()
        End If
    End Sub

    Private Sub ChangeColorPreviews()
        Label7.BackColor = NewTheme.BackgroundColor
        Label8.BackColor = NewTheme.ForegroundColor
        Label9.BackColor = NewTheme.SectionBackgroundColor
        Label10.BackColor = NewTheme.AccentColors(0)
        Label11.BackColor = NewTheme.AccentColors(1)
        Label12.BackColor = NewTheme.AccentColors(2)
        Label13.BackColor = NewTheme.AccentColors(3)
    End Sub

    Private Sub LoadCurrentTheme()
        NewTheme.DisabledForegroundColor = ThemeHelper.GetDisabledForegroundColor(NewTheme)

        ThemePreviewPanel.BackColor = NewTheme.BackgroundColor
        TestSection.BackColor = NewTheme.SectionBackgroundColor
        ThemePreviewPanel.ForeColor = NewTheme.ForegroundColor
        InactiveLabel.ForeColor = NewTheme.DisabledForegroundColor
        AccentedLabel1.BackColor = NewTheme.AccentColors(0)
        AccentedLabel2.BackColor = NewTheme.AccentColors(1)
        AccentedLabel3.BackColor = NewTheme.AccentColors(2)
        AccentedLabel4.BackColor = NewTheme.AccentColors(3)
        TextBox2.BackColor = NewTheme.BackgroundColor
        TextBox2.ForeColor = NewTheme.ForegroundColor

        TestGlyph1.Image = ThemeHelper.GetGlyphResource("newfile", True, NewTheme)
        TestGlyph2.Image = ThemeHelper.GetGlyphResource("openfile", True, NewTheme)
        TestGlyph3.Image = ThemeHelper.GetGlyphResource("info_glyph", True, NewTheme)
        TestGlyph4.Image = ThemeHelper.GetGlyphResource("save_glyph", True, NewTheme)
    End Sub

    Private Sub OpenFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        SavedThemePath = OpenFileDialog1.FileName
        NewTheme = ThemeHelper.LoadThemeFile(SavedThemePath)
        roMode = False
        TextBox1.Text = NewTheme.Name
        CheckBox1.Checked = NewTheme.IsDark
        ChangeColorPreviews()
        LoadCurrentTheme()
        Text = String.Format(LocalizationService.ForSection("ThemeDesigner.Main")("Window.Title"), Path.GetFileName(SavedThemePath))
        If (File.GetAttributes(SavedThemePath) And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
            MessageBox.Show(LocalizationService.ForSection("ThemeDesigner.Messages")("Loaded.Read.Only.Message"), LocalizationService.ForSection("ThemeDesigner.Messages")("ThemeDesigner.Label"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            roMode = True
            ToolStripButton5.Enabled = True
        End If
        TextBox1.Select(TextBox1.TextLength, 0)
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub Label7_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label9.MouseHover, Label8.MouseHover, Label7.MouseHover, Label13.MouseHover, Label12.MouseHover, Label11.MouseHover, Label10.MouseHover
        Try
            Dim BackgroundColor As Color = CType(sender, Label).BackColor
            CurrentColorTT.SetToolTip(sender, LocalizationService.ForSection("Tools.ThemeDesigner.Main").Format("Color.Rgbclick.Label", BackgroundColor.R, BackgroundColor.G, BackgroundColor.B))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label9.Click, Label8.Click, Label7.Click, Label13.Click, Label12.Click, Label11.Click, Label10.Click
        Try
            Dim BackgroundColor As Color = CType(sender, Label).BackColor
            My.Computer.Clipboard.SetText(String.Format("RGB({0}, {1}, {2})", BackgroundColor.R, BackgroundColor.G, BackgroundColor.B))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        If String.IsNullOrEmpty(NewTheme.Name) Then
            MessageBox.Show(LocalizationService.ForSection("ThemeDesigner.Messages")("Provide.Name.Label"), LocalizationService.ForSection("ThemeDesigner.Messages")("Name.Missing.Label"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        SaveFileDialog1.ShowDialog()
    End Sub

    Private Sub SaveFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        Cursor = Cursors.WaitCursor
        If ThemeHelper.SaveTheme(NewTheme, SaveFileDialog1.FileName) Then
            MessageBox.Show(LocalizationService.ForSection("ThemeDesigner.Messages")("Saved.Done.Label"), LocalizationService.ForSection("ThemeDesigner.Messages")("SaveSuccess.Label"), MessageBoxButtons.OK, MessageBoxIcon.Information)
            SavedThemePath = SaveFileDialog1.FileName
            Text = String.Format(LocalizationService.ForSection("ThemeDesigner.Main")("Window.Title"), Path.GetFileName(SavedThemePath))
            roMode = False
        Else
            MessageBox.Show(LocalizationService.ForSection("ThemeDesigner.Messages")("SaveTheme.Label"), LocalizationService.ForSection("ThemeDesigner.Messages")("SaveError.Label"), MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        Cursor = Cursors.Arrow
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        NewTheme = ThemeHelper.GetNewTheme()
        TextBox1.Text = NewTheme.Name
        CheckBox1.Checked = NewTheme.IsDark
        ChangeColorPreviews()
        LoadCurrentTheme()
        roMode = False
        SavedThemePath = ""
        Text = LocalizationService.ForSection("ThemeDesigner.Main")("Window.DefaultTitle")
    End Sub

    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
#If VBC_VER >= 9.0 Then
        MsgBox(LocalizationService.ForSection("ThemeDesigner.Messages").Format("DISM.Tools.Designer.Label", My.Application.Info.Version.ToString() & "_" & RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm"), My.Application.Info.Copyright, "INI File Parser:  2008 Ricardo Amores Hernndez"), vbOKOnly + vbInformation, LocalizationService.ForSection("ThemeDesigner.Messages")("About.Label"))
#Else
        MsgBox(LocalizationService.ForSection("ThemeDesigner.Messages").Format("About.Version.Message", My.Application.Info.Version.ToString(), My.Application.Info.Copyright, "INI File Parser:  2008 Ricardo Amores Hernndez"), vbOKOnly + vbInformation, LocalizationService.ForSection("ThemeDesigner.Messages")("About.Label"))
#End If
    End Sub

    Private Sub LightCM_TSMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LightCM_TSMI.Click
        SetColorMode(ColorThemeMode.Light)
    End Sub

    Private Sub DarkCM_TSMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DarkCM_TSMI.Click
        SetColorMode(ColorThemeMode.Dark)
    End Sub

    Private Sub SystemCM_TSMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SystemCM_TSMI.Click
        SetColorMode(ColorThemeMode.System)
    End Sub

    Private Sub EnableWriteAccess()
        If SavedThemePath = "" OrElse Not File.Exists(SavedThemePath) Then Exit Sub
        Try
            File.SetAttributes(SavedThemePath, (File.GetAttributes(SavedThemePath) And Not FileAttributes.ReadOnly))
            roMode = False
            ToolStripButton5.Enabled = False
        Catch ex As Exception
            MessageBox.Show(LocalizationService.ForSection("ThemeDesigner.Messages")("Enable.Write.Access.Message"), LocalizationService.ForSection("ThemeDesigner.Messages")("StarterScript.Editor.Label"), MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ToolStripButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton5.Click
        EnableWriteAccess()
    End Sub
End Class
