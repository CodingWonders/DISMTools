﻿Imports System.Windows.Forms
Imports Microsoft.Win32
Imports Microsoft.VisualBasic.ControlChars
Imports System.IO

Public Class ImgCleanup

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        ProgressPanel.CleanupTask = ComboBox1.SelectedIndex
        Select Case ComboBox1.SelectedIndex
            Case 1
                ProgressPanel.CleanupHideSP = CheckBox1.Checked = True
            Case 2
                ProgressPanel.ResetCompBase = CheckBox2.Checked = True
                ProgressPanel.DeferCleanupOps = If(CheckBox2.Checked And CheckBox3.Checked, True, False)
            Case 6
                ProgressPanel.UseCompRepairSource = CheckBox4.Checked = True
                If CheckBox4.Checked And TextBox1.Text = "" Or Not File.Exists(TextBox1.Text) Then
                    MsgBox("No valid source has been provided for component store repair." & CrLf & CrLf & If(TextBox1.Text = "", "Please provide a source and try again.", "Please make sure the specified source exists in the file system and try again."), vbOKOnly + vbCritical, "Image cleanup")
                    Exit Sub
                End If
                ProgressPanel.ComponentRepairSource = TextBox1.Text
                ProgressPanel.LimitWUAccess = CheckBox5.Checked = True
        End Select
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 32
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ImgCleanup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        ComboBox1.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        ' Determine when the last base reset was run
        Using reg As New Process
            reg.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe"
            reg.StartInfo.Arguments = "load HKLM\MountedSoft " & Quote & MainForm.MountDir & "\Windows\system32\config\software" & Quote
            reg.StartInfo.CreateNoWindow = True
            reg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            reg.Start()
            Do Until reg.HasExited
                If reg.HasExited then Exit do
            Loop
            If reg.ExitCode = 0 Then
                Dim regKey As RegistryKey = Registry.LocalMachine.OpenSubKey("MountedSoft\Microsoft\Windows\CurrentVersion\Component Based Servicing", False)
                Dim LastResetBase_UTC As String = regKey.GetValue("LastResetBase_UTC", "Could not get last base reset date. It is possible that no base resets were made").ToString()
                regKey.Close()
                Dim charArray() As Char = LastResetBase_UTC.ToCharArray()
                If LastResetBase_UTC.Contains("/") Then charArray(10) = " "
                LastResetBase_UTC = New String(charArray)
                Label6.Text = LastResetBase_UTC
                reg.StartInfo.Arguments = "unload HKLM\MountedSoft"
                reg.Start()
                Do Until reg.HasExited
                    If reg.HasExited Then Exit Do
                Loop
            Else
                Label6.Text = "Could not get last base reset date"
            End If
        End Using
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "" Then
            Label3.Text = "Choose a task to see its description"
            Panel2.Visible = False
            Panel3.Visible = False
            Panel4.Visible = False
            Panel5.Visible = False
            Panel6.Visible = False
            Panel7.Visible = False
            Panel8.Visible = False
        Else
            Select Case ComboBox1.SelectedIndex
                Case 0
                    Label3.Text = "If you experience a boot failure, this option can try to recover the system by reverting all pending actions from previous servicing operations"
                    Panel2.Visible = True
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 1
                    Label3.Text = "Removes backup files created during the installation of a service pack"
                    Panel2.Visible = False
                    Panel3.Visible = True
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 2
                    Label3.Text = "Cleans up the superseded components and reduces the size of the component store"
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = True
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 3
                    Label3.Text = "Creates a report of the component store, including its size"
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = True
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 4
                    Label3.Text = "Checks whether the image has been flagged as corrupted by a failed process and whether the corruption can be repaired"
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = True
                    Panel7.Visible = False
                    Panel8.Visible = False
                Case 5
                    Label3.Text = "Scans the image for component store corruption, but does not perform repair options automatically"
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = True
                    Panel8.Visible = False
                Case 6
                    Label3.Text = "Scans the image for component store corruption and performs repair operations automatically"
                    Panel2.Visible = False
                    Panel3.Visible = False
                    Panel4.Visible = False
                    Panel5.Visible = False
                    Panel6.Visible = False
                    Panel7.Visible = False
                    Panel8.Visible = True
            End Select
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        Label11.Enabled = CheckBox4.Checked = True
        TextBox1.Enabled = CheckBox4.Checked = True
        Button1.Enabled = CheckBox4.Checked = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        HealthRestoreSourceOFD.ShowDialog()
    End Sub

    Private Sub HealthRestoreSourceOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles HealthRestoreSourceOFD.FileOk
        TextBox1.Text = HealthRestoreSourceOFD.FileName
    End Sub
End Class
