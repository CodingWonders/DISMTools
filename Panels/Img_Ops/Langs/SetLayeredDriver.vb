Imports System.Windows.Forms
Imports DISMTools.Elements

Public Class SetLayeredDriverDialog

    Dim CurrentKeyboardDriver As KeyboardDrivers.LayeredKeyboardDriver

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ProgressPanel.currentKeybLayeredDriverType = CurrentKeyboardDriver
        ProgressPanel.KeyboardLayeredDriverType = (ComboBox1.SelectedIndex + 1)
        ProgressPanel.OperationNum = 60
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.Show(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SetLayeredDriver_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set to default value
        CurrentKeyboardDriver = KeyboardDrivers.LayeredKeyboardDriver.Unknown
        ' Color modes/language stuff go here
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Set keyboard layered driver"
                        Label1.Text = Text
                        Label2.Text = "This action will let you set a keyboard layered driver for Japanese and Korean keyboards, as some users have keyboards with additional keys. Simply specify the new layered driver from the list below and click OK"
                        Label3.Text = "Current keyboard layered driver:"
                        Label5.Text = "New keyboard layered driver:"
                        Label6.Text = "This driver has already been set"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                    Case "ESN"
                        Text = "Establecer controlador de teclado superpuesto"
                        Label1.Text = Text
                        Label2.Text = "Esta acción le permitirá establecer un controlador de teclado superpuesto para teclados japoneses y coreanos, debido a que algunos usuarios poseen teclados con teclas adicionales. Simplemente especifique el nuevo controlador superpuesto de la lista y haga clic en Aceptar"
                        Label3.Text = "Controlador de teclado superpuesto actual:"
                        Label5.Text = "Nuevo controlador de teclado superpuesto:"
                        Label6.Text = "Este controlador ya se ha establecido"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                    Case "FRA"
                        Text = "Définir le pilote du clavier en couches"
                        Label1.Text = Text
                        Label2.Text = "Cette action vous permet de définir un pilote de clavier superposé pour les claviers japonais et coréens, car certains utilisateurs ont des claviers avec des touches supplémentaires. Il vous suffit de spécifier le nouveau pilote de clavier dans la liste ci-dessous et de cliquer sur OK"
                        Label3.Text = "Pilote de clavier actuel :"
                        Label5.Text = "Nouveau pilote de clavier superposé :"
                        Label6.Text = "Ce pilote a déjà été défini"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                    Case "PTB", "PTG"
                        Text = "Configurar controlador de teclado em camadas"
                        Label1.Text = Text
                        Label2.Text = "Esta ação permite-lhe configurar um controlador de teclado em camadas para teclados japoneses e coreanos, uma vez que alguns utilizadores têm teclados com teclas adicionais. Basta especificar o novo controlador da lista abaixo e clicar em OK"
                        Label3.Text = "Controlador atual do teclado em camadas:"
                        Label5.Text = "Novo controlador de teclado em camadas:"
                        Label6.Text = "Este controlador já foi configurado"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                    Case "ITA"
                        Text = "Imposta driver a strati per tastiera"
                        Label1.Text = Text
                        Label2.Text = "Questa azione consente di impostare un driver a strati per le tastiere giapponesi e coreane, poiché alcuni utenti dispongono di tastiere con tasti aggiuntivi. È sufficiente specificare il nuovo driver stratificato dall'elenco sottostante e fare clic su OK"
                        Label3.Text = "Driver a strati per la tastiera attuale:"
                        Label5.Text = "Nuovo driver a strati per tastiera:"
                        Label6.Text = "Questo driver è già stato impostato"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                End Select
            Case 1
                Text = "Set keyboard layered driver"
                Label1.Text = Text
                Label2.Text = "This action will let you set a keyboard layered driver for Japanese and Korean keyboards, as some users have keyboards with additional keys. Simply specify the new layered driver from the list below and click OK"
                Label3.Text = "Current keyboard layered driver:"
                Label5.Text = "New keyboard layered driver:"
                Label6.Text = "This driver has already been set"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
            Case 2
                Text = "Establecer controlador de teclado superpuesto"
                Label1.Text = Text
                Label2.Text = "Esta acción le permitirá establecer un controlador de teclado superpuesto para teclados japoneses y coreanos, debido a que algunos usuarios poseen teclados con teclas adicionales. Simplemente especifique el nuevo controlador superpuesto de la lista y haga clic en Aceptar"
                Label3.Text = "Controlador de teclado superpuesto actual:"
                Label5.Text = "Nuevo controlador de teclado superpuesto:"
                Label6.Text = "Este controlador ya se ha establecido"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
            Case 3
                Text = "Définir le pilote du clavier en couches"
                Label1.Text = Text
                Label2.Text = "Cette action vous permet de définir un pilote de clavier superposé pour les claviers japonais et coréens, car certains utilisateurs ont des claviers avec des touches supplémentaires. Il vous suffit de spécifier le nouveau pilote de clavier dans la liste ci-dessous et de cliquer sur OK"
                Label3.Text = "Pilote de clavier actuel :"
                Label5.Text = "Nouveau pilote de clavier superposé :"
                Label6.Text = "Ce pilote a déjà été défini"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
            Case 4
                Text = "Configurar controlador de teclado em camadas"
                Label1.Text = Text
                Label2.Text = "Esta ação permite-lhe configurar um controlador de teclado em camadas para teclados japoneses e coreanos, uma vez que alguns utilizadores têm teclados com teclas adicionais. Basta especificar o novo controlador da lista abaixo e clicar em OK"
                Label3.Text = "Controlador atual do teclado em camadas:"
                Label5.Text = "Novo controlador de teclado em camadas:"
                Label6.Text = "Este controlador já foi configurado"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
            Case 5
                Text = "Imposta driver a strati per tastiera"
                Label1.Text = Text
                Label2.Text = "Questa azione consente di impostare un driver a strati per le tastiere giapponesi e coreane, poiché alcuni utenti dispongono di tastiere con tasti aggiuntivi. È sufficiente specificare il nuovo driver stratificato dall'elenco sottostante e fare clic su OK"
                Label3.Text = "Driver a strati per la tastiera attuale:"
                Label5.Text = "Nuovo driver a strati per tastiera:"
                Label6.Text = "Questo driver è già stato impostato"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ComboBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ComboBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ComboBox1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))

        ' Get keyboard driver
        CurrentKeyboardDriver = KeyboardDrivers.GetKeyboardDriver(MainForm.MountDir)
        Select Case CurrentKeyboardDriver
            Case KeyboardDrivers.LayeredKeyboardDriver.Unknown
                Label4.Text = "Unknown/Not installed"
                ComboBox1.SelectedIndex = 0
            Case KeyboardDrivers.LayeredKeyboardDriver.PCATKey
                Label4.Text = "PC/AT Enhanced Keyboard (101/102-Key)"
                ComboBox1.SelectedIndex = 1
            Case KeyboardDrivers.LayeredKeyboardDriver.K_PCATKeyT1
                Label4.Text = "Korean PC/AT 101-Key Compatible Keyboard/MS Natural Keyboard (Type 1)"
                ComboBox1.SelectedIndex = 0
            Case KeyboardDrivers.LayeredKeyboardDriver.K_PCATKeyT2
                Label4.Text = "Korean PC/AT 101-Key Compatible Keyboard/MS Natural Keyboard (Type 2)"
                ComboBox1.SelectedIndex = 0
            Case KeyboardDrivers.LayeredKeyboardDriver.K_PCATKeyT3
                Label4.Text = "Korean PC/AT 101-Key Compatible Keyboard/MS Natural Keyboard (Type 3)"
                ComboBox1.SelectedIndex = 0
            Case KeyboardDrivers.LayeredKeyboardDriver.K_103106Key
                Label4.Text = "Korean Keyboard (103/106 Key)"
                ComboBox1.SelectedIndex = 0
            Case KeyboardDrivers.LayeredKeyboardDriver.J_106109Key
                Label4.Text = "Japanese Keyboard (106/109 Key)"
                ComboBox1.SelectedIndex = 0
        End Select
        ' Do checks at startup
        If (CurrentKeyboardDriver - 1) = ComboBox1.SelectedIndex Then
            Label6.Visible = True
            OK_Button.Enabled = False
        Else
            Label6.Visible = False
            OK_Button.Enabled = True
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If (CurrentKeyboardDriver - 1) = ComboBox1.SelectedIndex Then
            Label6.Visible = True
            OK_Button.Enabled = False
        Else
            Label6.Visible = False
            OK_Button.Enabled = True
        End If
    End Sub
End Class
