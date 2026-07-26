Imports System

' Runtime localization is intentionally kept outside Windows Forms designer files.
' English design-time text remains available to the Visual Studio form designer.

Partial Class ADDSJoinDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.DnsToolsBtn.Text = LocalizationService.ForSection("Designer.DomainJoin")("DnstoolsBtn.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.DomainJoin")("Nicsettings.Group")
        Me.DnsSyntaxCheckerBtn.Text = LocalizationService.ForSection("Designer.DomainJoin")("Verify.DNS.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.DomainJoin")("Default.Adapter.Same.Message")
        Me.RadioButton2.Text = LocalizationService.ForSection("Designer.DomainJoin")("ManualAdapter.RadioButton")
        Me.Label7.Text = LocalizationService.ForSection("Designer.DomainJoin")("Address.First.Line.Message")
        Me.Label6.Text = LocalizationService.ForSection("Designer.DomainJoin")("DNSServer.Addresses.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.DomainJoin")("PrimarySuffix.Label")
        Me.RadioButton1.Text = LocalizationService.ForSection("Designer.DomainJoin")("PickAdapter.RadioButton")
        Me.Label3.Text = LocalizationService.ForSection("Designer.DomainJoin")("InterfaceAlias.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.DomainJoin")("Domain.Suffix.Added.Message")
        Me.Label1.Text = LocalizationService.ForSection("Designer.DomainJoin")("PrimarySuffix.Label")
        Me.DNSConfigHeader.Text = LocalizationService.ForSection("Designer.DomainJoin")("DNSSettings.Label")
        Me.Label17.Text = LocalizationService.ForSection("Designer.DomainJoin")("Type.Security.Account.Label")
        Me.Label15.Text = LocalizationService.ForSection("Designer.DomainJoin")("Organizational.Unit.Label")
        Me.Label16.Text = LocalizationService.ForSection("Designer.DomainJoin")("User.Label")
        Me.Label19.Text = LocalizationService.ForSection("Designer.DomainJoin")("SAM.Account.Label")
        Me.Label18.Text = LocalizationService.ForSection("Designer.DomainJoin")("Org.Unit.Account.Message")
        Me.DsAccountObjectPickerBtn.Text = LocalizationService.ForSection("Designer.DomainJoin")("Pick.Account.Object.Button")
        Me.ComboBox4.Items(0) = LocalizationService.ForSection("Designer.DomainJoin")("User.Manually.Item")
        Me.ComboBox4.Items(1) = LocalizationService.ForSection("Designer.DomainJoin")("Pick.User.Org.Item")
        Me.ComboBox4.Items(2) = LocalizationService.ForSection("Designer.DomainJoin")("Pick.User.Object.Item")
        Me.Label13.Text = LocalizationService.ForSection("Designer.DomainJoin")("User.Principal.Name.Label")
        Me.Label14.Text = LocalizationService.ForSection("Designer.DomainJoin")("Logon.Path.Pre.Label")
        Me.Label11.Text = LocalizationService.ForSection("Designer.DomainJoin")("Domain.Auto.Detected.Message")
        Me.Label12.Text = LocalizationService.ForSection("Designer.DomainJoin")("Ask.Admin.Provide.Message")
        Me.Label10.Text = LocalizationService.ForSection("Designer.DomainJoin")("Password.Label")
        Me.Label9.Text = LocalizationService.ForSection("Designer.DomainJoin")("UserAccount.Label")
        Me.Label8.Text = LocalizationService.ForSection("Designer.DomainJoin")("DomainName.Label")
        Me.DSDomainConfigHeader.Text = LocalizationService.ForSection("Designer.DomainJoin")("Domain.Auth.Label")
        Me.DS7_Description.Text = LocalizationService.ForSection("Designer.DomainJoin")("Wizard.Helps.Set.Description")
        Me.DS7_Header.Text = LocalizationService.ForSection("Designer.DomainJoin")("Join.Active.Dir.Label")
        Me.DNS_Explanation_Link.Text = LocalizationService.ForSection("Designer.DomainJoin")("WhatDNS.Link")
        Me.Back_Button.Text = LocalizationService.ForSection("Designer.DomainJoin")("Back.Button")
        Me.Next_Button.Text = LocalizationService.ForSection("Designer.DomainJoin")("Next.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.DomainJoin")("Cancel.Button")
        Me.Help_Button.Text = LocalizationService.ForSection("Designer.DomainJoin")("Help.Button")
        Me.DnsResolutionTSMI.Text = LocalizationService.ForSection("Designer.DomainJoin")("Test.Dnsresolution.Label")
        Me.DnsZoneTSMI.Text = LocalizationService.ForSection("Designer.DomainJoin")("DNSZone.Domain.Choose.Label")
        Me.Text = LocalizationService.ForSection("Designer.DomainJoin")("Domain.Services.Wizard.Label")
    End Sub

End Class

Partial Class DnsZoneChooserDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.DNSZones")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.DNSZones")("CancelButton.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.DNSZones")("OfferedZones.Message")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.DNSZones")("ZoneName.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.DNSZones")("DnsserverName.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.DNSZones")("DomainServices.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.DNSZones")("ZoneType.Column")
        Me.Refresh_Button.Text = LocalizationService.ForSection("Designer.DNSZones")("Refresh.Button")
        Me.Text = LocalizationService.ForSection("Designer.DNSZones")("DNSZone.Choose.Label")
    End Sub

End Class

Partial Class MainForm

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.FileToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("File.Label")
        Me.NewProjectToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("NewProject.Button")
        Me.OpenExistingProjectToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Open.Existing.Project.Label")
        Me.ManageOnlineInstallationToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Manage.Online.Install.Label")
        Me.ManageOfflineInstallationToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Manage.Ffline.Button")
        Me.RecentProjectsListMenu.Text = LocalizationService.ForSection("Designer.Main")("RecentProjects.Label")
        Me.SaveProjectToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("SaveProject.Button")
        Me.SaveProjectasToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Save.Project.Button")
        Me.ExitToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Exit.Label")
        Me.ProjectToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Project.Label")
        Me.ViewProjectFilesInFileExplorerToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("View.Project.Files.Label")
        Me.UnloadProjectToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("UnloadProject.Button")
        Me.SwitchImageIndexesToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Switch.Image.Indexes.Button")
        Me.ProjectPropertiesToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("ProjectProps.Label")
        Me.ImagePropertiesToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("ImageProps.Label")
        Me.CommandsToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Commands.Label")
        Me.ImageManagementToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("ImageManagement.Label")
        Me.AppendImage.Text = LocalizationService.ForSection("Designer.Main")("Append.Capture.Dir.Button")
        Me.ApplyFFU.Text = LocalizationService.ForSection("Designer.Main")("ApplyFfusfufile.Button")
        Me.ApplyImage.Text = LocalizationService.ForSection("Designer.Main")("ApplyWimswmfile.Button")
        Me.CaptureCustomImage.Text = LocalizationService.ForSection("Designer.Main")("Capture.Incremental.Button")
        Me.CaptureFFU.Text = LocalizationService.ForSection("Designer.Main")("Capture.Partitions.Button")
        Me.CaptureImage.Text = LocalizationService.ForSection("Designer.Main")("Capture.Image.Drive.Button")
        Me.CleanupMountpoints.Text = LocalizationService.ForSection("Designer.Main")("Delete.Resources.Button")
        Me.CommitImage.Text = LocalizationService.ForSection("Designer.Main")("Apply.Changes.Image.Button")
        Me.DeleteImage.Text = LocalizationService.ForSection("Designer.Main")("Delete.Volume.Image.Button")
        Me.ExportImage.Text = LocalizationService.ForSection("Designer.Main")("ExportImage.Button")
        Me.GetImageInfo.Text = LocalizationService.ForSection("Designer.Main")("Get.Image.Button")
        Me.GetWIMBootEntry.Text = LocalizationService.ForSection("Designer.Main")("Get.WIM.Boot.Button")
        Me.ListImage.Text = LocalizationService.ForSection("Designer.Main")("List.Files.Dirs.Button")
        Me.MountImage.Text = LocalizationService.ForSection("Designer.Main")("MountImage.Button")
        Me.OptimizeFFU.Text = LocalizationService.ForSection("Designer.Main")("Optimize.FFU.File.Button")
        Me.OptimizeImage.Text = LocalizationService.ForSection("Designer.Main")("OptimizeImage.Button")
        Me.RemountImage.Text = LocalizationService.ForSection("Designer.Main")("Remount.Image.Button")
        Me.SplitFFU.Text = LocalizationService.ForSection("Designer.Main")("Split.FFU.File.Button")
        Me.SplitImage.Text = LocalizationService.ForSection("Designer.Main")("Split.WIM.File.Button")
        Me.UnmountImage.Text = LocalizationService.ForSection("Designer.Main")("UnmountImage.Button")
        Me.UpdateWIMBootEntry.Text = LocalizationService.ForSection("Designer.Main")("Update.WIM.Boot.Button")
        Me.ApplySiloedPackage.Text = LocalizationService.ForSection("Designer.Main")("Apply.Siloed.Prov.Button")
        Me.SaveImageInformationToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Save.Image.Button")
        Me.OSPackagesToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("OSPackages.Label")
        Me.GetPackages.Text = LocalizationService.ForSection("Designer.Main")("GetPackages.Button")
        Me.AddPackage.Text = LocalizationService.ForSection("Designer.Main")("AddPackage.Button")
        Me.RemovePackage.Text = LocalizationService.ForSection("Designer.Main")("RemovePackage.Button")
        Me.GetFeatures.Text = LocalizationService.ForSection("Designer.Main")("GetFeatures.Button")
        Me.EnableFeature.Text = LocalizationService.ForSection("Designer.Main")("EnableFeature.Button")
        Me.DisableFeature.Text = LocalizationService.ForSection("Designer.Main")("DisableFeature.Button")
        Me.CleanupImage.Text = LocalizationService.ForSection("Designer.Main")("CleanupRecovery.Button")
        Me.ProvisioningPackagesToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("ProvPackages.Label")
        Me.AddProvisioningPackage.Text = LocalizationService.ForSection("Designer.Main")("Add.Prov.Package.Button")
        Me.GetProvisioningPackageInfo.Text = LocalizationService.ForSection("Designer.Main")("Get.Prov.Package.Button")
        Me.ApplyCustomDataImage.Text = LocalizationService.ForSection("Designer.Main")("Apply.CustomData.Button")
        Me.AppPackagesToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("AppPackages.Label")
        Me.GetProvisionedAppxPackages.Text = LocalizationService.ForSection("Designer.Main")("Get.App.Package.Button")
        Me.AddProvisionedAppxPackage.Text = LocalizationService.ForSection("Designer.Main")("Add.Provisioned.App.Button")
        Me.RemoveProvisionedAppxPackage.Text = LocalizationService.ForSection("Designer.Main")("Remove.Prov.App.Button")
        Me.OptimizeProvisionedAppxPackages.Text = LocalizationService.ForSection("Designer.Main")("Optimize.Provisioned.Button")
        Me.SetProvisionedAppxDataFile.Text = LocalizationService.ForSection("Designer.Main")("Add.CustomData.File.Button")
        Me.AppPatchesToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("AppMspservicing.Label")
        Me.CheckAppPatch.Text = LocalizationService.ForSection("Designer.Main")("Get.App.Patch.Button")
        Me.GetAppPatchInfo.Text = LocalizationService.ForSection("Designer.Main")("Installed.App.Details.Button")
        Me.GetAppPatches.Text = LocalizationService.ForSection("Designer.Main")("Basic.Installed.App.Button")
        Me.GetAppInfo.Text = LocalizationService.ForSection("Designer.Main")("Get.Detailed.Button")
        Me.GetApps.Text = LocalizationService.ForSection("Designer.Main")("Get.Basic.Windows.Button")
        Me.DefaultAppAssociationsToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("DefaultApp.Assoc.Label")
        Me.ExportDefaultAppAssociations.Text = LocalizationService.ForSection("Designer.Main")("Export.Default.Button")
        Me.GetDefaultAppAssociations.Text = LocalizationService.ForSection("Designer.Main")("DefaultApp.Assoc.Button")
        Me.ImportDefaultAppAssociations.Text = LocalizationService.ForSection("Designer.Main")("Import.Default.Button")
        Me.RemoveDefaultAppAssociations.Text = LocalizationService.ForSection("Designer.Main")("Remove.Default.Button")
        Me.LanguagesAndRegionSettingsToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Languages.Regional.Label")
        Me.GetIntl.Text = LocalizationService.ForSection("Designer.Main")("Intl.Settings.Button")
        Me.SetUILang.Text = LocalizationService.ForSection("Designer.Main")("SetUilanguage.Button")
        Me.SetUILangFallback.Text = LocalizationService.ForSection("Designer.Main")("Set.Default.Button")
        Me.SetSysUILang.Text = LocalizationService.ForSection("Designer.Main")("Set.System.Preferred.Button")
        Me.SetSysLocale.Text = LocalizationService.ForSection("Designer.Main")("Set.System.Locale.Button")
        Me.SetUserLocale.Text = LocalizationService.ForSection("Designer.Main")("Set.User.Locale.Button")
        Me.SetInputLocale.Text = LocalizationService.ForSection("Designer.Main")("Set.Input.Locale.Button")
        Me.SetAllIntl.Text = LocalizationService.ForSection("Designer.Main")("Set.UI.Button")
        Me.SetTimeZone.Text = LocalizationService.ForSection("Designer.Main")("Set.Default.Time.Button")
        Me.SetSKUIntlDefaults.Text = LocalizationService.ForSection("Designer.Main")("Set.Default.Languages.Button")
        Me.SetLayeredDriver.Text = LocalizationService.ForSection("Designer.Main")("Set.Layered.Driver.Button")
        Me.GenLangINI.Text = LocalizationService.ForSection("Designer.Main")("Generate.Lang.Ini.Button")
        Me.SetSetupUILang.Text = LocalizationService.ForSection("Designer.Main")("Set.Default.Setup.Button")
        Me.CapabilitiesToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Capabilities.Label")
        Me.AddCapability.Text = LocalizationService.ForSection("Designer.Main")("AddCapability.Button")
        Me.ExportSource.Text = LocalizationService.ForSection("Designer.Main")("Export.Capabilities.Button")
        Me.GetCapabilities.Text = LocalizationService.ForSection("Designer.Main")("GetCapabilities.Button")
        Me.RemoveCapability.Text = LocalizationService.ForSection("Designer.Main")("RemoveCapability.Button")
        Me.WindowsEditionsToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("WindowsEditions.Label")
        Me.GetCurrentEdition.Text = LocalizationService.ForSection("Designer.Main")("Get.Edition.Button")
        Me.GetTargetEditions.Text = LocalizationService.ForSection("Designer.Main")("Get.Upgrade.Targets.Button")
        Me.SetEdition.Text = LocalizationService.ForSection("Designer.Main")("UpgradeImage.Button")
        Me.SetProductKey.Text = LocalizationService.ForSection("Designer.Main")("SetProductKey.Button")
        Me.DriversToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Drivers.Label")
        Me.GetDrivers.Text = LocalizationService.ForSection("Designer.Main")("GetDrivers.Button")
        Me.AddDriver.Text = LocalizationService.ForSection("Designer.Main")("AddDriver.Button")
        Me.RemoveDriver.Text = LocalizationService.ForSection("Designer.Main")("RemoveDriver.Button")
        Me.ExportDriver.Text = LocalizationService.ForSection("Designer.Main")("Export.DriverPackages.Button")
        Me.ImportDriver.Text = LocalizationService.ForSection("Designer.Main")("Import.DriverPackages.Button")
        Me.UnattendedAnswerFilesToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Unattended.Answer.Label")
        Me.ApplyUnattend.Text = LocalizationService.ForSection("Designer.Main")("Apply.Unattended.Button")
        Me.RemoveAppliedAnswerFileToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Remove.Applied.Label")
        Me.AuditModeTSMI.Text = LocalizationService.ForSection("Designer.Main")("System.Enter.Audit.Label")
        Me.WindowsPEServicingToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("WindowsPE.Label")
        Me.GetPESettings.Text = LocalizationService.ForSection("Designer.Main")("GetSettings.Button")
        Me.SetScratchSpace.Text = LocalizationService.ForSection("Designer.Main")("SetScratchSpace.Button")
        Me.SetTargetPath.Text = LocalizationService.ForSection("Designer.Main")("Set.Target.Path.Button")
        Me.OSUninstallToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("OSUninstall.Label")
        Me.GetOSUninstallWindow.Text = LocalizationService.ForSection("Designer.Main")("Get.Uninstall.Window.Button")
        Me.InitiateOSUninstall.Text = LocalizationService.ForSection("Designer.Main")("Initiate.Uninstall.Button")
        Me.RemoveOSUninstall.Text = LocalizationService.ForSection("Designer.Main")("Remove.Roll.Back.Button")
        Me.SetOSUninstallWindow.Text = LocalizationService.ForSection("Designer.Main")("Set.Uninstall.Window.Button")
        Me.ReservedStorageToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("ReservedStorage.Label")
        Me.SetReservedStorageState.Text = LocalizationService.ForSection("Designer.Main")("Set.Reserved.Storage.Button")
        Me.GetReservedStorageState.Text = LocalizationService.ForSection("Designer.Main")("Get.Reserved.Storage.Button")
        Me.MicrosoftEdgeToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("MicrosoftEdge.Label")
        Me.AddEdge.Text = LocalizationService.ForSection("Designer.Main")("AddEdge.Button")
        Me.AddEdgeBrowser.Text = LocalizationService.ForSection("Designer.Main")("Add.Edge.Browser.Button")
        Me.AddEdgeWebView.Text = LocalizationService.ForSection("Designer.Main")("Add.Edge.Web.Button")
        Me.ToolsToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Tools.Label")
        Me.ImageConversionToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("ImageConversion.Label")
        Me.WIMESDToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Wimesd.Label")
        Me.MergeSWM.Text = LocalizationService.ForSection("Designer.Main")("MergeSwmfiles.Button")
        Me.RemountImageWithWritePermissionsToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Remount.Image.Write.Label")
        Me.CommandShellToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("CommandConsole.Label")
        Me.UnattendedAnswerFileManagerToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Unattended.AnswerFile.Label")
        Me.UnattendedAnswerFileCreatorToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Unattended.Creator.Label")
        Me.RegCplToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Manage.Image.Registry.Button")
        Me.ManageSystemServicesToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Manage.System.Button")
        Me.ManageSystemEnvironmentVariablesToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Manage.System.Env.Button")
        Me.WebResourcesToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("WebResources.Label")
        Me.LanguagesAndOptionalFeaturesISOToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Download.Languages.Button")
        Me.LanguagesAndFODWin10ToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Download.FOD.Button")
        Me.PxeHelperServersTSMI.Text = LocalizationService.ForSection("Designer.Main")("StartPXE.Button")
        Me.StartWdsHelperTSMI.Text = LocalizationService.ForSection("Designer.Main")("Windows.Label")
        Me.StartFogHelperTSMI.Text = LocalizationService.ForSection("Designer.Main")("FOG.Label")
        Me.UnixFogInstructionTSMI.Text = LocalizationService.ForSection("Designer.Main")("Show.Instructions.Label")
        Me.CopyImageToWdsServerTSMI.Text = LocalizationService.ForSection("Designer.Main")("Copy.My.Windows.Button")
        Me.EvaluateWindowsUEFICA2023ReadinessToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Evaluate.Windows.Label")
        Me.ReportManagerToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("ReportManager.Label")
        Me.MountedImageManagerTSMI.Text = LocalizationService.ForSection("Designer.Main")("Mounted.Image.Manager.Label")
        Me.CreateDiscImageToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Create.Disc.Image.Button")
        Me.CreateTestingEnvironmentToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Create.Testing.Button")
        Me.WimScriptEditorCommand.Text = LocalizationService.ForSection("Designer.Main")("Config.List.Editor.Label")
        Me.SSE_TSMI.Text = LocalizationService.ForSection("Designer.Main")("Create.StarterScript.Label")
        Me.ThemeDesigner_TSMI.Text = LocalizationService.ForSection("Designer.Main")("DesignTheme.Label")
        Me.OptionsToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Options.Label")
        Me.HelpToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Help.Label")
        Me.HelpTopicsToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("HelpTopics.Label")
        Me.DISMToolsTourToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("DISM.Tools.Tour.Label")
        Me.AboutDISMToolsToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("DISM.Tools.Label")
        Me.Discord.Text = LocalizationService.ForSection("Designer.Main")("Join.Discord.Opens.Label")
        Me.ReportFeedbackToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Report.Feedback.Opens.Label")
        Me.OpenDiagnosticLogsInLogViewerToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Open.Diagnostic.Logs.Label")
        Me.ContributeToTheHelpSystemToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Contribute.Help.System.Label")
        Me.BranchTSMI.Text = LocalizationService.ForSection("Designer.Main")("Branch.Label")
        Me.VersionTSMI.Text = LocalizationService.ForSection("Designer.Main")("Preview.Label")
        Me.VersionTSMI.ToolTipText = LocalizationService.ForSection("Designer.Main")("Beta.Release.Tooltip")
        Me.ExitFullScreenTSMI.Text = LocalizationService.ForSection("Designer.Main")("Full.Screen.Shortcut.Label")
        Me.InvalidSettingsTSMI.Text = LocalizationService.ForSection("Designer.Main")("Settings.Detected.Label")
        Me.ISFix.Text = LocalizationService.ForSection("Designer.Main")("MoreInfo.Label")
        Me.ISHelp.Text = LocalizationService.ForSection("Designer.Main")("WhatsThis.Label")
        Me.TourActionsTSMI.Text = LocalizationService.ForSection("Designer.Main")("DISM.Tools.Actions.Label")
        Me.ServerStatusTSMI.Text = LocalizationService.ForSection("Designer.Main")("Tour.Server.Active.Label")
        Me.RestartDTTourTSMI.Text = LocalizationService.ForSection("Designer.Main")("RestartTour.Label")
        Me.StopDTTourServerTSMI.Text = LocalizationService.ForSection("Designer.Main")("Stop.Tour.Server.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.Main")("Video.Content.Loaded.Label")
        Me.LinkLabel31.Text = LocalizationService.ForSection("Designer.Main")("LearnMore.Link")
        Me.LinkLabel32.Text = LocalizationService.ForSection("Designer.Main")("Retry.Button")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.Main")("Name.Column")
        Me.Label9.Text = LocalizationService.ForSection("Designer.Main")("FactDay.Label")
        Me.Label12.Text = LocalizationService.ForSection("Designer.Main")("Learn.Watching.Videos.Label")
        Me.LinkLabel30.Text = LocalizationService.ForSection("Designer.Main")("Managing.External.Link")
        Me.LinkLabel29.Text = LocalizationService.ForSection("Designer.Main")("Managing.Install.Link")
        Me.LinkLabel28.Text = LocalizationService.ForSection("Designer.Main")("Get.Started.DISM.Link")
        Me.LinkLabel27.Text = LocalizationService.ForSection("Designer.Main")("Learn.Snew.Link")
        Me.Label4.Text = LocalizationService.ForSection("Designer.Main")("Explore.Get.Started.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.Main")("News.Feed.Loaded.Label")
        Me.LinkLabel34.Text = LocalizationService.ForSection("Main.News")("LearnMore.Link")
        Me.LinkLabel33.Text = LocalizationService.ForSection("Main.News.Load")("Retry.Button")
        Me.Label5.Text = LocalizationService.ForSection("Designer.Main")("Stay.Up.Date.Label")
        Me.Label8.Text = LocalizationService.ForSection("Designer.Main")("News.Last.Updated.Label")
        Me.NewsFeedTextLabel.Text = LocalizationService.ForSection("Designer.Main")("NewsFeed.Item.Label")
        Me.NewsFeedDateLabel.Text = LocalizationService.ForSection("Designer.Main")("Item.Feed.Date.Label")
        Me.ComputerOSLabel.Text = LocalizationService.ForSection("Designer.Main")("OS.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.Main")("IP.Address.Config.Label")
        Me.ComputerProcessorLabel.Text = LocalizationService.ForSection("Designer.Main")("Processor.Label")
        Me.Label1.Text = LocalizationService.ForSection("Designer.Main")("DomainMembership.Label")
        Me.ComputerMemoryLabel.Text = LocalizationService.ForSection("Designer.Main")("Memory.Label")
        Me.ComputerStorageLabel.Text = LocalizationService.ForSection("Designer.Main")("Storage.Label")
        Me.ComputerDomainStatusLabel.Text = LocalizationService.ForSection("Designer.Main")("DomainStatus.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.Main")("WorkgroupDomain.Label")
        Me.ComputerModelLabel.Text = LocalizationService.ForSection("Designer.Main")("ComputerModel.Label")
        Me.ComputerNameLabel.Text = LocalizationService.ForSection("Designer.Main")("ComputerName.Label")
        Me.ChangeComputerNameLink.Text = LocalizationService.ForSection("Designer.Main")("Rename.Link")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.Main")("PathName.Column")
        Me.UpdateLink.Text = LocalizationService.ForSection("Designer.Main")("NewVersion.Available.Link")
        Me.RecentRemoveLink.Text = LocalizationService.ForSection("Designer.Main")("RemoveEntry.Link")
        Me.OfflineInstMgmt.Text = LocalizationService.ForSection("Designer.Main")("Manage.Offline.Button.Button")
        Me.OnlineInstMgmt.Text = LocalizationService.ForSection("Designer.Main")("Manage.Online.Install.Link")
        Me.ExistingProjLink.Text = LocalizationService.ForSection("Designer.Main")("Open.Existing.Project.Link")
        Me.NewProjLink.Text = LocalizationService.ForSection("Designer.Main")("NewProject.Link")
        Me.Label10.Text = LocalizationService.ForSection("Designer.Main")("RecentProjects.Label")
        Me.LabelHeader1.Text = LocalizationService.ForSection("Designer.Main")("Begin.Label")
        Me.GroupBox4.Text = LocalizationService.ForSection("Designer.Main")("ImageOperations.Group")
        Me.Button24.Text = LocalizationService.ForSection("Designer.Main")("Switch.Image.Indexes.Button")
        Me.Button31.Text = LocalizationService.ForSection("Designer.Main")("CaptureImage.Button")
        Me.Button30.Text = LocalizationService.ForSection("Designer.Main")("ApplyImage.Button")
        Me.Button33.Text = LocalizationService.ForSection("Designer.Main")("Save.Complete.Image.Button")
        Me.Button32.Text = LocalizationService.ForSection("Designer.Main")("Remove.VolumeImages.Button")
        Me.Button26.Text = LocalizationService.ForSection("Designer.Main")("MountImage.Button")
        Me.Button25.Text = LocalizationService.ForSection("Designer.Main")("Reload.Servicing.Button")
        Me.Button29.Text = LocalizationService.ForSection("Designer.Main")("Unmount.Image.Button")
        Me.Button28.Text = LocalizationService.ForSection("Designer.Main")("CommitImage.Button")
        Me.Button27.Text = LocalizationService.ForSection("Designer.Main")("Commit.Changes.Button")
        Me.GroupBox5.Text = LocalizationService.ForSection("Designer.Main")("Package.Operations.Group")
        Me.Button38.Text = LocalizationService.ForSection("Designer.Main")("Save.Installed.Button")
        Me.Button35.Text = LocalizationService.ForSection("Designer.Main")("RemovePackage.Button")
        Me.Button37.Text = LocalizationService.ForSection("Designer.Main")("Component.Store.Maint.Button")
        Me.Button34.Text = LocalizationService.ForSection("Designer.Main")("Get.Package.Button")
        Me.Button36.Text = LocalizationService.ForSection("Designer.Main")("AddPackage.Button")
        Me.GroupBox6.Text = LocalizationService.ForSection("Designer.Main")("Feature.Operations.Group")
        Me.Button42.Text = LocalizationService.ForSection("Designer.Main")("Save.Feature.Button")
        Me.Button39.Text = LocalizationService.ForSection("Designer.Main")("Get.Feature.Button")
        Me.Button41.Text = LocalizationService.ForSection("Designer.Main")("EnableFeature.Button")
        Me.Button40.Text = LocalizationService.ForSection("Designer.Main")("DisableFeature.Button")
        Me.GroupBox7.Text = LocalizationService.ForSection("Designer.Main")("AppX.Package.Operations")
        Me.Button46.Text = LocalizationService.ForSection("Designer.Main")("Save.Installed.AppX.Button")
        Me.Button44.Text = LocalizationService.ForSection("Designer.Main")("Add.AppX.Package.Button")
        Me.Button45.Text = LocalizationService.ForSection("Designer.Main")("Get.App.Button")
        Me.Button43.Text = LocalizationService.ForSection("Designer.Main")("Remove.AppX.Package.Button")
        Me.GroupBox8.Text = LocalizationService.ForSection("Designer.Main")("Capability.Operations.Group")
        Me.Button50.Text = LocalizationService.ForSection("Designer.Main")("Save.Capability.Button")
        Me.Button48.Text = LocalizationService.ForSection("Designer.Main")("AddCapability.Button")
        Me.Button49.Text = LocalizationService.ForSection("Designer.Main")("Get.Capability.Button")
        Me.Button47.Text = LocalizationService.ForSection("Designer.Main")("RemoveCapability.Button")
        Me.GroupBox9.Text = LocalizationService.ForSection("Designer.Main")("DriverOperations.Group")
        Me.Button54.Text = LocalizationService.ForSection("Designer.Main")("Save.Installed.Driver.Button")
        Me.Button53.Text = LocalizationService.ForSection("Designer.Main")("AddDriverPackage.Button")
        Me.Button51.Text = LocalizationService.ForSection("Designer.Main")("RemoveDriver.Button")
        Me.Button52.Text = LocalizationService.ForSection("Designer.Main")("Get.Driver.Button")
        Me.GroupBox10.Text = LocalizationService.ForSection("Designer.Main")("Windows.Group")
        Me.Button58.Text = LocalizationService.ForSection("Designer.Main")("SetScratchSpace.Button")
        Me.Button57.Text = LocalizationService.ForSection("Designer.Main")("Set.Target.Path.Button")
        Me.Button56.Text = LocalizationService.ForSection("Designer.Main")("SaveConfig.Button")
        Me.Button55.Text = LocalizationService.ForSection("Designer.Main")("GetConfig.Button")
        Me.BWFailLearnMoreBtn.Text = LocalizationService.ForSection("Designer.Main")("LearnMore.Button")
        Me.BWFailLabel.Text = LocalizationService.ForSection("Designer.Main")("One.Bg.Procs.Message")
        Me.Label55.Text = LocalizationService.ForSection("Designer.Main")("ProjectTasks.Label")
        Me.LinkLabel17.Text = LocalizationService.ForSection("Designer.Main")("UnloadProject.Link")
        Me.LinkLabel16.Text = LocalizationService.ForSection("Designer.Main")("Open.File.Explorer.Link")
        Me.LinkLabel15.Text = LocalizationService.ForSection("Designer.Main")("View.Project.Props.Link")
        Me.Button21.Text = LocalizationService.ForSection("Designer.Main")("UnloadProject.ActionButton")
        Me.Button22.Text = LocalizationService.ForSection("Designer.Main")("View.File.Explorer.Button")
        Me.Button23.Text = LocalizationService.ForSection("Designer.Main")("View.Project.Props.Button")
        Me.LinkLabel14.Text = LocalizationService.ForSection("Designer.Main")("Mount.Image.Link")
        Me.Label50.Text = LocalizationService.ForSection("Designer.Main")("ImgStatus.Label")
        Me.Label51.Text = LocalizationService.ForSection("Designer.Main")("Location.Label")
        Me.Label52.Text = LocalizationService.ForSection("Designer.Main")("ProjPath.Label")
        Me.Label53.Text = LocalizationService.ForSection("Designer.Main")("ImagesMounted.Label")
        Me.Label54.Text = LocalizationService.ForSection("Designer.Main")("Name.Label")
        Me.Label49.Text = LocalizationService.ForSection("Designer.Main")("ProjectName.DynamicLabel")
        Me.Label59.Text = LocalizationService.ForSection("Designer.Main")("ImageMounted.Label")
        Me.Label58.Text = LocalizationService.ForSection("Designer.Main")("Mount.Image.Order.Label")
        Me.Label57.Text = LocalizationService.ForSection("Designer.Main")("Choices.Label")
        Me.LinkLabel18.Text = LocalizationService.ForSection("Designer.Main")("Pick.Mounted.Image.Link")
        Me.LinkLabel21.Text = LocalizationService.ForSection("Designer.Main")("MountImage.Link")
        Me.Label56.Text = LocalizationService.ForSection("Designer.Main")("ImageTasks.Label")
        Me.LinkLabel19.Text = LocalizationService.ForSection("Designer.Main")("UnmountImage.Link")
        Me.LinkLabel20.Text = LocalizationService.ForSection("Designer.Main")("View.Image.Props.Link")
        Me.Label39.Text = LocalizationService.ForSection("Designer.Main")("ImageIndex.Label")
        Me.Label40.Text = LocalizationService.ForSection("Designer.Main")("Description.Label")
        Me.Label41.Text = LocalizationService.ForSection("Designer.Main")("ImgIndex.Label")
        Me.Label42.Text = LocalizationService.ForSection("Designer.Main")("Name.Label")
        Me.Label43.Text = LocalizationService.ForSection("Designer.Main")("MountPoint.Label")
        Me.Label44.Text = LocalizationService.ForSection("Designer.Main")("MountPoint.Value")
        Me.Label45.Text = LocalizationService.ForSection("Designer.Main")("Version.Label")
        Me.Label46.Text = LocalizationService.ForSection("Designer.Main")("ImgName.Label")
        Me.Label47.Text = LocalizationService.ForSection("Designer.Main")("ImgDesc.Label")
        Me.Label48.Text = LocalizationService.ForSection("Designer.Main")("ImgVersion.Label")
        Me.LinkLabel12.Text = LocalizationService.ForSection("Designer.Main")("Project.Link")
        Me.LinkLabel13.Text = LocalizationService.ForSection("Designer.Main")("Image.Link")
        Me.TimeLabel.Text = LocalizationService.ForSection("Designer.Main")("Clock.DynamicLabel")
        Me.GreetingLabel.Text = LocalizationService.ForSection("Designer.Main")("Welcome.Servicing.Label")
        Me.ToolStripButton1.Text = LocalizationService.ForSection("Designer.Main")("CloseTab.Label")
        Me.ToolStripButton2.Text = LocalizationService.ForSection("Designer.Main")("SaveProject.Label")
        Me.ToolStripButton3.Text = LocalizationService.ForSection("Designer.Main")("UnloadProject.Label")
        Me.ToolStripButton3.ToolTipText = LocalizationService.ForSection("Designer.Main")("Unload.Project.Tooltip")
        Me.ToolStripButton4.Text = LocalizationService.ForSection("Designer.Main")("Show.Progress.Window.Label")
        Me.RefreshViewTSB.Text = LocalizationService.ForSection("Designer.Main")("RefreshView.Label")
        Me.ExpandCollapseTSB.Text = LocalizationService.ForSection("Designer.Main")("Expand.Label")
        Me.ToolStripStatusLabel2.Text = LocalizationService.ForSection("Designer.Main")("Preparing.Project.Button")
        Me.StatusStrip.Text = LocalizationService.ForSection("Designer.Main")("Status.Label")
        Me.BackgroundProcessesButton.ToolTipText = LocalizationService.ForSection("Designer.Main")("View.BgProcesses.Tooltip")
        Me.MenuDesc.Text = LocalizationService.ForSection("Designer.Main")("Ready.Label")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.Main")("DISM.Tools.Project.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.Main")("Project.File.Load.Title")
        Me.PkgBasicInfo.Text = LocalizationService.ForSection("Designer.Main")("Get.Basic.Label")
        Me.PkgDetailedInfo.Text = LocalizationService.ForSection("Designer.Main")("Get.Detailed.Specific.Label")
        Me.LocalMountDirFBD.Description = LocalizationService.ForSection("Designer.Main")("MountDir.Description")
        Me.CommitAndUnmountTSMI.Text = LocalizationService.ForSection("Designer.Main")("CommitImage.Label")
        Me.DiscardAndUnmountTSMI.Text = LocalizationService.ForSection("Designer.Main")("Discard.Changes.Label")
        Me.UnmountSettingsToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("UnmountSettings.Button")
        Me.ViewPackageDirectoryToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("View.Package.Dir.Label")
        Me.ResViewTSMI.Text = LocalizationService.ForSection("Designer.Main")("ViewResources.Label")
        Me.ExpandToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("ExpandItem.Label")
        Me.AccessDirectoryToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("AccessDirectory.Label")
        Me.UnloadProjectToolStripMenuItem1.Text = LocalizationService.ForSection("Designer.Main")("UnloadProject.Label")
        Me.CopyDeploymentToolsToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Copy.Deployment.Tools.Label")
        Me.OfAllArchitecturesToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("AllArchitectures.Label")
        Me.OfSelectedArchitectureToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Selected.Architecture.Label")
        Me.ForX86ArchitectureToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Xarchitecture.Label")
        Me.ForAmd64ArchitectureToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Amarkdown.Architecture.Label")
        Me.ForARMArchitectureToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("ARM.Label")
        Me.ForARM64ArchitectureToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("ARM64.Label")
        Me.ImageOperationsToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("ImageOperations.Label")
        Me.MountImageToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("MountImage.Button")
        Me.UnmountImageToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("UnmountImage.Button")
        Me.RemoveVolumeImagesToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Remove.VolumeImages.Button")
        Me.SwitchImageIndexesToolStripMenuItem1.Text = LocalizationService.ForSection("Designer.Main")("Switch.Image.Indexes.Button")
        Me.UnattendedAnswerFilesToolStripMenuItem1.Text = LocalizationService.ForSection("Designer.Main")("Unattended.Answer.Label")
        Me.ManageToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Manage.Label")
        Me.CreationWizardToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Create.Label")
        Me.ScratchDirectorySettingsToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Configure.Scratch.Dir.Label")
        Me.ManageReportsToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("ManageReports.Label")
        Me.AddToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Add.Button")
        Me.NewFileToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("NewFile.Button")
        Me.ExistingFileToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("ExistingFile.Button")
        Me.SaveResourceToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("SaveResource.Button")
        Me.CopyToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("CopyResource.Label")
        Me.AppxResSFD.Filter = LocalizationService.ForSection("Designer.Main")("PngFiles.Filter")
        Me.MicrosoftAppsToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Visit.Microsoft.Apps.Label")
        Me.MicrosoftStoreGenerationProjectToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Visit.Microsoft.Label")
        Me.AppxDownloadHelpToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Iget.Apps.Label")
        Me.ImgInfoSFD.Filter = LocalizationService.ForSection("Designer.Main")("MarkdownFiles.Filter")
        Me.GetImageFileInformationToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Get.ImageFile.Button")
        Me.SaveCompleteImageInformationToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Save.Complete.Image.Button")
        Me.CreateDiscImageWithThisFileToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Create.Disc.ImageFile.Button")
        Me.UploadThisImageToMyWDSServerToolStripMenuItem.Text = LocalizationService.ForSection("Designer.Main")("Upload.Image.My.Button")
        Me.ApplyWimTSMI.Text = LocalizationService.ForSection("Designer.Main")("ApplyWimswmesd.Button")
        Me.ApplyFfuTSMI.Text = LocalizationService.ForSection("Designer.Main")("Apply.FFU.File.Button")
        Me.CaptureWimTSMI.Text = LocalizationService.ForSection("Designer.Main")("Capture.Install.Dir.Button")
        Me.CaptureFfuTSMI.Text = LocalizationService.ForSection("Designer.Main")("Capture.Install.Drive.Button")
        Me.Text = LocalizationService.ForSection("Designer.Main")("DISMTools.Label")
    End Sub

