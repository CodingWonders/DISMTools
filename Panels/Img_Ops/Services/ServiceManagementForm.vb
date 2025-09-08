Public Class ServiceManagementForm

    Private Sub ServiceManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListView1.Items.Clear()
        Dim ServiceList As New List(Of WindowsService)

        ServiceList = WindowsServiceHelper.GetServiceList(MainForm.MountDir)

        'MsgBox("Services: " & ServiceList.Count)

        For Each Service In ServiceList
            ListView1.Items.Add(New ListViewItem(New String() {Service.Name, Service.DisplayName, Service.Description, Service.StartType}))
        Next
    End Sub
End Class