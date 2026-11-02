Imports Microsoft.VisualBasic.ControlChars

Namespace Elements

    Public Class UserValidationResults

        Public Property ValidationErrorReason As String
        Public Property IsValid As Boolean

        Public Sub New()
            ValidationErrorReason = ""
            IsValid = False
        End Sub

        Public Sub New(Valid As Boolean, Reason As String)
            Me.ValidationErrorReason = Reason
            Me.IsValid = Valid
        End Sub

        Public Overrides Function ToString() As String
            Return Me.ValidationErrorReason
        End Function

    End Class

    Public Class UserNameSuggester

        ''' <summary>
        ''' From a user account name, suggest a user display name from the first 5 letters of the first word.
        ''' </summary>
        ''' <param name="UserName">The user account name</param>
        ''' <returns>The first 5 characters of the first word of the user name as the account display name</returns>
        ''' <remarks>
        ''' This function replicates the behavior of a system when a user creates a new user account using their
        ''' Microsoft account. In those cases, the system uses the first 5 letters of the account email as the user name.
        ''' In the case of DISMTools, this is the first 5 letters of the first word. This can be changed by the user
        ''' at any time by clicking "Generate..." on a given user card.
        ''' </remarks>
        Public Shared Function SuggestUserName(UserName As String) As String
            If UserName = "" Then Return ""

            Dim wordParts() As String = UserName.Split(" ")
            Return wordParts(0).ToLower().Substring(0, Math.Min(5, wordParts(0).Length))
        End Function

    End Class

    Public Class UserValidator

        Private Shared Function GetUserNamesByRID(ParamArray UserRids As Integer()) As List(Of String)
            Dim userNames As New List(Of String)

            For Each UserRid In UserRids
                Dim UserMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT Name FROM Win32_UserAccount WHERE SID LIKE {0}S-1-5-21-%-{1}{0}", Quote, UserRid))
                If UserMOC Is Nothing Then Continue For

                userNames.Add(WMIHelper.GetObjectValue(UserMOC(0), "Name"))
            Next

            Return userNames
        End Function

        Public Shared Function ValidateUsers(UserCardList As List(Of UserCard), Optional computerName As ComputerName = Nothing) As UserValidationResults
            DynaLog.LogMessage("Beginning validation of users in user list, with " & UserCardList.Count & " user(s)...")
            DynaLog.LogMessage("- Computer name passed for extended validation: " & Quote & computerName.Name & Quote)
            Dim ProblematicAccountNumbers As New List(Of Integer)
            Dim validationResults As New UserValidationResults()
            DynaLog.LogMessage("Checking if list contains any users...")
            If UserCardList Is Nothing OrElse UserCardList.Count = 0 Then
                DynaLog.LogMessage("There are no users in the list. Perhaps it's the user that's being too smart, or an error occurred when grabbing user accounts.")
                Return New UserValidationResults(False, "Either no users have been specified, or a program error occurred.")
            End If
            Dim errorReason As String = ""
            Dim ExistingUsers As New List(Of String) From {"administrator", "guest", "defaultaccount", "system", "network service", "local service", "none", "wdagutilityaccount"}
            ExistingUsers.AddRange(GetUserNamesByRID(500, 501))     ' Add the localized Administrator and Guest accounts; their RIDs don't change, so we're good.

            ' Assume it's true by default
            Dim FullyValid As Boolean = True
            For Each listedUserCard As UserCard In UserCardList
                DynaLog.LogMessage("=== User, with name: " & Quote & listedUserCard.AccountName & Quote & "; Display Name: " & Quote & listedUserCard.DisplayName & Quote & "; Group: " & If(listedUserCard.IsAdministrator, "Administrators", "Users") & " ===")
                If listedUserCard.Enabled Then
                    DynaLog.LogMessage("Checking if account name " & Quote & listedUserCard.AccountName & Quote & " doesn't use system-reserved names...")
                    If listedUserCard.AccountName = "" OrElse ExistingUsers.Contains(listedUserCard.AccountName, StringComparer.OrdinalIgnoreCase) Then
                        DynaLog.LogMessage("Account name " & Quote & listedUserCard.AccountName & Quote & " uses system-reserved names.")
                        ProblematicAccountNumbers.Add(UserCardList.IndexOf(listedUserCard) + 1)
                        FullyValid = False
                    End If
                End If
            Next
            ' Check if some accounts are problematic
            If ProblematicAccountNumbers.Count > 0 Then
                errorReason = "- User account names have been checked and " & ProblematicAccountNumbers.Count & " use system-reserved names:" & CrLf
                For Each accountNumber In ProblematicAccountNumbers
                    errorReason &= "    - Account " & accountNumber & ", with name " & Quote & UserCardList(accountNumber - 1).AccountName & Quote & CrLf
                Next
            End If
            DynaLog.LogMessage("Checking if account list has duplicate user account names...")
            ' Check collisions
            Dim Collisions = UserCardList.Where(Function(User) User.AccountName <> "").GroupBy(Function(User) User.AccountName, StringComparer.OrdinalIgnoreCase).Where(Function(group) group.Count() > 1).Select(Function(group) String.Format("'{0}'", group.Key))
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
                Dim computerNameCollisions = UserCardList.Where(Function(User) User.Name.Equals(computerName.Name, StringComparison.OrdinalIgnoreCase))
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
