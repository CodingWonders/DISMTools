Imports System.IO
Imports System.Threading.Tasks
Imports Microsoft.Dism

Namespace Elements.ISOCreation

    Public Class IsoCreationTask

        Public Property SourceImageFiles As List(Of String)
        Public Property DestinationIsoFile As String
        Public Property DestinationIsoArchitecture As IsoArchitecture

        Public Property ImageInformationCollection As List(Of DismImageInfoCollection)

        Private ReadOnly Property IsoArchitectureString As String
            Get
                Select Case DestinationIsoArchitecture
                    Case IsoArchitecture.X86 : Return "x86"
                    Case IsoArchitecture.AMD64 : Return "amd64"
                    Case IsoArchitecture.ARM64 : Return "arm64"
                    Case Else : Return ""
                End Select
            End Get
        End Property

        Public Property UnattendedAnswerFile As String
        Public Property CopyToVentoy As Boolean
        Public Property UseUEFICA2023Binaries As Boolean
        Public Property IncludeSystemDrivers As Boolean

        Public Sub New(SourceImage As List(Of String), InfoCollection As List(Of DismImageInfoCollection), DestinationIso As String, Architecture As IsoArchitecture)
            SourceImageFiles = SourceImage
            ImageInformationCollection = InfoCollection
            DestinationIsoFile = DestinationIso
            DestinationIsoArchitecture = Architecture
            UnattendedAnswerFile = ""
            CopyToVentoy = False
            UseUEFICA2023Binaries = False
            IncludeSystemDrivers = False
        End Sub

        Public Sub New(SourceImage As List(Of String), InfoCollection As List(Of DismImageInfoCollection), DestinationIso As String, Architecture As IsoArchitecture, AnswerFile As String, ToVentoyDrive As Boolean, UseUEFICA23BootBins As Boolean, IncludeSysDrivers As Boolean)
            SourceImageFiles = SourceImage
            ImageInformationCollection = InfoCollection
            DestinationIsoFile = DestinationIso
            DestinationIsoArchitecture = Architecture
            UnattendedAnswerFile = AnswerFile
            CopyToVentoy = ToVentoyDrive
            UseUEFICA2023Binaries = UseUEFICA23BootBins
            IncludeSystemDrivers = IncludeSysDrivers
        End Sub

        Public Async Function StartTaskAsync() As Task(Of Boolean)
            Dim PWSHPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "WindowsPowerShell", "v1.0", "powershell.exe"),
                PEHelperPath As String = Path.Combine(Application.StartupPath, "bin", "extps1", "PE_Helper"),
                PEHelperScriptPath As String = Path.Combine(PEHelperPath, "PE_Helper.ps1")

            If Not File.Exists(PWSHPath) OrElse Not Directory.Exists(PEHelperPath) OrElse Not File.Exists(PEHelperScriptPath) Then Return False

            Dim ISOCreator As New Process() With {
                .StartInfo = New ProcessStartInfo() With {
                    .FileName = PWSHPath,
                    .WorkingDirectory = PEHelperPath
                }
            }

            Dim SourceImageFilePaths() As String = SourceImageFiles.ToArray()

            ISOCreator.StartInfo.Arguments = String.Format("-noprofile -nologo -executionpolicy unrestricted -file {0}{1}{0} -cmd StartPEGen -arch {2} -imgFiles {0}{3}{0} -isoPath {0}{4}{0} -unattendFile {0}{5}{0}{6}{7}{8}",
                                                           Quote, PEHelperScriptPath, IsoArchitectureString, String.Join("|", SourceImageFilePaths), DestinationIsoFile, UnattendedAnswerFile, If(CopyToVentoy, " -copytoventoy", ""), If(UseUEFICA2023Binaries, " -bootex", ""), If(IncludeSystemDrivers, " -includeSysDrivers", ""))

            Dim ExitCode As Integer = 0

            Await Task.Run(Sub()
                               ISOCreator.Start()
                               ISOCreator.WaitForExit()
                               ExitCode = ISOCreator.ExitCode
                           End Sub)

            Return ExitCode = 0
        End Function

    End Class

    Public Enum IsoArchitecture As Integer
        X86 = 0
        AMD64 = 1
        ARM64 = 2
    End Enum

End Namespace