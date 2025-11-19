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
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, CurrentTheme.IsDark)

        For Each Group In GroupInformation
            ServiceGroupDetailsLv.Items.Add(New ListViewItem(New String() {Group.Name, String.Format("{0} service(s) in group", Group.Services.Count)}))
        Next
    End Sub

    Private Sub ServiceGroupDetailsLv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ServiceGroupDetailsLv.SelectedIndexChanged
        ServiceDetailsLv.Items.Clear()
        Try
            If ServiceGroupDetailsLv.SelectedItems.Count = 1 Then
                For Each ServiceInGroup In GroupInformation(ServiceGroupDetailsLv.FocusedItem.Index).Services
                    ServiceDetailsLv.Items.Add(New ListViewItem(New String() {ServiceInGroup.Name, ServiceInGroup.DisplayName, ServiceInGroup.TypeToString()}))
                Next
            End If
        Catch ex As Exception
            ' ignore possible exceptions
        End Try
    End Sub
End Class
