Public Class StarterScript

    Public Property Name As String
    Public Property Description As String
    Public Property Language As String
    Public Property ScriptCode As String

    Public Sub New(name As String, description As String, language As String, scriptCode As String)
        Me.Name = name
        Me.Description = description
        Me.Language = language
        Me.ScriptCode = scriptCode
    End Sub

    Public Overrides Function ToString() As String
        Return "Starter Script. Name: " & Name & ". Description: " & Description & ". Language: " & Language
    End Function

End Class
