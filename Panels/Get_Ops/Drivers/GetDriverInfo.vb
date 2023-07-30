Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports System.Threading

Public Class GetDriverInfo

    Dim DriverInfoList As New List(Of DismDriverCollection)
    Dim currentDriver As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub GetDriverInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        If BackgroundWorker1.IsBusy Then BackgroundWorker1.CancelAsync()
        While BackgroundWorker1.IsBusy
            Application.DoEvents()
            Thread.Sleep(500)
        End While
        BackgroundWorker1.RunWorkerAsync()
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

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        DriverInfoList.Clear()
        ' This code doesn't work
        Try
            DismApi.Initialize(DismLogLevel.LogErrors)
            Using imgSession As DismSession = DismApi.OpenOfflineSession(MainForm.MountDir)
                For Each drvFile In ListBox1.Items
                    If File.Exists(drvFile) Then
                        currentDriver = drvFile
                        BackgroundWorker1.ReportProgress(0)
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
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Label5.Text = "Ready"
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        Label5.Text = "Getting information from driver file " & Quote & Path.GetFileName(currentDriver) & Quote & "..."
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
        If BackgroundWorker1.IsBusy Then BackgroundWorker1.CancelAsync()
        While BackgroundWorker1.IsBusy
            Application.DoEvents()
            Thread.Sleep(500)
        End While
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub GetDriverInfo_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
    End Sub
End Class
