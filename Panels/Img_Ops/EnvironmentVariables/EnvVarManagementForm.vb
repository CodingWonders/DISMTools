Public Class EnvVarManagementForm

    Dim envVarList As New List(Of EnvironmentVariable)

    Private Sub ShowVariableInformation(VariableScope As EnvironmentVariable.EnvironmentVariableScope, Index As Integer)
        Dim machineEnvVars As List(Of EnvironmentVariable) = envVarList.Where(Function(envVar) envVar.Scope = EnvironmentVariable.EnvironmentVariableScope.Machine).ToList(),
            userEnvVars As List(Of EnvironmentVariable) = envVarList.Where(Function(envVar) envVar.Scope = EnvironmentVariable.EnvironmentVariableScope.User).ToList()
        If VariableScope = EnvironmentVariable.EnvironmentVariableScope.Machine Then
            TextBox1.Text = machineEnvVars(Index).Name
            TextBox2.Text = "Machine"
            TextBox3.Text = machineEnvVars(Index).Value
        Else
            TextBox1.Text = userEnvVars(Index).Name
            TextBox2.Text = "User"
            TextBox3.Text = userEnvVars(Index).Value
        End If
    End Sub

    Private Sub EnvVarManagementForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        SysEnvVarLV.BackColor = BackColor
        SysEnvVarLV.ForeColor = ForeColor
        UserEnvVarLV.BackColor = BackColor
        UserEnvVarLV.ForeColor = ForeColor
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        TextBox2.BackColor = BackColor
        TextBox2.ForeColor = ForeColor
        TextBox3.BackColor = BackColor
        TextBox3.ForeColor = ForeColor
        SysEnvVarGB.ForeColor = ForeColor
        UserEnvVarGB.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, CurrentTheme.IsDark)
        SysEnvVarLV.Items.Clear()
        UserEnvVarLV.Items.Clear()

        envVarList = EnvironmentVariableHelper.GetEnvironmentVariableList(MainForm.MountDir)

        For Each envVar In envVarList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.Machine)
            SysEnvVarLV.Items.Add(New ListViewItem(New String() {envVar.Name, envVar.Value}))
        Next

        For Each envVar In envVarList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.User)
            UserEnvVarLV.Items.Add(New ListViewItem(New String() {envVar.Name, envVar.Value}))
        Next
    End Sub

    Private Sub UserEnvVarLV_SelectedIndexChanged(sender As Object, e As EventArgs) Handles UserEnvVarLV.SelectedIndexChanged
        If UserEnvVarLV.SelectedItems.Count = 1 Then
            ShowVariableInformation(EnvironmentVariable.EnvironmentVariableScope.User, UserEnvVarLV.FocusedItem.Index)
        End If
    End Sub

    Private Sub SysEnvVarLV_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SysEnvVarLV.SelectedIndexChanged
        If SysEnvVarLV.SelectedItems.Count = 1 Then
            ShowVariableInformation(EnvironmentVariable.EnvironmentVariableScope.Machine, SysEnvVarLV.FocusedItem.Index)
        End If
    End Sub
End Class