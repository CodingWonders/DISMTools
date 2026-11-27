Imports DISMTools.Elements.Contemporaneus
Imports Microsoft.Dism
Imports System.Threading
Imports System.Threading.Tasks

Public Class ImageUtils

    Private Shared ReadOnly MountedImageDetectionSemaphore As New SemaphoreSlim(1, 1)

    ''' <summary>
    ''' Gets the images that are mounted in a system.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Async Function GetMountedImages() As Task(Of List(Of WindowsImage))
        Await MountedImageDetectionSemaphore.WaitAsync()
        Try
            Return Await Task.Run(Function()
                                      Dim mountedImages As New List(Of WindowsImage)
                                      Dim apiInitialized As Boolean = False
                                      Try
                                          DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
                                          apiInitialized = True
                                          Dim mountedImgs As DismMountedImageInfoCollection = DismApi.GetMountedImages()
                                          For Each imageInfo As DismMountedImageInfo In mountedImgs
                                              mountedImages.Add(New WindowsImage(imageInfo.ImageFilePath, imageInfo.ImageIndex, imageInfo.MountPath, imageInfo.MountStatus, imageInfo.MountMode))
                                          Next
                                      Catch ex As Exception
                                          DynaLog.LogMessage("Could not detect mounted images. Error message: " & ex.Message)
                                      Finally
                                          If apiInitialized Then
                                              Try
                                                  DismApi.Shutdown()
                                              Catch ex As Exception
                                                  ' ignore
                                              End Try
                                          End If
                                      End Try
                                      Return mountedImages
                                  End Function)
        Finally
            MountedImageDetectionSemaphore.Release()
        End Try
    End Function

End Class
