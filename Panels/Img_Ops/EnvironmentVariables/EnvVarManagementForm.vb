Public Class EnvVarManagementForm

    Dim envVarList As New List(Of EnvironmentVariable)

    Private Sub ShowVariableInformation(VariableScope As EnvironmentVariable.EnvironmentVariableScope, Index As Integer)
        Dim machineEnvVars As List(Of EnvironmentVariable) = envVarList.Where(Function(envVar) envVar.Scope = EnvironmentVariable.EnvironmentVariableScope.Machine).ToList(),
            userEnvVars As List(Of EnvironmentVariable) = envVarList.Where(Function(envVar) envVar.Scope = EnvironmentVariable.EnvironmentVariableScope.User).ToList(),
            variableName As String = ""
        If VariableScope = EnvironmentVariable.EnvironmentVariableScope.Machine Then
            TextBox1.Text = machineEnvVars(Index).Name
            TextBox2.Text = "Machine"
            TextBox3.Text = machineEnvVars(Index).Value

            MoveToMachineScopeBtn.Enabled = False
            CopyToMachineScopeBtn.Enabled = False
            MoveToUserScopeBtn.Enabled = True
            CopyToUserScopeBtn.Enabled = True
        Else
            TextBox1.Text = userEnvVars(Index).Name
            TextBox2.Text = "User"
            TextBox3.Text = userEnvVars(Index).Value

            MoveToMachineScopeBtn.Enabled = True
            CopyToMachineScopeBtn.Enabled = True
            MoveToUserScopeBtn.Enabled = False
            CopyToUserScopeBtn.Enabled = False
        End If
        variableName = TextBox1.Text

        TableLayoutPanel1.Enabled = Not ((machineEnvVars.Any(Function(envVar) envVar.Name.Equals(variableName, StringComparison.InvariantCultureIgnoreCase))) AndAlso
                                         (userEnvVars.Any(Function(envVar) envVar.Name.Equals(variableName, StringComparison.InvariantCultureIgnoreCase))))
        Label7.Visible = ((machineEnvVars.Any(Function(envVar) envVar.Name.Equals(variableName, StringComparison.InvariantCultureIgnoreCase))) AndAlso
                          (userEnvVars.Any(Function(envVar) envVar.Name.Equals(variableName, StringComparison.InvariantCultureIgnoreCase))))
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
        TableLayoutPanel1.Enabled = (UserEnvVarLV.SelectedItems.Count = 1 Or SysEnvVarLV.SelectedItems.Count = 1)

        If UserEnvVarLV.SelectedItems.Count = 1 Then
            ShowVariableInformation(EnvironmentVariable.EnvironmentVariableScope.User, UserEnvVarLV.FocusedItem.Index)
        End If
    End Sub

    Private Sub SysEnvVarLV_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SysEnvVarLV.SelectedIndexChanged
        TableLayoutPanel1.Enabled = (UserEnvVarLV.SelectedItems.Count = 1 Or SysEnvVarLV.SelectedItems.Count = 1)
        If SysEnvVarLV.SelectedItems.Count = 1 Then
            ShowVariableInformation(EnvironmentVariable.EnvironmentVariableScope.Machine, SysEnvVarLV.FocusedItem.Index)
        End If
    End Sub

    Private Sub SaveAllChangesBtn_Click(sender As Object, e As EventArgs) Handles SaveAllChangesBtn.Click
        Cursor = Cursors.WaitCursor
        If EnvironmentVariableHelper.SaveEnvironmentVariables(MainForm.MountDir, envVarList) Then
            MsgBox("Environment variable information has been successfully saved to the registry of the target image." & vbCrLf & vbCrLf &
                   "A backup of the previous variable configuration has been saved to your desktop should you need it in case modifications do not go as planned." & vbCrLf & vbCrLf &
                   "Simply load the target image's SYSTEM hive and import this registry file.", vbOKOnly + vbInformation)
        Else
            MsgBox("Environment variable information could not be saved to the registry of the target image.", vbOKOnly + vbExclamation)
        End If
        Cursor = Cursors.Arrow
    End Sub
End Class