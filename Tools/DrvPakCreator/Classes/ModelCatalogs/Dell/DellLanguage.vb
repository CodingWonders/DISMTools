Namespace Classes.ModelCatalogs.Dell

    <XmlType(Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    Public Class DellLanguageCollection

        Private _languages As List(Of DellLanguage)

        <XmlElement("Language")> _
        Public Property Languages() As List(Of DellLanguage)
            Get
                Return _languages
            End Get
            Set(ByVal value As List(Of DellLanguage))
                _languages = value
            End Set
        End Property

        Public Sub New()
            Languages = New List(Of DellLanguage)
        End Sub

    End Class

    <XmlType(Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    Public Class DellLanguage

        Private _code As String
        Private _name As String

        <XmlAttribute("lang")> _
        Public Property Code() As String
            Get
                Return _code
            End Get
            Set(ByVal value As String)
                _code = value
            End Set
        End Property

        <XmlText()> _
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("{0} -- {1}", Code, Name)
        End Function

    End Class

End Namespace