Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports DISMTools.Elements
Imports System.Xml
Imports System.Xml.Serialization

Public Class AddProvAppxPackage

    ' Variables used by the AppX scanner component
    Dim AppxNameList As New List(Of String)
    Dim AppxPublisherList As New List(Of String)
    Dim AppxVersionList As New List(Of String)
    Public AppxNames(65535) As String
    Public AppxPublishers(65535) As String
    Public AppxVersion(65535) As String

    ' Variables passed to ProgressPanel
    Public AppxPackages(65535) As String
    Public AppxDependencies(65535) As String

    ' Internal variables helpful to pass information
    Public AppxAdditionCount As Integer
    Public AppxDependencyCount As Integer

    Dim LogoAssetPopupForm As New Form()
    Dim LogoAssetPreview As New PictureBox()
    Dim previewer As New ToolTip()

    Dim Packages As New List(Of AppxPackage)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        AppxAdditionCount = ListView1.Items.Count
        AppxDependencyCount = ListBox1.Items.Count
        ProgressPanel.appxAdditionCount = AppxAdditionCount
        If ListView1.Items.Count = 0 Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            MsgBox("Please specify packed or unpacked AppX packages and try again.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                        Case "ESN"
                            MsgBox("Especifique archivos AppX empaquetados o desempaquetados e inténtelo de nuevo.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                    End Select
                Case 1
                    MsgBox("Please specify packed or unpacked AppX packages and try again.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                Case 2
                    MsgBox("Especifique archivos AppX empaquetados o desempaquetados e inténtelo de nuevo.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
            End Select
            Exit Sub
        Else
            If AppxAdditionCount > 65535 Then
                MsgBox("Right now, you can only specify less than 65535 AppX packages. This is a program limitation that will be gone in a future update.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                Exit Sub
            Else
                For x = 0 To AppxAdditionCount - 1
                    AppxPackages(x) = ListView1.Items(x).Text
                Next
                For x = 0 To AppxDependencyCount - 1
                    AppxDependencies(x) = ListBox1.Items(x).ToString()
                Next
                ' Fill in remote arrays, even empty slots
                For x = 0 To AppxPackages.Length - 1
                    ProgressPanel.appxAdditionPackages(x) = AppxPackages(x)
                Next
                For x = 0 To AppxDependencies.Length - 1
                    ProgressPanel.appxAdditionDependencies(x) = AppxDependencies(x)
                Next
                ProgressPanel.appxAdditionLastPackage = ListView1.Items(AppxAdditionCount - 1).ToString().Replace("ListViewItem: {", "").Trim().Replace("}", "").Trim()
                If AppxDependencyCount > 0 Then
                    ProgressPanel.appxAdditionLastDependency = ListBox1.Items(AppxDependencyCount - 1).ToString()
                Else
                    ProgressPanel.appxAdditionLastDependency = ""
                End If
                If CheckBox3.Checked Then
                    If TextBox1.Text = "" Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        MsgBox("Please specify a license file and try again. You can also continue without one, but this may compromise the image.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                                    Case "ESN"
                                        MsgBox("Especifique un archivo de licencia e inténtelo de nuevo. También puede continuar sin uno, pero esta acción podría comprometer la imagen.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                                End Select
                            Case 1
                                MsgBox("Please specify a license file and try again. You can also continue without one, but this may compromise the image.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                            Case 2
                                MsgBox("Especifique un archivo de licencia e inténtelo de nuevo. También puede continuar sin uno, pero esta acción podría comprometer la imagen.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                        End Select
                        Exit Sub
                    ElseIf Not File.Exists(TextBox1.Text) Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        MsgBox("The license file specified was not found. Make sure it exists on the specified location and try again.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                                    Case "ESN"
                                        MsgBox("El archivo de licencia especificado no se ha encontrado. Asegúrese de que exista en la ubicación especificada e inténtelo de nuevo.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                                End Select
                            Case 1
                                MsgBox("The license file specified was not found. Make sure it exists on the specified location and try again.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                            Case 2
                                MsgBox("El archivo de licencia especificado no se ha encontrado. Asegúrese de que exista en la ubicación especificada e inténtelo de nuevo.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                        End Select
                        Exit Sub
                    Else
                        ProgressPanel.appxAdditionUseLicenseFile = True
                        ProgressPanel.appxAdditionLicenseFile = TextBox1.Text
                    End If
                Else
                    ProgressPanel.appxAdditionUseLicenseFile = False
                    ProgressPanel.appxAdditionLicenseFile = ""
                End If
                If CheckBox1.Checked Then
                    If TextBox2.Text = "" Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        MsgBox("Please specify a custom data file and try again. You can also continue without one.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                                    Case "ESN"
                                        MsgBox("Especifique un archivo de datos personalizados e inténtelo de nuevo. También puede continuar sin uno", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                                End Select
                            Case 1
                                MsgBox("Please specify a custom data file and try again. You can also continue without one.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                            Case 2
                                MsgBox("Especifique un archivo de datos personalizados e inténtelo de nuevo. También puede continuar sin uno", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                        End Select
                        Exit Sub
                    ElseIf Not File.Exists(TextBox2.Text) Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        MsgBox("The custom data file specified was not found. Make sure it exists on the specified location and try again.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                                    Case "ESN"
                                        MsgBox("El archivo de datos personalizados especificado no se ha encontrado. Asegúrese de que exista en la ubicación especificada e inténtelo de nuevo.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                                End Select
                            Case 1
                                MsgBox("The custom data file specified was not found. Make sure it exists on the specified location and try again.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                            Case 2
                                MsgBox("El archivo de datos personalizados especificado no se ha encontrado. Asegúrese de que exista en la ubicación especificada e inténtelo de nuevo.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                        End Select
                        Exit Sub
                    Else
                        ProgressPanel.appxAdditionUseCustomDataFile = True
                        ProgressPanel.appxAdditionCustomDataFile = TextBox2.Text
                    End If
                Else
                    ProgressPanel.appxAdditionUseCustomDataFile = False
                    ProgressPanel.appxAdditionCustomDataFile = ""
                End If
                If CheckBox4.Checked Then
                    ProgressPanel.appxAdditionUseAllRegions = True
                    ProgressPanel.appxAdditionRegions = "all"
                Else
                    ProgressPanel.appxAdditionUseAllRegions = False
                    ProgressPanel.appxAdditionRegions = TextBox3.Text
                End If
                If CheckBox2.Checked And Not MainForm.OnlineManagement Then
                    ProgressPanel.appxAdditionCommit = True
                Else
                    ProgressPanel.appxAdditionCommit = False
                End If
            End If
            ProgressPanel.appxAdditionPackageList = Packages
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 37
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub AddProvAppxPackage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Add provisioned AppX packages"
                        Label1.Text = Text
                        Label2.Text = "Please add packed or unpacked AppX packages by using the buttons below, or by dropping them to the list view below:"
                        Label3.Text = "An AppX package may need some dependencies for it to be installed correctly. If so, you can specify a list of dependencies now:"
                        Label5.Text = "To specify multiple app regions, separate them with a semicolon (;)"
                        Label6.Text = "Select an entry in the list view to show the details of an app and to configure addition settings"
                        Button1.Text = "Add file"
                        Button2.Text = "Add folder"
                        Button3.Text = "Remove all entries"
                        Button4.Text = "Remove all dependencies"
                        Button5.Text = "Remove dependency"
                        Button6.Text = "Add dependency..."
                        Button7.Text = "Browse..."
                        Button8.Text = "Browse..."
                        Button9.Text = "Remove selected entry"
                        Cancel_Button.Text = "Cancel"
                        OK_Button.Text = "OK"
                        CheckBox1.Text = "Custom data file:"
                        CheckBox2.Text = "Commit image after adding AppX packages"
                        CustomDataFileOFD.Title = "Specify a custom data file"
                        GroupBox2.Text = "AppX dependencies"
                        GroupBox3.Text = "AppX regions"
                        LicenseFileOFD.Title = "Specify a license file"
                        LinkLabel1.Text = "App regions need to be in the form of ISO 3166-1 Alpha 2 or Alpha-3 codes. To learn more about these codes, click here"
                        LinkLabel1.LinkArea = New LinkArea(108, 10)
                        ListView1.Columns(0).Text = "File/Folder"
                        ListView1.Columns(1).Text = "Type"
                        ListView1.Columns(2).Text = "Application name"
                        ListView1.Columns(3).Text = "Application publisher"
                        ListView1.Columns(4).Text = "Application version"
                        CheckBox3.Text = "License file:"
                        CheckBox4.Text = "Make app available for all regions"
                        UnpackedAppxFolderFBD.Description = "Please specify a folder containing unpacked AppX files:"
                    Case "ESN"
                        Text = "Añadir paquetes aprovisionados AppX"
                        Label1.Text = Text
                        Label2.Text = "Añada archivos AppX empaquetados o desempaquetados usando los botones de abajo, o soltándolos en la lista de abajo:"
                        Label3.Text = "Un paquete AppX podría necesitar algunas dependencias para que sea instalado correctamente. Si es así, puede especificarlas ahora:"
                        Label5.Text = "Para especificar regiones de aplicación múltiples, sepáralos con un punto y coma (;)"
                        Label6.Text = "Seleccione una entrada en la lista para mostrar los detalles de una aplicación y para configurar opciones de adición"
                        Button1.Text = "Añadir archivo"
                        Button2.Text = "Añadir carpeta"
                        Button3.Text = "Eliminar todas las entradas"
                        Button4.Text = "Eliminar todas las dependencias"
                        Button5.Text = "Eliminar dependencia"
                        Button6.Text = "Añadir dependencia..."
                        Button7.Text = "Examinar..."
                        Button8.Text = "Examinar..."
                        Button9.Text = "Eliminar entrada seleccionada"
                        Cancel_Button.Text = "Cancelar"
                        OK_Button.Text = "Aceptar"
                        CheckBox1.Text = "Archivo de datos:"
                        CheckBox2.Text = "Guardar imagen tras añadir paquetes AppX"
                        CustomDataFileOFD.Title = "Especificar un archivo de datos personalizados"
                        GroupBox2.Text = "Dependencias de aplicaciones"
                        GroupBox3.Text = "Regiones de aplicaciones"
                        LicenseFileOFD.Title = "Especificar un archivo de licencia"
                        LinkLabel1.Text = "Las regiones de aplicaciones deben estar en el formato de códigos ISO 3166-1 Alpha 2 o Alpha 3. Saber más acerca de estos códigos"
                        LinkLabel1.LinkArea = New LinkArea(96, 33)
                        ListView1.Columns(0).Text = "Archivo/Carpeta"
                        ListView1.Columns(1).Text = "Tipo"
                        ListView1.Columns(2).Text = "Nombre de aplicación"
                        ListView1.Columns(3).Text = "Publicador de aplicación"
                        ListView1.Columns(4).Text = "Versión de aplicación"
                        CheckBox3.Text = "Archivo de licencia:"
                        CheckBox4.Text = "Hacer aplicación disponible para todas las regiones"
                        UnpackedAppxFolderFBD.Description = "Especifique un directorio contenedor de archivos de una aplicación AppX:"
                End Select
            Case 1
                Text = "Add provisioned AppX packages"
                Label1.Text = Text
                Label2.Text = "Please add packed or unpacked AppX packages by using the buttons below, or by dropping them to the list view below:"
                Label3.Text = "An AppX package may need some dependencies for it to be installed correctly. If so, you can specify a list of dependencies now:"
                Label5.Text = "To specify multiple app regions, separate them with a semicolon (;)"
                Label6.Text = "Select an entry in the list view to show the details of an app and to configure addition settings"
                Button1.Text = "Add file"
                Button2.Text = "Add folder"
                Button3.Text = "Remove all entries"
                Button4.Text = "Remove all dependencies"
                Button5.Text = "Remove dependency"
                Button6.Text = "Add dependency..."
                Button7.Text = "Browse..."
                Button8.Text = "Browse..."
                Button9.Text = "Remove selected entry"
                Cancel_Button.Text = "Cancel"
                OK_Button.Text = "OK"
                CheckBox1.Text = "Custom data file:"
                CheckBox2.Text = "Commit image after adding AppX packages"
                CustomDataFileOFD.Title = "Specify a custom data file"
                GroupBox2.Text = "AppX dependencies"
                GroupBox3.Text = "AppX regions"
                LicenseFileOFD.Title = "Specify a license file"
                LinkLabel1.Text = "App regions need to be in the form of ISO 3166-1 Alpha 2 or Alpha-3 codes. To learn more about these codes, click here"
                LinkLabel1.LinkArea = New LinkArea(108, 10)
                ListView1.Columns(0).Text = "File/Folder"
                ListView1.Columns(1).Text = "Type"
                ListView1.Columns(2).Text = "Application name"
                ListView1.Columns(3).Text = "Application publisher"
                ListView1.Columns(4).Text = "Application version"
                CheckBox3.Text = "License file:"
                CheckBox4.Text = "Make app available for all regions"
                UnpackedAppxFolderFBD.Description = "Please specify a folder containing unpacked AppX files:"
            Case 2
                Text = "Añadir paquetes aprovisionados AppX"
                Label1.Text = Text
                Label2.Text = "Añada archivos AppX empaquetados o desempaquetados usando los botones de abajo, o soltándolos en la lista de abajo:"
                Label3.Text = "Un paquete AppX podría necesitar algunas dependencias para que sea instalado correctamente. Si es así, puede especificarlas ahora:"
                Label5.Text = "Para especificar regiones de aplicación múltiples, sepáralos con un punto y coma (;)"
                Label6.Text = "Seleccione una entrada en la lista para mostrar los detalles de una aplicación y para configurar opciones de adición"
                Button1.Text = "Añadir archivo"
                Button2.Text = "Añadir carpeta"
                Button3.Text = "Eliminar todas las entradas"
                Button4.Text = "Eliminar todas las dependencias"
                Button5.Text = "Eliminar dependencia"
                Button6.Text = "Añadir dependencia..."
                Button7.Text = "Examinar..."
                Button8.Text = "Examinar..."
                Button9.Text = "Eliminar entrada seleccionada"
                Cancel_Button.Text = "Cancelar"
                OK_Button.Text = "Aceptar"
                CheckBox1.Text = "Archivo de datos:"
                CheckBox2.Text = "Guardar imagen tras añadir paquetes AppX"
                CustomDataFileOFD.Title = "Especificar un archivo de datos personalizados"
                GroupBox2.Text = "Dependencias de aplicaciones"
                GroupBox3.Text = "Regiones de aplicaciones"
                LicenseFileOFD.Title = "Especificar un archivo de licencia"
                LinkLabel1.Text = "Las regiones de aplicaciones deben estar en el formato de códigos ISO 3166-1 Alpha 2 o Alpha 3. Saber más acerca de estos códigos"
                LinkLabel1.LinkArea = New LinkArea(96, 33)
                ListView1.Columns(0).Text = "Archivo/Carpeta"
                ListView1.Columns(1).Text = "Tipo"
                ListView1.Columns(2).Text = "Nombre de aplicación"
                ListView1.Columns(3).Text = "Publicador de aplicación"
                ListView1.Columns(4).Text = "Versión de aplicación"
                CheckBox3.Text = "Archivo de licencia:"
                CheckBox4.Text = "Hacer aplicación disponible para todas las regiones"
                UnpackedAppxFolderFBD.Description = "Especifique un directorio contenedor de archivos de una aplicación AppX:"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
            GroupBox3.ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
            ListBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            TextBox3.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
            GroupBox3.ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
            ListBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            TextBox3.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListView1.ForeColor = ForeColor
        ListBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        CheckBox2.Enabled = MainForm.OnlineManagement = False
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        AppxDetailsPanel.Height = If(ListView1.SelectedItems.Count <= 0, 520, 83)
        GroupBox3.Enabled = If(FileVersionInfo.GetVersionInfo(MainForm.DismExe).ProductMajorPart < 10, False, True)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AppxFileOFD.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If UnpackedAppxFolderFBD.ShowDialog = Windows.Forms.DialogResult.OK And UnpackedAppxFolderFBD.SelectedPath <> "" Then
            ScanAppxPackage(True, UnpackedAppxFolderFBD.SelectedPath)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Packages.Clear()
        ListView1.Items.Clear()
        Button3.Enabled = False
        Button9.Enabled = False
        NoAppxFilePanel.Visible = True
        AppxFilePanel.Visible = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ListBox1.Items.Clear()
        Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Clear()
        Button4.Enabled = False
        Button5.Enabled = False
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If Not ListBox1.SelectedItem = "" Then
            'Dim dep As New AppxDependency()
            'dep.DependencyFile.Add(ListBox1.SelectedItem)
            Dim deps As New List(Of AppxDependency)
            deps = Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
            deps.RemoveAt(ListBox1.SelectedIndex)
            Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies = deps
            ListBox1.Items.Remove(ListBox1.SelectedItem)
        End If
        If ListBox1.SelectedItem = "" Then
            Button5.Enabled = False
        Else
            Button5.Enabled = True
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        AppxDependencyOFD.ShowDialog()
    End Sub

    Private Sub AppxFileOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles AppxFileOFD.FileOk
        ScanAppxPackage(False, AppxFileOFD.FileName)
    End Sub

    Private Sub AppxDependencyOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles AppxDependencyOFD.FileOk
        If ListView1.SelectedItems.Count = 1 Then
            Dim dep As New AppxDependency()
            dep.DependencyFile = AppxDependencyOFD.FileName
            If Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Count > 0 And Not Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Contains(dep) Then
                Dim deps As New List(Of AppxDependency)
                deps = Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
                deps.Add(dep)
                Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies = deps
            ElseIf Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Count = 0 Then
                Dim deps As New List(Of AppxDependency)
                deps = Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
                deps.Add(dep)
                Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies = deps
            End If
            ListBox1.Items.Add(AppxDependencyOFD.FileName)
            If ListBox1.Items.Count > 0 Then
                Button4.Enabled = True
            End If
        End If
    End Sub

    Private Sub LicenseFileOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles LicenseFileOFD.FileOk
        TextBox1.Text = LicenseFileOFD.FileName
    End Sub

    Private Sub CustomDataFileOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CustomDataFileOFD.FileOk
        TextBox2.Text = CustomDataFileOFD.FileName
    End Sub

    ''' <summary>
    ''' DISMTools AppX header scanner component: version 0.3.1
    ''' </summary>
    ''' <param name="IsFolder">Determines whether the given value for "Package" is a folder</param>
    ''' <param name="Package">The name of the packed or unpacked AppX file. It may be a file containing the full structure, or a folder containing all AppX files</param>
    ''' <remarks>Scans the header of AppX packages to gather application name, publisher, and version information</remarks>
    Sub ScanAppxPackage(IsFolder As Boolean, Package As String)
        ' Detect if the package specified is encrypted
        If Path.GetExtension(Package).Replace(".", "").Trim().StartsWith("e", StringComparison.OrdinalIgnoreCase) Then
            Dim msg As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            msg = "The package:" & CrLf & CrLf & Package & CrLf & CrLf & "is an encrypted application package. Neither DISMTools nor DISM support adding these application types. If you'd like to add it, you can do so, after the image is applied and booted to."
                        Case "ESN"
                            msg = "El paquete:" & CrLf & CrLf & Package & CrLf & CrLf & "es un paquete de aplicación encriptado. Ni DISMTools ni DISM soportan añadir estos tipos de aplicaciones. Si le gustaría añadirlo, puede hacerlo, después de que la imagen se haya aplicado e iniciado."
                    End Select
                Case 1
                    msg = "The package:" & CrLf & CrLf & Package & CrLf & CrLf & "is an encrypted application package. Neither DISMTools nor DISM support adding these application types. If you'd like to add it, you can do so, after the image is applied and booted to."
                Case 2
                    msg = "El paquete:" & CrLf & CrLf & Package & CrLf & CrLf & "es un paquete de aplicación encriptado. Ni DISMTools ni DISM soportan añadir estos tipos de aplicaciones. Si le gustaría añadirlo, puede hacerlo, después de que la imagen se haya aplicado e iniciado."
            End Select
            MsgBox(msg, vbOKOnly + vbExclamation, Label1.Text)
            Exit Sub
        End If
        Dim Stepper As Integer = 2
        Dim QuoteCount As Integer = 0
        Dim ScannerRTB As New RichTextBox()
        Dim currentAppxName As String = ""
        Dim currentAppxPublisher As String = ""
        Dim currentAppxVersion As String = ""
        Dim currentAppxArchitecture As String = ""
        Dim pkgName As String = ""
        If IsFolder Then
            If File.Exists(Package & "\AppxMetadata\AppxBundleManifest.xml") Then
                ' AppXBundle file
                ScannerRTB.Text = My.Computer.FileSystem.ReadAllText(Package & "\AppxMetadata\AppxBundleManifest.xml")
                Dim IdScanner As String = ScannerRTB.Lines(If(ScannerRTB.Lines(2).EndsWith("<!--"), 10, 4))
                Dim CharIndex As Integer = 0
                Dim CharNext As Integer
                For Each Character As Char In ScannerRTB.Lines(If(ScannerRTB.Lines(2).EndsWith("<!--"), 10, 4))
                    CharNext = CharIndex + 1
                    If Not IdScanner(CharIndex) = Quote Then
                        CharIndex += 1
                        Continue For
                    ElseIf IdScanner(CharIndex) = Quote And IdScanner(CharNext) = " " Then
                        CharIndex += 1
                        Continue For
                    Else
                        Character = IdScanner(CharIndex + 1)
                        If Not IdScanner(CharIndex + Stepper) = " " Then
                            If QuoteCount = 3 Then
                                QuoteCount += 1
                                Do
                                    If Character = Quote Then
                                        CharIndex += Stepper - 1
                                        Character = IdScanner(CharIndex - 1)
                                        QuoteCount += 1
                                        Stepper = 2
                                        Exit For
                                    Else
                                        pkgName &= Character.ToString()
                                        Character = IdScanner(CharIndex + Stepper)
                                        Stepper += 1
                                    End If
                                Loop
                            Else
                                QuoteCount += 1
                                CharIndex += Stepper - 1
                                Character = IdScanner(CharIndex + Stepper)
                            End If
                        End If
                    End If
                Next
                pkgName = pkgName.Replace(" ", "%20").Trim()
                QuoteCount = 0
                Stepper = 2
                If ScannerRTB.Lines(2).EndsWith("<!--") Then
                    ' XML comment
                    IdScanner = ScannerRTB.Lines(9)
                    Dim serializer As New XmlSerializer(GetType(AppxPackage))
                    Using tReader As TextReader = New StringReader(IdScanner)
                        Using reader As XmlReader = XmlReader.Create(tReader)
                            Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                            currentAppxName = id.PackageName
                            currentAppxPublisher = id.PackagePublisher
                            currentAppxVersion = id.PackageVersion
                            currentAppxArchitecture = id.PackageArchitecture
                        End Using
                    End Using
                    AppxNameList.Add(currentAppxName)
                    AppxPublisherList.Add(currentAppxPublisher)
                    AppxVersionList.Add(currentAppxVersion)
                    AppxNames = AppxNameList.ToArray()
                    AppxPublishers = AppxPublisherList.ToArray()
                    AppxVersion = AppxVersionList.ToArray()
                ElseIf ScannerRTB.Lines(2).Contains("<Identity Name=") Then
                    IdScanner = ScannerRTB.Lines(2)
                    Dim serializer As New XmlSerializer(GetType(AppxPackage))
                    Using tReader As TextReader = New StringReader(IdScanner)
                        Using reader As XmlReader = XmlReader.Create(tReader)
                            Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                            currentAppxName = id.PackageName
                            currentAppxPublisher = id.PackagePublisher
                            currentAppxVersion = id.PackageVersion
                            currentAppxArchitecture = id.PackageArchitecture
                        End Using
                    End Using
                    AppxNameList.Add(currentAppxName)
                    AppxPublisherList.Add(currentAppxPublisher)
                    AppxVersionList.Add(currentAppxVersion)
                    AppxNames = AppxNameList.ToArray()
                    AppxPublishers = AppxPublisherList.ToArray()
                    AppxVersion = AppxVersionList.ToArray()
                End If
            ElseIf File.Exists(Package & "\AppxManifest.xml") Then
                ' AppX file
                ScannerRTB.Text = My.Computer.FileSystem.ReadAllText(Package & "\AppxManifest.xml")
                If ScannerRTB.Lines(2).EndsWith("<!--") Then
                    Dim IdScanner As String = ScannerRTB.Lines(9)
                    Dim serializer As New XmlSerializer(GetType(AppxPackage))
                    Using tReader As TextReader = New StringReader(IdScanner)
                        Using reader As XmlReader = XmlReader.Create(tReader)
                            Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                            currentAppxName = id.PackageName
                            currentAppxPublisher = id.PackagePublisher
                            currentAppxVersion = id.PackageVersion
                            currentAppxArchitecture = id.PackageArchitecture
                        End Using
                    End Using
                    AppxNameList.Add(currentAppxName)
                    AppxPublisherList.Add(currentAppxPublisher)
                    AppxVersionList.Add(currentAppxVersion)
                    AppxNames = AppxNameList.ToArray()
                    AppxPublishers = AppxPublisherList.ToArray()
                    AppxVersion = AppxVersionList.ToArray()
                End If
            Else
                ' Unrecognized type
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                MsgBox("This folder doesn't seem to contain an AppX package structure. It will not be added to the list", vbOKOnly + vbExclamation, "Add provisioned AppX packages")
                            Case "ESN"
                                MsgBox("Esta carpeta no parece contener una estructura de un paquete AppX. No será añadida a la lista", vbOKOnly + vbExclamation, "Añadir paquetes aprovisionados AppX")
                        End Select
                    Case 1
                        MsgBox("This folder doesn't seem to contain an AppX package structure. It will not be added to the list", vbOKOnly + vbExclamation, "Add provisioned AppX packages")
                    Case 2
                        MsgBox("Esta carpeta no parece contener una estructura de un paquete AppX. No será añadida a la lista", vbOKOnly + vbExclamation, "Añadir paquetes aprovisionados AppX")
                End Select
                Exit Sub
            End If
            GetApplicationStoreLogoAssets(pkgName, True, False, Package, currentAppxName)
        Else
            If Directory.Exists(Application.StartupPath & "\appxscan") Then Directory.Delete(Application.StartupPath & "\appxscan", True)
            Directory.CreateDirectory(Application.StartupPath & "\appxscan")
            AppxScanner.StartInfo.FileName = Application.StartupPath & "\bin\utils\" & If(Environment.Is64BitOperatingSystem, "x64", "x86") & "\7z.exe"
            AppxScanner.StartInfo.Arguments = "e " & Quote & Package & Quote & " " & Quote & If(Path.GetExtension(Package).EndsWith("bundle", StringComparison.OrdinalIgnoreCase), "appxmetadata\appxbundlemanifest.xml", "appxmanifest.xml") & Quote & " -o" & Quote & Application.StartupPath & "\appxscan" & Quote
            AppxScanner.StartInfo.CreateNoWindow = True
            AppxScanner.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            AppxScanner.Start()
            AppxScanner.WaitForExit()
            If AppxScanner.ExitCode = 0 Then
                If Path.GetExtension(Package).EndsWith("bundle", StringComparison.OrdinalIgnoreCase) Then
                    ScannerRTB.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\appxscan\AppxBundleManifest.xml")
                    Dim IdScanner As String = ScannerRTB.Lines(If(ScannerRTB.Lines(2).EndsWith("<!--"), 10, 4))
                    Dim CharIndex As Integer = 0
                    Dim CharNext As Integer
                    For Each Character As Char In ScannerRTB.Lines(If(ScannerRTB.Lines(2).EndsWith("<!--"), 10, 4))
                        CharNext = CharIndex + 1
                        If Not IdScanner(CharIndex) = Quote Then
                            CharIndex += 1
                            Continue For
                        ElseIf IdScanner(CharIndex) = Quote And IdScanner(CharNext) = " " Then
                            CharIndex += 1
                            Continue For
                        Else
                            Character = IdScanner(CharIndex + 1)
                            If Not IdScanner(CharIndex + Stepper) = " " Then
                                If QuoteCount = 3 Then
                                    QuoteCount += 1
                                    Do
                                        If Character = Quote Then
                                            CharIndex += Stepper - 1
                                            Character = IdScanner(CharIndex - 1)
                                            QuoteCount += 1
                                            Stepper = 2
                                            Exit For
                                        Else
                                            pkgName &= Character.ToString()
                                            Character = IdScanner(CharIndex + Stepper)
                                            Stepper += 1
                                        End If
                                    Loop
                                Else
                                    QuoteCount += 1
                                    CharIndex += Stepper - 1
                                    Character = IdScanner(CharIndex + Stepper)
                                End If
                            End If
                        End If
                    Next
                    pkgName = pkgName.Replace(" ", "%20").Trim()
                    QuoteCount = 0
                    Stepper = 2
                    If ScannerRTB.Lines(2).EndsWith("<!--") Then
                        ' XML comment
                        IdScanner = ScannerRTB.Lines(9)
                        Dim serializer As New XmlSerializer(GetType(AppxPackage))
                        Using tReader As TextReader = New StringReader(IdScanner)
                            Using reader As XmlReader = XmlReader.Create(tReader)
                                Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                                currentAppxName = id.PackageName
                                currentAppxPublisher = id.PackagePublisher
                                currentAppxVersion = id.PackageVersion
                                currentAppxArchitecture = id.PackageArchitecture
                            End Using
                        End Using
                        AppxNameList.Add(currentAppxName)
                        AppxPublisherList.Add(currentAppxPublisher)
                        AppxVersionList.Add(currentAppxVersion)
                        AppxNames = AppxNameList.ToArray()
                        AppxPublishers = AppxPublisherList.ToArray()
                        AppxVersion = AppxVersionList.ToArray()
                    ElseIf ScannerRTB.Lines(2).Contains("<Identity Name=") Then
                        IdScanner = ScannerRTB.Lines(2)
                        Dim serializer As New XmlSerializer(GetType(AppxPackage))
                        Using tReader As TextReader = New StringReader(IdScanner)
                            Using reader As XmlReader = XmlReader.Create(tReader)
                                Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                                currentAppxName = id.PackageName
                                currentAppxPublisher = id.PackagePublisher
                                currentAppxVersion = id.PackageVersion
                                currentAppxArchitecture = id.PackageArchitecture
                            End Using
                        End Using
                        AppxNameList.Add(currentAppxName)
                        AppxPublisherList.Add(currentAppxPublisher)
                        AppxVersionList.Add(currentAppxVersion)
                        AppxNames = AppxNameList.ToArray()
                        AppxPublishers = AppxPublisherList.ToArray()
                        AppxVersion = AppxVersionList.ToArray()
                    End If
                Else
                    ScannerRTB.Text = My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\appxscan\AppxManifest.xml")
                    If ScannerRTB.Lines(2).EndsWith("<!--") Then
                        Dim IdScanner As String = ScannerRTB.Lines(9)
                        Dim serializer As New XmlSerializer(GetType(AppxPackage))
                        Using tReader As TextReader = New StringReader(IdScanner)
                            Using reader As XmlReader = XmlReader.Create(tReader)
                                Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                                currentAppxName = id.PackageName
                                currentAppxPublisher = id.PackagePublisher
                                currentAppxVersion = id.PackageVersion
                                currentAppxArchitecture = id.PackageArchitecture
                            End Using
                        End Using
                        AppxNameList.Add(currentAppxName)
                        AppxPublisherList.Add(currentAppxPublisher)
                        AppxVersionList.Add(currentAppxVersion)
                        AppxNames = AppxNameList.ToArray()
                        AppxPublishers = AppxPublisherList.ToArray()
                        AppxVersion = AppxVersionList.ToArray()
                    ElseIf ScannerRTB.Lines(2).Contains("<Identity Name=") Then
                        Dim IdScanner As String = ScannerRTB.Lines(2)
                        Dim serializer As New XmlSerializer(GetType(AppxPackage))
                        Using tReader As TextReader = New StringReader(IdScanner)
                            Using reader As XmlReader = XmlReader.Create(tReader)
                                Dim id = CType(serializer.Deserialize(reader), AppxPackage)
                                currentAppxName = id.PackageName
                                currentAppxPublisher = id.PackagePublisher
                                currentAppxVersion = id.PackageVersion
                                currentAppxArchitecture = id.PackageArchitecture
                            End Using
                        End Using
                        AppxNameList.Add(currentAppxName)
                        AppxPublisherList.Add(currentAppxPublisher)
                        AppxVersionList.Add(currentAppxVersion)
                        AppxNames = AppxNameList.ToArray()
                        AppxPublishers = AppxPublisherList.ToArray()
                        AppxVersion = AppxVersionList.ToArray()
                    End If
                End If
                GetApplicationStoreLogoAssets(pkgName, False, If(Path.GetExtension(Package).EndsWith("bundle", StringComparison.OrdinalIgnoreCase), True, False), Package, currentAppxName)
            Else

            End If
        End If
        ' Detect ListView items
        If ListView1.Items.Count > 0 Then
            ' Iterate through the ListView items until we can find an entry with properties similar to those currently obtained
            For Each Item As ListViewItem In ListView1.Items
                If Item.SubItems(2).Text = currentAppxName And Item.SubItems(3).Text = currentAppxPublisher And Item.SubItems(4).Text = currentAppxVersion And Packages(Item.Index).PackageArchitecture = currentAppxArchitecture Then
                    ' Cancel everything
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    MsgBox("The package you want to add is already added to the list, and all its properties match with the properties of the package specified. We won't add the specified package", vbOKOnly + vbCritical, Label1.Text)
                                Case "ESN"
                                    MsgBox("El paquete que desea añadir ya está añadido a la lista, y todas sus propiedades coinciden con las propiedades del paquete especificado. No añadiremos el paquete especificado", vbOKOnly + vbCritical, Label1.Text)
                            End Select
                        Case 1
                            MsgBox("The package you want to add is already added to the list, and all its properties match with the properties of the package specified. We won't add the specified package", vbOKOnly + vbCritical, Label1.Text)
                        Case 2
                            MsgBox("El paquete que desea añadir ya está añadido a la lista, y todas sus propiedades coinciden con las propiedades del paquete especificado. No añadiremos el paquete especificado", vbOKOnly + vbCritical, Label1.Text)
                    End Select
                    If Directory.Exists(Application.StartupPath & "\appxscan") Then
                        Directory.Delete(Application.StartupPath & "\appxscan", True)
                    End If
                    Exit Sub
                ElseIf Item.SubItems(2).Text = currentAppxName And Not Item.SubItems(3).Text = currentAppxPublisher Then
                    Dim msg As String = ""
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    msg = "The package you want to add is already added to the list, but it comes from a different developer or publisher." & CrLf & CrLf & "Do note that applications redistributed by third-party publishers or developers can cause damage to the Windows image." & CrLf & CrLf & "Do you want to replace the entry in the list with the package specified?"
                                Case "ESN"
                                    msg = "El paquete que desea añadir ya está añadido a la lista, pero proviene de un desarrollador o publicador distinto." & CrLf & CrLf & "Dese cuenta de que las aplicaciones redistribuidas por publicadores o desarrolladores de terceros pueden dañar la imagen de Windows." & CrLf & CrLf & "¿Desea reemplazar la entrada en la lista por el paquete especificado?"
                            End Select
                        Case 1
                            msg = "The package you want to add is already added to the list, but it comes from a different developer or publisher." & CrLf & CrLf & "Do note that applications redistributed by third-party publishers or developers can cause damage to the Windows image." & CrLf & CrLf & "Do you want to replace the entry in the list with the package specified?"
                        Case 2
                            msg = "El paquete que desea añadir ya está añadido a la lista, pero proviene de un desarrollador o publicador distinto." & CrLf & CrLf & "Dese cuenta de que las aplicaciones redistribuidas por publicadores o desarrolladores de terceros pueden dañar la imagen de Windows." & CrLf & CrLf & "¿Desea reemplazar la entrada en la lista por el paquete especificado?"
                    End Select
                    If MsgBox(msg, vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.Yes Then
                        ' Set properties
                        Item.SubItems(0).Text = Package
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Item.SubItems(1).Text = If(IsFolder, "Unpacked", "Packed")
                                    Case "ESN"
                                        Item.SubItems(1).Text = If(IsFolder, "Desempaquetado", "Empaquetado")
                                End Select
                            Case 1
                                Item.SubItems(1).Text = If(IsFolder, "Unpacked", "Packed")
                            Case 2
                                Item.SubItems(1).Text = If(IsFolder, "Desempaquetado", "Empaquetado")
                        End Select
                        Item.SubItems(2).Text = currentAppxName
                        Item.SubItems(3).Text = currentAppxPublisher
                        Item.SubItems(4).Text = currentAppxVersion

                        ' Configure Element list
                        Packages(Item.Index).PackageFile = Package
                        Packages(Item.Index).PackageName = currentAppxName
                        Packages(Item.Index).PackagePublisher = currentAppxPublisher
                        Packages(Item.Index).PackageVersion = currentAppxVersion
                    Else
                        If Directory.Exists(Application.StartupPath & "\appxscan") Then
                            Directory.Delete(Application.StartupPath & "\appxscan", True)
                        End If
                    End If
                    Exit Sub
                ElseIf Item.SubItems(2).Text = currentAppxName And Not Item.SubItems(4).Text = currentAppxVersion Then
                    ' This is a rudimentary check which will run even if specifying an older version. It will be improved, so expect the following enhancements:
                    ' - Cast the version strings to version objects
                    ' - Compare the version objects part by part
                    Dim msg As String = ""
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    msg = "The package you want to add is already added to the list, but it contains a newer version." & CrLf & CrLf & "Do you want to replace the entry in the list with the updated package specified?"
                                Case "ESN"
                                    msg = "El paquete que desea añadir ya está añadido a la lista, pero contiene una nueva versión." & CrLf & CrLf & "¿Desea reemplazar la entrada en la lista por el paquete actualizado especificado?"
                            End Select
                        Case 1
                            msg = "The package you want to add is already added to the list, but it contains a newer version." & CrLf & CrLf & "Do you want to replace the entry in the list with the updated package specified?"
                        Case 2
                            msg = "El paquete que desea añadir ya está añadido a la lista, pero contiene una nueva versión." & CrLf & CrLf & "¿Desea reemplazar la entrada en la lista por el paquete actualizado especificado?"
                    End Select
                    If MsgBox(msg, vbYesNo + vbQuestion, Label1.Text) = MsgBoxResult.Yes Then
                        ' Set properties
                        Item.SubItems(0).Text = Package
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        Item.SubItems(1).Text = If(IsFolder, "Unpacked", "Packed")
                                    Case "ESN"
                                        Item.SubItems(1).Text = If(IsFolder, "Desempaquetado", "Empaquetado")
                                End Select
                            Case 1
                                Item.SubItems(1).Text = If(IsFolder, "Unpacked", "Packed")
                            Case 2
                                Item.SubItems(1).Text = If(IsFolder, "Desempaquetado", "Empaquetado")
                        End Select
                        Item.SubItems(2).Text = currentAppxName
                        Item.SubItems(3).Text = currentAppxPublisher
                        Item.SubItems(4).Text = currentAppxVersion

                        ' Configure Element list
                        Packages(Item.Index).PackageFile = Package
                        Packages(Item.Index).PackageName = currentAppxName
                        Packages(Item.Index).PackagePublisher = currentAppxPublisher
                        Packages(Item.Index).PackageVersion = currentAppxVersion
                    Else
                        If Directory.Exists(Application.StartupPath & "\appxscan") Then
                            Directory.Delete(Application.StartupPath & "\appxscan", True)
                        End If
                    End If
                    Exit Sub
                End If
            Next
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        If IsFolder Then
                            ListView1.Items.Add(New ListViewItem(New String() {Package, "Unpacked", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                        Else
                            ListView1.Items.Add(New ListViewItem(New String() {Package, "Packed", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                        End If
                    Case "ESN"
                        If IsFolder Then
                            ListView1.Items.Add(New ListViewItem(New String() {Package, "Desempaquetado", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                        Else
                            ListView1.Items.Add(New ListViewItem(New String() {Package, "Empaquetado", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                        End If
                End Select
            Case 1
                If IsFolder Then
                    ListView1.Items.Add(New ListViewItem(New String() {Package, "Unpacked", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                Else
                    ListView1.Items.Add(New ListViewItem(New String() {Package, "Packed", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                End If
            Case 2
                If IsFolder Then
                    ListView1.Items.Add(New ListViewItem(New String() {Package, "Desempaquetado", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                Else
                    ListView1.Items.Add(New ListViewItem(New String() {Package, "Empaquetado", currentAppxName, currentAppxPublisher, currentAppxVersion}))
                End If
        End Select
        Dim currentPackage As New AppxPackage()
        currentPackage.PackageFile = Package
        currentPackage.PackageName = currentAppxName
        currentPackage.PackagePublisher = currentAppxPublisher
        currentPackage.PackageVersion = currentAppxVersion
        currentPackage.PackageArchitecture = currentAppxArchitecture
        If Not Packages.Contains(currentPackage) Then Packages.Add(currentPackage)
        Button3.Enabled = True
        If Directory.Exists(Application.StartupPath & "\appxscan") Then
            Directory.Delete(Application.StartupPath & "\appxscan", True)
        End If
    End Sub

    ''' <summary>
    ''' Gets the application store logo assets from APPX or APPXBUNDLE packages (also from MSIX and MSIXBUNDLE packages)
    ''' </summary>
    ''' <param name="PackageName">The name of the package. Packages with names containing spaces will replace those with &quot;%20&quot;</param>
    ''' <param name="IsDirectory">Determines if the package given is an unpacked APPX/MSIX/APPXBUNDLE/MSIXBUNDLE file</param>
    ''' <param name="IsBundlePackage">Determines if the package given is an APPXBUNDLE or MSIXBUNDLE package</param>
    ''' <param name="SourcePackage">The path of the source package</param>
    ''' <param name="AppxPackageName">The name of the AppX package, used for storing logo assets in an organized way</param>
    ''' <remarks>If the package processed is an APPXBUNDLE or MSIXBUNDLE package, this procedure will extract the asset contents from the package with the given name. Otherwise, it will directly extract them from the &quot;Assets&quot; folder</remarks>
    Sub GetApplicationStoreLogoAssets(PackageName As String, IsDirectory As Boolean, IsBundlePackage As Boolean, SourcePackage As String, AppxPackageName As String)
        ' The assets from the main package are enough for us. The current AppX XML schema also puts these in the Assets folder, so
        ' getting them should be a breeze
        Try
            If IsDirectory Then
                If File.Exists(SourcePackage & "\AppxMetadata\AppxBundleManifest.xml") Then
                    ' APPXBUNDLE/MSIXBUNDLE
                    AppxScanner.StartInfo.Arguments = "x " & Quote & SourcePackage & "\" & PackageName & Quote & " -o" & Quote & Application.StartupPath & "\appxscan" & Quote
                    AppxScanner.Start()
                    AppxScanner.WaitForExit()
                    If Not Directory.Exists(Application.StartupPath & "\temp\storeassets") Then Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets").Attributes = FileAttributes.Hidden
                    If AppxScanner.ExitCode = 0 Then
                        Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets\" & AppxPackageName)
                        If My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & AppxPackageName).Count <= 0 Then
                            For Each AssetFile In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\appxscan\Assets", FileIO.SearchOption.SearchTopLevelOnly)
                                If Path.GetFileNameWithoutExtension(AssetFile).StartsWith("small", StringComparison.OrdinalIgnoreCase) Then
                                    File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                                ElseIf Path.GetFileNameWithoutExtension(AssetFile).StartsWith("store", StringComparison.OrdinalIgnoreCase) Then
                                    File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                                ElseIf Path.GetFileNameWithoutExtension(AssetFile).StartsWith("large", StringComparison.OrdinalIgnoreCase) Then
                                    File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                                End If
                            Next
                        End If
                    End If
                    Directory.Delete(Application.StartupPath & "\appxscan", True)
                ElseIf File.Exists(SourcePackage & "\AppxManifest.xml") Then
                    ' APPX/MSIX
                    If Not Directory.Exists(Application.StartupPath & "\temp\storeassets") Then Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets").Attributes = FileAttributes.Hidden
                    If My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & AppxPackageName).Count <= 0 Then
                        For Each AssetFile In My.Computer.FileSystem.GetFiles(SourcePackage & "\Assets", FileIO.SearchOption.SearchTopLevelOnly)
                            If Path.GetFileNameWithoutExtension(AssetFile).StartsWith("small", StringComparison.OrdinalIgnoreCase) Then
                                File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                            ElseIf Path.GetFileNameWithoutExtension(AssetFile).StartsWith("store", StringComparison.OrdinalIgnoreCase) Then
                                File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                            ElseIf Path.GetFileNameWithoutExtension(AssetFile).StartsWith("large", StringComparison.OrdinalIgnoreCase) Then
                                File.Copy(AssetFile, Application.StartupPath & "\temp\storeassets\" & Path.GetFileName(AssetFile))
                            End If
                        Next
                    End If
                Else
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    MsgBox("Could not get application store logo assets from this package - cannot read from manifest", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                                Case "ESN"
                                    MsgBox("No se pudo obtener recursos de logotipos de este paquete - no se puede leer el manifiesto", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                            End Select
                        Case 1
                            MsgBox("Could not get application store logo assets from this package - cannot read from manifest", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                        Case 2
                            MsgBox("No se pudo obtener recursos de logotipos de este paquete - no se puede leer el manifiesto", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                    End Select
                End If
            Else
                If IsBundlePackage Then
                    AppxScanner.StartInfo.Arguments = "e " & Quote & SourcePackage & Quote & " " & Quote & PackageName & Quote & " -o" & Quote & Application.StartupPath & "\appxscan" & Quote
                    AppxScanner.Start()
                    AppxScanner.WaitForExit()
                    If Not Directory.Exists(Application.StartupPath & "\temp\storeassets") Then Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets").Attributes = FileAttributes.Hidden
                    If AppxScanner.ExitCode = 0 Then
                        Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets\" & AppxPackageName)
                        If My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & AppxPackageName).Count <= 0 Then
                            ' Try extracting small, store and large assets
                            AppxScanner.StartInfo.Arguments = "e " & Quote & Application.StartupPath & "\appxscan\" & PackageName & Quote & " " & Quote & "Assets\small*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                            AppxScanner.Start()
                            AppxScanner.WaitForExit()
                            AppxScanner.StartInfo.Arguments = "e " & Quote & Application.StartupPath & "\appxscan\" & PackageName & Quote & " " & Quote & "Assets\store*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                            AppxScanner.Start()
                            AppxScanner.WaitForExit()
                            AppxScanner.StartInfo.Arguments = "e " & Quote & Application.StartupPath & "\appxscan\" & PackageName & Quote & " " & Quote & "Assets\large*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                            AppxScanner.Start()
                            AppxScanner.WaitForExit()
                        End If
                    End If
                Else
                    If Not Directory.Exists(Application.StartupPath & "\temp\storeassets") Then Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets").Attributes = FileAttributes.Hidden
                    Directory.CreateDirectory(Application.StartupPath & "\temp\storeassets\" & AppxPackageName)
                    If My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & AppxPackageName).Count <= 0 Then
                        ' Try extracting small, store and large assets
                        AppxScanner.StartInfo.Arguments = "e " & Quote & SourcePackage & Quote & " " & Quote & "Assets\small*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                        AppxScanner.Start()
                        AppxScanner.WaitForExit()
                        AppxScanner.StartInfo.Arguments = "e " & Quote & SourcePackage & Quote & " " & Quote & "Assets\store*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                        AppxScanner.Start()
                        AppxScanner.WaitForExit()
                        AppxScanner.StartInfo.Arguments = "e " & Quote & SourcePackage & Quote & " " & Quote & "Assets\large*" & Quote & " -o" & Quote & Application.StartupPath & "\temp\storeassets\" & AppxPackageName & Quote
                        AppxScanner.Start()
                        AppxScanner.WaitForExit()
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine("Could not get store logo assets. Reason: " & ex.ToString())
        End Try
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If ListView1.FocusedItem.Text <> "" Then
            Packages.RemoveAt(ListView1.FocusedItem.Index)
            ListView1.Items.Remove(ListView1.FocusedItem)
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://en.wikipedia.org/wiki/ISO_3166-1#Current_codes")
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.Checked Then
            TextBox1.Enabled = True
            Button7.Enabled = True
        Else
            TextBox1.Enabled = False
            Button7.Enabled = False
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            TextBox2.Enabled = True
            Button8.Enabled = True
        Else
            TextBox2.Enabled = False
            Button8.Enabled = False
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked Then
            TextBox3.Enabled = False
            Label5.Enabled = False
            LinkLabel1.Enabled = False
        Else
            TextBox3.Enabled = True
            Label5.Enabled = True
            LinkLabel1.Enabled = True
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            If ListView1.SelectedItems.Count <> 1 Then
                Button9.Enabled = False
            Else
                Button9.Enabled = True
            End If
        Catch ex As NullReferenceException
            Button9.Enabled = True
        End Try
        NoAppxFilePanel.Visible = If(ListView1.SelectedItems.Count <= 0, True, False)
        AppxFilePanel.Visible = If(ListView1.SelectedItems.Count <= 0, False, True)
        AppxDetailsPanel.Height = If(ListView1.SelectedItems.Count <= 0, 520, 83)
        FlowLayoutPanel1.Visible = If(ListView1.SelectedItems.Count <= 0, False, True)
        If ListView1.SelectedItems.Count > 0 Then
            Try
                Label7.Text = ListView1.FocusedItem.SubItems(2).Text
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                Label8.Text = "Publisher: " & ListView1.FocusedItem.SubItems(3).Text
                                Label9.Text = "Version: " & ListView1.FocusedItem.SubItems(4).Text
                            Case "ESN"
                                Label8.Text = "Publicador: " & ListView1.FocusedItem.SubItems(3).Text
                                Label9.Text = "Versión: " & ListView1.FocusedItem.SubItems(4).Text
                        End Select
                    Case 1
                        Label8.Text = "Publisher: " & ListView1.FocusedItem.SubItems(3).Text
                        Label9.Text = "Version: " & ListView1.FocusedItem.SubItems(4).Text
                    Case 2
                        Label8.Text = "Publicador: " & ListView1.FocusedItem.SubItems(3).Text
                        Label9.Text = "Versión: " & ListView1.FocusedItem.SubItems(4).Text
                End Select
            Catch ex As NullReferenceException

            End Try
        End If
        Try
            If Directory.Exists(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text) And My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count > 0 Then
                PictureBox2.SizeMode = PictureBoxSizeMode.Zoom
                Dim asset As Image = Nothing
                For Each StoreAsset In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text)
                    If Path.GetExtension(StoreAsset).EndsWith("png") Then
                        asset = Image.FromFile(StoreAsset)
                        If asset.Width / asset.Height = 1 Then      ' Determine if the image's aspect ratio is 1:1
                            If asset.Width <= 100 And asset.Height <= 100 Then      ' Determine if it is a "small" or "store" asset
                                PictureBox2.Image = asset
                            End If
                        End If
                    End If
                Next
            Else
                PictureBox2.SizeMode = PictureBoxSizeMode.CenterImage
                PictureBox2.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.preview_unavail_dark, My.Resources.preview_unavail_light)
            End If
        Catch ex As Exception
            PictureBox2.SizeMode = PictureBoxSizeMode.CenterImage
            PictureBox2.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.preview_unavail_dark, My.Resources.preview_unavail_light)
        End Try

        ' Detect properties obtained by the AppxPackage Element
        Try
            If ListView1.SelectedItems.Count = 1 Then
                ListBox1.Items.Clear()
                If Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Count > 0 Then
                    For Each Dependency As AppxDependency In Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
                        ListBox1.Items.Add(Dependency.DependencyFile)
                    Next
                End If
            End If
        Catch ex As Exception
            NoAppxFilePanel.Visible = True
            AppxFilePanel.Visible = False
            AppxDetailsPanel.Height = 520
            FlowLayoutPanel1.Visible = False
        End Try
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.SelectedItem = "" Then
            Button5.Enabled = False
        Else
            Button5.Enabled = True
        End If
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        If My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0 Then Exit Sub
        HidePopupForm()
        With LogoAssetPopupForm
            .BackColor = BackColor
            .ForeColor = ForeColor
            .ShowIcon = False
            .ShowInTaskbar = False
            .ControlBox = False
            .FormBorderStyle = Windows.Forms.FormBorderStyle.None
            .Size = New Size(152, 152)
            Dim ctrlLoc As Point = PictureBox2.PointToScreen(Point.Empty)
            .StartPosition = FormStartPosition.Manual
            .Location = ctrlLoc
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            .Text = "Preview"
                        Case "ESN"
                            .Text = "Vista previa"
                    End Select
                Case 1
                    .Text = "Preview"
                Case 2
                    .Text = "Vista previa"
            End Select
            With LogoAssetPreview
                .Parent = LogoAssetPopupForm
                .Dock = DockStyle.Fill
                .SizeMode = PictureBoxSizeMode.Zoom
                Try
                    If Directory.Exists(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text) And My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count > 0 Then
                        Dim asset As Image = Nothing
                        For Each StoreAsset In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text)
                            If Path.GetExtension(StoreAsset).EndsWith("png") Then
                                asset = Image.FromFile(StoreAsset)
                                If asset.Width / asset.Height = 1 Then      ' Determine if the image's aspect ratio is 1:1
                                    If asset.Width > 100 And asset.Width <= 200 And asset.Height > 100 And asset.Height <= 200 Then      ' Determine if it is a "large" asset
                                        .Image = asset
                                        Exit For
                                    Else
                                        .SizeMode = PictureBoxSizeMode.CenterImage
                                        .Image = PictureBox2.Image
                                    End If
                                End If
                            End If
                        Next
                    End If
                Catch ex As Exception

                End Try
            End With
            .Controls.Add(LogoAssetPreview)
            AddHandler LogoAssetPreview.Click, AddressOf HidePopupForm
            .Show()
            .BringToFront()
        End With
    End Sub

    Sub HidePopupForm() Handles MyBase.FormClosing, MyBase.VisibleChanged
        LogoAssetPopupForm.Hide()
    End Sub

    Private Sub PictureBox2_MouseHover(sender As Object, e As EventArgs) Handles PictureBox2.MouseHover
        Try
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            previewer.SetToolTip(sender, If(My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0, "The logo assets for this file could not be detected", "Click here to enlarge the view"))
                        Case "ESN"
                            previewer.SetToolTip(sender, If(My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0, "Los recursos de este archivo no pudieron ser detectados", "Haga clic para agrandar la vista"))
                    End Select
                Case 1
                    previewer.SetToolTip(sender, If(My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0, "The logo assets for this file could not be detected", "Click here to enlarge the view"))
                Case 2
                    previewer.SetToolTip(sender, If(My.Computer.FileSystem.GetFiles(Application.StartupPath & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count <= 0, "Los recursos de este archivo no pudieron ser detectados", "Haga clic para agrandar la vista"))
            End Select
        Catch ex As Exception
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            previewer.SetToolTip(sender, "The logo assets for this file could not be detected")
                        Case "ESN"
                            previewer.SetToolTip(sender, "Los recursos de este archivo no pudieron ser detectados")
                    End Select
                Case 1
                    previewer.SetToolTip(sender, "The logo assets for this file could not be detected")
                Case 2
                    previewer.SetToolTip(sender, "Los recursos de este archivo no pudieron ser detectados")
            End Select
        End Try
    End Sub

    Private Sub ListView1_DragEnter(sender As Object, e As DragEventArgs) Handles ListView1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ListView1_DragDrop(sender As Object, e As DragEventArgs) Handles ListView1.DragDrop
        Dim PackageFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        Cursor = Cursors.WaitCursor
        For Each PackageFile In PackageFiles
            If Path.GetExtension(PackageFile).Equals(".appx", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(PackageFile).Equals(".msix", StringComparison.OrdinalIgnoreCase) Or _
                Path.GetExtension(PackageFile).Equals(".appxbundle", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(PackageFile).Equals(".msixbundle", StringComparison.OrdinalIgnoreCase) Or _
                Path.GetExtension(PackageFile).Equals(".eappx", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(PackageFile).Equals(".emsix", StringComparison.OrdinalIgnoreCase) Or _
                Path.GetExtension(PackageFile).Equals(".eappxbundle", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(PackageFile).Equals(".emsixbundle", StringComparison.OrdinalIgnoreCase) Then
                ScanAppxPackage(False, PackageFile)
            ElseIf File.GetAttributes(PackageFile) = FileAttributes.Directory Then
                ' Temporary support for directories
                If File.Exists(PackageFile & "\AppxSignature.p7x") And File.Exists(PackageFile & "\AppxMetadata\AppxBundleManifest.xml") Or File.Exists(PackageFile & "\AppxManifest.xml") Then
                    ScanAppxPackage(True, PackageFile)
                ElseIf My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchTopLevelOnly, "*.appx").Count > 0 Or My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchTopLevelOnly, "*.msix").Count > 0 Or _
                    My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchTopLevelOnly, "*.appxbundle").Count > 0 Or My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchTopLevelOnly, "*.msixbundle").Count > 0 Then
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    If MsgBox("The following directory:" & CrLf & Quote & PackageFile & Quote & CrLf & "contains application packages. Do you want to process them as well?" & CrLf & CrLf & "NOTE: this will scan this directory recursively, so it may take longer for this operation to complete", vbYesNo + vbQuestion, "Add provisioned AppX packages") = MsgBoxResult.Yes Then
                                        For Each AppPkg In My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchAllSubDirectories)
                                            If Path.GetExtension(AppPkg).Equals(".appx", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(AppPkg).Equals(".appxbundle", StringComparison.OrdinalIgnoreCase) Or _
                                                Path.GetExtension(AppPkg).Equals(".msix", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(AppPkg).Equals(".msixbundle", StringComparison.OrdinalIgnoreCase) Then
                                                ScanAppxPackage(False, AppPkg)
                                            Else
                                                Continue For
                                            End If
                                        Next
                                    Else
                                        Continue For
                                    End If
                                Case "ESN"
                                    If MsgBox("El siguiente directorio:" & CrLf & Quote & PackageFile & Quote & CrLf & "contiene paquetes de aplicación. ¿Desea procesarlos también?" & CrLf & CrLf & "NOTA: esto escaneará este directorio de una forma recursiva, así que esta operación podría tardar más tiempo en completar", vbYesNo + vbQuestion, "Añadir paquetes aprovisionados AppX") = MsgBoxResult.Yes Then
                                        For Each AppPkg In My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchAllSubDirectories)
                                            If Path.GetExtension(AppPkg).Equals(".appx", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(AppPkg).Equals(".appxbundle", StringComparison.OrdinalIgnoreCase) Or _
                                                Path.GetExtension(AppPkg).Equals(".msix", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(AppPkg).Equals(".msixbundle", StringComparison.OrdinalIgnoreCase) Then
                                                ScanAppxPackage(False, AppPkg)
                                            Else
                                                Continue For
                                            End If
                                        Next
                                    Else
                                        Continue For
                                    End If
                            End Select
                        Case 1
                            If MsgBox("The following directory:" & CrLf & Quote & PackageFile & Quote & CrLf & "contains application packages. Do you want to process them as well?" & CrLf & CrLf & "NOTE: this will scan this directory recursively, so it may take longer for this operation to complete", vbYesNo + vbQuestion, "Add provisioned AppX packages") = MsgBoxResult.Yes Then
                                For Each AppPkg In My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchAllSubDirectories)
                                    If Path.GetExtension(AppPkg).Equals(".appx", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(AppPkg).Equals(".appxbundle", StringComparison.OrdinalIgnoreCase) Or _
                                        Path.GetExtension(AppPkg).Equals(".msix", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(AppPkg).Equals(".msixbundle", StringComparison.OrdinalIgnoreCase) Then
                                        ScanAppxPackage(False, AppPkg)
                                    Else
                                        Continue For
                                    End If
                                Next
                            Else
                                Continue For
                            End If
                        Case 2
                            If MsgBox("El siguiente directorio:" & CrLf & Quote & PackageFile & Quote & CrLf & "contiene paquetes de aplicación. ¿Desea procesarlos también?" & CrLf & CrLf & "NOTA: esto escaneará este directorio de una forma recursiva, así que esta operación podría tardar más tiempo en completar", vbYesNo + vbQuestion, "Añadir paquetes aprovisionados AppX") = MsgBoxResult.Yes Then
                                For Each AppPkg In My.Computer.FileSystem.GetFiles(PackageFile, FileIO.SearchOption.SearchAllSubDirectories)
                                    If Path.GetExtension(AppPkg).Equals(".appx", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(AppPkg).Equals(".appxbundle", StringComparison.OrdinalIgnoreCase) Or _
                                        Path.GetExtension(AppPkg).Equals(".msix", StringComparison.OrdinalIgnoreCase) Or Path.GetExtension(AppPkg).Equals(".msixbundle", StringComparison.OrdinalIgnoreCase) Then
                                        ScanAppxPackage(False, AppPkg)
                                    Else
                                        Continue For
                                    End If
                                Next
                            Else
                                Continue For
                            End If
                    End Select
                End If
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                MsgBox("The file that has been dropped here isn't an application package.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                            Case "ESN"
                                MsgBox("El archivo que se ha soltado aquí no es un paquete de aplicación.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                        End Select
                    Case 1
                        MsgBox("The file that has been dropped here isn't an application package.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                    Case 2
                        MsgBox("El archivo que se ha soltado aquí no es un paquete de aplicación.", vbOKOnly + vbCritical, "Añadir paquetes aprovisionados AppX")
                End Select
            End If
        Next
        Cursor = Cursors.Arrow
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        LicenseFileOFD.ShowDialog()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        CustomDataFileOFD.ShowDialog()
    End Sub

    Private Sub ListBox1_DragEnter(sender As Object, e As DragEventArgs) Handles ListBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ListBox1_DragDrop(sender As Object, e As DragEventArgs) Handles ListBox1.DragDrop
        If ListView1.SelectedItems.Count < 1 Then Exit Sub
        Dim DependencyFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each Dependency In DependencyFiles
            If Not ListBox1.Items.Contains(Dependency) And (Path.GetExtension(Dependency).EndsWith("appx", StringComparison.OrdinalIgnoreCase) Or _
                                                            Path.GetExtension(Dependency).EndsWith("msix", StringComparison.OrdinalIgnoreCase) Or _
                                                            Path.GetExtension(Dependency).EndsWith("appxbundle", StringComparison.OrdinalIgnoreCase) Or _
                                                            Path.GetExtension(Dependency).EndsWith("msixbundle", StringComparison.OrdinalIgnoreCase)) Then
                Dim dep As New AppxDependency()
                dep.DependencyFile = Dependency
                If Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Count > 0 And Not Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Contains(dep) Then
                    Dim deps As New List(Of AppxDependency)
                    deps = Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
                    deps.Add(dep)
                    Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies = deps
                ElseIf Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies.Count = 0 Then
                    Dim deps As New List(Of AppxDependency)
                    deps = Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies
                    deps.Add(dep)
                    Packages(ListView1.FocusedItem.Index).PackageSpecifiedDependencies = deps
                End If
                ListBox1.Items.Add(Dependency)
            End If
        Next
        If ListBox1.Items.Count > 0 Then Button4.Enabled = True
    End Sub
End Class
