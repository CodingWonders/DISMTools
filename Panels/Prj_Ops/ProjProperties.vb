Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports Microsoft.Dism

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
    ''' Detects the properties of the image using the DISM API
    ''' </summary>
    ''' <remarks></remarks>
    Sub DetectImageProperties()
        DismApi.Initialize(DismLogLevel.LogErrors)
        ' Detect mounted images to find the loaded one
        Dim MountedImgs As DismMountedImageInfoCollection = DismApi.GetMountedImages()
        For Each MImg As DismMountedImageInfo In MountedImgs
            Try
                For x = 0 To Array.LastIndexOf(MainForm.MountedImageImgFiles, MainForm.MountedImageImgFiles.Last)
                    If MainForm.MountedImageMountDirs(x) = MainForm.MountDir Then
                        Debug.WriteLine("- Image file : " & MainForm.MountedImageImgFiles(x))
                        Debug.WriteLine("- Image index : " & MainForm.MountedImageImgIndexes(x))
                        Debug.WriteLine("- Mount directory : " & MainForm.MountedImageMountDirs(x))
                        Debug.WriteLine("- Mount status : " & MainForm.MountedImageImgStatuses(x) & If(MainForm.MountedImageImgStatuses(x) = 0, " (OK)", If(MainForm.MountedImageImgStatuses(x) = 1, " (Orphaned)", " (Invalid)")))
                        Debug.WriteLine("- Mount mode : " & MainForm.MountedImageMountedReWr(x) & If(MainForm.MountedImageMountedReWr(x) = 0, " (Write permissions enabled)", "(Write permissions disabled)"))
                        imgName.Text = MainForm.MountedImageImgFiles(x)
                        imgIndex.Text = MainForm.MountedImageImgIndexes(x)
                        imgMountDir.Text = MainForm.MountedImageMountDirs(x)
                        Select Case MainForm.MountedImageImgStatuses(x)
                            Case 0
                                imgMountedStatus.Text = "OK"
                                RecoverButton.Visible = False
                                RemountImgBtn.Visible = False
                            Case 1
                                imgMountedStatus.Text = "Needs Remount"
                                RecoverButton.Visible = False
                                RemountImgBtn.Visible = True
                            Case 2
                                imgMountedStatus.Text = "Invalid"
                                RecoverButton.Visible = True
                                RemountImgBtn.Visible = False
                        End Select
                        Dim infoCollection As DismImageInfoCollection = DismApi.GetImageInfo(MainForm.MountedImageImgFiles(x))
                        For Each info As DismImageInfo In infoCollection
                            imgVersion.Text = info.ProductVersion.ToString()
                            imgMountedName.Text = info.ImageName
                            imgMountedDesc.Text = info.ImageDescription
                            imgSize.Text = info.ImageSize
                            imgArch.Text = info.Architecture
                            imgHal.Text = info.Hal
                            imgSPBuild.Text = info.ProductVersion.Revision
                            imgSPLvl.Text = info.SpLevel
                            imgEdition.Text = info.EditionId
                            imgPType.Text = info.ProductType
                            imgPSuite.Text = info.ProductSuite
                            imgSysRoot.Text = info.SystemRoot
                            imgLangText.Text = info.Languages.ToString() & CrLf & _
                                "Default language: " & info.DefaultLanguage.ToString()
                            imgFormat.Text = Path.GetExtension(MainForm.MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper() & " file"
                            imgRW.Text = If(MainForm.MountedImageMountedReWr(x) = 0, "Yes", "No")
                            If MainForm.MountedImageMountedReWr(x) = 0 Then
                                RWRemountBtn.Visible = False
                            Else
                                RWRemountBtn.Visible = True
                            End If
                            Exit For
                        Next
                    End If
                    Exit For
                Next
            Catch ex As Exception
                Exit Try
            End Try
        Next
        DismApi.Shutdown()
    End Sub

    Private Sub ProjProperties_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set program colors
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TabPage1.BackColor = Color.FromArgb(31, 31, 31)
            TabPage2.BackColor = Color.FromArgb(31, 31, 31)
            imgLangText.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TabPage1.BackColor = Color.FromArgb(238, 238, 242)
            TabPage2.BackColor = Color.FromArgb(238, 238, 242)
            imgLangText.BackColor = Color.FromArgb(238, 238, 242)
        End If
        imgLangText.ForeColor = ForeColor
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
        imgLangText.Text = ""
        Visible = True
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
        Label4.Visible = True
        Label9.Text = MainForm.projName.Text
        Label10.Text = MainForm.projPath
        Label11.Text = File.GetCreationTime(MainForm.projPath)
        Dim rtb As New RichTextBox With {
            .Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\" & MainForm.projName.Text & ".dtproj")
        }
        If rtb.Lines(6).StartsWith("ProjGuid") Then
            Label12.Text = rtb.Lines(6).Replace("ProjGuid=", "").Trim()
        End If
        If MainForm.IsImageMounted Then
            If Debugger.IsAttached Then DetectImageProperties()
            Label19.Text = "Yes"
            If Not Debugger.IsAttached Then
                Try     ' Try getting image properties
                    If Not Directory.Exists(MainForm.projPath & "\tempinfo") Then
                        Directory.CreateDirectory(MainForm.projPath & "\tempinfo").Attributes = FileAttributes.Hidden
                    End If
                    Select Case DismVersionChecker.ProductMajorPart
                        Case 6
                            Select Case DismVersionChecker.ProductMinorPart
                                Case 1
                                    File.WriteAllText(".\bin\exthelpers\imginfo.bat",
                                                      "@echo off" & CrLf &
                                                      "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Mount Dir" & Quote & " /b > " & MainForm.projPath & "\tempinfo\mountdir" & CrLf &
                                                      "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Image File" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgfile" & CrLf &
                                                      "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Image Index" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgindex" & CrLf &
                                                      "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Mounted Read/Write" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgrw" & CrLf &
                                                      "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmountedstatus" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmountedname" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Description" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmounteddesc" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Size" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsize" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgwimboot" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Architecture" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgarch" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Hal" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imghal" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ServicePack Build" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgspbuild" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ServicePack Level" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsplevel" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Edition" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgedition" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Installation" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imginst" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ProductType" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgptype" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ProductSuite" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgpsuite" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "System Root" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsysroot" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Directories" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgdirs" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Files" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgfiles" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Created" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgcreation" & CrLf &
                                                      "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Modified" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmodification" & CrLf &
                                                      "dism /English /image=" & MainForm.MountDir & " /get-intl | findstr /c:" & Quote & "Installed language(s):" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imglangs", ASCII)
                                Case Is >= 2
                                    File.WriteAllText(".\bin\exthelpers\imginfo.bat",
                                                      "@echo off" & CrLf &
                                                      "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mount Dir" & Quote & " /b > " & MainForm.projPath & "\tempinfo\mountdir" & CrLf &
                                                      "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image File" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgfile" & CrLf &
                                                      "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image Index" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgindex" & CrLf &
                                                      "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mounted Read/Write" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgrw" & CrLf &
                                                      "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmountedstatus" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmountedname" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Description" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmounteddesc" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Size" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsize" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgwimboot" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Architecture" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgarch" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Hal" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imghal" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ServicePack Build" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgspbuild" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ServicePack Level" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsplevel" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Edition" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgedition" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Installation" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imginst" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ProductType" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgptype" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ProductSuite" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgpsuite" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "System Root" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsysroot" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Directories" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgdirs" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Files" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgfiles" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Created" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgcreation" & CrLf &
                                                      "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Modified" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmodification" & CrLf &
                                                      "dism /English /image=" & MainForm.MountDir & " /get-intl | findstr /c:" & Quote & "Installed language(s):" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imglangs", ASCII)
                            End Select
                        Case 10
                            File.WriteAllText(".\bin\exthelpers\imginfo.bat",
                                              "@echo off" & CrLf &
                                              "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mount Dir" & Quote & " /b > " & MainForm.projPath & "\tempinfo\mountdir" & CrLf &
                                              "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image File" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgfile" & CrLf &
                                              "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image Index" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgindex" & CrLf &
                                              "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mounted Read/Write" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgrw" & CrLf &
                                              "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmountedstatus" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmountedname" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Description" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmounteddesc" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Size" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsize" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgwimboot" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Architecture" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgarch" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Hal" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imghal" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ServicePack Build" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgspbuild" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ServicePack Level" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsplevel" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Edition" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgedition" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Installation" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imginst" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ProductType" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgptype" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ProductSuite" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgpsuite" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "System Root" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsysroot" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Directories" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgdirs" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Files" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgfiles" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Created" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgcreation" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Modified" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmodification" & CrLf &
                                              "dism /English /image=" & MainForm.MountDir & " /get-intl | findstr /c:" & Quote & "Installed language(s):" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imglangs", ASCII)
                    End Select

                    If Debugger.IsAttached Then
                        Process.Start("\Windows\system32\notepad.exe", ".\bin\exthelpers\imginfo.bat").WaitForExit()
                    End If
                    Process.Start(".\bin\exthelpers\imginfo.bat").WaitForExit()
                    imgName.Text = MainForm.SourceImg
                    imgIndex.Text = MainForm.ImgIndex
                    imgMountDir.Text = MainForm.MountDir
                    imgMountedStatus.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgmountedstatus", ASCII).Replace("Status : ", "").Trim()
                    If imgMountedStatus.Text = "Invalid" Then
                        RecoverButton.Visible = True
                    ElseIf imgMountedStatus.Text = "Needs Remount" Then
                        RemountImgBtn.Visible = True
                    End If
                    Try
                        Dim KeVerInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(MainForm.MountDir & "\Windows\system32\ntoskrnl.exe")
                        Dim KeVerStr As String = KeVerInfo.ProductVersion
                        imgVersion.Text = KeVerStr
                        imgMountedName.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgmountedname", ASCII).Replace("Name : ", "").Trim()
                        imgMountedDesc.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgmounteddesc", ASCII).Replace("Description : ", "").Trim()
                        Dim ImgSizeDbl As Double
                        ImgSizeDbl = CDbl(My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgsize", ASCII).Replace("Size : ", "").Trim().Replace(" bytes", "").Trim().Replace(".", "").Trim()) / (1024 ^ 3)
                        ImgSizeStr = Math.Round(ImgSizeDbl, 2)
                        imgSize.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgsize", ASCII).Replace("Size : ", "").Trim() & " (~" & ImgSizeStr & " GB)"
                        imgWimBootStatus.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgwimboot", ASCII).Replace("WIM Bootable : ", "").Trim()
                        imgArch.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgarch", ASCII).Replace("Architecture : ", "").Trim()
                        imgHal.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imghal", ASCII).Replace("Hal : ", "").Trim()
                        imgSPBuild.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgspbuild", ASCII).Replace("ServicePack Build : ", "").Trim()
                        imgSPLvl.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgsplevel", ASCII).Replace("ServicePack Level : ", "").Trim()
                        imgEdition.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgedition", ASCII).Replace("Edition : ", "").Trim()
                        imgPType.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgptype", ASCII).Replace("ProductType : ", "").Trim()
                        imgPSuite.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgpsuite", ASCII).Replace("ProductSuite : ", "").Trim()
                        imgSysRoot.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgsysroot", ASCII).Replace("System Root : ", "").Trim()
                        imgDirs.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgdirs", ASCII).Replace("Directories : ", "").Trim()
                        imgFiles.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgfiles", ASCII).Replace("Files : ", "").Trim()
                        imgCreation.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgcreation", ASCII).Replace("Created : ", "").Trim()
                        imgModification.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgmodification", ASCII).Replace("Modified : ", "").Trim()
                        imgLangText.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imglangs", ASCII).Replace("Installed language(s): ", "").Trim()
                        imgFormat.Text = Path.GetExtension(MainForm.SourceImg).Replace(".", "").Trim().ToUpper() & " file"
                        imgRW.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgrw", ASCII).Replace("Mounted Read/Write : ", "").Trim()
                        If imgRW.Text = "Yes" Then
                            RWRemountBtn.Visible = False
                        ElseIf imgRW.Text = "No" Then
                            RWRemountBtn.Visible = True
                        End If
                        For Each foundFile In My.Computer.FileSystem.GetFiles(MainForm.projPath & "\tempinfo", FileIO.SearchOption.SearchTopLevelOnly)
                            File.Delete(foundFile)
                        Next
                        Directory.Delete(MainForm.projPath & "\tempinfo")
                        File.Delete(".\bin\exthelpers\imginfo.bat")
                    Catch ex As Exception

                    End Try
                    MainForm.imgVersion = imgVersion.Text
                    MainForm.imgMountedStatus = imgMountedStatus.Text
                    MainForm.imgMountedName = imgMountedName.Text
                    MainForm.imgMountedDesc = imgMountedDesc.Text
                    MainForm.imgWimBootStatus = imgWimBootStatus.Text
                    MainForm.imgArch = imgArch.Text
                    MainForm.imgHal = imgHal.Text
                    MainForm.imgSPBuild = imgSPBuild.Text
                    MainForm.imgSPLvl = imgSPLvl.Text
                    MainForm.imgEdition = imgEdition.Text
                    MainForm.imgPType = imgPType.Text
                    MainForm.imgPSuite = imgPSuite.Text
                    MainForm.imgSysRoot = imgSysRoot.Text
                    MainForm.imgDirs = CInt(imgDirs.Text)
                    MainForm.imgFiles = CInt(imgFiles.Text)
                    MainForm.imgCreation = imgCreation.Text
                    MainForm.CreationTime = MainForm.imgCreation.Replace(" - ", " ")
                    MainForm.imgModification = imgModification.Text
                    MainForm.ModifyTime = MainForm.imgModification.Replace(" - ", " ")
                    MainForm.imgLangs = imgLangText.Text
                    MainForm.imgRW = imgRW.Text
                    DetectFeatureUpdate(MainForm.MountDir & "\Windows\system32\ntoskrnl.exe")
                Catch ex As Exception

                End Try
            End If

            If imgMountedName.Text = "<undefined>" Then
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
                ' Set MainForm variables
                MainForm.imgMountedName = imgMountedName.Text
                MainForm.imgMountedDesc = imgMountedDesc.Text
            End If
            Label4.Visible = False
        Else
            Label19.Text = "No"
            imgMountDir.Text = "Not available"
            imgIndex.Text = "Not available"
            imgName.Text = "Not available"
            imgMountedStatus.Text = "Not available"
            imgVersion.Text = "Not available"
            imgMountedName.Text = "Not available"
            imgMountedDesc.Text = "Not available"
            imgSize.Text = "Not available"
            imgWimBootStatus.Text = "Not available"
            imgArch.Text = "Not available"
            imgHal.Text = "Not available"
            imgSPBuild.Text = "Not available"
            imgSPLvl.Text = "Not available"
            imgEdition.Text = "Not available"
            imgPType.Text = "Not available"
            imgPSuite.Text = "Not available"
            imgSysRoot.Text = "Not available"
            imgDirs.Text = "Not available"
            imgFiles.Text = "Not available"
            imgCreation.Text = "Not available"
            imgModification.Text = "Not available"
            imgFormat.Text = "Not available"
            imgRW.Text = "Not available"
            Panel3.Visible = True
            Label4.Visible = False
        End If
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        Label1.Text = TabControl1.SelectedTab.Text & " properties"
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
        Else
            Text = Label1.Text
        End If
    End Sub

    Private Sub RWRemountBtn_Click(sender As Object, e As EventArgs) Handles RWRemountBtn.Click

    End Sub

    Private Sub RemountImgBtn_Click(sender As Object, e As EventArgs) Handles RemountImgBtn.Click
        ProgressPanel.OperationNum = 18     ' Reload image for new servicing session
        ProgressPanel.MountDir = MainForm.MountDir
        ProgressPanel.ShowDialog()
    End Sub

    Private Sub RecoverButton_Click(sender As Object, e As EventArgs) Handles RecoverButton.Click

    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        ImgMount.ShowDialog(MainForm)
    End Sub

    ''' <summary>
    ''' Detects the feature update the mounted image contains
    ''' </summary>
    ''' <param name="KeExe">The path of the "ntoskrnl.exe" file</param>
    ''' <remarks>Feature updates are only applicable to Windows 10 and 11. If this function detects an earlier version, it will leave</remarks>
    Sub DetectFeatureUpdate(KeExe As String)
        Try
            Dim KeVerInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(KeExe)
            Dim FeatUpd As String = ""
            Select Case KeVerInfo.ProductMajorPart
                Case 10
                    Select Case KeVerInfo.ProductBuildPart
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
                            If KeVerInfo.ProductPrivatePart >= 10000 Then
                                FeatUpd = "1909 (Vanadium)"
                            Else
                                FeatUpd = "1903 (Titanium)"
                            End If
                        Case Is = 18363
                            FeatUpd = "1909 (Vanadium)"
                        Case 18826 To 19041
                            FeatUpd = "2004 (Vibranium"
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
                        Case 22350 To 22623
                            FeatUpd = "22H2 (Nickel)"
                        Case 25057 To 25238
                            FeatUpd = "22H2 (Copper)"
                        Case 25240 To 26000     ' 26000 is a relative number. We still don't know Zinc's final build
                            FeatUpd = "23H2 (Zinc)"
                    End Select
                Case Else
                    Exit Sub
            End Select
            imgVersion.Text &= CrLf & "(feature update: " & FeatUpd & ")"
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub
End Class
