Imports System.Windows.Forms
Imports System.Xml.Serialization

Public Class WDSImageGroupSpecifier

    Public SpecifiedImageGroup As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        SpecifiedImageGroup = ComboBox1.SelectedItem
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub GetWdsGroups()
        Try
            Dim PSExtAppxGetterOutput As String = GetGroupGetterOutput()

            If PSExtAppxGetterOutput <> "" Then
                Dim deserializer As New XmlSerializer(GetType(PSInterop.PsObjects))
                Dim objectsCollection As New PSInterop.PsObjects()
                Using reader As New StringReader(PSExtAppxGetterOutput)
                    objectsCollection = CType(deserializer.Deserialize(reader), PSInterop.PsObjects)
                End Using
                If objectsCollection.Items.Count > 0 Then
                    For Each item In objectsCollection.Items
                        ComboBox1.Items.AddRange(item.Properties.Select(Function(prop) prop.Value).ToArray())
                    Next
                End If
            End If
        Catch ex As Exception
            MsgBox("Could not get image groups.", vbOKOnly + vbCritical, Text)
        End Try
    End Sub

    Private Function GetGroupGetterOutput() As String
        Dim output As String = ""
        DynaLog.LogMessage("Running PowerShell script...")
        Using PSWdsGroupProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\WindowsPowerShell\v1.0\powershell.exe",
                .WorkingDirectory = Application.StartupPath,
                .Arguments = "-command Get-WdsInstallImageGroup | Select-Object Name | ConvertTo-Xml -As String",
                .CreateNoWindow = True,
                .WindowStyle = ProcessWindowStyle.Hidden,
                .UseShellExecute = False,
                .RedirectStandardOutput = True
            }
        }
            PSWdsGroupProc.Start()
            output = PSWdsGroupProc.StandardOutput.ReadToEnd()
            PSWdsGroupProc.WaitForExit()
        End Using
        Return output
    End Function

    Private Sub WDSImageGroupSpecifier_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.ForeColor = ForeColor
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ComboBox1.Items.Clear()
        GetWdsGroups()
        Try
            ComboBox1.SelectedIndex = 0
        Catch ex As Exception

        End Try
    End Sub
End Class
