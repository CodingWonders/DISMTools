Imports StarterScriptEditor.Classes.AutoInspection
Imports StarterScriptEditor.Classes.ColorUtilities
Imports System.IO

Public Class AICustomRuleViewer

    Private CurrentColorMode As ColorThemeMode
    Private CustomRules As New List(Of AutoInspectionRule)
    Private IsModified As Boolean

    Private Sub AICustomRuleViewer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CurrentColorMode = MainForm.CurrentColorMode
        SetColorMode()
        LoadCustomRules()

        ColumnHeader1.Width = WindowHelper.ScaleLogical(192)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(344)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(344)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(72)
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

        CustomRuleLV.BackColor = BackColor
        CustomRuleLV.ForeColor = ForeColor
    End Sub

    Private Sub LoadCustomRules()
        Try
            CustomRules = AIHelper.GetScriptInspectionCustomRules()
            ViewCustomRules()
        Catch fnfEx As FileNotFoundException
            Dim creationResponse As DialogResult = MessageBox.Show(LocalizationService.ForSection("StarterScript.CustomRuleViewer.Messages")("CreateFile.Message"), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            Select Case creationResponse
                Case Windows.Forms.DialogResult.Yes
                    AIHelper.SaveCustomRules(New List(Of AutoInspectionRule))
                    LoadCustomRules()
                Case Windows.Forms.DialogResult.No
                    Close()
                    Exit Sub
            End Select
        Catch ex As Exception
            Dim recreationResponse As DialogResult = MessageBox.Show(LocalizationService.ForSection("StarterScript.CustomRuleViewer.Messages")("RecreateFile.Message"), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            Select Case recreationResponse
                Case Windows.Forms.DialogResult.Yes
                    AIHelper.SaveCustomRules(New List(Of AutoInspectionRule))
                    LoadCustomRules()
                Case Windows.Forms.DialogResult.No
                    Close()
                    Exit Sub
            End Select
        End Try
    End Sub

    Private Sub ViewCustomRules()
        CustomRuleLV.Items.Clear()
        Dim crItems(CustomRules.Count - 1) As ListViewItem
        For i As Integer = 0 To CustomRules.Count - 1
            Dim crItem As AutoInspectionRule = CustomRules(i)
            Dim crSeverityStatus As String = ""
            Select Case crItem.RuleSeverity
                Case AutoInspectionRuleSeverity.Low : crSeverityStatus = LocalizationService.ForSection("StarterScript.CustomRule.Common")("Low.Item")
                Case AutoInspectionRuleSeverity.Medium : crSeverityStatus = LocalizationService.ForSection("StarterScript.CustomRule.Common")("Medium.Item")
                Case AutoInspectionRuleSeverity.High : crSeverityStatus = LocalizationService.ForSection("StarterScript.CustomRule.Common")("High.Item")
            End Select
            crItems(i) = New ListViewItem(New String() {crItem.RuleName, crItem.RuleDescription, crItem.RuleExpression, crSeverityStatus})
        Next
        CustomRuleLV.Items.AddRange(crItems)
        ModifyCustomRuleButton.Enabled = False
        DeleteCustomRuleButton.Enabled = False
    End Sub

    Private Sub CustomRuleLV_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomRuleLV.SelectedIndexChanged
        Try
            ModifyCustomRuleButton.Enabled = CustomRuleLV.SelectedItems.Count = 1
            DeleteCustomRuleButton.Enabled = CustomRuleLV.SelectedItems.Count >= 1
        Catch ex As Exception
            ModifyCustomRuleButton.Enabled = False
            DeleteCustomRuleButton.Enabled = False
        End Try
    End Sub

    Private Sub SaveCustomRulesButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveCustomRulesButton.Click
        Dim dtssUserDataPath As String = MainForm.UserDataScriptFolder, _
            crXmlDataPath As String = IIf(dtssUserDataPath <> "", String.Format("{0}\..\sse_config_rules", dtssUserDataPath), "")

        If AIHelper.SaveCustomRules(CustomRules, crXmlDataPath) Then
            IsModified = False
        Else
            MessageBox.Show(LocalizationService.ForSection("StarterScript.CustomRuleViewer.Messages")("SaveFailed.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub RefreshRulesButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshRulesButton.Click
        If IsModified Then
            Dim saveResponse As DialogResult = MessageBox.Show(LocalizationService.ForSection("StarterScript.CustomRuleViewer.Messages")("SaveChanges.Message"), Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            Select Case saveResponse
                Case Windows.Forms.DialogResult.Yes : SaveCustomRulesButton.PerformClick()
                Case Windows.Forms.DialogResult.Cancel : Exit Sub
            End Select
        End If
        LoadCustomRules()
    End Sub

    Private Sub AddCustomRuleButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddCustomRuleButton.Click
        If CustomRuleDetailsDialog.CurrentCustomRule IsNot Nothing Then CustomRuleDetailsDialog.CurrentCustomRule = Nothing
        If CustomRuleDetailsDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            CustomRules.Add(CustomRuleDetailsDialog.CurrentCustomRule)
            IsModified = True
            ViewCustomRules()
        End If
    End Sub

    Private Sub ModifyCustomRuleButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModifyCustomRuleButton.Click
        Try
            Dim selectedIndex As Integer = CustomRuleLV.FocusedItem.Index
            CustomRuleDetailsDialog.CurrentCustomRule = CustomRules(selectedIndex)
            If CustomRuleDetailsDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                CustomRules(selectedIndex) = CustomRuleDetailsDialog.CurrentCustomRule
                IsModified = True
                ViewCustomRules()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DeleteCustomRuleButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteCustomRuleButton.Click
        Try
            If CustomRuleLV.SelectedItems.Count > 1 Then
                For i As Integer = CustomRuleLV.Items.Count - 1 To 0 Step -1
                    If CustomRuleLV.Items(i).Selected Then CustomRules.RemoveAt(i)
                Next
            ElseIf CustomRuleLV.SelectedItems.Count = 1 Then
                CustomRules.RemoveAt(CustomRuleLV.FocusedItem.Index)
            End If
            IsModified = True
            ViewCustomRules()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub AICustomRuleViewer_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If IsModified Then
            Dim saveResponse As DialogResult = MessageBox.Show(LocalizationService.ForSection("StarterScript.CustomRuleViewer.Messages")("SaveChanges.Message"), Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            Select Case saveResponse
                Case Windows.Forms.DialogResult.Yes : SaveCustomRulesButton.PerformClick()
                Case Windows.Forms.DialogResult.Cancel
                    e.Cancel = True
                    Exit Sub
            End Select
        End If
    End Sub
End Class
