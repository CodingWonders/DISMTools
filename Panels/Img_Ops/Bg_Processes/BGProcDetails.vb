Public Class BGProcDetails

    Private isMouseDown As Boolean = False
    Private mouseOffset As Point

    Private Sub BGProcDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If BGProcNotify.Visible Then BGProcNotify.Close()
        Control.CheckForIllegalCrossThreadCalls = False
        If Environment.OSVersion.Version.Major = 10 Then    ' The Left property also includes the window shadows on Windows 10 and 11
            Location = New Point(MainForm.Left + 8, MainForm.Top + MainForm.StatusStrip.Top - (75 + MainForm.StatusStrip.Height))
        ElseIf Environment.OSVersion.Version.Major = 6 Then
            If Environment.OSVersion.Version.Minor = 1 Then ' The same also applies to Windows 7
                Location = New Point(MainForm.Left + 8, MainForm.Top + MainForm.StatusStrip.Top - (75 + MainForm.StatusStrip.Height))
            Else
                Location = New Point(MainForm.Left + 8, MainForm.Top + MainForm.StatusStrip.Top - MainForm.StatusStrip.Height - 75)
            End If
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(37, 37, 38)
            ForeColor = Color.White
            If MainForm.pinState = 0 Then
                PictureBox1.Image = My.Resources.dlg_unpin_dark
            ElseIf MainForm.pinState = 1 Then
                PictureBox1.Image = My.Resources.dlg_pin_dark
            End If
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.White
            ForeColor = Color.Black
            If MainForm.pinState = 0 Then
                PictureBox1.Image = My.Resources.dlg_unpin
            ElseIf MainForm.pinState = 1 Then
                PictureBox1.Image = My.Resources.dlg_pin
            End If
        End If
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        ControlPaint.DrawBorder(e.Graphics, Panel1.ClientRectangle, Color.FromArgb(0, 122, 204), ButtonBorderStyle.Solid)
    End Sub

    Sub ChangePBValue(ByRef Divider As Integer)
        ProgressBar1.Value = (ProgressBar1.Value + (ProgressBar1.Maximum / Divider))
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        If MainForm.pinState = 0 Then
            If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
                PictureBox1.Image = My.Resources.dlg_pin_dark
            ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
                PictureBox1.Image = My.Resources.dlg_pin
            End If
            MainForm.pinState = 1
            AddHandler Panel1.MouseMove, AddressOf Panel1_MouseMove
            AddHandler Panel1.MouseUp, AddressOf Panel1_MouseUp
            AddHandler Panel1.MouseDown, AddressOf Panel1_MouseDown
            ShowInTaskbar = True
        ElseIf MainForm.pinState = 1 Then
            If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
                PictureBox1.Image = My.Resources.dlg_unpin_dark
            ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
                PictureBox1.Image = My.Resources.dlg_unpin
            End If
            MainForm.pinState = 0
            RemoveHandler Panel1.MouseMove, AddressOf Panel1_MouseMove
            RemoveHandler Panel1.MouseUp, AddressOf Panel1_MouseUp
            RemoveHandler Panel1.MouseDown, AddressOf Panel1_MouseDown
            ShowInTaskbar = False
            If Environment.OSVersion.Version.Major = 10 Then    ' The Left property also includes the window shadows on Windows 10 and 11
                Location = New Point(MainForm.Left + 8, MainForm.Top + MainForm.StatusStrip.Top - (75 + MainForm.StatusStrip.Height))
            ElseIf Environment.OSVersion.Version.Major = 6 Then
                If Environment.OSVersion.Version.Minor = 1 Then ' The same also applies to Windows 7
                    Location = New Point(MainForm.Left + 8, MainForm.Top + MainForm.StatusStrip.Top - (75 + MainForm.StatusStrip.Height))
                Else
                    Location = New Point(MainForm.Left + 8, MainForm.Top + MainForm.StatusStrip.Top - MainForm.StatusStrip.Height - 75)
                End If
            End If
        End If
    End Sub

    Private Sub Panel1_MouseDown(sender As Object, e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            ' Get the new position
            mouseOffset = New Point(-e.X, -e.Y)
            ' Set that left button is pressed
            isMouseDown = True
        End If
    End Sub

    Private Sub Panel1_MouseUp(sender As Object, e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            isMouseDown = False
        End If
    End Sub

    Private Sub Panel1_MouseMove(sender As Object, e As MouseEventArgs)
        If isMouseDown Then
            Dim mousePos As Point = Control.MousePosition
            ' Get the new form position
            mousePos.Offset(mouseOffset.X, mouseOffset.Y)
            Location = mousePos
        End If
    End Sub

    Private Sub BGProcDetails_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
        If Visible Then
            If MainForm.pinState <> 1 Then
                If Environment.OSVersion.Version.Major = 10 Then    ' The Left property also includes the window shadows on Windows 10 and 11
                    Location = New Point(MainForm.Left + 8, MainForm.Top + MainForm.StatusStrip.Top - (75 + MainForm.StatusStrip.Height))
                ElseIf Environment.OSVersion.Version.Major = 6 Then
                    If Environment.OSVersion.Version.Minor = 1 Then ' The same also applies to Windows 7
                        Location = New Point(MainForm.Left + 8, MainForm.Top + MainForm.StatusStrip.Top - (75 + MainForm.StatusStrip.Height))
                    Else
                        Location = New Point(MainForm.Left + 8, MainForm.Top + MainForm.StatusStrip.Top - MainForm.StatusStrip.Height - 75)
                    End If
                End If
            End If
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label1.Text = "Gathering image information..."
                            Label3.Text = "These processes may take some time to complete"
                        Case "ESN"
                            Label1.Text = "Recopilando información de la imagen..."
                            Label3.Text = "Estos procesos podrían tardar algo de tiempo en completar"
                        Case "FRA"
                            Label1.Text = "Collecte des informations de l'image en cours..."
                            Label3.Text = "Ces processus peuvent prendre un certain temps"
                    End Select
                Case 1
                    Label1.Text = "Gathering image information..."
                    Label3.Text = "These processes may take some time to complete"
                Case 2
                    Label1.Text = "Recopilando información de la imagen..."
                    Label3.Text = "Estos procesos podrían tardar algo de tiempo en completar"
                Case 3
                    Label1.Text = "Collecte des informations de l'image en cours..."
                    Label3.Text = "Ces processus peuvent prendre un certain temps"
            End Select
            If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
                BackColor = Color.FromArgb(37, 37, 38)
                ForeColor = Color.White
                If MainForm.pinState = 0 Then
                    PictureBox1.Image = My.Resources.dlg_unpin_dark
                ElseIf MainForm.pinState = 1 Then
                    PictureBox1.Image = My.Resources.dlg_pin_dark
                End If
            ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
                BackColor = Color.White
                ForeColor = Color.Black
                If MainForm.pinState = 0 Then
                    PictureBox1.Image = My.Resources.dlg_unpin
                ElseIf MainForm.pinState = 1 Then
                    PictureBox1.Image = My.Resources.dlg_pin
                End If
            End If
        End If
    End Sub
End Class