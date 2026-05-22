Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class NewProj

    Dim IsReqField1Valid As Boolean
    Dim IsReqField2Valid As Boolean

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        DynaLog.LogMessage("Creating project...")
        DynaLog.LogMessage("- Project name: " & TextBox1.Text)
        DynaLog.LogMessage("- Project path: " & Quote & TextBox2.Text & Quote)
        If Not Directory.Exists(TextBox2.Text) Then
            DynaLog.LogMessage("The project path does not exist. Asking user whether or not to create it...")
            Dim msg As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "The directory: " & CrLf & Quote & TextBox2.Text & Quote & CrLf & "does not exist. Do you want to create it?"
                        Case "ESN"
                            msg = "El directorio: " & CrLf & Quote & TextBox2.Text & Quote & CrLf & "no existe. ¿Desea crearlo?"
                        Case "FRA"
                            msg = "Le répertoire : " & CrLf & Quote & TextBox2.Text & Quote & CrLf & "n'existe pas. Voulez-vous le créer ?"
                        Case "PTB", "PTG"
                            msg = "O diretório: " & CrLf & Quote & TextBox2.Text & Quote & CrLf & "Deseja criá-lo?"
                        Case "ITA"
                            msg = "La cartella: " & CrLf & Quote & TextBox2.Text & Quote & CrLf & "non esiste. Si desidera crearla?"
                    End Select
                Case 1
                    msg = "The directory: " & CrLf & Quote & TextBox2.Text & Quote & CrLf & "does not exist. Do you want to create it?"
                Case 2
                    msg = "El directorio: " & CrLf & Quote & TextBox2.Text & Quote & CrLf & "no existe. ¿Desea crearlo?"
                Case 3
                    msg = "Le répertoire : " & CrLf & Quote & TextBox2.Text & Quote & CrLf & "n'existe pas. Voulez-vous le créer ?"
                Case 4
                    msg = "O diretório: " & CrLf & Quote & TextBox2.Text & Quote & CrLf & "Deseja criá-lo?"
                Case 5
                    msg = "La cartella: " & CrLf & Quote & TextBox2.Text & Quote & CrLf & "non esiste. Si desidera crearla?"
            End Select
            If MsgBox(msg, vbYesNo + vbQuestion, ImageTaskHeader1.ItemText) = MsgBoxResult.Yes Then
                DynaLog.LogMessage("The user has decided to create the folder. Attempting to create it...")
                Try
                    Directory.CreateDirectory(TextBox2.Text)
                Catch ex As Exception
                    DynaLog.LogMessage("Could not create the folder. Error message: " & ex.Message)
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg = "We could not create the project directory for you due to: " & CrLf & ex.ToString() & "; " & ex.Message
                                Case "ESN"
                                    msg = "No pudimos crear el directorio del proyecto porque: " & CrLf & ex.ToString() & "; " & ex.Message
                                Case "FRA"
                                    msg = "Nous n'avons pas pu créer le répertoire du projet pour vous pour les raisons suivantes : " & CrLf & ex.ToString() & "; " & ex.Message
                                Case "PTB", "PTG"
                                    msg = "Não foi possível criar o diretório do projeto para si devido a: " & CrLf & ex.ToString() & "; " & ex.Message
                                Case "ITA"
                                    msg = "Non è stato possibile creare la cartella del progetto a causa di: " & CrLf & ex.ToString() & "; " & ex.Message
                            End Select
                        Case 1
                            msg = "We could not create the project directory for you due to: " & CrLf & ex.ToString() & "; " & ex.Message
                        Case 2
                            msg = "No pudimos crear el directorio del proyecto porque: " & CrLf & ex.ToString() & "; " & ex.Message
                        Case 3
                            msg = "Nous n'avons pas pu créer le répertoire du projet pour vous pour les raisons suivantes : " & CrLf & ex.ToString() & "; " & ex.Message
                        Case 4
                            msg = "Não foi possível criar o diretório do projeto para si devido a: " & CrLf & ex.ToString() & "; " & ex.Message
                        Case 5
                            msg = "Non è stato possibile creare la cartella del progetto a causa di: " & CrLf & ex.ToString() & "; " & ex.Message
                    End Select
                    MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                    Exit Sub
                End Try
            Else
                Exit Sub
            End If
        End If
        ProgressPanel.OperationNum = 0
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        If MainForm.isProjectLoaded Then
            DynaLog.LogMessage("Unloading any loaded projects...")
            If MainForm.OnlineManagement Then
                MainForm.EndOnlineManagement()
            ElseIf MainForm.OfflineManagement Then
                MainForm.EndOfflineManagement()
            Else
                MainForm.UnloadDTProj(False, True)
            End If
            If MainForm.ImgBW.IsBusy Then Exit Sub
        End If
        ProgressPanel.projName = TextBox1.Text
        ProgressPanel.projPath = TextBox2.Text
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub NewProj_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Create a new project"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Please specify the options to create a new project:"
                        Label3.Text = "Name*:"
                        Label4.Text = "Location*:"
                        Label5.Text = "The fields that end in * are required"
                        Button1.Text = "Browse..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        GroupBox1.Text = "Project"
                        FolderBrowserDialog1.Description = "Please select a folder to store this project:"
                    Case "ESN"
                        Text = "Crear un nuevo proyecto"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Especifique las opciones para crear un nuevo proyecto:"
                        Label3.Text = "Nombre*:"
                        Label4.Text = "Ubicación*:"
                        Label5.Text = "Los campos que terminen en * son necesarios"
                        Button1.Text = "Examinar..."
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        GroupBox1.Text = "Proyecto"
                        FolderBrowserDialog1.Description = "Seleccione una carpeta donde almacenar este proyecto:"
                    Case "FRA"
                        Text = "Créer un nouveau projet"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Veuillez spécifier les options pour créer un nouveau projet :"
                        Label3.Text = "Nom* :"
                        Label4.Text = "Emplacement* :"
                        Label5.Text = "Les champs se terminant par * sont obligatoires"
                        Button1.Text = "Parcourir..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        GroupBox1.Text = "Projet"
                        FolderBrowserDialog1.Description = "Veuillez sélectionner un dossier pour stocker ce projet :"
                    Case "PTB", "PTG"
                        Text = "Criar um novo projeto"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Por favor, especifique as opções para criar um novo projeto:"
                        Label3.Text = "Nome*:"
                        Label4.Text = "Localização*:"
                        Label5.Text = "Os campos que terminam em * são obrigatórios"
                        Button1.Text = "Navegar..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        GroupBox1.Text = "Projeto"
                        FolderBrowserDialog1.Description = "Por favor, seleccione uma pasta para armazenar este projeto:"
                    Case "ITA"
                        Text = "Crea un nuovo progetto"
                        ImageTaskHeader1.ItemText = Text
                        Label2.Text = "Specificare le opzioni per creare un nuovo progetto:"
                        Label3.Text = "Nome*:"
                        Label4.Text = "Ubicazione*:"
                        Label5.Text = "I campi che terminano con * sono obbligatori"
                        Button1.Text = "Sfoglia..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        GroupBox1.Text = "Progetto"
                        FolderBrowserDialog1.Description = "Selezionare una cartella per memorizzare questo progetto:"
                End Select
            Case 1
                Text = "Create a new project"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Please specify the options to create a new project:"
                Label3.Text = "Name*:"
                Label4.Text = "Location*:"
                Label5.Text = "The fields that end in * are required"
                Button1.Text = "Browse..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                GroupBox1.Text = "Project"
                FolderBrowserDialog1.Description = "Please select a folder to store this project:"
            Case 2
                Text = "Crear un nuevo proyecto"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Especifique las opciones para crear un nuevo proyecto:"
                Label3.Text = "Nombre*:"
                Label4.Text = "Ubicación*:"
                Label5.Text = "Los campos que terminen en * son necesarios"
                Button1.Text = "Examinar..."
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                GroupBox1.Text = "Proyecto"
                FolderBrowserDialog1.Description = "Seleccione una carpeta donde almacenar este proyecto:"
            Case 3
                Text = "Créer un nouveau projet"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Veuillez spécifier les options pour créer un nouveau projet :"
                Label3.Text = "Nom* :"
                Label4.Text = "Emplacement* :"
                Label5.Text = "Les champs se terminant par * sont obligatoires"
                Button1.Text = "Parcourir..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                GroupBox1.Text = "Projet"
                FolderBrowserDialog1.Description = "Veuillez sélectionner un dossier pour stocker ce projet :"
            Case 4
                Text = "Criar um novo projeto"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Por favor, especifique as opções para criar um novo projeto:"
                Label3.Text = "Nome*:"
                Label4.Text = "Localização*:"
                Label5.Text = "Os campos que terminam em * são obrigatórios"
                Button1.Text = "Navegar..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                GroupBox1.Text = "Projeto"
                FolderBrowserDialog1.Description = "Por favor, seleccione uma pasta para armazenar este projeto:"
            Case 5
                Text = "Crea un nuovo progetto"
                ImageTaskHeader1.ItemText = Text
                Label2.Text = "Specificare le opzioni per creare un nuovo progetto:"
                Label3.Text = "Nome*:"
                Label4.Text = "Ubicazione*:"
                Label5.Text = "I campi che terminano con * sono obbligatori"
                Button1.Text = "Sfoglia..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                GroupBox1.Text = "Progetto"
                FolderBrowserDialog1.Description = "Selezionare una cartella per memorizzare questo progetto:"
        End Select
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog(Me)
        If DialogResult.OK And FolderBrowserDialog1.SelectedPath <> "" Then
            TextBox2.Text = FolderBrowserDialog1.SelectedPath
            IsReqField2Valid = True
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        DynaLog.LogMessage("Specified project name: " & Quote & TextBox1.Text & Quote)
        If TextBox1.Text <> "" Then
            DynaLog.LogMessage("Verifying name...")
            If TextBox1.Text.Equals("con", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("CON", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("aux", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("AUX", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("prn", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("PRN", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("nul", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("NUL", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com1", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com2", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com3", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com4", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com5", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com6", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com7", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com8", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com9", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM1", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM2", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM3", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM4", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM5", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM6", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM7", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM8", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM9", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt1", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt2", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt3", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt4", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt5", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt6", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt7", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt8", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt9", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT1", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT2", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT3", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT4", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT5", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT6", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT7", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT8", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT9", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Contains("<") Or _
                TextBox1.Text.Contains(">") Or _
                TextBox1.Text.Contains(":") Or _
                TextBox1.Text.Contains(Quote) Or _
                TextBox1.Text.Contains("/") Or _
                TextBox1.Text.Contains("\") Or _
                TextBox1.Text.Contains("|") Or _
                TextBox1.Text.Contains("?") Or _
                TextBox1.Text.Contains("*") Then
                DynaLog.LogMessage("Project name contains invalid text.")
                IsReqField1Valid = False
            Else
                DynaLog.LogMessage("Project name is valid.")
                IsReqField1Valid = True
            End If
        Else
            DynaLog.LogMessage("No project name has been specified.")
            IsReqField1Valid = False
        End If
        CheckReqFields()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        DynaLog.LogMessage("Specified project path: " & Quote & TextBox2.Text & Quote)
        If TextBox2.Text <> "" Then
            DynaLog.LogMessage("Verifying path...")
            If TextBox2.Text.Contains("con") Or _
                    TextBox2.Text.Contains("CON") Or _
                    TextBox2.Text.Contains("aux") Or _
                    TextBox2.Text.Contains("AUX") Or _
                    TextBox2.Text.Contains("prn") Or _
                    TextBox2.Text.Contains("PRN") Or _
                    TextBox2.Text.Contains("nul") Or _
                    TextBox2.Text.Contains("NUL") Or _
                    TextBox2.Text.Contains("com1") Or _
                    TextBox2.Text.Contains("com2") Or _
                    TextBox2.Text.Contains("com3") Or _
                    TextBox2.Text.Contains("com4") Or _
                    TextBox2.Text.Contains("com5") Or _
                    TextBox2.Text.Contains("com6") Or _
                    TextBox2.Text.Contains("com7") Or _
                    TextBox2.Text.Contains("com8") Or _
                    TextBox2.Text.Contains("com9") Or _
                    TextBox2.Text.Contains("COM1") Or _
                    TextBox2.Text.Contains("COM2") Or _
                    TextBox2.Text.Contains("COM3") Or _
                    TextBox2.Text.Contains("COM4") Or _
                    TextBox2.Text.Contains("COM5") Or _
                    TextBox2.Text.Contains("COM6") Or _
                    TextBox2.Text.Contains("COM7") Or _
                    TextBox2.Text.Contains("COM8") Or _
                    TextBox2.Text.Contains("COM9") Or _
                    TextBox2.Text.Contains("lpt1") Or _
                    TextBox2.Text.Contains("lpt2") Or _
                    TextBox2.Text.Contains("lpt3") Or _
                    TextBox2.Text.Contains("lpt4") Or _
                    TextBox2.Text.Contains("lpt5") Or _
                    TextBox2.Text.Contains("lpt6") Or _
                    TextBox2.Text.Contains("lpt7") Or _
                    TextBox2.Text.Contains("lpt8") Or _
                    TextBox2.Text.Contains("lpt9") Or _
                    TextBox2.Text.Contains("LPT1") Or _
                    TextBox2.Text.Contains("LPT2") Or _
                    TextBox2.Text.Contains("LPT3") Or _
                    TextBox2.Text.Contains("LPT4") Or _
                    TextBox2.Text.Contains("LPT5") Or _
                    TextBox2.Text.Contains("LPT6") Or _
                    TextBox2.Text.Contains("LPT7") Or _
                    TextBox2.Text.Contains("LPT8") Or _
                    TextBox2.Text.Contains("LPT9") Or _
                    TextBox2.Text.Contains("<") Or _
                    TextBox2.Text.Contains(">") Or _
                    TextBox2.Text.Contains("|") Or _
                    TextBox2.Text.Contains("?") Or _
                    TextBox2.Text.Contains("*") Then
                DynaLog.LogMessage("Project path contains invalid text.")
                IsReqField2Valid = False
            Else
                DynaLog.LogMessage("Project path is valid.")
                IsReqField2Valid = True
            End If
        Else
            DynaLog.LogMessage("No project path has been specified.")
            IsReqField2Valid = False
        End If
        CheckReqFields()
    End Sub

    Sub CheckReqFields()
        DynaLog.LogMessage("Checking fields...")
        If IsReqField1Valid And IsReqField2Valid Then
            DynaLog.LogMessage("All fields are valid.")
            OK_Button.Enabled = True
        Else
            DynaLog.LogMessage("None or not all fields are valid.")
            OK_Button.Enabled = False
        End If
    End Sub
End Class
