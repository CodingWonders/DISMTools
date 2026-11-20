Namespace Classes.Exceptions

    Public Class CorruptPackMetadataException
        Inherits Exception

        Public Overrides ReadOnly Property Message() As String
            Get
                Return "Driver Pack Metadata is invalid."
            End Get
        End Property

    End Class

End Namespace