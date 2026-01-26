Imports Microsoft.Dism
Imports System.Diagnostics
Imports System.Threading
Imports System.ComponentModel
Imports AutoReloadSvc.Classes

Public Class AutoReloadService

    Private ReloadResultDic As New Dictionary(Of DismMountedImageInfo, ReloadResult)

    Public Sub New()
        InitializeComponent()
        AutoLog = False

        If Not EventLog.SourceExists("DT-AutoReload") Then
            EventLog.CreateEventSource(AutoReloadEL.Source, AutoReloadEL.Log)
        End If
    End Sub

    Protected Overrides Sub OnStart(ByVal args() As String)
        RequestAdditionalTime(30000)
        Task.Run(Sub()
                     Try
                         ReloadImages()
                     Finally
                         Thread.Sleep(500)
                         Me.Stop()
                     End Try
                 End Sub)
    End Sub

    Private Sub ReloadImages()
        AutoReloadEL.WriteEntry("Preparing to reload images...")
        Try
            DismApi.Initialize(DismLogLevel.LogErrors)
            Dim MountedImages As DismMountedImageInfoCollection = DismApi.GetMountedImages()
            If MountedImages.Any(Function(image) image.MountStatus >= DismMountStatus.NeedsRemount) Then
                For Each MountedImage In MountedImages
                    Dim exitCode As Integer,
                        reloadResultType As ReloadResultType = Classes.ReloadResultType.Unknown
                    Try
                        If MountedImage.MountStatus = DismMountStatus.Ok Then
                            reloadResultType = Classes.ReloadResultType.Skipped
                        Else
                            DismApi.RemountImage(MountedImage.MountPath)
                            reloadResultType = Classes.ReloadResultType.Succeeded
                        End If
                        exitCode = 0
                    Catch DismEx As DismException
                        exitCode = DismEx.HResult
                    Catch ex As Exception
                        exitCode = ex.HResult
                    Finally
                        If exitCode <> 0 Then reloadResultType = Classes.ReloadResultType.Failed
                        ReloadResultDic.Add(MountedImage, New ReloadResult(reloadResultType, exitCode))
                    End Try
                Next
            Else
                AutoReloadEL.WriteEntry("No images need to be reloaded.")
                Exit Try
            End If
            Me.ExitCode = 0
        Catch ex As Exception
            AutoReloadEL.WriteEntry(String.Format("An error occurred while reloading images. Error Code: {0}", ex.HResult), EventLogEntryType.Error)
        Finally
            Try
                DismApi.Shutdown()
            Catch ex As Exception
                ' Ignore exceptions
            End Try

            Dim reloadMessage As String = "Image reload operations have finished. Results:" & Environment.NewLine
            For Each ReloadResultEntry In ReloadResultDic.Keys
                reloadMessage &= "- " & ControlChars.Quote & ReloadResultEntry.ImageFilePath & ControlChars.Quote & " (mounted at " & ControlChars.Quote & ReloadResultEntry.MountPath & ControlChars.Quote & ") -- "

                Select Case ReloadResultDic(ReloadResultEntry).ResultType
                    Case ReloadResultType.Succeeded
                        reloadMessage &= "Successfully reloaded"
                    Case ReloadResultType.Failed
                        reloadMessage &= "Failed to reload with code 0x" &
                            Hex(ReloadResultDic(ReloadResultEntry).ResultCode).PadLeft(8, "0") &
                            " (" & New Win32Exception(ReloadResultDic(ReloadResultEntry).GetHashCode).Message & ")"
                    Case ReloadResultType.Skipped
                        reloadMessage &= "Not necessary to reload"
                End Select
                reloadMessage &= Environment.NewLine
            Next
            AutoReloadEL.WriteEntry(reloadMessage)
        End Try
    End Sub

    Protected Overrides Sub OnShutdown()
        MyBase.OnShutdown()
    End Sub

    Protected Overrides Sub OnStop()
        AutoReloadEL.WriteEntry("Task Finished.")
    End Sub

End Class
