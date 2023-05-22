Imports System.Windows.Forms
Imports System.IO

Public Class AddDrivers

    Dim drvPkgList As New List(Of String)
    Dim drvPkgs(65535) As String
    Dim drvRecursiveList As New List(Of String)
    Dim drvRecursivePkgs(65535) As String

    Dim drvPkgCount As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        'Array.Clear(drvPkgs, 0, drvPkgs.Length)
        'Array.Clear(drvRecursivePkgs, 0, drvRecursivePkgs.Length)
        drvPkgList.Clear()
        drvRecursiveList.Clear()
        ProgressPanel.MountDir = MainForm.MountDir
        drvPkgCount = ListView1.Items.Count
        If ListView1.Items.Count > 0 Then
            For x = 0 To ListView1.Items.Count - 1
                drvPkgList.Add(ListView1.Items(x).SubItems(0).Text)
            Next
            drvPkgs = drvPkgList.ToArray()
            For x = 0 To drvPkgs.Length - 1
                ProgressPanel.drvAdditionPkgs(x) = drvPkgs(x)
            Next
            For x = 0 To drvPkgCount - 1
                If File.GetAttributes(ListView1.Items(x).SubItems(0).Text) = FileAttributes.Directory And CheckedListBox1.CheckedItems.Contains(ListView1.Items(x).SubItems(0).Text) Then
                    drvRecursiveList.Add(ListView1.Items(x).SubItems(0).Text)
                End If
            Next
            If drvRecursiveList.Count > 0 Then
                drvRecursivePkgs = drvRecursiveList.ToArray()
                For x = 0 To drvRecursivePkgs.Length - 1
                    ProgressPanel.drvAdditionFolderRecursiveScan(x) = drvRecursivePkgs(x)
                Next
            End If
            If CheckBox1.Checked Then
                ProgressPanel.drvAdditionForceUnsigned = True
            Else
                ProgressPanel.drvAdditionForceUnsigned = False
            End If
            If CheckBox2.Checked Then
                ProgressPanel.drvAdditionCommit = True
            Else
                ProgressPanel.drvAdditionCommit = False
            End If
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            MsgBox("There are no selected driver packages to install. Please specify the driver packages you'd like to install and try again.", vbOKOnly + vbCritical, Label1.Text)
                        Case "ESN"
                            MsgBox("No hay paquetes de controladores seleccionados para instalar. Especifique los paquetes de controladores que le gustaría instalar e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                    End Select
                Case 1
                    MsgBox("There are no selected driver packages to install. Please specify the driver packages you'd like to install and try again.", vbOKOnly + vbCritical, Label1.Text)
                Case 2
                    MsgBox("No hay paquetes de controladores seleccionados para instalar. Especifique los paquetes de controladores que le gustaría instalar e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
            End Select
            Exit Sub
        End If
        ProgressPanel.drvAdditionLastPkg = ListView1.Items(drvPkgCount - 1).SubItems(0).Text
        ProgressPanel.drvAdditionCount = drvPkgCount
        ProgressPanel.OperationNum = 75
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "File"}))
                    Case "ESN"
                        ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "Archivo"}))
                End Select
            Case 1
                ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "File"}))
            Case 2
                ListView1.Items.Add(New ListViewItem(New String() {OpenFileDialog1.FileName, "Archivo"}))
        End Select
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Cursor = Cursors.WaitCursor
            If My.Computer.FileSystem.GetFiles(FolderBrowserDialog1.SelectedPath, FileIO.SearchOption.SearchAllSubDirectories, "*.inf").Count > 0 Then
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Folder"}))
                            Case "ESN"
                                ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Carpeta"}))
                        End Select
                    Case 1
                        ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Folder"}))
                    Case 2
                        ListView1.Items.Add(New ListViewItem(New String() {FolderBrowserDialog1.SelectedPath, "Carpeta"}))
                End Select
                CheckedListBox1.Items.Add(FolderBrowserDialog1.SelectedPath)
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                MsgBox("There are no driver packages in the specified folder", vbOKOnly + vbCritical, Label1.Text)
                            Case "ESN"
                                MsgBox("No hay paquetes de controladores en la carpeta espcificada", vbOKOnly + vbCritical, Label1.Text)
                        End Select
                    Case 1
                        MsgBox("There are no driver packages in the specified folder", vbOKOnly + vbCritical, Label1.Text)
                    Case 2
                        MsgBox("No hay paquetes de controladores en la carpeta espcificada", vbOKOnly + vbCritical, Label1.Text)
                End Select
            End If
            Cursor = Cursors.Arrow
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count = 1 Then
            Button3.Enabled = True
            Button4.Enabled = True
        Else
            Button3.Enabled = False
            Button4.Enabled = False
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If ListView1.FocusedItem.Text <> "" Then
            If CheckedListBox1.Items.Contains(ListView1.FocusedItem.Text) Then
                CheckedListBox1.Items.RemoveAt(CheckedListBox1.FindStringExact(ListView1.FocusedItem.Text))
            End If
            ListView1.Items.Remove(ListView1.FocusedItem)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ListView1.Items.Clear()
        CheckedListBox1.Items.Clear()
        Button3.Enabled = False
        Button4.Enabled = False
    End Sub

    Private Sub AddDrivers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Add drivers"
                        Label1.Text = Text
                        Label2.Text = "Please specify the drivers to add by using the buttons below or by dropping them to the list below:"
                        Label3.Text = "You can let the program scan the driver folders present on the list below recursively and add them as well. To do so, tick the entries you'd like to be scanned:"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        Button1.Text = "Add file..."
                        Button2.Text = "Add folder..."
                        Button3.Text = "Remove all entries"
                        Button4.Text = "Remove selected entry"
                        CheckBox1.Text = "Force installation of unsigned drivers"
                        CheckBox2.Text = "Commit image after adding drivers"
                        GroupBox1.Text = "Driver files"
                        GroupBox2.Text = "Driver folders"
                        GroupBox3.Text = "Options"
                        ListView1.Columns(0).Text = "File/Folder"
                        ListView1.Columns(1).Text = "Type"
                        OpenFileDialog1.Title = "Specify the driver package to add"
                        FolderBrowserDialog1.Description = "Specify the folder containing driver packages. You will then be able to specify if it needs to be scanned recursively:"
                    Case "ESN"
                        Text = "Añadir controladores"
                        Label1.Text = Text
                        Label2.Text = "Especifique los controladores a añadir usando los botones de abajo o colocándolos en la lista de abajo:"
                        Label3.Text = "Puede dejar que el programa escanee las carpetas de controladores presentes en la lista de abajo de forma recursiva y añadirlos también. Para hacerlo, marque las entradas que le gustaría que fuesen escaneadas:"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        Button1.Text = "Añadir archivo..."
                        Button2.Text = "Añadir carpeta..."
                        Button3.Text = "Eliminar todas las entradas"
                        Button4.Text = "Eliminar entrada seleccionada"
                        CheckBox1.Text = "Forzar instalación de controladores no firmados"
                        CheckBox2.Text = "Guardar imagen tras añadir controladores"
                        GroupBox1.Text = "Archivos de controladores"
                        GroupBox2.Text = "Carpetas de controladores"
                        GroupBox3.Text = "Opciones"
                        ListView1.Columns(0).Text = "Archivo/Carpeta"
                        ListView1.Columns(1).Text = "Tipo"
                        OpenFileDialog1.Title = "Especifique el paquete de controlador a añadir"
                        FolderBrowserDialog1.Description = "Especifique la carpeta que contiene paquetes de controladores. Luego podrá especificar si necesita ser escaneada de forma recursiva:"
                End Select
            Case 1
                Text = "Add drivers"
                Label1.Text = Text
                Label2.Text = "Please specify the drivers to add by using the buttons below or by dropping them to the list below:"
                Label3.Text = "You can let the program scan the driver folders present on the list below recursively and add them as well. To do so, tick the entries you'd like to be scanned:"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                Button1.Text = "Add file..."
                Button2.Text = "Add folder..."
                Button3.Text = "Remove all entries"
                Button4.Text = "Remove selected entry"
                CheckBox1.Text = "Force installation of unsigned drivers"
                CheckBox2.Text = "Commit image after adding drivers"
                GroupBox1.Text = "Driver files"
                GroupBox2.Text = "Driver folders"
                GroupBox3.Text = "Options"
                ListView1.Columns(0).Text = "File/Folder"
                ListView1.Columns(1).Text = "Type"
                OpenFileDialog1.Title = "Specify the driver package to add"
                FolderBrowserDialog1.Description = "Specify the folder containing driver packages. You will then be able to specify if it needs to be scanned recursively:"
            Case 2
                Text = "Añadir controladores"
                Label1.Text = Text
                Label2.Text = "Especifique los controladores a añadir usando los botones de abajo o colocándolos en la lista de abajo:"
                Label3.Text = "Puede dejar que el programa escanee las carpetas de controladores presentes en la lista de abajo de forma recursiva y añadirlos también. Para hacerlo, marque las entradas que le gustaría que fuesen escaneadas:"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                Button1.Text = "Añadir archivo..."
                Button2.Text = "Añadir carpeta..."
                Button3.Text = "Eliminar todas las entradas"
                Button4.Text = "Eliminar entrada seleccionada"
                CheckBox1.Text = "Forzar instalación de controladores no firmados"
                CheckBox2.Text = "Guardar imagen tras añadir controladores"
                GroupBox1.Text = "Archivos de controladores"
                GroupBox2.Text = "Carpetas de controladores"
                GroupBox3.Text = "Opciones"
                ListView1.Columns(0).Text = "Archivo/Carpeta"
                ListView1.Columns(1).Text = "Tipo"
                OpenFileDialog1.Title = "Especifique el paquete de controlador a añadir"
                FolderBrowserDialog1.Description = "Especifique la carpeta que contiene paquetes de controladores. Luego podrá especificar si necesita ser escaneada de forma recursiva:"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
            GroupBox3.ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
            CheckedListBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
            GroupBox3.ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
            CheckedListBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListView1.ForeColor = ForeColor
        CheckedListBox1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub ListView1_DragEnter(sender As Object, e As DragEventArgs) Handles ListView1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ListView1_DragDrop(sender As Object, e As DragEventArgs) Handles ListView1.DragDrop
        Dim PackageFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each PkgFile In PackageFiles
            If Not File.GetAttributes(PkgFile) = FileAttributes.Directory And Not Path.GetExtension(PkgFile).EndsWith("inf") Then Continue For
            If File.GetAttributes(PkgFile) = FileAttributes.Directory Then
                Cursor = Cursors.WaitCursor
                If My.Computer.FileSystem.GetFiles(PkgFile, FileIO.SearchOption.SearchAllSubDirectories, "*.inf").Count < 0 Then
                    Continue For
                End If
                Cursor = Cursors.Arrow
            End If
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            ListView1.Items.Add(New ListViewItem(New String() {PkgFile, If(File.GetAttributes(PkgFile) = FileAttributes.Directory, "Folder", "File")}))
                        Case "ESN"
                            ListView1.Items.Add(New ListViewItem(New String() {PkgFile, If(File.GetAttributes(PkgFile) = FileAttributes.Directory, "Carpeta", "Archivo")}))
                    End Select
                Case 1
                    ListView1.Items.Add(New ListViewItem(New String() {PkgFile, If(File.GetAttributes(PkgFile) = FileAttributes.Directory, "Folder", "File")}))
                Case 2
                    ListView1.Items.Add(New ListViewItem(New String() {PkgFile, If(File.GetAttributes(PkgFile) = FileAttributes.Directory, "Carpeta", "Archivo")}))
            End Select
            If File.GetAttributes(PkgFile) = FileAttributes.Directory Then CheckedListBox1.Items.Add(PkgFile)
        Next
    End Sub
End Class
