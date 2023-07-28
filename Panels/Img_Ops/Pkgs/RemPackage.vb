Imports System.Windows.Forms
Imports System.IO

Public Class RemPackage

    Public pkgRemovalCount As Integer
    Public pkgRemovalNames(65535) As String
    Public pkgRemovalFiles(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MainForm.MountDir
        ProgressPanel.pkgRemovalSource = TextBox1.Text
        If RadioButton1.Checked Then
            pkgRemovalCount = CheckedListBox1.CheckedItems.Count
            ProgressPanel.pkgRemovalOp = 0
            ProgressPanel.pkgRemovalCount = pkgRemovalCount
            If CheckedListBox1.CheckedItems.Count <= 0 Then
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                MessageBox.Show(MainForm, "Please select packages to remove, and try again.", "No packages selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Case "ESN"
                                MessageBox.Show(MainForm, "Seleccione paquetes a eliminar, e inténtelo de nuevo.", "No se han seleccionado paquetes", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Select
                    Case 1
                        MessageBox.Show(MainForm, "Please select packages to remove, and try again.", "No packages selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Case 2
                        MessageBox.Show(MainForm, "Seleccione paquetes a eliminar, e inténtelo de nuevo.", "No se han seleccionado paquetes", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Select
                Exit Sub
            Else
                If pkgRemovalCount > 65535 Then
                    MessageBox.Show(MainForm, "Right now, due to program limitations, you can select 65535 packages or less.", "Current program limitation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                Else
                    Try
                        For x As Integer = 0 To pkgRemovalCount - 1
                            pkgRemovalNames(x) = CheckedListBox1.CheckedItems(x).ToString()
                        Next
                        For x = 0 To pkgRemovalNames.Length
                            ProgressPanel.pkgRemovalNames(x) = pkgRemovalNames(x)
                        Next
                    Catch ex As Exception

                    End Try
                End If
                ProgressPanel.pkgRemovalLastName = CheckedListBox1.CheckedItems(pkgRemovalCount - 1).ToString()
            End If
        Else
            pkgRemovalCount = CheckedListBox2.CheckedItems.Count
            ProgressPanel.pkgRemovalOp = 1
            ProgressPanel.pkgRemovalCount = pkgRemovalCount
            If CheckedListBox2.CheckedItems.Count < 0 Then
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                MessageBox.Show(MainForm, "Please select packages to remove, and try again.", "No packages selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Case "ESN"
                                MessageBox.Show(MainForm, "Seleccione paquetes a eliminar, e inténtelo de nuevo.", "No se han seleccionado paquetes", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Select
                    Case 1
                        MessageBox.Show(MainForm, "Please select packages to remove, and try again.", "No packages selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Case 2
                        MessageBox.Show(MainForm, "Seleccione paquetes a eliminar, e inténtelo de nuevo.", "No se han seleccionado paquetes", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Select
                Exit Sub
            Else
                If pkgRemovalCount > 65535 Then
                    MessageBox.Show(MainForm, "Right now, due to program limitations, you can select 65535 packages or less.", "Current program limitation", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                Else
                    Try
                        For x As Integer = 0 To pkgRemovalCount - 1
                            pkgRemovalFiles(x) = CheckedListBox2.CheckedItems(x).ToString()
                        Next
                        For x = 0 To pkgRemovalFiles.Length
                            ProgressPanel.pkgRemovalFiles(x) = pkgRemovalFiles(x)
                        Next
                    Catch ex As Exception

                    End Try
                End If
                ProgressPanel.pkgRemovalLastFile = CheckedListBox2.CheckedItems(pkgRemovalCount - 1).ToString()
            End If
        End If
        ProgressPanel.OperationNum = 27
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub RemPackage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Remove packages"
                        Label1.Text = Text
                        Label3.Text = "Package source:"
                        Label4.Text = "NOTE: the program may show packages that weren't added in the first place. However, if a package is not added, the program will skip it."
                        GroupBox1.Text = "Package removal"
                        RadioButton1.Text = "Specify package names:"
                        RadioButton2.Text = "Specify package files:"
                        Button1.Text = "Browse..."
                        Cancel_Button.Text = "Cancel"
                        OK_Button.Text = "OK"
                        FolderBrowserDialog1.Description = "Please specify a package source:"
                    Case "ESN"
                        Text = "Eliminar paquetes"
                        Label1.Text = Text
                        Label3.Text = "Origen:"
                        Label4.Text = "NOTA: el programa podría mostrar paquetes que no se hayan añadido en primer lugar. Si un paquete no se ha añadido, el programa lo omitirá."
                        GroupBox1.Text = "Eliminación de paquetes"
                        RadioButton1.Text = "Especificar nombres de paquetes:"
                        RadioButton2.Text = "Especificar archivos de paquetes:"
                        Button1.Text = "Examinar..."
                        Cancel_Button.Text = "Cancelar"
                        OK_Button.Text = "Aceptar"
                        FolderBrowserDialog1.Description = "Especifique un origen de paquetes:"
                End Select
            Case 1
                Text = "Remove packages"
                Label1.Text = Text
                Label3.Text = "Package source:"
                Label4.Text = "NOTE: the program may show packages that weren't added in the first place. However, if a package is not added, the program will skip it."
                GroupBox1.Text = "Package removal"
                RadioButton1.Text = "Specify package names:"
                RadioButton2.Text = "Specify package files:"
                Button1.Text = "Browse..."
                Cancel_Button.Text = "Cancel"
                OK_Button.Text = "OK"
                FolderBrowserDialog1.Description = "Please specify a package source:"
            Case 2
                Text = "Eliminar paquetes"
                Label1.Text = Text
                Label3.Text = "Origen:"
                Label4.Text = "NOTA: el programa podría mostrar paquetes que no se hayan añadido en primer lugar. Si un paquete no se ha añadido, el programa lo omitirá."
                GroupBox1.Text = "Eliminación de paquetes"
                RadioButton1.Text = "Especificar nombres de paquetes:"
                RadioButton2.Text = "Especificar archivos de paquetes:"
                Button1.Text = "Examinar..."
                Cancel_Button.Text = "Cancelar"
                OK_Button.Text = "Aceptar"
                FolderBrowserDialog1.Description = "Especifique un origen de paquetes:"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            GroupBox1.ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            CheckedListBox1.BackColor = Color.FromArgb(31, 31, 31)
            CheckedListBox2.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            GroupBox1.ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            CheckedListBox1.BackColor = Color.FromArgb(238, 238, 242)
            CheckedListBox2.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        CheckedListBox1.ForeColor = ForeColor
        CheckedListBox2.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            CheckedListBox1.Enabled = True
            Label2.Enabled = True
            Label3.Enabled = False
            TextBox1.Enabled = False
            Button1.Enabled = False
            CheckedListBox2.Enabled = False
            Label4.Enabled = False
        Else
            CheckedListBox1.Enabled = False
            Label2.Enabled = False
            Label3.Enabled = True
            TextBox1.Enabled = True
            Button1.Enabled = True
            CheckedListBox2.Enabled = True
            Label4.Enabled = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog()
        If DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        Else
            TextBox1.Text = ""
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Try
            For Each cabFile In My.Computer.FileSystem.GetFiles(TextBox1.Text, FileIO.SearchOption.SearchAllSubDirectories, "*.cab")
                CheckedListBox2.Items.Add(cabFile)
            Next
        Catch ex As Exception
            Try
                For Each cabFile In My.Computer.FileSystem.GetFiles(TextBox1.Text, FileIO.SearchOption.SearchTopLevelOnly, "*.cab")
                    CheckedListBox2.Items.Add(cabFile)
                Next
            Catch ex2 As Exception
                Exit Try    ' Give up
            End Try
        End Try
        If CheckedListBox2.Items.Count <= 0 Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            MsgBox("We couldn't scan the package source for CAB files. Please try again.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "DISMTools")
                        Case "ESN"
                            MsgBox("No pudimos escanear el origen de paquetes por archivos CAB. Inténtelo de nuevo.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "DISMTools")
                    End Select
                Case 1
                    MsgBox("We couldn't scan the package source for CAB files. Please try again.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "DISMTools")
                Case 2
                    MsgBox("No pudimos escanear el origen de paquetes por archivos CAB. Inténtelo de nuevo.", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "DISMTools")
            End Select
        End If
    End Sub
End Class
