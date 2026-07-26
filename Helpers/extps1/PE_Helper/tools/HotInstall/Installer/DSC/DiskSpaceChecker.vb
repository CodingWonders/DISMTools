Imports System.IO
Imports Microsoft.Dism
Imports Microsoft.VisualBasic.ControlChars
Imports System.Management

Public Class DiskSpaceChecker

    Private Function DSCText(Key As String, ParamArray values() As Object) As String
        Dim template As String = GetValueFromLanguageData("DiskSpaceChecker." & Key)
        If values IsNot Nothing AndAlso values.Length > 0 Then
            Return String.Format(template, values)
        End If
        Return template
    End Function

    Dim progressMessage As String = ""

    Dim reportContents As String = ""

    Dim isTestApplicable As Boolean
    Dim sourcePath As String = ""

#Region "WQL Drive Information Functions/Methods"

    Sub ListObtainedDisks(DriveObjects As ManagementObjectCollection)
        If DriveObjects Is Nothing Then
            Throw New Exception(DSCText("ReportNullDriveCollection"))
        End If
        DynaLog.LogMessage("Count of obtained disks: " & DriveObjects.Count)
        reportContents &= DSCText("ReportLocalDiskCount", DriveObjects.Count) & CrLf & CrLf
        If DriveObjects.Count > 0 Then
            DynaLog.LogMessage("Saving obtained disks to report...")
            For Each DriveObject As ManagementObject In DriveObjects
                reportContents &= DSCText("ReportDiskInfo", GetObjectValue(DriveObject, "DiskIndex"),
                                              GetObjectValue(DriveObject, "Index") + 1,
                                              GetObjectValue(DriveObject, "Size"),
                                              Converters.BytesToReadableSize(GetObjectValue(DriveObject, "Size")),
                                              If(GetObjectValue(DriveObject, "BootPartition"), DSCText("ReportYes"), DSCText("ReportNo")),
                                              If(GetObjectValue(DriveObject, "PrimaryPartition"), DSCText("ReportYes"), DSCText("ReportNo"))) & CrLf & CrLf
            Next
        End If
    End Sub

    Sub ListSpaceComparison(ImageNames As List(Of String), ImageSizes As List(Of Long), Drives As ManagementObjectCollection)
        If ImageNames.Count = 0 Then
            Throw New Exception(DSCText("ReportNoNames"))
        End If
        If ImageSizes.Count = 0 Then
            Throw New Exception(DSCText("ReportNoSizes"))
        End If
        If Drives.Count = 0 Then
            Throw New Exception(DSCText("ReportNoFixedDrives"))
        End If

        DynaLog.LogMessage("Comparing spaces of images and drives...")
        DynaLog.LogMessage("- Count of images: " & ImageNames.Count)
        DynaLog.LogMessage("- Count of drives: " & Drives.Count)

        reportContents &= DSCText("ReportSizeComparison") & CrLf & CrLf

        DynaLog.LogMessage("Comparing spaces for drives...")
        For Each Drive As ManagementObject In Drives
            reportContents &= DSCText("ReportDiskWithVolumeLabel", Quote & GetObjectValue(Drive, "VolumeName") & Quote, GetObjectValue(Drive, "DeviceID")) & CrLf
            For Each ImageSize In ImageSizes
                If GetObjectValue(Drive, "Size") > ImageSize Then
                    DynaLog.LogMessage("This image can be installed here.")
                    reportContents &= DSCText("ReportCanInstall", Quote & ImageNames(ImageSizes.IndexOf(ImageSize)) & Quote, ImageSizes.IndexOf(ImageSize) + 1) & CrLf
                Else
                    DynaLog.LogMessage("This image cannot be installed here.")
                    reportContents &= DSCText("ReportCannotInstall", Quote & ImageNames(ImageSizes.IndexOf(ImageSize)) & Quote, ImageSizes.IndexOf(ImageSize) + 1) & CrLf
                End If
            Next
        Next

        reportContents &= CrLf
    End Sub

