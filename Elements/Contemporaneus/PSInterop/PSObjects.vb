Imports System.Xml.Serialization

Namespace Elements.Contemporaneus.PSInterop

    <XmlRoot("Objects")>
    Public Class PsObjects
        <XmlElement("Object")>
        Public Property Items As List(Of PsObject)
    End Class

    Public Class PsObject
        <XmlElement("Property")>
        Public Property Properties As List(Of PsProperty)
    End Class

    Public Class PsProperty
        <XmlAttribute("Name")>
        Public Property Name As String

        <XmlText>
        Public Property Value As String
    End Class


End Namespace
