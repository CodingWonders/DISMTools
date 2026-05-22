Imports System.Windows.Forms

Public Class ImageFilePickerDialog

    Public SourceImageFileRepoPath As String
    Public SelectedImageFilePath As String
    Private SelectedImageFileName As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        SelectedImageFilePath = Path.Combine(SourceImageFileRepoPath, SelectedImageFileName)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImageFilePickerDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ListView1.Items.Clear()
        Dim ImageFiles As IEnumerable(Of String) = Directory.EnumerateFiles(SourceImageFileRepoPath, "*.*", SearchOption.TopDirectoryOnly).Where(Function(file) {".wim", ".esd", ".swm", ".ffu"}.Contains(Path.GetExtension(file).ToLower()))
        ListView1.Items.AddRange(ImageFiles.Select(Function(ImageFile) New ListViewItem(New String() {Path.GetFileName(ImageFile)})).ToArray())
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        OK_Button.Enabled = ListView1.SelectedItems.Count = 1
        Try
            SelectedImageFileName = ListView1.FocusedItem.Text
        Catch ex As Exception

        End Try
    End Sub
End Class
