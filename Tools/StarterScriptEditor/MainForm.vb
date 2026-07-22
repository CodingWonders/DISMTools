Imports StarterScript.Classes
Imports StarterScript.Classes.ColorUtilities
Imports System.IO
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Win32
Imports System.Text.RegularExpressions

Public Class MainForm

    Private CurrentScript As StarterScript
    Private SupportedLanguageList As New List(Of String)

    Private UserDataScriptFolder As String

    Private Modified As Boolean
    Private SavedScriptPath As String
    Private NotWillingToSave As Boolean

    Private roMode As Boolean

    Public CurrentColorMode As ColorThemeMode

    Private Const SSECodeName As String = "Luffy"

    Private Enum ScriptVersion As Integer
        ''' <summary>
        ''' Starter scripts for the DISMTools 0.7 Series (0.7.2, 0.7.3)
        ''' </summary>
        ''' <remarks></remarks>
        Seven = 0
        ''' <summary>
        ''' Starter scripts for the DISMTools 0.8 Series
        ''' </summary>
        ''' <remarks></remarks>
        Infinity = 1
    End Enum

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

        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        TextBox2.BackColor = BackColor
        TextBox2.ForeColor = ForeColor
        TextBox3.BackColor = BackColor
        TextBox3.ForeColor = ForeColor
        ComboBox1.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
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
                Dim dtssPath As String = arg.Replace("/dtss=", "")

                If File.Exists(dtssPath) Then
                    LoadScriptFile(dtssPath)
                    UpdateScriptProperties()
                    ' Loading the script file will make the modification factor true; we don't want that
                    Modified = False
                End If
            End If
        Next
    End Sub

    Private Function GetNewStarterScript() As StarterScript
        Return New StarterScript("PowerShell")
    End Function

    Private Sub UpdateScriptProperties()
        TextBox1.Text = CurrentScript.Name
        TextBox2.Text = CurrentScript.Description
        ' If our list of languages does NOT contain our language, we assume it's
        ' the first item
        If Not SupportedLanguageList.Contains(CurrentScript.Language.ToLower()) Then
            ComboBox1.SelectedIndex = 0
        Else
            ComboBox1.SelectedItem = CurrentScript.Language
        End If
        TextBox3.Text = CurrentScript.Code
        CheckBox2.Checked = CurrentScript.OptionsCustomizable
    End Sub

    Private Sub LoadScriptFile(ByVal ScriptFile As String)
        If Not File.Exists(ScriptFile) Then
            MsgBox(LocalizationService.ForSection("StarterScript")("FileMissing.Label"), vbOKOnly + vbExclamation)
            Exit Sub
        End If

        roMode = False
        ToolStripButton5.Enabled = False
        Dim scriptFileContents As String() = File.ReadAllLines(ScriptFile)

        ScriptVer = ScriptVersion.Seven
        Dim CodeBlockStartingIndex As Integer = 3
        If scriptFileContents(3).StartsWith("Customizable:", StringComparison.OrdinalIgnoreCase) Then
            ScriptVer = ScriptVersion.Infinity
            CodeBlockStartingIndex = 4
        End If

        ' Script Format:
        ' <Language>
        ' <Name>
        ' <Description>
        ' <Customizable> (0.8+)
        ' <code>
        Dim scriptLang As String = scriptFileContents(0).Replace("Language: ", "")
        Dim scriptName As String = scriptFileContents(1).Replace("Name: ", "")
        Dim scriptDescription As String = scriptFileContents(2).Replace("Description: ", "")
        Dim scriptOptionsCustomizable As Boolean = scriptFileContents(3).Equals("Customizable: Yes", StringComparison.OrdinalIgnoreCase)
#If VBC_VER >= 9.0 Then
        CurrentScript = New StarterScript(scriptName, scriptDescription, scriptLang, String.Join(ControlChars.CrLf, New List(Of String)(scriptFileContents).Skip(CodeBlockStartingIndex).ToArray()), scriptOptionsCustomizable)
#Else
        ' NDPv2 and earlier do not support LINQ statements.
        Dim ScriptCodeLines As New List(Of String)
        For x As Integer = CodeBlockStartingIndex To scriptFileContents.Length - 1
            ScriptCodeLines.Add(scriptFileContents(x))
        Next
        CurrentScript = New StarterScript(scriptName, scriptDescription, scriptLang, String.Join(ControlChars.CrLf, ScriptCodeLines.ToArray()), scriptOptionsCustomizable)
