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
    Dim TimeOffsetInteractive As Boolean = True
    Dim SelectedOffset As New TimeOffset()

    ' Disk Configuration Panel
    Dim DiskConfigurationInteractive As Boolean = True
    Dim SelectedDiskConfiguration As New DiskConfiguration()

    ' Product Key Panel
    Dim GenericChosen As Boolean = True
    Dim GenericKeys As New List(Of ProductKey)
    Dim SelectedKey As New ProductKey()

    ' User Accounts Panel
    Dim UserAccountsInteractive As Boolean = True
    Dim UserAccountsList As New List(Of User)
    Dim AutoLogon As New AutoLogonSettings()
    Dim PasswordObfuscate As Boolean
    Dim SelectedExpirationSettings As New PasswordExpirationSettings()
    Dim SelectedLockdownSettings As New AccountLockdownSettings()

    ' Space for more pages

    ' Default Settings
    Dim DefaultLanguage As New ImageLanguage()
    Dim DefaultLocale As New UserLocale()
    Dim DefaultKeybIdentifier As New KeyboardIdentifier()
    Dim DefaultGeoId As New GeoId()
    Dim DefaultOffset As New TimeOffset()
    Dim DefaultDiskConfiguration As New DiskConfiguration()
    Dim DefaultExpirationSettings As New PasswordExpirationSettings()
    Dim DefaultLockdownSettings As New AccountLockdownSettings()


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

    Function NewKeyVar(key As String) As ProductKey
        Dim pKey As New ProductKey()
        pKey.Valid = True
        pKey.Key = key
        Return pKey
    End Function

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

        GenericKeys.Add(NewKeyVar("YNMGQ-8RYV3-4PGQ3-C8XTP-7CFBY"))     ' Education
        GenericKeys.Add(NewKeyVar("84NGF-MHBT6-FXBX8-QWJK7-DRR8H"))     ' Education N
        GenericKeys.Add(NewKeyVar("YTMG3-N6DKC-DKB77-7M9GH-8HVX7"))     ' Home
        GenericKeys.Add(NewKeyVar("4CPRK-NM3K3-X6XXQ-RXX86-WXCHW"))     ' Home N
        GenericKeys.Add(NewKeyVar("BT79Q-G7N6G-PGBYW-4YWX6-6F4BT"))     ' Home Simple Language
        GenericKeys.Add(NewKeyVar("VK7JG-NPHTM-C97JM-9MPGT-3V66T"))     ' Pro
        GenericKeys.Add(NewKeyVar("8PTT6-RNW4C-6V7J2-C2D3X-MHBPB"))     ' Pro Education
        GenericKeys.Add(NewKeyVar("GJTYN-HDMQY-FRR76-HVGC7-QPF8P"))     ' Pro Education N
        GenericKeys.Add(NewKeyVar("DXG7C-N36C4-C4HTG-X4T3X-2YV77"))     ' Pro for Workstations
        GenericKeys.Add(NewKeyVar("2B87N-8KFHP-DKV6R-Y2C8J-PKCKT"))     ' Pro N
        GenericKeys.Add(NewKeyVar("WYPNQ-8C467-V2W6J-TX4WX-WT2RQ"))     ' Pro N for Workstations

        UserAccountsList.Add(New User(True, "Admin", "", UserGroup.Administrators))
        For i = 1 To 4
            UserAccountsList.Add(New User(False, "", "", UserGroup.Users))
        Next

        DefaultExpirationSettings.Mode = PasswordExpirationMode.NIST_Unlimited
        DefaultExpirationSettings.Days = 42
        DefaultLockdownSettings.Enabled = True
        DefaultLockdownSettings.DefaultPolicy = True
        DefaultLockdownSettings.TimedLockdownSettings.FailedAttempts = 10
        DefaultLockdownSettings.TimedLockdownSettings.Timeframe = 10
        DefaultLockdownSettings.TimedLockdownSettings.AutoUnlockTime = 10


        SelectedLanguage = DefaultLanguage
        SelectedLocale = DefaultLocale
        SelectedKeybIdentifier = DefaultKeybIdentifier
        SelectedGeoId = DefaultGeoId
        SelectedOffset = DefaultOffset
        SelectedDiskConfiguration = DefaultDiskConfiguration
        SelectedKey = GenericKeys(5)
        SelectedExpirationSettings = DefaultExpirationSettings
        SelectedLockdownSettings = DefaultLockdownSettings

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
            VerifyInPages.AddRange(New UnattendedWizardPage.Page() {UnattendedWizardPage.Page.SysConfigPage, UnattendedWizardPage.Page.DiskConfigPage, UnattendedWizardPage.Page.ProductKeyPage, UnattendedWizardPage.Page.UserAccountsPage})
            TimeZonePageTimer.Enabled = True
            ' Modify script contents of disk config for sample DP Script
            SelectedDiskConfiguration.DiskPartScriptConfig.ScriptContents = Scintilla2.Text
            ' Set PRO edition
            ComboBox6.SelectedItem = "Pro"
        End If
    End Sub

    Sub ChangePage(NewPage As UnattendedWizardPage.Page)
        If NewPage > CurrentWizardPage.WizardPage AndAlso VerifyInPages.Contains(CurrentWizardPage.WizardPage) Then
            If Not VerifyOptionsInPage(CurrentWizardPage.WizardPage) Then Exit Sub
        End If
        Select Case NewPage
            Case UnattendedWizardPage.Page.DisclaimerPage
                DisclaimerPanel.Visible = True
                RegionalSettingsPanel.Visible = False
                SysConfigPanel.Visible = False
                TimeZonePanel.Visible = False
                DiskConfigurationPanel.Visible = False
                ProductKeyPanel.Visible = False
                UserAccountPanel.Visible = False
                PWExpirationPanel.Visible = False
                AccountLockdownPanel.Visible = False
            Case UnattendedWizardPage.Page.RegionalPage
                DisclaimerPanel.Visible = False
                RegionalSettingsPanel.Visible = True
                SysConfigPanel.Visible = False
                TimeZonePanel.Visible = False
                DiskConfigurationPanel.Visible = False
                ProductKeyPanel.Visible = False
                UserAccountPanel.Visible = False
                PWExpirationPanel.Visible = False
                AccountLockdownPanel.Visible = False
            Case UnattendedWizardPage.Page.SysConfigPage
                DisclaimerPanel.Visible = False
                RegionalSettingsPanel.Visible = False
                SysConfigPanel.Visible = True
                TimeZonePanel.Visible = False
                DiskConfigurationPanel.Visible = False
                ProductKeyPanel.Visible = False
                UserAccountPanel.Visible = False
                PWExpirationPanel.Visible = False
                AccountLockdownPanel.Visible = False
            Case UnattendedWizardPage.Page.TimeZonePage
                DisclaimerPanel.Visible = False
                RegionalSettingsPanel.Visible = False
                SysConfigPanel.Visible = False
                TimeZonePanel.Visible = True
                DiskConfigurationPanel.Visible = False
                ProductKeyPanel.Visible = False
                UserAccountPanel.Visible = False
                PWExpirationPanel.Visible = False
                AccountLockdownPanel.Visible = False
            Case UnattendedWizardPage.Page.DiskConfigPage
                DisclaimerPanel.Visible = False
                RegionalSettingsPanel.Visible = False
                SysConfigPanel.Visible = False
                TimeZonePanel.Visible = False
                DiskConfigurationPanel.Visible = True
                ProductKeyPanel.Visible = False
                UserAccountPanel.Visible = False
                PWExpirationPanel.Visible = False
                AccountLockdownPanel.Visible = False
            Case UnattendedWizardPage.Page.ProductKeyPage
                DisclaimerPanel.Visible = False
                RegionalSettingsPanel.Visible = False
                SysConfigPanel.Visible = False
                TimeZonePanel.Visible = False
                DiskConfigurationPanel.Visible = False
                ProductKeyPanel.Visible = True
                UserAccountPanel.Visible = False
                PWExpirationPanel.Visible = False
                AccountLockdownPanel.Visible = False
            Case UnattendedWizardPage.Page.UserAccountsPage
                DisclaimerPanel.Visible = False
                RegionalSettingsPanel.Visible = False
                SysConfigPanel.Visible = False
                TimeZonePanel.Visible = False
                DiskConfigurationPanel.Visible = False
                ProductKeyPanel.Visible = False
                UserAccountPanel.Visible = True
                PWExpirationPanel.Visible = False
                AccountLockdownPanel.Visible = False
            Case UnattendedWizardPage.Page.PWExpirationPage
                DisclaimerPanel.Visible = False
                RegionalSettingsPanel.Visible = False
                SysConfigPanel.Visible = False
                TimeZonePanel.Visible = False
                DiskConfigurationPanel.Visible = False
                ProductKeyPanel.Visible = False
                UserAccountPanel.Visible = False
                PWExpirationPanel.Visible = True
                AccountLockdownPanel.Visible = False
            Case UnattendedWizardPage.Page.AccountLockdownPage
                DisclaimerPanel.Visible = False
                RegionalSettingsPanel.Visible = False
                SysConfigPanel.Visible = False
                TimeZonePanel.Visible = False
                DiskConfigurationPanel.Visible = False
                ProductKeyPanel.Visible = False
                UserAccountPanel.Visible = False
                PWExpirationPanel.Visible = False
                AccountLockdownPanel.Visible = True
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
            Case UnattendedWizardPage.Page.DiskConfigPage
                If Not DiskConfigurationInteractive AndAlso SelectedDiskConfiguration.DiskConfigMode = DiskConfigurationMode.DiskPart AndAlso Scintilla2.Text = "" Then
                    MessageBox.Show("Please enter the contents of the DiskPart script and try again. You can also use a script file", "DiskPart Script error")
                    Return False
                End If
            Case UnattendedWizardPage.Page.ProductKeyPage
                If Not GenericChosen Then
                    If TextBox3.Text = "" Then
                        MessageBox.Show("Please type a product key and try again", "Product Key error")
                        Return False
                    ElseIf TextBox3.Text <> "" And TextBox3.Text.Length <> 29 Then
                        MessageBox.Show("Please type all of the product key and try again", "Product Key error")
                        Return False
                    ElseIf TextBox3.Text <> "" And TextBox3.Text.Length = 29 Then
                        Dim pKey As ProductKey = ProductKeyValidator.ValidateProductKey(TextBox3.Text)
                        If Not pKey.Valid Then
                            MessageBox.Show("The product key entered:" & CrLf & CrLf & TextBox3.Text & CrLf & CrLf & "is ill-formed. Please type it again", "Product Key error")
                            Return False
                        End If
                    End If
                End If
            Case UnattendedWizardPage.Page.UserAccountsPage
                If Not UserAccountsInteractive AndAlso Not UserValidator.ValidateUsers(UserAccountsList) Then
                    MessageBox.Show("There is a problem with one or more of the users specified. Make sure that all user name fields are filled, or make sure no user uses the Administrator name, and try again", "User Accounts error")
                    Return False
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

    Private Sub RadioButton13_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton13.CheckedChanged
        GenericKeyPanel.Enabled = RadioButton13.Checked
        ManualKeyPanel.Enabled = Not RadioButton13.Checked
        GenericChosen = RadioButton13.Checked
    End Sub

    Private Sub ComboBox6_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox6.SelectedIndexChanged
        If GenericKeys IsNot Nothing AndAlso GenericKeys.Count > 0 Then
            SelectedKey = GenericKeys(ComboBox6.SelectedIndex)
            TextBox2.Text = SelectedKey.Key
        End If
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        SelectedKey.Key = TextBox3.Text
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        UserAccountsInteractive = CheckBox6.Checked
        ManualAccountPanel.Enabled = Not CheckBox6.Checked
    End Sub

