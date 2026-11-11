Imports System.Windows.Forms
Imports Microsoft.Win32
Imports DrvPakCreator.Classes
Imports DrvPakCreator.Classes.ModelCatalogs
Imports DrvPakCreator.Classes.ModelCatalogs.HP
Imports DrvPakCreator.Classes.ModelCatalogs.Dell
Imports DrvPakCreator.Classes.ModelCatalogs.Lenovo
Imports DrvPakCreator.Helpers
Imports DrvPakCreator.Helpers.ModelCatalogs
Imports DrvPakCreator.Helpers.FileIO
Imports DrvPakCreator.Classes.SetupAPI
Imports System.IO

Public Class AddCurrentComputerDialog

    Private Enum WizardPage As Integer
        Welcome = 0
        ComputerInfoReview = 1
        DriverAssociation = 2
        Finish = 3
    End Enum

    Private Const WizardPages As Integer = 4

    Private CurrentWizardPage As WizardPage
    Private VerifyInPages As New List(Of WizardPage)

    Public CurrentDriverPack As DriverPack
    Public NewComputer As Computer

    ' Wizard Fields

    Public DriverMainFile As String
    Public DriverFileName As String
    Public DriverFileManufacturer As String

    Public DriverPackProjectFilesRoot As String

    Private SystemDrivers As New List(Of Driver)
    Private SystemDriverPaths As New List(Of Dictionary(Of String, String))

    Private Sub ChangePage(ByVal NewPage As WizardPage)
        If NewPage > CurrentWizardPage AndAlso VerifyInPages.Contains(CurrentWizardPage) Then
            If Not VerifyOptionsInPage(CurrentWizardPage) Then Exit Sub
        End If

        pnlWelcome.Visible = NewPage = WizardPage.Welcome
        pnlCompInfoReview.Visible = NewPage = WizardPage.ComputerInfoReview
        pnlDrvSelect.Visible = NewPage = WizardPage.DriverAssociation
        pnlFinish.Visible = NewPage = WizardPage.Finish

        WizardStep1.WizardStepIsActive = NewPage = WizardPage.Welcome
        WizardStep2.WizardStepIsActive = NewPage = WizardPage.ComputerInfoReview
        WizardStep3.WizardStepIsActive = NewPage = WizardPage.DriverAssociation
        WizardStep4.WizardStepIsActive = NewPage = WizardPage.Finish

        Next_Button.Text = IIf(NewPage = WizardPage.Finish, "Finish", "Next")

        Back_Button.Enabled = NewPage > WizardPage.Welcome
        Cancel_Button.Enabled = NewPage < WizardPage.Finish

        If CurrentWizardPage = WizardPage.Welcome AndAlso NewPage = WizardPage.ComputerInfoReview Then
            SetSystemInformation()
        ElseIf CurrentWizardPage = WizardPage.ComputerInfoReview AndAlso NewPage = WizardPage.DriverAssociation Then
            GetSytemDrivers()
        ElseIf CurrentWizardPage = WizardPage.DriverAssociation AndAlso NewPage = WizardPage.Finish Then
            ConstructComputer()
            ShowComputerAdditionOverview()
        End If

        CurrentWizardPage = NewPage
    End Sub

    Private Function VerifyOptionsInPage(ByVal Page As WizardPage) As Boolean
        Select Case Page
            Case WizardPage.DriverAssociation
                If lvDriverFiles.CheckedItems.Count < 1 Then
                    MessageBox.Show(Me, "You must select at least 1 driver from the list.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
        End Select
        Return True
    End Function

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
        End With
    End Sub

    Private Sub ShowComputerAdditionOverview()
        Dim SelectedDriverFileString As String = ""

        For Each CheckedDriverPath As ListViewItem In lvDriverFiles.CheckedItems
            SelectedDriverFileString &= String.Format("- {1} -- {2}{0}", Environment.NewLine, Path.GetFileName(SystemDriverPaths(CheckedDriverPath.Index)("mainFile")), SystemDriverPaths(CheckedDriverPath.Index)("deviceName"))
        Next

        tbComputerAdditionOverview.Text = String.Format("The following computer will be added to your driver pack:{0}" & _
                                                        "    {1}{0}" & _
                                                        "The following devices will be added to your driver pack and will be associated to this computer:{0}" & _
                                                        "{2}{0}" & _
                                                        "Review this information and click Finish if everything looks right.", Environment.NewLine, _
                                                        String.Format("{0} {1}", NewComputer.ComputerInformation.Manufacturer, NewComputer.ComputerInformation.Model), SelectedDriverFileString)
    End Sub

    Private Sub GetSytemDrivers()
        Cursor = Cursors.WaitCursor
        SystemDriverPaths = SystemDriverHelper.EnumerateInstalledThirdPartyDrivers()
        lvDriverFiles.Items.Clear()

        Dim lvItems As New List(Of ListViewItem)

        For Each DriverPath As Dictionary(Of String, String) In SystemDriverPaths
            If Not File.Exists(DriverPath("mainFile")) Then Continue For

            lvItems.Add(New ListViewItem(New String() {DriverPath("deviceName"), Path.GetFileName(DriverPath("mainFile"))}))
        Next

        lvDriverFiles.Items.AddRange(lvItems.ToArray())
        Cursor = Cursors.Arrow
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

    Private Sub AddComputerDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        tbComputerAdditionOverview.BackColor = BackColor
        tbComputerAdditionOverview.ForeColor = ForeColor
        lvDriverFiles.BackColor = BackColor
        lvDriverFiles.ForeColor = ForeColor
        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)

        VerifyInPages.Clear()
        VerifyInPages.Add(WizardPage.DriverAssociation)


        NewComputer = New Computer()
        ChangePage(WizardPage.Welcome)

        lvDriverFiles.Items.Clear()
    End Sub

    Private Sub Back_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Back_Button.Click
        ChangePage(CurrentWizardPage - 1)
    End Sub

    Private Sub Next_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Next_Button.Click
        If CurrentWizardPage = WizardPage.Finish Then
            MessageBox.Show(Me, "This operation will take a while. Please wait until we finish. Click OK to begin the addition.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Cursor = Cursors.WaitCursor

            DriverAdditionProgressDialog.DriverPackProjectFilesRoot = DriverPackProjectFilesRoot

            For Each CheckedDriverEntry As ListViewItem In lvDriverFiles.CheckedItems
                Dim driverInfo As Dictionary(Of String, String) = SystemDriverPaths(CheckedDriverEntry.Index)

                Dim SourceDriverPath As String = Path.GetDirectoryName(driverInfo("mainFile"))

                DriverAdditionProgressDialog.DriverSource = SourceDriverPath
                DriverAdditionProgressDialog.DriverMetadata = CurrentDriverPack

                Dim drvName As String = "", drvManufacturer As String = "", drvMainFile As String = "", drvSubDrivers As New List(Of String)

                ' The main file is already defined in the dictionary.
                drvManufacturer = driverInfo("manufacturer")
                drvName = driverInfo("deviceName")
                drvMainFile = driverInfo("mainFile")

                DriverAdditionProgressDialog.DriverMainFile = drvMainFile.Replace(Path.GetDirectoryName(drvMainFile) & "\", "")
                DriverAdditionProgressDialog.DriverName = drvName
                DriverAdditionProgressDialog.DriverManufacturer = drvManufacturer

                DriverAdditionProgressDialog.ShowDialog(Me)

                CurrentDriverPack = DriverAdditionProgressDialog.DriverMetadata
                Dim newDriver As Driver = DriverAdditionProgressDialog.NewDriver
                newDriver.SubFiles = drvSubDrivers
                CurrentDriverPack.DriverFiles.Drivers.Add(newDriver)

                NewComputer.AssociatedDriverIds.Add(newDriver.Id)
            Next

            Cursor = Cursors.Arrow
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
End Class
