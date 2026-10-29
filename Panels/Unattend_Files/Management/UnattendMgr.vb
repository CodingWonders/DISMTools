Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class UnattendMgr

    Private Sub OK_Button_Click(sender As Object, e As EventArgs)
        DialogResult = Windows.Forms.DialogResult.OK
        Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            If FolderBrowserDialog1.SelectedPath.Contains("unattend_xml") Then
                TextBox1.Text = FolderBrowserDialog1.SelectedPath
            Else
                TextBox1.Text = Path.Combine(FolderBrowserDialog1.SelectedPath, "unattend_xml")
            End If
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" AndAlso Directory.Exists(TextBox1.Text) Then
            ScanForUnattendFiles(TextBox1.Text)
        End If
    End Sub

    Sub ScanForUnattendFiles(folderPath As String)
        DynaLog.LogMessage("Preparing to scan specified folder...")
        DynaLog.LogMessage("- Specified folder for scan process: " & Quote & folderPath & Quote)
        Dim UnattendFiles() As String
        Dim errorMsg As String = ""
        ListView1.Items.Clear()
        Try
            DynaLog.LogMessage("Checking if the folder exists...")
            If Directory.Exists(folderPath) Then
                DynaLog.LogMessage("The folder exists. Checking absolute path...")
                If folderPath.Contains("unattend_xml") Then
                    DynaLog.LogMessage("The folder is part of a DISMTools project.")
                    UnattendFiles = Directory.GetFiles(folderPath, "*.xml", SearchOption.AllDirectories)
                Else
                    If Directory.Exists(Path.Combine(folderPath, "unattend_xml")) Then
                        DynaLog.LogMessage("The folder is a DISMTools project folder that contains the unattended answer file collection.")
                        UnattendFiles = Directory.GetFiles(Path.Combine(folderPath, "unattend_xml"), "*.xml", SearchOption.AllDirectories)
                    Else
                        DynaLog.LogMessage("This folder is a regular directory.")
                        UnattendFiles = Directory.GetFiles(folderPath, "*.xml", SearchOption.AllDirectories)
                    End If
                End If
            Else
                DynaLog.LogMessage("The folder does not exist.")
                errorMsg = LocalizationService.ForSection("Unattend.Scan")("FolderMissing.Message")
                Throw New Exception(errorMsg)
            End If
            If UnattendFiles.Length > 0 Then
                DynaLog.LogMessage("Unattended answer files have been detected. Showing...")
                For Each xmlFile In UnattendFiles
                    ListView1.Items.Add(New ListViewItem(New String() {Path.GetFileName(xmlFile), File.GetCreationTime(xmlFile), File.GetLastWriteTime(xmlFile), File.GetLastAccessTime(xmlFile)}))
                Next
            Else
                DynaLog.LogMessage("No unattended answer files have been detected.")
                errorMsg = LocalizationService.ForSection("Unattend.Scan")("ElementsFound.Message")
                Throw New Exception(errorMsg)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbCritical, Text)
        End Try
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        ActionsTLP.Enabled = (ListView1.SelectedItems.Count = 1)
        Button4.Enabled = (MainForm.isProjectLoaded And Not (MainForm.OnlineManagement Or MainForm.OfflineManagement))
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DynaLog.LogMessage("Opening selected answer file...")
        Process.Start(Path.Combine(TextBox1.Text, ListView1.FocusedItem.SubItems(0).Text))
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        DynaLog.LogMessage("Opening location of the selected answer file...")
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", "/select," & Quote & Path.Combine(TextBox1.Text, ListView1.FocusedItem.SubItems(0).Text) & Quote)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        DynaLog.LogMessage("Preparing to apply the selected answer file...")
        ApplyUnattendFile.TextBox1.Text = Path.Combine(TextBox1.Text, ListView1.FocusedItem.SubItems(0).Text)
        WindowState = FormWindowState.Minimized
        ApplyUnattendFile.ShowDialog(MainForm)
        WindowState = FormWindowState.Normal
    End Sub

    Private Sub UnattendMgr_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("UnattendMgr")("Unattended.AnswerFile.Label")
        Label1.Text = LocalizationService.ForSection("UnattendMgr")("ProjectPath.Label")
        Button1.Text = LocalizationService.ForSection("UnattendMgr")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("UnattendMgr")("OpenFile.Button")
        Button3.Text = LocalizationService.ForSection("UnattendMgr")("Open.File.Location.Button")
        Button4.Text = LocalizationService.ForSection("UnattendMgr")("ApplyImage.Button")
        ListView1.Columns(0).Text = LocalizationService.ForSection("UnattendMgr")("FileName.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("UnattendMgr")("Created.Column")
        ListView1.Columns(2).Text = LocalizationService.ForSection("UnattendMgr")("LastModified.Column")
        ListView1.Columns(3).Text = LocalizationService.ForSection("UnattendMgr")("LastAccessed.Column")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ColumnHeader1.Width = WindowHelper.ScaleLogical(431)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(168)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(144)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(144)
    End Sub
End Class
