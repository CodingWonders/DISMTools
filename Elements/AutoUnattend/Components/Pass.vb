Namespace Elements

    Public Class Pass

        Public Property Name As String

        Public Property Compatible As Boolean

        Public Property Enabled As Boolean

        Public Sub New(passName As String)
            Me.Name = passName
            Me.Compatible = False
        End Sub

        Public Overrides Function Equals(obj As Object) As Boolean
            Return String.Equals(Name, TryCast(obj, Pass).Name, StringComparison.InvariantCultureIgnoreCase)
        End Function

    End Class

End Namespace