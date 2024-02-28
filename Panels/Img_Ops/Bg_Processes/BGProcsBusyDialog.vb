Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

Public Class BGProcsBusyDialog

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub BGProcsBusyDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label1.Text = "We're still gathering image information"
                        Label2.Text = "Once we finish this process, you can begin performing image tasks. This usually takes a couple of minutes, but this can depend on the image and the speed of your computer." & CrLf & CrLf & _
                            "You can check the status of this background process at any time by clicking the icon on the bottom left."
                        OK_Button.Text = "OK"
                    Case "ESN"
                        Label1.Text = "Aún estamos recopilando información de la imagen"
                        Label2.Text = "Cuando terminemos este proceso, puede comenzar a realizar operaciones con la imagen. Esto suele tardar unos minutos, pero esto puede depender en la imagen y el rendimiento de su equipo." & CrLf & CrLf & _
                            "Puede comprobar el estado de este proceso en segundo plano en cualquier momento haciendo clic en el icono en la parte inferior izquierda."
                        OK_Button.Text = "Aceptar"
                    Case "FRA"
                        Label1.Text = "Nous continuons à recueillir des informations de l'image"
                        Label2.Text = "Une fois ce processus terminé, vous pouvez commencer à exécuter les tâches liées à l'image. Cela prend généralement quelques minutes, mais cela peut dépendre de l'image et de la vitesse de votre ordinateur." & CrLf & CrLf & _
                            "Vous pouvez à tout moment vérifier l'état de ce processus en arrière plan en cliquant sur l'icône en bas à gauche."
                        OK_Button.Text = "OK"
                    Case "PTB", "PTG"
                        Label1.Text = "Ainda estamos a recolher informações sobre a imagem"
                        Label2.Text = "Quando terminarmos este processo, pode começar a executar tarefas de imagem. Normalmente, isto demora alguns minutos, mas pode depender da imagem e da velocidade do seu computador." & CrLf & CrLf & _
                            "Pode verificar o estado deste processo em segundo plano a qualquer momento, clicando no ícone no canto inferior esquerdo."
                        OK_Button.Text = "OK"
                End Select
            Case 1
                Label1.Text = "We're still gathering image information"
                Label2.Text = "Once we finish this process, you can begin performing image tasks. This usually takes a couple of minutes, but this can depend on the image and the speed of your computer." & CrLf & CrLf & _
                    "You can check the status of this background process at any time by clicking the icon on the bottom left."
                OK_Button.Text = "OK"
            Case 2
                Label1.Text = "Aún estamos recopilando información de la imagen"
                Label2.Text = "Cuando terminemos este proceso, puede comenzar a realizar operaciones con la imagen. Esto suele tardar unos minutos, pero esto puede depender en la imagen y el rendimiento de su equipo." & CrLf & CrLf & _
                    "Puede comprobar el estado de este proceso en segundo plano en cualquier momento haciendo clic en el icono en la parte inferior izquierda."
                OK_Button.Text = "Aceptar"
            Case 3
                Label1.Text = "Nous continuons à recueillir des informations de l'image"
                Label2.Text = "Une fois ce processus terminé, vous pouvez commencer à exécuter les tâches liées à l'image. Cela prend généralement quelques minutes, mais cela peut dépendre de l'image et de la vitesse de votre ordinateur." & CrLf & CrLf & _
                    "Vous pouvez à tout moment vérifier l'état de ce processus en arrière plan en cliquant sur l'icône en bas à gauche."
                OK_Button.Text = "OK"
            Case 4
                Label1.Text = "Ainda estamos a recolher informações sobre a imagem"
                Label2.Text = "Quando terminarmos este processo, pode começar a executar tarefas de imagem. Normalmente, isto demora alguns minutos, mas pode depender da imagem e da velocidade do seu computador." & CrLf & CrLf & _
                    "Pode verificar o estado deste processo em segundo plano a qualquer momento, clicando no ícone no canto inferior esquerdo."
                OK_Button.Text = "OK"
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
        Beep()
    End Sub
End Class
