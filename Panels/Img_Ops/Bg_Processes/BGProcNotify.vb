Imports System.Threading

Public Class BGProcNotify

    Dim opacityFade As Single

    Private Sub BGProcNotify_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Opacity = 100
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label1.Text = "This project has been loaded successfully"
                        Label2.Text = "The program is now gathering image information in the background. This may take some time."
                    Case "ESN"
                        Label1.Text = "Este proyecto ha sido cargado satisfactoriamente"
                        Label2.Text = "El programa está recopilando información de la imagen en segundo plano. Esto podría llevar algo de tiempo."
                    Case "FRA"
                        Label1.Text = "Ce projet a été chargé avec succès"
                        Label2.Text = "Le programme recueille maintenant des informations sur l'image en arrière-plan. Cela peut prendre un certain temps."
                    Case "PTB", "PTG"
                        Label1.Text = "Este projeto foi carregado com sucesso"
                        Label2.Text = "O programa está agora a recolher informações sobre a imagem em segundo plano. Isto pode demorar algum tempo"
                    Case "ITA"
                        Label1.Text = "Il progetto è stato caricato con successo"
                        Label2.Text = "Il programma sta raccogliendo informazioni sull'immagine in background. Questa operazione potrebbe richiedere del tempo"
                End Select
            Case 1
                Label1.Text = "This project has been loaded successfully"
                Label2.Text = "The program is now gathering image information in the background. This may take some time."
            Case 2
                Label1.Text = "Este proyecto ha sido cargado satisfactoriamente"
                Label2.Text = "El programa está recopilando información de la imagen en segundo plano. Esto podría llevar algo de tiempo."
            Case 3
                Label1.Text = "Ce projet a été chargé avec succès"
                Label2.Text = "Le programme recueille maintenant des informations sur l'image en arrière-plan. Cela peut prendre un certain temps."
            Case 4
                Label1.Text = "Este projeto foi carregado com sucesso"
                Label2.Text = "O programa está agora a recolher informações sobre a imagem em segundo plano. Isto pode demorar algum tempo"
            Case 5
                Label1.Text = "Il progetto è stato caricato con successo"
                Label2.Text = "Il programma sta raccogliendo informazioni sull'immagine in background. Questa operazione potrebbe richiedere del tempo"
        End Select
        If Environment.OSVersion.Version.Major = 10 Then    ' The Left property also includes the window shadows on Windows 10 and 11
            Location = New Point(MainForm.Left + 8, MainForm.Top + MainForm.StatusStrip.Top - (7 + MainForm.StatusStrip.Height))
        ElseIf Environment.OSVersion.Version.Major = 6 Then
            If Environment.OSVersion.Version.Minor = 1 Then ' The same also applies to Windows 7
                Location = New Point(MainForm.Left + 8, MainForm.Top + MainForm.StatusStrip.Top - (7 + MainForm.StatusStrip.Height))
            Else
                Location = New Point(MainForm.Left + 8, MainForm.Top + MainForm.StatusStrip.Top - MainForm.StatusStrip.Height - 7)
            End If
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(37, 37, 38)
            ForeColor = Color.White
            PictureBox1.Image = New Bitmap(My.Resources.close_glyph_dark)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.White
            ForeColor = Color.Black
            PictureBox1.Image = New Bitmap(My.Resources.close_glyph)
        End If
        Timer1.Enabled = True
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        For Me.opacityFade = 1 To 0 Step -0.005
            Opacity = opacityFade
            Refresh()
        Next opacityFade
        Timer1.Enabled = False
        Timer1.Stop()
        Close()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        ControlPaint.DrawBorder(e.Graphics, Panel1.ClientRectangle, If(MainForm.ColorSchemes = 0, Color.FromArgb(53, 153, 41), Color.FromArgb(0, 122, 204)), ButtonBorderStyle.Solid)
    End Sub
End Class