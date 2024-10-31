Imports System.IO
Imports Microsoft.Dism
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Win32
Imports DISMTools.Utilities

Public Class AutoReloadForm

    Dim SuccessfulReloads, FailedReloads, SkippedReloads As Integer
    Dim ImgCount As Integer
    Dim message As String
    Dim mntMsg As String
    Dim fileMsg As String
    Dim ImgFiles As New List(Of String)
    Dim MountDirs As New List(Of String)
    Dim MountStatus As New List(Of DismMountStatus)

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        If MainForm.MountedImageDetectorBW.IsBusy Then MainForm.MountedImageDetectorBW.CancelAsync()
        While MainForm.MountedImageDetectorBW.IsBusy
            Application.DoEvents()
            Thread.Sleep(100)
        End While
        DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
        Dim MountedImages As DismMountedImageInfoCollection = DismApi.GetMountedImages()
        ImgCount = MountedImages.Count
        If MountedImages.Count > 0 Then
            For Each imageinfo As DismMountedImageInfo In MountedImages
                ImgFiles.Add(imageinfo.ImageFilePath)
                MountDirs.Add(imageinfo.MountPath)
                MountStatus.Add(imageinfo.MountStatus)
            Next
            fileMsg = ImgFiles(0)
            mntMsg = MountDirs(0)
        End If
        DismApi.Shutdown()
        BackgroundWorker1.ReportProgress(0)
        If MountDirs.Count > 0 Then
            Try
                DismApi.Initialize(DismLogLevel.LogErrors, Application.StartupPath & "\logs\dism.log")
                For x = 0 To Array.LastIndexOf(MountDirs.ToArray(), MountDirs.ToArray().Last)
                    fileMsg = ImgFiles(x)
                    mntMsg = MountDirs(x)
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            message = "Reloading mounted image... (succeeded: " & SuccessfulReloads & ", failed: " & FailedReloads & ", skipped: " & SkippedReloads & ")"
                        Case "ESN"
                            message = "Recargando imagen montada... (satisfactorias: " & SuccessfulReloads & ", fallidas: " & FailedReloads & ", omitidas: " & SkippedReloads & ")"
                        Case "FRA"
                            message = "Rechargement de l'image montée en cours... (avec succès : " & SuccessfulReloads & ", échoué : " & FailedReloads & ", ignoré : " & SkippedReloads & ")"
                        Case "PTB", "PTG"
                            message = "Recarregando imagem montada... (bem-sucedido: " & SuccessfulReloads & ", falhou: " & FailedReloads & ", ignorado: " & SkippedReloads & ")"
                        Case "ITA"
                            message = "Ricarica dell'immagine montata... (riuscita: " & SuccessfulReloads & ", failed: " & FailedReloads & ", saltato: " & SkippedReloads & ")"
                    End Select
                    BackgroundWorker1.ReportProgress((x / ImgCount) * 100)
                    Try
                        If MountStatus(x) = DismMountStatus.NeedsRemount Then
                            DismApi.RemountImage(MountDirs(x))
                            SuccessfulReloads += 1
                        Else
                            SkippedReloads += 1
                        End If
                    Catch ex As Exception
                        FailedReloads += 1
                    End Try
                Next
            Catch ex As Exception
                Debug.WriteLine("Could not remount all orphaned images. Reason:" & CrLf & ex.ToString())
            Finally
                DismApi.Shutdown()
            End Try
        End If
        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
            Case "ENU", "ENG"
                message = "This process has completed"
            Case "ESN"
                message = "Este proceso ha completado"
            Case "FRA"
                message = "Ce processus s'est achevé"
            Case "PTB", "PTG"
                message = "Este processo foi concluído"
            Case "ITA"
                message = "Questo processo è stato completato"
        End Select
        BackgroundWorker1.ReportProgress(100)
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        Select Case e.ProgressPercentage
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label2.Text = "Preparing to reload images..."
                    Case "ESN"
                        Label2.Text = "Preparándonos para recargar imágenes..."
                    Case "FRA"
                        Label2.Text = "Préparation du rechargement des images en cours..."
                    Case "PTB", "PTG"
                        Label2.Text = "A preparar o recarregamento de imagens..."
                    Case "ITA"
                        Label2.Text = "Preparazione per ricaricare le immagini..."
                End Select
            Case Else
                Label2.Text = message
        End Select
        imgFile.Text = fileMsg
        imgMtPnt.Text = mntMsg
        ProgressBar1.Style = ProgressBarStyle.Blocks
        ProgressBar1.Value = e.ProgressPercentage
        TaskbarHelper.SetIndicatorState(e.ProgressPercentage, Windows.Shell.TaskbarItemProgressState.Normal, Handle)
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Refresh()
        TaskbarHelper.SetIndicatorState(100, Windows.Shell.TaskbarItemProgressState.None, Handle)
        Application.DoEvents()
        Thread.Sleep(1000)
        Close()
    End Sub

    Private Sub AutoReloadForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim ColorModeRk As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Themes\Personalize")
            Select Case ColorModeRk.GetValue("AppsUseLightTheme").ToString()
                Case "0"
                    BackColor = Color.FromArgb(31, 31, 31)
                    ForeColor = Color.White
                Case "1"
                    BackColor = Color.FromArgb(238, 238, 242)
                    ForeColor = Color.Black
            End Select
            GroupBox1.ForeColor = ForeColor
            ColorModeRk.Close()
        Catch ex As Exception
            ' Continue
        End Try
        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
            Case "ENU", "ENG"
                Label1.Text = "Please wait while DISMTools reloads the servicing session of orphaned images. This can take some time. Once complete, this dialog will close."
                Label3.Text = "Image file:"
                Label4.Text = "Image mount point:"
                GroupBox1.Text = "Image information"
            Case "ESN"
                Label1.Text = "Espere mientras DISMTools recarga la sesión de servicio de las imágenes huérfanas. Esto puede llevar algo de tiempo. Una vez completado, este diálogo se cerrará."
                Label3.Text = "Archivo de imagen:"
                Label4.Text = "Punto de montaje de la imagen:"
                GroupBox1.Text = "Información de la imagen"
            Case "FRA"
                Label1.Text = "Veuillez patienter pendant que DISMTools recharge la session d'entretien des images orphelines. Cela peut prendre un certain temps. Une fois terminé, cette boîte de dialogue se fermera."
                Label3.Text = "Fichier image :"
                Label4.Text = "Point de montage de l'image :"
                GroupBox1.Text = "Informations sur l'image"
            Case "PTB", "PTG"
                Label1.Text = "Aguarde enquanto o DISMTools recarrega a sessão de manutenção de imagens órfãs. Isso pode levar algum tempo. Uma vez concluído, esta caixa de diálogo será fechada."
                Label3.Text = "Ficheiro de imagem:"
                Label4.Text = "Ponto de montagem da imagem:"
                GroupBox1.Text = "Informações sobre a imagem"
            Case "ITA"
                Label1.Text = "Attendere mentre DISMTools ricarica la sessione di assistenza delle immagini orfane. Questa operazione può richiedere del tempo. Una volta completata, questa finestra di dialogo si chiuderà."
                Label3.Text = "File immagine:"
                Label4.Text = "Punto di montaggio dell'immagine:"
                GroupBox1.Text = "Informazioni sull'immagine"
        End Select
        TaskbarHelper.SetIndicatorState(0, Windows.Shell.TaskbarItemProgressState.Indeterminate, Handle)
        Thread.Sleep(2000)
        BackgroundWorker1.RunWorkerAsync()
    End Sub
End Class