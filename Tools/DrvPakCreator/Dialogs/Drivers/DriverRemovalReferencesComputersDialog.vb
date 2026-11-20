Imports System.Windows.Forms

Public Class DriverRemovalReferencesComputersDialog

    Public ReferenceComputerNames As List(Of String)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub DriverRemovalReferencesComputersDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.BackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        lbReferenceComputers.BackColor = BackColor
        lbReferenceComputers.ForeColor = ForeColor
        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)

        lbReferenceComputers.Items.Clear()
        lbReferenceComputers.Items.AddRange(ReferenceComputerNames.ToArray())
    End Sub
End Class