End Class

Partial Class LockVolumeDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.BDE.LockVolume")("Wait.Message")
        Me.Label2.Text = LocalizationService.ForSection("Designer.BDE.LockVolume")("DriveLetter.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.BDE.LockVolume")("PersistentVolumeId.Label")
        Me.Text = LocalizationService.ForSection("Designer.BDE.LockVolume")("Title")
    End Sub

End Class

Partial Class UnlockVolumeDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.BDE.UnlockVolume")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.BDE.UnlockVolume")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.BDE.UnlockVolume")("RecoveryKey.Message")
        Me.Label2.Text = LocalizationService.ForSection("Designer.BDE.UnlockVolume")("KeyProtectorId.Label")
        Me.Text = LocalizationService.ForSection("Designer.BDE.UnlockVolume")("Title")
    End Sub

End Class

Partial Class AddListEntryDlg

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.Add.List")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.Add.List")("Cancel.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.Add.List")("Browse.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.Add.List")("Entry.Label")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.Add.List")("AllFiles.Filter")
        Me.Text = LocalizationService.ForSection("Designer.Add.List")("AddEntry.Label")
    End Sub

End Class

Partial Class OneDriveExclusionDlg

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.OneDriveExclusion")("Exclude.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.OneDriveExclusion")("CancelButton.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.OneDriveExclusion")("Tool.Help.Exclude.Message")
        Me.Label3.Text = LocalizationService.ForSection("Designer.OneDriveExclusion")("Re.Ready.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.OneDriveExclusion")("Path.Exclude.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.OneDriveExclusion")("Browse.Button")
        Me.FolderBrowserDialog1.Description = LocalizationService.ForSection("Designer.OneDriveExclusion")("UserFolderPath.Description")
        Me.Text = LocalizationService.ForSection("Designer.OneDriveExclusion")("Exclude.User.Label")
    End Sub

End Class

Partial Class WimScriptEditor

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Config.List.Allows.Message")
        Me.GroupBox3.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Compression.Exclusion.List")
        Me.Button10.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Edit.Button")
        Me.Button9.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Add.Button")
        Me.Button11.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Remove.Button")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Exclusion.Exception.List")
        Me.Button5.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Add.Button")
        Me.Button7.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Remove.Button")
        Me.Button6.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Edit.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("ExclusionList.Group")
        Me.Button3.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Remove.Button")
        Me.Button2.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Edit.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Add.Button")
        Me.ToolStripButton2.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("New.Label")
        Me.ToolStripButton3.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Open.Button")
        Me.ToolStripButton4.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Save.Button")
        Me.ToolStripButton5.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Toggle.Word.Wrap.Label")
        Me.ToolStripButton6.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Help.Label")
        Me.ToolStripDropDownButton1.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Tools.Label")
        Me.NoOneDriveToolStripMenuItem.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("Exclude.User.One.Button")
        Me.WimScriptOFD.Filter = LocalizationService.ForSection("Designer.WimScriptEditor")("Inifiles.Filter")
        Me.WimScriptOFD.Title = LocalizationService.ForSection("Designer.WimScriptEditor")("Config.List.Load.Title")
        Me.WimScriptSFD.Filter = LocalizationService.ForSection("Designer.WimScriptEditor")("Wimscript.Filter")
        Me.WimScriptSFD.Title = LocalizationService.ForSection("Designer.WimScriptEditor")("Location.Save.Config.Title")
        Me.Text = LocalizationService.ForSection("Designer.WimScriptEditor")("ConfigList.Label")
    End Sub

End Class

Partial Class PleaseWaitDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.Wait")("Wait.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.Wait")("Action.Label")
        Me.Text = LocalizationService.ForSection("Designer.Wait")("Wait.Label")
    End Sub

End Class

Partial Class ProgressPanel

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.Progress")("Image.Operations.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.Progress")("Wait.Tasks.Label")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.Progress")("Cancel.Button")
        Me.currentTask.Text = LocalizationService.ForSection("Designer.Progress")("CurrentTask.Label")
        Me.allTasks.Text = LocalizationService.ForSection("Designer.Progress")("AllTasks.Label")
        Me.taskCountLbl.Text = LocalizationService.ForSection("Designer.Progress")("Tasks.Tcont.Label")
        Me.LogButton.Text = LocalizationService.ForSection("Designer.Progress")("ShowLog.Label")
        Me.LinkLabel1.Text = LocalizationService.ForSection("Designer.Progress")("Show.Dismlog.File.Link")
        Me.Text = LocalizationService.ForSection("Designer.Progress")("Progress.Label")
    End Sub

End Class

Partial Class ExceptionForm

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.ExceptionForm")("Sorry.Inconvenience.Message")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ExceptionForm")("Help.Us.Fix.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ExceptionForm")("Problem.Prevention.Message")
        Me.Issue_Btn.Text = LocalizationService.ForSection("Designer.ExceptionForm")("ReportIssue.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.ExceptionForm")("Continue.Running.Message")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ExceptionForm")("Reporting.Issue.Message")
        Me.LinkLabel1.Text = LocalizationService.ForSection("Designer.ExceptionForm")("Continue.Button")
        Me.LinkLabel2.Text = LocalizationService.ForSection("Designer.ExceptionForm")("Exit.Button")
        Me.DynaViewer_Button.Text = LocalizationService.ForSection("Designer.ExceptionForm")("Copy.Inspect.Logs.Button")
        Me.Text = LocalizationService.ForSection("Designer.ExceptionForm")("DISM.Tools.Internal.Label")
    End Sub

End Class

Partial Class BGProcsAdvSettings

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.BgProcesses")("Okbutton.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.BgProcesses")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.BgProcsSettings")("Additional.Label")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.BgProcesses")("Enhance.App.Detect.CheckBox")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.BgProcesses")("SkipNonRemovable.CheckBox")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.BgProcesses")("DetectAllDrivers.CheckBox")
        Me.CheckBox4.Text = LocalizationService.ForSection("Designer.BgProcesses")("Skip.Framework.CheckBox")
        Me.CheckBox5.Text = LocalizationService.ForSection("Designer.BgProcesses")("Run.CheckBox")
        Me.Text = LocalizationService.ForSection("Designer.BgProcsSettings")("Advanced.Process.Label")
    End Sub

End Class

Partial Class DismComponents

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.DISMComponents")("Ok.Button")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.DISMComponents")("Component.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.DISMComponents")("Version.Column")
        Me.Text = LocalizationService.ForSection("Designer.DISMComponents")("Dismcomponents.Label")
    End Sub

End Class

Partial Class InvalidSettingsDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Button1.Text = LocalizationService.ForSection("Designer.InvalidSettings")("Ok.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.InvalidSettings")("ResetDefaults.Message")
        Me.Label1.Text = LocalizationService.ForSection("Designer.InvalidSettings")("Found.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.InvalidSettings")("Scratch.Dir.Status.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.InvalidSettings")("Log.File.Status.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.InvalidSettings")("Log.Font.Status.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.InvalidSettings")("DISM.Executable.Status.Label")
        Me.Text = LocalizationService.ForSection("Designer.InvalidSettings")("Detected.Label")
    End Sub

End Class

Partial Class MigrationForm

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.MigrationForm")("Wait.Message")
        Me.Label2.Text = LocalizationService.ForSection("Designer.MigrationForm")("Wait.Label")
        Me.Text = LocalizationService.ForSection("Designer.MigrationForm")("DISMTools.Label")
    End Sub

End Class

