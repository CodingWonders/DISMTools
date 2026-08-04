Namespace Classes

    ''' <summary>
    ''' The <see cref="KeyProtector"/> class holds basic information about a key protector
    ''' used to protect an encrypted BitLocker volume.
    ''' </summary>
    Public Class KeyProtector

        ''' <summary>
        ''' The identifier for the key protector.
        ''' </summary>
        ''' <returns></returns>
        Public Property ProtectorID As String
        ''' <summary>
        ''' The type of the key protector.
        ''' </summary>
        ''' <returns></returns>
        Public Property ProtectorType As KeyProtectorType

        Public Sub New(protectorId As String, protectorType As KeyProtectorType)
            Me.ProtectorID = protectorId
            Me.ProtectorType = protectorType
        End Sub

    End Class

End Namespace
