Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars
Imports System.IO
Imports Microsoft.Dism

Public Class RemDrivers
    Implements IImageTaskDialog

    Dim drvPkgs(65535) As String
    Dim drvPkgCount As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MainForm.MountDir
        drvPkgCount = ListView1.CheckedItems.Count
        DynaLog.LogMessage("Detecting drivers to remove...")
        If ListView1.CheckedItems.Count > 0 Then
            Dim drvPkgList As New List(Of String)
            For x = 0 To ListView1.CheckedItems.Count - 1
                drvPkgList.Add(ListView1.CheckedItems(x).SubItems(0).Text)
            Next
            drvPkgs = drvPkgList.ToArray()
            ' Detect if there are boot-critical drivers checked
            For x = 0 To ListView1.CheckedItems.Count - 1
                If ListView1.CheckedItems(x).SubItems(5).Text = LocalizationService.ForSection("RemDrivers.Validation")("Yes.Button") Then
                    DynaLog.LogMessage("Some checked drivers are critical for the boot process. Removing these could result in an unbootable system.")
                    If MsgBox(LocalizationService.ForSection("RemDrivers.Validation")("Selected.Boot.Message") & CrLf & CrLf & LocalizationService.ForSection("RemDrivers.Validation")("WantContinue.Message"), vbYesNo + vbExclamation, ImageTaskHeader1.ItemText) = MsgBoxResult.No Then
                        Exit Sub
                    End If
                    Exit For
                End If
                If ListView1.CheckedItems(x).SubItems(4).Text = LocalizationService.ForSection("RemDrivers.Validation")("CheckedItems.Button") Then
                    DynaLog.LogMessage("Some checked drivers are part of the Windows distribution. Removing these could hinder the overall experience.")
                    If MsgBox(LocalizationService.ForSection("RemDrivers.Validation")("Selected.Part.Message") & CrLf & CrLf & LocalizationService.ForSection("RemDrivers.Validation")("ContinueQuestion.Message"), vbYesNo + vbExclamation, ImageTaskHeader1.ItemText) = MsgBoxResult.No Then
                        Exit Sub
                    End If
                    Exit For
                End If
            Next
            For x = 0 To drvPkgs.Length - 1
                ProgressPanel.drvRemovalPkgs(x) = drvPkgs(x)
            Next
            ProgressPanel.drvRemovalLastPkg = ListView1.CheckedItems(drvPkgCount - 1).SubItems(0).Text
        Else
            DynaLog.LogMessage("No items have been added to the queue.")
            MsgBox(LocalizationService.ForSection("RemDrivers.Validation")("Packages.Required.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        ProgressPanel.drvRemovalCount = drvPkgCount
        ProgressPanel.OperationNum = 76
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
        ListView1.Items.Clear()
        If Not MainForm.CompletedTasks(4) Then
            DynaLog.LogMessage("Device driver background processes haven't completed.")
            BGProcsBusyDialog.ShowDialog(Me)
            Return False
        End If
        DynaLog.LogMessage("Adding device drivers to arrays...")
        Try
            If MainForm.CurrentImage.ImageDrivers Is Nothing OrElse MainForm.CurrentImage.ImageDrivers_Backup.Count > MainForm.CurrentImage.ImageDrivers.Count Then
                CheckBox1.Enabled = False

                Dim filteredDrivers As IEnumerable(Of ImageDriver) = MainForm.CurrentImage.ImageDrivers_Backup.AsEnumerable()
                If CheckBox2.Checked Then
                    filteredDrivers = filteredDrivers.Where(Function(driver) Not driver.DriverInbox)
                End If

                ListView1.Items.AddRange(filteredDrivers.Select(Function(driver) New ListViewItem(New String() {driver.DriverPublishedName,
                                                                                                                Path.GetFileName(driver.DriverOriginalFileName),
                                                                                                                driver.DriverProviderName,
                                                                                                                driver.DriverClassName,
                                                                                                                driver.DriverInboxToString(),
                                                                                                                "Unknown",
                                                                                                                driver.DriverVersion.ToString(),
                                                                                                                driver.DriverDate})).ToArray())
            Else
                CheckBox1.Enabled = True

                Dim filteredDrivers As IEnumerable(Of DismDriverPackage) = MainForm.CurrentImage.ImageDrivers.AsEnumerable()
                filteredDrivers = filteredDrivers.Where(Function(driver) driver.BootCritical = Not CheckBox1.Checked And driver.InBox = Not CheckBox2.Checked)
                ListView1.Items.AddRange(filteredDrivers.Select(Function(driver) New ListViewItem(New String() {driver.PublishedName,
                                                                                                                Path.GetFileName(driver.OriginalFileName),
                                                                                                                driver.ProviderName,
                                                                                                                driver.ClassName,
                                                                                                                If(driver.InBox, "Yes", "No"),
                                                                                                                If(driver.BootCritical, "Yes", "No"),
                                                                                                                driver.Version.ToString(),
                                                                                                                driver.Date})).ToArray())
            End If
        Catch ex As Exception
            Exit Try
        End Try
        Return True
    End Function

    Private Sub RemDrivers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Text = LocalizationService.ForSection("RemDrivers")("RemoveDrivers.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("RemDrivers").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("RemDrivers")("DriverPackages.Wish.Label")
        CheckBox1.Text = LocalizationService.ForSection("RemDrivers")("Hide.Boot.Critical.CheckBox")
        CheckBox2.Text = LocalizationService.ForSection("RemDrivers")("Hide.Drivers.Part.CheckBox")
        OK_Button.Text = LocalizationService.ForSection("RemDrivers")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("RemDrivers")("Cancel.Button")
        ListView1.Columns(0).Text = LocalizationService.ForSection("RemDrivers")("PublishedName.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("RemDrivers")("Original.File.Name.Column")
        ListView1.Columns(2).Text = LocalizationService.ForSection("RemDrivers")("ProviderName.Column")
        ListView1.Columns(3).Text = LocalizationService.ForSection("RemDrivers")("ClassName.Column")
        ListView1.Columns(4).Text = LocalizationService.ForSection("RemDrivers")("Part.Windows.Column")
        ListView1.Columns(5).Text = LocalizationService.ForSection("RemDrivers")("BootCritical.Column")
        ListView1.Columns(6).Text = LocalizationService.ForSection("RemDrivers")("Version.Column")
        ListView1.Columns(7).Text = LocalizationService.ForSection("RemDrivers")("Date.Column")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ColumnHeader1.Width = WindowHelper.ScaleLogical(89)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(160)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(153)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(86)
        ColumnHeader5.Width = WindowHelper.ScaleLogical(176)
        ColumnHeader6.Width = WindowHelper.ScaleLogical(96)
        ColumnHeader7.Width = WindowHelper.ScaleLogical(71)
        ColumnHeader8.Width = WindowHelper.ScaleLogical(67)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged, CheckBox2.CheckedChanged
        DynaLog.LogMessage("Updating items shown...")
        DynaLog.LogMessage("- " & If(CheckBox1.Checked, "Drivers critical for the boot process will be shown", "Drivers critical for the boot process will not be shown"))
        DynaLog.LogMessage("- " & If(CheckBox2.Checked, "Drivers part of the Windows distribution will be shown", "Drivers part of the Windows distribution will not be shown"))
        ListView1.Items.Clear()
        ProgressPanel.OperationNum = 994
        PleaseWaitDialog.Label2.Text = LocalizationService.ForSection("RemDrivers")("Loading.DriverPackages.Label")
        If Not MainForm.areBackgroundProcessesDone Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Try
            If MainForm.CurrentImage.ImageDrivers Is Nothing OrElse MainForm.CurrentImage.ImageDrivers_Backup.Count > MainForm.CurrentImage.ImageDrivers.Count Then
                CheckBox1.Enabled = False

                Dim filteredDrivers As IEnumerable(Of ImageDriver) = MainForm.CurrentImage.ImageDrivers_Backup.AsEnumerable()
                If CheckBox2.Checked Then
                    filteredDrivers = filteredDrivers.Where(Function(driver) Not driver.DriverInbox)
                End If

                ListView1.Items.AddRange(filteredDrivers.Select(Function(driver) New ListViewItem(New String() {driver.DriverPublishedName,
                                                                                                                Path.GetFileName(driver.DriverOriginalFileName),
                                                                                                                driver.DriverProviderName,
                                                                                                                driver.DriverClassName,
                                                                                                                "Inbox Status",
                                                                                                                "Unknown",
                                                                                                                driver.DriverVersion.ToString(),
                                                                                                                driver.DriverDate})).ToArray())
            Else
                CheckBox1.Enabled = True

                Dim filteredDrivers As IEnumerable(Of DismDriverPackage) = MainForm.CurrentImage.ImageDrivers.AsEnumerable()
                If CheckBox1.Checked Then filteredDrivers = filteredDrivers.Where(Function(driver) Not driver.BootCritical)
                If CheckBox2.Checked Then filteredDrivers = filteredDrivers.Where(Function(driver) Not driver.InBox)
                ListView1.Items.AddRange(filteredDrivers.Select(Function(driver) New ListViewItem(New String() {driver.PublishedName,
                                                                                                                Path.GetFileName(driver.OriginalFileName),
                                                                                                                driver.ProviderName,
                                                                                                                driver.ClassName,
                                                                                                                If(driver.InBox, "Yes", "No"),
                                                                                                                If(driver.BootCritical, "Yes", "No"),
                                                                                                                driver.Version.ToString(),
                                                                                                                driver.Date})).ToArray())
            End If
        Catch ex As Exception
            Exit Try
        End Try
    End Sub
End Class
