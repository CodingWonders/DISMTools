Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class ActiveInstAccessWarn

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ActiveInstAccessWarn_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "About active installation management"
                        Label1.Text = "You are about to enter the online installation management mode, which lets you perform changes to your active Windows installation." & CrLf & CrLf & _
                            "Given that this mode lets you modify your installation, you should be extremely careful when performing tasks with this program." & CrLf & CrLf & _
                            "If you carelessly perform an operation to an online image, you may break it, to the point of making the installation unbootable." & CrLf & CrLf & _
                            "We AREN'T RESPONSIBLE for any damage done to your active installation. If you are left with an unbootable system, you should re-install Windows (while backing up your files first, if possible)" & CrLf & CrLf & _
                            "If you understand this warning, and would like to proceed, click OK. Otherwise, click OK, and then click " & Quote & "Unload project" & Quote & ". This will end online installation management."
                        Label2.Text = "The current project will be unloaded."
                        OK_Button.Text = "OK"
                    Case "ESN"
                        Text = "Acerca de la administración de instalaciones activas"
                        Label1.Text = "Está a punto de entrar en el modo de administración de instalaciones en línea, que le permite realizar cambios a su instalación activa de Windows." & CrLf & CrLf & _
                            "Dado a que este modo le permite modificar su instalación, debería ser extremadamente cuidadoso al realizar tareas con este programa." & CrLf & CrLf & _
                            "Si realiza una operación a una imagen en línea sin tener cuidado, puede romperla, a tal punto que la instalación no pueda iniciar." & CrLf & CrLf & _
                            "Nosotros NO SOMOS RESPONSABLES de cualquier daño producido a su instalación activa. Si se queda con un sistema que no puede iniciar, debería reinstalar Windows (haciendo una copia de seguridad de sus archivos en primer lugar, si es posible)" & CrLf & CrLf & _
                            "Si entiende este aviso, y le gustaría seguir, haga clic en Aceptar. De lo contrario, haga clic en Aceptar, y luego en " & Quote & "Descargar proyecto" & Quote & ". Esto terminará la administración de instalaciones en línea."
                        Label2.Text = "El proyecto actual será descargado."
                        OK_Button.Text = "Aceptar"
                    Case "FRA"
                        Text = "À propos de la gestion active des installations"
                        Label1.Text = "Vous êtes sur le point d'entrer dans le mode de gestion de l'installation en ligne, qui vous permet d'apporter des modifications à votre installation Windows active." & CrLf & CrLf & _
                            "Étant donné que ce mode vous permet de modifier votre installation, vous devez être extrêmement prudent lorsque vous effectuez des tâches avec ce programme." & CrLf & CrLf & _
                            "Si vous effectuez sans précaution une opération sur une image en ligne, vous risquez de la casser, au point de rendre l'installation non amorçable." & CrLf & CrLf & _
                            "Nous NE SOMMES PAS RESPONSABLES des dommages causés à votre installation active. Si vous vous retrouvez avec un système non amorçable, vous devez réinstaller Windows (en sauvegardant d'abord vos fichiers, si possible)." & CrLf & CrLf & _
                            "Si vous comprenez cet avertissement et que vous souhaitez continuer, cliquez sur OK. Sinon, cliquez sur OK, puis sur " & Quote & "Décharger projet" & Quote & ". Cela mettra fin à la gestion de l'installation en ligne."
                        Label2.Text = "Le projet actuel sera déchargé."
                        OK_Button.Text = "OK"
                End Select
            Case 1
                Text = "About active installation management"
                Label1.Text = "You are about to enter the online installation management mode, which lets you perform changes to your active Windows installation." & CrLf & CrLf & _
                    "Given that this mode lets you modify your installation, you should be extremely careful when performing tasks with this program." & CrLf & CrLf & _
                    "If you carelessly perform an operation to an online image, you may break it, to the point of making the installation unbootable." & CrLf & CrLf & _
                    "We AREN'T RESPONSIBLE for any damage done to your active installation. If you are left with an unbootable system, you should re-install Windows (while backing up your files first, if possible)" & CrLf & CrLf & _
                    "If you understand this warning, and would like to proceed, click OK. Otherwise, click OK, and then click " & Quote & "Unload project" & Quote & ". This will end online installation management."
                Label2.Text = "The current project will be unloaded."
                OK_Button.Text = "OK"
            Case 2
                Text = "Acerca de la administración de instalaciones activas"
                Label1.Text = "Está a punto de entrar en el modo de administración de instalaciones en línea, que le permite realizar cambios a su instalación activa de Windows." & CrLf & CrLf & _
                    "Dado a que este modo le permite modificar su instalación, debería ser extremadamente cuidadoso al realizar tareas con este programa." & CrLf & CrLf & _
                    "Si realiza una operación a una imagen en línea sin tener cuidado, puede romperla, a tal punto que la instalación no pueda iniciar." & CrLf & CrLf & _
                    "Nosotros NO SOMOS RESPONSABLES de cualquier daño producido a su instalación activa. Si se queda con un sistema que no puede iniciar, debería reinstalar Windows (haciendo una copia de seguridad de sus archivos en primer lugar, si es posible)" & CrLf & CrLf & _
                    "Si entiende este aviso, y le gustaría seguir, haga clic en Aceptar. De lo contrario, haga clic en Aceptar, y luego en " & Quote & "Descargar proyecto" & Quote & ". Esto terminará la administración de instalaciones en línea."
                Label2.Text = "El proyecto actual será descargado."
                OK_Button.Text = "Aceptar"
            Case 3
                Text = "À propos de la gestion active des installations"
                Label1.Text = "Vous êtes sur le point d'entrer dans le mode de gestion de l'installation en ligne, qui vous permet d'apporter des modifications à votre installation Windows active." & CrLf & CrLf & _
                    "Étant donné que ce mode vous permet de modifier votre installation, vous devez être extrêmement prudent lorsque vous effectuez des tâches avec ce programme." & CrLf & CrLf & _
                    "Si vous effectuez sans précaution une opération sur une image en ligne, vous risquez de la casser, au point de rendre l'installation non amorçable." & CrLf & CrLf & _
                    "Nous NE SOMMES PAS RESPONSABLES des dommages causés à votre installation active. Si vous vous retrouvez avec un système non amorçable, vous devez réinstaller Windows (en sauvegardant d'abord vos fichiers, si possible)." & CrLf & CrLf & _
                    "Si vous comprenez cet avertissement et que vous souhaitez continuer, cliquez sur OK. Sinon, cliquez sur OK, puis sur " & Quote & "Décharger projet" & Quote & ". Cela mettra fin à la gestion de l'installation en ligne."
                Label2.Text = "Le projet actuel sera déchargé."
                OK_Button.Text = "OK"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        Beep()
    End Sub
End Class
