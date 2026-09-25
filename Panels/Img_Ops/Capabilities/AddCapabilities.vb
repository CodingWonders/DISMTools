Imports System.Windows.Forms
Imports Microsoft.Win32
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports DISMTools.Utilities
Imports System.Text.RegularExpressions

Public Class AddCapabilities
    Implements IImageTaskDialog

    Dim capCount As Integer
    Dim capIds(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        Dim capIdList As New List(Of String)
        ProgressPanel.MountDir = MainForm.MountDir
        capCount = ListView1.CheckedItems.Count
        DynaLog.LogMessage("Detecting capabilities to add...")
        If ListView1.CheckedItems.Count >= 1 Then
            For x = 0 To capCount - 1
                capIdList.Add(ListView1.CheckedItems(x).SubItems(0).Text)
            Next
            capIds = capIdList.ToArray()
            For x = 0 To capIds.Length - 1
                ProgressPanel.capAdditionIds(x) = capIds(x)
            Next
            DynaLog.LogMessage("Getting states of capabilities for any missing sources...")
            For x = 0 To capCount - 1
                If MainForm.OnlineManagement And Not CheckBox2.Checked Then Exit For
                If ListView1.CheckedItems(x).SubItems(1).Text = LocalizationService.ForSection("Casters.Cast.DISM")("Present.Label") Then
                    If CheckBox1.Checked And RichTextBox1.Text = "" Or Not Directory.Exists(RichTextBox1.Text) Then
                        DynaLog.LogMessage("No source has been specified or it does not exist.")
                        If MsgBox(LocalizationService.ForSection("AddCapabilities.Validation")("SourceRequired.Message") & CrLf & CrLf & If(RichTextBox1.Text = "", LocalizationService.ForSection("AddCapabilities.Validation")("Source.Required.Message"), LocalizationService.ForSection("AddCapabilities.Validation")("Source.Message")), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText) = MsgBoxResult.Ok Then
                            CheckBox1.Checked = True
                            Button1.PerformClick()
                        End If
                    End If
                End If
            Next
            ProgressPanel.capAdditionLastId = ListView1.CheckedItems(capCount - 1).SubItems(0).Text
            If CheckBox1.Checked Then
                DynaLog.LogMessage("Specified source: " & Quote & RichTextBox1.Text & Quote)
                If RichTextBox1.Text <> "" Then
                    If Directory.Exists(RichTextBox1.Text) Then
                        DynaLog.LogMessage("The specified source exists in the file system.")
                        ProgressPanel.capAdditionUseSource = True
                        ProgressPanel.capAdditionSource = RichTextBox1.Text         ' Don't know if it would work on cases where it begins with "wim:\"
                    Else
                        DynaLog.LogMessage("The specified source does not exist in the file system.")
                        MsgBox(LocalizationService.ForSection("AddCapabilities.Validation")("Source.Exist.File.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Exit Sub
                    End If
                Else
                    DynaLog.LogMessage("No source has been specified.")
                    MsgBox(LocalizationService.ForSection("AddCapabilities.Validation")("Source.Required.No.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    Exit Sub
                End If
            End If
            If CheckBox2.Checked Then
                ProgressPanel.capAdditionLimitWUAccess = True
            Else
                ProgressPanel.capAdditionLimitWUAccess = False
            End If
            If CheckBox3.Checked And Not MainForm.OnlineManagement Then
                ProgressPanel.capAdditionCommit = True
            Else
                ProgressPanel.capAdditionCommit = False
            End If
        Else
            DynaLog.LogMessage("No items have been added to the queue.")
            MsgBox(LocalizationService.ForSection("AddCapabilities.Validation")("Selected.None.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        ProgressPanel.capAdditionCount = capCount
        ProgressPanel.OperationNum = 64
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
        DynaLog.LogMessage("Checking edition and version information for any unmet requirements...")
        If MainForm.CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Or Not MainForm.IsWindows10OrHigher(MainForm.MountDir & "\Windows\system32\ntoskrnl.exe") Then
            DynaLog.LogMessage("The image is not supported")
            MsgBox(LocalizationService.ForSection("AddCapabilities.Initialize")("UnsupportedImage.Message"), vbOKOnly + vbCritical, Text)
            Return False
        End If
        DynaLog.LogMessage("All requirements are met. Continuing with the task...")
        ListView1.Items.Clear()
        If Not MainForm.CompletedTasks(3) Then
            DynaLog.LogMessage("Capability background processes haven't completed.")
            BGProcsBusyDialog.ShowDialog(Me)
            Return False
        End If
        DynaLog.LogMessage("Adding capabilities to arrays...")
        If MainForm.CurrentImage.ImageCapabilities IsNot Nothing AndAlso MainForm.CurrentImage.ImageCapabilities.Count > 0 Then
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageCapabilities.Where(Function(capability) Not New DismPackageFeatureState() {DismPackageFeatureState.Installed, DismPackageFeatureState.InstallPending}.Contains(capability.State)).Select(Function(capability) New ListViewItem(New String() {capability.Name, Casters.CastDismFeatureState(capability.State, True)})).ToArray())
        Else
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageCapabilities_Backup.Where(Function(capability) Not New DismPackageFeatureState() {DismPackageFeatureState.Installed, DismPackageFeatureState.InstallPending}.Contains(capability.CapabilityState)).Select(Function(capability) New ListViewItem(New String() {capability.CapabilityName, Casters.CastDismFeatureState(capability.CapabilityState, True)})).ToArray())
        End If
        Return True
    End Function

    Private Sub AddCapability_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Text = LocalizationService.ForSection("AddCapabilities")("AddCapabilities.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("AddCapabilities").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("AddCapabilities")("Source.Label")
        OK_Button.Text = LocalizationService.ForSection("AddCapabilities")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("AddCapabilities")("Cancel.Button")
        Button1.Text = LocalizationService.ForSection("AddCapabilities")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("AddCapabilities")("SelectAll.Button")
        Button3.Text = LocalizationService.ForSection("AddCapabilities")("SelectNone.Button")
        Button4.Text = LocalizationService.ForSection("AddCapabilities")("Detect.Group.Policy.Button")
        GroupBox1.Text = LocalizationService.ForSection("AddCapabilities")("Capabilities.Group")
        GroupBox2.Text = LocalizationService.ForSection("AddCapabilities")("Options.Group")
        CheckBox1.Text = LocalizationService.ForSection("AddCapabilities")("DifferentSource.CheckBox")
        CheckBox2.Text = LocalizationService.ForSection("AddCapabilities")("WindowsUpdate.CheckBox")
        CheckBox3.Text = LocalizationService.ForSection("AddCaps.AddCap")("CommitImage.CheckBox")
        ListView1.Columns(0).Text = LocalizationService.ForSection("AddCapabilities")("Capability.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("AddCapabilities")("State.Column")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        RichTextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        WimFileSourcePanel.SetColors()
        CheckBox1.ForeColor = ForeColor
        CheckBox2.ForeColor = ForeColor
        CheckBox3.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        RichTextBox1.ForeColor = ForeColor
        If MainForm.OnlineManagement And (SystemInformation.BootMode = BootMode.Normal Or SystemInformation.BootMode = BootMode.FailSafeWithNetwork) Then
            CheckBox2.Enabled = True
        Else
            CheckBox2.Checked = False
            CheckBox2.Enabled = False
        End If
        CheckBox3.Enabled = If(MainForm.OnlineManagement Or MainForm.OfflineManagement, False, True)
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ColumnHeader1.Width = WindowHelper.ScaleLogical(520)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(204)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Label2.Enabled = True
            RichTextBox1.Enabled = True
            Button1.Enabled = True
            Button4.Enabled = True
        Else
            Label2.Enabled = False
            RichTextBox1.Enabled = False
            Button1.Enabled = False
            Button4.Enabled = False
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For i As Integer = 0 To ListView1.Items.Count - 1
            ListView1.Items(i).Checked = True
        Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        For i As Integer = 0 To ListView1.Items.Count - 1
            ListView1.Items(i).Checked = False
        Next
        DialogResult = Windows.Forms.DialogResult.None
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            RichTextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        TextBoxSourcePanel.Visible = True
        WimFileSourcePanel.Visible = False
    End Sub
End Class
