Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ControlChars

Namespace Elements

    <Serializable(), XmlRoot("Video")>
    Public Class Video

        <XmlAttribute("ID")>
        Public Property YT_ID As String

        <XmlAttribute("Name")>
        Public Property VideoName As String

        <XmlAttribute("Description")>
        Public Property VideoDesc As String

        Public Overrides Function ToString() As String
            Return "YouTube video with ID: " & Me.YT_ID & "; CW-specified video name: " & Quote & Me.VideoName & Quote & "; CW-specified video desc: " & Quote & Me.VideoDesc & Quote
        End Function

    End Class

End Namespace
