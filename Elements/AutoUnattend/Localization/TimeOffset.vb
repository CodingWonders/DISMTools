Imports System.Xml.Serialization
Imports System.Xml
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Namespace Elements

    <Serializable(), XmlRoot("TimeOffset")>
    Public Class TimeOffset

        ''' <summary>
        ''' The ID of the time offset
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlAttribute("Id")>
        Public Property Id As String

        ''' <summary>
        ''' The display name of the time offset
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlAttribute("DisplayName")>
        Public Property DisplayName As String

        Public Overrides Function ToString() As String
            Return "Time offset, with ID: " & Me.Id & "; Display name: " & Me.DisplayName
        End Function

        Public Shared Function LoadItems(filePath As String) As List(Of TimeOffset)
            DynaLog.LogMessage("Preparing to load items from file " & Quote & filePath & Quote & "...")
            Dim offsetList As New List(Of TimeOffset)
            Try
                Using fs As FileStream = New FileStream(filePath, FileMode.Open)
                    Dim xs As New XmlReaderSettings()
                    xs.IgnoreWhitespace = True
                    Using reader As XmlReader = XmlReader.Create(fs, xs)
                        While reader.Read()
                            If reader.NodeType = XmlNodeType.Element AndAlso reader.Name = "TimeOffset" Then
                                DynaLog.LogMessage("We are dealing with a component (XML node type is element and is " & Quote & "TimeOffset" & Quote & "). Parsing...")
                                Dim offset As New TimeOffset()
                                offset.Id = reader.GetAttribute("Id")
                                offset.DisplayName = reader.GetAttribute("DisplayName")
                                offsetList.Add(offset)
                            End If
                        End While
                    End Using
                End Using
                DynaLog.LogMessage("Time offset count: " & offsetList.Count)
                Return offsetList
            Catch ex As Exception
                DynaLog.LogMessage("Could not load items. Error message: " & ex.Message)
                If Debugger.IsAttached Then Debugger.Break()
                Return Nothing
            End Try
            Return Nothing
        End Function
    End Class

End Namespace
