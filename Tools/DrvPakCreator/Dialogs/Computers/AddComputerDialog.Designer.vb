<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddComputerDialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AddComputerDialog))
        Me.pnlButtonContainer = New System.Windows.Forms.Panel
        Me.tlpActions = New System.Windows.Forms.TableLayoutPanel
        Me.Help_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.Next_Button = New System.Windows.Forms.Button
        Me.Back_Button = New System.Windows.Forms.Button
        Me.flpWizardSteps = New System.Windows.Forms.FlowLayoutPanel
        Me.pnlGap = New System.Windows.Forms.Panel
        Me.WizardStep1 = New DrvPakCreator.WizardStep
        Me.WizardStep2 = New DrvPakCreator.WizardStep
        Me.WizardStep3 = New DrvPakCreator.WizardStep
        Me.WizardStep4 = New DrvPakCreator.WizardStep
        Me.pnlWizardStepContainer = New System.Windows.Forms.Panel
        Me.pnlCompInfo = New System.Windows.Forms.Panel
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel
        Me.nudStorage = New System.Windows.Forms.NumericUpDown
        Me.nudMemory = New System.Windows.Forms.NumericUpDown
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.tbSysProcessor = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.tbSysModel = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.tbSysManufacturer = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.tbSysUuid = New System.Windows.Forms.TextBox
        Me.tbSysOSName = New System.Windows.Forms.TextBox
        Me.tbSysOSSku = New System.Windows.Forms.TextBox
        Me.tbSysOSVer = New System.Windows.Forms.TextBox
        Me.btnSysInfo = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.pnlFinish = New System.Windows.Forms.Panel
        Me.tbComputerAdditionOverview = New System.Windows.Forms.TextBox
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label23 = New System.Windows.Forms.Label
        Me.pnlDrvAssoc = New System.Windows.Forms.Panel
        Me.lvDriverFiles = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.pnlWelcome = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.pnlButtonContainer.SuspendLayout()
        Me.tlpActions.SuspendLayout()
        Me.flpWizardSteps.SuspendLayout()
        Me.pnlWizardStepContainer.SuspendLayout()
        Me.pnlCompInfo.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        CType(Me.nudStorage, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudMemory, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlFinish.SuspendLayout()
        Me.pnlDrvAssoc.SuspendLayout()
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
        Me.WizardStep2.TabIndex = 2
        Me.WizardStep2.WizardStepIsActive = False
        Me.WizardStep2.WizardStepName = "Computer Information"
        '
        'WizardStep3
        '
        Me.WizardStep3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WizardStep3.Location = New System.Drawing.Point(3, 70)
        Me.WizardStep3.Name = "WizardStep3"
        Me.WizardStep3.Size = New System.Drawing.Size(256, 24)
        Me.WizardStep3.TabIndex = 2
        Me.WizardStep3.WizardStepIsActive = False
        Me.WizardStep3.WizardStepName = "Associate Drivers"
        '
        'WizardStep4
        '
        Me.WizardStep4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WizardStep4.Location = New System.Drawing.Point(3, 100)
        Me.WizardStep4.Name = "WizardStep4"
        Me.WizardStep4.Size = New System.Drawing.Size(256, 24)
        Me.WizardStep4.TabIndex = 2
        Me.WizardStep4.WizardStepIsActive = False
        Me.WizardStep4.WizardStepName = "Finish"
        '
        'pnlWizardStepContainer
        '
        Me.pnlWizardStepContainer.Controls.Add(Me.pnlCompInfo)
        Me.pnlWizardStepContainer.Controls.Add(Me.pnlFinish)
        Me.pnlWizardStepContainer.Controls.Add(Me.pnlDrvAssoc)
        Me.pnlWizardStepContainer.Controls.Add(Me.pnlWelcome)
        Me.pnlWizardStepContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlWizardStepContainer.Location = New System.Drawing.Point(260, 0)
        Me.pnlWizardStepContainer.Name = "pnlWizardStepContainer"
        Me.pnlWizardStepContainer.Size = New System.Drawing.Size(758, 519)
        Me.pnlWizardStepContainer.TabIndex = 3
        '
        'pnlCompInfo
        '
        Me.pnlCompInfo.Controls.Add(Me.TableLayoutPanel2)
        Me.pnlCompInfo.Controls.Add(Me.btnSysInfo)
        Me.pnlCompInfo.Controls.Add(Me.Label8)
        Me.pnlCompInfo.Controls.Add(Me.Label7)
        Me.pnlCompInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlCompInfo.Location = New System.Drawing.Point(0, 0)
        Me.pnlCompInfo.Name = "pnlCompInfo"
        Me.pnlCompInfo.Size = New System.Drawing.Size(758, 519)
        Me.pnlCompInfo.TabIndex = 0
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 3
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 37.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.nudStorage, 1, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.nudMemory, 1, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.Label15, 2, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.Label14, 2, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.Label11, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.tbSysProcessor, 1, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.Label10, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.tbSysModel, 1, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Label9, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.tbSysManufacturer, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label12, 0, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.Label13, 0, 4)
        Me.TableLayoutPanel2.Controls.Add(Me.Label16, 0, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.Label17, 0, 6)
        Me.TableLayoutPanel2.Controls.Add(Me.Label18, 0, 7)
        Me.TableLayoutPanel2.Controls.Add(Me.Label19, 0, 8)
        Me.TableLayoutPanel2.Controls.Add(Me.tbSysUuid, 1, 5)
        Me.TableLayoutPanel2.Controls.Add(Me.tbSysOSName, 1, 6)
        Me.TableLayoutPanel2.Controls.Add(Me.tbSysOSSku, 1, 7)
        Me.TableLayoutPanel2.Controls.Add(Me.tbSysOSVer, 1, 8)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(137, 176)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 9
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(485, 226)
        Me.TableLayoutPanel2.TabIndex = 4
        '
        'nudStorage
        '
        Me.nudStorage.Dock = System.Windows.Forms.DockStyle.Fill
        Me.nudStorage.Location = New System.Drawing.Point(115, 103)
        Me.nudStorage.Maximum = New Decimal(New Integer() {-1, -1, 0, 0})
        Me.nudStorage.Minimum = New Decimal(New Integer() {16, 0, 0, 0})
        Me.nudStorage.Name = "nudStorage"
        Me.nudStorage.Size = New System.Drawing.Size(330, 21)
        Me.nudStorage.TabIndex = 6
        Me.nudStorage.Value = New Decimal(New Integer() {16, 0, 0, 0})
        '
        'nudMemory
        '
        Me.nudMemory.Dock = System.Windows.Forms.DockStyle.Fill
        Me.nudMemory.Location = New System.Drawing.Point(115, 78)
        Me.nudMemory.Maximum = New Decimal(New Integer() {-1, -1, 0, 0})
        Me.nudMemory.Minimum = New Decimal(New Integer() {16, 0, 0, 0})
        Me.nudMemory.Name = "nudMemory"
        Me.nudMemory.Size = New System.Drawing.Size(330, 21)
        Me.nudMemory.TabIndex = 6
        Me.nudMemory.Value = New Decimal(New Integer() {16, 0, 0, 0})
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label15.Location = New System.Drawing.Point(451, 100)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(31, 25)
        Me.Label15.TabIndex = 7
        Me.Label15.Text = "MB"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label14.Location = New System.Drawing.Point(451, 75)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(31, 25)
        Me.Label14.TabIndex = 7
        Me.Label14.Text = "MB"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label11.Location = New System.Drawing.Point(3, 50)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(106, 25)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Processor:"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSysProcessor
        '
        Me.TableLayoutPanel2.SetColumnSpan(Me.tbSysProcessor, 2)
        Me.tbSysProcessor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbSysProcessor.Location = New System.Drawing.Point(115, 53)
        Me.tbSysProcessor.Name = "tbSysProcessor"
        Me.tbSysProcessor.Size = New System.Drawing.Size(367, 21)
        Me.tbSysProcessor.TabIndex = 5
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label10.Location = New System.Drawing.Point(3, 25)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(106, 25)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "Model:"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSysModel
        '
        Me.TableLayoutPanel2.SetColumnSpan(Me.tbSysModel, 2)
        Me.tbSysModel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbSysModel.Location = New System.Drawing.Point(115, 28)
        Me.tbSysModel.Name = "tbSysModel"
        Me.tbSysModel.Size = New System.Drawing.Size(367, 21)
        Me.tbSysModel.TabIndex = 3
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label9.Location = New System.Drawing.Point(3, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(106, 25)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Manufacturer:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSysManufacturer
        '
        Me.TableLayoutPanel2.SetColumnSpan(Me.tbSysManufacturer, 2)
        Me.tbSysManufacturer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbSysManufacturer.Location = New System.Drawing.Point(115, 3)
        Me.tbSysManufacturer.Name = "tbSysManufacturer"
        Me.tbSysManufacturer.Size = New System.Drawing.Size(367, 21)
        Me.tbSysManufacturer.TabIndex = 1
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label12.Location = New System.Drawing.Point(3, 75)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(106, 25)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "Memory:"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label13.Location = New System.Drawing.Point(3, 100)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(106, 25)
        Me.Label13.TabIndex = 4
        Me.Label13.Text = "Storage:"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label16.Location = New System.Drawing.Point(3, 125)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(106, 25)
        Me.Label16.TabIndex = 9
        Me.Label16.Text = "UUID (optional):"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label17.Location = New System.Drawing.Point(3, 150)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(106, 25)
        Me.Label17.TabIndex = 9
        Me.Label17.Text = "OS Name:"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label18.Location = New System.Drawing.Point(3, 175)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(106, 25)
        Me.Label18.TabIndex = 9
        Me.Label18.Text = "Edition:"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label19.Location = New System.Drawing.Point(3, 200)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(106, 26)
        Me.Label19.TabIndex = 9
        Me.Label19.Text = "OS Version:"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbSysUuid
        '
        Me.TableLayoutPanel2.SetColumnSpan(Me.tbSysUuid, 2)
        Me.tbSysUuid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbSysUuid.Location = New System.Drawing.Point(115, 128)
        Me.tbSysUuid.MaxLength = 36
        Me.tbSysUuid.Name = "tbSysUuid"
        Me.tbSysUuid.Size = New System.Drawing.Size(367, 21)
        Me.tbSysUuid.TabIndex = 5
        '
        'tbSysOSName
        '
        Me.TableLayoutPanel2.SetColumnSpan(Me.tbSysOSName, 2)
        Me.tbSysOSName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbSysOSName.Location = New System.Drawing.Point(115, 153)
        Me.tbSysOSName.Name = "tbSysOSName"
        Me.tbSysOSName.Size = New System.Drawing.Size(367, 21)
        Me.tbSysOSName.TabIndex = 5
        '
        'tbSysOSSku
        '
        Me.TableLayoutPanel2.SetColumnSpan(Me.tbSysOSSku, 2)
        Me.tbSysOSSku.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbSysOSSku.Location = New System.Drawing.Point(115, 178)
        Me.tbSysOSSku.Name = "tbSysOSSku"
        Me.tbSysOSSku.Size = New System.Drawing.Size(367, 21)
        Me.tbSysOSSku.TabIndex = 5
        '
        'tbSysOSVer
        '
        Me.TableLayoutPanel2.SetColumnSpan(Me.tbSysOSVer, 2)
        Me.tbSysOSVer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbSysOSVer.Location = New System.Drawing.Point(115, 203)
        Me.tbSysOSVer.Name = "tbSysOSVer"
        Me.tbSysOSVer.Size = New System.Drawing.Size(367, 21)
        Me.tbSysOSVer.TabIndex = 5
        '
        'btnSysInfo
        '
        Me.btnSysInfo.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnSysInfo.Location = New System.Drawing.Point(495, 428)
        Me.btnSysInfo.Name = "btnSysInfo"
        Me.btnSysInfo.Size = New System.Drawing.Size(192, 23)
        Me.btnSysInfo.TabIndex = 3
        Me.btnSysInfo.Text = "Use Current System Information"
        Me.btnSysInfo.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoEllipsis = True
        Me.Label8.Location = New System.Drawing.Point(60, 80)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(627, 74)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = resources.GetString("Label8.Text")
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(19, 20)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(369, 25)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Enter information about the computer"
        '
        'pnlFinish
        '
        Me.pnlFinish.Controls.Add(Me.tbComputerAdditionOverview)
        Me.pnlFinish.Controls.Add(Me.Label22)
        Me.pnlFinish.Controls.Add(Me.Label23)
        Me.pnlFinish.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlFinish.Location = New System.Drawing.Point(0, 0)
        Me.pnlFinish.Name = "pnlFinish"
        Me.pnlFinish.Size = New System.Drawing.Size(758, 519)
        Me.pnlFinish.TabIndex = 0
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
        Me.tbComputerAdditionOverview.TabIndex = 6
        '
        'Label22
        '
        Me.Label22.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label22.AutoEllipsis = True
        Me.Label22.Location = New System.Drawing.Point(60, 80)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(627, 32)
        Me.Label22.TabIndex = 4
        Me.Label22.Text = "Click Finish to begin adding this computer."
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(19, 20)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(200, 25)
        Me.Label23.TabIndex = 3
        Me.Label23.Text = "Review your options"
        '
        'pnlDrvAssoc
        '
        Me.pnlDrvAssoc.Controls.Add(Me.lvDriverFiles)
        Me.pnlDrvAssoc.Controls.Add(Me.Label20)
        Me.pnlDrvAssoc.Controls.Add(Me.Label21)
        Me.pnlDrvAssoc.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlDrvAssoc.Location = New System.Drawing.Point(0, 0)
        Me.pnlDrvAssoc.Name = "pnlDrvAssoc"
        Me.pnlDrvAssoc.Size = New System.Drawing.Size(758, 519)
        Me.pnlDrvAssoc.TabIndex = 0
        '
        'lvDriverFiles
        '
        Me.lvDriverFiles.CheckBoxes = True
        Me.lvDriverFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvDriverFiles.FullRowSelect = True
        Me.lvDriverFiles.Location = New System.Drawing.Point(63, 157)
        Me.lvDriverFiles.Name = "lvDriverFiles"
        Me.lvDriverFiles.Size = New System.Drawing.Size(624, 319)
        Me.lvDriverFiles.TabIndex = 5
        Me.lvDriverFiles.UseCompatibleStateImageBehavior = False
        Me.lvDriverFiles.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 256
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Driver File"
        Me.ColumnHeader2.Width = 256
        '
        'Label20
        '
        Me.Label20.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label20.AutoEllipsis = True
        Me.Label20.Location = New System.Drawing.Point(60, 80)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(627, 74)
        Me.Label20.TabIndex = 4
        Me.Label20.Text = resources.GetString("Label20.Text")
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(19, 20)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(354, 25)
        Me.Label21.TabIndex = 3
        Me.Label21.Text = "What drivers go with this computer?"
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
        Me.Label2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
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
        'AddComputerDialog
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
        Me.Name = "AddComputerDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Computer"
        Me.pnlButtonContainer.ResumeLayout(False)
        Me.tlpActions.ResumeLayout(False)
        Me.flpWizardSteps.ResumeLayout(False)
        Me.pnlWizardStepContainer.ResumeLayout(False)
        Me.pnlCompInfo.ResumeLayout(False)
        Me.pnlCompInfo.PerformLayout()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        CType(Me.nudStorage, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudMemory, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlFinish.ResumeLayout(False)
        Me.pnlFinish.PerformLayout()
        Me.pnlDrvAssoc.ResumeLayout(False)
        Me.pnlDrvAssoc.PerformLayout()
        Me.pnlWelcome.ResumeLayout(False)
        Me.pnlWelcome.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlButtonContainer As System.Windows.Forms.Panel
    Friend WithEvents flpWizardSteps As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents WizardStep1 As DrvPakCreator.WizardStep
    Friend WithEvents pnlWizardStepContainer As System.Windows.Forms.Panel
    Friend WithEvents WizardStep2 As DrvPakCreator.WizardStep
    Friend WithEvents pnlGap As System.Windows.Forms.Panel
    Friend WithEvents WizardStep3 As DrvPakCreator.WizardStep
    Friend WithEvents WizardStep4 As DrvPakCreator.WizardStep
    Friend WithEvents pnlFinish As System.Windows.Forms.Panel
    Friend WithEvents pnlDrvAssoc As System.Windows.Forms.Panel
    Friend WithEvents pnlCompInfo As System.Windows.Forms.Panel
    Friend WithEvents pnlWelcome As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tlpActions As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Help_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Next_Button As System.Windows.Forms.Button
    Friend WithEvents Back_Button As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnSysInfo As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents tbSysProcessor As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tbSysModel As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents tbSysManufacturer As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents nudStorage As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents nudMemory As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents tbSysUuid As System.Windows.Forms.TextBox
    Friend WithEvents tbSysOSName As System.Windows.Forms.TextBox
    Friend WithEvents tbSysOSSku As System.Windows.Forms.TextBox
    Friend WithEvents tbSysOSVer As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents lvDriverFiles As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents tbComputerAdditionOverview As System.Windows.Forms.TextBox

End Class
