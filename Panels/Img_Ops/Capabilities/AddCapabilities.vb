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
            ProgressPanel.capAdditionLastId = ListView1.CheckedItems(capCount - 1).SubItems(0).Text
            If CheckBox1.Checked Then
                If RichTextBox1.Text <> "" Then
                    If Directory.Exists(RichTextBox1.Text) Then
                        ProgressPanel.capAdditionSource = RichTextBox1.Text         ' Don't know if it would work on cases where it begins with "wim:\"
                    Else
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENG"
                                        MsgBox("The specified source directory does not exist in the file system. Make sure it exists and try again.", vbOKOnly + vbCritical, Label1.Text)
                                    Case "ESN"
                                        MsgBox("El directorio de origen especificado no existe en el sistema de archivos. Asegúrese de que existe e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                                End Select
                            Case 1
                                MsgBox("The specified source directory does not exist in the file system. Make sure it exists and try again.", vbOKOnly + vbCritical, Label1.Text)
                            Case 2
                                MsgBox("El directorio de origen especificado no existe en el sistema de archivos. Asegúrese de que existe e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
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
        Try
            ' It is actually getting stuff from the registry, as changes in this group policy add/edit a registry key. Change this if it's not accurate,
            ' as the documentation doesn't specify which group policy is detected
            Dim capGPOSourceRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Servicing", False)
            Dim capGPOSource As String = capGPOSourceRk.GetValue("LocalSourcePath").ToString()
            capGPOSourceRk.Close()
            If capGPOSource.StartsWith("wim:\", StringComparison.OrdinalIgnoreCase) Then
                TextBoxSourcePanel.Visible = False
                WimFileSourcePanel.Visible = True
                Dim parts() As String = capGPOSource.Split(":")
                Label3.Text = parts(parts.Length - 1)
                Label5.Text = parts(1).Replace("\", "").Trim() & ":" & parts(2)
                If Label5.Text.EndsWith(":" & parts(parts.Length - 1)) Then Label5.Text = Label5.Text.Replace(":" & parts(parts.Length - 1), "").Trim()
            End If
        Catch ex As Exception
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            MsgBox("Could not gather source from group policy. Reason:" & CrLf & CrLf & ex.ToString(), vbOKOnly + vbCritical, Label1.Text)
                        Case "ESN"
                            MsgBox("No se pudo recopilar el origen de las políticas de grupo. Razón:" & CrLf & CrLf & ex.ToString(), vbOKOnly + vbCritical, Label1.Text)
                    End Select
                Case 1
                    MsgBox("Could not gather source from group policy. Reason:" & CrLf & CrLf & ex.ToString(), vbOKOnly + vbCritical, Label1.Text)
                Case 2
                    MsgBox("No se pudo recopilar el origen de las políticas de grupo. Razón:" & CrLf & CrLf & ex.ToString(), vbOKOnly + vbCritical, Label1.Text)
            End Select
        End Try
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
End Class
