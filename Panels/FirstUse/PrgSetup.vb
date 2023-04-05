Imports System.Drawing.Drawing2D
Imports System.IO

Public Class PrgSetup

    Dim btnToolTip As New ToolTip()
    Private isMouseDown As Boolean = False
    Private mouseOffset As Point
    Dim pageInt As Integer = 0

    Private Sub minBox_MouseEnter(sender As Object, e As EventArgs) Handles minBox.MouseEnter
        minBox.Image = My.Resources.minBox_focus
    End Sub

    Private Sub minBox_MouseLeave(sender As Object, e As EventArgs) Handles minBox.MouseLeave
        minBox.Image = My.Resources.minBox
    End Sub

    Private Sub minBox_MouseDown(sender As Object, e As MouseEventArgs) Handles minBox.MouseDown
        minBox.Image = My.Resources.minBox_down
    End Sub

    Private Sub minBox_MouseUp(sender As Object, e As MouseEventArgs) Handles minBox.MouseUp
        minBox.Image = My.Resources.minBox_focus
    End Sub

    Private Sub minBox_MouseHover(sender As Object, e As EventArgs) Handles minBox.MouseHover
        btnToolTip.SetToolTip(sender, "Minimize")
    End Sub

    Private Sub minBox_Click(sender As Object, e As EventArgs) Handles minBox.Click
        WindowState = FormWindowState.Minimized
    End Sub

    Private Sub closeBox_MouseEnter(sender As Object, e As EventArgs) Handles closeBox.MouseEnter
        closeBox.Image = My.Resources.closebox_focus
    End Sub

    Private Sub closeBox_MouseLeave(sender As Object, e As EventArgs) Handles closeBox.MouseLeave
        closeBox.Image = My.Resources.closebox
    End Sub

    Private Sub closeBox_MouseDown(sender As Object, e As MouseEventArgs) Handles closeBox.MouseDown
        closeBox.Image = My.Resources.closebox_down
    End Sub

    Private Sub closeBox_MouseUp(sender As Object, e As MouseEventArgs) Handles closeBox.MouseUp
        closeBox.Image = My.Resources.closebox_focus
    End Sub

    Private Sub closeBox_MouseHover(sender As Object, e As EventArgs) Handles closeBox.MouseHover
        btnToolTip.SetToolTip(sender, "Close")
    End Sub

    Private Sub closeBox_Click(sender As Object, e As EventArgs) Handles closeBox.Click
        Close()
    End Sub

    Private Sub wndControlPanel_MouseDown(sender As Object, e As MouseEventArgs) Handles wndControlPanel.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Left Then
            ' Get the new position
            mouseOffset = New Point(-e.X, -e.Y)
            ' Set that left button is pressed
            isMouseDown = True
        End If
    End Sub

    Private Sub wndControlPanel_MouseMove(sender As Object, e As MouseEventArgs) Handles wndControlPanel.MouseMove
        If isMouseDown Then
            Dim mousePos As Point = Control.MousePosition
            ' Get the new form position
            mousePos.Offset(mouseOffset.X, mouseOffset.Y)
            Location = mousePos
        End If
    End Sub

    Private Sub wndControlPanel_MouseUp(sender As Object, e As MouseEventArgs) Handles wndControlPanel.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Left Then
            isMouseDown = False
        End If
    End Sub

    Private Sub Next_Button_Click(sender As Object, e As EventArgs) Handles Next_Button.Click
        If pageInt = 4 Then
            MainForm.SaveDTSettings()
            Close()
        End If
        pageInt += 1
        Select Case pageInt
            Case 0
                WelcomePanel.Visible = True
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 1
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = True
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 2
                MainForm.ColorMode = ComboBox1.SelectedIndex
                MainForm.Language = ComboBox2.SelectedIndex
                MainForm.LogFont = ComboBox3.SelectedItem
                MainForm.LogFontSize = NumericUpDown1.Value
                MainForm.LogFontIsBold = Toggle1.Checked
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = True
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 3
                MainForm.LogFile = TextBox2.Text
                MainForm.LogLevel = TrackBar1.Value + 1
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = True
                FinishPanel.Visible = False
            Case 4
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = True
        End Select
        If pageInt = 4 Then
            Next_Button.Text = "Finish"
            Cancel_Button.Enabled = False
            closeBox.Enabled = False
        Else
            Next_Button.Text = "Next"
            Cancel_Button.Enabled = True
            closeBox.Enabled = True
        End If
        If pageInt = 0 Then
            Back_Button.Enabled = False
        Else
            Back_Button.Enabled = True
        End If
    End Sub

    Private Sub Back_Button_Click(sender As Object, e As EventArgs) Handles Back_Button.Click
        pageInt -= 1
        Select Case pageInt
            Case 0
                WelcomePanel.Visible = True
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 1
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = True
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 2
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = True
                ModulesPanel.Visible = False
                FinishPanel.Visible = False
            Case 3
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = True
                FinishPanel.Visible = False
            Case 4
                WelcomePanel.Visible = False
                CustomizationPanel.Visible = False
                LogsPanel.Visible = False
                ModulesPanel.Visible = False
                FinishPanel.Visible = True
        End Select
        If pageInt = 4 Then
            Next_Button.Text = "Finish"
            Cancel_Button.Enabled = False
            closeBox.Enabled = False
        Else
            Next_Button.Text = "Next"
            Cancel_Button.Enabled = True
            closeBox.Enabled = True
        End If
        If pageInt = 0 Then
            Back_Button.Enabled = False
        Else
            Back_Button.Enabled = True
        End If
    End Sub

    Private Sub Panel4_Paint(sender As Object, e As PaintEventArgs) Handles Panel4.Paint
        Dim brush As LinearGradientBrush = New LinearGradientBrush(Panel4.ClientRectangle, TrackBar1.BackColor, Color.Transparent, LinearGradientMode.Horizontal)
        e.Graphics.FillRectangle(brush, Panel4.ClientRectangle)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ' MainForm.SaveDTSettings()
        Options.ShowDialog(Me)
    End Sub

    Private Sub PrgSetup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Generate new settings file and load it
        MainForm.GenerateDTSettings()
        MainForm.LoadDTSettings(1)
        GetSystemFonts()
        TextBox2.Text = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\Logs\DISM\DISM.log"
        MainForm.LogFile = TextBox2.Text
    End Sub

    Sub GetSystemFonts()
        ComboBox3.Items.Clear()
        For Each fntFamily As FontFamily In FontFamily.Families
            ComboBox3.Items.Add(fntFamily.Name)
        Next
        ComboBox3.SelectedItem = "Courier New"
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        TextBox1.Font = New Font(ComboBox3.Text, NumericUpDown1.Value, If(Toggle1.Checked, FontStyle.Bold, FontStyle.Regular))
        MainForm.LogFont = ComboBox3.SelectedItem
        MainForm.LogFontSize = NumericUpDown1.Value
        MainForm.LogFontIsBold = Toggle1.Checked
    End Sub

    Private Sub Toggle1_CheckedChanged(sender As Object, e As EventArgs) Handles Toggle1.CheckedChanged
        TextBox1.Font = New Font(ComboBox3.Text, NumericUpDown1.Value, If(Toggle1.Checked, FontStyle.Bold, FontStyle.Regular))
        MainForm.LogFont = ComboBox3.SelectedItem
        MainForm.LogFontSize = NumericUpDown1.Value
        MainForm.LogFontIsBold = Toggle1.Checked
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        TextBox1.Font = New Font(ComboBox3.Text, NumericUpDown1.Value, If(Toggle1.Checked, FontStyle.Bold, FontStyle.Regular))
        MainForm.LogFont = ComboBox3.SelectedItem
        MainForm.LogFontSize = NumericUpDown1.Value
        MainForm.LogFontIsBold = Toggle1.Checked
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        MainForm.ColorMode = ComboBox1.SelectedIndex
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        MainForm.Language = ComboBox2.SelectedIndex
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Select Case TrackBar1.Value
                            Case 0
                                Label11.Text = "Errors (Log level 1)"
                                Label16.Text = "The log file should only display errors after performing an image operation."
                            Case 1
                                Label11.Text = "Errors and warnings (Log level 2)"
                                Label16.Text = "The log file should display errors and warnings after performing an image operation."
                            Case 2
                                Label11.Text = "Errors, warnings and information messages (Log level 3)"
                                Label16.Text = "The log file should display errors, warnings and information messages after performing an image operation."
                            Case 3
                                Label11.Text = "Errors, warnings, information and debug messages (Log level 4)"
                                Label16.Text = "The log file should display errors, warnings, information and debug messages after performing an image operation."
                        End Select
                    Case "ESN"
                        Select Case TrackBar1.Value
                            Case 0
                                Label11.Text = "Errores (Nivel 1)"
                                Label16.Text = "El archivo de registro solo debe mostrar errores tras realizar una operación."
                            Case 1
                                Label11.Text = "Errores y advertencias (Nivel 2)"
                                Label16.Text = "El archivo de registro debe mostrar errores y advertencias tras realizar una operación."
                            Case 2
                                Label11.Text = "Errores, advertencias y mensajes de información (Nivel 3)"
                                Label16.Text = "El archivo de registro debe mostrar errores, advertencias y mensajes de información tras realizar una operación."
                            Case 3
                                Label11.Text = "Errores, advertencias, mensajes de información y de depuración (Nivel 4)"
                                Label16.Text = "El archivo de registro debe mostrar errores, advertencias, mensajes de información y de depuración tras realizar una operación."
                        End Select
                End Select
            Case 1
                Select Case TrackBar1.Value
                    Case 0
                        Label11.Text = "Errors (Log level 1)"
                        Label16.Text = "The log file should only display errors after performing an image operation."
                    Case 1
                        Label11.Text = "Errors and warnings (Log level 2)"
                        Label16.Text = "The log file should display errors and warnings after performing an image operation."
                    Case 2
                        Label11.Text = "Errors, warnings and information messages (Log level 3)"
                        Label16.Text = "The log file should display errors, warnings and information messages after performing an image operation."
                    Case 3
                        Label11.Text = "Errors, warnings, information and debug messages (Log level 4)"
                        Label16.Text = "The log file should display errors, warnings, information and debug messages after performing an image operation."
                End Select
            Case 2
                Select Case TrackBar1.Value
                    Case 0
                        Label11.Text = "Errores (Nivel 1)"
                        Label16.Text = "El archivo de registro solo debe mostrar errores tras realizar una operación."
                    Case 1
                        Label11.Text = "Errores y advertencias (Nivel 2)"
                        Label16.Text = "El archivo de registro debe mostrar errores y advertencias tras realizar una operación."
                    Case 2
                        Label11.Text = "Errores, advertencias y mensajes de información (Nivel 3)"
                        Label16.Text = "El archivo de registro debe mostrar errores, advertencias y mensajes de información tras realizar una operación."
                    Case 3
                        Label11.Text = "Errores, advertencias, mensajes de información y de depuración (Nivel 4)"
                        Label16.Text = "El archivo de registro debe mostrar errores, advertencias, mensajes de información y de depuración tras realizar una operación."
                End Select
        End Select
        MainForm.LogLevel = TrackBar1.Value + 1
    End Sub
End Class