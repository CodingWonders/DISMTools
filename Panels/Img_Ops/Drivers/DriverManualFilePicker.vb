Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class DriverManualFilePicker

    Public DriverDir As String = ""

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Items checked for addition: " & CheckedListBox1.CheckedItems.Count)
        If CheckedListBox1.CheckedItems.Count <= 0 Then Exit Sub
        DynaLog.LogMessage("Adding selected items...")
        Dim SelectedDrivers As New List(Of String)
        For Each DrvItem As ListViewItem In AddDrivers.ListView1.Items
            SelectedDrivers.Add(DrvItem.SubItems(0).Text)
        Next
        If CheckedListBox1.Items.Count > 0 Then
            For Each Item In CheckedListBox1.CheckedItems
                If SelectedDrivers.Contains(Item) Then Continue For
                AddDrivers.ListView1.Items.Add(New ListViewItem(New String() {Item, LocalizationService.ForSection("DriverFilePicker.Validation")("File.Label")}))
            Next
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub DriverManualFilePicker_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Control.CheckForIllegalCrossThreadCalls = False
        CheckedListBox1.Items.Clear()
        Text = LocalizationService.ForSection("Driver.Manual")("Driver.Files.Choose.Label")
        Label1.Text = LocalizationService.ForSection("Driver.Manual")("RecursiveListing.Message")
        OK_Button.Text = LocalizationService.ForSection("Driver.Manual")("Ok.Button")
        Cancel_Button.Text = LocalizationService.ForSection("Driver.Manual")("Cancel.Button")
        Button1.Text = LocalizationService.ForSection("Driver.Manual")("Refresh.Button")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        CheckedListBox1.BackColor = CurrentTheme.SectionBackgroundColor
        CheckedListBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        If DriverDir <> "" And Directory.Exists(DriverDir) Then ScanBW.RunWorkerAsync()
    End Sub

    Private Sub UpdateScanStatus()
        Label2.Text = LocalizationService.ForSection("Driver.Manual.Scan").Format("Scanning.Driver.Dir.Label", CheckedListBox1.Items.Count)
    End Sub

    Private Sub ScanBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles ScanBW.DoWork
        DynaLog.LogMessage("Scanning directory " & Quote & DriverDir & Quote & "...")
        UpdateScanStatus()
        For Each DrvFile In Directory.GetFiles(DriverDir, "*.inf", SearchOption.AllDirectories)
            CheckedListBox1.Items.Add(DrvFile)
            UpdateScanStatus()
        Next
        DynaLog.LogMessage("Items detected in directory: " & CheckedListBox1.Items.Count)
    End Sub

    Private Sub ScanBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles ScanBW.RunWorkerCompleted
        Label2.Text = LocalizationService.ForSection("Driver.Manual.Scan").Format("Dir.Complete.Driver.Label", CheckedListBox1.Items.Count)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DynaLog.LogMessage("Preparing to refresh results...")
        CheckedListBox1.Items.Clear()
        If DriverDir <> "" And Directory.Exists(DriverDir) Then ScanBW.RunWorkerAsync()
    End Sub
End Class
