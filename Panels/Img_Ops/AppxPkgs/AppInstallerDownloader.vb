Imports System.Windows.Forms
Imports System.Net
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports DISMTools.Elements
Imports DISMTools.Utilities.Converters
Imports System.Xml
Imports System.Xml.Serialization
Imports System.ComponentModel
Imports System.Threading

Public Class AppInstallerDownloader

    Public AppInstallerFile As String
    Dim AppInstallerUri As String
    Dim Downloader As New WebClient()

    Dim Language As Integer
    Dim progress As String

    Dim downSpd As Long

    Private sw As Stopwatch = New Stopwatch()
    Private time As TimeSpan = New TimeSpan()

    Private Sub AppInstallerDownloader_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
        downUriLbl.Text = ""
        sw.Reset()
        sw.Start()
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Downloading application package..."
                        Label1.Text = "Please wait while DISMTools downloads the application package to add it to this image. This can take some time, depending on your network connection speed."
                        StatusLbl.Text = "Please wait..."
                        GroupBox1.Text = "Transfer details"
                        Label2.Text = "Download URL:"
                        downSpdLbl.Text = "Download speed: unknown"
                        downETALbl.Text = "Estimated time remaining: unknown"
                    Case "ESN"
                        Text = "Descargando paquete de aplicación..."
                        Label1.Text = "Espere mientras DISMTools descarga el paquete de aplicación para añadirlo a esta imagen. Esto puede llevar algo de tiempo, dependiendo de la velocidad de su conexión de red."
                        StatusLbl.Text = "Espere..."
                        GroupBox1.Text = "Detalles de la transferencia"
                        Label2.Text = "URL de descarga:"
                        downSpdLbl.Text = "Velocidad de descarga: desconocida"
                        downETALbl.Text = "Tiempo restante estimado: desconocido"
                    Case "FRA"
                        Text = "Téléchargement du paquet de l'application en cours..."
                        Label1.Text = "Veuillez patienter pendant que DISMTools télécharge le paquet d'application pour l'ajouter à cette image. Cela peut prendre un certain temps, en fonction de la vitesse de votre connexion réseau."
                        StatusLbl.Text = "Veuillez patienter..."
                        GroupBox1.Text = "Détails du transfert"
                        Label2.Text = "URL de téléchargement :"
                        downSpdLbl.Text = "Vitesse de téléchargement : inconnue"
                        downETALbl.Text = "Temps restant estimé : inconnu"
                    Case "PTB", "PTG"
                        Text = "Descarregando o pacote da aplicação..."
                        Label1.Text = "Aguarde enquanto o DISMTools baixa o pacote de aplicativos para adicioná-lo a esta imagem. Isso pode levar algum tempo, dependendo da velocidade da conexão de rede."
                        StatusLbl.Text = "Aguarde..."
                        GroupBox1.Text = "Detalhes da transferência"
                        Label2.Text = "URL de transferência:"
                        downSpdLbl.Text = "Velocidade de transferência: desconhecida"
                        downETALbl.Text = "Tempo estimado restante: desconhecido"
                    Case "ITA"
                        Text = " Scaricamento del pacchetto dell'applicazione..."
                        Label1.Text = "Attendere che DISMTools scarichi il pacchetto applicativo per aggiungerlo a questa immagine. Questa operazione può richiedere del tempo, a seconda della velocità della connessione di rete."
                        StatusLbl.Text = "Attendere..."
                        GroupBox1.Text = "Dettagli del trasferimento"
                        Label2.Text = "URL di scaricamento:"
                        downSpdLbl.Text = "Velocità di scaricamento: sconosciuta"
                        downETALbl.Text = "Tempo stimato rimanente: sconosciuto"
                End Select
            Case 1
                Text = "Downloading application package..."
                Label1.Text = "Please wait while DISMTools downloads the application package to add it to this image. This can take some time, depending on your network connection speed."
                StatusLbl.Text = "Please wait..."
                GroupBox1.Text = "Transfer details"
                Label2.Text = "Download URL:"
                downSpdLbl.Text = "Download speed: unknown"
                downETALbl.Text = "Estimated time remaining: unknown"
            Case 2
                Text = "Descargando paquete de aplicación..."
                Label1.Text = "Espere mientras DISMTools descarga el paquete de aplicación para añadirlo a esta imagen. Esto puede llevar algo de tiempo, dependiendo de la velocidad de su conexión de red."
                StatusLbl.Text = "Espere..."
                GroupBox1.Text = "Detalles de la transferencia"
                Label2.Text = "URL de descarga:"
                downSpdLbl.Text = "Velocidad de descarga: desconocida"
                downETALbl.Text = "Tiempo restante estimado: desconocido"
            Case 3
                Text = "Téléchargement du paquet de l'application en cours..."
                Label1.Text = "Veuillez patienter pendant que DISMTools télécharge le paquet d'application pour l'ajouter à cette image. Cela peut prendre un certain temps, en fonction de la vitesse de votre connexion réseau."
                StatusLbl.Text = "Veuillez patienter..."
                GroupBox1.Text = "Détails du transfert"
                Label2.Text = "URL de téléchargement :"
                downSpdLbl.Text = "Vitesse de téléchargement : inconnue"
                downETALbl.Text = "Temps restant estimé : inconnu"
            Case 4
                Text = "Descarregando o pacote da aplicação..."
                Label1.Text = "Aguarde enquanto o DISMTools baixa o pacote de aplicativos para adicioná-lo a esta imagem. Isso pode levar algum tempo, dependendo da velocidade da conexão de rede."
                StatusLbl.Text = "Aguarde..."
                GroupBox1.Text = "Detalhes da transferência"
                Label2.Text = "URL de transferência:"
                downSpdLbl.Text = "Velocidade de transferência: desconhecida"
                downETALbl.Text = "Tempo estimado restante: desconhecido"
            Case 5
                Text = " Scaricamento del pacchetto dell'applicazione..."
                Label1.Text = "Attendere che DISMTools scarichi il pacchetto applicativo per aggiungerlo a questa immagine. Questa operazione può richiedere del tempo, a seconda della velocità della connessione di rete."
                StatusLbl.Text = "Attendere..."
                GroupBox1.Text = "Dettagli del trasferimento"
                Label2.Text = "URL di scaricamento:"
                downSpdLbl.Text = "Velocità di scaricamento: sconosciuta"
                downETALbl.Text = "Tempo stimato rimanente: sconosciuto"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        GroupBox1.ForeColor = ForeColor
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        Language = MainForm.Language
        Visible = True
        If AppInstallerFile IsNot Nothing And File.Exists(AppInstallerFile) Then
            TaskbarHelper.SetIndicatorState(0, Windows.Shell.TaskbarItemProgressState.Indeterminate, MainForm.Handle)
            ' Create a reader and get the URL information, since .appinstaller files are XML
            Try
                Dim reader As New RichTextBox()
                reader.Text = File.ReadAllText(AppInstallerFile, UTF8)
                If reader.Text <> "" Then
                    ' Detect if a URL property is present
                    If reader.Text.Contains("MainBundle") Then
                        ' Go through each line and find the URL
                        For x = 0 To reader.Lines.Count - 1
                            If reader.Lines(x).Contains("MainBundle") Then
                                Dim serializer As New XmlSerializer(GetType(AppInstallers))
                                Using tReader As TextReader = New StringReader(reader.Lines(x))
                                    Dim propertyLine As String = ""
                                    If Not reader.Lines(x).EndsWith(" />") Then
                                        Dim Properties As New List(Of String)
                                        Properties.Add(If(reader.Lines(x).EndsWith("MainBundle"), reader.Lines(x).Replace(" ", "").Trim(), reader.Lines(x)))
                                        Properties.Add(reader.Lines(x + 1).Replace(" ", "").Trim())
                                        Properties.Add(reader.Lines(x + 2).Replace(" ", "").Trim())
                                        Properties.Add(reader.Lines(x + 3).Replace(" ", "").Trim())
                                        Properties.Add(reader.Lines(x + 4).Replace(" ", "").Trim())
                                        propertyLine = String.Join(" ", Properties)
                                        Dim id = CType(serializer.Deserialize(New StringReader(propertyLine)), AppInstallers)
                                        AppInstallerUri = id.MainBundleUri
                                    Else
                                        Using ContentReader As XmlReader = XmlReader.Create(tReader)
                                            Dim id = CType(serializer.Deserialize(ContentReader), AppInstallers)
                                            AppInstallerUri = id.MainBundleUri
                                        End Using
                                    End If
                                End Using
                                Exit For
                            End If
                        Next
                    End If
                End If

                ' Detect if a URL has been detected and download it
                If AppInstallerUri <> "" Then
                    downUriLbl.Text = AppInstallerUri
                    BackgroundWorker1.RunWorkerAsync()
                End If
            Catch ex As Exception
                Close()
            End Try
        End If
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        AddHandler Downloader.DownloadProgressChanged, AddressOf WebClient_DownloadProgressChanged
        AddHandler Downloader.DownloadFileCompleted, AddressOf WebClient_DownloadFileCompleted
        Downloader.DownloadFileAsync(New Uri(AppInstallerUri), Path.GetDirectoryName(AppInstallerFile) & "\" & Path.GetFileNameWithoutExtension(AppInstallerFile) & Path.GetExtension(AppInstallerUri))
    End Sub

#Region "WebClient event handlers"

    Private Sub WebClient_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs)
        ProgressBar1.Value = e.ProgressPercentage
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        progress = "Downloading main application package... (" & BytesToReadableSize(e.BytesReceived) & " of " & BytesToReadableSize(e.TotalBytesToReceive) & " downloaded)"
                    Case "ESN"
                        progress = "Descargando paquete de aplicación principal... (" & BytesToReadableSize(e.BytesReceived) & " de " & BytesToReadableSize(e.TotalBytesToReceive) & " descargados)"
                    Case "FRA"
                        progress = "Téléchargement de l'application principale en cours... (" & BytesToReadableSize(e.BytesReceived, True) & " of " & BytesToReadableSize(e.TotalBytesToReceive, True) & " téléchargés)"
                    Case "PTB", "PTG"
                        progress = "Descarregar o pacote da aplicação principal... (" & BytesToReadableSize(e.BytesReceived) & " de " & BytesToReadableSize(e.TotalBytesToReceive) & " descarregados)"
                    Case "ITA"
                        progress = "Scaricamento del pacchetto dell'applicazione principale... (" & BytesToReadableSize(e.BytesReceived) & " di " & BytesToReadableSize(e.TotalBytesToReceive) & " scaricato)"
                End Select
            Case 1
                progress = "Downloading main application package... (" & BytesToReadableSize(e.BytesReceived) & " of " & BytesToReadableSize(e.TotalBytesToReceive) & " downloaded)"
            Case 2
                progress = "Descargando paquete de aplicación principal... (" & BytesToReadableSize(e.BytesReceived) & " de " & BytesToReadableSize(e.TotalBytesToReceive) & " descargados)"
            Case 3
                progress = "Téléchargement de l'application principale en cours... (" & BytesToReadableSize(e.BytesReceived, True) & " of " & BytesToReadableSize(e.TotalBytesToReceive, True) & " téléchargés)"
            Case 4
                progress = "Descarregar o pacote da aplicação principal... (" & BytesToReadableSize(e.BytesReceived) & " de " & BytesToReadableSize(e.TotalBytesToReceive) & " descarregados)"
            Case 5
                progress = "Scaricamento del pacchetto dell'applicazione principale... (" & BytesToReadableSize(e.BytesReceived) & " di " & BytesToReadableSize(e.TotalBytesToReceive) & " scaricato)"
        End Select
        downSpd = CLng(Math.Round(e.BytesReceived / sw.Elapsed.TotalSeconds, 2))
        If e.TotalBytesToReceive > 0 Then
            time = TimeSpan.FromSeconds((e.TotalBytesToReceive - e.BytesReceived) / CDbl(downSpd))
        End If
    End Sub

    Private Sub WebClient_DownloadFileCompleted(sender As Object, e As AsyncCompletedEventArgs)
        If e.Error IsNot Nothing Then
            Dim msg As String = ""
            Select Case Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            msg = "An error occurred while downloading the file: " & e.Error.Message
                        Case "ESN"
                            msg = "Se produjo un error al descargar el archivo: " & e.Error.Message
                        Case "FRA"
                            msg = "Une erreur s'est produite lors du téléchargement du fichier : " & e.Error.Message
                        Case "PTB", "PTG"
                            msg = "Ocorreu um erro ao baixar o arquivo: " & e.Error.Message
                        Case "ITA"
                            msg = "Si è verificato un errore durante il scaricamento del file: " & e.Error.Message
                    End Select
                Case 1
                    msg = "An error occurred while downloading the file: " & e.Error.Message
                Case 2
                    msg = "Se produjo un error al descargar el archivo: " & e.Error.Message
                Case 3
                    msg = "Une erreur s'est produite lors du téléchargement du fichier : " & e.Error.Message
                Case 4
                    msg = "Ocorreu um erro ao baixar o arquivo: " & e.Error.Message
                Case 5
                    msg = "Si è verificato un errore durante il scaricamento del file: " & e.Error.Message
            End Select
            MsgBox(msg, vbOKOnly + vbCritical, "DISMTools")
            If File.Exists(Path.GetDirectoryName(AppInstallerFile) & "\" & Path.GetFileNameWithoutExtension(AppInstallerFile) & Path.GetExtension(AppInstallerUri)) Then
                File.Delete(Path.GetDirectoryName(AppInstallerFile) & "\" & Path.GetFileNameWithoutExtension(AppInstallerFile) & Path.GetExtension(AppInstallerUri))
            End If
        End If
        Thread.Sleep(500)
        Close()
    End Sub

#End Region

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        StatusLbl.Text = progress
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        downSpdLbl.Text = "Download speed: " & BytesToReadableSize(downSpd) & "/s"
                        downETALbl.Text = "Estimated time remaining: " & time.ToString("m\:ss") & " seconds"
                    Case "ESN"
                        downSpdLbl.Text = "Velocidad de descarga: " & BytesToReadableSize(downSpd) & "/s"
                        downETALbl.Text = "Tiempo restante estimado: " & time.ToString("m\:ss") & " segundos"
                    Case "FRA"
                        downSpdLbl.Text = "Vitesse de téléchargement : " & BytesToReadableSize(downSpd, True) & "/s"
                        downETALbl.Text = "Estimation du temps restant : " & time.ToString("m\:ss") & " secondes"
                    Case "PTB", "PTG"
                        downSpdLbl.Text = "Velocidade de transferência: " & BytesToReadableSize(downSpd) & "/s"
                        downETALbl.Text = "Tempo restante estimado: " & time.ToString("m\:ss") & " segundos"
                    Case "ITA"
                        downSpdLbl.Text = "Velocità di scaricamento: " & BytesToReadableSize(downSpd) & "/s""."
                        downETALbl.Text = "Tempo stimato rimanente: " & time.ToString("m\:ss") & " secondi"
                End Select
            Case 1
                downSpdLbl.Text = "Download speed: " & BytesToReadableSize(downSpd) & "/s"
                downETALbl.Text = "Estimated time remaining: " & time.ToString("m\:ss") & " seconds"
            Case 2
                downSpdLbl.Text = "Velocidad de descarga: " & BytesToReadableSize(downSpd) & "/s"
                downETALbl.Text = "Tiempo restante estimado: " & time.ToString("m\:ss") & " segundos"
            Case 3
                downSpdLbl.Text = "Vitesse de téléchargement : " & BytesToReadableSize(downSpd, True) & "/s"
                downETALbl.Text = "Estimation du temps restant : " & time.ToString("m\:ss") & " secondes"
            Case 4
                downSpdLbl.Text = "Velocidade de transferência: " & BytesToReadableSize(downSpd) & "/s"
                downETALbl.Text = "Tempo restante estimado: " & time.ToString("m\:ss") & " segundos"
            Case 5
                downSpdLbl.Text = "Velocità di scaricamento: " & BytesToReadableSize(downSpd) & "/s""."
                downETALbl.Text = "Tempo stimato rimanente: " & time.ToString("m\:ss") & " secondi"
        End Select
        If ProgressBar1.Value <= ProgressBar1.Maximum Then
            TaskbarHelper.SetIndicatorState(ProgressBar1.Value, Windows.Shell.TaskbarItemProgressState.Normal, MainForm.Handle)
        End If
    End Sub

    Private Sub AppInstallerDownloader_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        TaskbarHelper.SetIndicatorState(100, Windows.Shell.TaskbarItemProgressState.None, MainForm.Handle)
        Timer1.Stop()
    End Sub
End Class
