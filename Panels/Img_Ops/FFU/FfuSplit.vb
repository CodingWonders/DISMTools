Imports System.Windows.Forms
Imports System.IO

Public Class FfuSplit

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("The source FFU file has been specified and exists in the file system.")
            ProgressPanel.SFUSplitSourceFile = TextBox1.Text
            ProgressPanel.SFUSplitFileSize = NumericUpDown1.Value
            If TextBox2.Text <> "" And Directory.Exists(Path.GetDirectoryName(TextBox2.Text)) Then
                DynaLog.LogMessage("A target file has been specified and its directory exists in the file system.")
                ProgressPanel.SFUSplitTargetFile = TextBox2.Text
            Else
                DynaLog.LogMessage("Either no target file has been specified or its directory does not exist in the file system.")
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MsgBox("Please specify a name and path for the target SFU file and try again. Also, make sure that the target path exists.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                            Case "ESN"
                                MsgBox("Especifique un nombre y un directorio para el archivo SFU de destino e inténtelo de nuevo. Asegúrese también de que el directorio de destino exista.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                            Case "FRA"
                                MsgBox("Veuillez indiquer un nom et un chemin pour le fichier SFU cible et réessayez. Assurez-vous également que le chemin d'accès à la cible existe.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                            Case "PTB", "PTG"
                                MsgBox("Especifique um nome e uma localização para o ficheiro SFU de destino e tente novamente. Além disso, certifique-se de que o caminho de destino existe.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                            Case "ITA"
                                MsgBox("Specificare un nome e un percorso per il file SFU di destinazione e riprovare. Assicurarsi inoltre che il percorso di destinazione esista.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        End Select
                    Case 1
                        MsgBox("Please specify a name and path for the target SFU file and try again. Also, make sure that the target path exists.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    Case 2
                        MsgBox("Especifique un nombre y un directorio para el archivo SFU de destino e inténtelo de nuevo. Asegúrese también de que el directorio de destino exista.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    Case 3
                        MsgBox("Veuillez indiquer un nom et un chemin pour le fichier SFU cible et réessayez. Assurez-vous également que le chemin d'accès à la cible existe.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    Case 4
                        MsgBox("Especifique um nome e uma localização para o ficheiro SFU de destino e tente novamente. Além disso, certifique-se de que o caminho de destino existe.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    Case 5
                        MsgBox("Specificare un nome e un percorso per il file SFU di destinazione e riprovare. Assicurarsi inoltre che il percorso di destinazione esista.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                End Select
                Exit Sub
            End If
            ProgressPanel.SFUSplitCheckIntegrity = CheckBox1.Checked
        Else
            DynaLog.LogMessage("Either no source FFU file has been specified or it does not exist in the file system.")
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("Please specify a source FFU file and try again. Also, make sure that it exists.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case "ESN"
                            MsgBox("Especifique un archivo FFU de origen e inténtelo de nuevo. Asegúrese también de que el archivo exista.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case "FRA"
                            MsgBox("Veuillez indiquer un fichier FFU source et réessayer. Assurez-vous également qu'il existe.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case "PTB", "PTG"
                            MsgBox("Especifique um ficheiro FFU de origem e tente novamente. Além disso, certifique-se de que ele existe.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                        Case "ITA"
                            MsgBox("Specificare un file FFU di origine e riprovare. Assicurarsi inoltre che esista", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    End Select
                Case 1
                    MsgBox("Please specify a source FFU file and try again. Also, make sure that it exists.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Case 2
                    MsgBox("Especifique un archivo FFU de origen e inténtelo de nuevo. Asegúrese también de que el archivo exista.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Case 3
                    MsgBox("Veuillez indiquer un fichier FFU source et réessayer. Assurez-vous également qu'il existe.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Case 4
                    MsgBox("Especifique um ficheiro FFU de origem e tente novamente. Além disso, certifique-se de que ele existe.", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                Case 5
                    MsgBox("Specificare un file FFU di origine e riprovare. Assicurarsi inoltre che esista", vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
            End Select
            Exit Sub
        End If
        ProgressPanel.OperationNum = 19
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub FfuSplit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Split FFU images"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Source image to split:"
                        Label3.Text = "Name and path of the destination split image:"
                        Label4.Text = "Maximum size of split images (in MB):"
                        Label5.Text = "Do note that, to accommodate a large file in the image, a split image file may be larger than the specified value"
                        Button1.Text = "Browse..."
                        Button2.Text = "Browse..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        CheckBox1.Text = "Check image integrity"
                        OpenFileDialog1.Title = "Specify the source FFU file to split:"
                        SaveFileDialog1.Title = "Specify the target location of the split images:"
                    Case "ESN"
                        Text = "Dividir imágenes FFU"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Imagen de origen a dividir:"
                        Label3.Text = "Nombre y ruta de la imagen dividida de destino:"
                        Label4.Text = "Tamaño máximo de imágenes divididas (en MB):"
                        Label5.Text = "Para acomodar un archivo grande de la imagen, una imagen dividida puede ocupar más tamaño del especificado"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Examinar..."
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        CheckBox1.Text = "Comprobar integridad de la imagen"
                        OpenFileDialog1.Title = "Especifique el archivo FFU de origen a dividir:"
                        SaveFileDialog1.Title = "Especifique la ubicación de destino de las imágenes divididas:"
                    Case "FRA"
                        Text = "Diviser les images FFU"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Image source à diviser :"
                        Label3.Text = "Nom et chemin de l'image divisée de destination :"
                        Label4.Text = "Taille maximale des images fractionnées (en Mo) :"
                        Label5.Text = "Notez que, pour tenir compte d'un fichier volumineux dans l'image, un fichier d'image divisé peut être plus grand que la valeur spécifiée."
                        Button1.Text = "Parcourir..."
                        Button2.Text = "Parcourir..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        CheckBox1.Text = "Vérifier l'intégrité de l'image"
                        OpenFileDialog1.Title = "Spécifiez le fichier FFU source à diviser :"
                        SaveFileDialog1.Title = "Spécifiez l'emplacement cible des images divisées :"
                    Case "PTB", "PTG"
                        Text = "Dividir imagens FFU"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Imagem de origem a dividir:"
                        Label3.Text = "Nome e caminho da imagem dividida de destino:"
                        Label4.Text = "Tamanho máximo das imagens divididas (em MB):"
                        Label5.Text = "Tenha em atenção que, para acomodar um ficheiro grande na imagem, um ficheiro de imagem dividida pode ser maior do que o valor especificado"
                        Button1.Text = "Navegar..."
                        Button2.Text = "Navegar..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        CheckBox1.Text = "Verificar a integridade da imagem"
                        OpenFileDialog1.Title = "Especificar o ficheiro FFU de origem a dividir:"
                        SaveFileDialog1.Title = "Especificar a localização de destino das imagens divididas:"
                    Case "ITA"
                        Text = "Dividere le immagini FFU"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Immagine sorgente da dividere:"
                        Label3.Text = "Nome e percorso dell'immagine di destinazione da dividere:"
                        Label4.Text = "Dimensione massima delle immagini divise (in MB):"
                        Label5.Text = "Tenere presente che, per ospitare un file di grandi dimensioni nell'immagine, un file di immagine divisa può essere più grande del valore specificato"
                        Button1.Text = "Sfoglia..."
                        Button2.Text = "Sfoglia..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annulla"
                        CheckBox1.Text = "Controlla l'integrità dell'immagine"
                        OpenFileDialog1.Title = "Specificare il file FFU di origine da dividere:"
                        SaveFileDialog1.Title = "Specificare la posizione di destinazione delle immagini divise:"
                End Select
            Case 1
                Text = "Split FFU images"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Source image to split:"
                Label3.Text = "Name and path of the destination split image:"
                Label4.Text = "Maximum size of split images (in MB):"
                Label5.Text = "Do note that, to accommodate a large file in the image, a split image file may be larger than the specified value"
                Button1.Text = "Browse..."
                Button2.Text = "Browse..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                CheckBox1.Text = "Check image integrity"
                OpenFileDialog1.Title = "Specify the source FFU file to split:"
                SaveFileDialog1.Title = "Specify the target location of the split images:"
            Case 2
                Text = "Dividir imágenes FFU"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Imagen de origen a dividir:"
                Label3.Text = "Nombre y ruta de la imagen dividida de destino:"
                Label4.Text = "Tamaño máximo de imágenes divididas (en MB):"
                Label5.Text = "Para acomodar un archivo grande de la imagen, una imagen dividida puede ocupar más tamaño del especificado"
                Button1.Text = "Examinar..."
                Button2.Text = "Examinar..."
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                CheckBox1.Text = "Comprobar integridad de la imagen"
                OpenFileDialog1.Title = "Especifique el archivo FFU de origen a dividir:"
                SaveFileDialog1.Title = "Especifique la ubicación de destino de las imágenes divididas:"
            Case 3
                Text = "Diviser les images FFU"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Image source à diviser :"
                Label3.Text = "Nom et chemin de l'image divisée de destination :"
                Label4.Text = "Taille maximale des images fractionnées (en Mo) :"
                Label5.Text = "Notez que, pour tenir compte d'un fichier volumineux dans l'image, un fichier d'image divisé peut être plus grand que la valeur spécifiée."
                Button1.Text = "Parcourir..."
                Button2.Text = "Parcourir..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                CheckBox1.Text = "Vérifier l'intégrité de l'image"
                OpenFileDialog1.Title = "Spécifiez le fichier FFU source à diviser :"
                SaveFileDialog1.Title = "Spécifiez l'emplacement cible des images divisées :"
            Case 4
                Text = "Dividir imagens FFU"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Imagem de origem a dividir:"
                Label3.Text = "Nome e caminho da imagem dividida de destino:"
                Label4.Text = "Tamanho máximo das imagens divididas (em MB):"
                Label5.Text = "Tenha em atenção que, para acomodar um ficheiro grande na imagem, um ficheiro de imagem dividida pode ser maior do que o valor especificado"
                Button1.Text = "Navegar..."
                Button2.Text = "Navegar..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                CheckBox1.Text = "Verificar a integridade da imagem"
                OpenFileDialog1.Title = "Especificar o ficheiro FFU de origem a dividir:"
                SaveFileDialog1.Title = "Especificar a localização de destino das imagens divididas:"
            Case 5
                Text = "Dividere le immagini FFU"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Immagine sorgente da dividere:"
                Label3.Text = "Nome e percorso dell'immagine di destinazione da dividere:"
                Label4.Text = "Dimensione massima delle immagini divise (in MB):"
                Label5.Text = "Tenere presente che, per ospitare un file di grandi dimensioni nell'immagine, un file di immagine divisa può essere più grande del valore specificato"
                Button1.Text = "Sfoglia..."
                Button2.Text = "Sfoglia..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annulla"
                CheckBox1.Text = "Controlla l'integrità dell'immagine"
                OpenFileDialog1.Title = "Specificare il file FFU di origine da dividere:"
                SaveFileDialog1.Title = "Specificare la posizione di destinazione delle immagini divise:"
        End Select
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        NumericUpDown1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then SaveFileDialog1.FileName = Path.GetFileNameWithoutExtension(TextBox1.Text) & "_"
        SaveFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        DynaLog.LogMessage("Source FFU file specified: " & Quote & OpenFileDialog1.FileName & Quote)
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        DynaLog.LogMessage("Target SFU file specified: " & Quote & SaveFileDialog1.FileName & Quote)
        TextBox2.Text = SaveFileDialog1.FileName
    End Sub
End Class
