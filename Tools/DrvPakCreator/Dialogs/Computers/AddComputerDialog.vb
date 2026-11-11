Imports System.Windows.Forms
Imports Microsoft.Win32
Imports DrvPakCreator.Classes
Imports System.IO

Public Class AddComputerDialog

    Private Enum WizardPage As Integer
        Welcome = 0
        ComputerInfo = 1
        DriverAssociation = 2
        Finish = 3
    End Enum

    Private Const WizardPages As Integer = 4

    Private CurrentWizardPage As WizardPage
    Private VerifyInPages As New List(Of WizardPage)

    Public CurrentDriverPack As DriverPack
    Public NewComputer As Computer

    Private Sub ChangePage(ByVal NewPage As WizardPage)
        If NewPage > CurrentWizardPage AndAlso VerifyInPages.Contains(CurrentWizardPage) Then
            If Not VerifyOptionsInPage(CurrentWizardPage) Then Exit Sub
        End If

        pnlWelcome.Visible = NewPage = WizardPage.Welcome
        pnlCompInfo.Visible = NewPage = WizardPage.ComputerInfo
        pnlDrvAssoc.Visible = NewPage = WizardPage.DriverAssociation
        pnlFinish.Visible = NewPage = WizardPage.Finish

        WizardStep1.WizardStepIsActive = NewPage = WizardPage.Welcome
        WizardStep2.WizardStepIsActive = NewPage = WizardPage.ComputerInfo
        WizardStep3.WizardStepIsActive = NewPage = WizardPage.DriverAssociation
        WizardStep4.WizardStepIsActive = NewPage = WizardPage.Finish

        Next_Button.Text = IIf(NewPage = WizardPage.Finish, "Finish", "Next")

        Back_Button.Enabled = NewPage > WizardPage.Welcome
        Cancel_Button.Enabled = NewPage < WizardPage.Finish

        CurrentWizardPage = NewPage

        If CurrentWizardPage = WizardPage.DriverAssociation Then
            DisplayDriverPackDriverFiles()
        ElseIf CurrentWizardPage = WizardPage.Finish Then
            ConstructComputer()
            ShowComputerAdditionOverview()
        End If

    End Sub

    Private Function VerifyOptionsInPage(ByVal Page As WizardPage) As Boolean
        Select Case Page
            Case WizardPage.ComputerInfo

        End Select
        Return True
    End Function

    Private Sub DetectComputerInfo()
        Refresh()
        SetSystemInformation()
    End Sub

    Private Sub DisplayDriverPackDriverFiles()
        lvDriverFiles.Items.Clear()
        If CurrentDriverPack.DriverFiles.Drivers.Count > 0 Then
            Dim lvItems As New List(Of ListViewItem)

            For Each DriverFile As Driver In CurrentDriverPack.DriverFiles.Drivers
                lvItems.Add(New ListViewItem(New String() {DriverFile.Name, Path.GetFileName(DriverFile.MainFile)}))
            Next

            lvDriverFiles.Items.AddRange(lvItems.ToArray())
        Else

        End If
    End Sub

    Private Sub AddComputerDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ChangePage(WizardPage.Welcome)
    End Sub

    Private Sub Back_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Back_Button.Click
        ChangePage(CurrentWizardPage - 1)
    End Sub

    Private Sub Next_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Next_Button.Click
        If CurrentWizardPage = WizardPage.Finish Then
            DialogResult = Windows.Forms.DialogResult.OK
            Close()
        Else
            ChangePage(CurrentWizardPage + 1)
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Close()
    End Sub

    Private Sub ConstructComputer()
        NewComputer = New Computer()
        Dim selectedDriverIds As New List(Of Integer)
        For Each SelectedDriver As ListViewItem In lvDriverFiles.CheckedItems
            selectedDriverIds.Add(SelectedDriver.Index + 1)
        Next
        With NewComputer
            .Id = CurrentDriverPack.Computers.Computers.Count + 1
            .ComputerInformation = New ComputerInfo()
            With .ComputerInformation
                .Manufacturer = tbSysManufacturer.Text
                .Model = tbSysModel.Text
                .Processor = tbSysProcessor.Text
                .MemoryCapacity = nudMemory.Value
                .StorageCapacity = nudStorage.Value
                Dim targetGuid As Guid = Guid.Empty
                Try
                    If String.IsNullOrEmpty(tbSysUuid.Text) Then
                        targetGuid = Guid.Empty
                    Else
                        targetGuid = New Guid(tbSysUuid.Text)
                    End If
                Catch ex As Exception
                    targetGuid = Guid.Empty
                End Try
                .Uuid = targetGuid
                .OperatingSystem = New OperatingSystemInfo()
                With .OperatingSystem
                    .Name = tbSysOSName.Text
                    .SKU = tbSysOSSku.Text
                    .VersionString = tbSysOSVer.Text
                End With
            End With
            .AssociatedDriverIds = selectedDriverIds
        End With
    End Sub

    Private Sub ShowComputerAdditionOverview()
        Dim SelectedDriverFileString As String = ""

        tbComputerAdditionOverview.Text = String.Format("The following computer will be added to your driver pack:{0}" & _
                                                        "    {1}{0}" & _
                                                        "...with the following specifications:{0}" & _
                                                        "- Processor : {2}{0}" & _
                                                        "- Memory    : {3} MB{0}" & _
                                                        "- Storage   : {4} MB{0}" & _
                                                        "- UUID      : {5}{0}" & _
                                                        "- OS Name   : {6}{0}" & _
                                                        "- OS SKU    : {7}{0}" & _
                                                        "- OS Version: {8}{0}" & _
                                                        "{9} device(s) will be associated to the computer. Review this information and click Finish if everything looks right.", Environment.NewLine, _
                                                        String.Format("{0} {1}", NewComputer.ComputerInformation.Manufacturer, NewComputer.ComputerInformation.Model), _
                                                        NewComputer.ComputerInformation.Processor, NewComputer.ComputerInformation.MemoryCapacity, NewComputer.ComputerInformation.StorageCapacity, _
                                                        NewComputer.ComputerInformation.Uuid, NewComputer.ComputerInformation.OperatingSystem.Name, NewComputer.ComputerInformation.OperatingSystem.SKU, _
                                                        NewComputer.ComputerInformation.OperatingSystem.VersionString, lvDriverFiles.CheckedItems.Count)
    End Sub

    Private Sub SetSystemInformation()
        Dim ComputerSystemDetails As Dictionary(Of String, Object) = Nothing, _
            ComputerSystemProductDetails As Dictionary(Of String, Object) = Nothing, _
            ProcessorDetails As Dictionary(Of String, Object) = Nothing, _
            StorageDetails As Dictionary(Of String, Object) = Nothing, _
            OperatingSystemDetails As Dictionary(Of String, Object) = Nothing

        Cursor = Cursors.WaitCursor

        ' Computer System
        Try
            Dim ComputerSystemMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery("SELECT Manufacturer, Model FROM Win32_ComputerSystem")
            If ComputerSystemMOC Is Nothing Then Throw New Exception()
