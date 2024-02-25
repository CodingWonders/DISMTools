Imports System.Windows.Forms
Imports System.IO

Public Class ImgCapture

    Dim CompressionTypeStrings() As String = New String(2) {"No compression will be applied to the destination image.", "Fast compression will be applied. This is the default option.", "Maximum compression will be applied. This will take the most time, but will result in a smaller image."}

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.CaptureSourceDir = TextBox1.Text
        ProgressPanel.CaptureDestinationImage = TextBox2.Text
        ProgressPanel.CaptureName = TextBox3.Text
        ProgressPanel.CaptureDescription = TextBox4.Text
        If CheckBox1.Checked Then
            ProgressPanel.CaptureWimScriptConfig = TextBox5.Text
        Else
            ProgressPanel.CaptureWimScriptConfig = ""
        End If
        Select Case ComboBox1.SelectedIndex
            Case 0
                ProgressPanel.CaptureCompressType = 0
            Case 1
                ProgressPanel.CaptureCompressType = 1
            Case 2
                ProgressPanel.CaptureCompressType = 2
        End Select
        If CheckBox2.Checked Then
            ProgressPanel.CaptureBootable = True
        Else
            ProgressPanel.CaptureBootable = False
        End If
        If CheckBox3.Checked Then
            ProgressPanel.CaptureCheckIntegrity = True
        Else
            ProgressPanel.CaptureCheckIntegrity = False
        End If
        If CheckBox4.Checked Then
            ProgressPanel.CaptureVerify = True
        Else
            ProgressPanel.CaptureVerify = False
        End If
        If CheckBox5.Checked Then
            ProgressPanel.CaptureReparsePt = True
        Else
            ProgressPanel.CaptureReparsePt = False
        End If
        If CheckBox6.Checked Then
            ProgressPanel.CaptureUseWimBoot = True
        Else
            ProgressPanel.CaptureUseWimBoot = False
        End If
        If CheckBox7.Checked Then
            ProgressPanel.CaptureExtendedAttributes = True
        Else
            ProgressPanel.CaptureExtendedAttributes = False
        End If
        If CheckBox8.Checked Then
            ProgressPanel.CaptureMountDestImg = True
            ' Since ProgressPanel doesn't consider what other form variables contain, set them to ProgressPanel variables
            ProgressPanel.UMountLocalDir = True
            ProgressPanel.RandomMountDir = ""
            ProgressPanel.MountDir = MainForm.MountDir
            ProgressPanel.UMountOp = 1
            ProgressPanel.CheckImgIntegrity = False
            ProgressPanel.SaveToNewIndex = False
            ProgressPanel.SourceImg = ProgressPanel.CaptureDestinationImage
            ProgressPanel.isOptimized = False
            ProgressPanel.isIntegrityTested = False
            ProgressPanel.TaskList.AddRange({6, 21, 15})
        Else
            ProgressPanel.CaptureMountDestImg = False
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 6
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgCapture_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Capture an image"
                        Label1.Text = Text
                        Label2.Text = "Destination image file:"
                        Label3.Text = "Source image directory:"
                        Label4.Text = "Destination image description:"
                        Label5.Text = "Destination image name:"
                        Label6.Text = "Path of configuration file:"
                        Label7.Text = "Destination image compression type:"
                        GroupBox1.Text = "Sources and destinations"
                        GroupBox2.Text = "Options"
                        Button1.Text = "Browse..."
                        Button2.Text = "Browse..."
                        Button3.Text = "Browse..."
                        Button5.Text = "Create..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        CheckBox1.Text = "Exclude certain files and directories for destination image"
                        CheckBox2.Text = "Make image bootable (Windows PE only)"
                        CheckBox3.Text = "Verify image integrity"
                        CheckBox4.Text = "Check for file errors"
                        CheckBox5.Text = "Use the reparse point tag fix"
                        CheckBox6.Text = "Append with WIMBoot configuration"
                        CheckBox7.Text = "Capture extended attributes"
                        CheckBox8.Text = "Mount destination image for later use"
                        CompressionTypeStrings(0) = "No compression will be applied to the destination image."
                        CompressionTypeStrings(1) = "Fast compression will be applied. This is the default option."
                        CompressionTypeStrings(2) = "Maximum compression will be applied. This will take the most time, but will result in a smaller image."
                    Case "ESN"
                        Text = "Capturar una imagen"
                        Label1.Text = Text
                        Label2.Text = "Archivo de imagen de destino:"
                        Label3.Text = "Directorio de imagen de origen:"
                        Label4.Text = "Descripción de la imagen de destino:"
                        Label5.Text = "Nombre de la imagen de destino:"
                        Label6.Text = "Ubicación de archivo de configuración:"
                        Label7.Text = "Tipo de compresión de imagen de destino:"
                        GroupBox1.Text = "Orígenes y destinos"
                        GroupBox2.Text = "Opciones"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Examinar..."
                        Button3.Text = "Examinar..."
                        Button5.Text = "Crear..."
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        CheckBox1.Text = "Excluir algunos archivos y directorios para la imagen de destino"
                        CheckBox2.Text = "Hacer imagen arrancable (solo Windows PE)"
                        CheckBox3.Text = "Verificar integridad de imagen"
                        CheckBox4.Text = "Comprobar errores de archivos"
                        CheckBox5.Text = "Utilizar corrección de etiquetas de puntos de repetición de análisis"
                        CheckBox6.Text = "Anexar con configuración WIMBoot"
                        CheckBox7.Text = "Capturar atributos extendidos"
                        CheckBox8.Text = "Montar imagen de destino para posterior uso"
                        CompressionTypeStrings(0) = "No se aplicará compresión a la imagen de destino."
                        CompressionTypeStrings(1) = "Se aplicará compresión rápida. Esta es la opción predeterminada."
                        CompressionTypeStrings(2) = "Se aplicará compresión máxima. Esto tardará más tiempo, pero resultará en una imagen más pequeña."
                    Case "FRA"
                        Text = "Capturer une image"
                        Label1.Text = Text
                        Label2.Text = "Fichier de l'image de destination :"
                        Label3.Text = "Répertoire de l'image source :"
                        Label4.Text = "Description de l'image de destination :"
                        Label5.Text = "Nom de l'image de destination :"
                        Label6.Text = "Emplacement du fichier de configuration :"
                        Label7.Text = "Type de compression de l'image de destination :"
                        GroupBox1.Text = "Sources et destinations"
                        GroupBox2.Text = "Paramètres"
                        Button1.Text = "Parcourir..."
                        Button2.Text = "Parcourir..."
                        Button3.Text = "Parcourir..."
                        Button5.Text = "Créer..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        CheckBox1.Text = "Exclure certains fichiers et répertoires pour l'image de destination"
                        CheckBox2.Text = "Rendre l'image amorçable (Windows PE uniquement)"
                        CheckBox3.Text = "Vérifier l'intégrité de l'image"
                        CheckBox4.Text = "Vérifier les erreurs de fichiers"
                        CheckBox5.Text = "Utiliser la correction de la balise reparse"
                        CheckBox6.Text = "Ajouter la configuration WIMBoot"
                        CheckBox7.Text = "Capturer les attributs étendus"
                        CheckBox8.Text = "Monter l'image de destination pour une utilisation ultérieure"
                        CompressionTypeStrings(0) = "Aucune compression ne sera appliquée à l'image de destination."
                        CompressionTypeStrings(1) = "Une compression rapide sera appliquée. Il s'agit du paramètre par défaut."
                        CompressionTypeStrings(2) = "La compression maximale sera appliquée. C'est ce qui prendra le plus de temps, mais l'image sera plus petite."
                    Case "PTB", "PTG"
                        Text = "Capturar uma imagem"
                        Label1.Text = Text
                        Label2.Text = "Ficheiro de imagem de destino:"
                        Label3.Text = "Diretório da imagem de origem:"
                        Label4.Text = "Descrição da imagem de destino:"
                        Label5.Text = "Nome da imagem de destino:"
                        Label6.Text = "Localização do ficheiro de configuração:"
                        Label7.Text = "Tipo de compressão da imagem de destino:"
                        GroupBox1.Text = "Origens e destinos"
                        GroupBox2.Text = "Opções"
                        Button1.Text = "Navegar..."
                        Button2.Text = "Navegar..."
                        Button3.Text = "Navegar..."
                        Button5.Text = "Criar..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        CheckBox1.Text = "Excluir determinados ficheiros e directórios para a imagem de destino"
                        CheckBox2.Text = "Tornar a imagem inicializável (somente Windows PE)"
                        CheckBox3.Text = "Verificar a integridade da imagem"
                        CheckBox4.Text = "Verificar se existem erros nos ficheiros"
                        CheckBox5.Text = "Utilizar a correção da etiqueta de ponto de reparação"
                        CheckBox6.Text = "Anexar com a configuração WIMBoot"
                        CheckBox7.Text = "Capturar atributos estendidos"
                        CheckBox8.Text = "Montar a imagem de destino para utilização posterior"
                        CompressionTypeStrings(0) = "Não será aplicada qualquer compressão à imagem de destino."
                        CompressionTypeStrings(1) = "Será aplicada uma compressão rápida. Esta é a opção predefinida."
                        CompressionTypeStrings(2) = "Será aplicada a compressão máxima. Esta opção demora mais tempo, mas resulta numa imagem mais pequena."
                End Select
            Case 1
                Text = "Capture an image"
                Label1.Text = Text
                Label2.Text = "Destination image file:"
                Label3.Text = "Source image directory:"
                Label4.Text = "Destination image description:"
                Label5.Text = "Destination image name:"
                Label6.Text = "Path of configuration file:"
                Label7.Text = "Destination image compression type:"
                GroupBox1.Text = "Sources and destinations"
                GroupBox2.Text = "Options"
                Button1.Text = "Browse..."
                Button2.Text = "Browse..."
                Button3.Text = "Browse..."
                Button5.Text = "Create..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                CheckBox1.Text = "Exclude certain files and directories for destination image"
                CheckBox2.Text = "Make image bootable (Windows PE only)"
                CheckBox3.Text = "Verify image integrity"
                CheckBox4.Text = "Check for file errors"
                CheckBox5.Text = "Use the reparse point tag fix"
                CheckBox6.Text = "Append with WIMBoot configuration"
                CheckBox7.Text = "Capture extended attributes"
                CheckBox8.Text = "Mount destination image for later use"
                CompressionTypeStrings(0) = "No compression will be applied to the destination image."
                CompressionTypeStrings(1) = "Fast compression will be applied. This is the default option."
                CompressionTypeStrings(2) = "Maximum compression will be applied. This will take the most time, but will result in a smaller image."
            Case 2
                Text = "Capturar una imagen"
                Label1.Text = Text
                Label2.Text = "Archivo de imagen de destino:"
                Label3.Text = "Directorio de imagen de origen:"
                Label4.Text = "Descripción de la imagen de destino:"
                Label5.Text = "Nombre de la imagen de destino:"
                Label6.Text = "Ubicación de archivo de configuración:"
                Label7.Text = "Tipo de compresión de imagen de destino:"
                GroupBox1.Text = "Orígenes y destinos"
                GroupBox2.Text = "Opciones"
                Button1.Text = "Examinar..."
                Button2.Text = "Examinar..."
                Button3.Text = "Examinar..."
                Button5.Text = "Crear..."
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                CheckBox1.Text = "Excluir algunos archivos y directorios para la imagen de destino"
                CheckBox2.Text = "Hacer imagen arrancable (solo Windows PE)"
                CheckBox3.Text = "Verificar integridad de imagen"
                CheckBox4.Text = "Comprobar errores de archivos"
                CheckBox5.Text = "Utilizar corrección de etiquetas de puntos de repetición de análisis"
                CheckBox6.Text = "Anexar con configuración WIMBoot"
                CheckBox7.Text = "Capturar atributos extendidos"
                CheckBox8.Text = "Montar imagen de destino para posterior uso"
                CompressionTypeStrings(0) = "No se aplicará compresión a la imagen de destino."
                CompressionTypeStrings(1) = "Se aplicará compresión rápida. Esta es la opción predeterminada."
                CompressionTypeStrings(2) = "Se aplicará compresión máxima. Esto tardará más tiempo, pero resultará en una imagen más pequeña."
            Case 3
                Text = "Capturer une image"
                Label1.Text = Text
                Label2.Text = "Fichier de l'image de destination :"
                Label3.Text = "Répertoire de l'image source :"
                Label4.Text = "Description de l'image de destination :"
                Label5.Text = "Nom de l'image de destination :"
                Label6.Text = "Emplacement du fichier de configuration :"
                Label7.Text = "Type de compression de l'image de destination :"
                GroupBox1.Text = "Sources et destinations"
                GroupBox2.Text = "Paramètres"
                Button1.Text = "Parcourir..."
                Button2.Text = "Parcourir..."
                Button3.Text = "Parcourir..."
                Button5.Text = "Créer..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                CheckBox1.Text = "Exclure certains fichiers et répertoires pour l'image de destination"
                CheckBox2.Text = "Rendre l'image amorçable (Windows PE uniquement)"
                CheckBox3.Text = "Vérifier l'intégrité de l'image"
                CheckBox4.Text = "Vérifier les erreurs de fichiers"
                CheckBox5.Text = "Utiliser la correction de la balise reparse"
                CheckBox6.Text = "Ajouter la configuration WIMBoot"
                CheckBox7.Text = "Capturer les attributs étendus"
                CheckBox8.Text = "Monter l'image de destination pour une utilisation ultérieure"
                CompressionTypeStrings(0) = "Aucune compression ne sera appliquée à l'image de destination."
                CompressionTypeStrings(1) = "Une compression rapide sera appliquée. Il s'agit du paramètre par défaut."
                CompressionTypeStrings(2) = "La compression maximale sera appliquée. C'est ce qui prendra le plus de temps, mais l'image sera plus petite."
            Case 4
                Text = "Capturar uma imagem"
                Label1.Text = Text
                Label2.Text = "Ficheiro de imagem de destino:"
                Label3.Text = "Diretório da imagem de origem:"
                Label4.Text = "Descrição da imagem de destino:"
                Label5.Text = "Nome da imagem de destino:"
                Label6.Text = "Localização do ficheiro de configuração:"
                Label7.Text = "Tipo de compressão da imagem de destino:"
                GroupBox1.Text = "Origens e destinos"
                GroupBox2.Text = "Opções"
                Button1.Text = "Navegar..."
                Button2.Text = "Navegar..."
                Button3.Text = "Navegar..."
                Button5.Text = "Criar..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                CheckBox1.Text = "Excluir determinados ficheiros e directórios para a imagem de destino"
                CheckBox2.Text = "Tornar a imagem inicializável (somente Windows PE)"
                CheckBox3.Text = "Verificar a integridade da imagem"
                CheckBox4.Text = "Verificar se existem erros nos ficheiros"
                CheckBox5.Text = "Utilizar a correção da etiqueta de ponto de reparação"
                CheckBox6.Text = "Anexar com a configuração WIMBoot"
                CheckBox7.Text = "Capturar atributos estendidos"
                CheckBox8.Text = "Montar a imagem de destino para utilização posterior"
                CompressionTypeStrings(0) = "Não será aplicada qualquer compressão à imagem de destino."
                CompressionTypeStrings(1) = "Será aplicada uma compressão rápida. Esta é a opção predefinida."
                CompressionTypeStrings(2) = "Será aplicada a compressão máxima. Esta opção demora mais tempo, mas resulta numa imagem mais pequena."
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            TextBox3.BackColor = Color.FromArgb(31, 31, 31)
            TextBox4.BackColor = Color.FromArgb(31, 31, 31)
            TextBox5.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox1.BackColor = Color.FromArgb(31, 31, 31)
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            TextBox3.BackColor = Color.FromArgb(238, 238, 242)
            TextBox4.BackColor = Color.FromArgb(238, 238, 242)
            TextBox5.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox1.BackColor = Color.FromArgb(238, 238, 242)
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
        End If
        ComboBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        TextBox5.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        If MainForm.OnlineManagement Or MainForm.OfflineManagement Then
            CheckBox8.Enabled = False
        Else
            CheckBox8.Enabled = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog()
        If DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        Else
            TextBox1.Text = ""
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SaveFileDialog1.ShowDialog()
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        TextBox2.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Label6.Enabled = True
            TextBox5.Enabled = True
            Button3.Enabled = True
            Button5.Enabled = True
        Else
            Label6.Enabled = False
            TextBox5.Enabled = False
            Button3.Enabled = False
            Button5.Enabled = False
        End If
        GatherValidFields()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox5.Text = OpenFileDialog1.FileName
    End Sub

    Sub GatherValidFields()
        If CheckBox1.Checked Then
            If Directory.Exists(TextBox1.Text) And TextBox2.Text IsNot "" And TextBox3.Text IsNot "" And TextBox5.Text IsNot "" Then
                OK_Button.Enabled = True
            Else
                OK_Button.Enabled = False
            End If
        Else
            If Directory.Exists(TextBox1.Text) And TextBox2.Text IsNot "" And TextBox3.Text IsNot "" Then
                OK_Button.Enabled = True
            Else
                OK_Button.Enabled = False
            End If
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        GatherValidFields()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        GatherValidFields()
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        GatherValidFields()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "none" Then
            Label8.Text = CompressionTypeStrings(0)
        ElseIf ComboBox1.SelectedItem = "fast" Then
            Label8.Text = CompressionTypeStrings(1)
        ElseIf ComboBox1.SelectedItem = "maximum" Then
            Label8.Text = CompressionTypeStrings(2)
        End If
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        GatherValidFields()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Visible = False
        ' Make it so that it can only close
        WimScriptEditor.MinimizeBox = False
        WimScriptEditor.MaximizeBox = False
        WimScriptEditor.ShowDialog(MainForm)
        If File.Exists(WimScriptEditor.ConfigListFile) Then
            TextBox5.Text = WimScriptEditor.ConfigListFile
        End If
        Visible = True
    End Sub
End Class
