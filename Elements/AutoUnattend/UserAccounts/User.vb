Imports Microsoft.VisualBasic.ControlChars

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
        ''' The display name of the account
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DisplayName As String

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
            Me.DisplayName = name
            Me.Password = password
            Me.Group = group
        End Sub

        Public Sub New(enabled As Boolean, name As String, displayName As String, password As String, group As UserGroup)
            Me.Enabled = enabled
            Me.Name = name
            Me.DisplayName = displayName
            Me.Password = password
            Me.Group = group
        End Sub

        Public Overrides Function ToString() As String
            ' You thought that I was going to leak user passwords, hackers?
            Return "User, with name: " & Quote & Me.Name & Quote & "; Display Name: " & Quote & Me.DisplayName & Quote & "; Enabled: " & If(Me.Enabled, "Yes", "No") & "; Password: " & Quote & New String("*", Password.Length) & Quote & "; Group: " & Me.Group.ToString()
        End Function

    End Class

    Public Enum UserGroup As Integer
        Administrators = 0
        Users = 1
    End Enum

    Public Class UserValidationResults

        Public Property ValidationErrorReason As String
        Public Property IsValid As Boolean

        Public Sub New()

        End Sub

        Public Sub New(Valid As Boolean, Reason As String)
            Me.ValidationErrorReason = Reason
            Me.IsValid = Valid
        End Sub

        Public Overrides Function ToString() As String
            Return Me.ValidationErrorReason
        End Function

    End Class

    Public Class UserValidator

        Public Shared Function ValidateUsers(userList As List(Of User), Optional computerName As ComputerName = Nothing) As UserValidationResults
            DynaLog.LogMessage("Beginning validation of users in user list, with " & userList.Count & " user(s)...")
            DynaLog.LogMessage("- Computer name passed for extended validation: " & Quote & computerName.Name & Quote)
            Dim ProblematicAccountNumbers As New List(Of Integer)
            Dim validationResults As New UserValidationResults()
            DynaLog.LogMessage("Checking if list contains any users...")
            If userList Is Nothing OrElse userList.Count = 0 Then
                DynaLog.LogMessage("There are no users in the list. Perhaps it's the user that's being too smart, or an error occurred when grabbing user accounts.")
                Return New UserValidationResults(False, "Either no users have been specified, or a program error occurred.")
            End If
            Dim errorReason As String = ""
            Dim ExistingUsers() As String = New String(7) {"administrator", "guest", "defaultaccount", "system", "network service", "local service", "none", "wdagutilityaccount"}
            ' Assume it's true by default
            Dim FullyValid As Boolean = True
            For Each listedUser As User In userList
                DynaLog.LogMessage("=== " & listedUser.ToString() & " ===")
                If listedUser.Enabled Then
                    DynaLog.LogMessage("Checking if account name " & Quote & listedUser.Name & Quote & " doesn't use system-reserved names...")
                    If listedUser.Name = "" OrElse ExistingUsers.Contains(listedUser.Name, StringComparer.OrdinalIgnoreCase) Then
                        DynaLog.LogMessage("Account name " & Quote & listedUser.Name & Quote & " uses system-reserved names.")
                        ProblematicAccountNumbers.Add(userList.IndexOf(listedUser) + 1)
                        FullyValid = False
                    End If
                End If
            Next
            ' Check if some accounts are problematic
            If ProblematicAccountNumbers.Count > 0 Then
                errorReason = "- User account names have been checked and " & ProblematicAccountNumbers.Count & " use system-reserved names:" & CrLf
                For Each accountNumber In ProblematicAccountNumbers
                    errorReason &= "    - Account " & accountNumber & ", with name " & Quote & userList(accountNumber - 1).Name & Quote & CrLf
                Next
            End If
            DynaLog.LogMessage("Checking if account list has duplicate user account names...")
            ' Check collisions
            Dim Collisions = userList _
                             .Where(Function(User) User.Enabled) _
                             .GroupBy(Function(User) User.Name, StringComparer.OrdinalIgnoreCase) _
                             .Where(Function(group) group.Count() > 1) _
                             .Select(Function(group) String.Format("'{0}'", group.Key))
            DynaLog.LogMessage("Collision count: " & Collisions.Count)
            If Collisions.Any() Then
                DynaLog.LogMessage("There are duplicate accounts.")
                If errorReason = "" Then
                    errorReason = "- Some accounts are duplicated. This is not allowed." & CrLf
                Else
                    errorReason &= "- Some accounts are duplicated. This is not allowed." & CrLf
                End If
                FullyValid = False
            End If

            If computerName IsNot Nothing AndAlso Not computerName.DefaultName Then
                DynaLog.LogMessage("Checking if there are accounts that use the computer name...")
                Dim computerNameCollisions = userList.Where(Function(User) User.Name.Equals(computerName.Name, StringComparison.OrdinalIgnoreCase))
                DynaLog.LogMessage("Collision count: " & computerNameCollisions.Count)
                If computerNameCollisions.Any() Then
                    DynaLog.LogMessage("Some accounts use the computer name as their name.")
                    If errorReason = "" Then
                        errorReason = "- Some accounts use the computer name (" & computerName.Name & ") as their name. This is not allowed."
                    Else
                        errorReason &= "- Some accounts use the computer name (" & computerName.Name & ") as their name. This is not allowed."
                    End If
                    FullyValid = False
                End If
            End If

            If errorReason <> "" Then DynaLog.LogMessage("Complete error reason:" & CrLf & errorReason)

            Return New UserValidationResults(FullyValid, errorReason)
        End Function

    End Class

End Namespace
