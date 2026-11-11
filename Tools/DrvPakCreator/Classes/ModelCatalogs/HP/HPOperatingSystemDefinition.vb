Namespace Classes.ModelCatalogs.HP

    Public Class HPOperatingSystemDefinitionCollection

        Private _operatingSystems As List(Of HPOperatingSystemDefinition)

        <XmlElement("OS")> _
        Public Property OperatingSystems() As List(Of HPOperatingSystemDefinition)
            Get
                Return _operatingSystems
            End Get
            Set(ByVal value As List(Of HPOperatingSystemDefinition))
                _operatingSystems = value
            End Set
        End Property

        Public Sub New()
            OperatingSystems = New List(Of HPOperatingSystemDefinition)
        End Sub

    End Class

    Public Class HPOperatingSystemDefinition

        Private _name As String
        Private _shortName As String
        Private _bitness As String
        Private _osId As Integer

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property ShortName() As String
            Get
                Return _shortName
            End Get
            Set(ByVal value As String)
                _shortName = value
            End Set
        End Property

        Public Property Bitness() As String
            Get
                Return _bitness
            End Get
            Set(ByVal value As String)
                _bitness = value
            End Set
        End Property

        Public Property Id() As Integer
            Get
                Return _osId
            End Get
            Set(ByVal value As Integer)
                _osId = value
            End Set
        End Property

    End Class

End Namespace