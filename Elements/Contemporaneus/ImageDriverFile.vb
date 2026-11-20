Imports Microsoft.Dism

Namespace Elements.Contemporaneus

    Public Class ImageDriverFile

        Public Property FilePath As String
        Public Property HardwareTargets As List(Of DismDriver)

        Public Sub New()
            HardwareTargets = New List(Of DismDriver)
        End Sub

        Public Sub New(filePath As String)
            Me.New()
            Me.FilePath = filePath
        End Sub

    End Class

End Namespace
