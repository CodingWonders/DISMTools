Namespace Classes

    Public Class EncryptableVolume

        Public Property DeviceID As String
        Public Property DriveLetter As String
        Public Property PersistentVolumeID As String

        Public Sub New(devId As String, drLetter As String, pvId As String)
            DeviceID = devId
            DriveLetter = drLetter
            PersistentVolumeID = pvId
        End Sub

    End Class

End Namespace
