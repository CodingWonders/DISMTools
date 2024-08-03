Namespace Elements

    Public Class User

        Public Property Enabled As Boolean

        Public Property Name As String

        Public Property Password As String

        Public Property Group As UserGroup

        Public Sub New(enabled As Boolean, name As String, password As String, group As UserGroup)
            Me.Enabled = enabled
            Me.Name = name
            Me.Password = password
            Me.Group = group
        End Sub

    End Class

    Public Enum UserGroup As Integer
        Administrators = 0
        Users = 1
    End Enum

    Public Class UserValidator

        Public Shared Function ValidateUsers(userList As List(Of User)) As Boolean
            If userList Is Nothing OrElse userList.Count = 0 Then
                Return False
            End If
            ' Assume it's true by default
            Dim FullyValid As Boolean = True
            For Each listedUser As User In userList
                If listedUser.Enabled Then
                    If listedUser.Name = "" OrElse listedUser.Name = "Administrator" Then
                        FullyValid = False
                        Exit For
                    Else
                        FullyValid = True
                    End If
                End If
            Next
            Return FullyValid
        End Function

    End Class

End Namespace
