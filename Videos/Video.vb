Imports System.Xml.Serialization

Namespace Elements

    <Serializable(), XmlRoot("Video")>
    Public Class Video

        <XmlAttribute("ID")>
        Public Property YT_ID As String

        <XmlAttribute("Name")>
        Public Property VideoName As String

        <XmlAttribute("Description")>
        Public Property VideoDesc As String

    End Class

End Namespace
