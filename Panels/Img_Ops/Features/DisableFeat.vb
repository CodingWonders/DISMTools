Imports System.Windows.Forms

Public Class DisableFeat

    Public featDisablementCount As Integer
    Public featDisablementNames(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ProgressPanel.MountDir = MainForm.MountDir
        featDisablementCount = ListView1.CheckedItems.Count
        ProgressPanel.featDisablementCount = featDisablementCount
        If ListView1.CheckedItems.Count <= 0 Then
            MessageBox.Show(MainForm, "Please select features to disable, and try again.", "No features selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        Else
            Try
                For x = 0 To featDisablementCount - 1
                    featDisablementNames(x) = ListView1.CheckedItems(x).ToString()
                Next
                For x = 0 To featDisablementNames.Length
                    ProgressPanel.featDisablementNames(x) = featDisablementNames(x)
                Next
            Catch ex As Exception

            End Try
            ProgressPanel.featDisablementLastName = ListView1.CheckedItems(featDisablementCount - 1).ToString()
            If CheckBox1.Checked Then
                ProgressPanel.featDisablementParentPkgUsed = True
                ProgressPanel.featDisablementParentPkg = TextBox1.Text
            Else
                ProgressPanel.featDisablementParentPkgUsed = False
                ProgressPanel.featDisablementParentPkg = ""
            End If
            If CheckBox2.Checked Then
                ProgressPanel.featRemoveManifest = False
            Else
                ProgressPanel.featRemoveManifest = True
            End If
        End If
        ProgressPanel.OperationNum = 31
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub DisableFeat_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
        Label2.Text &= " Only enabled features (" & ListView1.Items.Count & ") are shown"
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Label3.Enabled = True
            Button1.Enabled = True
        Else
            Label3.Enabled = False
            Button1.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PkgParentNameLookupDlg.pkgSource = MainForm.MountDir
        PkgParentNameLookupDlg.ShowDialog(Me)
    End Sub
End Class
