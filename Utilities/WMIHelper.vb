Imports System.Management

Module WMIHelper

    Public Function GetResultsFromManagementQuery(ManagementQuery As String) As ManagementObjectCollection
        DynaLog.LogMessage("Performing management query...")
        DynaLog.LogMessage("- Query: " & ManagementQuery)
        Try
            Return New ManagementObjectSearcher(ManagementQuery).Get()
        Catch ex As Exception
            Return Nothing
        End Try
        Return Nothing
    End Function

    Public Function GetObjectValue(Item As ManagementObject, PropertyOfInterest As String) As Object
        DynaLog.LogMessage("Getting value of object in the management object result...")
        DynaLog.LogMessage("- Property that we're interested in getting: " & PropertyOfInterest)
        If Item IsNot Nothing AndAlso PropertyOfInterest <> "" Then
            Return Item(PropertyOfInterest)
        End If
        Return Nothing
    End Function

    Public Function GetEscapedValue(ValueToEscape As String) As String
        Return ValueToEscape.Replace("\", "\\").Replace(Quote, String.Format("\{0}", Quote))
    End Function

End Module
