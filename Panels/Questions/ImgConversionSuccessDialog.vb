Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class ImgConversionSuccessDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgConversionSuccessDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label1.Text = "The image has been successfully converted"
                        Label2.Text = "The specified image has been successfully converted to the target format. For convenience, the File Explorer can be opened for you to see where the target image is located." & CrLf & CrLf & "Do you want to open the directory where the target image is stored?"
                        OK_Button.Text = "Yes"
                        Cancel_Button.Text = "No"
                    Case "ESN"
                        Label1.Text = "La imagen ha sido convertida satisfactoriamente"
                        Label2.Text = "La imagen especificada ha sido convertida satisfactoriamente al formato de destino. Por si lo desea, el Explorador de archivos puede ser abierto para ver dónde está ubicada la imagen." & CrLf & CrLf & "¿Desea abrir el directorio donde la imagen de destino está almacenada?"
                        OK_Button.Text = "Sí"
                        Cancel_Button.Text = "No"
                    Case "FRA"
                        Label1.Text = "L'image a été convertie avec succès"
                        Label2.Text = "L'image spécifiée a été convertie avec succès au format cible. Pour plus de commodité, l'explorateur de fichiers peut être ouvert pour vous permettre de voir où se trouve l'image cible." & CrLf & CrLf & "Voulez-vous ouvrir le répertoire dans lequel l'image cible est stockée ?"
                        OK_Button.Text = "Oui"
                        Cancel_Button.Text = "Non"
                    Case "PTB", "PTG"
                        Label1.Text = "A imagem foi convertida com êxito"
                        Label2.Text = "A imagem especificada foi convertida com sucesso para o formato de destino. Por conveniência, o Explorador de Ficheiros pode ser aberto para ver onde se encontra a imagem de destino." & CrLf & CrLf & "Pretende abrir o diretório onde a imagem de destino está armazenada?"
                        OK_Button.Text = "Sim"
                        Cancel_Button.Text = "Não"
                    Case "ITA"
                        Label1.Text = "L'immagine è stata convertita con successo"
                        Label2.Text = "L'immagine specificata è stata convertita con successo nel formato di destinazione. Per comodità, è possibile aprire l'Esplora file per vedere dove si trova l'immagine di destinazione." & CrLf & CrLf & "Si desidera aprire la directory in cui è memorizzata l'immagine di destinazione?"
                        OK_Button.Text = "Sì"
                        Cancel_Button.Text = "No"
                End Select
            Case 1
                Label1.Text = "The image has been successfully converted"
                Label2.Text = "The specified image has been successfully converted to the target format. For convenience, the File Explorer can be opened for you to see where the target image is located." & CrLf & CrLf & "Do you want to open the directory where the target image is stored?"
                OK_Button.Text = "Yes"
                Cancel_Button.Text = "No"
            Case 2
                Label1.Text = "La imagen ha sido convertida satisfactoriamente"
                Label2.Text = "La imagen especificada ha sido convertida satisfactoriamente al formato de destino. Por si lo desea, el Explorador de archivos puede ser abierto para ver dónde está ubicada la imagen." & CrLf & CrLf & "¿Desea abrir el directorio donde la imagen de destino está almacenada?"
                OK_Button.Text = "Sí"
                Cancel_Button.Text = "No"
            Case 3
                Label1.Text = "L'image a été convertie avec succès"
                Label2.Text = "L'image spécifiée a été convertie avec succès au format cible. Pour plus de commodité, l'explorateur de fichiers peut être ouvert pour vous permettre de voir où se trouve l'image cible." & CrLf & CrLf & "Voulez-vous ouvrir le répertoire dans lequel l'image cible est stockée ?"
                OK_Button.Text = "Oui"
                Cancel_Button.Text = "Non"
            Case 4
                Label1.Text = "A imagem foi convertida com êxito"
                Label2.Text = "A imagem especificada foi convertida com sucesso para o formato de destino. Por conveniência, o Explorador de Ficheiros pode ser aberto para ver onde se encontra a imagem de destino." & CrLf & CrLf & "Pretende abrir o diretório onde a imagem de destino está armazenada?"
                OK_Button.Text = "Sim"
                Cancel_Button.Text = "Não"
            Case 5
                Label1.Text = "L'immagine è stata convertita con successo"
                Label2.Text = "L'immagine specificata è stata convertita con successo nel formato di destinazione. Per comodità, è possibile aprire l'Esplora file per vedere dove si trova l'immagine di destinazione." & CrLf & CrLf & "Si desidera aprire la directory in cui è memorizzata l'immagine di destinazione?"
                OK_Button.Text = "Sì"
                Cancel_Button.Text = "No"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            Panel1.BackColor = Color.FromArgb(31, 31, 31)
            Label1.ForeColor = Color.FromArgb(0, 122, 204)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            Panel1.BackColor = Color.FromArgb(238, 238, 242)
            Label1.ForeColor = Color.FromArgb(0, 51, 153)
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class
