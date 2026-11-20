Namespace Classes.ModelCatalogs.HP

    Public Class HPSoftPaqCollection

        Private _softPaqs As List(Of HPSoftPaq)

        <XmlElement("SoftPaq")> _
        Public Property SoftPaqs() As List(Of HPSoftPaq)
            Get
                Return _softPaqs
            End Get
            Set(ByVal value As List(Of HPSoftPaq))
                _softPaqs = value
            End Set
        End Property

    End Class

    Public Class HPSoftPaq

        Private _softPaqId As String
        Private _softPaqName As String
        Private _softPaqVersion As String
        Private _softPaqCategory As String
        Private _softPaqReleaseDate As String
        Private _softPaqUri As String
        Private _softPaqSize As Long
        Private _softPaqMd5Hash As String
        Private _softPaqSha256Hash As String
        Private _softPaqCvaFileUri As String
        Private _softPaqRelNotesUri As String
        Private _softPaqCvaTitle As String
        Private _softPaqHsaCompliant As Boolean

        <XmlElement("Id")> _
        Public Property Id() As String
            Get
                Return _softPaqId
            End Get
            Set(ByVal value As String)
                _softPaqId = value
            End Set
        End Property

        <XmlElement("Name")> _
        Public Property Name() As String
            Get
                Return _softPaqName
            End Get
            Set(ByVal value As String)
                _softPaqName = value
            End Set
        End Property

        <XmlElement("Version")> _
        Public Property Version() As String
            Get
                Return _softPaqVersion
            End Get
            Set(ByVal value As String)
                _softPaqVersion = value
            End Set
        End Property

        <XmlElement("Category")> _
        Public Property Category() As String
            Get
                Return _softPaqCategory
            End Get
            Set(ByVal value As String)
                _softPaqCategory = value
            End Set
        End Property

        <XmlElement("DateReleased")> _
        Public Property ReleaseDate() As String
            Get
                Return _softPaqReleaseDate
            End Get
            Set(ByVal value As String)
                _softPaqReleaseDate = value
            End Set
        End Property

        <XmlElement("Url")> _
        Public Property Uri() As String
            Get
                Return _softPaqUri
            End Get
            Set(ByVal value As String)
                _softPaqUri = value
            End Set
        End Property

        <XmlElement("Size")> _
        Public Property Size() As Long
            Get
                Return _softPaqSize
            End Get
            Set(ByVal value As Long)
                _softPaqSize = value
            End Set
        End Property

        <XmlElement("MD5")> _
        Public Property MD5() As String
            Get
                Return _softPaqMd5Hash
            End Get
            Set(ByVal value As String)
                _softPaqMd5Hash = value
            End Set
        End Property

        <XmlElement("SHA256")> _
        Public Property SHA256() As String
            Get
                Return _softPaqSha256Hash
            End Get
            Set(ByVal value As String)
                _softPaqSha256Hash = value
            End Set
        End Property

        <XmlElement("CvaFileUrl")> _
        Public Property CVAFileUri() As String
            Get
                Return _softPaqCvaFileUri
            End Get
            Set(ByVal value As String)
                _softPaqCvaFileUri = value
            End Set
        End Property

        <XmlElement("ReleaseNotesUrl")> _
        Public Property ReleaseNotesUri() As String
            Get
                Return _softPaqRelNotesUri
            End Get
            Set(ByVal value As String)
                _softPaqRelNotesUri = value
            End Set
        End Property

        <XmlElement("CvaTitle")> _
        Public Property CVATitle() As String
            Get
                Return _softPaqCvaTitle
            End Get
            Set(ByVal value As String)
                _softPaqCvaTitle = value
            End Set
        End Property

        <XmlElement("HSACompliant")> _
        Public Property HSACompliant() As Boolean
            Get
                Return _softPaqHsaCompliant
            End Get
            Set(ByVal value As Boolean)
                _softPaqHsaCompliant = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("ID {0} -- {1}", Id, Name)
        End Function

    End Class

End Namespace