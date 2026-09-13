Imports System.Windows.Forms

Public Class RegisteredServiceHostGroupsDialog

    Public GroupInformation As New List(Of WindowsServiceHostGroup)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub RegisteredServiceHostGroupsDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ServiceGroupDetailsLv.Items.Clear()
        ServiceDetailsLv.Items.Clear()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ServiceGroupDetailsLv.BackColor = BackColor
        ServiceGroupDetailsLv.ForeColor = ForeColor
        ServiceDetailsLv.BackColor = BackColor
        ServiceDetailsLv.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ' Order group information based on service count
        GroupInformation = GroupInformation.OrderByDescending(Function(serviceGroup) serviceGroup.Services.Count).ThenBy(Function(serviceGroup) serviceGroup.Name).ToList()

        ServiceGroupDetailsLv.Items.AddRange(GroupInformation.Select(Function(Group) New ListViewItem(New String() {Group.Name, String.Format(LocalizationService.ForSection("ServiceGroups")("ServiceGroup.Label"), Group.Services.Count)})).ToArray())

        Dim count As Integer = GroupInformation.Sum(Function(serviceGroup) serviceGroup.Services.Count)
        Label2.Text = String.Format(LocalizationService.ForSection("ServiceGroups")("RegisteredHost.Label"), count)

        ColumnHeader1.Width = WindowHelper.ScaleLogical(274)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(233)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(175)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(274)
        ColumnHeader5.Width = WindowHelper.ScaleLogical(192)
    End Sub

    Private Sub ServiceGroupDetailsLv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ServiceGroupDetailsLv.SelectedIndexChanged
        ServiceDetailsLv.Items.Clear()
        Try
            If ServiceGroupDetailsLv.SelectedItems.Count = 1 Then
                ServiceDetailsLv.Items.AddRange(GroupInformation(ServiceGroupDetailsLv.FocusedItem.Index).Services.OrderBy(Function(ServiceInGroup) ServiceInGroup.DisplayName).Select(Function(ServiceInGroup) New ListViewItem(New String() {ServiceInGroup.Name, ServiceInGroup.DisplayName, ServiceInGroup.TypeToString()})).ToArray())
            End If
        Catch ex As Exception
            ' ignore possible exceptions
        End Try
    End Sub
End Class
