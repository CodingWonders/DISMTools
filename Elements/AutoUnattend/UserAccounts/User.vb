Namespace Elements

    Public Class User

        ''' <summary>
        ''' Determines whether the account can be added
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Enabled As Boolean

        ''' <summary>
        ''' The name of the account
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Account names must not exceed 20 characters</remarks>
        Public Property Name As String

        ''' <summary>
        ''' The password of the account
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Password As String

        ''' <summary>
        ''' The group of the user account
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>See enum for more information</remarks>
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
