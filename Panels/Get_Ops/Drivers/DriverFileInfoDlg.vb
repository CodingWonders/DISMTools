Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.Dism
Imports DISMTools.Utilities
Imports Microsoft.VisualBasic.ControlChars
Imports System.Globalization

Public Class DriverFileInfoDlg

    Dim drvPkg As DismDriverPackage

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Copy_Button_Click(sender As Object, e As EventArgs) Handles Copy_Button.Click
        DynaLog.LogMessage("Copying driver information to clipboard...")
        Dim clipStr As String = "Driver file information of " & GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex) & ":" & CrLf & CrLf & _
                                "Published name: " & drvPkg.PublishedName & CrLf & _
                                "Original file name: " & drvPkg.OriginalFileName & CrLf & _
                                "Is critical to the boot process? " & If(drvPkg.BootCritical, "Yes", "No") & CrLf & _
                                "Is part of the Windows distribution? " & If(drvPkg.InBox, "Yes", "No") & CrLf & _
                                "Version: " & drvPkg.Version.ToString() & CrLf & _
                                "Class name: " & drvPkg.ClassName & CrLf & _
                                "Class description: " & drvPkg.ClassDescription & CrLf & _
                                "Class GUID: " & drvPkg.ClassGuid & CrLf & _
                                "Provider name: " & drvPkg.ProviderName & CrLf & _
                                "Date: " & drvPkg.Date & CrLf & _
                                "Signature status: " & Casters.SignatureStatus(drvPkg.DriverSignature) & CrLf & _
                                "Catalog file: " & drvPkg.CatalogFile
        Dim data As New DataObject()
        data.SetText(clipStr, TextDataFormat.Text)
        Clipboard.SetDataObject(data, True)
        DialogResult = Windows.Forms.DialogResult.None
    End Sub

    Private Sub DriverFileInfoDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("DriverFileInfo")("Driver.File.Label")
        Label1.Text = LocalizationService.ForSection("DriverFileInfo").Format("Driver.File.Label.Label", Path.GetFileName(GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex)))
        ListView1.Columns(0).Text = LocalizationService.ForSection("DriverFileInfo")("Property.Column")
        ListView1.Columns(1).Text = LocalizationService.ForSection("DriverFileInfo")("Value.Column")
        OK_Button.Text = LocalizationService.ForSection("DriverFileInfo")("Ok.Button")
        Copy_Button.Text = LocalizationService.ForSection("DriverFileInfo")("Copy.Button")
        ListView1.Items.Clear()
        drvPkg = Nothing
        Try
            DynaLog.LogMessage("Initializing API...")
            DismApi.Initialize(DismLogLevel.LogErrors)
            DynaLog.LogMessage("Creating session...")
            Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                DynaLog.LogMessage("Driver file to get information about: " & Quote & GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex) & Quote)
                Dim drvInfo As DismDriverCollection = DismApi.GetDriverInfo(imgSession, GetDriverInfo.ListBox1.Items(GetDriverInfo.ListBox1.SelectedIndex), drvPkg)
                If drvPkg IsNot Nothing Then
                    DynaLog.LogMessage("Driver information was obtained successfully. Adding values to list...")

                    Dim CurrentOSCulture As CultureInfo = CultureInfo.CurrentCulture
                    Dim DriverDateString As String = ""
                    If MainForm.HumanizeDates Then
                        DriverDateString = String.Format("{0}, {1}", drvPkg.Date.ToString(CurrentOSCulture.DateTimeFormat.LongDatePattern, CurrentOSCulture), drvPkg.Date.ToString(CurrentOSCulture.DateTimeFormat.LongTimePattern, CurrentOSCulture))
                    Else
                        DriverDateString = drvPkg.Date.ToString("MM/dd/yyyy HH:mm:ss")
                    End If

                    ListView1.Items.Add(New ListViewItem(New String() {LocalizationService.ForSection("DriverFileInfo")("PublishedName.Label"), drvPkg.PublishedName}))
                    ListView1.Items.Add(New ListViewItem(New String() {LocalizationService.ForSection("DriverFileInfo")("Original.File.Name.Label"), drvPkg.OriginalFileName}))
                    ListView1.Items.Add(New ListViewItem(New String() {LocalizationService.ForSection("DriverFileInfo")("Critical.Boot.Process.Label"), If(drvPkg.BootCritical, LocalizationService.ForSection("DriverFileInfo")("Yes.Button"), LocalizationService.ForSection("DriverFileInfo")("No.Button"))}))
                    ListView1.Items.Add(New ListViewItem(New String() {LocalizationService.ForSection("DriverFileInfo")("Part.Windows.Label"), If(drvPkg.InBox, LocalizationService.ForSection("DriverFileInfo")("ListItem.Button"), LocalizationService.ForSection("DriverFileInfo")("No.Button"))}))
                    ListView1.Items.Add(New ListViewItem(New String() {LocalizationService.ForSection("DriverFileInfo")("Version.Label"), drvPkg.Version.ToString()}))
                    ListView1.Items.Add(New ListViewItem(New String() {LocalizationService.ForSection("DriverFileInfo")("ClassName.Label"), drvPkg.ClassName}))
                    ListView1.Items.Add(New ListViewItem(New String() {LocalizationService.ForSection("DriverFileInfo")("ClassDescription.Label"), drvPkg.ClassDescription}))
                    ListView1.Items.Add(New ListViewItem(New String() {LocalizationService.ForSection("DriverFileInfo")("ClassGUID.Label"), drvPkg.ClassGuid}))
                    ListView1.Items.Add(New ListViewItem(New String() {LocalizationService.ForSection("DriverFileInfo")("ProviderName.Label"), drvPkg.ProviderName}))
                    ListView1.Items.Add(New ListViewItem(New String() {LocalizationService.ForSection("DriverFileInfo")("Date.Label"), DriverDateString}))
                    ListView1.Items.Add(New ListViewItem(New String() {LocalizationService.ForSection("DriverFileInfo")("SignatureStatus.Label"), Casters.SignatureStatus(drvPkg.DriverSignature, True)}))
                    ListView1.Items.Add(New ListViewItem(New String() {LocalizationService.ForSection("DriverFileInfo")("CatalogFile.Label"), drvPkg.CatalogFile}))
                End If
            End Using
        Catch ex As Exception
            DynaLog.LogMessage("Could not get information. Error: " & ex.Message)
            MsgBox(ex.Message & String.Format(LocalizationService.ForSection("DriverFileInfo.Messages")("Hresult.Label"), ex.HResult), vbOKOnly + vbCritical, Text)
        Finally
            DynaLog.LogMessage("Shutting down API...")
            Try
                DismApi.Shutdown()
            Catch ex As Exception

            End Try
        End Try
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
    End Sub
End Class
