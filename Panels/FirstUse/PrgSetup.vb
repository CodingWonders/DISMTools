Imports System.Drawing.Drawing2D
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Net

Public Class PrgSetup

    Dim ColorModes() As String = New String(2) {String.Empty, String.Empty, String.Empty}
    
    Dim btnToolTip As New ToolTip()
    Private isMouseDown As Boolean = False
    Private mouseOffset As Point
    Dim pageInt As Integer = 0
    Private isApplyingLocalizedText As Boolean = False

    Private Sub minBox_MouseEnter(sender As Object, e As EventArgs) Handles minBox.MouseEnter
        minBox.Image = My.Resources.minBox_focus
    End Sub

    Private Sub minBox_MouseLeave(sender As Object, e As EventArgs) Handles minBox.MouseLeave
        minBox.Image = My.Resources.minBox
    End Sub

    Private Sub minBox_MouseDown(sender As Object, e As MouseEventArgs) Handles minBox.MouseDown
        minBox.Image = My.Resources.minBox_down
    End Sub

    Private Sub minBox_MouseUp(sender As Object, e As MouseEventArgs) Handles minBox.MouseUp
        minBox.Image = My.Resources.minBox_focus
    End Sub

    Private Sub minBox_MouseHover(sender As Object, e As EventArgs) Handles minBox.MouseHover
        Dim msg As String = LocalizationService.ForSection("PrgSetup.ToolTip")("Minimize.Label")
        btnToolTip.SetToolTip(sender, msg)
    End Sub

    Private Sub minBox_Click(sender As Object, e As EventArgs) Handles minBox.Click
        WindowState = FormWindowState.Minimized
    End Sub

    Private Sub closeBox_MouseEnter(sender As Object, e As EventArgs) Handles closeBox.MouseEnter
        closeBox.Image = My.Resources.closebox_focus
    End Sub

    Private Sub closeBox_MouseLeave(sender As Object, e As EventArgs) Handles closeBox.MouseLeave
        closeBox.Image = My.Resources.closebox
    End Sub

    Private Sub closeBox_MouseDown(sender As Object, e As MouseEventArgs) Handles closeBox.MouseDown
        closeBox.Image = My.Resources.closebox_down
    End Sub

    Private Sub closeBox_MouseUp(sender As Object, e As MouseEventArgs) Handles closeBox.MouseUp
        closeBox.Image = My.Resources.closebox_focus
    End Sub

    Private Sub closeBox_MouseHover(sender As Object, e As EventArgs) Handles closeBox.MouseHover
        Dim msg As String = LocalizationService.ForSection("PrgSetup.ToolTip")("Close.Label")
        btnToolTip.SetToolTip(sender, msg)
    End Sub

    Private Sub closeBox_Click(sender As Object, e As EventArgs) Handles closeBox.Click
        IncompleteSetupDlg.ShowDialog(Me)
        If IncompleteSetupDlg.DialogResult = Windows.Forms.DialogResult.OK Then
            Close()
        End If
    End Sub

    Private Sub backBox_MouseEnter(sender As Object, e As EventArgs) Handles backBox.MouseEnter
        backBox.Image = My.Resources.backbox_focus
    End Sub

    Private Sub backBox_MouseLeave(sender As Object, e As EventArgs) Handles backBox.MouseLeave
        backBox.Image = My.Resources.backbox
    End Sub

    Private Sub backBox_MouseDown(sender As Object, e As MouseEventArgs) Handles backBox.MouseDown
        backBox.Image = My.Resources.backbox_down
    End Sub

    Private Sub backBox_MouseUp(sender As Object, e As MouseEventArgs) Handles backBox.MouseUp
        backBox.Image = My.Resources.backbox_focus
    End Sub

    Private Sub backBox_MouseHover(sender As Object, e As EventArgs) Handles backBox.MouseHover
        Dim msg As String = LocalizationService.ForSection("PrgSetup.ToolTip")("GoBack.Label")
        btnToolTip.SetToolTip(sender, msg)
    End Sub

    Private Sub wndControlPanel_MouseDown(sender As Object, e As MouseEventArgs) Handles wndControlPanel.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            ' Get the new position
            mouseOffset = New Point(-e.X, -e.Y)
            ' Set that left button is pressed
            isMouseDown = True
        End If
    End Sub

    Private Sub wndControlPanel_MouseMove(sender As Object, e As MouseEventArgs) Handles wndControlPanel.MouseMove
        If isMouseDown Then
            Dim mousePos As Point = Control.MousePosition
            ' Get the new form position
            mousePos.Offset(mouseOffset.X, mouseOffset.Y)
            Location = mousePos
        End If
    End Sub

    Private Sub wndControlPanel_MouseUp(sender As Object, e As MouseEventArgs) Handles wndControlPanel.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            isMouseDown = False
        End If
    End Sub

    Private Sub Next_Button_Click(sender As Object, e As EventArgs) Handles Next_Button.Click
        If pageInt = 4 Then
            MainForm.LanguageCode = LocalizationService.NormalizeCultureCode(MainForm.LanguageCode)
            MainForm.SaveDTSettings()
            Close()
        End If
        pageInt += 1
        Select Case pageInt
            Case 0
                WelcomePanel.Visible = True
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 1
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = True
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 2
                MainForm.ColorMode = ComboBox1.SelectedIndex
                MainForm.LanguageCode = GetSelectedLanguageCode(ComboBox2, MainForm.LanguageCode)
                MainForm.LogFont = ComboBox3.SelectedItem
                MainForm.LogFontSize = NumericUpDown1.Value
                MainForm.LogFontIsBold = Toggle1.Checked
                MainForm.ProgressPanelStyle = If(RadioButton1.Checked, 1, 0)
                MainForm.ProjectView.Visible = True
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = True
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 3
                MainForm.AutoLogs = CheckBox1.Checked
                If Not CheckBox1.Checked And Not Directory.Exists(Path.GetDirectoryName(TextBox2.Text)) Then
                    Dim msg As String = LocalizationService.ForSection("PrgSetup.Next.Actions")("Folder.Log.File.Message")
                    MsgBox(msg, vbOKOnly + vbCritical, Text)
                    Exit Sub
                End If
                MainForm.LogFile = TextBox2.Text
                MainForm.LogLevel = TrackBar1.Value + 1
                pageInt += 1
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = True
            Case 4
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = True
        End Select
        If pageInt = 4 Then
            Next_Button.Text = LocalizationService.ForSection("PrgSetup.Next")("Finish.Label")
            Cancel_Button.Enabled = False
            closeBox.Enabled = False
        Else
            Next_Button.Text = LocalizationService.ForSection("PrgSetup.Next.Actions")("Next.Button")
            Cancel_Button.Enabled = True
            closeBox.Enabled = True
        End If
        If pageInt = 0 Then
            Back_Button.Enabled = False
            backBox.Visible = False
            Label1.Left = WindowHelper.ScaleLogical(8)
        Else
            Back_Button.Enabled = True
            backBox.Visible = True
            Label1.Left = WindowHelper.ScaleLogical(54)
        End If
    End Sub

    Private Sub Back_Button_Click(sender As Object, e As EventArgs) Handles Back_Button.Click, backBox.Click
        pageInt -= 1
        Select Case pageInt
            Case 0
                WelcomePanel.Visible = True
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 1
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = True
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 2
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = True
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 3
                ' Same
                'WelcomePanel.Visible = False
                'CustomizationPanel.Visible = False
                'LogsPanel.Visible = False
                'ModulesPanel.Visible = True
                'FinishPanel.Visible = False
                pageInt -= 1
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = True
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 4
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = True
        End Select
        If pageInt = 4 Then
            Next_Button.Text = LocalizationService.ForSection("PrgSetup.Next")("Finish.Label")
            Cancel_Button.Enabled = False
            closeBox.Enabled = False
        Else
            Next_Button.Text = LocalizationService.ForSection("PrgSetup.Next.Actions")("Next.Button")
            Cancel_Button.Enabled = True
            closeBox.Enabled = True
        End If
        If pageInt = 0 Then
            Back_Button.Enabled = False
            backBox.Visible = False
            Label1.Left = WindowHelper.ScaleLogical(8)
        Else
            Back_Button.Enabled = True
            backBox.Visible = True
            Label1.Left = WindowHelper.ScaleLogical(54)
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ' MainForm.SaveDTSettings()
        Options.PrefReset.Enabled = False
        Options.ShowDialog(Me)
    End Sub

    Private Sub PrgSetup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Generate new settings file and load it
        MainForm.GenerateDTSettings()
        MainForm.LoadDTSettings(1)
        GetSystemFonts()
        TextBox2.Text = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\Logs\DISM\DISM.log"
        MainForm.LogFile = TextBox2.Text

        MainForm.IsFirstTime = True

        ' Reposition and resize buttons
        If WindowHelper.GetSystemDpi() > 96.0F Then
            backBox.SizeMode = PictureBoxSizeMode.Zoom
            minBox.SizeMode = PictureBoxSizeMode.Zoom
            closeBox.SizeMode = PictureBoxSizeMode.Zoom
        End If
        backBox.Size = WindowHelper.ScaleSizeLogical(46, 32)
        minBox.Size = WindowHelper.ScaleSizeLogical(46, 32)
        closeBox.Size = WindowHelper.ScaleSizeLogical(46, 32)
        Next_Button.Left = WindowHelper.ScaleLogical(998)

        ' Set color modes
        BodyPanelContainer.BackColor = CurrentTheme.BackgroundColor
        BodyPanelContainer.ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.BackColor = BodyPanelContainer.BackColor
        ComboBox2.BackColor = BodyPanelContainer.BackColor
        ComboBox3.BackColor = BodyPanelContainer.BackColor
        ComboBox1.ForeColor = BodyPanelContainer.ForeColor
        ComboBox2.ForeColor = BodyPanelContainer.ForeColor
        ComboBox3.ForeColor = BodyPanelContainer.ForeColor
        NumericUpDown1.BackColor = BodyPanelContainer.BackColor
        NumericUpDown1.ForeColor = BodyPanelContainer.ForeColor
        TextBox1.BackColor = BodyPanelContainer.BackColor
        TextBox1.ForeColor = BodyPanelContainer.ForeColor
        TextBox2.BackColor = BodyPanelContainer.BackColor
        TextBox2.ForeColor = BodyPanelContainer.ForeColor
        TrackBar1.BackColor = BodyPanelContainer.BackColor

        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()
        ComboBox1.SelectedText = ""
        ComboBox2.SelectedText = ""

        ApplyLocalizedText()

        ' English is the default language when no saved language is available.
        ComboBox1.SelectedIndex = 0
        SelectLanguageComboBox(ComboBox2, MainForm.LanguageCode)

        If Not Environment.OSVersion.Version.Major >= 10 Or Not (DetectFont("Segoe UI Variable Display Semib") Or DetectFont("Segoe UI Variable Semib")) Then
            Label2.Font = New Font("Segoe UI", Label2.Font.Size, FontStyle.Regular)
            Label6.Font = New Font("Segoe UI", Label6.Font.Size, FontStyle.Regular)
            Label14.Font = New Font("Segoe UI", Label14.Font.Size, FontStyle.Regular)
            Label24.Font = New Font("Segoe UI", Label24.Font.Size, FontStyle.Regular)
        End If
    End Sub

    Private Function GetSelectedLanguageCode(comboBox As ComboBox, fallbackCultureCode As String) As String
        If comboBox.SelectedItem IsNot Nothing AndAlso TypeOf comboBox.SelectedItem Is LocalizationLanguageInfo Then
            Return DirectCast(comboBox.SelectedItem, LocalizationLanguageInfo).Code
        End If

        If comboBox.SelectedValue IsNot Nothing Then
            Return comboBox.SelectedValue.ToString()
        End If

        Return LocalizationService.NormalizeCultureCode(fallbackCultureCode)
    End Function

    Private Sub PopulateLanguageComboBox(comboBox As ComboBox, selectedCultureCode As String)
        comboBox.Items.Clear()
        For Each languageInfo As LocalizationLanguageInfo In LocalizationService.GetAvailableLanguages()
            comboBox.Items.Add(languageInfo)
        Next
        SelectLanguageComboBox(comboBox, selectedCultureCode)
    End Sub

    Private Sub SelectLanguageComboBox(comboBox As ComboBox, selectedCultureCode As String)
        Dim normalizedCultureCode As String = LocalizationService.NormalizeCultureCode(selectedCultureCode)
        Dim selectedIndex As Integer = -1

        For index As Integer = 0 To comboBox.Items.Count - 1
            Dim languageInfo As LocalizationLanguageInfo = TryCast(comboBox.Items(index), LocalizationLanguageInfo)
            If languageInfo IsNot Nothing AndAlso languageInfo.Code.Equals(normalizedCultureCode, StringComparison.OrdinalIgnoreCase) Then
                selectedIndex = index
                Exit For
            End If
        Next

        If selectedIndex < 0 Then
            For index As Integer = 0 To comboBox.Items.Count - 1
                Dim languageInfo As LocalizationLanguageInfo = TryCast(comboBox.Items(index), LocalizationLanguageInfo)
                If languageInfo IsNot Nothing AndAlso languageInfo.Code.Equals(LocalizationService.DefaultCultureCode, StringComparison.OrdinalIgnoreCase) Then
                    selectedIndex = index
                    Exit For
                End If
            Next
        End If

        If selectedIndex >= 0 Then comboBox.SelectedIndex = selectedIndex
    End Sub

    Private Sub ApplyLocalizedText()
        Dim selectedColorMode As Integer = ComboBox1.SelectedIndex
        Dim selectedLanguageCode As String = GetSelectedLanguageCode(ComboBox2, MainForm.LanguageCode)

        If selectedColorMode < 0 Then selectedColorMode = 0

        isApplyingLocalizedText = True
        Try
            Text = LocalizationService.ForSection("PrgSetup")("Set.Up.DISM.Label")
            Label1.Text = Text
            Label2.Text = LocalizationService.ForSection("PrgSetup")("Welcome.DISM.Tools.Label")
            Label3.Text = LocalizationService.ForSection("PrgSetup")("DISM.Tools.Free.Message")
            Label5.Text = LocalizationService.ForSection("PrgSetup")("Yours.Customize.Message")
            Label6.Text = LocalizationService.ForSection("PrgSetup")("CustomizeProgram.Label")
            Label7.Text = LocalizationService.ForSection("PrgSetup")("ColorMode.Label")
            Label8.Text = LocalizationService.ForSection("PrgSetup")("Language.Label")
            Label9.Text = LocalizationService.ForSection("PrgSetup")("Log.Window.Font.Label")
            Label10.Text = LocalizationService.ForSection("PrgSetup")("LogFile.Label")
            Label13.Text = LocalizationService.ForSection("PrgSetup")("Log.Settings.Message")
            Label14.Text = LocalizationService.ForSection("PrgSetup")("Log.Label")
            Label20.Text = LocalizationService.ForSection("PrgSetup")("Anything.Like.Label")
            Label21.Text = LocalizationService.ForSection("PrgSetup")("Settings.Available.Message")
            Label22.Text = LocalizationService.ForSection("PrgSetup")("Perform.Steps.Time.Label")
            Label23.Text = LocalizationService.ForSection("PrgSetup")("Done.Setting.Up.Message")
            Label24.Text = LocalizationService.ForSection("PrgSetup")("SetupComplete.Label")
            Label25.Text = LocalizationService.ForSection("PrgSetup")("Ve.Set.Things.Label")
            Label26.Text = LocalizationService.ForSection("PrgSetup")("Stay.Up.Date.Label")
            Label27.Text = LocalizationService.ForSection("PrgSetup")("Get.Started.DISM.Label")
            Label28.Text = LocalizationService.ForSection("PrgSetup")("Secondary.Progress.Label")
            Label29.Text = LocalizationService.ForSection("PrgSetup")("Font.Readable.Log.Message")
            TextBox1.Text = LocalizationService.ForSection("PrgSetup.LogPreview")("Packages.Add.Message")
            ApplySecondaryProgressPreview()
            Back_Button.Text = LocalizationService.ForSection("PrgSetup")("Back.Button")
            Cancel_Button.Text = LocalizationService.ForSection("PrgSetup")("Cancel.Button")
            Button1.Text = LocalizationService.ForSection("PrgSetup")("Browse.Button")
            Button2.Text = LocalizationService.ForSection("PrgSetup")("Default.Log.File.Button")
            Button5.Text = LocalizationService.ForSection("PrgSetup")("Configure.Settings.Button")
            Button6.Text = LocalizationService.ForSection("PrgSetup")("GetStarted.Button")
            Button7.Text = LocalizationService.ForSection("PrgSetup")("CheckUpdates.Button")
            CheckBox1.Text = LocalizationService.ForSection("PrgSetup")("Auto.Create.Logs.CheckBox")
            RadioButton1.Text = LocalizationService.ForSection("PrgSetup")("Modern.RadioButton")
            RadioButton2.Text = LocalizationService.ForSection("PrgSetup")("Classic.RadioButton")
            SaveFileDialog1.Title = LocalizationService.ForSection("PrgSetup")("Log.File.Title")
            SaveFileDialog1.Filter = LocalizationService.ForSection("PrgSetup.Dialogs")("SaveFile.Filter")

            ColorModes(0) = LocalizationService.ForSection("PrgSetup")("System.Setting.Item")
            ColorModes(1) = LocalizationService.ForSection("PrgSetup")("LightMode.Item")
            ColorModes(2) = LocalizationService.ForSection("PrgSetup")("DarkMode.Item")
            ComboBox1.Items.Clear()
            ComboBox2.Items.Clear()
            ComboBox1.Items.AddRange(ColorModes)
            PopulateLanguageComboBox(ComboBox2, selectedLanguageCode)

            ComboBox1.SelectedIndex = Math.Min(selectedColorMode, ComboBox1.Items.Count - 1)
            SelectLanguageComboBox(ComboBox2, selectedLanguageCode)
        Finally
            isApplyingLocalizedText = False
        End Try

        ApplyTrackBarText()
        ApplyNavigationText()
    End Sub

    Private Sub ApplyNavigationText()
        If pageInt = 4 Then
            Next_Button.Text = LocalizationService.ForSection("PrgSetup.Next")("Finish.Label")
        Else
            Next_Button.Text = LocalizationService.ForSection("PrgSetup.Next.Actions")("Next.Button")
        End If
    End Sub

    Private Sub ApplyTrackBarText()
        Select Case TrackBar1.Value
            Case 0
                Label11.Text = LocalizationService.ForSection("PrgSetup.LogLevel")("Errors.Label")
                Label16.Text = LocalizationService.ForSection("PrgSetup.LogLevel")("File.Only.Display.Label")
            Case 1
                Label11.Text = LocalizationService.ForSection("PrgSetup.LogLevel")("Errors.Warnings.Label")
                Label16.Text = LocalizationService.ForSection("PrgSetup.LogLevel")("File.Display.Errors.Label")
            Case 2
                Label11.Text = LocalizationService.ForSection("PrgSetup.LogLevel")("Errors.Messages.Label")
                Label16.Text = LocalizationService.ForSection("PrgSetup.LogLevel")("File.Display.Errors.Message")
            Case 3
                Label11.Text = LocalizationService.ForSection("PrgSetup.LogLevel")("Errors.Warnings.Debug.Label")
                Label16.Text = LocalizationService.ForSection("PrgSetup.LogLevel")("Level3.Message")
        End Select
    End Sub


    Private Sub ApplySecondaryProgressPreview()
        Dim previewText As String = LocalizationService.ForSection("PrgSetup.ProgressPreview")("ImageIndexes.Message")
        Dim waitText As String = LocalizationService.ForSection("PrgSetup.ProgressPreview")("Wait.Label")
        SecProgressStylePreview.Image = RenderSecondaryProgressPreview(RadioButton1.Checked, waitText, previewText)
    End Sub

    Private Function RenderSecondaryProgressPreview(modernStyle As Boolean, waitText As String, previewText As String) As Bitmap
        Dim image As Bitmap = New Bitmap(If(modernStyle, My.Resources.secprogress_modern, My.Resources.secprogress_classic))

        Using graphics As Graphics = Graphics.FromImage(image)
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

            Using backgroundBrush As New SolidBrush(Color.FromArgb(32, 32, 32))
                If modernStyle Then
                    graphics.FillRectangle(backgroundBrush, 1, 1, image.Width - 2, image.Height - 2)
                Else
                    graphics.FillRectangle(backgroundBrush, 55, 1, image.Width - 56, image.Height - 2)
                End If
            End Using

            Using textBrush As New SolidBrush(Color.White)
                If modernStyle Then
                    Using previewFont As New Font("Segoe UI", 9.0F, FontStyle.Regular)
                        Using format As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                            graphics.DrawString(previewText, previewFont, textBrush, New RectangleF(0, 0, image.Width, image.Height), format)
                        End Using
                    End Using
                Else
                    Using waitFont As New Font("Segoe UI", 8.25F, FontStyle.Bold)
                        Using previewFont As New Font("Segoe UI", 8.25F, FontStyle.Regular)
                            graphics.DrawString(waitText, waitFont, textBrush, New PointF(56.0F, 13.0F))
                            graphics.DrawString(previewText, previewFont, textBrush, New PointF(56.0F, 29.0F))
                        End Using
                    End Using
                End If
            End Using
        End Using

        Return image
    End Function

    Function DetectFont(FontName As String) As Boolean
        DynaLog.LogMessage("Detecting if specified font is installed in this computer...")
        DynaLog.LogMessage("Font to test: " & FontName)
        For Each fntFamily As FontFamily In FontFamily.Families
            If fntFamily.Name = FontName Then
                DynaLog.LogMessage("The specified font is installed.")
                Return True
            End If
        Next
        DynaLog.LogMessage("The specified font is not installed.")
        Return False
    End Function

    Sub GetSystemFonts()
        ComboBox3.Items.Clear()
        For Each fntFamily As FontFamily In FontFamily.Families
            ComboBox3.Items.Add(fntFamily.Name)
        Next
        DynaLog.LogMessage(ComboBox3.Items.Count & " font(s) have been detected on this system.")
        ComboBox3.SelectedItem = "Consolas"
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        TextBox1.Font = New Font(ComboBox3.Text, NumericUpDown1.Value, If(Toggle1.Checked, FontStyle.Bold, FontStyle.Regular))
        MainForm.LogFont = ComboBox3.SelectedItem
        MainForm.LogFontSize = NumericUpDown1.Value
        MainForm.LogFontIsBold = Toggle1.Checked
        Panel9.Visible = Not IsMonospacedFont(ComboBox3.Text)
    End Sub

    Function IsMonospacedFont(ftName As String) As Boolean
        DynaLog.LogMessage("Detecting if font " & Quote & ftName & Quote & " is monospaced...")
        Using testFont As Font = New Font(ftName, 10)
            Dim widthI As Decimal = MeasureCharacterWidth(testFont, "i")
            Dim widthW As Decimal = MeasureCharacterWidth(testFont, "w")
            DynaLog.LogMessage("Width of character " & Quote & "i" & Quote & ": " & widthI)
            DynaLog.LogMessage("Width of character " & Quote & "W" & Quote & ": " & widthW)
            DynaLog.LogMessage("Are widths equal? " & If(widthI = widthW, "Yes", "No"))
            Return widthI = widthW
        End Using
        Return False
    End Function

    Function MeasureCharacterWidth(ft As Font, character As Char) As Decimal
        Using bmp As Bitmap = New Bitmap(1, 1)
            Using g As Graphics = Graphics.FromImage(bmp)
                Dim size As SizeF = g.MeasureString(character.ToString(), ft)
                Return size.Width
            End Using
        End Using
        Return 0
    End Function

    Private Sub Toggle1_CheckedChanged(sender As Object, e As EventArgs) Handles Toggle1.CheckedChanged
        TextBox1.Font = New Font(ComboBox3.Text, NumericUpDown1.Value, If(Toggle1.Checked, FontStyle.Bold, FontStyle.Regular))
        MainForm.LogFont = ComboBox3.SelectedItem
        MainForm.LogFontSize = NumericUpDown1.Value
        MainForm.LogFontIsBold = Toggle1.Checked
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        TextBox1.Font = New Font(ComboBox3.Text, NumericUpDown1.Value, If(Toggle1.Checked, FontStyle.Bold, FontStyle.Regular))
        MainForm.LogFont = ComboBox3.SelectedItem
        MainForm.LogFontSize = NumericUpDown1.Value
        MainForm.LogFontIsBold = Toggle1.Checked
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        MainForm.ColorMode = ComboBox1.SelectedIndex
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If isApplyingLocalizedText Then Return
        If ComboBox2.SelectedIndex < 0 Then Return

        Dim previousLanguageCode As String = MainForm.LanguageCode
        Dim selectedLanguageCode As String = GetSelectedLanguageCode(ComboBox2, previousLanguageCode)
        Dim validationMessage As String = ""
        If Not LocalizationService.ValidateLanguage(selectedLanguageCode, validationMessage) Then
            MessageBox.Show(validationMessage,
                            "Incompatible or invalid DISMTools language file",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            isApplyingLocalizedText = True
            Try
                SelectLanguageComboBox(ComboBox2, previousLanguageCode)
            Finally
                isApplyingLocalizedText = False
            End Try
            Return
        End If

        MainForm.LanguageCode = selectedLanguageCode
        LocalizationService.SetLanguageByCultureCode(MainForm.LanguageCode)
        ApplyLocalizedText()
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        DynaLog.LogMessage("Value of log level trackbar: " & TrackBar1.Value)
        ApplyTrackBarText()
        MainForm.LogLevel = TrackBar1.Value + 1
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        IncompleteSetupDlg.ShowDialog(Me)
        If IncompleteSetupDlg.DialogResult = Windows.Forms.DialogResult.OK Then
            Close()
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        ApplySecondaryProgressPreview()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Panel8.Enabled = Not CheckBox1.Checked
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox2.Text = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\Logs\DISM\DISM.log"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SaveFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        TextBox2.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        DynaLog.LogMessage("Beginning download of Update System...")
        Try
            If File.Exists(Application.StartupPath & "\update.exe") Then File.Delete(Application.StartupPath & "\update.exe")
        Catch ex As Exception
            DynaLog.LogMessage("Could not delete existing update downloader...")
            Exit Sub
        End Try
        Try
            DynaLog.LogMessage("Downloading " & Quote & "update.exe" & Quote & " from DISMTools repository...")
            Using client As New WebClient()
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                client.DownloadFile("https://github.com/CodingWonders/DISMTools/raw/stable/Updater/DISMTools-UCS/update-bin/update.exe", Application.StartupPath & "\update.exe")
            End Using
        Catch ex As WebException
            DynaLog.LogMessage("Could not get updater. Error message: " & ex.Status.ToString())
            MsgBox(LocalizationService.ForSection("PrgSetup.Validation").Format("DownloadFailure.Message", ex.Status.ToString()), vbOKOnly + vbCritical, LocalizationService.ForSection("PrgSetup.Actions")("UpdateChecker.Title"))
            Exit Sub
        End Try
        DynaLog.LogMessage("Information to pass to updater:")
        DynaLog.LogMessage("- Branch: " & MainForm.dtBranch)
        DynaLog.LogMessage("- Process ID (PID): " & Process.GetCurrentProcess().Id)
        If File.Exists(Application.StartupPath & "\update.exe") Then
            Process.Start(Application.StartupPath & "\update.exe", "/" & MainForm.dtBranch & " /pid=" & Process.GetCurrentProcess().Id & " " & LocalizationService.GetLanguageCommandLineArgument())
            Next_Button.PerformClick()
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        HelpDocsModule.DisplayHelpDocumentation("docs\getting_started\start.html")
    End Sub

    Private Sub PrgSetup_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If WindowState = FormWindowState.Maximized Then
            WindowState = FormWindowState.Normal
        End If
    End Sub
End Class
