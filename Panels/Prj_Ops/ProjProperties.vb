Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding

Public Class ProjProperties

    Public GuidStr As String
    Public PublicImgSize As String
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

    Private Sub ProjProperties_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        ' Support for reading project GUID will be added later
        If MainForm.IsImageMounted Then
            Label19.Text = "Yes"
            Try     ' Try getting image properties
                If Not Directory.Exists(MainForm.projPath & "\tempinfo") Then
                    Directory.CreateDirectory(MainForm.projPath & "\tempinfo").Attributes = FileAttributes.Hidden
                End If
                Select Case DismVersionChecker.ProductMajorPart
                    Case 6
                        Select Case DismVersionChecker.ProductMinorPart
                            Case 1
                                File.WriteAllText(".\bin\exthelpers\imginfo.bat", _
                                                  "@echo off" & CrLf & _
                                                  "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Mount Dir" & Quote & " /b > " & MainForm.projPath & "\tempinfo\mountdir" & CrLf & _
                                                  "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Image File" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgfile" & CrLf & _
                                                  "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Image Index" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgindex" & CrLf & _
                                                  "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Mounted Read/Write" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgrw" & CrLf & _
                                                  "dism /English /get-mountedwiminfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmountedstatus" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmountedname" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Description" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmounteddesc" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Size" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsize" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgwimboot" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Architecture" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgarch" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Hal" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imghal" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ServicePack Build" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgspbuild" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ServicePack Level" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsplevel" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Edition" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgedition" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Installation" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imginst" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ProductType" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgptype" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ProductSuite" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgpsuite" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "System Root" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsysroot" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Directories" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgdirs" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Files" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgfiles" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Created" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgcreation" & CrLf & _
                                                  "dism /English /get-wiminfo /wimfile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Modified" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmodification" & CrLf & _
                                                  "dism /English /image=" & MainForm.MountDir & " /get-intl | findstr /c:" & Quote & "Installed language(s):" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imglangs", ASCII)
                            Case Is >= 2
                                File.WriteAllText(".\bin\exthelpers\imginfo.bat", _
                                                  "@echo off" & CrLf & _
                                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mount Dir" & Quote & " /b > " & MainForm.projPath & "\tempinfo\mountdir" & CrLf & _
                                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image File" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgfile" & CrLf & _
                                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image Index" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgindex" & CrLf & _
                                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mounted Read/Write" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgrw" & CrLf & _
                                                  "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmountedstatus" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmountedname" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Description" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmounteddesc" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Size" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsize" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgwimboot" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Architecture" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgarch" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Hal" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imghal" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ServicePack Build" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgspbuild" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ServicePack Level" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsplevel" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Edition" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgedition" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Installation" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imginst" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ProductType" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgptype" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ProductSuite" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgpsuite" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "System Root" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsysroot" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Directories" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgdirs" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Files" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgfiles" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Created" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgcreation" & CrLf & _
                                                  "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Modified" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmodification" & CrLf & _
                                                  "dism /English /image=" & MainForm.MountDir & " /get-intl | findstr /c:" & Quote & "Installed language(s):" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imglangs", ASCII)
                        End Select
                    Case 10
                        File.WriteAllText(".\bin\exthelpers\imginfo.bat", _
                                          "@echo off" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mount Dir" & Quote & " /b > " & MainForm.projPath & "\tempinfo\mountdir" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image File" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgfile" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Image Index" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgindex" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Mounted Read/Write" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgrw" & CrLf & _
                                          "dism /English /get-mountedimageinfo | findstr /c:" & Quote & "Status" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmountedstatus" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Name" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmountedname" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Description" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmounteddesc" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Size" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsize" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgwimboot" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Architecture" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgarch" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Hal" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imghal" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ServicePack Build" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgspbuild" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ServicePack Level" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsplevel" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Edition" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgedition" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Installation" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imginst" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ProductType" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgptype" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "ProductSuite" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgpsuite" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "System Root" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgsysroot" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Directories" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgdirs" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Files" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgfiles" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Created" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgcreation" & CrLf & _
                                          "dism /English /get-imageinfo /imagefile=" & MainForm.SourceImg & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "Modified" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgmodification" & CrLf & _
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
            Catch ex As Exception

            End Try
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
End Class
