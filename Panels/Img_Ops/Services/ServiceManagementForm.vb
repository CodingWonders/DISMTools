Imports System.Threading.Tasks

Public Class ServiceManagementForm

    Dim ServiceList As New List(Of WindowsService)
    Dim ServiceStartTypes() As String = New String() {"Boot Loader", "I/O System", "Automatic", "Manual", "Disabled"}

    Public Event ServiceSaveReported(current As Integer, count As Integer)

    Private progressMessage As String = ""
    Private isBusy As Boolean = False

    Private isModified As Boolean = False

    Private Sub OnServiceSaveReported(current As Integer, count As Integer) Handles Me.ServiceSaveReported
        progressMessage = String.Format("Saving service information... ({0}/{1}, {2}%)", current, count, Math.Round((current / count) * 100, 0))
    End Sub

    Public Sub ReportServiceSave(current As Integer, count As Integer)
        RaiseEvent ServiceSaveReported(current, count)
    End Sub

    Private Sub DisplayServiceInformation(Index As Integer)
        If (Index < 0) OrElse (Index > ServiceList.Count - 1) Then Exit Sub

        TextBox1.Text = ServiceList(Index).Name
        TextBox2.Text = ServiceList(Index).DisplayName
        TextBox3.Text = ServiceList(Index).Description
        TextBox4.Text = ServiceList(Index).ImagePath
        TextBox5.Text = ServiceList(Index).ObjectName
        RemoveHandler ComboBox1.SelectedIndexChanged, AddressOf ComboBox1_SelectedIndexChanged
        ComboBox1.SelectedIndex = ServiceList(Index).StartType
        AddHandler ComboBox1.SelectedIndexChanged, AddressOf ComboBox1_SelectedIndexChanged
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

        CheckBox1.Checked = If(ServiceList(Index).StartType = WindowsService.ServiceStartType.Automatic, ServiceList(Index).DelayedStart, False)
        CheckBox1.Enabled = ServiceList(Index).StartType = WindowsService.ServiceStartType.Automatic

        ListView2.Items.Clear()
        For Each RequiredPrivilege In ServiceList(Index).RequiredPrivileges
            ListView2.Items.Add(New ListViewItem(New String() {RequiredPrivilege.ConstantNameText, RequiredPrivilege.ConstantUserRight, RequiredPrivilege.ConstantDescription}))
        Next

        ListView3.Items.Clear()
        ListView4.Items.Clear()

        Dim dependencies As List(Of WindowsService) = ServiceList.Where(Function(service) ServiceList(Index).Dependencies.Contains(service.Name)).OrderBy(Function(service) service.DisplayName).ToList()
        Dim dependents As List(Of WindowsService) = ServiceList.Where(Function(service) service.Dependencies.Contains(ServiceList(Index).Name)).OrderBy(Function(service) service.DisplayName).ToList()

        For Each dependency As WindowsService In dependencies
            ListView3.Items.Add(New ListViewItem(New String() {dependency.Name, dependency.DisplayName, dependency.TypeToString()}))
        Next

        For Each dependent As WindowsService In dependents
            ListView4.Items.Add(New ListViewItem(New String() {dependent.Name, dependent.DisplayName, dependent.TypeToString()}))
        Next
    End Sub

    Private Sub ServiceManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListView1.Items.Clear()
        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange(ServiceStartTypes)
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
        ComboBox1.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, CurrentTheme.IsDark)

        isModified = False

        DynaLog.DisableLogging()
        ServiceList = WindowsServiceHelper.GetServiceList(MainForm.MountDir)
        DynaLog.EnableLogging()

        For Each Service In ServiceList
            ListView1.Items.Add(New ListViewItem(New String() {Service.Name, Service.DisplayName, Service.Description, Service.StartTypeToString(), Service.TypeToString()}))
        Next
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count = 1 Then
            DisplayServiceInformation(ListView1.FocusedItem.Index)
        End If
        NoServiceSelectedPanel.Visible = (ListView1.SelectedItems.Count <> 1)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If ListView1.SelectedItems.Count = 1 Then
            ServiceList(ListView1.FocusedItem.Index).DelayedStart = CheckBox1.Checked

            isModified = True
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ListView1.SelectedItems.Count = 1 Then
            Dim ForbiddenTypesForNonServices() As WindowsService.ServiceType = New WindowsService.ServiceType() {WindowsService.ServiceType.WindowsService, WindowsService.ServiceType.WindowsApplication}
            Dim ForbiddenStartTypesForNonServices() As WindowsService.ServiceStartType = New WindowsService.ServiceStartType() {WindowsService.ServiceStartType.BootLoader, WindowsService.ServiceStartType.IOSystem}

            Dim selectedIndex As Integer = ListView1.FocusedItem.Index

            If ForbiddenTypesForNonServices.Contains(ServiceList(selectedIndex).Type) AndAlso
                ForbiddenStartTypesForNonServices.Contains(ComboBox1.SelectedIndex) Then
                If MsgBox("The selected start type is unsupported for services of this type. The selected service may not work correctly or at all if you continue with this start type." & vbCrLf & vbCrLf & "Do you want to reset this start type to its current value?", vbYesNo + vbExclamation) = MsgBoxResult.Yes Then
                    ComboBox1.SelectedIndex = ServiceList(selectedIndex).StartType
                    Exit Sub
                End If
            End If

            ServiceList(selectedIndex).StartType = ComboBox1.SelectedIndex

            ' We don't have to uncheck the box, we simply disable it, if it's not automatic
            CheckBox1.Enabled = (ComboBox1.SelectedIndex = WindowsService.ServiceStartType.Automatic)

            isModified = True
        End If
    End Sub

    Private Async Sub SaveServiceInfoBtn_Click(sender As Object, e As EventArgs) Handles SaveServiceInfoBtn.Click
        If isBusy Then Exit Sub

        ProgressLabel.Visible = True
        Timer1.Enabled = True
        Cursor = Cursors.WaitCursor
        Dim mntPath As String = MainForm.MountDir
        isBusy = True
        WindowHelper.DisableCloseCapability(Handle)
        If Await Task.Run(Function()
                              Return WindowsServiceHelper.SaveServiceInformation(mntPath, ServiceList, Sub(current, count)
                                                                                                           ReportServiceSave(current, count)
                                                                                                       End Sub)
                          End Function) Then
            MsgBox("System service information has been successfully saved to the registry of the target image." & vbCrLf & vbCrLf &
                   "A backup of the previous service configuration has been saved to your desktop should you need it in case service modifications do not go as planned." & vbCrLf & vbCrLf &
                   "Simply load the target image's SYSTEM hive and import this registry file.", vbOKOnly + vbInformation)
        End If
        WindowHelper.EnableCloseCapability(Handle)
        Cursor = Cursors.Arrow
        ProgressLabel.Visible = False
        Timer1.Enabled = False
        isBusy = False
        ReloadServiceInformation()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ProgressLabel.Text = progressMessage
    End Sub

    Private Sub ServiceManagementForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If isBusy Then
            e.Cancel = True
            Beep()
            Exit Sub
        End If

        If isModified Then
            If MsgBox("Some changes have been made. Closing this window will discard all your changes to Windows services. Do you want to discard these changes?", vbYesNo + vbQuestion) = MsgBoxResult.No Then
                e.Cancel = True
                Beep()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub ServiceManagementForm_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If isBusy Then WindowHelper.DisableCloseCapability(Handle)
    End Sub

    Sub ReloadServiceInformation()
        Cursor = Cursors.WaitCursor
        NoServiceSelectedPanel.Visible = True
        ListView1.Items.Clear()

        isModified = False

        DynaLog.DisableLogging()
        ServiceList = WindowsServiceHelper.GetServiceList(MainForm.MountDir)
        DynaLog.EnableLogging()

        For Each Service In ServiceList
            ListView1.Items.Add(New ListViewItem(New String() {Service.Name, Service.DisplayName, Service.Description, Service.StartTypeToString(), Service.TypeToString()}))
        Next

        Cursor = Cursors.Arrow
    End Sub

    Private Sub ReloadServiceInformationBtn_Click(sender As Object, e As EventArgs) Handles ReloadServiceInformationBtn.Click
        If isBusy Then Exit Sub

        If isModified Then
            If MsgBox("Some changes have been made. Reloading service information will discard all your changes to Windows services. Do you want to discard these changes?", vbYesNo + vbQuestion) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        ReloadServiceInformation()
    End Sub
End Class