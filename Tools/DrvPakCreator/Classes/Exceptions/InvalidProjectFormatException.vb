Namespace Classes.Exceptions

    Public Class InvalidProjectFormatException
        Inherits Exception

        Public Overrides ReadOnly Property Message() As String
            Get
                Return "The project loaded is of invalid format."
            End Get
        End Property

    End Class

End Namespace