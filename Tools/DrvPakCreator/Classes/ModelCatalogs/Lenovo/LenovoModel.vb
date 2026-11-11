Namespace Classes.ModelCatalogs.Lenovo

    Public Class LenovoMachineTypeCollection

        Private _machineTypes As List(Of String)

        <XmlElement("Type")> _
        Public Property MachineTypes() As List(Of String)
            Get
                Return _machineTypes
            End Get
            Set(ByVal value As List(Of String))
                _machineTypes = value
            End Set
        End Property

        Public Sub New()
            MachineTypes = New List(Of String)
        End Sub

    End Class

    Public Class LenovoModel

        Private _name As String
        Private _architecture As String
        Private _machineTypes As LenovoMachineTypeCollection
        Private _bios As LenovoBios
        Private _sccmDriverPacks As List(Of LenovoDriverPack)
        Private _hsaDriverPacks As List(Of LenovoDriverPack)
        Private _graphicsDriverPacks As List(Of LenovoGraphicsDriverPack)

        <XmlAttribute("name")> _
        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        <XmlAttribute("arch")> _
        Public Property Architecture() As String
            Get
                Return _architecture
            End Get
            Set(ByVal value As String)
                _architecture = value
            End Set
        End Property

        <XmlElement("Types")> _
        Public Property MachineTypes() As LenovoMachineTypeCollection
            Get
                Return _machineTypes
            End Get
            Set(ByVal value As LenovoMachineTypeCollection)
                _machineTypes = value
            End Set
        End Property

        <XmlElement("BIOS")> _
        Public Property Bios() As LenovoBios
            Get
                Return _bios
            End Get
            Set(ByVal value As LenovoBios)
                _bios = value
            End Set
        End Property

        <XmlElement("SCCM")> _
        Public Property SCCMDriverPacks() As List(Of LenovoDriverPack)
            Get
                Return _sccmDriverPacks
            End Get
            Set(ByVal value As List(Of LenovoDriverPack))
                _sccmDriverPacks = value
            End Set
        End Property

        <XmlElement("HSA")> _
        Public Property HSADriverPacks() As List(Of LenovoDriverPack)
            Get
                Return _hsaDriverPacks
            End Get
            Set(ByVal value As List(Of LenovoDriverPack))
                _hsaDriverPacks = value
            End Set
        End Property

        <XmlElement("GFX")> _
        Public Property GraphicsDriverPacks() As List(Of LenovoGraphicsDriverPack)
            Get
                Return _graphicsDriverPacks
            End Get
            Set(ByVal value As List(Of LenovoGraphicsDriverPack))
                _graphicsDriverPacks = value
            End Set
        End Property

        Public Sub New()
            SCCMDriverPacks = New List(Of LenovoDriverPack)
            HSADriverPacks = New List(Of LenovoDriverPack)
            GraphicsDriverPacks = New List(Of LenovoGraphicsDriverPack)
        End Sub

        Public Overrides Function ToString() As String
            Return Name
        End Function

    End Class

End Namespace