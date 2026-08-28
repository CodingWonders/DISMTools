Imports StarterScriptEditor.Classes
Imports StarterScriptEditor.Classes.ColorUtilities
Imports System.IO
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Win32
Imports System.Text.RegularExpressions

Public Class MainForm

    Private CurrentScript As StarterScript
    Private SupportedLanguageList As New List(Of String)

    Public UserDataScriptFolder As String

    Private Modified As Boolean
    Private SavedScriptPath As String
    Private NotWillingToSave As Boolean

    Private roMode As Boolean

    Public CurrentColorMode As ColorThemeMode

    Private Const SSECodeName As String = "Luffy"

    Private ScriptVer As ScriptVersion = ScriptVersion.Infinity

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
                    If ColorModeRk IsNot Nothing Then
                        darkMode = ColorModeRk.GetValue("AppsUseLightTheme", 1) = 0
                        ColorModeRk.Close()

                        If darkMode Then SetColorMode(ColorThemeMode.Dark) Else SetColorMode(ColorThemeMode.Light)
                    End If
                Catch ex As Exception
                    SetColorMode(ColorThemeMode.Light)
                End Try

                Exit Sub
        End Select

        tbScriptName.BackColor = BackColor
        tbScriptName.ForeColor = ForeColor
        tbScriptDescription.BackColor = BackColor
        tbScriptDescription.ForeColor = ForeColor
        tbScriptCode.BackColor = BackColor
        tbScriptCode.ForeColor = ForeColor
        comboLanguage.BackColor = BackColor
        comboLanguage.ForeColor = ForeColor
        ColorModeTSDDB.ForeColor = ForeColor

        If NewColorMode = ColorThemeMode.Light Then
            ToolStrip1.Renderer = New LightModeRenderer()
        ElseIf NewColorMode = ColorThemeMode.Dark Then
            ToolStrip1.Renderer = New DarkModeRenderer()
        End If
        ChangeMenuItemColors(BackColor, ForeColor, ColorModeTSDDB.DropDownItems)
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
            ElseIf arg.StartsWith("/dtss", StringComparison.OrdinalIgnoreCase) Then
                ' This parameter determines the path to a DTSS
                Dim dtssPath As String = Regex.Replace(arg, "\/dtss=", "", RegexOptions.IgnoreCase)

                If File.Exists(dtssPath) Then
                    LoadScriptFile(dtssPath)
                    UpdateScriptProperties()
                    ' Loading the script file will make the modification factor true; we don't want that
                    Modified = False
                End If
            ElseIf arg.StartsWith("/convert", StringComparison.OrdinalIgnoreCase) Then
                Dim sourcePath As String = Regex.Replace(arg, "\/convert=", "", RegexOptions.IgnoreCase)

                BulkScriptConversionDialog.SourceScriptPath = sourcePath
                BulkScriptConversionDialog.ShowDialog()
            End If
        Next
    End Sub

    Private Function GetNewStarterScript() As StarterScript
        Return New StarterScript("PowerShell")
    End Function

    Private Sub UpdateScriptProperties()
        tbScriptName.Text = CurrentScript.Name
        tbScriptDescription.Text = CurrentScript.Description
        ' If our list of languages does NOT contain our language, we assume it's
        ' the first item
        If Not SupportedLanguageList.Contains(CurrentScript.Language.ToLower()) Then
            comboLanguage.SelectedIndex = 0
        Else
            comboLanguage.SelectedItem = CurrentScript.Language
        End If
        tbScriptCode.Text = CurrentScript.Code
        CheckBox2.Checked = CurrentScript.OptionsCustomizable
    End Sub

    Private Sub LoadScriptFile(ByVal ScriptFile As String)
        If Not File.Exists(ScriptFile) Then
            MsgBox("The script file does not exist.", vbOKOnly + vbExclamation)
            Exit Sub
        End If

        roMode = False
        ToolStripButton5.Enabled = False

        ' Read the file here first so we can guess the version
        Dim scriptFileContents As String() = File.ReadAllLines(ScriptFile)
        ScriptVer = ScriptVersion.Seven
        Dim CodeBlockStartingIndex As Integer = 3
        If scriptFileContents(3).StartsWith("Customizable:", StringComparison.OrdinalIgnoreCase) Then
            ScriptVer = ScriptVersion.Infinity
            CodeBlockStartingIndex = 4
        End If

        CurrentScript = StarterScriptHelper.LoadScriptFile(ScriptFile, CodeBlockStartingIndex)

        SavedScriptPath = ScriptFile
        Text = String.Format("Starter Script Editor - {0}", Path.GetFileName(SavedScriptPath))

        If (File.GetAttributes(ScriptFile) And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
            MessageBox.Show("This script file has been loaded with read-only privileges. If you make changes to this script, you must save them to a new script file or enable write access for this script.", "Starter Script Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            roMode = True
            ToolStripButton5.Enabled = True
        End If
    End Sub

    Private Sub SaveScriptFile(ByVal ScriptFile As String, Optional ByVal DefaultScriptVersion As Boolean = True)
        If DefaultScriptVersion AndAlso ScriptVer < ScriptVersion.Infinity Then
            If MessageBox.Show("The starter script had been created with an earlier version of the Starter Script Editor and will be saved with properties that will make it compatible with the current format. After this is done, the starter script will no longer be compatible with earlier versions of DISMTools or the Starter Script Editor." & CrLf & CrLf & _
                               "Do you want to save this file?", "Starter Script Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                NotWillingToSave = True
                Exit Sub
            End If
        End If

        If File.Exists(ScriptFile) Then
            Try
                File.Delete(ScriptFile)
            Catch ex As Exception
                ' ignore these
            End Try
        End If

        Try
            If Not DefaultScriptVersion AndAlso ScriptVer = ScriptVersion.Seven Then
                If Not StarterScriptHelper.SaveStarterScript(ScriptFile, CurrentScript, ScriptVersion.Seven) Then Throw New Exception()
            Else
                If Not StarterScriptHelper.SaveStarterScript(ScriptFile, CurrentScript, ScriptVersion.Infinity) Then Throw New Exception()
            End If

            SavedScriptPath = ScriptFile
            Text = String.Format("Starter Script Editor - {0}", Path.GetFileName(SavedScriptPath))
            Modified = False
            roMode = False
            ToolStripButton5.Enabled = False
        Catch ex As Exception
            MessageBox.Show("Changes could not be saved to the script file. Make sure write access is present in the file. " & CrLf & CrLf & ex.Message & CrLf & CrLf & "To enable write access for this file, use the respective button in the toolbar.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            NotWillingToSave = True
        End Try
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        AIResults.CheckBox1.Checked = False
        NotWillingToSave = False
        If Modified Then
            Select Case MessageBox.Show("Do you want to save the changes to your script file?", "Starter Script Editor", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                Case Windows.Forms.DialogResult.Yes
                    ToolStripButton3.PerformClick()
                    If NotWillingToSave Then Exit Sub
                Case Windows.Forms.DialogResult.Cancel
                    Exit Sub
            End Select
        End If

        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        AIResults.CheckBox1.Checked = False
        If tbScriptName.Text = "" Then
            MessageBox.Show("You must provide a name for this starter script.", "Starter Script Editor", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            NotWillingToSave = True
            Exit Sub
        End If
        If tbScriptDescription.Text = "" Then
            MessageBox.Show("You must provide a description for this starter script.", "Starter Script Editor", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            NotWillingToSave = True
            Exit Sub
        End If
        NotWillingToSave = False
        If Not String.IsNullOrEmpty(SavedScriptPath) AndAlso File.Exists(SavedScriptPath) AndAlso Not roMode Then
            SaveScriptFile(SavedScriptPath, ScriptVer = ScriptVersion.Infinity)
            Exit Sub
        End If
        If SaveFileDialog1.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then
            NotWillingToSave = True
        Else
            SaveScriptFile(SaveFileDialog1.FileName, ScriptVer = ScriptVersion.Infinity)
        End If
    End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SystemCM_TSMI.Enabled = Environment.OSVersion.Version.Major >= 10

        SetColorMode(ColorThemeMode.System)
        If Environment.OSVersion.Version.Major > 5 OrElse (Environment.OSVersion.Version.Major = 5 AndAlso Environment.OSVersion.Version.Minor = 1) Then
            CheckBox2.FlatStyle = FlatStyle.Standard
        End If
        GetArguments()
        SaveFileDialog1.InitialDirectory = UserDataScriptFolder

        SupportedLanguageList.AddRange(New String(3) {"batch", "powershell", "vbscript", "jscript"})
        If CurrentScript Is Nothing Then CurrentScript = GetNewStarterScript()
        UpdateScriptProperties()
        UpdateCaretPosition()

        Modified = False

#If VBC_VER < 10.0 Then
#If Not Debug Then
        ToolStripButton9.Visible = False
        ToolStripSeparator3.Visible = False
#End If
#End If
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        AIResults.CheckBox1.Checked = False
        NotWillingToSave = False
        If Modified Then
            Select Case MessageBox.Show("Do you want to save the changes to your script file?", "Starter Script Editor", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                Case Windows.Forms.DialogResult.Yes
                    ToolStripButton3.PerformClick()
                    If NotWillingToSave Then Exit Sub
                Case Windows.Forms.DialogResult.Cancel
                    Exit Sub
            End Select
        End If

        CurrentScript = GetNewStarterScript()
        UpdateScriptProperties()
        Modified = False
        roMode = False
        ToolStripButton5.Enabled = False
        ScriptVer = ScriptVersion.Infinity
        SavedScriptPath = ""
        Text = "Starter Script Editor"
    End Sub

    Private Sub OpenFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        SavedScriptPath = OpenFileDialog1.FileName
        LoadScriptFile(OpenFileDialog1.FileName)
        UpdateScriptProperties()
        Modified = False
    End Sub

    Private Sub SaveFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        SaveScriptFile(SaveFileDialog1.FileName, ScriptVer = ScriptVersion.Infinity)
    End Sub

    Private Sub tbScriptName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbScriptName.TextChanged
        CurrentScript.Name = tbScriptName.Text
        Modified = True
    End Sub

    Private Sub tbScriptDescription_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbScriptDescription.TextChanged
        CurrentScript.Description = tbScriptDescription.Text
        Modified = True
    End Sub

    Private Sub comboLanguage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles comboLanguage.SelectedIndexChanged
        CurrentScript.Language = comboLanguage.SelectedItem
        Modified = True
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        CurrentScript.OptionsCustomizable = CheckBox2.Checked
        Modified = True
    End Sub

    Private Sub tbScriptCode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbScriptCode.TextChanged
        CurrentScript.Code = tbScriptCode.Text
        Modified = True
    End Sub

    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
        AIResults.CheckBox1.Checked = False
#If VBC_VER >= 9.0 Then
#If DEBUG Then
        MsgBox(String.Format("DISMTools Starter Script Editor version {0} ({1}_DEBUG)" & CrLf & CrLf & "{2}", _
                My.Application.Info.Version.ToString() & "_" & RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm") , _
                SSECodeName.ToUpper(), _
                My.Application.Info.Copyright), _
            vbOKOnly + vbInformation, "About")
#Else
        MsgBox(String.Format("DISMTools Starter Script Editor version {0}" & CrLf & CrLf & "{1}", _
                My.Application.Info.Version.ToString() & "_" & RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm") , _
                My.Application.Info.Copyright), _
            vbOKOnly + vbInformation, "About")
#End If
#Else
#If DEBUG Then
        MsgBox(String.Format("DISMTools Starter Script Editor version {0}_NET2REL ({1}_DEBUG)" & CrLf & CrLf & "{2}", _
                My.Application.Info.Version.ToString(), SSECodeName.ToUpper(), My.Application.Info.Copyright), _
            vbOKOnly + vbInformation, "About")
#Else
        MsgBox(String.Format("DISMTools Starter Script Editor version {0}_NET2REL" & CrLf & CrLf & "{1}", _
                My.Application.Info.Version.ToString(), My.Application.Info.Copyright), _
            vbOKOnly + vbInformation, "About")
#End If
#End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        OpenFileDialog2.FilterIndex = comboLanguage.SelectedIndex + 1
        OpenFileDialog2.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog2_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog2.FileOk
        If Not File.Exists(OpenFileDialog2.FileName) Then Exit Sub

        If tbScriptCode.Text <> "" Then
            If MessageBox.Show("Importing the selected script will replace existing contents of your script.", "Import Existing Script", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Cancel Then
                Exit Sub
            End If
        End If

        Dim scriptFileName As String = OpenFileDialog2.FileName
        Dim scriptExtension As String = Path.GetExtension(scriptFileName).ToLower()

        Dim expectedBatchExtensions As New List(Of String), expectedVbScriptExtensions As New List(Of String), expectedJScriptExtensions As New List(Of String)
        expectedBatchExtensions.AddRange(New String(2) {".bat", ".cmd", ".nt"})
        expectedVbScriptExtensions.AddRange(New String(3) {".vbs", ".vbe", ".wsf", ".wsc"})
        expectedJScriptExtensions.AddRange(New String(1) {".js", ".jse"})
        If expectedBatchExtensions.Contains(scriptExtension) Then
            comboLanguage.SelectedIndex = 0
        ElseIf scriptExtension.ToLower() = ".ps1" Then
            comboLanguage.SelectedIndex = 1
        ElseIf expectedVbScriptExtensions.Contains(scriptExtension) Then
            comboLanguage.SelectedIndex = 2
        ElseIf expectedJScriptExtensions.Contains(scriptExtension) Then
            comboLanguage.SelectedIndex = 3
        Else
            MessageBox.Show("This script is not supported by the Starter Script Editor.", "Unrecognized script", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Try
            Dim scriptContents As String = File.ReadAllText(scriptFileName)
            tbScriptCode.Text = scriptContents
            UpdateCaretPosition()
        Catch ex As Exception
            MessageBox.Show("The contents of the script could not be loaded.", "Could not read file contents", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        tbScriptCode.WordWrap = CheckBox1.Checked
        Label6.Visible = Not CheckBox1.Checked
        UpdateCaretPosition()
    End Sub

    Private Sub tbScriptCode_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbScriptCode.KeyDown
        UpdateCaretPosition()
    End Sub

    Private Sub tbScriptCode_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbScriptCode.MouseUp
        UpdateCaretPosition()
    End Sub

    Public Sub UpdateCaretPosition()
        Dim caret As Integer = tbScriptCode.SelectionStart, _
            line As Integer = tbScriptCode.GetLineFromCharIndex(caret), _
            column As Integer = caret - tbScriptCode.GetFirstCharIndexFromLine(line)

        Label6.Text = String.Format("Ln {0}, Col {1}", line + 1, column + 1)
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        NotWillingToSave = False
        If Modified Then
            ' EditorEX dialogs have to go away
            If FindReplaceDialog IsNot Nothing Then FindReplaceDialog.Close()
            If DocumentOutlineViewer IsNot Nothing Then DocumentOutlineViewer.Close()
            Select Case MessageBox.Show("Do you want to save the changes to your script file?", "Starter Script Editor", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                Case Windows.Forms.DialogResult.Yes
                    ToolStripButton3.PerformClick()
                    If NotWillingToSave Then
                        e.Cancel = True
                        Exit Sub
                    End If
                Case Windows.Forms.DialogResult.Cancel
                    e.Cancel = True
                    Exit Sub
            End Select
        End If

    End Sub

    Private Sub InvokeFindDialog(Optional ByVal FindAndReplace As Boolean = False)
        FindReplaceDialog.EditorControl = tbScriptCode
        FindReplaceDialog.MyParent = Me
        FindReplaceDialog.ReplaceMode = FindAndReplace
        If FindReplaceDialog.Visible Then
            If FindReplaceDialog.WindowState = FormWindowState.Minimized Then FindReplaceDialog.WindowState = FormWindowState.Normal
            FindReplaceDialog.BringToFront()
            FindReplaceDialog.Focus()
            Exit Sub
        End If
        FindReplaceDialog.Location = New Point(Left + WindowHelper.ScaleLogical(32), Top + WindowHelper.ScaleLogical(32))
        FindReplaceDialog.Show()
    End Sub

    Private Sub MainForm_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.Control Then
            Select Case e.KeyCode
                Case Keys.N
                    ' New item
                    ToolStripButton1.PerformClick()
                Case Keys.O
                    ' Open item
                    ToolStripButton2.PerformClick()
                Case Keys.S
                    If e.Shift Then
                        ' Save item AS
                        ToolStripButton8.PerformClick()
                    Else
                        ' Save item
                        ToolStripButton3.PerformClick()
                    End If
#If VBC_VER >= 10.0 Then
                Case Keys.U
                    ' Upload to Script Library
                    ToolStripButton9.PerformClick()
#End If
                Case Keys.I
                    ' Inspect Code
                    ToolStripButton10.PerformClick()
                Case Keys.F
                    ' Find dialog
                    InvokeFindDialog()
                Case Keys.R
                    ' Find & Replace dialog
                    InvokeFindDialog(True)
                Case Keys.T
                    ' CTRL + ALT + T: Document Outline
                    If e.Alt Then InvokeDocumentOutlineDialog()
            End Select
        End If
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

    Private Sub ToolStripButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton5.Click
        AIResults.CheckBox1.Checked = False
        EnableWriteAccess()
    End Sub

    Private Sub EnableWriteAccess()
        If SavedScriptPath = "" OrElse Not File.Exists(SavedScriptPath) Then Exit Sub
        Try
            File.SetAttributes(SavedScriptPath, (File.GetAttributes(SavedScriptPath) And Not FileAttributes.ReadOnly))
            roMode = False
            ToolStripButton5.Enabled = False
        Catch ex As Exception
            MessageBox.Show("Could not enable write access for this script file. Make sure that the script is not in read-only media.", "Starter Script Editor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CheckBox2_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.MouseHover
        WindowHelper.DisplayToolTip(sender, "Check this option if this script contains settings that can be configured by the user" & CrLf & "after importing the starter script from the Starter Script Browser.")
    End Sub

    Private Sub ToolStripButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton6.Click
        AIResults.CheckBox1.Checked = False
        ScriptVersionChooser.RadioButton1.Checked = ScriptVer = ScriptVersion.Infinity
        ScriptVersionChooser.RadioButton2.Checked = ScriptVer = ScriptVersion.Seven
        If ScriptVersionChooser.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            If ScriptVersionChooser.IsInfinityScript Then
                ScriptVer = ScriptVersion.Infinity
            Else
                ScriptVer = ScriptVersion.Seven
            End If
        End If
    End Sub

    Private Sub ToolStripButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton7.Click
        AIResults.CheckBox1.Checked = False
        EditorFD.Font = tbScriptCode.Font
        Dim fontConfigured As Boolean = False
        Do Until fontConfigured
            Try
                If EditorFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    If Not IsMonospacedFont(EditorFD.Font.Name) AndAlso MessageBox.Show("You have selected a non-monospaced font. Text may not look correctly. Do you want to continue?", "Starter Script Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                        Exit Sub
                    End If
                    tbScriptCode.Font = EditorFD.Font
                End If
                fontConfigured = True
            Catch arEx As ArgumentException
                ' The user may have selected a non-TrueType font
                MessageBox.Show(arEx.Message, "Starter Script Editor", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        Loop
    End Sub

    Private Function IsMonospacedFont(ByVal ftName As String) As Boolean
        Using testFont As Font = New Font(ftName, 10)
            Dim widthI As Decimal = MeasureCharacterWidth(testFont, "i")
            Dim widthW As Decimal = MeasureCharacterWidth(testFont, "w")
            Return widthI = widthW
        End Using
        Return False
    End Function

    Private Function MeasureCharacterWidth(ByVal ft As Font, ByVal character As Char) As Decimal
        Using bmp As Bitmap = New Bitmap(1, 1)
            Using g As Graphics = Graphics.FromImage(bmp)
                Dim size As SizeF = g.MeasureString(character.ToString(), ft)
                Return size.Width
            End Using
        End Using
        Return 0
    End Function

    Private Sub ToolStripButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton8.Click
        AIResults.CheckBox1.Checked = False
        If tbScriptName.Text = "" Then
            MessageBox.Show("You must provide a name for this starter script.", "Starter Script Editor", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            NotWillingToSave = True
            Exit Sub
        End If
        If tbScriptDescription.Text = "" Then
            MessageBox.Show("You must provide a description for this starter script.", "Starter Script Editor", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            NotWillingToSave = True
            Exit Sub
        End If
        NotWillingToSave = False
        If SaveFileDialog1.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then
            NotWillingToSave = True
        Else
            SaveScriptFile(SaveFileDialog1.FileName, ScriptVer = ScriptVersion.Infinity)
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        tbScriptCode.Text = Regex.Replace(tbScriptCode.Text, ControlChars.Tab, "    ")
    End Sub

    Private Sub ToolStripButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton9.Click
        AIResults.Close()
        UploadToScriptLibraryDialog.StarterScriptToUpload = CurrentScript
        UploadToScriptLibraryDialog.ShowDialog(Me)
    End Sub

    Private Sub ToolStripButton10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton10.Click
        AIResults.Close()
        InspectionProgressDialog.ScriptCode = CurrentScript.Code
        InspectionProgressDialog.ShowDialog(Me)
        AIResults.Results = InspectionProgressDialog.InspectionResults
        AIResults.Show()
    End Sub

    Private Sub ToolStripButton11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton11.Click
        AIResults.CheckBox1.Checked = False
        AICustomRuleViewer.Show()
    End Sub

    Private Sub ToolStripButton12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton12.Click
        InvokeFindDialog()
    End Sub

    Private Sub ToolStripButton13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton13.Click
        InvokeFindDialog(True)
    End Sub

    Private Sub InvokeDocumentOutlineDialog()
        DocumentOutlineViewer.comboLangMode.SelectedIndex = comboLanguage.SelectedIndex
        DocumentOutlineViewer.EditorControl = tbScriptCode
        DocumentOutlineViewer.MyParent = Me
        If DocumentOutlineViewer.Visible Then
            If DocumentOutlineViewer.WindowState = FormWindowState.Minimized Then DocumentOutlineViewer.WindowState = FormWindowState.Normal
            DocumentOutlineViewer.BringToFront()
            DocumentOutlineViewer.Focus()
            Exit Sub
        End If
        DocumentOutlineViewer.Show()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        InvokeDocumentOutlineDialog()
    End Sub
End Class
