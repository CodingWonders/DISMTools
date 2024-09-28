Imports System.Xml.Serialization
Imports System.Xml
Imports System.IO

Namespace Elements

    <Serializable(), XmlRoot("Component")>
    Public Class Component

        <XmlAttribute("Id")>
        Public Property Id As String

        Public Property Passes As New List(Of Pass)

        Public Shared Function LoadItems(filePath As String) As List(Of Component)
            Dim componentList As New List(Of Component)
            Try
                Using fs As FileStream = New FileStream(filePath, FileMode.Open)
                    Dim xs As New XmlReaderSettings()
                    xs.IgnoreWhitespace = True
                    Using reader As XmlReader = XmlReader.Create(fs, xs)
                        While reader.Read()
                            If reader.NodeType = XmlNodeType.Element AndAlso reader.Name = "Component" Then
                                Dim sysComponent As New Component()
                                sysComponent.Id = reader.GetAttribute("Id")
                                Dim PassList As String = reader.GetAttribute("Passes")
                                Dim passListTemp As New List(Of String)
                                passListTemp = PassList.Split(",").ToList()

                                Dim knownPasses As New Dictionary(Of String, Boolean)
                                knownPasses.Add("offlineServicing", False)
                                knownPasses.Add("windowsPE", False)
                                knownPasses.Add("generalize", False)
                                knownPasses.Add("specialize", False)
                                knownPasses.Add("auditSystem", False)
                                knownPasses.Add("auditUser", False)
                                knownPasses.Add("oobeSystem", False)

                                For Each systemPass In knownPasses.Keys
                                    Dim sysPass As New Pass(systemPass)
                                    sysPass.Compatible = (passListTemp.Contains(systemPass))
                                    sysComponent.Passes.Add(sysPass)
                                Next
                                componentList.Add(sysComponent)
                            End If
                        End While
                    End Using
                End Using
                Return componentList
            Catch ex As Exception
                If Debugger.IsAttached Then Debugger.Break()
                Return Nothing
            End Try
            Return Nothing
        End Function

    End Class

End Namespace