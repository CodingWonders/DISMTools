Namespace Classes

    Public Class KeyProtector

        Public Property ProtectorID As String
        Public Property ProtectorType As KeyProtectorType

        Public Sub New(protectorId As String, protectorType As KeyProtectorType)
            Me.ProtectorID = protectorId
            Me.ProtectorType = protectorType
        End Sub

    End Class

End Namespace
