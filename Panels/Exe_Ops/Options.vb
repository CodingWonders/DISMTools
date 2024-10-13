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
    Dim Languages() As String = New String(5) {"Use system language", "English", "Spanish", "French", "Portuguese", "Italian"}
    Dim LogViews() As String = New String(1) {"list", "table"}
    Dim NotFreqs() As String = New String(1) {"Every time a project has been loaded successfully", "Once"}

    Public SectionNum As Integer = 0

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
                    If Not Directory.Exists(Path.GetDirectoryName(TextBox2.Text)) Then
                        Directory.CreateDirectory(Path.GetDirectoryName(TextBox2.Text))
                    End If
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
        MainForm.AutoCleanMounts = CheckBox22.Checked
        MainForm.AutoLogs = CheckBox10.Checked
        If MainForm.VolatileMode Then
            MainForm.SaveDTSettings()
        End If
        If MainForm.IsImageMounted Then MainForm.DetectVersions(FileVersionInfo.GetVersionInfo(MainForm.DismExe), MainForm.imgVersionInfo)
        MainForm.SkipQuestions = CheckBox14.Checked
        MainForm.AutoCompleteInfo(0) = CheckBox15.Checked
        MainForm.AutoCompleteInfo(1) = CheckBox16.Checked
        MainForm.AutoCompleteInfo(2) = CheckBox17.Checked
        MainForm.AutoCompleteInfo(3) = CheckBox18.Checked
        MainForm.AutoCompleteInfo(4) = CheckBox19.Checked
        MainForm.ColorSchemes = If(RadioButton1.Checked, 0, 1)
        Select Case MainForm.ColorSchemes
            Case 0
                MainForm.StatusStrip.BackColor = Color.FromArgb(53, 153, 41)
            Case 1
                MainForm.StatusStrip.BackColor = Color.FromArgb(0, 122, 204)
        End Select
        MainForm.ExpandedProgressPanel = CheckBox7.Checked
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
                    Case "ENU", "ENG"
                        Label42.Text = If(DetectFileAssociations(), "associations set", "associations not set")
                        Button9.Text = If(DetectFileAssociations(), "Remove file associations", "Set file associations")
                    Case "ESN"
                        Label42.Text = If(DetectFileAssociations(), "asociaciones establecidas", "asociaciones no establecidas")
                        Button9.Text = If(DetectFileAssociations(), "Eliminar asociaciones", "Establecer asociaciones")
                    Case "FRA"
                        Label42.Text = If(DetectFileAssociations(), "associations établies", "associations non établies")
                        Button9.Text = If(DetectFileAssociations(), "Supprimer les associations de fichiers", "Établir des associations de fichiers")
                    Case "PTB", "PTG"
                        Label42.Text = If(DetectFileAssociations(), "associações estabelecidas", "associações não estabelecidas")
                        Button9.Text = If(DetectFileAssociations(), "Remover associações de ficheiros", "Estabelecer associações de ficheiros")
                    Case "ITA"
                        Label42.Text = If(DetectFileAssociations(), "associazioni impostate", "associazioni non impostate")
                        Button9.Text = If(DetectFileAssociations(), "Rimuovi associazioni file", "Imposta associazioni file")
                End Select
            Case 1
                Label42.Text = If(DetectFileAssociations(), "associations set", "associations not set")
                Button9.Text = If(DetectFileAssociations(), "Remove file associations", "Set file associations")
            Case 2
                Label42.Text = If(DetectFileAssociations(), "asociaciones establecidas", "asociaciones no establecidas")
                Button9.Text = If(DetectFileAssociations(), "Eliminar asociaciones", "Establecer asociaciones")
            Case 3
                Label42.Text = If(DetectFileAssociations(), "associations établies", "associations non établies")
                Button9.Text = If(DetectFileAssociations(), "Supprimer les associations de fichiers", "Établir des associations de fichiers")
            Case 4
                Label42.Text = If(DetectFileAssociations(), "associações estabelecidas", "associações não estabelecidas")
                Button9.Text = If(DetectFileAssociations(), "Remover associações de ficheiros", "Estabelecer associações de ficheiros")
            Case 5
                Label42.Text = If(DetectFileAssociations(), "associazioni impostate", "associazioni non impostate")
                Button9.Text = If(DetectFileAssociations(), "Rimuovi associazioni file", "Imposta associazioni file")
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
                    Case "ENU", "ENG"
                        Text = "Options"
                        Label1.Text = Text
                        Label49.Text = "Program"
                        Label50.Text = "Personalization"
                        Label51.Text = "Logs"
                        Label52.Text = "Image operations"
                        Label53.Text = "Scratch directory"
                        Label54.Text = "Program output"
                        Label55.Text = "Background processes"
                        Label56.Text = "Image detection"
                        Label57.Text = "File associations"
                        Label58.Text = "Startup options"
                        Label34.Text = "Shutdown options"
                        Label2.Text = "DISM executable path:"
                        Label3.Text = "Version:"
                        Label5.Text = "Save settings on:"
                        Label6.Text = "While in volatile mode, settings will be reset on program closure."
                        Label7.Text = "Color mode:"
                        Label8.Text = "Language:"
                        'Label9.Text = "Please specify the settings for the log window:"
                        Label10.Text = "Log window font:"
                        Label11.Text = "Preview:"
                        Label12.Text = "Operation log file:"
                        Label13.Text = "When performing image operations in the command line, specify the " & Quote & "/LogPath" & Quote & " argument to save the image operation log to the target log file."
                        Label14.Text = "Log file level:"
                        Label17.Text = "Color scheme of status bar and panels:"
                        Label18.Text = "When quietly performing operations, the program will hide information and progress output. Error messages will still be shown." & CrLf & "This option will not be used when getting information of, for example, packages or features." & CrLf & "Also, when performing image servicing, your computer may restart automatically."
                        Label19.Text = "When this option is checked, your computer will not restart automatically; even when quietly performing operations"
                        Label20.Text = "Please specify the scratch directory to be used for DISM operations:"
                        Label21.Text = "Scratch directory:"
                        Label22.Text = "Space left on selected scratch directory:"
                        Label25.Text = "Log view:"
                        Label26.Text = "Example report:"
                        Label27.Text = "Some reports do not allow being shown as a table."
                        Label28.Text = "When should the program notify you about background processes being started?"
                        Label29.Text = "The program uses background processes to gather complete image information, like modification dates, installed packages, features present; and more"
                        Label30.Text = "Previews:"
                        Label31.Text = "Idle"
                        Label32.Text = "Performing operations"
                        Label33.Text = "Progress panel:"
                        Label35.Text = "Modify these settings only if you experience constant program or system slowdowns due to high CPU usage"
                        Label36.Text = "Review the status of this background process:"
                        Label37.Text = "Status:"
                        Label40.Text = "File associations let you access project files directly, without having to load the program first"
                        Label41.Text = "Association status:"
                        Label42.Text = If(DetectFileAssociations(), "associations set", "associations not set")
                        Label43.Text = "Set options you would like to perform when the program starts up:"
                        Label44.Text = "The program will use the scratch directory provided by the project if one is loaded. If you are in the online or offline installation management modes, the program will use its scratch directory"
                        Label45.Text = "Secondary progress panel style:"
                        Label46.Text = "These settings aren't applicable to non-portable installations"
                        Label47.Text = "This font may not be readable on log windows. While you can still use it, we recommend monospaced fonts for increased readability."
                        Label48.Text = "Choose the settings the program should consider when saving image information:"
                        Button1.Text = "Browse..."
                        Button2.Text = "View DISM component versions"
                        Button3.Text = "Browse..."
                        Button4.Text = "Browse..."
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
                        CheckBox7.Text = "Show log view on the progress panel by default"
                        CheckBox8.Text = "Detect mounted images at all times"
                        CheckBox9.Text = "Use uppercase menus"
                        CheckBox10.Text = "Automatically create logs for each operation performed"
                        CheckBox11.Text = "Set custom file icons for DISMTools projects"
                        CheckBox12.Text = "Remount mounted images in need of a servicing session reload"
                        CheckBox13.Text = "Check for updates"
                        CheckBox14.Text = "Always save complete information for the following elements:"
                        CheckBox15.Text = "Installed packages"
                        CheckBox16.Text = "Features"
                        CheckBox17.Text = "Installed AppX packages"
                        CheckBox18.Text = "Capabilities"
                        CheckBox19.Text = "Installed drivers"
                        CheckBox22.Text = "Automatically clean up mount points (launches a separate process)"
                        DismOFD.Title = "Specify the DISM executable to use"
                        Label59.Text = "Log customization"
                        Label60.Text = "Set options you would like to perform when the program closes:"
                        Label61.Text = "Preview:"
                        GroupBox5.Text = "Associations"
                        Label9.Text = "Saving image information"
                        LinkLabel1.Text = "The program will enable or disable certain features according to what the DISM version supports. How is it going to affect my usage of this program, and which features will be disabled accordingly?"
                        LinkLabel1.LinkArea = New LinkArea(97, 100)
                        LinkLabel2.Text = "Learn more about background processes"
                        LogSFD.Title = "Specify the location of the log file"
                        RadioButton1.Text = "DISMTools 0.5 Color Scheme (Green)"
                        RadioButton2.Text = "DISMTools 0.1.1-0.4.2 Color Scheme (Blue)"
                        RadioButton3.Text = "Use the project or program scratch directory"
                        RadioButton4.Text = "Use the specified scratch directory"
                        RadioButton5.Text = "Modern"
                        RadioButton6.Text = "Classic"
                        ScratchFBD.Description = "Specify the scratch directory the program should use:"
                    Case "ESN"
                        Text = "Opciones"
                        Label1.Text = Text
                        Label49.Text = "Programa"
                        Label50.Text = "Personalización"
                        Label51.Text = "Registros"
                        Label52.Text = "Operaciones"
                        Label53.Text = "Directorio temporal"
                        Label54.Text = "Salida del programa"
                        Label55.Text = "Procesos en segundo plano"
                        Label56.Text = "Detección de imágenes"
                        Label57.Text = "Asociaciones de archivos"
                        Label58.Text = "Opciones de inicio"
                        Label34.Text = "Opciones de cierre"
                        Label2.Text = "Ruta del ejecutable:"
                        Label3.Text = "Versión:"
                        Label5.Text = "Guardar configuraciones en:"
                        Label6.Text = "Cuando se está en el modo volátil, las configuraciones se restablecerán al cerrar el programa."
                        Label7.Text = "Modo de color:"
                        Label8.Text = "Idioma:"
                        'Label9.Text = "Especifique las configuraciones para la ventana de registro:"
                        Label10.Text = "Fuente:"
                        Label11.Text = "Vista previa:"
                        Label12.Text = "Archivo de registro:"
                        Label13.Text = "Cuando se realizan operaciones en la línea de comandos, especifique el argumento " & Quote & "/LogPath" & Quote & " para guardar el registro de operaciones en el archivo de destino"
                        Label14.Text = "Nivel de registro:"
                        Label17.Text = "Esquema de color de la barra de estado y los paneles:"
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
                        Label30.Text = "Vistas previas:"
                        Label31.Text = "En reposo"
                        Label32.Text = "Realizando operaciones"
                        Label33.Text = "Panel de progreso:"
                        Label35.Text = "Modifique estas configuraciones solo si experimenta ralentizaciones constantes del programa o del sistema debido a un uso elevado de CPU"
                        Label36.Text = "Consulte el estado de este proceso en segundo plano:"
                        Label37.Text = "Estado:"
                        Label40.Text = "Las asociaciones le permiten acceder a archivos de proyectos directamente, sin tener que cargar el programa en primer lugar"
                        Label41.Text = "Estado de asociaciones:"
                        Label42.Text = If(DetectFileAssociations(), "asociaciones establecidas", "asociaciones no establecidas")
                        Label43.Text = "Establezca las opciones que le gustaría realizar cuando el programa inicie:"
                        Label44.Text = "El programa usará el directorio temporal proporcionado por el proyecto si se cargó alguno. Si está en los modos de administración de instalaciones en línea o fuera de línea, el programa utilizará su directorio temporal"
                        Label45.Text = "Estilo del panel de progreso secundario:"
                        Label46.Text = "Estas configuraciones no son aplicables a instalaciones no portátiles"
                        Label47.Text = "Esta fuente podría no ser legible en ventanas de registro. Aunque todavía pueda utilizarla, le recomendamos fuentes monoespaciadas para una legibilidad aumentada."
                        Label48.Text = "Escoja las opciones que el programa debería considerar al guardar información de la imagen:"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Ver versiones de componentes"
                        Button3.Text = "Examinar..."
                        Button4.Text = "Examinar..."
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
                        CheckBox7.Text = "Mostrar vista de registro en el panel de progreso por defecto"
                        CheckBox8.Text = "Detectar imágenes montadas todo el tiempo"
                        CheckBox9.Text = "Usar menús en mayúscula"
                        CheckBox10.Text = "Crear registros para cada operación realizada automáticamente"
                        CheckBox11.Text = "Establecer iconos personalizados para proyectos de DISMTools"
                        CheckBox12.Text = "Remontar imágenes montadas que necesitan una recarga de su sesión de servicio"
                        CheckBox13.Text = "Comprobar actualizaciones"
                        CheckBox14.Text = "Siempre guardar información completa para los siguientes elementos:"
                        CheckBox15.Text = "Paquetes instalados"
                        CheckBox16.Text = "Características"
                        CheckBox17.Text = "Paquetes AppX instalados"
                        CheckBox18.Text = "Funcionalidades"
                        CheckBox19.Text = "Controladores instalados"
                        CheckBox22.Text = "Limpiar puntos de montaje automáticamente (inicia un proceso separado)"
                        DismOFD.Title = "Especifique el ejecutable de DISM a usar"
                        Label59.Text = "Personalización del registro"
                        Label60.Text = "Establezca las opciones que le gustaría realizar cuando el programa se cierra:"
                        Label61.Text = "Vista previa:"
                        GroupBox5.Text = "Asociaciones"
                        Label9.Text = "Guardando información de la imagen"
                        LinkLabel1.Text = "El programa habilitará o deshabilitará algunas características atendiendo a lo que soporte la versión de DISM. ¿Cómo va a afectar esto mi uso del programa, y qué características serán deshabilitadas?"
                        LinkLabel1.LinkArea = New LinkArea(111, 88)
                        LinkLabel2.Text = "Conocer más sobre los procesos en segundo plano"
                        LogSFD.Title = "Especifique la ubicación del archivo de registro"
                        RadioButton1.Text = "Esquema de color de DISMTools 0.5 (Verde)"
                        RadioButton2.Text = "Esquema de color de DISMTools 0.1.1-0.4.2 (Azul)"
                        RadioButton3.Text = "Utilizar el directorio temporal del proyecto o del programa"
                        RadioButton4.Text = "Utilizar el directorio temporal especificado"
                        RadioButton5.Text = "Moderno"
                        RadioButton6.Text = "Clásico"
                        ScratchFBD.Description = "Especifique el directorio temporal que debería usar el programa:"
                    Case "FRA"
                        Text = "Paramètres"
                        Label1.Text = Text
                        Label49.Text = "Programme"
                        Label50.Text = "Personnalisation"
                        Label51.Text = "Journaux"
                        Label52.Text = "Opérations sur les images"
                        Label53.Text = "Répertoire temporaire"
                        Label54.Text = "Sortie du programme"
                        Label55.Text = "Processus en arrière plan"
                        Label56.Text = "Détection des images"
                        Label57.Text = "Associations de fichiers"
                        Label58.Text = "Paramètres de démarrage"
                        Label34.Text = "Paramètres de fermeture"
                        Label2.Text = "Chemin d'accès à l'exécutable DISM :"
                        Label3.Text = "Version:"
                        Label5.Text = "Sauvegarder les paramètres sur :"
                        Label6.Text = "En mode volatile, les paramètres sont réinitialisés à la fermeture du programme."
                        Label7.Text = "Mode couleur :"
                        Label8.Text = "Langue:"
                        'Label9.Text = "Veuillez spécifier les paramètres de la fenêtre d'enregistrement :"
                        Label10.Text = "Fonte de la fenêtre du journal :"
                        Label11.Text = "Aperçu:"
                        Label12.Text = "Fichier journal des opérations :"
                        Label13.Text = "Lorsque vous effectuez des opérations sur les images dans la ligne de commande, spécifiez l'argument " & Quote & "/LogPath" & Quote & " pour sauvegarder le journal des opérations sur les images dans le fichier journal cible."
                        Label14.Text = "Niveau du fichier journal :"
                        Label17.Text = "Schéma de couleurs de la barre d'état et des panneaux :"
                        Label18.Text = "Lors de l'exécution silencieuse d'une opération, le programme masquera les informations et la progression de l'opération. Les messages d'erreur seront toujours affichés." & CrLf & "Cette option ne sera pas utilisée pour obtenir des informations, par exemple, sur les paquets ou les caractéristiques." & CrLf & "En outre, lors de la maintenance de l'image, votre ordinateur peut redémarrer automatiquement."
                        Label19.Text = "Lorsque cette option est cochée, l'ordinateur ne redémarre pas automatiquement, même lorsqu'il effectue des opérations en silence."
                        Label20.Text = "Veuillez indiquer le répertoire temporaire à utiliser pour les opérations DISM :"
                        Label21.Text = "Répertoire temporaire:"
                        Label22.Text = "Espace restant sur le répertoire temporaire sélectionné :"
                        Label25.Text = "Vue du journal :"
                        Label26.Text = "Exemple de rapport :"
                        Label27.Text = "Certains rapports ne permettent pas d'être présentés sous forme de tableau."
                        Label28.Text = "Quand le programme doit-il vous avertir du démarrage de processus en arrière plan ?"
                        Label29.Text = "Le programme utilise des processus en arrière plan pour recueillir des informations complètes sur l'image, comme les dates de modification, les paquets installés, les caractéristiques présentes, etc."
                        Label30.Text = "Aperçu :"
                        Label31.Text = "Oisiveté"
                        Label32.Text = "Exécution des opérations"
                        Label33.Text = "Panneau de progrès :"
                        Label35.Text = "Ne modifiez ces paramètres que si vous constatez des ralentissements constants du programme ou du système en raison d'une utilisation élevée de l'unité centrale."
                        Label36.Text = "Examiner l'état d'avancement de ce processus en arrière plan :"
                        Label37.Text = "État :"
                        Label40.Text = "Les associations de fichiers vous permettent d'accéder directement aux fichiers du projet, sans avoir à charger le programme au préalable."
                        Label41.Text = "État de l'association :"
                        Label42.Text = If(DetectFileAssociations(), "associations établies", "associations non établies")
                        Label43.Text = "Définissez les options que vous souhaitez exécuter au démarrage du programme :"
                        Label44.Text = "Le programme utilisera le répertoire temporaire fourni par le projet s'il en existe un. Si vous êtes en les modes de gestion de l'installation en ligne ou hors ligne, le programme utilisera son répertoire temporaire."
                        Label45.Text = "Style du panneau de progression secondaire :"
                        Label46.Text = "Ces paramètres ne s'appliquent pas aux installations non portables."
                        Label47.Text = "Cette police peut ne pas être lisible sur les fenêtres logiques. Bien que vous puissiez encore l'utiliser, nous recommandons les polices monospaces pour une meilleure lisibilité."
                        Label48.Text = "Choisissez les paramètres que le programme doit prendre en compte lors de la sauvegarde des informations de l'image :"
                        Button1.Text = "Parcourir..."
                        Button2.Text = "Voir les versions des composants DISM"
                        Button3.Text = "Parcourir..."
                        Button4.Text = "Parcourir..."
                        Button9.Text = If(DetectFileAssociations(), "Supprimer les associations de fichiers", "Établir des associations de fichiers")
                        Button10.Text = "Paramètres avancés"
                        If MainForm.MountedImageDetectorBW.IsBusy Then Button8.Text = "Arrêter" Else Button8.Text = "Démarrer"
                        Cancel_Button.Text = "Annuler"
                        OK_Button.Text = "OK"
                        PrefReset.Text = "Réinitialiser les préférences"
                        CheckBox1.Text = "Mode volatile"
                        CheckBox2.Text = "Effectuer des opérations d'image en silence"
                        CheckBox3.Text = "Sauter le redémarrage du système"
                        CheckBox4.Text = "Utiliser un répertoire temporaire"
                        CheckBox5.Text = "Afficher la sortie de la commande en anglais"
                        CheckBox6.Text = "M'avertir lorsque des processus en arrière plan ont démarré"
                        CheckBox7.Text = "Afficher par défaut la vue du journal dans le panneau de progression"
                        CheckBox8.Text = "Détecter les images montées à tout moment"
                        CheckBox9.Text = "Utiliser des menus en majuscules"
                        CheckBox10.Text = "Créer automatiquement des journaux pour chaque opération effectuée"
                        CheckBox11.Text = "Définir des icônes de fichiers personnalisés pour les projets DISMTools"
                        CheckBox12.Text = "Remonter les images montées nécessitant un rechargement de la session de maintenance"
                        CheckBox13.Text = "Mettre à jour les données"
                        CheckBox14.Text = "Sauvegardez toujours des informations complètes pour les éléments suivants :"
                        CheckBox15.Text = "Paquets installés"
                        CheckBox16.Text = "Caractéristiques"
                        CheckBox17.Text = "Paquets AppX installés"
                        CheckBox18.Text = "Capacités"
                        CheckBox19.Text = "Pilotes installés"
                        CheckBox22.Text = "Nettoyer automatiquement les points de montage (lance un processus séparé)"
                        DismOFD.Title = "Spécifier l'exécutable DISM à utiliser"
                        Label59.Text = "Personnalisation du journal"
                        Label60.Text = "Définissez les paramètres que vous souhaitez effectuer à la fermeture du programme :"
                        Label61.Text = "Aperçu :"
                        GroupBox5.Text = "Associations"
                        Label9.Text = "Sauvegarde des informations de l'image"
                        LinkLabel1.Text = "Le programme activera ou désactivera certaines caractéristiques en fonction de ce que la version de DISM prend en charge. Comment cela va-t-il affecter mon utilisation de ce programme, et quelles caractéristiques seront désactivées en conséquence ?"
                        LinkLabel1.LinkArea = New LinkArea(122, 126)
                        LinkLabel2.Text = "Savoir plus sur les processus en arrière plan"
                        LogSFD.Title = "Spécifier l'emplacement du fichier journal"
                        RadioButton1.Text = "Schéma de couleurs de DISMTools 0.5 (vert)"
                        RadioButton2.Text = "Schéma de couleurs de DISMTools 0.1.1-0.4.2 (bleu)"
                        RadioButton3.Text = "Utiliser le répertoire temporaire du projet ou du programme"
                        RadioButton4.Text = "Utiliser le répertoire temporaire spécifié"
                        RadioButton5.Text = "Moderne"
                        RadioButton6.Text = "Classique"
                        ScratchFBD.Description = "Indiquez le répertoire temporaire que le programme doit utiliser :"
                    Case "PTB", "PTG"
                        Text = "Opções"
                        Label1.Text = Text
                        Label49.Text = "Programa"
                        Label50.Text = "Personalização"
                        Label51.Text = "Registos"
                        Label52.Text = "Operações de imagem"
                        Label53.Text = "Diretório temporário"
                        Label54.Text = "Saída do programa"
                        Label55.Text = "Processos em segundo plano"
                        Label56.Text = "Deteção de imagens"
                        Label57.Text = "Associações de ficheiros"
                        Label58.Text = "Opções de arranque"
                        Label34.Text = "Opções de encerramento"
                        Label2.Text = "Localização do executável DISM:"
                        Label3.Text = "Versão:"
                        Label5.Text = "Guardar configurações em:"
                        Label6.Text = "Enquanto estiver em modo volátil, as configurações serão repostas quando o programa for encerrado."
                        Label7.Text = "Modo de cor:"
                        Label8.Text = "Idioma:"
                        Label9.Text = "Especifique as configurações para a janela de registo:"
                        Label10.Text = "Tipo de letra da janela de registo:"
                        Label11.Text = "Pré-visualização:"
                        Label12.Text = "Ficheiro de registo de operações:"
                        Label13.Text = "Quando efetuar operações de imagem na linha de comandos, especifique o argumento " & Quote & "/LogPath" & Quote & " para guardar o registo da operação de imagem no ficheiro de registo de destino."
                        Label14.Text = "Nível do ficheiro de registo:"
                        Label17.Text = "Esquema de cores da barra de estado e dos painéis:"
                        Label18.Text = "Quando as operações são efectuadas em silêncio, o programa oculta as informações e o progresso. As mensagens de erro continuarão a ser mostradas." & CrLf & "Esta opção não será utilizada para obter informações sobre, por exemplo, pacotes ou funcionalidades." & CrLf & "Além disso, ao efetuar operações de imagem, o computador pode reiniciar-se automaticamente."
                        Label19.Text = "Se esta opção estiver selecionada, o computador não será reiniciado automaticamente, mesmo quando estiver a efetuar operações silenciosas"
                        Label20.Text = "Especifique o diretório de rascunho a utilizar para as operações DISM:"
                        Label21.Text = "Diretório de rascunho:"
                        Label22.Text = "Espaço restante no diretório de rascunho selecionado:"
                        Label25.Text = "Vista de registo:"
                        Label26.Text = "Exemplo de relatório:"
                        Label27.Text = "Alguns relatórios não permitem ser mostrados como uma tabela."
                        Label28.Text = "Quando é que o programa o deve notificar sobre os processos em segundo plano que estão a ser iniciados?"
                        Label29.Text = "O programa usa processos em segundo plano para reunir informações completas sobre a imagem, como datas de modificação, pacotes instalados, recursos presentes e muito mais"
                        Label30.Text = "Pré-visualizações:"
                        Label31.Text = "Inativo"
                        Label32.Text = "Execução de operações"
                        Label33.Text = "Painel de progresso:"
                        Label35.Text = "Modifique estas configurações apenas se o programa ou o sistema abrandar constantemente devido à elevada utilização da CPU"
                        Label36.Text = "Rever o estado deste processo em segundo plano:"
                        Label37.Text = "Estado:"
                        Label40.Text = "As associações de ficheiros permitem-lhe aceder diretamente aos ficheiros do projeto, sem ter de carregar primeiro o programa"
                        Label41.Text = "Estado da associação:"
                        Label42.Text = If(DetectFileAssociations(), "associações estabelecidas", "associações não estabelecidas")
                        Label43.Text = "Definir opções que gostaria de efetuar quando o programa arranca:"
                        Label44.Text = "O programa utilizará o diretório de rascunho fornecido pelo projeto, se tiver sido carregado um. Se estiver nos modos de gestão da instalação online ou offline, o programa utilizará o seu diretório de rascunho"
                        Label45.Text = "Estilo do painel de progresso secundário:"
                        Label46.Text = "Estas configurações não são aplicáveis a instalações não portáteis"
                        Label47.Text = "Este tipo de letra pode não ser legível em janelas de registo. Embora possa continuar a utilizá-lo, recomendamos tipos de letra monoespaçados para uma maior legibilidade."
                        Label48.Text = "Escolha as configurações que o programa deve considerar quando guardar informações de imagem:"
                        Button1.Text = "Navegar..."
                        Button2.Text = "Ver versões de componentes DISM"
                        Button3.Text = "Navegar..."
                        Button4.Text = "Navegar..."
                        Button9.Text = If(DetectFileAssociations(), "Remover associações de ficheiros", "Configurar associações de ficheiros")
                        Button10.Text = "Configurações avançadas"
                        If MainForm.MountedImageDetectorBW.IsBusy Then Button8.Text = " Parar" Else Button8.Text = "Iniciar"
                        Cancel_Button.Text = "Cancelar"
                        OK_Button.Text = "OK"
                        PrefReset.Text = "Repor preferências"
                        CheckBox1.Text = "Modo volátil"
                        CheckBox2.Text = "Efetuar operações de imagem silenciosamente"
                        CheckBox3.Text = "Ignorar o reinício do sistema"
                        CheckBox4.Text = "Utilizar um diretório de rascunho"
                        CheckBox5.Text = "Mostrar a saída do comando em inglês"
                        CheckBox6.Text = "Notificar-me quando os processos em segundo plano tiverem iniciado"
                        CheckBox7.Text = "Mostrar a vista de registo no painel de progresso por predefinição"
                        CheckBox8.Text = "Detetar imagens montadas a todo o momento"
                        CheckBox9.Text = "Utilizar menus em maiúsculas"
                        CheckBox10.Text = "Criar automaticamente registos para cada operação realizada"
                        CheckBox11.Text = "Configurar ícones de ficheiros personalizados para projectos DISMTools"
                        CheckBox12.Text = "Remontar imagens montadas que necessitem de um recarregamento da sessão de manutenção"
                        CheckBox13.Text = "Verificar se há actualizações"
                        CheckBox14.Text = "Guardar sempre informações completas sobre os seguintes elementos:"
                        CheckBox15.Text = "Pacotes instalados"
                        CheckBox16.Text = "Características"
                        CheckBox17.Text = "Pacotes AppX instalados"
                        CheckBox18.Text = " Capacidades"
                        CheckBox19.Text = "Controladores instalados"
                        CheckBox22.Text = "Limpar automaticamente os pontos de montagem (inicia um processo separado)"
                        DismOFD.Title = "Especificar o executável DISM a utilizar"
                        Label59.Text = "Personalização do registo"
                        Label60.Text = "Configurar as opções que gostaria de executar quando o programa fecha:"
                        Label61.Text = "Pré-visualização:"
                        GroupBox5.Text = "Associações"
                        Label9.Text = "Guardar informação da imagem"
                        LinkLabel1.Text = "O programa irá ativar ou desativar determinadas funcionalidades de acordo com o que a versão DISM suporta. Como é que isso vai afetar a minha utilização deste programa e que funcionalidades serão desactivadas em conformidade?"
                        LinkLabel1.LinkArea = New LinkArea(107, 118)
                        LinkLabel2.Text = "Saiba mais sobre os processos em segundo plano"
                        LogSFD.Title = "Especificar a localização do ficheiro de registo"
                        RadioButton1.Text = "Esquema de cores do DISMTools 0.5 (verde)"
                        RadioButton2.Text = "Esquema de cores do DISMTools 0.1.1-0.4.2 (azul)"
                        RadioButton3.Text = "Utilizar o diretório de rascunho do projeto ou do programa"
                        RadioButton4.Text = "Utilizar o diretório de rascunho especificado"
                        RadioButton5.Text = "Moderna"
                        RadioButton6.Text = "Clássico"
                        ScratchFBD.Description = "Especificar o diretório de rascunho que o programa deve utilizar:"
                    Case "ITA"
                        Text = "Opzioni"
                        Label1.Text = Text
                        Label49.Text = "Programma"
                        Label50.Text = "Personalizzazione"
                        Label51.Text = "Registri"
                        Label52.Text = "Operazioni di immagine"
                        Label53.Text = "Cartella temporanea"
                        Label54.Text = "Output del programma"
                        Label55.Text = "Processi in secondo piano"
                        Label56.Text = "Rilevamento immagini"
                        Label57.Text = "Associazioni di file"
                        Label58.Text = "Opzioni di avvio"
                        Label34.Text = "Opzioni di spegnimento"
                        Label2.Text = "Percorso eseguibile DISM:"
                        Label3.Text = "Versione:"
                        Label5.Text = "Salva impostazioni su:"
                        Label6.Text = "In modalità volatile, le impostazioni verranno ripristinate alla chiusura del programma"
                        Label7.Text = "Modalità colore:"
                        Label8.Text = "Lingua:"
                        Label9.Text = "Specificare le impostazioni per la finestra di log:"
                        Label10.Text = "Carattere della finestra di registro:"
                        Label11.Text = "Anteprima:"
                        Label12.Text = "File registro operazioni:"
                        Label13.Text = "Quando si eseguono operazioni di immagine nella riga di comando, specificare l'argomento " & Quote & "/LogPath" & Quote & " per salvare il registro delle operazioni di immagine nel file di registro di destinazione"
                        Label14.Text = "Livello del file di registro:"
                        Label17.Text = "Schema di colore della barra di stato e dei pannelli:"
                        Label18.Text = "Quando si eseguono tranquillamente le operazioni, il programma nasconde le informazioni e l'output di avanzamento. I messaggi di errore verranno comunque visualizzati." & CrLf & "Questa opzione non verrà utilizzata quando si ottengono informazioni, ad esempio, sui pacchetti o sulle funzioni." & CrLf & "Inoltre, quando si esegue la manutenzione delle immagini, il computer potrebbe riavviarsi automaticamente."
                        Label19.Text = "Quando questa opzione è selezionata, il computer non si riavvia automaticamente, anche quando si eseguono tranquillamente delle operazioni"
                        Label20.Text = "Specificare la cartella temporanea da utilizzare per le operazioni DISM:"
                        Label21.Text = "Cartella temporanea:"
                        Label22.Text = "Spazio rimanente nella cartella temporanea selezionata:"
                        Label25.Text = "Visualizzazione del registro:"
                        Label26.Text = "Esempio di rapporto:"
                        Label27.Text = "Alcuni rapporti non possono essere visualizzati come tabella"
                        Label28.Text = "Quando il programma dovrebbe notificare l'avvio dei processi in background?"
                        Label29.Text = "Il programma utilizza i processi in background per raccogliere informazioni complete sull'immagine, come le date di modifica, i pacchetti installati, le funzioni presenti e altro ancora"
                        Label30.Text = "Anteprime:"
                        Label31.Text = "Inattivo"
                        Label32.Text = "Esecuzione di operazioni"
                        Label33.Text = "Pannello di avanzamento:"
                        Label35.Text = "Modificare queste impostazioni solo se si verificano continui rallentamenti del programma o del sistema a causa dell'elevato utilizzo della CPU"
                        Label36.Text = "Esaminare lo stato di questo processo in background:"
                        Label37.Text = "Stato:"
                        Label40.Text = "Le associazioni di file consentono di accedere direttamente ai file del progetto, senza dover prima caricare il programma"
                        Label41.Text = "Stato dell'associazione:"
                        Label42.Text = If(DetectFileAssociations(), "associazioni impostate", "associazioni non impostate")
                        Label43.Text = "Impostare le opzioni che si desidera eseguire all'avvio del programma:"
                        Label44.Text = "Il programma utilizzerà la cartella temporanea fornita dal progetto, se ne è stata caricata una. Se ci si trova nelle modalità di gestione dell'installazione online o offline, il programma utilizzerà la sua directory scratch"
                        Label45.Text = "Stile del pannello di avanzamento secondario:"
                        Label46.Text = "Queste impostazioni non sono applicabili alle installazioni non portatili"
                        Label47.Text = "Questo carattere potrebbe non essere leggibile sulle finestre di registro. Anche se è possibile utilizzarlo, si consiglia di utilizzare caratteri monospaziati per una maggiore leggibilità."
                        Label48.Text = "Scegliere le impostazioni che il programma deve considerare quando salva le informazioni sull'immagine:"
                        Button1.Text = "Sfoglia..."
                        Button2.Text = "Visualizza le versioni dei componenti DISM"
                        Button3.Text = "Sfoglia..."
                        Button4.Text = "Sfoglia..."
                        Button9.Text = If(DetectFileAssociations(), "Rimuovi associazioni file", "Imposta associazioni file")
                        Button10.Text = "Impostazioni avanzate"
                        If MainForm.MountedImageDetectorBW.IsBusy Then Button8.Text = "Arresto" Else Button8.Text = "Avvio"
                        Cancel_Button.Text = "Annullare"
                        OK_Button.Text = "OK"
                        PrefReset.Text = "Reimpostare le preferenze"
                        CheckBox1.Text = "Modalità volatile"
                        CheckBox2.Text = "Esegui silenziosamente le operazioni sull'immagine"
                        CheckBox3.Text = "Salta il riavvio del sistema"
                        CheckBox4.Text = "Utilizza una directory scratch"
                        CheckBox5.Text = "Mostra l'output del comando in inglese"
                        CheckBox6.Text = "Notifica l'avvio di processi in background"
                        CheckBox7.Text = "Mostra la visualizzazione del registro nel pannello di avanzamento per impostazione predefinita"
                        CheckBox8.Text = "Rileva sempre le immagini montate"
                        CheckBox9.Text = "Utilizza i menu in maiuscolo"
                        CheckBox10.Text = "Crea automaticamente i registri per ogni operazione eseguita"
                        CheckBox11.Text = "Imposta icone di file personalizzate per i progetti DISMTools"
                        CheckBox12.Text = "Rimonta le immagini montate che necessitano di un ricaricamento della sessione di assistenza"
                        CheckBox13.Text = "Controlla gli aggiornamenti"
                        CheckBox14.Text = "Salva sempre le informazioni complete per i seguenti elementi:"
                        CheckBox15.Text = "Pacchetti installati"
                        CheckBox16.Text = "Caratteristiche"
                        CheckBox17.Text = "Pacchetti AppX installati"
                        CheckBox18.Text = "Capacità"
                        CheckBox19.Text = "Driver installati"
                        CheckBox22.Text = "Pulisci automaticamente i punti di montaggio (lancia un processo separato)"
                        DismOFD.Title = "Specificare l'eseguibile DISM da utilizzare"
                        Label59.Text = "Personalizzazione dei registri"
                        Label60.Text = "Impostare le opzioni che si desidera eseguire alla chiusura del programma:"
                        Label61.Text = "Anteprima:"
                        GroupBox5.Text = "Associazioni"
                        Label9.Text = "Salvataggio delle informazioni sull'immagine"
                        LinkLabel1.Text = "Il programma abilita o disabilita alcune funzioni in base alla versione di DISM supportata. Come influirà sull'uso di questo programma e quali funzioni saranno disabilitate di conseguenza?"
                        LinkLabel1.LinkArea = New LinkArea(92, 100)
                        LinkLabel2.Text = "Ulteriori informazioni sui processi in background"
                        LogSFD.Title = "Specificare la posizione del file di log"
                        RadioButton1.Text = "Schema di colore DISMTools 0.5 (verde)"
                        RadioButton2.Text = "Schema di colori DISMTools 0.1.1-0.4.2 (blu)"
                        RadioButton3.Text = "Utilizza la cartella temporanea del progetto o del programma"
                        RadioButton4.Text = "Utilizza la cartella temporanea specificata"
                        RadioButton5.Text = "Moderno"
                        RadioButton6.Text = "Classic"
                        ScratchFBD.Description = "Specifica la directory di scratch che il programma deve utilizzare:"
                End Select
            Case 1
                Text = "Options"
                Label1.Text = Text
                Label49.Text = "Program"
                Label50.Text = "Personalization"
                Label51.Text = "Logs"
                Label52.Text = "Image operations"
                Label53.Text = "Scratch directory"
                Label54.Text = "Program output"
                Label55.Text = "Background processes"
                Label56.Text = "Image detection"
                Label57.Text = "File associations"
                Label58.Text = "Startup options"
                Label34.Text = "Shutdown options"
                Label2.Text = "DISM executable path:"
                Label3.Text = "Version:"
                Label5.Text = "Save settings on:"
                Label6.Text = "While in volatile mode, settings will be reset on program closure."
                Label7.Text = "Color mode:"
                Label8.Text = "Language:"
                'Label9.Text = "Please specify the settings for the log window:"
                Label10.Text = "Log window font:"
                Label11.Text = "Preview:"
                Label12.Text = "Operation log file:"
                Label13.Text = "When performing image operations in the command line, specify the " & Quote & "/LogPath" & Quote & " argument to save the image operation log to the target log file."
                Label14.Text = "Log file level:"
                Label17.Text = "Color scheme of status bar and panels:"
                Label18.Text = "When quietly performing operations, the program will hide information and progress output. Error messages will still be shown." & CrLf & "This option will not be used when getting information of, for example, packages or features." & CrLf & "Also, when performing image servicing, your computer may restart automatically."
                Label19.Text = "When this option is checked, your computer will not restart automatically; even when quietly performing operations"
                Label20.Text = "Please specify the scratch directory to be used for DISM operations:"
                Label21.Text = "Scratch directory:"
                Label22.Text = "Space left on selected scratch directory:"
                Label25.Text = "Log view:"
                Label26.Text = "Example report:"
                Label27.Text = "Some reports do not allow being shown as a table."
                Label28.Text = "When should the program notify you about background processes being started?"
                Label29.Text = "The program uses background processes to gather complete image information, like modification dates, installed packages, features present; and more"
                Label30.Text = "Previews:"
                Label31.Text = "Idle"
                Label32.Text = "Performing operations"
                Label33.Text = "Progress panel:"
                Label35.Text = "Modify these settings only if you experience constant program or system slowdowns due to high CPU usage"
                Label36.Text = "Review the status of this background process:"
                Label37.Text = "Status:"
                Label40.Text = "File associations let you access project files directly, without having to load the program first"
                Label41.Text = "Association status:"
                Label42.Text = If(DetectFileAssociations(), "associations set", "associations not set")
                Label43.Text = "Set options you would like to perform when the program starts up:"
                Label44.Text = "The program will use the scratch directory provided by the project if one is loaded. If you are in the online or offline installation management modes, the program will use its scratch directory"
                Label45.Text = "Secondary progress panel style:"
                Label46.Text = "These settings aren't applicable to non-portable installations"
                Label47.Text = "This font may not be readable on log windows. While you can still use it, we recommend monospaced fonts for increased readability."
                Label48.Text = "Choose the settings the program should consider when saving image information:"
                Button1.Text = "Browse..."
                Button2.Text = "View DISM component versions"
                Button3.Text = "Browse..."
                Button4.Text = "Browse..."
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
                CheckBox7.Text = "Show log view on the progress panel by default"
                CheckBox8.Text = "Detect mounted images at all times"
                CheckBox9.Text = "Use uppercase menus"
                CheckBox10.Text = "Automatically create logs for each operation performed"
                CheckBox11.Text = "Set custom file icons for DISMTools projects"
                CheckBox12.Text = "Remount mounted images in need of a servicing session reload"
                CheckBox13.Text = "Check for updates"
                CheckBox14.Text = "Always save complete information for the following elements:"
                CheckBox15.Text = "Installed packages"
                CheckBox16.Text = "Features"
                CheckBox17.Text = "Installed AppX packages"
                CheckBox18.Text = "Capabilities"
                CheckBox19.Text = "Installed drivers"
                CheckBox22.Text = "Automatically clean up mount points (launches a separate process)"
                DismOFD.Title = "Specify the DISM executable to use"
                Label59.Text = "Log customization"
                Label60.Text = "Set options you would like to perform when the program closes:"
                Label61.Text = "Preview:"
                GroupBox5.Text = "Associations"
                Label9.Text = "Saving image information"
                LinkLabel1.Text = "The program will enable or disable certain features according to what the DISM version supports. How is it going to affect my usage of this program, and which features will be disabled accordingly?"
                LinkLabel1.LinkArea = New LinkArea(97, 100)
                LinkLabel2.Text = "Learn more about background processes"
                LogSFD.Title = "Specify the location of the log file"
                RadioButton1.Text = "DISMTools 0.5 Color Scheme (Green)"
                RadioButton2.Text = "DISMTools 0.1.1-0.4.2 Color Scheme (Blue)"
                RadioButton3.Text = "Use the project or program scratch directory"
                RadioButton4.Text = "Use the specified scratch directory"
                RadioButton5.Text = "Modern"
                RadioButton6.Text = "Classic"
                ScratchFBD.Description = "Specify the scratch directory the program should use:"
            Case 2
                Text = "Opciones"
                Label1.Text = Text
                Label49.Text = "Programa"
                Label50.Text = "Personalización"
                Label51.Text = "Registros"
                Label52.Text = "Operaciones"
                Label53.Text = "Directorio temporal"
                Label54.Text = "Salida del programa"
                Label55.Text = "Procesos en segundo plano"
                Label56.Text = "Detección de imágenes"
                Label57.Text = "Asociaciones de archivos"
                Label58.Text = "Opciones de inicio"
                Label34.Text = "Opciones de cierre"
                Label2.Text = "Ruta del ejecutable:"
                Label3.Text = "Versión:"
                Label5.Text = "Guardar configuraciones en:"
                Label6.Text = "Cuando se está en el modo volátil, las configuraciones se restablecerán al cerrar el programa."
                Label7.Text = "Modo de color:"
                Label8.Text = "Idioma:"
                'Label9.Text = "Especifique las configuraciones para la ventana de registro:"
                Label10.Text = "Fuente:"
                Label11.Text = "Vista previa:"
                Label12.Text = "Archivo de registro:"
                Label13.Text = "Cuando se realizan operaciones en la línea de comandos, especifique el argumento " & Quote & "/LogPath" & Quote & " para guardar el registro de operaciones en el archivo de destino"
                Label14.Text = "Nivel de registro:"
                Label17.Text = "Esquema de color de la barra de estado y los paneles:"
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
                Label30.Text = "Vistas previas:"
                Label31.Text = "En reposo"
                Label32.Text = "Realizando operaciones"
                Label33.Text = "Panel de progreso:"
                Label35.Text = "Modifique estas configuraciones solo si experimenta ralentizaciones constantes del programa o del sistema debido a un uso elevado de CPU"
                Label36.Text = "Consulte el estado de este proceso en segundo plano:"
                Label37.Text = "Estado:"
                Label40.Text = "Las asociaciones le permiten acceder a archivos de proyectos directamente, sin tener que cargar el programa en primer lugar"
                Label41.Text = "Estado de asociaciones:"
                Label42.Text = If(DetectFileAssociations(), "asociaciones establecidas", "asociaciones no establecidas")
                Label43.Text = "Establezca las opciones que le gustaría realizar cuando el programa inicie:"
                Label44.Text = "El programa usará el directorio temporal proporcionado por el proyecto si se cargó alguno. Si está en los modos de administración de instalaciones en línea o fuera de línea, el programa utilizará su directorio temporal"
                Label45.Text = "Estilo del panel de progreso secundario:"
                Label46.Text = "Estas configuraciones no son aplicables a instalaciones no portátiles"
                Label47.Text = "Esta fuente podría no ser legible en ventanas de registro. Aunque todavía pueda utilizarla, le recomendamos fuentes monoespaciadas para una legibilidad aumentada."
                Label48.Text = "Escoja las opciones que el programa debería considerar al guardar información de la imagen:"
                Button1.Text = "Examinar..."
                Button2.Text = "Ver versiones de componentes"
                Button3.Text = "Examinar..."
                Button4.Text = "Examinar..."
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
                CheckBox7.Text = "Mostrar vista de registro en el panel de progreso por defecto"
                CheckBox8.Text = "Detectar imágenes montadas todo el tiempo"
                CheckBox9.Text = "Usar menús en mayúscula"
                CheckBox10.Text = "Crear registros para cada operación realizada automáticamente"
                CheckBox11.Text = "Establecer iconos personalizados para proyectos de DISMTools"
                CheckBox12.Text = "Remontar imágenes montadas que necesitan una recarga de su sesión de servicio"
                CheckBox13.Text = "Comprobar actualizaciones"
                CheckBox14.Text = "Siempre guardar información completa para los siguientes elementos:"
                CheckBox15.Text = "Paquetes instalados"
                CheckBox16.Text = "Características"
                CheckBox17.Text = "Paquetes AppX instalados"
                CheckBox18.Text = "Funcionalidades"
                CheckBox19.Text = "Controladores instalados"
                CheckBox22.Text = "Limpiar puntos de montaje automáticamente (inicia un proceso separado)"
                DismOFD.Title = "Especifique el ejecutable de DISM a usar"
                Label59.Text = "Personalización del registro"
                Label60.Text = "Establezca las opciones que le gustaría realizar cuando el programa se cierra:"
                Label61.Text = "Vista previa:"
                GroupBox5.Text = "Asociaciones"
                Label9.Text = "Guardando información de la imagen"
                LinkLabel1.Text = "El programa habilitará o deshabilitará algunas características atendiendo a lo que soporte la versión de DISM. ¿Cómo va a afectar esto mi uso del programa, y qué características serán deshabilitadas?"
                LinkLabel1.LinkArea = New LinkArea(111, 88)
                LinkLabel2.Text = "Conocer más sobre los procesos en segundo plano"
                LogSFD.Title = "Especifique la ubicación del archivo de registro"
                RadioButton1.Text = "Esquema de color de DISMTools 0.5 (Verde)"
                RadioButton2.Text = "Esquema de color de DISMTools 0.1.1-0.4.2 (Azul)"
                RadioButton3.Text = "Utilizar el directorio temporal del proyecto o del programa"
                RadioButton4.Text = "Utilizar el directorio temporal especificado"
                RadioButton5.Text = "Moderno"
                RadioButton6.Text = "Clásico"
                ScratchFBD.Description = "Especifique el directorio temporal que debería usar el programa:"
            Case 3
                Text = "Paramètres"
                Label1.Text = Text
                Label49.Text = "Programme"
                Label50.Text = "Personnalisation"
                Label51.Text = "Journaux"
                Label52.Text = "Opérations sur les images"
                Label53.Text = "Répertoire temporaire"
                Label54.Text = "Sortie du programme"
                Label55.Text = "Processus en arrière plan"
                Label56.Text = "Détection des images"
                Label57.Text = "Associations de fichiers"
                Label58.Text = "Paramètres de démarrage"
                Label34.Text = "Paramètres de fermeture"
                Label2.Text = "Chemin d'accès à l'exécutable DISM :"
                Label3.Text = "Version:"
                Label5.Text = "Sauvegarder les paramètres sur :"
                Label6.Text = "En mode volatile, les paramètres sont réinitialisés à la fermeture du programme."
                Label7.Text = "Mode couleur :"
                Label8.Text = "Langue:"
                'Label9.Text = "Veuillez spécifier les paramètres de la fenêtre d'enregistrement :"
                Label10.Text = "Fonte de la fenêtre du journal :"
                Label11.Text = "Aperçu:"
                Label12.Text = "Fichier journal des opérations :"
                Label13.Text = "Lorsque vous effectuez des opérations sur les images dans la ligne de commande, spécifiez l'argument " & Quote & "/LogPath" & Quote & " pour sauvegarder le journal des opérations sur les images dans le fichier journal cible."
                Label14.Text = "Niveau du fichier journal :"
                Label17.Text = "Schéma de couleurs de la barre d'état et des panneaux :"
                Label18.Text = "Lors de l'exécution silencieuse d'une opération, le programme masquera les informations et la progression de l'opération. Les messages d'erreur seront toujours affichés." & CrLf & "Cette option ne sera pas utilisée pour obtenir des informations, par exemple, sur les paquets ou les caractéristiques." & CrLf & "En outre, lors de la maintenance de l'image, votre ordinateur peut redémarrer automatiquement."
                Label19.Text = "Lorsque cette option est cochée, l'ordinateur ne redémarre pas automatiquement, même lorsqu'il effectue des opérations en silence."
                Label20.Text = "Veuillez indiquer le répertoire temporaire à utiliser pour les opérations DISM :"
                Label21.Text = "Répertoire temporaire:"
                Label22.Text = "Espace restant sur le répertoire temporaire sélectionné :"
                Label25.Text = "Vue du journal :"
                Label26.Text = "Exemple de rapport :"
                Label27.Text = "Certains rapports ne permettent pas d'être présentés sous forme de tableau."
                Label28.Text = "Quand le programme doit-il vous avertir du démarrage de processus en arrière plan ?"
                Label29.Text = "Le programme utilise des processus en arrière plan pour recueillir des informations complètes sur l'image, comme les dates de modification, les paquets installés, les caractéristiques présentes, etc."
                Label30.Text = "Aperçu :"
                Label31.Text = "Oisiveté"
                Label32.Text = "Exécution des opérations"
                Label33.Text = "Panneau de progrès :"
                Label35.Text = "Ne modifiez ces paramètres que si vous constatez des ralentissements constants du programme ou du système en raison d'une utilisation élevée de l'unité centrale."
                Label36.Text = "Examiner l'état d'avancement de ce processus en arrière plan :"
                Label37.Text = "État :"
                Label40.Text = "Les associations de fichiers vous permettent d'accéder directement aux fichiers du projet, sans avoir à charger le programme au préalable."
                Label41.Text = "État de l'association :"
                Label42.Text = If(DetectFileAssociations(), "associations établies", "associations non établies")
                Label43.Text = "Définissez les options que vous souhaitez exécuter au démarrage du programme :"
                Label44.Text = "Le programme utilisera le répertoire temporaire fourni par le projet s'il en existe un. Si vous êtes en les modes de gestion de l'installation en ligne ou hors ligne, le programme utilisera son répertoire temporaire."
                Label45.Text = "Style du panneau de progression secondaire :"
                Label46.Text = "Ces paramètres ne s'appliquent pas aux installations non portables."
                Label47.Text = "Cette police peut ne pas être lisible sur les fenêtres logiques. Bien que vous puissiez encore l'utiliser, nous recommandons les polices monospaces pour une meilleure lisibilité."
                Label48.Text = "Choisissez les paramètres que le programme doit prendre en compte lors de la sauvegarde des informations de l'image :"
                Button1.Text = "Parcourir..."
                Button2.Text = "Voir les versions des composants DISM"
                Button3.Text = "Parcourir..."
                Button4.Text = "Parcourir..."
                Button9.Text = If(DetectFileAssociations(), "Supprimer les associations de fichiers", "Établir des associations de fichiers")
                Button10.Text = "Paramètres avancés"
                If MainForm.MountedImageDetectorBW.IsBusy Then Button8.Text = "Arrêter" Else Button8.Text = "Démarrer"
                Cancel_Button.Text = "Annuler"
                OK_Button.Text = "OK"
                PrefReset.Text = "Réinitialiser les préférences"
                CheckBox1.Text = "Mode volatile"
                CheckBox2.Text = "Effectuer des opérations d'image en silence"
                CheckBox3.Text = "Sauter le redémarrage du système"
                CheckBox4.Text = "Utiliser un répertoire temporaire"
                CheckBox5.Text = "Afficher la sortie de la commande en anglais"
                CheckBox6.Text = "M'avertir lorsque des processus en arrière plan ont démarré"
                CheckBox7.Text = "Afficher par défaut la vue du journal dans le panneau de progression"
                CheckBox8.Text = "Détecter les images montées à tout moment"
                CheckBox9.Text = "Utiliser des menus en majuscules"
                CheckBox10.Text = "Créer automatiquement des journaux pour chaque opération effectuée"
                CheckBox11.Text = "Définir des icônes de fichiers personnalisés pour les projets DISMTools"
                CheckBox12.Text = "Remonter les images montées nécessitant un rechargement de la session de maintenance"
                CheckBox13.Text = "Mettre à jour les données"
                CheckBox14.Text = "Sauvegardez toujours des informations complètes pour les éléments suivants :"
                CheckBox15.Text = "Paquets installés"
                CheckBox16.Text = "Caractéristiques"
                CheckBox17.Text = "Paquets AppX installés"
                CheckBox18.Text = "Capacités"
                CheckBox19.Text = "Pilotes installés"
                CheckBox22.Text = "Nettoyer automatiquement les points de montage (lance un processus séparé)"
                DismOFD.Title = "Spécifier l'exécutable DISM à utiliser"
                Label59.Text = "Personnalisation du journal"
                Label60.Text = "Définissez les paramètres que vous souhaitez effectuer à la fermeture du programme :"
                Label61.Text = "Aperçu :"
                GroupBox5.Text = "Associations"
                Label9.Text = "Sauvegarde des informations de l'image"
                LinkLabel1.Text = "Le programme activera ou désactivera certaines caractéristiques en fonction de ce que la version de DISM prend en charge. Comment cela va-t-il affecter mon utilisation de ce programme, et quelles caractéristiques seront désactivées en conséquence ?"
                LinkLabel1.LinkArea = New LinkArea(122, 126)
                LinkLabel2.Text = "Savoir plus sur les processus en arrière plan"
                LogSFD.Title = "Spécifier l'emplacement du fichier journal"
                RadioButton1.Text = "Schéma de couleurs de DISMTools 0.5 (vert)"
                RadioButton2.Text = "Schéma de couleurs de DISMTools 0.1.1-0.4.2 (bleu)"
                RadioButton3.Text = "Utiliser le répertoire temporaire du projet ou du programme"
                RadioButton4.Text = "Utiliser le répertoire temporaire spécifié"
                RadioButton5.Text = "Moderne"
                RadioButton6.Text = "Classique"
                ScratchFBD.Description = "Indiquez le répertoire temporaire que le programme doit utiliser :"
            Case 4
                Text = "Opções"
                Label1.Text = Text
                Label49.Text = "Programa"
                Label50.Text = "Personalização"
                Label51.Text = "Registos"
                Label52.Text = "Operações de imagem"
                Label53.Text = "Diretório temporário"
                Label54.Text = "Saída do programa"
                Label55.Text = "Processos em segundo plano"
                Label56.Text = "Deteção de imagens"
                Label57.Text = "Associações de ficheiros"
                Label58.Text = "Opções de arranque"
                Label34.Text = "Opções de encerramento"
                Label2.Text = "Localização do executável DISM:"
                Label3.Text = "Versão:"
                Label5.Text = "Guardar configurações em:"
                Label6.Text = "Enquanto estiver em modo volátil, as configurações serão repostas quando o programa for encerrado."
                Label7.Text = "Modo de cor:"
                Label8.Text = "Idioma:"
                Label9.Text = "Especifique as configurações para a janela de registo:"
                Label10.Text = "Tipo de letra da janela de registo:"
                Label11.Text = "Pré-visualização:"
                Label12.Text = "Ficheiro de registo de operações:"
                Label13.Text = "Quando efetuar operações de imagem na linha de comandos, especifique o argumento " & Quote & "/LogPath" & Quote & " para guardar o registo da operação de imagem no ficheiro de registo de destino."
                Label14.Text = "Nível do ficheiro de registo:"
                Label17.Text = "Esquema de cores da barra de estado e dos painéis:"
                Label18.Text = "Quando as operações são efectuadas em silêncio, o programa oculta as informações e o progresso. As mensagens de erro continuarão a ser mostradas." & CrLf & "Esta opção não será utilizada para obter informações sobre, por exemplo, pacotes ou funcionalidades." & CrLf & "Além disso, ao efetuar operações de imagem, o computador pode reiniciar-se automaticamente."
                Label19.Text = "Se esta opção estiver selecionada, o computador não será reiniciado automaticamente, mesmo quando estiver a efetuar operações silenciosas"
                Label20.Text = "Especifique o diretório de rascunho a utilizar para as operações DISM:"
                Label21.Text = "Diretório de rascunho:"
                Label22.Text = "Espaço restante no diretório de rascunho selecionado:"
                Label25.Text = "Vista de registo:"
                Label26.Text = "Exemplo de relatório:"
                Label27.Text = "Alguns relatórios não permitem ser mostrados como uma tabela."
                Label28.Text = "Quando é que o programa o deve notificar sobre os processos em segundo plano que estão a ser iniciados?"
                Label29.Text = "O programa usa processos em segundo plano para reunir informações completas sobre a imagem, como datas de modificação, pacotes instalados, recursos presentes e muito mais"
                Label30.Text = "Pré-visualizações:"
                Label31.Text = "Inativo"
                Label32.Text = "Execução de operações"
                Label33.Text = "Painel de progresso:"
                Label35.Text = "Modifique estas configurações apenas se o programa ou o sistema abrandar constantemente devido à elevada utilização da CPU"
                Label36.Text = "Rever o estado deste processo em segundo plano:"
                Label37.Text = "Estado:"
                Label40.Text = "As associações de ficheiros permitem-lhe aceder diretamente aos ficheiros do projeto, sem ter de carregar primeiro o programa"
                Label41.Text = "Estado da associação:"
                Label42.Text = If(DetectFileAssociations(), "associações estabelecidas", "associações não estabelecidas")
                Label43.Text = "Definir opções que gostaria de efetuar quando o programa arranca:"
                Label44.Text = "O programa utilizará o diretório de rascunho fornecido pelo projeto, se tiver sido carregado um. Se estiver nos modos de gestão da instalação online ou offline, o programa utilizará o seu diretório de rascunho"
                Label45.Text = "Estilo do painel de progresso secundário:"
                Label46.Text = "Estas configurações não são aplicáveis a instalações não portáteis"
                Label47.Text = "Este tipo de letra pode não ser legível em janelas de registo. Embora possa continuar a utilizá-lo, recomendamos tipos de letra monoespaçados para uma maior legibilidade."
                Label48.Text = "Escolha as configurações que o programa deve considerar quando guardar informações de imagem:"
                Button1.Text = "Navegar..."
                Button2.Text = "Ver versões de componentes DISM"
                Button3.Text = "Navegar..."
                Button4.Text = "Navegar..."
                Button9.Text = If(DetectFileAssociations(), "Remover associações de ficheiros", "Configurar associações de ficheiros")
                Button10.Text = "Configurações avançadas"
                If MainForm.MountedImageDetectorBW.IsBusy Then Button8.Text = " Parar" Else Button8.Text = "Iniciar"
                Cancel_Button.Text = "Cancelar"
                OK_Button.Text = "OK"
                PrefReset.Text = "Repor preferências"
                CheckBox1.Text = "Modo volátil"
                CheckBox2.Text = "Efetuar operações de imagem silenciosamente"
                CheckBox3.Text = "Ignorar o reinício do sistema"
                CheckBox4.Text = "Utilizar um diretório de rascunho"
                CheckBox5.Text = "Mostrar a saída do comando em inglês"
                CheckBox6.Text = "Notificar-me quando os processos em segundo plano tiverem iniciado"
                CheckBox7.Text = "Mostrar a vista de registo no painel de progresso por predefinição"
                CheckBox8.Text = "Detetar imagens montadas a todo o momento"
                CheckBox9.Text = "Utilizar menus em maiúsculas"
                CheckBox10.Text = "Criar automaticamente registos para cada operação realizada"
                CheckBox11.Text = "Configurar ícones de ficheiros personalizados para projectos DISMTools"
                CheckBox12.Text = "Remontar imagens montadas que necessitem de um recarregamento da sessão de manutenção"
                CheckBox13.Text = "Verificar se há actualizações"
                CheckBox14.Text = "Guardar sempre informações completas sobre os seguintes elementos:"
                CheckBox15.Text = "Pacotes instalados"
                CheckBox16.Text = "Características"
                CheckBox17.Text = "Pacotes AppX instalados"
                CheckBox18.Text = " Capacidades"
                CheckBox19.Text = "Controladores instalados"
                CheckBox22.Text = "Limpar automaticamente os pontos de montagem (inicia um processo separado)"
                DismOFD.Title = "Especificar o executável DISM a utilizar"
                Label59.Text = "Personalização do registo"
                Label60.Text = "Configurar as opções que gostaria de executar quando o programa fecha:"
                Label61.Text = "Pré-visualização:"
                GroupBox5.Text = "Associações"
                Label9.Text = "Guardar informação da imagem"
                LinkLabel1.Text = "O programa irá ativar ou desativar determinadas funcionalidades de acordo com o que a versão DISM suporta. Como é que isso vai afetar a minha utilização deste programa e que funcionalidades serão desactivadas em conformidade?"
                LinkLabel1.LinkArea = New LinkArea(107, 118)
                LinkLabel2.Text = "Saiba mais sobre os processos em segundo plano"
                LogSFD.Title = "Especificar a localização do ficheiro de registo"
                RadioButton1.Text = "Esquema de cores do DISMTools 0.5 (verde)"
                RadioButton2.Text = "Esquema de cores do DISMTools 0.1.1-0.4.2 (azul)"
                RadioButton3.Text = "Utilizar o diretório de rascunho do projeto ou do programa"
                RadioButton4.Text = "Utilizar o diretório de rascunho especificado"
                RadioButton5.Text = "Moderna"
                RadioButton6.Text = "Clássico"
                ScratchFBD.Description = "Especificar o diretório de rascunho que o programa deve utilizar:"
            Case 5
                Text = "Opzioni"
                Label1.Text = Text
                Label49.Text = "Programma"
                Label50.Text = "Personalizzazione"
                Label51.Text = "Registri"
                Label52.Text = "Operazioni di immagine"
                Label53.Text = "Cartella temporanea"
                Label54.Text = "Output del programma"
                Label55.Text = "Processi in secondo piano"
                Label56.Text = "Rilevamento immagini"
                Label57.Text = "Associazioni di file"
                Label58.Text = "Opzioni di avvio"
                Label34.Text = "Opzioni di spegnimento"
                Label2.Text = "Percorso eseguibile DISM:"
                Label3.Text = "Versione:"
                Label5.Text = "Salva impostazioni su:"
                Label6.Text = "In modalità volatile, le impostazioni verranno ripristinate alla chiusura del programma"
                Label7.Text = "Modalità colore:"
                Label8.Text = "Lingua:"
                Label9.Text = "Specificare le impostazioni per la finestra di log:"
                Label10.Text = "Carattere della finestra di registro:"
                Label11.Text = "Anteprima:"
                Label12.Text = "File registro operazioni:"
                Label13.Text = "Quando si eseguono operazioni di immagine nella riga di comando, specificare l'argomento " & Quote & "/LogPath" & Quote & " per salvare il registro delle operazioni di immagine nel file di registro di destinazione"
                Label14.Text = "Livello del file di registro:"
                Label17.Text = "Schema di colore della barra di stato e dei pannelli:"
                Label18.Text = "Quando si eseguono tranquillamente le operazioni, il programma nasconde le informazioni e l'output di avanzamento. I messaggi di errore verranno comunque visualizzati." & CrLf & "Questa opzione non verrà utilizzata quando si ottengono informazioni, ad esempio, sui pacchetti o sulle funzioni." & CrLf & "Inoltre, quando si esegue la manutenzione delle immagini, il computer potrebbe riavviarsi automaticamente."
                Label19.Text = "Quando questa opzione è selezionata, il computer non si riavvia automaticamente, anche quando si eseguono tranquillamente delle operazioni"
                Label20.Text = "Specificare la cartella temporanea da utilizzare per le operazioni DISM:"
                Label21.Text = "Cartella temporanea:"
                Label22.Text = "Spazio rimanente nella cartella temporanea selezionata:"
                Label25.Text = "Visualizzazione del registro:"
                Label26.Text = "Esempio di rapporto:"
                Label27.Text = "Alcuni rapporti non possono essere visualizzati come tabella"
                Label28.Text = "Quando il programma dovrebbe notificare l'avvio dei processi in background?"
                Label29.Text = "Il programma utilizza i processi in background per raccogliere informazioni complete sull'immagine, come le date di modifica, i pacchetti installati, le funzioni presenti e altro ancora"
                Label30.Text = "Anteprime:"
                Label31.Text = "Inattivo"
                Label32.Text = "Esecuzione di operazioni"
                Label33.Text = "Pannello di avanzamento:"
                Label35.Text = "Modificare queste impostazioni solo se si verificano continui rallentamenti del programma o del sistema a causa dell'elevato utilizzo della CPU"
                Label36.Text = "Esaminare lo stato di questo processo in background:"
                Label37.Text = "Stato:"
                Label40.Text = "Le associazioni di file consentono di accedere direttamente ai file del progetto, senza dover prima caricare il programma"
                Label41.Text = "Stato dell'associazione:"
                Label42.Text = If(DetectFileAssociations(), "associazioni impostate", "associazioni non impostate")
                Label43.Text = "Impostare le opzioni che si desidera eseguire all'avvio del programma:"
                Label44.Text = "Il programma utilizzerà la cartella temporanea fornita dal progetto, se ne è stata caricata una. Se ci si trova nelle modalità di gestione dell'installazione online o offline, il programma utilizzerà la sua directory scratch"
                Label45.Text = "Stile del pannello di avanzamento secondario:"
                Label46.Text = "Queste impostazioni non sono applicabili alle installazioni non portatili"
                Label47.Text = "Questo carattere potrebbe non essere leggibile sulle finestre di registro. Anche se è possibile utilizzarlo, si consiglia di utilizzare caratteri monospaziati per una maggiore leggibilità."
                Label48.Text = "Scegliere le impostazioni che il programma deve considerare quando salva le informazioni sull'immagine:"
                Button1.Text = "Sfoglia..."
                Button2.Text = "Visualizza le versioni dei componenti DISM"
                Button3.Text = "Sfoglia..."
                Button4.Text = "Sfoglia..."
                Button9.Text = If(DetectFileAssociations(), "Rimuovi associazioni file", "Imposta associazioni file")
                Button10.Text = "Impostazioni avanzate"
                If MainForm.MountedImageDetectorBW.IsBusy Then Button8.Text = "Arresto" Else Button8.Text = "Avvio"
                Cancel_Button.Text = "Annullare"
                OK_Button.Text = "OK"
                PrefReset.Text = "Reimpostare le preferenze"
                CheckBox1.Text = "Modalità volatile"
                CheckBox2.Text = "Esegui silenziosamente le operazioni sull'immagine"
                CheckBox3.Text = "Salta il riavvio del sistema"
                CheckBox4.Text = "Utilizza una directory scratch"
                CheckBox5.Text = "Mostra l'output del comando in inglese"
                CheckBox6.Text = "Notifica l'avvio di processi in background"
                CheckBox7.Text = "Mostra la visualizzazione del registro nel pannello di avanzamento per impostazione predefinita"
                CheckBox8.Text = "Rileva sempre le immagini montate"
                CheckBox9.Text = "Utilizza i menu in maiuscolo"
                CheckBox10.Text = "Crea automaticamente i registri per ogni operazione eseguita"
                CheckBox11.Text = "Imposta icone di file personalizzate per i progetti DISMTools"
                CheckBox12.Text = "Rimonta le immagini montate che necessitano di un ricaricamento della sessione di assistenza"
                CheckBox13.Text = "Controlla gli aggiornamenti"
                CheckBox14.Text = "Salva sempre le informazioni complete per i seguenti elementi:"
                CheckBox15.Text = "Pacchetti installati"
                CheckBox16.Text = "Caratteristiche"
                CheckBox17.Text = "Pacchetti AppX installati"
                CheckBox18.Text = "Capacità"
                CheckBox19.Text = "Driver installati"
                CheckBox22.Text = "Pulisci automaticamente i punti di montaggio (lancia un processo separato)"
                DismOFD.Title = "Specificare l'eseguibile DISM da utilizzare"
                Label59.Text = "Personalizzazione dei registri"
                Label60.Text = "Impostare le opzioni che si desidera eseguire alla chiusura del programma:"
                Label61.Text = "Anteprima:"
                GroupBox5.Text = "Associazioni"
                Label9.Text = "Salvataggio delle informazioni sull'immagine"
                LinkLabel1.Text = "Il programma abilita o disabilita alcune funzioni in base alla versione di DISM supportata. Come influirà sull'uso di questo programma e quali funzioni saranno disabilitate di conseguenza?"
                LinkLabel1.LinkArea = New LinkArea(92, 100)
                LinkLabel2.Text = "Ulteriori informazioni sui processi in background"
                LogSFD.Title = "Specificare la posizione del file di log"
                RadioButton1.Text = "Schema di colore DISMTools 0.5 (verde)"
                RadioButton2.Text = "Schema di colori DISMTools 0.1.1-0.4.2 (blu)"
                RadioButton3.Text = "Utilizza la cartella temporanea del progetto o del programma"
                RadioButton4.Text = "Utilizza la cartella temporanea specificata"
                RadioButton5.Text = "Moderno"
                RadioButton6.Text = "Classic"
                ScratchFBD.Description = "Specifica la directory di scratch che il programma deve utilizzare:"
        End Select
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        SaveLocations(0) = "Settings file"
                        SaveLocations(1) = "Registry"
                        ColorModes(0) = "Use system setting"
                        ColorModes(1) = "Light mode"
                        ColorModes(2) = "Dark mode"
                        Languages(0) = "Use system language"
                        Languages(1) = "English"
                        Languages(2) = "Spanish"
                        Languages(3) = "French"
                        Languages(4) = "Portuguese"
                        Languages(5) = "Italian"
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
                        Languages(3) = "Francés"
                        Languages(4) = "Portugués"
                        Languages(5) = "Italiano"
                        LogViews(0) = "lista"
                        LogViews(1) = "tabla"
                        NotFreqs(0) = "Cada vez que un proyecto ha sido cargado satisfactoriamente"
                        NotFreqs(1) = "Una vez"
                    Case "FRA"
                        SaveLocations(0) = "Fichier des paramètres"
                        SaveLocations(1) = "Registre"
                        ColorModes(0) = "Utiliser les paramètres du système"
                        ColorModes(1) = "Mode lumineux"
                        ColorModes(2) = "Mode sombre"
                        Languages(0) = "Utiliser le langage du système"
                        Languages(1) = "Anglais"
                        Languages(2) = "Espagnol"
                        Languages(3) = "Français"
                        Languages(4) = "Portugais"
                        Languages(5) = "Italien"
                        LogViews(0) = "liste"
                        LogViews(1) = "tableau"
                        NotFreqs(0) = "Chaque fois qu'un projet a été chargé avec succès"
                        NotFreqs(1) = "Une fois"
                    Case "PTB", "PTG"
                        SaveLocations(0) = "Ficheiro de configurações"
                        SaveLocations(1) = "Registo"
                        ColorModes(0) = "Utilizar a configuração do sistema"
                        ColorModes(1) = "Modo claro"
                        ColorModes(2) = "Modo escuro"
                        Languages(0) = "Utilizar o idioma do sistema"
                        Languages(1) = "Inglês"
                        Languages(2) = "Espanhol"
                        Languages(3) = "Francês"
                        Languages(4) = "Português"
                        Languages(5) = "Italiano"
                        LogViews(0) = "lista"
                        LogViews(1) = "tabela"
                        NotFreqs(0) = "Sempre que um projeto tenha sido carregado com êxito"
                        NotFreqs(1) = "Uma vez"
                    Case "ITA"
                        SaveLocations(0) = "File delle impostazioni"
                        SaveLocations(1) = "Registro di sistema"
                        ColorModes(0) = "Usa le impostazioni di sistema"
                        ColorModes(1) = "Modalità chiara"
                        ColorModes(2) = "Modalità scura"
                        Languages(0) = "Utilizza la lingua di sistema"
                        Languages(1) = "Inglese"
                        Languages(2) = "Spagnolo"
                        Languages(3) = "Francese"
                        Languages(4) = "Portoghese"
                        Languages(5) = "Italiano"
                        LogViews(0) = "lista"
                        LogViews(1) = "tabella"
                        NotFreqs(0) = "Ogni volta che un progetto è stato caricato con successo"
                        NotFreqs(1) = "Una volta"
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
                Languages(3) = "French"
                Languages(4) = "Portuguese"
                Languages(5) = "Italian"
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
                Languages(3) = "Francés"
                Languages(4) = "Portugués"
                Languages(5) = "Italiano"
                LogViews(0) = "lista"
                LogViews(1) = "tabla"
                NotFreqs(0) = "Cada vez que un proyecto ha sido cargado satisfactoriamente"
                NotFreqs(1) = "Una vez"
            Case 3
                SaveLocations(0) = "Fichier des paramètres"
                SaveLocations(1) = "Registre"
                ColorModes(0) = "Utiliser les paramètres du système"
                ColorModes(1) = "Mode lumineux"
                ColorModes(2) = "Mode sombre"
                Languages(0) = "Utiliser le langage du système"
                Languages(1) = "Anglais"
                Languages(2) = "Espagnol"
                Languages(3) = "Français"
                Languages(4) = "Portugais"
                Languages(5) = "Italien"
                LogViews(0) = "liste"
                LogViews(1) = "tableau"
                NotFreqs(0) = "Chaque fois qu'un projet a été chargé avec succès"
                NotFreqs(1) = "Une fois"
            Case 4
                SaveLocations(0) = "Ficheiro de configurações"
                SaveLocations(1) = "Registo"
                ColorModes(0) = "Utilizar a configuração do sistema"
                ColorModes(1) = "Modo claro"
                ColorModes(2) = "Modo escuro"
                Languages(0) = "Utilizar o idioma do sistema"
                Languages(1) = "Inglês"
                Languages(2) = "Espanhol"
                Languages(3) = "Francês"
                Languages(4) = "Português"
                Languages(5) = "Italiano"
                LogViews(0) = "lista"
                LogViews(1) = "tabela"
                NotFreqs(0) = "Sempre que um projeto tenha sido carregado com êxito"
                NotFreqs(1) = "Uma vez"
            Case 5
                SaveLocations(0) = "File delle impostazioni"
                SaveLocations(1) = "Registro di sistema"
                ColorModes(0) = "Usa le impostazioni di sistema"
                ColorModes(1) = "Modalità chiara"
                ColorModes(2) = "Modalità scura"
                Languages(0) = "Utilizza la lingua di sistema"
                Languages(1) = "Inglese"
                Languages(2) = "Spagnolo"
                Languages(3) = "Francese"
                Languages(4) = "Portoghese"
                Languages(5) = "Italiano"
                LogViews(0) = "lista"
                LogViews(1) = "tabella"
                NotFreqs(0) = "Ogni volta che un progetto è stato caricato con successo"
                NotFreqs(1) = "Una volta"
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
            NumericUpDown1.BackColor = Color.FromArgb(31, 31, 31)
            NumericUpDown1.ForeColor = Color.White
            'GroupBox1.ForeColor = Color.White
            GroupBox5.ForeColor = Color.White
            'GroupBox6.ForeColor = Color.White
            TrackBar1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
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
            NumericUpDown1.BackColor = Color.FromArgb(238, 238, 242)
            NumericUpDown1.ForeColor = Color.Black
            GroupBox5.ForeColor = Color.Black
            TrackBar1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        PictureBox10.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.options_program_dark, My.Resources.options_program_light)
        PictureBox11.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.options_personalization_dark, My.Resources.options_personalization_light)
        PictureBox12.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.options_logs_dark, My.Resources.options_logs_light)
        PictureBox13.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.image_dark, My.Resources.image_light)
        PictureBox14.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.options_scratch_dark, My.Resources.options_scratch_light)
        PictureBox15.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.options_output_dark, My.Resources.options_output_light)
        PictureBox16.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.options_bgprocs_dark, My.Resources.options_bgprocs_light)
        PictureBox17.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.options_imgdetect_dark, My.Resources.options_imgdetect_light)
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "running", "stopped")
                        Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Stop", "Start")
                    Case "ESN"
                        Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "iniciado", "detenido")
                        Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Detener", "Iniciar")
                    Case "FRA"
                        Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "démarré", "arrêté")
                        Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Arrêter", "Démarrer")
                    Case "PTB", "PTG"
                        Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "funcionando", "parado")
                        Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Parar", "Iniciar")
                    Case "ITA"
                        Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "in esecuzione", "arrestato")
                        Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Arresto", "Avvio")
                End Select
            Case 1
                Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "running", "stopped")
                Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Stop", "Start")
            Case 2
                Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "iniciado", "detenido")
                Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Detener", "Iniciar")
            Case 3
                Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "démarré", "arrêté")
                Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Arrêter", "Démarrer")
            Case 4
                Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "funcionando", "parado")
                Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Parar", "Iniciar")
            Case 5
                Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "in esecuzione", "arrestato")
                Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Arresto", "Avvio")
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
        ChangeSections(SectionNum)
        FlowLayoutPanel1.BackColor = Win10Title.BackColor
    End Sub

    Sub GetSystemFonts()
        ComboBox4.Items.Clear()
        For Each fntFamily As FontFamily In FontFamily.Families
            ComboBox4.Items.Add(fntFamily.Name)
        Next
        If ComboBox4.SelectedItem = Nothing Then ComboBox4.SelectedItem = "Consolas"
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
                'RadioButton1.Checked = True
                'RadioButton2.Checked = False
            Case 1
                'RadioButton1.Checked = False
                'RadioButton2.Checked = True
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
        CheckBox14.Checked = MainForm.SkipQuestions
        CheckBox15.Checked = MainForm.AutoCompleteInfo(0)
        CheckBox16.Checked = MainForm.AutoCompleteInfo(1)
        CheckBox17.Checked = MainForm.AutoCompleteInfo(2)
        CheckBox18.Checked = MainForm.AutoCompleteInfo(3)
        CheckBox19.Checked = MainForm.AutoCompleteInfo(4)
        CheckBox22.Checked = MainForm.AutoCleanMounts
        Select Case MainForm.ColorSchemes
            Case 0
                RadioButton1.Checked = True
                RadioButton2.Checked = False
            Case 1
                RadioButton1.Checked = False
                RadioButton2.Checked = True
        End Select
        CheckBox7.Checked = MainForm.ExpandedProgressPanel
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
        Panel4.Visible = Not IsMonospacedFont(ComboBox4.Text)
    End Sub

    Function IsMonospacedFont(ftName As String) As Boolean
        Using testFont As Font = New Font(ftName, 10)
            Dim widthI As Decimal = MeasureCharacterWidth(testFont, "i")
            Dim widthW As Decimal = MeasureCharacterWidth(testFont, "w")
            Return widthI = widthW
        End Using
        Return False
    End Function

    Function MeasureCharacterWidth(ft As Font, character As Char) As Decimal
        Using bmp As Bitmap = New Bitmap(1, 1)
            Using g As Graphics = Graphics.FromImage(bmp)
                Dim size As SizeF = g.MeasureString(character.ToString(), ft)
                Return size.Width
            End Using
        End Using
        Return 0
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Not Directory.Exists(Path.Combine(Path.GetDirectoryName(TextBox1.Text), "dism")) Then
            Dim msg As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "The DISM components directory could not be found. If you have all components in the same folder of the DISM executable, please create a " & Quote & "dism" & Quote & " folder and try again."
                        Case "ESN"
                            msg = "La carpeta de componentes de DISM no pudo ser encontrada. Si tiene todos los componentes en la misma carpeta del ejecutable de DISM, cree una carpeta " & Quote & "dism" & Quote & " e inténtelo de nuevo."
                        Case "FRA"
                            msg = "Le répertoire des composants DISM n'a pas été trouvé. Si vous avez tous les composants dans le même dossier de l'exécutable DISM, veuillez créer un dossier " & Quote & "dism" & Quote & " et réessayer."
                        Case "PTB", "PTG"
                            msg = "Não foi possível encontrar o diretório de componentes do DISM. Se tiver todos os componentes na mesma pasta do executável DISM, crie uma pasta " & Quote & "dism" & Quote & " e tente novamente."
                        Case "ITA"
                            msg = "Non è stato possibile trovare la cartella dei componenti DISM. Se tutti i componenti si trovano nella stessa cartella dell'eseguibile DISM, creare una cartella " & Quote & "dism" & Quote & " e riprovare."
                    End Select
                Case 1
                    msg = "The DISM components directory could not be found. If you have all components in the same folder of the DISM executable, please create a " & Quote & "dism" & Quote & " folder and try again."
                Case 2
                    msg = "La carpeta de componentes de DISM no pudo ser encontrada. Si tiene todos los componentes en la misma carpeta del ejecutable de DISM, cree una carpeta " & Quote & "dism" & Quote & " e inténtelo de nuevo."
                Case 3
                    msg = "Le répertoire des composants DISM n'a pas été trouvé. Si vous avez tous les composants dans le même dossier de l'exécutable DISM, veuillez créer un dossier " & Quote & "dism" & Quote & " et réessayer."
                Case 4
                    msg = "Não foi possível encontrar o diretório de componentes do DISM. Se tiver todos os componentes na mesma pasta do executável DISM, crie uma pasta " & Quote & "dism" & Quote & " e tente novamente."
                Case 5
                    msg = "Non è stato possibile trovare la cartella dei componenti DISM. Se tutti i componenti si trovano nella stessa cartella dell'eseguibile DISM, creare una cartella " & Quote & "dism" & Quote & " e riprovare."
            End Select
            MsgBox(msg, vbOKOnly + vbExclamation, Label1.Text)
            Exit Sub
        End If
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
                    Case "ENU", "ENG"
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
                    Case "FRA"
                        Select Case TrackBar1.Value
                            Case 0
                                Label15.Text = "Erreurs (niveau du journal 1)"
                                Label16.Text = "Le fichier journal ne doit afficher les erreurs qu'après l'exécution d'une opération d'image."
                            Case 1
                                Label15.Text = "Erreurs et avertissements (niveau de journal 2)"
                                Label16.Text = "Le fichier journal doit afficher les erreurs et les avertissements après l'exécution d'une opération d'image."
                            Case 2
                                Label15.Text = "Erreurs, avertissements et messages d'information (niveau du journal 3)"
                                Label16.Text = "Le fichier journal doit afficher les erreurs, les avertissements et les messages d'information après l'exécution d'une opération d'image."
                            Case 3
                                Label15.Text = "Erreurs, avertissements, informations et messages de débogage (niveau du journal 4)"
                                Label16.Text = "Le fichier journal doit afficher les erreurs, les avertissements, les informations et les messages de débogage après l'exécution d'une opération d'image."
                        End Select
                    Case "PTB", "PTG"
                        Select Case TrackBar1.Value
                            Case 0
                                Label15.Text = "Erros (nível de registo 1)"
                                Label16.Text = "O ficheiro de registo só deve apresentar erros depois de executar uma operação de imagem."
                            Case 1
                                Label15.Text = "Erros e avisos (nível de registo 2)"
                                Label16.Text = "O ficheiro de registo deve apresentar erros e avisos após a realização de uma operação de imagem."
                            Case 2
                                Label15.Text = "Erros, avisos e mensagens de informação (nível de registo 3)"
                                Label16.Text = "O ficheiro de registo deve apresentar erros, avisos e mensagens de informação após a realização de uma operação de imagem."
                            Case 3
                                Label15.Text = "Erros, avisos, informações e mensagens de depuração (nível de registo 4)"
                                Label16.Text = "O ficheiro de registo deve apresentar erros, avisos, informações e mensagens de depuração após a realização de uma operação de imagem."
                        End Select
                    Case "ITA"
                        Select Case TrackBar1.Value
                            Case 0
                                Label15.Text = "Errori (livello di log 1)"
                                Label16.Text = "Il file di registro dovrebbe visualizzare gli errori solo dopo l'esecuzione di un'operazione di immagine"
                            Case 1
                                Label15.Text = "Errori e avvisi (livello di registro 2)"
                                Label16.Text = "Il file di log deve visualizzare errori e avvisi dopo l'esecuzione di un'operazione di immagine"
                            Case 2
                                Label15.Text = "Errori, avvisi e messaggi informativi (livello di registro 3)"
                                Label16.Text = "Il file di log deve visualizzare errori, avvisi e messaggi informativi dopo l'esecuzione di un'operazione di immagine."
                            Case 3
                                Label15.Text = "Errori, avvisi, informazioni e messaggi di debug (livello di registro 4)"
                                Label16.Text = "Il file di log deve visualizzare errori, avvisi, informazioni e messaggi di debug dopo l'esecuzione di un'operazione di immagine."
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
            Case 3
                Select Case TrackBar1.Value
                    Case 0
                        Label15.Text = "Erreurs (niveau du journal 1)"
                        Label16.Text = "Le fichier journal ne doit afficher les erreurs qu'après l'exécution d'une opération d'image."
                    Case 1
                        Label15.Text = "Erreurs et avertissements (niveau de journal 2)"
                        Label16.Text = "Le fichier journal doit afficher les erreurs et les avertissements après l'exécution d'une opération d'image."
                    Case 2
                        Label15.Text = "Erreurs, avertissements et messages d'information (niveau du journal 3)"
                        Label16.Text = "Le fichier journal doit afficher les erreurs, les avertissements et les messages d'information après l'exécution d'une opération d'image."
                    Case 3
                        Label15.Text = "Erreurs, avertissements, informations et messages de débogage (niveau du journal 4)"
                        Label16.Text = "Le fichier journal doit afficher les erreurs, les avertissements, les informations et les messages de débogage après l'exécution d'une opération d'image."
                End Select
            Case 4
                Select Case TrackBar1.Value
                    Case 0
                        Label15.Text = "Erros (nível de registo 1)"
                        Label16.Text = "O ficheiro de registo só deve apresentar erros depois de executar uma operação de imagem."
                    Case 1
                        Label15.Text = "Erros e avisos (nível de registo 2)"
                        Label16.Text = "O ficheiro de registo deve apresentar erros e avisos após a realização de uma operação de imagem."
                    Case 2
                        Label15.Text = "Erros, avisos e mensagens de informação (nível de registo 3)"
                        Label16.Text = "O ficheiro de registo deve apresentar erros, avisos e mensagens de informação após a realização de uma operação de imagem."
                    Case 3
                        Label15.Text = "Erros, avisos, informações e mensagens de depuração (nível de registo 4)"
                        Label16.Text = "O ficheiro de registo deve apresentar erros, avisos, informações e mensagens de depuração após a realização de uma operação de imagem."
                End Select
            Case 5
                Select Case TrackBar1.Value
                    Case 0
                        Label15.Text = "Errori (livello di log 1)"
                        Label16.Text = "Il file di registro dovrebbe visualizzare gli errori solo dopo l'esecuzione di un'operazione di immagine"
                    Case 1
                        Label15.Text = "Errori e avvisi (livello di registro 2)"
                        Label16.Text = "Il file di log deve visualizzare errori e avvisi dopo l'esecuzione di un'operazione di immagine"
                    Case 2
                        Label15.Text = "Errori, avvisi e messaggi informativi (livello di registro 3)"
                        Label16.Text = "Il file di log deve visualizzare errori, avvisi e messaggi informativi dopo l'esecuzione di un'operazione di immagine."
                    Case 3
                        Label15.Text = "Errori, avvisi, informazioni e messaggi di debug (livello di registro 4)"
                        Label16.Text = "Il file di log deve visualizzare errori, avvisi, informazioni e messaggi di debug dopo l'esecuzione di un'operazione di immagine."
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
                    Case "ENU", "ENG"
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
                                    Case 5 To 19.989999999999998
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
                                    Case 5 To 19.989999999999998
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
                    Case "FRA"
                        If SourceDir = "" Then
                            Label23.Text = "Veuillez indiquer un répertoire temporaire."
                            Label24.Visible = False
                            PictureBox5.Visible = False
                            PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                            Label24.Text = "Vous disposez de suffisamment d'espace dans le répertoire temporaire sélectionné."
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
                                        Label24.Text = "Vous ne disposez pas de suffisamment d'espace sur le répertoire temporaire sélectionné pour effectuer des opérations sur les images. Essayez de libérer de l'espace sur le disque"
                                    Case 5 To 19.989999999999998
                                        Label24.Visible = True
                                        PictureBox5.Visible = True
                                        PictureBox5.Image = New Bitmap(My.Resources.warning_16px)
                                        Label24.Text = "Il se peut que vous ne disposiez pas de suffisamment d'espace sur le répertoire temporaire sélectionné pour certaines opérations."
                                    Case Is >= 20
                                        Label24.Visible = False
                                        PictureBox5.Visible = False
                                        PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                                        Label24.Text = "Vous disposez de suffisamment d'espace dans le répertoire temporaire sélectionné."
                                End Select
                            Catch ex As Exception
                                Label23.Text = "Impossible d'obtenir l'espace libre disponible. Poursuivre à vos risques et périls"
                                Label24.Visible = False
                                PictureBox5.Visible = False
                                PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                                Label24.Text = "Vous disposez de suffisamment d'espace dans le répertoire temporaire sélectionné."
                                Exit Sub
                            End Try
                        End If
                    Case "PTB", "PTG"
                        If SourceDir = "" Then
                            Label23.Text = "Especifique um diretório temporário."
                            Label24.Visible = False
                            PictureBox5.Visible = False
                            PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                            Label24.Text = "Há espaço suficiente no diretório temporário selecionado"
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
                                        Label24.Text = "Não há espaço suficiente no diretório de rascunho selecionado para executar operações de imagem. Tente libertar algum espaço na unidade"
                                    Case 5 To 19.989999999999998
                                        Label24.Visible = True
                                        PictureBox5.Visible = True
                                        PictureBox5.Image = New Bitmap(My.Resources.warning_16px)
                                        Label24.Text = "Pode não haver espaço suficiente no diretório de rascunho selecionado para algumas operações."
                                    Case Is >= 20
                                        Label24.Visible = False
                                        PictureBox5.Visible = False
                                        PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                                        Label24.Text = "Há espaço suficiente no diretório temporário selecionado"
                                End Select
                            Catch ex As Exception
                                Label23.Text = "Não foi possível obter espaço livre disponível. Continue por sua conta e risco"
                                Label24.Visible = False
                                PictureBox5.Visible = False
                                PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                                Label24.Text = "Há espaço suficiente no diretório temporário selecionado"
                                Exit Sub
                            End Try
                        End If
                    Case "ITA"
                        If SourceDir = "" Then
                            Label23.Text = "Specificare una cartella temporanea"
                            Label24.Visible = False
                            PictureBox5.Visible = False
                            PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                            Label24.Text = "Lo spazio disponibile nella cartella temporanea selezionata è sufficiente"
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
                                        Label24.Text = "Nella cartella temporanea selezionata non c'è abbastanza spazio per eseguire operazioni sulle immagini. Provare a liberare spazio nell'unità"
                                    Case 5 To 19.989999999999998
                                        Label24.Visible = True
                                        PictureBox5.Visible = True
                                        PictureBox5.Image = New Bitmap(My.Resources.warning_16px)
                                        Label24.Text = "È possibile che la cartella temporanea selezionata non disponga di spazio sufficiente per alcune operazioni"
                                    Case Is >= 20
                                        Label24.Visible = False
                                        PictureBox5.Visible = False
                                        PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                                        Label24.Text = "Lo spazio disponibile nella cartella temporanea selezionata è sufficiente"
                                End Select
                            Catch ex As Exception
                                Label23.Text = "Impossibile ottenere spazio libero disponibile. Continuare a proprio rischio"
                                Label24.Visible = False
                                PictureBox5.Visible = False
                                PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                                Label24.Text = "Lo spazio disponibile nella directory temporanea selezionata è sufficiente"
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
                            Case 5 To 19.989999999999998
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
                            Case 5 To 19.989999999999998
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
            Case 3
                If SourceDir = "" Then
                    Label23.Text = "Veuillez indiquer un répertoire temporaire."
                    Label24.Visible = False
                    PictureBox5.Visible = False
                    PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                    Label24.Text = "Vous disposez de suffisamment d'espace dans le répertoire temporaire sélectionné."
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
                                Label24.Text = "Vous ne disposez pas de suffisamment d'espace sur le répertoire temporaire sélectionné pour effectuer des opérations sur les images. Essayez de libérer de l'espace sur le disque"
                            Case 5 To 19.989999999999998
                                Label24.Visible = True
                                PictureBox5.Visible = True
                                PictureBox5.Image = New Bitmap(My.Resources.warning_16px)
                                Label24.Text = "Il se peut que vous ne disposiez pas de suffisamment d'espace sur le répertoire temporaire sélectionné pour certaines opérations."
                            Case Is >= 20
                                Label24.Visible = False
                                PictureBox5.Visible = False
                                PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                                Label24.Text = "Vous disposez de suffisamment d'espace dans le répertoire temporaire sélectionné."
                        End Select
                    Catch ex As Exception
                        Label23.Text = "Impossible d'obtenir l'espace libre disponible. Poursuivre à vos risques et périls"
                        Label24.Visible = False
                        PictureBox5.Visible = False
                        PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                        Label24.Text = "Vous disposez de suffisamment d'espace dans le répertoire temporaire sélectionné."
                        Exit Sub
                    End Try
                End If
            Case 4
                If SourceDir = "" Then
                    Label23.Text = "Especifique um diretório temporário."
                    Label24.Visible = False
                    PictureBox5.Visible = False
                    PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                    Label24.Text = "Há espaço suficiente no diretório temporário selecionado"
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
                                Label24.Text = "Não há espaço suficiente no diretório de rascunho selecionado para executar operações de imagem. Tente libertar algum espaço na unidade"
                            Case 5 To 19.989999999999998
                                Label24.Visible = True
                                PictureBox5.Visible = True
                                PictureBox5.Image = New Bitmap(My.Resources.warning_16px)
                                Label24.Text = "Pode não haver espaço suficiente no diretório de rascunho selecionado para algumas operações."
                            Case Is >= 20
                                Label24.Visible = False
                                PictureBox5.Visible = False
                                PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                                Label24.Text = "Há espaço suficiente no diretório temporário selecionado"
                        End Select
                    Catch ex As Exception
                        Label23.Text = "Não foi possível obter espaço livre disponível. Continue por sua conta e risco"
                        Label24.Visible = False
                        PictureBox5.Visible = False
                        PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                        Label24.Text = "Há espaço suficiente no diretório temporário selecionado"
                        Exit Sub
                    End Try
                End If
            Case 5
                If SourceDir = "" Then
                    Label23.Text = "Specificare una cartella temporanea"
                    Label24.Visible = False
                    PictureBox5.Visible = False
                    PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                    Label24.Text = "Lo spazio disponibile nella cartella temporanea selezionata è sufficiente"
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
                                Label24.Text = "Nella cartella temporanea selezionata non c'è abbastanza spazio per eseguire operazioni sulle immagini. Provare a liberare spazio nell'unità"
                            Case 5 To 19.989999999999998
                                Label24.Visible = True
                                PictureBox5.Visible = True
                                PictureBox5.Image = New Bitmap(My.Resources.warning_16px)
                                Label24.Text = "È possibile che la cartella temporanea selezionata non disponga di spazio sufficiente per alcune operazioni"
                            Case Is >= 20
                                Label24.Visible = False
                                PictureBox5.Visible = False
                                PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                                Label24.Text = "Lo spazio disponibile nella cartella temporanea selezionata è sufficiente"
                        End Select
                    Catch ex As Exception
                        Label23.Text = "Impossibile ottenere spazio libero disponibile. Continuare a proprio rischio"
                        Label24.Visible = False
                        PictureBox5.Visible = False
                        PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                        Label24.Text = "Lo spazio disponibile nella directory temporanea selezionata è sufficiente"
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
                    Case "ENU", "ENG"
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
                    Case "FRA"
                        If Button8.Text = "Arrêter" Then
                            MainForm.MountedImageDetectorBW.CancelAsync()
                        ElseIf Button8.Text = "Démarrer" Then
                            Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
                        End If
                        Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "démarré", "arrêté")
                        Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Arrêter", "Démarrer")
                    Case "PTB", "PTG"
                        If Button8.Text = "Parar" Then
                            MainForm.MountedImageDetectorBW.CancelAsync()
                        ElseIf Button8.Text = "Iniciar" Then
                            Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
                        End If
                        Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "funcionando", "parado")
                        Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Parar", "Iniciar")
                    Case "ITA"
                        If Button8.Text = "Arresto" Then
                            MainForm.MountedImageDetectorBW.CancelAsync()
                        ElseIf Button8.Text = "Avvio" Then
                            Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
                        End If
                        Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "in esecuzione", "arrestato")
                        Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Arresto", "Avvio")
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
            Case 3
                If Button8.Text = "Arrêter" Then
                    MainForm.MountedImageDetectorBW.CancelAsync()
                ElseIf Button8.Text = "Démarrer" Then
                    Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
                End If
                Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "démarré", "arrêté")
                Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Arrêter", "Démarrer")
            Case 4
                If Button8.Text = "Parar" Then
                    MainForm.MountedImageDetectorBW.CancelAsync()
                ElseIf Button8.Text = "Iniciar" Then
                    Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
                End If
                Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "funcionando", "parado")
                Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Parar", "Iniciar")
            Case 5
                If Button8.Text = "Arresto" Then
                    MainForm.MountedImageDetectorBW.CancelAsync()
                ElseIf Button8.Text = "Avvio" Then
                    Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
                End If
                Label38.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "in esecuzione", "arrestato")
                Button8.Text = If(MainForm.MountedImageDetectorBW.IsBusy, "Arresto", "Avvio")
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
        Panel12.Enabled = CheckBox6.Checked
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

    Private Sub CheckBox14_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox14.CheckedChanged
        TableLayoutPanel2.Enabled = CheckBox14.Checked
    End Sub

