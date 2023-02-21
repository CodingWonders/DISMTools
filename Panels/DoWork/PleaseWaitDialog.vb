Imports System.Windows.Forms
Imports System.IO
Imports System.Text.Encoding
Imports Microsoft.VisualBasic.ControlChars
Imports System.Threading

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
            MainForm.SplitPanels.Visible = True
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
            MainForm.SplitPanels.Visible = True
            If ProjectValueLoadForm.RichTextBox5.Text = "N/A" Or ProjectValueLoadForm.RichTextBox6.Text = "N/A" Or ProjectValueLoadForm.RichTextBox7.Text = "N/A" Then
                MainForm.IsImageMounted = False
            Else
                MainForm.IsImageMounted = True
                MainForm.ImgIndex = ProjectValueLoadForm.RichTextBox6.Text
                MainForm.MountDir = ProjectValueLoadForm.RichTextBox7.Text
            End If
        ElseIf ProgressPanel.OperationNum = 993 Then
            If MainForm.expBackgroundProcesses Then
                Visible = False
                BGProcsBusyDialog.ShowDialog(MainForm)
            Else
                File.WriteAllText(".\temp.bat",
                                  "@echo off" & CrLf &
                                  "dism /English /image=" & pkgSourceImgStr & " /get-packages | findstr /c:" & Quote & "Package Identity : " & Quote & " > .\pkgnames")
                Sup_DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
                Sup_CommandArgs = "/c " & Directory.GetCurrentDirectory() & "\temp.bat"
                Sup_DISMProc.StartInfo.Arguments = Sup_CommandArgs
                Sup_DISMProc.StartInfo.CreateNoWindow = True
                Sup_DISMProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                Sup_DISMProc.Start()
                Do Until Sup_DISMProc.HasExited
                    If Sup_DISMProc.HasExited Then
                        Exit Do
                    End If
                Loop
                If Decimal.ToInt32(Sup_DISMProc.ExitCode) = 0 Then
                    RemPackage.CheckedListBox1.Items.Clear()
                    RemPackage.CheckedListBox2.Items.Clear()
                    If Debugger.IsAttached Then
                        Debug.WriteLine("[INFO] Package names were successfully gathered. The program should return to normal state")
                        Debug.WriteLine("Listing package names:" & CrLf & My.Computer.FileSystem.ReadAllText(".\pkgnames"))
                    End If
                    Dim pkgNames As New RichTextBox
                    pkgNames.Text = My.Computer.FileSystem.ReadAllText(".\pkgnames")
                    For x = 0 To pkgNames.Lines.Count - 1
                        If pkgNames.Lines(x) = "" Then
                            Continue For
                        End If
                        RemPackage.CheckedListBox1.Items.Add(pkgNames.Lines(x).Replace("Package Identity : ", "").Trim())
                    Next
                    File.Delete(".\temp.bat")
                    File.Delete(".\pkgnames")
                Else
                    Debug.WriteLine("[FAIL] Package names were not gathered. Please verify everything's working")
                End If
            End If
        ElseIf ProgressPanel.OperationNum = 994 Then
            If MainForm.expBackgroundProcesses Then
                Visible = False
                BGProcsBusyDialog.ShowDialog(MainForm)
            Else
                File.WriteAllText(".\temp.bat",
                                  "@echo off" & CrLf &
                                  "dism /English /image=" & featSourceImg & " /get-features | findstr /c:" & Quote & "Feature Name : " & Quote & " > .\featnames" & CrLf & _
                                  "dism /English /image=" & featSourceImg & " /get-features | findstr /c:" & Quote & "State : " & Quote & " > .\featstate")
                Sup_DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
                Sup_DISMProc.StartInfo.Arguments = "/c " & Directory.GetCurrentDirectory() & "\temp.bat"
                Sup_DISMProc.StartInfo.CreateNoWindow = True
                Sup_DISMProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                Sup_DISMProc.Start()
                Do Until Sup_DISMProc.HasExited
                    If Sup_DISMProc.HasExited Then
                        Exit Do
                    End If
                Loop
                If Decimal.ToInt32(Sup_DISMProc.ExitCode) = 0 Then
                    EnableFeat.ListView1.Items.Clear()
                    DisableFeat.ListView1.Items.Clear()
                    Debug.WriteLine("[INFO] Feature names and their states were successfully gathered. The program should return to normal state")
                    Debug.WriteLine("Listing feature names and their states:")
                    Dim featNameRTB As New RichTextBox With {
                        .Text = My.Computer.FileSystem.ReadAllText(".\featnames")
                    }
                    Dim featStateRTB As New RichTextBox With {
                        .Text = My.Computer.FileSystem.ReadAllText(".\featstate")
                    }
                    Select Case featOpType
                        Case 0
                            If Debugger.IsAttached Then
                                For x = 0 To featNameRTB.Lines.Count - 1
                                    If featNameRTB.Lines(x) = "" Then
                                        Continue For
                                    End If
                                    Debug.WriteLine("Feature: " & featNameRTB.Lines(x).Replace("Feature Name : ", "").Trim().ToString() & " (" & featStateRTB.Lines(x).Replace("State : ", "").Trim().ToString() & ")")
                                Next
                            End If
                            For x = 0 To featNameRTB.Lines.Count - 1
                                If featNameRTB.Lines(x) = "" Or featStateRTB.Lines(x).Contains("Enable") Then
                                    Continue For
                                End If
                                EnableFeat.ListView1.Items.Add(featNameRTB.Lines(x).Replace("Feature Name : ", "").Trim().ToString()).SubItems.Add(featStateRTB.Lines(x).Replace("State : ", "").Trim().ToString())
                            Next
                            File.Delete(".\temp.bat")
                            File.Delete(".\featnames")
                            File.Delete(".\featstate")
                            EnableFeat.Label2.Text = "This image contains " & featNameRTB.Lines.Count & " features."
                        Case 1
                            If Debugger.IsAttached Then
                                For x = 0 To featNameRTB.Lines.Count - 1
                                    If featNameRTB.Lines(x) = "" Then
                                        Continue For
                                    End If
                                    Debug.WriteLine("Feature: " & featNameRTB.Lines(x).Replace("Feature Name : ", "").Trim().ToString() & " (" & featStateRTB.Lines(x).Replace("State : ", "").Trim().ToString() & ")")
                                Next
                            End If
                            For x = 0 To featNameRTB.Lines.Count - 1
                                If featNameRTB.Lines(x) = "" Or featStateRTB.Lines(x).Contains("Disable") Then
                                    Continue For
                                End If
                                DisableFeat.ListView1.Items.Add(featNameRTB.Lines(x).Replace("Feature Name : ", "").Trim().ToString()).SubItems.Add(featStateRTB.Lines(x).Replace("State : ", "").Trim().ToString())
                            Next
                            File.Delete(".\temp.bat")
                            File.Delete(".\featnames")
                            File.Delete(".\featstate")
                            DisableFeat.Label2.Text = "This image contains " & featNameRTB.Lines.Count & " features."
                    End Select

                Else

                End If
            End If
        ElseIf ProgressPanel.OperationNum = 995 Then
            Dim DismExe As String = MainForm.DismExe
            Dim DismFileVer As FileVersionInfo = FileVersionInfo.GetVersionInfo(DismExe)
            Select Case DismFileVer.ProductMajorPart
                Case 6
                    Select Case DismFileVer.ProductMinorPart
                        Case 1
                            File.WriteAllText(".\temp.bat", _
                                              "@echo off" & CrLf & _
                                              "dism /English /get-wiminfo /wimfile=" & indexesSourceImg & " | find /c " & Quote & "Index : " & Quote & " > .\indexcount" & CrLf & _
                                              "dism /English /get-wiminfo /wimfile=" & indexesSourceImg & " | findstr /c:" & Quote & "Name : " & Quote & " > .\indexnames", _
                                              ASCII)
                        Case Is >= 2
                            File.WriteAllText(".\temp.bat", _
                                              "@echo off" & CrLf & _
                                              "dism /English /get-imageinfo /imagefile=" & indexesSourceImg & " | find /c " & Quote & "Index : " & Quote & " > .\indexcount" & CrLf & _
                                              "dism /English /get-imageinfo /imagefile=" & indexesSourceImg & " | findstr /c:" & Quote & "Name : " & Quote & " > .\indexnames", _
                                              ASCII)
                    End Select
                Case 10
                    File.WriteAllText(".\temp.bat", _
                                      "@echo off" & CrLf & _
                                      "dism /English /get-imageinfo /imagefile=" & indexesSourceImg & " | find /c " & Quote & "Index : " & Quote & " > .\indexcount" & CrLf & _
                                      "dism /English /get-imageinfo /imagefile=" & indexesSourceImg & " | findstr /c:" & Quote & "Name : " & Quote & " > .\indexnames", _
                                      ASCII)

            End Select
            Sup_DISMProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\cmd.exe"
            Sup_DISMProc.StartInfo.Arguments = "/c " & Directory.GetCurrentDirectory() & "\temp.bat"
            Sup_DISMProc.StartInfo.CreateNoWindow = True
            Sup_DISMProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            Sup_DISMProc.Start()
            Do Until Sup_DISMProc.HasExited
                If Sup_DISMProc.HasExited Then
                    Exit Do
                End If
            Loop
            If Decimal.ToInt32(Sup_DISMProc.ExitCode) = 0 Then
                ' Set values
                imgIndexes = CInt(My.Computer.FileSystem.ReadAllText(".\indexcount"))
                If imgIndexes = 1 Then
                    SingleImageIndexError.ShowDialog(MainForm)
                    If Not IsDisposed Then
                        Dispose()
                    End If
                    File.Delete(".\temp.bat")
                    Exit Sub
                End If
                Dim indexNameRTB As New RichTextBox With {
                    .Text = My.Computer.FileSystem.ReadAllText(".\indexnames")
                }
                For x = 0 To indexNameRTB.Lines.Count - 1
                    indexStr(x) = indexNameRTB.Lines(x).Replace("Name : ", "").Trim()
                Next
                For x = 0 To indexStr.Length
                    Try
                        If indexStr(x) = "" Then
                            Continue For
                        Else
                            ImgIndexSwitch.indexNames(x) = indexStr(x)
                        End If
                    Catch ex As Exception
                        Continue For
                    End Try
                Next
                ' Load form
                ImgIndexSwitch.TextBox1.Text = indexesSourceImg
                ImgIndexSwitch.NumericUpDown1.Maximum = imgIndexes
                File.Delete(".\indexcount")
                File.Delete(".\indexnames")
                File.Delete(".\temp.bat")
            End If
        End If
        Close()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        ControlPaint.DrawBorder(e.Graphics, Panel1.ClientRectangle, Color.FromArgb(0, 122, 204), ButtonBorderStyle.Solid)
    End Sub
End Class
