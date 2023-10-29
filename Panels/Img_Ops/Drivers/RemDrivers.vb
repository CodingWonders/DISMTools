Imports System.Windows.Forms
Imports Microsoft.VisualBasic.ControlChars
Imports System.IO

Public Class RemDrivers

    Dim drvPkgs(65535) As String
    Dim drvPkgCount As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MainForm.MountDir
        drvPkgCount = ListView1.CheckedItems.Count
        If ListView1.CheckedItems.Count > 0 Then
            Dim drvPkgList As New List(Of String)
            For x = 0 To ListView1.CheckedItems.Count - 1
                drvPkgList.Add(ListView1.CheckedItems(x).SubItems(0).Text)
            Next
            drvPkgs = drvPkgList.ToArray()
            ' Detect if there are boot-critical drivers checked
            For x = 0 To ListView1.CheckedItems.Count - 1
                If ListView1.CheckedItems(x).SubItems(5).Text = "Yes" Or ListView1.CheckedItems(x).SubItems(5).Text = "Sí" Or ListView1.CheckedItems(x).SubItems(5).Text = "Oui" Then
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    If MsgBox("You have selected driver packages that are boot-critical. Proceeding with the removal of such packages may leave the target image unbootable." & CrLf & CrLf & "Do you want to continue?", vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.No Then
                                        Exit Sub
                                    End If
                                Case "ESN"
                                    If MsgBox("Ha seleccionado paquetes de controladores que son críticos para el arranque. Continuar con la eliminación de dichos paquetes podría dejar la imagen de destino sin poder arrancar." & CrLf & CrLf & "¿Desea continuar?", vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.No Then
                                        Exit Sub
                                    End If
                                Case "FRA"
                                    If MsgBox("Vous avez sélectionné des paquets de pilotes critiques pour le démarrage. En procédant à la suppression de ces paquets, vous risquez de rendre l'image cible non amorçable." & CrLf & CrLf & "Voulez-vous continuer ?", vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.No Then
                                        Exit Sub
                                    End If
                            End Select
                        Case 1
                            If MsgBox("You have selected driver packages that are boot-critical. Proceeding with the removal of such packages may leave the target image unbootable." & CrLf & CrLf & "Do you want to continue?", vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.No Then
                                Exit Sub
                            End If
                        Case 2
                            If MsgBox("Ha seleccionado paquetes de controladores que son críticos para el arranque. Continuar con la eliminación de dichos paquetes podría dejar la imagen de destino sin poder arrancar." & CrLf & CrLf & "¿Desea continuar?", vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.No Then
                                Exit Sub
                            End If
                        Case 3
                            If MsgBox("Vous avez sélectionné des paquets de pilotes critiques pour le démarrage. En procédant à la suppression de ces paquets, vous risquez de rendre l'image cible non amorçable." & CrLf & CrLf & "Voulez-vous continuer ?", vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.No Then
                                Exit Sub
                            End If
                    End Select
                    Exit For
                End If
                If ListView1.CheckedItems(x).SubItems(4).Text = "Yes" Or ListView1.CheckedItems(x).SubItems(4).Text = "Sí" Or ListView1.CheckedItems(x).SubItems(4).Text = "Oui" Then
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    If MsgBox("You have selected driver packages that are part of the Windows distribution. Proceeding may leave certain parts of Windows that depend on these drivers inaccessible." & CrLf & CrLf & "Do you want to continue?", vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.No Then
                                        Exit Sub
                                    End If
                                Case "ESN"
                                    If MsgBox("Ha seleccionado paquetes de controladores que son parte de la distribución de Windows. Continuar podría dejar algunas partes de Windows que dependan de estos contoladores inaccesibles." & CrLf & CrLf & "¿Desea continuar?", vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.No Then
                                        Exit Sub
                                    End If
                                Case "FRA"
                                    If MsgBox("Vous avez sélectionné des paquets de pilotes qui font partie de la distribution de Windows. La poursuite de l'opération peut rendre inaccessibles certaines parties de Windows qui dépendent de ces pilotes." & CrLf & CrLf & "Voulez-vous continuer ?", vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.No Then
                                        Exit Sub
                                    End If
                            End Select
                        Case 1
                            If MsgBox("You have selected driver packages that are part of the Windows distribution. Proceeding may leave certain parts of Windows that depend on these drivers inaccessible." & CrLf & CrLf & "Do you want to continue?", vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.No Then
                                Exit Sub
                            End If
                        Case 2
                            If MsgBox("Ha seleccionado paquetes de controladores que son parte de la distribución de Windows. Continuar podría dejar algunas partes de Windows que dependan de estos contoladores inaccesibles." & CrLf & CrLf & "¿Desea continuar?", vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.No Then
                                Exit Sub
                            End If
                        Case 3
                            If MsgBox("Vous avez sélectionné des paquets de pilotes qui font partie de la distribution de Windows. La poursuite de l'opération peut rendre inaccessibles certaines parties de Windows qui dépendent de ces pilotes." & CrLf & CrLf & "Voulez-vous continuer ?", vbYesNo + vbExclamation, Label1.Text) = MsgBoxResult.No Then
                                Exit Sub
                            End If
                    End Select
                    Exit For
                End If
            Next
            For x = 0 To drvPkgs.Length - 1
                ProgressPanel.drvRemovalPkgs(x) = drvPkgs(x)
            Next
            ProgressPanel.drvRemovalLastPkg = ListView1.CheckedItems(drvPkgCount - 1).SubItems(0).Text
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MsgBox("Please specify the driver packages you wish to remove and try again", vbOKOnly + vbCritical, Label1.Text)
                        Case "ESN"
                            MsgBox("Especifique los paquetes de controladores que desea eliminar e inténtelo de nuevo", vbOKOnly + vbCritical, Label1.Text)
                        Case "FRA"
                            MsgBox("Veuillez spécifier les paquets de pilotes que vous souhaitez supprimer et réessayez.", vbOKOnly + vbCritical, Label1.Text)
                    End Select
                Case 1
                    MsgBox("Please specify the driver packages you wish to remove and try again", vbOKOnly + vbCritical, Label1.Text)
                Case 2
                    MsgBox("Especifique los paquetes de controladores que desea eliminar e inténtelo de nuevo", vbOKOnly + vbCritical, Label1.Text)
                Case 3
                    MsgBox("Veuillez spécifier les paquets de pilotes que vous souhaitez supprimer et réessayez.", vbOKOnly + vbCritical, Label1.Text)
            End Select
            Exit Sub
        End If
        ProgressPanel.drvRemovalCount = drvPkgCount
        ProgressPanel.OperationNum = 76
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub RemDrivers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Remove drivers"
                        Label1.Text = Text
                        Label2.Text = "Specify the driver packages you wish to remove and click OK:"
                        CheckBox1.Text = "Hide boot-critical drivers"
                        CheckBox2.Text = "Hide drivers part of the Windows distribution"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Cancel"
                        ListView1.Columns(0).Text = "Published name"
                        ListView1.Columns(1).Text = "Original file name"
                        ListView1.Columns(2).Text = "Provider name"
                        ListView1.Columns(3).Text = "Class name"
                        ListView1.Columns(4).Text = "Part of the Windows distribution?"
                        ListView1.Columns(5).Text = "Is boot-critical?"
                        ListView1.Columns(6).Text = "Version"
                        ListView1.Columns(7).Text = "Date"
                    Case "ESN"
                        Text = "Eliminar controladores"
                        Label1.Text = Text
                        Label2.Text = "Especifique los paquetes de controladores que desea eliminar y haga clic en Aceptar:"
                        CheckBox1.Text = "Ocultar controladores críticos para el arranque"
                        CheckBox2.Text = "Ocultar controladores que son parte de la distribución de Windows"
                        OK_Button.Text = "Aceptar"
                        Cancel_Button.Text = "Cancelar"
                        ListView1.Columns(0).Text = "Nombre publicado"
                        ListView1.Columns(1).Text = "Nombre original del archivo"
                        ListView1.Columns(2).Text = "Nombre del proveedor"
                        ListView1.Columns(3).Text = "Nombre de clase"
                        ListView1.Columns(4).Text = "¿Parte de la distribución de Windows?"
                        ListView1.Columns(5).Text = "¿Es crítico para el arranque?"
                        ListView1.Columns(6).Text = "Versión"
                        ListView1.Columns(7).Text = "Fecha"
                    Case "FRA"
                        Text = "Supprimer les pilotes"
                        Label1.Text = Text
                        Label2.Text = "Indiquez les paquets de pilotes que vous souhaitez supprimer et cliquez sur OK :"
                        CheckBox1.Text = "Cacher les pilotes critiques pour le démarrage"
                        CheckBox2.Text = "Cacher les pilotes qui font partie de la distribution de Windows"
                        OK_Button.Text = "OK"
                        Cancel_Button.Text = "Annuler"
                        ListView1.Columns(0).Text = "Nom publié"
                        ListView1.Columns(1).Text = "Nom du fichier original"
                        ListView1.Columns(2).Text = "Nom du prestataire"
                        ListView1.Columns(3).Text = "Nom de la classe"
                        ListView1.Columns(4).Text = "Fait-il partie de la distribution Windows ?"
                        ListView1.Columns(5).Text = "Est-il critique pour le démarrage ?"
                        ListView1.Columns(6).Text = "Version"
                        ListView1.Columns(7).Text = "Date"
                End Select
            Case 1
                Text = "Remove drivers"
                Label1.Text = Text
                Label2.Text = "Specify the driver packages you wish to remove and click OK:"
                CheckBox1.Text = "Hide boot-critical drivers"
                CheckBox2.Text = "Hide drivers part of the Windows distribution"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Cancel"
                ListView1.Columns(0).Text = "Published name"
                ListView1.Columns(1).Text = "Original file name"
                ListView1.Columns(2).Text = "Provider name"
                ListView1.Columns(3).Text = "Class name"
                ListView1.Columns(4).Text = "Part of the Windows distribution?"
                ListView1.Columns(5).Text = "Is boot-critical?"
                ListView1.Columns(6).Text = "Version"
                ListView1.Columns(7).Text = "Date"
            Case 2
                Text = "Eliminar controladores"
                Label1.Text = Text
                Label2.Text = "Especifique los paquetes de controladores que desea eliminar y haga clic en Aceptar:"
                CheckBox1.Text = "Ocultar controladores críticos para el arranque"
                CheckBox2.Text = "Ocultar controladores que son parte de la distribución de Windows"
                OK_Button.Text = "Aceptar"
                Cancel_Button.Text = "Cancelar"
                ListView1.Columns(0).Text = "Nombre publicado"
                ListView1.Columns(1).Text = "Nombre original del archivo"
                ListView1.Columns(2).Text = "Nombre del proveedor"
                ListView1.Columns(3).Text = "Nombre de clase"
                ListView1.Columns(4).Text = "¿Parte de la distribución de Windows?"
                ListView1.Columns(5).Text = "¿Es crítico para el arranque?"
                ListView1.Columns(6).Text = "Versión"
                ListView1.Columns(7).Text = "Fecha"
            Case 3
                Text = "Supprimer les pilotes"
                Label1.Text = Text
                Label2.Text = "Indiquez les paquets de pilotes que vous souhaitez supprimer et cliquez sur OK :"
                CheckBox1.Text = "Cacher les pilotes critiques pour le démarrage"
                CheckBox2.Text = "Cacher les pilotes qui font partie de la distribution de Windows"
                OK_Button.Text = "OK"
                Cancel_Button.Text = "Annuler"
                ListView1.Columns(0).Text = "Nom publié"
                ListView1.Columns(1).Text = "Nom du fichier original"
                ListView1.Columns(2).Text = "Nom du prestataire"
                ListView1.Columns(3).Text = "Nom de la classe"
                ListView1.Columns(4).Text = "Fait-il partie de la distribution Windows ?"
                ListView1.Columns(5).Text = "Est-il critique pour le démarrage ?"
                ListView1.Columns(6).Text = "Version"
                ListView1.Columns(7).Text = "Date"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListView1.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged, CheckBox2.CheckedChanged
        ListView1.Items.Clear()
        ProgressPanel.OperationNum = 994
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        PleaseWaitDialog.Label2.Text = "Getting installed driver packages..."
                    Case "ESN"
                        PleaseWaitDialog.Label2.Text = "Obteniendo paquetes de controladores instalados..."
                    Case "FRA"
                        PleaseWaitDialog.Label2.Text = "Obtention des paquets de pilotes installés en cours..."
                End Select
            Case 1
                PleaseWaitDialog.Label2.Text = "Getting installed driver packages..."
            Case 2
                PleaseWaitDialog.Label2.Text = "Obteniendo paquetes de controladores instalados..."
            Case 3
                PleaseWaitDialog.Label2.Text = "Obtention des paquets de pilotes installés en cours..."
        End Select
        If Not MainForm.areBackgroundProcessesDone Then
            PleaseWaitDialog.ShowDialog(Me)
            Exit Sub
        End If
        Try
            For x = 0 To Array.LastIndexOf(MainForm.imgDrvPublishedNames, MainForm.imgDrvPublishedNames.Last)
                If CheckBox1.Checked Then
                    If MainForm.imgDrvBootCriticalStatus(x) Then Continue For
                End If
                If CheckBox2.Checked Then
                    If CBool(MainForm.imgDrvInbox(x)) Then Continue For
                End If
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                ListView1.Items.Add(New ListViewItem(New String() {MainForm.imgDrvPublishedNames(x), Path.GetFileName(MainForm.imgDrvOGFileNames(x)), MainForm.imgDrvProviderNames(x), MainForm.imgDrvClassNames(x), If(CBool(MainForm.imgDrvInbox(x)), "Yes", "No"), If(MainForm.imgDrvBootCriticalStatus(x), "Yes", "No"), MainForm.imgDrvVersions(x), MainForm.imgDrvDates(x)}))
                            Case "ESN"
                                ListView1.Items.Add(New ListViewItem(New String() {MainForm.imgDrvPublishedNames(x), Path.GetFileName(MainForm.imgDrvOGFileNames(x)), MainForm.imgDrvProviderNames(x), MainForm.imgDrvClassNames(x), If(CBool(MainForm.imgDrvInbox(x)), "Sí", "No"), If(MainForm.imgDrvBootCriticalStatus(x), "Sí", "No"), MainForm.imgDrvVersions(x), MainForm.imgDrvDates(x)}))
                            Case "FRA"
                                ListView1.Items.Add(New ListViewItem(New String() {MainForm.imgDrvPublishedNames(x), Path.GetFileName(MainForm.imgDrvOGFileNames(x)), MainForm.imgDrvProviderNames(x), MainForm.imgDrvClassNames(x), If(CBool(MainForm.imgDrvInbox(x)), "Oui", "Non"), If(MainForm.imgDrvBootCriticalStatus(x), "Oui", "Non"), MainForm.imgDrvVersions(x), MainForm.imgDrvDates(x)}))
                        End Select
                    Case 1
                        ListView1.Items.Add(New ListViewItem(New String() {MainForm.imgDrvPublishedNames(x), Path.GetFileName(MainForm.imgDrvOGFileNames(x)), MainForm.imgDrvProviderNames(x), MainForm.imgDrvClassNames(x), If(CBool(MainForm.imgDrvInbox(x)), "Yes", "No"), If(MainForm.imgDrvBootCriticalStatus(x), "Yes", "No"), MainForm.imgDrvVersions(x), MainForm.imgDrvDates(x)}))
                    Case 2
                        ListView1.Items.Add(New ListViewItem(New String() {MainForm.imgDrvPublishedNames(x), Path.GetFileName(MainForm.imgDrvOGFileNames(x)), MainForm.imgDrvProviderNames(x), MainForm.imgDrvClassNames(x), If(CBool(MainForm.imgDrvInbox(x)), "Sí", "No"), If(MainForm.imgDrvBootCriticalStatus(x), "Sí", "No"), MainForm.imgDrvVersions(x), MainForm.imgDrvDates(x)}))
                    Case 3
                        ListView1.Items.Add(New ListViewItem(New String() {MainForm.imgDrvPublishedNames(x), Path.GetFileName(MainForm.imgDrvOGFileNames(x)), MainForm.imgDrvProviderNames(x), MainForm.imgDrvClassNames(x), If(CBool(MainForm.imgDrvInbox(x)), "Oui", "Non"), If(MainForm.imgDrvBootCriticalStatus(x), "Oui", "Non"), MainForm.imgDrvVersions(x), MainForm.imgDrvDates(x)}))
                End Select
            Next
        Catch ex As Exception
            Exit Try
        End Try
    End Sub
End Class
