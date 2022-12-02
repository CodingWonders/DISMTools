Imports System.Windows.Forms
Imports System.IO

Public Class RemPackage

    Public pkgRemovalCount As Integer
    Public pkgRemovalNames(65535) As String
    Public pkgRemovalFiles(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ProgressPanel.MountDir = MainForm.MountDir
        ProgressPanel.pkgRemovalSource = TextBox1.Text
        If RadioButton1.Checked Then
            pkgRemovalCount = CheckedListBox1.CheckedItems.Count
            ProgressPanel.pkgRemovalOp = 0
            ProgressPanel.pkgRemovalCount = pkgRemovalCount
            If CheckedListBox1.CheckedItems.Count <= 0 Then
                MessageBox.Show(MainForm, "Please select packages to remove, and try again.", "No packages selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Else
                If pkgRemovalCount > 65535 Then
                    MessageBox.Show(MainForm, "Right now, due to program limitations, you can select 65535 packages or less.", "Current program limitation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                Else
                    Try
                        For x As Integer = 0 To pkgRemovalCount - 1
                            pkgRemovalNames(x) = CheckedListBox1.CheckedItems(x).ToString()
                        Next
                        For x = 0 To pkgRemovalNames.Length
                            ProgressPanel.pkgRemovalNames(x) = pkgRemovalNames(x)
                        Next
                    Catch ex As Exception

                    End Try
                End If
                ProgressPanel.pkgRemovalLastName = CheckedListBox1.CheckedItems(pkgRemovalCount - 1).ToString()
            End If
        Else
            pkgRemovalCount = CheckedListBox2.CheckedItems.Count
            ProgressPanel.pkgRemovalOp = 1
            ProgressPanel.pkgRemovalCount = pkgRemovalCount
            If CheckedListBox2.CheckedItems.Count < 0 Then
                MessageBox.Show(MainForm, "Please select packages to remove, and try again.", "No packages selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            Else
                If pkgRemovalCount > 65535 Then
                    MessageBox.Show(MainForm, "Right now, due to program limitations, you can select 65535 packages or less.", "Current program limitation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                Else
                    Try
                        For x As Integer = 0 To pkgRemovalCount - 1
                            pkgRemovalFiles(x) = CheckedListBox2.CheckedItems(x).ToString()
                        Next
                        For x = 0 To pkgRemovalFiles.Length
                            ProgressPanel.pkgRemovalFiles(x) = pkgRemovalFiles(x)
                        Next
                    Catch ex As Exception

                    End Try
                End If
                ProgressPanel.pkgRemovalLastFile = CheckedListBox2.CheckedItems(pkgRemovalCount - 1).ToString()
            End If
        End If
        ProgressPanel.OperationNum = 27
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub RemPackage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
        Label2.Text = "This image contains " & CheckedListBox1.Items.Count & " packages"
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            CheckedListBox1.Enabled = True
            Label2.Enabled = True
            Label3.Enabled = False
            TextBox1.Enabled = False
            Button1.Enabled = False
            CheckedListBox2.Enabled = False
            Label4.Enabled = False
        Else
            CheckedListBox1.Enabled = False
            Label2.Enabled = False
            Label3.Enabled = True
            TextBox1.Enabled = True
            Button1.Enabled = True
            CheckedListBox2.Enabled = True
            Label4.Enabled = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog()
        If DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        Else
            TextBox1.Text = ""
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Try
            For Each cabFile In My.Computer.FileSystem.GetFiles(TextBox1.Text, FileIO.SearchOption.SearchAllSubDirectories, "*.cab")
                CheckedListBox2.Items.Add(cabFile)
            Next
        Catch ex As Exception
            Try
                For Each cabFile In My.Computer.FileSystem.GetFiles(TextBox1.Text, FileIO.SearchOption.SearchTopLevelOnly, "*.cab")
                    CheckedListBox2.Items.Add(cabFile)
                Next
            Catch ex2 As Exception
                Exit Try    ' Give up
            End Try
        End Try
        If CheckedListBox2.Items.Count <= 0 Then
            MsgBox("We couldn't scan the package source for CAB files. Please try again.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "DISMTools")
        End If
    End Sub
End Class
