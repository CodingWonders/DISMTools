Imports System.IO
Imports IniParser
Imports IniParser.Parser
Imports IniParser.Model
Imports System.Text
Imports DrvPakCreator.Classes
Imports DrvPakCreator.CLASSES.Exceptions
Imports Microsoft.Win32

Namespace Helpers

    Public Class DriverPackProjectHelper

        Private Const PackVersion As String = "0.9"
        Private Const PackCodename As String = "Discovery"
        Private Const PackSignature As String = "Stardust"

        Public Shared Function CreatePackProject(ByVal PackProjectName As String, ByVal PackProjectLocation As String, Optional ByVal CreateInnerStructure As Boolean = True) As Boolean
            If PackProjectName = "" Then Throw New ArgumentNullException(PackProjectName)
            If PackProjectLocation = "" Then Throw New ArgumentNullException(PackProjectLocation)

            If Not Directory.Exists(PackProjectLocation) Then
                Try
                    Directory.CreateDirectory(PackProjectLocation)
                Catch ex As Exception
                    Throw New DirectoryNotFoundException(PackProjectLocation)
                End Try
            End If

            ' Define paths for Project Files
            Dim PakProjLocation As String = Path.Combine(PackProjectLocation, String.Format("{0}.pakproj", PackProjectName)), _
                PakProjectFiles As String = Path.Combine(PackProjectLocation, "ProjectFiles"), _
                PakProjPAKDAT As String = Path.Combine(PakProjectFiles, "PAKDAT"), _
                PakProjMetadata As String = Path.Combine(PakProjectFiles, "DRV_METADATA.XML"), _
                PakProjDrivers As String = Path.Combine(PakProjectFiles, "DRIVERS")

            Try
                ' Create Pack Project Structure
                Dim PakProjParser As New FileIniDataParser(), _
                    PakProjData As New IniData()

                PakProjData.Sections.AddSection("DriverPackProject")
                PakProjData.Sections("DriverPackProject").AddKey("Signature", PackSignature)
                PakProjData.Sections("DriverPackProject").AddKey("Name", PackProjectName)

                PakProjParser.WriteFile(PakProjLocation, PakProjData)

                Directory.CreateDirectory(PakProjectFiles)
                If CreateInnerStructure Then
                    File.WriteAllText(PakProjPAKDAT, String.Format("DISMTools {0}{1}{0} Driver Pack File -- Driver Pack File Format {2}", Quote, PackCodename, PackVersion))
                    File.SetAttributes(PakProjPAKDAT, FileAttributes.ReadOnly)

                    Dim registeredUserInfoStr As String = "", _
                        regUserInfoRk As RegistryKey = Nothing, _
                        registeredUser As String = "", _
                        registeredOrg As String = ""
                    Try
                        regUserInfoRk = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion")
                        registeredUser = regUserInfoRk.GetValue("RegisteredOwner", "")
                        registeredOrg = regUserInfoRk.GetValue("RegisteredOrganization", "")
                        registeredUserInfoStr = String.Format("{0}{1}", registeredUser, IIf(Not String.IsNullOrEmpty(registeredOrg), String.Format(" - {0}", registeredOrg), ""))
                    Catch ex As Exception

                    Finally
                        If regUserInfoRk IsNot Nothing Then regUserInfoRk.Close()
                    End Try

                    Dim NewDriverMetadata As New DriverPack()
                    With NewDriverMetadata
                        .Name = PackProjectName
                        .Author = registeredUserInfoStr
                    End With

                    DriverPackHelper.SerializeDriverPackMetadata(PakProjMetadata, NewDriverMetadata)

                    Directory.CreateDirectory(PakProjDrivers)
                End If
                Return True
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                Return False
            End Try
        End Function

        Public Shared Function LoadDriverPackProject(ByVal ProjectPath As String) As DriverPackProject
            If ProjectPath = "" Then Throw New ArgumentNullException(ProjectPath)
            If Not File.Exists(ProjectPath) Then Throw New FileNotFoundException(ProjectPath)

            Try
                Dim parser As New FileIniDataParser(), _
                    projectData As IniData
                Using reader As New StreamReader(ProjectPath)
                    projectData = parser.ReadData(reader)
                End Using

                If Not projectData.Sections.ContainsSection("DriverPackProject") OrElse projectData("DriverPackProject")("Signature") <> PackSignature Then
                    Throw New InvalidProjectFormatException()
                End If

                Dim ProjectPathLocation As String = Path.GetDirectoryName(ProjectPath), _
                    ProjectFilesLocation As String = Path.Combine(ProjectPathLocation, "ProjectFiles"), _
                    ProjectPAKDAT As String = Path.Combine(ProjectFilesLocation, "PAKDAT"), _
                    ProjectMetadata As String = Path.Combine(ProjectFilesLocation, "DRV_METADATA.XML")

                If Not Directory.Exists(ProjectFilesLocation) Then Throw New InvalidProjectFormatException()
                If Not File.Exists(ProjectPAKDAT) Then Throw New InvalidProjectFormatException()
                If Not File.Exists(ProjectMetadata) Then Throw New InvalidProjectFormatException()

                Dim ObtainedDriverMetadata As DriverPack = DriverPackHelper.DeserializeDriverPackMetadata(ProjectMetadata)
                If ObtainedDriverMetadata Is Nothing Then Throw New CorruptPackMetadataException()

                Dim ParsedProject As New DriverPackProject()

                With ParsedProject
                    .Name = projectData("DriverPackProject")("Name")
                    .Signature = projectData("DriverPackProject")("Signature")
                    .PackMetadata = ObtainedDriverMetadata
                End With

                Return ParsedProject
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Load Failure", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return Nothing
            End Try
        End Function

        Public Shared Function SaveDriverPackProject(ByVal ProjectPath As String, ByVal Project As DriverPackProject) As Boolean
            If ProjectPath = "" Then Throw New ArgumentNullException(ProjectPath)
            If Project Is Nothing Then Throw New ArgumentNullException("Project")
            If Not File.Exists(ProjectPath) Then Throw New FileNotFoundException()

            ' Define paths for Project Files
            Dim PakProjLocation As String = Path.GetDirectoryName(ProjectPath), _
                PakProjectFiles As String = Path.Combine(PakProjLocation, "ProjectFiles"), _
                PakProjMetadata As String = Path.Combine(PakProjectFiles, "DRV_METADATA.XML")

            Try
                Dim parser As New FileIniDataParser(), _
                    PakProjData As New IniData()

                PakProjData.Sections.AddSection("DriverPackProject")
                PakProjData.Sections("DriverPackProject").AddKey("Signature", PackSignature)
                PakProjData.Sections("DriverPackProject").AddKey("Name", Project.Name)

                parser.WriteFile(ProjectPath, PakProjData)

                If Not DriverPackHelper.SerializeDriverPackMetadata(PakProjMetadata, Project.PackMetadata) Then
                    MessageBox.Show("Metadata could not be saved.", "Save Failure", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If

                Return True
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Save Failure", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Function

    End Class

End Namespace