Imports System.Windows.Forms

Public Class EnableFeat

    Public featEnablementCount As Integer
    Public featEnablementNames(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ProgressPanel.MountDir = MainForm.MountDir
        featEnablementCount = ListView1.CheckedItems.Count
        ProgressPanel.featEnablementCount = featEnablementCount
        If ListView1.CheckedItems.Count <= 0 Then
            MessageBox.Show(MainForm, "Please select features to enable, and try again.", "No features selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        Else
            Try
                For x = 0 To featEnablementCount - 1
                    featEnablementNames(x) = ListView1.CheckedItems(x).ToString()
                Next
                For x = 0 To featEnablementNames.Length
                    ProgressPanel.featEnablementNames(x) = featEnablementNames(x)
                Next
            Catch ex As Exception

            End Try
            ProgressPanel.featEnablementLastName = ListView1.CheckedItems(featEnablementCount - 1).ToString()
            If CheckBox1.Checked Then
                ProgressPanel.featisParentPkgNameUsed = True
                ProgressPanel.featParentPkgName = TextBox1.Text
            Else
                ProgressPanel.featisParentPkgNameUsed = False
                ProgressPanel.featParentPkgName = ""
            End If
            If CheckBox2.Checked Then
                ProgressPanel.featisSourceSpecified = True
                ProgressPanel.featSource = TextBox2.Text
            Else
                ProgressPanel.featisSourceSpecified = True
                ProgressPanel.featSource = ""
            End If
            If CheckBox3.Checked Then
                ProgressPanel.featParentIsEnabled = True
            Else
                ProgressPanel.featParentIsEnabled = False
            End If
            If CheckBox4.Checked Then
                ProgressPanel.featContactWindowsUpdate = True
            ElseIf CheckBox4.Checked = False And CheckBox4.Enabled Then
                ProgressPanel.featContactWindowsUpdate = False
            ElseIf CheckBox4.Enabled = False Then
                ' Tell program to contact Windows Update, as the parameter "/LimitAccess" doesn't apply to offline images
                ProgressPanel.featContactWindowsUpdate = True
            End If
            If CheckBox5.Checked Then
                ProgressPanel.featCommitAfterEnablement = True
            Else
                ProgressPanel.featCommitAfterEnablement = False
            End If
        End If
        ProgressPanel.OperationNum = 30
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub EnableFeature_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
        Label2.Text &= " Only disabled features (" & ListView1.Items.Count & ") are shown"
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

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked Then
            Label4.Enabled = True
            Button2.Enabled = True
            TextBox2.Enabled = True
        Else
            Label4.Enabled = False
            Button2.Enabled = False
            TextBox2.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PkgParentNameLookupDlg.pkgSource = MainForm.MountDir
        PkgParentNameLookupDlg.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK And FolderBrowserDialog1.SelectedPath <> "" Then
            TextBox2.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub
End Class
