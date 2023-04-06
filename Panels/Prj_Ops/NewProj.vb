Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class NewProj

    Dim IsReqField1Valid As Boolean
    Dim IsReqField2Valid As Boolean

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not Directory.Exists(TextBox2.Text) Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENG"
                            If MsgBox("The directory: " & CrLf & Quote & TextBox2.Text & Quote & CrLf & "does not exist. Do you want to create it?", vbYesNo + vbQuestion, "Create a new project") = MsgBoxResult.Yes Then
                                Try
                                    Directory.CreateDirectory(TextBox2.Text)
                                Catch ex As Exception
                                    MsgBox("We could not create the project directory for you due to: " & CrLf & ex.ToString() & "; " & ex.Message, vbOKOnly + vbCritical, "Create a new project")
                                    Exit Sub
                                End Try
                            Else
                                Exit Sub
                            End If
                        Case "ESN"
                            If MsgBox("El directorio: " & CrLf & Quote & TextBox2.Text & Quote & CrLf & "no existe. ¿Desea crearlo?", vbYesNo + vbQuestion, "Crear un nuevo proyecto") = MsgBoxResult.Yes Then
                                Try
                                    Directory.CreateDirectory(TextBox2.Text)
                                Catch ex As Exception
                                    MsgBox("No pudimos crear el directorio del proyecto porque: " & CrLf & ex.ToString() & "; " & ex.Message, vbOKOnly + vbCritical, "Crear un nuevo proyecto")
                                    Exit Sub
                                End Try
                            Else
                                Exit Sub
                            End If
                    End Select
                Case 1
                    If MsgBox("The directory: " & CrLf & Quote & TextBox2.Text & Quote & CrLf & "does not exist. Do you want to create it?", vbYesNo + vbQuestion, "Create a new project") = MsgBoxResult.Yes Then
                        Try
                            Directory.CreateDirectory(TextBox2.Text)
                        Catch ex As Exception
                            MsgBox("We could not create the project directory for you due to: " & CrLf & ex.ToString() & "; " & ex.Message, vbOKOnly + vbCritical, "Create a new project")
                            Exit Sub
                        End Try
                    Else
                        Exit Sub
                    End If
                Case 2
                    If MsgBox("El directorio: " & CrLf & Quote & TextBox2.Text & Quote & CrLf & "no existe. ¿Desea crearlo?", vbYesNo + vbQuestion, "Crear un nuevo proyecto") = MsgBoxResult.Yes Then
                        Try
                            Directory.CreateDirectory(TextBox2.Text)
                        Catch ex As Exception
                            MsgBox("No pudimos crear el directorio del proyecto porque: " & CrLf & ex.ToString() & "; " & ex.Message, vbOKOnly + vbCritical, "Crear un nuevo proyecto")
                            Exit Sub
                        End Try
                    Else
                        Exit Sub
                    End If
            End Select
        End If
        ProgressPanel.OperationNum = 0
        ProgressPanel.projName = TextBox1.Text
        ProgressPanel.projPath = TextBox2.Text
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        If MainForm.isProjectLoaded Then
            MainForm.UnloadDTProj(False, True, False)
        End If
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub NewProj_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Create a new project"
                        Label1.Text = Text
                        Label2.Text = "Please specify the options to create a new project:"
                        Label3.Text = "Name*:"
                        Label4.Text = "Location*:"
                        Label5.Text = "The fields that end in * are required"
                        Button1.Text = "Browse..."
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        GroupBox1.Text = "Project"
                        FolderBrowserDialog1.Description = "Please select a folder to store this project:"
                    Case "ESN"
                        Text = "Crear un nuevo proyecto"
                        Label1.Text = Text
                        Label2.Text = "Especifique las opciones para crear un nuevo proyecto:"
                        Label3.Text = "Nombre*:"
                        Label4.Text = "Ubicación*:"
                        Label5.Text = "Los campos que terminen en * son necesarios"
                        Button1.Text = "Examinar..."
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        GroupBox1.Text = "Proyecto"
                        FolderBrowserDialog1.Description = "Seleccione una carpeta donde almacenar este proyecto:"
                End Select
            Case 1
                Text = "Create a new project"
                Label1.Text = Text
                Label2.Text = "Please specify the options to create a new project:"
                Label3.Text = "Name*:"
                Label4.Text = "Location*:"
                Label5.Text = "The fields that end in * are required"
                Button1.Text = "Browse..."
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                GroupBox1.Text = "Project"
                FolderBrowserDialog1.Description = "Please select a folder to store this project:"
            Case 2
                Text = "Crear un nuevo proyecto"
                Label1.Text = Text
                Label2.Text = "Especifique las opciones para crear un nuevo proyecto:"
                Label3.Text = "Nombre*:"
                Label4.Text = "Ubicación*:"
                Label5.Text = "Los campos que terminen en * son necesarios"
                Button1.Text = "Examinar..."
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                GroupBox1.Text = "Proyecto"
                FolderBrowserDialog1.Description = "Seleccione una carpeta donde almacenar este proyecto:"
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            GroupBox1.ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            GroupBox1.ForeColor = Color.Black
        End If
        TextBox2.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FolderBrowserDialog1.ShowDialog()
        If DialogResult.OK And FolderBrowserDialog1.SelectedPath <> "" Then
            TextBox2.Text = FolderBrowserDialog1.SelectedPath
            IsReqField2Valid = True
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" Then
            If TextBox1.Text.Equals("con", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("CON", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("aux", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("AUX", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("prn", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("PRN", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("nul", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("NUL", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com1", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com2", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com3", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com4", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com5", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com6", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com7", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com8", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("com9", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM1", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM2", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM3", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM4", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM5", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM6", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM7", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM8", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("COM9", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt1", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt2", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt3", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt4", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt5", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt6", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt7", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt8", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("lpt9", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT1", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT2", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT3", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT4", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT5", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT6", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT7", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT8", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Equals("LPT9", StringComparison.OrdinalIgnoreCase) Or _
                TextBox1.Text.Contains("<") Or _
                TextBox1.Text.Contains(">") Or _
                TextBox1.Text.Contains(":") Or _
                TextBox1.Text.Contains(Quote) Or _
                TextBox1.Text.Contains("/") Or _
                TextBox1.Text.Contains("\") Or _
                TextBox1.Text.Contains("|") Or _
                TextBox1.Text.Contains("?") Or _
                TextBox1.Text.Contains("*") Then
                IsReqField1Valid = False
            Else
                IsReqField1Valid = True
            End If
        Else
            IsReqField1Valid = False
        End If
        CheckReqFields()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If TextBox2.Text.Contains("con") Or _
                TextBox2.Text.Contains("CON") Or _
                TextBox2.Text.Contains("aux") Or _
                TextBox2.Text.Contains("AUX") Or _
                TextBox2.Text.Contains("prn") Or _
                TextBox2.Text.Contains("PRN") Or _
                TextBox2.Text.Contains("nul") Or _
                TextBox2.Text.Contains("NUL") Or _
                TextBox2.Text.Contains("com1") Or _
                TextBox2.Text.Contains("com2") Or _
                TextBox2.Text.Contains("com3") Or _
                TextBox2.Text.Contains("com4") Or _
                TextBox2.Text.Contains("com5") Or _
                TextBox2.Text.Contains("com6") Or _
                TextBox2.Text.Contains("com7") Or _
                TextBox2.Text.Contains("com8") Or _
                TextBox2.Text.Contains("com9") Or _
                TextBox2.Text.Contains("COM1") Or _
                TextBox2.Text.Contains("COM2") Or _
                TextBox2.Text.Contains("COM3") Or _
                TextBox2.Text.Contains("COM4") Or _
                TextBox2.Text.Contains("COM5") Or _
                TextBox2.Text.Contains("COM6") Or _
                TextBox2.Text.Contains("COM7") Or _
                TextBox2.Text.Contains("COM8") Or _
                TextBox2.Text.Contains("COM9") Or _
                TextBox2.Text.Contains("lpt1") Or _
                TextBox2.Text.Contains("lpt2") Or _
                TextBox2.Text.Contains("lpt3") Or _
                TextBox2.Text.Contains("lpt4") Or _
                TextBox2.Text.Contains("lpt5") Or _
                TextBox2.Text.Contains("lpt6") Or _
                TextBox2.Text.Contains("lpt7") Or _
                TextBox2.Text.Contains("lpt8") Or _
                TextBox2.Text.Contains("lpt9") Or _
                TextBox2.Text.Contains("LPT1") Or _
                TextBox2.Text.Contains("LPT2") Or _
                TextBox2.Text.Contains("LPT3") Or _
                TextBox2.Text.Contains("LPT4") Or _
                TextBox2.Text.Contains("LPT5") Or _
                TextBox2.Text.Contains("LPT6") Or _
                TextBox2.Text.Contains("LPT7") Or _
                TextBox2.Text.Contains("LPT8") Or _
                TextBox2.Text.Contains("LPT9") Or _
                TextBox2.Text.Contains("<") Or _
                TextBox2.Text.Contains(">") Or _
                TextBox2.Text.Contains("|") Or _
                TextBox2.Text.Contains("?") Or _
                TextBox2.Text.Contains("*") Then
            IsReqField2Valid = False
        Else
            IsReqField2Valid = True
        End If
        CheckReqFields()
    End Sub

    Sub CheckReqFields()
        If IsReqField1Valid And IsReqField2Valid Then
            OK_Button.Enabled = True
        Else
            OK_Button.Enabled = False
        End If
    End Sub
End Class
