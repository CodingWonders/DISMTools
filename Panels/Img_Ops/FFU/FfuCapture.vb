Imports System.Windows.Forms

Public Class FfuCapture

    Dim CompressionTypeStrings() As String = New String(1) {"No compression will be applied for FFU files. Choose this option if you want to split the resulting file.", "Default compression will be applied for FFU files."}

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()

        DynaLog.LogMessage("Checking fields...")
        If Not WMIDiskHelper.DriveIdExists(TextBox2.Text) Then
            DynaLog.LogMessage("A device with the provided ID does not exist.")
            MsgBox("The specified source drive does not exist.", vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If

        If TextBox1.Text = "" Then
            MsgBox("Please provide a destination path for the FFU file.", vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If

        If TextBox3.Text = "" Then
            MsgBox("Please provide a name for the destination FFU file.", vbOKOnly + vbCritical, Label1.Text)
            Exit Sub
        End If

        ProgressPanel.FFUCaptureSourceDrive = TextBox2.Text
        ProgressPanel.FFUCaptureDestinationFfuImage = TextBox1.Text
        ProgressPanel.FFUCaptureName = TextBox3.Text
        ProgressPanel.FFUCaptureDescription = TextBox4.Text
        ProgressPanel.FFUCaptureCompressType = ComboBox1.SelectedIndex

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 5
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ApplicationDriveSpecifier.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            TextBox2.Text = ApplicationDriveSpecifier.SelectedDriveId
        End If
    End Sub

    Private Sub FfuCapture_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox3.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox4.BackColor = CurrentTheme.SectionBackgroundColor
        RichTextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        RichTextBox1.ForeColor = ForeColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        GroupBox3.ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        RichTextBox1.Clear()
        Try
            Dim SelectedDriveMO As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT Description, Manufacturer, Model, PNPDeviceID, Size, Status, Partitions FROM Win32_DiskDrive WHERE DeviceID LIKE {0}{1}{0}", Quote, TextBox2.Text.Replace("\", "\\")))
            If SelectedDriveMO IsNot Nothing Then
                RichTextBox1.Text = String.Format("  - Model: {1}{0}" &
                                                  "  - Manufacturer: {2}{0}" &
                                                  "  - Description: {3}{0}" &
                                                  "  - Device ID (Plug-and-Play): {4}{0}" &
                                                  "  - Partitions: {5}{0}" &
                                                  "  - Size: {6} bytes (~{7}){0}" &
                                                  "  - Status: {8}",
                                                  Environment.NewLine, WMIHelper.GetObjectValue(SelectedDriveMO(0), "Model"),
                                                                       WMIHelper.GetObjectValue(SelectedDriveMO(0), "Manufacturer"),
                                                                       WMIHelper.GetObjectValue(SelectedDriveMO(0), "Description"),
                                                                       WMIHelper.GetObjectValue(SelectedDriveMO(0), "PNPDeviceId"),
                                                                       WMIHelper.GetObjectValue(SelectedDriveMO(0), "Partitions"),
                                                                       WMIHelper.GetObjectValue(SelectedDriveMO(0), "Size"),
                                                                       Converters.BytesToReadableSize(WMIHelper.GetObjectValue(SelectedDriveMO(0), "Size")),
                                                                       WMIHelper.GetObjectValue(SelectedDriveMO(0), "Status"))
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SaveFileDialog1.ShowDialog(Me)
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        DynaLog.LogMessage("Selected destination image file: " & Quote & SaveFileDialog1.FileName & Quote)
        TextBox1.Text = SaveFileDialog1.FileName
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "none" Then
            Label8.Text = CompressionTypeStrings(0)
        ElseIf ComboBox1.SelectedItem = "default" Then
            Label8.Text = CompressionTypeStrings(1)
        End If
    End Sub
End Class