#Region "User Account settings"

    Sub ModifyUserDetails(index As Integer, enabled As Boolean, name As String, password As String, group As UserGroup)
        If UserAccountsList Is Nothing OrElse UserAccountsList.Count = 0 Then Exit Sub
        UserAccountsList(index).Enabled = enabled
        UserAccountsList(index).Name = name
        UserAccountsList(index).Password = password
        UserAccountsList(index).Group = group
    End Sub

    Function GroupFromSelectedItem(index As Integer) As UserGroup
        Select Case index
            Case 0
                Return UserGroup.Administrators
            Case 1
                Return UserGroup.Users
        End Select
        Return Nothing
    End Function

    Private Sub CheckBox8_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox8.CheckedChanged
        ModifyUserDetails(1, CheckBox8.Checked, TextBox8.Text, TextBox9.Text, GroupFromSelectedItem(ComboBox9.SelectedIndex))
        TextBox8.Enabled = CheckBox8.Checked
        TextBox9.Enabled = CheckBox8.Checked
        ComboBox9.Enabled = CheckBox8.Checked
    End Sub

    Private Sub CheckBox9_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox9.CheckedChanged
        ModifyUserDetails(2, CheckBox9.Checked, TextBox11.Text, TextBox12.Text, GroupFromSelectedItem(ComboBox10.SelectedIndex))
        TextBox11.Enabled = CheckBox9.Checked
        TextBox12.Enabled = CheckBox9.Checked
        ComboBox10.Enabled = CheckBox9.Checked
    End Sub

    Private Sub CheckBox10_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox10.CheckedChanged
        ModifyUserDetails(3, CheckBox10.Checked, TextBox14.Text, TextBox15.Text, GroupFromSelectedItem(ComboBox11.SelectedIndex))
        TextBox14.Enabled = CheckBox10.Checked
        TextBox15.Enabled = CheckBox10.Checked
        ComboBox11.Enabled = CheckBox10.Checked
    End Sub

    Private Sub CheckBox11_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox11.CheckedChanged
        ModifyUserDetails(4, CheckBox11.Checked, TextBox17.Text, TextBox18.Text, GroupFromSelectedItem(ComboBox12.SelectedIndex))
        TextBox17.Enabled = CheckBox11.Checked
        TextBox18.Enabled = CheckBox11.Checked
        ComboBox12.Enabled = CheckBox11.Checked
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        ModifyUserDetails(0, True, TextBox4.Text, TextBox6.Text, GroupFromSelectedItem(ComboBox7.SelectedIndex))
    End Sub

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged
        ModifyUserDetails(0, True, TextBox4.Text, TextBox6.Text, GroupFromSelectedItem(ComboBox7.SelectedIndex))
    End Sub

    Private Sub TextBox8_TextChanged(sender As Object, e As EventArgs) Handles TextBox8.TextChanged
        ModifyUserDetails(1, CheckBox8.Checked, TextBox8.Text, TextBox9.Text, GroupFromSelectedItem(ComboBox9.SelectedIndex))
    End Sub

    Private Sub TextBox9_TextChanged(sender As Object, e As EventArgs) Handles TextBox9.TextChanged
        ModifyUserDetails(1, CheckBox8.Checked, TextBox8.Text, TextBox9.Text, GroupFromSelectedItem(ComboBox9.SelectedIndex))
    End Sub

    Private Sub TextBox11_TextChanged(sender As Object, e As EventArgs) Handles TextBox11.TextChanged
        ModifyUserDetails(2, CheckBox9.Checked, TextBox11.Text, TextBox12.Text, GroupFromSelectedItem(ComboBox10.SelectedIndex))
    End Sub

    Private Sub TextBox12_TextChanged(sender As Object, e As EventArgs) Handles TextBox12.TextChanged
        ModifyUserDetails(2, CheckBox9.Checked, TextBox11.Text, TextBox12.Text, GroupFromSelectedItem(ComboBox10.SelectedIndex))
    End Sub

    Private Sub TextBox14_TextChanged(sender As Object, e As EventArgs) Handles TextBox14.TextChanged
        ModifyUserDetails(3, CheckBox10.Checked, TextBox14.Text, TextBox15.Text, GroupFromSelectedItem(ComboBox11.SelectedIndex))
    End Sub

    Private Sub TextBox15_TextChanged(sender As Object, e As EventArgs) Handles TextBox15.TextChanged
        ModifyUserDetails(3, CheckBox10.Checked, TextBox14.Text, TextBox15.Text, GroupFromSelectedItem(ComboBox11.SelectedIndex))
    End Sub

    Private Sub TextBox17_TextChanged(sender As Object, e As EventArgs) Handles TextBox17.TextChanged
        ModifyUserDetails(4, CheckBox11.Checked, TextBox17.Text, TextBox18.Text, GroupFromSelectedItem(ComboBox12.SelectedIndex))
    End Sub

    Private Sub TextBox18_TextChanged(sender As Object, e As EventArgs) Handles TextBox18.TextChanged
        ModifyUserDetails(4, CheckBox11.Checked, TextBox17.Text, TextBox18.Text, GroupFromSelectedItem(ComboBox12.SelectedIndex))
    End Sub

    Private Sub ComboBox7_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox7.SelectedIndexChanged
        ModifyUserDetails(0, True, TextBox4.Text, TextBox6.Text, GroupFromSelectedItem(ComboBox7.SelectedIndex))
    End Sub

    Private Sub ComboBox9_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox9.SelectedIndexChanged
        ModifyUserDetails(1, CheckBox8.Checked, TextBox8.Text, TextBox9.Text, GroupFromSelectedItem(ComboBox9.SelectedIndex))
    End Sub

    Private Sub ComboBox10_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox10.SelectedIndexChanged
        ModifyUserDetails(2, CheckBox9.Checked, TextBox11.Text, TextBox12.Text, GroupFromSelectedItem(ComboBox10.SelectedIndex))
    End Sub

    Private Sub ComboBox11_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox11.SelectedIndexChanged
        ModifyUserDetails(3, CheckBox10.Checked, TextBox14.Text, TextBox15.Text, GroupFromSelectedItem(ComboBox11.SelectedIndex))
    End Sub

    Private Sub ComboBox12_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox12.SelectedIndexChanged
        ModifyUserDetails(4, CheckBox11.Checked, TextBox17.Text, TextBox18.Text, GroupFromSelectedItem(ComboBox12.SelectedIndex))
    End Sub

