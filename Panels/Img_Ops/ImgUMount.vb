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
                If Directory.Exists(TextBox1.Text & "\Windows") Then
                    ProgressPanel.RandomMountDir = TextBox1.Text    ' Assume it's valid
                Else
                    Do Until Directory.Exists(TextBox1.Text) And Directory.Exists(TextBox1.Text & "\Windows")
                        If Not Directory.Exists(TextBox1.Text) Or Not Directory.Exists(TextBox1.Text & "\Windows") Then
                            Button1.PerformClick()
                        End If
                    Loop
                    ProgressPanel.RandomMountDir = TextBox1.Text    ' Assume it's valid
                End If
            Else
                Do Until Directory.Exists(TextBox1.Text) And Directory.Exists(TextBox1.Text & "\Windows")
                    If Not Directory.Exists(TextBox1.Text) Or Not Directory.Exists(TextBox1.Text & "\Windows") Then
                        Button1.PerformClick()
                    End If
                Loop
                ProgressPanel.RandomMountDir = TextBox1.Text
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
                        Button1.Text = "Browse..."
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
                        Button1.Text = "Examinar..."
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
                Button1.Text = "Browse..."
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
                Button1.Text = "Examinar..."
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
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If MainForm.MountedImageMountDirs.Contains(FolderBrowserDialog1.SelectedPath) Then
                TextBox1.Text = FolderBrowserDialog1.SelectedPath
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                MsgBox("The directory specified isn't a valid mount directory. Please select a valid directory and try again." & CrLf & CrLf & "You can get some help finding a valid mount directory with the mounted image manager, which is located in " & Quote & "Tools > Mounted image manager" & Quote, vbOKOnly + vbCritical, "Unmount an image")
                                Exit Sub
                            Case "ESN"
                                MsgBox("El directorio especificado no es un directorio de montaje válido. Seleccione uno válido e inténtelo de nuevo." & CrLf & CrLf & "El administrador de imágenes montadas, que está ubicado en " & Quote & "Herramientas > Administrador de imágenes montadas" & Quote & ", le puede ayudar a encontrar un directorio de montaje válido", vbOKOnly + vbCritical, "Desmontar una imagen")
                                Exit Sub
                        End Select
                    Case 1
                        MsgBox("The directory specified isn't a valid mount directory. Please select a valid directory and try again." & CrLf & CrLf & "You can get some help finding a valid mount directory with the mounted image manager, which is located in " & Quote & "Tools > Mounted image manager" & Quote, vbOKOnly + vbCritical, "Unmount an image")
                        Exit Sub
                    Case 2
                        MsgBox("El directorio especificado no es un directorio de montaje válido. Seleccione uno válido e inténtelo de nuevo." & CrLf & CrLf & "El administrador de imágenes montadas, que está ubicado en " & Quote & "Herramientas > Administrador de imágenes montadas" & Quote & ", le puede ayudar a encontrar un directorio de montaje válido", vbOKOnly + vbCritical, "Desmontar una imagen")
                        Exit Sub
                End Select
            End If
        End If
        'FolderBrowserDialog1.ShowDialog()
        'If DialogResult.OK Then
        '    TextBox1.Text = FolderBrowserDialog1.SelectedPath
        'Else
        '    TextBox1.Text = ""
        'End If
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
