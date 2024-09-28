Namespace Elements

    Public Class Pass

        Public Property Name As String

        Public Property Compatible As Boolean

        Public Property Enabled As Boolean

        Public Sub New(passName As String)
            Me.Name = passName
            Me.Compatible = False
        End Sub

    End Class

End Namespace