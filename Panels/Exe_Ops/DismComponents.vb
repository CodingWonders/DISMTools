Imports System.Windows.Forms
Imports System.IO

Public Class DismComponents

    Dim fv As FileVersionInfo

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub DismComponents_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListView1.Items.Clear()
        Visible = True
        For Each DismComponent In My.Computer.FileSystem.GetFiles(Path.GetDirectoryName(Options.TextBox1.Text) & "\dism", FileIO.SearchOption.SearchTopLevelOnly)
            fv = FileVersionInfo.GetVersionInfo(DismComponent)
            ListView1.Items.Add(Path.GetFileName(DismComponent)).SubItems.Add(fv.ProductVersion.ToString())
        Next
    End Sub
End Class