Partial Class Options

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.Options")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.Options")("Cancel.Button")
        Me.DismOFD.Filter = LocalizationService.ForSection("Designer.Options")("DISM.Executable.Filter")
        Me.DismOFD.Title = LocalizationService.ForSection("Designer.Options")("Dismexecutable.Title")
        Me.ScratchFBD.Description = LocalizationService.ForSection("Designer.Options")("ScratchDir.Description")
        Me.CheckBox13.Text = LocalizationService.ForSection("Designer.Options")("CheckUpdates.CheckBox")
        Me.CheckBox12.Text = LocalizationService.ForSection("Designer.Options")("Remount.Mounted.CheckBox")
        Me.Label43.Text = LocalizationService.ForSection("Designer.Options")("Behavior.OnStartup.Label")
        Me.Label46.Text = LocalizationService.ForSection("Designer.Options")("Settings.Aren.Label")
        Me.CheckBox24.Text = LocalizationService.ForSection("Designer.Options")("Set.Custom.CheckBox")
        Me.CheckBox11.Text = LocalizationService.ForSection("Designer.Options")("FileIcons.Projects.CheckBox")
        Me.DTSSEditAssocCB.Text = LocalizationService.ForSection("Designer.Options")("Open.Starter.Scripts.Label")
        Me.Button9.Text = LocalizationService.ForSection("Designer.Options")("Set.File.Assoc.Button")
        Me.DTProjAssocCB.Text = LocalizationService.ForSection("Designer.Options")("Open.My.Projects.Label")
        Me.Label40.Text = LocalizationService.ForSection("Designer.Options")("Manage.File.Assoc.Label")
        Me.Button10.Text = LocalizationService.ForSection("Designer.Options")("AdvancedSettings.Button")
        Me.LinkLabel2.Text = LocalizationService.ForSection("Designer.Options")("Learn.Background.Link")
        Me.Label29.Text = LocalizationService.ForSection("Designer.Options")("Uses.Bg.Procs.Message")
        Me.ComboBox6.Items(0) = LocalizationService.ForSection("Designer.Options")("Every.Time.Project.Item")
        Me.ComboBox6.Items(1) = LocalizationService.ForSection("Designer.Options")("Once.Item")
        Me.ComboBox6.Text = LocalizationService.ForSection("Designer.Options")("Every.Time.Project.Item")
        Me.Label28.Text = LocalizationService.ForSection("Designer.Options")("Notify.Label")
        Me.CheckBox6.Text = LocalizationService.ForSection("Designer.Options")("Notify.Me.CheckBox")
        Me.Label27.Text = LocalizationService.ForSection("Designer.Options")("Reports.Allow.Shown.Label")
        Me.TextBox4.Text = LocalizationService.ForSection("Designer.Options")("Image.Version.Message")
        Me.ComboBox5.Items(0) = LocalizationService.ForSection("Designer.Options")("List.Item")
        Me.ComboBox5.Items(1) = LocalizationService.ForSection("Designer.Options")("Table.Item")
        Me.ComboBox5.Text = LocalizationService.ForSection("Designer.Options")("List.Item")
        Me.Label26.Text = LocalizationService.ForSection("Designer.Options")("ExampleReport.Label")
        Me.Label25.Text = LocalizationService.ForSection("Designer.Options")("LogView.Label")
        Me.CheckBox5.Text = LocalizationService.ForSection("Designer.Options")("Show.Command.Output.CheckBox")
        Me.RadioButton4.Text = LocalizationService.ForSection("Designer.Options")("Custom.Scratch.RadioButton")
        Me.RadioButton3.Text = LocalizationService.ForSection("Designer.Options")("Project.Scratch.RadioButton")
        Me.Label24.Text = LocalizationService.ForSection("Designer.Options")("Enough.Space.Selected.Label")
        Me.Label23.Text = LocalizationService.ForSection("Designer.Options")("ScdirSpace.Label")
        Me.Label22.Text = LocalizationService.ForSection("Designer.Options")("Space.Left.Selected.Label")
        Me.Button4.Text = LocalizationService.ForSection("Designer.Options")("Browse.Button")
        Me.Label21.Text = LocalizationService.ForSection("Designer.Options")("ScratchDirectory.Label")
        Me.Label44.Text = LocalizationService.ForSection("Designer.Options")("Scratch.Dir.Message")
        Me.Label20.Text = LocalizationService.ForSection("Designer.Options")("Scratch.Dir.Required.Label")
        Me.CheckBox4.Text = LocalizationService.ForSection("Designer.Options")("Scratch.Dir.CheckBox")
        Me.CheckBox14.Text = LocalizationService.ForSection("Designer.Options")("Always.Save.CheckBox")
        Me.Label48.Text = LocalizationService.ForSection("Designer.Options")("SettingsConsider.Label")
        Me.CheckBox15.Text = LocalizationService.ForSection("Designer.Options")("Installed.Packages.CheckBox")
        Me.CheckBox19.Text = LocalizationService.ForSection("Designer.Options")("InstalledDrivers.CheckBox")
        Me.CheckBox18.Text = LocalizationService.ForSection("Designer.Options")("Capabilities.CheckBox")
        Me.CheckBox16.Text = LocalizationService.ForSection("Designer.Options")("Features.CheckBox")
        Me.CheckBox17.Text = LocalizationService.ForSection("Designer.Options")("Installed.AppX.CheckBox")
        Me.Label19.Text = LocalizationService.ForSection("Designer.Options")("Checked.Computer.Message")
        Me.Label18.Text = LocalizationService.ForSection("Designer.Options")("QuietOperations.Message")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.Options")("Skip.System.Restart.CheckBox")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.Options")("Quietly.Image.Ops.CheckBox")
        Me.Label16.Text = LocalizationService.ForSection("Designer.Options")("Log.File.Display.Message")
        Me.Label15.Text = LocalizationService.ForSection("Designer.Options")("Errors.Warnings.Label")
        Me.Label13.Text = LocalizationService.ForSection("Designer.Options")("Image.Ops.Message")
        Me.Button3.Text = LocalizationService.ForSection("Designer.Options")("Browse.Button")
        Me.Label14.Text = LocalizationService.ForSection("Designer.Options")("Log.File.Level.Label")
        Me.Label12.Text = LocalizationService.ForSection("Designer.Options")("Operation.Log.File.Label")
        Me.CheckBox10.Text = LocalizationService.ForSection("Designer.Options")("Auto.Create.Logs.CheckBox")
        Me.RadioButton6.Text = LocalizationService.ForSection("Designer.Options")("Classic.RadioButton")
        Me.RadioButton5.Text = LocalizationService.ForSection("Designer.Options")("Modern.RadioButton")
        Me.Label45.Text = LocalizationService.ForSection("Designer.Options")("Secondary.Progress.Label")
        Me.Label47.Text = LocalizationService.ForSection("Designer.Options")("Font.Readable.Log.Message")
        Me.LogPreview.Text = LocalizationService.ForSection("Options.LogPreview")("Packages.Add.Message")
        Me.Label11.Text = LocalizationService.ForSection("Designer.Options")("Preview.Label")
        Me.Label10.Text = LocalizationService.ForSection("Designer.Options")("Log.Window.Font.Label")
        Me.CheckBox9.Text = LocalizationService.ForSection("Designer.Options")("Uppercase.Menus.CheckBox")
        Me.ComboBox2.Items(0) = LocalizationService.ForSection("Designer.Options")("System.Setting.Item")
        Me.ComboBox2.Items(1) = LocalizationService.ForSection("Designer.Options")("LightMode.Item")
        Me.ComboBox2.Items(2) = LocalizationService.ForSection("Designer.Options")("DarkMode.Item")
        Me.ComboBox2.Text = LocalizationService.ForSection("Designer.Options")("System.Setting.Item")
        Me.Label8.Text = LocalizationService.ForSection("Designer.Options")("Language.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.Options")("ColorMode.Label")
        Me.ComboBox1.Items(0) = LocalizationService.ForSection("Designer.Options")("SettingsFile.Item")
        Me.ComboBox1.Items(1) = LocalizationService.ForSection("Designer.Options")("Registry.Item")
        Me.ComboBox1.Text = LocalizationService.ForSection("Designer.Options")("SettingsFile.Item")
        Me.LinkLabel1.Text = LocalizationService.ForSection("Designer.Options")("Enable.Disable.Message")
        Me.Button2.Text = LocalizationService.ForSection("Designer.Options")("View.DISM.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.Options")("Browse.Button")
        Me.Label4.Text = LocalizationService.ForSection("Designer.Options")("Dismver.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.Options")("SaveSettings.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.Options")("Version.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.Options")("Dismexecutable.Path.Label")
        Me.PrefReset.Text = LocalizationService.ForSection("Designer.Options")("ResetPreferences.Label")
        Me.LogSFD.Filter = LocalizationService.ForSection("Designer.Options")("LogSFD.Filter")
        Me.LogSFD.Title = LocalizationService.ForSection("Designer.Options")("Location.Log.File.Title")
        Me.Label49.Text = LocalizationService.ForSection("Designer.Options")("Program.Label")
        Me.Label50.Text = LocalizationService.ForSection("Designer.Options")("Personalization.Label")
        Me.Label51.Text = LocalizationService.ForSection("Designer.Options")("Logs.Label")
        Me.Label52.Text = LocalizationService.ForSection("Designer.Options")("ImageOperations.Label")
        Me.Label53.Text = LocalizationService.ForSection("Designer.Options")("Scratch.Dir.Label")
        Me.Label54.Text = LocalizationService.ForSection("Designer.Options")("ProgramOutput.Label")
        Me.Label55.Text = LocalizationService.ForSection("Designer.Options")("BgProcesses.Label")
        Me.Label57.Text = LocalizationService.ForSection("Designer.Options")("FileAssociations.Label")
        Me.Label58.Text = LocalizationService.ForSection("Designer.Options")("StartupOptions.Label")
        Me.Label34.Text = LocalizationService.ForSection("Designer.Options")("ShutdownOptions.Label")
        Me.LinkLabel4.Text = LocalizationService.ForSection("Designer.Options")("Difference.Between.Link")
        Me.Label72.Text = LocalizationService.ForSection("Designer.Options")("PackageName.Label")
        Me.Label73.Text = LocalizationService.ForSection("Designer.Options")("RaymanJungle.Label")
        Me.Label74.Text = LocalizationService.ForSection("Designer.Options")("DisplayName.Label")
        Me.ComboBox8.Items(0) = LocalizationService.ForSection("Designer.Options")("Display.Name.Only.Item")
        Me.ComboBox8.Items(1) = LocalizationService.ForSection("Designer.Options")("Display.Name.Friendly.Item")
        Me.ComboBox8.Items(2) = LocalizationService.ForSection("Designer.Options")("Friendly.Display.Name.Item")
        Me.Label71.Text = LocalizationService.ForSection("Designer.Options")("Example.Label")
        Me.Label70.Text = LocalizationService.ForSection("Designer.Options")("Remove.AppX.Label")
        Me.Label32.Text = LocalizationService.ForSection("Designer.Options")("Only.Available.Message")
        Me.CheckBox23.Text = LocalizationService.ForSection("Designer.Options")("Map.System.Accounts.CheckBox")
        Me.CheckBox25.Text = LocalizationService.ForSection("Designer.Options")("Lock.BitLocker.Volumes.CheckBox")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.Options")("Show.Dates.Human.CheckBox")
        Me.CheckBox8.Text = LocalizationService.ForSection("Designer.Options")("PreventSleep.CheckBox")
        Me.Label9.Text = LocalizationService.ForSection("Designer.Options")("Saving.Image.Label")
        Me.LinkLabel5.Text = LocalizationService.ForSection("Designer.Options")("Help.Me.Understand.Link")
        Me.ComboBox9.Items(0) = LocalizationService.ForSection("Designer.Options")("Turn.Off.Many.Item")
        Me.ComboBox9.Items(1) = LocalizationService.ForSection("Designer.Options")("Me.Control.AI.Item")
        Me.ComboBox9.Items(2) = LocalizationService.ForSection("Designer.Options")("Turn.Many.Aifeatures.Item")
        Me.Label76.Text = LocalizationService.ForSection("Designer.Options")("AIFeature.Label")
        Me.Label69.Text = LocalizationService.ForSection("Designer.Options")("Search.Engine.Web.Label")
        Me.Label67.Text = LocalizationService.ForSection("Designer.Options")("Searching.Image.Online.Label")
        Me.Label68.Text = LocalizationService.ForSection("Designer.Options")("Learn.Message")
        Me.Button14.Text = LocalizationService.ForSection("Designer.Options")("RunNow.Button")
        Me.Label60.Text = LocalizationService.ForSection("Designer.Options")("Behavior.OnClose.Label")
        Me.CheckBox22.Text = LocalizationService.ForSection("Designer.Options")("Automatically.Clean.CheckBox")
        Me.Button7.Text = LocalizationService.ForSection("Designer.Options")("InstallService.Button")
        Me.Button11.Text = LocalizationService.ForSection("Designer.Options")("EnableService.Button")
        Me.Button12.Text = LocalizationService.ForSection("Designer.Options")("DisableService.Button")
        Me.Button13.Text = LocalizationService.ForSection("Designer.Options")("DeleteService.Button")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.Options")("ServiceStatus.Group")
        Me.Label79.Text = LocalizationService.ForSection("Designer.Options")("Installed.Label")
        Me.Label81.Text = LocalizationService.ForSection("Designer.Options")("InstallationPath.Label")
        Me.Label77.Text = LocalizationService.ForSection("Designer.Options")("Automatic.Image.Reload.Label")
        Me.Label83.Text = LocalizationService.ForSection("Designer.Options")("Still.See.Standard.Message")
        Me.Label78.Text = LocalizationService.ForSection("Designer.Options")("Automatic.Image.Message")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.Options")("ColorThemes.Group")
        Me.Button6.Text = LocalizationService.ForSection("Designer.Options")("DesignThemes.Button")
        Me.Label30.Text = LocalizationService.ForSection("Designer.Options")("LightMode.Label")
        Me.Label33.Text = LocalizationService.ForSection("Designer.Options")("Own.Themes.Label")
        Me.Label31.Text = LocalizationService.ForSection("Designer.Options")("Change.Color.Theme.Label")
        Me.Label17.Text = LocalizationService.ForSection("Designer.Options")("DarkMode.Label")
        Me.CheckBox21.Text = LocalizationService.ForSection("Designer.Options")("Show.Date.Time.CheckBox")
        Me.Label59.Text = LocalizationService.ForSection("Designer.Options")("LogCustomization.Label")
        Me.Label61.Text = LocalizationService.ForSection("Designer.Options")("Preview.Label")
        Me.CheckBox7.Text = LocalizationService.ForSection("Designer.Options")("Show.Log.View.CheckBox")
        Me.LinkLabel3.Text = LocalizationService.ForSection("Designer.Options")("Show.Me.Logs.Link")
        Me.CheckBox20.Text = LocalizationService.ForSection("Designer.Options")("Disable.Dyna.Log.CheckBox")
        Me.Label64.Text = LocalizationService.ForSection("Designer.Options")("Dyna.Log.Logging.Label")
        Me.Label62.Text = LocalizationService.ForSection("Designer.Options")("Dyna.Log.Logging.Message")
        Me.Button5.Text = LocalizationService.ForSection("Designer.Options")("Browse.Button")
        Me.Label66.Text = LocalizationService.ForSection("Designer.Options")("SystemEditor.Label")
        Me.Label65.Text = LocalizationService.ForSection("Designer.Options")("Editor.Open.Log.Label")
        Me.Label63.Text = LocalizationService.ForSection("Designer.Options")("Default.Op.Logs.Message")
        Me.EditorOFD.Filter = LocalizationService.ForSection("Designer.Options")("ProgramsEXE.Filter")
        Me.EditorOFD.Title = LocalizationService.ForSection("Designer.Options")("Editor.Title")
        Me.Label1.Text = LocalizationService.ForSection("Designer.Options")("Concurrent.ISO.Prompt.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.Options")("Concurrent.ISO.Tasks.Label")
        Me.Label35.Text = LocalizationService.ForSection("Designer.Options")("Concurrent.ISO.Note.Label")
        Me.Button8.Text = LocalizationService.ForSection("Designer.Options")("Determine.Button")
        Me.Text = LocalizationService.ForSection("Designer.Options")("Options.Label")
    End Sub

End Class

Partial Class PrgAbout

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.PrgAbout")("Ok.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.PrgAbout").Format("DISM.Tools.Version.Label", "")
        Me.Label15.Text = LocalizationService.ForSection("Designer.PrgAbout")("Build.Date.Goes.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.PrgAbout")("ResourcesUsed.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.PrgAbout")("Resources.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.PrgAbout")("Fluency.Label")
        Me.LinkLabel4.Text = LocalizationService.ForSection("Designer.PrgAbout")("Icons.Link")
        Me.Label6.Text = LocalizationService.ForSection("Designer.PrgAbout")("Sqlserver.Icon.Color.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.PrgAbout")("Utilities.Label")
        Me.Label8.Text = LocalizationService.ForSection("Designer.PrgAbout")("Zip.Label")
        Me.LinkLabel5.Text = LocalizationService.ForSection("Designer.PrgAbout")("VisitWebsite.Link")
        Me.LinkLabel9.Text = LocalizationService.ForSection("Designer.PrgAbout")("VisitWebsite.Link")
        Me.LinkLabel10.Text = LocalizationService.ForSection("Designer.PrgAbout")("VisitWebsite.Link")
        Me.Label10.Text = LocalizationService.ForSection("Designer.PrgAbout")("Help.Documentation.Label")
        Me.Label13.Text = LocalizationService.ForSection("Designer.PrgAbout")("Scintila.Netnu.Get.Label")
        Me.Label16.Text = LocalizationService.ForSection("Designer.PrgAbout")("Managed.Dismnu.Get.Label")
        Me.Label11.Text = LocalizationService.ForSection("Designer.PrgAbout")("Command.Help.Source.Label")
        Me.LinkLabel7.Text = LocalizationService.ForSection("Designer.PrgAbout")("Microsoft.Link")
        Me.Label17.Text = LocalizationService.ForSection("Designer.PrgAbout")("BrandingAssets.Label")
        Me.Label19.Text = LocalizationService.ForSection("Designer.PrgAbout")("DarkUI.Label")
        Me.LinkLabel12.Text = LocalizationService.ForSection("Designer.PrgAbout")("VisitWebsite.Link")
        Me.LinkLabel11.Text = LocalizationService.ForSection("Designer.PrgAbout")("Microsoft.Link")
        Me.Label18.Text = LocalizationService.ForSection("Designer.PrgAbout")("Windows.Label")
        Me.LinkLabel3.Text = LocalizationService.ForSection("Designer.PrgAbout")("Whatsnew.Link")
        Me.LinkLabel2.Text = LocalizationService.ForSection("Designer.PrgAbout")("Licenses.Link")
        Me.LinkLabel1.Text = LocalizationService.ForSection("Designer.PrgAbout")("Credits.Link")
        Me.UpdCheckBtn.Text = LocalizationService.ForSection("Designer.PrgAbout")("CheckUpdates.Label")
        Me.Text = LocalizationService.ForSection("Designer.PrgAbout")("AboutProgram.Label")
    End Sub

End Class

Partial Class SettingsResetDlg

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.SettingsResetDlg")("Yes.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.SettingsResetDlg")("No.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.SettingsResetDlg")("ProceedReset.Message")
        Me.Text = LocalizationService.ForSection("Designer.SettingsResetDlg")("Form.Label")
    End Sub

End Class

Partial Class IncompleteSetupDlg

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.IncompleteSetupDlg")("Yes.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.IncompleteSetupDlg")("No.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.IncompleteSetupDlg")("SetupIncomplete.Message")
        Me.Text = LocalizationService.ForSection("Designer.IncompleteSetupDlg")("DISMTools.Label")
    End Sub

End Class

Partial Class PrgSetup

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.PrgSetup")("Set.Up.DISM.Label")
        Me.Back_Button.Text = LocalizationService.ForSection("Designer.PrgSetup")("Back.Button")
        Me.Next_Button.Text = LocalizationService.ForSection("Designer.PrgSetup")("Next.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.PrgSetup")("Cancel.Button")
        Me.Label3.Text = LocalizationService.ForSection("Designer.PrgSetup")("DISM.Tools.Free.Message")
        Me.Label2.Text = LocalizationService.ForSection("Designer.PrgSetup")("Welcome.DISM.Tools.Label")
        Me.Label28.Text = LocalizationService.ForSection("Designer.PrgSetup")("Secondary.Progress.Label")
        Me.Label9.Text = LocalizationService.ForSection("Designer.PrgSetup")("Log.Window.Font.Label")
        Me.Label8.Text = LocalizationService.ForSection("Designer.PrgSetup")("Language.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.PrgSetup")("ColorMode.Label")
        Me.ComboBox1.Items(0) = LocalizationService.ForSection("Designer.PrgSetup")("System.Setting.ThemeItem")
        Me.ComboBox1.Items(1) = LocalizationService.ForSection("Designer.PrgSetup")("LightMode.Item")
        Me.ComboBox1.Items(2) = LocalizationService.ForSection("Designer.PrgSetup")("DarkMode.Item")
        Me.ComboBox1.Text = LocalizationService.ForSection("Designer.PrgSetup")("System.Setting.ThemeItem")
        Me.TextBox1.Text = LocalizationService.ForSection("PrgSetup.LogPreview")("Packages.Add.Message")
        Me.Label29.Text = LocalizationService.ForSection("Designer.PrgSetup")("Font.Readable.Log.Message")
        Me.RadioButton2.Text = LocalizationService.ForSection("Designer.PrgSetup")("Classic.RadioButton")
        Me.RadioButton1.Text = LocalizationService.ForSection("Designer.PrgSetup")("Modern.RadioButton")
        Me.Label5.Text = LocalizationService.ForSection("Designer.PrgSetup")("Yours.Customize.Message")
        Me.Label6.Text = LocalizationService.ForSection("Designer.PrgSetup")("CustomizeProgram.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.PrgSetup")("Default.Log.File.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.PrgSetup")("Browse.Button")
        Me.Label10.Text = LocalizationService.ForSection("Designer.PrgSetup")("LogFile.Label")
        Me.Label16.Text = LocalizationService.ForSection("Designer.PrgSetup")("Log.File.Display.Message")
        Me.Label11.Text = LocalizationService.ForSection("Designer.PrgSetup")("Errors.Warnings.Label")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.PrgSetup")("Auto.Create.Logs.CheckBox")
        Me.Label13.Text = LocalizationService.ForSection("Designer.PrgSetup")("Log.Settings.Message")
        Me.Label14.Text = LocalizationService.ForSection("Designer.PrgSetup")("Log.Label")
        Me.Label15.Text = LocalizationService.ForSection("Designer.PrgSetup")("Windows.ADK.Module.Label")
        Me.Label12.Text = LocalizationService.ForSection("Designer.PrgSetup")("WimlibModule.Label")
        Me.Button3.Text = LocalizationService.ForSection("Designer.PrgSetup")("Install.Button")
        Me.Button4.Text = LocalizationService.ForSection("Designer.PrgSetup")("Install.Button")
        Me.Label17.Text = LocalizationService.ForSection("Designer.PrgSetup")("Module.Install.Isn.Message")
        Me.Label18.Text = LocalizationService.ForSection("Designer.PrgSetup")("DISM.Tools.Supports.Message")
        Me.Label19.Text = LocalizationService.ForSection("Designer.PrgSetup")("ExtendProgram.Label")
        Me.Button5.Text = LocalizationService.ForSection("Designer.PrgSetup")("Configure.Settings.Button")
        Me.Label20.Text = LocalizationService.ForSection("Designer.PrgSetup")("Anything.Like.Label")
        Me.Label21.Text = LocalizationService.ForSection("Designer.PrgSetup")("Settings.Available.Message")
        Me.Label23.Text = LocalizationService.ForSection("Designer.PrgSetup")("Done.Setting.Up.Message")
        Me.Label24.Text = LocalizationService.ForSection("Designer.PrgSetup")("SetupComplete.Label")
        Me.Label26.Text = LocalizationService.ForSection("Designer.PrgSetup")("Stay.Up.Date.Label")
        Me.Label27.Text = LocalizationService.ForSection("Designer.PrgSetup")("Get.Started.DISM.Label")
        Me.Button6.Text = LocalizationService.ForSection("Designer.PrgSetup")("GetStarted.Button")
        Me.Button7.Text = LocalizationService.ForSection("Designer.PrgSetup")("CheckUpdates.Button")
        Me.Label25.Text = LocalizationService.ForSection("Designer.PrgSetup")("Ve.Set.Things.Label")
        Me.Label22.Text = LocalizationService.ForSection("Designer.PrgSetup")("Perform.Steps.Time.Label")
        Me.SaveFileDialog1.Filter = LocalizationService.ForSection("Designer.PrgSetup")("SaveFile.Filter")
        Me.SaveFileDialog1.Title = LocalizationService.ForSection("Designer.PrgSetup")("Log.File.Title")
        Me.Text = LocalizationService.ForSection("Designer.PrgSetup")("Set.Up.DISM.Label")
    End Sub

End Class

Partial Class GetAppxPkgInfoDlg

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label22.Text = LocalizationService.ForSection("Designer.Get.AppX")("PackageName.Label")
        Me.Label23.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label24.Text = LocalizationService.ForSection("Designer.Get.AppX")("Display.Name.Label")
        Me.Label25.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label26.Text = LocalizationService.ForSection("Designer.Get.AppX")("Architecture.Label")
        Me.Label35.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label31.Text = LocalizationService.ForSection("Designer.Get.AppX")("ResourceID.Label")
        Me.Label32.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label41.Text = LocalizationService.ForSection("Designer.Get.AppX")("Version.Label")
        Me.Label40.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label43.Text = LocalizationService.ForSection("Designer.Get.AppX")("Registered.User.Label")
        Me.Label42.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.Get.AppX")("Install.Dir.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.Get.AppX")("Package.Manifest.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label8.Text = LocalizationService.ForSection("Designer.Get.AppX")("StoreLogo.Asset.Dir.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.Get.AppX")("DynamicValue.Label")
        Me.Label9.Text = LocalizationService.ForSection("Designer.Get.AppX")("Main.StoreLogo.Asset.Label")
        Me.Label10.Text = LocalizationService.ForSection("Designer.Get.AppX")("Asset.Guessed.DISM.Message")
        Me.LinkLabel1.Text = LocalizationService.ForSection("Designer.Get.AppX")("Asset.One.IM.Link")
        Me.Label36.Text = LocalizationService.ForSection("Designer.Get.AppX")("AppX.Package.Label")
        Me.Label37.Text = LocalizationService.ForSection("Designer.Get.AppX")("Installed.AppX.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.Get.AppX")("Save.Button")
        Me.Text = LocalizationService.ForSection("Designer.Get.AppX")("AppX.Package.Get.Label")
    End Sub

End Class

