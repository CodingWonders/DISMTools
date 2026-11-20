Namespace Classes.ModelCatalogs.Lenovo

    Public MustInherit Class LenovoDownload

        Private _releaseDate As String
        Private _sha256Hash As String
        Private _md5Hash As String
        Private _uri As String

        <XmlAttribute("date")> _
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

        ''' <summary>Lenovo stores a SHA-256 digest (64 hex characters) in an attribute called "crc".</summary>
        <XmlAttribute("crc")> _
        Public Property SHA256() As String
            Get
                Return _sha256Hash
            End Get
            Set(ByVal value As String)
                _sha256Hash = value
            End Set
        End Property

        <XmlAttribute("md5")> _
        Public Property MD5() As String
            Get
                Return _md5Hash
            End Get
            Set(ByVal value As String)
                _md5Hash = value
            End Set
        End Property

        <XmlText()> _
        Public Property Uri() As String
            Get
                Return _uri
            End Get
            Set(ByVal value As String)
                _uri = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Uri
        End Function

    End Class

End Namespace