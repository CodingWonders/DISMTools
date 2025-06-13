Imports System.Management

Module WMIHelper

    Function GetResultsFromManagementQuery(ManagementQuery As String) As ManagementObjectCollection
        Try
            Return New ManagementObjectSearcher(ManagementQuery).Get()
        Catch ex As Exception
            Return Nothing
        End Try
        Return Nothing
    End Function

    Function GetObjectValue(Item As ManagementObject, PropertyOfInterest As String) As Object
        If Item IsNot Nothing AndAlso PropertyOfInterest <> "" Then
            Return Item(PropertyOfInterest)
        End If
        Return Nothing
    End Function

End Module
