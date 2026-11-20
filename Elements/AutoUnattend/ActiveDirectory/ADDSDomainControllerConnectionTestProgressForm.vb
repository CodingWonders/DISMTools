Public Class ADDSDomainControllerConnectionTestProgressForm

    Public DomainName As String

    Private Sub ADDSDomainControllerConnectionTestProgressForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        WindowHelper.DisableCloseCapability(Handle)
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        Text = LocalizationService.ForSection("DomainControllerConnectionTester")("WndText")
        Label1.Text = LocalizationService.ForSection("DomainControllerConnectionTester")("HeaderText")
        lblDomainName.Text = DomainName
        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)
    End Sub
End Class