Imports System

' Runtime localization is intentionally kept outside Windows Forms designer files.
' English design-time text remains available to the Visual Studio form designer.

Partial Class AIResults

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("StarterScript.Designer.AIResults")("Results.Message")
        Me.Label2.Text = LocalizationService.ForSection("StarterScript.Designer.AIResults")("Accuracy.Warning")
        Me.DataGridViewImageColumn1.HeaderText = LocalizationService.ForSection("StarterScript.Designer.AIResults")("Severity.Column")
        Me.CheckBox1.Text = LocalizationService.ForSection("StarterScript.Designer.AIResults")("PinTop.CheckBox")
        Me.lblViolationCount.Text = LocalizationService.ForSection("StarterScript.Designer.AIResults")("ViolationCount.Label")
        Me.cbHighViolations.Text = LocalizationService.ForSection("StarterScript.Designer.AIResults")("HighViolations.CheckBox")
        Me.cbLowViolations.Text = LocalizationService.ForSection("StarterScript.Designer.AIResults")("LowViolations.CheckBox")
        Me.cbMediumViolations.Text = LocalizationService.ForSection("StarterScript.Designer.AIResults")("MediumViolations.CheckBox")
        Me.CustomRulesButton.Text = LocalizationService.ForSection("StarterScript.Designer.AIResults")("CustomRules.Button")
        Me.ScannedRuleSeverityColumn.HeaderText = LocalizationService.ForSection("StarterScript.Designer.AIResults")("Severity.Column")
        Me.ScannedRuleDataGridViewTextBoxColumn.HeaderText = LocalizationService.ForSection("StarterScript.Designer.AIResults")("ScannedRule.Column")
        Me.Text = LocalizationService.ForSection("StarterScript.Designer.AIResults")("Title")
    End Sub

End Class

Partial Class AIResultWindowPinDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("StarterScript.Designer.PinDialog")("ChoosePosition.Message")
        Me.PMDetailLabel.Text = LocalizationService.ForSection("StarterScript.Designer.PinDialog")("Monitor.Label")
        Me.Cancel_Button.Text = LocalizationService.ForSection("StarterScript.Designer.PinDialog")("ManualPosition.Button")
        Me.Text = LocalizationService.ForSection("StarterScript.Designer.PinDialog")("Title")
    End Sub

End Class

Partial Class ApiKeyGenerationStepsWizard

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Close_Button.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("Close.Button")
        Me.TabPage1.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("ClassicPat.Tab")
        Me.Label10.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("ProvideTokenInfo.Label")
        Me.Label14.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("ClassicPatDetails.Message")
        Me.Label11.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("WorkWithKey.Label")
        Me.Label12.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("UseGeneratedKey.Message")
        Me.TabPage2.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("FineGrainedPat.Tab")
        Me.Label16.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("ProvideTokenInfo.Label")
        Me.Label17.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("FineGrainedDetails.Message")
        Me.Label18.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("ProvideTokenInfo.Label")
        Me.Label19.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("FineGrainedPermissions.Message")
        Me.Label13.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("WorkWithKey.Label")
        Me.Label15.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("UseGeneratedKey.Message")
        Me.Label9.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("ChooseTokenType.Message")
        Me.Label8.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("CreatePat.Label")
        Me.Label4.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("OpenSettings.Message")
        Me.Label5.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("OpenDeveloperSettings.Message")
        Me.Label6.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("GenerateToken.Message")
        Me.Label7.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("ClassicTokenNote.Message")
        Me.Label3.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("TokenManagementSteps.Message")
        Me.Label2.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("AccessTokenManagement.Label")
        Me.LinkLabel2.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("CreatePat.Link")
        Me.LinkLabel1.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("AccessTokenManagement.Link")
        Me.Label1.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("SelectStep.Message")
        Me.Text = LocalizationService.ForSection("StarterScript.Designer.ApiKeyWizard")("Title")
    End Sub

End Class

