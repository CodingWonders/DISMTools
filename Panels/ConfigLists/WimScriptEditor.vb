Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports ScintillaNET
Imports System.Text.Encoding

Public Class WimScriptEditor

    Public ConfigListFile As String
    Dim EditedLVI As String

    Private Sub WimScriptEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "DISM Configuration List Editor"
                        Label1.Text = "The Configuration List Editor allows you to exclude files and/or folders during actions that let you specify these files, like capturing an image. You can either specify the settings from the graphical interface, or you can create the configuration file manually. When you've finished, click the Save icon."
                        GroupBox1.Text = "Exclusion list"
                        GroupBox2.Text = "Exclusion exception list"
                        GroupBox3.Text = "Compression exclusion list"
                        Button1.Text = "Add..."
                        Button2.Text = "Edit..."
                        Button3.Text = "Remove"
                        Button5.Text = "Add..."
                        Button6.Text = "Edit..."
                        Button7.Text = "Remove"
                        Button9.Text = "Add..."
                        Button10.Text = "Edit..."
                        Button11.Text = "Remove"
                        WimScriptOFD.Title = "Specify the configuration list to load"
                        WimScriptSFD.Title = "Specify the location to save the configuration list to"
                        ToolStripButton2.ToolTipText = "New"
                        ToolStripButton3.ToolTipText = "Open..."
                        ToolStripButton4.ToolTipText = "Save..."
                        ToolStripButton5.ToolTipText = "Toggle word wrap"
                        ToolStripButton6.ToolTipText = "Help"
                    Case "ESN"
                        Text = "Editor de lista de configuraciones de DISM"
                        Label1.Text = "El Editor de Lista de configuraciones le permite excluir archivos y/o carpetas durante acciones que le permiten especificar estos archivos, como capturar una imagen. Puede especificar las configuraciones desde la interfaz gráfica, o puede crear el archivo de configuración manualmente. Cuando haya acabado, haga clic en el icono de Guardar."
                        GroupBox1.Text = "Lista de exclusiones"
                        GroupBox2.Text = "Lista de excepción de exclusiones"
                        GroupBox3.Text = "Lista de exclusión de compresión"
                        Button1.Text = "Añadir..."
                        Button2.Text = "Editar..."
                        Button3.Text = "Eliminar"
                        Button5.Text = "Añadir..."
                        Button6.Text = "Editar..."
                        Button7.Text = "Eliminar"
                        Button9.Text = "Añadir..."
                        Button10.Text = "Editar..."
                        Button11.Text = "Eliminar"
                        WimScriptOFD.Title = "Especifique el archivo de configuración a cargar"
                        WimScriptSFD.Title = "Especifique la ubicación donde guardar el archivo de configuración"
                        ToolStripButton2.ToolTipText = "Nuevo"
                        ToolStripButton3.ToolTipText = "Abrir..."
                        ToolStripButton4.ToolTipText = "Guardar..."
                        ToolStripButton5.ToolTipText = "Cambiar ajuste de línea"
                        ToolStripButton6.ToolTipText = "Ayuda"
                    Case "FRA"
                        Text = "Éditeur de liste de configuration DISM"
                        Label1.Text = "L'éditeur de liste de configuration vous permet d'exclure des fichiers et/ou des dossiers lors d'actions qui vous permettent de spécifier ces fichiers, comme la capture d'une image. Vous pouvez soit spécifier les paramètres à partir de l'interface graphique, soit créer le fichier de configuration manuellement. Lorsque vous avez terminé, cliquez sur l'icône Sauvegarder."
                        GroupBox1.Text = "Liste d'exclusion"
                        GroupBox2.Text = "Liste des exceptions d'exclusion"
                        GroupBox3.Text = "Liste d'exclusion de la compression"
                        Button1.Text = "Ajouter..."
                        Button2.Text = "Modifier..."
                        Button3.Text = "Supprimer"
                        Button5.Text = "Ajouter..."
                        Button6.Text = "Modifier..."
                        Button7.Text = "Supprimer"
                        Button9.Text = "Ajouter..."
                        Button10.Text = "Modifier..."
                        Button11.Text = "Supprimer"
                        WimScriptOFD.Title = "Spécifier la liste de configuration à charger"
                        WimScriptSFD.Title = "Spécifiez l'emplacement où sauvegarder la liste de configuration"
                        ToolStripButton2.ToolTipText = "Nouveau"
                        ToolStripButton3.ToolTipText = "Ouvrir..."
                        ToolStripButton4.ToolTipText = "Sauvegarder..."
                        ToolStripButton5.ToolTipText = "Basculer l'habillage des mots"
                        ToolStripButton6.ToolTipText = "Aide"
                End Select
            Case 1
                Text = "DISM Configuration List Editor"
                Label1.Text = "The Configuration List Editor allows you to exclude files and/or folders during actions that let you specify these files, like capturing an image. You can either specify the settings from the graphical interface, or you can create the configuration file manually. When you've finished, click the Save icon."
                GroupBox1.Text = "Exclusion list"
                GroupBox2.Text = "Exclusion exception list"
                GroupBox3.Text = "Compression exclusion list"
                Button1.Text = "Add..."
                Button2.Text = "Edit..."
                Button3.Text = "Remove"
                Button5.Text = "Add..."
                Button6.Text = "Edit..."
                Button7.Text = "Remove"
                Button9.Text = "Add..."
                Button10.Text = "Edit..."
                Button11.Text = "Remove"
                WimScriptOFD.Title = "Specify the configuration list to load"
                WimScriptSFD.Title = "Specify the location to save the configuration list to"
                ToolStripButton2.ToolTipText = "New"
                ToolStripButton3.ToolTipText = "Open..."
                ToolStripButton4.ToolTipText = "Save..."
                ToolStripButton5.ToolTipText = "Toggle word wrap"
                ToolStripButton6.ToolTipText = "Help"
            Case 2
                Text = "Editor de lista de configuraciones de DISM"
                Label1.Text = "El Editor de Lista de configuraciones le permite excluir archivos y/o carpetas durante acciones que le permiten especificar estos archivos, como capturar una imagen. Puede especificar las configuraciones desde la interfaz gráfica, o puede crear el archivo de configuración manualmente. Cuando haya acabado, haga clic en el icono de Guardar."
                GroupBox1.Text = "Lista de exclusiones"
                GroupBox2.Text = "Lista de excepción de exclusiones"
                GroupBox3.Text = "Lista de exclusión de compresión"
                Button1.Text = "Añadir..."
                Button2.Text = "Editar..."
                Button3.Text = "Eliminar"
                Button5.Text = "Añadir..."
                Button6.Text = "Editar..."
                Button7.Text = "Eliminar"
                Button9.Text = "Añadir..."
                Button10.Text = "Editar..."
                Button11.Text = "Eliminar"
                WimScriptOFD.Title = "Especifique el archivo de configuración a cargar"
                WimScriptSFD.Title = "Especifique la ubicación donde guardar el archivo de configuración"
                ToolStripButton2.ToolTipText = "Nuevo"
                ToolStripButton3.ToolTipText = "Abrir..."
                ToolStripButton4.ToolTipText = "Guardar..."
                ToolStripButton5.ToolTipText = "Cambiar ajuste de línea"
                ToolStripButton6.ToolTipText = "Ayuda"
            Case 3
                Text = "Éditeur de liste de configuration DISM"
                Label1.Text = "L'éditeur de liste de configuration vous permet d'exclure des fichiers et/ou des dossiers lors d'actions qui vous permettent de spécifier ces fichiers, comme la capture d'une image. Vous pouvez soit spécifier les paramètres à partir de l'interface graphique, soit créer le fichier de configuration manuellement. Lorsque vous avez terminé, cliquez sur l'icône Sauvegarder."
                GroupBox1.Text = "Liste d'exclusion"
                GroupBox2.Text = "Liste des exceptions d'exclusion"
                GroupBox3.Text = "Liste d'exclusion de la compression"
                Button1.Text = "Ajouter..."
                Button2.Text = "Modifier..."
                Button3.Text = "Supprimer"
                Button5.Text = "Ajouter..."
                Button6.Text = "Modifier..."
                Button7.Text = "Supprimer"
                Button9.Text = "Ajouter..."
                Button10.Text = "Modifier..."
                Button11.Text = "Supprimer"
                WimScriptOFD.Title = "Spécifier la liste de configuration à charger"
                WimScriptSFD.Title = "Spécifiez l'emplacement où sauvegarder la liste de configuration"
                ToolStripButton2.ToolTipText = "Nouveau"
                ToolStripButton3.ToolTipText = "Ouvrir..."
                ToolStripButton4.ToolTipText = "Sauvegarder..."
                ToolStripButton5.ToolTipText = "Basculer l'habillage des mots"
                ToolStripButton6.ToolTipText = "Aide"
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
        ListView2.BackColor = BackColor
        ListView2.ForeColor = ForeColor
        ListView3.BackColor = BackColor
        ListView3.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        GroupBox2.ForeColor = ForeColor
        GroupBox3.ForeColor = ForeColor
        ' Fill in font combinations
        FontFamilyTSCB.Items.Clear()
        For Each fntFamily As FontFamily In FontFamily.Families
            FontFamilyTSCB.Items.Add(fntFamily.Name)
        Next
        InitScintilla("Courier New", 10)
        FontFamilyTSCB.SelectedItem = "Courier New"
    End Sub

    ''' <summary>
    ''' Initializes the Scintilla editor for WimScript.ini editing
    ''' </summary>
    ''' <param name="fntName">The name of the font used in the Scintilla editor</param>
    ''' <param name="fntSize">The size of the font used in the Scintilla editor</param>
    ''' <remarks></remarks>
    Sub InitScintilla(fntName As String, fntSize As Integer)
        ' Initialize Scintilla editor
        Scintilla1.StyleResetDefault()
        ' Use VS's selection color, as I find it the most natural
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Scintilla1.SetSelectionBackColor(True, Color.FromArgb(38, 79, 120))
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Scintilla1.SetSelectionBackColor(True, Color.FromArgb(153, 201, 239))
        End If
        Scintilla1.Styles(Style.Default).Font = fntName
        Scintilla1.Styles(Style.Default).Size = fntSize

        ' Set background and foreground colors (from Visual Studio)
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Scintilla1.Styles(Style.Default).BackColor = Color.FromArgb(30, 30, 30)
            Scintilla1.Styles(Style.Default).ForeColor = Color.White
            Scintilla1.Styles(Style.LineNumber).BackColor = Color.FromArgb(30, 30, 30)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Scintilla1.Styles(Style.Default).BackColor = Color.White
            Scintilla1.Styles(Style.Default).ForeColor = Color.Black
            Scintilla1.Styles(Style.LineNumber).BackColor = Color.White
        End If
        Scintilla1.StyleClearAll()

        ' Use Notepad++'s lexer style colors
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Scintilla1.Styles(Style.Properties.Default).ForeColor = Color.FromArgb(220, 220, 204)
            Scintilla1.Styles(Style.Properties.Comment).ForeColor = Color.FromArgb(127, 159, 127)
            Scintilla1.Styles(Style.Properties.Section).ForeColor = Color.FromArgb(140, 208, 211)
            Scintilla1.Styles(Style.Properties.Assignment).ForeColor = Color.FromArgb(159, 157, 109)
            Scintilla1.Styles(Style.Properties.DefVal).ForeColor = Color.FromArgb(255, 207, 175)
            Scintilla1.Styles(Style.Properties.Key).ForeColor = Color.FromArgb(223, 196, 125)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Scintilla1.Styles(Style.Properties.Default).ForeColor = Color.Black
            Scintilla1.Styles(Style.Properties.Comment).ForeColor = Color.FromArgb(0, 128, 0)
            Scintilla1.Styles(Style.Properties.Section).ForeColor = Color.FromArgb(128, 0, 255)
            Scintilla1.Styles(Style.Properties.Assignment).ForeColor = Color.Red
            Scintilla1.Styles(Style.Properties.DefVal).ForeColor = Color.Red
            Scintilla1.Styles(Style.Properties.Key).ForeColor = Color.Blue
        End If


        ' Set lexer
        Scintilla1.Lexer = Lexer.Properties

        ' Set line number margin properties
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Scintilla1.Styles(Style.LineNumber).BackColor = Color.FromArgb(30, 30, 30)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Scintilla1.Styles(Style.LineNumber).BackColor = Color.White
        End If
        Scintilla1.Styles(Style.LineNumber).ForeColor = Color.FromArgb(165, 165, 165)
        Dim Margin = Scintilla1.Margins(1)
        Margin.Width = 30
        Margin.Type = MarginType.Number
        Margin.Sensitive = True
        Margin.Mask = 0

        ' Initialize code folding
        Scintilla1.SetFoldMarginColor(True, Scintilla1.Styles(Style.Default).BackColor)
        Scintilla1.SetFoldMarginColor(True, Scintilla1.Styles(Style.Default).BackColor)
        Scintilla1.SetProperty("fold", "1")
        Scintilla1.SetProperty("fold.compact", "1")

        ' Configure bookmark margins
        Dim Bookmarks = Scintilla1.Margins(2)
        Bookmarks.Width = 20
        Bookmarks.Sensitive = True
        Bookmarks.Type = MarginType.Symbol
        Bookmarks.Mask = (1 << 2)
        Dim Marker = Scintilla1.Markers(2)
        Marker.Symbol = MarkerSymbol.Circle
        Marker.SetBackColor(Color.FromArgb(255, 0, 59))
        Marker.SetForeColor(Color.Black)
        Marker.SetAlpha(100)

        ' Set editor caret settings
        Scintilla1.CaretForeColor = ForeColor


        ' Configure code folding margins
        Scintilla1.Margins(3).Type = MarginType.Symbol
        Scintilla1.Margins(3).Mask = Marker.MaskFolders
        Scintilla1.Margins(3).Sensitive = True
        Scintilla1.Margins(3).Width = 1

        ' Set colors for all folding markers
        For x = 25 To 31
            Scintilla1.Markers(x).SetForeColor(Scintilla1.Styles(Style.Default).BackColor)
            Scintilla1.Markers(x).SetBackColor(Scintilla1.Styles(Style.Default).ForeColor)
        Next

        ' Folding marker configuration
        Scintilla1.Markers(Marker.Folder).Symbol = MarkerSymbol.BoxPlus
        Scintilla1.Markers(Marker.FolderOpen).Symbol = MarkerSymbol.BoxMinus
        Scintilla1.Markers(Marker.FolderEnd).Symbol = MarkerSymbol.BoxPlusConnected
        Scintilla1.Markers(Marker.FolderMidTail).Symbol = MarkerSymbol.TCorner
        Scintilla1.Markers(Marker.FolderOpenMid).Symbol = MarkerSymbol.BoxMinusConnected
        Scintilla1.Markers(Marker.FolderSub).Symbol = MarkerSymbol.VLine
        Scintilla1.Markers(Marker.FolderTail).Symbol = MarkerSymbol.LCorner

        ' Enable folding
        Scintilla1.AutomaticFold = (AutomaticFold.Show Or AutomaticFold.Click Or AutomaticFold.Show)
    End Sub

    Private Sub Scintilla1_TextChanged(sender As Object, e As EventArgs) Handles Scintilla1.TextChanged
        ' Clear list views for updated listings
        ListView1.Items.Clear()
        ListView2.Items.Clear()
        ListView3.Items.Clear()

        Dim nextLine As Integer = 0

        ' Go through the configuration file to fill in the entries
        For Each TextLine In Scintilla1.Lines
            nextLine = 0
            If TextLine.Text.Contains("[ExclusionList]") Then
                While Not Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[CompressionExclusionList]") Or Not Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[ExclusionException]")
                    If (TextLine.Index + 1) + nextLine >= Scintilla1.Lines.Count Then Exit While
                    If Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[CompressionExclusionList]") Or Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[ExclusionException]") Then Exit While
                    nextLine += 1
                    If String.IsNullOrWhiteSpace(Scintilla1.Lines(TextLine.Index + nextLine).Text) Then Continue While
                    ListView1.Items.Add(Scintilla1.Lines(TextLine.Index + nextLine).Text)
                End While
            ElseIf TextLine.Text.Contains("[ExclusionException]") Then
                While Not Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[CompressionExclusionList]") Or Not Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[ExclusionList]")
                    If (TextLine.Index + 1) + nextLine >= Scintilla1.Lines.Count Then Exit While
                    If Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[CompressionExclusionList]") Or Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[ExclusionList]") Then Exit While
                    nextLine += 1
                    If String.IsNullOrWhiteSpace(Scintilla1.Lines(TextLine.Index + nextLine).Text) Then Continue While
                    ListView2.Items.Add(Scintilla1.Lines(TextLine.Index + nextLine).Text)
                End While
            ElseIf TextLine.Text.Contains("[CompressionExclusionList]") Then
                While Not Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[ExclusionList]") Or Not Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[ExclusionException]")
                    If (TextLine.Index + 1) + nextLine >= Scintilla1.Lines.Count Then Exit While
                    If Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[ExclusionList]") Or Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[ExclusionException]") Then Exit While
                    nextLine += 1
                    If String.IsNullOrWhiteSpace(Scintilla1.Lines(TextLine.Index + nextLine).Text) Then Continue While
                    ListView3.Items.Add(Scintilla1.Lines(TextLine.Index + nextLine).Text)
                End While
            End If
        Next

        ' Remove unnecessary ListView items
        For Each LVItem As ListViewItem In ListView1.Items
            If LVItem.Text.Contains("[ExclusionList]") Or _
                LVItem.Text.Contains("[CompressionExclusionList]") Or _
                LVItem.Text.Contains("[ExclusionException]") Then
                ListView1.Items.Remove(LVItem)
            End If
        Next
        For Each LVItem As ListViewItem In ListView2.Items
            If LVItem.Text.Contains("[ExclusionList]") Or _
                LVItem.Text.Contains("[CompressionExclusionList]") Or _
                LVItem.Text.Contains("[ExclusionException]") Then
                ListView2.Items.Remove(LVItem)
            End If
        Next
        For Each LVItem As ListViewItem In ListView3.Items
            If LVItem.Text.Contains("[ExclusionList]") Or _
                LVItem.Text.Contains("[CompressionExclusionList]") Or _
                LVItem.Text.Contains("[ExclusionException]") Then
                ListView3.Items.Remove(LVItem)
            End If
        Next

        ' Indicate whether file has seen changes, if it exists
        If ConfigListFile IsNot Nothing And File.Exists(ConfigListFile) Then
            Dim titleMsg As String = ""
            If File.ReadAllText(ConfigListFile).ToString() = Scintilla1.Text Then
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                            Case "ESN"
                                titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                            Case "FRA"
                                titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                        End Select
                    Case 1
                        titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                    Case 2
                        titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                    Case 3
                        titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                End Select
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                titleMsg = Path.GetFileName(ConfigListFile) & " (modified) - DISM Configuration List Editor"
                            Case "ESN"
                                titleMsg = Path.GetFileName(ConfigListFile) & " (modificado) - Editor de lista de configuraciones de DISM"
                            Case "FRA"
                                titleMsg = Path.GetFileName(ConfigListFile) & " (modifié) - Éditeur de liste de configuration DISM"
                        End Select
                    Case 1
                        titleMsg = Path.GetFileName(ConfigListFile) & " (modified) - DISM Configuration List Editor"
                    Case 2
                        titleMsg = Path.GetFileName(ConfigListFile) & " (modificado) - Editor de lista de configuraciones de DISM"
                    Case 3
                        titleMsg = Path.GetFileName(ConfigListFile) & " (modifié) - Éditeur de liste de configuration DISM"
                End Select
            End If
            Text = titleMsg
        End If
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        Dim msg As String = ""
        Dim titleMsg As String = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        msg = "Do you want to save this configuration list file?"
                        titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & " - DISM Configuration List Editor"
                    Case "ESN"
                        msg = "¿Desea guardar este archivo de lista de configuraciones?"
                        titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & " - Editor de lista de configuraciones de DISM"
                    Case "FRA"
                        msg = "Voulez-vous sauvegarder ce fichier de liste de configuration ?"
                        titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & " - Éditeur de liste de configuration DISM"
                End Select
            Case 1
                msg = "Do you want to save this configuration list file?"
                titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & " - DISM Configuration List Editor"
            Case 2
                msg = "¿Desea guardar este archivo de lista de configuraciones?"
                titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & " - Editor de lista de configuraciones de DISM"
            Case 3
                msg = "Voulez-vous sauvegarder ce fichier de liste de configuration ?"
                titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & " - Éditeur de liste de configuration DISM"
        End Select
        If (ConfigListFile Is Nothing Or Not File.Exists(ConfigListFile)) And Scintilla1.Text <> "" Then
            Dim Result As MsgBoxResult = MsgBox(msg, vbYesNoCancel + vbQuestion, Text)
            Select Case Result
                Case MsgBoxResult.Yes
                    If File.Exists(ConfigListFile) Then
                        File.WriteAllText(ConfigListFile, Scintilla1.Text, ASCII)
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                    Case "ESN"
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                    Case "FRA"
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                End Select
                            Case 1
                                titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                            Case 2
                                titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                            Case 3
                                titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                        End Select
                        Text = titleMsg
                    Else
                        If WimScriptSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                            ConfigListFile = WimScriptSFD.FileName
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                        Case "ESN"
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                        Case "FRA"
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                    End Select
                                Case 1
                                    titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                Case 2
                                    titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                Case 3
                                    titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                            End Select
                            Text = titleMsg
                        Else
                            Exit Sub
                        End If
                    End If
                Case MsgBoxResult.No
                    Exit Select
                Case MsgBoxResult.Cancel
                    Exit Sub
            End Select
        Else
            Try
                If (ConfigListFile IsNot Nothing And File.Exists(ConfigListFile) And File.ReadAllText(ConfigListFile).ToString() <> Scintilla1.Text) Then
                    Dim Result As MsgBoxResult = MsgBox(msg, vbYesNoCancel + vbQuestion, Text)
                    Select Case Result
                        Case MsgBoxResult.Yes
                            If File.Exists(ConfigListFile) Then
                                File.WriteAllText(ConfigListFile, Scintilla1.Text, ASCII)
                                Select Case MainForm.Language
                                    Case 0
                                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                            Case "ENU", "ENG"
                                                titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                            Case "ESN"
                                                titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                            Case "FRA"
                                                titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                        End Select
                                    Case 1
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                    Case 2
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                    Case 3
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                End Select
                                Text = titleMsg
                            Else
                                If WimScriptSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
                                    File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                                    ConfigListFile = WimScriptSFD.FileName
                                    Select Case MainForm.Language
                                        Case 0
                                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                                Case "ENU", "ENG"
                                                    titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                                Case "ESN"
                                                    titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                                Case "FRA"
                                                    titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                            End Select
                                        Case 1
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                        Case 2
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                        Case 3
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                    End Select
                                    Text = titleMsg
                                Else
                                    Exit Sub
                                End If
                            End If
                        Case MsgBoxResult.No
                            Exit Select
                        Case MsgBoxResult.Cancel
                            Exit Sub
                    End Select
                End If
            Catch ex As Exception
                Exit Try
            End Try
        End If

        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "New configuration list - DISM Configuration List Editor"
                    Case "ESN"
                        Text = "Nueva lista de configuraciones - Editor de lista de configuración de DISM"
                    Case "FRA"
                        Text = "Nouvelle liste de configuration - Éditeur de liste de configuration DISM"
                End Select
            Case 1
                Text = "New configuration list - DISM Configuration List Editor"
            Case 2
                Text = "Nueva lista de configuraciones - Editor de lista de configuración de DISM"
            Case 3
                Text = "Nouvelle liste de configuration - Éditeur de liste de configuration DISM"
        End Select

        ' Generate a default configuration list, as shown in the DISM configuration list documentation.
        ' Source: https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism-configuration-list-and-wimscriptini-files-winnext?view=windows-11

        ConfigListFile = ""

        Scintilla1.Text = CrLf & _
            "[ExclusionList]" & CrLf & _
            "\$ntfs.log" & CrLf & _
            "\hiberfil.sys" & CrLf & _
            "\pagefile.sys" & CrLf & _
            "\swapfile.sys" & CrLf & _
            "\System Volume Information" & CrLf & _
            "\RECYCLER" & CrLf & _
            "\Windows\CSC" & CrLf & CrLf & _
            "[CompressionExclusionList]" & CrLf & _
            "*.mp3" & CrLf & _
            "*.zip" & CrLf & _
            "*.cab" & CrLf & _
            "\WINDOWS\inf\*.pnf"
    End Sub

    Private Sub FontChange(sender As Object, e As EventArgs) Handles FontFamilyTSCB.SelectedIndexChanged, FontSizeTSCB.SelectedIndexChanged
        ' Change Scintilla editor font
        InitScintilla(FontFamilyTSCB.SelectedItem, FontSizeTSCB.SelectedItem)
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        Dim msg As String = ""
        Dim titleMsg As String = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        msg = "Do you want to save this configuration list file?"
                        titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & " - DISM Configuration List Editor"
                    Case "ESN"
                        msg = "¿Desea guardar este archivo de lista de configuraciones?"
                        titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                    Case "FRA"
                        msg = "Voulez-vous sauvegarder ce fichier de liste de configuration ?"
                        titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                End Select
            Case 1
                msg = "Do you want to save this configuration list file?"
                titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
            Case 2
                msg = "¿Desea guardar este archivo de lista de configuraciones?"
                titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
            Case 3
                msg = "Voulez-vous sauvegarder ce fichier de liste de configuration ?"
                titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
        End Select
        If (ConfigListFile Is Nothing Or Not File.Exists(ConfigListFile)) And Scintilla1.Text <> "" Then
            Dim Result As MsgBoxResult = MsgBox(msg, vbYesNoCancel + vbQuestion, Text)
            Select Case Result
                Case MsgBoxResult.Yes
                    If File.Exists(ConfigListFile) Then
                        File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                        ConfigListFile = WimScriptSFD.FileName
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                    Case "ESN"
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                    Case "FRA"
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                End Select
                            Case 1
                                titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                            Case 2
                                titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                            Case 3
                                titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                        End Select
                        Text = titleMsg
                    Else
                        If WimScriptSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                            ConfigListFile = WimScriptSFD.FileName
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                        Case "ESN"
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                        Case "FRA"
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                    End Select
                                Case 1
                                    titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                Case 2
                                    titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                Case 3
                                    titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                            End Select
                            Text = titleMsg
                        Else
                            Exit Sub
                        End If
                    End If
                Case MsgBoxResult.No
                    Exit Select
                Case MsgBoxResult.Cancel
                    Exit Sub
            End Select
        Else
            Try
                If (ConfigListFile IsNot Nothing And File.Exists(ConfigListFile) And File.ReadAllText(ConfigListFile).ToString() <> Scintilla1.Text) Then
                    Dim Result As MsgBoxResult = MsgBox(msg, vbYesNoCancel + vbQuestion, Text)
                    Select Case Result
                        Case MsgBoxResult.Yes
                            If File.Exists(ConfigListFile) Then
                                File.WriteAllText(ConfigListFile, Scintilla1.Text, ASCII)
                                Select Case MainForm.Language
                                    Case 0
                                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                            Case "ENU", "ENG"
                                                titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                            Case "ESN"
                                                titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                            Case "FRA"
                                                titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                        End Select
                                    Case 1
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                    Case 2
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                    Case 3
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                End Select
                                Text = titleMsg
                            Else
                                If WimScriptSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
                                    File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                                    ConfigListFile = WimScriptSFD.FileName
                                    Select Case MainForm.Language
                                        Case 0
                                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                                Case "ENU", "ENG"
                                                    titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                                Case "ESN"
                                                    titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                                Case "FRA"
                                                    titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                            End Select
                                        Case 1
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                        Case 2
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                        Case 3
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                    End Select
                                    Text = titleMsg
                                Else
                                    Exit Sub
                                End If
                            End If
                        Case MsgBoxResult.No
                            Exit Select
                        Case MsgBoxResult.Cancel
                            Exit Sub
                    End Select
                End If
            Catch ex As Exception
                Exit Try
            End Try
        End If
        WimScriptOFD.ShowDialog()
    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        If ConfigListFile Is Nothing Or Not File.Exists(ConfigListFile) Then
            If WimScriptSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
                File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                ConfigListFile = WimScriptSFD.FileName
            End If
        Else
            File.WriteAllText(ConfigListFile, Scintilla1.Text, ASCII)
        End If
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                    Case "ESN"
                        Text = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                    Case "FRA"
                        Text = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                End Select
            Case 1
                Text = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
            Case 2
                Text = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
            Case 3
                Text = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
        End Select
    End Sub

    Private Sub WimScriptOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles WimScriptOFD.FileOk
        Scintilla1.Text = File.ReadAllText(WimScriptOFD.FileName)
        ConfigListFile = WimScriptOFD.FileName
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                    Case "ESN"
                        Text = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                    Case "FRA"
                        Text = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                End Select
            Case 1
                Text = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
            Case 2
                Text = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
            Case 3
                Text = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
        End Select
    End Sub

    Private Sub ToolStripButton6_Click(sender As Object, e As EventArgs) Handles ToolStripButton6.Click
        Process.Start("https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism-configuration-list-and-wimscriptini-files-winnext?view=windows-11")
    End Sub

    Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click
        If ToolStripButton5.Checked Then
            ToolStripButton5.Checked = False
        Else
            ToolStripButton5.Checked = True
        End If
        Scintilla1.WrapMode = If(ToolStripButton5.Checked, WrapMode.Word, WrapMode.None)
    End Sub