Partial Class GetCapabilityInfoDlg

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label2.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("Ready.Label")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("Identity.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("State.Column")
        Me.Label22.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("Identity.Label")
        Me.Label23.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("DynamicValue.Label")
        Me.Label24.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("CapabilityName.Label")
        Me.Label25.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("DynamicValue.Label")
        Me.Label26.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("CapabilityState.Label")
        Me.Label35.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("DynamicValue.Label")
        Me.Label31.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("DisplayName.Label")
        Me.Label32.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("DynamicValue.Label")
        Me.Label41.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("Description.Label")
        Me.Label40.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("DynamicValue.Label")
        Me.Label43.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("Sizes.Label")
        Me.Label42.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("DynamicValue.Label")
        Me.Label36.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("CapabilityInfo.Label")
        Me.Label37.Text = LocalizationService.ForSection("Designer.GetCapInfo")("SelectCapability.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("Save.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("Look.Item.Online.Button")
        Me.Text = LocalizationService.ForSection("Designer.CapabilityInfo")("Get.Label")
    End Sub

End Class

Partial Class DriverFileInfoDlg

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.DriverFileInfo")("Ok.Button")
        Me.Copy_Button.Text = LocalizationService.ForSection("Designer.DriverFileInfo")("Copy.Button")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.DriverFileInfo")("Property.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.DriverFileInfo")("Value.Column")
        Me.Label1.Text = LocalizationService.ForSection("Designer.DriverFileInfo")("Driver.File.Label")
        Me.Text = LocalizationService.ForSection("Designer.DriverFileInfo")("Driver.File.Label.Label")
    End Sub

End Class

Partial Class GetDriverInfo

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Button9.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("View.Driver.File.Button")
        Me.Button7.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Change.Button")
        Me.Label48.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Bg.Procs.Notice.Message")
        Me.Button8.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Save.Button")
        Me.Label5.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Status.Label")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("PublishedName.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Original.File.Name.Column")
        Me.Label22.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("PublishedName.Label")
        Me.Label23.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label24.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Original.File.Name.Label")
        Me.Label25.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label26.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("ProviderName.Label")
        Me.Label35.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label31.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("ClassName.Label")
        Me.Label32.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label41.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("ClassDescription.Label")
        Me.Label40.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label43.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("ClassGUID.Label")
        Me.Label42.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label47.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Catalog.File.Path.Label")
        Me.Label46.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label33.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Part.Windows.Label")
        Me.Label34.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label28.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Critical.Boot.Process.Label")
        Me.Label27.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label30.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Version.Label")
        Me.Label29.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label39.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Date.Label")
        Me.Label38.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label45.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Driver.Signature.Label")
        Me.Label44.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label36.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DriverInfo.Label")
        Me.Label37.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Installed.Driver.View.Label")
        Me.Button3.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("RemoveAll.Button")
        Me.Button2.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("RemoveSelected.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("AddDriver.Button")
        Me.Label8.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Hardware.Description.Label")
        Me.Label9.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label10.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("HardwareID.Label")
        Me.Label11.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label12.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("AdditionalIds.Label")
        Me.Label13.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("CompatibleIds.Label")
        Me.Label14.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label16.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("ExcludeIds.Label")
        Me.Label15.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label17.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Hardware.Manufacturer.Label")
        Me.Label18.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label20.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Architecture.Label")
        Me.Label19.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("DynamicValue.Label")
        Me.Label21.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("JumpTarget.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("HardwareTargets.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Add.DriverPackage.Label")
        Me.LinkLabel1.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("GoBack.Link")
        Me.Label4.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Help.AddDrivers.Message")
        Me.Label3.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Get.Drivers.Message")
        Me.InstalledDriverLink.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("InstalledDriver.Link")
        Me.DriverFileLink.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Iwant.Link")
        Me.Label2.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Get.Label")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.GetDriverInfo")("Driver.Files.Inf.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.GetDriverInfo")("Locate.Driver.Files.Title")
        Me.Text = LocalizationService.ForSection("Designer.GetDriverInfo")("Driver.Label")
    End Sub

End Class

Partial Class GetFeatureInfoDlg

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("FeatureName.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("FeatureState.Column")
        Me.Label22.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("FeatureName.Label")
        Me.Label23.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("DynamicValue.Label")
        Me.Label24.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("DisplayName.Label")
        Me.Label25.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("DynamicValue.Label")
        Me.Label26.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("Description.Label")
        Me.Label35.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("DynamicValue.Label")
        Me.Label31.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("RestartRequired.Label")
        Me.Label32.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("DynamicValue.Label")
        Me.Label41.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("FeatureState.Label")
        Me.Label40.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("DynamicValue.Label")
        Me.Label43.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("CustomProps.Label")
        Me.Label42.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("DynamicValue.Label")
        Me.Label36.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("FeatureInfo.Label")
        Me.Label37.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("Installed.Left.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("Ready.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("Save.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("Look.Item.Online.Button")
        Me.Text = LocalizationService.ForSection("Designer.GetFeatureInfo")("Get.Feature.Label")
    End Sub

End Class

Partial Class AppxFilterAssistantDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("Apply.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("Clear.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("FilterBy.Label")
        Me.NameFilterRadioButton.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("Name.RadioButton")
        Me.RegStatusRadioButton.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("RegistrationStatus.RadioButton")
        Me.Label2.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("RegisteredTo.Label")
        Me.RegStatusComboBox.Items(0) = LocalizationService.ForSection("Designer.AppxFilterAssistant")("RegisteredToNoOne.Item")
        Me.RegStatusComboBox.Items(1) = LocalizationService.ForSection("Designer.AppxFilterAssistant")("RegisteredToAnyone.Item")
        Me.RegStatusComboBox.Items(2) = LocalizationService.ForSection("Designer.AppxFilterAssistant")("RegisteredToMe.Item")
        Me.RegStatusComboBox.Items(3) = LocalizationService.ForSection("Designer.AppxFilterAssistant")("RegisteredToUser.Item")
        Me.RegStatusComboBox.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("RegisteredToMe.Item")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("AccountName.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("DisplayName.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("Sid.Column")
        Me.Label3.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("SelectUser.Message")
        Me.Text = LocalizationService.ForSection("Designer.AppxFilterAssistant")("Title")
    End Sub

End Class

Partial Class CapabilityFilterAssistantDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.CapabilityFilter")("Apply.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.CapabilityFilter")("Clear.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.CapabilityFilter")("Name.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.CapabilityFilter")("State.Label")
        Me.ComboBox1.Items(0) = LocalizationService.ForSection("Designer.CapabilityFilter")("AnyState.Item")
        Me.ComboBox1.Items(1) = LocalizationService.ForSection("Designer.CapabilityFilter")("Installed.Item")
        Me.ComboBox1.Items(2) = LocalizationService.ForSection("Designer.CapabilityFilter")("Install.Pending.Item")
        Me.ComboBox1.Items(3) = LocalizationService.ForSection("Designer.CapabilityFilter")("Removed.Item")
        Me.Label1.Text = LocalizationService.ForSection("Designer.CapabilityFilter")("FilterInfo.Prompt.Label")
        Me.Text = LocalizationService.ForSection("Designer.CapabilityFilter")("FilterInfo.Title")
    End Sub

End Class

Partial Class DriverFilterAssistantDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.DriverFilter")("Apply.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.DriverFilter")("Clear.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.DriverFilter")("FilterPrompt.Label")
        Me.ComboBox1.Items(0) = LocalizationService.ForSection("Designer.DriverFilter")("PublishedName.Item")
        Me.ComboBox1.Items(1) = LocalizationService.ForSection("Designer.DriverFilter")("Original.File.Name.Item")
        Me.ComboBox1.Items(2) = LocalizationService.ForSection("Designer.DriverFilter")("ProviderName.Item")
        Me.ComboBox1.Items(3) = LocalizationService.ForSection("Designer.DriverFilter")("ClassName.Item")
        Me.ComboBox1.Items(4) = LocalizationService.ForSection("Designer.DriverFilter")("InboxStatus.Item")
        Me.ComboBox1.Items(5) = LocalizationService.ForSection("Designer.DriverFilter")("Boot.Critical.Status.Item")
        Me.ComboBox1.Items(6) = LocalizationService.ForSection("Designer.DriverFilter")("SignatureStatus.Item")
        Me.ComboBox1.Items(7) = LocalizationService.ForSection("Designer.DriverFilter")("Date.Item")
        Me.Label13.Text = LocalizationService.ForSection("Designer.DriverFilter")("MonthName.Label")
        Me.ComboBox4.Items(0) = LocalizationService.ForSection("Designer.DriverFilter")("Year.Item")
        Me.ComboBox4.Items(1) = LocalizationService.ForSection("Designer.DriverFilter")("Month.Item")
        Me.ComboBox4.Items(2) = LocalizationService.ForSection("Designer.DriverFilter")("Date.Item")
        Me.ComboBox3.Items(0) = LocalizationService.ForSection("Designer.DriverFilter")("Released.Item")
        Me.ComboBox3.Items(1) = LocalizationService.ForSection("Designer.DriverFilter")("NotReleased.Item")
        Me.ComboBox3.Items(2) = LocalizationService.ForSection("Designer.DriverFilter")("ReleasedBefore.Item")
        Me.ComboBox3.Items(3) = LocalizationService.ForSection("Designer.DriverFilter")("ReleasedOnBefore.Item")
        Me.ComboBox3.Items(4) = LocalizationService.ForSection("Designer.DriverFilter")("ReleasedAfter.Item")
        Me.ComboBox3.Items(5) = LocalizationService.ForSection("Designer.DriverFilter")("ReleasedOnAfter.Item")
        Me.Label12.Text = LocalizationService.ForSection("Designer.DriverFilter")("Date.Label")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.DriverFilter")("Search.Signed.CheckBox")
        Me.Label11.Text = LocalizationService.ForSection("Designer.DriverFilter")("SignatureStatus.Label")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.DriverFilter")("Search.BootCritical.CheckBox")
        Me.Label10.Text = LocalizationService.ForSection("Designer.DriverFilter")("Boot.Critical.Status.Label")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.DriverFilter")("Search.Inbox.CheckBox")
        Me.Label9.Text = LocalizationService.ForSection("Designer.DriverFilter")("InboxStatus.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.DriverFilter")("ClassName.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.DriverFilter")("Class.Name.Notes.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.DriverFilter")("ProviderName.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.DriverFilter")("Original.File.Name.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.DriverFilter")("PublishedName.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.DriverFilter")("Driver.Searches.Choose.Label")
        Me.Text = LocalizationService.ForSection("Designer.DriverFilter")("Title")
    End Sub

End Class

Partial Class FeatureFilterAssistantDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.FeatureFilter")("Apply.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.FeatureFilter")("Clear.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.FeatureFilter")("Filter.Feature.Prompt.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.FeatureFilter")("Name.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.FeatureFilter")("State.Label")
        Me.ComboBox1.Items(0) = LocalizationService.ForSection("Designer.FeatureFilter")("AnyState.Item")
        Me.ComboBox1.Items(1) = LocalizationService.ForSection("Designer.FeatureFilter")("Enabled.Item")
        Me.ComboBox1.Items(2) = LocalizationService.ForSection("Designer.FeatureFilter")("Enablement.Pending.Item")
        Me.ComboBox1.Items(3) = LocalizationService.ForSection("Designer.FeatureFilter")("Disabled.Item")
        Me.ComboBox1.Items(4) = LocalizationService.ForSection("Designer.FeatureFilter")("Disablement.Pending.Item")
        Me.Text = LocalizationService.ForSection("Designer.FeatureFilter")("Filter.Feature.Title")
    End Sub

End Class

Partial Class GetImgInfoDlg

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.Get.Img")("WIM.Files.Wimvirtual.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.Get.Img")("Image.Title")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.Get.Img")("Index.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.Get.Img")("ImageName.Column")
        Me.Button3.Text = LocalizationService.ForSection("Designer.Get.Img")("Pick.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.Get.Img")("Browse.Button")
        Me.RadioButton2.Text = LocalizationService.ForSection("Designer.Get.Img")("AnotherImage.RadioButton")
        Me.RadioButton1.Text = LocalizationService.ForSection("Designer.Get.Img")("CurrentlyMounted.RadioButton")
        Me.Label3.Text = LocalizationService.ForSection("Designer.Get.Img")("List.Indexes.ImageFile.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.Get.Img")("ImageFile.Label")
        Me.Label22.Text = LocalizationService.ForSection("Designer.Get.Img")("ImageVersion.Label")
        Me.Label23.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label24.Text = LocalizationService.ForSection("Designer.Get.Img")("ImageName.Label")
        Me.Label25.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label26.Text = LocalizationService.ForSection("Designer.Get.Img")("ImageDescription.Label")
        Me.Label35.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label31.Text = LocalizationService.ForSection("Designer.Get.Img")("ImageSize.Label")
        Me.Label32.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label41.Text = LocalizationService.ForSection("Designer.Get.Img")("Supports.WIM.Boot.Label")
        Me.Label40.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label43.Text = LocalizationService.ForSection("Designer.Get.Img")("Architecture.Label")
        Me.Label42.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label47.Text = LocalizationService.ForSection("Designer.Get.Img")("HAL.Label")
        Me.Label46.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label33.Text = LocalizationService.ForSection("Designer.Get.Img")("ServicePackBuild.Label")
        Me.Label34.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label28.Text = LocalizationService.ForSection("Designer.Get.Img")("ServicePackLevel.Label")
        Me.Label27.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label30.Text = LocalizationService.ForSection("Designer.Get.Img")("InstallationType.Label")
        Me.Label29.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label39.Text = LocalizationService.ForSection("Designer.Get.Img")("Edition.Label")
        Me.Label38.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label45.Text = LocalizationService.ForSection("Designer.Get.Img")("ProductType.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.Get.Img")("ProductSuite.Label")
        Me.Label44.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.Get.Img")("System.Root.Dir.Label")
        Me.Label8.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label9.Text = LocalizationService.ForSection("Designer.Get.Img")("FileCount.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label11.Text = LocalizationService.ForSection("Designer.Get.Img")("Dates.Label")
        Me.Label10.Text = LocalizationService.ForSection("Designer.Get.Img")("DynamicValue.Label")
        Me.Label13.Text = LocalizationService.ForSection("Designer.Get.Img")("Installed.Languages.Label")
        Me.Label36.Text = LocalizationService.ForSection("Designer.Get.Img")("ImageInfo.Label")
        Me.Label37.Text = LocalizationService.ForSection("Designer.Get.Img")("Index.List.View.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.Get.Img")("Save.Button")
        Me.Text = LocalizationService.ForSection("Designer.Get.Img")("Image.Label")
    End Sub

End Class

Partial Class ImgInfoSaveDlg

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label2.Text = LocalizationService.ForSection("Designer.Img.Save")("Status.Label")
        Me.Label1.Text = LocalizationService.ForSection("Designer.Img.Save")("Wait.Message")
        Me.Text = LocalizationService.ForSection("Designer.Img.Save")("Saving.Image.Button")
    End Sub

End Class

Partial Class InfoSaveResults

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.InfoSaveResults")("ReportSaved.Message")
        Me.Button1.Text = LocalizationService.ForSection("Designer.InfoSaveResults")("Ok.Button")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.InfoSaveResults")("Display.Content.Web.CheckBox")
        Me.Button2.Text = LocalizationService.ForSection("Designer.InfoSaveResults")("SaveReport.Button")
        Me.SaveFileDialog1.Filter = LocalizationService.ForSection("Designer.InfoSaveResults")("Htmlreports.Filter")
        Me.Text = LocalizationService.ForSection("Designer.InfoSaveResults")("Image.Report.Label")
    End Sub

End Class

Partial Class GetPkgInfoDlg

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label4.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("AddPackages.Help.Message")
        Me.Label3.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Get.Packages.Message")
        Me.InstalledPackageLink.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("InstalledPackage.Link")
        Me.PackageFileLink.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("PackageFile.Link")
        Me.Label2.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Get.Label")
        Me.Button4.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Save.Button")
        Me.Label5.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Status.Label")
        Me.Label22.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("PackageName.Label")
        Me.Label23.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label24.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Package.Applicable.Label")
        Me.Label25.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label26.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Copyright.Label")
        Me.Label35.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label31.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Company.Label")
        Me.Label32.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label41.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("CreationTime.Label")
        Me.Label40.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label43.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Description.Label")
        Me.Label42.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label47.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("InstallClient.Label")
        Me.Label46.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label33.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Install.Package.Name.Label")
        Me.Label34.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label28.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("InstallTime.Label")
        Me.Label27.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label30.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Last.Update.Time.Label")
        Me.Label29.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label39.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DisplayName.Label")
        Me.Label38.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label45.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("ProductName.Label")
        Me.Label44.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label14.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("ProductVersion.Label")
        Me.Label15.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label16.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("ReleaseType.Label")
        Me.Label21.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label48.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("RestartRequired.Label")
        Me.Label13.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label50.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("SupportInfo.Label")
        Me.Label49.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label52.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("State.Label")
        Me.Label51.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label54.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Boot.Up.Required.Label")
        Me.Label53.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label61.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Capability.Identity.Label")
        Me.Label56.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label58.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("CustomProps.Label")
        Me.Label57.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label60.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Features.Label")
        Me.Label59.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label36.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("PackageInfo.Label")
        Me.Label37.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Installed.Package.View.Label")
        Me.Button3.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("RemoveAll.Button")
        Me.Button2.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("RemoveSelected.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("AddPackage.Button")
        Me.Label8.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("PackageName.Label")
        Me.Label9.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label10.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Package.Applicable.Label")
        Me.Label11.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label12.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Copyright.Label")
        Me.Label17.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label18.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Company.Label")
        Me.Label19.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label20.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("CreationTime.Label")
        Me.Label62.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label63.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Description.Label")
        Me.Label64.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label65.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("InstallClient.Label")
        Me.Label66.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label67.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Install.Package.Name.Label")
        Me.Label68.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label69.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("InstallTime.Label")
        Me.Label70.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label71.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Last.Update.Time.Label")
        Me.Label72.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label73.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DisplayName.Label")
        Me.Label74.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label75.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("ProductName.Label")
        Me.Label76.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label77.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("ProductVersion.Label")
        Me.Label78.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label79.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("ReleaseType.Label")
        Me.Label80.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label81.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("RestartRequired.Label")
        Me.Label82.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label83.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("SupportInfo.Label")
        Me.Label84.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label85.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("State.Label")
        Me.Label86.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label87.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Boot.Up.Required.Label")
        Me.Label88.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label89.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Capability.Identity.Label")
        Me.Label90.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label91.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("CustomProps.Label")
        Me.Label92.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label93.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Features.Label")
        Me.Label94.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("DynamicValue.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("PackageInfo.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Add.Package.File.Label")
        Me.LinkLabel1.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("GoBack.Link")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.GetPkgInfo")("Cabfiles.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.GetPkgInfo")("Locate.Package.Files.Title")
        Me.Text = LocalizationService.ForSection("Designer.GetPkgInfo")("Package.Label")
    End Sub

End Class

Partial Class GetWinPESettings

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.WinPESettings")("Ok.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.WinPESettings")("Windows.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.WinPESettings")("Change.Button")
        Me.Button2.Text = LocalizationService.ForSection("Designer.WinPESettings")("Change.Button")
        Me.Label3.Text = LocalizationService.ForSection("Designer.WinPESettings")("TargetPath.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.WinPESettings")("ScratchSpace.Label")
        Me.Button4.Text = LocalizationService.ForSection("Designer.WinPESettings")("Save.Button")
        Me.Text = LocalizationService.ForSection("Designer.WinPESettings")("Get.Windows.Pesettings.Label")
    End Sub

End Class

Partial Class ImageTaskHeader

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.ItemTitle.Text = LocalizationService.ForSection("Designer.ImageTaskHeader")("ItemText.Title")
    End Sub

End Class

Partial Class ActiveInstAccessWarn

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ActiveInstall")("Continue.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ActiveInstall")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.ActiveInstall")("Enter.Online.Message")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ActiveInstall")("ProjectUnloaded.Label")
        Me.Text = LocalizationService.ForSection("Designer.ActiveInstall")("Active.Install.Label")
    End Sub

End Class

Partial Class AddProvAppxPackage

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.AppxProvision")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.AppxProvision")("Cancel.Button")
        Me.Label6.Text = LocalizationService.ForSection("Designer.AppxProvision")("Entry.List.View.Message")
        Me.Label9.Text = LocalizationService.ForSection("Designer.AppxProvision")("AppxVersion.Label")
        Me.Label8.Text = LocalizationService.ForSection("Designer.AppxProvision")("AppxPublisher.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.AppxProvision")("AppxTitle.Label")
        Me.Button9.Text = LocalizationService.ForSection("Designer.AppxProvision")("Remove.Selected.Entry.Button")
        Me.Button3.Text = LocalizationService.ForSection("Designer.AppxProvision")("Remove.Entries.Button")
        Me.Button2.Text = LocalizationService.ForSection("Designer.AppxProvision")("AddFolder.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.AppxProvision")("AddFile.Button")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.AppxProvision")("FileFolder.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.AppxProvision")("Type.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.AppxProvision")("ApplicationName.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.AppxProvision")("App.Publisher.Column")
        Me.ColumnHeader5.Text = LocalizationService.ForSection("Designer.AppxProvision")("App.Version.Column")
        Me.Label2.Text = LocalizationService.ForSection("Designer.AppxProvision")("Packages.Required.Message")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.AppxProvision")("AppxDependencies.Group")
        Me.Button4.Text = LocalizationService.ForSection("Designer.AppxProvision")("Remove.Dependencies.Button")
        Me.Button5.Text = LocalizationService.ForSection("Designer.AppxProvision")("RemoveDependency.Button")
        Me.Button6.Text = LocalizationService.ForSection("Designer.AppxProvision")("AddDependency.Button")
        Me.Label3.Text = LocalizationService.ForSection("Designer.AppxProvision")("Package.Message")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.AppxProvision")("CustomDataFile.CheckBox")
        Me.Button7.Text = LocalizationService.ForSection("Designer.AppxProvision")("Browse.Button")
        Me.Button8.Text = LocalizationService.ForSection("Designer.AppxProvision")("Browse.Button")
        Me.GroupBox3.Text = LocalizationService.ForSection("Designer.AppxProvision")("AppxRegions.Group")
        Me.CheckBox4.Text = LocalizationService.ForSection("Designer.AppxProvision")("App.Available.CheckBox")
        Me.LinkLabel1.Text = LocalizationService.ForSection("Designer.AppxProvision")("App.Regions.Form.Message")
        Me.Label5.Text = LocalizationService.ForSection("Designer.AppxProvision")("Multiple.App.Regions.Label")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.AppxProvision")("CommitImage.CheckBox")
        Me.AppxFileOFD.Filter = LocalizationService.ForSection("Designer.AppxProvision")("MSIX.Packages.Filter")
        Me.AppxFileOFD.Title = LocalizationService.ForSection("Designer.AppxProvision")("Files.Title")
        Me.AppxDependencyOFD.Filter = LocalizationService.ForSection("Designer.AppxProvision")("DependencyFiles.Filter")
        Me.AppxDependencyOFD.Title = LocalizationService.ForSection("Designer.AppxProvision")("Browse.Dependencies.Title")
        Me.LicenseFileOFD.Filter = LocalizationService.ForSection("Designer.AppxProvision")("Xmllicenses.Filter")
        Me.LicenseFileOFD.Title = LocalizationService.ForSection("Designer.AppxProvision")("LicenseFile.Title")
        Me.CustomDataFileOFD.Filter = LocalizationService.ForSection("Designer.AppxProvision")("CustomData.Filter")
        Me.CustomDataFileOFD.Title = LocalizationService.ForSection("Designer.AppxProvision")("CustomData.File.Title")
        Me.UnpackedAppxFolderFBD.Description = LocalizationService.ForSection("Designer.AppxProvision")("Folder.Required.Description")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.AppxProvision")("LicenseFile.CheckBox")
        Me.ComboBox1.Items(0) = LocalizationService.ForSection("Designer.AppxProvision")("Configure.Stub.Item")
        Me.ComboBox1.Items(1) = LocalizationService.ForSection("Designer.AppxProvision")("Install.Stub.Package.Item")
        Me.ComboBox1.Items(2) = LocalizationService.ForSection("Designer.AppxProvision")("Install.Full.Package.Item")
        Me.ComboBox1.Text = LocalizationService.ForSection("Designer.AppxProvision")("Configure.Stub.Item")
        Me.Label4.Text = LocalizationService.ForSection("Designer.AppxProvision")("StubPreference.Label")
        Me.Button10.Text = LocalizationService.ForSection("Designer.AppxProvision")("Value.Button")
        Me.Text = LocalizationService.ForSection("Designer.AppxProvision")("Add.Prov.Label")
    End Sub

End Class

Partial Class AppInstallerDownloader

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.AppInstaller")("Wait.Message")
        Me.StatusLbl.Text = LocalizationService.ForSection("Designer.AppInstaller")("StatusLbl.Label")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.AppInstaller")("TransferDetails.Group")
        Me.downETALbl.Text = LocalizationService.ForSection("Designer.AppInstaller")("TimeRemaining.Label")
        Me.downSpdLbl.Text = LocalizationService.ForSection("Designer.AppInstaller")("DownloadSpeed.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.AppInstaller")("DownloadURL.Label")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.AppInstaller")("Cancel.Button")
        Me.Label3.Text = LocalizationService.ForSection("Designer.AppInstaller")("Wait.Label")
        Me.CopyUri_Button.Text = LocalizationService.ForSection("Designer.AppInstaller")("CopyURI.Button")
        Me.Text = LocalizationService.ForSection("Designer.AppInstaller")("DownloadPackage.Button")
    End Sub

End Class

Partial Class RemProvAppxPackage

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.RemoveAppx")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.RemoveAppx")("Cancel.Button")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.RemoveAppx")("PackageName.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.RemoveAppx")("App.Display.Name.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.RemoveAppx")("Architecture.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.RemoveAppx")("ResourceID.Column")
        Me.ColumnHeader5.Text = LocalizationService.ForSection("Designer.RemoveAppx")("Version.Column")
        Me.ColumnHeader6.Text = LocalizationService.ForSection("Designer.RemoveAppx")("Registered.User.Column")
        Me.Text = LocalizationService.ForSection("Designer.RemoveAppx")("Prov.Label")
    End Sub

End Class

Partial Class BGProcDetails

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.BgprocDetails")("Gathering.Image.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.BgprocDetails")("InfoTask.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.BgprocDetails")("Processes.Take.Time.Label")
        Me.Text = LocalizationService.ForSection("Designer.BgprocDetails")("DISMTools.Label")
    End Sub

End Class

Partial Class BGProcFailureDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.BgprocFailure")("Ok.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.BgprocFailure")("Run.Issues.Message")
        Me.Text = LocalizationService.ForSection("Designer.BgprocFailure")("Failed.Bg.Procs.Label")
    End Sub

End Class

Partial Class BGProcNotify

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.BgprocNotify")("Project.Loaded.Done.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.BgprocNotify")("Gathering.Image.Label")
    End Sub

End Class

Partial Class BGProcsBusyDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.BgProcessesBusy")("Ok.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.BgProcessesBusy")("Finish.Process.Begin.Message")
        Me.Label1.Text = LocalizationService.ForSection("Designer.BgProcessesBusy")("Re.Still.Gathering.Label")
        Me.Text = LocalizationService.ForSection("Designer.BgProcessesBusy")("DISMTools.Label")
    End Sub

End Class

Partial Class AddCapabilities

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.AddCapabilities")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.AddCapabilities")("Cancel.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.AddCapabilities")("Capabilities.Group")
        Me.Button2.Text = LocalizationService.ForSection("Designer.AddCapabilities")("SelectAll.Button")
        Me.Button3.Text = LocalizationService.ForSection("Designer.AddCapabilities")("SelectNone.Button")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.AddCapabilities")("Capability.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.AddCapabilities")("State.Column")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.AddCapabilities")("Options.Group")
        Me.Button1.Text = LocalizationService.ForSection("Designer.AddCapabilities")("Browse.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.AddCapabilities")("Source.Label")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.AddCaps")("CommitImage.CheckBox")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.AddCapabilities")("WindowsUpdate.CheckBox")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.AddCapabilities")("DifferentSource.CheckBox")
        Me.Button4.Text = LocalizationService.ForSection("Designer.AddCapabilities")("Detect.Group.Policy.Button")
        Me.FolderBrowserDialog1.Description = LocalizationService.ForSection("Designer.AddCapabilities")("SourceHint.Description")
        Me.Text = LocalizationService.ForSection("Designer.AddCapabilities")("AddCapabilities.Label")
    End Sub

End Class

Partial Class RemCapabilities

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.RemCapabilities")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.RemCapabilities")("Cancel.Button")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.RemCapabilities")("Capability.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.RemCapabilities")("State.Column")
        Me.Text = LocalizationService.ForSection("Designer.RemCapabilities")("Remove.Label")
    End Sub

End Class

Partial Class ImgCleanup

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ImgCleanup")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ImgCleanup")("Cancel.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ImgCleanup")("Task.Choose.Label")
        Me.ComboBox1.Items(0) = LocalizationService.ForSection("Designer.ImgCleanup")("Revert.Pending.Actions.Item")
        Me.ComboBox1.Items(1) = LocalizationService.ForSection("Designer.ImgCleanup")("Clean.Up.ServicePack.Item")
        Me.ComboBox1.Items(2) = LocalizationService.ForSection("Designer.ImgCleanup")("Clean.Up.Component.Item")
        Me.ComboBox1.Items(3) = LocalizationService.ForSection("Designer.ImgCleanup")("Analyze.Component.Store.Item")
        Me.ComboBox1.Items(4) = LocalizationService.ForSection("Designer.ImgCleanup")("Check.Component.Store.Item")
        Me.ComboBox1.Items(5) = LocalizationService.ForSection("Designer.ImgCleanup")("Scan.Comp.Store.Item")
        Me.ComboBox1.Items(6) = LocalizationService.ForSection("Designer.ImgCleanup")("Repair.Component.Store.Item")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.ImgCleanup")("TaskOptions.Group")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ImgCleanup")("NoOptions.Message")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.ImgCleanup")("HideServicePack.CheckBox")
        Me.Label6.Text = LocalizationService.ForSection("Designer.ImgCleanup")("Last.Reset.Base.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.ImgCleanup")("Only.Check.Option.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.ImgCleanup")("Superseded.Base.Reset.Label")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.ImgCleanup")("Defer.Long.Running.CheckBox")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.ImgCleanup")("Reset.Base.CheckBox")
        Me.Label8.Text = LocalizationService.ForSection("Designer.ImgCleanup")("NoOptions.Label")
        Me.Label9.Text = LocalizationService.ForSection("Designer.ImgCleanup")("NoOptions.Label")
        Me.Label10.Text = LocalizationService.ForSection("Designer.ImgCleanup")("NoOptions.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ImgCleanup")("Browse.Button")
        Me.Label11.Text = LocalizationService.ForSection("Designer.ImgCleanup")("Source.Label")
        Me.CheckBox5.Text = LocalizationService.ForSection("Designer.ImgCleanup")("WindowsUpdate.CheckBox")
        Me.CheckBox4.Text = LocalizationService.ForSection("Designer.ImgCleanup")("Different.Source.CheckBox")
        Me.Button2.Text = LocalizationService.ForSection("Designer.ImgCleanup")("Detect.Group.Policy.Button")
        Me.Label12.Text = LocalizationService.ForSection("Designer.ImgCleanup")("Task.Listed.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ImgCleanup")("Task.See.Choose.Label")
        Me.HealthRestoreSourceOFD.Filter = LocalizationService.ForSection("Designer.ImgCleanup")("WIM.Files.Wimesd.Filter")
        Me.HealthRestoreSourceOFD.Title = LocalizationService.ForSection("Designer.ImgCleanup")("Source.Title")
        Me.Text = LocalizationService.ForSection("Designer.ImgCleanup")("ImageCleanup.Label")
    End Sub

End Class

Partial Class ImgWim2Esd

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.Img.WIM")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.Img.WIM")("Cancel.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.Img.WIM")("Source.Group")
        Me.Button1.Text = LocalizationService.ForSection("Designer.Img.WIM")("Browse.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.Img.WIM")("SourceImageFile.Label")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.Img.WIM")("Options.Group")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.Img.WIM")("Index.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.Img.WIM")("ImageName.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.Img.WIM")("ImageDescription.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.Img.WIM")("ImageVersion.Column")
        Me.Label7.Text = LocalizationService.ForSection("Designer.Img.WIM")("Index.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.Img.WIM")("Format.Converted.Image.Label")
        Me.LinkLabel1.Text = LocalizationService.ForSection("Designer.Img.WIM")("Format.Ichoose.Link")
        Me.GroupBox3.Text = LocalizationService.ForSection("Designer.Img.WIM")("Destination.Group")
        Me.Button2.Text = LocalizationService.ForSection("Designer.Img.WIM")("Browse.Button")
        Me.Label5.Text = LocalizationService.ForSection("Designer.Img.WIM")("Destination.ImageFile.Label")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.Img.WIM")("OpenFile.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.Img.WIM")("Source.ImageFile.Title")
        Me.SaveFileDialog1.Title = LocalizationService.ForSection("Designer.Img.WIM")("Target.Image.Stored.Title")
        Me.Text = LocalizationService.ForSection("Designer.Img.WIM")("ConvertImage.Label")
    End Sub

