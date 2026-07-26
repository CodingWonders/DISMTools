Imports System.Windows.Forms

Public Class ScriptReorderDialog

    Public ScriptSet As New List(Of PostInstallScript)
    Private CurrentIndex As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ScriptReorderDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        RichTextBox1.BackColor = BackColor
        RichTextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        RichTextBox1.Text = ""
        ShowScriptSet()
    End Sub

    Private Sub ShowScriptSet()
        ListView1.Items.Clear()
        ListView1.Items.AddRange(ScriptSet.Select(Function(script) New ListViewItem(New String() {String.Format(LocalizationService.ForSection("ScriptReorder")("Script.Label"), ScriptSet.IndexOf(script) + 1)})).ToArray())
    End Sub

    Private Sub MoveScript(SourceIndex As Integer, NewIndex As Integer)
        Dim selectedIndex As Integer = SourceIndex
        Dim scriptToMove As PostInstallScript = ScriptSet.ElementAtOrDefault(selectedIndex)
        If scriptToMove Is Nothing Then Exit Sub

        ScriptSet.Remove(scriptToMove)
        ScriptSet.Insert(NewIndex, scriptToMove)
        ShowScriptSet()
        RemoveHandler ListView1.SelectedIndexChanged, AddressOf ListView1_SelectedIndexChanged
        ListView1.Items(NewIndex).Selected = True
        ListView1.Select()
        CurrentIndex = NewIndex
        AddHandler ListView1.SelectedIndexChanged, AddressOf ListView1_SelectedIndexChanged
        ToggleButtonState(NewIndex)
    End Sub

    Private Sub ToggleButtonState(ReferenceIndex As Integer)
        MoveFirstBtn.Enabled = ReferenceIndex > 0
        MovePreviousBtn.Enabled = ReferenceIndex > 0
        MoveNextBtn.Enabled = ReferenceIndex < ScriptSet.Count - 1
        MoveLastBtn.Enabled = ReferenceIndex < ScriptSet.Count - 1
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Panel6.Enabled = ListView1.SelectedItems.Count = 1
        RichTextBox1.Text = ""
        Try
            RichTextBox1.Text = ScriptSet.ElementAtOrDefault(ListView1.FocusedItem.Index).ScriptContents

            ' Disable the buttons themselves accordingly
            ToggleButtonState(ListView1.FocusedItem.Index)
            CurrentIndex = ListView1.FocusedItem.Index
        Catch ex As Exception

        End Try
    End Sub

    Private Sub MoveFirstBtn_Click(sender As Object, e As EventArgs) Handles MoveFirstBtn.Click
        MoveScript(CurrentIndex, 0)
        ToggleButtonState(0)
    End Sub

    Private Sub MovePreviousBtn_Click(sender As Object, e As EventArgs) Handles MovePreviousBtn.Click
        MoveScript(CurrentIndex, CurrentIndex - 1)
        ToggleButtonState(CurrentIndex)
    End Sub

    Private Sub MoveNextBtn_Click(sender As Object, e As EventArgs) Handles MoveNextBtn.Click
        MoveScript(CurrentIndex, CurrentIndex + 1)
        ToggleButtonState(CurrentIndex)
    End Sub

    Private Sub MoveLastBtn_Click(sender As Object, e As EventArgs) Handles MoveLastBtn.Click
        MoveScript(CurrentIndex, ScriptSet.Count - 1)
        ToggleButtonState(CurrentIndex)
    End Sub

    Private Sub MoveFirstBtn_MouseHover(sender As Object, e As EventArgs) Handles MoveFirstBtn.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("ScriptReorder")("Move.Selected.Top.Label"))
    End Sub

    Private Sub MovePreviousBtn_MouseHover(sender As Object, e As EventArgs) Handles MovePreviousBtn.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("ScriptReorder")("Move.Selected.Previous.Label"))
    End Sub

    Private Sub MoveNextBtn_MouseHover(sender As Object, e As EventArgs) Handles MoveNextBtn.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("ScriptReorder")("Move.Selected.Next.Label"))
    End Sub

    Private Sub MoveLastBtn_MouseHover(sender As Object, e As EventArgs) Handles MoveLastBtn.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("ScriptReorder")("Move.Selected.Bottom.Label"))
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        RichTextBox1.WordWrap = CheckBox1.Checked
    End Sub
End Class
