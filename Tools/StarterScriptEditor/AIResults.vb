Imports StarterScriptEditor.Classes.AutoInspection
Imports StarterScriptEditor.Classes.ColorUtilities
Imports System.ComponentModel

Public Class AIResults

    Private CurrentColorMode As ColorThemeMode

    Public Results As New List(Of AutoInspectionResult)
    Private FilteredResults As New List(Of AutoInspectionResult)
    Private IsViewFiltered As Boolean

    ' Variables for counting based on severity level
    Private HighSeverityViolations As Long, _
            MediumSeverityViolations As Long, _
            LowSeverityViolations As Long

    Private Const WINDOW_PIN_TOPLEFT As Integer = 0, _
                  WINDOW_PIN_TOPRIGHT As Integer = 1, _
                  WINDOW_PIN_BOTTOMLEFT As Integer = 2, _
                  WINDOW_PIN_BOTTOMRIGHT As Integer = 3

    Private Sub AIResults_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Results.Count < 1 Then
            MessageBox.Show("This code does not appear to have any security violations.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Close()
            Exit Sub
        End If

        CurrentColorMode = MainForm.CurrentColorMode
        SetColorMode()

        Results.Sort(AddressOf SortResults)
        DataGridView1.DataSource = Results

        ' Count violations based on level
        HighSeverityViolations = 0
        MediumSeverityViolations = 0
        LowSeverityViolations = 0
        For Each Violation As AutoInspectionResult In Results
            Select Case Violation.ScannedRule.RuleSeverity
                Case AutoInspectionRuleSeverity.High : HighSeverityViolations += 1
                Case AutoInspectionRuleSeverity.Medium : MediumSeverityViolations += 1
                Case AutoInspectionRuleSeverity.Low : LowSeverityViolations += 1
            End Select
        Next
        cbHighViolations.Text = String.Format("{0} high-severity violations", HighSeverityViolations)
        cbMediumViolations.Text = String.Format("{0} medium-severity violations", MediumSeverityViolations)
        cbLowViolations.Text = String.Format("{0} low-severity violations", LowSeverityViolations)

        ' Reset filter checkboxes
        RemoveHandler cbHighViolations.CheckedChanged, AddressOf ToggleViolationFilters
        RemoveHandler cbMediumViolations.CheckedChanged, AddressOf ToggleViolationFilters
        RemoveHandler cbLowViolations.CheckedChanged, AddressOf ToggleViolationFilters
        cbHighViolations.Checked = True
        cbMediumViolations.Checked = True
        cbLowViolations.Checked = True
        AddHandler cbHighViolations.CheckedChanged, AddressOf ToggleViolationFilters
        AddHandler cbMediumViolations.CheckedChanged, AddressOf ToggleViolationFilters
        AddHandler cbLowViolations.CheckedChanged, AddressOf ToggleViolationFilters

        IsViewFiltered = False
        FilteredResults.Clear()
        lblViolationCount.Text = String.Format("{0} security violation(s)", DataGridView1.Rows.Count)

        DataGridView1.Columns(0).Width = WindowHelper.ScaleLogical(64)
        DataGridView1.Columns(1).Width = WindowHelper.ScaleLogical(840)
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

        DataGridView1.DefaultCellStyle.BackColor = BackColor
        DataGridView1.DefaultCellStyle.ForeColor = ForeColor
    End Sub

    Private Function SortResults(ByVal result1 As AutoInspectionResult, ByVal result2 As AutoInspectionResult) As Integer
        Return result1.OccurrenceIndex.CompareTo(result2.OccurrenceIndex)
    End Function

    Private Sub DataGridView1_CellFormatting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting
        Dim result As AutoInspectionResult = CType(DataGridView1.Rows(e.RowIndex).DataBoundItem, AutoInspectionResult)
        Select Case DataGridView1.Columns(e.ColumnIndex).Name
            Case "ScannedRuleSeverityColumn"
                Select Case result.ScannedRule.RuleSeverity
                    Case AutoInspectionRuleSeverity.Low : e.Value = My.Resources.inspect_low_severity
                    Case AutoInspectionRuleSeverity.Medium : e.Value = My.Resources.inspect_medium_severity
                    Case AutoInspectionRuleSeverity.High : e.Value = My.Resources.inspect_high_severity
                End Select
                e.FormattingApplied = True
            Case "ScannedRuleDataGridViewTextBoxColumn"
                e.Value = result.ScannedRule.RuleDescription
                e.FormattingApplied = True
        End Select
    End Sub

    Private Sub DataGridView1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DataGridView1.MouseDoubleClick
        ' If a row is selected, we move the caret in the main form to the character
        If DataGridView1.SelectedRows.Count = 1 Then
            If Not CheckBox1.Checked Then CheckBox1.Checked = True
            Dim result As AutoInspectionResult = CType(DataGridView1.Rows(DataGridView1.CurrentRow.Index).DataBoundItem, AutoInspectionResult)
            MainForm.TextBox3.Select(result.OccurrenceIndex, result.OccurrenceLength)
            MainForm.Focus()
            MainForm.TextBox3.Focus()
            MainForm.TextBox3.ScrollToCaret()

            MainForm.UpdateCaretPosition()
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        TopMost = CheckBox1.Checked
        If CheckBox1.Checked AndAlso DataGridView1.Rows.Count > 0 Then
            If AIResultWindowPinDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                Dim PMBounds As Rectangle = Screen.PrimaryScreen.Bounds

                Select Case AIResultWindowPinDialog.PinMode
                    Case WINDOW_PIN_TOPLEFT : Location = New Point(16, 16)
                    Case WINDOW_PIN_TOPRIGHT : Location = New Point(PMBounds.Width - Width - 16, 16)
                    Case WINDOW_PIN_BOTTOMLEFT : Location = New Point(16, PMBounds.Height - Height - 48)
                    Case WINDOW_PIN_BOTTOMRIGHT : Location = New Point(PMBounds.Width - Width - 16, PMBounds.Height - Height - 48)
                End Select
            End If
        End If
    End Sub

    Private Sub FilterViolations(ByVal FilterByHigh As Boolean, ByVal FilterByMedium As Boolean, ByVal FilterByLow As Boolean)
        IsViewFiltered = FilterByHigh AndAlso FilterByMedium AndAlso FilterByLow
        DataGridView1.DataSource = Nothing
        FilteredResults.Clear()

        If IsViewFiltered Then
            ' don't filter; just bind the grid view to the full source
            DataGridView1.DataSource = Results
            Exit Sub
        End If

        For Each Violation As AutoInspectionResult In Results
            If FilterByHigh AndAlso Violation.ScannedRule.RuleSeverity = AutoInspectionRuleSeverity.High Then FilteredResults.Add(Violation)
            If FilterByMedium AndAlso Violation.ScannedRule.RuleSeverity = AutoInspectionRuleSeverity.Medium Then FilteredResults.Add(Violation)
            If FilterByLow AndAlso Violation.ScannedRule.RuleSeverity = AutoInspectionRuleSeverity.Low Then FilteredResults.Add(Violation)
        Next
        DataGridView1.DataSource = FilteredResults
    End Sub

    Private Sub ToggleViolationFilters(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbLowViolations.CheckedChanged, cbMediumViolations.CheckedChanged, cbHighViolations.CheckedChanged
        FilterViolations(cbHighViolations.Checked, cbMediumViolations.Checked, cbLowViolations.Checked)
    End Sub

    Private Sub CustomRulesButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomRulesButton.Click
        AICustomRuleViewer.Show()
    End Sub
End Class