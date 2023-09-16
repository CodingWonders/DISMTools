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

    Private Sub AppInstallerDownloader_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        Text = "Downloading application package..."
                        Label1.Text = "Please wait while DISMTools downloads the application package to add it to this image. This can take some time, depending on your network connection speed."
                        Label2.Text = "Please wait..."
                    Case "ESN"
                        Text = "Descargando paquete de aplicación..."
                        Label1.Text = "Espere mientras DISMTools descarga el paquete de aplicación para añadirlo a esta imagen. Esto puede llevar algo de tiempo, dependiendo de la velocidad de su conexión de red."
                        Label2.Text = "Espere..."
                End Select
            Case 1
                Text = "Downloading application package..."
                Label1.Text = "Please wait while DISMTools downloads the application package to add it to this image. This can take some time, depending on your network connection speed."
                Label2.Text = "Please wait..."
            Case 2
                Text = "Descargando paquete de aplicación..."
                Label1.Text = "Espere mientras DISMTools descarga el paquete de aplicación para añadirlo a esta imagen. Esto puede llevar algo de tiempo, dependiendo de la velocidad de su conexión de red."
                Label2.Text = "Espere..."
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        Language = MainForm.Language
        Visible = True
        If AppInstallerFile IsNot Nothing And File.Exists(AppInstallerFile) Then
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
                                        Properties.Add(reader.Lines(x).Replace(" ", "").Trim())
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
        Downloader.DownloadFileAsync(New Uri(AppInstallerUri), Path.GetDirectoryName(AppInstallerFile) & "\" & Path.GetFileNameWithoutExtension(AppInstallerFile) & ".appxbundle")
    End Sub

#Region "WebClient event handlers"

    Private Sub WebClient_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs)
        ProgressBar1.Value = e.ProgressPercentage
        Select Case Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENG"
                        progress = "Downloading main application package... (" & BytesToReadableSize(e.BytesReceived) & " of " & BytesToReadableSize(e.TotalBytesToReceive) & ")"
                    Case "ESN"
                        progress = "Descargando paquete de aplicación principal... (" & BytesToReadableSize(e.BytesReceived) & " de " & BytesToReadableSize(e.TotalBytesToReceive) & ")"
                End Select
            Case 1
                progress = "Downloading main application package... (" & BytesToReadableSize(e.BytesReceived) & " of " & BytesToReadableSize(e.TotalBytesToReceive) & ")"
            Case 2
                progress = "Descargando paquete de aplicación principal... (" & BytesToReadableSize(e.BytesReceived) & " de " & BytesToReadableSize(e.TotalBytesToReceive) & ")"
        End Select
    End Sub

    Private Sub WebClient_DownloadFileCompleted(sender As Object, e As AsyncCompletedEventArgs)
        Thread.Sleep(500)
        Close()
    End Sub

#End Region

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label2.Text = progress
    End Sub

    Private Sub AppInstallerDownloader_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Timer1.Stop()
    End Sub
End Class
