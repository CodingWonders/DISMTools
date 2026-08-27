Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.ControlChars
Imports DynaViewer.Classes
Imports DynaViewer.Classes.ColorUtilities
Imports Microsoft.Win32
#If VBC_VER >= 9.0 Then
Imports System.Linq
#End If

Public Class MainForm

    Private LogEvents As New List(Of DynaLogEvent), _
            FilteredLogEvents As New List(Of DynaLogEvent), _
            LogEventPids As New List(Of String), _
            LogEventCallers As New List(Of String), _
            LowercaseLogEventCallers As New List(Of String)
    Private IsViewFiltered As Boolean

    ' Predicate values for background worker
    Dim usePidPredicate As Boolean, _
        useCallerPredicate As Boolean
    Dim pidBWPredicate As String, _
        callerBWPredicate As String, _
        messageBWPredicate As String
    Dim regexException As ArgumentException

    Friend NotInheritable Class NativeMethods

        Public Sub New()
        End Sub

        <DllImport("user32.dll")> _
        Public Shared Function SendMessage(ByVal hwnd As IntPtr, ByVal wMsg As UInteger, ByVal wParam As UInteger, ByVal lParam As IntPtr) As IntPtr
        End Function
    End Class

    Const WM_VSCROLL As Integer = &H115
    Const SB_BOTTOM As Integer = 7

    Public CurrentColorMode As ColorThemeMode

    Private Sub ChangeMenuItemColors(ByVal bgColor As Color, ByVal fgColor As Color, ByVal itemCollection As ToolStripItemCollection)
        For Each tsi As ToolStripItem In itemCollection
            If TypeOf tsi Is ToolStripDropDownItem Then
                Dim item As ToolStripDropDownItem = CType(tsi, ToolStripDropDownItem)
                Try
                    item.DropDown.BackColor = bgColor
                    item.DropDown.ForeColor = fgColor
                    If item.DropDownItems.Count > 0 Then
                        ChangeMenuItemColors(bgColor, fgColor, item.DropDownItems)
                    End If
                Catch ex As Exception
                    Continue For
                End Try
            End If
        Next
    End Sub

    Private Sub SetColorMode(ByVal NewColorMode As ColorThemeMode)
        CurrentColorMode = NewColorMode
        Select Case NewColorMode
            Case ColorThemeMode.Light
                WindowHelper.ToggleDarkTitleBar(Handle, False)

                BackColor = Color.FromArgb(239, 239, 242)
                ForeColor = Color.Black
            Case ColorThemeMode.Dark
                WindowHelper.ToggleDarkTitleBar(Handle, True)

                BackColor = Color.FromArgb(32, 32, 32)
                ForeColor = Color.White
            Case ColorThemeMode.System
                If Environment.OSVersion.Version.Major < 10 Then SetColorMode(ColorThemeMode.Light)

                Try
                    Dim darkMode As Boolean
                    Dim ColorModeRk As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", False)
                    darkMode = ColorModeRk.GetValue("AppsUseLightTheme", 1) = 0
                    ColorModeRk.Close()

                    If darkMode Then SetColorMode(ColorThemeMode.Dark) Else SetColorMode(ColorThemeMode.Light)
                Catch ex As Exception
                    SetColorMode(ColorThemeMode.Light)
                End Try

                Exit Sub
        End Select

        TableLayoutPanel1.ColumnCount = 5

        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        TextBox2.BackColor = BackColor
        TextBox2.ForeColor = ForeColor
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        ComboBox1.BackColor = BackColor
        ComboBox1.ForeColor = ForeColor
        ComboBox2.BackColor = BackColor
        ComboBox2.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        ColorModeCMS.ForeColor = ForeColor

        ProgressBar1.ForeColor = IIf(NewColorMode = ColorThemeMode.Light, Color.FromKnownColor(KnownColor.Highlight), Color.DodgerBlue)

        ColorModeCMS.Renderer = IIf(NewColorMode = ColorThemeMode.Light, New LightModeRenderer(), New DarkModeRenderer())
        ChangeMenuItemColors(BackColor, ForeColor, ColorModeCMS.Items)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub OpenFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBox1.Text = OpenFileDialog1.FileName
        ListView1.Items.Clear()
        Refresh()
        Cursor = Cursors.WaitCursor
        LoadDynaLogFile(OpenFileDialog1.FileName)
        Cursor = Cursors.Arrow
        Button2.Enabled = True
    End Sub

    Sub LoadDynaLogFile(ByVal DynaLogFile As String)
        Label2.Visible = False

        RegexFailureBtn.Visible = False
        TableLayoutPanel1.ColumnCount = 5

        RemoveHandler ComboBox1.SelectedIndexChanged, AddressOf ComboBox1_SelectedIndexChanged
        RemoveHandler ComboBox2.SelectedIndexChanged, AddressOf ComboBox2_SelectedIndexChanged
        RemoveHandler TextBox2.TextChanged, AddressOf TextBox2_TextChanged

        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()
        TextBox2.Text = ""

        ' Clear our previous filters and reset them
        LogEvents.Clear()
        FilteredLogEvents.Clear()
        LogEventPids.Clear()
        LogEventCallers.Clear()
        LowercaseLogEventCallers.Clear()
        LogEventPids.Add("Any PID")
        LogEventCallers.Add("Any Event Caller")

        Refresh()
        Dim dlEvent As DynaLogEvent = Nothing
        If File.Exists(DynaLogFile) Then
            Dim DynaLogLines As String() = File.ReadAllLines(DynaLogFile)
