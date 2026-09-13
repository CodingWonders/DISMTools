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
        BackColor = CurrentTheme.BackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        If MainForm.pinState = 0 Then
            PictureBox1.Image = GetGlyphResource("dlg_unpin")
        ElseIf MainForm.pinState = 1 Then
            PictureBox1.Image = GetGlyphResource("dlg_pin")
        End If
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        ControlPaint.DrawBorder(e.Graphics, Panel1.ClientRectangle, CurrentTheme.AccentColors(1), ButtonBorderStyle.Solid)
    End Sub

    Sub ChangePBValue(ByRef Divider As Integer)
        ProgressBar1.Value = (ProgressBar1.Value + (ProgressBar1.Maximum / Divider))
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        If MainForm.pinState = 0 Then
            PictureBox1.Image = GetGlyphResource("dlg_pin")
            MainForm.pinState = 1
            AddHandler Panel1.MouseMove, AddressOf Panel1_MouseMove
            AddHandler Panel1.MouseUp, AddressOf Panel1_MouseUp
            AddHandler Panel1.MouseDown, AddressOf Panel1_MouseDown
            ShowInTaskbar = True
        ElseIf MainForm.pinState = 1 Then
            PictureBox1.Image = GetGlyphResource("dlg_unpin")
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
            Label1.Text = LocalizationService.ForSection("BGProcDetails.VisibleChanged")("Gathering.Image.Label")
            Label3.Text = LocalizationService.ForSection("BGProcDetails.VisibleChanged")("Processes.Take.Time.Label")
            BackColor = CurrentTheme.BackgroundColor
            ForeColor = CurrentTheme.ForegroundColor
            If MainForm.pinState = 0 Then
                PictureBox1.Image = GetGlyphResource("dlg_unpin")
            ElseIf MainForm.pinState = 1 Then
                PictureBox1.Image = GetGlyphResource("dlg_pin")
            End If
        End If
    End Sub

    Private Sub BGProcDetails_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        If WindowState = FormWindowState.Maximized Then
            WindowState = FormWindowState.Normal
        End If
    End Sub
End Class
