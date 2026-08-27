Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Win32
Imports DynaViewer.Classes.ColorUtilities

Public Class EventProperties

    Public CurrentEventIndex As Integer
    Public EventCount As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub SetColorMode(ByVal NewColorMode As ColorThemeMode)
        Select Case NewColorMode
            Case ColorThemeMode.Light
                WindowHelper.ToggleDarkTitleBar(Handle, False)

                BackColor = Color.FromArgb(239, 239, 242)
                ForeColor = Color.Black
            Case ColorThemeMode.Dark
                WindowHelper.ToggleDarkTitleBar(Handle, True)

                BackColor = Color.FromArgb(32, 32, 32)
                ForeColor = Color.White
        End Select

        txtEventTimestamp.BackColor = BackColor
        txtEventTimestamp.ForeColor = ForeColor
        txtEventCaller.BackColor = BackColor
        txtEventCaller.ForeColor = ForeColor
        txtEventParentCaller.BackColor = BackColor
        txtEventParentCaller.ForeColor = ForeColor
        txtEventMessage.BackColor = BackColor
        txtEventMessage.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor

        LinkLabel1.LinkColor = Color.DodgerBlue
    End Sub

    Private Sub EventProperties_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetColorMode(MainForm.CurrentColorMode)
        btnPreviousEvent.Enabled = Not (CurrentEventIndex = 0)
        btnNextEvent.Enabled = Not (CurrentEventIndex >= EventCount - 1)
    End Sub

    Sub GetEventInfo()
        Label1.Text = String.Format("Information for event {0} of {1}:", CurrentEventIndex + 1, EventCount)
        txtEventTimestamp.Text = MainForm.ListView1.Items(CurrentEventIndex).SubItems(0).Text
        Label6.Text = String.Format("PID {0}", MainForm.ListView1.Items(CurrentEventIndex).SubItems(1).Text)
        Dim evtCallerParts As String() = MainForm.ListView1.Items(CurrentEventIndex).SubItems(2).Text.Replace(" (", " ").Trim().Split(" ")
        txtEventCaller.Text = evtCallerParts(0)
        If evtCallerParts.Length = 2 Then
            txtEventParentCaller.Text = evtCallerParts(1).TrimEnd(")")
        Else
            txtEventParentCaller.Text = ""
        End If
        txtEventMessage.Text = MainForm.ListView1.Items(CurrentEventIndex).SubItems(3).Text
    End Sub

    Private Sub btnPreviousEvent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreviousEvent.Click
        CurrentEventIndex -= 1
        GetEventInfo()
        btnPreviousEvent.Enabled = Not (CurrentEventIndex = 0)
        btnNextEvent.Enabled = Not (CurrentEventIndex >= EventCount - 1)
    End Sub

    Private Sub btnNextEvent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNextEvent.Click
        CurrentEventIndex += 1
        GetEventInfo()
        btnPreviousEvent.Enabled = Not (CurrentEventIndex = 0)
        btnNextEvent.Enabled = Not (CurrentEventIndex >= EventCount - 1)
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MsgBox(LocalizationService.ForSection("DynaViewer.EventProps")("Field.Empty.Caller.Message"), vbOKOnly + vbInformation, LocalizationService.ForSection("DynaViewer.EventProps")("Parent.Caller.Title"))
    End Sub
End Class
