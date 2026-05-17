Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars
Imports System.IO
Imports DISMTools.Utilities
Imports Microsoft.Dism

Public Class RemProvAppxPackage
    Implements IImageTaskDialog

    Public AppxRemovalPackages(65535) As String
    Public AppxRemovalFriendlyNames(65535) As String
    Public AppxRemovalCount As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Disposing of progress panel if not disposed of previously...")
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        AppxRemovalCount = ListView1.CheckedItems.Count
        ProgressPanel.appxRemovalCount = AppxRemovalCount
        DynaLog.LogMessage("Detecting AppX packages to remove...")
        If ListView1.CheckedItems.Count = 0 Then
            DynaLog.LogMessage("No items have been selected for removal.")
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("Please specify AppX packages to remove and try again.", vbOKOnly + vbCritical, "Remove provisioned AppX packages")
                        Case "ESN"
                            MsgBox("Especifique paquetes AppX a eliminar e inténtelo de nuevo.", vbOKOnly + vbCritical, "Eliminar paquetes aprovisionados AppX")
                        Case "FRA"
                            MsgBox("Veuillez indiquer les paquets AppX à supprimer et réessayer.", vbOKOnly + vbCritical, "Supprimer les paquets AppX provisionnés")
                        Case "PTB", "PTG"
                            MsgBox("Especifique os pacotes AppX a remover e tente novamente.", vbOKOnly + vbCritical, "Remover pacotes AppX aprovisionados")
                        Case "ITA"
                            MsgBox("Specificare i pacchetti AppX da rimuovere e riprovare", vbOKOnly + vbCritical, "Rimuovere i pacchetti AppX in dotazione")
                    End Select
                Case 1
                    MsgBox("Please specify AppX packages to remove and try again.", vbOKOnly + vbCritical, "Remove provisioned AppX packages")
                Case 2
                    MsgBox("Especifique paquetes AppX a eliminar e inténtelo de nuevo.", vbOKOnly + vbCritical, "Eliminar paquetes aprovisionados AppX")
                Case 3
                    MsgBox("Veuillez indiquer les paquets AppX à supprimer et réessayer.", vbOKOnly + vbCritical, "Supprimer les paquets AppX provisionnés")
                Case 4
                    MsgBox("Especifique os pacotes AppX a remover e tente novamente.", vbOKOnly + vbCritical, "Remover pacotes AppX aprovisionados")
                Case 5
                    MsgBox("Specificare i pacchetti AppX da rimuovere e riprovare", vbOKOnly + vbCritical, "Rimuovere i pacchetti AppX in dotazione")
            End Select
            Exit Sub
        Else
            DynaLog.LogMessage("AppX packages to remove: " & AppxRemovalCount)
            If AppxRemovalCount > 65535 Then
                MsgBox("Right now, you can only specify less than 65535 AppX packages. This is a program limitation that will be gone in a future update.", vbOKOnly + vbCritical, "Remove provisioned AppX packages")
                Exit Sub
            Else
                DynaLog.LogMessage("Adding AppX packages to queue...")
                For x = 0 To AppxRemovalCount - 1
                    AppxRemovalPackages(x) = ListView1.CheckedItems(x).Text
                Next
                For x = 0 To AppxRemovalCount - 1
                    AppxRemovalFriendlyNames(x) = ListView1.CheckedItems(x).SubItems(1).Text
                Next
                For x = 0 To AppxRemovalPackages.Length - 1
                    ProgressPanel.appxRemovalPackages(x) = AppxRemovalPackages(x)
                Next
                For x = 0 To AppxRemovalFriendlyNames.Length - 1
                    ProgressPanel.appxRemovalPkgNames(x) = AppxRemovalFriendlyNames(x)
                Next
                ProgressPanel.appxRemovalLastPackage = ListView1.CheckedItems(AppxRemovalCount - 1).ToString().Replace("ListViewItem: {", "").Trim().Replace("}", "").Trim()

                ' If the image contains a Server Core/Nano Server installation, detect whether the Desktop Experience
                ' feature is installed
                DynaLog.LogMessage("Detecting conditions imposed by the Windows image...")
                If MainForm.CurrentImage.ImageInstallationType <> "" And (MainForm.CurrentImage.ImageInstallationType.Contains("Nano") Or MainForm.CurrentImage.ImageInstallationType.Contains("Core")) Then
                    DynaLog.LogMessage("Target Windows image contains Server Core SKU. Detecting state of Desktop Experience feature...")
                    ' Go through every feature and find Desktop Experience
                    If MainForm.CurrentImage.ImageFeatures.Count > 0 Then
                        Dim DesktopExperienceEnabled As Boolean = MainForm.CurrentImage.ImageFeatures.Any(Function(feature) feature.FeatureName = "DesktopExperience" AndAlso feature.State = DismPackageFeatureState.Installed)
                        If Not DesktopExperienceEnabled Then
                            DynaLog.LogMessage("Desktop Experience has been detected as a disabled feature.")
                            Dim msg As String = ""
                            ' Display incompatibility
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            msg = "The Desktop Experience (DesktopExperience) feature needs to be enabled in order to remove AppX packages in Windows Server Core/Nano Server images." & CrLf & CrLf & "Enable this feature, boot to the image, and try again."
                                        Case "ESN"
                                            msg = "La característica Experiencia del Escritorio (DesktopExperience) debe estar habilitada para eliminar paquetes AppX en imágenes Windows Server Core/Nano Server." & CrLf & CrLf & "Habilite esta característica, arranque la imagen, e inténtelo de nuevo."
                                        Case "FRA"
                                            msg = "La caractéristique Expérience du bureau (DesktopExperience) doit être activée afin de supprimer les paquets AppX dans les images Windows Server Core/Nano Server." & CrLf & CrLf & "Activez cette caractéristique, démarrez sur l'image et réessayez."
                                        Case "PTB", "PTG"
                                            msg = "A caraterística Área de Trabalho (DesktopExperience) tem de ser ativada para remover pacotes AppX nas imagens do Windows Server Core/Nano Server." & CrLf & CrLf & "Ative esta caraterística, arranque para a imagem e tente novamente."
                                        Case "ITA"
                                            msg = "Le caratteristiche di Esperienza del Desktop (DesktopExperience) devono essere abilitate per rimuovere i pacchetti AppX nelle immagini di Windows Server Core/Nano Server." & CrLf & CrLf & "Abilitate questa caratteristica, avviate l'immagine e riprovate"
                                    End Select
                                Case 1
                                    msg = "The Desktop Experience (DesktopExperience) feature needs to be enabled in order to remove AppX packages in Windows Server Core/Nano Server images." & CrLf & CrLf & "Enable this feature, boot to the image, and try again."
                                Case 2
                                    msg = "La característica Experiencia del Escritorio (DesktopExperience) debe estar habilitada para eliminar paquetes AppX en imágenes Windows Server Core/Nano Server." & CrLf & CrLf & "Habilite esta característica, arranque la imagen, e inténtelo de nuevo."
                                Case 3
                                    msg = "La caractéristique Expérience du bureau (DesktopExperience) doit être activée afin de supprimer les paquets AppX dans les images Windows Server Core/Nano Server." & CrLf & CrLf & "Activez cette caractéristique, démarrez sur l'image et réessayez."
                                Case 4
                                    msg = "A caraterística Área de Trabalho (DesktopExperience) tem de ser ativada para remover pacotes AppX nas imagens do Windows Server Core/Nano Server." & CrLf & CrLf & "Ative esta caraterística, arranque para a imagem e tente novamente."
                                Case 5
                                    msg = "Le caratteristiche di Esperienza del Desktop (DesktopExperience) devono essere abilitate per rimuovere i pacchetti AppX nelle immagini di Windows Server Core/Nano Server." & CrLf & CrLf & "Abilitate questa caratteristica, avviate l'immagine e riprovate"
                            End Select
                            MsgBox(msg, vbOKOnly + vbCritical, ImageTaskHeader1.ItemText)
                            Exit Sub
                        End If
                    End If
                End If
            End If
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 38
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Function Initialize() As Boolean Implements IImageTaskDialog.Initialize
        AppxHelper.ClearRootPaths()
        AppxHelper.SetRootPaths(MainForm.MountDir)
        DynaLog.LogMessage("Checking edition and version information for any unmet requirements...")
        If MainForm.CurrentImage.ImageEditionId.Equals("WindowsPE", StringComparison.OrdinalIgnoreCase) Or Not MainForm.IsWindows8OrHigher(MainForm.MountDir & "\Windows\system32\ntoskrnl.exe") Then
            DynaLog.LogMessage("The image is not supported")
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                        Case "ESN"
                            MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                        Case "FRA"
                            MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                        Case "PTB", "PTG"
                            MsgBox("Esta ação não é suportada nesta imagem", vbOKOnly + vbCritical, Text)
                        Case "ITA"
                            MsgBox("Questa azione non è supportata su questa immagine", vbOKOnly + vbCritical, Text)
                    End Select
                Case 1
                    MsgBox("This action is not supported on this image", vbOKOnly + vbCritical, Text)
                Case 2
                    MsgBox("Esta acción no está soportada en esta imagen", vbOKOnly + vbCritical, Text)
                Case 3
                    MsgBox("Cette action n'est pas prise en charge sur cette image", vbOKOnly + vbCritical, Text)
                Case 4
                    MsgBox("Esta ação não é suportada nesta imagem", vbOKOnly + vbCritical, Text)
                Case 5
                    MsgBox("Questa azione non è supportata su questa immagine", vbOKOnly + vbCritical, Text)
            End Select
            Return False
        End If
        DynaLog.LogMessage("All requirements are met. Continuing with the task...")
        ListView1.Items.Clear()
        If Not MainForm.CompletedTasks(2) Then
            DynaLog.LogMessage("AppX package background processes haven't completed.")
            BGProcsBusyDialog.ShowDialog(Me)
            Return False
        End If
        DynaLog.LogMessage("Adding AppX packages to arrays...")
        If MainForm.CurrentImage.ImageAppxPackages Is Nothing OrElse MainForm.CurrentImage.ImageAppxPackages_Backup.Count > MainForm.CurrentImage.ImageAppxPackages.Count Then
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageAppxPackages_Backup.Select(Function(appxPackage) New ListViewItem(New String() {appxPackage.PackageFullName,
                                                                                                                                              String.Format("{0}{1}", If(MainForm.AppxDisplayNameFormatOnRemoval < 2, appxPackage.PackageName, ""),
                                                                                                                                                            If(MainForm.AppxDisplayNameFormatOnRemoval > 0,
                                                                                                                                                               If(MainForm.AppxDisplayNameFormatOnRemoval < 2,
                                                                                                                                                                  " (" & AppxHelper.GetPackageDisplayName(MainForm.MountDir, appxPackage.PackageFullName, appxPackage.PackageName) & ")",
                                                                                                                                                                  AppxHelper.GetPackageDisplayName(MainForm.MountDir, appxPackage.PackageFullName, appxPackage.PackageName)
                                                                                                                                               ), "")),
                                                                                                                                              Casters.CastDismArchitecture(appxPackage.PackageArchitecture),
                                                                                                                                              appxPackage.PackageResourceId,
                                                                                                                                              appxPackage.PackageVersion.ToString(),
                                                                                                                                              appxPackage.GetLocalizedRegistrationStatus(MainForm.MountDir, MainForm.Language)})).ToArray())
        Else
            ListView1.Items.AddRange(MainForm.CurrentImage.ImageAppxPackages.Select(Function(appxPackage) New ListViewItem(New String() {appxPackage.PackageName,
                                                                                                                                         String.Format("{0}{1}", If(MainForm.AppxDisplayNameFormatOnRemoval < 2, appxPackage.PackageName, ""),
                                                                                                                                                       If(MainForm.AppxDisplayNameFormatOnRemoval > 0,
                                                                                                                                                          If(MainForm.AppxDisplayNameFormatOnRemoval < 2,
                                                                                                                                                             " (" & AppxHelper.GetPackageDisplayName(MainForm.MountDir, appxPackage.PackageName, appxPackage.DisplayName) & ")",
                                                                                                                                                             AppxHelper.GetPackageDisplayName(MainForm.MountDir, appxPackage.PackageName, appxPackage.DisplayName)
                                                                                                                                        ), "")),
                                                                                                                                         Casters.CastDismArchitecture(appxPackage.Architecture),
                                                                                                                                         appxPackage.ResourceId,
                                                                                                                                         appxPackage.Version.ToString(),
                                                                                                                                         If(IsPackageRegistered(MainForm.MountDir, appxPackage), "Yes", "No")})).ToArray())
        End If
        Return True
    End Function

    Private Sub RemProvAppxPackage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Initialize() Then
            Close()
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Remove provisioned AppX packages"
                        ImageTaskHeader1.ItemText = Text
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        ListView1.Columns(0).Text = "Package name"
                        ListView1.Columns(1).Text = "Application display name"
                        ListView1.Columns(2).Text = "Architecture"
                        ListView1.Columns(3).Text = "Resource ID"
                        ListView1.Columns(4).Text = "Version"
                        ListView1.Columns(5).Text = "Registered to any user?"
                    Case "ESN"
                        Text = "Eliminar paquetes aprovisionados AppX"
                        ImageTaskHeader1.ItemText = Text
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        ListView1.Columns(0).Text = "Nombre de paquete"
                        ListView1.Columns(1).Text = "Nombre de aplicación"
                        ListView1.Columns(2).Text = "Arquitectura"
                        ListView1.Columns(3).Text = "ID de recursos"
                        ListView1.Columns(4).Text = "Versión"
                        ListView1.Columns(5).Text = "¿Registrada a un usuario?"
                    Case "FRA"
                        Text = "Supprimer les paquets AppX provisionnés"
                        ImageTaskHeader1.ItemText = Text
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        ListView1.Columns(0).Text = "Nom du paquet"
                        ListView1.Columns(1).Text = "Nom d'affichage de l'application"
                        ListView1.Columns(2).Text = "Architecture"
                        ListView1.Columns(3).Text = "ID de la ressource"
                        ListView1.Columns(4).Text = "Version"
                        ListView1.Columns(5).Text = "Enregistré au nom d'un utilisateur ?"
                    Case "PTB", "PTG"
                        Text = "Remover pacotes AppX provisionados"
                        ImageTaskHeader1.ItemText = Text
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                        ListView1.Columns(0).Text = "Nome do pacote"
                        ListView1.Columns(1).Text = "Nome de apresentação da aplicação"
                        ListView1.Columns(2).Text = "Arquitetura"
                        ListView1.Columns(3).Text = "ID do recurso"
                        ListView1.Columns(4).Text = "Versão"
                        ListView1.Columns(5).Text = "Registado por algum utilizador?"
                    Case "ITA"
                        Text = "Rimuovi i pacchetti AppX in provisioning"
                        ImageTaskHeader1.ItemText = Text
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                        ListView1.Columns(0).Text = "Nome del pacchetto"
                        ListView1.Columns(1).Text = "Nome del display dell'applicazione"
                        ListView1.Columns(2).Text = "Architettura"
                        ListView1.Columns(3).Text = "ID risorsa"
                        ListView1.Columns(4).Text = "Versione"
                        ListView1.Columns(5).Text = "Registrato a qualche utente?"
                End Select
            Case 1
                Text = "Remove provisioned AppX packages"
                ImageTaskHeader1.ItemText = Text
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                ListView1.Columns(0).Text = "Package name"
                ListView1.Columns(1).Text = "Application display name"
                ListView1.Columns(2).Text = "Architecture"
                ListView1.Columns(3).Text = "Resource ID"
                ListView1.Columns(4).Text = "Version"
                ListView1.Columns(5).Text = "Registered to any user?"
            Case 2
                Text = "Eliminar paquetes aprovisionados AppX"
                ImageTaskHeader1.ItemText = Text
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                ListView1.Columns(0).Text = "Nombre de paquete"
                ListView1.Columns(1).Text = "Nombre de aplicación"
                ListView1.Columns(2).Text = "Arquitectura"
                ListView1.Columns(3).Text = "ID de recursos"
                ListView1.Columns(4).Text = "Versión"
                ListView1.Columns(5).Text = "¿Registrada a un usuario?"
            Case 3
                Text = "Supprimer les paquets AppX provisionnés"
                ImageTaskHeader1.ItemText = Text
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                ListView1.Columns(0).Text = "Nom du paquet"
                ListView1.Columns(1).Text = "Nom d'affichage de l'application"
                ListView1.Columns(2).Text = "Architecture"
                ListView1.Columns(3).Text = "ID de la ressource"
                ListView1.Columns(4).Text = "Version"
                ListView1.Columns(5).Text = "Enregistré au nom d'un utilisateur ?"
            Case 4
                Text = "Remover pacotes AppX provisionados"
                ImageTaskHeader1.ItemText = Text
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
                ListView1.Columns(0).Text = "Nome do pacote"
                ListView1.Columns(1).Text = "Nome de apresentação da aplicação"
                ListView1.Columns(2).Text = "Arquitetura"
                ListView1.Columns(3).Text = "ID do recurso"
                ListView1.Columns(4).Text = "Versão"
                ListView1.Columns(5).Text = "Registado por algum utilizador?"
            Case 5
                Text = "Rimuovi i pacchetti AppX in provisioning"
                ImageTaskHeader1.ItemText = Text
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
                ListView1.Columns(0).Text = "Nome del pacchetto"
                ListView1.Columns(1).Text = "Nome del display dell'applicazione"
                ListView1.Columns(2).Text = "Architettura"
                ListView1.Columns(3).Text = "ID risorsa"
                ListView1.Columns(4).Text = "Versione"
                ListView1.Columns(5).Text = "Registrato a qualche utente?"
        End Select
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.SectionBackgroundColor
        ListView1.ForeColor = ForeColor
        MainForm.ViewPackageDirectoryToolStripMenuItem.Image = GetGlyphResource("openfile")
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(1))

        ColumnHeader1.Width = WindowHelper.ScaleLogical(243)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(202)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(74)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(74)
        ColumnHeader5.Width = WindowHelper.ScaleLogical(80)
        ColumnHeader6.Width = WindowHelper.ScaleLogical(130)
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Private Sub ListView1_MouseClick(sender As Object, e As MouseEventArgs) Handles ListView1.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim item As ListViewItem = ListView1.GetItemAt(e.X, e.Y)
            If item IsNot Nothing Then
                MainForm.AppxPackagePopupCMS.Show(sender, e.Location)
            End If
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count = 1 Then
            MainForm.ResViewTSMI.Visible = True
            DynaLog.LogMessage("Updating context menu items...")
            Dim selectedAppx
            If MainForm.CurrentImage.ImageAppxPackages Is Nothing OrElse MainForm.CurrentImage.ImageAppxPackages_Backup.Count > MainForm.CurrentImage.ImageAppxPackages.Count Then
                selectedAppx = MainForm.CurrentImage.ImageAppxPackages_Backup.ElementAtOrDefault(ListView1.FocusedItem.Index)
            Else
                selectedAppx = MainForm.CurrentImage.ImageAppxPackages.ElementAtOrDefault(ListView1.FocusedItem.Index)
            End If

            If selectedAppx Is Nothing Then
                MainForm.ResViewTSMI.Text = ""
                MainForm.ResViewTSMI.Visible = False
            End If

            Dim friendlyDisplayName As String = ""
            If TypeOf (selectedAppx) Is ImageAppxPackage Then
                friendlyDisplayName = AppxHelper.GetPackageDisplayName(MainForm.MountDir, CType(selectedAppx, ImageAppxPackage).PackageFullName, CType(selectedAppx, ImageAppxPackage).PackageName)
            ElseIf TypeOf (selectedAppx) Is DismAppxPackage Then
                friendlyDisplayName = AppxHelper.GetPackageDisplayName(MainForm.MountDir, CType(selectedAppx, DismAppxPackage).PackageName, CType(selectedAppx, DismAppxPackage).DisplayName)
            End If

            If friendlyDisplayName.StartsWith("ms-resource:", StringComparison.OrdinalIgnoreCase) Then
                If TypeOf (selectedAppx) Is ImageAppxPackage Then
                    friendlyDisplayName = CType(selectedAppx, ImageAppxPackage).PackageName
                ElseIf TypeOf (selectedAppx) Is DismAppxPackage Then
                    friendlyDisplayName = CType(selectedAppx, DismAppxPackage).DisplayName
                End If
            End If

            Try
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                MainForm.ResViewTSMI.Text = "View resources of " & friendlyDisplayName
                            Case "ESN"
                                MainForm.ResViewTSMI.Text = "Ver recursos de " & friendlyDisplayName
                            Case "FRA"
                                MainForm.ResViewTSMI.Text = "Voir les ressources de " & friendlyDisplayName
                            Case "PTB", "PTG"
                                MainForm.ResViewTSMI.Text = "Ver recursos de " & friendlyDisplayName
                            Case "ITA"
                                MainForm.ResViewTSMI.Text = "Visualizza le risorse di " & friendlyDisplayName
                        End Select
                    Case 1
                        MainForm.ResViewTSMI.Text = "View resources of " & friendlyDisplayName
                    Case 2
                        MainForm.ResViewTSMI.Text = "Ver recursos de " & friendlyDisplayName
                    Case 3
                        MainForm.ResViewTSMI.Text = "Voir les ressources de " & friendlyDisplayName
                    Case 4
                        MainForm.ResViewTSMI.Text = "Ver recursos de " & friendlyDisplayName
                    Case 5
                        MainForm.ResViewTSMI.Text = "Visualizza le risorse di " & friendlyDisplayName
                End Select
            Catch ex As Exception
                MainForm.ResViewTSMI.Text = ""
                MainForm.ResViewTSMI.Visible = False
            End Try
        End If
    End Sub
End Class
