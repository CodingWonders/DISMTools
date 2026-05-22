Imports System.Windows.Forms

Public Class FeatureFilterAssistantDialog

    Public AppliedQuery As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        AppliedQuery = TextBox1.Text
        If ComboBox1.SelectedIndex > 0 Then
            AppliedQuery &= " "
            Select Case ComboBox1.SelectedIndex
                Case 1
                    AppliedQuery &= "state:enabled"
                Case 2
                    AppliedQuery &= "state:enablepending"
                Case 3
                    AppliedQuery &= "state:disabled"
                Case 4
                    AppliedQuery &= "state:disablepending"
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
