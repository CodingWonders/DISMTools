Imports System.IO
Imports Microsoft.Dism
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Win32
Imports DISMTools.Utilities

Public Class AutoReloadForm

    Dim SuccessfulReloads, FailedReloads, SkippedReloads As Integer
    Dim ImgCount As Integer
    Dim message As String
    Dim mntMsg As String
    Dim fileMsg As String
    Dim ImgFiles As New List(Of String)
    Dim MountDirs As New List(Of String)
    Dim MountStatus As New List(Of DismMountStatus)

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        DynaLog.LogMessage("Preparing to remount any orphaned Windows images...")
        DynaLog.LogMessage("Mounted image detector might be busy. Stopping it if it is...")
        MainForm.StopMountedImageDetector()
        DynaLog.LogMessage("Initializing API...")
        DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
        Dim MountedImages As DismMountedImageInfoCollection = DismApi.GetMountedImages()
        DynaLog.LogMessage("Information collection count: " & MountedImages.Count)
        ImgCount = MountedImages.Count
        If MountedImages.Count > 0 Then
            DynaLog.LogMessage("Images have been mounted on this system. Storing information...")
            For Each imageinfo As DismMountedImageInfo In MountedImages
                ImgFiles.Add(imageinfo.ImageFilePath)
                MountDirs.Add(imageinfo.MountPath)
                MountStatus.Add(imageinfo.MountStatus)
            Next
            fileMsg = ImgFiles(0)
            mntMsg = MountDirs(0)
        End If
        DismApi.Shutdown()
        BackgroundWorker1.ReportProgress(0)
        If MountDirs.Count > 0 Then
            DynaLog.LogMessage("Reloading the servicing sessions of any orphaned Windows images...")
            Try
                DynaLog.LogMessage("Initializing API...")
                DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
                For x = 0 To Array.LastIndexOf(MountDirs.ToArray(), MountDirs.ToArray().Last)
                    fileMsg = ImgFiles(x)
                    mntMsg = MountDirs(x)
                    message = LocalizationService.ForSection("AutoReload.Bg").Format("ReloadSucceeded.Message", SuccessfulReloads, FailedReloads, SkippedReloads)
                    BackgroundWorker1.ReportProgress((x / ImgCount) * 100)
                    DynaLog.LogMessage("Checking mount status of image file " & Quote & fileMsg & Quote & "...")
                    Try
                        If MountStatus(x) = DismMountStatus.NeedsRemount Then
                            DynaLog.LogMessage("The image needs to be remounted. Remounting...")
                            DismApi.RemountImage(MountDirs(x))
                            SuccessfulReloads += 1
                        Else
                            DynaLog.LogMessage("The image is fine. Skipping...")
                            SkippedReloads += 1
                        End If
                    Catch ex As Exception
                        DynaLog.LogMessage("Could not remount the image. Error message: " & ex.Message)
                        FailedReloads += 1
                    End Try
                Next
            Catch ex As Exception
                DynaLog.LogMessage("Could not remount all orphaned images. Reason:" & CrLf & ex.ToString())
            Finally
                Try
                    DynaLog.LogMessage("Shutting down API...")
                    DismApi.Shutdown()
                Catch ex As Exception

                End Try
            End Try
        End If
        DynaLog.LogMessage("This process has completed.")
        message = LocalizationService.ForSection("AutoReload.Background")("ProcessCompleted.Message")
        BackgroundWorker1.ReportProgress(100)
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        Select Case e.ProgressPercentage
            Case 0
                Label2.Text = LocalizationService.ForSection("AutoReload.Progress")("Preparing.Images.Label")
            Case Else
                Label2.Text = message
        End Select
        imgFile.Text = fileMsg
        imgMtPnt.Text = mntMsg
        ProgressBar1.Style = ProgressBarStyle.Blocks
        ProgressBar1.Value = e.ProgressPercentage
        TaskbarHelper.SetIndicatorState(e.ProgressPercentage, Windows.Shell.TaskbarItemProgressState.Normal, Handle)
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Refresh()
        TaskbarHelper.SetIndicatorState(100, Windows.Shell.TaskbarItemProgressState.None, Handle)
        Application.DoEvents()
        Thread.Sleep(1000)
        Close()
    End Sub

    Private Sub AutoReloadForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim ColorModeRk As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Themes\Personalize")
            Select Case ColorModeRk.GetValue("AppsUseLightTheme").ToString()
                Case "0"
                    BackColor = CurrentTheme.SectionBackgroundColor
                    ForeColor = CurrentTheme.ForegroundColor
                Case "1"
                    BackColor = Color.FromArgb(238, 238, 242)
                    ForeColor = Color.Black
            End Select
            GroupBox1.ForeColor = ForeColor
            ColorModeRk.Close()
        Catch ex As Exception
            ' Continue
        End Try
        Label1.Text = LocalizationService.ForSection("AutoReload")("Wait.Message")
        Label3.Text = LocalizationService.ForSection("AutoReload")("ImageFile.Label")
        Label4.Text = LocalizationService.ForSection("AutoReload")("Image.Mount.Point.Label")
        GroupBox1.Text = LocalizationService.ForSection("AutoReload")("ImageInfo.Group")
        TaskbarHelper.SetIndicatorState(0, Windows.Shell.TaskbarItemProgressState.Indeterminate, Handle)
        Thread.Sleep(2000)
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub AutoReloadForm_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.FromArgb(53, 153, 41), ButtonBorderStyle.Solid)
    End Sub

    Private Sub AutoReloadForm_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If WindowState = FormWindowState.Maximized Then
            WindowState = FormWindowState.Normal
        End If
    End Sub
End Class
