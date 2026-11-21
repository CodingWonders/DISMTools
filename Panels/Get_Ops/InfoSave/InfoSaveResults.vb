Imports System.IO
Imports System.Drawing.Printing
Imports Markdig
Imports Microsoft.VisualBasic.ControlChars
Imports System.Threading.Tasks
Imports System.Collections.Generic
Imports DISMTools.Elements.Contemporaneus
Imports DISMTools.Elements.Contemporaneus.ImageEXProperties
Imports DISMTools.Utilities.OfficeUtilities
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Drawing
Imports Microsoft.Dism
Imports System.Runtime.InteropServices
Imports System.Globalization

Public Class InfoSaveResults

    Public FilePath As String = ""

    Private LogFont As String
    Private LogFontSize As Single

    Public StoredInformation As ObtainedImageInformation

    Private currentOSCulture As CultureInfo = CultureInfo.CurrentCulture

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Close()
    End Sub

    Private Async Sub InfoSaveResults_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = LocalizationService.ForSection("InfoSaveResults")("Image.Report.Label")
        Label1.Text = LocalizationService.ForSection("InfoSaveResults")("ReportSaved.Message")
        Button1.Text = LocalizationService.ForSection("InfoSaveResults.Actions")("Ok.Button")
        Button2.Text = LocalizationService.ForSection("InfoSaveResults")("SaveReport.Button")
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        cmsSaveTargets.BackColor = BackColor
        cmsSaveTargets.ForeColor = ForeColor
        cmsSaveTargets.Renderer = GetProfessionalRenderer()

        ' update icons
        ExcelTSMI.Image = GetGlyphResource("report_target_excel")

        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        LogFont = MainForm.LogFont
        LogFontSize = MainForm.LogFontSize
        If Debugger.IsAttached Then
            Debugger.Break()
        End If
        ' Only file and Markdown processing happens in the background. Controls (including the WebBrowser, which is
        ' an STA COM control) are only touched on the UI thread
        TextBox1.Clear()
        Await Task.Run(Sub()
                           DynaLog.LogMessage("Checking if the report exists...")
                           If File.Exists(FilePath) Then
                               DynaLog.LogMessage("The report exists. Reading and parsing to HTML...")
                               Dim reportText As String = File.ReadAllText(FilePath)
                               UiThread.RunOnUi(Me, Sub()
                                                        TextBox1.Text = reportText
                                                        TextBox1.Font = New Font(LogFont, LogFontSize, FontStyle.Regular)
                                                    End Sub)

                               ' Convert Markdown report to HTML and add style tags to make the HTML report prettier.
                               Dim prettyHTML As String = "<html>" & CrLf &
                                                          "    <head>" & CrLf &
                                                          "        <meta charset=" & Quote & "utf-8" & Quote & ">" & CrLf &
                                                          "        <title>DISMTools Image Information Report</title>" & CrLf &
                                                          "        <style>" & CrLf &
                                                          "            body {" & CrLf &
                                                          "                font-family: " & Quote & "Segoe UI" & Quote & ", Arial, Verdana, sans-serif;" & CrLf &
                                                          "                display: flex;" & CrLf &
                                                          "                margin: 0;" & CrLf &
                                                          "                height: 100vh;" & CrLf &
                                                          "                overflow: hidden;" & CrLf &
                                                          "                background-color: #FCFBFF;" & CrLf &
                                                          "                color: black;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            table {" & CrLf &
                                                          "                border-collapse: collapse;" & CrLf &
                                                          "                margin-bottom: 20px;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            table th {" & CrLf &
                                                          "                padding: 8px;" & CrLf &
                                                          "                border-bottom: 1px solid #222" & CrLf &
                                                          "            }" & CrLf &
                                                          "            table td {" & CrLf &
                                                          "                padding: 8px;" & CrLf &
                                                          "                border-bottom: 1px solid #222" & CrLf &
                                                          "            }" & CrLf &
                                                          "            code {" & CrLf &
                                                          "                font-family: Inconsolata, " & Quote & "Cascadia Code" & Quote & ", Consolas, " & Quote & "Courier New" & Quote & CrLf &
                                                          "            }" & CrLf &
                                                          "            #sidebar {" & CrLf &
                                                          "                width: 200px;" & CrLf &
                                                          "                border-right: 1px solid #222;" & CrLf &
                                                          "                position: fixed;" & CrLf &
                                                          "                height: 100%;" & CrLf &
                                                          "                overflow-y: auto;" & CrLf &
                                                          "                overflow-x: hidden;" & CrLf &
                                                          "                background-color: white;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #sidebar.shrink {" & CrLf &
                                                          "                width: 50px;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #sidebar.shrink a {" & CrLf &
                                                          "                display: none;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #content {" & CrLf &
                                                          "                margin-left: 200px;" & CrLf &
                                                          "                padding-left: 24px;" & CrLf &
                                                          "                padding-right: 24px;" & CrLf &
                                                          "                overflow: auto;" & CrLf &
                                                          "                width: calc(100% - 50px);" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #content.shrink {" & CrLf &
                                                          "                margin-left: 50px;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #sidebar a {" & CrLf &
                                                          "                text-decoration: none;" & CrLf &
                                                          "                color: black;" & CrLf &
                                                          "                display: block;" & CrLf &
                                                          "                padding: 5px 16px;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #sidebar a:hover {" & CrLf &
                                                          "                background-color: #f0f0f0;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #menu-toggle {" & CrLf &
                                                          "                cursor: pointer;" & CrLf &
                                                          "                padding: 5px 16px;" & CrLf &
                                                          "                background-color: #222;" & CrLf &
                                                          "                color: white;" & CrLf &
                                                          "                margin-bottom: 10px;" & CrLf &
                                                          "                text-align: center;" & CrLf &
                                                          "            }" & CrLf &
                                                          "            #menu-toggle:hover {" & CrLf &
                                                          "                background-color: #333;" & CrLf &
                                                          "            }" & CrLf & CrLf &
                                                          "            @media (prefers-color-sheme: dark) {" & CrLf &
                                                          "                body {" & CrLf &
                                                          "                    background-color: #1F1F1F;" & CrLf &
                                                          "                    color: white;" & CrLf &
                                                          "                }" & CrLf & CrLf &
                                                          "                table th {" & CrLf &
                                                          "                    border-bottom: 1px solid #EEE" & CrLf &
                                                          "                }" & CrLf & CrLf &
                                                          "                table td {" & CrLf &
                                                          "                    border-bottom: 1px solid #EEE" & CrLf &
                                                          "                }" & CrLf & CrLf &
                                                          "                #sidebar {" & CrLf &
                                                          "                    border-right: 1px solid #EEE" & CrLf &
                                                          "                }" & CrLf & CrLf &
                                                          "                #sidebar a {" & CrLf &
                                                          "                    color: white;" & CrLf &
                                                          "                }" & CrLf & CrLf &
                                                          "                #sidebar a:hover {" & CrLf &
                                                          "                    background-color: #282828;" & CrLf &
                                                          "                }" & CrLf &
                                                          "            }" & CrLf &
                                                          "        </style>" & CrLf &
                                                          "    </head>" & CrLf &
                                                          "    <body>" & CrLf &
                                                          "        <div id=" & Quote & "sidebar" & Quote & ">" & CrLf &
                                                          "            <div id=" & Quote & "menu-toggle" & Quote & ">☰</div>" & CrLf &
                                                          "        </div>" & CrLf &
                                                          "        <div id=" & Quote & "content" & Quote & ">" & CrLf &
                                                          "            <!-- Content Goes Here!!! -->" & CrLf &
                                                          "        </div>" & CrLf &
                                                          "        <script>" & CrLf &
                                                          "            document.addEventListener(" & Quote & "DOMContentLoaded" & Quote & ", function() {" & CrLf &
                                                          "                var sidebar = document.getElementById(" & Quote & "sidebar" & Quote & ");" & CrLf &
                                                          "                var content = document.getElementById(" & Quote & "content" & Quote & ");" & CrLf &
                                                          "                var menuToggle = document.getElementById(" & Quote & "menu-toggle" & Quote & ");" & CrLf & CrLf &
                                                          "                menuToggle.addEventListener(" & Quote & "click" & Quote & ", function() {" & CrLf &
                                                          "                    sidebar.classList.toggle(" & Quote & "shrink" & Quote & ");" & CrLf &
                                                          "                    content.classList.toggle(" & Quote & "shrink" & Quote & ");" & CrLf &
                                                          "                });" & CrLf & CrLf &
                                                          "                var headings = content.querySelectorAll(" & Quote & "h2, h4, h5, h6" & Quote & ");" & CrLf & CrLf &
                                                          "                for (var i = 0; i < headings.length - 1; i++) {" & CrLf &
                                                          "                    var heading = headings[i];" & CrLf &
                                                          "                    var link = document.createElement(" & Quote & "a" & Quote & ");" & CrLf &
                                                          "                    link.href = " & Quote & "#" & Quote & " + heading.id;" & CrLf &
                                                          "                    link.textContent = heading.textContent;" & CrLf &
                                                          "                    if (link.textContent.indexOf(" & Quote & "We have ended" & Quote & ") === 0) {" & CrLf &
                                                          "                        return;" & CrLf &
                                                          "                    }" & CrLf &
                                                          "                    if (heading.tagName == " & Quote & "H4" & Quote & ") {" & CrLf &
                                                          "                        link.style.fontSize = " & Quote & "0.75em" & Quote & ";" & CrLf &
                                                          "                    }" & CrLf &
                                                          "                    sidebar.appendChild(link);" & CrLf &
                                                          "                }" & CrLf &
                                                          "            });" & CrLf & CrLf &
                                                          "            window.addEventListener(" & Quote & "resize" & Quote & ", function() {" & CrLf &
                                                          "                var sidebar = document.getElementById(" & Quote & "sidebar" & Quote & ");" & CrLf &
                                                          "                var content = document.getElementById(" & Quote & "content" & Quote & ");" & CrLf & CrLf &
                                                          "                if (window.innerWidth < 680) {" & CrLf &
                                                          "                    sidebar.classList.add(" & Quote & "shrink" & Quote & ");" & CrLf &
                                                          "                    content.classList.add(" & Quote & "shrink" & Quote & ");" & CrLf &
                                                          "                } else {" & CrLf &
                                                          "                    sidebar.classList.remove(" & Quote & "shrink" & Quote & ");" & CrLf &
                                                          "                    content.classList.remove(" & Quote & "shrink" & Quote & ");" & CrLf &
                                                          "                }" & CrLf &
                                                          "            });" & CrLf & CrLf &
                                                          "        </script>" & CrLf & CrLf &
                                                          "    </body>" & CrLf &
                                                          "</html>" & CrLf
                               Try
                                   DynaLog.LogMessage("Parsing to HTML...")
                                   Dim pipeline As MarkdownPipeline = New MarkdownPipelineBuilder().UseAdvancedExtensions().Build()
                                   Dim result As String = Markdown.ToHtml(reportText, pipeline)
                                   DynaLog.LogMessage("Saving to prettier HTML report...")
                                   File.WriteAllText(Application.StartupPath & "\report.html", prettyHTML.Replace("<!-- Content Goes Here!!! -->", result))
                                   Dim reportExists As Boolean = File.Exists(Application.StartupPath & "\report.html")
                                   UiThread.RunOnUi(Me, Sub()
                                                            If reportExists Then
                                                                WebBrowser1.Navigate("file:///" & Application.StartupPath.Replace("\", "/").Trim() & "/report.html")
                                                            End If
                                                            BringToFront()
                                                        End Sub)
                               Catch ex As Exception
                                   DynaLog.LogMessage("Could not convert to HTML. Error message: " & ex.Message)
                                   DynaLog.LogMessage("This could be an issue with Markdig.")
                                   If UiThread.MsgBoxOnUi(Me, LocalizationService.ForSection("InfoSave.Results.Messages").Format("HtmlFailed.Message", ex.Message), vbYesNo + vbCritical, LocalizationService.ForSection("InfoSave.Results.Messages")("ConversionError.Label")) = MsgBoxResult.Yes Then
                                       Process.Start(FilePath)
                                   End If
                                   UiThread.PostToUi(Me, Sub() Close())
                                   Exit Sub
                               End Try
                           Else
                               UiThread.PostToUi(Me, Sub() Close())
                               Exit Sub
                           End If
                       End Sub)

        ExcelTSMI.Enabled = IsExcelAvailable()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        WebBrowser1.Visible = CheckBox1.Checked
    End Sub

    Private Sub InfoSaveResults_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If File.Exists(Application.StartupPath & "\report.html") Then
            DynaLog.LogMessage("Attempting to delete temporary report...")
            Try
                File.Delete(Application.StartupPath & "\report.html")
            Catch ex As Exception
                ' Let something else delete it
            End Try
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        cmsSaveTargets.Show(sender, New Point(0, Button2.Height))
    End Sub

    Private Sub WebBrowser1_Navigating(sender As Object, e As WebBrowserNavigatingEventArgs) Handles WebBrowser1.Navigating
        If e.Url.AbsoluteUri.StartsWith("http", StringComparison.OrdinalIgnoreCase) Or e.Url.AbsoluteUri.StartsWith("ftp", StringComparison.OrdinalIgnoreCase) Then
            DynaLog.LogMessage("An external link has been clicked. Opening it in the default browser...")
            Process.Start(e.Url.AbsoluteUri)
            WebBrowser1.Navigate("file:///" & Application.StartupPath.Replace("\", "/").Trim() & "/report.html")
        End If
    End Sub

    Private Sub ExcelTSMI_Click(sender As Object, e As EventArgs) Handles ExcelTSMI.Click
        If sfdExcelSpreadsheet.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim finalSheetFormat As Excel.XlFileFormat = If(sfdExcelSpreadsheet.FilterIndex = 1, Excel.XlFileFormat.xlOpenXMLWorkbook, Excel.XlFileFormat.xlOpenDocumentSpreadsheet)
            Cursor = Cursors.WaitCursor
            Dim exported As Boolean = SaveReportToExcel(sfdExcelSpreadsheet.FileName, finalSheetFormat)
            Cursor = Cursors.Arrow
            If exported Then
                WindowHelper.DisplayNotificationBalloon(ToolTipIcon.Info, Text, "The report was successfully exported to an Excel spreadsheet.")
            Else
                WindowHelper.DisplayNotificationBalloon(ToolTipIcon.Warning, Text, "The report could not be exported to an Excel spreadsheet.")
            End If
        End If
    End Sub

    Private Function SaveReportToExcel(SpreadsheetPath As String, SpreadsheetFormat As Excel.XlFileFormat) As Boolean
        Dim xlApplication As Excel.Application = Nothing,
            xlWorkbooks As Excel.Workbooks = Nothing,
            xlWorkbook As Excel.Workbook = Nothing,
            xlSheets As Excel.Sheets = Nothing,
            xlSummarySheet As Excel.Worksheet = Nothing,
            xlImageSheet As Excel.Worksheet = Nothing,
            xlPackageSheet As Excel.Worksheet = Nothing,
            xlLastAdditionalSheet As Excel.Worksheet = Nothing
        Dim xlAdditionalSheets As New List(Of Excel.Worksheet)

        Dim exported As Boolean = False

        Try
            xlApplication = New Excel.Application()
            xlApplication.Visible = False
            xlApplication.DisplayAlerts = False

            xlWorkbooks = xlApplication.Workbooks
            xlWorkbook = xlWorkbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet)
            ' By default Excel will save the spreadsheet using the name of the current user as the author; we don't
            ' really want that. We want something sounding more "official".
            xlWorkbook.Author = "CodingWonders Software"
            xlSheets = xlWorkbook.Worksheets
            xlSummarySheet = DirectCast(xlSheets(1), Excel.Worksheet)
            xlSummarySheet.Name = "Information Report Summary"

            ' Every other sheet is added after this one; so set this one as the first sheet
            xlLastAdditionalSheet = xlSummarySheet

            If StoredInformation.ImageList.Count > 0 Then
                xlImageSheet = DirectCast(xlSheets.Add(After:=xlLastAdditionalSheet), Excel.Worksheet)
                xlLastAdditionalSheet = xlImageSheet
            End If
            If StoredInformation.PackageList.Count > 0 Then
                xlPackageSheet = DirectCast(xlSheets.Add(After:=xlLastAdditionalSheet), Excel.Worksheet)
                xlLastAdditionalSheet = xlPackageSheet
            End If

            SetSpannedCell(xlSummarySheet, 1, 1, 2, 6, "DISMTools Image Information Report", CellStyle.Title())
            SetSpannedCell(xlSummarySheet, 1, 7, 2, 9, "This spreadsheet was generated from the obtained image information. Use the sheets at the bottom of the window to explore the sections from which you grabbed information.", wrapText:=True, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter,
                           style:=New CellStyle() With {
                               .BackColor = Color.Gray,
                               .FontColor = Color.White
                           })

            Dim modeStr As String = String.Empty
            Select Case StoredInformation.Mode
                Case ImageInformationMode.WindowsImage : modeStr = "Windows Image"
                Case ImageInformationMode.OnlineInstallation : modeStr = "Active Installation"
                Case ImageInformationMode.OfflineInstallation : modeStr = "Offline Installation"
            End Select
            SetSpannedCell(xlSummarySheet, 5, 2, 1, 5, "Image operation mode:", style:=New CellStyle() With {.Bold = True}, wrapText:=True, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter)
            SetSpannedCell(xlSummarySheet, 6, 2, 1, 5, "Source image path:", style:=New CellStyle() With {.Bold = True}, wrapText:=True, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter)
            SetSpannedCell(xlSummarySheet, 7, 2, 1, 5, "Information retrieval started at:", style:=New CellStyle() With {.Bold = True}, wrapText:=True, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter)
            SetSpannedCell(xlSummarySheet, 8, 2, 1, 5, "Information retrieval ended at:", style:=New CellStyle() With {.Bold = True}, wrapText:=True, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter)
            SetSpannedCell(xlSummarySheet, 5, 7, 1, 9, modeStr, wrapText:=True, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter)
            SetSpannedCell(xlSummarySheet, 6, 7, 1, 9, StoredInformation.InformationSourcePath, wrapText:=True, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter)
            SetSpannedCell(xlSummarySheet, 7, 7, 1, 9, StoredInformation.InformationSnapshotTime.ToString(), wrapText:=True, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignLeft)
            SetSpannedCell(xlSummarySheet, 8, 7, 1, 9, StoredInformation.InformationEndTime.ToString(), wrapText:=True, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignLeft)
            SetSpannedCell(xlSummarySheet, 13, 2, 1, 14, String.Format("Information retrieved by DISMTools version {1} ({2}) ({3}) -- {0}{4}{0}", Quote, My.Application.Info.Version.ToString(), MainForm.dt_codeName, BuildGetter.RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm"), MainForm.SongCode), style:=New CellStyle() With {.Italic = True}, wrapText:=True, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter)
            SetSpannedCell(xlSummarySheet, 14, 2, 1, 14, My.Application.Info.Copyright, style:=New CellStyle() With {.Italic = True}, wrapText:=True, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter)
            SetSpannedCell(xlSummarySheet, 16, 2, 1, 14, "https://codingwonders.github.io", style:=New CellStyle() With {.Italic = True}, wrapText:=True, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter)
            StyleRange(xlSummarySheet, "G7", New CellStyle() With {.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft})
            StyleRange(xlSummarySheet, "G8", New CellStyle() With {.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft})

            If xlImageSheet IsNot Nothing Then PopulateImageInformationSheet(xlImageSheet)
            If xlPackageSheet IsNot Nothing Then PopulatePackageInformationSheet(xlPackageSheet)
            If StoredInformation.FeatureList.Count > 0 Then xlLastAdditionalSheet = AddFeatureInformationSheet(xlSheets, xlLastAdditionalSheet, xlAdditionalSheets)
            If StoredInformation.AppxList.Count > 0 Then xlLastAdditionalSheet = AddAppxInformationSheet(xlSheets, xlLastAdditionalSheet, xlAdditionalSheets)
            If StoredInformation.CapabilityList.Count > 0 Then xlLastAdditionalSheet = AddCapabilityInformationSheet(xlSheets, xlLastAdditionalSheet, xlAdditionalSheets)
            If StoredInformation.DriverList.Count > 0 Then xlLastAdditionalSheet = AddDriverInformationSheet(xlSheets, xlLastAdditionalSheet, xlAdditionalSheets)
            If StoredInformation.ServiceList.Count > 0 Then xlLastAdditionalSheet = AddServiceInformationSheet(xlSheets, xlLastAdditionalSheet, xlAdditionalSheets)
            If StoredInformation.PackageFileList.Count > 0 Then xlLastAdditionalSheet = AddPackageFileInformationSheet(xlSheets, xlLastAdditionalSheet, xlAdditionalSheets)
            If StoredInformation.DriverFileList.Count > 0 Then xlLastAdditionalSheet = AddDriverFileInformationSheet(xlSheets, xlLastAdditionalSheet, xlAdditionalSheets)

            ' The first sheet is the default one; activate it.
            xlSummarySheet.Activate()

            Try
                xlWorkbook.SaveAs(Filename:=SpreadsheetPath, FileFormat:=SpreadsheetFormat)
            Catch comEx As COMException
                Dim canExport As Boolean = False
                Do Until canExport
                    MessageBox.Show(Me, "The Excel spreadsheet could not be saved to the specified file because it either is in use or you don't have permissions. Close the file and try again." & CrLf & CrLf & "You can also save it to another location.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    If sfdExcelSpreadsheet.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        SpreadsheetPath = sfdExcelSpreadsheet.FileName

                        Try
                            xlWorkbook.SaveAs(Filename:=SpreadsheetPath, FileFormat:=SpreadsheetFormat)
                            canExport = True
                        Catch ex As COMException
                            canExport = False
                        End Try
                    End If
                Loop
            End Try
            xlWorkbook.Close(False)
            exported = True
        Catch ex As COMException
            DynaLog.LogMessage("Failed to export the image information report to Excel: " & ex.GetType().Name & " - " & Hex(ex.ErrorCode))
            MessageBox.Show(Me, "The Excel spreadsheet could not be saved to the specified file: " & ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If xlApplication IsNot Nothing Then
                xlApplication.DisplayAlerts = True
                xlApplication.Quit()
            End If

            For Each additionalSheet As Excel.Worksheet In xlAdditionalSheets
                ReleaseCom(additionalSheet)
            Next
            ReleaseCom(xlPackageSheet)
            ReleaseCom(xlImageSheet)
            ReleaseCom(xlSummarySheet)
            ReleaseCom(xlSheets)
            ReleaseCom(xlWorkbook)
            ReleaseCom(xlWorkbooks)
            ReleaseCom(xlApplication)
        End Try

        Return exported
    End Function

    Private Sub WriteSheetTitle(sheet As Excel.Worksheet, Subtitle As String)
        If sheet Is Nothing Then Exit Sub

        sheet.Name = Subtitle
        SetSpannedCell(sheet, 1, 1, 2, 6, "DISMTools Image Information Report", CellStyle.Title())
        SetSpannedCell(sheet, 1, 7, 2, 12, Subtitle, style:=New CellStyle() With {.Bold = True, .Italic = True, .BackColor = Color.Gray, .FontColor = Color.White}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter)
    End Sub

    Private Sub PopulateImageInformationSheet(sheet As Excel.Worksheet)
        WriteSheetTitle(sheet, "Image File Information")
        SetSpannedCell(sheet, 5, 2, 1, 10, String.Format("INFORMATION SUMMARY FOR {0} IMAGE(S):", StoredInformation.ImageList.Count), style:=New CellStyle() With {.Bold = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter)

        Dim headers As String() = {"#", "Image Name", "Image Description", "Image Size (BYTES)", "Measured Size", "Version", "Architecture", "HAL", "SP Build", "SP Level", "Installation Type", "Edition", "Product Type", "Product Suite", "System Root Directory", "Languages", "Creation Date", "Modification Date"}
        Dim imageCount As Integer = StoredInformation.ImageList.Count
        Dim values(imageCount, headers.Length - 1) As Object
        For column As Integer = 0 To headers.Length - 1
            values(0, column) = headers(column)
        Next

        For imageIndex As Integer = 0 To imageCount - 1
            Dim imageInfo As DismImageInfo = StoredInformation.ImageList(imageIndex)
            Dim row As Integer = imageIndex + 1
            Dim languages As New List(Of String)
            If imageInfo.Languages IsNot Nothing Then
                For Each language In imageInfo.Languages
                    languages.Add(language.DisplayName & If(imageInfo.DefaultLanguage IsNot Nothing AndAlso imageInfo.DefaultLanguage.Name = language.Name, " (default)", String.Empty))
                Next
            End If

            values(row, 0) = imageIndex + 1
            values(row, 1) = imageInfo.ImageName
            values(row, 2) = imageInfo.ImageDescription
            values(row, 3) = imageInfo.ImageSize
            values(row, 4) = "~" & Converters.BytesToReadableSize(imageInfo.ImageSize)
            values(row, 5) = imageInfo.ProductVersion.ToString()
            values(row, 6) = Casters.CastDismArchitecture(imageInfo.Architecture)
            values(row, 7) = If(String.IsNullOrEmpty(imageInfo.Hal), "Undefined by the image", imageInfo.Hal)
            values(row, 8) = imageInfo.ProductVersion.Revision
            values(row, 9) = imageInfo.SpLevel
            values(row, 10) = imageInfo.InstallationType
            values(row, 11) = imageInfo.EditionId
            values(row, 12) = imageInfo.ProductType
            values(row, 13) = imageInfo.ProductSuite
            values(row, 14) = imageInfo.SystemRoot
            values(row, 15) = If(languages.Count > 0, String.Join(Environment.NewLine, languages), "None")
            If imageInfo.CustomizedInfo IsNot Nothing Then
                values(row, 16) = imageInfo.CustomizedInfo.CreatedTime.ToString(currentOSCulture.DateTimeFormat.FullDateTimePattern, currentOSCulture)
                values(row, 17) = imageInfo.CustomizedInfo.ModifiedTime.ToString(currentOSCulture.DateTimeFormat.FullDateTimePattern, currentOSCulture)
            End If
        Next

        SetRangeValues(sheet, "B7", values, CellStyle.Bordered(), wrapText:=True)
        StyleRange(sheet, "B7:S7", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30})
        StyleRange(sheet, "B:S", New CellStyle() With {.ColumnWidth = 20, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "B:B", New CellStyle() With {.ColumnWidth = 8, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "C:D", New CellStyle() With {.ColumnWidth = 28, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "E:F", New CellStyle() With {.ColumnWidth = 18, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "H:I", New CellStyle() With {.ColumnWidth = 22, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "J:K", New CellStyle() With {.ColumnWidth = 12, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "Q:Q", New CellStyle() With {.ColumnWidth = 36, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "R:S", New CellStyle() With {.ColumnWidth = 22, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})

        Dim toolsRow As Integer = Math.Max(10, imageCount + 9)
        SetSpannedCell(sheet, toolsRow, 2, 1, 1, "TOOLS:", style:=New CellStyle() With {.Bold = True})
        SetCellValue(sheet, toolsRow + 3, 3, "Count based on Edition ID:", New CellStyle() With {.Bold = True})

        Dim editionIds As New List(Of String)
        For Each imageInfo In StoredInformation.ImageList
            Dim editionId As String = If(imageInfo.EditionId, String.Empty)
            If editionId.Length > 0 AndAlso Not editionIds.Contains(editionId) Then
                editionIds.Add(editionId)
            End If
        Next

        Dim toolStartRow As Integer = toolsRow + 5
        Dim lastImageRow As Integer = imageCount + 7
        For index As Integer = 0 To editionIds.Count - 1
            Dim row As Integer = toolStartRow + index
            SetCellValue(sheet, row, 3, "Edition ID:")
            SetCellValue(sheet, row, 4, editionIds(index))
            SetNonLocalizedFormula(sheet, ToAddress(row, 5), String.Format("=COUNTIF($M$8:$M${0},D{1})", lastImageRow, row))
        Next
        If editionIds.Count = 0 Then
            SetCellValue(sheet, toolStartRow, 3, "No edition IDs were found in the image information.")
        End If
        SetCellValue(sheet, toolStartRow + Math.Max(editionIds.Count, 1) + 1, 3, "Add Edition IDs below and copy the adjacent formula to count additional values.")
    End Sub

    Private Sub PopulatePackageInformationSheet(sheet As Excel.Worksheet)
        WriteSheetTitle(sheet, "Package Information")
        SetSpannedCell(sheet, 5, 2, 1, 10, String.Format("INFORMATION SUMMARY FOR {0} PACKAGE(S):", StoredInformation.PackageList.Count), style:=New CellStyle() With {.Bold = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter)

        Dim headers As String() = {"Package name", "Applicable?", "Copyright", "Company", "Creation time", "Description", "Install client", "Install package name", "Install time", "Last update time", "Display name", "Product name", "Product version", "Release type", "Restart required?", "Support information", "Package state", "Boot up required?", "Capability identity", "Custom properties", "Features"}
        Dim packageCount As Integer = StoredInformation.PackageList.Count
        Dim values(packageCount, headers.Length - 1) As Object
        For column As Integer = 0 To headers.Length - 1
            values(0, column) = headers(column)
        Next

        For packageIndex As Integer = 0 To packageCount - 1
            Dim packageInfo As ImagePackageEX = StoredInformation.PackageList(packageIndex)
            Dim row As Integer = packageIndex + 1
            Dim customProperties As New List(Of String)
            If packageInfo.PackageCustomProperties IsNot Nothing Then
                For Each customProperty In packageInfo.PackageCustomProperties
                    customProperties.Add(If(String.IsNullOrEmpty(customProperty.Path), String.Empty, customProperty.Path & "\") & customProperty.Name & ": " & customProperty.Value)
                Next
            End If

            Dim packageFeatures As New List(Of String)
            If packageInfo.PackageFeatures IsNot Nothing Then
                For Each packageFeature In packageInfo.PackageFeatures
                    packageFeatures.Add(packageFeature.FeatureName & " (" & Casters.CastDismFeatureState(packageFeature.State) & ")")
                Next
            End If

            values(row, 0) = packageInfo.PackageName
            values(row, 8) = FormatReportDate(packageInfo.PackageInstallTime)
            values(row, 13) = Casters.CastDismReleaseType(packageInfo.PackageReleaseType)
            values(row, 16) = Casters.CastDismPackageState(packageInfo.PackageState)
            If StoredInformation.EveryPackageObtained Then
                values(row, 1) = Casters.CastDismApplicabilityStatus(packageInfo.PackageApplicable)
                values(row, 2) = packageInfo.PackageCopyright
                values(row, 3) = packageInfo.PackageCompany
                values(row, 4) = FormatReportDate(packageInfo.PackageCreationTime)
                values(row, 5) = packageInfo.PackageDescription
                values(row, 6) = packageInfo.PackageInstallClient
                values(row, 7) = packageInfo.PackageInstallPackageName
                values(row, 9) = FormatReportDate(packageInfo.PackageLastUpdateTime)
                values(row, 10) = packageInfo.PackageDisplayName
                values(row, 11) = packageInfo.PackageProductName
                values(row, 12) = packageInfo.PackageProductVersion
                values(row, 14) = Casters.CastDismRestartType(packageInfo.PackageRestartRequired)
                values(row, 15) = packageInfo.PackageSupportInformation
                values(row, 17) = Casters.CastDismFullyOfflineInstallationType(packageInfo.PackageFullyOffline)
                values(row, 18) = packageInfo.PackageCapabilityId
                values(row, 19) = If(customProperties.Count > 0, String.Join(Environment.NewLine, customProperties), "None")
                values(row, 20) = If(packageFeatures.Count > 0, String.Join(Environment.NewLine, packageFeatures), "None")
            Else
                For column As Integer = 1 To headers.Length - 1
                    If values(row, column) Is Nothing Then values(row, column) = "Not collected"
                Next
            End If
        Next

        SetRangeValues(sheet, "B7", values, CellStyle.Bordered(), wrapText:=True)
        StyleRange(sheet, "B7:V7", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30})
        StyleRange(sheet, "B:V", New CellStyle() With {.ColumnWidth = 24, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "B:B", New CellStyle() With {.ColumnWidth = 60, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "C:C", New CellStyle() With {.ColumnWidth = 12, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "D:D", New CellStyle() With {.ColumnWidth = 54, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "F:F", New CellStyle() With {.ColumnWidth = 40, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "G:G", New CellStyle() With {.ColumnWidth = 40, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "I:I", New CellStyle() With {.ColumnWidth = 64, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "L:M", New CellStyle() With {.ColumnWidth = 24, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "P:P", New CellStyle() With {.ColumnWidth = 24, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "Q:Q", New CellStyle() With {.ColumnWidth = 40, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "S:V", New CellStyle() With {.ColumnWidth = 32, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})

        SetSpannedCell(sheet, 5, 24, 1, 2, "Packages by release type", style:=New CellStyle() With {.Italic = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter)
        SetCellValue(sheet, "X7", "Release type", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})
        SetCellValue(sheet, "Y7", "Count", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})
        Dim releaseTypes As New List(Of String)
        For Each packageInfo In StoredInformation.PackageList
            Dim releaseType As String = Casters.CastDismReleaseType(packageInfo.PackageReleaseType)
            If Not releaseTypes.Contains(releaseType) Then
                releaseTypes.Add(releaseType)
            End If
        Next

        Dim lastPackageRow As Integer = packageCount + 7
        For index As Integer = 0 To releaseTypes.Count - 1
            Dim row As Integer = index + 8
            SetCellValue(sheet, row, 24, releaseTypes(index))
            SetNonLocalizedFormula(sheet, ToAddress(row, 25), String.Format("=COUNTIF($O$8:$O${0},X{1})", lastPackageRow, row))
        Next
        If releaseTypes.Count = 0 Then
            SetCellValue(sheet, "X8", "No package information was collected.")
            StyleRange(sheet, "X8:Y8", New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        Else
            StyleRange(sheet, String.Format("X8:Y{0}", 8 + releaseTypes.Count - 1), New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft})
            Dim chart As Excel.ChartObject = Nothing
            Try
                chart = AddChart(sheet, "X7:Y" & (releaseTypes.Count + 7).ToString(), "AA7:AH9", New ChartOptions() With {.ChartType = Excel.XlChartType.xlDoughnut, .Title = "Packages by release type"})
            Finally
                ReleaseCom(chart)
            End Try
        End If
    End Sub

    Private Function AddFeatureInformationSheet(sheets As Excel.Sheets, afterSheet As Excel.Worksheet, ownedSheets As List(Of Excel.Worksheet)) As Excel.Worksheet
        Dim sheet As Excel.Worksheet = If(afterSheet IsNot Nothing, DirectCast(sheets.Add(After:=afterSheet), Excel.Worksheet), DirectCast(sheets.Add(), Excel.Worksheet))
        ownedSheets.Add(sheet)
        WriteSheetTitle(sheet, "Feature Information")
        SetSpannedCell(sheet, 5, 2, 1, 6, String.Format("INFORMATION SUMMARY FOR {0} FEATURE(S):", StoredInformation.FeatureList.Count), style:=New CellStyle() With {.Bold = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignCenter)

        Dim headers As String() = {"Feature name", "Display name", "Description", "Feature state", "Restart required?", "Custom properties"}
        Dim featureCount As Integer = StoredInformation.FeatureList.Count
        Dim values(featureCount, headers.Length - 1) As Object
        For column As Integer = 0 To headers.Length - 1
            values(0, column) = headers(column)
        Next

        For featureIndex = 0 To featureCount - 1
            Dim feature As ImageFeatureEX = StoredInformation.FeatureList(featureIndex)
            Dim row As Integer = featureIndex + 1
            values(row, 0) = feature.FeatureName
            values(row, 3) = Casters.CastDismFeatureState(feature.FeatureState)
            values(row, 1) = "Not collected"
            values(row, 2) = "Not collected"
            values(row, 4) = "Not collected"
            values(row, 5) = "Not collected"
            If StoredInformation.EveryFeatureObtained Then
                values(row, 1) = feature.FeatureDisplayName
                values(row, 2) = feature.FeatureDescription
                values(row, 4) = Casters.CastDismRestartType(feature.FeatureRestartRequired)
                values(row, 5) = FormatCustomProperties(feature.FeatureCustomProperties)
            End If
        Next

        SetRangeValues(sheet, "B7", values, CellStyle.Bordered(), wrapText:=True)
        StyleRange(sheet, "B7:G7", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30})
        StyleRange(sheet, "B:G", New CellStyle() With {.ColumnWidth = 28, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "B:B", New CellStyle() With {.ColumnWidth = 32, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "C:D", New CellStyle() With {.ColumnWidth = 36, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "F:F", New CellStyle() With {.ColumnWidth = 32, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "G:G", New CellStyle() With {.ColumnWidth = 32, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})

        SetSpannedCell(sheet, 5, 9, 1, 2, "Features by state", style:=New CellStyle() With {.Italic = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignLeft)
        SetCellValue(sheet, "I7", "State", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})
        SetCellValue(sheet, "J7", "Count", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})

        Dim states As New List(Of String)
        For Each feature In StoredInformation.FeatureList
            Dim state As String = Casters.CastDismFeatureState(feature.FeatureState)
            If Not states.Contains(state) Then
                states.Add(state)
            End If
        Next

        Dim lastFeatureRow As Integer = featureCount + 7
        For index As Integer = 0 To states.Count - 1
            Dim row As Integer = index + 8
            SetCellValue(sheet, row, 9, states(index))
            SetNonLocalizedFormula(sheet, ToAddress(row, 10), String.Format("=COUNTIF($E$8:$E${0},I{1})", lastFeatureRow, row))
        Next
        If states.Count = 0 Then
            SetCellValue(sheet, "I8", "No feature information was collected.")
            StyleRange(sheet, "I8:J8", New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        Else
            StyleRange(sheet, String.Format("I8:J{0}", 8 + states.Count - 1), New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft})
        End If

        Return sheet
    End Function

    Private Function AddAppxInformationSheet(sheets As Excel.Sheets, afterSheet As Excel.Worksheet, ownedSheets As List(Of Excel.Worksheet)) As Excel.Worksheet
        Dim sheet As Excel.Worksheet = If(afterSheet IsNot Nothing, DirectCast(sheets.Add(After:=afterSheet), Excel.Worksheet), DirectCast(sheets.Add(), Excel.Worksheet))
        ownedSheets.Add(sheet)
        WriteSheetTitle(sheet, "AppX Package Information")
        SetSpannedCell(sheet, 5, 2, 1, 11, String.Format("INFORMATION SUMMARY FOR {0} APPX PACKAGE(S):", StoredInformation.AppxList.Count), style:=New CellStyle() With {.Bold = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignCenter)

        Dim headers As String() = {"Package name", "Package full name", "Architecture", "Resource ID", "Version", "Display name", "Registration status", "Install location", "Manifest location", "Store logo asset directory", "Main store logo asset"}
        Dim appxCount As Integer = StoredInformation.AppxList.Count
        Dim values(appxCount, headers.Length - 1) As Object
        For column As Integer = 0 To headers.Length - 1
            values(0, column) = headers(column)
        Next

        For appxIndex As Integer = 0 To appxCount - 1
            Dim packageInfo As ImageAppxPackageEX = StoredInformation.AppxList(appxIndex)
            Dim row As Integer = appxIndex + 1
            values(row, 0) = packageInfo.PackageName
            values(row, 1) = If(packageInfo.PackageFullName <> "", packageInfo.PackageFullName, packageInfo.PackageName)
            values(row, 2) = Casters.CastDismArchitecture(packageInfo.PackageArchitecture)
            values(row, 3) = packageInfo.PackageResourceId
            values(row, 4) = If(packageInfo.PackageVersion Is Nothing, String.Empty, packageInfo.PackageVersion.ToString())
            values(row, 5) = packageInfo.PackageDisplayName
            values(row, 6) = packageInfo.PackageRegistrationStatus
            values(row, 7) = packageInfo.PackageInstallLocation
            values(row, 8) = packageInfo.PackageManifestLocation
            values(row, 9) = packageInfo.PackageStoreLogoAssetDirectory
            values(row, 10) = packageInfo.PackageMainStoreLogoAsset
        Next

        SetRangeValues(sheet, "B7", values, CellStyle.Bordered(), wrapText:=True)
        StyleRange(sheet, "B7:L7", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30})
        StyleRange(sheet, "B:L", New CellStyle() With {.ColumnWidth = 28, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "B:B", New CellStyle() With {.ColumnWidth = 40, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "C:C", New CellStyle() With {.ColumnWidth = 52, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "G:G", New CellStyle() With {.ColumnWidth = 28, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "H:K", New CellStyle() With {.ColumnWidth = 36, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "L:L", New CellStyle() With {.ColumnWidth = 40, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})

        SetSpannedCell(sheet, 5, 14, 1, 2, "AppX packages by registration", style:=New CellStyle() With {.Italic = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignLeft)
        SetCellValue(sheet, "N7", "Registration status", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})
        SetCellValue(sheet, "O7", "Count", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})

        Dim registrationStatuses As New List(Of String)
        For Each packageInfo In StoredInformation.AppxList
            Dim status As String = packageInfo.PackageRegistrationStatus
            If Not registrationStatuses.Contains(status) Then
                registrationStatuses.Add(status)
            End If
        Next

        Dim lastAppxRow As Integer = appxCount + 7
        For index As Integer = 0 To registrationStatuses.Count - 1
            Dim row As Integer = index + 8
            SetCellValue(sheet, row, 14, registrationStatuses(index))
            SetNonLocalizedFormula(sheet, ToAddress(row, 15), String.Format("=COUNTIF($G$8:$G${0},N{1})", lastAppxRow, row))
        Next
        If registrationStatuses.Count = 0 Then
            SetCellValue(sheet, "N8", "No AppX package information was collected.")
            StyleRange(sheet, "N8:O8", New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        Else
            StyleRange(sheet, String.Format("N8:O{0}", 8 + registrationStatuses.Count - 1), New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft})
        End If

        Return sheet
    End Function

    Private Function AddCapabilityInformationSheet(sheets As Excel.Sheets, afterSheet As Excel.Worksheet, ownedSheets As List(Of Excel.Worksheet)) As Excel.Worksheet
        Dim sheet As Excel.Worksheet = If(afterSheet IsNot Nothing, DirectCast(sheets.Add(After:=afterSheet), Excel.Worksheet), DirectCast(sheets.Add(), Excel.Worksheet))
        ownedSheets.Add(sheet)
        WriteSheetTitle(sheet, "Capability Information")
        SetSpannedCell(sheet, 5, 2, 1, 7, String.Format("INFORMATION SUMMARY FOR {0} CAPABILIT(Y/IES):", StoredInformation.CapabilityList.Count), style:=New CellStyle() With {.Bold = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignCenter)

        Dim headers As String() = {"Capability identity", "Capability name", "State", "Display name", "Description", "Download size (bytes)", "Install size (bytes)"}
        Dim capabilityCount As Integer = StoredInformation.CapabilityList.Count
        Dim values(capabilityCount, headers.Length - 1) As Object
        For column As Integer = 0 To headers.Length - 1
            values(0, column) = headers(column)
        Next

        For capabilityIndex As Integer = 0 To capabilityCount - 1
            Dim capability As ImageCapabilityEX = StoredInformation.CapabilityList(capabilityIndex)
            Dim row As Integer = capabilityIndex + 1
            values(row, 0) = capability.CapabilityName
            values(row, 1) = CapabilityShortName(capability.CapabilityName)
            values(row, 2) = Casters.CastDismPackageState(capability.CapabilityState)
            values(row, 3) = "Not collected"
            values(row, 4) = "Not collected"
            values(row, 5) = "Not collected"
            values(row, 6) = "Not collected"
            If StoredInformation.EveryCapabilityObtained Then
                values(row, 3) = capability.CapabilityDisplayName
                values(row, 4) = capability.CapabilityDescription
                values(row, 5) = capability.CapabilityDownloadSize
                values(row, 6) = capability.CapabilityInstallSize
            End If
        Next

        SetRangeValues(sheet, "B7", values, CellStyle.Bordered(), wrapText:=True)
        StyleRange(sheet, "B7:H7", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30})
        StyleRange(sheet, "B:H", New CellStyle() With {.ColumnWidth = 28, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "B:B", New CellStyle() With {.ColumnWidth = 48, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "C:D", New CellStyle() With {.ColumnWidth = 32, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "E:F", New CellStyle() With {.ColumnWidth = 36, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "G:H", New CellStyle() With {.ColumnWidth = 28, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})

        SetSpannedCell(sheet, 5, 10, 1, 2, "Capabilities by state", style:=New CellStyle() With {.Italic = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignLeft)
        SetCellValue(sheet, "J7", "State", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})
        SetCellValue(sheet, "K7", "Count", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})

        Dim states As New List(Of String)
        For Each capability In StoredInformation.CapabilityList
            Dim state As String = Casters.CastDismPackageState(capability.CapabilityState)
            If Not states.Contains(state) Then
                states.Add(state)
            End If
        Next

        Dim lastCapabilityRow As Integer = capabilityCount + 7
        For index As Integer = 0 To states.Count - 1
            Dim row As Integer = index + 8
            SetCellValue(sheet, row, 10, states(index))
            SetNonLocalizedFormula(sheet, ToAddress(row, 11), String.Format("=COUNTIF($D$8:$D${0},J{1})", lastCapabilityRow, row))
        Next
        If states.Count = 0 Then
            SetCellValue(sheet, "J8", "No capability information was collected.")
            StyleRange(sheet, "J8:K8", New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        Else
            StyleRange(sheet, String.Format("J8:K{0}", 8 + states.Count - 1), New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft})
        End If

        Return sheet
    End Function

    Private Function AddDriverInformationSheet(sheets As Excel.Sheets, afterSheet As Excel.Worksheet, ownedSheets As List(Of Excel.Worksheet)) As Excel.Worksheet
        Dim sheet As Excel.Worksheet = If(afterSheet IsNot Nothing, DirectCast(sheets.Add(After:=afterSheet), Excel.Worksheet), DirectCast(sheets.Add(), Excel.Worksheet))
        ownedSheets.Add(sheet)
        WriteSheetTitle(sheet, "Driver Information")
        SetSpannedCell(sheet, 5, 2, 1, 12, String.Format("INFORMATION SUMMARY FOR {0} DRIVER(S):", StoredInformation.DriverList.Count), style:=New CellStyle() With {.Bold = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignCenter)

        Dim headers As String() = {"Published name", "Original file name", "Provider", "Class", "Class description", "Class GUID", "Catalog file", "Inbox?", "Boot critical?", "Version", "Date", "Signature status"}
        Dim driverCount As Integer = StoredInformation.DriverList.Count
        Dim values(driverCount, headers.Length - 1) As Object
        For column As Integer = 0 To headers.Length - 1
            values(0, column) = headers(column)
        Next

        For driverIndex As Integer = 0 To driverCount - 1
            Dim driver As ImageDriverEX = StoredInformation.DriverList(driverIndex)
            Dim row As Integer = driverIndex + 1
            values(row, 0) = driver.DriverPublishedName
            values(row, 1) = driver.DriverOriginalFileName
            values(row, 2) = driver.DriverProviderName
            values(row, 3) = driver.DriverClassName
            values(row, 4) = "Not collected"
            values(row, 5) = "Not collected"
            values(row, 6) = "Not collected"
            values(row, 7) = If(driver.DriverInbox, "Yes", "No")
            values(row, 8) = "Not collected"
            values(row, 9) = If(driver.DriverVersion Is Nothing, String.Empty, driver.DriverVersion.ToString())
            values(row, 10) = driver.DriverDate
            values(row, 11) = "Not collected"
            If StoredInformation.EveryDriverObtained Then
                values(row, 4) = driver.DriverClassDescription
                values(row, 5) = driver.DriverClassGuid
                values(row, 6) = driver.DriverCatalogFile
                values(row, 8) = If(driver.DriverBootCritical, "Yes", "No")
                values(row, 11) = Casters.CastSignatureStatus(driver.DriverSignature)
            End If
        Next

        SetRangeValues(sheet, "B7", values, CellStyle.Bordered(), wrapText:=True)
        StyleRange(sheet, "B7:M7", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30})
        StyleRange(sheet, "B:M", New CellStyle() With {.ColumnWidth = 28, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "B:B", New CellStyle() With {.ColumnWidth = 40, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "C:C", New CellStyle() With {.ColumnWidth = 36, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "E:F", New CellStyle() With {.ColumnWidth = 32, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "G:G", New CellStyle() With {.ColumnWidth = 28, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "H:I", New CellStyle() With {.ColumnWidth = 16, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "J:K", New CellStyle() With {.ColumnWidth = 20, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "M:M", New CellStyle() With {.ColumnWidth = 28, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})

        SetSpannedCell(sheet, 5, 15, 1, 2, "Drivers by class", style:=New CellStyle() With {.Italic = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignLeft)
        SetCellValue(sheet, "O7", "Class", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})
        SetCellValue(sheet, "P7", "Count", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})

        Dim classes As New List(Of String)
        For Each driver In StoredInformation.DriverList
            Dim driverClass As String = driver.DriverClassName
            If Not classes.Contains(driverClass) Then
                classes.Add(driverClass)
            End If
        Next

        Dim lastDriverRow As Integer = driverCount + 7
        For index As Integer = 0 To classes.Count - 1
            Dim row As Integer = index + 8
            SetCellValue(sheet, row, 15, classes(index))
            SetNonLocalizedFormula(sheet, ToAddress(row, 16), String.Format("=COUNTIF($E$8:$E${0},O{1})", lastDriverRow, row))
        Next
        If classes.Count = 0 Then
            SetCellValue(sheet, "O8", "No driver information was collected.")
            StyleRange(sheet, "O8:P8", New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        Else
            StyleRange(sheet, String.Format("O8:P{0}", 8 + classes.Count - 1), New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft})
        End If

        Return sheet
    End Function

    Private Function AddServiceInformationSheet(sheets As Excel.Sheets, afterSheet As Excel.Worksheet, ownedSheets As List(Of Excel.Worksheet)) As Excel.Worksheet
        Dim sheet As Excel.Worksheet = If(afterSheet IsNot Nothing, DirectCast(sheets.Add(After:=afterSheet), Excel.Worksheet), DirectCast(sheets.Add(), Excel.Worksheet))
        ownedSheets.Add(sheet)
        WriteSheetTitle(sheet, "Service Information")
        SetSpannedCell(sheet, 5, 2, 1, 15, String.Format("INFORMATION SUMMARY FOR {0} SERVICE(S):", StoredInformation.ServiceList.Count), style:=New CellStyle() With {.Bold = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignCenter)

        Dim headers As String() = {"Service name", "Display name", "Description", "Account", "Image path", "Group", "Start type", "Service type", "Error control", "Dependencies", "Required privileges", "User service flags", "Safe mode (minimal)", "Safe mode (network)", "Failure actions"}
        Dim serviceCount As Integer = StoredInformation.ServiceList.Count
        Dim values(serviceCount, headers.Length - 1) As Object
        For column As Integer = 0 To headers.Length - 1
            values(0, column) = headers(column)
        Next

        Dim refSvc As New WindowsService()
        Dim serviceStartTypes As New List(Of String)
        If serviceCount > 0 Then
            For Each StartType As WindowsService.ServiceStartType In [Enum].GetValues(GetType(WindowsService.ServiceStartType))
                refSvc.StartType = StartType
                serviceStartTypes.Add(refSvc.StartTypeToString())
                If StartType = WindowsService.ServiceStartType.Automatic Then
                    refSvc.DelayedStart = True
                    serviceStartTypes.Add(refSvc.StartTypeToString())
                End If
            Next
        End If

        For serviceIndex As Integer = 0 To serviceCount - 1
            Dim service As WindowsService = StoredInformation.ServiceList(serviceIndex)
            Dim row As Integer = serviceIndex + 1
            values(row, 0) = service.Name
            values(row, 1) = service.DisplayName
            values(row, 2) = service.Description
            values(row, 3) = service.ObjectName
            values(row, 4) = service.ImagePath
            values(row, 5) = service.Group
            values(row, 6) = service.StartTypeToString()
            values(row, 7) = service.Type.ToString()
            values(row, 8) = service.ErrorControl.ToString()
            values(row, 9) = If(service.Dependencies Is Nothing, String.Empty, String.Join(Environment.NewLine, service.Dependencies))
            values(row, 10) = If(service.RequiredPrivileges Is Nothing, String.Empty, String.Join(Environment.NewLine, service.RequiredPrivileges.Select(Function(privilege) privilege.ToString())))
            values(row, 11) = service.UserServiceFlags
            values(row, 12) = If(service.SafeModeOptions IsNot Nothing AndAlso service.SafeModeOptions.AvailableInMinimalSafeBoot, "Yes", "No")
            values(row, 13) = If(service.SafeModeOptions IsNot Nothing AndAlso service.SafeModeOptions.AvailableInNetworkSafeBoot, "Yes", "No")
            values(row, 14) = FormatServiceFailureActions(service.FailureActions)
        Next

        SetRangeValues(sheet, "B7", values, CellStyle.Bordered(), wrapText:=True)
        StyleRange(sheet, "B7:P7", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30})
        StyleRange(sheet, "B:R", New CellStyle() With {.ColumnWidth = 24, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "B:B", New CellStyle() With {.ColumnWidth = 32, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "C:D", New CellStyle() With {.ColumnWidth = 40, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "E:F", New CellStyle() With {.ColumnWidth = 36, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "G:H", New CellStyle() With {.ColumnWidth = 18, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "K:L", New CellStyle() With {.ColumnWidth = 40, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "P:P", New CellStyle() With {.ColumnWidth = 48, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})

        SetSpannedCell(sheet, 5, 18, 1, 2, "Services by start type", style:=New CellStyle() With {.Italic = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignLeft)
        SetCellValue(sheet, "R7", "Start type", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})
        SetCellValue(sheet, "S7", "Count", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})

        Dim lastServiceRow As Integer = serviceCount + 7
        For i = 0 To serviceStartTypes.Count - 1
            Dim row As Integer = i + 8
            SetCellValue(sheet, row, 18, serviceStartTypes(i))
            SetNonLocalizedFormula(sheet, ToAddress(row, 19), String.Format("=COUNTIF($H$8:$H${0},R{1})", lastServiceRow, row))
        Next
        If serviceStartTypes.Count = 0 Then
            SetCellValue(sheet, "R8", "No service information was collected.")
            StyleRange(sheet, "R8:S8", New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        Else
            StyleRange(sheet, String.Format("R8:S{0}", 8 + serviceStartTypes.Count - 1), New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        End If

        Return sheet
    End Function

    Private Function AddPackageFileInformationSheet(sheets As Excel.Sheets, afterSheet As Excel.Worksheet, ownedSheets As List(Of Excel.Worksheet)) As Excel.Worksheet
        Dim sheet As Excel.Worksheet = If(afterSheet IsNot Nothing, DirectCast(sheets.Add(After:=afterSheet), Excel.Worksheet), DirectCast(sheets.Add(), Excel.Worksheet))
        ownedSheets.Add(sheet)
        WriteSheetTitle(sheet, "Package File Information")
        SetSpannedCell(sheet, 5, 2, 1, 22, String.Format("INFORMATION SUMMARY FOR {0} PACKAGE FILE(S):", StoredInformation.PackageFileList.Count), style:=New CellStyle() With {.Bold = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignCenter)

        Dim headers As String() = {"Package file path", "Package name", "Applicable?", "Company", "Creation time", "Description", "Install client", "Install package name", "Install time", "Last update time", "Display name", "Product name", "Product version", "Release type", "Restart required?", "Support information", "Package state", "Boot up required?", "Capability identity", "Custom properties", "Features", "Copyright"}
        Dim packageFileCount As Integer = StoredInformation.PackageFileList.Count
        Dim values(packageFileCount, headers.Length - 1) As Object
        For column As Integer = 0 To headers.Length - 1
            values(0, column) = headers(column)
        Next

        For packageFileIndex As Integer = 0 To packageFileCount - 1
            Dim packageInfo As ImagePackageFile = StoredInformation.PackageFileList(packageFileIndex)
            Dim row As Integer = packageFileIndex + 1
            values(row, 0) = packageInfo.FilePath
            values(row, 1) = packageInfo.PackageName
            values(row, 2) = Casters.CastDismApplicabilityStatus(packageInfo.PackageApplicable)
            values(row, 3) = packageInfo.PackageCompany
            values(row, 4) = FormatReportDate(packageInfo.PackageCreationTime)
            values(row, 5) = packageInfo.PackageDescription
            values(row, 6) = packageInfo.PackageInstallClient
            values(row, 7) = packageInfo.PackageInstallPackageName
            values(row, 8) = FormatReportDate(packageInfo.PackageInstallTime)
            values(row, 9) = FormatReportDate(packageInfo.PackageLastUpdateTime)
            values(row, 10) = packageInfo.PackageDisplayName
            values(row, 11) = packageInfo.PackageProductName
            values(row, 12) = packageInfo.PackageProductVersion
            values(row, 13) = Casters.CastDismReleaseType(packageInfo.PackageReleaseType)
            values(row, 14) = Casters.CastDismRestartType(packageInfo.PackageRestartRequired)
            values(row, 15) = packageInfo.PackageSupportInformation
            values(row, 16) = Casters.CastDismPackageState(packageInfo.PackageState)
            values(row, 17) = Casters.CastDismFullyOfflineInstallationType(packageInfo.PackageFullyOffline)
            values(row, 18) = packageInfo.PackageCapabilityId
            values(row, 19) = FormatCustomProperties(packageInfo.PackageCustomProperties)
            values(row, 20) = FormatPackageFeatures(packageInfo.PackageFeatures)
            values(row, 21) = packageInfo.PackageCopyright
        Next

        SetRangeValues(sheet, "B7", values, CellStyle.Bordered(), wrapText:=True)
        StyleRange(sheet, "B7:W7", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30})
        StyleRange(sheet, "B:W", New CellStyle() With {.ColumnWidth = 24, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "B:B", New CellStyle() With {.ColumnWidth = 48, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "C:C", New CellStyle() With {.ColumnWidth = 48, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "D:D", New CellStyle() With {.ColumnWidth = 20, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "E:E", New CellStyle() With {.ColumnWidth = 40, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "F:F", New CellStyle() With {.ColumnWidth = 48, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "G:H", New CellStyle() With {.ColumnWidth = 40, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "N:O", New CellStyle() With {.ColumnWidth = 20, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "P:P", New CellStyle() With {.ColumnWidth = 40, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "S:W", New CellStyle() With {.ColumnWidth = 32, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})

        SetSpannedCell(sheet, 5, 25, 1, 2, "Package files by release type", style:=New CellStyle() With {.Italic = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignLeft)
        SetCellValue(sheet, "Y7", "Release type", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})
        SetCellValue(sheet, "Z7", "Count", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})

        Dim releaseTypes As New List(Of String)
        For Each packageInfo In StoredInformation.PackageFileList
            Dim releaseType As String = Casters.CastDismReleaseType(packageInfo.PackageReleaseType)
            If Not releaseTypes.Contains(releaseType) Then
                releaseTypes.Add(releaseType)
            End If
        Next

        Dim lastPackageFileRow As Integer = packageFileCount + 7
        For index As Integer = 0 To releaseTypes.Count - 1
            Dim row As Integer = index + 8
            SetCellValue(sheet, row, 25, releaseTypes(index))
            SetNonLocalizedFormula(sheet, ToAddress(row, 26), String.Format("=COUNTIF($O$8:$O${0},Y{1})", lastPackageFileRow, row))
        Next
        If releaseTypes.Count = 0 Then
            SetCellValue(sheet, "Y8", "No package information was collected.")
            StyleRange(sheet, "Y8:Z8", New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        Else
            StyleRange(sheet, String.Format("Y8:Z{0}", 8 + releaseTypes.Count - 1), New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft})
            Dim chart As Excel.ChartObject = Nothing
            Try
                chart = AddChart(sheet, "Y7:Z" & (releaseTypes.Count + 7).ToString(), "AB7:AI9", New ChartOptions() With {.ChartType = Excel.XlChartType.xlDoughnut, .Title = "Packages by release type"})
            Finally
                ReleaseCom(chart)
            End Try
        End If

        Return sheet
    End Function

    Private Function AddDriverFileInformationSheet(sheets As Excel.Sheets, afterSheet As Excel.Worksheet, ownedSheets As List(Of Excel.Worksheet)) As Excel.Worksheet
        Dim sheet As Excel.Worksheet = If(afterSheet IsNot Nothing, DirectCast(sheets.Add(After:=afterSheet), Excel.Worksheet), DirectCast(sheets.Add(), Excel.Worksheet))
        ownedSheets.Add(sheet)
        WriteSheetTitle(sheet, "Driver File Information")

        Dim rowCount As Integer = 0
        For Each driverFile In StoredInformation.DriverFileList
            rowCount += Math.Max(1, If(driverFile.HardwareTargets Is Nothing, 0, driverFile.HardwareTargets.Count))
        Next

        SetSpannedCell(sheet, 5, 2, 1, 8, String.Format("INFORMATION SUMMARY FOR {0} DRIVER FILE(S), TOTALING {1} HARDWARE TARGET(S):", StoredInformation.DriverFileList.Count, rowCount), style:=New CellStyle() With {.Bold = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignCenter)

        Dim headers As String() = {"Driver package path", "Hardware description", "Hardware ID", "Compatible IDs", "Exclude IDs", "Manufacturer", "Architecture", "Service"}
        Dim values(rowCount, headers.Length - 1) As Object
        For column As Integer = 0 To headers.Length - 1
            values(0, column) = headers(column)
        Next

        Dim rowIndex As Integer = 1
        For Each driverFile In StoredInformation.DriverFileList
            If driverFile.HardwareTargets Is Nothing OrElse driverFile.HardwareTargets.Count = 0 Then
                values(rowIndex, 0) = driverFile.FilePath
                values(rowIndex, 1) = "No hardware targets"
                rowIndex += 1
                Continue For
            End If
            For Each hardwareTarget In driverFile.HardwareTargets
                values(rowIndex, 0) = driverFile.FilePath
                values(rowIndex, 1) = hardwareTarget.HardwareDescription
                values(rowIndex, 2) = hardwareTarget.HardwareId
                values(rowIndex, 3) = hardwareTarget.CompatibleIds
                values(rowIndex, 4) = hardwareTarget.ExcludeIds
                values(rowIndex, 5) = hardwareTarget.ManufacturerName
                values(rowIndex, 6) = Casters.CastDismArchitecture(hardwareTarget.Architecture)
                values(rowIndex, 7) = hardwareTarget.ServerName
                rowIndex += 1
            Next
        Next

        SetRangeValues(sheet, "B7", values, CellStyle.Bordered(), wrapText:=True)
        StyleRange(sheet, "B7:I7", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30})
        StyleRange(sheet, "B:I", New CellStyle() With {.ColumnWidth = 28, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "B:B", New CellStyle() With {.ColumnWidth = 48, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "C:E", New CellStyle() With {.ColumnWidth = 36, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "F:G", New CellStyle() With {.ColumnWidth = 24, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        StyleRange(sheet, "I:I", New CellStyle() With {.ColumnWidth = 32, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})

        ' Data Calculation
        SetSpannedCell(sheet, 5, 11, 1, 2, "Hardware targets by architecture", style:=New CellStyle() With {.Italic = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignLeft)
        SetCellValue(sheet, "K7", "Architecture", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})
        SetCellValue(sheet, "L7", "Count", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})

        Dim architectures As New List(Of String)
        For Each driverFile In StoredInformation.DriverFileList
            If driverFile.HardwareTargets IsNot Nothing Then
                For Each hardwareTarget In driverFile.HardwareTargets
                    Dim architecture As String = Casters.CastDismArchitecture(hardwareTarget.Architecture)
                    If Not architectures.Contains(architecture) Then
                        architectures.Add(architecture)
                    End If
                Next
            End If
        Next

        Dim lastDriverFileRow As Integer = rowCount + 7
        For i As Integer = 0 To architectures.Count - 1
            Dim row As Integer = i + 8
            SetCellValue(sheet, row, 11, architectures(i))
            SetNonLocalizedFormula(sheet, ToAddress(row, 12), String.Format("=COUNTIF($H$8:$H${0},K{1})", lastDriverFileRow, row))
        Next
        If architectures.Count = 0 Then
            SetCellValue(sheet, "K8", "No driver file information was collected.")
            StyleRange(sheet, "K8:L8", New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter})
        Else
            StyleRange(sheet, String.Format("K8:L{0}", 8 + architectures.Count - 1), New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft})
        End If

        SetSpannedCell(sheet, 5, 14, 1, 2, "Hardware targets by manufacturer", style:=New CellStyle() With {.Italic = True}, verticalAlignment:=Excel.XlVAlign.xlVAlignCenter, horizontalAlignment:=Excel.XlHAlign.xlHAlignLeft)
        SetCellValue(sheet, "N7", "Manufacturer", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})
        SetCellValue(sheet, "O7", "Count", New CellStyle() With {.Bold = True, .BackColor = Color.Black, .FontColor = Color.White, .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True, .RowHeight = 30, .ColumnWidth = 30})
        Dim manufacturers As IEnumerable(Of String) = StoredInformation.DriverFileList.SelectMany(Function(driverTarget) driverTarget.HardwareTargets.Select(Function(hwTarget) hwTarget.ManufacturerName)).Distinct().OrderBy(Function(manufacturer) manufacturer)
        For i = 0 To manufacturers.Count - 1
            Dim row As Integer = i + 8
            SetCellValue(sheet, row, 14, manufacturers(i))
            SetNonLocalizedFormula(sheet, ToAddress(row, 15), String.Format("=COUNTIF($G$8:$G${0},N{1})", lastDriverFileRow, row))
        Next
        If manufacturers.Count = 0 Then
            SetCellValue(sheet, "N8", "No driver file information was collected.")
            StyleRange(sheet, "N8:O8", New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .WrapText = True})
        Else
            StyleRange(sheet, String.Format("N8:O{0}", 8 + manufacturers.Count - 1), New CellStyle() With {.BorderStyle = Excel.XlLineStyle.xlContinuous, .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter, .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft, .WrapText = True})
        End If

        Return sheet
    End Function

    Private Function FormatCustomProperties(properties As Microsoft.Dism.DismCustomPropertyCollection) As String
        If properties Is Nothing OrElse properties.Count = 0 Then Return "None"
        Dim values As New List(Of String)
        For Each customProperty In properties
            values.Add(If(String.IsNullOrEmpty(customProperty.Path), String.Empty, customProperty.Path & "\") & customProperty.Name & ": " & customProperty.Value)
        Next
        Return String.Join(Environment.NewLine, values)
    End Function

    Private Function FormatPackageFeatures(features As Microsoft.Dism.DismFeatureCollection) As String
        If features Is Nothing OrElse features.Count = 0 Then Return "None"
        Dim values As New List(Of String)
        For Each feature In features
            values.Add(feature.FeatureName & " (" & Casters.CastDismFeatureState(feature.State) & ")")
        Next
        Return String.Join(Environment.NewLine, values)
    End Function

    Private Function CapabilityShortName(capabilityName As String) As String
        If String.IsNullOrEmpty(capabilityName) Then Return String.Empty
        Dim separator As Integer = capabilityName.IndexOf("~", StringComparison.Ordinal)
        If separator < 0 Then Return capabilityName
        Return capabilityName.Substring(0, separator)
    End Function

    Private Function FormatServiceFailureActions(actions As WindowsService.ServiceFailureActions) As String
        If actions Is Nothing Then Return "Not collected"
        Return String.Format("First: {0} ({1} ms){2}Second: {3} ({4} ms){2}Subsequent: {5} ({6} ms){2}Reset after: {7} sec",
                             actions.FirstFailure, actions.FirstDelayInMillis,
                             Environment.NewLine,
                             actions.SecondFailure, actions.SecondDelayInMillis,
                             actions.SubsequentFailure, actions.SubsequentDelaysInMillis,
                             actions.ResetDelayInSeconds)
    End Function

    Private Function FormatReportDate(value As DateTime) As String
        If value.Year < 1900 Then
            Return "12:00:00 AM - Preposterous time and date"
        End If

        Return value.ToString(currentOSCulture.DateTimeFormat.FullDateTimePattern, currentOSCulture)
    End Function

    Private Sub HTMLFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HTMLFileToolStripMenuItem.Click
        If sfdHtmlReport.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Try
                File.Copy(Path.Combine(Application.StartupPath, "report.html"), sfdHtmlReport.FileName, True)
            Catch ex As Exception

            End Try
        End If
    End Sub
End Class
