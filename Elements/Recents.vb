Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ControlChars

Namespace Elements

    <Serializable(), XmlRoot("ArrayOfRecents")>
    Public Class Recents
        <XmlAttribute("Path")>
        Public Property ProjPath As String

        <XmlAttribute("Name")>
        Public Property ProjName As String

        <XmlAttribute("Order")>
        Public Property Order As Integer

        Public Overrides Function ToString() As String
            Return "Project, with path: " & Quote & Me.ProjPath & Quote & "; with name: " & Quote & Me.ProjName & Quote & "; with order: " & Me.Order
        End Function
    End Class

End Namespace
