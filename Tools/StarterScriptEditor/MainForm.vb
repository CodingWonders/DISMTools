Imports StarterScriptEditor.Classes
Imports System.IO
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.ControlChars

Public Class MainForm

    Private CurrentScript As StarterScript
    Private SupportedLanguageList As New List(Of String)

    Private UserDataScriptFolder As String

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
    End Sub

    Private Sub LoadScriptFile(ByVal ScriptFile As String)
        If Not File.Exists(ScriptFile) Then
            MsgBox("The script file does not exist.", vbOKOnly + vbExclamation)
            Exit Sub
        End If

        Dim scriptFileContents As String() = File.ReadAllLines(ScriptFile)

        ' Script Format:
        ' <Language>
        ' <Name>
        ' <Description>
        ' <code>
        Dim scriptLang As String = scriptFileContents(0).Replace("Language: ", "")
        Dim scriptName As String = scriptFileContents(1).Replace("Name: ", "")
        Dim scriptDescription As String = scriptFileContents(2).Replace("Description: ", "")
#If VBC_VER >= 9.0 Then
        CurrentScript = New StarterScript(scriptName, scriptDescription, scriptLang, String.Join(ControlChars.CrLf, New List(Of String)(scriptFileContents).Skip(3).ToArray()))
#Else
        ' NDPv2 and earlier do not support LINQ statements.
        Dim ScriptCodeLines As New List(Of String)
        For x As Integer = 3 To scriptFileContents.Length - 1
            ScriptCodeLines.Add(scriptFileContents(x))
        Next
        CurrentScript = New StarterScript(scriptName, scriptDescription, scriptLang, String.Join(ControlChars.CrLf, ScriptCodeLines.ToArray()))
#End If
    End Sub

    Private Sub SaveScriptFile(ByVal ScriptFile As String)
        If File.Exists(ScriptFile) Then
            Try
                File.Delete(ScriptFile)
            Catch ex As Exception
                ' ignore these
            End Try
        End If

        Try
            File.WriteAllText(ScriptFile, String.Format("Language: {0}{1}" & _
            "Name: {2}{1}" & _
            "Description: {3} {1}" & _
            "{4}", CurrentScript.Language, Environment.NewLine, CurrentScript.Name, CurrentScript.Description, CurrentScript.Code), UTF8)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        SaveFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Environment.GetCommandLineArgs().Length = 2 Then
            ' The second parameter (counting the app path) determines
            ' the path to a DT UserData folder.
            Dim userDataPath As String = Environment.GetCommandLineArgs()(1).Replace("/userdata=", "")

            If Directory.Exists(userDataPath) Then
                UserDataScriptFolder = userDataPath
            End If
        End If
        SaveFileDialog1.InitialDirectory = UserDataScriptFolder

        SupportedLanguageList.AddRange(New String(1) {"batch", "powershell"})
        CurrentScript = GetNewStarterScript()
        UpdateCaretPosition()
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        CurrentScript = GetNewStarterScript()
        UpdateScriptProperties()
    End Sub

    Private Sub OpenFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        LoadScriptFile(OpenFileDialog1.FileName)
        UpdateScriptProperties()
    End Sub

    Private Sub SaveFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        SaveScriptFile(SaveFileDialog1.FileName)
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        CurrentScript.Name = TextBox1.Text
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        CurrentScript.Description = TextBox2.Text
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        CurrentScript.Language = ComboBox1.SelectedItem
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        CurrentScript.Code = TextBox3.Text
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
        Dim scriptExtension As String = Path.GetExtension(scriptFileName)

        Dim expectedBatchExtensions As New List(Of String)
        expectedBatchExtensions.AddRange(New String(2) {".bat", ".cmd", ".nt"})
        If expectedBatchExtensions.Contains(scriptExtension) Then
            ComboBox1.SelectedIndex = 0
        ElseIf scriptExtension.ToLower() = ".ps1" Then
            ComboBox1.SelectedIndex = 1
        Else
            MessageBox.Show("This script is not supported by the Starter Script Editor.", "Unrecognized script", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Try
            Dim scriptContents As String = File.ReadAllText(scriptFileName)
            TextBox3.Text = scriptContents
        Catch ex As Exception
            MessageBox.Show("The contents of the script could not be loaded.", "Could not read file contents", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        TextBox3.WordWrap = CheckBox1.Checked
        Label6.Visible = Not CheckBox1.Checked
        UpdateCaretPosition()
    End Sub

    Private Sub TextBox3_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox3.KeyUp
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
End Class
