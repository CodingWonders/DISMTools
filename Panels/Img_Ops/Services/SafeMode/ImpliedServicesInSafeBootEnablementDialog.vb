Imports System.Windows.Forms

Public Class ImpliedServicesInSafeBootEnablementDialog

    Public ImpliedServices As IEnumerable(Of WindowsService)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Yes
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.No
        Me.Close()
    End Sub

    Private Sub btnNoAdditionalServices_Click(sender As Object, e As EventArgs) Handles btnNoAdditionalServices.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImpliedServicesInSafeBootEnablementDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ServiceDetailsLv.Items.Clear()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ServiceDetailsLv.BackColor = BackColor
        ServiceDetailsLv.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ServiceDetailsLv.Items.AddRange(ImpliedServices.OrderBy(Function(Service) Service.DisplayName).Select(Function(Service) New ListViewItem(New String() {Service.Name, Service.DisplayName, Service.TypeToString()})).ToArray())
        ColumnHeader3.Width = WindowHelper.ScaleLogical(175)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(274)
        ColumnHeader5.Width = WindowHelper.ScaleLogical(192)
    End Sub
End Class