#End Region

    Private Sub CheckBox12_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox12.CheckedChanged
        AutoLogon.EnableAutoLogon = CheckBox12.Checked
        AutoLogonSettingsPanel.Enabled = CheckBox12.Checked
    End Sub

    Private Sub RadioButton15_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton15.CheckedChanged
        AutoLogon.LogonMode = If(RadioButton15.Checked, AutoLogonMode.FirstAdmin, AutoLogonMode.WindowsAdmin)
        TextBox5.Enabled = Not RadioButton15.Checked
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        AutoLogon.LogonPassword = TextBox5.Text
    End Sub

    Private Sub CheckBox7_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox7.CheckedChanged
        PasswordObfuscate = CheckBox7.Checked
    End Sub

    Private Sub RadioButton17_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton17.CheckedChanged
        SelectedExpirationSettings.Mode = If(RadioButton17.Checked, PasswordExpirationMode.NIST_Unlimited, PasswordExpirationMode.NIST_Limited)
        AutoExpirationPanel.Enabled = Not RadioButton17.Checked
    End Sub

    Private Sub RadioButton19_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton19.CheckedChanged
        SelectedExpirationSettings.WindowsDefault = RadioButton19.Checked
        TimedExpirationPanel.Enabled = Not RadioButton19.Checked
    End Sub

    Private Sub NumericUpDown5_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown5.ValueChanged
        SelectedExpirationSettings.Days = NumericUpDown5.Value
    End Sub

    Private Sub CheckBox13_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox13.CheckedChanged
        SelectedLockdownSettings.Enabled = CheckBox13.Checked
        EnabledAccountLockdownPanel.Enabled = Not CheckBox13.Checked
    End Sub

    Private Sub RadioButton21_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton21.CheckedChanged
        SelectedLockdownSettings.DefaultPolicy = RadioButton21.Checked
        AccountLockdownParametersPanel.Enabled = Not RadioButton21.Checked
    End Sub

    Private Sub NumericUpDown6_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown6.ValueChanged
        SelectedLockdownSettings.TimedLockdownSettings.FailedAttempts = NumericUpDown6.Value
    End Sub

    Private Sub NumericUpDown7_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown7.ValueChanged
        SelectedLockdownSettings.TimedLockdownSettings.Timeframe = NumericUpDown7.Value
    End Sub

    Private Sub NumericUpDown8_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown8.ValueChanged
        SelectedLockdownSettings.TimedLockdownSettings.AutoUnlockTime = NumericUpDown8.Value
    End Sub
End Class