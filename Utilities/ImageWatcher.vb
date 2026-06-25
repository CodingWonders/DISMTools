Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism

Namespace Utilities

    Public Class ImageWatcher

        Enum Status As Integer
            ''' <summary>
            ''' The image hasn't seen any major status changes
            ''' </summary>
            ''' <remarks></remarks>
            OK = 0
            ''' <summary>
            ''' The status of the image has changed to require a servicing session reload
            ''' </summary>
            ''' <remarks></remarks>
            NeedsRemount = 1
            ''' <summary>
            ''' The image is no longer mounted
            ''' </summary>
            ''' <remarks></remarks>
            NotMounted = 2
        End Enum

        Shared Function WatchStatus(imageFile As String, mountedImages As List(Of WindowsImage)) As Status
            If mountedImages IsNot Nothing AndAlso mountedImages.Count > 0 Then
                Try
                    Dim DetectedImage As WindowsImage = mountedImages.FirstOrDefault(Function(image) image.ImageFile = imageFile.Replace("\\", "\"))
                    If DetectedImage IsNot Nothing Then
                        Select Case DetectedImage.ImageMountStatus
                            Case DismMountStatus.Ok
                                Debug.WriteLine("[WatchImageStatus] INFO: Watcher has detected that the image " & Quote & imageFile & Quote & " is OK")
                                Return Status.OK
                            Case DismMountStatus.NeedsRemount
                                Debug.WriteLine("[WatchImageStatus] INFO: Watcher has detected that the image " & Quote & imageFile & Quote & " needs a servicing session reload")
                                Return Status.NeedsRemount
                        End Select
                    Else
                        Debug.WriteLine("[WatchImageStatus] INFO: The image file " & Quote & imageFile & Quote & " is no longer mounted")
                        Throw New Exception()
                    End If
                Catch ex As Exception
                    Return Status.NotMounted
                End Try
            End If
            Return Status.OK
        End Function

    End Class

End Namespace
