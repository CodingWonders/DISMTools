Namespace Classes.ProgressReporting

    Public Class TaskReporter

        Private _message As String
        Private _percentage As Double

        Public Property Message() As String
            Get
                Return _message
            End Get
            Set(ByVal value As String)
                _message = value
            End Set
        End Property

        Public Property Percentage() As Double
            Get
                Return _percentage
            End Get
            Set(ByVal value As Double)
                _percentage = value
            End Set
        End Property

        Public Sub New(ByVal Message As String, ByVal Percentage As Double)
            Me.Message = Message
            Me.Percentage = Percentage
        End Sub

    End Class

End Namespace