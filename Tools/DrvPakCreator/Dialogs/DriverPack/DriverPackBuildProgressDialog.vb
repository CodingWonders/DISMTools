Imports System.Windows.Forms
Imports System.IO
Imports DrvPakCreator.Classes
Imports DrvPakCreator.Classes.ProgressReporting
Imports DrvPakCreator.Helpers.FileIO

Public Class DriverPackBuildProgressDialog

    Public SourceDriverFilesFolder As String
    Public DestinationPackFile As String

    Private CurrentTaskStatus As TaskReporter

    Private zipper As New ZipCompressor()

    Private tempDir As String

    Private StartTime As Date

    Private Sub CopyDriverPackFilesToTemporaryFolder()
        Try
            If Not Directory.Exists(tempDir) Then
                Directory.CreateDirectory(tempDir)
            End If

            PathUtilities.CopyRecursive(SourceDriverFilesFolder, tempDir, True, AddressOf ReportFileCopy)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ReportFileCopy(ByVal OperationStatus As TaskReporter)
        CurrentTaskStatus = OperationStatus
        bwFileCopy.ReportProgress(OperationStatus.Percentage)
    End Sub

    Private Sub ZipDriverPack()
        Try
            Dim zipSource As String = tempDir, _
                destinationPackLocation As String = DestinationPackFile

            Dim filesToCompress() As String = Directory.GetFiles(zipSource, "*.*", SearchOption.AllDirectories)

            zipper.CreateZip(filesToCompress, zipSource, destinationPackLocation, IIf(options.UseLZMA, ZipCompressionMethod.Lzma, ZipCompressionMethod.Deflate))
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub ReportFilePack(ByVal OperationStatus As TaskReporter)
        CurrentTaskStatus = OperationStatus
        bwPakBuild.ReportProgress(OperationStatus.Percentage)
    End Sub

    Private Sub DriverPackBuildProgressDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If File.Exists(DestinationPackFile) Then
            Try
                File.Delete(DestinationPackFile)
            Catch ex As Exception

            End Try
        End If

        WindowHelper.DisableCloseCapability(Handle)

        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor

        ProgressTask1.SetColors()
        ProgressTask2.SetColors()

        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)

        ProgressTask1.TaskStatus = ProgressTaskStatus.InProgress
        ProgressTask2.TaskStatus = ProgressTaskStatus.NotStarted
        tempDir = Path.Combine(Path.GetTempPath(), String.Format("DRVPAKTMP_{0}", New Random().Next()))

        StartTime = Now
        tmrTimeTaken.Start()

        AddHandler zipper.ProgressChanged, AddressOf OnZipProgress

        bwFileCopy.RunWorkerAsync()
    End Sub

    Private Sub bwPakBuild_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwPakBuild.DoWork
        ZipDriverPack()
    End Sub

    Private Sub bwPakBuild_ProgressChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwPakBuild.ProgressChanged
        lblBuildStatus.Text = CurrentTaskStatus.Message
        ProgressTask2.TaskPercentage = e.ProgressPercentage
    End Sub

    Private Sub bwPakBuild_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwPakBuild.RunWorkerCompleted
        ProgressTask2.TaskStatus = IIf(e.Error Is Nothing, ProgressTaskStatus.Succeeded, ProgressTaskStatus.Failed)
        ProgressTask2.TaskPercentage = -1
        tmrTimeTaken.Stop()
        Refresh()
        If e.Error IsNot Nothing Then
            MessageBox.Show("The Driver Pack cannot be built.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        Try
            lblBuildStatus.Text = "Deleting temporary files..."
            Refresh()
            ' PAKDAT remains read-only and causes the directory to not get deleted
            Dim PakDatPath As String = Path.Combine(tempDir, "PAKDAT")
            File.SetAttributes(PakDatPath, (File.GetAttributes(PakDatPath) And Not FileAttributes.ReadOnly))
            Directory.Delete(tempDir, True)
        Catch ex As Exception

        End Try
        lblBuildStatus.Text = "Done"
        Refresh()
        Threading.Thread.Sleep(500)

        If e.Error IsNot Nothing Then
            DialogResult = Windows.Forms.DialogResult.Cancel
        Else
            DialogResult = Windows.Forms.DialogResult.OK
        End If
        Close()
    End Sub

    Private Sub OnZipProgress(ByVal sender As Object, ByVal e As ZipProgressEventArgs)
        Dim msg As String = String.Format("({0}/{1}) Packing file {2}", e.FilesProcessed, e.TotalFiles, Path.GetFileName(e.CurrentFile.Replace("/", "\")))
        ReportFilePack(New TaskReporter(msg, e.Percentage))
    End Sub

    Private Sub bwFileCopy_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwFileCopy.DoWork
        CopyDriverPackFilesToTemporaryFolder()
    End Sub

    Private Sub bwFileCopy_ProgressChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwFileCopy.ProgressChanged
        lblBuildStatus.Text = CurrentTaskStatus.Message
        ProgressTask1.TaskPercentage = e.ProgressPercentage
    End Sub

    Private Sub bwFileCopy_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwFileCopy.RunWorkerCompleted
        ProgressTask1.TaskStatus = IIf(e.Error Is Nothing, ProgressTaskStatus.Succeeded, ProgressTaskStatus.Failed)
        ProgressTask1.TaskPercentage = -1
        Refresh()
        If e.Error IsNot Nothing Then
            MessageBox.Show("The Driver Pack cannot be built because files could not be copied.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
            Exit Sub
        End If

        ProgressTask2.TaskStatus = ProgressTaskStatus.InProgress
        ProgressTask2.TaskPercentage = 0

        bwPakBuild.RunWorkerAsync()
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
