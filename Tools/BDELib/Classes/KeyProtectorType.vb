Namespace Classes

    ''' <summary>
    ''' The type of a key protector in an encrypted BitLocker volume.
    ''' </summary>
    Public Enum KeyProtectorType As Integer
        ''' <summary>
        ''' Unknown or no protector.
        ''' </summary>
        Unknown = 0
        ''' <summary>
        ''' Trusted Platform Module
        ''' </summary>
        TPM = 1
        ''' <summary>
        ''' External Key
        ''' </summary>
        ExternalKey = 2
        ''' <summary>
        ''' 48-digit numerical password.
        ''' </summary>
        NumericalPassword = 3
        ''' <summary>
        ''' Trusted Platform Module and PIN combination.
        ''' </summary>
        TPMPin = 4
        ''' <summary>
        ''' Trusted Platform Module and startup key combination.
        ''' </summary>
        TPMStartupKey = 5
        ''' <summary>
        ''' Trusted Platform Module, PIN, and startup key combination.
        ''' </summary>
        TPMPinStartupKey = 6
        ''' <summary>
        ''' Public key.
        ''' </summary>
        PublicKey = 7
        ''' <summary>
        ''' Passphrase.
        ''' </summary>
        Passphrase = 8
        ''' <summary>
        ''' Trusted Platform Module certificate.
        ''' </summary>
        TPMCert = 9
        ''' <summary>
        ''' CryptoAPI Next Generation protector.
        ''' </summary>
        CNG = 10
    End Enum

End Namespace