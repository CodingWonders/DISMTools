<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenExistingProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.SaveProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveProjectasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewProjectFilesInFileExplorerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UnloadProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.SwitchImageIndexesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.ProjectPropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImagePropertiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CommandsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageManagementToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AppendImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ApplyFFU = New System.Windows.Forms.ToolStripMenuItem()
        Me.ApplyImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.CaptureCustomImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.CaptureFFU = New System.Windows.Forms.ToolStripMenuItem()
        Me.CaptureImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.CleanupMountpoints = New System.Windows.Forms.ToolStripMenuItem()
        Me.CommitImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetImageInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetMountedImageInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetWIMBootEntry = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.MountImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptimizeFFU = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptimizeImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemountImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitFFU = New System.Windows.Forms.ToolStripMenuItem()
        Me.SplitImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.UnmountImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.UpdateWIMBootEntry = New System.Windows.Forms.ToolStripMenuItem()
        Me.ApplySiloedPackage = New System.Windows.Forms.ToolStripMenuItem()
        Me.OSPackagesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetPackages = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetPackageInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddPackage = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemovePackage = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetFeatures = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetFeatureInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.EnableFeature = New System.Windows.Forms.ToolStripMenuItem()
        Me.DisableFeature = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.CleanupImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProvisioningPackagesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddProvisioningPackage = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetProvisioningPackageInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ApplyCustomDataImage = New System.Windows.Forms.ToolStripMenuItem()
        Me.AppPackagesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetProvisionedAppxPackages = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddProvisionedAppxPackage = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveProvisionedAppxPackage = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptimizeProvisionedAppxPackages = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetProvisionedAppxDataFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.AppPatchesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckAppPatch = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetAppPatchInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetAppPatches = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetAppInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetApps = New System.Windows.Forms.ToolStripMenuItem()
        Me.DefaultAppAssociationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportDefaultAppAssociations = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetDefaultAppAssociations = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImportDefaultAppAssociations = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveDefaultAppAssociations = New System.Windows.Forms.ToolStripMenuItem()
        Me.LanguagesAndRegionSettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetIntl = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetUILang = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetUILangFallback = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetSysUILang = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetSysLocale = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetUserLocale = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetInputLocale = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.SetAllIntl = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.SetTimeZone = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.SetSKUIntlDefaults = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.SetLayeredDriver = New System.Windows.Forms.ToolStripMenuItem()
        Me.GenLangINI = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetSetupUILang = New System.Windows.Forms.ToolStripMenuItem()
        Me.CapabilitiesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddCapability = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportSource = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetCapabilities = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetCapabilityInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveCapability = New System.Windows.Forms.ToolStripMenuItem()
        Me.WindowsEditionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetCurrentEdition = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetTargetEditions = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetEdition = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetProductKey = New System.Windows.Forms.ToolStripMenuItem()
        Me.DriversToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetDrivers = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetDriverInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddDriver = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveDriver = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportDriver = New System.Windows.Forms.ToolStripMenuItem()
        Me.UnattendedAnswerFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ApplyUnattend = New System.Windows.Forms.ToolStripMenuItem()
        Me.WindowsPEServicingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetPESettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetScratchSpace = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetTargetPath = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetScratchSpace = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetTargetPath = New System.Windows.Forms.ToolStripMenuItem()
        Me.OSUninstallToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetOSUninstallWindow = New System.Windows.Forms.ToolStripMenuItem()
        Me.InitiateOSUninstall = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoveOSUninstall = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetOSUninstallWindow = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReservedStorageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetReservedStorageState = New System.Windows.Forms.ToolStripMenuItem()
        Me.GetReservedStorageState = New System.Windows.Forms.ToolStripMenuItem()
        Me.MicrosoftEdgeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddEdge = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddEdgeBrowser = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddEdgeWebView = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageConversionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WIMESDToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.MergeSWM = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator18 = New System.Windows.Forms.ToolStripSeparator()
        Me.RemountImageWithWritePermissionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.CommandShellToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator16 = New System.Windows.Forms.ToolStripSeparator()
        Me.UnattendedAnswerFileManagerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReportManagerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpTopicsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GlossaryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CommandHelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.AboutDISMToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BranchTSMI = New System.Windows.Forms.ToolStripMenuItem()
        Me.VersionTSMI = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReportFeedbackToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InvalidSettingsTSMI = New System.Windows.Forms.ToolStripMenuItem()
        Me.ISFix = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator19 = New System.Windows.Forms.ToolStripSeparator()
        Me.ISHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.HomePanel = New System.Windows.Forms.Panel()
        Me.WelcomePanel = New System.Windows.Forms.Panel()
        Me.WelcomeTabControl = New System.Windows.Forms.TabControl()
        Me.WelcomeTab = New System.Windows.Forms.TabPage()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.NewsFeedTab = New System.Windows.Forms.TabPage()
        Me.VideosTab = New System.Windows.Forms.TabPage()
        Me.SidePanel = New System.Windows.Forms.Panel()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.ExistingProjLink = New System.Windows.Forms.LinkLabel()
        Me.NewProjLink = New System.Windows.Forms.LinkLabel()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.LabelHeader1 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.PictureBox5 = New System.Windows.Forms.PictureBox()
        Me.PrjPanel = New System.Windows.Forms.Panel()
        Me.SplitPanels = New System.Windows.Forms.SplitContainer()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.UnloadBtn = New System.Windows.Forms.Button()
        Me.ProjNameEditBtn = New System.Windows.Forms.Button()
        Me.ExplorerView = New System.Windows.Forms.Button()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Button14 = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.projName = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabTitleSummary1 = New System.Windows.Forms.Panel()
        Me.TabPageIcon1 = New System.Windows.Forms.PictureBox()
        Me.TabPageDescription1 = New System.Windows.Forms.Label()
        Me.TabPageTitle1 = New System.Windows.Forms.Label()
        Me.projNameText = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.ImagePanel = New System.Windows.Forms.Panel()
        Me.TabTitleSummary2 = New System.Windows.Forms.Panel()
        Me.TabPageIcon2 = New System.Windows.Forms.PictureBox()
        Me.TabPageDescription2 = New System.Windows.Forms.Label()
        Me.TabPageTitle2 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Button16 = New System.Windows.Forms.Button()
        Me.Button15 = New System.Windows.Forms.Button()
        Me.ImageNotMountedPanel = New System.Windows.Forms.Panel()
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.TabControl2 = New System.Windows.Forms.TabControl()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button13 = New System.Windows.Forms.Button()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripButton4 = New System.Windows.Forms.ToolStripButton()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.prjTreeView = New System.Windows.Forms.TreeView()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.RefreshViewTSB = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExpandCollapseTSB = New System.Windows.Forms.ToolStripButton()
        Me.prjTreeStatus = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.MenuDesc = New System.Windows.Forms.ToolStripStatusLabel()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.PkgInfoCMS = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.PkgBasicInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.PkgDetailedInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.FeatureInfoCMS = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.FeatureBasicInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.FeatureDetailedInfo = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImgBW = New System.ComponentModel.BackgroundWorker()
        Me.ImgProcesses = New System.Diagnostics.Process()
        Me.MenuStrip1.SuspendLayout()
        Me.HomePanel.SuspendLayout()
        Me.WelcomePanel.SuspendLayout()
        Me.WelcomeTabControl.SuspendLayout()
        Me.WelcomeTab.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SidePanel.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PrjPanel.SuspendLayout()
        CType(Me.SplitPanels, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitPanels.Panel1.SuspendLayout()
        Me.SplitPanels.Panel2.SuspendLayout()
        Me.SplitPanels.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabTitleSummary1.SuspendLayout()
        CType(Me.TabPageIcon1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.ImagePanel.SuspendLayout()
        Me.TabTitleSummary2.SuspendLayout()
        CType(Me.TabPageIcon2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ImageNotMountedPanel.SuspendLayout()
        Me.TabControl2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.prjTreeStatus.SuspendLayout()
        Me.StatusStrip.SuspendLayout()
        Me.PkgInfoCMS.SuspendLayout()
        Me.FeatureInfoCMS.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ProjectToolStripMenuItem, Me.CommandsToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.HelpToolStripMenuItem, Me.BranchTSMI, Me.VersionTSMI, Me.InvalidSettingsTSMI})
        Me.MenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1008, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewProjectToolStripMenuItem, Me.OpenExistingProjectToolStripMenuItem, Me.ToolStripSeparator1, Me.SaveProjectToolStripMenuItem, Me.SaveProjectasToolStripMenuItem, Me.ToolStripSeparator2, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'NewProjectToolStripMenuItem
        '
        Me.NewProjectToolStripMenuItem.Name = "NewProjectToolStripMenuItem"
        Me.NewProjectToolStripMenuItem.Size = New System.Drawing.Size(187, 22)
        Me.NewProjectToolStripMenuItem.Text = "&New project..."
        '
        'OpenExistingProjectToolStripMenuItem
        '
        Me.OpenExistingProjectToolStripMenuItem.Name = "OpenExistingProjectToolStripMenuItem"
        Me.OpenExistingProjectToolStripMenuItem.Size = New System.Drawing.Size(187, 22)
        Me.OpenExistingProjectToolStripMenuItem.Text = "&Open existing project"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(184, 6)
        '
        'SaveProjectToolStripMenuItem
        '
        Me.SaveProjectToolStripMenuItem.Enabled = False
        Me.SaveProjectToolStripMenuItem.Name = "SaveProjectToolStripMenuItem"
        Me.SaveProjectToolStripMenuItem.Size = New System.Drawing.Size(187, 22)
        Me.SaveProjectToolStripMenuItem.Text = "&Save project..."
        '
        'SaveProjectasToolStripMenuItem
        '
        Me.SaveProjectasToolStripMenuItem.Enabled = False
        Me.SaveProjectasToolStripMenuItem.Name = "SaveProjectasToolStripMenuItem"
        Me.SaveProjectasToolStripMenuItem.Size = New System.Drawing.Size(187, 22)
        Me.SaveProjectasToolStripMenuItem.Text = "Save project &as..."
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(184, 6)
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(187, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'ProjectToolStripMenuItem
        '
        Me.ProjectToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewProjectFilesInFileExplorerToolStripMenuItem, Me.UnloadProjectToolStripMenuItem, Me.ToolStripSeparator3, Me.SwitchImageIndexesToolStripMenuItem, Me.ToolStripSeparator11, Me.ProjectPropertiesToolStripMenuItem, Me.ImagePropertiesToolStripMenuItem})
        Me.ProjectToolStripMenuItem.Name = "ProjectToolStripMenuItem"
        Me.ProjectToolStripMenuItem.Size = New System.Drawing.Size(56, 20)
        Me.ProjectToolStripMenuItem.Text = "&Project"
        Me.ProjectToolStripMenuItem.Visible = False
        '
        'ViewProjectFilesInFileExplorerToolStripMenuItem
        '
        Me.ViewProjectFilesInFileExplorerToolStripMenuItem.Name = "ViewProjectFilesInFileExplorerToolStripMenuItem"
        Me.ViewProjectFilesInFileExplorerToolStripMenuItem.Size = New System.Drawing.Size(243, 22)
        Me.ViewProjectFilesInFileExplorerToolStripMenuItem.Text = "View project files in File Explorer"
        '
        'UnloadProjectToolStripMenuItem
        '
        Me.UnloadProjectToolStripMenuItem.Name = "UnloadProjectToolStripMenuItem"
        Me.UnloadProjectToolStripMenuItem.Size = New System.Drawing.Size(243, 22)
        Me.UnloadProjectToolStripMenuItem.Text = "Unload project..."
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(240, 6)
        '
        'SwitchImageIndexesToolStripMenuItem
        '
        Me.SwitchImageIndexesToolStripMenuItem.Name = "SwitchImageIndexesToolStripMenuItem"
        Me.SwitchImageIndexesToolStripMenuItem.Size = New System.Drawing.Size(243, 22)
        Me.SwitchImageIndexesToolStripMenuItem.Text = "Switch image indexes..."
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(240, 6)
        '
        'ProjectPropertiesToolStripMenuItem
        '
        Me.ProjectPropertiesToolStripMenuItem.Name = "ProjectPropertiesToolStripMenuItem"
        Me.ProjectPropertiesToolStripMenuItem.Size = New System.Drawing.Size(243, 22)
        Me.ProjectPropertiesToolStripMenuItem.Text = "Project properties"
        '
        'ImagePropertiesToolStripMenuItem
        '
        Me.ImagePropertiesToolStripMenuItem.Name = "ImagePropertiesToolStripMenuItem"
        Me.ImagePropertiesToolStripMenuItem.Size = New System.Drawing.Size(243, 22)
        Me.ImagePropertiesToolStripMenuItem.Text = "Image properties"
        '
        'CommandsToolStripMenuItem
        '
        Me.CommandsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ImageManagementToolStripMenuItem, Me.OSPackagesToolStripMenuItem, Me.ProvisioningPackagesToolStripMenuItem, Me.AppPackagesToolStripMenuItem, Me.AppPatchesToolStripMenuItem, Me.DefaultAppAssociationsToolStripMenuItem, Me.LanguagesAndRegionSettingsToolStripMenuItem, Me.CapabilitiesToolStripMenuItem, Me.WindowsEditionsToolStripMenuItem, Me.DriversToolStripMenuItem, Me.UnattendedAnswerFilesToolStripMenuItem, Me.WindowsPEServicingToolStripMenuItem, Me.OSUninstallToolStripMenuItem, Me.ReservedStorageToolStripMenuItem, Me.MicrosoftEdgeToolStripMenuItem})
        Me.CommandsToolStripMenuItem.Name = "CommandsToolStripMenuItem"
        Me.CommandsToolStripMenuItem.Size = New System.Drawing.Size(81, 20)
        Me.CommandsToolStripMenuItem.Text = "Com&mands"
        Me.CommandsToolStripMenuItem.Visible = False
        '
        'ImageManagementToolStripMenuItem
        '
        Me.ImageManagementToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AppendImage, Me.ApplyFFU, Me.ApplyImage, Me.CaptureCustomImage, Me.CaptureFFU, Me.CaptureImage, Me.CleanupMountpoints, Me.CommitImage, Me.DeleteImage, Me.ExportImage, Me.GetImageInfo, Me.GetMountedImageInfo, Me.GetWIMBootEntry, Me.ListImage, Me.MountImage, Me.OptimizeFFU, Me.OptimizeImage, Me.RemountImage, Me.SplitFFU, Me.SplitImage, Me.UnmountImage, Me.UpdateWIMBootEntry, Me.ApplySiloedPackage})
        Me.ImageManagementToolStripMenuItem.Name = "ImageManagementToolStripMenuItem"
        Me.ImageManagementToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.ImageManagementToolStripMenuItem.Text = "Image management"
        '
        'AppendImage
        '
        Me.AppendImage.Name = "AppendImage"
        Me.AppendImage.Size = New System.Drawing.Size(305, 22)
        Me.AppendImage.Text = "Append capture directory to image..."
        '
        'ApplyFFU
        '
        Me.ApplyFFU.Name = "ApplyFFU"
        Me.ApplyFFU.Size = New System.Drawing.Size(305, 22)
        Me.ApplyFFU.Text = "Apply FFU or SFU file..."
        '
        'ApplyImage
        '
        Me.ApplyImage.Name = "ApplyImage"
        Me.ApplyImage.Size = New System.Drawing.Size(305, 22)
        Me.ApplyImage.Text = "Apply WIM or SWM file..."
        '
        'CaptureCustomImage
        '
        Me.CaptureCustomImage.Name = "CaptureCustomImage"
        Me.CaptureCustomImage.Size = New System.Drawing.Size(305, 22)
        Me.CaptureCustomImage.Text = "Capture incremental changes to file..."
        '
        'CaptureFFU
        '
        Me.CaptureFFU.Name = "CaptureFFU"
        Me.CaptureFFU.Size = New System.Drawing.Size(305, 22)
        Me.CaptureFFU.Text = "Capture partitions to FFU file..."
        '
        'CaptureImage
        '
        Me.CaptureImage.Name = "CaptureImage"
        Me.CaptureImage.Size = New System.Drawing.Size(305, 22)
        Me.CaptureImage.Text = "Capture image of a drive to WIM file..."
        '
        'CleanupMountpoints
        '
        Me.CleanupMountpoints.Name = "CleanupMountpoints"
        Me.CleanupMountpoints.Size = New System.Drawing.Size(305, 22)
        Me.CleanupMountpoints.Text = "Delete resources from corrupted image..."
        '
        'CommitImage
        '
        Me.CommitImage.Name = "CommitImage"
        Me.CommitImage.Size = New System.Drawing.Size(305, 22)
        Me.CommitImage.Text = "Apply changes to image..."
        '
        'DeleteImage
        '
        Me.DeleteImage.Name = "DeleteImage"
        Me.DeleteImage.Size = New System.Drawing.Size(305, 22)
        Me.DeleteImage.Text = "Delete volume image from WIM file..."
        '
        'ExportImage
        '
        Me.ExportImage.Name = "ExportImage"
        Me.ExportImage.Size = New System.Drawing.Size(305, 22)
        Me.ExportImage.Text = "Export image..."
        '
        'GetImageInfo
        '
        Me.GetImageInfo.Name = "GetImageInfo"
        Me.GetImageInfo.Size = New System.Drawing.Size(305, 22)
        Me.GetImageInfo.Text = "Get image information..."
        '
        'GetMountedImageInfo
        '
        Me.GetMountedImageInfo.Name = "GetMountedImageInfo"
        Me.GetMountedImageInfo.Size = New System.Drawing.Size(305, 22)
        Me.GetMountedImageInfo.Text = "Get currently mounted image information..."
        '
        'GetWIMBootEntry
        '
        Me.GetWIMBootEntry.Name = "GetWIMBootEntry"
        Me.GetWIMBootEntry.Size = New System.Drawing.Size(305, 22)
        Me.GetWIMBootEntry.Text = "Get WIMBoot configuration entries..."
        '
        'ListImage
        '
        Me.ListImage.Name = "ListImage"
        Me.ListImage.Size = New System.Drawing.Size(305, 22)
        Me.ListImage.Text = "List files and directories in image..."
        '
        'MountImage
        '
        Me.MountImage.Name = "MountImage"
        Me.MountImage.Size = New System.Drawing.Size(305, 22)
        Me.MountImage.Text = "Mount image..."
        '
        'OptimizeFFU
        '
        Me.OptimizeFFU.Name = "OptimizeFFU"
        Me.OptimizeFFU.Size = New System.Drawing.Size(305, 22)
        Me.OptimizeFFU.Text = "Optimize FFU file..."
        '
        'OptimizeImage
        '
        Me.OptimizeImage.Name = "OptimizeImage"
        Me.OptimizeImage.Size = New System.Drawing.Size(305, 22)
        Me.OptimizeImage.Text = "Optimize image..."
        '
        'RemountImage
        '
        Me.RemountImage.Name = "RemountImage"
        Me.RemountImage.Size = New System.Drawing.Size(305, 22)
        Me.RemountImage.Text = "Remount image for servicing..."
        '
        'SplitFFU
        '
        Me.SplitFFU.Name = "SplitFFU"
        Me.SplitFFU.Size = New System.Drawing.Size(305, 22)
        Me.SplitFFU.Text = "Splt FFU file into SFU files..."
        '
        'SplitImage
        '
        Me.SplitImage.Name = "SplitImage"
        Me.SplitImage.Size = New System.Drawing.Size(305, 22)
        Me.SplitImage.Text = "Split WIM file into SWM files..."
        '
        'UnmountImage
        '
        Me.UnmountImage.Name = "UnmountImage"
        Me.UnmountImage.Size = New System.Drawing.Size(305, 22)
        Me.UnmountImage.Text = "Unmount image..."
        '
        'UpdateWIMBootEntry
        '
        Me.UpdateWIMBootEntry.Name = "UpdateWIMBootEntry"
        Me.UpdateWIMBootEntry.Size = New System.Drawing.Size(305, 22)
        Me.UpdateWIMBootEntry.Text = "Update WIMBoot configuration entry..."
        '
        'ApplySiloedPackage
        '
        Me.ApplySiloedPackage.Name = "ApplySiloedPackage"
        Me.ApplySiloedPackage.Size = New System.Drawing.Size(305, 22)
        Me.ApplySiloedPackage.Text = "Apply siloed provisioning package..."
        '
        'OSPackagesToolStripMenuItem
        '
        Me.OSPackagesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GetPackages, Me.GetPackageInfo, Me.AddPackage, Me.RemovePackage, Me.GetFeatures, Me.GetFeatureInfo, Me.EnableFeature, Me.DisableFeature, Me.ToolStripSeparator4, Me.CleanupImage})
        Me.OSPackagesToolStripMenuItem.Name = "OSPackagesToolStripMenuItem"
        Me.OSPackagesToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.OSPackagesToolStripMenuItem.Text = "OS packages"
        '
        'GetPackages
        '
        Me.GetPackages.Name = "GetPackages"
        Me.GetPackages.Size = New System.Drawing.Size(292, 22)
        Me.GetPackages.Text = "Get basic package information..."
        '
        'GetPackageInfo
        '
        Me.GetPackageInfo.Name = "GetPackageInfo"
        Me.GetPackageInfo.Size = New System.Drawing.Size(292, 22)
        Me.GetPackageInfo.Text = "Get detailed package information..."
        '
        'AddPackage
        '
        Me.AddPackage.Name = "AddPackage"
        Me.AddPackage.Size = New System.Drawing.Size(292, 22)
        Me.AddPackage.Text = "Add package..."
        '
        'RemovePackage
        '
        Me.RemovePackage.Name = "RemovePackage"
        Me.RemovePackage.Size = New System.Drawing.Size(292, 22)
        Me.RemovePackage.Text = "Remove package..."
        '
        'GetFeatures
        '
        Me.GetFeatures.Name = "GetFeatures"
        Me.GetFeatures.Size = New System.Drawing.Size(292, 22)
        Me.GetFeatures.Text = "Get basic feature information..."
        '
        'GetFeatureInfo
        '
        Me.GetFeatureInfo.Name = "GetFeatureInfo"
        Me.GetFeatureInfo.Size = New System.Drawing.Size(292, 22)
        Me.GetFeatureInfo.Text = "Get detailed feature information..."
        '
        'EnableFeature
        '
        Me.EnableFeature.Name = "EnableFeature"
        Me.EnableFeature.Size = New System.Drawing.Size(292, 22)
        Me.EnableFeature.Text = "Enable feature..."
        '
        'DisableFeature
        '
        Me.DisableFeature.Name = "DisableFeature"
        Me.DisableFeature.Size = New System.Drawing.Size(292, 22)
        Me.DisableFeature.Text = "Disable feature..."
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(289, 6)
        '
        'CleanupImage
        '
        Me.CleanupImage.Name = "CleanupImage"
        Me.CleanupImage.Size = New System.Drawing.Size(292, 22)
        Me.CleanupImage.Text = "Perform cleanup or recovery operations..."
        '
        'ProvisioningPackagesToolStripMenuItem
        '
        Me.ProvisioningPackagesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddProvisioningPackage, Me.GetProvisioningPackageInfo, Me.ApplyCustomDataImage})
        Me.ProvisioningPackagesToolStripMenuItem.Name = "ProvisioningPackagesToolStripMenuItem"
        Me.ProvisioningPackagesToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.ProvisioningPackagesToolStripMenuItem.Text = "Provisioning packages"
        '
        'AddProvisioningPackage
        '
        Me.AddProvisioningPackage.Name = "AddProvisioningPackage"
        Me.AddProvisioningPackage.Size = New System.Drawing.Size(283, 22)
        Me.AddProvisioningPackage.Text = "Add provisioning package..."
        '
        'GetProvisioningPackageInfo
        '
        Me.GetProvisioningPackageInfo.Name = "GetProvisioningPackageInfo"
        Me.GetProvisioningPackageInfo.Size = New System.Drawing.Size(283, 22)
        Me.GetProvisioningPackageInfo.Text = "Get provisioning package information..."
        '
        'ApplyCustomDataImage
        '
        Me.ApplyCustomDataImage.Name = "ApplyCustomDataImage"
        Me.ApplyCustomDataImage.Size = New System.Drawing.Size(283, 22)
        Me.ApplyCustomDataImage.Text = "Apply custom data image..."
        '
        'AppPackagesToolStripMenuItem
        '
        Me.AppPackagesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GetProvisionedAppxPackages, Me.AddProvisionedAppxPackage, Me.RemoveProvisionedAppxPackage, Me.OptimizeProvisionedAppxPackages, Me.SetProvisionedAppxDataFile})
        Me.AppPackagesToolStripMenuItem.Name = "AppPackagesToolStripMenuItem"
        Me.AppPackagesToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.AppPackagesToolStripMenuItem.Text = "App packages"
        '
        'GetProvisionedAppxPackages
        '
        Me.GetProvisionedAppxPackages.Name = "GetProvisionedAppxPackages"
        Me.GetProvisionedAppxPackages.Size = New System.Drawing.Size(287, 22)
        Me.GetProvisionedAppxPackages.Text = "Get app package information..."
        '
        'AddProvisionedAppxPackage
        '
        Me.AddProvisionedAppxPackage.Name = "AddProvisionedAppxPackage"
        Me.AddProvisionedAppxPackage.Size = New System.Drawing.Size(287, 22)
        Me.AddProvisionedAppxPackage.Text = "Add provisioned app package..."
        '
        'RemoveProvisionedAppxPackage
        '
        Me.RemoveProvisionedAppxPackage.Name = "RemoveProvisionedAppxPackage"
        Me.RemoveProvisionedAppxPackage.Size = New System.Drawing.Size(287, 22)
        Me.RemoveProvisionedAppxPackage.Text = "Remove provisioning for app package..."
        '
        'OptimizeProvisionedAppxPackages
        '
        Me.OptimizeProvisionedAppxPackages.Name = "OptimizeProvisionedAppxPackages"
        Me.OptimizeProvisionedAppxPackages.Size = New System.Drawing.Size(287, 22)
        Me.OptimizeProvisionedAppxPackages.Text = "Optimize provisioned packages..."
        '
        'SetProvisionedAppxDataFile
        '
        Me.SetProvisionedAppxDataFile.Name = "SetProvisionedAppxDataFile"
        Me.SetProvisionedAppxDataFile.Size = New System.Drawing.Size(287, 22)
        Me.SetProvisionedAppxDataFile.Text = "Add custom data file into app package..."
        '
        'AppPatchesToolStripMenuItem
        '
        Me.AppPatchesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CheckAppPatch, Me.GetAppPatchInfo, Me.GetAppPatches, Me.GetAppInfo, Me.GetApps})
        Me.AppPatchesToolStripMenuItem.Name = "AppPatchesToolStripMenuItem"
        Me.AppPatchesToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.AppPatchesToolStripMenuItem.Text = "App (MSP) servicing"
        '
        'CheckAppPatch
        '
        Me.CheckAppPatch.Name = "CheckAppPatch"
        Me.CheckAppPatch.Size = New System.Drawing.Size(408, 22)
        Me.CheckAppPatch.Text = "Get application patch information..."
        '
        'GetAppPatchInfo
        '
        Me.GetAppPatchInfo.Name = "GetAppPatchInfo"
        Me.GetAppPatchInfo.Size = New System.Drawing.Size(408, 22)
        Me.GetAppPatchInfo.Text = "Get detailed installed application patch information..."
        '
        'GetAppPatches
        '
        Me.GetAppPatches.Name = "GetAppPatches"
        Me.GetAppPatches.Size = New System.Drawing.Size(408, 22)
        Me.GetAppPatches.Text = "Get basic installed application patch information..."
        '
        'GetAppInfo
        '
        Me.GetAppInfo.Name = "GetAppInfo"
        Me.GetAppInfo.Size = New System.Drawing.Size(408, 22)
        Me.GetAppInfo.Text = "Get detailed Windows Installer (*.msi) application information..."
        '
        'GetApps
        '
        Me.GetApps.Name = "GetApps"
        Me.GetApps.Size = New System.Drawing.Size(408, 22)
        Me.GetApps.Text = "Get basic Windows Installer (*.msi) application information..."
        '
        'DefaultAppAssociationsToolStripMenuItem
        '
        Me.DefaultAppAssociationsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExportDefaultAppAssociations, Me.GetDefaultAppAssociations, Me.ImportDefaultAppAssociations, Me.RemoveDefaultAppAssociations})
        Me.DefaultAppAssociationsToolStripMenuItem.Name = "DefaultAppAssociationsToolStripMenuItem"
        Me.DefaultAppAssociationsToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.DefaultAppAssociationsToolStripMenuItem.Text = "Default app associations"
        '
        'ExportDefaultAppAssociations
        '
        Me.ExportDefaultAppAssociations.Name = "ExportDefaultAppAssociations"
        Me.ExportDefaultAppAssociations.Size = New System.Drawing.Size(331, 22)
        Me.ExportDefaultAppAssociations.Text = "Export default application associations..."
        '
        'GetDefaultAppAssociations
        '
        Me.GetDefaultAppAssociations.Name = "GetDefaultAppAssociations"
        Me.GetDefaultAppAssociations.Size = New System.Drawing.Size(331, 22)
        Me.GetDefaultAppAssociations.Text = "Get default application association information..."
        '
        'ImportDefaultAppAssociations
        '
        Me.ImportDefaultAppAssociations.Name = "ImportDefaultAppAssociations"
        Me.ImportDefaultAppAssociations.Size = New System.Drawing.Size(331, 22)
        Me.ImportDefaultAppAssociations.Text = "Import default application associations..."
        '
        'RemoveDefaultAppAssociations
        '
        Me.RemoveDefaultAppAssociations.Name = "RemoveDefaultAppAssociations"
        Me.RemoveDefaultAppAssociations.Size = New System.Drawing.Size(331, 22)
        Me.RemoveDefaultAppAssociations.Text = "Remove default application associations..."
        '
        'LanguagesAndRegionSettingsToolStripMenuItem
        '
        Me.LanguagesAndRegionSettingsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GetIntl, Me.SetUILang, Me.SetUILangFallback, Me.SetSysUILang, Me.SetSysLocale, Me.SetUserLocale, Me.SetInputLocale, Me.ToolStripSeparator5, Me.SetAllIntl, Me.ToolStripSeparator6, Me.SetTimeZone, Me.ToolStripSeparator7, Me.SetSKUIntlDefaults, Me.ToolStripSeparator8, Me.SetLayeredDriver, Me.GenLangINI, Me.SetSetupUILang})
        Me.LanguagesAndRegionSettingsToolStripMenuItem.Name = "LanguagesAndRegionSettingsToolStripMenuItem"
        Me.LanguagesAndRegionSettingsToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.LanguagesAndRegionSettingsToolStripMenuItem.Text = "Languages and region settings"
        '
        'GetIntl
        '
        Me.GetIntl.Name = "GetIntl"
        Me.GetIntl.Size = New System.Drawing.Size(295, 22)
        Me.GetIntl.Text = "Get international settings and languages..."
        '
        'SetUILang
        '
        Me.SetUILang.Name = "SetUILang"
        Me.SetUILang.Size = New System.Drawing.Size(295, 22)
        Me.SetUILang.Text = "Set UI language..."
        '
        'SetUILangFallback
        '
        Me.SetUILangFallback.Name = "SetUILangFallback"
        Me.SetUILangFallback.Size = New System.Drawing.Size(295, 22)
        Me.SetUILangFallback.Text = "Set default UI fallback language..."
        '
        'SetSysUILang
        '
        Me.SetSysUILang.Name = "SetSysUILang"
        Me.SetSysUILang.Size = New System.Drawing.Size(295, 22)
        Me.SetSysUILang.Text = "Set system preferred UI language..."
        '
        'SetSysLocale
        '
        Me.SetSysLocale.Name = "SetSysLocale"
        Me.SetSysLocale.Size = New System.Drawing.Size(295, 22)
        Me.SetSysLocale.Text = "Set system locale..."
        '
        'SetUserLocale
        '
        Me.SetUserLocale.Name = "SetUserLocale"
        Me.SetUserLocale.Size = New System.Drawing.Size(295, 22)
        Me.SetUserLocale.Text = "Set user locale..."
        '
        'SetInputLocale
        '
        Me.SetInputLocale.Name = "SetInputLocale"
        Me.SetInputLocale.Size = New System.Drawing.Size(295, 22)
        Me.SetInputLocale.Text = "Set input locale..."
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(292, 6)
        '
        'SetAllIntl
        '
        Me.SetAllIntl.Name = "SetAllIntl"
        Me.SetAllIntl.Size = New System.Drawing.Size(295, 22)
        Me.SetAllIntl.Text = "Set UI language and locales..."
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(292, 6)
        '
        'SetTimeZone
        '
        Me.SetTimeZone.Name = "SetTimeZone"
        Me.SetTimeZone.Size = New System.Drawing.Size(295, 22)
        Me.SetTimeZone.Text = "Set default time zone..."
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(292, 6)
        '
        'SetSKUIntlDefaults
        '
        Me.SetSKUIntlDefaults.Name = "SetSKUIntlDefaults"
        Me.SetSKUIntlDefaults.Size = New System.Drawing.Size(295, 22)
        Me.SetSKUIntlDefaults.Text = "Set default languages and locales..."
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(292, 6)
        '
        'SetLayeredDriver
        '
        Me.SetLayeredDriver.Name = "SetLayeredDriver"
        Me.SetLayeredDriver.Size = New System.Drawing.Size(295, 22)
        Me.SetLayeredDriver.Text = "Set layered driver..."
        '
        'GenLangINI
        '
        Me.GenLangINI.Name = "GenLangINI"
        Me.GenLangINI.Size = New System.Drawing.Size(295, 22)
        Me.GenLangINI.Text = "Generate Lang.ini file..."
        '
        'SetSetupUILang
        '
        Me.SetSetupUILang.Name = "SetSetupUILang"
        Me.SetSetupUILang.Size = New System.Drawing.Size(295, 22)
        Me.SetSetupUILang.Text = "Set default Setup language..."
        '
        'CapabilitiesToolStripMenuItem
        '
        Me.CapabilitiesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddCapability, Me.ExportSource, Me.GetCapabilities, Me.GetCapabilityInfo, Me.RemoveCapability})
        Me.CapabilitiesToolStripMenuItem.Name = "CapabilitiesToolStripMenuItem"
        Me.CapabilitiesToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.CapabilitiesToolStripMenuItem.Text = "Capabilities"
        '
        'AddCapability
        '
        Me.AddCapability.Name = "AddCapability"
        Me.AddCapability.Size = New System.Drawing.Size(266, 22)
        Me.AddCapability.Text = "Add capability..."
        '
        'ExportSource
        '
        Me.ExportSource.Name = "ExportSource"
        Me.ExportSource.Size = New System.Drawing.Size(266, 22)
        Me.ExportSource.Text = "Export capabilities into repository..."
        '
        'GetCapabilities
        '
        Me.GetCapabilities.Name = "GetCapabilities"
        Me.GetCapabilities.Size = New System.Drawing.Size(266, 22)
        Me.GetCapabilities.Text = "Get basic capability information..."
        '
        'GetCapabilityInfo
        '
        Me.GetCapabilityInfo.Name = "GetCapabilityInfo"
        Me.GetCapabilityInfo.Size = New System.Drawing.Size(266, 22)
        Me.GetCapabilityInfo.Text = "Get detailed capability information..."
        '
        'RemoveCapability
        '
        Me.RemoveCapability.Name = "RemoveCapability"
        Me.RemoveCapability.Size = New System.Drawing.Size(266, 22)
        Me.RemoveCapability.Text = "Remove capability..."
        '
        'WindowsEditionsToolStripMenuItem
        '
        Me.WindowsEditionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GetCurrentEdition, Me.GetTargetEditions, Me.SetEdition, Me.SetProductKey})
        Me.WindowsEditionsToolStripMenuItem.Name = "WindowsEditionsToolStripMenuItem"
        Me.WindowsEditionsToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.WindowsEditionsToolStripMenuItem.Text = "Windows editions"
        '
        'GetCurrentEdition
        '
        Me.GetCurrentEdition.Name = "GetCurrentEdition"
        Me.GetCurrentEdition.Size = New System.Drawing.Size(187, 22)
        Me.GetCurrentEdition.Text = "Get current edition..."
        '
        'GetTargetEditions
        '
        Me.GetTargetEditions.Name = "GetTargetEditions"
        Me.GetTargetEditions.Size = New System.Drawing.Size(187, 22)
        Me.GetTargetEditions.Text = "Get upgrade targets..."
        '
        'SetEdition
        '
        Me.SetEdition.Name = "SetEdition"
        Me.SetEdition.Size = New System.Drawing.Size(187, 22)
        Me.SetEdition.Text = "Upgrade image..."
        '
        'SetProductKey
        '
        Me.SetProductKey.Name = "SetProductKey"
        Me.SetProductKey.Size = New System.Drawing.Size(187, 22)
        Me.SetProductKey.Text = "Set product key..."
        '
        'DriversToolStripMenuItem
        '
        Me.DriversToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GetDrivers, Me.GetDriverInfo, Me.AddDriver, Me.RemoveDriver, Me.ExportDriver})
        Me.DriversToolStripMenuItem.Name = "DriversToolStripMenuItem"
        Me.DriversToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.DriversToolStripMenuItem.Text = "Drivers"
        '
        'GetDrivers
        '
        Me.GetDrivers.Name = "GetDrivers"
        Me.GetDrivers.Size = New System.Drawing.Size(245, 22)
        Me.GetDrivers.Text = "Get basic driver information..."
        '
        'GetDriverInfo
        '
        Me.GetDriverInfo.Name = "GetDriverInfo"
        Me.GetDriverInfo.Size = New System.Drawing.Size(245, 22)
        Me.GetDriverInfo.Text = "Get detailed driver information..."
        '
        'AddDriver
        '
        Me.AddDriver.Name = "AddDriver"
        Me.AddDriver.Size = New System.Drawing.Size(245, 22)
        Me.AddDriver.Text = "Add driver..."
        '
        'RemoveDriver
        '
        Me.RemoveDriver.Name = "RemoveDriver"
        Me.RemoveDriver.Size = New System.Drawing.Size(245, 22)
        Me.RemoveDriver.Text = "Remove driver..."
        '
        'ExportDriver
        '
        Me.ExportDriver.Name = "ExportDriver"
        Me.ExportDriver.Size = New System.Drawing.Size(245, 22)
        Me.ExportDriver.Text = "Export driver packages..."
        '
        'UnattendedAnswerFilesToolStripMenuItem
        '
        Me.UnattendedAnswerFilesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ApplyUnattend})
        Me.UnattendedAnswerFilesToolStripMenuItem.Name = "UnattendedAnswerFilesToolStripMenuItem"
        Me.UnattendedAnswerFilesToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.UnattendedAnswerFilesToolStripMenuItem.Text = "Unattended answer files"
        '
        'ApplyUnattend
        '
        Me.ApplyUnattend.Name = "ApplyUnattend"
        Me.ApplyUnattend.Size = New System.Drawing.Size(237, 22)
        Me.ApplyUnattend.Text = "Apply unattended answer file..."
        '
        'WindowsPEServicingToolStripMenuItem
        '
        Me.WindowsPEServicingToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GetPESettings, Me.GetScratchSpace, Me.GetTargetPath, Me.SetScratchSpace, Me.SetTargetPath})
        Me.WindowsPEServicingToolStripMenuItem.Name = "WindowsPEServicingToolStripMenuItem"
        Me.WindowsPEServicingToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.WindowsPEServicingToolStripMenuItem.Text = "Windows PE servicing"
        '
        'GetPESettings
        '
        Me.GetPESettings.Name = "GetPESettings"
        Me.GetPESettings.Size = New System.Drawing.Size(175, 22)
        Me.GetPESettings.Text = "Get settings..."
        '
        'GetScratchSpace
        '
        Me.GetScratchSpace.Name = "GetScratchSpace"
        Me.GetScratchSpace.Size = New System.Drawing.Size(175, 22)
        Me.GetScratchSpace.Text = "Get scratch space..."
        '
        'GetTargetPath
        '
        Me.GetTargetPath.Name = "GetTargetPath"
        Me.GetTargetPath.Size = New System.Drawing.Size(175, 22)
        Me.GetTargetPath.Text = "Get target path..."
        '
        'SetScratchSpace
        '
        Me.SetScratchSpace.Name = "SetScratchSpace"
        Me.SetScratchSpace.Size = New System.Drawing.Size(175, 22)
        Me.SetScratchSpace.Text = "Set scratch space..."
        '
        'SetTargetPath
        '
        Me.SetTargetPath.Name = "SetTargetPath"
        Me.SetTargetPath.Size = New System.Drawing.Size(175, 22)
        Me.SetTargetPath.Text = "Set target path..."
        '
        'OSUninstallToolStripMenuItem
        '
        Me.OSUninstallToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GetOSUninstallWindow, Me.InitiateOSUninstall, Me.RemoveOSUninstall, Me.SetOSUninstallWindow})
        Me.OSUninstallToolStripMenuItem.Name = "OSUninstallToolStripMenuItem"
        Me.OSUninstallToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.OSUninstallToolStripMenuItem.Text = "OS uninstall"
        '
        'GetOSUninstallWindow
        '
        Me.GetOSUninstallWindow.Name = "GetOSUninstallWindow"
        Me.GetOSUninstallWindow.Size = New System.Drawing.Size(209, 22)
        Me.GetOSUninstallWindow.Text = "Get uninstall window..."
        '
        'InitiateOSUninstall
        '
        Me.InitiateOSUninstall.Name = "InitiateOSUninstall"
        Me.InitiateOSUninstall.Size = New System.Drawing.Size(209, 22)
        Me.InitiateOSUninstall.Text = "Initiate uninstall..."
        '
        'RemoveOSUninstall
        '
        Me.RemoveOSUninstall.Name = "RemoveOSUninstall"
        Me.RemoveOSUninstall.Size = New System.Drawing.Size(209, 22)
        Me.RemoveOSUninstall.Text = "Remove roll back ability..."
        '
        'SetOSUninstallWindow
        '
        Me.SetOSUninstallWindow.Name = "SetOSUninstallWindow"
        Me.SetOSUninstallWindow.Size = New System.Drawing.Size(209, 22)
        Me.SetOSUninstallWindow.Text = "Set uninstall window..."
        '
        'ReservedStorageToolStripMenuItem
        '
        Me.ReservedStorageToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SetReservedStorageState, Me.GetReservedStorageState})
        Me.ReservedStorageToolStripMenuItem.Name = "ReservedStorageToolStripMenuItem"
        Me.ReservedStorageToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.ReservedStorageToolStripMenuItem.Text = "Reserved storage"
        '
        'SetReservedStorageState
        '
        Me.SetReservedStorageState.Name = "SetReservedStorageState"
        Me.SetReservedStorageState.Size = New System.Drawing.Size(218, 22)
        Me.SetReservedStorageState.Text = "Set reserved storage state..."
        '
        'GetReservedStorageState
        '
        Me.GetReservedStorageState.Name = "GetReservedStorageState"
        Me.GetReservedStorageState.Size = New System.Drawing.Size(218, 22)
        Me.GetReservedStorageState.Text = "Get reserved storage state..."
        '
        'MicrosoftEdgeToolStripMenuItem
        '
        Me.MicrosoftEdgeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddEdge, Me.AddEdgeBrowser, Me.AddEdgeWebView})
        Me.MicrosoftEdgeToolStripMenuItem.Name = "MicrosoftEdgeToolStripMenuItem"
        Me.MicrosoftEdgeToolStripMenuItem.Size = New System.Drawing.Size(235, 22)
        Me.MicrosoftEdgeToolStripMenuItem.Text = "Microsoft Edge"
        '
        'AddEdge
        '
        Me.AddEdge.Name = "AddEdge"
        Me.AddEdge.Size = New System.Drawing.Size(177, 22)
        Me.AddEdge.Text = "Add Edge"
        '
        'AddEdgeBrowser
        '
        Me.AddEdgeBrowser.Name = "AddEdgeBrowser"
        Me.AddEdgeBrowser.Size = New System.Drawing.Size(177, 22)
        Me.AddEdgeBrowser.Text = "Add Edge browser"
        '
        'AddEdgeWebView
        '
        Me.AddEdgeWebView.Name = "AddEdgeWebView"
        Me.AddEdgeWebView.Size = New System.Drawing.Size(177, 22)
        Me.AddEdgeWebView.Text = "Add Edge WebView"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ImageConversionToolStripMenuItem, Me.ToolStripSeparator12, Me.MergeSWM, Me.ToolStripSeparator18, Me.RemountImageWithWritePermissionsToolStripMenuItem, Me.ToolStripSeparator13, Me.CommandShellToolStripMenuItem, Me.ToolStripSeparator16, Me.UnattendedAnswerFileManagerToolStripMenuItem, Me.ReportManagerToolStripMenuItem, Me.ToolStripSeparator9, Me.OptionsToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(46, 20)
        Me.ToolsToolStripMenuItem.Text = "&Tools"
        '
        'ImageConversionToolStripMenuItem
        '
        Me.ImageConversionToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.WIMESDToolStripMenuItem})
        Me.ImageConversionToolStripMenuItem.Name = "ImageConversionToolStripMenuItem"
        Me.ImageConversionToolStripMenuItem.Size = New System.Drawing.Size(280, 22)
        Me.ImageConversionToolStripMenuItem.Text = "Image conversion"
        '
        'WIMESDToolStripMenuItem
        '
        Me.WIMESDToolStripMenuItem.Name = "WIMESDToolStripMenuItem"
        Me.WIMESDToolStripMenuItem.Size = New System.Drawing.Size(146, 22)
        Me.WIMESDToolStripMenuItem.Text = "WIM <-> ESD"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(277, 6)
        '
        'MergeSWM
        '
        Me.MergeSWM.Name = "MergeSWM"
        Me.MergeSWM.Size = New System.Drawing.Size(280, 22)
        Me.MergeSWM.Text = "Merge SWM files..."
        '
        'ToolStripSeparator18
        '
        Me.ToolStripSeparator18.Name = "ToolStripSeparator18"
        Me.ToolStripSeparator18.Size = New System.Drawing.Size(277, 6)
        '
        'RemountImageWithWritePermissionsToolStripMenuItem
        '
        Me.RemountImageWithWritePermissionsToolStripMenuItem.Enabled = False
        Me.RemountImageWithWritePermissionsToolStripMenuItem.Name = "RemountImageWithWritePermissionsToolStripMenuItem"
        Me.RemountImageWithWritePermissionsToolStripMenuItem.Size = New System.Drawing.Size(280, 22)
        Me.RemountImageWithWritePermissionsToolStripMenuItem.Text = "Remount image with write permissions"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(277, 6)
        '
        'CommandShellToolStripMenuItem
        '
        Me.CommandShellToolStripMenuItem.Name = "CommandShellToolStripMenuItem"
        Me.CommandShellToolStripMenuItem.Size = New System.Drawing.Size(280, 22)
        Me.CommandShellToolStripMenuItem.Text = "Command Console"
        '
        'ToolStripSeparator16
        '
        Me.ToolStripSeparator16.Name = "ToolStripSeparator16"
        Me.ToolStripSeparator16.Size = New System.Drawing.Size(277, 6)
        '
        'UnattendedAnswerFileManagerToolStripMenuItem
        '
        Me.UnattendedAnswerFileManagerToolStripMenuItem.Name = "UnattendedAnswerFileManagerToolStripMenuItem"
        Me.UnattendedAnswerFileManagerToolStripMenuItem.Size = New System.Drawing.Size(280, 22)
        Me.UnattendedAnswerFileManagerToolStripMenuItem.Text = "Unattended answer file manager"
        '
        'ReportManagerToolStripMenuItem
        '
        Me.ReportManagerToolStripMenuItem.Name = "ReportManagerToolStripMenuItem"
        Me.ReportManagerToolStripMenuItem.Size = New System.Drawing.Size(280, 22)
        Me.ReportManagerToolStripMenuItem.Text = "Report manager"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(277, 6)
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(280, 22)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpTopicsToolStripMenuItem, Me.GlossaryToolStripMenuItem, Me.CommandHelpToolStripMenuItem, Me.ToolStripSeparator10, Me.AboutDISMToolsToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'HelpTopicsToolStripMenuItem
        '
        Me.HelpTopicsToolStripMenuItem.Name = "HelpTopicsToolStripMenuItem"
        Me.HelpTopicsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.HelpTopicsToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.HelpTopicsToolStripMenuItem.Text = "Help Topics"
        '
        'GlossaryToolStripMenuItem
        '
        Me.GlossaryToolStripMenuItem.Name = "GlossaryToolStripMenuItem"
        Me.GlossaryToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.F1), System.Windows.Forms.Keys)
        Me.GlossaryToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.GlossaryToolStripMenuItem.Text = "Glossary"
        '
        'CommandHelpToolStripMenuItem
        '
        Me.CommandHelpToolStripMenuItem.Name = "CommandHelpToolStripMenuItem"
        Me.CommandHelpToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Shift Or System.Windows.Forms.Keys.F1), System.Windows.Forms.Keys)
        Me.CommandHelpToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.CommandHelpToolStripMenuItem.Text = "Command help..."
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(228, 6)
        '
        'AboutDISMToolsToolStripMenuItem
        '
        Me.AboutDISMToolsToolStripMenuItem.Name = "AboutDISMToolsToolStripMenuItem"
        Me.AboutDISMToolsToolStripMenuItem.Size = New System.Drawing.Size(231, 22)
        Me.AboutDISMToolsToolStripMenuItem.Text = "About DISMTools"
        '
        'BranchTSMI
        '
        Me.BranchTSMI.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.BranchTSMI.Image = Global.DISMTools.My.Resources.Resources.branch
        Me.BranchTSMI.Name = "BranchTSMI"
        Me.BranchTSMI.Size = New System.Drawing.Size(72, 20)
        Me.BranchTSMI.Text = "Branch"
        Me.BranchTSMI.Visible = False
        '
        'VersionTSMI
        '
        Me.VersionTSMI.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.VersionTSMI.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReportFeedbackToolStripMenuItem})
        Me.VersionTSMI.Name = "VersionTSMI"
        Me.VersionTSMI.Size = New System.Drawing.Size(57, 20)
        Me.VersionTSMI.Text = "ALPHA"
        Me.VersionTSMI.ToolTipText = "This is an alpha release. In it, you will encounter lots of bugs and incomplete f" & _
    "eatures."
        '
        'ReportFeedbackToolStripMenuItem
        '
        Me.ReportFeedbackToolStripMenuItem.Name = "ReportFeedbackToolStripMenuItem"
        Me.ReportFeedbackToolStripMenuItem.Size = New System.Drawing.Size(286, 22)
        Me.ReportFeedbackToolStripMenuItem.Text = "Report feedback (opens in web browser)"
        '
        'InvalidSettingsTSMI
        '
        Me.InvalidSettingsTSMI.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.InvalidSettingsTSMI.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ISFix, Me.ToolStripSeparator19, Me.ISHelp})
        Me.InvalidSettingsTSMI.Image = Global.DISMTools.My.Resources.Resources.setting_error_glyph
        Me.InvalidSettingsTSMI.Name = "InvalidSettingsTSMI"
        Me.InvalidSettingsTSMI.Size = New System.Drawing.Size(220, 20)
        Me.InvalidSettingsTSMI.Text = "Invalid settings have been detected"
        Me.InvalidSettingsTSMI.Visible = False
        '
        'ISFix
        '
        Me.ISFix.Name = "ISFix"
        Me.ISFix.Size = New System.Drawing.Size(140, 22)
        Me.ISFix.Text = "Fix..."
        '
        'ToolStripSeparator19
        '
        Me.ToolStripSeparator19.Name = "ToolStripSeparator19"
        Me.ToolStripSeparator19.Size = New System.Drawing.Size(137, 6)
        '
        'ISHelp
        '
        Me.ISHelp.Name = "ISHelp"
        Me.ISHelp.Size = New System.Drawing.Size(140, 22)
        Me.ISHelp.Text = "What is this?"
        '
        'HomePanel
        '
        Me.HomePanel.Controls.Add(Me.WelcomePanel)
        Me.HomePanel.Controls.Add(Me.SidePanel)
        Me.HomePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HomePanel.Location = New System.Drawing.Point(0, 24)
        Me.HomePanel.Name = "HomePanel"
        Me.HomePanel.Size = New System.Drawing.Size(1008, 537)
        Me.HomePanel.TabIndex = 3
        '
        'WelcomePanel
        '
        Me.WelcomePanel.Controls.Add(Me.WelcomeTabControl)
        Me.WelcomePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WelcomePanel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WelcomePanel.Location = New System.Drawing.Point(256, 0)
        Me.WelcomePanel.Name = "WelcomePanel"
        Me.WelcomePanel.Size = New System.Drawing.Size(752, 537)
        Me.WelcomePanel.TabIndex = 1
        '
        'WelcomeTabControl
        '
        Me.WelcomeTabControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WelcomeTabControl.Controls.Add(Me.WelcomeTab)
        Me.WelcomeTabControl.Controls.Add(Me.NewsFeedTab)
        Me.WelcomeTabControl.Controls.Add(Me.VideosTab)
        Me.WelcomeTabControl.Location = New System.Drawing.Point(10, 12)
        Me.WelcomeTabControl.Name = "WelcomeTabControl"
        Me.WelcomeTabControl.SelectedIndex = 0
        Me.WelcomeTabControl.Size = New System.Drawing.Size(733, 513)
        Me.WelcomeTabControl.TabIndex = 0
        '
        'WelcomeTab
        '
        Me.WelcomeTab.Controls.Add(Me.PictureBox2)
        Me.WelcomeTab.Controls.Add(Me.PictureBox4)
        Me.WelcomeTab.Controls.Add(Me.PictureBox3)
        Me.WelcomeTab.Controls.Add(Me.PictureBox1)
        Me.WelcomeTab.Controls.Add(Me.Label29)
        Me.WelcomeTab.Controls.Add(Me.Label33)
        Me.WelcomeTab.Controls.Add(Me.Label31)
        Me.WelcomeTab.Controls.Add(Me.Label27)
        Me.WelcomeTab.Controls.Add(Me.Label28)
        Me.WelcomeTab.Controls.Add(Me.Label25)
        Me.WelcomeTab.Controls.Add(Me.Label32)
        Me.WelcomeTab.Controls.Add(Me.Label30)
        Me.WelcomeTab.Controls.Add(Me.Label26)
        Me.WelcomeTab.Controls.Add(Me.Label24)
        Me.WelcomeTab.Location = New System.Drawing.Point(4, 24)
        Me.WelcomeTab.Name = "WelcomeTab"
        Me.WelcomeTab.Padding = New System.Windows.Forms.Padding(3)
        Me.WelcomeTab.Size = New System.Drawing.Size(725, 485)
        Me.WelcomeTab.TabIndex = 0
        Me.WelcomeTab.Text = "Welcome"
        Me.WelcomeTab.UseVisualStyleBackColor = True
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.DISMTools.My.Resources.Resources.icons8_code_32px
        Me.PictureBox2.Location = New System.Drawing.Point(32, 176)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox2.TabIndex = 4
        Me.PictureBox2.TabStop = False
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = Global.DISMTools.My.Resources.Resources.getting_started
        Me.PictureBox4.Location = New System.Drawing.Point(32, 359)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox4.TabIndex = 4
        Me.PictureBox4.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = Global.DISMTools.My.Resources.Resources.caution
        Me.PictureBox3.Location = New System.Drawing.Point(32, 265)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox3.TabIndex = 4
        Me.PictureBox3.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.DISMTools.My.Resources.Resources.ver_stability
        Me.PictureBox1.Location = New System.Drawing.Point(32, 83)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 4
        Me.PictureBox1.TabStop = False
        '
        'Label29
        '
        Me.Label29.Location = New System.Drawing.Point(75, 211)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(631, 48)
        Me.Label29.TabIndex = 3
        Me.Label29.Text = "This program is open-source, meaning you can take a look at how it works and unde" & _
    "rstand it better."
        '
        'Label33
        '
        Me.Label33.Location = New System.Drawing.Point(75, 394)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(631, 75)
        Me.Label33.TabIndex = 3
        Me.Label33.Text = resources.GetString("Label33.Text")
        '
        'Label31
        '
        Me.Label31.Location = New System.Drawing.Point(75, 300)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(631, 52)
        Me.Label31.TabIndex = 3
        Me.Label31.Text = resources.GetString("Label31.Text")
        '
        'Label27
        '
        Me.Label27.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label27.Location = New System.Drawing.Point(75, 118)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(631, 52)
        Me.Label27.TabIndex = 3
        Me.Label27.Text = "Currently, this program is in alpha. This means lots of things will not work as e" & _
    "xpected. There will also be lots of bugs, and, generally, the program is incompl" & _
    "ete (as you can see right now)"
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(70, 176)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(277, 30)
        Me.Label28.TabIndex = 1
        Me.Label28.Text = "This program is open-source"
        '
        'Label25
        '
        Me.Label25.Location = New System.Drawing.Point(16, 48)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(690, 21)
        Me.Label25.TabIndex = 3
        Me.Label25.Text = "The graphical front-end to perform DISM operations."
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.Location = New System.Drawing.Point(70, 359)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(151, 30)
        Me.Label32.TabIndex = 1
        Me.Label32.Text = "Getting started"
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.Location = New System.Drawing.Point(70, 265)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(346, 30)
        Me.Label30.TabIndex = 1
        Me.Label30.Text = "Be sure to know what you are doing"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Bold)
        Me.Label26.Location = New System.Drawing.Point(70, 83)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(225, 30)
        Me.Label26.TabIndex = 1
        Me.Label26.Text = "This is alpha software"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Segoe UI", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(14, 14)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(228, 30)
        Me.Label24.TabIndex = 1
        Me.Label24.Text = "Welcome to DISMTools"
        '
        'NewsFeedTab
        '
        Me.NewsFeedTab.Location = New System.Drawing.Point(4, 24)
        Me.NewsFeedTab.Name = "NewsFeedTab"
        Me.NewsFeedTab.Padding = New System.Windows.Forms.Padding(3)
        Me.NewsFeedTab.Size = New System.Drawing.Size(725, 485)
        Me.NewsFeedTab.TabIndex = 1
        Me.NewsFeedTab.Text = "Latest news"
        Me.NewsFeedTab.UseVisualStyleBackColor = True
        '
        'VideosTab
        '
        Me.VideosTab.Location = New System.Drawing.Point(4, 24)
        Me.VideosTab.Name = "VideosTab"
        Me.VideosTab.Padding = New System.Windows.Forms.Padding(3)
        Me.VideosTab.Size = New System.Drawing.Size(725, 485)
        Me.VideosTab.TabIndex = 2
        Me.VideosTab.Text = "Tutorial videos"
        Me.VideosTab.UseVisualStyleBackColor = True
        '
        'SidePanel
        '
        Me.SidePanel.BackColor = System.Drawing.Color.White
        Me.SidePanel.Controls.Add(Me.Label11)
        Me.SidePanel.Controls.Add(Me.ExistingProjLink)
        Me.SidePanel.Controls.Add(Me.NewProjLink)
        Me.SidePanel.Controls.Add(Me.Label10)
        Me.SidePanel.Controls.Add(Me.LabelHeader1)
        Me.SidePanel.Controls.Add(Me.Panel3)
        Me.SidePanel.Dock = System.Windows.Forms.DockStyle.Left
        Me.SidePanel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SidePanel.Location = New System.Drawing.Point(0, 0)
        Me.SidePanel.Name = "SidePanel"
        Me.SidePanel.Size = New System.Drawing.Size(256, 537)
        Me.SidePanel.TabIndex = 0
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(34, 203)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(188, 20)
        Me.Label11.TabIndex = 3
        Me.Label11.Text = "Coming soon!"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ExistingProjLink
        '
        Me.ExistingProjLink.AutoSize = True
        Me.ExistingProjLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.ExistingProjLink.LinkColor = System.Drawing.Color.DodgerBlue
        Me.ExistingProjLink.Location = New System.Drawing.Point(31, 132)
        Me.ExistingProjLink.Name = "ExistingProjLink"
        Me.ExistingProjLink.Size = New System.Drawing.Size(129, 15)
        Me.ExistingProjLink.TabIndex = 2
        Me.ExistingProjLink.TabStop = True
        Me.ExistingProjLink.Text = "Open existing project..."
        '
        'NewProjLink
        '
        Me.NewProjLink.AutoSize = True
        Me.NewProjLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.NewProjLink.LinkColor = System.Drawing.Color.DodgerBlue
        Me.NewProjLink.Location = New System.Drawing.Point(31, 114)
        Me.NewProjLink.Name = "NewProjLink"
        Me.NewProjLink.Size = New System.Drawing.Size(80, 15)
        Me.NewProjLink.TabIndex = 2
        Me.NewProjLink.TabStop = True
        Me.NewProjLink.Text = "New project..."
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(14, 170)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(116, 21)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = "Recent projects"
        '
        'LabelHeader1
        '
        Me.LabelHeader1.AutoSize = True
        Me.LabelHeader1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelHeader1.Location = New System.Drawing.Point(14, 76)
        Me.LabelHeader1.Name = "LabelHeader1"
        Me.LabelHeader1.Size = New System.Drawing.Size(49, 21)
        Me.LabelHeader1.TabIndex = 1
        Me.LabelHeader1.Text = "Begin"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.PictureBox5)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(0, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(256, 64)
        Me.Panel3.TabIndex = 0
        '
        'PictureBox5
        '
        Me.PictureBox5.Image = Global.DISMTools.My.Resources.Resources.logo_mainscr_light
        Me.PictureBox5.Location = New System.Drawing.Point(32, 14)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(192, 36)
        Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox5.TabIndex = 7
        Me.PictureBox5.TabStop = False
        '
        'PrjPanel
        '
        Me.PrjPanel.Controls.Add(Me.SplitPanels)
        Me.PrjPanel.Controls.Add(Me.ToolStrip1)
        Me.PrjPanel.Controls.Add(Me.Panel2)
        Me.PrjPanel.Controls.Add(Me.StatusStrip)
        Me.PrjPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PrjPanel.Location = New System.Drawing.Point(0, 24)
        Me.PrjPanel.Name = "PrjPanel"
        Me.PrjPanel.Size = New System.Drawing.Size(1008, 537)
        Me.PrjPanel.TabIndex = 4
        '
        'SplitPanels
        '
        Me.SplitPanels.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitPanels.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitPanels.Location = New System.Drawing.Point(0, 25)
        Me.SplitPanels.Name = "SplitPanels"
        '
        'SplitPanels.Panel1
        '
        Me.SplitPanels.Panel1.Controls.Add(Me.TabControl1)
        Me.SplitPanels.Panel1MinSize = 256
        '
        'SplitPanels.Panel2
        '
        Me.SplitPanels.Panel2.Controls.Add(Me.TabControl2)
        Me.SplitPanels.Panel2MinSize = 384
        Me.SplitPanels.Size = New System.Drawing.Size(752, 490)
        Me.SplitPanels.SplitterDistance = 264
        Me.SplitPanels.SplitterWidth = 2
        Me.SplitPanels.TabIndex = 1
        Me.SplitPanels.Visible = False
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(264, 490)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.UnloadBtn)
        Me.TabPage1.Controls.Add(Me.ProjNameEditBtn)
        Me.TabPage1.Controls.Add(Me.ExplorerView)
        Me.TabPage1.Controls.Add(Me.LinkLabel1)
        Me.TabPage1.Controls.Add(Me.Label5)
        Me.TabPage1.Controls.Add(Me.Button14)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.projName)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.TabTitleSummary1)
        Me.TabPage1.Controls.Add(Me.projNameText)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(256, 464)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Project"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'UnloadBtn
        '
        Me.UnloadBtn.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UnloadBtn.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.UnloadBtn.Location = New System.Drawing.Point(36, 422)
        Me.UnloadBtn.Name = "UnloadBtn"
        Me.UnloadBtn.Size = New System.Drawing.Size(185, 28)
        Me.UnloadBtn.TabIndex = 1
        Me.UnloadBtn.Text = "Unload project..."
        Me.UnloadBtn.UseVisualStyleBackColor = True
        '
        'ProjNameEditBtn
        '
        Me.ProjNameEditBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProjNameEditBtn.Image = Global.DISMTools.My.Resources.Resources.proj_name_edit
        Me.ProjNameEditBtn.Location = New System.Drawing.Point(209, 81)
        Me.ProjNameEditBtn.Name = "ProjNameEditBtn"
        Me.ProjNameEditBtn.Size = New System.Drawing.Size(24, 23)
        Me.ProjNameEditBtn.TabIndex = 7
        Me.ProjNameEditBtn.UseVisualStyleBackColor = True
        '
        'ExplorerView
        '
        Me.ExplorerView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExplorerView.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ExplorerView.Location = New System.Drawing.Point(36, 388)
        Me.ExplorerView.Name = "ExplorerView"
        Me.ExplorerView.Size = New System.Drawing.Size(185, 28)
        Me.ExplorerView.TabIndex = 1
        Me.ExplorerView.Text = "View in File Explorer"
        Me.ExplorerView.UseVisualStyleBackColor = True
        '
        'LinkLabel1
        '
        Me.LinkLabel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel1.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel1.Location = New System.Drawing.Point(122, 193)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(111, 32)
        Me.LinkLabel1.TabIndex = 5
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Click here to mount an image"
        '
        'Label5
        '
        Me.Label5.AutoEllipsis = True
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(122, 180)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(54, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "imgStatus"
        '
        'Button14
        '
        Me.Button14.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button14.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button14.Location = New System.Drawing.Point(36, 354)
        Me.Button14.Name = "Button14"
        Me.Button14.Size = New System.Drawing.Size(185, 28)
        Me.Button14.TabIndex = 1
        Me.Button14.Text = "View project properties"
        Me.Button14.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(24, 180)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(92, 13)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Images mounted?"
        '
        'Label3
        '
        Me.Label3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoEllipsis = True
        Me.Label3.Location = New System.Drawing.Point(81, 116)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(152, 28)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "projPath"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(24, 116)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Location:"
        '
        'projName
        '
        Me.projName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.projName.AutoEllipsis = True
        Me.projName.Location = New System.Drawing.Point(68, 80)
        Me.projName.Name = "projName"
        Me.projName.Size = New System.Drawing.Size(135, 24)
        Me.projName.TabIndex = 2
        Me.projName.Text = "projName"
        Me.projName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 86)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Name:"
        '
        'TabTitleSummary1
        '
        Me.TabTitleSummary1.Controls.Add(Me.TabPageIcon1)
        Me.TabTitleSummary1.Controls.Add(Me.TabPageDescription1)
        Me.TabTitleSummary1.Controls.Add(Me.TabPageTitle1)
        Me.TabTitleSummary1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TabTitleSummary1.Location = New System.Drawing.Point(3, 3)
        Me.TabTitleSummary1.Name = "TabTitleSummary1"
        Me.TabTitleSummary1.Size = New System.Drawing.Size(250, 64)
        Me.TabTitleSummary1.TabIndex = 0
        '
        'TabPageIcon1
        '
        Me.TabPageIcon1.Image = Global.DISMTools.My.Resources.Resources.project
        Me.TabPageIcon1.Location = New System.Drawing.Point(8, 8)
        Me.TabPageIcon1.Name = "TabPageIcon1"
        Me.TabPageIcon1.Size = New System.Drawing.Size(48, 48)
        Me.TabPageIcon1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.TabPageIcon1.TabIndex = 1
        Me.TabPageIcon1.TabStop = False
        '
        'TabPageDescription1
        '
        Me.TabPageDescription1.AutoSize = True
        Me.TabPageDescription1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPageDescription1.Location = New System.Drawing.Point(63, 37)
        Me.TabPageDescription1.Name = "TabPageDescription1"
        Me.TabPageDescription1.Size = New System.Drawing.Size(123, 13)
        Me.TabPageDescription1.TabIndex = 0
        Me.TabPageDescription1.Text = "View project information"
        '
        'TabPageTitle1
        '
        Me.TabPageTitle1.AutoSize = True
        Me.TabPageTitle1.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPageTitle1.Location = New System.Drawing.Point(62, 14)
        Me.TabPageTitle1.Name = "TabPageTitle1"
        Me.TabPageTitle1.Size = New System.Drawing.Size(67, 23)
        Me.TabPageTitle1.TabIndex = 0
        Me.TabPageTitle1.Text = "Project"
        '
        'projNameText
        '
        Me.projNameText.Location = New System.Drawing.Point(68, 83)
        Me.projNameText.Name = "projNameText"
        Me.projNameText.Size = New System.Drawing.Size(135, 21)
        Me.projNameText.TabIndex = 4
        Me.projNameText.Text = "projName"
        Me.projNameText.Visible = False
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.ImageNotMountedPanel)
        Me.TabPage2.Controls.Add(Me.ImagePanel)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(256, 464)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Image"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'ImagePanel
        '
        Me.ImagePanel.Controls.Add(Me.TabTitleSummary2)
        Me.ImagePanel.Controls.Add(Me.Label15)
        Me.ImagePanel.Controls.Add(Me.Label14)
        Me.ImagePanel.Controls.Add(Me.Label17)
        Me.ImagePanel.Controls.Add(Me.Label18)
        Me.ImagePanel.Controls.Add(Me.Label13)
        Me.ImagePanel.Controls.Add(Me.Label20)
        Me.ImagePanel.Controls.Add(Me.Label16)
        Me.ImagePanel.Controls.Add(Me.Label19)
        Me.ImagePanel.Controls.Add(Me.Label21)
        Me.ImagePanel.Controls.Add(Me.Label12)
        Me.ImagePanel.Controls.Add(Me.Button16)
        Me.ImagePanel.Controls.Add(Me.Button15)
        Me.ImagePanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ImagePanel.Location = New System.Drawing.Point(3, 3)
        Me.ImagePanel.Name = "ImagePanel"
        Me.ImagePanel.Size = New System.Drawing.Size(250, 458)
        Me.ImagePanel.TabIndex = 9
        '
        'TabTitleSummary2
        '
        Me.TabTitleSummary2.Controls.Add(Me.TabPageIcon2)
        Me.TabTitleSummary2.Controls.Add(Me.TabPageDescription2)
        Me.TabTitleSummary2.Controls.Add(Me.TabPageTitle2)
        Me.TabTitleSummary2.Dock = System.Windows.Forms.DockStyle.Top
        Me.TabTitleSummary2.Location = New System.Drawing.Point(0, 0)
        Me.TabTitleSummary2.Name = "TabTitleSummary2"
        Me.TabTitleSummary2.Size = New System.Drawing.Size(250, 64)
        Me.TabTitleSummary2.TabIndex = 1
        '
        'TabPageIcon2
        '
        Me.TabPageIcon2.Image = Global.DISMTools.My.Resources.Resources.image
        Me.TabPageIcon2.Location = New System.Drawing.Point(8, 8)
        Me.TabPageIcon2.Name = "TabPageIcon2"
        Me.TabPageIcon2.Size = New System.Drawing.Size(48, 48)
        Me.TabPageIcon2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.TabPageIcon2.TabIndex = 1
        Me.TabPageIcon2.TabStop = False
        '
        'TabPageDescription2
        '
        Me.TabPageDescription2.AutoSize = True
        Me.TabPageDescription2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPageDescription2.Location = New System.Drawing.Point(63, 37)
        Me.TabPageDescription2.Name = "TabPageDescription2"
        Me.TabPageDescription2.Size = New System.Drawing.Size(117, 13)
        Me.TabPageDescription2.TabIndex = 0
        Me.TabPageDescription2.Text = "View image information"
        '
        'TabPageTitle2
        '
        Me.TabPageTitle2.AutoSize = True
        Me.TabPageTitle2.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPageTitle2.Location = New System.Drawing.Point(62, 14)
        Me.TabPageTitle2.Name = "TabPageTitle2"
        Me.TabPageTitle2.Size = New System.Drawing.Size(64, 23)
        Me.TabPageTitle2.TabIndex = 0
        Me.TabPageTitle2.Text = "Image"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(27, 81)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(70, 13)
        Me.Label15.TabIndex = 4
        Me.Label15.Text = "Image index:"
        '
        'Label14
        '
        Me.Label14.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label14.AutoEllipsis = True
        Me.Label14.Location = New System.Drawing.Point(103, 75)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(103, 24)
        Me.Label14.TabIndex = 6
        Me.Label14.Text = "imgIndex"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label17
        '
        Me.Label17.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label17.AutoEllipsis = True
        Me.Label17.Location = New System.Drawing.Point(79, 218)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(157, 56)
        Me.Label17.TabIndex = 6
        Me.Label17.Text = "imgVersion"
        '
        'Label18
        '
        Me.Label18.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label18.AutoEllipsis = True
        Me.Label18.Location = New System.Drawing.Point(71, 283)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(165, 56)
        Me.Label18.TabIndex = 6
        Me.Label18.Text = "imgName"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(27, 111)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(68, 13)
        Me.Label13.TabIndex = 3
        Me.Label13.Text = "Mount point:"
        '
        'Label20
        '
        Me.Label20.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label20.AutoEllipsis = True
        Me.Label20.Location = New System.Drawing.Point(97, 345)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(139, 32)
        Me.Label20.TabIndex = 6
        Me.Label20.Text = "imgDesc"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(27, 218)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(46, 13)
        Me.Label16.TabIndex = 3
        Me.Label16.Text = "Version:"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(27, 283)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(38, 13)
        Me.Label19.TabIndex = 3
        Me.Label19.Text = "Name:"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(27, 345)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(64, 13)
        Me.Label21.TabIndex = 3
        Me.Label21.Text = "Description:"
        '
        'Label12
        '
        Me.Label12.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label12.AutoEllipsis = True
        Me.Label12.Location = New System.Drawing.Point(101, 111)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(135, 54)
        Me.Label12.TabIndex = 5
        Me.Label12.Text = "mountPoint"
        '
        'Button16
        '
        Me.Button16.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button16.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button16.Location = New System.Drawing.Point(33, 419)
        Me.Button16.Name = "Button16"
        Me.Button16.Size = New System.Drawing.Size(185, 28)
        Me.Button16.TabIndex = 8
        Me.Button16.Text = "Unmount image..."
        Me.Button16.UseVisualStyleBackColor = True
        '
        'Button15
        '
        Me.Button15.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button15.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button15.Location = New System.Drawing.Point(33, 385)
        Me.Button15.Name = "Button15"
        Me.Button15.Size = New System.Drawing.Size(185, 28)
        Me.Button15.TabIndex = 8
        Me.Button15.Text = "View image properties"
        Me.Button15.UseVisualStyleBackColor = True
        '
        'ImageNotMountedPanel
        '
        Me.ImageNotMountedPanel.Controls.Add(Me.LinkLabel2)
        Me.ImageNotMountedPanel.Controls.Add(Me.Label23)
        Me.ImageNotMountedPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ImageNotMountedPanel.Location = New System.Drawing.Point(3, 3)
        Me.ImageNotMountedPanel.Name = "ImageNotMountedPanel"
        Me.ImageNotMountedPanel.Size = New System.Drawing.Size(250, 458)
        Me.ImageNotMountedPanel.TabIndex = 0
        '
        'LinkLabel2
        '
        Me.LinkLabel2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LinkLabel2.ForeColor = System.Drawing.Color.Crimson
        Me.LinkLabel2.LinkArea = New System.Windows.Forms.LinkArea(72, 4)
        Me.LinkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline
        Me.LinkLabel2.LinkColor = System.Drawing.Color.DodgerBlue
        Me.LinkLabel2.Location = New System.Drawing.Point(21, 233)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(210, 44)
        Me.LinkLabel2.TabIndex = 1
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "You need to mount an image in order to view its information here. Click here to m" & _
    "ount an image."
        Me.LinkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.LinkLabel2.UseCompatibleTextRendering = True
        '
        'Label23
        '
        Me.Label23.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label23.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.Crimson
        Me.Label23.Location = New System.Drawing.Point(20, 182)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(211, 51)
        Me.Label23.TabIndex = 0
        Me.Label23.Text = "No image has been mounted"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabControl2
        '
        Me.TabControl2.Controls.Add(Me.TabPage3)
        Me.TabControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl2.Location = New System.Drawing.Point(0, 0)
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.Size = New System.Drawing.Size(486, 490)
        Me.TabControl2.TabIndex = 3
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.GroupBox3)
        Me.TabPage3.Controls.Add(Me.GroupBox2)
        Me.TabPage3.Controls.Add(Me.GroupBox1)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(478, 464)
        Me.TabPage3.TabIndex = 0
        Me.TabPage3.Text = "Actions"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox3.Controls.Add(Me.Button8)
        Me.GroupBox3.Controls.Add(Me.Button9)
        Me.GroupBox3.Controls.Add(Me.Button10)
        Me.GroupBox3.Location = New System.Drawing.Point(9, 332)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(460, 92)
        Me.GroupBox3.TabIndex = 2
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Feature operations"
        '
        'Button8
        '
        Me.Button8.Enabled = False
        Me.Button8.Image = Global.DISMTools.My.Resources.Resources.get_feat_info
        Me.Button8.Location = New System.Drawing.Point(118, 20)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(201, 63)
        Me.Button8.TabIndex = 1
        Me.Button8.Text = "Get feature information..."
        Me.Button8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Button9
        '
        Me.Button9.Enabled = False
        Me.Button9.Image = Global.DISMTools.My.Resources.Resources.disable_feature
        Me.Button9.Location = New System.Drawing.Point(325, 20)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(129, 63)
        Me.Button9.TabIndex = 1
        Me.Button9.Text = "Disable feature..."
        Me.Button9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button9.UseVisualStyleBackColor = True
        '
        'Button10
        '
        Me.Button10.Enabled = False
        Me.Button10.Image = Global.DISMTools.My.Resources.Resources.enable_feature
        Me.Button10.Location = New System.Drawing.Point(6, 20)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(106, 63)
        Me.Button10.TabIndex = 1
        Me.Button10.Text = "Enable feature..."
        Me.Button10.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button10.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox2.Controls.Add(Me.Button6)
        Me.GroupBox2.Controls.Add(Me.Button7)
        Me.GroupBox2.Controls.Add(Me.Button5)
        Me.GroupBox2.Controls.Add(Me.Button12)
        Me.GroupBox2.Location = New System.Drawing.Point(9, 205)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(460, 121)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Package operations"
        '
        'Button6
        '
        Me.Button6.Enabled = False
        Me.Button6.Image = Global.DISMTools.My.Resources.Resources.get_pkg_info
        Me.Button6.Location = New System.Drawing.Point(118, 20)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(201, 63)
        Me.Button6.TabIndex = 1
        Me.Button6.Text = "Get package information..."
        Me.Button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.Enabled = False
        Me.Button7.Image = Global.DISMTools.My.Resources.Resources.rem_pkg
        Me.Button7.Location = New System.Drawing.Point(325, 20)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(129, 63)
        Me.Button7.TabIndex = 1
        Me.Button7.Text = "Remove package..."
        Me.Button7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Enabled = False
        Me.Button5.Image = Global.DISMTools.My.Resources.Resources.add_pkg
        Me.Button5.Location = New System.Drawing.Point(6, 20)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(106, 63)
        Me.Button5.TabIndex = 1
        Me.Button5.Text = "Add package..."
        Me.Button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button12
        '
        Me.Button12.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button12.Enabled = False
        Me.Button12.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button12.Location = New System.Drawing.Point(6, 87)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(448, 28)
        Me.Button12.TabIndex = 1
        Me.Button12.Text = "Perform component cleanup and/or repair..."
        Me.Button12.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.GroupBox1.Controls.Add(Me.Button13)
        Me.GroupBox1.Controls.Add(Me.Button11)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.Button2)
        Me.GroupBox1.Controls.Add(Me.Button3)
        Me.GroupBox1.Controls.Add(Me.Button4)
        Me.GroupBox1.Location = New System.Drawing.Point(9, 40)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(460, 159)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Image operations"
        '
        'Button13
        '
        Me.Button13.Enabled = False
        Me.Button13.Image = Global.DISMTools.My.Resources.Resources.switch_indexes
        Me.Button13.Location = New System.Drawing.Point(233, 89)
        Me.Button13.Name = "Button13"
        Me.Button13.Size = New System.Drawing.Size(221, 63)
        Me.Button13.TabIndex = 1
        Me.Button13.Text = "Switch indexes..."
        Me.Button13.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button13.UseVisualStyleBackColor = True
        '
        'Button11
        '
        Me.Button11.Enabled = False
        Me.Button11.Image = Global.DISMTools.My.Resources.Resources.servsession_reload
        Me.Button11.Location = New System.Drawing.Point(6, 89)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(221, 63)
        Me.Button11.TabIndex = 1
        Me.Button11.Text = "Reload servicing session..."
        Me.Button11.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button11.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Image = Global.DISMTools.My.Resources.Resources.mount_img
        Me.Button1.Location = New System.Drawing.Point(6, 20)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(106, 63)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Mount image..."
        Me.Button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Enabled = False
        Me.Button2.Image = Global.DISMTools.My.Resources.Resources.commit_img
        Me.Button2.Location = New System.Drawing.Point(118, 20)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(145, 63)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Commit current changes"
        Me.Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button3.Enabled = False
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button3.Location = New System.Drawing.Point(269, 20)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(185, 28)
        Me.Button3.TabIndex = 1
        Me.Button3.Text = "Commit and unmount image"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.Enabled = False
        Me.Button4.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button4.Location = New System.Drawing.Point(269, 54)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(185, 28)
        Me.Button4.TabIndex = 1
        Me.Button4.Text = "Unmount image discarding changes"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripButton2, Me.ToolStripSeparator14, Me.ToolStripButton3, Me.ToolStripSeparator15, Me.ToolStripButton4})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(752, 25)
        Me.ToolStrip1.TabIndex = 2
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Enabled = False
        Me.ToolStripButton1.Image = Global.DISMTools.My.Resources.Resources.close_glyph
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton1.Text = "Close tab"
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton2.Image = Global.DISMTools.My.Resources.Resources.save_glyph
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton2.Text = "Save project"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButton3
        '
        Me.ToolStripButton3.Image = Global.DISMTools.My.Resources.Resources.prj_unload_glyph
        Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton3.Name = "ToolStripButton3"
        Me.ToolStripButton3.Size = New System.Drawing.Size(105, 22)
        Me.ToolStripButton3.Text = "Unload project"
        Me.ToolStripButton3.ToolTipText = "Unload project from this program"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButton4
        '
        Me.ToolStripButton4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton4.Image = Global.DISMTools.My.Resources.Resources.progress_window
        Me.ToolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton4.Name = "ToolStripButton4"
        Me.ToolStripButton4.Size = New System.Drawing.Size(23, 22)
        Me.ToolStripButton4.Text = "Show progress window"
        Me.ToolStripButton4.Visible = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.prjTreeView)
        Me.Panel2.Controls.Add(Me.ToolStrip2)
        Me.Panel2.Controls.Add(Me.prjTreeStatus)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel2.Location = New System.Drawing.Point(752, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(256, 515)
        Me.Panel2.TabIndex = 1
        '
        'prjTreeView
        '
        Me.prjTreeView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.prjTreeView.Location = New System.Drawing.Point(0, 25)
        Me.prjTreeView.Name = "prjTreeView"
        Me.prjTreeView.Size = New System.Drawing.Size(256, 490)
        Me.prjTreeView.TabIndex = 3
        '
        'ToolStrip2
        '
        Me.ToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RefreshViewTSB, Me.ToolStripSeparator17, Me.ExpandCollapseTSB})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(256, 25)
        Me.ToolStrip2.TabIndex = 1
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'RefreshViewTSB
        '
        Me.RefreshViewTSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.RefreshViewTSB.Image = Global.DISMTools.My.Resources.Resources.refresh_glyph
        Me.RefreshViewTSB.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.RefreshViewTSB.Name = "RefreshViewTSB"
        Me.RefreshViewTSB.Size = New System.Drawing.Size(23, 22)
        Me.RefreshViewTSB.Text = "Refresh view"
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(6, 25)
        '
        'ExpandCollapseTSB
        '
        Me.ExpandCollapseTSB.Image = Global.DISMTools.My.Resources.Resources.expand_glyph
        Me.ExpandCollapseTSB.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ExpandCollapseTSB.Name = "ExpandCollapseTSB"
        Me.ExpandCollapseTSB.Size = New System.Drawing.Size(66, 22)
        Me.ExpandCollapseTSB.Text = "Expand"
        '
        'prjTreeStatus
        '
        Me.prjTreeStatus.BackColor = System.Drawing.SystemColors.Control
        Me.prjTreeStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.prjTreeStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel2, Me.ToolStripProgressBar1})
        Me.prjTreeStatus.Location = New System.Drawing.Point(0, 400)
        Me.prjTreeStatus.Name = "prjTreeStatus"
        Me.prjTreeStatus.Size = New System.Drawing.Size(252, 22)
        Me.prjTreeStatus.SizingGrip = False
        Me.prjTreeStatus.TabIndex = 0
        Me.prjTreeStatus.Text = "StatusStrip1"
        Me.prjTreeStatus.Visible = False
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(125, 17)
        Me.ToolStripStatusLabel2.Text = "Preparing project tree..."
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(100, 16)
        '
        'StatusStrip
        '
        Me.StatusStrip.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuDesc})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 515)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(1008, 22)
        Me.StatusStrip.TabIndex = 0
        Me.StatusStrip.Text = "Status"
        '
        'MenuDesc
        '
        Me.MenuDesc.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuDesc.Name = "MenuDesc"
        Me.MenuDesc.Size = New System.Drawing.Size(39, 17)
        Me.MenuDesc.Text = "Ready"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.Filter = "DISMTools project files|*.dtproj"
        Me.OpenFileDialog1.RestoreDirectory = True
        Me.OpenFileDialog1.SupportMultiDottedExtensions = True
        Me.OpenFileDialog1.Title = "Specify the project file to load"
        '
        'PkgInfoCMS
        '
        Me.PkgInfoCMS.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PkgInfoCMS.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PkgBasicInfo, Me.PkgDetailedInfo})
        Me.PkgInfoCMS.Name = "PkgInfoCMS"
        Me.PkgInfoCMS.ShowImageMargin = False
        Me.PkgInfoCMS.Size = New System.Drawing.Size(277, 48)
        '
        'PkgBasicInfo
        '
        Me.PkgBasicInfo.Name = "PkgBasicInfo"
        Me.PkgBasicInfo.Size = New System.Drawing.Size(276, 22)
        Me.PkgBasicInfo.Text = "Get basic information (all packages)"
        '
        'PkgDetailedInfo
        '
        Me.PkgDetailedInfo.Name = "PkgDetailedInfo"
        Me.PkgDetailedInfo.Size = New System.Drawing.Size(276, 22)
        Me.PkgDetailedInfo.Text = "Get detailed information (specific package)"
        '
        'FeatureInfoCMS
        '
        Me.FeatureInfoCMS.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.FeatureInfoCMS.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FeatureBasicInfo, Me.FeatureDetailedInfo})
        Me.FeatureInfoCMS.Name = "PkgInfoCMS"
        Me.FeatureInfoCMS.ShowImageMargin = False
        Me.FeatureInfoCMS.Size = New System.Drawing.Size(270, 48)
        '
        'FeatureBasicInfo
        '
        Me.FeatureBasicInfo.Name = "FeatureBasicInfo"
        Me.FeatureBasicInfo.Size = New System.Drawing.Size(269, 22)
        Me.FeatureBasicInfo.Text = "Get basic information (all features)"
        '
        'FeatureDetailedInfo
        '
        Me.FeatureDetailedInfo.Name = "FeatureDetailedInfo"
        Me.FeatureDetailedInfo.Size = New System.Drawing.Size(269, 22)
        Me.FeatureDetailedInfo.Text = "Get detailed information (specific feature)"
        '
        'ImgBW
        '
        '
        'ImgProcesses
        '
        Me.ImgProcesses.StartInfo.Domain = ""
        Me.ImgProcesses.StartInfo.LoadUserProfile = False
        Me.ImgProcesses.StartInfo.Password = Nothing
        Me.ImgProcesses.StartInfo.StandardErrorEncoding = Nothing
        Me.ImgProcesses.StartInfo.StandardOutputEncoding = Nothing
        Me.ImgProcesses.StartInfo.UserName = ""
        Me.ImgProcesses.SynchronizingObject = Me
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1008, 561)
        Me.Controls.Add(Me.HomePanel)
        Me.Controls.Add(Me.PrjPanel)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(1024, 600)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DISMTools"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.HomePanel.ResumeLayout(False)
        Me.WelcomePanel.ResumeLayout(False)
        Me.WelcomeTabControl.ResumeLayout(False)
        Me.WelcomeTab.ResumeLayout(False)
        Me.WelcomeTab.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SidePanel.ResumeLayout(False)
        Me.SidePanel.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PrjPanel.ResumeLayout(False)
        Me.PrjPanel.PerformLayout()
        Me.SplitPanels.Panel1.ResumeLayout(False)
        Me.SplitPanels.Panel2.ResumeLayout(False)
        CType(Me.SplitPanels, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitPanels.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabTitleSummary1.ResumeLayout(False)
        Me.TabTitleSummary1.PerformLayout()
        CType(Me.TabPageIcon1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.ImagePanel.ResumeLayout(False)
        Me.ImagePanel.PerformLayout()
        Me.TabTitleSummary2.ResumeLayout(False)
        Me.TabTitleSummary2.PerformLayout()
        CType(Me.TabPageIcon2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ImageNotMountedPanel.ResumeLayout(False)
        Me.TabControl2.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.prjTreeStatus.ResumeLayout(False)
        Me.prjTreeStatus.PerformLayout()
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.PkgInfoCMS.ResumeLayout(False)
        Me.FeatureInfoCMS.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenExistingProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SaveProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveProjectasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewProjectFilesInFileExplorerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UnloadProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SwitchImageIndexesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProjectPropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CommandsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImageManagementToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AppendImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ApplyFFU As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ApplyImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CaptureCustomImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CaptureFFU As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CaptureImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CleanupMountpoints As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CommitImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetImageInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetMountedImageInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetWIMBootEntry As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ListImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MountImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptimizeFFU As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptimizeImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemountImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitFFU As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SplitImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UnmountImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UpdateWIMBootEntry As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ApplySiloedPackage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OSPackagesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetPackages As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetPackageInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddPackage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemovePackage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetFeatures As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetFeatureInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EnableFeature As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DisableFeature As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents CleanupImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProvisioningPackagesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddProvisioningPackage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetProvisioningPackageInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ApplyCustomDataImage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AppPackagesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetProvisionedAppxPackages As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddProvisionedAppxPackage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveProvisionedAppxPackage As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptimizeProvisionedAppxPackages As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetProvisionedAppxDataFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AppPatchesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CheckAppPatch As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetAppPatchInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetAppPatches As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetAppInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetApps As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DefaultAppAssociationsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportDefaultAppAssociations As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetDefaultAppAssociations As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImportDefaultAppAssociations As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveDefaultAppAssociations As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LanguagesAndRegionSettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetIntl As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetUILang As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetUILangFallback As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetSysUILang As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetSysLocale As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetUserLocale As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetInputLocale As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SetAllIntl As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SetTimeZone As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SetSKUIntlDefaults As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SetLayeredDriver As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GenLangINI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetSetupUILang As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CapabilitiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddCapability As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportSource As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetCapabilities As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetCapabilityInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveCapability As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WindowsEditionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetCurrentEdition As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetTargetEditions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetEdition As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetProductKey As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DriversToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetDrivers As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetDriverInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddDriver As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveDriver As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportDriver As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UnattendedAnswerFilesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ApplyUnattend As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WindowsPEServicingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetPESettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetScratchSpace As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetTargetPath As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetScratchSpace As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetTargetPath As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OSUninstallToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetOSUninstallWindow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InitiateOSUninstall As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveOSUninstall As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetOSUninstallWindow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReservedStorageToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetReservedStorageState As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GetReservedStorageState As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpTopicsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GlossaryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CommandHelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AboutDISMToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HomePanel As System.Windows.Forms.Panel
    Friend WithEvents WelcomePanel As System.Windows.Forms.Panel
    Friend WithEvents SidePanel As System.Windows.Forms.Panel
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ExistingProjLink As System.Windows.Forms.LinkLabel
    Friend WithEvents NewProjLink As System.Windows.Forms.LinkLabel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents LabelHeader1 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents PrjPanel As System.Windows.Forms.Panel
    Friend WithEvents SplitPanels As System.Windows.Forms.SplitContainer
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents ProjNameEditBtn As System.Windows.Forms.Button
    Friend WithEvents UnloadBtn As System.Windows.Forms.Button
    Friend WithEvents ExplorerView As System.Windows.Forms.Button
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents projName As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TabTitleSummary1 As System.Windows.Forms.Panel
    Friend WithEvents TabPageIcon1 As System.Windows.Forms.PictureBox
    Friend WithEvents TabPageDescription1 As System.Windows.Forms.Label
    Friend WithEvents TabPageTitle1 As System.Windows.Forms.Label
    Friend WithEvents projNameText As System.Windows.Forms.TextBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents TabTitleSummary2 As System.Windows.Forms.Panel
    Friend WithEvents TabPageIcon2 As System.Windows.Forms.PictureBox
    Friend WithEvents TabPageDescription2 As System.Windows.Forms.Label
    Friend WithEvents TabPageTitle2 As System.Windows.Forms.Label
    Friend WithEvents TabControl2 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button12 As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button13 As System.Windows.Forms.Button
    Friend WithEvents Button11 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripButton1 As System.Windows.Forms.ToolStripButton
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents MenuDesc As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ImagePropertiesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents prjTreeStatus As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel2 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents ToolStripButton2 As System.Windows.Forms.ToolStripButton
    Friend WithEvents Button14 As System.Windows.Forms.Button
    Friend WithEvents Button15 As System.Windows.Forms.Button
    Friend WithEvents ImageConversionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WIMESDToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ImageNotMountedPanel As System.Windows.Forms.Panel
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents ImagePanel As System.Windows.Forms.Panel
    Friend WithEvents WelcomeTabControl As System.Windows.Forms.TabControl
    Friend WithEvents WelcomeTab As System.Windows.Forms.TabPage
    Friend WithEvents NewsFeedTab As System.Windows.Forms.TabPage
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents VideosTab As System.Windows.Forms.TabPage
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents PkgInfoCMS As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents PkgBasicInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PkgDetailedInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FeatureInfoCMS As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents FeatureBasicInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FeatureDetailedInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents RemountImageWithWritePermissionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator13 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator14 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton3 As System.Windows.Forms.ToolStripButton
    Friend WithEvents CommandShellToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator16 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Button16 As System.Windows.Forms.Button
    Friend WithEvents UnattendedAnswerFileManagerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents MicrosoftEdgeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddEdge As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddEdgeBrowser As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddEdgeWebView As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReportManagerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents prjTreeView As System.Windows.Forms.TreeView
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents RefreshViewTSB As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator17 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExpandCollapseTSB As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator15 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripButton4 As System.Windows.Forms.ToolStripButton
    Friend WithEvents MergeSWM As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator18 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents VersionTSMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InvalidSettingsTSMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ISFix As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator19 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ISHelp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BranchTSMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReportFeedbackToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImgBW As System.ComponentModel.BackgroundWorker
    Friend WithEvents ImgProcesses As Process
End Class
