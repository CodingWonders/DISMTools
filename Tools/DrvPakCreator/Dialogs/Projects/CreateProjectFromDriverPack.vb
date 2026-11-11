Imports System.Windows.Forms
Imports DrvPakCreator.Helpers
Imports System.IO

Public Class CreateProjectFromDriverPack

    Public PackLocation As String
    Public ProjectPath As String = ""

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If tbSourceDriverPack.Text = "" OrElse Not File.Exists(tbSourceDriverPack.Text) Then
            MessageBox.Show(Me, "You need to specify the location of the driver pack and make sure that it exists.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If

        If tbProjectName.Text = "" Or tbProjectLocation.Text = "" Then
            MessageBox.Show(Me, "You need to specify a name and location for the project.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If

        Cursor = Cursors.WaitCursor
        If Not DriverPackProjectHelper.CreatePackProject(tbProjectName.Text, tbProjectLocation.Text, False) Then
            MessageBox.Show(Me, "The project could not be created.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Cursor = Cursors.Arrow
        ProjectPath = String.Format("{0}\{1}.pakproj", tbProjectLocation.Text.TrimEnd("\"), tbProjectName.Text)
        PackLocation = tbSourceDriverPack.Text

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

    Private Sub btnBrowsePack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowsePack.Click
        If ofdDriverPackLocation.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            If DriverPackHelper.IsPackValid(ofdDriverPackLocation.FileName) Then
                tbSourceDriverPack.Text = ofdDriverPackLocation.FileName
            Else
                MessageBox.Show(Me, "This Driver Pack is not valid.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Private Sub CreateProjectFromDriverPack_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor

        tbSourceDriverPack.BackColor = BackColor
        tbSourceDriverPack.ForeColor = ForeColor
        tbProjectName.BackColor = BackColor
        tbProjectName.ForeColor = ForeColor
        tbProjectLocation.BackColor = BackColor
        tbProjectLocation.ForeColor = ForeColor

        GroupBox1.ForeColor = ForeColor

        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)
    End Sub
End Class
