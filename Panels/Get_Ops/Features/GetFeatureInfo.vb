Imports System.Windows.Forms
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports DISMTools.Utilities

Public Class GetFeatureInfoDlg

    Dim _lvwColumnSorter As New ListViewColumnSorter()

    Private Sub GetFeatureInfoDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        cPropPathView.BackColor = CurrentTheme.SectionBackgroundColor
        cPropValue.BackColor = CurrentTheme.SectionBackgroundColor
        SearchBox1.BackColor = BackColor
        SearchBox1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        cPropPathView.ForeColor = ForeColor
        cPropValue.ForeColor = ForeColor
        cPropValue.Font = New Font(MainForm.LogFont, MainForm.LogFontSize, If(MainForm.LogFontIsBold, FontStyle.Bold, FontStyle.Regular))
        SearchPic.Image = GetGlyphResource("search")
        WizardBtn.Image = GetGlyphResource("assistant")
        Text = LocalizationService.ForSection("GetFeatureInfo")("Get.Feature.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("GetFeatureInfo").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("GetFeatureInfo")("Ready.Label")
        Label22.Text = LocalizationService.ForSection("GetFeatureInfo")("FeatureName.Label")
        Label24.Text = LocalizationService.ForSection("GetFeatureInfo")("DisplayName.Label")
        Label26.Text = LocalizationService.ForSection("GetFeatureInfo")("Description.Label")
        Label31.Text = LocalizationService.ForSection("GetFeatureInfo")("RestartRequired.Label")
        Label36.Text = LocalizationService.ForSection("GetFeatureInfo")("FeatureInfo.Label")
        Label37.Text = LocalizationService.ForSection("GetFeatureInfo")("Installed.Left.Label")
        Label41.Text = LocalizationService.ForSection("GetFeatureInfo")("FeatureState.Label")
        Label43.Text = LocalizationService.ForSection("GetFeatureInfo")("CustomProps.Label")
        ListView1.Columns(0).Text = LocalizationService.ForSection("GetFeatureInfo")("FeatureName.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("GetFeatureInfo")("FeatureState.Column")
        Button2.Text = LocalizationService.ForSection("GetFeatureInfo")("Save.Button")
        SearchBox1.cueBanner = LocalizationService.ForSection("GetFeatureInfo")("Type.Search.Label")
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
        DynaLog.LogMessage("Getting features...")
        If MainForm.CurrentImage.ImageFeatures Is Nothing OrElse MainForm.CurrentImage.ImageFeatures.Count = 0 Then
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageFeatures_Backup.Select(Function(InstalledFeature) New ListViewItem(New String() {InstalledFeature.FeatureName, Casters.CastDismFeatureState(InstalledFeature.FeatureState, True)})).ToArray())
        Else
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageFeatures.Select(Function(InstalledFeature) New ListViewItem(New String() {InstalledFeature.FeatureName, Casters.CastDismFeatureState(InstalledFeature.State, True)})).ToArray())
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
                    msg = LocalizationService.ForSection("GetFeatureInfo")("Wait.Background.Message")
                    MsgBox(msg, vbOKOnly + vbInformation, ImageTaskHeader1.ItemText)
                    Label2.Text = LocalizationService.ForSection("GetFeatureInfo")("Waiting.Background.Label")
                    While MainForm.ImgBW.IsBusy
                        Application.DoEvents()
                        Thread.Sleep(500)
                    End While
                End If
                MainForm.StopMountedImageDetector()
                cPropPathView.Nodes.Clear()
                cPropName.Text = ""
                cPropValue.Text = ""
                Label2.Text = LocalizationService.ForSection("GetFeatureInfo")("Preparing.Item")
                Application.DoEvents()
                Try
                    DynaLog.LogMessage("Initializing API...")
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    DynaLog.LogMessage("Creating session...")
                    Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                        Label2.Text = LocalizationService.ForSection("GetFeatureInfo").Format("GettingInfo.Item", ListView1.FocusedItem.SubItems(0).Text)
                        DynaLog.LogMessage("Feature to get information about: " & ListView1.FocusedItem.SubItems(0).Text)
                        Application.DoEvents()
                        Dim featInfo As DismFeatureInfo = DismApi.GetFeatureInfo(imgSession, ListView1.FocusedItem.SubItems(0).Text)
                        Label23.Text = featInfo.FeatureName
                        Label25.Text = featInfo.DisplayName
                        Label35.Text = featInfo.Description
                        Label32.Text = Casters.CastDismRestartType(featInfo.RestartRequired, True)
                        Label40.Text = Casters.CastDismFeatureState(featInfo.FeatureState, True)
                        Dim cProps As DismCustomPropertyCollection = featInfo.CustomProperties
                        DynaLog.LogMessage("Custom property count: " & cProps.Count)
                        If cProps.Count > 0 Then
                            DynaLog.LogMessage("This feature has custom properties.")
                            Label42.Visible = False
                            CPropViewer.Visible = True
                            Dim cPropContents As String = ""
                            For Each cProp As DismCustomProperty In cProps
                                cPropContents &= "- " & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value & CrLf
                            Next
                            PopulateTreeView(cPropPathView, cPropContents.Replace("- ", "").Trim())
                            cPropValue.Text = LocalizationService.ForSection("GetFeatureInfo")("Expand.Entry.Label")
                        Else
                            DynaLog.LogMessage("This feature does not have custom properties.")
                            Label42.Text = LocalizationService.ForSection("GetFeatureInfo")("None.Label")
                            Label42.Visible = True
                            CPropViewer.Visible = False
                        End If
                    End Using
                Catch NRE As NullReferenceException
                    Panel4.Visible = False
                    Panel7.Visible = True
                Catch ex As Exception
                    DynaLog.LogMessage("Could not get feature information. Error message: " & ex.Message)
                    Dim msg As String = ""
                    msg = LocalizationService.ForSection("GetFeatureInfo").Format("Reason.Message", ex.ToString(), ex.Message, Hex(ex.HResult))
                    MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Finally
                    DynaLog.LogMessage("Shutting down API...")
                    Try
                        DismApi.Shutdown()
                    Catch ex As Exception

                    End Try
                End Try
                Label2.Text = LocalizationService.ForSection("GetFeatureInfo")("Ready.Item")
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

        _lvwColumnSorter = New ListViewColumnSorter()
        ListView1.ListViewItemSorter = _lvwColumnSorter
        WindowHelper.EnableCloseCapability(Handle)

        Button1.Visible = (ListView1.SelectedItems.Count = 1)
    End Sub

    Private Sub PopulateTreeView(treeView As TreeView, input As String)
        DynaLog.LogMessage("Populating items in custom property tree view...")
        Dim lines As String() = input.Split(New String() {Environment.NewLine}, StringSplitOptions.None)
        For Each line As String In lines
            ' Split the line at the last colon to get the path and value
            Dim colonIndex As Integer = line.LastIndexOf(": ")
            If colonIndex = -1 Then Continue For ' Skip lines without a colon
            Dim path As String = line.Substring(0, colonIndex).Trim()
            Dim value As String = line.Substring(colonIndex + 1).Trim()
            Dim pathParts As String() = path.Split("\"c)
            AddNodeRecursive(treeView.Nodes, pathParts, 0, value)
        Next
    End Sub

    Private Sub AddNodeRecursive(parentNodes As TreeNodeCollection, parts As String(), startIndex As Integer, value As String)
        If startIndex >= parts.Length Then
            Return
        End If
        Dim nodeName As String = parts(startIndex).Trim()
        Dim existingNode As TreeNode = parentNodes.Cast(Of TreeNode)().FirstOrDefault(Function(n) n.Text = nodeName)
        If existingNode Is Nothing Then
            existingNode = New TreeNode(nodeName)
            parentNodes.Add(existingNode)
        Else
            If startIndex = parts.Length - 1 Then
                ' If the node already exists and we are at the last part, add a new node
                Dim newNode As New TreeNode(nodeName)
                newNode.Tag = value
                parentNodes.Add(newNode)
                Return
            End If
        End If
        If startIndex = parts.Length - 1 Then
            existingNode.Tag = value
        Else
            AddNodeRecursive(existingNode.Nodes, parts, startIndex + 1, value)
        End If
    End Sub

    Private Sub cPropPathView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles cPropPathView.AfterSelect
        cPropName.Text = cPropPathView.SelectedNode.Text
        Dim selectedNode As TreeNode = cPropPathView.SelectedNode
        If selectedNode IsNot Nothing AndAlso selectedNode.Tag IsNot Nothing Then
            DynaLog.LogMessage("Value of selected custom property: " & selectedNode.Tag.ToString())
            cPropValue.Text = selectedNode.Tag.ToString()
        Else
            cPropValue.Text = LocalizationService.ForSection("FeatureInfo.PathSelection")("SelectedValue.Message")
        End If
    End Sub

    Private Sub GetFeatureInfoDlg_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        MainForm.StartMountedImageDetector()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MainForm.ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Saving feature information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = MainForm.SourceImg
            ImgInfoSaveDlg.ImgMountDir = If(Not MainForm.OnlineManagement, MainForm.MountDir, "")
            ImgInfoSaveDlg.SaveTarget = MainForm.ImgInfoSFD.FileName
            ImgInfoSaveDlg.OnlineMode = MainForm.OnlineManagement
            ImgInfoSaveDlg.OfflineMode = MainForm.OfflineManagement
            ImgInfoSaveDlg.SkipQuestions = MainForm.SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = MainForm.AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = 4
            ImgInfoSaveDlg.ImageToGetInfoFrom = MainForm.CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Sub SearchFeatures(sQuery As String, Optional featureState As String = "")
        DynaLog.LogMessage("Search query: " & sQuery)
        Dim expectedFeatureState As DismPackageFeatureState = DismPackageFeatureState.NotPresent
        If featureState <> "" Then
            DynaLog.LogMessage("Feature state query is not nothing (" & Quote & featureState & Quote & ")")
            Select Case featureState.ToLower()
                Case "notpresent"
                    expectedFeatureState = DismPackageFeatureState.NotPresent
                Case "disablepending"
                    expectedFeatureState = DismPackageFeatureState.UninstallPending
                Case "disabled"
                    expectedFeatureState = DismPackageFeatureState.Staged
                Case "removed"
                    expectedFeatureState = DismPackageFeatureState.Removed
                Case "resolved"
                    expectedFeatureState = DismPackageFeatureState.Resolved
                Case "enabled"
                    expectedFeatureState = DismPackageFeatureState.Installed
                Case "enablepending"
                    expectedFeatureState = DismPackageFeatureState.InstallPending
                Case "superseded"
                    expectedFeatureState = DismPackageFeatureState.Superseded
                Case "partiallyinstalled"
                    expectedFeatureState = DismPackageFeatureState.PartiallyInstalled
            End Select
        End If
        If MainForm.CurrentImage.ImageFeatures Is Nothing OrElse MainForm.CurrentImage.ImageFeatures.Count = 0 Then
            Dim finalFeatureLookup As IEnumerable(Of ImageFeature) = MainForm.CurrentImage.ImageFeatures_Backup.Where(Function(feature) feature.FeatureName.ToLowerInvariant().Contains(sQuery.ToLowerInvariant()))
            If featureState <> "" Then      ' We filter them again based on the state
                finalFeatureLookup = finalFeatureLookup.Where(Function(feature) feature.FeatureState = expectedFeatureState)
            End If
            ListView1.Items.AddRange(finalFeatureLookup.Select(Function(filteredFeature) New ListViewItem(New String() {filteredFeature.FeatureName, Casters.CastDismFeatureState(filteredFeature.FeatureState, True)})).ToArray())
        Else
            Dim finalFeatureLookup As IEnumerable(Of DismFeature) = MainForm.CurrentImage.ImageFeatures.Where(Function(feature) feature.FeatureName.ToLowerInvariant().Contains(sQuery.ToLowerInvariant()))
            If featureState <> "" Then      ' We filter them again based on the state
                finalFeatureLookup = finalFeatureLookup.Where(Function(feature) feature.State = expectedFeatureState)
            End If
            ListView1.Items.AddRange(finalFeatureLookup.Select(Function(filteredFeature) New ListViewItem(New String() {filteredFeature.FeatureName, Casters.CastDismFeatureState(filteredFeature.State, True)})).ToArray())
        End If
    End Sub

    Private Sub SearchBox1_TextChanged(sender As Object, e As EventArgs) Handles SearchBox1.TextChanged
        ListView1.Items.Clear()
        If SearchBox1.Text <> "" Then
            If SearchBox1.Text.ToLower().Contains("state:") Then
                Dim state As String = SearchBox1.Text.Substring(SearchBox1.Text.IndexOf("state:") + "state:".Length).Trim()
                SearchFeatures(SearchBox1.Text.Replace("state:" & state, "").Trim(), state)
            Else
                SearchFeatures(SearchBox1.Text)
            End If
        Else
            DynaLog.LogMessage("No search query has been specified. Showing all items...")
            If MainForm.CurrentImage.ImageFeatures Is Nothing OrElse MainForm.CurrentImage.ImageFeatures.Count = 0 Then
                ListView1.Items.AddRange(MainForm.CurrentImage.ImageFeatures_Backup.Select(Function(InstalledFeature) New ListViewItem(New String() {InstalledFeature.FeatureName, Casters.CastDismFeatureState(InstalledFeature.FeatureState, True)})).ToArray())
            Else
                ListView1.Items.AddRange(MainForm.CurrentImage.ImageFeatures.Select(Function(InstalledFeature) New ListViewItem(New String() {InstalledFeature.FeatureName, Casters.CastDismFeatureState(InstalledFeature.State, True)})).ToArray())
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

        ' If we haven't selected items, force sorting
        If ListView1.SelectedItems.Count < 1 Then ListView1.Sorting = _lvwColumnSorter.Order

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
        If FeatureFilterAssistantDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            SearchBox1.Text = FeatureFilterAssistantDialog.AppliedQuery
        End If
    End Sub

    Private Sub WizardBtn_MouseHover(sender As Object, e As EventArgs) Handles WizardBtn.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("GetFeatureInfo")("Build.Query.Assistant.Label"))
    End Sub
End Class
