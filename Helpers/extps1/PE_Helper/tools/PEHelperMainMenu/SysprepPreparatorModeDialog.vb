Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class SysprepPreparatorModeDialog

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MainForm.AutoCapture = CheckBox1.Checked
        MainForm.CopyProfile = CheckBox2.Checked
        DialogResult = System.Windows.Forms.DialogResult.Yes
        Close()
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        MainForm.AutoCapture = CheckBox1.Checked
        DialogResult = System.Windows.Forms.DialogResult.No
        Close()
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        DialogResult = System.Windows.Forms.DialogResult.Cancel
        Close()
    End Sub

    Private Sub SysprepPreparatorModeDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = MainForm.LinkLabel4.Text

        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
            Case "ENU", "ENG"
                Label1.Text = "The Sysprep Preparation Tool, responsible for preparing systems for image capture, can operate in 2 modes: Automatic mode and Manual mode." & CrLf & CrLf &
                    "- Automatic mode performs checks and, if said checks pass, prepares and generalizes your computer automatically. You don't need to interact with the tool, unless checks fail or complete with warnings. A default set of options for Sysprep will also be used" & CrLf &
                    "- Manual mode lets you configure Sysprep launch settings and lets you go through each step of the tool at your own pace. This is recommended for advanced users" & CrLf & CrLf &
                    "Select the mode you want to use:"
                LinkLabel1.Text = "Launch in Automatic mode (recommended)"
                LinkLabel2.Text = "Launch in Manual mode (advanced)"
                LinkLabel3.Text = "Cancel launch of this tool"
                CheckBox1.Text = "Capture image after preparing the system"
            Case "ESN"
                Label1.Text = "La herramienta de preparación de Sysprep, utilizada para preparar sistemas, puede operar en 2 modos: modo automático y modo manual" & CrLf & CrLf &
                    "- El modo automático realiza comprobaciones y, si dichas comprobaciones se realizaron correctamente, prepara y generaliza su equipo automáticamente. No tiene que interactuar con la herramienta a menos que las comprobaciones fallen o completen con advertencias. Una configuración predeterminada para Sysprep también será usada" & CrLf &
                    "- El modo manual le permite configurar las opciones de Sysprep y le permite realizar cada paso de la herramienta a su propio ritmo. Esto es recomendado para usuarios avanzados" & CrLf & CrLf &
                    "Seleccione el modo que quiera usar:"
                LinkLabel1.Text = "Iniciar en modo automático (recomendado)"
                LinkLabel2.Text = "Iniciar en modo manual (avanzado)"
                LinkLabel3.Text = "Cancelar el inicio de esta herramienta"
                CheckBox1.Text = "Capturar imagen tras preparar el sistema"
            Case "FRA"
                Label1.Text = "L’outil de préparation Sysprep, chargé de préparer les systèmes pour la capture d’image, peut fonctionner en 2 modes : mode automatique et mode manuel." & CrLf & CrLf &
                    "- Le mode automatique effectue des vérifications et, si celles-ci réussissent, prépare et généralise automatiquement votre ordinateur. Vous n’avez pas besoin d’interagir avec l’outil, sauf si les vérifications échouent ou renvoient des avertissements. Un ensemble d’options par défaut pour Sysprep sera également utilisé" & CrLf &
                    "- Le mode manuel vous permet de configurer les paramètres de lancement de Sysprep et de suivre chaque étape de l’outil à votre rythme. Il est recommandé pour les utilisateurs avancés" & CrLf & CrLf &
                    "Sélectionnez le mode que vous souhaitez utiliser :"
                LinkLabel1.Text = "Lancer en mode automatique (recommandé)"
                LinkLabel2.Text = "Lancer en mode manuel (avancé)"
                LinkLabel3.Text = "Annuler le lancement de cet outil"
                CheckBox1.Text = "Capturez l'image après avoir préparé le système"
            Case "PTB", "PTG"
                Label1.Text = "A ferramenta de preparação Sysprep, responsável por preparar sistemas para a captura de imagem, pode funcionar em 2 modos: modo automático e modo manual." & CrLf & CrLf &
                    "- O modo automático realiza verificações e, se estas forem bem-sucedidas, prepara e generaliza o computador automaticamente. Não é necessário interagir com a ferramenta, a menos que as verificações falhem ou retornem avisos. Será também usado um conjunto de opções padrão para o Sysprep" & CrLf &
                    "- O modo manual permite configurar as definições de execução do Sysprep e avançar por cada etapa da ferramenta ao seu próprio ritmo. É recomendado para utilizadores avançados" & CrLf & CrLf &
                    "Selecione o modo que deseja usar:"
                LinkLabel1.Text = "Iniciar em modo automático (recomendado)"
                LinkLabel2.Text = "Iniciar em modo manual (avançado)"
                LinkLabel3.Text = "Cancelar o lançamento desta ferramenta"
                CheckBox1.Text = "Capture a imagem após preparar o sistema"
            Case "ITA"
                Label1.Text = "Lo strumento di preparazione Sysprep, responsabile della preparazione dei sistemi per l’acquisizione dell’immagine, può funzionare in 2 modalità: automatica e manuale." & CrLf & CrLf &
                    "- La modalità automatica esegue dei controlli e, se superati, prepara e generalizza automaticamente il computer. Non è necessario interagire con lo strumento, a meno che i controlli falliscano o restituiscano avvisi. Verrà anche utilizzato un set di opzioni predefinite per Sysprep" & CrLf &
                    "- La modalità manuale consente di configurare le impostazioni di avvio di Sysprep e di procedere attraverso ogni fase dello strumento al proprio ritmo. È consigliata agli utenti avanzati" & CrLf & CrLf &
                    "Seleziona la modalità che vuoi utilizzare:"
                LinkLabel1.Text = "Avvia in modalità automatica (consigliata)"
                LinkLabel2.Text = "Avvia in modalità manuale (avanzata)"
                LinkLabel3.Text = "Annulla l’avvio di questo strumento"
                CheckBox1.Text = "Acquisizione dell'immagine dopo aver preparato il sistema"
        End Select
    End Sub
End Class
