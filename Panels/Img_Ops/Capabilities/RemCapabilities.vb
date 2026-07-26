Imports System.Windows.Forms
Imports Microsoft.Dism
Imports DISMTools.Utilities

Public Class RemCapabilities
    Implements IImageTaskDialog

    Dim capCount As Integer
    Dim capIds(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        Dim capIdList As New List(Of String)
        capCount = ListView1.CheckedItems.Count
        ProgressPanel.MountDir = MainForm.MountDir
        DynaLog.LogMessage("Detecting capabilities to remove...")
        If ListView1.CheckedItems.Count >= 1 Then
            For x = 0 To capCount - 1
                capIdList.Add(ListView1.CheckedItems(x).SubItems(0).Text)
            Next
            capIds = capIdList.ToArray()
            For x = 0 To capIds.Length - 1
                ProgressPanel.capRemovalIds(x) = capIds(x)
            Next
            ProgressPanel.capRemovalLastId = ListView1.CheckedItems(capCount - 1).SubItems(0).Text
        Else
            DynaLog.LogMessage("No items have been added to the queue.")
            MsgBox(LocalizationService.ForSection("RemCapabilities.Validation")("Selected.None.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        ProgressPanel.capRemovalCount = capCount
        ProgressPanel.OperationNum = 68
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
            MsgBox(LocalizationService.ForSection("RemCapabilities.Initialize")("UnsupportedImage.Message"), vbOKOnly + vbCritical, Text)
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
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageCapabilities.Where(Function(capability) New DismPackageFeatureState() {DismPackageFeatureState.Installed, DismPackageFeatureState.InstallPending}.Contains(capability.State)).Select(Function(capability) New ListViewItem(New String() {capability.Name, Casters.CastDismFeatureState(capability.State, True)})).ToArray())
        Else
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageCapabilities_Backup.Where(Function(capability) New DismPackageFeatureState() {DismPackageFeatureState.Installed, DismPackageFeatureState.InstallPending}.Contains(capability.CapabilityState)).Select(Function(capability) New ListViewItem(New String() {capability.CapabilityName, Casters.CastDismFeatureState(capability.CapabilityState, True)})).ToArray())
        End If
        Return True
    End Function

    Private Sub RemCapabilities_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Text = LocalizationService.ForSection("RemCapabilities")("Remove.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("RemCapabilities").Format("Image.Task.Header.Label", Text)
        OK_Button.Text = LocalizationService.ForSection("RemCapabilities")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("RemCapabilities")("Cancel.Button")
        ListView1.Columns(0).Text = LocalizationService.ForSection("RemCapabilities")("Capability.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("RemCapabilities")("State.Column")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ColumnHeader1.Width = WindowHelper.ScaleLogical(524)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(199)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub
End Class