#End Region

    Function GetDirectorySize(DirectoryName As String, ExcludedFile As String) As Long
        DynaLog.LogMessage("Getting the size of provided directory...")
        DynaLog.LogMessage("- Directory: " & DirectoryName)
        DynaLog.LogMessage("- File to exclude: " & ExcludedFile)
        Dim DirectorySize As Long = 0
        If Directory.Exists(DirectoryName) Then
            For Each FileInDir In Directory.GetFiles(DirectoryName, "*", SearchOption.AllDirectories)
                If Path.GetFileName(FileInDir).Equals(ExcludedFile, StringComparison.OrdinalIgnoreCase) Then Continue For
                DirectorySize += New FileInfo(FileInDir).Length
            Next
        End If
        DynaLog.LogMessage("Reported size: " & DirectorySize & " bytes")
        Return DirectorySize
    End Function

    Sub InitializeReport()
        reportContents = DSCText("ReportTitle") & CrLf &
            "==========================" & CrLf &
            DSCText("ReportGeneratedBy", My.Application.Info.Version.ToString()) & CrLf & CrLf
    End Sub

    Sub ListFreeSpace(FreeSpace As Long, SpaceToCompare As Long, Optional SystemDriveMO As ManagementObject = Nothing)
        DynaLog.LogMessage("Listing free space in drive...")
        DynaLog.LogMessage("- Free space: " & FreeSpace)
        DynaLog.LogMessage("- Referenced space: " & SpaceToCompare)
        If SystemDriveMO IsNot Nothing Then
            DynaLog.LogMessage("System drive management object is something. We select the drive")
            reportContents &= DSCText("ReportSystemDrive", GetObjectValue(SystemDriveMO, "DeviceID")) & CrLf & CrLf
        End If

        DynaLog.LogMessage("Saving information to report...")
        reportContents &= DSCText("ReportCopyPlan") & CrLf &
                          DSCText("ReportTotalImageSize", SpaceToCompare, Converters.BytesToReadableSize(SpaceToCompare)) & CrLf &
                          DSCText("ReportFreeSpace", FreeSpace, Converters.BytesToReadableSize(FreeSpace)) & CrLf & CrLf

        reportContents &= Converters.BytesToReadableSize(FreeSpace) & " > " & Converters.BytesToReadableSize(SpaceToCompare) & " ? " & If(FreeSpace > SpaceToCompare, DSCText("ReportYes"), DSCText("ReportNo")) & CrLf &
                          Converters.BytesToReadableSize(FreeSpace) & " > " & Converters.BytesToReadableSize(SpaceToCompare * 2) & " ? " & If(FreeSpace > SpaceToCompare * 2, DSCText("ReportYes"), DSCText("ReportNo")) & CrLf & CrLf

        If FreeSpace < (SpaceToCompare * 2) Then
            reportContents &= DSCText("ReportMayNotHaveEnoughSpace") & CrLf & CrLf
        ElseIf FreeSpace < SpaceToCompare Then
            reportContents &= DSCText("ReportNotEnoughSpace") & CrLf & CrLf
        Else
            reportContents &= DSCText("ReportPlentyOfSpace") & CrLf & CrLf
        End If
    End Sub

    Sub SaveReport(Location As String)
        DynaLog.LogMessage("Saving DSC report...")
        DynaLog.LogMessage("- Destination: " & Location)
        Try
            File.WriteAllText(Location, reportContents, System.Text.Encoding.UTF8)
        Catch ex As Exception

        End Try
    End Sub

    Sub GenerateDSCReport()
        DynaLog.LogMessage("Generating Disk Space Checker report...")
        DynaLog.LogMessage("Getting System Drives...")
        Dim SystemDrives As ManagementObjectCollection = GetResultsFromManagementQuery("SELECT * FROM Win32_LogicalDisk WHERE DriveType = 3")        ' DriveType = 3 --> Local Disk
        If SystemDrives IsNot Nothing AndAlso SystemDrives.Count > 0 Then
            DynaLog.LogMessage("System drives were obtained. Listing...")
            ' List the disks with a management query
            ListObtainedDisks(GetResultsFromManagementQuery("SELECT * FROM Win32_DiskPartition"))

            ' Get System Drive
            DynaLog.LogMessage("Getting System Boot Drive...")
            Dim SystemBootDrive As ManagementObjectCollection = GetResultsFromManagementQuery("SELECT * FROM Win32_LogicalDisk WHERE DriveType = 3 AND DeviceID LIKE " & Quote & Environment.GetEnvironmentVariable("HOMEDRIVE") & Quote)
            If SystemBootDrive IsNot Nothing Then
                DynaLog.LogMessage("Getting image file sizes and free spaces...")
                ' We have grabbed the system boot drive
                progressMessage = GetValueFromLanguageData("DiskSpaceChecker.DSC_GetSizeOfImageFiles")
                BackgroundWorker1.ReportProgress(10)
                Dim FolderSize As Long = GetDirectorySize(sourcePath, "")
                ' Get the free space of the system boot drive, since we'll copy the files there
                Dim FreeSpaceOnSystemDrive As Long = GetObjectValue(SystemBootDrive(0), "FreeSpace")
                ListFreeSpace(FreeSpaceOnSystemDrive, FolderSize, SystemBootDrive(0))
                DynaLog.LogMessage("Free space on system drive: " & FreeSpaceOnSystemDrive & " bytes")
                DynaLog.LogMessage("Folder Size: " & FolderSize & " bytes")
                If FreeSpaceOnSystemDrive < FolderSize Then
                    DynaLog.LogMessage("Free space is lower than folder size.")
                    Throw New Exception(DSCText("ReportNotEnoughSystemDriveSpace"))
                End If
                ' Get information about the installation image and compare the expanded sizes of all indexes with the total space of all fixed drives
                progressMessage = GetValueFromLanguageData("DiskSpaceChecker.DSC_GetImageFileInfo")
                BackgroundWorker1.ReportProgress(40)
                DynaLog.LogMessage("Getting image information...")
                Dim imgInfoCollection As DismImageInfoCollection = GetImageInformation(Path.Combine(sourcePath, "sources", "install.wim"))
                If imgInfoCollection IsNot Nothing Then
                    DynaLog.LogMessage("Getting image names and sizes...")
                    progressMessage = GetValueFromLanguageData("DiskSpaceChecker.DSC_GetImageNamesAndSizes")
                    BackgroundWorker1.ReportProgress(60)
                    ' Grab the image names and expanded sizes
                    Dim ImageNames As New List(Of String)
                    Dim ImageSizes As New List(Of Long)
                    For Each imgInfo As DismImageInfo In imgInfoCollection
                        ImageNames.Add(imgInfo.ImageName)
                        ImageSizes.Add(imgInfo.ImageSize)
                    Next
                    progressMessage = GetValueFromLanguageData("DiskSpaceChecker.DSC_CompareSizes")
                    BackgroundWorker1.ReportProgress(80)
                    DynaLog.LogMessage("Comparing spaces...")
                    ListSpaceComparison(ImageNames, ImageSizes, SystemDrives)
                End If
            End If
        Else
            Throw New Exception(DSCText("ReportFixedDrivesNotDetected"))
        End If
    End Sub

    Function GetImageInformation(ImagePath As String) As DismImageInfoCollection
        DynaLog.LogMessage("Getting Windows image information...")
        DynaLog.LogMessage("- Image Path: " & ImagePath)
        Dim imgInfoCollection As DismImageInfoCollection = Nothing
        Try
            If ImagePath <> "" AndAlso File.Exists(ImagePath) Then
                DynaLog.LogMessage("Preparing to initialize API and get image info")
                DismApi.Initialize(DismLogLevel.LogErrors)
                imgInfoCollection = DismApi.GetImageInfo(Path.Combine(sourcePath, "sources", "install.wim"))
            End If
        Catch ex As Exception
            Throw ex
        Finally
            DynaLog.LogMessage("Shutting down API...")
            Try
                DismApi.Shutdown()
            Catch ex As Exception

            End Try
        End Try
        Return imgInfoCollection
    End Function

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        InitializeReport()
        progressMessage = GetValueFromLanguageData("DiskSpaceChecker.DSC_GetSysDrives")
        BackgroundWorker1.ReportProgress(5)
        GenerateDSCReport()     ' Let's make my database teacher happy!
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        Label2.Text = progressMessage
        ProgressBar1.Value = e.ProgressPercentage
        Application.DoEvents()
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Dim success As Boolean = True
        If e.Error IsNot Nothing Then
            If e.Error.Message.StartsWith(DSCText("ReportWarningOnlyPrefix"), StringComparison.OrdinalIgnoreCase) Then
                MsgBox(e.Error.Message, vbOKOnly + vbExclamation, Text)
            Else
                success = False
                reportContents &= CrLf & CrLf & e.Error.ToString()
                Visible = False
            End If
        End If
        DialogResult = If(success, Windows.Forms.DialogResult.OK, Windows.Forms.DialogResult.Cancel)
        SaveReport(Path.Combine(Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)),
                                "DscReport.txt"))
        Close()
    End Sub

    Private Sub DiskSpaceChecker_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        isTestApplicable = MainForm.TestMode Or MainForm.TestBCD
        If isTestApplicable Then
            sourcePath = Application.StartupPath
        Else
            sourcePath = Path.GetPathRoot(Application.StartupPath)
        End If
        Dim IsDarkMode As Boolean = Utilities.IsSystemInDarkMode
        If Utilities.IsWindowsVersionOrGreater(10, 0, 18362) Then Utilities.EnableDarkTitleBar(Handle, IsDarkMode)
        BackColor = If(IsDarkMode, Color.FromArgb(12, 12, 12), Color.FromArgb(246, 246, 249))
        ForeColor = If(IsDarkMode, Color.White, Color.Black)
        Text = GetValueFromLanguageData("DiskSpaceChecker.WndTitle")
        Label1.Text = GetValueFromLanguageData("DiskSpaceChecker.WndDesc")
        Label2.Text = GetValueFromLanguageData("DiskSpaceChecker.DSC_GenericProgress")
        BackgroundWorker1.RunWorkerAsync()
    End Sub
End Class