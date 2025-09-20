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
        TextBox8.Text = ServiceList(Index).ErrorControlToString()
        TextBox9.Text = ServiceList(Index).FailureActionToString(ServiceList(Index).FailureActions.FirstFailure)
        TextBox10.Text = ServiceList(Index).FailureActionToString(ServiceList(Index).FailureActions.SecondFailure)
        TextBox11.Text = ServiceList(Index).FailureActionToString(ServiceList(Index).FailureActions.SubsequentFailure)
        TextBox12.Text = String.Format("{0} minute(s)", (ServiceList(Index).FailureActions.ResetDelayInSeconds / 60))
        TextBox13.Text = String.Format("{0} minute(s) ({1} seconds) after first failure, {2} minute(s) ({3} seconds) after second failure, {4} minute(s) ({5} seconds) after subsequent failures",
                                       Math.Round((ServiceList(Index).FailureActions.FirstDelayInMillis / 60000), 2),
                                       Math.Round((ServiceList(Index).FailureActions.FirstDelayInMillis / 1000), 2),
                                       Math.Round((ServiceList(Index).FailureActions.SecondDelayInMillis / 60000), 2),
                                       Math.Round((ServiceList(Index).FailureActions.SecondDelayInMillis / 1000), 2),
                                       Math.Round((ServiceList(Index).FailureActions.SubsequentDelaysInMillis / 60000), 2),
                                       Math.Round((ServiceList(Index).FailureActions.SubsequentDelaysInMillis / 1000), 2))

        CheckBox1.Checked = ServiceList(Index).DelayedStart

        ListView2.Items.Clear()
        For Each RequiredPrivilege In ServiceList(Index).RequiredPrivileges
            ListView2.Items.Add(New ListViewItem(New String() {RequiredPrivilege.ConstantNameText, RequiredPrivilege.ConstantUserRight, RequiredPrivilege.ConstantDescription}))
        Next

        ListView3.Items.Clear()
        ListView4.Items.Clear()

        Dim dependencies As List(Of WindowsService) = ServiceList.Where(Function(service) ServiceList(Index).Dependencies.Contains(service.Name)).OrderBy(Function(service) service.DisplayName).ToList()
        Dim dependents As List(Of WindowsService) = ServiceList.Where(Function(service) service.Dependencies.Contains(ServiceList(Index).Name)).OrderBy(Function(service) service.DisplayName).ToList()

        For Each dependency As WindowsService In dependencies
            ListView3.Items.Add(New ListViewItem(New String() {dependency.Name, dependency.DisplayName}))
        Next

        For Each dependent As WindowsService In dependents
            ListView4.Items.Add(New ListViewItem(New String() {dependent.Name, dependent.DisplayName}))
        Next
    End Sub

    Private Sub ServiceManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListView1.Items.Clear()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        ListView2.BackColor = BackColor
        ListView2.ForeColor = ForeColor
        ListView3.BackColor = BackColor
        ListView3.ForeColor = ForeColor
        ListView4.BackColor = BackColor
        ListView4.ForeColor = ForeColor
        TabPage1.BackColor = BackColor
        TabPage1.ForeColor = ForeColor
        TabPage2.BackColor = BackColor
        TabPage2.ForeColor = ForeColor
        TabPage3.BackColor = BackColor
        TabPage3.ForeColor = ForeColor
        TabPage4.BackColor = BackColor
        TabPage4.ForeColor = ForeColor
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        TextBox2.BackColor = BackColor
        TextBox2.ForeColor = ForeColor
        TextBox3.BackColor = BackColor
        TextBox3.ForeColor = ForeColor
        TextBox4.BackColor = BackColor
        TextBox4.ForeColor = ForeColor
        TextBox5.BackColor = BackColor
        TextBox5.ForeColor = ForeColor
        TextBox6.BackColor = BackColor
        TextBox6.ForeColor = ForeColor
        TextBox7.BackColor = BackColor
        TextBox7.ForeColor = ForeColor
        TextBox8.BackColor = BackColor
        TextBox8.ForeColor = ForeColor
        TextBox9.BackColor = BackColor
        TextBox9.ForeColor = ForeColor
        TextBox10.BackColor = BackColor
        TextBox10.ForeColor = ForeColor
        TextBox11.BackColor = BackColor
        TextBox11.ForeColor = ForeColor
        TextBox12.BackColor = BackColor
        TextBox12.ForeColor = ForeColor
        TextBox13.BackColor = BackColor
        TextBox13.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, CurrentTheme.IsDark)

        DynaLog.DisableLogging()
        ServiceList = WindowsServiceHelper.GetServiceList(MainForm.MountDir)
        DynaLog.EnableLogging()

        For Each Service In ServiceList
            ListView1.Items.Add(New ListViewItem(New String() {Service.Name, Service.DisplayName, Service.Description, Service.StartTypeToString}))
        Next
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count = 1 Then
            DisplayServiceInformation(ListView1.FocusedItem.Index)
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If ListView1.SelectedItems.Count = 1 Then
            If CheckBox1.Checked <> ServiceList(ListView1.FocusedItem.Index).DelayedStart Then
                CheckBox1.Checked = ServiceList(ListView1.FocusedItem.Index).DelayedStart
            End If
        End If
    End Sub
End Class