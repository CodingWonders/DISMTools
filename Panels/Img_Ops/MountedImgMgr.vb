Imports System.IO
Public Class MountedImgMgr

    Public ignoreRepeats As Boolean = False

    Private Sub MountedImgMgr_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        ListView1.Items.Clear()
        DetectorBW.RunWorkerAsync()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Options.TabControl1.SelectedIndex = 8
        If MainForm.WindowState = FormWindowState.Minimized Then MainForm.WindowState = FormWindowState.Normal
        Options.ShowDialog(MainForm)
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        ' Enable buttons according to the image conditions
        If ListView1.FocusedItem.Text <> "" Then
            Button1.Enabled = True
            If MainForm.MountedImageImgStatuses(ListView1.FocusedItem.Index) > 0 Then
                Button2.Enabled = True
                Select Case MainForm.MountedImageImgStatuses(ListView1.FocusedItem.Index)
                    Case 1
                        Button2.Text = "Reload servicing"
                    Case 2
                        Button2.Text = "Repair component store"
                End Select
            Else
                Button2.Enabled = False
            End If
            IIf(MainForm.MountedImageMountedReWr(ListView1.FocusedItem.Index) = 1, Button3.Enabled = True, Button3.Enabled = False)
            Button4.Enabled = True
            Button5.Enabled = True
            If MainForm.isProjectLoaded And MainForm.MountDir = "N/A" Or Not Directory.Exists(MainForm.MountDir & "\Windows") Then
                Button6.Enabled = True
            Else
                Button6.Enabled = False
            End If
        Else
            Button1.Enabled = False
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            Button5.Enabled = False
            Button6.Enabled = False
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Directory.Exists(ListView1.FocusedItem.SubItems(2).Text) Then
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", ListView1.FocusedItem.SubItems(2).Text)
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim useAlternateMethod As Boolean = False
        If MainForm.isProjectLoaded Then
            For x = 0 To ListView1.Columns.Count - 1
                If ListView1.FocusedItem.SubItems(x).Text = "" Or ListView1.FocusedItem.SubItems(x).Text = Nothing Then
                    useAlternateMethod = True
                    Exit For
                End If
            Next
        End If
        If useAlternateMethod Then
            Try
                For x = 0 To Array.LastIndexOf(MainForm.MountedImageMountDirs, MainForm.MountedImageMountDirs.Last)
                    If MainForm.MountedImageMountDirs(x) = ListView1.FocusedItem.SubItems(2).Text Then
                        MainForm.MountDir = MainForm.MountedImageMountDirs(x)
                        MainForm.ImgIndex = MainForm.MountedImageImgIndexes(x)
                        MainForm.SourceImg = MainForm.MountedImageImgFiles(x)
                        IIf(MainForm.MountedImageMountedReWr(x) = "Yes", MainForm.isReadOnly = True, MainForm.isReadOnly = False)
                    End If
                Next
            Catch ex As Exception
                Exit Try
            End Try
            MainForm.UpdateProjProperties(True, If(MainForm.isReadOnly, True, False))
            MainForm.SaveDTProj()
        Else
            MainForm.MountDir = ListView1.FocusedItem.SubItems(2).Text
            MainForm.ImgIndex = ListView1.FocusedItem.SubItems(1).Text
            MainForm.SourceImg = ListView1.FocusedItem.SubItems(0).Text
            IIf(ListView1.FocusedItem.SubItems(4).Text = "Yes", MainForm.isReadOnly = False, MainForm.isReadOnly = True)
            MainForm.UpdateProjProperties(True, If(MainForm.isReadOnly, True, False))
            MainForm.SaveDTProj()
        End If
        Button6.Enabled = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ProgressPanel.MountDir = ListView1.FocusedItem.SubItems(2).Text
        ProgressPanel.OperationNum = 18
        ProgressPanel.ShowDialog()
        Button2.Enabled = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MainForm.ImgUMountPopupCMS.Show(sender, New Point(24, Button1.Height * 0.75))
    End Sub

    Private Sub DetectorBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles DetectorBW.DoWork
        Dim timer As New Stopwatch
        Do
            timer.Start()
            Do
                If DetectorBW.CancellationPending Then
                    timer.Stop()
                    timer.Reset()
                    Exit Sub
                End If
                If timer.ElapsedMilliseconds >= 100 Then
                    timer.Stop()
                    DetectorBW.ReportProgress(0)
                    timer.Reset()
                    Exit Do
                End If
            Loop
        Loop
    End Sub

    Private Sub MountedImgMgr_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        DetectorBW.CancelAsync()
    End Sub

    Private Sub DetectorBW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles DetectorBW.ProgressChanged
        If DetectorBW.CancellationPending Then Exit Sub
        Try
            For x = 0 To Array.LastIndexOf(MainForm.MountedImageImgFiles, MainForm.MountedImageImgFiles.Last)
                If ignoreRepeats Then
                    If ListView1.Items.Count <> MainForm.MountedImageImgFiles.Length Then
                        ListView1.Items.Clear()
                        ignoreRepeats = False
                        Exit Sub
                    End If
                    ' Thanks ChatGPT for providing a little help on this
                    For Each item As ListViewItem In ListView1.Items
                        If Not MainForm.MountedImageImgFiles.Contains(item.Text) Then
                            ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "OK", If(MainForm.MountedImageImgStatuses(x) = 1, "Needs Remount", "Invalid")), If(MainForm.MountedImageMountedReWr(x) = 0, "Yes", "No"), If(File.Exists(MainForm.MountedImageMountDirs(x) & "\Windows\System32\ntoskrnl.exe"), FileVersionInfo.GetVersionInfo(MainForm.MountedImageMountDirs(x) & "\Windows\system32\ntoskrnl.exe").ProductVersion, "Could not get version info")}))
                        End If
                    Next
                End If
                If Not ignoreRepeats Then ListView1.Items.Add(New ListViewItem(New String() {MainForm.MountedImageImgFiles(x), MainForm.MountedImageImgIndexes(x), MainForm.MountedImageMountDirs(x), If(MainForm.MountedImageImgStatuses(x) = 0, "OK", If(MainForm.MountedImageImgStatuses(x) = 1, "Needs Remount", "Invalid")), If(MainForm.MountedImageMountedReWr(x) = 0, "Yes", "No"), If(File.Exists(MainForm.MountedImageMountDirs(x) & "\Windows\System32\ntoskrnl.exe"), FileVersionInfo.GetVersionInfo(MainForm.MountedImageMountDirs(x) & "\Windows\system32\ntoskrnl.exe").ProductVersion, "Could not get version info")}))
            Next
            ignoreRepeats = True
        Catch ex As Exception
            Exit Try
        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ImgIndexDelete.TextBox1.Text = ListView1.FocusedItem.SubItems(0).Text
        ImgIndexDelete.ShowDialog()
    End Sub
End Class