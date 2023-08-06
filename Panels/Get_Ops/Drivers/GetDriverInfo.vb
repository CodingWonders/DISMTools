Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports System.Threading

Public Class GetDriverInfo

    Dim DriverInfoList As New List(Of DismDriverCollection)

    Dim CurrentHWTarget As Integer

    Dim ButtonTT As New ToolTip()

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub GetDriverInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ListBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ListBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListBox1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        ListBox1.Items.Add(OpenFileDialog1.FileName)
        Button3.Enabled = True
        GetDriverInformation()
    End Sub

    Private Sub InstalledDriverLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles InstalledDriverLink.LinkClicked
        MenuPanel.Visible = False
        DriverInfoPanel.Visible = True
        InfoFromInstalledDrvsPanel.Visible = True
        InfoFromDrvPackagesPanel.Visible = False
    End Sub

    Private Sub DriverFileLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles DriverFileLink.LinkClicked
        MenuPanel.Visible = False
        DriverInfoPanel.Visible = True
        InfoFromInstalledDrvsPanel.Visible = False
        InfoFromDrvPackagesPanel.Visible = True
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MenuPanel.Visible = True
        DriverInfoPanel.Visible = False
    End Sub

    Sub GetDriverInformation()
        DriverInfoList.Clear()
        Try
            Label5.Text = "Preparing driver information processes..."
            Application.DoEvents()
            DismApi.Initialize(DismLogLevel.LogErrors)
            Using imgSession As DismSession = DismApi.OpenOfflineSession(MainForm.MountDir)
                For Each drvFile In ListBox1.Items
                    If File.Exists(drvFile) Then
                        Label5.Text = "Getting information from driver file " & Quote & Path.GetFileName(drvFile) & Quote & "..." & CrLf & "This may take some time and the program may temporarily freeze"
                        Application.DoEvents()
                        Dim drvInfoCollection As DismDriverCollection = DismApi.GetDriverInfo(imgSession, drvFile)
                        If drvInfoCollection.Count > 0 Then DriverInfoList.Add(drvInfoCollection)
                    End If
                Next
            End Using
        Catch ex As Exception
            ' Cancel it
        Finally
            DismApi.Shutdown()
        End Try
        Label5.Text = "Ready"
    End Sub

    Sub DisplayDriverInformation(HWTarget As Integer)
        Dim CurrentDriverCollection As DismDriverCollection = DriverInfoList(ListBox1.SelectedIndex)
        For Each DriverPackageInfo As DismDriver In CurrentDriverCollection
            If CurrentDriverCollection.IndexOf(DriverPackageInfo) = HWTarget Then
                Label9.Text = DriverPackageInfo.HardwareDescription
                Label11.Text = DriverPackageInfo.HardwareId
                Label14.Text = DriverPackageInfo.CompatibleIds
                Label15.Text = DriverPackageInfo.ExcludeIds
                Label18.Text = DriverPackageInfo.ManufacturerName
                If Label14.Text = "" Then Label14.Text = "None declared by the hardware manufacturer"
                If Label15.Text = "" Then Label15.Text = "None declared by the hardware manufacturer"
                Exit For
            End If
        Next
    End Sub

    Private Sub ListBox1_DragEnter(sender As Object, e As DragEventArgs) Handles ListBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ListBox1_DragDrop(sender As Object, e As DragEventArgs) Handles ListBox1.DragDrop
        Dim PackageFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each PackageFile In PackageFiles
            If Path.GetExtension(PackageFile).EndsWith("inf", StringComparison.OrdinalIgnoreCase) Then
                ListBox1.Items.Add(PackageFile)
            End If
        Next
        Button3.Enabled = True
        GetDriverInformation()
    End Sub

    Private Sub GetDriverInfo_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.SelectedItems.Count = 1 Then
            NoDrvPanel.Visible = False
            DrvPackageInfoPanel.Visible = True
            Button2.Enabled = True
            Label7.Text = "Hardware target 1 of " & DriverInfoList(ListBox1.SelectedIndex).Count
            CurrentHWTarget = 1
            Button4.Enabled = False
            DisplayDriverInformation(1)
        Else
            NoDrvPanel.Visible = True
            DrvPackageInfoPanel.Visible = False
            Button2.Enabled = False
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DriverInfoList.RemoveAt(ListBox1.SelectedIndex)
        ListBox1.Items.Remove(ListBox1.SelectedItem)
        NoDrvPanel.Visible = True
        DrvPackageInfoPanel.Visible = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        DriverInfoList.Clear()
        ListBox1.Items.Clear()
        Button3.Enabled = False
        NoDrvPanel.Visible = True
        DrvPackageInfoPanel.Visible = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If CurrentHWTarget > 1 Then
            DisplayDriverInformation(CurrentHWTarget - 1)
            CurrentHWTarget -= 1
            Label7.Text = "Hardware target " & CurrentHWTarget & " of " & DriverInfoList(ListBox1.SelectedIndex).Count
            Button5.Enabled = True
            If CurrentHWTarget = 1 Then Button4.Enabled = False
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If CurrentHWTarget < DriverInfoList(ListBox1.SelectedIndex).Count Then
            DisplayDriverInformation(CurrentHWTarget + 1)
            CurrentHWTarget += 1
            Label7.Text = "Hardware target " & CurrentHWTarget & " of " & DriverInfoList(ListBox1.SelectedIndex).Count
            Button4.Enabled = True
            If CurrentHWTarget = DriverInfoList(ListBox1.SelectedIndex).Count Then Button5.Enabled = False
        End If
    End Sub

    Private Sub Button4_MouseHover(sender As Object, e As EventArgs) Handles Button4.MouseHover
        ButtonTT.SetToolTip(sender, "Previous hardware target")
    End Sub

    Private Sub Button5_MouseHover(sender As Object, e As EventArgs) Handles Button5.MouseHover
        ButtonTT.SetToolTip(sender, "Next hardware target")
    End Sub

    Private Sub Button6_MouseHover(sender As Object, e As EventArgs) Handles Button6.MouseHover
        ButtonTT.SetToolTip(sender, "Jump to specific hardware target")
    End Sub
End Class
