Imports BDELib.Classes
Imports BDELib.Helpers

Public Class BDELib

    Public Shared Function GetEncryptedVolumes() As List(Of EncryptableVolume)
        Return VolumeHelper.GetEncryptedVolumes()
    End Function

    Public Shared Function GetKeyProtectors(PersistentVolumeId As String) As List(Of KeyProtector)
        Return VolumeHelper.GetVolumeKeyProtectors(PersistentVolumeId)
    End Function

    Public Shared Function GetLockStatus(PersistentVolumeId As String) As LockStatus
        Return VolumeHelper.GetVolumeLockStatus(PersistentVolumeId)
    End Function

    Public Shared Function LockVolume(PersistentVolumeId As String) As UInteger
        Return VolumeHelper.LockVolume(PersistentVolumeId)
    End Function

    Public Shared Function UnlockVolumeWithNumericalPassword(PersistentVolumeId As String, NumericalPassword As String) As UInteger
        Return VolumeHelper.UnlockEncryptedVolumeWithNumericalPassword(PersistentVolumeId, NumericalPassword)
    End Function

    Public Shared Function GetVolumeConversionStatus(PersistentVolumeId As String, Optional PrecisionFactor As Integer = 4) As ConversionStatus
        Return VolumeHelper.GetVolumeConversionStatus(PersistentVolumeId, PrecisionFactor)
    End Function

    Public Shared Async Function StartVolumeDecryption(PersistentVolumeId As String, Optional DecryptionProgressReporter As Action(Of ConversionStatus) = Nothing) As Task(Of UInteger)
        Dim decryptionResult As UInteger = Await VolumeHelper.StartVolumeDecryption(PersistentVolumeId, DecryptionProgressReporter)
        Return decryptionResult
    End Function

End Class
