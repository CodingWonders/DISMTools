Imports System.Windows.Forms
Imports DISMTools.Elements
Imports Microsoft.Dism

Public Class SetImageKey
    Implements IImageTaskDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        DynaLog.LogMessage("Preparing to validate the product key syntax...")
        Dim key As ProductKey = ProductKeyValidator.ValidateProductKey(TextBox1.Text)
        If Not key.Valid Then
            DynaLog.LogMessage("Syntactically, the product key is bad.")
            MsgBox("The product key has not been typed correctly.", vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        ProgressPanel.OperationNum = 72
        ProgressPanel.pkSetNewProductKey = TextBox1.Text
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
        DynaLog.LogMessage("Preparing to validate the product key...")
        DynaLog.LogMessage("Stage 1: Product Key Syntax Check...")
        Dim key As ProductKey = ProductKeyValidator.ValidateProductKey(TextBox1.Text)
        If Not key.Valid Then
            DynaLog.LogMessage("Syntactically, the product key is bad.")
            MsgBox("The product key has not been typed correctly.", vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        DynaLog.LogMessage("Syntactically, the product key is good. Passing to stage 2...")
        DynaLog.LogMessage("Stage 2: Product Key Validation Check...")
        Dim validKey As Boolean
        Try
            DynaLog.LogMessage("Starting API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            DynaLog.LogMessage("Creating session and validating key...")
            Using imgSession As DismSession = DismApi.OpenOfflineSession(MainForm.MountDir)
                validKey = DismApi.ValidateProductKey(imgSession, TextBox1.Text)
            End Using
            If validKey Then
                DynaLog.LogMessage("The product key can be applied to this Windows image.")
                MsgBox("The product key is valid for this Windows image.", vbOKOnly + vbInformation, ImageTaskHeader1.ItemText)
            Else
                DynaLog.LogMessage("The product key cannot be applied to this Windows image.")
                MsgBox("The product key has been typed correctly, but is not valid for this Windows image.", vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not validate product key. Error message: " & ex.Message)
            MsgBox("The product key has been typed correctly, but we could not check if it's valid for this Windows image.", vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
        Finally
            Try
                DismApi.Shutdown()
            Catch ex As Exception

            End Try
        End Try
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        Dim msg As String = ""
        If MainForm.CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Then
            DynaLog.LogMessage("Image edition is WindowsPE. This is a Windows PE image.")
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "Windows PE images cannot be upgraded to higher editions."
                        Case "ESN"
                            msg = "Las imágenes de Windows PE no pueden ser actualizadas a ediciones superiores."
                        Case "FRA"
                            msg = "Les images Windows PE ne peuvent pas être mises à niveau vers des éditions supérieures."
                        Case "PTB", "PTG"
                            msg = "As imagens do Windows PE não podem ser actualizadas para edições superiores."
                        Case "ITA"
                            msg = "Le immagini di Windows PE non possono essere aggiornate a edizioni superiori."
                    End Select
                Case 1
                    msg = "Windows PE images cannot be upgraded to higher editions."
                Case 2
                    msg = "Las imágenes de Windows PE no pueden ser actualizadas a ediciones superiores."
                Case 3
                    msg = "Les images Windows PE ne peuvent pas être mises à niveau vers des éditions supérieures."
                Case 4
                    msg = "As imagens do Windows PE não podem ser actualizadas para edições superiores."
                Case 5
                    msg = "Le immagini di Windows PE non possono essere aggiornate a edizioni superiori."
            End Select
            MsgBox(msg, vbOKOnly + vbInformation, Text)
            Return False
        End If
        Return True
    End Function

    Private Sub SetImageKey_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Set product key"
                        ImageTaskHeader1.ItemText = Text
                        Label1.Text = "Type the product key that you want to set to your Windows image, including the dashes:"
                        Label2.Text = "If you want to check if your product key is valid for the Windows image, click Validate key. This will also check the syntax of your key."
                        Button1.Text = "Validate key"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                    Case "ESN"
                        Text = "Establecer clave de producto"
                        ImageTaskHeader1.ItemText = Text
                        Label1.Text = "Escriba la clave de producto que quiere establecer en la imagen de Windows, incluyendo los guiones:"
                        Label2.Text = "Si desea comprobar si la clave de producto es válida para la imagen de Windows, haga clic en Validar clave. Esto también comprobará la sintaxis de la clave."
                        Button1.Text = "Validar clave"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                    Case "FRA"
                        Text = "Définir la clé de produit"
                        ImageTaskHeader1.ItemText = Text
                        Label1.Text = "Tapez la clé de produit que vous souhaitez définir pour votre image Windows, y compris les tirets :"
                        Label2.Text = "Si vous souhaitez vérifier si votre clé de produit est valide pour l'image Windows, cliquez sur Valider la clé. Cela vérifiera également la syntaxe de votre clé."
                        Button1.Text = "Valider la clé"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                    Case "PTB", "PTG"
                        Text = "Definir chave do produto"
                        ImageTaskHeader1.ItemText = Text
                        Label1.Text = "Digite a chave do produto que você deseja definir para a imagem do Windows, incluindo os traços:"
                        Label2.Text = "Se você deseja verificar se a chave do produto é válida para a imagem do Windows, clique em Validar chave. Isso também verificará a sintaxe da chave."
                        Button1.Text = "Validar chave"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                    Case "ITA"
                        Text = "Imposta chiave prodotto"
                        ImageTaskHeader1.ItemText = Text
                        Label1.Text = "Digita la chiave prodotto che desideri impostare per l'immagine di Windows, inclusi i trattini:"
                        Label2.Text = "Se desideri verificare se la chiave prodotto è valida per l'immagine di Windows, fai clic su Convalida chiave. Questo controllerà anche la sintassi della chiave."
                        Button1.Text = "Convalida chiave"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annulla"
                End Select
            Case 1
                Text = "Set product key"
                ImageTaskHeader1.ItemText = Text
                Label1.Text = "Type the product key that you want to set to your Windows image, including the dashes:"
                Label2.Text = "If you want to check if your product key is valid for the Windows image, click Validate key. This will also check the syntax of your key."
                Button1.Text = "Validate key"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
            Case 2
                Text = "Establecer clave de producto"
                ImageTaskHeader1.ItemText = Text
                Label1.Text = "Escriba la clave de producto que quiere establecer en la imagen de Windows, incluyendo los guiones:"
                Label2.Text = "Si desea comprobar si la clave de producto es válida para la imagen de Windows, haga clic en Validar clave. Esto también comprobará la sintaxis de la clave."
                Button1.Text = "Validar clave"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
            Case 3
                Text = "Définir la clé de produit"
                ImageTaskHeader1.ItemText = Text
                Label1.Text = "Tapez la clé de produit que vous souhaitez définir pour votre image Windows, y compris les tirets :"
                Label2.Text = "Si vous souhaitez vérifier si votre clé de produit est valide pour l'image Windows, cliquez sur Valider la clé. Cela vérifiera également la syntaxe de votre clé."
                Button1.Text = "Valider la clé"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
            Case 4
                Text = "Definir chave do produto"
                ImageTaskHeader1.ItemText = Text
                Label1.Text = "Digite a chave do produto que você deseja definir para a imagem do Windows, incluindo os traços:"
                Label2.Text = "Se você deseja verificar se a chave do produto é válida para a imagem do Windows, clique em Validar chave. Isso também verificará a sintaxe da chave."
                Button1.Text = "Validar chave"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
            Case 5
                Text = "Imposta chiave prodotto"
                ImageTaskHeader1.ItemText = Text
                Label1.Text = "Digita la chiave prodotto che desideri impostare per l'immagine di Windows, inclusi i trattini:"
                Label2.Text = "Se desideri verificare se la chiave prodotto è valida per l'immagine di Windows, fai clic su Convalida chiave. Questo controllerà anche la sintassi della chiave."
                Button1.Text = "Convalida chiave"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annulla"
        End Select
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = CurrentTheme.ForegroundColor
        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)
        ImageTaskHeader1.HideWindowTitle(Handle)
    End Sub
End Class
