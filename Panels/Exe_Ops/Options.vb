Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class Options

    Dim DismVersion As FileVersionInfo
    Dim CanExit As Boolean

    Sub DetermineSettingValidity()
        If TextBox1.Text = "" Then
            CanExit = False
            GiveErrorExplanation(1)
        Else
            If File.Exists(TextBox1.Text) Then
                CanExit = True
            Else
                CanExit = False
                GiveErrorExplanation(2)
            End If
        End If
        If TextBox2.Text = "" Then
            CanExit = False
            GiveErrorExplanation(3)
        Else
            If File.Exists(TextBox2.Text) Then
                CanExit = True
            Else
                Try
                    File.Create(TextBox2.Text)
                    CanExit = True
                Catch ex As Exception
                    CanExit = False
                    GiveErrorExplanation(4)
                End Try
            End If
        End If
        If CheckBox4.Checked Then
            If TextBox3.Text = "" Then
                CanExit = False
                GiveErrorExplanation(5)
            Else
                If Directory.Exists(TextBox3.Text) Then
                    CanExit = True
                Else
                    Try
                        Directory.CreateDirectory(TextBox3.Text)
                        CanExit = True
                    Catch ex As Exception
                        CanExit = False
                        GiveErrorExplanation(5)
                    End Try
                End If
            End If
        End If
        If Not CanExit Then
            Exit Sub
        End If
    End Sub

    Sub ApplyProgSettings()
        DetermineSettingValidity()
        MainForm.DismExe = TextBox1.Text
        Select Case ComboBox1.SelectedIndex
            Case 0
                MainForm.SaveOnSettingsIni = True
            Case 1
                MainForm.SaveOnSettingsIni = False
        End Select
        If CheckBox1.Checked Then
            MainForm.VolatileMode = True
        Else
            MainForm.VolatileMode = False
        End If
        MainForm.ColorMode = ComboBox2.SelectedIndex
        MainForm.Language = 1       ' This version doesn't support languages
        MainForm.LogFont = ComboBox4.Text
        MainForm.LogFile = TextBox2.Text
        MainForm.LogLevel = TrackBar1.Value + 1
        If RadioButton1.Checked Then
            MainForm.ImgOperationMode = 0
        Else
            MainForm.ImgOperationMode = 1
        End If
        If CheckBox2.Checked Then
            MainForm.QuietOperations = True
        Else
            MainForm.QuietOperations = False
        End If
        If CheckBox3.Checked Then
            MainForm.SysNoRestart = True
        Else
            MainForm.SysNoRestart = False
        End If
        If CheckBox4.Checked Then
            MainForm.UseScratch = True
        Else
            MainForm.UseScratch = False
        End If
        MainForm.ScratchDir = TextBox3.Text
        If CheckBox5.Checked Then
            MainForm.EnglishOutput = True
        Else
            MainForm.EnglishOutput = False
        End If
        MainForm.ReportView = ComboBox5.SelectedIndex
        MainForm.ChangePrgColors(MainForm.ColorMode)
        MainForm.ChangeLangs(MainForm.Language)
        If MainForm.VolatileMode Then
            MainForm.SaveDTSettings()
        End If
    End Sub

    Sub GiveErrorExplanation(ErrorCode As Integer)
        Select Case ErrorCode
            Case 1
                MsgBox("The DISM executable path was not specified. Please specify one and try again", MsgBoxStyle.Critical, "DISMTools")
            Case 2
                MsgBox("The DISM executable does not exist in the file system. Please verify the file still exists and try again", MsgBoxStyle.Critical, "DISMTools")
            Case 3
                MsgBox("The log file was not specified. Please specify one and try again", MsgBoxStyle.Critical, "DISMTools")
            Case 4
                MsgBox("The program tried to create the specified log file, but has failed. Please try again", MsgBoxStyle.Critical, "DISMTools")
        End Select
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ApplyProgSettings()
        If CanExit Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Options_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
        GetSystemFonts()
        ' Set default values before loading custom ones
        TextBox1.Text = Path.GetPathRoot(Directory.GetCurrentDirectory()) & "Windows\system32\dism.exe"
        DismVersion = FileVersionInfo.GetVersionInfo(TextBox1.Text)
        Label4.Text = DismVersion.ProductVersion
        TextBox2.Text = Path.GetPathRoot(Directory.GetCurrentDirectory()) & "Windows\Logs\DISM\DISM.log"
        GatherCustomSettings()
    End Sub

    Sub GetSystemFonts()
        ComboBox4.Items.Clear()
        For Each fntFamily As FontFamily In FontFamily.Families
            ComboBox4.Items.Add(fntFamily.Name)
        Next
    End Sub

    Sub GatherCustomSettings()
        TextBox1.Text = MainForm.DismExe
        DismVersion = FileVersionInfo.GetVersionInfo(TextBox1.Text)
        Label4.Text = DismVersion.ProductVersion
        If MainForm.SaveOnSettingsIni Then
            ComboBox1.SelectedIndex = 0
        Else
            ComboBox1.SelectedIndex = 1
        End If
        If MainForm.VolatileMode Then
            CheckBox1.Checked = True
        Else
            CheckBox1.Checked = False
        End If
        Select Case MainForm.ColorMode
            Case 0
                ComboBox2.SelectedIndex = 0
            Case 1
                ComboBox2.SelectedIndex = 1
            Case 2
                ComboBox2.SelectedIndex = 2
        End Select
        ComboBox3.SelectedIndex = 1
        ComboBox4.Text = MainForm.LogFont
        TextBox2.Text = MainForm.LogFile
        TrackBar1.Value = MainForm.LogLevel - 1
        Select Case MainForm.ImgOperationMode
            Case 0
                RadioButton1.Checked = True
                RadioButton2.Checked = False
            Case 1
                RadioButton1.Checked = False
                RadioButton2.Checked = True
        End Select
        If MainForm.QuietOperations Then
            CheckBox2.Checked = True
        Else
            CheckBox2.Checked = False
        End If
        If MainForm.SysNoRestart Then
            CheckBox3.Checked = True
        Else
            CheckBox3.Checked = False
        End If
        If MainForm.UseScratch Then
            CheckBox4.Checked = True
            TextBox3.Text = MainForm.ScratchDir
        Else
            CheckBox4.Checked = False
            TextBox3.Text = ""
        End If
        If MainForm.EnglishOutput Then
            CheckBox5.Checked = True
        Else
            CheckBox5.Checked = False
        End If
        Select Case MainForm.ReportView
            Case 0
                ComboBox5.SelectedIndex = 0
            Case 1
                ComboBox5.SelectedIndex = 1
        End Select
    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox5.SelectedIndexChanged
        Select Case ComboBox5.SelectedIndex
            Case 0
                TextBox4.Text = "Image Version: 10.0.19045.2075" & CrLf & CrLf & _
                    "Features listing for package : Microsoft-Windows-Foundation-Package~31bf3856ad364e35~amd64~~10.0.19041.1" & CrLf & CrLf & _
                    "Feature Name : TFTP" & CrLf & _
                    "State : Disabled" & CrLf & CrLf & _
                    "Feature Name : LegacyComponents" & CrLf & _
                    "State : Enabled" & CrLf & CrLf & _
                    "Feature Name : DirectPlay" & CrLf & _
                    "State : Enabled" & CrLf & CrLf & _
                    "Feature Name : SimpleTCP" & CrLf & _
                    "State : Disabled" & CrLf & CrLf & _
                    "Feature Name : Windows-Identity-Foundation" & CrLf & _
                    "State : Disabled" & CrLf & CrLf & _
                    "Feature Name : NetFx3" & CrLf & _
                    "State : Enabled"
            Case 1
                TextBox4.Text = "Image Version: 10.0.19045.2075" & CrLf & CrLf & _
                    "Features listing for package : Microsoft-Windows-Foundation-Package~31bf3856ad364e35~amd64~~10.0.19041.1" & CrLf & CrLf & CrLf & _
                    "------------------------------------------- | --------" & CrLf & _
                    "Feature Name                                | State" & CrLf & _
                    "------------------------------------------- | --------" & CrLf & _
                    "TFTP                                        | Disabled" & CrLf & _
                    "LegacyComponents                            | Enabled" & CrLf & _
                    "DirectPlay                                  | Enabled" & CrLf & _
                    "SimpleTCP                                   | Disabled" & CrLf & _
                    "Windows-Identity-Foundation                 | Disabled" & CrLf & _
                    "NetFx3                                      | Enabled"
        End Select
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        LogPreview.Font = New Font(ComboBox4.Text, 9.75)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DismComponents.ShowDialog()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DismOFD.ShowDialog()
    End Sub

    Private Sub DismOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles DismOFD.FileOk
        TextBox1.Text = DismOFD.FileName
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        DismVersion = FileVersionInfo.GetVersionInfo(TextBox1.Text)
        Label4.Text = DismVersion.ProductVersion
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        LogSFD.ShowDialog()
    End Sub

    Private Sub LogSFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles LogSFD.FileOk
        TextBox2.Text = LogSFD.FileName
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ScratchFBD.ShowDialog()
        If DialogResult.OK And ScratchFBD.SelectedPath <> "" Then
            TextBox3.Text = ScratchFBD.SelectedPath
        End If
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Select Case TrackBar1.Value
            Case 0
                Label15.Text = "Errors (Log level 1)"
                Label16.Text = "The log file should only display errors after performing an image operation."
            Case 1
                Label15.Text = "Errors and warnings (Log level 2)"
                Label16.Text = "The log file should display errors and warnings after performing an image operation."
            Case 2
                Label15.Text = "Errors, warnings and information messages (Log level 3)"
                Label16.Text = "The log file should display errors, warnings and information messages after performing an image operation."
            Case 3
                Label15.Text = "Errors, warnings, information and debug messages (Log level 4)"
                Label16.Text = "The log file should display errors, warnings, information and debug messages after performing an image operation."
        End Select
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked Then
            Label20.Enabled = True
            Label21.Enabled = True
            TextBox3.Enabled = True
            Button4.Enabled = True
        Else
            Label20.Enabled = False
            Label21.Enabled = False
            TextBox3.Enabled = False
            Button4.Enabled = False
        End If
    End Sub
End Class
