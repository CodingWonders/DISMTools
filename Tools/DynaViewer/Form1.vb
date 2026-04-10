Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.ControlChars
Imports DynaViewer.Classes
Imports DynaViewer.Classes.ColorUtilities
Imports Microsoft.Win32
#If VBC_VER >= 9.0 Then
Imports System.Linq
#End If

Public Class Form1

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

        TextBox1.BackColor = BackColor
        TextBox1.ForeColor = ForeColor
        ListView1.BackColor = BackColor
        ListView1.ForeColor = ForeColor
        GroupBox1.ForeColor = ForeColor
        ColorModeCMS.ForeColor = ForeColor

        If NewColorMode = ColorThemeMode.Light Then
            ColorModeCMS.Renderer = New LightModeRenderer()
        ElseIf NewColorMode = ColorThemeMode.Dark Then
            ColorModeCMS.Renderer = New DarkModeRenderer()
        End If
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
        Refresh()
        Dim dlEvent As DynaLogEvent
        If File.Exists(DynaLogFile) Then
            Dim DynaLogLines As String() = File.ReadAllLines(DynaLogFile)
#If VBC_VER < 9.0 Then
            Dim dlItems(DynaLogLines.Length - 1) As ListViewItem
            Dim idx As Integer = 0

            For Each LogLine As String In DynaLogLines
                dlEvent = LogHelper.ParseEventLine(LogLine)
                If dlEvent IsNot Nothing Then
                    dlItems(idx) = New ListViewItem(New String() {dlEvent.EventTimestamp, dlEvent.EventPid, dlEvent.EventCaller, dlEvent.EventMessage})
                    idx += 1
                End If
            Next
            ListView1.Items.AddRange(dlItems)
#Else
            Dim dlEvents As New List(of DynaLogEvent)
            For Each LogLine As String In DynaLogLines
                dlEvent = LogHelper.ParseEventLine(LogLine)
                If dlEvent IsNot Nothing Then
                    dlEvents.Add(dlEvent)
                End If
            Next
            ListView1.Items.AddRange(dlEvents.Select(Function(dle) New ListViewItem(New String() {dle.EventTimestamp, dle.EventPid, dle.EventCaller, dle.EventMessage})).ToArray())
#End If
        Else
            MsgBox("The file " & Quote & DynaLogFile & Quote & " does not exist.", vbOKOnly + vbCritical, Text)
            Exit Sub
        End If
        Label2.Text = String.Format("Number of processed entries: {0}. Double-click an entry to get its information.", ListView1.Items.Count)
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

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SystemCM_TSMI.Enabled = Environment.OSVersion.Version.Major >= 10

        SetColorMode(ColorThemeMode.System)
        ' Resize column headers to match system DPI
        ColumnHeader1.Width = WindowHelper.ScaleLogical(145)
        ColumnHeader2.Width = WindowHelper.ScaleLogical(149)
        ColumnHeader3.Width = WindowHelper.ScaleLogical(443)
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
                    MsgBox("The file " & Quote & CommandArgument & Quote & " does not exist.", vbOKOnly + vbCritical, Text)
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
        MsgBox(String.Format("DynaLog Log Viewer (DynaViewer) version {0}" & CrLf & CrLf & "{1}", _
                My.Application.Info.Version.ToString() & "_" & RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm") , _
                My.Application.Info.Copyright), _
            vbOKOnly + vbInformation, Text)
#Else
        MsgBox(String.Format("DynaLog Log Viewer (DynaViewer) version {0}_NET2REL" & CrLf & CrLf & "{1}", _
                My.Application.Info.Version.ToString(), _
                My.Application.Info.Copyright), _
            vbOKOnly + vbInformation, Text)
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
End Class
