Imports System.Windows.Forms
Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports DISMTools.Utilities
Imports System.Globalization

Public Class GetPkgInfoDlg

    Dim PackageInfoExList As New List(Of DismPackageInfoEx)
    Dim PackageInfoList As New List(Of DismPackageInfo)
    Dim OSVer As Version

    Private Sub GetPkgInfoDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListBox2.BackColor = CurrentTheme.SectionBackgroundColor
        cPropPathView.BackColor = CurrentTheme.SectionBackgroundColor
        cPropValue.BackColor = CurrentTheme.SectionBackgroundColor
        cPropValue.Font = New Font(MainForm.LogFont, MainForm.LogFontSize, If(MainForm.LogFontIsBold, FontStyle.Bold, FontStyle.Regular))
        SearchBox1.BackColor = BackColor
        SearchBox1.ForeColor = ForeColor
        cPropPathView.ForeColor = ForeColor
        cPropValue.ForeColor = ForeColor
        SearchPic.Image = GetGlyphResource("search")
        Text = LocalizationService.ForSection("GetPkgInfo")("Package.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("GetPkgInfo").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("GetPkgInfo")("Get.Label")
        Label3.Text = LocalizationService.ForSection("GetPkgInfo")("Get.Packages.Message")
        Label4.Text = LocalizationService.ForSection("GetPkgInfo")("AddPackages.Help.Message")
        Label5.Text = LocalizationService.ForSection("GetPkgInfo")("Ready.Label")
        Label6.Text = LocalizationService.ForSection("GetPkgInfo")("Add.Package.File.Label")
        Label7.Text = LocalizationService.ForSection("GetPkgInfo")("PackageInfo.Label")
        Label8.Text = LocalizationService.ForSection("GetPkgInfo")("PackageName.Label")
        Label10.Text = LocalizationService.ForSection("GetPkgInfo")("Package.Applicable.Label")
        Label12.Text = LocalizationService.ForSection("GetPkgInfo")("Copyright.Label")
        Label14.Text = LocalizationService.ForSection("GetPkgInfo")("ProductVersion.Label")
        Label16.Text = LocalizationService.ForSection("GetPkgInfo")("ReleaseType.Label")
        Label18.Text = LocalizationService.ForSection("GetPkgInfo")("Company.Label")
        Label20.Text = LocalizationService.ForSection("GetPkgInfo")("CreationTime.Label")
        Label22.Text = LocalizationService.ForSection("GetPkgInfo")("PackageName.Label")
        Label24.Text = LocalizationService.ForSection("GetPkgInfo")("Package.Applicable.Label")
        Label26.Text = LocalizationService.ForSection("GetPkgInfo")("Copyright.Label")
        Label28.Text = LocalizationService.ForSection("GetPkgInfo")("InstallTime.Label")
        Label30.Text = LocalizationService.ForSection("GetPkgInfo")("Last.Update.Time.Label")
        Label31.Text = LocalizationService.ForSection("GetPkgInfo")("Company.Label")
        Label33.Text = LocalizationService.ForSection("GetPkgInfo")("Install.Package.Name.Label")
        Label36.Text = LocalizationService.ForSection("GetPkgInfo")("PackageInfo.Label")
        Label37.Text = LocalizationService.ForSection("GetPkgInfo")("Installed.Package.View.Label")
        Label39.Text = LocalizationService.ForSection("GetPkgInfo")("DisplayName.Label")
        Label41.Text = LocalizationService.ForSection("GetPkgInfo")("CreationTime.Label")
        Label43.Text = LocalizationService.ForSection("GetPkgInfo")("Description.Label")
        Label45.Text = LocalizationService.ForSection("GetPkgInfo")("ProductName.Label")
        Label47.Text = LocalizationService.ForSection("GetPkgInfo")("InstallClient.Label")
        Label48.Text = LocalizationService.ForSection("GetPkgInfo")("RestartRequired.Label")
        Label50.Text = LocalizationService.ForSection("GetPkgInfo")("SupportInfo.Label")
        Label52.Text = LocalizationService.ForSection("GetPkgInfo")("State.Label")
        Label54.Text = LocalizationService.ForSection("GetPkgInfo")("Boot.Up.Required.Label")
        Label58.Text = LocalizationService.ForSection("GetPkgInfo")("CustomProps.Label")
        Label60.Text = LocalizationService.ForSection("GetPkgInfo")("Features.Label")
        Label61.Text = LocalizationService.ForSection("GetPkgInfo")("Capability.Identity.Label")
        Label63.Text = LocalizationService.ForSection("GetPkgInfo")("Description.Label")
        Label65.Text = LocalizationService.ForSection("GetPkgInfo")("InstallClient.Label")
        Label67.Text = LocalizationService.ForSection("GetPkgInfo")("Install.Package.Name.Label")
        Label69.Text = LocalizationService.ForSection("GetPkgInfo")("InstallTime.Label")
        Label71.Text = LocalizationService.ForSection("GetPkgInfo")("Last.Update.Time.Label")
        Label73.Text = LocalizationService.ForSection("GetPkgInfo")("DisplayName.Label")
        Label75.Text = LocalizationService.ForSection("GetPkgInfo")("ProductName.Label")
        Label77.Text = LocalizationService.ForSection("GetPkgInfo")("ProductVersion.Label")
        Label79.Text = LocalizationService.ForSection("GetPkgInfo")("ReleaseType.Label")
        Label81.Text = LocalizationService.ForSection("GetPkgInfo")("RestartRequired.Label")
        Label83.Text = LocalizationService.ForSection("GetPkgInfo")("SupportInfo.Label")
        Label85.Text = LocalizationService.ForSection("GetPkgInfo")("State.Label")
        Label87.Text = LocalizationService.ForSection("GetPkgInfo")("Boot.Up.Required.Label")
        Label89.Text = LocalizationService.ForSection("GetPkgInfo")("Capability.Identity.Label")
        Label91.Text = LocalizationService.ForSection("GetPkgInfo")("CustomProps.Label")
        Label93.Text = LocalizationService.ForSection("GetPkgInfo")("Features.Label")
        LinkLabel1.Text = LocalizationService.ForSection("GetPkgInfo")("GoBack.Link")
        Button1.Text = LocalizationService.ForSection("GetPkgInfo")("AddPackage.Button")
        Button2.Text = LocalizationService.ForSection("GetPkgInfo")("RemoveSelected.Button")
        Button3.Text = LocalizationService.ForSection("GetPkgInfo")("RemoveAll.Button")
        Button4.Text = LocalizationService.ForSection("GetPkgInfo")("Save.Button")
        InstalledPackageLink.Text = LocalizationService.ForSection("GetPkgInfo")("Iwant.Link")
        PackageFileLink.Text = LocalizationService.ForSection("GetPkgInfo")("PackageFile.Link")
        OpenFileDialog1.Title = LocalizationService.ForSection("GetPkgInfo")("Locate.Package.Files.Title")
        SearchBox1.cueBanner = LocalizationService.ForSection("GetPkgInfo")("Type.Search.Package.Label")
        ListBox1.ForeColor = ForeColor
        ListBox2.ForeColor = ForeColor
        If SplitContainer1.SplitterDistance = 440 Then
            SplitContainer1.SplitterDistance = WindowHelper.ScaleLogical(SplitContainer1.SplitterDistance)
            SplitContainer2.SplitterDistance = WindowHelper.ScaleLogical(SplitContainer2.SplitterDistance)
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        ' Populate installed package listing
        DynaLog.LogMessage("Updating items in list...")
        ListBox2.Items.Clear()
        DynaLog.LogMessage("Getting installed packages...")
        If MainForm.CurrentImage.ImagePackages Is Nothing OrElse MainForm.CurrentImage.ImagePackages.Count = 0 Then
            ListBox2.Items.AddRange(MainForm.CurrentImage.ImagePackages_Backup.Select(Function(package) package.PackageName).ToArray())
        Else
            ListBox2.Items.AddRange(MainForm.CurrentImage.ImagePackages.Select(Function(package) package.PackageName).ToArray())
        End If
        NoPkgPanel.Visible = True
        PackageFileInfoPanel.Visible = False
        Panel4.Visible = False
        Panel7.Visible = True
        SearchBox1.Text = ""

        OSVer = Environment.OSVersion.Version
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MenuPanel.Visible = True
        PackageInfoPanel.Visible = False
    End Sub

    Private Sub InstalledPackageLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles InstalledPackageLink.LinkClicked
        MenuPanel.Visible = False
        PackageInfoPanel.Visible = True
        InfoFromInstalledPkgsPanel.Visible = True
        InfoFromPackageFilesPanel.Visible = False
        Button4.Enabled = True
    End Sub

    Private Sub PackageFileLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles PackageFileLink.LinkClicked
        MenuPanel.Visible = False
        PackageInfoPanel.Visible = True
        InfoFromInstalledPkgsPanel.Visible = False
        InfoFromPackageFilesPanel.Visible = True
        Button4.Enabled = ListBox1.Items.Count > 0
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        WindowHelper.DisableCloseCapability(Handle)
        Try
            If ListBox2.SelectedItems.Count = 1 Then
                ' Background processes need to have completed before showing information
                DynaLog.LogMessage("Checking if background processes are busy...")
                If MainForm.ImgBW.IsBusy Then
                    DynaLog.LogMessage("Background processes are busy. Stopping them...")
                    Dim msg As String = ""
                    msg = LocalizationService.ForSection("GetPkgInfo.PackageList")("Wait.Background.Message")
                    MsgBox(msg, vbOKOnly + vbInformation, ImageTaskHeader1.ItemText)
                    Label5.Text = LocalizationService.ForSection("GetPkgInfo.PackageList")("Waiting.Background.Label")
                    While MainForm.ImgBW.IsBusy
                        Application.DoEvents()
                        Thread.Sleep(500)
                    End While
                End If
                MainForm.StopMountedImageDetector()
                cPropPathView.Nodes.Clear()
                cPropName.Text = ""
                cPropValue.Text = ""
                Label5.Text = LocalizationService.ForSection("GetPkgInfo.PackageList")("Preparing.Package.Item")
                Application.DoEvents()
                Try
                    DynaLog.LogMessage("Initializing API...")
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    DynaLog.LogMessage("Creating session...")
                    Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                        DynaLog.LogMessage("Package to get information about: " & Quote & ListBox2.SelectedItem & Quote)
                        Label5.Text = LocalizationService.ForSection("GetPkgInfo.PackageList").Format("GettingInfo.Item", ListBox2.SelectedItem)
                        Dim PkgInfoEx As DismPackageInfoEx = Nothing
                        Dim PkgInfo As DismPackageInfo = Nothing
                        ' On Windows 10 and later, use the extended version, as DISM gets extended package information.
                        ' Windows 8 and earlier cannot use the extended type, as no "Ex" function is declared in their DISM API DLL
                        DynaLog.LogMessage("Detecting conditions imposed by host system...")
                        If OSVer.Major >= 10 Then
                            DynaLog.LogMessage("Host system is running Windows 10 or 11. Capability information can be obtained alongside the package.")
                            PkgInfoEx = DismApi.GetPackageInfoExByName(imgSession, ListBox2.SelectedItem)
                        Else
                            DynaLog.LogMessage("Host system is running Windows 8. Capability information cannot be obtained alongside the package.")
                            PkgInfo = DismApi.GetPackageInfoByName(imgSession, ListBox2.SelectedItem)
                        End If
                        Label23.Text = If(OSVer.Major >= 10, PkgInfoEx.PackageName, PkgInfo.PackageName)
                        Label25.Text = Casters.Applicability(If(OSVer.Major >= 10, PkgInfoEx.Applicable, PkgInfo.Applicable), True)
                        Label35.Text = If(OSVer.Major >= 10, PkgInfoEx.Copyright, PkgInfo.Copyright)
                        Label32.Text = If(OSVer.Major >= 10, PkgInfoEx.Company, PkgInfo.Company)
                        Label40.Text = If(OSVer.Major >= 10, PkgInfoEx.CreationTime, PkgInfo.CreationTime)
                        Label42.Text = If(OSVer.Major >= 10, PkgInfoEx.Description, PkgInfo.Description)
                        Label46.Text = If(OSVer.Major >= 10, PkgInfoEx.InstallClient, PkgInfo.InstallClient)
                        Label34.Text = If(OSVer.Major >= 10, PkgInfoEx.InstallPackageName, PkgInfo.InstallPackageName)

                        Dim CurrentOSCulture As CultureInfo = CultureInfo.CurrentCulture
                        Dim PackageInstallTime As Date = If(OSVer.Major >= 10, PkgInfoEx.InstallTime, PkgInfo.InstallTime),
                            PackageLastUpdate As Date = If(OSVer.Major >= 10, PkgInfoEx.LastUpdateTime, PkgInfo.LastUpdateTime)
                        Dim PackageInstallTimeString As String = "",
                            PackageLastUpdateString As String = ""
                        If MainForm.HumanizeDates Then
                            PackageInstallTimeString = String.Format("{0}, {1}", PackageInstallTime.ToString(CurrentOSCulture.DateTimeFormat.LongDatePattern, CurrentOSCulture), PackageInstallTime.ToString(CurrentOSCulture.DateTimeFormat.LongTimePattern, CurrentOSCulture))
                            PackageLastUpdateString = String.Format("{0}, {1}", PackageLastUpdate.ToString(CurrentOSCulture.DateTimeFormat.LongDatePattern, CurrentOSCulture), PackageLastUpdate.ToString(CurrentOSCulture.DateTimeFormat.LongTimePattern, CurrentOSCulture))
                        Else
                            PackageInstallTimeString = PackageInstallTime.ToString("MM/dd/yyyy HH:mm:ss")
                            PackageLastUpdateString = PackageLastUpdate.ToString("MM/dd/yyyy HH:mm:ss")
                        End If

                        Label27.Text = PackageInstallTimeString
                        Label29.Text = PackageLastUpdateString
                        Label38.Text = If(OSVer.Major >= 10, PkgInfoEx.DisplayName, PkgInfo.DisplayName)
                        Label44.Text = If(OSVer.Major >= 10, PkgInfoEx.ProductName, PkgInfo.ProductName)
                        Label15.Text = If(OSVer.Major >= 10, PkgInfoEx.ProductVersion.ToString(), PkgInfo.ProductVersion.ToString())
                        Label21.Text = Casters.CastDismReleaseType(If(OSVer.Major >= 10, PkgInfoEx.ReleaseType, PkgInfo.ReleaseType), True)
                        Label13.Text = Casters.CastDismRestartType(If(OSVer.Major >= 10, PkgInfoEx.RestartRequired, PkgInfo.RestartRequired), True)
                        Label49.Text = If(OSVer.Major >= 10, PkgInfoEx.SupportInformation, PkgInfo.SupportInformation)
                        Label51.Text = Casters.CastDismPackageState(If(OSVer.Major >= 10, PkgInfoEx.PackageState, PkgInfo.PackageState), True)
                        Label53.Text = Casters.OfflineInstallType(If(OSVer.Major >= 10, PkgInfoEx.FullyOffline, PkgInfo.FullyOffline), True)
                        If OSVer.Major >= 10 Then Label56.Text = PkgInfoEx.CapabilityId Else Label56.Text = ""
                        Label57.Text = ""
                        Dim cProps As DismCustomPropertyCollection = If(OSVer.Major >= 10, PkgInfoEx.CustomProperties, PkgInfo.CustomProperties)
                        DynaLog.LogMessage("Custom property count: " & cProps.Count)
                        If cProps.Count > 0 Then
                            DynaLog.LogMessage("This package has custom properties.")
                            Label57.Visible = False
                            CPropViewer.Visible = True
                            Dim cPropContents As String = ""
                            For Each cProp As DismCustomProperty In cProps
                                cPropContents &= "- " & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value & CrLf
                            Next
                            PopulateTreeView(cPropPathView, cPropContents.Replace("- ", "").Trim())
                            cPropValue.Text = LocalizationService.ForSection("GetPkgInfo.PackageList")("Expand.Entry.Label")
                        Else
                            DynaLog.LogMessage("This package does not have custom properties.")
                            Label57.Text = LocalizationService.ForSection("GetPkgInfo.PackageList")("None.Label")
                            Label57.Visible = True
                            CPropViewer.Visible = False
                        End If
                        Label59.Text = ""
                        Dim pkgFeats As DismFeatureCollection = If(OSVer.Major >= 10, PkgInfoEx.Features, PkgInfo.Features)
                        DynaLog.LogMessage("Feature count: " & pkgFeats.Count)
                        If pkgFeats.Count > 0 Then
                            DynaLog.LogMessage("This package has features.")
                            ' Output all features
                            For Each pkgFeat As DismFeature In pkgFeats
                                Label59.Text &= "- " & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State, True) & ")" & CrLf
                            Next
                        Else
                            DynaLog.LogMessage("This package does not have features.")
                            Label59.Text = LocalizationService.ForSection("GetPkgInfo.PackageList")("None.Label")
                        End If
                    End Using
                    Panel4.Visible = True
                    Panel7.Visible = False
                    Label5.Text = LocalizationService.ForSection("GetPkgInfo.PackageList")("Ready.Item")
                Finally
                    Try
                        DismApi.Shutdown()
                    Catch ex As Exception

                    End Try
                End Try
            Else
                Panel4.Visible = False
                Panel7.Visible = True
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not get package information. Error message: " & ex.Message)
            Panel4.Visible = False
            Panel7.Visible = True
        End Try
        WindowHelper.EnableCloseCapability(Handle)
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
            cPropValue.Text = LocalizationService.ForSection("GetPkgInfo.PropertyPath")("SelectedValue.Message")
        End If
    End Sub

    Sub GetPackageFileInformation()
        WindowHelper.DisableCloseCapability(Handle)
        DynaLog.LogMessage("Clearing information lists...")
        PackageInfoList.Clear()
        PackageInfoExList.Clear()
        Try
            ' Background processes need to have completed before showing information
            DynaLog.LogMessage("Checking if background processes are busy...")
            If MainForm.ImgBW.IsBusy Then
                DynaLog.LogMessage("Background processes are busy. Stopping them...")
                Dim msg As String = ""
                msg = LocalizationService.ForSection("PackageInfo.File")("Wait.Background.Message")
                MsgBox(msg, vbOKOnly + vbInformation, ImageTaskHeader1.ItemText)
                Label5.Text = LocalizationService.ForSection("PackageInfo.File")("Waiting.Background.Label")
                While MainForm.ImgBW.IsBusy
                    Application.DoEvents()
                    Thread.Sleep(500)
                End While
            End If
            MainForm.StopMountedImageDetector()
            Label5.Text = LocalizationService.ForSection("PackageInfo.File")("Preparing.Item")
            Application.DoEvents()
            Try
                DynaLog.LogMessage("Initializing API...")
                DismApi.Initialize(DismLogLevel.LogErrors)
                DynaLog.LogMessage("Creating session...")
                Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                    For Each pkgFile In ListBox1.Items
                        Try
                            DynaLog.LogMessage("Package file to get information about: " & Quote & Path.GetFileName(pkgFile) & Quote)
                            If File.Exists(pkgFile) Then
                                DynaLog.LogMessage("Package file exists.")
                                Label5.Text = LocalizationService.ForSection("PackageInfo.File").Format("Loading.Package.Message", Path.GetFileName(pkgFile))
                                Application.DoEvents()
                                Dim pkgInfoEx As DismPackageInfoEx = Nothing
                                Dim pkgInfo As DismPackageInfo = Nothing
                                DynaLog.LogMessage("Detecting conditions imposed by host system...")
                                If OSVer.Major >= 10 Then
                                    DynaLog.LogMessage("Host system is running Windows 10 or 11. Capability information can be obtained alongside the package.")
                                    pkgInfoEx = DismApi.GetPackageInfoExByPath(imgSession, pkgFile)
                                Else
                                    DynaLog.LogMessage("Host system is running Windows 8. Capability information cannot be obtained alongside the package.")
                                    pkgInfo = DismApi.GetPackageInfoByPath(imgSession, pkgFile)
                                End If
                                If pkgInfoEx IsNot Nothing Then PackageInfoExList.Add(pkgInfoEx)
                                If pkgInfo IsNot Nothing Then PackageInfoList.Add(pkgInfo)
                            End If
                        Catch PkgInfoEx As DismException
                            DynaLog.LogMessage("Could not get package file information. Error message: " & PkgInfoEx.Message)
                        End Try
                    Next
                End Using
            Catch DISMEx As DismException
                DynaLog.LogMessage("Could not get package file information. Error message: " & DISMEx.Message)
                MsgBox(DISMEx.Message & String.Format(LocalizationService.ForSection("GetPkgInfo.Messages")("Hresult.Label"), Hex(DISMEx.HResult)), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            Finally
                Try
                    DismApi.Shutdown()
                Catch ex As Exception

                End Try
            End Try
        Catch ex As Exception
            ' Cancel it
        End Try
        DynaLog.LogMessage("This process has finished.")
        Label5.Text = LocalizationService.ForSection("PackageInfo.File")("Ready.Item")
        WindowHelper.EnableCloseCapability(Handle)
    End Sub

    Sub DisplayPackageFileInformation(PkgFile As Integer)
        DynaLog.LogMessage("Displaying information of a specific package file...")
        DynaLog.LogMessage("Index of Selected Package File: " & PkgFile)
        Label9.Text = If(OSVer.Major >= 10, PackageInfoExList(PkgFile).PackageName, PackageInfoList(PkgFile).PackageName)
        Label11.Text = Casters.Applicability(If(OSVer.Major >= 10, PackageInfoExList(PkgFile).Applicable, PackageInfoList(PkgFile).Applicable), True)
        Label17.Text = If(OSVer.Major >= 10, PackageInfoExList(PkgFile).Copyright, PackageInfoList(PkgFile).Copyright)
        Label19.Text = If(OSVer.Major >= 10, PackageInfoExList(PkgFile).Company, PackageInfoList(PkgFile).Company)
        Label62.Text = If(OSVer.Major >= 10, PackageInfoExList(PkgFile).CreationTime, PackageInfoList(PkgFile).CreationTime)
        Label64.Text = If(OSVer.Major >= 10, PackageInfoExList(PkgFile).Description, PackageInfoList(PkgFile).Description)
        Label66.Text = If(OSVer.Major >= 10, PackageInfoExList(PkgFile).InstallClient, PackageInfoList(PkgFile).InstallClient)
        Label68.Text = If(OSVer.Major >= 10, PackageInfoExList(PkgFile).InstallPackageName, PackageInfoList(PkgFile).InstallPackageName)
        Label70.Text = If(OSVer.Major >= 10, PackageInfoExList(PkgFile).InstallTime, PackageInfoList(PkgFile).InstallTime)
        Label72.Text = If(OSVer.Major >= 10, PackageInfoExList(PkgFile).LastUpdateTime, PackageInfoList(PkgFile).LastUpdateTime)
        Label74.Text = If(OSVer.Major >= 10, PackageInfoExList(PkgFile).DisplayName, PackageInfoList(PkgFile).DisplayName)
        Label76.Text = If(OSVer.Major >= 10, PackageInfoExList(PkgFile).ProductName, PackageInfoList(PkgFile).ProductName)
        Label78.Text = If(OSVer.Major >= 10, PackageInfoExList(PkgFile).ProductVersion.ToString(), PackageInfoList(PkgFile).ProductVersion.ToString())
        Label80.Text = Casters.CastDismReleaseType(If(OSVer.Major >= 10, PackageInfoExList(PkgFile).ReleaseType, PackageInfoList(PkgFile).ReleaseType), True)
        Label82.Text = Casters.CastDismRestartType(If(OSVer.Major >= 10, PackageInfoExList(PkgFile).RestartRequired, PackageInfoList(PkgFile).RestartRequired), True)
        Label84.Text = If(OSVer.Major >= 10, PackageInfoExList(PkgFile).SupportInformation, PackageInfoList(PkgFile).SupportInformation)
        Label86.Text = Casters.CastDismPackageState(If(OSVer.Major >= 10, PackageInfoExList(PkgFile).PackageState, PackageInfoList(PkgFile).PackageState), True)
        Label88.Text = Casters.OfflineInstallType(If(OSVer.Major >= 10, PackageInfoExList(PkgFile).FullyOffline, PackageInfoList(PkgFile).FullyOffline), True)
        If OSVer.Major >= 10 Then Label90.Text = PackageInfoExList(PkgFile).CapabilityId Else Label90.Text = ""
        Label92.Text = ""
        Dim cProps As DismCustomPropertyCollection = If(OSVer.Major >= 10, PackageInfoExList(PkgFile).CustomProperties, PackageInfoList(PkgFile).CustomProperties)
        DynaLog.LogMessage("Custom property count: " & cProps.Count)
        If cProps.Count > 0 Then
            DynaLog.LogMessage("This package has custom properties.")
            For Each cProp As DismCustomProperty In cProps
                Label92.Text &= "- " & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value & CrLf
            Next
        Else
            DynaLog.LogMessage("This package does not have custom properties.")
            Label92.Text = LocalizationService.ForSection("PackageInfo.Display")("None.Label")
        End If
        Label94.Text = ""
        Dim pkgFeats As DismFeatureCollection = If(OSVer.Major >= 10, PackageInfoExList(PkgFile).Features, PackageInfoList(PkgFile).Features)
        DynaLog.LogMessage("Feature count: " & pkgFeats.Count)
        If pkgFeats.Count > 0 Then
            DynaLog.LogMessage("This package has features.")
            ' Output all features
            For Each pkgFeat As DismFeature In pkgFeats
                Label94.Text &= "- " & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State, True) & ")" & CrLf
            Next
        Else
            DynaLog.LogMessage("This package does not have features.")
            Label94.Text = LocalizationService.ForSection("PackageInfo.Display")("None.Label")
        End If
    End Sub

    Private Sub ListBox1_DragEnter(sender As Object, e As DragEventArgs) Handles ListBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ListBox1_DragDrop(sender As Object, e As DragEventArgs) Handles ListBox1.DragDrop
        Dim PackageFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each PackageFile In PackageFiles
            If Path.GetExtension(PackageFile).EndsWith("cab", StringComparison.OrdinalIgnoreCase) Then
                ListBox1.Items.Add(PackageFile)
            End If
        Next
        Button3.Enabled = True
        Button4.Enabled = True
        GetPackageFileInformation()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Try
            If ListBox1.SelectedItems.Count = 1 Then
                NoPkgPanel.Visible = False
                PackageFileInfoPanel.Visible = True
                Button2.Enabled = True
                If PackageInfoExList.Count > 0 Or PackageInfoList.Count > 0 Then DisplayPackageFileInformation(ListBox1.SelectedIndex)
            Else
                NoPkgPanel.Visible = True
                PackageFileInfoPanel.Visible = False
                Button2.Enabled = False
            End If
        Catch ex As Exception
            ListBox1.Items.Remove(ListBox1.SelectedItem)
            NoPkgPanel.Visible = True
            PackageFileInfoPanel.Visible = False
            If ListBox1.Items.Count < 1 Then
                Button2.Enabled = False
                Button3.Enabled = False
                Button4.Enabled = False
            End If
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            PackageInfoList.RemoveAt(ListBox1.SelectedIndex)
        Catch ex As Exception
            ' Not in there
        End Try
        Try
            PackageInfoExList.RemoveAt(ListBox1.SelectedIndex)
        Catch ex As Exception
            ' Not in there
        End Try
        ListBox1.Items.Remove(ListBox1.SelectedItem)
        If ListBox1.Items.Count >= 1 Then
            Button2.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
        Else
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
        End If
        NoPkgPanel.Visible = True
        PackageFileInfoPanel.Visible = False
        Button2.Enabled = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        PackageInfoList.Clear()
        PackageInfoExList.Clear()
        ListBox1.Items.Clear()
        Button2.Enabled = False
        Button3.Enabled = False
        Button4.Enabled = False
        NoPkgPanel.Visible = True
        PackageFileInfoPanel.Visible = False
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        ListBox1.Items.Add(OpenFileDialog1.FileName)
        Button3.Enabled = True
        Button4.Enabled = True
        GetPackageFileInformation()
    End Sub

    Private Sub GetPkgInfoDlg_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        MainForm.StartMountedImageDetector()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If MainForm.ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Saving package information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            If ImgInfoSaveDlg.PackageFiles.Count > 0 Then ImgInfoSaveDlg.PackageFiles.Clear()
            ImgInfoSaveDlg.SourceImage = MainForm.SourceImg
            ImgInfoSaveDlg.SaveTarget = MainForm.ImgInfoSFD.FileName
            ImgInfoSaveDlg.ImgMountDir = If(Not MainForm.OnlineManagement, MainForm.MountDir, "")
            ImgInfoSaveDlg.OnlineMode = MainForm.OnlineManagement
            ImgInfoSaveDlg.SkipQuestions = MainForm.SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = MainForm.AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = If(InfoFromPackageFilesPanel.Visible, 3, 2)
            If InfoFromPackageFilesPanel.Visible Then
                For Each pkgFile In ListBox1.Items
                    If File.Exists(pkgFile) Then ImgInfoSaveDlg.PackageFiles.Add(pkgFile)
                Next
            End If
            ImgInfoSaveDlg.ImageToGetInfoFrom = MainForm.CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Sub SearchPackages(sQuery As String)
        DynaLog.LogMessage("Search query: " & sQuery)
        If MainForm.CurrentImage.ImagePackages Is Nothing OrElse MainForm.CurrentImage.ImagePackages.Count = 0 Then
            Dim FilteredPackages As IEnumerable(Of ImagePackage) = MainForm.CurrentImage.ImagePackages_Backup.Where(Function(package) package.PackageName.ToLower().Contains(sQuery.ToLower()))
            ListBox2.Items.AddRange(FilteredPackages.Select(Function(package) package.PackageName).ToArray())
        Else
            Dim FilteredPackages As IEnumerable(Of DismPackage) = MainForm.CurrentImage.ImagePackages.Where(Function(package) package.PackageName.ToLower().Contains(sQuery.ToLower()))
            ListBox2.Items.AddRange(FilteredPackages.Select(Function(package) package.PackageName).ToArray())
        End If
    End Sub

    Private Sub SearchBox1_TextChanged(sender As Object, e As EventArgs) Handles SearchBox1.TextChanged
        ListBox2.Items.Clear()
        If SearchBox1.Text <> "" Then
            SearchPackages(SearchBox1.Text)
        Else
            DynaLog.LogMessage("No search query has been specified. Showing all items...")
            If MainForm.CurrentImage.ImagePackages Is Nothing OrElse MainForm.CurrentImage.ImagePackages.Count = 0 Then
                ListBox2.Items.AddRange(MainForm.CurrentImage.ImagePackages_Backup.Select(Function(package) package.PackageName).ToArray())
            Else
                ListBox2.Items.AddRange(MainForm.CurrentImage.ImagePackages.Select(Function(package) package.PackageName).ToArray())
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
End Class
