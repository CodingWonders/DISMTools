Imports System.Windows.Forms
Imports System.IO

Public Class BGProcFailureDialog

    Public FailedTasks As New Dictionary(Of String, Exception)
    Private DismComponents As New Dictionary(Of String, String)
    Public ImageInQuestion As WindowsImage

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub BGProcFailureDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = ""
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListBox1.BackColor = BackColor
        ListBox1.ForeColor = ForeColor
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ListBox1.Items.Clear()
        ListBox1.Items.AddRange(FailedTasks.Keys.ToArray())

        DynaLog.LogMessage("Getting DISM components...")
        Dim fv As FileVersionInfo
        For Each DismComponent In My.Computer.FileSystem.GetFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "dism"), FileIO.SearchOption.SearchTopLevelOnly)
            Try
                fv = FileVersionInfo.GetVersionInfo(DismComponent)
                DynaLog.LogMessage("Version of component " & ControlChars.Quote & Path.GetFileName(DismComponent) & ControlChars.Quote & ": " & fv.ProductVersion)
                DismComponents.Add(Path.GetFileName(DismComponent), fv.ProductVersion)
            Catch ex As Exception
                Continue For
            End Try
        Next
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        TextBox1.Text = ""
        Dim bgProcError As Exception = FailedTasks.Values.ElementAtOrDefault(ListBox1.SelectedIndex)
        If bgProcError IsNot Nothing Then
            TextBox1.Text = String.Format("- What failed: {1}{0}" &
                                          "- Error Code: 0x{2}{0}{0}" &
                                          "DISM System Components:{0}{0}", ControlChars.CrLf, bgProcError.Message, Hex(bgProcError.InnerException.HResult))
            For Each DismComponent In DismComponents.Keys
                TextBox1.Text &= String.Format("- {0}: {1}{2}", DismComponent, DismComponents(DismComponent), ControlChars.CrLf)
            Next
        End If
        If ImageInQuestion IsNot Nothing Then
            Try
                TextBox1.Text &= ControlChars.CrLf & ControlChars.CrLf & ImageInQuestion.ToString()
            Catch ex As Exception
                ' Don't show it
            End Try
        End If
    End Sub
End Class
