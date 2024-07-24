Imports System.Xml.Serialization
Imports System.Xml
Imports System.IO

Namespace Elements

    <Serializable(), XmlRoot("TimeOffset")>
    Public Class TimeOffset

        <XmlAttribute("Id")>
        Public Property Id As String

        <XmlAttribute("DisplayName")>
        Public Property DisplayName As String

        Public Shared Function LoadItems(filePath As String) As List(Of TimeOffset)
            Dim offsetList As New List(Of TimeOffset)
            Try
                Using fs As FileStream = New FileStream(filePath, FileMode.Open)
                    Dim xs As New XmlReaderSettings()
                    xs.IgnoreWhitespace = True
                    Using reader As XmlReader = XmlReader.Create(fs, xs)
                        While reader.Read()
                            If reader.NodeType = XmlNodeType.Element AndAlso reader.Name = "TimeOffset" Then
                                Dim offset As New TimeOffset()
                                offset.Id = reader.GetAttribute("Id")
                                offset.DisplayName = reader.GetAttribute("DisplayName")
                                offsetList.Add(offset)
                            End If
                        End While
                    End Using
                End Using
                Return offsetList
            Catch ex As Exception
                If Debugger.IsAttached Then Debugger.Break()
                Return Nothing
            End Try
            Return Nothing
        End Function
    End Class

End Namespace
