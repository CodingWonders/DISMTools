Imports Microsoft.Win32
Imports System.IO
Imports System.Text
Imports DrvPakCreator.Classes.SetupAPI
Imports DrvPakCreator.Helpers.FileIO

Namespace Helpers

    Public Class SystemDriverHelper

        ' System Driver Dictionary Keys:
        ' -- mainFile
        ' -- deviceName
        ' -- manufacturer

        Public Shared Function EnumerateInstalledThirdPartyDrivers() As List(Of Dictionary(Of String, String))
            ' This varies depending on the OS.
            Select Case Environment.OSVersion.Version.Major
                Case 5
                    Select Case Environment.OSVersion.Version.Minor
                        Case 0 : Return EnumerateDriversFromInfDirectory()                  ' Windows 2000. Manual enumeration of INF files in \WINNT\INF
                        Case 1, 2 : Return EnumerateDriversFromWmiSignedDrivers()           ' Windows XP/Server 2003. Win32_PnpSignedDriver WMI class
                    End Select
                Case 6
                    Select Case Environment.OSVersion.Version.Minor
                        Case 0, 1 : Return EnumerateDriversFromWmiSignedDrivers()           ' Windows Vista/Server 2008/7/2008 R2
                        Case 2, 3 : Return GetDriverPathsFromDriverDatabase()               ' Windows 8/8.1/Server 2012/2012 R2
                    End Select
                Case 10 : Return GetDriverPathsFromDriverDatabase()                         ' Windows 10/11/Server 2016/2019/2022/2025
            End Select
            Return New List(Of Dictionary(Of String, String))
        End Function

        Private Shared ReadOnly InfPath As String = Path.Combine(Environment.GetEnvironmentVariable("SYSTEMROOT"), "inf")

        Private Shared Function EnumerateDriversFromInfDirectory() As List(Of Dictionary(Of String, String))
            Dim driverPaths As New List(Of Dictionary(Of String, String))

            Try
                Dim oemInfFiles() As String = Directory.GetFiles(InfPath, "oem*.inf", SearchOption.TopDirectoryOnly)
                ' All the obtained INF files in the retrieval will be added. We don't really need to check if they are
                ' valid INF files as Windows has already sorted this out for us.
                For Each oemInfFile As String In oemInfFiles
                    Dim driverDictionary As New Dictionary(Of String, String)
                    driverDictionary.Add("mainFile", oemInfFile)
                    driverDictionary.Add("deviceName", "")
                    driverDictionary.Add("manufacturer", "")
                    driverPaths.Add(driverDictionary)
                Next
            Catch ex As Exception
                ' ignore
            End Try

            Return driverPaths
        End Function

        Private Shared Function EnumerateDriversFromWmiSignedDrivers() As List(Of Dictionary(Of String, String))
            Dim driverPaths As New List(Of Dictionary(Of String, String))

            Try
                Dim PnpDriverMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT InfName, DeviceName, Manufacturer FROM Win32_PnpSignedDriver WHERE InfName LIKE {0}oem%.inf{0}", Quote))
                Dim PnpDriverEnumerator As ManagementObjectCollection.ManagementObjectEnumerator = PnpDriverMOC.GetEnumerator()
                While PnpDriverEnumerator.MoveNext()
                    Dim driverDictionary As New Dictionary(Of String, String)
                    driverDictionary.Add("mainFile", WMIHelper.GetObjectValue(PnpDriverEnumerator.Current, "InfName"))
                    driverDictionary.Add("deviceName", WMIHelper.GetObjectValue(PnpDriverEnumerator.Current, "DeviceName"))
                    driverDictionary.Add("manufacturer", WMIHelper.GetObjectValue(PnpDriverEnumerator.Current, "Manufacturer"))
                    driverPaths.Add(driverDictionary)
                End While

                ' If we are on Vista or 7 we can query the original path.
                Dim supportedOriginalPathVersions As New List(Of Integer)
                supportedOriginalPathVersions.AddRange(New Integer() {0, 1})
                If Environment.OSVersion.Version.Major = 6 And supportedOriginalPathVersions.Contains(Environment.OSVersion.Version.Minor) Then
                    QueryOriginalFileNames(driverPaths)
                    For i As Integer = driverPaths.Count To 0 Step -1
                        If driverPaths(i)("mainFile") = "" Then driverPaths.RemoveAt(i)
                    Next
                End If
            Catch ex As Exception
                ' ignore
            End Try

            Return driverPaths
        End Function

        Private Shared Sub QueryOriginalFileNames(ByRef DriverPaths As List(Of Dictionary(Of String, String)))
            Dim OemPathCopy As List(Of Dictionary(Of String, String)) = New List(Of Dictionary(Of String, String))(DriverPaths)

            Dim index As Integer = 0
            For Each OemPath As Dictionary(Of String, String) In OemPathCopy
                Dim originalFilePath As String = "", _
                    buffer As New StringBuilder(520), _
                    requiredSize As Integer = 0
                If NativeMethods.SetupGetInfDriverStoreLocation(OemPath("mainFile"), IntPtr.Zero, Nothing, buffer, buffer.Capacity, requiredSize) Then
                    DriverPaths(index)("mainFile") = buffer.ToString()
                Else
                    DriverPaths(index)("mainFile") = ""
                End If
                index += 1
            Next
        End Sub

        Private Shared ReadOnly WinVistaDriverPath As String = Path.Combine(Environment.GetEnvironmentVariable("SYSTEMROOT"), "system32\DriverStore\FileRepository")

        Private Class DriverEntry
            Public InfName As String
            Public DeviceName As String
            Public Manufacturer As String

            Public Sub New(ByVal infName As String, ByVal deviceName As String, ByVal manufacturer As String)
                Me.InfName = infName
                Me.DeviceName = deviceName
                Me.Manufacturer = manufacturer
            End Sub
        End Class

        Private Shared Function CompareByInfName(ByVal a As DriverEntry, ByVal b As DriverEntry) As Integer
            Dim result As Integer = String.Compare(a.InfName, b.InfName, StringComparison.OrdinalIgnoreCase)
            ' The same INF can be used by several devices, so break ties by device name
            If result = 0 Then
                result = String.Compare(a.DeviceName, b.DeviceName, StringComparison.OrdinalIgnoreCase)
            End If
            Return result
        End Function

        ''' <summary>
        ''' Enumerates driver files from driver databases on Windows 8 and newer Windows versions.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function GetDriverPathsFromDriverDatabase() As List(Of Dictionary(Of String, String))
            Dim DriverFilesRk As RegistryKey = Nothing

            Dim driverPaths As New List(Of Dictionary(Of String, String))

            Try
                DriverFilesRk = Registry.LocalMachine.OpenSubKey("SYSTEM\DriverDatabase\DriverInfFiles", False)
                Dim DatabaseDriverFiles() As String = DriverFilesRk.GetSubKeyNames()
                Dim PnpDriverMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT InfName, DeviceName, Manufacturer FROM Win32_PnpSignedDriver WHERE InfName LIKE {0}oem%.inf{0}", Quote)), _
                    DriverEntries As New List(Of DriverEntry)
                For Each PnpDriverMO As ManagementObject In PnpDriverMOC
                    DriverEntries.Add(New DriverEntry(PnpDriverMO("InfName"), PnpDriverMO("DeviceName"), PnpDriverMO("Manufacturer")))
                    PnpDriverMO.Dispose()
                Next
                DriverEntries.Sort(AddressOf CompareByInfName)
                For Each DatabaseDriverFile As String In DatabaseDriverFiles
                    If Not DatabaseDriverFile.StartsWith("oem", StringComparison.OrdinalIgnoreCase) Then Continue For

                    Dim driverDictionary As New Dictionary(Of String, String)

                    ' We use WMI to get the name of the device. If there isn't one, then setupapi is used
                    ' and grabs the first device name. Though do that check later.
                    driverDictionary.Add("mainFile", "")
                    driverDictionary.Add("deviceName", "")
                    driverDictionary.Add("manufacturer", "")
                    For Each entry As DriverEntry In DriverEntries
                        If entry.InfName.Equals(DatabaseDriverFile, StringComparison.OrdinalIgnoreCase) Then
                            driverDictionary("deviceName") = entry.DeviceName
                            driverDictionary("manufacturer") = entry.Manufacturer
                            Exit For
                        End If
                    Next

                    ' In each of the OEM keys, a set of driver configurations is defined. The active configuration
                    ' indicates the one that is being used by the system at the current time. Based on that, we look
                    ' at the driver store for a folder named like that and we return it back.
                    Try
                        Dim DatabaseDriverFileRk As RegistryKey = DriverFilesRk.OpenSubKey(DatabaseDriverFile, False)
                        Dim activeValue As String = DatabaseDriverFileRk.GetValue("Active", "")
                        DatabaseDriverFileRk.Close()

                        If activeValue = "" Then
                            Continue For
                        Else
                            Dim mainDriverFilePath As String = Path.Combine(WinVistaDriverPath, activeValue)
                            ' Add the file name. In pretty much ALL cases, the folder the driver file is in is named
                            ' after it.
                            Dim folderName As String = PathUtilities.GetDirectoryName(mainDriverFilePath)
                            Dim infExtIdx As Integer = folderName.ToLower().IndexOf(".inf_")
                            Dim driverFile As String = String.Format("{0}.inf", folderName.Remove(infExtIdx, folderName.Length - infExtIdx))

                            mainDriverFilePath = Path.Combine(mainDriverFilePath, driverFile)
                            driverDictionary("mainFile") = mainDriverFilePath

                            If driverDictionary("deviceName") = "" AndAlso File.Exists(mainDriverFilePath) Then
                                Dim className As String = "", _
                                    classGuid As Guid = Guid.Empty
                                If InfParser.GetInfClassInfo(mainDriverFilePath, className, classGuid) Then
                                    Dim infEntries As List(Of InfDriverEntry) = InfParser.ParseInfFile(mainDriverFilePath)
                                    If infEntries.Count > 0 Then
                                        driverDictionary("deviceName") = infEntries(0).Description
                                        driverDictionary("manufacturer") = infEntries(0).Manufacturer
                                    End If
                                End If
                            End If

                            ' If we still didn't get the name we'll fall back to the file name
                            If driverDictionary("deviceName") = "" Then driverDictionary("deviceName") = Path.GetFileName(driverDictionary("mainFile"))
                            If String.IsNullOrEmpty(driverDictionary("manufacturer")) Then driverDictionary("manufacturer") = ""
                        End If
                    Catch ex As Exception
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try

                    driverPaths.Add(driverDictionary)
                Next
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                If DriverFilesRk IsNot Nothing Then DriverFilesRk.Close()
            End Try

            Return driverPaths
        End Function

    End Class

End Namespace