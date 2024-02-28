Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.Dism
Imports System.Threading

Public Class ImgSwmToWim

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
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
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
            GroupBox3.ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            NumericUpDown1.BackColor = Color.FromArgb(31, 31, 31)
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
            GroupBox3.ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            NumericUpDown1.BackColor = Color.FromArgb(238, 238, 242)
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        NumericUpDown1.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://linustechtips.com/topic/1318158-merge-two-swm-files/")
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
        End If
    End Sub
End Class
