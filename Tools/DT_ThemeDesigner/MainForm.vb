Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class MainForm

    Public NewTheme As Theme

    Private UserDataScriptFolder As String

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GetArguments()
        SaveFileDialog1.InitialDirectory = UserDataScriptFolder
        NewTheme = GetNewTheme()
        ChangeColorPreviews()
        LoadCurrentTheme()
    End Sub

    Private Sub GetArguments()
        Dim args As String() = Environment.GetCommandLineArgs()
        If args.Length <= 1 Then Exit Sub

        For Each arg As String In args
            If arg.StartsWith("/userdata", StringComparison.OrdinalIgnoreCase) Then
                ' This parameter determines the path to a DT UserData folder.
                Dim userDataPath As String = arg.Replace("/userdata=", "")

                If Directory.Exists(userDataPath) Then
                    UserDataScriptFolder = userDataPath
                End If
            End If
        Next
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        NewTheme.Name = TextBox1.Text
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        NewTheme.IsDark = CheckBox1.Checked
        LoadCurrentTheme()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ColorDialog1.Color = NewTheme.BackgroundColor
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NewTheme.BackgroundColor = ColorDialog1.Color
            ChangeColorPreviews()
            LoadCurrentTheme()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        ColorDialog1.Color = NewTheme.SectionBackgroundColor
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NewTheme.SectionBackgroundColor = ColorDialog1.Color
            ChangeColorPreviews()
            LoadCurrentTheme()
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ColorDialog1.Color = NewTheme.ForegroundColor
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NewTheme.ForegroundColor = ColorDialog1.Color
            ChangeColorPreviews()
            LoadCurrentTheme()
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        ColorDialog1.Color = NewTheme.AccentColors(0)
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NewTheme.AccentColors(0) = ColorDialog1.Color
            ChangeColorPreviews()
            LoadCurrentTheme()
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        ColorDialog1.Color = NewTheme.AccentColors(1)
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NewTheme.AccentColors(1) = ColorDialog1.Color
            ChangeColorPreviews()
            LoadCurrentTheme()
        End If
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        ColorDialog1.Color = NewTheme.AccentColors(2)
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NewTheme.AccentColors(2) = ColorDialog1.Color
            ChangeColorPreviews()
            LoadCurrentTheme()
        End If
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        ColorDialog1.Color = NewTheme.AccentColors(3)
        If ColorDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            NewTheme.AccentColors(3) = ColorDialog1.Color
            ChangeColorPreviews()
            LoadCurrentTheme()
        End If
    End Sub

    Private Sub ChangeColorPreviews()
        Label7.BackColor = NewTheme.BackgroundColor
        Label8.BackColor = NewTheme.ForegroundColor
        Label9.BackColor = NewTheme.SectionBackgroundColor
        Label10.BackColor = NewTheme.AccentColors(0)
        Label11.BackColor = NewTheme.AccentColors(1)
        Label12.BackColor = NewTheme.AccentColors(2)
        Label13.BackColor = NewTheme.AccentColors(3)
    End Sub

    Private Sub LoadCurrentTheme()
        NewTheme.DisabledForegroundColor = ThemeHelper.GetDisabledForegroundColor(NewTheme)

        ThemePreviewPanel.BackColor = NewTheme.BackgroundColor
        TestSection.BackColor = NewTheme.SectionBackgroundColor
        ThemePreviewPanel.ForeColor = NewTheme.ForegroundColor
        InactiveLabel.ForeColor = NewTheme.DisabledForegroundColor
        AccentedLabel1.BackColor = NewTheme.AccentColors(0)
        AccentedLabel2.BackColor = NewTheme.AccentColors(1)
        AccentedLabel3.BackColor = NewTheme.AccentColors(2)
        AccentedLabel4.BackColor = NewTheme.AccentColors(3)
        TextBox2.BackColor = NewTheme.BackgroundColor
        TextBox2.ForeColor = NewTheme.ForegroundColor

        TestGlyph1.Image = ThemeHelper.GetGlyphResource("newfile", True, NewTheme)
        TestGlyph2.Image = ThemeHelper.GetGlyphResource("openfile", True, NewTheme)
        TestGlyph3.Image = ThemeHelper.GetGlyphResource("info_glyph", True, NewTheme)
        TestGlyph4.Image = ThemeHelper.GetGlyphResource("save_glyph", True, NewTheme)
    End Sub

    Private Sub OpenFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        NewTheme = ThemeHelper.LoadThemeFile(OpenFileDialog1.FileName)
        TextBox1.Text = NewTheme.Name
        CheckBox1.Checked = NewTheme.IsDark
        ChangeColorPreviews()
        LoadCurrentTheme()
        Text = String.Format("DISMTools Theme Designer - {0}", Path.GetFileName(OpenFileDialog1.FileName))

        TextBox1.Select(TextBox1.TextLength, 0)
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub Label7_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label9.MouseHover, Label8.MouseHover, Label7.MouseHover, Label13.MouseHover, Label12.MouseHover, Label11.MouseHover, Label10.MouseHover
        Try
            Dim BackgroundColor As Color = CType(sender, Label).BackColor
            CurrentColorTT.SetToolTip(sender, String.Format("Current Color: RGB({0}, {1}, {2}). Click to copy to clipboard", BackgroundColor.R, BackgroundColor.G, BackgroundColor.B))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label9.Click, Label8.Click, Label7.Click, Label13.Click, Label12.Click, Label11.Click, Label10.Click
        Try
            Dim BackgroundColor As Color = CType(sender, Label).BackColor
            My.Computer.Clipboard.SetText(String.Format("RGB({0}, {1}, {2})", BackgroundColor.R, BackgroundColor.G, BackgroundColor.B))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        If String.IsNullOrEmpty(NewTheme.Name) Then
            MessageBox.Show("You must provide a name for the theme.", "Theme name missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If
        SaveFileDialog1.ShowDialog()
    End Sub

    Private Sub SaveFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        Cursor = Cursors.WaitCursor
        If ThemeHelper.SaveTheme(NewTheme, SaveFileDialog1.FileName) Then
            MessageBox.Show("The theme has been saved successfully at the specified location.", "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Text = String.Format("DISMTools Theme Designer - {0}", Path.GetFileName(SaveFileDialog1.FileName))
        Else
            MessageBox.Show("Could not save the theme.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        Cursor = Cursors.Arrow
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        NewTheme = ThemeHelper.GetNewTheme()
        TextBox1.Text = NewTheme.Name
        CheckBox1.Checked = NewTheme.IsDark
        ChangeColorPreviews()
        LoadCurrentTheme()
        Text = "DISMTools Theme Designer"
    End Sub

    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
#If VBC_VER >= 9.0 Then
        MsgBox(String.Format("DISMTools Theme Designer version {0}" & CrLf & CrLf & "{1}. ", _
                My.Application.Info.Version.ToString() & "_" & RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm"), _
                My.Application.Info.Copyright) & _
                "INI File Parser: © 2008 Ricardo Amores Hernández", _
            vbOKOnly + vbInformation, "About")
#Else
        MsgBox(String.Format("DISMTools Theme Designer version {0}_NET2REL" & CrLf & CrLf & "{1}. {2}", _
                My.Application.Info.Version.ToString(), _
                My.Application.Info.Copyright, "INI File Parser: © 2008 Ricardo Amores Hernández"), _
            vbOKOnly + vbInformation, "About")
#End If
    End Sub
End Class
