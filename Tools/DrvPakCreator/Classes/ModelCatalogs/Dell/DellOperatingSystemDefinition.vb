Namespace Classes.ModelCatalogs.Dell

    <XmlType(Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    Public Class DellOperatingSystemDefinitionCollection

        Private _operatingSystems As List(Of DellOperatingSystemDefinition)

        <XmlElement("OperatingSystem")> _
        Public Property OperatingSystems() As List(Of DellOperatingSystemDefinition)
            Get
                Return _operatingSystems
            End Get
            Set(ByVal value As List(Of DellOperatingSystemDefinition))
                _operatingSystems = value
            End Set
        End Property

        Public Sub New()
            OperatingSystems = New List(Of DellOperatingSystemDefinition)
        End Sub

    End Class

    <XmlType(Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    Public Class DellOperatingSystemDefinition

        Private _code As String
        Private _vendor As String
        Private _architecture As String
        Private _osType As String
        Private _majorVersion As Integer
        Private _minorVersion As Integer
        Private _spMajorVersion As Integer
        Private _spMinorVersion As Integer
        Private _display As DellDisplay
        Private _languages As DellLanguageCollection

        <XmlAttribute("osCode")> _
        Public Property Code() As String
            Get
                Return _code
            End Get
            Set(ByVal value As String)
                _code = value
            End Set
        End Property

        <XmlAttribute("osVendor")> _
        Public Property Vendor() As String
            Get
                Return _vendor
            End Get
            Set(ByVal value As String)
                _vendor = value
            End Set
        End Property

        <XmlAttribute("osArch")> _
        Public Property Architecture() As String
            Get
                Return _architecture
            End Get
            Set(ByVal value As String)
                _architecture = value
            End Set
        End Property

        <XmlAttribute("osType")> _
        Public Property OSType() As String
            Get
                Return _osType
            End Get
            Set(ByVal value As String)
                _osType = value
            End Set
        End Property

        <XmlAttribute("majorVersion")> _
        Public Property MajorVersion() As Integer
            Get
                Return _majorVersion
            End Get
            Set(ByVal value As Integer)
                _majorVersion = value
            End Set
        End Property

        <XmlAttribute("minorVersion")> _
        Public Property MinorVersion() As Integer
            Get
                Return _minorVersion
            End Get
            Set(ByVal value As Integer)
                _minorVersion = value
            End Set
        End Property

        <XmlAttribute("spMajorVersion")> _
        Public Property ServicePackMajorVersion() As Integer
            Get
                Return _spMajorVersion
            End Get
            Set(ByVal value As Integer)
                _spMajorVersion = value
            End Set
        End Property

        <XmlAttribute("spMinorVersion")> _
        Public Property ServicePackMinorVersion() As Integer
            Get
                Return _spMinorVersion
            End Get
            Set(ByVal value As Integer)
                _spMinorVersion = value
            End Set
        End Property

        <XmlElement("Display")> _
        Public Property Display() As DellDisplay
            Get
                Return _display
            End Get
            Set(ByVal value As DellDisplay)
                _display = value
            End Set
        End Property

        <XmlElement("SupportedLanguages")> _
        Public Property Languages() As DellLanguageCollection
            Get
                Return _languages
            End Get
            Set(ByVal value As DellLanguageCollection)
                _languages = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            If Display Is Nothing Then
                Return Code
            End If
            Return String.Format("{0} -- {1}", Code, Display.ToString())
        End Function

    End Class

End Namespace