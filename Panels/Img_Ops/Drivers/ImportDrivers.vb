Imports System.Windows.Forms
Imports System.IO
Imports DISMTools.Utilities

Public Class ImportDrivers

    Dim DIList As New List(Of DriveInfo)
    Dim ImportSourceInt As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim msg As String = ""
        If ComboBox1.SelectedItem = "" Then
            msg = "Choose an action and try again"
            MsgBox(msg, vbOKOnly + vbInformation, Label1.Text)
            Exit Sub
        Else
            Select Case ImportSourceInt
                Case 0
                    If TextBox1.Text <> "" Then
                        If TextBox1.Text = MainForm.MountDir Then
                            msg = "The import target can't be specified as the import source. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                            Exit Sub
                        End If
                    Else
                        msg = "No import source has been specified. Specify a source and try again"
                        MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                        Exit Sub
                    End If
                Case 2
                    If TextBox2.Text <> "" Then
                        If TextBox2.Text = DIList(ListView1.FocusedItem.Index).Name Then
                            msg = "The import target can't be specified as the import source. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                            Exit Sub
                        End If
                        If DIList(ListView1.FocusedItem.Index).DriveFormat <> "NTFS" Then
                            msg = "The import source needs to be a drive formatted with NTFS. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                            Exit Sub
                        End If
                        If Casters.CastDriveType(DIList(ListView1.FocusedItem.Index).DriveType) <> "Fixed" Then
                            msg = "The import source needs to be a fixed drive. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                            Exit Sub
                        End If
                        If Not File.Exists(ListView1.FocusedItem.SubItems(0).Text & "\Windows\system32\ntoskrnl.exe") Then
                            msg = "The import source doesn't contain a Windows installation. Choose a different source and try again"
                            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                            Exit Sub
                        Else
                            ' Don't support Windows Vista (incl. betas) or anything older than Vista
                            Dim sysVer As FileVersionInfo = FileVersionInfo.GetVersionInfo(ListView1.FocusedItem.SubItems(0).Text & "\Windows\system32\ntoskrnl.exe")
                            If sysVer.ProductMajorPart < 6 Or _
                               (sysVer.ProductMajorPart = 6 And sysVer.ProductMinorPart = 0) Then
                                msg = "The import source has an installation of Windows Vista or an earlier version of Windows. Choose a different source and try again"
                                MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
                                Exit Sub
                            End If
                        End If
                    Else
                        msg = "No import source has been specified. Specify a source and try again"
                        MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
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

    Private Sub ImportDrivers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Languages go here
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ComboBox1.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox1.ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ComboBox1.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox1.ForeColor = Color.Black
        End If
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        TextBox2.BackColor = BackColor
        TextBox2.ForeColor = ForeColor
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        ListView1.Items.Clear()
        DIList.Clear()
        For Each DI As DriveInfo In DIList
            If DI.IsReady Then
                ListView1.Items.Add(New ListViewItem(New String() {DI.Name, DI.VolumeLabel, Casters.CastDriveType(DI.DriveType, True), Converters.BytesToReadableSize(DI.TotalSize), Converters.BytesToReadableSize(DI.AvailableFreeSpace), DI.DriveFormat, If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), "Yes", "No"), If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), FileVersionInfo.GetVersionInfo(DI.Name & "\Windows\system32\ntoskrnl.exe").ProductVersion, "")}))
            End If
        Next
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "" Then
            Label4.Text = "Choose a source listed above to configure its settings."
            DefaultPanel.Visible = True
            WinImagePanel.Visible = False
            OfflineInstPanel.Visible = False
        Else
            Select Case ComboBox1.SelectedIndex
                Case 0
                    DefaultPanel.Visible = False
                    WinImagePanel.Visible = True
                    OfflineInstPanel.Visible = False
                Case 1
                    Label4.Text = "This source doesn't have any additional settings available."
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
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PopupImageManager.Location = Button1.PointToScreen(Point.Empty)
        If PopupImageManager.ShowDialog() = DialogResult.OK Then
            TextBox1.Text = PopupImageManager.selectedMntDir
            Label6.Visible = (TextBox1.Text = MainForm.MountDir)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ListView1.Items.Clear()
        DIList.Clear()
        DIList = DriveInfo.GetDrives().ToList()
        For Each DI As DriveInfo In DIList
            If DI.IsReady Then
                ListView1.Items.Add(New ListViewItem(New String() {DI.Name, DI.VolumeLabel, Casters.CastDriveType(DI.DriveType, True), Converters.BytesToReadableSize(DI.TotalSize), Converters.BytesToReadableSize(DI.AvailableFreeSpace), DI.DriveFormat, If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), "Yes", "No"), If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), FileVersionInfo.GetVersionInfo(DI.Name & "\Windows\system32\ntoskrnl.exe").ProductVersion, "")}))
            End If
        Next
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            For x = 0 To DIList.Count - 1
                If DIList(x).Name = ListView1.FocusedItem.SubItems(0).Text Then
                    TextBox2.Text = DIList(x).Name
                    Label8.Visible = (DIList(x).Name = MainForm.MountDir)
                End If
            Next
        End If
    End Sub
End Class
