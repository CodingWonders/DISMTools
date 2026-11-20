Imports System.IO
Imports DrvPakCreator.Classes
Imports DrvPakCreator.Helpers
Imports DrvPakCreator.Classes.ColorUtilities
Imports Microsoft.Win32

Public Class MainForm

    Private CurrentProject As DriverPackProject, _
            CurrentProjectPath As String

    Public IsProjectLoaded As Boolean

    Public CurrentColorMode As ColorThemeMode

    Private IsModified As Boolean, _
            IsClosing As Boolean

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        OptionsHelper.LoadSettings()
        UseLZMATSMI.Checked = Options.UseLZMA
        OpenInExplorerTSMI.Checked = Options.ShowCreatedDriverPackInExplorer
        ThemeHelper.LoadThemes()
        SetColorMode(Options.ColorMode)
        Visible = True
        MessageBox.Show(Me, "Welcome to Preview 1 of the Driver Pack Creator. Begin using it to create your driver packs. There will be issues and incomplete features, so report your feedback via GitHub Issues." & Environment.NewLine & Environment.NewLine & "Thanks!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub ChangeMenuItemColors(ByVal bgColor As Color, ByVal fgColor As Color, ByVal itemCollection As ToolStripItemCollection)
        For Each tsi As ToolStripItem In itemCollection
            If TypeOf tsi Is ToolStripDropDownItem Then
                Dim item As ToolStripDropDownItem = CType(tsi, ToolStripDropDownItem)
                Try
                    item.DropDown.BackColor = bgColor
                    item.DropDown.ForeColor = fgColor
                    If item.DropDownItems.Count > 0 Then
                        ChangeMenuItemColors(bgColor, fgColor, item.DropDownItems)
                    End If
                Catch ex As Exception
                    Continue For
                End Try
            End If
        Next
    End Sub

    Private Sub ChangeTheme(ByVal NewMode As ColorThemeMode)
        For Each AvailableTheme As Theme In ThemeHelper.GetThemes()
            If NewMode = ColorThemeMode.Light AndAlso Not AvailableTheme.IsDark Then CurrentTheme = AvailableTheme : Exit For
            If NewMode = ColorThemeMode.Dark AndAlso AvailableTheme.IsDark Then CurrentTheme = AvailableTheme : Exit For
        Next

        If CurrentTheme IsNot Nothing Then
            BackColor = CurrentTheme.BackgroundColor
            ForeColor = CurrentTheme.ForegroundColor

            msActions.BackColor = BackColor
            msActions.ForeColor = ForeColor
            TabPage1.BackColor = BackColor
            TabPage1.ForeColor = ForeColor
            TabPage2.BackColor = BackColor
            TabPage2.ForeColor = ForeColor
            pnlHomeScreen.BackColor = BackColor
            pnlHomeScreen.ForeColor = ForeColor
            pnlProjectScreen.BackColor = BackColor
            pnlProjectScreen.ForeColor = ForeColor
            lvRecentProjects.BackColor = BackColor
            lvRecentProjects.ForeColor = ForeColor

            tbProjectName.BackColor = BackColor
            tbProjectName.ForeColor = ForeColor
            tbDriverPackName.BackColor = BackColor
            tbDriverPackName.ForeColor = ForeColor
            tbDriverPackAuthor.BackColor = BackColor
            tbDriverPackAuthor.ForeColor = ForeColor
            tbDriverPackDescription.BackColor = BackColor
            tbDriverPackDescription.ForeColor = ForeColor

            lvDriverFiles.BackColor = BackColor
            lvDriverFiles.ForeColor = ForeColor
            lvComputers.BackColor = BackColor
            lvComputers.ForeColor = ForeColor

            ' Driver Files View (selected)
            tbDriverName.BackColor = BackColor
            tbDriverName.ForeColor = ForeColor
            tbDriverManufacturer.BackColor = BackColor
            tbDriverManufacturer.ForeColor = ForeColor
            tbDriverMainFile.BackColor = BackColor
            tbDriverMainFile.ForeColor = ForeColor
            lbDriverSubdrivers.BackColor = BackColor
            lbDriverSubdrivers.ForeColor = ForeColor

            ' Computer View (selected)
            tbComputerManufacturer.BackColor = BackColor
            tbComputerManufacturer.ForeColor = ForeColor
            tbComputerModel.BackColor = BackColor
            tbComputerModel.ForeColor = ForeColor
            tbComputerUuid.BackColor = BackColor
            tbComputerUuid.ForeColor = ForeColor
            lvAssociatedDriverFiles.BackColor = BackColor
            lvAssociatedDriverFiles.ForeColor = ForeColor



            ToolStrip1.Renderer = ThemeHelper.GetProfessionalRenderer()
            msActions.Renderer = ThemeHelper.GetProfessionalRenderer()

            ChangeMenuItemColors(CurrentTheme.SectionBackgroundColor, CurrentTheme.ForegroundColor, msActions.Items)

            ThemeHelper.CurrentTheme = CurrentTheme

            ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        End If
    End Sub

    Private Sub SetColorMode(ByVal NewColorMode As ColorThemeMode)
        CurrentColorMode = NewColorMode
        Select Case NewColorMode
            Case ColorThemeMode.Light
                ChangeTheme(NewColorMode)
                Exit Sub

                WindowHelper.ToggleDarkTitleBar(Handle, False)

                BackColor = Color.FromArgb(239, 239, 242)
                ForeColor = Color.Black
            Case ColorThemeMode.Dark
                ChangeTheme(NewColorMode)
                Exit Sub

                WindowHelper.ToggleDarkTitleBar(Handle, True)

                BackColor = Color.FromArgb(32, 32, 32)
                ForeColor = Color.White
            Case ColorThemeMode.System
                If Environment.OSVersion.Version.Major < 10 Then SetColorMode(ColorThemeMode.Light)

                Try
                    Dim darkMode As Boolean
                    Dim ColorModeRk As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", False)
                    If ColorModeRk IsNot Nothing Then
                        darkMode = ColorModeRk.GetValue("AppsUseLightTheme", 1) = 0
                        ColorModeRk.Close()

                        If darkMode Then SetColorMode(ColorThemeMode.Dark) Else SetColorMode(ColorThemeMode.Light)

                        WindowHelper.ToggleDarkTitleBar(Handle, darkMode)
                    End If
                Catch ex As Exception
                    SetColorMode(ColorThemeMode.Light)
                End Try

                Exit Sub
        End Select
    End Sub

    Private Sub LoadDriverPackProject(ByVal ProjectPath As String)
        If Not File.Exists(ProjectPath) Then Exit Sub

        Try
            ' we have to check twice because we may want to cancel project load
            If CurrentProject IsNot Nothing Then UnloadDriverPackProject()
            If CurrentProject IsNot Nothing Then Exit Sub
        Catch ex As Exception

        End Try

        Cursor = Cursors.WaitCursor

        CurrentProject = DriverPackProjectHelper.LoadDriverPackProject(ProjectPath)
        If CurrentProject Is Nothing Then
            Cursor = Cursors.Arrow
            Exit Sub
        End If
        CurrentProjectPath = ProjectPath

        Text = String.Format("{0} - Driver Pack Creator", CurrentProject.Name)

        pnlHomeScreen.Visible = False
        tbProjectName.Text = CurrentProject.Name
        tbDriverPackName.Text = CurrentProject.PackMetadata.Name
        tbDriverPackDescription.Text = CurrentProject.PackMetadata.Description
        tbDriverPackAuthor.Text = CurrentProject.PackMetadata.Author

        IsProjectLoaded = True

        SaveProjectToolStripMenuItem.Enabled = IsProjectLoaded
        BuildDriverPackToolStripMenuItem.Enabled = IsProjectLoaded
        ComputersToolStripMenuItem.Visible = IsProjectLoaded
        DriversToolStripMenuItem.Visible = IsProjectLoaded

        Cursor = Cursors.Arrow

        ScanDriverFiles()
        ScanComputers()

        IsModified = False
    End Sub

    Private Sub SaveDriverPackProject()
        If String.IsNullOrEmpty(CurrentProject.Name) Then
            MessageBox.Show(Me, "Please specify a name for your project.", Text, MessageBoxButtons.OK, MessageBoxIcon.Hand)
            Throw New Exception()
        End If
        Cursor = Cursors.WaitCursor
        DriverPackProjectHelper.SaveDriverPackProject(CurrentProjectPath, CurrentProject)
        Cursor = Cursors.Arrow
        IsModified = False
    End Sub

    Private Sub UnloadDriverPackProject()
        If IsModified Then
            Try
                Dim saveResult As DialogResult = MessageBox.Show(Me, "Some changes were made to your project. Would you like to save them?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                Select Case saveResult
                    Case Windows.Forms.DialogResult.Yes : SaveDriverPackProject()
                    Case Windows.Forms.DialogResult.No : IsModified = False
                    Case Windows.Forms.DialogResult.Cancel : Throw New Exception()
                End Select
            Catch ex As Exception
                If IsClosing Then
                    Throw
                Else
                    Exit Sub
                End If
            End Try
        End If


        Text = "Driver Pack Creator"

        pnlHomeScreen.Visible = True
        IsProjectLoaded = False

        SaveProjectToolStripMenuItem.Enabled = IsProjectLoaded
        BuildDriverPackToolStripMenuItem.Enabled = IsProjectLoaded
        ComputersToolStripMenuItem.Visible = IsProjectLoaded
        DriversToolStripMenuItem.Visible = IsProjectLoaded

        CurrentProject = Nothing
        CurrentProjectPath = ""
    End Sub

    Private Sub InvokeProjectCreation(ByVal sender As Object, ByVal e As EventArgs) Handles NewProjectToolStripMenuItem.Click, lnkNewProject.Click
        If NewProject.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            LoadDriverPackProject(NewProject.ProjectPath)
        End If
    End Sub

    Private Sub InvokeProjectLoad(ByVal sender As Object, ByVal e As EventArgs) Handles OpenExistingProjectToolStripMenuItem.Click, lnkOpenProject.Click
        If ofdProject.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            LoadDriverPackProject(ofdProject.FileName)
        End If
    End Sub

    Private Sub InvokeProjectImport(ByVal sender As Object, ByVal e As EventArgs) Handles CreateProjectFromDriverPackToolStripMenuItem.Click, lnkImportProject.Click
        If CreateProjectFromDriverPack.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DriverPackImportProgressDialog.SourceDriverPackLocation = CreateProjectFromDriverPack.PackLocation
            DriverPackImportProgressDialog.DestinationProjectFilesPath = Path.Combine(Path.GetDirectoryName(CreateProjectFromDriverPack.ProjectPath), "ProjectFiles")
            DriverPackImportProgressDialog.ShowDialog(Me)
            LoadDriverPackProject(CreateProjectFromDriverPack.ProjectPath)
        End If
    End Sub

    Private Sub tbSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbSave.Click
        Try
            SaveDriverPackProject()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tbUnload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbUnload.Click
        UnloadDriverPackProject()
    End Sub

    Private Sub AddComputerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddComputerToolStripMenuItem.Click
        InvokeComputerAddition()
    End Sub

    Private Sub AddComputerFromManufacturerModelCatalogsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddComputerFromManufacturerModelCatalogsToolStripMenuItem.Click
        InvokeComputerAdditionFromDeviceModelCatalog()
    End Sub

    Private Sub InvokeDriverAddition()
        AddDriverDialog.DriverPackMetadata = CurrentProject.PackMetadata
        AddDriverDialog.DriverPackProjectFilesRoot = Path.Combine(Path.GetDirectoryName(CurrentProjectPath), "ProjectFiles\DRIVERS")
        If AddDriverDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            If DriverAdditionProgressDialog.NewDriver IsNot Nothing Then
                CurrentProject.PackMetadata.DriverFiles.Drivers.Add(DriverAdditionProgressDialog.NewDriver)
                Try
                    SaveDriverPackProject()
                Catch ex As Exception

                End Try
                ScanDriverFiles()
            End If
        End If
    End Sub

    Private Sub InvokeComputerAddition()
        AddComputerDialog.CurrentDriverPack = CurrentProject.PackMetadata
        If AddComputerDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            CurrentProject.PackMetadata.Computers.Computers.Add(AddComputerDialog.NewComputer)
            SaveDriverPackProject()
            ScanComputers()
        End If
    End Sub

    Private Sub InvokeComputerAdditionFromDeviceModelCatalog()
        AddComputerFromModelCatalogDialog.CurrentDriverPack = CurrentProject.PackMetadata
        AddComputerFromModelCatalogDialog.DriverPackProjectFilesRoot = Path.Combine(Path.GetDirectoryName(CurrentProjectPath), "ProjectFiles\DRIVERS")
        If AddComputerFromModelCatalogDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            CurrentProject.PackMetadata = AddComputerFromModelCatalogDialog.CurrentDriverPack
            CurrentProject.PackMetadata.Computers.Computers.Add(AddComputerFromModelCatalogDialog.NewComputer)
            SaveDriverPackProject()
            ScanDriverFiles()
            ScanComputers()
        End If
    End Sub

    Private Sub InvokeCurrentComputerAddition()
        AddCurrentComputerDialog.CurrentDriverPack = CurrentProject.PackMetadata
        AddCurrentComputerDialog.DriverPackProjectFilesRoot = Path.Combine(Path.GetDirectoryName(CurrentProjectPath), "ProjectFiles\DRIVERS")
        If AddCurrentComputerDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            CurrentProject.PackMetadata = AddCurrentComputerDialog.CurrentDriverPack
            CurrentProject.PackMetadata.Computers.Computers.Add(AddCurrentComputerDialog.NewComputer)
            SaveDriverPackProject()
            ScanDriverFiles()
            ScanComputers()
        End If
    End Sub

    Private Sub ScanDriverFiles()
        Dim lvItems As New List(Of ListViewItem), _
            associatedDriverItems As New List(Of ListViewItem)
        pnlNoSelectedDriver.Visible = True

        lvDriverFiles.Items.Clear()
        lvAssociatedDriverFiles.Items.Clear()

        For Each DriverFile As Driver In CurrentProject.PackMetadata.DriverFiles.Drivers
            lvItems.Add(New ListViewItem(New String() {Path.GetFileName(DriverFile.MainFile), DriverFile.Name}))
            associatedDriverItems.Add(New ListViewItem(New String() {DriverFile.Name, Path.GetFileName(DriverFile.MainFile)}))
        Next

        lvDriverFiles.Items.AddRange(lvItems.ToArray())
        lvAssociatedDriverFiles.Items.AddRange(associatedDriverItems.ToArray())

        If lvComputers.SelectedItems.Count = 1 Then
            For Each SelectedDriverId As Integer In CurrentProject.PackMetadata.Computers.Computers(lvComputers.FocusedItem.Index).AssociatedDriverIds
                If SelectedDriverId - 1 >= lvAssociatedDriverFiles.Items.Count Then Continue For
                lvAssociatedDriverFiles.Items(SelectedDriverId - 1).Checked = True
            Next
        End If
    End Sub

    Private Sub ScanComputers()
        Dim lvItems As New List(Of ListViewItem)
        pnlNoSelectedComputer.Visible = True

        lvComputers.Items.Clear()

        For Each DriverPackComputer As Computer In CurrentProject.PackMetadata.Computers.Computers
            lvItems.Add(New ListViewItem(New String() {String.Format("{0} {1}", DriverPackComputer.ComputerInformation.Manufacturer, DriverPackComputer.ComputerInformation.Model)}))
        Next

        lvComputers.Items.AddRange(lvItems.ToArray())
    End Sub

    Private Sub ShowDriverInformation(ByVal DriverIndex As UInteger)
        If DriverIndex >= CurrentProject.PackMetadata.DriverFiles.Drivers.Count Then Exit Sub

        Dim obtainedDriver As Driver = CurrentProject.PackMetadata.DriverFiles.Drivers(DriverIndex)

        ' begin showing info
        tbDriverName.Text = obtainedDriver.Name
        tbDriverManufacturer.Text = obtainedDriver.Manufacturer
        tbDriverMainFile.Text = obtainedDriver.MainFile
        lbDriverSubdrivers.Items.Clear()
        lbDriverSubdrivers.Items.AddRange(obtainedDriver.SubFiles.ToArray())
    End Sub

    Private Sub ShowComputerInformation(ByVal ComputerIndex As UInteger)
        If ComputerIndex >= CurrentProject.PackMetadata.Computers.Computers.Count Then Exit Sub

        Dim obtainedComputer As Computer = CurrentProject.PackMetadata.Computers.Computers(ComputerIndex)

        ' begin showing info
        tbComputerManufacturer.Text = obtainedComputer.ComputerInformation.Manufacturer
        tbComputerModel.Text = obtainedComputer.ComputerInformation.Model
        tbComputerUuid.Text = obtainedComputer.ComputerInformation.Uuid.ToString()

        ' we have to uncheck all associated driver files before we check them all
        For Each lvi As ListViewItem In lvAssociatedDriverFiles.Items
            lvi.Checked = False
        Next

        For Each SelectedDriverId As Integer In obtainedComputer.AssociatedDriverIds
            If SelectedDriverId - 1 >= lvAssociatedDriverFiles.Items.Count Then Continue For

            lvAssociatedDriverFiles.Items(SelectedDriverId - 1).Checked = True
        Next
    End Sub

    Private Sub AddDriverToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddDriverToolStripMenuItem.Click
        InvokeDriverAddition()
    End Sub

    Private Sub lnkAddDrvFiles_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkAddDrvFiles.LinkClicked
        InvokeDriverAddition()
    End Sub

    Private Sub lnkAddComputer_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkAddComputer.LinkClicked
        InvokeComputerAddition()
    End Sub

    Private Sub tbProjectName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbProjectName.TextChanged
        CurrentProject.Name = tbProjectName.Text
        Text = String.Format("{0} - Driver Pack Creator", tbProjectName.Text)
        IsModified = True
    End Sub

    Private Sub tbDriverPackDescription_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbDriverPackDescription.TextChanged
        CurrentProject.PackMetadata.Description = tbDriverPackDescription.Text
        IsModified = True
    End Sub

    Private Sub tbDriverPackAuthor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbDriverPackAuthor.TextChanged
        CurrentProject.PackMetadata.Author = tbDriverPackAuthor.Text
        IsModified = True
    End Sub

    Private Sub tbDriverPackName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbDriverPackName.TextChanged
        CurrentProject.PackMetadata.Name = tbDriverPackName.Text
        IsModified = True
    End Sub

    Private Sub lvDriverFiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvDriverFiles.SelectedIndexChanged
        pnlNoSelectedDriver.Visible = lvDriverFiles.SelectedItems.Count = 0
        Try
            If lvDriverFiles.SelectedItems.Count = 1 Then
                ShowDriverInformation(lvDriverFiles.FocusedItem.Index)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub lvComputers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvComputers.SelectedIndexChanged
        pnlNoSelectedComputer.Visible = lvComputers.SelectedItems.Count = 0
        Try
            If lvComputers.SelectedItems.Count = 1 Then
                ShowComputerInformation(lvComputers.FocusedItem.Index)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnRemoveDriver_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveDriver.Click
        Try
            If lvDriverFiles.SelectedItems.Count = 1 Then
                If MessageBox.Show(Me, "Are you sure that you want to remove this driver from the Driver Pack?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If

                Dim needsReindexing As Boolean = lvDriverFiles.FocusedItem.Index < CurrentProject.PackMetadata.DriverFiles.Drivers.Count - 1, _
                    selectedDrvId As Integer = CurrentProject.PackMetadata.DriverFiles.Drivers(lvDriverFiles.FocusedItem.Index).Id - 1, _
                    driverFilesRoot As String = Path.Combine(Path.GetDirectoryName(CurrentProjectPath), "ProjectFiles\DRIVERS"), _
                    driverRootPath As String = String.Format("DRV_{0}", (selectedDrvId + 1).ToString().PadLeft(10, "0"c)), _
                    driverRootFullPath As String = Path.Combine(driverFilesRoot, driverRootPath)

                Dim ComputerReferenceIndexes As New List(Of Integer), _
                    ComputerReferenceNames As New List(Of String)

                ' some computers may reference this driver file; get those
                For Each DriverPackComputer As Computer In CurrentProject.PackMetadata.Computers.Computers
                    If DriverPackComputer.AssociatedDriverIds.Contains(selectedDrvId + 1) Then
                        ComputerReferenceIndexes.Add(CurrentProject.PackMetadata.Computers.Computers.IndexOf(DriverPackComputer))
                        ComputerReferenceNames.Add(String.Format("{0} {1}", DriverPackComputer.ComputerInformation.Manufacturer, DriverPackComputer.ComputerInformation.Model))
                    End If
                Next

                If ComputerReferenceIndexes.Count > 0 Then
                    DriverRemovalReferencesComputersDialog.ReferenceComputerNames = ComputerReferenceNames
                    If DriverRemovalReferencesComputersDialog.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then Exit Sub

                    Cursor = Cursors.AppStarting
                    For Each ComputerReferenceIndex As Integer In ComputerReferenceIndexes
                        CurrentProject.PackMetadata.Computers.Computers(ComputerReferenceIndex).AssociatedDriverIds.Remove(selectedDrvId + 1)
                        For i As Integer = 0 To CurrentProject.PackMetadata.Computers.Computers(ComputerReferenceIndex).AssociatedDriverIds.Count - 1
                            If CurrentProject.PackMetadata.Computers.Computers(ComputerReferenceIndex).AssociatedDriverIds(i) > selectedDrvId + 1 Then
                                CurrentProject.PackMetadata.Computers.Computers(ComputerReferenceIndex).AssociatedDriverIds(i) -= 1
                            End If
                        Next
                    Next
                End If

                Cursor = Cursors.WaitCursor

                CurrentProject.PackMetadata.DriverFiles.Drivers.RemoveAt(lvDriverFiles.FocusedItem.Index)
                If Directory.Exists(driverRootFullPath) Then Directory.Delete(driverRootFullPath, True)

                ' let's reindex if we need to
                If needsReindexing Then
                    DriverReindexProgressDialog.StartingDriverId = selectedDrvId
                    DriverReindexProgressDialog.DriverFilesRoot = driverFilesRoot
                    DriverReindexProgressDialog.SourceDriverMetadata = CurrentProject.PackMetadata.DriverFiles.Drivers

                    DriverReindexProgressDialog.ShowDialog(Me)

                    CurrentProject.PackMetadata.DriverFiles.Drivers = DriverReindexProgressDialog.UpdatedDriverMetadata
                End If

                Try
                    SaveDriverPackProject()
                Catch ex As Exception

                End Try
                ScanDriverFiles()

                Cursor = Cursors.Arrow
            Else

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tbBuild_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbBuild.Click
        If IsModified Then
            If MessageBox.Show(Me, "Some changes were made to your project. Building the driver pack requires that you save these changes. Would you like to save them?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                SaveDriverPackProject()
            Else
                Exit Sub
            End If
        End If
        If sfdDrvPackBuild.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DriverPackBuildProgressDialog.SourceDriverFilesFolder = Path.Combine(Path.GetDirectoryName(CurrentProjectPath), "ProjectFiles")
            DriverPackBuildProgressDialog.DestinationPackFile = sfdDrvPackBuild.FileName
            If DriverPackBuildProgressDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                ExplorerHelper.SelectFileInExplorer(sfdDrvPackBuild.FileName)
            End If
        End If
    End Sub

    Private Sub btnSaveDriver_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveDriver.Click
        Try
            If lvDriverFiles.SelectedItems.Count = 1 Then
                IsModified = True

                ' Save driver info
                CurrentProject.PackMetadata.DriverFiles.Drivers(lvDriverFiles.FocusedItem.Index).Name = tbDriverName.Text
                CurrentProject.PackMetadata.DriverFiles.Drivers(lvDriverFiles.FocusedItem.Index).Manufacturer = tbDriverManufacturer.Text
                CurrentProject.PackMetadata.DriverFiles.Drivers(lvDriverFiles.FocusedItem.Index).MainFile = tbDriverMainFile.Text
                Dim subdrivers As New List(Of String)
                For Each SubDriver As String In lbDriverSubdrivers.Items
                    subdrivers.Add(SubDriver)
                Next
                CurrentProject.PackMetadata.DriverFiles.Drivers(lvDriverFiles.FocusedItem.Index).SubFiles = subdrivers

                ScanDriverFiles()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnChangeMainDriverFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeMainDriverFile.Click
        Try
            If lvDriverFiles.SelectedItems.Count = 1 Then
                Dim driverFilesRoot As String = Path.Combine(Path.GetDirectoryName(CurrentProjectPath), String.Format("ProjectFiles\DRIVERS\DRV_{0}", CurrentProject.PackMetadata.DriverFiles.Drivers(lvDriverFiles.FocusedItem.Index).Id.ToString().PadLeft(10, "0"c)))
                ChangeMainDriverDialog.DriverRootDirectory = driverFilesRoot
                If ChangeMainDriverDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    tbDriverMainFile.Text = ChangeMainDriverDialog.NewDriverMainFile
                    If ChangeMainDriverDialog.NewDriverName <> "" Then tbDriverName.Text = ChangeMainDriverDialog.NewDriverName
                    If ChangeMainDriverDialog.NewDriverManufacturer <> "" Then tbDriverManufacturer.Text = ChangeMainDriverDialog.NewDriverManufacturer
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnManageSubdrivers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnManageSubdrivers.Click
        Try
            If lvDriverFiles.SelectedItems.Count = 1 Then
                Dim driverFilesRoot As String = Path.Combine(Path.GetDirectoryName(CurrentProjectPath), String.Format("ProjectFiles\DRIVERS\DRV_{0}", CurrentProject.PackMetadata.DriverFiles.Drivers(lvDriverFiles.FocusedItem.Index).Id.ToString().PadLeft(10, "0"c)))
                ManageDeviceSubdrivers.DriverRootDirectory = driverFilesRoot
                ManageDeviceSubdrivers.DriverMainFile = CurrentProject.PackMetadata.DriverFiles.Drivers(lvDriverFiles.FocusedItem.Index).MainFile
                ManageDeviceSubdrivers.SelectedSubdrivers = CurrentProject.PackMetadata.DriverFiles.Drivers(lvDriverFiles.FocusedItem.Index).SubFiles
                If ManageDeviceSubdrivers.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    CurrentProject.PackMetadata.DriverFiles.Drivers(lvDriverFiles.FocusedItem.Index).SubFiles = ManageDeviceSubdrivers.SelectedSubdrivers

                    ' repopulate list
                    lbDriverSubdrivers.Items.Clear()
                    lbDriverSubdrivers.Items.AddRange(CurrentProject.PackMetadata.DriverFiles.Drivers(lvDriverFiles.FocusedItem.Index).SubFiles.ToArray())
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub InvokeComputerInfoChange(Optional ByVal TabIdx As Integer = 0)
        If lvComputers.SelectedItems.Count = 0 Then Exit Sub
        Try
            ChangeComputerInfoDialog.ComputerInformation = CurrentProject.PackMetadata.Computers.Computers(lvComputers.FocusedItem.Index).ComputerInformation
            ChangeComputerInfoDialog.tcCompInfo.SelectedIndex = TabIdx
            If ChangeComputerInfoDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                CurrentProject.PackMetadata.Computers.Computers(lvComputers.FocusedItem.Index).ComputerInformation = ChangeComputerInfoDialog.ComputerInformation
                MessageBox.Show(Me, "Changes made to this computer will be saved.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                btnSaveComputer.PerformClick()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnChangeComputerHardware_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeComputerHardware.Click
        InvokeComputerInfoChange(0)
    End Sub

    Private Sub btnChangeComputerSoftware_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeComputerSoftware.Click
        InvokeComputerInfoChange(1)
    End Sub

    Private Sub btnRemoveComputer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveComputer.Click
        Try
            If lvComputers.SelectedItems.Count = 1 Then
                Dim needsReindexing As Boolean = lvComputers.FocusedItem.Index < CurrentProject.PackMetadata.Computers.Computers.Count - 1, _
                    selectedDrvId As Integer = CurrentProject.PackMetadata.Computers.Computers(lvComputers.FocusedItem.Index).Id - 1

                CurrentProject.PackMetadata.Computers.Computers.RemoveAt(lvComputers.FocusedItem.Index)

                ' let's reindex if we need to
                If needsReindexing Then
                    For i As Integer = selectedDrvId To CurrentProject.PackMetadata.Computers.Computers.Count - 1
                        CurrentProject.PackMetadata.Computers.Computers(i).Id = i + 1
                    Next
                End If

                Try
                    SaveDriverPackProject()
                Catch ex As Exception

                End Try
                ScanComputers()
            Else

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnSaveComputer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveComputer.Click
        Try
            If lvComputers.SelectedItems.Count = 1 Then
                IsModified = True

                ' Save computer info
                Dim selectedIds As New List(Of Integer)
                For Each CheckedDriver As ListViewItem In lvAssociatedDriverFiles.CheckedItems
                    selectedIds.Add(CheckedDriver.Index + 1)
                Next
                CurrentProject.PackMetadata.Computers.Computers(lvComputers.FocusedItem.Index).AssociatedDriverIds = selectedIds
                CurrentProject.PackMetadata.Computers.Computers(lvComputers.FocusedItem.Index).ComputerInformation.Manufacturer = tbComputerManufacturer.Text
                CurrentProject.PackMetadata.Computers.Computers(lvComputers.FocusedItem.Index).ComputerInformation.Model = tbComputerModel.Text
                Try
                    CurrentProject.PackMetadata.Computers.Computers(lvComputers.FocusedItem.Index).ComputerInformation.Uuid = New Guid(tbComputerUuid.Text)
                Catch ex As Exception
                    CurrentProject.PackMetadata.Computers.Computers(lvComputers.FocusedItem.Index).ComputerInformation.Uuid = Guid.Empty
                End Try

                ScanComputers()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            IsClosing = True
            UnloadDriverPackProject()
        Catch ex As Exception
            e.Cancel = True
            Exit Sub
        End Try
        OptionsHelper.SaveSettings()
    End Sub

    Private Sub TESTMODELCATALOGSToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TESTMODELCATALOGSToolStripMenuItem.Click
        ModelCatalogViewerForm.Show()
    End Sub

    Private Sub AddCurrentComputerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddCurrentComputerToolStripMenuItem.Click
        'Dim DriverPaths As List(Of String) = SystemDriverHelper.EnumerateInstalledThirdPartyDrivers()
        'For Each DriverPath As String In DriverPaths
        '    MessageBox.Show(Me, DriverPath, Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        'Next
        InvokeCurrentComputerAddition()
    End Sub

    Private Sub LightToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LightToolStripMenuItem.Click
        Options.ColorMode = ColorThemeMode.Light
        ChangeTheme(ColorThemeMode.Light)
    End Sub

    Private Sub DarkToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DarkToolStripMenuItem.Click
        Options.ColorMode = ColorThemeMode.Dark
        ChangeTheme(ColorThemeMode.Dark)
    End Sub

    Private Sub SystemToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SystemToolStripMenuItem.Click
        Options.ColorMode = ColorThemeMode.System
        ChangeTheme(ColorThemeMode.System)
    End Sub

    Private Sub UseLZMATSMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UseLZMATSMI.Click
        If UseLZMATSMI.Checked Then
            MessageBox.Show(Me, "LZMA functions are experimental and may not offer the best compression ratios when building Driver Packs. This will be improved in the future.", "LZMA support", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        Options.UseLZMA = UseLZMATSMI.Checked
    End Sub

    Private Const SongCode As String = "Nightvision"

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        MessageBox.Show(Me, String.Format("DISMTools {1}Discovery{1} Driver Pack Creator, version {2} ({3}){0}{0}{4}", Environment.NewLine, Quote, My.Application.Info.Version.ToString(), SongCode, My.Application.Info.Copyright), "About the Driver Pack Creator", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class