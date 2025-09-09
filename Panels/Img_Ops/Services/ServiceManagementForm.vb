Public Class ServiceManagementForm

    Dim ServiceList As New List(Of WindowsService)

    Private Sub DisplayServiceInformation(Index As Integer)
        If Index > ServiceList.Count - 1 Then Exit Sub

        TextBox1.Text = ServiceList(Index).Name
        TextBox2.Text = ServiceList(Index).DisplayName
        TextBox3.Text = ServiceList(Index).Description
        TextBox4.Text = ServiceList(Index).ImagePath
        TextBox5.Text = ServiceList(Index).ObjectName
        TextBox6.Text = ServiceList(Index).StartTypeToString()
        TextBox7.Text = ServiceList(Index).TypeToString()

        CheckBox1.Checked = ServiceList(Index).DelayedStart

        ListView2.Items.Clear()
        For Each RequiredPrivilege In ServiceList(Index).RequiredPrivileges
            ListView2.Items.Add(New ListViewItem(New String() {RequiredPrivilege.ConstantNameText, RequiredPrivilege.ConstantUserRight, RequiredPrivilege.ConstantDescription}))
        Next
    End Sub

    Private Sub ServiceManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListView1.Items.Clear()

        ServiceList = WindowsServiceHelper.GetServiceList(MainForm.MountDir)

        'MsgBox("Services: " & ServiceList.Count)

        For Each Service In ServiceList

            ListView1.Items.Add(New ListViewItem(New String() {Service.Name, Service.DisplayName, Service.Description, Service.StartTypeToString}))
        Next
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count = 1 Then
            DisplayServiceInformation(ListView1.FocusedItem.Index)
        End If
    End Sub
End Class