#Region "Button Regions"

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count = 1 Then
            Button2.Enabled = True
            Button3.Enabled = True
        Else
            Button2.Enabled = False
            Button3.Enabled = False
        End If
    End Sub

    Private Sub ListView2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView2.SelectedIndexChanged
        If ListView2.SelectedItems.Count = 1 Then
            Button6.Enabled = True
            Button7.Enabled = True
        Else
            Button6.Enabled = False
            Button7.Enabled = False
        End If
    End Sub

    Private Sub ListView3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView3.SelectedIndexChanged
        If ListView3.SelectedItems.Count = 1 Then
            Button11.Enabled = True
            Button9.Enabled = True
        Else
            Button11.Enabled = False
            Button9.Enabled = False
        End If
    End Sub

    Sub UpdateConfigListContents()
        ' Remove the TextChanged handler to avoid bad behavior (nothing showing up)
        RemoveHandler Scintilla1.TextChanged, AddressOf Scintilla1_TextChanged

        ' Clear text in Scintilla editor and add it back
        Scintilla1.ClearAll()
        If ListView1.Items.Count > 0 Then
            Scintilla1.AppendText(CrLf & _
                                  "[ExclusionList]" & CrLf)
            For Each LVI As ListViewItem In ListView1.Items
                Scintilla1.AppendText(LVI.Text)
            Next
            ' End with carriage return line feed
            Scintilla1.AppendText(CrLf)
        End If
        If ListView2.Items.Count > 0 Then
            Scintilla1.AppendText(CrLf & _
                                  "[ExclusionException]" & CrLf)
            For Each LVI As ListViewItem In ListView2.Items
                Scintilla1.AppendText(LVI.Text)
            Next
            ' End with carriage return line feed
            Scintilla1.AppendText(CrLf)
        End If
        If ListView3.Items.Count > 0 Then
            Scintilla1.AppendText(CrLf & _
                                  "[CompressionExclusionList]" & CrLf)
            For Each LVI As ListViewItem In ListView3.Items
                Scintilla1.AppendText(LVI.Text)
            Next
            ' End with carriage return line feed
            Scintilla1.AppendText(CrLf)
        End If

        ' Indicate whether file has seen changes, if it exists
        If ConfigListFile IsNot Nothing And File.Exists(ConfigListFile) Then
            If File.ReadAllText(ConfigListFile).ToString() = Scintilla1.Text Then
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                Text = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                            Case "ESN"
                                Text = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                        End Select
                    Case 1
                        Text = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                    Case 2
                        Text = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                End Select
            Else
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                Text = Path.GetFileName(ConfigListFile) & " (modified) - DISM Configuration List Editor"
                            Case "ESN"
                                Text = Path.GetFileName(ConfigListFile) & " (modificado) - Editor de lista de configuraciones de DISM"
                        End Select
                    Case 1
                        Text = Path.GetFileName(ConfigListFile) & " (modified) - DISM Configuration List Editor"
                    Case 2
                        Text = Path.GetFileName(ConfigListFile) & " (modificado) - Editor de lista de configuraciones de DISM"
                End Select
            End If
        End If

        ' Add TextChanged event handler to let the user type files in the Scintilla editor again
        AddHandler Scintilla1.TextChanged, AddressOf Scintilla1_TextChanged
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AddListEntryDlg.IsForExclusionList = True
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        AddListEntryDlg.Text = "Add " & GroupBox1.Text.ToLower() & " entry"
                    Case "ESN"
                        AddListEntryDlg.Text = "Añadir entrada de " & GroupBox1.Text.ToLower()
                    Case "FRA"
                        AddListEntryDlg.Text = "Ajouter une entrée à la " & GroupBox1.Text.ToLower()
                End Select
            Case 1
                AddListEntryDlg.Text = "Add " & GroupBox1.Text.ToLower() & " entry"
            Case 2
                AddListEntryDlg.Text = "Añadir entrada de " & GroupBox1.Text.ToLower()
            Case 3
                AddListEntryDlg.Text = "Ajouter une entrée à la " & GroupBox1.Text.ToLower()
        End Select
        AddListEntryDlg.Left = Left + ((SplitContainer1.SplitterDistance + Scintilla1.Width) / 2)
        AddListEntryDlg.Top = Top + Panel2.Top + DarkToolStrip1.Height + SplitContainer1.Top + GroupBox1.Top + 8
        AddListEntryDlg.ShowDialog()
        If AddListEntryDlg.DialogResult = Windows.Forms.DialogResult.OK Then
            ListView1.Items.Add(AddListEntryDlg.TextBox1.Text)
            UpdateConfigListContents()
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        AddListEntryDlg.IsForExclusionList = False
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        AddListEntryDlg.Text = "Add " & GroupBox2.Text.ToLower() & " entry"
                    Case "ESN"
                        AddListEntryDlg.Text = "Añadir entrada de " & GroupBox2.Text.ToLower()
                    Case "FRA"
                        AddListEntryDlg.Text = "Ajouter une entrée à la " & GroupBox2.Text.ToLower()
                End Select
            Case 1
                AddListEntryDlg.Text = "Add " & GroupBox2.Text.ToLower() & " entry"
            Case 2
                AddListEntryDlg.Text = "Añadir entrada de " & GroupBox2.Text.ToLower()
            Case 3
                AddListEntryDlg.Text = "Ajouter une entrée à la " & GroupBox2.Text.ToLower()
        End Select
        AddListEntryDlg.Left = Left + ((SplitContainer1.SplitterDistance + Scintilla1.Width) / 2)
        AddListEntryDlg.Top = Top + Panel2.Top + DarkToolStrip1.Height + SplitContainer1.Top + GroupBox2.Top + 8
        AddListEntryDlg.ShowDialog()
        If AddListEntryDlg.DialogResult = Windows.Forms.DialogResult.OK Then
            ListView2.Items.Add(AddListEntryDlg.TextBox1.Text)
            UpdateConfigListContents()
        End If
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button9.Click
        AddListEntryDlg.IsForExclusionList = False
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        AddListEntryDlg.Text = "Add " & GroupBox3.Text.ToLower() & " entry"
                    Case "ESN"
                        AddListEntryDlg.Text = "Añadir entrada de " & GroupBox3.Text.ToLower()
                    Case "FRA"
                        AddListEntryDlg.Text = "Ajouter une entrée à la " & GroupBox3.Text.ToLower()
                End Select
            Case 1
                AddListEntryDlg.Text = "Add " & GroupBox3.Text.ToLower() & " entry"
            Case 2
                AddListEntryDlg.Text = "Añadir entrada de " & GroupBox3.Text.ToLower()
            Case 3
                AddListEntryDlg.Text = "Ajouter une entrée à la " & GroupBox3.Text.ToLower()
        End Select
        AddListEntryDlg.Left = Left + ((SplitContainer1.SplitterDistance + Scintilla1.Width) / 2)
        AddListEntryDlg.Top = Top + Panel2.Top + DarkToolStrip1.Height + SplitContainer1.Top + GroupBox3.Top + 8
        AddListEntryDlg.ShowDialog()
        If AddListEntryDlg.DialogResult = Windows.Forms.DialogResult.OK Then
            ListView3.Items.Add(AddListEntryDlg.TextBox1.Text)
            UpdateConfigListContents()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If ListView1.SelectedItems.Count = 1 Then
            ListView1.Items.Remove(ListView1.FocusedItem)
            UpdateConfigListContents()
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If ListView2.SelectedItems.Count = 1 Then
            ListView2.Items.Remove(ListView2.FocusedItem)
            UpdateConfigListContents()
        End If
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If ListView3.SelectedItems.Count = 1 Then
            ListView3.Items.Remove(ListView3.FocusedItem)
            UpdateConfigListContents()
        End If
    End Sub

    Private Sub ListView1_BeforeLabelEdit(sender As Object, e As LabelEditEventArgs) Handles ListView1.BeforeLabelEdit
        EditedLVI = ListView1.Items(e.Item).Text
    End Sub

    Private Sub ListView1_AfterLabelEdit(sender As Object, e As LabelEditEventArgs) Handles ListView1.AfterLabelEdit
        Scintilla1.Text = Scintilla1.Text.Replace(EditedLVI, e.Label & CrLf).Trim()
    End Sub

    Private Sub ListView2_BeforeLabelEdit(sender As Object, e As LabelEditEventArgs) Handles ListView2.BeforeLabelEdit
        EditedLVI = ListView2.Items(e.Item).Text
    End Sub

    Private Sub ListView2_AfterLabelEdit(sender As Object, e As LabelEditEventArgs) Handles ListView2.AfterLabelEdit
        Scintilla1.Text = Scintilla1.Text.Replace(EditedLVI, e.Label & CrLf).Trim()
    End Sub

    Private Sub ListView3_BeforeLabelEdit(sender As Object, e As LabelEditEventArgs) Handles ListView3.BeforeLabelEdit
        EditedLVI = ListView3.Items(e.Item).Text
    End Sub

    Private Sub ListView3_AfterLabelEdit(sender As Object, e As LabelEditEventArgs) Handles ListView3.AfterLabelEdit
        Scintilla1.Text = Scintilla1.Text.Replace(EditedLVI, e.Label & CrLf).Trim()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If ListView3.SelectedItems.Count = 1 Then
            Dim LVI As ListViewItem = ListView3.FocusedItem
            LVI.BeginEdit()
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If ListView2.SelectedItems.Count = 1 Then
            Dim LVI As ListViewItem = ListView2.FocusedItem
            LVI.BeginEdit()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ListView1.SelectedItems.Count = 1 Then
            Dim LVI As ListViewItem = ListView1.FocusedItem
            LVI.BeginEdit()
        End If
    End Sub

