Namespace Classes.ModelCatalogs.Dell

    <XmlType(Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    Public Class DellImportantInfo
        Inherits DellLocalizedText

        Private _uri As String

        <XmlAttribute("URL")> _
        Public Property Uri() As String
            Get
                Return _uri
            End Get
            Set(ByVal value As String)
                _uri = value
            End Set
        End Property

    End Class

End Namespace