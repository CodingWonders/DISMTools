Imports System.Windows.Forms
Imports DrvPakCreator.Classes
Imports DrvPakCreator.Classes.ProgressReporting
Imports DrvPakCreator.Helpers.FileIO
Imports System.IO

Public Class DriverAdditionProgressDialog

    Public DriverMetadata As DriverPack
    Public DriverPackProjectFilesRoot As String

    ' Addition Wizard Fields
    Public DriverSource As String
    Public DriverMainFile As String
    Public DriverName As String
    Public DriverManufacturer As String

    Public NewDriver As Driver

    Private DriverFilesRoot As String

    Private CurrentTaskStatus As TaskReporter

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CreateDriverFolder()
        Try
            Directory.CreateDirectory(DriverFilesRoot)
            ProgressTask1.TaskStatus = ProgressTaskStatus.Succeeded
        Catch ex As Exception
            ProgressTask1.TaskStatus = ProgressTaskStatus.Failed
        End Try
    End Sub

    Private Sub CopyDriverFiles()
        PathUtilities.CopyRecursive(DriverSource, DriverFilesRoot, True, AddressOf ReportFileCopy)
    End Sub

    Private Sub DriverAdditionProgressDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        WindowHelper.DisableCloseCapability(Handle)

        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor

        ProgressTask1.SetColors()
        ProgressTask2.SetColors()

        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)

        ' Create the driver pack folder
        ProgressTask1.TaskStatus = ProgressTaskStatus.InProgress
        Refresh()

        NewDriver = New Driver()
        Dim newDriverId As Integer = DriverMetadata.DriverFiles.Drivers.Count + 1
        NewDriver.Id = newDriverId
        NewDriver.MainFile = DriverMainFile
        NewDriver.Name = DriverName
        NewDriver.Manufacturer = DriverManufacturer
        DriverFilesRoot = Path.Combine(DriverPackProjectFilesRoot, String.Format("DRV_{0}", newDriverId.ToString().PadLeft(10, "0"c)))
        CreateDriverFolder()
        Refresh()

        ' Perform recursive copy
        ProgressTask2.TaskStatus = ProgressTaskStatus.InProgress
        bwFileCopy.RunWorkerAsync()
    End Sub

    Private Sub ReportFileCopy(ByVal OperationStatus As TaskReporter)
        CurrentTaskStatus = OperationStatus
        bwFileCopy.ReportProgress(OperationStatus.Percentage)
    End Sub

    Private Sub bwFileCopy_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwFileCopy.DoWork
        CopyDriverFiles()
    End Sub

    Private Sub bwFileCopy_ProgressChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwFileCopy.ProgressChanged
        lblAdditionStatus.Text = CurrentTaskStatus.Message
        ProgressTask2.TaskPercentage = e.ProgressPercentage
    End Sub

    Private Sub bwFileCopy_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwFileCopy.RunWorkerCompleted
        ProgressTask2.TaskStatus = IIf(e.Error Is Nothing, ProgressTaskStatus.Succeeded, ProgressTaskStatus.Failed)
        Refresh()
        Threading.Thread.Sleep(500)
        Close()
    End Sub
End Class
