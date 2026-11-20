Namespace Classes.ModelCatalogs.Dell

    <XmlType(Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    Public Class DellDriverPackMetadata

        Private _size As Long
        Private _path As String
        Private _hashes As DellHashCollection

        <XmlElement("Size")> _
        Public Property Size() As Long
            Get
                Return _size
            End Get
            Set(ByVal value As Long)
                _size = value
            End Set
        End Property

        <XmlElement("path")> _
        Public Property Path() As String
            Get
                Return _path
            End Get
            Set(ByVal value As String)
                _path = value
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

    End Class

End Namespace