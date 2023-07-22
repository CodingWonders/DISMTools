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
        Scintilla1.Lexer = Lexer.Xml

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
        GroupBox1.ForeColor = ForeColor
        GroupBox2.ForeColor = ForeColor
        GroupBox3.ForeColor = ForeColor
        GroupBox4.ForeColor = ForeColor
        GroupBox5.ForeColor = ForeColor
        GroupBox6.ForeColor = ForeColor
        SidePanel.BackColor = BackColor
        StepsTreeView.ForeColor = ForeColor
        PictureBox2.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.editor_mode_select, My.Resources.editor_mode)
        ' Fill in font combinations
        FontFamilyTSCB.Items.Clear()
        For Each fntFamily As FontFamily In FontFamily.Families
            FontFamilyTSCB.Items.Add(fntFamily.Name)
        Next
        InitScintilla("Courier New", 10)
        StepsTreeView.ExpandAll()

        FontFamilyTSCB.SelectedItem = "Courier New"
        SetNodeColors(StepsTreeView.Nodes, BackColor, ForeColor)
    End Sub

    Private Sub StepsTreeView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles StepsTreeView.AfterSelect
        LocationLbl.Text = "You are currently at: " & StepsTreeView.SelectedNode.Text
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
        LocationPanel.Visible = True
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
        LocationPanel.Visible = False
        PictureBox3.Image = My.Resources.editor_mode_fc
        Label3.Text = "Editor mode"
        Label4.Text = "Create your unattended answer files from scratch and save them anywhere"
    End Sub

    Private Sub Back_Button_Click(sender As Object, e As EventArgs) Handles Back_Button.Click
        ExpressPage -= 1
        If ExpressPage <= 1 Then
            Back_Button.Enabled = False
        Else
            Back_Button.Enabled = True
        End If
        Try
            If IncompleteWarningPanel.Visible Then
                Exit Try
            End If
            StepsTreeView.SelectedNode = StepsTreeView.SelectedNode.PrevVisibleNode
        Catch ex As Exception
            StepsTreeView.SelectedNode = StepsTreeView.SelectedNode.FirstNode
        End Try
        ChangePage()
    End Sub

    Private Sub Next_Button_Click(sender As Object, e As EventArgs) Handles Next_Button.Click
        If ExpressPage = 1 Then
            If RadioButton1.Checked Then
                If CanGatherImgVersion Then
                    Next_Button.Enabled = True
                Else
                    Next_Button.Enabled = False
                End If
            End If
        ElseIf ExpressPage = 2 Then
            TailorWizardToTargetOS()
            If RadioButton3.Checked Then
                If ComboBox2.SelectedItem = "" Then
                    Next_Button.Enabled = False
                Else
                    Next_Button.Enabled = True
                End If
            Else
                KeyString = KeyInputBox1.Text & "-" & KeyInputBox2.Text & "-" & KeyInputBox3.Text & "-" & KeyInputBox4.Text & "-" & KeyInputBox5.Text
                If KeyString.Length < 29 Then
                    Next_Button.Enabled = False
                Else
                    Next_Button.Enabled = True
                End If
            End If
        ElseIf ExpressPage = 3 Then
            If RadioButton3.Checked Then
                KeyString = GenericBox1.Text & "-" & GenericBox2.Text & "-" & GenericBox3.Text & "-" & GenericBox4.Text & "-" & GenericBox5.Text
            Else
                KeyString = KeyInputBox1.Text & "-" & KeyInputBox2.Text & "-" & KeyInputBox3.Text & "-" & KeyInputBox4.Text & "-" & KeyInputBox5.Text
            End If
        ElseIf ExpressPage = 4 Then
            If TextBox1.Text = "" Then
                Next_Button.Enabled = False
            Else
                If TextBox1.Text.Contains(".") Or _
                    TextBox1.Text.Contains("\") Or _
                    TextBox1.Text.Contains("/") Or _
                    TextBox1.Text.Contains(":") Or _
                    TextBox1.Text.Contains("*") Or _
                    TextBox1.Text.Contains("?") Or _
                    TextBox1.Text.Contains(Quote) Or _
                    TextBox1.Text.Contains("<") Or _
                    TextBox1.Text.Contains(">") Or _
                    TextBox1.Text.Contains("|") Or _
                    TextBox1.Text.Contains(",") Or _
                    TextBox1.Text.Contains("~") Or _
                    TextBox1.Text.Contains("!") Or _
                    TextBox1.Text.Contains("@") Or _
                    TextBox1.Text.Contains("#") Or _
                    TextBox1.Text.Contains("$") Or _
                    TextBox1.Text.Contains("%") Or _
                    TextBox1.Text.Contains("^") Or _
                    TextBox1.Text.Contains("&") Or _
                    TextBox1.Text.Contains("`") Or _
                    TextBox1.Text.Contains("(") Or TextBox1.Text.Contains(")") Or _
                    TextBox1.Text.Contains("{") Or TextBox1.Text.Contains("}") Or _
                    TextBox1.Text.Contains("_") Then
                    Next_Button.Enabled = False
                Else
                    Next_Button.Enabled = True
                End If
            End If
        ElseIf ExpressPage = 6 Then
            If ComboBox5.SelectedItem = "" Or ComboBox6.SelectedItem = "" Or ComboBox7.SelectedItem = "" Or ComboBox8.SelectedItem = "" Then
                Next_Button.Enabled = False
            Else
                Next_Button.Enabled = True
            End If
        ElseIf ExpressPage = 13 Then
            If TextBox4.Text = "" Then
                Next_Button.Enabled = False
            Else
                Next_Button.Enabled = True
            End If
        ElseIf ExpressPage = 14 Then
            UserNameLabel.Text = TextBox4.Text
        ElseIf ExpressPage = 15 Then
            If PasswordBox.Text = "" And PasswordRepeatBox.Text = "" Then
                If MsgBox("Continuing without a password might leave the unattended system more vulnerable. Do you still want to continue?", vbYesNo + vbExclamation, "Unattended answer file creation wizard") = MsgBoxResult.No Then
                    Exit Sub
                End If
            Else
                If Not PasswordRepeatBox.Text.Equals(PasswordBox.Text, StringComparison.Ordinal) Then
                    MsgBox("The passwords provided don't match. Make sure you typed the password correctly on both fields and try again", vbOKOnly + vbCritical, "Unattended answer file creation wizard")
                    Exit Sub
                End If
            End If
        End If
        ExpressPage += 1
        Back_Button.Enabled = True
        ' Add condition here
        If StepsTreeView.SelectedNode.Index < StepsTreeView.Nodes.Count - 1 Then
            StepsTreeView.SelectedNode = StepsTreeView.SelectedNode.NextVisibleNode
        End If
        ChangePage()
    End Sub

    Sub ChangePage()
        If IsInExpress Then
            Select Case ExpressPage
                Case 1
                    IncompleteWarningPanel.Visible = True
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                Case 2
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = True
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                    If Not StepsTreeView.SelectedNode.Text = "Target operating system selection" Then
                        StepsTreeView.SelectedNode = StepsTreeView.SelectedNode.PrevVisibleNode
                    End If
                Case 3
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = True
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                    If Not StepsTreeView.SelectedNode.Text = "Product activation" Then
                        StepsTreeView.SelectedNode = StepsTreeView.SelectedNode.NextVisibleNode
                    End If
                Case 4
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = True
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                Case 5
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = True
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                Case 6
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = True
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                Case 7
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = True
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                    If Not StepsTreeView.SelectedNode.Text = "Regional settings" Then
                        StepsTreeView.SelectedNode = StepsTreeView.SelectedNode.PrevVisibleNode
                    End If
                Case 8
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = True
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                    If Not StepsTreeView.SelectedNode.Text = "End-user License Agreement" Then
                        StepsTreeView.SelectedNode = StepsTreeView.SelectedNode.NextVisibleNode
                    End If
                Case 9
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = True
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                Case 10
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = True
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                Case 11
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = True
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                Case 12
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = True
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                Case 13
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = True
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                    If Not StepsTreeView.SelectedNode.Text = "Control Panel" Then
                        StepsTreeView.SelectedNode = StepsTreeView.SelectedNode.PrevVisibleNode
                    End If
                Case 14
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = True
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                    If Not StepsTreeView.SelectedNode.Text = "Account personalization" Then
                        StepsTreeView.SelectedNode = StepsTreeView.SelectedNode.NextVisibleNode
                    End If
                Case 15
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = True
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                Case 16
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = True
                    SettingRecapPanel.Visible = False
                    ' Add more panels as this becomes more complete
                Case 17
                    IncompleteWarningPanel.Visible = False
                    TargetOSSelectionPanel.Visible = False
                    ActivationPanel.Visible = False
                    EndUserLicenseAgreementPanel.Visible = False
                    CompPersonalizationPanel.Visible = False
                    DiskPartPanel.Visible = False
                    RegionalSettingsPanel.Visible = False
                    UserEulaPanel.Visible = False
                    OobeSkipsPanel.Visible = False
                    NetSecurityPanel.Visible = False
                    WirelessPanel.Visible = False
                    CompProtectPanel.Visible = False
                    ControlPanel.Visible = False
                    UsrPersonalizationPanel.Visible = False
                    UsrSecurityPanel.Visible = False
                    UsrCeipPanel.Visible = False
                    SettingRecapPanel.Visible = True
                    ' Add more panels as this becomes more complete
                Case 18

                Case 19

            End Select
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Next_Button.Enabled = True
        Else
            Next_Button.Enabled = False
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            Label7.Enabled = True
            Label8.Enabled = True
            Label9.Enabled = True
            Label10.Enabled = True
            Label11.Enabled = False
            ComboBox1.Enabled = False
            CheckBox2.Enabled = False
            GroupBox1.Enabled = False
            DetectTargetOS()
            If CanGatherImgVersion Then
                Next_Button.Enabled = True
            ElseIf CanGatherImgVersion = False Then
                Next_Button.Enabled = False
            End If
        Else
            Label7.Enabled = False
            Label8.Enabled = False
            Label9.Enabled = False
            Label10.Enabled = False
            Label11.Enabled = True
            ComboBox1.Enabled = True
            CheckBox2.Enabled = True
            GroupBox1.Enabled = True
            Next_Button.Enabled = True
        End If
    End Sub

    Sub DetectTargetOS()
        Try
            Dim KeVersionInfo As FileVersionInfo
            If File.Exists(MainForm.MountDir & "\Windows\system32\ntoskrnl.exe") Then
                KeVersionInfo = FileVersionInfo.GetVersionInfo(MainForm.MountDir & "\Windows\system32\ntoskrnl.exe")
            Else    ' Do Windows 2000 WIMs exist out there?
                KeVersionInfo = FileVersionInfo.GetVersionInfo(MainForm.MountDir & "\WINNT\System32\ntoskrnl.exe")
            End If
            Label9.Text = KeVersionInfo.ProductVersion
            If KeVersionInfo.ProductMajorPart = 5 Then          ' Windows 2000/XP/Server 2003/R2
                Select Case KeVersionInfo.ProductMinorPart
                    Case 0                                      ' Windows 2000
                        Label10.Text = "Windows 2000 - Unsupported system"
                    Case 1                                      ' Windows XP/XP IA-64_2002
                        If MainForm.imgMountedName.Contains("64-Bit Edition") Then
                            Label10.Text = "Windows XP (Intel(R) Itanium(TM)) - Unsupported system"
                        Else
                            Label10.Text = "Windows XP"
                        End If
                    Case 2                                      ' Windows Server 2003/R2/XP IA-64_2003/XP AMD-64
                        If MainForm.imgMountedName.Contains("Server") Then
                            Label10.Text = "Windows Server 2003 - Unsupported system"
                        Else
                            If MainForm.imgMountedName.Contains("64-Bit Edition") Then
                                Label10.Text = "Windows XP (Intel(R) Itanium(TM)) - Unsupported system"
                            ElseIf MainForm.imgMountedName.Contains("Professional x64 Edition") Then
                                Label10.Text = "Windows XP (AMD64) - Unsupported system"
                            End If
                        End If
                End Select
            ElseIf KeVersionInfo.ProductMajorPart = 6 Then      ' Windows Vista/7/8.x/10TP/Server 2008/R2/2012/R2/vNextTP
                Select Case KeVersionInfo.ProductMinorPart
                    Case 0                                      ' Windows Vista/Server 2008
                        If MainForm.imgMountedName.Contains("Server") Then
                            Label10.Text = "Windows Server 2008"
                        Else
                            Label10.Text = "Windows Vista"
                        End If
                    Case 1                                      ' Windows 7/Server 2008 R2
                        If MainForm.imgMountedName.Contains("Server") Then
                            Label10.Text = "Windows Server 2008 R2"
                        Else
                            Label10.Text = "Windows 7"
                        End If
                    Case 2                                      ' Windows 8/Server 2012
                        If MainForm.imgMountedName.Contains("Server") Then
                            Label10.Text = "Windows Server 2012"
                        Else
                            Label10.Text = "Windows 8"
                        End If
                    Case 3                                      ' Windows 8.1/Server 2012 R2
                        If MainForm.imgMountedName.Contains("Server") Then
                            Label10.Text = "Windows Server 2012 R2"
                        Else
                            Label10.Text = "Windows 8.1"
                        End If
                    Case 4                                      ' Windows 10 Tech Preview/Server vNext Technical Preview
                        If MainForm.imgMountedName.Contains("Server") Then
                            Label10.Text = "Windows Server vNext Technical Preview - Unsupported system"
                        Else
                            Label10.Text = "Windows 10 Technical Preview - Unsupported system"
                        End If
                End Select
            ElseIf KeVersionInfo.ProductMajorPart = 10 Then     ' Windows 10/11/Server 2016/2019/2022/vNext
                Select Case KeVersionInfo.ProductBuildPart
                    Case 9888 To 21390                          ' Windows 10/Server 2016/2019/2022
                        If MainForm.imgMountedName.Contains("Server") Then
                            Select Case KeVersionInfo.ProductBuildPart
                                Case 14393
                                    Label10.Text = "Windows Server 2016"
                                Case 16299
                                    Label10.Text = "Windows Server Semi-Annual Channel (SAC), version 1709 - Unsupported system"
                                Case 17134
                                    Label10.Text = "Windows Server Semi-Annual Channel (SAC), version 1803 - Unsupported system"
                                Case 17763
                                    Label10.Text = "Windows Server 2019"
                                Case 18362
                                    Label10.Text = "Windows Server Semi-Annual Channel (SAC), version 1903 - Unsupported system"
                                Case 18363
                                    Label10.Text = "Windows Server Semi-Annual Channel (SAC), version 1909 - Unsupported system"
                                Case 19041
                                    Label10.Text = "Windows Server Semi-Annual Channel (SAC), version 2004 - Unsupported system"
                                Case 19042
                                    Label10.Text = "Windows Server Semi-Annual Channel (SAC), version 20H2 - Unsupported system"
                                Case 20348
                                    Label10.Text = "Windows Server 2022"
                                Case 20349
                                    Label10.Text = "Azure Stack HCI, version 22H2 - Unsupported system"
                                Case Else
                                    Label10.Text = "Unknown Windows Server version - Unsupported system"
                            End Select
                        ElseIf MainForm.imgMountedName.Contains("Azure") Then
                            Label10.Text = "Azure Stack HCI - Unsupported system"
                        Else
                            Label10.Text = "Windows 10"
                        End If
                    Case Is >= 21996                            ' Windows 11/Server vNext
                        If MainForm.imgMountedName.Contains("Server") Then
                            Select Case KeVersionInfo.ProductBuildPart
                                Case Is >= 25057
                                    Label10.Text = "Windows Server Copper/vNext - Unsupported system"
                                Case Else
                                    Label10.Text = "Unknown Windows Server version - Unsupported system"
                            End Select
                        Else
                            Label10.Text = "Windows 11"
                        End If
                End Select
            End If
            CanGatherImgVersion = True
        Catch ex As Exception
            Label9.Text = "Could not get version from mounted image"
            Label10.Text = "Could not get target operating system version"
            CanGatherImgVersion = False
        End Try
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem = "Windows XP" Then
            Label13.Text = "25/10/2001"
            Label15.Text = "5.1"
            OSWiki = "https://en.wikipedia.org/wiki/Windows_XP"
        ElseIf ComboBox1.SelectedItem = "Windows Vista" Then
            Label13.Text = "30/01/2007"
            Label15.Text = "6.0"
            OSWiki = "https://en.wikipedia.org/wiki/Windows_Vista"
        ElseIf ComboBox1.SelectedItem = "Windows Server 2008" Then
            Label13.Text = "27/02/2008"
            Label15.Text = "6.0"
            OSWiki = "https://en.wikipedia.org/wiki/Windows_Server_2008"
        ElseIf ComboBox1.SelectedItem = "Windows 7" Then
            Label13.Text = "22/10/2009"
            Label15.Text = "6.1"
            OSWiki = "https://en.wikipedia.org/wiki/Windows_7"
        ElseIf ComboBox1.SelectedItem = "Windows Server 2008 R2" Then
            Label13.Text = "22/10/2009"
            Label15.Text = "6.1"
            OSWiki = "https://en.wikipedia.org/wiki/Windows_Server_2008_R2"
        ElseIf ComboBox1.SelectedItem = "Windows 8" Then
            Label13.Text = "26/10/2012"
            Label15.Text = "6.2"
            OSWiki = "https://en.wikipedia.org/wiki/Windows_8"
        ElseIf ComboBox1.SelectedItem = "Windows Server 2012" Then
            Label13.Text = "04/09/2012"
            Label15.Text = "6.2"
            OSWiki = "https://en.wikipedia.org/wiki/Windows_Server_2012"
        ElseIf ComboBox1.SelectedItem = "Windows 8.1" Then
            Label13.Text = "18/10/2013"
            Label15.Text = "6.3"
            OSWiki = "https://en.wikipedia.org/wiki/Windows_8.1"
        ElseIf ComboBox1.SelectedItem = "Windows Server 2012 R2" Then
            Label13.Text = "18/10/2013"
            Label15.Text = "6.3"
            OSWiki = "https://en.wikipedia.org/wiki/Windows_Server_2012_R2"
        ElseIf ComboBox1.SelectedItem = "Windows 10" Then
            Label13.Text = "29/07/2015"
            Label15.Text = "10.0"
            OSWiki = "https://en.wikipedia.org/wiki/Windows_10"
        ElseIf ComboBox1.SelectedItem = "Windows 11" Then
            Label13.Text = "05/10/2021"
            Label15.Text = "10.0"
            OSWiki = "https://en.wikipedia.org/wiki/Windows_11"
        ElseIf ComboBox1.SelectedItem = "Windows Server 2016" Then
            Label13.Text = "12/10/2016"
            Label15.Text = "10.0"
            OSWiki = "https://en.wikipedia.org/wiki/Windows_Server_2016"
        ElseIf ComboBox1.SelectedItem = "Windows Server 2019" Then
            Label13.Text = "02/10/2018"
            Label15.Text = "10.0"
            OSWiki = "https://en.wikipedia.org/wiki/Windows_Server_2019"
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Process.Start(OSWiki)
    End Sub

    Sub TailorWizardToTargetOS()
        ComboBox2.Items.Clear()
        ExpressStatusLbl.Visible = True
        ExpressStatusLbl.Text = "Tailoring the wizard to the specified target OS, please wait..."
        Cursor = Cursors.WaitCursor
        Refresh()
        If RadioButton1.Checked Then
            Dim KeVersion As FileVersionInfo = FileVersionInfo.GetVersionInfo(MainForm.MountDir & "\Windows\system32\ntoskrnl.exe")
            Select Case KeVersion.ProductMajorPart
                Case 6
                    Select Case KeVersion.ProductMinorPart
                        Case 0
                            If MainForm.imgMountedName.Contains("Server") Then
                                ComboBox2.Items.Add("Windows Server 2008 Standard")
                                ComboBox2.Items.Add("Windows Server 2008 Web")
                                ComboBox2.Items.Add("Windows Server 2008 for High Performance Computing")
                                ComboBox2.Items.Add("Windows Server 2008 Enterprise")
                                ComboBox2.Items.Add("Windows Server 2008 for Intel(R) Itanium (TM)")
                                ComboBox2.Items.Add("Windows Server 2008 Datacenter")
                            Else
                                ComboBox2.Items.Add("Windows Vista Starter")
                                ComboBox2.Items.Add("Windows Vista Home Basic")
                                ComboBox2.Items.Add("Windows Vista Home Premium")
                                ComboBox2.Items.Add("Windows Vista Business")
                                ComboBox2.Items.Add("Windows Vista Ultimate")
                            End If
                        Case 1
                            If MainForm.imgMountedName.Contains("Server") Then
                                ComboBox2.Items.Add("Windows Server 2008 R2 Foundation")
                                ComboBox2.Items.Add("Windows Server 2008 R2 Standard")
                                ComboBox2.Items.Add("Windows Server 2008 R2 Web")
                                ComboBox2.Items.Add("Windows Server 2008 R2 for High Performance Computing")
                                ComboBox2.Items.Add("Windows Server 2008 R2 Enterprise")
                                ComboBox2.Items.Add("Windows Server 2008 R2 for Intel(R) Itanium (TM)")
                                ComboBox2.Items.Add("Windows Server 2008 R2 Datacenter")
                                ComboBox2.Items.Add("Microsoft Hyper-V Server 2008 R2")
                                ComboBox2.Items.Add("Windows MultiPoint Server 2010")
                            Else
                                ComboBox2.Items.Add("Windows 7 Starter")
                                ComboBox2.Items.Add("Windows 7 Home Basic")
                                ComboBox2.Items.Add("Windows 7 Home Premium")
                                ComboBox2.Items.Add("Windows 7 Professional")
                                ComboBox2.Items.Add("Windows 7 Ultimate")
                                ComboBox2.Items.Add("Windows 7 Enterprise")
                                ComboBox2.Items.Add("Windows 7 Starter N")
                                ComboBox2.Items.Add("Windows 7 Home Basic N")
                                ComboBox2.Items.Add("Windows 7 Home Premium N")
                                ComboBox2.Items.Add("Windows 7 Professional N")
                                ComboBox2.Items.Add("Windows 7 Ultimate N")
                                ComboBox2.Items.Add("Windows 7 Enterprise N")
                                ComboBox2.Items.Add("Windows 7 Starter E")
                                ComboBox2.Items.Add("Windows 7 Home Basic E")
                                ComboBox2.Items.Add("Windows 7 Home Premium E")
                                ComboBox2.Items.Add("Windows 7 Professional E")
                                ComboBox2.Items.Add("Windows 7 Ultimate E")
                                ComboBox2.Items.Add("Windows 7 Enterprise E")
                            End If
                        Case 2
                            If MainForm.imgMountedName.Contains("Server") Then
                                ComboBox2.Items.Add("Windows Server 2012 Foundation")
                                ComboBox2.Items.Add("Windows Server 2012 Standard")
                                ComboBox2.Items.Add("Windows Server 2012 Datacenter")
                                ComboBox2.Items.Add("Windows Server 2012 Storage Server")
                                ComboBox2.Items.Add("Windows MultiPoint Server 2012 Standard")
                                ComboBox2.Items.Add("Windows MultiPoint Server 2012 Premium")
                                ComboBox2.Items.Add("Windows Server 2012 Standard Core")
                                ComboBox2.Items.Add("Windows Server 2012 Datacenter Core")
                            Else
                                ComboBox2.Items.Add("Windows 8")
                                ComboBox2.Items.Add("Windows RT 8")
                                ComboBox2.Items.Add("Windows 8 with Bing")
                                ComboBox2.Items.Add("Windows 8 Single Language")
                                ComboBox2.Items.Add("Windows 8 Pro")
                                ComboBox2.Items.Add("Windows 8 Pro with Windows Media Center")
                                ComboBox2.Items.Add("Windows 8 Enterprise")
                                ComboBox2.Items.Add("Windows 8 N")
                                ComboBox2.Items.Add("Windows 8 Pro N")
                                ComboBox2.Items.Add("Windows 8 Enterprise N")
                            End If
                        Case 3
                            If MainForm.imgMountedName.Contains("Server") Then
                                ComboBox2.Items.Add("Windows Server 2012 R2 Essentials")
                                ComboBox2.Items.Add("Windows Server 2012 R2 Foundation")
                                ComboBox2.Items.Add("Windows Server 2012 R2 Standard")
                                ComboBox2.Items.Add("Windows Server 2012 R2 Datacenter")
                                ComboBox2.Items.Add("Windows Server 2012 R2 Storage Server Standard")
                                ComboBox2.Items.Add("Windows Server 2012 R2 Storage Server Workgroup")
                                ComboBox2.Items.Add("Microsoft Hyper-V Server 2012 R2")
                                ComboBox2.Items.Add("Windows Server 2012 R2 Essentials Core")
                                ComboBox2.Items.Add("Windows Server 2012 R2 Foundation Core")
                                ComboBox2.Items.Add("Windows Server 2012 R2 Standard Core")
                                ComboBox2.Items.Add("Windows Server 2012 R2 Datacenter Core")
                                ComboBox2.Items.Add("Windows Server 2012 R2 Storage Server Standard Core")
                                ComboBox2.Items.Add("Windows Server 2012 R2 Storage Server Workgroup Core")
                            Else
                                ComboBox2.Items.Add("Windows 8.1")
                                ComboBox2.Items.Add("Windows RT 8.1")
                                ComboBox2.Items.Add("Windows 8.1 with Bing")
                                ComboBox2.Items.Add("Windows 8.1 Single Language")
                                ComboBox2.Items.Add("Windows 8.1 Pro")
                                ComboBox2.Items.Add("Windows 8.1 Pro with Windows Media Center")
                                ComboBox2.Items.Add("Windows 8.1 Enterprise")
                                ComboBox2.Items.Add("Windows 8.1 N")
                                ComboBox2.Items.Add("Windows 8.1 Pro N")
                                ComboBox2.Items.Add("Windows 8.1 Enterprise N")
                            End If
                    End Select
                Case 10
                    Select Case KeVersion.ProductBuildPart
                        Case 9888 To 21390
                            If MainForm.imgMountedName.Contains("Server") Then
                                Select Case KeVersion.ProductBuildPart
                                    Case 14393
                                        ComboBox2.Items.Add("Windows Server 2016 Essentials")
                                        ComboBox2.Items.Add("Windows Server 2016 Standard")
                                        ComboBox2.Items.Add("Windows Server 2016 Datacenter")
                                    Case 17763
                                        ComboBox2.Items.Add("Windows Server 2019 Standard")
                                        ComboBox2.Items.Add("Windows Server 2019 Datacenter")
                                End Select
                            Else
                                ComboBox2.Items.Add("Windows 10 Home")
                                ComboBox2.Items.Add("Windows 10 Pro")
                                ComboBox2.Items.Add("Windows 10 Education")
                                ComboBox2.Items.Add("Windows 10 Enterprise")
                            End If
                        Case Is >= 21996
                            ComboBox2.Items.Add("Windows 11 Home")
                            ComboBox2.Items.Add("Windows 11 Pro")
                            ComboBox2.Items.Add("Windows 11 Education")
                            ComboBox2.Items.Add("Windows 11 Enterprise")
                    End Select
            End Select
            If KeVersion.ProductMajorPart = 6 And KeVersion.ProductMinorPart = 2 Or KeVersion.ProductMinorPart = 3 Then
                GroupBox6.Visible = True
                PictureBox19.Location = New Point(133, 84)
                Label78.Location = New Point(33, 189)
                TextBox4.Location = New Point(34, 208)
                Label79.Location = New Point(33, 234)
                TextBox5.Location = New Point(34, 253)
                Label80.Location = New Point(33, 281)
            Else
                GroupBox6.Visible = False
                PictureBox19.Location = New Point(328, 84)
                Label78.Location = New Point(228, 189)
                TextBox4.Location = New Point(229, 208)
                Label79.Location = New Point(228, 234)
                TextBox5.Location = New Point(229, 253)
                Label80.Location = New Point(228, 281)
            End If
        Else
            Select Case ComboBox1.SelectedIndex
                Case 0
                    IsWindows8 = False
                    ComboBox2.Items.Add("Windows 7 Starter")
                    ComboBox2.Items.Add("Windows 7 Home Basic")
                    ComboBox2.Items.Add("Windows 7 Home Premium")
                    ComboBox2.Items.Add("Windows 7 Professional")
                    ComboBox2.Items.Add("Windows 7 Ultimate")
                    ComboBox2.Items.Add("Windows 7 Enterprise")
                    ComboBox2.Items.Add("Windows 7 Starter N")
                    ComboBox2.Items.Add("Windows 7 Home Basic N")
                    ComboBox2.Items.Add("Windows 7 Home Premium N")
                    ComboBox2.Items.Add("Windows 7 Professional N")
                    ComboBox2.Items.Add("Windows 7 Ultimate N")
                    ComboBox2.Items.Add("Windows 7 Enterprise N")
                    ComboBox2.Items.Add("Windows 7 Starter E")
                    ComboBox2.Items.Add("Windows 7 Home Basic E")
                    ComboBox2.Items.Add("Windows 7 Home Premium E")
                    ComboBox2.Items.Add("Windows 7 Professional E")
                    ComboBox2.Items.Add("Windows 7 Ultimate E")
                    ComboBox2.Items.Add("Windows 7 Enterprise E")
                Case 1
                    IsWindows8 = False
                    ComboBox2.Items.Add("Windows Server 2008 R2 Foundation")
                    ComboBox2.Items.Add("Windows Server 2008 R2 Standard")
                    ComboBox2.Items.Add("Windows Server 2008 R2 Web")
                    ComboBox2.Items.Add("Windows Server 2008 R2 for High Performance Computing")
                    ComboBox2.Items.Add("Windows Server 2008 R2 Enterprise")
                    ComboBox2.Items.Add("Windows Server 2008 R2 for Intel(R) Itanium (TM)")
                    ComboBox2.Items.Add("Windows Server 2008 R2 Datacenter")
                    ComboBox2.Items.Add("Microsoft Hyper-V Server 2008 R2")
                    ComboBox2.Items.Add("Windows MultiPoint Server 2010")
                Case 2
                    IsWindows8 = True
                    ComboBox2.Items.Add("Windows 8")
                    ComboBox2.Items.Add("Windows RT 8")
                    ComboBox2.Items.Add("Windows 8 with Bing")
                    ComboBox2.Items.Add("Windows 8 Single Language")
                    ComboBox2.Items.Add("Windows 8 Pro")
                    ComboBox2.Items.Add("Windows 8 Pro with Windows Media Center")
                    ComboBox2.Items.Add("Windows 8 Enterprise")
                    ComboBox2.Items.Add("Windows 8 N")
                    ComboBox2.Items.Add("Windows 8 Pro N")
                    ComboBox2.Items.Add("Windows 8 Enterprise N")
                Case 3
                    IsWindows8 = True
                    ComboBox2.Items.Add("Windows Server 2012 Foundation")
                    ComboBox2.Items.Add("Windows Server 2012 Standard")
                    ComboBox2.Items.Add("Windows Server 2012 Datacenter")
                    ComboBox2.Items.Add("Windows Server 2012 Storage Server")
                    ComboBox2.Items.Add("Windows MultiPoint Server 2012 Standard")
                    ComboBox2.Items.Add("Windows MultiPoint Server 2012 Premium")
                    ComboBox2.Items.Add("Windows Server 2012 Standard Core")
                    ComboBox2.Items.Add("Windows Server 2012 Datacenter Core")
                Case 4
                    IsWindows8 = True
                    ComboBox2.Items.Add("Windows 8.1")
                    ComboBox2.Items.Add("Windows RT 8.1")
                    ComboBox2.Items.Add("Windows 8.1 with Bing")
                    ComboBox2.Items.Add("Windows 8.1 Single Language")
                    ComboBox2.Items.Add("Windows 8.1 Pro")
                    ComboBox2.Items.Add("Windows 8.1 Pro with Windows Media Center")
                    ComboBox2.Items.Add("Windows 8.1 Enterprise")
                    ComboBox2.Items.Add("Windows 8.1 N")
                    ComboBox2.Items.Add("Windows 8.1 Pro N")
                    ComboBox2.Items.Add("Windows 8.1 Enterprise N")
                Case 5
                    IsWindows8 = True
                    ComboBox2.Items.Add("Windows Server 2012 R2 Essentials")
                    ComboBox2.Items.Add("Windows Server 2012 R2 Foundation")
                    ComboBox2.Items.Add("Windows Server 2012 R2 Standard")
                    ComboBox2.Items.Add("Windows Server 2012 R2 Datacenter")
                    ComboBox2.Items.Add("Windows Server 2012 R2 Storage Server Standard")
                    ComboBox2.Items.Add("Windows Server 2012 R2 Storage Server Workgroup")
                    ComboBox2.Items.Add("Microsoft Hyper-V Server 2012 R2")
                    ComboBox2.Items.Add("Windows Server 2012 R2 Essentials Core")
                    ComboBox2.Items.Add("Windows Server 2012 R2 Foundation Core")
                    ComboBox2.Items.Add("Windows Server 2012 R2 Standard Core")
                    ComboBox2.Items.Add("Windows Server 2012 R2 Datacenter Core")
                    ComboBox2.Items.Add("Windows Server 2012 R2 Storage Server Standard Core")
                    ComboBox2.Items.Add("Windows Server 2012 R2 Storage Server Workgroup Core")
                Case 6
                    IsWindows8 = False
                    ComboBox2.Items.Add("Windows 10 Home")
                    ComboBox2.Items.Add("Windows 10 Pro")
                    ComboBox2.Items.Add("Windows 10 Education")
                    ComboBox2.Items.Add("Windows 10 Enterprise")
                Case 7
                    IsWindows8 = False
                    ComboBox2.Items.Add("Windows 11 Home")
                    ComboBox2.Items.Add("Windows 11 Pro")
                    ComboBox2.Items.Add("Windows 11 Education")
                    ComboBox2.Items.Add("Windows 11 Enterprise")
                Case 8
                    IsWindows8 = False
                    ComboBox2.Items.Add("Windows Server 2016 Essentials")
                    ComboBox2.Items.Add("Windows Server 2016 Standard")
                    ComboBox2.Items.Add("Windows Server 2016 Datacenter")
                Case 9
                    IsWindows8 = False
                    ComboBox2.Items.Add("Windows Server 2019 Standard")
                    ComboBox2.Items.Add("Windows Server 2019 Datacenter")
            End Select
            If IsWindows8 Then
                GroupBox6.Visible = True
                PictureBox19.Location = New Point(133, 84)
                Label78.Location = New Point(33, 189)
                TextBox4.Location = New Point(34, 208)
                Label79.Location = New Point(33, 234)
                TextBox5.Location = New Point(34, 253)
                Label80.Location = New Point(33, 281)
            Else
                GroupBox6.Visible = False
                PictureBox19.Location = New Point(328, 84)
                Label78.Location = New Point(228, 189)
                TextBox4.Location = New Point(229, 208)
                Label79.Location = New Point(228, 234)
                TextBox5.Location = New Point(229, 253)
                Label80.Location = New Point(228, 281)
            End If
        End If
        Thread.Sleep(2000)
        ExpressStatusLbl.Visible = False
        Cursor = Cursors.Arrow
    End Sub

    Sub ChangeGenericKey()
        Dim OSKey As String = ComboBox2.SelectedItem.ToString()
        ' Add generic key conditions. Key source: https://www.windowsafg.com/keys.html
        If OSKey = "Windows Vista Starter" Then
            GenericBox1.Text = "X9PYV"
            GenericBox2.Text = "YBQRV"
            GenericBox3.Text = "9BXWV"
            GenericBox4.Text = "TQDMK"
            GenericBox5.Text = "QDWK4"
        ElseIf OSKey = "Windows Vista Home Basic" Then
            GenericBox1.Text = "RCG7P"
            GenericBox2.Text = "TX42D"
            GenericBox3.Text = "HM8FM"
            GenericBox4.Text = "TCFCW"
            GenericBox5.Text = "3V4VD"
        ElseIf OSKey = "Windows Vista Home Premium" Then
            GenericBox1.Text = "H9HTF"
            GenericBox2.Text = "MKJQQ"
            GenericBox3.Text = "XK376"
            GenericBox4.Text = "TJ7T4"
            GenericBox5.Text = "76PKF"
        ElseIf OSKey = "Windows Vista Business" Then
            GenericBox1.Text = "4D2XH"
            GenericBox2.Text = "PRBMM"
            GenericBox3.Text = "8Q22B"
            GenericBox4.Text = "K8BM3"
            GenericBox5.Text = "MRW4W"
        ElseIf OSKey = "Windows Vista Ultimate" Then
            GenericBox1.Text = "VMCB9"
            GenericBox2.Text = "FDRV6"
            GenericBox3.Text = "6CDQM"
            GenericBox4.Text = "RV23K"
            GenericBox5.Text = "RP8F7"
        ElseIf OSKey = "Windows 7 Starter" Then
            GenericBox1.Text = "7Q28W"
            GenericBox2.Text = "FT9PC"
            GenericBox3.Text = "CMMYT"
            GenericBox4.Text = "WHMY2"
            GenericBox5.Text = "89M6G"
        ElseIf OSKey = "Windows 7 Home Basic" Then
            GenericBox1.Text = "YGFVB"
            GenericBox2.Text = "QFTXQ"
            GenericBox3.Text = "3H233"
            GenericBox4.Text = "PTWTJ"
            GenericBox5.Text = "YRYRV"
        ElseIf OSKey = "Windows 7 Home Premium" Then
            GenericBox1.Text = "RHPQ2"
            GenericBox2.Text = "RMFJH"
            GenericBox3.Text = "74XYM"
            GenericBox4.Text = "BH4JX"
            GenericBox5.Text = "XM76F"
        ElseIf OSKey = "Windows 7 Professional" Then
            GenericBox1.Text = "HYF8J"
            GenericBox2.Text = "CVRMY"
            GenericBox3.Text = "CM74G"
            GenericBox4.Text = "RPHKF"
            GenericBox5.Text = "PW487"
        ElseIf OSKey = "Windows 7 Ultimate" Then
            GenericBox1.Text = "D4F6K"
            GenericBox2.Text = "QK3RD"
            GenericBox3.Text = "TMVMJ"
            GenericBox4.Text = "BBMRX"
            GenericBox5.Text = "3MBMV"
        ElseIf OSKey = "Windows 7 Enterprise" Then
            GenericBox1.Text = "H7X92"
            GenericBox2.Text = "3VPBB"
            GenericBox3.Text = "Q799D"
            GenericBox4.Text = "Y6JJ3"
            GenericBox5.Text = "86WC6"
        ElseIf OSKey = "Windows 7 Starter N" Then
            GenericBox1.Text = "D4C3G"
            GenericBox2.Text = "38HGY"
            GenericBox3.Text = "HGQCV"
            GenericBox4.Text = "QCWR8"
            GenericBox5.Text = "97FFR"
        ElseIf OSKey = "Windows 7 Home Basic N" Then
            GenericBox1.Text = "MD83G"
            GenericBox2.Text = "H98CG"
            GenericBox3.Text = "DXPYQ"
            GenericBox4.Text = "Q8GCR"
            GenericBox5.Text = "HM8X2"
        ElseIf OSKey = "Windows 7 Home Premium N" Then
            GenericBox1.Text = "D3PVQ"
            GenericBox2.Text = "V7M4J"
            GenericBox3.Text = "9Q9K3"
            GenericBox4.Text = "GG4K3"
            GenericBox5.Text = "F99JM"
        ElseIf OSKey = "Windows 7 Professional N" Then
            GenericBox1.Text = "BKFRB"
            GenericBox2.Text = "RTCT3"
            GenericBox3.Text = "9HW44"
            GenericBox4.Text = "FX3X8"
            GenericBox5.Text = "M48M6"
        ElseIf OSKey = "Windows 7 Ultimate N" Then
            GenericBox1.Text = "HTJK6"
            GenericBox2.Text = "DXX8T"
            GenericBox3.Text = "TVCR6"
            GenericBox4.Text = "KDG67"
            GenericBox5.Text = "97JHQ"
        ElseIf OSKey = "Windows 7 Enterprise N" Then
            GenericBox1.Text = "BQ4TH"
            GenericBox2.Text = "BWRRY"
            GenericBox3.Text = "424Y9"
            GenericBox4.Text = "7PQX2"
            GenericBox5.Text = "B4WBD"
        ElseIf OSKey = "Windows 7 Starter E" Then
            GenericBox1.Text = "BRQCV"
            GenericBox2.Text = "K7HCQ"
            GenericBox3.Text = "CKXP6"
            GenericBox4.Text = "2XP7K"
            GenericBox5.Text = "F233B"
        ElseIf OSKey = "Windows 7 Home Basic E" Then
            GenericBox1.Text = "VTKM9"
            GenericBox2.Text = "74GQY"
            GenericBox3.Text = "K3W94"
            GenericBox4.Text = "47DHV"
            GenericBox5.Text = "FTXJY"
        ElseIf OSKey = "Windows 7 Home Premium E" Then
            GenericBox1.Text = "76BRM"
            GenericBox2.Text = "9Q4K3"
            GenericBox3.Text = "QDJ48"
            GenericBox4.Text = "FH4F3"
            GenericBox5.Text = "9WT2R"
        ElseIf OSKey = "Windows 7 Professional E" Then
            GenericBox1.Text = "3YHKG"
            GenericBox2.Text = "DVQ27"
            GenericBox3.Text = "RYRBX"
            GenericBox4.Text = "JMPVM"
            GenericBox5.Text = "WG38T"
        ElseIf OSKey = "Windows 7 Ultimate E" Then
            GenericBox1.Text = "TWMF7"
            GenericBox2.Text = "M387V"
            GenericBox3.Text = "XKW4Y"
            GenericBox4.Text = "PVQQD"
            GenericBox5.Text = "RK7C8"
        ElseIf OSKey = "Windows 7 Enterprise E" Then
            GenericBox1.Text = "H3V6Q"
            GenericBox2.Text = "JKQJG"
            GenericBox3.Text = "GKVK3"
            GenericBox4.Text = "FDDRF"
            GenericBox5.Text = "TKCVR"
        ElseIf OSKey = "Windows 8" Then
            GenericBox1.Text = "FB4WR"
            GenericBox2.Text = "32NVD"
            GenericBox3.Text = "4RW79"
            GenericBox4.Text = "XQFWH"
            GenericBox5.Text = "CYQG3"
        ElseIf OSKey = "Windows RT 8" Then
            GenericBox1.Text = "6D4CN"
            GenericBox2.Text = "WMGRW"
            GenericBox3.Text = "DG8M6"
            GenericBox4.Text = "XX8W9"
            GenericBox5.Text = "3RPT8"
        ElseIf OSKey = "Windows 8 with Bing" Then
            GenericBox1.Text = "XYNVP"
            GenericBox2.Text = "TW798"
            GenericBox3.Text = "F8893"
            GenericBox4.Text = "7B89K"
            GenericBox5.Text = "8QHDK"
        ElseIf OSKey = "Windows 8 Single Language" Then
            GenericBox1.Text = "XBRND"
            GenericBox2.Text = "QDJTG"
            GenericBox3.Text = "CQJDB"
            GenericBox4.Text = "7DRBW"
            GenericBox5.Text = "RX6HB"
        ElseIf OSKey = "Windows 8 Pro" Then
            GenericBox1.Text = "XKY4K"
            GenericBox2.Text = "2NRWR"
            GenericBox3.Text = "8F6P2"
            GenericBox4.Text = "448RF"
            GenericBox5.Text = "CRYQ8"
        ElseIf OSKey = "Windows 8 Pro with Windows Media Center" Then
            GenericBox1.Text = "RR3BN"
            GenericBox2.Text = "3YY9P"
            GenericBox3.Text = "9D7FC"
            GenericBox4.Text = "7J4YF"
            GenericBox5.Text = "QGJXW"
        ElseIf OSKey = "Windows 8 Enterprise" Then
            GenericBox1.Text = "32JNW"
            GenericBox2.Text = "9KQ84"
            GenericBox3.Text = "P47T8"
            GenericBox4.Text = "D8GGY"
            GenericBox5.Text = "CWCK7"
        ElseIf OSKey = "Windows 8 N" Then
            GenericBox1.Text = "VDKYM"
            GenericBox2.Text = "JNKJ7"
            GenericBox3.Text = "DC489"
            GenericBox4.Text = "BT3QR"
            GenericBox5.Text = "JHRDC"
        ElseIf OSKey = "Windows 8 Pro N" Then
            GenericBox1.Text = "BHHD4"
            GenericBox2.Text = "FKNK8"
            GenericBox3.Text = "89X83"
            GenericBox4.Text = "HTGM4"
            GenericBox5.Text = "3C73G"
        ElseIf OSKey = "Windows 8 Enterprise N" Then
            GenericBox1.Text = "NCVKH"
            GenericBox2.Text = "RB9D4"
            GenericBox3.Text = "R86X8"
            GenericBox4.Text = "GB8WG"
            GenericBox5.Text = "4M2K6"
        ElseIf OSKey = "Windows 8.1" Then
            GenericBox1.Text = "334NH"
            GenericBox2.Text = "RXG76"
            GenericBox3.Text = "64THK"
            GenericBox4.Text = "C7CKG"
            GenericBox5.Text = "D3VPT"
        ElseIf OSKey = "Windows RT 8.1" Then
            GenericBox1.Text = "NK2V7"
            GenericBox2.Text = "9DW8G"
            GenericBox3.Text = "KMTWQ"
            GenericBox4.Text = "K9H9M"
            GenericBox5.Text = "6VHPJ"
        ElseIf OSKey = "Windows 8.1 with Bing" Then
            GenericBox1.Text = "TNH8J"
            GenericBox2.Text = "KG84C"
            GenericBox3.Text = "TRMG4"
            GenericBox4.Text = "FFD7J"
            GenericBox5.Text = "VH4WX"
        ElseIf OSKey = "Windows 8.1 Single Language" Then
            GenericBox1.Text = "Y9NXP"
            GenericBox2.Text = "XT8MV"
            GenericBox3.Text = "PT9TG"
            GenericBox4.Text = "97CT3"
            GenericBox5.Text = "9D6TC"
        ElseIf OSKey = "Windows 8.1 Pro" Then
            GenericBox1.Text = "GCRJD"
            GenericBox2.Text = "8NW9H"
            GenericBox3.Text = "F2CDX"
            GenericBox4.Text = "CCM8D"
            GenericBox5.Text = "9D6T9"
        ElseIf OSKey = "Windows 8.1 Pro with Windows Media Center" Then
            GenericBox1.Text = "GBFNG"
            GenericBox2.Text = "2X3TC"
            GenericBox3.Text = "8R27F"
            GenericBox4.Text = "RMKYB"
            GenericBox5.Text = "JK7QT"
        ElseIf OSKey = "Windows 8.1 Enterprise" Then
            GenericBox1.Text = "FHQNR"
            GenericBox2.Text = "XYXYC"
            GenericBox3.Text = "89MHT"
            GenericBox4.Text = "TV4PH"
            GenericBox5.Text = "DRQ3H"
        ElseIf OSKey = "Windows 8.1 N" Then
            GenericBox1.Text = "6NPQ8"
            GenericBox2.Text = "PK64X"
            GenericBox3.Text = "W4WMM"
            GenericBox4.Text = "MF84V"
            GenericBox5.Text = "RGB89"
        ElseIf OSKey = "Windows 8.1 Pro N" Then
            GenericBox1.Text = "JRBBN"
            GenericBox2.Text = "4Q997"
            GenericBox3.Text = "H4RM2"
            GenericBox4.Text = "H3B7W"
            GenericBox5.Text = "Q68KC"
        ElseIf OSKey = "Windows 8.1 Enterprise N" Then
            GenericBox1.Text = "NDRDJ"
            GenericBox2.Text = "3YBP2"
            GenericBox3.Text = "8WTKD"
            GenericBox4.Text = "CK7VB"
            GenericBox5.Text = "HT8KW"
        ElseIf OSKey = "Windows 10 Home" Or OSKey = "Windows 11 Home" Then
            GenericBox1.Text = "TX9XD"
            GenericBox2.Text = "98N7W"
            GenericBox3.Text = "6WMQ6"
            GenericBox4.Text = "BX7FG"
            GenericBox5.Text = "H8Q99"
        ElseIf OSKey = "Windows 10 Pro" Or OSKey = "Windows 11 Pro" Then
            GenericBox1.Text = "W269N"
            GenericBox2.Text = "WFGWX"
            GenericBox3.Text = "YVC9B"
            GenericBox4.Text = "4J6C9"
            GenericBox5.Text = "T83GX"
        ElseIf OSKey = "Windows 10 Education" Or OSKey = "Windows 11 Education" Then
            GenericBox1.Text = "NW6C2"
            GenericBox2.Text = "QMPVW"
            GenericBox3.Text = "D7KKK"
            GenericBox4.Text = "3GKT6"
            GenericBox5.Text = "VCFB2"
        ElseIf OSKey = "Windows 10 Enterprise" Or OSKey = "Windows 11 Enterprise" Then
            GenericBox1.Text = "NPPR9"
            GenericBox2.Text = "FWDCX"
            GenericBox3.Text = "D2C8J"
            GenericBox4.Text = "H872K"
            GenericBox5.Text = "2YT43"
        ElseIf OSKey = "Windows Server 2008 Standard" Then
            GenericBox1.Text = "TM24T"
            GenericBox2.Text = "X9RMF"
            GenericBox3.Text = "VWXK6"
            GenericBox4.Text = "X8JC9"
            GenericBox5.Text = "BFGM2"
        ElseIf OSKey = "Windows Server 2008 Web" Then
            GenericBox1.Text = "WYR28"
            GenericBox2.Text = "R7TFJ"
            GenericBox3.Text = "3X2YQ"
            GenericBox4.Text = "YCY4H"
            GenericBox5.Text = "M249D"
        ElseIf OSKey = "Windows Server 2008 for High Performance Computing" Then
            GenericBox1.Text = "RCTX3"
            GenericBox2.Text = "KWVHP"
            GenericBox3.Text = "BR6TB"
            GenericBox4.Text = "RB6DM"
            GenericBox5.Text = "6X7HP"
        ElseIf OSKey = "Windows Server 2008 Enterprise" Then
            GenericBox1.Text = "YQGMW"
            GenericBox2.Text = "MPWTJ"
            GenericBox3.Text = "34KDK"
            GenericBox4.Text = "48M3W"
            GenericBox5.Text = "X4Q6V"
        ElseIf OSKey = "Windows Server 2008 for Intel(R) Itanium(TM)" Then
            GenericBox1.Text = "7YKJ4"
            GenericBox2.Text = "CX8QP"
            GenericBox3.Text = "Q23QY"
            GenericBox4.Text = "7BYQM"
            GenericBox5.Text = "H2893"
        ElseIf OSKey = "Windows Server 2008 Datacenter" Then
            GenericBox1.Text = "7M67G"
            GenericBox2.Text = "PC374"
            GenericBox3.Text = "GR742"
            GenericBox4.Text = "YH8V4"
            GenericBox5.Text = "TCBY3"
        ElseIf OSKey = "Windows Server 2008 R2 Foundation" Then
            GenericBox1.Text = "36RXV"
            GenericBox2.Text = "4Y4PJ"
            GenericBox3.Text = "B7DWH"
            GenericBox4.Text = "XY4VW"
            GenericBox5.Text = "KQXDQ"
        ElseIf OSKey = "Windows Server 2008 R2 Standard" Then
            GenericBox1.Text = "YC6KT"
            GenericBox2.Text = "GKW9T"
            GenericBox3.Text = "YTKYR"
            GenericBox4.Text = "T4X34"
            GenericBox5.Text = "R7VHC"
        ElseIf OSKey = "Windows Server 2008 R2 Web" Then
            GenericBox1.Text = "6TPJF"
            GenericBox2.Text = "RBVHG"
            GenericBox3.Text = "WBW2R"
            GenericBox4.Text = "86QPH"
            GenericBox5.Text = "6RTM4"
        ElseIf OSKey = "Windows Server 2008 R2 for High Performance Computing" Then
            GenericBox1.Text = "TT8MH"
            GenericBox2.Text = "CG224"
            GenericBox3.Text = "D3D7Q"
            GenericBox4.Text = "498W2"
            GenericBox5.Text = "9QCTX"
        ElseIf OSKey = "Windows Server 2008 R2 Enterprise" Then
            GenericBox1.Text = "489J6"
            GenericBox2.Text = "VHDMP"
            GenericBox3.Text = "X63PK"
            GenericBox4.Text = "3K798"
            GenericBox5.Text = "CPX3Y"
        ElseIf OSKey = "Windows Server 2008 R2 for Intel(R) Itanium(TM)" Then
            GenericBox1.Text = "GT63C"
            GenericBox2.Text = "RJFQ3"
            GenericBox3.Text = "4GMB6"
            GenericBox4.Text = "BRFB9"
            GenericBox5.Text = "CB83V"
        ElseIf OSKey = "Windows Server 2008 R2 Datacenter" Then
            GenericBox1.Text = "74YFP"
            GenericBox2.Text = "3QBF3"
            GenericBox3.Text = "KQT8W"
            GenericBox4.Text = "PMXWJ"
            GenericBox5.Text = "7M648"
        ElseIf OSKey = "Microsoft Hyper-V Server 2008 R2" Then
            GenericBox1.Text = "Q8R8C"
            GenericBox2.Text = "T2W6H"
            GenericBox3.Text = "7MGPB"
            GenericBox4.Text = "4CQ9R"
            GenericBox5.Text = "KR36H"
        ElseIf OSKey = "Windows MultiPoint Server 2010" Then
            GenericBox1.Text = "736RC"
            GenericBox2.Text = "XDKJK"
            GenericBox3.Text = "V34PF"
            GenericBox4.Text = "BHK87"
            GenericBox5.Text = "J6X3K"
        ElseIf OSKey = "Windows Server 2012 Foundation" Then
            GenericBox1.Text = "PN24B"
            GenericBox2.Text = "X6THG"
            GenericBox3.Text = "274MF"
            GenericBox4.Text = "YHM9G"
            GenericBox5.Text = "H8MVG"
        ElseIf OSKey = "Windows Server 2012 Standard" Then
            GenericBox1.Text = "VN93G"
            GenericBox2.Text = "8PVT3"
            GenericBox3.Text = "W2X3H"
            GenericBox4.Text = "F3X87"
            GenericBox5.Text = "FJMTW"
        ElseIf OSKey = "Windows Server 2012 Datacenter" Then
            GenericBox1.Text = "2GMNX"
            GenericBox2.Text = "8K7D2"
            GenericBox3.Text = "X968C"
            GenericBox4.Text = "7P62F"
            GenericBox5.Text = "8B2QK"
        ElseIf OSKey = "Windows Server 2012 Storage Server" Then
            GenericBox1.Text = "RD9XF"
            GenericBox2.Text = "6N3MC"
            GenericBox3.Text = "2P2R3"
            GenericBox4.Text = "MK2WX"
            GenericBox5.Text = "C7GCW"
        ElseIf OSKey = "Windows MultiPoint Server 2012 Standard" Then
            GenericBox1.Text = "32TNQ"
            GenericBox2.Text = "HMFWQ"
            GenericBox3.Text = "8R933"
            GenericBox4.Text = "X6VYY"
            GenericBox5.Text = "WHRFX"
        ElseIf OSKey = "Windows MultiPoint Server 2012 Premium" Then
            GenericBox1.Text = "CBR2N"
            GenericBox2.Text = "2HG39"
            GenericBox3.Text = "2TGGT"
            GenericBox4.Text = "GQB27"
            GenericBox5.Text = "46V47"
        ElseIf OSKey = "Windows Server 2012 Standard Core" Then
            GenericBox1.Text = "VN93G"
            GenericBox2.Text = "8PVT3"
            GenericBox3.Text = "W2X3H"
            GenericBox4.Text = "F3X87"
            GenericBox5.Text = "FJMTW"
        ElseIf OSKey = "Windows Server 2012 Datacenter Core" Then
            GenericBox1.Text = "2GMNX"
            GenericBox2.Text = "8K7D2"
            GenericBox3.Text = "X968C"
            GenericBox4.Text = "7P62F"
            GenericBox5.Text = "8B2QK"
        ElseIf OSKey = "Windows Server 2012 R2 Essentials" Then
            GenericBox1.Text = "KNC87"
            GenericBox2.Text = "3J2TX"
            GenericBox3.Text = "XB4WP"
            GenericBox4.Text = "VCPJV"
            GenericBox5.Text = "M4FWM"
        ElseIf OSKey = "Windows Server 2012 R2 Foundation" Then
            GenericBox1.Text = "7JGXN"
            GenericBox2.Text = "BW8X3"
            GenericBox3.Text = "DTJCK"
            GenericBox4.Text = "WG7XB"
            GenericBox5.Text = "YWP26"
        ElseIf OSKey = "Windows Server 2012 R2 Standard" Then
            GenericBox1.Text = "D2N9P"
            GenericBox2.Text = "3P6X9"
            GenericBox3.Text = "2R39C"
            GenericBox4.Text = "7RTCD"
            GenericBox5.Text = "MDVJX"
        ElseIf OSKey = "Windows Server 2012 R2 Datacenter" Then
            GenericBox1.Text = "W3GGN"
            GenericBox2.Text = "FT8W3"
            GenericBox3.Text = "Y4M2J"
            GenericBox4.Text = "J84CP"
            GenericBox5.Text = "QDWK4"
        ElseIf OSKey = "Windows Server 2012 R2 Storage Server Standard" Then
            GenericBox1.Text = "H2K4M"
            GenericBox2.Text = "QNKQ2"
            GenericBox3.Text = "64699"
            GenericBox4.Text = "FYQHD"
            GenericBox5.Text = "2WDYT"
        ElseIf OSKey = "Windows Server 2012 R2 Storage Server Workgroup" Then
            GenericBox1.Text = "8N7PM"
            GenericBox2.Text = "D3C64"
            GenericBox3.Text = "RQVYF"
            GenericBox4.Text = "MX8T7"
            GenericBox5.Text = "G6MB2"
        ElseIf OSKey = "Microsoft Hyper-V Server 2012 R2" Then
            GenericBox1.Text = "Q8R8C"
            GenericBox2.Text = "T2W6H"
            GenericBox3.Text = "7MGPB"
            GenericBox4.Text = "4CQ9R"
            GenericBox5.Text = "KR36H"
        ElseIf OSKey = "Windows Server 2012 R2 Essentials Core" Then
            GenericBox1.Text = "326N4"
            GenericBox2.Text = "6GMBX"
            GenericBox3.Text = "PD2QT"
            GenericBox4.Text = "M7HX4"
            GenericBox5.Text = "TVHM8"
        ElseIf OSKey = "Windows Server 2012 R2 Foundation Core" Then
            GenericBox1.Text = "7JGXN"
            GenericBox2.Text = "BW8X3"
            GenericBox3.Text = "DTJCK"
            GenericBox4.Text = "WG7XB"
            GenericBox5.Text = "YWP26"
        ElseIf OSKey = "Windows Server 2012 R2 Standard Core" Then
            GenericBox1.Text = "NB4WH"
            GenericBox2.Text = "BBBYV"
            GenericBox3.Text = "3MPPC"
            GenericBox4.Text = "9RCMV"
            GenericBox5.Text = "46XCB"
        ElseIf OSKey = "Windows Server 2012 R2 Datacenter Core" Then
            GenericBox1.Text = "BH9T4"
            GenericBox2.Text = "4N7CW"
            GenericBox3.Text = "67J3M"
            GenericBox4.Text = "64J36"
            GenericBox5.Text = "WW98Y"
        ElseIf OSKey = "Windows Server 2012 R2 Storage Server Standard Core" Then
            GenericBox1.Text = "H2K4M"
            GenericBox2.Text = "QNKQ2"
            GenericBox3.Text = "64699"
            GenericBox4.Text = "FYQHD"
            GenericBox5.Text = "2WDYT"
        ElseIf OSKey = "Windows Server 2012 R2 Storage Server Workgroup Core" Then
            GenericBox1.Text = "8N7PM"
            GenericBox2.Text = "D3C64"
            GenericBox3.Text = "RQVYF"
            GenericBox4.Text = "MX8T7"
            GenericBox5.Text = "G6MB2"
        ElseIf OSKey = "Windows Server 2016 Essentials" Then
            GenericBox1.Text = "JCKRF"
            GenericBox2.Text = "N37P4"
            GenericBox3.Text = "C2D82"
            GenericBox4.Text = "9YXRT"
            GenericBox5.Text = "4M63B"
        ElseIf OSKey = "Windows Server 2016 Standard" Then
            GenericBox1.Text = "WC2BQ"
            GenericBox2.Text = "8NRM3"
            GenericBox3.Text = "FDDYY"
            GenericBox4.Text = "2BFGV"
            GenericBox5.Text = "KHKQY"
        ElseIf OSKey = "Windows Server 2016 Datacenter" Then
            GenericBox1.Text = "CB7KF"
            GenericBox2.Text = "BWN84"
            GenericBox3.Text = "R7R2Y"
            GenericBox4.Text = "793K2"
            GenericBox5.Text = "8XDDG"
        ElseIf OSKey = "Windows Server 2019 Standard" Then
            GenericBox1.Text = "N69G4"
            GenericBox2.Text = "B89J2"
            GenericBox3.Text = "4G8F4"
            GenericBox4.Text = "WWYCC"
            GenericBox5.Text = "J464C"
        ElseIf OSKey = "Windows Server 2019 Datacenter" Then
            GenericBox1.Text = "WMDGN"
            GenericBox2.Text = "G9PQG"
            GenericBox3.Text = "XVVXX"
            GenericBox4.Text = "R3X43"
            GenericBox5.Text = "63DFG"
        End If
        KeyString = GenericBox1.Text & "-" & GenericBox2.Text & "-" & GenericBox3.Text & "-" & GenericBox4.Text & "-" & GenericBox5.Text
    End Sub

    Private Sub CheckBox10_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox10.CheckedChanged
        If CheckBox10.Checked Then
            MsgBox("If you skip Windows Welcome, and process the unattended installation on a non-testing environment, the image will not work correctly.", MsgBoxStyle.Exclamation, "Skip Windows Welcome")
        End If
    End Sub

    Private Sub ComboBox9_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox9.SelectedIndexChanged
        If ComboBox9.SelectedItem = "Home" Then
            PictureBox13.Image = New Bitmap(My.Resources.home_net)
            Label68.Text = "If all the computers on this network are at your home, and you recognize them, choose this option. However, if you are on a public place such as airports or coffee shops, don't choose this option."
        ElseIf ComboBox9.SelectedItem = "Work" Then
            PictureBox13.Image = New Bitmap(My.Resources.work_net)
            Label68.Text = "If all the computers on this network are at your workplace, and you recognize them, choose this option. However, if you are on a public place such as airports or coffee shops, don't choose this option."
        ElseIf ComboBox9.SelectedItem = "Public" Then
            PictureBox13.Image = New Bitmap(My.Resources.public_net)
            Label68.Text = "If you don't recognize all the computers on the network, this is a public network and is not trusted."
        End If
    End Sub

    Private Sub CheckBox9_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox9.CheckedChanged
        If CheckBox9.Checked Then
            EulaScreen.Image = New Bitmap(My.Resources.eula_scr_disabled)
            Label60.Text = "The End-User License Agreement screen will not be shown."
        Else
            EulaScreen.Image = New Bitmap(My.Resources.eula_scr)
            Label60.Text = "The End-User License Agreement screen will be shown."
        End If
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Select Case TrackBar1.Value
            Case 0
                TrackBarSelValueDesc1.Text = "Do not install updates"
                TrackBarSelValueMsg1.Text = "Security updates will not be installed. This will make your unattended installation easier to compromise."
            Case 1
                TrackBarSelValueDesc1.Text = "Important updates only"
                TrackBarSelValueMsg1.Text = "Security updates and other important updates for Windows will be installed"
            Case 2
                TrackBarSelValueDesc1.Text = "Use recommended settings"
                TrackBarSelValueMsg1.Text = "Recommended and important updates will be installed. This will also help make Internet browsing safer, check online for solutions to problems, and help Microsoft improve Windows."
        End Select
    End Sub

    Private Sub TrackBar2_Scroll(sender As Object, e As EventArgs) Handles TrackBar2.Scroll
        Select Case TrackBar2.Value
            Case 0
                TrackBarSelValueDesc2.Text = "Do not check for updates"
                TrackBarSelValueMsg2.Text = "Windows will not check for updates. This will make your unattended installation out of date, which also makes it easier to compromise."
            Case 1
                TrackBarSelValueDesc2.Text = "Detect updates, but let you choose which to download"
                TrackBarSelValueMsg2.Text = "Windows will check for updates, but will inform you when they are available; so you can choose which to download and install."
            Case 2
                TrackBarSelValueDesc2.Text = "Download updates, but let you choose which to install"
                TrackBarSelValueMsg2.Text = "This is the recommended option. When Windows downloads updates, it will inform you about available updates and will also let you choose them."
            Case 3
                TrackBarSelValueDesc2.Text = "Install updates automatically"
                TrackBarSelValueMsg2.Text = "Windows will install available updates automatically. This will make your system more up-to-date, but will not give you control over available updates."
        End Select
    End Sub

    Private Sub ComboBox10_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox10.SelectedIndexChanged
        If ComboBox10.SelectedItem = "Category View" Then
            ControlPreview.Image = New Bitmap(My.Resources.cpl_catview)
            Label76.Enabled = False
            ComboBox11.Enabled = False
        ElseIf ComboBox10.SelectedItem = "Classic View" Then
            Label76.Enabled = True
            ComboBox11.Enabled = True
            If ComboBox11.SelectedItem = "Large icons" Then
                ControlPreview.Image = New Bitmap(My.Resources.cpl_classic_largeview)
            ElseIf ComboBox11.SelectedItem = "Small icons" Then
                ControlPreview.Image = New Bitmap(My.Resources.cpl_classic_smallview)
            End If
        End If
    End Sub

    Private Sub ComboBox11_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox11.SelectedIndexChanged
        If ComboBox11.SelectedItem = "Large icons" Then
            ControlPreview.Image = New Bitmap(My.Resources.cpl_classic_largeview)
        ElseIf ComboBox11.SelectedItem = "Small icons" Then
            ControlPreview.Image = New Bitmap(My.Resources.cpl_classic_smallview)
        End If
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Close()
    End Sub

    Sub ChangePreviewColors(ColorCombination As Integer)
        If ColoredMode = 0 Then                 ' Windows 8/Server 2012 (+ Desktop Experience)
            Select Case ColorCombination
                Case 0
                    StartScreen.BackColor = Color.FromArgb(37, 37, 37)
                    StartButton.Image = New Bitmap(My.Resources.win8_0)
                Case 1
                    StartScreen.BackColor = Color.FromArgb(37, 37, 37)
                    StartButton.Image = New Bitmap(My.Resources.win8_1)
                Case 2
                    StartScreen.BackColor = Color.FromArgb(37, 37, 37)
                    StartButton.Image = New Bitmap(My.Resources.win8_2)
                Case 3
                    StartScreen.BackColor = Color.FromArgb(37, 37, 37)
                    StartButton.Image = New Bitmap(My.Resources.win8_3)
                Case 4
                    StartScreen.BackColor = Color.FromArgb(46, 23, 0)
                    StartButton.Image = New Bitmap(My.Resources.win8_4)
                Case 5
                    StartScreen.BackColor = Color.FromArgb(78, 0, 0)
                    StartButton.Image = New Bitmap(My.Resources.win8_5)
                Case 6
                    StartScreen.BackColor = Color.FromArgb(77, 0, 56)
                    StartButton.Image = New Bitmap(My.Resources.win8_6)
                Case 7
                    StartScreen.BackColor = Color.FromArgb(45, 0, 78)
                    StartButton.Image = New Bitmap(My.Resources.win8_7)
                Case 8
                    StartScreen.BackColor = Color.FromArgb(31, 0, 104)
                    StartButton.Image = New Bitmap(My.Resources.win8_8)
                Case 9
                    StartScreen.BackColor = Color.FromArgb(0, 29, 78)
                    StartButton.Image = New Bitmap(My.Resources.win8_9)
                Case 10
                    StartScreen.BackColor = Color.FromArgb(0, 77, 96)
                    StartButton.Image = New Bitmap(My.Resources.win8_10)
                Case 11
                    StartScreen.BackColor = Color.FromArgb(0, 74, 0)
                    StartButton.Image = New Bitmap(My.Resources.win8_11)
                Case 12
                    StartScreen.BackColor = Color.FromArgb(21, 153, 42)
                    StartButton.Image = New Bitmap(My.Resources.win8_12)
                Case 13
                    StartScreen.BackColor = Color.FromArgb(229, 108, 25)
                    StartButton.Image = New Bitmap(My.Resources.win8_13)
                Case 14
                    StartScreen.BackColor = Color.FromArgb(184, 27, 27)
                    StartButton.Image = New Bitmap(My.Resources.win8_14)
                Case 15
                    StartScreen.BackColor = Color.FromArgb(184, 27, 108)
                    StartButton.Image = New Bitmap(My.Resources.win8_15)
                Case 16
                    StartScreen.BackColor = Color.FromArgb(105, 27, 184)
                    StartButton.Image = New Bitmap(My.Resources.win8_16)
                Case 17
                    StartScreen.BackColor = Color.FromArgb(27, 88, 184)
                    StartButton.Image = New Bitmap(My.Resources.win8_17)
                Case 18
                    StartScreen.BackColor = Color.FromArgb(86, 156, 227)
                    StartButton.Image = New Bitmap(My.Resources.win8_18)
                Case 19
                    StartScreen.BackColor = Color.FromArgb(0, 168, 169)
                    StartButton.Image = New Bitmap(My.Resources.win8_19)
                Case 20
                    StartScreen.BackColor = Color.FromArgb(131, 186, 31)
                    StartButton.Image = New Bitmap(My.Resources.win8_20)
                Case 21
                    StartScreen.BackColor = Color.FromArgb(211, 157, 9)
                    StartButton.Image = New Bitmap(My.Resources.win8_21)
                Case 22
                    StartScreen.BackColor = Color.FromArgb(224, 100, 183)
                    StartButton.Image = New Bitmap(My.Resources.win8_22)
                Case 23
                    StartScreen.BackColor = Color.FromArgb(105, 105, 105)
                    StartButton.Image = New Bitmap(My.Resources.win8_23)
                Case 24
                    StartScreen.BackColor = Color.FromArgb(105, 105, 105)
                    StartButton.Image = New Bitmap(My.Resources.win8_24)
            End Select
        ElseIf ColoredMode = 1 Then             ' Windows 8.1/Server 2012 R2 (+ Desktop Experience)
            Select Case ColorCombination
                Case 0

                Case 1

                Case 2

                Case 3

                Case 4

                Case 5

                Case 6

                Case 7

                Case 8

                Case 9

                Case 10

                Case 11

                Case 12

                Case 13

                Case 14

                Case 15

                Case 16

                Case 17

                Case 18

                Case 19

                Case 20

                Case 21

                Case 22

                Case 23

                Case 24

            End Select
        End If
    End Sub

    Private Sub Win8Color1_Click(sender As Object, e As EventArgs) Handles Win8Color1.Click
        ChangePreviewColors(0)
    End Sub

    Private Sub Win8Color2_Click(sender As Object, e As EventArgs) Handles Win8Color2.Click
        ChangePreviewColors(1)
    End Sub

    Private Sub Win8Color3_Click(sender As Object, e As EventArgs) Handles Win8Color3.Click
        ChangePreviewColors(2)
    End Sub

    Private Sub Win8Color4_Click(sender As Object, e As EventArgs) Handles Win8Color4.Click
        ChangePreviewColors(3)
    End Sub

    Private Sub Win8Color5_Click(sender As Object, e As EventArgs) Handles Win8Color5.Click
        ChangePreviewColors(4)
    End Sub

    Private Sub Win8Color6_Click(sender As Object, e As EventArgs) Handles Win8Color6.Click
        ChangePreviewColors(5)
    End Sub

    Private Sub Win8Color7_Click(sender As Object, e As EventArgs) Handles Win8Color7.Click
        ChangePreviewColors(6)
    End Sub

    Private Sub Win8Color8_Click(sender As Object, e As EventArgs) Handles Win8Color8.Click
        ChangePreviewColors(7)
    End Sub

    Private Sub Win8Color9_Click(sender As Object, e As EventArgs) Handles Win8Color9.Click
        ChangePreviewColors(8)
    End Sub

    Private Sub Win8Color10_Click(sender As Object, e As EventArgs) Handles Win8Color10.Click
        ChangePreviewColors(9)
    End Sub

    Private Sub Win8Color11_Click(sender As Object, e As EventArgs) Handles Win8Color11.Click
        ChangePreviewColors(10)
    End Sub

    Private Sub Win8Color12_Click(sender As Object, e As EventArgs) Handles Win8Color12.Click
        ChangePreviewColors(11)
    End Sub

    Private Sub Win8Color13_Click(sender As Object, e As EventArgs) Handles Win8Color13.Click
        ChangePreviewColors(12)
    End Sub

    Private Sub Win8Color14_Click(sender As Object, e As EventArgs) Handles Win8Color14.Click
        ChangePreviewColors(13)
    End Sub

    Private Sub Win8Color15_Click(sender As Object, e As EventArgs) Handles Win8Color15.Click
        ChangePreviewColors(14)
    End Sub

    Private Sub Win8Color16_Click(sender As Object, e As EventArgs) Handles Win8Color16.Click
        ChangePreviewColors(15)
    End Sub

    Private Sub Win8Color17_Click(sender As Object, e As EventArgs) Handles Win8Color17.Click
        ChangePreviewColors(16)
    End Sub

    Private Sub Win8Color18_Click(sender As Object, e As EventArgs) Handles Win8Color18.Click
        ChangePreviewColors(17)
    End Sub

    Private Sub Win8Color19_Click(sender As Object, e As EventArgs) Handles Win8Color19.Click
        ChangePreviewColors(18)
    End Sub

    Private Sub Win8Color20_Click(sender As Object, e As EventArgs) Handles Win8Color20.Click
        ChangePreviewColors(19)
    End Sub

    Private Sub Win8Color21_Click(sender As Object, e As EventArgs) Handles Win8Color21.Click
        ChangePreviewColors(20)
    End Sub

    Private Sub Win8Color22_Click(sender As Object, e As EventArgs) Handles Win8Color22.Click
        ChangePreviewColors(21)
    End Sub

    Private Sub Win8Color23_Click(sender As Object, e As EventArgs) Handles Win8Color23.Click
        ChangePreviewColors(22)
    End Sub

    Private Sub Win8Color24_Click(sender As Object, e As EventArgs) Handles Win8Color24.Click
        ChangePreviewColors(23)
    End Sub

    Private Sub Win8Color25_Click(sender As Object, e As EventArgs) Handles Win8Color25.Click
        ChangePreviewColors(24)
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        If RadioButton3.Checked Then
            Label17.Enabled = True
            ComboBox2.Enabled = True
            Label18.Enabled = True
            GenericBox1.Enabled = True
            GenericBox2.Enabled = True
            GenericBox3.Enabled = True
            GenericBox4.Enabled = True
            GenericBox5.Enabled = True
            GenericDash1.Enabled = True
            GenericDash2.Enabled = True
            GenericDash3.Enabled = True
            GenericDash4.Enabled = True
            Label19.Enabled = False
            KeyInputBox1.Enabled = False
            KeyInputBox2.Enabled = False
            KeyInputBox3.Enabled = False
            KeyInputBox4.Enabled = False
            KeyInputBox5.Enabled = False
            KeyDash1.Enabled = False
            KeyDash2.Enabled = False
            KeyDash3.Enabled = False
            KeyDash4.Enabled = False
            Label20.Enabled = False
            Label21.Enabled = False
            If ComboBox2.SelectedItem = "" Then
                Next_Button.Enabled = False
            Else
                Next_Button.Enabled = True
            End If
        Else
            Label17.Enabled = False
            ComboBox2.Enabled = False
            Label18.Enabled = False
            GenericBox1.Enabled = False
            GenericBox2.Enabled = False
            GenericBox3.Enabled = False
            GenericBox4.Enabled = False
            GenericBox5.Enabled = False
            GenericDash1.Enabled = False
            GenericDash2.Enabled = False
            GenericDash3.Enabled = False
            GenericDash4.Enabled = False
            Label19.Enabled = True
            KeyInputBox1.Enabled = True
            KeyInputBox2.Enabled = True
            KeyInputBox3.Enabled = True
            KeyInputBox4.Enabled = True
            KeyInputBox5.Enabled = True
            KeyDash1.Enabled = True
            KeyDash2.Enabled = True
            KeyDash3.Enabled = True
            KeyDash4.Enabled = True
            Label20.Enabled = True
            Label21.Enabled = True
            ' Concatenate key boxes
            KeyString = KeyInputBox1.Text & "-" & KeyInputBox2.Text & "-" & KeyInputBox3.Text & "-" & KeyInputBox4.Text & "-" & KeyInputBox5.Text
            If KeyString.Length < 29 Then
                Next_Button.Enabled = False
            Else
                Next_Button.Enabled = True
            End If
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedItem = "" Then
            Next_Button.Enabled = False
        Else
            Next_Button.Enabled = True
        End If
        ChangeGenericKey()
    End Sub

    Private Sub FontChange(sender As Object, e As EventArgs) Handles FontFamilyTSCB.SelectedIndexChanged, FontSizeTSCB.SelectedIndexChanged
        ' Change Scintilla editor font
        InitScintilla(FontFamilyTSCB.SelectedItem, FontSizeTSCB.SelectedItem)
    End Sub

    Private Sub ProductKeyChanged(sender As Object, e As EventArgs) Handles KeyInputBox1.TextChanged, KeyInputBox2.TextChanged, KeyInputBox3.TextChanged, KeyInputBox4.TextChanged, KeyInputBox5.TextChanged
        KeyString = KeyInputBox1.Text & "-" & KeyInputBox2.Text & "-" & KeyInputBox3.Text & "-" & KeyInputBox4.Text & "-" & KeyInputBox5.Text
        If KeyString.Length < 29 Then
            Next_Button.Enabled = False
        Else
            Next_Button.Enabled = True
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text = "" Then
            Next_Button.Enabled = False
        Else
            If TextBox1.Text.Contains(".") Or _
                TextBox1.Text.Contains("\") Or _
                TextBox1.Text.Contains("/") Or _
                TextBox1.Text.Contains(":") Or _
                TextBox1.Text.Contains("*") Or _
                TextBox1.Text.Contains("?") Or _
                TextBox1.Text.Contains(Quote) Or _
                TextBox1.Text.Contains("<") Or _
                TextBox1.Text.Contains(">") Or _
                TextBox1.Text.Contains("|") Or _
                TextBox1.Text.Contains(",") Or _
                TextBox1.Text.Contains("~") Or _
                TextBox1.Text.Contains("!") Or _
                TextBox1.Text.Contains("@") Or _
                TextBox1.Text.Contains("#") Or _
                TextBox1.Text.Contains("$") Or _
                TextBox1.Text.Contains("%") Or _
                TextBox1.Text.Contains("^") Or _
                TextBox1.Text.Contains("&") Or _
                TextBox1.Text.Contains("`") Or _
                TextBox1.Text.Contains("(") Or TextBox1.Text.Contains(")") Or _
                TextBox1.Text.Contains("{") Or TextBox1.Text.Contains("}") Or _
                TextBox1.Text.Contains("_") Then
                Next_Button.Enabled = False
            Else
                Next_Button.Enabled = True
            End If
        End If
    End Sub

    Private Sub RegionalSettingsChanged(sender As Object, e As EventArgs) Handles ComboBox5.SelectedIndexChanged, ComboBox6.SelectedIndexChanged, ComboBox7.SelectedIndexChanged, ComboBox8.SelectedIndexChanged
        If ComboBox5.SelectedItem = "" Or ComboBox6.SelectedItem = "" Or ComboBox7.SelectedItem = "" Or ComboBox8.SelectedItem = "" Then
            Next_Button.Enabled = False
        Else
            Next_Button.Enabled = True
        End If
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        If TextBox4.Text = "" Then
            Next_Button.Enabled = False
        Else
            Next_Button.Enabled = True
        End If
    End Sub

    Private Sub KeyCopyButton_Click(sender As Object, e As EventArgs) Handles KeyCopyButton.Click
        If KeyString.Length >= 29 Then
            Dim data As New DataObject()
            data.SetText(KeyString, TextDataFormat.Text)
            Clipboard.SetDataObject(data, True)
            Dim notify As New NotifyIcon()
            notify.Visible = True
            notify.Icon = MainForm.Icon
            notify.BalloonTipText = "To paste this key in a virtual machine, use the keyboard shortcut provided by the VM solution"
            notify.BalloonTipTitle = "The key has been copied to the clipboard"
            notify.ShowBalloonTip(3000)
        End If
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