#If VBC_VER >= 9.0 Then
            ComputerSystemDetails = WMIHelper.GetObjectValues(ComputerSystemMOC(0), "Manufacturer", "Model")
#Else
            Dim CS_MOE As ManagementObjectCollection.ManagementObjectEnumerator = ComputerSystemMOC.GetEnumerator()
            If CS_MOE.MoveNext() Then
                ComputerSystemDetails = WMIHelper.GetObjectValues(CS_MOE.Current, "Manufacturer", "Model")
            End If
#End If
            tbSysManufacturer.Text = ComputerSystemDetails("Manufacturer")
            tbSysModel.Text = ComputerSystemDetails("Model")
        Catch ex As Exception

        End Try

        ' Computer System Product
        Try
            Dim ComputerSystemProductMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery("SELECT UUID FROM Win32_ComputerSystemProduct")
            If ComputerSystemProductMOC Is Nothing Then Throw New Exception()
#If VBC_VER >= 9.0 Then
            ComputerSystemProductDetails = WMIHelper.GetObjectValues(ComputerSystemProductMOC(0), "UUID")
#Else
            Dim CSP_MOE As ManagementObjectCollection.ManagementObjectEnumerator = ComputerSystemProductMOC.GetEnumerator()
            If CSP_MOE.MoveNext() Then
                ComputerSystemProductDetails = WMIHelper.GetObjectValues(CSP_MOE.Current, "UUID")
            End If
#End If
            tbSysUuid.Text = ComputerSystemProductDetails("UUID")
        Catch ex As Exception

        End Try

        ' Processor
        Try
            Dim ProcessorMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery("SELECT Name FROM Win32_Processor")
            If ProcessorMOC Is Nothing Then Throw New Exception()

#If VBC_VER >= 9.0 Then
            ProcessorDetails = WMIHelper.GetObjectValues(ProcessorMOC(0), "Name")
#Else
            Dim Proc_MOE As ManagementObjectCollection.ManagementObjectEnumerator = ProcessorMOC.GetEnumerator()
            If Proc_MOE.MoveNext() Then
                ProcessorDetails = WMIHelper.GetObjectValues(Proc_MOE.Current, "Name")
            End If
