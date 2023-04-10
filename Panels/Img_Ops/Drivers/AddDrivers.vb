Imports System.Windows.Forms

Public Class AddDrivers

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "File"}))
                    Case "ESN"
                        ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "Archivo"}))
                End Select
            Case 1
                ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "File"}))
            Case 2
                ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "Archivo"}))
        End Select
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Cursor = Cursors.WaitCursor
            If My.Computer.FileSystem.GetFiles(FolderBrowserDialog1.SelectedPath, FileIO.SearchOption.SearchAllSubDirectories, "*.inf").Count > 0 Then
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Folder"}))
                            Case "ESN"
                                ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Carpeta"}))
                        End Select
                    Case 1
                        ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Folder"}))
                    Case 2
                        ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Carpeta"}))
                End Select
                CheckedListBox1.Items.Add(FolderBrowserDialog1.SelectedPath)
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                MsgBox("There are no driver packages in the specified folder", vbOKOnly + vbCritical, Label1.Text)
                            Case "ESN"
                                MsgBox("No hay paquetes de controladores en la carpeta espcificada", vbOKOnly + vbCritical, Label1.Text)
                        End Select
                    Case 1
                        MsgBox("There are no driver packages in the specified folder", vbOKOnly + vbCritical, Label1.Text)
                    Case 2
                        MsgBox("No hay paquetes de controladores en la carpeta espcificada", vbOKOnly + vbCritical, Label1.Text)
                End Select
            End If
            Cursor = Cursors.Arrow
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count = 1 Then
            Button3.Enabled = True
            Button4.Enabled = True
        Else
            Button3.Enabled = False
            Button4.Enabled = False
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If ListView1.FocusedItem.Text <> "" Then
            If CheckedListBox1.Items.Contains(ListView1.FocusedItem.Text) Then
                CheckedListBox1.Items.RemoveAt(CheckedListBox1.FindStringExact(ListView1.FocusedItem.Text))
            End If
            ListView1.Items.Remove(ListView1.FocusedItem)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ListView1.Items.Clear()
        CheckedListBox1.Items.Clear()
        Button3.Enabled = False
        Button4.Enabled = False
    End Sub

    Private Sub AddDrivers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
    End Sub
End Class
