Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports System.Threading
Imports DISMTools.Utilities
Imports System.Globalization

Public Class GetDriverInfo

    Dim DriverInfoList As New List(Of DismDriverCollection)
    Dim InstalledDriverList As New List(Of DismDriverPackage)
    Dim InstalledDriverList_Backup As New List(Of ImageDriver)
    Dim SearchedDriverList As New List(Of DismDriverPackage)

    Dim CurrentHWTarget As Integer
    Dim CurrentHWFile As Integer = -1        ' This variable gets updated every time an element is selected in the driver packages list box
    Dim JumpTo As Integer = -1               ' This variable gets updated every time a target is specified in the Jump To panel

    Dim IsInDrvPkgs As Boolean

    Enum SearchMode As Integer
        OriginalFileName
        ProviderName
        ClassName
        NoInBox
        InBox
        NoBootCritical
        BootCritical
        DateField
        NotSigned
        Signed
        None
    End Enum

    Private Sub GetDriverInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("GetDriverInfo")("Driver.Label")
        ImageTaskHeader1.ItemText = Text
        Label2.Text = LocalizationService.ForSection("GetDriverInfo")("Get.Label")
        Label3.Text = LocalizationService.ForSection("GetDriverInfo")("Get.Drivers.Message")
        Label4.Text = LocalizationService.ForSection("GetDriverInfo")("AddDrivers.Help.Message")
        Label5.Text = LocalizationService.ForSection("GetDriverInfo")("Ready.Label")
        Label6.Text = LocalizationService.ForSection("GetDriverInfo")("Add.DriverPackage.Label")
        Label7.Text = LocalizationService.ForSection("GetDriverInfo")("HardwareTargets.Label")
        Label8.Text = LocalizationService.ForSection("GetDriverInfo")("Hardware.Description.Label")
        Label10.Text = LocalizationService.ForSection("GetDriverInfo")("HardwareID.Label")
        Label12.Text = LocalizationService.ForSection("GetDriverInfo")("AdditionalIds.Label")
        Label13.Text = LocalizationService.ForSection("GetDriverInfo")("CompatibleIds.Label")
        Label16.Text = LocalizationService.ForSection("GetDriverInfo")("ExcludeIds.Label")
        Label17.Text = LocalizationService.ForSection("GetDriverInfo")("Hardware.Manufacturer.Label")
        Label20.Text = LocalizationService.ForSection("GetDriverInfo")("Architecture.Label")
        Label21.Text = LocalizationService.ForSection("GetDriverInfo")("JumpTarget.Label")
        Label22.Text = LocalizationService.ForSection("GetDriverInfo")("PublishedName.Label")
        Label24.Text = LocalizationService.ForSection("GetDriverInfo")("Original.File.Name.Label")
        Label26.Text = LocalizationService.ForSection("GetDriverInfo")("ProviderName.Label")
        Label28.Text = LocalizationService.ForSection("GetDriverInfo")("Critical.Boot.Process.Label")
        Label30.Text = LocalizationService.ForSection("GetDriverInfo")("Version.Label")
        Label31.Text = LocalizationService.ForSection("GetDriverInfo")("ClassName.Label")
        Label33.Text = LocalizationService.ForSection("GetDriverInfo")("Part.Windows.Label")
        Label36.Text = LocalizationService.ForSection("GetDriverInfo")("DriverInfo.Label")
        Label37.Text = LocalizationService.ForSection("GetDriverInfo")("Installed.Driver.View.Label")
        Label39.Text = LocalizationService.ForSection("GetDriverInfo")("Date.Label")
        Label41.Text = LocalizationService.ForSection("GetDriverInfo")("ClassDescription.Label")
        Label43.Text = LocalizationService.ForSection("GetDriverInfo")("ClassGUID.Label")
        Label45.Text = LocalizationService.ForSection("GetDriverInfo")("Driver.Signature.Label")
        Label47.Text = LocalizationService.ForSection("GetDriverInfo")("Catalog.File.Path.Label")
        Label48.Text = LocalizationService.ForSection("GetDriverInfo")("Bg.Procs.Notice.Message")
        Button1.Text = LocalizationService.ForSection("GetDriverInfo")("AddDriver.Button")
        Button2.Text = LocalizationService.ForSection("GetDriverInfo")("RemoveSelected.Button")
        Button3.Text = LocalizationService.ForSection("GetDriverInfo")("RemoveAll.Button")
        Button7.Text = LocalizationService.ForSection("GetDriverInfo")("Change.Button")
        Button8.Text = LocalizationService.ForSection("GetDriverInfo")("Save.Button")
        Button9.Text = LocalizationService.ForSection("GetDriverInfo")("View.Driver.File.Button")
        LinkLabel1.Text = LocalizationService.ForSection("GetDriverInfo")("GoBack.Link")
        InstalledDriverLink.Text = LocalizationService.ForSection("GetDriverInfo")("InstalledDriver.Link")
        DriverFileLink.Text = LocalizationService.ForSection("GetDriverInfo")("Iwant.Link")
        ListView1.Columns(0).Text = LocalizationService.ForSection("GetDriverInfo")("PublishedName.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("GetDriverInfo")("Original.File.Name.Column")
        OpenFileDialog1.Title = LocalizationService.ForSection("GetDriverInfo")("Locate.Driver.Files.Title")
        SearchBox1.Text = LocalizationService.ForSection("GetDriverInfo")("Type.Search.Driver.Button")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListBox1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        SearchBox1.BackColor = BackColor
        SearchBox1.ForeColor = ForeColor
        SearchPic.Image = GetGlyphResource("search")
        WizardBtn.Image = GetGlyphResource("assistant")
        If SplitContainer1.SplitterDistance = 440 Then
            SplitContainer1.SplitterDistance = WindowHelper.ScaleLogical(SplitContainer1.SplitterDistance)
            SplitContainer2.SplitterDistance = WindowHelper.ScaleLogical(SplitContainer2.SplitterDistance)
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        DynaLog.LogMessage("Updating items in list...")
        InstalledDriverList.Clear()
        InstalledDriverList_Backup.Clear()
        SearchedDriverList.Clear()
        ListView1.Items.Clear()
        DynaLog.LogMessage("Getting installed drivers...")
        If MainForm.CurrentImage.ImageDrivers Is Nothing OrElse MainForm.CurrentImage.ImageDrivers.Count = 0 Then
            InstalledDriverList_Backup.AddRange(MainForm.CurrentImage.ImageDrivers_Backup.Select(Function(driver) driver))
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageDrivers_Backup.Select(Function(driver) New ListViewItem(New String() {driver.DriverPublishedName, Path.GetFileName(driver.DriverOriginalFileName)})).ToArray())
            SearchPanel.Visible = False
        Else
            InstalledDriverList.AddRange(MainForm.CurrentImage.ImageDrivers.Select(Function(driver) driver))
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageDrivers.Select(Function(driver) New ListViewItem(New String() {driver.PublishedName, Path.GetFileName(driver.OriginalFileName)})).ToArray())
            SearchPanel.Visible = True
        End If

        ' Detect if the "Detect all drivers" option is checked and act accordingly
        Panel6.Visible = Not MainForm.AllDrivers

        ' Forcefully hide that panel if the driver packages panel is visible
        If IsInDrvPkgs Then Panel6.Visible = False

        Button9.Visible = IsInDrvPkgs
        Button9.Enabled = False

        ' Switch to the selection panels
        Panel4.Visible = False
        Panel7.Visible = True
        DrvPackageInfoPanel.Visible = False
        NoDrvPanel.Visible = True

        SearchBox1.Text = ""
        ColumnHeader1.Width = WindowHelper.ScaleLogical(188)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(220)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        ListBox1.Items.Add(OpenFileDialog1.FileName)
        Button3.Enabled = True
        Button8.Enabled = True
        GetDriverInformation()
    End Sub

    Private Sub InstalledDriverLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles InstalledDriverLink.LinkClicked
        MenuPanel.Visible = False
        DriverInfoPanel.Visible = True
        InfoFromInstalledDrvsPanel.Visible = True
        InfoFromDrvPackagesPanel.Visible = False

        ' Detect if the "Detect all drivers" option is checked and act accordingly
        Panel6.Visible = Not MainForm.AllDrivers

        Label5.Visible = False
        IsInDrvPkgs = False
        Button8.Enabled = True
        Button9.Visible = False
    End Sub

    Private Sub DriverFileLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles DriverFileLink.LinkClicked
        MenuPanel.Visible = False
        DriverInfoPanel.Visible = True
        InfoFromInstalledDrvsPanel.Visible = False
        InfoFromDrvPackagesPanel.Visible = True
        Panel6.Visible = False
        Label5.Visible = True
        IsInDrvPkgs = True
        Button8.Enabled = ListBox1.Items.Count > 0
        Button9.Visible = True
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MenuPanel.Visible = True
        DriverInfoPanel.Visible = False
    End Sub

    Sub GetDriverInformation()
        WindowHelper.DisableCloseCapability(Handle)
        DynaLog.LogMessage("Clearing information lists...")
        DriverInfoList.Clear()
        Try
            ' Background processes need to have completed before showing information
            DynaLog.LogMessage("Checking if background processes are busy...")
            If MainForm.ImgBW.IsBusy Then
                DynaLog.LogMessage("Background processes are busy. Stopping them...")
                Dim msg As String = ""
                msg = LocalizationService.ForSection("GetDriverInfo.DriverInfo")("Wait.Background.Message")
                MsgBox(msg, vbOKOnly + vbInformation, ImageTaskHeader1.ItemText)
                Label5.Text = LocalizationService.ForSection("GetDriverInfo.DriverInfo")("Waiting.Background.Label")
                While MainForm.ImgBW.IsBusy
                    Application.DoEvents()
                    Thread.Sleep(500)
                End While
            End If
            MainForm.StopMountedImageDetector()
            Label5.Text = LocalizationService.ForSection("DriverInfo.Load")("Preparing.Driver.Item")
            Application.DoEvents()
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            DynaLog.LogMessage("Creating session...")
            Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                For Each drvFile In ListBox1.Items
                    DynaLog.LogMessage("Driver file to get information about: " & Quote & Path.GetFileName(drvFile) & Quote)
                    If File.Exists(drvFile) Then
                        DynaLog.LogMessage("Driver file exists.")
                        Label5.Text = LocalizationService.ForSection("GetDriverInfo.DriverInfo").Format("Driver.File.Message", Path.GetFileName(drvFile))
                        Application.DoEvents()
                        Dim drvInfoCollection As DismDriverCollection = DismApi.GetDriverInfo(imgSession, drvFile)
                        DynaLog.LogMessage("Information collection count: " & drvInfoCollection.Count)
                        If drvInfoCollection.Count > 0 Then DriverInfoList.Add(drvInfoCollection)
                    End If
                Next
            End Using
        Catch ex As Exception
            DynaLog.LogMessage("Could not get driver information. Error message: " & ex.Message)
            ' Cancel it
        Finally
            DynaLog.LogMessage("Shutting down API...")
            Try
                DismApi.Shutdown()
            Catch ex As Exception

            End Try
        End Try
        DynaLog.LogMessage("This process has finished.")
        Label5.Text = LocalizationService.ForSection("GetDriverInfo.DriverInfo")("Ready.Item")
        WindowHelper.EnableCloseCapability(Handle)
    End Sub

    Sub DisplayDriverInformation(HWTarget As Integer)
        DynaLog.LogMessage("Displaying driver information of a specific hardware target...")
        DynaLog.LogMessage("Selected Hardware Target: " & HWTarget)
        Dim CurrentDriverCollection As DismDriverCollection = DriverInfoList(ListBox1.SelectedIndex)
        DynaLog.LogMessage("Driver collection has " & CurrentDriverCollection.Count & " hardware target(s) available.")
        Dim selectedDriver As DismDriver = CurrentDriverCollection.ElementAtOrDefault(HWTarget - 1)
        If selectedDriver Is Nothing Then Exit Sub
        DynaLog.LogMessage("We have the appropriate hardware target. Displaying information...")
        Label9.Text = selectedDriver.HardwareDescription
        Label11.Text = selectedDriver.HardwareId
        Label14.Text = selectedDriver.CompatibleIds
        Label15.Text = selectedDriver.ExcludeIds
        Label18.Text = selectedDriver.ManufacturerName
        Label19.Text = Casters.CastDismArchitecture(selectedDriver.Architecture, True)
        If Label14.Text = "" Then
            DynaLog.LogMessage("There are no Compatible IDs declared by the device manufacturer (" & Quote & selectedDriver.ManufacturerName & Quote & ")")
            Label14.Text = LocalizationService.ForSection("DriverInfo.Display")("NoManufacturer.Label")
        End If
        If Label15.Text = "" Then
            DynaLog.LogMessage("There are no Exclude IDs declared by the device manufacturer (" & Quote & selectedDriver.ManufacturerName & Quote & ")")
            Label15.Text = LocalizationService.ForSection("DriverInfo.Display")("NoManufacturer.Label")
        End If
    End Sub

    Sub DisplayHardwareTargetOverview()
        DynaLog.LogMessage("Selected items in list box: " & ListBox1.SelectedItems.Count)
        ' This function is called when the user clicks on the "Jump to target" button
        If ListBox1.SelectedItems.Count <> 1 Then
            ' Don't continue
            Exit Sub
        Else
            DynaLog.LogMessage("There is only 1 item selected.")
            JumpTo = -1
            ComboBox1.Text = ""
            Dim CurrentDriverCollection As DismDriverCollection = DriverInfoList(ListBox1.SelectedIndex)
            DynaLog.LogMessage("Showing " & CurrentDriverCollection.Count & " entry/ies...")
            ComboBox1.Items.AddRange(CurrentDriverCollection.Select(Function(DriverPackageInfo) String.Format("{0} - {1} ({2})", CurrentDriverCollection.IndexOf(DriverPackageInfo) + 1, DriverPackageInfo.HardwareDescription, DriverPackageInfo.HardwareId)).ToArray())
        End If
    End Sub

    Private Sub ListBox1_DragEnter(sender As Object, e As DragEventArgs) Handles ListBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ListBox1_DragDrop(sender As Object, e As DragEventArgs) Handles ListBox1.DragDrop
        Dim PackageFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        ListBox1.Items.AddRange(PackageFiles.Where(Function(PackageFile) Path.GetExtension(PackageFile).EndsWith("inf", StringComparison.OrdinalIgnoreCase)).Select(Function(PackageFile) PackageFile).ToArray())
        Button3.Enabled = True
        Button8.Enabled = True
        GetDriverInformation()
    End Sub

    Private Sub GetDriverInfo_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        MainForm.StartMountedImageDetector()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Try
            If ListBox1.SelectedItems.Count = 1 Then
                DynaLog.LogMessage("Amount of hardware targets of selected driver file: " & DriverInfoList(ListBox1.SelectedIndex).Count)
                JumpToPanel.Visible = False
                NoDrvPanel.Visible = False
                DrvPackageInfoPanel.Visible = True
                Button2.Enabled = True
                If Not CurrentHWFile = ListBox1.SelectedIndex Then
                    Label7.Text = LocalizationService.ForSection("GetDriverInfo.Hardware").Format("HardwareTarget.Label", DriverInfoList(ListBox1.SelectedIndex).Count)
                End If
                If Not CurrentHWFile = ListBox1.SelectedIndex Then CurrentHWTarget = 1
                Button4.Enabled = False
                Button5.Enabled = True
                If Not CurrentHWFile = ListBox1.SelectedIndex Then DisplayDriverInformation(1)
                CurrentHWFile = ListBox1.SelectedIndex
                Button9.Enabled = True
            Else
                NoDrvPanel.Visible = True
                DrvPackageInfoPanel.Visible = False
                Button2.Enabled = False
                Button9.Enabled = False
            End If
        Catch ex As Exception
            ListBox1.Items.Remove(ListBox1.SelectedItem)
            NoDrvPanel.Visible = True
            DrvPackageInfoPanel.Visible = False
            If ListBox1.Items.Count < 1 Then
                Button2.Enabled = False
                Button3.Enabled = False
                Button8.Enabled = False
                Button9.Enabled = False
            End If
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DriverInfoList.RemoveAt(ListBox1.SelectedIndex)
        ListBox1.Items.Remove(ListBox1.SelectedItem)
        If ListBox1.Items.Count >= 1 Then
            Button2.Enabled = True
            Button3.Enabled = True
            Button8.Enabled = True
            Button9.Enabled = True
        Else
            Button2.Enabled = False
            Button3.Enabled = False
            Button8.Enabled = False
            Button9.Enabled = False
        End If
        NoDrvPanel.Visible = True
        DrvPackageInfoPanel.Visible = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        DriverInfoList.Clear()
        ListBox1.Items.Clear()
        Button2.Enabled = False
        Button3.Enabled = False
        Button8.Enabled = False
        Button9.Enabled = False
        NoDrvPanel.Visible = True
        DrvPackageInfoPanel.Visible = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If CurrentHWTarget > 1 Then
            DynaLog.LogMessage("Switching to the previous hardware target...")
            DisplayDriverInformation(CurrentHWTarget - 1)
            CurrentHWTarget -= 1
            Label7.Text = LocalizationService.ForSection("GetDriverInfo").Format("HardwareTarget.Label", CurrentHWTarget, DriverInfoList(ListBox1.SelectedIndex).Count)
            Button5.Enabled = True
            If CurrentHWTarget = 1 Then Button4.Enabled = False
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If CurrentHWTarget < DriverInfoList(ListBox1.SelectedIndex).Count Then
            DynaLog.LogMessage("Switching to the next hardware target...")
            DisplayDriverInformation(CurrentHWTarget + 1)
            CurrentHWTarget += 1
            Label7.Text = LocalizationService.ForSection("GetDriverInfo").Format("HardwareTarget.Label", CurrentHWTarget, DriverInfoList(ListBox1.SelectedIndex).Count)
            Button4.Enabled = True
            If CurrentHWTarget = DriverInfoList(ListBox1.SelectedIndex).Count Then Button5.Enabled = False
        End If
    End Sub

    Private Sub Button4_MouseHover(sender As Object, e As EventArgs) Handles Button4.MouseHover
        Dim msg As String = ""
        msg = LocalizationService.ForSection("GetDriverInfo.Tooltip")("Previous.Hardware.Message")
        WindowHelper.DisplayToolTip(sender, msg)
    End Sub

    Private Sub Button5_MouseHover(sender As Object, e As EventArgs) Handles Button5.MouseHover
        Dim msg As String = ""
        msg = LocalizationService.ForSection("GetDriverInfo.Tooltip")("Next.Hardware.Target.Message")
        WindowHelper.DisplayToolTip(sender, msg)
    End Sub

    Private Sub Button6_MouseHover(sender As Object, e As EventArgs) Handles Button6.MouseHover
        Dim msg As String = ""
        msg = LocalizationService.ForSection("GetDriverInfo.Tooltip")("Jump.Specific.Message")
        WindowHelper.DisplayToolTip(sender, msg)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        JumpTo = ComboBox1.SelectedIndex + 1
        If JumpTo < 1 Then Exit Sub
        Label7.Text = LocalizationService.ForSection("GetDriverInfo").Format("HardwareTarget.Label", JumpTo, DriverInfoList(ListBox1.SelectedIndex).Count)
        CurrentHWTarget = JumpTo
        DisplayDriverInformation(JumpTo)
        JumpToPanel.Visible = False
        If CurrentHWTarget = DriverInfoList(ListBox1.SelectedIndex).Count Then Button5.Enabled = False
        If CurrentHWTarget = 1 Then Button4.Enabled = False
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        DynaLog.LogMessage("Showing hardware target overview...")
        JumpToPanel.Visible = True
        Button4.Enabled = True
        Button5.Enabled = True
        ComboBox1.Items.Clear()
        DisplayHardwareTargetOverview()
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            If ListView1.SelectedItems.Count = 1 Then
                Panel4.Visible = True
                Panel7.Visible = False
                If InstalledDriverList_Backup.Count > InstalledDriverList.Count Then
                    Dim drv As ImageDriver = Nothing
                    drv = InstalledDriverList_Backup(ListView1.FocusedItem.Index)
                    DynaLog.LogMessage("Getting information about driver " & Quote & Path.GetFileName(drv.DriverOriginalFileName) & Quote & "...")
                    Label23.Text = drv.DriverPublishedName
                    Label25.Text = Path.GetFileName(drv.DriverOriginalFileName)
                    Label27.Text = LocalizationService.ForSection("GetDriverInfo")("Value.Label")
                    Label34.Text = If(drv.DriverInbox, LocalizationService.ForSection("GetDriverInfo")("Yes.Button"), LocalizationService.ForSection("GetDriverInfo")("No.Button"))
                    Label29.Text = drv.DriverVersion.ToString()
                    Label32.Text = drv.DriverClassName
                    Label35.Text = drv.DriverProviderName
                    Label38.Text = drv.DriverDate
                    Label40.Text = ""
                    Label42.Text = ""
                    Label44.Text = LocalizationService.ForSection("DriverInfo")("Unknown.Label")
                    Label46.Text = LocalizationService.ForSection("DriverInfo")("Unknown.Label")
                Else
                    Dim drv As DismDriverPackage = Nothing
                    If SearchBox1.Text = "" Then
                        drv = InstalledDriverList(ListView1.FocusedItem.Index)
                    Else
                        DynaLog.LogMessage("A search query has been made.")
                        drv = SearchedDriverList(ListView1.FocusedItem.Index)
                    End If
                    DynaLog.LogMessage("Getting information about driver " & Quote & Path.GetFileName(drv.OriginalFileName) & Quote & "...")
                    Label23.Text = drv.PublishedName
                    Label25.Text = Path.GetFileName(drv.OriginalFileName)
                    Label27.Text = If(drv.BootCritical, LocalizationService.ForSection("GetDriverInfo")("Yes.Button"), LocalizationService.ForSection("GetDriverInfo")("No.Button"))
                    Label34.Text = If(drv.InBox, LocalizationService.ForSection("GetDriverInfo")("Value.Button"), LocalizationService.ForSection("GetDriverInfo")("No.Button"))
                    Label29.Text = drv.Version.ToString()
                    Label32.Text = drv.ClassName
                    Label35.Text = drv.ProviderName

                    Dim CurrentOSCulture As CultureInfo = CultureInfo.CurrentCulture
                    Dim DriverDateString As String = ""
                    If MainForm.HumanizeDates Then
                        DriverDateString = String.Format("{0}, {1}", drv.Date.ToString(CurrentOSCulture.DateTimeFormat.LongDatePattern, CurrentOSCulture), drv.Date.ToString(CurrentOSCulture.DateTimeFormat.LongTimePattern, CurrentOSCulture))
                    Else
                        DriverDateString = drv.Date.ToString("MM/dd/yyyy HH:mm:ss")
                    End If

                    Label38.Text = DriverDateString
                    Label40.Text = drv.ClassDescription
                    Label42.Text = drv.ClassGuid
                    Label44.Text = Casters.SignatureStatus(drv.DriverSignature, True)
                    Label46.Text = drv.CatalogFile
                    DynaLog.LogMessage("Getting driver signer...")
                    Dim signer As String = DriverSignerViewer.GetSignerInfo(drv.OriginalFileName)
                    If Not (signer Is Nothing OrElse signer = "") Then
                        DynaLog.LogMessage("Driver signer information has been obtained.")
                        DynaLog.LogMessage(String.Format("Driver file: {0} ; Signer: {1}", Quote & Path.GetFileName(drv.OriginalFileName) & Quote, signer))
                        Label44.Text &= LocalizationService.ForSection("GetDriverInfo")("Text1.Label") & signer
                    End If
                End If
            Else
                Panel4.Visible = False
                Panel7.Visible = True
            End If
        Catch ex As Exception
            Panel4.Visible = False
            Panel7.Visible = True
        End Try
    End Sub

    Private Sub Label25_MouseHover(sender As Object, e As EventArgs) Handles Label25.MouseHover
        If SearchBox1.Text = "" Then
            WindowHelper.DisplayToolTip(sender, InstalledDriverList(ListView1.FocusedItem.Index).OriginalFileName)
        Else
            WindowHelper.DisplayToolTip(sender, SearchedDriverList(ListView1.FocusedItem.Index).OriginalFileName)
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Visible = False
        BGProcsAdvSettings.ShowDialog(MainForm)
        If BGProcsAdvSettings.DialogResult = Windows.Forms.DialogResult.OK And BGProcsAdvSettings.NeedsDriverChecks Then Close() Else Visible = True
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If MainForm.ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Saving installed device driver information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            If ImgInfoSaveDlg.DriverPkgs.Count > 0 Then ImgInfoSaveDlg.DriverPkgs.Clear()
            ImgInfoSaveDlg.SourceImage = MainForm.SourceImg
            ImgInfoSaveDlg.SaveTarget = MainForm.ImgInfoSFD.FileName
            ImgInfoSaveDlg.ImgMountDir = If(Not MainForm.OnlineManagement, MainForm.MountDir, "")
            ImgInfoSaveDlg.OnlineMode = MainForm.OnlineManagement
            ImgInfoSaveDlg.OfflineMode = MainForm.OfflineManagement
            ImgInfoSaveDlg.AllDrivers = MainForm.AllDrivers
            ImgInfoSaveDlg.SkipQuestions = MainForm.SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = MainForm.AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = If(InfoFromDrvPackagesPanel.Visible, 8, 7)
            If InfoFromDrvPackagesPanel.Visible Then
                For Each drvFile In ListBox1.Items
                    If File.Exists(drvFile) Then ImgInfoSaveDlg.DriverPkgs.Add(drvFile)
                Next
            End If
            ImgInfoSaveDlg.ImageToGetInfoFrom = MainForm.CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        DriverFileInfoDlg.ShowDialog(Me)
    End Sub

    Sub SearchDrivers(sQuery As String, Optional driverSearchMode As SearchMode = SearchMode.None)
        DynaLog.LogMessage("Search query: " & sQuery)
        If MainForm.CurrentImage.ImageDrivers Is Nothing Then Exit Sub
        If MainForm.CurrentImage.ImageDrivers.Count > 0 Then
            Dim FilteredDrivers As IEnumerable(Of DismDriverPackage) = Nothing
            Select Case driverSearchMode
                Case SearchMode.OriginalFileName
                    FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Path.GetFileName(Driver.OriginalFileName).ToLower().Contains(sQuery.Replace("og:", "").ToLower()))
                Case SearchMode.ProviderName
                    FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.ProviderName.ToLower().Contains(sQuery.Replace("prov:", "").ToLower()))
                Case SearchMode.ClassName
                    FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.ClassName.ToLower().Contains(sQuery.Replace("classname:", "").Replace("cn:", "").ToLower()))
                Case SearchMode.InBox
                    FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.InBox)
                Case SearchMode.NoInBox
                    FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Not Driver.InBox)
                Case SearchMode.BootCritical
                    FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.BootCritical)
                Case SearchMode.NoBootCritical
                    FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Not Driver.BootCritical)
                Case SearchMode.DateField
                    ' We guess the SUBMODE by the operator used
                    Try
                        Dim dateComparatorFormats() As String = {"dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy"}

                        Dim fullField As String = sQuery.Replace("date:", "")
                        ' Syntax: Operator-Field
                        Dim fieldParts() As String = fullField.Split("-")
                        Dim searchOperator As String = fieldParts(0),
                            field As String = If(fieldParts.ElementAtOrDefault(1), "")
                        Dim convertedField As Object = Nothing
                        If {"eq", "ne", "gt", "ge", "lt", "le"}.Contains(searchOperator.ToLower()) Then
                            ' Perform date conversion
                            If Not Date.TryParseExact(field, dateComparatorFormats, Nothing, Globalization.DateTimeStyles.None, convertedField) Then
                                convertedField = New Date(1970, 1, 1, 0, 0, 0)
                            End If
                        Else
                            ' Perform integer conversion
                            If Not Integer.TryParse(field, convertedField) Then
                                If searchOperator.EndsWith("y", StringComparison.OrdinalIgnoreCase) Then
                                    convertedField = 1970
                                Else
                                    convertedField = 1
                                End If
                            End If
                        End If
                        Select Case searchOperator.ToLower()
                            Case "eqy" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date.Year = CInt(convertedField))
                            Case "eqm" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date.Month = CInt(convertedField))
                            Case "eq" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date = CType(convertedField, Date))
                            Case "ney" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date.Year <> CInt(convertedField))
                            Case "nem" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date.Month <> CInt(convertedField))
                            Case "ne" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date <> CType(convertedField, Date))
                            Case "gty" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date.Year > CInt(convertedField))
                            Case "gtm" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date.Month > CInt(convertedField))
                            Case "gt" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date > CType(convertedField, Date))
                            Case "gey" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date.Year >= CInt(convertedField))
                            Case "gem" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date.Month >= CInt(convertedField))
                            Case "ge" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date >= CType(convertedField, Date))
                            Case "lty" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date.Year < CInt(convertedField))
                            Case "ltm" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date.Month < CInt(convertedField))
                            Case "lt" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date < CType(convertedField, Date))
                            Case "ley" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date.Year <= CInt(convertedField))
                            Case "lem" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date.Month <= CInt(convertedField))
                            Case "le" : FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.Date <= CType(convertedField, Date))
                            Case Else : Throw New Exception()
                        End Select
                    Catch ex As Exception
                        FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.PublishedName.ToLower().Contains(sQuery.ToLower()))
                    End Try
                Case SearchMode.NotSigned
                    FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.DriverSignature <> DismDriverSignature.Signed)
                Case SearchMode.Signed
                    FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.DriverSignature = DismDriverSignature.Signed)
                Case Else
                    FilteredDrivers = MainForm.CurrentImage.ImageDrivers.Where(Function(Driver) Driver.PublishedName.ToLower().Contains(sQuery.ToLower()))
            End Select
            If FilteredDrivers IsNot Nothing Then
                ListView1.Items.AddRange(FilteredDrivers.Select(Function(FilteredDriver) New ListViewItem(New String() {FilteredDriver.PublishedName, Path.GetFileName(FilteredDriver.OriginalFileName)})).ToArray())
                SearchedDriverList.AddRange(FilteredDrivers.Select(Function(FilteredDriver) FilteredDriver))
            End If
        End If
    End Sub

    Private Sub SearchBox1_TextChanged(sender As Object, e As EventArgs) Handles SearchBox1.TextChanged
        ListView1.Items.Clear()
        SearchedDriverList.Clear()
        If SearchBox1.Text <> "" Then
            Dim modeToUse As SearchMode
            If SearchBox1.Text.StartsWith("og:") Then
                modeToUse = SearchMode.OriginalFileName
            ElseIf SearchBox1.Text.StartsWith("prov:") Then
                modeToUse = SearchMode.ProviderName
            ElseIf SearchBox1.Text.StartsWith("classname:") Or SearchBox1.Text.StartsWith("cn:") Then
                modeToUse = SearchMode.ClassName
            ElseIf SearchBox1.Text.StartsWith("inbox:") Then
                modeToUse = SearchMode.InBox
            ElseIf SearchBox1.Text.StartsWith("noinbox:") Then
                modeToUse = SearchMode.NoInBox
            ElseIf SearchBox1.Text.StartsWith("bc:") Then
                modeToUse = SearchMode.BootCritical
            ElseIf SearchBox1.Text.StartsWith("nobc:") Then
                modeToUse = SearchMode.NoBootCritical
            ElseIf SearchBox1.Text.StartsWith("date:") Then
                modeToUse = SearchMode.DateField
            ElseIf SearchBox1.Text.StartsWith("nosig:") Then
                modeToUse = SearchMode.NotSigned
            ElseIf SearchBox1.Text.StartsWith("sig:") Then
                modeToUse = SearchMode.Signed
            Else
                modeToUse = SearchMode.None
            End If
            SearchDrivers(SearchBox1.Text, modeToUse)
        Else
            DynaLog.LogMessage("No search query has been specified. Showing all items...")
            If MainForm.CurrentImage.ImageDrivers Is Nothing OrElse MainForm.CurrentImage.ImageDrivers.Count = 0 Then
                ListView1.Items.AddRange(MainForm.CurrentImage.ImageDrivers_Backup.Select(Function(driver) New ListViewItem(New String() {driver.DriverPublishedName, Path.GetFileName(driver.DriverOriginalFileName)})).ToArray())
            Else
                InstalledDriverList.AddRange(MainForm.CurrentImage.ImageDrivers.Select(Function(driver) driver))
                ListView1.Items.AddRange(MainForm.CurrentImage.ImageDrivers.Select(Function(driver) New ListViewItem(New String() {driver.PublishedName, Path.GetFileName(driver.OriginalFileName)})).ToArray())
            End If
        End If
    End Sub

    Private Sub SearchBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles SearchBox1.KeyDown
        If e.KeyCode = Keys.Back And e.Control Then
            Dim text As String = SearchBox1.Text
            Dim lastSpaceIndex As Integer = text.LastIndexOf(" "c)
            If lastSpaceIndex > 0 Then
                SearchBox1.Text = text.Substring(0, lastSpaceIndex).TrimEnd()
            Else
                SearchBox1.Text = ""
            End If
            e.SuppressKeyPress = True
            SearchBox1.SelectionStart = SearchBox1.TextLength
        End If
    End Sub

    Private Sub WizardBtn_Click(sender As Object, e As EventArgs) Handles WizardBtn.Click
        Try
            If InstalledDriverList_Backup.Count > InstalledDriverList.Count Then
                DriverFilterAssistantDialog.ProvidedImageClassNames = InstalledDriverList_Backup.Select(Function(driver) driver.DriverClassName).Distinct().ToList()
            Else
                DriverFilterAssistantDialog.ProvidedImageClassNames = InstalledDriverList.Select(Function(driver) driver.ClassName).Distinct().ToList()
            End If
        Catch ex As Exception
            ' ignore
        End Try
        If DriverFilterAssistantDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            SearchBox1.Text = DriverFilterAssistantDialog.AppliedQuery
        End If
    End Sub

    Private Sub WizardBtn_MouseHover(sender As Object, e As EventArgs) Handles WizardBtn.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("GetDriverInfo")("Build.Query.Assistant.Label"))
    End Sub
End Class
