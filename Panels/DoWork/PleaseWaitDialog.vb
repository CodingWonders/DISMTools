Imports System.Windows.Forms
Imports System.IO
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.ControlChars
Imports System.Threading
Imports Microsoft.Dism

Public Class PleaseWaitDialog

    Public Sup_CommandArgs As String

    ' OperationNum: 993
    Public pkgSourceImgStr As String

    Public featOpType As Integer    ' 0: enable; 1: disable

    ' OperationNum: 994
    Public featSourceImg As String

    ' OperationNum: 995
    Public indexesSourceImg As String
    Public imgIndexes As Integer
    Public indexStr(1024) As String

    Private Sub PleaseWaitDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.ProgressPanelStyle
            Case 0
                PictureBox1.Visible = True
                Label1.Visible = True
                Label2.TextAlign = ContentAlignment.TopLeft
                Label2.Location = New Point(52, 29)
                Label2.Size = New Size(295, 15)
                Label2.Font = New Font("Segoe UI", 9)
            Case 1
                PictureBox1.Visible = False
                Label1.Visible = False
                Label2.TextAlign = ContentAlignment.MiddleCenter
                Label2.Location = New Point(8, 8)
                Label2.Size = New Size(343, 43)
                Label2.Font = New Font("Segoe UI", 11.25)
        End Select
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label1.Text = "Please wait..."
                    Case "ESN"
                        Label1.Text = "Espere..."
                    Case "FRA"
                        Label1.Text = "Veuillez patienter..."
                    Case "PTB", "PTG"
                        Label1.Text = "Por favor, aguarde..."
                    Case "ITA"
                        Label1.Text = "Attendere..."
                End Select
            Case 1
                Label1.Text = "Please wait..."
            Case 2
                Label1.Text = "Espere..."
            Case 3
                Label1.Text = "Veuillez patienter..."
            Case 4
                Label1.Text = "Por favor, aguarde..."
            Case 5
                Label1.Text = "Attendere..."
        End Select
        Visible = True
        Panel1.BorderStyle = BorderStyle.None
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Panel1.BackColor = Color.FromArgb(37, 37, 38)
            Panel1.ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Panel1.BackColor = Color.FromArgb(246, 246, 246)
            Panel1.ForeColor = Color.Black
        End If
        Refresh()
        If ProgressPanel.OperationNum = 0 Then
            ProjectValueLoadForm.RichTextBox1.Text = File.ReadAllText(NewProj.TextBox2.Text & "\" & NewProj.TextBox1.Text & "\" & "settings\project.ini", ASCII)
            ProjectValueLoadForm.RichTextBox2.Text = ProjectValueLoadForm.RichTextBox1.Lines(1).ToString().Replace("Name=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox3.Text = ProjectValueLoadForm.RichTextBox1.Lines(2).ToString().Replace("Location=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox4.Text = ProjectValueLoadForm.RichTextBox1.Lines(3).ToString().Replace("EpochCreationTime=", "").Trim()
            ProjectValueLoadForm.RichTextBox5.Text = ProjectValueLoadForm.RichTextBox1.Lines(6).ToString().Replace("ImageFile=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox6.Text = ProjectValueLoadForm.RichTextBox1.Lines(7).ToString().Replace("ImageIndex=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox7.Text = ProjectValueLoadForm.RichTextBox1.Lines(8).ToString().Replace("ImageMountPoint=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox8.Text = ProjectValueLoadForm.RichTextBox1.Lines(9).ToString().Replace("ImageVersion=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox9.Text = ProjectValueLoadForm.RichTextBox1.Lines(10).ToString().Replace("ImageName=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox10.Text = ProjectValueLoadForm.RichTextBox1.Lines(11).ToString().Replace("ImageDescription=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox11.Text = ProjectValueLoadForm.RichTextBox1.Lines(12).ToString().Replace("ImageWIMBoot=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox12.Text = ProjectValueLoadForm.RichTextBox1.Lines(13).ToString().Replace("ImageArch=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox13.Text = ProjectValueLoadForm.RichTextBox1.Lines(14).ToString().Replace("ImageHal=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox14.Text = ProjectValueLoadForm.RichTextBox1.Lines(15).ToString().Replace("ImageSPBuild=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox15.Text = ProjectValueLoadForm.RichTextBox1.Lines(16).ToString().Replace("ImageSPLevel=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox16.Text = ProjectValueLoadForm.RichTextBox1.Lines(17).ToString().Replace("ImageEdition=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox17.Text = ProjectValueLoadForm.RichTextBox1.Lines(18).ToString().Replace("ImagePType=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox18.Text = ProjectValueLoadForm.RichTextBox1.Lines(19).ToString().Replace("ImagePSuite=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox19.Text = ProjectValueLoadForm.RichTextBox1.Lines(20).ToString().Replace("ImageSysRoot=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox20.Text = ProjectValueLoadForm.RichTextBox1.Lines(21).ToString().Replace("ImageDirCount=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox21.Text = ProjectValueLoadForm.RichTextBox1.Lines(22).ToString().Replace("ImageFileCount=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox22.Text = ProjectValueLoadForm.RichTextBox1.Lines(23).ToString().Replace("ImageEpochCreate=", "").Trim()
            ProjectValueLoadForm.RichTextBox23.Text = ProjectValueLoadForm.RichTextBox1.Lines(24).ToString().Replace("ImageEpochModify=", "").Trim()
            ProjectValueLoadForm.RichTextBox24.Text = ProjectValueLoadForm.RichTextBox1.Lines(25).ToString().Replace("ImageLang=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox25.Text = ProjectValueLoadForm.RichTextBox1.Lines(28).ToString().Replace("ImageReadWrite=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.EpochRTB1.Text = DateTimeOffset.FromUnixTimeSeconds(CInt(ProjectValueLoadForm.RichTextBox4.Text)).ToString().Replace(" +00:00", "").Trim()
            Try     ' These are separate, as they aren't set when the project creates
                ProjectValueLoadForm.EpochRTB2.Text = DateTimeOffset.FromUnixTimeSeconds(CInt(ProjectValueLoadForm.RichTextBox22.Text)).ToString().Replace(" +00:00", "").Trim()
                ProjectValueLoadForm.EpochRTB3.Text = DateTimeOffset.FromUnixTimeSeconds(CInt(ProjectValueLoadForm.RichTextBox23.Text)).ToString().Replace(" +00:00", "").Trim()
            Catch ex As Exception
                ProjectValueLoadForm.EpochRTB2.Text = "Not available"
                ProjectValueLoadForm.EpochRTB3.Text = "Not available"
            End Try
            If Debugger.IsAttached Then
                ProjectValueLoadForm.ShowDialog(MainForm)
            End If
            MainForm.ProjectToolStripMenuItem.Visible = True
            Thread.Sleep(250)
            MainForm.Refresh()
            MainForm.CommandsToolStripMenuItem.Visible = True
            Thread.Sleep(250)
            MainForm.Refresh()
            MainForm.HomePanel.Visible = False
            MainForm.PrjPanel.Visible = True
            If ProjectValueLoadForm.RichTextBox5.Text = "N/A" Or ProjectValueLoadForm.RichTextBox6.Text = "N/A" Or ProjectValueLoadForm.RichTextBox7.Text = "N/A" Then
                MainForm.IsImageMounted = False
            End If
        ElseIf ProgressPanel.OperationNum = 990 Then
            ProjectValueLoadForm.RichTextBox1.Text = File.ReadAllText(MainForm.projPath & "\" & "settings\project.ini", ASCII)
            ProjectValueLoadForm.RichTextBox2.Text = ProjectValueLoadForm.RichTextBox1.Lines(1).ToString().Replace("Name=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox3.Text = ProjectValueLoadForm.RichTextBox1.Lines(2).ToString().Replace("Location=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox4.Text = ProjectValueLoadForm.RichTextBox1.Lines(3).ToString().Replace("EpochCreationTime=", "").Trim()
            ProjectValueLoadForm.RichTextBox5.Text = ProjectValueLoadForm.RichTextBox1.Lines(6).ToString().Replace("ImageFile=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox6.Text = ProjectValueLoadForm.RichTextBox1.Lines(7).ToString().Replace("ImageIndex=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox7.Text = ProjectValueLoadForm.RichTextBox1.Lines(8).ToString().Replace("ImageMountPoint=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox8.Text = ProjectValueLoadForm.RichTextBox1.Lines(9).ToString().Replace("ImageVersion=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox9.Text = ProjectValueLoadForm.RichTextBox1.Lines(10).ToString().Replace("ImageName=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox10.Text = ProjectValueLoadForm.RichTextBox1.Lines(11).ToString().Replace("ImageDescription=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox11.Text = ProjectValueLoadForm.RichTextBox1.Lines(12).ToString().Replace("ImageWIMBoot=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox12.Text = ProjectValueLoadForm.RichTextBox1.Lines(13).ToString().Replace("ImageArch=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox13.Text = ProjectValueLoadForm.RichTextBox1.Lines(14).ToString().Replace("ImageHal=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox14.Text = ProjectValueLoadForm.RichTextBox1.Lines(15).ToString().Replace("ImageSPBuild=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox15.Text = ProjectValueLoadForm.RichTextBox1.Lines(16).ToString().Replace("ImageSPLevel=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox16.Text = ProjectValueLoadForm.RichTextBox1.Lines(17).ToString().Replace("ImageEdition=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox17.Text = ProjectValueLoadForm.RichTextBox1.Lines(18).ToString().Replace("ImagePType=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox18.Text = ProjectValueLoadForm.RichTextBox1.Lines(19).ToString().Replace("ImagePSuite=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox19.Text = ProjectValueLoadForm.RichTextBox1.Lines(20).ToString().Replace("ImageSysRoot=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox20.Text = ProjectValueLoadForm.RichTextBox1.Lines(21).ToString().Replace("ImageDirCount=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox21.Text = ProjectValueLoadForm.RichTextBox1.Lines(22).ToString().Replace("ImageFileCount=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.RichTextBox22.Text = ProjectValueLoadForm.RichTextBox1.Lines(23).ToString().Replace("ImageEpochCreate=", "").Trim()
            ProjectValueLoadForm.RichTextBox23.Text = ProjectValueLoadForm.RichTextBox1.Lines(24).ToString().Replace("ImageEpochModify=", "").Trim()
            ProjectValueLoadForm.RichTextBox24.Text = ProjectValueLoadForm.RichTextBox1.Lines(25).ToString().Replace("ImageLang=", "").Trim().Replace(Quote, "").Trim()
            'ProjectValueLoadForm.RichTextBox25.Text = ProjectValueLoadForm.RichTextBox1.Lines(28).ToString().Replace("ImageReadWrite=", "").Trim().Replace(Quote, "").Trim()
            ProjectValueLoadForm.EpochRTB1.Text = DateTimeOffset.FromUnixTimeSeconds(CInt(ProjectValueLoadForm.RichTextBox4.Text)).ToString().Replace(" +00:00", "").Trim()
            Try     ' These are separate, as they aren't set when the project creates
                ProjectValueLoadForm.EpochRTB2.Text = DateTimeOffset.FromUnixTimeSeconds(CInt(ProjectValueLoadForm.RichTextBox22.Text)).ToString().Replace(" +00:00", "").Trim()
                ProjectValueLoadForm.EpochRTB3.Text = DateTimeOffset.FromUnixTimeSeconds(CInt(ProjectValueLoadForm.RichTextBox23.Text)).ToString().Replace(" +00:00", "").Trim()
            Catch ex As Exception
                ProjectValueLoadForm.EpochRTB2.Text = "Not available"
                ProjectValueLoadForm.EpochRTB3.Text = "Not available"
            End Try
            If Debugger.IsAttached Then
                ProjectValueLoadForm.ShowDialog(MainForm)
            End If
            MainForm.ProjectToolStripMenuItem.Visible = True
            Thread.Sleep(250)
            MainForm.Refresh()
            MainForm.CommandsToolStripMenuItem.Visible = True
            Thread.Sleep(250)
            MainForm.Refresh()
            MainForm.HomePanel.Visible = False
            MainForm.PrjPanel.Visible = True
            If ProjectValueLoadForm.RichTextBox5.Text = "N/A" Or ProjectValueLoadForm.RichTextBox6.Text = "N/A" Or ProjectValueLoadForm.RichTextBox7.Text = "N/A" Then
                MainForm.IsImageMounted = False
            Else
                MainForm.IsImageMounted = True
                MainForm.ImgIndex = ProjectValueLoadForm.RichTextBox6.Text
                MainForm.MountDir = ProjectValueLoadForm.RichTextBox7.Text
            End If
        ElseIf ProgressPanel.OperationNum = 993 Then
            Visible = False
            BGProcsBusyDialog.ShowDialog(MainForm)
        ElseIf ProgressPanel.OperationNum = 994 Then
            Visible = False
            BGProcsBusyDialog.ShowDialog(MainForm)
        ElseIf ProgressPanel.OperationNum = 995 Then
            If MainForm.MountedImageDetectorBW.CancellationPending Then
                While MainForm.MountedImageDetectorBW.IsBusy
                    Application.DoEvents()
                    Thread.Sleep(100)
                End While
            End If
            Dim imgInfoCollection As DismImageInfoCollection = Nothing
            Try
                imgInfoCollection = DismApi.GetImageInfo(indexesSourceImg)
            Catch ex As DismNotInitializedException
                DismApi.Initialize(DismLogLevel.LogErrors)
                imgInfoCollection = DismApi.GetImageInfo(indexesSourceImg)
            End Try
            If imgInfoCollection.Count <= 1 Then
                SingleImageIndexError.ShowDialog(MainForm)
                Try
                    DismApi.Shutdown()
                Catch ex As DismOpenSessionsException
                    ' Leave session open
                End Try
            Else
                Dim indexNames As New List(Of String)
                For Each imgInfo As DismImageInfo In imgInfoCollection
                    indexNames.Add(imgInfo.ImageName)
                Next
                ImgIndexSwitch.indexNames = indexNames.ToArray()
                ImgIndexSwitch.TextBox1.Text = indexesSourceImg
                ImgIndexSwitch.NumericUpDown1.Maximum = imgInfoCollection.Count
            End If
            imgIndexes = imgInfoCollection.Count
            Try
                DismApi.Shutdown()
            Catch ex As DismOpenSessionsException
                ' Leave session open
            End Try
        End If
        Close()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        ControlPaint.DrawBorder(e.Graphics, Panel1.ClientRectangle, If(MainForm.ColorSchemes = 0, Color.FromArgb(53, 153, 41), Color.FromArgb(0, 122, 204)), ButtonBorderStyle.Solid)
    End Sub
End Class
