Imports System.Xml.Serialization
Imports System.Xml
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

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

        Public Overrides Function ToString() As String
            Return "Keyboard, with ID: " & Me.Id & "; Display name: " & Me.DisplayName & "; Type: " & Me.Type
        End Function

        Public Shared Function LoadItems(filePath As String) As List(Of KeyboardIdentifier)
            DynaLog.LogMessage("Preparing to load items from file " & Quote & filePath & Quote & "...")
            Dim keyboardList As New List(Of KeyboardIdentifier)
            Try
                Using fs As FileStream = New FileStream(filePath, FileMode.Open)
                    DynaLog.LogMessage("Creating XML reader...")
                    Dim xs As New XmlReaderSettings()
                    xs.IgnoreWhitespace = True
                    Using reader As XmlReader = XmlReader.Create(fs, xs)
                        While reader.Read()
                            If reader.NodeType = XmlNodeType.Element AndAlso reader.Name = "KeyboardIdentifier" Then
                                DynaLog.LogMessage("We are dealing with a component (XML node type is element and is " & Quote & "KeyboardIdentifier" & Quote & "). Parsing...")
                                Dim keyboard As New KeyboardIdentifier()
                                keyboard.Id = reader.GetAttribute("Id")
                                keyboard.DisplayName = reader.GetAttribute("DisplayName")
                                keyboard.Type = reader.GetAttribute("Type")
                                keyboardList.Add(keyboard)
                            End If
                        End While
                    End Using
                End Using
                DynaLog.LogMessage("Keyboard count: " & keyboardList.Count)
                Return keyboardList
            Catch ex As Exception
                DynaLog.LogMessage("Could not load items. Error message: " & ex.Message)
                If Debugger.IsAttached Then Debugger.Break()
                Return Nothing
            End Try
            Return Nothing
        End Function
    End Class

End Namespace
