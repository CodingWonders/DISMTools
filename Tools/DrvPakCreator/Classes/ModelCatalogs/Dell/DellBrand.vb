Namespace Classes.ModelCatalogs.Dell

    <XmlType(Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    Public Class DellBrandCollection

        Private _brands As List(Of DellBrand)

        <XmlElement("Brand")> _
        Public Property Brands() As List(Of DellBrand)
            Get
                Return _brands
            End Get
            Set(ByVal value As List(Of DellBrand))
                _brands = value
            End Set
        End Property

        Public Sub New()
            Brands = New List(Of DellBrand)
        End Sub

    End Class

    <XmlType(Namespace:=DellDriverPackCatalog.XmlNamespace)> _
    Public Class DellBrand

        Private _id As Integer
        Private _prefix As String
        Private _display As DellDisplay
        Private _models As List(Of DellModel)

        <XmlAttribute("key")> _
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        <XmlAttribute("prefix")> _
        Public Property Prefix() As String
            Get
                Return _prefix
            End Get
            Set(ByVal value As String)
                _prefix = value
            End Set
        End Property

        <XmlElement("Display")> _
        Public Property Display() As DellDisplay
            Get
                Return _display
            End Get
            Set(ByVal value As DellDisplay)
                _display = value
            End Set
        End Property

        <XmlElement("Model")> _
        Public Property Models() As List(Of DellModel)
            Get
                Return _models
            End Get
            Set(ByVal value As List(Of DellModel))
                _models = value
            End Set
        End Property

        Public Sub New()
            Models = New List(Of DellModel)
        End Sub

        Public Overrides Function ToString() As String
            If Display Is Nothing Then
                Return Prefix
            End If
            Return String.Format("{0} -- {1}", Prefix, Display.ToString())
        End Function

    End Class

End Namespace