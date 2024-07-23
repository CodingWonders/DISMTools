Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports System.Threading
Imports ScintillaNET

Public Class NewUnattendWiz

    ' Declare initial vars
    Dim IsInExpress As Boolean = True
    Dim ExpressPage As Integer = 1
    Dim OSWiki As String
    Dim ColoredMode As Integer = 0      ' 0: Windows 8; 1: Windows 8.1
    Dim IsWindows8 As Boolean
    Dim KeyString As String
    Dim CanGatherImgVersion As Boolean
    Dim WinVersion As String
    Dim SkipEula As Boolean
    Dim ComputerName As String
    Dim OrgName As String
    Dim SetupLang As Integer
    Dim WipeDisk As Boolean
    Dim TargetDiskNum As Integer
    Dim UseNtfs As Boolean
    Dim MakeActive As Boolean
    Dim TargetDrLetter As String
    Dim TargetDrLabel As String
    Dim UseMbr As Boolean
    Dim PartOrder As Integer
    Dim InputMethod As Integer
    Dim Currency As Integer
    Dim TZData As Integer
    Dim UILang As Integer
    Dim HideEula As Boolean
    Dim SkipMachineOobe As Boolean
    Dim SkipUserOobe As Boolean
    Dim NetLocation As Integer
    Dim HideWirelessChooser As Boolean
    Dim ProtectionLevel As Integer
    Dim WULevel As Integer
    Dim IsInCategoryView As Boolean
    Dim IconSize As Integer
    Dim UserName As String
    Dim UserDesc As String
    Dim ColorMode As Integer
    Dim UserPass As String
    Dim UserGroup As String
    Dim ShowHiddenGroups As Boolean
    Dim EnableUAC As Boolean
    Dim EnableFirewall As Boolean
    Dim Autologin As Boolean
    Dim PasswordExpiry As Boolean
    Dim JoinCeip As Boolean

    ''' <summary>
    ''' Initializes the Scintilla editor
    ''' </summary>
    ''' <param name="fntName">The name of the font used in the Scintilla editor</param>
    ''' <param name="fntSize">The size of the font used in the Scintilla editor</param>
    ''' <remarks></remarks>
    Sub InitScintilla(fntName As String, fntSize As Integer)
        ' Initialize Scintilla editor
        Scintilla1.StyleResetDefault()
        ' Use VS's selection color, as I find it the most natural
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Scintilla1.SelectionBackColor = Color.FromArgb(38, 79, 120)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Scintilla1.SelectionBackColor = Color.FromArgb(153, 201, 239)
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
            Scintilla1.Styles(Style.Xml.XmlStart).ForeColor = Color.FromArgb(127, 159, 127)
            Scintilla1.Styles(Style.Xml.XmlEnd).ForeColor = Color.FromArgb(127, 159, 127)
            Scintilla1.Styles(Style.Xml.Default).ForeColor = Color.FromArgb(220, 220, 204)
            Scintilla1.Styles(Style.Xml.Comment).ForeColor = Color.FromArgb(127, 159, 127)
            Scintilla1.Styles(Style.Xml.Number).ForeColor = Color.FromArgb(140, 208, 211)
            Scintilla1.Styles(Style.Xml.DoubleString).ForeColor = Color.FromArgb(200, 145, 145)
            Scintilla1.Styles(Style.Xml.SingleString).ForeColor = Color.FromArgb(200, 145, 145)
            Scintilla1.Styles(Style.Xml.Tag).ForeColor = Color.FromArgb(227, 206, 171)
            Scintilla1.Styles(Style.Xml.TagEnd).ForeColor = Color.FromArgb(227, 206, 171)
            Scintilla1.Styles(Style.Xml.TagUnknown).ForeColor = Color.FromArgb(237, 214, 237)
            Scintilla1.Styles(Style.Xml.Attribute).ForeColor = Color.FromArgb(190, 200, 158)
            Scintilla1.Styles(Style.Xml.AttributeUnknown).ForeColor = Color.FromArgb(223, 223, 223)
            Scintilla1.Styles(Style.Xml.CData).ForeColor = Color.FromArgb(200, 145, 145)
            Scintilla1.Styles(Style.Xml.Entity).ForeColor = Color.FromArgb(207, 191, 175)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Scintilla1.Styles(Style.Xml.XmlStart).ForeColor = Color.Red
            Scintilla1.Styles(Style.Xml.XmlEnd).ForeColor = Color.Red
            Scintilla1.Styles(Style.Xml.Default).ForeColor = Color.Black
            Scintilla1.Styles(Style.Xml.Comment).ForeColor = Color.FromArgb(0, 128, 0)
            Scintilla1.Styles(Style.Xml.Number).ForeColor = Color.Red
            Scintilla1.Styles(Style.Xml.DoubleString).ForeColor = Color.FromArgb(128, 0, 255)
            Scintilla1.Styles(Style.Xml.SingleString).ForeColor = Color.FromArgb(128, 0, 255)
            Scintilla1.Styles(Style.Xml.Tag).ForeColor = Color.Blue
            Scintilla1.Styles(Style.Xml.TagEnd).ForeColor = Color.Blue
            Scintilla1.Styles(Style.Xml.TagUnknown).ForeColor = Color.Blue
            Scintilla1.Styles(Style.Xml.Attribute).ForeColor = Color.Red
            Scintilla1.Styles(Style.Xml.AttributeUnknown).ForeColor = Color.Red
            Scintilla1.Styles(Style.Xml.CData).ForeColor = Color.FromArgb(255, 128, 0)
            Scintilla1.Styles(Style.Xml.Entity).ForeColor = Color.Black
        End If
        ' Set lexer
        Scintilla1.LexerName = "xml"

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

    Private Sub NewUnattendWiz_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            StepsTreeView.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            StepsTreeView.BackColor = Color.FromArgb(238, 238, 242)
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))

        SidePanel.BackColor = BackColor
        StepsTreeView.ForeColor = ForeColor
        PictureBox2.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.editor_mode_select, My.Resources.editor_mode)
        ' Fill in font combinations
        FontFamilyTSCB.Items.Clear()
        For Each fntFamily As FontFamily In FontFamily.Families
            FontFamilyTSCB.Items.Add(fntFamily.Name)
        Next
        InitScintilla("Consolas", 11)
        StepsTreeView.ExpandAll()

        FontFamilyTSCB.SelectedItem = "Consolas"
        SetNodeColors(StepsTreeView.Nodes, BackColor, ForeColor)

        ' Go to schneegans generator for now
        Process.Start("https://schneegans.de/windows/unattend-generator/")
        If Not MainForm.EnableExperiments Then
            MessageBox.Show("The unattended answer file tasks are undergoing a major reconstruction." & If(MainForm.dtBranch.Contains("stable"), CrLf & CrLf & "The reconstruction is only happening in the preview releases.", ""))
        End If
    End Sub

    Private Sub ExpressPanelTrigger_MouseEnter(sender As Object, e As EventArgs) Handles ExpressPanelTrigger.MouseEnter
        If ExpressPanelContainer.Visible Then
            ExpressPanelTrigger.BackColor = Color.FromKnownColor(KnownColor.HotTrack)
        Else
            ExpressPanelTrigger.BackColor = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), Color.FromArgb(48, 48, 48), Color.Gainsboro)
        End If
    End Sub

    Private Sub ExpressPanelTrigger_MouseLeave(sender As Object, e As EventArgs) Handles ExpressPanelTrigger.MouseLeave
        If ExpressPanelContainer.Visible Then
            ExpressPanelTrigger.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        Else
            ExpressPanelTrigger.BackColor = SidePanel.BackColor
        End If
    End Sub

    Private Sub ExpressPanelTrigger_MouseDown(sender As Object, e As MouseEventArgs) Handles ExpressPanelTrigger.MouseDown
        If ExpressPanelContainer.Visible Then
            ExpressPanelTrigger.BackColor = Color.SteelBlue
        Else
            ExpressPanelTrigger.BackColor = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), Color.FromArgb(36, 36, 36), Color.Silver)
        End If
    End Sub

    Private Sub ExpressPanelTrigger_MouseUp(sender As Object, e As MouseEventArgs) Handles ExpressPanelTrigger.MouseUp
        If ExpressPanelContainer.Visible Then
            ExpressPanelTrigger.BackColor = Color.FromKnownColor(KnownColor.HotTrack)
        Else
            ExpressPanelTrigger.BackColor = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), Color.FromArgb(48, 48, 48), Color.Gainsboro)
        End If
    End Sub

    Private Sub ExpressPanelTrigger_Click(sender As Object, e As EventArgs) Handles ExpressPanelTrigger.Click
        IsInExpress = True
        StepsTreeView.Enabled = True
        EditorPanelContainer.Visible = False
        ExpressPanelContainer.Visible = True
        ExpressPanelTrigger.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        ExpressPanelTrigger.ForeColor = Color.White
        PictureBox1.Image = My.Resources.express_mode_select
        EditorPanelTrigger.BackColor = SidePanel.BackColor
        EditorPanelTrigger.ForeColor = Color.LightGray
        PictureBox2.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.editor_mode_select, My.Resources.editor_mode)
        PictureBox3.Image = My.Resources.express_mode_fc
        Label3.Text = "Express mode"
        Label4.Text = "If you haven't created unattended answer files before, use this wizard to create one"
    End Sub

    Private Sub EditorPanelTrigger_MouseEnter(sender As Object, e As EventArgs) Handles EditorPanelTrigger.MouseEnter
        If EditorPanelContainer.Visible Then
            EditorPanelTrigger.BackColor = Color.FromKnownColor(KnownColor.HotTrack)
        Else
            EditorPanelTrigger.BackColor = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), Color.FromArgb(48, 48, 48), Color.Gainsboro)
        End If
    End Sub

    Private Sub EditorPanelTrigger_MouseLeave(sender As Object, e As EventArgs) Handles EditorPanelTrigger.MouseLeave
        If EditorPanelContainer.Visible Then
            EditorPanelTrigger.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        Else
            EditorPanelTrigger.BackColor = SidePanel.BackColor
        End If
    End Sub

    Private Sub EditorPanelTrigger_MouseDown(sender As Object, e As MouseEventArgs) Handles EditorPanelTrigger.MouseDown
        If EditorPanelContainer.Visible Then
            EditorPanelTrigger.BackColor = Color.SteelBlue
        Else
            EditorPanelTrigger.BackColor = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), Color.FromArgb(36, 36, 36), Color.Silver)
        End If
    End Sub

    Private Sub EditorPanelTrigger_MouseUp(sender As Object, e As MouseEventArgs) Handles EditorPanelTrigger.MouseUp
        If EditorPanelContainer.Visible Then
            EditorPanelTrigger.BackColor = Color.FromKnownColor(KnownColor.HotTrack)
        Else
            EditorPanelTrigger.BackColor = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), Color.FromArgb(48, 48, 48), Color.Gainsboro)
        End If
    End Sub

    Private Sub EditorPanelTrigger_Click(sender As Object, e As EventArgs) Handles EditorPanelTrigger.Click
        IsInExpress = False
        StepsTreeView.Enabled = False
        EditorPanelContainer.Visible = True
        ExpressPanelContainer.Visible = False
        ExpressPanelTrigger.BackColor = SidePanel.BackColor
        ExpressPanelTrigger.ForeColor = Color.LightGray
        PictureBox1.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.express_mode_select, My.Resources.express_mode)
        EditorPanelTrigger.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        EditorPanelTrigger.ForeColor = Color.White
        PictureBox2.Image = My.Resources.editor_mode_select
        PictureBox3.Image = My.Resources.editor_mode_fc
        Label3.Text = "Editor mode"
        Label4.Text = "Create your unattended answer files from scratch and save them anywhere"
    End Sub

    Private Sub Back_Button_Click(sender As Object, e As EventArgs) Handles Back_Button.Click

    End Sub

    Private Sub Next_Button_Click(sender As Object, e As EventArgs) Handles Next_Button.Click

    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Close()
    End Sub

    Private Sub FontChange(sender As Object, e As EventArgs) Handles FontFamilyTSCB.SelectedIndexChanged, FontSizeTSCB.SelectedIndexChanged
        ' Change Scintilla editor font
        InitScintilla(FontFamilyTSCB.SelectedItem, FontSizeTSCB.SelectedItem)
    End Sub

    Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click
        If ToolStripButton5.Checked Then
            ToolStripButton5.Checked = False
        Else
            ToolStripButton5.Checked = True
        End If
        Scintilla1.WrapMode = If(ToolStripButton5.Checked, WrapMode.Word, WrapMode.None)
    End Sub

    Sub SetNodeColors(nodes As TreeNodeCollection, bg As Color, fg As Color)
        For Each node As TreeNode In nodes
            node.BackColor = BackColor
            node.ForeColor = ForeColor
            SetNodeColors(node.Nodes, BackColor, ForeColor)
        Next
    End Sub
End Class