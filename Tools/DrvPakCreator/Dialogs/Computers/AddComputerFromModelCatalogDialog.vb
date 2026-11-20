Imports System.Windows.Forms
Imports Microsoft.Win32
Imports DrvPakCreator.Classes
Imports DrvPakCreator.Classes.ModelCatalogs
Imports DrvPakCreator.Classes.ModelCatalogs.HP
Imports DrvPakCreator.Classes.ModelCatalogs.Dell
Imports DrvPakCreator.Classes.ModelCatalogs.Lenovo
Imports DrvPakCreator.Helpers.ModelCatalogs
Imports DrvPakCreator.Helpers.FileIO
Imports DrvPakCreator.Classes.SetupAPI
Imports System.IO

Public Class AddComputerFromModelCatalogDialog

    Private Enum WizardPage As Integer
        Welcome = 0
        ManufacturerSelect = 1
        ModelSelect = 2
        DriverAssociation = 3
        Finish = 4
    End Enum

    Private Enum SourceManufacturer As Integer
        Unknown = -1
        Lenovo = 0
        Dell = 1
        HP = 2
    End Enum

    Private Const WizardPages As Integer = 5

    Private CurrentWizardPage As WizardPage
    Private VerifyInPages As New List(Of WizardPage)

    Public CurrentDriverPack As DriverPack
    Public NewComputer As Computer

    Private SelectedComputerManufacturer As SourceManufacturer = SourceManufacturer.Unknown
    Private LoadedModelCatalog As ModelCatalog

    Private ModelSelected As Boolean

    ' --- HEWLETT-PACKARD (HP) ---
    ' ---- PRIVATE FIELDS FOR HEWLETT-PACKARD ----
    Private HPDestinationSoftpaqExtractedLocation As String
    Private HPSelectedModel As HPProductOSDriverPack
    Private HPFinalExtractedDriverLocation As String
    Private HPDriverFileMappings As New Dictionary(Of String, String())
    Private HPSelectedFiles As New Dictionary(Of String, String())

    ' ---- CONSTANT FIELDS FOR HEWLETT-PACKARD ----
    Private Const HPOS_WIN11_26H2 As Integer = 4347, _
                  HPOS_WIN11_25H2 As Integer = 4333, _
                  HPOS_WIN11_IOTENT_24H2 As Integer = 4324, _
                  HPOS_WIN11_24H2 As Integer = 4323, _
                  HPOS_WIN11_23H2 As Integer = 4317, _
                  HPOS_WIN11_22H2 As Integer = 4306, _
                  HPOS_WIN10_22H2 As Integer = 4308, _
                  HPOS_WIN11_21H2 As Integer = 4298, _
                  HPOS_WIN10_21H2 As Integer = 4297, _
                  HPOS_WIN10_IOTENT_21H2 As Integer = 4305, _
                  HPOS_WIN10_21H1 As Integer = 4295, _
                  HPOS_WIN10_20H2 As Integer = 4288, _
                  HPOS_WIN10_2004 As Integer = 4285, _
                  HPOS_WIN10_1909 As Integer = 4283, _
                  HPOS_WIN10_1903 As Integer = 4279, _
                  HPOS_WIN10_1809 As Integer = 4263, _
                  HPOS_WIN10_IOTENT_1809 As Integer = 4261, _
                  HPOS_WIN10_1803 As Integer = 4254, _
                  HPOS_WIN10_1709 As Integer = 4249, _
                  HPOS_WIN10_1703 As Integer = 4244, _
                  HPOS_WIN10_IOTENT_1607 As Integer = 4260
    Private HPOsVersionMapping As New Dictionary(Of Short, Version)

    ' Wizard Fields

    Public DriverMainFile As String
    Public DriverFileName As String
    Public DriverFileManufacturer As String

    Public DriverPackProjectFilesRoot As String

    Private Sub ChangePage(ByVal NewPage As WizardPage)
        If NewPage > CurrentWizardPage AndAlso VerifyInPages.Contains(CurrentWizardPage) Then
            If Not VerifyOptionsInPage(CurrentWizardPage) Then Exit Sub
        End If

        pnlWelcome.Visible = NewPage = WizardPage.Welcome
        pnlManufacturerSelect.Visible = NewPage = WizardPage.ManufacturerSelect
        pnlModelSelect.Visible = NewPage = WizardPage.ModelSelect
        pnlDrvSelect.Visible = NewPage = WizardPage.DriverAssociation
        pnlFinish.Visible = NewPage = WizardPage.Finish

        WizardStep1.WizardStepIsActive = NewPage = WizardPage.Welcome
        WizardStep2.WizardStepIsActive = NewPage = WizardPage.ManufacturerSelect
        WizardStep3.WizardStepIsActive = NewPage = WizardPage.ModelSelect
        WizardStep4.WizardStepIsActive = NewPage = WizardPage.DriverAssociation
        WizardStep5.WizardStepIsActive = NewPage = WizardPage.Finish

        Next_Button.Text = IIf(NewPage = WizardPage.Finish, "Finish", "Next")

        Back_Button.Enabled = NewPage > WizardPage.Welcome
        Cancel_Button.Enabled = NewPage < WizardPage.Finish

        If CurrentWizardPage = WizardPage.ModelSelect AndAlso NewPage = WizardPage.DriverAssociation Then
            PrepareDriverSelection()
        ElseIf CurrentWizardPage = WizardPage.DriverAssociation AndAlso NewPage = WizardPage.Finish Then
            ShowComputerAdditionOverview()
        End If

        CurrentWizardPage = NewPage

        pnlButtonContainer.Visible = NewPage <> WizardPage.ManufacturerSelect
    End Sub

    Private Function VerifyOptionsInPage(ByVal Page As WizardPage) As Boolean
        Select Case Page
            Case WizardPage.ModelSelect
                If Not ModelSelected Then
                    MessageBox.Show(Me, "Please select a model from the list and try again.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return False
                End If
            Case WizardPage.DriverAssociation
                If lbSelectedDrivers.Items.Count < 1 Then
                    Return False
                End If
        End Select
        Return True
    End Function

    Private Sub PrepareDriverSelection()
        If LoadedModelCatalog Is Nothing Then Exit Sub

        If TypeOf LoadedModelCatalog Is HPDriverPackCatalog Then
            Try
                Directory.Delete(HPDestinationSoftpaqExtractedLocation, True)
            Catch ex As Exception

            End Try

            ' The SoftPAQ needs to be downloaded and extracted
            Dim productAssociatedSoftPAQ As HPSoftPaq = Nothing
            Dim HPCatalog As HPDriverPackCatalog = CType(LoadedModelCatalog, HPDriverPackCatalog)

            Dim obtainedHPProduct As HPProductOSDriverPack = HPCatalog.Products.Products(lvHpProducts.FocusedItem.Index), _
                softpaqId As String = obtainedHPProduct.SoftPaqId
            For Each SoftPaq As HPSoftPaq In CType(LoadedModelCatalog, HPDriverPackCatalog).SoftPaqs.SoftPaqs
                If SoftPaq.Id.Equals(softpaqId, StringComparison.OrdinalIgnoreCase) Then
                    productAssociatedSoftPAQ = SoftPaq
                    Exit For
                End If
            Next

            ' Begin populating the computer
            NewComputer.ComputerInformation.Manufacturer = "HP"
            NewComputer.ComputerInformation.Model = HPSelectedModel.SystemName.Replace("HP ", "")
            If HPOsVersionMapping.ContainsKey(HPSelectedModel.OSId) Then
                NewComputer.ComputerInformation.OperatingSystem.Name = HPSelectedModel.OSName
                NewComputer.ComputerInformation.OperatingSystem.VersionString = HPOsVersionMapping(HPSelectedModel.OSId).ToString()
            End If
            ' Sample data is populated for processor, RAM and storage
            NewComputer.ComputerInformation.Processor = "Generic Processor. Populate this information."
            NewComputer.ComputerInformation.MemoryCapacity = 16384
            NewComputer.ComputerInformation.StorageCapacity = 524288

            If productAssociatedSoftPAQ Is Nothing Then Exit Sub

            HPSoftPaqPreparerDialog.SoftPaqUri = productAssociatedSoftPAQ.Uri
            HPSoftPaqPreparerDialog.SoftPaqSHA256 = productAssociatedSoftPAQ.SHA256

            If HPSoftPaqPreparerDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                ' On the treeview display the directories of the extracted softpaq
                HPDestinationSoftpaqExtractedLocation = HPSoftPaqPreparerDialog.DestinationExtractedSoftPaqDir
                tvSoftPaqSubdirs.Nodes.Clear()
                HPDriverFileMappings.Clear()
                HPSelectedFiles.Clear()

                AdvanceHPExtractedSoftpaqDirectoryStructure(HPDestinationSoftpaqExtractedLocation, HPFinalExtractedDriverLocation)

                Dim SoftpaqTopLevelSubdirs() As String = Directory.GetDirectories(HPFinalExtractedDriverLocation, "*", SearchOption.TopDirectoryOnly)

                For Each SoftpaqTopLevelSubdir As String In SoftpaqTopLevelSubdirs
                    Dim subdirName As String = PathUtilities.GetDirectoryName(SoftpaqTopLevelSubdir)
                    tvSoftPaqSubdirs.Nodes.Add(subdirName, subdirName)
                    HPSelectedFiles.Add(subdirName, New String() {})

                    Dim SubDirInfFiles() As String = Directory.GetFiles(SoftpaqTopLevelSubdir, "*.inf", SearchOption.AllDirectories)

                    If HPDriverFileMappings.ContainsKey(subdirName) Then
                        HPDriverFileMappings(subdirName) = SubDirInfFiles
                    Else
                        HPDriverFileMappings.Add(subdirName, SubDirInfFiles)
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub AdvanceHPExtractedSoftpaqDirectoryStructure(SourceDirectory As String, ByRef FinalDirectory As String)
        Dim SoftpaqTopLevelSubdirs() As String = Directory.GetDirectories(SourceDirectory, "*", SearchOption.TopDirectoryOnly)
        FinalDirectory = SourceDirectory

        ' If we have a CVA file in the extracted dir we have to advance one directory, 
        ' as well as any additional directories until we have a README.
        ' TODO some older softpaqs are laid out differently; the file we have to check is readme.txt
        If Directory.GetFiles(HPDestinationSoftpaqExtractedLocation, "*.cva", SearchOption.TopDirectoryOnly).Length = 1 Then
            FinalDirectory = SoftpaqTopLevelSubdirs(0)

            Do Until File.Exists(Path.Combine(FinalDirectory, "readme.txt"))
                AdvanceHPExtractedSoftpaqDirectoryStructure(FinalDirectory, FinalDirectory)
            Loop
        End If
    End Sub

    Private Sub ShowComputerAdditionOverview()
        Dim SelectedDriverFileString As String = ""

        If TypeOf LoadedModelCatalog Is HPDriverPackCatalog Then
            Dim SelectedDriverFiles As New Dictionary(Of String, String())
            ' filter by actually selected items; then show them in the report
            For Each driverCategory As String In HPSelectedFiles.Keys
                If HPSelectedFiles(driverCategory).Length > 0 Then
                    SelectedDriverFiles.Add(driverCategory, HPSelectedFiles(driverCategory))
                End If
            Next

            For Each SelectedDriverCategory As String In SelectedDriverFiles.Keys
                SelectedDriverFileString &= String.Format("- {1}{0}" & _
                                                          "  - {2}{0}", Environment.NewLine, SelectedDriverCategory, _
                                                          String.Join(String.Format("{0}  - ", Environment.NewLine), SelectedDriverFiles(SelectedDriverCategory)))
            Next
        Else
            Dim SelectedDriverFiles(lbSelectedDrivers.Items.Count - 1) As String
            For i As Integer = 0 To lbSelectedDrivers.Items.Count - 1
                SelectedDriverFiles(i) = lbSelectedDrivers.Items(i)
            Next

            SelectedDriverFileString &= String.Format("- {0}", String.Join(String.Format("{0}  -", Environment.NewLine), SelectedDriverFiles))
        End If


        tbComputerAdditionOverview.Text = String.Format("The following computer will be added to your driver pack:{0}" & _
                                                        "    {1}{0}" & _
                                                        "Generic information will be provided with this new system. Please update it to what the true specifications for the machine are.{0}{0}" & _
                                                        "The following devices will be added to your driver pack and will be associated to this computer:{0}" & _
                                                        "{2}{0}" & _
                                                        "Review this information and click Finish if everything looks right.", Environment.NewLine, _
                                                        String.Format("{0} {1}", NewComputer.ComputerInformation.Manufacturer, NewComputer.ComputerInformation.Model), SelectedDriverFileString)
    End Sub

    Private Sub tvSoftPaqSubdirs_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tvSoftPaqSubdirs.AfterSelect
        If tvSoftPaqSubdirs.SelectedNode IsNot Nothing Then
            lvHPDriverFiles.Items.Clear()
            Dim lvItems As New List(Of ListViewItem)

            If HPDriverFileMappings.ContainsKey(tvSoftPaqSubdirs.SelectedNode.Name) Then
                RemoveHandler lvHPDriverFiles.ItemChecked, AddressOf lvHPDriverFiles_ItemChecked
                For Each DriverFile As String In HPDriverFileMappings(tvSoftPaqSubdirs.SelectedNode.Name)
                    Dim lvi As New ListViewItem(New String() {DriverFile.Replace(Path.Combine(HPFinalExtractedDriverLocation, tvSoftPaqSubdirs.SelectedNode.Name), "").TrimStart("\")})
                    lvi.Checked = Array.IndexOf(HPSelectedFiles(tvSoftPaqSubdirs.SelectedNode.Name), String.Format("{0}\{1}", tvSoftPaqSubdirs.SelectedNode.Name, lvi.Text)) > -1

                    lvItems.Add(lvi)
                Next
                lvHPDriverFiles.Items.AddRange(lvItems.ToArray())
                AddHandler lvHPDriverFiles.ItemChecked, AddressOf lvHPDriverFiles_ItemChecked
            End If
        End If
    End Sub

    Private Sub lvHPDriverFiles_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lvHPDriverFiles.ItemChecked
        If LoadedModelCatalog IsNot Nothing Then
            Dim selectedFiles(lvHPDriverFiles.CheckedItems.Count - 1) As String
            For i As Integer = 0 To lvHPDriverFiles.CheckedItems.Count - 1
                selectedFiles(i) = String.Format("{0}\{1}", tvSoftPaqSubdirs.SelectedNode.Name, lvHPDriverFiles.CheckedItems(i).Text)
            Next

            HPSelectedFiles(tvSoftPaqSubdirs.SelectedNode.Name) = selectedFiles

            lbSelectedDrivers.Items.Clear()
            For Each Node As TreeNode In tvSoftPaqSubdirs.Nodes
                lbSelectedDrivers.Items.AddRange(HPSelectedFiles(Node.Name))
            Next
        End If
    End Sub

    Private Sub AddComputerDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        lvHPDriverFiles.BackColor = BackColor
        lvHPDriverFiles.ForeColor = ForeColor
        lvHpProducts.BackColor = BackColor
        lvHpProducts.ForeColor = ForeColor
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        tvSoftPaqSubdirs.BackColor = BackColor
        tvSoftPaqSubdirs.ForeColor = ForeColor
        tbComputerAdditionOverview.BackColor = BackColor
        tbComputerAdditionOverview.ForeColor = ForeColor
        lbSelectedDrivers.BackColor = BackColor
        lbSelectedDrivers.ForeColor = ForeColor
        WindowHelper.ToggleDarkTitleBar(Handle, CurrentTheme.IsDark)


        NewComputer = New Computer()
        ' Fill in mapping dictionary
        HPOsVersionMapping.Clear()
        HPOsVersionMapping.Add(HPOS_WIN11_26H2, New Version(10, 0, 26300, 9457))
        HPOsVersionMapping.Add(HPOS_WIN11_25H2, New Version(10, 0, 26200, 6584))
        HPOsVersionMapping.Add(HPOS_WIN11_IOTENT_24H2, New Version(10, 0, 26100, 1742))
        HPOsVersionMapping.Add(HPOS_WIN11_24H2, New Version(10, 0, 26100, 1742))
        HPOsVersionMapping.Add(HPOS_WIN11_23H2, New Version(10, 0, 22631, 2428))
        HPOsVersionMapping.Add(HPOS_WIN11_22H2, New Version(10, 0, 22621, 382))
        HPOsVersionMapping.Add(HPOS_WIN11_21H2, New Version(10, 0, 22000, 194))
        HPOsVersionMapping.Add(HPOS_WIN10_22H2, New Version(10, 0, 19045, 2006))
        HPOsVersionMapping.Add(HPOS_WIN10_21H2, New Version(10, 0, 19044, 1288))
        HPOsVersionMapping.Add(HPOS_WIN10_IOTENT_21H2, New Version(10, 0, 19044, 1288))
        HPOsVersionMapping.Add(HPOS_WIN10_21H1, New Version(10, 0, 19043, 928))
        HPOsVersionMapping.Add(HPOS_WIN10_20H2, New Version(10, 0, 19042, 508))
        HPOsVersionMapping.Add(HPOS_WIN10_2004, New Version(10, 0, 19041, 208))
        HPOsVersionMapping.Add(HPOS_WIN10_1909, New Version(10, 0, 18363, 418))
        HPOsVersionMapping.Add(HPOS_WIN10_1903, New Version(10, 0, 18362, 30))
        HPOsVersionMapping.Add(HPOS_WIN10_1809, New Version(10, 0, 17763, 55))
        HPOsVersionMapping.Add(HPOS_WIN10_IOTENT_1809, New Version(10, 0, 17763, 55))
        HPOsVersionMapping.Add(HPOS_WIN10_1803, New Version(10, 0, 17134, 1))
        HPOsVersionMapping.Add(HPOS_WIN10_1709, New Version(10, 0, 16299, 15))
        HPOsVersionMapping.Add(HPOS_WIN10_1703, New Version(10, 0, 15063, 0))
        HPOsVersionMapping.Add(HPOS_WIN10_IOTENT_1607, New Version(10, 0, 14393, 0))
        ChangePage(WizardPage.Welcome)

        lbSelectedDrivers.Items.Clear()
        tvSoftPaqSubdirs.Nodes.Clear()
        lvHPDriverFiles.Items.Clear()
    End Sub

    Private Sub Back_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Back_Button.Click
        ChangePage(CurrentWizardPage - 1)
    End Sub

    Private Sub Next_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Next_Button.Click
        If CurrentWizardPage = WizardPage.Finish Then
            MessageBox.Show(Me, "This operation will take a while. Please wait until we finish. Click OK to begin the addition.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Cursor = Cursors.WaitCursor
            If TypeOf LoadedModelCatalog Is HPDriverPackCatalog Then

                ' For each selected category in a SoftPAQ, the first checked item is the main driver and any
                ' other drivers are considered subdrivers.
                Dim SelectedDriverFiles As New Dictionary(Of String, String())
                ' filter by actually selected items
                For Each driverCategory As String In HPSelectedFiles.Keys
                    If HPSelectedFiles(driverCategory).Length > 0 Then
                        SelectedDriverFiles.Add(driverCategory, HPSelectedFiles(driverCategory))
                    End If
                Next

                DriverAdditionProgressDialog.DriverPackProjectFilesRoot = DriverPackProjectFilesRoot

                For Each SelectedFilesCategory As String In SelectedDriverFiles.Keys
                    Dim SourceDriverPath As String = Path.Combine(HPFinalExtractedDriverLocation, SelectedFilesCategory)

                    DriverAdditionProgressDialog.DriverSource = SourceDriverPath
                    DriverAdditionProgressDialog.DriverMetadata = CurrentDriverPack

                    Dim drvName As String = "", drvManufacturer As String = "", drvMainFile As String = "", drvSubDrivers As New List(Of String)

                    ' To grab the main driver file, we'll go through each driver file and check if we have a set of targets.
                    ' If we don't, the main driver file will become the next one in the iteration.
                    For i As Integer = 0 To SelectedDriverFiles(SelectedFilesCategory).Length - 1
                        Dim className As String = "", _
                            classGuid As Guid = Guid.Empty

                        Dim infFile As String = Path.Combine(HPFinalExtractedDriverLocation, SelectedDriverFiles(SelectedFilesCategory)(i))

                        If Not InfParser.GetInfClassInfo(infFile, className, classGuid) Then Continue For

                        Dim hwEntries As List(Of InfDriverEntry) = InfParser.ParseInfFile(infFile)
                        If hwEntries.Count = 0 Then Continue For

                        drvManufacturer = hwEntries(0).Manufacturer
                        drvName = hwEntries(0).Description
                        drvMainFile = SelectedDriverFiles(SelectedFilesCategory)(i).Replace(SelectedFilesCategory & "\", "")

                        ' Mark everything else as a subdriver. Everything to the left, then everything
                        ' to the right
                        If i > 0 Then
                            For j As Integer = 0 To i - 1
                                drvSubDrivers.Add(SelectedDriverFiles(SelectedFilesCategory)(j).Replace(SelectedFilesCategory & "\", ""))
                            Next
                        End If

                        If i < SelectedDriverFiles(SelectedFilesCategory).Length - 1 Then
                            For j As Integer = i + 1 To SelectedDriverFiles(SelectedFilesCategory).Length - 1
                                drvSubDrivers.Add(SelectedDriverFiles(SelectedFilesCategory)(j).Replace(SelectedFilesCategory & "\", ""))
                            Next
                        End If

                        Exit For
                    Next

                    DriverAdditionProgressDialog.DriverMainFile = drvMainFile
                    DriverAdditionProgressDialog.DriverName = drvName
                    DriverAdditionProgressDialog.DriverManufacturer = drvManufacturer

                    DriverAdditionProgressDialog.ShowDialog(Me)

                    CurrentDriverPack = DriverAdditionProgressDialog.DriverMetadata
                    Dim newDriver As Driver = DriverAdditionProgressDialog.NewDriver
                    newDriver.SubFiles = drvSubDrivers
                    CurrentDriverPack.DriverFiles.Drivers.Add(newDriver)

                    NewComputer.AssociatedDriverIds.Add(newDriver.Id)
                Next

            End If

            Cursor = Cursors.Arrow
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

    Private Sub DisplayHPModels()
        Dim lvItems As New List(Of ListViewItem)

        lvHpProducts.Items.Clear()

        For Each CatalogProduct As HPProductOSDriverPack In CType(LoadedModelCatalog, HPDriverPackCatalog).Products.Products
            lvItems.Add(New ListViewItem(New String() {CatalogProduct.SystemName, CatalogProduct.OSName}))
        Next

        lvHpProducts.Items.AddRange(lvItems.ToArray())
    End Sub

    Private Sub DisplayDellModels()

    End Sub

    Private Sub DisplayLenovoModels()

    End Sub

    Private Sub UpdateModelCatalogViews()
        pnlHPModels.Visible = SelectedComputerManufacturer = SourceManufacturer.HP
        pnlDellModels.Visible = SelectedComputerManufacturer = SourceManufacturer.Dell
        pnlLenovoModels.Visible = SelectedComputerManufacturer = SourceManufacturer.Lenovo

        ' Begin the model load
        Dim modelLoader As ModelCatalogLoader

        Select Case SelectedComputerManufacturer
            Case SourceManufacturer.HP : modelLoader = New HPModelCatalogLoader()
            Case SourceManufacturer.Dell : modelLoader = New DellModelCatalogLoader()
            Case SourceManufacturer.Lenovo : modelLoader = New LenovoModelCatalogLoader()
            Case Else : Exit Sub
        End Select

        Try
            LoadedModelCatalog = modelLoader.LoadManufacturerCatalog()

            If LoadedModelCatalog Is Nothing Then Throw New Exception()

            Select Case SelectedComputerManufacturer
                Case SourceManufacturer.HP : DisplayHPModels()
                Case SourceManufacturer.Dell : DisplayDellModels()
                Case SourceManufacturer.Lenovo : DisplayLenovoModels()
            End Select
        Catch ex As Exception
            MessageBox.Show(Me, "Could not load computer manufacturer models for the selected manufacturer.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSelectHP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectHP.Click
        SelectedComputerManufacturer = SourceManufacturer.HP
        UpdateModelCatalogViews()
        ChangePage(CurrentWizardPage + 1)
    End Sub

    Private Sub lvHpProducts_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvHpProducts.SelectedIndexChanged
        pnlModelDetailContainer.Visible = lvHpProducts.SelectedItems.Count = 1

        If lvHpProducts.SelectedItems.Count = 1 Then
            Try
                If LoadedModelCatalog IsNot Nothing AndAlso TypeOf LoadedModelCatalog Is HPDriverPackCatalog Then
                    ' Driver Packs, in HP land, are SoftPAQs. A computer may have multiple SoftPAQs targeting
                    ' multiple operating system versions. The thing about these SoftPAQs is that they contain
                    ' every driver for that computer. It allows us to scan driver files more easily, but
                    ' incurs a larger download and a longer pack extraction time.

                    Dim HPCatalog As HPDriverPackCatalog = CType(LoadedModelCatalog, HPDriverPackCatalog)

                    Dim obtainedHPProduct As HPProductOSDriverPack = HPCatalog.Products.Products(lvHpProducts.FocusedItem.Index), _
                        softpaqId As String = obtainedHPProduct.SoftPaqId

                    Dim softpaqCount As Integer = 0
                    lblModelName.Text = obtainedHPProduct.SystemName
                    lblSysId.Text = obtainedHPProduct.SystemId

                    HPSelectedModel = obtainedHPProduct

                    For Each SoftPaq As HPSoftPaq In HPCatalog.SoftPaqs.SoftPaqs
                        If SoftPaq.Id.Equals(softpaqId, StringComparison.OrdinalIgnoreCase) Then softpaqCount += 1
                    Next

                    lblDrvPackCount.Text = softpaqCount

                End If
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub AddComputerFromModelCatalogDialog_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Delete temporary files
        Try
            If LoadedModelCatalog IsNot Nothing Then
                If TypeOf LoadedModelCatalog Is HPDriverPackCatalog Then
                    Directory.Delete(HPDestinationSoftpaqExtractedLocation, True)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