End Class

Partial Class AddDrivers

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.AddDrivers")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.AddDrivers")("Cancel.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.AddDrivers")("DriverFiles.Group")
        Me.Label2.Text = LocalizationService.ForSection("Designer.AddDrivers")("Drivers.Required.Message")
        Me.Button4.Text = LocalizationService.ForSection("Designer.AddDrivers")("Remove.Selected.Entry.Button")
        Me.Button3.Text = LocalizationService.ForSection("Designer.AddDrivers")("Remove.Entries.Button")
        Me.Button2.Text = LocalizationService.ForSection("Designer.AddDrivers")("AddFolder.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.AddDrivers")("AddFile.Button")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.AddDrivers")("FileFolder.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.AddDrivers")("Type.Column")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.AddDrivers")("DriverFolders.Group")
        Me.Label3.Text = LocalizationService.ForSection("Designer.AddDrivers")("Scan.Driver.Message")
        Me.GroupBox3.Text = LocalizationService.ForSection("Designer.AddDrivers")("Options.Group")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.AddDrivers")("CommitImage.CheckBox")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.AddDrivers")("Force.Install.CheckBox")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.AddDrivers")("Driver.Files.Inf.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.AddDrivers")("DriverPackage.Title")
        Me.FolderBrowserDialog1.Description = LocalizationService.ForSection("Designer.AddDrivers")("DriverFolder.Description")
        Me.Text = LocalizationService.ForSection("Designer.AddDrivers")("AddDrivers.Label")
    End Sub

End Class

Partial Class DriverManualFilePicker

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.DriverFilePicker")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.DriverFilePicker")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.DriverFilePicker")("RecursiveListing.Message")
        Me.Button1.Text = LocalizationService.ForSection("Designer.DriverFilePicker")("Refresh.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.DriverFilePicker")("DirectoryStatus.Label")
        Me.Text = LocalizationService.ForSection("Designer.DriverFilePicker")("Driver.Files.Choose.Label")
    End Sub

End Class

Partial Class ExportDrivers

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ExportDrivers")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ExportDrivers")("Cancel.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ExportDrivers")("ExportTarget.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ExportDrivers")("Browse.Button")
        Me.FolderBrowserDialog1.Description = LocalizationService.ForSection("Designer.ExportDrivers")("DriversPath.Description")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.ExportDrivers")("Driver.Mode.Group")
        Me.Button3.Text = LocalizationService.ForSection("Designer.ExportDrivers")("Remove.ClassName.Button")
        Me.Button2.Text = LocalizationService.ForSection("Designer.ExportDrivers")("Add.ClassName.Button")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.ExportDrivers")("Organize.ClassNames.CheckBox")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ExportDrivers")("ClassName.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ExportDrivers")("Class.Name.Notes.Label")
        Me.RadioButton2.Text = LocalizationService.ForSection("Designer.ExportDrivers")("Matching.Drivers.RadioButton")
        Me.RadioButton1.Text = LocalizationService.ForSection("Designer.ExportDrivers")("Image.Drivers.RadioButton")
        Me.Text = LocalizationService.ForSection("Designer.ExportDrivers")("ExportDrivers.Label")
    End Sub

End Class

Partial Class ImportDrivers

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ImportDrivers")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ImportDrivers")("Cancel.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ImportDrivers")("Process.Third.Message")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ImportDrivers")("ImportSource.Label")
        Me.ComboBox1.Items(0) = LocalizationService.ForSection("Designer.ImportDrivers")("Windows.Item")
        Me.ComboBox1.Items(1) = LocalizationService.ForSection("Designer.ImportDrivers")("Online.Install.Item")
        Me.ComboBox1.Items(2) = LocalizationService.ForSection("Designer.ImportDrivers")("Offline.Install.Item")
        Me.Label10.Text = LocalizationService.ForSection("Designer.ImportDrivers")("ImgFile.Label")
        Me.Label9.Text = LocalizationService.ForSection("Designer.ImportDrivers")("ImageFile.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.ImportDrivers")("Tuse.Target.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ImportDrivers")("Pick.Button")
        Me.Label5.Text = LocalizationService.ForSection("Designer.ImportDrivers")("Windows.Label")
        Me.Label8.Text = LocalizationService.ForSection("Designer.ImportDrivers")("Tuse.Target.Label")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.ImportDrivers")("DriveLetter.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.ImportDrivers")("DriveLabel.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.ImportDrivers")("DriveType.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.ImportDrivers")("TotalSize.Column")
        Me.ColumnHeader5.Text = LocalizationService.ForSection("Designer.ImportDrivers")("Available.Free.Space.Column")
        Me.ColumnHeader6.Text = LocalizationService.ForSection("Designer.ImportDrivers")("DriveFormat.Column")
        Me.ColumnHeader7.Text = LocalizationService.ForSection("Designer.ImportDrivers")("ContainsWindows.Column")
        Me.ColumnHeader8.Text = LocalizationService.ForSection("Designer.ImportDrivers")("Windows.Column")
        Me.Button2.Text = LocalizationService.ForSection("Designer.ImportDrivers")("Refresh.Button")
        Me.Label7.Text = LocalizationService.ForSection("Designer.ImportDrivers")("Offline.Drivers.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ImportDrivers")("Source.Listed.Choose.Label")
        Me.Text = LocalizationService.ForSection("Designer.ImportDrivers")("ImportDrivers.Label")
    End Sub

End Class

Partial Class RemDrivers

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.RemDrivers")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.RemDrivers")("Cancel.Button")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.RemDrivers")("PublishedName.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.RemDrivers")("Original.File.Name.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.RemDrivers")("ProviderName.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.RemDrivers")("ClassName.Column")
        Me.ColumnHeader5.Text = LocalizationService.ForSection("Designer.RemDrivers")("Part.Windows.Column")
        Me.ColumnHeader6.Text = LocalizationService.ForSection("Designer.RemDrivers")("BootCritical.Column")
        Me.ColumnHeader7.Text = LocalizationService.ForSection("Designer.RemDrivers")("Version.Column")
        Me.ColumnHeader8.Text = LocalizationService.ForSection("Designer.RemDrivers")("Date.Column")
        Me.Label2.Text = LocalizationService.ForSection("Designer.RemDrivers")("DriverPackages.Wish.Label")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.RemDrivers")("Hide.Boot.Critical.CheckBox")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.RemDrivers")("Hide.Drivers.Part.CheckBox")
        Me.Text = LocalizationService.ForSection("Designer.RemDrivers")("RemoveDrivers.Label")
    End Sub

End Class

Partial Class SetImageEdition

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ImageEdition")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ImageEdition")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.ImageEdition")("Target.Upgrade.Label")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.ImageEdition")("ServerOptions.Group")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ImageEdition")("Browse.Button")
        Me.RadioButton2.Text = LocalizationService.ForSection("Designer.ImageEdition")("AcceptEULA.RadioButton")
        Me.RadioButton1.Text = LocalizationService.ForSection("Designer.ImageEdition")("Copy.EndUser.RadioButton")
        Me.Text = LocalizationService.ForSection("Designer.ImageEdition")("Set.Image.Label")
    End Sub

End Class

Partial Class SetImageKey

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.SetProductKey")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.SetProductKey")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.SetProductKey")("Type.ProductKey.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.SetProductKey")("ValidateKey.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.SetProductKey")("Check.ProductKey.Message")
        Me.Text = LocalizationService.ForSection("Designer.SetProductKey")("SetProductKey.Label")
    End Sub

End Class

Partial Class EnvVarManagementForm

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.SaveAllChangesBtn.Text = LocalizationService.ForSection("Designer.EnvVars")("Save.Changes.Label")
        Me.Label1.Text = LocalizationService.ForSection("Designer.EnvVars")("Intro.Message")
        Me.SysEnvVarGB.Text = LocalizationService.ForSection("Designer.EnvVars")("TargetSystem.Label")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.EnvVars")("Name.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.EnvVars")("Value.Column")
        Me.RemoveMachineVarButton.Text = LocalizationService.ForSection("Designer.EnvVars")("Remove.Machine.Label")
        Me.AddMachineVarButton.Text = LocalizationService.ForSection("Designer.EnvVars")("Add.Machine.Variable.Button")
        Me.UserEnvVarGB.Text = LocalizationService.ForSection("Designer.EnvVars")("DefaultUser.Label")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.EnvVars")("Name.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.EnvVars")("Value.Column")
        Me.RemoveUserVarBtn.Text = LocalizationService.ForSection("Designer.EnvVars")("Remove.User.Variable.Label")
        Me.AddUserVarButton.Text = LocalizationService.ForSection("Designer.EnvVars")("Add.User.Variable.Button")
        Me.SaveVarBtn.Text = LocalizationService.ForSection("Designer.EnvVars")("SaveVariable.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.EnvVars")("Scope.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.EnvVars")("Hierarchical.Values.Message")
        Me.Label6.Text = LocalizationService.ForSection("Designer.EnvVars")("Variables.Location.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.EnvVars")("Value.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.EnvVars")("Name.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.EnvVars")("VariableInfo.Label")
        Me.CopyToUserScopeBtn.Text = LocalizationService.ForSection("Designer.EnvVars")("Copy.Default.User.Label")
        Me.CopyToMachineScopeBtn.Text = LocalizationService.ForSection("Designer.EnvVars")("Copy.Machine.Scope.Label")
        Me.MoveToMachineScopeBtn.Text = LocalizationService.ForSection("Designer.EnvVars")("Move.Machine.Scope.Label")
        Me.MoveToUserScopeBtn.Text = LocalizationService.ForSection("Designer.EnvVars")("Move.Default.User.Label")
        Me.Text = LocalizationService.ForSection("Designer.EnvVars")("SystemVariables.Label")
    End Sub

End Class

Partial Class DisableFeat

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.DisableFeat")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.DisableFeat")("Cancel.Button")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.DisableFeat")("Options.Group")
        Me.Button1.Text = LocalizationService.ForSection("Designer.DisableFeat")("Lookup.Button")
        Me.Label3.Text = LocalizationService.ForSection("Designer.DisableFeat")("PackageName.Label")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.DisableFeat")("Remove.Feature.CheckBox")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.DisableFeat")("ParentPackage.CheckBox")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.DisableFeat")("Features.Group")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.DisableFeat")("FeatureName.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.DisableFeat")("State.Column")
        Me.Text = LocalizationService.ForSection("Designer.DisableFeat")("DisableFeatures.Label")
    End Sub

End Class

Partial Class EnableFeat

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.EnableFeat")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.EnableFeat")("Cancel.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.EnableFeat")("Features.Group")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.EnableFeat")("FeatureName.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.EnableFeat")("State.Column")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.EnableFeat")("Options.Group")
        Me.Button3.Text = LocalizationService.ForSection("Designer.EnableFeat")("Detect.Group.Policy.Button")
        Me.Button2.Text = LocalizationService.ForSection("Designer.EnableFeat")("Browse.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.EnableFeat")("Lookup.Button")
        Me.Label4.Text = LocalizationService.ForSection("Designer.EnableFeat")("FeatureSource.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.EnableFeat")("PackageName.Label")
        Me.CheckBox5.Text = LocalizationService.ForSection("Designer.EnableFeat")("CommitImage.CheckBox")
        Me.CheckBox4.Text = LocalizationService.ForSection("Designer.EnableFeat")("Contact.Win.Update.CheckBox")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.EnableFeat")("ParentFeatures.CheckBox")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.EnableFeat")("Feature.Source.CheckBox")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.EnableFeat")("ParentPackage.CheckBox")
        Me.FolderBrowserDialog1.Description = LocalizationService.ForSection("Designer.EnableFeat")("SourceFolder.Description")
        Me.Text = LocalizationService.ForSection("Designer.EnableFeat")("EnableFeatures.Label")
    End Sub

End Class

Partial Class PkgParentNameLookupDlg

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.PkgNameLookup")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.PkgNameLookup")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.PkgParentLookup")("Names.Installed.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.PkgNameLookup")("ParentPackage.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.PkgNameLookup")("Get.Package.Names.Label")
        Me.Text = LocalizationService.ForSection("Designer.PkgNameLookup")("Installed.Package.Label")
    End Sub

End Class

Partial Class ApplicationDriveSpecifier

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.AppDrive")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.AppDrive")("Cancel.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.AppDrive")("Destination.Disk.Id.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.AppDrive")("Refresh.Button")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.AppDrive")("DeviceID.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.AppDrive")("Model.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.AppDrive")("Partitions.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.AppDrive")("Size.Column")
        Me.Text = LocalizationService.ForSection("Designer.AppDrive")("Target.Disk.Button")
    End Sub

End Class

Partial Class FfuApply

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.FFUApply")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.FFUApply")("Cancel.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.FFUApply")("Source.Group")
        Me.Button1.Text = LocalizationService.ForSection("Designer.FFUApply")("Browse.Button")
        Me.UseMountedImgBtn.Text = LocalizationService.ForSection("Designer.FFUApply")("Mounted.Image.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.FFUApply")("SourceImageFile.Label")
        Me.GroupBox4.Text = LocalizationService.ForSection("Designer.FFUApply")("SfufilePattern.Group")
        Me.ToolStripStatusLabel1.Text = LocalizationService.ForSection("Designer.FFUApply")("Status.InitialLabel")
        Me.Button5.Text = LocalizationService.ForSection("Designer.FFUApply")("ScanPattern.Button")
        Me.Button4.Text = LocalizationService.ForSection("Designer.FFUApply")("Name.Image.Button")
        Me.Label4.Text = LocalizationService.ForSection("Designer.FFUApply")("NamingPattern.Label")
        Me.GroupBox3.Text = LocalizationService.ForSection("Designer.FFUApply")("Destination.Group")
        Me.Label1.Text = LocalizationService.ForSection("Designer.FFUApply")("DriveDetails.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.FFUApply")("DestinationDrive.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.FFUApply")("Specify.Button")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.FFUApply")("Full.Flash.Utility.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.FFUApply")("Source.Image.Required.Title")
        Me.CheckBox4.Text = LocalizationService.ForSection("Designer.FFUApply")("Reference.Sfufiles.CheckBox")
        Me.Text = LocalizationService.ForSection("Designer.FFUApply")("File.Label")
    End Sub

End Class

Partial Class FfuCapture

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.FFUCapture")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.FFUCapture")("Cancel.Button")
        Me.GroupBox3.Text = LocalizationService.ForSection("Designer.FFUCapture")("Source.Group")
        Me.Label1.Text = LocalizationService.ForSection("Designer.FFUCapture")("DriveDetails.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.FFUCapture")("SourceDrive.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.FFUCapture")("Specify.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.FFUCapture")("Destination.Group")
        Me.Button1.Text = LocalizationService.ForSection("Designer.FFUCapture")("Browse.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.FFUCapture")("Destination.ImageFile.Label")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.FFUCapture")("Options.Group")
        Me.Label8.Text = LocalizationService.ForSection("Designer.FFUCapture")("Description.Goes.Label")
        Me.ComboBox1.Items(0) = LocalizationService.ForSection("Designer.FFUCapture")("None.Item")
        Me.ComboBox1.Items(1) = LocalizationService.ForSection("Designer.FFUCapture")("Default.Item")
        Me.ComboBox1.Text = LocalizationService.ForSection("Designer.FFUCapture")("Default.Item")
        Me.Label7.Text = LocalizationService.ForSection("Designer.FFUCapture")("CompressionType.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.FFUCapture")("Dest.Image.Description.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.FFUCapture")("Destination.Image.Name.Label")
        Me.SaveFileDialog1.Filter = LocalizationService.ForSection("Designer.FFUCapture")("Full.Flash.Utility.Filter")
        Me.Text = LocalizationService.ForSection("Designer.FFUCapture")("File.Label")
    End Sub

End Class

Partial Class FfuInfoDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("Cancel.Button")
        Me.TabPage1.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("Ffuheader.Tab")
        Me.Label10.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("Value.Label")
        Me.Label8.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("Value.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("Value.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("Value.Label")
        Me.Label9.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("CompressionType.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("Ffuversion.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("Physical.Disk.Path.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("Vhdstorage.Device.ID.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("MountedVHDID.Label")
        Me.Label1.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("MountedVhdpath.Label")
        Me.TabPage2.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("MountedVHD.Tab")
        Me.Label12.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("Selected.Partition.Label")
        Me.Label11.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("Mounted.FFU.Message")
        Me.TabPage3.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("Manifest.Tab")
        Me.Text = LocalizationService.ForSection("Designer.FFUInfoDialog")("Full.Flash.Utility.Label")
    End Sub

End Class

Partial Class FfuOptimize

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.FFUOptimize")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.FFUOptimize")("Cancel.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.FFUOptimize")("Browse.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.FFUOptimize")("ImageFile.Label")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.FFUOptimize")("Default.Partition.CheckBox")
        Me.Label2.Text = LocalizationService.ForSection("Designer.FFUOptimize")("PartitionNumber.Label")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.FFUOptimize")("Full.Flash.Utility.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.FFUOptimize")("OpenFile.Title")
        Me.Text = LocalizationService.ForSection("Designer.FFUOptimize")("Ffuimages.Label")
    End Sub

End Class

Partial Class FfuSplit

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.FFUSplit")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.FFUSplit")("Cancel.Button")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.FFUSplit")("Integrity.CheckBox")
        Me.Button2.Text = LocalizationService.ForSection("Designer.FFUSplit")("Browse.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.FFUSplit")("Browse.Button")
        Me.Label5.Text = LocalizationService.ForSection("Designer.FFUSplit")("LargeFile.Note.Message")
        Me.Label4.Text = LocalizationService.ForSection("Designer.FFUSplit")("Maximum.Size.Images.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.FFUSplit")("Name.Path.Destination.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.FFUSplit")("Source.Image.Label")
        Me.SaveFileDialog1.Filter = LocalizationService.ForSection("Designer.FFUSplit")("Sfufiles.Filter")
        Me.SaveFileDialog1.Title = LocalizationService.ForSection("Designer.FFUSplit")("Target.Location.Title")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.FFUSplit")("Full.Flash.Utility.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.FFUSplit")("Source.WIM.File.Title")
        Me.Text = LocalizationService.ForSection("Designer.FFUSplit")("SplitFfuimages.Label")
    End Sub

End Class

Partial Class ImageFilePickerDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ImageFilePicker")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ImageFilePicker")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.ImageFilePicker")("MountList.Prompt.Label")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.ImageFilePicker")("ImageFile.Column")
        Me.Text = LocalizationService.ForSection("Designer.ImageFilePicker")("Pick.Windows.ImageFile.Label")
    End Sub

End Class

Partial Class ImgAppend

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ImgAppend")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ImgAppend")("Cancel.Button")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.ImgAppend")("Options.Group")
        Me.Button5.Text = LocalizationService.ForSection("Designer.ImgAppend")("Create.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ImgAppend")("Path.Config.File.Label")
        Me.Button4.Text = LocalizationService.ForSection("Designer.ImgAppend")("Grab.Last.Image.Button")
        Me.Button3.Text = LocalizationService.ForSection("Designer.ImgAppend")("Browse.Button")
        Me.CheckBox6.Text = LocalizationService.ForSection("Designer.ImgAppend")("Reparse.Point.Tag.CheckBox")
        Me.CheckBox7.Text = LocalizationService.ForSection("Designer.ImgAppend")("ExtendedAttributes.CheckBox")
        Me.CheckBox5.Text = LocalizationService.ForSection("Designer.ImgAppend")("Check.File.Errors.CheckBox")
        Me.CheckBox4.Text = LocalizationService.ForSection("Designer.ImgAppend")("Verify.Image.CheckBox")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.ImgAppend")("Image.Bootable.CheckBox")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.ImgAppend")("WIM.Boot.Config.CheckBox")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.ImgAppend")("Exclude.Files.Dirs.CheckBox")
        Me.Label5.Text = LocalizationService.ForSection("Designer.ImgAppend")("Dest.Image.Description.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.ImgAppend")("Destination.Image.Name.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.ImgAppend")("Browse.Button")
        Me.Label6.Text = LocalizationService.ForSection("Designer.ImgAppend")("Destination.ImageFile.Label")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.ImgAppend")("Sources.Destinations.Group")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ImgAppend")("Browse.Button")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ImgAppend")("Source.Image.Dir.Label")
        Me.SaveFileDialog1.Filter = LocalizationService.ForSection("Designer.ImgAppend")("WIM.Files.Filter")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.ImgAppend")("WimscriptIniwim.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.ImgAppend")("Wimscript.Ini.Title")
        Me.Text = LocalizationService.ForSection("Designer.ImgAppend")("AppendImage.Label")
    End Sub

End Class

Partial Class ImgApply

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ImgApply")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ImgApply")("Cancel.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.ImgApply")("Source.Group")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ImgApply")("Browse.Button")
        Me.UseMountedImgBtn.Text = LocalizationService.ForSection("Designer.ImgApply")("Mounted.Image.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ImgApply")("SourceImageFile.Label")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.ImgApply")("Options.Group")
        Me.CheckBox8.Text = LocalizationService.ForSection("Designer.ImgApply")("Extended.Attributes.CheckBox")
        Me.CheckBox7.Text = LocalizationService.ForSection("Designer.ImgApply")("Image.Compact.Mode.CheckBox")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ImgApply")("ImageIndex.Label")
        Me.CheckBox6.Text = LocalizationService.ForSection("Designer.ImgApply")("Append.Image.WIM.CheckBox")
        Me.CheckBox5.Text = LocalizationService.ForSection("Designer.ImgApply")("Validate.Image.CheckBox")
        Me.CheckBox4.Text = LocalizationService.ForSection("Designer.ImgApply")("Reference.Swmfiles.CheckBox")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.ImgApply")("Reparse.Point.Tag.CheckBox")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.ImgApply")("Verify.CheckBox")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.ImgApply")("Integrity.CheckBox")
        Me.GroupBox3.Text = LocalizationService.ForSection("Designer.ImgApply")("Destination.Group")
        Me.Label5.Text = LocalizationService.ForSection("Designer.ImgApply")("Destination.Dir.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.ImgApply")("Browse.Button")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.ImgApply")("WIM.Files.Wimswm.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.ImgApply")("Source.Image.Required.Title")
        Me.GroupBox4.Text = LocalizationService.ForSection("Designer.ImgApply")("SwmfilePattern.Group")
        Me.ToolStripStatusLabel1.Text = LocalizationService.ForSection("Designer.ImgApply")("Status.InitialLabel")
        Me.Button5.Text = LocalizationService.ForSection("Designer.ImgApply")("ScanPattern.Button")
        Me.Button4.Text = LocalizationService.ForSection("Designer.ImgApply")("Name.Image.Button")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ImgApply")("NamingPattern.Label")
        Me.FolderBrowserDialog1.Description = LocalizationService.ForSection("Designer.ImgApply")("DestinationDir.Description")
        Me.Text = LocalizationService.ForSection("Designer.ImgApply")("ApplyImage.Label")
    End Sub

End Class

Partial Class ImgCapture

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ImgCapture")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ImgCapture")("Cancel.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.ImgCapture")("Sources.Destinations.Group")
        Me.Button2.Text = LocalizationService.ForSection("Designer.ImgCapture")("Browse.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ImgCapture")("Destination.ImageFile.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ImgCapture")("Browse.Button")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ImgCapture")("Source.Image.Dir.Label")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.ImgCapture")("Options.Group")
        Me.Label8.Text = LocalizationService.ForSection("Designer.ImgCapture")("Description.Goes.Label")
        Me.ComboBox1.Items(0) = LocalizationService.ForSection("Designer.ImgCapture")("None.Item")
        Me.ComboBox1.Items(1) = LocalizationService.ForSection("Designer.ImgCapture")("Fast.Item")
        Me.ComboBox1.Items(2) = LocalizationService.ForSection("Designer.ImgCapture")("Maximum.Item")
        Me.ComboBox1.Text = LocalizationService.ForSection("Designer.ImgCapture")("Fast.Item")
        Me.Button5.Text = LocalizationService.ForSection("Designer.ImgCapture")("Create.Button")
        Me.Button3.Text = LocalizationService.ForSection("Designer.ImgCapture")("Browse.Button")
        Me.Label6.Text = LocalizationService.ForSection("Designer.ImgCapture")("Path.Config.File.Label")
        Me.CheckBox5.Text = LocalizationService.ForSection("Designer.ImgCapture")("Reparse.Point.Tag.CheckBox")
        Me.CheckBox8.Text = LocalizationService.ForSection("Designer.ImgCapture")("Mount.Dest.Image.CheckBox")
        Me.CheckBox7.Text = LocalizationService.ForSection("Designer.ImgCapture")("Extended.Attributes.CheckBox")
        Me.CheckBox6.Text = LocalizationService.ForSection("Designer.ImgCapture")("Append.WIM.Boot.CheckBox")
        Me.CheckBox4.Text = LocalizationService.ForSection("Designer.ImgCapture")("Check.File.Errors.CheckBox")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.ImgCapture")("Verify.Image.CheckBox")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.ImgCapture")("Image.Bootable.CheckBox")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.ImgCapture")("Exclude.Files.Dirs.CheckBox")
        Me.Label7.Text = LocalizationService.ForSection("Designer.ImgCapture")("CompressionType.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ImgCapture")("Dest.Image.Description.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.ImgCapture")("Destination.Image.Name.Label")
        Me.SaveFileDialog1.Filter = LocalizationService.ForSection("Designer.ImgCapture")("WIM.Files.Filter")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.ImgCapture")("WimscriptIniwim.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.ImgCapture")("Wimscript.Ini.Title")
        Me.Text = LocalizationService.ForSection("Designer.ImgCapture")("CaptureImage.Label")
    End Sub

End Class

