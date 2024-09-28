Imports System.Windows.Forms
Imports System.IO

Public Class ApplyUnattendFile

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        If TextBox1.Text <> "" AndAlso File.Exists(TextBox1.Text) Then
            ProgressPanel.UnattendedFile = TextBox1.Text
        Else
            Dim msg As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "Either no unattended answer file has been specified or the specified file does not exist. Please verify that it exists, and try again."
                        Case "ESN"
                            msg = "O ningún archivo de respuesta desatendida se ha especificado o el archivo especificado no existe. Verifique de que existe, e inténtelo de nuevo."
                        Case "FRA"
                            msg = "Soit aucun fichier de réponse sans surveillance n'a été spécifié, soit le fichier spécifié n'existe pas. Veuillez vérifier qu'il existe et réessayer."
                        Case "PTB", "PTG"
                            msg = "Ou não foi especificado nenhum ficheiro de resposta não assistida ou o ficheiro especificado não existe. Verifique se ele existe e tente novamente."
                        Case "ITA"
                            msg = "Non è stato specificato alcun file di risposta non presidiato oppure il file specificato non esiste. Verificare che esista e riprovare."
                    End Select
                Case 1
                    msg = "Either no unattended answer file has been specified or the specified file does not exist. Please verify that it exists, and try again."
                Case 2
                    msg = "O ningún archivo de respuesta desatendida se ha especificado o el archivo especificado no existe. Verifique de que existe, e inténtelo de nuevo."
                Case 3
                    msg = "Soit aucun fichier de réponse sans surveillance n'a été spécifié, soit le fichier spécifié n'existe pas. Veuillez vérifier qu'il existe et réessayer."
                Case 4
                    msg = "Ou não foi especificado nenhum ficheiro de resposta não assistida ou o ficheiro especificado não existe. Verifique se ele existe e tente novamente."
                Case 5
                    msg = "Non è stato specificato alcun file di risposta non presidiato oppure il file specificato non esiste. Verificare che esista e riprovare."
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If
        ProgressPanel.OperationNum = 79
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
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

    Private Sub ApplyUnattendFile_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Apply unattended answer file"
                        Label1.Text = Text
                        Label2.Text = "Answer file:"
                        Button1.Text = "Browse..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                    Case "ESN"
                        Text = "Aplicar archivo de respuesta desatendida"
                        Label1.Text = Text
                        Label2.Text = "Archivo de respuesta:"
                        Button1.Text = "Examinar..."
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                    Case "FRA"
                        Text = "Appliquer le fichier de réponses sans surveillance"
                        Label1.Text = Text
                        Label2.Text = "Fichier de réponse :"
                        Button1.Text = "Parcourir..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                    Case "PTB", "PTG"
                        Text = "Aplicar ficheiro de resposta não assistida"
                        Label1.Text = Text
                        Label2.Text = "Ficheiro de resposta:"
                        Button1.Text = "Procurar..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                    Case "ITA"
                        Text = "Applicare il file di risposta non presidiato"
                        Label1.Text = Text
                        Label2.Text = "File di risposta:"
                        Button1.Text = "Sfoglia..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                End Select
            Case 1
                Text = "Apply unattended answer file"
                Label1.Text = Text
                Label2.Text = "Answer file:"
                Button1.Text = "Browse..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
            Case 2
                Text = "Aplicar archivo de respuesta desatendida"
                Label1.Text = Text
                Label2.Text = "Archivo de respuesta:"
                Button1.Text = "Examinar..."
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
            Case 3
                Text = "Appliquer le fichier de réponses sans surveillance"
                Label1.Text = Text
                Label2.Text = "Fichier de réponse :"
                Button1.Text = "Parcourir..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
            Case 4
                Text = "Aplicar ficheiro de resposta não assistida"
                Label1.Text = Text
                Label2.Text = "Ficheiro de resposta:"
                Button1.Text = "Procurar..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
            Case 5
                Text = "Applicare il file di risposta non presidiato"
                Label1.Text = Text
                Label2.Text = "File di risposta:"
                Button1.Text = "Sfoglia..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class
