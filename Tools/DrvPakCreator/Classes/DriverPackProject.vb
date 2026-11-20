Namespace Classes

    Public Class DriverPackProject

        Private _signature As String
        Private _name As String
        Private _packMetadata As DriverPack

        Public Property Signature() As String
            Get
                Return _signature
            End Get
            Set(ByVal value As String)
                _signature = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property PackMetadata() As DriverPack
            Get
                Return _packMetadata
            End Get
            Set(ByVal value As DriverPack)
                _packMetadata = value
            End Set
        End Property

    End Class

End Namespace