Partial Class ImgExport

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ImgExport")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ImgExport")("Cancel.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.ImgExport")("Sources.Destinations.Group")
        Me.Button2.Text = LocalizationService.ForSection("Designer.ImgExport")("Browse.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ImgExport")("Destination.ImageFile.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ImgExport")("Browse.Button")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ImgExport")("SourceImageFile.Label")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.ImgExport")("Options.Group")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.ImgExport")("Reference.Swmfiles.CheckBox")
        Me.ToolStripStatusLabel1.Text = LocalizationService.ForSection("Designer.ImgExport")("Status.InitialLabel")
        Me.Button5.Text = LocalizationService.ForSection("Designer.ImgExport")("ScanPattern.Button")
        Me.Button4.Text = LocalizationService.ForSection("Designer.ImgExport")("Name.Image.Button")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ImgExport")("NamingPattern.Label")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.ImgExport")("CustomName.CheckBox")
        Me.Label8.Text = LocalizationService.ForSection("Designer.ImgExport")("Description.Goes.Label")
        Me.ComboBox1.Items(0) = LocalizationService.ForSection("Designer.ImgExport")("None.Item")
        Me.ComboBox1.Items(1) = LocalizationService.ForSection("Designer.ImgExport")("Fast.Item")
        Me.ComboBox1.Items(2) = LocalizationService.ForSection("Designer.ImgExport")("Maximum.Item")
        Me.ComboBox1.Items(3) = LocalizationService.ForSection("Designer.ImgExport")("Recovery.Item")
        Me.ComboBox1.Text = LocalizationService.ForSection("Designer.ImgExport")("Fast.Item")
        Me.Label5.Text = LocalizationService.ForSection("Designer.ImgExport")("CompressionType.Label")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.ImgExport")("Image.Bootable.CheckBox")
        Me.CheckBox4.Text = LocalizationService.ForSection("Designer.ImgExport")("Append.Image.WIM.CheckBox")
        Me.CheckBox5.Text = LocalizationService.ForSection("Designer.ImgExport")("CheckIntegrity.CheckBox")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.ImgExport")("Index.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.ImgExport")("ImageName.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.ImgExport")("ImageDescription.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.ImgExport")("ImageVersion.Column")
        Me.Label7.Text = LocalizationService.ForSection("Designer.ImgExport")("Source.Image.Index.Label")
        Me.SaveFileDialog1.Filter = LocalizationService.ForSection("Designer.ImgExport")("WIM.Files.Wimesd.Filter")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.ImgExport")("WIM.Files.Wimswm.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.ImgExport")("Source.ImageFile.Title")
        Me.Text = LocalizationService.ForSection("Designer.ImgExport")("ExportImage.Label")
    End Sub

End Class

Partial Class ImgIndexDelete

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ImageIndexDelete")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ImageIndexDelete")("Cancel.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ImageIndexDelete")("SourceImage.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ImageIndexDelete")("Browse.Button")
        Me.Button2.Text = LocalizationService.ForSection("Designer.ImageIndexDelete")("Mounted.Image.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.ImageIndexDelete")("VolumeImages.Group")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.ImageIndexDelete")("Index.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.ImageIndexDelete")("ImageName.Column")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.ImageIndexDelete")("Index.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.ImageIndexDelete")("ImageName.Column")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ImageIndexDelete")("Get.Indexes.Image.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ImageIndexDelete")("Mark.VolumeImages.Message")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.ImageIndexDelete")("Integrity.CheckBox")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.ImageIndexDelete")("WIM.Files.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.ImageIndexDelete")("Source.Image.Remove.Title")
        Me.Text = LocalizationService.ForSection("Designer.ImageIndexDelete")("Remove.Volume.Image.Label")
    End Sub

End Class

Partial Class ImgMount

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ImgMount")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ImgMount")("Cancel.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ImgMount")("Options.Required.Label")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.ImgMount")("Source.Group")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ImgMount")("Notewant.ESD.Label")
        Me.Button3.Text = LocalizationService.ForSection("Designer.ImgMount")("Convert.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ImgMount")("Browse.Button")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ImgMount")("ImageFile.Label")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.ImgMount")("Destination.Group")
        Me.Button2.Text = LocalizationService.ForSection("Designer.ImgMount")("Browse.Button")
        Me.Label6.Text = LocalizationService.ForSection("Designer.ImgMount")("MountDirectory.Label")
        Me.GroupBox3.Text = LocalizationService.ForSection("Designer.ImgMount")("Options.Group")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.ImgMount")("Index.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.ImgMount")("ImageName.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.ImgMount")("ImageDescription.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.ImgMount")("ImageVersion.Column")
        Me.CheckBox4.Text = LocalizationService.ForSection("Designer.ImgMount")("Integrity.CheckBox")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.ImgMount")("Optimize.Times.CheckBox")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.ImgMount")("Mount.Read.CheckBox")
        Me.Label7.Text = LocalizationService.ForSection("Designer.ImgMount")("Index.Label")
        Me.Label11.Text = LocalizationService.ForSection("Designer.ImgMount")("Fields.End.Required.Label")
        Me.FileSpecDialog.Filter = LocalizationService.ForSection("Designer.ImgMount")("FileSpec.Filter")
        Me.Text = LocalizationService.ForSection("Designer.ImgMount")("MountImage.Label")
    End Sub

End Class

Partial Class ImgOptimize

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ImgOptimize")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ImgOptimize")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.ImgOptimize")("Path.Mounted.Image.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ImgOptimize")("Pick.Button")
        Me.Button2.Text = LocalizationService.ForSection("Designer.ImgOptimize")("Mounted.Image.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.ImgOptimize")("Image.Optimization.Mode")
        Me.RadioButton1.Text = LocalizationService.ForSection("Designer.ImgOptimize")("Reduce.Online.RadioButton")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ImgOptimize")("Image.Again.Label")
        Me.RadioButton2.Text = LocalizationService.ForSection("Designer.ImgOptimize")("OfflineImage.RadioButton")
        Me.Text = LocalizationService.ForSection("Designer.ImgOptimize")("OptimizeImages.Label")
    End Sub

End Class

Partial Class ImgSplit

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ImgSplit")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ImgSplit")("Cancel.Button")
        Me.SaveFileDialog1.Filter = LocalizationService.ForSection("Designer.ImgSplit")("Swmfiles.Filter")
        Me.SaveFileDialog1.Title = LocalizationService.ForSection("Designer.ImgSplit")("SaveFile.Title")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.ImgSplit")("WIM.Files.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.ImgSplit")("Source.WIM.File.Title")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ImgSplit")("Source.Image.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ImgSplit")("Browse.Button")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ImgSplit")("Name.Path.Destination.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.ImgSplit")("Browse.Button")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ImgSplit")("Maximum.Size.Images.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.ImgSplit")("LargeFile.Note.Message")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.ImgSplit")("Integrity.CheckBox")
        Me.Text = LocalizationService.ForSection("Designer.ImgSplit")("SplitImages.Label")
    End Sub

End Class

Partial Class ImgUMount

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ImgUmount")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ImgUmount")("Cancel.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ImgUmount")("Options.Required.Label")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.ImgUmount")("MountDirectory.Group")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ImgUmount")("Pick.Button")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ImgUmount")("MountDirectory.Label")
        Me.RadioButton2.Text = LocalizationService.ForSection("Designer.ImgUmount")("LocatedSomewhere.RadioButton")
        Me.RadioButton1.Text = LocalizationService.ForSection("Designer.ImgUmount")("LoadedProject.RadioButton")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ImgUmount")("Mount.Dir.Label")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.ImgUmount")("Additional.Options.Group")
        Me.ComboBox1.Items(0) = LocalizationService.ForSection("Designer.ImgUmount")("Save.Changes.Unmount.Item")
        Me.ComboBox1.Items(1) = LocalizationService.ForSection("Designer.ImgUmount")("Discard.Changes.Unmount.Item")
        Me.ComboBox1.Text = LocalizationService.ForSection("Designer.ImgUmount")("Save.Changes.Unmount.Item")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.ImgUmount")("Append.Changes.CheckBox")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.ImgUmount")("Integrity.CheckBox")
        Me.Label7.Text = LocalizationService.ForSection("Designer.ImgUmount")("UnmountOperation.Label")
        Me.FolderBrowserDialog1.Description = LocalizationService.ForSection("Designer.ImgUmount")("MountDir.Description")
        Me.Text = LocalizationService.ForSection("Designer.ImgUmount")("UnmountImage.Label")
    End Sub

End Class

Partial Class SetLayeredDriverDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.SetLayeredDriver")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.SetLayeredDriver")("Cancel.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.SetLayeredDriver")("Intro.Message")
        Me.Label3.Text = LocalizationService.ForSection("Designer.SetLayeredDriver")("CurrentDriver.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.SetLayeredDriver")("NewDriver.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.SetLayeredDriver")("Driver.Already.Label")
        Me.Text = LocalizationService.ForSection("Designer.SetLayeredDriver")("Title")
    End Sub

End Class

Partial Class ImgSwmToWim

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.Img.SWM")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.Img.SWM")("Cancel.Button")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.Img.SWM")("Split.WIM.Files.Filter")
        Me.OpenFileDialog1.Title = LocalizationService.ForSection("Designer.Img.SWM")("Source.Swmfile.Title")
        Me.SaveFileDialog1.Filter = LocalizationService.ForSection("Designer.Img.SWM")("WIM.Files.Filter")
        Me.SaveFileDialog1.Title = LocalizationService.ForSection("Designer.Img.SWM")("Dest.WIM.File.Title")
        Me.Label2.Text = LocalizationService.ForSection("Designer.Img.SWM")("SourceSwmfile.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.Img.SWM")("Browse.Button")
        Me.Label4.Text = LocalizationService.ForSection("Designer.Img.SWM")("Destination.WIM.File.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.Img.SWM")("Browse.Button")
        Me.Label3.Text = LocalizationService.ForSection("Designer.Img.SWM")("Notewhen.Specifying.Message")
        Me.LinkLabel1.Text = LocalizationService.ForSection("Designer.Img.SWM")("LearnHow.Link")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.Img.SWM")("Source.Group")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.Img.SWM")("Options.Group")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.Img.SWM")("Index.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.Img.SWM")("ImageName.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.Img.SWM")("ImageDescription.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.Img.SWM")("ImageVersion.Column")
        Me.Label5.Text = LocalizationService.ForSection("Designer.Img.SWM")("Index.Label")
        Me.GroupBox3.Text = LocalizationService.ForSection("Designer.Img.SWM")("Destination.Group")
        Me.Text = LocalizationService.ForSection("Designer.Img.SWM")("MergeSwmfiles.Label")
    End Sub

End Class

Partial Class MountedImgMgr

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Overview.Images.Message")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("ImageFile.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Index.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("MountDirectory.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Status.Column")
        Me.ColumnHeader5.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Read.Write.Column")
        Me.Button6.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("LoadProject.Button")
        Me.Button7.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Value.Button")
        Me.Button4.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Open.Mount.Dir.Button")
        Me.Button3.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Enable.Write.Button")
        Me.Button2.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("ReloadServicing.Button")
        Me.Button5.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Remove.VolumeImages.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("UnmountImage.Button")
        Me.Text = LocalizationService.ForSection("Designer.MountedImgMgr")("Image.Manager.Label")
    End Sub

End Class

Partial Class OfflineInstDriveLister

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.OfflineDriveList")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.OfflineDriveList")("Cancel.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.OfflineDriveList")("Refresh.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.OfflineDriveList")("Begin.Install.Message")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.OfflineDriveList")("DriveLetter.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.OfflineDriveList")("DriveLabel.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.OfflineDriveList")("DriveType.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.OfflineDriveList")("TotalSize.Column")
        Me.ColumnHeader5.Text = LocalizationService.ForSection("Designer.OfflineDriveList")("Available.Free.Space.Column")
        Me.ColumnHeader6.Text = LocalizationService.ForSection("Designer.OfflineDriveList")("DriveFormat.Column")
        Me.ColumnHeader7.Text = LocalizationService.ForSection("Designer.OfflineDriveList")("ContainsWindows.Column")
        Me.ColumnHeader8.Text = LocalizationService.ForSection("Designer.OfflineDriveList")("Windows.Column")
        Me.UnlockNoticeLabel.Text = LocalizationService.ForSection("Designer.OfflineDriveList")("UnlockNotice.Label")
        Me.Text = LocalizationService.ForSection("Designer.OfflineDriveList")("Disk.Choose.Label")
    End Sub

End Class

Partial Class OSNoRollbackErrorDlg

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.NoRollbackError")("Ok.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.NoRollbackError")("Old.Versions.None.Message")
        Me.Label1.Text = LocalizationService.ForSection("Designer.NoRollbackError")("Troll.Back.Older.Label")
        Me.Text = LocalizationService.ForSection("Designer.NoRollbackError")("DISMTools.Label")
    End Sub

End Class

Partial Class SetOSUninstWindow

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.OSRollback")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.OSRollback")("Cancel.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.OSRollback")("Default.OS.Message")
        Me.Label3.Text = LocalizationService.ForSection("Designer.OSRollback")("Amount.Days.Revert.Label")
        Me.Text = LocalizationService.ForSection("Designer.OSRollback")("OSUninstall.Label")
    End Sub

End Class

Partial Class AddPackageDlg

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.AddPackage")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.AddPackage")("Cancel.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.AddPackage")("Packages.Group")
        Me.Label4.Text = LocalizationService.ForSection("Designer.AddPackage")("Folder.Contains.Pkgnum.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.AddPackage")("SelectAll.Button")
        Me.Button3.Text = LocalizationService.ForSection("Designer.AddPackage")("SelectNone.Button")
        Me.RadioButton2.Text = LocalizationService.ForSection("Designer.AddPackage")("Packages.Choose.RadioButton")
        Me.RadioButton1.Text = LocalizationService.ForSection("Designer.AddPkg")("ScanRecursive.RadioButton")
        Me.Button1.Text = LocalizationService.ForSection("Designer.AddPackage")("Browse.Button")
        Me.Label3.Text = LocalizationService.ForSection("Designer.AddPackage")("PackageOperation.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.AddPackage")("PackageSource.Label")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.AddPackage")("Options.Group")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.AddPackage")("Save.Image.Packages.CheckBox")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.AddPkg")("Skip.Online.Install.CheckBox")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.AddPackage")("Ignore.CheckBox")
        Me.FolderBrowserDialog1.Description = LocalizationService.ForSection("Designer.AddPackage")("CabFolder.Description")
        Me.Button4.Text = LocalizationService.ForSection("Designer.AddPackage")("Update.Manifest.Button")
        Me.Text = LocalizationService.ForSection("Designer.AddPackage")("AddPackages.Label")
    End Sub

End Class

Partial Class MUMAdditionDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.MUMAdd")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.MUMAdd")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.MUMAdd")("DialogHelp.Message")
        Me.Label2.Text = LocalizationService.ForSection("Designer.MUMAdd")("Path.Manifest.File.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.MUMAdd")("Browse.Button")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.MUMAdd")("MUMFiles.Filter")
        Me.Text = LocalizationService.ForSection("Designer.MUMAdd")("Update.Manifest.Label")
    End Sub

End Class

Partial Class RemPackage

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.RemPackage")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.RemPackage")("Cancel.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.RemPackage")("PackageRemoval.Group")
        Me.Button1.Text = LocalizationService.ForSection("Designer.RemPackage")("Browse.Button")
        Me.Label4.Text = LocalizationService.ForSection("Designer.RemPackage")("Note.May.Message")
        Me.Label3.Text = LocalizationService.ForSection("Designer.RemPackage")("PackageSource.Label")
        Me.RadioButton2.Text = LocalizationService.ForSection("Designer.RemPackage")("Package.Files.RadioButton")
        Me.RadioButton1.Text = LocalizationService.ForSection("Designer.RemPackage")("Package.Names.RadioButton")
        Me.FolderBrowserDialog1.Description = LocalizationService.ForSection("Designer.RemPackage")("PackageSource.Description")
        Me.Text = LocalizationService.ForSection("Designer.RemPackage")("RemovePackages.Label")
    End Sub

End Class

Partial Class AddProvisioningPkg

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ProvPackage")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ProvPackage")("Cancel.Button")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.ProvPackage")("CommitImage.CheckBox")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ProvPackage")("PackagePath.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ProvPackage")("Browse.Button")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ProvPackage")("Action.Treverted.Add.Message")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.ProvPackage")("Package.Ppkg.Filter")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ProvPackage")("CatalogPath.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.ProvPackage")("Browse.Button")
        Me.OpenFileDialog2.Filter = LocalizationService.ForSection("Designer.ProvPackage")("Catalog.File.Cat.Filter")
        Me.Text = LocalizationService.ForSection("Designer.ProvPackage")("Add.Packages.Label")
    End Sub

End Class

Partial Class RegistryControlPanel

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Tool.Lets.Load.Message")
        Me.Button12.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Load.Button")
        Me.Button11.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Load.Button")
        Me.Button10.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Load.Button")
        Me.Label5.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Ntuserdatdefault.User.Label")
        Me.Button4.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Open.Button")
        Me.Label4.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Default.Label")
        Me.Button3.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Open.Button")
        Me.Label3.Text = LocalizationService.ForSection("Designer.RegistryPanel")("System.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Open.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Software.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Open.Button")
        Me.Button9.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Load.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Load.Custom.Hive")
        Me.Button6.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Unload.Button")
        Me.Button7.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Open.Button")
        Me.Button8.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Load.Button")
        Me.Button5.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Browse.Button")
        Me.Label8.Text = LocalizationService.ForSection("Designer.RegistryPanel")("PathRegistry.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.RegistryPanel")("HiveLocation.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Load.Different.Label")
        Me.Text = LocalizationService.ForSection("Designer.RegistryPanel")("Image.Hives.Label")
    End Sub

End Class

Partial Class RegisteredServiceHostGroupsDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ServiceGroups")("Ok.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.ServiceGroups")("Windows.Message")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.ServiceGroups")("GroupName.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.ServiceGroups")("ServicesGroup.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.ServiceGroups")("ServiceName.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.ServiceGroups")("DisplayName.Column")
        Me.ColumnHeader5.Text = LocalizationService.ForSection("Designer.ServiceGroups")("Type.Column")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ServiceGroups")("Total.Label")
        Me.Text = LocalizationService.ForSection("Designer.ServiceGroups")("Registered.Svc.Host.Label")
    End Sub

End Class

Partial Class ServiceManagementForm

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.Services")("Intro.Message")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.Services")("ServiceName.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.Services")("DisplayName.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.Services")("Description.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.Services")("StartType.Column")
        Me.ColumnHeader12.Text = LocalizationService.ForSection("Designer.Services")("Type.Column")
        Me.TabPage1.Text = LocalizationService.ForSection("Designer.Services")("ServiceInfo.Tab")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.Services")("DelayedStart.CheckBox")
        Me.Label4.Text = LocalizationService.ForSection("Designer.Services")("Description.Label")
        Me.Label19.Text = LocalizationService.ForSection("Designer.Services")("User.Flags.Label")
        Me.Label8.Text = LocalizationService.ForSection("Designer.Services")("ServiceType.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.Services")("Start.Type.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.Services")("Object.Name.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.Services")("Image.Path.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.Services")("Display.Name.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.Services")("ServiceName.Label")
        Me.TabPage2.Text = LocalizationService.ForSection("Designer.Services")("Required.Privileges.Tab")
        Me.ColumnHeader5.Text = LocalizationService.ForSection("Designer.Services")("PrivilegeName.Column")
        Me.ColumnHeader6.Text = LocalizationService.ForSection("Designer.Services")("PrivilegeName.Display.Column")
        Me.ColumnHeader7.Text = LocalizationService.ForSection("Designer.Services")("Privilege.Description.Column")
        Me.TabPage3.Text = LocalizationService.ForSection("Designer.Services")("ErrorControl.Tab")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.Services")("FailureActions.Group")
        Me.Label12.Text = LocalizationService.ForSection("Designer.Services")("FutureErrors.Label")
        Me.Label11.Text = LocalizationService.ForSection("Designer.Services")("NdError.Label")
        Me.Label14.Text = LocalizationService.ForSection("Designer.ServiceMgmt")("Restart.Minutes.Label")
        Me.Label13.Text = LocalizationService.ForSection("Designer.Services")("ResetErrorCount.Label")
        Me.Label10.Text = LocalizationService.ForSection("Designer.Services")("StError.Label")
        Me.Label9.Text = LocalizationService.ForSection("Designer.Services")("Error.Windows.Label")
        Me.TabPage4.Text = LocalizationService.ForSection("Designer.Services")("Dependencies.Tab")
        Me.ColumnHeader8.Text = LocalizationService.ForSection("Designer.Services")("ServiceName.Column")
        Me.ColumnHeader9.Text = LocalizationService.ForSection("Designer.Services")("DisplayName.Column")
        Me.ColumnHeader10.Text = LocalizationService.ForSection("Designer.Services")("Type.Column")
        Me.Label17.Text = LocalizationService.ForSection("Designer.ServiceMgmt")("Dependencies.Label")
        Me.ColumnHeader11.Text = LocalizationService.ForSection("Designer.Services")("ServiceName.Column")
        Me.ColumnHeader13.Text = LocalizationService.ForSection("Designer.Services")("DisplayName.Column")
        Me.ColumnHeader14.Text = LocalizationService.ForSection("Designer.Services")("Type.Column")
        Me.Label18.Text = LocalizationService.ForSection("Designer.ServiceMgmt")("Dependent.Services.Label")
        Me.TabPage5.Text = LocalizationService.ForSection("Designer.Services")("ServiceGroups.Tab")
        Me.GetSvchostGroupsBtn.Text = LocalizationService.ForSection("Designer.Services")("RegisteredHosts.Label")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.Services")("Services.Belong.Group")
        Me.ColumnHeader15.Text = LocalizationService.ForSection("Designer.Services")("ServiceName.Column")
        Me.ColumnHeader16.Text = LocalizationService.ForSection("Designer.Services")("DisplayName.Column")
        Me.ColumnHeader17.Text = LocalizationService.ForSection("Designer.Services")("Type.Column")
        Me.Label16.Text = LocalizationService.ForSection("Designer.Services")("Part.Group.Label")
        Me.SaveServiceInfoBtn.Text = LocalizationService.ForSection("Designer.Services")("Save.Changes.Label")
        Me.ProgressLabel.Text = LocalizationService.ForSection("Designer.Services")("ProgressLabel.Label")
        Me.ReloadServiceInformationBtn.Text = LocalizationService.ForSection("Designer.Services")("Reload.Label")
        Me.Label15.Text = LocalizationService.ForSection("Designer.Services")("SelectService.Label")
        Me.ReportServiceInfoBtn.Text = LocalizationService.ForSection("Designer.Services")("Save.Button")
        Me.ServiceInfoSFD.Filter = LocalizationService.ForSection("Designer.Services")("MarkdownFiles.Filter")
        Me.RestoreServiceBtn.Text = LocalizationService.ForSection("Designer.Services")("RestoreService.Label")
        Me.DeleteServiceBtn.Text = LocalizationService.ForSection("Designer.Services")("DeleteService.Label")
        Me.Text = LocalizationService.ForSection("Designer.Services")("System.Label")
    End Sub

End Class

Partial Class ImgIndexSwitch

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ImageIndexSwitch")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ImageIndexSwitch")("Cancel.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.ImageIndexSwitch")("Indexes.Group")
        Me.RadioButton2.Text = LocalizationService.ForSection("Designer.ImageIndexSwitch")("DiscardChanges.RadioButton")
        Me.RadioButton1.Text = LocalizationService.ForSection("Designer.ImageIndexSwitch")("Save.Changes.RadioButton")
        Me.Label5.Text = LocalizationService.ForSection("Designer.ImageIndexSwitch")("Index.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ImageIndexSwitch")("Destination.Mount.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ImageIndexSwitch")("Unmounting.Source.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ImageIndexSwitch")("Image.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.ImageIndexSwitch")("Already.Mounted.Label")
        Me.Text = LocalizationService.ForSection("Designer.ImageIndexSwitch")("Image.Indexes.Label")
    End Sub

End Class

Partial Class SingleImageIndexError

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.LinkLabel1.Text = LocalizationService.ForSection("Designer.SingleImageIndex")("Know.Indexes.Message")
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.SingleImageIndex")("Ok.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.SingleImageIndex")("Cannot.Switch.Message")
        Me.Label1.Text = LocalizationService.ForSection("Designer.SingleImageIndex")("Image.Seems.Only.Label")
        Me.Text = LocalizationService.ForSection("Designer.SingleImageIndex")("DISMTools.Label")
    End Sub

End Class

Partial Class ApplyUnattendFile

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ApplyUnattend")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ApplyUnattend")("Cancel.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ApplyUnattend")("Browse.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ApplyUnattend")("AnswerFile.Label")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.ApplyUnattend")("Answer.Files.XML.Filter")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.ApplyUnattend")("Copy.AnswerFile.CheckBox")
        Me.Label1.Text = LocalizationService.ForSection("Designer.ApplyUnattend")("LeaveUnchecked.Message")
        Me.Text = LocalizationService.ForSection("Designer.ApplyUnattend")("UnattendAnswer.File.Label")
    End Sub

End Class

Partial Class SetPEScratchSpace

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.Scratch")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.Scratch")("Cancel.Button")
        Me.Label3.Text = LocalizationService.ForSection("Designer.Scratch")("ScratchSpace.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.Scratch")("AmountWritable.Message")
        Me.Label4.Text = LocalizationService.ForSection("Designer.Scratch")("MB.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.Scratch")("ScratchSpace.Amount.Label")
        Me.Text = LocalizationService.ForSection("Designer.Scratch")("Set.Windows.Pescratch.Label")
    End Sub

End Class

Partial Class SetPETargetPath

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.SetTargetPath")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.SetTargetPath")("Cancel.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.SetTargetPath")("Target.Dir.Message")
        Me.Label3.Text = LocalizationService.ForSection("Designer.SetTargetPath")("TargetPath.Label")
        Me.Text = LocalizationService.ForSection("Designer.SetTargetPath")("Windows.Petarget.Label")
    End Sub

End Class

