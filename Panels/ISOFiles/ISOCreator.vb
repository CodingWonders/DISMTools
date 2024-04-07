Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism

Public Class ISOCreator

    Dim ImageInfoCollection As DismImageInfoCollection
    Dim ISOMsg As String = ""
    Dim success As Boolean

    Private Sub ISOCreator_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            TextBox3.BackColor = Color.FromArgb(31, 31, 31)
            ComboBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            TextBox3.BackColor = Color.FromArgb(238, 238, 242)
            ComboBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        GroupBox2.ForeColor = ForeColor
        ComboBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        SaveFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
    End Sub

    Sub GetImageInfo(ImageFile As String)
        If MainForm.MountedImageDetectorBW.IsBusy Then
            MainForm.MountedImageDetectorBW.CancelAsync()
            While MainForm.MountedImageDetectorBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(500)
            End While
        End If
        MainForm.WatcherTimer.Enabled = False
        If MainForm.WatcherBW.IsBusy Then MainForm.WatcherBW.CancelAsync()
        While MainForm.WatcherBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        Try
            DismApi.Initialize(DismLogLevel.LogErrors)
            ImageInfoCollection = DismApi.GetImageInfo(ImageFile)
            For Each ImageInfo As DismImageInfo In ImageInfoCollection
                TextBox2.Text = "Images in selected file: " & ImageInfoCollection.Count & CrLf & CrLf &
                                " - Image " & ImageInfoCollection.IndexOf(ImageInfo) + 1 & " of " & ImageInfoCollection.Count & CrLf &
                                "   - Image name: " & ImageInfo.ImageName & CrLf &
                                "   - Image description: " & ImageInfo.ImageDescription & CrLf &
                                "   - Image version: " & ImageInfo.ProductVersion.ToString() & CrLf & CrLf
            Next
        Catch ex As Exception
            Dim msg As String = ""
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "Could not gather information of this image file. Reason:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case "ESN"
                            msg = "No pudimos obtener información de este archivo de imagen. Razón:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case "FRA"
                            msg = "Impossible de recueillir des informations sur ce fichier de l'image. Raison :" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                        Case "PTB", "PTG"
                            msg = "Não foi possível recolher informações sobre este ficheiro de imagem. Motivo:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                    End Select
                Case 1
                    msg = "Could not gather information of this image file. Reason:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 2
                    msg = "No pudimos obtener información de este archivo de imagen. Razón:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 3
                    msg = "Impossible de recueillir des informations sur ce fichier de l'image. Raison :" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
                Case 4
                    msg = "Não foi possível recolher informações sobre este ficheiro de imagem. Motivo:" & CrLf & CrLf & ex.ToString() & " - " & ex.Message & " (HRESULT " & Hex(ex.HResult) & ")"
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, Label1.Text)
        Finally
            DismApi.Shutdown()
        End Try
        Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        TextBox3.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        If TextBox1.Text = "" OrElse Not File.Exists(TextBox1.Text) Then
            ISOMsg = "Either the source image file does not exist or you haven't provided any image file. Please specify a valid image file and try again."
            MsgBox(ISOMsg, vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If
        If TextBox3.Text = "" Then
            ISOMsg = "The target ISO hasn't been specified. Please specify a location for the ISO file and try again."
            MsgBox(ISOMsg, vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If
        If File.Exists(TextBox3.Text) Then
            ISOMsg = "The target ISO already exists. Do you want to replace it?"
            If MsgBox(ISOMsg, vbYesNo + vbQuestion, Label1.Text) = MsgBoxResult.Yes Then
                Try
                    File.Delete(TextBox3.Text)
                Catch ex As Exception
                    ' Could not delete ISO
                End Try
            Else
                Exit Sub
            End If
        End If
        OK_Button.Enabled = False
        Cancel_Button.Enabled = False
        GroupBox1.Enabled = False
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        BackgroundWorker1.ReportProgress(0)
        Dim ISOCreator As New Process()
        ISOCreator.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
        ISOCreator.StartInfo.WorkingDirectory = Application.StartupPath & "\bin\extps1\PE_Helper"
        ISOCreator.StartInfo.Arguments = Quote & Application.StartupPath & "\bin\extps1\PE_Helper\PE_Helper.ps1" & Quote & " -cmd StartPEGen -arch " & ComboBox1.SelectedItem & " -imgFile " & Quote & TextBox1.Text & Quote & " -isoPath " & Quote & TextBox3.Text & Quote
        ISOCreator.Start()
        ISOCreator.WaitForExit()
        success = (ISOCreator.ExitCode = 0)
        BackgroundWorker1.ReportProgress(100)
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        IdlePanel.Visible = False
        ISOProgressPanel.Visible = True
        If e.ProgressPercentage < 100 Then
            ProgressBar1.Style = ProgressBarStyle.Marquee
        Else
            ProgressBar1.Style = ProgressBarStyle.Blocks
        End If
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        If success Then
            MsgBox("The ISO file has been created successfully", vbOKOnly + vbInformation, Label1.Text)
        Else
            MsgBox("Failed to create the ISO file", vbOKOnly + vbExclamation, Label1.Text)
        End If
        OK_Button.Enabled = True
        Cancel_Button.Enabled = True
        GroupBox1.Enabled = True
        IdlePanel.Visible = True
        ISOProgressPanel.Visible = False
    End Sub

    Private Sub ISOCreator_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If BackgroundWorker1.IsBusy Then
            e.Cancel = True
            Beep()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PopupImageManager.Location = Button2.PointToScreen(Point.Empty)
        If PopupImageManager.ShowDialog() = DialogResult.OK Then
            TextBox1.Text = PopupImageManager.selectedImgFile
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text <> "" And File.Exists(TextBox1.Text) Then
            GetImageInfo(TextBox1.Text)
        End If
    End Sub
End Class