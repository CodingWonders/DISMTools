Imports System.Windows.Forms
Imports System.IO

Public Class AddPackageDlg

    Public CheckedCount As Integer
    Public pkgCount As Integer
    Public pkgs(65535) As String        ' This is hard-coded. If you have more than 65535 selected packages, the program will throw an exception

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ProgressPanel.MountDir = MainForm.MountDir
        ProgressPanel.pkgSource = TextBox1.Text
        pkgCount = CheckedListBox1.CheckedItems.Count
        If RadioButton1.Checked Then
            ProgressPanel.pkgAdditionOp = 0
        Else
            ProgressPanel.pkgAdditionOp = 1
            ProgressPanel.pkgCount = pkgCount
        End If
        If CheckBox1.Checked Then
            ProgressPanel.pkgIgnoreApplicabilityChecks = True
        Else
            ProgressPanel.pkgIgnoreApplicabilityChecks = False
        End If
        If CheckBox2.Checked Then
            ProgressPanel.pkgPreventIfPendingOnline = True
        Else
            ProgressPanel.pkgPreventIfPendingOnline = False
        End If
        If CheckBox3.Checked And Not MainForm.OnlineManagement Then
            ProgressPanel.imgCommitAfterOps = True
        Else
            ProgressPanel.imgCommitAfterOps = False
        End If
        If ProgressPanel.pkgAdditionOp = 1 Then
            If CheckedListBox1.CheckedItems.Count <= 0 Then
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                MessageBox.Show(MainForm, "Please select packages to add, and try again. You can also continue with letting DISM scan applicable packages", "No packages selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Case "ESN"
                                MessageBox.Show(MainForm, "Seleccione paquetes a añadir, e inténtelo de nuevo. También puede continuar dejando que DISM escanee paquetes aplicables", "No hay paquetes seleccionados", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Select
                    Case 1
                        MessageBox.Show(MainForm, "Please select packages to add, and try again. You can also continue with letting DISM scan applicable packages", "No packages selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Case 2
                        MessageBox.Show(MainForm, "Seleccione paquetes a añadir, e inténtelo de nuevo. También puede continuar dejando que DISM escanee paquetes aplicables", "No hay paquetes seleccionados", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Select
            Else
                If pkgCount > 65535 Then
                    MessageBox.Show(MainForm, "Right now, due to program limitations, you can select 65535 packages or less.", "Current program limitation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    Try
                        For x As Integer = 0 To pkgCount - 1
                            pkgs(x) = CheckedListBox1.CheckedItems(x).ToString()
                        Next
                        For x = 0 To pkgs.Length
                            ProgressPanel.pkgs(x) = pkgs(x)
                        Next
                    Catch ex As Exception

                    End Try
                End If
                If ProgressPanel.pkgAdditionOp = 1 Then
                    ProgressPanel.pkgLastCheckedPackageName = CheckedListBox1.CheckedItems(pkgCount - 1).ToString()
                End If
                ProgressPanel.OperationNum = 26
                Visible = False
                ProgressPanel.ShowDialog(MainForm)
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        ElseIf ProgressPanel.pkgAdditionOp = 0 Then
            ProgressPanel.OperationNum = 26
            Visible = False
            ProgressPanel.ShowDialog(MainForm)
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog()
        If DialogResult.OK And FolderBrowserDialog1.SelectedPath <> "" Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
            ScanBW.RunWorkerAsync()
        End If
    End Sub

    Sub GatherPackages(FolderToScan As String)
        CheckedListBox1.Items.Clear()
        Cursor = Cursors.WaitCursor
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Label4.Text = "Scanning directory for packages. Please wait..."
                    Case "ESN"
                        Label4.Text = "Escaneando directorio por paquetes. Espere..."
                End Select
            Case 1
                Label4.Text = "Scanning directory for packages. Please wait..."
            Case 2
                Label4.Text = "Escaneando directorio por paquetes. Espere..."
        End Select
        Refresh()
        ' TODO: show CheckedListBox items without full path
        Try
            For Each CabPkg In My.Computer.FileSystem.GetFiles(FolderToScan, FileIO.SearchOption.SearchAllSubDirectories, "*.cab")
                If CabPkg.Contains("MsuExtract") Then
                    ' CAB files stored in MsuExtract are skipped, as they come from MSU files. Skip these items and continue loop
                    Continue For
                End If
                CheckedListBox1.Items.Add(CabPkg)
            Next
            For Each MsuPkg In My.Computer.FileSystem.GetFiles(FolderToScan, FileIO.SearchOption.SearchAllSubDirectories, "*.msu")
                CheckedListBox1.Items.Add(MsuPkg)
            Next
        Catch ex As Exception
            For Each CabPkg In My.Computer.FileSystem.GetFiles(FolderToScan, FileIO.SearchOption.SearchTopLevelOnly, "*.cab")
                If CabPkg.Contains("MsuExtract") Then
                    ' CAB files stored in MsuExtract are skipped, as they come from MSU files. Skip these items and continue loop
                    Continue For
                End If
                CheckedListBox1.Items.Add(CabPkg)
            Next
            For Each MsuPkg In My.Computer.FileSystem.GetFiles(FolderToScan, FileIO.SearchOption.SearchTopLevelOnly, "*.msu")
                CheckedListBox1.Items.Add(MsuPkg)
            Next
        End Try
        CountItems()
        Cursor = Cursors.Arrow
    End Sub

    Sub CountItems()
        If CheckedCount > CheckedListBox1.CheckedItems.Count Then
            Do Until CheckedCount = CheckedListBox1.CheckedItems.Count
                CheckedCount -= 1
            Loop
        ElseIf CheckedCount < 0 Then
            Do Until CheckedCount = 0
                CheckedCount += 1
            Loop
        End If
        If CheckedListBox1.Items.Count = 0 Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            Label4.Text = "This folder does not contain any packages. Please use a different source and try again"
                        Case "ESN"
                            Label4.Text = "Esta carpeta no contiene ningún paquete. Utilice un origen diferente e inténtelo de nuevo"
                    End Select
                Case 1
                    Label4.Text = "This folder does not contain any packages. Please use a different source and try again"
                Case 2
                    Label4.Text = "Esta carpeta no contiene ningún paquete. Utilice un origen diferente e inténtelo de nuevo"
            End Select
            Beep()
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            Label4.Text = "This folder contains " & CheckedListBox1.Items.Count & " package" & If(CheckedListBox1.Items.Count = 1, ".", "s.")
                        Case "ESN"
                            Label4.Text = "Esta carpeta contiene " & CheckedListBox1.Items.Count & " paquete" & If(CheckedListBox1.Items.Count = 1, ".", "s.")
                    End Select
                Case 1
                    Label4.Text = "This folder contains " & CheckedListBox1.Items.Count & " package" & If(CheckedListBox1.Items.Count = 1, ".", "s.")
                Case 2
                    Label4.Text = "Esta carpeta contiene " & CheckedListBox1.Items.Count & " paquete" & If(CheckedListBox1.Items.Count = 1, ".", "s.")
            End Select
        End If
    End Sub

    Private Sub AddPackageDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Add packages"
                        Label1.Text = Text
                        Label2.Text = "Package source:"
                        Label3.Text = "Package operation:"
                        Button1.Text = "Browse..."
                        Button2.Text = "Select all"
                        Button3.Text = "Select none"
                        Cancel_Button.Text = "Cancel"
                        OK_Button.Text = "OK"
                        RadioButton1.Text = "Scan folder recursively for packages"
                        RadioButton2.Text = "Choose which packages to add:"
                        CheckBox1.Text = "Ignore applicability checks (not recommended)"
                        CheckBox2.Text = "Skip package installation if online operations are pending"
                        CheckBox3.Text = "Commit image after adding packages"
                        FolderBrowserDialog1.Description = "Specify the folder containing CAB or MSU packages:"
                        GroupBox1.Text = "Packages"
                        GroupBox2.Text = "Options"
                    Case "ESN"
                        Text = "Añadir paquetes"
                        Label1.Text = Text
                        Label2.Text = "Origen:"
                        Label3.Text = "Operación de paquetes:"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Todos los paquetes"
                        Button3.Text = "Ningún paquete"
                        Cancel_Button.Text = "Cancelar"
                        OK_Button.Text = "Aceptar"
                        RadioButton1.Text = "Escanear carpeta de forma recursiva por paquetes"
                        RadioButton2.Text = "Elegir qué paquetes añadir:"
                        CheckBox1.Text = "Ignorar comprobaciones de aplicabilidad (no recomendado)"
                        CheckBox2.Text = "Omitir instalación de paquetes si operaciones en línea deben realizarse"
                        CheckBox3.Text = "Guardar imagen tras añadir paquetes"
                        FolderBrowserDialog1.Description = "Especifique la carpeta que contenga paquetes CAB o MSU:"
                        GroupBox1.Text = "Paquetes"
                        GroupBox2.Text = "Opciones"
                End Select
            Case 1
                Text = "Add packages"
                Label1.Text = Text
                Label2.Text = "Package source:"
                Label3.Text = "Package operation:"
                Button1.Text = "Browse..."
                Button2.Text = "Select all"
                Button3.Text = "Select none"
                Cancel_Button.Text = "Cancel"
                OK_Button.Text = "OK"
                RadioButton1.Text = "Scan folder recursively for packages"
                RadioButton2.Text = "Choose which packages to add:"
                CheckBox1.Text = "Ignore applicability checks (not recommended)"
                CheckBox2.Text = "Skip package installation if online operations are pending"
                CheckBox3.Text = "Commit image after adding packages"
                FolderBrowserDialog1.Description = "Specify the folder containing CAB or MSU packages:"
                GroupBox1.Text = "Packages"
                GroupBox2.Text = "Options"
            Case 2
                Text = "Añadir paquetes"
                Label1.Text = Text
                Label2.Text = "Origen:"
                Label3.Text = "Operación de paquetes:"
                Button1.Text = "Examinar..."
                Button2.Text = "Todos los paquetes"
                Button3.Text = "Ningún paquete"
                Cancel_Button.Text = "Cancelar"
                OK_Button.Text = "Aceptar"
                RadioButton1.Text = "Escanear carpeta de forma recursiva por paquetes"
                RadioButton2.Text = "Elegir qué paquetes añadir:"
                CheckBox1.Text = "Ignorar comprobaciones de aplicabilidad (no recomendado)"
                CheckBox2.Text = "Omitir instalación de paquetes si operaciones en línea deben realizarse"
                CheckBox3.Text = "Guardar imagen tras añadir paquetes"
                FolderBrowserDialog1.Description = "Especifique la carpeta que contenga paquetes CAB o MSU:"
                GroupBox1.Text = "Paquetes"
                GroupBox2.Text = "Opciones"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
            CheckedListBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
            CheckedListBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        CheckedListBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        Control.CheckForIllegalCrossThreadCalls = False
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        If CheckedListBox1.Items.Count = 0 Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            Label4.Text = "Please specify a directory where CAB or MSU files are located."
                        Case "ESN"
                            Label4.Text = "Especifique el directorio donde se encuentran archivos CAB o MSU."
                    End Select
                Case 1
                    Label4.Text = "Please specify a directory where CAB or MSU files are located."
                Case 2
                    Label4.Text = "Especifique el directorio donde se encuentran archivos CAB o MSU."
            End Select
        End If
        CheckBox3.Enabled = MainForm.OnlineManagement = False
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            Label4.Enabled = False
            CheckedListBox1.Enabled = False
            TableLayoutPanel2.Enabled = False
        Else
            Label4.Enabled = True
            CheckedListBox1.Enabled = True
            TableLayoutPanel2.Enabled = True
        End If
        If ProgressPanel.OperationNum = 26 Then
            pkgCount = CheckedCount
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For i As Integer = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, True)
            CheckedCount += 1
            CountItems()
        Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        For i As Integer = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, False)
            CheckedCount -= 1
            CountItems()
        Next
        DialogResult = Windows.Forms.DialogResult.None
    End Sub

    Private Sub ScanBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ScanBW.DoWork
        GatherPackages(TextBox1.Text)
    End Sub

    Private Sub CheckedListBox1_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles CheckedListBox1.ItemCheck
        CheckedCount = CheckedListBox1.CheckedItems.Count
    End Sub
End Class
