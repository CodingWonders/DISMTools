Imports System

' Runtime localization is intentionally kept outside Windows Forms designer files.
' English design-time text remains available to the Visual Studio form designer.

Partial Class MainForm

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("WhatWant.Label")
        Me.LinkLabel1.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Install.Operating.Link")
        Me.LinkLabel2.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Restart.Install.Media.Link")
        Me.LinkLabel3.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("StartPXE.Link")
        Me.ExitLink.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Exit.Button")
        Me.LinkLabel6.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Explore.Contents.Disc.Link")
        Me.LinkLabel4.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Prepare.System.Image.Link")
        Me.Label2.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("PE.Helper.Message")
        Me.Label3.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("StartPXE.Label")
        Me.LinkLabel5.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Back.Button")
        Me.LinkLabel10.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Copy.Install.Image.Link")
        Me.LinkLabel9.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("Copy.Boot.Image.Link")
        Me.LinkLabel7.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("StartPXE.PXEFOG.Link")
        Me.LinkLabel8.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("StartPXE.PXE.Windows.Link")
        Me.Text = LocalizationService.ForSection("PEHelper.Designer.Main")("DISM.Tools.PE.Label")
    End Sub

End Class

Partial Class ServerPortSpecifier

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("PEHelper.Designer.ServerPort")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("PEHelper.Designer.ServerPort")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("PEHelper.Designer.ServerPort")("Components.Disc.Rely.Message")
        Me.Label2.Text = LocalizationService.ForSection("PEHelper.Designer.ServerPort")("Port.Server.Label")
        Me.Button1.Text = LocalizationService.ForSection("PEHelper.Designer.ServerPort")("Default.Button")
        Me.Button2.Text = LocalizationService.ForSection("PEHelper.Designer.ServerPort")("Check.Button")
        Me.Text = LocalizationService.ForSection("PEHelper.Designer.ServerPort")("ServerComponents.Label")
    End Sub

End Class

Partial Class SysprepPreparatorModeDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("PEHelper.Designer.Sysprep")("Responsibility.Message")
        Me.LinkLabel3.Text = LocalizationService.ForSection("PEHelper.Designer.Sysprep")("Cancel.Link")
        Me.LinkLabel2.Text = LocalizationService.ForSection("PEHelper.Designer.Sysprep")("ManualMode.Link")
        Me.LinkLabel1.Text = LocalizationService.ForSection("PEHelper.Designer.Sysprep")("AutomaticMode.Link")
        Me.CheckBox1.Text = LocalizationService.ForSection("PEHelper.Designer.Sysprep")("CaptureImage.CheckBox")
        Me.CheckBox2.Text = LocalizationService.ForSection("PEHelper.Designer.Sysprep")("CopyRegistry.CheckBox")
        Me.Text = LocalizationService.ForSection("PEHelper.Designer.Sysprep")("PrepareCapture.Label")
    End Sub

End Class

Partial Class WDSBootImageArchitectureSelector

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("PEHelper.Designer.WDSArch")("Okbutton.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("PEHelper.Designer.WDSArch")("CancelButton.Button")
        Me.Label1.Text = LocalizationService.ForSection("PEHelper.Designer.WDSArch")("Architecture.Label")
        Me.Text = LocalizationService.ForSection("PEHelper.Designer.WDSArch")("Architecture.Label.Label")
    End Sub

End Class

Partial Class WDSImageGroupSpecifier

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("PEHelper.Designer.WDSGroup")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("PEHelper.Designer.WDSGroup")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("PEHelper.Designer.WDSGroup")("Action.Choose.Label")
        Me.Refresh_Button.Text = LocalizationService.ForSection("PEHelper.Designer.WDSGroup")("Refresh.Button")
        Me.RadioButton1.Text = LocalizationService.ForSection("PEHelper.Designer.WDSGroup")("Upload.RadioButton")
        Me.RadioButton2.Text = LocalizationService.ForSection("PEHelper.Designer.WDSGroup")("CreateGroup.RadioButton")
        Me.Label2.Text = LocalizationService.ForSection("PEHelper.Designer.WDSGroup")("Already.Exists.Label")
        Me.Text = LocalizationService.ForSection("PEHelper.Designer.WDSGroup")("SpecifyGroup.Button")
    End Sub

End Class