#If VBC_VER < 9.0 Then
            Dim dlItems(DynaLogLines.Length - 1) As ListViewItem
            Dim idx As Integer = 0

            For Each LogLine As String In DynaLogLines
                dlEvent = LogHelper.ParseEventLine(LogLine)
                LogEvents.Add(dlEvent)
                If dlEvent IsNot Nothing Then
                    dlItems(idx) = New ListViewItem(New String() {dlEvent.EventTimestamp, dlEvent.EventPid, dlEvent.EventCaller, dlEvent.EventMessage})
                    idx += 1

                    ' Add fields for our filters
                    If dlEvent.EventPid <> "" AndAlso Not LogEventPids.Contains(dlEvent.EventPid) Then LogEventPids.Add(dlEvent.EventPid)
                    If dlEvent.EventCaller <> "" AndAlso Not LowercaseLogEventCallers.Contains(dlEvent.EventCaller.ToLower()) Then
                        LogEventCallers.Add(dlEvent.EventCaller)
                        LowercaseLogEventCallers.Add(dlEvent.EventCaller.ToLower())
                    End If
                End If
            Next
            ListView1.Items.AddRange(dlItems)
            ComboBox1.Items.AddRange(LogEventPids.ToArray())
            ComboBox2.Items.AddRange(LogEventCallers.ToArray())
#Else
            Dim dlEvents As List(Of DynaLogEvent) = DynaLogLines.Select(Function(dle) LogHelper.ParseEventLine(dle)).Where(Function(dle) dle IsNot Nothing).ToList()
            LogEvents.AddRange(dlEvents.ToArray())
            ListView1.Items.AddRange(LogEvents.Select(Function(dle) New ListViewItem(New String() {dle.EventTimestamp, dle.EventPid, dle.EventCaller, dle.EventMessage})).ToArray())
            LogEventPids.AddRange(LogEvents.Where(Function(dle) dle.EventPid <> "").Select(Function(dle) dle.EventPid).Distinct().ToArray())
            LogEventCallers.AddRange(LogEvents.Where(Function(dle) dle.EventCaller <> "").Select(Function(dle) dle.EventCaller).Distinct().ToArray())
            LowercaseLogEventCallers.AddRange(LogEvents.Select(Function(dle) dle.EventCaller.ToLower()).Distinct().ToArray())
            ComboBox1.Items.AddRange(LogEventPids.ToArray())
            ComboBox2.Items.AddRange(LogEventCallers.ToArray())