Partial Class ProjectValueLoadForm

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.ProjectValues")("Old.File.Label")
        Me.Exit_Button.Text = LocalizationService.ForSection("Designer.ProjectValues")("ExitButton.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.ProjectValues")("Independent.Values.Group")
        Me.Label25.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageLang.Label")
        Me.Label26.Text = LocalizationService.ForSection("Designer.ProjectValues")("Image.Read.Write.Label")
        Me.Label24.Text = LocalizationService.ForSection("Designer.ProjectValues")("Image.Epoch.Modify.Label")
        Me.Label23.Text = LocalizationService.ForSection("Designer.ProjectValues")("Image.Epoch.Create.Label")
        Me.Label22.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageFileCount.Value")
        Me.Label21.Text = LocalizationService.ForSection("Designer.ProjectValues")("Image.Dir.Count.Label")
        Me.Label20.Text = LocalizationService.ForSection("Designer.ProjectValues")("Image.Sys.Root.Label")
        Me.Label19.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImagePsuite.Label")
        Me.Label18.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImagePtype.Label")
        Me.Label17.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageEdition.Value")
        Me.Label16.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageSplevel.Label")
        Me.Label15.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageSpbuild.Label")
        Me.Label14.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageHal.Label")
        Me.Label13.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageArch.Label")
        Me.Label12.Text = LocalizationService.ForSection("Designer.ProjectValues")("Image.WIM.Boot.Label")
        Me.Label11.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageDescription.Label")
        Me.Label10.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageName.Label")
        Me.Label9.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageVersion.Label")
        Me.Label8.Text = LocalizationService.ForSection("Designer.ProjectValues")("Image.Mount.Point.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageIndex.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageFile.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.ProjectValues")("Epoch.Creation.Time.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ProjectValues")("Location.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ProjectValues")("Name.Label")
        Me.Label48.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageFile.Languages.Label")
        Me.Label47.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageFileDates.Label")
        Me.Label49.Text = LocalizationService.ForSection("Designer.ProjectValues")("Verify.Image.Read.Label")
        Me.Label46.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageFileCount.Label")
        Me.Label45.Text = LocalizationService.ForSection("Designer.ProjectValues")("Image.Dir.Label.Label")
        Me.Label44.Text = LocalizationService.ForSection("Designer.ProjectValues")("Image.System.Root.Label")
        Me.Label43.Text = LocalizationService.ForSection("Designer.ProjectValues")("Image.Product.Suite.Label")
        Me.Label42.Text = LocalizationService.ForSection("Designer.ProjectValues")("Image.Product.Type.Label")
        Me.Label41.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageEdition.Label")
        Me.Label40.Text = LocalizationService.ForSection("Designer.ProjectValues")("ServicePackLevel.Label")
        Me.Label39.Text = LocalizationService.ForSection("Designer.ProjectValues")("ServicePackBuild.Label")
        Me.Label38.Text = LocalizationService.ForSection("Designer.ProjectValues")("HAL.Label")
        Me.Label37.Text = LocalizationService.ForSection("Designer.ProjectValues")("Mounted.Image.Arch.Label")
        Me.Label36.Text = LocalizationService.ForSection("Designer.ProjectValues")("Verify.Image.Supports.Label")
        Me.Label35.Text = LocalizationService.ForSection("Designer.ProjectValues")("MountedDescription.Label")
        Me.Label34.Text = LocalizationService.ForSection("Designer.ProjectValues")("Mounted.Image.Friendly.Label")
        Me.Label33.Text = LocalizationService.ForSection("Designer.ProjectValues")("Image.Version.Grab.Label")
        Me.Label32.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageFile.Mount.Point.Label")
        Me.Label31.Text = LocalizationService.ForSection("Designer.ProjectValues")("ImageFileIndex.Label")
        Me.Label30.Text = LocalizationService.ForSection("Designer.ProjectValues")("Mounted.ImageFile.Name.Label")
        Me.Label29.Text = LocalizationService.ForSection("Designer.ProjectValues")("Creation.Time.Unix.Label")
        Me.Label28.Text = LocalizationService.ForSection("Designer.ProjectValues")("ProjectLocation.Label")
        Me.Label27.Text = LocalizationService.ForSection("Designer.ProjectValues")("ProjectName.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ProjectValues")("Independent.Values.Message")
        Me.Label50.Text = LocalizationService.ForSection("Designer.ProjectValues")("New.File.Label")
        Me.Continue_Button.Text = LocalizationService.ForSection("Designer.ProjectValues")("ContinueButton.Button")
        Me.Text = LocalizationService.ForSection("Designer.ProjectValues")("ProjectValues.Label")
    End Sub

End Class

Partial Class ISOCreator

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label2.Text = LocalizationService.ForSection("Designer.ISOCreator")("ISO.File.Message")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.ISOCreator")("Options.Group")
        Me.CheckBox4.Text = LocalizationService.ForSection("Designer.ISOCreator")("Include.Essential.CheckBox")
        Me.Button6.Text = LocalizationService.ForSection("Designer.ISOCreator")("Customize.Environment.Button")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.ISOCreator")("Value.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.ISOCreator")("ImageName.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.ISOCreator")("ImageDescription.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.ISOCreator")("ImageVersion.Column")
        Me.ColumnHeader5.Text = LocalizationService.ForSection("Designer.ISOCreator")("Image.Architecture.Column")
        Me.Button5.Text = LocalizationService.ForSection("Designer.ISOCreator")("Browse.Button")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.ISOCreator")("Copy.Ventoy.Drives.CheckBox")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.ISOCreator")("Unattended.CheckBox")
        Me.Label6.Text = LocalizationService.ForSection("Designer.ISOCreator")("Architecture.Label")
        Me.Button2.Text = LocalizationService.ForSection("Designer.ISOCreator")("Pick.Button")
        Me.Button3.Text = LocalizationService.ForSection("Designer.ISOCreator")("Browse.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.ISOCreator")("Browse.Button")
        Me.Label7.Text = LocalizationService.ForSection("Designer.ISOCreator")("Target.Isolocation.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ISOCreator")("ImageFile.Add.Label")
        Me.Button4.Text = LocalizationService.ForSection("Designer.ISOCreator")("Mounted.Image.Button")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.ISOCreator")("Newly.Signed.Boot.CheckBox")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ISOCreator")("Cancel.Button")
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ISOCreator")("Create.Button")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.ISOCreator")("Progress.Group")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ISOCreator")("Re.Ready.Create.Label")
        Me.Label8.Text = LocalizationService.ForSection("Designer.ISOCreator")("Jobs.Label")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.ISOCreator")("WIM.Files.Filter")
        Me.SaveFileDialog1.Filter = LocalizationService.ForSection("Designer.ISOCreator")("Isofiles.Filter")
        Me.LinkLabel1.Text = LocalizationService.ForSection("Designer.ISOCreator")("Download.Windows.ADK.Link")
        Me.OpenFileDialog2.Filter = LocalizationService.ForSection("Designer.ISOCreator")("Answer.Files.XML.Filter")
        Me.ColumnHeader6.Text = LocalizationService.ForSection("Designer.ISOCreator")("JobId.Column")
        Me.ColumnHeader7.Text = LocalizationService.ForSection("Designer.ISOCreator")("DestinationFile.Column")
        Me.ColumnHeader8.Text = LocalizationService.ForSection("Designer.ISOCreator")("Status.Column")
        Me.ImageTaskHeader1.ItemText = LocalizationService.ForSection("Designer.ISOCreator")("CreateIsofile.Label")
        Me.Text = LocalizationService.ForSection("Designer.ISOCreator")("CreateIsofile.Label")
    End Sub

End Class

Partial Class NewTestingEnv

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.LinkLabel1.Text = LocalizationService.ForSection("Designer.NewTestingEnv")("Download.Windows.ADK.Link")
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.NewTestingEnv")("Create.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.NewTestingEnv")("Cancel.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.NewTestingEnv")("WizardHelp.Message")
        Me.Label6.Text = LocalizationService.ForSection("Designer.NewTestingEnv")("Architecture.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.NewTestingEnv")("Env.Architecture.Label")
        Me.Button3.Text = LocalizationService.ForSection("Designer.NewTestingEnv")("Browse.Button")
        Me.Label7.Text = LocalizationService.ForSection("Designer.NewTestingEnv")("Target.Project.Label")
        Me.GroupBox2.Text = LocalizationService.ForSection("Designer.NewTestingEnv")("Progress.Group")
        Me.Label3.Text = LocalizationService.ForSection("Designer.NewTestingEnv")("Re.Ready.Create.Label")
        Me.Label9.Text = LocalizationService.ForSection("Designer.NewTestingEnv")("Other.Things.Message")
        Me.Label8.Text = LocalizationService.ForSection("Designer.NewTestingEnv")("Status.Label")
        Me.Text = LocalizationService.ForSection("Designer.NewTestingEnv")("Create.Environment.Label")
    End Sub

End Class

Partial Class PECustomizerDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.PECustomizer")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.PECustomizer")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.PECustomizer")("Customize.Session.Label")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.PECustomizer")("Wallpaper.Group")
        Me.Button1.Text = LocalizationService.ForSection("Designer.PECustomizer")("Browse.Button")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.PECustomizer")("My.Desktop.CheckBox")
        Me.Label2.Text = LocalizationService.ForSection("Designer.PECustomizer")("Path.Custom.Wallpaper.Label")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.PECustomizer")("Show.Version.Top.CheckBox")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.PECustomizer")("Display.Images.CheckBox")
        Me.CheckBox4.Text = LocalizationService.ForSection("Designer.PECustomizer")("Show.Report.Hardware.Message")
        Me.Label3.Text = LocalizationService.ForSection("Designer.PECustomizer")("Default.Partitio.Table.Label")
        Me.ComboBox1.Items(0) = LocalizationService.ForSection("Designer.PECustomizer")("Partition.Table.Item")
        Me.ComboBox1.Items(1) = LocalizationService.ForSection("Designer.PECustomizer")("Default.Mbrpartition.Item")
        Me.ComboBox1.Items(2) = LocalizationService.ForSection("Designer.PECustomizer")("Default.Gptpartition.Item")
        Me.ComboBox1.Text = LocalizationService.ForSection("Designer.PECustomizer")("Partition.Table.Item")
        Me.Label4.Text = LocalizationService.ForSection("Designer.PECustomizer")("Partition.Table.Message")
        Me.Label5.Text = LocalizationService.ForSection("Designer.PECustomizer")("SecureBoot.Label")
        Me.ComboBox2.Items(0) = LocalizationService.ForSection("Designer.PECustomizer")("Ask.Me.Version.Item")
        Me.ComboBox2.Items(1) = LocalizationService.ForSection("Designer.PECustomizer.BootSign")("Windows.Production.PCA.Item")
        Me.ComboBox2.Items(2) = LocalizationService.ForSection("Designer.PECustomizer.BootSign")("Windows.UEFI.CA.Item")
        Me.ComboBox2.Text = LocalizationService.ForSection("Designer.PECustomizer")("Ask.Me.Version.Item")
        Me.Label6.Text = LocalizationService.ForSection("Designer.PECustomizer")("Connection.Attempts.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.PECustomizer")("ConnectionAttempts.Label")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.PECustomizer")("JpgfilesJpg.Filter")
        Me.CheckBox5.Text = LocalizationService.ForSection("Designer.PECustomizer")("CopyAnswerFiles.Message")
        Me.Label8.Text = LocalizationService.ForSection("Designer.PECustomizer")("Port.Used.PXE.Label")
        Me.Label9.Text = LocalizationService.ForSection("Designer.PECustomizer")("Pick.Default.Keyboard.Label")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.PECustomizer")("LayoutCode.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.PECustomizer")("LayoutName.Column")
        Me.Label10.Text = LocalizationService.ForSection("Designer.PECustomizer")("Layout.Code.Selected.Label")
        Me.DefaultPolicySaveButton.Text = LocalizationService.ForSection("Designer.PECustomizer")("Save.Default.Policies.Label")
        Me.TabPage1.Text = LocalizationService.ForSection("Designer.PECustomizer")("General.Tab")
        Me.TabPage2.Text = LocalizationService.ForSection("Designer.PECustomizer")("PXEs.Tab")
        Me.TabPage3.Text = LocalizationService.ForSection("Designer.PECustomizer")("KeyboardLayouts.Tab")
        Me.Label11.Text = LocalizationService.ForSection("Designer.PECustomizer")("Option.Only.Take.Label")
        Me.CheckBox6.Text = LocalizationService.ForSection("Designer.PECustomizer")("KeyboardOverride.CheckBox")
        Me.TabPage4.Text = LocalizationService.ForSection("Designer.PECustomizer")("Unattended.Deployments.Tab")
        Me.Label12.Text = LocalizationService.ForSection("Designer.PECustomizer")("Unattended.AnswerFile.Label")
        Me.ComboBox3.Items(0) = LocalizationService.ForSection("Designer.PECustomizer")("Ask.Me.Resolve.Item")
        Me.ComboBox3.Items(1) = LocalizationService.ForSection("Designer.PECustomizer.Conflict")("ISO.Item")
        Me.ComboBox3.Items(2) = LocalizationService.ForSection("Designer.PECustomizer.Conflict")("WindowsImage.Item")
        Me.ComboBox3.Text = LocalizationService.ForSection("Designer.PECustomizer")("Ask.Me.Resolve.Item")
        Me.Label13.Text = LocalizationService.ForSection("Designer.PECustomizer")("Assuming.Each.Answer.Message")
        Me.Text = LocalizationService.ForSection("Designer.PECustomizer")("CustomizePE.Label")
    End Sub

End Class

Partial Class PxeServerPortSpecifier

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.PXEServerPort")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.PXEServerPort")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.PXEServerPort")("Other.Message")
        Me.Label2.Text = LocalizationService.ForSection("Designer.PXEServerPort")("Port.Server.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.PXEServerPort")("Default.Button")
        Me.Button2.Text = LocalizationService.ForSection("Designer.PXEServerPort")("Check.Button")
        Me.Text = LocalizationService.ForSection("Designer.PXEServerPort")("ServerComponents.Label")
    End Sub

End Class

Partial Class WDSImageGroupSpecifier

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.WDSImageGroup")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.WDSImageGroup")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.WDSImageGroup")("Action.Choose.Label")
        Me.Refresh_Button.Text = LocalizationService.ForSection("Designer.WDSImageGroup")("Refresh.Button")
        Me.RadioButton1.Text = LocalizationService.ForSection("Designer.WDSImageGroup")("Upload.RadioButton")
        Me.RadioButton2.Text = LocalizationService.ForSection("Designer.WDSImageGroup")("CreateGroup.RadioButton")
        Me.Label2.Text = LocalizationService.ForSection("Designer.WDSImageGroup")("Already.Exists.Label")
        Me.Text = LocalizationService.ForSection("Designer.WDSImageGroup")("SpecifyGroup.Button")
    End Sub

End Class

Partial Class WDSInstallImageCopy

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Cancel.Button")
        Me.Button2.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Pick.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Browse.Button")
        Me.Label4.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("ImageFile.Server.Label")
        Me.Button3.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Mounted.Image.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Images.Added.Group.Label")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Value.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("ImageName.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("ImageDescription.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("ImageVersion.Column")
        Me.ColumnHeader5.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Image.Architecture.Column")
        Me.Button4.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Pick.Server.Groups.Button")
        Me.Button5.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("SelectAll.Button")
        Me.Button6.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("ClearSelection.Button")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Progress.Group")
        Me.Label3.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Re.Ready.OK.Label")
        Me.Label8.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Status.Label")
        Me.OpenFileDialog1.Filter = LocalizationService.ForSection("Designer.WDSImageCopy")("WIM.Files.Filter")
        Me.Text = LocalizationService.ForSection("Designer.WDSImageCopy")("Image.Win.Deploy.Label")
    End Sub

End Class

Partial Class AddEdgeBrowser

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.AddEdgeBrowser")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.AddEdgeBrowser")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.AddEdgeBrowser")("Microsoft.Label")
        Me.Text = LocalizationService.ForSection("Designer.AddEdgeBrowser")("Microsoft.Label")
    End Sub

End Class

Partial Class AddEdgeFull

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.AddEdgeFull")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.AddEdgeFull")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.AddEdgeFull")("Microsoft.Label")
        Me.Text = LocalizationService.ForSection("Designer.AddEdgeFull")("Microsoft.Label")
    End Sub

End Class

Partial Class AddEdgeWebView

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.Add.Edge")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.Add.Edge")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.Add.Edge")("Microsoft.Web.Label")
        Me.Text = LocalizationService.ForSection("Designer.Add.Edge")("Microsoft.Web.Label")
    End Sub

End Class

Partial Class NewProj

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.NewProj")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.NewProj")("Cancel.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.NewProj")("Options.Required.Label")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.NewProj")("Project.Group")
        Me.Button1.Text = LocalizationService.ForSection("Designer.NewProj")("Browse.Button")
        Me.Label4.Text = LocalizationService.ForSection("Designer.NewProj")("Location.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.NewProj")("Name.Label")
        Me.FolderBrowserDialog1.Description = LocalizationService.ForSection("Designer.NewProj")("Folder.Store.Description")
        Me.Label5.Text = LocalizationService.ForSection("Designer.NewProj")("Fields.End.Required.Label")
        Me.Text = LocalizationService.ForSection("Designer.NewProj")("Create.Project.Label")
    End Sub

End Class

Partial Class ProjProperties

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ProjProps")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ProjProps")("Cancel.Button")
        Me.Label8.Text = LocalizationService.ForSection("Designer.ProjProps")("ProjectGUID.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.ProjProps")("CreationDate.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.ProjProps")("Location.Label")
        Me.Label12.Text = LocalizationService.ForSection("Designer.ProjProps")("ProjGuid.Label")
        Me.Label11.Text = LocalizationService.ForSection("Designer.ProjProps")("ProjTzdata.Label")
        Me.Label10.Text = LocalizationService.ForSection("Designer.ProjProps")("ProjPath.Label")
        Me.Label9.Text = LocalizationService.ForSection("Designer.ProjProps")("ProjName.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.ProjProps")("Name.Label")
        Me.RemountImgBtn.Text = LocalizationService.ForSection("Designer.ProjProps")("RemountImg.Label")
        Me.RecoverButton.Text = LocalizationService.ForSection("Designer.ProjProps")("Recover.Label")
        Me.Label13.Text = LocalizationService.ForSection("Designer.ProjProps")("MountDirectory.Label")
        Me.Label58.Text = LocalizationService.ForSection("Designer.ProjProps")("Installed.Languages.Label")
        Me.Label60.Text = LocalizationService.ForSection("Designer.ProjProps")("FileFormat.Label")
        Me.Label57.Text = LocalizationService.ForSection("Designer.ProjProps")("ModificationDate.Label")
        Me.Label55.Text = LocalizationService.ForSection("Designer.ProjProps")("CreationDate.Label")
        Me.Label53.Text = LocalizationService.ForSection("Designer.ProjProps")("FileCount.Label")
        Me.Label51.Text = LocalizationService.ForSection("Designer.ProjProps")("DirectoryCount.Label")
        Me.Label49.Text = LocalizationService.ForSection("Designer.ProjProps")("System.Root.Dir.Label")
        Me.Label47.Text = LocalizationService.ForSection("Designer.ProjProps")("ProductSuite.Label")
        Me.Label45.Text = LocalizationService.ForSection("Designer.ProjProps")("ProductType.Label")
        Me.Label43.Text = LocalizationService.ForSection("Designer.ProjProps")("Edition.Label")
        Me.Label41.Text = LocalizationService.ForSection("Designer.ProjProps")("ServicePackLevel.Label")
        Me.Label39.Text = LocalizationService.ForSection("Designer.ProjProps")("ServicePackBuild.Label")
        Me.Label37.Text = LocalizationService.ForSection("Designer.ProjProps")("HAL.Label")
        Me.Label35.Text = LocalizationService.ForSection("Designer.ProjProps")("Architecture.Label")
        Me.Label33.Text = LocalizationService.ForSection("Designer.ProjProps")("Supports.WIM.Boot.Label")
        Me.Label22.Text = LocalizationService.ForSection("Designer.ProjProps")("ImageStatus.Label")
        Me.Label14.Text = LocalizationService.ForSection("Designer.ProjProps")("ImageIndex.Label")
        Me.Label31.Text = LocalizationService.ForSection("Designer.ProjProps")("Size.Label")
        Me.Label29.Text = LocalizationService.ForSection("Designer.ProjProps")("Description.Label")
        Me.Label27.Text = LocalizationService.ForSection("Designer.ProjProps")("Name.Label")
        Me.Label25.Text = LocalizationService.ForSection("Designer.ProjProps")("Version.Label")
        Me.Label15.Text = LocalizationService.ForSection("Designer.ProjProps")("ImageFile.Label")
        Me.imgFormat.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgFormat.Label")
        Me.imgModification.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgModification.Label")
        Me.imgCreation.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgCreation.Label")
        Me.imgFiles.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgFiles.Label")
        Me.imgDirs.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgDirs.Label")
        Me.imgSysRoot.Text = LocalizationService.ForSection("Designer.ProjProps")("Img.Sys.Root.Label")
        Me.imgPSuite.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgPsuite.Label")
        Me.imgPType.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgPtype.Label")
        Me.imgEdition.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgEdition.Label")
        Me.imgSPLvl.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgSplvl.Label")
        Me.imgSPBuild.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgSpbuild.Label")
        Me.imgHal.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgHal.Label")
        Me.imgMountDir.Text = LocalizationService.ForSection("Designer.ProjProps")("Img.Mount.Dir.Label")
        Me.imgArch.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgArch.Label")
        Me.imgWimBootStatus.Text = LocalizationService.ForSection("Designer.ProjProps")("Img.WIM.Boot.Label")
        Me.imgMountedStatus.Text = LocalizationService.ForSection("Designer.ProjProps")("Img.Mounted.Status.Label")
        Me.imgSize.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgSize.Label")
        Me.imgMountedDesc.Text = LocalizationService.ForSection("Designer.ProjProps")("Img.Mounted.Desc.Label")
        Me.imgMountedName.Text = LocalizationService.ForSection("Designer.ProjProps")("Img.Mounted.Name.Label")
        Me.imgVersion.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgVersion.Label")
        Me.imgIndex.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgIndex.Label")
        Me.imgName.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgName.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ProjProps")("Getting.Project.Image.Label")
        Me.FfuInfoBtn.Text = LocalizationService.ForSection("Designer.ProjProps")("View.Ffuinformation.Label")
        Me.RWRemountBtn.Text = LocalizationService.ForSection("Designer.ProjProps")("Remount.Write.Label")
        Me.imgRW.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgRW.Label")
        Me.Label62.Text = LocalizationService.ForSection("Designer.ProjProps")("Image.Rwpermissions.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ProjProps")("InstallationType.Label")
        Me.imgInstType.Text = LocalizationService.ForSection("Designer.ProjProps")("Img.Inst.Type.Label")
        Me.Label20.Text = LocalizationService.ForSection("Designer.ProjProps")("Image.Present.Project.Label")
        Me.Label19.Text = LocalizationService.ForSection("Designer.ProjProps")("ImgStatus.Label")
        Me.LinkLabel2.Text = LocalizationService.ForSection("Designer.ProjProps")("Many.Cannot.Seen.Message")
        Me.Text = LocalizationService.ForSection("Designer.ProjProps")("Props.Label")
    End Sub

End Class

Partial Class ImgConversionSuccessDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label2.Text = LocalizationService.ForSection("Designer.ImageConvert")("Converted.Message")
        Me.Label1.Text = LocalizationService.ForSection("Designer.ImageConvert")("Converted.Label")
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ImageConvert")("Yes.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ImageConvert")("No.Button")
        Me.Text = LocalizationService.ForSection("Designer.ImageConvert")("AppName.Label")
    End Sub

End Class

Partial Class ImgWinVistaIncompatibilityDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label2.Text = LocalizationService.ForSection("Designer.VistaWarning")("Unsupported.Message")
        Me.Label1.Text = LocalizationService.ForSection("Designer.VistaWarning")("Windows.Service.Message")
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.VistaWarning")("Yes.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.VistaWarning")("No.Button")
        Me.Text = LocalizationService.ForSection("Designer.VistaWarning")("DISMTools.Label")
    End Sub

End Class

Partial Class MountOpDirCreationDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.MountDirCreation")("Create.Label")
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.MountDirCreation")("Yes.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.MountDirCreation")("No.Button")
        Me.Text = LocalizationService.ForSection("Designer.MountDirCreation")("MountImage.Label")
    End Sub

End Class

Partial Class OrphanedMountedImgDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.OrphanedMount")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.OrphanedMount")("Cancel.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.OrphanedMount")("Project.Has.Orphans.Message")
        Me.Label1.Text = LocalizationService.ForSection("Designer.OrphanedMount")("Servicing.Session.Label")
        Me.Text = LocalizationService.ForSection("Designer.OrphanedMount")("DISMTools.Label")
    End Sub

End Class

Partial Class ReloadProjectQuestionDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ReloadProject")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ReloadProject")("Cancel.Button")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ReloadProject")("ImageUnavailable.Message")
        Me.Label1.Text = LocalizationService.ForSection("Designer.ReloadProject")("ImageMissing.Label")
        Me.Text = LocalizationService.ForSection("Designer.ReloadProject")("DISMTools.Label")
    End Sub

End Class

Partial Class SaveProjectQuestionDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Yes_Button.Text = LocalizationService.ForSection("Designer.SaveProject")("Yes.Button")
        Me.No_Button.Text = LocalizationService.ForSection("Designer.SaveProject")("No.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.SaveProject")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.SaveProject")("SaveChanges.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.SaveProject")("Shutdown.Message")
        Me.Text = LocalizationService.ForSection("Designer.SaveProject")("AppName.Label")
    End Sub

End Class

Partial Class AutoReloadForm

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.AutoReloadForm")("Wait.Message")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.AutoReloadForm")("ImageInfo.Group")
        Me.Label4.Text = LocalizationService.ForSection("Designer.AutoReloadForm")("Image.Mount.Point.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.AutoReloadForm")("ImageFile.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.AutoReloadForm")("Wait.Label")
        Me.Text = LocalizationService.ForSection("Designer.AutoReloadForm")("DISMTools.Label")
    End Sub

End Class

Partial Class SplashScreen

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.VersionLabel.Text = LocalizationService.ForSection("Designer.SplashScreen")("VersionLabel.Label")
        Me.Text = LocalizationService.ForSection("Designer.SplashScreen")("DISM.Tools.Starting.Button")
    End Sub

End Class

