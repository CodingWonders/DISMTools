Imports System.Windows.Forms
Imports System.IO
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.ControlChars

Public Class ImgIndexDelete

    Public IndexPositions(65535) As String
    Public IndexNames(65535) As String

    Public IndexRemovalNames(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Array.Clear(IndexRemovalNames, 0, IndexRemovalNames.Last)
        Dim imageCount As Integer = ListView1.CheckedItems.Count
        ' Detect whether volume indexes have been marked for removal
        If ListView1.CheckedItems.Count <= 0 Then
            MsgBox("Please select volume images to remove from this image, and try again.", vbOKOnly + vbCritical, "Remove a volume image")
            Exit Sub
        End If
        ProgressPanel.imgIndexDeletionSourceImg = TextBox1.Text
        ' Detect whether image is mounted
        ProgressPanel.imgIndexDeletionUnmount = False
        If MainForm.MountedImageImgFiles.Contains(TextBox1.Text) Then
            If MsgBox("The program has detected that this image is mounted. In order to remove volume images from a file, it needs to be unmounted. You can remount it later, if you want." & vbCrLf & vbCrLf & "Do note that this will unmount the image without saving changes. Make sure all your changes have been saved before proceeding." & vbCrLf & vbCrLf & "Do you want to unmount this image?", vbYesNo + vbExclamation, "Remove a volume image") = MsgBoxResult.Yes Then
                Try
                    For x = 0 To Array.LastIndexOf(MainForm.MountedImageImgFiles, MainForm.MountedImageImgFiles.Last)
                        If MainForm.MountedImageImgFiles(x) = TextBox1.Text Then
                            ProgressPanel.imgIndexDeletionUnmount = True
                            ProgressPanel.UMountImgIndex = MainForm.MountedImageImgIndexes(x)
                            If MainForm.MountedImageMountDirs(x) = MainForm.MountDir Then ProgressPanel.UMountLocalDir = True Else ProgressPanel.UMountLocalDir = False
                            ProgressPanel.MountDir = MainForm.MountedImageMountDirs(x)
                            ProgressPanel.UMountOp = 1
                            Exit For
                        End If
                    Next
                Catch ex As Exception
                    Exit Try
                End Try
            Else
                Exit Sub
            End If
        End If
        For x = 0 To ListView1.CheckedItems.Count - 1
            IndexRemovalNames(x) = ListView1.CheckedItems(x).SubItems(1).Text
        Next
        For x = 0 To IndexRemovalNames.Length - 1
            ProgressPanel.imgIndexDeletionNames(x) = IndexRemovalNames(x)
        Next
        ProgressPanel.imgIndexDeletionLastName = ListView1.CheckedItems(imageCount - 1).SubItems(1).Text.Replace("{ListViewSubItem: {", "").Trim().Replace("}}", "").Trim()
        imageCount = ListView1.CheckedItems.Count
        ProgressPanel.imgIndexDeletionCount = imageCount
        If CheckBox1.Checked Then
            ProgressPanel.imgIndexDeletionIntCheck = True
        Else
            ProgressPanel.imgIndexDeletionIntCheck = False
        End If
        ProgressPanel.OperationNum = 9
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgIndexDelete_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
        If MainForm.SourceImg = "N/A" Then Button2.Enabled = False Else Button2.Enabled = True
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
            ListView2.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
            ListView2.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        ListView2.ForeColor = ForeColor

        ' Set disabled ListView's backcolor. Source: https://stackoverflow.com/questions/17461902/changing-background-color-of-listview-c-sharp-when-disabled
        Dim bm As New Bitmap(ListView2.ClientSize.Width, ListView2.ClientSize.Height)
        Graphics.FromImage(bm).Clear(ListView2.BackColor)
        ListView2.BackgroundImage = bm
    End Sub

    Sub GetImageIndexInfo(SourceImage As String)
        RemoveHandler ListView1.ItemChecked, AddressOf ListView1_ItemChecked
        ' Clear arrays
        Array.Clear(IndexNames, 0, IndexNames.Length)
        Array.Clear(IndexPositions, 0, IndexPositions.Length)
        ListView1.Items.Clear()
        ListView2.Items.Clear()
        Label4.Visible = True
        Dim IndexGetter As New Process
        IndexGetter.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
        IndexGetter.StartInfo.CreateNoWindow = True
        IndexGetter.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
        Directory.CreateDirectory(".\tempinfo")
        ' First, detect if the image has more than 1 index
        Try
            File.WriteAllText(".\bin\exthelpers\indexnumbers.bat", _
                              "@echo off" & CrLf & _
                              "dism /english /get-imageinfo /imagefile=" & SourceImage & " | find /c " & Quote & "Index : " & Quote & " > .\tempinfo\indexcount")
        Catch ex As Exception
            MsgBox("Could not get image indexes. Reason: " & ex.Message, vbOKOnly + vbCritical, "Remove a volume image")
            Label4.Visible = False
            Exit Sub
        End Try
        IndexGetter.StartInfo.Arguments = "/c " & Directory.GetCurrentDirectory() & "\bin\exthelpers\indexnumbers.bat"
        IndexGetter.Start()
        Do Until IndexGetter.HasExited
            If IndexGetter.HasExited Then
                Exit Do
            End If
        Loop
        If IndexGetter.ExitCode = 0 Then
            Dim IndexCount As Integer = CInt(My.Computer.FileSystem.ReadAllText(".\tempinfo\indexcount"))
            File.Delete(".\bin\exthelpers\indexnumbers.bat")
            File.Delete(".\tempinfo\indexcount")
            If IndexCount = 1 Then
                MsgBox("This image only contains 1 index. You can't remove volume images from this file", vbOKOnly + vbExclamation, "Remove a volume image")
                Label4.Visible = False
                Exit Sub
            End If
        End If
        Try
            File.WriteAllText(".\bin\exthelpers\indexes.bat", _
                              "@echo off" & CrLf & _
                              "dism /english /get-imageinfo /imagefile=" & SourceImage & " | findstr /c:" & Quote & "Index : " & Quote & " > .\tempinfo\indexnums" & CrLf & _
                              "dism /english /get-imageinfo /imagefile=" & SourceImage & " | findstr /c:" & Quote & "Name : " & Quote & " > .\tempinfo\indexnames", _
                              ASCII)
        Catch ex As Exception
            MsgBox("Could not get image indexes. Reason: " & ex.Message, vbOKOnly + vbCritical, "Remove a volume image")
            Label4.Visible = False
            Exit Sub
        End Try
        IndexGetter.StartInfo.Arguments = "/c " & Directory.GetCurrentDirectory() & "\bin\exthelpers\indexes.bat"
        IndexGetter.Start()
        Do Until IndexGetter.HasExited
            If IndexGetter.HasExited Then
                Exit Do
            End If
        Loop
        Dim FileGetterRTB As New RichTextBox()
        Dim TypeLookups() As String = New String(1) {"Index : ", "Name : "}
        Dim lineToAppend As String = ""
        If IndexGetter.ExitCode = 0 Then
            For Each indexFile In My.Computer.FileSystem.GetFiles(".\tempinfo", FileIO.SearchOption.SearchTopLevelOnly)
                FileGetterRTB.Clear()
                If Path.GetFileName(indexFile).StartsWith("index") Then
                    FileGetterRTB.Text = My.Computer.FileSystem.ReadAllText(indexFile)
                    For x = 0 To FileGetterRTB.Lines.Count - 1
                        If FileGetterRTB.Lines(x) = "" Then
                            Continue For
                        Else
                            If FileGetterRTB.Lines(x).StartsWith(TypeLookups(0)) Then
                                lineToAppend = FileGetterRTB.Lines(x).Replace(TypeLookups(0), "").Trim()
                                IndexPositions(x) = CInt(lineToAppend)
                            ElseIf FileGetterRTB.Lines(x).StartsWith(TypeLookups(1)) Then
                                lineToAppend = FileGetterRTB.Lines(x).Replace(TypeLookups(1), "").Trim()
                                IndexNames(x) = lineToAppend
                            End If
                        End If
                    Next
                End If
            Next
        End If
        ' IndexPositions and IndexNames go hand in hand
        Try
            For x = 0 To Array.LastIndexOf(IndexPositions, IndexPositions.Last)
                If IndexPositions(x) = "" Or IndexPositions(x) = Nothing Then
                    Exit For
                Else
                    ListView1.Items.Add(New ListViewItem(New String() {IndexPositions(x), IndexNames(x)}))
                    ListView2.Items.Add(New ListViewItem(New String() {IndexPositions(x), IndexNames(x)}))
                End If
            Next
        Catch ex As Exception
            Exit Sub
        End Try
        Label4.Visible = False
        AddHandler ListView1.ItemChecked, AddressOf ListView1_ItemChecked
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If File.Exists(TextBox1.Text) Then
            If Path.GetExtension(TextBox1.Text).Equals(".wim", StringComparison.OrdinalIgnoreCase) Or _
                Path.GetExtension(TextBox1.Text).Equals(".esd", StringComparison.OrdinalIgnoreCase) Or _
                Path.GetExtension(TextBox1.Text).Equals(".vhd", StringComparison.OrdinalIgnoreCase) Or _
                Path.GetExtension(TextBox1.Text).Equals(".vhdx", StringComparison.OrdinalIgnoreCase) Then
                GetImageIndexInfo(TextBox1.Text)
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox1.Text = MainForm.SourceImg
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub ListView1_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ListView1.ItemChecked
        ListView2.Items.Clear()
        Try
            For x = 0 To ListView1.Items.Count - 1
                If ListView1.Items(x).Checked Then
                    Continue For
                Else
                    If CInt(ListView1.Items(x).Text) - 1 < 0 Then
                        ListView2.Items.Add(New ListViewItem(New String() {CInt(ListView1.Items(x).Text) + 1, ListView1.Items(x).SubItems(1).Text}))
                    Else
                        ListView2.Items.Add(New ListViewItem(New String() {ListView1.Items(x).Text, ListView1.Items(x).SubItems(1).Text}))
                    End If
                End If
            Next
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
End Class
