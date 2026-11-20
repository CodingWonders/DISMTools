Namespace Classes.ModelCatalogs.Dell

    <XmlType(Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    Public Class DellDriverPackage

        Private _format As String
        Private _md5Hash As String
        Private _size As Long
        Private _releaseDate As String
        Private _vendorVersion As String
        Private _dellVersion As String
        Private _path As String
        Private _id As String
        Private _delta As String
        Private _packageType As String
        Private _name As DellLocalizedText
        Private _description As DellLocalizedText
        Private _operatingSystems As DellOperatingSystemDefinitionCollection
        Private _brands As DellBrandCollection
        Private _importantInfo As DellImportantInfo
        Private _hashes As DellHashCollection
        Private _metadata As DellDriverPackMetadata

        <XmlAttribute("format")> _
        Public Property Format() As String
            Get
                Return _format
            End Get
            Set(ByVal value As String)
                _format = value
            End Set
        End Property

        <XmlAttribute("hashMD5")> _
        Public Property MD5() As String
            Get
                Return _md5Hash
            End Get
            Set(ByVal value As String)
                _md5Hash = value
            End Set
        End Property

        <XmlAttribute("size")> _
        Public Property Size() As Long
            Get
                Return _size
            End Get
            Set(ByVal value As Long)
                _size = value
            End Set
        End Property

        <XmlAttribute("dateTime")> _
        Public Property ReleaseDate() As String
            Get
                Return _releaseDate
            End Get
            Set(ByVal value As String)
                _releaseDate = value
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

        <XmlAttribute("vendorVersion")> _
        Public Property VendorVersion() As String
            Get
                Return _vendorVersion
            End Get
            Set(ByVal value As String)
                _vendorVersion = value
            End Set
        End Property

        <XmlAttribute("dellVersion")> _
        Public Property DellVersion() As String
            Get
                Return _dellVersion
            End Get
            Set(ByVal value As String)
                _dellVersion = value
            End Set
        End Property

        <XmlAttribute("path")> _
        Public Property Path() As String
            Get
                Return _path
            End Get
            Set(ByVal value As String)
                _path = value
            End Set
        End Property

        <XmlAttribute("releaseID")> _
        Public Property Id() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property

        <XmlAttribute("delta")> _
        Public Property Delta() As String
            Get
                Return _delta
            End Get
            Set(ByVal value As String)
                _delta = value
            End Set
        End Property

        <XmlAttribute("type")> _
        Public Property PackageType() As String
            Get
                Return _packageType
            End Get
            Set(ByVal value As String)
                _packageType = value
            End Set
        End Property

        <XmlElement("Name")> _
        Public Property Name() As DellLocalizedText
            Get
                Return _name
            End Get
            Set(ByVal value As DellLocalizedText)
                _name = value
            End Set
        End Property

        <XmlElement("Description")> _
        Public Property Description() As DellLocalizedText
            Get
                Return _description
            End Get
            Set(ByVal value As DellLocalizedText)
                _description = value
            End Set
        End Property

        <XmlElement("SupportedOperatingSystems")> _
        Public Property OperatingSystems() As DellOperatingSystemDefinitionCollection
            Get
                Return _operatingSystems
            End Get
            Set(ByVal value As DellOperatingSystemDefinitionCollection)
                _operatingSystems = value
            End Set
        End Property

        <XmlElement("SupportedSystems")> _
        Public Property Brands() As DellBrandCollection
            Get
                Return _brands
            End Get
            Set(ByVal value As DellBrandCollection)
                _brands = value
            End Set
        End Property

        <XmlElement("ImportantInfo")> _
        Public Property ImportantInfo() As DellImportantInfo
            Get
                Return _importantInfo
            End Get
            Set(ByVal value As DellImportantInfo)
                _importantInfo = value
            End Set
        End Property

        <XmlElement("Cryptography")> _
        Public Property Hashes() As DellHashCollection
            Get
                Return _hashes
            End Get
            Set(ByVal value As DellHashCollection)
                _hashes = value
            End Set
        End Property

        <XmlElement("DriverPackMetadataInfo")> _
        Public Property Metadata() As DellDriverPackMetadata
            Get
                Return _metadata
            End Get
            Set(ByVal value As DellDriverPackMetadata)
                _metadata = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            If Name Is Nothing Then
                Return Id
            End If
            Return String.Format("ID {0} -- {1}", Id, Name.Text)
        End Function

    End Class

End Namespace