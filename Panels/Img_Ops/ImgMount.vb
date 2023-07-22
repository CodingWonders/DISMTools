Imports System.IO
Imports System.Windows.Forms
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism

Public Class ImgMount

    Dim WimInfo As Process
    Dim WimStr As String
    Dim IsReqField1Valid As Boolean
    Dim IsReqField2Valid As Boolean
    Dim IsReqField3Valid As Boolean
    Public SourceImg As String
    Public ImgIndex As Integer
    Public MountDir As String
    Public isReadOnly As Boolean
    Public isOptimized As Boolean
    Public isIntegrityTested As Boolean
    Dim IndexOperationMode As Integer       ' 0: Get-ImageInfo (Win8+); 1: Get-WimInfo (Win7)
    Dim DismVerChecker As FileVersionInfo

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not Directory.Exists(TextBox2.Text) Then
            MountOpDirCreationDialog.ShowDialog()
            If MountOpDirCreationDialog.DialogResult = Windows.Forms.DialogResult.Yes Then
                Try
                    Directory.CreateDirectory(TextBox2.Text)
                Catch ex As Exception
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENG"
                                    MsgBox("Could not create mount directory. Reason: " & ex.ToString() & "; " & ex.Message, MsgBoxStyle.OkOnly + vbCritical, "Mount an image")
                                Case "ESN"
                                    MsgBox("No se pudo crear el directorio de montaje. Razón: " & ex.ToString() & "; " & ex.Message, MsgBoxStyle.OkOnly + vbCritical, "Montar una imagen")
                            End Select
                        Case 1
                            MsgBox("Could not create mount directory. Reason: " & ex.ToString() & "; " & ex.Message, MsgBoxStyle.OkOnly + vbCritical, "Mount an image")
                        Case 2
                            MsgBox("No se pudo crear el directorio de montaje. Razón: " & ex.ToString() & "; " & ex.Message, MsgBoxStyle.OkOnly + vbCritical, "Montar una imagen")
                    End Select
                    Exit Sub
                End Try
            ElseIf MountOpDirCreationDialog.DialogResult = Windows.Forms.DialogResult.No Then
                Exit Sub
            End If
        End If
        TextBox1.Text = ProgressPanel.SourceImg
        NumericUpDown1.Value = ImgIndex
        TextBox2.Text = ProgressPanel.MountDir
        If CheckBox1.Checked Then
            ProgressPanel.isReadOnly = True
        Else
            ProgressPanel.isReadOnly = False
        End If
        If CheckBox3.Checked Then
            ProgressPanel.isOptimized = True
        Else
            ProgressPanel.isOptimized = False
        End If
        If CheckBox4.Checked Then
            ProgressPanel.isIntegrityTested = True
        Else
            ProgressPanel.isIntegrityTested = False
        End If
        'ProgressPanel.SourceImg = SourceImg
        ProgressPanel.ImgIndex = ImgIndex
        'ProgressPanel.MountDir = MountDir
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 15
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgMount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Mount an image"
                        Label1.Text = Text
                        Label2.Text = "Please specify the options to mount an image:"
                        Label3.Text = "Image file*:"
                        Label4.Text = "NOTE: if you want to mount an ESD file, you need to convert it to a WIM file first"
                        Label6.Text = "Mount directory*:"
                        Label7.Text = "Index*:"
                        Label11.Text = "The fields that end in * are required"
                        GroupBox1.Text = "Source"
                        GroupBox2.Text = "Destination"
                        GroupBox3.Text = "Options"
                        Button1.Text = "Browse..."
                        Button2.Text = "Browse..."
                        Button5.Text = "Use defaults"
                        Cancel_Button.Text = "Cancel"
                        OK_Button.Text = "OK"
                        ListView1.Columns(0).Text = "Index"
                        ListView1.Columns(1).Text = "Image name"
                        ListView1.Columns(2).Text = "Image description"
                        ListView1.Columns(3).Text = "Image version"
                        CheckBox1.Text = "Mount with read only permissions"
                        CheckBox3.Text = "Optimize mount times"
                        CheckBox4.Text = "Check image integrity"
                    Case "ESN"
                        Text = "Montar una imagen"
                        Label1.Text = Text
                        Label2.Text = "Especifique las opciones para montar una imagen:"
                        Label3.Text = "Archivo de imagen*:"
                        Label4.Text = "NOTA: si desea montar un archivo ESD, necesita convertirlo a un archivo WIM en primer lugar"
                        Label6.Text = "Directorio de montaje*:"
                        Label7.Text = "Índice*:"
                        Label11.Text = "Los campos que terminen en * son necesarios"
                        GroupBox1.Text = "Origen"
                        GroupBox2.Text = "Destino"
                        GroupBox3.Text = "Opciones"
                        Button1.Text = "Examinar..."
                        Button2.Text = "Examinar..."
                        Button5.Text = "Predeterminados"
                        Cancel_Button.Text = "Cancelar"
                        OK_Button.Text = "Aceptar"
                        ListView1.Columns(0).Text = "Índice"
                        ListView1.Columns(1).Text = "Nombre de imagen"
                        ListView1.Columns(2).Text = "Descripción de la imagen"
                        ListView1.Columns(3).Text = "Versión de la imagen"
                        CheckBox1.Text = "Montar con permisos de solo lectura"
                        CheckBox3.Text = "Optimizar tiempos de montaje"
                        CheckBox4.Text = "Comprobar integridad de la imagen"
                End Select
            Case 1
                Text = "Mount an image"
                Label1.Text = Text
                Label2.Text = "Please specify the options to mount an image:"
                Label3.Text = "Image file*:"
                Label4.Text = "NOTE: if you want to mount an ESD file, you need to convert it to a WIM file first"
                Label6.Text = "Mount directory*:"
                Label7.Text = "Index*:"
                Label11.Text = "The fields that end in * are required"
                GroupBox1.Text = "Source"
                GroupBox2.Text = "Destination"
                GroupBox3.Text = "Options"
                Button1.Text = "Browse..."
                Button2.Text = "Browse..."
                Button5.Text = "Use defaults"
                Cancel_Button.Text = "Cancel"
                OK_Button.Text = "OK"
                ListView1.Columns(0).Text = "Index"
                ListView1.Columns(1).Text = "Image name"
                ListView1.Columns(2).Text = "Image description"
                ListView1.Columns(3).Text = "Image version"
                CheckBox1.Text = "Mount with read only permissions"
                CheckBox3.Text = "Optimize mount times"
                CheckBox4.Text = "Check image integrity"
            Case 2
                Text = "Montar una imagen"
                Label1.Text = Text
                Label2.Text = "Especifique las opciones para montar una imagen:"
                Label3.Text = "Archivo de imagen*:"
                Label4.Text = "NOTA: si desea montar un archivo ESD, necesita convertirlo a un archivo WIM en primer lugar"
                Label6.Text = "Directorio de montaje*:"
                Label7.Text = "Índice*:"
                Label11.Text = "Los campos que terminen en * son necesarios"
                GroupBox1.Text = "Origen"
                GroupBox2.Text = "Destino"
                GroupBox3.Text = "Opciones"
                Button1.Text = "Examinar..."
                Button2.Text = "Examinar..."
                Button5.Text = "Predeterminados"
                Cancel_Button.Text = "Cancelar"
                OK_Button.Text = "Aceptar"
                ListView1.Columns(0).Text = "Índice"
                ListView1.Columns(1).Text = "Nombre de imagen"
                ListView1.Columns(2).Text = "Descripción de la imagen"
                ListView1.Columns(3).Text = "Versión de la imagen"
                CheckBox1.Text = "Montar con permisos de solo lectura"
                CheckBox3.Text = "Optimizar tiempos de montaje"
                CheckBox4.Text = "Comprobar integridad de la imagen"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            NumericUpDown1.BackColor = Color.FromArgb(31, 31, 31)
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
            GroupBox3.ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            NumericUpDown1.BackColor = Color.FromArgb(238, 238, 242)
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
            GroupBox3.ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        NumericUpDown1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        ListView1.ForeColor = ForeColor
        DismVerChecker = FileVersionInfo.GetVersionInfo(MainForm.DismExe)
        Select Case DismVerChecker.ProductMajorPart
            Case 6
                Select Case DismVerChecker.ProductMinorPart
                    Case 1
                        IndexOperationMode = 1
                        FileSpecDialog.Filter = "WIM files|*.wim"
                    Case Is >= 2
                        IndexOperationMode = 0
                End Select
            Case 10
                IndexOperationMode = 0
        End Select
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        FileSpecDialog.ShowDialog()
    End Sub

    Private Sub FileSpecDialog_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles FileSpecDialog.FileOk
        TextBox1.Text = FileSpecDialog.FileName
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs)
        ListView1.Items.Clear()
        Width = 800
        CenterToParent()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        FolderBrowserDialog1.ShowDialog()
        If DialogResult.OK Then
            TextBox2.Text = FolderBrowserDialog1.SelectedPath
        Else
            TextBox2.Text = ""
        End If
        GetFields()
    End Sub

    Sub GetIndexes(ImgFile As String)
        Try
            ListView1.Items.Clear()
            DismApi.Initialize(DismLogLevel.LogErrors)
            Dim imgInfoCollection As DismImageInfoCollection = DismApi.GetImageInfo(ImgFile)
            NumericUpDown1.Maximum = imgInfoCollection.Count
            For Each imgInfo As DismImageInfo In imgInfoCollection
                ListView1.Items.Add(New ListViewItem(New String() {imgInfo.ImageIndex, imgInfo.ImageName, imgInfo.ImageDescription, imgInfo.ProductVersion.ToString()}))
            Next
            DismApi.Shutdown()
        Catch ex As AccessViolationException
            If IndexOperationMode = 0 Then
                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", _
                                  "@echo off" & CrLf & _
                                  "dism /English /get-imageinfo /imagefile=" & ImgFile & " | find /c " & Quote & "Index" & Quote & " > .\indexcount", ASCII)
            ElseIf IndexOperationMode = 1 Then
                File.WriteAllText(Application.StartupPath & "\bin\exthelpers\temp.bat", _
                                  "@echo off" & CrLf & _
                                  "dism /English /get-wiminfo /wimfile=" & ImgFile & " | find /c " & Quote & "Index" & Quote & " > .\indexcount", ASCII)
            End If
            Process.Start(Application.StartupPath & "\bin\exthelpers\temp.bat").WaitForExit()
            MainForm.imgIndexCount = CInt(My.Computer.FileSystem.ReadAllText(Application.StartupPath & "\indexcount"))
            NumericUpDown1.Maximum = MainForm.imgIndexCount
            File.Delete(Application.StartupPath & "\indexcount")
        End Try
    End Sub

    Sub GetFields()
        IsReqField3Valid = True
        If TextBox1.Text = "" Then
            If ProgressPanel.OperationNum = 15 Then
                TextBox1.Text = ProgressPanel.SourceImg
            Else
                IsReqField1Valid = False
            End If
        Else
            If File.Exists(TextBox1.Text) Then
                IsReqField1Valid = True
                ProgressPanel.SourceImg = TextBox1.Text
                GetIndexes(TextBox1.Text)
            Else
                IsReqField1Valid = False
            End If
        End If
        If TextBox2.Text = "" Then
            If ProgressPanel.OperationNum = 15 Then
                TextBox2.Text = ProgressPanel.MountDir
            Else
                IsReqField1Valid = False
            End If
            IsReqField2Valid = False
        Else
            If Directory.Exists(TextBox2.Text) Then
                IsReqField2Valid = True
                ProgressPanel.MountDir = TextBox2.Text
            End If
        End If
        If IsReqField1Valid And IsReqField2Valid And IsReqField3Valid Then
            OK_Button.Enabled = True
        Else
            OK_Button.Enabled = False
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        GetFields()
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        GetFields()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If ProgressPanel.OperationNum = 0 Then
            If ProgressPanel.projPath = "" Then
                TextBox2.Text = MainForm.projPath & "\mount"
            Else
                TextBox2.Text = ProgressPanel.projPath & ProgressPanel.projName & "\mount"
            End If
        Else
            TextBox2.Text = MainForm.projPath & "\mount"
        End If
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        ImgIndex = NumericUpDown1.Value
        ProgressPanel.ImgIndex = NumericUpDown1.Value
    End Sub

    Private Sub ImgMount_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
    End Sub
End Class
