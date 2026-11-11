Namespace Classes.ModelCatalogs.HP

    Public Class HPDriverPackCatalog
        Inherits ModelCatalog

        Public Overrides ReadOnly Property Manufacturer() As String
            Get
                Return "HP"
            End Get
        End Property

        Private _schemaVer As String
        Private _toolVer As String
        Private _relDate As String
        Private _operatingSystems As HPOperatingSystemDefinitionCollection
        Private _softPaqs As HPSoftPaqCollection
        Private _products As HPProductOSDriverPackCollection

        <XmlAttribute("SchemaVersion")> _
        Public Property SchemaVersion() As String
            Get
                Return _schemaVer
            End Get
            Set(ByVal value As String)
                _schemaVer = value
            End Set
        End Property

        <XmlAttribute("ToolVersion")> _
        Public Property ToolVersion() As String
            Get
                Return _toolVer
            End Get
            Set(ByVal value As String)
                _toolVer = value
            End Set
        End Property

        <XmlAttribute("DateReleased")> _
        Public Property ReleaseDate() As String
            Get
                Return _relDate
            End Get
            Set(ByVal value As String)
                _relDate = value
            End Set
        End Property

        Public ReadOnly Property DateReleased() As Date
            Get
                Dim finalDate As String = New Date(1970, 1, 1, 0, 0, 0)
                Date.TryParse(ReleaseDate, finalDate)
                Return finalDate
            End Get
        End Property

        <XmlElement("OSList")> _
        Public Property OperatingSystems() As HPOperatingSystemDefinitionCollection
            Get
                Return _operatingSystems
            End Get
            Set(ByVal value As HPOperatingSystemDefinitionCollection)
                _operatingSystems = value
            End Set
        End Property

        <XmlElement("SoftPaqList")> _
        Public Property SoftPaqs() As HPSoftPaqCollection
            Get
                Return _softPaqs
            End Get
            Set(ByVal value As HPSoftPaqCollection)
                _softPaqs = value
            End Set
        End Property

        <XmlElement("ProductOSDriverPackList")> _
        Public Property Products() As HPProductOSDriverPackCollection
            Get
                Return _products
            End Get
            Set(ByVal value As HPProductOSDriverPackCollection)
                _products = value
            End Set
        End Property


    End Class

End Namespace