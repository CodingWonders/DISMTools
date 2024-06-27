Imports Microsoft.VisualBasic.ControlChars

Public Class ExceptionForm

    Dim copySuccess As String = "This information has been copied to the clipboard."
    Dim copyFail As String = "You'll need to copy this information manually."

    Private Sub Issue_Btn_Click(sender As Object, e As EventArgs) Handles Issue_Btn.Click
        DialogResult = Windows.Forms.DialogResult.None
        Process.Start("https://github.com/CodingWonders/DISMTools/issues/new?assignees=CodingWonders&labels=bug%2C+good+first+issue&projects=&template=bug.md&title=Program%20Exception")
    End Sub

    Private Sub Continue_Btn_Click(sender As Object, e As EventArgs) Handles Continue_Btn.Click
        DialogResult = Windows.Forms.DialogResult.OK
        Close()
    End Sub

    Private Sub Exit_Btn_Click(sender As Object, e As EventArgs) Handles Exit_Btn.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Close()
    End Sub

    Private Sub ExceptionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Use system language in case exception is thrown when trying to load settings
        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
            Case "ENU", "ENG"
                Text = "DISMTools - Internal Error"
                Label1.Text = "We are sorry for the inconvenience, but DISMTools has run into an error that it couldn't handle and we need your help in order to continue." & CrLf & CrLf & "Here is the error information if you need it:"
                Label2.Text = "Please help us fix this issue"
                Label3.Text = "In order to prevent this problem from happening again, we would like to know more about it by reporting an issue on the GitHub repository. You will need a GitHub account to report feedback."
                Label4.Text = "You may be able to continue running the program by clicking Continue. However, if this error is displayed for a second time, you can forcefully close the program by clicking Exit. Do note that changes made to projects, as well as changes in the Recents list, will not be saved." & CrLf & CrLf & "What do you want to do?"
                Issue_Btn.Text = "Report this issue"
                Continue_Btn.Text = "Continue"
                Exit_Btn.Text = "Exit"
                copySuccess = "This information has been copied to the clipboard."
                copyFail = "You'll need to copy this information manually."
            Case "ESN"
                Text = "DISMTools - Error interno"
                Label1.Text = "Lo sentimos por el inconveniente, pero DISMTools ha sufrido un error que no pudo controlar y necesitamos su ayuda para poder continuar." & CrLf & CrLf & "Aquí tiene la información del error por si lo necesita:"
                Label2.Text = "Por favor, ayúdenos a corregir este problema"
                Label3.Text = "Para evitar que este problema ocurra de nuevo, nos gustaría saber más acerca de él reportando un error en el repositorio de GitHub. Necesitará una cuenta de GitHub para enviar comentarios."
                Label4.Text = "Podrá ser capaz de continuar con la ejecución del programa haciendo clic en Continuar. En cambio, si este error se muestra por una segunda vez, puede cerrar el programa forzadamente haciendo clic en Salir. Dese cuenta de que los cambios de proyectos y de la lista de Recientes no se guardarán." & CrLf & CrLf & "¿Qué le gustaría hacer?"
                Issue_Btn.Text = "Reportar este problema"
                Continue_Btn.Text = "Continuar"
                Exit_Btn.Text = "Salir"
                copySuccess = "Esta información ha sido copiada al portapapeles."
                copyFail = "Deberá copiar esta información manualmente."
            Case "FRA"
                Text = "DISMTools - Erreur interne"
                Label1.Text = "Nous sommes désolés pour la gêne occasionnée, mais DISMTools a rencontré une erreur qu'il n'a pas pu gérer et nous avons besoin de votre aide pour continuer" & CrLf & CrLf & "Voici les informations sur l'erreur si vous en avez besoin :"
                Label2.Text = "Veuillez nous aider à résoudre ce problème"
                Label3.Text = "Afin d'éviter que ce problème ne se reproduise, nous aimerions en savoir plus en signalant un problème sur le dépôt GitHub. Vous devez disposer d'un compte GitHub pour signaler un problème."
                Label4.Text = "Vous pouvez continuer à exécuter le programme en cliquant sur Continuer. Cependant, si cette erreur s'affiche une seconde fois, vous pouvez fermer le programme en cliquant sur Quitter. Notez que les modifications apportées aux projets, ainsi que les modifications apportées à la liste Récents, ne seront pas sauvegardées." & CrLf & CrLf & "Que voulez-vous faire ?"
                Issue_Btn.Text = "Signaler ce problème"
                Continue_Btn.Text = "Continuer"
                Exit_Btn.Text = "Quitter"
                copySuccess = "Cette information a été copiée dans le presse-papiers"
                copyFail = "Vous devrez copier ces informations manuellement."
            Case "PTB", "PTG"
                Text = "DISMTools - Erro interno"
                Label1.Text = "Lamentamos o incómodo, mas o DISMTools deparou-se com um erro que não conseguiu resolver e precisamos da sua ajuda para continuar." & CrLf & CrLf & "Aqui está a informação do erro, se precisar dela:"
                Label2.Text = "Por favor, ajude-nos a resolver este problema"
                Label3.Text = "Para evitar que este problema volte a acontecer, gostaríamos de saber mais sobre o mesmo, reportando um problema no repositório do GitHub. Necessita de uma conta GitHub para comunicar comentários."
                Label4.Text = "Poderá continuar a executar o programa clicando em Continuar. No entanto, se este erro for apresentado pela segunda vez, pode fechar o programa à força, clicando em Sair. Tenha em atenção que as alterações efectuadas nos projectos, bem como as alterações na lista Recentes, não serão guardadas." & CrLf & CrLf & "O que pretende fazer?"
                Issue_Btn.Text = "Comunicar este problema"
                Continue_Btn.Text = "Continuar"
                Exit_Btn.Text = "Sair"
                copySuccess = "Esta informação foi copiada para a área de transferência."
                copyFail = "Terá de copiar esta informação manualmente."
            Case "ITA"
                Text = "DISMTools - Errore interno"
                Label1.Text = "Ci scusiamo per l'inconveniente, ma DISMTools ha riscontrato un errore che non è stato in grado di gestire e abbiamo bisogno del vostro aiuto per continuare." & CrLf & CrLf & "Ecco le informazioni sull'errore se ne avete bisogno:"
                Label2.Text = "Per favore, aiutateci a risolvere questo problema"
                Label3.Text = "Per evitare che questo problema si ripeta, vorremmo saperne di più segnalando un problema sul repository GitHub. Per segnalare un feedback è necessario un account GitHub"
                Label4.Text = "È possibile continuare a eseguire il programma facendo clic su Continua. Tuttavia, se questo errore viene visualizzato per la seconda volta, è possibile chiudere forzatamente il programma facendo clic su Exit. Si noti che le modifiche apportate ai progetti e quelle nell'elenco dei Recenti non verranno salvate." & CrLf & CrLf & "Cosa si desidera fare?"
                Issue_Btn.Text = "Segnala questo problema"
                Continue_Btn.Text = "Continua"
                Exit_Btn.Text = "Esci"
                copySuccess = "Queste informazioni sono state copiate negli appunti"
                copyFail = "È necessario copiare queste informazioni manualmente"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        ErrorText.BackColor = BackColor
        ErrorText.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        Try
            Dim data As New DataObject()
            data.SetText(ErrorText.Text, TextDataFormat.Text)
            Clipboard.SetDataObject(data, True)
            ErrorText.AppendText(CrLf & CrLf & copySuccess)
        Catch ex As Exception
            ErrorText.AppendText(CrLf & CrLf & copyFail)
        End Try
        Beep()
    End Sub
End Class