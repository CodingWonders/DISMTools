Imports System

' Runtime localization is intentionally kept outside Windows Forms designer files.
' English design-time text remains available to the Visual Studio form designer.

Partial Class DiskSpaceChecker

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = GetValueFromLanguageData("DiskSpaceChecker.WndDesc")
        Me.Label2.Text = GetValueFromLanguageData("DiskSpaceChecker.DSC_GenericProgress")
        Me.Text = GetValueFromLanguageData("DiskSpaceChecker.WndTitle")
    End Sub

End Class

Partial Class MainForm

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.GetImgInfoBtn.Text = GetValueFromLanguageData("MainForm.GetImageInformationButton")
        Me.ExportDrvsBtn.Text = GetValueFromLanguageData("MainForm.ExportDriversButton")
        Me.BackButton.Text = GetValueFromLanguageData("MainForm.NavigationBackButtonText")
        Me.NextButton.Text = GetValueFromLanguageData("MainForm.NavigationNextButtonText")
        Me.ExitButton.Text = GetValueFromLanguageData("MainForm.NavigationExitButtonText")
        Me.Label38.Text = GetValueFromLanguageData("MainForm.ErrorPanel_PossibleFixes")
        Me.Label37.Text = GetValueFromLanguageData("MainForm.ErrorPanel_Description")
        Me.Label36.Text = GetValueFromLanguageData("MainForm.ErrorPanel_Header")
        Me.RestartButton.Text = GetValueFromLanguageData("MainForm.FinishPanel_RestartNow")
        Me.Label32.Text = GetValueFromLanguageData("MainForm.FinishPanel_Description")
        Me.Label35.Text = GetValueFromLanguageData("MainForm.FinishPanel_RestartTimer_Beginning")
        Me.Label33.Text = GetValueFromLanguageData("MainForm.FinishPanel_Header")
        Me.Label20.Text = GetValueFromLanguageData("MainForm.PreparationPanel_Step1")
        Me.Label27.Text = GetValueFromLanguageData("MainForm.PreparationPanel_Step2")
        Me.Label31.Text = GetValueFromLanguageData("MainForm.PreparationPanel_Step3")
        Me.Label34.Text = String.Format(GetValueFromLanguageData("MainForm.PreparationPanel_ApiProgress"), 0)
        Me.Label19.Text = GetValueFromLanguageData("MainForm.PreparationPanel_GenericProgress")
        Me.Label17.Text = GetValueFromLanguageData("MainForm.PreparationPanel_Description")
        Me.Label18.Text = GetValueFromLanguageData("MainForm.PreparationPanel_Header")
        Me.Label15.Text = GetValueFromLanguageData("MainForm.ExplanationPanel_Description")
        Me.Label16.Text = GetValueFromLanguageData("MainForm.ExplanationPanel_Header")
        Me.GroupBox2.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_InstallImageInfoGroup")
        Me.ColumnHeader1.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_IndexColumnHeader")
        Me.ColumnHeader2.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_InstallImageName")
        Me.ColumnHeader3.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_InstallImageDescription")
        Me.ColumnHeader4.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_InstallImageVersion")
        Me.ColumnHeader5.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_InstallImageArchitecture")
        Me.GroupBox1.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_BootImageInfoGroup")
        Me.Label10.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_BootImageArchitecture")
        Me.Label9.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_BootImageVersion")
        Me.Label13.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_BootImageArchitecturePlaceholder")
        Me.Label12.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_BootImageVersionPlaceholder")
        Me.Label11.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_BootImageNamePlaceholder")
        Me.Label8.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_BootImageName")
        Me.Label6.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_ComputerArchitecturePlaceholder")
        Me.Label7.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_ImageArchitectureMismatchError")
        Me.Label5.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_ComputerArchitecture")
        Me.Label14.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_DIM_Notice")
        Me.Label3.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_Description")
        Me.Label4.Text = GetValueFromLanguageData("MainForm.ReviewImageInfo_Header")
        Me.CheckBox1.Text = GetValueFromLanguageData("MainForm.DisclaimerPanel_DisclaimerCheck")
        Me.TabPage1.Text = GetValueFromLanguageData("MainForm.DisclaimerPanel_ContentTabTitle1")
        Me.TextBox1.Text = GetValueFromLanguageData("MainForm.DisclaimerPanel_Warranties")
        Me.TabPage2.Text = GetValueFromLanguageData("MainForm.DisclaimerPanel_ContentTabTitle2")
        Me.TextBox2.Text = GetValueFromLanguageData("MainForm.DisclaimerPanel_UseOfDiscImages")
        Me.TabPage3.Text = GetValueFromLanguageData("MainForm.DisclaimerPanel_ContentTabTitle3")
        Me.Label2.Text = GetValueFromLanguageData("MainForm.DisclaimerPanel_Description")
        Me.Label1.Text = GetValueFromLanguageData("MainForm.DisclaimerPanel_Header")
        Me.ExportDrvsFBD.Description = GetValueFromLanguageData("MainForm.ExportDriversFolderDialog")
    End Sub

End Class

Partial Class SplashForm

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.VersionLabel.Text = GetValueFromLanguageData("SplashScreen.VersionLabel")
        Me.Label1.Text = GetValueFromLanguageData("SplashScreen.OSInstTitle")
        Me.Label2.Text = GetValueFromLanguageData("SplashScreen.OSInstStatus_StartingUp")
        Me.Text = GetValueFromLanguageData("SplashScreen.WindowTitle")
    End Sub

End Class
