Imports System.Threading.Tasks

Public Class ServiceManagementForm

    Dim ServiceList As New List(Of WindowsService),
        ModifiedServiceList As New List(Of WindowsService)
    Dim ServiceStartTypes() As String

    Public Event ServiceSaveReported(current As Integer, count As Integer)

    Private progressMessage As String = ""
    Private isBusy As Boolean = False

    Private isModified As Boolean = False

    Private Sub OnServiceSaveReported(current As Integer, count As Integer) Handles Me.ServiceSaveReported
        progressMessage = LocalizationService.ForSection("ServiceManagement.Progress").Format("Saving.Label", current, count, Math.Round((current / count) * 100, 0))
    End Sub

    Public Sub ReportServiceSave(current As Integer, count As Integer)
        RaiseEvent ServiceSaveReported(current, count)
    End Sub

    Private Sub DisplayServiceInformation(Index As Integer)
        If (Index < 0) OrElse (Index > ServiceList.Count - 1) Then Exit Sub

        Dim selectedService As WindowsService = ServiceList.ElementAtOrDefault(Index)
        If selectedService Is Nothing Then Exit Sub

        DeleteServiceBtn.Enabled = Not selectedService.MarkedForDeletion
        RestoreServiceBtn.Enabled = selectedService.MarkedForDeletion

        TextBox1.Text = selectedService.Name
        TextBox2.Text = selectedService.DisplayName
        TextBox3.Text = selectedService.Description
        TextBox4.Text = selectedService.ImagePath
        TextBox5.Text = selectedService.ObjectName
        RemoveHandler ComboBox1.SelectedIndexChanged, AddressOf ComboBox1_SelectedIndexChanged
        ComboBox1.SelectedIndex = selectedService.StartType
        AddHandler ComboBox1.SelectedIndexChanged, AddressOf ComboBox1_SelectedIndexChanged
        TextBox6.Text = selectedService.Group
        TextBox7.Text = selectedService.TypeToString()
        TextBox8.Text = selectedService.ErrorControlToString()
        TextBox9.Text = selectedService.FailureActionToString(selectedService.FailureActions.FirstFailure)
        TextBox10.Text = selectedService.FailureActionToString(selectedService.FailureActions.SecondFailure)
        TextBox11.Text = selectedService.FailureActionToString(selectedService.FailureActions.SubsequentFailure)
        TextBox12.Text = LocalizationService.ForSection("ServiceManagement.Display").Format("MinuteS.Label", (selectedService.FailureActions.ResetDelayInSeconds / 60))
        TextBox13.Text = LocalizationService.ForSection("Services.Display").Format("MinutesSeconds.Message", Math.Round((selectedService.FailureActions.FirstDelayInMillis / 60000), 2), Math.Round((selectedService.FailureActions.FirstDelayInMillis / 1000), 2), Math.Round((selectedService.FailureActions.SecondDelayInMillis / 60000), 2), Math.Round((selectedService.FailureActions.SecondDelayInMillis / 1000), 2), Math.Round((selectedService.FailureActions.SubsequentDelaysInMillis / 60000), 2), Math.Round((selectedService.FailureActions.SubsequentDelaysInMillis / 1000), 2))

        CheckBox1.Checked = If(selectedService.StartType = WindowsService.ServiceStartType.Automatic, selectedService.DelayedStart, False)
        CheckBox1.Enabled = selectedService.StartType = WindowsService.ServiceStartType.Automatic

        ' Only enable user service flags with certain service types
        Label19.Enabled = {80, 96}.Contains(selectedService.Type)
        TextBox14.Enabled = {80, 96}.Contains(selectedService.Type)

        If {80, 96}.Contains(selectedService.Type) Then
            If selectedService.UserServiceFlags = Integer.MinValue Then
                TextBox14.Text = LocalizationService.ForSection("ServiceManagement.Display")("Undefined.Label")
            Else
                TextBox14.Text = selectedService.UserServiceFlags
            End If
        Else
            TextBox14.Text = LocalizationService.ForSection("ServiceManagement.Display")("Per.User.Label")
        End If

        ListView2.Items.Clear()
        ListView2.Items.AddRange(selectedService.RequiredPrivileges.Select(Function(RequiredPrivilege) New ListViewItem(New String() {RequiredPrivilege.ConstantNameText, RequiredPrivilege.ConstantUserRight, RequiredPrivilege.ConstantDescription})).ToArray())

        ListView3.Items.Clear()
        ListView4.Items.Clear()
        ListView5.Items.Clear()

        Dim dependencies As IEnumerable(Of WindowsService) = ServiceList.Where(Function(service) selectedService.Dependencies.Contains(service.Name)).OrderBy(Function(service) service.DisplayName)
        Dim dependents As IEnumerable(Of WindowsService) = ServiceList.Where(Function(service) service.Dependencies.Contains(selectedService.Name)).OrderBy(Function(service) service.DisplayName)

        ListView3.Items.AddRange(dependencies.Select(Function(dependency) New ListViewItem(New String() {dependency.Name, dependency.DisplayName, dependency.TypeToString()})).ToArray())
        ListView4.Items.AddRange(dependents.Select(Function(dependent) New ListViewItem(New String() {dependent.Name, dependent.DisplayName, dependent.TypeToString()})).ToArray())

        If selectedService.Group <> "" Then
            Dim servicesInGroup As IEnumerable(Of WindowsService) = ServiceList.Where(Function(service) service.Group.Equals(selectedService.Group, StringComparison.InvariantCultureIgnoreCase)).OrderBy(Function(service) service.DisplayName)
            ListView5.Items.AddRange(servicesInGroup.Select(Function(serviceInGroup) New ListViewItem(New String() {serviceInGroup.Name, serviceInGroup.DisplayName, serviceInGroup.TypeToString()})).ToArray())
            ListView5.Visible = True
        Else
            TextBox6.Text = LocalizationService.ForSection("ServiceManagement.Display")("Undefined.Group.Label")
            ListView5.Visible = False
        End If
    End Sub

    Private Sub ServiceManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListView1.Items.Clear()
        ComboBox1.Items.Clear()
        ServiceStartTypes = New String() {LocalizationService.ForSection("ServiceManagement.StartTypes")("BootLoader.Label"),
                                          LocalizationService.ForSection("ServiceManagement.StartTypes")("Iosystem.Label"),
                                          LocalizationService.ForSection("ServiceManagement.StartTypes")("Automatic.Label"),
                                          LocalizationService.ForSection("ServiceManagement.StartTypes")("Manual.Label"),
                                          LocalizationService.ForSection("ServiceManagement.StartTypes")("Disabled.Label")}
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
        ListView5.BackColor = BackColor
        ListView5.ForeColor = ForeColor
        TabPage1.BackColor = BackColor
        TabPage1.ForeColor = ForeColor
        TabPage2.BackColor = BackColor
        TabPage2.ForeColor = ForeColor
        TabPage3.BackColor = BackColor
        TabPage3.ForeColor = ForeColor
        TabPage4.BackColor = BackColor
        TabPage4.ForeColor = ForeColor
        TabPage5.BackColor = BackColor
        TabPage5.ForeColor = ForeColor
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
        TextBox14.BackColor = BackColor
        TextBox14.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        GroupBox2.ForeColor = ForeColor
        ComboBox1.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        SplitContainer1.SplitterDistance = WindowHelper.ScaleLogical(SplitContainer1.SplitterDistance)
        ListView4.Size = New Size(WindowHelper.ScaleLogical(ListView4.Width), WindowHelper.ScaleLogical(ListView4.Height))

        ModifiedServiceList.Clear()
        isModified = False

        DynaLog.DisableLogging()
        ServiceList = WindowsServiceHelper.GetServiceList(MainForm.MountDir)
        DynaLog.EnableLogging()

        ListView1.Items.AddRange(ServiceList.Select(Function(Service) New ListViewItem(New String() {Service.Name, Service.DisplayName, Service.Description, Service.StartTypeToString(), Service.TypeToString()})).ToArray())

        ColumnHeader1.Width = WindowHelper.ScaleLogical(218)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(279)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(237)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(173)
        ColumnHeader5.Width = WindowHelper.ScaleLogical(170)
        ColumnHeader6.Width = WindowHelper.ScaleLogical(177)
        ColumnHeader7.Width = WindowHelper.ScaleLogical(592)
        ColumnHeader8.Width = WindowHelper.ScaleLogical(209)
        ColumnHeader9.Width = WindowHelper.ScaleLogical(209)
        ColumnHeader10.Width = WindowHelper.ScaleLogical(120)
        ColumnHeader11.Width = WindowHelper.ScaleLogical(209)
        ColumnHeader12.Width = WindowHelper.ScaleLogical(195)
        ColumnHeader13.Width = WindowHelper.ScaleLogical(209)
        ColumnHeader14.Width = WindowHelper.ScaleLogical(120)
        ColumnHeader15.Width = WindowHelper.ScaleLogical(209)
        ColumnHeader16.Width = WindowHelper.ScaleLogical(567)
        ColumnHeader17.Width = WindowHelper.ScaleLogical(311)
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        DeleteServiceBtn.Enabled = ListView1.SelectedItems.Count = 1
        RestoreServiceBtn.Enabled = ListView1.SelectedItems.Count = 1

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
                If MsgBox(LocalizationService.ForSection("Services.Messages")("StartType.Message"), vbYesNo + vbExclamation) = MsgBoxResult.Yes Then
                    ComboBox1.SelectedIndex = ServiceList(selectedIndex).StartType
                    Exit Sub
                End If
            End If

            ' Hold a copy of the service so we can queue it for modification
            Dim newService As WindowsService = ServiceList(selectedIndex)
            newService.StartType = ComboBox1.SelectedIndex

            ' Store it in the modification queue, or update it
            If ModifiedServiceList.Any(Function(svc) svc.Name.Equals(newService.Name, StringComparison.OrdinalIgnoreCase)) Then
                Dim svcIndex As Integer = ModifiedServiceList.FindIndex(Function(svc) svc.Name.Equals(newService.Name, StringComparison.OrdinalIgnoreCase))
                ModifiedServiceList(svcIndex) = newService
            Else
                ModifiedServiceList.Add(newService)
            End If

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
                              Return WindowsServiceHelper.SaveServiceInformation(mntPath, ModifiedServiceList, Sub(current, count)
                                                                                                                   ReportServiceSave(current, count)
                                                                                                               End Sub)
                          End Function) Then
            MsgBox(LocalizationService.ForSection("Services.Messages")("System.Done.Message"), vbOKOnly + vbInformation)
        Else
            MsgBox(LocalizationService.ForSection("Services.Messages")("InfoSaved.Message"), vbOKOnly + vbExclamation)
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
            If MsgBox(LocalizationService.ForSection("Services.Messages")("UnsavedClose.Message"), vbYesNo + vbQuestion) = MsgBoxResult.No Then
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

        ModifiedServiceList.Clear()
        isModified = False

        DynaLog.DisableLogging()
        ServiceList = WindowsServiceHelper.GetServiceList(MainForm.MountDir)
        DynaLog.EnableLogging()
        
        ListView1.Items.AddRange(ServiceList.Select(Function(Service) New ListViewItem(New String() {Service.Name, Service.DisplayName, Service.Description, Service.StartTypeToString(), Service.TypeToString()})).ToArray())

        Cursor = Cursors.Arrow
    End Sub

    Private Sub ReloadServiceInformationBtn_Click(sender As Object, e As EventArgs) Handles ReloadServiceInformationBtn.Click
        If isBusy Then Exit Sub

        If isModified Then
            If MsgBox(LocalizationService.ForSection("Services.Messages")("UnsavedReload.Message"), vbYesNo + vbQuestion) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        ReloadServiceInformation()
    End Sub

    Private Sub GetSvchostGroupsBtn_Click(sender As Object, e As EventArgs) Handles GetSvchostGroupsBtn.Click
        Dim groups As List(Of WindowsServiceHostGroup) = WindowsServiceHelper.GetSvchostGroups(MainForm.MountDir, ServiceList)

        RegisteredServiceHostGroupsDialog.GroupInformation = groups
        RegisteredServiceHostGroupsDialog.ShowDialog(Me)
    End Sub

    Private Sub ReportServiceInfoBtn_Click(sender As Object, e As EventArgs) Handles ReportServiceInfoBtn.Click
        If ServiceInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Preparing to save image information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SaveTarget = ServiceInfoSFD.FileName
            Dim CurrentImage As WindowsImage = MainForm.MountedImageList.FirstOrDefault(Function(mountedImage) mountedImage.ImageMountDirectory = MainForm.MountDir)
            ' If it's still nothing then we give up.
            If CurrentImage Is Nothing Then Exit Sub
            DynaLog.LogMessage("Image to get information about: " & CurrentImage.ImageFile)
            ImgInfoSaveDlg.SourceImage = CurrentImage.ImageFile
            ImgInfoSaveDlg.ImgMountDir = If(Not MainForm.OnlineManagement, MainForm.MountDir, "")
            ImgInfoSaveDlg.OnlineMode = MainForm.OnlineManagement
            ImgInfoSaveDlg.OfflineMode = MainForm.OfflineManagement
            ImgInfoSaveDlg.AllDrivers = MainForm.AllDrivers
            ImgInfoSaveDlg.SkipQuestions = MainForm.SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = MainForm.AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = 10
            ImgInfoSaveDlg.ImageToGetInfoFrom = CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Private Sub DeleteServiceBtn_Click(sender As Object, e As EventArgs) Handles DeleteServiceBtn.Click
        If ListView1.SelectedItems.Count = 1 Then
            If MessageBox.Show(LocalizationService.ForSection("ServiceMgmt.Messages")("Continui.Removal.Svc.Message"),
                               LocalizationService.ForSection("Services.Messages")("RemoveService.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.No Then Exit Sub

            Dim selectedIndex As Integer = ListView1.FocusedItem.Index

            ' Hold a copy of the service so we can queue it for modification
            Dim newService As WindowsService = ServiceList(selectedIndex)
            newService.MarkedForDeletion = True

            ' Store it in the modification queue, or update it
            If ModifiedServiceList.Any(Function(svc) svc.Name.Equals(newService.Name, StringComparison.OrdinalIgnoreCase)) Then
                Dim svcIndex As Integer = ModifiedServiceList.FindIndex(Function(svc) svc.Name.Equals(newService.Name, StringComparison.OrdinalIgnoreCase))
                ModifiedServiceList(svcIndex) = newService
            Else
                ModifiedServiceList.Add(newService)
            End If

            MessageBox.Show(LocalizationService.ForSection("Services.Messages")("Scheduled.Deletion.Message"),
                            LocalizationService.ForSection("Services.Messages")("RemoveService.Title"), MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' Force refresh of service information
            DisplayServiceInformation(ListView1.FocusedItem.Index)
        End If
    End Sub

    Private Sub RestoreServiceBtn_Click(sender As Object, e As EventArgs) Handles RestoreServiceBtn.Click
        If ListView1.SelectedItems.Count = 1 Then
            ServiceList(ListView1.FocusedItem.Index).MarkedForDeletion = False

            ' Force refresh of service information
            DisplayServiceInformation(ListView1.FocusedItem.Index)
        End If
    End Sub
End Class