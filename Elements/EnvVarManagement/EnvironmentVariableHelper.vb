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

End Module
