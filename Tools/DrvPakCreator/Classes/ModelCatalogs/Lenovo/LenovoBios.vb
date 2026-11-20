Namespace Classes.ModelCatalogs.Lenovo

    Public Class LenovoBios
        Inherits LenovoDownload

        Private _version As String
        Private _image As String

        <XmlAttribute("version")> _
        Public Property Version() As String
            Get
                Return _version
            End Get
            Set(ByVal value As String)
                _version = value
            End Set
        End Property

        <XmlAttribute("image")> _
        Public Property Image() As String
            Get
                Return _image
            End Get
            Set(ByVal value As String)
                _image = value
            End Set
        End Property

    End Class

End Namespace