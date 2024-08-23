Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class MUMAdditionDialog

    Public MUMFile As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub MUMAdditionDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Add update manifest"
                        Label1.Text = "This dialog lets you add a Microsoft Update Manifest (MUM) file to the target image. You can only specify one at a time." & CrLf & CrLf &
                            "Do note that this is for advanced use only and may compromise the target Windows image."
                        Label2.Text = "Path of the manifest file to add:"
                        Button1.Text = "Browse..."
                        Cancel_Button.Text = "Cancel"
                        OK_Button.Text = "OK"
                    Case "ESN"
                        Text = "Añadir manifiesto de actualización"
                        Label1.Text = "Este diálogo le permite añadir un archivo de manifiesto de actualización de Microsoft (MUM) a la imagen de destino. Solo puede especificar uno a la vez." & CrLf & CrLf &
                            "Dese cuenta de que esto es solo para usos avanzados y que podría dañar la imagen de Windows de destino."
                        Label2.Text = "Ubicación del archivo de manifesto a añadir:"
                        Button1.Text = "Examinar..."
                        Cancel_Button.Text = "Cancelar"
                        OK_Button.Text = "Aceptar"
                    Case "FRA"
                        Text = "Ajouter un manifeste de mise à jour"
                        Label1.Text = "Cette boîte de dialogue vous permet d'ajouter un fichier Microsoft Update Manifest (MUM) à l'image cible. Vous ne pouvez en spécifier qu'un seul à la fois." & CrLf & CrLf &
                            "Notez que cette opération est réservée à un usage avancé et qu'elle peut compromettre l'image Windows cible."
                        Label2.Text = "Chemin du fichier manifeste à ajouter :"
                        Button1.Text = "Parcourir..."
                        Cancel_Button.Text = "Annuler"
                        OK_Button.Text = "OK"
                    Case "PTB", "PTG"
                        Text = "Adicionar manifesto de atualização"
                        Label1.Text = "Esta caixa de diálogo permite-lhe adicionar um ficheiro Microsoft Update Manifest (MUM) à imagem de destino. Só pode especificar um de cada vez." & CrLf & CrLf &
                            "Tenha em atenção que isto é apenas para utilização avançada e pode comprometer a imagem de destino do Windows."
                        Label2.Text = "Caminho do ficheiro de manifesto a adicionar:"
                        Button1.Text = "Procurar..."
                        Cancel_Button.Text = "Cancelar"
                        OK_Button.Text = "OK"
                    Case "ITA"
                        Text = "Aggiungi manifesto di aggiornamento"
                        Label1.Text = "Questa finestra di dialogo consente di aggiungere un file Microsoft Update Manifest (MUM) all'immagine di destinazione. È possibile specificarne solo uno alla volta." & CrLf & CrLf &
                            "Si noti che questa operazione è riservata a un uso avanzato e può compromettere l'immagine di Windows di destinazione."
                        Label2.Text = "Percorso del file manifest da aggiungere:"
                        Button1.Text = "Sfoglia..."
                        Cancel_Button.Text = "Annullare"
                        OK_Button.Text = "OK"
                End Select
            Case 1
                Text = "Add update manifest"
                Label1.Text = "This dialog lets you add a Microsoft Update Manifest (MUM) file to the target image. You can only specify one at a time." & CrLf & CrLf &
                    "Do note that this is for advanced use only and may compromise the target Windows image."
                Label2.Text = "Path of the manifest file to add:"
                Button1.Text = "Browse..."
                Cancel_Button.Text = "Cancel"
                OK_Button.Text = "OK"
            Case 2
                Text = "Añadir manifiesto de actualización"
                Label1.Text = "Este diálogo le permite añadir un archivo de manifiesto de actualización de Microsoft (MUM) a la imagen de destino. Solo puede especificar uno a la vez." & CrLf & CrLf &
                    "Dese cuenta de que esto es solo para usos avanzados y que podría dañar la imagen de Windows de destino."
                Label2.Text = "Ubicación del archivo de manifesto a añadir:"
                Button1.Text = "Examinar..."
                Cancel_Button.Text = "Cancelar"
                OK_Button.Text = "Aceptar"
            Case 3
                Text = "Ajouter un manifeste de mise à jour"
                Label1.Text = "Cette boîte de dialogue vous permet d'ajouter un fichier Microsoft Update Manifest (MUM) à l'image cible. Vous ne pouvez en spécifier qu'un seul à la fois." & CrLf & CrLf &
                    "Notez que cette opération est réservée à un usage avancé et qu'elle peut compromettre l'image Windows cible."
                Label2.Text = "Chemin du fichier manifeste à ajouter :"
                Button1.Text = "Parcourir..."
                Cancel_Button.Text = "Annuler"
                OK_Button.Text = "OK"
            Case 4
                Text = "Adicionar manifesto de atualização"
                Label1.Text = "Esta caixa de diálogo permite-lhe adicionar um ficheiro Microsoft Update Manifest (MUM) à imagem de destino. Só pode especificar um de cada vez." & CrLf & CrLf &
                    "Tenha em atenção que isto é apenas para utilização avançada e pode comprometer a imagem de destino do Windows."
                Label2.Text = "Caminho do ficheiro de manifesto a adicionar:"
                Button1.Text = "Procurar..."
                Cancel_Button.Text = "Cancelar"
                OK_Button.Text = "OK"
            Case 5
                Text = "Aggiungi manifesto di aggiornamento"
                Label1.Text = "Questa finestra di dialogo consente di aggiungere un file Microsoft Update Manifest (MUM) all'immagine di destinazione. È possibile specificarne solo uno alla volta." & CrLf & CrLf &
                    "Si noti che questa operazione è riservata a un uso avanzato e può compromettere l'immagine di Windows di destinazione."
                Label2.Text = "Percorso del file manifest da aggiungere:"
                Button1.Text = "Sfoglia..."
                Cancel_Button.Text = "Annullare"
                OK_Button.Text = "OK"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        MUMFile = OpenFileDialog1.FileName
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub
End Class
