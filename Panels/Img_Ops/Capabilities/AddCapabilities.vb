Imports System.Windows.Forms
Imports Microsoft.Win32
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class AddCapabilities

    Dim capCount As Integer
    Dim capIds(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim capIdList As New List(Of String)
        ProgressPanel.MountDir = MainForm.MountDir
        capCount = ListView1.CheckedItems.Count
        If ListView1.CheckedItems.Count >= 1 Then
            For x = 0 To capCount - 1
                capIdList.Add(ListView1.CheckedItems(x).SubItems(0).Text)
            Next
            capIds = capIdList.ToArray()
            For x = 0 To capIds.Length - 1
                ProgressPanel.capAdditionIds(x) = capIds(x)
            Next
            For x = 0 To capCount - 1
                If ListView1.CheckedItems(x).SubItems(1).Text = "Not present" Then
                    If CheckBox1.Checked And RichTextBox1.Text = "" Or Not Directory.Exists(RichTextBox1.Text) Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        If MsgBox("Some capabilities in this image require specifying a source for them to be enabled. The specified source is not valid for this operation." & CrLf & CrLf & If(RichTextBox1.Text = "", "Please specify a valid source and try again.", "Please make sure the source exists in the file system and try again."), vbOKOnly + vbCritical, "Enable features") = MsgBoxResult.Ok Then
                                            CheckBox1.Checked = True
                                            Button1.PerformClick()
                                        End If
                                    Case "ESN"
                                        If MsgBox("Algunas funcionalidades en esta imagen requieren especificar un origen para ser habilitadas. El origen especificado no es válido para esta operación" & CrLf & CrLf & If(RichTextBox1.Text = "", "Especifique un origen válido e inténtelo de nuevo.", "Asegúrese de que el origen exista en el sistema de archivos e inténtelo de nuevo."), vbOKOnly + vbCritical, "Habilitar características") = MsgBoxResult.Ok Then
                                            CheckBox1.Checked = True
                                            Button1.PerformClick()
                                        End If
                                End Select
                            Case 1
                                If MsgBox("Some capabilities in this image require specifying a source for them to be enabled. The specified source is not valid for this operation." & CrLf & CrLf & If(RichTextBox1.Text = "", "Please specify a valid source and try again.", "Please make sure the source exists in the file system and try again."), vbOKOnly + vbCritical, "Enable features") = MsgBoxResult.Ok Then
                                    CheckBox1.Checked = True
                                    Button1.PerformClick()
                                End If
                            Case 2
                                If MsgBox("Algunas funcionalidades en esta imagen requieren especificar un origen para ser habilitadas. El origen especificado no es válido para esta operación" & CrLf & CrLf & If(RichTextBox1.Text = "", "Especifique un origen válido e inténtelo de nuevo.", "Asegúrese de que el origen exista en el sistema de archivos e inténtelo de nuevo."), vbOKOnly + vbCritical, "Habilitar características") = MsgBoxResult.Ok Then
                                    CheckBox1.Checked = True
                                    Button1.PerformClick()
                                End If
                        End Select
                    End If
                End If
            Next
            ProgressPanel.capAdditionLastId = ListView1.CheckedItems(capCount - 1).SubItems(0).Text
            If CheckBox1.Checked Then
                If RichTextBox1.Text <> "" Then
                    If Directory.Exists(RichTextBox1.Text) Then
                        ProgressPanel.capAdditionUseSource = True
                        ProgressPanel.capAdditionSource = RichTextBox1.Text         ' Don't know if it would work on cases where it begins with "wim:\"
                    Else
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        MsgBox("The specified source does not exist in the file system. Make sure it exists and try again.", vbOKOnly + vbCritical, Label1.Text)
                                    Case "ESN"
                                        MsgBox("El origen especificado no existe en el sistema de archivos. Asegúrese de que existe e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                                End Select
                            Case 1
                                MsgBox("The specified source does not exist in the file system. Make sure it exists and try again.", vbOKOnly + vbCritical, Label1.Text)
                            Case 2
                                MsgBox("El origen especificado no existe en el sistema de archivos. Asegúrese de que existe e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                        End Select
                        Exit Sub
                    End If
                Else
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    MsgBox("There is no source specified. Specify a source and try again.", vbOKOnly + vbCritical, Label1.Text)
                                Case "ESN"
                                    MsgBox("No se ha especificado un origen. Especifique un origen e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                            End Select
                        Case 1
                            MsgBox("There is no source specified. Specify a source and try again.", vbOKOnly + vbCritical, Label1.Text)
                        Case 2
                            MsgBox("No se ha especificado un origen. Especifique un origen e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                    End Select
                    Exit Sub
                End If
            End If
            If CheckBox2.Checked Then
                ProgressPanel.capAdditionLimitWUAccess = True
            Else
                ProgressPanel.capAdditionLimitWUAccess = False
            End If
            If CheckBox3.Checked Then
                ProgressPanel.capAdditionCommit = True
            Else
                ProgressPanel.capAdditionCommit = False
            End If
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            MsgBox("There aren't any selected capabilities to install. Please select some capabilities and try again.", vbOKOnly + vbCritical, Label1.Text)
                        Case "ESN"
                            MsgBox("No hay funcionalidades seleccionadas para instalar. Seleccione algunas de ellas e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                    End Select
                Case 1
                    MsgBox("There aren't any selected capabilities to install. Please select some capabilities and try again.", vbOKOnly + vbCritical, Label1.Text)
                Case 2
                    MsgBox("No hay funcionalidades seleccionadas para instalar. Seleccione algunas de ellas e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
            End Select
            Exit Sub
        End If
        ProgressPanel.capAdditionCount = capCount
        ProgressPanel.OperationNum = 64
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub AddCapability_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Add capabilities"
                        Label1.Text = Text
                        Label2.Text = "Source:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        Button1.Text = "Browse..."
                        Button2.Text = "Select all"
                        Button3.Text = "Select none"
                        Button4.Text = "Determine from group policy"
                        GroupBox1.Text = "Capabilities"
                        GroupBox2.Text = "Options"
                        CheckBox1.Text = "Specify different source for capability installs"
                        CheckBox2.Text = "Limit access to Windows Update"
                        CheckBox3.Text = "Commit image after adding capabilities"
                        ListView1.Columns(0).Text = "Capability"
                        ListView1.Columns(1).Text = "State"
                    Case "ESN"
                        Text = "Añadir funcionalidades"
                        Label1.Text = Text
                        Label2.Text = "Origen:"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Seleccionar todas"
                        Button3.Text = "Seleccionar ninguna"
                        Button4.Text = "Detectar políticas de grupo"
                        GroupBox1.Text = "Funcionalidades"
                        GroupBox2.Text = "Opciones"
                        CheckBox1.Text = "Especificar un origen diferente para la instalación de funcionalidades"
                        CheckBox2.Text = "Limitar acceso a Windows Update"
                        CheckBox3.Text = "Guardar imagen tras añadir funcionalidades"
                        ListView1.Columns(0).Text = "Funcionalidad"
                        ListView1.Columns(1).Text = "Estado"
                End Select
            Case 1
                Text = "Add capabilities"
                Label1.Text = Text
                Label2.Text = "Source:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                Button1.Text = "Browse..."
                Button2.Text = "Select all"
                Button3.Text = "Select none"
                Button4.Text = "Determine from group policy"
                GroupBox1.Text = "Capabilities"
                GroupBox2.Text = "Options"
                CheckBox1.Text = "Specify different source for capability installs"
                CheckBox2.Text = "Limit access to Windows Update"
                CheckBox3.Text = "Commit image after adding capabilities"
                ListView1.Columns(0).Text = "Capability"
                ListView1.Columns(1).Text = "State"
            Case 2
                Text = "Añadir funcionalidades"
                Label1.Text = Text
                Label2.Text = "Origen:"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                Button1.Text = "Examinar..."
                Button2.Text = "Seleccionar todas"
                Button3.Text = "Seleccionar ninguna"
                Button4.Text = "Detectar políticas de grupo"
                GroupBox1.Text = "Funcionalidades"
                GroupBox2.Text = "Opciones"
                CheckBox1.Text = "Especificar un origen diferente para la instalación de funcionalidades"
                CheckBox2.Text = "Limitar acceso a Windows Update"
                CheckBox3.Text = "Guardar imagen tras añadir funcionalidades"
                ListView1.Columns(0).Text = "Funcionalidad"
                ListView1.Columns(1).Text = "Estado"
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Label4.Text &= " Only not installed capabilities (" & ListView1.Items.Count & ") are shown"
                    Case "ESN"
                        Label4.Text &= " Solo las funcionalidades no instaladas (" & ListView1.Items.Count & ") son mostradas"
                End Select
            Case 1
                Label4.Text &= " Only not installed capabilities (" & ListView1.Items.Count & ") are shown"
            Case 2
                Label4.Text &= " Solo las funcionalidades no instaladas (" & ListView1.Items.Count & ") son mostradas"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
            RichTextBox1.BackColor = Color.FromArgb(31, 31, 31)
            PictureBox2.Image = My.Resources.image_dark
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
            RichTextBox1.BackColor = Color.FromArgb(238, 238, 242)
            PictureBox2.Image = My.Resources.image_light
        End If
        CheckBox1.ForeColor = ForeColor
        CheckBox2.ForeColor = ForeColor
        CheckBox3.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        RichTextBox1.ForeColor = ForeColor
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Label2.Enabled = True
            RichTextBox1.Enabled = True
            Button1.Enabled = True
            Button4.Enabled = True
        Else
            Label2.Enabled = False
            RichTextBox1.Enabled = False
            Button1.Enabled = False
            Button4.Enabled = False
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        RichTextBox1.Text = MainForm.GetSrcFromGPO()
        If RichTextBox1.Text.StartsWith("wim:\", StringComparison.OrdinalIgnoreCase) Then
            TextBoxSourcePanel.Visible = False
            WimFileSourcePanel.Visible = True
            Dim parts() As String = RichTextBox1.Text.Split(":")
            Label3.Text = parts(parts.Length - 1)
            Label5.Text = parts(1).Replace("\", "").Trim() & ":" & parts(2)
            If Label5.Text.EndsWith(":" & parts(parts.Length - 1)) Then Label5.Text = Label5.Text.Replace(":" & parts(parts.Length - 1), "").Trim()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For i As Integer = 0 To ListView1.Items.Count - 1
            ListView1.Items(i).Checked = True
        Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        For i As Integer = 0 To ListView1.Items.Count - 1
            ListView1.Items(i).Checked = False
        Next
        DialogResult = Windows.Forms.DialogResult.None
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            RichTextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub
End Class
