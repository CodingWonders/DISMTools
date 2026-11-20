<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddComputerFromModelCatalogDialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AddComputerFromModelCatalogDialog))
        Me.pnlButtonContainer = New System.Windows.Forms.Panel()
        Me.tlpActions = New System.Windows.Forms.TableLayoutPanel()
        Me.Help_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.Next_Button = New System.Windows.Forms.Button()
        Me.Back_Button = New System.Windows.Forms.Button()
        Me.flpWizardSteps = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlGap = New System.Windows.Forms.Panel()
        Me.WizardStep1 = New DrvPakCreator.WizardStep()
        Me.WizardStep2 = New DrvPakCreator.WizardStep()
        Me.WizardStep3 = New DrvPakCreator.WizardStep()
        Me.WizardStep4 = New DrvPakCreator.WizardStep()
        Me.WizardStep5 = New DrvPakCreator.WizardStep()
        Me.pnlWizardStepContainer = New System.Windows.Forms.Panel()
        Me.pnlFinish = New System.Windows.Forms.Panel()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.tbComputerAdditionOverview = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.pnlDrvSelect = New System.Windows.Forms.Panel()
        Me.pnlModelDriverContainer = New System.Windows.Forms.Panel()
        Me.pnlHPDriverSelect = New System.Windows.Forms.Panel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.tvSoftPaqSubdirs = New System.Windows.Forms.TreeView()
        Me.lvHPDriverFiles = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lbSelectedDrivers = New System.Windows.Forms.ListBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.pnlModelSelect = New System.Windows.Forms.Panel()
        Me.pnlModelSelectorContainer = New System.Windows.Forms.Panel()
        Me.pnlModelContainer = New System.Windows.Forms.Panel()
        Me.pnlHPModels = New System.Windows.Forms.Panel()
        Me.lvHpProducts = New System.Windows.Forms.ListView()
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.pnlLenovoModels = New System.Windows.Forms.Panel()
        Me.pnlDellModels = New System.Windows.Forms.Panel()
        Me.pnlModelDetailContainer = New System.Windows.Forms.Panel()
        Me.lblDrvPackCount = New System.Windows.Forms.Label()
        Me.lblSysId = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.lblModelName = New System.Windows.Forms.Label()
        Me.pnlModelSearch = New System.Windows.Forms.Panel()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.pnlManufacturerSelect = New System.Windows.Forms.Panel()
        Me.btnSelectLenovo = New System.Windows.Forms.Button()
        Me.btnSelectDell = New System.Windows.Forms.Button()
        Me.btnSelectHP = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.pbHP = New System.Windows.Forms.PictureBox()
        Me.pbDell = New System.Windows.Forms.PictureBox()
        Me.pbLenovo = New System.Windows.Forms.PictureBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.pnlWelcome = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlButtonContainer.SuspendLayout()
        Me.tlpActions.SuspendLayout()
        Me.flpWizardSteps.SuspendLayout()
        Me.pnlWizardStepContainer.SuspendLayout()
        Me.pnlFinish.SuspendLayout()
        Me.pnlDrvSelect.SuspendLayout()
        Me.pnlModelDriverContainer.SuspendLayout()
        Me.pnlHPDriverSelect.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.pnlModelSelect.SuspendLayout()
        Me.pnlModelSelectorContainer.SuspendLayout()
        Me.pnlModelContainer.SuspendLayout()
        Me.pnlHPModels.SuspendLayout()
        Me.pnlModelDetailContainer.SuspendLayout()
        Me.pnlModelSearch.SuspendLayout()
        Me.pnlManufacturerSelect.SuspendLayout()
        CType(Me.pbHP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbDell, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbLenovo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlWelcome.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlButtonContainer
        '
        Me.pnlButtonContainer.Controls.Add(Me.tlpActions)
        Me.pnlButtonContainer.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlButtonContainer.Location = New System.Drawing.Point(0, 519)
        Me.pnlButtonContainer.Name = "pnlButtonContainer"
        Me.pnlButtonContainer.Size = New System.Drawing.Size(1018, 56)
        Me.pnlButtonContainer.TabIndex = 1
        '
        'tlpActions
        '
        Me.tlpActions.ColumnCount = 4
        Me.tlpActions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpActions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpActions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpActions.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpActions.Controls.Add(Me.Help_Button, 3, 0)
        Me.tlpActions.Controls.Add(Me.Cancel_Button, 2, 0)
        Me.tlpActions.Controls.Add(Me.Next_Button, 1, 0)
        Me.tlpActions.Controls.Add(Me.Back_Button, 0, 0)
        Me.tlpActions.Location = New System.Drawing.Point(724, 15)
        Me.tlpActions.Name = "tlpActions"
        Me.tlpActions.RowCount = 1
        Me.tlpActions.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpActions.Size = New System.Drawing.Size(282, 29)
        Me.tlpActions.TabIndex = 0
        '
        'Help_Button
        '
        Me.Help_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Help_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Help_Button.Location = New System.Drawing.Point(213, 3)
        Me.Help_Button.Name = "Help_Button"
        Me.Help_Button.Size = New System.Drawing.Size(66, 23)
        Me.Help_Button.TabIndex = 3
        Me.Help_Button.Text = "Help"
        Me.Help_Button.UseVisualStyleBackColor = True
        '
        'Cancel_Button
        '
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Cancel_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Cancel_Button.Location = New System.Drawing.Point(143, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(64, 23)
        Me.Cancel_Button.TabIndex = 2
        Me.Cancel_Button.Text = "Cancel"
        Me.Cancel_Button.UseVisualStyleBackColor = True
        '
        'Next_Button
        '
        Me.Next_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Next_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Next_Button.Location = New System.Drawing.Point(73, 3)
        Me.Next_Button.Name = "Next_Button"
        Me.Next_Button.Size = New System.Drawing.Size(64, 23)
        Me.Next_Button.TabIndex = 1
        Me.Next_Button.Text = "Next"
        Me.Next_Button.UseVisualStyleBackColor = True
        '
        'Back_Button
        '
        Me.Back_Button.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Back_Button.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Back_Button.Location = New System.Drawing.Point(3, 3)
        Me.Back_Button.Name = "Back_Button"
        Me.Back_Button.Size = New System.Drawing.Size(64, 23)
        Me.Back_Button.TabIndex = 0
        Me.Back_Button.Text = "Back"
        Me.Back_Button.UseVisualStyleBackColor = True
        '
        'flpWizardSteps
        '
        Me.flpWizardSteps.Controls.Add(Me.pnlGap)
        Me.flpWizardSteps.Controls.Add(Me.WizardStep1)
        Me.flpWizardSteps.Controls.Add(Me.WizardStep2)
        Me.flpWizardSteps.Controls.Add(Me.WizardStep3)
        Me.flpWizardSteps.Controls.Add(Me.WizardStep4)
        Me.flpWizardSteps.Controls.Add(Me.WizardStep5)
        Me.flpWizardSteps.Dock = System.Windows.Forms.DockStyle.Left
        Me.flpWizardSteps.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpWizardSteps.Location = New System.Drawing.Point(0, 0)
        Me.flpWizardSteps.Name = "flpWizardSteps"
        Me.flpWizardSteps.Size = New System.Drawing.Size(260, 519)
        Me.flpWizardSteps.TabIndex = 2
        '
        'pnlGap
        '
        Me.pnlGap.Location = New System.Drawing.Point(3, 3)
        Me.pnlGap.Name = "pnlGap"
        Me.pnlGap.Size = New System.Drawing.Size(200, 1)
        Me.pnlGap.TabIndex = 3
        '
        'WizardStep1
        '
        Me.WizardStep1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WizardStep1.Location = New System.Drawing.Point(3, 10)
        Me.WizardStep1.Name = "WizardStep1"
        Me.WizardStep1.Size = New System.Drawing.Size(256, 24)
        Me.WizardStep1.TabIndex = 0
        Me.WizardStep1.WizardStepIsActive = True
        Me.WizardStep1.WizardStepName = "Welcome"
        '
        'WizardStep2
        '
        Me.WizardStep2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WizardStep2.Location = New System.Drawing.Point(3, 40)
        Me.WizardStep2.Name = "WizardStep2"
        Me.WizardStep2.Size = New System.Drawing.Size(256, 24)
        Me.WizardStep2.TabIndex = 1
        Me.WizardStep2.WizardStepIsActive = False
        Me.WizardStep2.WizardStepName = "Select Manufacturer"
        '
        'WizardStep3
        '
        Me.WizardStep3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WizardStep3.Location = New System.Drawing.Point(3, 70)
        Me.WizardStep3.Name = "WizardStep3"
        Me.WizardStep3.Size = New System.Drawing.Size(256, 24)
        Me.WizardStep3.TabIndex = 2
        Me.WizardStep3.WizardStepIsActive = False
        Me.WizardStep3.WizardStepName = "Select Model"
        '
        'WizardStep4
        '
        Me.WizardStep4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WizardStep4.Location = New System.Drawing.Point(3, 100)
        Me.WizardStep4.Name = "WizardStep4"
        Me.WizardStep4.Size = New System.Drawing.Size(256, 24)
        Me.WizardStep4.TabIndex = 2
        Me.WizardStep4.WizardStepIsActive = False
        Me.WizardStep4.WizardStepName = "Select Drivers"
        '
        'WizardStep5
        '
        Me.WizardStep5.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WizardStep5.Location = New System.Drawing.Point(3, 130)
        Me.WizardStep5.Name = "WizardStep5"
        Me.WizardStep5.Size = New System.Drawing.Size(256, 24)
        Me.WizardStep5.TabIndex = 2
        Me.WizardStep5.WizardStepIsActive = False
        Me.WizardStep5.WizardStepName = "Finish"
        '
        'pnlWizardStepContainer
        '
        Me.pnlWizardStepContainer.Controls.Add(Me.pnlFinish)
        Me.pnlWizardStepContainer.Controls.Add(Me.pnlDrvSelect)
        Me.pnlWizardStepContainer.Controls.Add(Me.pnlModelSelect)
        Me.pnlWizardStepContainer.Controls.Add(Me.pnlManufacturerSelect)
        Me.pnlWizardStepContainer.Controls.Add(Me.pnlWelcome)
        Me.pnlWizardStepContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlWizardStepContainer.Location = New System.Drawing.Point(260, 0)
        Me.pnlWizardStepContainer.Name = "pnlWizardStepContainer"
        Me.pnlWizardStepContainer.Size = New System.Drawing.Size(758, 519)
        Me.pnlWizardStepContainer.TabIndex = 3
        '
        'pnlFinish
        '
        Me.pnlFinish.Controls.Add(Me.Label12)
        Me.pnlFinish.Controls.Add(Me.tbComputerAdditionOverview)
        Me.pnlFinish.Controls.Add(Me.Label11)
        Me.pnlFinish.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlFinish.Location = New System.Drawing.Point(0, 0)
        Me.pnlFinish.Name = "pnlFinish"
        Me.pnlFinish.Size = New System.Drawing.Size(758, 519)
        Me.pnlFinish.TabIndex = 0
        '
        'Label12
        '
        Me.Label12.AutoEllipsis = True
        Me.Label12.Location = New System.Drawing.Point(60, 80)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(627, 32)
        Me.Label12.TabIndex = 6
        Me.Label12.Text = "Click Finish to begin adding this computer and all selected Driver Files."
        '
        'tbComputerAdditionOverview
        '
        Me.tbComputerAdditionOverview.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbComputerAdditionOverview.Location = New System.Drawing.Point(63, 128)
        Me.tbComputerAdditionOverview.MaxLength = 2147483647
        Me.tbComputerAdditionOverview.Multiline = True
        Me.tbComputerAdditionOverview.Name = "tbComputerAdditionOverview"
        Me.tbComputerAdditionOverview.ReadOnly = True
        Me.tbComputerAdditionOverview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbComputerAdditionOverview.Size = New System.Drawing.Size(624, 346)
        Me.tbComputerAdditionOverview.TabIndex = 5
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(19, 20)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(200, 25)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Review your options"
        '
        'pnlDrvSelect
        '
        Me.pnlDrvSelect.Controls.Add(Me.pnlModelDriverContainer)
        Me.pnlDrvSelect.Controls.Add(Me.Label20)
        Me.pnlDrvSelect.Controls.Add(Me.Label21)
        Me.pnlDrvSelect.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlDrvSelect.Location = New System.Drawing.Point(0, 0)
        Me.pnlDrvSelect.Name = "pnlDrvSelect"
        Me.pnlDrvSelect.Size = New System.Drawing.Size(758, 519)
        Me.pnlDrvSelect.TabIndex = 0
        '
        'pnlModelDriverContainer
        '
        Me.pnlModelDriverContainer.Controls.Add(Me.pnlHPDriverSelect)
        Me.pnlModelDriverContainer.Controls.Add(Me.lbSelectedDrivers)
        Me.pnlModelDriverContainer.Location = New System.Drawing.Point(63, 128)
        Me.pnlModelDriverContainer.Name = "pnlModelDriverContainer"
        Me.pnlModelDriverContainer.Size = New System.Drawing.Size(621, 349)
        Me.pnlModelDriverContainer.TabIndex = 5
        '
        'pnlHPDriverSelect
        '
        Me.pnlHPDriverSelect.Controls.Add(Me.SplitContainer1)
        Me.pnlHPDriverSelect.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlHPDriverSelect.Location = New System.Drawing.Point(0, 0)
        Me.pnlHPDriverSelect.Name = "pnlHPDriverSelect"
        Me.pnlHPDriverSelect.Size = New System.Drawing.Size(621, 254)
        Me.pnlHPDriverSelect.TabIndex = 1
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.tvSoftPaqSubdirs)
        Me.SplitContainer1.Panel1MinSize = 48
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.lvHPDriverFiles)
        Me.SplitContainer1.Panel2MinSize = 48
        Me.SplitContainer1.Size = New System.Drawing.Size(621, 254)
        Me.SplitContainer1.SplitterDistance = 207
        Me.SplitContainer1.TabIndex = 0
        '
        'tvSoftPaqSubdirs
        '
        Me.tvSoftPaqSubdirs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvSoftPaqSubdirs.Location = New System.Drawing.Point(0, 0)
        Me.tvSoftPaqSubdirs.Name = "tvSoftPaqSubdirs"
        Me.tvSoftPaqSubdirs.Size = New System.Drawing.Size(207, 254)
        Me.tvSoftPaqSubdirs.TabIndex = 0
        '
        'lvHPDriverFiles
        '
        Me.lvHPDriverFiles.CheckBoxes = True
        Me.lvHPDriverFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lvHPDriverFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvHPDriverFiles.FullRowSelect = True
        Me.lvHPDriverFiles.Location = New System.Drawing.Point(0, 0)
        Me.lvHPDriverFiles.Name = "lvHPDriverFiles"
        Me.lvHPDriverFiles.Size = New System.Drawing.Size(410, 254)
        Me.lvHPDriverFiles.TabIndex = 0
        Me.lvHPDriverFiles.UseCompatibleStateImageBehavior = False
        Me.lvHPDriverFiles.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "File Path"
        Me.ColumnHeader1.Width = 380
        '
        'lbSelectedDrivers
        '
        Me.lbSelectedDrivers.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lbSelectedDrivers.FormattingEnabled = True
        Me.lbSelectedDrivers.Location = New System.Drawing.Point(0, 254)
        Me.lbSelectedDrivers.Name = "lbSelectedDrivers"
        Me.lbSelectedDrivers.Size = New System.Drawing.Size(621, 95)
        Me.lbSelectedDrivers.TabIndex = 0
        '
        'Label20
        '
        Me.Label20.AutoEllipsis = True
        Me.Label20.Location = New System.Drawing.Point(60, 80)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(627, 32)
        Me.Label20.TabIndex = 4
        Me.Label20.Text = "Out of the following drivers, select the ones you would like to add to your Drive" & _
    "r Pack. The files you select here will act as the main driver files for a Driver" & _
    " Pack entry."
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(19, 20)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(242, 25)
        Me.Label21.TabIndex = 3
        Me.Label21.Text = "Select the drivers to add"
        '
        'pnlModelSelect
        '
        Me.pnlModelSelect.Controls.Add(Me.pnlModelSelectorContainer)
        Me.pnlModelSelect.Controls.Add(Me.Label8)
        Me.pnlModelSelect.Controls.Add(Me.Label7)
        Me.pnlModelSelect.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlModelSelect.Location = New System.Drawing.Point(0, 0)
        Me.pnlModelSelect.Name = "pnlModelSelect"
        Me.pnlModelSelect.Size = New System.Drawing.Size(758, 519)
        Me.pnlModelSelect.TabIndex = 0
        '
        'pnlModelSelectorContainer
        '
        Me.pnlModelSelectorContainer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlModelSelectorContainer.Controls.Add(Me.pnlModelContainer)
        Me.pnlModelSelectorContainer.Controls.Add(Me.pnlModelDetailContainer)
        Me.pnlModelSelectorContainer.Controls.Add(Me.pnlModelSearch)
        Me.pnlModelSelectorContainer.Location = New System.Drawing.Point(63, 130)
        Me.pnlModelSelectorContainer.Name = "pnlModelSelectorContainer"
        Me.pnlModelSelectorContainer.Size = New System.Drawing.Size(624, 346)
        Me.pnlModelSelectorContainer.TabIndex = 3
        '
        'pnlModelContainer
        '
        Me.pnlModelContainer.Controls.Add(Me.pnlHPModels)
        Me.pnlModelContainer.Controls.Add(Me.pnlLenovoModels)
        Me.pnlModelContainer.Controls.Add(Me.pnlDellModels)
        Me.pnlModelContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlModelContainer.Location = New System.Drawing.Point(0, 28)
        Me.pnlModelContainer.Name = "pnlModelContainer"
        Me.pnlModelContainer.Size = New System.Drawing.Size(624, 190)
        Me.pnlModelContainer.TabIndex = 3
        '
        'pnlHPModels
        '
        Me.pnlHPModels.Controls.Add(Me.lvHpProducts)
        Me.pnlHPModels.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlHPModels.Location = New System.Drawing.Point(0, 0)
        Me.pnlHPModels.Name = "pnlHPModels"
        Me.pnlHPModels.Size = New System.Drawing.Size(624, 190)
        Me.pnlHPModels.TabIndex = 0
        '
        'lvHpProducts
        '
        Me.lvHpProducts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader4})
        Me.lvHpProducts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvHpProducts.FullRowSelect = True
        Me.lvHpProducts.Location = New System.Drawing.Point(0, 0)
        Me.lvHpProducts.MultiSelect = False
        Me.lvHpProducts.Name = "lvHpProducts"
        Me.lvHpProducts.Size = New System.Drawing.Size(624, 190)
        Me.lvHpProducts.TabIndex = 1
        Me.lvHpProducts.UseCompatibleStateImageBehavior = False
        Me.lvHpProducts.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Model"
        Me.ColumnHeader3.Width = 363
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Operating System"
        Me.ColumnHeader4.Width = 163
        '
        'pnlLenovoModels
        '
        Me.pnlLenovoModels.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlLenovoModels.Location = New System.Drawing.Point(0, 0)
        Me.pnlLenovoModels.Name = "pnlLenovoModels"
        Me.pnlLenovoModels.Size = New System.Drawing.Size(624, 190)
        Me.pnlLenovoModels.TabIndex = 0
        '
        'pnlDellModels
        '
        Me.pnlDellModels.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlDellModels.Location = New System.Drawing.Point(0, 0)
        Me.pnlDellModels.Name = "pnlDellModels"
        Me.pnlDellModels.Size = New System.Drawing.Size(624, 190)
        Me.pnlDellModels.TabIndex = 0
        '
        'pnlModelDetailContainer
        '
        Me.pnlModelDetailContainer.Controls.Add(Me.lblDrvPackCount)
        Me.pnlModelDetailContainer.Controls.Add(Me.lblSysId)
        Me.pnlModelDetailContainer.Controls.Add(Me.Label10)
        Me.pnlModelDetailContainer.Controls.Add(Me.Label9)
        Me.pnlModelDetailContainer.Controls.Add(Me.lblModelName)
        Me.pnlModelDetailContainer.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlModelDetailContainer.Location = New System.Drawing.Point(0, 218)
        Me.pnlModelDetailContainer.Name = "pnlModelDetailContainer"
        Me.pnlModelDetailContainer.Size = New System.Drawing.Size(624, 128)
        Me.pnlModelDetailContainer.TabIndex = 1
        Me.pnlModelDetailContainer.Visible = False
        '
        'lblDrvPackCount
        '
        Me.lblDrvPackCount.AutoSize = True
        Me.lblDrvPackCount.Location = New System.Drawing.Point(130, 80)
        Me.lblDrvPackCount.Name = "lblDrvPackCount"
        Me.lblDrvPackCount.Size = New System.Drawing.Size(19, 13)
        Me.lblDrvPackCount.TabIndex = 1
        Me.lblDrvPackCount.Text = "    "
        '
        'lblSysId
        '
        Me.lblSysId.AutoSize = True
        Me.lblSysId.Location = New System.Drawing.Point(130, 58)
        Me.lblSysId.Name = "lblSysId"
        Me.lblSysId.Size = New System.Drawing.Size(19, 13)
        Me.lblSysId.TabIndex = 1
        Me.lblSysId.Text = "    "
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(16, 80)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(108, 13)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = "Drivers/Driver Packs:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(16, 58)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(60, 13)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "System ID:"
        '
        'lblModelName
        '
        Me.lblModelName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblModelName.AutoEllipsis = True
        Me.lblModelName.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblModelName.Location = New System.Drawing.Point(12, 12)
        Me.lblModelName.Name = "lblModelName"
        Me.lblModelName.Size = New System.Drawing.Size(596, 42)
        Me.lblModelName.TabIndex = 0
        Me.lblModelName.Text = "Model"
        '
        'pnlModelSearch
        '
        Me.pnlModelSearch.Controls.Add(Me.TextBox1)
        Me.pnlModelSearch.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlModelSearch.Location = New System.Drawing.Point(0, 0)
        Me.pnlModelSearch.Name = "pnlModelSearch"
        Me.pnlModelSearch.Size = New System.Drawing.Size(624, 28)
        Me.pnlModelSearch.TabIndex = 0
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(390, 4)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(231, 21)
        Me.TextBox1.TabIndex = 0
        '
        'Label8
        '
        Me.Label8.AutoEllipsis = True
        Me.Label8.Location = New System.Drawing.Point(60, 80)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(627, 23)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Select the model of your computer from the list below:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(19, 20)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(130, 25)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Select Model"
        '
        'pnlManufacturerSelect
        '
        Me.pnlManufacturerSelect.Controls.Add(Me.btnSelectLenovo)
        Me.pnlManufacturerSelect.Controls.Add(Me.btnSelectDell)
        Me.pnlManufacturerSelect.Controls.Add(Me.btnSelectHP)
        Me.pnlManufacturerSelect.Controls.Add(Me.Label5)
        Me.pnlManufacturerSelect.Controls.Add(Me.Label6)
        Me.pnlManufacturerSelect.Controls.Add(Me.Label22)
        Me.pnlManufacturerSelect.Controls.Add(Me.pbHP)
        Me.pnlManufacturerSelect.Controls.Add(Me.pbDell)
        Me.pnlManufacturerSelect.Controls.Add(Me.pbLenovo)
        Me.pnlManufacturerSelect.Controls.Add(Me.Label23)
        Me.pnlManufacturerSelect.Controls.Add(Me.Label4)
        Me.pnlManufacturerSelect.Controls.Add(Me.Label3)
        Me.pnlManufacturerSelect.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlManufacturerSelect.Location = New System.Drawing.Point(0, 0)
        Me.pnlManufacturerSelect.Name = "pnlManufacturerSelect"
        Me.pnlManufacturerSelect.Size = New System.Drawing.Size(758, 519)
        Me.pnlManufacturerSelect.TabIndex = 0
        '
        'btnSelectLenovo
        '
        Me.btnSelectLenovo.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnSelectLenovo.Location = New System.Drawing.Point(540, 311)
        Me.btnSelectLenovo.Name = "btnSelectLenovo"
        Me.btnSelectLenovo.Size = New System.Drawing.Size(147, 23)
        Me.btnSelectLenovo.TabIndex = 9
        Me.btnSelectLenovo.Text = "Select This Manufacturer"
        Me.btnSelectLenovo.UseVisualStyleBackColor = True
        '
        'btnSelectDell
        '
        Me.btnSelectDell.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnSelectDell.Location = New System.Drawing.Point(540, 241)
        Me.btnSelectDell.Name = "btnSelectDell"
        Me.btnSelectDell.Size = New System.Drawing.Size(147, 23)
        Me.btnSelectDell.TabIndex = 9
        Me.btnSelectDell.Text = "Select This Manufacturer"
        Me.btnSelectDell.UseVisualStyleBackColor = True
        '
        'btnSelectHP
        '
        Me.btnSelectHP.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnSelectHP.Location = New System.Drawing.Point(540, 171)
        Me.btnSelectHP.Name = "btnSelectHP"
        Me.btnSelectHP.Size = New System.Drawing.Size(147, 23)
        Me.btnSelectHP.TabIndex = 9
        Me.btnSelectHP.Text = "Select This Manufacturer"
        Me.btnSelectHP.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(134, 130)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(391, 19)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "HP Client Management Solutions SoftPAQ Lists"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(134, 200)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(370, 19)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "Dell Command | Deploy Driver Pack Catalog"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(134, 270)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(532, 19)
        Me.Label22.TabIndex = 6
        Me.Label22.Text = "Lenovo Commercial Deployment Readiness Team Model Catalog"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'pbHP
        '
        Me.pbHP.Image = Global.DrvPakCreator.My.Resources.Resources.man_hp
        Me.pbHP.Location = New System.Drawing.Point(63, 130)
        Me.pbHP.Name = "pbHP"
        Me.pbHP.Size = New System.Drawing.Size(64, 64)
        Me.pbHP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbHP.TabIndex = 5
        Me.pbHP.TabStop = False
        '
        'pbDell
        '
        Me.pbDell.Image = Global.DrvPakCreator.My.Resources.Resources.man_dell
        Me.pbDell.Location = New System.Drawing.Point(63, 200)
        Me.pbDell.Name = "pbDell"
        Me.pbDell.Size = New System.Drawing.Size(64, 64)
        Me.pbDell.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbDell.TabIndex = 4
        Me.pbDell.TabStop = False
        '
        'pbLenovo
        '
        Me.pbLenovo.Image = Global.DrvPakCreator.My.Resources.Resources.man_lenovo
        Me.pbLenovo.Location = New System.Drawing.Point(63, 270)
        Me.pbLenovo.Name = "pbLenovo"
        Me.pbLenovo.Size = New System.Drawing.Size(64, 64)
        Me.pbLenovo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbLenovo.TabIndex = 3
        Me.pbLenovo.TabStop = False
        '
        'Label23
        '
        Me.Label23.AutoEllipsis = True
        Me.Label23.Location = New System.Drawing.Point(60, 454)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(627, 23)
        Me.Label23.TabIndex = 2
        Me.Label23.Text = "If your computer manufacturer has a model catalog available but is not listed her" & _
    "e, send feedback so we can add it here."
        '
        'Label4
        '
        Me.Label4.AutoEllipsis = True
        Me.Label4.Location = New System.Drawing.Point(60, 80)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(627, 44)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Choose the manufacturer of your device:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(19, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(200, 25)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Select Manufacturer"
        '
        'pnlWelcome
        '
        Me.pnlWelcome.Controls.Add(Me.Label2)
        Me.pnlWelcome.Controls.Add(Me.Label1)
        Me.pnlWelcome.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlWelcome.Location = New System.Drawing.Point(0, 0)
        Me.pnlWelcome.Name = "pnlWelcome"
        Me.pnlWelcome.Size = New System.Drawing.Size(758, 519)
        Me.pnlWelcome.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoEllipsis = True
        Me.Label2.Location = New System.Drawing.Point(60, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(627, 144)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = resources.GetString("Label2.Text")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(19, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 25)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Welcome"
        '
        'AddComputerFromModelCatalogDialog
        '
        Me.AcceptButton = Me.Next_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(1018, 575)
        Me.Controls.Add(Me.pnlWizardStepContainer)
        Me.Controls.Add(Me.flpWizardSteps)
        Me.Controls.Add(Me.pnlButtonContainer)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AddComputerFromModelCatalogDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Computer from Model Catalog"
        Me.pnlButtonContainer.ResumeLayout(False)
        Me.tlpActions.ResumeLayout(False)
        Me.flpWizardSteps.ResumeLayout(False)
        Me.pnlWizardStepContainer.ResumeLayout(False)
        Me.pnlFinish.ResumeLayout(False)
        Me.pnlFinish.PerformLayout()
        Me.pnlDrvSelect.ResumeLayout(False)
        Me.pnlDrvSelect.PerformLayout()
        Me.pnlModelDriverContainer.ResumeLayout(False)
        Me.pnlHPDriverSelect.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.pnlModelSelect.ResumeLayout(False)
        Me.pnlModelSelect.PerformLayout()
        Me.pnlModelSelectorContainer.ResumeLayout(False)
        Me.pnlModelContainer.ResumeLayout(False)
        Me.pnlHPModels.ResumeLayout(False)
        Me.pnlModelDetailContainer.ResumeLayout(False)
        Me.pnlModelDetailContainer.PerformLayout()
        Me.pnlModelSearch.ResumeLayout(False)
        Me.pnlModelSearch.PerformLayout()
        Me.pnlManufacturerSelect.ResumeLayout(False)
        Me.pnlManufacturerSelect.PerformLayout()
        CType(Me.pbHP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbDell, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbLenovo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlWelcome.ResumeLayout(False)
        Me.pnlWelcome.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlButtonContainer As System.Windows.Forms.Panel
    Friend WithEvents flpWizardSteps As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents WizardStep1 As DrvPakCreator.WizardStep
    Friend WithEvents pnlWizardStepContainer As System.Windows.Forms.Panel
    Friend WithEvents WizardStep2 As DrvPakCreator.WizardStep
    Friend WithEvents WizardStep3 As DrvPakCreator.WizardStep
    Friend WithEvents pnlGap As System.Windows.Forms.Panel
    Friend WithEvents WizardStep4 As DrvPakCreator.WizardStep
    Friend WithEvents WizardStep5 As DrvPakCreator.WizardStep
    Friend WithEvents pnlFinish As System.Windows.Forms.Panel
    Friend WithEvents pnlDrvSelect As System.Windows.Forms.Panel
    Friend WithEvents pnlModelSelect As System.Windows.Forms.Panel
    Friend WithEvents pnlManufacturerSelect As System.Windows.Forms.Panel
    Friend WithEvents pnlWelcome As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents tlpActions As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Help_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Next_Button As System.Windows.Forms.Button
    Friend WithEvents Back_Button As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents pbHP As System.Windows.Forms.PictureBox
    Friend WithEvents pbDell As System.Windows.Forms.PictureBox
    Friend WithEvents pbLenovo As System.Windows.Forms.PictureBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents btnSelectLenovo As System.Windows.Forms.Button
    Friend WithEvents btnSelectDell As System.Windows.Forms.Button
    Friend WithEvents btnSelectHP As System.Windows.Forms.Button
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents pnlModelSelectorContainer As System.Windows.Forms.Panel
    Friend WithEvents pnlModelSearch As System.Windows.Forms.Panel
    Friend WithEvents pnlModelDetailContainer As System.Windows.Forms.Panel
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents pnlModelContainer As System.Windows.Forms.Panel
    Friend WithEvents lblModelName As System.Windows.Forms.Label
    Friend WithEvents pnlHPModels As System.Windows.Forms.Panel
    Friend WithEvents pnlLenovoModels As System.Windows.Forms.Panel
    Friend WithEvents pnlDellModels As System.Windows.Forms.Panel
    Friend WithEvents lvHpProducts As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lblSysId As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents lblDrvPackCount As System.Windows.Forms.Label
    Friend WithEvents pnlModelDriverContainer As System.Windows.Forms.Panel
    Friend WithEvents pnlHPDriverSelect As System.Windows.Forms.Panel
    Friend WithEvents lbSelectedDrivers As System.Windows.Forms.ListBox
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents tvSoftPaqSubdirs As System.Windows.Forms.TreeView
    Friend WithEvents lvHPDriverFiles As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents tbComputerAdditionOverview As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label

End Class
