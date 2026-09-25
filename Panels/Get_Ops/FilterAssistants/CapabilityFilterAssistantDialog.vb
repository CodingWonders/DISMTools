Imports System.Windows.Forms

Public Class CapabilityFilterAssistantDialog

    Public AppliedQuery As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        AppliedQuery = TextBox1.Text
        If ComboBox1.SelectedIndex > 0 Then
            AppliedQuery &= " "
            Select Case ComboBox1.SelectedIndex
                Case 1
                    AppliedQuery &= "state:installed"
                Case 2
                    AppliedQuery &= "state:installpending"
                Case 3
                    AppliedQuery &= "state:notpresent"
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

    Private Sub FeatureFilterAssistantDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        OK_Button.Text = LocalizationService.ForSection("Designer.CapabilityFilter")("Apply.Button")
        Cancel_Button.Text = LocalizationService.ForSection("Designer.CapabilityFilter")("Clear.Button")
        Label2.Text = LocalizationService.ForSection("Designer.CapabilityFilter")("Name.Label")
        Label3.Text = LocalizationService.ForSection("Designer.CapabilityFilter")("State.Label")
        ComboBox1.Items.Clear()
        ComboBox1.Items.AddRange({
            LocalizationService.ForSection("Designer.CapabilityFilter")("AnyState.Item"),
            LocalizationService.ForSection("Designer.CapabilityFilter")("Installed.Item"),
            LocalizationService.ForSection("Designer.CapabilityFilter")("Install.Pending.Item"),
            LocalizationService.ForSection("Designer.CapabilityFilter")("Removed.Item")
        })
        Label1.Text = LocalizationService.ForSection("Designer.CapabilityFilter")("FilterInfo.Prompt.Label")
        Text = LocalizationService.ForSection("Designer.CapabilityFilter")("FilterInfo.Title")

        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        ComboBox1.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        If ComboBox1.SelectedIndex < 0 Then ComboBox1.SelectedIndex = 0
    End Sub
End Class
