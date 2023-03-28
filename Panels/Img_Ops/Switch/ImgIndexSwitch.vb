Imports System.Windows.Forms

Public Class ImgIndexSwitch

    Public indexNames(1024) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ProgressPanel.SwitchSourceImg = MainForm.SourceImg
        ProgressPanel.SwitchTarget = MainForm.MountDir
        ProgressPanel.SwitchSourceIndex = MainForm.ImgIndex
        ProgressPanel.SwitchTargetIndex = NumericUpDown1.Value
        ProgressPanel.SwitchTargetIndexName = Label5.Text
        If RadioButton1.Checked Then
            ProgressPanel.SwitchCommitSourceIndex = True
        Else
            ProgressPanel.SwitchCommitSourceIndex = False
        End If
        If MainForm.isReadOnly Then
            ProgressPanel.SwitchMountAsReadOnly = True
        Else
            ProgressPanel.SwitchMountAsReadOnly = False
        End If
        ProgressPanel.OperationNum = 996
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgIndexSwitch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Switch image indexes"
                        Label1.Text = Text
                        Label2.Text = "Image:"
                        Label3.Text = "When unmounting source index, what to do?"
                        Label4.Text = "Destination index to mount:"
                        Label6.Text = "This index has already been mounted"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        GroupBox1.Text = "Indexes"
                        RadioButton1.Text = "Save changes to index"
                        RadioButton2.Text = "Unmount discarding changes"
                    Case "ESN"
                        Text = "Cambiar índices de imagen"
                        Label1.Text = Text
                        Label2.Text = "Imagen:"
                        Label3.Text = "Al desmontar índice de origen, ¿qué hacer?"
                        Label4.Text = "Índice de destino a montar:"
                        Label6.Text = "Este índice ya está montado"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        GroupBox1.Text = "Índice"
                        RadioButton1.Text = "Guardar cambios en el índice"
                        RadioButton2.Text = "Desmontar descartando cambios"
                End Select
            Case 1
                Text = "Switch image indexes"
                Label1.Text = Text
                Label2.Text = "Image:"
                Label3.Text = "When unmounting source index, what to do?"
                Label4.Text = "Destination index to mount:"
                Label6.Text = "This index has already been mounted"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                GroupBox1.Text = "Indexes"
                RadioButton1.Text = "Save changes to index"
                RadioButton2.Text = "Unmount discarding changes"
            Case 2
                Text = "Cambiar índices de imagen"
                Label1.Text = Text
                Label2.Text = "Imagen:"
                Label3.Text = "Al desmontar índice de origen, ¿qué hacer?"
                Label4.Text = "Índice de destino a montar:"
                Label6.Text = "Este índice ya está montado"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                GroupBox1.Text = "Índice"
                RadioButton1.Text = "Guardar cambios en el índice"
                RadioButton2.Text = "Desmontar descartando cambios"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            GroupBox1.ForeColor = Color.White
            NumericUpDown1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            GroupBox1.ForeColor = Color.Black
            NumericUpDown1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        NumericUpDown1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
        Label5.Text = indexNames(NumericUpDown1.Value - 1)
        If Label5.Text = MainForm.imgMountedName Then
            Label6.Visible = True
            OK_Button.Enabled = False
        Else
            Label6.Visible = False
            OK_Button.Enabled = True
        End If
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        Label5.Text = indexNames(NumericUpDown1.Value - 1)
        If Label5.Text = MainForm.imgMountedName Then
            Label6.Visible = True
            OK_Button.Enabled = False
        Else
            Label6.Visible = False
            OK_Button.Enabled = True
        End If
    End Sub
End Class
