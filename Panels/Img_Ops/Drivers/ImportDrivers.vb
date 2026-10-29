Imports System.Windows.Forms
Imports System.IO
Imports DISMTools.Utilities
Imports Microsoft.Dism

Public Class ImportDrivers
    Implements IImageTaskDialog

    Dim DIList As New List(Of DriveInfo)
    Dim ImportSourceInt As Integer = -1
    Dim ImportSources() As String = New String(2) {"", "", ""}

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        If ImportSourceInt < 0 Then Exit Sub
        Dim msg As String = ""
        If ComboBox1.SelectedItem = "" Then
            DynaLog.LogMessage("No source has been selected.")
            msg = "Choose an action and try again"
            MsgBox(msg, vbOKOnly + vbInformation, ImageTaskHeader1.ItemText)
            Exit Sub
        Else
            DynaLog.LogMessage("A source has been selected. Verifying inputs...")
            If ListView1.SelectedItems.Count = 1 Then
                If DIList(ListView1.FocusedItem.Index).Name = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) Then ImportSourceInt = 1
            End If
            DynaLog.LogMessage("Import source flag: " & ImportSourceInt)
            Select Case ImportSourceInt
                Case 0
                    DynaLog.LogMessage("Validating import source...")
                    If TextBox1.Text <> "" Then
                        DynaLog.LogMessage("An import source has been specified.")
                        DynaLog.LogMessage("Source: " & TextBox1.Text)
                        If TextBox1.Text = MainForm.MountDir Then
                            DynaLog.LogMessage("The import source is the same as the import target.")
                            msg = "The import target can't be specified as the import source. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                            Exit Sub
                        End If
                    Else
                        DynaLog.LogMessage("No import source has been specified.")
                        msg = "No import source has been specified. Specify a source and try again"
                        MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Exit Sub
                    End If
                Case 1
                    DynaLog.LogMessage("Validating import source...")
                    If MainForm.OnlineManagement Then
                        msg = "The import target can't be specified as the import source. Choose a different source and try again"
                        MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Exit Sub
                    End If
                Case 2
                    DynaLog.LogMessage("Validating import source...")
                    DynaLog.LogMessage("Source: " & TextBox2.Text)
                    If TextBox2.Text <> "" Then
                        DynaLog.LogMessage("An import source has been specified.")
                        DynaLog.LogMessage("Checking drive letter...")
                        If TextBox2.Text = DIList(ListView1.FocusedItem.Index).Name Then
                            DynaLog.LogMessage("The import source is the same as the import target.")
                            msg = "The import target can't be specified as the import source. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                            Exit Sub
                        End If
                        DynaLog.LogMessage("Checking drive format...")
                        If DIList(ListView1.FocusedItem.Index).DriveFormat <> "NTFS" Then
                            DynaLog.LogMessage("The source is not formatted with NTFS.")
                            msg = "The import source needs to be a drive formatted with NTFS. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                            Exit Sub
                        End If
                        DynaLog.LogMessage("Checking Windows installation in the drive...")
                        If Not File.Exists(ListView1.FocusedItem.SubItems(0).Text & "\Windows\system32\ntoskrnl.exe") Then
                            DynaLog.LogMessage("The source drive does not contain ntoskrnl. There is either an utterly broken Windows installation or no installation at all.")
                            msg = "The import source doesn't contain a Windows installation. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                            Exit Sub
                        Else
                            DynaLog.LogMessage("The source drive contains ntoskrnl. Checking version...")
                            ' Don't support Windows Vista (incl. betas) or anything older than Vista
                            Dim sysVer As FileVersionInfo = FileVersionInfo.GetVersionInfo(ListView1.FocusedItem.SubItems(0).Text & "\Windows\system32\ntoskrnl.exe")
                            If sysVer.ProductMajorPart < 6 Or _
                               (sysVer.ProductMajorPart = 6 And sysVer.ProductMinorPart = 0) Then
                                DynaLog.LogMessage("The import source contains Windows Vista or an earlier version of Windows.")
                                msg = "The import source has an installation of Windows Vista or an earlier version of Windows. Choose a different source and try again"
                                MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                                Exit Sub
                            End If
                        End If
                    Else
                        DynaLog.LogMessage("No import source has been specified.")
                        msg = "No import source has been specified. Specify a source and try again"
                        MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Exit Sub
                    End If
            End Select
        End If
        ProgressPanel.ImportSourceInt = ImportSourceInt
        ProgressPanel.DrvImport_SourceImage = TextBox1.Text
        ProgressPanel.DrvImport_SourceDisk = TextBox2.Text
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 78
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        Return True
    End Function

    Private Sub ImportDrivers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        ComboBox1.Items.Clear()
        ComboBox1.SelectedText = ""
        Text = LocalizationService.ForSection("ImportDrivers")("Title.Label")
        ImageTaskHeader1.ItemText = Text
        Label2.Text = LocalizationService.ForSection("ImportDrivers")("Process.Third.Message")
        Label3.Text = LocalizationService.ForSection("ImportDrivers")("ImportSource.Label")
        Label4.Text = If(ImportSourceInt = 1, LocalizationService.ForSection("ImportDrivers")("Source.Doesn.Tany.Label"), LocalizationService.ForSection("ImportDrivers")("Source.Listed.Choose.Label"))
        Label5.Text = LocalizationService.ForSection("ImportDrivers")("Windows.Label")
        Label6.Text = LocalizationService.ForSection("ImportDrivers")("Tuse.Target.Label")
        Label7.Text = LocalizationService.ForSection("ImportDrivers")("Offline.Drivers.Label")
        Label8.Text = LocalizationService.ForSection("ImportDrivers")("Tuse.Target.Label")
        Label9.Text = LocalizationService.ForSection("ImportDrivers")("ImageFile.Label")
        Button1.Text = LocalizationService.ForSection("ImportDrivers")("Pick.Button")
        Button2.Text = LocalizationService.ForSection("ImportDrivers")("Refresh.Button")
        OK_Button.Text = LocalizationService.ForSection("ImportDrivers")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ImportDrivers")("Cancel.Button")
        ListView1.Columns(0).Text = LocalizationService.ForSection("ImportDrivers")("DriveLetter.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("ImportDrivers")("DriveLabel.Column")
        ListView1.Columns(2).Text = LocalizationService.ForSection("ImportDrivers")("DriveType.Column")
        ListView1.Columns(3).Text = LocalizationService.ForSection("ImportDrivers")("TotalSize.Column")
        ListView1.Columns(4).Text = LocalizationService.ForSection("ImportDrivers")("Available.Free.Space.Column")
        ListView1.Columns(5).Text = LocalizationService.ForSection("ImportDrivers")("DriveFormat.Column")
        ListView1.Columns(6).Text = LocalizationService.ForSection("ImportDrivers")("ContainsWindows.Column")
        ListView1.Columns(7).Text = LocalizationService.ForSection("ImportDrivers")("Windows.Column")
        ImportSources(0) = LocalizationService.ForSection("ImportDrivers")("Windows.Item")
        ImportSources(1) = LocalizationService.ForSection("ImportDrivers")("Online.Install.Item")
        ImportSources(2) = LocalizationService.ForSection("ImportDrivers")("Offline.Install.Item")
        ComboBox1.Items.AddRange(ImportSources)
        If ImportSourceInt >= 0 Then ComboBox1.SelectedIndex = ImportSourceInt
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        TextBox2.BackColor = BackColor
        TextBox2.ForeColor = ForeColor
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        DynaLog.LogMessage("Getting disks...")
        ListView1.Items.Clear()
        DIList.Clear()
        DIList = DriveInfo.GetDrives().Where(Function(disk) disk.IsReady).ToList()
        ListView1.Items.AddRange(DIList.Select(Function(DI) New ListViewItem(New String() {DI.Name,
                                                                                           DI.VolumeLabel,
                                                                                           Casters.CastDriveType(DI.DriveType, True),
                                                                                           Converters.BytesToReadableSize(DI.TotalSize),
                                                                                           Converters.BytesToReadableSize(DI.AvailableFreeSpace),
                                                                                           DI.DriveFormat,
                                                                                           If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), "Yes", "No"),
                                                                                           If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"),
                                                                                              FileVersionInfo.GetVersionInfo(DI.Name & "\Windows\system32\ntoskrnl.exe").ProductVersion, "")})).ToArray())
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ColumnHeader1.Width = WindowHelper.ScaleLogical(68)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(128)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(70)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(94)
        ColumnHeader5.Width = WindowHelper.ScaleLogical(110)
        ColumnHeader6.Width = WindowHelper.ScaleLogical(77)
        ColumnHeader7.Width = WindowHelper.ScaleLogical(110)
        ColumnHeader8.Width = WindowHelper.ScaleLogical(104)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "" Then
            DefaultPanel.Visible = True
            WinImagePanel.Visible = False
            OfflineInstPanel.Visible = False
            ImportSourceInt = -1
        Else
            Select Case ComboBox1.SelectedIndex
                Case 0
                    DefaultPanel.Visible = False
                    WinImagePanel.Visible = True
                    OfflineInstPanel.Visible = False
                Case 1
                    DefaultPanel.Visible = True
                    WinImagePanel.Visible = False
                    OfflineInstPanel.Visible = False
                Case 2
                    DefaultPanel.Visible = False
                    WinImagePanel.Visible = False
                    OfflineInstPanel.Visible = True
            End Select
            ImportSourceInt = ComboBox1.SelectedIndex
        End If
        Label4.Text = If(ImportSourceInt = 1, LocalizationService.ForSection("ImportDrivers")("Source.Doesn.Tany.Label"), LocalizationService.ForSection("ImportDrivers")("Source.Listed.Choose.Label"))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim selectedImage As WindowsImage = PopupMountedImagePicker.PickImage()
        If selectedImage IsNot Nothing Then
            DynaLog.LogMessage("Information will be obtained from the popup mounted image manager...")
            TextBox1.Text = selectedImage.ImageMountDirectory
            Label6.Visible = (TextBox1.Text = MainForm.MountDir)
            Label10.Text = selectedImage.ImageFile
            Label10.Visible = (TextBox1.Text <> "" And Directory.Exists(TextBox1.Text))
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DynaLog.LogMessage("Refreshing disk listings...")
        ListView1.Items.Clear()
        DIList.Clear()
        DIList = DriveInfo.GetDrives().Where(Function(disk) disk.IsReady).ToList()
        ListView1.Items.AddRange(DIList.Select(Function(DI) New ListViewItem(New String() {DI.Name,
                                                                                           DI.VolumeLabel,
                                                                                           Casters.CastDriveType(DI.DriveType, True),
                                                                                           Converters.BytesToReadableSize(DI.TotalSize),
                                                                                           Converters.BytesToReadableSize(DI.AvailableFreeSpace),
                                                                                           DI.DriveFormat,
                                                                                           If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), "Yes", "No"),
                                                                                           If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"),
                                                                                              FileVersionInfo.GetVersionInfo(DI.Name & "\Windows\system32\ntoskrnl.exe").ProductVersion, "")})).ToArray())
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            DynaLog.LogMessage("Checking selected item...")
            For x = 0 To DIList.Count - 1
                If DIList(x).Name = ListView1.FocusedItem.SubItems(0).Text Then
                    TextBox2.Text = DIList(x).Name
                    Label8.Visible = (DIList(x).Name = MainForm.MountDir)
                    If DIList(x).Name = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) Then ComboBox1.SelectedIndex = 1
                End If
            Next
        End If
    End Sub
End Class