#End If
            tbSysProcessor.Text = ProcessorDetails("Name")
        Catch ex As Exception

        End Try

        ' Memory
        Try
            Dim MemoryMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery("SELECT Capacity FROM Win32_PhysicalMemory")
            Dim Memory_MOE As ManagementObjectCollection.ManagementObjectEnumerator = MemoryMOC.GetEnumerator()

            Dim TotalMemoryCapacity As ULong = 0UL

            While Memory_MOE.MoveNext()
                TotalMemoryCapacity += CULng(WMIHelper.GetObjectValue(Memory_MOE.Current, "Capacity"))
            End While

            nudMemory.Value = TotalMemoryCapacity / 1048576
        Catch ex As Exception

        End Try

        ' Storage
        Try
            Dim StorageMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT Size FROM Win32_LogicalDisk WHERE DeviceID = {0}{1}{0}", Quote, WMIHelper.GetEscapedValue(Environment.GetEnvironmentVariable("SYSTEMDRIVE"))))
#If VBC_VER >= 9.0 Then
            StorageDetails = WMIHelper.GetObjectValues(StorageMOC(0), "Size")
#Else
            Dim Storage_MOE As ManagementObjectCollection.ManagementObjectEnumerator = StorageMOC.GetEnumerator()
            If Storage_MOE.MoveNext() Then
                StorageDetails = WMIHelper.GetObjectValues(Storage_MOE.Current, "Size")
            End If
#End If
            nudStorage.Value = CULng(StorageDetails("Size")) / 1048576
        Catch ex As Exception

        End Try

        ' Operating System
        Try
            Dim WinMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery("SELECT Caption, Version FROM Win32_OperatingSystem")
#If VBC_VER >= 9.0 Then
            OperatingSystemDetails = WMIHelper.GetObjectValues(WinMOC(0), "Caption", "Version")
#Else
            Dim Win_MOE As ManagementObjectCollection.ManagementObjectEnumerator = WinMOC.GetEnumerator()
            If Win_MOE.MoveNext() Then
                OperatingSystemDetails = WMIHelper.GetObjectValues(Win_MOE.Current, "Caption", "Version")
            End If
#End If
            Dim SysRk As RegistryKey = Nothing
            Dim UBR As UInteger = 0
            Try
                ' Try to get edition; on NT 5.x this will not work because of the lack of an edition ID.
                ' This starts working on NT 6.
                SysRk = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion", False)
                OperatingSystemDetails.Add("OsEdition", SysRk.GetValue("EditionID", ""))

                ' To grab the full version we need the revision number: on NT 5.x we simply look at ntoskrnl revision;
                ' on NT 6.2 and higher we look at UBR
                If Environment.OSVersion.Version.Major < 6 Then
                    Try
                        Dim ntoskrnlPath As String = Path.Combine(Environment.GetEnvironmentVariable("SYSTEMROOT"), "system32\ntoskrnl.exe")
                        Dim ntosFvi As FileVersionInfo = FileVersionInfo.GetVersionInfo(ntoskrnlPath)
                        UBR = ntosFvi.FilePrivatePart
                    Catch ex As Exception
                        UBR = 0
                    End Try
                ElseIf Environment.OSVersion.Version.Major = 6 Then
                    ' Windows Vista (NT 6.0) and Windows 7 (NT 6.1) do not have UBR
                    If Environment.OSVersion.Version.Minor = 0 Or Environment.OSVersion.Version.Minor = 1 Then
                        Try
                            Dim ntoskrnlPath As String = Path.Combine(Environment.GetEnvironmentVariable("SYSTEMROOT"), "system32\ntoskrnl.exe")
                            Dim ntosFvi As FileVersionInfo = FileVersionInfo.GetVersionInfo(ntoskrnlPath)
                            UBR = ntosFvi.FilePrivatePart
                        Catch ex As Exception
                            UBR = 0
                        End Try
                    Else
                        UBR = SysRk.GetValue("UBR", 0)
                    End If
                Else
                    UBR = SysRk.GetValue("UBR", 0)
                End If
            Catch ex As Exception

            Finally
                If SysRk IsNot Nothing Then SysRk.Close()
            End Try

            tbSysOSName.Text = OperatingSystemDetails("Caption")
            tbSysOSSku.Text = OperatingSystemDetails("OsEdition")
            tbSysOSVer.Text = String.Format("{0}.{1}", OperatingSystemDetails("Version"), UBR)
        Catch ex As Exception

        End Try

        Cursor = Cursors.Arrow
    End Sub

    Private Sub btnSysInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSysInfo.Click
        SetSystemInformation()
    End Sub
End Class
