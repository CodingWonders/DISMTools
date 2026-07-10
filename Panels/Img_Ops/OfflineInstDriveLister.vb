Imports System.Windows.Forms
Imports System.IO
Imports DISMTools.Utilities

Public Class OfflineInstDriveLister

    Dim DIList As New List(Of DriveInfo)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            MainForm.InBitLockerMode = False
            DynaLog.LogMessage("Selected drive: " & ListView1.FocusedItem.SubItems(0).Text)

            Dim SelectedDrive As DriveInfo = DIList.ElementAtOrDefault(ListView1.FocusedItem.Index)
            If SelectedDrive Is Nothing Then Exit Sub

            ' If it is encrypted with BitLocker then we ask for the key
            If GetDriveFSType(SelectedDrive) = "BITLOCKER" Then
                UnlockVolumeDialog.DriveLetter = SelectedDrive.Name
                If UnlockVolumeDialog.ShowDialog(Me) <> Windows.Forms.DialogResult.OK Then Exit Sub

                ' We were able to unlock it; now to see if there's Windows in there.
                If Not File.Exists(String.Format("{0}Windows\system32\ntoskrnl.exe", SelectedDrive.Name)) Then
                    DynaLog.LogMessage("The selected drive does not contain ntoskrnl. There is either an utterly broken Windows installation or no installation at all.")
                    LockVolumeDialog.DriveLetter = SelectedDrive.Name
                    LockVolumeDialog.ShowDialog(Me)
                    Exit Sub
                Else
                    DynaLog.LogMessage("The selected drive contains ntoskrnl. Checking version...")
                    ' Don't support Windows Vista (incl. betas) or anything older than Vista
                    Dim sysVer As FileVersionInfo = FileVersionInfo.GetVersionInfo(String.Format("{0}Windows\system32\ntoskrnl.exe", SelectedDrive.Name))
                    If sysVer.ProductMajorPart < 6 Or (sysVer.ProductMajorPart = 6 And sysVer.ProductMinorPart = 0) Then
                        DynaLog.LogMessage("The specified drive contains Windows Vista or an earlier version of Windows.")
                        LockVolumeDialog.DriveLetter = SelectedDrive.Name
                        LockVolumeDialog.ShowDialog(Me)
                        Exit Sub
                    End If
                End If

                MainForm.InBitLockerMode = True
            End If
            MainForm.drivePath = SelectedDrive.Name
        Catch ex As Exception

        End Try
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub OfflineInstDriveLister_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Choose a disk"
                        Label1.Text = "To begin performing offline installation management, please choose a disk shown in the list below. This list will be updated automatically every minute, or when you click the Refresh button."
                        ListView1.Columns(0).Text = "Drive letter"
                        ListView1.Columns(1).Text = "Drive label"
                        ListView1.Columns(2).Text = "Drive type"
                        ListView1.Columns(3).Text = "Total size"
                        ListView1.Columns(4).Text = "Available free space"
                        ListView1.Columns(5).Text = "Drive format"
                        ListView1.Columns(6).Text = "Contains Windows?"
                        ListView1.Columns(7).Text = "Windows version"
                        Button1.Text = "Refresh"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                    Case "ESN"
                        Text = "Elija un disco"
                        Label1.Text = "Para comenzar a realizar el mantenimiento de instalaciones fuera de línea, escoja un disco mostrado en la lista de abajo. Esta lista se actualizará automáticamente cada minuto, o cuando haga clic en el botón Actualizar."
                        ListView1.Columns(0).Text = "Letra de disco"
                        ListView1.Columns(1).Text = "Etiqueta de disco"
                        ListView1.Columns(2).Text = "Tipo de disco"
                        ListView1.Columns(3).Text = "Tamaño total"
                        ListView1.Columns(4).Text = "Espacio libre"
                        ListView1.Columns(5).Text = "Formato del disco"
                        ListView1.Columns(6).Text = "¿Contiene Windows?"
                        ListView1.Columns(7).Text = "Versión de Windows"
                        Button1.Text = "Actualizar"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                    Case "FRA"
                        Text = "Choisir un disque"
                        Label1.Text = "Pour commencer la gestion de l'installation hors ligne, veuillez choisir un disque dans la liste ci-dessous. Cette liste sera mise à jour automatiquement toutes les minutes, ou lorsque vous cliquez sur le bouton Actualiser."
                        ListView1.Columns(0).Text = "Lettre de disque"
                        ListView1.Columns(1).Text = "Étiquette de disque"
                        ListView1.Columns(2).Text = "Type de disque"
                        ListView1.Columns(3).Text = "Taille totale"
                        ListView1.Columns(4).Text = "Espace libre disponible"
                        ListView1.Columns(5).Text = "Format de disque"
                        ListView1.Columns(6).Text = "Contient Windows ?"
                        ListView1.Columns(7).Text = "Version Windows"
                        Button1.Text = "Actualiser"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                    Case "PTB", "PTG"
                        Text = "Escolha um disco"
                        Label1.Text = "Para começar a efetuar a gestão da instalação offline, escolha um disco apresentado na lista abaixo. Esta lista será actualizada automaticamente a cada minuto, ou quando clicar no botão Atualizar."
                        ListView1.Columns(0).Text = "Letra da unidade"
                        ListView1.Columns(1).Text = "Etiqueta da unidade"
                        ListView1.Columns(2).Text = "Tipo de unidade"
                        ListView1.Columns(3).Text = "Tamanho total"
                        ListView1.Columns(4).Text = "Espaço livre disponível"
                        ListView1.Columns(5).Text = "Formato da unidade"
                        ListView1.Columns(6).Text = "Contém Windows?"
                        ListView1.Columns(7).Text = "Versão do Windows"
                        Button1.Text = "Atualizar"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancelar"
                    Case "ITA"
                        Text = "Scegliere un disco"
                        Label1.Text = "Per iniziare la gestione dell'installazione offline, scegliere un disco nell'elenco sottostante. Questo elenco verrà aggiornato automaticamente ogni minuto o quando si fa clic sul pulsante Aggiorna."
                        ListView1.Columns(0).Text = "Lettera unità"
                        ListView1.Columns(1).Text = "Etichetta unità"
                        ListView1.Columns(2).Text = "Tipo di unità"
                        ListView1.Columns(3).Text = "Dimensione totale"
                        ListView1.Columns(4).Text = "Spazio libero disponibile"
                        ListView1.Columns(5).Text = "Formato unità"
                        ListView1.Columns(6).Text = "Contiene Windows?"
                        ListView1.Columns(7).Text = "Versione di Windows"
                        Button1.Text = "Aggiorna"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annullare"
                End Select
            Case 1
                Text = "Choose a disk"
                Label1.Text = "To begin performing offline installation management, please choose a disk shown in the list below. This list will be updated automatically every minute, or when you click the Refresh button."
                ListView1.Columns(0).Text = "Drive letter"
                ListView1.Columns(1).Text = "Drive label"
                ListView1.Columns(2).Text = "Drive type"
                ListView1.Columns(3).Text = "Total size"
                ListView1.Columns(4).Text = "Available free space"
                ListView1.Columns(5).Text = "Drive format"
                ListView1.Columns(6).Text = "Contains Windows?"
                ListView1.Columns(7).Text = "Windows version"
                Button1.Text = "Refresh"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
            Case 2
                Text = "Elija un disco"
                Label1.Text = "Para comenzar a realizar el mantenimiento de instalaciones fuera de línea, escoja un disco mostrado en la lista de abajo. Esta lista se actualizará automáticamente cada minuto, o cuando haga clic en el botón Actualizar."
                ListView1.Columns(0).Text = "Letra de disco"
                ListView1.Columns(1).Text = "Etiqueta de disco"
                ListView1.Columns(2).Text = "Tipo de disco"
                ListView1.Columns(3).Text = "Tamaño total"
                ListView1.Columns(4).Text = "Espacio libre"
                ListView1.Columns(5).Text = "Formato del disco"
                ListView1.Columns(6).Text = "¿Contiene Windows?"
                ListView1.Columns(7).Text = "Versión de Windows"
                Button1.Text = "Actualizar"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
            Case 3
                Text = "Choisir un disque"
                Label1.Text = "Pour commencer la gestion de l'installation hors ligne, veuillez choisir un disque dans la liste ci-dessous. Cette liste sera mise à jour automatiquement toutes les minutes, ou lorsque vous cliquez sur le bouton Actualiser."
                ListView1.Columns(0).Text = "Lettre de disque"
                ListView1.Columns(1).Text = "Étiquette de disque"
                ListView1.Columns(2).Text = "Type de disque"
                ListView1.Columns(3).Text = "Taille totale"
                ListView1.Columns(4).Text = "Espace libre disponible"
                ListView1.Columns(5).Text = "Format de disque"
                ListView1.Columns(6).Text = "Contient Windows ?"
                ListView1.Columns(7).Text = "Version Windows"
                Button1.Text = "Actualiser"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
            Case 4
                Text = "Escolha um disco"
                Label1.Text = "Para começar a efetuar a gestão da instalação offline, escolha um disco apresentado na lista abaixo. Esta lista será actualizada automaticamente a cada minuto, ou quando clicar no botão Atualizar."
                ListView1.Columns(0).Text = "Letra da unidade"
                ListView1.Columns(1).Text = "Etiqueta da unidade"
                ListView1.Columns(2).Text = "Tipo de unidade"
                ListView1.Columns(3).Text = "Tamanho total"
                ListView1.Columns(4).Text = "Espaço livre disponível"
                ListView1.Columns(5).Text = "Formato da unidade"
                ListView1.Columns(6).Text = "Contém Windows?"
                ListView1.Columns(7).Text = "Versão do Windows"
                Button1.Text = "Atualizar"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancelar"
            Case 5
                Text = "Scegliere un disco"
                Label1.Text = "Per iniziare la gestione dell'installazione offline, scegliere un disco nell'elenco sottostante. Questo elenco verrà aggiornato automaticamente ogni minuto o quando si fa clic sul pulsante Aggiorna."
                ListView1.Columns(0).Text = "Lettera unità"
                ListView1.Columns(1).Text = "Etichetta unità"
                ListView1.Columns(2).Text = "Tipo di unità"
                ListView1.Columns(3).Text = "Dimensione totale"
                ListView1.Columns(4).Text = "Spazio libero disponibile"
                ListView1.Columns(5).Text = "Formato unità"
                ListView1.Columns(6).Text = "Contiene Windows?"
                ListView1.Columns(7).Text = "Versione di Windows"
                Button1.Text = "Aggiorna"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annullare"
        End Select
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        ListDisks()
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        Timer1.Enabled = True

        ColumnHeader1.Width = WindowHelper.ScaleLogical(68)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(128)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(70)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(94)
        ColumnHeader5.Width = WindowHelper.ScaleLogical(110)
        ColumnHeader6.Width = WindowHelper.ScaleLogical(77)
        ColumnHeader7.Width = WindowHelper.ScaleLogical(110)
        ColumnHeader8.Width = WindowHelper.ScaleLogical(104)
    End Sub

    Private Sub ListDisks()
        DynaLog.LogMessage("Getting disks...")
        ListView1.Items.Clear()
        DIList.Clear()
        DIList = DriveInfo.GetDrives().Where(Function(disk) Not {DriveType.CDRom, DriveType.Unknown}.Contains(disk.DriveType)).ToList()
        ListView1.Items.AddRange(DIList.Select(Function(DI) New ListViewItem(New String() {DI.Name,
                                                                                           GetDriveVolumeLabel(DI),
                                                                                           Casters.CastDriveType(DI.DriveType, True),
                                                                                           Converters.BytesToReadableSize(GetDriveTotalSpace(DI)),
                                                                                           Converters.BytesToReadableSize(GetDriveFreeSpace(DI)),
                                                                                           GetDriveFSType(DI),
                                                                                           If(GetDriveFSType(DI) <> "BITLOCKER" AndAlso File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), "Yes", "No"),
                                                                                           If(GetDriveFSType(DI) <> "BITLOCKER" AndAlso File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"),
                                                                                              FileVersionInfo.GetVersionInfo(DI.Name & "\Windows\system32\ntoskrnl.exe").ProductVersion, "")})).ToArray())
    End Sub

    Private Function GetDriveVolumeLabel(drInfo As DriveInfo) As String
        Try
            Return drInfo.VolumeLabel
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Function GetDriveFSType(drInfo As DriveInfo) As String
        Try
            Return drInfo.DriveFormat
        Catch bitLockerEx As IOException When bitLockerEx.Message.IndexOf("bitlocker", StringComparison.OrdinalIgnoreCase) > -1
            Return "BITLOCKER"
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Private Function GetDriveTotalSpace(drInfo As DriveInfo) As Long
        Try
            Return drInfo.TotalSize
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Function GetDriveFreeSpace(drInfo As DriveInfo) As Long
        Try
            Return drInfo.AvailableFreeSpace
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListDisks()
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        UnlockNoticeLabel.Visible = False

        If ListView1.SelectedItems.Count > 0 Then
            OK_Button.Enabled = True
            Dim SelectedDisk As DriveInfo = DIList.ElementAtOrDefault(ListView1.FocusedItem.Index)
            If SelectedDisk Is Nothing Then
                OK_Button.Enabled = False
                Exit Sub
            End If

            Try
                If SelectedDisk.DriveFormat <> "NTFS" Then
                    DynaLog.LogMessage("The selected drive is not formatted with NTFS.")
                    OK_Button.Enabled = False
                End If
                If Not File.Exists(ListView1.FocusedItem.SubItems(0).Text & "\Windows\system32\ntoskrnl.exe") Then
                    DynaLog.LogMessage("The selected drive does not contain ntoskrnl. There is either an utterly broken Windows installation or no installation at all.")
                    OK_Button.Enabled = False
                Else
                    DynaLog.LogMessage("The selected drive contains ntoskrnl. Checking version...")
                    ' Don't support Windows Vista (incl. betas) or anything older than Vista
                    Dim sysVer As FileVersionInfo = FileVersionInfo.GetVersionInfo(String.Format("{0}Windows\system32\ntoskrnl.exe", SelectedDisk.Name))
                    If sysVer.ProductMajorPart < 6 Or (sysVer.ProductMajorPart = 6 And sysVer.ProductMinorPart = 0) Then
                        DynaLog.LogMessage("The specified drive contains Windows Vista or an earlier version of Windows.")
                        OK_Button.Enabled = False
                    End If
                End If
            Catch bitlockerEx As IOException When bitlockerEx.Message.IndexOf("bitlocker", StringComparison.OrdinalIgnoreCase) > -1
                ' leave it enabled for now; we'll ask the user to unlock it when we click OK
                UnlockNoticeLabel.Visible = True
            Catch ex As Exception
                OK_Button.Enabled = False
            End Try
        Else
            OK_Button.Enabled = False
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Button1.PerformClick()
    End Sub

    Private Sub OfflineInstDriveLister_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Timer1.Enabled = False
    End Sub
End Class
