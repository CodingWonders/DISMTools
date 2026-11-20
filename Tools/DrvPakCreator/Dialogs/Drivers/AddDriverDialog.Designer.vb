<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddDriverDialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AddDriverDialog))
        Me.pnlButtonContainer = New System.Windows.Forms.Panel
        Me.tlpActions = New System.Windows.Forms.TableLayoutPanel
        Me.Help_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.Next_Button = New System.Windows.Forms.Button
        Me.Back_Button = New System.Windows.Forms.Button
        Me.pnlWelcome = New System.Windows.Forms.Panel
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.pnlDriverSource = New System.Windows.Forms.Panel
        Me.lbDriverFiles = New System.Windows.Forms.ListBox
        Me.btnBrowseDriverSource = New System.Windows.Forms.Button
        Me.tbDriverSource = New System.Windows.Forms.TextBox
        Me.lblScanStatus = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.pnlFinish = New System.Windows.Forms.Panel
        Me.tbAdditionOverview = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.pnlGap = New System.Windows.Forms.Panel
        Me.flpWizardSteps = New System.Windows.Forms.FlowLayoutPanel
        Me.WizardStep1 = New DrvPakCreator.WizardStep
        Me.WizardStep2 = New DrvPakCreator.WizardStep
        Me.WizardStep3 = New DrvPakCreator.WizardStep
        Me.WizardStep4 = New DrvPakCreator.WizardStep
        Me.pnlWizardStepContainer = New System.Windows.Forms.Panel
        Me.pnlDrvInfo = New System.Windows.Forms.Panel
        Me.tlpManualDriverInfo = New System.Windows.Forms.TableLayoutPanel
        Me.Label10 = New System.Windows.Forms.Label
        Me.tbManualDriverManufacturer = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.tbManualDriverName = New System.Windows.Forms.TextBox
        Me.rbAutoDriverInfo = New System.Windows.Forms.RadioButton
        Me.rbManualDriverInfo = New System.Windows.Forms.RadioButton
        Me.pnlHwTargetChooser = New System.Windows.Forms.Panel
        Me.lvHwTargets = New System.Windows.Forms.ListView
        Me.chHwTargets = New System.Windows.Forms.ColumnHeader
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.tbAutoDriverManufacturer = New System.Windows.Forms.TextBox
        Me.tbAutoDriverName = New System.Windows.Forms.TextBox
        Me.fbdDriverSource = New System.Windows.Forms.FolderBrowserDialog
        Me.bwDirScanner = New System.ComponentModel.BackgroundWorker
        Me.pnlButtonContainer.SuspendLayout()
        Me.tlpActions.SuspendLayout()
        Me.pnlWelcome.SuspendLayout()
        Me.pnlDriverSource.SuspendLayout()
        Me.pnlFinish.SuspendLayout()
        Me.flpWizardSteps.SuspendLayout()
        Me.pnlWizardStepContainer.SuspendLayout()
        Me.pnlDrvInfo.SuspendLayout()
        Me.tlpManualDriverInfo.SuspendLayout()
        Me.pnlHwTargetChooser.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlButtonContainer
        '
        Me.pnlButtonContainer.Controls.Add(Me.tlpActions)
        Me.pnlButtonContainer.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlButtonContainer.Location = New System.Drawing.Point(0, 519)
        Me.pnlButtonContainer.Name = "pnlButtonContainer"
        Me.pnlButtonContainer.Size = New System.Drawing.Size(1018, 56)
        Me.pnlButtonContainer.TabIndex = 4
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
        'pnlWelcome
        '
        Me.pnlWelcome.Controls.Add(Me.Button1)
        Me.pnlWelcome.Controls.Add(Me.Label9)
        Me.pnlWelcome.Controls.Add(Me.Label2)
        Me.pnlWelcome.Controls.Add(Me.Label1)
        Me.pnlWelcome.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlWelcome.Location = New System.Drawing.Point(0, 0)
        Me.pnlWelcome.Name = "pnlWelcome"
        Me.pnlWelcome.Size = New System.Drawing.Size(758, 519)
        Me.pnlWelcome.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.Button1.Location = New System.Drawing.Point(63, 472)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(168, 23)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Add Multiple Drivers"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoEllipsis = True
        Me.Label9.Location = New System.Drawing.Point(60, 440)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(627, 28)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "If you need to add drivers from multiple sources at once, you can add driver file" & _
            "s in bulk."
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
        'pnlDriverSource
        '
        Me.pnlDriverSource.Controls.Add(Me.lbDriverFiles)
        Me.pnlDriverSource.Controls.Add(Me.btnBrowseDriverSource)
        Me.pnlDriverSource.Controls.Add(Me.tbDriverSource)
        Me.pnlDriverSource.Controls.Add(Me.lblScanStatus)
        Me.pnlDriverSource.Controls.Add(Me.Label6)
        Me.pnlDriverSource.Controls.Add(Me.Label5)
        Me.pnlDriverSource.Controls.Add(Me.Label4)
        Me.pnlDriverSource.Controls.Add(Me.Label3)
        Me.pnlDriverSource.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlDriverSource.Location = New System.Drawing.Point(0, 0)
        Me.pnlDriverSource.Name = "pnlDriverSource"
        Me.pnlDriverSource.Size = New System.Drawing.Size(758, 519)
        Me.pnlDriverSource.TabIndex = 0
        '
        'lbDriverFiles
        '
        Me.lbDriverFiles.FormattingEnabled = True
        Me.lbDriverFiles.Location = New System.Drawing.Point(63, 248)
        Me.lbDriverFiles.Name = "lbDriverFiles"
        Me.lbDriverFiles.ScrollAlwaysVisible = True
        Me.lbDriverFiles.Size = New System.Drawing.Size(623, 199)
        Me.lbDriverFiles.TabIndex = 6
        '
        'btnBrowseDriverSource
        '
        Me.btnBrowseDriverSource.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.btnBrowseDriverSource.Location = New System.Drawing.Point(611, 189)
        Me.btnBrowseDriverSource.Name = "btnBrowseDriverSource"
        Me.btnBrowseDriverSource.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseDriverSource.TabIndex = 5
        Me.btnBrowseDriverSource.Text = "Browse..."
        Me.btnBrowseDriverSource.UseVisualStyleBackColor = True
        '
        'tbDriverSource
        '
        Me.tbDriverSource.Location = New System.Drawing.Point(63, 189)
        Me.tbDriverSource.Name = "tbDriverSource"
        Me.tbDriverSource.Size = New System.Drawing.Size(542, 21)
        Me.tbDriverSource.TabIndex = 4
        '
        'lblScanStatus
        '
        Me.lblScanStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblScanStatus.AutoEllipsis = True
        Me.lblScanStatus.Location = New System.Drawing.Point(60, 459)
        Me.lblScanStatus.Name = "lblScanStatus"
        Me.lblScanStatus.Size = New System.Drawing.Size(627, 57)
        Me.lblScanStatus.TabIndex = 3
        Me.lblScanStatus.Text = "Ready to scan"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(60, 224)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(230, 13)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Choose the main driver file from the list below:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(60, 172)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(174, 13)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "Copy driver files from this location:"
        '
        'Label4
        '
        Me.Label4.AutoEllipsis = True
        Me.Label4.Location = New System.Drawing.Point(60, 80)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(627, 74)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = resources.GetString("Label4.Text")
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(19, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(336, 25)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Where are the driver files located?"
        '
        'pnlFinish
        '
        Me.pnlFinish.Controls.Add(Me.tbAdditionOverview)
        Me.pnlFinish.Controls.Add(Me.Label11)
        Me.pnlFinish.Controls.Add(Me.Label13)
        Me.pnlFinish.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlFinish.Location = New System.Drawing.Point(0, 0)
        Me.pnlFinish.Name = "pnlFinish"
        Me.pnlFinish.Size = New System.Drawing.Size(758, 519)
        Me.pnlFinish.TabIndex = 0
        '
        'tbAdditionOverview
        '
        Me.tbAdditionOverview.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbAdditionOverview.Location = New System.Drawing.Point(63, 142)
        Me.tbAdditionOverview.Multiline = True
        Me.tbAdditionOverview.Name = "tbAdditionOverview"
        Me.tbAdditionOverview.ReadOnly = True
        Me.tbAdditionOverview.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tbAdditionOverview.Size = New System.Drawing.Size(624, 329)
        Me.tbAdditionOverview.TabIndex = 5
        '
        'Label11
        '
        Me.Label11.AutoEllipsis = True
        Me.Label11.Location = New System.Drawing.Point(60, 80)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(627, 74)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "When you click OK, the driver files from the source location will be copied to yo" & _
            "ur Driver Pack folder. Review the following information for any information you " & _
            "might want to change:"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(19, 20)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(242, 25)
        Me.Label13.TabIndex = 3
        Me.Label13.Text = "Finishing Driver Addition"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(19, 20)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(334, 25)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Enter information about the driver"
        '
        'Label8
        '
        Me.Label8.AutoEllipsis = True
        Me.Label8.Location = New System.Drawing.Point(60, 80)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(627, 74)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = resources.GetString("Label8.Text")
        '
        'pnlGap
        '
        Me.pnlGap.Location = New System.Drawing.Point(3, 3)
        Me.pnlGap.Name = "pnlGap"
        Me.pnlGap.Size = New System.Drawing.Size(200, 1)
        Me.pnlGap.TabIndex = 3
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
        Me.flpWizardSteps.TabIndex = 5
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
        Me.WizardStep2.WizardStepName = "Driver Source"
        '
        'WizardStep3
        '
        Me.WizardStep3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WizardStep3.Location = New System.Drawing.Point(3, 70)
        Me.WizardStep3.Name = "WizardStep3"
        Me.WizardStep3.Size = New System.Drawing.Size(256, 24)
        Me.WizardStep3.TabIndex = 2
        Me.WizardStep3.WizardStepIsActive = False
        Me.WizardStep3.WizardStepName = "Identify Driver"
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
        Me.pnlWizardStepContainer.Controls.Add(Me.pnlFinish)
        Me.pnlWizardStepContainer.Controls.Add(Me.pnlDrvInfo)
        Me.pnlWizardStepContainer.Controls.Add(Me.pnlDriverSource)
        Me.pnlWizardStepContainer.Controls.Add(Me.pnlWelcome)
        Me.pnlWizardStepContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlWizardStepContainer.Location = New System.Drawing.Point(260, 0)
        Me.pnlWizardStepContainer.Name = "pnlWizardStepContainer"
        Me.pnlWizardStepContainer.Size = New System.Drawing.Size(758, 519)
        Me.pnlWizardStepContainer.TabIndex = 6
        '
        'pnlDrvInfo
        '
        Me.pnlDrvInfo.Controls.Add(Me.tlpManualDriverInfo)
        Me.pnlDrvInfo.Controls.Add(Me.rbAutoDriverInfo)
        Me.pnlDrvInfo.Controls.Add(Me.rbManualDriverInfo)
        Me.pnlDrvInfo.Controls.Add(Me.Label8)
        Me.pnlDrvInfo.Controls.Add(Me.Label7)
        Me.pnlDrvInfo.Controls.Add(Me.pnlHwTargetChooser)
        Me.pnlDrvInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlDrvInfo.Location = New System.Drawing.Point(0, 0)
        Me.pnlDrvInfo.Name = "pnlDrvInfo"
        Me.pnlDrvInfo.Size = New System.Drawing.Size(758, 519)
        Me.pnlDrvInfo.TabIndex = 0
        '
        'tlpManualDriverInfo
        '
        Me.tlpManualDriverInfo.ColumnCount = 2
        Me.tlpManualDriverInfo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.39175!))
        Me.tlpManualDriverInfo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.60825!))
        Me.tlpManualDriverInfo.Controls.Add(Me.Label10, 0, 1)
        Me.tlpManualDriverInfo.Controls.Add(Me.tbManualDriverManufacturer, 1, 1)
        Me.tlpManualDriverInfo.Controls.Add(Me.Label12, 0, 0)
        Me.tlpManualDriverInfo.Controls.Add(Me.tbManualDriverName, 1, 0)
        Me.tlpManualDriverInfo.Location = New System.Drawing.Point(137, 191)
        Me.tlpManualDriverInfo.Name = "tlpManualDriverInfo"
        Me.tlpManualDriverInfo.RowCount = 2
        Me.tlpManualDriverInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.tlpManualDriverInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11111!))
        Me.tlpManualDriverInfo.Size = New System.Drawing.Size(485, 51)
        Me.tlpManualDriverInfo.TabIndex = 5
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label10.Location = New System.Drawing.Point(3, 25)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(121, 26)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "Manufacturer/Vendor:"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbManualDriverManufacturer
        '
        Me.tbManualDriverManufacturer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbManualDriverManufacturer.Location = New System.Drawing.Point(130, 28)
        Me.tbManualDriverManufacturer.Name = "tbManualDriverManufacturer"
        Me.tbManualDriverManufacturer.Size = New System.Drawing.Size(352, 21)
        Me.tbManualDriverManufacturer.TabIndex = 3
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label12.Location = New System.Drawing.Point(3, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(121, 25)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Name:"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbManualDriverName
        '
        Me.tbManualDriverName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbManualDriverName.Location = New System.Drawing.Point(130, 3)
        Me.tbManualDriverName.Name = "tbManualDriverName"
        Me.tbManualDriverName.Size = New System.Drawing.Size(352, 21)
        Me.tbManualDriverName.TabIndex = 1
        '
        'rbAutoDriverInfo
        '
        Me.rbAutoDriverInfo.AutoSize = True
        Me.rbAutoDriverInfo.Location = New System.Drawing.Point(64, 248)
        Me.rbAutoDriverInfo.Name = "rbAutoDriverInfo"
        Me.rbAutoDriverInfo.Size = New System.Drawing.Size(212, 17)
        Me.rbAutoDriverInfo.TabIndex = 3
        Me.rbAutoDriverInfo.Text = "Use information from hardware targets"
        Me.rbAutoDriverInfo.UseVisualStyleBackColor = True
        '
        'rbManualDriverInfo
        '
        Me.rbManualDriverInfo.AutoSize = True
        Me.rbManualDriverInfo.Checked = True
        Me.rbManualDriverInfo.Location = New System.Drawing.Point(64, 168)
        Me.rbManualDriverInfo.Name = "rbManualDriverInfo"
        Me.rbManualDriverInfo.Size = New System.Drawing.Size(188, 17)
        Me.rbManualDriverInfo.TabIndex = 3
        Me.rbManualDriverInfo.TabStop = True
        Me.rbManualDriverInfo.Text = "Enter driver information manually:"
        Me.rbManualDriverInfo.UseVisualStyleBackColor = True
        '
        'pnlHwTargetChooser
        '
        Me.pnlHwTargetChooser.Controls.Add(Me.lvHwTargets)
        Me.pnlHwTargetChooser.Controls.Add(Me.Label15)
        Me.pnlHwTargetChooser.Controls.Add(Me.Label14)
        Me.pnlHwTargetChooser.Controls.Add(Me.tbAutoDriverManufacturer)
        Me.pnlHwTargetChooser.Controls.Add(Me.tbAutoDriverName)
        Me.pnlHwTargetChooser.Enabled = False
        Me.pnlHwTargetChooser.Location = New System.Drawing.Point(85, 272)
        Me.pnlHwTargetChooser.Name = "pnlHwTargetChooser"
        Me.pnlHwTargetChooser.Size = New System.Drawing.Size(618, 223)
        Me.pnlHwTargetChooser.TabIndex = 6
        '
        'lvHwTargets
        '
        Me.lvHwTargets.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.chHwTargets})
        Me.lvHwTargets.FullRowSelect = True
        Me.lvHwTargets.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvHwTargets.Location = New System.Drawing.Point(12, 13)
        Me.lvHwTargets.MultiSelect = False
        Me.lvHwTargets.Name = "lvHwTargets"
        Me.lvHwTargets.Size = New System.Drawing.Size(322, 186)
        Me.lvHwTargets.TabIndex = 4
        Me.lvHwTargets.UseCompatibleStateImageBehavior = False
        Me.lvHwTargets.View = System.Windows.Forms.View.Details
        '
        'chHwTargets
        '
        Me.chHwTargets.Text = "Driver"
        Me.chHwTargets.Width = 1024
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(340, 14)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(38, 13)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "Name:"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(340, 113)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(114, 13)
        Me.Label14.TabIndex = 2
        Me.Label14.Text = "Manufacturer/Vendor:"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbAutoDriverManufacturer
        '
        Me.tbAutoDriverManufacturer.Location = New System.Drawing.Point(343, 132)
        Me.tbAutoDriverManufacturer.Multiline = True
        Me.tbAutoDriverManufacturer.Name = "tbAutoDriverManufacturer"
        Me.tbAutoDriverManufacturer.ReadOnly = True
        Me.tbAutoDriverManufacturer.Size = New System.Drawing.Size(253, 68)
        Me.tbAutoDriverManufacturer.TabIndex = 3
        '
        'tbAutoDriverName
        '
        Me.tbAutoDriverName.Location = New System.Drawing.Point(343, 30)
        Me.tbAutoDriverName.Multiline = True
        Me.tbAutoDriverName.Name = "tbAutoDriverName"
        Me.tbAutoDriverName.ReadOnly = True
        Me.tbAutoDriverName.Size = New System.Drawing.Size(253, 76)
        Me.tbAutoDriverName.TabIndex = 1
        '
        'fbdDriverSource
        '
        Me.fbdDriverSource.Description = "Specify a driver source:"
        Me.fbdDriverSource.RootFolder = System.Environment.SpecialFolder.MyComputer
        Me.fbdDriverSource.ShowNewFolderButton = False
        '
        'bwDirScanner
        '
        '
        'AddDriverDialog
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
        Me.Name = "AddDriverDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Device Driver"
        Me.pnlButtonContainer.ResumeLayout(False)
        Me.tlpActions.ResumeLayout(False)
        Me.pnlWelcome.ResumeLayout(False)
        Me.pnlWelcome.PerformLayout()
        Me.pnlDriverSource.ResumeLayout(False)
        Me.pnlDriverSource.PerformLayout()
        Me.pnlFinish.ResumeLayout(False)
        Me.pnlFinish.PerformLayout()
        Me.flpWizardSteps.ResumeLayout(False)
        Me.pnlWizardStepContainer.ResumeLayout(False)
        Me.pnlDrvInfo.ResumeLayout(False)
        Me.pnlDrvInfo.PerformLayout()
        Me.tlpManualDriverInfo.ResumeLayout(False)
        Me.tlpManualDriverInfo.PerformLayout()
        Me.pnlHwTargetChooser.ResumeLayout(False)
        Me.pnlHwTargetChooser.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlButtonContainer As System.Windows.Forms.Panel
    Friend WithEvents tlpActions As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Help_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Next_Button As System.Windows.Forms.Button
    Friend WithEvents Back_Button As System.Windows.Forms.Button
    Friend WithEvents pnlWelcome As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pnlDriverSource As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents WizardStep1 As DrvPakCreator.WizardStep
    Friend WithEvents WizardStep2 As DrvPakCreator.WizardStep
    Friend WithEvents WizardStep3 As DrvPakCreator.WizardStep
    Friend WithEvents pnlFinish As System.Windows.Forms.Panel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents pnlGap As System.Windows.Forms.Panel
    Friend WithEvents flpWizardSteps As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents WizardStep4 As DrvPakCreator.WizardStep
    Friend WithEvents pnlWizardStepContainer As System.Windows.Forms.Panel
    Friend WithEvents pnlDrvInfo As System.Windows.Forms.Panel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents btnBrowseDriverSource As System.Windows.Forms.Button
    Friend WithEvents tbDriverSource As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lbDriverFiles As System.Windows.Forms.ListBox
    Friend WithEvents rbManualDriverInfo As System.Windows.Forms.RadioButton
    Friend WithEvents rbAutoDriverInfo As System.Windows.Forms.RadioButton
    Friend WithEvents tlpManualDriverInfo As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents tbManualDriverManufacturer As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents tbManualDriverName As System.Windows.Forms.TextBox
    Friend WithEvents pnlHwTargetChooser As System.Windows.Forms.Panel
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents tbAutoDriverManufacturer As System.Windows.Forms.TextBox
    Friend WithEvents tbAutoDriverName As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents fbdDriverSource As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents bwDirScanner As System.ComponentModel.BackgroundWorker
    Friend WithEvents lblScanStatus As System.Windows.Forms.Label
    Friend WithEvents lvHwTargets As System.Windows.Forms.ListView
    Friend WithEvents chHwTargets As System.Windows.Forms.ColumnHeader
    Friend WithEvents tbAdditionOverview As System.Windows.Forms.TextBox

End Class
