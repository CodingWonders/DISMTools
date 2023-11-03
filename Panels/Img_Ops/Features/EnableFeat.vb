Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class EnableFeat

    Public featEnablementCount As Integer
    Public featEnablementNames(65535) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not ProgressPanel.IsDisposed Then ProgressPanel.Dispose()
        ProgressPanel.MountDir = MainForm.MountDir
        featEnablementCount = ListView1.CheckedItems.Count
        ProgressPanel.featEnablementCount = featEnablementCount
        If ListView1.CheckedItems.Count <= 0 Then
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            MessageBox.Show(MainForm, "Please select features to enable, and try again.", "No features selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case "ESN"
                            MessageBox.Show(MainForm, "Seleccione las características a habilitar, e inténtelo de nuevo.", "No se ha seleccionado ninguna característica", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Case "FRA"
                            MessageBox.Show(MainForm, "Veuillez sélectionner les caractéristiques à activer et réessayer.", "Aucune caractéristique sélectionnée", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Select
                Case 1
                    MessageBox.Show(MainForm, "Please select features to enable, and try again.", "No features selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Case 2
                    MessageBox.Show(MainForm, "Seleccione las características a habilitar, e inténtelo de nuevo.", "No se ha seleccionado ninguna característica", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Case 3
                    MessageBox.Show(MainForm, "Veuillez sélectionner les caractéristiques à activer et réessayer.", "Aucune caractéristique sélectionnée", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select
            Exit Sub
        Else
            Try
                For x = 0 To featEnablementCount - 1
                    featEnablementNames(x) = ListView1.CheckedItems(x).ToString()
                Next
                For x = 0 To featEnablementNames.Length
                    ProgressPanel.featEnablementNames(x) = featEnablementNames(x)
                Next
            Catch ex As Exception

            End Try
            For x = 0 To featEnablementCount - 1
                If MainForm.OnlineManagement And CheckBox4.Checked Then Exit For
                If ListView1.CheckedItems(x).SubItems(1).Text = "Removed" Or ListView1.CheckedItems(x).SubItems(1).Text = "Eliminado" Or ListView1.CheckedItems(x).SubItems(1).Text = "Supprimée" Then
                    If RichTextBox1.Text = "" Or Not Directory.Exists(RichTextBox1.Text) Then
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        If MsgBox("Some features in this image require specifying a source for them to be enabled. The specified source is not valid for this operation." & CrLf & CrLf & If(RichTextBox1.Text = "", "Please specify a valid source and try again.", "Please make sure the source exists in the file system and try again."), vbOKOnly + vbCritical, "Enable features") = MsgBoxResult.Ok Then
                                            CheckBox2.Checked = True
                                            Button2.PerformClick()
                                        End If
                                    Case "ESN"
                                        If MsgBox("Algunas características en esta imagen requieren especificar un origen para ser habilitadas. El origen especificado no es válido para esta operación" & CrLf & CrLf & If(RichTextBox1.Text = "", "Especifique un origen válido e inténtelo de nuevo.", "Asegúrese de que el origen exista en el sistema de archivos e inténtelo de nuevo."), vbOKOnly + vbCritical, "Habilitar características") = MsgBoxResult.Ok Then
                                            CheckBox2.Checked = True
                                            Button2.PerformClick()
                                        End If
                                    Case "FRA"
                                        If MsgBox("Certaines caractéristiques de cette image nécessitent la spécification d'une source pour être activées. La source spécifiée n'est pas valide pour cette opération." & CrLf & CrLf & If(RichTextBox1.Text = "", "Veuillez indiquer une source valide et réessayer.", "Assurez-vous que la source existe dans le système de fichiers et réessayez."), vbOKOnly + vbCritical, "Activer les caractéristiques") = MsgBoxResult.Ok Then
                                            CheckBox2.Checked = True
                                            Button2.PerformClick()
                                        End If
                                End Select
                            Case 1
                                If MsgBox("Some features in this image require specifying a source for them to be enabled. The specified source is not valid for this operation." & CrLf & CrLf & If(RichTextBox1.Text = "", "Please specify a valid source and try again.", "Please make sure the source exists in the file system and try again."), vbOKOnly + vbCritical, "Enable features") = MsgBoxResult.Ok Then
                                    CheckBox2.Checked = True
                                    Button2.PerformClick()
                                End If
                            Case 2
                                If MsgBox("Algunas características en esta imagen requieren especificar un origen para ser habilitadas. El origen especificado no es válido para esta operación" & CrLf & CrLf & If(RichTextBox1.Text = "", "Especifique un origen válido e inténtelo de nuevo.", "Asegúrese de que el origen exista en el sistema de archivos e inténtelo de nuevo."), vbOKOnly + vbCritical, "Habilitar características") = MsgBoxResult.Ok Then
                                    CheckBox2.Checked = True
                                    Button2.PerformClick()
                                End If
                            Case 3
                                If MsgBox("Certaines caractéristiques de cette image nécessitent la spécification d'une source pour être activées. La source spécifiée n'est pas valide pour cette opération." & CrLf & CrLf & If(RichTextBox1.Text = "", "Veuillez indiquer une source valide et réessayer.", "Assurez-vous que la source existe dans le système de fichiers et réessayez."), vbOKOnly + vbCritical, "Activer les caractéristiques") = MsgBoxResult.Ok Then
                                    CheckBox2.Checked = True
                                    Button2.PerformClick()
                                End If
                        End Select
                    Else

                    End If
                    Exit For
                End If
            Next
            ProgressPanel.featEnablementLastName = ListView1.CheckedItems(featEnablementCount - 1).ToString()
            If CheckBox1.Checked Then
                ProgressPanel.featisParentPkgNameUsed = True
                ProgressPanel.featParentPkgName = TextBox1.Text
            Else
                ProgressPanel.featisParentPkgNameUsed = False
                ProgressPanel.featParentPkgName = ""
            End If
            If CheckBox2.Checked Then
                ProgressPanel.featisSourceSpecified = True
                If RichTextBox1.Text = "" Or Not Directory.Exists(RichTextBox1.Text) Then
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    MsgBox("The specified source is not valid. Please specify a valid source and try again", vbOKOnly + vbCritical, "Enable features")
                                Case "ESN"
                                    MsgBox("El origen especificado no es válido. Especifique uno válido e inténtelo de nuevo", vbOKOnly + vbCritical, "Habilitar características")
                                Case "FRA"
                                    MsgBox("La source spécifiée n'est pas valide. Veuillez indiquer une source valide et réessayer", vbOKOnly + vbCritical, "Activer les caractéristiques")
                            End Select
                        Case 1
                            MsgBox("The specified source is not valid. Please specify a valid source and try again", vbOKOnly + vbCritical, "Enable features")
                        Case 2
                            MsgBox("El origen especificado no es válido. Especifique uno válido e inténtelo de nuevo", vbOKOnly + vbCritical, "Habilitar características")
                        Case 3
                            MsgBox("La source spécifiée n'est pas valide. Veuillez indiquer une source valide et réessayer", vbOKOnly + vbCritical, "Activer les caractéristiques")
                    End Select
                    Exit Sub
                Else
                    ProgressPanel.featSource = RichTextBox1.Text
                End If
            Else
                ProgressPanel.featisSourceSpecified = True
                ProgressPanel.featSource = ""
            End If
            If CheckBox3.Checked Then
                ProgressPanel.featParentIsEnabled = True
            Else
                ProgressPanel.featParentIsEnabled = False
            End If
            If CheckBox4.Checked Then
                ProgressPanel.featContactWindowsUpdate = True
            ElseIf CheckBox4.Checked = False And CheckBox4.Enabled Then
                ProgressPanel.featContactWindowsUpdate = False
            ElseIf CheckBox4.Enabled = False Then
                ' Tell program to contact Windows Update, as the parameter "/LimitAccess" doesn't apply to offline images
                ProgressPanel.featContactWindowsUpdate = True
            End If
            If CheckBox5.Checked And Not MainForm.OnlineManagement Then
                ProgressPanel.featCommitAfterEnablement = True
            Else
                ProgressPanel.featCommitAfterEnablement = False
            End If
        End If
        ProgressPanel.OperationNum = 30
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub EnableFeature_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Enable features"
                        Label1.Text = Text
                        Label3.Text = "Package name:"
                        Label4.Text = "Feature source:"
                        Button1.Text = "Lookup..."
                        Button2.Text = "Browse..."
                        Button3.Text = "Detect from group policy"
                        Cancel_Button.Text = "Cancel"
                        OK_Button.Text = "OK"
                        GroupBox1.Text = "Features"
                        GroupBox2.Text = "Options"
                        CheckBox1.Text = "Specify parent package name for features"
                        CheckBox2.Text = "Specify feature source"
                        CheckBox3.Text = "Enable all parent features"
                        CheckBox4.Text = "Contact Windows Update for online images"
                        CheckBox5.Text = "Commit image after enabling features"
                        ListView1.Columns(0).Text = "Feature name"
                        ListView1.Columns(1).Text = "State"
                        FolderBrowserDialog1.Description = "Specify a folder which will act as the feature source:"
                    Case "ESN"
                        Text = "Habilitar característica"
                        Label1.Text = Text
                        Label3.Text = "Paquete:"
                        Label4.Text = "Origen:"
                        Button1.Text = "Consultar"
                        Button2.Text = "Examinar..."
                        Button3.Text = "Detectar políticas de grupo"
                        Cancel_Button.Text = "Cancelar"
                        OK_Button.Text = "Aceptar"
                        GroupBox1.Text = "Características"
                        GroupBox2.Text = "Opciones"
                        CheckBox1.Text = "Especificar nombre de paquete principal para características"
                        CheckBox2.Text = "Especificar origen de características"
                        CheckBox3.Text = "Habilitar todas las características principales"
                        CheckBox4.Text = "Contactar Windows Update para instalaciones activas"
                        CheckBox5.Text = "Guardar imagen tras habilitar características"
                        ListView1.Columns(0).Text = "Nombre de característica"
                        ListView1.Columns(1).Text = "Estado"
                        FolderBrowserDialog1.Description = "Especifique una carpeta que actuará como origen de las características:"
                    Case "FRA"
                        Text = "Activer les caractéristiques"
                        Label1.Text = Text
                        Label3.Text = "Nom du paquet :"
                        Label4.Text = "Source de la caractéristique :"
                        Button1.Text = "Rechercher..."
                        Button2.Text = "Parcourir..."
                        Button3.Text = "Détecter à partir des politiques de groupe"
                        Cancel_Button.Text = "Annuler"
                        OK_Button.Text = "OK"
                        GroupBox1.Text = "Caractéristiques"
                        GroupBox2.Text = "Paramètres"
                        CheckBox1.Text = "Spécifier le nom du paquet parent pour les caractéristiques"
                        CheckBox2.Text = "Spécifier la source des caractéristiques"
                        CheckBox3.Text = "Activer toutes les caractéristiques des parents"
                        CheckBox4.Text = "Contacter Windows Update sur les images en ligne"
                        CheckBox5.Text = "Sauvegarder l'image après l'activation des caractéristiques"
                        ListView1.Columns(0).Text = "Nom de la caractéristique"
                        ListView1.Columns(1).Text = "État"
                        FolderBrowserDialog1.Description = "Spécifiez un répertoire qui servira de source des caractéristiques :"
                End Select
            Case 1
                Text = "Enable features"
                Label1.Text = Text
                Label3.Text = "Package name:"
                Label4.Text = "Feature source:"
                Button1.Text = "Lookup..."
                Button2.Text = "Browse..."
                Button3.Text = "Detect from group policy"
                Cancel_Button.Text = "Cancel"
                OK_Button.Text = "OK"
                GroupBox1.Text = "Features"
                GroupBox2.Text = "Options"
                CheckBox1.Text = "Specify parent package name for features"
                CheckBox2.Text = "Specify feature source"
                CheckBox3.Text = "Enable all parent features"
                CheckBox4.Text = "Contact Windows Update for online images"
                CheckBox5.Text = "Commit image after enabling features"
                ListView1.Columns(0).Text = "Feature name"
                ListView1.Columns(1).Text = "State"
                FolderBrowserDialog1.Description = "Specify a folder which will act as the feature source:"
            Case 2
                Text = "Habilitar característica"
                Label1.Text = Text
                Label3.Text = "Paquete:"
                Label4.Text = "Origen:"
                Button1.Text = "Consultar"
                Button2.Text = "Examinar..."
                Button3.Text = "Detectar políticas de grupo"
                Cancel_Button.Text = "Cancelar"
                OK_Button.Text = "Aceptar"
                GroupBox1.Text = "Características"
                GroupBox2.Text = "Opciones"
                CheckBox1.Text = "Especificar nombre de paquete principal para características"
                CheckBox2.Text = "Especificar origen de características"
                CheckBox3.Text = "Habilitar todas las características principales"
                CheckBox4.Text = "Contactar Windows Update para instalaciones activas"
                CheckBox5.Text = "Guardar imagen tras habilitar características"
                ListView1.Columns(0).Text = "Nombre de característica"
                ListView1.Columns(1).Text = "Estado"
                FolderBrowserDialog1.Description = "Especifique una carpeta que actuará como origen de las características:"
            Case 3
                Text = "Activer les caractéristiques"
                Label1.Text = Text
                Label3.Text = "Nom du paquet :"
                Label4.Text = "Source de la caractéristique :"
                Button1.Text = "Rechercher..."
                Button2.Text = "Parcourir..."
                Button3.Text = "Détecter à partir des politiques de groupe"
                Cancel_Button.Text = "Annuler"
                OK_Button.Text = "OK"
                GroupBox1.Text = "Caractéristiques"
                GroupBox2.Text = "Paramètres"
                CheckBox1.Text = "Spécifier le nom du paquet parent pour les caractéristiques"
                CheckBox2.Text = "Spécifier la source des caractéristiques"
                CheckBox3.Text = "Activer toutes les caractéristiques des parents"
                CheckBox4.Text = "Contacter Windows Update sur les images en ligne"
                CheckBox5.Text = "Sauvegarder l'image après l'activation des caractéristiques"
                ListView1.Columns(0).Text = "Nom de la caractéristique"
                ListView1.Columns(1).Text = "État"
                FolderBrowserDialog1.Description = "Spécifiez un répertoire qui servira de source des caractéristiques :"
        End Select
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            RichTextBox1.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            RichTextBox1.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListView1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        RichTextBox1.ForeColor = ForeColor
        PictureBox2.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.image_dark, My.Resources.image_light)
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label2.Text &= " Only disabled features (" & ListView1.Items.Count & ") are shown"
                    Case "ESN"
                        Label2.Text &= " Solo las características deshabilitadas (" & ListView1.Items.Count & ") son mostradas"
                    Case "FRA"
                        Label2.Text &= " Seules les caractéristiques désactivées (" & ListView1.Items.Count & ") sont représentées"
                End Select
            Case 1
                Label2.Text &= " Only disabled features (" & ListView1.Items.Count & ") are shown"
            Case 2
                Label2.Text &= " Solo las características deshabilitadas (" & ListView1.Items.Count & ") son mostradas"
            Case 3
                Label2.Text &= " Seules les caractéristiques désactivées (" & ListView1.Items.Count & ") sont représentées"
        End Select
        CheckBox5.Enabled = If(MainForm.OnlineManagement Or MainForm.OfflineManagement, False, True)
        If MainForm.OnlineManagement And (SystemInformation.BootMode = BootMode.Normal Or SystemInformation.BootMode = BootMode.FailSafeWithNetwork) Then
            CheckBox4.Enabled = True
        Else
            CheckBox4.Checked = False
            CheckBox4.Enabled = False
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Label3.Enabled = True
            Button1.Enabled = True
        Else
            Label3.Enabled = False
            Button1.Enabled = False
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        Label4.Enabled = CheckBox2.Checked = True
        Button2.Enabled = CheckBox2.Checked = True
        RichTextBox1.Enabled = CheckBox2.Checked = True
        Button3.Enabled = CheckBox2.Checked = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PkgParentNameLookupDlg.pkgSource = MainForm.MountDir
        PkgParentNameLookupDlg.ShowDialog(Me)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK And FolderBrowserDialog1.SelectedPath <> "" Then
            RichTextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        RichTextBox1.Text = MainForm.GetSrcFromGPO()
        If RichTextBox1.Text.StartsWith("wim:\", StringComparison.OrdinalIgnoreCase) Then
            TextBoxSourcePanel.Visible = False
            WimFileSourcePanel.Visible = True
            Dim parts() As String = RichTextBox1.Text.Split(":")
            Label6.Text = parts(parts.Length - 1)
            Label5.Text = parts(1).Replace("\", "").Trim() & ":" & parts(2)
            If Label5.Text.EndsWith(":" & parts(parts.Length - 1)) Then Label5.Text = Label5.Text.Replace(":" & parts(parts.Length - 1), "").Trim()
        Else
            TextBoxSourcePanel.Visible = True
            WimFileSourcePanel.Visible = False
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        TextBoxSourcePanel.Visible = True
        WimFileSourcePanel.Visible = False
    End Sub
End Class
