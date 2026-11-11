Imports System.Windows.Forms
Imports DrvPakCreator.Helpers.FileIO
Imports DrvPakCreator.Classes
Imports DrvPakCreator.Classes.SetupAPI
Imports System.IO

Public Class AddDriverDialog

    Private Enum WizardPage As Integer
        Welcome = 0
        DriverSource = 1
        DriverInfo = 2
        Finish = 3
    End Enum

    Private Const WizardPages As Integer = 4

    Private CurrentWizardPage As WizardPage
    Private VerifyInPages As New List(Of WizardPage)

    ' Wizard Fields
    Public DriverPackMetadata As DriverPack

    Public SourceDriverPath As String
    Public DriverMainFile As String
    Public DriverFileName As String
    Public DriverFileManufacturer As String

    Public DriverPackProjectFilesRoot As String

    Private InfFiles As New List(Of String)
    Private hwEntries As List(Of InfDriverEntry)

    Private Sub ChangePage(ByVal NewPage As WizardPage)
        If NewPage > CurrentWizardPage AndAlso VerifyInPages.Contains(CurrentWizardPage) Then
            If Not VerifyOptionsInPage(CurrentWizardPage) Then Exit Sub
        End If

        pnlWelcome.Visible = NewPage = WizardPage.Welcome
        pnlDriverSource.Visible = NewPage = WizardPage.DriverSource
        pnlDrvInfo.Visible = NewPage = WizardPage.DriverInfo
        pnlFinish.Visible = NewPage = WizardPage.Finish

        WizardStep1.WizardStepIsActive = NewPage = WizardPage.Welcome
        WizardStep2.WizardStepIsActive = NewPage = WizardPage.DriverSource
        WizardStep3.WizardStepIsActive = NewPage = WizardPage.DriverInfo
        WizardStep4.WizardStepIsActive = NewPage = WizardPage.Finish

        Next_Button.Text = IIf(NewPage = WizardPage.Finish, "Finish", "Next")

        Back_Button.Enabled = NewPage > WizardPage.Welcome
        Cancel_Button.Enabled = NewPage < WizardPage.Finish

        If CurrentWizardPage = WizardPage.DriverSource AndAlso NewPage = WizardPage.DriverInfo Then
            ' begin scanning the INF file for HW targets
            GetHardwareTargets(InfFiles(lbDriverFiles.SelectedIndex))
        ElseIf CurrentWizardPage = WizardPage.DriverInfo AndAlso NewPage = WizardPage.Finish Then
            DisplayDriverAdditionOverview()
        End If

        CurrentWizardPage = NewPage

    End Sub

    Private Sub GetHardwareTargets(ByVal InfFile As String)
        Dim className As String = "", _
            classGuid As Guid = Guid.Empty

        MessageBox.Show("The selected INF file will be scanned for hardware targets you may be able to choose from. This process will take some time, depending on the size of the INF file and the speed of your computer.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Cursor = Cursors.WaitCursor

        If Not InfParser.GetInfClassInfo(InfFile, className, classGuid) Then
            MessageBox.Show("This INF file appears to be invalid.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If

        hwEntries = InfParser.ParseInfFile(InfFile)
        If hwEntries.Count = 0 Then
            MessageBox.Show("This INF file appears to be invalid.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If

        Dim hwTargets As New List(Of ListViewItem)

        For Each entry As InfDriverEntry In hwEntries
            Dim position As UInteger = CUInt(hwEntries.IndexOf(entry) + 1)
            Dim DriverEntry As String = ""
            If entry.HardwareIDs.Count > 0 Then
                DriverEntry = String.Format("{0}. {1} - {2}", position, entry.Description, entry.HardwareIDs(0))
            Else
                DriverEntry = String.Format("{0}. {1}", position, entry.Description)
            End If

            hwTargets.Add(New ListViewItem(New String() {DriverEntry}))
        Next

        Cursor = Cursors.Arrow

        lvHwTargets.Items.Clear()
        lvHwTargets.Items.AddRange(hwTargets.ToArray())
    End Sub

    Private Sub DisplayDriverAdditionOverview()
        tbAdditionOverview.Text = String.Format("- Driver Source:{0}" & _
                                                "  - Files will be copied from {1}{2}{1}{0}" & _
                                                "  - Main Driver file: {1}{3}{1}{0}" & _
                                                "- Driver Identification:{0}" & _
                                                "  - Driver Name: {1}{4}{1}{0}" & _
                                                "  - Driver Manufacturer/Vendor: {5}", Environment.NewLine, Quote, SourceDriverPath, DriverMainFile, DriverFileName, DriverFileManufacturer)
    End Sub

    Private Function VerifyOptionsInPage(ByVal Page As WizardPage) As Boolean
        Select Case Page
            Case WizardPage.DriverSource
                If tbDriverSource.Text = "" OrElse Not Directory.Exists(tbDriverSource.Text) Then
                    MessageBox.Show("The driver source must be provided and must exist.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    Return False
                End If
                If lbDriverFiles.SelectedItems.Count = 0 Then
                    MessageBox.Show("You must select the driver file from the list that will act as the main file for this driver.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    Return False
                End If
            Case WizardPage.DriverInfo
                If DriverFileName = "" Or DriverFileManufacturer = "" Then
                    MessageBox.Show("You must provide information that can identify this driver in the Driver Pack. Enter this information manually or use the information provided by the hardware targets of the INF file.", Text, MessageBoxButtons.OK, MessageBoxIcon.Stop)
                    Return False
                End If
        End Select
        Return True
    End Function

    Private Sub AddDriverDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        VerifyInPages.Clear()
        VerifyInPages.AddRange(New WizardPage() {WizardPage.DriverSource, WizardPage.DriverInfo})
        ChangePage(WizardPage.Welcome)
    End Sub

    Private Sub Back_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Back_Button.Click
        ChangePage(CurrentWizardPage - 1)
    End Sub

    Private Sub Next_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Next_Button.Click
        If CurrentWizardPage = WizardPage.Finish Then
            ' Begin adding the driver
            DriverAdditionProgressDialog.DriverSource = SourceDriverPath
            DriverAdditionProgressDialog.DriverMetadata = DriverPackMetadata
            DriverAdditionProgressDialog.DriverPackProjectFilesRoot = DriverPackProjectFilesRoot

            DriverAdditionProgressDialog.DriverMainFile = DriverMainFile
            DriverAdditionProgressDialog.DriverName = DriverFileName
            DriverAdditionProgressDialog.DriverManufacturer = DriverFileManufacturer

            DriverAdditionProgressDialog.ShowDialog(Me)

            DialogResult = Windows.Forms.DialogResult.OK
            Close()
        Else
            ChangePage(CurrentWizardPage + 1)
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Close()
    End Sub

    Private Sub btnBrowseDriverSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDriverSource.Click
        If fbdDriverSource.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            tbDriverSource.Text = fbdDriverSource.SelectedPath
            MessageBox.Show("The directory you provided will be scanned for INF files. This will take a while, depending on how many files there are in the directory. After the scan completes, you can choose the main INF file.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            SourceDriverPath = fbdDriverSource.SelectedPath
            tlpActions.Enabled = False
            WindowHelper.DisableCloseCapability(Handle)
            Cursor = Cursors.WaitCursor
            lblScanStatus.Text = "Scanning..."
            lbDriverFiles.Items.Clear()
            bwDirScanner.RunWorkerAsync()
        End If
    End Sub

    Private Sub bwDirScanner_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDirScanner.DoWork
        InfFiles = PathUtilities.GetRecursiveFileList(SourceDriverPath, "*.inf")
    End Sub

    Private Sub bwDirScanner_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDirScanner.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            ' TODO handle this case
        End If

        lblScanStatus.Text = String.Format("Items Scanned: {0}", InfFiles.Count)
        tlpActions.Enabled = True
        WindowHelper.EnableCloseCapability(Handle)
        Cursor = Cursors.Arrow

        Dim InfFileNames As New List(Of String)
        For Each InfFile As String In InfFiles
            InfFileNames.Add(Path.GetFileName(InfFile))
        Next

        lbDriverFiles.Items.AddRange(InfFileNames.ToArray())
    End Sub

    Private Sub lbDriverFiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbDriverFiles.SelectedIndexChanged
        Try
            If lbDriverFiles.SelectedItems.Count = 1 Then
                Dim drvDirectory As String = Path.GetDirectoryName(InfFiles(lbDriverFiles.SelectedIndex)), _
                    mainFileSubPath As String = ""

                ' The bits that remain in the directory need to be used for the main driver file
                mainFileSubPath = drvDirectory.Replace(SourceDriverPath, "").TrimStart("\").TrimEnd("\")

                DriverMainFile = String.Format("{0}{1}", IIf(mainFileSubPath <> "", String.Format("{0}\", mainFileSubPath), ""), lbDriverFiles.SelectedItem)
                lblScanStatus.Text = String.Format("On folder {0}", Path.GetDirectoryName(InfFiles(lbDriverFiles.SelectedIndex)))
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub rbManualDriverInfo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbManualDriverInfo.CheckedChanged
        tlpManualDriverInfo.Enabled = rbManualDriverInfo.Checked
        pnlHwTargetChooser.Enabled = Not rbManualDriverInfo.Checked

        ' update the final fields
        If rbManualDriverInfo.Checked Then
            DriverFileName = tbManualDriverName.Text
            DriverFileManufacturer = tbManualDriverManufacturer.Text
        Else
            DriverFileName = tbAutoDriverName.Text
            DriverFileManufacturer = tbAutoDriverManufacturer.Text
        End If
    End Sub

    Private Sub lvHwTargets_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvHwTargets.SelectedIndexChanged
        Try
            If lvHwTargets.SelectedItems.Count = 1 Then
                tbAutoDriverName.Text = hwEntries(lvHwTargets.FocusedItem.Index).Description
                tbAutoDriverManufacturer.Text = hwEntries(lvHwTargets.FocusedItem.Index).Manufacturer
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tbDriverName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbManualDriverName.TextChanged, tbAutoDriverName.TextChanged
        If Not TypeOf sender Is TextBox Then Exit Sub

        DriverFileName = CType(sender, TextBox).Text
    End Sub

    Private Sub tbDriverManufacturer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbManualDriverManufacturer.TextChanged, tbAutoDriverManufacturer.TextChanged
        If Not TypeOf sender Is TextBox Then Exit Sub

        DriverFileManufacturer = CType(sender, TextBox).Text
    End Sub
End Class
