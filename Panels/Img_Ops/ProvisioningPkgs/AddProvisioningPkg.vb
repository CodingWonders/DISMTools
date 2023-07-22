Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class AddProvisioningPkg

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If TextBox1.Text <> "" Then
            If File.Exists(TextBox1.Text) Then
                ProgressPanel.ppkgAdditionPackagePath = TextBox1.Text
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                MsgBox("The specified provisioning package does not exist. Make sure it exists in the file system and try again.", vbOKOnly + vbCritical, Label1.Text)
                            Case "ESN"
                                MsgBox("El paquete de aprovisionamiento especificado no existe. Asegúrese de que exista en el sistema de archivos e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                        End Select
                    Case 1
                        MsgBox("The specified provisioning package does not exist. Make sure it exists in the file system and try again.", vbOKOnly + vbCritical, Label1.Text)
                    Case 2
                        MsgBox("El paquete de aprovisionamiento especificado no existe. Asegúrese de que exista en el sistema de archivos e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                End Select
                Exit Sub
            End If
            If TextBox2.Text <> "" And File.Exists(TextBox2.Text) Then
                ProgressPanel.ppkgAdditionCatalogPath = TextBox2.Text
            ElseIf TextBox2.Text <> "" And Not File.Exists(TextBox2.Text) Then
                Dim msg As String = ""
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENG"
                                msg = "The catalog file specified doesn't exist. We won't use this file if you proceed." & CrLf & CrLf & "Do you want to continue?"
                            Case "ESN"
                                msg = "El archivo de catálogo especificado no existe. No usaremos este archivo si continúa." & CrLf & CrLf & "¿Desea continuar?"
                        End Select
                    Case 1
                        msg = "The catalog file specified doesn't exist. We won't use this file if you proceed." & CrLf & CrLf & "Do you want to continue?"
                    Case 2
                        msg = "El archivo de catálogo especificado no existe. No usaremos este archivo si continúa." & CrLf & CrLf & "¿Desea continuar?"
                End Select
                If MsgBox(msg, vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.No Then
                    Exit Sub
                End If
            Else
                ProgressPanel.ppkgAdditionCatalogPath = ""
            End If
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            MsgBox("No provisioning package has been specified. Please specify a provisioning package to add and try again.", vbOKOnly + vbCritical, Label1.Text)
                        Case "ESN"
                            MsgBox("No se ha especificado un paquete de aprovisionamiento. Especifique un paquete de aprovisionamiento a añadir e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                    End Select
                Case 1
                    MsgBox("No provisioning package has been specified. Please specify a provisioning package to add and try again.", vbOKOnly + vbCritical, Label1.Text)
                Case 2
                    MsgBox("No se ha especificado un paquete de aprovisionamiento. Especifique un paquete de aprovisionamiento a añadir e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
            End Select
            Exit Sub
        End If
        ProgressPanel.ppkgAdditionCommit = If(CheckBox1.Checked, True, False)
        ProgressPanel.OperationNum = 33
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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        OpenFileDialog2.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub OpenFileDialog2_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog2.FileOk
        TextBox2.Text = OpenFileDialog2.FileName
    End Sub

    Private Sub TextBox1_DragEnter(sender As Object, e As DragEventArgs) Handles TextBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub TextBox1_DragDrop(sender As Object, e As DragEventArgs) Handles TextBox1.DragDrop
        TextBox1.Text = e.Data.GetData(DataFormats.FileDrop)
    End Sub

    Private Sub TextBox2_DragEnter(sender As Object, e As DragEventArgs) Handles TextBox2.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub TextBox2_DragDrop(sender As Object, e As DragEventArgs) Handles TextBox2.DragDrop
        TextBox2.Text = e.Data.GetData(DataFormats.FileDrop)
    End Sub

    Private Sub AddProvisioningPkg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class
