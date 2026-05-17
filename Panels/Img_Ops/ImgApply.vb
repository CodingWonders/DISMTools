Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports Microsoft.Dism
Imports System.Threading

Public Class ImgApply

    Dim ImageVersions As New List(Of Version)
    Dim ImageEditions As New List(Of String)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        If TextBox1.Text = "" Or Not File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("Either no image file has been specified or it does not exist in the file system.")
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("The specified image file is not valid. Please specify a valid image and try again.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case "ESN"
                            MsgBox("El archivo de imagen especificado no es válido. Especifique una imagen válida e inténtelo de nuevo.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case "FRA"
                            MsgBox("Le fichier image spécifié n'est pas valide. Veuillez spécifier une image valide et réessayer.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case "PTB", "PTG"
                            MsgBox("O ficheiro de imagem especificado não é válido. Especifique uma imagem válida e tente novamente.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case "ITA"
                            MsgBox("Il file immagine specificato non è valido. Specificare un'immagine valida e riprovare.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    End Select
                Case 1
                    MsgBox("The specified image file is not valid. Please specify a valid image and try again.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Case 2
                    MsgBox("El archivo de imagen especificado no es válido. Especifique una imagen válida e inténtelo de nuevo.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Case 3
                    MsgBox("Le fichier image spécifié n'est pas valide. Veuillez spécifier une image valide et réessayer.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Case 4
                    MsgBox("O ficheiro de imagem especificado não é válido. Especifique uma imagem válida e tente novamente.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Case 5
                    MsgBox("Il file immagine specificato non è valido. Specificare un'immagine valida e riprovare.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            End Select
            Exit Sub
        End If
        ProgressPanel.ApplicationSourceImg = TextBox1.Text
        ProgressPanel.ApplicationIndex = ComboBox1.SelectedIndex + 1
        ProgressPanel.ApplicationDestDir = TextBox2.Text
        If CheckBox1.Checked Then
            ProgressPanel.ApplicationCheckInt = True
        Else
            ProgressPanel.ApplicationCheckInt = False
        End If
        If CheckBox2.Checked Then
            ProgressPanel.ApplicationVerify = True
        Else
            ProgressPanel.ApplicationVerify = False
        End If
        If CheckBox3.Checked Then
            ProgressPanel.ApplicationReparsePt = True
        Else
            ProgressPanel.ApplicationReparsePt = False
        End If
        If CheckBox4.Checked Then
            ProgressPanel.ApplicationSWMPattern = Path.GetDirectoryName(TextBox1.Text) & "\" & TextBox4.Text & "*.swm"
        Else
            ProgressPanel.ApplicationSWMPattern = ""
        End If
        If CheckBox5.Checked Then
            ProgressPanel.ApplicationValidateForTD = True
        Else
            ProgressPanel.ApplicationValidateForTD = False
        End If
        If CheckBox6.Checked Then
            ProgressPanel.ApplicationUseWimBoot = True
        Else
            ProgressPanel.ApplicationUseWimBoot = False
        End If
        If CheckBox7.Checked Then
            ProgressPanel.ApplicationCompactMode = True
        Else
            ProgressPanel.ApplicationCompactMode = False
        End If
        If CheckBox8.Checked Then
            ProgressPanel.ApplicationUseExtAttr = True
        Else
            ProgressPanel.ApplicationUseExtAttr = False
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 3
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgApply_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Apply an image"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Source image file:"
                        Label3.Text = "Image index:"
                        Label4.Text = "Naming pattern:"
                        CheckBox1.Text = "Check image integrity"
                        CheckBox2.Text = "Verify"
                        CheckBox3.Text = "Use the reparse point tag fix"
                        CheckBox4.Text = "Reference SWM files"
                        CheckBox5.Text = "Validate image for Trusted Desktop"
                        CheckBox6.Text = "Append image with WIMBoot configuration"
                        CheckBox7.Text = "Apply image in compact mode"
                        CheckBox8.Text = "Apply extended attributes"
                        Button1.Text = "Browse..."
                        Button2.Text = "Browse..."
                        Button4.Text = "Use name of the image"
                        Button5.Text = "Scan pattern"
                        UseMountedImgBtn.Text = "Use mounted image"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        Label5.Text = "Destination directory:"
                        GroupBox1.Text = "Source"
                        GroupBox2.Text = "Options"
                        GroupBox3.Text = "Destination"
                        GroupBox4.Text = "SWM file pattern"
                    Case "ESN"
                        Text = "Aplicar una imagen"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Imagen de origen:"
                        Label3.Text = "Índice:"
                        Label4.Text = "Nomenclatura:"
                        CheckBox1.Text = "Comprobar integridad de imagen"
                        CheckBox2.Text = "Verificar"
                        CheckBox3.Text = "Utilizar corrección de etiquetas de puntos de repetición de análisis"
                        CheckBox4.Text = "Hacer referencia a archivos SWM"
                        CheckBox5.Text = "Validar imagen de Trusted Desktop"
                        CheckBox6.Text = "Aplicar imagen con configuración WIMBoot"
                        CheckBox7.Text = "Aplicar imagen en modo compacto"
                        CheckBox8.Text = "Aplicar atributos extendidos"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Examinar..."
                        Button4.Text = "Usar nombre de imagen"
                        Button5.Text = "Escanear patrón"
                        UseMountedImgBtn.Text = "Usar imagen montada"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        Label5.Text = "Directorio de destino:"
                        GroupBox1.Text = "Origen"
                        GroupBox2.Text = "Opciones"
                        GroupBox3.Text = "Destino"
                        GroupBox4.Text = "Patrón de archivos SWM"
                    Case "FRA"
                        Text = "Appliquer une image"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Fichier de l'image originale :"
                        Label3.Text = "Index de l'image:"
                        Label4.Text = "Modèle de dénomination :"
                        CheckBox1.Text = "Vérifier l'intégrité de l'image"
                        CheckBox2.Text = "Verifier"
                        CheckBox3.Text = "Utiliser la correction de la balise reparse"
                        CheckBox4.Text = "Référence aux fichiers SWM"
                        CheckBox5.Text = "Valider l'image pour Trusted Desktop"
                        CheckBox6.Text = "Ajouter une image avec la configuration WIMBoot"
                        CheckBox7.Text = "Appliquer l'image en mode compact"
                        CheckBox8.Text = "Appliquer des attributs étendus"
                        Button1.Text = "Parcourir..."
                        Button2.Text = "Parcourir..."
                        Button4.Text = "Utiliser le nom de l'image"
                        Button5.Text = "Scanner le modèle"
                        UseMountedImgBtn.Text = "Utiliser une image montée"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        Label5.Text = "Répertoire de destination :"
                        GroupBox1.Text = "Source"
                        GroupBox2.Text = "Paramètres"
                        GroupBox3.Text = "Destination"
                        GroupBox4.Text = "Modèle de fichier SWM"
                    Case "PTB", "PTG"
                        Text = "Aplicar uma imagem"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Ficheiro de imagem de origem:"
                        Label3.Text = "Índice da imagem:"
                        Label4.Text = "Padrão de nomenclatura:"
                        CheckBox1.Text = "Verificar integridade da imagem"
                        CheckBox2.Text = "Verificar"
                        CheckBox3.Text = "Utilizar a correção da etiqueta de ponto de reparação"
                        CheckBox4.Text = "Referenciar ficheiros SWM"
                        CheckBox5.Text = "Validar imagem para o Trusted Desktop"
                        CheckBox6.Text = "Anexar imagem com configuração WIMBoot"
                        CheckBox7.Text = "Aplicar imagem em modo compacto"
                        CheckBox8.Text = "Aplicar atributos alargados"
                        Button1.Text = "Navegar..."
                        Button2.Text = "Navegar..."
                        Button4.Text = "Utilizar o nome da imagem"
                        Button5.Text = "Padrão de digitalização"
                        UseMountedImgBtn.Text = "Utilizar imagem montada"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        Label5.Text = "Diretório de destino:"
                        GroupBox1.Text = "Origem"
                        GroupBox2.Text = "Opções"
                        GroupBox3.Text = "Destino"
                        GroupBox4.Text = "Padrão de ficheiro SWM"
                    Case "ITA"
                        Text = "Applica un'immagine"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "File immagine di origine:"
                        Label3.Text = "Indice immagine:"
                        Label4.Text = "Modello di denominazione:"
                        CheckBox1.Text = "Verifica l'integrità dell'immagine"
                        CheckBox2.Text = "Verifica"
                        CheckBox3.Text = "Utilizza il tag fix del punto di reparse"
                        CheckBox4.Text = "File SWM di riferimento"
                        CheckBox5.Text = "Convalida l'immagine per Trusted Desktop"
                        CheckBox6.Text = "Aggiungi all'immagine la configurazione WIMBoot"
                        CheckBox7.Text = "Applica l'immagine in modalità compatta"
                        CheckBox8.Text = "Applica gli attributi estesi"
                        Button1.Text = "Sfoglia..."
                        Button2.Text = "Sfoglia..."
                        Button4.Text = "Usa il nome dell'immagine"
                        Button5.Text = "Modello di scansione"
                        UseMountedImgBtn.Text = "Usa immagine montata"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annulla"
                        Label5.Text = "Directory di destinazione:"
                        GroupBox1.Text = "Origine"
                        GroupBox2.Text = "Opzioni"
                        GroupBox3.Text = "Destinazione"
                        GroupBox4.Text = "Schema di file SWM"
                End Select
            Case 1
                Text = "Apply an image"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Source image file:"
                Label3.Text = "Image index:"
                Label4.Text = "Naming pattern:"
                CheckBox1.Text = "Check image integrity"
                CheckBox2.Text = "Verify"
                CheckBox3.Text = "Use the reparse point tag fix"
                CheckBox4.Text = "Reference SWM files"
                CheckBox5.Text = "Validate image for Trusted Desktop"
                CheckBox6.Text = "Append image with WIMBoot configuration"
                CheckBox7.Text = "Apply image in compact mode"
                CheckBox8.Text = "Apply extended attributes"
                Button1.Text = "Browse..."
                Button2.Text = "Browse..."
                Button4.Text = "Use name of the image"
                Button5.Text = "Scan pattern"
                UseMountedImgBtn.Text = "Use mounted image"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                Label5.Text = "Destination directory:"
                GroupBox1.Text = "Source"
                GroupBox2.Text = "Options"
                GroupBox3.Text = "Destination"
                GroupBox4.Text = "SWM file pattern"
            Case 2
                Text = "Aplicar una imagen"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Imagen de origen:"
                Label3.Text = "Índice:"
                Label4.Text = "Nomenclatura:"
                CheckBox1.Text = "Comprobar integridad de imagen"
                CheckBox2.Text = "Verificar"
                CheckBox3.Text = "Utilizar corrección de etiquetas de puntos de repetición de análisis"
                CheckBox4.Text = "Hacer referencia a archivos SWM"
                CheckBox5.Text = "Validar imagen de Trusted Desktop"
                CheckBox6.Text = "Aplicar imagen con configuración WIMBoot"
                CheckBox7.Text = "Aplicar imagen en modo compacto"
                CheckBox8.Text = "Aplicar atributos extendidos"
                Button1.Text = "Examinar..."
                Button2.Text = "Examinar..."
                Button4.Text = "Usar nombre de imagen"
                Button5.Text = "Escanear patrón"
                UseMountedImgBtn.Text = "Usar imagen montada"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                Label5.Text = "Directorio de destino:"
                GroupBox1.Text = "Origen"
                GroupBox2.Text = "Opciones"
                GroupBox3.Text = "Destino"
                GroupBox4.Text = "Patrón de archivos SWM"
            Case 3
                Text = "Appliquer une image"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Fichier de l'image originale :"
                Label3.Text = "Index de l'image:"
                Label4.Text = "Modèle de dénomination :"
                CheckBox1.Text = "Vérifier l'intégrité de l'image"
                CheckBox2.Text = "Verifier"
                CheckBox3.Text = "Utiliser la correction de la balise reparse"
                CheckBox4.Text = "Référence aux fichiers SWM"
                CheckBox5.Text = "Valider l'image pour Trusted Desktop"
                CheckBox6.Text = "Ajouter une image avec la configuration WIMBoot"
                CheckBox7.Text = "Appliquer l'image en mode compact"
                CheckBox8.Text = "Appliquer des attributs étendus"
                Button1.Text = "Parcourir..."
                Button2.Text = "Parcourir..."
                Button4.Text = "Utiliser le nom de l'image"
                Button5.Text = "Scanner le modèle"
                UseMountedImgBtn.Text = "Utiliser une image montée"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                Label5.Text = "Répertoire de destination :"
                GroupBox1.Text = "Source"
                GroupBox2.Text = "Paramètres"
                GroupBox3.Text = "Destination"
                GroupBox4.Text = "Modèle de fichier SWM"
            Case 4
                Text = "Aplicar uma imagem"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Ficheiro de imagem de origem:"
                Label3.Text = "Índice da imagem:"
                Label4.Text = "Padrão de nomenclatura:"
                CheckBox1.Text = "Verificar integridade da imagem"
                CheckBox2.Text = "Verificar"
                CheckBox3.Text = "Utilizar a correção da etiqueta de ponto de reparação"
                CheckBox4.Text = "Referenciar ficheiros SWM"
                CheckBox5.Text = "Validar imagem para o Trusted Desktop"
                CheckBox6.Text = "Anexar imagem com configuração WIMBoot"
                CheckBox7.Text = "Aplicar imagem em modo compacto"
                CheckBox8.Text = "Aplicar atributos alargados"
                Button1.Text = "Navegar..."
                Button2.Text = "Navegar..."
                Button4.Text = "Utilizar o nome da imagem"
                Button5.Text = "Padrão de digitalização"
                UseMountedImgBtn.Text = "Utilizar imagem montada"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                Label5.Text = "Diretório de destino:"
                GroupBox1.Text = "Origem"
                GroupBox2.Text = "Opções"
                GroupBox3.Text = "Destino"
                GroupBox4.Text = "Padrão de ficheiro SWM"
            Case 5
                Text = "Applica un'immagine"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "File immagine di origine:"
                Label3.Text = "Indice immagine:"
                Label4.Text = "Modello di denominazione:"
                CheckBox1.Text = "Verifica l'integrità dell'immagine"
                CheckBox2.Text = "Verifica"
                CheckBox3.Text = "Utilizza il tag fix del punto di reparse"
                CheckBox4.Text = "File SWM di riferimento"
                CheckBox5.Text = "Convalida l'immagine per Trusted Desktop"
                CheckBox6.Text = "Aggiungi all'immagine la configurazione WIMBoot"
                CheckBox7.Text = "Applica l'immagine in modalità compatta"
                CheckBox8.Text = "Applica gli attributi estesi"
                Button1.Text = "Sfoglia..."
                Button2.Text = "Sfoglia..."
                Button4.Text = "Usa il nome dell'immagine"
                Button5.Text = "Modello di scansione"
                UseMountedImgBtn.Text = "Usa immagine montata"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annulla"
                Label5.Text = "Directory di destinazione:"
                GroupBox1.Text = "Origine"
                GroupBox2.Text = "Opzioni"
                GroupBox3.Text = "Destinazione"
                GroupBox4.Text = "Schema di file SWM"
        End Select
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox4.BackColor = CurrentTheme.SectionBackgroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        GroupBox3.ForeColor = CurrentTheme.ForegroundColor
        GroupBox4.ForeColor = CurrentTheme.ForegroundColor
        ListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        StatusStrip1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        ListBox1.ForeColor = ForeColor
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        ToolStripStatusLabel1.Text = "Please specify the naming pattern of the SWM files"
                    Case "ESN"
                        ToolStripStatusLabel1.Text = "Especifique la nomenclatura del patrón de los archivos SWM"
                    Case "FRA"
                        ToolStripStatusLabel1.Text = "Veuillez spécifier le modèle de dénomination des fichiers SWM"
                    Case "PTB", "PTG"
                        ToolStripStatusLabel1.Text = "Especifique o padrão de nomenclatura dos ficheiros SWM"
                    Case "ITA"
                        ToolStripStatusLabel1.Text = "Specificare il modello di denominazione dei file SWM"
                End Select
            Case 1
                ToolStripStatusLabel1.Text = "Please specify the naming pattern of the SWM files"
            Case 2
                ToolStripStatusLabel1.Text = "Especifique la nomenclatura del patrón de los archivos SWM"
            Case 3
                ToolStripStatusLabel1.Text = "Veuillez spécifier le modèle de dénomination des fichiers SWM"
            Case 4
                ToolStripStatusLabel1.Text = "Especifique o padrão de nomenclatura dos ficheiros SWM"
            Case 5
                ToolStripStatusLabel1.Text = "Specificare il modello di denominazione dei file SWM"
        End Select
        If MainForm.SourceImg = "N/A" Or Not File.Exists(MainForm.SourceImg) Or MainForm.OnlineManagement Or MainForm.OfflineManagement Then
            UseMountedImgBtn.Enabled = False
        Else
            UseMountedImgBtn.Enabled = True
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then GetIndexes(TextBox1.Text) Else Exit Sub
        If TextBox1.Text.EndsWith(".swm") Then
            CheckBox4.Checked = True
            Button4.PerformClick()
        End If
    End Sub

    Sub GetIndexes(ImgFile As String)
        DynaLog.LogMessage("Mounted image detector might be busy. Stopping it if it is...")
        MainForm.MountedImageDetectorBWRestarterTimer.Enabled = False
        MainForm.StopMountedImageDetector()
        Dim imgInfo As DismImageInfoCollection = Nothing
        ComboBox1.Items.Clear()
        ImageVersions.Clear()
        ImageEditions.Clear()
        Try
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            imgInfo = DismApi.GetImageInfo(TextBox1.Text)
            DynaLog.LogMessage("Information collection count: " & imgInfo.Count)
            If imgInfo.Count > 0 Then
                DynaLog.LogMessage("Getting indexes and names...")
                For Each imageInfo In imgInfo
                    ComboBox1.Items.Add(imageInfo.ImageIndex & " (" & imageInfo.ImageName & ")")
                    ImageVersions.Add(imageInfo.ProductVersion)
                    ImageEditions.Add(imageInfo.EditionId)
                Next
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
            Dim msg As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "Could not gather information of this image file. Reason:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case "ESN"
                            msg = "No pudimos obtener información de este archivo de imagen. Razón:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case "FRA"
                            msg = "Impossible de recueillir des informations sur ce fichier de l'image. Raison :" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case "PTB", "PTG"
                            msg = "Não foi possível recolher informações sobre este ficheiro de imagem. Motivo:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case "ITA"
                            msg = "Impossibile raccogliere informazioni sull'immagine. Motivo:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                    End Select
                Case 1
                    msg = "Could not gather information of this image file. Reason:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 2
                    msg = "No pudimos obtener información de este archivo de imagen. Razón:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 3
                    msg = "Impossible de recueillir des informations sur ce fichier de l'image. Raison :" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 4
                    msg = "Não foi possível recolher informações sobre este ficheiro de imagem. Motivo:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 5
                    msg = "Impossibile raccogliere informazioni sull'immagine. Motivo:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
        Finally
            Try
                DynaLog.LogMessage("Shutting down API...")
                DismApi.Shutdown()
            Catch ex As DismException
                ' Don't do anything
            End Try
        End Try
        MainForm.StartMountedImageDetector()
        If ComboBox1.Items.Count > 0 Then
            ComboBox1.SelectedIndex = 0
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        FolderBrowserDialog1.ShowDialog(Me)
        If DialogResult.OK Then
            TextBox2.Text = FolderBrowserDialog1.SelectedPath
        Else
            TextBox2.Text = ""
        End If
    End Sub

    Private Sub UseMountedImgBtn_Click(sender As Object, e As EventArgs) Handles UseMountedImgBtn.Click
        TextBox1.Text = MainForm.SourceImg
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        TextBox4.Text = Path.GetFileNameWithoutExtension(TextBox1.Text)
        ScanSwmPattern(TextBox4.Text)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ScanSwmPattern(TextBox4.Text)
    End Sub

    Sub ScanSwmPattern(PatternName As String)
        DynaLog.LogMessage("Preparing to scan files with the specified pattern...")
        DynaLog.LogMessage("- Scan pattern: " & PatternName)
        ListBox1.Items.Clear()
        If TextBox1.Text = "" Or PatternName = "" Then
            DynaLog.LogMessage("Either no source image file has been specified or no pattern has been specified.")
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("Please specify a source WIM file. This will let you use the SWM files for later image application", vbOKOnly + vbCritical, "Apply an image")
                            ToolStripStatusLabel1.Text = "This naming pattern returns " & ListBox1.Items.Count & " SWM files"
                        Case "ESN"
                            MsgBox("Especifique el arhivo WIM de origen. Esto le permitirá usar los archivos SWM para la aplicación posterior de la imagen", vbOKOnly + vbCritical, "Aplicar una imagen")
                            ToolStripStatusLabel1.Text = "Esta nomenclatura de patrón devuelve " & ListBox1.Items.Count & " archivos SWM"
                        Case "FRA"
                            MsgBox("Veuillez indiquer un fichier WIM original. Cela vous permettra d'utiliser les fichiers SWM pour une application d'image ultérieure.", vbOKOnly + vbCritical, "Appliquer une image")
                            ToolStripStatusLabel1.Text = "Ce modèle de dénomination renvoie " & ListBox1.Items.Count & " fichiers SWM"
                        Case "PTB", "PTG"
                            MsgBox("Especifique um ficheiro WIM de origem. Isto permitir-lhe-á utilizar os ficheiros SWM para uma aplicação de imagem posterior", vbOKOnly + vbCritical, "Aplicar uma imagem")
                            ToolStripStatusLabel1.Text = "Este padrão de nomenclatura devolve " & ListBox1.Items.Count & " ficheiros SWM"
                        Case "ITA"
                            MsgBox("Specificare un file WIM di origine. In questo modo sarà possibile utilizzare i file SWM per una successiva applicazione di immagini", vbOKOnly + vbCritical, "Applica un'immagine")
                            ToolStripStatusLabel1.Text = "Questo modello di denominazione restituisce " & ListBox1.Items.Count & " file SWM"
                    End Select
                Case 1
                    MsgBox("Please specify a source WIM file. This will let you use the SWM files for later image application", vbOKOnly + vbCritical, "Apply an image")
                    ToolStripStatusLabel1.Text = "This naming pattern returns " & ListBox1.Items.Count & " SWM files"
                Case 2
                    MsgBox("Especifique el arhivo WIM de origen. Esto le permitirá usar los archivos SWM para la aplicación posterior de la imagen", vbOKOnly + vbCritical, "Aplicar una imagen")
                    ToolStripStatusLabel1.Text = "Esta nomenclatura de patrón devuelve " & ListBox1.Items.Count & " archivos SWM"
                Case 3
                    MsgBox("Veuillez indiquer un fichier WIM original. Cela vous permettra d'utiliser les fichiers SWM pour une application d'image ultérieure.", vbOKOnly + vbCritical, "Appliquer une image")
                    ToolStripStatusLabel1.Text = "Ce modèle de dénomination renvoie " & ListBox1.Items.Count & " fichiers SWM"
                Case 4
                    MsgBox("Especifique um ficheiro WIM de origem. Isto permitir-lhe-á utilizar os ficheiros SWM para uma aplicação de imagem posterior", vbOKOnly + vbCritical, "Aplicar uma imagem")
                    ToolStripStatusLabel1.Text = "Este padrão de nomenclatura devolve " & ListBox1.Items.Count & " ficheiros SWM"
                Case 5
                    MsgBox("Specificare un file WIM di origine. In questo modo sarà possibile utilizzare i file SWM per una successiva applicazione di immagini", vbOKOnly + vbCritical, "Applica un'immagine")
                    ToolStripStatusLabel1.Text = "Questo modello di denominazione restituisce " & ListBox1.Items.Count & " file SWM"
            End Select
            Beep()
            Exit Sub
        End If
        DynaLog.LogMessage("Scanning SWM files with given pattern...")
        For Each swmFile In My.Computer.FileSystem.GetFiles(Path.GetDirectoryName(TextBox1.Text), FileIO.SearchOption.SearchTopLevelOnly, "*.swm")
            If Path.GetFileNameWithoutExtension(swmFile).StartsWith(PatternName) Then
                ListBox1.Items.Add(Path.GetFileName(swmFile))
            End If
        Next
        DynaLog.LogMessage("Pattern search results: " & ListBox1.Items.Count)
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        ToolStripStatusLabel1.Text = "This naming pattern returns " & ListBox1.Items.Count & " SWM files"
                    Case "ESN"
                        ToolStripStatusLabel1.Text = "Esta nomenclatura de patrón devuelve " & ListBox1.Items.Count & " archivos SWM"
                    Case "FRA"
                        ToolStripStatusLabel1.Text = "Ce modèle de dénomination renvoie " & ListBox1.Items.Count & " fichiers SWM"
                    Case "PTB", "PTG"
                        ToolStripStatusLabel1.Text = "Este padrão de nomenclatura devolve " & ListBox1.Items.Count & " ficheiros SWM"
                    Case "ITA"
                        ToolStripStatusLabel1.Text = "Questo modello di denominazione restituisce " & ListBox1.Items.Count & " file SWM"
                End Select
            Case 1
                ToolStripStatusLabel1.Text = "This naming pattern returns " & ListBox1.Items.Count & " SWM files"
            Case 2
                ToolStripStatusLabel1.Text = "Esta nomenclatura de patrón devuelve " & ListBox1.Items.Count & " archivos SWM"
            Case 3
                ToolStripStatusLabel1.Text = "Ce modèle de dénomination renvoie " & ListBox1.Items.Count & " fichiers SWM"
            Case 4
                ToolStripStatusLabel1.Text = "Este padrão de nomenclatura devolve " & ListBox1.Items.Count & " ficheiros SWM"
            Case 5
                ToolStripStatusLabel1.Text = "Questo modello di denominazione restituisce " & ListBox1.Items.Count & " file SWM"
        End Select
        If ListBox1.Items.Count <= 0 Then Beep()
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        SWMFilePanel.Enabled = CheckBox4.Checked = True
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Try
            If (ImageVersions.Count > 0) AndAlso (ImageEditions.Count > 0) Then
                DynaLog.LogMessage("Comparing Edition ID and version of selected image...")
                ' Windows PE 4.0 (based on Windows 8 - NT 6.2.9200)
                If ImageEditions(ComboBox1.SelectedIndex).Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) AndAlso ImageVersions(ComboBox1.SelectedIndex) >= New Version(6, 2, 9200, 0) Then
                    DynaLog.LogMessage("This is a Windows PE 4+ image. Trusted Desktop validation can be carried out.")
                    CheckBox5.Enabled = True
                Else
                    DynaLog.LogMessage("This is not a Windows PE 4+ image. Trusted Desktop validation cannot be carried out.")
                    CheckBox5.Enabled = False
                End If
                If ImageVersions(ComboBox1.SelectedIndex).Build = 9600 Then
                    DynaLog.LogMessage("The image that is being serviced contains Windows 8.1. It supports WIMBoot.")
                    CheckBox6.Enabled = True
                Else
                    DynaLog.LogMessage("The image that is being serviced does not contain Windows 8.1. It does not support WIMBoot.")
                    CheckBox6.Enabled = False
                End If
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
            CheckBox5.Enabled = False
            CheckBox6.Enabled = False
        End Try
    End Sub
End Class
