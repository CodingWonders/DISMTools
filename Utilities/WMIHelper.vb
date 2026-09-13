Imports System.Management

Module WMIHelper

    ''' <summary>
    ''' Gets a collection of Windows Management Instrumentation (WMI) management objects using a provided query
    ''' </summary>
    ''' <param name="ManagementQuery">The management query to perform, in WQL</param>
    ''' <param name="ManagementNamespace">The namespace to query</param>
    ''' <returns>A <see cref="ManagementObjectCollection"/> object that contains all the results of the provided management query.</returns>
    ''' <remarks>
    ''' To succeed, the WMI infrastructure must be in good state and the query must be written correctly. An exception will be thrown if the
    ''' management query is not written correctly. In such cases, the function will return Nothing. To determine if queries are written correctly,
    ''' you can use tools such as the WMI checker tool (wbemtest). Most classes are accessible from the "root\cimv2" namespace. In some cases,
    ''' values that you pass to, for example, WHERE statements; must be parsed separately before performing the management query to avoid potential
    ''' syntax errors. Use the <seealso cref="GetEscapedValue"/> function to escape quotes and backslashes from values before proceeding. Finally,
    ''' a query may be correctly written, but may either not return anything or return the wrong dataset. In such cases, it is best that you check
    ''' the query.
    ''' </remarks>
    Public Function GetResultsFromManagementQuery(ManagementQuery As String, Optional ManagementNamespace As String = "root\cimv2") As ManagementObjectCollection
        DynaLog.LogMessage("Performing management query...")
        DynaLog.LogMessage("- Query: " & ManagementQuery)
        DynaLog.LogMessage("- Namespace: " & ManagementNamespace)
        Try
            If ManagementNamespace = "root\cimv2" Then
                ' We're fine with just using the query
                Return New ManagementObjectSearcher(ManagementQuery).Get()
            Else
                ' Some extra work needs to be made
                Return New ManagementObjectSearcher(New ManagementScope(ManagementNamespace), New ObjectQuery(ManagementQuery)).Get()
            End If
        Catch ex As Exception
            Dim wmiErrorStr As String = String.Format("An error occurred while executing the WMI query: {0}{1}{0}, on namespace {2} -- {3}", Quote, ManagementQuery, ManagementNamespace, ex.Message)
            DynaLog.LogMessage(wmiErrorStr)
            If Debugger.IsAttached Then MessageBox.Show(wmiErrorStr, LocalizationService.ForSection("Utilities.WMIHelper")("Wmierror.Message"), MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End Try
        Return Nothing
    End Function

    ''' <summary>
    ''' Determines whether a specific property exists in a Windows Management Instrumentation (WMI) management object.
    ''' </summary>
    ''' <param name="WmiObject">The base object to which to query the properties</param>
    ''' <param name="PropertyName">The name of the property to check</param>
    ''' <returns>
    ''' This function returns True if the property specified in <paramref name="PropertyName"/> exists in the WMI object, and False if either it does
    ''' not exist or if the WMI object is Nothing.
    ''' </returns>
    ''' <remarks></remarks>
    Private Function WmiObjectPropertyExists(WmiObject As ManagementObject, PropertyName As String) As Boolean
        If WmiObject Is Nothing Then Return False
        Return WmiObject.Properties.Cast(Of PropertyData)().Any(Function(prop) prop.Name.Equals(PropertyName, StringComparison.OrdinalIgnoreCase))
    End Function

    ''' <summary>
    ''' Gets a property from a Windows Management Instrumentation (WMI) management object.
    ''' </summary>
    ''' <param name="Item">The object to get the property from</param>
    ''' <param name="PropertyOfInterest">The property to get from the management object</param>
    ''' <returns>An object reflecting the value of the property of the WMI object.</returns>
    ''' <remarks></remarks>
    Public Function GetObjectValue(Item As ManagementObject, PropertyOfInterest As String) As Object
        DynaLog.LogMessage("Getting value of object in the management object result...")
        DynaLog.LogMessage("- Property that we're interested in getting: " & PropertyOfInterest)
        If Item IsNot Nothing AndAlso PropertyOfInterest <> "" Then
            Return Item(PropertyOfInterest)
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Gets a set of properties from a Windows Management Instrumentation (WMI) management object.
    ''' </summary>
    ''' <param name="BaseItem">The object to get properties from</param>
    ''' <param name="PropertiesOfInterest">The set of properties to get from the management object</param>
    ''' <returns>
    ''' A <see cref="Dictionary(Of String, Object)"/> collection. The keys reflect the property names that were passed to <paramref name="PropertiesOfInterest"/> and
    ''' the values reflect the values of each of the properties.
    ''' </returns>
    ''' <remarks>
    ''' For each of the properties that are passed, this method performs a check to determine if a specific property exists in the management object. If
    ''' a property does not exist in a management object, an entry will still be added to the resulting dictionary. However, the entry will be set to
    ''' Nothing.
    ''' </remarks>
    Public Function GetObjectValues(BaseItem As ManagementObject, ParamArray PropertiesOfInterest() As String) As Dictionary(Of String, Object)
        DynaLog.LogMessage("Getting " & PropertiesOfInterest.Count & " properties of the management object result...")
        DynaLog.DisableLogging()
        Dim ObtainedProperties As New Dictionary(Of String, Object)
        If BaseItem IsNot Nothing Then
            For Each PropertyOfInterest In PropertiesOfInterest
                Dim ObtainedProperty As Object
                If WmiObjectPropertyExists(BaseItem, PropertyOfInterest) Then
                    ObtainedProperty = GetObjectValue(BaseItem, PropertyOfInterest)
                Else
                    ObtainedProperty = Nothing
                End If

                If ObtainedProperties.ContainsKey(PropertyOfInterest) Then
                    ObtainedProperties(PropertyOfInterest) = ObtainedProperty
                Else
                    ObtainedProperties.Add(PropertyOfInterest, ObtainedProperty)
                End If
            Next
        End If
        DynaLog.EnableLogging()
        Return ObtainedProperties
    End Function

    ''' <summary>
    ''' Gets an escaped string value to use as parameters in WMI queries.
    ''' </summary>
    ''' <param name="ValueToEscape">The string value to escape</param>
    ''' <returns>A <see cref="String"/> object with escaped quotes and backslashes</returns>
    ''' <remarks>
    ''' Use this method to prepare string parameters that will be used when querying WMI. For example, the following query:
    ''' "ASSOCIATORS OF {Win32_DiskDrive.DeviceID="\\.\PHYSICALDRIVE1"} WHERE RESULTCLASS = Win32_DiskPartition", will throw an error. In this case,
    ''' the backslashes need to be escaped to become literal backslashes, similarly to how you would escape characters in programming languages such as
    ''' C#, C, C++, or Java. The correct query, in this case, would be: "ASSOCIATORS OF {Win32_DiskDrive.DeviceID="\\\\.\\PHYSICALDRIVE1"} WHERE
    ''' RESULTCLASS = Win32_DiskPartition"
    ''' </remarks>
    Public Function GetEscapedValue(ValueToEscape As String) As String
        Return ValueToEscape.Replace("\", "\\").Replace(Quote, String.Format("\{0}", Quote))
    End Function

End Module
