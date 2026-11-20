Imports System.Windows.Forms
Imports System.IO
Imports DrvPakCreator.Classes.ProgressReporting
Imports DrvPakCreator.Helpers.FileIO

Public Class DriverPackImportProgressDialog

    Public SourceDriverPackLocation As String
    Public DestinationProjectFilesPath As String

    Private CurrentTaskStatus As TaskReporter

    Private unzipper As New ZipExtractor()

    Private StartTime As Date

    Private Sub UnzipDriverPack()
        Try
            Dim zipSource As String = SourceDriverPackLocation, _
                destinationProjectFilesLocation As String = DestinationProjectFilesPath

            unzipper.ExtractAll(zipSource, destinationProjectFilesLocation)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ReportFileUnpack(ByVal OperationStatus As TaskReporter)
        CurrentTaskStatus = OperationStatus
        bwPakUnpack.ReportProgress(OperationStatus.Percentage)
    End Sub

    Private Sub DriverPackBuildProgressDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        WindowHelper.DisableCloseCapability(Handle)

        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor

        ProgressTask1.SetColors()

        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)

        ProgressTask1.TaskStatus = ProgressTaskStatus.InProgress

        StartTime = Now
        tmrTimeTaken.Start()

        AddHandler unzipper.ProgressChanged, AddressOf OnZipProgress
        bwPakUnpack.RunWorkerAsync()
    End Sub

    Private Sub bwPakUnpack_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwPakUnpack.DoWork
        UnzipDriverPack()
    End Sub

    Private Sub bwPakUnpack_ProgressChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwPakUnpack.ProgressChanged
        lblBuildStatus.Text = CurrentTaskStatus.Message
        ProgressTask1.TaskPercentage = e.ProgressPercentage
    End Sub

    Private Sub bwPakUnpack_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwPakUnpack.RunWorkerCompleted
        ProgressTask1.TaskStatus = IIf(e.Error Is Nothing, ProgressTaskStatus.Succeeded, ProgressTaskStatus.Failed)
        ProgressTask1.TaskPercentage = -1
        tmrTimeTaken.Stop()
        Refresh()
        If e.Error IsNot Nothing Then
            MessageBox.Show("The Driver Pack cannot be imported.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        lblBuildStatus.Text = "Done"
        Refresh()
        Threading.Thread.Sleep(500)
        Close()
    End Sub

    Private Sub OnZipProgress(ByVal sender As Object, ByVal e As ZipProgressEventArgs)
        Dim msg As String = String.Format("({0}/{1}) Unpacking file {2}", e.FilesProcessed, e.TotalFiles, Path.GetFileName(e.CurrentFile.Replace("/", "\")))
        ReportFileUnpack(New TaskReporter(msg, e.Percentage))
    End Sub

    Private Sub tmrTimeTaken_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrTimeTaken.Tick
        Dim duration As TimeSpan = Now - StartTime
        Dim days As Integer = duration.Days, _
            hours As Integer = duration.Hours, _
            minutes As Integer = duration.Minutes, _
            seconds As Integer = duration.Seconds
        lblTimeTaken.Text = String.Format("{0}.{1:00}:{2:00}:{3:00}", days, hours, minutes, seconds)
    End Sub
End Class