#End Region

    Private Sub WimScriptEditor_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim msg As String = ""
        Dim titleMsg As String = ""
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        msg = "Do you want to save this configuration list file?"
                        titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & " - DISM Configuration List Editor"
                    Case "ESN"
                        msg = "¿Desea guardar este archivo de lista de configuraciones?"
                        titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                    Case "FRA"
                        msg = "Voulez-vous sauvegarder ce fichier de liste de configuration ?"
                        titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                End Select
            Case 1
                msg = "Do you want to save this configuration list file?"
                titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
            Case 2
                msg = "¿Desea guardar este archivo de lista de configuraciones?"
                titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
            Case 3
                msg = "Voulez-vous sauvegarder ce fichier de liste de configuration ?"
                titleMsg = If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), "") & Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
        End Select
        If (ConfigListFile Is Nothing Or Not File.Exists(ConfigListFile)) And Scintilla1.Text <> "" Then
            Dim Result As MsgBoxResult = MsgBox(msg, vbYesNoCancel + vbQuestion, Text)
            Select Case Result
                Case MsgBoxResult.Yes
                    If File.Exists(ConfigListFile) Then
                        File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                        ConfigListFile = WimScriptSFD.FileName
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                    Case "ESN"
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                    Case "FRA"
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                End Select
                            Case 1
                                titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                            Case 2
                                titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                            Case 3
                                titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                        End Select
                        Text = titleMsg
                    Else
                        If WimScriptSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                            ConfigListFile = WimScriptSFD.FileName
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                        Case "ESN"
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                        Case "FRA"
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                    End Select
                                Case 1
                                    titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                Case 2
                                    titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                Case 3
                                    titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                            End Select
                            Text = titleMsg
                        Else
                            e.Cancel = True
                        End If
                    End If
                Case MsgBoxResult.No
                    Exit Select
                Case MsgBoxResult.Cancel
                    e.Cancel = True
            End Select
        Else
            Try
                If (ConfigListFile IsNot Nothing And File.Exists(ConfigListFile) And File.ReadAllText(ConfigListFile).ToString() <> Scintilla1.Text) Then
                    Dim Result As MsgBoxResult = MsgBox(msg, vbYesNoCancel + vbQuestion, Text)
                    Select Case Result
                        Case MsgBoxResult.Yes
                            If File.Exists(ConfigListFile) Then
                                File.WriteAllText(ConfigListFile, Scintilla1.Text, ASCII)
                                Select Case MainForm.Language
                                    Case 0
                                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                            Case "ENU", "ENG"
                                                titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                            Case "ESN"
                                                titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                            Case "FRA"
                                                titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                        End Select
                                    Case 1
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                    Case 2
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                    Case 3
                                        titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                End Select
                                Text = titleMsg
                            Else
                                If WimScriptSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
                                    File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                                    ConfigListFile = WimScriptSFD.FileName
                                    Select Case MainForm.Language
                                        Case 0
                                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                                Case "ENU", "ENG"
                                                    titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                                Case "ESN"
                                                    titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                                Case "FRA"
                                                    titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                            End Select
                                        Case 1
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                                        Case 2
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - Editor de lista de configuraciones de DISM"
                                        Case 3
                                            titleMsg = Path.GetFileName(ConfigListFile) & " - Éditeur de liste de configuration DISM"
                                    End Select
                                    Text = titleMsg
                                Else
                                    e.Cancel = True
                                End If
                            End If
                        Case MsgBoxResult.No
                            Exit Select
                        Case MsgBoxResult.Cancel
                            e.Cancel = True
                    End Select
                End If
            Catch ex As Exception
                Exit Try
            End Try
        End If
    End Sub

    Private Sub WimScriptEditor_VisibleChanged(sender As Object, e As EventArgs) Handles MyBase.VisibleChanged
        If Visible Then
            Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
            If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
        End If
    End Sub
End Class