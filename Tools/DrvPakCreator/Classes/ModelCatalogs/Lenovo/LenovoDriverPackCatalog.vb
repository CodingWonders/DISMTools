Namespace Classes.ModelCatalogs.Lenovo

    <XmlRoot("ModelList")> _
    Public Class LenovoDriverPackCatalog
        Inherits ModelCatalog

        Public Overrides ReadOnly Property Manufacturer() As String
            Get
                Return "Lenovo"
            End Get
        End Property

        Private _version As String
        Private _models As List(Of LenovoModel)
        Private _tools As LenovoToolCollection

        <XmlAttribute("version")> _
        Public Property Version() As String
            Get
                Return _version
            End Get
            Set(ByVal value As String)
                _version = value
            End Set
        End Property

        <XmlElement("Model")> _
        Public Property Models() As List(Of LenovoModel)
            Get
                Return _models
            End Get
            Set(ByVal value As List(Of LenovoModel))
                _models = value
            End Set
        End Property

        <XmlElement("ToolList")> _
        Public Property Tools() As LenovoToolCollection
            Get
                Return _tools
            End Get
            Set(ByVal value As LenovoToolCollection)
                _tools = value
            End Set
        End Property

        Public Sub New()
            Models = New List(Of LenovoModel)
        End Sub

    End Class

End Namespace