#Region "Section functionality"

    Sub ChangeSections(Number As Integer)
        Select Case Number
            Case 0
                Options_Program.Visible = True
                Options_Personalization.Visible = False
                Options_Logs.Visible = False
                Options_ImgOps.Visible = False
                Options_Scratch.Visible = False
                Options_Output.Visible = False
                Options_BgProcs.Visible = False
                Options_ImgDetection.Visible = False
                Options_FileAssocs.Visible = False
                Options_Startup.Visible = False
                Options_Shutdown.Visible = False
                Label49.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                Label50.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label51.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label52.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label53.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label54.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label55.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label56.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label57.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label58.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label34.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                ProgramSectionBtn.BackColor = BackColor
                PersonalizationSectionBtn.BackColor = Win10Title.BackColor
                LogSectionBtn.BackColor = Win10Title.BackColor
                ImgOpsSectionBtn.BackColor = Win10Title.BackColor
                ScDirSectionBtn.BackColor = Win10Title.BackColor
                OutputSectionBtn.BackColor = Win10Title.BackColor
                BgProcsSectionBtn.BackColor = Win10Title.BackColor
                ImgDetectSectionBtn.BackColor = Win10Title.BackColor
                AssocsSectionBtn.BackColor = Win10Title.BackColor
                StartupSectionBtn.BackColor = Win10Title.BackColor
                ShutdownSectionBtn.BackColor = Win10Title.BackColor
            Case 1
                Options_Program.Visible = False
                Options_Personalization.Visible = True
                Options_Logs.Visible = False
                Options_ImgOps.Visible = False
                Options_Scratch.Visible = False
                Options_Output.Visible = False
                Options_BgProcs.Visible = False
                Options_ImgDetection.Visible = False
                Options_FileAssocs.Visible = False
                Options_Startup.Visible = False
                Options_Shutdown.Visible = False
                Label49.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label50.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                Label51.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label52.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label53.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label54.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label55.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label56.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label57.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label58.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label34.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                ProgramSectionBtn.BackColor = Win10Title.BackColor
                PersonalizationSectionBtn.BackColor = BackColor
                LogSectionBtn.BackColor = Win10Title.BackColor
                ImgOpsSectionBtn.BackColor = Win10Title.BackColor
                ScDirSectionBtn.BackColor = Win10Title.BackColor
                OutputSectionBtn.BackColor = Win10Title.BackColor
                BgProcsSectionBtn.BackColor = Win10Title.BackColor
                ImgDetectSectionBtn.BackColor = Win10Title.BackColor
                AssocsSectionBtn.BackColor = Win10Title.BackColor
                StartupSectionBtn.BackColor = Win10Title.BackColor
                ShutdownSectionBtn.BackColor = Win10Title.BackColor
            Case 2
                Options_Program.Visible = False
                Options_Personalization.Visible = False
                Options_Logs.Visible = True
                Options_ImgOps.Visible = False
                Options_Scratch.Visible = False
                Options_Output.Visible = False
                Options_BgProcs.Visible = False
                Options_ImgDetection.Visible = False
                Options_FileAssocs.Visible = False
                Options_Startup.Visible = False
                Options_Shutdown.Visible = False
                Label49.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label50.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label51.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                Label52.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label53.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label54.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label55.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label56.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label57.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label58.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label34.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                ProgramSectionBtn.BackColor = Win10Title.BackColor
                PersonalizationSectionBtn.BackColor = Win10Title.BackColor
                LogSectionBtn.BackColor = BackColor
                ImgOpsSectionBtn.BackColor = Win10Title.BackColor
                ScDirSectionBtn.BackColor = Win10Title.BackColor
                OutputSectionBtn.BackColor = Win10Title.BackColor
                BgProcsSectionBtn.BackColor = Win10Title.BackColor
                ImgDetectSectionBtn.BackColor = Win10Title.BackColor
                AssocsSectionBtn.BackColor = Win10Title.BackColor
                StartupSectionBtn.BackColor = Win10Title.BackColor
                ShutdownSectionBtn.BackColor = Win10Title.BackColor
            Case 3
                Options_Program.Visible = False
                Options_Personalization.Visible = False
                Options_Logs.Visible = False
                Options_ImgOps.Visible = True
                Options_Scratch.Visible = False
                Options_Output.Visible = False
                Options_BgProcs.Visible = False
                Options_ImgDetection.Visible = False
                Options_FileAssocs.Visible = False
                Options_Startup.Visible = False
                Options_Shutdown.Visible = False
                Label49.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label50.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label51.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label52.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                Label53.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label54.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label55.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label56.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label57.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label58.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label34.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                ProgramSectionBtn.BackColor = Win10Title.BackColor
                PersonalizationSectionBtn.BackColor = Win10Title.BackColor
                LogSectionBtn.BackColor = Win10Title.BackColor
                ImgOpsSectionBtn.BackColor = BackColor
                ScDirSectionBtn.BackColor = Win10Title.BackColor
                OutputSectionBtn.BackColor = Win10Title.BackColor
                BgProcsSectionBtn.BackColor = Win10Title.BackColor
                ImgDetectSectionBtn.BackColor = Win10Title.BackColor
                AssocsSectionBtn.BackColor = Win10Title.BackColor
                StartupSectionBtn.BackColor = Win10Title.BackColor
                ShutdownSectionBtn.BackColor = Win10Title.BackColor
            Case 4
                Options_Program.Visible = False
                Options_Personalization.Visible = False
                Options_Logs.Visible = False
                Options_ImgOps.Visible = False
                Options_Scratch.Visible = True
                Options_Output.Visible = False
                Options_BgProcs.Visible = False
                Options_ImgDetection.Visible = False
                Options_FileAssocs.Visible = False
                Options_Startup.Visible = False
                Options_Shutdown.Visible = False
                Label49.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label50.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label51.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label52.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label53.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                Label54.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label55.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label56.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label57.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label58.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label34.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                ProgramSectionBtn.BackColor = Win10Title.BackColor
                PersonalizationSectionBtn.BackColor = Win10Title.BackColor
                LogSectionBtn.BackColor = Win10Title.BackColor
                ImgOpsSectionBtn.BackColor = Win10Title.BackColor
                ScDirSectionBtn.BackColor = BackColor
                OutputSectionBtn.BackColor = Win10Title.BackColor
                BgProcsSectionBtn.BackColor = Win10Title.BackColor
                ImgDetectSectionBtn.BackColor = Win10Title.BackColor
                AssocsSectionBtn.BackColor = Win10Title.BackColor
                StartupSectionBtn.BackColor = Win10Title.BackColor
                ShutdownSectionBtn.BackColor = Win10Title.BackColor
            Case 5
                Options_Program.Visible = False
                Options_Personalization.Visible = False
                Options_Logs.Visible = False
                Options_ImgOps.Visible = False
                Options_Scratch.Visible = False
                Options_Output.Visible = True
                Options_BgProcs.Visible = False
                Options_ImgDetection.Visible = False
                Options_FileAssocs.Visible = False
                Options_Startup.Visible = False
                Options_Shutdown.Visible = False
                Label49.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label50.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label51.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label52.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label53.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label54.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                Label55.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label56.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label57.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label58.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label34.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                ProgramSectionBtn.BackColor = Win10Title.BackColor
                PersonalizationSectionBtn.BackColor = Win10Title.BackColor
                LogSectionBtn.BackColor = Win10Title.BackColor
                ImgOpsSectionBtn.BackColor = Win10Title.BackColor
                ScDirSectionBtn.BackColor = Win10Title.BackColor
                OutputSectionBtn.BackColor = BackColor
                BgProcsSectionBtn.BackColor = Win10Title.BackColor
                ImgDetectSectionBtn.BackColor = Win10Title.BackColor
                AssocsSectionBtn.BackColor = Win10Title.BackColor
                StartupSectionBtn.BackColor = Win10Title.BackColor
                ShutdownSectionBtn.BackColor = Win10Title.BackColor
            Case 6
                Options_Program.Visible = False
                Options_Personalization.Visible = False
                Options_Logs.Visible = False
                Options_ImgOps.Visible = False
                Options_Scratch.Visible = False
                Options_Output.Visible = False
                Options_BgProcs.Visible = True
                Options_ImgDetection.Visible = False
                Options_FileAssocs.Visible = False
                Options_Startup.Visible = False
                Options_Shutdown.Visible = False
                Label49.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label50.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label51.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label52.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label53.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label54.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label55.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                Label56.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label57.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label58.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label34.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                ProgramSectionBtn.BackColor = Win10Title.BackColor
                PersonalizationSectionBtn.BackColor = Win10Title.BackColor
                LogSectionBtn.BackColor = Win10Title.BackColor
                ImgOpsSectionBtn.BackColor = Win10Title.BackColor
                ScDirSectionBtn.BackColor = Win10Title.BackColor
                OutputSectionBtn.BackColor = Win10Title.BackColor
                BgProcsSectionBtn.BackColor = BackColor
                ImgDetectSectionBtn.BackColor = Win10Title.BackColor
                AssocsSectionBtn.BackColor = Win10Title.BackColor
                StartupSectionBtn.BackColor = Win10Title.BackColor
                ShutdownSectionBtn.BackColor = Win10Title.BackColor
            Case 7
                Options_Program.Visible = False
                Options_Personalization.Visible = False
                Options_Logs.Visible = False
                Options_ImgOps.Visible = False
                Options_Scratch.Visible = False
                Options_Output.Visible = False
                Options_BgProcs.Visible = False
                Options_ImgDetection.Visible = True
                Options_FileAssocs.Visible = False
                Options_Startup.Visible = False
                Options_Shutdown.Visible = False
                Label49.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label50.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label51.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label52.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label53.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label54.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label55.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label56.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                Label57.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label58.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label34.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                ProgramSectionBtn.BackColor = Win10Title.BackColor
                PersonalizationSectionBtn.BackColor = Win10Title.BackColor
                LogSectionBtn.BackColor = Win10Title.BackColor
                ImgOpsSectionBtn.BackColor = Win10Title.BackColor
                ScDirSectionBtn.BackColor = Win10Title.BackColor
                OutputSectionBtn.BackColor = Win10Title.BackColor
                BgProcsSectionBtn.BackColor = Win10Title.BackColor
                ImgDetectSectionBtn.BackColor = BackColor
                AssocsSectionBtn.BackColor = Win10Title.BackColor
                StartupSectionBtn.BackColor = Win10Title.BackColor
                ShutdownSectionBtn.BackColor = Win10Title.BackColor
            Case 8
                Options_Program.Visible = False
                Options_Personalization.Visible = False
                Options_Logs.Visible = False
                Options_ImgOps.Visible = False
                Options_Scratch.Visible = False
                Options_Output.Visible = False
                Options_BgProcs.Visible = False
                Options_ImgDetection.Visible = False
                Options_FileAssocs.Visible = True
                Options_Startup.Visible = False
                Options_Shutdown.Visible = False
                Label49.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label50.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label51.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label52.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label53.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label54.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label55.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label56.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label57.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                Label58.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label34.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                ProgramSectionBtn.BackColor = Win10Title.BackColor
                PersonalizationSectionBtn.BackColor = Win10Title.BackColor
                LogSectionBtn.BackColor = Win10Title.BackColor
                ImgOpsSectionBtn.BackColor = Win10Title.BackColor
                ScDirSectionBtn.BackColor = Win10Title.BackColor
                OutputSectionBtn.BackColor = Win10Title.BackColor
                BgProcsSectionBtn.BackColor = Win10Title.BackColor
                ImgDetectSectionBtn.BackColor = Win10Title.BackColor
                AssocsSectionBtn.BackColor = BackColor
                StartupSectionBtn.BackColor = Win10Title.BackColor
                ShutdownSectionBtn.BackColor = Win10Title.BackColor
            Case 9
                Options_Program.Visible = False
                Options_Personalization.Visible = False
                Options_Logs.Visible = False
                Options_ImgOps.Visible = False
                Options_Scratch.Visible = False
                Options_Output.Visible = False
                Options_BgProcs.Visible = False
                Options_ImgDetection.Visible = False
                Options_FileAssocs.Visible = False
                Options_Startup.Visible = True
                Options_Shutdown.Visible = False
                Label49.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label50.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label51.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label52.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label53.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label54.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label55.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label56.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label57.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label58.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                Label34.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                ProgramSectionBtn.BackColor = Win10Title.BackColor
                PersonalizationSectionBtn.BackColor = Win10Title.BackColor
                LogSectionBtn.BackColor = Win10Title.BackColor
                ImgOpsSectionBtn.BackColor = Win10Title.BackColor
                ScDirSectionBtn.BackColor = Win10Title.BackColor
                OutputSectionBtn.BackColor = Win10Title.BackColor
                BgProcsSectionBtn.BackColor = Win10Title.BackColor
                ImgDetectSectionBtn.BackColor = Win10Title.BackColor
                AssocsSectionBtn.BackColor = Win10Title.BackColor
                StartupSectionBtn.BackColor = BackColor
                ShutdownSectionBtn.BackColor = Win10Title.BackColor
            Case 10
                Options_Program.Visible = False
                Options_Personalization.Visible = False
                Options_Logs.Visible = False
                Options_ImgOps.Visible = False
                Options_Scratch.Visible = False
                Options_Output.Visible = False
                Options_BgProcs.Visible = False
                Options_ImgDetection.Visible = False
                Options_FileAssocs.Visible = False
                Options_Startup.Visible = False
                Options_Shutdown.Visible = True
                Label49.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label50.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label51.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label52.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label53.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label54.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label55.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label56.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label57.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label58.Font = New Font("Segoe UI", 9, FontStyle.Regular)
                Label34.Font = New Font("Segoe UI", 9, FontStyle.Bold)
                ProgramSectionBtn.BackColor = Win10Title.BackColor
                PersonalizationSectionBtn.BackColor = Win10Title.BackColor
                LogSectionBtn.BackColor = Win10Title.BackColor
                ImgOpsSectionBtn.BackColor = Win10Title.BackColor
                ScDirSectionBtn.BackColor = Win10Title.BackColor
                OutputSectionBtn.BackColor = Win10Title.BackColor
                BgProcsSectionBtn.BackColor = Win10Title.BackColor
                ImgDetectSectionBtn.BackColor = Win10Title.BackColor
                AssocsSectionBtn.BackColor = Win10Title.BackColor
                StartupSectionBtn.BackColor = Win10Title.BackColor
                ShutdownSectionBtn.BackColor = BackColor
        End Select
        SectionNum = Number
    End Sub

    Private Sub ProgramSectionBtn_Click(sender As Object, e As EventArgs) Handles ProgramSectionBtn.Click, Label49.Click, PictureBox10.Click
        ChangeSections(0)
    End Sub

    Private Sub PersonalizationSectionBtn_Click(sender As Object, e As EventArgs) Handles PersonalizationSectionBtn.Click, Label50.Click, PictureBox11.Click
        ChangeSections(1)
    End Sub

    Private Sub LogSectionBtn_Click(sender As Object, e As EventArgs) Handles LogSectionBtn.Click, Label51.Click, PictureBox12.Click
        ChangeSections(2)
    End Sub

    Private Sub ImgOpsSectionBtn_Click(sender As Object, e As EventArgs) Handles ImgOpsSectionBtn.Click, Label52.Click, PictureBox13.Click
        ChangeSections(3)
    End Sub

    Private Sub ScDirSectionBtn_Click(sender As Object, e As EventArgs) Handles ScDirSectionBtn.Click, Label53.Click, PictureBox14.Click
        ChangeSections(4)
    End Sub

    Private Sub OutputSectionBtn_Click(sender As Object, e As EventArgs) Handles OutputSectionBtn.Click, Label54.Click, PictureBox15.Click
        ChangeSections(5)
    End Sub

    Private Sub BgProcsSectionBtn_Click(sender As Object, e As EventArgs) Handles BgProcsSectionBtn.Click, Label55.Click, PictureBox16.Click
        ChangeSections(6)
    End Sub

    Private Sub ImgDetectSectionBtn_Click(sender As Object, e As EventArgs) Handles ImgDetectSectionBtn.Click, Label56.Click, PictureBox17.Click
        ChangeSections(7)
    End Sub

    Private Sub AssocsSectionBtn_Click(sender As Object, e As EventArgs) Handles AssocsSectionBtn.Click, Label57.Click, PictureBox18.Click
        ChangeSections(8)
    End Sub

    Private Sub StartupSectionBtn_Click(sender As Object, e As EventArgs) Handles StartupSectionBtn.Click, Label58.Click, PictureBox19.Click
        ChangeSections(9)
    End Sub

    Private Sub ShutdownSectionBtn_Click(sender As Object, e As EventArgs) Handles ShutdownSectionBtn.Click, Label34.Click, PictureBox20.Click
        ChangeSections(10)
    End Sub

#End Region

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            StatusBar_Idle.Image = My.Resources.CS_Idle_Green
            StatusBar_Ops.Image = My.Resources.CS_Ops_Green
            ProgressPanel_Ops.Image = My.Resources.CS_ProgressPanel_Green
        Else
            StatusBar_Idle.Image = My.Resources.CS_Idle_Blue
            StatusBar_Ops.Image = My.Resources.CS_Ops_Blue
            ProgressPanel_Ops.Image = My.Resources.CS_ProgressPanel_Blue
        End If
    End Sub

    Private Sub CheckBox7_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox7.CheckedChanged
        ProgressPanelPic.Image = If(CheckBox7.Checked, My.Resources.progresspanel_logview_shown, My.Resources.progresspanel_logview_hidden)
    End Sub
End Class
