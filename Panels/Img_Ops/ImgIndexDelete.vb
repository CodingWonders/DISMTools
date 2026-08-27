Imports System.Windows.Forms
Imports System.IO
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports System.Threading

Public Class ImgIndexDelete
    Implements IImageTaskDialog

    Public IndexPositions(65535) As String
    Public IndexNames(65535) As String

    Public IndexRemovalNames(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        Array.Clear(IndexRemovalNames, 0, IndexRemovalNames.Last)
        Dim imageCount As Integer = ListView1.CheckedItems.Count
        ' Detect whether volume indexes have been marked for removal
        DynaLog.LogMessage("Detecting indexes that are marked for removal...")
        If ListView1.CheckedItems.Count <= 0 Then
            DynaLog.LogMessage("No indexes have been marked for removal.")
            MsgBox(LocalizationService.ForSection("ImageIndexDelete.Validation")("SelectImages.Message"), vbOKOnly + vbCritical, LocalizationService.ForSection("ImageIndexDelete.Validation")("Remove.Volume.Image.Title"))
            Exit Sub
        End If
        ProgressPanel.imgIndexDeletionSourceImg = TextBox1.Text
        ' Detect whether image is mounted
        ProgressPanel.imgIndexDeletionUnmount = False
        If MainForm.MountedImageList.Select(Function(image) image.ImageFile).Contains(TextBox1.Text) Then
            DynaLog.LogMessage("The image selected for index removal is mounted and needs to be unmounted before proceeding with this task.")
            Dim msg As String = ""
            msg = LocalizationService.ForSection("ImageIndexDelete.Validation")("ImageMounted.Message")
            If MsgBox(msg, vbYesNo + vbExclamation, ImageTaskHeader1.ItemText) = MsgBoxResult.Yes Then
                Dim mountedImage As WindowsImage = MainForm.MountedImageList.FirstOrDefault(Function(image) image.ImageFile = TextBox1.Text)
                If mountedImage IsNot Nothing Then
                    DynaLog.LogMessage("The image has been detected. Marking for unmount...")
                    ProgressPanel.imgIndexDeletionUnmount = True
                    ProgressPanel.UMountImgIndex = mountedImage.ImageIndex
                    ProgressPanel.UMountLocalDir = (mountedImage.ImageMountDirectory = MainForm.MountDir)
                    ProgressPanel.MountDir = mountedImage.ImageMountDirectory
                    ProgressPanel.UMountOp = 1
                End If
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

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        Try
            DynaLog.LogMessage("Opening volume image removal dialog...")
            DynaLog.LogMessage("Stopping mounted image detector...")
            MainForm.StopMountedImageDetector()
            Dim ImageToProcess As WindowsImage = MainForm.MountedImageList.FirstOrDefault(Function(image) image.ImageMountDirectory = MainForm.MountDir)
            If ImageToProcess IsNot Nothing Then
                TextBox1.Text = ImageToProcess.ImageFile
            End If
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Private Sub ImgIndexDelete_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Text = LocalizationService.ForSection("ImageIndexDelete")("Remove.Volume.Image.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("ImageIndexDelete").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("ImageIndexDelete")("SourceImage.Label")
        Label3.Text = LocalizationService.ForSection("ImageIndexDelete")("Mark.VolumeImages.Message")
        Label4.Text = LocalizationService.ForSection("ImageIndexDelete")("Get.Indexes.Image.Label")
        Button1.Text = LocalizationService.ForSection("ImageIndexDelete")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("ImageIndexDelete")("Mounted.Image.Button")
        OK_Button.Text = LocalizationService.ForSection("ImageIndexDelete")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ImageIndexDelete")("Cancel.Button")
        CheckBox1.Text = LocalizationService.ForSection("ImageIndexDelete")("Integrity.CheckBox")
        ListView1.Columns(0).Text = LocalizationService.ForSection("ImageIndexDelete")("Index.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("ImageIndexDelete")("ImageName.Column")
        ListView2.Columns(0).Text = LocalizationService.ForSection("ImageIndexDelete")("Columns0.Column")
        ListView2.Columns(1).Text = LocalizationService.ForSection("ImageIndexDelete")("Columns1.Column")
        GroupBox1.Text = LocalizationService.ForSection("ImageIndexDelete")("VolumeImages.Group")
        If MainForm.SourceImg = "N/A" Or Not File.Exists(MainForm.SourceImg) Or MainForm.OnlineManagement Or MainForm.OfflineManagement Then Button2.Enabled = False Else Button2.Enabled = True
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView2.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        ListView2.ForeColor = ForeColor

        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ' Set disabled ListView's backcolor. Source: https://stackoverflow.com/questions/17461902/changing-background-color-of-listview-c-sharp-when-disabled
        Dim bm As New Bitmap(ListView2.ClientSize.Width, ListView2.ClientSize.Height)
        Graphics.FromImage(bm).Clear(ListView2.BackColor)
        ListView2.BackgroundImage = bm

        ColumnHeader1.Width = WindowHelper.ScaleLogical(41)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(254)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(41)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(254)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Sub GetImageIndexInfo(SourceImage As String)
        DynaLog.LogMessage("Preparing to get image file information...")
        DynaLog.LogMessage("Mounted image detector might be busy. Stopping it if it is...")
        MainForm.StopMountedImageDetector()
        RemoveHandler ListView1.ItemChecked, AddressOf ListView1_ItemChecked
        ' Clear arrays
        Array.Clear(IndexNames, 0, IndexNames.Length)
        Array.Clear(IndexPositions, 0, IndexPositions.Length)
        ListView1.Items.Clear()
        ListView2.Items.Clear()
        Label4.Visible = True
        Dim infoCollection As DismImageInfoCollection = Nothing
        Try
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            infoCollection = DismApi.GetImageInfo(SourceImage)
            DynaLog.LogMessage("Information collection count: " & infoCollection.Count)
        Catch ex As Exception
            DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
        Finally
            Try
                DynaLog.LogMessage("Shutting down API...")
                DismApi.Shutdown()
            Catch ex As Exception

            End Try
        End Try
        If infoCollection.Count <= 1 Then
            MsgBox(LocalizationService.ForSection("ImageIndexDelete.IndexInfo")("Image.Only.Contains.Message"), vbOKOnly + vbExclamation, LocalizationService.ForSection("ImageIndexDelete.IndexInfo")("Remove.Volume.Image.Title"))
            Label4.Visible = False
            OK_Button.Enabled = False
            Exit Sub
        Else
            DynaLog.LogMessage("Adding indexes to lists...")
            For Each indexInfo As DismImageInfo In infoCollection
                ListView1.Items.Add(New ListViewItem(New String() {indexInfo.ImageIndex, indexInfo.ImageName}))
                ListView2.Items.Add(New ListViewItem(New String() {indexInfo.ImageIndex, indexInfo.ImageName}))
            Next
        End If
        OK_Button.Enabled = True
        Label4.Visible = False
        AddHandler ListView1.ItemChecked, AddressOf ListView1_ItemChecked
        MainForm.StartMountedImageDetector()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If File.Exists(TextBox1.Text) Then
            If Path.GetExtension(TextBox1.Text).Equals(".wim", StringComparison.OrdinalIgnoreCase) Or _
                Path.GetExtension(TextBox1.Text).Equals(".esd", StringComparison.OrdinalIgnoreCase) Or _
                Path.GetExtension(TextBox1.Text).Equals(".vhd", StringComparison.OrdinalIgnoreCase) Or _
                Path.GetExtension(TextBox1.Text).Equals(".vhdx", StringComparison.OrdinalIgnoreCase) Then
                DynaLog.LogMessage("Getting information about image file " & Quote & TextBox1.Text & Quote & "...")
                GetImageIndexInfo(TextBox1.Text)
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox1.Text = MainForm.SourceImg
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub ListView1_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles ListView1.ItemChecked
        DynaLog.LogMessage("Items have changed. Every item that has not been checked in the first list stays in the second list.")
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
        If ListView2.Items.Count < 1 Then
            OK_Button.Enabled = False
        Else
            OK_Button.Enabled = True
        End If
    End Sub

    Private Sub ImgIndexDelete_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        MainForm.StartMountedImageDetector()
    End Sub
End Class
