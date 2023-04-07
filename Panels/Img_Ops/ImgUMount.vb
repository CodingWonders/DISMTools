Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class ImgUMount

    Dim UMountOperations() As String = New String(1) {"Save changes and unmount", "Discard changes and unmount"}

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If RadioButton1.Checked = True Then
            ProgressPanel.UMountLocalDir = True
            ProgressPanel.RandomMountDir = ""   ' Hope there isn't anything to set here
            ProgressPanel.MountDir = MainForm.MountDir
        Else
            ProgressPanel.UMountLocalDir = False
            ' Determine if given mount dir exists
            If Directory.Exists(TextBox1.Text) Then
                ' Detect whether the mount dir has an image mounted (I don't believe on what users claim, just to be sure)
                If MainForm.MountedImageMountDirs.Contains(TextBox1.Text) Then
                    ProgressPanel.RandomMountDir = TextBox1.Text
                Else
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    MsgBox("The specified directory isn't a valid mount directory.", vbOKOnly + vbCritical, Label1.Text)
                                Case "ESN"
                                    MsgBox("El directorio especificado no es un directorio de montaje válido.", vbOKOnly + vbCritical, Label1.Text)
                            End Select
                        Case 1
                            MsgBox("The specified directory isn't a valid mount directory.", vbOKOnly + vbCritical, Label1.Text)
                        Case 2
                            MsgBox("El directorio especificado no es un directorio de montaje válido.", vbOKOnly + vbCritical, Label1.Text)
                    End Select
                    Exit Sub
                End If
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                MsgBox("The mount directory doesn't exist.", vbOKOnly + vbCritical, Label1.Text)
                            Case "ESN"
                                MsgBox("El directorio de montaje no existe.", vbOKOnly + vbCritical, Label1.Text)
                        End Select
                    Case 1
                        MsgBox("The mount directory doesn't exist.", vbOKOnly + vbCritical, Label1.Text)
                    Case 2
                        MsgBox("El directorio de montaje no existe.", vbOKOnly + vbCritical, Label1.Text)
                End Select
                Exit Sub
            End If
        End If
        If ComboBox1.SelectedIndex = 0 Then
            ProgressPanel.UMountOp = 0
        ElseIf ComboBox1.SelectedIndex = 1 Then
            ProgressPanel.UMountOp = 1
        End If
        If CheckBox1.Checked Then
            ProgressPanel.CheckImgIntegrity = True
        Else
            ProgressPanel.CheckImgIntegrity = False
        End If
        If CheckBox2.Checked Then
            ProgressPanel.SaveToNewIndex = True
        Else
            ProgressPanel.SaveToNewIndex = False
        End If
        If MainForm.isProjectLoaded Then
            ProgressPanel.UMountImgIndex = MainForm.ImgIndex
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 21
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgUMount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedText = ""
        ComboBox1.Items.Clear()
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Unmount an image"
                        Label1.Text = Text
                        Label2.Text = "Please specify the options to unmount this image:"
                        Label3.Text = "The mount directory:"
                        Label4.Text = "Mount directory:"
                        Label7.Text = "Unmount operation:"
                        CheckBox1.Text = "Check image integrity"
                        CheckBox2.Text = "Append changes to another index"
                        Button1.Text = "Pick..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        FolderBrowserDialog1.Description = "Please specify a mount directory:"
                        RadioButton1.Text = "is loaded in the project"
                        RadioButton2.Text = "is located somewhere else"
                        UMountOperations(0) = "Save changes and unmount"
                        UMountOperations(1) = "Discard changes and unmount"
                        ComboBox1.Items.AddRange(UMountOperations)
                        GroupBox1.Text = "Mount directory"
                        GroupBox2.Text = "Additional options"
                    Case "ESN"
                        Text = "Desmontar una imagen"
                        Label1.Text = Text
                        Label2.Text = "Especifique las opciones para desmontar esta imagen:"
                        Label3.Text = "El directorio de montaje:"
                        Label4.Text = "Directorio de montaje:"
                        Label7.Text = "Operación de desmontaje:"
                        CheckBox1.Text = "Comprobar integridad de la imagen"
                        CheckBox2.Text = "Anexar los cambios en otro índice"
                        Button1.Text = "Escoger..."
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        FolderBrowserDialog1.Description = "Especifique un directorio de montaje:"
                        RadioButton1.Text = "está cargado en el proyecto"
                        RadioButton2.Text = "se ubica en otro lugar"
                        UMountOperations(0) = "Guardar cambios y desmontar"
                        UMountOperations(1) = "Descartar cambios y desmontar"
                        ComboBox1.Items.AddRange(UMountOperations)
                        GroupBox1.Text = "Directorio de montaje"
                        GroupBox2.Text = "Opciones adicionales"
                End Select
            Case 1
                Text = "Unmount an image"
                Label1.Text = Text
                Label2.Text = "Please specify the options to unmount this image:"
                Label3.Text = "The mount directory:"
                Label4.Text = "Mount directory:"
                Label7.Text = "Unmount operation:"
                CheckBox1.Text = "Check image integrity"
                CheckBox2.Text = "Append changes to another index"
                Button1.Text = "Pick..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                FolderBrowserDialog1.Description = "Please specify a mount directory:"
                RadioButton1.Text = "is loaded in the project"
                RadioButton2.Text = "is located somewhere else"
                UMountOperations(0) = "Save changes and unmount"
                UMountOperations(1) = "Discard changes and unmount"
                ComboBox1.Items.AddRange(UMountOperations)
                GroupBox1.Text = "Mount directory"
                GroupBox2.Text = "Additional options"
            Case 2
                Text = "Desmontar una imagen"
                Label1.Text = Text
                Label2.Text = "Especifique las opciones para desmontar esta imagen:"
                Label3.Text = "El directorio de montaje:"
                Label4.Text = "Directorio de montaje:"
                Label7.Text = "Operación de desmontaje:"
                CheckBox1.Text = "Comprobar integridad de la imagen"
                CheckBox2.Text = "Anexar los cambios en otro índice"
                Button1.Text = "Escoger..."
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                FolderBrowserDialog1.Description = "Especifique un directorio de montaje:"
                RadioButton1.Text = "está cargado en el proyecto"
                RadioButton2.Text = "se ubica en otro lugar"
                UMountOperations(0) = "Guardar cambios y desmontar"
                UMountOperations(1) = "Descartar cambios y desmontar"
                ComboBox1.Items.AddRange(UMountOperations)
                GroupBox1.Text = "Directorio de montaje"
                GroupBox2.Text = "Opciones adicionales"
        End Select
        ComboBox1.SelectedIndex = 0
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox1.BackColor = Color.FromArgb(31, 31, 31)
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox1.BackColor = Color.FromArgb(238, 238, 242)
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
        End If
        ComboBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        If RadioButton1.Checked Then
            Label4.Enabled = False
            TextBox1.Enabled = False
            Button1.Enabled = False
        Else
            Label4.Enabled = True
            TextBox1.Enabled = True
            Button1.Enabled = True
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            Label4.Enabled = False
            TextBox1.Enabled = False
            Button1.Enabled = False
        Else
            Label4.Enabled = True
            TextBox1.Enabled = True
            Button1.Enabled = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PopupImageManager.Location = Button1.PointToScreen(Point.Empty)
        If PopupImageManager.ShowDialog() = DialogResult.OK Then
            TextBox1.Text = PopupImageManager.selectedMntDir
            If TextBox1.Text = MainForm.MountDir Then
                TextBox1.Text = ""
                RadioButton1.Checked = True
                RadioButton2.Checked = False
            End If
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Select Case ComboBox1.SelectedIndex
            Case 0
                CheckBox1.Enabled = True
                CheckBox2.Enabled = True
            Case 1
                CheckBox1.Enabled = False
                CheckBox2.Enabled = False
        End Select
    End Sub
End Class
