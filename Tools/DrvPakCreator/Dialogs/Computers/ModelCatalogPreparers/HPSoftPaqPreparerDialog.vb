Imports System.Windows.Forms
Imports System.IO
Imports System.Net
Imports DrvPakCreator.Utilities
Imports System.Threading
Imports System.ComponentModel
Imports DrvPakCreator.Classes.ProgressReporting
Imports System.Security.Cryptography

Public Class HPSoftPaqPreparerDialog

    Public SoftPaqUri As String
    Public DestinationExtractedSoftPaqDir As String
    Private SoftPaqPath As String
    Public SoftPaqSHA256 As String

    Private Downloader As New WebClient()
    Private ProgressString As String

    Private DownloadSpeed As Long

    Private sw As New Stopwatch()
    Private ts As New TimeSpan()

    Private DownloadError As Exception

    ' TODO Add metered network detection capabilities

    ' --- EXTRACTOR ---
    Private CurrentTaskStatus As TaskReporter
    Private unzipper As New ZipExtractor()

    Private Sub HPSoftPaqPreparerDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor

        ProgressTask1.SetColors()
        ProgressTask2.SetColors()

        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)

        ProgressTask1.TaskStatus = ProgressTaskStatus.InProgress
        ProgressTask2.TaskStatus = ProgressTaskStatus.NotStarted
        DownloadError = Nothing
        WindowHelper.DisableCloseCapability(Handle)
        SoftPaqPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(SoftPaqUri))
        DestinationExtractedSoftPaqDir = Path.Combine(Path.GetTempPath(), String.Format("HPSOFTPAQ_{0}", New Random().Next()))

        AddHandler unzipper.ProgressChanged, AddressOf OnZipProgress

#If VBC_VER >= 10.0 Then
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
#Else
    ' NOT SUPPORTED; FORCE STANDARD TLS FOR OLD WINDOWS WININET
    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
#End If

        AddHandler Downloader.DownloadProgressChanged, AddressOf WebClient_DownloadProgressChanged
        AddHandler Downloader.DownloadFileCompleted, AddressOf WebClient_DownloadFileCompleted

        sw.Start()
        tmrDownloadStatus.Start()

        ' Called from the UI thread, so WebClient's events are marshalled back to it
        Downloader.DownloadFileAsync(New Uri(SoftPaqUri), SoftPaqPath)
    End Sub

    Private Sub WebClient_DownloadProgressChanged(ByVal sender As Object, ByVal e As DownloadProgressChangedEventArgs)
        ProgressTask1.TaskPercentage = e.ProgressPercentage
        Dim useFrenchSizeUnits As Boolean = My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName.Equals("FRA", StringComparison.OrdinalIgnoreCase)
        ProgressString = String.Format("Downloading SoftPAQ... ({0} of {1} downloaded).", Converters.BytesToReadableSize(e.BytesReceived, useFrenchSizeUnits), Converters.BytesToReadableSize(e.TotalBytesToReceive, useFrenchSizeUnits))
        DownloadSpeed = CLng(Math.Round(e.BytesReceived / sw.Elapsed.TotalSeconds, 2))
        If e.TotalBytesToReceive > 0 Then
            ts = TimeSpan.FromSeconds((e.TotalBytesToReceive - e.BytesReceived) / CDbl(DownloadSpeed))
        End If
    End Sub

    Private Sub WebClient_DownloadFileCompleted(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
        ProgressTask1.TaskStatus = IIf(e.Error Is Nothing, ProgressTaskStatus.Succeeded, ProgressTaskStatus.Failed)
        ProgressTask1.TaskPercentage = -1
        If e.Error IsNot Nothing Then
            DownloadError = e.Error
            MessageBox.Show(Me, DownloadError.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
            Exit Sub
        End If
        sw.Stop()
        tmrDownloadStatus.Stop()

        StatusLbl.Text = "Verifying SoftPAQ Integrity..."
        Refresh()
        If SoftPaqSHA256 <> "" Then
            Try
                Using sr As Stream = File.OpenRead(SoftPaqPath)
                    Using hashCalculator As SHA256 = SHA256.Create()
                        Dim obtainedHash As Byte() = hashCalculator.ComputeHash(sr)
                        Dim obtainedHashString As String = BitConverter.ToString(obtainedHash).Replace("-", "").ToLowerInvariant()

                        If obtainedHashString <> SoftPaqSHA256.ToLowerInvariant() Then
                            MessageBox.Show(Me, "The download may not have been performed correctly.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End If
                    End Using
                End Using
            Catch ex As Exception

            End Try
        End If

        StatusLbl.Text = "Unpacking SoftPAQ. This will take some time..."
        ProgressTask2.TaskStatus = ProgressTaskStatus.InProgress
        Thread.Sleep(500)
        If Not bwPakUnpack.IsBusy Then
            bwPakUnpack.RunWorkerAsync()
        End If
    End Sub

    Private Sub tmrDownloadStatus_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrDownloadStatus.Tick
        StatusLbl.Text = ProgressString

        ' Append download speed and ETA to the status
        Dim downloadSpeedString As String = String.Format("Speed: {0}/s", Converters.BytesToReadableSize(DownloadSpeed, My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName.Equals("FRA", StringComparison.OrdinalIgnoreCase))), _
            downloadETAString As String = String.Format("ETA: {0}:{1:00} seconds", ts.Minutes, ts.Seconds)

        StatusLbl.Text &= String.Format(" {0}. {1}", downloadSpeedString, downloadETAString)
    End Sub

    Private Sub OnZipProgress(ByVal sender As Object, ByVal e As ZipProgressEventArgs)
        Dim msg As String = String.Format("({0}/{1}) Unpacking file {2}", e.FilesProcessed, e.TotalFiles, Path.GetFileName(e.CurrentFile.Replace("/", "\")))
        ReportFileUnpack(New TaskReporter(msg, e.Percentage))
    End Sub

    Private Sub ReportFileUnpack(ByVal OperationStatus As TaskReporter)
        CurrentTaskStatus = OperationStatus
        bwPakUnpack.ReportProgress(OperationStatus.Percentage)
    End Sub

    Private Sub bwPakUnpack_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwPakUnpack.DoWork
        UnzipDriverPack()
    End Sub

    Private Sub UnzipDriverPack()
        Try
            unzipper.SevenZipExecutablePath = Path.Combine(Application.StartupPath, "7z.exe")
            unzipper.ExtractAll(SoftPaqPath, DestinationExtractedSoftPaqDir)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub bwPakUnpack_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwPakUnpack.RunWorkerCompleted
        ProgressTask2.TaskStatus = IIf(e.Error Is Nothing, ProgressTaskStatus.Succeeded, ProgressTaskStatus.Failed)
        Refresh()
        If e.Error IsNot Nothing Then
            MessageBox.Show(e.Error.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Try
            StatusLbl.Text = "Deleting temporary files..."
            Refresh()
            File.Delete(SoftPaqPath)
        Catch ex As Exception

        End Try
        StatusLbl.Text = "Done"
        Refresh()
        DialogResult = Windows.Forms.DialogResult.OK
        Close()
    End Sub
End Class
