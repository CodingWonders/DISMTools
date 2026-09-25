Imports System.Threading.Tasks

Public Class ServiceManagementForm

    Dim ServiceList As New List(Of WindowsService),
        ModifiedServiceList As New List(Of WindowsService)
    Dim ServiceStartTypes() As String

    Public Event ServiceSaveReported(current As Integer, count As Integer)

    Private progressMessage As String = ""
    Private isBusy As Boolean = False

    Private isModified As Boolean = False

    Private CriticalServiceNames As New List(Of String) From {"BFE", "CoreMessagingRegistrar", "CryptSvc", "DcomLaunch", "DeviceInstall", "Dhcp", "Dnscache", "EventLog",
                                                              "gpsvc", "IKEEXT", "lmhosts", "LSM", "mpssvc", "nsi", "PlugPlay", "Power", "RpcSs", "RpcEptMapper", "SamSs", "SystemEventsBroker",
                                                              "TcpIp", "UserManager", "ProfSvc", "LanmanWorkstation"}

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

        RemoveHandler CheckBox2.CheckedChanged, AddressOf CheckBox2_CheckedChanged
        RemoveHandler CheckBox3.CheckedChanged, AddressOf CheckBox3_CheckedChanged
        CheckBox2.Checked = selectedService.SafeModeOptions.AvailableInMinimalSafeBoot
        CheckBox3.Checked = selectedService.SafeModeOptions.AvailableInNetworkSafeBoot
        AddHandler CheckBox2.CheckedChanged, AddressOf CheckBox2_CheckedChanged
        AddHandler CheckBox3.CheckedChanged, AddressOf CheckBox3_CheckedChanged

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
        Label1.Text = LocalizationService.ForSection("Designer.Services")("Intro.Message")
        ColumnHeader1.Text = LocalizationService.ForSection("Designer.Services")("ServiceName.Column")
        ColumnHeader2.Text = LocalizationService.ForSection("Designer.Services")("DisplayName.Column")
        ColumnHeader3.Text = LocalizationService.ForSection("Designer.Services")("Description.Column")
        ColumnHeader4.Text = LocalizationService.ForSection("Designer.Services")("StartType.Column")
        ColumnHeader12.Text = LocalizationService.ForSection("Designer.Services")("Type.Column")
        TabPage1.Text = LocalizationService.ForSection("Designer.Services")("ServiceInfo.Tab")
        CheckBox1.Text = LocalizationService.ForSection("Designer.Services")("DelayedStart.CheckBox")
        Label4.Text = LocalizationService.ForSection("Designer.Services")("Description.Label")
        Label19.Text = LocalizationService.ForSection("Designer.Services")("User.Flags.Label")
        Label8.Text = LocalizationService.ForSection("Designer.Services")("ServiceType.Label")
        Label7.Text = LocalizationService.ForSection("Designer.Services")("Start.Type.Label")
        Label6.Text = LocalizationService.ForSection("Designer.Services")("Object.Name.Label")
        Label5.Text = LocalizationService.ForSection("Designer.Services")("Image.Path.Label")
        Label3.Text = LocalizationService.ForSection("Designer.Services")("Display.Name.Label")
        Label2.Text = LocalizationService.ForSection("Designer.Services")("ServiceName.Label")
        TabPage2.Text = LocalizationService.ForSection("Designer.Services")("Required.Privileges.Tab")
        ColumnHeader5.Text = LocalizationService.ForSection("Designer.Services")("PrivilegeName.Column")
        ColumnHeader6.Text = LocalizationService.ForSection("Designer.Services")("PrivilegeName.Display.Column")
        ColumnHeader7.Text = LocalizationService.ForSection("Designer.Services")("Privilege.Description.Column")
        TabPage3.Text = LocalizationService.ForSection("Designer.Services")("ErrorControl.Tab")
        GroupBox1.Text = LocalizationService.ForSection("Designer.Services")("FailureActions.Group")
        Label12.Text = LocalizationService.ForSection("Designer.Services")("FutureErrors.Label")
        Label11.Text = LocalizationService.ForSection("Designer.Services")("NdError.Label")
        Label14.Text = LocalizationService.ForSection("Designer.ServiceMgmt")("Restart.Minutes.Label")
        Label13.Text = LocalizationService.ForSection("Designer.Services")("ResetErrorCount.Label")
        Label10.Text = LocalizationService.ForSection("Designer.Services")("StError.Label")
        Label9.Text = LocalizationService.ForSection("Designer.Services")("Error.Windows.Label")
        TabPage4.Text = LocalizationService.ForSection("Designer.Services")("Dependencies.Tab")
        ColumnHeader8.Text = LocalizationService.ForSection("Designer.Services")("ServiceName.Column")
        ColumnHeader9.Text = LocalizationService.ForSection("Designer.Services")("DisplayName.Column")
        ColumnHeader10.Text = LocalizationService.ForSection("Designer.Services")("Type.Column")
        Label17.Text = LocalizationService.ForSection("Designer.ServiceMgmt")("Dependencies.Label")
        ColumnHeader11.Text = LocalizationService.ForSection("Designer.Services")("ServiceName.Column")
        ColumnHeader13.Text = LocalizationService.ForSection("Designer.Services")("DisplayName.Column")
        ColumnHeader14.Text = LocalizationService.ForSection("Designer.Services")("Type.Column")
        Label18.Text = LocalizationService.ForSection("Designer.ServiceMgmt")("Dependent.Services.Label")
        TabPage5.Text = LocalizationService.ForSection("Designer.Services")("ServiceGroups.Tab")
        GetSvchostGroupsBtn.Text = LocalizationService.ForSection("Designer.Services")("RegisteredHosts.Label")
        GroupBox2.Text = LocalizationService.ForSection("Designer.Services")("Services.Belong.Group")
        ColumnHeader15.Text = LocalizationService.ForSection("Designer.Services")("ServiceName.Column")
        ColumnHeader16.Text = LocalizationService.ForSection("Designer.Services")("DisplayName.Column")
        ColumnHeader17.Text = LocalizationService.ForSection("Designer.Services")("Type.Column")
        Label16.Text = LocalizationService.ForSection("Designer.Services")("Part.Group.Label")
        SaveServiceInfoBtn.Text = LocalizationService.ForSection("Designer.Services")("Save.Changes.Label")
        ProgressLabel.Text = LocalizationService.ForSection("Designer.Services")("ProgressLabel.Label")
        ReloadServiceInformationBtn.Text = LocalizationService.ForSection("Designer.Services")("Reload.Label")
        Label15.Text = LocalizationService.ForSection("Designer.Services")("SelectService.Label")
        ReportServiceInfoBtn.Text = LocalizationService.ForSection("Designer.Services")("Save.Button")
        ServiceInfoSFD.Filter = LocalizationService.ForSection("Designer.Services")("MarkdownFiles.Filter")
        RestoreServiceBtn.Text = LocalizationService.ForSection("Designer.Services")("RestoreService.Label")
        DeleteServiceBtn.Text = LocalizationService.ForSection("Designer.Services")("DeleteService.Label")
        Text = LocalizationService.ForSection("Designer.Services")("System.Label")

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
        TabPage6.BackColor = BackColor
        TabPage6.ForeColor = ForeColor
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

        ModifiedServiceList.Clear()
        isModified = False

        If Not Debugger.IsAttached Then DynaLog.DisableLogging()
        ServiceList = WindowsServiceHelper.GetServiceList(MainForm.MountDir)
        If Not Debugger.IsAttached Then DynaLog.EnableLogging()

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
            If CriticalServiceNames.Contains(ServiceList(ListView1.FocusedItem.Index).Name) Then
                DisplayCriticalServiceWarning()
                Exit Sub
            End If

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

        If Not Debugger.IsAttached Then DynaLog.DisableLogging()
        ServiceList = WindowsServiceHelper.GetServiceList(MainForm.MountDir)
        If Not Debugger.IsAttached Then DynaLog.EnableLogging()
        
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
            ImgInfoSaveDlg.DoNotAskOnNonComplete = MainForm.DoNotAskOnNonComplete
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Private Sub DeleteServiceBtn_Click(sender As Object, e As EventArgs) Handles DeleteServiceBtn.Click
        If ListView1.SelectedItems.Count = 1 Then
            If CriticalServiceNames.Contains(ServiceList(ListView1.FocusedItem.Index).Name) Then
                DisplayCriticalServiceWarning()
                Exit Sub
            End If

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

    Private Sub btnAllSafeModes_Click(sender As Object, e As EventArgs) Handles btnAllSafeModes.Click
        CheckBox2.Checked = True
        CheckBox3.Checked = True
    End Sub

    Private Sub btnNoSafeModes_Click(sender As Object, e As EventArgs) Handles btnNoSafeModes.Click
        CheckBox2.Checked = False
        CheckBox3.Checked = False
    End Sub

    Private Sub DisplayCriticalServiceWarning()
        MessageBox.Show(Me, "This service can only be viewed because it is critical for core Windows components to function. Improper configuration of this service will result in an unstable system.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If ListView1.SelectedItems.Count = 1 Then
            If CriticalServiceNames.Contains(ServiceList(ListView1.FocusedItem.Index).Name) Then
                DisplayCriticalServiceWarning()
                Exit Sub
            End If

            ' Hold a copy of the service so we can queue it for modification
            Dim newService As WindowsService = ServiceList(ListView1.FocusedItem.Index)

            ' For a service to work correctly in Safe Mode (when the checkbox is checked), any of its dependencies
            ' need to be enabled in Safe Mode too, as well as the dependencies of those dependencies, as well as
            ' the dependencies of the dependencies of those dependencies...
            '
            '                                                       Dependency 1 of dependency 1 \
            '                                                                                     \
            '                                                       Dependency 2 of dependency 1 ---- Dependency 1 \
            '                                                                                     /                 \
            '                                                       Dependency 3 of dependency 1 /                   \
            '                                                                                                         \
            '     Dependency 1 of dependency 1 of dependency 2 \                                                      ---------- Main Service
            '                                                   --- Dependency 1 of dependency 2 \                   /
            '     Dependency 2 of dependency 1 of dependency 2 /                                  \                 /
            '                                                                                      -- Dependency 2 /
            '                                                                                     /
            '                                                       Dependency 2 of dependency 2 /
            '
            ' For disabling a service in Safe Mode, the dependencies need to be disabled, as well as its dependents, 
            ' as well as the dependents of those dependents. Then, the dependents of that service need to be disabled,
            ' plus other dependencies, plus their dependents; as well as those dependents' dependents...
            '
            '                                    /---- Dependent of dependency 1 of dependency 1
            '                                   /
            '     Dependency 1 of dependency 1 ---------------------------------------------------------------- Dependency 1 --------- Dependent                   ------ Dependent 1 ------------ Dependent 1 of dependent 1
            '                                                                                         /                       \                                   /                        \
            '                                    /---- Dependent 1 of dependency 2 of dependency 1   /                         \                                 /                          ------ Dependent 2 of dependent 1
            '                                   /                                                   /                           \                               /                            \
            '     Dependency 2 of dependency 1 -----------------------------------------------------                             \                             /                              ---- Dependent 3 of dependent 1
            '                                   \                                                                                 \                           /
            '                                    \---- Dependent 2 of dependency 2 of dependency 1                                 ------------ Main Service ------------ Dependent 2 ------------ Dependent of dependent 2
            '                                                                                                                     /                           \
            '                                                                                                                    /                             \
            '                                                                                                                   /                               \                     
            '                                                                                                                  /                                 \                      ---------- Dependent 1 of dependent 3
            '                                                                                                                 /                                   \                    /
            '                                                                                                   Dependency 2 --------- Dependent                   ------ Dependent 3 ------------ Dependent 2 of dependent 3
            '                                                                                                                                                                          \
            '                                                                                                                                                                           ---------- Dependent 3 of dependent 3
            '
            ' Alright, I'm going to stop.
            Dim AdditionalServiceNames As New List(Of String),
                ImpliedServices As IEnumerable(Of WindowsService) = Nothing
            If CheckBox2.Checked Then
                AdditionalServiceNames = EnumerateServiceDependenciesForSafeModeToggles(newService, Not CheckBox2.Checked, False).Where(Function(service) Not service = newService.Name).Distinct().ToList()

                If AdditionalServiceNames.Any() Then
                    ImpliedServices = ServiceList.Where(Function(service) AdditionalServiceNames.Contains(service.Name))

                    ImpliedServicesInSafeBootEnablementDialog.ImpliedServices = ImpliedServices
                    Dim userChoice As DialogResult = ImpliedServicesInSafeBootEnablementDialog.ShowDialog(Me)
                    If userChoice <> Windows.Forms.DialogResult.Yes Then
                        ' restore the previous state
                        RemoveHandler CheckBox2.CheckedChanged, AddressOf CheckBox2_CheckedChanged
                        CheckBox2.Checked = False
                        AddHandler CheckBox2.CheckedChanged, AddressOf CheckBox2_CheckedChanged
                        Exit Sub
                    End If

                    If userChoice = Windows.Forms.DialogResult.Yes Then
                        ' Add the additional services first
                        For Each ImpliedService In ImpliedServices
                            ImpliedService.SafeModeOptions.AvailableInMinimalSafeBoot = True

                            Dim modifiedSvcIndex As Integer = ModifiedServiceList.FindIndex(Function(svc) svc.Name.Equals(ImpliedService.Name, StringComparison.OrdinalIgnoreCase)),
                                svcIndex As Integer = ServiceList.FindIndex(Function(svc) svc.Name.Equals(ImpliedService.Name, StringComparison.OrdinalIgnoreCase))
                            ServiceList(svcIndex).SafeModeOptions.AvailableInMinimalSafeBoot = True
                            If modifiedSvcIndex > -1 Then
                                ModifiedServiceList(modifiedSvcIndex) = ImpliedService
                            Else
                                ModifiedServiceList.Add(ImpliedService)
                            End If
                        Next
                    End If
                End If
            Else
                AdditionalServiceNames = EnumerateServiceDependentsForSafeModeToggles(newService, Not CheckBox2.Checked, False).Distinct().ToList()
                Dim ServiceDependencies As List(Of String) = EnumerateServiceDependenciesForSafeModeToggles(newService, Not CheckBox2.Checked, False).Distinct().ToList(),
                    ServiceDependentsExclusiveToMainService As List(Of String) = New List(Of String)(AdditionalServiceNames)

                For Each ServiceDependency In ServiceDependencies
                    If Not ServiceList.Any(Function(service) service.Name = ServiceDependency) Then Continue For

                    Dim dependencyService As WindowsService = ServiceList.First(Function(service) service.Name = ServiceDependency)
                    AdditionalServiceNames.AddRange(EnumerateServiceDependentsForSafeModeToggles(dependencyService, Not CheckBox2.Checked, False, newService.Name).Distinct().ToArray())
                    AdditionalServiceNames = AdditionalServiceNames.Distinct().ToList()
                Next

                AdditionalServiceNames = AdditionalServiceNames.Where(Function(service) Not service = newService.Name).ToList()

                Dim AdditionalServices As New Dictionary(Of String, List(Of WindowsService)) From {
                    {"dependencies", ServiceList.Where(Function(service) ServiceDependencies.Contains(service.Name)).ToList()},
                    {"allDependents", ServiceList.Where(Function(service) AdditionalServiceNames.Contains(service.Name)).ToList()},
                    {"mainServiceDependents", ServiceList.Where(Function(service) ServiceDependentsExclusiveToMainService.Contains(service.Name)).ToList()}
                }

                Dim warrantedDialogShown As Boolean = AdditionalServices.Any(Function(kvp) kvp.Value.Any())
                If warrantedDialogShown Then
                    ImpliedServicesInSafeBootDisablementDialog.ImpliedServices = AdditionalServices

                    Dim userChoice As DialogResult = ImpliedServicesInSafeBootDisablementDialog.ShowDialog(Me)
                    If userChoice <> Windows.Forms.DialogResult.Yes Then
                        ' restore the previous state
                        RemoveHandler CheckBox2.CheckedChanged, AddressOf CheckBox2_CheckedChanged
                        CheckBox2.Checked = True
                        AddHandler CheckBox2.CheckedChanged, AddressOf CheckBox2_CheckedChanged
                        Exit Sub
                    End If

                    If userChoice = Windows.Forms.DialogResult.Yes Then
                        If ImpliedServicesInSafeBootDisablementDialog.ImplyServiceDependencies Then
                            ' Add the additional services first
                            For Each ImpliedService In AdditionalServices("dependencies")
                                ImpliedService.SafeModeOptions.AvailableInMinimalSafeBoot = False

                                Dim modifiedSvcIndex As Integer = ModifiedServiceList.FindIndex(Function(svc) svc.Name.Equals(ImpliedService.Name, StringComparison.OrdinalIgnoreCase)),
                                    svcIndex As Integer = ServiceList.FindIndex(Function(svc) svc.Name.Equals(ImpliedService.Name, StringComparison.OrdinalIgnoreCase))
                                ServiceList(svcIndex).SafeModeOptions.AvailableInMinimalSafeBoot = False
                                If modifiedSvcIndex > -1 Then
                                    ModifiedServiceList(modifiedSvcIndex) = ImpliedService
                                Else
                                    ModifiedServiceList.Add(ImpliedService)
                                End If
                            Next
                        End If

                        For Each ImpliedService In AdditionalServices("allDependents")
                            ImpliedService.SafeModeOptions.AvailableInMinimalSafeBoot = False

                            Dim modifiedSvcIndex As Integer = ModifiedServiceList.FindIndex(Function(svc) svc.Name.Equals(ImpliedService.Name, StringComparison.OrdinalIgnoreCase)),
                                svcIndex As Integer = ServiceList.FindIndex(Function(svc) svc.Name.Equals(ImpliedService.Name, StringComparison.OrdinalIgnoreCase))
                            ServiceList(svcIndex).SafeModeOptions.AvailableInMinimalSafeBoot = False
                            If modifiedSvcIndex > -1 Then
                                ModifiedServiceList(modifiedSvcIndex) = ImpliedService
                            Else
                                ModifiedServiceList.Add(ImpliedService)
                            End If
                        Next
                    End If
                End If
            End If

            ServiceList(ListView1.FocusedItem.Index).SafeModeOptions.AvailableInMinimalSafeBoot = CheckBox2.Checked
            newService.SafeModeOptions.AvailableInMinimalSafeBoot = CheckBox2.Checked

            ' Store it in the modification queue, or update it
            If ModifiedServiceList.Any(Function(svc) svc.Name.Equals(newService.Name, StringComparison.OrdinalIgnoreCase)) Then
                Dim svcIndex As Integer = ModifiedServiceList.FindIndex(Function(svc) svc.Name.Equals(newService.Name, StringComparison.OrdinalIgnoreCase))
                ModifiedServiceList(svcIndex) = newService
            Else
                ModifiedServiceList.Add(newService)
            End If
        End If
    End Sub

    Private Function EnumerateServiceDependenciesForSafeModeToggles(BaseService As WindowsService, ExpectedSafebootSetting As Boolean, NetworkedSafeboot As Boolean) As List(Of String)
        Dim svcDeps As New List(Of String)

        For Each ServiceDependency In BaseService.Dependencies
            If Not ServiceList.Any(Function(service) service.Name = ServiceDependency) Then Continue For
            Dim dependencyService As WindowsService = ServiceList.First(Function(service) service.Name = ServiceDependency)

            Dim serviceMeetsSafeModeToggles As Boolean = If(NetworkedSafeboot, dependencyService.SafeModeOptions.AvailableInNetworkSafeBoot, dependencyService.SafeModeOptions.AvailableInMinimalSafeBoot) = ExpectedSafebootSetting
            If serviceMeetsSafeModeToggles Then svcDeps.Add(ServiceDependency)

            If dependencyService.Dependencies.Any() Then svcDeps.AddRange(EnumerateServiceDependenciesForSafeModeToggles(dependencyService, ExpectedSafebootSetting, NetworkedSafeboot))
        Next

        Return svcDeps
    End Function

    Private Function EnumerateServiceDependentsForSafeModeToggles(BaseService As WindowsService, ExpectedSafebootSetting As Boolean, NetworkedSafeboot As Boolean, Optional BaseServiceName As String = "") As List(Of String)
        Dim dependents As New List(Of String)

        For Each ServiceDependent In ServiceList.Where(Function(service) service.Dependencies.Contains(BaseService.Name))
            If BaseServiceName <> "" And ServiceDependent.Dependencies.Contains(BaseServiceName) Then Continue For

            Dim serviceMeetsSafeModeToggles As Boolean = If(NetworkedSafeboot, ServiceDependent.SafeModeOptions.AvailableInNetworkSafeBoot, ServiceDependent.SafeModeOptions.AvailableInMinimalSafeBoot) = ExpectedSafebootSetting
            If serviceMeetsSafeModeToggles Then dependents.Add(ServiceDependent.Name)

            Dim ServiceDependentSubDependents As IEnumerable(Of WindowsService) = ServiceList.Where(Function(service) service.Dependencies.Contains(ServiceDependent.Name))
            For Each ServiceDependentSubDependent In ServiceDependentSubDependents
                dependents.AddRange(EnumerateServiceDependentsForSafeModeToggles(ServiceDependentSubDependent, ExpectedSafebootSetting, NetworkedSafeboot, BaseServiceName))
            Next
        Next

        Return dependents
    End Function

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If ListView1.SelectedItems.Count = 1 Then
            If CriticalServiceNames.Contains(ServiceList(ListView1.FocusedItem.Index).Name) Then
                DisplayCriticalServiceWarning()
                Exit Sub
            End If

            ' Hold a copy of the service so we can queue it for modification
            Dim newService As WindowsService = ServiceList(ListView1.FocusedItem.Index)

            ' look at the comment from checkbox2; i'm not repeating it here.
            Dim AdditionalServiceNames As New List(Of String),
                ImpliedServices As IEnumerable(Of WindowsService) = Nothing
            If CheckBox3.Checked Then
                AdditionalServiceNames = EnumerateServiceDependenciesForSafeModeToggles(newService, Not CheckBox3.Checked, True).Where(Function(service) Not service = newService.Name).Distinct().ToList()

                If AdditionalServiceNames.Any() Then
                    ImpliedServices = ServiceList.Where(Function(service) AdditionalServiceNames.Contains(service.Name))

                    ImpliedServicesInSafeBootEnablementDialog.ImpliedServices = ImpliedServices
                    Dim userChoice As DialogResult = ImpliedServicesInSafeBootEnablementDialog.ShowDialog(Me)
                    If userChoice <> Windows.Forms.DialogResult.Yes Then
                        ' restore the previous state
                        RemoveHandler CheckBox3.CheckedChanged, AddressOf CheckBox3_CheckedChanged
                        CheckBox3.Checked = False
                        AddHandler CheckBox3.CheckedChanged, AddressOf CheckBox3_CheckedChanged
                        Exit Sub
                    End If

                    If userChoice = Windows.Forms.DialogResult.Yes Then
                        ' Add the additional services first
                        For Each ImpliedService In ImpliedServices
                            ImpliedService.SafeModeOptions.AvailableInNetworkSafeBoot = True

                            Dim modifiedSvcIndex As Integer = ModifiedServiceList.FindIndex(Function(svc) svc.Name.Equals(ImpliedService.Name, StringComparison.OrdinalIgnoreCase)),
                                svcIndex As Integer = ServiceList.FindIndex(Function(svc) svc.Name.Equals(ImpliedService.Name, StringComparison.OrdinalIgnoreCase))
                            ServiceList(svcIndex).SafeModeOptions.AvailableInNetworkSafeBoot = True
                            If modifiedSvcIndex > -1 Then
                                ModifiedServiceList(modifiedSvcIndex) = ImpliedService
                            Else
                                ModifiedServiceList.Add(ImpliedService)
                            End If
                        Next
                    End If
                End If
            Else
                AdditionalServiceNames = EnumerateServiceDependentsForSafeModeToggles(newService, Not CheckBox3.Checked, False).Distinct().ToList()
                Dim ServiceDependencies As List(Of String) = EnumerateServiceDependenciesForSafeModeToggles(newService, Not CheckBox3.Checked, True).Distinct().ToList(),
                    ServiceDependentsExclusiveToMainService As List(Of String) = New List(Of String)(AdditionalServiceNames)

                For Each ServiceDependency In ServiceDependencies
                    If Not ServiceList.Any(Function(service) service.Name = ServiceDependency) Then Continue For

                    Dim dependencyService As WindowsService = ServiceList.First(Function(service) service.Name = ServiceDependency)
                    AdditionalServiceNames.AddRange(EnumerateServiceDependentsForSafeModeToggles(dependencyService, Not CheckBox3.Checked, True, newService.Name).Distinct().ToArray())
                    AdditionalServiceNames = AdditionalServiceNames.Distinct().ToList()
                Next

                AdditionalServiceNames = AdditionalServiceNames.Where(Function(service) Not service = newService.Name).ToList()

                Dim AdditionalServices As New Dictionary(Of String, List(Of WindowsService)) From {
                    {"dependencies", ServiceList.Where(Function(service) ServiceDependencies.Contains(service.Name)).ToList()},
                    {"allDependents", ServiceList.Where(Function(service) AdditionalServiceNames.Contains(service.Name)).ToList()},
                    {"mainServiceDependents", ServiceList.Where(Function(service) ServiceDependentsExclusiveToMainService.Contains(service.Name)).ToList()}
                }

                Dim warrantedDialogShown As Boolean = AdditionalServices.Any(Function(kvp) kvp.Value.Any())
                If warrantedDialogShown Then
                    ImpliedServicesInSafeBootDisablementDialog.ImpliedServices = AdditionalServices

                    Dim userChoice As DialogResult = ImpliedServicesInSafeBootDisablementDialog.ShowDialog(Me)
                    If userChoice <> Windows.Forms.DialogResult.Yes Then
                        ' restore the previous state
                        RemoveHandler CheckBox3.CheckedChanged, AddressOf CheckBox3_CheckedChanged
                        CheckBox3.Checked = True
                        AddHandler CheckBox3.CheckedChanged, AddressOf CheckBox3_CheckedChanged
                        Exit Sub
                    End If

                    If userChoice = Windows.Forms.DialogResult.Yes Then
                        If ImpliedServicesInSafeBootDisablementDialog.ImplyServiceDependencies Then
                            ' Add the additional services first
                            For Each ImpliedService In AdditionalServices("dependencies")
                                ImpliedService.SafeModeOptions.AvailableInNetworkSafeBoot = False

                                Dim modifiedSvcIndex As Integer = ModifiedServiceList.FindIndex(Function(svc) svc.Name.Equals(ImpliedService.Name, StringComparison.OrdinalIgnoreCase)),
                                    svcIndex As Integer = ServiceList.FindIndex(Function(svc) svc.Name.Equals(ImpliedService.Name, StringComparison.OrdinalIgnoreCase))
                                ServiceList(svcIndex).SafeModeOptions.AvailableInNetworkSafeBoot = False
                                If modifiedSvcIndex > -1 Then
                                    ModifiedServiceList(modifiedSvcIndex) = ImpliedService
                                Else
                                    ModifiedServiceList.Add(ImpliedService)
                                End If
                            Next
                        End If

                        For Each ImpliedService In AdditionalServices("allDependents")
                            ImpliedService.SafeModeOptions.AvailableInNetworkSafeBoot = False

                            Dim modifiedSvcIndex As Integer = ModifiedServiceList.FindIndex(Function(svc) svc.Name.Equals(ImpliedService.Name, StringComparison.OrdinalIgnoreCase)),
                                svcIndex As Integer = ServiceList.FindIndex(Function(svc) svc.Name.Equals(ImpliedService.Name, StringComparison.OrdinalIgnoreCase))
                            ServiceList(svcIndex).SafeModeOptions.AvailableInNetworkSafeBoot = False
                            If modifiedSvcIndex > -1 Then
                                ModifiedServiceList(modifiedSvcIndex) = ImpliedService
                            Else
                                ModifiedServiceList.Add(ImpliedService)
                            End If
                        Next
                    End If
                End If
            End If

            ServiceList(ListView1.FocusedItem.Index).SafeModeOptions.AvailableInNetworkSafeBoot = CheckBox3.Checked
            newService.SafeModeOptions.AvailableInNetworkSafeBoot = CheckBox3.Checked

            ' Store it in the modification queue, or update it
            If ModifiedServiceList.Any(Function(svc) svc.Name.Equals(newService.Name, StringComparison.OrdinalIgnoreCase)) Then
                Dim svcIndex As Integer = ModifiedServiceList.FindIndex(Function(svc) svc.Name.Equals(newService.Name, StringComparison.OrdinalIgnoreCase))
                ModifiedServiceList(svcIndex) = newService
            Else
                ModifiedServiceList.Add(newService)
            End If
        End If
    End Sub

    Private Sub ViewAsGraphBtn_Click(sender As Object, e As EventArgs) Handles ViewAsGraphBtn.Click
        If ListView1.SelectedItems.Count = 0 Then Exit Sub

        Dim selectedService As WindowsService = ServiceList.ElementAtOrDefault(ListView1.FocusedItem.Index)
        If selectedService Is Nothing Then Exit Sub

        Dim dependencies As IEnumerable(Of WindowsService) = ServiceList.Where(Function(service) selectedService.Dependencies.Contains(service.Name)).OrderBy(Function(service) service.DisplayName),
            dependents As IEnumerable(Of WindowsService) = ServiceList.Where(Function(service) service.Dependencies.Contains(selectedService.Name)).OrderBy(Function(service) service.DisplayName)

        Dim servicesToShow As New List(Of WindowsService)
        servicesToShow.Add(selectedService)
        servicesToShow.AddRange(dependencies)
        servicesToShow.AddRange(dependents)

        ServiceDependencyGraphViewer.ServicesToDisplay = servicesToShow.AsEnumerable()
        ServiceDependencyGraphViewer.MainServiceName = selectedService.Name
        If ServiceDependencyGraphViewer.Visible Then
            ServiceDependencyGraphViewer.RedisplayServices()
            If ServiceDependencyGraphViewer.WindowState = FormWindowState.Minimized Then ServiceDependencyGraphViewer.WindowState = FormWindowState.Normal
            ServiceDependencyGraphViewer.BringToFront()
        Else
            ServiceDependencyGraphViewer.Show()
        End If
    End Sub
End Class