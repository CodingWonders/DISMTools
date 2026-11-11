Namespace Classes.ModelCatalogs.Lenovo

    Public Class LenovoDriverPack
        Inherits LenovoDownload

        Private _osName As String
        Private _osVersion As String

        <XmlAttribute("os")> _
        Public Property OSName() As String
            Get
                Return _osName
            End Get
            Set(ByVal value As String)
                _osName = value
            End Set
        End Property

        <XmlAttribute("version")> _
        Public Property OSVersion() As String
            Get
                Return _osVersion
            End Get
            Set(ByVal value As String)
                _osVersion = value
            End Set
        End Property

    End Class

End Namespace