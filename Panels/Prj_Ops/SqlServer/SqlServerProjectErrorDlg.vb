Imports System.Windows.Forms
Imports System.IO

Public Class SqlServerProjectErrorDlg

    Dim VsVersionCount As Integer
    Dim SsmsVersionCount As Integer
    Dim VsVersionArr(10) As String
    Dim SsmsVersionArr(10) As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Sub CheckVisualStudioVersions()
        ' Check versions of Visual Studio
        Dim x As Integer = 0
        Dim LastArrEntry As String = ""
        ' Perform check for versions prior to VS2022
        If Environment.Is64BitOperatingSystem Then
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\Microsoft Visual Studio 8.0\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2005"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2005"
            End If
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\Microsoft Visual Studio 9.0\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2008"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2008"
            End If
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2010"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2010"
            End If
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\Microsoft Visual Studio 11.0\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2012"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2012"
            End If
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\Microsoft Visual Studio 12.0\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2013"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2013"
            End If
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2015"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2015"
            End If
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2017"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2017"
            End If
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) & "\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2019"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2019"
            End If
        Else
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Microsoft Visual Studio 8.0\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2005"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2005"
            End If
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Microsoft Visual Studio 9.0\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2008"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2008"
            End If
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2010"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2010"
            End If
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Microsoft Visual Studio 11.0\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2012"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2012"
            End If
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Microsoft Visual Studio 12.0\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2013"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2013"
            End If
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2015"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2015"
            End If
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2017"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2017"
            End If
            If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe") Then
                VsVersionArr(x) = "2019"
                VsVersionCount += 1
                x += 1
                LastArrEntry = "2019"
            End If
        End If
        If File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe") Or File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) & "\Microsoft Visual Studio\2022\Preview\Common7\IDE\devenv.exe") Then
            VsVersionArr(x) = "2022"
            VsVersionCount += 1
            x += 1
            LastArrEntry = "2022"
        End If
        For x = 0 To Array.LastIndexOf(VsVersionArr, LastArrEntry)
            ComboBox1.Items.Add(VsVersionArr(x))
        Next
        Select Case VsVersionCount
            Case 0

            Case Is > 0

        End Select
        ComboBox1.SelectedIndex = ComboBox1.Items.Count - 1
    End Sub

    Sub CheckSsmsVersions()
        ' Check versions of SQL Server Management Studio
    End Sub

    Private Sub SqlServerProjectErrorDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()
        CheckVisualStudioVersions()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://visualstudio.microsoft.com")
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Process.Start("https://microsoft.com/sql-server")
    End Sub
End Class
