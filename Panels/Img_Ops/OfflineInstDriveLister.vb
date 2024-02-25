Imports System.Windows.Forms
Imports System.IO
Imports DISMTools.Utilities

Public Class OfflineInstDriveLister

    Dim DIList As New List(Of DriveInfo)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        MainForm.drivePath = ListView1.FocusedItem.SubItems(0).Text
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
                        Label1.Text = "To begin performing offline installation management, please choose a disk shown in the list below. If additional disks that contain Windows installations have been added or removed, simply click the Refresh button."
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
                        Label1.Text = "Para comenzar a realizar el mantenimiento de instalaciones fuera de línea, escoja un disco mostrado en la lista de abajo. Si se han añadido o removido discos adicionales que contienen instalaciones de Windows, simplemente haga clic en el botón Actualizar."
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
                        Label1.Text = "Pour commencer à gérer l'installation hors ligne, choisissez un disque dans la liste ci-dessous. Si des disques contenant des installations Windows ont été ajoutés ou supprimés, il suffit de cliquer sur le bouton Actualiser."
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
                        Label1.Text = "Para começar a efetuar a gestão de instalações offline, escolha um disco apresentado na lista abaixo. Se tiverem sido adicionados ou removidos discos adicionais que contenham instalações do Windows, basta clicar no botão Atualizar."
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
                End Select
            Case 1
                Text = "Choose a disk"
                Label1.Text = "To begin performing offline installation management, please choose a disk shown in the list below. If additional disks that contain Windows installations have been added or removed, simply click the Refresh button."
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
                Label1.Text = "Para comenzar a realizar el mantenimiento de instalaciones fuera de línea, escoja un disco mostrado en la lista de abajo. Si se han añadido o removido discos adicionales que contienen instalaciones de Windows, simplemente haga clic en el botón Actualizar."
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
                Label1.Text = "Pour commencer à gérer l'installation hors ligne, choisissez un disque dans la liste ci-dessous. Si des disques contenant des installations Windows ont été ajoutés ou supprimés, il suffit de cliquer sur le bouton Actualiser."
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
                Label1.Text = "Para começar a efetuar a gestão de instalações offline, escolha um disco apresentado na lista abaixo. Se tiverem sido adicionados ou removidos discos adicionais que contenham instalações do Windows, basta clicar no botão Atualizar."
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
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
        End If
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        ListView1.Items.Clear()
        DIList.Clear()
        DIList = DriveInfo.GetDrives().ToList()
        For Each DI As DriveInfo In DIList
            If DI.IsReady Then
                ListView1.Items.Add(New ListViewItem(New String() {DI.Name, DI.VolumeLabel, Casters.CastDriveType(DI.DriveType, True), Converters.BytesToReadableSize(DI.TotalSize), Converters.BytesToReadableSize(DI.AvailableFreeSpace), DI.DriveFormat, If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), "Yes", "No"), If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), FileVersionInfo.GetVersionInfo(DI.Name & "\Windows\system32\ntoskrnl.exe").ProductVersion, "")}))
            End If
        Next
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListView1.Items.Clear()
        DIList.Clear()
        DIList = DriveInfo.GetDrives().ToList()
        For Each DI As DriveInfo In DIList
            If DI.IsReady Then
                ListView1.Items.Add(New ListViewItem(New String() {DI.Name, DI.VolumeLabel, Casters.CastDriveType(DI.DriveType, True), Converters.BytesToReadableSize(DI.TotalSize), Converters.BytesToReadableSize(DI.AvailableFreeSpace), DI.DriveFormat, If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), "Yes", "No"), If(File.Exists(DI.Name & "\Windows\system32\ntoskrnl.exe"), FileVersionInfo.GetVersionInfo(DI.Name & "\Windows\system32\ntoskrnl.exe").ProductVersion, "")}))
            End If
        Next
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            OK_Button.Enabled = True
            For x = 0 To DIList.Count - 1
                If DIList(x).Name = ListView1.FocusedItem.SubItems(0).Text Then
                    If DIList(x).DriveFormat <> "NTFS" Then
                        OK_Button.Enabled = False
                    End If
                    If Casters.CastDriveType(DIList(x).DriveType) <> "Fixed" Then
                        OK_Button.Enabled = False
                    End If
                    If Not File.Exists(ListView1.FocusedItem.SubItems(0).Text & "\Windows\system32\ntoskrnl.exe") Then
                        OK_Button.Enabled = False
                    Else
                        ' Don't support Windows Vista (incl. betas) or anything older than Vista
                        Dim sysVer As FileVersionInfo = FileVersionInfo.GetVersionInfo(ListView1.FocusedItem.SubItems(0).Text & "\Windows\system32\ntoskrnl.exe")
                        If sysVer.ProductMajorPart < 6 Or _
                           (sysVer.ProductMajorPart = 6 And sysVer.ProductMinorPart = 0) Then
                            OK_Button.Enabled = False
                        End If
                    End If
                End If
            Next
        Else
            OK_Button.Enabled = False
        End If
    End Sub
End Class