Partial Class NewUnattendWiz

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.StepsTreeView.Nodes(0).Text = LocalizationService.ForSection("Designer.Unattend")("Welcome.Label")
        Me.StepsTreeView.Nodes(1).Text = LocalizationService.ForSection("Designer.Unattend")("RegionalConfig.Label")
        Me.StepsTreeView.Nodes(2).Text = LocalizationService.ForSection("Designer.Unattend")("Basic.System.Config.Label")
        Me.StepsTreeView.Nodes(3).Text = LocalizationService.ForSection("Designer.Unattend")("TreeNode.Label")
        Me.StepsTreeView.Nodes(4).Text = LocalizationService.ForSection("Designer.Unattend")("DiskConfig.Label")
        Me.StepsTreeView.Nodes(5).Text = LocalizationService.ForSection("Designer.Unattend")("ProductKey.Label")
        Me.StepsTreeView.Nodes(6).Text = LocalizationService.ForSection("Designer.Unattend")("UserAccounts.Label")
        Me.StepsTreeView.Nodes(7).Text = LocalizationService.ForSection("Designer.Unattend")("VirtualMachine.Support.Label")
        Me.StepsTreeView.Nodes(8).Text = LocalizationService.ForSection("Designer.Unattend")("Wireless.Networking.Label")
        Me.StepsTreeView.Nodes(9).Text = LocalizationService.ForSection("Designer.Unattend")("SystemTelemetry.Label")
        Me.StepsTreeView.Nodes(10).Text = LocalizationService.ForSection("Designer.Unattend")("PostInstall.Scripts.Label")
        Me.StepsTreeView.Nodes(11).Text = LocalizationService.ForSection("Designer.Unattend")("Component.Settings.Label")
        Me.StepsTreeView.Nodes(12).Text = LocalizationService.ForSection("Designer.Unattend")("Finish.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.Unattend")("EditorMode.Label")
        Me.Label1.Text = LocalizationService.ForSection("Designer.Unattend")("ExpressMode.Label")
        Me.Label59.Text = LocalizationService.ForSection("Designer.Unattend")("Notereturn.Applying.Label")
        Me.LinkLabel7.Text = LocalizationService.ForSection("Designer.Unattend")("EditAnswerFile.Link")
        Me.LinkLabel6.Text = LocalizationService.ForSection("Designer.Unattend")("Open.Windows.System.Link")
        Me.LinkLabel4.Text = LocalizationService.ForSection("Designer.Unattend")("Apply.Unattended.Link")
        Me.LinkLabel3.Text = LocalizationService.ForSection("Designer.Unattend")("Open.Location.File.Link")
        Me.LinkLabel2.Text = LocalizationService.ForSection("Designer.Unattend")("Create.Another.Link")
        Me.Label58.Text = LocalizationService.ForSection("Designer.Unattend")("FileCreated.Message")
        Me.FinishHeader.Text = LocalizationService.ForSection("Designer.Unattend")("Congratulations.Done.Label")
        Me.Label57.Text = LocalizationService.ForSection("Designer.Unattend")("Wait.Take.Label")
        Me.Label56.Text = LocalizationService.ForSection("Designer.Unattend")("Progress.Label")
        Me.UnattendProgressHeader.Text = LocalizationService.ForSection("Designer.Unattend")("Wait.UnattendAnswer.Button")
        Me.Label54.Text = LocalizationService.ForSection("Designer.Unattend")("Something.Right.Go.Message")
        Me.CheckBox17.Text = LocalizationService.ForSection("Designer.Unattend")("WordWrap.CheckBox")
        Me.FinalReviewHeader.Text = LocalizationService.ForSection("Designer.Unattend")("ReviewSettings.Label")
        Me.Label65.Text = LocalizationService.ForSection("Designer.Unattend")("Don.Twant.Add.Label")
        Me.Label15.Text = LocalizationService.ForSection("Designer.Unattend")("No.Custom.None.Message")
        Me.LinkLabel5.Text = LocalizationService.ForSection("Designer.Unattend")("Learn.Custom.Link")
        Me.Label64.Text = LocalizationService.ForSection("Designer.Unattend")("Pass.Label")
        Me.Label61.Text = LocalizationService.ForSection("Designer.Unattend")("Component.Label")
        Me.Label60.Text = LocalizationService.ForSection("Designer.Unattend")("Component.Count.Label")
        Me.LinkLabel9.Text = LocalizationService.ForSection("Designer.Unattend")("Learn.Component.Link")
        Me.Label52.Text = LocalizationService.ForSection("Designer.Unattend")("Screen.Add.Message")
        Me.ComponentHeader.Text = LocalizationService.ForSection("Designer.Unattend")("Components.Label")
        Me.CheckBox22.Text = LocalizationService.ForSection("Designer.Unattend")("Hide.Script.Windows.CheckBox")
        Me.CheckBox20.Text = LocalizationService.ForSection("Designer.Unattend")("RestartExplorer.CheckBox")
        Me.Button20.Text = LocalizationService.ForSection("Designer.Unattend")("Import.StarterScript.Button")
        Me.Button19.Text = LocalizationService.ForSection("Designer.Unattend")("ImportScript.Button")
        Me.Label67.Text = LocalizationService.ForSection("Designer.Unattend")("Language.Label")
        Me.Button4.Text = LocalizationService.ForSection("Designer.Unattend")("OpenScript.Button")
        Me.Label68.Text = LocalizationService.ForSection("Designer.Unattend")("Scripts.Have.None.Message")
        Me.Label66.Text = LocalizationService.ForSection("Designer.Unattend")("Script.Count.Label")
        Me.StageEditorDescriptionLabel.Text = LocalizationService.ForSection("Designer.Unattend")("ScriptRun.Description")
        Me.StageLink1.Text = LocalizationService.ForSection("Designer.Unattend")("System.Config.Link")
        Me.StageLink2.Text = LocalizationService.ForSection("Designer.Unattend")("First.User.Logs.Link")
        Me.StageLink3.Text = LocalizationService.ForSection("Designer.Unattend")("Whenever.User.Logs.Link")
        Me.Label51.Text = LocalizationService.ForSection("Designer.Unattend")("ScriptScreenHelp.Message")
        Me.PostInstallHeader.Text = LocalizationService.ForSection("Designer.Unattend")("Run.Install.Label")
        Me.RadioButton27.Text = LocalizationService.ForSection("Designer.Unattend")("EnableTelemetry.RadioButton")
        Me.RadioButton26.Text = LocalizationService.ForSection("Designer.Unattend")("DisableTelemetry.RadioButton")
        Me.CheckBox16.Text = LocalizationService.ForSection("Designer.Unattend")("ConfigureSettings.CheckBox")
        Me.SystemTelemetryHeader.Text = LocalizationService.ForSection("Designer.Unattend")("Control.Limit.Much.Message")
        Me.RadioButton25.Text = LocalizationService.ForSection("Designer.Unattend")("WirelessSettings.RadioButton")
        Me.LinkLabel8.Text = LocalizationService.ForSection("Designer.Unattend")("Access.Router.Config.Link")
        Me.ComboBox13.Items(0) = LocalizationService.ForSection("Designer.Unattend")("Open.Least.Secure.Item")
        Me.ComboBox13.Items(1) = LocalizationService.ForSection("Designer.Unattend")("Wpapsk.Item")
        Me.ComboBox13.Items(2) = LocalizationService.ForSection("Designer.Unattend")("Wpasae.Item")
        Me.CheckBox15.Text = LocalizationService.ForSection("Designer.Unattend")("ConnectHidden.CheckBox")
        Me.Label49.Text = LocalizationService.ForSection("Designer.Unattend")("Password.Label")
        Me.Label48.Text = LocalizationService.ForSection("Designer.Unattend")("AuthTechnology.Label")
        Me.Label50.Text = LocalizationService.ForSection("Designer.Unattend")("Technology.Both.Choose.Label")
        Me.Label47.Text = LocalizationService.ForSection("Designer.Unattend")("SsidnetworkName.Label")
        Me.RadioButton30.Text = LocalizationService.ForSection("Designer.Unattend")("SkipConfig.RadioButton")
        Me.Label55.Text = LocalizationService.ForSection("Designer.Unattend")("Option.Either.Choose.Label")
        Me.CheckBox14.Text = LocalizationService.ForSection("Designer.Unattend")("ConfigureSettings.CheckBox")
        Me.NetworkConnectionHeader.Text = LocalizationService.ForSection("Designer.Unattend")("WirelessSettings.Label")
        Me.Label46.Text = LocalizationService.ForSection("Designer.Unattend")("Guest.Additions.Message")
        Me.ComboBox8.Items(0) = LocalizationService.ForSection("Designer.Unattend")("Virtual.Box.Guest.Item")
        Me.ComboBox8.Items(1) = LocalizationService.ForSection("Designer.Unattend")("VmwareTools.Item")
        Me.ComboBox8.Items(2) = LocalizationService.ForSection("Designer.Unattend")("Virt.Ioguest.Tools.Item")
        Me.ComboBox8.Items(3) = LocalizationService.ForSection("Designer.Unattend")("ParallelsTools.Item")
        Me.ComboBox8.Text = LocalizationService.ForSection("Designer.Unattend")("Virt.Ioguest.Tools.Item")
        Me.Label45.Text = LocalizationService.ForSection("Designer.Unattend")("VirtualMachine.Label")
        Me.RadioButton24.Text = LocalizationService.ForSection("Designer.Unattend")("Iplan.Target.RadioButton")
        Me.RadioButton23.Text = LocalizationService.ForSection("Designer.Unattend")("Iwant.Target.RadioButton")
        Me.VirtualMachineHeader.Text = LocalizationService.ForSection("Designer.Unattend")("Add.Enhanced.Support.Message")
        Me.Label44.Text = LocalizationService.ForSection("Designer.Unattend")("Checking.Option.Target.Label")
        Me.Label41.Text = LocalizationService.ForSection("Designer.Unattend")("Amount.Failed.Attempts.Label")
        Me.Label43.Text = LocalizationService.ForSection("Designer.Unattend")("UnlockMinutes.Label")
        Me.Label40.Text = LocalizationService.ForSection("Designer.Unattend")("Lock.Out.Account.Label")
        Me.Label42.Text = LocalizationService.ForSection("Designer.Unattend")("TimeframeMinutes.Label")
        Me.RadioButton22.Text = LocalizationService.ForSection("Designer.Unattend")("CustomLockout.RadioButton")
        Me.RadioButton21.Text = LocalizationService.ForSection("Designer.Unattend")("DefaultLockout.RadioButton")
        Me.CheckBox13.Text = LocalizationService.ForSection("Designer.Unattend")("DisablePolicy.CheckBox")
        Me.AccountLockdownHeader.Text = LocalizationService.ForSection("Designer.Unattend")("AccountLockout.Label")
        Me.Label39.Text = LocalizationService.ForSection("Designer.Unattend")("Days.Label")
        Me.RadioButton20.Text = LocalizationService.ForSection("Designer.Unattend")("ExpirePassword.RadioButton")
        Me.RadioButton19.Text = LocalizationService.ForSection("Designer.Unattend")("Expire42Days.RadioButton")
        Me.RadioButton18.Text = LocalizationService.ForSection("Designer.Unattend")("PasswordsExpire.RadioButton")
        Me.RadioButton17.Text = LocalizationService.ForSection("Designer.Unattend")("NeverExpire.RadioButton")
        Me.PWExpirationHeader.Text = LocalizationService.ForSection("Designer.Unattend")("PasswordsExpire.Label")
        Me.Label34.Text = LocalizationService.ForSection("Designer.NewUnattend.LocalAccounts")("OnlyNow.Label")
        Me.CheckBox6.Text = LocalizationService.ForSection("Designer.Unattend")("ConfigureSettings.CheckBox")
        Me.Label35.Text = LocalizationService.ForSection("Designer.Unattend")("AccountName.Label")
        Me.Label38.Text = LocalizationService.ForSection("Designer.Unattend")("Account.Label")
        Me.CheckBox8.Text = LocalizationService.ForSection("Designer.Unattend")("Account.Option2.CheckBox")
        Me.CheckBox9.Text = LocalizationService.ForSection("Designer.Unattend")("Account.Option3.CheckBox")
        Me.CheckBox10.Text = LocalizationService.ForSection("Designer.Unattend")("Account.Option4.CheckBox")
        Me.CheckBox11.Text = LocalizationService.ForSection("Designer.Unattend")("Account.Option5.CheckBox")
        Me.UserListOverviewLabel.Text = LocalizationService.ForSection("Designer.Unattend")("UserList.Label")
        Me.Label37.Text = LocalizationService.ForSection("Designer.Unattend")("AccountGroup.Label")
        Me.Label36.Text = LocalizationService.ForSection("Designer.Unattend")("AccountPassword.Label")
        Me.Label69.Text = LocalizationService.ForSection("Designer.Unattend")("Account.Display.Name.Label")
        Me.GroupBox1.Text = LocalizationService.ForSection("Designer.Unattend")("FirstLog.Group")
        Me.RadioButton16.Text = LocalizationService.ForSection("Designer.Unattend")("Log.Built.Admin.RadioButton")
        Me.RadioButton15.Text = LocalizationService.ForSection("Designer.Unattend")("Log.First.Admin.RadioButton")
        Me.CheckBox12.Text = LocalizationService.ForSection("Designer.Unattend")("Auto.Login.Admin.CheckBox")
        Me.CheckBox7.Text = LocalizationService.ForSection("Designer.Unattend")("ObscurePasswords.CheckBox")
        Me.CheckBox18.Text = LocalizationService.ForSection("Designer.Unattend")("Ask.Microsoft.CheckBox")
        Me.UserAccountHeader.Text = LocalizationService.ForSection("Designer.Unattend")("Target.Install.Label")
        Me.CheckBox21.Text = LocalizationService.ForSection("Designer.Unattend")("FirmwareProductKey.CheckBox")
        Me.Label32.Text = LocalizationService.ForSection("Designer.Unattend")("Product.Label")
        Me.Label31.Text = LocalizationService.ForSection("Designer.Unattend")("DISM.Tools.Cannot.Label")
        Me.Label33.Text = LocalizationService.ForSection("Designer.Unattend")("Type.Each.Character.Label")
        Me.Label30.Text = LocalizationService.ForSection("Designer.Unattend")("ProductKey.Custom.Label")
        Me.Button21.Text = LocalizationService.ForSection("Designer.Unattend")("Detect.Image.Edition.Button")
        Me.Button5.Text = LocalizationService.ForSection("Designer.Unattend")("Copy.Button")
        Me.Label29.Text = LocalizationService.ForSection("Designer.Unattend")("Only.Generic.Key.Label")
        Me.Label28.Text = LocalizationService.ForSection("Designer.Unattend")("ProductKey.Generic.Label")
        Me.Label27.Text = LocalizationService.ForSection("Designer.Unattend")("ProductKey.Edition.Label")
        Me.RadioButton14.Text = LocalizationService.ForSection("Designer.Unattend")("CustomProductKey.RadioButton")
        Me.RadioButton13.Text = LocalizationService.ForSection("Designer.Unattend")("GenericKey.RadioButton")
        Me.ProductKeyHeader.Text = LocalizationService.ForSection("Designer.Unattend")("ProductKey.Type.Label")
        Me.Label23.Text = LocalizationService.ForSection("Designer.Unattend")("RecoveryPartition.Label")
        Me.CheckBox5.Text = LocalizationService.ForSection("Designer.Unattend")("InstallRecoveryEnv.CheckBox")
        Me.Label22.Text = LocalizationService.ForSection("Designer.Unattend")("EFI.System.Label")
        Me.RadioButton8.Text = LocalizationService.ForSection("Designer.Unattend")("MBR.RadioButton")
        Me.RadioButton7.Text = LocalizationService.ForSection("Designer.Unattend")("GPT.RadioButton")
        Me.Label21.Text = LocalizationService.ForSection("Designer.Unattend")("PartitionTable.Label")
        Me.Label20.Text = LocalizationService.ForSection("Designer.Unattend")("Skip.Disk.Config.Label")
        Me.CheckBox4.Text = LocalizationService.ForSection("Designer.Unattend")("ConfigureSettings.CheckBox")
        Me.DiskConfigurationHeader.Text = LocalizationService.ForSection("Designer.Unattend")("DiskLayout.Label")
        Me.CurrentTimeSelTZ.Text = LocalizationService.ForSection("Designer.Unattend")("Time.Label")
        Me.CurrentTimeUTC.Text = LocalizationService.ForSection("Designer.Unattend")("CurrentTime.Label")
        Me.Label19.Text = LocalizationService.ForSection("Designer.Unattend")("Time.Selected.Zone.Label")
        Me.Label18.Text = LocalizationService.ForSection("Designer.Unattend")("Time.UTC.Label")
        Me.Label17.Text = LocalizationService.ForSection("Designer.Unattend")("TimeZone.Label")
        Me.RadioButton4.Text = LocalizationService.ForSection("Designer.Unattend")("Set.Time.Zone.RadioButton")
        Me.RadioButton3.Text = LocalizationService.ForSection("Designer.Unattend")("Windows.Decide.RadioButton")
        Me.TimeZoneHeader.Text = LocalizationService.ForSection("Designer.Unattend")("Configure.Time.Zone.Label")
        Me.CheckedListBox1.Items(0) = LocalizationService.ForSection("Designer.Unattend")("DesktopX86.Item")
        Me.CheckedListBox1.Items(1) = LocalizationService.ForSection("Designer.Unattend")("DesktopX64.Item")
        Me.CheckedListBox1.Items(2) = LocalizationService.ForSection("Designer.Unattend")("Armwindows.Item")
        Me.CheckBox19.Text = LocalizationService.ForSection("Designer.Unattend")("UseConfigSet.CheckBox")
        Me.CheckBox3.Text = LocalizationService.ForSection("Designer.Unattend")("Windows.Set.Random.CheckBox")
        Me.Label62.Text = LocalizationService.ForSection("Designer.Unattend")("Config.Set.Message")
        Me.RadioButton29.Text = LocalizationService.ForSection("Designer.Unattend")("Script.Sets.Name.RadioButton")
        Me.RadioButton28.Text = LocalizationService.ForSection("Designer.Unattend")("ComputerName.RadioButton")
        Me.Button3.Text = LocalizationService.ForSection("Designer.Unattend")("Get.Computer.Name.Button")
        Me.Label16.Text = LocalizationService.ForSection("Designer.Unattend")("ComputerName.Label")
        Me.Label63.Text = LocalizationService.ForSection("Designer.Unattend")("Type.Computer.Name.Label")
        Me.Label14.Text = LocalizationService.ForSection("Designer.Unattend")("Check.Option.Only.Message")
        Me.CheckBox2.Text = LocalizationService.ForSection("Designer.Unattend")("BypassNetwork.CheckBox")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.Unattend")("BypassRequirements.CheckBox")
        Me.Label13.Text = LocalizationService.ForSection("Designer.Unattend")("Windows11.Label")
        Me.Label12.Text = LocalizationService.ForSection("Designer.Unattend")("System.Architec.Label")
        Me.Label11.Text = LocalizationService.ForSection("Designer.Unattend")("Processor.Architecture.Label")
        Me.SysConfigHeader.Text = LocalizationService.ForSection("Designer.Unattend")("BasicSettings.Label")
        Me.Label10.Text = LocalizationService.ForSection("Designer.Unattend")("Configure.Settings.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.Unattend")("SystemLanguage.Label")
        Me.Label7.Text = LocalizationService.ForSection("Designer.Unattend")("SystemLocale.Label")
        Me.Label9.Text = LocalizationService.ForSection("Designer.Unattend")("HomeLocation.Label")
        Me.Label8.Text = LocalizationService.ForSection("Designer.Unattend")("Keyboard.Layout.IME.Label")
        Me.Button22.Text = LocalizationService.ForSection("Designer.Unattend")("Country.EEA.Choose.Button")
        Me.Button1.Text = LocalizationService.ForSection("Designer.Unattend")("Additional.Layouts.Button")
        Me.RadioButton2.Text = LocalizationService.ForSection("Designer.Unattend")("ConfigureLater.RadioButton")
        Me.RadioButton1.Text = LocalizationService.ForSection("Designer.Unattend")("SettingsNow.RadioButton")
        Me.RegionalSettingsHeader.Text = LocalizationService.ForSection("Designer.Unattend")("LanguageKeyboard.Label")
        Me.LinkLabel10.Text = LocalizationService.ForSection("Designer.Unattend")("Copy.Linux.Mac.Link")
        Me.LinkLabel1.Text = LocalizationService.ForSection("Designer.Unattend")("OnlineGenerator.Link")
        Me.WelcomeHeader.Text = LocalizationService.ForSection("Designer.Unattend")("Welcome.Unattended.Label")
        Me.WelcomeDesc.Text = LocalizationService.ForSection("Designer.Unattend")("CreationHelp.Message")
        Me.Label5.Text = LocalizationService.ForSection("Designer.Unattend")("AvailableNow.Label")
        Me.ToolStripButton2.Text = LocalizationService.ForSection("Designer.Unattend")("NewOverwrite.Label")
        Me.ToolStripButton3.Text = LocalizationService.ForSection("Designer.Unattend")("Open.Button")
        Me.ToolStripButton4.Text = LocalizationService.ForSection("Designer.Unattend")("Save.Button")
        Me.ToolStripButton5.Text = LocalizationService.ForSection("Designer.Unattend")("WordWrap.Label")
        Me.ToolStripButton6.Text = LocalizationService.ForSection("Designer.Unattend")("Help.Label")
        Me.ToolStripButton1.Text = LocalizationService.ForSection("Designer.Unattend")("NormalizeSpacing.Label")
        Me.ToolStripButton1.ToolTipText = LocalizationService.ForSection("Designer.Unattend")("NormalizeSpacing.Tooltip")
        Me.Label4.Text = LocalizationService.ForSection("Designer.Unattend")("WizardHelp.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.Unattend")("ExpressMode.Label")
        Me.Button12.Text = LocalizationService.ForSection("Designer.Unattend")("Join.Target.Device.Button")
        Me.Back_Button.Text = LocalizationService.ForSection("Designer.Unattend")("BackButton.Button")
        Me.Next_Button.Text = LocalizationService.ForSection("Designer.Unattend")("NextButton.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.Unattend")("Cancel.Button")
        Me.Help_Button.Text = LocalizationService.ForSection("Designer.Unattend")("Help.Button")
        Me.SaveFileDialog1.Filter = LocalizationService.ForSection("Designer.Unattend")("Answer.Files.XML.Filter")
        Me.UGNotify.BalloonTipText = LocalizationService.ForSection("Designer.Unattend")("SelfContained.Message")
        Me.UGNotify.BalloonTipTitle = LocalizationService.ForSection("Designer.Unattend")("Gen.Download.Complete.Title")
        Me.EditorModeOFD.Filter = LocalizationService.ForSection("Designer.Unattend")("EditorMode.Filter")
        Me.EditorModeSFD.Filter = LocalizationService.ForSection("Designer.Unattend")("Answer.Files.XML.Filter")
        Me.ScriptEditorOFD.Filter = LocalizationService.ForSection("Designer.Unattend")("Power.Shell.Scripts.Filter")
        Me.ScriptEditorOFD.Title = LocalizationService.ForSection("Designer.Unattend")("OpenScript.Title")
        Me.CPUnattendGenFBD.Description = LocalizationService.ForSection("Designer.Unattend")("Path.Description")
        Me.OpenFileDialog2.Filter = LocalizationService.ForSection("Designer.Unattend")("DISM.Tools.Starter.Filter")
        Me.OpenFileDialog2.Title = LocalizationService.ForSection("Designer.Unattend")("Pick.StarterScript.Title")
        Me.Text = LocalizationService.ForSection("Designer.Unattend")("CreationHelp.Label")
    End Sub

End Class

Partial Class SampleScriptBrowser

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Cancel.Button")
        Me.CreateStarterScriptBtn.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Create.Starter.Button")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Name.Column")
        Me.ComboBox1.Items(0) = LocalizationService.ForSection("Designer.ScriptBrowser")("System.Config.Item")
        Me.ComboBox1.Items(1) = LocalizationService.ForSection("Designer.ScriptBrowser")("First.User.Logs.Item")
        Me.ComboBox1.Items(2) = LocalizationService.ForSection("Designer.ScriptBrowser")("Whenever.User.Logs.Item")
        Me.ComboBox1.Items(3) = LocalizationService.ForSection("Designer.ScriptBrowser")("Scripts.Uploaded.Library.Item")
        Me.ComboBox1.Items(4) = LocalizationService.ForSection("Designer.ScriptBrowser")("Scripts.Defined.User.Item")
        Me.Label1.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Stage.Type.Choose.Label")
        Me.EnterFSModeBtn.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("EnlargePreview.Label")
        Me.ExportScriptCodeBtn.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Export.Code.File.Button")
        Me.Label7.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Okinsert.Label")
        Me.Label6.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("ScriptCode.Label")
        Me.Label5.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Language.Label")
        Me.Label4.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Description.Label")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("ScriptName.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("View.Label")
        Me.Label8.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("StarterScripts.Help.Message")
        Me.ScriptCodeExporterSFD.Title = LocalizationService.ForSection("Designer.ScriptBrowser")("Export.Code.Title")
        Me.Label9.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("Leave.Full.Screen.Label")
        Me.ExitFSModeBtn.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("GoBack.Label")
        Me.Text = LocalizationService.ForSection("Designer.ScriptBrowser")("LoadStarterScript.Label")
    End Sub

End Class

Partial Class ScriptReorderDialog

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.OK_Button.Text = LocalizationService.ForSection("Designer.ScriptReorder")("Ok.Button")
        Me.Cancel_Button.Text = LocalizationService.ForSection("Designer.ScriptReorder")("Cancel.Button")
        Me.Label1.Text = LocalizationService.ForSection("Designer.ScriptReorder")("Dialog.Alter.Order.Message")
        Me.Label3.Text = LocalizationService.ForSection("Designer.ScriptReorder")("ScriptCode.Label")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.ScriptReorder")("Script.Column")
        Me.Label2.Text = LocalizationService.ForSection("Designer.ScriptReorder")("ScriptOrder.Label")
        Me.CheckBox1.Text = LocalizationService.ForSection("Designer.ScriptReorder")("WordWrap.CheckBox")
        Me.Text = LocalizationService.ForSection("Designer.ScriptReorder")("Scripts.Stage.Label")
    End Sub

End Class

Partial Class UnattendMgr

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.UnattendMgr")("ProjectPath.Label")
        Me.Button1.Text = LocalizationService.ForSection("Designer.UnattendMgr")("Browse.Button")
        Me.ColumnHeader1.Text = LocalizationService.ForSection("Designer.UnattendMgr")("FileName.Column")
        Me.ColumnHeader2.Text = LocalizationService.ForSection("Designer.UnattendMgr")("Created.Column")
        Me.ColumnHeader3.Text = LocalizationService.ForSection("Designer.UnattendMgr")("LastModified.Column")
        Me.ColumnHeader4.Text = LocalizationService.ForSection("Designer.UnattendMgr")("LastAccessed.Column")
        Me.Button4.Text = LocalizationService.ForSection("Designer.UnattendMgr")("ApplyImage.Button")
        Me.Button3.Text = LocalizationService.ForSection("Designer.UnattendMgr")("Open.File.Location.Button")
        Me.Button2.Text = LocalizationService.ForSection("Designer.UnattendMgr")("OpenFile.Button")
        Me.Text = LocalizationService.ForSection("Designer.UnattendMgr")("Unattended.AnswerFile.Label")
    End Sub

End Class

Partial Class NewsFeedItemCard

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.FeedItemLinkLabel.Text = LocalizationService.ForSection("Designer.NewsFeedCard")("Item.Title")
        Me.FeedItemDateLabel.Text = LocalizationService.ForSection("Designer.NewsFeedCard")("ItemDate.Label")
    End Sub

End Class

Partial Class WimFileSourceControl

    Protected Overrides Sub OnLoad(e As EventArgs)
        ApplyRuntimeLocalization()
        MyBase.OnLoad(e)
    End Sub

    Private Sub ApplyRuntimeLocalization()
        Me.Label1.Text = LocalizationService.ForSection("Designer.WimFileSource")("ImageFile.Label")
        Me.Label2.Text = LocalizationService.ForSection("Designer.WimFileSource")("ImageIndex.Label")
    End Sub

End Class
