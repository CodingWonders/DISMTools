Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Globalization
Imports Microsoft.Win32

Public Class Options

    Dim DismVersion As FileVersionInfo
    Dim CanExit As Boolean
    Dim SaveLocations() As String = New String(1) {"Settings file", "Registry"}
    Dim ColorModes() As String = New String(2) {"Use system setting", "Light mode", "Dark mode"}
    Dim Languages() As String = New String(2) {"Use system language", "English", "Spanish"}
    Dim LogViews() As String = New String(1) {"list", "table"}
    Dim NotFreqs() As String = New String(1) {"Every time a project has been loaded successfully", "Once"}

    Sub DetermineSettingValidity()
        If TextBox1.Text = "" Then
            CanExit = False
            GiveErrorExplanation(1)
        Else
            If File.Exists(TextBox1.Text) Then
                CanExit = True
            Else
                CanExit = False
                GiveErrorExplanation(2)
            End If
        End If
        If TextBox2.Text = "" Then
            CanExit = False
            GiveErrorExplanation(3)
        Else
            If File.Exists(TextBox2.Text) Then
                CanExit = True
            Else
                Try
                    File.Create(TextBox2.Text)
                    CanExit = True
                Catch ex As Exception
                    CanExit = False
                    GiveErrorExplanation(4)
                End Try
            End If
        End If
        If CheckBox4.Checked Then
            If RadioButton3.Checked Then
                CanExit = True
                Exit Sub
            End If
            If TextBox3.Text = "" Then
                CanExit = False
                GiveErrorExplanation(5)
            Else
                If Directory.Exists(TextBox3.Text) Then
                    CanExit = True
                Else
                    Try
                        Directory.CreateDirectory(TextBox3.Text)
                        CanExit = True
                    Catch ex As Exception
                        CanExit = False
                        GiveErrorExplanation(5)
                    End Try
                End If
            End If
        End If
        If Not CanExit Then
            Exit Sub
        End If
    End Sub

    Sub ApplyProgSettings()
        DetermineSettingValidity()
        If MainForm.Language = 1 And ComboBox3.SelectedIndex <> 1 Then
            MsgBox("Support for languages is partial, so this program isn't fully translated yet. Please wait until the next version to experience full language support." & CrLf & CrLf & "El soporte para idiomas es parcial, así que este programa aún no está traducido completamente. Espere hasta la próxima versión para experimentar soporte completo de idiomas.", vbOKOnly + vbInformation, "Options/Opciones")
        End If
        MainForm.DismExe = TextBox1.Text
        Select Case ComboBox1.SelectedIndex
            Case 0
                MainForm.SaveOnSettingsIni = True
            Case 1
                MainForm.SaveOnSettingsIni = False
        End Select
        If CheckBox1.Checked Then
            MainForm.VolatileMode = True
        Else
            MainForm.VolatileMode = False
        End If
        MainForm.ColorMode = ComboBox2.SelectedIndex
        MainForm.Language = ComboBox3.SelectedIndex
        MainForm.LogFont = ComboBox4.Text
        MainForm.LogFontSize = NumericUpDown1.Value
        If Toggle1.Checked Then
            MainForm.LogFontIsBold = True
        Else
            MainForm.LogFontIsBold = False
        End If
        If RadioButton5.Checked Then
            MainForm.ProgressPanelStyle = 1
        Else
            MainForm.ProgressPanelStyle = 0
        End If
        MainForm.LogFile = TextBox2.Text
        MainForm.LogLevel = TrackBar1.Value + 1
        If RadioButton1.Checked Then
            MainForm.ImgOperationMode = 0
        Else
            MainForm.ImgOperationMode = 1
        End If
        If CheckBox2.Checked Then
            MainForm.QuietOperations = True
        Else
            MainForm.QuietOperations = False
        End If
        If CheckBox3.Checked Then
            MainForm.SysNoRestart = True
        Else
            MainForm.SysNoRestart = False
        End If
        If CheckBox4.Checked Then
            MainForm.UseScratch = True
        Else
            MainForm.UseScratch = False
        End If
        If RadioButton3.Checked Then
            MainForm.AutoScrDir = True
        Else
            MainForm.AutoScrDir = False
        End If
        MainForm.ScratchDir = TextBox3.Text
        If CheckBox5.Checked Then
            MainForm.EnglishOutput = True
        Else
            MainForm.EnglishOutput = False
        End If
        Dim ti As TextInfo = New CultureInfo("en-US", False).TextInfo
        MainForm.AllCaps = CheckBox9.Checked
        If CheckBox9.Checked Then
            MainForm.FileToolStripMenuItem.Text = MainForm.FileToolStripMenuItem.Text.ToUpper()
            MainForm.ProjectToolStripMenuItem.Text = MainForm.ProjectToolStripMenuItem.Text.ToUpper()
            MainForm.CommandsToolStripMenuItem.Text = MainForm.CommandsToolStripMenuItem.Text.ToUpper()
            MainForm.ToolsToolStripMenuItem.Text = MainForm.ToolsToolStripMenuItem.Text.ToUpper()
            MainForm.HelpToolStripMenuItem.Text = MainForm.HelpToolStripMenuItem.Text.ToUpper()
        Else
            MainForm.FileToolStripMenuItem.Text = ti.ToTitleCase(MainForm.FileToolStripMenuItem.Text.ToLower())
            MainForm.ProjectToolStripMenuItem.Text = ti.ToTitleCase(MainForm.ProjectToolStripMenuItem.Text.ToLower())
            MainForm.CommandsToolStripMenuItem.Text = ti.ToTitleCase(MainForm.CommandsToolStripMenuItem.Text.ToLower())
            MainForm.ToolsToolStripMenuItem.Text = ti.ToTitleCase(MainForm.ToolsToolStripMenuItem.Text.ToLower())
            MainForm.HelpToolStripMenuItem.Text = ti.ToTitleCase(MainForm.HelpToolStripMenuItem.Text.ToLower())
        End If
        MainForm.ReportView = ComboBox5.SelectedIndex
        MainForm.ChangePrgColors(MainForm.ColorMode)
        MainForm.ChangeLangs(MainForm.Language)
        If MountedImgMgr.Visible Then
            MountedImgMgr.Close()
            MountedImgMgr.Show()
        End If
        If BGProcDetails.Visible Then
            BGProcDetails.Visible = False
            BGProcDetails.Visible = True
        End If
        If MainForm.isProjectLoaded Then
            MainForm.UnpopulateProjectTree()
            MainForm.PopulateProjectTree(MainForm.prjName)
        End If
        If CheckBox6.Checked Then
            MainForm.NotificationShow = True
        Else
            MainForm.NotificationShow = False
        End If
        MainForm.NotificationFrequency = ComboBox6.SelectedIndex
        MainForm.StartupRemount = CheckBox12.Checked
        MainForm.StartupUpdateCheck = CheckBox13.Checked
        MainForm.AutoLogs = CheckBox10.Checked
        If MainForm.VolatileMode Then
            MainForm.SaveDTSettings()
        End If
        If MainForm.IsImageMounted Then MainForm.DetectVersions(FileVersionInfo.GetVersionInfo(MainForm.DismExe), MainForm.imgVersionInfo)
    End Sub

    Sub GiveErrorExplanation(ErrorCode As Integer)
        Select Case ErrorCode
            Case 1
                MsgBox("The DISM executable path was not specified. Please specify one and try again", MsgBoxStyle.Critical, "DISMTools")
            Case 2
                MsgBox("The DISM executable does not exist in the file system. Please verify the file still exists and try again", MsgBoxStyle.Critical, "DISMTools")
            Case 3
                MsgBox("The log file was not specified. Please specify one and try again", MsgBoxStyle.Critical, "DISMTools")
            Case 4
                MsgBox("The program tried to create the specified log file, but has failed. Please try again", MsgBoxStyle.Critical, "DISMTools")
        End Select
    End Sub

    Function DetectFileAssociations() As Boolean
        Try
            Dim AssocRk As RegistryKey = Registry.ClassesRoot.OpenSubKey("DISMTools.Project\Shell\Open\Command", False)
            Dim AssocCmd As String = AssocRk.GetValue(Nothing).ToString()
            AssocRk.Close()
            If File.Exists(AssocCmd.Replace(" " & Quote & "/load=" & Quote & "%1" & Quote & Quote, "").Trim().Replace(Quote, "").Trim()) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Manages the file associations for files with the ".dtproj" extension
    ''' </summary>
    ''' <param name="AssocOp"></param>
    ''' <param name="UseCustomIcons"></param>
    ''' <remarks></remarks>
    Sub ManageAssociations(AssocOp As Integer, UseCustomIcons As Boolean)
        Select Case AssocOp
            Case 0
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe", "/c assoc .dtproj=DISMTools.Project").WaitForExit()
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe", "/c ftype DISMTools.Project=" & Quote & Environment.CurrentDirectory & "\DISMTools.exe" & Quote & " " & Quote & "/load=" & Quote & "%1" & Quote & Quote).WaitForExit()
                Dim AssocRk As RegistryKey = Registry.ClassesRoot.OpenSubKey("DISMTools.Project", True)
                AssocRk.SetValue(Nothing, "DISMTools project", RegistryValueKind.String)
                If UseCustomIcons Then
                    If File.Exists(Environment.CurrentDirectory & "\resources\dtproj.ico") Then
                        AssocRk.CreateSubKey("DefaultIcon")
                        Dim DefIcon As RegistryKey = Registry.ClassesRoot.OpenSubKey("DISMTools.Project\DefaultIcon", True)
                        DefIcon.SetValue(Nothing, Environment.CurrentDirectory & "\resources\dtproj.ico", RegistryValueKind.String)
                        DefIcon.Close()
                    End If
                End If
                AssocRk.Close()
            Case 1
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe", "/c assoc .dtproj=").WaitForExit()
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe", "/c ftype DISMTools.Project=").WaitForExit()
                ' Delete registry key remnants
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe", "delete HKCR\DISMTools.Project /f").WaitForExit()
        End Select
        ' Clear icon cache
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\ie4uinit.exe", "-ClearIconCache").WaitForExit()
        ' Restart explorer.exe
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\taskkill.exe", "/f /im explorer.exe").WaitForExit()
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe")
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Label42.Text = If(DetectFileAssociations(), "associations set", "associations not set")
                        Button9.Text = If(DetectFileAssociations(), "Remove file associations", "Set file associations")
                    Case "ESN"
                        Label42.Text = If(DetectFileAssociations(), "asociaciones establecidas", "asociaciones no establecidas")
                        Button9.Text = If(DetectFileAssociations(), "Eliminar asociaciones", "Establecer asociaciones")
                End Select
            Case 1
                Label42.Text = If(DetectFileAssociations(), "associations set", "associations not set")
                Button9.Text = If(DetectFileAssociations(), "Remove file associations", "Set file associations")
            Case 2
                Label42.Text = If(DetectFileAssociations(), "asociaciones establecidas", "asociaciones no establecidas")
                Button9.Text = If(DetectFileAssociations(), "Eliminar asociaciones", "Establecer asociaciones")
        End Select
        CheckBox11.Enabled = If(DetectFileAssociations(), False, True)
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ApplyProgSettings()
        If CanExit Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Options_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()
        ComboBox3.Items.Clear()
        ComboBox5.Items.Clear()
        ComboBox6.Items.Clear()
        ComboBox1.SelectedText = ""
        ComboBox2.SelectedText = ""
        ComboBox3.SelectedText = ""
        ComboBox5.SelectedText = ""
        ComboBox6.SelectedText = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Options"
                        Label1.Text = Text
                        TabPage1.Text = "Program"
                        TabPage2.Text = "Personalization"
                        TabPage3.Text = "Logs"
                        TabPage4.Text = "Image operations"
                        TabPage5.Text = "Scratch directory"
                        TabPage6.Text = "Program output"
                        TabPage7.Text = "Background processes"
                        TabPage8.Text = "Modules"
                        TabPage9.Text = "Image detection"
                        TabPage10.Text = "File associations"
                        TabPage11.Text = "Startup"
                        Label2.Text = "DISM executable path:"
                        Label3.Text = "Version:"
                        Label5.Text = "Save settings on:"
                        Label6.Text = "While in volatile mode, settings will be reset on program closure."
                        Label7.Text = "Color mode:"
                        Label8.Text = "Language:"
                        Label9.Text = "Please specify the settings for the log window:"
                        Label10.Text = "Log window font:"
                        Label11.Text = "Preview:"
                        Label12.Text = "Operation log file:"
                        Label13.Text = "When performing image operations in the command line, specify the " & Quote & "/LogPath" & Quote & " argument to save the image operation log to the target log file."
                        Label14.Text = "Log file level:"
                        Label17.Text = "Perform image operations on:"
                        Label18.Text = "When quietly performing operation, the program will hide information and progress output. Error messages will still be shown." & CrLf & "This option will not be used when getting information of, for example, packages or features." & CrLf & "Also, when performing image servicing, your computer may restart automatically."
                        Label19.Text = "When this option is checked, your computer will not restart automatically; even when quietly performing operations"
                        Label20.Text = "Please specify the scratch directory to be used for DISM operations:"
                        Label21.Text = "Scratch directory:"
                        Label22.Text = "Space left on selected scratch directory:"
                        Label25.Text = "Log view:"
                        Label26.Text = "Example report:"
                        Label27.Text = "Some reports do not allow being shown as a table."
                        Label28.Text = "When should the program notify you about background processes being started?"
                        Label29.Text = "The program uses background processes to gather complete image information, like modification dates, installed packages, features present; and more"
                        Label30.Text = "Manage program modules:"
                        Label31.Text = "Status:"
                        Label32.Text = "not installed"
                        Label33.Text = "Version:"
                        Label34.Text = "could not get module version"
                        Label35.Text = "Modify these settings only if you experience constant program or system slowdowns due to high CPU usage"
                        Label36.Text = "Review the status of this background process:"
                        Label37.Text = "Status:"
                        Label40.Text = "File associations let you access project files directly, without having to load the program first"
                        Label41.Text = "Association status:"
                        Label42.Text = If(DetectFileAssociations(), "associations set", "associations not set")
                        Label43.Text = "Set options you would like to perform when the program starts up:"
                        Label44.Text = "The program will use the scratch directory provided by the project if one is loaded. If you are in online installation management mode, the program will use its scratch directory"
                        Label45.Text = "Secondary progress panel style:"
                        Label46.Text = "These settings aren't applicable to non-portable installations"
                        Button1.Text = "Browse..."
                        Button2.Text = "View DISM component versions"
                        Button3.Text = "Browse..."
                        Button4.Text = "Browse..."
                        Button5.Text = "Install"
                        Button6.Text = "Check for updates"
                        Button7.Text = "Remove"
                        Button9.Text = If(DetectFileAssociations(), "Remove file associations", "Set file associations")
                        Button10.Text = "Advanced settings"
                        If MainForm.MountedImageDetectorBW.IsBusy Then Button8.Text = "Stop" Else Button8.Text = "Start"
                        Cancel_Button.Text = "Cancel"
                        OK_Button.Text = "OK"
                        PrefReset.Text = "Reset preferences"
                        CheckBox1.Text = "Volatile mode"
                        CheckBox2.Text = "Quietly perform image operations"
                        CheckBox3.Text = "Skip system restart"
                        CheckBox4.Text = "Use a scratch directory"
                        CheckBox5.Text = "Show command output in English"
                        CheckBox6.Text = "Notify me when background processes have started"
                        CheckBox7.Text = "Use module when needed"
                        CheckBox8.Text = "Detect mounted images at all times"
                        CheckBox9.Text = "Use uppercase menus"
                        CheckBox10.Text = "Automatically create logs for each operation performed"
                        CheckBox11.Text = "Set custom file icons for DISMTools projects"
                        CheckBox12.Text = "Remount mounted images in need of a servicing session reload"
                        CheckBox13.Text = "Check for updates"
                        DismOFD.Title = "Specify the DISM executable to use"
                        GroupBox1.Text = "Log customization"
                        GroupBox2.Text = "Notification frequency"
                        GroupBox3.Text = "Module details"
                        GroupBox4.Text = "Background process"
                        GroupBox5.Text = "Associations"
                        LinkLabel1.Text = "The program will enable or disable certain features according to what the DISM version supports. How is it going to affect my usage of this program, and which features will be disabled accordingly?"
                        LinkLabel1.LinkArea = New LinkArea(97, 100)
                        LinkLabel2.Text = "Learn more about background processes"
                        LogSFD.Title = "Specify the location of the log file"
                        RadioButton1.Text = "Mounted Windows image"
                        RadioButton2.Text = "Active installation"
                        RadioButton3.Text = "Use the project or program scratch directory"
                        RadioButton4.Text = "Use the specified scratch directory"
                        RadioButton5.Text = "Modern"
                        RadioButton6.Text = "Classic"
                        ScratchFBD.Description = "Specify the scratch directory the program should use:"
                    Case "ESN"
                        Text = "Opciones"
                        Label1.Text = Text
                        TabPage1.Text = "Programa"
                        TabPage2.Text = "Personalización"
                        TabPage3.Text = "Registros"
                        TabPage4.Text = "Operaciones"
                        TabPage5.Text = "Directorio temporal"
                        TabPage6.Text = "Salida del programa"
                        TabPage7.Text = "Procesos en segundo plano"
                        TabPage8.Text = "Módulos"
                        TabPage9.Text = "Detección de imágenes"
                        TabPage10.Text = "Asociaciones de archivos"
                        TabPage11.Text = "Inicio"
                        Label2.Text = "Ruta del ejecutable:"
                        Label3.Text = "Versión:"
                        Label5.Text = "Guardar configuraciones en:"
                        Label6.Text = "Cuando se está en el modo volátil, las configuraciones se restablecerán al cerrar el programa."
                        Label7.Text = "Modo de color:"
                        Label8.Text = "Idioma:"
                        Label9.Text = "Especifique las configuraciones para la ventana de registro:"
                        Label10.Text = "Fuente:"
                        Label11.Text = "Vista previa:"
                        Label12.Text = "Archivo de registro:"
                        Label13.Text = "Cuando se realizan operaciones en la línea de comandos, especifique el argumento " & Quote & "/LogPath" & Quote & " para guardar el registro de operaciones en el archivo de destino"
                        Label14.Text = "Nivel de registro:"
                        Label17.Text = "Realizar operaciones en:"
                        Label18.Text = "Cuando se realizan operaciones silenciosamente, el programa ocultará información y salida del progreso." & CrLf & "Esta opción no se usará al obtener información de, por ejemplo, paquetes o características." & CrLf & "También, al realizar un servicio de imágenes, su sistema podría reiniciarse automáticamente."
                        Label19.Text = "Cuando esta opción está marcada, su sistema no se reiniciará automáticamente; incluso si se realizan operaciones silenciosamente"
                        Label20.Text = "Especifique el directorio temporal a ser usado en operaciones de DISM:"
                        Label21.Text = "Directorio temporal:"
                        Label22.Text = "Espacio disponible en directorio temporal:"
                        Label25.Text = "Vista de registro:"
                        Label26.Text = "Informe de prueba:"
                        Label27.Text = "Algunos informes no permiten ser mostrados como una tabla."
                        Label28.Text = "¿Cuándo debería el programa notificarle acerca de procesos en segundo plano siendo iniciados?"
                        Label29.Text = "El programa utiliza procesos en segundo plano para recopilar información completa de la imagen, como fechas de modificación, paquetes instalados, características presentes; y más"
                        Label30.Text = "Administrar módulos del programa:"
                        Label31.Text = "Estado:"
                        Label32.Text = "no instalado"
                        Label33.Text = "Versión:"
                        Label34.Text = "no se pudo obtener información"
                        Label35.Text = "Modifique estas configuraciones solo si experimenta ralentizaciones constantes del programa o del sistema debido a un uso elevado de CPU"
                        Label36.Text = "Consulte el estado de este proceso en segundo plano:"
                        Label37.Text = "Estado:"
                        Label40.Text = "Las asociaciones le permiten acceder a archivos de proyectos directamente, sin tener que cargar el programa en primer lugar"
                        Label41.Text = "Estado de asociaciones:"
                        Label42.Text = If(DetectFileAssociations(), "asociaciones establecidas", "asociaciones no establecidas")
                        Label43.Text = "Establezca las opciones que le gustaría realizar cuando el programa inicie:"
                        Label44.Text = "El programa usará el directorio temporal proporcionado por el proyecto si se cargó alguno. Si está en modo de administración de instalaciones en línea, el programa utilizará su directorio temporal"
                        Label45.Text = "Estilo del panel de progreso secundario:"
                        Label46.Text = "Estas configuraciones no son aplicables a instalaciones no portátiles"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Ver versiones de componentes"
                        Button3.Text = "Examinar..."
                        Button4.Text = "Examinar..."
                        Button5.Text = "Instalar"
                        Button6.Text = "Comprobar actualizaciones"
                        Button7.Text = "Eliminar"
                        Button9.Text = If(DetectFileAssociations(), "Eliminar asociaciones", "Establecer asociaciones")
                        Button10.Text = "Opciones avanzadas"
                        If MainForm.MountedImageDetectorBW.IsBusy Then Button8.Text = "Detener" Else Button8.Text = "Iniciar"
                        Cancel_Button.Text = "Cancelar"
                        OK_Button.Text = "Aceptar"
                        PrefReset.Text = "Restablecer preferencias"
                        CheckBox1.Text = "Modo volátil"
                        CheckBox2.Text = "Realizar operaciones silenciosamente"
                        CheckBox3.Text = "Omitir reinicio del sistema"
                        CheckBox4.Text = "Usar un directorio temporal"
                        CheckBox5.Text = "Mostrar salida del programa en inglés"
                        CheckBox6.Text = "Notificarme cuando los procesos en segundo plano se hayan iniciado"
                        CheckBox7.Text = "Usar módulo cuando sea necesario"
                        CheckBox8.Text = "Detectar imágenes montadas todo el tiempo"
                        CheckBox9.Text = "Usar menús en mayúscula"
                        CheckBox10.Text = "Crear registros para cada operación realizada automáticamente"
                        CheckBox11.Text = "Establecer iconos personalizados para proyectos de DISMTools"
                        CheckBox12.Text = "Remontar imágenes montadas que necesitan una recarga de su sesión de servicio"
                        CheckBox13.Text = "Comprobar actualizaciones"
                        DismOFD.Title = "Especifique el ejecutable de DISM a usar"
                        GroupBox1.Text = "Personalización del registro"
                        GroupBox2.Text = "Frecuencia de notificaciones"
                        GroupBox3.Text = "Detalles de módulo"
                        GroupBox4.Text = "Proceso en segundo plano"
                        GroupBox5.Text = "Asociaciones"
                        LinkLabel1.Text = "El programa habilitará o deshabilitará algunas características atendiendo a lo que soporte la versión de DISM. ¿Cómo va a afectar esto mi uso del programa, y qué características serán deshabilitadas?"
                        LinkLabel1.LinkArea = New LinkArea(111, 88)
                        LinkLabel2.Text = "Conocer más sobre los procesos en segundo plano"
                        LogSFD.Title = "Especifique la ubicación del archivo de registro"
                        RadioButton1.Text = "Imagen de Windows montada"
                        RadioButton2.Text = "Instalación actual"
                        RadioButton3.Text = "Utilizar el directorio temporal del proyecto o del programa"
                        RadioButton4.Text = "Utilizar el directorio temporal especificado"
                        RadioButton5.Text = "Moderno"
                        RadioButton6.Text = "Clásico"
                        ScratchFBD.Description = "Especifique el directorio temporal que debería usar el programa:"
                End Select
            Case 1
                Text = "Options"
                Label1.Text = Text
                TabPage1.Text = "Program"
                TabPage2.Text = "Personalization"
                TabPage3.Text = "Logs"
                TabPage4.Text = "Image operations"
                TabPage5.Text = "Scratch directory"
                TabPage6.Text = "Program output"
                TabPage7.Text = "Background processes"
                TabPage8.Text = "Modules"
                TabPage9.Text = "Image detection"
                TabPage10.Text = "File associations"
                TabPage11.Text = "Startup"
                Label2.Text = "DISM executable path:"
                Label3.Text = "Version:"
                Label5.Text = "Save settings on:"
                Label6.Text = "While in volatile mode, settings will be reset on program closure."
                Label7.Text = "Color mode:"
                Label8.Text = "Language:"
                Label9.Text = "Please specify the settings for the log window:"
                Label10.Text = "Log window font:"
                Label11.Text = "Preview:"
                Label12.Text = "Operation log file:"
                Label13.Text = "When performing image operations in the command line, specify the " & Quote & "/LogPath" & Quote & " argument to save the image operation log to the target log file."
                Label14.Text = "Log file level:"
                Label17.Text = "Perform image operations on:"
                Label18.Text = "When quietly performing operation, the program will hide information and progress output. Error messages will still be shown." & CrLf & "This option will not be used when getting information of, for example, packages or features." & CrLf & "Also, when performing image servicing, your computer may restart automatically."
                Label19.Text = "When this option is checked, your computer will not restart automatically; even when quietly performing operations"
                Label20.Text = "Please specify the scratch directory to be used for DISM operations:"
                Label21.Text = "Scratch directory:"
                Label22.Text = "Space left on selected scratch directory:"
                Label25.Text = "Log view:"
                Label26.Text = "Example report:"
                Label27.Text = "Some reports do not allow being shown as a table."
                Label28.Text = "When should the program notify you about background processes being started?"
                Label29.Text = "The program uses background processes to gather complete image information, like modification dates, installed packages, features present; and more"
                Label30.Text = "Manage program modules:"
                Label31.Text = "Status:"
                Label32.Text = "not installed"
                Label33.Text = "Version:"
                Label34.Text = "could not get module version"
                Label35.Text = "Modify these settings only if you experience constant program or system slowdowns due to high CPU usage"
                Label36.Text = "Review the status of this background process:"
                Label37.Text = "Status:"
                Label40.Text = "File associations let you access project files directly, without having to load the program first"
                Label41.Text = "Association status:"
                Label42.Text = If(DetectFileAssociations(), "associations set", "associations not set")
                Label43.Text = "Set options you would like to perform when the program starts up:"
                Label44.Text = "The program will use the scratch directory provided by the project if one is loaded. If you are in online installation management mode, the program will use its scratch directory"
                Label45.Text = "Secondary progress panel style:"
                Label46.Text = "These settings aren't applicable to non-portable installations"
                Button1.Text = "Browse..."
                Button2.Text = "View DISM component versions"
                Button3.Text = "Browse..."
                Button4.Text = "Browse..."
                Button5.Text = "Install"
                Button6.Text = "Check for updates"
                Button7.Text = "Remove"
                Button9.Text = If(DetectFileAssociations(), "Remove file associations", "Set file associations")
                Button10.Text = "Advanced settings"
                If MainForm.MountedImageDetectorBW.IsBusy Then Button8.Text = "Stop" Else Button8.Text = "Start"
                Cancel_Button.Text = "Cancel"
                OK_Button.Text = "OK"
                PrefReset.Text = "Reset preferences"
                CheckBox1.Text = "Volatile mode"
                CheckBox2.Text = "Quietly perform image operations"
                CheckBox3.Text = "Skip system restart"
                CheckBox4.Text = "Use a scratch directory"
                CheckBox5.Text = "Show command output in English"
                CheckBox6.Text = "Notify me when background processes have started"
                CheckBox7.Text = "Use module when needed"
                CheckBox8.Text = "Detect mounted images at all times"
                CheckBox9.Text = "Use uppercase menus"
                CheckBox10.Text = "Automatically create logs for each operation performed"
                CheckBox11.Text = "Set custom file icons for DISMTools projects"
                CheckBox12.Text = "Remount mounted images in need of a servicing session reload"
                CheckBox13.Text = "Check for updates"
                DismOFD.Title = "Specify the DISM executable to use"
                GroupBox1.Text = "Log customization"
                GroupBox2.Text = "Notification frequency"
                GroupBox3.Text = "Module details"
                GroupBox4.Text = "Background process"
                GroupBox5.Text = "Associations"
                LinkLabel1.Text = "The program will enable or disable certain features according to what the DISM version supports. How is it going to affect my usage of this program, and which features will be disabled accordingly?"
                LinkLabel1.LinkArea = New LinkArea(97, 100)
                LinkLabel2.Text = "Learn more about background processes"
                LogSFD.Title = "Specify the location of the log file"
                RadioButton1.Text = "Mounted Windows image"
                RadioButton2.Text = "Active installation"
                RadioButton3.Text = "Use the project or program scratch directory"
                RadioButton4.Text = "Use the specified scratch directory"
                RadioButton5.Text = "Modern"
                RadioButton6.Text = "Classic"
                ScratchFBD.Description = "Specify the scratch directory the program should use:"
            Case 2
                Text = "Opciones"
                Label1.Text = Text
                TabPage1.Text = "Programa"
                TabPage2.Text = "Personalización"
                TabPage3.Text = "Registros"
                TabPage4.Text = "Operaciones"
                TabPage5.Text = "Directorio temporal"
                TabPage6.Text = "Salida del programa"
                TabPage7.Text = "Procesos en segundo plano"
                TabPage8.Text = "Módulos"
                TabPage9.Text = "Detección de imágenes"
                TabPage10.Text = "Asociaciones de archivos"
                TabPage11.Text = "Inicio"
                Label2.Text = "Ruta del ejecutable:"
                Label3.Text = "Versión:"
                Label5.Text = "Guardar configuraciones en:"
                Label6.Text = "Cuando se está en el modo volátil, las configuraciones se restablecerán al cerrar el programa."
                Label7.Text = "Modo de color:"
                Label8.Text = "Idioma:"
                Label9.Text = "Especifique las configuraciones para la ventana de registro:"
                Label10.Text = "Fuente:"
                Label11.Text = "Vista previa:"
                Label12.Text = "Archivo de registro:"
                Label13.Text = "Cuando se realizan operaciones en la línea de comandos, especifique el argumento " & Quote & "/LogPath" & Quote & " para guardar el registro de operaciones en el archivo de destino"
                Label14.Text = "Nivel de registro:"
                Label17.Text = "Realizar operaciones en:"
                Label18.Text = "Cuando se realizan operaciones silenciosamente, el programa ocultará información y salida del progreso." & CrLf & "Esta opción no se usará al obtener información de, por ejemplo, paquetes o características." & CrLf & "También, al realizar un servicio de imágenes, su sistema podría reiniciarse automáticamente."
                Label19.Text = "Cuando esta opción está marcada, su sistema no se reiniciará automáticamente; incluso si se realizan operaciones silenciosamente"
                Label20.Text = "Especifique el directorio temporal a ser usado en operaciones de DISM:"
                Label21.Text = "Directorio temporal:"
                Label22.Text = "Espacio disponible en directorio temporal:"
                Label25.Text = "Vista de registro:"
                Label26.Text = "Informe de prueba:"
                Label27.Text = "Algunos informes no permiten ser mostrados como una tabla."
                Label28.Text = "¿Cuándo debería el programa notificarle acerca de procesos en segundo plano siendo iniciados?"
                Label29.Text = "El programa utiliza procesos en segundo plano para recopilar información completa de la imagen, como fechas de modificación, paquetes instalados, características presentes; y más"
                Label30.Text = "Administrar módulos del programa:"
                Label31.Text = "Estado:"
                Label32.Text = "no instalado"
                Label33.Text = "Versión:"
                Label34.Text = "no se pudo obtener información"
                Label35.Text = "Modifique estas configuraciones solo si experimenta ralentizaciones constantes del programa o del sistema debido a un uso elevado de CPU"
                Label36.Text = "Consulte el estado de este proceso en segundo plano:"
                Label37.Text = "Estado:"
                Label40.Text = "Las asociaciones le permiten acceder a archivos de proyectos directamente, sin tener que cargar el programa en primer lugar"
                Label41.Text = "Estado de asociaciones:"
                Label42.Text = If(DetectFileAssociations(), "asociaciones establecidas", "asociaciones no establecidas")
                Label43.Text = "Establezca las opciones que le gustaría realizar cuando el programa inicie:"
                Label44.Text = "El programa usará el directorio temporal proporcionado por el proyecto si se cargó alguno. Si está en modo de administración de instalaciones en línea, el programa utilizará su directorio temporal"
                Label45.Text = "Estilo del panel de progreso secundario:"
                Label46.Text = "Estas configuraciones no son aplicables a instalaciones no portátiles"
                Button1.Text = "Examinar..."
                Button2.Text = "Ver versiones de componentes"
                Button3.Text = "Examinar..."
                Button4.Text = "Examinar..."
                Button5.Text = "Instalar"
                Button6.Text = "Comprobar actualizaciones"
                Button7.Text = "Eliminar"
                Button9.Text = If(DetectFileAssociations(), "Eliminar asociaciones", "Establecer asociaciones")
                Button10.Text = "Opciones avanzadas"
                If MainForm.MountedImageDetectorBW.IsBusy Then Button8.Text = "Detener" Else Button8.Text = "Iniciar"
                Cancel_Button.Text = "Cancelar"
                OK_Button.Text = "Aceptar"
                PrefReset.Text = "Restablecer preferencias"
                CheckBox1.Text = "Modo volátil"
                CheckBox2.Text = "Realizar operaciones silenciosamente"
                CheckBox3.Text = "Omitir reinicio del sistema"
                CheckBox4.Text = "Usar un directorio temporal"
                CheckBox5.Text = "Mostrar salida del programa en inglés"
                CheckBox6.Text = "Notificarme cuando los procesos en segundo plano se hayan iniciado"
                CheckBox7.Text = "Usar módulo cuando sea necesario"
                CheckBox8.Text = "Detectar imágenes montadas todo el tiempo"
                CheckBox9.Text = "Usar menús en mayúscula"
                CheckBox10.Text = "Crear registros para cada operación realizada automáticamente"
                CheckBox11.Text = "Establecer iconos personalizados para proyectos de DISMTools"
                CheckBox12.Text = "Remontar imágenes montadas que necesitan una recarga de su sesión de servicio"
                CheckBox13.Text = "Comprobar actualizaciones"
                DismOFD.Title = "Especifique el ejecutable de DISM a usar"
                GroupBox1.Text = "Personalización del registro"
                GroupBox2.Text = "Frecuencia de notificaciones"
                GroupBox3.Text = "Detalles de módulo"
                GroupBox4.Text = "Proceso en segundo plano"
                GroupBox5.Text = "Asociaciones"
                LinkLabel1.Text = "El programa habilitará o deshabilitará algunas características atendiendo a lo que soporte la versión de DISM. ¿Cómo va a afectar esto mi uso del programa, y qué características serán deshabilitadas?"
                LinkLabel1.LinkArea = New LinkArea(111, 88)
                LinkLabel2.Text = "Conocer más sobre los procesos en segundo plano"
                LogSFD.Title = "Especifique la ubicación del archivo de registro"
                RadioButton1.Text = "Imagen de Windows montada"
                RadioButton2.Text = "Instalación actual"
                RadioButton3.Text = "Utilizar el directorio temporal del proyecto o del programa"
                RadioButton4.Text = "Utilizar el directorio temporal especificado"
                RadioButton5.Text = "Moderno"
                RadioButton6.Text = "Clásico"
                ScratchFBD.Description = "Especifique el directorio temporal que debería usar el programa:"
        End Select
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        SaveLocations(0) = "Settings file"
                        SaveLocations(1) = "Registry"
                        ColorModes(0) = "Use system setting"
                        ColorModes(1) = "Light mode"
                        ColorModes(2) = "Dark mode"
                        Languages(0) = "Use system language"
                        Languages(1) = "English"
                        Languages(2) = "Spanish"
                        LogViews(0) = "list"
                        LogViews(1) = "table"
                        NotFreqs(0) = "Every time a project has been loaded successfully"
                        NotFreqs(1) = "Once"
                    Case "ESN"
                        SaveLocations(0) = "Archivo de configuración"
                        SaveLocations(1) = "Registro"
                        ColorModes(0) = "Usar configuración del sistema"
                        ColorModes(1) = "Modo claro"
                        ColorModes(2) = "Modo oscuro"
                        Languages(0) = "Usar idioma del sistema"
                        Languages(1) = "Inglés"
                        Languages(2) = "Español"
                        LogViews(0) = "lista"
                        LogViews(1) = "tabla"
                        NotFreqs(0) = "Cada vez que un proyecto ha sido cargado satisfactoriamente"
                        NotFreqs(1) = "Una vez"
                End Select
            Case 1
                SaveLocations(0) = "Settings file"
                SaveLocations(1) = "Registry"
                ColorModes(0) = "Use system setting"
                ColorModes(1) = "Light mode"
                ColorModes(2) = "Dark mode"
                Languages(0) = "Use system language"
                Languages(1) = "English"
                Languages(2) = "Spanish"
                LogViews(0) = "list"
                LogViews(1) = "table"
                NotFreqs(0) = "Every time a project has been loaded successfully"
                NotFreqs(1) = "Once"
            Case 2
                SaveLocations(0) = "Archivo de configuración"
                SaveLocations(1) = "Registro"
                ColorModes(0) = "Usar configuración del sistema"
                ColorModes(1) = "Modo claro"
                ColorModes(2) = "Modo oscuro"
                Languages(0) = "Usar idioma del sistema"
                Languages(1) = "Inglés"
                Languages(2) = "Español"
                LogViews(0) = "lista"
                LogViews(1) = "tabla"
                NotFreqs(0) = "Cada vez que un proyecto ha sido cargado satisfactoriamente"
                NotFreqs(1) = "Una vez"
        End Select
        ComboBox1.Items.AddRange(SaveLocations)
        ComboBox2.Items.AddRange(ColorModes)
        ComboBox3.Items.AddRange(Languages)
        ComboBox5.Items.AddRange(LogViews)
        ComboBox6.Items.AddRange(NotFreqs)
        If File.Exists(Application.StartupPath & "\portable") Then ComboBox1.Items.RemoveAt(1)
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        GetSystemFonts()
        ' Set default values before loading custom ones
        TextBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
        DismVersion = FileVersionInfo.GetVersionInfo(TextBox1.Text)
        Label4.Text = DismVersion.ProductVersion
        TextBox2.Text = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\Windows\Logs\DISM\DISM.log"
        GatherCustomSettings()

        ' Set program colors
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TabPage1.BackColor = Color.FromArgb(31, 31, 31)
            TabPage2.BackColor = Color.FromArgb(31, 31, 31)
            TabPage3.BackColor = Color.FromArgb(31, 31, 31)
            TabPage4.BackColor = Color.FromArgb(31, 31, 31)
            TabPage5.BackColor = Color.FromArgb(31, 31, 31)
            TabPage6.BackColor = Color.FromArgb(31, 31, 31)
            TabPage7.BackColor = Color.FromArgb(31, 31, 31)
            TabPage8.BackColor = Color.FromArgb(31, 31, 31)
            TabPage9.BackColor = Color.FromArgb(31, 31, 31)
            TabPage10.BackColor = Color.FromArgb(31, 31, 31)
            TabPage11.BackColor = Color.FromArgb(31, 31, 31)
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox1.ForeColor = Color.White
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.ForeColor = Color.White
            TextBox3.BackColor = Color.FromArgb(31, 31, 31)
            TextBox3.ForeColor = Color.White
            TextBox4.BackColor = Color.FromArgb(31, 31, 31)
            TextBox4.ForeColor = Color.White
            LogPreview.BackColor = Color.FromArgb(31, 31, 31)
            LogPreview.ForeColor = Color.White
            ComboBox1.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox1.ForeColor = Color.White
            ComboBox2.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox2.ForeColor = Color.White
            ComboBox3.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox3.ForeColor = Color.White
            ComboBox4.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox4.ForeColor = Color.White
            ComboBox5.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox5.ForeColor = Color.White
            ComboBox6.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox6.ForeColor = Color.White
            ListBox1.BackColor = Color.FromArgb(31, 31, 31)
            ListBox1.ForeColor = Color.White
            NumericUpDown1.BackColor = Color.FromArgb(31, 31, 31)
            NumericUpDown1.ForeColor = Color.White
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
            GroupBox3.ForeColor = Color.White
            GroupBox4.ForeColor = Color.White
            GroupBox5.ForeColor = Color.White
            TrackBar1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TabPage1.BackColor = Color.FromArgb(238, 238, 242)
            TabPage2.BackColor = Color.FromArgb(238, 238, 242)
            TabPage3.BackColor = Color.FromArgb(238, 238, 242)
            TabPage4.BackColor = Color.FromArgb(238, 238, 242)
            TabPage5.BackColor = Color.FromArgb(238, 238, 242)
            TabPage6.BackColor = Color.FromArgb(238, 238, 242)
            TabPage7.BackColor = Color.FromArgb(238, 238, 242)
            TabPage8.BackColor = Color.FromArgb(238, 238, 242)
            TabPage9.BackColor = Color.FromArgb(238, 238, 242)
            TabPage10.BackColor = Color.FromArgb(238, 238, 242)
            TabPage11.BackColor = Color.FromArgb(238, 238, 242)
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox1.ForeColor = Color.Black
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.ForeColor = Color.Black
            TextBox3.BackColor = Color.FromArgb(238, 238, 242)
            TextBox3.ForeColor = Color.Black
            TextBox4.BackColor = Color.FromArgb(238, 238, 242)
            TextBox4.ForeColor = Color.Black
            LogPreview.BackColor = Color.FromArgb(238, 238, 242)
            LogPreview.ForeColor = Color.Black
            ComboBox1.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox1.ForeColor = Color.Black
            ComboBox2.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox2.ForeColor = Color.Black
            ComboBox3.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox3.ForeColor = Color.Black
            ComboBox4.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox4.ForeColor = Color.Black
            ComboBox5.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox5.ForeColor = Color.Black
            ComboBox6.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox6.ForeColor = Color.Black
            ListBox1.BackColor = Color.FromArgb(238, 238, 242)
            ListBox1.ForeColor = Color.Black
            NumericUpDown1.BackColor = Color.FromArgb(238, 238, 242)
            NumericUpDown1.ForeColor = Color.Black
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
            GroupBox3.ForeColor = Color.Black
            GroupBox4.ForeColor = Color.Black
            GroupBox5.ForeColor = Color.Black
            TrackBar1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "running", "stopped")
                        Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Stop", "Start")
                    Case "ESN"
                        Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "iniciado", "detenido")
                        Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Detener", "Iniciar")
                End Select
            Case 1
                Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "running", "stopped")
                Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Stop", "Start")
            Case 2
                Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "iniciado", "detenido")
                Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Detener", "Iniciar")
        End Select
        CheckBox11.Enabled = If(DetectFileAssociations(), False, True)
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        If Not File.Exists(Application.StartupPath & "\portable") Then
            Panel2.Enabled = False
            Panel3.Visible = True
        Else
            Panel2.Enabled = True
            Panel3.Visible = False
        End If
    End Sub

    Sub GetSystemFonts()
        ComboBox4.Items.Clear()
        For Each fntFamily As FontFamily In FontFamily.Families
            ComboBox4.Items.Add(fntFamily.Name)
        Next
        If ComboBox4.SelectedItem = Nothing Then ComboBox4.SelectedItem = "Courier New"
    End Sub

    Sub GatherCustomSettings()
        TextBox1.Text = MainForm.DismExe
        DismVersion = FileVersionInfo.GetVersionInfo(TextBox1.Text)
        Label4.Text = DismVersion.ProductVersion
        If MainForm.SaveOnSettingsIni Then
            ComboBox1.SelectedIndex = 0
        Else
            ComboBox1.SelectedIndex = 1
        End If
        If MainForm.VolatileMode Then
            CheckBox1.Checked = True
        Else
            CheckBox1.Checked = False
        End If
        Select Case MainForm.ColorMode
            Case 0
                ComboBox2.SelectedIndex = 0
            Case 1
                ComboBox2.SelectedIndex = 1
            Case 2
                ComboBox2.SelectedIndex = 2
        End Select
        ComboBox3.SelectedIndex = MainForm.Language
        ComboBox4.Text = MainForm.LogFont
        NumericUpDown1.Value = MainForm.LogFontSize
        If MainForm.LogFontIsBold Then
            Toggle1.Checked = True
        Else
            Toggle1.Checked = False
        End If
        Select Case MainForm.ProgressPanelStyle
            Case 0
                RadioButton5.Checked = False
                RadioButton6.Checked = True
            Case 1
                RadioButton5.Checked = True
                RadioButton6.Checked = False
        End Select
        TextBox2.Text = MainForm.LogFile
        TrackBar1.Value = If(MainForm.LogLevel = TrackBar1.Minimum, MainForm.LogLevel, MainForm.LogLevel - 1)
        Select Case MainForm.ImgOperationMode
            Case 0
                RadioButton1.Checked = True
                RadioButton2.Checked = False
            Case 1
                RadioButton1.Checked = False
                RadioButton2.Checked = True
        End Select
        If MainForm.QuietOperations Then
            CheckBox2.Checked = True
        Else
            CheckBox2.Checked = False
        End If
        If MainForm.SysNoRestart Then
            CheckBox3.Checked = True
        Else
            CheckBox3.Checked = False
        End If
        If MainForm.UseScratch Then
            CheckBox4.Checked = True
            TextBox3.Text = MainForm.ScratchDir
        Else
            CheckBox4.Checked = False
            TextBox3.Text = ""
        End If
        If MainForm.AutoScrDir Then
            RadioButton3.Checked = True
            RadioButton4.Checked = False
        Else
            RadioButton3.Checked = False
            RadioButton4.Checked = True
        End If
        If MainForm.EnglishOutput Then
            CheckBox5.Checked = True
        Else
            CheckBox5.Checked = False
        End If
        Select Case MainForm.ReportView
            Case 0
                ComboBox5.SelectedIndex = 0
            Case 1
                ComboBox5.SelectedIndex = 1
        End Select
        If MainForm.NotificationShow Then
            CheckBox6.Checked = True
        Else
            CheckBox6.Checked = False
        End If
        Select Case MainForm.NotificationFrequency
            Case 0
                ComboBox6.SelectedIndex = 0
            Case 1
                ComboBox6.SelectedIndex = 1
        End Select
        GetRootSpace(TextBox3.Text)
        CheckBox10.Checked = MainForm.AutoLogs
        CheckBox12.Checked = MainForm.StartupRemount
        CheckBox13.Checked = MainForm.StartupUpdateCheck
        CheckBox9.Checked = MainForm.AllCaps
    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox5.SelectedIndexChanged
        Select Case ComboBox5.SelectedIndex
            Case 0
                TextBox4.Text = "Image Version: 10.0.19045.2075" & CrLf & CrLf & _
                    "Features listing for package : Microsoft-Windows-Foundation-Package~31bf3856ad364e35~amd64~~10.0.19041.1" & CrLf & CrLf & _
                    "Feature Name : TFTP" & CrLf & _
                    "State : Disabled" & CrLf & CrLf & _
                    "Feature Name : LegacyComponents" & CrLf & _
                    "State : Enabled" & CrLf & CrLf & _
                    "Feature Name : DirectPlay" & CrLf & _
                    "State : Enabled" & CrLf & CrLf & _
                    "Feature Name : SimpleTCP" & CrLf & _
                    "State : Disabled" & CrLf & CrLf & _
                    "Feature Name : Windows-Identity-Foundation" & CrLf & _
                    "State : Disabled" & CrLf & CrLf & _
                    "Feature Name : NetFx3" & CrLf & _
                    "State : Enabled"
            Case 1
                TextBox4.Text = "Image Version: 10.0.19045.2075" & CrLf & CrLf & _
                    "Features listing for package : Microsoft-Windows-Foundation-Package~31bf3856ad364e35~amd64~~10.0.19041.1" & CrLf & CrLf & CrLf & _
                    "------------------------------------------- | --------" & CrLf & _
                    "Feature Name                                | State" & CrLf & _
                    "------------------------------------------- | --------" & CrLf & _
                    "TFTP                                        | Disabled" & CrLf & _
                    "LegacyComponents                            | Enabled" & CrLf & _
                    "DirectPlay                                  | Enabled" & CrLf & _
                    "SimpleTCP                                   | Disabled" & CrLf & _
                    "Windows-Identity-Foundation                 | Disabled" & CrLf & _
                    "NetFx3                                      | Enabled"
        End Select
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        If Toggle1.Checked Then
            LogPreview.Font = New Font(ComboBox4.Text, NumericUpDown1.Value, FontStyle.Bold)
        Else
            LogPreview.Font = New Font(ComboBox4.Text, NumericUpDown1.Value, FontStyle.Regular)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DismComponents.ShowDialog()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DismOFD.ShowDialog()
    End Sub

    Private Sub DismOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles DismOFD.FileOk
        TextBox1.Text = DismOFD.FileName
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text = "" Or Not File.Exists(TextBox1.Text) Then
            TextBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
        End If
        DismVersion = FileVersionInfo.GetVersionInfo(TextBox1.Text)
        Label4.Text = DismVersion.ProductVersion
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        LogSFD.ShowDialog()
    End Sub

    Private Sub LogSFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles LogSFD.FileOk
        TextBox2.Text = LogSFD.FileName
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ScratchFBD.ShowDialog()
        If DialogResult.OK And ScratchFBD.SelectedPath <> "" Then
            TextBox3.Text = ScratchFBD.SelectedPath
        End If
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Select Case TrackBar1.Value
                            Case 0
                                Label15.Text = "Errors (Log level 1)"
                                Label16.Text = "The log file should only display errors after performing an image operation."
                            Case 1
                                Label15.Text = "Errors and warnings (Log level 2)"
                                Label16.Text = "The log file should display errors and warnings after performing an image operation."
                            Case 2
                                Label15.Text = "Errors, warnings and information messages (Log level 3)"
                                Label16.Text = "The log file should display errors, warnings and information messages after performing an image operation."
                            Case 3
                                Label15.Text = "Errors, warnings, information and debug messages (Log level 4)"
                                Label16.Text = "The log file should display errors, warnings, information and debug messages after performing an image operation."
                        End Select
                    Case "ESN"
                        Select Case TrackBar1.Value
                            Case 0
                                Label15.Text = "Errores (Nivel 1)"
                                Label16.Text = "El archivo de registro solo debe mostrar errores tras realizar una operación."
                            Case 1
                                Label15.Text = "Errores y advertencias (Nivel 2)"
                                Label16.Text = "El archivo de registro debe mostrar errores y advertencias tras realizar una operación."
                            Case 2
                                Label15.Text = "Errores, advertencias y mensajes de información (Nivel 3)"
                                Label16.Text = "El archivo de registro debe mostrar errores, advertencias y mensajes de información tras realizar una operación."
                            Case 3
                                Label15.Text = "Errores, advertencias, mensajes de información y de depuración (Nivel 4)"
                                Label16.Text = "El archivo de registro debe mostrar errores, advertencias, mensajes de información y de depuración tras realizar una operación."
                        End Select
                End Select
            Case 1
                Select Case TrackBar1.Value
                    Case 0
                        Label15.Text = "Errors (Log level 1)"
                        Label16.Text = "The log file should only display errors after performing an image operation."
                    Case 1
                        Label15.Text = "Errors and warnings (Log level 2)"
                        Label16.Text = "The log file should display errors and warnings after performing an image operation."
                    Case 2
                        Label15.Text = "Errors, warnings and information messages (Log level 3)"
                        Label16.Text = "The log file should display errors, warnings and information messages after performing an image operation."
                    Case 3
                        Label15.Text = "Errors, warnings, information and debug messages (Log level 4)"
                        Label16.Text = "The log file should display errors, warnings, information and debug messages after performing an image operation."
                End Select
            Case 2
                Select Case TrackBar1.Value
                    Case 0
                        Label15.Text = "Errores (Nivel 1)"
                        Label16.Text = "El archivo de registro solo debe mostrar errores tras realizar una operación."
                    Case 1
                        Label15.Text = "Errores y advertencias (Nivel 2)"
                        Label16.Text = "El archivo de registro debe mostrar errores y advertencias tras realizar una operación."
                    Case 2
                        Label15.Text = "Errores, advertencias y mensajes de información (Nivel 3)"
                        Label16.Text = "El archivo de registro debe mostrar errores, advertencias y mensajes de información tras realizar una operación."
                    Case 3
                        Label15.Text = "Errores, advertencias, mensajes de información y de depuración (Nivel 4)"
                        Label16.Text = "El archivo de registro debe mostrar errores, advertencias, mensajes de información y de depuración tras realizar una operación."
                End Select
        End Select
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        If RadioButton3.Checked Then
            Label20.Enabled = False
            Label21.Enabled = False
            TextBox3.Enabled = False
            Button4.Enabled = False
            Label22.Enabled = False
            Label23.Enabled = False
            Label24.Enabled = False
            PictureBox5.Enabled = False
            Label44.Enabled = True
        Else
            Label20.Enabled = True
            Label21.Enabled = True
            TextBox3.Enabled = True
            Button4.Enabled = True
            Label22.Enabled = True
            Label23.Enabled = True
            Label24.Enabled = True
            PictureBox5.Enabled = True
            Label44.Enabled = False
        End If
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        GetRootSpace(TextBox3.Text)
    End Sub

    ''' <summary>
    ''' Gets the space of the drive which contains the scratch directory (referred to as the root drive)
    ''' </summary>
    ''' <param name="SourceDir">The source scratch directory</param>
    ''' <remarks></remarks>
    Sub GetRootSpace(SourceDir As String)
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        If SourceDir = "" Then
                            Label23.Text = "Please specify a scratch directory."
                            Label24.Visible = False
                            PictureBox5.Visible = False
                            PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                            Label24.Text = "You have enough space on the selected scratch directory"
                        Else
                            Try
                                Dim drInfo As New DriveInfo(Path.GetPathRoot(SourceDir))
                                Dim FreeSpace As Double = drInfo.AvailableFreeSpace / (1024 ^ 3)
                                Label23.Text = Math.Round(FreeSpace, 2) & " GB"
                                Select Case Math.Round(FreeSpace, 0)
                                    Case Is < 5
                                        Label24.Visible = True
                                        PictureBox5.Visible = True
                                        PictureBox5.Image = New Bitmap(My.Resources.error_16px)
                                        Label24.Text = "You don't have enough space on the selected scratch directory to perform image operations. Try freeing some space from the drive"
                                    Case 5 To 19.99
                                        Label24.Visible = True
                                        PictureBox5.Visible = True
                                        PictureBox5.Image = New Bitmap(My.Resources.warning_16px)
                                        Label24.Text = "You may not have enough space on the selected scratch directory for some operations."
                                    Case Is >= 20
                                        Label24.Visible = False
                                        PictureBox5.Visible = False
                                        PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                                        Label24.Text = "You have enough space on the selected scratch directory"
                                End Select
                            Catch ex As Exception
                                Label23.Text = "Could not get available free space. Continue at your own risk"
                                Label24.Visible = False
                                PictureBox5.Visible = False
                                PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                                Label24.Text = "You have enough space on the selected scratch directory"
                                Exit Sub
                            End Try
                        End If
                    Case "ESN"
                        If SourceDir = "" Then
                            Label23.Text = "Especifique un directorio temporal."
                            Label24.Visible = False
                            PictureBox5.Visible = False
                            PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                            Label24.Text = "Hay espacio suficiente en el directorio temporal seleccionado"
                        Else
                            Try
                                Dim drInfo As New DriveInfo(Path.GetPathRoot(SourceDir))
                                Dim FreeSpace As Double = drInfo.AvailableFreeSpace / (1024 ^ 3)
                                Label23.Text = Math.Round(FreeSpace, 2) & " GB"
                                Select Case Math.Round(FreeSpace, 0)
                                    Case Is < 5
                                        Label24.Visible = True
                                        PictureBox5.Visible = True
                                        PictureBox5.Image = New Bitmap(My.Resources.error_16px)
                                        Label24.Text = "No hay espacio suficiente en el directorio temporal seleccionado para realizar operaciones con la imagen. Intente liberar algo de espacio en el disco"
                                    Case 5 To 19.99
                                        Label24.Visible = True
                                        PictureBox5.Visible = True
                                        PictureBox5.Image = New Bitmap(My.Resources.warning_16px)
                                        Label24.Text = "Podría no tener espacio suficiente en el directorio temporal seleccionado para algunas operaciones."
                                    Case Is >= 20
                                        Label24.Visible = False
                                        PictureBox5.Visible = False
                                        PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                                        Label24.Text = "Tiene espacio suficiente en el directorio temporal seleccionado"
                                End Select
                            Catch ex As Exception
                                Label23.Text = "No pudimos obtener el espacio libre disponible. Continúe bajo su propio riesgo"
                                Label24.Visible = False
                                PictureBox5.Visible = False
                                PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                                Label24.Text = "Tiene espacio suficiente en el directorio temporal seleccionado"
                                Exit Sub
                            End Try
                        End If
                End Select
            Case 1
                If SourceDir = "" Then
                    Label23.Text = "Please specify a scratch directory."
                    Label24.Visible = False
                    PictureBox5.Visible = False
                    PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                    Label24.Text = "You have enough space on the selected scratch directory"
                Else
                    Try
                        Dim drInfo As New DriveInfo(Path.GetPathRoot(SourceDir))
                        Dim FreeSpace As Double = drInfo.AvailableFreeSpace / (1024 ^ 3)
                        Label23.Text = Math.Round(FreeSpace, 2) & " GB"
                        Select Case Math.Round(FreeSpace, 0)
                            Case Is < 5
                                Label24.Visible = True
                                PictureBox5.Visible = True
                                PictureBox5.Image = New Bitmap(My.Resources.error_16px)
                                Label24.Text = "You don't have enough space on the selected scratch directory to perform image operations. Try freeing some space from the drive"
                            Case 5 To 19.99
                                Label24.Visible = True
                                PictureBox5.Visible = True
                                PictureBox5.Image = New Bitmap(My.Resources.warning_16px)
                                Label24.Text = "You may not have enough space on the selected scratch directory for some operations."
                            Case Is >= 20
                                Label24.Visible = False
                                PictureBox5.Visible = False
                                PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                                Label24.Text = "You have enough space on the selected scratch directory"
                        End Select
                    Catch ex As Exception
                        Label23.Text = "Could not get available free space. Continue at your own risk"
                        Label24.Visible = False
                        PictureBox5.Visible = False
                        PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                        Label24.Text = "You have enough space on the selected scratch directory"
                        Exit Sub
                    End Try
                End If
            Case 2
                If SourceDir = "" Then
                    Label23.Text = "Especifique un directorio temporal."
                    Label24.Visible = False
                    PictureBox5.Visible = False
                    PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                    Label24.Text = "Hay espacio suficiente en el directorio temporal seleccionado"
                Else
                    Try
                        Dim drInfo As New DriveInfo(Path.GetPathRoot(SourceDir))
                        Dim FreeSpace As Double = drInfo.AvailableFreeSpace / (1024 ^ 3)
                        Label23.Text = Math.Round(FreeSpace, 2) & " GB"
                        Select Case Math.Round(FreeSpace, 0)
                            Case Is < 5
                                Label24.Visible = True
                                PictureBox5.Visible = True
                                PictureBox5.Image = New Bitmap(My.Resources.error_16px)
                                Label24.Text = "No hay espacio suficiente en el directorio temporal seleccionado para realizar operaciones con la imagen. Intente liberar algo de espacio en el disco"
                            Case 5 To 19.99
                                Label24.Visible = True
                                PictureBox5.Visible = True
                                PictureBox5.Image = New Bitmap(My.Resources.warning_16px)
                                Label24.Text = "Podría no tener espacio suficiente en el directorio temporal seleccionado para algunas operaciones."
                            Case Is >= 20
                                Label24.Visible = False
                                PictureBox5.Visible = False
                                PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                                Label24.Text = "Tiene espacio suficiente en el directorio temporal seleccionado"
                        End Select
                    Catch ex As Exception
                        Label23.Text = "No pudimos obtener el espacio libre disponible. Continúe bajo su propio riesgo"
                        Label24.Visible = False
                        PictureBox5.Visible = False
                        PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                        Label24.Text = "Tiene espacio suficiente en el directorio temporal seleccionado"
                        Exit Sub
                    End Try
                End If
        End Select
    End Sub

    Private Sub Toggle1_CheckedChanged(sender As Object, e As EventArgs) Handles Toggle1.CheckedChanged
        If Toggle1.Checked Then
            LogPreview.Font = New Font(ComboBox4.Text, NumericUpDown1.Value, FontStyle.Bold)
        Else
            LogPreview.Font = New Font(ComboBox4.Text, NumericUpDown1.Value, FontStyle.Regular)
        End If
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        If Toggle1.Checked Then
            LogPreview.Font = New Font(ComboBox4.Text, NumericUpDown1.Value, FontStyle.Bold)
        Else
            LogPreview.Font = New Font(ComboBox4.Text, NumericUpDown1.Value, FontStyle.Regular)
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        If Button8.Text = "Stop" Then
                            MainForm.MountedImageDetectorBW.CancelAsync()
                        ElseIf Button8.Text = "Start" Then
                            Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
                        End If
                        Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "running", "stopped")
                        Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Stop", "Start")
                    Case "ESN"
                        If Button8.Text = "Detener" Then
                            MainForm.MountedImageDetectorBW.CancelAsync()
                        ElseIf Button8.Text = "Iniciar" Then
                            Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
                        End If
                        Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "iniciado", "detenido")
                        Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Detener", "Iniciar")
                End Select
            Case 1
                If Button8.Text = "Stop" Then
                    MainForm.MountedImageDetectorBW.CancelAsync()
                ElseIf Button8.Text = "Start" Then
                    Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
                End If
                Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "running", "stopped")
                Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Stop", "Start")
            Case 2
                If Button8.Text = "Detener" Then
                    MainForm.MountedImageDetectorBW.CancelAsync()
                ElseIf Button8.Text = "Iniciar" Then
                    Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
                End If
                Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "iniciado", "detenido")
                Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Detener", "Iniciar")
        End Select
    End Sub

    Private Sub CheckBox10_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox10.CheckedChanged
        If CheckBox10.Checked Then
            Label12.Enabled = False
            TextBox2.Enabled = False
            Button3.Enabled = False
        Else
            Label12.Enabled = True
            TextBox2.Enabled = True
            Button3.Enabled = True
        End If
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        If CheckBox6.Checked Then
            GroupBox2.Enabled = True
        Else
            GroupBox2.Enabled = False
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If DetectFileAssociations() Then ManageAssociations(1, False) Else ManageAssociations(0, If(CheckBox11.Checked, True, False))
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        BGProcsAdvSettings.ShowDialog()
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked Then
            RadioButton3.Enabled = True
            RadioButton4.Enabled = True
            If RadioButton3.Checked Then
                Label20.Enabled = False
                Label21.Enabled = False
                TextBox3.Enabled = False
                Button4.Enabled = False
                Label22.Enabled = False
                Label23.Enabled = False
                Label24.Enabled = False
                PictureBox5.Enabled = False
                Label44.Enabled = True
            Else
                Label20.Enabled = True
                Label21.Enabled = True
                TextBox3.Enabled = True
                Button4.Enabled = True
                Label22.Enabled = True
                Label23.Enabled = True
                Label24.Enabled = True
                PictureBox5.Enabled = True
                Label44.Enabled = False
            End If
        Else
            RadioButton3.Enabled = False
            RadioButton4.Enabled = False
            Label20.Enabled = False
            Label21.Enabled = False
            TextBox3.Enabled = False
            Button4.Enabled = False
            Label22.Enabled = False
            Label23.Enabled = False
            Label24.Enabled = False
            PictureBox5.Enabled = False
            Label44.Enabled = False
        End If
    End Sub

    Private Sub RadioButton5_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton5.CheckedChanged
        If RadioButton5.Checked Then
            SecProgressStylePreview.Image = My.Resources.secprogress_modern
        Else
            SecProgressStylePreview.Image = My.Resources.secprogress_classic
        End If
    End Sub

    Private Sub PrefReset_Click(sender As Object, e As EventArgs) Handles PrefReset.Click
        SettingsResetDlg.ShowDialog()
        If SettingsResetDlg.DialogResult = Windows.Forms.DialogResult.OK Then
            MainForm.ResetDTSettings()
            Cancel_Button.PerformClick()
        End If
    End Sub
End Class
