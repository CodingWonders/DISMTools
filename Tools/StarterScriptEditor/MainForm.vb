Imports StarterScriptEditor.Classes
Imports StarterScriptEditor.Classes.ColorUtilities
Imports System.IO
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Win32

Public Class MainForm

    Private CurrentScript As StarterScript
    Private SupportedLanguageList As New List(Of String)

    Private UserDataScriptFolder As String

    Private Modified As Boolean
    Private SavedScriptPath As String
    Private NotWillingToSave As Boolean

    Private roMode As Boolean

    Public CurrentColorMode As ColorThemeMode

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
            MsgBox("The script file does not exist.", vbOKOnly + vbExclamation)
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
        Text = String.Format("Starter Script Editor - {0}", Path.GetFileName(SavedScriptPath))

        If (File.GetAttributes(ScriptFile) And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
            MessageBox.Show("This script file has been loaded with read-only privileges. If you make changes to this script, you must save them to a new script file or enable write access for this script.", "Starter Script Editor", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            roMode = True
            ToolStripButton5.Enabled = True
        End If
    End Sub

    Private Sub SaveScriptFile(ByVal ScriptFile As String)
        If ScriptVer < ScriptVersion.Infinity Then
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
            Dim customizableStr As String
            If CurrentScript.OptionsCustomizable Then
                customizableStr = "Yes"
            Else
                customizableStr = "No"
            End If

            File.WriteAllText(ScriptFile, String.Format("Language: {0}{1}" & _
            "Name: {2}{1}" & _
            "Description: {3}{1}" & _
            "Customizable: {4}{1}" & _
            "{5}", CurrentScript.Language, Environment.NewLine, CurrentScript.Name, CurrentScript.Description, customizableStr, CurrentScript.Code), UTF8)

            SavedScriptPath = ScriptFile
            Text = String.Format("Starter Script Editor - {0}", Path.GetFileName(SavedScriptPath))
            Modified = False
            roMode = False
            ToolStripButton5.Enabled = False
            ScriptVer = ScriptVersion.Infinity
        Catch ex As Exception
            MessageBox.Show("Changes could not be saved to the script file. Make sure write access is present in the file. " & CrLf & CrLf & ex.Message & CrLf & CrLf & "To enable write access for this file, use the respective button in the toolbar.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            NotWillingToSave = True
        End Try
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
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
        NotWillingToSave = False
        If Not String.IsNullOrEmpty(SavedScriptPath) AndAlso File.Exists(SavedScriptPath) AndAlso Not roMode Then
            Select Case MessageBox.Show(String.Format("You had previously saved this script to the following location:{0}{0}    {1}{0}{0}Do you want to save changes to this file instead of another file?", _
                                            Environment.NewLine, Path.GetDirectoryName(SavedScriptPath)), _
                                            "Save Script", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                Case Windows.Forms.DialogResult.Yes
                    SaveScriptFile(SavedScriptPath)
                    Exit Sub
                Case Windows.Forms.DialogResult.Cancel
                    NotWillingToSave = True
                    Exit Sub
            End Select
        End If
        If SaveFileDialog1.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then
            NotWillingToSave = True
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
        SaveScriptFile(SaveFileDialog1.FileName)
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
        MsgBox(String.Format("DISMTools Starter Script Editor version {0} (DEBUG)" & CrLf & CrLf & "{1}", _
                My.Application.Info.Version.ToString() & "_" & RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm") , _
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
        MsgBox(String.Format("DISMTools Starter Script Editor version {0}_NET2REL (DEBUG)" & CrLf & CrLf & "{1}", _
                My.Application.Info.Version.ToString(), My.Application.Info.Copyright), _
            vbOKOnly + vbInformation, "About")
#Else
        MsgBox(String.Format("DISMTools Starter Script Editor version {0}_NET2REL" & CrLf & CrLf & "{1}", _
                My.Application.Info.Version.ToString(), My.Application.Info.Copyright), _
            vbOKOnly + vbInformation, "About")
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
            ComboBox1.SelectedIndex = 0
        ElseIf scriptExtension.ToLower() = ".ps1" Then
            ComboBox1.SelectedIndex = 1
        ElseIf expectedVbScriptExtensions.Contains(scriptExtension) Then
            ComboBox1.SelectedIndex = 2
        ElseIf expectedJScriptExtensions.Contains(scriptExtension) Then
            ComboBox1.SelectedIndex = 3
        Else
            MessageBox.Show("This script is not supported by the Starter Script Editor.", "Unrecognized script", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Try
            Dim scriptContents As String = File.ReadAllText(scriptFileName)
            TextBox3.Text = scriptContents
            UpdateCaretPosition()
        Catch ex As Exception
            MessageBox.Show("The contents of the script could not be loaded.", "Could not read file contents", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                    ' Save item
                    ToolStripButton3.PerformClick()
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
            MessageBox.Show("Could not enable write access for this script file. Make sure that the script is not in read-only media.", "Starter Script Editor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CheckBox2_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.MouseHover
        WindowHelper.DisplayToolTip(sender, "Check this option if this script contains settings that can be configured by the user" & CrLf & "after importing the starter script from the Starter Script Browser.")
    End Sub
End Class
