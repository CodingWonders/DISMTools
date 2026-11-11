Namespace Classes.ModelCatalogs.Lenovo

    Public Class LenovoGraphicsDriverPack
        Inherits LenovoDriverPack

        Private _brand As String

        <XmlAttribute("brand")> _
        Public Property Brand() As String
            Get
                Return _brand
            End Get
            Set(ByVal value As String)
                _brand = value
            End Set
        End Property

    End Class

End Namespace