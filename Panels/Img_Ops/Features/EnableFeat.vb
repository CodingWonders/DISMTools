Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports DISMTools.Utilities
Imports System.Text.RegularExpressions

Public Class EnableFeat
    Implements IImageTaskDialog

    Public featEnablementCount As Integer
    Public featEnablementNames(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MainForm.MountDir
        featEnablementCount = ListView1.CheckedItems.Count
        ProgressPanel.featEnablementCount = featEnablementCount
        DynaLog.LogMessage("Detecting features to enable...")
        If ListView1.CheckedItems.Count <= 0 Then
            DynaLog.LogMessage("No items have been added to the queue.")
            MessageBox.Show(MainForm, LocalizationService.ForSection("EnableFeat.Validation")("Features.Message"), LocalizationService.ForSection("EnableFeat.Validation")("FeaturesSelected.Title"), MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        Else
            Try
                For x = 0 To featEnablementCount - 1
                    featEnablementNames(x) = ListView1.CheckedItems(x).ToString()
                Next
                For x = 0 To featEnablementNames.Length
                    ProgressPanel.featEnablementNames(x) = featEnablementNames(x)
                Next
            Catch ex As Exception

            End Try
            DynaLog.LogMessage("Getting states of features for any missing sources...")
            For x = 0 To featEnablementCount - 1
                If MainForm.OnlineManagement And CheckBox4.Checked Then Exit For
                If ListView1.CheckedItems(x).SubItems(1).Text = LocalizationService.ForSection("Casters.Cast.DISM")("Removed.Label") Then
                    If RichTextBox1.Text = "" Or Not Directory.Exists(RichTextBox1.Text) Then
                        DynaLog.LogMessage("No source has been specified or it does not exist.")
                        If MsgBox(LocalizationService.ForSection("EnableFeat.Validation")("Features.Image.Message") & CrLf & CrLf & If(RichTextBox1.Text = "", LocalizationService.ForSection("EnableFeat.Validation")("Source.Required.Message"), LocalizationService.ForSection("EnableFeat.Validation")("Source.Message")), vbOKOnly + vbCritical, LocalizationService.ForSection("EnableFeat.Validation")("EnableFeatures.Message")) = MsgBoxResult.Ok Then
                            CheckBox2.Checked = True
                            Button2.PerformClick()
                        End If
                    Else

                    End If
                    Exit For
                End If
            Next
            ProgressPanel.featEnablementLastName = ListView1.CheckedItems(featEnablementCount - 1).ToString()
            If CheckBox1.Checked Then
                ProgressPanel.featisParentPkgNameUsed = True
                ProgressPanel.featParentPkgName = TextBox1.Text
            Else
                ProgressPanel.featisParentPkgNameUsed = False
                ProgressPanel.featParentPkgName = ""
            End If
            If CheckBox2.Checked Then
                ProgressPanel.featisSourceSpecified = True
                If RichTextBox1.Text = "" Or Not Directory.Exists(RichTextBox1.Text) Then
                    DynaLog.LogMessage("No source has been specified or it does not exist.")
                    MsgBox(LocalizationService.ForSection("EnableFeat.Validation")("Source.Message.Message"), vbOKOnly + vbCritical, LocalizationService.ForSection("EnableFeat.Validation")("EnableFeatures.Title"))
                    Exit Sub
                Else
                    ProgressPanel.featSource = RichTextBox1.Text
                End If
            Else
                ProgressPanel.featisSourceSpecified = False
                ProgressPanel.featSource = ""
            End If
            If CheckBox3.Checked Then
                ProgressPanel.featParentIsEnabled = True
            Else
                ProgressPanel.featParentIsEnabled = False
            End If
            If CheckBox4.Checked Then
                ProgressPanel.featContactWindowsUpdate = True
            ElseIf CheckBox4.Checked = False And CheckBox4.Enabled Then
                ProgressPanel.featContactWindowsUpdate = False
            ElseIf CheckBox4.Enabled = False Then
                ' Tell program to contact Windows Update, as the parameter "/LimitAccess" doesn't apply to offline images
                ProgressPanel.featContactWindowsUpdate = True
            End If
            If CheckBox5.Checked And Not MainForm.OnlineManagement Then
                ProgressPanel.featEnablementCommit = True
            Else
                ProgressPanel.featEnablementCommit = False
            End If
        End If
        ProgressPanel.OperationNum = 30
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        DynaLog.LogMessage("Opening feature enablement dialog...")
        ListView1.Items.Clear()
        DisableFeat.ListView1.Items.Clear()
        If Not MainForm.CompletedTasks(1) Then
            DynaLog.LogMessage("Feature background processes haven't completed.")
            BGProcsBusyDialog.ShowDialog(Me)
            Return False
        End If
        DynaLog.LogMessage("Adding features to arrays...")
        If MainForm.CurrentImage.ImageFeatures IsNot Nothing AndAlso MainForm.CurrentImage.ImageFeatures.Count > MainForm.CurrentImage.ImageFeatures_Backup.Count Then
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageFeatures.Where(Function(feature) New DismPackageFeatureState() {DismPackageFeatureState.NotPresent, DismPackageFeatureState.UninstallPending, DismPackageFeatureState.Staged, DismPackageFeatureState.Removed}.Contains(feature.State)).Select(Function(feature) New ListViewItem(New String() {feature.FeatureName, Casters.CastDismFeatureState(feature.State, True)})).ToArray())
        Else
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageFeatures_Backup.Where(Function(feature) New DismPackageFeatureState() {DismPackageFeatureState.NotPresent, DismPackageFeatureState.UninstallPending, DismPackageFeatureState.Staged, DismPackageFeatureState.Removed}.Contains(feature.FeatureState)).Select(Function(feature) New ListViewItem(New String() {feature.FeatureName, Casters.CastDismFeatureState(feature.FeatureState, True)})).ToArray())
        End If
        Return True
    End Function

    Private Sub EnableFeature_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("EnableFeatures.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("EnableFeat.EnableFeature").Format("Image.Task.Header.Label", Text)
        Label3.Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("PackageName.Label")
        Label4.Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("FeatureSource.Label")
        Button1.Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("Lookup.Button")
        Button2.Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("Browse.Button")
        Button3.Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("Detect.Group.Policy.Button")
        Cancel_Button.Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("Cancel.Button")
        OK_Button.Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("Ok.Button")
        GroupBox1.Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("Features.Group")
        GroupBox2.Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("Options.Group")
        CheckBox1.Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("ParentPackage.CheckBox")
        CheckBox2.Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("Source.CheckBox")
        CheckBox3.Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("ParentFeatures.CheckBox")
        CheckBox4.Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("Contact.Win.Update.CheckBox")
        CheckBox5.Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("CommitImage.CheckBox")
        ListView1.Columns(0).Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("FeatureName.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("EnableFeat.EnableFeature")("State.Column")
        FolderBrowserDialog1.Description = LocalizationService.ForSection("EnableFeat.EnableFeature")("SourceFolder.Description")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        RichTextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        RichTextBox1.ForeColor = ForeColor
        CheckBox5.Enabled = If(MainForm.OnlineManagement Or MainForm.OfflineManagement, False, True)
        DynaLog.LogMessage("Detecting ability to contact Windows Update (in the case of active installation management)...")
        DynaLog.LogMessage("Boot Mode of Host System: " & SystemInformation.BootMode.ToString())
        If MainForm.OnlineManagement And (SystemInformation.BootMode = BootMode.Normal Or SystemInformation.BootMode = BootMode.FailSafeWithNetwork) Then
            DynaLog.LogMessage("Host system is booted to either normal mode or Safe Mode with networking.")
            CheckBox4.Enabled = True
        Else
            If MainForm.OnlineManagement Then
                DynaLog.LogMessage("Host system is booted to Safe Mode. This mode does not have networking capabilities.")
            Else
                DynaLog.LogMessage("The active installation is not being managed. No online capabilities are supported, regardless of the mode the host system is in.")
            End If
            CheckBox4.Checked = False
            CheckBox4.Enabled = False
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        WimFileSourcePanel.SetColors()

        ColumnHeader1.Width = WindowHelper.ScaleLogical(372)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(339)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Label3.Enabled = True
            Button1.Enabled = True
        Else
            Label3.Enabled = False
            Button1.Enabled = False
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        Label4.Enabled = CheckBox2.Checked = True
        Button2.Enabled = CheckBox2.Checked = True
        RichTextBox1.Enabled = CheckBox2.Checked = True
        Button3.Enabled = CheckBox2.Checked = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PkgParentNameLookupDlg.pkgSource = MainForm.MountDir
        PkgParentNameLookupDlg.OriginatedFrom = "enablement"
        PkgParentNameLookupDlg.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If FolderBrowserDialog1.ShowDialog(Me) = Windows.Forms.DialogResult.OK And FolderBrowserDialog1.SelectedPath <> "" Then
            RichTextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
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
End Class
