Imports System.Windows.Forms
Imports Microsoft.Dism
Imports DISMTools.Utilities

Public Class DisableFeat
    Implements IImageTaskDialog

    Public featDisablementCount As Integer
    Public featDisablementNames(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MainForm.MountDir
        featDisablementCount = ListView1.CheckedItems.Count
        ProgressPanel.featDisablementCount = featDisablementCount
        DynaLog.LogMessage("Detecting features to disable...")
        If ListView1.CheckedItems.Count <= 0 Then
            DynaLog.LogMessage("No items have been added to the queue.")
            MessageBox.Show(MainForm, LocalizationService.ForSection("DisableFeat.Validation")("Features.Message"), LocalizationService.ForSection("DisableFeat.Validation")("FeaturesSelected.Title"), MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        Else
            Try
                For x = 0 To featDisablementCount - 1
                    featDisablementNames(x) = ListView1.CheckedItems(x).ToString()
                Next
                For x = 0 To featDisablementNames.Length
                    ProgressPanel.featDisablementNames(x) = featDisablementNames(x)
                Next
            Catch ex As Exception

            End Try
            ProgressPanel.featDisablementLastName = ListView1.CheckedItems(featDisablementCount - 1).ToString()
            If CheckBox1.Checked Then
                ProgressPanel.featDisablementParentPkgUsed = True
                ProgressPanel.featDisablementParentPkg = TextBox1.Text
            Else
                ProgressPanel.featDisablementParentPkgUsed = False
                ProgressPanel.featDisablementParentPkg = ""
            End If
            If CheckBox2.Checked Then
                ProgressPanel.featDisablementRemoveManifest = False
            Else
                ProgressPanel.featDisablementRemoveManifest = True
            End If
        End If
        ProgressPanel.OperationNum = 31
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
        DynaLog.LogMessage("Opening feature disablement dialog...")
        ListView1.Items.Clear()
        If Not MainForm.CompletedTasks(1) Then
            DynaLog.LogMessage("Feature background processes haven't completed.")
            BGProcsBusyDialog.ShowDialog(Me)
            Return False
        End If
        DynaLog.LogMessage("Adding features to arrays...")
        If MainForm.CurrentImage.ImageFeatures IsNot Nothing AndAlso MainForm.CurrentImage.ImageFeatures.Count > MainForm.CurrentImage.ImageFeatures_Backup.Count Then
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageFeatures.Where(Function(feature) Not New DismPackageFeatureState() {DismPackageFeatureState.NotPresent, DismPackageFeatureState.UninstallPending, DismPackageFeatureState.Staged, DismPackageFeatureState.Removed}.Contains(feature.State)).Select(Function(feature) New ListViewItem(New String() {feature.FeatureName, Casters.CastDismFeatureState(feature.State, True)})).ToArray())
        Else
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageFeatures_Backup.Where(Function(feature) Not New DismPackageFeatureState() {DismPackageFeatureState.NotPresent, DismPackageFeatureState.UninstallPending, DismPackageFeatureState.Staged, DismPackageFeatureState.Removed}.Contains(feature.FeatureState)).Select(Function(feature) New ListViewItem(New String() {feature.FeatureName, Casters.CastDismFeatureState(feature.FeatureState, True)})).ToArray())
        End If
        Return True
    End Function

    Private Sub DisableFeat_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Text = LocalizationService.ForSection("DisableFeat")("DisableFeatures.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("DisableFeat").Format("Image.Task.Header.Label", Text)
        Label3.Text = LocalizationService.ForSection("DisableFeat")("PackageName.Label")
        GroupBox1.Text = LocalizationService.ForSection("DisableFeat")("Features.Group")
        GroupBox2.Text = LocalizationService.ForSection("DisableFeat")("Options.Group")
        Button1.Text = LocalizationService.ForSection("DisableFeat")("Lookup.Button")
        OK_Button.Text = LocalizationService.ForSection("DisableFeat")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("DisableFeat")("Cancel.Button")
        ListView1.Columns(0).Text = LocalizationService.ForSection("DisableFeat")("FeatureName.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("DisableFeat")("State.Column")
        CheckBox1.Text = LocalizationService.ForSection("DisableFeat")("ParentPackage.CheckBox")
        CheckBox2.Text = LocalizationService.ForSection("DisableFeat")("Remove.Feature.CheckBox")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PkgParentNameLookupDlg.pkgSource = MainForm.MountDir
        PkgParentNameLookupDlg.OriginatedFrom = "disablement"
        PkgParentNameLookupDlg.ShowDialog(Me)
    End Sub
End Class
