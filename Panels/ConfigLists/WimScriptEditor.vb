Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports ScintillaNET
Imports System.Text.Encoding

Public Class WimScriptEditor

    Public ConfigListFile As String
    Dim EditedLVI As String
    Dim scaled As Boolean

    Private Sub WimScriptEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("WimScriptEditor")("ConfigList.Title")
        Label1.Text = LocalizationService.ForSection("WimScriptEditor")("Config.List.Allows.Message")
        GroupBox1.Text = LocalizationService.ForSection("WimScriptEditor")("ExclusionList.Group")
        GroupBox2.Text = LocalizationService.ForSection("WimScriptEditor")("Exclusion.Exception.List")
        GroupBox3.Text = LocalizationService.ForSection("WimScriptEditor")("Compression.Exclusion.List")
        Button1.Text = LocalizationService.ForSection("WimScriptEditor")("Add.Button")
        Button2.Text = LocalizationService.ForSection("WimScriptEditor")("Edit.Button")
        Button3.Text = LocalizationService.ForSection("WimScriptEditor")("Remove.Button")
        Button5.Text = LocalizationService.ForSection("WimScriptEditor")("Add.Button")
        Button6.Text = LocalizationService.ForSection("WimScriptEditor")("Edit.Button")
        Button7.Text = LocalizationService.ForSection("WimScriptEditor")("Remove.Button")
        Button9.Text = LocalizationService.ForSection("WimScriptEditor")("Add.Button")
        Button10.Text = LocalizationService.ForSection("WimScriptEditor")("Edit.Button")
        Button11.Text = LocalizationService.ForSection("WimScriptEditor")("Remove.Button")
        WimScriptOFD.Title = LocalizationService.ForSection("WimScriptEditor")("Config.List.Load.Title")
        WimScriptSFD.Title = LocalizationService.ForSection("WimScriptEditor")("Location.Save.Config.Title")
        ToolStripButton2.ToolTipText = LocalizationService.ForSection("WimScriptEditor")("New.Tooltip")
        ToolStripButton3.ToolTipText = LocalizationService.ForSection("WimScriptEditor")("Open.Button")
        ToolStripButton4.ToolTipText = LocalizationService.ForSection("WimScriptEditor")("Save.Button")
        ToolStripButton5.ToolTipText = LocalizationService.ForSection("WimScriptEditor")("Toggle.Word.Wrap.Tooltip")
        ToolStripButton6.ToolTipText = LocalizationService.ForSection("WimScriptEditor")("Help.Tooltip")
        ToolStripDropDownButton1.Text = LocalizationService.ForSection("WimScriptEditor")("Tools.Label")
        NoOneDriveToolStripMenuItem.Text = LocalizationService.ForSection("WimScriptEditor")("Exclude.User.One.Button")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ListView1.BackColor = CurrentTheme.BackgroundColor
        ListView1.ForeColor = CurrentTheme.ForegroundColor
        ListView2.BackColor = CurrentTheme.BackgroundColor
        ListView2.ForeColor = CurrentTheme.ForegroundColor
        ListView3.BackColor = CurrentTheme.BackgroundColor
        ListView3.ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        GroupBox3.ForeColor = CurrentTheme.ForegroundColor
        ' Fill in font combinations
        FontFamilyTSCB.Items.Clear()
        For Each fntFamily As FontFamily In FontFamily.Families
            FontFamilyTSCB.Items.Add(fntFamily.Name)
        Next
        If Not scaled Then
            SplitContainer1.SplitterDistance = WindowHelper.ScaleLogical(SplitContainer1.SplitterDistance)
            scaled = True
        End If
        InitScintilla("Consolas", 11)
        FontFamilyTSCB.SelectedItem = "Consolas"
    End Sub

    ''' <summary>
    ''' Initializes the Scintilla editor for WimScript.ini editing
    ''' </summary>
    ''' <param name="fntName">The name of the font used in the Scintilla editor</param>
    ''' <param name="fntSize">The size of the font used in the Scintilla editor</param>
    ''' <remarks></remarks>
    Sub InitScintilla(fntName As String, fntSize As Integer)
        DynaLog.LogMessage("Initializing the Scintilla Editor...")
        DynaLog.LogMessage("- Font name: " & fntName)
        DynaLog.LogMessage("- Font size: " & fntSize)
        ' Initialize Scintilla editor
        DynaLog.LogMessage("Resetting styles...")
        Scintilla1.StyleResetDefault()
        ' Use VS's selection color, as I find it the most natural
        DynaLog.LogMessage("Setting colors for selection...")
        If CurrentTheme.IsDark Then
            Scintilla1.SelectionBackColor = Color.FromArgb(38, 79, 120)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Scintilla1.SelectionBackColor = Color.FromArgb(153, 201, 239)
        End If
        Scintilla1.Styles(Style.Default).Font = fntName
        Scintilla1.Styles(Style.Default).Size = fntSize

        ' Set background and foreground colors (from Visual Studio)
        DynaLog.LogMessage("Setting colors for styles...")
        Scintilla1.Styles(Style.Default).BackColor = CurrentTheme.SectionBackgroundColor
        Scintilla1.Styles(Style.Default).ForeColor = CurrentTheme.ForegroundColor
        Scintilla1.Styles(Style.LineNumber).BackColor = CurrentTheme.SectionBackgroundColor
        Scintilla1.StyleClearAll()

        ' Use Notepad++'s lexer style colors
        DynaLog.LogMessage("Setting colors for INI lexer...")
        If CurrentTheme.IsDark Then
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
        Scintilla1.LexerName = "props"

        ' Set line number margin properties
        DynaLog.LogMessage("Setting colors for line margin...")
        Scintilla1.Styles(Style.LineNumber).BackColor = CurrentTheme.SectionBackgroundColor
        Scintilla1.Styles(Style.LineNumber).ForeColor = CurrentTheme.ForegroundColor
        Dim Margin = Scintilla1.Margins(1)
        Margin.Width = 30
        Margin.Type = MarginType.Number
        Margin.Sensitive = True
        Margin.Mask = 0

        ' Initialize code folding
        DynaLog.LogMessage("Setting code folding...")
        Scintilla1.SetFoldMarginColor(True, Scintilla1.Styles(Style.Default).BackColor)
        Scintilla1.SetFoldMarginColor(True, Scintilla1.Styles(Style.Default).BackColor)
        Scintilla1.SetProperty("fold", "1")
        Scintilla1.SetProperty("fold.compact", "1")

        ' Configure bookmark margins
        DynaLog.LogMessage("Seting bookmark margins...")
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
        DynaLog.LogMessage("Setting colors for editor caret...")
        Scintilla1.CaretForeColor = CurrentTheme.ForegroundColor


        ' Configure code folding margins
        DynaLog.LogMessage("Setting margins for code folding...")
        Scintilla1.Margins(3).Type = MarginType.Symbol
        Scintilla1.Margins(3).Mask = Marker.MaskFolders
        Scintilla1.Margins(3).Sensitive = True
        Scintilla1.Margins(3).Width = 1

        ' Set colors for all folding markers
        DynaLog.LogMessage("Setting colors for folding markers...")
        For x = 25 To 31
            Scintilla1.Markers(x).SetForeColor(Scintilla1.Styles(Style.Default).BackColor)
            Scintilla1.Markers(x).SetBackColor(Scintilla1.Styles(Style.Default).ForeColor)
        Next

        ' Folding marker configuration
        DynaLog.LogMessage("Setting folding marker...")
        Scintilla1.Markers(Marker.Folder).Symbol = MarkerSymbol.BoxPlus
        Scintilla1.Markers(Marker.FolderOpen).Symbol = MarkerSymbol.BoxMinus
        Scintilla1.Markers(Marker.FolderEnd).Symbol = MarkerSymbol.BoxPlusConnected
        Scintilla1.Markers(Marker.FolderMidTail).Symbol = MarkerSymbol.TCorner
        Scintilla1.Markers(Marker.FolderOpenMid).Symbol = MarkerSymbol.BoxMinusConnected
        Scintilla1.Markers(Marker.FolderSub).Symbol = MarkerSymbol.VLine
        Scintilla1.Markers(Marker.FolderTail).Symbol = MarkerSymbol.LCorner

        ' Enable folding
        DynaLog.LogMessage("Enabling folding...")
        Scintilla1.AutomaticFold = (AutomaticFold.Show Or AutomaticFold.Click Or AutomaticFold.Show)

        DynaLog.LogMessage("Scintilla editor initialization complete.")
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
                    DynaLog.LogMessage("Adding item to exclusion list...")
                    ListView1.Items.Add(Scintilla1.Lines(TextLine.Index + nextLine).Text)
                End While
            ElseIf TextLine.Text.Contains("[ExclusionException]") Then
                While Not Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[CompressionExclusionList]") Or Not Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[ExclusionList]")
                    If (TextLine.Index + 1) + nextLine >= Scintilla1.Lines.Count Then Exit While
                    If Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[CompressionExclusionList]") Or Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[ExclusionList]") Then Exit While
                    nextLine += 1
                    If String.IsNullOrWhiteSpace(Scintilla1.Lines(TextLine.Index + nextLine).Text) Then Continue While
                    DynaLog.LogMessage("Adding item to exclusion exception list...")
                    ListView2.Items.Add(Scintilla1.Lines(TextLine.Index + nextLine).Text)
                End While
            ElseIf TextLine.Text.Contains("[CompressionExclusionList]") Then
                While Not Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[ExclusionList]") Or Not Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[ExclusionException]")
                    If (TextLine.Index + 1) + nextLine >= Scintilla1.Lines.Count Then Exit While
                    If Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[ExclusionList]") Or Scintilla1.Lines(TextLine.Index + nextLine).Text.Contains("[ExclusionException]") Then Exit While
                    nextLine += 1
                    If String.IsNullOrWhiteSpace(Scintilla1.Lines(TextLine.Index + nextLine).Text) Then Continue While
                    DynaLog.LogMessage("Adding item to compression exclusion list...")
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
                DynaLog.LogMessage("This file does not have pending modifications.")
                titleMsg = LocalizationService.ForSection("WimScriptEditor.Editor").Format("ConfigList.Title", Path.GetFileName(ConfigListFile))
            Else
                DynaLog.LogMessage("This file has pending modifications.")
                titleMsg = LocalizationService.ForSection("WimScriptEditor.Editor").Format("ConfigList.ModifiedTitle", Path.GetFileName(ConfigListFile))
            End If
            Text = titleMsg
        End If
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        Dim msg As String = ""
        Dim titleMsg As String = ""
        msg = LocalizationService.ForSection("WimScriptEditor.Actions")("Save.Config.List.Prompt")
        titleMsg = LocalizationService.ForSection("WimScriptEditor.Actions").Format("ConfigList.Title", If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), ""))
        If (ConfigListFile Is Nothing Or Not File.Exists(ConfigListFile)) And Scintilla1.Text <> "" Then
            DynaLog.LogMessage("Asking user whether or not to save the file...")
            Dim Result As MsgBoxResult = MsgBox(msg, vbYesNoCancel + vbQuestion, Text)
            Select Case Result
                Case MsgBoxResult.Yes
                    If File.Exists(ConfigListFile) Then
                        File.WriteAllText(ConfigListFile, Scintilla1.Text, ASCII)
                        titleMsg = LocalizationService.ForSection("WimScriptEditor.Actions").Format("ConfigList.Title", Path.GetFileName(ConfigListFile))
                        Text = titleMsg
                    Else
                        If WimScriptSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                            ConfigListFile = WimScriptSFD.FileName
                            titleMsg = LocalizationService.ForSection("WimScriptEditor.Actions").Format("ConfigList.Title", Path.GetFileName(ConfigListFile))
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
                    DynaLog.LogMessage("Asking user whether or not to save modifications...")
                    Dim Result As MsgBoxResult = MsgBox(msg, vbYesNoCancel + vbQuestion, Text)
                    Select Case Result
                        Case MsgBoxResult.Yes
                            If File.Exists(ConfigListFile) Then
                                File.WriteAllText(ConfigListFile, Scintilla1.Text, ASCII)
                                titleMsg = LocalizationService.ForSection("WimScriptEditor.Actions").Format("ConfigList.Title", Path.GetFileName(ConfigListFile))
                                Text = titleMsg
                            Else
                                If WimScriptSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                                    File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                                    ConfigListFile = WimScriptSFD.FileName
                                    titleMsg = LocalizationService.ForSection("WimScriptEditor.Actions").Format("ConfigList.Title", Path.GetFileName(ConfigListFile))
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

        Text = LocalizationService.ForSection("WimScriptEditor")("New.Config.List.Label")

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
        msg = LocalizationService.ForSection("WimScriptEditor.Actions")("Save.Config.List.Prompt")
        titleMsg = LocalizationService.ForSection("WimScriptEditor.Actions").Format("ConfigList.FileTitle", If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), ""), Path.GetFileName(ConfigListFile))
        If (ConfigListFile Is Nothing Or Not File.Exists(ConfigListFile)) And Scintilla1.Text <> "" Then
            DynaLog.LogMessage("Asking user whether or not to save the file...")
            Dim Result As MsgBoxResult = MsgBox(msg, vbYesNoCancel + vbQuestion, Text)
            Select Case Result
                Case MsgBoxResult.Yes
                    If File.Exists(ConfigListFile) Then
                        File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                        ConfigListFile = WimScriptSFD.FileName
                        titleMsg = LocalizationService.ForSection("WimScriptEditor.Actions").Format("ConfigList.Title", Path.GetFileName(ConfigListFile))
                        Text = titleMsg
                    Else
                        If WimScriptSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                            ConfigListFile = WimScriptSFD.FileName
                            titleMsg = LocalizationService.ForSection("WimScriptEditor.Actions").Format("ConfigList.Title", Path.GetFileName(ConfigListFile))
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
                    DynaLog.LogMessage("Asking user whether or not to save modifications...")
                    Dim Result As MsgBoxResult = MsgBox(msg, vbYesNoCancel + vbQuestion, Text)
                    Select Case Result
                        Case MsgBoxResult.Yes
                            If File.Exists(ConfigListFile) Then
                                File.WriteAllText(ConfigListFile, Scintilla1.Text, ASCII)
                                titleMsg = LocalizationService.ForSection("WimScriptEditor.Actions").Format("ConfigList.Title", Path.GetFileName(ConfigListFile))
                                Text = titleMsg
                            Else
                                If WimScriptSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                                    File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                                    ConfigListFile = WimScriptSFD.FileName
                                    titleMsg = LocalizationService.ForSection("WimScriptEditor.Actions").Format("ConfigList.Title", Path.GetFileName(ConfigListFile))
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
        WimScriptOFD.ShowDialog(Me)
    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        If ConfigListFile Is Nothing Or Not File.Exists(ConfigListFile) Then
            If WimScriptSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                DynaLog.LogMessage("Saving contents to file...")
                DynaLog.LogMessage("Destination file: " & Quote & WimScriptSFD.FileName & Quote)
                File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                ConfigListFile = WimScriptSFD.FileName
            End If
        Else
            DynaLog.LogMessage("Saving contents to file...")
            DynaLog.LogMessage("Destination file: " & Quote & ConfigListFile & Quote)
            File.WriteAllText(ConfigListFile, Scintilla1.Text, ASCII)
        End If
        Text = LocalizationService.ForSection("WimScriptEditor").Format("ConfigList.FileTitle", Path.GetFileName(ConfigListFile))
    End Sub

    Private Sub WimScriptOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles WimScriptOFD.FileOk
        DynaLog.LogMessage("Loading file contents...")
        DynaLog.LogMessage("Configuration list file: " & Quote & WimScriptOFD.FileName & Quote)
        Scintilla1.Text = File.ReadAllText(WimScriptOFD.FileName)
        ConfigListFile = WimScriptOFD.FileName
        Text = LocalizationService.ForSection("WimScriptEditor.OpenFile").Format("ConfigList.Title", Path.GetFileName(ConfigListFile))
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
            DynaLog.LogMessage("Adding exclusion list items...")
            Scintilla1.AppendText(CrLf & _
                                  "[ExclusionList]" & CrLf)
            For Each LVI As ListViewItem In ListView1.Items
                Scintilla1.AppendText(LVI.Text & If(Not LVI.Text.EndsWith(CrLf), CrLf, ""))
            Next
            ' End with carriage return line feed
            Scintilla1.AppendText(CrLf)
        End If
        If ListView2.Items.Count > 0 Then
            DynaLog.LogMessage("Adding exclusion exception list items...")
            Scintilla1.AppendText(CrLf & _
                                  "[ExclusionException]" & CrLf)
            For Each LVI As ListViewItem In ListView2.Items
                Scintilla1.AppendText(LVI.Text & If(Not LVI.Text.EndsWith(CrLf), CrLf, ""))
            Next
            ' End with carriage return line feed
            Scintilla1.AppendText(CrLf)
        End If
        If ListView3.Items.Count > 0 Then
            DynaLog.LogMessage("Adding compression exclusion list items...")
            Scintilla1.AppendText(CrLf & _
                                  "[CompressionExclusionList]" & CrLf)
            For Each LVI As ListViewItem In ListView3.Items
                Scintilla1.AppendText(LVI.Text & If(Not LVI.Text.EndsWith(CrLf), CrLf, ""))
            Next
            ' End with carriage return line feed
            Scintilla1.AppendText(CrLf)
        End If

        ' Indicate whether file has seen changes, if it exists
        If ConfigListFile IsNot Nothing And File.Exists(ConfigListFile) Then
            If File.ReadAllText(ConfigListFile).ToString() = Scintilla1.Text Then
                Text = LocalizationService.ForSection("WimScriptEditor.Content").Format("ConfigList.Title", Path.GetFileName(ConfigListFile))
            Else
                Text = LocalizationService.ForSection("WimScriptEditor.Content").Format("ConfigList.ModifiedTitle", Path.GetFileName(ConfigListFile))
            End If
        End If

        ' Add TextChanged event handler to let the user type files in the Scintilla editor again
        AddHandler Scintilla1.TextChanged, AddressOf Scintilla1_TextChanged
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AddListEntryDlg.IsForExclusionList = True
        AddListEntryDlg.Text = LocalizationService.ForSection("WimScriptEditor").Format("AddList.Label", GroupBox1.Text.ToLower())
        AddListEntryDlg.Left = Left + ((SplitContainer1.SplitterDistance + Scintilla1.Width) / 2)
        AddListEntryDlg.Top = Top + Panel2.Top + DarkToolStrip1.Height + SplitContainer1.Top + GroupBox1.Top + 8
        AddListEntryDlg.ShowDialog(Me)
        If AddListEntryDlg.DialogResult = Windows.Forms.DialogResult.OK Then
            ListView1.Items.Add(AddListEntryDlg.TextBox1.Text)
            UpdateConfigListContents()
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        AddListEntryDlg.IsForExclusionList = False
        AddListEntryDlg.Text = LocalizationService.ForSection("WimScriptEditor").Format("AddEntry.Label", GroupBox2.Text.ToLower())
        AddListEntryDlg.Left = Left + ((SplitContainer1.SplitterDistance + Scintilla1.Width) / 2)
        AddListEntryDlg.Top = Top + Panel2.Top + DarkToolStrip1.Height + SplitContainer1.Top + GroupBox2.Top + 8
        AddListEntryDlg.ShowDialog(Me)
        If AddListEntryDlg.DialogResult = Windows.Forms.DialogResult.OK Then
            ListView2.Items.Add(AddListEntryDlg.TextBox1.Text)
            UpdateConfigListContents()
        End If
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button9.Click
        AddListEntryDlg.IsForExclusionList = False
        AddListEntryDlg.Text = LocalizationService.ForSection("WimScriptEditor").Format("AddEntry.Label", GroupBox3.Text.ToLower())
        AddListEntryDlg.Left = Left + ((SplitContainer1.SplitterDistance + Scintilla1.Width) / 2)
        AddListEntryDlg.Top = Top + Panel2.Top + DarkToolStrip1.Height + SplitContainer1.Top + GroupBox3.Top + 8
        AddListEntryDlg.ShowDialog(Me)
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
        msg = LocalizationService.ForSection("WimScriptEditor.Close")("Save.Config.List.Prompt")
        titleMsg = LocalizationService.ForSection("WimScriptEditor.Close").Format("ConfigList.FileTitle", If((ConfigListFile IsNot Nothing And File.Exists(ConfigListFile)), Path.GetFileName(ConfigListFile), ""), Path.GetFileName(ConfigListFile))
        If (ConfigListFile Is Nothing Or Not File.Exists(ConfigListFile)) And Scintilla1.Text <> "" Then
            DynaLog.LogMessage("Asking user whether or not to save the file...")
            Dim Result As MsgBoxResult = MsgBox(msg, vbYesNoCancel + vbQuestion, Text)
            Select Case Result
                Case MsgBoxResult.Yes
                    If File.Exists(ConfigListFile) Then
                        File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                        ConfigListFile = WimScriptSFD.FileName
                        titleMsg = LocalizationService.ForSection("WimScriptEditor.Close").Format("ConfigList.Title", Path.GetFileName(ConfigListFile))
                        Text = titleMsg
                    Else
                        If WimScriptSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                            File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                            ConfigListFile = WimScriptSFD.FileName
                            titleMsg = LocalizationService.ForSection("WimScriptEditor.Close").Format("ConfigList.Title", Path.GetFileName(ConfigListFile))
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
                    DynaLog.LogMessage("Asking user whether or not to save modifications...")
                    Dim Result As MsgBoxResult = MsgBox(msg, vbYesNoCancel + vbQuestion, Text)
                    Select Case Result
                        Case MsgBoxResult.Yes
                            If File.Exists(ConfigListFile) Then
                                File.WriteAllText(ConfigListFile, Scintilla1.Text, ASCII)
                                titleMsg = LocalizationService.ForSection("WimScriptEditor.Close").Format("ConfigList.Title", Path.GetFileName(ConfigListFile))
                                Text = titleMsg
                            Else
                                If WimScriptSFD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                                    File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                                    ConfigListFile = WimScriptSFD.FileName
                                    titleMsg = LocalizationService.ForSection("WimScriptEditor.Close").Format("ConfigList.Title", Path.GetFileName(ConfigListFile))
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
            Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
            WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
            ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        End If
    End Sub

    Private Sub NoOneDriveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NoOneDriveToolStripMenuItem.Click
        If OneDriveExclusionDlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            If OneDriveExclusionDlg.ExcludedFolders.Count > 0 Then
                For Each ExcludedFolder In OneDriveExclusionDlg.ExcludedFolders
                    ListView1.Items.Add(If(ExcludedFolder.Contains(" "), Quote & ExcludedFolder & Quote, ExcludedFolder))
                    DynaLog.LogMessage("Adding OneDrive item to exclusion...")
                    UpdateConfigListContents()
                Next
            End If
        End If
    End Sub
End Class
