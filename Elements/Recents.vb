Imports System.Xml.Serialization

Namespace Elements

    <Serializable(), XmlRoot("ArrayOfRecents")>
    Public Class Recents
        <XmlAttribute("Path")>
        Public Property ProjPath As String

        <XmlAttribute("Name")>
        Public Property ProjName As String

        <XmlAttribute("Order")>
        Public Property Order As Integer
    End Class

End Namespace
