Imports System.Windows.Forms
Imports DrvPakCreator.Classes
Imports System.Text.RegularExpressions
Imports Microsoft.Win32
Imports System.IO

Public Class ChangeComputerInfoDialog

    Public ComputerInformation As ComputerInfo

    Private VersionSyntaxChecker As New Regex("^\d*\.\d*\.\d*\.\d*$")

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If tbSysOSVer.Text <> "" AndAlso Not VersionSyntaxChecker.IsMatch(tbSysOSVer.Text) Then
            MessageBox.Show(Me, "The version field must be written using this format: <number>.<number>.<number>.<number>", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If
        With ComputerInformation
            .Processor = tbSysProcessor.Text
            .MemoryCapacity = nudMemory.Value
            .StorageCapacity = nudStorage.Value
            .OperatingSystem = New OperatingSystemInfo()
            With .OperatingSystem
                .Name = tbSysOSName.Text
                .SKU = tbSysOSSku.Text
                .VersionString = tbSysOSVer.Text
            End With
        End With
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub tcCompInfo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tcCompInfo.SelectedIndexChanged
        Select Case tcCompInfo.SelectedIndex
            Case 0 : Text = String.Format("Change Computer Hardware Information for {0} {1}", ComputerInformation.Manufacturer, ComputerInformation.Model)
            Case 1 : Text = String.Format("Change Computer Software Information for {0} {1}", ComputerInformation.Manufacturer, ComputerInformation.Model)
        End Select
    End Sub

    Private Sub ChangeComputerInfoDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Select Case tcCompInfo.SelectedIndex
            Case 0 : Text = String.Format("Change Computer Hardware Information for {0} {1}", ComputerInformation.Manufacturer, ComputerInformation.Model)
            Case 1 : Text = String.Format("Change Computer Software Information for {0} {1}", ComputerInformation.Manufacturer, ComputerInformation.Model)
        End Select

        tbSysProcessor.Text = ComputerInformation.Processor
        nudMemory.Value = ComputerInformation.MemoryCapacity
        nudStorage.Value = ComputerInformation.StorageCapacity
        tbSysOSName.Text = ComputerInformation.OperatingSystem.Name
        tbSysOSSku.Text = ComputerInformation.OperatingSystem.SKU
        tbSysOSVer.Text = ComputerInformation.OperatingSystem.VersionString

        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor

        tpHardware.BackColor = BackColor
        tpHardware.ForeColor = ForeColor
        tpSoftware.BackColor = BackColor
        tpSoftware.ForeColor = ForeColor

        tbSysProcessor.BackColor = BackColor
        tbSysProcessor.ForeColor = ForeColor
        nudMemory.BackColor = BackColor
        nudMemory.ForeColor = ForeColor
        nudStorage.BackColor = BackColor
        nudStorage.ForeColor = ForeColor
        tbSysOSName.BackColor = BackColor
        tbSysOSName.ForeColor = ForeColor
        tbSysOSSku.BackColor = BackColor
        tbSysOSSku.ForeColor = ForeColor
        tbSysOSVer.BackColor = BackColor
        tbSysOSVer.ForeColor = ForeColor

        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)

        cmsAutoDetectOptions.Renderer = GetProfessionalRenderer()
        cmsAutoDetectOptions.ForeColor = ForeColor
    End Sub

    Private Sub btnAutoDetect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAutoDetect.Click
        cmsAutoDetectOptions.Show(sender, New Point(0, btnAutoDetect.Height))
    End Sub

    Private Sub SetSystemInformation(Optional ByVal SectionNumber As Integer = -1)
        Dim ProcessorDetails As Dictionary(Of String, Object) = Nothing, _
            StorageDetails As Dictionary(Of String, Object) = Nothing, _
            OperatingSystemDetails As Dictionary(Of String, Object) = Nothing

        Cursor = Cursors.WaitCursor

        If SectionNumber = -1 Or SectionNumber = 0 Then
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
        End If

        If SectionNumber = -1 Or SectionNumber = 1 Then
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
        End If

        Cursor = Cursors.Arrow
    End Sub

    Private Sub AutoDetectCurrentSectionTSMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoDetectCurrentSectionTSMI.Click
        SetSystemInformation(tcCompInfo.SelectedIndex)
    End Sub

    Private Sub AutoDetectAllSectionsTSMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoDetectAllSectionsTSMI.Click
        SetSystemInformation()
    End Sub
End Class
