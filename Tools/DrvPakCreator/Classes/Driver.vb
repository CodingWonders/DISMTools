Imports System.Xml.Serialization

Namespace Classes

    Public Class DriverCollection

        Private _drivers As List(Of Driver)
        <XmlElement("driver")> _
        Public Property Drivers() As List(Of Driver)
            Get
                Return _drivers
            End Get
            Set(ByVal value As List(Of Driver))
                _drivers = value
            End Set
        End Property

        Public Sub New()
            Drivers = New List(Of Driver)
        End Sub

    End Class

    Public Class Driver

        Private _drvId As Integer
        Private _drvName As String
        Private _drvManufacturer As String
        Private _drvMainFile As String
        Private _drvSubFiles As List(Of String)

        <XmlAttribute("ID")> _
        Public Property Id() As Integer
            Get
                Return _drvId
            End Get
            Set(ByVal value As Integer)
                _drvId = value
            End Set
        End Property

        <XmlElement("name")> _
        Public Property Name() As String
            Get
                Return _drvName
            End Get
            Set(ByVal value As String)
                _drvName = value
            End Set
        End Property

        <XmlElement("manufacturer")> _
        Public Property Manufacturer() As String
            Get
                Return _drvManufacturer
            End Get
            Set(ByVal value As String)
                _drvManufacturer = value
            End Set
        End Property

        <XmlElement("mainFile")> _
        Public Property MainFile() As String
            Get
                Return _drvMainFile
            End Get
            Set(ByVal value As String)
                _drvMainFile = value
            End Set
        End Property

        <XmlArray("subDrivers"), XmlArrayItem("subDriver")> _
        Public Property SubFiles() As List(Of String)
            Get
                Return _drvSubFiles
            End Get
            Set(ByVal value As List(Of String))
                _drvSubFiles = value
            End Set
        End Property

        Public Sub New()
            Id = 0
            Name = ""
            Manufacturer = ""
            MainFile = ""
            SubFiles = New List(Of String)
        End Sub

        Public Function Clone() As Driver
            Dim clonedDriver As Driver = DirectCast(Me.MemberwiseClone(), Driver)
            clonedDriver.SubFiles = New List(Of String)(Me.SubFiles)
            Return clonedDriver
        End Function

    End Class

End Namespace