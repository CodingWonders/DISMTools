Namespace Classes

    ''' <summary>
    ''' The status for volume conversion of a BitLocker volume.
    ''' </summary>
    Public Enum VolumeConversionStatus As Integer
        ''' <summary>
        ''' Unknown conversion status.
        ''' </summary>
        Unknown = -1
        ''' <summary>
        ''' The volume is fully decrypted.
        ''' </summary>
        FullyDecrypted = 0
        ''' <summary>
        ''' The volume is fully encrypted.
        ''' </summary>
        FullyEncrypted = 1
        ''' <summary>
        ''' The volume is being encrypted.
        ''' </summary>
        EncryptionInProgress = 2
        ''' <summary>
        ''' The volume is being decrypted.
        ''' </summary>
        DecryptionInProgress = 3
        ''' <summary>
        ''' Encryption has been paused on the volume.
        ''' </summary>
        EncryptionPaused = 4
        ''' <summary>
        ''' Decryption has been paused on the volume.
        ''' </summary>
        DecryptionPaused = 5
    End Enum

End Namespace
