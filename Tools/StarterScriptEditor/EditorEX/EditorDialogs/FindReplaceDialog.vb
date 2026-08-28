Imports StarterScriptEditor.Classes.ColorUtilities
Imports System.Text.RegularExpressions
Imports System.ServiceProcess

Public Class FindReplaceDialog

    Public EditorControl As Control, _
           MyParent As Form

    ' This is invisible and will only serve us for repositioning the caret and determining the line and column
    ' based on that.
    Private InternalTB As New TextBox

    Private CurrentColorMode As ColorThemeMode

    Private _replaceMode As Boolean
    Public Property ReplaceMode() As Boolean
        Get
            Return _replaceMode
        End Get
        Set(ByVal value As Boolean)
            _replaceMode = value
            pnlReplace.Enabled = value
            Text = IIf(value, "Find & Replace in Script Code", "Find in Script Code")

            If ReplaceToggleClickedInUI Then
                ReplaceToggleClickedInUI = False
            Else
                RemoveHandler cbReplaceMode.CheckedChanged, AddressOf cbReplaceMode_CheckedChanged
                cbReplaceMode.Checked = ReplaceMode
                AddHandler cbReplaceMode.CheckedChanged, AddressOf cbReplaceMode_CheckedChanged
            End If
        End Set
    End Property

    Private ReplaceToggleClickedInUI As Boolean

    Private ReferenceExpression As Regex, _
            FindOperationMatches As MatchCollection, _
            SelectedMatchIndex As Integer = 0

    Private Expanded As Boolean, _
            FirstOpen As Boolean = True

    Private MinHeight As Integer = 212 + IIf(IsThemesSvcRunning(), 8, 0)

    Private Sub cbPin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPin.CheckedChanged
        TopMost = cbPin.Checked
        ShowInTaskbar = Not cbPin.Checked
        MinimizeBox = Not cbPin.Checked
    End Sub

    Private Sub FindReplaceDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not TypeOf EditorControl Is TextBox Then Close() : Exit Sub
        MinimumSize = WindowHelper.ScaleSizeLogical(576, 0)

        CurrentColorMode = MainForm.CurrentColorMode
        InternalTB.Multiline = True
        InternalTB.WordWrap = False
        InternalTB.MaxLength = Integer.MaxValue
        SetColorMode()
        If FirstOpen Then Height = MinHeight : FirstOpen = False

        RemoveHandler cbReplaceMode.CheckedChanged, AddressOf cbReplaceMode_CheckedChanged
        cbReplaceMode.Checked = ReplaceMode
        AddHandler cbReplaceMode.CheckedChanged, AddressOf cbReplaceMode_CheckedChanged
        MinimumSize = WindowHelper.ScaleSizeLogical(576, MinHeight)

        ColumnHeader1.Width = WindowHelper.ScaleLogical(60)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(60)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(60)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(362)
    End Sub

    Private Function IsThemesSvcRunning() As Boolean
        Try
            Dim themesService As New ServiceController("Themes")
            Return themesService.Status = ServiceControllerStatus.Running
        Catch ex As Exception
            Return False
        End Try
    End Function

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

        ' If the themes service is running, we don't theme the image
        If Not IsThemesSvcRunning() Then cbPin.Image = IIf(CurrentColorMode = ColorThemeMode.Light, My.Resources.pin, My.Resources.pin_dark)
        cbPin.ImageAlign = IIf(WindowHelper.GetSystemDpi() > 96.0F, ContentAlignment.MiddleCenter, ContentAlignment.BottomRight)

        tbFindContents.BackColor = BackColor
        tbFindContents.ForeColor = ForeColor
        tbReplaceContents.BackColor = BackColor
        tbReplaceContents.ForeColor = ForeColor
        lvResults.BackColor = BackColor
        lvResults.ForeColor = ForeColor
    End Sub

    Private Sub GetAndDisplaySubstringMatches(ByVal Substring As String, Optional ByVal RegexMode As Boolean = False, Optional ByVal CaseMatch As Boolean = False)
        If Substring = "" Then Exit Sub
        lvResults.Items.Clear()
        Label3.Visible = False

        InternalTB.Text = EditorControl.Text

        Dim options As RegexOptions = IIf(CaseMatch, RegexOptions.Compiled, RegexOptions.Compiled Or RegexOptions.IgnoreCase)

        Try
            If Not RegexMode Then Substring = Regex.Escape(Substring)

            ReferenceExpression = New Regex(Substring, options)
            FindOperationMatches = ReferenceExpression.Matches(EditorControl.Text)
            lblStatus.Text = String.Format("{0} occurrence(s) of provided input.", FindOperationMatches.Count)
            If FindOperationMatches.Count = 0 Then
                MessageBox.Show("No matches were found.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                btnFindNext.Enabled = False
                btnFindPrevious.Enabled = False
                Exit Sub
            End If

            If Not Expanded Then btnExpandCollapse.PerformClick()

            ' Display all matches in the list view
            Dim lvItems(FindOperationMatches.Count - 1) As ListViewItem, _
                idx As Integer = 0
            For Each FindOperationMatch As Match In FindOperationMatches
                ' Reposition the "caret" to get line and column
                InternalTB.SelectionStart = FindOperationMatch.Index
                InternalTB.SelectionLength = 0
                InternalTB.Select()

                Dim caretPosition As Integer = InternalTB.SelectionStart, _
                    line As Integer = InternalTB.GetLineFromCharIndex(caretPosition), _
                    column As Integer = caretPosition - InternalTB.GetFirstCharIndexFromLine(line)

                lvItems(idx) = New ListViewItem(New String() {line + 1, column + 1, FindOperationMatch.Length, InternalTB.Lines(line)})

                idx += 1
            Next

            lvResults.Items.AddRange(lvItems)
            ' determine which is our current match based on where we are in the textbox
            For i As Integer = 0 To FindOperationMatches.Count - 1
                If CType(EditorControl, TextBox).SelectionStart < FindOperationMatches(i).Index Then
                    ' Our last successful comparison was the previous match and not the current one.
                    SelectedMatchIndex = i - 1
                    Exit For
                End If
            Next
            If SelectedMatchIndex < 0 Then SelectedMatchIndex = 0
        Catch regexEx As ArgumentException
            MessageBox.Show("A malformed regular expression has been written.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tbFindContents_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbFindContents.TextChanged
        btnFindNext.Enabled = tbFindContents.Text <> ""
        btnFindPrevious.Enabled = tbFindContents.Text <> ""
        btnFindAll.Enabled = tbFindContents.Text <> ""
    End Sub

    Private Sub tbReplaceContents_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbReplaceContents.TextChanged
        btnReplace.Enabled = pnlReplace.Enabled AndAlso tbReplaceContents.Text <> ""
        btnReplaceAll.Enabled = pnlReplace.Enabled AndAlso tbReplaceContents.Text <> ""
    End Sub

    Private Sub btnFindAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindAll.Click
        GetAndDisplaySubstringMatches(tbFindContents.Text, cbRegex.Checked, cbMatchCase.Checked)
    End Sub

    Private Sub btnExpandCollapse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExpandCollapse.Click
        MinimumSize = WindowHelper.ScaleSizeLogical(576, 0)
        Expanded = Not Expanded
        Height = WindowHelper.ScaleLogical(IIf(Expanded, 384, MinHeight))
        MinimumSize = Size
        lvResults.Visible = Expanded
        btnExpandCollapse.Text = IIf(Expanded, "Collapse", "Expand")
    End Sub

    Private Sub DisplayMatch(ByVal MatchIndex As Integer)
        If MatchIndex < 0 OrElse MatchIndex >= FindOperationMatches.Count Then Exit Sub

        Try
            With CType(EditorControl, TextBox)
                .SelectionStart = FindOperationMatches(MatchIndex).Index
                .SelectionLength = FindOperationMatches(MatchIndex).Length
                .Select()
                .ScrollToCaret()
            End With
            MyParent.Focus()
            SelectedMatchIndex = MatchIndex
            If MyParent.Name = "MainForm" Then MainForm.UpdateCaretPosition() ' we know for certain our parent is the mainform
        Catch ex As Exception

        End Try
    End Sub

    Private Function ReplaceOccurrence(ByVal InputText As String, ByVal OccurrenceNumber As Integer, ByVal ReplacementText As String) As String
        If InputText = "" Then Return ""
        If ReplacementText = "" Then Return InputText
        If FindOperationMatches Is Nothing OrElse OccurrenceNumber >= FindOperationMatches.Count Then Return InputText

        Dim targetMatch As Match = FindOperationMatches(OccurrenceNumber)

        Return InputText.Remove(targetMatch.Index, targetMatch.Length).Insert(targetMatch.Index, ReplacementText)
    End Function

    Private Sub ReplaceAllOccurrences(ByVal TextToReplace As String, ByVal ReplacementText As String, Optional ByVal RegexMode As Boolean = False, Optional ByVal MatchCase As Boolean = False)
        If TextToReplace = "" Or ReplacementText = "" Then Exit Sub

        Dim options As RegexOptions = IIf(MatchCase, RegexOptions.Compiled, RegexOptions.Compiled Or RegexOptions.IgnoreCase)

        Try
            If Not RegexMode Then TextToReplace = Regex.Escape(TextToReplace)

            ReferenceExpression = New Regex(TextToReplace, options)
            EditorControl.Text = ReferenceExpression.Replace(EditorControl.Text, ReplacementText)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnFindNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNext.Click
        If FindOperationMatches Is Nothing Then GetAndDisplaySubstringMatches(tbFindContents.Text, cbRegex.Checked, cbMatchCase.Checked)

        Dim newMatchIndex As Integer = SelectedMatchIndex + 1
        ' the new match index could exceed the amount of matches; wrap to 0 if so
        If newMatchIndex >= FindOperationMatches.Count Then
            MessageBox.Show("Reached the end of the code. The find operation will continue from the beginning.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            newMatchIndex = 0
        End If
        lvResults.Items(SelectedMatchIndex).Checked = False
        lvResults.Items(newMatchIndex).Checked = True
        lvResults.Select()
        DisplayMatch(newMatchIndex)
    End Sub

    Private Sub btnFindPrevious_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindPrevious.Click
        If FindOperationMatches Is Nothing Then GetAndDisplaySubstringMatches(tbFindContents.Text, cbRegex.Checked, cbMatchCase.Checked)

        Dim newMatchIndex As Integer = SelectedMatchIndex - 1
        ' the new match index could go below 0; wrap to the highest one if so
        If newMatchIndex < 0 Then
            MessageBox.Show("Reached the beginning of the code. The find operation will continue from the end.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            newMatchIndex = FindOperationMatches.Count - 1
        End If
        lvResults.Items(SelectedMatchIndex).Checked = False
        lvResults.Items(newMatchIndex).Checked = True
        lvResults.Select()
        DisplayMatch(newMatchIndex)
    End Sub

    Private Sub btnReplaceAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReplaceAll.Click
        ReplaceAllOccurrences(tbFindContents.Text, tbReplaceContents.Text, cbRegex.Checked, cbMatchCase.Checked)
    End Sub

    Private Sub lvResults_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvResults.MouseDoubleClick
        Try
            If lvResults.SelectedItems.Count = 1 Then
                DisplayMatch(lvResults.FocusedItem.Index)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub cbReplaceMode_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbReplaceMode.CheckedChanged
        ReplaceToggleClickedInUI = True
        ReplaceMode = Not ReplaceMode
    End Sub

    Private Sub btnReplace_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReplace.Click
        If FindOperationMatches Is Nothing Then GetAndDisplaySubstringMatches(tbFindContents.Text, cbRegex.Checked, cbMatchCase.Checked)
        EditorControl.Text = ReplaceOccurrence(EditorControl.Text, SelectedMatchIndex, tbReplaceContents.Text)
        ' get matches again and switch to the next one
        Dim oldMatchIndex As Integer = SelectedMatchIndex
        GetAndDisplaySubstringMatches(tbFindContents.Text, cbRegex.Checked, cbMatchCase.Checked)
        SelectedMatchIndex = oldMatchIndex
        If FindOperationMatches Is Nothing OrElse FindOperationMatches.Count = 0 Then
            btnReplace.Enabled = False
            btnReplaceAll.Enabled = False
            Exit Sub
        End If
        Label3.Visible = True
        If SelectedMatchIndex >= FindOperationMatches.Count Then
            MessageBox.Show("Reached the end of the code. The find operation will continue from the beginning.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            SelectedMatchIndex = 0
        End If
        DisplayMatch(SelectedMatchIndex)
    End Sub
End Class