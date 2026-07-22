Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports DISMTools.Elements
Imports System.Xml
Imports System.Xml.Serialization

Public Class AddProvAppxPackage
    Implements IImageTaskDialog

    ' Variables used by the AppX scanner component
    Dim AppxNameList As New List(Of String)
    Dim AppxPublisherList As New List(Of String)
    Dim AppxVersionList As New List(Of String)
    Public AppxNames(65535) As String
    Public AppxPublishers(65535) As String
    Public AppxVersion(65535) As String

    ' Variables passed to ProgressPanel
    Public AppxPackages(65535) As String
    Public AppxDependencies(65535) As String

    ' Internal variables helpful to pass information
    Public AppxAdditionCount As Integer
    Public AppxDependencyCount As Integer

    Dim LogoAssetPopupForm As New Form()
    Dim LogoAssetPreview As New PictureBox()

    Dim Packages As New List(Of AppxPackage)

    Dim StubPreferences() As String = New String(2) {"", "", ""}

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        AppxAdditionCount = ListView1.Items.Count
        AppxDependencyCount = ListBox1.Items.Count
        ProgressPanel.appxAdditionCount = AppxAdditionCount
        DynaLog.LogMessage("Detecting AppX packages to add...")
        If ListView1.Items.Count = 0 Then
            DynaLog.LogMessage("No items have been added to the queue.")
            MsgBox(LocalizationService.ForSection("AppxProvision.Validation")("Packages.Required.Message"), vbOKOnly + vbCritical, LocalizationService.ForSection("AppxProvision.Validation")("Add.Prov.Title"))
            Exit Sub
        Else
            DynaLog.LogMessage("AppX packages to add to the queue: " & AppxAdditionCount)
            If AppxAdditionCount > 65535 Then
                MsgBox(LocalizationService.ForSection("AppxPackages.Add.Messages")("Right.Only.Message"), vbOKOnly + vbCritical, LocalizationService.ForSection("AppxPackages.Add.Messages")("Prov.Label"))
                Exit Sub
            Else
                DynaLog.LogMessage("Adding AppX packages to queue...")
                For x = 0 To AppxAdditionCount - 1
                    AppxPackages(x) = ListView1.Items(x).Text
                Next
                For x = 0 To AppxDependencyCount - 1
                    AppxDependencies(x) = ListBox1.Items(x).ToString()
                Next
                ' Fill in remote arrays, even empty slots
                For x = 0 To AppxPackages.Length - 1
                    ProgressPanel.appxAdditionPackages(x) = AppxPackages(x)
                Next
                For x = 0 To AppxDependencies.Length - 1
                    ProgressPanel.appxAdditionDependencies(x) = AppxDependencies(x)
                Next
                ProgressPanel.appxAdditionLastPackage = ListView1.Items(AppxAdditionCount - 1).ToString().Replace("ListViewItem: {", "").Trim().Replace("}", "").Trim()
                If AppxDependencyCount > 0 Then
                    ProgressPanel.appxAdditionLastDependency = ListBox1.Items(AppxDependencyCount - 1).ToString()
                Else
                    ProgressPanel.appxAdditionLastDependency = ""
                End If
                DynaLog.LogMessage("Detecting license file status...")
                If CheckBox3.Checked Then
                    DynaLog.LogMessage("A license file is expected to be used.")
                    If TextBox1.Text = "" Then
                        DynaLog.LogMessage("No license file has been specified.")
                        MsgBox(LocalizationService.ForSection("AppxProvision.Validation")("LicenseFile.Required.Message"), vbOKOnly + vbCritical, LocalizationService.ForSection("AppxProvision.Validation")("Add.Prov.Title"))
                        Exit Sub
                    ElseIf Not File.Exists(TextBox1.Text) Then
                        DynaLog.LogMessage("The license file does not exist in the file system.")
                        MsgBox(LocalizationService.ForSection("AppxProvision.Validation")("LicenseNotFound.Message"), vbOKOnly + vbCritical, LocalizationService.ForSection("AppxProvision.Validation")("Add.Prov.Title"))
                        Exit Sub
                    Else
                        DynaLog.LogMessage("The license file exists in the file system.")
                        ProgressPanel.appxAdditionUseLicenseFile = True
                        ProgressPanel.appxAdditionLicenseFile = TextBox1.Text
                    End If
                Else
                    DynaLog.LogMessage("A license file is not expected to be used.")
                    ProgressPanel.appxAdditionUseLicenseFile = False
                    ProgressPanel.appxAdditionLicenseFile = ""
                End If
                DynaLog.LogMessage("Detecting custom data file status...")
                If CheckBox1.Checked Then
                    DynaLog.LogMessage("A custom data file is expected to be used.")
                    If TextBox2.Text = "" Then
                        DynaLog.LogMessage("No custom data file has been specified.")
                        MsgBox(LocalizationService.ForSection("AppxProvision.Validation")("CustomData.Required.Message"), vbOKOnly + vbCritical, LocalizationService.ForSection("AppxProvision.Validation")("Add.Prov.Title"))
                        Exit Sub
                    ElseIf Not File.Exists(TextBox2.Text) Then
                        DynaLog.LogMessage("The custom data file does not exist in the file system.")
                        MsgBox(LocalizationService.ForSection("AppxProvision.Validation")("CustomData.File.Message"), vbOKOnly + vbCritical, LocalizationService.ForSection("AppxProvision.Validation")("Add.Prov.Title"))
                        Exit Sub
                    Else
                        DynaLog.LogMessage("The custom data file exists in the file system.")
                        ProgressPanel.appxAdditionUseCustomDataFile = True
                        ProgressPanel.appxAdditionCustomDataFile = TextBox2.Text
                    End If
                Else
                    DynaLog.LogMessage("A custom data file is not expected to be used.")
                    ProgressPanel.appxAdditionUseCustomDataFile = False
                    ProgressPanel.appxAdditionCustomDataFile = ""
                End If
                If CheckBox4.Checked Then
                    DynaLog.LogMessage("A specific set of regions is expected to be used.")
                    ProgressPanel.appxAdditionUseAllRegions = True
                    ProgressPanel.appxAdditionRegions = "all"
                Else
                    DynaLog.LogMessage("A specific set of regions is not expected to be used.")
                    ProgressPanel.appxAdditionUseAllRegions = False
                    ProgressPanel.appxAdditionRegions = TextBox3.Text
                End If
                If CheckBox2.Checked And Not MainForm.OnlineManagement Then
                    DynaLog.LogMessage("Changes will be committed to the Windows image after adding AppX packages.")
                    ProgressPanel.appxAdditionCommit = True
                Else
                    DynaLog.LogMessage("Changes will not be committed to the Windows image after adding AppX packages (user decided not to commit them or the active installation is being managed.)")
                    ProgressPanel.appxAdditionCommit = False
                End If
            End If
            ProgressPanel.appxAdditionPackageList = Packages
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 37
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
        If Not MainForm.CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) And MainForm.IsWindows8OrHigher(MainForm.MountDir & "\Windows\system32\ntoskrnl.exe") Then
            DynaLog.LogMessage("All requirements are met. Continuing with the task...")
            Return True
        Else
            DynaLog.LogMessage("The image is not supported")
            MsgBox(LocalizationService.ForSection("AppxProvision.Init")("UnsupportedImage.Message"), vbOKOnly + vbCritical, Text)
            Return False
        End If
    End Function

    Private Sub AddProvAppxPackage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        ComboBox1.Items.Clear()
        Text = LocalizationService.ForSection("AppxProvision")("Add.Prov.Item")
        ImageTaskHeader1.ItemText = Text
        Label2.Text = LocalizationService.ForSection("AppxProvision")("Packages.Required.Message")
        Label3.Text = LocalizationService.ForSection("AppxProvision")("Package.Message")
        Label4.Text = LocalizationService.ForSection("AppxProvision")("StubPreference.Item")
        Label5.Text = LocalizationService.ForSection("AppxProvision")("Multiple.App.Regions.Item")
        Label6.Text = LocalizationService.ForSection("AppxProvision")("Entry.List.View.Message")
        Button1.Text = LocalizationService.ForSection("AppxProvision")("AddFile.Item")
        Button2.Text = LocalizationService.ForSection("AppxProvision")("AddFolder.Item")
        Button3.Text = LocalizationService.ForSection("AppxProvision")("Remove.Entries.Item")
        Button4.Text = LocalizationService.ForSection("AppxProvision")("Remove.Dependencies.Item")
        Button5.Text = LocalizationService.ForSection("AppxProvision")("RemoveDependency.Item")
        Button6.Text = LocalizationService.ForSection("AppxProvision")("AddDependency.Item")
        Button7.Text = LocalizationService.ForSection("AppxProvision")("Browse.Button")
        Button8.Text = LocalizationService.ForSection("AppxProvision")("Browse.Button")
        Button9.Text = LocalizationService.ForSection("AppxProvision")("Remove.Selected.Entry.Item")
        Cancel_Button.Text = LocalizationService.ForSection("AppxProvision")("Cancel.Button")
        OK_Button.Text = LocalizationService.ForSection("AppxProvision")("Ok.Button")
        CheckBox1.Text = LocalizationService.ForSection("AppxProvision")("CustomDataFile.Item")
        CheckBox2.Text = LocalizationService.ForSection("AppxProvision")("CommitImage.Item")
        CustomDataFileOFD.Title = LocalizationService.ForSection("AppxProvision")("CustomData.File.Title")
        GroupBox2.Text = LocalizationService.ForSection("AppxProvision")("AppxDependencies.Item")
        GroupBox3.Text = LocalizationService.ForSection("AppxProvision")("AppxRegions.Item")
        LicenseFileOFD.Title = LocalizationService.ForSection("AppxProvision")("LicenseFile.Title")
        LinkLabel1.Text = LocalizationService.ForSection("AppxProvision")("App.Regions.Form.Message")
        LinkLabel1.LinkArea = LocalizationService.GetLinkArea(LinkLabel1.Text, LocalizationService.ForSection("AppxProvision")("Help.Link"))
        ListView1.Columns(0).Text = LocalizationService.ForSection("AppxProvision")("FileFolder.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("AppxProvision")("Type.Column")
        ListView1.Columns(2).Text = LocalizationService.ForSection("AppxProvision")("ApplicationName.Column")
        ListView1.Columns(3).Text = LocalizationService.ForSection("AppxProvision")("App.Publisher.Column")
        ListView1.Columns(4).Text = LocalizationService.ForSection("AppxProvision")("App.Version.Column")
        CheckBox3.Text = LocalizationService.ForSection("AppxProvision")("LicenseFile.CheckBox")
        CheckBox4.Text = LocalizationService.ForSection("AppxProvision")("App.Available.CheckBox")
        UnpackedAppxFolderFBD.Description = LocalizationService.ForSection("AppxProvision")("Folder.Required.Description")
        StubPreferences(0) = LocalizationService.ForSection("AppxProvision")("Configure.Stub.Item")
        StubPreferences(1) = LocalizationService.ForSection("AppxProvision")("Install.Stub.Package.Item")
        StubPreferences(2) = LocalizationService.ForSection("AppxProvision")("Install.Full.Package.Item")
        ComboBox1.Items.AddRange(StubPreferences)
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        GroupBox3.ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        ListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox3.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        ListBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        CheckBox2.Enabled = If(MainForm.OnlineManagement Or MainForm.OfflineManagement, False, True)
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        AppxDetailsPanel.Height = WindowHelper.ScaleLogical(If(ListView1.SelectedItems.Count <= 0, 520, 83))
        Try
            DynaLog.LogMessage("Detecting conditions imposed by DISM version and Windows image for AppX regions and stub package preferences...")
            If (FileVersionInfo.GetVersionInfo(MainForm.DismExe).ProductMajorPart >= 10 And FileVersionInfo.GetVersionInfo(MainForm.DismExe).ProductBuildPart >= 17134) And
                (MainForm.CurrentImage.ImageVersion.Major >= 10 And MainForm.CurrentImage.ImageVersion.Build >= 17134) Then
                DynaLog.LogMessage("All conditions met for AppX regions (image version >= 10.0.17134; DISM version >= 10.0.17134.)")
                GroupBox3.Enabled = True
            Else
                DynaLog.LogMessage("Not all or no conditions met for AppX regions.")
                GroupBox3.Enabled = False
            End If
            If FileVersionInfo.GetVersionInfo(MainForm.DismExe).ProductMajorPart >= 10 And MainForm.CurrentImage.ImageVersion.Major >= 10 Then
                DynaLog.LogMessage("All conditions met for stub package preferences (image version >= 10.0; DISM version >= 10.0.)")
                Panel2.Enabled = True
            Else
                DynaLog.LogMessage("Not all or no conditions met for stub package preferences.")
                Panel2.Enabled = False
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not detect conditions imposed by DISM version and Windows image. Error message: " & ex.Message)
            GroupBox3.Enabled = False
            Panel2.Enabled = False
        End Try

        ColumnHeader1.Width = WindowHelper.ScaleLogical(343)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(120)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(139)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(275)
        ColumnHeader5.Width = WindowHelper.ScaleLogical(162)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AppxFileOFD.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If UnpackedAppxFolderFBD.ShowDialog = Windows.Forms.DialogResult.OK And UnpackedAppxFolderFBD.SelectedPath <> "" Then
            DynaLog.LogMessage("Selected folder for the scan operation: " & Quote & UnpackedAppxFolderFBD.SelectedPath & Quote)
            ScanAppxPackage(True, UnpackedAppxFolderFBD.SelectedPath)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        DynaLog.LogMessage("Clearing queue...")
        Packages.Clear()
        ListView1.Items.Clear()
        Button3.Enabled = False
        Button9.Enabled = False
        NoAppxFilePanel.Visible = True
        AppxFilePanel.Visible = False
        AppxDetailsPanel.Height = WindowHelper.ScaleLogical(520)
        FlowLayoutPanel1.Visible = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        DynaLog.LogMessage("Removing item at index " & ListView1.FocusedItem.Index & "...")
        ListBox1.Items.Clear()
        Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Clear()
        Button4.Enabled = False
        Button5.Enabled = False
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If Not ListBox1.SelectedItem = "" Then
            DynaLog.LogMessage("Removing dependency from dependency list...")
            'Dim dep As New AppxDependency()
            'dep.DependencyFile.Add(ListBox1.SelectedItem)
            Dim deps As New List(Of AppxDependency)
            deps = Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
            deps.RemoveAt(ListBox1.SelectedIndex)
            Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies = deps
            ListBox1.Items.Remove(ListBox1.SelectedItem)
        End If
        If ListBox1.SelectedItem = "" Then
            Button5.Enabled = False
        Else
            Button5.Enabled = True
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        AppxDependencyOFD.ShowDialog(Me)
    End Sub

    Private Sub AppxFileOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles AppxFileOFD.FileOk
        DynaLog.LogMessage("Getting selected file names...")
        DynaLog.LogMessage("Selected files: " & AppxFileOFD.FileNames.Count)
        If AppxFileOFD.FileNames.Count > 0 Then
            For Each AppxFile In AppxFileOFD.FileNames
                DynaLog.LogMessage("Determining extension of file " & Quote & Path.GetFileName(AppxFile) & Quote & "...")
                If Path.GetExtension(AppxFile).Equals(".appinstaller", StringComparison.OrdinalIgnoreCase) Then
                    DynaLog.LogMessage("Selected file is of APPINSTALLER format. Preparing to parse XML file and downloading main package...")
                    If Not AppInstallerDownloader.IsDisposed Then AppInstallerDownloader.Dispose()
                    AppInstallerDownloader.AppInstallerFile = AppxFile
                    If Not File.Exists(AppxFile.Replace(".appinstaller", ".appxbundle").Trim()) Then AppInstallerDownloader.ShowDialog(Me)
                    DynaLog.LogMessage("Detecting if main package exists and scanning it...")
                    If File.Exists(AppxFile.Replace(".appinstaller", ".appxbundle").Trim()) Then ScanAppxPackage(False, AppxFile.Replace(".appinstaller", ".appxbundle").Trim())
                    Continue For
                End If
                ScanAppxPackage(False, AppxFile)
            Next
        End If
    End Sub

    Private Sub AppxDependencyOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles AppxDependencyOFD.FileOk
        DynaLog.LogMessage("Checking if an AppX package is selected before adding specified dependency...")
        If ListView1.SelectedItems.Count = 1 Then
            DynaLog.LogMessage("Specified dependency: " & Quote & Path.GetFileName(AppxDependencyOFD.FileName) & Quote)
            Dim dep As New AppxDependency()
            dep.DependencyFile = AppxDependencyOFD.FileName
            If Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Count > 0 And Not Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Contains(dep) Then
                Dim deps As New List(Of AppxDependency)
                deps = Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
                deps.Add(dep)
                Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies = deps
            ElseIf Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Count = 0 Then
                Dim deps As New List(Of AppxDependency)
                deps = Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
                deps.Add(dep)
                Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies = deps
            End If
            ListBox1.Items.Add(AppxDependencyOFD.FileName)
            If ListBox1.Items.Count > 0 Then
                Button4.Enabled = True
            End If
        End If
    End Sub

    Private Sub LicenseFileOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles LicenseFileOFD.FileOk
        If ListView1.SelectedItems.Count = 1 Then
            DynaLog.LogMessage("Specified license file: " & Quote & LicenseFileOFD.FileName & Quote)
            TextBox1.Text = LicenseFileOFD.FileName
        End If
    End Sub

    Private Sub CustomDataFileOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CustomDataFileOFD.FileOk
        If ListView1.SelectedItems.Count = 1 Then
            DynaLog.LogMessage("Specified custom data file: " & Quote & CustomDataFileOFD.FileName & Quote)
            TextBox2.Text = CustomDataFileOFD.FileName
        End If
    End Sub

    ''' <summary>
    ''' DISMTools AppX header scanner component: version 0.6.1
    ''' </summary>
    ''' <param name="IsFolder">Determines whether the given value for "Package" is a folder</param>
    ''' <param name="Package">The name of the packed or unpacked AppX file. It may be a file containing the full structure, or a folder containing all AppX files</param>
    ''' <remarks>Scans the header of AppX packages to gather application name, publisher, and version information</remarks>
    Sub ScanAppxPackage(IsFolder As Boolean, Package As String)
        DynaLog.LogMessage("Preparing to scan AppX packages...")
        DynaLog.LogMessage("- Is the specified package unpacked? " & If(IsFolder, "Yes", "No"))
        DynaLog.LogMessage("- Specified package: " & Quote & Path.GetFileName(Package) & Quote)
        ' Detect if the package specified is encrypted
        DynaLog.LogMessage("- Extension of specified package: " & Path.GetExtension(Package))
        If Path.GetExtension(Package).Replace(".", "").Trim().StartsWith("e", StringComparison.OrdinalIgnoreCase) AndAlso MainForm.OnlineManagement Then
            DynaLog.LogMessage("Specified package is encrypted and the active installation is being managed.")
            If Not Path.GetExtension(Package).EndsWith("bundle", StringComparison.OrdinalIgnoreCase) Then
                DynaLog.LogMessage("Specified package is a standard encrypted application. Running UnpEax to extract manifest and Store logo assets...")
                Dim uEAppProc As New Process()
                uEAppProc.StartInfo.FileName = Application.StartupPath & "\Tools\UnpEax\UnpEax.exe"
                uEAppProc.StartInfo.Arguments = Quote & Package & Quote
                If Not Debugger.IsAttached Then
                    uEAppProc.StartInfo.CreateNoWindow = True
                    uEAppProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                End If
                uEAppProc.Start()
                uEAppProc.WaitForExit()
                DynaLog.LogMessage("UnpEax process finished with exit code " & Hex(uEAppProc.ExitCode))
                If uEAppProc.ExitCode = 0 Then
                    DynaLog.LogMessage("Getting information from manifest...")
                    If Directory.Exists(Application.StartupPath & "\appxscan") Then Directory.Delete(Application.StartupPath & "\appxscan", True)
                    Directory.CreateDirectory(Application.StartupPath & "\appxscan")
                    Directory.CreateDirectory(Application.StartupPath & "\appxscan\Assets")
                    For Each asset In Directory.GetFiles(Path.Combine(Application.StartupPath, Package.Replace(Path.GetExtension(Package), "").Trim(), "Assets"))
                        File.Copy(asset, Path.Combine(Application.StartupPath, "appxscan\Assets", Path.GetFileName(asset)))
                    Next
                    File.Copy(Path.Combine(Application.StartupPath, Package.Replace(Path.GetExtension(Package), "").Trim(), "AppxManifest.xml"), Application.StartupPath & "\appxscan\AppxManifest.xml")
                    If Directory.Exists(Path.Combine(Application.StartupPath, Package.Replace(Path.GetExtension(Package), "").Trim())) Then
                        Directory.Delete(Path.Combine(Application.StartupPath, Package.Replace(Path.GetExtension(Package), "").Trim()), True)
                    End If
                    Dim EScanner As String()
                    EScanner = File.ReadAllLines(Application.StartupPath & "\appxscan\AppxManifest.xml")
                    Dim EIdScanner As String
                    Dim EcurrentAppxName As String = ""
                    Dim EcurrentAppxPublisher As String = ""
                    Dim EcurrentAppxVersion As String = ""
                    Dim EcurrentAppxArchitecture As String = ""
                    For x = 0 To EScanner.Count - 1
                        If EScanner(x).Contains("<Identity") Then
                            EIdScanner = EScanner(x)
                            Dim serializer As New XmlSerializer(GetType(AppxPackage))
                            Using tReader As TextReader = New StringReader(EIdScanner)
                                Using reader As XmlReader = XmlReader.Create(tReader)
                                    Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                                    DynaLog.LogMessage(id.ToString())
                                    EcurrentAppxName = id.PackageName
                                    EcurrentAppxPublisher = id.PackagePublisher
                                    EcurrentAppxVersion = id.PackageVersion
                                    EcurrentAppxArchitecture = id.PackageArchitecture
                                End Using
                            End Using
                            DynaLog.LogMessage("Adding AppX package to queue...")
                            AppxNameList.Add(EcurrentAppxName)
                            AppxPublisherList.Add(EcurrentAppxPublisher)
                            AppxVersionList.Add(EcurrentAppxVersion)
                            AppxNames = AppxNameList.ToArray()
                            AppxPublishers = AppxPublisherList.ToArray()
                            AppxVersion = AppxVersionList.ToArray()
                            ' Add the package right away
                            If IsFolder Then
                                ListView1.Items.Add(New ListViewItem(New String() {Package, LocalizationService.ForSection("AppxProvision.Scan")("Unpacked.Encrypted.Label"), EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                            Else
                                ListView1.Items.Add(New ListViewItem(New String() {Package, LocalizationService.ForSection("AppxProvision.Scan")("PackedEncrypted.Label"), EcurrentAppxName, EcurrentAppxPublisher, EcurrentAppxVersion}))
                            End If
                            Exit For
                        End If
                    Next
                    Dim extPackage As New AppxPackage()
                    extPackage.PackageFile = Package
                    extPackage.PackageName = EcurrentAppxName
                    extPackage.PackagePublisher = EcurrentAppxPublisher
                    extPackage.PackageVersion = EcurrentAppxVersion
                    extPackage.PackageArchitecture = EcurrentAppxArchitecture
                    extPackage.StubPackageOption = StubPreference.NoPreference
                    If Not Packages.Contains(extPackage) Then Packages.Add(extPackage)
                    Button3.Enabled = True
                    DynaLog.LogMessage("Getting Store logo asset...")
                    GetApplicationStoreLogoAssets("", False, False, Package, EcurrentAppxName)
                    Exit Sub
                End If
            End If
            DynaLog.LogMessage("Specified package is an encrypted bundle application.")
            ' Add the package right away
            If IsFolder Then
                ListView1.Items.Add(New ListViewItem(New String() {Package, LocalizationService.ForSection("AppxProvision.Scan")("Unpacked.Encrypted.Item"), LocalizationService.ForSection("AppxProvision.Scan")("Encrypted.App.Label"), LocalizationService.ForSection("AppxProvision.Scan")("ListItem.Label"), LocalizationService.ForSection("AppxProvision.Scan")("Encrypted.App.Label")}))
            Else
                ListView1.Items.Add(New ListViewItem(New String() {Package, LocalizationService.ForSection("AppxProvision.Scan")("PackedEncrypted.Item"), LocalizationService.ForSection("AppxProvision.Scan")("Encrypted.App.Label"), LocalizationService.ForSection("AppxProvision.Scan")("Encrypted.App.Label"), LocalizationService.ForSection("AppxProvision.Scan")("Encrypted.App.Label")}))
            End If
            Dim encPackage As New AppxPackage()
            encPackage.PackageFile = Package
            encPackage.PackageName = "<Encrypted>"
            encPackage.PackagePublisher = "<Encrypted>"
            encPackage.PackageVersion = "<Encrypted>"
            encPackage.PackageArchitecture = "<Encrypted>"
            encPackage.StubPackageOption = StubPreference.NoPreference
            If Not Packages.Contains(encPackage) Then Packages.Add(encPackage)
            Button3.Enabled = True
            Exit Sub
        ElseIf Path.GetExtension(Package).Replace(".", "").Trim().StartsWith("e", StringComparison.OrdinalIgnoreCase) AndAlso Not MainForm.OnlineManagement Then
            DynaLog.LogMessage("Specified package is encrypted and the active installation is not being managed.")
            Dim msg As String = ""
            msg = LocalizationService.ForSection("AppxProvision.Scan").Format("Package.Encrypted.Message", Package)
            MsgBox(msg, vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        Dim Stepper As Integer = 2
        Dim QuoteCount As Integer = 0
        Dim Scanner As String()
        Dim currentAppxName As String = ""
        Dim currentAppxPublisher As String = ""
        Dim currentAppxVersion As String = ""
        Dim currentAppxArchitecture As String = ""
        Dim pkgName As String = ""
        Dim IdScanner As String
        Dim StubSupported As Boolean = False
        If IsFolder Then
            DynaLog.LogMessage("Specified package is a folder. Detecting contents in folder...")
            If File.Exists(Package & "\AppxMetadata\AppxBundleManifest.xml") Then
                DynaLog.LogMessage("A bundle manifest has been detected. Treating as a bundle package...")
                ' AppXBundle file
                Scanner = File.ReadAllLines(Package & "\AppxMetadata\AppxBundleManifest.xml")
                StubSupported = Scanner.Contains("IsStub=" & Quote & "true" & Quote)
                IdScanner = Scanner(If(Scanner(2).EndsWith("<!--"), 10, 4))
                Dim CharIndex As Integer = 0
                Dim CharNext As Integer
                For Each Character As Char In Scanner(If(Scanner(2).EndsWith("<!--"), 10, 4))
                    CharNext = CharIndex + 1
                    If Not IdScanner(CharIndex) = Quote Then
                        CharIndex += 1
                        Continue For
                    ElseIf IdScanner(CharIndex) = Quote And IdScanner(CharNext) = " " Then
                        CharIndex += 1
                        Continue For
                    Else
                        Character = IdScanner(CharIndex + 1)
                        If Not IdScanner(CharIndex + Stepper) = " " Then
                            If QuoteCount = 3 Then
                                QuoteCount += 1
                                Do
                                    If Character = Quote Then
                                        CharIndex += Stepper - 1
                                        Character = IdScanner(CharIndex - 1)
                                        QuoteCount += 1
                                        Stepper = 2
                                        Exit For
                                    Else
                                        pkgName &= Character.ToString()
                                        Character = IdScanner(CharIndex + Stepper)
                                        Stepper += 1
                                    End If
                                Loop
                            Else
                                QuoteCount += 1
                                CharIndex += Stepper - 1
                                Character = IdScanner(CharIndex + Stepper)
                            End If
                        End If
                    End If
                Next
                pkgName = pkgName.Replace(" ", "%20").Trim()
                QuoteCount = 0
                Stepper = 2
                For x = 0 To Scanner.Count - 1
                    If Scanner(x).Contains("<Identity") Then
                        IdScanner = Scanner(x)

                        ' Certain AppX packages, like .NET runtimes, decide not to end their identity declarations
                        ' on their expected lines. If we detect such a case, we keep appending text to the ID scanner
                        ' to get the full XML string.
                        If Not IdScanner.EndsWith("/>") Then
                            Dim offset As Integer = 1
                            Do Until IdScanner.EndsWith("/>")
                                IdScanner &= String.Format(" {0}", Scanner(x + offset))
                                offset += 1
                            Loop
                        End If

                        Dim serializer As New XmlSerializer(GetType(AppxPackage))
                        Using tReader As TextReader = New StringReader(IdScanner)
                            Using reader As XmlReader = XmlReader.Create(tReader)
                                Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                                DynaLog.LogMessage(id.ToString())
                                currentAppxName = id.PackageName
                                currentAppxPublisher = id.PackagePublisher
                                currentAppxVersion = id.PackageVersion
                                currentAppxArchitecture = id.PackageArchitecture
                            End Using
                        End Using
                        DynaLog.LogMessage("Adding AppX package to queue...")
                        AppxNameList.Add(currentAppxName)
                        AppxPublisherList.Add(currentAppxPublisher)
                        AppxVersionList.Add(currentAppxVersion)
                        AppxNames = AppxNameList.ToArray()
                        AppxPublishers = AppxPublisherList.ToArray()
                        AppxVersion = AppxVersionList.ToArray()
                        Exit For
                    End If
                Next
            ElseIf File.Exists(Package & "\AppxManifest.xml") Then
                DynaLog.LogMessage("A standard manifest has been detected. Treating as a standard package...")
                ' AppX file
                Scanner = File.ReadAllLines(Package & "\AppxManifest.xml")
                For x = 0 To Scanner.Count - 1
                    If Scanner(x).Contains("<Identity") Then
                        IdScanner = Scanner(x)

                        ' Certain AppX packages, like .NET runtimes, decide not to end their identity declarations
                        ' on their expected lines. If we detect such a case, we keep appending text to the ID scanner
                        ' to get the full XML string.
                        If Not IdScanner.EndsWith("/>") Then
                            Dim offset As Integer = 1
                            Do Until IdScanner.EndsWith("/>")
                                IdScanner &= String.Format(" {0}", Scanner(x + offset))
                                offset += 1
                            Loop
                        End If

                        Dim serializer As New XmlSerializer(GetType(AppxPackage))
                        Using tReader As TextReader = New StringReader(IdScanner)
                            Using reader As XmlReader = XmlReader.Create(tReader)
                                Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                                DynaLog.LogMessage(id.ToString())
                                currentAppxName = id.PackageName
                                currentAppxPublisher = id.PackagePublisher
                                currentAppxVersion = id.PackageVersion
                                currentAppxArchitecture = id.PackageArchitecture
                            End Using
                        End Using
                        DynaLog.LogMessage("Adding AppX package to queue...")
                        AppxNameList.Add(currentAppxName)
                        AppxPublisherList.Add(currentAppxPublisher)
                        AppxVersionList.Add(currentAppxVersion)
                        AppxNames = AppxNameList.ToArray()
                        AppxPublishers = AppxPublisherList.ToArray()
                        AppxVersion = AppxVersionList.ToArray()
                        Exit For
                    End If
                Next
            Else
                DynaLog.LogMessage("Either no manifest or an unknown manifest has been detected. This is unknown.")
                ' Unrecognized type
                MsgBox(LocalizationService.ForSection("AppxProvision.Scan")("Folder.Message"), vbOKOnly + vbExclamation, LocalizationService.ForSection("AppxProvision.Scan")("Add.Title"))
                Exit Sub
            End If
            DynaLog.LogMessage("Getting Store logo asset...")
            GetApplicationStoreLogoAssets(pkgName, True, False, Package, currentAppxName)
        Else
            DynaLog.LogMessage("Specified package is not a folder. Beginning to scan package...")
            If Directory.Exists(Application.StartupPath & "\appxscan") Then Directory.Delete(Application.StartupPath & "\appxscan", True)
            Directory.CreateDirectory(Application.StartupPath & "\appxscan")
            DynaLog.LogMessage("Extracting application manifest...")
            AppxScanner.StartInfo.FileName = Application.StartupPath & "\bin\utils\" & If(Environment.Is64BitOperatingSystem, "x64", "x86") & "\7z.exe"
            AppxScanner.StartInfo.Arguments = "e " & Quote & Package & Quote & " " & Quote & If(Path.GetExtension(Package).EndsWith("bundle", StringComparison.OrdinalIgnoreCase), "appxmetadata\appxbundlemanifest.xml", "appxmanifest.xml") & Quote & " -o" & Quote & Application.StartupPath & "\appxscan" & Quote
            AppxScanner.StartInfo.CreateNoWindow = True
            AppxScanner.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            AppxScanner.Start()
            AppxScanner.WaitForExit()
            DynaLog.LogMessage("7-Zip process finished with exit code " & Hex(AppxScanner.ExitCode))
            If AppxScanner.ExitCode = 0 Then
                DynaLog.LogMessage("Detecting application extension...")
                If Path.GetExtension(Package).EndsWith("bundle", StringComparison.OrdinalIgnoreCase) Then
                    DynaLog.LogMessage("This is a bundle package.")
                    DynaLog.LogMessage("Reading manifest...")
                    Scanner = File.ReadAllLines(Application.StartupPath & "\appxscan\AppxBundleManifest.xml")
                    DynaLog.LogMessage("Getting stub package status...")
                    StubSupported = Scanner.Contains("IsStub=" & Quote & "true" & Quote)
                    IdScanner = Scanner(If(Scanner(2).EndsWith("<!--"), 10, 4))
                    Dim CharIndex As Integer = 0
                    Dim CharNext As Integer
                    For Each Character As Char In Scanner(If(Scanner(2).EndsWith("<!--"), 10, 4))
                        CharNext = CharIndex + 1
                        If Not IdScanner(CharIndex) = Quote Then
                            CharIndex += 1
                            Continue For
                        ElseIf IdScanner(CharIndex) = Quote And IdScanner(CharNext) = " " Then
                            CharIndex += 1
                            Continue For
                        Else
                            Character = IdScanner(CharIndex + 1)
                            If Not IdScanner(CharIndex + Stepper) = " " Then
                                If QuoteCount = 3 Then
                                    QuoteCount += 1
                                    Do
                                        If Character = Quote Then
                                            CharIndex += Stepper - 1
                                            Character = IdScanner(CharIndex - 1)
                                            QuoteCount += 1
                                            Stepper = 2
                                            Exit For
                                        Else
                                            pkgName &= Character.ToString()
                                            Character = IdScanner(CharIndex + Stepper)
                                            Stepper += 1
                                        End If
                                    Loop
                                Else
                                    QuoteCount += 1
                                    CharIndex += Stepper - 1
                                    Character = IdScanner(CharIndex + Stepper)
                                End If
                            End If
                        End If
                    Next
                    pkgName = pkgName.Replace(" ", "%20").Trim()
                    QuoteCount = 0
                    Stepper = 2
                    For x = 0 To Scanner.Count - 1
                        If Scanner(x).Contains("<Identity") Then
                            IdScanner = Scanner(x)

                            ' Certain AppX packages, like .NET runtimes, decide not to end their identity declarations
                            ' on their expected lines. If we detect such a case, we keep appending text to the ID scanner
                            ' to get the full XML string.
                            If Not IdScanner.EndsWith("/>") Then
                                Dim offset As Integer = 1
                                Do Until IdScanner.EndsWith("/>")
                                    IdScanner &= String.Format(" {0}", Scanner(x + offset))
                                    offset += 1
                                Loop
                            End If

                            Dim serializer As New XmlSerializer(GetType(AppxPackage))
                            Using tReader As TextReader = New StringReader(IdScanner)
                                Using reader As XmlReader = XmlReader.Create(tReader)
                                    Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                                    DynaLog.LogMessage(id.ToString())
                                    currentAppxName = id.PackageName
                                    currentAppxPublisher = id.PackagePublisher
                                    currentAppxVersion = id.PackageVersion
                                    currentAppxArchitecture = id.PackageArchitecture
                                End Using
                            End Using
                            DynaLog.LogMessage("Adding AppX package to queue...")
                            AppxNameList.Add(currentAppxName)
                            AppxPublisherList.Add(currentAppxPublisher)
                            AppxVersionList.Add(currentAppxVersion)
                            AppxNames = AppxNameList.ToArray()
                            AppxPublishers = AppxPublisherList.ToArray()
                            AppxVersion = AppxVersionList.ToArray()
                            Exit For
                        End If
                    Next
                Else
                    DynaLog.LogMessage("This is a standard package.")
                    DynaLog.LogMessage("Reading manifest...")
                    Scanner = File.ReadAllLines(Application.StartupPath & "\appxscan\AppxManifest.xml")
                    For x = 0 To Scanner.Count - 1
                        If Scanner(x).Contains("<Identity") Then
                            IdScanner = Scanner(x)

                            ' Certain AppX packages, like .NET runtimes, decide not to end their identity declarations
                            ' on their expected lines. If we detect such a case, we keep appending text to the ID scanner
                            ' to get the full XML string.
                            If Not IdScanner.EndsWith("/>") Then
                                Dim offset As Integer = 1
                                Do Until IdScanner.EndsWith("/>")
                                    IdScanner &= String.Format(" {0}", Scanner(x + offset))
                                    offset += 1
                                Loop
                            End If

                            Dim serializer As New XmlSerializer(GetType(AppxPackage))
                            Using tReader As TextReader = New StringReader(IdScanner)
                                Using reader As XmlReader = XmlReader.Create(tReader)
                                    Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                                    DynaLog.LogMessage(id.ToString())
                                    currentAppxName = id.PackageName
                                    currentAppxPublisher = id.PackagePublisher
                                    currentAppxVersion = id.PackageVersion
                                    currentAppxArchitecture = id.PackageArchitecture
                                End Using
                            End Using
                            DynaLog.LogMessage("Adding AppX package to queue...")
                            AppxNameList.Add(currentAppxName)
                            AppxPublisherList.Add(currentAppxPublisher)
                            AppxVersionList.Add(currentAppxVersion)
                            AppxNames = AppxNameList.ToArray()
                            AppxPublishers = AppxPublisherList.ToArray()
                            AppxVersion = AppxVersionList.ToArray()
                            Exit For
                        End If
                    Next
                End If
                DynaLog.LogMessage("Getting Store logo asset...")
                GetApplicationStoreLogoAssets(pkgName, False, Path.GetExtension(Package).EndsWith("bundle", StringComparison.OrdinalIgnoreCase), Package, currentAppxName)
            Else

            End If
        End If
        DynaLog.LogMessage("Detecting items in list...")
        ' Detect ListView items
        If ListView1.Items.Count > 0 Then
            DynaLog.LogMessage("Getting similar items...")
            ' Iterate through the ListView items until we can find an entry with properties similar to those currently obtained
            For Each Item As ListViewItem In ListView1.Items
                If Item.SubItems(2).Text = currentAppxName And Item.SubItems(3).Text = currentAppxPublisher And Item.SubItems(4).Text = currentAppxVersion And Packages(Item.Index).PackageArchitecture = currentAppxArchitecture Then
                    DynaLog.LogMessage("The package has already been added. Cancelling process...")
                    ' Cancel everything
                    MsgBox(LocalizationService.ForSection("AppxProvision.Scan")("Package.Add.Message"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    If Directory.Exists(Application.StartupPath & "\appxscan") Then
                        Directory.Delete(Application.StartupPath & "\appxscan", True)
                    End If
                    Exit Sub
                ElseIf Item.SubItems(2).Text = currentAppxName And Not Item.SubItems(3).Text = currentAppxPublisher Then
                    DynaLog.LogMessage("The package is already present in the list but comes from a different developer/publisher.")
                    Dim msg As String = ""
                    msg = LocalizationService.ForSection("AppxProvision.Scan")("Package.Added.Message")
                    If MsgBox(msg, vbYesNo + vbExclamation, ImageTaskHeader1.ItemText) = MsgBoxResult.Yes Then
                        DynaLog.LogMessage("Changing packages...")
                        ' Set properties
                        Item.SubItems(0).Text = Package
                        Item.SubItems(1).Text = If(IsFolder, LocalizationService.ForSection("AppxProvision.Scan")("Unpacked.Item"), LocalizationService.ForSection("AppxProvision.Scan")("Packed.Item"))
                        Item.SubItems(2).Text = currentAppxName
                        Item.SubItems(3).Text = currentAppxPublisher
                        Item.SubItems(4).Text = currentAppxVersion

                        ' Configure Element list
                        Packages(Item.Index).PackageFile = Package
                        Packages(Item.Index).PackageName = currentAppxName
                        Packages(Item.Index).PackagePublisher = currentAppxPublisher
                        Packages(Item.Index).PackageVersion = currentAppxVersion
                    Else
                        If Directory.Exists(Application.StartupPath & "\appxscan") Then
                            Directory.Delete(Application.StartupPath & "\appxscan", True)
                        End If
                    End If
                    Exit Sub
                ElseIf Item.SubItems(2).Text = currentAppxName And Not Item.SubItems(4).Text = currentAppxVersion Then
                    DynaLog.LogMessage("The package is already present in the list but the specified one is newer.")
                    ' This is a rudimentary check which will run even if specifying an older version. It will be improved, so expect the following enhancements:
                    ' - Cast the version strings to version objects
                    ' - Compare the version objects part by part
                    Dim msg As String = ""
                    msg = LocalizationService.ForSection("AppxProvision.Scan")("Package.Already.Message")
                    If MsgBox(msg, vbYesNo + vbQuestion, ImageTaskHeader1.ItemText) = MsgBoxResult.Yes Then
                        DynaLog.LogMessage("Updating package to add...")
                        ' Set properties
                        Item.SubItems(0).Text = Package
                        Item.SubItems(1).Text = If(IsFolder, LocalizationService.ForSection("AppxProvision.Scan")("ItemSub.Item"), LocalizationService.ForSection("AppxProvision.Scan")("Packed.Item"))
                        Item.SubItems(2).Text = currentAppxName
                        Item.SubItems(3).Text = currentAppxPublisher
                        Item.SubItems(4).Text = currentAppxVersion

                        ' Configure Element list
                        Packages(Item.Index).PackageFile = Package
                        Packages(Item.Index).PackageName = currentAppxName
                        Packages(Item.Index).PackagePublisher = currentAppxPublisher
                        Packages(Item.Index).PackageVersion = currentAppxVersion
                    Else
                        If Directory.Exists(Application.StartupPath & "\appxscan") Then
                            Directory.Delete(Application.StartupPath & "\appxscan", True)
                        End If
                    End If
                    Exit Sub
                End If
            Next
        End If
        DynaLog.LogMessage("Adding item to list...")
        If IsFolder Then
            ListView1.Items.Add(New ListViewItem(New String() {Package, LocalizationService.ForSection("AppxProvision.Scan")("ListItem.Item"), currentAppxName, currentAppxPublisher, currentAppxVersion}))
        Else
            ListView1.Items.Add(New ListViewItem(New String() {Package, LocalizationService.ForSection("AppxProvision.Scan")("Packed.Item"), currentAppxName, currentAppxPublisher, currentAppxVersion}))
        End If
        Dim currentPackage As New AppxPackage()
        currentPackage.PackageFile = Package
        currentPackage.PackageName = currentAppxName
        currentPackage.PackagePublisher = currentAppxPublisher
        currentPackage.PackageVersion = currentAppxVersion
        currentPackage.PackageArchitecture = currentAppxArchitecture
        currentPackage.SupportsStub = StubSupported
        currentPackage.StubPackageOption = StubPreference.NoPreference
        If Not Packages.Contains(currentPackage) Then Packages.Add(currentPackage)
        Button3.Enabled = True
        If Directory.Exists(Application.StartupPath & "\appxscan") Then
            Directory.Delete(Application.StartupPath & "\appxscan", True)
        End If
    End Sub

    ''' <summary>
    ''' Gets the application store logo assets from APPX or APPXBUNDLE packages (also from MSIX and MSIXBUNDLE packages)
    ''' </summary>
    ''' <param name="PackageName">The name of the package. Packages with names containing spaces will replace those with &quot;%20&quot;</param>
    ''' <param name="IsDirectory">Determines if the package given is an unpacked APPX/MSIX/APPXBUNDLE/MSIXBUNDLE file</param>
    ''' <param name="IsBundlePackage">Determines if the package given is an APPXBUNDLE or MSIXBUNDLE package</param>
    ''' <param name="SourcePackage">The path of the source package</param>
    ''' <param name="AppxPackageName">The name of the AppX package, used for storing logo assets in an organized way</param>
    ''' <remarks>If the package processed is an APPXBUNDLE or MSIXBUNDLE package, this procedure will extract the asset contents from the package with the given name. Otherwise, it will directly extract them from the &quot;Assets&quot; folder</remarks>
    Sub GetApplicationStoreLogoAssets(PackageName As String, IsDirectory As Boolean, IsBundlePackage As Boolean, SourcePackage As String, AppxPackageName As String)
        DynaLog.LogMessage("Attempting to get Store logo assets...")
        DynaLog.LogMessage("- Package name: " & PackageName)
        DynaLog.LogMessage("- Is it a directory? " & If(IsDirectory, "Yes", "No"))
        DynaLog.LogMessage("- Is it a bundle package? " & If(IsBundlePackage, "Yes", "No"))
        DynaLog.LogMessage("- Path to source package: " & Quote & SourcePackage & Quote)
        DynaLog.LogMessage("- Package display name: " & AppxPackageName)
        ' The assets from the main package are enough for us. The current AppX XML schema also puts these in the Assets folder, so
        ' getting them should be a breeze
        Try
            If IsDirectory Then
                DynaLog.LogMessage("Specified package is a folder. Detecting contents in folder...")
                If File.Exists(SourcePackage & "\AppxMetadata\AppxBundleManifest.xml") Then
                    DynaLog.LogMessage("A bundle manifest has been detected. Treating as a bundle package...")
                    ' APPXBUNDLE/MSIXBUNDLE
                    DynaLog.LogMessage("Extracting main AppX package from bundle to grab assets...")
                    AppxScanner.StartInfo.Arguments = "x " & Quote & SourcePackage & "\" & PackageName & Quote & " -o" & Quote & Application.StartupPath & "\appxscan" & Quote
                    AppxScanner.Start()
                    AppxScanner.WaitForExit()
                    If Not Directory.Exists(Application.StartupPath & "\temp\storeassets") Then Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets").Attributes = FileAttributes.Hidden
                    DynaLog.LogMessage("7-Zip process finished with exit code " & Hex(AppxScanner.ExitCode))
                    If AppxScanner.ExitCode = 0 Then
                        Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets\" & AppxPackageName)
                        If My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & AppxPackageName).Count <= 0 Then
                            For Each AssetFile In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\appxscan\Assets", FileIO.SearchOption.SearchTopLevelOnly)
                                If Path.GetFileNameWithoutExtension(AssetFile).StartsWith("small", StringComparison.OrdinalIgnoreCase) Then
                                    DynaLog.LogMessage("Copying item " & Quote & Path.GetFileName(AssetFile) & Quote & " to Store logo asset cache...")
                                    File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                                ElseIf Path.GetFileNameWithoutExtension(AssetFile).StartsWith("store", StringComparison.OrdinalIgnoreCase) Then
                                    DynaLog.LogMessage("Copying item " & Quote & Path.GetFileName(AssetFile) & Quote & " to Store logo asset cache...")
                                    File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                                ElseIf Path.GetFileNameWithoutExtension(AssetFile).StartsWith("large", StringComparison.OrdinalIgnoreCase) Then
                                    DynaLog.LogMessage("Copying item " & Quote & Path.GetFileName(AssetFile) & Quote & " to Store logo asset cache...")
                                    File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                                End If
                            Next
                        End If
                    End If
                    Directory.Delete(Application.StartupPath & "\appxscan", True)
                ElseIf File.Exists(SourcePackage & "\AppxManifest.xml") Then
                    DynaLog.LogMessage("A standard manifest has been detected. Treating as a standard package...")
                    ' APPX/MSIX
                    If Not Directory.Exists(Application.StartupPath & "\temp\storeassets") Then Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets").Attributes = FileAttributes.Hidden
                    If My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & AppxPackageName).Count <= 0 Then
                        For Each AssetFile In My.Computer.FileSystem.GetFiles(SourcePackage & "\Assets", FileIO.SearchOption.SearchTopLevelOnly)
                            If Path.GetFileNameWithoutExtension(AssetFile).StartsWith("small", StringComparison.OrdinalIgnoreCase) Then
                                DynaLog.LogMessage("Copying item " & Quote & Path.GetFileName(AssetFile) & Quote & " to Store logo asset cache...")
                                File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                            ElseIf Path.GetFileNameWithoutExtension(AssetFile).StartsWith("store", StringComparison.OrdinalIgnoreCase) Then
                                DynaLog.LogMessage("Copying item " & Quote & Path.GetFileName(AssetFile) & Quote & " to Store logo asset cache...")
                                File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                            ElseIf Path.GetFileNameWithoutExtension(AssetFile).StartsWith("large", StringComparison.OrdinalIgnoreCase) Then
                                DynaLog.LogMessage("Copying item " & Quote & Path.GetFileName(AssetFile) & Quote & " to Store logo asset cache...")
                                File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                            End If
                        Next
                    End If
                Else
                    DynaLog.LogMessage("Either no manifest or an unknown manifest has been detected. This is unknown.")
                    MsgBox(LocalizationService.ForSection("AppxProvision.StoreLogo")("ReadFailed.Message"), vbOKOnly + vbCritical, LocalizationService.ForSection("AppxProvision.StoreLogo")("Add.Title"))
                End If
            Else
                DynaLog.LogMessage("Specified package is not a folder. Beginning to scan package...")
                If IsBundlePackage Then
                    DynaLog.LogMessage("This is a bundle package.")
                    DynaLog.LogMessage("Extracting main AppX package from bundle to grab assets...")
                    AppxScanner.StartInfo.Arguments = "e " & Quote & SourcePackage & Quote & " " & Quote & PackageName & Quote & " -o" & Quote & Application.StartupPath & "\appxscan" & Quote
                    AppxScanner.Start()
                    AppxScanner.WaitForExit()
                    If Not Directory.Exists(Application.StartupPath & "\temp\storeassets") Then Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets").Attributes = FileAttributes.Hidden
                    DynaLog.LogMessage("7-Zip process finished with exit code " & Hex(AppxScanner.ExitCode))
                    If AppxScanner.ExitCode = 0 Then
                        Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets\" & AppxPackageName)
                        If My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & AppxPackageName).Count <= 0 Then
                            ' Try extracting small, store and large assets
                            DynaLog.LogMessage("Extracting small logo assets...")
                            AppxScanner.StartInfo.Arguments = "e " & Quote & Application.StartupPath & "\appxscan\" & PackageName & Quote & " " & Quote & "Assets\small*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                            AppxScanner.Start()
                            AppxScanner.WaitForExit()
                            DynaLog.LogMessage("Extracting store-sized logo assets...")
                            AppxScanner.StartInfo.Arguments = "e " & Quote & Application.StartupPath & "\appxscan\" & PackageName & Quote & " " & Quote & "Assets\store*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                            AppxScanner.Start()
                            AppxScanner.WaitForExit()
                            DynaLog.LogMessage("Extracting large logo assets...")
                            AppxScanner.StartInfo.Arguments = "e " & Quote & Application.StartupPath & "\appxscan\" & PackageName & Quote & " " & Quote & "Assets\large*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                            AppxScanner.Start()
                            AppxScanner.WaitForExit()
                        End If
                    End If
                Else
                    DynaLog.LogMessage("This is a standard package.")
                    If Not Directory.Exists(Application.StartupPath & "\temp\storeassets") Then Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets").Attributes = FileAttributes.Hidden
                    Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets\" & AppxPackageName)
                    If My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & AppxPackageName).Count <= 0 Then
                        DynaLog.LogMessage("Detecting if the standard package is encrypted...")
                        If Path.GetExtension(SourcePackage).Replace(".", "").Trim().StartsWith("e", StringComparison.OrdinalIgnoreCase) Then
                            DynaLog.LogMessage("Specified package is encrypted. Copying assets obtained with UnpEax to logo asset cache...")
                            For Each asset In Directory.GetFiles(Application.StartupPath & "\appxscan\Assets")
                                File.Copy(asset, Application.StartupPath & "\temp\storeassets\" & AppxPackageName & "\" & Path.GetFileName(asset))
                            Next
                        Else
                            ' Try extracting small, store and large assets
                            DynaLog.LogMessage("Extracting small logo assets...")
                            AppxScanner.StartInfo.Arguments = "e " & Quote & SourcePackage & Quote & " " & Quote & "Assets\small*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                            AppxScanner.Start()
                            AppxScanner.WaitForExit()
                            DynaLog.LogMessage("Extracting store-sized logo assets...")
                            AppxScanner.StartInfo.Arguments = "e " & Quote & SourcePackage & Quote & " " & Quote & "Assets\store*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                            AppxScanner.Start()
                            AppxScanner.WaitForExit()
                            DynaLog.LogMessage("Extracting large logo assets...")
                            AppxScanner.StartInfo.Arguments = "e " & Quote & SourcePackage & Quote & " " & Quote & "Assets\large*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                            AppxScanner.Start()
                            AppxScanner.WaitForExit()
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not get Store logo assets. Error message: " & ex.Message)
            Debug.WriteLine("Could not get store logo assets. Reason: " & ex.ToString())
        End Try
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If ListView1.SelectedItems.Count > 0 Then
            If ListView1.SelectedItems.Count > 1 Then
                For x = ListView1.Items.Count - 1 To 0 Step -1
                    If ListView1.Items(x).Selected Then
                        Packages.RemoveAt(x)
                        ListView1.Items(x).Remove()
                    End If
                Next
            Else
                Packages.RemoveAt(ListView1.FocusedItem.Index)
                ListView1.Items.Remove(ListView1.FocusedItem)
            End If


            NoAppxFilePanel.Visible = (ListView1.SelectedItems.Count <= 0)
            AppxFilePanel.Visible = Not (ListView1.SelectedItems.Count <= 0)
            AppxDetailsPanel.Height = WindowHelper.ScaleLogical(If(ListView1.SelectedItems.Count <= 0, 520, 83))
            FlowLayoutPanel1.Visible = Not (ListView1.SelectedItems.Count <= 0)
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://en.wikipedia.org/wiki/ISO_3166-1#Current_codes")
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked Then
            TextBox1.Enabled = True
            Button7.Enabled = True
        Else
            TextBox1.Enabled = False
            Button7.Enabled = False
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            TextBox2.Enabled = True
            Button8.Enabled = True
        Else
            TextBox2.Enabled = False
            Button8.Enabled = False
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked Then
            TextBox3.Enabled = False
            Label5.Enabled = False
            LinkLabel1.Enabled = False
        Else
            TextBox3.Enabled = True
            Label5.Enabled = True
            LinkLabel1.Enabled = True
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            Button9.Enabled = (ListView1.SelectedItems.Count > 0)
            If ListView1.SelectedItems.Count > 1 Then
                DetectMultiSelectionCommonProperties()
            Else
                Label9.Visible = True
                PictureBox2.Visible = True
                TableLayoutPanel3.Enabled = True
                ListBox1.Enabled = True
                CheckBox3.Enabled = True
                TextBox1.Enabled = True
                CheckBox1.Enabled = True
                TextBox2.Enabled = True
                CheckBox4.Enabled = True
                TextBox3.Enabled = True
            End If
        Catch ex As NullReferenceException
            Button9.Enabled = True
        End Try
        Try
            DynaLog.LogMessage("Getting properties of selected item. Index: " & ListView1.FocusedItem.Index)
        Catch ex As Exception
            ' Do Not Log
        End Try
        NoAppxFilePanel.Visible = If(ListView1.SelectedItems.Count <= 0, True, False)
        AppxFilePanel.Visible = If(ListView1.SelectedItems.Count <= 0, False, True)
        AppxDetailsPanel.Height = WindowHelper.ScaleLogical(If(ListView1.SelectedItems.Count <= 0, 520, 83))
        FlowLayoutPanel1.Visible = If(ListView1.SelectedItems.Count <= 0, False, True)
        If ListView1.SelectedItems.Count = 1 Then
            Try
                Label7.Text = ListView1.FocusedItem.SubItems(2).Text
                Label8.Text = LocalizationService.ForSection("AppxProvision").Format("Publisher.Label", ListView1.FocusedItem.SubItems(3).Text)
                Label9.Text = LocalizationService.ForSection("AppxProvision").Format("Version.Label", ListView1.FocusedItem.SubItems(4).Text)
            Catch ex As NullReferenceException

            End Try
        End If
        Try
            DynaLog.LogMessage("Getting appropriate Store logo asset...")
            If Directory.Exists(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text) And My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count > 0 Then
                DynaLog.LogMessage("There are items in the Store logo asset cache folder. Grabbing item...")
                PictureBox2.SizeMode = PictureBoxSizeMode.Zoom
                Dim asset As Image = Nothing
                For Each StoreAsset In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text)
                    If Path.GetExtension(StoreAsset).EndsWith("png") Then
                        asset = Image.FromFile(StoreAsset)
                        If asset.Width / asset.Height = 1 Then      ' Determine if the image's aspect ratio is 1:1
                            If asset.Width <= 100 And asset.Height <= 100 Then      ' Determine if it is a "small" or "store" asset
                                DynaLog.LogMessage("The dimension has been obtained and it fits the criteria (1:1 aspect ratio and physical dimensions over 100px.)")
                                PictureBox2.Image = asset
                            End If
                        End If
                    End If
                Next
            Else
                DynaLog.LogMessage("There are no items in the Store logo asset cache folder. Grabbing item...")
                PictureBox2.SizeMode = PictureBoxSizeMode.CenterImage
                PictureBox2.Image = GetGlyphResource("preview_unavail")
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not get logo asset. Error message: " & ex.Message)
            PictureBox2.SizeMode = PictureBoxSizeMode.CenterImage
            PictureBox2.Image = GetGlyphResource("preview_unavail")
        End Try

        If ListView1.FocusedItem IsNot Nothing Then
            FlowLayoutPanel1.Enabled = Not Path.GetExtension(ListView1.FocusedItem.SubItems(0).Text).Replace(".", "").Trim().StartsWith("e", StringComparison.OrdinalIgnoreCase)
        End If

        ' Detect properties obtained by the AppxPackage Element
        Try
            If ListView1.SelectedItems.Count = 1 Then
                DynaLog.LogMessage("There is 1 item selected. Grabbing properties...")
                ListBox1.Items.Clear()
                If Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Count > 0 Then
                    For Each Dependency As AppxDependency In Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
                        ListBox1.Items.Add(Dependency.DependencyFile)
                    Next
                End If
                TextBox1.Text = Packages(ListView1.FocusedItem.Index).PackageLicenseFile
                If Packages(ListView1.FocusedItem.Index).PackageLicenseFile <> "" And File.Exists(Packages(ListView1.FocusedItem.Index).PackageLicenseFile) Then
                    CheckBox3.Checked = True
                Else
                    CheckBox3.Checked = False
                End If
                TextBox2.Text = Packages(ListView1.FocusedItem.Index).PackageCustomDataFile
                If Packages(ListView1.FocusedItem.Index).PackageCustomDataFile <> "" And File.Exists(Packages(ListView1.FocusedItem.Index).PackageCustomDataFile) Then
                    CheckBox1.Checked = True
                Else
                    CheckBox1.Checked = False
                End If
                TextBox3.Text = Packages(ListView1.FocusedItem.Index).PackageRegions
                If TextBox3.Text = "" Then
                    CheckBox4.Checked = True
                Else
                    CheckBox4.Checked = False
                End If
                DynaLog.LogMessage("Detecting conditions imposed by DISM and the Windows image for stub package preferences...")
                If (FileVersionInfo.GetVersionInfo(MainForm.DismExe).ProductMajorPart >= 10 And MainForm.CurrentImage.ImageVersion.Major >= 10) And
                    Packages(ListView1.FocusedItem.Index).SupportsStub Then
                    DynaLog.LogMessage("All requirements are met.")
                    Panel2.Enabled = True
                    Select Case Packages(ListView1.FocusedItem.Index).StubPackageOption
                        Case StubPreference.NoPreference
                            ComboBox1.SelectedIndex = 0
                        Case StubPreference.StubOnly
                            ComboBox1.SelectedIndex = 1
                        Case StubPreference.FullPackage
                            ComboBox1.SelectedIndex = 2
                    End Select
                Else
                    DynaLog.LogMessage("Either none or not all requirements are met.")
                    Panel2.Enabled = False
                    ComboBox1.SelectedIndex = 0
                End If
            End If
        Catch ex As Exception
            NoAppxFilePanel.Visible = True
            AppxFilePanel.Visible = False
            AppxDetailsPanel.Height = WindowHelper.ScaleLogical(520)
            FlowLayoutPanel1.Visible = False
        End Try
    End Sub

    Sub DetectMultiSelectionCommonProperties()
        NoAppxFilePanel.Visible = If(ListView1.SelectedItems.Count <= 0, True, False)
        AppxFilePanel.Visible = If(ListView1.SelectedItems.Count <= 0, False, True)
        AppxDetailsPanel.Height = WindowHelper.ScaleLogical(If(ListView1.SelectedItems.Count <= 0, 520, 83))
        FlowLayoutPanel1.Visible = If(ListView1.SelectedItems.Count <= 0, False, True)
        Label7.Text = LocalizationService.ForSection("AppxProvision.MultiSelect")("Selection.Label")
        Label8.Text = LocalizationService.ForSection("AppxProvision.MultiSelect")("CommonProps.Label")
        DynaLog.LogMessage("Detecting common properties with the Elements...")
        Label9.Visible = False
        PictureBox2.Visible = False
        ListBox1.Items.Clear()
        TextBox1.Text = ""
        ' Detect common properties. Use the Elements for that
        Dim depLists As New List(Of AppxPackage)
        Dim commonDeps As New List(Of String)
        Dim liFileLists As New List(Of AppxPackage)
        Dim commonLicense As String = ""
        Dim cdFileLists As New List(Of AppxPackage)
        Dim commonCustomDataFile As String = ""
        Dim regionLists As New List(Of AppxPackage)
        Dim commonRegions As String = ""
        For Each SelectedItem As ListViewItem In ListView1.SelectedItems
            ' Detect dependencies
            depLists.Add(Packages(ListView1.Items.IndexOf(SelectedItem)))
            ' Detect license files
            liFileLists.Add(Packages(ListView1.Items.IndexOf(SelectedItem)))
            ' Detect custom data files
            cdFileLists.Add(Packages(ListView1.Items.IndexOf(SelectedItem)))
            ' Detect regions
            regionLists.Add(Packages(ListView1.Items.IndexOf(SelectedItem)))
        Next
        If depLists.Count > 0 Then
            For Each dependency As AppxPackage In depLists
                Dim depList As New List(Of AppxDependency)
                depList = dependency.PackageSpecifiedDependencies
                Dim depFileList As New List(Of String)
                For Each dep As AppxDependency In depList
                    depFileList.Add(dep.DependencyFile)
                Next
                If commonDeps.Count = 0 Then commonDeps = depFileList
                commonDeps = commonDeps.Intersect(depFileList).ToList()
            Next
        End If
        If commonDeps.Count > 0 Then
            For Each commonDep In commonDeps
                ListBox1.Items.Add(commonDep)
            Next
        End If
        If liFileLists.Count > 0 Then
            Dim liFileList As New List(Of String)
            For Each LicenseFileInPkg As AppxPackage In liFileLists
                liFileList.Add(LicenseFileInPkg.PackageLicenseFile)
            Next
            If liFileList.Count > 0 Then
                commonLicense = liFileList(0)
                Dim singleLiFile As String = liFileList(0)
                ' If a license file is repeated every time, it's our common one
                For Each licenseFile In liFileList
                    If Not licenseFile = singleLiFile Then
                        singleLiFile = licenseFile
                        commonLicense = ""
                    End If
                Next
            End If
        End If
        If commonLicense <> "" And File.Exists(commonLicense) Then
            CheckBox3.Checked = True
            TextBox1.Text = commonLicense
        End If
        If cdFileLists.Count > 0 Then
            Dim cdFileList As New List(Of String)
            For Each CustomDataFile As AppxPackage In cdFileLists
                cdFileList.Add(CustomDataFile.PackageCustomDataFile)
            Next
            If cdFileList.Count > 0 Then
                commonCustomDataFile = cdFileList(0)
                Dim singleCdFile As String = cdFileList(0)
                ' If a custom data file is repeated every time, it's our common one
                For Each customDataFile In cdFileList
                    If Not customDataFile = singleCdFile Then
                        singleCdFile = customDataFile
                        commonCustomDataFile = ""
                    End If
                Next
            End If
        End If
        If commonCustomDataFile <> "" And File.Exists(commonCustomDataFile) Then
            CheckBox1.Checked = True
            TextBox2.Text = commonCustomDataFile
        End If
        If regionLists.Count > 0 Then
            Dim regionList As New List(Of String)
            For Each regionString As AppxPackage In regionLists
                regionList.Add(regionString.PackageRegions)
            Next
            If regionList.Count > 0 Then
                commonRegions = regionList(0)
                Dim singleRegion As String = regionList(0)
                ' If a custom data file is repeated every time, it's our common one
                For Each regionStr In regionList
                    If Not regionStr = singleRegion Then
                        singleRegion = regionStr
                        commonRegions = ""
                    End If
                Next
            End If
        End If
        If commonRegions <> "" Then
            CheckBox4.Checked = True
            TextBox3.Text = commonRegions
        End If

        ' Disable manipulation controls, as editing is not implemented yet
        TableLayoutPanel3.Enabled = False
        ListBox1.Enabled = False
        CheckBox3.Enabled = False
        TextBox1.Enabled = False
        CheckBox1.Enabled = False
        TextBox2.Enabled = False
        CheckBox4.Enabled = False
        TextBox3.Enabled = False
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.SelectedItem = "" Then
            Button5.Enabled = False
        Else
            Button5.Enabled = True
        End If
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        If My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0 Then Exit Sub
        HidePopupForm()
        DynaLog.LogMessage("Showing image...")
        With LogoAssetPopupForm
            .BackColor = BackColor
            .ForeColor = ForeColor
            .ShowIcon = False
            .ShowInTaskbar = False
            .ControlBox = False
            .FormBorderStyle = Windows.Forms.FormBorderStyle.None
            .Size = New Size(152, 152)
            Dim ctrlLoc As Point = PictureBox2.PointToScreen(Point.Empty)
            .StartPosition = FormStartPosition.Manual
            .Location = ctrlLoc
            .Text = LocalizationService.ForSection("AppxProvision")("Preview.Label")
            With LogoAssetPreview
                .Parent = LogoAssetPopupForm
                .Dock = DockStyle.Fill
                .SizeMode = PictureBoxSizeMode.Zoom
                Try
                    DynaLog.LogMessage("Getting appropriate Store logo asset...")
                    If Directory.Exists(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text) And My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count > 0 Then
                        DynaLog.LogMessage("There are items in the Store logo asset cache folder. Grabbing item...")
                        Dim asset As Image = Nothing
                        For Each StoreAsset In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text)
                            If Path.GetExtension(StoreAsset).EndsWith("png") Then
                                asset = Image.FromFile(StoreAsset)
                                If asset.Width / asset.Height = 1 Then      ' Determine if the image's aspect ratio is 1:1
                                    If asset.Width > 100 And asset.Width <= 200 And asset.Height > 100 And asset.Height <= 200 Then      ' Determine if it is a "large" asset
                                        DynaLog.LogMessage("The dimension has been obtained and it fits the criteria (1:1 aspect ratio and physical dimensions over 200px.)")
                                        .Image = asset
                                        Exit For
                                    Else
                                        DynaLog.LogMessage("The dimension has been obtained but does not fit some or all of the criteria specified (1:1 aspect ratio and physical dimensions over 200px.)")
                                        DynaLog.LogMessage("Adjusting preview...")
                                        .SizeMode = PictureBoxSizeMode.CenterImage
                                        .Image = PictureBox2.Image
                                    End If
                                End If
                            End If
                        Next
                    End If
                Catch ex As Exception

                End Try
            End With
            .Controls.Add(LogoAssetPreview)
            AddHandler LogoAssetPreview.Click, AddressOf HidePopupForm
            .Show()
            .BringToFront()
        End With
    End Sub

    Sub HidePopupForm() Handles MyBase.FormClosing, MyBase.VisibleChanged
        LogoAssetPopupForm.Hide()
    End Sub

    Private Sub PictureBox2_MouseHover(sender As Object, e As EventArgs) Handles PictureBox2.MouseHover
        Try
            WindowHelper.DisplayToolTip(sender, If(My.Computer.FileSystem.GetFiles(Application.StartupPath & LocalizationService.ForSection("AppxProvision.Tooltip")("TempStoreassets.Label") & ListView1.FocusedItem.SubItems(2).Text).Count <= 0, LocalizationService.ForSection("AppxProvision.Tooltip")("Logo.Assets.File.Label"), LocalizationService.ForSection("AppxProvision.Tooltip")("Enlarge.View.Label")))
        Catch ex As Exception
            WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("AppxProvision.Tooltip")("Logo.Assets.File.Item"))
        End Try
    End Sub

    Private Sub ListView1_DragEnter(sender As Object, e As DragEventArgs) Handles ListView1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Function GetDownloadedPackageExtensionFromAppInstaller(PkgFile As String) As String
        DynaLog.LogMessage("Getting downloaded package from the App Installer package...")
        DynaLog.LogMessage("Package file: " & Quote & Path.GetFileName(PkgFile) & Quote)
        Dim appExtensions() As String = New String(7) {".appx", ".appxbundle", ".msix", ".msixbundle", ".eappx", ".eappxbundle", ".emsix", ".emsixbundle"}
        Try
            For Each appExtension In appExtensions
                Dim targetFile As String = PkgFile.Replace(Path.GetExtension(PkgFile), appExtension)
                DynaLog.LogMessage("Checking if file " & Quote & Path.GetFileName(targetFile) & Quote & " exists...")
                If File.Exists(targetFile) Then Return appExtension
            Next
        Catch ex As Exception
            DynaLog.LogMessage("Could not get suitable item. Error message: " & ex.Message)
            Return Nothing
        End Try
        Return Nothing
    End Function

    Private Sub ListView1_DragDrop(sender As Object, e As DragEventArgs) Handles ListView1.DragDrop
        Dim PackageFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        Dim HasBeenScannedByAppInstaller As Boolean = False
        Cursor = Cursors.WaitCursor
        DynaLog.LogMessage("Interpreting items to add to queue...")
        For Each PackageFile In PackageFiles
            ' Force the indication of waiting
            Cursor = Cursors.WaitCursor
            If Path.GetExtension(PackageFile).Equals(".appx", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(PackageFile).Equals(".msix", StringComparison.OrdinalIgnoreCase) Or
                Path.GetExtension(PackageFile).Equals(".appxbundle", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(PackageFile).Equals(".msixbundle", StringComparison.OrdinalIgnoreCase) Or
                Path.GetExtension(PackageFile).Equals(".eappx", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(PackageFile).Equals(".emsix", StringComparison.OrdinalIgnoreCase) Or
                Path.GetExtension(PackageFile).Equals(".eappxbundle", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(PackageFile).Equals(".emsixbundle", StringComparison.OrdinalIgnoreCase) Then
                DynaLog.LogMessage("The item to add " & Quote & Path.GetFileName(PackageFile) & Quote & " is a regular AppX package.")
                If Not HasBeenScannedByAppInstaller Then
                    ScanAppxPackage(False, PackageFile)
                Else
                    ' The item has been detected by the app installer package, but the resulting package is already present,
                    ' so instead of scanning it again, we get rid of an error by ignoring the second scan. Instead of re-scanning
                    ' the package, we set this flag to false so that we can get more stuff.
                    HasBeenScannedByAppInstaller = False
                End If
            ElseIf Path.GetExtension(PackageFile).Equals(".appinstaller", StringComparison.OrdinalIgnoreCase) Then
                DynaLog.LogMessage("The item to add " & Quote & Path.GetFileName(PackageFile) & Quote & " is an App Installer package.")
                If Not AppInstallerDownloader.IsDisposed Then AppInstallerDownloader.Dispose()
                AppInstallerDownloader.AppInstallerFile = PackageFile
                If Not File.Exists(PackageFile.Replace(".appinstaller", GetDownloadedPackageExtensionFromAppInstaller(PackageFile))) Then
                    AppInstallerDownloader.ShowDialog(Me)
                    Dim obtainedExtension As String = GetDownloadedPackageExtensionFromAppInstaller(PackageFile)
                    If obtainedExtension IsNot Nothing Then ScanAppxPackage(False, PackageFile.Replace(".appinstaller", obtainedExtension).Trim())
                Else
                    Dim obtainedExtension As String = GetDownloadedPackageExtensionFromAppInstaller(PackageFile)
                    If obtainedExtension IsNot Nothing Then ScanAppxPackage(False, PackageFile.Replace(".appinstaller", obtainedExtension).Trim())
                    HasBeenScannedByAppInstaller = True
                End If
            ElseIf (File.GetAttributes(PackageFile) And FileAttributes.Directory) = FileAttributes.Directory Then
                DynaLog.LogMessage("The item to add is a directory. Getting contents...")
                Dim msg As String = ""
                ' Temporary support for directories
                If File.Exists(PackageFile & "\AppxSignature.p7x") And File.Exists(PackageFile & "\AppxMetadata\AppxBundleManifest.xml") Or File.Exists(PackageFile & "\AppxManifest.xml") Then
                    DynaLog.LogMessage("There are contents of an AppX package. We are dealing with an unpacked AppX package.")
                    DynaLog.LogMessage("Scanning AppX package...")
                    ScanAppxPackage(True, PackageFile)
                ElseIf My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchTopLevelOnly, "*.appx").Count > 0 Or My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchTopLevelOnly, "*.msix").Count > 0 Or
                    My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchTopLevelOnly, "*.appxbundle").Count > 0 Or My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchTopLevelOnly, "*.msixbundle").Count > 0 Then
                    DynaLog.LogMessage("There are AppX packages. Asking user whether or not to scan folder recursively...")
                    msg = LocalizationService.ForSection("AppxProvision.DragDrop").Format("Dir.Contains.App.Message", PackageFile)
                    If MsgBox(msg, vbYesNo + vbQuestion, ImageTaskHeader1.ItemText) = MsgBoxResult.Yes Then
                        DynaLog.LogMessage("The user has accepted the question.")
                        For Each AppPkg In My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchAllSubDirectories)
                            If Path.GetExtension(AppPkg).Equals(".appx", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(AppPkg).Equals(".appxbundle", StringComparison.OrdinalIgnoreCase) Or
                                Path.GetExtension(AppPkg).Equals(".msix", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(AppPkg).Equals(".msixbundle", StringComparison.OrdinalIgnoreCase) Then
                                DynaLog.LogMessage("Item " & Quote & Path.GetFileName(AppPkg) & Quote & " is an AppX package.")
                                ScanAppxPackage(False, AppPkg)
                            ElseIf Path.GetExtension(AppPkg).Equals(".appinstaller", StringComparison.OrdinalIgnoreCase) Then
                                DynaLog.LogMessage("Item " & Quote & Path.GetFileName(AppPkg) & Quote & " is an App Installer package.")
                                If Not AppInstallerDownloader.IsDisposed Then AppInstallerDownloader.Dispose()
                                AppInstallerDownloader.AppInstallerFile = AppPkg
                                If Not File.Exists(AppPkg.Replace(".appinstaller", GetDownloadedPackageExtensionFromAppInstaller(AppPkg))) Then AppInstallerDownloader.ShowDialog(Me)
                                If File.Exists(AppPkg.Replace(".appinstaller", GetDownloadedPackageExtensionFromAppInstaller(AppPkg))) Then ScanAppxPackage(False, AppPkg.Replace(".appinstaller", GetDownloadedPackageExtensionFromAppInstaller(AppPkg)))
                            Else
                                DynaLog.LogMessage("Item " & Quote & Path.GetFileName(AppPkg) & Quote & " is an unrecognized file.")
                                Continue For
                            End If
                        Next
                    Else
                        Continue For
                    End If
                End If
            Else
                DynaLog.LogMessage("Item " & Quote & Path.GetFileName(PackageFile) & Quote & " is an unrecognized file.")
                MsgBox(LocalizationService.ForSection("AppxProvision.DragDrop")("File.Dropped.Isn.Message"), vbOKOnly + vbCritical, LocalizationService.ForSection("AppxProvision.DragDrop")("Add.Prov.Title"))
            End If
        Next
        Cursor = Cursors.Arrow
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        LicenseFileOFD.ShowDialog(Me)
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        CustomDataFileOFD.ShowDialog(Me)
    End Sub

    Private Sub ListBox1_DragEnter(sender As Object, e As DragEventArgs) Handles ListBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ListBox1_DragDrop(sender As Object, e As DragEventArgs) Handles ListBox1.DragDrop
        If ListView1.SelectedItems.Count < 1 Then Exit Sub
        Dim DependencyFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each Dependency In DependencyFiles
            If Not ListBox1.Items.Contains(Dependency) And (Path.GetExtension(Dependency).EndsWith("appx", StringComparison.OrdinalIgnoreCase) Or _
                                                            Path.GetExtension(Dependency).EndsWith("msix", StringComparison.OrdinalIgnoreCase) Or _
                                                            Path.GetExtension(Dependency).EndsWith("appxbundle", StringComparison.OrdinalIgnoreCase) Or _
                                                            Path.GetExtension(Dependency).EndsWith("msixbundle", StringComparison.OrdinalIgnoreCase)) Then
                DynaLog.LogMessage("Item " & Quote & Path.GetFileName(Dependency) & Quote & " is not already present in the list and has a supported format.")
                DynaLog.LogMessage("Adding dependency to dependency list and selected element in addition queue...")
                Dim dep As New AppxDependency()
                dep.DependencyFile = Dependency
                If Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Count > 0 And Not Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Contains(dep) Then
                    Dim deps As New List(Of AppxDependency)
                    deps = Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
                    deps.Add(dep)
                    Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies = deps
                ElseIf Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Count = 0 Then
                    Dim deps As New List(Of AppxDependency)
                    deps = Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
                    deps.Add(dep)
                    Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies = deps
                End If
                ListBox1.Items.Add(Dependency)
            End If
        Next
        If ListBox1.Items.Count > 0 Then Button4.Enabled = True
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If ListView1.SelectedItems.Count = 1 Then Packages(ListView1.FocusedItem.Index).PackageLicenseFile = TextBox1.Text
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If ListView1.SelectedItems.Count = 1 Then Packages(ListView1.FocusedItem.Index).PackageCustomDataFile = TextBox2.Text
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        MainForm.AppxRelatedLinksCMS.Show(sender, New Point(8, 8))
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        DynaLog.LogMessage("Setting stub package preferences accordingly...")
        If ComboBox1.SelectedIndex = 0 Then
            If ListView1.SelectedItems.Count = 1 Then Packages(ListView1.FocusedItem.Index).StubPackageOption = StubPreference.NoPreference
        ElseIf ComboBox1.SelectedIndex = 1 Then
            If ListView1.SelectedItems.Count = 1 Then Packages(ListView1.FocusedItem.Index).StubPackageOption = StubPreference.StubOnly
        ElseIf ComboBox1.SelectedIndex = 2 Then
            If ListView1.SelectedItems.Count = 1 Then Packages(ListView1.FocusedItem.Index).StubPackageOption = StubPreference.FullPackage
        End If
    End Sub
End Class
