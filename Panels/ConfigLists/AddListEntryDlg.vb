Imports System.Windows.Forms

Public Class AddListEntryDlg

    Public IsForExclusionList As Boolean

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If TextBox1.Text = "" Then Exit Sub
        DynaLog.LogMessage("Typed Path: " & TextBox1.Text)
        DynaLog.LogMessage("Checking if item is going to be in the exclusion list...")
        If IsForExclusionList Then
            DynaLog.LogMessage("Item is for exclusion list. Checking validity of paths...")
            ' Check if entry contains wildcard characters and if it begins with a \
            If TextBox1.Text.Contains("*") And TextBox1.Text.StartsWith("\") Then
                DynaLog.LogMessage("Item starts with a backslash and has a wildcard character. This is not valid.")
                MsgBox(LocalizationService.ForSection("ConfigLists.AddEntry")("Start.Backslash.Message"), vbOKOnly + vbExclamation, Text)
                Exit Sub
            End If
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub AddListEntryDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = LocalizationService.ForSection("AddListEntry")("Entry.Label")
        Button1.Text = LocalizationService.ForSection("AddListEntry")("Browse.Button")
        OK_Button.Text = LocalizationService.ForSection("AddListEntry")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("AddListEntry")("Cancel.Button")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
    End Sub
End Class
