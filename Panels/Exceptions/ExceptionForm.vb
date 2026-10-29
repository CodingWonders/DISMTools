Imports Microsoft.VisualBasic.ControlChars
Imports System.IO

Public Class ExceptionForm

    Dim copySuccess As String = String.Empty
    Dim copyFail As String = String.Empty

    Dim dvPath As String = Path.Combine(Application.StartupPath, "Tools", "DynaViewer", "DynaViewer.exe")

    Private Sub Issue_Btn_Click(sender As Object, e As EventArgs) Handles Issue_Btn.Click
        DialogResult = Windows.Forms.DialogResult.None
        Process.Start("https://github.com/CodingWonders/DISMTools/issues/new?assignees=CodingWonders&labels=bug%2C+good+first+issue&projects=&template=program-exception.md&title=Program+exception")
    End Sub

    Private Sub LinkLabel1_Click(sender As Object, e As EventArgs) Handles LinkLabel1.Click
        DialogResult = Windows.Forms.DialogResult.OK
        Close()
    End Sub

    Private Sub LinkLabel2_Click(sender As Object, e As EventArgs) Handles LinkLabel2.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Close()
    End Sub

    Private Sub ExceptionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("Exception")("DISM.Tools.Internal.Label")
        Label1.Text = LocalizationService.ForSection("Exception")("Sorry.Inconvenience.Message")
        Label2.Text = LocalizationService.ForSection("Exception")("Help.Us.Fix.Label")
        Label3.Text = LocalizationService.ForSection("Exception")("Problem.Prevention.Message")
        Label4.Text = LocalizationService.ForSection("Exception")("Reporting.Issue.Message")
        Label5.Text = LocalizationService.ForSection("Exception")("Continue.Running.Message")
        Issue_Btn.Text = LocalizationService.ForSection("Exception")("ReportIssue.Label")
        LinkLabel1.Text = LocalizationService.ForSection("Exception")("Continue.Button")
        LinkLabel2.Text = LocalizationService.ForSection("Exception")("Exit.Button")
        copySuccess = LocalizationService.ForSection("Exception")("Copied.Clipboard.Label")
        copyFail = LocalizationService.ForSection("Exception")("Ll.Copy.Label")
        If CurrentTheme IsNot Nothing Then
            BackColor = CurrentTheme.SectionBackgroundColor
            ForeColor = CurrentTheme.ForegroundColor
            ErrorText.BackColor = CurrentTheme.BackgroundColor
            ErrorText.ForeColor = CurrentTheme.ForegroundColor
            Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
            WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
            If CurrentTheme.AccentColors IsNot Nothing AndAlso CurrentTheme.AccentColors.Count > 0 Then
                ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
            End If
        End If
        Try
            Dim data As New DataObject()
            data.SetText(ErrorText.Text, TextDataFormat.Text)
            Clipboard.SetDataObject(data, True)
            ErrorText.AppendText(CrLf & CrLf & copySuccess)
        Catch ex As Exception
            ErrorText.AppendText(CrLf & CrLf & copyFail)
        End Try
        Beep()
    End Sub

    Private Sub DynaViewer_Button_Click(sender As Object, e As EventArgs) Handles DynaViewer_Button.Click
        DialogResult = Windows.Forms.DialogResult.None
        If Not File.Exists(dvPath) Then
            Exit Sub
        End If
        Try
            ' Copy logs first
            File.Copy(Path.Combine(Application.StartupPath, "logs", "DT_DynaLog.log"),
                      Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DynaLog_Trace.log"))
            Process.Start(dvPath, Quote & Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DynaLog_Trace.log") & Quote & _
                          " /selectlast=10 " & LocalizationService.GetLanguageCommandLineArgument())
        Catch ex As Exception
            Process.Start(dvPath, Quote & Path.Combine(Application.StartupPath, "logs", "DT_DynaLog.log") & Quote & _
                          " /selectlast=10 " & LocalizationService.GetLanguageCommandLineArgument())
        End Try
    End Sub
End Class