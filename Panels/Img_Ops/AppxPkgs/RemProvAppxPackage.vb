Imports System.Windows.Forms

Public Class RemProvAppxPackage

    Public AppxRemovalPackages(65535) As String
    Public AppxRemovalFriendlyNames(65535) As String
    Public AppxRemovalCount As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        AppxRemovalCount = ListView1.CheckedItems.Count
        ProgressPanel.appxRemovalCount = AppxRemovalCount
        If ListView1.CheckedItems.Count = 0 Then
            MsgBox("Please specify AppX packages to remove and try again.", vbOKOnly + vbCritical, "Remove provisioned AppX packages")
        Else
            If AppxRemovalCount > 65535 Then
                MsgBox("Right now, you can only specify less than 65535 AppX packages. This is a program limitation that will be gone in a future update.", vbOKOnly + vbCritical, "Remove provisioned AppX packages")
                Exit Sub
            Else
                For x = 0 To AppxRemovalCount - 1
                    AppxRemovalPackages(x) = ListView1.CheckedItems(x).Text
                Next
                For x = 0 To AppxRemovalCount - 1
                    AppxRemovalFriendlyNames(x) = ListView1.CheckedItems(x).SubItems(1).Text
                Next
                For x = 0 To AppxRemovalPackages.Length - 1
                    ProgressPanel.appxRemovalPackages(x) = AppxRemovalPackages(x)
                Next
                For x = 0 To AppxRemovalFriendlyNames.Length - 1
                    ProgressPanel.appxRemovalPkgNames(x) = AppxRemovalFriendlyNames(x)
                Next
                ProgressPanel.appxRemovalLastPackage = ListView1.CheckedItems(AppxRemovalCount - 1).ToString().Replace("ListViewItem: {", "").Trim().Replace("}", "").Trim()
            End If
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 38
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub RemProvAppxPackage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListView1.ForeColor = ForeColor
    End Sub
End Class
