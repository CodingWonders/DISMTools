Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports Microsoft.Dism

Public Class ProjProperties

    Dim ImgSizeStr As String
    Dim DismVersionChecker As FileVersionInfo
    Dim HalHelper As New ToolTip()

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
        If MainForm.MountedImageDetectorBW.IsBusy Then MainForm.MountedImageDetectorBW.CancelAsync()
        DismApi.Initialize(DismLogLevel.LogErrors)
        ' Detect mounted images to find the loaded one
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
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
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
                                Case "ESN"
                                    Select Case MainForm.MountedImageImgStatuses(x)
                                        Case 0
                                            imgMountedStatus.Text = "Correcto"
                                            RecoverButton.Visible = False
                                            RemountImgBtn.Visible = False
                                        Case 1
                                            imgMountedStatus.Text = "Necesita recarga"
                                            RecoverButton.Visible = False
                                            RemountImgBtn.Visible = True
                                        Case 2
                                            imgMountedStatus.Text = "Inválido"
                                            RecoverButton.Visible = True
                                            RemountImgBtn.Visible = False
                                    End Select
                            End Select
                        Case 1
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
                        Case 2
                            Select Case MainForm.MountedImageImgStatuses(x)
                                Case 0
                                    imgMountedStatus.Text = "Correcto"
                                    RecoverButton.Visible = False
                                    RemountImgBtn.Visible = False
                                Case 1
                                    imgMountedStatus.Text = "Necesita recarga"
                                    RecoverButton.Visible = False
                                    RemountImgBtn.Visible = True
                                Case 2
                                    imgMountedStatus.Text = "Inválido"
                                    RecoverButton.Visible = True
                                    RemountImgBtn.Visible = False
                            End Select
                    End Select

                    Dim infoCollection As DismImageInfoCollection = DismApi.GetImageInfo(MainForm.MountedImageImgFiles(x))
                    For Each info As DismImageInfo In infoCollection
                        If info.ImageIndex = MainForm.MountedImageImgIndexes(x) Then
                            imgVersion.Text = info.ProductVersion.ToString()
                            DetectFeatureUpdate(info.ProductVersion)
                            imgMountedName.Text = info.ImageName
                            imgMountedDesc.Text = info.ImageDescription
                            imgSize.Text = info.ImageSize.ToString("N0") & " bytes (~" & Math.Round(info.ImageSize / (1024 ^ 3), 2) & " GB)"
                            If info.Architecture = DismProcessorArchitecture.None Then
                                imgArch.Text = "Unknown"
                            ElseIf info.Architecture = DismProcessorArchitecture.Neutral Then
                                imgArch.Text = "Neutral"
                            ElseIf info.Architecture = DismProcessorArchitecture.Intel Then
                                imgArch.Text = "x86"
                            ElseIf info.Architecture = DismProcessorArchitecture.IA64 Then
                                ' I'm not sure what systems run Itanium versions of Windows, but still
                                imgArch.Text = "Itanium (64-bit)"
                            ElseIf info.Architecture = DismProcessorArchitecture.ARM64 Then
                                imgArch.Text = "ARM64"
                            ElseIf info.Architecture = DismProcessorArchitecture.ARM Then
                                ' This must be the case on Windows RT images
                                imgArch.Text = "ARM"
                            ElseIf info.Architecture = DismProcessorArchitecture.AMD64 Then
                                imgArch.Text = "x64"
                            End If
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENG"
                                            imgHal.Text = If(Not info.Hal = "", info.Hal, "Undefined by the image")
                                        Case "ESN"
                                            imgHal.Text = If(Not info.Hal = "", info.Hal, "No definida por la imagen")
                                    End Select
                                Case 1
                                    imgHal.Text = If(Not info.Hal = "", info.Hal, "Undefined by the image")
                                Case 2
                                    imgHal.Text = If(Not info.Hal = "", info.Hal, "No definida por la imagen")
                            End Select
                            imgSPBuild.Text = info.ProductVersion.Revision
                            imgSPLvl.Text = info.SpLevel
                            imgEdition.Text = info.EditionId
                            imgPType.Text = info.ProductType
                            imgPSuite.Text = info.ProductSuite
                            imgSysRoot.Text = info.SystemRoot
                            imgLangText.Clear()
                            For Each language In info.Languages
                                If imgLangText.Text = "" Then
                                    Select Case MainForm.Language
                                        Case 0
                                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                                Case "ENG"
                                                    imgLangText.Text = language.Name & If(info.DefaultLanguage.Name = language.Name, " (default)", "") & ", "
                                                Case "ESN"
                                                    imgLangText.Text = language.Name & If(info.DefaultLanguage.Name = language.Name, " (predeterminado)", "") & ", "
                                            End Select
                                        Case 1
                                            imgLangText.Text = language.Name & If(info.DefaultLanguage.Name = language.Name, " (default)", "") & ", "
                                        Case 2
                                            imgLangText.Text = language.Name & If(info.DefaultLanguage.Name = language.Name, " (predeterminado)", "") & ", "
                                    End Select
                                Else
                                    Select Case MainForm.Language
                                        Case 0
                                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                                Case "ENG"
                                                    imgLangText.AppendText(language.Name & If(info.DefaultLanguage.Name = language.Name, " (default)", "") & ", ")
                                                Case "ESN"
                                                    imgLangText.AppendText(language.Name & If(info.DefaultLanguage.Name = language.Name, " (predeterminado)", "") & ", ")
                                            End Select
                                        Case 1
                                            imgLangText.AppendText(language.Name & If(info.DefaultLanguage.Name = language.Name, " (default)", "") & ", ")
                                        Case 2
                                            imgLangText.AppendText(language.Name & If(info.DefaultLanguage.Name = language.Name, " (predeterminado)", "") & ", ")
                                    End Select
                                End If
                            Next
                            Dim langarr() As Char = imgLangText.Text.ToCharArray()
                            langarr(langarr.Count - 2) = ""
                            imgLangText.Text = New String(langarr)
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENG"
                                            imgFormat.Text = Path.GetExtension(MainForm.MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper() & " file"
                                        Case "ESN"
                                            imgFormat.Text = "Archivo " & Path.GetExtension(MainForm.MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper()
                                    End Select
                                Case 1
                                    imgFormat.Text = Path.GetExtension(MainForm.MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper() & " file"
                                Case 2
                                    imgFormat.Text = "Archivo " & Path.GetExtension(MainForm.MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper()
                            End Select
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENG"
                                            imgRW.Text = If(MainForm.MountedImageMountedReWr(x) = 0, "Yes", "No")
                                        Case "ESN"
                                            imgRW.Text = If(MainForm.MountedImageMountedReWr(x) = 0, "Sí", "No")
                                    End Select
                                Case 1
                                    imgRW.Text = If(MainForm.MountedImageMountedReWr(x) = 0, "Yes", "No")
                                Case 2
                                    imgRW.Text = If(MainForm.MountedImageMountedReWr(x) = 0, "Sí", "No")
                            End Select
                            If MainForm.MountedImageMountedReWr(x) = 0 Then
                                RWRemountBtn.Visible = False
                            Else
                                RWRemountBtn.Visible = True
                            End If
                            imgDirs.Text = info.CustomizedInfo.DirectoryCount
                            imgFiles.Text = info.CustomizedInfo.FileCount
                            imgCreation.Text = info.CustomizedInfo.CreatedTime
                            imgModification.Text = info.CustomizedInfo.ModifiedTime
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
            Exit Try
        End Try
        DismApi.Shutdown()
        ' The DISM API part is over. Switch to regular DISM.exe mode for missing details
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
                                              "dism /English /get-wiminfo /wimfile=" & Quote & MainForm.SourceImg & Quote & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgwimboot", ASCII)
                        Case Is >= 2
                            File.WriteAllText(".\bin\exthelpers\imginfo.bat",
                                              "@echo off" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & Quote & MainForm.SourceImg & Quote & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgwimboot", ASCII)
                    End Select
                Case 10
                    File.WriteAllText(".\bin\exthelpers\imginfo.bat",
                                      "@echo off" & CrLf &
                                      "dism /English /get-imageinfo /imagefile=" & Quote & MainForm.SourceImg & Quote & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & MainForm.projPath & "\tempinfo\imgwimboot", ASCII)
            End Select
            If Debugger.IsAttached Then
                Process.Start("\Windows\system32\notepad.exe", ".\bin\exthelpers\imginfo.bat").WaitForExit()
            End If
            Using WIMBootProc As New Process()
                WIMBootProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
                WIMBootProc.StartInfo.Arguments = "/c " & Quote & Directory.GetCurrentDirectory() & "\bin\exthelpers\imginfo.bat" & Quote
                WIMBootProc.StartInfo.CreateNoWindow = True
                WIMBootProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                WIMBootProc.Start()
                Do Until WIMBootProc.HasExited
                    If WIMBootProc.HasExited Then Exit Do
                Loop
            End Using
            Try
                imgWimBootStatus.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgwimboot", ASCII).Replace("WIM Bootable : ", "").Trim()
                If Not MainForm.ImgBW.IsBusy Then
                    For Each foundFile In My.Computer.FileSystem.GetFiles(MainForm.projPath & "\tempinfo", FileIO.SearchOption.SearchTopLevelOnly)
                        File.Delete(foundFile)
                    Next
                    Directory.Delete(MainForm.projPath & "\tempinfo")
                End If
                File.Delete(".\bin\exthelpers\imginfo.bat")
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ProjProperties_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Label2.Text = "View project properties, such as name or location"
                        Label3.Text = "View mounted image properties, such as name, description, or index"
                        Label4.Text = "Getting project and image information. Please wait..."
                        Label5.Text = "Name:"
                        Label6.Text = "Location:"
                        Label7.Text = "Creation date:"
                        Label8.Text = "Project GUID:"
                        Label13.Text = "Mount directory:"
                        Label14.Text = "Image index:"
                        Label15.Text = "Image file:"
                        Label20.Text = "Image present on project?"
                        Label22.Text = "Image status:"
                        Label25.Text = "Version:"
                        Label27.Text = "Name:"
                        Label29.Text = "Description:"
                        Label31.Text = "Size:"
                        Label33.Text = "Supports WIMBoot?"
                        Label35.Text = "Architecture:"
                        Label39.Text = "Service Pack build:"
                        Label41.Text = "Service Pack level:"
                        Label43.Text = "Edition:"
                        Label45.Text = "Product type:"
                        Label47.Text = "Product suite:"
                        Label49.Text = "System root directory:"
                        Label51.Text = "Directory count:"
                        Label53.Text = "File count:"
                        Label55.Text = "Creation date:"
                        Label57.Text = "Modification date:"
                        Label58.Text = "Installed languages:"
                        Label60.Text = "File format:"
                        Label62.Text = "Image R/W permissions:"
                        TabPage1.Text = "Project"
                        TabPage2.Text = "Image"
                        RecoverButton.Text = "Recover"
                        RemountImgBtn.Text = "Reload"
                        RWRemountBtn.Text = "Remount with write permissions"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        LinkLabel2.Text = "Many properties cannot be seen because an image has not yet been mounted. Once you mount it, detailed information will be shown here. Click here to mount an image"
                    Case "ESN"
                        Label2.Text = "Ver propiedades del proyecto, como nombre y ubicación"
                        Label3.Text = "Ver propiedades de la imagen montada, como nombre, descripción, o índice"
                        Label4.Text = "Obteniendo información del proyecto y la imagen. Espere..."
                        Label5.Text = "Nombre:"
                        Label6.Text = "Ubicación:"
                        Label7.Text = "Fecha de creación:"
                        Label8.Text = "GUID del proyecto:"
                        Label13.Text = "Directorio de montaje:"
                        Label14.Text = "Índice de imagen:"
                        Label15.Text = "Archivo de imagen:"
                        Label20.Text = "¿La imagen está presente en el proyecto?"
                        Label22.Text = "Estado de imagen:"
                        Label25.Text = "Versión:"
                        Label27.Text = "Nombre:"
                        Label29.Text = "Descripción:"
                        Label31.Text = "Tamaño:"
                        Label33.Text = "¿Soporta WIMBoot?"
                        Label35.Text = "Arquitectura:"
                        Label39.Text = "Compilación de Service Pack:"
                        Label41.Text = "Nivel de Service Pack:"
                        Label43.Text = "Edición:"
                        Label45.Text = "Tipo de producto:"
                        Label47.Text = "Suite de producto:"
                        Label49.Text = "Directorio de raíz del sistema:"
                        Label51.Text = "Número de directorios:"
                        Label53.Text = "Número de archivos:"
                        Label55.Text = "Fecha de creación:"
                        Label57.Text = "Fecha de modificación:"
                        Label58.Text = "Idiomas instalados:"
                        Label60.Text = "Formato de archivo:"
                        Label62.Text = "Permisos de L/E de imagen:"
                        TabPage1.Text = "Proyecto"
                        TabPage2.Text = "Imagen"
                        RecoverButton.Text = "Recuperar"
                        RemountImgBtn.Text = "Recargar"
                        RWRemountBtn.Text = "Recargar con permisos de escritura"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        LinkLabel2.Text = "Las propiedades no pueden ser obtenidas porque aún no se ha montado una imagen. Cuando lo haga, información detallada aparecerá aquí. Haga clic aquí para montar una imagen"
                End Select
            Case 1
                Label2.Text = "View project properties, such as name or location"
                Label3.Text = "View mounted image properties, such as name, description, or index"
                Label4.Text = "Getting project and image information. Please wait..."
                Label5.Text = "Name:"
                Label6.Text = "Location:"
                Label7.Text = "Creation time and date:"
                Label8.Text = "Project GUID:"
                Label13.Text = "Mount directory:"
                Label14.Text = "Image index:"
                Label15.Text = "Image file:"
                Label20.Text = "Image present on project?"
                Label22.Text = "Image status:"
                Label25.Text = "Version:"
                Label27.Text = "Name:"
                Label29.Text = "Description:"
                Label31.Text = "Size:"
                Label33.Text = "Supports WIMBoot?"
                Label35.Text = "Architecture:"
                Label39.Text = "Service Pack build:"
                Label41.Text = "Service Pack level:"
                Label43.Text = "Edition:"
                Label45.Text = "Product type:"
                Label47.Text = "Product suite:"
                Label49.Text = "System root directory:"
                Label51.Text = "Directory count:"
                Label53.Text = "File count:"
                Label55.Text = "Creation date:"
                Label57.Text = "Modification date:"
                Label58.Text = "Installed languages:"
                Label60.Text = "File format:"
                Label62.Text = "Image R/W permissions:"
                TabPage1.Text = "Project"
                TabPage2.Text = "Image"
                RecoverButton.Text = "Recover"
                RemountImgBtn.Text = "Reload"
                RWRemountBtn.Text = "Remount with write permissions"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                LinkLabel2.Text = "Many properties cannot be seen because an image has not yet been mounted. Once you mount it, detailed information will be shown here. Click here to mount an image"
            Case 2
                Label2.Text = "Ver propiedades del proyecto, como nombre y ubicación"
                Label3.Text = "Ver propiedades de la imagen montada, como nombre, descripción, o índice"
                Label4.Text = "Obteniendo información del proyecto y la imagen. Espere..."
                Label5.Text = "Nombre:"
                Label6.Text = "Ubicación:"
                Label7.Text = "Fecha de creación:"
                Label8.Text = "GUID del proyecto:"
                Label13.Text = "Directorio de montaje:"
                Label14.Text = "Índice de imagen:"
                Label15.Text = "Archivo de imagen:"
                Label20.Text = "¿La imagen está presente en el proyecto?"
                Label22.Text = "Estado de imagen:"
                Label25.Text = "Versión:"
                Label27.Text = "Nombre:"
                Label29.Text = "Descripción:"
                Label31.Text = "Tamaño:"
                Label33.Text = "¿Soporta WIMBoot?"
                Label35.Text = "Arquitectura:"
                Label39.Text = "Compilación de Service Pack:"
                Label41.Text = "Nivel de Service Pack:"
                Label43.Text = "Edición:"
                Label45.Text = "Tipo de producto:"
                Label47.Text = "Suite de producto:"
                Label49.Text = "Directorio de raíz del sistema:"
                Label51.Text = "Número de directorios:"
                Label53.Text = "Número de archivos:"
                Label55.Text = "Fecha de creación:"
                Label57.Text = "Fecha de modificación:"
                Label58.Text = "Idiomas instalados:"
                Label60.Text = "Formato de archivo:"
                Label62.Text = "Permisos de L/E de imagen:"
                TabPage1.Text = "Proyecto"
                TabPage2.Text = "Imagen"
                RecoverButton.Text = "Recuperar"
                RemountImgBtn.Text = "Recargar"
                RWRemountBtn.Text = "Recargar con permisos de escritura"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                LinkLabel2.Text = "Las propiedades no pueden ser obtenidas porque aún no se ha montado una imagen. Cuando lo haga, información detallada aparecerá aquí. Haga clic aquí para montar una imagen"
        End Select
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Label1.Text = TabControl1.SelectedTab.Text & " properties"
                    Case "ESN"
                        Label1.Text = "Propiedades de " & TabControl1.SelectedTab.Text.ToLower()
                End Select
            Case 1
                Label1.Text = TabControl1.SelectedTab.Text & " properties"
            Case 2
                Label1.Text = "Propiedades de " & TabControl1.SelectedTab.Text.ToLower()
        End Select
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
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
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
        If Environment.OSVersion.Version.Major = 10 Then
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
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            Label19.Text = "Yes"
                        Case "ESN"
                            Label19.Text = "Sí"
                    End Select
                Case 1
                    Label19.Text = "Yes"
                Case 2
                    Label19.Text = "Sí"
            End Select
            Try
                If Not Directory.Exists(MainForm.projPath & "\tempinfo") Then
                    Directory.CreateDirectory(MainForm.projPath & "\tempinfo").Attributes = FileAttributes.Hidden
                End If
                DetectImageProperties()
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
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
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
                        Case "ESN"
                            imgMountDir.Text = "No disponible"
                            imgIndex.Text = "No disponible"
                            imgName.Text = "No disponible"
                            imgMountedStatus.Text = "No disponible"
                            imgVersion.Text = "No disponible"
                            imgMountedName.Text = "No disponible"
                            imgMountedDesc.Text = "No disponible"
                            imgSize.Text = "No disponible"
                            imgWimBootStatus.Text = "No disponible"
                            imgArch.Text = "No disponible"
                            imgHal.Text = "No disponible"
                            imgSPBuild.Text = "No disponible"
                            imgSPLvl.Text = "No disponible"
                            imgEdition.Text = "No disponible"
                            imgPType.Text = "No disponible"
                            imgPSuite.Text = "No disponible"
                            imgSysRoot.Text = "No disponible"
                            imgDirs.Text = "No disponible"
                            imgFiles.Text = "No disponible"
                            imgCreation.Text = "No disponible"
                            imgModification.Text = "No disponible"
                            imgFormat.Text = "No disponible"
                            imgRW.Text = "No disponible"
                    End Select
                Case 1
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
                Case 2
                    imgMountDir.Text = "No disponible"
                    imgIndex.Text = "No disponible"
                    imgName.Text = "No disponible"
                    imgMountedStatus.Text = "No disponible"
                    imgVersion.Text = "No disponible"
                    imgMountedName.Text = "No disponible"
                    imgMountedDesc.Text = "No disponible"
                    imgSize.Text = "No disponible"
                    imgWimBootStatus.Text = "No disponible"
                    imgArch.Text = "No disponible"
                    imgHal.Text = "No disponible"
                    imgSPBuild.Text = "No disponible"
                    imgSPLvl.Text = "No disponible"
                    imgEdition.Text = "No disponible"
                    imgPType.Text = "No disponible"
                    imgPSuite.Text = "No disponible"
                    imgSysRoot.Text = "No disponible"
                    imgDirs.Text = "No disponible"
                    imgFiles.Text = "No disponible"
                    imgCreation.Text = "No disponible"
                    imgModification.Text = "No disponible"
                    imgFormat.Text = "No disponible"
                    imgRW.Text = "No disponible"
            End Select
            Panel3.Visible = True
            Label4.Visible = False
        End If
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Label1.Text = TabControl1.SelectedTab.Text & " properties"
                    Case "ESN"
                        Label1.Text = "Propiedades de " & TabControl1.SelectedTab.Text.ToLower()
                End Select
            Case 1
                Label1.Text = TabControl1.SelectedTab.Text & " properties"
            Case 2
                Label1.Text = "Propiedades de " & TabControl1.SelectedTab.Text.ToLower()
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
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
    ''' <param name="SysVer">The image version detected by the DISM API</param>
    ''' <remarks>Feature updates are only applicable to Windows 10 and 11. If this function detects an earlier version, it will leave</remarks>
    Sub DetectFeatureUpdate(SysVer As Version)
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
                    Case 22350 To 25000     ' 25000 is a relative number. This is because of the structural changes in Windows Insider channels, where 23xxx builds are the new Dev builds, and the Zinc development builds since 25314 are the new Canary builds
                        FeatUpd = "22H2 (Nickel)"
                    Case 25057 To 25238
                        FeatUpd = "22H2 (Copper)"
                    Case 25240 To 26000     ' 26000 is a relative number. We still don't know Zinc's final build
                        FeatUpd = "23H2 (Zinc)"
                End Select
            Case Else
                Exit Sub
        End Select
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        imgVersion.Text &= CrLf & "(feature update: " & FeatUpd & ")"
                    Case "ESN"
                        imgVersion.Text &= CrLf & "(act. de características: " & FeatUpd & ")"
                End Select
            Case 1
                imgVersion.Text &= CrLf & "(feature update: " & FeatUpd & ")"
            Case 2
                imgVersion.Text &= CrLf & "(act. de características: " & FeatUpd & ")"
        End Select
    End Sub

    Private Sub ProjProperties_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
    End Sub

    Private Sub Label37_MouseHover(sender As Object, e As EventArgs) Handles Label37.MouseHover
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        HalHelper.SetToolTip(sender, "Hardware Abstraction Layer")
                    Case "ESN"
                        HalHelper.SetToolTip(sender, "Capa de abstracción de hardware")
                End Select
            Case 1
                HalHelper.SetToolTip(sender, "Hardware Abstraction Layer")
            Case 2
                HalHelper.SetToolTip(sender, "Capa de abstracción de hardware")
        End Select
    End Sub
End Class
