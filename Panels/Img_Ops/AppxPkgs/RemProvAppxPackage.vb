Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars
Imports System.IO
Imports DISMTools.Utilities
Imports Microsoft.Dism

Public Class RemProvAppxPackage
    Implements IImageTaskDialog

    Public AppxRemovalPackages(65535) As String
    Public AppxRemovalFriendlyNames(65535) As String
    Public AppxRemovalCount As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        AppxRemovalCount = ListView1.CheckedItems.Count
        ProgressPanel.appxRemovalCount = AppxRemovalCount
        DynaLog.LogMessage("Detecting AppX packages to remove...")
        If ListView1.CheckedItems.Count = 0 Then
            DynaLog.LogMessage("No items have been selected for removal.")
            MsgBox(LocalizationService.ForSection("RemoveAppx.Validation")("Packages.Required.Message"), vbOKOnly + vbCritical, LocalizationService.ForSection("RemoveAppx.Validation")("Prov.Title"))
            Exit Sub
        Else
            DynaLog.LogMessage("AppX packages to remove: " & AppxRemovalCount)
            If AppxRemovalCount > 65535 Then
                MsgBox(LocalizationService.ForSection("AppxPackages.Remove.Messages")("Right.Only.Message"), vbOKOnly + vbCritical, LocalizationService.ForSection("AppxPackages.Remove.Messages")("Prov.Label"))
                Exit Sub
            Else
                DynaLog.LogMessage("Adding AppX packages to queue...")
                For x = 0 To AppxRemovalCount - 1
                    AppxRemovalPackages(x) = ListView1.CheckedItems(x).Text
                Next
                For x = 0 To AppxRemovalCount - 1
                    AppxRemovalFriendlyNames(x) = ListView1.CheckedItems(x).SubItems(1).Text
                Next
                For x = 0 To AppxRemovalPackages.Length - 1
                    ProgressPanel.appxRemovalPackages(x) = AppxRemovalPackages(x)
                Next
                For x = 0 To AppxRemovalFriendlyNames.Length - 1
                    ProgressPanel.appxRemovalPkgNames(x) = AppxRemovalFriendlyNames(x)
                Next
                ProgressPanel.appxRemovalLastPackage = ListView1.CheckedItems(AppxRemovalCount - 1).ToString().Replace("ListViewItem: {", "").Trim().Replace("}", "").Trim()

                ' If the image contains a Server Core/Nano Server installation, detect whether the Desktop Experience
                ' feature is installed
                DynaLog.LogMessage("Detecting conditions imposed by the Windows image...")
                If MainForm.CurrentImage.ImageInstallationType <> "" And (MainForm.CurrentImage.ImageInstallationType.Contains("Nano") Or MainForm.CurrentImage.ImageInstallationType.Contains("Core")) Then
                    DynaLog.LogMessage("Target Windows image contains Server Core SKU. Detecting state of Desktop Experience feature...")
                    ' Go through every feature and find Desktop Experience
                    If MainForm.CurrentImage.ImageFeatures.Count > 0 Then
                        Dim DesktopExperienceEnabled As Boolean = MainForm.CurrentImage.ImageFeatures.Any(Function(feature) feature.FeatureName = "DesktopExperience" AndAlso feature.State = DismPackageFeatureState.Installed)
                        If Not DesktopExperienceEnabled Then
                            DynaLog.LogMessage("Desktop Experience has been detected as a disabled feature.")
                            Dim msg As String = ""
                            ' Display incompatibility
                            msg = LocalizationService.ForSection("RemoveAppx.Validation")("DesktopExperience.Message")
                            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                            Exit Sub
                        End If
                    End If
                End If
            End If
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 38
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        AppxHelper.ClearRootPaths()
        AppxHelper.SetRootPaths(MainForm.MountDir)
        DynaLog.LogMessage("Checking edition and version information for any unmet requirements...")
        If MainForm.CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Or Not MainForm.IsWindows8OrHigher(MainForm.MountDir & "\Windows\system32\ntoskrnl.exe") Then
            DynaLog.LogMessage("The image is not supported")
            MsgBox(LocalizationService.ForSection("RemoveAppx.Init")("UnsupportedImage.Message"), vbOKOnly + vbCritical, Text)
            Return False
        End If
        DynaLog.LogMessage("All requirements are met. Continuing with the task...")
        ListView1.Items.Clear()
        If Not MainForm.CompletedTasks(2) Then
            DynaLog.LogMessage("AppX package background processes haven't completed.")
            BGProcsBusyDialog.ShowDialog(Me)
            Return False
        End If
        DynaLog.LogMessage("Adding AppX packages to arrays...")
        If MainForm.CurrentImage.ImageAppxPackages Is Nothing OrElse MainForm.CurrentImage.ImageAppxPackages_Backup.Count > MainForm.CurrentImage.ImageAppxPackages.Count Then
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageAppxPackages_Backup.Select(Function(appxPackage) New ListViewItem(New String() {appxPackage.PackageFullName,
                                                                                                                                              String.Format("{0}{1}", If(MainForm.AppxDisplayNameFormatOnRemoval < 2, appxPackage.PackageName, ""),
                                                                                                                                                            If(MainForm.AppxDisplayNameFormatOnRemoval > 0,
                                                                                                                                                               If(MainForm.AppxDisplayNameFormatOnRemoval < 2,
                                                                                                                                                                  " (" & AppxHelper.GetPackageDisplayName(MainForm.MountDir, appxPackage.PackageFullName, appxPackage.PackageName) & ")",
                                                                                                                                                                  AppxHelper.GetPackageDisplayName(MainForm.MountDir, appxPackage.PackageFullName, appxPackage.PackageName)
                                                                                                                                               ), "")),
                                                                                                                                              Casters.CastDismArchitecture(appxPackage.PackageArchitecture),
                                                                                                                                              appxPackage.PackageResourceId,
                                                                                                                                              appxPackage.PackageVersion.ToString(),
                                                                                                                                              appxPackage.GetLocalizedRegistrationStatus(MainForm.MountDir)})).ToArray())
        Else
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageAppxPackages.Select(Function(appxPackage) New ListViewItem(New String() {appxPackage.PackageName,
                                                                                                                                         String.Format("{0}{1}", If(MainForm.AppxDisplayNameFormatOnRemoval < 2, appxPackage.PackageName, ""),
                                                                                                                                                       If(MainForm.AppxDisplayNameFormatOnRemoval > 0,
                                                                                                                                                          If(MainForm.AppxDisplayNameFormatOnRemoval < 2,
                                                                                                                                                             " (" & AppxHelper.GetPackageDisplayName(MainForm.MountDir, appxPackage.PackageName, appxPackage.DisplayName) & ")",
                                                                                                                                                             AppxHelper.GetPackageDisplayName(MainForm.MountDir, appxPackage.PackageName, appxPackage.DisplayName)
                                                                                                                                        ), "")),
                                                                                                                                         Casters.CastDismArchitecture(appxPackage.Architecture),
                                                                                                                                         appxPackage.ResourceId,
                                                                                                                                         appxPackage.Version.ToString(),
                                                                                                                                         If(IsPackageRegistered(MainForm.MountDir, appxPackage), "Yes", "No")})).ToArray())
        End If
        Return True
    End Function

    Private Sub RemProvAppxPackage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Text = LocalizationService.ForSection("RemoveAppx")("Prov.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("RemoveAppx").Format("Image.Task.Header.Label", Text)
        OK_Button.Text = LocalizationService.ForSection("RemoveAppx")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("RemoveAppx")("Cancel.Button")
        ListView1.Columns(0).Text = LocalizationService.ForSection("RemoveAppx")("PackageName.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("RemoveAppx")("App.Display.Name.Column")
        ListView1.Columns(2).Text = LocalizationService.ForSection("RemoveAppx")("Architecture.Column")
        ListView1.Columns(3).Text = LocalizationService.ForSection("RemoveAppx")("ResourceID.Column")
        ListView1.Columns(4).Text = LocalizationService.ForSection("RemoveAppx")("Version.Column")
        ListView1.Columns(5).Text = LocalizationService.ForSection("RemoveAppx")("Registered.User.Column")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        MainForm.ViewPackageDirectoryToolStripMenuItem.Image = GetGlyphResource("openfile")
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ColumnHeader1.Width = WindowHelper.ScaleLogical(243)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(202)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(74)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(74)
        ColumnHeader5.Width = WindowHelper.ScaleLogical(80)
        ColumnHeader6.Width = WindowHelper.ScaleLogical(130)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub ListView1_MouseClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim item As ListViewItem = ListView1.GetItemAt(e.X, e.Y)
            If item IsNot Nothing Then
                MainForm.AppxPackagePopupCMS.Show(sender, e.Location)
            End If
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count = 1 Then
            MainForm.ResViewTSMI.Visible = True
            DynaLog.LogMessage("Updating context menu items...")
            Dim selectedAppx
            If MainForm.CurrentImage.ImageAppxPackages Is Nothing OrElse MainForm.CurrentImage.ImageAppxPackages_Backup.Count > MainForm.CurrentImage.ImageAppxPackages.Count Then
                selectedAppx = MainForm.CurrentImage.ImageAppxPackages_Backup.ElementAtOrDefault(ListView1.FocusedItem.Index)
            Else
                selectedAppx = MainForm.CurrentImage.ImageAppxPackages.ElementAtOrDefault(ListView1.FocusedItem.Index)
            End If

            If selectedAppx Is Nothing Then
                MainForm.ResViewTSMI.Text = ""
                MainForm.ResViewTSMI.Visible = False
            End If

            Dim friendlyDisplayName As String = ""
            If TypeOf (selectedAppx) Is ImageAppxPackage Then
                friendlyDisplayName = AppxHelper.GetPackageDisplayName(MainForm.MountDir, CType(selectedAppx, ImageAppxPackage).PackageFullName, CType(selectedAppx, ImageAppxPackage).PackageName)
            ElseIf TypeOf (selectedAppx) Is DismAppxPackage Then
                friendlyDisplayName = AppxHelper.GetPackageDisplayName(MainForm.MountDir, CType(selectedAppx, DismAppxPackage).PackageName, CType(selectedAppx, DismAppxPackage).DisplayName)
            End If

            If friendlyDisplayName.StartsWith("ms-resource:", StringComparison.OrdinalIgnoreCase) Then
                If TypeOf (selectedAppx) Is ImageAppxPackage Then
                    friendlyDisplayName = CType(selectedAppx, ImageAppxPackage).PackageName
                ElseIf TypeOf (selectedAppx) Is DismAppxPackage Then
                    friendlyDisplayName = CType(selectedAppx, DismAppxPackage).DisplayName
                End If
            End If

            Try
                MainForm.ResViewTSMI.Text = LocalizationService.ForSection("RemoveAppx").Format("ViewResources.Label", friendlyDisplayName)
            Catch ex As Exception
                MainForm.ResViewTSMI.Text = ""
                MainForm.ResViewTSMI.Visible = False
            End Try
        End If
    End Sub
End Class
