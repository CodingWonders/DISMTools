<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.msActions = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenExistingProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.SaveProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.BuildDriverPackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CreateProjectFromDriverPackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DriversToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AddDriverToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AddDriversInBulkToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator
        Me.ReuseDriverFromDriverPackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ComputersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AddComputerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AddComputerFromManufacturerModelCatalogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AddCurrentComputerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator
        Me.ReuseComputerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FromKnownComputersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FromAnotherDriverPackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.ManageKnownComputersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator
        Me.TESTMODELCATALOGSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlHomeScreen = New System.Windows.Forms.Panel
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.lnkImportProject = New System.Windows.Forms.LinkLabel
        Me.lnkOpenProject = New System.Windows.Forms.LinkLabel
        Me.lnkNewProject = New System.Windows.Forms.LinkLabel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.lvRecentProjects = New System.Windows.Forms.ListView
        Me.chProjectName = New System.Windows.Forms.ColumnHeader
        Me.Label3 = New System.Windows.Forms.Label
        Me.pnlProjectScreen = New System.Windows.Forms.Panel
        Me.tcProjectItems = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.scDrivers = New System.Windows.Forms.SplitContainer
        Me.lvDriverFiles = New System.Windows.Forms.ListView
        Me.chMainDriverFile = New System.Windows.Forms.ColumnHeader
        Me.chDriverName = New System.Windows.Forms.ColumnHeader
        Me.pnlDriverDetailContainer = New System.Windows.Forms.Panel
        Me.pnlNoSelectedDriver = New System.Windows.Forms.Panel
        Me.lnkAddDrvFiles = New System.Windows.Forms.LinkLabel
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.pnlSelectedDriver = New System.Windows.Forms.Panel
        Me.btnManageSubdrivers = New System.Windows.Forms.Button
        Me.lbDriverSubdrivers = New System.Windows.Forms.ListBox
        Me.btnChangeMainDriverFile = New System.Windows.Forms.Button
        Me.tbDriverMainFile = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.tbDriverManufacturer = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.tbDriverName = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.pnlDriverActions = New System.Windows.Forms.Panel
        Me.btnSaveDriver = New System.Windows.Forms.Button
        Me.btnRemoveDriver = New System.Windows.Forms.Button
        Me.Label16 = New System.Windows.Forms.Label
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.scComputers = New System.Windows.Forms.SplitContainer
        Me.lvComputers = New System.Windows.Forms.ListView
        Me.chComputer = New System.Windows.Forms.ColumnHeader
        Me.pnlComputerDetailContainer = New System.Windows.Forms.Panel
        Me.pnlNoSelectedComputer = New System.Windows.Forms.Panel
        Me.lnkAddComputer = New System.Windows.Forms.LinkLabel
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.pnlSelectedComputer = New System.Windows.Forms.Panel
        Me.btnChangeComputerSoftware = New System.Windows.Forms.Button
        Me.btnChangeComputerHardware = New System.Windows.Forms.Button
        Me.lvAssociatedDriverFiles = New System.Windows.Forms.ListView
        Me.chDriverFriendlyName = New System.Windows.Forms.ColumnHeader
        Me.chDriverMainFile = New System.Windows.Forms.ColumnHeader
        Me.tbComputerUuid = New System.Windows.Forms.TextBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label26 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.tbComputerModel = New System.Windows.Forms.TextBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.tbComputerManufacturer = New System.Windows.Forms.TextBox
        Me.Label24 = New System.Windows.Forms.Label
        Me.pnlComputerActions = New System.Windows.Forms.Panel
        Me.btnSaveComputer = New System.Windows.Forms.Button
        Me.btnRemoveComputer = New System.Windows.Forms.Button
        Me.Label25 = New System.Windows.Forms.Label
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.Label6 = New System.Windows.Forms.Label
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.Panel7 = New System.Windows.Forms.Panel
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.tbProjectName = New System.Windows.Forms.TextBox
        Me.pnlDriverPackDetails = New System.Windows.Forms.Panel
        Me.tbDriverPackAuthor = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.tbDriverPackDescription = New System.Windows.Forms.TextBox
        Me.tbDriverPackName = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tbSave = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.tbUnload = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.tbBuild = New System.Windows.Forms.ToolStripButton
        Me.ofdProject = New System.Windows.Forms.OpenFileDialog
        Me.sfdDrvPackBuild = New System.Windows.Forms.SaveFileDialog
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ColorModeTSMI = New System.Windows.Forms.ToolStripMenuItem
        Me.UseLZMATSMI = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenInExplorerTSMI = New System.Windows.Forms.ToolStripMenuItem
        Me.LightToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DarkToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SystemToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpTopicsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.msActions.SuspendLayout()
        Me.pnlHomeScreen.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.pnlProjectScreen.SuspendLayout()
        Me.tcProjectItems.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.scDrivers.Panel1.SuspendLayout()
        Me.scDrivers.Panel2.SuspendLayout()
        Me.scDrivers.SuspendLayout()
        Me.pnlDriverDetailContainer.SuspendLayout()
        Me.pnlNoSelectedDriver.SuspendLayout()
        Me.pnlSelectedDriver.SuspendLayout()
        Me.pnlDriverActions.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.scComputers.Panel1.SuspendLayout()
        Me.scComputers.Panel2.SuspendLayout()
        Me.scComputers.SuspendLayout()
        Me.pnlComputerDetailContainer.SuspendLayout()
        Me.pnlNoSelectedComputer.SuspendLayout()
        Me.pnlSelectedComputer.SuspendLayout()
        Me.pnlComputerActions.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.pnlDriverPackDetails.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'msActions
        '
        Me.msActions.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.DriversToolStripMenuItem, Me.ComputersToolStripMenuItem, Me.OptionsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.msActions.Location = New System.Drawing.Point(0, 0)
        Me.msActions.Name = "msActions"
        Me.msActions.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.msActions.Size = New System.Drawing.Size(1272, 24)
        Me.msActions.TabIndex = 0
        Me.msActions.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewProjectToolStripMenuItem, Me.OpenExistingProjectToolStripMenuItem, Me.ToolStripSeparator2, Me.SaveProjectToolStripMenuItem, Me.ToolStripSeparator5, Me.BuildDriverPackToolStripMenuItem, Me.CreateProjectFromDriverPackToolStripMenuItem, Me.ToolStripSeparator6, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'NewProjectToolStripMenuItem
        '
        Me.NewProjectToolStripMenuItem.Name = "NewProjectToolStripMenuItem"
        Me.NewProjectToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.NewProjectToolStripMenuItem.Size = New System.Drawing.Size(248, 22)
        Me.NewProjectToolStripMenuItem.Text = "New Project..."
        '
        'OpenExistingProjectToolStripMenuItem
        '
        Me.OpenExistingProjectToolStripMenuItem.Name = "OpenExistingProjectToolStripMenuItem"
        Me.OpenExistingProjectToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.OpenExistingProjectToolStripMenuItem.Size = New System.Drawing.Size(248, 22)
        Me.OpenExistingProjectToolStripMenuItem.Text = "Open Existing Project..."
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(245, 6)
        '
        'SaveProjectToolStripMenuItem
        '
        Me.SaveProjectToolStripMenuItem.Enabled = False
        Me.SaveProjectToolStripMenuItem.Name = "SaveProjectToolStripMenuItem"
        Me.SaveProjectToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.SaveProjectToolStripMenuItem.Size = New System.Drawing.Size(248, 22)
        Me.SaveProjectToolStripMenuItem.Text = "Save Project"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(245, 6)
        '
        'BuildDriverPackToolStripMenuItem
        '
        Me.BuildDriverPackToolStripMenuItem.Enabled = False
        Me.BuildDriverPackToolStripMenuItem.Name = "BuildDriverPackToolStripMenuItem"
        Me.BuildDriverPackToolStripMenuItem.Size = New System.Drawing.Size(248, 22)
        Me.BuildDriverPackToolStripMenuItem.Text = "Build Driver Pack..."
        '
        'CreateProjectFromDriverPackToolStripMenuItem
        '
        Me.CreateProjectFromDriverPackToolStripMenuItem.Name = "CreateProjectFromDriverPackToolStripMenuItem"
        Me.CreateProjectFromDriverPackToolStripMenuItem.Size = New System.Drawing.Size(248, 22)
        Me.CreateProjectFromDriverPackToolStripMenuItem.Text = "Create project from Driver Pack..."
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(245, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Q), System.Windows.Forms.Keys)
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(248, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'DriversToolStripMenuItem
        '
        Me.DriversToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddDriverToolStripMenuItem, Me.AddDriversInBulkToolStripMenuItem, Me.ToolStripSeparator7, Me.ReuseDriverFromDriverPackToolStripMenuItem})
        Me.DriversToolStripMenuItem.Name = "DriversToolStripMenuItem"
        Me.DriversToolStripMenuItem.Size = New System.Drawing.Size(55, 20)
        Me.DriversToolStripMenuItem.Text = "Drivers"
        Me.DriversToolStripMenuItem.Visible = False
        '
        'AddDriverToolStripMenuItem
        '
        Me.AddDriverToolStripMenuItem.Name = "AddDriverToolStripMenuItem"
        Me.AddDriverToolStripMenuItem.Size = New System.Drawing.Size(239, 22)
        Me.AddDriverToolStripMenuItem.Text = "Add Driver..."
        '
        'AddDriversInBulkToolStripMenuItem
        '
        Me.AddDriversInBulkToolStripMenuItem.Name = "AddDriversInBulkToolStripMenuItem"
        Me.AddDriversInBulkToolStripMenuItem.Size = New System.Drawing.Size(239, 22)
        Me.AddDriversInBulkToolStripMenuItem.Text = "Add Drivers in Bulk"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(236, 6)
        '
        'ReuseDriverFromDriverPackToolStripMenuItem
        '
        Me.ReuseDriverFromDriverPackToolStripMenuItem.Name = "ReuseDriverFromDriverPackToolStripMenuItem"
        Me.ReuseDriverFromDriverPackToolStripMenuItem.Size = New System.Drawing.Size(239, 22)
        Me.ReuseDriverFromDriverPackToolStripMenuItem.Text = "Reuse Driver from Driver Pack..."
        '
        'ComputersToolStripMenuItem
        '
        Me.ComputersToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddComputerToolStripMenuItem, Me.AddComputerFromManufacturerModelCatalogsToolStripMenuItem, Me.AddCurrentComputerToolStripMenuItem, Me.ToolStripSeparator9, Me.ReuseComputerToolStripMenuItem, Me.ToolStripSeparator1, Me.ManageKnownComputersToolStripMenuItem, Me.ToolStripSeparator8, Me.TESTMODELCATALOGSToolStripMenuItem})
        Me.ComputersToolStripMenuItem.Name = "ComputersToolStripMenuItem"
        Me.ComputersToolStripMenuItem.Size = New System.Drawing.Size(78, 20)
        Me.ComputersToolStripMenuItem.Text = "Computers"
        Me.ComputersToolStripMenuItem.Visible = False
        '
        'AddComputerToolStripMenuItem
        '
        Me.AddComputerToolStripMenuItem.Name = "AddComputerToolStripMenuItem"
        Me.AddComputerToolStripMenuItem.Size = New System.Drawing.Size(352, 22)
        Me.AddComputerToolStripMenuItem.Text = "Add Computer..."
        '
        'AddComputerFromManufacturerModelCatalogsToolStripMenuItem
        '
        Me.AddComputerFromManufacturerModelCatalogsToolStripMenuItem.Name = "AddComputerFromManufacturerModelCatalogsToolStripMenuItem"
        Me.AddComputerFromManufacturerModelCatalogsToolStripMenuItem.Size = New System.Drawing.Size(352, 22)
        Me.AddComputerFromManufacturerModelCatalogsToolStripMenuItem.Text = "Add Computer from Manufacturer Model Catalogs..."
        '
        'AddCurrentComputerToolStripMenuItem
        '
        Me.AddCurrentComputerToolStripMenuItem.Name = "AddCurrentComputerToolStripMenuItem"
        Me.AddCurrentComputerToolStripMenuItem.Size = New System.Drawing.Size(352, 22)
        Me.AddCurrentComputerToolStripMenuItem.Text = "Add Current Computer..."
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(349, 6)
        '
        'ReuseComputerToolStripMenuItem
        '
        Me.ReuseComputerToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FromKnownComputersToolStripMenuItem, Me.FromAnotherDriverPackToolStripMenuItem})
        Me.ReuseComputerToolStripMenuItem.Name = "ReuseComputerToolStripMenuItem"
        Me.ReuseComputerToolStripMenuItem.Size = New System.Drawing.Size(352, 22)
        Me.ReuseComputerToolStripMenuItem.Text = "Reuse Computer..."
        '
        'FromKnownComputersToolStripMenuItem
        '
        Me.FromKnownComputersToolStripMenuItem.Name = "FromKnownComputersToolStripMenuItem"
        Me.FromKnownComputersToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.FromKnownComputersToolStripMenuItem.Text = "From known computers"
        '
        'FromAnotherDriverPackToolStripMenuItem
        '
        Me.FromAnotherDriverPackToolStripMenuItem.Name = "FromAnotherDriverPackToolStripMenuItem"
        Me.FromAnotherDriverPackToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.FromAnotherDriverPackToolStripMenuItem.Text = "From another Driver Pack"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(349, 6)
        '
        'ManageKnownComputersToolStripMenuItem
        '
        Me.ManageKnownComputersToolStripMenuItem.Name = "ManageKnownComputersToolStripMenuItem"
        Me.ManageKnownComputersToolStripMenuItem.Size = New System.Drawing.Size(352, 22)
        Me.ManageKnownComputersToolStripMenuItem.Text = "Manage Known Computers"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(349, 6)
        '
        'TESTMODELCATALOGSToolStripMenuItem
        '
        Me.TESTMODELCATALOGSToolStripMenuItem.Name = "TESTMODELCATALOGSToolStripMenuItem"
        Me.TESTMODELCATALOGSToolStripMenuItem.Size = New System.Drawing.Size(352, 22)
        Me.TESTMODELCATALOGSToolStripMenuItem.Text = "TEST MODEL CATALOGS"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpTopicsToolStripMenuItem, Me.ToolStripSeparator10, Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'pnlHomeScreen
        '
        Me.pnlHomeScreen.Controls.Add(Me.TableLayoutPanel1)
        Me.pnlHomeScreen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlHomeScreen.Location = New System.Drawing.Point(0, 24)
        Me.pnlHomeScreen.Name = "pnlHomeScreen"
        Me.pnlHomeScreen.Size = New System.Drawing.Size(1272, 669)
        Me.pnlHomeScreen.TabIndex = 1
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel2, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel3, 1, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(356, 175)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.1536!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.8464!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(561, 319)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Panel1
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.Panel1, 2)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(555, 86)
        Me.Panel1.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(294, 39)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Driver Pack Creator"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(119, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Welcome to the"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.lnkImportProject)
        Me.Panel2.Controls.Add(Me.lnkOpenProject)
        Me.Panel2.Controls.Add(Me.lnkNewProject)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 95)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(274, 221)
        Me.Panel2.TabIndex = 0
        '
        'lnkImportProject
        '
        Me.lnkImportProject.AutoEllipsis = True
        Me.lnkImportProject.Location = New System.Drawing.Point(20, 72)
        Me.lnkImportProject.Name = "lnkImportProject"
        Me.lnkImportProject.Size = New System.Drawing.Size(236, 45)
        Me.lnkImportProject.TabIndex = 0
        Me.lnkImportProject.TabStop = True
        Me.lnkImportProject.Text = "Create a Driver Pack project from an existing Driver Pack..."
        '
        'lnkOpenProject
        '
        Me.lnkOpenProject.AutoSize = True
        Me.lnkOpenProject.Location = New System.Drawing.Point(20, 48)
        Me.lnkOpenProject.Name = "lnkOpenProject"
        Me.lnkOpenProject.Size = New System.Drawing.Size(194, 13)
        Me.lnkOpenProject.TabIndex = 0
        Me.lnkOpenProject.TabStop = True
        Me.lnkOpenProject.Text = "Open an existing Driver Pack project..."
        '
        'lnkNewProject
        '
        Me.lnkNewProject.AutoSize = True
        Me.lnkNewProject.Location = New System.Drawing.Point(20, 24)
        Me.lnkNewProject.Name = "lnkNewProject"
        Me.lnkNewProject.Size = New System.Drawing.Size(178, 13)
        Me.lnkNewProject.TabIndex = 0
        Me.lnkNewProject.TabStop = True
        Me.lnkNewProject.Text = "Create a new Driver Pack project..."
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.lvRecentProjects)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Location = New System.Drawing.Point(283, 95)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(275, 221)
        Me.Panel3.TabIndex = 0
        '
        'lvRecentProjects
        '
        Me.lvRecentProjects.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chProjectName})
        Me.lvRecentProjects.FullRowSelect = True
        Me.lvRecentProjects.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvRecentProjects.Location = New System.Drawing.Point(15, 48)
        Me.lvRecentProjects.MultiSelect = False
        Me.lvRecentProjects.Name = "lvRecentProjects"
        Me.lvRecentProjects.Size = New System.Drawing.Size(244, 156)
        Me.lvRecentProjects.TabIndex = 1
        Me.lvRecentProjects.UseCompatibleStateImageBehavior = False
        Me.lvRecentProjects.View = System.Windows.Forms.View.Details
        '
        'chProjectName
        '
        Me.chProjectName.Text = "Name"
        Me.chProjectName.Width = 192
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 12)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(97, 16)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Recent Projects"
        '
        'pnlProjectScreen
        '
        Me.pnlProjectScreen.Controls.Add(Me.tcProjectItems)
        Me.pnlProjectScreen.Controls.Add(Me.Panel4)
        Me.pnlProjectScreen.Controls.Add(Me.ToolStrip1)
        Me.pnlProjectScreen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlProjectScreen.Location = New System.Drawing.Point(0, 24)
        Me.pnlProjectScreen.Name = "pnlProjectScreen"
        Me.pnlProjectScreen.Size = New System.Drawing.Size(1272, 669)
        Me.pnlProjectScreen.TabIndex = 2
        '
        'tcProjectItems
        '
        Me.tcProjectItems.Controls.Add(Me.TabPage1)
        Me.tcProjectItems.Controls.Add(Me.TabPage2)
        Me.tcProjectItems.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcProjectItems.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tcProjectItems.Location = New System.Drawing.Point(0, 153)
        Me.tcProjectItems.Name = "tcProjectItems"
        Me.tcProjectItems.SelectedIndex = 0
        Me.tcProjectItems.Size = New System.Drawing.Size(1272, 516)
        Me.tcProjectItems.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.scDrivers)
        Me.TabPage1.Controls.Add(Me.Panel5)
        Me.TabPage1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1264, 490)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Driver Files"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'scDrivers
        '
        Me.scDrivers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scDrivers.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.scDrivers.Location = New System.Drawing.Point(3, 31)
        Me.scDrivers.Name = "scDrivers"
        '
        'scDrivers.Panel1
        '
        Me.scDrivers.Panel1.Controls.Add(Me.lvDriverFiles)
        '
        'scDrivers.Panel2
        '
        Me.scDrivers.Panel2.Controls.Add(Me.pnlDriverDetailContainer)
        Me.scDrivers.Size = New System.Drawing.Size(1258, 456)
        Me.scDrivers.SplitterDistance = 419
        Me.scDrivers.TabIndex = 1
        '
        'lvDriverFiles
        '
        Me.lvDriverFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chMainDriverFile, Me.chDriverName})
        Me.lvDriverFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvDriverFiles.FullRowSelect = True
        Me.lvDriverFiles.Location = New System.Drawing.Point(0, 0)
        Me.lvDriverFiles.MultiSelect = False
        Me.lvDriverFiles.Name = "lvDriverFiles"
        Me.lvDriverFiles.Size = New System.Drawing.Size(419, 456)
        Me.lvDriverFiles.TabIndex = 0
        Me.lvDriverFiles.UseCompatibleStateImageBehavior = False
        Me.lvDriverFiles.View = System.Windows.Forms.View.Details
        '
        'chMainDriverFile
        '
        Me.chMainDriverFile.Text = "Driver File"
        Me.chMainDriverFile.Width = 118
        '
        'chDriverName
        '
        Me.chDriverName.Text = "Driver Name"
        Me.chDriverName.Width = 267
        '
        'pnlDriverDetailContainer
        '
        Me.pnlDriverDetailContainer.Controls.Add(Me.pnlNoSelectedDriver)
        Me.pnlDriverDetailContainer.Controls.Add(Me.pnlSelectedDriver)
        Me.pnlDriverDetailContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlDriverDetailContainer.Location = New System.Drawing.Point(0, 0)
        Me.pnlDriverDetailContainer.Name = "pnlDriverDetailContainer"
        Me.pnlDriverDetailContainer.Size = New System.Drawing.Size(835, 456)
        Me.pnlDriverDetailContainer.TabIndex = 0
        '
        'pnlNoSelectedDriver
        '
        Me.pnlNoSelectedDriver.Controls.Add(Me.lnkAddDrvFiles)
        Me.pnlNoSelectedDriver.Controls.Add(Me.Label8)
        Me.pnlNoSelectedDriver.Controls.Add(Me.Label7)
        Me.pnlNoSelectedDriver.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlNoSelectedDriver.Location = New System.Drawing.Point(0, 0)
        Me.pnlNoSelectedDriver.Name = "pnlNoSelectedDriver"
        Me.pnlNoSelectedDriver.Size = New System.Drawing.Size(835, 456)
        Me.pnlNoSelectedDriver.TabIndex = 0
        '
        'lnkAddDrvFiles
        '
        Me.lnkAddDrvFiles.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lnkAddDrvFiles.AutoSize = True
        Me.lnkAddDrvFiles.Location = New System.Drawing.Point(370, 289)
        Me.lnkAddDrvFiles.Name = "lnkAddDrvFiles"
        Me.lnkAddDrvFiles.Size = New System.Drawing.Size(94, 13)
        Me.lnkAddDrvFiles.TabIndex = 1
        Me.lnkAddDrvFiles.TabStop = True
        Me.lnkAddDrvFiles.Text = "Add Driver Files..."
        '
        'Label8
        '
        Me.Label8.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label8.AutoEllipsis = True
        Me.Label8.Location = New System.Drawing.Point(147, 201)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(546, 72)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "To modify details about a driver file in this Driver Pack, select an entry on the" & _
            " left. From there, you will be able to remove driver files." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "To add your first" & _
            " driver file, click the following link:"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label7
        '
        Me.Label7.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label7.AutoEllipsis = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(150, 155)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(543, 32)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "No driver files have been selected"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlSelectedDriver
        '
        Me.pnlSelectedDriver.Controls.Add(Me.btnManageSubdrivers)
        Me.pnlSelectedDriver.Controls.Add(Me.lbDriverSubdrivers)
        Me.pnlSelectedDriver.Controls.Add(Me.btnChangeMainDriverFile)
        Me.pnlSelectedDriver.Controls.Add(Me.tbDriverMainFile)
        Me.pnlSelectedDriver.Controls.Add(Me.Label20)
        Me.pnlSelectedDriver.Controls.Add(Me.Label19)
        Me.pnlSelectedDriver.Controls.Add(Me.tbDriverManufacturer)
        Me.pnlSelectedDriver.Controls.Add(Me.Label18)
        Me.pnlSelectedDriver.Controls.Add(Me.tbDriverName)
        Me.pnlSelectedDriver.Controls.Add(Me.Label17)
        Me.pnlSelectedDriver.Controls.Add(Me.pnlDriverActions)
        Me.pnlSelectedDriver.Controls.Add(Me.Label16)
        Me.pnlSelectedDriver.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSelectedDriver.Location = New System.Drawing.Point(0, 0)
        Me.pnlSelectedDriver.Name = "pnlSelectedDriver"
        Me.pnlSelectedDriver.Size = New System.Drawing.Size(835, 456)
        Me.pnlSelectedDriver.TabIndex = 1
        '
        'btnManageSubdrivers
        '
        Me.btnManageSubdrivers.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnManageSubdrivers.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnManageSubdrivers.Location = New System.Drawing.Point(202, 350)
        Me.btnManageSubdrivers.Name = "btnManageSubdrivers"
        Me.btnManageSubdrivers.Size = New System.Drawing.Size(143, 23)
        Me.btnManageSubdrivers.TabIndex = 8
        Me.btnManageSubdrivers.Text = "Manage subdrivers..."
        Me.btnManageSubdrivers.UseVisualStyleBackColor = True
        '
        'lbDriverSubdrivers
        '
        Me.lbDriverSubdrivers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbDriverSubdrivers.FormattingEnabled = True
        Me.lbDriverSubdrivers.IntegralHeight = False
        Me.lbDriverSubdrivers.Location = New System.Drawing.Point(202, 151)
        Me.lbDriverSubdrivers.Name = "lbDriverSubdrivers"
        Me.lbDriverSubdrivers.ScrollAlwaysVisible = True
        Me.lbDriverSubdrivers.Size = New System.Drawing.Size(582, 189)
        Me.lbDriverSubdrivers.TabIndex = 6
        '
        'btnChangeMainDriverFile
        '
        Me.btnChangeMainDriverFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnChangeMainDriverFile.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnChangeMainDriverFile.Location = New System.Drawing.Point(709, 122)
        Me.btnChangeMainDriverFile.Name = "btnChangeMainDriverFile"
        Me.btnChangeMainDriverFile.Size = New System.Drawing.Size(75, 23)
        Me.btnChangeMainDriverFile.TabIndex = 5
        Me.btnChangeMainDriverFile.Text = "Change..."
        Me.btnChangeMainDriverFile.UseVisualStyleBackColor = True
        '
        'tbDriverMainFile
        '
        Me.tbDriverMainFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDriverMainFile.Location = New System.Drawing.Point(202, 123)
        Me.tbDriverMainFile.Name = "tbDriverMainFile"
        Me.tbDriverMainFile.ReadOnly = True
        Me.tbDriverMainFile.Size = New System.Drawing.Size(501, 21)
        Me.tbDriverMainFile.TabIndex = 4
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(50, 153)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(62, 13)
        Me.Label20.TabIndex = 3
        Me.Label20.Text = "Subdrivers:"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(50, 126)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(84, 13)
        Me.Label19.TabIndex = 3
        Me.Label19.Text = "Main Driver File:"
        '
        'tbDriverManufacturer
        '
        Me.tbDriverManufacturer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDriverManufacturer.Location = New System.Drawing.Point(202, 96)
        Me.tbDriverManufacturer.Name = "tbDriverManufacturer"
        Me.tbDriverManufacturer.Size = New System.Drawing.Size(583, 21)
        Me.tbDriverManufacturer.TabIndex = 4
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(50, 99)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(114, 13)
        Me.Label18.TabIndex = 3
        Me.Label18.Text = "Manufacturer/Vendor:"
        '
        'tbDriverName
        '
        Me.tbDriverName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDriverName.Location = New System.Drawing.Point(202, 69)
        Me.tbDriverName.Name = "tbDriverName"
        Me.tbDriverName.Size = New System.Drawing.Size(583, 21)
        Me.tbDriverName.TabIndex = 4
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(50, 72)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(38, 13)
        Me.Label17.TabIndex = 3
        Me.Label17.Text = "Name:"
        '
        'pnlDriverActions
        '
        Me.pnlDriverActions.Controls.Add(Me.btnSaveDriver)
        Me.pnlDriverActions.Controls.Add(Me.btnRemoveDriver)
        Me.pnlDriverActions.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlDriverActions.Location = New System.Drawing.Point(0, 408)
        Me.pnlDriverActions.Name = "pnlDriverActions"
        Me.pnlDriverActions.Size = New System.Drawing.Size(835, 48)
        Me.pnlDriverActions.TabIndex = 2
        '
        'btnSaveDriver
        '
        Me.btnSaveDriver.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveDriver.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnSaveDriver.Location = New System.Drawing.Point(650, 12)
        Me.btnSaveDriver.Name = "btnSaveDriver"
        Me.btnSaveDriver.Size = New System.Drawing.Size(174, 23)
        Me.btnSaveDriver.TabIndex = 5
        Me.btnSaveDriver.Text = "Save Driver"
        Me.btnSaveDriver.UseVisualStyleBackColor = True
        '
        'btnRemoveDriver
        '
        Me.btnRemoveDriver.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnRemoveDriver.Location = New System.Drawing.Point(12, 12)
        Me.btnRemoveDriver.Name = "btnRemoveDriver"
        Me.btnRemoveDriver.Size = New System.Drawing.Size(174, 23)
        Me.btnRemoveDriver.TabIndex = 5
        Me.btnRemoveDriver.Text = "Delete Driver"
        Me.btnRemoveDriver.UseVisualStyleBackColor = True
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(24, 24)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(137, 25)
        Me.Label16.TabIndex = 1
        Me.Label16.Text = "Driver Details"
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.Label5)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Location = New System.Drawing.Point(3, 3)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(1258, 28)
        Me.Panel5.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(8, 8)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(375, 13)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "In this tab, you can add the source driver files to include in your Driver Pack."
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.scComputers)
        Me.TabPage2.Controls.Add(Me.Panel6)
        Me.TabPage2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(1264, 490)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Computers"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'scComputers
        '
        Me.scComputers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scComputers.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.scComputers.Location = New System.Drawing.Point(3, 31)
        Me.scComputers.Name = "scComputers"
        '
        'scComputers.Panel1
        '
        Me.scComputers.Panel1.Controls.Add(Me.lvComputers)
        '
        'scComputers.Panel2
        '
        Me.scComputers.Panel2.Controls.Add(Me.pnlComputerDetailContainer)
        Me.scComputers.Size = New System.Drawing.Size(1258, 456)
        Me.scComputers.SplitterDistance = 419
        Me.scComputers.TabIndex = 4
        '
        'lvComputers
        '
        Me.lvComputers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chComputer})
        Me.lvComputers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvComputers.FullRowSelect = True
        Me.lvComputers.Location = New System.Drawing.Point(0, 0)
        Me.lvComputers.MultiSelect = False
        Me.lvComputers.Name = "lvComputers"
        Me.lvComputers.Size = New System.Drawing.Size(419, 456)
        Me.lvComputers.TabIndex = 0
        Me.lvComputers.UseCompatibleStateImageBehavior = False
        Me.lvComputers.View = System.Windows.Forms.View.Details
        '
        'chComputer
        '
        Me.chComputer.Text = "Computer"
        Me.chComputer.Width = 384
        '
        'pnlComputerDetailContainer
        '
        Me.pnlComputerDetailContainer.Controls.Add(Me.pnlNoSelectedComputer)
        Me.pnlComputerDetailContainer.Controls.Add(Me.pnlSelectedComputer)
        Me.pnlComputerDetailContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlComputerDetailContainer.Location = New System.Drawing.Point(0, 0)
        Me.pnlComputerDetailContainer.Name = "pnlComputerDetailContainer"
        Me.pnlComputerDetailContainer.Size = New System.Drawing.Size(835, 456)
        Me.pnlComputerDetailContainer.TabIndex = 0
        '
        'pnlNoSelectedComputer
        '
        Me.pnlNoSelectedComputer.Controls.Add(Me.lnkAddComputer)
        Me.pnlNoSelectedComputer.Controls.Add(Me.Label12)
        Me.pnlNoSelectedComputer.Controls.Add(Me.Label13)
        Me.pnlNoSelectedComputer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlNoSelectedComputer.Location = New System.Drawing.Point(0, 0)
        Me.pnlNoSelectedComputer.Name = "pnlNoSelectedComputer"
        Me.pnlNoSelectedComputer.Size = New System.Drawing.Size(835, 456)
        Me.pnlNoSelectedComputer.TabIndex = 0
        '
        'lnkAddComputer
        '
        Me.lnkAddComputer.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lnkAddComputer.AutoSize = True
        Me.lnkAddComputer.Location = New System.Drawing.Point(373, 289)
        Me.lnkAddComputer.Name = "lnkAddComputer"
        Me.lnkAddComputer.Size = New System.Drawing.Size(88, 13)
        Me.lnkAddComputer.TabIndex = 1
        Me.lnkAddComputer.TabStop = True
        Me.lnkAddComputer.Text = "Add Computer..."
        '
        'Label12
        '
        Me.Label12.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label12.AutoEllipsis = True
        Me.Label12.Location = New System.Drawing.Point(147, 201)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(546, 72)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "To modify details about a computer in this Driver Pack, select an entry on the le" & _
            "ft. From there, you will be able to remove computer entries." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "To add your firs" & _
            "t computer, click the following link:"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label13
        '
        Me.Label13.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label13.AutoEllipsis = True
        Me.Label13.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(150, 155)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(543, 32)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "No computers have been selected"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlSelectedComputer
        '
        Me.pnlSelectedComputer.Controls.Add(Me.btnChangeComputerSoftware)
        Me.pnlSelectedComputer.Controls.Add(Me.btnChangeComputerHardware)
        Me.pnlSelectedComputer.Controls.Add(Me.lvAssociatedDriverFiles)
        Me.pnlSelectedComputer.Controls.Add(Me.tbComputerUuid)
        Me.pnlSelectedComputer.Controls.Add(Me.Label21)
        Me.pnlSelectedComputer.Controls.Add(Me.Label26)
        Me.pnlSelectedComputer.Controls.Add(Me.Label22)
        Me.pnlSelectedComputer.Controls.Add(Me.tbComputerModel)
        Me.pnlSelectedComputer.Controls.Add(Me.Label23)
        Me.pnlSelectedComputer.Controls.Add(Me.tbComputerManufacturer)
        Me.pnlSelectedComputer.Controls.Add(Me.Label24)
        Me.pnlSelectedComputer.Controls.Add(Me.pnlComputerActions)
        Me.pnlSelectedComputer.Controls.Add(Me.Label25)
        Me.pnlSelectedComputer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSelectedComputer.Location = New System.Drawing.Point(0, 0)
        Me.pnlSelectedComputer.Name = "pnlSelectedComputer"
        Me.pnlSelectedComputer.Size = New System.Drawing.Size(835, 456)
        Me.pnlSelectedComputer.TabIndex = 1
        '
        'btnChangeComputerSoftware
        '
        Me.btnChangeComputerSoftware.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnChangeComputerSoftware.Location = New System.Drawing.Point(382, 150)
        Me.btnChangeComputerSoftware.Name = "btnChangeComputerSoftware"
        Me.btnChangeComputerSoftware.Size = New System.Drawing.Size(174, 23)
        Me.btnChangeComputerSoftware.TabIndex = 5
        Me.btnChangeComputerSoftware.Text = "Change Software Information..."
        Me.btnChangeComputerSoftware.UseVisualStyleBackColor = True
        '
        'btnChangeComputerHardware
        '
        Me.btnChangeComputerHardware.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnChangeComputerHardware.Location = New System.Drawing.Point(202, 150)
        Me.btnChangeComputerHardware.Name = "btnChangeComputerHardware"
        Me.btnChangeComputerHardware.Size = New System.Drawing.Size(174, 23)
        Me.btnChangeComputerHardware.TabIndex = 5
        Me.btnChangeComputerHardware.Text = "Change Hardware Information..."
        Me.btnChangeComputerHardware.UseVisualStyleBackColor = True
        '
        'lvAssociatedDriverFiles
        '
        Me.lvAssociatedDriverFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvAssociatedDriverFiles.CheckBoxes = True
        Me.lvAssociatedDriverFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chDriverFriendlyName, Me.chDriverMainFile})
        Me.lvAssociatedDriverFiles.FullRowSelect = True
        Me.lvAssociatedDriverFiles.Location = New System.Drawing.Point(202, 179)
        Me.lvAssociatedDriverFiles.Name = "lvAssociatedDriverFiles"
        Me.lvAssociatedDriverFiles.Size = New System.Drawing.Size(582, 195)
        Me.lvAssociatedDriverFiles.TabIndex = 18
        Me.lvAssociatedDriverFiles.UseCompatibleStateImageBehavior = False
        Me.lvAssociatedDriverFiles.View = System.Windows.Forms.View.Details
        '
        'chDriverFriendlyName
        '
        Me.chDriverFriendlyName.Text = "Name"
        Me.chDriverFriendlyName.Width = 256
        '
        'chDriverMainFile
        '
        Me.chDriverMainFile.Text = "Driver File"
        Me.chDriverMainFile.Width = 256
        '
        'tbComputerUuid
        '
        Me.tbComputerUuid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbComputerUuid.Location = New System.Drawing.Point(202, 123)
        Me.tbComputerUuid.MaxLength = 36
        Me.tbComputerUuid.Name = "tbComputerUuid"
        Me.tbComputerUuid.Size = New System.Drawing.Size(583, 21)
        Me.tbComputerUuid.TabIndex = 15
        '
        'Label21
        '
        Me.Label21.AutoEllipsis = True
        Me.Label21.Location = New System.Drawing.Point(50, 184)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(136, 32)
        Me.Label21.TabIndex = 13
        Me.Label21.Text = "Drivers to install with this computer:"
        '
        'Label26
        '
        Me.Label26.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(200, 381)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(312, 13)
        Me.Label26.TabIndex = 10
        Me.Label26.Text = "Add drivers to the Driver Pack in order to select them in the list."
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(50, 126)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(36, 13)
        Me.Label22.TabIndex = 10
        Me.Label22.Text = "UUID:"
        '
        'tbComputerModel
        '
        Me.tbComputerModel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbComputerModel.Location = New System.Drawing.Point(202, 96)
        Me.tbComputerModel.Name = "tbComputerModel"
        Me.tbComputerModel.Size = New System.Drawing.Size(583, 21)
        Me.tbComputerModel.TabIndex = 16
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(50, 99)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(39, 13)
        Me.Label23.TabIndex = 11
        Me.Label23.Text = "Model:"
        '
        'tbComputerManufacturer
        '
        Me.tbComputerManufacturer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbComputerManufacturer.Location = New System.Drawing.Point(202, 69)
        Me.tbComputerManufacturer.Name = "tbComputerManufacturer"
        Me.tbComputerManufacturer.Size = New System.Drawing.Size(583, 21)
        Me.tbComputerManufacturer.TabIndex = 14
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(50, 72)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(76, 13)
        Me.Label24.TabIndex = 12
        Me.Label24.Text = "Manufacturer:"
        '
        'pnlComputerActions
        '
        Me.pnlComputerActions.Controls.Add(Me.btnSaveComputer)
        Me.pnlComputerActions.Controls.Add(Me.btnRemoveComputer)
        Me.pnlComputerActions.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlComputerActions.Location = New System.Drawing.Point(0, 408)
        Me.pnlComputerActions.Name = "pnlComputerActions"
        Me.pnlComputerActions.Size = New System.Drawing.Size(835, 48)
        Me.pnlComputerActions.TabIndex = 9
        '
        'btnSaveComputer
        '
        Me.btnSaveComputer.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveComputer.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnSaveComputer.Location = New System.Drawing.Point(650, 12)
        Me.btnSaveComputer.Name = "btnSaveComputer"
        Me.btnSaveComputer.Size = New System.Drawing.Size(174, 23)
        Me.btnSaveComputer.TabIndex = 5
        Me.btnSaveComputer.Text = "Save Computer"
        Me.btnSaveComputer.UseVisualStyleBackColor = True
        '
        'btnRemoveComputer
        '
        Me.btnRemoveComputer.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnRemoveComputer.Location = New System.Drawing.Point(12, 12)
        Me.btnRemoveComputer.Name = "btnRemoveComputer"
        Me.btnRemoveComputer.Size = New System.Drawing.Size(174, 23)
        Me.btnRemoveComputer.TabIndex = 5
        Me.btnRemoveComputer.Text = "Remove Computer"
        Me.btnRemoveComputer.UseVisualStyleBackColor = True
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(24, 24)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(173, 25)
        Me.Label25.TabIndex = 8
        Me.Label25.Text = "Computer Details"
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.Label6)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel6.Location = New System.Drawing.Point(3, 3)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(1258, 28)
        Me.Panel6.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(8, 8)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(481, 13)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "In this tab, you can associate your driver files with one or more target devices " & _
            "you configure here."
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.TableLayoutPanel2)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(0, 25)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1272, 128)
        Me.Panel4.TabIndex = 1
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.28931!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.71069!))
        Me.TableLayoutPanel2.Controls.Add(Me.Panel7, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.pnlDriverPackDetails, 1, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(1272, 128)
        Me.TableLayoutPanel2.TabIndex = 3
        '
        'Panel7
        '
        Me.Panel7.Controls.Add(Me.Label14)
        Me.Panel7.Controls.Add(Me.Label15)
        Me.Panel7.Controls.Add(Me.tbProjectName)
        Me.Panel7.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel7.Location = New System.Drawing.Point(3, 3)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(392, 122)
        Me.Panel7.TabIndex = 0
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(10, 10)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(161, 13)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Driver Pack Project Information:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(20, 38)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(38, 13)
        Me.Label15.TabIndex = 1
        Me.Label15.Text = "Name:"
        '
        'tbProjectName
        '
        Me.tbProjectName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbProjectName.Location = New System.Drawing.Point(64, 35)
        Me.tbProjectName.Name = "tbProjectName"
        Me.tbProjectName.Size = New System.Drawing.Size(294, 21)
        Me.tbProjectName.TabIndex = 2
        '
        'pnlDriverPackDetails
        '
        Me.pnlDriverPackDetails.Controls.Add(Me.tbDriverPackAuthor)
        Me.pnlDriverPackDetails.Controls.Add(Me.Label4)
        Me.pnlDriverPackDetails.Controls.Add(Me.Label11)
        Me.pnlDriverPackDetails.Controls.Add(Me.Label9)
        Me.pnlDriverPackDetails.Controls.Add(Me.tbDriverPackDescription)
        Me.pnlDriverPackDetails.Controls.Add(Me.tbDriverPackName)
        Me.pnlDriverPackDetails.Controls.Add(Me.Label10)
        Me.pnlDriverPackDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlDriverPackDetails.Location = New System.Drawing.Point(401, 3)
        Me.pnlDriverPackDetails.Name = "pnlDriverPackDetails"
        Me.pnlDriverPackDetails.Size = New System.Drawing.Size(868, 122)
        Me.pnlDriverPackDetails.TabIndex = 0
        '
        'tbDriverPackAuthor
        '
        Me.tbDriverPackAuthor.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDriverPackAuthor.Location = New System.Drawing.Point(494, 35)
        Me.tbDriverPackAuthor.Name = "tbDriverPackAuthor"
        Me.tbDriverPackAuthor.Size = New System.Drawing.Size(342, 21)
        Me.tbDriverPackAuthor.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(10, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(124, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Driver Pack Information:"
        '
        'Label11
        '
        Me.Label11.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(444, 38)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(44, 13)
        Me.Label11.TabIndex = 1
        Me.Label11.Text = "Author:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(38, 38)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(38, 13)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "Name:"
        '
        'tbDriverPackDescription
        '
        Me.tbDriverPackDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDriverPackDescription.Location = New System.Drawing.Point(82, 62)
        Me.tbDriverPackDescription.Multiline = True
        Me.tbDriverPackDescription.Name = "tbDriverPackDescription"
        Me.tbDriverPackDescription.Size = New System.Drawing.Size(754, 49)
        Me.tbDriverPackDescription.TabIndex = 2
        '
        'tbDriverPackName
        '
        Me.tbDriverPackName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbDriverPackName.Location = New System.Drawing.Point(82, 35)
        Me.tbDriverPackName.Name = "tbDriverPackName"
        Me.tbDriverPackName.Size = New System.Drawing.Size(342, 21)
        Me.tbDriverPackName.TabIndex = 2
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(12, 65)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(64, 13)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = "Description:"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tbSave, Me.ToolStripSeparator3, Me.tbUnload, Me.ToolStripSeparator4, Me.tbBuild})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1272, 25)
        Me.ToolStrip1.TabIndex = 2
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tbSave
        '
        Me.tbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tbSave.Image = Global.DrvPakCreator.My.Resources.Resources.savefile
        Me.tbSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tbSave.Name = "tbSave"
        Me.tbSave.Size = New System.Drawing.Size(23, 22)
        Me.tbSave.Text = "Save Driver Pack Project"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'tbUnload
        '
        Me.tbUnload.Image = Global.DrvPakCreator.My.Resources.Resources.unload_project
        Me.tbUnload.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tbUnload.Name = "tbUnload"
        Me.tbUnload.Size = New System.Drawing.Size(105, 22)
        Me.tbUnload.Text = "Unload Project"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'tbBuild
        '
        Me.tbBuild.Image = Global.DrvPakCreator.My.Resources.Resources.build_pack
        Me.tbBuild.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tbBuild.Name = "tbBuild"
        Me.tbBuild.Size = New System.Drawing.Size(116, 22)
        Me.tbBuild.Text = "Build Driver Pack"
        '
        'ofdProject
        '
        Me.ofdProject.Filter = "Driver Pack Projects|*.pakproj"
        '
        'sfdDrvPackBuild
        '
        Me.sfdDrvPackBuild.DefaultExt = "dtdrvpak"
        Me.sfdDrvPackBuild.Filter = "DISMTools Driver Pack|*.dtdrvpak"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ColorModeTSMI, Me.UseLZMATSMI, Me.OpenInExplorerTSMI})
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'ColorModeTSMI
        '
        Me.ColorModeTSMI.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LightToolStripMenuItem, Me.DarkToolStripMenuItem, Me.SystemToolStripMenuItem})
        Me.ColorModeTSMI.Name = "ColorModeTSMI"
        Me.ColorModeTSMI.Size = New System.Drawing.Size(370, 22)
        Me.ColorModeTSMI.Text = "Color Mode"
        '
        'UseLZMATSMI
        '
        Me.UseLZMATSMI.CheckOnClick = True
        Me.UseLZMATSMI.Name = "UseLZMATSMI"
        Me.UseLZMATSMI.Size = New System.Drawing.Size(370, 22)
        Me.UseLZMATSMI.Text = "Use LZMA compression to build Driver Packs"
        '
        'OpenInExplorerTSMI
        '
        Me.OpenInExplorerTSMI.Checked = True
        Me.OpenInExplorerTSMI.CheckOnClick = True
        Me.OpenInExplorerTSMI.CheckState = System.Windows.Forms.CheckState.Checked
        Me.OpenInExplorerTSMI.Name = "OpenInExplorerTSMI"
        Me.OpenInExplorerTSMI.Size = New System.Drawing.Size(370, 22)
        Me.OpenInExplorerTSMI.Text = "After building a Driver Pack, open its location in Explorer"
        '
        'LightToolStripMenuItem
        '
        Me.LightToolStripMenuItem.Name = "LightToolStripMenuItem"
        Me.LightToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.LightToolStripMenuItem.Text = "Light"
        '
        'DarkToolStripMenuItem
        '
        Me.DarkToolStripMenuItem.Name = "DarkToolStripMenuItem"
        Me.DarkToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.DarkToolStripMenuItem.Text = "Dark"
        '
        'SystemToolStripMenuItem
        '
        Me.SystemToolStripMenuItem.Name = "SystemToolStripMenuItem"
        Me.SystemToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.SystemToolStripMenuItem.Text = "System"
        '
        'HelpTopicsToolStripMenuItem
        '
        Me.HelpTopicsToolStripMenuItem.Name = "HelpTopicsToolStripMenuItem"
        Me.HelpTopicsToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.HelpTopicsToolStripMenuItem.Text = "Help Topics"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(149, 6)
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.AboutToolStripMenuItem.Text = "About..."
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1272, 693)
        Me.Controls.Add(Me.pnlHomeScreen)
        Me.Controls.Add(Me.pnlProjectScreen)
        Me.Controls.Add(Me.msActions)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.msActions
        Me.MinimumSize = New System.Drawing.Size(1024, 600)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Driver Pack Creator"
        Me.msActions.ResumeLayout(False)
        Me.msActions.PerformLayout()
        Me.pnlHomeScreen.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.pnlProjectScreen.ResumeLayout(False)
        Me.pnlProjectScreen.PerformLayout()
        Me.tcProjectItems.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.scDrivers.Panel1.ResumeLayout(False)
        Me.scDrivers.Panel2.ResumeLayout(False)
        Me.scDrivers.ResumeLayout(False)
        Me.pnlDriverDetailContainer.ResumeLayout(False)
        Me.pnlNoSelectedDriver.ResumeLayout(False)
        Me.pnlNoSelectedDriver.PerformLayout()
        Me.pnlSelectedDriver.ResumeLayout(False)
        Me.pnlSelectedDriver.PerformLayout()
        Me.pnlDriverActions.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.scComputers.Panel1.ResumeLayout(False)
        Me.scComputers.Panel2.ResumeLayout(False)
        Me.scComputers.ResumeLayout(False)
        Me.pnlComputerDetailContainer.ResumeLayout(False)
        Me.pnlNoSelectedComputer.ResumeLayout(False)
        Me.pnlNoSelectedComputer.PerformLayout()
        Me.pnlSelectedComputer.ResumeLayout(False)
        Me.pnlSelectedComputer.PerformLayout()
        Me.pnlComputerActions.ResumeLayout(False)
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        Me.pnlDriverPackDetails.ResumeLayout(False)
        Me.pnlDriverPackDetails.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents msActions As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ComputersToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DriversToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlHomeScreen As System.Windows.Forms.Panel
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents pnlProjectScreen As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lnkOpenProject As System.Windows.Forms.LinkLabel
    Friend WithEvents lnkNewProject As System.Windows.Forms.LinkLabel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lvRecentProjects As System.Windows.Forms.ListView
    Friend WithEvents chProjectName As System.Windows.Forms.ColumnHeader
    Friend WithEvents lnkImportProject As System.Windows.Forms.LinkLabel
    Friend WithEvents tcProjectItems As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents scDrivers As System.Windows.Forms.SplitContainer
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents lvDriverFiles As System.Windows.Forms.ListView
    Friend WithEvents chMainDriverFile As System.Windows.Forms.ColumnHeader
    Friend WithEvents chDriverName As System.Windows.Forms.ColumnHeader
    Friend WithEvents pnlDriverDetailContainer As System.Windows.Forms.Panel
    Friend WithEvents pnlNoSelectedDriver As System.Windows.Forms.Panel
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents pnlSelectedDriver As System.Windows.Forms.Panel
    Friend WithEvents lnkAddDrvFiles As System.Windows.Forms.LinkLabel
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents tbDriverPackAuthor As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents tbDriverPackDescription As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tbDriverPackName As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents scComputers As System.Windows.Forms.SplitContainer
    Friend WithEvents lvComputers As System.Windows.Forms.ListView
    Friend WithEvents chComputer As System.Windows.Forms.ColumnHeader
    Friend WithEvents pnlComputerDetailContainer As System.Windows.Forms.Panel
    Friend WithEvents pnlNoSelectedComputer As System.Windows.Forms.Panel
    Friend WithEvents lnkAddComputer As System.Windows.Forms.LinkLabel
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents pnlSelectedComputer As System.Windows.Forms.Panel
    Friend WithEvents AddComputerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReuseComputerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FromKnownComputersToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FromAnotherDriverPackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ManageKnownComputersToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenExistingProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ofdProject As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents BuildDriverPackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Friend WithEvents pnlDriverPackDetails As System.Windows.Forms.Panel
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents tbProjectName As System.Windows.Forms.TextBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tbSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tbUnload As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tbBuild As System.Windows.Forms.ToolStripButton
    Friend WithEvents CreateProjectFromDriverPackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddDriverToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddDriversInBulkToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ReuseDriverFromDriverPackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents tbDriverManufacturer As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents tbDriverName As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents pnlDriverActions As System.Windows.Forms.Panel
    Friend WithEvents lbDriverSubdrivers As System.Windows.Forms.ListBox
    Friend WithEvents btnChangeMainDriverFile As System.Windows.Forms.Button
    Friend WithEvents tbDriverMainFile As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents btnSaveDriver As System.Windows.Forms.Button
    Friend WithEvents btnRemoveDriver As System.Windows.Forms.Button
    Friend WithEvents sfdDrvPackBuild As System.Windows.Forms.SaveFileDialog
    Friend WithEvents tbComputerUuid As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents tbComputerModel As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents tbComputerManufacturer As System.Windows.Forms.TextBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents pnlComputerActions As System.Windows.Forms.Panel
    Friend WithEvents btnSaveComputer As System.Windows.Forms.Button
    Friend WithEvents btnRemoveComputer As System.Windows.Forms.Button
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents btnChangeComputerSoftware As System.Windows.Forms.Button
    Friend WithEvents btnChangeComputerHardware As System.Windows.Forms.Button
    Friend WithEvents lvAssociatedDriverFiles As System.Windows.Forms.ListView
    Friend WithEvents chDriverFriendlyName As System.Windows.Forms.ColumnHeader
    Friend WithEvents chDriverMainFile As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents btnManageSubdrivers As System.Windows.Forms.Button
    Friend WithEvents AddComputerFromManufacturerModelCatalogsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TESTMODELCATALOGSToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddCurrentComputerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ColorModeTSMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LightToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DarkToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SystemToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UseLZMATSMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenInExplorerTSMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpTopicsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
