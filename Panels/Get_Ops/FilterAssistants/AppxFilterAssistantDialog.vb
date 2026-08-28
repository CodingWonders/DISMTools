Imports System.Windows.Forms

Public Class AppxFilterAssistantDialog

    Public AppliedQuery As String

    Private SelectedUserSid As String = ""
    Private userAccounts As New List(Of SystemUserAccount)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If NameFilterRadioButton.Checked Then
            AppliedQuery = PackageNameTextBox.Text
        Else
            ' Determine reg status filter
            Select Case RegStatusComboBox.SelectedIndex
                Case 0 : AppliedQuery = "regto:noone"
                Case 1 : AppliedQuery = "regto:anyone"
                Case 2 : AppliedQuery = "regto:me"
                Case 3
                    If Not SelectedUserSid.StartsWith("S-1-5", StringComparison.OrdinalIgnoreCase) Then Exit Sub
                    AppliedQuery = String.Format("regto:{0}", SelectedUserSid)
            End Select
        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        AppliedQuery = ""
        ' This one does the same thing as the OK button, but after clearing the query.
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Class SystemUserAccount
        Public Property AccountName As String
        Public Property AccountFullName As String
        Public Property AccountSid As String

        Public Sub New(name As String, fullName As String, sid As String)
            AccountName = name
            AccountFullName = fullName
            AccountSid = sid
        End Sub
    End Class

    Private Function GetSystemUsers() As List(Of SystemUserAccount)
        Dim userAccounts As New List(Of SystemUserAccount)

        Dim UserMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery("SELECT Name, FullName, Sid FROM Win32_UserAccount WHERE Disabled = FALSE")
        If UserMOC Is Nothing Then Return userAccounts

        For Each UserMO In UserMOC
            Dim userDetails As Dictionary(Of String, Object) = WMIHelper.GetObjectValues(UserMO, "Name", "FullName", "Sid")
            userAccounts.Add(New SystemUserAccount(userDetails("Name"), userDetails("FullName"), userDetails("Sid")))
        Next

        Return userAccounts
    End Function

    Private Sub AppxFilterAssistantDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        PackageNameTextBox.BackColor = BackColor
        PackageNameTextBox.ForeColor = ForeColor
        SelectedUserDetailsTextBox.BackColor = BackColor
        SelectedUserDetailsTextBox.ForeColor = ForeColor
        RegStatusComboBox.BackColor = BackColor
        RegStatusComboBox.ForeColor = ForeColor
        UserAccountLV.BackColor = BackColor
        UserAccountLV.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ' Get user accounts
        UserAccountLV.Items.Clear()
        userAccounts = GetSystemUsers()
        UserAccountLV.Items.AddRange(userAccounts.Select(Function(sysAccount) New ListViewItem(New String() {sysAccount.AccountName, sysAccount.AccountFullName, sysAccount.AccountSid})).ToArray())

        ' Set disabled ListView's backcolor. Source: https://stackoverflow.com/questions/17461902/changing-background-color-of-listview-c-sharp-when-disabled
        Dim clientHeight As Integer = WindowHelper.ScaleLogical(24) * (userAccounts.Count + 1)
        Dim bm As New Bitmap(UserAccountLV.ClientSize.Width, If(UserAccountLV.ClientSize.Height > clientHeight, UserAccountLV.ClientSize.Height, clientHeight))
        Graphics.FromImage(bm).Clear(UserAccountLV.BackColor)
        UserAccountLV.BackgroundImage = bm

        ColumnHeader1.Width = WindowHelper.ScaleLogical(128)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(192)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(280)
    End Sub

    Private Sub NameFilterRadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles NameFilterRadioButton.CheckedChanged
        PackageNameTextBox.Enabled = NameFilterRadioButton.Checked
        RegStatusPanel.Enabled = Not NameFilterRadioButton.Checked
    End Sub

    Private Sub RegStatusComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RegStatusComboBox.SelectedIndexChanged
        SystemUserFilterPanel.Enabled = RegStatusComboBox.SelectedIndex >= 3

        ' If no mappings policy is enabled, then don't filter by SIDS
        If MainForm.NoNTSamMappings AndAlso RegStatusComboBox.SelectedIndex > 1 Then RegStatusComboBox.SelectedIndex = 1
    End Sub

    Private Sub UserAccountLV_SelectedIndexChanged(sender As Object, e As EventArgs) Handles UserAccountLV.SelectedIndexChanged
        Try
            If UserAccountLV.SelectedItems.Count = 1 Then
                Dim selectedUser As SystemUserAccount = userAccounts.ElementAtOrDefault(UserAccountLV.FocusedItem.Index)
                If selectedUser IsNot Nothing Then
                    SelectedUserDetailsTextBox.Text = String.Format("{0} - SID {1}", If(selectedUser.AccountFullName <> "", String.Format("{0} ({1})", selectedUser.AccountFullName, selectedUser.AccountName), selectedUser.AccountName), selectedUser.AccountSid)
                    SelectedUserSid = selectedUser.AccountSid
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
