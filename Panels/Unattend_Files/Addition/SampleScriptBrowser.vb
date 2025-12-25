Imports System.Windows.Forms
Imports System.IO

Public Class SampleScriptBrowser

    Public FinalScriptCode As String
    Public FinalScriptLanguage As String
    Public FinalScriptStage As Integer

    Private SysConfigScripts As New List(Of StarterScript)
    Private FirstUserLogonScripts As New List(Of StarterScript)
    Private UserFirstLogonScripts As New List(Of StarterScript)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Function ParseStarterScript(ScriptPath As String) As StarterScript
        DynaLog.LogMessage("Preparing to read starter script...")
        DynaLog.LogMessage("- Script Path: " & ScriptPath)

        Dim starterScript As StarterScript = Nothing
        If Not File.Exists(ScriptPath) Then Return Nothing

        DynaLog.LogMessage("This file exists. Beginning to read...")

        Try
            Dim scriptFileContents As String() = File.ReadAllLines(ScriptPath)

            ' Script Format:
            ' <Language>
            ' <Name>
            ' <Description>
            ' <code>
            Dim scriptLang As String = scriptFileContents(0).Replace("Language: ", "")
            Dim scriptName As String = scriptFileContents(1).Replace("Name: ", "")
            Dim scriptDescription As String = scriptFileContents(2).Replace("Description: ", "")

            starterScript = New StarterScript(scriptName, scriptDescription, scriptLang, String.Join(ControlChars.CrLf, scriptFileContents.Skip(3).ToArray()))
            If starterScript IsNot Nothing Then DynaLog.LogMessage(starterScript.ToString())
        Catch ex As Exception
            DynaLog.LogMessage("Could not read this file. Error message: " & ex.Message)
        End Try

        Return starterScript
    End Function

    Private Function LoadAllStarterScripts() As Boolean
        DynaLog.LogMessage("Preparing to load all scripts...")

        ' First we check if we have a script collection
        If Not Directory.Exists(Path.Combine(Application.StartupPath, "AutoUnattend", "StarterScripts")) Then
            DynaLog.LogMessage("The starter script directory does not exist.")
            ' we can't continue
            Return False
        End If

        ' Now, we load the ones that are applied during system configuration
        For Each SysConfigScript In Directory.GetFiles(Path.Combine(Application.StartupPath, "AutoUnattend", "StarterScripts", "DuringSystemConfiguration"), "*.dtss")
            SysConfigScripts.Add(ParseStarterScript(SysConfigScript))
        Next

        ' We do the same for the other 2 collections
        For Each FirstUserLogonScript In Directory.GetFiles(Path.Combine(Application.StartupPath, "AutoUnattend", "StarterScripts", "WhenFirstUserLogsOn"), "*.dtss")
            FirstUserLogonScripts.Add(ParseStarterScript(FirstUserLogonScript))
        Next

        For Each UserFirstLogonScript In Directory.GetFiles(Path.Combine(Application.StartupPath, "AutoUnattend", "StarterScripts", "WhenUsersLogOnForFirstTime"), "*.dtss")
            UserFirstLogonScripts.Add(ParseStarterScript(UserFirstLogonScript))
        Next

        Return True
    End Function

    Private Sub ShowScriptsInStage(StageContext As Integer)
        ListView1.Items.Clear()
        Select Case StageContext
            Case 0
                For Each scriptObj In SysConfigScripts.Where(Function(script) script IsNot Nothing).ToList()
                    ListView1.Items.Add(scriptObj.Name)
                Next
            Case 1
                For Each scriptObj In FirstUserLogonScripts.Where(Function(script) script IsNot Nothing).ToList()
                    ListView1.Items.Add(scriptObj.Name)
                Next
            Case 2
                For Each scriptObj In UserFirstLogonScripts.Where(Function(script) script IsNot Nothing).ToList()
                    ListView1.Items.Add(scriptObj.Name)
                Next
        End Select
        FinalScriptStage = StageContext
    End Sub

    Private Function GetScriptFromIndex(index As Integer) As StarterScript
        Try
            Select Case FinalScriptStage
                Case 0
                    Return SysConfigScripts(index)
                Case 1
                    Return FirstUserLogonScripts(index)
                Case 2
                    Return UserFirstLogonScripts(index)
            End Select
        Catch ex As Exception

        End Try
        Return Nothing
    End Function

    Private Sub SampleScriptBrowser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Clear existing items
        SysConfigScripts.Clear()
        FirstUserLogonScripts.Clear()
        UserFirstLogonScripts.Clear()

        ' Reset screens and get rid of listview items
        ScriptDetailsPanel.Visible = False
        ListView1.Items.Clear()
        ' this keeps on being enabled; disable it
        OK_Button.Enabled = False

        If Not LoadAllStarterScripts() Then
            ' starter scripts could not be loaded. stop
            MessageBox.Show("The starter scripts could not be loaded.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            DialogResult = Windows.Forms.DialogResult.Cancel
            Close()
            Exit Sub
        End If

        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        RichTextBox1.BackColor = BackColor
        RichTextBox1.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, CurrentTheme.IsDark)

        If ComboBox1.SelectedIndex = FinalScriptStage Then
            ' force showing again
            ShowScriptsInStage(FinalScriptStage)
        Else
            ComboBox1.SelectedIndex = FinalScriptStage
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ShowScriptsInStage(ComboBox1.SelectedIndex)
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            If ListView1.SelectedItems.Count = 1 Then
                Dim script As StarterScript = GetScriptFromIndex(ListView1.FocusedItem.Index)

                If script Is Nothing Then Exit Sub

                Label3.Text = script.Name
                Label4.Text = script.Description
                Label5.Text = String.Format("Language: {0}", script.Language)
                RichTextBox1.Text = script.ScriptCode

                FinalScriptCode = script.ScriptCode
                FinalScriptLanguage = script.Language
            End If

            ScriptDetailsPanel.Visible = (ListView1.SelectedItems.Count = 1)
            OK_Button.Enabled = (ListView1.SelectedItems.Count = 1)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ListView1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDoubleClick
        Try
            If ListView1.SelectedItems.Count = 1 Then
                OK_Button.PerformClick()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
