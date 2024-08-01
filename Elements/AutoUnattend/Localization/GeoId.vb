Imports System.Xml.Serialization
Imports System.Xml
Imports System.IO

Namespace Elements

    <Serializable(), XmlRoot("GeoId")>
    Public Class GeoId

        <XmlAttribute("Id")>
        Public Property Id As Integer

        <XmlAttribute("DisplayName")>
        Public Property DisplayName As String

        Public Shared Function LoadItems(filePath As String) As List(Of GeoId)
            Dim GeoList As New List(Of GeoId)
            Try
                Using fs As FileStream = New FileStream(filePath, FileMode.Open)
                    Dim xs As New XmlReaderSettings()
                    xs.IgnoreWhitespace = True
                    Using reader As XmlReader = XmlReader.Create(fs, xs)
                        While reader.Read()
                            If reader.NodeType = XmlNodeType.Element AndAlso reader.Name = "GeoId" Then
                                Dim Geo As New GeoId()
                                Geo.Id = reader.GetAttribute("Id")
                                Geo.DisplayName = reader.GetAttribute("DisplayName")
                                GeoList.Add(Geo)
                            End If
                        End While
                    End Using
                End Using
                Return GeoList
            Catch ex As Exception
                If Debugger.IsAttached Then Debugger.Break()
                Return Nothing
            End Try
            Return Nothing
        End Function
    End Class

End Namespace
