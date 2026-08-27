Imports System

' Runtime localization is intentionally kept outside Windows Forms designer files.
' English design-time text remains available to the Visual Studio form designer.

Partial Class MainForm

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Updater.Designer.Main")("DISM.Tools.Update.Label")
        Me.Label2.Text = LocalizationService.ForSection("Updater.Designer.Main")("ProductUpdates.Label")
        Me.Button1.Text = LocalizationService.ForSection("Updater.Designer.Main")("Update.Button")
        Me.LinkLabel1.Text = LocalizationService.ForSection("Updater.Designer.Main")("View.Release.Notes.Link")
        Me.Label6.Text = LocalizationService.ForSection("Updater.Designer.Main")("VersionInfo.Label")
        Me.Label7.Text = LocalizationService.ForSection("Updater.Designer.Main")("Close.Open.Message")
        Me.Label5.Text = LocalizationService.ForSection("Updater.Designer.Main")("NewVersion.Label")
        Me.Label4.Text = LocalizationService.ForSection("Updater.Designer.Main")("Progress.Label")
        Me.Label3.Text = LocalizationService.ForSection("Updater.Designer.Main")("CheckingUpdates.Label")
        Me.CheckBox1.Text = LocalizationService.ForSection("Updater.Designer.Main")("Launch.Ready.CheckBox")
        Me.Label13.Text = LocalizationService.ForSection("Updater.Designer.Main")("Finishing.Update.Label")
        Me.Label12.Text = LocalizationService.ForSection("Updater.Designer.Main")("InstallingUpdate.Label")
        Me.Label11.Text = LocalizationService.ForSection("Updater.Designer.Main")("Prepare.Update.Install.Label")
        Me.Label10.Text = LocalizationService.ForSection("Updater.Designer.Main")("Downloading.Update.Label")
        Me.Label14.Text = LocalizationService.ForSection("Updater.Designer.Main")("Update.Take.Time.Label")
        Me.Label9.Text = LocalizationService.ForSection("Updater.Designer.Main")("Wait.Update.Label")
        Me.Label8.Text = LocalizationService.ForSection("Updater.Designer.Main")("Updating.DISM.Tools.Label")
        Me.Button2.Text = LocalizationService.ForSection("Updater.Designer.Main")("Launch.Button")
        Me.Label17.Text = LocalizationService.ForSection("Updater.Designer.Main")("Version.Come.New.Message")
        Me.Label16.Text = LocalizationService.ForSection("Updater.Designer.Main")("DISM.Tools.Updated.Label")
        Me.Label15.Text = LocalizationService.ForSection("Updater.Designer.Main")("UpdateComplete.Label")
        Me.Text = LocalizationService.ForSection("Updater.Designer.Main")("ProductUpdates.Label")
    End Sub

End Class
