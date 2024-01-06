Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

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

        Shared Function WatchStatus(imageFile As String, mountedImages As List(Of String), imageStatusList As List(Of String)) As Status
            If mountedImages.Contains(imageFile) Then
                If mountedImages.Count > 0 Then
                    For x = 0 To Array.LastIndexOf(mountedImages.ToArray(), mountedImages.ToArray().Last)
                        If mountedImages(x) = imageFile Then
                            ' Detect its status. Do not detect whether an image is invalid, as unmounting an image causes it to become invalid
                            If imageStatusList(x) = "0" Then
                                Debug.WriteLine("[WatchImageStatus] INFO: Watcher has detected that the image " & Quote & imageFile & Quote & " is OK")
                                Return Status.OK
                            ElseIf imageStatusList(x) = "1" Then
                                Debug.WriteLine("[WatchImageStatus] INFO: Watcher has detected that the image " & Quote & imageFile & Quote & " needs a servicing session reload")
                                Return Status.NeedsRemount
                            End If
                            Exit For
                        End If
                    Next
                End If
            Else
                Debug.WriteLine("[WatchImageStatus] INFO: The image file " & Quote & imageFile & Quote & " is no longer mounted")
                Return Status.NotMounted
            End If
            Return Status.OK
        End Function

    End Class

End Namespace
