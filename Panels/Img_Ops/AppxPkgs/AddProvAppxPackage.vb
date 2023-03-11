Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class AddProvAppxPackage

    ' Variables used by the AppX scanner component
    Dim AppxNameList As New List(Of String)
    Dim AppxPublisherList As New List(Of String)
    Dim AppxVersionList As New List(Of String)
    Public AppxNames(65535) As String
    Public AppxPublishers(65535) As String
    Public AppxVersion(65535) As String

    ' Variables passed to ProgressPanel
    Public AppxPackages(65535) As String
    Public AppxDependencies(65535) As String

    ' Internal variables helpful to pass information
    Public AppxAdditionCount As Integer
    Public AppxDependencyCount As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        AppxAdditionCount = ListView1.Items.Count
        AppxDependencyCount = ListBox1.Items.Count
        ProgressPanel.appxAdditionCount = AppxAdditionCount
        If ListView1.Items.Count = 0 Then
            MsgBox("Please specify packed or unpacked AppX packages and try again.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
            Exit Sub
        Else
            If AppxAdditionCount > 65535 Then
                MsgBox("Right now, you can only specify less than 65535 AppX packages. This is a program limitation that will be gone in a future update.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                Exit Sub
            Else
                For x = 0 To AppxAdditionCount - 1
                    AppxPackages(x) = ListView1.Items(x).Text
                Next
                For x = 0 To AppxDependencyCount - 1
                    AppxDependencies(x) = ListBox1.Items(x).ToString()
                Next
                ' Fill in remote arrays, even empty slots
                For x = 0 To AppxPackages.Length - 1
                    ProgressPanel.appxAdditionPackages(x) = AppxPackages(x)
                Next
                For x = 0 To AppxDependencies.Length - 1
                    ProgressPanel.appxAdditionDependencies(x) = AppxDependencies(x)
                Next
                ProgressPanel.appxAdditionLastPackage = ListView1.Items(AppxAdditionCount - 1).ToString().Replace("ListViewItem: {", "").Trim().Replace("}", "").Trim()
                If AppxDependencyCount > 0 Then
                    ProgressPanel.appxAdditionLastDependency = ListBox1.Items(AppxDependencyCount - 1).ToString()
                Else
                    ProgressPanel.appxAdditionLastDependency = ""
                End If
                If RadioButton1.Checked Then
                    If TextBox1.Text = "" Then
                        MsgBox("Please specify a license file and try again. You can also continue without one, but this may compromise the image.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                        Exit Sub
                    ElseIf Not File.Exists(TextBox1.Text) Then
                        MsgBox("The license file specified was not found. Make sure it exists on the specified location and try again.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                        Exit Sub
                    Else
                        ProgressPanel.appxAdditionUseLicenseFile = True
                        ProgressPanel.appxAdditionLicenseFile = TextBox1.Text
                    End If
                Else
                    ProgressPanel.appxAdditionUseLicenseFile = False
                    ProgressPanel.appxAdditionLicenseFile = ""
                End If
                If CheckBox1.Checked Then
                    If TextBox2.Text = "" Then
                        MsgBox("Please specify a custom data file and try again. You can also continue without one.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                        Exit Sub
                    ElseIf Not File.Exists(TextBox2.Text) Then
                        MsgBox("The custom data file specified was not found. Make sure it exists on the specified location and try again.", vbOKOnly + vbCritical, "Add provisioned AppX packages")
                        Exit Sub
                    Else
                        ProgressPanel.appxAdditionUseCustomDataFile = True
                        ProgressPanel.appxAdditionCustomDataFile = TextBox2.Text
                    End If
                Else
                    ProgressPanel.appxAdditionUseCustomDataFile = False
                    ProgressPanel.appxAdditionCustomDataFile = ""
                End If
                If RadioButton3.Checked Then
                    ProgressPanel.appxAdditionUseAllRegions = True
                    ProgressPanel.appxAdditionRegions = "all"
                Else
                    ProgressPanel.appxAdditionUseAllRegions = False
                    ProgressPanel.appxAdditionRegions = TextBox3.Text
                End If
                If CheckBox2.Checked Then
                    ProgressPanel.appxAdditionCommit = True
                Else
                    ProgressPanel.appxAdditionCommit = False
                End If
            End If
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        ProgressPanel.OperationNum = 37
        Visible = False
        ProgressPanel.ShowDialog(MainForm)
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub AddProvAppxPackage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            GroupBox1.ForeColor = Color.White
            GroupBox2.ForeColor = Color.White
            GroupBox3.ForeColor = Color.White
            ListView1.BackColor = Color.FromArgb(31, 31, 31)
            ListBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox1.BackColor = Color.FromArgb(31, 31, 31)
            TextBox2.BackColor = Color.FromArgb(31, 31, 31)
            TextBox3.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            GroupBox1.ForeColor = Color.Black
            GroupBox2.ForeColor = Color.Black
            GroupBox3.ForeColor = Color.Black
            ListView1.BackColor = Color.FromArgb(238, 238, 242)
            ListBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox1.BackColor = Color.FromArgb(238, 238, 242)
            TextBox2.BackColor = Color.FromArgb(238, 238, 242)
            TextBox3.BackColor = Color.FromArgb(238, 238, 242)
        End If
        ListView1.ForeColor = ForeColor
        ListBox1.ForeColor = ForeColor
        TextBox1.ForeColor = ForeColor
        TextBox2.ForeColor = ForeColor
        TextBox3.ForeColor = ForeColor
        If My.Computer.Info.OSFullName.Contains("Windows 10") Or My.Computer.Info.OSFullName.Contains("Windows 11") Then
            Text = ""
            Win10Title.Visible = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AppxFileOFD.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If UnpackedAppxFolderFBD.ShowDialog = Windows.Forms.DialogResult.OK And UnpackedAppxFolderFBD.SelectedPath <> "" Then
            ScanAppxPackage(True, UnpackedAppxFolderFBD.SelectedPath)
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ListView1.Items.Clear()
        Button3.Enabled = False
        Button9.Enabled = False
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ListBox1.Items.Clear()
        Button4.Enabled = False
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If Not ListBox1.SelectedItem = "" Then
            ListBox1.Items.Remove(ListBox1.SelectedIndex)
        End If
        If ListBox1.SelectedItem = "" Then
            Button5.Enabled = False
        Else
            Button5.Enabled = True
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        AppxDependencyOFD.ShowDialog()
    End Sub

    Private Sub AppxFileOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles AppxFileOFD.FileOk
        ScanAppxPackage(False, AppxFileOFD.FileName)
    End Sub

    Private Sub AppxDependencyOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles AppxDependencyOFD.FileOk
        ListBox1.Items.Add(AppxDependencyOFD.FileName)
        If ListBox1.Items.Count > 0 Then
            Button4.Enabled = True
        End If
    End Sub

    Private Sub LicenseFileOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles LicenseFileOFD.FileOk
        TextBox1.Text = LicenseFileOFD.FileName
    End Sub

    Private Sub CustomDataFileOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CustomDataFileOFD.FileOk
        TextBox2.Text = CustomDataFileOFD.FileName
    End Sub

    ''' <summary>
    ''' DISMTools AppX header scanner component: version 0.2.2
    ''' </summary>
    ''' <param name="IsFolder">Determines whether the given value for "Package" is a folder</param>
    ''' <param name="Package">The name of the packed or unpacked AppX file. It may be a file containing the full structure, or a folder containing all AppX files</param>
    ''' <remarks>Scans the header of AppX packages to gather application name, publisher, and version information</remarks>
    Sub ScanAppxPackage(IsFolder As Boolean, Package As String)
        Dim Stepper As Integer = 2
        Dim QuoteCount As Integer = 0
        Dim ScannerRTB As New RichTextBox()
        Dim currentAppxName As String = ""
        Dim currentAppxPublisher As String = ""
        Dim currentAppxVersion As String = ""
        Dim pkgName As String = ""
        If IsFolder Then
            If File.Exists(Package & "\AppxMetadata\AppxBundleManifest.xml") Then
                ' AppXBundle file
                ScannerRTB.Text = My.Computer.FileSystem.ReadAllText(Package & "\AppxMetadata\AppxBundleManifest.xml")
                If ScannerRTB.Lines(2).EndsWith("<!--") Then
                    ' XML comment
                    Dim IdScanner As String = ScannerRTB.Lines(9)
                    Dim CharIndex As Integer = 0
                    Dim CharNext As Integer
                    For Each Character As Char In ScannerRTB.Lines(9)
                        CharNext = CharIndex + 1
                        If Not IdScanner(CharIndex) = Quote Then
                            CharIndex += 1
                            Continue For
                        ElseIf IdScanner(CharIndex) = Quote And IdScanner(CharNext) = " " Then
                            CharIndex += 1
                            Continue For
                        Else
                            Character = IdScanner(CharIndex + 1)
                            If Not IdScanner(CharIndex + Stepper) = " " Then
                                If QuoteCount = 0 Then
                                    QuoteCount += 1
                                    Do
                                        If Character = Quote Then
                                            CharIndex += Stepper - 1
                                            Character = IdScanner(CharIndex - 1)
                                            QuoteCount += 1
                                            Stepper = 2
                                            Exit Do
                                        Else
                                            currentAppxName &= Character.ToString()
                                            Character = IdScanner(CharIndex + Stepper)
                                            Stepper += 1
                                        End If
                                    Loop
                                ElseIf QuoteCount = 2 Then
                                    QuoteCount += 1
                                    Do
                                        If Character = Quote Then
                                            CharIndex += Stepper - 1
                                            Character = IdScanner(CharIndex - 1)
                                            QuoteCount += 1
                                            Stepper = 2
                                            Exit Do
                                        Else
                                            currentAppxPublisher &= Character.ToString()
                                            Character = IdScanner(CharIndex + Stepper)
                                            Stepper += 1
                                        End If
                                    Loop
                                ElseIf QuoteCount = 4 Then
                                    QuoteCount += 1
                                    Do
                                        If Character = Quote Then
                                            CharIndex += Stepper - 1
                                            Character = IdScanner(CharIndex - 1)
                                            QuoteCount += 1
                                            Stepper = 2
                                            Exit Do
                                        Else
                                            currentAppxVersion &= Character.ToString()
                                            Character = IdScanner(CharIndex + Stepper)
                                            Stepper += 1
                                        End If
                                    Loop
                                End If
                            End If
                        End If
                    Next
                    AppxNameList.Add(currentAppxName)
                    AppxPublisherList.Add(currentAppxPublisher)
                    AppxVersionList.Add(currentAppxVersion)
                    AppxNames = AppxNameList.ToArray()
                    AppxPublishers = AppxPublisherList.ToArray()
                    AppxVersion = AppxVersionList.ToArray()
                ElseIf ScannerRTB.Lines(2).Contains("<Identity Name=") Then
                    Dim IdScanner As String = ScannerRTB.Lines(2)
                    Dim CharIndex As Integer = 0
                    Dim CharNext As Integer
                    For Each Character As Char In ScannerRTB.Lines(2)
                        CharNext = CharIndex + 1
                        If Not IdScanner(CharIndex) = Quote Then
                            CharIndex += 1
                            Continue For
                        ElseIf IdScanner(CharIndex) = Quote And IdScanner(CharNext) = " " Then
                            CharIndex += 1
                            Continue For
                        Else
                            Character = IdScanner(CharIndex + 1)
                            If Not IdScanner(CharIndex + Stepper) = " " Then
                                If QuoteCount = 0 Then
                                    QuoteCount += 1
                                    Do
                                        If Character = Quote Then
                                            CharIndex += Stepper - 1
                                            Character = IdScanner(CharIndex - 1)
                                            QuoteCount += 1
                                            Stepper = 2
                                            Exit Do
                                        Else
                                            currentAppxName &= Character.ToString()
                                            Character = IdScanner(CharIndex + Stepper)
                                            Stepper += 1
                                        End If
                                    Loop
                                ElseIf QuoteCount = 2 Then
                                    QuoteCount += 1
                                    Do
                                        If Character = Quote Then
                                            CharIndex += Stepper - 1
                                            Character = IdScanner(CharIndex - 1)
                                            QuoteCount += 1
                                            Stepper = 2
                                            Exit Do
                                        Else
                                            currentAppxPublisher &= Character.ToString()
                                            Character = IdScanner(CharIndex + Stepper)
                                            Stepper += 1
                                        End If
                                    Loop
                                ElseIf QuoteCount = 4 Then
                                    QuoteCount += 1
                                    Do
                                        If Character = Quote Then
                                            CharIndex += Stepper - 1
                                            Character = IdScanner(CharIndex - 1)
                                            QuoteCount += 1
                                            Stepper = 2
                                            Exit Do
                                        Else
                                            currentAppxVersion &= Character.ToString()
                                            Character = IdScanner(CharIndex + Stepper)
                                            Stepper += 1
                                        End If
                                    Loop
                                End If
                            End If
                        End If
                    Next
                    AppxNameList.Add(currentAppxName)
                    AppxPublisherList.Add(currentAppxPublisher)
                    AppxVersionList.Add(currentAppxVersion)
                    AppxNames = AppxNameList.ToArray()
                    AppxPublishers = AppxPublisherList.ToArray()
                    AppxVersion = AppxVersionList.ToArray()
                End If
            ElseIf File.Exists(Package & "\AppxManifest.xml") Then
                ' AppX file
                ScannerRTB.Text = My.Computer.FileSystem.ReadAllText(Package & "\AppxManifest.xml")
                If ScannerRTB.Lines(2).EndsWith("<!--") Then
                    Dim IdScanner As String = ScannerRTB.Lines(9)
                    Dim CharIndex As Integer = 0
                    Dim CharNext As Integer
                    For Each Character As Char In ScannerRTB.Lines(9)
                        CharNext = CharIndex + 1
                        If Not IdScanner(CharIndex) = Quote Then
                            CharIndex += 1
                            Continue For
                        ElseIf IdScanner(CharIndex) = Quote And IdScanner(CharNext) = " " Then
                            CharIndex += 1
                            Continue For
                        Else
                            Character = IdScanner(CharIndex + 1)
                            If Not IdScanner(CharIndex + Stepper) = " " Then
                                If QuoteCount = 0 Then
                                    QuoteCount += 1
                                    Do
                                        If Character = Quote Then
                                            CharIndex += Stepper - 1
                                            Character = IdScanner(CharIndex - 1)
                                            QuoteCount += 1
                                            Stepper = 2
                                            Exit Do
                                        Else
                                            currentAppxName &= Character.ToString()
                                            Character = IdScanner(CharIndex + Stepper)
                                            Stepper += 1
                                        End If
                                    Loop
                                ElseIf QuoteCount = 2 Then
                                    QuoteCount += 1
                                    Do
                                        If Character = Quote Then
                                            CharIndex += Stepper - 1
                                            Character = IdScanner(CharIndex - 1)
                                            QuoteCount += 1
                                            Stepper = 2
                                            Exit Do
                                        Else
                                            currentAppxPublisher &= Character.ToString()
                                            Character = IdScanner(CharIndex + Stepper)
                                            Stepper += 1
                                        End If
                                    Loop
                                ElseIf QuoteCount = 4 Then
                                    QuoteCount += 1
                                    Do
                                        If Character = Quote Then
                                            CharIndex += Stepper - 1
                                            Character = IdScanner(CharIndex - 1)
                                            QuoteCount += 1
                                            Stepper = 2
                                            Exit Do
                                        Else
                                            currentAppxVersion &= Character.ToString()
                                            Character = IdScanner(CharIndex + Stepper)
                                            Stepper += 1
                                        End If
                                    Loop
                                End If
                            End If
                        End If
                    Next
                    AppxNameList.Add(currentAppxName)
                    AppxPublisherList.Add(currentAppxPublisher)
                    AppxVersionList.Add(currentAppxVersion)
                    AppxNames = AppxNameList.ToArray()
                    AppxPublishers = AppxPublisherList.ToArray()
                    AppxVersion = AppxVersionList.ToArray()
                End If
            Else
                ' Unrecognized type
                MsgBox("This folder doesn't seem to contain an AppX package structure. It will not be added to the list", vbOKOnly + vbExclamation, "Add provisioned AppX packages")
                Exit Sub
            End If
        Else
            If Directory.Exists(".\appxscan") Then Directory.Delete(".\appxscan", True)
            Directory.CreateDirectory(".\appxscan")
            AppxScanner.StartInfo.FileName = ".\bin\utils\7z.exe"
            AppxScanner.StartInfo.Arguments = "e " & Quote & Package & Quote & " " & Quote & If(Path.GetExtension(Package).EndsWith("bundle"), "appxmetadata\appxbundlemanifest.xml", "appxmanifest.xml") & Quote & " -o.\appxscan"
            AppxScanner.StartInfo.CreateNoWindow = True
            AppxScanner.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            AppxScanner.Start()
            Do Until AppxScanner.HasExited
                If AppxScanner.HasExited Then
                    Exit Do
                End If
            Loop
            If AppxScanner.ExitCode = 0 Then
                If Path.GetExtension(Package).EndsWith("bundle") Then
                    ScannerRTB.Text = My.Computer.FileSystem.ReadAllText(".\appxscan\AppxBundleManifest.xml")
                    Dim IdScanner As String = ScannerRTB.Lines(If(ScannerRTB.Lines(2).EndsWith("<!--"), 10, 4))
                    Dim CharIndex As Integer = 0
                    Dim CharNext As Integer
                    For Each Character As Char In ScannerRTB.Lines(If(ScannerRTB.Lines(2).EndsWith("<!--"), 10, 4))
                        CharNext = CharIndex + 1
                        If Not IdScanner(CharIndex) = Quote Then
                            CharIndex += 1
                            Continue For
                        ElseIf IdScanner(CharIndex) = Quote And IdScanner(CharNext) = " " Then
                            CharIndex += 1
                            Continue For
                        Else
                            Character = IdScanner(CharIndex + 1)
                            If Not IdScanner(CharIndex + Stepper) = " " Then
                                If QuoteCount = 3 Then
                                    QuoteCount += 1
                                    Do
                                        If Character = Quote Then
                                            CharIndex += Stepper - 1
                                            Character = IdScanner(CharIndex - 1)
                                            QuoteCount += 1
                                            Stepper = 2
                                            Exit For
                                        Else
                                            pkgName &= Character.ToString()
                                            Character = IdScanner(CharIndex + Stepper)
                                            Stepper += 1
                                        End If
                                    Loop
                                Else
                                    QuoteCount += 1
                                    CharIndex += Stepper - 1
                                    Character = IdScanner(CharIndex + Stepper)
                                    'Stepper += 1
                                End If
                            End If
                        End If
                    Next
                    pkgName = pkgName.Replace(" ", "%20").Trim()
                    QuoteCount = 0
                    Stepper = 2
                    If ScannerRTB.Lines(2).EndsWith("<!--") Then
                        ' XML comment
                        IdScanner = ScannerRTB.Lines(9)
                        CharIndex = 0
                        CharNext = 0
                        For Each Character As Char In ScannerRTB.Lines(9)
                            CharNext = CharIndex + 1
                            If Not IdScanner(CharIndex) = Quote Then
                                CharIndex += 1
                                Continue For
                            ElseIf IdScanner(CharIndex) = Quote And IdScanner(CharNext) = " " Then
                                CharIndex += 1
                                Continue For
                            Else
                                Character = IdScanner(CharIndex + 1)
                                If Not IdScanner(CharIndex + Stepper) = " " Then
                                    If QuoteCount = 0 Then
                                        QuoteCount += 1
                                        Do
                                            If Character = Quote Then
                                                CharIndex += Stepper - 1
                                                Character = IdScanner(CharIndex - 1)
                                                QuoteCount += 1
                                                Stepper = 2
                                                Exit Do
                                            Else
                                                currentAppxName &= Character.ToString()
                                                Character = IdScanner(CharIndex + Stepper)
                                                Stepper += 1
                                            End If
                                        Loop
                                    ElseIf QuoteCount = 2 Then
                                        QuoteCount += 1
                                        Do
                                            If Character = Quote Then
                                                CharIndex += Stepper - 1
                                                Character = IdScanner(CharIndex - 1)
                                                QuoteCount += 1
                                                Stepper = 2
                                                Exit Do
                                            Else
                                                currentAppxPublisher &= Character.ToString()
                                                Character = IdScanner(CharIndex + Stepper)
                                                Stepper += 1
                                            End If
                                        Loop
                                    ElseIf QuoteCount = 4 Then
                                        QuoteCount += 1
                                        Do
                                            If Character = Quote Then
                                                CharIndex += Stepper - 1
                                                Character = IdScanner(CharIndex - 1)
                                                QuoteCount += 1
                                                Stepper = 2
                                                Exit Do
                                            Else
                                                currentAppxVersion &= Character.ToString()
                                                Character = IdScanner(CharIndex + Stepper)
                                                Stepper += 1
                                            End If
                                        Loop
                                    End If
                                End If
                            End If
                        Next
                        AppxNameList.Add(currentAppxName)
                        AppxPublisherList.Add(currentAppxPublisher)
                        AppxVersionList.Add(currentAppxVersion)
                        AppxNames = AppxNameList.ToArray()
                        AppxPublishers = AppxPublisherList.ToArray()
                        AppxVersion = AppxVersionList.ToArray()
                    ElseIf ScannerRTB.Lines(2).Contains("<Identity Name=") Then
                        IdScanner = ScannerRTB.Lines(2)
                        CharIndex = 0
                        CharNext = 0
                        For Each Character As Char In ScannerRTB.Lines(2)
                            CharNext = CharIndex + 1
                            If Not IdScanner(CharIndex) = Quote Then
                                CharIndex += 1
                                Continue For
                            ElseIf IdScanner(CharIndex) = Quote And IdScanner(CharNext) = " " Then
                                CharIndex += 1
                                Continue For
                            Else
                                Character = IdScanner(CharIndex + 1)
                                If Not IdScanner(CharIndex + Stepper) = " " Then
                                    If QuoteCount = 0 Then
                                        QuoteCount += 1
                                        Do
                                            If Character = Quote Then
                                                CharIndex += Stepper - 1
                                                Character = IdScanner(CharIndex - 1)
                                                QuoteCount += 1
                                                Stepper = 2
                                                Exit Do
                                            Else
                                                currentAppxName &= Character.ToString()
                                                Character = IdScanner(CharIndex + Stepper)
                                                Stepper += 1
                                            End If
                                        Loop
                                    ElseIf QuoteCount = 2 Then
                                        QuoteCount += 1
                                        Do
                                            If Character = Quote Then
                                                CharIndex += Stepper - 1
                                                Character = IdScanner(CharIndex - 1)
                                                QuoteCount += 1
                                                Stepper = 2
                                                Exit Do
                                            Else
                                                currentAppxPublisher &= Character.ToString()
                                                Character = IdScanner(CharIndex + Stepper)
                                                Stepper += 1
                                            End If
                                        Loop
                                    ElseIf QuoteCount = 4 Then
                                        QuoteCount += 1
                                        Do
                                            If Character = Quote Then
                                                CharIndex += Stepper - 1
                                                Character = IdScanner(CharIndex - 1)
                                                QuoteCount += 1
                                                Stepper = 2
                                                Exit Do
                                            Else
                                                currentAppxVersion &= Character.ToString()
                                                Character = IdScanner(CharIndex + Stepper)
                                                Stepper += 1
                                            End If
                                        Loop
                                    End If
                                End If
                            End If
                        Next
                        AppxNameList.Add(currentAppxName)
                        AppxPublisherList.Add(currentAppxPublisher)
                        AppxVersionList.Add(currentAppxVersion)
                        AppxNames = AppxNameList.ToArray()
                        AppxPublishers = AppxPublisherList.ToArray()
                        AppxVersion = AppxVersionList.ToArray()
                    End If
                Else
                    ScannerRTB.Text = My.Computer.FileSystem.ReadAllText(".\appxscan\AppxManifest.xml")
                    If ScannerRTB.Lines(2).EndsWith("<!--") Then
                        Dim IdScanner As String = ScannerRTB.Lines(9)
                        Dim CharIndex As Integer = 0
                        Dim CharNext As Integer
                        For Each Character As Char In ScannerRTB.Lines(9)
                            CharNext = CharIndex + 1
                            If Not IdScanner(CharIndex) = Quote Then
                                CharIndex += 1
                                Continue For
                            ElseIf IdScanner(CharIndex) = Quote And IdScanner(CharNext) = " " Then
                                CharIndex += 1
                                Continue For
                            Else
                                Character = IdScanner(CharIndex + 1)
                                If Not IdScanner(CharIndex + Stepper) = " " Then
                                    If QuoteCount = 0 Then
                                        QuoteCount += 1
                                        Do
                                            If Character = Quote Then
                                                CharIndex += Stepper - 1
                                                Character = IdScanner(CharIndex - 1)
                                                QuoteCount += 1
                                                Stepper = 2
                                                Exit Do
                                            Else
                                                currentAppxName &= Character.ToString()
                                                Character = IdScanner(CharIndex + Stepper)
                                                Stepper += 1
                                            End If
                                        Loop
                                    ElseIf QuoteCount = 2 Then
                                        QuoteCount += 1
                                        Do
                                            If Character = Quote Then
                                                CharIndex += Stepper - 1
                                                Character = IdScanner(CharIndex - 1)
                                                QuoteCount += 1
                                                Stepper = 2
                                                Exit Do
                                            Else
                                                currentAppxPublisher &= Character.ToString()
                                                Character = IdScanner(CharIndex + Stepper)
                                                Stepper += 1
                                            End If
                                        Loop
                                    ElseIf QuoteCount = 4 Then
                                        QuoteCount += 1
                                        Do
                                            If Character = Quote Then
                                                CharIndex += Stepper - 1
                                                Character = IdScanner(CharIndex - 1)
                                                QuoteCount += 1
                                                Stepper = 2
                                                Exit Do
                                            Else
                                                currentAppxVersion &= Character.ToString()
                                                Character = IdScanner(CharIndex + Stepper)
                                                Stepper += 1
                                            End If
                                        Loop
                                    End If
                                End If
                            End If
                        Next
                        AppxNameList.Add(currentAppxName)
                        AppxPublisherList.Add(currentAppxPublisher)
                        AppxVersionList.Add(currentAppxVersion)
                        AppxNames = AppxNameList.ToArray()
                        AppxPublishers = AppxPublisherList.ToArray()
                        AppxVersion = AppxVersionList.ToArray()
                    ElseIf ScannerRTB.Lines(2).Contains("<Identity Name=") Then
                        Dim IdScanner As String = ScannerRTB.Lines(2)
                        Dim CharIndex As Integer = 0
                        Dim CharNext As Integer
                        For Each Character As Char In ScannerRTB.Lines(2)
                            CharNext = CharIndex + 1
                            If Not IdScanner(CharIndex) = Quote Then
                                CharIndex += 1
                                Continue For
                            ElseIf IdScanner(CharIndex) = Quote And IdScanner(CharNext) = " " Then
                                CharIndex += 1
                                Continue For
                            Else
                                Character = IdScanner(CharIndex + 1)
                                If Not IdScanner(CharIndex + Stepper) = " " Then
                                    If QuoteCount = 0 Then
                                        QuoteCount += 1
                                        Do
                                            If Character = Quote Then
                                                CharIndex += Stepper - 1
                                                Character = IdScanner(CharIndex - 1)
                                                QuoteCount += 1
                                                Stepper = 2
                                                Exit Do
                                            Else
                                                currentAppxName &= Character.ToString()
                                                Character = IdScanner(CharIndex + Stepper)
                                                Stepper += 1
                                            End If
                                        Loop
                                    ElseIf QuoteCount = 2 Then
                                        QuoteCount += 1
                                        Do
                                            If Character = Quote Then
                                                CharIndex += Stepper - 1
                                                Character = IdScanner(CharIndex - 1)
                                                QuoteCount += 1
                                                Stepper = 2
                                                Exit Do
                                            Else
                                                currentAppxPublisher &= Character.ToString()
                                                Character = IdScanner(CharIndex + Stepper)
                                                Stepper += 1
                                            End If
                                        Loop
                                    ElseIf QuoteCount = 4 Then
                                        QuoteCount += 1
                                        Do
                                            If Character = Quote Then
                                                CharIndex += Stepper - 1
                                                Character = IdScanner(CharIndex - 1)
                                                QuoteCount += 1
                                                Stepper = 2
                                                Exit Do
                                            Else
                                                currentAppxVersion &= Character.ToString()
                                                Character = IdScanner(CharIndex + Stepper)
                                                Stepper += 1
                                            End If
                                        Loop
                                    End If
                                End If
                            End If
                        Next
                        AppxNameList.Add(currentAppxName)
                        AppxPublisherList.Add(currentAppxPublisher)
                        AppxVersionList.Add(currentAppxVersion)
                        AppxNames = AppxNameList.ToArray()
                        AppxPublishers = AppxPublisherList.ToArray()
                        AppxVersion = AppxVersionList.ToArray()
                    End If
                End If
                GetApplicationStoreLogoAssets(pkgName, If(Path.GetExtension(AppxFileOFD.FileName).EndsWith("bundle"), True, False), Package, currentAppxName)
            Else

            End If
        End If
        If IsFolder Then
            ListView1.Items.Add(New ListViewItem(New String() {Package, "Unpacked", currentAppxName, currentAppxPublisher, currentAppxVersion}))
        Else
            ListView1.Items.Add(New ListViewItem(New String() {Package, "Packed", currentAppxName, currentAppxPublisher, currentAppxVersion}))
        End If
        Button3.Enabled = True
        If Directory.Exists(".\appxscan") Then
            Directory.Delete(".\appxscan", True)
        End If
    End Sub

    ''' <summary>
    ''' Gets the application store logo assets from APPX or APPXBUNDLE packages (also from MSIX and MSIXBUNDLE packages)
    ''' </summary>
    ''' <param name="PackageName">The name of the package. Packages with names containing spaces will replace those with &quot;%20&quot;</param>
    ''' <param name="IsBundlePackage">Determines if the package given is an APPXBUNDLE or MSIXBUNDLE package</param>
    ''' <param name="SourcePackage">The path of the source package</param>
    ''' <param name="AppxPackageName">The name of the AppX package, used for storing logo assets in an organized way</param>
    ''' <remarks>If the package processed is an APPXBUNDLE or MSIXBUNDLE package, this procedure will extract the asset contents from the package with the given name. Otherwise, it will directly extract them from the &quot;Assets&quot; folder</remarks>
    Sub GetApplicationStoreLogoAssets(PackageName As String, IsBundlePackage As Boolean, SourcePackage As String, AppxPackageName As String)
        ' The assets from the main package are enough for us. The current AppX XML schema also puts these in the Assets folder, so
        ' getting them should be a breeze
        If IsBundlePackage Then
            AppxScanner.StartInfo.Arguments = "e " & Quote & SourcePackage & Quote & " " & Quote & PackageName & Quote & " -o.\appxscan"
            AppxScanner.Start()
            Do Until AppxScanner.HasExited
                If AppxScanner.HasExited Then
                    Exit Do
                End If
            Loop
            If Not Directory.Exists(Directory.GetCurrentDirectory() & "\temp\storeassets") Then Directory.CreateDirectory(Directory.GetCurrentDirectory() & "\temp\storeassets").Attributes = FileAttributes.Hidden
            If AppxScanner.ExitCode = 0 Then
                Directory.CreateDirectory(Directory.GetCurrentDirectory() & "\temp\storeassets\" & AppxPackageName)
                If My.Computer.FileSystem.GetFiles(Directory.GetCurrentDirectory() & "\temp\storeassets\" & AppxPackageName).Count <= 0 Then
                    ' Try extracting small, store and large assets
                    AppxScanner.StartInfo.Arguments = "e " & Quote & Directory.GetCurrentDirectory() & "\appxscan\" & PackageName & Quote & " " & Quote & "Assets\small*" & Quote & " -o" & Quote & ".\temp\storeassets\" & AppxPackageName & Quote
                    AppxScanner.Start()
                    Do Until AppxScanner.HasExited
                        If AppxScanner.HasExited Then
                            Exit Do
                        End If
                    Loop
                    AppxScanner.StartInfo.Arguments = "e " & Quote & Directory.GetCurrentDirectory() & "\appxscan\" & PackageName & Quote & " " & Quote & "Assets\store*" & Quote & " -o" & Quote & ".\temp\storeassets\" & AppxPackageName & Quote
                    AppxScanner.Start()
                    Do Until AppxScanner.HasExited
                        If AppxScanner.HasExited Then
                            Exit Do
                        End If
                    Loop
                    AppxScanner.StartInfo.Arguments = "e " & Quote & Directory.GetCurrentDirectory() & "\appxscan\" & PackageName & Quote & " " & Quote & "Assets\large*" & Quote & " -o" & Quote & ".\temp\storeassets\" & AppxPackageName & Quote
                    AppxScanner.Start()
                    Do Until AppxScanner.HasExited
                        If AppxScanner.HasExited Then
                            Exit Do
                        End If
                    Loop
                End If
            End If
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If ListView1.FocusedItem.Text <> "" Then
            ListView1.Items.Remove(ListView1.FocusedItem)
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://en.wikipedia.org/wiki/ISO_3166-1#Current_codes")
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            TextBox1.Enabled = True
            Button7.Enabled = True
        Else
            TextBox1.Enabled = False
            Button7.Enabled = False
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            TextBox2.Enabled = True
            Button8.Enabled = True
        Else
            TextBox2.Enabled = False
            Button8.Enabled = False
        End If
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        If RadioButton3.Checked Then
            TextBox3.Enabled = False
            Label5.Enabled = False
            LinkLabel1.Enabled = False
        Else
            TextBox3.Enabled = True
            Label5.Enabled = True
            LinkLabel1.Enabled = True
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            If ListView1.SelectedItems.Count <> 1 Then
                Button9.Enabled = False
            Else
                Button9.Enabled = True
            End If
        Catch ex As NullReferenceException
            Button9.Enabled = True
        End Try
        NoAppxFilePanel.Visible = If(ListView1.SelectedItems.Count <= 0, True, False)
        AppxFilePanel.Visible = If(ListView1.SelectedItems.Count <= 0, False, True)
        If ListView1.SelectedItems.Count > 0 Then
            Try
                Label7.Text = ListView1.FocusedItem.SubItems(2).Text
                Label8.Text = "Publisher: " & ListView1.FocusedItem.SubItems(3).Text
                Label9.Text = "Version: " & ListView1.FocusedItem.SubItems(4).Text
            Catch ex As NullReferenceException

            End Try
        End If
        Try
            If Directory.Exists(Directory.GetCurrentDirectory() & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text) And My.Computer.FileSystem.GetFiles(Directory.GetCurrentDirectory() & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text).Count > 0 Then
                Dim asset As Image = Nothing
                For Each StoreAsset In My.Computer.FileSystem.GetFiles(Directory.GetCurrentDirectory() & "\temp\storeassets\" & ListView1.FocusedItem.SubItems(2).Text)
                    If Path.GetExtension(StoreAsset).EndsWith("png") Then
                        asset = Image.FromFile(StoreAsset)
                        If asset.Width / asset.Height = 1 Then      ' Determine if the image's aspect ratio is 1:1
                            If asset.Width <= 100 And asset.Height <= 100 Then      ' Determine if it is a "small" or "store" asset
                                PictureBox2.Image = asset
                            End If
                        End If
                    End If
                Next
            End If
        Catch ex As NullReferenceException

        End Try
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.SelectedItem = "" Then
            Button5.Enabled = False
        Else
            Button5.Enabled = True
        End If
    End Sub
End Class
