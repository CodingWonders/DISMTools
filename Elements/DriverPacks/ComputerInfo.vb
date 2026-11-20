Imports System.Xml.Serialization

Namespace Elements.DriverPacks

    Public Class ComputerInfo

        Private _compManufacturer As String
        Private _compModel As String
        Private _processorString As String
        Private _memoryCapacity As Long
        Private _storageCapacity As Long
        Private _systemUuid As String
        Private _osInfo As OperatingSystemInfo

        <XmlElement("manufacturer")> _
        Public Property Manufacturer() As String
            Get
                Return _compManufacturer
            End Get
            Set(ByVal value As String)
                _compManufacturer = value
            End Set
        End Property

        <XmlElement("model")> _
        Public Property Model() As String
            Get
                Return _compModel
            End Get
            Set(ByVal value As String)
                _compModel = value
            End Set
        End Property

        <XmlElement("processor")> _
        Public Property Processor() As String
            Get
                Return _processorString
            End Get
            Set(ByVal value As String)
                _processorString = value
            End Set
        End Property

        <XmlElement("memory")> _
        Public Property MemoryCapacity() As ULong
            Get
                Return _memoryCapacity
            End Get
            Set(ByVal value As ULong)
                _memoryCapacity = value
            End Set
        End Property

        <XmlElement("storage")> _
        Public Property StorageCapacity() As ULong
            Get
                Return _storageCapacity
            End Get
            Set(ByVal value As ULong)
                _storageCapacity = value
            End Set
        End Property

        <XmlElement("uuid")> _
        Public Property Uuid() As Guid
            Get
                Try
                    Return IIf(String.IsNullOrEmpty(_systemUuid), Guid.Empty, New Guid(_systemUuid))
                Catch ex As Exception
                    Return Guid.Empty
                End Try
            End Get
            Set(ByVal value As Guid)
                Try
                    _systemUuid = value.ToString()
                Catch ex As Exception
                    _systemUuid = Guid.Empty.ToString()
                End Try
            End Set
        End Property

        <XmlElement("os")> _
        Public Property OperatingSystem() As OperatingSystemInfo
            Get
                Return _osInfo
            End Get
            Set(ByVal value As OperatingSystemInfo)
                _osInfo = value
            End Set
        End Property

        Public Sub New()
            Manufacturer = ""
            Model = ""
            Processor = ""
            MemoryCapacity = 128UL
            StorageCapacity = 1024UL
            Uuid = Guid.Empty
            OperatingSystem = New OperatingSystemInfo()
        End Sub

    End Class

    Public Class OperatingSystemInfo

        Private _osName As String
        Private _osSku As String
        Private _osVersion As String

        <XmlElement("name")> _
        Public Property Name() As String
            Get
                Return _osName
            End Get
            Set(ByVal value As String)
                _osName = value
            End Set
        End Property

        <XmlElement("sku")> _
        Public Property SKU() As String
            Get
                Return _osSku
            End Get
            Set(ByVal value As String)
                _osSku = value
            End Set
        End Property

        <XmlElement("verstring")> _
        Public Property VersionString() As String
            Get
                Return _osVersion
            End Get
            Set(ByVal value As String)
                _osVersion = value
            End Set
        End Property

        Public ReadOnly Property Version() As Version
            Get
                Return New Version(VersionString)
            End Get
        End Property

        Public Sub New()
            Name = "Microsoft Windows"
            SKU = ""
            VersionString = "10.0.26200.6584"
        End Sub

    End Class

End Namespace