Namespace Classes

    Public Enum KeyProtectorType As Integer
        Unknown = 0
        TPM = 1
        ExternalKey = 2
        NumericalPassword = 3
        TPMPin = 4
        TPMStartupKey = 5
        TPMPinStartupKey = 6
        PublicKey = 7
        Passphrase = 8
        TPMCert = 9
        CNG = 10
    End Enum

End Namespace