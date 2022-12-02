Imports System.Windows.Forms
Imports System.IO
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.ControlChars

Public Class PkgParentNameLookupDlg

    Public pkgSource As String
    Public pkgArgs As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If TextBox1.Text = "" Then
            MsgBox("Please specify a package name, and try again.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Installed package names")
            Exit Sub
        ElseIf Not ListBox1.Items.Contains(TextBox1.Text) Then
            MsgBox("The specified package name does not seem to be in the image. Please specify an available entry, and try again", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Installed package names")
            Exit Sub
        Else
            EnableFeat.TextBox1.Text = TextBox1.Text
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        TextBox1.Text = ListBox1.SelectedItem
    End Sub

    Private Sub PkgParentNameLookupDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
        Label3.Visible = True
        OK_Button.Enabled = False
        Cancel_Button.Enabled = False
        ListBox1.Items.Clear()
        PackageListerBW.RunWorkerAsync()
    End Sub

    Private Sub PackageListerBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles PackageListerBW.DoWork
        File.WriteAllText(".\temp.bat",
                  "@echo off" & CrLf &
                  "dism /English /image=" & pkgSource & " /get-packages | findstr /c:" & Quote & "Package Identity : " & Quote & " > .\pkgnames")
        pkgProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
        pkgArgs = "/c " & Directory.GetCurrentDirectory() & "\temp.bat"
        pkgProc.StartInfo.Arguments = pkgArgs
        pkgProc.StartInfo.CreateNoWindow = True
        pkgProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        pkgProc.Start()
        Do Until pkgProc.HasExited
            If pkgProc.HasExited Then
                Exit Do
            End If
        Loop
        If Decimal.ToInt32(pkgProc.ExitCode) = 0 Then
            RemPackage.CheckedListBox1.Items.Clear()
            RemPackage.CheckedListBox2.Items.Clear()
            Debug.WriteLine("[INFO] Package names were successfully gathered. The program should return to normal state")
            Debug.WriteLine("Listing package names:" & CrLf & My.Computer.FileSystem.ReadAllText(".\pkgnames"))
            Dim pkgNames As New RichTextBox
            pkgNames.Text = My.Computer.FileSystem.ReadAllText(".\pkgnames")
            For x = 0 To pkgNames.Lines.Count - 1
                If pkgNames.Lines(x) = "" Then
                    Continue For
                End If
                ListBox1.Items.Add(pkgNames.Lines(x).Replace("Package Identity : ", "").Trim())
            Next
            File.Delete(".\temp.bat")
            File.Delete(".\pkgnames")
        Else
            Debug.WriteLine("[FAIL] Package names were not gathered. Please verify everything's working")
        End If
        Label3.Visible = False
        OK_Button.Enabled = True
        Cancel_Button.Enabled = True
    End Sub
End Class
