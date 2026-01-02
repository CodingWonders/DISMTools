Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.Dism
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars

Public Class ImgSwmToWim

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.imgSwmSource = TextBox1.Text
        ProgressPanel.imgMergerIndex = NumericUpDown1.Value
        ProgressPanel.imgWimDestination = TextBox2.Text
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 992
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SaveFileDialog1.ShowDialog()
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        TextBox2.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub ImgSwmToWim_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Merge SWM files"
                        Label1.Text = Text
                        Label2.Text = "Source SWM file:"
                        Label3.Text = "NOTE: when specifying the SWM file, choose the first file. DISMTools will take care of additional SWM files stored in that directory."
                        Label4.Text = "Destination WIM file:"
                        Label5.Text = "Index:"
                        Button1.Text = "Browse..."
                        Button2.Text = "Browse..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        LinkLabel1.Text = "Learn how to do it"
                        ListView1.Columns(0).Text = "Index"
                        ListView1.Columns(1).Text = "Image name"
                        ListView1.Columns(2).Text = "Image description"
                        ListView1.Columns(3).Text = "Image version"
                        OpenFileDialog1.Title = "Specify the source SWM file to merge"
                        SaveFileDialog1.Title = "Specify the destination WIM file to merge the source SWM files to"
                    Case "ESN"
                        Text = "Combinar archivos SWM"
                        Label1.Text = Text
                        Label2.Text = "Archivo SWM de origen:"
                        Label3.Text = "NOTA: al especificar el archivo SWM, escoja el primer archivo. DISMTools se encargará de los archivos SWM adicionales en ese directorio."
                        Label4.Text = "Archivo WIM de destino:"
                        Label5.Text = "Índice:"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Examinar..."
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        LinkLabel1.Text = "Aprenda cómo hacerlo"
                        ListView1.Columns(0).Text = "Índice"
                        ListView1.Columns(1).Text = "Nombre de imagen"
                        ListView1.Columns(2).Text = "Descripción de imagen"
                        ListView1.Columns(3).Text = "Versión de imagen"
                        OpenFileDialog1.Title = "Especifique el archivo SWM de origen a combinar"
                        SaveFileDialog1.Title = "Especifique el archivo WIM de destino al que combinar los archivos SWM"
                    Case "FRA"
                        Text = "Fusionner des fichiers SWM"
                        Label1.Text = Text
                        Label2.Text = "Fichier SWM source :"
                        Label3.Text = "NOTE : lorsque vous spécifiez le fichier SWM, choisissez le premier fichier. DISMTools s'occupera des fichiers SWM supplémentaires stockés dans ce répertoire."
                        Label4.Text = "Fichier WIM de destination :"
                        Label5.Text = "Index :"
                        Button1.Text = "Parcourir..."
                        Button2.Text = "Parcourir..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        LinkLabel1.Text = "Apprendre à le faire"
                        ListView1.Columns(0).Text = "Index"
                        ListView1.Columns(1).Text = "Nom de l'image"
                        ListView1.Columns(2).Text = "Description de l'image"
                        ListView1.Columns(3).Text = "Version de l'image"
                        OpenFileDialog1.Title = "Spécifier le fichier SWM source à fusionner"
                        SaveFileDialog1.Title = "Spécifier le fichier WIM de destination dans lequel fusionner les fichiers SWM sources"
                    Case "PTB", "PTG"
                        Text = "Combinar ficheiros SWM"
                        Label1.Text = Text
                        Label2.Text = "Ficheiro SWM de origem:"
                        Label3.Text = "NOTA: ao especificar o arquivo SWM, escolha o primeiro arquivo. DISMTools cuidará dos arquivos SWM adicionais armazenados nesse diretório."
                        Label4.Text = "Ficheiro WIM de destino:"
                        Label5.Text = "Índice:"
                        Button1.Text = "Navegar..."
                        Button2.Text = "Navegar..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        LinkLabel1.Text = "Saiba como o fazer"
                        ListView1.Columns(0).Text = "Índice"
                        ListView1.Columns(1).Text = "Nome da imagem"
                        ListView1.Columns(2).Text = "Descrição da imagem"
                        ListView1.Columns(3).Text = "Versão da imagem"
                        OpenFileDialog1.Title = "Especificar o ficheiro SWM de origem a combinar"
                        SaveFileDialog1.Title = "Especificar o ficheiro WIM de destino para combinar os ficheiros SWM de origem"
                    Case "ITA"
                        Text = "Unire i file SWM"
                        Label1.Text = Text
                        Label2.Text = "File SWM di origine:"
                        Label3.Text = "NOTA: quando si specifica il file SWM, scegliere il primo file. DISMTools si occuperà dei file SWM aggiuntivi memorizzati in quella directory."
                        Label4.Text = "File WIM di destinazione:"
                        Label5.Text = "Indice:"
                        Button1.Text = "Sfoglia..."
                        Button2.Text = "Sfoglia..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        LinkLabel1.Text = "Impara come si fa"
                        ListView1.Columns(0).Text = "Indice"
                        ListView1.Columns(1).Text = "Nome dell'immagine"
                        ListView1.Columns(2).Text = "Descrizione dell'immagine"
                        ListView1.Columns(3).Text = "Versione dell'immagine"
                        OpenFileDialog1.Title = "Specificare il file SWM di origine da unire"
                        SaveFileDialog1.Title = "Specificare il file WIM di destinazione in cui unire i file SWM di origine"
                End Select
            Case 1
                Text = "Merge SWM files"
                Label1.Text = Text
                Label2.Text = "Source SWM file:"
                Label3.Text = "NOTE: when specifying the SWM file, choose the first file. DISMTools will take care of additional SWM files stored in that directory."
                Label4.Text = "Destination WIM file:"
                Label5.Text = "Index:"
                Button1.Text = "Browse..."
                Button2.Text = "Browse..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                LinkLabel1.Text = "Learn how to do it"
                ListView1.Columns(0).Text = "Index"
                ListView1.Columns(1).Text = "Image name"
                ListView1.Columns(2).Text = "Image description"
                ListView1.Columns(3).Text = "Image version"
                OpenFileDialog1.Title = "Specify the source SWM file to merge"
                SaveFileDialog1.Title = "Specify the destination WIM file to merge the source SWM files to"
            Case 2
                Text = "Combinar archivos SWM"
                Label1.Text = Text
                Label2.Text = "Archivo SWM de origen:"
                Label3.Text = "NOTA: al especificar el archivo SWM, escoja el primer archivo. DISMTools se encargará de los archivos SWM adicionales en ese directorio."
                Label4.Text = "Archivo WIM de destino:"
                Label5.Text = "Índice:"
                Button1.Text = "Examinar..."
                Button2.Text = "Examinar..."
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                LinkLabel1.Text = "Aprenda cómo hacerlo"
                ListView1.Columns(0).Text = "Índice"
                ListView1.Columns(1).Text = "Nombre de imagen"
                ListView1.Columns(2).Text = "Descripción de imagen"
                ListView1.Columns(3).Text = "Versión de imagen"
                OpenFileDialog1.Title = "Especifique el archivo SWM de origen a combinar"
                SaveFileDialog1.Title = "Especifique el archivo WIM de destino al que combinar los archivos SWM"
            Case 3
                Text = "Fusionner des fichiers SWM"
                Label1.Text = Text
                Label2.Text = "Fichier SWM source :"
                Label3.Text = "NOTE : lorsque vous spécifiez le fichier SWM, choisissez le premier fichier. DISMTools s'occupera des fichiers SWM supplémentaires stockés dans ce répertoire."
                Label4.Text = "Fichier WIM de destination :"
                Label5.Text = "Index :"
                Button1.Text = "Parcourir..."
                Button2.Text = "Parcourir..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                LinkLabel1.Text = "Apprendre à le faire"
                ListView1.Columns(0).Text = "Index"
                ListView1.Columns(1).Text = "Nom de l'image"
                ListView1.Columns(2).Text = "Description de l'image"
                ListView1.Columns(3).Text = "Version de l'image"
                OpenFileDialog1.Title = "Spécifier le fichier SWM source à fusionner"
                SaveFileDialog1.Title = "Spécifier le fichier WIM de destination dans lequel fusionner les fichiers SWM sources"
            Case 4
                Text = "Combinar ficheiros SWM"
                Label1.Text = Text
                Label2.Text = "Ficheiro SWM de origem:"
                Label3.Text = "NOTA: ao especificar o arquivo SWM, escolha o primeiro arquivo. DISMTools cuidará dos arquivos SWM adicionais armazenados nesse diretório."
                Label4.Text = "Ficheiro WIM de destino:"
                Label5.Text = "Índice:"
                Button1.Text = "Navegar..."
                Button2.Text = "Navegar..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                LinkLabel1.Text = "Saiba como o fazer"
                ListView1.Columns(0).Text = "Índice"
                ListView1.Columns(1).Text = "Nome da imagem"
                ListView1.Columns(2).Text = "Descrição da imagem"
                ListView1.Columns(3).Text = "Versão da imagem"
                OpenFileDialog1.Title = "Especificar o ficheiro SWM de origem a combinar"
                SaveFileDialog1.Title = "Especificar o ficheiro WIM de destino para combinar os ficheiros SWM de origem"
            Case 5
                Text = "Unire i file SWM"
                Label1.Text = Text
                Label2.Text = "File SWM di origine:"
                Label3.Text = "NOTA: quando si specifica il file SWM, scegliere il primo file. DISMTools si occuperà dei file SWM aggiuntivi memorizzati in quella directory."
                Label4.Text = "File WIM di destinazione:"
                Label5.Text = "Indice:"
                Button1.Text = "Sfoglia..."
                Button2.Text = "Sfoglia..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                LinkLabel1.Text = "Impara come si fa"
                ListView1.Columns(0).Text = "Indice"
                ListView1.Columns(1).Text = "Nome dell'immagine"
                ListView1.Columns(2).Text = "Descrizione dell'immagine"
                ListView1.Columns(3).Text = "Versione dell'immagine"
                OpenFileDialog1.Title = "Specificare il file SWM di origine da unire"
                SaveFileDialog1.Title = "Specificare il file WIM di destinazione in cui unire i file SWM di origine"
        End Select
        Win10Title.BackColor = CurrentTheme.BackgroundColor
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        GroupBox3.ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        NumericUpDown1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://linustechtips.com/topic/1318158-merge-two-swm-files/")
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("Getting and displaying information of specified image file...")
            DynaLog.LogMessage("Image file to get information about: " & Quote & TextBox1.Text & Quote)
            MainForm.StopMountedImageDetector()
            Try
                DynaLog.LogMessage("Getting information about the image file...")
                ListView1.Items.Clear()
                DynaLog.LogMessage("Initializing API...")
                DismApi.Initialize(DismLogLevel.LogErrors)
                Dim imgInfoCollection As DismImageInfoCollection = DismApi.GetImageInfo(TextBox1.Text)
                DynaLog.LogMessage("Information collection count: " & imgInfoCollection.Count)
                NumericUpDown1.Maximum = imgInfoCollection.Count
                ListView1.Items.AddRange(imgInfoCollection.Select(Function(imgInfo) New ListViewItem(New String() {imgInfo.ImageIndex, imgInfo.ImageName, imgInfo.ImageDescription, imgInfo.ProductVersion.ToString()})).ToArray())
            Catch ex As Exception
                DynaLog.LogMessage("Could not get image file information. Error message: " & ex.Message)
                MsgBox("Could not get index information for this image file", vbOKOnly + vbCritical, Label1.Text)
            Finally
                DynaLog.LogMessage("Shutting down API...")
                Try
                    DismApi.Shutdown()
                Catch ex As Exception
                    ' Don't do anything
                End Try
            End Try
            DynaLog.LogMessage("This process has finished.")
        End If
    End Sub
End Class
