Imports System.Xml.Serialization

Namespace Elements.InfinityHome

    <XmlRoot("InfinityFacts")>
    Public Class InfinityFactsDocument

        <XmlArray("facts")>
        <XmlArrayItem("fact")>
        Public Property Facts As List(Of InfinityFact)

    End Class

    Public Class InfinityFact

        <XmlAttribute("id")>
        Public Property Id As String

        <XmlIgnore>
        Public Property Type As InfinityFactType

        <XmlAttribute("type")>
        Public Property XmlType As String
            Get
                Select Case Type
                    Case InfinityFactType.Historical
                        Return "historical"
                    Case InfinityFactType.WindowsTip
                        Return "tip"
                    Case InfinityFactType.DTTip
                        Return "dttip"
                    Case Else
                        Return ""
                End Select
            End Get
            Set(value As String)
                Select Case value.ToLowerInvariant()
                    Case "historical"
                        Type = InfinityFactType.Historical
                    Case "tip"
                        Type = InfinityFactType.WindowsTip
                    Case "dttip"
                        Type = InfinityFactType.DTTip
                End Select
            End Set
        End Property

        <XmlAttribute("era")>
        Public Property Era As String

        <XmlElement("topic")>
        Public Property Topic As String

        <XmlElement("text")>
        Public Property Message As String

    End Class

    Public Enum InfinityFactType
        Historical = 0
        WindowsTip = 1
        DTTip = 2
    End Enum

End Namespace