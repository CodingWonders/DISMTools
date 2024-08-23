Imports System.Xml.Serialization
Imports System.Xml
Imports System.IO

Namespace Elements

    <Serializable(), XmlRoot("ImageLanguage")>
    Public Class ImageLanguage

        ''' <summary>
        ''' The ID of the image language
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlAttribute("Id")>
        Public Property Id As String

        ''' <summary>
        ''' The display name of the image language
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlAttribute("DisplayName")>
        Public Property DisplayName As String

        Public Shared Function LoadItems(filePath As String) As List(Of ImageLanguage)
            Dim langList As New List(Of ImageLanguage)
            Try
                Using fs As FileStream = New FileStream(filePath, FileMode.Open)
                    Dim xs As New XmlReaderSettings()
                    xs.IgnoreWhitespace = True
                    Using reader As XmlReader = XmlReader.Create(fs, xs)
                        While reader.Read()
                            If reader.NodeType = XmlNodeType.Element AndAlso reader.Name = "ImageLanguage" Then
                                Dim imgLang As New ImageLanguage()
                                imgLang.Id = reader.GetAttribute("Id")
                                imgLang.DisplayName = reader.GetAttribute("DisplayName")
                                langList.Add(imgLang)
                            End If
                        End While
                    End Using
                End Using
                Return langList
            Catch ex As Exception
                If Debugger.IsAttached Then Debugger.Break()
                Return Nothing
            End Try
            Return Nothing
        End Function
    End Class

End Namespace
