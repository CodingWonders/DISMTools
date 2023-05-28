Imports ScintillaNET
Public Class Actions_MainForm

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
            Scintilla1.Styles(Style.Vb.Default).ForeColor = Color.FromArgb(220, 220, 204)
            Scintilla1.Styles(Style.Vb.Comment).ForeColor = Color.FromArgb(127, 159, 127)
            Scintilla1.Styles(Style.Vb.Number).ForeColor = Color.FromArgb(140, 208, 211)
            Scintilla1.Styles(Style.Vb.DocKeyword).ForeColor = Color.FromArgb(206, 223, 153)
            Scintilla1.Styles(Style.Vb.String).ForeColor = Color.FromArgb(204, 147, 147)
            Scintilla1.Styles(Style.Vb.Preprocessor).ForeColor = Color.FromArgb(255, 207, 175)
            Scintilla1.Styles(Style.Vb.Operator).ForeColor = Color.FromArgb(159, 157, 109)
            Scintilla1.Styles(Style.Vb.Date).ForeColor = Color.FromArgb(223, 196, 125)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Scintilla1.Styles(Style.Vb.Default).ForeColor = ForeColor
            Scintilla1.Styles(Style.Vb.Comment).ForeColor = Color.FromArgb(0, 128, 0)
            Scintilla1.Styles(Style.Vb.Number).ForeColor = Color.FromArgb(255, 0, 0)
            Scintilla1.Styles(Style.Vb.DocKeyword).ForeColor = Color.FromArgb(0, 0, 255)
            Scintilla1.Styles(Style.Vb.String).ForeColor = Color.FromArgb(128, 128, 128)
            Scintilla1.Styles(Style.Vb.Preprocessor).ForeColor = Color.FromArgb(255, 0, 0)
            Scintilla1.Styles(Style.Vb.Operator).ForeColor = ForeColor
            Scintilla1.Styles(Style.Vb.Date).ForeColor = Color.FromArgb(0, 255, 0)
        End If
        ' Set lexer
        Scintilla1.Lexer = Lexer.Vb

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

    Private Sub Actions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MenuStrip1.Renderer = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), New ToolStripProfessionalRenderer(New MainForm.DarkModeColorTable()), New ToolStripProfessionalRenderer(New MainForm.LightModeColorTable()))
        MenuStrip1.BackColor = MainForm.BackColor
        MenuStrip1.ForeColor = MainForm.ForeColor
        BackColor = MainForm.BackColor
        ForeColor = MainForm.ForeColor
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            For Each item As ToolStripDropDownItem In MenuStrip1.Items
                item.DropDown.BackColor = Color.FromArgb(27, 27, 28)
                item.DropDown.ForeColor = Color.White
                Try
                    For Each dropDownItem As ToolStripDropDownItem In item.DropDownItems
                        dropDownItem.DropDown.BackColor = Color.FromArgb(27, 27, 28)
                        dropDownItem.DropDown.ForeColor = Color.White
                    Next
                Catch ex As Exception
                    Continue For
                End Try
            Next
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            For Each item As ToolStripDropDownItem In MenuStrip1.Items
                item.DropDown.BackColor = Color.FromArgb(231, 232, 236)
                item.DropDown.ForeColor = Color.Black
                Try
                    For Each dropDownItem As ToolStripDropDownItem In item.DropDownItems
                        dropDownItem.DropDown.BackColor = Color.FromArgb(231, 232, 236)
                        dropDownItem.DropDown.ForeColor = Color.Black
                    Next
                Catch ex As Exception
                    Continue For
                End Try
            Next
        End If
        StatusStrip.BackColor = MainForm.StatusStrip.BackColor
        InitScintilla("Courier New", 10)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(Handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))
    End Sub
End Class