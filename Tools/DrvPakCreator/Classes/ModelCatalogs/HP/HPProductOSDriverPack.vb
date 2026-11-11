Namespace Classes.ModelCatalogs.HP

    Public Class HPProductOSDriverPackCollection

        Private _products As List(Of HPProductOSDriverPack)

        <XmlElement("ProductOSDriverPack")> _
        Public Property Products() As List(Of HPProductOSDriverPack)
            Get
                Return _products
            End Get
            Set(ByVal value As List(Of HPProductOSDriverPack))
                _products = value
            End Set
        End Property

    End Class

    Public Class HPProductOSDriverPack

        Private _productArchitecture As String
        Private _productType As String
        Private _productSystemId As String
        Private _productSystemName As String
        Private _productOSName As String
        Private _productOSId As Integer
        Private _productSoftPaqId As String
        Private _productId As Integer

        <XmlElement("Architecture")> _
        Public Property Architecture() As String
            Get
                Return _productArchitecture
            End Get
            Set(ByVal value As String)
                _productArchitecture = value
            End Set
        End Property

        <XmlElement("ProductType")> _
        Public Property ProductType() As String
            Get
                Return _productType
            End Get
            Set(ByVal value As String)
                _productType = value
            End Set
        End Property

        <XmlElement("SystemId")> _
        Public Property SystemId() As String
            Get
                Return _productSystemId
            End Get
            Set(ByVal value As String)
                _productSystemId = value
            End Set
        End Property

        <XmlElement("SystemName")> _
        Public Property SystemName() As String
            Get
                Return _productSystemName
            End Get
            Set(ByVal value As String)
                _productSystemName = value
            End Set
        End Property

        <XmlElement("OSName")> _
        Public Property OSName() As String
            Get
                Return _productOSName
            End Get
            Set(ByVal value As String)
                _productOSName = value
            End Set
        End Property

        <XmlElement("OSId")> _
        Public Property OSId() As Integer
            Get
                Return _productOSId
            End Get
            Set(ByVal value As Integer)
                _productOSId = value
            End Set
        End Property

        <XmlElement("SoftPaqId")> _
        Public Property SoftPaqId() As String
            Get
                Return _productSoftPaqId
            End Get
            Set(ByVal value As String)
                _productSoftPaqId = value
            End Set
        End Property

        <XmlElement("ProductId")> _
        Public Property Id() As Integer
            Get
                Return _productId
            End Get
            Set(ByVal value As Integer)
                _productId = value
            End Set
        End Property

    End Class

End Namespace