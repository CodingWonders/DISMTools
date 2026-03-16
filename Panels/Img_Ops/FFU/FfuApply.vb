Imports System.Windows.Forms
Imports System.IO
Imports System.Management
Imports DISMTools.Utilities

Public Class FfuApply

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        If TextBox1.Text = "" Or Not File.Exists(TextBox1.Text) Then
            DynaLog.LogMessage("Either no image file has been specified or it does not exist in the file system.")
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("The specified image file is not valid. Please specify a valid image and try again.", vbOKOnly + vbCritical, Label1.Text)
                        Case "ESN"
                            MsgBox("El archivo de imagen especificado no es válido. Especifique una imagen válida e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                        Case "FRA"
                            MsgBox("Le fichier image spécifié n'est pas valide. Veuillez spécifier une image valide et réessayer.", vbOKOnly + vbCritical, Label1.Text)
                        Case "PTB", "PTG"
                            MsgBox("O ficheiro de imagem especificado não é válido. Especifique uma imagem válida e tente novamente.", vbOKOnly + vbCritical, Label1.Text)
                        Case "ITA"
                            MsgBox("Il file immagine specificato non è valido. Specificare un'immagine valida e riprovare.", vbOKOnly + vbCritical, Label1.Text)
                    End Select
                Case 1
                    MsgBox("The specified image file is not valid. Please specify a valid image and try again.", vbOKOnly + vbCritical, Label1.Text)
                Case 2
                    MsgBox("El archivo de imagen especificado no es válido. Especifique una imagen válida e inténtelo de nuevo.", vbOKOnly + vbCritical, Label1.Text)
                Case 3
                    MsgBox("Le fichier image spécifié n'est pas valide. Veuillez spécifier une image valide et réessayer.", vbOKOnly + vbCritical, Label1.Text)
                Case 4
                    MsgBox("O ficheiro de imagem especificado não é válido. Especifique uma imagem válida e tente novamente.", vbOKOnly + vbCritical, Label1.Text)
                Case 5
                    MsgBox("Il file immagine specificato non è valido. Specificare un'immagine valida e riprovare.", vbOKOnly + vbCritical, Label1.Text)
            End Select
            Exit Sub
        End If
        ProgressPanel.FFUApplicationSourceImg = TextBox1.Text
        ProgressPanel.FFUApplicationDestDrive = TextBox2.Text
        If CheckBox4.Checked Then
            ProgressPanel.FFUApplicationSFUPattern = Path.GetDirectoryName(TextBox1.Text) & "\" & TextBox4.Text & "*.swm"
        Else
            ProgressPanel.FFUApplicationSFUPattern = ""
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 2
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Private Sub UseMountedImgBtn_Click(sender As Object, e As EventArgs) Handles UseMountedImgBtn.Click
        TextBox1.Text = MainForm.SourceImg
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ApplicationDriveSpecifier.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            TextBox2.Text = ApplicationDriveSpecifier.SelectedDriveId
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ScanSFUPattern(TextBox4.Text)
    End Sub

    Sub ScanSFUPattern(PatternName As String)
        DynaLog.LogMessage("Preparing to scan files with the specified pattern...")
        DynaLog.LogMessage("- Scan pattern: " & PatternName)
        ListBox1.Items.Clear()
        If TextBox1.Text = "" Or PatternName = "" Then
            DynaLog.LogMessage("Either no source image file has been specified or no pattern has been specified.")
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("Please specify a source FFU file. This will let you use the SFU files for later image application", vbOKOnly + vbCritical, "Apply an image")
                            ToolStripStatusLabel1.Text = "This naming pattern returns " & ListBox1.Items.Count & " SFU files"
                        Case "ESN"
                            MsgBox("Especifique el arhivo FFU de origen. Esto le permitirá usar los archivos SFU para la aplicación posterior de la imagen", vbOKOnly + vbCritical, "Aplicar una imagen")
                            ToolStripStatusLabel1.Text = "Esta nomenclatura de patrón devuelve " & ListBox1.Items.Count & " archivos SFU"
                        Case "FRA"
                            MsgBox("Veuillez indiquer un fichier FFU original. Cela vous permettra d'utiliser les fichiers SFU pour une application d'image ultérieure.", vbOKOnly + vbCritical, "Appliquer une image")
                            ToolStripStatusLabel1.Text = "Ce modèle de dénomination renvoie " & ListBox1.Items.Count & " fichiers SFU"
                        Case "PTB", "PTG"
                            MsgBox("Especifique um ficheiro FFU de origem. Isto permitir-lhe-á utilizar os ficheiros SFU para uma aplicação de imagem posterior", vbOKOnly + vbCritical, "Aplicar uma imagem")
                            ToolStripStatusLabel1.Text = "Este padrão de nomenclatura devolve " & ListBox1.Items.Count & " ficheiros SFU"
                        Case "ITA"
                            MsgBox("Specificare un file FFU di origine. In questo modo sarà possibile utilizzare i file SFU per una successiva applicazione di immagini", vbOKOnly + vbCritical, "Applica un'immagine")
                            ToolStripStatusLabel1.Text = "Questo modello di denominazione restituisce " & ListBox1.Items.Count & " file SFU"
                    End Select
                Case 1
                    MsgBox("Please specify a source FFU file. This will let you use the SFU files for later image application", vbOKOnly + vbCritical, "Apply an image")
                    ToolStripStatusLabel1.Text = "This naming pattern returns " & ListBox1.Items.Count & " SFU files"
                Case 2
                    MsgBox("Especifique el arhivo FFU de origen. Esto le permitirá usar los archivos SFU para la aplicación posterior de la imagen", vbOKOnly + vbCritical, "Aplicar una imagen")
                    ToolStripStatusLabel1.Text = "Esta nomenclatura de patrón devuelve " & ListBox1.Items.Count & " archivos SFU"
                Case 3
                    MsgBox("Veuillez indiquer un fichier FFU original. Cela vous permettra d'utiliser les fichiers SFU pour une application d'image ultérieure.", vbOKOnly + vbCritical, "Appliquer une image")
                    ToolStripStatusLabel1.Text = "Ce modèle de dénomination renvoie " & ListBox1.Items.Count & " fichiers SFU"
                Case 4
                    MsgBox("Especifique um ficheiro FFU de origem. Isto permitir-lhe-á utilizar os ficheiros SFU para uma aplicação de imagem posterior", vbOKOnly + vbCritical, "Aplicar uma imagem")
                    ToolStripStatusLabel1.Text = "Este padrão de nomenclatura devolve " & ListBox1.Items.Count & " ficheiros SFU"
                Case 5
                    MsgBox("Specificare un file FFU di origine. In questo modo sarà possibile utilizzare i file SFU per una successiva applicazione di immagini", vbOKOnly + vbCritical, "Applica un'immagine")
                    ToolStripStatusLabel1.Text = "Questo modello di denominazione restituisce " & ListBox1.Items.Count & " file SFU"
            End Select
            Beep()
            Exit Sub
        End If
        DynaLog.LogMessage("Scanning SFU files with given pattern...")
        For Each sfuFile In My.Computer.FileSystem.GetFiles(Path.GetDirectoryName(TextBox1.Text), FileIO.SearchOption.SearchTopLevelOnly, "*.sfu")
            If Path.GetFileNameWithoutExtension(sfuFile).StartsWith(PatternName) Then
                ListBox1.Items.Add(Path.GetFileName(sfuFile))
            End If
        Next
        DynaLog.LogMessage("Pattern search results: " & ListBox1.Items.Count)
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        ToolStripStatusLabel1.Text = "This naming pattern returns " & ListBox1.Items.Count & " SFU files"
                    Case "ESN"
                        ToolStripStatusLabel1.Text = "Esta nomenclatura de patrón devuelve " & ListBox1.Items.Count & " archivos SFU"
                    Case "FRA"
                        ToolStripStatusLabel1.Text = "Ce modèle de dénomination renvoie " & ListBox1.Items.Count & " fichiers SFU"
                    Case "PTB", "PTG"
                        ToolStripStatusLabel1.Text = "Este padrão de nomenclatura devolve " & ListBox1.Items.Count & " ficheiros SFU"
                    Case "ITA"
                        ToolStripStatusLabel1.Text = "Questo modello di denominazione restituisce " & ListBox1.Items.Count & " file SFU"
                End Select
            Case 1
                ToolStripStatusLabel1.Text = "This naming pattern returns " & ListBox1.Items.Count & " SFU files"
            Case 2
                ToolStripStatusLabel1.Text = "Esta nomenclatura de patrón devuelve " & ListBox1.Items.Count & " archivos SFU"
            Case 3
                ToolStripStatusLabel1.Text = "Ce modèle de dénomination renvoie " & ListBox1.Items.Count & " fichiers SFU"
            Case 4
                ToolStripStatusLabel1.Text = "Este padrão de nomenclatura devolve " & ListBox1.Items.Count & " ficheiros SFU"
            Case 5
                ToolStripStatusLabel1.Text = "Questo modello di denominazione restituisce " & ListBox1.Items.Count & " file SFU"
        End Select
        If ListBox1.Items.Count <= 0 Then Beep()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        TextBox4.Text = Path.GetFileNameWithoutExtension(TextBox1.Text)
        ScanSFUPattern(TextBox4.Text)
    End Sub

    Private Sub FfuApply_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        RichTextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox4.BackColor = CurrentTheme.SectionBackgroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox3.ForeColor = CurrentTheme.ForegroundColor
        GroupBox4.ForeColor = CurrentTheme.ForegroundColor
        ListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        StatusStrip1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = ForeColor
        RichTextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        ListBox1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            ImageTaskHeader1.Visible = True
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        ToolStripStatusLabel1.Text = "Please specify the naming pattern of the SFU files"
                    Case "ESN"
                        ToolStripStatusLabel1.Text = "Especifique la nomenclatura del patrón de los archivos SFU"
                    Case "FRA"
                        ToolStripStatusLabel1.Text = "Veuillez spécifier le modèle de dénomination des fichiers SFU"
                    Case "PTB", "PTG"
                        ToolStripStatusLabel1.Text = "Especifique o padrão de nomenclatura dos ficheiros SFU"
                    Case "ITA"
                        ToolStripStatusLabel1.Text = "Specificare il modello di denominazione dei file SFU"
                End Select
            Case 1
                ToolStripStatusLabel1.Text = "Please specify the naming pattern of the SFU files"
            Case 2
                ToolStripStatusLabel1.Text = "Especifique la nomenclatura del patrón de los archivos SFU"
            Case 3
                ToolStripStatusLabel1.Text = "Veuillez spécifier le modèle de dénomination des fichiers SFU"
            Case 4
                ToolStripStatusLabel1.Text = "Especifique o padrão de nomenclatura dos ficheiros SFU"
            Case 5
                ToolStripStatusLabel1.Text = "Specificare il modello di denominazione dei file SFU"
        End Select
        If MainForm.SourceImg = "N/A" Or Not File.Exists(MainForm.SourceImg) Or MainForm.OnlineManagement Or MainForm.OfflineManagement Then
            UseMountedImgBtn.Enabled = False
        Else
            UseMountedImgBtn.Enabled = True
        End If
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        RichTextBox1.Clear()
        Try
            Dim SelectedDriveMO As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT Description, Manufacturer, Model, PNPDeviceID, Size, Status, Partitions FROM Win32_DiskDrive WHERE DeviceID LIKE {0}{1}{0}", Quote, TextBox2.Text.Replace("\", "\\")))
            If SelectedDriveMO IsNot Nothing Then
                RichTextBox1.Text = String.Format("  - Model: {1}{0}" &
                                                  "  - Manufacturer: {2}{0}" &
                                                  "  - Description: {3}{0}" &
                                                  "  - Device ID (Plug-and-Play): {4}{0}" &
                                                  "  - Partitions: {5}{0}" &
                                                  "  - Size: {6} bytes (~{7}){0}" &
                                                  "  - Status: {8}",
                                                  Environment.NewLine, WMIHelper.GetObjectValue(SelectedDriveMO(0), "Model"),
                                                                       WMIHelper.GetObjectValue(SelectedDriveMO(0), "Manufacturer"),
                                                                       WMIHelper.GetObjectValue(SelectedDriveMO(0), "Description"),
                                                                       WMIHelper.GetObjectValue(SelectedDriveMO(0), "PNPDeviceId"),
                                                                       WMIHelper.GetObjectValue(SelectedDriveMO(0), "Partitions"),
                                                                       WMIHelper.GetObjectValue(SelectedDriveMO(0), "Size"),
                                                                       Converters.BytesToReadableSize(WMIHelper.GetObjectValue(SelectedDriveMO(0), "Size")),
                                                                       WMIHelper.GetObjectValue(SelectedDriveMO(0), "Status"))
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        SFUFilePanel.Enabled = CheckBox4.Checked = True
    End Sub
End Class
