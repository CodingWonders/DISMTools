Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class ImgWinVistaIncompatibilityDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgWinVistaIncompatibilityDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label1.Text = "The program can't service Windows Vista images"
                        Label2.Text = "Neither this program nor DISM support servicing Windows Vista images. DISM is meant to service Windows 7 or newer images. You can still mount Windows Vista images, but all options will be disabled." & CrLf & CrLf & "If you still want to service a Windows Vista image, refer to the " & Quote & "Compatibility with older Windows versions" & Quote & " topic in the Help documentation." & CrLf & CrLf & "Do you want to continue?"
                        OK_Button.Text = "Yes"
                        Cancel_Button.Text = "No"
                    Case "ESN"
                        Label1.Text = "El programa no puede ofrecer servicio a imágenes de Windows Vista"
                        Label2.Text = "Ni este programa ni DISM soportan ofrecer servicio a imágenes de Windows Vista. DISM puede ofrecer servicio a imágenes de Windows 7 o posterior. Aún puede montar imágenes de Windows Vista, pero todas las opciones serán deshabilitadas." & CrLf & CrLf & "Si todavía desea ofrecer servicio a una imagen de Windows Vista, véase el tópico " & Quote & "Compatibilidad con versiones antiguas de Windows" & Quote & " en la documentación de ayuda." & CrLf & CrLf & "¿Desea continuar?"
                        OK_Button.Text = "Sí"
                        Cancel_Button.Text = "No"
                    Case "FRA"
                        Label1.Text = "Le programme ne peut pas gérer les images de Windows Vista"
                        Label2.Text = "Ni ce programme ni DISM ne prennent en charge l'entretien des images Windows Vista. DISM est conçu pour gérer les images Windows 7 ou plus récentes. Vous pouvez toujours monter des images Windows Vista, mais toutes les options seront désactivées." & CrLf & CrLf & "Si vous souhaitez toujours entretenir une image Windows Vista, reportez-vous à la rubrique " & Quote & "Compatibilité avec les anciennes versions de Windows" & Quote & " dans la documentation d'aide." & CrLf & CrLf & "Voulez-vous continuer ?"
                        OK_Button.Text = "Oui"
                        Cancel_Button.Text = "Non"
                    Case "PTB", "PTG"
                        Label1.Text = "O programa não pode efetuar a manutenção de imagens do Windows Vista"
                        Label2.Text = "Nem este programa nem o DISM suportam a manutenção de imagens do Windows Vista. O DISM destina-se a servir imagens do Windows 7 ou mais recentes. Pode continuar a montar imagens do Windows Vista, mas todas as opções serão desactivadas." & CrLf & CrLf & "Se ainda pretender reparar uma imagem do Windows Vista, consulte o tópico " & Quote & "Compatibilidade com versões anteriores do Windows" & Quote & " na documentação da Ajuda." & CrLf & CrLf & "Pretende continuar?"
                        OK_Button.Text = "Sim"
                        Cancel_Button.Text = "Não"
                    Case "ITA"
                        Label1.Text = "Il programma non è in grado di gestire le immagini di Windows Vista"
                        Label2.Text = "Né questo programma né DISM supportano la manutenzione delle immagini di Windows Vista. DISM è destinato a gestire immagini di Windows 7 o più recenti. È ancora possibile montare le immagini di Windows Vista, ma tutte le opzioni saranno disabilitate." & CrLf & CrLf & "Se si desidera comunque eseguire la manutenzione di un'immagine di Windows Vista, consultare l'argomento " & Quote & "Compatibilità con le versioni precedenti di Windows" & Quote & " nella documentazione della Guida in linea." & CrLf & CrLf & "Si desidera continuare?"
                        OK_Button.Text = "Sì"
                        Cancel_Button.Text = "No"
                End Select
            Case 1
                Label1.Text = "The program can't service Windows Vista images"
                Label2.Text = "Neither this program nor DISM support servicing Windows Vista images. DISM is meant to service Windows 7 or newer images. You can still mount Windows Vista images, but all options will be disabled." & CrLf & CrLf & "If you still want to service a Windows Vista image, refer to the " & Quote & "Compatibility with older Windows versions" & Quote & " topic in the Help documentation." & CrLf & CrLf & "Do you want to continue?"
                OK_Button.Text = "Yes"
                Cancel_Button.Text = "No"
            Case 2
                Label1.Text = "El programa no puede ofrecer servicio a imágenes de Windows Vista"
                Label2.Text = "Ni este programa ni DISM soportan ofrecer servicio a imágenes de Windows Vista. DISM puede ofrecer servicio a imágenes de Windows 7 o posterior. Aún puede montar imágenes de Windows Vista, pero todas las opciones serán deshabilitadas." & CrLf & CrLf & "Si todavía desea ofrecer servicio a una imagen de Windows Vista, véase el tópico " & Quote & "Compatibilidad con versiones antiguas de Windows" & Quote & " en la documentación de ayuda." & CrLf & CrLf & "¿Desea continuar?"
                OK_Button.Text = "Sí"
                Cancel_Button.Text = "No"
            Case 3
                Label1.Text = "Le programme ne peut pas gérer les images de Windows Vista"
                Label2.Text = "Ni ce programme ni DISM ne prennent en charge l'entretien des images Windows Vista. DISM est conçu pour gérer les images Windows 7 ou plus récentes. Vous pouvez toujours monter des images Windows Vista, mais toutes les options seront désactivées." & CrLf & CrLf & "Si vous souhaitez toujours entretenir une image Windows Vista, reportez-vous à la rubrique " & Quote & "Compatibilité avec les anciennes versions de Windows" & Quote & " dans la documentation d'aide." & CrLf & CrLf & "Voulez-vous continuer ?"
                OK_Button.Text = "Oui"
                Cancel_Button.Text = "Non"
            Case 4
                Label1.Text = "O programa não pode efetuar a manutenção de imagens do Windows Vista"
                Label2.Text = "Nem este programa nem o DISM suportam a manutenção de imagens do Windows Vista. O DISM destina-se a servir imagens do Windows 7 ou mais recentes. Pode continuar a montar imagens do Windows Vista, mas todas as opções serão desactivadas." & CrLf & CrLf & "Se ainda pretender reparar uma imagem do Windows Vista, consulte o tópico " & Quote & "Compatibilidade com versões anteriores do Windows" & Quote & " na documentação da Ajuda." & CrLf & CrLf & "Pretende continuar?"
                OK_Button.Text = "Sim"
                Cancel_Button.Text = "Não"
            Case 5
                Label1.Text = "Il programma non è in grado di gestire le immagini di Windows Vista"
                Label2.Text = "Né questo programma né DISM supportano la manutenzione delle immagini di Windows Vista. DISM è destinato a gestire immagini di Windows 7 o più recenti. È ancora possibile montare le immagini di Windows Vista, ma tutte le opzioni saranno disabilitate." & CrLf & CrLf & "Se si desidera comunque eseguire la manutenzione di un'immagine di Windows Vista, consultare l'argomento " & Quote & "Compatibilità con le versioni precedenti di Windows" & Quote & " nella documentazione della Guida in linea." & CrLf & CrLf & "Si desidera continuare?"
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
