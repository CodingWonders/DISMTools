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
    Dim progress As String

    Dim downSpd As Long

    Private sw As Stopwatch = New Stopwatch()
    Private time As TimeSpan = New TimeSpan()

    Private originalTitle As String

    Private DownloadError As Exception

    Private Sub AppInstallerDownloader_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DownloadError = Nothing
        Timer1.Enabled = True
        downUriLbl.Text = ""
        sw.Reset()
        sw.Start()
        Text = LocalizationService.ForSection("AppInstaller")("DownloadPackage.Button")
        Label1.Text = LocalizationService.ForSection("AppInstaller")("Wait.Message")
        StatusLbl.Text = LocalizationService.ForSection("AppInstaller")("StatusLbl.Label")
        GroupBox1.Text = LocalizationService.ForSection("AppInstaller")("TransferDetails.Group")
        Label2.Text = LocalizationService.ForSection("AppInstaller")("DownloadURL.Label")
        downSpdLbl.Text = LocalizationService.ForSection("AppInstaller")("DownloadSpeed.Label")
        downETALbl.Text = LocalizationService.ForSection("AppInstaller")("EtaUnknown.Label")
        Cancel_Button.Text = LocalizationService.ForSection("AppInstaller")("Cancel.Button")
        Label3.Text = LocalizationService.ForSection("AppInstaller")("Wait.Label")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        Height = WindowHelper.ScaleLogical(320)
        originalTitle = Text
        Visible = True
        DynaLog.LogMessage("App Installer file passed: " & Quote & Path.GetFileName(AppInstallerFile) & Quote)
        If AppInstallerFile IsNot Nothing And File.Exists(AppInstallerFile) Then
            DynaLog.LogMessage("App Installer file exists. Parsing XML information...")
            TaskbarHelper.SetIndicatorState(0, Windows.Shell.TaskbarItemProgressState.Indeterminate, MainForm.Handle)
            ' Create a reader and get the URL information, since .appinstaller files are XML
            Try
                Dim reader As String() = File.ReadAllLines(AppInstallerFile, UTF8)
                Dim readerContents As String = String.Join(CrLf, reader)
                If reader.Any() Then
                    ' Detect if a URL property is present
                    If readerContents.Contains("MainBundle") Then
                        ' Go through each line and find the URL
                        For x = 0 To reader.Count - 1
                            If reader(x).Contains("MainBundle") Then
                                DynaLog.LogMessage("Line " & x + 1 & " contains main package information. Parsing...")
                                Dim serializer As New XmlSerializer(GetType(AppInstallerBundle))
                                Using tReader As TextReader = New StringReader(reader(x))
                                    Dim propertyLine As String = ""
                                    If Not reader(x).EndsWith(" />") Then
                                        DynaLog.LogMessage("Line does not end with XML tag end. Joining line with next lines until tag closure...")
                                        Dim Properties As New List(Of String)
                                        Properties.Add(If(reader(x).EndsWith("MainBundle"), reader(x).Replace(" ", "").Trim(), reader(x)))
                                        Dim nextLineIdx As Integer = 1
                                        Do Until String.Join(" ", Properties).EndsWith(">")
                                            Try
                                                Properties.Add(reader(x + nextLineIdx).Replace(" ", "").Trim())
                                                nextLineIdx += 1
                                            Catch ex As Exception
                                                ' We'll roll with what we have
                                                Exit Do
                                            End Try
                                        Loop
                                        propertyLine = String.Join(" ", Properties)
                                        Dim id = CType(serializer.Deserialize(New StringReader(propertyLine)), AppInstallerBundle)
                                        AppInstallerUri = id.MainBundleUri
                                    Else
                                        DynaLog.LogMessage("Line ends with XML tag end.")
                                        Using ContentReader As XmlReader = XmlReader.Create(tReader)
                                            Dim id = CType(serializer.Deserialize(ContentReader), AppInstallerBundle)
                                            AppInstallerUri = id.MainBundleUri
                                        End Using
                                    End If
                                End Using
                                Exit For
                            End If
                        Next
                    ElseIf readerContents.Contains("MainPackage") Then
                        Dim startingIndex As Integer = reader.ToList().FindIndex(Function(line) line.Contains("MainPackage"))
                        Dim serializer As New XmlSerializer(GetType(AppInstallerStandalone))
                        Using tReader As TextReader = New StringReader(reader(startingIndex))
                            Dim propertyLine As String = ""
                            If Not reader(startingIndex).EndsWith(" />") Then
                                DynaLog.LogMessage("Line does not end with XML tag end. Joining line with next lines until tag closure...")
                                Dim Properties As New List(Of String)
                                Properties.Add(If(reader(startingIndex).EndsWith("MainPackage"), reader(startingIndex).Replace(" ", "").Trim(), reader(startingIndex)))
                                Dim nextLineIdx As Integer = 1
                                Do Until String.Join(" ", Properties).EndsWith(">")
                                    Try
                                        Properties.Add(reader(startingIndex + nextLineIdx).Replace(" ", "").Trim())
                                        nextLineIdx += 1
                                    Catch ex As Exception
                                        ' We'll roll with what we have
                                        Exit Do
                                    End Try
                                Loop
                                propertyLine = String.Join(" ", Properties)
                                Dim id = CType(serializer.Deserialize(New StringReader(propertyLine)), AppInstallerStandalone)
                                AppInstallerUri = id.MainPackageUri
                            Else
                                DynaLog.LogMessage("Line ends with XML tag end.")
                                Using ContentReader As XmlReader = XmlReader.Create(tReader)
                                    Dim id = CType(serializer.Deserialize(ContentReader), AppInstallerStandalone)
                                    AppInstallerUri = id.MainPackageUri
                                End Using
                            End If
                        End Using
                    End If
                End If

                DynaLog.LogMessage("URL of main package: " & Quote & AppInstallerUri & Quote)

                ' Detect if a URL has been detected and download it
                If AppInstallerUri <> "" Then
                    DynaLog.LogMessage("We have a link. Beginning download...")
                    downUriLbl.Text = AppInstallerUri
                    Cancel_Button.Enabled = True
                    Label3.Visible = False
                    BackgroundWorker1.RunWorkerAsync()
                Else
                    DynaLog.LogMessage("We don't have a link. Cancelling...")
                    Throw New Exception()
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
        Dim useFrenchSizeUnits As Boolean = LocalizationService.CurrentCultureCode.StartsWith("fr", StringComparison.OrdinalIgnoreCase)
        progress = LocalizationService.ForSection("AppInstaller.Progress").Format("MainPackage.Label", BytesToReadableSize(e.BytesReceived, useFrenchSizeUnits), BytesToReadableSize(e.TotalBytesToReceive, useFrenchSizeUnits))
        downSpd = CLng(Math.Round(e.BytesReceived / sw.Elapsed.TotalSeconds, 2))
        If e.TotalBytesToReceive > 0 Then
            time = TimeSpan.FromSeconds((e.TotalBytesToReceive - e.BytesReceived) / CDbl(downSpd))
        End If
    End Sub

    Private Sub WebClient_DownloadFileCompleted(sender As Object, e As AsyncCompletedEventArgs)
        If Not e.Cancelled AndAlso e.Error IsNot Nothing Then
            DownloadError = e.Error
            If File.Exists(Path.GetDirectoryName(AppInstallerFile) & "\" & Path.GetFileNameWithoutExtension(AppInstallerFile) & Path.GetExtension(AppInstallerUri)) Then
                DynaLog.LogMessage("Deleting incomplete download...")
                File.Delete(Path.GetDirectoryName(AppInstallerFile) & "\" & Path.GetFileNameWithoutExtension(AppInstallerFile) & Path.GetExtension(AppInstallerUri))
            End If
        ElseIf e.Cancelled Then
            DynaLog.LogMessage("The download has been cancelled.")
            If File.Exists(Path.GetDirectoryName(AppInstallerFile) & "\" & Path.GetFileNameWithoutExtension(AppInstallerFile) & Path.GetExtension(AppInstallerUri)) Then
                DynaLog.LogMessage("Deleting incomplete download...")
                File.Delete(Path.GetDirectoryName(AppInstallerFile) & "\" & Path.GetFileNameWithoutExtension(AppInstallerFile) & Path.GetExtension(AppInstallerUri))
            End If
        End If
        Thread.Sleep(500)
        Close()
    End Sub

#End Region

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        StatusLbl.Text = progress
        Dim useFrenchSizeUnits As Boolean = LocalizationService.CurrentCultureCode.StartsWith("fr", StringComparison.OrdinalIgnoreCase)
        downSpdLbl.Text = LocalizationService.ForSection("AppInstaller.Status").Format("DownloadSpeed.Label", BytesToReadableSize(downSpd, useFrenchSizeUnits))
        downETALbl.Text = LocalizationService.ForSection("AppInstaller.Status").Format("EtaSeconds.Label", time.ToString("m\:ss"))
        If ProgressBar1.Value <= ProgressBar1.Maximum Then
            TaskbarHelper.SetIndicatorState(ProgressBar1.Value, Windows.Shell.TaskbarItemProgressState.Normal, MainForm.Handle)
            Text = String.Format("[{0}%] {1}", Math.Round(ProgressBar1.Value, 0), originalTitle)
        End If
    End Sub

    Private Sub AppInstallerDownloader_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        TaskbarHelper.SetIndicatorState(100, Windows.Shell.TaskbarItemProgressState.None, MainForm.Handle)
        Timer1.Stop()

        If DownloadError IsNot Nothing Then
            DynaLog.LogMessage("An error has occurred and was not caused by user cancellation. Error message: " & DownloadError.Message)
            Dim msg As String = ""
            msg = LocalizationService.ForSection("AppInstaller.Error").Format("DownloadFailed.Message", DownloadError.Message)
            MsgBox(msg, vbOKOnly + vbCritical, "DISMTools")
        End If
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Downloader.CancelAsync()
        Cancel_Button.Enabled = False
        Label3.Visible = True
    End Sub

    Private Sub CopyUri_Button_Click(sender As Object, e As EventArgs) Handles CopyUri_Button.Click
        Dim data As New DataObject()
        data.SetText(downUriLbl.Text)
        Clipboard.SetDataObject(data, True)
    End Sub
End Class
