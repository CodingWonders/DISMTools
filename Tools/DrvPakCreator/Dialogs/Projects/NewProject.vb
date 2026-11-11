Imports System.Windows.Forms
Imports DrvPakCreator.Helpers

Public Class NewProject

    Public ProjectPath As String = ""

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If tbProjectName.Text = "" Or tbProjectLocation.Text = "" Then
            MessageBox.Show(Me, "You need to specify a name and location for the project.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If

        Cursor = Cursors.WaitCursor
        If Not DriverPackProjectHelper.CreatePackProject(tbProjectName.Text, tbProjectLocation.Text) Then
            MessageBox.Show(Me, "The project could not be created.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Cursor = Cursors.Arrow

        ProjectPath = String.Format("{0}\{1}.pakproj", tbProjectLocation.Text.TrimEnd("\"), tbProjectName.Text)

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        If fbdProjectLocation.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            tbProjectLocation.Text = fbdProjectLocation.SelectedPath
        End If
    End Sub
End Class
