Imports System.IO
Imports Microsoft.Win32

Module EnvironmentVariableHelper

    Private Function GetMachineEnvironmentVariables(MountPath As String) As List(Of EnvironmentVariable)
        Dim machineEnvironmentVariables As New List(Of EnvironmentVariable)
        Try
            If Not RegistryHelper.LoadRegistryHive(Path.Combine(MountPath, "Windows", "system32", "config", "SYSTEM"), "HKLM\zSYSTEM") = 0 Then Throw New Exception("Registry hive could not be loaded. Machine variables cannot be obtained")
            Dim defaultControlSet As Integer = RegistryHelper.GetDefaultControlSet("zSYSTEM")
            If defaultControlSet > 0 Then
                Dim registryPath As String = String.Format("zSYSTEM\ControlSet{0}\Control\Session Manager\Environment", defaultControlSet.ToString().PadLeft(3, "0"c))

                Dim envVarRk As RegistryKey = Registry.LocalMachine.OpenSubKey(registryPath, False)
                For Each VariableName In envVarRk.GetValueNames()
                    machineEnvironmentVariables.Add(New EnvironmentVariable(VariableName, envVarRk.GetValue(VariableName, "", RegistryValueOptions.DoNotExpandEnvironmentNames), EnvironmentVariable.EnvironmentVariableScope.Machine, envVarRk.GetValueKind(VariableName)))
                Next
                envVarRk.Close()
            End If
            RegistryHelper.UnloadRegistryHive("HKLM\zSYSTEM")
        Catch ex As Exception

        End Try
        Return machineEnvironmentVariables
    End Function

    Private Function GetDefaultUserEnvironmentVariables(MountPath As String) As List(Of EnvironmentVariable)
        Dim userEnvironmentVariables As New List(Of EnvironmentVariable)
        Try
            If Not RegistryHelper.LoadRegistryHive(Path.Combine(MountPath, "Users", "Default", "NTUSER.DAT"), "HKLM\zDEFAULT") = 0 Then Throw New Exception("Registry hive could not be loaded. User variables cannot be obtained")

            Dim envVarRk As RegistryKey = Registry.LocalMachine.OpenSubKey("zDEFAULT\Environment", False)
            For Each VariableName In envVarRk.GetValueNames()
                userEnvironmentVariables.Add(New EnvironmentVariable(VariableName, envVarRk.GetValue(VariableName, "", RegistryValueOptions.DoNotExpandEnvironmentNames), EnvironmentVariable.EnvironmentVariableScope.User, envVarRk.GetValueKind(VariableName)))
            Next
            envVarRk.Close()

            RegistryHelper.UnloadRegistryHive("HKLM\zDEFAULT")

        Catch ex As Exception

        End Try
        Return userEnvironmentVariables
    End Function

    Public Function GetEnvironmentVariableList(MountPath As String) As List(Of EnvironmentVariable)
        Dim envVarList As New List(Of EnvironmentVariable)
        Dim machineVariables As List(Of EnvironmentVariable) = GetMachineEnvironmentVariables(MountPath),
            userVariables As List(Of EnvironmentVariable) = GetDefaultUserEnvironmentVariables(MountPath)

        envVarList.AddRange(machineVariables)
        envVarList.AddRange(userVariables)

        Return envVarList
    End Function

    Public Function SaveEnvironmentVariables(MountPath As String, VariableList As List(Of EnvironmentVariable)) As Boolean
        If VariableList Is Nothing Then Return False

        Dim machineVariables As List(Of EnvironmentVariable) = VariableList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.Machine).ToList(),
            userVariables As List(Of EnvironmentVariable) = VariableList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.User).ToList()

        Try
            ' An interesting discovery is that environment variables can only really be string or expand string values. Even
            ' if it only contains numeric values, it will be saved as a string.

            If RegistryHelper.LoadRegistryHive(Path.Combine(MountPath, "Windows", "system32", "config", "SYSTEM"), "HKLM\zSYSTEM") = 0 Then
                Dim defaultControlSet As Integer = RegistryHelper.GetDefaultControlSet("zSYSTEM")
                If defaultControlSet > 0 Then
                    ' If we could load the hive, we determine the control set then configure machine vars.
                    For Each machineVariable In machineVariables
                        ' We'll check if we can expand the environment variables contained within the envvar value. If we could expand them,
                        ' then we go with an expand string. Otherwise we'll go with a string
                        Dim expandedValue As String = Environment.ExpandEnvironmentVariables(machineVariable.Value)

                        RegistryHelper.AddRegistryItem(New RegistryItem(String.Format("HKLM\zSYSTEM\ControlSet{0}\Control\Session Manager\Environment",
                                                                                      defaultControlSet.ToString().PadLeft(3, "0"c)),
                                                                                  machineVariable.Name,
                                                                                  If(machineVariable.Value.Equals(expandedValue), RegistryValueKind.String, RegistryValueKind.ExpandString),
                                                                                  machineVariable.Value))
                    Next
                End If
                RegistryHelper.UnloadRegistryHive("HKLM\zSYSTEM")
            End If

            If RegistryHelper.LoadRegistryHive(Path.Combine(MountPath, "Users", "Default", "NTUSER.DAT"), "HKLM\zDEFAULT") = 0 Then
                ' We don't have to determine control sets here
                For Each userVariable In userVariables
                    Dim expandedValue As String = Environment.ExpandEnvironmentVariables(userVariable.Value)

                    RegistryHelper.AddRegistryItem(New RegistryItem("HKLM\zDEFAULT\Environment",
                                                                    userVariable.Name,
                                                                    If(userVariable.Value.Equals(expandedValue), RegistryValueKind.String, RegistryValueKind.ExpandString),
                                                                    userVariable.Value))

                Next
                RegistryHelper.UnloadRegistryHive("HKLM\zDEFAULT")
            End If

        Catch ex As Exception

            Return False
        End Try
        Return True
    End Function

End Module
