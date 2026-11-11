Imports System.Windows.Forms
Imports DrvPakCreator.Classes
Imports System.IO

Public Class DriverReindexProgressDialog

    Public StartingDriverId As Integer
    Public DriverFilesRoot As String

    Public SourceDriverMetadata As List(Of Driver)
    Public UpdatedDriverMetadata As List(Of Driver)

    Private Sub ReindexDriverMetadata()
        For i As Integer = StartingDriverId To SourceDriverMetadata.Count - 1
            UpdatedDriverMetadata(i).Id = i + 1
        Next
    End Sub

    Private Sub MoveDriverFolders()
        For i As Integer = StartingDriverId To SourceDriverMetadata.Count - 1
            Dim sourceDriverFolderName As String = String.Format("DRV_{0}", SourceDriverMetadata(i).Id.ToString().PadLeft(10, "0"c)), _
                destinationDriverFolderName As String = String.Format("DRV_{0}", UpdatedDriverMetadata(i).Id.ToString().PadLeft(10, "0"c))

            Dim sourceDriverFolderPath As String = Path.Combine(DriverFilesRoot, sourceDriverFolderName), _
                destinationDriverFolderPath As String = Path.Combine(DriverFilesRoot, destinationDriverFolderName)

            Directory.Move(sourceDriverFolderPath, destinationDriverFolderPath)
        Next
    End Sub

    Private Sub DriverReindexProgressDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        WindowHelper.DisableCloseCapability(Handle)

        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor

        ProgressTask1.SetColors()
        ProgressTask2.SetColors()

        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)

        Visible = True

        UpdatedDriverMetadata = New List(Of Driver)
        ' deep copy items; no shallow copies allowed
        For Each driverInSourceMetadata As Driver In SourceDriverMetadata
            UpdatedDriverMetadata.Add(driverInSourceMetadata.Clone())
        Next

        ' begin reindexing in metadata
        ProgressTask1.TaskStatus = ProgressTaskStatus.InProgress
        Refresh()
        ReindexDriverMetadata()
        ProgressTask1.TaskStatus = ProgressTaskStatus.Succeeded
        ProgressTask2.TaskStatus = ProgressTaskStatus.InProgress
        Refresh()
        MoveDriverFolders()
        ProgressTask2.TaskStatus = ProgressTaskStatus.Succeeded
        Refresh()

        Threading.Thread.Sleep(500)
        Close()
    End Sub
End Class
