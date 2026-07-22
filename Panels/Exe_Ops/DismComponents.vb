Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class DismComponents

    Dim fv As FileVersionInfo

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub DismComponents_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("DismComponents")("Title.Label")
        ListView1.Columns(0).Text = LocalizationService.ForSection("DismComponents")("Component.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("DismComponents")("Version.Column")
        OK_Button.Text = LocalizationService.ForSection("DismComponents")("Ok.Button")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        ListView1.Items.Clear()
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        Visible = True
        DynaLog.LogMessage("Getting DISM components...")
        For Each DismComponent In My.Computer.FileSystem.GetFiles(Path.GetDirectoryName(Options.TextBox1.Text) & "\dism", FileIO.SearchOption.SearchTopLevelOnly)
            Try
                fv = FileVersionInfo.GetVersionInfo(DismComponent)
                DynaLog.LogMessage("Version of component " & Quote & Path.GetFileName(DismComponent) & Quote & ": " & fv.ProductVersion)
                ListView1.Items.Add(Path.GetFileName(DismComponent)).SubItems.Add(fv.ProductVersion)
            Catch ex As Exception
                Continue For
            End Try
        Next

        ColumnHeader1.Width = WindowHelper.ScaleLogical(250)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(238)
    End Sub
End Class
