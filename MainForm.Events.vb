Partial Class MainForm
    ' Event raised when MountedImageList changes
    Public Event MountedImagesUpdated As EventHandler

    Public Sub RaiseMountedImagesUpdated()
        Try
            RaiseEvent MountedImagesUpdated(Me, EventArgs.Empty)
        Catch
            ' Ignore subscriber exceptions
        End Try
    End Sub
End Class