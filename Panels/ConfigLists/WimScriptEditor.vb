Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports ScintillaNET
Imports System.Text.Encoding

Public Class WimScriptEditor

    Dim ConfigListFile As String

    Private Sub WimScriptEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
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
            If File.ReadAllText(ConfigListFile).ToString() = Scintilla1.Text Then
                Text = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
            Else
                Text = Path.GetFileName(ConfigListFile) & " (modified) - DISM Configuration List Editor"
            End If
        End If
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        If (ConfigListFile Is Nothing Or Not File.Exists(ConfigListFile)) And Scintilla1.Text <> "" Then
            Dim Result As MsgBoxResult = MsgBox("Do you want to save this configuration list file?", vbYesNoCancel + vbQuestion, Text)
            Select Case Result
                Case MsgBoxResult.Yes
                    If File.Exists(ConfigListFile) Then
                        File.WriteAllText(ConfigListFile, Scintilla1.Text, ASCII)
                        Text = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                    Else
                        If WimScriptSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                            ConfigListFile = WimScriptSFD.FileName
                            Text = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
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

        Text = "New configuration list - DISM Configuration List Editor"

        ' Generate a default configuration list, as shown in the DISM configuration list documentation.
        ' Source: https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism-configuration-list-and-wimscriptini-files-winnext?view=windows-11

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
        If (ConfigListFile Is Nothing Or Not File.Exists(ConfigListFile)) And Scintilla1.Text <> "" Then
            Dim Result As MsgBoxResult = MsgBox("Do you want to save this configuration list file?", vbYesNoCancel + vbQuestion, Text)
            Select Case Result
                Case MsgBoxResult.Yes
                    If File.Exists(ConfigListFile) Then
                        File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                        ConfigListFile = WimScriptSFD.FileName
                        Text = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                    Else
                        If WimScriptSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
                            File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                            ConfigListFile = WimScriptSFD.FileName
                            Text = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
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
                    Dim Result As MsgBoxResult = MsgBox("Do you want to save this configuration list file?", vbYesNoCancel + vbQuestion, Text)
                    Select Case Result
                        Case MsgBoxResult.Yes
                            If File.Exists(ConfigListFile) Then
                                File.WriteAllText(ConfigListFile, Scintilla1.Text, ASCII)
                                Text = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
                            Else
                                If WimScriptSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
                                    File.WriteAllText(WimScriptSFD.FileName, Scintilla1.Text, ASCII)
                                    ConfigListFile = WimScriptSFD.FileName
                                    Text = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
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
        Text = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
    End Sub

    Private Sub WimScriptOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles WimScriptOFD.FileOk
        Scintilla1.Text = File.ReadAllText(WimScriptOFD.FileName)
        ConfigListFile = WimScriptOFD.FileName
        Text = Path.GetFileName(ConfigListFile) & " - DISM Configuration List Editor"
    End Sub

    Private Sub ToolStripButton6_Click(sender As Object, e As EventArgs) Handles ToolStripButton6.Click
        Process.Start("https://learn.microsoft.com/en-us/windows-hardware/manufacture/desktop/dism-configuration-list-and-wimscriptini-files-winnext?view=windows-11")
    End Sub
End Class