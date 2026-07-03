Imports System.Windows.Forms
Imports System.IO
Imports StarterScriptEditor.Classes
Imports StarterScriptEditor.Classes.ColorUtilities

Public Class BulkScriptConversionDialog

    Public SourceScriptPath As String

    Private ProgressMessage As String
    Private CurrentFile As Integer, _
            FileCount As Integer

    Private CurrentColorMode As ColorThemeMode

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub BulkScriptConversionDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CurrentColorMode = MainForm.CurrentColorMode
        SetColorMode()
        ConverterBW.RunWorkerAsync()
    End Sub

    Private Sub SetColorMode()
        Select Case CurrentColorMode
            Case ColorThemeMode.Light
                WindowHelper.ToggleDarkTitleBar(Handle, False)

                BackColor = Color.FromArgb(239, 239, 242)
                ForeColor = Color.Black
            Case ColorThemeMode.Dark
                WindowHelper.ToggleDarkTitleBar(Handle, True)

                BackColor = Color.FromArgb(32, 32, 32)
                ForeColor = Color.White
        End Select
    End Sub

    Private Sub ConvertStarterScript(ByVal StarterScriptPath As String)
        If File.Exists(StarterScriptPath) Then
            Dim scriptFileContents As String() = File.ReadAllLines(StarterScriptPath)
            Dim CodeBlockStartingIndex As Integer = 3
            If scriptFileContents(3).StartsWith("Customizable:", StringComparison.OrdinalIgnoreCase) Then
                CodeBlockStartingIndex = 4
            End If

            ' If the starting line for the code block is 4 then we're dealing with a Infinity script; don't
            ' process it.
            If CodeBlockStartingIndex = 4 Then Exit Sub

            Dim SourceScript As StarterScript = StarterScriptHelper.LoadScriptFile(StarterScriptPath, 3)
            StarterScriptHelper.SaveStarterScript(StarterScriptPath, SourceScript, ScriptVersion.Infinity)
        End If
    End Sub

    Private Sub ConverterBW_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles ConverterBW.DoWork
        ProgressMessage = "Please wait..."
        ConverterBW.ReportProgress(0)

        If Directory.Exists(SourceScriptPath) Then
            ProgressMessage = "Enumerating starter scripts in source folder..."
            ConverterBW.ReportProgress(5)
            Try
                Dim StarterScriptsInFolder() As String = Directory.GetFiles(SourceScriptPath, "*.dtss", SearchOption.AllDirectories)
                Dim idx As Integer = 0, _
                    files As Integer = StarterScriptsInFolder.Length
                FileCount = files
                For Each StarterScriptInFolder As String In StarterScriptsInFolder
                    ProgressMessage = String.Format("Converting starter script {0}{1}{0}...", ControlChars.Quote, Path.GetFileName(StarterScriptInFolder))
                    ConvertStarterScript(StarterScriptInFolder)
                    CurrentFile = idx + 1
                    ConverterBW.ReportProgress((idx / files) * 100)
                    idx += 1
                Next
            Catch ex As Exception

            End Try
        ElseIf File.Exists(SourceScriptPath) Then
            ProgressMessage = String.Format("Converting starter script {0}{1}{0}...", ControlChars.Quote, Path.GetFileName(SourceScriptPath))
            ConvertStarterScript(SourceScriptPath)
            ConverterBW.ReportProgress(100)
            Threading.Thread.Sleep(100)
        End If
    End Sub

    Private Sub ConverterBW_ProgressChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles ConverterBW.ProgressChanged
        Label2.Text = ProgressMessage
        Label3.Text = String.Format("{0}/{1}", CurrentFile, FileCount)
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub ConverterBW_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ConverterBW.RunWorkerCompleted
        Close()
    End Sub

    Private Sub BulkScriptConversionDialog_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If ConverterBW.IsBusy Then
            e.Cancel = True
            Exit Sub
        End If
    End Sub
End Class
