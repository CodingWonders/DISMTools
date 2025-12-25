Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars

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

    Private Sub EventProperties_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        btnPreviousEvent.Enabled = Not (CurrentEventIndex = 0)
        btnNextEvent.Enabled = Not (CurrentEventIndex >= EventCount - 1)
    End Sub

    Sub GetEventInfo()
        Label1.Text = String.Format("Information for event {0} of {1}:", CurrentEventIndex + 1, EventCount)
        txtEventTimestamp.Text = Form1.ListView1.Items(CurrentEventIndex).SubItems(0).Text
        Label6.Text = String.Format("PID {0}", Form1.ListView1.Items(CurrentEventIndex).SubItems(1).Text)
        Dim evtCallerParts As String() = Form1.ListView1.Items(CurrentEventIndex).SubItems(2).Text.Replace(" (", " ").Trim().Split(" ")
        txtEventCaller.Text = evtCallerParts(0)
        If evtCallerParts.Length = 2 Then
            txtEventParentCaller.Text = evtCallerParts(1).TrimEnd(")")
        Else
            txtEventParentCaller.Text = ""
        End If
        txtEventMessage.Text = Form1.ListView1.Items(CurrentEventIndex).SubItems(3).Text
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
        MsgBox("The above field can be empty if the caller does not have a parent, or if the logging system was called by the method in the program with the GetParentCaller parameter set to false." & CrLf & CrLf & _
        "As a developer, you can log events without getting the parent caller like this:" & CrLf & CrLf & _
        "    DynaLog.LogMessage(" & Quote & "Event Message" & Quote & ", False)", vbOKOnly + vbInformation, "Event Parent Caller")
    End Sub
End Class
