Imports System.Windows.Forms
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports DISMTools.Utilities

Public Class GetCapabilityInfoDlg

    Dim _lvwColumnSorter As New ListViewColumnSorter()

    Private Sub GetCapabilityInfoDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        SearchBox1.BackColor = BackColor
        SearchBox1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        SearchPic.Image = GetGlyphResource("search")
        WizardBtn.Image = GetGlyphResource("assistant")
        Text = LocalizationService.ForSection("CapabilityInfo")("Get.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("CapabilityInfo").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("CapabilityInfo")("Ready.Label")
        Label22.Text = LocalizationService.ForSection("CapabilityInfo")("Identity.Label")
        Label24.Text = LocalizationService.ForSection("CapabilityInfo")("CapabilityName.Label")
        Label26.Text = LocalizationService.ForSection("CapabilityInfo")("CapabilityState.Label")
        Label31.Text = LocalizationService.ForSection("CapabilityInfo")("DisplayName.Label")
        Label36.Text = LocalizationService.ForSection("CapabilityInfo")("CapabilityInfo.Label")
        Label37.Text = LocalizationService.ForSection("GetCapInfo")("SelectCapability.Label")
        Label41.Text = LocalizationService.ForSection("CapabilityInfo")("Description.Label")
        Label43.Text = LocalizationService.ForSection("CapabilityInfo")("Sizes.Label")
        ListView1.Columns(0).Text = LocalizationService.ForSection("CapabilityInfo")("Identity.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("CapabilityInfo")("State.Column")
        Button2.Text = LocalizationService.ForSection("CapabilityInfo")("Save.Button")
        SearchBox1.cueBanner = LocalizationService.ForSection("CapabilityInfo")("Type.Search.Label")
        If SplitContainer2.SplitterDistance = 440 Then
            SplitContainer2.SplitterDistance = WindowHelper.ScaleLogical(SplitContainer2.SplitterDistance)
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ' Populate feature information list
        Panel4.Visible = False
        Panel7.Visible = True
        Button1.Visible = False
        DynaLog.LogMessage("Updating items in list...")
        ListView1.Items.Clear()
        DynaLog.LogMessage("Getting capabilities...")
        If MainForm.CurrentImage.ImageCapabilities Is Nothing OrElse MainForm.CurrentImage.ImageCapabilities.Count = 0 Then
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageCapabilities_Backup.Select(Function(capability) New ListViewItem(New String() {capability.CapabilityName, Casters.CastDismPackageState(capability.CapabilityState, True)})).ToArray())
        Else
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageCapabilities.Select(Function(capability) New ListViewItem(New String() {capability.Name, Casters.CastDismPackageState(capability.State, True)})).ToArray())
        End If
        SearchBox1.Text = ""
        ColumnHeader1.Width = WindowHelper.ScaleLogical(298)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(118)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        WindowHelper.DisableCloseCapability(Handle)
        DynaLog.LogMessage("Selected items: " & ListView1.SelectedItems.Count)
        Try
            If ListView1.SelectedItems.Count = 1 Then
                ' Background processes need to have completed before showing information
                DynaLog.LogMessage("Checking if background processes are busy...")
                If MainForm.ImgBW.IsBusy Then
                    DynaLog.LogMessage("Background processes are busy. Stopping them...")
                    Dim msg As String = ""
                    msg = LocalizationService.ForSection("CapabilityInfo")("Wait.Background.Message")
                    MsgBox(msg, vbOKOnly + vbInformation, ImageTaskHeader1.ItemText)
                    Label2.Text = LocalizationService.ForSection("CapabilityInfo")("Waiting.Background.Label")
                    While MainForm.ImgBW.IsBusy
                        Application.DoEvents()
                        Thread.Sleep(500)
                    End While
                End If
                MainForm.StopMountedImageDetector()
                Label2.Text = LocalizationService.ForSection("CapabilityInfo")("Prepare.Cap.Item")
                Application.DoEvents()
                Try
                    DynaLog.LogMessage("Initializing API...")
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    DynaLog.LogMessage("Creating session...")
                    Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                        Label2.Text = LocalizationService.ForSection("CapabilityInfo").Format("GettingInfo.Item", ListView1.FocusedItem.SubItems(0).Text)
                        DynaLog.LogMessage("Capability to get information about: " & ListView1.FocusedItem.SubItems(0).Text)
                        Application.DoEvents()
                        Dim capInfo As DismCapabilityInfo = DismApi.GetCapabilityInfo(imgSession, ListView1.FocusedItem.SubItems(0).Text)
                        Label23.Text = capInfo.Name
                        Label25.Text = capInfo.Name.Remove(InStr(capInfo.Name, "~") - 1)
                        Label35.Text = Casters.CastDismPackageState(capInfo.State, True)
                        Label32.Text = capInfo.DisplayName
                        Label40.Text = capInfo.Description
                        Dim isFrenchSizeText As Boolean = LocalizationService.CurrentCultureCode.Equals("fr-FR", StringComparison.OrdinalIgnoreCase)
                        Dim downloadReadableSize As String = If(isFrenchSizeText, Converters.BytesToReadableSize(capInfo.DownloadSize, True), Converters.BytesToReadableSize(capInfo.DownloadSize))
                        Dim installReadableSize As String = If(isFrenchSizeText, Converters.BytesToReadableSize(capInfo.InstallSize, True), Converters.BytesToReadableSize(capInfo.InstallSize))
                        Dim downloadReadableSuffix As String = If(capInfo.DownloadSize >= 1024, LocalizationService.ForSection("CapabilityInfo").Format("ReadableSize.Suffix", downloadReadableSize), "")
                        Dim installReadableSuffix As String = If(capInfo.InstallSize >= 1024, LocalizationService.ForSection("CapabilityInfo").Format("ReadableSize.Suffix", installReadableSize), "")
                        Label42.Text = LocalizationService.ForSection("CapabilityInfo").Format("Download.Size.Bytes.Label", capInfo.DownloadSize, downloadReadableSuffix, capInfo.InstallSize, installReadableSuffix)
                    End Using
                Catch NRE As NullReferenceException
                    Panel4.Visible = False
                    Panel7.Visible = True
                Catch ex As Exception
                    DynaLog.LogMessage("Could not get capability information. Error message: " & ex.Message)
                    Dim msg As String = ""
                    msg = LocalizationService.ForSection("CapabilityInfo").Format("Get.Reason.Message", ex.ToString(), ex.Message, Hex(ex.HResult))
                    MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Finally
                    DynaLog.LogMessage("Shutting down API...")
                    Try
                        DismApi.Shutdown()
                    Catch ex As Exception

                    End Try
                End Try
                Label2.Text = LocalizationService.ForSection("CapabilityInfo")("Ready.Item")
                Panel4.Visible = True
                Panel7.Visible = False
            Else
                Panel4.Visible = False
                Panel7.Visible = True
            End If
        Catch ex As Exception
            Panel4.Visible = False
            Panel7.Visible = True
        End Try
        WindowHelper.EnableCloseCapability(Handle)

        Button1.Visible = (ListView1.SelectedItems.Count = 1)
    End Sub

    Private Sub GetCapabilityInfoDlg_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        MainForm.StartMountedImageDetector()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MainForm.ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Saving capability information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = MainForm.SourceImg
            ImgInfoSaveDlg.ImgMountDir = If(Not MainForm.OnlineManagement, MainForm.MountDir, "")
            ImgInfoSaveDlg.SaveTarget = MainForm.ImgInfoSFD.FileName
            ImgInfoSaveDlg.OnlineMode = MainForm.OnlineManagement
            ImgInfoSaveDlg.OfflineMode = MainForm.OfflineManagement
            ImgInfoSaveDlg.SkipQuestions = MainForm.SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = MainForm.AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = 6
            ImgInfoSaveDlg.ImageToGetInfoFrom = MainForm.CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Sub SearchCapabilities(sQuery As String, Optional capState As String = "")
        DynaLog.LogMessage("Search query: " & sQuery)
        Dim expectedCapabilityState As DismPackageFeatureState = DismPackageFeatureState.NotPresent
        If capState <> "" Then
            DynaLog.LogMessage("Capability state query is not nothing (" & Quote & capState & Quote & ")")
            Select Case capState.ToLower()
                Case "notpresent"
                    expectedCapabilityState = DismPackageFeatureState.NotPresent
                Case "uninstallpending"
                    expectedCapabilityState = DismPackageFeatureState.UninstallPending
                Case "uninstalled"
                    expectedCapabilityState = DismPackageFeatureState.Staged
                Case "removed"
                    expectedCapabilityState = DismPackageFeatureState.Removed
                Case "resolved"
                    expectedCapabilityState = DismPackageFeatureState.Resolved
                Case "installed"
                    expectedCapabilityState = DismPackageFeatureState.Installed
                Case "installpending"
                    expectedCapabilityState = DismPackageFeatureState.InstallPending
                Case "superseded"
                    expectedCapabilityState = DismPackageFeatureState.Superseded
                Case "partiallyinstalled"
                    expectedCapabilityState = DismPackageFeatureState.PartiallyInstalled
            End Select
        End If
        If MainForm.CurrentImage.ImageCapabilities Is Nothing OrElse MainForm.CurrentImage.ImageCapabilities.Count = 0 Then
            Dim finalCapabilityLookup As IEnumerable(Of ImageCapability) = MainForm.CurrentImage.ImageCapabilities_Backup.Where(Function(capability) capability.CapabilityName.ToLowerInvariant().Contains(sQuery.ToLowerInvariant()))
            If capState <> "" Then      ' We filter them again based on the state
                finalCapabilityLookup = finalCapabilityLookup.Where(Function(capability) capability.CapabilityState = expectedCapabilityState)
            End If
            ListView1.Items.AddRange(finalCapabilityLookup.Select(Function(filteredCapability) New ListViewItem(New String() {filteredCapability.CapabilityName, Casters.CastDismPackageState(filteredCapability.CapabilityState, True)})).ToArray())
        Else
            Dim finalCapabilityLookup As IEnumerable(Of DismCapability) = MainForm.CurrentImage.ImageCapabilities.Where(Function(capability) capability.Name.ToLowerInvariant().Contains(sQuery.ToLowerInvariant()))
            If capState <> "" Then      ' We filter them again based on the state
                finalCapabilityLookup = finalCapabilityLookup.Where(Function(capability) capability.State = expectedCapabilityState)
            End If
            ListView1.Items.AddRange(finalCapabilityLookup.Select(Function(filteredCapability) New ListViewItem(New String() {filteredCapability.Name, Casters.CastDismPackageState(filteredCapability.State, True)})).ToArray())
        End If
    End Sub

    Private Sub SearchBox1_TextChanged(sender As Object, e As EventArgs) Handles SearchBox1.TextChanged
        ListView1.Items.Clear()
        If SearchBox1.Text <> "" Then
            If SearchBox1.Text.ToLower().Contains("state:") Then
                Dim state As String = SearchBox1.Text.Substring(SearchBox1.Text.IndexOf("state:") + "state:".Length).Trim()
                SearchCapabilities(SearchBox1.Text.Replace("state:" & state, "").Trim(), state)
            Else
                SearchCapabilities(SearchBox1.Text)
            End If
        Else
            DynaLog.LogMessage("No search query has been specified. Showing all items...")
            If MainForm.CurrentImage.ImageCapabilities Is Nothing OrElse MainForm.CurrentImage.ImageCapabilities.Count = 0 Then
                ListView1.Items.AddRange(MainForm.CurrentImage.ImageCapabilities_Backup.Select(Function(capability) New ListViewItem(New String() {capability.CapabilityName, Casters.CastDismPackageState(capability.CapabilityState, True)})).ToArray())
            Else
                ListView1.Items.AddRange(MainForm.CurrentImage.ImageCapabilities.Select(Function(capability) New ListViewItem(New String() {capability.Name, Casters.CastDismPackageState(capability.State, True)})).ToArray())
            End If
        End If
    End Sub

    Private Sub ListView1_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles ListView1.ColumnClick
        ' From Microsoft documentation: https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/sort-listview-by-column
        DynaLog.LogMessage("Sorting items...")
        DynaLog.LogMessage("Column to sort: " & e.Column + 1)
        DynaLog.LogMessage("Current sort order (may be modified): " & _lvwColumnSorter.Order)
        If e.Column = _lvwColumnSorter.SortColumn Then
            If _lvwColumnSorter.Order = SortOrder.Ascending Then
                _lvwColumnSorter.Order = SortOrder.Descending
            Else
                _lvwColumnSorter.Order = SortOrder.Ascending
            End If
        Else
            _lvwColumnSorter.SortColumn = e.Column
            _lvwColumnSorter.Order = SortOrder.Ascending
        End If

        ' Force sorting
        ListView1.Sorting = _lvwColumnSorter.Order

        ListView1.Sort()
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SearchEngineHelper.InvokeSearchQuery(MainForm.SearchEngineName, String.Format("microsoft windows {0}", Quote & Label23.Text & Quote))
    End Sub

    Private Sub WizardBtn_Click(sender As Object, e As EventArgs) Handles WizardBtn.Click
        If CapabilityFilterAssistantDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            SearchBox1.Text = CapabilityFilterAssistantDialog.AppliedQuery
        End If
    End Sub

    Private Sub WizardBtn_MouseHover(sender As Object, e As EventArgs) Handles WizardBtn.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("CapabilityInfo")("Build.Query.Assistant.Label"))
    End Sub
End Class
