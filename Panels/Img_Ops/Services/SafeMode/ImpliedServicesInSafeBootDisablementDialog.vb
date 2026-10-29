Imports System.Windows.Forms

Public Class ImpliedServicesInSafeBootDisablementDialog

    Public ImpliedServices As Dictionary(Of String, List(Of WindowsService))

    Public ImplyServiceDependencies As Boolean

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ImplyServiceDependencies = CheckBox1.Checked
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

    Private Sub ImpliedServicesInSafeBootDisablementDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ServiceDependentDetailsLv.Items.Clear()
        ServiceDependencyDetailsLv.Items.Clear()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ServiceDependentDetailsLv.BackColor = BackColor
        ServiceDependentDetailsLv.ForeColor = ForeColor
        ServiceDependencyDetailsLv.BackColor = BackColor
        ServiceDependencyDetailsLv.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ServiceDependentDetailsLv.Items.AddRange(ImpliedServices("allDependents").OrderBy(Function(Service) Service.DisplayName).Select(Function(Service) New ListViewItem(New String() {String.Format("{0}{1}", Service.Name, If(ImpliedServices("mainServiceDependents").Any(Function(svc) svc.Name = Service.Name), " (*)", "")),
                                                                                                                                                                                         Service.DisplayName, Service.TypeToString()})).ToArray())
        ServiceDependencyDetailsLv.Items.AddRange(ImpliedServices("dependencies").OrderBy(Function(Service) Service.DisplayName).Select(Function(Service) New ListViewItem(New String() {Service.Name, Service.DisplayName, Service.TypeToString()})).ToArray())

        ColumnHeader1.Width = WindowHelper.ScaleLogical(175)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(274)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(175)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(274)
        ColumnHeader5.Width = WindowHelper.ScaleLogical(192)
        ColumnHeader6.Width = WindowHelper.ScaleLogical(192)
    End Sub
End Class
