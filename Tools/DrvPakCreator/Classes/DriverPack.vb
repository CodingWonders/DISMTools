Imports System.Xml.Serialization

Namespace Classes

    <XmlRoot("DriverPackMetadata")> _
    Public Class DriverPack

        Private _drvpakName As String
        Private _drvpakDescription As String
        Private _drvpakAuthor As String

        Private _drvPakComputers As ComputerCollection
        Private _drvPakDriverFiles As DriverCollection

        <XmlElement("name")> _
        Public Property Name() As String
            Get
                Return _drvpakName
            End Get
            Set(ByVal value As String)
                _drvpakName = value
            End Set
        End Property

        <XmlElement("description")> _
        Public Property Description() As String
            Get
                Return _drvpakDescription
            End Get
            Set(ByVal value As String)
                _drvpakDescription = value
            End Set
        End Property

        <XmlElement("author")> _
        Public Property Author() As String
            Get
                Return _drvpakAuthor
            End Get
            Set(ByVal value As String)
                _drvpakAuthor = value
            End Set
        End Property

        <XmlElement("computers")> _
        Public Property Computers() As ComputerCollection
            Get
                Return _drvPakComputers
            End Get
            Set(ByVal value As ComputerCollection)
                _drvPakComputers = value
            End Set
        End Property

        <XmlElement("drivers")> _
        Public Property DriverFiles() As DriverCollection
            Get
                Return _drvPakDriverFiles
            End Get
            Set(ByVal value As DriverCollection)
                _drvPakDriverFiles = value
            End Set
        End Property

        Public Sub New()
            Name = ""
            Description = ""
            Author = ""
            Computers = New ComputerCollection()
            DriverFiles = New DriverCollection()
        End Sub

    End Class

End Namespace