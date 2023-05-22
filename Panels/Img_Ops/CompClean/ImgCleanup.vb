Imports System.Windows.Forms
Imports Microsoft.Win32
Imports Microsoft.VisualBasic.ControlChars
Imports System.IO

Public Class ImgCleanup

    Dim Tasks() As String = New String(6) {"Revert pending actions", "Clean up Service Pack backup files", "Clean up component store", "Analyze component store", "Check component store", "Scan component store for corruption", "Repair component store"}

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ProgressPanel.CleanupTask = ComboBox1.SelectedIndex
        Select Case ComboBox1.SelectedIndex
            Case 1
                ProgressPanel.CleanupHideSP = CheckBox1.Checked = True
            Case 2
                ProgressPanel.ResetCompBase = CheckBox2.Checked = True
                ProgressPanel.DeferCleanupOps = If(CheckBox2.Checked And CheckBox3.Checked, True, False)
            Case 6
                ProgressPanel.UseCompRepairSource = CheckBox4.Checked = True
                If CheckBox4.Checked And RichTextBox1.Text = "" Then
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    MsgBox("No valid source has been provided for component store repair." & CrLf & CrLf & If(RichTextBox1.Text = "", "Please provide a source and try again.", "Please make sure the specified source exists in the file system and try again."), vbOKOnly + vbCritical, "Image cleanup")
                                Case "ESN"
                                    MsgBox("No se ha proporcionado un origen válido para la reparación del almacén de componentes." & CrLf & CrLf & If(RichTextBox1.Text = "", "Proporcione un origen e inténtelo de nuevo", "Asegúrese de que el origen especificado exista en el sistema de archivos e inténtelo de nuevo."), vbOKOnly + vbCritical, "Limpieza de imagen")
                            End Select
                        Case 1
                            MsgBox("No valid source has been provided for component store repair." & CrLf & CrLf & If(RichTextBox1.Text = "", "Please provide a source and try again.", "Please make sure the specified source exists in the file system and try again."), vbOKOnly + vbCritical, "Image cleanup")
                        Case 2
                            MsgBox("No se ha proporcionado un origen válido para la reparación del almacén de componentes." & CrLf & CrLf & If(RichTextBox1.Text = "", "Proporcione un origen e inténtelo de nuevo", "Asegúrese de que el origen especificado exista en el sistema de archivos e inténtelo de nuevo."), vbOKOnly + vbCritical, "Limpieza de imagen")
                    End Select
                    Exit Sub
                End If
                ProgressPanel.ComponentRepairSource = RichTextBox1.Text
                ProgressPanel.LimitWUAccess = CheckBox5.Checked = True
        End Select
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 32
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgCleanup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Image cleanup"
                        Label1.Text = Text
                        Label2.Text = "Choose a task:"
                        If ComboBox1.SelectedItem = "" Then Label3.Text = "Choose a task to see its description"
                        Label4.Text = "There are no configurable options for this task. However, you should only run this task to try to recover a Windows image that fails to boot."
                        Label5.Text = "The superseded components base reset was last run on:"
                        Label7.Text = "You should only check this option if the base reset takes more than 30 minutes to complete"
                        Label8.Text = "There are no configurable options for this task."
                        Label9.Text = "There are no configurable options for this task."
                        Label10.Text = "There are no configurable options for this task."
                        Label11.Text = "Source:"
                        Label12.Text = "Select a task listed above to configure its options."
                        GroupBox1.Text = "Task options"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        Tasks(0) = "Revert pending actions"
                        Tasks(1) = "Clean up Service Pack backup files"
                        Tasks(2) = "Clean up component store"
                        Tasks(3) = "Analyze component store"
                        Tasks(4) = "Check component store"
                        Tasks(5) = "Scan component store for corruption"
                        Tasks(6) = "Repair component store"
                        ComboBox1.Items.AddRange(Tasks)
                        HealthRestoreSourceOFD.Title = "Specify the source from which we will restore the component store health"
                        Button1.Text = "Browse..."
                        Button2.Text = "Detect from group policy"
                        CheckBox1.Text = "Hide service pack from the Installed Updates list"
                        CheckBox2.Text = "Reset base of superseded components"
                        CheckBox3.Text = "Defer long-running cleanup operations"
                        CheckBox4.Text = "Use different source for component repair"
                        CheckBox5.Text = "Limit access to Windows Update"
                    Case "ESN"
                        Text = "Limpieza de imagen"
                        Label1.Text = Text
                        Label2.Text = "Elija una tarea:"
                        If ComboBox1.SelectedItem = "" Then Label3.Text = "Elija una tarea para ver su descripción"
                        Label4.Text = "No hay opciones configurables para esta tarea. Sin embargo, debería ejecutar esta tarea para intentar recuperar una imagen de Windows que no inicia."
                        Label5.Text = "El restablecimiento de la base de componentes sustituidos fue ejecutado por última vez en:"
                        Label7.Text = "Debería marcar esta opción solo si el restablecimiento de la base tarda más de 30 minutos en completar"
                        Label8.Text = "No hay opciones configurables para esta tarea."
                        Label9.Text = "No hay opciones configurables para esta tarea."
                        Label10.Text = "No hay opciones configurables para esta tarea."
                        Label11.Text = "Origen:"
                        Label12.Text = "Seleccione una tarea en la lista de arriba para configurar sus opciones."
                        GroupBox1.Text = "Opciones de tarea"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        Tasks(0) = "Revertir acciones pendientes"
                        Tasks(1) = "Limpiar archivos de copia de seguridad de Service Pack"
                        Tasks(2) = "Limpiar almacén de componentes"
                        Tasks(3) = "Analizar almacén de componentes"
                        Tasks(4) = "Comprobar almacén de componentes"
                        Tasks(5) = "Escanear almacén de componentes para detectar corrupción"
                        Tasks(6) = "Reparar almacén de componentes"
                        ComboBox1.Items.AddRange(Tasks)
                        HealthRestoreSourceOFD.Title = "Especifique el origen desde donde restauraremos la salud del almacén de componentes"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Detectar políticas de grupo"
                        CheckBox1.Text = "Ocultar Service Pack del listado de Actualizaciones instaladas"
                        CheckBox2.Text = "Restablecer la base de componentes sustituidos"
                        CheckBox3.Text = "Diferir operaciones largas de limpieza"
                        CheckBox4.Text = "Usar origen diferente para la reparación de componentes"
                        CheckBox5.Text = "Limitar acceso a Windows Update"
                End Select
            Case 1
                Text = "Image cleanup"
                Label1.Text = Text
                Label2.Text = "Choose a task:"
                If ComboBox1.SelectedItem = "" Then Label3.Text = "Choose a task to see its description"
                Label4.Text = "There are no configurable options for this task. However, you should only run this task to try to recover a Windows image that fails to boot."
                Label5.Text = "The superseded components base reset was last run on:"
                Label7.Text = "You should only check this option if the base reset takes more than 30 minutes to complete"
                Label8.Text = "There are no configurable options for this task."
                Label9.Text = "There are no configurable options for this task."
                Label10.Text = "There are no configurable options for this task."
                Label11.Text = "Source:"
                Label12.Text = "Select a task listed above to configure its options."
                GroupBox1.Text = "Task options"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                Tasks(0) = "Revert pending actions"
                Tasks(1) = "Clean up Service Pack backup files"
                Tasks(2) = "Clean up component store"
                Tasks(3) = "Analyze component store"
                Tasks(4) = "Check component store"
                Tasks(5) = "Scan component store for corruption"
                Tasks(6) = "Repair component store"
                ComboBox1.Items.AddRange(Tasks)
                HealthRestoreSourceOFD.Title = "Specify the source from which we will restore the component store health"
                Button1.Text = "Browse..."
                Button2.Text = "Detect from group policy"
                CheckBox1.Text = "Hide service pack from the Installed Updates list"
                CheckBox2.Text = "Reset base of superseded components"
                CheckBox3.Text = "Defer long-running cleanup operations"
                CheckBox4.Text = "Use different source for component repair"
                CheckBox5.Text = "Limit access to Windows Update"
            Case 2
                Text = "Limpieza de imagen"
                Label1.Text = Text
                Label2.Text = "Elija una tarea:"
                If ComboBox1.SelectedItem = "" Then Label3.Text = "Elija una tarea para ver su descripción"
                Label4.Text = "No hay opciones configurables para esta tarea. Sin embargo, debería ejecutar esta tarea para intentar recuperar una imagen de Windows que no inicia."
                Label5.Text = "El restablecimiento de la base de componentes sustituidos fue ejecutado por última vez en:"
                Label7.Text = "Debería marcar esta opción solo si el restablecimiento de la base tarda más de 30 minutos en completar"
                Label8.Text = "No hay opciones configurables para esta tarea."
                Label9.Text = "No hay opciones configurables para esta tarea."
                Label10.Text = "No hay opciones configurables para esta tarea."
                Label11.Text = "Origen:"
                Label12.Text = "Seleccione una tarea en la lista de arriba para configurar sus opciones."
                GroupBox1.Text = "Opciones de tarea"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                Tasks(0) = "Revertir acciones pendientes"
                Tasks(1) = "Limpiar archivos de copia de seguridad de Service Pack"
                Tasks(2) = "Limpiar almacén de componentes"
                Tasks(3) = "Analizar almacén de componentes"
                Tasks(4) = "Comprobar almacén de componentes"
                Tasks(5) = "Escanear almacén de componentes para detectar corrupción"
                Tasks(6) = "Reparar almacén de componentes"
                ComboBox1.Items.AddRange(Tasks)
                HealthRestoreSourceOFD.Title = "Especifique el origen desde donde restauraremos la salud del almacén de componentes"
                Button1.Text = "Examinar..."
                Button2.Text = "Detectar políticas de grupo"
                CheckBox1.Text = "Ocultar Service Pack del listado de Actualizaciones instaladas"
                CheckBox2.Text = "Restablecer la base de componentes sustituidos"
                CheckBox3.Text = "Diferir operaciones largas de limpieza"
                CheckBox4.Text = "Usar origen diferente para la reparación de componentes"
                CheckBox5.Text = "Limitar acceso a Windows Update"
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        ComboBox1.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
        RichTextBox1.BackColor = BackColor
        RichTextBox1.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        PictureBox2.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.image_dark, My.Resources.image_light)
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        ' Determine when the last base reset was run
        If MainForm.OnlineManagement Then
            Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing", False)
            Dim LastResetBase_UTC As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Could not get last base reset date. It is possible that no base resets were made").ToString()
                        Case "ESN"
                            LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "No se pudo obtener la fecha de restablecimiento de base. Es posible que no se haya hecho ningún restablecimiento").ToString()
                    End Select
                Case 1
                    LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Could not get last base reset date. It is possible that no base resets were made").ToString()
                Case 2
                    LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "No se pudo obtener la fecha de restablecimiento de base. Es posible que no se haya hecho ningún restablecimiento").ToString()
            End Select
            regKey.Close()
            Dim charArray() As Char = LastResetBase_UTC.ToCharArray()
            If LastResetBase_UTC.Contains("/") Then charArray(10) = " "
            LastResetBase_UTC = New String(charArray)
            Label6.Text = LastResetBase_UTC
        Else
            Using reg As New Process
                reg.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe"
                reg.StartInfo.Arguments = "load HKLM\MountedSoft " & Quote & MainForm.MountDir & "\Windows\system32\config\software" & Quote
                reg.StartInfo.CreateNoWindow = True
                reg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                reg.Start()
                Do Until reg.HasExited
                    If reg.HasExited Then Exit Do
                Loop
                If reg.ExitCode = 0 Then
                    Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("MountedSoft\Microsoft\Windows\CurrentVersion\Component Based Servicing", False)
                    Dim LastResetBase_UTC As String = ""
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Could not get last base reset date. It is possible that no base resets were made").ToString()
                                Case "ESN"
                                    LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "No se pudo obtener la fecha de restablecimiento de base. Es posible que no se haya hecho ningún restablecimiento").ToString()
                            End Select
                        Case 1
                            LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "Could not get last base reset date. It is possible that no base resets were made").ToString()
                        Case 2
                            LastResetBase_UTC = regKey.GetValue("LastResetBase_UTC", "No se pudo obtener la fecha de restablecimiento de base. Es posible que no se haya hecho ningún restablecimiento").ToString()
                    End Select
                    regKey.Close()
                    Dim charArray() As Char = LastResetBase_UTC.ToCharArray()
                    If LastResetBase_UTC.Contains("/") Then charArray(10) = " "
                    LastResetBase_UTC = New String(charArray)
                    Label6.Text = LastResetBase_UTC
                    reg.StartInfo.Arguments = "unload HKLM\MountedSoft"
                    reg.Start()
                    Do Until reg.HasExited
                        If reg.HasExited Then Exit Do
                    Loop
                Else
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    Label6.Text = "Could not get last base reset date"
                                Case "ESN"
                                    Label6.Text = "No se pudo obtener la fecha del último restablecimiento de base"
                            End Select
                        Case 1
                            Label6.Text = "Could not get last base reset date"
                        Case 2
                            Label6.Text = "No se pudo obtener la fecha del último restablecimiento de base"
                    End Select
                End If
            End Using
        End If

        CheckBox5.Enabled = MainForm.OnlineManagement = True
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "" Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            Label3.Text = "Choose a task to see its description"
                        Case "ESN"
                            Label3.Text = "Elija una tarea para ver su descripción"
                    End Select
                Case 1
                    Label3.Text = "Choose a task to see its description"
                Case 2
                    Label3.Text = "Elija una tarea para ver su descripción"
            End Select
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = False
        Else
            Select Case ComboBox1.SelectedIndex
                Case 0
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    Label3.Text = "If you experience a boot failure, this option can try to recover the system by reverting all pending actions from previous servicing operations"
                                Case "ESN"
                                    Label3.Text = "Si experimenta un error en el arranque, esta opción puede intentar recuperar el sistema revirtiendo todas las acciones pendientes de operaciones de servicio previas"
                            End Select
                        Case 1
                            Label3.Text = "If you experience a boot failure, this option can try to recover the system by reverting all pending actions from previous servicing operations"
                        Case 2
                            Label3.Text = "Si experimenta un error en el arranque, esta opción puede intentar recuperar el sistema revirtiendo todas las acciones pendientes de operaciones de servicio previas"
                    End Select
                    Panel2.Visible = True
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 1
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    Label3.Text = "Removes backup files created during the installation of a service pack"
                                Case "ESN"
                                    Label3.Text = "Elimina archivos de copia de seguridad creados durante la instalación de un Service Pack"
                            End Select
                        Case 1
                            Label3.Text = "Removes backup files created during the installation of a service pack"
                        Case 2
                            Label3.Text = "Elimina archivos de copia de seguridad creados durante la instalación de un Service Pack"
                    End Select
                    Panel2.Visible = False
                    Panel3.Visible = True
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 2
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    Label3.Text = "Cleans up the superseded components and reduces the size of the component store"
                                Case "ESN"
                                    Label3.Text = "Limpia los componentes sustituidos y reduce el tamaño del almacén de componentes"
                            End Select
                        Case 1
                            Label3.Text = "Cleans up the superseded components and reduces the size of the component store"
                        Case 2
                            Label3.Text = "Limpia los componentes sustituidos y reduce el tamaño del almacén de componentes"
                    End Select
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = True
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 3
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    Label3.Text = "Creates a report of the component store, including its size"
                                Case "ESN"
                                    Label3.Text = "Crea un informe del almacén de componentes, incluyendo su tamaño"
                            End Select
                        Case 1
                            Label3.Text = "Creates a report of the component store, including its size"
                        Case 2
                            Label3.Text = "Crea un informe del almacén de componentes, incluyendo su tamaño"
                    End Select
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = True
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 4
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    Label3.Text = "Checks whether the image has been flagged as corrupted by a failed process and whether the corruption can be repaired"
                                Case "ESN"
                                    Label3.Text = "Comprueba si la imagen ha sido reportada como corrupta por un proceso fallido y si la corrupción puede ser reparada"
                            End Select
                        Case 1
                            Label3.Text = "Checks whether the image has been flagged as corrupted by a failed process and whether the corruption can be repaired"
                        Case 2
                            Label3.Text = "Comprueba si la imagen ha sido reportada como corrupta por un proceso fallido y si la corrupción puede ser reparada"
                    End Select
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = True
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 5
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    Label3.Text = "Scans the image for component store corruption, but does not perform repair options automatically"
                                Case "ESN"
                                    Label3.Text = "Escanea la imagen para comprobar corrupción en el almacén de componentes, pero no realiza operaciones de reparación automáticamente"
                            End Select
                        Case 1
                            Label3.Text = "Scans the image for component store corruption, but does not perform repair options automatically"
                        Case 2
                            Label3.Text = "Escanea la imagen para comprobar corrupción en el almacén de componentes, pero no realiza operaciones de reparación automáticamente"
                    End Select
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = True
                    Panel8.Visible = False
                Case 6
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    Label3.Text = "Scans the image for component store corruption and performs repair operations automatically"
                                Case "ESN"
                                    Label3.Text = "Escanea la imagen para comprobar corrupción en el almacén de componentes y realiza operaciones de reparación automáticamente"
                            End Select
                        Case 1
                            Label3.Text = "Scans the image for component store corruption and performs repair operations automatically"
                        Case 2
                            Label3.Text = "Escanea la imagen para comprobar corrupción en el almacén de componentes y realiza operaciones de reparación automáticamente"
                    End Select
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = True
            End Select
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        Label11.Enabled = CheckBox4.Checked = True
        RichTextBox1.Enabled = CheckBox4.Checked = True
        Button1.Enabled = CheckBox4.Checked = True
        Button2.Enabled = CheckBox4.Checked = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        HealthRestoreSourceOFD.ShowDialog()
    End Sub

    Private Sub HealthRestoreSourceOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles HealthRestoreSourceOFD.FileOk
        RichTextBox1.Text = HealthRestoreSourceOFD.FileName
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        RichTextBox1.Text = MainForm.GetSrcFromGPO()
        If RichTextBox1.Text.StartsWith("wim:\", StringComparison.OrdinalIgnoreCase) Then
            TextBoxSourcePanel.Visible = False
            WimFileSourcePanel.Visible = True
            Dim parts() As String = RichTextBox1.Text.Split(":")
            Label14.Text = parts(parts.Length - 1)
            Label13.Text = parts(1).Replace("\", "").Trim() & ":" & parts(2)
            If Label13.Text.EndsWith(":" & parts(parts.Length - 1)) Then Label13.Text = Label13.Text.Replace(":" & parts(parts.Length - 1), "").Trim()
        Else
            TextBoxSourcePanel.Visible = True
            WimFileSourcePanel.Visible = False
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        TextBoxSourcePanel.Visible = True
        WimFileSourcePanel.Visible = False
    End Sub
End Class
