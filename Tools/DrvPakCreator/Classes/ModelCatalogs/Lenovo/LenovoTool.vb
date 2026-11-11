Namespace Classes.ModelCatalogs.Lenovo

    Public Class LenovoToolCollection

        Private _tools As List(Of LenovoTool)

        <XmlElement("Tool")> _
        Public Property Tools() As List(Of LenovoTool)
            Get
                Return _tools
            End Get
            Set(ByVal value As List(Of LenovoTool))
                _tools = value
            End Set
        End Property

        Public Sub New()
            Tools = New List(Of LenovoTool)
        End Sub

    End Class

    Public Class LenovoTool

        Private _name As String
        Private _sha256Hash As String
        Private _version As String
        Private _uri As String

        <XmlAttribute("name")> _
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        <XmlAttribute("sha256")> _
        Public Property SHA256() As String
            Get
                Return _sha256Hash
            End Get
            Set(ByVal value As String)
                _sha256Hash = value
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
            Return String.Format("{0} -- {1}", Name, Version)
        End Function

    End Class

End Namespace