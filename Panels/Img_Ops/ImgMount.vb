Imports System.IO
Imports System.Windows.Forms
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.ControlChars

Public Class ImgMount

    Dim WimInfo As Process
    Dim WimStr As String
    Dim IsReqField1Valid As Boolean
    Dim IsReqField2Valid As Boolean
    Dim IsReqField3Valid As Boolean
    Public SourceImg As String
    Public ImgIndex As Integer
    Public MountDir As String
    Public isReadOnly As Boolean
    Public isOptimized As Boolean
    Public isIntegrityTested As Boolean

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        TextBox1.Text = ProgressPanel.SourceImg
        NumericUpDown1.Value = ImgIndex
        TextBox2.Text = ProgressPanel.MountDir
        If CheckBox1.Checked Then
            ProgressPanel.isReadOnly = True
        Else
            ProgressPanel.isReadOnly = False
        End If
        If CheckBox3.Checked Then
            ProgressPanel.isOptimized = True
        Else
            ProgressPanel.isOptimized = False
        End If
        If CheckBox4.Checked Then
            ProgressPanel.isIntegrityTested = True
        Else
            ProgressPanel.isIntegrityTested = False
        End If
        'ProgressPanel.SourceImg = SourceImg
        ProgressPanel.ImgIndex = ImgIndex
        'ProgressPanel.MountDir = MountDir
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 15
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgMount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FileSpecDialog.ShowDialog()
    End Sub

    Private Sub FileSpecDialog_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles FileSpecDialog.FileOk
        TextBox1.Text = FileSpecDialog.FileName
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If TextBox1.Text = "" Then
            Button1.PerformClick()
        End If
        If File.Exists(TextBox1.Text) Then
            File.WriteAllText(".\bin\exthelpers\temp.bat", "dism /English /get-imageinfo /imagefile=" & TextBox1.Text & " > dismtools.txt", ASCII)
            Process.Start(".\bin\exthelpers\temp.bat").WaitForExit()
            Width = 800
            Indexes.Text = My.Computer.FileSystem.ReadAllText(".\dismtools.txt")
            File.Delete(".\bin\exthelpers\temp.bat")
            File.Delete(".\dismtools.txt")
        Else
            Exit Sub
        End If
        Width = 1148
        CenterToParent()
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Indexes.Clear()
        Width = 800
        CenterToParent()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        FolderBrowserDialog1.ShowDialog()
        If DialogResult.OK Then
            TextBox2.Text = FolderBrowserDialog1.SelectedPath
        Else
            TextBox2.Text = ""
        End If
        GetFields()
    End Sub

    Sub GetMaxIndexCount(ImgFile As String)
        File.WriteAllText(".\bin\exthelpers\temp.bat", _
                          "@echo off" & CrLf & _
                          "dism /English /get-imageinfo /imagefile=" & ImgFile & " | find /c " & Quote & "Index" & Quote & " > .\indexcount", ASCII)
        Process.Start(".\bin\exthelpers\temp.bat").WaitForExit()
        MainForm.imgIndexCount = CInt(My.Computer.FileSystem.ReadAllText(".\indexcount"))
        NumericUpDown1.Maximum = MainForm.imgIndexCount
        File.Delete(".\indexcount")
    End Sub

    Sub GetFields()
        IsReqField3Valid = True
        If TextBox1.Text = "" Then
            If ProgressPanel.OperationNum = 15 Then
                TextBox1.Text = ProgressPanel.SourceImg
            Else
                IsReqField1Valid = False
            End If
        Else
            If File.Exists(TextBox1.Text) Then
                IsReqField1Valid = True
                ProgressPanel.SourceImg = TextBox1.Text
                GetMaxIndexCount(TextBox1.Text)
            Else
                IsReqField1Valid = False
            End If
        End If
        If TextBox2.Text = "" Then
            If ProgressPanel.OperationNum = 15 Then
                TextBox2.Text = ProgressPanel.MountDir
            Else
                IsReqField1Valid = False
            End If
            IsReqField2Valid = False
        Else
            If Directory.Exists(TextBox2.Text) Then
                IsReqField2Valid = True
                ProgressPanel.MountDir = TextBox2.Text
            Else
                Try
                    Directory.CreateDirectory(TextBox2.Text)
                    IsReqField2Valid = True
                Catch ex As Exception
                    IsReqField2Valid = False
                End Try
            End If
        End If
        If IsReqField1Valid And IsReqField2Valid And IsReqField3Valid Then
            OK_Button.Enabled = True
        Else
            OK_Button.Enabled = False
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        GetFields()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        GetFields()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If ProgressPanel.OperationNum = 0 Then
            If ProgressPanel.projPath = "" Then
                TextBox2.Text = MainForm.projPath & "\mount"
            Else
                TextBox2.Text = ProgressPanel.projPath & ProgressPanel.projName & "\mount"
            End If
        Else
            TextBox2.Text = MainForm.projPath & "\mount"
        End If
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        ImgIndex = NumericUpDown1.Value
        ProgressPanel.ImgIndex = NumericUpDown1.Value
    End Sub
End Class
