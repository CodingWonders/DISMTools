Imports System.IO

Public Class SplashForm

    Dim BackgroundPicture As Image
    Dim InstallerProcess As New Process()

    Public SetupSuccess As Boolean

    Public TestMode As Boolean = Environment.GetCommandLineArgs().Contains("/test")
    Public TestBCD As Boolean = Environment.GetCommandLineArgs().Contains("/bcdtest")

    Sub ChangeLanguage(LanguageCode As String)
        If Not File.Exists(Path.Combine(Application.StartupPath, "Languages", "lang_" & LanguageCode & ".ini")) Then
            LanguageCode = "en"
        End If
        LoadLanguageFile(Path.Combine(Application.StartupPath, "Languages", "lang_" & LanguageCode & ".ini"))
        Label1.Text = GetValueFromLanguageData("SplashScreen.OSInstTitle")
        Label2.Text = GetValueFromLanguageData("SplashScreen.OSInstStatus_StartingUp")
    End Sub

    Private Sub SplashForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load background image
        If File.Exists(Application.StartupPath & "\Resources\SplashScreen\background.jpg") Then
            BackgroundPicture = Image.FromFile(Application.StartupPath & "\Resources\SplashScreen\background.jpg")
            ResizeImage()
        End If
        ChangeLanguage(My.Computer.Info.InstalledUICulture.TwoLetterISOLanguageName)
        ' Change status font size
        Dim ReferenceSize As Size = New Size(1024, 768)
        If Width <= ReferenceSize.Width AndAlso Height <= ReferenceSize.Height Then
            ' Keep font size as is
        Else
            Label2.Font = New Font("Segoe UI", 24)
        End If
        If File.Exists(Application.StartupPath & "\setup.exe") Then
            VersionLabel.Text = VersionLabel.Text.Replace("<version>", My.Application.Info.Version.ToString()).Trim() & "_" & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\setup.exe").ToString("yyMMdd-HHmm")
        Else
            VersionLabel.Text = VersionLabel.Text.Replace("<version>", My.Application.Info.Version.ToString()).Trim() & "_" & RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe").ToString("yyMMdd-HHmm")
        End If

        VersionLabel.Visible = File.Exists(Path.Combine(Application.StartupPath, "testbuild"))

        Refresh()
        MainForm.ShowDialog(Me)
        Close()
    End Sub

    Private Sub SplashForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If Width > 800 AndAlso Height > 600 Then
            BackgroundImageLayout = ImageLayout.Stretch
        ElseIf Width < 800 AndAlso Height < 600 Then
            BackgroundImageLayout = ImageLayout.Zoom
        Else
            BackgroundImageLayout = ImageLayout.Center
        End If
        ResizeImage()
    End Sub

    Private Sub ResizeImage()
        If BackgroundPicture Is Nothing Then Exit Sub

        Dim formWidth As Integer = ClientSize.Width
        Dim formHeight As Integer = ClientSize.Height

        ' Calculate aspect ratios
        Dim formAspectRatio As Single = formWidth / formHeight
        Dim imageAspectRatio As Single = BackgroundPicture.Width / BackgroundPicture.Height

        Dim croppedImage As Bitmap

        If formAspectRatio > imageAspectRatio Then
            ' Form is wider than the image: crop vertically
            Dim newHeight As Integer = CInt(BackgroundPicture.Width / formAspectRatio)
            Dim cropY As Integer = (BackgroundPicture.Height - newHeight) \ 2
            croppedImage = New Bitmap(BackgroundPicture).Clone(New Rectangle(0, cropY, BackgroundPicture.Width, newHeight), BackgroundPicture.PixelFormat)
        Else
            ' Form is taller than the image: crop horizontally
            Dim newWidth As Integer = CInt(BackgroundPicture.Height * formAspectRatio)
            Dim cropX As Integer = (BackgroundPicture.Width - newWidth) \ 2
            croppedImage = New Bitmap(BackgroundPicture).Clone(New Rectangle(cropX, 0, newWidth, BackgroundPicture.Height), BackgroundPicture.PixelFormat)
        End If

        ' Set the cropped image as the background, resized to the form's dimensions
        BackgroundImage = New Bitmap(croppedImage, New Size(formWidth, formHeight))
        croppedImage.Dispose() ' Dispose of the intermediate cropped image
    End Sub

    Function RetrieveLinkerTimestamp(ByVal filePath As String) As DateTime
        Const PeHeaderOffset As Integer = 60
        Const LinkerTimestampOffset As Integer = 8
        Dim b(2047) As Byte
        Dim s As Stream = Nothing
        Try
            s = New FileStream(filePath, FileMode.Open, FileAccess.Read)
            s.Read(b, 0, 2048)
        Finally
            If Not s Is Nothing Then s.Close()
        End Try
        Dim i As Integer = BitConverter.ToInt32(b, PeHeaderOffset)
        Dim SecondsSince1970 As Integer = BitConverter.ToInt32(b, i + LinkerTimestampOffset)
        Dim dt As New DateTime(1970, 1, 1, 0, 0, 0)
        dt = dt.AddSeconds(SecondsSince1970)
        Dim tz As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")
        dt = TimeZoneInfo.ConvertTimeFromUtc(dt, tz)
        Return dt
    End Function

    Private Sub SplashForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If SetupSuccess Then
            Label2.Text = GetValueFromLanguageData("SplashScreen.OSInstStatus_Restarting")
            Label2.Visible = True
            Refresh()
            If Not TestMode Then
                Dim Shutter As New Process
                Shutter.StartInfo.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "shutdown.exe")
                Shutter.StartInfo.Arguments = "/r /t 0"
                Shutter.StartInfo.CreateNoWindow = True
                Shutter.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                Shutter.Start()
            End If
        End If
    End Sub
End Class
