Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports Microsoft.Dism
Imports DISMTools.Utilities
Imports System.Threading

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
        While MainForm.MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Threading.Thread.Sleep(100)
        End While
        MainForm.WatcherTimer.Enabled = False
        If MainForm.WatcherBW.IsBusy Then MainForm.WatcherBW.CancelAsync()
        While MainForm.WatcherBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
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
                                Case "ENU", "ENG"
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
                                Case "FRA"
                                    Select Case MainForm.MountedImageImgStatuses(x)
                                        Case 0
                                            imgMountedStatus.Text = "OK"
                                            RecoverButton.Visible = False
                                            RemountImgBtn.Visible = False
                                        Case 1
                                            imgMountedStatus.Text = "Nécessite un remontage"
                                            RecoverButton.Visible = False
                                            RemountImgBtn.Visible = True
                                        Case 2
                                            imgMountedStatus.Text = "Invalide"
                                            RecoverButton.Visible = True
                                            RemountImgBtn.Visible = False
                                    End Select
                                Case "PTB", "PTG"
                                    Select Case MainForm.MountedImageImgStatuses(x)
                                        Case 0
                                            imgMountedStatus.Text = "OK"
                                            RecoverButton.Visible = False
                                            RemountImgBtn.Visible = False
                                        Case 1
                                            imgMountedStatus.Text = "Necessita de remontagem"
                                            RecoverButton.Visible = False
                                            RemountImgBtn.Visible = True
                                        Case 2
                                            imgMountedStatus.Text = "Inválido"
                                            RecoverButton.Visible = True
                                            RemountImgBtn.Visible = False
                                    End Select
                                Case "ITA"
                                    Select Case MainForm.MountedImageImgStatuses(x)
                                        Case 0
                                            imgMountedStatus.Text = "OK"
                                            RecoverButton.Visible = False
                                            RemountImgBtn.Visible = False
                                        Case 1
                                            imgMountedStatus.Text = "Necessità di rimontaggio"
                                            RecoverButton.Visible = False
                                            RemountImgBtn.Visible = True
                                        Case 2
                                            imgMountedStatus.Text = "Non valido"
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
                        Case 3
                            Select Case MainForm.MountedImageImgStatuses(x)
                                Case 0
                                    imgMountedStatus.Text = "OK"
                                    RecoverButton.Visible = False
                                    RemountImgBtn.Visible = False
                                Case 1
                                    imgMountedStatus.Text = "Nécessite un remontage"
                                    RecoverButton.Visible = False
                                    RemountImgBtn.Visible = True
                                Case 2
                                    imgMountedStatus.Text = "Invalide"
                                    RecoverButton.Visible = True
                                    RemountImgBtn.Visible = False
                            End Select
                        Case 4
                            Select Case MainForm.MountedImageImgStatuses(x)
                                Case 0
                                    imgMountedStatus.Text = "OK"
                                    RecoverButton.Visible = False
                                    RemountImgBtn.Visible = False
                                Case 1
                                    imgMountedStatus.Text = "Necessita de remontagem"
                                    RecoverButton.Visible = False
                                    RemountImgBtn.Visible = True
                                Case 2
                                    imgMountedStatus.Text = "Inválido"
                                    RecoverButton.Visible = True
                                    RemountImgBtn.Visible = False
                            End Select
                        Case 5
                            Select Case MainForm.MountedImageImgStatuses(x)
                                Case 0
                                    imgMountedStatus.Text = "OK"
                                    RecoverButton.Visible = False
                                    RemountImgBtn.Visible = False
                                Case 1
                                    imgMountedStatus.Text = "Necessità di rimontaggio"
                                    RecoverButton.Visible = False
                                    RemountImgBtn.Visible = True
                                Case 2
                                    imgMountedStatus.Text = "Non valido"
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
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            imgSize.Text = info.ImageSize.ToString("N0") & " bytes (~" & Converters.BytesToReadableSize(info.ImageSize) & ")"
                                        Case "ESN"
                                            imgSize.Text = info.ImageSize.ToString("N0") & " bytes (~" & Converters.BytesToReadableSize(info.ImageSize) & ")"
                                        Case "FRA"
                                            imgSize.Text = info.ImageSize.ToString("N0") & " octets (~" & Converters.BytesToReadableSize(info.ImageSize, True) & ")"
                                        Case "PTB", "PTG"
                                            imgSize.Text = info.ImageSize.ToString("N0") & " bytes (~" & Converters.BytesToReadableSize(info.ImageSize) & ")"
                                        Case "ITA"
                                            imgSize.Text = info.ImageSize.ToString("N0") & " bytes (~" & Converters.BytesToReadableSize(info.ImageSize) & ")"
                                    End Select
                                Case 1
                                    imgSize.Text = info.ImageSize.ToString("N0") & " bytes (~" & Converters.BytesToReadableSize(info.ImageSize) & ")"
                                Case 2
                                    imgSize.Text = info.ImageSize.ToString("N0") & " bytes (~" & Converters.BytesToReadableSize(info.ImageSize) & ")"
                                Case 3
                                    imgSize.Text = info.ImageSize.ToString("N0") & " octets (~" & Converters.BytesToReadableSize(info.ImageSize, True) & ")"
                                Case 4
                                    imgSize.Text = info.ImageSize.ToString("N0") & " bytes (~" & Converters.BytesToReadableSize(info.ImageSize) & ")"
                                Case 5
                                    imgSize.Text = info.ImageSize.ToString("N0") & " bytes (~" & Converters.BytesToReadableSize(info.ImageSize) & ")"
                            End Select

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
                                        Case "ENU", "ENG"
                                            imgHal.Text = If(Not info.Hal = "", info.Hal, "Undefined by the image")
                                        Case "ESN"
                                            imgHal.Text = If(Not info.Hal = "", info.Hal, "No definida por la imagen")
                                        Case "FRA"
                                            imgHal.Text = If(Not info.Hal = "", info.Hal, "Non défini par l'image")
                                        Case "PTB", "PTG"
                                            imgHal.Text = If(Not info.Hal = "", info.Hal, "Não definido pela imagem")
                                        Case "ITA"
                                            imgHal.Text = If(Not info.Hal = "", info.Hal, "Non definito dall'immagine")
                                    End Select
                                Case 1
                                    imgHal.Text = If(Not info.Hal = "", info.Hal, "Undefined by the image")
                                Case 2
                                    imgHal.Text = If(Not info.Hal = "", info.Hal, "No definida por la imagen")
                                Case 3
                                    imgHal.Text = If(Not info.Hal = "", info.Hal, "Non défini par l'image")
                                Case 4
                                    imgHal.Text = If(Not info.Hal = "", info.Hal, "Não definido pela imagem")
                                Case 5
                                    imgHal.Text = If(Not info.Hal = "", info.Hal, "Non definito dall'immagine")
                            End Select
                            imgSPBuild.Text = info.ProductVersion.Revision
                            imgSPLvl.Text = info.SpLevel
                            imgEdition.Text = info.EditionId
                            imgPType.Text = info.ProductType
                            imgPSuite.Text = info.ProductSuite
                            imgSysRoot.Text = info.SystemRoot
                            imgLangText.Clear()
                            For Each language In info.Languages
                                ' Leave this temporarily
                                If imgLangText.Text = "" Then
                                    Select Case MainForm.Language
                                        Case 0
                                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                                Case "ENU", "ENG"
                                                    imgLangText.Text = language.Name & If(info.DefaultLanguage.Name = language.Name, " (default)", "") & ", "
                                                Case "ESN"
                                                    imgLangText.Text = language.Name & If(info.DefaultLanguage.Name = language.Name, " (predeterminado)", "") & ", "
                                                Case "FRA"
                                                    imgLangText.Text = language.Name & If(info.DefaultLanguage.Name = language.Name, " (défaut)", "") & ", "
                                                Case "PTB", "PTG"
                                                    imgLangText.Text = language.Name & If(info.DefaultLanguage.Name = language.Name, " (predefinido)", "") & ", "
                                                Case "ITA"
                                                    imgLangText.Text = language.Name & If(info.DefaultLanguage.Name = language.Name, " (predefinito)", "") & ", "
                                            End Select
                                        Case 1
                                            imgLangText.Text = language.Name & If(info.DefaultLanguage.Name = language.Name, " (default)", "") & ", "
                                        Case 2
                                            imgLangText.Text = language.Name & If(info.DefaultLanguage.Name = language.Name, " (predeterminado)", "") & ", "
                                        Case 3
                                            imgLangText.Text = language.Name & If(info.DefaultLanguage.Name = language.Name, " (défaut)", "") & ", "
                                        Case 4
                                            imgLangText.Text = language.Name & If(info.DefaultLanguage.Name = language.Name, " (predefinido)", "") & ", "
                                        Case 5
                                            imgLangText.Text = language.Name & If(info.DefaultLanguage.Name = language.Name, " (predefinito)", "") & ", "
                                    End Select
                                Else
                                    Select Case MainForm.Language
                                        Case 0
                                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                                Case "ENU", "ENG"
                                                    imgLangText.AppendText(language.Name & If(info.DefaultLanguage.Name = language.Name, " (default)", "") & ", ")
                                                Case "ESN"
                                                    imgLangText.AppendText(language.Name & If(info.DefaultLanguage.Name = language.Name, " (predeterminado)", "") & ", ")
                                                Case "FRA"
                                                    imgLangText.AppendText(language.Name & If(info.DefaultLanguage.Name = language.Name, " (défaut)", "") & ", ")
                                                Case "PTB", "PTG"
                                                    imgLangText.AppendText(language.Name & If(info.DefaultLanguage.Name = language.Name, " (predefinido)", "") & ", ")
                                                Case "ITA"
                                                    imgLangText.AppendText(language.Name & If(info.DefaultLanguage.Name = language.Name, " (predefinito)", "") & ", ")
                                            End Select
                                        Case 1
                                            imgLangText.AppendText(language.Name & If(info.DefaultLanguage.Name = language.Name, " (default)", "") & ", ")
                                        Case 2
                                            imgLangText.AppendText(language.Name & If(info.DefaultLanguage.Name = language.Name, " (predeterminado)", "") & ", ")
                                        Case 3
                                            imgLangText.AppendText(language.Name & If(info.DefaultLanguage.Name = language.Name, " (défaut)", "") & ", ")
                                        Case 4
                                            imgLangText.AppendText(language.Name & If(info.DefaultLanguage.Name = language.Name, " (predefinido)", "") & ", ")
                                        Case 5
                                            imgLangText.AppendText(language.Name & If(info.DefaultLanguage.Name = language.Name, " (predefinito)", "") & ", ")
                                    End Select
                                End If

                                ' Our new, recommended way
                                Select Case MainForm.Language
                                    Case 0
                                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                            Case "ENU", "ENG"
                                                LanguageList.Items.Add(language.Name & " (" & language.DisplayName & If(info.DefaultLanguage.Name = language.Name, ", default", "") & ")")
                                            Case "ESN"
                                                LanguageList.Items.Add(language.Name & " (" & language.DisplayName & If(info.DefaultLanguage.Name = language.Name, ", predeterminado", "") & ")")
                                            Case "FRA"
                                                LanguageList.Items.Add(language.Name & " (" & language.DisplayName & If(info.DefaultLanguage.Name = language.Name, ", défaut", "") & ")")
                                            Case "PTB", "PTG"
                                                LanguageList.Items.Add(language.Name & " (" & language.DisplayName & If(info.DefaultLanguage.Name = language.Name, ", predefinido", "") & ")")
                                            Case "ITA"
                                                LanguageList.Items.Add(language.Name & " (" & language.DisplayName & If(info.DefaultLanguage.Name = language.Name, ", predefinito", "") & ")")
                                        End Select
                                    Case 1
                                        LanguageList.Items.Add(language.Name & " (" & language.DisplayName & If(info.DefaultLanguage.Name = language.Name, ", default", "") & ")")
                                    Case 2
                                        LanguageList.Items.Add(language.Name & " (" & language.DisplayName & If(info.DefaultLanguage.Name = language.Name, ", predeterminado", "") & ")")
                                    Case 3
                                        LanguageList.Items.Add(language.Name & " (" & language.DisplayName & If(info.DefaultLanguage.Name = language.Name, ", défaut", "") & ")")
                                    Case 4
                                        LanguageList.Items.Add(language.Name & " (" & language.DisplayName & If(info.DefaultLanguage.Name = language.Name, ", predefinido", "") & ")")
                                    Case 5
                                        LanguageList.Items.Add(language.Name & " (" & language.DisplayName & If(info.DefaultLanguage.Name = language.Name, ", predefinito", "") & ")")
                                End Select
                            Next
                            Dim langarr() As Char = imgLangText.Text.ToCharArray()
                            langarr(langarr.Count - 2) = ""
                            imgLangText.Text = New String(langarr)
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            imgFormat.Text = Path.GetExtension(MainForm.MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper() & " file"
                                        Case "ESN"
                                            imgFormat.Text = "Archivo " & Path.GetExtension(MainForm.MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper()
                                        Case "FRA"
                                            imgFormat.Text = "Fichier " & Path.GetExtension(MainForm.MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper()
                                        Case "PTB", "PTG"
                                            imgFormat.Text = "Ficheiro " & Path.GetExtension(MainForm.MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper()
                                        Case "ITA"
                                            imgFormat.Text = "File " & Path.GetExtension(MainForm.MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper()
                                    End Select
                                Case 1
                                    imgFormat.Text = Path.GetExtension(MainForm.MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper() & " file"
                                Case 2
                                    imgFormat.Text = "Archivo " & Path.GetExtension(MainForm.MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper()
                                Case 3
                                    imgFormat.Text = "Fichier " & Path.GetExtension(MainForm.MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper()
                                Case 4
                                    imgFormat.Text = "Ficheiro " & Path.GetExtension(MainForm.MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper()
                                Case 5
                                    imgFormat.Text = "File " & Path.GetExtension(MainForm.MountedImageImgFiles(x)).Replace(".", "").Trim().ToUpper()
                            End Select
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            imgRW.Text = If(MainForm.MountedImageMountedReWr(x) = 0, "Yes", "No")
                                        Case "ESN"
                                            imgRW.Text = If(MainForm.MountedImageMountedReWr(x) = 0, "Sí", "No")
                                        Case "FRA"
                                            imgRW.Text = If(MainForm.MountedImageMountedReWr(x) = 0, "Oui", "Non")
                                        Case "PTB", "PTG"
                                            imgRW.Text = If(MainForm.MountedImageMountedReWr(x) = 0, "Sim", "Não")
                                        Case "ITA"
                                            imgRW.Text = If(MainForm.MountedImageMountedReWr(x) = 0, "Sì", "No")
                                    End Select
                                Case 1
                                    imgRW.Text = If(MainForm.MountedImageMountedReWr(x) = 0, "Yes", "No")
                                Case 2
                                    imgRW.Text = If(MainForm.MountedImageMountedReWr(x) = 0, "Sí", "No")
                                Case 3
                                    imgRW.Text = If(MainForm.MountedImageMountedReWr(x) = 0, "Oui", "Non")
                                Case 4
                                    imgRW.Text = If(MainForm.MountedImageMountedReWr(x) = 0, "Sim", "Não")
                                Case 5
                                    imgRW.Text = If(MainForm.MountedImageMountedReWr(x) = 0, "Sì", "No")
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
                            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat",
                                              "@echo off" & CrLf &
                                              "dism /English /get-wiminfo /wimfile=" & Quote & MainForm.SourceImg & Quote & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & Quote & MainForm.projPath & "\tempinfo\imgwimboot" & Quote, ASCII)
                        Case Is >= 2
                            File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat",
                                              "@echo off" & CrLf &
                                              "dism /English /get-imageinfo /imagefile=" & Quote & MainForm.SourceImg & Quote & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & Quote & MainForm.projPath & "\tempinfo\imgwimboot" & Quote, ASCII)
                    End Select
                Case 10
                    File.WriteAllText(Application.StartupPath & "\bin\exthelpers\imginfo.bat",
                                      "@echo off" & CrLf &
                                      "dism /English /get-imageinfo /imagefile=" & Quote & MainForm.SourceImg & Quote & " /index=" & MainForm.ImgIndex & " | findstr /c:" & Quote & "WIM Bootable" & Quote & " /b > " & Quote & MainForm.projPath & "\tempinfo\imgwimboot" & Quote, ASCII)
            End Select
            If Debugger.IsAttached Then
                Process.Start("\Windows\system32\notepad.exe", Application.StartupPath & "\bin\exthelpers\imginfo.bat").WaitForExit()
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
                imgWimBootStatus.Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\tempinfo\imgwimboot", ASCII).Replace("WIM Bootable : ", "").Trim()
                If Not MainForm.ImgBW.IsBusy Then
                    For Each foundFile In My.Computer.FileSystem.GetFiles(MainForm.projPath & "\tempinfo", FileIO.SearchOption.SearchTopLevelOnly)
                        File.Delete(foundFile)
                    Next
                    Directory.Delete(MainForm.projPath & "\tempinfo")
                End If
                File.Delete(Application.StartupPath & "\bin\exthelpers\imginfo.bat")
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ProjProperties_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
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
                    Case "FRA"
                        Label2.Text = "Voir les propriétés du projet, telles que le nom ou l'emplacement"
                        Label3.Text = "Voir les propriétés de l'image montée, telles que le nom, la description ou l'index"
                        Label4.Text = "Obtention des informations sur les projets et les images en cours. Veuillez patienter..."
                        Label5.Text = "Nom :"
                        Label6.Text = "Lieu :"
                        Label7.Text = "Date de création :"
                        Label8.Text = "GUID du projet :"
                        Label13.Text = "Répertoire de montage :"
                        Label14.Text = "Index de l'image :"
                        Label15.Text = "Fichier de l'image :"
                        Label20.Text = "Image présente sur le projet ?"
                        Label22.Text = "État de l'image :"
                        Label25.Text = "Version :"
                        Label27.Text = "Nom :"
                        Label29.Text = "Description :"
                        Label31.Text = "Taille :"
                        Label33.Text = "Supporte WIMBoot ?"
                        Label35.Text = "Architecture :"
                        Label39.Text = "Compilation du Service Pack :"
                        Label41.Text = "Niveau du Service Pack :"
                        Label43.Text = "Édition :"
                        Label45.Text = "Type de produit :"
                        Label47.Text = "Suite du produit :"
                        Label49.Text = "Répertoire racine du système :"
                        Label51.Text = "Nombre de répertoires :"
                        Label53.Text = "Nombre de fichiers :"
                        Label55.Text = "Date de création :"
                        Label57.Text = "Date de modification :"
                        Label58.Text = "Langues installées :"
                        Label60.Text = "Format du fichier :"
                        Label62.Text = "Droits L/E de l'image :"
                        TabPage1.Text = "Projet"
                        TabPage2.Text = "Image"
                        RecoverButton.Text = "Récupérer"
                        RemountImgBtn.Text = "Recharger"
                        RWRemountBtn.Text = "Remonter avec les droits d'écriture"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        LinkLabel2.Text = "De nombreuses propriétés ne sont pas visibles car l'image n'a pas encore été montée. Une fois l'image montée, des informations détaillées s'afficheront ici. Cliquez ici pour monter une image"
                    Case "PTB", "PTG"
                        Label2.Text = "Ver as propriedades do projeto, como o nome ou a localização"
                        Label3.Text = "Ver as propriedades da imagem montada, como o nome, a descrição ou o índice"
                        Label4.Text = "Obter informações sobre o projeto e a imagem. Aguarde..."
                        Label5.Text = "Nome:"
                        Label6.Text = "Localização:"
                        Label7.Text = "Data de criação:"
                        Label8.Text = "GUID do projeto:"
                        Label13.Text = "Diretório de montagem:"
                        Label14.Text = "Índice da imagem:"
                        Label15.Text = "Ficheiro de imagem:"
                        Label20.Text = "Imagem presente no projeto?"
                        Label22.Text = "Estado da imagem:"
                        Label25.Text = "Versão:"
                        Label27.Text = "Nome:"
                        Label29.Text = "Descrição:"
                        Label31.Text = "Tamanho:"
                        Label33.Text = "Suporta WIMBoot?"
                        Label35.Text = "Arquitetura:"
                        Label39.Text = "Service Pack build:"
                        Label41.Text = "Nível do Service Pack:"
                        Label43.Text = "Edição:"
                        Label45.Text = "Tipo de produto:"
                        Label47.Text = "Conjunto de produtos:"
                        Label49.Text = "Diretório raiz do sistema:"
                        Label51.Text = "Contagem de directórios:"
                        Label53.Text = "Contagem de ficheiros:"
                        Label55.Text = "Data de criação:"
                        Label57.Text = "Data de modificação:"
                        Label58.Text = "Idiomas instalados:"
                        Label60.Text = "Formato do ficheiro:"
                        Label62.Text = "Permissões de imagem R/W:"
                        TabPage1.Text = "Projeto"
                        TabPage2.Text = "Imagem"
                        RecoverButton.Text = "Recuperar"
                        RemountImgBtn.Text = "Recarregar"
                        RWRemountBtn.Text = "Remontar com permissões de escrita"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        LinkLabel2.Text = "Muitas propriedades não podem ser vistas porque a imagem ainda não foi montada. Depois de a montar, serão mostradas aqui informações detalhadas. Clique aqui para montar uma imagem"
                    Case "ITA"
                        Label2.Text = "Visualizza le proprietà del progetto, come il nome o l'ubicazione"
                        Label3.Text = "Visualizza le proprietà dell'immagine montata, come il nome, la descrizione o l'indice"
                        Label4.Text = "Ottenere informazioni sul progetto e sull'immagine. Attendere..."
                        Label5.Text = "Nome:"
                        Label6.Text = " Ubicazione:"
                        Label7.Text = "Data di creazione:"
                        Label8.Text = "GUID del progetto:"
                        Label13.Text = "Directory di montaggio:"
                        Label14.Text = "Indice immagine:"
                        Label15.Text = "File immagine:"
                        Label20.Text = "Immagine presente nel progetto?"
                        Label22.Text = "Stato immagine:"
                        Label25.Text = "Versione:"
                        Label27.Text = "Nome:"
                        Label29.Text = "Descrizione:"
                        Label31.Text = "Dimensione:"
                        Label33.Text = "Supporta WIMBoot?"
                        Label35.Text = "Architettura:"
                        Label39.Text = "Service Pack build:"
                        Label41.Text = "Livello del Service Pack:"
                        Label43.Text = "Edizione:"
                        Label45.Text = "Tipo di prodotto:"
                        Label47.Text = "Suite di prodotti:"
                        Label49.Text = " Cartella principale del sistema:"
                        Label51.Text = "Numero di cartelle:"
                        Label53.Text = "Numero di file:"
                        Label55.Text = "Data di creazione:"
                        Label57.Text = "Data di modifica:"
                        Label58.Text = "Lingue installate:"
                        Label60.Text = "Formato file:"
                        Label62.Text = "Autorizzazioni R/W immagine:"
                        TabPage1.Text = "Progetto"
                        TabPage2.Text = "Immagine"
                        RecoverButton.Text = "Recupera"
                        RemountImgBtn.Text = "Ricaricare"
                        RWRemountBtn.Text = "Rimonta con i permessi di scrittura"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        LinkLabel2.Text = "Molte proprietà non possono essere visualizzate perché l'immagine non è ancora stata montata. Una volta montata, le informazioni dettagliate saranno mostrate qui. Fare clic qui per montare un'immagine"
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
            Case 3
                Label2.Text = "Voir les propriétés du projet, telles que le nom ou l'emplacement"
                Label3.Text = "Voir les propriétés de l'image montée, telles que le nom, la description ou l'index"
                Label4.Text = "Obtention des informations sur les projets et les images en cours. Veuillez patienter..."
                Label5.Text = "Nom :"
                Label6.Text = "Lieu :"
                Label7.Text = "Date de création :"
                Label8.Text = "GUID du projet :"
                Label13.Text = "Répertoire de montage :"
                Label14.Text = "Index de l'image :"
                Label15.Text = "Fichier de l'image :"
                Label20.Text = "Image présente sur le projet ?"
                Label22.Text = "État de l'image :"
                Label25.Text = "Version :"
                Label27.Text = "Nom :"
                Label29.Text = "Description :"
                Label31.Text = "Taille :"
                Label33.Text = "Supporte WIMBoot ?"
                Label35.Text = "Architecture :"
                Label39.Text = "Compilation du Service Pack :"
                Label41.Text = "Niveau du Service Pack :"
                Label43.Text = "Édition :"
                Label45.Text = "Type de produit :"
                Label47.Text = "Suite du produit :"
                Label49.Text = "Répertoire racine du système :"
                Label51.Text = "Nombre de répertoires :"
                Label53.Text = "Nombre de fichiers :"
                Label55.Text = "Date de création :"
                Label57.Text = "Date de modification :"
                Label58.Text = "Langues installées :"
                Label60.Text = "Format du fichier :"
                Label62.Text = "Droits L/E de l'image :"
                TabPage1.Text = "Projet"
                TabPage2.Text = "Image"
                RecoverButton.Text = "Récupérer"
                RemountImgBtn.Text = "Recharger"
                RWRemountBtn.Text = "Remonter avec les droits d'écriture"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                LinkLabel2.Text = "De nombreuses propriétés ne sont pas visibles car l'image n'a pas encore été montée. Une fois l'image montée, des informations détaillées s'afficheront ici. Cliquez ici pour monter une image"
            Case 4
                Label2.Text = "Ver as propriedades do projeto, como o nome ou a localização"
                Label3.Text = "Ver as propriedades da imagem montada, como o nome, a descrição ou o índice"
                Label4.Text = "Obter informações sobre o projeto e a imagem. Aguarde..."
                Label5.Text = "Nome:"
                Label6.Text = "Localização:"
                Label7.Text = "Data de criação:"
                Label8.Text = "GUID do projeto:"
                Label13.Text = "Diretório de montagem:"
                Label14.Text = "Índice da imagem:"
                Label15.Text = "Ficheiro de imagem:"
                Label20.Text = "Imagem presente no projeto?"
                Label22.Text = "Estado da imagem:"
                Label25.Text = "Versão:"
                Label27.Text = "Nome:"
                Label29.Text = "Descrição:"
                Label31.Text = "Tamanho:"
                Label33.Text = "Suporta WIMBoot?"
                Label35.Text = "Arquitetura:"
                Label39.Text = "Service Pack build:"
                Label41.Text = "Nível do Service Pack:"
                Label43.Text = "Edição:"
                Label45.Text = "Tipo de produto:"
                Label47.Text = "Conjunto de produtos:"
                Label49.Text = "Diretório raiz do sistema:"
                Label51.Text = "Contagem de directórios:"
                Label53.Text = "Contagem de ficheiros:"
                Label55.Text = "Data de criação:"
                Label57.Text = "Data de modificação:"
                Label58.Text = "Idiomas instalados:"
                Label60.Text = "Formato do ficheiro:"
                Label62.Text = "Permissões de imagem R/W:"
                TabPage1.Text = "Projeto"
                TabPage2.Text = "Imagem"
                RecoverButton.Text = "Recuperar"
                RemountImgBtn.Text = "Recarregar"
                RWRemountBtn.Text = "Remontar com permissões de escrita"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                LinkLabel2.Text = "Muitas propriedades não podem ser vistas porque a imagem ainda não foi montada. Depois de a montar, serão mostradas aqui informações detalhadas. Clique aqui para montar uma imagem"
            Case 5
                Label2.Text = "Visualizza le proprietà del progetto, come il nome o l'ubicazione"
                Label3.Text = "Visualizza le proprietà dell'immagine montata, come il nome, la descrizione o l'indice"
                Label4.Text = "Ottenere informazioni sul progetto e sull'immagine. Attendere..."
                Label5.Text = "Nome:"
                Label6.Text = " Ubicazione:"
                Label7.Text = "Data di creazione:"
                Label8.Text = "GUID del progetto:"
                Label13.Text = "Directory di montaggio:"
                Label14.Text = "Indice immagine:"
                Label15.Text = "File immagine:"
                Label20.Text = "Immagine presente nel progetto?"
                Label22.Text = "Stato immagine:"
                Label25.Text = "Versione:"
                Label27.Text = "Nome:"
                Label29.Text = "Descrizione:"
                Label31.Text = "Dimensione:"
                Label33.Text = "Supporta WIMBoot?"
                Label35.Text = "Architettura:"
                Label39.Text = "Service Pack build:"
                Label41.Text = "Livello del Service Pack:"
                Label43.Text = "Edizione:"
                Label45.Text = "Tipo di prodotto:"
                Label47.Text = "Suite di prodotti:"
                Label49.Text = " Cartella principale del sistema:"
                Label51.Text = "Numero di cartelle:"
                Label53.Text = "Numero di file:"
                Label55.Text = "Data di creazione:"
                Label57.Text = "Data di modifica:"
                Label58.Text = "Lingue installate:"
                Label60.Text = "Formato file:"
                Label62.Text = "Autorizzazioni R/W immagine:"
                TabPage1.Text = "Progetto"
                TabPage2.Text = "Immagine"
                RecoverButton.Text = "Recupera"
                RemountImgBtn.Text = "Ricaricare"
                RWRemountBtn.Text = "Rimonta con i permessi di scrittura"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                LinkLabel2.Text = "Molte proprietà non possono essere visualizzate perché l'immagine non è ancora stata montata. Una volta montata, le informazioni dettagliate saranno mostrate qui. Fare clic qui per montare un'immagine"
        End Select
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label1.Text = TabControl1.SelectedTab.Text & " properties"
                    Case "ESN"
                        Label1.Text = "Propiedades " & If(TabControl1.SelectedIndex = 0, "del proyecto", "de la imagen")
                    Case "FRA"
                        Label1.Text = "Propriétés " & If(TabControl1.SelectedIndex = 0, "du projet", "de l'image")
                    Case "PTB", "PTG"
                        Label1.Text = "Propriedades " & If(TabControl1.SelectedIndex = 0, "do projeto", "da imagem")
                    Case "ITA"
                        Label1.Text = "Proprietà " & If(TabControl1.SelectedIndex = 0, "del progetto", "dell'immagine")
                End Select
            Case 1
                Label1.Text = TabControl1.SelectedTab.Text & " properties"
            Case 2
                Label1.Text = "Propiedades " & If(TabControl1.SelectedIndex = 0, "del proyecto", "de la imagen")
            Case 3
                Label1.Text = "Propriétés " & If(TabControl1.SelectedIndex = 0, "du projet", "de l'image")
            Case 4
                Label1.Text = "Propriedades " & If(TabControl1.SelectedIndex = 0, "do projeto", "da imagem")
            Case 5
                Label1.Text = "Proprietà " & If(TabControl1.SelectedIndex = 0, "del progetto", "dell'immagine")
        End Select
        ' Set program colors
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TabPage1.BackColor = Color.FromArgb(31, 31, 31)
            TabPage2.BackColor = Color.FromArgb(31, 31, 31)
            imgLangText.BackColor = Color.FromArgb(31, 31, 31)
            LanguageList.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TabPage1.BackColor = Color.FromArgb(238, 238, 242)
            TabPage2.BackColor = Color.FromArgb(238, 238, 242)
            imgLangText.BackColor = Color.FromArgb(238, 238, 242)
            LanguageList.BackColor = Color.FromArgb(238, 238, 242)
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        imgLangText.ForeColor = ForeColor
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
        imgLangText.Text = ""
        LanguageList.Items.Clear()
        Visible = True
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Label4.Visible = True
        Label9.Text = MainForm.Label49.Text
        Label10.Text = MainForm.projPath
        Label11.Text = File.GetCreationTime(MainForm.projPath)
        Dim rtb As New RichTextBox With {
            .Text = My.Computer.FileSystem.ReadAllText(MainForm.projPath & "\" & MainForm.Label49.Text & ".dtproj")
        }
        If rtb.Lines(6).StartsWith("ProjGuid") Then
            Label12.Text = rtb.Lines(6).Replace("ProjGuid=", "").Trim()
        End If
        If MainForm.IsImageMounted Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label19.Text = "Yes"
                        Case "ESN"
                            Label19.Text = "Sí"
                        Case "FRA"
                            Label19.Text = "Oui"
                        Case "PTB", "PTG"
                            Label19.Text = "Sim"
                        Case "ITA"
                            Label19.Text = "Sì"
                    End Select
                Case 1
                    Label19.Text = "Yes"
                Case 2
                    Label19.Text = "Sí"
                Case 3
                    Label19.Text = "Oui"
                Case 4
                    Label19.Text = "Sim"
                Case 5
                    Label19.Text = "Sì"
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
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label19.Text = "No"
                        Case "ESN"
                            Label19.Text = "No"
                        Case "FRA"
                            Label19.Text = "Non"
                        Case "PTB", "PTG"
                            Label19.Text = "Não"
                        Case "ITA"
                            Label19.Text = "No"
                    End Select
                Case 1
                    Label19.Text = "No"
                Case 2
                    Label19.Text = "No"
                Case 3
                    Label19.Text = "Non"
                Case 4
                    Label19.Text = "Não"
                Case 5
                    Label19.Text = "No"
            End Select
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
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
                        Case "FRA"
                            imgMountDir.Text = "Non disponible"
                            imgIndex.Text = "Non disponible"
                            imgName.Text = "Non disponible"
                            imgMountedStatus.Text = "Non disponible"
                            imgVersion.Text = "Non disponible"
                            imgMountedName.Text = "Non disponible"
                            imgMountedDesc.Text = "Non disponible"
                            imgSize.Text = "Non disponible"
                            imgWimBootStatus.Text = "Non disponible"
                            imgArch.Text = "Non disponible"
                            imgHal.Text = "Non disponible"
                            imgSPBuild.Text = "Non disponible"
                            imgSPLvl.Text = "Non disponible"
                            imgEdition.Text = "Non disponible"
                            imgPType.Text = "Non disponible"
                            imgPSuite.Text = "Non disponible"
                            imgSysRoot.Text = "Non disponible"
                            imgDirs.Text = "Non disponible"
                            imgFiles.Text = "Non disponible"
                            imgCreation.Text = "Non disponible"
                            imgModification.Text = "Non disponible"
                            imgFormat.Text = "Non disponible"
                            imgRW.Text = "Non disponible"
                        Case "PTB", "PTG"
                            imgMountDir.Text = "Não disponível"
                            imgIndex.Text = "Não disponível"
                            imgName.Text = "Não disponível"
                            imgMountedStatus.Text = "Não disponível"
                            imgVersion.Text = "Não disponível"
                            imgMountedName.Text = "Não disponível"
                            imgMountedDesc.Text = "Não disponível"
                            imgSize.Text = "Não disponível"
                            imgWimBootStatus.Text = "Não disponível"
                            imgArch.Text = "Não disponível"
                            imgHal.Text = "Não disponível"
                            imgSPBuild.Text = "Não disponível"
                            imgSPLvl.Text = "Não disponível"
                            imgEdition.Text = "Não disponível"
                            imgPType.Text = "Não disponível"
                            imgPSuite.Text = "Não disponível"
                            imgSysRoot.Text = "Não disponível"
                            imgDirs.Text = "Não disponível"
                            imgFiles.Text = "Não disponível"
                            imgCreation.Text = "Não disponível"
                            imgModification.Text = "Não disponível"
                            imgFormat.Text = "Não disponível"
                            imgRW.Text = "Não disponível"
                        Case "ITA"
                            imgMountDir.Text = "Non disponibile"
                            imgIndex.Text = "Non disponibile"
                            imgName.Text = "Non disponibile"
                            imgMountedStatus.Text = "Non disponibile"
                            imgVersion.Text = "Non disponibile"
                            imgMountedName.Text = "Non disponibile"
                            imgMountedDesc.Text = "Non disponibile"
                            imgSize.Text = "Non disponibile"
                            imgWimBootStatus.Text = "Non disponibile"
                            imgArch.Text = "Non disponibile"
                            imgHal.Text = "Non disponibile"
                            imgSPBuild.Text = "Non disponibile"
                            imgSPLvl.Text = "Non disponibile"
                            imgEdition.Text = "Non disponibile"
                            imgPType.Text = "Non disponibile"
                            imgPSuite.Text = "Non disponibile"
                            imgSysRoot.Text = "Non disponibile"
                            imgDirs.Text = "Non disponibile"
                            imgFiles.Text = "Non disponibile"
                            imgCreation.Text = "Non disponibile"
                            imgModification.Text = "Non disponibile"
                            imgFormat.Text = "Non disponibile"
                            imgRW.Text = "Non disponibile"
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
                Case 3
                    imgMountDir.Text = "Non disponible"
                    imgIndex.Text = "Non disponible"
                    imgName.Text = "Non disponible"
                    imgMountedStatus.Text = "Non disponible"
                    imgVersion.Text = "Non disponible"
                    imgMountedName.Text = "Non disponible"
                    imgMountedDesc.Text = "Non disponible"
                    imgSize.Text = "Non disponible"
                    imgWimBootStatus.Text = "Non disponible"
                    imgArch.Text = "Non disponible"
                    imgHal.Text = "Non disponible"
                    imgSPBuild.Text = "Non disponible"
                    imgSPLvl.Text = "Non disponible"
                    imgEdition.Text = "Non disponible"
                    imgPType.Text = "Non disponible"
                    imgPSuite.Text = "Non disponible"
                    imgSysRoot.Text = "Non disponible"
                    imgDirs.Text = "Non disponible"
                    imgFiles.Text = "Non disponible"
                    imgCreation.Text = "Non disponible"
                    imgModification.Text = "Non disponible"
                    imgFormat.Text = "Non disponible"
                    imgRW.Text = "Non disponible"
                Case 4
                    imgMountDir.Text = "Não disponível"
                    imgIndex.Text = "Não disponível"
                    imgName.Text = "Não disponível"
                    imgMountedStatus.Text = "Não disponível"
                    imgVersion.Text = "Não disponível"
                    imgMountedName.Text = "Não disponível"
                    imgMountedDesc.Text = "Não disponível"
                    imgSize.Text = "Não disponível"
                    imgWimBootStatus.Text = "Não disponível"
                    imgArch.Text = "Não disponível"
                    imgHal.Text = "Não disponível"
                    imgSPBuild.Text = "Não disponível"
                    imgSPLvl.Text = "Não disponível"
                    imgEdition.Text = "Não disponível"
                    imgPType.Text = "Não disponível"
                    imgPSuite.Text = "Não disponível"
                    imgSysRoot.Text = "Não disponível"
                    imgDirs.Text = "Não disponível"
                    imgFiles.Text = "Não disponível"
                    imgCreation.Text = "Não disponível"
                    imgModification.Text = "Não disponível"
                    imgFormat.Text = "Não disponível"
                    imgRW.Text = "Não disponível"
                Case 5
                    imgMountDir.Text = "Non disponibile"
                    imgIndex.Text = "Non disponibile"
                    imgName.Text = "Non disponibile"
                    imgMountedStatus.Text = "Non disponibile"
                    imgVersion.Text = "Non disponibile"
                    imgMountedName.Text = "Non disponibile"
                    imgMountedDesc.Text = "Non disponibile"
                    imgSize.Text = "Non disponibile"
                    imgWimBootStatus.Text = "Non disponibile"
                    imgArch.Text = "Non disponibile"
                    imgHal.Text = "Non disponibile"
                    imgSPBuild.Text = "Non disponibile"
                    imgSPLvl.Text = "Non disponibile"
                    imgEdition.Text = "Non disponibile"
                    imgPType.Text = "Non disponibile"
                    imgPSuite.Text = "Non disponibile"
                    imgSysRoot.Text = "Non disponibile"
                    imgDirs.Text = "Non disponibile"
                    imgFiles.Text = "Non disponibile"
                    imgCreation.Text = "Non disponibile"
                    imgModification.Text = "Non disponibile"
                    imgFormat.Text = "Non disponibile"
                    imgRW.Text = "Non disponibile"
            End Select
            Panel3.Visible = True
            Label4.Visible = False
        End If
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label1.Text = TabControl1.SelectedTab.Text & " properties"
                    Case "ESN"
                        Label1.Text = "Propiedades " & If(TabControl1.SelectedIndex = 0, "del proyecto", "de la imagen")
                    Case "FRA"
                        Label1.Text = "Propriétés " & If(TabControl1.SelectedIndex = 0, "du projet", "de l'image")
                    Case "PTB", "PTG"
                        Label1.Text = "Propriedades " & If(TabControl1.SelectedIndex = 0, "do projeto", "da imagem")
                    Case "ITA"
                        Label1.Text = "Proprietà " & If(TabControl1.SelectedIndex = 0, "del progetto", "dell'immagine")
                End Select
            Case 1
                Label1.Text = TabControl1.SelectedTab.Text & " properties"
            Case 2
                Label1.Text = "Propiedades " & If(TabControl1.SelectedIndex = 0, "del proyecto", "de la imagen")
            Case 3
                Label1.Text = "Propriétés " & If(TabControl1.SelectedIndex = 0, "du projet", "de l'image")
            Case 4
                Label1.Text = "Propriedades " & If(TabControl1.SelectedIndex = 0, "do projeto", "da imagem")
            Case 5
                Label1.Text = "Proprietà " & If(TabControl1.SelectedIndex = 0, "del progetto", "dell'immagine")
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
        Else
            Text = Label1.Text
        End If
    End Sub

    Private Sub RWRemountBtn_Click(sender As Object, e As EventArgs) Handles RWRemountBtn.Click
        Visible = False
        If MainForm.MountedImageMountDirs.Count > 0 Then
            If MainForm.MountedImageMountDirs.Contains(MainForm.MountDir) Then
                For x = 0 To Array.LastIndexOf(MainForm.MountedImageMountDirs, MainForm.MountedImageMountDirs.Last)
                    If MainForm.MountedImageMountDirs(x) = MainForm.MountDir Then
                        MainForm.EnableWritePermissions(MainForm.MountedImageImgFiles(x), CInt(MainForm.MountedImageImgIndexes(x)), MainForm.MountedImageMountDirs(x))
                        Exit For
                    End If
                Next
            End If
        End If
        Visible = True
        If Not Directory.Exists(MainForm.projPath & "\tempinfo") Then
            Directory.CreateDirectory(MainForm.projPath & "\tempinfo").Attributes = FileAttributes.Hidden
        End If
        LanguageList.Items.Clear()
        DetectImageProperties()
    End Sub

    Private Sub RemountImgBtn_Click(sender As Object, e As EventArgs) Handles RemountImgBtn.Click
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
                    Case 25942 To 27500
                        FeatUpd = "24H2 (Germanium)"
                    Case 27501 To 27686
                        FeatUpd = "25H1 (Dilithium)"
                    Case Is >= 27687
                        FeatUpd = "25H2 (Selenium)"
                End Select
            Case Else
                Exit Sub
        End Select
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        imgVersion.Text &= CrLf & "(feature update: " & FeatUpd & ")"
                    Case "ESN"
                        imgVersion.Text &= CrLf & "(act. de características: " & FeatUpd & ")"
                    Case "FRA"
                        imgVersion.Text &= CrLf & "(m-à-j des caractéristiques: " & FeatUpd & ")"
                    Case "PTB", "PTG"
                        imgVersion.Text &= " (atualização de funcionalidades: " & FeatUpd & ")"
                    Case "ITA"
                        imgVersion.Text &= " (aggiornamento della caratteristica: " & FeatUpd & ")"
                End Select
            Case 1
                imgVersion.Text &= CrLf & "(feature update: " & FeatUpd & ")"
            Case 2
                imgVersion.Text &= CrLf & "(act. de características: " & FeatUpd & ")"
            Case 3
                imgVersion.Text &= CrLf & "(m-à-j des caractéristiques: " & FeatUpd & ")"
            Case 4
                imgVersion.Text &= " (atualização de funcionalidades: " & FeatUpd & ")"
            Case 5
                imgVersion.Text &= " (aggiornamento della caratteristica: " & FeatUpd & ")"
        End Select
    End Sub

    Private Sub ProjProperties_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
        MainForm.WatcherTimer.Enabled = True
    End Sub

    Private Sub Label37_MouseHover(sender As Object, e As EventArgs) Handles Label37.MouseHover
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        HalHelper.SetToolTip(sender, "Hardware Abstraction Layer")
                    Case "ESN"
                        HalHelper.SetToolTip(sender, "Capa de abstracción de hardware")
                    Case "FRA"
                        HalHelper.SetToolTip(sender, "Couche d'abstraction du matériel")
                    Case "PTB", "PTG"
                        HalHelper.SetToolTip(sender, "Camada de abstração de hardware")
                    Case "ITA"
                        HalHelper.SetToolTip(sender, "Livello di astrazione hardware")
                End Select
            Case 1
                HalHelper.SetToolTip(sender, "Hardware Abstraction Layer")
            Case 2
                HalHelper.SetToolTip(sender, "Capa de abstracción de hardware")
            Case 3
                HalHelper.SetToolTip(sender, "Couche d'abstraction du matériel")
            Case 4
                HalHelper.SetToolTip(sender, "Camada de abstração de hardware")
            Case 5
                HalHelper.SetToolTip(sender, "Livello di astrazione hardware")
        End Select
    End Sub
End Class
