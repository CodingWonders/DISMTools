Imports System.Windows.Forms
Imports Microsoft.Win32
Imports Microsoft.VisualBasic.ControlChars
Imports System.IO
Imports System.Text.RegularExpressions

Public Class ImgCleanup

    Dim Tasks() As String = New String(6) {"", "", "", "", "", "", ""}

    Dim SelTask As Integer = -1

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.CleanupTask = ComboBox1.SelectedIndex
        DynaLog.LogMessage("Image cleanup task: " & ComboBox1.SelectedIndex)
        Select Case ComboBox1.SelectedIndex
            Case 1
                ProgressPanel.CleanupHideSP = CheckBox1.Checked = True
            Case 2
                ProgressPanel.ResetCompBase = CheckBox2.Checked = True
                ProgressPanel.DeferCleanupOps = If(CheckBox2.Checked And CheckBox3.Checked, True, False)
            Case 6
                ProgressPanel.UseCompRepairSource = CheckBox4.Checked = True
                DynaLog.LogMessage("Detecting state of component repair sources...")
                If CheckBox4.Checked And RichTextBox1.Text = "" Then
                    DynaLog.LogMessage("No source has been provided.")
                    MsgBox(LocalizationService.ForSection("ImgCleanup.Validation")("Source.Has.None.Message") & CrLf & CrLf & If(RichTextBox1.Text = "", LocalizationService.ForSection("ImgCleanup.Validation")("Provide.Source.Try.Message"), LocalizationService.ForSection("ImgCleanup.Validation")("Please.Make.Message")), vbOKOnly + vbCritical, LocalizationService.ForSection("ImgCleanup.Validation")("ImageCleanup.Message"))
                    Exit Sub
                End If
                ProgressPanel.ComponentRepairSource = RichTextBox1.Text
                ProgressPanel.LimitWUAccess = CheckBox5.Checked = True
        End Select
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 32
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgCleanup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        Text = LocalizationService.ForSection("ImgCleanup")("ImageCleanup.Label")
        ImageTaskHeader1.ItemText = Text
        Label2.Text = LocalizationService.ForSection("ImgCleanup")("Task.Choose.Label")
        If ComboBox1.SelectedItem = "" Then Label3.Text = LocalizationService.ForSection("ImgCleanup")("Text4.Label")
        Label4.Text = LocalizationService.ForSection("ImgCleanup")("NoOptions.Message")
        Label5.Text = LocalizationService.ForSection("ImgCleanup")("Superseded.Base.Reset.Label")
        Label7.Text = LocalizationService.ForSection("ImgCleanup")("Only.Check.Option.Label")
        Label8.Text = LocalizationService.ForSection("ImgCleanup")("NoOptions.Label")
        Label9.Text = LocalizationService.ForSection("ImgCleanup")("NoOptions.Label")
        Label10.Text = LocalizationService.ForSection("ImgCleanup")("NoOptions.Label")
        Label11.Text = LocalizationService.ForSection("ImgCleanup")("Source.Label")
        Label12.Text = LocalizationService.ForSection("ImgCleanup")("Task.Listed.Label")
        GroupBox1.Text = LocalizationService.ForSection("ImgCleanup")("TaskOptions.Group")
        OK_Button.Text = LocalizationService.ForSection("ImgCleanup")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ImgCleanup")("Cancel.Button")
        Tasks(0) = LocalizationService.ForSection("ImgCleanup")("Revert.Pending.Actions.Item")
        Tasks(1) = LocalizationService.ForSection("ImgCleanup")("Clean.Up.ServicePack.Item")
        Tasks(2) = LocalizationService.ForSection("ImgCleanup")("Clean.Up.Component.Item")
        Tasks(3) = LocalizationService.ForSection("ImgCleanup")("Analyze.Component.Store.Item")
        Tasks(4) = LocalizationService.ForSection("ImgCleanup")("Check.Component.Store.Item")
        Tasks(5) = LocalizationService.ForSection("ImgCleanup")("Scan.Comp.Store.Item")
        Tasks(6) = LocalizationService.ForSection("ImgCleanup")("Repair.Component.Store.Item")
        ComboBox1.Items.AddRange(Tasks)
        HealthRestoreSourceOFD.Title = LocalizationService.ForSection("ImgCleanup")("Source.Title")
        Button1.Text = LocalizationService.ForSection("ImgCleanup")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("ImgCleanup")("Detect.Group.Policy.Button")
        CheckBox1.Text = LocalizationService.ForSection("ImgCleanup")("HideServicePack.CheckBox")
        CheckBox2.Text = LocalizationService.ForSection("ImgCleanup")("Reset.Base.CheckBox")
        CheckBox3.Text = LocalizationService.ForSection("ImgCleanup")("Defer.Long.Running.CheckBox")
        CheckBox4.Text = LocalizationService.ForSection("ImgCleanup")("Different.Source.CheckBox")
        CheckBox5.Text = LocalizationService.ForSection("ImgCleanup")("WindowsUpdate.CheckBox")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
        RichTextBox1.BackColor = BackColor
        RichTextBox1.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        WimFileSourcePanel.SetColors()
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ' Determine when the last base reset was run
        DynaLog.LogMessage("Getting status of last base reset...")
        If MainForm.OnlineManagement Then
            DynaLog.LogMessage("Detecting last base reset of active installation...")
            Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing", False)
            Dim LastResetBase_UTC As String = ""
            LastResetBase_UTC = regKey.GetValue(LocalizationService.ForSection("ImgCleanup")("Last.Reset.Base.Label"), LocalizationService.ForSection("ImgCleanup")("Get.Last.Base.Message")).ToString()
            regKey.Close()
            Dim charArray() As Char = LastResetBase_UTC.ToCharArray()
            If LastResetBase_UTC.Contains("/") Then charArray(10) = " "
            LastResetBase_UTC = New String(charArray)
            DynaLog.LogMessage("Detected date: " & LastResetBase_UTC)
            Label6.Text = LastResetBase_UTC
        Else
            DynaLog.LogMessage("Detecting last base reset of image...")
            DynaLog.LogMessage("Loading SOFTWARE hive...")
            Dim regExitCode As Integer = RegistryHelper.LoadRegistryHive(Path.Combine(MainForm.MountDir, "Windows", "system32", "config", "SOFTWARE"), "HKLM\MountedSoft")
            DynaLog.LogMessage("Registry process finished with exit code " & Hex(regExitCode))
            If regExitCode = 0 Then
                Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("MountedSoft\Microsoft\Windows\CurrentVersion\Component Based Servicing", False)
                Dim LastResetBase_UTC As String = ""
                LastResetBase_UTC = regKey.GetValue(LocalizationService.ForSection("ImgCleanup")("Last.Reset.Base.Item"), LocalizationService.ForSection("ImgCleanup")("Get.Last.Base.Item")).ToString()
                regKey.Close()
                Dim charArray() As Char = LastResetBase_UTC.ToCharArray()
                If LastResetBase_UTC.Contains("/") Then charArray(10) = " "
                LastResetBase_UTC = New String(charArray)
                Label6.Text = LastResetBase_UTC
                DynaLog.LogMessage("Detected date: " & LastResetBase_UTC)
                RegistryHelper.UnloadRegistryHive("HKLM\MountedSoft")
            Else
                Label6.Text = LocalizationService.ForSection("ImgCleanup")("Get.Last.Base.Label")
            End If
        End If

        If MainForm.OnlineManagement And (SystemInformation.BootMode = BootMode.Normal Or SystemInformation.BootMode = BootMode.FailSafeWithNetwork) Then
            CheckBox5.Enabled = True
        Else
            CheckBox5.Checked = False
            CheckBox5.Enabled = False
        End If

        If SelTask >= 0 And SelTask < ComboBox1.Items.Count Then ComboBox1.SelectedIndex = SelTask
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "" Then
            Label3.Text = LocalizationService.ForSection("ImgCleanup")("Task.See.Choose.Label")
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = False
        Else
            Select Case ComboBox1.SelectedIndex
                Case 0
                    Label3.Text = LocalizationService.ForSection("ImgCleanup")("Experience.Boot.Message")
                    Panel2.Visible = True
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 1
                    Label3.Text = LocalizationService.ForSection("ImgCleanup")("Removes.Backup.Files.Item")
                    Panel2.Visible = False
                    Panel3.Visible = True
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 2
                    Label3.Text = LocalizationService.ForSection("ImgCleanup")("Cleans.Up.Superseded.Item")
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = True
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 3
                    Label3.Text = LocalizationService.ForSection("ImgCleanup")("Creates.Report.Comp.Item")
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = True
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 4
                    Label3.Text = LocalizationService.ForSection("ImgCleanup")("Checks.Whether.Image.Message")
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = True
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 5
                    Label3.Text = LocalizationService.ForSection("ImgCleanup")("Scans.Image.Message")
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = True
                    Panel8.Visible = False
                Case 6
                    Label3.Text = LocalizationService.ForSection("ImgCleanup")("Scans.Image.Component.Item")
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = True
            End Select
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        Label11.Enabled = CheckBox4.Checked = True
        RichTextBox1.Enabled = CheckBox4.Checked = True
        Button1.Enabled = CheckBox4.Checked = True
        Button2.Enabled = CheckBox4.Checked = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        HealthRestoreSourceOFD.ShowDialog(Me)
    End Sub

    Private Sub HealthRestoreSourceOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles HealthRestoreSourceOFD.FileOk
        RichTextBox1.Text = HealthRestoreSourceOFD.FileName
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DynaLog.LogMessage("Getting source established in the group policy...")
        RichTextBox1.Text = ServicingGPOHelper.GetSrcFromGPO()
        If Regex.IsMatch(RichTextBox1.Text, "(^wim:\\)(.*)(:\d+$)") Then
            ' Divide the source to only grab image file and index
            Dim ImageFileMatches As MatchCollection = Regex.Matches(RichTextBox1.Text, "(^wim:\\)(.*)(:\d+$)")
            WimFileSourcePanel.ImageFile = ImageFileMatches(0).Groups(2).Value
            WimFileSourcePanel.ImageIndex = CInt(ImageFileMatches(0).Groups(3).Value.Replace(":", ""))
            WimFileSourcePanel.Visible = True
        Else
            WimFileSourcePanel.Visible = False
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        TextBoxSourcePanel.Visible = True
        WimFileSourcePanel.Visible = False
    End Sub

    Private Sub ImgCleanup_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        SelTask = ComboBox1.SelectedIndex
    End Sub
End Class