#End If

            ComboBox1.SelectedIndex = 0
            ComboBox2.SelectedIndex = 0

            AddHandler ComboBox1.SelectedIndexChanged, AddressOf ComboBox1_SelectedIndexChanged
            AddHandler ComboBox2.SelectedIndexChanged, AddressOf ComboBox2_SelectedIndexChanged
            AddHandler TextBox2.TextChanged, AddressOf TextBox2_TextChanged
        Else
            MsgBox(LocalizationService.ForSection("DynaViewer.Messages").Format("FileExist.Label", DynaLogFile), vbOKOnly + vbCritical, Text)
            Exit Sub
        End If
        Label2.Text = String.Format(LocalizationService.ForSection("DynaViewer")("Proced.Entries.Double.Label"), LogEvents.Count)
        Label2.Visible = True
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If String.IsNullOrEmpty(TextBox1.Text) OrElse Not File.Exists(TextBox1.Text) Then
            Button2.Enabled = False
            Exit Sub
        End If
        ListView1.Items.Clear()
        Refresh()
        Cursor = Cursors.WaitCursor
        LoadDynaLogFile(TextBox1.Text)
        Cursor = Cursors.Arrow
    End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SystemCM_TSMI.Enabled = Environment.OSVersion.Version.Major >= 10

        DebounceTimer.Enabled = True

        SetColorMode(ColorThemeMode.System)
        ' Resize column headers to match system DPI
        ColumnHeader1.Width = WindowHelper.ScaleLogical(192)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(256)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(640)
        ColumnHeader4.Width = WindowHelper.ScaleLogical(94)
        If Environment.GetCommandLineArgs().Length > 0 Then
            For Each CommandArgument As String In Environment.GetCommandLineArgs()
                If CommandArgument.Equals(Environment.GetCommandLineArgs()(0), StringComparison.OrdinalIgnoreCase) Then
                    Continue For
                End If

                If Not CommandArgument.StartsWith("/", StringComparison.OrdinalIgnoreCase) AndAlso File.Exists(CommandArgument) Then
                    TextBox1.Text = CommandArgument.Trim()
                    ListView1.Items.Clear()
                    Refresh()
                    Cursor = Cursors.WaitCursor
                    LoadDynaLogFile(CommandArgument.Trim())
                    Cursor = Cursors.Arrow
                    Button2.Enabled = True
                ElseIf Not CommandArgument.StartsWith("/", StringComparison.OrdinalIgnoreCase) AndAlso Not File.Exists(CommandArgument) Then
                    MsgBox(LocalizationService.ForSection("DynaViewer.Messages").Format("File.NotFound.Message", CommandArgument), vbOKOnly + vbCritical, Text)
                    Exit Sub
                Else
                    If CommandArgument.StartsWith("/selectfirst=", StringComparison.OrdinalIgnoreCase) Then
                        If ListView1.Items.Count > 0 Then
                            Try
                                Dim SelectedItemCount As Integer = CInt(CommandArgument.Replace("/selectfirst=", "").Trim())
                                For i As Integer = 0 To SelectedItemCount - 1
                                    ListView1.Items(i).Selected = True
                                Next
                            Catch ex As Exception

                            End Try
                            ListView1.Select()
                        End If
                    ElseIf CommandArgument.StartsWith("/selectlast=", StringComparison.OrdinalIgnoreCase) Then
                        If ListView1.Items.Count > 0 Then
                            Try
                                Dim SelectedItemCount As Integer = CInt(CommandArgument.Replace("/selectlast=", "").Trim())
                                For i As Integer = 0 To SelectedItemCount - 1
                                    ListView1.Items(ListView1.Items.Count - 1 - i).Selected = True
                                Next
                            Catch ex As Exception

                            End Try
                            ListView1.Select()
                            NativeMethods.SendMessage(ListView1.Handle, WM_VSCROLL, SB_BOTTOM, IntPtr.Zero)
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub ListView1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseDoubleClick
        If e.Button = Windows.Forms.MouseButtons.Left AndAlso ListView1.SelectedItems.Count = 1 Then
            EventProperties.Label1.Text = String.Format("Information for event {0} of {1}:", ListView1.FocusedItem.Index + 1, ListView1.Items.Count)
            EventProperties.txtEventTimestamp.Text = ListView1.FocusedItem.SubItems(0).Text
            EventProperties.Label6.Text = String.Format("PID {0}", ListView1.FocusedItem.SubItems(1).Text)
            Dim evtCallerParts As String() = ListView1.FocusedItem.SubItems(2).Text.Replace(" (", " ").Trim().Split(" ")
            EventProperties.txtEventCaller.Text = evtCallerParts(0)
            If evtCallerParts.Length = 2 Then
                EventProperties.txtEventParentCaller.Text = evtCallerParts(1).TrimEnd(")")
            Else
                EventProperties.txtEventParentCaller.Text = ""
            End If
            EventProperties.txtEventMessage.Text = ListView1.FocusedItem.SubItems(3).Text

            EventProperties.CurrentEventIndex = ListView1.FocusedItem.Index
            EventProperties.EventCount = ListView1.Items.Count
            EventProperties.ShowDialog(Me)
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
#If VBC_VER >= 9.0 Then
        MsgBox(LocalizationService.ForSection("DynaViewer.Messages").Format("Log.Version.Label", My.Application.Info.Version.ToString() & "_" & RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm"), My.Application.Info.Copyright), vbOKOnly + vbInformation, Text)
#Else
        MsgBox(LocalizationService.ForSection("DynaViewer.Messages").Format("About.Version.Message", My.Application.Info.Version.ToString(), My.Application.Info.Copyright), vbOKOnly + vbInformation, Text)
#End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim cmsPos As Point = Button4.PointToScreen(Point.Empty)
        cmsPos.Offset(WindowHelper.ScaleLogical(8), Button4.Height * 0.75)
        ColorModeCMS.Show(cmsPos)
    End Sub

    Private Sub LightCM_TSMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LightCM_TSMI.Click
        SetColorMode(ColorThemeMode.Light)
    End Sub

    Private Sub DarkCM_TSMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DarkCM_TSMI.Click
        SetColorMode(ColorThemeMode.Dark)
    End Sub

    Private Sub SystemCM_TSMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SystemCM_TSMI.Click
        SetColorMode(ColorThemeMode.System)
    End Sub

    Private Sub FilterLogs(Optional ByVal Pid As String = "", Optional ByVal EventCaller As String = "", Optional ByVal Message As String = "")
        FilteredLogEvents.Clear()
        Dim pidPredicate As Boolean = Pid <> "", _
            callerPredicate As Boolean = EventCaller <> "", _
            messagePredicate As Boolean = Message <> ""

        Dim logCount As Integer = LogEvents.Count
        Dim regexMatcher As Regex = Nothing

        If messagePredicate AndAlso RegexCB.Checked Then
            If CaseSensitiveCB.Checked Then
                regexMatcher = New Regex(Message, RegexOptions.Compiled Or RegexOptions.CultureInvariant)
            Else
                regexMatcher = New Regex(Message, RegexOptions.IgnoreCase Or RegexOptions.Compiled Or RegexOptions.CultureInvariant)
            End If
        End If

        For i As Integer = 0 To logCount - 1
            If (i And 63) = 0 AndAlso FilterBW.CancellationPending Then Exit For
            Dim LogEvent As DynaLogEvent = LogEvents(i)
            FilterBW.ReportProgress(CInt((i / logCount) * 100))
            Dim Matches As Boolean = True

            ' PID filter
            If pidPredicate Then
                If LogEvent.EventPid <> Pid Then
                    Matches = False
                End If
            End If

            ' Caller filter
            If Matches AndAlso callerPredicate Then
                If LogEvent.EventCaller <> EventCaller Then
                    Matches = False
                End If
            End If

            ' Message filter
            If Matches AndAlso messagePredicate Then
                Dim messageText As String = LogEvent.EventMessage
                If RegexCB.Checked Then
                    If Not regexMatcher.IsMatch(messageText) Then
                        Matches = False
                    End If
                Else
                    If messageText.IndexOf(Message, IIf(CaseSensitiveCB.Checked, StringComparison.Ordinal, StringComparison.OrdinalIgnoreCase)) = -1 Then
                        Matches = False
                    End If
                End If
            End If

            If Matches Then
                FilteredLogEvents.Add(LogEvent)
            End If
        Next

        IsViewFiltered = pidPredicate Or callerPredicate Or messagePredicate
    End Sub

    Private Sub QueueFilter()
        DebounceTimer.Stop()
        DebounceTimer.Start()
    End Sub

    Private Sub DebounceTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DebounceTimer.Tick
        DebounceTimer.Stop()
        InvokeFilter()
    End Sub

    Private Sub InvokeFilter()
        usePidPredicate = ComboBox1.SelectedIndex > 0
        useCallerPredicate = ComboBox2.SelectedIndex > 0

        ProgressBar1.Visible = True

        pidBWPredicate = ComboBox1.SelectedItem
        callerBWPredicate = ComboBox2.SelectedItem
        messageBWPredicate = TextBox2.Text
        If FilterBW.IsBusy Then
            FilterBW.CancelAsync()
            Do While FilterBW.IsBusy
                Application.DoEvents()
                Threading.Thread.Sleep(25)
            Loop
        End If
        FilterBW.RunWorkerAsync()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        QueueFilter()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        QueueFilter()
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        QueueFilter()
    End Sub

    Private Sub RegexCB_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RegexCB.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("DynaViewer")("Regex.Expressions.Label"))
    End Sub

    Private Sub CaseSensitiveCB_MouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CaseSensitiveCB.MouseHover
        WindowHelper.DisplayToolTip(sender, LocalizationService.ForSection("DynaViewer")("MatchCase.Label"))
    End Sub

    Private Sub RegexCB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RegexCB.CheckedChanged
        If RegexCB.Checked Then
            RegexCheatsheet.Show()
        Else
            RegexCheatsheet.Hide()
        End If
        If TextBox2.Text <> "" Then
            ' We may need to refresh the info based on the mode we enter, whether it is
            ' regex mode or wildcard mode.
            QueueFilter()
        End If
    End Sub

    Private Sub CaseSensitiveCB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CaseSensitiveCB.CheckedChanged
        If TextBox2.Text <> "" Then
            ' We may need to refresh the info based on whether the search is case-sensitive.
            QueueFilter()
        End If
    End Sub

    Private Sub FilterBW_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles FilterBW.DoWork
        FilterBW.ReportProgress(0)
        Try
            FilterLogs(IIf(usePidPredicate, pidBWPredicate, ""), IIf(useCallerPredicate, callerBWPredicate, ""), messageBWPredicate)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub FilterBW_ProgressChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles FilterBW.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage

        If e.ProgressPercentage = 0 Then
            ListView1.Items.Clear()
            Cursor = Cursors.WaitCursor

            Label2.Text = LocalizationService.ForSection("DynaViewer.Main")("EventsFiltering.Message")
        End If
    End Sub

    Private Sub FilterBW_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles FilterBW.RunWorkerCompleted
        Cursor = Cursors.Arrow()
        ProgressBar1.Visible = False

        If e.Error IsNot Nothing AndAlso TypeOf e.Error Is ArgumentException Then
            ' The regex query failed.
            regexException = e.Error
            RegexFailureBtn.Visible = True
            TableLayoutPanel1.ColumnCount = 6
        Else
            RegexFailureBtn.Visible = False
            TableLayoutPanel1.ColumnCount = 5
        End If

        If IsViewFiltered Then
            If FilteredLogEvents.Count = 0 Then Beep()

#If VBC_VER >= 9.0 Then
            ListView1.Items.AddRange(FilteredLogEvents.Select(Function(dle) New ListViewItem(New String() {dle.EventTimestamp, dle.EventPid, dle.EventCaller, dle.EventMessage})).ToArray())
#Else
            Dim dlItems(FilteredLogEvents.Count - 1) As ListViewItem
            Dim idx As Integer = 0

            For Each LogEvent As DynaLogEvent In FilteredLogEvents
                dlItems(idx) = New ListViewItem(New String() {LogEvent.EventTimestamp, LogEvent.EventPid, LogEvent.EventCaller, LogEvent.EventMessage})
                idx += 1
            Next
            ListView1.Items.AddRange(dlItems)
#End If
        Else
#If VBC_VER >= 9.0 Then
            ListView1.Items.AddRange(LogEvents.Select(Function(dle) New ListViewItem(New String() {dle.EventTimestamp, dle.EventPid, dle.EventCaller, dle.EventMessage})).ToArray())
#Else
            Dim dlItems(LogEvents.Count - 1) As ListViewItem
            Dim idx As Integer = 0

            For Each LogEvent As DynaLogEvent In LogEvents
                dlItems(idx) = New ListViewItem(New String() {LogEvent.EventTimestamp, LogEvent.EventPid, LogEvent.EventCaller, LogEvent.EventMessage})
                idx += 1
            Next
            ListView1.Items.AddRange(dlItems)
#End If
        End If

        Label2.Text = String.Format(LocalizationService.ForSection("DynaViewer")("Processed.Entries.Message"), LogEvents.Count, _
                                    IIf(IsViewFiltered, String.Format(LocalizationService.ForSection("DynaViewer")("FilteredEntries.Suffix"), FilteredLogEvents.Count), ""))
    End Sub

    Private Sub RegexFailureBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RegexFailureBtn.Click
        MessageBox.Show(LocalizationService.ForSection("DynaViewer.Messages").Format("RegexInvalid.Message", Environment.NewLine, TextBox2.Text, regexException.Message), LocalizationService.ForSection("DynaViewer.Messages")("Regex.Failure.Label"), MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub
End Class
