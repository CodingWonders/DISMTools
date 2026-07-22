Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.Dism
Imports DISMTools.Utilities
Imports Microsoft.VisualBasic.ControlChars

Public Class GetAppxPkgInfoDlg

    Dim mainAsset As String = ""
    Dim assetDir As String = ""

    Public displayName As String = ""

    Private FilteredAppxPackages As IEnumerable(Of DismAppxPackage)
    Private FilteredAppxPackages_Backup As IEnumerable(Of ImageAppxPackage)

    Private Sub GetAppxPkgInfoDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("Get.AppX")("AppX.Package.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("Get.AppX").Format("Image.Task.Header.Label", Text)
        Label36.Text = LocalizationService.ForSection("Get.AppX")("AppX.Package.Label.Label")
        Label37.Text = LocalizationService.ForSection("Get.AppX")("Installed.AppX.Label")
        Label22.Text = LocalizationService.ForSection("Get.AppX")("PackageName.Label")
        Label24.Text = LocalizationService.ForSection("Get.AppX")("Display.Name.Label")
        Label26.Text = LocalizationService.ForSection("Get.AppX")("Architecture.Label")
        Label31.Text = LocalizationService.ForSection("Get.AppX")("ResourceID.Label")
        Label41.Text = LocalizationService.ForSection("Get.AppX")("Version.Label")
        Label43.Text = LocalizationService.ForSection("Get.AppX")("Registered.User.Label")
        Label4.Text = LocalizationService.ForSection("Get.AppX")("Install.Dir.Label")
        Label6.Text = LocalizationService.ForSection("Get.AppX")("Package.Manifest.Label")
        Label8.Text = LocalizationService.ForSection("Get.AppX")("StoreLogo.Asset.Dir.Label")
        Label9.Text = LocalizationService.ForSection("Get.AppX")("Main.StoreLogo.Asset.Label")
        Label10.Text = LocalizationService.ForSection("Get.AppX")("Asset.Guessed.DISM.Message")
        LinkLabel1.Text = LocalizationService.ForSection("Get.AppX")("Asset.One.IM.Link")
        Button2.Text = LocalizationService.ForSection("Get.AppX")("Save.Button")
        SearchBox1.cueBanner = LocalizationService.ForSection("Get.AppX")("Type.Search.Label")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListBox1.ForeColor = ForeColor
        SearchBox1.BackColor = BackColor
        SearchBox1.ForeColor = ForeColor
        SearchPic.Image = GetGlyphResource("search")
        If SplitContainer2.SplitterDistance = 440 Then
            SplitContainer2.SplitterDistance = WindowHelper.ScaleLogical(SplitContainer2.SplitterDistance)
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ' Populate feature information list
        Panel4.Visible = False
        Panel7.Visible = True
        ' Populate package listing
        DynaLog.LogMessage("Updating items in list box...")
        ListBox1.Items.Clear()
        ' The PowerShell helper may have added stuff to the MainForm arrays. Check that
        DynaLog.LogMessage("Detecting conditions imposed by host system...")
        If MainForm.CurrentImage.ImageAppxPackages IsNot Nothing Then
            DynaLog.LogMessage("Host system is running Windows 10 or 11. Using the technology provided by the DISM API...")
            DynaLog.LogMessage("Detecting if the extended AppX package getter script has been run...")
            If MainForm.CurrentImage.ImageAppxPackages_Backup.Count > MainForm.CurrentImage.ImageAppxPackages.Count Then
                DynaLog.LogMessage("Array has more items than AppX package collection. The script has been run.")
                DynaLog.LogMessage("Getting AppX packages from arrays...")
                ListBox1.Items.AddRange(MainForm.CurrentImage.ImageAppxPackages_Backup.Select(Function(appxPackage) appxPackage.PackageFullName).ToArray())
            Else
                DynaLog.LogMessage("Array has the same items as the AppX package collection. The script has not been run.")
                DynaLog.LogMessage("Getting AppX packages...")
                ListBox1.Items.AddRange(MainForm.CurrentImage.ImageAppxPackages.Select(Function(appxPackage) appxPackage.PackageName).ToArray())
            End If
        Else
            DynaLog.LogMessage("Host system is running Windows 8. Getting AppX packages from arrays...")
            ' This condition is met on Windows 8 hosts, as they can't get AppX package information with the DISM API.
            ListBox1.Items.AddRange(MainForm.CurrentImage.ImageAppxPackages_Backup.Select(Function(appxPackage) appxPackage.PackageFullName).ToArray())
        End If
        SearchBox1.Text = ""

        AppxHelper.ClearRootPaths()
        AppxHelper.SetRootPaths(MainForm.MountDir)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Label10.Visible = True
        LinkLabel1.Visible = True
        mainAsset = ""
        assetDir = ""
        ' Clear the values of Label7, Label5, and Label3; as the program can't update their text properties on some packages
        Label7.Text = ""
        Label5.Text = ""
        Label3.Text = ""

        Dim trueIndex As Integer = 0

        DynaLog.LogMessage("Selected items: " & ListBox1.SelectedItems.Count)

        If ListBox1.SelectedItems.Count = 1 Then
            DynaLog.LogMessage("An item is selected.")
            DynaLog.LogMessage("Detecting conditions imposed by host system...")

            If SearchBox1.Text <> "" Then
                If FilteredAppxPackages Is Nothing OrElse FilteredAppxPackages.Count = 0 OrElse FilteredAppxPackages.Count < FilteredAppxPackages_Backup.Count Then
                    DynaLog.LogMessage("Host system is running Windows 8 or extended processes had been run.")
                    Label23.Text = FilteredAppxPackages_Backup(ListBox1.SelectedIndex).PackageFullName
                    Label25.Text = FilteredAppxPackages_Backup(ListBox1.SelectedIndex).PackageName
                    Label35.Text = Casters.CastDismArchitecture(FilteredAppxPackages_Backup(ListBox1.SelectedIndex).PackageArchitecture, True)
                    Label32.Text = FilteredAppxPackages_Backup(ListBox1.SelectedIndex).PackageResourceId
                    Label40.Text = FilteredAppxPackages_Backup(ListBox1.SelectedIndex).PackageVersion.ToString()
                Else
                    DynaLog.LogMessage("Host system is running Windows 10 or 11. Using the technology provided by the DISM API...")
                    Label23.Text = FilteredAppxPackages(ListBox1.SelectedIndex).PackageName
                    Label25.Text = FilteredAppxPackages(ListBox1.SelectedIndex).DisplayName
                    Label35.Text = Casters.CastDismArchitecture(FilteredAppxPackages(ListBox1.SelectedIndex).Architecture, True)
                    Label32.Text = FilteredAppxPackages(ListBox1.SelectedIndex).ResourceId
                    Label40.Text = FilteredAppxPackages(ListBox1.SelectedIndex).Version.ToString()
                End If
            Else
                If MainForm.CurrentImage.ImageAppxPackages IsNot Nothing Then
                    DynaLog.LogMessage("Host system is running Windows 10 or 11. Using the technology provided by the DISM API...")
                    DynaLog.LogMessage("Detecting if the extended AppX package getter script has been run...")
                    If MainForm.CurrentImage.ImageAppxPackages_Backup.Count > MainForm.CurrentImage.ImageAppxPackages.Count Then
                        DynaLog.LogMessage("Array has more items than AppX package collection. The script has been run.")
                        DynaLog.LogMessage("Getting AppX packages from arrays...")
                        Label23.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageFullName
                        Label25.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageName
                        Label35.Text = Casters.CastDismArchitecture(MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageArchitecture, True)
                        Label32.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageResourceId
                        Label40.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageVersion.ToString()
                    Else
                        DynaLog.LogMessage("Array has the same items as the AppX package collection. The script has not been run.")
                        DynaLog.LogMessage("Getting AppX packages...")
                        DynaLog.LogMessage("Search function may have been used. Grabbing true index of selected AppX package...")
                        For Each InstalledAppx As DismAppxPackage In MainForm.CurrentImage.ImageAppxPackages
                            If InstalledAppx.PackageName.ToLower().Contains(SearchBox1.Text.ToLower()) And InstalledAppx.PackageName = ListBox1.Items(ListBox1.SelectedIndex) Then
                                trueIndex = MainForm.CurrentImage.ImageAppxPackages.IndexOf(InstalledAppx)
                            End If
                        Next
                        DynaLog.LogMessage("True index: " & trueIndex)
                        If SearchBox1.Text = "" Then
                            DynaLog.LogMessage("No search query has been typed.")
                            Label23.Text = MainForm.CurrentImage.ImageAppxPackages(ListBox1.SelectedIndex).PackageName
                            Label25.Text = MainForm.CurrentImage.ImageAppxPackages(ListBox1.SelectedIndex).DisplayName
                            Label35.Text = Casters.CastDismArchitecture(MainForm.CurrentImage.ImageAppxPackages(ListBox1.SelectedIndex).Architecture, True)
                            Label32.Text = MainForm.CurrentImage.ImageAppxPackages(ListBox1.SelectedIndex).ResourceId
                            Label40.Text = MainForm.CurrentImage.ImageAppxPackages(ListBox1.SelectedIndex).Version.ToString()
                        Else
                            DynaLog.LogMessage("A search query has been typed. Using true index...")
                            Label23.Text = MainForm.CurrentImage.ImageAppxPackages(trueIndex).PackageName
                            Label25.Text = MainForm.CurrentImage.ImageAppxPackages(trueIndex).DisplayName
                            Label35.Text = Casters.CastDismArchitecture(MainForm.CurrentImage.ImageAppxPackages(trueIndex).Architecture, True)
                            Label32.Text = MainForm.CurrentImage.ImageAppxPackages(trueIndex).ResourceId
                            Label40.Text = MainForm.CurrentImage.ImageAppxPackages(trueIndex).Version.ToString()
                        End If
                    End If
                Else
                    DynaLog.LogMessage("Host system is running Windows 8. Getting AppX packages from arrays...")
                    Label23.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageFullName
                    Label25.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageName
                    Label35.Text = Casters.CastDismArchitecture(MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageArchitecture, True)
                    Label32.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageResourceId
                    Label40.Text = MainForm.CurrentImage.ImageAppxPackages_Backup(ListBox1.SelectedIndex).PackageVersion.ToString()
                End If
            End If

            displayName = Label25.Text

            DynaLog.LogMessage("Getting AppX display name (the one you would see if the app is installed)...")
            DynaLog.LogMessage("- Package name: " & Label23.Text)
            DynaLog.LogMessage("- Package display name: " & Label25.Text)
            Dim packageDispName As String = AppxHelper.GetPackageDisplayName(MainForm.MountDir, Label23.Text, Label25.Text)

            Dim appDisplayName As String = ""

            If packageDispName IsNot Nothing Then
                DynaLog.LogMessage("Package display name: " & packageDispName)
                DynaLog.LogMessage("Checking if display name relies on a PRI...")
                appDisplayName = If(Not packageDispName.StartsWith("ms-resource:"), packageDispName, "")
                If MainForm.CurrentImage.ImageAppxPackages IsNot Nothing And packageDispName.StartsWith("ms-resource:") Then
                    DynaLog.LogMessage("Display name starts with " & Quote & "ms-resource:" & Quote & ". Using PRI reader...")
                    If MainForm.CurrentImage.ImageAppxPackages_Backup.Count > MainForm.CurrentImage.ImageAppxPackages.Count Then
                        DynaLog.LogMessage("Array has more items than AppX package collection. The script has been run.")
                        Dim PriName As String = PriReader.ReadFromPri((If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & Label23.Text).Replace("\\", "\").Trim(), _
                                                                      Label25.Text, _
                                                                      packageDispName)
                        DynaLog.LogMessage("Name obtained from " & Quote & "resources.pri" & Quote & ": " & PriName)
                        If PriName <> "" And Not PriName = Label25.Text Then appDisplayName = PriName
                    Else
                        DynaLog.LogMessage("Array has the same items as the AppX package collection. The script has not been run.")
                        If SearchBox1.Text = "" Then
                            Dim PriName As String = PriReader.ReadFromPri(MainForm.CurrentImage.ImageAppxPackages(ListBox1.SelectedIndex).InstallLocation, _
                                                                          MainForm.CurrentImage.ImageAppxPackages(ListBox1.SelectedIndex).DisplayName, _
                                                                          packageDispName)
                            DynaLog.LogMessage("Name obtained from " & Quote & "resources.pri" & Quote & ": " & PriName)
                            If PriName <> "" And Not PriName = MainForm.CurrentImage.ImageAppxPackages(ListBox1.SelectedIndex).DisplayName Then appDisplayName = PriName
                        Else
                            Dim PriName As String = PriReader.ReadFromPri(MainForm.CurrentImage.ImageAppxPackages(trueIndex).InstallLocation, _
                                                                          MainForm.CurrentImage.ImageAppxPackages(trueIndex).DisplayName, _
                                                                          packageDispName)
                            DynaLog.LogMessage("Name obtained from " & Quote & "resources.pri" & Quote & ": " & PriName)
                            If PriName <> "" And Not PriName = MainForm.CurrentImage.ImageAppxPackages(trueIndex).DisplayName Then appDisplayName = PriName
                        End If
                    End If
                End If
            End If

            If appDisplayName <> "" Then Label25.Text &= " (" & appDisplayName & ")"

            DynaLog.LogMessage("Getting registration status of AppX package...")
            ' Get exclusive things that can't be obtained with the DISM API
            Dim IsPackageRegistered As Boolean
            If MainForm.CurrentImage.ImageAppxPackages Is Nothing OrElse MainForm.CurrentImage.ImageAppxPackages_Backup.Count > MainForm.CurrentImage.ImageAppxPackages.Count Then
                IsPackageRegistered = AppxHelper.IsPackageRegistered(MainForm.MountDir, MainForm.CurrentImage.ImageAppxPackages_Backup.ElementAtOrDefault(ListBox1.SelectedIndex))
            Else
                IsPackageRegistered = AppxHelper.IsPackageRegistered(MainForm.MountDir, MainForm.CurrentImage.ImageAppxPackages.ElementAtOrDefault(ListBox1.SelectedIndex))
            End If

            If IsPackageRegistered Then
                Label42.Text = LocalizationService.ForSection("Get.AppX.PackageList")("Yes.Button")
                If MainForm.OnlineManagement AndAlso Not MainForm.NoNTSamMappings Then
                    DynaLog.LogMessage("Online installation management mode has been detected and we're expected to map SAM information. Proceeding...")
                    Try
                        Dim profileFiles As New List(Of String)
                        profileFiles = My.Computer.FileSystem.GetFiles(MainForm.MountDir & "\ProgramData\Microsoft\Windows\AppRepository\Packages\" & Label23.Text, FileIO.SearchOption.SearchTopLevelOnly, "*.pckgdep").ToList()
                        If profileFiles.Count > 0 Then
                            Label42.Text &= ":" & CrLf & SamHelper.MapPckgdepsToSamProfiles(profileFiles)
                        End If
                    Catch ex As Exception

                    End Try
                End If
            Else
                Label42.Text = LocalizationService.ForSection("Get.AppX.PackageList")("No.Button")
            End If

            DynaLog.LogMessage("Getting AppX main Store logo asset...")
            mainAsset = MainForm.GetStoreAppMainLogo(Label23.Text)
            DynaLog.LogMessage("Main asset location: " & Quote & mainAsset & Quote)
            If mainAsset <> "" And File.Exists(mainAsset) Then
                Try
                    DynaLog.LogMessage("Attempting to open picture...")
                    Dim asset As Image = Image.FromFile(mainAsset)
                    If (asset.Width > PictureBox2.Width) Or (asset.Height > PictureBox2.Height) Then
                        PictureBox2.SizeMode = PictureBoxSizeMode.Zoom
                    Else
                        PictureBox2.SizeMode = PictureBoxSizeMode.CenterImage
                    End If
                Catch ex As Exception
                    DynaLog.LogMessage("Could not open picture. This can happen if the file is corrupted. Error message: " & ex.Message)
                End Try
            Else
                Label10.Visible = False
                LinkLabel1.Visible = False
                PictureBox2.SizeMode = PictureBoxSizeMode.CenterImage
            End If
            Try
                If mainAsset <> "" And File.Exists(mainAsset) Then PictureBox2.Image = Image.FromFile(mainAsset) Else PictureBox2.Image = GetGlyphResource("preview_unavail")
            Catch ex As Exception
                PictureBox2.SizeMode = PictureBoxSizeMode.CenterImage
                PictureBox2.Image = GetGlyphResource("preview_unavail")
            End Try
            Try
                assetDir = MainForm.GetSuitablePackageFolder(Label25.Text)
            Catch ex As Exception
                ' Continue
            End Try
            DynaLog.LogMessage("Part of asset directory: " & Quote & assetDir & Quote)
            If assetDir <> "" Then
                DynaLog.LogMessage("Getting full asset directory...")
                If File.Exists(assetDir & "\AppxManifest.xml") Then
                    Dim ManFile As New RichTextBox() With {
                        .Text = File.ReadAllText(assetDir & "\AppxManifest.xml")
                    }
                    For Each line In ManFile.Lines
                        If line.Contains("<Logo>") Then
                            Dim SplitPaths As New List(Of String)
                            SplitPaths = line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim().Split("\").ToList()
                            SplitPaths.RemoveAt(SplitPaths.Count - 1)
                            Dim newPath As String = String.Join("\", SplitPaths)
                            Label7.Text = (assetDir & "\" & newPath).Replace("\\", "\").Trim()
                            Exit For
                        End If
                    Next
                End If
            Else
                DynaLog.LogMessage("Getting full asset directory...")
                If File.Exists(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & Label23.Text & "\AppxManifest.xml") Then
                    Dim ManFile As New RichTextBox() With {
                        .Text = File.ReadAllText(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & Label23.Text & "\AppxManifest.xml")
                    }
                    For Each line In ManFile.Lines
                        If line.Contains("<Logo>") Then
                            Dim SplitPaths As New List(Of String)
                            SplitPaths = line.Replace(" ", "").Trim().Replace("/", "").Trim().Replace("<Logo>", "").Trim().Split("\").ToList()
                            SplitPaths.RemoveAt(SplitPaths.Count - 1)
                            Dim newPath As String = String.Join("\", SplitPaths)
                            Label7.Text = (If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & Label23.Text & "\" & newPath).Replace("\\", "\").Trim()
                            Exit For
                        End If
                    Next
                End If
            End If
            DynaLog.LogMessage("Getting location of AppX manifest...")
            Label3.Text = (If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps\" & Label23.Text).Replace("\\", "\").Trim()
            Dim pkgDirs() As String = Directory.GetDirectories(If(MainForm.OnlineManagement, Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Windows)), MainForm.MountDir) & "\Program Files\WindowsApps", Label23.Text & "*", SearchOption.TopDirectoryOnly)
            For Each folder In pkgDirs
                If Not folder.Contains("neutral") Then
                    Label5.Text = (folder & "\AppxManifest.xml").Replace("\\", "\").Trim()
                End If
            Next
            Try
                If pkgDirs.Count <= 1 And Not Label5.Text.Contains(Label23.Text) Then
                    If File.Exists(pkgDirs(0).Replace("\\", "\").Trim() & "\AppxMetadata\AppxBundleManifest.xml") Then
                        DynaLog.LogMessage("The installed application originated from a bundle package.")
                        Label5.Text = pkgDirs(0).Replace("\\", "\").Trim() & "\AppxMetadata\AppxBundleManifest.xml"
                    ElseIf File.Exists(pkgDirs(0).Replace("\\", "\").Trim() & "\AppxManifest.xml") Then
                        DynaLog.LogMessage("The installed application originated from a regular AppX package.")
                        Label5.Text = pkgDirs(0).Replace("\\", "\").Trim() & "\AppxManifest.xml"
                    Else
                        DynaLog.LogMessage("The installed application originated from an unknown source.")
                        Label5.Text = ""
                    End If
                End If
            Catch ex As Exception
                DynaLog.LogMessage("Could not get some information about this application. Error message: " & ex.Message)
                MsgBox(LocalizationService.ForSection("AppxPackages.Info.Messages")("Get.Label"), vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            End Try
            Panel4.Visible = True
            Panel7.Visible = False
        Else
            Panel4.Visible = False
            Panel7.Visible = True
        End If
    End Sub

    Private Sub PictureBox2_MouseClick(sender As Object, e As MouseEventArgs) Handles PictureBox2.MouseClick
        If mainAsset <> "" And File.Exists(mainAsset) Then
            If e.Button = Windows.Forms.MouseButtons.Right Then
                MainForm.AppxResCMS.Show(sender, e.Location)
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MainForm.ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Saving installed AppX package information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = MainForm.SourceImg
            ImgInfoSaveDlg.ImgMountDir = If(Not MainForm.OnlineManagement, MainForm.MountDir, "")
            ImgInfoSaveDlg.SaveTarget = MainForm.ImgInfoSFD.FileName
            ImgInfoSaveDlg.OnlineMode = MainForm.OnlineManagement
            ImgInfoSaveDlg.OfflineMode = MainForm.OfflineManagement
            ImgInfoSaveDlg.SkipQuestions = MainForm.SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = MainForm.AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = 5
            ImgInfoSaveDlg.ImageToGetInfoFrom = MainForm.CurrentImage
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://github.com/CodingWonders/DISMTools/issues/new?assignees=CodingWonders&labels=bug&projects=&template=store-logo-asset-preview-issue.md&title=")
    End Sub

    Sub SearchPackages(sQuery As String)
        DynaLog.LogMessage("Search query: " & sQuery)
        If MainForm.CurrentImage.ImageAppxPackages Is Nothing OrElse MainForm.CurrentImage.ImageAppxPackages.Count = 0 OrElse
            MainForm.CurrentImage.ImageAppxPackages_Backup.Count > MainForm.CurrentImage.ImageAppxPackages.Count Then
            FilteredAppxPackages_Backup = MainForm.CurrentImage.ImageAppxPackages_Backup.Where(Function(AppxPackage) AppxPackage.PackageFullName.ToLower().Contains(sQuery.ToLower()))
            ListBox1.Items.AddRange(FilteredAppxPackages_Backup.Select(Function(AppxPackage) AppxPackage.PackageFullName).ToArray())
        Else
            FilteredAppxPackages = MainForm.CurrentImage.ImageAppxPackages.Where(Function(AppxPackage) AppxPackage.PackageName.ToLower().Contains(sQuery.ToLower()))
            FilteredAppxPackages_Backup = Enumerable.Repeat(Of ImageAppxPackage)(Nothing, FilteredAppxPackages.Count)
            ListBox1.Items.AddRange(FilteredAppxPackages.Select(Function(AppxPackage) AppxPackage.PackageName).ToArray())
        End If
    End Sub

    Private Sub SearchBox1_TextChanged(sender As Object, e As EventArgs) Handles SearchBox1.TextChanged
        ListBox1.Items.Clear()
        If SearchBox1.Text <> "" Then
            SearchPackages(SearchBox1.Text)
        Else
            DynaLog.LogMessage("No search query has been specified. Showing all items...")
            If MainForm.CurrentImage.ImageAppxPackages IsNot Nothing Then
                If MainForm.CurrentImage.ImageAppxPackages_Backup.Count > MainForm.CurrentImage.ImageAppxPackages.Count Then
                    ListBox1.Items.AddRange(MainForm.CurrentImage.ImageAppxPackages_Backup.Select(Function(appxPackage) appxPackage.PackageFullName).ToArray())
                Else
                    ListBox1.Items.AddRange(MainForm.CurrentImage.ImageAppxPackages.Select(Function(appxPackage) appxPackage.PackageName).ToArray())
                End If
            Else
                ListBox1.Items.AddRange(MainForm.CurrentImage.ImageAppxPackages_Backup.Select(Function(appxPackage) appxPackage.PackageFullName).ToArray())
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
