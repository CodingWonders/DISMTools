Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class UnattendMgr

    Private Sub OK_Button_Click(sender As Object, e As EventArgs)
        DialogResult = Windows.Forms.DialogResult.OK
        Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If FolderBrowserDialog1.SelectedPath.Contains("unattend_xml") Then
                TextBox1.Text = FolderBrowserDialog1.SelectedPath
            Else
                TextBox1.Text = Path.Combine(FolderBrowserDialog1.SelectedPath, "unattend_xml")
            End If
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" AndAlso Directory.Exists(TextBox1.Text) Then
            ScanForUnattendFiles(TextBox1.Text)
        End If
    End Sub

    Sub ScanForUnattendFiles(folderPath As String)
        Dim UnattendFiles() As String
        ListView1.Items.Clear()
        Try
            If Directory.Exists(folderPath) Then
                If folderPath.Contains("unattend_xml") Then
                    UnattendFiles = Directory.GetFiles(folderPath, "*.xml", SearchOption.AllDirectories)
                Else
                    UnattendFiles = Directory.GetFiles(Path.Combine(folderPath, "unattend_xml"), "*.xml", SearchOption.AllDirectories)
                End If
            Else
                Throw New Exception("The folder path does not exist")
            End If
            If UnattendFiles.Length > 0 Then
                For Each xmlFile In UnattendFiles
                    ListView1.Items.Add(New ListViewItem(New String() {Path.GetFileName(xmlFile), File.GetCreationTime(xmlFile), File.GetLastWriteTime(xmlFile), File.GetLastAccessTime(xmlFile)}))
                Next
            Else
                Throw New Exception("Search concluded with no elements found")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbCritical, "Search error")
        End Try
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        ActionsTLP.Enabled = (ListView1.SelectedItems.Count = 1)
        Button4.Enabled = (MainForm.isProjectLoaded And Not (MainForm.OnlineManagement Or MainForm.OfflineManagement))
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Process.Start(Path.Combine(TextBox1.Text, ListView1.FocusedItem.SubItems(0).Text))
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", "/select," & Quote & Path.Combine(TextBox1.Text, ListView1.FocusedItem.SubItems(0).Text) & Quote)
    End Sub
End Class