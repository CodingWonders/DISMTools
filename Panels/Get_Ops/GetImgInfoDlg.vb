Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.Dism
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports DISMTools.Utilities
Imports System.Globalization

Public Class GetImgInfoDlg

    Dim ImageInfoCollection As DismImageInfoCollection
    Dim ImageInfoList As New List(Of DismImageInfo)
    Dim DismVersionChecker As FileVersionInfo

    Dim SelectedImageFile As String

    Private Sub GetImgInfoDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("ImageInfo")("Get.Image.Label")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("ImageInfo").Format("Image.Task.Header.Label", Text)
        Label2.Text = LocalizationService.ForSection("ImageInfo")("ImageFile.Get.Label")
        Label3.Text = LocalizationService.ForSection("ImageInfo")("List.Indexes.ImageFile.Label")
        Label22.Text = LocalizationService.ForSection("ImageInfo")("ImageVersion.Label")
        Label24.Text = LocalizationService.ForSection("ImageInfo")("ImageName.Label")
        Label26.Text = LocalizationService.ForSection("ImageInfo")("ImageDescription.Label")
        Label31.Text = LocalizationService.ForSection("ImageInfo")("ImageSize.Label")
        Label41.Text = LocalizationService.ForSection("ImageInfo")("Supports.WIM.Boot.Label")
        Label43.Text = LocalizationService.ForSection("ImageInfo")("Architecture.Label")
        Label47.Text = LocalizationService.ForSection("ImageInfo")("HAL.Label")
        Label33.Text = LocalizationService.ForSection("ImageInfo")("ServicePackBuild.Label")
        Label28.Text = LocalizationService.ForSection("ImageInfo")("ServicePackLevel.Label")
        Label30.Text = LocalizationService.ForSection("ImageInfo")("InstallationType.Label")
        Label39.Text = LocalizationService.ForSection("ImageInfo")("Edition.Label")
        Label45.Text = LocalizationService.ForSection("ImageInfo")("ProductType.Label")
        Label5.Text = LocalizationService.ForSection("ImageInfo")("ProductSuite.Label")
        Label7.Text = LocalizationService.ForSection("ImageInfo")("System.Root.Dir.Label")
        Label9.Text = LocalizationService.ForSection("ImageInfo")("FileCount.Label")
        Label11.Text = LocalizationService.ForSection("ImageInfo")("Dates.Label")
        Label13.Text = LocalizationService.ForSection("ImageInfo")("Installed.Languages.Label")
        Label36.Text = LocalizationService.ForSection("ImageInfo")("ImageInfo.Label")
        Label37.Text = LocalizationService.ForSection("ImageInfo")("Index.List.View.Label")
        RadioButton1.Text = LocalizationService.ForSection("ImageInfo")("CurrentlyMounted.RadioButton")
        RadioButton2.Text = LocalizationService.ForSection("ImageInfo")("AnotherImage.RadioButton")
        Button1.Text = LocalizationService.ForSection("ImageInfo")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("ImageInfo")("Save.Button")
        Button3.Text = LocalizationService.ForSection("ImageInfo")("Pick.Button")
        ListView1.Columns(0).Text = LocalizationService.ForSection("ImageInfo")("Index.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("ImageInfo")("ImageName.Column")
        OpenFileDialog1.Title = LocalizationService.ForSection("ImageInfo")("Image.Get.Title")
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        LanguageList.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        LanguageList.ForeColor = ForeColor
        If SplitContainer2.SplitterDistance = 440 Then
            SplitContainer2.SplitterDistance = WindowHelper.ScaleLogical(SplitContainer2.SplitterDistance)
        End If

        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        DismVersionChecker = FileVersionInfo.GetVersionInfo(MainForm.DismExe)
        If Not MainForm.IsImageMounted Or MainForm.OnlineManagement Then
            RadioButton1.Enabled = False
            RadioButton1.Checked = False
            RadioButton2.Checked = True
            If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then Button2.Enabled = True Else Button2.Enabled = False
        Else
            RadioButton1.Enabled = True
        End If
        ColumnHeader1.Width = WindowHelper.ScaleLogical(60)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(344)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Sub GetImageInfo(ImageFile As String)
        DynaLog.LogMessage("Image file to get information about: " & Quote & ImageFile & Quote)
        MainForm.StopMountedImageDetector()
        ImageInfoList.Clear()
        ListView1.Items.Clear()
        Try
            DynaLog.LogMessage("Getting information about the image file...")
            SelectedImageFile = ImageFile
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            ImageInfoCollection = DismApi.GetImageInfo(ImageFile)
            DynaLog.LogMessage("Information collection count: " & ImageInfoCollection.Count)
            If ImageInfoCollection.Count > 0 Then
                DynaLog.LogMessage("This file has images. Updating lists...")
                ListView1.Items.AddRange(ImageInfoCollection.Select(Function(ImageInfo) New ListViewItem(New String() {ImageInfo.ImageIndex, ImageInfo.ImageName})).ToArray())
                ImageInfoList.AddRange(ImageInfoCollection.Select(Function(ImageInfo) ImageInfo))
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
            Dim msg As String = ""
            msg = LocalizationService.ForSection("ImageInfo.GetImageInfo").Format("Gather.ImageFile.Message", ex.ToString(), ex.Message, Hex(ex.HResult))
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
        Finally
            DynaLog.LogMessage("Shutting down API...")
            Try
                DismApi.Shutdown()
            Catch ex As Exception
                ' Don't do anything
            End Try
        End Try
        DynaLog.LogMessage("This process has finished.")
    End Sub

    Sub DisplayImageInfo(Index As Integer)
        DynaLog.LogMessage("Displaying information of specified image file...")
        DynaLog.LogMessage("Index to get information about: " & Index + 1)
        Label23.Text = ImageInfoList(Index).ProductVersion.ToString()
        DetectFeatureUpdate(ImageInfoList(Index).ProductVersion)
        Label25.Text = ImageInfoList(Index).ImageName
        Label35.Text = ImageInfoList(Index).ImageDescription
        Dim isFrenchSizeText As Boolean = LocalizationService.CurrentCultureCode.Equals("fr-FR", StringComparison.OrdinalIgnoreCase)
        Dim readableImageSize As String = If(isFrenchSizeText, Converters.BytesToReadableSize(ImageInfoList(Index).ImageSize, True), Converters.BytesToReadableSize(ImageInfoList(Index).ImageSize))
        Label32.Text = LocalizationService.ForSection("ImageInfo").Format("Bytes.Label", ImageInfoList(Index).ImageSize.ToString("N0"), readableImageSize)

        Label42.Text = Casters.CastDismArchitecture(ImageInfoList(Index).Architecture, True)
        Label46.Text = If(Not ImageInfoList(Index).Hal = "", ImageInfoList(Index).Hal, LocalizationService.ForSection("ImageInfo.DisplayImageInfo")("UndefinedImage.Label"))
        Label34.Text = ImageInfoList(Index).ProductVersion.Revision
        Label27.Text = ImageInfoList(Index).SpLevel
        Label29.Text = ImageInfoList(Index).InstallationType
        Label38.Text = ImageInfoList(Index).EditionId
        Label4.Text = ImageInfoList(Index).ProductType
        Label44.Text = ImageInfoList(Index).ProductSuite
        Label8.Text = ImageInfoList(Index).SystemRoot
        LanguageList.Items.Clear()
        For Each language In ImageInfoList(Index).Languages
            LanguageList.Items.Add(language.Name & LocalizationService.ForSection("ImageInfo.LanguageList")("Display.Name.Open.Label") & language.DisplayName & If(ImageInfoList(Index).DefaultLanguage.Name = language.Name, LocalizationService.ForSection("ImageInfo.LanguageList")("Default.Label"), "") & LocalizationService.ForSection("ImageInfo.LanguageList")("Display.Name.Close.Label"))
        Next
        If ImageInfoList(Index).CustomizedInfo IsNot Nothing Then
            Dim CurrentOSCulture As CultureInfo = CultureInfo.CurrentCulture
            Dim ImageCreationDate As String = "",
                ImageModificationDate As String = ""
            Dim CreatedDate As Date = ImageInfoList(Index).CustomizedInfo.CreatedTime,
                ModifiedDate As Date = ImageInfoList(Index).CustomizedInfo.ModifiedTime
            If MainForm.HumanizeDates Then
                ImageCreationDate = String.Format("{0}, {1}", CreatedDate.ToString(CurrentOSCulture.DateTimeFormat.LongDatePattern, CurrentOSCulture), CreatedDate.ToString(CurrentOSCulture.DateTimeFormat.LongTimePattern, CurrentOSCulture))
                ImageModificationDate = String.Format("{0}, {1}", ModifiedDate.ToString(CurrentOSCulture.DateTimeFormat.LongDatePattern, CurrentOSCulture), ModifiedDate.ToString(CurrentOSCulture.DateTimeFormat.LongTimePattern, CurrentOSCulture))
            Else
                ImageCreationDate = CreatedDate.ToString("MM/dd/yyyy HH:mm:ss")
                ImageModificationDate = ModifiedDate.ToString("MM/dd/yyyy HH:mm:ss")
            End If

            Label6.Text = LocalizationService.ForSection("ImageInfo.DisplayImageInfo").Format("FilesDirectories.Label", ImageInfoList(Index).CustomizedInfo.FileCount, ImageInfoList(Index).CustomizedInfo.DirectoryCount)
            Label10.Text = LocalizationService.ForSection("ImageInfo.DisplayImageInfo").Format("Date.Created.Modified.Label", ImageCreationDate, ImageModificationDate)
        Else
            Label6.Text = ""
            Label10.Text = ""
        End If

        DynaLog.LogMessage("Getting WIMBoot status...")

        ' The DISM API part is over. Switch to regular DISM.exe mode for missing details
        Try     ' Try getting image properties
            If Not Directory.Exists(Application.StartupPath & "\tempinfo") Then
                Directory.CreateDirectory(Application.StartupPath & "\tempinfo").Attributes = FileAttributes.Hidden
            End If
            Select Case DismVersionChecker.ProductMajorPart
                Case 6
                    Select Case DismVersionChecker.ProductMinorPart
                        Case 1
                            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat",
                                              "@echo off" & CrLf &
                                              "dism /English /get-wiminfo /wimfile=" & Quote & SelectedImageFile & Quote & " /index=" & ListView1.FocusedItem.Index + 1 & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & Quote & Application.StartupPath & "\tempinfo\imgwimboot" & Quote, ASCII)
                        Case Is >= 2
                            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat",
                                              "@echo off" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & Quote & SelectedImageFile & Quote & " /index=" & ListView1.FocusedItem.Index + 1 & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & Quote & Application.StartupPath & "\tempinfo\imgwimboot" & Quote, ASCII)
                    End Select
                Case 10
                    File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat",
                                      "@echo off" & CrLf &
                                      "dism /English /get-imageinfo /imagefile=" & Quote & SelectedImageFile & Quote & " /index=" & ListView1.FocusedItem.Index + 1 & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & Quote & Application.StartupPath & "\tempinfo\imgwimboot" & Quote, ASCII)
            End Select
            If Debugger.IsAttached Then
                Process.Start(Environment.GetEnvironmentVariable("SYSTEMROOT") & "\system32\notepad.exe", Application.StartupPath & "\bin\exthelpers\imginfo.bat").WaitForExit()
            End If
            Using WIMBootProc As New Process()
                WIMBootProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
                WIMBootProc.StartInfo.Arguments = "/c " & Quote & Application.StartupPath & "\bin\exthelpers\imginfo.bat" & Quote
                WIMBootProc.StartInfo.CreateNoWindow = True
                WIMBootProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                WIMBootProc.Start()
                WIMBootProc.WaitForExit()
            End Using
            Try
                Label40.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\tempinfo\imgwimboot", ASCII).Replace("WIM Bootable : ", "").Trim()
                If Not MainForm.ImgBW.IsBusy Then
                    For Each foundFile In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\tempinfo", FileIO.SearchOption.SearchTopLevelOnly)
                        File.Delete(foundFile)
                    Next
                    Directory.Delete(Application.StartupPath & "\tempinfo")
                End If
                File.Delete(Application.StartupPath & "\bin\exthelpers\imginfo.bat")
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
    End Sub

    Sub DetectFeatureUpdate(SysVer As Version)
        DynaLog.LogMessage("Detecting feature update from version...")
        DynaLog.LogMessage("Version: " & SysVer.ToString())
        Dim FeatUpd As String = ""
        Select Case SysVer.Major
            Case 10
                Select Case SysVer.Build
                    Case 9650 To 10240
                        FeatUpd = "1507 (Threshold 1)"
                    Case 10525 To 10587     ' 10587 is a Post-RTM build of Windows 10 November Update
                        FeatUpd = "1511 (Threshold 2)"
                    Case 11065 To 14393
                        FeatUpd = "1607 (Redstone 1)"
                    Case 14832 To 15063
                        FeatUpd = "1703 (Redstone 2)"
                    Case 15140 To 16299
                        FeatUpd = "1709 (Redstone 3)"
                    Case 16251 To 17134
                        FeatUpd = "1803 (Redstone 4)"
                    Case 17604 To 17763
                        FeatUpd = "1809 (Redstone 5)"
                    Case 18204 To 18362
                        FeatUpd = "1903 (Titanium)"
                    Case Is = 18362
                        If SysVer.Revision >= 10000 Then
                            FeatUpd = "1909 (Vanadium)"
                        Else
                            FeatUpd = "1903 (Titanium)"
                        End If
                    Case Is = 18363
                        FeatUpd = "1909 (Vanadium)"
                    Case 18826 To 19041
                        FeatUpd = "2004 (Vibranium)"
                    Case 19041 To 19489
                        FeatUpd = "2004+ (Vibranium)"
                    Case 19489 To 19645
                        FeatUpd = "2004 (Manganese)"
                    Case 20124 To 20279
                        FeatUpd = "21H1 (Iron)"
                    Case 20282 To 20348
                        FeatUpd = "21H2 (Iron)"
                    Case 21242 To 22000     ' Also includes Windows 11 Cobalt (21H2)
                        FeatUpd = "21H2 (Cobalt)"
                    Case 22350 To 22630     ' This goes until Windows 11 build 22631 (2022 Update Moment 4)
                        FeatUpd = "22H2 (Nickel)"
                    Case 22631 To 22634
                        FeatUpd = "23H2 (Nickel)"
                    Case 22635 To 23400
                        FeatUpd = "23H2 (Nickel Moment 5)"
                    Case 23401 To 25000
                        FeatUpd = "Dev (Nickel)"
                    Case 25057 To 25238
                        FeatUpd = "23H1 (Copper)"
                    Case 25240 To 25400     ' 25400 is a relative number. 25398 is the final build of Zinc
                        FeatUpd = "23H2 (Zinc)"
                    Case 25801 To 25941
                        FeatUpd = "24H1 (Gallium)"
                    Case 25942 To 26199
                        FeatUpd = "24H2 (Germanium)"
                    Case 26200 To 27500
                        FeatUpd = "25H2 (Germanium)"
                    Case 27501 To 27686
                        FeatUpd = "25H1 (Dilithium)"
                    Case 27687 To 27788
                        FeatUpd = "25H2 (Selenium)"
                    Case 27789 To 28999
                        FeatUpd = "26H1 (Bromine)"
                    Case Is >= 29000
                        FeatUpd = "26H2 (Krypton)"
                End Select
            Case Else
                Exit Sub
        End Select
        DynaLog.LogMessage("Detected feature update: " & FeatUpd)
        Label23.Text &= LocalizationService.ForSection("ImageInfo.FeatureUpdate")("FeatureUpdate.Label") & FeatUpd & LocalizationService.ForSection("ImageInfo.FeatureUpdate")("Text1.Label")
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            GetImageInfo(TextBox1.Text)
            Button2.Enabled = True
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        Button2.Enabled = False
        If RadioButton1.Checked Then
            ' Go through the mounted image listings to find the appropriate image
            If MainForm.MountedImageList.Count > 0 Then
                TextBox1.Enabled = False
                Button1.Enabled = False
                Button3.Enabled = False
                Dim ActualImage As WindowsImage = MainForm.MountedImageList.FirstOrDefault(Function(image) image.ImageMountDirectory = MainForm.MountDir)
                If ActualImage IsNot Nothing Then
                    DynaLog.LogMessage("Getting information about the mounted image...")
                    GetImageInfo(ActualImage.ImageFile)
                    Button2.Enabled = True
                End If
            End If
        Else
            TextBox1.Enabled = True
            Button1.Enabled = True
            Button3.Enabled = True

            ' If the user had specified an image file, get information of it immediately
            If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
                GetImageInfo(TextBox1.Text)
                Button2.Enabled = True
            End If
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            If ListView1.SelectedItems.Count = 1 Then
                Panel7.Visible = False
                Panel4.Visible = True
                DisplayImageInfo(ListView1.FocusedItem.Index)
            Else
                Panel7.Visible = True
                Panel4.Visible = False
            End If
        Catch ex As Exception
            Panel7.Visible = True
            Panel4.Visible = False
        End Try
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub GetImgInfoDlg_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not MainForm.MountedImageDetectorBW.IsBusy And Not PleaseWaitDialog.Visible Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
        MainForm.WatcherTimer.Enabled = True
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MainForm.ImgInfoSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Preparing to save image information...")
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            ImgInfoSaveDlg.SourceImage = SelectedImageFile
            ImgInfoSaveDlg.SaveTarget = MainForm.ImgInfoSFD.FileName
            ImgInfoSaveDlg.OnlineMode = False
            ImgInfoSaveDlg.OfflineMode = False
            ImgInfoSaveDlg.SaveTask = 1
            ImgInfoSaveDlg.ShowDialog(Me)
            InfoSaveResults.Show()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim selectedImage As WindowsImage = PopupMountedImagePicker.PickImage()
        If selectedImage IsNot Nothing Then
            TextBox1.Text = selectedImage.ImageFile
        End If
    End Sub
End Class
