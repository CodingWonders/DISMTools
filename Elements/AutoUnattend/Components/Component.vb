Imports System.Xml.Serialization
Imports System.Xml
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Namespace Elements

    <Serializable(), XmlRoot("Component")>
    Public Class Component

        <XmlAttribute("Id")>
        Public Property Id As String

        Public Property Pass As Pass

        Public Property Passes As New List(Of Pass)

        Public Property XmlData As String

        Public Sub New()

        End Sub

        Public Sub New(id As String, pass As Pass)
            Me.Id = id
            Me.Pass = pass
        End Sub

        Public Overrides Function ToString() As String
            Dim passList As String = ""
            If Me.Passes.Count > 0 Then
                For Each componentPass As Pass In Me.Passes
                    If passList <> "" Then
                        passList &= ", " & componentPass.Name
                    Else
                        passList = componentPass.Name
                    End If
                Next
            End If
            Return "Component, with ID: " & Me.Id & "; Pass: " & Pass.Name
        End Function

        Public Shared Function LoadItems(filePath As String) As List(Of Component)
            DynaLog.LogMessage("Preparing to load items from file " & Quote & filePath & Quote & "...")
            Dim componentList As New List(Of Component)
            Try
                Using fs As FileStream = New FileStream(filePath, FileMode.Open)
                    DynaLog.LogMessage("Creating XML reader...")
                    Dim xs As New XmlReaderSettings()
                    xs.IgnoreWhitespace = True
                    Using reader As XmlReader = XmlReader.Create(fs, xs)
                        While reader.Read()
                            If reader.NodeType = XmlNodeType.Element AndAlso reader.Name = "Component" Then
                                DynaLog.LogMessage("We are dealing with a component (XML node type is element and is " & Quote & "Component" & Quote & "). Parsing...")
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
                DynaLog.LogMessage("System component count: " & componentList.Count)
                Return componentList
            Catch ex As Exception
                DynaLog.LogMessage("Could not load items. Error message: " & ex.Message)
                If Debugger.IsAttached Then Debugger.Break()
                Return Nothing
            End Try
            Return Nothing
        End Function

    End Class

End Namespace