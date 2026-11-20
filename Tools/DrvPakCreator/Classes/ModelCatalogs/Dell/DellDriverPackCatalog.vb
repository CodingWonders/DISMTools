Namespace Classes.ModelCatalogs.Dell

    <XmlRoot("DriverPackManifest", Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    <XmlType(Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    Public Class DellDriverPackCatalog
        Inherits ModelCatalog

        ''' <summary>Every element of the Dell catalog lives in this XML namespace.</summary>
        Public Const XmlNamespace As String = "openmanage/cm/dm"

        Public Overrides ReadOnly Property Manufacturer() As String
            Get
                Return "Dell"
            End Get
        End Property

        Private _baseLocation As String
        Private _accessProtocols As String
        Private _deltaBase As String
        Private _relDate As String
        Private _releaseId As String
        Private _version As String
        Private _application As String
        Private _schemaVer As String
        Private _driverPackages As List(Of DellDriverPackage)

        <XmlAttribute("baseLocation")> _
        Public Property BaseLocation() As String
            Get
                Return _baseLocation
            End Get
            Set(ByVal value As String)
                _baseLocation = value
            End Set
        End Property

        <XmlAttribute("baseLocationAccessProtocols")> _
        Public Property BaseLocationAccessProtocols() As String
            Get
                Return _accessProtocols
            End Get
            Set(ByVal value As String)
                _accessProtocols = value
            End Set
        End Property

        <XmlAttribute("deltaBase")> _
        Public Property DeltaBase() As String
            Get
                Return _deltaBase
            End Get
            Set(ByVal value As String)
                _deltaBase = value
            End Set
        End Property

        <XmlAttribute("dateTime")> _
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
                Dim parsedDate As Date
                If Date.TryParse(ReleaseDate, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, parsedDate) Then
                    Return parsedDate
                End If
                Return New Date(1970, 1, 1, 0, 0, 0)
            End Get
        End Property

        <XmlAttribute("releaseID")> _
        Public Property ReleaseId() As String
            Get
                Return _releaseId
            End Get
            Set(ByVal value As String)
                _releaseId = value
            End Set
        End Property

        <XmlAttribute("version")> _
        Public Property Version() As String
            Get
                Return _version
            End Get
            Set(ByVal value As String)
                _version = value
            End Set
        End Property

        <XmlAttribute("application")> _
        Public Property Application() As String
            Get
                Return _application
            End Get
            Set(ByVal value As String)
                _application = value
            End Set
        End Property

        <XmlAttribute("schemaVersion")> _
        Public Property SchemaVersion() As String
            Get
                Return _schemaVer
            End Get
            Set(ByVal value As String)
                _schemaVer = value
            End Set
        End Property

        <XmlElement("DriverPackage")> _
        Public Property DriverPackages() As List(Of DellDriverPackage)
            Get
                Return _driverPackages
            End Get
            Set(ByVal value As List(Of DellDriverPackage))
                _driverPackages = value
            End Set
        End Property

        Public Sub New()
            DriverPackages = New List(Of DellDriverPackage)
        End Sub

    End Class

End Namespace