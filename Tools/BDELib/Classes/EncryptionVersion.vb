Namespace Classes

    ''' <summary>
    ''' The encryption version for encrypted BitLocker volumes.
    ''' </summary>
    Public Enum EncryptionVersion As Integer
        ''' <summary>
        ''' Unknown or no encryption.
        ''' </summary>
        Unknown = 0
        ''' <summary>
        ''' BitLocker encryption from a system running Windows Vista or Windows Server 2008.
        ''' </summary>
        Vista = 1
        ''' <summary>
        ''' BitLocker encryption from a system running Windows 7 or later, or Windows
        ''' Server 2008 R2 or later.
        ''' </summary>
        Seven = 2
    End Enum

End Namespace
