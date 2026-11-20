Imports System.Xml.Serialization

Namespace Elements.DriverPacks

    Public Class ComputerCollection

        Private _computers As List(Of Computer)
        <XmlElement("computer")> _
        Public Property Computers() As List(Of Computer)
            Get
                Return _computers
            End Get
            Set(ByVal value As List(Of Computer))
                _computers = value
            End Set
        End Property

        Public Sub New()
            Computers = New List(Of Computer)
        End Sub

    End Class

    Public Class Computer

        Private _compId As String
        Private _compInfo As ComputerInfo
        Private _driverIds As List(Of Integer)

        <XmlAttribute("ID")> _
        Public Property Id() As String
            Get
                Return _compId
            End Get
            Set(ByVal value As String)
                _compId = value
            End Set
        End Property

        <XmlElement("compinfo")> _
        Public Property ComputerInformation() As ComputerInfo
            Get
                Return _compInfo
            End Get
            Set(ByVal value As ComputerInfo)
                _compInfo = value
            End Set
        End Property

        <XmlArray("drivers"), XmlArrayItem("driver")> _
        Public Property AssociatedDriverIds() As List(Of Integer)
            Get
                Return _driverIds
            End Get
            Set(ByVal value As List(Of Integer))
                _driverIds = value
            End Set
        End Property

        Public Sub New()
            Id = 0
            ComputerInformation = New ComputerInfo()
            AssociatedDriverIds = New List(Of Integer)
        End Sub

    End Class

End Namespace