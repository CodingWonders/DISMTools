Namespace Classes

    ''' <summary>
    ''' The <see cref="EncryptableVolume"/> class holds basic information about an
    ''' encrypted BitLocker volume.
    ''' </summary>
    Public Class EncryptableVolume

        ''' <summary>
        ''' The device ID of a volume that can be used to associate a WMI instance to other
        ''' classes via an ASSOCIATORS OF query.
        ''' </summary>
        ''' <returns></returns>
        Public Property DeviceID As String
        ''' <summary>
        ''' The drive letter associated to the encrypted volume, if any.
        ''' </summary>
        ''' <returns></returns>
        Public Property DriveLetter As String
        ''' <summary>
        ''' The persistent volume ID of an encrypted volume.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' No persistent volume ID is present for decrypted volumes that are picked up when
        ''' querying Win32_EncryptableVolume.
        ''' </remarks>
        Public Property PersistentVolumeID As String

        Public Sub New(devId As String, drLetter As String, pvId As String)
            DeviceID = devId
            DriveLetter = drLetter
            PersistentVolumeID = pvId
        End Sub

    End Class

End Namespace
