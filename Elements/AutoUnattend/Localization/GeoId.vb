Imports System.Xml.Serialization
Imports System.Xml
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Namespace Elements

    <Serializable(), XmlRoot("GeoId")>
    Public Class GeoId

        ''' <summary>
        ''' The ID of the home location
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlAttribute("Id")>
        Public Property Id As Integer

        ''' <summary>
        ''' The display name of the home location
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlAttribute("DisplayName")>
        Public Property DisplayName As String

        Public Overrides Function ToString() As String
            Return "GeoID, with ID: " & Me.Id & "; Display name: " & Me.DisplayName
        End Function

        Public Shared Function LoadItems(filePath As String) As List(Of GeoId)
            DynaLog.LogMessage("Preparing to load items from file " & Quote & filePath & Quote & "...")
            Dim GeoList As New List(Of GeoId)
            Try
                Using fs As FileStream = New FileStream(filePath, FileMode.Open)
                    DynaLog.LogMessage("Creating XML reader...")
                    Dim xs As New XmlReaderSettings()
                    xs.IgnoreWhitespace = True
                    Using reader As XmlReader = XmlReader.Create(fs, xs)
                        While reader.Read()
                            If reader.NodeType = XmlNodeType.Element AndAlso reader.Name = "GeoId" Then
                                DynaLog.LogMessage("We are dealing with a component (XML node type is element and is " & Quote & "GeoId" & Quote & "). Parsing...")
                                Dim Geo As New GeoId()
                                Geo.Id = reader.GetAttribute("Id")
                                Geo.DisplayName = reader.GetAttribute("DisplayName")
                                GeoList.Add(Geo)
                            End If
                        End While
                    End Using
                End Using
                DynaLog.LogMessage("GeoID count: " & GeoList.Count)
                Return GeoList
            Catch ex As Exception
                DynaLog.LogMessage("Could not load items. Error message: " & ex.Message)
                If Debugger.IsAttached Then Debugger.Break()
                Return Nothing
            End Try
            Return Nothing
        End Function
    End Class

End Namespace
