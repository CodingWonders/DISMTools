Imports System.Xml.Serialization
Imports System.Xml
Imports System.IO

Namespace Elements

    <Serializable(), XmlRoot("KeyboardIdentifier")>
    Public Class KeyboardIdentifier

        ''' <summary>
        ''' The ID of the keyboard identifier
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlAttribute("Id")>
        Public Property Id As String

        ''' <summary>
        ''' The display name of the keyboard identifier
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlAttribute("DisplayName")>
        Public Property DisplayName As String

        ''' <summary>
        ''' The type of the keyboard identifier
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Possible values: Keyboard, IME</remarks>
        <XmlAttribute("Type")>
        Public Property Type As String

        Public Shared Function LoadItems(filePath As String) As List(Of KeyboardIdentifier)
            Dim keyboardList As New List(Of KeyboardIdentifier)
            Try
                Using fs As FileStream = New FileStream(filePath, FileMode.Open)
                    Dim xs As New XmlReaderSettings()
                    xs.IgnoreWhitespace = True
                    Using reader As XmlReader = XmlReader.Create(fs, xs)
                        While reader.Read()
                            If reader.NodeType = XmlNodeType.Element AndAlso reader.Name = "KeyboardIdentifier" Then
                                Dim keyboard As New KeyboardIdentifier()
                                keyboard.Id = reader.GetAttribute("Id")
                                keyboard.DisplayName = reader.GetAttribute("DisplayName")
                                keyboard.Type = reader.GetAttribute("Type")
                                keyboardList.Add(keyboard)
                            End If
                        End While
                    End Using
                End Using
                Return keyboardList
            Catch ex As Exception
                If Debugger.IsAttached Then Debugger.Break()
                Return Nothing
            End Try
            Return Nothing
        End Function
    End Class

End Namespace