#End If
        SavedScriptPath = ScriptFile
        Text = String.Format(LocalizationService.ForSection("StarterScript")("Window.Title"), Path.GetFileName(SavedScriptPath))

        If (File.GetAttributes(ScriptFile) And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
            MessageBox.Show(LocalizationService.ForSection("StarterScript")("ReadOnlyFile.Message"), LocalizationService.ForSection("StarterScript")("Editor.Label"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            roMode = True
            ToolStripButton5.Enabled = True
        End If
    End Sub

    Private Sub SaveScriptFile(ByVal ScriptFile As String, Optional ByVal DefaultScriptVersion As Boolean = True)
        If DefaultScriptVersion AndAlso ScriptVer < ScriptVersion.Infinity Then
            If MessageBox.Show(LocalizationService.ForSection("StarterScript")("AlreadyCreated.Message"), LocalizationService.ForSection("StarterScript")("Dialog.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
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
            Dim customizableStr As String
            If CurrentScript.OptionsCustomizable Then
                customizableStr = "Yes"
            Else
                customizableStr = "No"
            End If

            If Not DefaultScriptVersion AndAlso ScriptVer = ScriptVersion.Seven Then
                File.WriteAllText(ScriptFile, String.Format("Language: {0}{1}" & _
                "Name: {2}{1}" & _
                "Description: {3}{1}" & _
                "{4}", CurrentScript.Language, Environment.NewLine, CurrentScript.Name, CurrentScript.Description, CurrentScript.Code), UTF8)
            Else
                File.WriteAllText(ScriptFile, String.Format("Language: {0}{1}" & _
                "Name: {2}{1}" & _
                "Description: {3}{1}" & _
                "Customizable: {4}{1}" & _
                "{5}", CurrentScript.Language, Environment.NewLine, CurrentScript.Name, CurrentScript.Description, customizableStr, CurrentScript.Code), UTF8)
            End If

            SavedScriptPath = ScriptFile
            Text = String.Format(LocalizationService.ForSection("StarterScript")("Window.Title"), Path.GetFileName(SavedScriptPath))
            Modified = False
            roMode = False
            ToolStripButton5.Enabled = False
        Catch ex As Exception
            MessageBox.Show(LocalizationService.ForSection("StarterScript").Format("SaveFailed.Message", ex.Message), LocalizationService.ForSection("StarterScript")("SaveError.Label"), MessageBoxButtons.OK, MessageBoxIcon.Error)
            NotWillingToSave = True
        End Try
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        NotWillingToSave = False
        If Modified Then
            Select Case MessageBox.Show(LocalizationService.ForSection("StarterScript")("SaveChanges.Label"), LocalizationService.ForSection("StarterScript")("Dialog.Title"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
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
        If TextBox1.Text = "" Then
            MessageBox.Show(LocalizationService.ForSection("StarterScript")("Name.Required.Label"), LocalizationService.ForSection("StarterScript")("Dialog.Title"), MessageBoxButtons.OK, MessageBoxIcon.Stop)
            NotWillingToSave = True
            Exit Sub
        End If
        If TextBox2.Text = "" Then
            MessageBox.Show(LocalizationService.ForSection("StarterScript")("Description.Required.Label"), LocalizationService.ForSection("StarterScript")("Dialog.Title"), MessageBoxButtons.OK, MessageBoxIcon.Stop)
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
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        NotWillingToSave = False
        If Modified Then
            Select Case MessageBox.Show(LocalizationService.ForSection("StarterScript")("SaveChanges.Message"), LocalizationService.ForSection("StarterScript")("Dialog.Title"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
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
        Text = LocalizationService.ForSection("StarterScript")("Window.Default")
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

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        CurrentScript.Name = TextBox1.Text
        Modified = True
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        CurrentScript.Description = TextBox2.Text
        Modified = True
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        CurrentScript.Language = ComboBox1.SelectedItem
        Modified = True
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        CurrentScript.OptionsCustomizable = CheckBox2.Checked
        Modified = True
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        CurrentScript.Code = TextBox3.Text
        Modified = True
    End Sub

    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
#If VBC_VER >= 9.0 Then
#If DEBUG Then
        MsgBox(LocalizationService.ForSection("StarterScript").Format("DebugEditor.Label", My.Application.Info.Version.ToString() & "_" & RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm"), SSECodeName.ToUpper(), My.Application.Info.Copyright), vbOKOnly + vbInformation, LocalizationService.ForSection("StarterScript")("About.Label"))
#Else
        MsgBox(LocalizationService.ForSection("StarterScript").Format("Editor.Message", My.Application.Info.Version.ToString() & "_" & RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm"), My.Application.Info.Copyright), vbOKOnly + vbInformation, LocalizationService.ForSection("StarterScript")("About.Label"))
#End If
#Else
#If DEBUG Then
        MsgBox(LocalizationService.ForSection("StarterScript").Format("DebugVersion.Message", My.Application.Info.Version.ToString(), SSECodeName.ToUpper(), My.Application.Info.Copyright), vbOKOnly + vbInformation, LocalizationService.ForSection("StarterScript")("About.Label"))
#Else
        MsgBox(LocalizationService.ForSection("StarterScript").Format("Version.Message", My.Application.Info.Version.ToString(), My.Application.Info.Copyright), vbOKOnly + vbInformation, LocalizationService.ForSection("StarterScript")("About.Label"))
#End If
#End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        OpenFileDialog2.FilterIndex = ComboBox1.SelectedIndex + 1
        OpenFileDialog2.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog2_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog2.FileOk
        If Not File.Exists(OpenFileDialog2.FileName) Then Exit Sub

        If TextBox3.Text <> "" Then
            If MessageBox.Show(LocalizationService.ForSection("StarterScript")("ImportSelected.Message"), LocalizationService.ForSection("StarterScript")("ImportExisting.Label"), MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = Windows.Forms.DialogResult.Cancel Then
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
            ComboBox1.SelectedIndex = 0
        ElseIf scriptExtension.ToLower() = ".ps1" Then
            ComboBox1.SelectedIndex = 1
        ElseIf expectedVbScriptExtensions.Contains(scriptExtension) Then
            ComboBox1.SelectedIndex = 2
        ElseIf expectedJScriptExtensions.Contains(scriptExtension) Then
            ComboBox1.SelectedIndex = 3
        Else
            MessageBox.Show(LocalizationService.ForSection("StarterScript")("SupportedScript.Label"), LocalizationService.ForSection("StarterScript")("Unrecognized.Label"), MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Try
            Dim scriptContents As String = File.ReadAllText(scriptFileName)
            TextBox3.Text = scriptContents
            UpdateCaretPosition()
        Catch ex As Exception
            MessageBox.Show(LocalizationService.ForSection("StarterScript")("LoadFailed.Label"), LocalizationService.ForSection("StarterScript")("ReadFailed.Label"), MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        TextBox3.WordWrap = CheckBox1.Checked
        Label6.Visible = Not CheckBox1.Checked
        UpdateCaretPosition()
    End Sub

    Private Sub TextBox3_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox3.KeyDown
        UpdateCaretPosition()
    End Sub

    Private Sub TextBox3_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TextBox3.MouseUp
        UpdateCaretPosition()
    End Sub

    Private Sub UpdateCaretPosition()
        Dim caret As Integer = TextBox3.SelectionStart, _
            line As Integer = TextBox3.GetLineFromCharIndex(caret), _
            column As Integer = caret - TextBox3.GetFirstCharIndexFromLine(line)

        Label6.Text = String.Format("Ln {0}, Col {1}", line + 1, column + 1)
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        NotWillingToSave = False
        If Modified Then
            Select Case MessageBox.Show(LocalizationService.ForSection("StarterScript")("SaveChanges.Message"), LocalizationService.ForSection("StarterScript")("Dialog.Title"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
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
        EnableWriteAccess()
    End Sub

    Private Sub EnableWriteAccess()
        If SavedScriptPath = "" OrElse Not File.Exists(SavedScriptPath) Then Exit Sub
        Try
            File.SetAttributes(SavedScriptPath, (File.GetAttributes(SavedScriptPath) And Not FileAttributes.ReadOnly))
            roMode = False
            ToolStripButton5.Enabled = False
        Catch ex As Exception
            MessageBox.Show(LocalizationService.ForSection("StarterScript")("WriteAccess.Message"), LocalizationService.ForSection("StarterScript")("Dialog.Title"), MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CheckBox2_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("StarterScript")("CheckScript.Message"))
    End Sub

    Private Sub ToolStripButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton6.Click
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
        EditorFD.Font = TextBox3.Font
        Dim fontConfigured As Boolean = False
        Do Until fontConfigured
            Try
                If EditorFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    If Not IsMonospacedFont(EditorFD.Font.Name) AndAlso MessageBox.Show(LocalizationService.ForSection("StarterScript")("NonMonospace.Message"), LocalizationService.ForSection("StarterScript")("Dialog.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                        Exit Sub
                    End If
                    TextBox3.Font = EditorFD.Font
                End If
                fontConfigured = True
            Catch arEx As ArgumentException
                ' The user may have selected a non-TrueType font
                MessageBox.Show(arEx.Message, LocalizationService.ForSection("StarterScript")("Dialog.Title"), MessageBoxButtons.OK, MessageBoxIcon.Warning)
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
        If TextBox1.Text = "" Then
            MessageBox.Show(LocalizationService.ForSection("StarterScript")("Name.Required.Message"), LocalizationService.ForSection("StarterScript")("Dialog.Title"), MessageBoxButtons.OK, MessageBoxIcon.Stop)
            NotWillingToSave = True
            Exit Sub
        End If
        If TextBox2.Text = "" Then
            MessageBox.Show(LocalizationService.ForSection("StarterScript")("Description.Required.Message"), LocalizationService.ForSection("StarterScript")("Dialog.Title"), MessageBoxButtons.OK, MessageBoxIcon.Stop)
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
        TextBox3.Text = Regex.Replace(TextBox3.Text, ControlChars.Tab, "    ")
    End Sub
End Class
