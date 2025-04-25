Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.ControlChars

Public Class ConsoleControl
    Inherits UserControl

    Friend NotInheritable Class NativeMethods

        Public Sub New()
        End Sub

        <DllImport("user32.dll")>
        Public Shared Function SendMessage(hwnd As IntPtr, wMsg As UInteger, wParam As UInteger, lParam As IntPtr) As IntPtr
        End Function

    End Class

    Public proc As Process

    Public Sub New()
        ' Llamada necesaria para el diseñador.
        InitializeComponent()
    End Sub

    Public Function StartProcess(executablePath As String, arguments As String) As Integer
        DynaLog.LogMessage("Preparing to start process and redirect output...")
        DynaLog.LogMessage("- Executable path: " & Quote & executablePath & Quote)
        DynaLog.LogMessage("- Arguments: " & arguments & Quote)
        proc = New Process()
        Try
            DynaLog.LogMessage("Setting STDOUT/STDERR encodings...")
            proc.StartInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding(Globalization.CultureInfo.CurrentCulture.TextInfo.OEMCodePage)
            proc.StartInfo.StandardErrorEncoding = System.Text.Encoding.GetEncoding(Globalization.CultureInfo.CurrentCulture.TextInfo.OEMCodePage)
        Catch ex As Exception
            DynaLog.LogMessage("Could not set STDOUT/STDERR encodings. Error message: " & ex.Message)
            DynaLog.LogMessage("Falling back to system default encodings...")
            proc.StartInfo.StandardOutputEncoding = Nothing
            proc.StartInfo.StandardErrorEncoding = Nothing
        End Try
        proc.StartInfo.FileName = executablePath
        proc.StartInfo.Arguments = arguments
        DynaLog.LogMessage("Enabling redirection...")
        proc.StartInfo.CreateNoWindow = True
        proc.StartInfo.UseShellExecute = False
        proc.StartInfo.RedirectStandardOutput = True
        proc.StartInfo.RedirectStandardError = True
        AddHandler proc.OutputDataReceived, AddressOf ProcessOutputHandler
        DynaLog.LogMessage("Starting process...")
        proc.Start()
        proc.BeginOutputReadLine()
        proc.WaitForExit()
        DynaLog.LogMessage("Returning exit code " & Hex(proc.ExitCode) & "...")
        Return proc.ExitCode
    End Function

    Function GetLineEnding(data As String) As String
        If data Is Nothing Then
            Return CrLf
        End If
        If data.StartsWith("[") Then
            Return ""
        End If
        Return CrLf
    End Function

    Private Sub ProcessOutputHandler(sender As Object, e As DataReceivedEventArgs)
        ' Thanks GPT.
        If e.Data Is Nothing Then Exit Sub
        ' Get the last line of the RichTextBox
        Dim lines As String() = RichTextBox1.Lines
        If lines.Length > 0 Then
            Dim lastLine As String = lines.Last()

            ' Check if the last line starts with "[" and replace it
            If lastLine.StartsWith("[") Then
                lines(lines.Length - 1) = e.Data
                RichTextBox1.Lines = lines
            Else
                ' Otherwise, just append the new line
                RichTextBox1.Invoke(CType(Sub()
                                              RichTextBox1.AppendText(e.Data & GetLineEnding(e.Data))
                                          End Sub, MethodInvoker))
            End If
        Else
            ' If there are no lines, just append the new line
            RichTextBox1.Invoke(CType(Sub()
                                          RichTextBox1.AppendText(e.Data & GetLineEnding(e.Data))
                                      End Sub, MethodInvoker))
        End If

        Const WM_VSCROLL As Integer = &H115
        Const SB_BOTTOM As Integer = 7

        RichTextBox1.Invoke(CType(Sub()
                                      NativeMethods.SendMessage(RichTextBox1.Handle, WM_VSCROLL, SB_BOTTOM, IntPtr.Zero)
                                  End Sub, MethodInvoker))
    End Sub

End Class
