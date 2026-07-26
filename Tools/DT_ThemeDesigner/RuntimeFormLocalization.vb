Imports System

' Runtime localization is intentionally kept outside Windows Forms designer files.
' English design-time text remains available to the Visual Studio form designer.

Partial Class MainForm

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.GroupBox1.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("ThemeColors.Group")
        Me.Label17.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("OptionFour.Label")
        Me.Label16.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("OptionThree.Label")
        Me.Label15.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("OptionTwo.Label")
        Me.Label14.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("OptionOne.Label")
        Me.Button7.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Change.Button")
        Me.Button6.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Change.Button")
        Me.Button5.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Change.Button")
        Me.Button4.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Change.Button")
        Me.Button2.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Change.Button")
        Me.Label3.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Bg.Color.Inner.Label")
        Me.Button3.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Change.Button")
        Me.Button1.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Change.Button")
        Me.Label4.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("ForegroundColor.Label")
        Me.Label5.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Inactive.Colors.Label")
        Me.Label6.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("AccentColors.Label")
        Me.Label2.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("BackgroundColor.Label")
        Me.CheckBox1.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("DISM.Tools.Dark.CheckBox")
        Me.Label1.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("ThemeName.Label")
        Me.Label20.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("See.Changes.Live.Label")
        Me.ToolStripButton1.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("NewTheme.Label")
        Me.ToolStripButton2.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Open.Theme.File.Button")
        Me.ToolStripButton3.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Save.Theme.File.Button")
        Me.ToolStripButton4.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("About.Button")
        Me.AccentedLabel4.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Value.Option4.Label")
        Me.AccentedLabel3.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Value.Option3.Label")
        Me.AccentedLabel2.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Value.Option2.Label")
        Me.TextBox2.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Heuristic.Reasoning.Message")
        Me.InactiveLabel.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Inactivecontrol.Label")
        Me.ActiveLabel.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Activecontrol.Label")
        Me.Label18.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Label.Inner.Section.Label")
        Me.AccentedLabel1.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Value.Option1.Label")
        Me.Label19.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("TestControl.Label")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Theme.Files.Ini.Filter")
        Me.SaveFileDialog1.Filter = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("SaveFile.Filter")
        Me.ColorModeTSDDB.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Change.Color.Mode.Button")
        Me.LightCM_TSMI.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Light.Label")
        Me.DarkCM_TSMI.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Dark.Label")
        Me.SystemCM_TSMI.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("System.Label")
        Me.ToolStripButton5.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("Enable.Write.Access.Button")
        Me.Text = LocalizationService.ForSection("ThemeDesigner.Designer.Main")("DISM.Tools.Theme.Label")
    End Sub

End Class
