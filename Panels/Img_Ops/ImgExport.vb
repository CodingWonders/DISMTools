Imports System.Windows.Forms
Imports Microsoft.Dism
Imports System.IO
Imports System.Threading

Public Class ImgExport

    Dim CompressionTypeStrings() As String = New String(3) {"No compression will be applied to the destination image.", "Fast compression will be applied. This is the default option.", "Maximum compression will be applied. This will take the most time, but will result in a smaller image.", "The compression level for push-button reset images will be applied. This requires exporting the image as an ESD file."}
    Dim originalFileFilters As String = "WIM files|*.wim|ESD files|*.esd"

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            If Path.GetExtension(TextBox1.Text).EndsWith("swm", StringComparison.OrdinalIgnoreCase) Then
                ProgressPanel.imgExportSourceImage = TextBox1.Text.Replace(Path.GetFileNameWithoutExtension(TextBox1.Text), _
                                                                           Path.GetFileNameWithoutExtension(TextBox1.Text) & "*")
            Else
                ProgressPanel.imgExportSourceImage = TextBox1.Text
            End If
        Else
            Dim msg As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "Please specify a source image file to export and try again"
                        Case "ESN"
                            msg = "Por favor, especifique un archivo de imagen de origen e inténtelo de nuevo"
                        Case "FRA"
                            msg = "Veuillez indiquer un fichier d'image source à exporter et réessayez."
                        Case "PTB", "PTG"
                            msg = "Especifique um ficheiro de imagem de origem para exportar e tente novamente"
                    End Select
                Case 1
                    msg = "Please specify a source image file to export and try again"
                Case 2
                    msg = "Por favor, especifique un archivo de imagen de origen e inténtelo de nuevo"
                Case 3
                    msg = "Veuillez indiquer un fichier d'image source à exporter et réessayez."
                Case 4
                    msg = "Especifique um ficheiro de imagem de origem para exportar e tente novamente"
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If
        If TextBox2.Text <> "" Then
            ProgressPanel.imgExportDestinationImage = TextBox2.Text
        Else
            Dim msg As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "Please specify a destination image file and try again"
                        Case "ESN"
                            msg = "Por favor, especifique un archivo de imagen de destino e intente de nuevo"
                        Case "FRA"
                            msg = "Veuillez spécifier un fichier d'image de destination et réessayer"
                        Case "PTB", "PTG"
                            msg = "Especifique um ficheiro de imagem de destino e tente novamente"
                    End Select
                Case 1
                    msg = "Please specify a destination image file and try again"
                Case 2
                    msg = "Por favor, especifique un archivo de imagen de destino e intente de nuevo"
                Case 3
                    msg = "Veuillez spécifier un fichier d'image de destination et réessayer"
                Case 4
                    msg = "Especifique um ficheiro de imagem de destino e tente novamente"
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If
        ProgressPanel.imgExportSourceIndex = NumericUpDown1.Value
        ProgressPanel.imgExportDestinationUseCustomName = CheckBox2.Checked
        If CheckBox2.Checked Then
            If TextBox3.Text <> "" Then
                ProgressPanel.imgExportDestinationName = TextBox3.Text
            Else
                ProgressPanel.imgExportDestinationName = ""
                ProgressPanel.imgExportDestinationUseCustomName = False
            End If
        End If
        ProgressPanel.imgExportCompressType = ComboBox1.SelectedIndex
        ProgressPanel.imgExportMarkBootable = CheckBox3.Checked
        ProgressPanel.imgExportUseWimBoot = CheckBox4.Checked
        ProgressPanel.imgExportCheckIntegrity = CheckBox5.Checked
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 10
        Visible = False
        ProgressPanel.ShowDialog(Me)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        TextBox2.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub ImgExport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Export an image"
                        Label1.Text = Text
                        Label2.Text = "Destination image file:"
                        Label3.Text = "Source image file:"
                        Label4.Text = "Naming pattern:"
                        Label5.Text = "Destination image compression type:"
                        Label7.Text = "Source image index:"
                        CheckBox1.Text = "Reference SWM files"
                        CheckBox2.Text = "Specify a custom name for the destination image"
                        CheckBox3.Text = "Make image bootable (Windows PE only)"
                        CheckBox4.Text = "Append image with WIMBoot configuration"
                        CheckBox5.Text = "Check integrity before exporting image"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        Button1.Text = "Browse..."
                        Button2.Text = "Browse..."
                        Button4.Text = "Use name of the image"
                        Button5.Text = "Scan pattern"
                        GroupBox1.Text = "Sources and destinations"
                        GroupBox2.Text = "Options"
                        OpenFileDialog1.Title = "Specify a source image file to export"
                        ListView1.Columns(0).Text = "Index"
                        ListView1.Columns(1).Text = "Image name"
                        ListView1.Columns(2).Text = "Image description"
                        ListView1.Columns(3).Text = "Image version"
                        CompressionTypeStrings(0) = "No compression will be applied to the destination image."
                        CompressionTypeStrings(1) = "Fast compression will be applied. This is the default option."
                        CompressionTypeStrings(2) = "Maximum compression will be applied. This will take the most time, but will result in a smaller image."
                        CompressionTypeStrings(3) = "The compression level for push-button reset images will be applied. This requires exporting the image as an ESD file."
                    Case "ESN"
                        Text = "Exportar una imagen"
                        Label1.Text = Text
                        Label2.Text = "Archivo de imagen de destino:"
                        Label3.Text = "Archivo de imagen de origen:"
                        Label4.Text = "Patrón de nomenclatura:"
                        Label5.Text = "Tipo de compresión de la imagen de destino:"
                        Label7.Text = "Índice de imagen de origen:"
                        CheckBox1.Text = "Hacer referencia a archivos SWM"
                        CheckBox2.Text = "Especificar un nombre personalizado para la imagen de destino"
                        CheckBox3.Text = "Hacer imagen arrancable (solo Windows PE)"
                        CheckBox4.Text = "Exportar la imagen con configuración WIMBoot"
                        CheckBox5.Text = "Comprobar integridad antes de exportar la imagen"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Examinar..."
                        Button4.Text = "Usar nombre de imagen"
                        Button5.Text = "Escanear patrón"
                        GroupBox1.Text = "Orígenes y destinos"
                        GroupBox2.Text = "Opciones"
                        OpenFileDialog1.Title = "Especifique un archivo de imagen de origen a exportar"
                        ListView1.Columns(0).Text = "Índice"
                        ListView1.Columns(1).Text = "Nombre de imagen"
                        ListView1.Columns(2).Text = "Descripción de imagen"
                        ListView1.Columns(3).Text = "Versión de imagen"
                        CompressionTypeStrings(0) = "No se aplicará compresión a la imagen de destino."
                        CompressionTypeStrings(1) = "Se aplicará compresión rápida. Esta es la opción predeterminada."
                        CompressionTypeStrings(2) = "Se aplicará compresión máxima. Esto tardará más tiempo, pero resultará en una imagen más pequeña."
                        CompressionTypeStrings(3) = "Se aplicará el nivel de compresión de imágenes de restablecimiento por botón. Esto requiere exportar la imagen como un archivo ESD."
                    Case "FRA"
                        Text = "Exporter une image"
                        Label1.Text = Text
                        Label2.Text = "Fichier d'image de destination :"
                        Label3.Text = "Fichier d'image source :"
                        Label4.Text = "Modèle de dénomination :"
                        Label5.Text = "Type de compression de l'image de destination :"
                        Label7.Text = "Index de l'image source :"
                        CheckBox1.Text = "Référence aux fichiers SWM"
                        CheckBox2.Text = "Spécifier un nom personnalisé pour l'image de destination"
                        CheckBox3.Text = "Rendre l'image démarrable (Windows PE uniquement)"
                        CheckBox4.Text = "Ajouter la configuration WIMBoot à l'image"
                        CheckBox5.Text = "Vérifier l'intégrité avant d'exporter l'image"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        Button1.Text = "Parcourir..."
                        Button2.Text = "Parcourir..."
                        Button4.Text = "Utiliser le nom de l'image"
                        Button5.Text = "Scanner modèle"
                        GroupBox1.Text = "Sources et destinations"
                        GroupBox2.Text = "Paramètres"
                        OpenFileDialog1.Title = "Spécifier un fichier image source à exporter"
                        ListView1.Columns(0).Text = "Index"
                        ListView1.Columns(1).Text = "Nom de l'image"
                        ListView1.Columns(2).Text = "Description de l'image"
                        ListView1.Columns(3).Text = "Version de l'image"
                        CompressionTypeStrings(0) = "Aucune compression ne sera appliquée à l'image de destination."
                        CompressionTypeStrings(1) = "Une compression rapide sera appliquée. C'est l'option par défaut."
                        CompressionTypeStrings(2) = "Une compression maximale sera appliquée. Cette option prend le plus de temps, mais permet d'obtenir une image plus petite."
                        CompressionTypeStrings(3) = "Le niveau de compression des images réinitialisées par bouton-poussoir sera appliqué. Cela nécessite l'exportation de l'image en tant que fichier ESD."
                    Case "PTB", "PTG"
                        Text = "Exportar uma imagem"
                        Label1.Text = Text
                        Label2.Text = "Ficheiro de imagem de destino:"
                        Label3.Text = "Ficheiro de imagem de origem:"
                        Label4.Text = "Padrão de nomenclatura:"
                        Label5.Text = "Tipo de compressão da imagem de destino:"
                        Label7.Text = "Índice da imagem de origem:"
                        CheckBox1.Text = "Ficheiros SWM de referência"
                        CheckBox2.Text = "Especificar um nome personalizado para a imagem de destino"
                        CheckBox3.Text = "Tornar a imagem de arranque (só para Windows PE)"
                        CheckBox4.Text = "Anexar imagem com a configuração WIMBoot"
                        CheckBox5.Text = "Verificar a integridade antes de exportar a imagem"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        Button1.Text = "Procurar..."
                        Button2.Text = "Procurar..."
                        Button4.Text = "Utilizar nome da imagem"
                        Button5.Text = "Examinar padrão"
                        GroupBox1.Text = "Origens e destinos"
                        GroupBox2.Text = "Configurações"
                        OpenFileDialog1.Title = "Especificar um ficheiro de imagem de origem para exportar"
                        ListView1.Columns(0).Text = "Índice"
                        ListView1.Columns(1).Text = "Nome da imagem"
                        ListView1.Columns(2).Text = "Descrição da imagem"
                        ListView1.Columns(3).Text = "Versão da imagem"
                        CompressionTypeStrings(0) = "Não será aplicada qualquer compressão à imagem de destino."
                        CompressionTypeStrings(1) = "Será aplicada uma compressão rápida. Esta é a opção predefinida."
                        CompressionTypeStrings(2) = "Será aplicada a compressão máxima. Esta opção demora mais tempo, mas resulta numa imagem mais pequena."
                        CompressionTypeStrings(3) = "Será aplicado o nível de compressão para imagens reiniciadas por botão de pressão. Para tal, é necessário exportar a imagem como um ficheiro ESD."
                End Select
            Case 1
                Text = "Export an image"
                Label1.Text = Text
                Label2.Text = "Destination image file:"
                Label3.Text = "Source image file:"
                Label4.Text = "Naming pattern:"
                Label5.Text = "Destination image compression type:"
                Label7.Text = "Source image index:"
                CheckBox1.Text = "Reference SWM files"
                CheckBox2.Text = "Specify a custom name for the destination image"
                CheckBox3.Text = "Make image bootable (Windows PE only)"
                CheckBox4.Text = "Append image with WIMBoot configuration"
                CheckBox5.Text = "Check integrity before exporting image"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                Button1.Text = "Browse..."
                Button2.Text = "Browse..."
                Button4.Text = "Use name of the image"
                Button5.Text = "Scan pattern"
                GroupBox1.Text = "Sources and destinations"
                GroupBox2.Text = "Options"
                OpenFileDialog1.Title = "Specify a source image file to export"
                ListView1.Columns(0).Text = "Index"
                ListView1.Columns(1).Text = "Image name"
                ListView1.Columns(2).Text = "Image description"
                ListView1.Columns(3).Text = "Image version"
                CompressionTypeStrings(0) = "No compression will be applied to the destination image."
                CompressionTypeStrings(1) = "Fast compression will be applied. This is the default option."
                CompressionTypeStrings(2) = "Maximum compression will be applied. This will take the most time, but will result in a smaller image."
                CompressionTypeStrings(3) = "The compression level for push-button reset images will be applied. This requires exporting the image as an ESD file."
            Case 2
                Text = "Exportar una imagen"
                Label1.Text = Text
                Label2.Text = "Archivo de imagen de destino:"
                Label3.Text = "Archivo de imagen de origen:"
                Label4.Text = "Patrón de nomenclatura:"
                Label5.Text = "Tipo de compresión de la imagen de destino:"
                Label7.Text = "Índice de imagen de origen:"
                CheckBox1.Text = "Hacer referencia a archivos SWM"
                CheckBox2.Text = "Especificar un nombre personalizado para la imagen de destino"
                CheckBox3.Text = "Hacer imagen arrancable (solo Windows PE)"
                CheckBox4.Text = "Exportar la imagen con configuración WIMBoot"
                CheckBox5.Text = "Comprobar integridad antes de exportar la imagen"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                Button1.Text = "Examinar..."
                Button2.Text = "Examinar..."
                Button4.Text = "Usar nombre de imagen"
                Button5.Text = "Escanear patrón"
                GroupBox1.Text = "Orígenes y destinos"
                GroupBox2.Text = "Opciones"
                OpenFileDialog1.Title = "Especifique un archivo de imagen de origen a exportar"
                ListView1.Columns(0).Text = "Índice"
                ListView1.Columns(1).Text = "Nombre de imagen"
                ListView1.Columns(2).Text = "Descripción de imagen"
                ListView1.Columns(3).Text = "Versión de imagen"
                CompressionTypeStrings(0) = "No se aplicará compresión a la imagen de destino."
                CompressionTypeStrings(1) = "Se aplicará compresión rápida. Esta es la opción predeterminada."
                CompressionTypeStrings(2) = "Se aplicará compresión máxima. Esto tardará más tiempo, pero resultará en una imagen más pequeña."
                CompressionTypeStrings(3) = "Se aplicará el nivel de compresión de imágenes de restablecimiento por botón. Esto requiere exportar la imagen como un archivo ESD."
            Case 3
                Text = "Exporter une image"
                Label1.Text = Text
                Label2.Text = "Fichier d'image de destination :"
                Label3.Text = "Fichier d'image source :"
                Label4.Text = "Modèle de dénomination :"
                Label5.Text = "Type de compression de l'image de destination :"
                Label7.Text = "Index de l'image source :"
                CheckBox1.Text = "Référence aux fichiers SWM"
                CheckBox2.Text = "Spécifier un nom personnalisé pour l'image de destination"
                CheckBox3.Text = "Rendre l'image démarrable (Windows PE uniquement)"
                CheckBox4.Text = "Ajouter la configuration WIMBoot à l'image"
                CheckBox5.Text = "Vérifier l'intégrité avant d'exporter l'image"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                Button1.Text = "Parcourir..."
                Button2.Text = "Parcourir..."
                Button4.Text = "Utiliser le nom de l'image"
                Button5.Text = "Scanner modèle"
                GroupBox1.Text = "Sources et destinations"
                GroupBox2.Text = "Paramètres"
                OpenFileDialog1.Title = "Spécifier un fichier image source à exporter"
                ListView1.Columns(0).Text = "Index"
                ListView1.Columns(1).Text = "Nom de l'image"
                ListView1.Columns(2).Text = "Description de l'image"
                ListView1.Columns(3).Text = "Version de l'image"
                CompressionTypeStrings(0) = "Aucune compression ne sera appliquée à l'image de destination."
                CompressionTypeStrings(1) = "Une compression rapide sera appliquée. C'est l'option par défaut."
                CompressionTypeStrings(2) = "Une compression maximale sera appliquée. Cette option prend le plus de temps, mais permet d'obtenir une image plus petite."
                CompressionTypeStrings(3) = "Le niveau de compression des images réinitialisées par bouton-poussoir sera appliqué. Cela nécessite l'exportation de l'image en tant que fichier ESD."
            Case 4
                Text = "Exportar uma imagem"
                Label1.Text = Text
                Label2.Text = "Ficheiro de imagem de destino:"
                Label3.Text = "Ficheiro de imagem de origem:"
                Label4.Text = "Padrão de nomenclatura:"
                Label5.Text = "Tipo de compressão da imagem de destino:"
                Label7.Text = "Índice da imagem de origem:"
                CheckBox1.Text = "Ficheiros SWM de referência"
                CheckBox2.Text = "Especificar um nome personalizado para a imagem de destino"
                CheckBox3.Text = "Tornar a imagem de arranque (só para Windows PE)"
                CheckBox4.Text = "Anexar imagem com a configuração WIMBoot"
                CheckBox5.Text = "Verificar a integridade antes de exportar a imagem"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                Button1.Text = "Procurar..."
                Button2.Text = "Procurar..."
                Button4.Text = "Utilizar nome da imagem"
                Button5.Text = "Examinar padrão"
                GroupBox1.Text = "Origens e destinos"
                GroupBox2.Text = "Configurações"
                OpenFileDialog1.Title = "Especificar um ficheiro de imagem de origem para exportar"
                ListView1.Columns(0).Text = "Índice"
                ListView1.Columns(1).Text = "Nome da imagem"
                ListView1.Columns(2).Text = "Descrição da imagem"
                ListView1.Columns(3).Text = "Versão da imagem"
                CompressionTypeStrings(0) = "Não será aplicada qualquer compressão à imagem de destino."
                CompressionTypeStrings(1) = "Será aplicada uma compressão rápida. Esta é a opção predefinida."
                CompressionTypeStrings(2) = "Será aplicada a compressão máxima. Esta opção demora mais tempo, mas resulta numa imagem mais pequena."
                CompressionTypeStrings(3) = "Será aplicado o nível de compressão para imagens reiniciadas por botão de pressão. Para tal, é necessário exportar a imagem como um ficheiro ESD."
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            TextBox3.BackColor = Color.FromArgb(31, 31, 31)
            TextBox4.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox1.BackColor = Color.FromArgb(31, 31, 31)
            NumericUpDown1.BackColor = Color.FromArgb(31, 31, 31)
            ListBox1.BackColor = Color.FromArgb(31, 31, 31)
            StatusStrip1.BackColor = Color.FromArgb(31, 31, 31)
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            TextBox3.BackColor = Color.FromArgb(238, 238, 242)
            TextBox4.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox1.BackColor = Color.FromArgb(238, 238, 242)
            NumericUpDown1.BackColor = Color.FromArgb(238, 238, 242)
            ListBox1.BackColor = Color.FromArgb(238, 238, 242)
            StatusStrip1.BackColor = Color.FromArgb(238, 238, 242)
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        NumericUpDown1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        ListBox1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
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
                End Select
            Case 1
                ToolStripStatusLabel1.Text = "Please specify the naming pattern of the SWM files"
            Case 2
                ToolStripStatusLabel1.Text = "Especifique la nomenclatura del patrón de los archivos SWM"
            Case 3
                ToolStripStatusLabel1.Text = "Veuillez spécifier le modèle de dénomination des fichiers SWM"
            Case 4
                ToolStripStatusLabel1.Text = "Especifique o padrão de nomenclatura dos ficheiros SWM"
        End Select
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SaveFileDialog1.ShowDialog()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            If MainForm.MountedImageDetectorBW.IsBusy Then
                MainForm.MountedImageDetectorBW.CancelAsync()
                While MainForm.MountedImageDetectorBW.IsBusy
                    Application.DoEvents()
                    Thread.Sleep(500)
                End While
            End If
            MainForm.WatcherTimer.Enabled = False
            If MainForm.WatcherBW.IsBusy Then MainForm.WatcherBW.CancelAsync()
            While MainForm.WatcherBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(100)
            End While
            Try
                ListView1.Items.Clear()
                DismApi.Initialize(DismLogLevel.LogErrors)
                Dim imgInfoCollection As DismImageInfoCollection = DismApi.GetImageInfo(TextBox1.Text)
                NumericUpDown1.Maximum = imgInfoCollection.Count
                For Each imgInfo As DismImageInfo In imgInfoCollection
                    ListView1.Items.Add(New ListViewItem(New String() {imgInfo.ImageIndex, imgInfo.ImageName, imgInfo.ImageDescription, imgInfo.ProductVersion.ToString()}))
                Next
            Catch ex As Exception
                MsgBox("Could not get index information for this image file", vbOKOnly + vbCritical, Label1.Text)
            Finally
                DismApi.Shutdown()
            End Try
            If File.Exists(TextBox1.Text) And Path.GetExtension(TextBox1.Text).EndsWith("swm", StringComparison.OrdinalIgnoreCase) Then
                CheckBox1.Checked = True
                Button4.PerformClick()
            End If
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        SWMFilePanel.Enabled = CheckBox1.Checked = True
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        TextBox3.Enabled = CheckBox2.Checked = True
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        SaveFileDialog1.Filter = originalFileFilters
        If ComboBox1.SelectedItem = "none" Then
            Label8.Text = CompressionTypeStrings(0)
        ElseIf ComboBox1.SelectedItem = "fast" Then
            Label8.Text = CompressionTypeStrings(1)
        ElseIf ComboBox1.SelectedItem = "maximum" Then
            Label8.Text = CompressionTypeStrings(2)
        ElseIf ComboBox1.SelectedItem = "recovery" Then
            Label8.Text = CompressionTypeStrings(3)
            ' If recovery is specified, the target image must be an ESD file
            SaveFileDialog1.Filter = "ESD files|*.esd"
            If TextBox2.Text <> "" Then
                ' Switch the extension of the target image file
                TextBox2.Text = TextBox2.Text.Replace(Path.GetExtension(TextBox2.Text), ".esd").Trim()
            End If
        End If
    End Sub

    Private Sub ImgExport_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        SaveFileDialog1.Filter = originalFileFilters
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        TextBox4.Text = Path.GetFileNameWithoutExtension(TextBox1.Text)
        ScanSwmPattern(TextBox4.Text)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ScanSwmPattern(TextBox4.Text)
    End Sub

    Sub ScanSwmPattern(PatternName As String)
        ListBox1.Items.Clear()
        If TextBox1.Text = "" Or PatternName = "" Then
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
            End Select
            Beep()
            Exit Sub
        End If
        For Each swmFile In My.Computer.FileSystem.GetFiles(Path.GetDirectoryName(TextBox1.Text), FileIO.SearchOption.SearchTopLevelOnly, "*.swm")
            If Path.GetFileNameWithoutExtension(swmFile).StartsWith(PatternName) Then
                ListBox1.Items.Add(Path.GetFileName(swmFile))
            End If
        Next
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
                End Select
            Case 1
                ToolStripStatusLabel1.Text = "This naming pattern returns " & ListBox1.Items.Count & " SWM files"
            Case 2
                ToolStripStatusLabel1.Text = "Esta nomenclatura de patrón devuelve " & ListBox1.Items.Count & " archivos SWM"
            Case 3
                ToolStripStatusLabel1.Text = "Ce modèle de dénomination renvoie " & ListBox1.Items.Count & " fichiers SWM"
            Case 4
                ToolStripStatusLabel1.Text = "Este padrão de nomenclatura devolve " & ListBox1.Items.Count & " ficheiros SWM"
        End Select
        If ListBox1.Items.Count <= 0 Then Beep()
    End Sub
End Class
