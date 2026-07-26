Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Text.Encoding
Imports System.Threading
Imports ScintillaNET
Imports DISMTools.Elements
Imports Microsoft.Dism
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Text.RegularExpressions
Imports System.Text

Public Class NewUnattendWiz

    ' Declare initial vars
    Dim IsInExpress As Boolean = True
    Dim CurrentWizardPage As New UnattendedWizardPage()
    Dim VerifyInPages As New List(Of UnattendedWizardPage.Page)

    Dim DotNetRuntimeSupported As Boolean
    Dim PreferSelfContained As Boolean
    Const UnattendGenReleaseTag As String = "2663"

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
    Dim SelectedArchitectures As New Dictionary(Of DismProcessorArchitecture, Boolean)
    Dim Win11Config As New SVSettings()
    Dim PCName As New ComputerName()
    Dim PCNameScript As String = "return 'DESKTOP-{0}' -f -join ((48..57) + (65..90) | Get-Random -Count 7 | ForEach-Object {[char]$_})"
    Dim UseConfigSet As Boolean

    ' Time Zone Panel
    Dim TimeOffsets As New List(Of TimeOffset)
    Dim TimeOffsetInteractive As Boolean = True
    Dim SelectedOffset As New TimeOffset()

    ' Disk Configuration Panel
    Dim DiskConfigurationInteractive As Boolean = True
    Dim SelectedDiskConfiguration As New DiskConfiguration()

    ' Product Key Panel
    Dim GenericChosen As Boolean = True
    Dim FirmwareChosen As Boolean
    Dim GenericKeys As New List(Of ProductKey)
    Dim SelectedKey As New ProductKey()

    ' User Accounts Panel
    Dim UserAccountsInteractive As Boolean = True
    Dim MicrosoftAccountInteractive As Boolean
    Dim UserAccountsList As New List(Of User)
    Dim AutoLogon As New AutoLogonSettings()
    Dim PasswordObfuscate As Boolean
    Dim SelectedExpirationSettings As New PasswordExpirationSettings()
    Dim SelectedLockoutSettings As New AccountLockoutSettings()

    ' Virtual Machine Panel
    Dim VirtualMachineSupported As Boolean
    Dim SelectedVMSettings As New VirtualMachineSettings()

    ' Wireless Networking Panel
    Dim NetworkConfigInteractive As Boolean = True
    Dim NetworkConfigManualSkip As Boolean = False
    Dim SelectedNetworkConfiguration As New WirelessSettings()

    ' System Telemetry Panel
    Dim SystemTelemetryInteractive As Boolean
    Dim SelectedTelemetrySettings As New SystemTelemetry()

    ' Scripts Panel
    Dim ConfiguredScripts As New Dictionary(Of PostInstallScript.Stage, List(Of PostInstallScript)) From {
        {PostInstallScript.Stage.Specialize, New List(Of PostInstallScript)},
        {PostInstallScript.Stage.FirstRun, New List(Of PostInstallScript)},
        {PostInstallScript.Stage.UserFirstLogon, New List(Of PostInstallScript)}
    }
    Dim CurrentlyConfiguredScripts As New List(Of PostInstallScript)
    Dim CurrentlyEditedStage As Integer = 0
    Dim CurrentlyEditedScript As Integer = 0
    Dim ScriptsRestartExplorer As Boolean
    Dim ScriptsHideWindow As Boolean

    ' Component Panel
    Dim SystemComponents As New List(Of Component)
    Dim SystemComponentsEx As New List(Of Component)
    Dim ReservedComponents As New List(Of Component)
    Dim ComponentIndex As Integer
    Dim IsComponentBeingLoaded As Boolean

    ' Default Settings
    Dim DefaultLanguage As New ImageLanguage()
    Dim DefaultLocale As New UserLocale()
    Dim DefaultKeybIdentifier As New KeyboardIdentifier()
    Dim DefaultGeoId As New GeoId()
    Dim DefaultOffset As New TimeOffset()
    Dim DefaultDiskConfiguration As New DiskConfiguration()
    Dim DefaultExpirationSettings As New PasswordExpirationSettings()
    Dim DefaultLockoutSettings As New AccountLockoutSettings()
    Dim DefaultVMSettings As New VirtualMachineSettings()
    Dim DefaultNetworkConfiguration As New WirelessSettings()
    Dim DefaultPostInstallScript As PostInstallScript = New PostInstallScript(My.Resources.DefaultPostInstallScriptCode, PostInstallScript.Extension.PowerShell)

    ' Progress info
    Dim ProgressMessage As String = ""

    Dim SaveTarget As String = ""

    ' Editor Mode
    Dim DefaultContents As String

    Private EditionMapping As New Dictionary(Of String, String) From {
        {"Education", "Education"},
        {"EducationN", "Education N"},
        {"Home", "Home"},
        {"HomeN", "Home N"},
        {"HomeSingleLanguage", "Home Single Language"},
        {"Professional", "Pro"},
        {"ProfessionalEducation", "Pro Education"},
        {"ProfessionalEducationN", "Pro Education N"},
        {"ProfessionalWorkstation", "Pro for Workstations"},
        {"ProfessionalN", "Pro N"},
        {"ProfessionalWorkstationN", "Pro N for Workstations"},
        {"Enterprise", "Enterprise"},
        {"EnterpriseN", "Enterprise N"}
    }

    ''' <summary>
    ''' Initializes the Scintilla editor
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
        Scintilla3.StyleResetDefault()
        Scintilla4.StyleResetDefault()
        ' Use VS's selection color, as I find it the most natural
        DynaLog.LogMessage("Setting colors for selection...")
        If CurrentTheme.IsDark Then
            Scintilla1.SelectionBackColor = Color.FromArgb(38, 79, 120)
            Scintilla3.SelectionBackColor = Color.FromArgb(38, 79, 120)
            Scintilla4.SelectionBackColor = Color.FromArgb(38, 79, 120)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Scintilla1.SelectionBackColor = Color.FromArgb(153, 201, 239)
            Scintilla3.SelectionBackColor = Color.FromArgb(153, 201, 239)
            Scintilla4.SelectionBackColor = Color.FromArgb(153, 201, 239)
        End If
        Scintilla1.Styles(Style.Default).Font = fntName
        Scintilla1.Styles(Style.Default).Size = fntSize
        Scintilla3.Styles(Style.Default).Font = fntName
        Scintilla3.Styles(Style.Default).Size = fntSize
        Scintilla4.Styles(Style.Default).Font = fntName
        Scintilla4.Styles(Style.Default).Size = fntSize

        ' Set background and foreground colors (from Visual Studio)
        DynaLog.LogMessage("Setting colors for styles...")
        Scintilla1.Styles(Style.Default).BackColor = CurrentTheme.SectionBackgroundColor
        Scintilla1.Styles(Style.Default).ForeColor = CurrentTheme.ForegroundColor
        Scintilla1.Styles(Style.LineNumber).BackColor = CurrentTheme.SectionBackgroundColor
        Scintilla3.Styles(Style.Default).BackColor = CurrentTheme.SectionBackgroundColor
        Scintilla3.Styles(Style.Default).ForeColor = CurrentTheme.ForegroundColor
        Scintilla3.Styles(Style.LineNumber).BackColor = CurrentTheme.SectionBackgroundColor
        Scintilla4.Styles(Style.Default).BackColor = CurrentTheme.SectionBackgroundColor
        Scintilla4.Styles(Style.Default).ForeColor = CurrentTheme.ForegroundColor
        Scintilla4.Styles(Style.LineNumber).BackColor = CurrentTheme.SectionBackgroundColor
        Scintilla1.StyleClearAll()
        Scintilla3.StyleClearAll()
        Scintilla4.StyleClearAll()

        ' Use Notepad++'s lexer style colors
        DynaLog.LogMessage("Setting colors for XML and PowerShell lexers...")
        If CurrentTheme.IsDark Then
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
            Scintilla4.Styles(Style.Xml.XmlStart).ForeColor = Color.FromArgb(127, 159, 127)
            Scintilla4.Styles(Style.Xml.XmlEnd).ForeColor = Color.FromArgb(127, 159, 127)
            Scintilla4.Styles(Style.Xml.Default).ForeColor = Color.FromArgb(220, 220, 204)
            Scintilla4.Styles(Style.Xml.Comment).ForeColor = Color.FromArgb(127, 159, 127)
            Scintilla4.Styles(Style.Xml.Number).ForeColor = Color.FromArgb(140, 208, 211)
            Scintilla4.Styles(Style.Xml.DoubleString).ForeColor = Color.FromArgb(200, 145, 145)
            Scintilla4.Styles(Style.Xml.SingleString).ForeColor = Color.FromArgb(200, 145, 145)
            Scintilla4.Styles(Style.Xml.Tag).ForeColor = Color.FromArgb(227, 206, 171)
            Scintilla4.Styles(Style.Xml.TagEnd).ForeColor = Color.FromArgb(227, 206, 171)
            Scintilla4.Styles(Style.Xml.TagUnknown).ForeColor = Color.FromArgb(237, 214, 237)
            Scintilla4.Styles(Style.Xml.Attribute).ForeColor = Color.FromArgb(190, 200, 158)
            Scintilla4.Styles(Style.Xml.AttributeUnknown).ForeColor = Color.FromArgb(223, 223, 223)
            Scintilla4.Styles(Style.Xml.CData).ForeColor = Color.FromArgb(200, 145, 145)
            Scintilla4.Styles(Style.Xml.Entity).ForeColor = Color.FromArgb(207, 191, 175)
            Scintilla3.Styles(Style.PowerShell.Default).ForeColor = Color.FromArgb(220, 220, 204)
            Scintilla3.Styles(Style.PowerShell.Comment).ForeColor = Color.FromArgb(127, 159, 127)
            Scintilla3.Styles(Style.PowerShell.String).ForeColor = Color.FromArgb(204, 147, 147)
            Scintilla3.Styles(Style.PowerShell.Character).ForeColor = Color.FromArgb(220, 163, 163)
            Scintilla3.Styles(Style.PowerShell.Number).ForeColor = Color.FromArgb(140, 208, 211)
            Scintilla3.Styles(Style.PowerShell.Variable).ForeColor = Color.FromArgb(220, 220, 204)
            Scintilla3.Styles(Style.PowerShell.Operator).ForeColor = Color.FromArgb(159, 157, 109)
            Scintilla3.Styles(Style.PowerShell.Identifier).ForeColor = Color.FromArgb(220, 220, 204)
            Scintilla3.Styles(Style.PowerShell.Keyword).ForeColor = Color.FromArgb(223, 196, 125)
            Scintilla3.Styles(Style.PowerShell.Cmdlet).ForeColor = Color.FromArgb(255, 207, 175)
            Scintilla3.Styles(Style.PowerShell.Alias).ForeColor = Color.FromArgb(206, 223, 153)
            Scintilla3.Styles(Style.PowerShell.Function).ForeColor = Color.FromArgb(255, 207, 175)
            Scintilla3.Styles(Style.PowerShell.User1).ForeColor = Color.FromArgb(220, 220, 204)
            Scintilla3.Styles(Style.PowerShell.CommentStream).ForeColor = Color.FromArgb(127, 159, 127)
            Scintilla3.Styles(Style.PowerShell.HereString).ForeColor = Color.FromArgb(204, 147, 147)
            Scintilla3.Styles(Style.PowerShell.HereCharacter).ForeColor = Color.FromArgb(204, 147, 147)
            Scintilla3.Styles(Style.PowerShell.CommentDocKeyword).ForeColor = Color.FromArgb(127, 159, 127)
            Scintilla3.Styles(Style.Batch.Default).ForeColor = Color.FromArgb(220, 220, 204)
            Scintilla3.Styles(Style.Batch.Comment).ForeColor = Color.FromArgb(127, 159, 127)
            Scintilla3.Styles(Style.Batch.Word).ForeColor = Color.FromArgb(220, 220, 204)
            Scintilla3.Styles(Style.Batch.Label).ForeColor = Color.FromArgb(223, 196, 125)
            Scintilla3.Styles(Style.Batch.Hide).ForeColor = Color.FromArgb(140, 208, 211)
            Scintilla3.Styles(Style.Batch.Command).ForeColor = Color.FromArgb(255, 207, 175)
            Scintilla3.Styles(Style.Batch.Identifier).ForeColor = Color.FromArgb(204, 147, 147)
            Scintilla3.Styles(Style.Batch.Operator).ForeColor = Color.FromArgb(159, 157, 109)
            Scintilla3.Styles(Style.VbScript.Default).ForeColor = Color.FromArgb(220, 220, 204)
            Scintilla3.Styles(Style.VbScript.Comment).ForeColor = Color.FromArgb(127, 159, 127)
            Scintilla3.Styles(Style.VbScript.Number).ForeColor = Color.FromArgb(140, 208, 211)
            Scintilla3.Styles(Style.VbScript.Keyword).ForeColor = Color.FromArgb(206, 223, 153)
            Scintilla3.Styles(Style.VbScript.String).ForeColor = Color.FromArgb(204, 147, 147)
            Scintilla3.Styles(Style.VbScript.Preprocessor).ForeColor = Color.FromArgb(255, 207, 175)
            Scintilla3.Styles(Style.VbScript.Operator).ForeColor = Color.FromArgb(159, 157, 109)
            Scintilla3.Styles(Style.VbScript.Date).ForeColor = Color.FromArgb(223, 196, 125)
            Scintilla3.Styles(Style.JavaScript.Default).ForeColor = Color.FromArgb(220, 220, 204)
            Scintilla3.Styles(Style.JavaScript.Word).ForeColor = Color.FromArgb(223, 196, 125)
            Scintilla3.Styles(Style.JavaScript.Keyword).ForeColor = Color.FromArgb(223, 196, 125)
            Scintilla3.Styles(Style.JavaScript.Number).ForeColor = Color.FromArgb(140, 208, 211)
            Scintilla3.Styles(Style.JavaScript.DoubleString).ForeColor = Color.FromArgb(204, 147, 147)
            Scintilla3.Styles(Style.JavaScript.SingleString).ForeColor = Color.FromArgb(204, 147, 147)
            Scintilla3.Styles(Style.JavaScript.Regex).ForeColor = Color.FromArgb(204, 147, 147)
            Scintilla3.Styles(Style.JavaScript.Comment).ForeColor = Color.FromArgb(127, 159, 207)
            Scintilla3.Styles(Style.JavaScript.CommentLine).ForeColor = Color.FromArgb(127, 159, 207)
            Scintilla3.Styles(Style.JavaScript.CommentDoc).ForeColor = Color.FromArgb(127, 159, 207)
        Else
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
            Scintilla4.Styles(Style.Xml.XmlStart).ForeColor = Color.Red
            Scintilla4.Styles(Style.Xml.XmlEnd).ForeColor = Color.Red
            Scintilla4.Styles(Style.Xml.Default).ForeColor = Color.Black
            Scintilla4.Styles(Style.Xml.Comment).ForeColor = Color.FromArgb(0, 128, 0)
            Scintilla4.Styles(Style.Xml.Number).ForeColor = Color.Red
            Scintilla4.Styles(Style.Xml.DoubleString).ForeColor = Color.FromArgb(128, 0, 255)
            Scintilla4.Styles(Style.Xml.SingleString).ForeColor = Color.FromArgb(128, 0, 255)
            Scintilla4.Styles(Style.Xml.Tag).ForeColor = Color.Blue
            Scintilla4.Styles(Style.Xml.TagEnd).ForeColor = Color.Blue
            Scintilla4.Styles(Style.Xml.TagUnknown).ForeColor = Color.Blue
            Scintilla4.Styles(Style.Xml.Attribute).ForeColor = Color.Red
            Scintilla4.Styles(Style.Xml.AttributeUnknown).ForeColor = Color.Red
            Scintilla4.Styles(Style.Xml.CData).ForeColor = Color.FromArgb(255, 128, 0)
            Scintilla4.Styles(Style.Xml.Entity).ForeColor = Color.Black
            Scintilla3.Styles(Style.PowerShell.Default).ForeColor = Color.Black
            Scintilla3.Styles(Style.PowerShell.Comment).ForeColor = Color.FromArgb(0, 128, 0)
            Scintilla3.Styles(Style.PowerShell.String).ForeColor = Color.FromArgb(128, 128, 128)
            Scintilla3.Styles(Style.PowerShell.Character).ForeColor = Color.FromArgb(128, 128, 128)
            Scintilla3.Styles(Style.PowerShell.Number).ForeColor = Color.FromArgb(255, 128, 0)
            Scintilla3.Styles(Style.PowerShell.Variable).ForeColor = Color.Black
            Scintilla3.Styles(Style.PowerShell.Operator).ForeColor = Color.FromArgb(0, 0, 128)
            Scintilla3.Styles(Style.PowerShell.Identifier).ForeColor = Color.Black
            Scintilla3.Styles(Style.PowerShell.Keyword).ForeColor = Color.FromArgb(0, 0, 255)
            Scintilla3.Styles(Style.PowerShell.Cmdlet).ForeColor = Color.FromArgb(128, 0, 255)
            Scintilla3.Styles(Style.PowerShell.Alias).ForeColor = Color.FromArgb(0, 128, 255)
            Scintilla3.Styles(Style.PowerShell.Function).ForeColor = Color.FromArgb(196, 0, 98)
            Scintilla3.Styles(Style.PowerShell.User1).ForeColor = Color.FromArgb(128, 0, 0)
            Scintilla3.Styles(Style.PowerShell.CommentStream).ForeColor = Color.FromArgb(0, 128, 128)
            Scintilla3.Styles(Style.PowerShell.HereString).ForeColor = Color.FromArgb(128, 128, 128)
            Scintilla3.Styles(Style.PowerShell.HereCharacter).ForeColor = Color.FromArgb(128, 128, 128)
            Scintilla3.Styles(Style.PowerShell.CommentDocKeyword).ForeColor = Color.FromArgb(0, 128, 128)
            Scintilla3.Styles(Style.Batch.Default).ForeColor = Color.Black
            Scintilla3.Styles(Style.Batch.Comment).ForeColor = Color.FromArgb(0, 128, 0)
            Scintilla3.Styles(Style.Batch.Word).ForeColor = Color.Blue
            Scintilla3.Styles(Style.Batch.Label).ForeColor = Color.Red
            Scintilla3.Styles(Style.Batch.Hide).ForeColor = Color.Magenta
            Scintilla3.Styles(Style.Batch.Command).ForeColor = Color.FromArgb(0, 128, 255)
            Scintilla3.Styles(Style.Batch.Identifier).ForeColor = Color.FromArgb(255, 128, 0)
            Scintilla3.Styles(Style.Batch.Operator).ForeColor = Color.Red
            Scintilla3.Styles(Style.VbScript.Default).ForeColor = Color.Black
            Scintilla3.Styles(Style.VbScript.Comment).ForeColor = Color.FromArgb(0, 128, 0)
            Scintilla3.Styles(Style.VbScript.Number).ForeColor = Color.Red
            Scintilla3.Styles(Style.VbScript.Keyword).ForeColor = Color.Blue
            Scintilla3.Styles(Style.VbScript.String).ForeColor = Color.Gray
            Scintilla3.Styles(Style.VbScript.Preprocessor).ForeColor = Color.Red
            Scintilla3.Styles(Style.VbScript.Operator).ForeColor = Color.Black
            Scintilla3.Styles(Style.VbScript.Date).ForeColor = Color.FromArgb(0, 255, 0)
            Scintilla3.Styles(Style.JavaScript.Default).ForeColor = Color.Black
            Scintilla3.Styles(Style.JavaScript.Word).ForeColor = Color.Blue
            Scintilla3.Styles(Style.JavaScript.Keyword).ForeColor = Color.Blue
            Scintilla3.Styles(Style.JavaScript.Number).ForeColor = Color.FromArgb(255, 128, 0)
            Scintilla3.Styles(Style.JavaScript.DoubleString).ForeColor = Color.Gray
            Scintilla3.Styles(Style.JavaScript.SingleString).ForeColor = Color.Gray
            Scintilla3.Styles(Style.JavaScript.Regex).ForeColor = Color.Black
            Scintilla3.Styles(Style.JavaScript.Comment).ForeColor = Color.FromArgb(0, 128, 0)
            Scintilla3.Styles(Style.JavaScript.CommentLine).ForeColor = Color.FromArgb(0, 128, 0)
            Scintilla3.Styles(Style.JavaScript.CommentDoc).ForeColor = Color.FromArgb(0, 128, 0)
        End If
        ' Set lexer
        Scintilla1.LexerName = "xml"
        Scintilla3.LexerName = "powershell"
        Scintilla4.LexerName = "xml"

        ' Set line number margin properties
        DynaLog.LogMessage("Setting colors for line margin...")
        Scintilla1.Styles(Style.LineNumber).BackColor = CurrentTheme.SectionBackgroundColor
        Scintilla3.Styles(Style.LineNumber).BackColor = CurrentTheme.SectionBackgroundColor
        Scintilla4.Styles(Style.LineNumber).BackColor = CurrentTheme.SectionBackgroundColor
        Scintilla1.Styles(Style.LineNumber).ForeColor = Color.FromArgb(165, 165, 165)
        Scintilla3.Styles(Style.LineNumber).ForeColor = Color.FromArgb(165, 165, 165)
        Scintilla4.Styles(Style.LineNumber).ForeColor = Color.FromArgb(165, 165, 165)
        Dim Margin = Scintilla1.Margins(1)
        Margin.Width = 48
        Margin.Type = MarginType.Number
        Margin.Sensitive = True
        Margin.Mask = 0
        Margin = Scintilla3.Margins(1)
        Margin.Width = 48
        Margin.Type = MarginType.Number
        Margin.Sensitive = True
        Margin.Mask = 0
        Margin = Scintilla4.Margins(1)
        Margin.Width = 48
        Margin.Type = MarginType.Number
        Margin.Sensitive = True
        Margin.Mask = 0

        ' Initialize code folding
        DynaLog.LogMessage("Setting code folding...")
        Scintilla1.SetFoldMarginColor(True, Scintilla1.Styles(Style.Default).BackColor)
        Scintilla1.SetFoldMarginColor(True, Scintilla1.Styles(Style.Default).BackColor)
        Scintilla1.SetProperty("fold", "1")
        Scintilla1.SetProperty("fold.compact", "1")
        Scintilla3.SetFoldMarginColor(True, Scintilla3.Styles(Style.Default).BackColor)
        Scintilla3.SetFoldMarginColor(True, Scintilla3.Styles(Style.Default).BackColor)
        Scintilla3.SetProperty("fold", "1")
        Scintilla3.SetProperty("fold.compact", "1")
        Scintilla4.SetFoldMarginColor(True, Scintilla3.Styles(Style.Default).BackColor)
        Scintilla4.SetFoldMarginColor(True, Scintilla3.Styles(Style.Default).BackColor)
        Scintilla4.SetProperty("fold", "1")
        Scintilla4.SetProperty("fold.compact", "1")

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
        Bookmarks = Scintilla3.Margins(2)
        Bookmarks.Width = 20
        Bookmarks.Sensitive = True
        Bookmarks.Type = MarginType.Symbol
        Bookmarks.Mask = (1 << 2)
        Marker = Scintilla3.Markers(2)
        Marker.Symbol = MarkerSymbol.Circle
        Marker.SetBackColor(Color.FromArgb(255, 0, 59))
        Marker.SetForeColor(Color.Black)
        Marker.SetAlpha(100)
        Bookmarks = Scintilla4.Margins(2)
        Bookmarks.Width = 20
        Bookmarks.Sensitive = True
        Bookmarks.Type = MarginType.Symbol
        Bookmarks.Mask = (1 << 2)
        Marker = Scintilla4.Markers(2)
        Marker.Symbol = MarkerSymbol.Circle
        Marker.SetBackColor(Color.FromArgb(255, 0, 59))
        Marker.SetForeColor(Color.Black)
        Marker.SetAlpha(100)

        ' Set editor caret settings
        DynaLog.LogMessage("Setting colors for editor caret...")
        Scintilla1.CaretForeColor = ForeColor
        Scintilla3.CaretForeColor = ForeColor
        Scintilla4.CaretForeColor = ForeColor


        ' Configure code folding margins
        DynaLog.LogMessage("Setting margins for code folding...")
        Scintilla1.Margins(3).Type = MarginType.Symbol
        Scintilla1.Margins(3).Mask = Marker.MaskFolders
        Scintilla1.Margins(3).Sensitive = True
        Scintilla1.Margins(3).Width = 1
        Scintilla3.Margins(3).Type = MarginType.Symbol
        Scintilla3.Margins(3).Mask = Marker.MaskFolders
        Scintilla3.Margins(3).Sensitive = True
        Scintilla3.Margins(3).Width = 1
        Scintilla4.Margins(3).Type = MarginType.Symbol
        Scintilla4.Margins(3).Mask = Marker.MaskFolders
        Scintilla4.Margins(3).Sensitive = True
        Scintilla4.Margins(3).Width = 1

        ' Set colors for all folding markers
        DynaLog.LogMessage("Setting colors for folding markers...")
        For x = 25 To 31
            Scintilla1.Markers(x).SetForeColor(Scintilla1.Styles(Style.Default).BackColor)
            Scintilla1.Markers(x).SetBackColor(Scintilla1.Styles(Style.Default).ForeColor)
            Scintilla3.Markers(x).SetForeColor(Scintilla1.Styles(Style.Default).BackColor)
            Scintilla3.Markers(x).SetBackColor(Scintilla1.Styles(Style.Default).ForeColor)
            Scintilla4.Markers(x).SetForeColor(Scintilla1.Styles(Style.Default).BackColor)
            Scintilla4.Markers(x).SetBackColor(Scintilla1.Styles(Style.Default).ForeColor)
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
        Scintilla3.Markers(Marker.Folder).Symbol = MarkerSymbol.BoxPlus
        Scintilla3.Markers(Marker.FolderOpen).Symbol = MarkerSymbol.BoxMinus
        Scintilla3.Markers(Marker.FolderEnd).Symbol = MarkerSymbol.BoxPlusConnected
        Scintilla3.Markers(Marker.FolderMidTail).Symbol = MarkerSymbol.TCorner
        Scintilla3.Markers(Marker.FolderOpenMid).Symbol = MarkerSymbol.BoxMinusConnected
        Scintilla3.Markers(Marker.FolderSub).Symbol = MarkerSymbol.VLine
        Scintilla3.Markers(Marker.FolderTail).Symbol = MarkerSymbol.LCorner
        Scintilla4.Markers(Marker.Folder).Symbol = MarkerSymbol.BoxPlus
        Scintilla4.Markers(Marker.FolderOpen).Symbol = MarkerSymbol.BoxMinus
        Scintilla4.Markers(Marker.FolderEnd).Symbol = MarkerSymbol.BoxPlusConnected
        Scintilla4.Markers(Marker.FolderMidTail).Symbol = MarkerSymbol.TCorner
        Scintilla4.Markers(Marker.FolderOpenMid).Symbol = MarkerSymbol.BoxMinusConnected
        Scintilla4.Markers(Marker.FolderSub).Symbol = MarkerSymbol.VLine
        Scintilla4.Markers(Marker.FolderTail).Symbol = MarkerSymbol.LCorner

        ' Enable folding
        DynaLog.LogMessage("Enabling folding...")
        Scintilla1.AutomaticFold = (AutomaticFold.Show Or AutomaticFold.Click Or AutomaticFold.Show)
        Scintilla3.AutomaticFold = (AutomaticFold.Show Or AutomaticFold.Click Or AutomaticFold.Show)
        Scintilla4.AutomaticFold = (AutomaticFold.Show Or AutomaticFold.Click Or AutomaticFold.Show)

        ' Add Keywords
        DynaLog.LogMessage("Adding keywords to editors...")
        AddScintillaKeywords("powershell", 0, "begin break catch class continue data do dynamicparam else elseif end enum exit filter finally for foreach function hidden if in inlinescript parallel param process return sequence static switch throw trap try until using while workflow")
        AddScintillaKeywords("powershell", 1, "add-appprovisionedsharedpackagecontainer add-appsharedpackagecontainer add-appvclientconnectiongroup add-appvclientpackage add-appvpublishingserver add-appxpackage add-appxprovisionedpackage add-appxvolume add-bitsfile add-certificateenrollmentpolicyserver add-computer add-content add-history add-jobtrigger add-kdsrootkey add-localgroupmember add-member add-pssnapin add-signerrule add-type add-windowscapability add-windowsdriver add-windowsimage add-windowspackage checkpoint-computer clear-content clear-eventlog clear-history clear-item clear-itemproperty clear-kdscache clear-recyclebin clear-tpm clear-uevappxpackage clear-uevconfiguration clear-variable clear-windowscorruptmountpoint compare-object complete-bitstransfer complete-dtcdiagnostictransaction complete-transaction confirm-securebootuefi connect-pssession connect-wsman convert-path convert-string convertfrom-cipolicy convertfrom-csv convertfrom-json convertfrom-securestring convertfrom-string convertfrom-stringdata convertto-csv convertto-html convertto-json convertto-processmitigationpolicy convertto-securestring convertto-tpmownerauth convertto-xml copy-bcdentry copy-item copy-itemproperty copy-userinternationalsettingstosystem debug-job debug-process debug-runspace disable-appbackgroundtaskdiagnosticlog disable-appv disable-appvclientconnectiongroup disable-bcdelementbootdebug disable-bcdelementbootems disable-bcdelementdebug disable-bcdelementems disable-bcdelementeventlogging disable-bcdelementhypervisordebug disable-computerrestore disable-jobtrigger disable-localuser disable-psbreakpoint disable-psremoting disable-pssessionconfiguration disable-runspacedebug disable-scheduledjob disable-tlsciphersuite disable-tlsecccurve disable-tlssessionticketkey disable-tpmautoprovisioning disable-uev disable-uevappxpackage disable-uevtemplate disable-wsmancredssp disable-windowserrorreporting disable-windowsoptionalfeature disconnect-pssession disconnect-wsman dismount-appxvolume dismount-windowsimage edit-cipolicyrule enable-appbackgroundtaskdiagnosticlog enable-appv enable-appvclientconnectiongroup enable-bcdelementbootdebug enable-bcdelementbootems enable-bcdelementdebug enable-bcdelementems enable-bcdelementeventlogging enable-bcdelementhypervisordebug enable-computerrestore enable-jobtrigger enable-localuser enable-psbreakpoint enable-psremoting enable-pssessionconfiguration enable-runspacedebug enable-scheduledjob enable-tlsciphersuite enable-tlsecccurve enable-tlssessionticketkey enable-tpmautoprovisioning enable-uev enable-uevappxpackage enable-uevtemplate enable-wsmancredssp enable-windowserrorreporting enable-windowsoptionalfeature enter-pshostprocess enter-pssession exit-pshostprocess exit-pssession expand-windowscustomdataimage expand-windowsimage export-alias export-bcdstore export-binarymilog export-certificate export-clixml export-console export-counter export-csv export-formatdata export-modulemember export-pssession export-pfxcertificate export-provisioningpackage export-startlayout export-startlayoutedgeassets export-tlssessionticketkey export-trace export-uevconfiguration export-uevpackage export-windowscapabilitysource export-windowsdriver export-windowsimage find-package find-packageprovider foreach-object format-custom format-list format-securebootuefi format-table format-wide get-acl get-alias get-applockerfileinformation get-applockerpolicy get-appprovisionedsharedpackagecontainer get-appsharedpackagecontainer get-appvclientapplication get-appvclientconfiguration get-appvclientconnectiongroup get-appvclientmode get-appvclientpackage get-appvpublishingserver get-appvstatus get-appxdefaultvolume get-appxpackage get-appxpackageautoupdatesettings get-appxpackagemanifest get-appxprovisionedpackage get-appxvolume get-authenticodesignature get-bcdentry get-bcdentrydebugsettings get-bcdentryhypervisorsettings get-bcdstore get-bitstransfer get-cipolicy get-cipolicyidinfo get-cipolicyinfo get-certificate get-certificateautoenrollmentpolicy get-certificateenrollmentpolicyserver get-certificatenotificationtask get-childitem get-cimassociatedinstance get-cimclass get-ciminstance get-cimsession get-clipboard get-cmsmessage get-command get-computerinfo get-computerrestorepoint get-content get-controlpanelitem get-counter get-credential get-culture get-dapolicychange get-date get-deliveryoptimizationlog get-deliveryoptimizationloganalysis get-event get-eventlog get-eventsubscriber get-executionpolicy get-formatdata get-help get-history get-host get-hotfix get-installedlanguage get-item get-itemproperty get-itempropertyvalue get-job get-jobtrigger get-kdsconfiguration get-kdsrootkey get-localgroup get-localgroupmember get-localuser get-location get-member get-module get-nonremovableappspolicy get-psbreakpoint get-pscallstack get-psdrive get-pshostprocessinfo get-psprovider get-psreadlinekeyhandler get-psreadlineoption get-pssession get-pssessioncapability get-pssessionconfiguration get-pssnapin get-package get-packageprovider get-packagesource get-pfxcertificate get-pfxdata get-pmemdedicatedmemory get-pmemdisk get-pmemphysicaldevice get-pmemunusedregion get-process get-processmitigation get-provisioningpackage get-random get-runspace get-runspacedebug get-scheduledjob get-scheduledjoboption get-securebootpolicy get-securebootuefi get-service get-systemdriver get-systempreferreduilanguage get-timezone get-tlsciphersuite get-tlsecccurve get-tpm get-tpmendorsementkeyinfo get-tpmsupportedfeature get-tracesource get-transaction get-troubleshootingpack get-trustedprovisioningcertificate get-typedata get-uiculture get-uevappxpackage get-uevconfiguration get-uevstatus get-uevtemplate get-uevtemplateprogram get-unique get-variable get-wimbootentry get-wsmancredssp get-wsmaninstance get-wheamemorypolicy get-winacceptlanguagefromlanguagelistoptout get-winculturefromlanguagelistoptout get-windefaultinputmethodoverride get-winevent get-winhomelocation get-winlanguagebaroption get-winsystemlocale get-winuilanguageoverride get-winuserlanguagelist get-windowscapability get-windowsdeveloperlicense get-windowsdriver get-windowsedition get-windowserrorreporting get-windowsimage get-windowsimagecontent get-windowsoptionalfeature get-windowspackage get-windowsreservedstoragestate get-windowssearchsetting get-wmiobject group-object import-alias import-bcdstore import-binarymilog import-certificate import-clixml import-counter import-csv import-localizeddata import-module import-pssession import-packageprovider import-pfxcertificate import-startlayout import-tpmownerauth import-uevconfiguration initialize-pmemphysicaldevice initialize-tpm install-language install-package install-packageprovider install-provisioningpackage install-trustedprovisioningcertificate invoke-cimmethod invoke-command invoke-commandindesktoppackage invoke-dscresource invoke-expression invoke-history invoke-item invoke-restmethod invoke-troubleshootingpack invoke-wsmanaction invoke-webrequest invoke-wmimethod join-dtcdiagnosticresourcemanager join-path limit-eventlog measure-command measure-object merge-cipolicy mount-appvclientconnectiongroup mount-appvclientpackage mount-appxvolume mount-windowsimage move-appxpackage move-item move-itemproperty new-alias new-applockerpolicy new-bcdentry new-bcdstore new-cipolicy new-cipolicyrule new-certificatenotificationtask new-ciminstance new-cimsession new-cimsessionoption new-dtcdiagnostictransaction new-event new-eventlog new-filecatalog new-item new-itemproperty new-jobtrigger new-localgroup new-localuser new-module new-modulemanifest new-netipsecauthproposal new-netipsecmainmodecryptoproposal new-netipsecquickmodecryptoproposal new-object new-psdrive new-psrolecapabilityfile new-pssession new-pssessionconfigurationfile new-pssessionoption new-pstransportoption new-psworkflowexecutionoption new-pmemdedicatedmemory new-pmemdisk new-provisioningrepro new-scheduledjoboption new-selfsignedcertificate new-service new-timespan new-tlssessionticketkey new-variable new-wsmaninstance new-wsmansessionoption new-webserviceproxy new-winevent new-winuserlanguagelist new-windowscustomimage new-windowsimage optimize-appxprovisionedpackages optimize-windowsimage out-default out-file out-gridview out-host out-null out-printer out-string pop-location protect-cmsmessage publish-appvclientpackage publish-dscconfiguration push-location read-host receive-dtcdiagnostictransaction receive-job receive-pssession register-argumentcompleter register-cimindicationevent register-engineevent register-objectevent register-pssessionconfiguration register-packagesource register-scheduledjob register-uevtemplate register-wmievent remove-appprovisionedsharedpackagecontainer remove-appsharedpackagecontainer remove-appvclientconnectiongroup remove-appvclientpackage remove-appvpublishingserver remove-appxpackage remove-appxpackageautoupdatesettings remove-appxprovisionedpackage remove-appxvolume remove-bcdelement remove-bcdentry remove-bitstransfer remove-cipolicyrule remove-certificateenrollmentpolicyserver remove-certificatenotificationtask remove-ciminstance remove-cimsession remove-computer remove-event remove-eventlog remove-item remove-itemproperty remove-job remove-jobtrigger remove-localgroup remove-localgroupmember remove-localuser remove-module remove-psbreakpoint remove-psdrive remove-psreadlinekeyhandler remove-pssession remove-pssnapin remove-pmemdedicatedmemory remove-pmemdisk remove-typedata remove-variable remove-wsmaninstance remove-windowscapability remove-windowsdriver remove-windowsimage remove-windowspackage remove-wmiobject rename-computer rename-item rename-itemproperty rename-localgroup rename-localuser repair-appvclientconnectiongroup repair-appvclientpackage repair-uevtemplateindex repair-windowsimage reset-appsharedpackagecontainer reset-appxpackage reset-computermachinepassword resolve-dnsname resolve-path restart-computer restart-service restore-computer restore-uevbackup restore-uevusersetting resume-bitstransfer resume-job resume-provisioningsession resume-service save-help save-package save-windowsimage select-object select-string select-xml send-appvclientreport send-dtcdiagnostictransaction send-mailmessage set-acl set-alias set-appbackgroundtaskresourcepolicy set-applockerpolicy set-appxprovisioneddatafile set-appvclientconfiguration set-appvclientmode set-appvclientpackage set-appvpublishingserver set-appxdefaultvolume set-appxpackageautoupdatesettings set-authenticodesignature set-bcdbootdefault set-bcdbootdisplayorder set-bcdbootsequence set-bcdboottimeout set-bcdboottoolsdisplayorder set-bcddebugsettings set-bcdelement set-bcdhypervisorsettings set-bitstransfer set-cipolicyidinfo set-cipolicysetting set-cipolicyversion set-certificateautoenrollmentpolicy set-ciminstance set-clipboard set-content set-culture set-date set-dsclocalconfigurationmanager set-executionpolicy set-hvcioptions set-item set-itemproperty set-jobtrigger set-kdsconfiguration set-localgroup set-localuser set-location set-nonremovableappspolicy set-psbreakpoint set-psdebug set-psreadlinekeyhandler set-psreadlineoption set-pssessionconfiguration set-packagesource set-processmitigation set-ruleoption set-scheduledjob set-scheduledjoboption set-securebootuefi set-service set-strictmode set-systempreferreduilanguage set-timezone set-tpmownerauth set-tracesource set-uevconfiguration set-uevtemplateprofile set-variable set-wsmaninstance set-wsmanquickconfig set-wheamemorypolicy set-winacceptlanguagefromlanguagelistoptout set-winculturefromlanguagelistoptout set-windefaultinputmethodoverride set-winhomelocation set-winlanguagebaroption set-winsystemlocale set-winuilanguageoverride set-winuserlanguagelist set-windowsedition set-windowsproductkey set-windowsreservedstoragestate set-windowssearchsetting set-wmiinstance show-command show-controlpanelitem show-eventlog show-windowsdeveloperlicenseregistration sort-object split-path split-windowsimage start-bitstransfer start-dscconfiguration start-dtcdiagnosticresourcemanager start-job start-osuninstall start-process start-service start-sleep start-transaction start-transcript stop-appvclientconnectiongroup stop-appvclientpackage stop-computer stop-dtcdiagnosticresourcemanager stop-job stop-process stop-service stop-transcript suspend-bitstransfer suspend-job suspend-service switch-certificate sync-appvpublishingserver tee-object test-applockerpolicy test-certificate test-computersecurechannel test-connection test-dscconfiguration test-filecatalog test-kdsrootkey test-modulemanifest test-pssessionconfigurationfile test-path test-uevtemplate test-wsman trace-command unblock-file unblock-tpm undo-dtcdiagnostictransaction undo-transaction uninstall-language uninstall-package uninstall-provisioningpackage uninstall-trustedprovisioningcertificate unprotect-cmsmessage unpublish-appvclientpackage unregister-event unregister-pssessionconfiguration unregister-packagesource unregister-scheduledjob unregister-uevtemplate unregister-windowsdeveloperlicense update-dscconfiguration update-formatdata update-help update-list update-typedata update-uevtemplate update-wimbootentry use-transaction use-windowsunattend wait-debugger wait-event wait-job wait-process where-object write-debug write-error write-eventlog write-host write-information write-output write-progress write-verbose write-warning")
        AddScintillaKeywords("powershell", 2, "% ? add-apppackage add-apppackagevolume add-appprovisionedpackage add-provisionedapppackage add-provisionedappsharedpackagecontainer add-provisionedappxpackage add-provisioningpackage add-trustedprovisioningcertificate apply-windowsunattend cfs disable-physicaldiskindication disable-storagediagnosticlog dismount-apppackagevolume enable-physicaldiskindication enable-storagediagnosticlog flush-volume get-apppackage get-apppackageautoupdatesettings get-apppackagedefaultvolume get-apppackagelasterror get-apppackagelog get-apppackagemanifest get-apppackagevolume get-appprovisionedpackage get-disksnv get-language get-physicaldisksnv get-preferredlanguage get-provisionedapppackage get-provisionedappsharedpackagecontainer get-provisionedappxpackage get-storageenclosuresnv get-systemlanguage initialize-volume mount-apppackagevolume move-apppackage move-smbclient optimize-appprovisionedpackages optimize-provisionedapppackages optimize-provisionedappxpackages remove-apppackage remove-apppackageautoupdatesettings remove-apppackagevolume remove-appprovisionedpackage remove-etwtracesession remove-provisionedapppackage remove-provisionedappsharedpackagecontainer remove-provisionedappxpackage remove-provisioningpackage remove-trustedprovisioningcertificate reset-apppackage set-apppackageautoupdatesettings set-apppackagedefaultvolume set-apppackageprovisioneddatafile set-autologgerconfig set-etwtracesession set-preferredlanguage set-provisionedapppackagedatafile set-provisionedappxdatafile set-systemlanguage tnc write-filesystemcache ac algm asnp blsmba cat cd chdir clc clear clhy cli clp cls clv cnsn compare copy cp cpi cpp cssmbo cssmbse curl cvpa dbp del diff dir dlu dnsn dsmbd ebp echo elu epal epcsv epsn erase esmbd etsn exsn fc fhx fimo fl foreach ft fw gal gbp gc gcai gcb gcfg gcfgs gci gcim gcls gcm gcms gcs gdr ghy gi gin gip gjb gl glcm glg glgm glu gm gmo gp gps gpv group grsmba gsmba gsmbb gsmbc gsmbcc gsmbcn gsmbd gsmbgm gsmbm gsmbmc gsmbo gsmbs gsmbsc gsmbscm gsmbscp gsmbse gsmbsn gsmbt gsmbw gsn gsnp gsv gtz gu gv gwmi h history icim icm iex ihy ii inmo ipal ipcsv ipmo ipsn irm ise iwmi iwr kill lp ls man md measure mi mount move mp msmbw mv nal ncim ncms ncso ndr ni nlg nlu nmo npssc nsmbgm nsmbm nsmbs nsmbscm nsmbt nsn nv nwsn ogv oh pbcfg popd ps pumo pushd pwd r rbp rcie rcim rcjb rcms rcsn rd rdr ren ri rjb rksmba rlg rlgm rlu rm rmdir rmo rni rnlg rnlu rnp rp rsmbb rsmbc rsmbcc rsmbgm rsmbm rsmbs rsmbsc rsmbscm rsmbt rsn rsnp rtcfg rujb rv rvpa rwmi sacfg sajb sal saps sasv sbp sc scb scim select set shcm si sl slcm sleep slg sls slu sort sp spjb spps spsv ssmbb ssmbcc ssmbp ssmbs ssmbsc ssmbscm start stz sujb sv swmi tcfg tee trcm type udsmbmc ulsmba upcfg upmo wget where wjb write")
        AddScintillaKeywords("powershell", 3, "a: add-bcdatacacheextension add-bitlockerkeyprotector add-dnsclientdohserveraddress add-dnsclientnrptrule add-dtcclustertmmapping add-etwtraceprovider add-initiatoridtomaskingset add-mppreference add-neteventnetworkadapter add-neteventpacketcaptureprovider add-neteventprovider add-neteventvfpprovider add-neteventvmnetworkadapter add-neteventvmswitch add-neteventvmswitchprovider add-neteventwfpcaptureprovider add-netiphttpscertbinding add-netlbfoteammember add-netlbfoteamnic add-netnatexternaladdress add-netnatstaticmapping add-netswitchteammember add-odbcdsn add-partitionaccesspath add-physicaldisk add-printer add-printerdriver add-printerport add-storagefaultdomain add-targetporttomaskingset add-vmdirectvirtualdisk add-virtualdisktomaskingset add-vpnconnection add-vpnconnectionroute add-vpnconnectiontriggerapplication add-vpnconnectiontriggerdnsconfiguration add-vpnconnectiontriggertrustednetwork afterall aftereach assert-mockcalled assert-verifiablemocks b: backup-bitlockerkeyprotector backuptoaad-bitlockerkeyprotector beforeall beforeeach block-fileshareaccess block-smbshareaccess c: clear-assignedaccess clear-bccache clear-bitlockerautounlock clear-disk clear-dnsclientcache clear-filestoragetier clear-host clear-pcsvdevicelog clear-storagebusdisk clear-storagediagnosticinfo close-smbopenfile close-smbsession compress-archive configuration connect-iscsitarget connect-virtualdisk context convertfrom-sddlstring copy-netfirewallrule copy-netipsecmainmodecryptoset copy-netipsecmainmoderule copy-netipsecphase1authset copy-netipsecphase2authset copy-netipsecquickmodecryptoset copy-netipsecrule d: debug-fileshare debug-mmappprelaunch debug-storagesubsystem debug-volume delete-deliveryoptimizationcache describe disable-bc disable-bcdowngrading disable-bcserveonbattery disable-bitlocker disable-bitlockerautounlock disable-damanualentrypointselection disable-deliveryoptimizationverboselogs disable-dscdebug disable-mmagent disable-netadapter disable-netadapterbinding disable-netadapterchecksumoffload disable-netadapterencapsulatedpackettaskoffload disable-netadapteripsecoffload disable-netadapterlso disable-netadapterpacketdirect disable-netadapterpowermanagement disable-netadapterqos disable-netadapterrdma disable-netadapterrsc disable-netadapterrss disable-netadaptersriov disable-netadapteruso disable-netadaptervmq disable-netdnstransitionconfiguration disable-netfirewallrule disable-netiphttpsprofile disable-netipsecmainmoderule disable-netipsecrule disable-netnattransitionconfiguration disable-networkswitchethernetport disable-networkswitchfeature disable-networkswitchvlan disable-odbcperfcounter disable-pstrace disable-pswsmancombinedtrace disable-physicaldiskidentification disable-pnpdevice disable-scheduledtask disable-smbdelegation disable-storagebuscache disable-storagebusdisk disable-storagedatacollection disable-storageenclosureidentification disable-storageenclosurepower disable-storagehighavailability disable-storagemaintenancemode disable-wsmantrace disable-wdacbidtrace disconnect-iscsitarget disconnect-virtualdisk dismount-diskimage e: enable-bcdistributed enable-bcdowngrading enable-bchostedclient enable-bchostedserver enable-bclocal enable-bcserveonbattery enable-bitlocker enable-bitlockerautounlock enable-damanualentrypointselection enable-deliveryoptimizationverboselogs enable-dscdebug enable-mmagent enable-netadapter enable-netadapterbinding enable-netadapterchecksumoffload enable-netadapterencapsulatedpackettaskoffload enable-netadapteripsecoffload enable-netadapterlso enable-netadapterpacketdirect enable-netadapterpowermanagement enable-netadapterqos enable-netadapterrdma enable-netadapterrsc enable-netadapterrss enable-netadaptersriov enable-netadapteruso enable-netadaptervmq enable-netdnstransitionconfiguration enable-netfirewallrule enable-netiphttpsprofile enable-netipsecmainmoderule enable-netipsecrule enable-netnattransitionconfiguration enable-networkswitchethernetport enable-networkswitchfeature enable-networkswitchvlan enable-odbcperfcounter enable-pstrace enable-pswsmancombinedtrace enable-physicaldiskidentification enable-pnpdevice enable-scheduledtask enable-smbdelegation enable-storagebuscache enable-storagebusdisk enable-storagedatacollection enable-storageenclosureidentification enable-storageenclosurepower enable-storagehighavailability enable-storagemaintenancemode enable-wsmantrace enable-wdacbidtrace expand-archive export-bccachepackage export-bcsecretkey export-odataendpointproxy export-scheduledtask export-winhttpproxy f: find-command find-dscresource find-module find-netipsecrule find-netroute find-rolecapability find-script flush-etwtracesession format-hex format-volume g: get-appbackgroundtask get-appvvirtualprocess get-appxlasterror get-appxlog get-assignedaccess get-autologgerconfig get-bcclientconfiguration get-bccontentserverconfiguration get-bcdatacache get-bcdatacacheextension get-bchashcache get-bchostedcacheserverconfiguration get-bcnetworkconfiguration get-bcstatus get-bitlockervolume get-clusteredscheduledtask get-daclientexperienceconfiguration get-daconnectionstatus get-daentrypointtableitem get-doconfig get-dodownloadmode get-dopercentagemaxbackgroundbandwidth get-dopercentagemaxforegroundbandwidth get-dedupproperties get-deliveryoptimizationperfsnap get-deliveryoptimizationperfsnapthismonth get-deliveryoptimizationstatus get-disk get-diskimage get-diskstoragenodeview get-dnsclient get-dnsclientcache get-dnsclientdohserveraddress get-dnsclientglobalsetting get-dnsclientnrptglobal get-dnsclientnrptpolicy get-dnsclientnrptrule get-dnsclientserveraddress get-dscconfiguration get-dscconfigurationstatus get-dsclocalconfigurationmanager get-dscresource get-dtc get-dtcadvancedhostsetting get-dtcadvancedsetting get-dtcclusterdefault get-dtcclustertmmapping get-dtcdefault get-dtclog get-dtcnetworksetting get-dtctransaction get-dtctransactionsstatistics get-dtctransactionstracesession get-dtctransactionstracesetting get-etwtraceprovider get-etwtracesession get-filehash get-fileintegrity get-fileshare get-fileshareaccesscontrolentry get-filestoragetier get-initiatorid get-initiatorport get-installedmodule get-installedscript get-iscsiconnection get-iscsisession get-iscsitarget get-iscsitargetportal get-isesnippet get-logproperties get-mmagent get-maskingset get-mockdynamicparameters get-mpcomputerstatus get-mpperformancereport get-mppreference get-mpthreat get-mpthreatcatalog get-mpthreatdetection get-ncsipolicyconfiguration get-net6to4configuration get-netadapter get-netadapteradvancedproperty get-netadapterbinding get-netadapterchecksumoffload get-netadapterdatapathconfiguration get-netadapterencapsulatedpackettaskoffload get-netadapterhardwareinfo get-netadapteripsecoffload get-netadapterlso get-netadapterpacketdirect get-netadapterpowermanagement get-netadapterqos get-netadapterrdma get-netadapterrsc get-netadapterrss get-netadaptersriov get-netadaptersriovvf get-netadapterstatistics get-netadapteruso get-netadaptervmqqueue get-netadaptervport get-netadaptervmq get-netcompartment get-netconnectionprofile get-netdnstransitionconfiguration get-netdnstransitionmonitoring get-neteventnetworkadapter get-neteventpacketcaptureprovider get-neteventprovider get-neteventsession get-neteventvfpprovider get-neteventvmnetworkadapter get-neteventvmswitch get-neteventvmswitchprovider get-neteventwfpcaptureprovider get-netfirewalladdressfilter get-netfirewallapplicationfilter get-netfirewalldynamickeywordaddress get-netfirewallinterfacefilter get-netfirewallinterfacetypefilter get-netfirewallportfilter get-netfirewallprofile get-netfirewallrule get-netfirewallsecurityfilter get-netfirewallservicefilter get-netfirewallsetting get-netipaddress get-netipconfiguration get-netiphttpsconfiguration get-netiphttpsstate get-netipinterface get-netipsecdospsetting get-netipsecmainmodecryptoset get-netipsecmainmoderule get-netipsecmainmodesa get-netipsecphase1authset get-netipsecphase2authset get-netipsecquickmodecryptoset get-netipsecquickmodesa get-netipsecrule get-netipv4protocol get-netipv6protocol get-netisatapconfiguration get-netlbfoteam get-netlbfoteammember get-netlbfoteamnic get-netnat get-netnatexternaladdress get-netnatglobal get-netnatsession get-netnatstaticmapping get-netnattransitionconfiguration get-netnattransitionmonitoring get-netneighbor get-netoffloadglobalsetting get-netprefixpolicy get-netqospolicy get-netroute get-netswitchteam get-netswitchteammember get-nettcpconnection get-nettcpsetting get-netteredoconfiguration get-netteredostate get-nettransportfilter get-netudpendpoint get-netudpsetting get-netview get-networkswitchethernetport get-networkswitchfeature get-networkswitchglobaldata get-networkswitchvlan get-odbcdriver get-odbcdsn get-odbcperfcounter get-offloaddatatransfersetting get-operationvalidation get-psrepository get-partition get-partitionsupportedsize get-pcsvdevice get-pcsvdevicelog get-physicaldisk get-physicaldiskstoragenodeview get-physicalextent get-physicalextentassociation get-pnpdevice get-pnpdeviceproperty get-printconfiguration get-printjob get-printer get-printerdriver get-printerport get-printerproperty get-resiliencysetting get-scheduledtask get-scheduledtaskinfo get-smbbandwidthlimit get-smbclientconfiguration get-smbclientnetworkinterface get-smbconnection get-smbdelegation get-smbglobalmapping get-smbmapping get-smbmultichannelconnection get-smbmultichannelconstraint get-smbopenfile get-smbservercertprops get-smbservercertificatemapping get-smbserverconfiguration get-smbservernetworkinterface get-smbsession get-smbshare get-smbshareaccess get-smbwitnessclient get-startapps get-storageadvancedproperty get-storagebusbinding get-storagebuscache get-storagebusclientdevice get-storagebusdisk get-storagebustargetcachestore get-storagebustargetcachestoresinstance get-storagebustargetdevice get-storagebustargetdeviceinstance get-storagechassis get-storagedatacollection get-storagediagnosticinfo get-storageenclosure get-storageenclosurestoragenodeview get-storageenclosurevendordata get-storageextendedstatus get-storagefaultdomain get-storagefileserver get-storagefirmwareinformation get-storagehealthaction get-storagehealthreport get-storagehealthsetting get-storagehistory get-storagejob get-storagenode get-storagepool get-storageprovider get-storagerack get-storagereliabilitycounter get-storagescaleunit get-storagesetting get-storagesite get-storagesubsystem get-storagetier get-storagetiersupportedsize get-supportedclustersizes get-supportedfilesystems get-targetport get-targetportal get-testdriveitem get-vmdirectvirtualdisk get-verb get-virtualdisk get-virtualdisksupportedsize get-volume get-volumecorruptioncount get-volumescrubpolicy get-vpnconnection get-vpnconnectiontrigger get-wdacbidtrace get-windowsupdatelog get-winhttpproxy grant-fileshareaccess grant-smbshareaccess h: hide-virtualdisk i: import-bccachepackage import-bcsecretkey import-isesnippet import-powershelldatafile import-winhttpproxy importsystemmodules in inmodulescope initialize-disk install-dtc install-module install-script invoke-asworkflow invoke-mock invoke-operationvalidation invoke-pester it j: k: l: lock-bitlocker m: mock mount-diskimage move-smbwitnessclient n: new-autologgerconfig new-daentrypointtableitem new-dscchecksum new-eapconfiguration new-etwtracesession new-fileshare new-fixture new-guid new-iscsitargetportal new-isesnippet new-maskingset new-mpperformancerecording new-netadapteradvancedproperty new-neteventsession new-netfirewalldynamickeywordaddress new-netfirewallrule new-netipaddress new-netiphttpsconfiguration new-netipsecdospsetting new-netipsecmainmodecryptoset new-netipsecmainmoderule new-netipsecphase1authset new-netipsecphase2authset new-netipsecquickmodecryptoset new-netipsecrule new-netlbfoteam new-netnat new-netnattransitionconfiguration new-netneighbor new-netqospolicy new-netroute new-netswitchteam new-nettransportfilter new-networkswitchvlan new-psworkflowsession new-partition new-pesteroption new-scheduledtask new-scheduledtaskaction new-scheduledtaskprincipal new-scheduledtasksettingsset new-scheduledtasktrigger new-scriptfileinfo new-smbglobalmapping new-smbmapping new-smbmultichannelconstraint new-smbservercertificatemapping new-smbshare new-storagebusbinding new-storagebuscachestore new-storagefileserver new-storagepool new-storagesubsystemvirtualdisk new-storagetier new-temporaryfile new-virtualdisk new-virtualdiskclone new-virtualdisksnapshot new-volume new-vpnserveraddress o: open-netgpo optimize-storagepool optimize-volume p: psconsolehostreadline pause publish-bcfilecontent publish-bcwebcontent publish-module publish-script q: r: read-printernfctag register-clusteredscheduledtask register-dnsclient register-iscsisession register-psrepository register-scheduledtask register-storagesubsystem remove-autologgerconfig remove-bcdatacacheextension remove-bitlockerkeyprotector remove-daentrypointtableitem remove-dnsclientdohserveraddress remove-dnsclientnrptrule remove-dscconfigurationdocument remove-dtcclustertmmapping remove-etwtraceprovider remove-fileshare remove-initiatorid remove-initiatoridfrommaskingset remove-iscsitargetportal remove-maskingset remove-mppreference remove-mpthreat remove-netadapteradvancedproperty remove-neteventnetworkadapter remove-neteventpacketcaptureprovider remove-neteventprovider remove-neteventsession remove-neteventvfpprovider remove-neteventvmnetworkadapter remove-neteventvmswitch remove-neteventvmswitchprovider remove-neteventwfpcaptureprovider remove-netfirewalldynamickeywordaddress remove-netfirewallrule remove-netipaddress remove-netiphttpscertbinding remove-netiphttpsconfiguration remove-netipsecdospsetting remove-netipsecmainmodecryptoset remove-netipsecmainmoderule remove-netipsecmainmodesa remove-netipsecphase1authset remove-netipsecphase2authset remove-netipsecquickmodecryptoset remove-netipsecquickmodesa remove-netipsecrule remove-netlbfoteam remove-netlbfoteammember remove-netlbfoteamnic remove-netnat remove-netnatexternaladdress remove-netnatstaticmapping remove-netnattransitionconfiguration remove-netneighbor remove-netqospolicy remove-netroute remove-netswitchteam remove-netswitchteammember remove-nettransportfilter remove-networkswitchethernetportipaddress remove-networkswitchvlan remove-odbcdsn remove-partition remove-partitionaccesspath remove-physicaldisk remove-printjob remove-printer remove-printerdriver remove-printerport remove-smbbandwidthlimit remove-smbcomponent remove-smbglobalmapping remove-smbmapping remove-smbmultichannelconstraint remove-smbservercertificatemapping remove-smbshare remove-storagebusbinding remove-storagefaultdomain remove-storagefileserver remove-storagehealthintent remove-storagehealthsetting remove-storagepool remove-storagetier remove-targetportfrommaskingset remove-vmdirectvirtualdisk remove-virtualdisk remove-virtualdiskfrommaskingset remove-vpnconnection remove-vpnconnectionroute remove-vpnconnectiontriggerapplication remove-vpnconnectiontriggerdnsconfiguration remove-vpnconnectiontriggertrustednetwork rename-daentrypointtableitem rename-maskingset rename-netadapter rename-netfirewallrule rename-netiphttpsconfiguration rename-netipsecmainmodecryptoset rename-netipsecmainmoderule rename-netipsecphase1authset rename-netipsecphase2authset rename-netipsecquickmodecryptoset rename-netipsecrule rename-netlbfoteam rename-netswitchteam rename-printer repair-fileintegrity repair-virtualdisk repair-volume reset-bc reset-daclientexperienceconfiguration reset-daentrypointtableitem reset-dtclog reset-ncsipolicyconfiguration reset-net6to4configuration reset-netadapteradvancedproperty reset-netdnstransitionconfiguration reset-netiphttpsconfiguration reset-netisatapconfiguration reset-netteredoconfiguration reset-physicaldisk reset-smbclientconfiguration reset-smbserverconfiguration reset-storagereliabilitycounter reset-winhttpproxy resize-partition resize-storagetier resize-virtualdisk restart-netadapter restart-pcsvdevice restart-printjob restore-dscconfiguration restore-networkswitchconfiguration resume-bitlocker resume-printjob resume-storagebusdisk revoke-fileshareaccess revoke-smbshareaccess s: safegetcommand save-etwtracesession save-module save-netgpo save-networkswitchconfiguration save-script save-storagedatacollection send-etwtracesession set-assignedaccess set-bcauthentication set-bccache set-bcdatacacheentrymaxage set-bcminsmblatency set-bcsecretkey set-clusteredscheduledtask set-daclientexperienceconfiguration set-daentrypointtableitem set-dodownloadmode set-domaxbackgroundbandwidth set-domaxforegroundbandwidth set-dopercentagemaxbackgroundbandwidth set-dopercentagemaxforegroundbandwidth set-deliveryoptimizationstatus set-disk set-dnsclient set-dnsclientdohserveraddress set-dnsclientglobalsetting set-dnsclientnrptglobal set-dnsclientnrptrule set-dnsclientserveraddress set-dtcadvancedhostsetting set-dtcadvancedsetting set-dtcclusterdefault set-dtcclustertmmapping set-dtcdefault set-dtclog set-dtcnetworksetting set-dtctransaction set-dtctransactionstracesession set-dtctransactionstracesetting set-dynamicparametervariables set-etwtraceprovider set-fileintegrity set-fileshare set-filestoragetier set-initiatorport set-iscsichapsecret set-logproperties set-mmagent set-mppreference set-ncsipolicyconfiguration set-net6to4configuration set-netadapter set-netadapteradvancedproperty set-netadapterbinding set-netadapterchecksumoffload set-netadapterdatapathconfiguration set-netadapterencapsulatedpackettaskoffload set-netadapteripsecoffload set-netadapterlso set-netadapterpacketdirect set-netadapterpowermanagement set-netadapterqos set-netadapterrdma set-netadapterrsc set-netadapterrss set-netadaptersriov set-netadapteruso set-netadaptervmq set-netconnectionprofile set-netdnstransitionconfiguration set-neteventpacketcaptureprovider set-neteventprovider set-neteventsession set-neteventvfpprovider set-neteventvmswitchprovider set-neteventwfpcaptureprovider set-netfirewalladdressfilter set-netfirewallapplicationfilter set-netfirewallinterfacefilter set-netfirewallinterfacetypefilter set-netfirewallportfilter set-netfirewallprofile set-netfirewallrule set-netfirewallsecurityfilter set-netfirewallservicefilter set-netfirewallsetting set-netipaddress set-netiphttpsconfiguration set-netipinterface set-netipsecdospsetting set-netipsecmainmodecryptoset set-netipsecmainmoderule set-netipsecphase1authset set-netipsecphase2authset set-netipsecquickmodecryptoset set-netipsecrule set-netipv4protocol set-netipv6protocol set-netisatapconfiguration set-netlbfoteam set-netlbfoteammember set-netlbfoteamnic set-netnat set-netnatglobal set-netnattransitionconfiguration set-netneighbor set-netoffloadglobalsetting set-netqospolicy set-netroute set-nettcpsetting set-netteredoconfiguration set-netudpsetting set-networkswitchethernetportipaddress set-networkswitchportmode set-networkswitchportproperty set-networkswitchvlanproperty set-odbcdriver set-odbcdsn set-psrepository set-partition set-pcsvdevicebootconfiguration set-pcsvdevicenetworkconfiguration set-pcsvdeviceuserpassword set-physicaldisk set-printconfiguration set-printer set-printerproperty set-resiliencysetting set-scheduledtask set-smbbandwidthlimit set-smbclientconfiguration set-smbpathacl set-smbservercertificatemapping set-smbserverconfiguration set-smbshare set-storagebuscache set-storagebusprofile set-storagefileserver set-storagehealthsetting set-storagepool set-storageprovider set-storagesetting set-storagesubsystem set-storagetier set-testinconclusive set-virtualdisk set-volume set-volumescrubpolicy set-vpnconnection set-vpnconnectionipsecconfiguration set-vpnconnectionproxy set-vpnconnectiontriggerdnsconfiguration set-vpnconnectiontriggertrustednetwork set-winhttpproxy setup should show-netfirewallrule show-netipsecrule show-storagehistory show-virtualdisk start-appbackgroundtask start-appvvirtualprocess start-autologgerconfig start-dtc start-dtctransactionstracesession start-etwtracesession start-mprollback start-mpscan start-mpwdoscan start-neteventsession start-pcsvdevice start-scheduledtask start-storagediagnosticlog start-trace stop-dscconfiguration stop-dtc stop-dtctransactionstracesession stop-etwtracesession stop-neteventsession stop-pcsvdevice stop-scheduledtask stop-storagediagnosticlog stop-storagejob stop-trace suspend-bitlocker suspend-printjob suspend-storagebusdisk sync-netipsecrule t: tabexpansion2 test-dtc test-netconnection test-scriptfileinfo u: unblock-fileshareaccess unblock-smbshareaccess uninstall-dtc uninstall-module uninstall-script unlock-bitlocker unregister-appbackgroundtask unregister-clusteredscheduledtask unregister-iscsisession unregister-psrepository unregister-scheduledtask unregister-storagesubsystem update-autologgerconfig update-disk update-etwtracesession update-hoststoragecache update-iscsitarget update-iscsitargetportal update-module update-modulemanifest update-mpsignature update-netfirewalldynamickeywordaddress update-netipsecrule update-script update-scriptfileinfo update-smbmultichannelconnection update-storagebuscache update-storagefirmware update-storagepool update-storageprovidercache v: w: write-dtctransactionstracesession write-printernfctag write-volumecache x: y: z: cd.. cd\ help mkdir more oss prompt")
        AddScintillaKeywords("powershell", 4, "component description example externalhelp forwardhelpcategory forwardhelptargetname functionality inputs link notes outputs parameter remotehelprunspace role synopsis")

        DynaLog.LogMessage("Scintilla editor initialization complete.")
    End Sub

    Sub AddScintillaKeywords(Language As String, Index As Integer, KeywordSet As String)
        DynaLog.LogMessage("Setting the keywords for the appropriate Scintilla control...")
        DynaLog.LogMessage("- Language: " & Language)
        DynaLog.LogMessage("- Index: " & Index)
        DynaLog.LogMessage("- Keyword Set: " & KeywordSet & CrLf)
        If Language <> "" Then
            DynaLog.LogMessage("Language is not nothing. Proceeding to add keywords...")
            Select Case Language
                Case "powershell", "batch", "vbscript", "jscript"
                    Scintilla3.SetKeywords(Index, KeywordSet)
                Case "xml"
                    Scintilla1.SetKeywords(Index, KeywordSet)
            End Select
        End If
    End Sub

    Sub ClearScriptEditorKeywords()
        ' The maximum amount of keyword sets is 4 at this point
        Const MaxKeywordSets As Integer = 4
        For x = 0 To MaxKeywordSets
            Scintilla3.SetKeywords(x, "")
        Next
    End Sub

    Function NewKeyVar(key As String) As ProductKey
        DynaLog.LogMessage("Creating key object with product key " & Quote & key & Quote & "...")
        Dim pKey As New ProductKey()
        pKey.Valid = True
        pKey.Key = key
        Return pKey
    End Function

    Sub SetDefaultSettings()
        DynaLog.LogMessage("Setting default configuration...")
        DynaLog.LogMessage("Setting default regional settings and time offsets...")
        DefaultLanguage.Id = "en-US"
        DefaultLanguage.DisplayName = "English"
        DefaultLocale.Id = "en-US"
        DefaultLocale.DisplayName = "English (United States)"
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
        DynaLog.LogMessage("Setting default disk configuration...")
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

        DynaLog.LogMessage("Adding generic product keys...")
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
        GenericKeys.Add(NewKeyVar("XGVPP-NMH47-7TTHJ-W3FW7-8HV2C"))     ' Enterprise
        GenericKeys.Add(NewKeyVar("WGGHN-J84D6-QYCPR-T7PJ7-X766F"))     ' Enterprise N

        DynaLog.LogMessage("Adding default users. 1 Admin and 4 unused Users...")
        UserAccountsList.Add(New User(True, "Admin", "", UserGroup.Administrators))
        For i = 1 To 4
            UserAccountsList.Add(New User(False, "", "", UserGroup.Users))
        Next

        DynaLog.LogMessage("Setting default password expiration configuration...")
        DefaultExpirationSettings.Mode = PasswordExpirationMode.NIST_Unlimited
        DefaultExpirationSettings.Days = 42
        DynaLog.LogMessage("Setting default Account Lockout configuration...")
        DefaultLockoutSettings.Enabled = True
        DefaultLockoutSettings.DefaultPolicy = True
        DefaultLockoutSettings.TimedLockoutSettings.FailedAttempts = 10
        DefaultLockoutSettings.TimedLockoutSettings.Timeframe = 10
        DefaultLockoutSettings.TimedLockoutSettings.AutoUnlockTime = 10
        DynaLog.LogMessage("Setting default VM configuration...")
        DefaultVMSettings.Provider = VMProvider.VirtIO_Guest_Tools
        DynaLog.LogMessage("Setting default wireless configuration...")
        DefaultNetworkConfiguration.SSID = ""
        DefaultNetworkConfiguration.ConnectWithoutBroadcast = False
        DefaultNetworkConfiguration.Authentication = WiFiAuthenticationMode.WPA2_PSK
        DefaultNetworkConfiguration.Password = ""
        DynaLog.LogMessage("Setting default post-install scripts...")


        SelectedLanguage = DefaultLanguage
        SelectedLocale = DefaultLocale
        SelectedKeybIdentifier = DefaultKeybIdentifier
        SelectedGeoId = DefaultGeoId
        SelectedOffset = DefaultOffset
        SelectedDiskConfiguration = DefaultDiskConfiguration
        SelectedKey = GenericKeys(5)
        SelectedExpirationSettings = DefaultExpirationSettings
        SelectedLockoutSettings = DefaultLockoutSettings
        SelectedVMSettings = DefaultVMSettings
        SelectedNetworkConfiguration = DefaultNetworkConfiguration
        'ConfiguredScripts = DefaultPostInstallScripts

        ConfiguredScripts(PostInstallScript.Stage.FirstRun).Add(DefaultPostInstallScript)
        ConfiguredScripts(PostInstallScript.Stage.Specialize).Add(DefaultPostInstallScript)
        ConfiguredScripts(PostInstallScript.Stage.UserFirstLogon).Add(DefaultPostInstallScript)

        CurrentlyConfiguredScripts.Add(DefaultPostInstallScript)
        SwitchStages(0, True)
    End Sub

    Sub DetectDotNetRuntime(SDKVersion As String, RuntimeVersion As String)
        DynaLog.LogMessage("Detecting installed .NET Core-based runtimes...")
        DynaLog.LogMessage("- .NET SDK version: " & SDKVersion)
        DynaLog.LogMessage("- .NET Runtime version: " & RuntimeVersion)
        DynaLog.LogMessage("Checking if UnattendGen is present...")
        If Not Directory.Exists(Path.Combine(Application.StartupPath, "Tools\UnattendGen")) Then
            DynaLog.LogMessage("UnattendGen is not present. This copy of DISMTools is not complete.")
            DotNetRuntimeSupported = False
            Exit Sub
        End If
        DynaLog.LogMessage("Checking if self-contained UnattendGen is present...")
        If Directory.Exists(Path.Combine(Application.StartupPath, "Tools\UnattendGen\SelfContained")) Then
            ' Self-contained version detected
            DynaLog.LogMessage("Self-contained UnattendGen is present. Determining version...")
            ' We determine if the version of UnattendGen that we downloaded matches the one associated with the
            ' release tag. If it is the same version, then we accept it. Otherwise, we remove the directory and
            ' start over. As a lowest common denominator, we'll go with the 32-bit Windows release to check 
            ' version information. Then we'll go with the 64-bit version.
            Dim incompatibleReleases As Integer = 0
            Try
                Dim UG32VerInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(Application.StartupPath, "Tools\UnattendGen\SelfContained", "x86", "UnattendGen.exe"))
                DynaLog.LogMessage("Version of 32-bit UnattendGen: " & UG32VerInfo.FileVersion)
                DynaLog.LogMessage("--- We are expecting release build " & UnattendGenReleaseTag & " - anything other than that will be discarded ---")
                If UG32VerInfo.FilePrivatePart <> UnattendGenReleaseTag Then
                    DynaLog.LogMessage("This release of UnattendGen has been marked as unsupported and will be deleted in favor of a newer version.")
                    incompatibleReleases += 1
                End If
                Dim UG64VerInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(Application.StartupPath, "Tools\UnattendGen\SelfContained", "amd64", "UnattendGen.exe"))
                DynaLog.LogMessage("Version of 64-bit UnattendGen: " & UG64VerInfo.FileVersion)
                DynaLog.LogMessage("--- We are expecting release build " & UnattendGenReleaseTag & " - anything other than that will be discarded ---")
                If UG64VerInfo.FilePrivatePart <> UnattendGenReleaseTag Then
                    DynaLog.LogMessage("This release of UnattendGen has been marked as unsupported and will be deleted in favor of a newer version.")
                    incompatibleReleases += 1
                End If

                If incompatibleReleases > 0 Then
                    DynaLog.LogMessage("One or more releases have been marked as unsupported. UnattendGen will be redownloaded shortly...")
                    Directory.Delete(Path.Combine(Application.StartupPath, "Tools\UnattendGen\SelfContained"), True)

                    DotNetRuntimeSupported = False
                    PreferSelfContained = False
                    Exit Sub
                End If
            Catch ex As Exception

            End Try
            DotNetRuntimeSupported = True
            PreferSelfContained = True
            Exit Sub
        End If
        DynaLog.LogMessage("Detecting if .NET installations have been made...")
        DynaLog.LogMessage("Do not be confused. This is not .NET Framework.")
        If Not Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "dotnet")) Then
            DynaLog.LogMessage("No installations have been made.")
            DotNetRuntimeSupported = False
            Exit Sub
        End If
        DynaLog.LogMessage("Checking .NET SDK installations...")
        If Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "dotnet\sdk", SDKVersion)) Then
            DynaLog.LogMessage("A compatible .NET SDK installation has been detected.")
            ' .NET SDK exists, skip further checks
            DotNetRuntimeSupported = True
            Exit Sub
        End If
        DynaLog.LogMessage("Checking .NET Runtime installations...")
        If Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "dotnet\shared\Microsoft.NETCore.App"), RuntimeVersion & "*", SearchOption.TopDirectoryOnly).Any() Then
            DynaLog.LogMessage("A compatible .NET Runtime installation has been detected.")
            ' .NET Runtime exists, skip further checks
            DotNetRuntimeSupported = True
            Exit Sub
        End If
    End Sub

    Private Sub NewUnattendWiz_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        StepsTreeView.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.BackColor = BackColor
        ComboBox2.BackColor = BackColor
        ComboBox3.BackColor = BackColor
        ComboBox4.BackColor = BackColor
        ComboBox5.BackColor = BackColor
        ComboBox6.BackColor = BackColor
        ComboBox7.BackColor = BackColor
        ComboBox8.BackColor = BackColor
        ComboBox9.BackColor = BackColor
        ComboBox10.BackColor = BackColor
        ComboBox11.BackColor = BackColor
        ComboBox12.BackColor = BackColor
        ComboBox13.BackColor = BackColor
        CheckedListBox1.BackColor = BackColor
        TextBox1.BackColor = BackColor
        TextBox2.BackColor = BackColor
        TextBox3.BackColor = BackColor
        TextBox4.BackColor = BackColor
        TextBox5.BackColor = BackColor
        TextBox6.BackColor = BackColor
        TextBox7.BackColor = BackColor
        TextBox8.BackColor = BackColor
        TextBox9.BackColor = BackColor
        TextBox10.BackColor = BackColor
        TextBox11.BackColor = BackColor
        TextBox12.BackColor = BackColor
        TextBox13.BackColor = BackColor
        TextBox14.BackColor = BackColor
        TextBox15.BackColor = BackColor
        TextBox16.BackColor = BackColor
        TextBox17.BackColor = BackColor
        TextBox18.BackColor = BackColor
        TextBox19.BackColor = BackColor
        TextBox20.BackColor = BackColor
        TextBox21.BackColor = BackColor
        TextBox22.BackColor = BackColor
        TextBox23.BackColor = BackColor
        NumericUpDown1.BackColor = BackColor
        NumericUpDown2.BackColor = BackColor
        NumericUpDown5.BackColor = BackColor
        NumericUpDown6.BackColor = BackColor
        NumericUpDown7.BackColor = BackColor
        NumericUpDown8.BackColor = BackColor
        GroupBox1.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
        ComboBox2.ForeColor = ForeColor
        ComboBox3.ForeColor = ForeColor
        ComboBox4.ForeColor = ForeColor
        ComboBox5.ForeColor = ForeColor
        ComboBox6.ForeColor = ForeColor
        ComboBox7.ForeColor = ForeColor
        ComboBox8.ForeColor = ForeColor
        ComboBox9.ForeColor = ForeColor
        ComboBox10.ForeColor = ForeColor
        ComboBox11.ForeColor = ForeColor
        ComboBox12.ForeColor = ForeColor
        ComboBox13.ForeColor = ForeColor
        CheckedListBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        TextBox4.ForeColor = ForeColor
        TextBox5.ForeColor = ForeColor
        TextBox6.ForeColor = ForeColor
        TextBox7.ForeColor = ForeColor
        TextBox8.ForeColor = ForeColor
        TextBox9.ForeColor = ForeColor
        TextBox10.ForeColor = ForeColor
        TextBox11.ForeColor = ForeColor
        TextBox12.ForeColor = ForeColor
        TextBox13.ForeColor = ForeColor
        TextBox14.ForeColor = ForeColor
        TextBox15.ForeColor = ForeColor
        TextBox16.ForeColor = ForeColor
        TextBox17.ForeColor = ForeColor
        TextBox18.ForeColor = ForeColor
        TextBox19.ForeColor = ForeColor
        TextBox20.ForeColor = ForeColor
        TextBox21.ForeColor = ForeColor
        TextBox22.ForeColor = ForeColor
        TextBox23.ForeColor = ForeColor
        NumericUpDown1.ForeColor = ForeColor
        NumericUpDown2.ForeColor = ForeColor
        NumericUpDown5.ForeColor = ForeColor
        NumericUpDown6.ForeColor = ForeColor
        NumericUpDown7.ForeColor = ForeColor
        NumericUpDown8.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))

        SidePanel.BackColor = BackColor
        StepsTreeView.ForeColor = ForeColor
        PictureBox2.Image = If(CurrentTheme.IsDark, My.Resources.editor_mode_select, My.Resources.editor_mode)
        PictureBox4.Image = If(CurrentTheme.IsDark, My.Resources.cmps_addfirstcomponent_dark, My.Resources.cmps_addfirstcomponent_light)
        PictureBox5.Image = If(CurrentTheme.IsDark, My.Resources.scripts_addfirstscript_dark, My.Resources.scripts_addfirstscript_light)
        ' Fill in font combinations
        FontFamilyTSCB.Items.Clear()
        For Each fntFamily As FontFamily In FontFamily.Families
            FontFamilyTSCB.Items.Add(fntFamily.Name)
        Next
        InitScintilla("Consolas", 11)
        StepsTreeView.ExpandAll()

        FontFamilyTSCB.SelectedItem = "Consolas"
        SetNodeColors(StepsTreeView.Nodes, BackColor, ForeColor)

        DefaultContents = Scintilla1.Text

        SetDefaultSettings()

        DynaLog.DisableLogging()

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
        ' System components
        If File.Exists(Application.StartupPath & "\AutoUnattend\Component.xml") Then
            SystemComponents = Component.LoadItems(Application.StartupPath & "\AutoUnattend\Component.xml")
            If SystemComponents IsNot Nothing Then
                For Each SystemComponent As Component In SystemComponents
                    ComboBox14.Items.Add(SystemComponent.Id)
                Next
            End If
        End If
        ' Begin reserving components for proper OS installation
        If ReservedComponents.Count > 0 Then ReservedComponents.Clear()
        ReservedComponents.Add(New Component("Microsoft-Windows-Deployment", New Pass("specialize")))
        ReservedComponents.Add(New Component("Microsoft-Windows-International-Core", New Pass("oobeSystem")))
        ReservedComponents.Add(New Component("Microsoft-Windows-International-Core-WinPE", New Pass("windowsPE")))
        ReservedComponents.Add(New Component("Microsoft-Windows-Setup", New Pass("windowsPE")))
        ReservedComponents.Add(New Component("Microsoft-Windows-Shell-Setup", New Pass("specialize")))
        ReservedComponents.Add(New Component("Microsoft-Windows-Shell-Setup", New Pass("oobeSystem")))
        CheckedListBox1.SelectedIndex = 1
        ChangePage(UnattendedWizardPage.Page.WelcomePage)
        VerifyInPages.AddRange(New UnattendedWizardPage.Page() {UnattendedWizardPage.Page.SysConfigPage, UnattendedWizardPage.Page.DiskConfigPage, UnattendedWizardPage.Page.ProductKeyPage, UnattendedWizardPage.Page.UserAccountsPage, UnattendedWizardPage.Page.NetworkConnectionsPage})
        TimeZonePageTimer.Enabled = True
        ' Set PRO edition
        If ComboBox6.SelectedItem = Nothing Then ComboBox6.SelectedItem = "Pro"
        ' Set default auth tech to WPA2
        If ComboBox13.SelectedItem = Nothing Then ComboBox13.SelectedItem = "WPA2-PSK"

        DynaLog.EnableLogging()

        SwitchScript(0)

        ' Detect .NET runtimes/SDKs
        DetectDotNetRuntime("10.0.109", "10.0")
        If Not DotNetRuntimeSupported Then
            DynaLog.LogMessage("Detections have concluded with no recognized .NET Core-based installations. The included copy of UnattendGen cannot be used.")
            DynaLog.LogMessage("Asking user whether or not to download self-contained UnattendGen...")
            If MsgBox(LocalizationService.ForSection("Unattend.Messages")("Requires.Netruntime.Message"), vbYesNo + vbQuestion, LocalizationService.ForSection("Unattend.Messages")("Netruntime.Missing.Title")) = Windows.Forms.DialogResult.Yes Then
                DynaLog.LogMessage("Proceeding to download self-contained UnattendGen...")
                ExpressPanelFooter.Enabled = False
                UnattendGenBW.RunWorkerAsync()
            Else
                DynaLog.LogMessage("No downloads will be performed.")
                Close()
            End If
        Else
            UGNotify.Visible = False
        End If

        ' Detect presence of Windows SIM
        DynaLog.LogMessage("Checking if Windows System Image Manager (SIM) is present on the host system...")
        If File.Exists(Path.Combine(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)),
                                    "Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\WSIM\x86\imgmgr.exe")) Then
            DynaLog.LogMessage("Windows SIM is present on the host system.")
            LinkLabel6.Enabled = True
        Else
            DynaLog.LogMessage("Windows SIM is not present on the host system. This can be installed with the default options of the ADK installer.")
            LinkLabel6.Enabled = False
        End If

        CheckedListBox1.SetItemChecked(0, False)

        ' Preconfigure the keys
        DynaLog.LogMessage("Preconfiguring the system architectures...")
        SelectedArchitectures = New Dictionary(Of DismProcessorArchitecture, Boolean)
        SelectedArchitectures.Add(DismProcessorArchitecture.Intel, False)
        SelectedArchitectures.Add(DismProcessorArchitecture.AMD64, True)
        SelectedArchitectures.Add(DismProcessorArchitecture.ARM64, False)

        CheckedListBox1.SetItemChecked(0, False)
        CheckedListBox1.SetItemChecked(1, True)
        CheckedListBox1.SetItemChecked(2, False)

        Button21.Enabled = MainForm.IsImageMounted
    End Sub

    Sub ReloadSettings()
        DynaLog.LogMessage("Restoring original wizard settings for a new answer file...")
        ' Restore regional configuration
        ComboBox1.SelectedItem = DefaultLanguage.DisplayName
        ComboBox2.SelectedItem = DefaultLocale.DisplayName
        ComboBox3.SelectedItem = DefaultKeybIdentifier.DisplayName
        ComboBox4.SelectedItem = DefaultGeoId.DisplayName
        ' Restore basic system configuration
        CheckedListBox1.SelectedIndex = 1
        Win11Config.LabConfig_BypassRequirements = False
        Win11Config.OOBE_BypassNRO = False
        CheckBox1.Checked = False
        CheckBox2.Checked = False
        RadioButton28.Checked = True
        TextBox16.Text = "return 'DESKTOP-{0}' -f -join ((48..57) + (65..90) | Get-Random -Count 7 | ForEach-Object {[char]$_})"
        CheckBox3.Checked = True
        TextBox1.Text = ""
        CheckBox19.Checked = False
        ' Restore time zone
        ComboBox5.SelectedItem = DefaultOffset.DisplayName
        RadioButton1.Checked = True
        ' Restore disk configuration
        CheckBox4.Checked = True
        RadioButton7.Checked = True
        NumericUpDown1.Value = 300
        CheckBox5.Checked = True
        NumericUpDown2.Value = 1000
        SelectedDiskConfiguration = DefaultDiskConfiguration
        ' Restore product key
        RadioButton13.Checked = True
        ComboBox6.SelectedItem = "Pro"
        TextBox3.Text = ""
        CheckBox21.Checked = False
        ' Restore user accounts
        CheckBox6.Checked = True
        TextBox4.Text = "Admin"
        TextBox6.Text = ""
        TextBox8.Text = ""
        TextBox9.Text = ""
        TextBox11.Text = ""
        TextBox12.Text = ""
        TextBox14.Text = ""
        TextBox15.Text = ""
        TextBox17.Text = ""
        TextBox18.Text = ""
        CheckBox8.Checked = False
        CheckBox9.Checked = False
        CheckBox10.Checked = False
        CheckBox11.Checked = False
        ComboBox7.SelectedIndex = 0
        ComboBox9.SelectedIndex = 1
        ComboBox10.SelectedIndex = 1
        ComboBox11.SelectedIndex = 1
        ComboBox12.SelectedIndex = 1
        CheckBox12.Checked = False
        RadioButton15.Checked = True
        TextBox5.Text = ""
        CheckBox7.Checked = True
        CheckBox18.Checked = False
        ' Restore password expiration
        RadioButton17.Checked = True
        RadioButton19.Checked = True
        NumericUpDown5.Value = 10
        ' Restore Account lockout
        CheckBox13.Checked = False
        RadioButton21.Checked = True
        NumericUpDown6.Value = 10
        NumericUpDown7.Value = 10
        NumericUpDown8.Value = 10
        ' Restore VM support
        ComboBox8.SelectedIndex = 2
        RadioButton24.Checked = True
        ' Restore network settings
        CheckBox14.Checked = True
        RadioButton25.Checked = True
        TextBox7.Text = ""
        CheckBox15.Checked = False
        ComboBox13.SelectedIndex = 1
        TextBox10.Text = ""
        ' Restore system telemetry
        CheckBox16.Checked = False
        RadioButton26.Checked = True
        ' Restore default script settings
        CheckBox20.Checked = False
        ' Restore default selections for components
        SystemComponentsEx.Clear()
        NoSpecifiedComponentsPanel.Visible = True
        ComponentEditorPanel.Visible = False
        Label60.Visible = False
        Button10.Enabled = False
        Button6.Enabled = False
        Button7.Enabled = False
        Button8.Enabled = False
        Button9.Enabled = False
        LinkLabel9.Visible = False

        ' Restore variables
        UserAccountsList.Clear()
        SetDefaultSettings()

        ' Reconfigure the keys
        DynaLog.LogMessage("Reconfiguring the system architectures...")
        CheckedListBox1.SetItemChecked(0, False)
        CheckedListBox1.SetItemChecked(1, True)
        CheckedListBox1.SetItemChecked(2, False)

        SwitchScript(0)
    End Sub

    Sub SelectTreeNode(NodeIndex As Integer)
        StepsTreeView.SelectedNode = StepsTreeView.Nodes(NodeIndex)
        StepsTreeView.Refresh()
    End Sub

    Sub ChangePage(NewPage As UnattendedWizardPage.Page)
        DynaLog.LogMessage("Changing current page of the wizard...")
        DynaLog.LogMessage("New page to load: " & NewPage.ToString())
        If NewPage > CurrentWizardPage.WizardPage AndAlso VerifyInPages.Contains(CurrentWizardPage.WizardPage) Then
            If Not VerifyOptionsInPage(CurrentWizardPage.WizardPage) Then Exit Sub
        ElseIf NewPage > CurrentWizardPage.WizardPage AndAlso NewPage = UnattendedWizardPage.Page.ReviewPage Then
            ShowSettingOverview()
        End If
        WelcomePanel.Visible = (NewPage = UnattendedWizardPage.Page.WelcomePage)
        RegionalSettingsPanel.Visible = (NewPage = UnattendedWizardPage.Page.RegionalPage)
        SysConfigPanel.Visible = (NewPage = UnattendedWizardPage.Page.SysConfigPage)
        TimeZonePanel.Visible = (NewPage = UnattendedWizardPage.Page.TimeZonePage)
        DiskConfigurationPanel.Visible = (NewPage = UnattendedWizardPage.Page.DiskConfigPage)
        ProductKeyPanel.Visible = (NewPage = UnattendedWizardPage.Page.ProductKeyPage)
        UserAccountPanel.Visible = (NewPage = UnattendedWizardPage.Page.UserAccountsPage)
        PWExpirationPanel.Visible = (NewPage = UnattendedWizardPage.Page.PWExpirationPage)
        AccountLockoutPanel.Visible = (NewPage = UnattendedWizardPage.Page.AccountLockoutPage)
        VirtualMachinePanel.Visible = (NewPage = UnattendedWizardPage.Page.VirtualMachinePage)
        NetworkConnectionPanel.Visible = (NewPage = UnattendedWizardPage.Page.NetworkConnectionsPage)
        SystemTelemetryPanel.Visible = (NewPage = UnattendedWizardPage.Page.SystemTelemetryPage)
        PostInstallPanel.Visible = (NewPage = UnattendedWizardPage.Page.PostInstallPage)
        ComponentPanel.Visible = (NewPage = UnattendedWizardPage.Page.ComponentPage)
        FinalReviewPanel.Visible = (NewPage = UnattendedWizardPage.Page.ReviewPage)
        UnattendProgressPanel.Visible = (NewPage = UnattendedWizardPage.Page.ProgressPage)
        FinishPanel.Visible = (NewPage = UnattendedWizardPage.Page.FinishPage)
        CurrentWizardPage.WizardPage = NewPage
        Next_Button.Enabled = (Not NewPage <> UnattendedWizardPage.Page.FinishPage) OrElse (Not NewPage + 1 >= UnattendedWizardPage.PageCount)
        Cancel_Button.Enabled = Not (NewPage = UnattendedWizardPage.Page.FinishPage)
        Back_Button.Enabled = Not (NewPage = UnattendedWizardPage.Page.WelcomePage) And Not (NewPage = UnattendedWizardPage.Page.FinishPage)
        Button12.Visible = New UnattendedWizardPage.Page() {UnattendedWizardPage.Page.SysConfigPage,
                                                            UnattendedWizardPage.Page.UserAccountsPage,
                                                            UnattendedWizardPage.Page.ComponentPage}.Contains(NewPage)

        Next_Button.Text = If(NewPage = UnattendedWizardPage.Page.FinishPage, "Close", "Next")

        ' Select tree nodes according to page
        Select Case CurrentWizardPage.WizardPage
            Case UnattendedWizardPage.Page.WelcomePage
                SelectTreeNode(0)
            Case UnattendedWizardPage.Page.RegionalPage
                SelectTreeNode(1)
            Case UnattendedWizardPage.Page.SysConfigPage
                SelectTreeNode(2)
            Case UnattendedWizardPage.Page.TimeZonePage
                SelectTreeNode(3)
            Case UnattendedWizardPage.Page.DiskConfigPage
                SelectTreeNode(4)
            Case UnattendedWizardPage.Page.ProductKeyPage
                SelectTreeNode(5)
            Case UnattendedWizardPage.Page.UserAccountsPage, UnattendedWizardPage.Page.PWExpirationPage, UnattendedWizardPage.Page.AccountLockoutPage
                SelectTreeNode(6)
            Case UnattendedWizardPage.Page.VirtualMachinePage
                SelectTreeNode(7)
            Case UnattendedWizardPage.Page.NetworkConnectionsPage
                SelectTreeNode(8)
            Case UnattendedWizardPage.Page.SystemTelemetryPage
                SelectTreeNode(9)
            Case UnattendedWizardPage.Page.PostInstallPage
                SelectTreeNode(10)
            Case UnattendedWizardPage.Page.ComponentPage
                SelectTreeNode(11)
            Case UnattendedWizardPage.Page.ReviewPage, UnattendedWizardPage.Page.ProgressPage, UnattendedWizardPage.Page.FinishPage
                SelectTreeNode(12)
        End Select

        ' Change sizes of controls if the normal resize event does not work
        'AutoDiskConfigPanel.Width = ManualPartPanel.Width - (AutoDiskConfigPanel.Margin.Left * 2) - 4
        GroupBox1.Width = ManualAccountPanel.Width - (GroupBox1.Margin.Left * 2) - 4
        AccountsPanel.Width = UserAccountListing.Width
        UserAccountListing.Width = ManualAccountPanel.Width - (UserAccountListing.Margin.Left * 2) - 4
        WirelessNetworkSettingsPanel.Width = ManualNetworkConfigPanel.Width - (WirelessNetworkSettingsPanel.Margin.Left * 2) - 4

        ExpressPanelFooter.Enabled = Not (CurrentWizardPage.WizardPage = UnattendedWizardPage.Page.ProgressPage)
        If CurrentWizardPage.WizardPage = UnattendedWizardPage.Page.ProgressPage Then
            ' Save post-install scripts
            DynaLog.LogMessage("Saving post-install script configuration...")
            SaveConfiguredScripts(CurrentlyEditedStage)
            DynaLog.LogMessage("Configuring save dialog initial location depending on whether or not a project is loaded...")
            ' Detect if a project has been loaded
            If MainForm.isProjectLoaded And Not (MainForm.OnlineManagement Or MainForm.OfflineManagement) Then
                DynaLog.LogMessage("A project has been loaded and we are not managing any Windows installation.")
                SaveFileDialog1.InitialDirectory = Path.Combine(MainForm.projPath, "unattend_xml")
            Else
                DynaLog.LogMessage("Either no project has been loaded or we are managing a Windows installation.")
                SaveFileDialog1.InitialDirectory = ""
            End If
            SaveFileDialog1.FileName = "autounattend_" & Now.ToString().Replace("/", "-").Trim().Replace(":", "-").Trim() & ".xml"
            SaveFileDialog1.ShowDialog(Me)
            UnattendGeneratorBW.RunWorkerAsync()
        ElseIf CurrentWizardPage.WizardPage = UnattendedWizardPage.Page.ReviewPage Then
            SaveTarget = ""
        End If
    End Sub

    Function VerifyOptionsInPage(WizardPage As UnattendedWizardPage.Page) As Boolean
        DynaLog.LogMessage("Verifying user options before moving on to next page...")
        DynaLog.LogMessage("Page in which we need to verify user settings: " & WizardPage.ToString())
        Select Case WizardPage
            Case UnattendedWizardPage.Page.SysConfigPage
                DynaLog.LogMessage("Checking selected architectures...")
                If CheckedListBox1.CheckedItems.Count = 0 Then
                    DynaLog.LogMessage("No architectures have been selected.")
                    MessageBox.Show(LocalizationService.ForSection("Unattend.Validation")("Arch.Try.Label"), LocalizationService.ForSection("Unattend.Validation")("ValidationError.Title"))
                    Return False
                End If
                If RadioButton28.Checked Then
                    If Not PCName.DefaultName Then
                        DynaLog.LogMessage("Checking computer name...")
                        Dim testerPC As ComputerName = ComputerNameValidator.ValidateComputerName(TextBox1.Text)
                        If Not testerPC.Valid AndAlso testerPC.ErrorMessage <> "" Then
                            DynaLog.LogMessage("This computer name is not valid. Look above for reasons why.")
                            MessageBox.Show(testerPC.ErrorMessage, LocalizationService.ForSection("Unattend.Validation")("Computer.Name.Error.Title"))
                            Return False
                        End If
                    End If
                Else
                    DynaLog.LogMessage("Checking if a computer name script has been specified...")
                    If String.IsNullOrEmpty(PCNameScript) Then
                        DynaLog.LogMessage("No script has been provided")
                        MessageBox.Show(LocalizationService.ForSection("Unattend.Validation")("Script.Has.None.Label"), LocalizationService.ForSection("Unattend.Validation")("Computer.Name.Error.Title"))
                        Return False
                    End If
                End If
            Case UnattendedWizardPage.Page.ProductKeyPage
                If Not GenericChosen Then
                    DynaLog.LogMessage("Checking user-specified product key...")
                    If TextBox3.Text = "" Then
                        DynaLog.LogMessage("No product key has been specified.")
                        MessageBox.Show(LocalizationService.ForSection("Unattend.Validation")("Type.ProductKey.Label"), LocalizationService.ForSection("Unattend.Validation")("ProductKeyError.Title"))
                        Return False
                    ElseIf TextBox3.Text <> "" And TextBox3.Text.Length <> 29 Then
                        DynaLog.LogMessage("Not all characters of the product key have been typed. Expected length: 29; Current length: " & TextBox3.Text.Length)
                        MessageBox.Show(LocalizationService.ForSection("Unattend.Validation")("Type.Product.Label"), LocalizationService.ForSection("Unattend.Validation")("ProductKeyError.Title"))
                        Return False
                    ElseIf TextBox3.Text <> "" And TextBox3.Text.Length = 29 Then
                        DynaLog.LogMessage("Validating product key...")
                        Dim pKey As ProductKey = ProductKeyValidator.ValidateProductKey(TextBox3.Text)
                        If Not pKey.Valid Then
                            DynaLog.LogMessage("Previously run regex match did not return results. This product key is bad.")
                            MessageBox.Show(LocalizationService.ForSection("Unattend.Validation").Format("ProductKey.Entered.Ill.Label", TextBox3.Text), LocalizationService.ForSection("Unattend.Validation")("ProductKeyError.Title"))
                            Return False
                        End If
                    End If
                End If
            Case UnattendedWizardPage.Page.UserAccountsPage
                DynaLog.LogMessage("Validating user accounts...")
                Dim validationResults As UserValidationResults = UserValidator.ValidateUsers(UserAccountsList, PCName)
                If Not UserAccountsInteractive AndAlso Not MicrosoftAccountInteractive AndAlso Not validationResults.IsValid Then
                    DynaLog.LogMessage("Validation has failed due to the reasons that appear above this line.")
                    MessageBox.Show(LocalizationService.ForSection("Unattend.Validation").Format("Problem.One.Message", validationResults.ValidationErrorReason), LocalizationService.ForSection("Unattend.Validation")("User.Accounts.Error.Title"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return False
                End If
                Dim invalidChars As Char() = {"/", "\", "[", "]", ":", ";", "|", "=", ",", "+", "*", "?", "<", ">", Quote, "%"}
                If Not UserAccountsInteractive AndAlso Not MicrosoftAccountInteractive Then
                    DynaLog.LogMessage("Checking account names and groups...")
                    DynaLog.LogMessage("This process will trim any invalid characters from user accounts automatically.")
                    Dim AtLeastOneAdmin As Boolean = False
                    If UserAccountsList.Count > 0 Then
                        For Each UserAccount As User In UserAccountsList
                            UserAccount.Name = New String(UserAccount.Name.Where(Function(c) Not invalidChars.Contains(c)).ToArray()).TrimEnd(".")
                            If UserAccount.Group = UserGroup.Administrators Then
                                AtLeastOneAdmin = True
                                Exit For
                            End If
                        Next
                    End If
                    If Not AtLeastOneAdmin Then
                        DynaLog.LogMessage("No users have been detected as part of the Administrators group. All users are part of the Users group.")
                        DynaLog.LogMessage("At least one user must be part of the Administrators group.")
                        MessageBox.Show(LocalizationService.ForSection("Unattend.Validation")("Least.One.Account.Message"), LocalizationService.ForSection("Unattend.Validation")("User.Accounts.Error.Title"))
                        Return False
                    End If
                End If
            Case UnattendedWizardPage.Page.NetworkConnectionsPage
                DynaLog.LogMessage("Validating wireless settings if they have been specified...")
                If Not NetworkConfigInteractive AndAlso Not NetworkConfigManualSkip AndAlso Not WirelessValidator.ValidateWiFi(SelectedNetworkConfiguration) Then
                    DynaLog.LogMessage("Wireless setting validation has failed.")
                    MessageBox.Show(LocalizationService.ForSection("Unattend.Validation")("Problem.Wireless.Message"), LocalizationService.ForSection("Unattend.Validation")("WirelessError.Title"))
                    Return False
                End If
        End Select
        Return True
    End Function

    Function ShowArchitectures(Architectures As Dictionary(Of DismProcessorArchitecture, Boolean)) As String
        Dim architectureList As New List(Of String)

        If Architectures IsNot Nothing Then
            For Each Architecture As DismProcessorArchitecture In Architectures.Keys
                If Architectures(Architecture) Then
                    architectureList.Add(Utilities.Casters.CastDismArchitecture(Architecture))
                End If
            Next
        End If
        Return String.Join("; ", architectureList.ToArray())
    End Function

    Sub ShowSettingOverview()
        DynaLog.LogMessage("Showing overview of settings...")
        TextBox13.Clear()
        ' Display settings in the following order:
        TextBox13.Text = LocalizationService.ForSection("UnattendWizard.Review")("Configs.UnattendAnswer.Label") & CrLf
        ' 1. -- REGIONAL CONFIGURATION
        TextBox13.AppendText("Regional settings: " & If(RegionalInteractive, "configured during setup" & CrLf, CrLf))
        If Not RegionalInteractive Then
            TextBox13.AppendText("- System language: " & SelectedLanguage.DisplayName & CrLf &
                                 "- System locale: " & SelectedLocale.DisplayName & CrLf &
                                 "- Keyboard/IME: " & SelectedKeybIdentifier.DisplayName & CrLf &
                                 "- Home location: " & SelectedGeoId.DisplayName & CrLf)
        End If
        ' 2. -- BASIC SYSTEM CONFIGURATION
        TextBox13.AppendText("Basic system configuration: " & CrLf &
                             "- Processor architectures: " & ShowArchitectures(SelectedArchitectures) & CrLf &
                             "- Windows 11 Settings:" & CrLf &
                             "    - Bypass System Requirements? " & If(Win11Config.LabConfig_BypassRequirements, "Yes", "No") & CrLf &
                             "    - Bypass Mandatory Network Connection? " & If(Win11Config.OOBE_BypassNRO, "Yes", "No") & CrLf &
                             "- Computer name: " & If(String.IsNullOrEmpty(PCNameScript),
                                                      If(PCName.DefaultName,
                                                         "random by Windows",
                                                         PCName.Name),
                                                      "determined by script " & Quote & PCNameScript & Quote) & CrLf &
                             "- Will a configuration set or distribution share be used? " & If(UseConfigSet, "Yes", "No") & CrLf)
        ' 3. -- TIME ZONE
        TextBox13.AppendText("Time zone configuration: " & If(TimeOffsetInteractive, "based on regional settings" & CrLf, CrLf))
        If Not TimeOffsetInteractive Then
            TextBox13.AppendText("- Time zone: " & SelectedOffset.DisplayName & CrLf)
        End If
        ' 4. -- DISK CONFIGURATION
        TextBox13.AppendText("Disk configuration: " & If(DiskConfigurationInteractive, "configured during setup" & CrLf, CrLf))
        If Not DiskConfigurationInteractive Then
            TextBox13.AppendText("- Disk configuration mode: automatically configure Disk 0" & CrLf)
            TextBox13.AppendText("    - Partition table: " & If(SelectedDiskConfiguration.PartStyle = PartitionStyle.GPT, "GPT (UEFI)", "MBR (BIOS/CSM)") & CrLf)
            If SelectedDiskConfiguration.PartStyle = PartitionStyle.GPT Then
                TextBox13.AppendText("      - EFI System Partition Size: " & SelectedDiskConfiguration.ESPSize & " MB" & CrLf)
            End If
            TextBox13.AppendText("    - Install a Recovery Environment? " & If(SelectedDiskConfiguration.InstallRecEnv, "Yes", "No") & CrLf)
            If SelectedDiskConfiguration.InstallRecEnv Then
                TextBox13.AppendText("      - Location of the Recovery Environment: " & If(SelectedDiskConfiguration.RecEnvPartition = RecoveryEnvironmentLocation.WinREPartition, "Recovery partition", "Windows partition") & CrLf)
                If SelectedDiskConfiguration.RecEnvPartition = RecoveryEnvironmentLocation.WinREPartition Then
                    TextBox13.AppendText("        - Recovery Partition Size: " & SelectedDiskConfiguration.RecEnvSize & " MB" & CrLf)
                End If
            End If
        End If
        ' 5. -- PRODUCT KEY
        TextBox13.AppendText("Product key: " & If(CheckBox21.Checked, "get from firmware" & CrLf, If(GenericChosen, "generic" & CrLf, "custom" & CrLf)) &
                             "- Key: " & SelectedKey.Key & CrLf)
        ' 6. -- USER ACCOUNTS
        TextBox13.AppendText("User account settings: " & If(UserAccountsInteractive, "configured during setup" & CrLf, CrLf))
        If Not UserAccountsInteractive And Not MicrosoftAccountInteractive Then
            For Each UserAccount As User In UserAccountsList
                TextBox13.AppendText("- Account " & UserAccountsList.IndexOf(UserAccount) + 1 & "? " & If(UserAccount.Enabled, "Yes", "No") & CrLf)
                If UserAccount.Enabled Then
                    TextBox13.AppendText("    - Name: " & UserAccount.Name & CrLf &
                                         "    - Display Name: " & UserAccount.DisplayName & CrLf &
                                         "    - Password: " & UserAccount.Password & CrLf &
                                         "    - Group: " & If(UserAccount.Group = UserGroup.Administrators, "Administrators", "Users") & CrLf)
                End If
            Next
            ' First logon settings
            TextBox13.AppendText("- Log on as an Administrator account? " & If(AutoLogon.EnableAutoLogon, "Yes", "No") & CrLf)
            If AutoLogon.EnableAutoLogon Then
                TextBox13.AppendText("    - Administrator account: " & If(AutoLogon.LogonMode = AutoLogonMode.FirstAdmin, "first admin account created", "built-in Administrator account") & CrLf)
                If AutoLogon.LogonMode = AutoLogonMode.WindowsAdmin Then
                    TextBox13.AppendText("      - Password for built-in administrator: " & AutoLogon.LogonPassword & CrLf)
                End If
            End If
            TextBox13.AppendText("- Obscure passwords with Base64? " & If(PasswordObfuscate, "Yes", "No") & CrLf)
        ElseIf (Not UserAccountsInteractive) And MicrosoftAccountInteractive Then
            TextBox13.AppendText("- The target system will ask for a Microsoft account" & CrLf)
        End If
        TextBox13.AppendText("Password expiration policy: " & If(SelectedExpirationSettings.Mode = PasswordExpirationMode.NIST_Limited, "enabled" & CrLf, "disabled" & CrLf))
        If SelectedExpirationSettings.Mode = PasswordExpirationMode.NIST_Limited Then
            TextBox13.AppendText("- Expiration policy mode: " & If(SelectedExpirationSettings.WindowsDefault, "Windows default (42 days)", "custom") & CrLf)
            If Not SelectedExpirationSettings.WindowsDefault Then
                TextBox13.AppendText("    - Expiration period: " & SelectedExpirationSettings.Days & " days" & CrLf)
            End If
        End If
        TextBox13.AppendText("Account Lockout policy status: " & If(SelectedLockoutSettings.Enabled, "enabled" & CrLf, "disabled" & CrLf))
        If SelectedLockoutSettings.Enabled Then
            TextBox13.AppendText("- Account Lockout policies: " & If(SelectedLockoutSettings.DefaultPolicy, "default", "custom") & CrLf)
            If Not SelectedLockoutSettings.DefaultPolicy Then
                TextBox13.AppendText("    - After " & SelectedLockoutSettings.TimedLockoutSettings.FailedAttempts & " failed attempts within " & SelectedLockoutSettings.TimedLockoutSettings.Timeframe & " minutes, unlock account after " & SelectedLockoutSettings.TimedLockoutSettings.AutoUnlockTime & " minutes" & CrLf)
            End If
        End If
        ' 7. -- VIRTUAL MACHINE SUPPORT
        TextBox13.AppendText("Virtual Machine Support: " & If(VirtualMachineSupported, "enabled" & CrLf, "disabled" & CrLf))
        If VirtualMachineSupported Then
            Select Case SelectedVMSettings.Provider
                Case VMProvider.VirtualBox_GAs
                    TextBox13.AppendText("- Selected Hypervisor: Oracle VM VirtualBox (VirtualBox Guest Additions)" & CrLf)
                Case VMProvider.VMware_Tools
                    TextBox13.AppendText("- Selected Hypervisor: VMware (VMware Tools)" & CrLf)
                Case VMProvider.VirtIO_Guest_Tools
                    TextBox13.AppendText("- Selected Hypervisor: QEMU/Proxmox VE/etc. (VirtIO Guest Tools)" & CrLf)
                Case VMProvider.Parallels
                    TextBox13.AppendText("- Selected Hypervisor: Parallels (Parallels Tools)" & CrLf)
            End Select
        End If
        ' 8. -- WIRELESS NETWORKING
        TextBox13.AppendText("Wireless networking settings: " & If(NetworkConfigInteractive, "configured during setup" & CrLf, CrLf))
        If Not NetworkConfigInteractive Then
            TextBox13.AppendText("- Skip configuration? " & If(NetworkConfigManualSkip, "Yes", "No") & CrLf)
            If Not NetworkConfigManualSkip Then
                TextBox13.AppendText("    - SSID: " & SelectedNetworkConfiguration.SSID & CrLf &
                                     "    - Connect even if not broadcasting? " & If(SelectedNetworkConfiguration.ConnectWithoutBroadcast, "Yes", "No") & CrLf)
                Select Case SelectedNetworkConfiguration.Authentication
                    Case WiFiAuthenticationMode.Open
                        TextBox13.AppendText("    - Authentication mode: open" & CrLf)
                    Case WiFiAuthenticationMode.WPA2_PSK
                        TextBox13.AppendText("    - Authentication mode: WPA2-Personal" & CrLf)
                    Case WiFiAuthenticationMode.WPA3_SAE
                        TextBox13.AppendText("    - Authentication mode: WPA3 (Simultaneous Authentication of Equals)" & CrLf)
                End Select
                If SelectedNetworkConfiguration.Authentication <> WiFiAuthenticationMode.Open Then TextBox13.AppendText("    - Password: " & New String("*", SelectedNetworkConfiguration.Password.Length) & " (hidden for your security)" & CrLf)
            End If
        End If
        ' 9. -- SYSTEM TELEMETRY
        TextBox13.AppendText("System telemetry settings: " & If(SystemTelemetryInteractive, "configured during setup" & CrLf, CrLf))
        If Not SystemTelemetryInteractive Then
            TextBox13.AppendText("- (Attempt to) disable telemetry? " & If(Not SelectedTelemetrySettings.Enabled, "Yes", "No") & CrLf)
        End If
        ' Post Install Scripts and Component Manager will be added in a future release
        ' 11. -- COMPONENTS
        TextBox13.AppendText("Additional components: " & If(Not SystemComponentsEx.Count = 0, SystemComponentsEx.Count, "none") & CrLf)
        If SystemComponentsEx.Count > 0 Then
            For Each SpecifiedComponent As Component In SystemComponentsEx
                TextBox13.AppendText("Component Details:" & CrLf &
                                     "  - Component ID: " & SpecifiedComponent.Id & CrLf &
                                     "  - Pass: " & SpecifiedComponent.Pass.Name & CrLf)
            Next
        End If
    End Sub

    Private Sub ExpressPanelTrigger_MouseEnter(sender As Object, e As EventArgs) Handles ExpressPanelTrigger.MouseEnter
        If ExpressPanelContainer.Visible Then
            ExpressPanelTrigger.BackColor = Color.FromKnownColor(KnownColor.HotTrack)
        Else
            ExpressPanelTrigger.BackColor = If(CurrentTheme.IsDark, CurrentTheme.BackgroundColor, Color.Gainsboro)
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
        ExpressPanelTrigger.ForeColor = CurrentTheme.ForegroundColor
        PictureBox1.Image = My.Resources.express_mode_select
        EditorPanelTrigger.BackColor = SidePanel.BackColor
        EditorPanelTrigger.ForeColor = If(CurrentTheme.IsDark, Color.LightGray, Color.Black)
        PictureBox2.Image = If(CurrentTheme.IsDark, My.Resources.editor_mode_select, My.Resources.editor_mode)
        PictureBox3.Image = My.Resources.express_mode_fc
        Label3.Text = LocalizationService.ForSection("Unattend.Mode")("ExpressMode.Title")
        Label4.Text = LocalizationService.ForSection("Unattend.Mode")("WizardHelp.Description")
        FooterContainer.Visible = True
    End Sub

    Private Sub EditorPanelTrigger_MouseEnter(sender As Object, e As EventArgs) Handles EditorPanelTrigger.MouseEnter
        If EditorPanelContainer.Visible Then
            EditorPanelTrigger.BackColor = Color.FromKnownColor(KnownColor.HotTrack)
        Else
            EditorPanelTrigger.BackColor = If(CurrentTheme.IsDark, CurrentTheme.BackgroundColor, Color.Gainsboro)
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
            EditorPanelTrigger.BackColor = If(CurrentTheme.IsDark, Color.FromArgb(36, 36, 36), Color.Silver)
        End If
    End Sub

    Private Sub EditorPanelTrigger_MouseUp(sender As Object, e As MouseEventArgs) Handles EditorPanelTrigger.MouseUp
        If EditorPanelContainer.Visible Then
            EditorPanelTrigger.BackColor = Color.FromKnownColor(KnownColor.HotTrack)
        Else
            EditorPanelTrigger.BackColor = If(CurrentTheme.IsDark, CurrentTheme.BackgroundColor, Color.Gainsboro)
        End If
    End Sub

    Private Sub EditorPanelTrigger_Click(sender As Object, e As EventArgs) Handles EditorPanelTrigger.Click
        IsInExpress = False
        StepsTreeView.Enabled = False
        EditorPanelContainer.Visible = True
        ExpressPanelContainer.Visible = False
        ExpressPanelTrigger.BackColor = SidePanel.BackColor
        ExpressPanelTrigger.ForeColor = If(CurrentTheme.IsDark, Color.LightGray, Color.Black)
        PictureBox1.Image = If(CurrentTheme.IsDark, My.Resources.express_mode_select, My.Resources.express_mode)
        EditorPanelTrigger.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        EditorPanelTrigger.ForeColor = CurrentTheme.ForegroundColor
        PictureBox2.Image = My.Resources.editor_mode_select
        PictureBox3.Image = My.Resources.editor_mode_fc
        Label3.Text = LocalizationService.ForSection("Unattend.Mode")("EditorMode.Title")
        Label4.Text = LocalizationService.ForSection("Unattend.Mode")("CreateUnattended.Description")
        FooterContainer.Visible = False
    End Sub

    Private Sub Back_Button_Click(sender As Object, e As EventArgs) Handles Back_Button.Click
        ChangePage(CurrentWizardPage.WizardPage - 1)
    End Sub

    Private Sub Next_Button_Click(sender As Object, e As EventArgs) Handles Next_Button.Click
        If CurrentWizardPage.WizardPage = UnattendedWizardPage.Page.FinishPage Then
            Close()
        Else
            ChangePage(CurrentWizardPage.WizardPage + 1)
        End If
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
        Try
            If New StackFrame(6).GetMethod().Name = "ReloadSettings" Then
                DynaLog.LogMessage("The text box contents have been cleared by the setting reload method. Skipping checks...")
                Exit Sub
            End If
        Catch ex As Exception
            ' Continue the method
        End Try
        ' Hold default value for now
        Dim defVal As Boolean = False
        defVal = PCName.DefaultName
        PCName = ComputerNameValidator.ValidateComputerName(TextBox1.Text)
        PCName.DefaultName = defVal
        If Not PCName.Valid AndAlso PCName.ErrorMessage <> "" Then
            Label63.Visible = True
        Else
            Label63.Visible = False
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
        DiskConfigurationInteractive = CheckBox4.Checked
        AutoDiskConfigPanel.Enabled = Not DiskConfigurationInteractive
    End Sub

    Private Sub RadioButton7_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton7.CheckedChanged
        ESPPanel.Enabled = RadioButton7.Checked
        SelectedDiskConfiguration.PartStyle = If(RadioButton7.Checked, PartitionStyle.GPT, PartitionStyle.MBR)
    End Sub

    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        SelectedDiskConfiguration.InstallRecEnv = CheckBox5.Checked
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        SelectedDiskConfiguration.ESPSize = NumericUpDown1.Value
    End Sub

    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown2.ValueChanged
        SelectedDiskConfiguration.RecEnvSize = NumericUpDown2.Value
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

    Sub ModifyUserDetails(index As Integer, enabled As Boolean, name As String, displayName As String, password As String, group As UserGroup)
        If UserAccountsList Is Nothing OrElse UserAccountsList.Count = 0 Then Exit Sub
        UserAccountsList(index).Enabled = enabled
        UserAccountsList(index).Name = name
        UserAccountsList(index).DisplayName = If(String.IsNullOrWhiteSpace(displayName), name, displayName)
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
        ModifyUserDetails(1, CheckBox8.Checked, TextBox8.Text, If(CheckBox24.Checked, TextBox20.Text, ""), TextBox9.Text, GroupFromSelectedItem(ComboBox9.SelectedIndex))
        TextBox8.Enabled = CheckBox8.Checked
        TextBox9.Enabled = CheckBox8.Checked
        ComboBox9.Enabled = CheckBox8.Checked
        DisplayNamePanel2.Enabled = CheckBox8.Checked
    End Sub

    Private Sub CheckBox9_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox9.CheckedChanged
        ModifyUserDetails(2, CheckBox9.Checked, TextBox11.Text, If(CheckBox25.Checked, TextBox21.Text, ""), TextBox12.Text, GroupFromSelectedItem(ComboBox10.SelectedIndex))
        TextBox11.Enabled = CheckBox9.Checked
        TextBox12.Enabled = CheckBox9.Checked
        ComboBox10.Enabled = CheckBox9.Checked
        DisplayNamePanel3.Enabled = CheckBox9.Checked
    End Sub

    Private Sub CheckBox10_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox10.CheckedChanged
        ModifyUserDetails(3, CheckBox10.Checked, TextBox14.Text, If(CheckBox26.Checked, TextBox22.Text, ""), TextBox15.Text, GroupFromSelectedItem(ComboBox11.SelectedIndex))
        TextBox14.Enabled = CheckBox10.Checked
        TextBox15.Enabled = CheckBox10.Checked
        ComboBox11.Enabled = CheckBox10.Checked
        DisplayNamePanel4.Enabled = CheckBox10.Checked
    End Sub

    Private Sub CheckBox11_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox11.CheckedChanged
        ModifyUserDetails(4, CheckBox11.Checked, TextBox17.Text, If(CheckBox27.Checked, TextBox23.Text, ""), TextBox18.Text, GroupFromSelectedItem(ComboBox12.SelectedIndex))
        TextBox17.Enabled = CheckBox11.Checked
        TextBox18.Enabled = CheckBox11.Checked
        ComboBox12.Enabled = CheckBox11.Checked
        DisplayNamePanel5.Enabled = CheckBox11.Checked
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        ModifyUserDetails(0, True, TextBox4.Text, If(CheckBox23.Checked, TextBox19.Text, ""), TextBox6.Text, GroupFromSelectedItem(ComboBox7.SelectedIndex))
    End Sub

    Private Sub TextBox6_TextChanged(sender As Object, e As EventArgs) Handles TextBox6.TextChanged
        ModifyUserDetails(0, True, TextBox4.Text, If(CheckBox23.Checked, TextBox19.Text, ""), TextBox6.Text, GroupFromSelectedItem(ComboBox7.SelectedIndex))
    End Sub

    Private Sub TextBox8_TextChanged(sender As Object, e As EventArgs) Handles TextBox8.TextChanged
        ModifyUserDetails(1, CheckBox8.Checked, TextBox8.Text, If(CheckBox24.Checked, TextBox20.Text, ""), TextBox9.Text, GroupFromSelectedItem(ComboBox9.SelectedIndex))
    End Sub

    Private Sub TextBox9_TextChanged(sender As Object, e As EventArgs) Handles TextBox9.TextChanged
        ModifyUserDetails(1, CheckBox8.Checked, TextBox8.Text, If(CheckBox24.Checked, TextBox20.Text, ""), TextBox9.Text, GroupFromSelectedItem(ComboBox9.SelectedIndex))
    End Sub

    Private Sub TextBox11_TextChanged(sender As Object, e As EventArgs) Handles TextBox11.TextChanged
        ModifyUserDetails(2, CheckBox9.Checked, TextBox11.Text, If(CheckBox25.Checked, TextBox21.Text, ""), TextBox12.Text, GroupFromSelectedItem(ComboBox10.SelectedIndex))
    End Sub

    Private Sub TextBox12_TextChanged(sender As Object, e As EventArgs) Handles TextBox12.TextChanged
        ModifyUserDetails(2, CheckBox9.Checked, TextBox11.Text, If(CheckBox25.Checked, TextBox21.Text, ""), TextBox12.Text, GroupFromSelectedItem(ComboBox10.SelectedIndex))
    End Sub

    Private Sub TextBox14_TextChanged(sender As Object, e As EventArgs) Handles TextBox14.TextChanged
        ModifyUserDetails(3, CheckBox10.Checked, TextBox14.Text, If(CheckBox26.Checked, TextBox22.Text, ""), TextBox15.Text, GroupFromSelectedItem(ComboBox11.SelectedIndex))
    End Sub

    Private Sub TextBox15_TextChanged(sender As Object, e As EventArgs) Handles TextBox15.TextChanged
        ModifyUserDetails(3, CheckBox10.Checked, TextBox14.Text, If(CheckBox26.Checked, TextBox22.Text, ""), TextBox15.Text, GroupFromSelectedItem(ComboBox11.SelectedIndex))
    End Sub

    Private Sub TextBox17_TextChanged(sender As Object, e As EventArgs) Handles TextBox17.TextChanged
        ModifyUserDetails(4, CheckBox11.Checked, TextBox17.Text, If(CheckBox27.Checked, TextBox23.Text, ""), TextBox18.Text, GroupFromSelectedItem(ComboBox12.SelectedIndex))
    End Sub

    Private Sub TextBox18_TextChanged(sender As Object, e As EventArgs) Handles TextBox18.TextChanged
        ModifyUserDetails(4, CheckBox11.Checked, TextBox17.Text, If(CheckBox27.Checked, TextBox23.Text, ""), TextBox18.Text, GroupFromSelectedItem(ComboBox12.SelectedIndex))
    End Sub

    Private Sub ComboBox7_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox7.SelectedIndexChanged
        ModifyUserDetails(0, True, TextBox4.Text, If(CheckBox23.Checked, TextBox19.Text, ""), TextBox6.Text, GroupFromSelectedItem(ComboBox7.SelectedIndex))
    End Sub

    Private Sub ComboBox9_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox9.SelectedIndexChanged
        ModifyUserDetails(1, CheckBox8.Checked, TextBox8.Text, If(CheckBox24.Checked, TextBox20.Text, ""), TextBox9.Text, GroupFromSelectedItem(ComboBox9.SelectedIndex))
    End Sub

    Private Sub ComboBox10_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox10.SelectedIndexChanged
        ModifyUserDetails(2, CheckBox9.Checked, TextBox11.Text, If(CheckBox25.Checked, TextBox21.Text, ""), TextBox12.Text, GroupFromSelectedItem(ComboBox10.SelectedIndex))
    End Sub

    Private Sub ComboBox11_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox11.SelectedIndexChanged
        ModifyUserDetails(3, CheckBox10.Checked, TextBox14.Text, If(CheckBox26.Checked, TextBox22.Text, ""), TextBox15.Text, GroupFromSelectedItem(ComboBox11.SelectedIndex))
    End Sub

    Private Sub ComboBox12_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox12.SelectedIndexChanged
        ModifyUserDetails(4, CheckBox11.Checked, TextBox17.Text, If(CheckBox27.Checked, TextBox23.Text, ""), TextBox18.Text, GroupFromSelectedItem(ComboBox12.SelectedIndex))
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

    Private Sub CheckBox18_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox18.CheckedChanged
        MicrosoftAccountInteractive = CheckBox18.Checked
        AccountsPanel.Enabled = Not CheckBox18.Checked
        GroupBox1.Enabled = Not CheckBox18.Checked
        CheckBox7.Enabled = Not CheckBox18.Checked
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
        SelectedLockoutSettings.Enabled = CheckBox13.Checked
        EnabledAccountLockoutPanel.Enabled = Not CheckBox13.Checked
    End Sub

    Private Sub RadioButton21_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton21.CheckedChanged
        SelectedLockoutSettings.DefaultPolicy = RadioButton21.Checked
        AccountLockoutParametersPanel.Enabled = Not RadioButton21.Checked
    End Sub

    Private Sub NumericUpDown6_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown6.ValueChanged
        SelectedLockoutSettings.TimedLockoutSettings.FailedAttempts = NumericUpDown6.Value
    End Sub

    Private Sub NumericUpDown7_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown7.ValueChanged
        SelectedLockoutSettings.TimedLockoutSettings.Timeframe = NumericUpDown7.Value
    End Sub

    Private Sub NumericUpDown8_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown8.ValueChanged
        SelectedLockoutSettings.TimedLockoutSettings.AutoUnlockTime = NumericUpDown8.Value
        NumericUpDown7.Maximum = NumericUpDown8.Value
    End Sub

    Private Sub RadioButton23_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton23.CheckedChanged
        VirtualMachineSupported = RadioButton23.Checked
        VMProviderPanel.Enabled = RadioButton23.Checked
    End Sub

    Private Sub ComboBox8_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox8.SelectedIndexChanged
        SelectedVMSettings.Provider = ComboBox8.SelectedIndex
    End Sub

    Private Sub CheckBox14_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox14.CheckedChanged
        NetworkConfigInteractive = CheckBox14.Checked
        ManualNetworkConfigPanel.Enabled = Not CheckBox14.Checked
    End Sub

    Private Sub RadioButton25_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton25.CheckedChanged
        WirelessNetworkSettingsPanel.Enabled = RadioButton25.Checked
        NetworkConfigManualSkip = Not RadioButton25.Checked
    End Sub

    Private Sub TextBox7_TextChanged(sender As Object, e As EventArgs) Handles TextBox7.TextChanged
        SelectedNetworkConfiguration.SSID = TextBox7.Text
    End Sub

    Private Sub CheckBox15_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox15.CheckedChanged
        SelectedNetworkConfiguration.ConnectWithoutBroadcast = CheckBox15.Checked
    End Sub

    Private Sub ComboBox13_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox13.SelectedIndexChanged
        SelectedNetworkConfiguration.Authentication = ComboBox13.SelectedIndex
        ' Disable password on open connections
        TextBox10.Enabled = (ComboBox13.SelectedIndex <> 0)
    End Sub

    Private Sub TextBox10_TextChanged(sender As Object, e As EventArgs) Handles TextBox10.TextChanged
        SelectedNetworkConfiguration.Password = TextBox10.Text
    End Sub

    Private Sub CheckBox16_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox16.CheckedChanged
        SystemTelemetryInteractive = CheckBox16.Checked
        TelemetryOptionsPanel.Enabled = Not CheckBox16.Checked
    End Sub

    Private Sub RadioButton26_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton26.CheckedChanged
        SelectedTelemetrySettings.Enabled = Not RadioButton26.Checked
    End Sub

    Private Sub CheckBox17_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox17.CheckedChanged
        TextBox13.WordWrap = CheckBox17.Checked
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://schneegans.de/windows/unattend-generator/")
    End Sub

    Function EditionIDFromDisplayName(displayName As String) As String
        DynaLog.LogMessage("Grabbing target Edition ID from specified display name...")
        DynaLog.LogMessage("Display name of edition: " & displayName)
        Select Case displayName
            Case "Home"
                Return "home"
            Case "Home N"
                Return "home_n"
            Case "Home Single Language"
                Return "home_single"
            Case "Education"
                Return "education"
            Case "Education N"
                Return "education_n"
            Case "Pro"
                Return "pro"
            Case "Pro N"
                Return "pro_n"
            Case "Pro Education"
                Return "pro_education"
            Case "Pro Education N"
                Return "pro_education_n"
            Case "Pro for Workstations"
                Return "pro_workstations"
            Case "Pro N for Workstations"
                Return "pro_workstations_n"
            Case "Enterprise"
                Return "enterprise"
            Case "Enterprise N"
                Return "enterprise_n"
        End Select
        Return ""
    End Function

    Private Sub UnattendGeneratorBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles UnattendGeneratorBW.DoWork
        DynaLog.LogMessage("Preparing file generation...")
        ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Preparing.Generate.Label"), 0)
        DynaLog.LogMessage("Checking save target...")
        DynaLog.LogMessage("Save target: " & Quote & SaveTarget & Quote)
        If SaveTarget = "" Then
            DynaLog.LogMessage("No save target has been specified. Cancelling...")
            e.Cancel = True
            Exit Sub
        End If
        ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Preparing.Generate.Label"), 0)
        Dim UnattendGen As New Process()
        ' Get most appropriate binary of UnattendGen
        DynaLog.LogMessage("Getting the most appropriate UnattendGen executable...")
        If Environment.Is64BitOperatingSystem Then
            DynaLog.LogMessage("This operating system is a 64-bit OS")
            If PreferSelfContained Then
                DynaLog.LogMessage("The self-contained package will be used.")
                UnattendGen.StartInfo.FileName = Path.Combine(Application.StartupPath, "Tools\UnattendGen\SelfContained\amd64\unattendgen.exe")
                UnattendGen.StartInfo.WorkingDirectory = Path.Combine(Application.StartupPath, "Tools\UnattendGen\SelfContained\amd64")
            Else
                DynaLog.LogMessage("The self-contained package will not be used.")
                UnattendGen.StartInfo.FileName = Path.Combine(Application.StartupPath, "Tools\UnattendGen\win-x64\unattendgen.exe")
                UnattendGen.StartInfo.WorkingDirectory = Path.Combine(Application.StartupPath, "Tools\UnattendGen\win-x64")
            End If
        Else
            DynaLog.LogMessage("This operating system is a 32-bit OS")
            If PreferSelfContained Then
                DynaLog.LogMessage("The self-contained package will be used.")
                UnattendGen.StartInfo.FileName = Path.Combine(Application.StartupPath, "Tools\UnattendGen\SelfContained\x86\unattendgen.exe")
                UnattendGen.StartInfo.WorkingDirectory = Path.Combine(Application.StartupPath, "Tools\UnattendGen\SelfContained\x86")
            Else
                DynaLog.LogMessage("The self-contained package will not be used.")
                UnattendGen.StartInfo.FileName = Path.Combine(Application.StartupPath, "Tools\UnattendGen\win-x86\unattendgen.exe")
                UnattendGen.StartInfo.WorkingDirectory = Path.Combine(Application.StartupPath, "Tools\UnattendGen\win-x86")
            End If
        End If

        If Not File.Exists(UnattendGen.StartInfo.FileName) Then
            DynaLog.LogMessage("UnattendGen binary does not exist. Cancelling...")
            e.Cancel = True
            Exit Sub
        End If

        UnattendGen.StartInfo.Arguments = "--target=" & Quote & SaveTarget & Quote
        If Debugger.IsAttached Then
            DynaLog.LogMessage("A debugger has been attached. Telling UnattendGen to show debug output...")
            UnattendGen.StartInfo.Arguments &= " --debug"
        End If
        Try
            ' Save settings to appropriate XML files
            ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 2)
            DynaLog.LogMessage("Saving regional settings...")
            Dim regSetContents As String = "<?xml version=" & Quote & "1.0" & Quote & " ?>" & CrLf &
                "<root>" & CrLf &
                "   <ImageLanguage Id=" & Quote & SelectedLanguage.Id & Quote & " DisplayName=" & Quote & SelectedLanguage.DisplayName & Quote & "/>" & CrLf &
                "   <UserLocale Id=" & Quote & SelectedLocale.Id & Quote & " DisplayName=" & Quote & SelectedLocale.DisplayName & Quote & " LCID=" & Quote & SelectedLocale.LCID & Quote & " KeyboardLayout=" & Quote & SelectedLocale.KeybId & Quote & " GeoLocation=" & Quote & SelectedLocale.GeoLoc & Quote & "/>" & CrLf &
                "   <KeyboardIdentifier Id=" & Quote & SelectedKeybIdentifier.Id & Quote & " DisplayName=" & Quote & SelectedKeybIdentifier.DisplayName & Quote & " Type=" & Quote & SelectedKeybIdentifier.Type & Quote & "/>" & CrLf &
                "   <GeoId Id=" & Quote & SelectedGeoId.Id & Quote & " DisplayName=" & Quote & SelectedGeoId.DisplayName & Quote & "/>" & CrLf &
                "   <TimeOffset Id=" & Quote & SelectedOffset.Id & Quote & " DisplayName=" & Quote & If(SelectedOffset.DisplayName.Contains("&"), SelectedOffset.DisplayName.Replace("&", "&amp;").Trim(), SelectedOffset.DisplayName) & Quote & "/>" & CrLf &
                "</root>"
            File.WriteAllText(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "region.xml"), regSetContents, UTF8)
            UnattendGen.StartInfo.Arguments &= " --regionfile=" & Quote & Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "region.xml") & Quote
            ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 4)
            DynaLog.LogMessage("Saving architecture settings...")
            ' Build architecture string for UnattendGen
            Dim Architectures As New List(Of String)
            Dim ArchitectureString As String = ""
            For Each Architecture In SelectedArchitectures.Keys
                If SelectedArchitectures(Architecture) Then
                    Architectures.Add(Utilities.Casters.CastDismArchitecture(Architecture).ToLower())
                End If
            Next
            ArchitectureString = String.Join(",", Architectures.ToArray())
            UnattendGen.StartInfo.Arguments &= " --architecture=" & ArchitectureString
            ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 6)
            DynaLog.LogMessage("Saving Windows 11 settings...")
            If Win11Config.LabConfig_BypassRequirements Then
                UnattendGen.StartInfo.Arguments &= " --LabConfig"
            End If
            If Win11Config.OOBE_BypassNRO Then
                UnattendGen.StartInfo.Arguments &= " --BypassNRO"
            End If
            ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 8)
            DynaLog.LogMessage("Saving computer settings...")
            If RadioButton28.Checked Then
                If Not PCName.DefaultName Then
                    UnattendGen.StartInfo.Arguments &= " --computername=" & PCName.Name
                End If
            Else
                If Not String.IsNullOrEmpty(PCNameScript) Then
                    UnattendGen.StartInfo.Arguments &= " --computername=script:" & Quote & PCNameScript & Quote
                End If
            End If
            DynaLog.LogMessage("Saving configuration set/distribution share settings...")
            If UseConfigSet Then
                UnattendGen.StartInfo.Arguments &= " --ConfigSet"
            End If
            ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 10)
            DynaLog.LogMessage("Saving time zone settings...")
            If TimeOffsetInteractive Then
                UnattendGen.StartInfo.Arguments &= " --tzImplicit"
            End If
            ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 12)
            DynaLog.LogMessage("Saving disk configuration...")
            If DiskConfigurationInteractive Then
                DynaLog.LogMessage("Disks will be configured interactively.")
                UnattendGen.StartInfo.Arguments &= " --partmode=interactive"
            Else
                DynaLog.LogMessage("Disks will be configured in an unattended manner.")
                DynaLog.LogMessage("Disk 0 will be configured automatically.")
                UnattendGen.StartInfo.Arguments &= " --partmode=unattended"
                Dim diskZeroContents As String = "<?xml version=" & Quote & "1.0" & Quote & " ?>" & CrLf &
                    "<root>" & CrLf &
                    "   <DiskZero PartitionStyle=" & Quote & If(SelectedDiskConfiguration.PartStyle = PartitionStyle.GPT, "GPT", "MBR") & Quote & " RecoveryEnvironment=" & Quote & If(SelectedDiskConfiguration.InstallRecEnv, If(SelectedDiskConfiguration.RecEnvPartition = RecoveryEnvironmentLocation.WinREPartition, "WinRE", "Windows"), "No") & Quote & " ESPSize=" & Quote & SelectedDiskConfiguration.ESPSize & Quote & " RESize=" & Quote & SelectedDiskConfiguration.RecEnvSize & Quote & " />" & CrLf &
                    "</root>"
                File.WriteAllText(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "unattPartSettings.xml"), diskZeroContents, UTF8)
            End If
            ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 14)
            DynaLog.LogMessage("Saving edition settings...")
            If FirmwareChosen Then
                DynaLog.LogMessage("The product key will be grabbed from firmware.")
                UnattendGen.StartInfo.Arguments &= " --firmware"
            ElseIf GenericChosen Then
                DynaLog.LogMessage("A generic product key has been chosen.")
                UnattendGen.StartInfo.Arguments &= " --generic"
                Dim genericEditionContents As String = "<?xml version=" & Quote & "1.0" & Quote & " ?>" & CrLf &
                    "<root>" & CrLf &
                    "   <Edition Id=" & Quote & EditionIDFromDisplayName(ComboBox6.SelectedItem) & Quote & " DisplayName=" & Quote & ComboBox6.SelectedItem & Quote & " Key=" & Quote & SelectedKey.Key & Quote & " />" & CrLf &
                    "</root>"
                File.WriteAllText(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "edition.xml"), genericEditionContents, UTF8)
            Else
                DynaLog.LogMessage("A custom product key has been chosen.")
                UnattendGen.StartInfo.Arguments &= " --customkey=" & SelectedKey.Key
            End If
            If Not UserAccountsInteractive And Not MicrosoftAccountInteractive Then
                ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 16)
                DynaLog.LogMessage("Saving user accounts...")
                UnattendGen.StartInfo.Arguments &= " --customusers"
                Dim customUserContents As String = "<?xml version=" & Quote & "1.0" & Quote & " ?>" & CrLf &
                    "<root>" & CrLf
                If UserAccountsList.Count > 0 Then
                    For Each account As User In UserAccountsList
                        DynaLog.LogMessage("Saving information of account " & Quote & account.Name & Quote & " to file...")
                        customUserContents &= "   <UserAccount Enabled=" & Quote & If(account.Enabled, "1", "0") & Quote & " Name=" & Quote & If(account.Name.Contains("&"), account.Name.Replace("&", "&amp;").Trim(), account.Name) & Quote & " DisplayName=" & Quote & account.DisplayName.Replace("&", "&amp;") & Quote & " Password=" & Quote & If(account.Password.Contains("&"), account.Password.Replace("&", "&amp;").Trim(), account.Password) & Quote & " Group=" & Quote & If(account.Group = UserGroup.Administrators, "Admins", "Users") & Quote & " />" & CrLf
                    Next
                    customUserContents &= "</root>"
                    File.WriteAllText(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "userAccounts.xml"), customUserContents, UTF8)
                    If AutoLogon.EnableAutoLogon Then
                        DynaLog.LogMessage("Automatic logon will be used. Saving auto-logon settings.")
                        If AutoLogon.LogonMode = AutoLogonMode.FirstAdmin Then
                            UnattendGen.StartInfo.Arguments &= " --autologon=firstadmin"
                        ElseIf AutoLogon.LogonMode = AutoLogonMode.WindowsAdmin Then
                            UnattendGen.StartInfo.Arguments &= " --autologon=builtinadmin"
                            Dim builtinAdminContents As String = "<?xml version=" & Quote & "1.0" & Quote & " ?>" & CrLf &
                                "<root>" & CrLf &
                                "   <BuiltInAdmin Password=" & Quote & If(AutoLogon.LogonPassword.Contains("&"), AutoLogon.LogonPassword.Replace("&", "&amp;").Trim(), AutoLogon.LogonPassword) & Quote & " />" & CrLf &
                                "</root>"
                            File.WriteAllText(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "autoLogon.xml"), builtinAdminContents, UTF8)
                        End If
                    End If
                    If PasswordObfuscate Then
                        DynaLog.LogMessage("Passwords will be encoded with Base64.")
                        UnattendGen.StartInfo.Arguments &= " --b64obscure"
                    End If
                Else
                    UnattendGen.StartInfo.Arguments = UnattendGen.StartInfo.Arguments.Replace(" /customusers", "").Trim()
                End If
            ElseIf (Not UserAccountsInteractive) And MicrosoftAccountInteractive Then
                DynaLog.LogMessage("A Microsoft account is expected to be used in the target installation.")
                ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 16)
                UnattendGen.StartInfo.Arguments &= " --msa"
            End If
            If SelectedExpirationSettings.Mode = PasswordExpirationMode.NIST_Limited Then
                ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 18)
                DynaLog.LogMessage("Saving password expiration settings...")
                UnattendGen.StartInfo.Arguments &= " --pwExpire=" & If(SelectedExpirationSettings.WindowsDefault, 42, SelectedExpirationSettings.Days)
            End If
            ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 20)
            DynaLog.LogMessage("Saving Account Lockout settings...")
            If SelectedLockoutSettings.Enabled Then
                UnattendGen.StartInfo.Arguments &= " --lockout=yes"
                Dim lockoutContents As String = ""
                If SelectedLockoutSettings.DefaultPolicy Then
                    lockoutContents = "<?xml version=" & Quote & "1.0" & Quote & " ?>" & CrLf &
                        "<root>" & CrLf &
                        "   <AccountLockout FailedAttempts=" & Quote & 10 & Quote & " Timeframe=" & Quote & 10 & Quote & " AutoUnlock=" & Quote & 10 & Quote & " />" & CrLf &
                        "</root>"
                Else
                    lockoutContents = "<?xml version=" & Quote & "1.0" & Quote & " ?>" & CrLf &
                        "<root>" & CrLf &
                        "   <AccountLockout FailedAttempts=" & Quote & SelectedLockoutSettings.TimedLockoutSettings.FailedAttempts & Quote & " Timeframe=" & Quote & SelectedLockoutSettings.TimedLockoutSettings.Timeframe & Quote & " AutoUnlock=" & Quote & SelectedLockoutSettings.TimedLockoutSettings.AutoUnlockTime & Quote & " />" & CrLf &
                        "</root>"
                End If
                File.WriteAllText(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "lockout.xml"), lockoutContents, UTF8)
            Else
                UnattendGen.StartInfo.Arguments &= " --lockout=no"
            End If
            If VirtualMachineSupported Then
                ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 22)
                DynaLog.LogMessage("Saving VM provider settings...")
                Select Case SelectedVMSettings.Provider
                    Case VMProvider.VirtualBox_GAs
                        UnattendGen.StartInfo.Arguments &= " --vm=vbox_gas"
                    Case VMProvider.VMware_Tools
                        UnattendGen.StartInfo.Arguments &= " --vm=vmware"
                    Case VMProvider.VirtIO_Guest_Tools
                        UnattendGen.StartInfo.Arguments &= " --vm=virtio"
                    Case VMProvider.Parallels
                        UnattendGen.StartInfo.Arguments &= " --vm=parallels"
                End Select
            End If
            If Not NetworkConfigInteractive Then
                ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 24)
                DynaLog.LogMessage("Saving wireless settings...")
                If NetworkConfigManualSkip Then
                    UnattendGen.StartInfo.Arguments &= " --wifi=no"
                Else
                    UnattendGen.StartInfo.Arguments &= " --wifi=yes"
                    Dim wirelessContents As String = "<?xml version=" & Quote & "1.0" & Quote & " ?>" & CrLf &
                        "<root>" & CrLf &
                        "   <WirelessNetwork Name=" & Quote & If(SelectedNetworkConfiguration.SSID.Contains("&"), SelectedNetworkConfiguration.SSID.Replace("&", "&amp;").Trim(), SelectedNetworkConfiguration.SSID) & Quote & " Password=" & Quote & If(SelectedNetworkConfiguration.Password.Contains("&"), SelectedNetworkConfiguration.Password.Replace("&", "&amp;").Trim(), SelectedNetworkConfiguration.Password) & Quote & " AuthMode=" & Quote & If(SelectedNetworkConfiguration.Authentication = WiFiAuthenticationMode.Open, "Open", If(SelectedNetworkConfiguration.Authentication = WiFiAuthenticationMode.WPA2_PSK, "WPA2", "WPA3")) & Quote & " NonBroadcast=" & Quote & If(SelectedNetworkConfiguration.ConnectWithoutBroadcast, "1", "0") & Quote & " />" & CrLf &
                        "</root>"
                    File.WriteAllText(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "wireless.xml"), wirelessContents, UTF8)
                End If
            End If
            If Not SystemTelemetryInteractive Then
                ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 24.5)
                DynaLog.LogMessage("Saving system telemetry settings...")
                If SelectedTelemetrySettings.Enabled Then
                    UnattendGen.StartInfo.Arguments &= " --telem=yes"
                Else
                    UnattendGen.StartInfo.Arguments &= " --telem=no"
                End If
            End If
            ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 24.625)
            DynaLog.LogMessage("Checking if scripts directory exists...")
            If Not Directory.Exists(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "Scripts")) Then
                DynaLog.LogMessage("Scripts directory does not exist. Attempting to create it...")
                Directory.CreateDirectory(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "Scripts"))
            End If
            DynaLog.LogMessage("Saving and referencing scripts...")
            Dim postInstallScriptContents As String = "<?xml version=" & Quote & "1.0" & Quote & " ?>" & CrLf &
                "<root>" & CrLf
            For Each Stage In ConfiguredScripts.Keys
                Dim xmlPart As String = ""
                Dim StageString As String = ""
                Select Case Stage
                    Case PostInstallScript.Stage.Specialize
                        StageString = "System"
                    Case PostInstallScript.Stage.FirstRun
                        StageString = "FirstLogon"
                    Case PostInstallScript.Stage.UserFirstLogon
                        StageString = "FirstTimeUserLogon"
                End Select
                If ConfiguredScripts(Stage).Count > 0 Then
                    DynaLog.LogMessage("Saving scripts...")
                    Directory.CreateDirectory(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "Scripts", StageString))
                    Dim scriptCountFlag As Integer = 1
                    ' The name of the destination script will be "Scriptnnnn.ext"
                    For Each Script As PostInstallScript In ConfiguredScripts(Stage)
                        Dim scriptExt As String = ""
                        Select Case Script.ScriptExtension
                            Case PostInstallScript.Extension.PowerShell
                                scriptExt = "ps1"
                            Case PostInstallScript.Extension.Batch
                                scriptExt = "bat"
                            Case PostInstallScript.Extension.VBScript
                                scriptExt = "vbs"
                            Case PostInstallScript.Extension.JScript
                                scriptExt = "js"
                        End Select
                        Dim scriptFile As String = String.Format("Script{0}.{1}", scriptCountFlag.ToString().PadLeft(4, "0"), scriptExt)
                        File.WriteAllText(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "Scripts", StageString, scriptFile),
                                          Script.ScriptContents, UTF8)
                        xmlPart &= String.Format("    <PostInstallScript ScriptContent=" & Quote & "file:.\Scripts\{0}\{1}" & Quote & " Stage=" & Quote & "{0}" & Quote & " />", StageString, scriptFile) & CrLf
                        scriptCountFlag += 1
                    Next
                    postInstallScriptContents &= xmlPart
                End If
            Next
            postInstallScriptContents &= CrLf & "</root>"
            UnattendGen.StartInfo.Arguments &= " --customscripts"
            File.WriteAllText(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "scripts.xml"), postInstallScriptContents, UTF8)
            DynaLog.LogMessage("Checking if Windows Explorer will be restarted after running scripts...")
            If ScriptsRestartExplorer Then
                DynaLog.LogMessage("Explorer will be restarted.")
                UnattendGen.StartInfo.Arguments &= " --restartexplorer"
            End If
            DynaLog.LogMessage("Checking if script windows will be hidden...")
            If ScriptsHideWindow Then
                DynaLog.LogMessage("Windows will be hidden.")
                UnattendGen.StartInfo.Arguments &= " --hidewindows"
            End If
            If SystemComponentsEx.Count > 0 Then
                ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Saving.User.Settings.Label"), 24.75)
                DynaLog.LogMessage("Checking if components directory exists...")
                If Not Directory.Exists(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "Components")) Then
                    DynaLog.LogMessage("Components directory does not exist. Attempting to create it...")
                    Directory.CreateDirectory(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "Components"))
                End If
                DynaLog.LogMessage("Saving custom components...")
                UnattendGen.StartInfo.Arguments &= " --customcomponents"
                For Each SystemComponent As Component In SystemComponentsEx
                    File.WriteAllText(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "Components", String.Format("{0}_{1}.xml", SystemComponent.Id, SystemComponent.Pass.Name)),
                                      SystemComponent.XmlData, UTF8)
                Next
                DynaLog.LogMessage("Components were saved.")
            End If
            ReportMessage(LocalizationService.ForSection("Unattend.Progress")("GenerateAnswerFile.Label"), 25)
            DynaLog.LogMessage("Starting UnattendGen...")
            If Debugger.IsAttached Then UnattendGen.StartInfo.Arguments &= " --debug"
            UnattendGen.Start()
            UnattendGen.WaitForExit()
            DynaLog.LogMessage("UnattendGen finished with exit code " & Hex(UnattendGen.ExitCode))
            ReportMessage(LocalizationService.ForSection("Unattend.Progress")("GenerateAnswerFile.Label"), 50)
            ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Deleting.Temporary.Label"), 75)
            If File.Exists(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "diskpart.dp")) Then
                DynaLog.LogMessage("Deleting temporary DiskPart scripts...")
                File.Delete(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "diskpart.dp"))
            End If
            DynaLog.LogMessage("Deleting temporary XML files...")
            For Each xmlFile In My.Computer.FileSystem.GetFiles(UnattendGen.StartInfo.WorkingDirectory, FileIO.SearchOption.SearchTopLevelOnly, "*.xml")
                If File.Exists(xmlFile) Then File.Delete(xmlFile)
            Next
            DynaLog.LogMessage("Deleting temporary scripts...")
            Directory.Delete(Path.Combine(UnattendGen.StartInfo.WorkingDirectory, "Scripts"), True)
            If UnattendGen.ExitCode <> 0 Then
                MessageBox.Show(LocalizationService.ForSection("Unattend.Messages").Format("GeneratorExit.Message", Hex(UnattendGen.ExitCode)))
                e.Cancel = True
            End If
            ReportMessage(LocalizationService.ForSection("Unattend.Progress")("Generation.Completed.Label"), 100)
        Catch ex As Exception
            DynaLog.LogMessage("Could not generate the answer file. Error message: " & ex.Message)
            If UnattendGen.ExitCode <> 0 Then
                MessageBox.Show(LocalizationService.ForSection("Unattend.Messages").Format("Generator.Message", ex.Message))
                e.Cancel = True
            End If
        End Try
    End Sub

    Private Sub SaveFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles SaveFileDialog1.FileOk
        SaveTarget = SaveFileDialog1.FileName
    End Sub

    Sub ReportMessage(msg As String, percent As Integer)
        ProgressMessage = msg
        UnattendGeneratorBW.ReportProgress(percent)
    End Sub

    Private Sub UnattendGeneratorBW_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles UnattendGeneratorBW.ProgressChanged
        Label56.Text = ProgressMessage
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub UnattendGeneratorBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles UnattendGeneratorBW.RunWorkerCompleted
        If e.Cancelled Then
            ChangePage(CurrentWizardPage.WizardPage - 1)
            Exit Sub
        End If
        ChangePage(CurrentWizardPage.WizardPage + 1)
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        If MsgBox(LocalizationService.ForSection("Unattend.Messages")("Reuse.Settings.Ve.Message"), vbQuestion + vbYesNo, Text) = MsgBoxResult.No Then
            ' Refresh the settings
            ReloadSettings()
        End If
        ChangePage(UnattendedWizardPage.Page.RegionalPage)
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\explorer.exe", "/select," & Quote & SaveTarget & Quote)
    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        If MainForm.isProjectLoaded And Not (MainForm.OnlineManagement Or MainForm.OfflineManagement) Then
            DynaLog.LogMessage("Proceeding to apply unattended answer file...")
            ApplyUnattendFile.TextBox1.Text = SaveTarget
            WindowState = FormWindowState.Minimized
            ApplyUnattendFile.ShowDialog(MainForm)
            WindowState = FormWindowState.Normal
        Else
            MsgBox(LocalizationService.ForSection("Unattend.Messages")("Load.Project.Order.Label"), vbOKOnly + vbExclamation, Text)
            Exit Sub
        End If
    End Sub

    Private Sub NewUnattendWiz_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If UnattendGeneratorBW.IsBusy OrElse UnattendGenBW.IsBusy Then
            e.Cancel = True
            Beep()
            Exit Sub
        End If
    End Sub

    Private Sub StepsTreeView_DrawNode(sender As Object, e As DrawTreeNodeEventArgs) Handles StepsTreeView.DrawNode
        ' Determine the custom background color
        Dim customBackColor As Color = CurrentTheme.SectionBackgroundColor

        ' Determine the custom foreground color based on the custom background color
        Dim customForeColor As Color = CurrentTheme.ForegroundColor

        ' Check if the node is selected
        If (e.State And TreeNodeStates.Selected) <> 0 Then
            ' Draw the background with the highlight color
            e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds)
            ' Draw the text with the highlighted text color and vertically centered
            TextRenderer.DrawText(e.Graphics, e.Node.Text, If(e.Node.NodeFont, e.Node.TreeView.Font), e.Bounds, SystemColors.HighlightText, TextFormatFlags.VerticalCenter)
        Else
            ' Draw the background with the custom color for unselected nodes
            Using backgroundBrush As New SolidBrush(customBackColor)
                e.Graphics.FillRectangle(backgroundBrush, e.Bounds)
            End Using
            ' Draw the text with the custom foreground color and vertically centered
            TextRenderer.DrawText(e.Graphics, e.Node.Text, If(e.Node.NodeFont, e.Node.TreeView.Font), e.Bounds, customForeColor, TextFormatFlags.VerticalCenter)
        End If

        ' If the node has focus, draw the focus rectangle
        If (e.State And TreeNodeStates.Focused) <> 0 Then
            Using focusPen As New Pen(Color.Black)
                focusPen.DashStyle = Drawing2D.DashStyle.Dot
                Dim focusBounds As Rectangle = e.Bounds
                focusBounds.Size = New Size(focusBounds.Width - 1, focusBounds.Height - 1)
                e.Graphics.DrawRectangle(focusPen, focusBounds)
            End Using
        End If

        ' Signal that the node has been drawn
        e.DrawDefault = False
    End Sub

    Private Sub UnattendGenBW_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles UnattendGenBW.DoWork
        Try
            ' Download UnattendGen and run it
            If Not Directory.Exists(Application.StartupPath & "\Tools\UnattendGen\SelfContained") Then
                DynaLog.LogMessage("Creating self-contained package directory...")
                Directory.CreateDirectory(Application.StartupPath & "\Tools\UnattendGen\SelfContained")
            End If
            Using UnattClient As New WebClient()
                DynaLog.LogMessage("Downloading UnattendGen installer from the UnattendGen repository...")
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                Dim contents As String = ""
                Try
                    contents = UnattClient.DownloadString("https://raw.githubusercontent.com/CodingWonders/UnattendGen/master/DISMTools-Install.ps1")
                Catch ex As WebException
                    Throw ex
                End Try
                If contents <> "" Then
                    DynaLog.LogMessage("Writing contents to file...")
                    File.WriteAllText(Application.StartupPath & "\setup.ps1", contents, UTF8)
                End If
            End Using
            If File.Exists(Application.StartupPath & "\setup.ps1") Then
                DynaLog.LogMessage("Installing self-contained UnattendGen...")
                ' Run installer
                Dim UAProc As New Process()
                UAProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe"
                UAProc.StartInfo.WorkingDirectory = Application.StartupPath
                UAProc.StartInfo.Arguments = "-executionpolicy unrestricted -file " & Quote & Application.StartupPath & "\setup.ps1" & Quote & " -tag " & Quote & "DT_" & UnattendGenReleaseTag & Quote
                UAProc.Start()
                UAProc.WaitForExit()
                DynaLog.LogMessage("UnattendGen installer finished with exit code " & Hex(UAProc.ExitCode))
                If UAProc.ExitCode <> 0 Then
                    Throw New System.ComponentModel.Win32Exception(UAProc.ExitCode)
                End If
            End If
            If File.Exists(Application.StartupPath & "\setup.ps1") Then
                Try
                    DynaLog.LogMessage("Attempting to delete temporary installer...")
                    File.Delete(Application.StartupPath & "\setup.ps1")
                Catch ex As Exception
                    ' Don't delete it
                End Try
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not download and install self-contained UnattendGen. Error message: " & ex.Message)
            Throw ex
        End Try
    End Sub

    Private Sub UnattendGenBW_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles UnattendGenBW.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            MessageBox.Show(LocalizationService.ForSection("Unattend.Messages").Format("PrepareFailed.Label", e.Error.Message), LocalizationService.ForSection("NewUnattend.Validation")("Gen.Error.Title"), MessageBoxButtons.OK, MessageBoxIcon.Error)
            If Directory.Exists(Path.Combine(Application.StartupPath, "Tools\UnattendGen\SelfContained")) Then
                Try
                    Directory.Delete(Path.Combine(Application.StartupPath, "Tools\UnattendGen\SelfContained"), True)
                Catch ex As Exception
                    ' Leave dir
                End Try
            End If
            Close()
            Exit Sub
        End If
        ExpressPanelFooter.Enabled = True
        PreferSelfContained = True
        UGNotify.ShowBalloonTip(5000)
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        Scintilla1.Text = DefaultContents
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        EditorModeOFD.ShowDialog(Me)
    End Sub

    Private Sub EditorModeOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles EditorModeOFD.FileOk
        Try
            DynaLog.LogMessage("Loading contents of file in editor...")
            DynaLog.LogMessage("File to load: " & Quote & EditorModeOFD.FileName & Quote)
            Scintilla1.Text = File.ReadAllText(EditorModeOFD.FileName)
        Catch ex As Exception
            DynaLog.LogMessage("Could not load file. Error message: " & ex.Message)
            MsgBox(LocalizationService.ForSection("Unattend.Messages").Format("OpenFile.Label", ex.Message), vbOKOnly + vbCritical, Text)
        End Try
    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        EditorModeSFD.ShowDialog(Me)
    End Sub

    Private Sub EditorModeSFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles EditorModeSFD.FileOk
        Try
            DynaLog.LogMessage("Saving contents of editor to file...")
            DynaLog.LogMessage("Destination: " & Quote & EditorModeSFD.FileName & Quote)
            File.WriteAllText(EditorModeSFD.FileName, Scintilla1.Text, UTF8)
        Catch ex As Exception
            DynaLog.LogMessage("Could not save file. Error message: " & ex.Message)
            MsgBox(LocalizationService.ForSection("Unattend.Messages").Format("SaveFile.Label", ex.Message), vbOKOnly + vbCritical, Text)
        End Try
    End Sub

    Private Sub Help_Button_Click(sender As Object, e As EventArgs) Handles Help_Button.Click, ToolStripButton6.Click
        HelpDocsModule.DisplayHelpDocumentation("docs\img_tasks\unattend\unatt_create.html")
    End Sub

    Private Sub NewUnattendWiz_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        'AutoDiskConfigPanel.Width = ManualPartPanel.Width - (AutoDiskConfigPanel.Margin.Left * 2) - 4
        GroupBox1.Width = ManualAccountPanel.Width - (GroupBox1.Margin.Left * 2) - 4
        AccountsPanel.Width = UserAccountListing.Width
        UserAccountListing.Width = ManualAccountPanel.Width - (UserAccountListing.Margin.Left * 2) - 4
        WirelessNetworkSettingsPanel.Width = ManualNetworkConfigPanel.Width - (WirelessNetworkSettingsPanel.Margin.Left * 2) - 4
    End Sub

    Private Sub LinkLabel5_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked
        Process.Start("https://learn.microsoft.com/en-us/windows-hardware/customize/desktop/unattend/components-b-unattend")
    End Sub

    Private Sub LinkLabel6_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel6.LinkClicked
        If File.Exists(Path.Combine(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)),
                                    "Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\WSIM\x86\imgmgr.exe")) Then
            DynaLog.LogMessage("Starting Windows SIM...")
            Process.Start(Path.Combine(Environment.GetFolderPath(If(Environment.Is64BitOperatingSystem, Environment.SpecialFolder.ProgramFilesX86, Environment.SpecialFolder.ProgramFiles)), "Windows Kits\10\Assessment and Deployment Kit\Deployment Tools\WSIM\x86\imgmgr.exe"), Quote & SaveTarget & Quote)
        End If
    End Sub

    Private Sub LinkLabel7_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel7.LinkClicked
        Try
            DynaLog.LogMessage("Loading contents of file in editor...")
            DynaLog.LogMessage("File to load: " & Quote & SaveTarget & Quote)
            Scintilla1.Text = File.ReadAllText(SaveTarget)
        Catch ex As Exception
            DynaLog.LogMessage("Could not load file. Error message: " & ex.Message)
            MsgBox(LocalizationService.ForSection("Unattend.Messages").Format("OpenFile.Label", ex.Message), vbOKOnly + vbCritical, Text)
            Exit Sub
        End Try

        IsInExpress = False
        StepsTreeView.Enabled = False
        EditorPanelContainer.Visible = True
        ExpressPanelContainer.Visible = False
        ExpressPanelTrigger.BackColor = SidePanel.BackColor
        ExpressPanelTrigger.ForeColor = If(CurrentTheme.IsDark, Color.LightGray, Color.Black)
        PictureBox1.Image = If(CurrentTheme.IsDark, My.Resources.express_mode_select, My.Resources.express_mode)
        EditorPanelTrigger.BackColor = Color.FromKnownColor(KnownColor.Highlight)
        EditorPanelTrigger.ForeColor = CurrentTheme.ForegroundColor
        PictureBox2.Image = My.Resources.editor_mode_select
        PictureBox3.Image = My.Resources.editor_mode_fc
        Label3.Text = LocalizationService.ForSection("Unattend.Mode")("EditorMode.Title")
        Label4.Text = LocalizationService.ForSection("Unattend.Mode")("CreateUnattended.Description")
        FooterContainer.Visible = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        DynaLog.LogMessage("Grabbing computer name...")
        TextBox1.Text = My.Computer.Name
    End Sub

    Private Sub Button3_MouseHover(sender As Object, e As EventArgs) Handles Button3.MouseHover
        CNameTTip.Show(LocalizationService.ForSection("Unattend.Tooltips")("Uses.Name.Computer.Message"), sender)
    End Sub

    Private Sub CheckBox19_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox19.CheckedChanged
        UseConfigSet = CheckBox19.Checked
    End Sub

    Sub SaveConfiguredScripts(Stage As Integer)
        DynaLog.LogMessage("Saving scripts...")
        DynaLog.LogMessage("- Stage Number: " & Stage)
        DynaLog.LogMessage("Determining status of stage number...")
        If Stage > ConfiguredScripts.Keys.Count - 1 Then
            DynaLog.LogMessage("A bogus stage integer has been passed. Exiting...")
            Exit Sub
        End If
        DynaLog.LogMessage("Stage Number is fine. Saving contents...")
        SaveConfiguredScript(CurrentlyEditedScript, Scintilla3.Text)
        ConfiguredScripts(Stage) = New List(Of PostInstallScript)(CurrentlyConfiguredScripts)
    End Sub

    Sub LoadConfiguredScripts(Stage As Integer)
        Try
            DynaLog.LogMessage("Loading script contents...")
            DynaLog.LogMessage("- Stage Number: " & Stage)
            DynaLog.LogMessage("Determining status of stage number...")
            If Stage > ConfiguredScripts.Keys.Count - 1 Then
                DynaLog.LogMessage("A bogus stage integer has been passed. Exiting...")
                Exit Sub
            End If
            DynaLog.LogMessage("Stage Number is fine. Loading contents...")
            CurrentlyConfiguredScripts = ConfiguredScripts(Stage)
        Catch ex As Exception
            ' For some reason, Scintilla causes an access violation
        End Try
    End Sub

    Sub SaveConfiguredScript(ScriptIndex As Integer, Contents As String)
        DynaLog.LogMessage("Saving script contents...")
        DynaLog.LogMessage("- Script Index: " & ScriptIndex)
        If Debugger.IsAttached Then DynaLog.LogMessage("- Script Contents to Save:" & CrLf & Contents)
        DynaLog.LogMessage("Determining status of stage number...")
        If ScriptIndex > CurrentlyConfiguredScripts.Count - 1 Then
            DynaLog.LogMessage("A bogus stage integer has been passed. Exiting...")
            Exit Sub
        End If
        DynaLog.LogMessage("Stage Number is fine. Saving contents...")
        ' We have to clone the item first. Otherwise, every item in our list will be updated,
        ' and we don't want this.
        Dim newScript As PostInstallScript = CurrentlyConfiguredScripts(ScriptIndex).Clone()
        newScript.ScriptContents = Contents
        CurrentlyConfiguredScripts(ScriptIndex) = newScript
    End Sub

    Sub SwitchStages(NewStage As Integer, Optional DontSave As Boolean = False)
        DynaLog.LogMessage("Switching stages...")
        DynaLog.LogMessage("- Current stage: " & CurrentlyEditedStage)
        DynaLog.LogMessage("- New Stage to change to: " & NewStage)
        If CurrentlyEditedStage = NewStage Then
            DynaLog.LogMessage("The same stage has been changed to")
            Exit Sub
        End If
        If Not DontSave Then
            DynaLog.LogMessage("Saving current scripts to list...")
            SaveConfiguredScripts(CurrentlyEditedStage)
        End If
        DynaLog.LogMessage("Loading scripts in new stage...")
        LoadConfiguredScripts(NewStage)
        DynaLog.LogMessage("Configuring stages...")
        CurrentlyEditedStage = NewStage
        If CurrentlyConfiguredScripts.Count > 0 Then
            SwitchScript(0)
        Else
            Button13.Enabled = False
            Button14.Enabled = False
            Button15.Enabled = False
            Button17.Enabled = False
            Button18.Enabled = False
            Button23.Enabled = False
        End If
        NoSpecifiedScriptsPanel.Visible = (CurrentlyConfiguredScripts.Count = 0)
        ScriptEditorPanel.Visible = (CurrentlyConfiguredScripts.Count > 0)
        Label66.Visible = (CurrentlyConfiguredScripts.Count > 0)
    End Sub

    Sub SwitchScript(NewIndex As Integer)
        DynaLog.LogMessage("Switching scripts...")
        DynaLog.LogMessage("- Current Script: " & CurrentlyEditedScript)
        DynaLog.LogMessage("- New Script to change to: " & NewIndex)
        Label66.Text = String.Format("Script {0} of {1}", NewIndex + 1, CurrentlyConfiguredScripts.Count)
        Scintilla3.Text = CurrentlyConfiguredScripts(NewIndex).ScriptContents
        CurrentlyEditedScript = NewIndex

        Button13.Enabled = True
        Button14.Enabled = Not (NewIndex = 0)
        Button15.Enabled = Not (NewIndex = 0)
        Button17.Enabled = Not (NewIndex = CurrentlyConfiguredScripts.Count - 1)
        Button18.Enabled = Not (NewIndex = CurrentlyConfiguredScripts.Count - 1)
        Button23.Enabled = True

        ComboBox16.SelectedItem = ComboBox16.Items(CurrentlyConfiguredScripts(NewIndex).ScriptExtension)
    End Sub

    Private Sub StageLink1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles StageLink1.LinkClicked
        SwitchStages(0)
        StageLink1.LinkBehavior = LinkBehavior.AlwaysUnderline
        StageLink2.LinkBehavior = LinkBehavior.HoverUnderline
        StageLink3.LinkBehavior = LinkBehavior.HoverUnderline
    End Sub

    Private Sub StageLink2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles StageLink2.LinkClicked
        SwitchStages(1)
        StageLink1.LinkBehavior = LinkBehavior.HoverUnderline
        StageLink2.LinkBehavior = LinkBehavior.AlwaysUnderline
        StageLink3.LinkBehavior = LinkBehavior.HoverUnderline
    End Sub

    Private Sub StageLink3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles StageLink3.LinkClicked
        SwitchStages(2)
        StageLink1.LinkBehavior = LinkBehavior.HoverUnderline
        StageLink2.LinkBehavior = LinkBehavior.HoverUnderline
        StageLink3.LinkBehavior = LinkBehavior.AlwaysUnderline
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ScriptEditorOFD.ShowDialog(Me)
    End Sub

    Private Sub ScriptEditorOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ScriptEditorOFD.FileOk
        DynaLog.LogMessage("Opening contents of script...")
        DynaLog.LogMessage("- Script to open: " & Quote & ScriptEditorOFD.FileName & Quote)
        DynaLog.LogMessage("Checking if file exists...")
        If File.Exists(ScriptEditorOFD.FileName) Then
            DynaLog.LogMessage("File exists. Attempting to read...")
            Try
                DynaLog.LogMessage("Checking file extension for special files...")
                If {".bat", ".cmd"}.Contains(Path.GetExtension(ScriptEditorOFD.FileName).ToLower()) Then
                    ' We'll set it to Batch
                    ComboBox16.SelectedIndex = 1
                End If
                Scintilla3.Text = File.ReadAllText(ScriptEditorOFD.FileName)
            Catch ex As Exception
                DynaLog.LogMessage("Could not load file. Error message: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub CheckBox20_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox20.CheckedChanged
        ScriptsRestartExplorer = CheckBox20.Checked
    End Sub

    Private Sub CheckedListBox1_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles CheckedListBox1.ItemCheck
        DynaLog.LogMessage("Changing state of selected architectures...")
        Dim changedIndex As Integer = e.Index
        Dim newValueIsChecked As Boolean = (e.NewValue = CheckState.Checked)
        DynaLog.LogMessage("Index that changed: " & changedIndex)
        DynaLog.LogMessage("Will the answer file target the architecture in the dictionary? " & newValueIsChecked)

        Select Case changedIndex
            Case 0
                SelectedArchitectures(DismProcessorArchitecture.Intel) = newValueIsChecked
            Case 1
                SelectedArchitectures(DismProcessorArchitecture.AMD64) = newValueIsChecked
            Case 2
                SelectedArchitectures(DismProcessorArchitecture.ARM64) = newValueIsChecked
        End Select

        ' Disable Windows 11 settings for x86 (if and only if x86 is selected)
        WinSVSettingsPanel.Enabled = Not (SelectedArchitectures(DismProcessorArchitecture.Intel) AndAlso
                                          Not SelectedArchitectures(DismProcessorArchitecture.AMD64) AndAlso
                                          Not SelectedArchitectures(DismProcessorArchitecture.ARM64))
    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Scintilla1.Text = Regex.Replace(Scintilla1.Text, Tab, "    ")
    End Sub

    Private Sub CheckBox21_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox21.CheckedChanged
        FirmwareChosen = CheckBox21.Checked
        ManualProductKeyOptionsPanel.Enabled = Not CheckBox21.Checked
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            DynaLog.LogMessage("Copying key to clipboard...")
            My.Computer.Clipboard.SetText(TextBox2.Text)
            DynaLog.LogMessage("Key copied successfully.")
            MsgBox(LocalizationService.ForSection("Unattend.Messages")("ProductKey.Copied.Done.Label"), vbOKOnly + vbInformation)
        Catch ex As Exception
            DynaLog.LogMessage("Could not copy key. Error message: " & ex.Message)
            MsgBox(LocalizationService.ForSection("Unattend.Messages").Format("Copy.Key.Clipboard.Label", ex.Message), vbOKOnly + vbInformation)
        End Try
    End Sub

    Private Sub RadioButton28_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton28.CheckedChanged
        ManualComputerNamePanel.Enabled = RadioButton28.Checked
        ScriptedComputerNamePanel.Enabled = Not RadioButton28.Checked
    End Sub

    Private Sub TextBox16_TextChanged(sender As Object, e As EventArgs) Handles TextBox16.TextChanged
        PCNameScript = TextBox16.Text
    End Sub

    Private Sub RadioButton29_MouseHover(sender As Object, e As EventArgs) Handles RadioButton29.MouseHover
        Dim ExampleName As String = ""
        Const Characters As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        Dim random = New Random()
        Dim sb = New StringBuilder(7)
        For i = 0 To 6
            sb.Append(Characters(random.Next(Characters.Length)))
        Next
        ExampleName = String.Format("DESKTOP-{0}", sb.ToString())
        CNameTTip.Show("Choose this option if the unattended answer file will be used on multiple computers on the same network." & CrLf &
                       "The default script will return a random computer name similar to " & Quote & ExampleName & Quote & CrLf &
                       "This can avoid name resolution conflicts.", sender)
    End Sub

    Private Sub CheckBox22_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox22.CheckedChanged
        ScriptsHideWindow = CheckBox22.Checked
    End Sub

    Private Sub LinkLabel8_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel8.LinkClicked
        Dim IpGateway As IPAddress = NetworkInterface.GetAllNetworkInterfaces().
            Where(Function(nic) nic.OperationalStatus = OperationalStatus.Up).
            Where(Function(nic) nic.NetworkInterfaceType = NetworkInterfaceType.Wireless80211).
            SelectMany(Function(nic) nic.GetIPProperties().GatewayAddresses).
            Select(Function(gateway) gateway.Address).
            Where(Function(address) address IsNot Nothing And Not address.IsIPv6LinkLocal).
            FirstOrDefault()
        If IpGateway IsNot Nothing Then
            Process.Start(String.Format("http://{0}", IpGateway.ToString()))
        End If
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If SystemComponentsEx.Count = 0 Then
            NoSpecifiedComponentsPanel.Visible = False
            ComponentEditorPanel.Visible = True
            Label60.Visible = True
            Button10.Enabled = True
            LinkLabel9.Visible = True
        End If
        SystemComponentsEx.Add(New Component(SystemComponents(0).Id, SystemComponents(0).Passes(0)))
        ComponentIndex = SystemComponentsEx.Count - 1
        LoadCustomComponent(ComponentIndex, True)
    End Sub

    Sub LoadCustomComponent(Index As Integer, Optional NewItem As Boolean = False)
        DynaLog.LogMessage("Loading custom component item...")
        DynaLog.LogMessage("Index: " & Index)
        Label60.Text = String.Format("Component {0} of {1}", Index + 1, SystemComponentsEx.Count)
        If SystemComponentsEx(Index) IsNot Nothing Then
            If Not NewItem Then IsComponentBeingLoaded = True
            ComboBox14.SelectedItem = SystemComponentsEx(Index).Id
            If NewItem Then
                ComboBox15.SelectedIndex = 0
            Else
                ComboBox15.SelectedIndex = ComboBox15.Items.IndexOf(SystemComponentsEx(Index).Pass.Name)
            End If
            IsComponentBeingLoaded = False

            Scintilla4.Text = SystemComponentsEx(Index).XmlData
            Button6.Enabled = Not (Index = SystemComponentsEx.Count - 1)
            Button7.Enabled = Not (Index = 0)
            Button8.Enabled = Not (Index = 0)
            Button9.Enabled = Not (Index = SystemComponentsEx.Count - 1)

            ShowReservedComponentStatusMessage(Index)
        End If
    End Sub

    Private Function IsAReservedComponent(cmpName As String, cmpPass As Pass) As Boolean
        Return ReservedComponents.Where(Function(component) component.Id = cmpName And component.Pass.Equals(cmpPass))(0) IsNot Nothing
    End Function

    Sub ShowReservedComponentStatusMessage(SelectedComponentIndex As Integer)
        If IsAReservedComponent(SystemComponentsEx(SelectedComponentIndex).Id, SystemComponentsEx(SelectedComponentIndex).Pass) Then
            MessageBox.Show(LocalizationService.ForSection("Unattend.Messages")("Component.Already.Message"), LocalizationService.ForSection("Unattend.Messages")("ComponentUse.Title"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub ComboBox14_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox14.SelectedIndexChanged
        ComboBox15.Items.Clear()
        For Each componentPass As Pass In SystemComponents(ComboBox14.SelectedIndex).Passes.Where(Function(pass) pass.Compatible)
            ComboBox15.Items.Add(componentPass.Name)
        Next
        SystemComponentsEx(ComponentIndex).Id = ComboBox14.SelectedItem
        If Not IsComponentBeingLoaded Then
            If ComboBox15.Items.Count > 0 Then
                ComboBox15.SelectedIndex = 0
            End If
        End If
        If Not IsComponentBeingLoaded Then ShowReservedComponentStatusMessage(ComponentIndex)
    End Sub

    Private Sub ComboBox15_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox15.SelectedIndexChanged
        SystemComponentsEx(ComponentIndex).Pass = SystemComponents(ComponentIndex).Passes.Where(Function(pass) pass.Name = ComboBox15.SelectedItem)(0)
        If Not IsComponentBeingLoaded Then ShowReservedComponentStatusMessage(ComponentIndex)
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        ComponentIndex = SystemComponentsEx.Count - 1
        LoadCustomComponent(ComponentIndex)
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        ComponentIndex += 1
        LoadCustomComponent(ComponentIndex)
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        ' Don't do it if there are no items
        If SystemComponentsEx.Count = 0 Then Exit Sub

        SystemComponentsEx.RemoveAt(ComponentIndex)
        ' Check again if there are no items
        If SystemComponentsEx.Count = 0 Then
            NoSpecifiedComponentsPanel.Visible = True
            ComponentEditorPanel.Visible = False
            Label60.Visible = False
            Button10.Enabled = False
            Button6.Enabled = False
            Button7.Enabled = False
            Button8.Enabled = False
            Button9.Enabled = False
            LinkLabel9.Visible = False
        Else
            If ComponentIndex > SystemComponentsEx.Count - 1 Then
                ComponentIndex = SystemComponentsEx.Count - 1
            End If
            LoadCustomComponent(ComponentIndex)
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        ComponentIndex -= 1
        LoadCustomComponent(ComponentIndex)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        ComponentIndex = 0
        LoadCustomComponent(ComponentIndex)
    End Sub

    Sub AddComponent(ComponentName As String, ComponentPass As String, ComponentData As String)
        DynaLog.LogMessage("Adding data to component...")
        DynaLog.LogMessage("- Component name: " & ComponentName)
        DynaLog.LogMessage("- Component pass: " & ComponentPass)
        If Not ComboBox14.Items.Contains(ComponentName) Then
            DynaLog.LogMessage("Component list does not contain component name. Leaving...")
            Exit Sub
        End If
        Dim component = SystemComponents.Where(Function(cmp) cmp.Id.Equals(ComponentName, StringComparison.InvariantCultureIgnoreCase))
        Dim pass = component(0).Passes.Where(Function(systemPass) systemPass.Name.Equals(ComponentPass))
        If pass Is Nothing Then
            DynaLog.LogMessage("Pass is not supported by this component. Leaving...")
            Exit Sub
        End If

        ' Clicking the Add button won't trigger the event when hidden
        If SystemComponentsEx.Count = 0 Then
            NoSpecifiedComponentsPanel.Visible = False
            ComponentEditorPanel.Visible = True
            Label60.Visible = True
            Button10.Enabled = True
            LinkLabel9.Visible = True
        End If
        SystemComponentsEx.Add(New Component(SystemComponents(0).Id, SystemComponents(0).Passes(0)))
        ComponentIndex = SystemComponentsEx.Count - 1
        LoadCustomComponent(ComponentIndex, True)

        ComboBox14.SelectedIndex = ComboBox14.Items.IndexOf(ComponentName)
        ComboBox15.SelectedIndex = ComboBox15.Items.IndexOf(ComponentPass)
        Scintilla4.Text = ComponentData
    End Sub

    Private Sub Scintilla4_TextChanged(sender As Object, e As EventArgs) Handles Scintilla4.TextChanged
        SystemComponentsEx(ComponentIndex).XmlData = Scintilla4.Text
    End Sub

    Private Sub LinkLabel9_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel9.LinkClicked
        If ComboBox14.SelectedItem IsNot Nothing Then
            SearchEngineHelper.InvokeSearchQuery(MainForm.SearchEngineName, String.Format("{0}+site:learn.microsoft.com", ComboBox14.SelectedItem))
        End If
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        ADDSJoinDialog.ShowDialog(Me)
    End Sub

    Private Sub LinkLabel10_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel10.LinkClicked
        DynaLog.LogMessage("Preparing to copy non-Windows UnattendGen...")
        If CPUnattendGenFBD.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("FBD accepted. Copying non-Windows UnattendGen...")
            For Each CrossPlatformZip In Directory.GetFiles(Path.Combine(Application.StartupPath, "Tools", "UnattendGen"), "*.zip", SearchOption.TopDirectoryOnly).
                Where(Function(zip) Path.GetFileNameWithoutExtension(zip).ToLower().Contains("linux") OrElse
                          Path.GetFileNameWithoutExtension(zip).ToLower().Contains("macos"))
                DynaLog.LogMessage(String.Format("Copying {0} to destination...", Path.GetFileName(CrossPlatformZip)))
                Try
                    File.Copy(CrossPlatformZip, Path.Combine(CPUnattendGenFBD.SelectedPath, Path.GetFileName(CrossPlatformZip)), True)
                Catch ex As Exception
                    DynaLog.LogMessage("Could not copy this file. Error message: " & ex.Message)
                End Try
            Next
            MsgBox(LocalizationService.ForSection("Unattend.Messages").Format("Cross.Platform.Label", CPUnattendGenFBD.SelectedPath), vbOKOnly + vbInformation)
        End If
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        If CurrentlyConfiguredScripts.Count = 0 Then
            NoSpecifiedScriptsPanel.Visible = False
            ScriptEditorPanel.Visible = True
            Label66.Visible = True
            Button13.Enabled = True
        Else
            SaveConfiguredScript(CurrentlyEditedScript, Scintilla3.Text)
        End If
        CurrentlyConfiguredScripts.Add(New PostInstallScript(My.Resources.DefaultPostInstallScriptCode, PostInstallScript.Extension.PowerShell))
        CurrentlyEditedScript = CurrentlyConfiguredScripts.Count - 1
        SwitchScript(CurrentlyEditedScript)
    End Sub

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        SaveConfiguredScript(CurrentlyEditedScript, Scintilla3.Text)
        SwitchScript(CurrentlyEditedScript + 1)
    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        SaveConfiguredScript(CurrentlyEditedScript, Scintilla3.Text)
        SwitchScript(CurrentlyConfiguredScripts.Count - 1)
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        SaveConfiguredScript(CurrentlyEditedScript, Scintilla3.Text)
        SwitchScript(CurrentlyEditedScript - 1)
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        SaveConfiguredScript(CurrentlyEditedScript, Scintilla3.Text)
        SwitchScript(0)
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        ' Don't do it if there are no items
        If CurrentlyConfiguredScripts.Count = 0 Then Exit Sub

        CurrentlyConfiguredScripts.RemoveAt(CurrentlyEditedScript)
        ' Check again if there are no items
        If CurrentlyConfiguredScripts.Count = 0 Then
            NoSpecifiedScriptsPanel.Visible = True
            ScriptEditorPanel.Visible = False
            Label66.Visible = False
            Button13.Enabled = False
            Button14.Enabled = False
            Button15.Enabled = False
            Button17.Enabled = False
            Button18.Enabled = False
            Button23.Enabled = False
        Else
            If CurrentlyEditedScript > CurrentlyConfiguredScripts.Count - 1 Then
                CurrentlyEditedScript = CurrentlyConfiguredScripts.Count - 1
            End If
            SwitchScript(CurrentlyEditedScript)
        End If
    End Sub

    Sub UpdateScriptEditorLexer(LexerName As String)
        If {"powershell", "batch", "vbscript", "jscript"}.Contains(LexerName) Then
            ClearScriptEditorKeywords()

            ' I want a correct set of keywords whenever we switch the lexer language.
            'Scintilla3.LexerName = LexerName
            Select Case LexerName
                Case "powershell"
                    Scintilla3.LexerName = "powershell"
                    AddScintillaKeywords("powershell", 0, "begin break catch class continue data do dynamicparam else elseif end enum exit filter finally for foreach function hidden if in inlinescript parallel param process return sequence static switch throw trap try until using while workflow")
                    AddScintillaKeywords("powershell", 1, "add-appprovisionedsharedpackagecontainer add-appsharedpackagecontainer add-appvclientconnectiongroup add-appvclientpackage add-appvpublishingserver add-appxpackage add-appxprovisionedpackage add-appxvolume add-bitsfile add-certificateenrollmentpolicyserver add-computer add-content add-history add-jobtrigger add-kdsrootkey add-localgroupmember add-member add-pssnapin add-signerrule add-type add-windowscapability add-windowsdriver add-windowsimage add-windowspackage checkpoint-computer clear-content clear-eventlog clear-history clear-item clear-itemproperty clear-kdscache clear-recyclebin clear-tpm clear-uevappxpackage clear-uevconfiguration clear-variable clear-windowscorruptmountpoint compare-object complete-bitstransfer complete-dtcdiagnostictransaction complete-transaction confirm-securebootuefi connect-pssession connect-wsman convert-path convert-string convertfrom-cipolicy convertfrom-csv convertfrom-json convertfrom-securestring convertfrom-string convertfrom-stringdata convertto-csv convertto-html convertto-json convertto-processmitigationpolicy convertto-securestring convertto-tpmownerauth convertto-xml copy-bcdentry copy-item copy-itemproperty copy-userinternationalsettingstosystem debug-job debug-process debug-runspace disable-appbackgroundtaskdiagnosticlog disable-appv disable-appvclientconnectiongroup disable-bcdelementbootdebug disable-bcdelementbootems disable-bcdelementdebug disable-bcdelementems disable-bcdelementeventlogging disable-bcdelementhypervisordebug disable-computerrestore disable-jobtrigger disable-localuser disable-psbreakpoint disable-psremoting disable-pssessionconfiguration disable-runspacedebug disable-scheduledjob disable-tlsciphersuite disable-tlsecccurve disable-tlssessionticketkey disable-tpmautoprovisioning disable-uev disable-uevappxpackage disable-uevtemplate disable-wsmancredssp disable-windowserrorreporting disable-windowsoptionalfeature disconnect-pssession disconnect-wsman dismount-appxvolume dismount-windowsimage edit-cipolicyrule enable-appbackgroundtaskdiagnosticlog enable-appv enable-appvclientconnectiongroup enable-bcdelementbootdebug enable-bcdelementbootems enable-bcdelementdebug enable-bcdelementems enable-bcdelementeventlogging enable-bcdelementhypervisordebug enable-computerrestore enable-jobtrigger enable-localuser enable-psbreakpoint enable-psremoting enable-pssessionconfiguration enable-runspacedebug enable-scheduledjob enable-tlsciphersuite enable-tlsecccurve enable-tlssessionticketkey enable-tpmautoprovisioning enable-uev enable-uevappxpackage enable-uevtemplate enable-wsmancredssp enable-windowserrorreporting enable-windowsoptionalfeature enter-pshostprocess enter-pssession exit-pshostprocess exit-pssession expand-windowscustomdataimage expand-windowsimage export-alias export-bcdstore export-binarymilog export-certificate export-clixml export-console export-counter export-csv export-formatdata export-modulemember export-pssession export-pfxcertificate export-provisioningpackage export-startlayout export-startlayoutedgeassets export-tlssessionticketkey export-trace export-uevconfiguration export-uevpackage export-windowscapabilitysource export-windowsdriver export-windowsimage find-package find-packageprovider foreach-object format-custom format-list format-securebootuefi format-table format-wide get-acl get-alias get-applockerfileinformation get-applockerpolicy get-appprovisionedsharedpackagecontainer get-appsharedpackagecontainer get-appvclientapplication get-appvclientconfiguration get-appvclientconnectiongroup get-appvclientmode get-appvclientpackage get-appvpublishingserver get-appvstatus get-appxdefaultvolume get-appxpackage get-appxpackageautoupdatesettings get-appxpackagemanifest get-appxprovisionedpackage get-appxvolume get-authenticodesignature get-bcdentry get-bcdentrydebugsettings get-bcdentryhypervisorsettings get-bcdstore get-bitstransfer get-cipolicy get-cipolicyidinfo get-cipolicyinfo get-certificate get-certificateautoenrollmentpolicy get-certificateenrollmentpolicyserver get-certificatenotificationtask get-childitem get-cimassociatedinstance get-cimclass get-ciminstance get-cimsession get-clipboard get-cmsmessage get-command get-computerinfo get-computerrestorepoint get-content get-controlpanelitem get-counter get-credential get-culture get-dapolicychange get-date get-deliveryoptimizationlog get-deliveryoptimizationloganalysis get-event get-eventlog get-eventsubscriber get-executionpolicy get-formatdata get-help get-history get-host get-hotfix get-installedlanguage get-item get-itemproperty get-itempropertyvalue get-job get-jobtrigger get-kdsconfiguration get-kdsrootkey get-localgroup get-localgroupmember get-localuser get-location get-member get-module get-nonremovableappspolicy get-psbreakpoint get-pscallstack get-psdrive get-pshostprocessinfo get-psprovider get-psreadlinekeyhandler get-psreadlineoption get-pssession get-pssessioncapability get-pssessionconfiguration get-pssnapin get-package get-packageprovider get-packagesource get-pfxcertificate get-pfxdata get-pmemdedicatedmemory get-pmemdisk get-pmemphysicaldevice get-pmemunusedregion get-process get-processmitigation get-provisioningpackage get-random get-runspace get-runspacedebug get-scheduledjob get-scheduledjoboption get-securebootpolicy get-securebootuefi get-service get-systemdriver get-systempreferreduilanguage get-timezone get-tlsciphersuite get-tlsecccurve get-tpm get-tpmendorsementkeyinfo get-tpmsupportedfeature get-tracesource get-transaction get-troubleshootingpack get-trustedprovisioningcertificate get-typedata get-uiculture get-uevappxpackage get-uevconfiguration get-uevstatus get-uevtemplate get-uevtemplateprogram get-unique get-variable get-wimbootentry get-wsmancredssp get-wsmaninstance get-wheamemorypolicy get-winacceptlanguagefromlanguagelistoptout get-winculturefromlanguagelistoptout get-windefaultinputmethodoverride get-winevent get-winhomelocation get-winlanguagebaroption get-winsystemlocale get-winuilanguageoverride get-winuserlanguagelist get-windowscapability get-windowsdeveloperlicense get-windowsdriver get-windowsedition get-windowserrorreporting get-windowsimage get-windowsimagecontent get-windowsoptionalfeature get-windowspackage get-windowsreservedstoragestate get-windowssearchsetting get-wmiobject group-object import-alias import-bcdstore import-binarymilog import-certificate import-clixml import-counter import-csv import-localizeddata import-module import-pssession import-packageprovider import-pfxcertificate import-startlayout import-tpmownerauth import-uevconfiguration initialize-pmemphysicaldevice initialize-tpm install-language install-package install-packageprovider install-provisioningpackage install-trustedprovisioningcertificate invoke-cimmethod invoke-command invoke-commandindesktoppackage invoke-dscresource invoke-expression invoke-history invoke-item invoke-restmethod invoke-troubleshootingpack invoke-wsmanaction invoke-webrequest invoke-wmimethod join-dtcdiagnosticresourcemanager join-path limit-eventlog measure-command measure-object merge-cipolicy mount-appvclientconnectiongroup mount-appvclientpackage mount-appxvolume mount-windowsimage move-appxpackage move-item move-itemproperty new-alias new-applockerpolicy new-bcdentry new-bcdstore new-cipolicy new-cipolicyrule new-certificatenotificationtask new-ciminstance new-cimsession new-cimsessionoption new-dtcdiagnostictransaction new-event new-eventlog new-filecatalog new-item new-itemproperty new-jobtrigger new-localgroup new-localuser new-module new-modulemanifest new-netipsecauthproposal new-netipsecmainmodecryptoproposal new-netipsecquickmodecryptoproposal new-object new-psdrive new-psrolecapabilityfile new-pssession new-pssessionconfigurationfile new-pssessionoption new-pstransportoption new-psworkflowexecutionoption new-pmemdedicatedmemory new-pmemdisk new-provisioningrepro new-scheduledjoboption new-selfsignedcertificate new-service new-timespan new-tlssessionticketkey new-variable new-wsmaninstance new-wsmansessionoption new-webserviceproxy new-winevent new-winuserlanguagelist new-windowscustomimage new-windowsimage optimize-appxprovisionedpackages optimize-windowsimage out-default out-file out-gridview out-host out-null out-printer out-string pop-location protect-cmsmessage publish-appvclientpackage publish-dscconfiguration push-location read-host receive-dtcdiagnostictransaction receive-job receive-pssession register-argumentcompleter register-cimindicationevent register-engineevent register-objectevent register-pssessionconfiguration register-packagesource register-scheduledjob register-uevtemplate register-wmievent remove-appprovisionedsharedpackagecontainer remove-appsharedpackagecontainer remove-appvclientconnectiongroup remove-appvclientpackage remove-appvpublishingserver remove-appxpackage remove-appxpackageautoupdatesettings remove-appxprovisionedpackage remove-appxvolume remove-bcdelement remove-bcdentry remove-bitstransfer remove-cipolicyrule remove-certificateenrollmentpolicyserver remove-certificatenotificationtask remove-ciminstance remove-cimsession remove-computer remove-event remove-eventlog remove-item remove-itemproperty remove-job remove-jobtrigger remove-localgroup remove-localgroupmember remove-localuser remove-module remove-psbreakpoint remove-psdrive remove-psreadlinekeyhandler remove-pssession remove-pssnapin remove-pmemdedicatedmemory remove-pmemdisk remove-typedata remove-variable remove-wsmaninstance remove-windowscapability remove-windowsdriver remove-windowsimage remove-windowspackage remove-wmiobject rename-computer rename-item rename-itemproperty rename-localgroup rename-localuser repair-appvclientconnectiongroup repair-appvclientpackage repair-uevtemplateindex repair-windowsimage reset-appsharedpackagecontainer reset-appxpackage reset-computermachinepassword resolve-dnsname resolve-path restart-computer restart-service restore-computer restore-uevbackup restore-uevusersetting resume-bitstransfer resume-job resume-provisioningsession resume-service save-help save-package save-windowsimage select-object select-string select-xml send-appvclientreport send-dtcdiagnostictransaction send-mailmessage set-acl set-alias set-appbackgroundtaskresourcepolicy set-applockerpolicy set-appxprovisioneddatafile set-appvclientconfiguration set-appvclientmode set-appvclientpackage set-appvpublishingserver set-appxdefaultvolume set-appxpackageautoupdatesettings set-authenticodesignature set-bcdbootdefault set-bcdbootdisplayorder set-bcdbootsequence set-bcdboottimeout set-bcdboottoolsdisplayorder set-bcddebugsettings set-bcdelement set-bcdhypervisorsettings set-bitstransfer set-cipolicyidinfo set-cipolicysetting set-cipolicyversion set-certificateautoenrollmentpolicy set-ciminstance set-clipboard set-content set-culture set-date set-dsclocalconfigurationmanager set-executionpolicy set-hvcioptions set-item set-itemproperty set-jobtrigger set-kdsconfiguration set-localgroup set-localuser set-location set-nonremovableappspolicy set-psbreakpoint set-psdebug set-psreadlinekeyhandler set-psreadlineoption set-pssessionconfiguration set-packagesource set-processmitigation set-ruleoption set-scheduledjob set-scheduledjoboption set-securebootuefi set-service set-strictmode set-systempreferreduilanguage set-timezone set-tpmownerauth set-tracesource set-uevconfiguration set-uevtemplateprofile set-variable set-wsmaninstance set-wsmanquickconfig set-wheamemorypolicy set-winacceptlanguagefromlanguagelistoptout set-winculturefromlanguagelistoptout set-windefaultinputmethodoverride set-winhomelocation set-winlanguagebaroption set-winsystemlocale set-winuilanguageoverride set-winuserlanguagelist set-windowsedition set-windowsproductkey set-windowsreservedstoragestate set-windowssearchsetting set-wmiinstance show-command show-controlpanelitem show-eventlog show-windowsdeveloperlicenseregistration sort-object split-path split-windowsimage start-bitstransfer start-dscconfiguration start-dtcdiagnosticresourcemanager start-job start-osuninstall start-process start-service start-sleep start-transaction start-transcript stop-appvclientconnectiongroup stop-appvclientpackage stop-computer stop-dtcdiagnosticresourcemanager stop-job stop-process stop-service stop-transcript suspend-bitstransfer suspend-job suspend-service switch-certificate sync-appvpublishingserver tee-object test-applockerpolicy test-certificate test-computersecurechannel test-connection test-dscconfiguration test-filecatalog test-kdsrootkey test-modulemanifest test-pssessionconfigurationfile test-path test-uevtemplate test-wsman trace-command unblock-file unblock-tpm undo-dtcdiagnostictransaction undo-transaction uninstall-language uninstall-package uninstall-provisioningpackage uninstall-trustedprovisioningcertificate unprotect-cmsmessage unpublish-appvclientpackage unregister-event unregister-pssessionconfiguration unregister-packagesource unregister-scheduledjob unregister-uevtemplate unregister-windowsdeveloperlicense update-dscconfiguration update-formatdata update-help update-list update-typedata update-uevtemplate update-wimbootentry use-transaction use-windowsunattend wait-debugger wait-event wait-job wait-process where-object write-debug write-error write-eventlog write-host write-information write-output write-progress write-verbose write-warning")
                    AddScintillaKeywords("powershell", 2, "% ? add-apppackage add-apppackagevolume add-appprovisionedpackage add-provisionedapppackage add-provisionedappsharedpackagecontainer add-provisionedappxpackage add-provisioningpackage add-trustedprovisioningcertificate apply-windowsunattend cfs disable-physicaldiskindication disable-storagediagnosticlog dismount-apppackagevolume enable-physicaldiskindication enable-storagediagnosticlog flush-volume get-apppackage get-apppackageautoupdatesettings get-apppackagedefaultvolume get-apppackagelasterror get-apppackagelog get-apppackagemanifest get-apppackagevolume get-appprovisionedpackage get-disksnv get-language get-physicaldisksnv get-preferredlanguage get-provisionedapppackage get-provisionedappsharedpackagecontainer get-provisionedappxpackage get-storageenclosuresnv get-systemlanguage initialize-volume mount-apppackagevolume move-apppackage move-smbclient optimize-appprovisionedpackages optimize-provisionedapppackages optimize-provisionedappxpackages remove-apppackage remove-apppackageautoupdatesettings remove-apppackagevolume remove-appprovisionedpackage remove-etwtracesession remove-provisionedapppackage remove-provisionedappsharedpackagecontainer remove-provisionedappxpackage remove-provisioningpackage remove-trustedprovisioningcertificate reset-apppackage set-apppackageautoupdatesettings set-apppackagedefaultvolume set-apppackageprovisioneddatafile set-autologgerconfig set-etwtracesession set-preferredlanguage set-provisionedapppackagedatafile set-provisionedappxdatafile set-systemlanguage tnc write-filesystemcache ac algm asnp blsmba cat cd chdir clc clear clhy cli clp cls clv cnsn compare copy cp cpi cpp cssmbo cssmbse curl cvpa dbp del diff dir dlu dnsn dsmbd ebp echo elu epal epcsv epsn erase esmbd etsn exsn fc fhx fimo fl foreach ft fw gal gbp gc gcai gcb gcfg gcfgs gci gcim gcls gcm gcms gcs gdr ghy gi gin gip gjb gl glcm glg glgm glu gm gmo gp gps gpv group grsmba gsmba gsmbb gsmbc gsmbcc gsmbcn gsmbd gsmbgm gsmbm gsmbmc gsmbo gsmbs gsmbsc gsmbscm gsmbscp gsmbse gsmbsn gsmbt gsmbw gsn gsnp gsv gtz gu gv gwmi h history icim icm iex ihy ii inmo ipal ipcsv ipmo ipsn irm ise iwmi iwr kill lp ls man md measure mi mount move mp msmbw mv nal ncim ncms ncso ndr ni nlg nlu nmo npssc nsmbgm nsmbm nsmbs nsmbscm nsmbt nsn nv nwsn ogv oh pbcfg popd ps pumo pushd pwd r rbp rcie rcim rcjb rcms rcsn rd rdr ren ri rjb rksmba rlg rlgm rlu rm rmdir rmo rni rnlg rnlu rnp rp rsmbb rsmbc rsmbcc rsmbgm rsmbm rsmbs rsmbsc rsmbscm rsmbt rsn rsnp rtcfg rujb rv rvpa rwmi sacfg sajb sal saps sasv sbp sc scb scim select set shcm si sl slcm sleep slg sls slu sort sp spjb spps spsv ssmbb ssmbcc ssmbp ssmbs ssmbsc ssmbscm start stz sujb sv swmi tcfg tee trcm type udsmbmc ulsmba upcfg upmo wget where wjb write")
                    AddScintillaKeywords("powershell", 3, "a: add-bcdatacacheextension add-bitlockerkeyprotector add-dnsclientdohserveraddress add-dnsclientnrptrule add-dtcclustertmmapping add-etwtraceprovider add-initiatoridtomaskingset add-mppreference add-neteventnetworkadapter add-neteventpacketcaptureprovider add-neteventprovider add-neteventvfpprovider add-neteventvmnetworkadapter add-neteventvmswitch add-neteventvmswitchprovider add-neteventwfpcaptureprovider add-netiphttpscertbinding add-netlbfoteammember add-netlbfoteamnic add-netnatexternaladdress add-netnatstaticmapping add-netswitchteammember add-odbcdsn add-partitionaccesspath add-physicaldisk add-printer add-printerdriver add-printerport add-storagefaultdomain add-targetporttomaskingset add-vmdirectvirtualdisk add-virtualdisktomaskingset add-vpnconnection add-vpnconnectionroute add-vpnconnectiontriggerapplication add-vpnconnectiontriggerdnsconfiguration add-vpnconnectiontriggertrustednetwork afterall aftereach assert-mockcalled assert-verifiablemocks b: backup-bitlockerkeyprotector backuptoaad-bitlockerkeyprotector beforeall beforeeach block-fileshareaccess block-smbshareaccess c: clear-assignedaccess clear-bccache clear-bitlockerautounlock clear-disk clear-dnsclientcache clear-filestoragetier clear-host clear-pcsvdevicelog clear-storagebusdisk clear-storagediagnosticinfo close-smbopenfile close-smbsession compress-archive configuration connect-iscsitarget connect-virtualdisk context convertfrom-sddlstring copy-netfirewallrule copy-netipsecmainmodecryptoset copy-netipsecmainmoderule copy-netipsecphase1authset copy-netipsecphase2authset copy-netipsecquickmodecryptoset copy-netipsecrule d: debug-fileshare debug-mmappprelaunch debug-storagesubsystem debug-volume delete-deliveryoptimizationcache describe disable-bc disable-bcdowngrading disable-bcserveonbattery disable-bitlocker disable-bitlockerautounlock disable-damanualentrypointselection disable-deliveryoptimizationverboselogs disable-dscdebug disable-mmagent disable-netadapter disable-netadapterbinding disable-netadapterchecksumoffload disable-netadapterencapsulatedpackettaskoffload disable-netadapteripsecoffload disable-netadapterlso disable-netadapterpacketdirect disable-netadapterpowermanagement disable-netadapterqos disable-netadapterrdma disable-netadapterrsc disable-netadapterrss disable-netadaptersriov disable-netadapteruso disable-netadaptervmq disable-netdnstransitionconfiguration disable-netfirewallrule disable-netiphttpsprofile disable-netipsecmainmoderule disable-netipsecrule disable-netnattransitionconfiguration disable-networkswitchethernetport disable-networkswitchfeature disable-networkswitchvlan disable-odbcperfcounter disable-pstrace disable-pswsmancombinedtrace disable-physicaldiskidentification disable-pnpdevice disable-scheduledtask disable-smbdelegation disable-storagebuscache disable-storagebusdisk disable-storagedatacollection disable-storageenclosureidentification disable-storageenclosurepower disable-storagehighavailability disable-storagemaintenancemode disable-wsmantrace disable-wdacbidtrace disconnect-iscsitarget disconnect-virtualdisk dismount-diskimage e: enable-bcdistributed enable-bcdowngrading enable-bchostedclient enable-bchostedserver enable-bclocal enable-bcserveonbattery enable-bitlocker enable-bitlockerautounlock enable-damanualentrypointselection enable-deliveryoptimizationverboselogs enable-dscdebug enable-mmagent enable-netadapter enable-netadapterbinding enable-netadapterchecksumoffload enable-netadapterencapsulatedpackettaskoffload enable-netadapteripsecoffload enable-netadapterlso enable-netadapterpacketdirect enable-netadapterpowermanagement enable-netadapterqos enable-netadapterrdma enable-netadapterrsc enable-netadapterrss enable-netadaptersriov enable-netadapteruso enable-netadaptervmq enable-netdnstransitionconfiguration enable-netfirewallrule enable-netiphttpsprofile enable-netipsecmainmoderule enable-netipsecrule enable-netnattransitionconfiguration enable-networkswitchethernetport enable-networkswitchfeature enable-networkswitchvlan enable-odbcperfcounter enable-pstrace enable-pswsmancombinedtrace enable-physicaldiskidentification enable-pnpdevice enable-scheduledtask enable-smbdelegation enable-storagebuscache enable-storagebusdisk enable-storagedatacollection enable-storageenclosureidentification enable-storageenclosurepower enable-storagehighavailability enable-storagemaintenancemode enable-wsmantrace enable-wdacbidtrace expand-archive export-bccachepackage export-bcsecretkey export-odataendpointproxy export-scheduledtask export-winhttpproxy f: find-command find-dscresource find-module find-netipsecrule find-netroute find-rolecapability find-script flush-etwtracesession format-hex format-volume g: get-appbackgroundtask get-appvvirtualprocess get-appxlasterror get-appxlog get-assignedaccess get-autologgerconfig get-bcclientconfiguration get-bccontentserverconfiguration get-bcdatacache get-bcdatacacheextension get-bchashcache get-bchostedcacheserverconfiguration get-bcnetworkconfiguration get-bcstatus get-bitlockervolume get-clusteredscheduledtask get-daclientexperienceconfiguration get-daconnectionstatus get-daentrypointtableitem get-doconfig get-dodownloadmode get-dopercentagemaxbackgroundbandwidth get-dopercentagemaxforegroundbandwidth get-dedupproperties get-deliveryoptimizationperfsnap get-deliveryoptimizationperfsnapthismonth get-deliveryoptimizationstatus get-disk get-diskimage get-diskstoragenodeview get-dnsclient get-dnsclientcache get-dnsclientdohserveraddress get-dnsclientglobalsetting get-dnsclientnrptglobal get-dnsclientnrptpolicy get-dnsclientnrptrule get-dnsclientserveraddress get-dscconfiguration get-dscconfigurationstatus get-dsclocalconfigurationmanager get-dscresource get-dtc get-dtcadvancedhostsetting get-dtcadvancedsetting get-dtcclusterdefault get-dtcclustertmmapping get-dtcdefault get-dtclog get-dtcnetworksetting get-dtctransaction get-dtctransactionsstatistics get-dtctransactionstracesession get-dtctransactionstracesetting get-etwtraceprovider get-etwtracesession get-filehash get-fileintegrity get-fileshare get-fileshareaccesscontrolentry get-filestoragetier get-initiatorid get-initiatorport get-installedmodule get-installedscript get-iscsiconnection get-iscsisession get-iscsitarget get-iscsitargetportal get-isesnippet get-logproperties get-mmagent get-maskingset get-mockdynamicparameters get-mpcomputerstatus get-mpperformancereport get-mppreference get-mpthreat get-mpthreatcatalog get-mpthreatdetection get-ncsipolicyconfiguration get-net6to4configuration get-netadapter get-netadapteradvancedproperty get-netadapterbinding get-netadapterchecksumoffload get-netadapterdatapathconfiguration get-netadapterencapsulatedpackettaskoffload get-netadapterhardwareinfo get-netadapteripsecoffload get-netadapterlso get-netadapterpacketdirect get-netadapterpowermanagement get-netadapterqos get-netadapterrdma get-netadapterrsc get-netadapterrss get-netadaptersriov get-netadaptersriovvf get-netadapterstatistics get-netadapteruso get-netadaptervmqqueue get-netadaptervport get-netadaptervmq get-netcompartment get-netconnectionprofile get-netdnstransitionconfiguration get-netdnstransitionmonitoring get-neteventnetworkadapter get-neteventpacketcaptureprovider get-neteventprovider get-neteventsession get-neteventvfpprovider get-neteventvmnetworkadapter get-neteventvmswitch get-neteventvmswitchprovider get-neteventwfpcaptureprovider get-netfirewalladdressfilter get-netfirewallapplicationfilter get-netfirewalldynamickeywordaddress get-netfirewallinterfacefilter get-netfirewallinterfacetypefilter get-netfirewallportfilter get-netfirewallprofile get-netfirewallrule get-netfirewallsecurityfilter get-netfirewallservicefilter get-netfirewallsetting get-netipaddress get-netipconfiguration get-netiphttpsconfiguration get-netiphttpsstate get-netipinterface get-netipsecdospsetting get-netipsecmainmodecryptoset get-netipsecmainmoderule get-netipsecmainmodesa get-netipsecphase1authset get-netipsecphase2authset get-netipsecquickmodecryptoset get-netipsecquickmodesa get-netipsecrule get-netipv4protocol get-netipv6protocol get-netisatapconfiguration get-netlbfoteam get-netlbfoteammember get-netlbfoteamnic get-netnat get-netnatexternaladdress get-netnatglobal get-netnatsession get-netnatstaticmapping get-netnattransitionconfiguration get-netnattransitionmonitoring get-netneighbor get-netoffloadglobalsetting get-netprefixpolicy get-netqospolicy get-netroute get-netswitchteam get-netswitchteammember get-nettcpconnection get-nettcpsetting get-netteredoconfiguration get-netteredostate get-nettransportfilter get-netudpendpoint get-netudpsetting get-netview get-networkswitchethernetport get-networkswitchfeature get-networkswitchglobaldata get-networkswitchvlan get-odbcdriver get-odbcdsn get-odbcperfcounter get-offloaddatatransfersetting get-operationvalidation get-psrepository get-partition get-partitionsupportedsize get-pcsvdevice get-pcsvdevicelog get-physicaldisk get-physicaldiskstoragenodeview get-physicalextent get-physicalextentassociation get-pnpdevice get-pnpdeviceproperty get-printconfiguration get-printjob get-printer get-printerdriver get-printerport get-printerproperty get-resiliencysetting get-scheduledtask get-scheduledtaskinfo get-smbbandwidthlimit get-smbclientconfiguration get-smbclientnetworkinterface get-smbconnection get-smbdelegation get-smbglobalmapping get-smbmapping get-smbmultichannelconnection get-smbmultichannelconstraint get-smbopenfile get-smbservercertprops get-smbservercertificatemapping get-smbserverconfiguration get-smbservernetworkinterface get-smbsession get-smbshare get-smbshareaccess get-smbwitnessclient get-startapps get-storageadvancedproperty get-storagebusbinding get-storagebuscache get-storagebusclientdevice get-storagebusdisk get-storagebustargetcachestore get-storagebustargetcachestoresinstance get-storagebustargetdevice get-storagebustargetdeviceinstance get-storagechassis get-storagedatacollection get-storagediagnosticinfo get-storageenclosure get-storageenclosurestoragenodeview get-storageenclosurevendordata get-storageextendedstatus get-storagefaultdomain get-storagefileserver get-storagefirmwareinformation get-storagehealthaction get-storagehealthreport get-storagehealthsetting get-storagehistory get-storagejob get-storagenode get-storagepool get-storageprovider get-storagerack get-storagereliabilitycounter get-storagescaleunit get-storagesetting get-storagesite get-storagesubsystem get-storagetier get-storagetiersupportedsize get-supportedclustersizes get-supportedfilesystems get-targetport get-targetportal get-testdriveitem get-vmdirectvirtualdisk get-verb get-virtualdisk get-virtualdisksupportedsize get-volume get-volumecorruptioncount get-volumescrubpolicy get-vpnconnection get-vpnconnectiontrigger get-wdacbidtrace get-windowsupdatelog get-winhttpproxy grant-fileshareaccess grant-smbshareaccess h: hide-virtualdisk i: import-bccachepackage import-bcsecretkey import-isesnippet import-powershelldatafile import-winhttpproxy importsystemmodules in inmodulescope initialize-disk install-dtc install-module install-script invoke-asworkflow invoke-mock invoke-operationvalidation invoke-pester it j: k: l: lock-bitlocker m: mock mount-diskimage move-smbwitnessclient n: new-autologgerconfig new-daentrypointtableitem new-dscchecksum new-eapconfiguration new-etwtracesession new-fileshare new-fixture new-guid new-iscsitargetportal new-isesnippet new-maskingset new-mpperformancerecording new-netadapteradvancedproperty new-neteventsession new-netfirewalldynamickeywordaddress new-netfirewallrule new-netipaddress new-netiphttpsconfiguration new-netipsecdospsetting new-netipsecmainmodecryptoset new-netipsecmainmoderule new-netipsecphase1authset new-netipsecphase2authset new-netipsecquickmodecryptoset new-netipsecrule new-netlbfoteam new-netnat new-netnattransitionconfiguration new-netneighbor new-netqospolicy new-netroute new-netswitchteam new-nettransportfilter new-networkswitchvlan new-psworkflowsession new-partition new-pesteroption new-scheduledtask new-scheduledtaskaction new-scheduledtaskprincipal new-scheduledtasksettingsset new-scheduledtasktrigger new-scriptfileinfo new-smbglobalmapping new-smbmapping new-smbmultichannelconstraint new-smbservercertificatemapping new-smbshare new-storagebusbinding new-storagebuscachestore new-storagefileserver new-storagepool new-storagesubsystemvirtualdisk new-storagetier new-temporaryfile new-virtualdisk new-virtualdiskclone new-virtualdisksnapshot new-volume new-vpnserveraddress o: open-netgpo optimize-storagepool optimize-volume p: psconsolehostreadline pause publish-bcfilecontent publish-bcwebcontent publish-module publish-script q: r: read-printernfctag register-clusteredscheduledtask register-dnsclient register-iscsisession register-psrepository register-scheduledtask register-storagesubsystem remove-autologgerconfig remove-bcdatacacheextension remove-bitlockerkeyprotector remove-daentrypointtableitem remove-dnsclientdohserveraddress remove-dnsclientnrptrule remove-dscconfigurationdocument remove-dtcclustertmmapping remove-etwtraceprovider remove-fileshare remove-initiatorid remove-initiatoridfrommaskingset remove-iscsitargetportal remove-maskingset remove-mppreference remove-mpthreat remove-netadapteradvancedproperty remove-neteventnetworkadapter remove-neteventpacketcaptureprovider remove-neteventprovider remove-neteventsession remove-neteventvfpprovider remove-neteventvmnetworkadapter remove-neteventvmswitch remove-neteventvmswitchprovider remove-neteventwfpcaptureprovider remove-netfirewalldynamickeywordaddress remove-netfirewallrule remove-netipaddress remove-netiphttpscertbinding remove-netiphttpsconfiguration remove-netipsecdospsetting remove-netipsecmainmodecryptoset remove-netipsecmainmoderule remove-netipsecmainmodesa remove-netipsecphase1authset remove-netipsecphase2authset remove-netipsecquickmodecryptoset remove-netipsecquickmodesa remove-netipsecrule remove-netlbfoteam remove-netlbfoteammember remove-netlbfoteamnic remove-netnat remove-netnatexternaladdress remove-netnatstaticmapping remove-netnattransitionconfiguration remove-netneighbor remove-netqospolicy remove-netroute remove-netswitchteam remove-netswitchteammember remove-nettransportfilter remove-networkswitchethernetportipaddress remove-networkswitchvlan remove-odbcdsn remove-partition remove-partitionaccesspath remove-physicaldisk remove-printjob remove-printer remove-printerdriver remove-printerport remove-smbbandwidthlimit remove-smbcomponent remove-smbglobalmapping remove-smbmapping remove-smbmultichannelconstraint remove-smbservercertificatemapping remove-smbshare remove-storagebusbinding remove-storagefaultdomain remove-storagefileserver remove-storagehealthintent remove-storagehealthsetting remove-storagepool remove-storagetier remove-targetportfrommaskingset remove-vmdirectvirtualdisk remove-virtualdisk remove-virtualdiskfrommaskingset remove-vpnconnection remove-vpnconnectionroute remove-vpnconnectiontriggerapplication remove-vpnconnectiontriggerdnsconfiguration remove-vpnconnectiontriggertrustednetwork rename-daentrypointtableitem rename-maskingset rename-netadapter rename-netfirewallrule rename-netiphttpsconfiguration rename-netipsecmainmodecryptoset rename-netipsecmainmoderule rename-netipsecphase1authset rename-netipsecphase2authset rename-netipsecquickmodecryptoset rename-netipsecrule rename-netlbfoteam rename-netswitchteam rename-printer repair-fileintegrity repair-virtualdisk repair-volume reset-bc reset-daclientexperienceconfiguration reset-daentrypointtableitem reset-dtclog reset-ncsipolicyconfiguration reset-net6to4configuration reset-netadapteradvancedproperty reset-netdnstransitionconfiguration reset-netiphttpsconfiguration reset-netisatapconfiguration reset-netteredoconfiguration reset-physicaldisk reset-smbclientconfiguration reset-smbserverconfiguration reset-storagereliabilitycounter reset-winhttpproxy resize-partition resize-storagetier resize-virtualdisk restart-netadapter restart-pcsvdevice restart-printjob restore-dscconfiguration restore-networkswitchconfiguration resume-bitlocker resume-printjob resume-storagebusdisk revoke-fileshareaccess revoke-smbshareaccess s: safegetcommand save-etwtracesession save-module save-netgpo save-networkswitchconfiguration save-script save-storagedatacollection send-etwtracesession set-assignedaccess set-bcauthentication set-bccache set-bcdatacacheentrymaxage set-bcminsmblatency set-bcsecretkey set-clusteredscheduledtask set-daclientexperienceconfiguration set-daentrypointtableitem set-dodownloadmode set-domaxbackgroundbandwidth set-domaxforegroundbandwidth set-dopercentagemaxbackgroundbandwidth set-dopercentagemaxforegroundbandwidth set-deliveryoptimizationstatus set-disk set-dnsclient set-dnsclientdohserveraddress set-dnsclientglobalsetting set-dnsclientnrptglobal set-dnsclientnrptrule set-dnsclientserveraddress set-dtcadvancedhostsetting set-dtcadvancedsetting set-dtcclusterdefault set-dtcclustertmmapping set-dtcdefault set-dtclog set-dtcnetworksetting set-dtctransaction set-dtctransactionstracesession set-dtctransactionstracesetting set-dynamicparametervariables set-etwtraceprovider set-fileintegrity set-fileshare set-filestoragetier set-initiatorport set-iscsichapsecret set-logproperties set-mmagent set-mppreference set-ncsipolicyconfiguration set-net6to4configuration set-netadapter set-netadapteradvancedproperty set-netadapterbinding set-netadapterchecksumoffload set-netadapterdatapathconfiguration set-netadapterencapsulatedpackettaskoffload set-netadapteripsecoffload set-netadapterlso set-netadapterpacketdirect set-netadapterpowermanagement set-netadapterqos set-netadapterrdma set-netadapterrsc set-netadapterrss set-netadaptersriov set-netadapteruso set-netadaptervmq set-netconnectionprofile set-netdnstransitionconfiguration set-neteventpacketcaptureprovider set-neteventprovider set-neteventsession set-neteventvfpprovider set-neteventvmswitchprovider set-neteventwfpcaptureprovider set-netfirewalladdressfilter set-netfirewallapplicationfilter set-netfirewallinterfacefilter set-netfirewallinterfacetypefilter set-netfirewallportfilter set-netfirewallprofile set-netfirewallrule set-netfirewallsecurityfilter set-netfirewallservicefilter set-netfirewallsetting set-netipaddress set-netiphttpsconfiguration set-netipinterface set-netipsecdospsetting set-netipsecmainmodecryptoset set-netipsecmainmoderule set-netipsecphase1authset set-netipsecphase2authset set-netipsecquickmodecryptoset set-netipsecrule set-netipv4protocol set-netipv6protocol set-netisatapconfiguration set-netlbfoteam set-netlbfoteammember set-netlbfoteamnic set-netnat set-netnatglobal set-netnattransitionconfiguration set-netneighbor set-netoffloadglobalsetting set-netqospolicy set-netroute set-nettcpsetting set-netteredoconfiguration set-netudpsetting set-networkswitchethernetportipaddress set-networkswitchportmode set-networkswitchportproperty set-networkswitchvlanproperty set-odbcdriver set-odbcdsn set-psrepository set-partition set-pcsvdevicebootconfiguration set-pcsvdevicenetworkconfiguration set-pcsvdeviceuserpassword set-physicaldisk set-printconfiguration set-printer set-printerproperty set-resiliencysetting set-scheduledtask set-smbbandwidthlimit set-smbclientconfiguration set-smbpathacl set-smbservercertificatemapping set-smbserverconfiguration set-smbshare set-storagebuscache set-storagebusprofile set-storagefileserver set-storagehealthsetting set-storagepool set-storageprovider set-storagesetting set-storagesubsystem set-storagetier set-testinconclusive set-virtualdisk set-volume set-volumescrubpolicy set-vpnconnection set-vpnconnectionipsecconfiguration set-vpnconnectionproxy set-vpnconnectiontriggerdnsconfiguration set-vpnconnectiontriggertrustednetwork set-winhttpproxy setup should show-netfirewallrule show-netipsecrule show-storagehistory show-virtualdisk start-appbackgroundtask start-appvvirtualprocess start-autologgerconfig start-dtc start-dtctransactionstracesession start-etwtracesession start-mprollback start-mpscan start-mpwdoscan start-neteventsession start-pcsvdevice start-scheduledtask start-storagediagnosticlog start-trace stop-dscconfiguration stop-dtc stop-dtctransactionstracesession stop-etwtracesession stop-neteventsession stop-pcsvdevice stop-scheduledtask stop-storagediagnosticlog stop-storagejob stop-trace suspend-bitlocker suspend-printjob suspend-storagebusdisk sync-netipsecrule t: tabexpansion2 test-dtc test-netconnection test-scriptfileinfo u: unblock-fileshareaccess unblock-smbshareaccess uninstall-dtc uninstall-module uninstall-script unlock-bitlocker unregister-appbackgroundtask unregister-clusteredscheduledtask unregister-iscsisession unregister-psrepository unregister-scheduledtask unregister-storagesubsystem update-autologgerconfig update-disk update-etwtracesession update-hoststoragecache update-iscsitarget update-iscsitargetportal update-module update-modulemanifest update-mpsignature update-netfirewalldynamickeywordaddress update-netipsecrule update-script update-scriptfileinfo update-smbmultichannelconnection update-storagebuscache update-storagefirmware update-storagepool update-storageprovidercache v: w: write-dtctransactionstracesession write-printernfctag write-volumecache x: y: z: cd.. cd\ help mkdir more oss prompt")
                Case "batch"
                    Scintilla3.LexerName = "batch"
                    AddScintillaKeywords("batch", 0, "assoc aux break call cd chdir cls cmdextversion color com com1 com2 com3 com4 con copy country ctty date defined del dir do dpath echo else endlocal erase errorlevel exist exit for ftype goto if in loadfix loadhigh lpt lpt1 lpt2 lpt3 lpt4 md mkdir move not nul path pause popd prn prompt pushd rd rem ren rename rmdir set setlocal shift start time title type ver verify vol")
                Case "vbscript"
                    Scintilla3.LexerName = "vb"
                    AddScintillaKeywords("vbscript", 0, "addhandler addressof aggregate alias and andalso ansi as assembly async attribute auto await begin binary boolean by byref byte byval call case catch cbool cbyte cchar ccur cdate cdbl cdec char cint class clng clnglng clngptr cobj compare const continue csbyte cshort csng cstr ctype cuint culng currency cushort custom cvar date decimal declare default defbool defbyte defcur defdate defdbl defdec defint deflng deflnglng deflngptr defobj defsng defstr defvar delegate dim directcast distinct do double each else elseif end endif enum equals erase error event exit explicit false finally for friend from function get gettype global gosub goto group handles if implement implements imports in inherits integer interface into is isfalse isnot istrue iterator join key let lib like load long longlong longptr loop lset me mid mod module mustinherit mustoverride mybase myclass namespace narrowing new next not nothing notinheritable notoverridable object of off on operator option optional or order orelse out overloads overridable overrides paramarray partial preserve private property protected ptrsafe public raiseevent readonly redim rem removehandler resume return rset sbyte select set shadows shared short single skip static step stop strict string structure sub synclock take text then throw to true try trycast type typeof uinteger ulong unicode unload until ushort using variant vbarray vbboolean vbbyte vbcurrency vbdataobject vbdate vbdecimal vbdouble vbempty vberror vbinteger vblong vblonglong vbnull vbobject vbsingle vbuserdefinedtype vbvariant wend when where while widening with withevents writeonly xor yield")
                Case "jscript"
                    Scintilla3.LexerName = "coffeescript"
                    AddScintillaKeywords("jscript", 0, "abstract async await boolean break byte case catch char class const continue debugger default delete do double else enum export extends final finally float for function goto if implements import in instanceof int interface let long native new null of package private protected public return short static super switch synchronized this throw throws transient try typeof var void volatile while with true false prototype yield")
                    AddScintillaKeywords("jscript", 1, "Array Date eval hasOwnProperty Infinity isFinite isNaN isPrototypeOf Math NaN Number Object prototype String toString undefined valueOf")
                    AddScintillaKeywords("jscript", 2, "alert all anchor anchors area assign blur button checkbox clearInterval clearTimeout clientInformation close closed confirm constructor crypto decodeURI decodeURIComponent defaultStatus document element elements embed embeds encodeURI encodeURIComponent escape event fileUpload focus form forms frame innerHeight innerWidth layer layers link location mimeTypes navigate navigator frames frameRate hidden history image images offscreenBuffering onblur onclick onerror onfocus onkeydown onkeypress onkeyup onmouseover onload onmouseup onmousedown onsubmit open opener option outerHeight outerWidth packages pageXOffset pageYOffset parent parseFloat parseInt password pkcs11 plugin prompt propertyIsEnum radio reset screenX screenY scroll secure select self setInterval setTimeout status submit taint text textarea top unescape untaint window")
            End Select
        End If
    End Sub

    Private Sub ComboBox16_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox16.SelectedIndexChanged
        Try
            CurrentlyConfiguredScripts(CurrentlyEditedScript).ScriptExtension = ComboBox16.SelectedIndex
            Select Case ComboBox16.SelectedIndex
                Case 0
                    UpdateScriptEditorLexer("powershell")
                Case 1
                    UpdateScriptEditorLexer("batch")
                Case 2
                    UpdateScriptEditorLexer("vbscript")
                Case 3
                    UpdateScriptEditorLexer("jscript")
            End Select
        Catch ex As Exception

        End Try
    End Sub

    Private Sub OpenFileDialog2_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog2.FileOk
        Try
            DynaLog.LogMessage("Detemining contents of current Scintilla control...")
            If Scintilla3.Text <> "" Then
                DynaLog.LogMessage("Current Scintilla control is not empty. Asking before proceeding...")
                If MsgBox(LocalizationService.ForSection("Unattend.Messages")("ImportOverwrite.Message"), vbYesNo + vbQuestion) = MsgBoxResult.No Then
                    DynaLog.LogMessage("User said no. Exiting...")
                    Exit Sub
                End If
            End If

            DynaLog.LogMessage("Opening the file for read access...")
            Dim StarterScriptContents() As String = File.ReadAllLines(OpenFileDialog2.FileName)

            DynaLog.LogMessage("Determining file extension...")
            ' The first line indicates the extension we need to apply to show pretty colors. The rest is the script.
            Select Case StarterScriptContents(0)
                Case "Language: PowerShell"
                    ComboBox16.SelectedIndex = 0
                Case "Language: Batch"
                    ComboBox16.SelectedIndex = 1
                Case "Language: VBScript"
                    ComboBox16.SelectedIndex = 2
                Case "Language: JScript"
                    ComboBox16.SelectedIndex = 3
            End Select

            DynaLog.LogMessage("Loading contents...")
            Scintilla3.Text = String.Join(CrLf, StarterScriptContents.Skip(3).ToArray())
        Catch ex As Exception
            DynaLog.LogMessage("Could not open file. Error: " & ex.Message)
        End Try
    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        OpenFileDialog2.ShowDialog(Me)
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        ' Determining on the selected stage, we show a certain amount of items
        SampleScriptBrowser.FinalScriptStage = CurrentlyEditedStage

        If SampleScriptBrowser.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Opening the file for read access...")
            Dim StarterScriptContents As String = SampleScriptBrowser.FinalScriptCode

            DynaLog.LogMessage("Determining file extension...")
            ' The first line indicates the extension we need to apply to show pretty colors. The rest is the script.
            Select Case SampleScriptBrowser.FinalScriptLanguage
                Case "PowerShell"
                    ComboBox16.SelectedIndex = 0
                Case "Batch"
                    ComboBox16.SelectedIndex = 1
            End Select

            DynaLog.LogMessage("Loading contents...")
            Scintilla3.Text = StarterScriptContents
        End If
    End Sub

    Private Sub Button21_Click(sender As Object, e As EventArgs) Handles Button21.Click
        If MainForm.CurrentImage Is Nothing Then Exit Sub

        If EditionMapping.ContainsKey(MainForm.CurrentImage.ImageEditionId) Then
            ComboBox6.SelectedItem = EditionMapping(MainForm.CurrentImage.ImageEditionId)
        Else
            MsgBox(LocalizationService.ForSection("Unattend.Messages").Format("ProductKey.None.Label", MainForm.CurrentImage.ImageEditionId), vbOKOnly + vbInformation)
        End If
    End Sub

    Private Sub Button21_MouseHover(sender As Object, e As EventArgs) Handles Button21.MouseHover
        CNameTTip.Show(LocalizationService.ForSection("Unattend.Tooltips")("Attempt.Grab.Message"), sender)
    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        ComboBox4.SelectedItem = "Ireland"
    End Sub

    Private Sub Button22_MouseHover(sender As Object, e As EventArgs) Handles Button22.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("Unattend.Tooltips")("AutoChoose.Message"))
    End Sub

    Private Sub CheckBox23_MouseHover(sender As Object, e As EventArgs) Handles CheckBox27.MouseHover, CheckBox26.MouseHover, CheckBox25.MouseHover, CheckBox24.MouseHover, CheckBox23.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("Unattend.Tooltips")("Check.Field.Customize.Label"))
    End Sub

    Private Sub CheckBox27_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox27.CheckedChanged
        TextBox23.Enabled = CheckBox27.Checked
        ModifyUserDetails(4, CheckBox11.Checked, TextBox17.Text, If(CheckBox27.Checked, TextBox23.Text, ""), TextBox18.Text, GroupFromSelectedItem(ComboBox12.SelectedIndex))
    End Sub

    Private Sub CheckBox26_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox26.CheckedChanged
        TextBox22.Enabled = CheckBox26.Checked
        ModifyUserDetails(3, CheckBox10.Checked, TextBox14.Text, If(CheckBox26.Checked, TextBox22.Text, ""), TextBox15.Text, GroupFromSelectedItem(ComboBox11.SelectedIndex))
    End Sub

    Private Sub CheckBox25_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox25.CheckedChanged
        TextBox21.Enabled = CheckBox25.Checked
        ModifyUserDetails(2, CheckBox9.Checked, TextBox11.Text, If(CheckBox25.Checked, TextBox21.Text, ""), TextBox12.Text, GroupFromSelectedItem(ComboBox10.SelectedIndex))
    End Sub

    Private Sub CheckBox24_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox24.CheckedChanged
        TextBox20.Enabled = CheckBox24.Checked
        ModifyUserDetails(1, CheckBox8.Checked, TextBox8.Text, If(CheckBox24.Checked, TextBox20.Text, ""), TextBox9.Text, GroupFromSelectedItem(ComboBox9.SelectedIndex))
    End Sub

    Private Sub CheckBox23_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox23.CheckedChanged
        TextBox19.Enabled = CheckBox23.Checked
        ModifyUserDetails(0, True, TextBox4.Text, If(CheckBox23.Checked, TextBox19.Text, ""), TextBox6.Text, GroupFromSelectedItem(ComboBox7.SelectedIndex))
    End Sub

    Private Sub TextBox19_TextChanged(sender As Object, e As EventArgs) Handles TextBox19.TextChanged
        ModifyUserDetails(0, True, TextBox4.Text, If(CheckBox23.Checked, TextBox19.Text, ""), TextBox6.Text, GroupFromSelectedItem(ComboBox7.SelectedIndex))
    End Sub

    Private Sub TextBox20_TextChanged(sender As Object, e As EventArgs) Handles TextBox20.TextChanged
        ModifyUserDetails(1, CheckBox8.Checked, TextBox8.Text, If(CheckBox24.Checked, TextBox20.Text, ""), TextBox9.Text, GroupFromSelectedItem(ComboBox9.SelectedIndex))
    End Sub

    Private Sub TextBox21_TextChanged(sender As Object, e As EventArgs) Handles TextBox21.TextChanged
        ModifyUserDetails(2, CheckBox9.Checked, TextBox11.Text, If(CheckBox25.Checked, TextBox21.Text, ""), TextBox12.Text, GroupFromSelectedItem(ComboBox10.SelectedIndex))
    End Sub

    Private Sub TextBox22_TextChanged(sender As Object, e As EventArgs) Handles TextBox22.TextChanged
        ModifyUserDetails(3, CheckBox10.Checked, TextBox14.Text, If(CheckBox26.Checked, TextBox22.Text, ""), TextBox15.Text, GroupFromSelectedItem(ComboBox11.SelectedIndex))
    End Sub

    Private Sub TextBox23_TextChanged(sender As Object, e As EventArgs) Handles TextBox23.TextChanged
        ModifyUserDetails(4, CheckBox11.Checked, TextBox17.Text, If(CheckBox27.Checked, TextBox23.Text, ""), TextBox18.Text, GroupFromSelectedItem(ComboBox12.SelectedIndex))
    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        SaveConfiguredScripts(CurrentlyEditedStage)
        Select Case CurrentlyEditedStage
            Case 0
                ScriptReorderDialog.ScriptSet = ConfiguredScripts(PostInstallScript.Stage.Specialize)
            Case 1
                ScriptReorderDialog.ScriptSet = ConfiguredScripts(PostInstallScript.Stage.FirstRun)
            Case 2
                ScriptReorderDialog.ScriptSet = ConfiguredScripts(PostInstallScript.Stage.UserFirstLogon)
        End Select
        If ScriptReorderDialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            CurrentlyConfiguredScripts = New List(Of PostInstallScript)(ScriptReorderDialog.ScriptSet)
            ' Saving the script set here causes the currently edited script to change its contents
            ' to the previous values; we'll force setting the new contents.
            Scintilla3.Text = CurrentlyConfiguredScripts(CurrentlyEditedScript).ScriptContents
            SaveConfiguredScripts(CurrentlyEditedStage)
            SwitchScript(CurrentlyEditedScript)
        End If
    End Sub

    Private Sub Button23_MouseHover(sender As Object, e As EventArgs) Handles Button23.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("Unattend.Tooltips")("RearrangeScripts.Label"))
    End Sub
End Class