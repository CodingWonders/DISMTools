Namespace Elements

    Public Class User

        Public Property Enabled As Boolean

        Public Property Name As String

        Public Property Password As String

        Public Property Group As UserGroup

    End Class

    Public Enum UserGroup As Integer
        Administrators = 0
        Users = 1
    End Enum

End Namespace
