Namespace Classes.ModelCatalogs.HP

    <XmlRoot("NewDataSet")> _
    Public Class HPDataSet

        Private _catalog As HPDriverPackCatalog

        <XmlElement("HPClientDriverPackCatalog")> _
        Public Property Catalog() As HPDriverPackCatalog
            Get
                Return _catalog
            End Get
            Set(ByVal value As HPDriverPackCatalog)
                _catalog = value
            End Set
        End Property

    End Class

End Namespace