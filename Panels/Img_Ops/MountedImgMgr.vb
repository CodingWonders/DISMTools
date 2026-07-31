Imports System.IO
Imports System.Threading
Imports Microsoft.Dism

Public Class MountedImgMgr

    Public ignoreRepeats As Boolean = False

    Private Sub MountedImgMgr_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("MountedImgMgr")("Image.Manager.Item")
        Label1.Text = LocalizationService.ForSection("MountedImgMgr")("Overview.Images.Message")
        ListView1.Columns(0).Text = LocalizationService.ForSection("MountedImgMgr")("ImageFile.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("MountedImgMgr")("Index.Column")
        ListView1.Columns(2).Text = LocalizationService.ForSection("MountedImgMgr")("MountDirectory.Column")
        ListView1.Columns(3).Text = LocalizationService.ForSection("MountedImgMgr")("Status.Column")
        ListView1.Columns(4).Text = LocalizationService.ForSection("MountedImgMgr")("Read.Write.Column")
        Button1.Text = LocalizationService.ForSection("MountedImgMgr")("UnmountImage.Button")
        Button2.Text = LocalizationService.ForSection("MountedImgMgr")("ReloadServicing.Button")
        Button3.Text = LocalizationService.ForSection("MountedImgMgr")("Enable.Write.Button")
        Button4.Text = LocalizationService.ForSection("MountedImgMgr")("Open.Mount.Dir.Button")
        Button5.Text = LocalizationService.ForSection("MountedImgMgr")("Remove.VolumeImages.Button")
        Button6.Text = LocalizationService.ForSection("MountedImgMgr")("LoadProject.Button")
        CheckForIllegalCrossThreadCalls = False
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        ListView1.Items.Clear()
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ' Subscribe to MainForm event to get updates
        AddHandler MainForm.MountedImagesUpdated, AddressOf OnMountedImagesUpdated

        ColumnHeader1.Width = WindowHelper.ScaleLogical(480)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(72)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(420)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(60)
        ColumnHeader5.Width = WindowHelper.ScaleLogical(148)
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            ' Enable buttons according to the image conditions
            If ListView1.SelectedItems.Count > 0 Then
                Button1.Enabled = True
                Dim markedImage As WindowsImage = MainForm.MountedImageList.ElementAtOrDefault(ListView1.FocusedItem.Index)
                If markedImage Is Nothing Then Exit Sub
                If markedImage.ImageMountStatus <> DismMountStatus.Ok Then
                    Button2.Enabled = True
                    Select Case markedImage.ImageMountStatus
                        Case DismMountStatus.NeedsRemount
                            Button2.Text = LocalizationService.ForSection("MountedImgMgr")("ReloadServicing.Button")
                        Case DismMountStatus.Invalid
                            Button2.Text = LocalizationService.ForSection("MountedImgMgr")("Repair.Component.Store.Item")
                    End Select
                Else
                    Button2.Enabled = False
                End If
                Button3.Enabled = (markedImage.ImageMountMode = DismMountMode.ReadOnly)
                Button4.Enabled = True
                Button5.Enabled = True
                If MainForm.isProjectLoaded And MainForm.MountDir = "N/A" Or Not Directory.Exists(MainForm.MountDir & "\Windows") Then
                    Button6.Enabled = True
                Else
                    Button6.Enabled = False
                End If
                Button7.Enabled = True
            Else
                Button1.Enabled = False
                Button2.Enabled = False
                Button3.Enabled = False
                Button4.Enabled = False
                Button5.Enabled = False
                Button6.Enabled = False
                Button7.Enabled = False
            End If
        Catch ex As Exception
            Button1.Enabled = False
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            Button5.Enabled = False
            Button6.Enabled = False
            Button7.Enabled = False
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Directory.Exists(ListView1.FocusedItem.SubItems(2).Text) Then
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", ListView1.FocusedItem.SubItems(2).Text)
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        DynaLog.LogMessage("Preparing to load the selected image in loaded project...")
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
            Dim ImageToLoad As WindowsImage = MainForm.MountedImageList.FirstOrDefault(Function(image) image.ImageMountDirectory = ListView1.FocusedItem.SubItems(2).Text)
            If ImageToLoad IsNot Nothing Then
                MainForm.MountDir = ImageToLoad.ImageMountDirectory
                MainForm.ImgIndex = ImageToLoad.ImageIndex
                MainForm.SourceImg = ImageToLoad.ImageFile
                MainForm.isReadOnly = (ImageToLoad.ImageMountMode = DismMountMode.ReadOnly)
            End If
            MainForm.UpdateProjProperties(True, If(MainForm.isReadOnly, True, False))
            MainForm.SaveDTProj()
        Else
            MainForm.MountDir = ListView1.FocusedItem.SubItems(2).Text
            MainForm.ImgIndex = ListView1.FocusedItem.SubItems(1).Text
            MainForm.SourceImg = ListView1.FocusedItem.SubItems(0).Text
            IIf(ListView1.FocusedItem.SubItems(4).Text = LocalizationService.ForSection("MountedImgMgr")("Yes.Button"), MainForm.isReadOnly = False, MainForm.isReadOnly = True)
            MainForm.UpdateProjProperties(True, If(MainForm.isReadOnly, True, False))
            MainForm.SaveDTProj()
        End If
        Button6.Enabled = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        DynaLog.LogMessage("Checking status of the selected mount image...")
        Dim SelectedImage As WindowsImage = MainForm.MountedImageList.ElementAtOrDefault(ListView1.FocusedItem.Index)
        If SelectedImage IsNot Nothing Then
            Select Case SelectedImage.ImageMountStatus
                Case DismMountStatus.NeedsRemount
                    DynaLog.LogMessage("The selected image needs to be remounted.")
                    ProgressPanel.MountDir = ListView1.FocusedItem.SubItems(2).Text
                    ProgressPanel.OperationNum = 18
                    ProgressPanel.ShowDialog(Me)
                    Button2.Enabled = False
                Case DismMountStatus.Invalid
                    DynaLog.LogMessage("The selected image needs to be repaired.")
                    Visible = False
                    ImgCleanup.ComboBox1.SelectedIndex = 6
                    ImgCleanup.ShowDialog(MainForm)
                    Visible = True
            End Select
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DynaLog.LogMessage("Determining if changes can be written to the selected Windows image...")
        Select Case MainForm.MountedImageList(ListView1.FocusedItem.Index).ImageMountMode
            Case DismMountMode.ReadWrite
                DynaLog.LogMessage("The image has been mounted with read-write permissions.")
                MainForm.ImgUMountPopupCMS.Show(sender, New Point(24, Button1.Height * 0.75))
            Case DismMountMode.ReadOnly
                DynaLog.LogMessage("The image has been mounted with read-only permissions. No tasks other than unmounting whilst discarding changes can be made.")
                ' Unmount the image discarding changes
                If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
                ProgressPanel.OperationNum = 21
                ProgressPanel.UMountLocalDir = False
                ProgressPanel.RandomMountDir = ListView1.FocusedItem.SubItems(2).Text   ' Hope there isn't anything to set here
                ProgressPanel.UMountImgIndex = ListView1.FocusedItem.SubItems(1).Text
                ProgressPanel.MountDir = ""
                ProgressPanel.UMountOp = 1
                ProgressPanel.ShowDialog(Me)
        End Select
    End Sub

    Private Sub MountedImgMgr_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        RemoveHandler MainForm.MountedImagesUpdated, AddressOf OnMountedImagesUpdated
    End Sub

    Private Sub OnMountedImagesUpdated(sender As Object, e As EventArgs)
        Try
            ' Force a refresh of the ListView on the UI thread
            If InvokeRequired Then
                BeginInvoke(New MethodInvoker(AddressOf RefreshMountedList))
            Else
                RefreshMountedList()
            End If
        Catch ex As Exception
            DynaLog.LogMessage("OnMountedImagesUpdated error: " & ex.Message)
        End Try
    End Sub

    Private Sub RefreshMountedList()
        If ListView1.Items.Count <> MainForm.MountedImageList.Count Then
            DynaLog.LogMessage("There is a different amount of images mounted now. Forcing refresh of lists...")
            Button1.Enabled = False
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            Button5.Enabled = False
            Button6.Enabled = False
            Button7.Enabled = False
            Try
                ListView1.Items.Clear()
                For Each MountedImage In MainForm.MountedImageList
                    ListView1.Items.Add(New ListViewItem(New String() {MountedImage.ImageFile, MountedImage.ImageIndex, MountedImage.ImageMountDirectory, MountedImage.MountStatusToString(), MountedImage.MountModeToString()}))
                Next
                ignoreRepeats = True
            Catch ex As Exception
                DynaLog.LogMessage("RefreshMountedList error: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        DynaLog.LogMessage("Preparing to remove volume images from selected image file...")
        DynaLog.LogMessage("Mounted image detector might be busy. Stopping it if it is...")
        MainForm.StopMountedImageDetector()
        ImgIndexDelete.TextBox1.Text = ListView1.FocusedItem.SubItems(0).Text
        ImgIndexDelete.ShowDialog(Me)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If MainForm.MountedImageList.Select(Function(image) image.ImageMountDirectory).Count > 0 Then
            DynaLog.LogMessage("Enabling write permissions on the selected image...")
            MainForm.EnableWritePermissions(ListView1.FocusedItem.SubItems(0).Text, CInt(ListView1.FocusedItem.SubItems(1).Text), ListView1.FocusedItem.SubItems(2).Text)
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        DynaLog.LogMessage("Showing special tasks...")
        MainForm.ImgSpecialToolsCMS.Show(sender, New Point(8, Button7.Height * 0.75))
    End Sub
End Class
