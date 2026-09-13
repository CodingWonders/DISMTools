Imports System.Windows.Forms

Public Class DnsZoneChooserDialog

    Public SelectedDnsZone As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If SelectedDnsZone = "" Then
            MessageBox.Show(LocalizationService.ForSection("ActiveDirectory.DnsZone")("SelectZone.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If IsDnsZoneShutdown(SelectedDnsZone) Then
            MessageBox.Show(LocalizationService.ForSection("ActiveDirectory.DnsZone")("Selected.Too.Long.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Function IsDnsZoneShutdown(DnsZoneName As String) As Boolean
        Dim DnsZoneStateMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT Shutdown FROM MicrosoftDNS_Zone WHERE Name = {0}{1}{0}", Quote, WMIHelper.GetEscapedValue(DnsZoneName)), "\\.\root\MicrosoftDNS")

        If DnsZoneName IsNot Nothing Then
            Return WMIHelper.GetObjectValue(DnsZoneStateMOC(0), "Shutdown")
        Else
            Return False
        End If
    End Function

    Private Sub DnsZoneChooserDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor

        ColumnHeader1.Width = WindowHelper.ScaleLogical(168)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(192)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(180)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(192)

        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        GetDnsZones()
    End Sub

    Private Function GetDnsZoneTypeString(ZoneType As Integer) As String
        Select Case ZoneType
            Case 1
                Return "Primary"
            Case 2
                Return "Secondary"
            Case 3
                Return "Stub"
        End Select
        Return ""
    End Function

    Private Sub GetDnsZones()
        ListView1.Items.Clear()
        Dim DnsZoneMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery("SELECT * FROM MicrosoftDNS_Zone", "\\.\root\MicrosoftDNS")

        If DnsZoneMOC IsNot Nothing Then
            For Each DnsZoneMO As ManagementObject In DnsZoneMOC
                Dim DnsZoneProperties As Dictionary(Of String, Object) = WMIHelper.GetObjectValues(DnsZoneMO, "Name", "DnsServerName", "DsIntegrated", "ZoneType", "Reverse")
                ListView1.Items.Add(New ListViewItem(New String() {DnsZoneProperties("Name"),
                                                                   DnsZoneProperties("DnsServerName"),
                                                                   If(DnsZoneProperties("DsIntegrated"), "Yes", "No"),
                                                                   String.Format("{0} ({1} Lookup)", GetDnsZoneTypeString(DnsZoneProperties("ZoneType")), If(DnsZoneProperties("Reverse"), "Reverse", "Forward"))}))
            Next
        Else
            MessageBox.Show(LocalizationService.ForSection("ActiveDirectory.DnsZone")("ZonesLoaded.Message"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            OK_Button.Enabled = ListView1.SelectedItems.Count = 1
            SelectedDnsZone = ListView1.FocusedItem.Text
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Refresh_Button_Click(sender As Object, e As EventArgs) Handles Refresh_Button.Click
        OK_Button.Enabled = False
        SelectedDnsZone = ""
        GetDnsZones()
    End Sub
End Class
