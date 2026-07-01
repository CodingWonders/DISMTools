Imports System.Windows.Forms
Imports StarterScriptEditor.Classes.AutoInspection
Imports StarterScriptEditor.Classes.ColorUtilities

Public Class InspectionProgressDialog

    Public ScriptCode As String
    Public InspectionResults As List(Of AutoInspectionResult)

    Private CurrentRuleName As String

    Private CurrentColorMode As ColorThemeMode

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub InspectionProgressDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label2.Text = ""
        ProgressBar1.Value = 0
        CurrentColorMode = MainForm.CurrentColorMode
        SetColorMode()
        WindowHelper.DisableCloseCapability(Handle)
        InspectorBW.RunWorkerAsync()
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

    Private Sub UpdateProgress(ByVal reportedRule As AutoInspectionProgressReport)
        CurrentRuleName = reportedRule.RuleName
        InspectorBW.ReportProgress(reportedRule.Percentage)
    End Sub

    Private Sub InspectorBW_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles InspectorBW.DoWork
        InspectionResults = AIHelper.GetScriptCodeSecurityViolations(ScriptCode, AddressOf UpdateProgress)
    End Sub

    Private Sub InspectorBW_ProgressChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles InspectorBW.ProgressChanged
        Label2.Text = String.Format("Performing check {0}{1}{0}", ControlChars.Quote, CurrentRuleName)
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub InspectorBW_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles InspectorBW.RunWorkerCompleted
        Close()
    End Sub
End Class
