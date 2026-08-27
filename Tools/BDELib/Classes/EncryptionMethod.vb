Namespace Classes

    ''' <summary>
    ''' The encryption method for encrypted BitLocker volumes.
    ''' </summary>
    Public Enum EncryptionMethod As Integer
        ''' <summary>
        ''' Unknown or no encryption.
        ''' </summary>
        None = 0
        ''' <summary>
        ''' AES-128 encryption with diffuser.
        ''' </summary>
        Aes128Diffuser = 1
        ''' <summary>
        ''' AES-256 encryption with diffuser.
        ''' </summary>
        Aes256Diffuser = 2
        ''' <summary>
        ''' AES-128 encryption.
        ''' </summary>
        Aes128 = 3
        ''' <summary>
        ''' AES-256 encryption.
        ''' </summary>
        Aes256 = 4
        ''' <summary>
        ''' Hardware-based encryption.
        ''' </summary>
        HWEnc = 5
        ''' <summary>
        ''' XTS-AES-128 encryption.
        ''' </summary>
        XtsAes128 = 6
        ''' <summary>
        ''' XTS-AES-256 encryption with diffuser.
        ''' </summary>
        XtsAes256Diffuser = 7
    End Enum

End Namespace
