Public Class EnvVarManagementForm

    Private Sub EnvVarManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SysEnvVarLV.Items.Clear()
        UserEnvVarLV.Items.Clear()

        Dim envVarList As List(Of EnvironmentVariable) = EnvironmentVariableHelper.GetEnvironmentVariableList(MainForm.MountDir)

        For Each envVar In envVarList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.Machine)
            SysEnvVarLV.Items.Add(New ListViewItem(New String() {envVar.Name, envVar.Value}))
        Next

        For Each envVar In envVarList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.User)
            UserEnvVarLV.Items.Add(New ListViewItem(New String() {envVar.Name, envVar.Value}))
        Next
    End Sub
End Class