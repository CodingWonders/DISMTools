Namespace Classes

    Public Class DynaLogEvent

        Private _eventTimestamp As String
        Private _eventCaller As String
        Private _eventMessage As String

        Public Sub New()

        End Sub

        Public Sub New(ByVal Timestamp As String, ByVal Caller As String, ByVal Message As String)
            Me._eventTimestamp = Timestamp
            Me._eventCaller = Caller
            Me._eventMessage = Message
        End Sub

        Public Property EventTimestamp() As String
            Get
                Return _eventTimestamp
            End Get
            Set(ByVal value As String)
                _eventTimestamp = value
            End Set
        End Property

        Public Property EventCaller() As String
            Get
                Return _eventCaller
            End Get
            Set(ByVal value As String)
                _eventCaller = value
            End Set
        End Property

        Public Property EventMessage() As String
            Get
                Return _eventMessage
            End Get
            Set(ByVal value As String)
                _eventMessage = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("[{0}] [{1}] {2}", Me._eventTimestamp, Me._eventCaller, Me._eventMessage)
        End Function

    End Class

End Namespace