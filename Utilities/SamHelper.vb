Imports System.Management
Imports System.IO

Module SamHelper

    ''' <summary>
    ''' Retrieves user profiles that an AppX package has been registered to, based on their SIDs (Security Identifiers)
    ''' </summary>
    ''' <param name="pckgdeps">The list of package registrations (PCKGDEP files)</param>
    ''' <returns>A message showing mapped users and their IDs</returns>
    ''' <remarks>This is only possible in the online installation management mode as the WMI queries use the system's SAM file. 
    ''' SAM registry hives in other images can't be mounted for security reasons, and it's impossible to continue in order not to
    ''' show incorrect mappings between user information and package registrations.
    ''' 
    ''' An example of an incorrect mapping could be SID "S-1-5-21-...-...-...-1000", which could be associated to user "John Doe", but in reality be associated to "Jane Doe" because it's an 
    ''' offline image.
    ''' </remarks>
    Function MapPckgdepsToSamProfiles(pckgdeps As List(Of String)) As String
        Dim mappedUsers As New Dictionary(Of String, String)
        Dim userMessage As String = ""
        Try
            ' First, let's check our SAM profiles and map them
            DynaLog.LogMessage("Checking user profiles in system...")
            Dim UserCollection As ManagementObjectCollection = GetResultsFromManagementQuery("SELECT Name, SID FROM Win32_UserAccount WHERE LocalAccount = True AND Disabled = False")
            If UserCollection Is Nothing OrElse UserCollection.Count = 0 Then
                Return ""
            End If
            DynaLog.LogMessage("Management collection is not null. Proceeding to map account SIDs with their names...")
            For Each UserObject As ManagementObject In UserCollection
                mappedUsers.Add(GetObjectValue(UserObject, "SID"), GetObjectValue(UserObject, "Name"))
            Next
            ' Then we'll see the names of the pckgdeps for any matches
            DynaLog.LogMessage("Putting names to the pckgdeps...")
            For Each pckgdep In pckgdeps
                Dim fileName As String = Path.GetFileNameWithoutExtension(pckgdep)
                Dim userName As String = mappedUsers(fileName)
                If userName <> "" Then
                    DynaLog.LogMessage("We grabbed a user name.")
                    userMessage &= "    - SID " & fileName & " -- " & ControlChars.Quote & userName & ControlChars.Quote & ControlChars.CrLf
                Else
                    DynaLog.LogMessage("We didn't grab a user name.")
                    userMessage &= "    - SID " & fileName & " could not be associated to any user" & ControlChars.CrLf
                End If
            Next
        Catch ex As Exception
            DynaLog.LogMessage("Could not map users. Error code: " & Hex(ex.HResult))
            DynaLog.LogMessage("The error message will not be logged in the offchance it has personally identifiable information.")
            DynaLog.LogMessage("Hook a debugger to the process with a PID you can see on the left of this message to learn more.")
            If Debugger.IsAttached Then
                Debugger.Break()
            End If
            Return ""
        End Try
        Return userMessage
    End Function

End Module
