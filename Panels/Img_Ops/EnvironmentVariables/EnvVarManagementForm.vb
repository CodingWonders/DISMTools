Imports Microsoft.Win32

Public Class EnvVarManagementForm

    Dim envVarList As New List(Of EnvironmentVariable)
    Dim currentEnvVarIndex As Tuple(Of EnvironmentVariable.EnvironmentVariableScope, Integer)

    Private Sub ShowVariableInformation(VariableScope As EnvironmentVariable.EnvironmentVariableScope, Index As Integer)
        Dim machineEnvVars As List(Of EnvironmentVariable) = envVarList.Where(Function(envVar) envVar.Scope = EnvironmentVariable.EnvironmentVariableScope.Machine).ToList(),
            userEnvVars As List(Of EnvironmentVariable) = envVarList.Where(Function(envVar) envVar.Scope = EnvironmentVariable.EnvironmentVariableScope.User).ToList(),
            variableName As String = ""
        If VariableScope = EnvironmentVariable.EnvironmentVariableScope.Machine Then
            TextBox1.Text = machineEnvVars(Index).Name
            TextBox2.Text = LocalizationService.ForSection("EnvVars.Info")("Machine.Field")
            TextBox3.Text = machineEnvVars(Index).Value

            MoveToMachineScopeBtn.Enabled = False
            CopyToMachineScopeBtn.Enabled = False
            MoveToUserScopeBtn.Enabled = True
            CopyToUserScopeBtn.Enabled = True

            currentEnvVarIndex = New Tuple(Of EnvironmentVariable.EnvironmentVariableScope, Integer)(EnvironmentVariable.EnvironmentVariableScope.Machine, Index)
        Else
            TextBox1.Text = userEnvVars(Index).Name
            TextBox2.Text = LocalizationService.ForSection("EnvVars.Info")("User.Field")
            TextBox3.Text = userEnvVars(Index).Value

            currentEnvVarIndex = New Tuple(Of EnvironmentVariable.EnvironmentVariableScope, Integer)(EnvironmentVariable.EnvironmentVariableScope.User, Index)

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

    Private Sub ReloadEnvironmentVariableInformation(Optional CleanData As Boolean = False)
        SysEnvVarLV.Items.Clear()
        UserEnvVarLV.Items.Clear()

        If CleanData Then envVarList = EnvironmentVariableHelper.GetEnvironmentVariableList(MainForm.MountDir)

        For Each envVar In envVarList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.Machine)
            SysEnvVarLV.Items.Add(New ListViewItem(New String() {String.Format("{0}{1}", envVar.Name, If(envVar.NoLongerExists, LocalizationService.ForSection("EnvVars.Management")("Removed.Label"), "")), envVar.Value}))
        Next

        For Each envVar In envVarList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.User)
            UserEnvVarLV.Items.Add(New ListViewItem(New String() {String.Format("{0}{1}", envVar.Name, If(envVar.NoLongerExists, LocalizationService.ForSection("EnvVars.Management")("Removed.Label"), "")), envVar.Value}))
        Next
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
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ReloadEnvironmentVariableInformation(True)

        ColumnHeader1.Width = WindowHelper.ScaleLogical(221)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(476)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(221)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(476)
    End Sub

    Private Sub UserEnvVarLV_SelectedIndexChanged(sender As Object, e As EventArgs) Handles UserEnvVarLV.SelectedIndexChanged
        EnvVarDetailsPanel.Enabled = (UserEnvVarLV.SelectedItems.Count = 1 Or SysEnvVarLV.SelectedItems.Count = 1)
        RemoveUserVarBtn.Enabled = UserEnvVarLV.SelectedItems.Count = 1

        If UserEnvVarLV.SelectedItems.Count = 1 Then
            ShowVariableInformation(EnvironmentVariable.EnvironmentVariableScope.User, UserEnvVarLV.FocusedItem.Index)
        End If
    End Sub

    Private Sub SysEnvVarLV_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SysEnvVarLV.SelectedIndexChanged
        EnvVarDetailsPanel.Enabled = (UserEnvVarLV.SelectedItems.Count = 1 Or SysEnvVarLV.SelectedItems.Count = 1)
        RemoveMachineVarButton.Enabled = SysEnvVarLV.SelectedItems.Count = 1

        If SysEnvVarLV.SelectedItems.Count = 1 Then
            ShowVariableInformation(EnvironmentVariable.EnvironmentVariableScope.Machine, SysEnvVarLV.FocusedItem.Index)
        End If
    End Sub

    Private Sub SaveAllChangesBtn_Click(sender As Object, e As EventArgs) Handles SaveAllChangesBtn.Click
        Cursor = Cursors.WaitCursor
        If EnvironmentVariableHelper.SaveEnvironmentVariables(MainForm.MountDir, envVarList) Then
            MsgBox(LocalizationService.ForSection("EnvVars.Management")("InfoLoaded.Message"), vbOKOnly + vbInformation)
        Else
            MsgBox(LocalizationService.ForSection("EnvVars.Management")("InfoSaved.Message"), vbOKOnly + vbExclamation)
        End If
        Cursor = Cursors.Arrow
        EnvVarDetailsPanel.Enabled = False
        ReloadEnvironmentVariableInformation(True)
    End Sub

    Private Function GetEnvironmentVariableIndex(Name As String, Scope As EnvironmentVariable.EnvironmentVariableScope) As Integer
        Return envVarList.FindIndex(Function(variable) variable.Scope = Scope AndAlso variable.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase))
    End Function

    Private Function GetEnvironmentVariableFromIndex(Index As Integer) As EnvironmentVariable
        Return envVarList(Index)
    End Function

    Private Sub MoveEnvironmentVariableToMachineScope(Name As String)
        Dim idx As Integer = GetEnvironmentVariableIndex(Name, EnvironmentVariable.EnvironmentVariableScope.User)
        If idx > -1 Then
            envVarList(idx).Scope = EnvironmentVariable.EnvironmentVariableScope.Machine
        End If
    End Sub

    Private Sub MoveEnvironmentVariableToUserScope(Name As String)
        Dim idx As Integer = GetEnvironmentVariableIndex(Name, EnvironmentVariable.EnvironmentVariableScope.Machine)
        If idx > -1 Then
            envVarList(idx).Scope = EnvironmentVariable.EnvironmentVariableScope.User
        End If
    End Sub

    Private Sub CopyEnvironmentVariableToMachineScope(Name As String, Value As String)
        If envVarList.Any(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.Machine AndAlso
                              variable.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase) AndAlso
                              variable.Value.Equals(Value)) Then
            Exit Sub
        End If

        ' We determine value kind by expanding envvars in the value. Success? ExpandString. Failure? String
        envVarList.Add(New EnvironmentVariable(Name, Value, EnvironmentVariable.EnvironmentVariableScope.Machine,
                                               If(Environment.ExpandEnvironmentVariables(Value).Equals(Value), RegistryValueKind.String, RegistryValueKind.ExpandString)))
    End Sub

    Private Sub CopyEnvironmentVariableToUserScope(Name As String, Value As String)
        If envVarList.Any(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.User AndAlso
                              variable.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase) AndAlso
                              variable.Value.Equals(Value)) Then
            Exit Sub
        End If

        ' We determine value kind by expanding envvars in the value. Success? ExpandString. Failure? String
        envVarList.Add(New EnvironmentVariable(Name, Value, EnvironmentVariable.EnvironmentVariableScope.User,
                                               If(Environment.ExpandEnvironmentVariables(Value).Equals(Value), RegistryValueKind.String, RegistryValueKind.ExpandString)))
    End Sub

    Private Sub ResetVariableList(machineVariables As List(Of EnvironmentVariable), userVariables As List(Of EnvironmentVariable))
        envVarList.Clear()
        envVarList.AddRange(machineVariables)
        envVarList.AddRange(userVariables)
    End Sub

    Private Sub SaveEnvironmentVariableInformation(IndexInfo As Tuple(Of EnvironmentVariable.EnvironmentVariableScope, Integer), Name As String, Value As String)
        Dim machineVariables As List(Of EnvironmentVariable) = envVarList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.Machine).ToList(),
            userVariables As List(Of EnvironmentVariable) = envVarList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.User).ToList()

        If IndexInfo.Item1 = EnvironmentVariable.EnvironmentVariableScope.Machine Then
            machineVariables(IndexInfo.Item2).Name = Name
            machineVariables(IndexInfo.Item2).Value = Value
            machineVariables(IndexInfo.Item2).ValueKind = If(Environment.ExpandEnvironmentVariables(Value).Equals(Value, StringComparison.InvariantCultureIgnoreCase), RegistryValueKind.String, RegistryValueKind.ExpandString)
        Else
            userVariables(IndexInfo.Item2).Name = Name
            userVariables(IndexInfo.Item2).Value = Value
            userVariables(IndexInfo.Item2).ValueKind = If(Environment.ExpandEnvironmentVariables(Value).Equals(Value, StringComparison.InvariantCultureIgnoreCase), RegistryValueKind.String, RegistryValueKind.ExpandString)
        End If

        ResetVariableList(machineVariables, userVariables)
    End Sub

    Private Sub AddUserEnvironmentVariable()
        Dim machineVariables As List(Of EnvironmentVariable) = envVarList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.Machine).ToList(),
            userVariables As List(Of EnvironmentVariable) = envVarList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.User).ToList()

        userVariables.Add(New EnvironmentVariable("NEW_VARIABLE", "", EnvironmentVariable.EnvironmentVariableScope.User, RegistryValueKind.String))

        ResetVariableList(machineVariables, userVariables)
    End Sub

    Private Sub AddMachineEnvironmentVariable()
        Dim machineVariables As List(Of EnvironmentVariable) = envVarList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.Machine).ToList(),
            userVariables As List(Of EnvironmentVariable) = envVarList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.User).ToList()

        machineVariables.Add(New EnvironmentVariable("NEW_VARIABLE", "", EnvironmentVariable.EnvironmentVariableScope.Machine, RegistryValueKind.String))

        ResetVariableList(machineVariables, userVariables)
    End Sub

    Private Sub RemoveEnvironmentVariable()
        Dim machineVariables As List(Of EnvironmentVariable) = envVarList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.Machine).ToList(),
            userVariables As List(Of EnvironmentVariable) = envVarList.Where(Function(variable) variable.Scope = EnvironmentVariable.EnvironmentVariableScope.User).ToList()

        If currentEnvVarIndex.Item1 = EnvironmentVariable.EnvironmentVariableScope.Machine Then
            machineVariables(currentEnvVarIndex.Item2).NoLongerExists = True
        Else
            userVariables(currentEnvVarIndex.Item2).NoLongerExists = True
        End If

        ResetVariableList(machineVariables, userVariables)
    End Sub

    Private Sub CopyToMachineScopeBtn_Click(sender As Object, e As EventArgs) Handles CopyToMachineScopeBtn.Click
        CopyEnvironmentVariableToMachineScope(TextBox1.Text, TextBox3.Text)
        ReloadEnvironmentVariableInformation()
    End Sub

    Private Sub CopyToUserScopeBtn_Click(sender As Object, e As EventArgs) Handles CopyToUserScopeBtn.Click
        CopyEnvironmentVariableToUserScope(TextBox1.Text, TextBox3.Text)
        ReloadEnvironmentVariableInformation()
    End Sub

    Private Sub MoveToMachineScopeBtn_Click(sender As Object, e As EventArgs) Handles MoveToMachineScopeBtn.Click
        MoveEnvironmentVariableToMachineScope(TextBox1.Text)
        ReloadEnvironmentVariableInformation()
    End Sub

    Private Sub MoveToUserScopeBtn_Click(sender As Object, e As EventArgs) Handles MoveToUserScopeBtn.Click
        MoveEnvironmentVariableToUserScope(TextBox1.Text)
        ReloadEnvironmentVariableInformation()
    End Sub

    Private Sub SaveVarBtn_Click(sender As Object, e As EventArgs) Handles SaveVarBtn.Click
        SaveEnvironmentVariableInformation(currentEnvVarIndex, TextBox1.Text, TextBox3.Text)
        ReloadEnvironmentVariableInformation()
    End Sub

    Private Sub AddUserVarButton_Click(sender As Object, e As EventArgs) Handles AddUserVarButton.Click
        AddUserEnvironmentVariable()
        ReloadEnvironmentVariableInformation()
    End Sub

    Private Sub AddMachineVarButton_Click(sender As Object, e As EventArgs) Handles AddMachineVarButton.Click
        AddMachineEnvironmentVariable()
        ReloadEnvironmentVariableInformation()
    End Sub

    Private Sub RemoveUserVarBtn_Click(sender As Object, e As EventArgs) Handles RemoveUserVarBtn.Click, RemoveMachineVarButton.Click
        RemoveEnvironmentVariable()
        ReloadEnvironmentVariableInformation()
    End Sub
End Class