Partial Class BulkScriptConversionDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("StarterScript.Designer.BulkConversion")("Wait.Message")
        Me.Label2.Text = LocalizationService.ForSection("StarterScript.Designer.BulkConversion")("Progress.Label")
        Me.Text = LocalizationService.ForSection("StarterScript.Designer.BulkConversion")("Title")
    End Sub

End Class

Partial Class CryptographicProgressDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Text = LocalizationService.ForSection("StarterScript.Designer.CryptoProgress")("Title")
    End Sub

End Class

Partial Class AICustomRuleViewer

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleViewer")("Help.Message")
        Me.AddCustomRuleButton.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleViewer")("Add.Button")
        Me.ModifyCustomRuleButton.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleViewer")("Modify.Button")
        Me.DeleteCustomRuleButton.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleViewer")("Delete.Button")
        Me.SaveCustomRulesButton.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleViewer")("Save.Button")
        Me.RefreshRulesButton.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleViewer")("Refresh.Button")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleViewer")("Name.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleViewer")("Description.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleViewer")("Expression.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleViewer")("Severity.Column")
        Me.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleViewer")("Title")
    End Sub

End Class

Partial Class CustomRuleDetailsDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("RuleName.Label")
        Me.Label2.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("RuleDescription.Label")
        Me.Label3.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("RuleSeverity.Label")
        Me.RuleSeverityComboBox.Items(0) = LocalizationService.ForSection("StarterScript.CustomRule.Common")("Low.Item")
        Me.RuleSeverityComboBox.Items(1) = LocalizationService.ForSection("StarterScript.CustomRule.Common")("Medium.Item")
        Me.RuleSeverityComboBox.Items(2) = LocalizationService.ForSection("StarterScript.CustomRule.Common")("High.Item")
        Me.RuleSeverityComboBox.Text = LocalizationService.ForSection("StarterScript.CustomRule.Common")("Medium.Item")
        Me.GroupBox1.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("ExpressionParameters.Group")
        Me.RegexNextMatchButton.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("NextMatch.Button")
        Me.RegexPrevMatchButton.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("PreviousMatch.Button")
        Me.MatchCountLabel.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails").Format("MatchCount.Label", 0)
        Me.RegexTesterButton.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("TestMatches.Button")
        Me.ComboBox2.Items(0) = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("Custom.Item")
        Me.ComboBox2.Items(1) = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("ApiKeyLeaks.Item")
        Me.ComboBox2.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("Custom.Item")
        Me.Label6.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("PatternTemplate.Label")
        Me.Label5.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("TestRule.Message")
        Me.RegexCheatSheetButton.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("RegexCheatsheet.Button")
        Me.Label4.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("RuleExpression.Label")
        Me.Text = LocalizationService.ForSection("StarterScript.Designer.CustomRuleDetails")("Title")
    End Sub

End Class

Partial Class InspectionProgressDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("StarterScript.Designer.InspectionProgress")("Wait.Message")
        Me.Text = LocalizationService.ForSection("StarterScript.Designer.InspectionProgress")("Title")
    End Sub

End Class

