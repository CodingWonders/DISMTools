Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports Microsoft.Dism
Imports DISMTools.Utilities
Imports System.Threading
Imports System.Globalization

Public Class ProjProperties

    Dim ImgSizeStr As String
    Dim DismVersionChecker As FileVersionInfo

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    ''' <summary>
    ''' Detects the properties of the image using the DISM API and, later, the DISM executable
    ''' </summary>
    ''' <remarks></remarks>
    Sub DetectImageProperties()
        ' Detect mounted images to find the loaded one
        Try
            If MainForm.CurrentImage Is Nothing Then
                Exit Sub
            End If
            DynaLog.LogMessage("Basic image information has been obtained.")
            DynaLog.LogMessage("- Image file : " & MainForm.CurrentImage.ImageFile)
            DynaLog.LogMessage("- Image index : " & MainForm.CurrentImage.ImageIndex)
            DynaLog.LogMessage("- Mount directory : " & MainForm.CurrentImage.ImageMountDirectory)
            DynaLog.LogMessage("- Mount status : " & If(MainForm.CurrentImage.ImageMountStatus = DismMountStatus.Ok, " (OK)", If(MainForm.CurrentImage.ImageMountStatus = DismMountStatus.NeedsRemount, " (Orphaned)", " (Invalid)")))
            DynaLog.LogMessage("- Mount mode : " & If(MainForm.CurrentImage.ImageMountMode = DismMountMode.ReadWrite, " (Write permissions enabled)", "(Write permissions disabled)"))
            imgName.Text = MainForm.CurrentImage.ImageFile
            imgIndex.Text = MainForm.CurrentImage.ImageIndex
            imgMountDir.Text = MainForm.CurrentImage.ImageMountDirectory
            imgMountedStatus.Text = MainForm.CurrentImage.MountStatusToString()
            RecoverButton.Visible = MainForm.CurrentImage.ImageMountStatus = DismMountStatus.Invalid
            RemountImgBtn.Visible = MainForm.CurrentImage.ImageMountStatus = DismMountStatus.NeedsRemount
            imgVersion.Text = MainForm.CurrentImage.ImageVersion.ToString()
            DetectFeatureUpdate(MainForm.CurrentImage.ImageVersion)
            imgMountedName.Text = MainForm.CurrentImage.ImageName
            imgMountedDesc.Text = MainForm.CurrentImage.ImageDescription
            Dim isFrenchSizeText As Boolean = LocalizationService.CurrentCultureCode.Equals("fr-FR", StringComparison.OrdinalIgnoreCase)
            Dim readableImageSize As String = If(isFrenchSizeText, Converters.BytesToReadableSize(MainForm.CurrentImage.ImageSize, True), Converters.BytesToReadableSize(MainForm.CurrentImage.ImageSize))
            imgSize.Text = LocalizationService.ForSection("ProjProps").Format("Bytes.Item", MainForm.CurrentImage.ImageSize.ToString("N0"), readableImageSize)

            imgArch.Text = Casters.CastDismArchitecture(MainForm.CurrentImage.ImageArchitecture, True)
            imgHal.Text = If(Not MainForm.CurrentImage.ImageHal = "", MainForm.CurrentImage.ImageHal, LocalizationService.ForSection("ProjectProps.Image")("UndefinedImage.Label"))
            imgSPBuild.Text = MainForm.CurrentImage.ImageVersion.Revision
            imgSPLvl.Text = MainForm.CurrentImage.ImageSpLevel
            imgEdition.Text = MainForm.CurrentImage.ImageEditionId
            imgInstType.Text = MainForm.CurrentImage.ImageInstallationType
            imgPType.Text = MainForm.CurrentImage.ImageProductType
            imgPSuite.Text = MainForm.CurrentImage.ImageProductSuite
            imgSysRoot.Text = MainForm.CurrentImage.ImageSystemRoot
            DynaLog.LogMessage("Language count: " & MainForm.CurrentImage.ImageLanguages.Count)
            For Each language In MainForm.CurrentImage.ImageLanguages
                LanguageList.Items.Add(language.Name & LocalizationService.ForSection("ProjectProps.Image")("OpenParenthesis.Label") & language.DisplayName & If(MainForm.CurrentImage.ImageDefaultLanguage.Name = language.Name, LocalizationService.ForSection("ProjectProps.Image")("Default.Label"), "") & LocalizationService.ForSection("ProjectProps.Image")("CloseParenthesis.Label"))
            Next
            imgFormat.Text = LocalizationService.ForSection("ProjectProps.Image").Format("File.Label", Path.GetExtension(MainForm.CurrentImage.ImageFile).Replace(".", "").Trim().ToUpper())
            imgRW.Text = MainForm.CurrentImage.MountModeToString()
            RWRemountBtn.Visible = MainForm.CurrentImage.ImageMountMode = DismMountMode.ReadOnly
            imgDirs.Text = MainForm.CurrentImage.ImageDirectoryCount
            imgFiles.Text = MainForm.CurrentImage.ImageFileCount
            Dim CurrentOSCulture As CultureInfo = CultureInfo.CurrentCulture
            Dim ImageCreationDate As String = "",
                ImageModificationDate As String = ""
            Dim CreatedDate As Date = MainForm.CurrentImage.ImageCreationDate,
                ModifiedDate As Date = MainForm.CurrentImage.ImageModificationDate
            If MainForm.HumanizeDates Then
                ImageCreationDate = String.Format("{0}, {1}", CreatedDate.ToString(CurrentOSCulture.DateTimeFormat.LongDatePattern, CurrentOSCulture), CreatedDate.ToString(CurrentOSCulture.DateTimeFormat.LongTimePattern, CurrentOSCulture))
                ImageModificationDate = String.Format("{0}, {1}", ModifiedDate.ToString(CurrentOSCulture.DateTimeFormat.LongDatePattern, CurrentOSCulture), ModifiedDate.ToString(CurrentOSCulture.DateTimeFormat.LongTimePattern, CurrentOSCulture))
            Else
                ImageCreationDate = CreatedDate.ToString("MM/dd/yyyy HH:mm:ss")
                ImageModificationDate = ModifiedDate.ToString("MM/dd/yyyy HH:mm:ss")
            End If
            imgCreation.Text = ImageCreationDate
            imgModification.Text = ImageModificationDate
            DynaLog.LogMessage("Getting WIMBoot information")
            Dim args As String = "/English",
                out As String = ""
            If DismVersionChecker.ProductMajorPart = 6 AndAlso DismVersionChecker.FileMinorPart = 1 Then
                args &= String.Format(" /get-wiminfo /wimfile={0} ", Quote & MainForm.CurrentImage.ImageFile & Quote)
            Else
                args &= String.Format(" /get-imageinfo /imagefile={0} ", Quote & MainForm.CurrentImage.ImageFile & Quote)
            End If
            args &= String.Format(" /index={0}", MainForm.CurrentImage.ImageIndex)
            Using WIMBootProc As New Process() With {
                .StartInfo = New ProcessStartInfo() With {
                    .FileName = MainForm.DismExe,
                    .Arguments = args,
                    .UseShellExecute = False,
                    .CreateNoWindow = True,
                    .RedirectStandardOutput = True,
                    .RedirectStandardError = True,
                    .WindowStyle = ProcessWindowStyle.Hidden
                }
            }
                WIMBootProc.Start()
                out = WIMBootProc.StandardOutput.ReadToEnd()
                WIMBootProc.WaitForExit()

                If WIMBootProc.ExitCode = 0 Then
                    imgWimBootStatus.Text = If(out.ToLower().Contains("wim bootable : yes"), "Yes", "No")
                End If
            End Using
        Catch ex As Exception
            DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
        End Try
    End Sub

    Private Sub ProjProperties_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label4.Text = LocalizationService.ForSection("ProjProps")("Getting.Project.Image.Label")
        Label5.Text = LocalizationService.ForSection("ProjProps")("Name.Label")
        Label6.Text = LocalizationService.ForSection("ProjProps")("Location.Label")
        Label7.Text = LocalizationService.ForSection("ProjProps")("Creation.Time.Date.Label")
        Label8.Text = LocalizationService.ForSection("ProjProps")("ProjectGUID.Label")
        Label13.Text = LocalizationService.ForSection("ProjProps")("MountDirectory.Label")
        Label14.Text = LocalizationService.ForSection("ProjProps")("ImageIndex.Label")
        Label15.Text = LocalizationService.ForSection("ProjProps")("ImageFile.Label")
        Label20.Text = LocalizationService.ForSection("ProjProps")("Image.Present.Project.Label")
        Label22.Text = LocalizationService.ForSection("ProjProps")("ImageStatus.Label")
        Label25.Text = LocalizationService.ForSection("ProjProps")("Version.Label")
        Label27.Text = LocalizationService.ForSection("ProjProps")("Name.Label")
        Label29.Text = LocalizationService.ForSection("ProjProps")("Description.Label")
        Label31.Text = LocalizationService.ForSection("ProjProps")("Size.Label")
        Label33.Text = LocalizationService.ForSection("ProjProps")("Supports.WIM.Boot.Label")
        Label35.Text = LocalizationService.ForSection("ProjProps")("Architecture.Label")
        Label39.Text = LocalizationService.ForSection("ProjProps")("ServicePackBuild.Label")
        Label41.Text = LocalizationService.ForSection("ProjProps")("ServicePackLevel.Label")
        Label43.Text = LocalizationService.ForSection("ProjProps")("Edition.Label")
        Label45.Text = LocalizationService.ForSection("ProjProps")("ProductType.Label")
        Label47.Text = LocalizationService.ForSection("ProjProps")("ProductSuite.Label")
        Label49.Text = LocalizationService.ForSection("ProjProps")("System.Root.Dir.Label")
        Label51.Text = LocalizationService.ForSection("ProjProps")("DirectoryCount.Label")
        Label53.Text = LocalizationService.ForSection("ProjProps")("FileCount.Label")
        Label55.Text = LocalizationService.ForSection("ProjProps")("CreationDate.Label")
        Label57.Text = LocalizationService.ForSection("ProjProps")("ModificationDate.Label")
        Label58.Text = LocalizationService.ForSection("ProjProps")("Installed.Languages.Label")
        Label60.Text = LocalizationService.ForSection("ProjProps")("FileFormat.Label")
        Label62.Text = LocalizationService.ForSection("ProjProps")("Image.Rwpermissions.Label")
        RecoverButton.Text = LocalizationService.ForSection("ProjProps")("Recover.Label")
        RemountImgBtn.Text = LocalizationService.ForSection("ProjProps")("Reload.Label")
        RWRemountBtn.Text = LocalizationService.ForSection("ProjProps")("Remount.Write.Label")
        OK_Button.Text = LocalizationService.ForSection("ProjProps")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("ProjProps")("Cancel.Button")
        LinkLabel2.Text = LocalizationService.ForSection("ProjProps")("Many.Cannot.Seen.Message")
        ImageTaskHeader1.ItemText = LocalizationService.ForSection("ProjProps")("Props.Label")
        ' Set program colors
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        LanguageList.BackColor = CurrentTheme.SectionBackgroundColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        LanguageList.ForeColor = ForeColor
        DismVersionChecker = FileVersionInfo.GetVersionInfo(MainForm.DismExe)
        imgMountDir.Text = ""
        imgIndex.Text = ""
        imgName.Text = ""
        imgMountedStatus.Text = ""
        imgVersion.Text = ""
        imgMountedName.Text = ""
        imgMountedDesc.Text = ""
        imgSize.Text = ""
        imgWimBootStatus.Text = ""
        imgArch.Text = ""
        imgHal.Text = ""
        imgSPBuild.Text = ""
        imgSPLvl.Text = ""
        imgEdition.Text = ""
        imgPType.Text = ""
        imgPSuite.Text = ""
        imgSysRoot.Text = ""
        imgDirs.Text = ""
        imgFiles.Text = ""
        imgCreation.Text = ""
        imgModification.Text = ""
        imgFormat.Text = ""
        imgRW.Text = ""
        LanguageList.Items.Clear()
        Visible = True
        Label4.Visible = True
        Label9.Text = MainForm.Label49.Text
        Label10.Text = MainForm.projPath
        Label11.Text = File.GetCreationTime(MainForm.projPath)
        DynaLog.LogMessage("Getting project information...")
        Dim ProjectFileLines As String() = File.ReadAllLines(MainForm.projPath & "\" & MainForm.Label49.Text & ".dtproj")
        If ProjectFileLines(6).StartsWith("ProjGuid") Then
            Label12.Text = ProjectFileLines(6).Replace("ProjGuid=", "").Trim()
        End If
        If MainForm.IsImageMounted Then
            DynaLog.LogMessage("An image is mounted.")
            Label19.Text = LocalizationService.ForSection("ProjProps")("Yes.Button")
            DetectImageProperties()
            If imgMountedName.Text = "<undefined>" Then
                DynaLog.LogMessage("Getting name and edition...")
                ' Determine name. Do this for both Windows 10 and 11, as this seems to occur with VHDX images (for Windows-on-ARM)
                ' Begin by determining Windows version
                Dim KeVerInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(MainForm.MountDir & "\Windows\system32\ntoskrnl.exe")
                Select Case KeVerInfo.ProductMajorPart
                    Case 10
                        ' Skip ProductMinorPart, as it is 0 on both Windows 10 and 11 (Windows 11 is 10 with a coat of paint)
                        Select Case KeVerInfo.ProductBuildPart
                            Case 10240 To 21390
                                ' Windows 10
                                imgMountedName.Text = "Windows 10"
                            Case Is >= 21996
                                ' Windows 11
                                imgMountedName.Text = "Windows 11"
                        End Select
                    Case Else
                        ' Don't know what to do in this case. Leave it as "<undefined>"
                End Select
                ' Append Windows edition to the already set text
                Select Case imgEdition.Text
                    Case "Core"
                        imgMountedName.Text = imgMountedName.Text & " Home"
                    Case "CoreN"
                        imgMountedName.Text = imgMountedName.Text & " Home N"
                    Case "CoreSingleLanguage"
                        imgMountedName.Text = imgMountedName.Text & " Home Single Language"
                    Case "Education"
                        imgMountedName.Text = imgMountedName.Text & " Education"
                    Case "EducationN"
                        imgMountedName.Text = imgMountedName.Text & " Education N"
                    Case "Professional"
                        imgMountedName.Text = imgMountedName.Text & " Pro"
                    Case "ProfessionalN"
                        imgMountedName.Text = imgMountedName.Text & " Pro N"
                    Case "ProfessionalEducation"
                        imgMountedName.Text = imgMountedName.Text & " Pro Education"
                    Case "ProfessionalEducationN"
                        imgMountedName.Text = imgMountedName.Text & " Pro Education N"
                    Case "ProfessionalWorkstation"
                        imgMountedName.Text = imgMountedName.Text & " Pro For Workstations"
                    Case "ProfessionalWorkstationN"
                        imgMountedName.Text = imgMountedName.Text & " Pro N For Workstations"
                End Select
                ' The image description may be the same as its name
                imgMountedDesc.Text = imgMountedName.Text
            End If
            Label4.Visible = False
            Panel3.Visible = False
        Else
            DynaLog.LogMessage("An image is not mounted.")
            Label19.Text = LocalizationService.ForSection("ProjProps")("No.Button")
            imgMountDir.Text = LocalizationService.ForSection("ProjProps")("ImgMount.Label")
            imgIndex.Text = LocalizationService.ForSection("ProjProps")("ImgIndex.Label")
            imgName.Text = LocalizationService.ForSection("ProjProps")("ImgName.Label")
            imgMountedStatus.Text = LocalizationService.ForSection("ProjProps")("NotAvailable.Label")
            imgVersion.Text = LocalizationService.ForSection("ProjProps")("ImgVersion.Label")
            imgMountedName.Text = LocalizationService.ForSection("ProjProps")("NotAvailable.Label")
            imgMountedDesc.Text = LocalizationService.ForSection("ProjProps")("ImgMounted.Label")
            imgSize.Text = LocalizationService.ForSection("ProjProps")("ImgSize.Label")
            imgWimBootStatus.Text = LocalizationService.ForSection("ProjProps")("ImgWIM.Label")
            imgArch.Text = LocalizationService.ForSection("ProjProps")("NotAvailable.Label")
            imgHal.Text = LocalizationService.ForSection("ProjProps")("ImgHal.Label")
            imgSPBuild.Text = LocalizationService.ForSection("ProjProps")("ImgSP.Label")
            imgSPLvl.Text = LocalizationService.ForSection("ProjProps")("NotAvailable.Label")
            imgEdition.Text = LocalizationService.ForSection("ProjProps")("ImgEdition.Label")
            imgPType.Text = LocalizationService.ForSection("ProjProps")("NotAvailable.Label")
            imgPSuite.Text = LocalizationService.ForSection("ProjProps")("ImgP.Label")
            imgSysRoot.Text = LocalizationService.ForSection("ProjProps")("ImgSys.Label")
            imgDirs.Text = LocalizationService.ForSection("ProjProps")("ImgDirs.Label")
            imgFiles.Text = LocalizationService.ForSection("ProjProps")("ImgFiles.Label")
            imgCreation.Text = LocalizationService.ForSection("ProjProps")("ImgCreation.Label")
            imgModification.Text = LocalizationService.ForSection("ProjProps")("ImgModification.Label")
            imgFormat.Text = LocalizationService.ForSection("ProjProps")("ImgFormat.Label")
            imgRW.Text = LocalizationService.ForSection("ProjProps")("ImgRW.Label")
            Panel3.Visible = True
            Label4.Visible = False
        End If
        ImageTaskHeader1.HideWindowTitle(handle)

        FfuInfoBtn.Visible = Path.GetExtension(MainForm.SourceImg).Equals(".ffu", StringComparison.OrdinalIgnoreCase)
    End Sub

    Private Sub RWRemountBtn_Click(sender As Object, e As EventArgs) Handles RWRemountBtn.Click
        DynaLog.LogMessage("Preparing to remount the Windows image with read-write permissions...")
        Visible = False
        If MainForm.CurrentImage Is Nothing Then
            MainForm.CurrentImage = MainForm.MountedImageList.FirstOrDefault(Function(mountedImage) mountedImage.ImageMountDirectory = MainForm.MountDir)
        End If
        If MainForm.CurrentImage Is Nothing Then
            Exit Sub
        End If
        MainForm.EnableWritePermissions(MainForm.CurrentImage.ImageFile, MainForm.CurrentImage.ImageIndex, MainForm.CurrentImage.ImageMountDirectory)
        Visible = True
        If Not Directory.Exists(MainForm.projPath & "\tempinfo") Then
            Directory.CreateDirectory(MainForm.projPath & "\tempinfo").Attributes = FileAttributes.Hidden
        End If
        LanguageList.Items.Clear()
        DetectImageProperties()
    End Sub

    Private Sub RemountImgBtn_Click(sender As Object, e As EventArgs) Handles RemountImgBtn.Click
        DynaLog.LogMessage("Preparing to remount the Windows image...")
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.OperationNum = 18     ' Reload image for new servicing session
        ProgressPanel.MountDir = MainForm.MountDir
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Visible = True
        If Not Directory.Exists(MainForm.projPath & "\tempinfo") Then
            Directory.CreateDirectory(MainForm.projPath & "\tempinfo").Attributes = FileAttributes.Hidden
        End If
        LanguageList.Items.Clear()
        DetectImageProperties()
    End Sub

    Private Sub RecoverButton_Click(sender As Object, e As EventArgs) Handles RecoverButton.Click
        Visible = False
        ImgCleanup.ComboBox1.SelectedIndex = 6
        ImgCleanup.ShowDialog(MainForm)
        Visible = True
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        ImgMount.ShowDialog(MainForm)
    End Sub

    ''' <summary>
    ''' Detects the feature update the mounted image contains
    ''' </summary>
    ''' <param name="SysVer">The image version detected by the DISM API</param>
    ''' <remarks>Feature updates are only applicable to Windows 10 and 11. If this function detects an earlier version, it will leave</remarks>
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
        imgVersion.Text &= LocalizationService.ForSection("ProjectProps.FeatureUpdate").Format("FeatureUpdate.Label", FeatUpd)
    End Sub

    Private Sub Label37_MouseHover(sender As Object, e As EventArgs) Handles Label37.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("ProjProps.Tooltip")("Hardware.Abstraction.Label"))
    End Sub

    Private Sub FfuInfoBtn_Click(sender As Object, e As EventArgs) Handles FfuInfoBtn.Click
        FfuInfoDialog.MountedFfuInformation = MainForm.CurrentImage.FFUInfo
        FfuInfoDialog.ShowDialog(Me)
    End Sub
End Class
