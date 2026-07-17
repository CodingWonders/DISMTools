Imports System.Windows.Forms
Imports StarterScriptEditor.Classes.AutoInspection
Imports StarterScriptEditor.Classes.ColorUtilities
Imports System.Text.RegularExpressions
Imports System.IO

Public Class CustomRuleDetailsDialog

    Private CurrentColorMode As ColorThemeMode

    Public CurrentCustomRule As AutoInspectionRule

    Private ExpressionTester As Regex
    Private ExpressionMatches As MatchCollection

    Private CurrentSelectedMatchIndex As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CustomRuleDetailsDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        AcceptButton = OK_Button
        CurrentColorMode = MainForm.CurrentColorMode
        SetColorMode()

        If CurrentCustomRule Is Nothing Then
            ' we're creating a new rule
            CurrentCustomRule = New AutoInspectionRule()
            Text = "Add Custom Inspection Rule"
        Else
            Text = "Modify Custom Inspection Rule"
        End If

        RuleNameTextBox.Text = CurrentCustomRule.RuleName
        RuleDescriptionTextBox.Text = CurrentCustomRule.RuleDescription
        RuleSeverityComboBox.SelectedIndex = CurrentCustomRule.RuleSeverity - 1
        RuleExpressionTextBox.Text = CurrentCustomRule.RuleExpression
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

        RuleNameTextBox.BackColor = BackColor
        RuleNameTextBox.ForeColor = ForeColor
        RuleDescriptionTextBox.BackColor = BackColor
        RuleDescriptionTextBox.ForeColor = ForeColor
        RuleExpressionTextBox.BackColor = BackColor
        RuleExpressionTextBox.ForeColor = ForeColor
        RuleExpressionTesterTextBox.BackColor = BackColor
        RuleExpressionTesterTextBox.ForeColor = ForeColor
        RuleSeverityComboBox.BackColor = BackColor
        RuleSeverityComboBox.ForeColor = ForeColor
        ComboBox2.BackColor = BackColor
        ComboBox2.ForeColor = ForeColor
        GroupBox1.BackColor = BackColor
        GroupBox1.ForeColor = ForeColor
    End Sub

    Private Sub RuleNameTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RuleNameTextBox.TextChanged
        If CurrentCustomRule IsNot Nothing Then CurrentCustomRule.RuleName = RuleNameTextBox.Text
    End Sub

    Private Sub RuleDescriptionTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RuleDescriptionTextBox.TextChanged
        If CurrentCustomRule IsNot Nothing Then CurrentCustomRule.RuleDescription = RuleDescriptionTextBox.Text
    End Sub

    Private Sub RuleSeverityComboBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RuleSeverityComboBox.SelectedIndexChanged
        If CurrentCustomRule IsNot Nothing Then CurrentCustomRule.RuleSeverity = RuleSeverityComboBox.SelectedIndex + 1
    End Sub

    Private Sub RuleExpressionTextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RuleExpressionTextBox.TextChanged
        If CurrentCustomRule IsNot Nothing Then CurrentCustomRule.RuleExpression = RuleExpressionTextBox.Text
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedIndex = 1 Then
            RuleExpressionTesterTextBox.Text = My.Resources.ApiKeyLeakTemplate
        End If
    End Sub

    Private Sub SelectMatch(ByVal MatchIndex As Integer)
        If MatchIndex < 0 OrElse MatchIndex >= ExpressionMatches.Count Then Exit Sub

        Try
            Dim selectedMatch As Match = ExpressionMatches(MatchIndex)
            RuleExpressionTesterTextBox.Select(selectedMatch.Index, selectedMatch.Length)
            RuleExpressionTesterTextBox.Focus()
            RuleExpressionTesterTextBox.ScrollToCaret()

            CurrentSelectedMatchIndex = MatchIndex

            ' enable/disable buttons
            RegexPrevMatchButton.Enabled = MatchIndex > 0
            RegexNextMatchButton.Enabled = MatchIndex < ExpressionMatches.Count - 1
        Catch ex As Exception

        End Try
    End Sub

    Private Sub RegexTesterButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RegexTesterButton.Click
        Try
            ExpressionTester = New Regex(RuleExpressionTextBox.Text, RegexOptions.Compiled Or RegexOptions.Multiline Or RegexOptions.IgnoreCase)
            ExpressionMatches = ExpressionTester.Matches(RuleExpressionTesterTextBox.Text)

            MatchCountLabel.Text = String.Format("{0} match{1}", ExpressionMatches.Count, IIf(ExpressionMatches.Count <> 1, "es", ""))

            ' Select the first match.
            SelectMatch(0)
        Catch regexEx As ArgumentException
            MessageBox.Show("This expression is malformed.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            MatchCountLabel.Text = "0 matches"

            RegexPrevMatchButton.Enabled = False
            RegexNextMatchButton.Enabled = False
        Catch ex As Exception
            ' ignore, but still care about
            MatchCountLabel.Text = "0 matches"

            RegexPrevMatchButton.Enabled = False
            RegexNextMatchButton.Enabled = False
        End Try
    End Sub

    Private Sub RegexPrevMatchButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RegexPrevMatchButton.Click
        SelectMatch(CurrentSelectedMatchIndex - 1)
    End Sub

    Private Sub RegexNextMatchButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RegexNextMatchButton.Click
        SelectMatch(CurrentSelectedMatchIndex + 1)
    End Sub

    Private Sub RegexCheatSheetButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RegexCheatSheetButton.Click
        Try
            Dim cheatsheetPath As String = Path.Combine(Path.GetTempPath(), "cheatsheet.txt"), _
                notepadPath As String = String.Format("{0}\system32\notepad.exe", Environment.GetEnvironmentVariable("WINDIR"))
            File.WriteAllText(cheatsheetPath, My.Resources.RegexCheatsheet)
            Process.Start(notepadPath, ControlChars.Quote & cheatsheetPath & ControlChars.Quote)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub RuleExpressionTesterTextBox_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RuleExpressionTesterTextBox.Enter
        ' Prevent ENTER from being intercepted.
        AcceptButton = Nothing
    End Sub

    Private Sub RuleExpressionTesterTextBox_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RuleExpressionTesterTextBox.Leave
        AcceptButton = OK_Button
    End Sub
End Class