Partial Class MainForm

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.ToolStripButton1.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("New.StarterScript.Ctrl.Label")
        Me.ToolStripButton2.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("Open.StarterScript.Label")
        Me.ToolStripButton3.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("Save.StarterScript.Label")
        Me.ToolStripButton3.ToolTipText = LocalizationService.ForSection("StarterScript.Designer.Main")("Save.StarterScript.Tooltip")
        Me.ToolStripButton4.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("About.ToolButton")
        Me.ColorModeTSDDB.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("Change.Color.Mode.Button")
        Me.LightCM_TSMI.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("Light.Label")
        Me.DarkCM_TSMI.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("Dark.Label")
        Me.SystemCM_TSMI.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("System.Label")
        Me.ToolStripButton8.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("SaveScript.Ctrl.Shift.Label")
        Me.ToolStripButton5.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("Enable.Write.Access.Button")
        Me.ToolStripButton6.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("Configure.Target.Button")
        Me.ToolStripButton7.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("Change.Editor.Font.Button")
        Me.ToolStripButton9.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("Upload.Script.Button")
        Me.ToolStripButton9.ToolTipText = LocalizationService.ForSection("StarterScript.Designer.Main")("Upload.Script.Tooltip")
        Me.ToolStripButton10.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("Inspect.Script.Security.Button")
        Me.ToolStripButton10.ToolTipText = LocalizationService.ForSection("StarterScript.Designer.Main")("Inspect.Script.Security.Tooltip")
        Me.Label1.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("Starter.Scripts.Message")
        Me.Label6.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("LineColumn.Label")
        Me.CheckBox1.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("WordWrap.CheckBox")
        Me.Button2.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("NormalizeSpacing.Button")
        Me.Button1.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("Import.Existing.Button")
        Me.Label5.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("ScriptCode.Label")
        Me.Label4.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("ScriptLanguage.Label")
        Me.Label3.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("Script.Description.Label")
        Me.Label2.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("ScriptName.Label")
        Me.CheckBox2.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("ScriptOptions.CheckBox")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("StarterScript.Designer.Main")("Starter.Scripts.Dtss.Filter")
        Me.SaveFileDialog1.Filter = LocalizationService.ForSection("StarterScript.Designer.Main")("Starter.Scripts.Dtss.Filter")
        Me.OpenFileDialog2.Filter = LocalizationService.ForSection("StarterScript.Designer.Main")("BatchScripts.Filter")
        Me.OpenFileDialog2.Title = LocalizationService.ForSection("StarterScript.Designer.Main")("Import.Existing.Script.Title")
        Me.ToolStripButton11.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("Customize.Inspection.Rules.Button")
        Me.Text = LocalizationService.ForSection("StarterScript.Designer.Main")("StarterScript.Editor.Label")
    End Sub

End Class

Partial Class ScriptVersionChooser

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("StarterScript.Designer.Version")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("StarterScript.Designer.Version")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("StarterScript.Designer.Version")("ConfiguredScript.Message")
        Me.RadioButton1.Text = LocalizationService.ForSection("StarterScript.Designer.Version")("Target.Future08.RadioButton")
        Me.RadioButton2.Text = LocalizationService.ForSection("StarterScript.Designer.Version")("Target.Legacy073.RadioButton")
        Me.Text = LocalizationService.ForSection("StarterScript.Designer.Version")("TargetVersion.Label")
    End Sub

End Class

Partial Class UploadToScriptLibraryDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("StarterScript.Designer.UploadLibrary")("Upload.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("StarterScript.Designer.UploadLibrary")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("StarterScript.Designer.UploadLibrary")("Introduction.Message")
        Me.LinkLabel1.Text = LocalizationService.ForSection("StarterScript.Designer.UploadLibrary")("SignUp.Link")
        Me.Label2.Text = LocalizationService.ForSection("StarterScript.Designer.UploadLibrary")("ApiKeyExplanation.Message")
        Me.Label3.Text = LocalizationService.ForSection("StarterScript.Designer.UploadLibrary")("ApiKey.Label")
        Me.LinkLabel2.Text = LocalizationService.ForSection("StarterScript.Designer.UploadLibrary")("GetApiKey.Link")
        Me.Label4.Text = LocalizationService.ForSection("StarterScript.Designer.UploadLibrary")("PrivacyWarning.Message")
        Me.CheckBox1.Text = LocalizationService.ForSection("StarterScript.Designer.UploadLibrary")("AllowUse.CheckBox")
        Me.CheckBox2.Text = LocalizationService.ForSection("StarterScript.Designer.UploadLibrary")("AcknowledgeRisk.CheckBox")
        Me.CheckBox3.Text = LocalizationService.ForSection("StarterScript.Designer.UploadLibrary")("SaveApiKey.CheckBox")
        Me.PreventLeaks_InfoBtn.Text = LocalizationService.ForSection("StarterScript.Designer.UploadLibrary")("LearnMore.Button")
        Me.PreventLeaks_InspectBtn.Text = LocalizationService.ForSection("StarterScript.Designer.UploadLibrary")("InspectCode.Button")
        Me.Text = LocalizationService.ForSection("StarterScript.Designer.UploadLibrary")("Title")
    End Sub

End Class
