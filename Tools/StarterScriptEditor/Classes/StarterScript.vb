Namespace Classes

    Public Class StarterScript

#If VBC_VER < 10.0 Then
        Private scriptName As String
#End If

        Public Property Name() As String
#If VBC_VER < 10.0 Then
            Get
                Return scriptName
            End Get
            Set(ByVal value As String)
                scriptName = value
            End Set
        End Property
#End If

#If VBC_VER < 10.0 Then
        Private scriptDescription As String
#End If

        Public Property Description() As String
#If VBC_VER < 10.0 Then
            Get
                Return scriptDescription
            End Get
            Set(ByVal value As String)
                scriptDescription = value
            End Set
        End Property
#End If

#If VBC_VER < 10.0 Then
        Private scriptLanguage As String
#End If

        Public Property Language() As String
#If VBC_VER < 10.0 Then
            Get
                Return scriptLanguage
            End Get
            Set(ByVal value As String)
                scriptLanguage = value
            End Set
        End Property
#End If

#If VBC_VER < 10.0 Then
        Private scriptCode As String
#End If

        Public Property Code() As String
#If VBC_VER < 10.0 Then
            Get
                Return scriptCode
            End Get
            Set(ByVal value As String)
                scriptCode = value
            End Set
        End Property
#End If

        Public Sub New(ByVal scriptLanguage As String)
            Name = ""
            Description = ""
            Language = scriptLanguage
            Code = ""
        End Sub

        Public Sub New(ByVal scriptName As String, ByVal scriptDescription As String, ByVal scriptLanguage As String, ByVal scriptCode As String)
            Name = scriptName
            Description = scriptDescription
            Language = scriptLanguage
            Code = scriptCode
        End Sub

        Public Overrides Function ToString() As String
            Return "Starter Script. Name: " & Name & ". Description: " & Description & ". Language: " & Language
        End Function

    End Class

End Namespace
