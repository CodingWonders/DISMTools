Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports System.Threading
Imports ScintillaNET
Imports DISMTools.Elements
Imports Microsoft.Dism

Public Class NewUnattendWiz

    ' Declare initial vars
    Dim IsInExpress As Boolean = True
    Dim CurrentWizardPage As New UnattendedWizardPage()
    Dim VerifyInPages As New List(Of UnattendedWizardPage.Page)

    ' Regional Settings Page
    Dim ImageLanguages As New List(Of ImageLanguage)
    Dim UserLocales As New List(Of UserLocale)
    Dim KeyboardIdentifiers As New List(Of KeyboardIdentifier)
    Dim GeoIds As New List(Of GeoId)
    Dim RegionalInteractive As Boolean
    Dim SelectedLanguage As New ImageLanguage()
    Dim SelectedLocale As New UserLocale()
    Dim SelectedKeybIdentifier As New KeyboardIdentifier()
    Dim SelectedGeoId As New GeoId()

    ' System Configuration Page
    Dim SelectedArchitecture As New DismProcessorArchitecture()
    Dim Win11Config As New SVSettings()
    Dim PCName As New ComputerName()

    ' Time Zone Panel
    Dim TimeOffsets As New List(Of TimeOffset)
    Dim TimeOffsetInteractive As Boolean
    Dim SelectedOffset As New TimeOffset()

    ' Disk Configuration Panel
    Dim DiskConfigurationInteractive As Boolean
    Dim SelectedDiskConfiguration As New DiskConfiguration()

    ' Space for more pages

    ' Default Settings
    Dim DefaultLanguage As New ImageLanguage()
    Dim DefaultLocale As New UserLocale()
    Dim DefaultKeybIdentifier As New KeyboardIdentifier()
    Dim DefaultGeoId As New GeoId()
    Dim DefaultOffset As New TimeOffset()
    Dim DefaultDiskConfiguration As New DiskConfiguration()


    ''' <summary>
    ''' Initializes the Scintilla editor
    ''' </summary>
    ''' <param name="fntName">The name of the font used in the Scintilla editor</param>
    ''' <param name="fntSize">The size of the font used in the Scintilla editor</param>
    ''' <remarks></remarks>
    Sub InitScintilla(fntName As String, fntSize As Integer)
        ' Initialize Scintilla editor
        Scintilla1.StyleResetDefault()
        Scintilla2.StyleResetDefault()
        ' Use VS's selection color, as I find it the most natural
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Scintilla1.SelectionBackColor = Color.FromArgb(38, 79, 120)
            Scintilla2.SelectionBackColor = Color.FromArgb(38, 79, 120)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Scintilla1.SelectionBackColor = Color.FromArgb(153, 201, 239)
            Scintilla2.SelectionBackColor = Color.FromArgb(153, 201, 239)
        End If
        Scintilla1.Styles(Style.Default).Font = fntName
        Scintilla1.Styles(Style.Default).Size = fntSize
        Scintilla2.Styles(Style.Default).Font = fntName
        Scintilla2.Styles(Style.Default).Size = fntSize

        ' Set background and foreground colors (from Visual Studio)
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Scintilla1.Styles(Style.Default).BackColor = Color.FromArgb(30, 30, 30)
            Scintilla1.Styles(Style.Default).ForeColor = Color.White
            Scintilla1.Styles(Style.LineNumber).BackColor = Color.FromArgb(30, 30, 30)
            Scintilla2.Styles(Style.Default).BackColor = Color.FromArgb(30, 30, 30)
            Scintilla2.Styles(Style.Default).ForeColor = Color.White
            Scintilla2.Styles(Style.LineNumber).BackColor = Color.FromArgb(30, 30, 30)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Scintilla1.Styles(Style.Default).BackColor = Color.White
            Scintilla1.Styles(Style.Default).ForeColor = Color.Black
            Scintilla1.Styles(Style.LineNumber).BackColor = Color.White
            Scintilla2.Styles(Style.Default).BackColor = Color.White
            Scintilla2.Styles(Style.Default).ForeColor = Color.Black
            Scintilla2.Styles(Style.LineNumber).BackColor = Color.White
        End If
        Scintilla1.StyleClearAll()
        Scintilla2.StyleClearAll()

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
            Scintilla2.Styles(Style.LineNumber).BackColor = Color.FromArgb(30, 30, 30)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Scintilla1.Styles(Style.LineNumber).BackColor = Color.White
            Scintilla2.Styles(Style.LineNumber).BackColor = Color.White
        End If
        Scintilla1.Styles(Style.LineNumber).ForeColor = Color.FromArgb(165, 165, 165)
        Scintilla2.Styles(Style.LineNumber).ForeColor = Color.FromArgb(165, 165, 165)
        Dim Margin = Scintilla1.Margins(1)
        Margin.Width = 30
        Margin.Type = MarginType.Number
        Margin.Sensitive = True
        Margin.Mask = 0
        Margin = Scintilla2.Margins(1)
        Margin.Width = 30
        Margin.Type = MarginType.Number
        Margin.Sensitive = True
        Margin.Mask = 0

        ' Initialize code folding
        Scintilla1.SetFoldMarginColor(True, Scintilla1.Styles(Style.Default).BackColor)
        Scintilla1.SetFoldMarginColor(True, Scintilla1.Styles(Style.Default).BackColor)
        Scintilla1.SetProperty("fold", "1")
        Scintilla1.SetProperty("fold.compact", "1")
        Scintilla2.SetFoldMarginColor(True, Scintilla1.Styles(Style.Default).BackColor)
        Scintilla2.SetFoldMarginColor(True, Scintilla1.Styles(Style.Default).BackColor)
        Scintilla2.SetProperty("fold", "1")
        Scintilla2.SetProperty("fold.compact", "1")

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
        Bookmarks = Scintilla2.Margins(2)
        Bookmarks.Width = 20
        Bookmarks.Sensitive = True
        Bookmarks.Type = MarginType.Symbol
        Bookmarks.Mask = (1 << 2)
        Marker = Scintilla2.Markers(2)
        Marker.Symbol = MarkerSymbol.Circle
        Marker.SetBackColor(Color.FromArgb(255, 0, 59))
        Marker.SetForeColor(Color.Black)
        Marker.SetAlpha(100)

        ' Set editor caret settings
        Scintilla1.CaretForeColor = ForeColor
        Scintilla2.CaretForeColor = ForeColor


        ' Configure code folding margins
        Scintilla1.Margins(3).Type = MarginType.Symbol
        Scintilla1.Margins(3).Mask = Marker.MaskFolders
        Scintilla1.Margins(3).Sensitive = True
        Scintilla1.Margins(3).Width = 1
        Scintilla2.Margins(3).Type = MarginType.Symbol
        Scintilla2.Margins(3).Mask = Marker.MaskFolders
        Scintilla2.Margins(3).Sensitive = True
        Scintilla2.Margins(3).Width = 1

        ' Set colors for all folding markers
        For x = 25 To 31
            Scintilla1.Markers(x).SetForeColor(Scintilla1.Styles(Style.Default).BackColor)
            Scintilla1.Markers(x).SetBackColor(Scintilla1.Styles(Style.Default).ForeColor)
            Scintilla2.Markers(x).SetForeColor(Scintilla1.Styles(Style.Default).BackColor)
            Scintilla2.Markers(x).SetBackColor(Scintilla1.Styles(Style.Default).ForeColor)
        Next

        ' Folding marker configuration
        Scintilla1.Markers(Marker.Folder).Symbol = MarkerSymbol.BoxPlus
        Scintilla1.Markers(Marker.FolderOpen).Symbol = MarkerSymbol.BoxMinus
        Scintilla1.Markers(Marker.FolderEnd).Symbol = MarkerSymbol.BoxPlusConnected
        Scintilla1.Markers(Marker.FolderMidTail).Symbol = MarkerSymbol.TCorner
        Scintilla1.Markers(Marker.FolderOpenMid).Symbol = MarkerSymbol.BoxMinusConnected
        Scintilla1.Markers(Marker.FolderSub).Symbol = MarkerSymbol.VLine
        Scintilla1.Markers(Marker.FolderTail).Symbol = MarkerSymbol.LCorner
        Scintilla2.Markers(Marker.Folder).Symbol = MarkerSymbol.BoxPlus
        Scintilla2.Markers(Marker.FolderOpen).Symbol = MarkerSymbol.BoxMinus
        Scintilla2.Markers(Marker.FolderEnd).Symbol = MarkerSymbol.BoxPlusConnected
        Scintilla2.Markers(Marker.FolderMidTail).Symbol = MarkerSymbol.TCorner
        Scintilla2.Markers(Marker.FolderOpenMid).Symbol = MarkerSymbol.BoxMinusConnected
        Scintilla2.Markers(Marker.FolderSub).Symbol = MarkerSymbol.VLine
        Scintilla2.Markers(Marker.FolderTail).Symbol = MarkerSymbol.LCorner

        ' Enable folding
        Scintilla1.AutomaticFold = (AutomaticFold.Show Or AutomaticFold.Click Or AutomaticFold.Show)
        Scintilla2.AutomaticFold = (AutomaticFold.Show Or AutomaticFold.Click Or AutomaticFold.Show)
    End Sub

    Sub SetDefaultSettings()
        DefaultLanguage.Id = "en-US"
        DefaultLanguage.DisplayName = "English"
        DefaultLocale.Id = "en-US"
        DefaultLocale.DisplayName = "English (United States"
        DefaultLocale.LCID = "0409"
        DefaultLocale.KeybId = "00000409"
        DefaultLocale.GeoLoc = "244"
        DefaultKeybIdentifier.Id = "00000409"
        DefaultKeybIdentifier.DisplayName = "US"
        DefaultKeybIdentifier.Type = "Keyboard"
        DefaultGeoId.Id = "244"
        DefaultGeoId.DisplayName = "United States"
        DefaultOffset.Id = "UTC"
        DefaultOffset.DisplayName = "(UTC) Coordinated Universal Time"
        DefaultDiskConfiguration.DiskConfigMode = DiskConfigurationMode.AutoDisk0
        DefaultDiskConfiguration.PartStyle = PartitionStyle.GPT
        DefaultDiskConfiguration.ESPSize = 300
        DefaultDiskConfiguration.InstallRecEnv = True
        DefaultDiskConfiguration.RecEnvPartition = RecoveryEnvironmentLocation.WinREPartition
        DefaultDiskConfiguration.RecEnvSize = 1000
        DefaultDiskConfiguration.DiskPartScriptConfig.ScriptContents = ""
        DefaultDiskConfiguration.DiskPartScriptConfig.AutomaticInstall = True
        DefaultDiskConfiguration.DiskPartScriptConfig.TargetDisk.DiskNum = 0
        DefaultDiskConfiguration.DiskPartScriptConfig.TargetDisk.PartNum = 3

        SelectedLanguage = DefaultLanguage
        SelectedLocale = DefaultLocale
        SelectedKeybIdentifier = DefaultKeybIdentifier
        SelectedGeoId = DefaultGeoId
        SelectedOffset = DefaultOffset
        SelectedDiskConfiguration = DefaultDiskConfiguration

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

        StepsContainer.Visible = MainForm.EnableExperiments
        ' Go to schneegans generator for now
        If Not MainForm.EnableExperiments Then
            Process.Start("https://schneegans.de/windows/unattend-generator/")
            MessageBox.Show("The unattended answer file tasks are undergoing a major reconstruction." & If(MainForm.dtBranch.Contains("stable"), CrLf & CrLf & "The reconstruction is only happening in the preview releases.", ""))
        Else
            SetDefaultSettings()
            ' System language
            If File.Exists(Application.StartupPath & "\AutoUnattend\ImageLanguage.xml") Then
                ImageLanguages = ImageLanguage.LoadItems(Application.StartupPath & "\AutoUnattend\ImageLanguage.xml")
                If ImageLanguages IsNot Nothing Then
                    For Each imgLang As ImageLanguage In ImageLanguages
                        ComboBox1.Items.Add(imgLang.DisplayName)
                    Next
                    If ComboBox1.SelectedItem = Nothing Then ComboBox1.SelectedItem = DefaultLanguage.DisplayName
                End If
            End If
            ' System locale
            If File.Exists(Application.StartupPath & "\AutoUnattend\UserLocale.xml") Then
                UserLocales = UserLocale.LoadItems(Application.StartupPath & "\AutoUnattend\UserLocale.xml")
                If UserLocales IsNot Nothing Then
                    For Each userLoc As UserLocale In UserLocales
                        ComboBox2.Items.Add(userLoc.DisplayName)
                    Next
                    If ComboBox2.SelectedItem = Nothing Then ComboBox2.SelectedItem = DefaultLocale.DisplayName
                End If
            End If
            ' Keyboard layout/IME
            If File.Exists(Application.StartupPath & "\AutoUnattend\KeyboardIdentifier.xml") Then
                KeyboardIdentifiers = KeyboardIdentifier.LoadItems(Application.StartupPath & "\AutoUnattend\KeyboardIdentifier.xml")
                If KeyboardIdentifiers IsNot Nothing Then
                    For Each keyb As KeyboardIdentifier In KeyboardIdentifiers
                        ComboBox3.Items.Add(keyb.DisplayName)
                    Next
                    If ComboBox3.SelectedItem = Nothing Then ComboBox3.SelectedItem = DefaultKeybIdentifier.DisplayName
                End If
            End If
            ' Home location
            If File.Exists(Application.StartupPath & "\AutoUnattend\GeoId.xml") Then
                GeoIds = GeoId.LoadItems(Application.StartupPath & "\AutoUnattend\GeoId.xml")
                If GeoIds IsNot Nothing Then
                    For Each Geo As GeoId In GeoIds
                        ComboBox4.Items.Add(Geo.DisplayName)
                    Next
                    If ComboBox4.SelectedItem = Nothing Then ComboBox4.SelectedItem = DefaultGeoId.DisplayName
                End If
            End If
            ' Time offsets
            If File.Exists(Application.StartupPath & "\AutoUnattend\TimeOffset.xml") Then
                TimeOffsets = TimeOffset.LoadItems(Application.StartupPath & "\AutoUnattend\TimeOffset.xml")
                If TimeOffsets IsNot Nothing Then
                    For Each Offset As TimeOffset In TimeOffsets
                        ComboBox5.Items.Add(Offset.DisplayName)
                    Next
                    If ComboBox5.SelectedItem = Nothing Then ComboBox5.SelectedItem = DefaultOffset.DisplayName
                End If
            End If
            ListBox1.SelectedIndex = 1
            ChangePage(UnattendedWizardPage.Page.DisclaimerPage)
            VerifyInPages.AddRange(New UnattendedWizardPage.Page() {UnattendedWizardPage.Page.SysConfigPage})
            TimeZonePageTimer.Enabled = True
            ' Modify script contents of disk config for sample DP Script
            SelectedDiskConfiguration.DiskPartScriptConfig.ScriptContents = Scintilla2.Text
        End If
    End Sub

    Sub ChangePage(NewPage As UnattendedWizardPage.Page)
        If VerifyInPages.Contains(CurrentWizardPage.WizardPage) Then
            If Not VerifyOptionsInPage(CurrentWizardPage.WizardPage) Then Exit Sub
        End If
        Select Case NewPage
            Case UnattendedWizardPage.Page.DisclaimerPage
                DisclaimerPanel.Visible = True
                RegionalSettingsPanel.Visible = False
                SysConfigPanel.Visible = False
                TimeZonePanel.Visible = False
                DiskConfigurationPanel.Visible = False
            Case UnattendedWizardPage.Page.RegionalPage
                DisclaimerPanel.Visible = False
                RegionalSettingsPanel.Visible = True
                SysConfigPanel.Visible = False
                TimeZonePanel.Visible = False
                DiskConfigurationPanel.Visible = False
            Case UnattendedWizardPage.Page.SysConfigPage
                DisclaimerPanel.Visible = False
                RegionalSettingsPanel.Visible = False
                SysConfigPanel.Visible = True
                TimeZonePanel.Visible = False
                DiskConfigurationPanel.Visible = False
            Case UnattendedWizardPage.Page.TimeZonePage
                DisclaimerPanel.Visible = False
                RegionalSettingsPanel.Visible = False
                SysConfigPanel.Visible = False
                TimeZonePanel.Visible = True
                DiskConfigurationPanel.Visible = False
            Case UnattendedWizardPage.Page.DiskConfigPage
                DisclaimerPanel.Visible = False
                RegionalSettingsPanel.Visible = False
                SysConfigPanel.Visible = False
                TimeZonePanel.Visible = False
                DiskConfigurationPanel.Visible = True
        End Select
        CurrentWizardPage.WizardPage = NewPage
        Next_Button.Enabled = Not (NewPage + 1 >= UnattendedWizardPage.PageCount)
        Back_Button.Enabled = Not (NewPage = UnattendedWizardPage.Page.DisclaimerPage)
    End Sub

    Function VerifyOptionsInPage(WizardPage As UnattendedWizardPage.Page) As Boolean
        Select Case WizardPage
            Case UnattendedWizardPage.Page.SysConfigPage
                If ListBox1.SelectedItems.Count = 0 Then
                    MessageBox.Show("Please select an architecture and try again", "Validation error")
                    Return False
                End If
                If Not PCName.DefaultName Then
                    Dim testerPC As ComputerName = ComputerNameValidator.ValidateComputerName(TextBox1.Text)
                    If Not testerPC.Valid AndAlso testerPC.ErrorMessage <> "" Then
                        MessageBox.Show(testerPC.ErrorMessage, "Computer name error")
                        Return False
                    End If
                End If
        End Select
        Return True
    End Function

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
        ChangePage(CurrentWizardPage.WizardPage - 1)
    End Sub

    Private Sub Next_Button_Click(sender As Object, e As EventArgs) Handles Next_Button.Click
        ChangePage(CurrentWizardPage.WizardPage + 1)
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

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        RegionalInteractive = Not RadioButton1.Checked
        RegionalSettings.Enabled = RadioButton1.Checked
        Label10.Enabled = Not RadioButton1.Checked
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        SelectedLanguage = ImageLanguages(ComboBox1.SelectedIndex)
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        SelectedLocale = UserLocales(ComboBox2.SelectedIndex)
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        SelectedKeybIdentifier = KeyboardIdentifiers(ComboBox3.SelectedIndex)
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        SelectedGeoId = GeoIds(ComboBox4.SelectedIndex)
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Select Case ListBox1.SelectedIndex
            Case 0
                SelectedArchitecture = DismProcessorArchitecture.Intel
            Case 1
                SelectedArchitecture = DismProcessorArchitecture.AMD64
            Case 2
                SelectedArchitecture = DismProcessorArchitecture.ARM64
        End Select
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        PCName.DefaultName = CheckBox3.Checked
        ComputerNamePanel.Enabled = Not CheckBox3.Checked
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Win11Config.LabConfig_BypassRequirements = CheckBox1.Checked
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        Win11Config.OOBE_BypassNRO = CheckBox2.Checked
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        ' Hold default value for now
        Dim defVal As Boolean = False
        defVal = PCName.DefaultName
        PCName = ComputerNameValidator.ValidateComputerName(TextBox1.Text)
        PCName.DefaultName = defVal
        If Not PCName.Valid AndAlso PCName.ErrorMessage <> "" Then
            MessageBox.Show(PCName.ErrorMessage, "Computer name error")
        End If
    End Sub

    Private Sub TimeZonePageTimer_Tick(sender As Object, e As EventArgs) Handles TimeZonePageTimer.Tick
        Dim UTC As Date = Date.UtcNow
        Dim SelTZ As Date = Date.UtcNow
        Dim tz As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(TimeOffsets(ComboBox5.SelectedIndex).Id)
        SelTZ = TimeZoneInfo.ConvertTimeFromUtc(SelTZ, tz)
        CurrentTimeUTC.Text = UTC.ToString("D") & " - " & UTC.ToString("HH:mm")
        CurrentTimeSelTZ.Text = SelTZ.ToString("D") & " - " & SelTZ.ToString("HH:mm")
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        TimeOffsetInteractive = RadioButton3.Checked
        TimeZoneSettings.Enabled = Not RadioButton3.Checked
    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox5.SelectedIndexChanged
        SelectedOffset = TimeOffsets(ComboBox5.SelectedIndex)
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        ManualPartPanel.Enabled = Not CheckBox4.Checked
        DiskConfigurationInteractive = CheckBox4.Checked
    End Sub

    Private Sub RadioButton5_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton5.CheckedChanged
        AutoDiskConfigPanel.Enabled = RadioButton5.Checked
        DiskPartPanel.Enabled = Not RadioButton5.Checked
        SelectedDiskConfiguration.DiskConfigMode = If(RadioButton5.Checked, DiskConfigurationMode.AutoDisk0, DiskConfigurationMode.DiskPart)
    End Sub

    Private Sub RadioButton7_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton7.CheckedChanged
        ESPPanel.Enabled = RadioButton7.Checked
        SelectedDiskConfiguration.PartStyle = If(RadioButton7.Checked, PartitionStyle.GPT, PartitionStyle.MBR)
    End Sub

    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        WindowsREPanel.Enabled = CheckBox5.Checked
        SelectedDiskConfiguration.InstallRecEnv = CheckBox5.Checked
    End Sub

    Private Sub RadioButton9_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton9.CheckedChanged
        RESizePanel.Enabled = RadioButton9.Checked
        SelectedDiskConfiguration.RecEnvPartition = If(RadioButton9.Checked, RecoveryEnvironmentLocation.WinREPartition, RecoveryEnvironmentLocation.WindowsPartition)
    End Sub

    Private Sub RadioButton11_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton11.CheckedChanged
        ManualInstallPanel.Enabled = Not RadioButton11.Checked
        SelectedDiskConfiguration.DiskPartScriptConfig.AutomaticInstall = RadioButton11.Checked
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        SelectedDiskConfiguration.ESPSize = NumericUpDown1.Value
    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown2.ValueChanged
        SelectedDiskConfiguration.RecEnvSize = NumericUpDown2.Value
    End Sub

    Private Sub NumericUpDown3_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown3.ValueChanged
        SelectedDiskConfiguration.DiskPartScriptConfig.TargetDisk.DiskNum = NumericUpDown3.Value
    End Sub

    Private Sub NumericUpDown4_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown4.ValueChanged
        SelectedDiskConfiguration.DiskPartScriptConfig.TargetDisk.PartNum = NumericUpDown4.Value
    End Sub

    Private Sub Scintilla2_TextChanged(sender As Object, e As EventArgs) Handles Scintilla2.TextChanged
        SelectedDiskConfiguration.DiskPartScriptConfig.ScriptContents = Scintilla2.Text
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        Scintilla2.Text = File.ReadAllText(OpenFileDialog1.FileName)
    End Sub
End Class