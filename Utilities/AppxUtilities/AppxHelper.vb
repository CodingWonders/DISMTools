Imports System.IO
Imports Microsoft.Win32
Imports Microsoft.Dism
Imports DISMTools.Utilities

Module AppxHelper

    Private ReadOnly CopiedAppxFilePath As String = Path.Combine(Application.StartupPath, "AppxManifest.xml")

    Private PackageRootPath As String = ""
    Private PackageRepositoryRootPath As String = ""

    Private Function GetAppxPackageRootPath(MountDirectory As String) As String
        Dim PackageRootPath As String = ""
        If Not Directory.Exists(MountDirectory) Then Return ""

        DynaLog.LogMessage("Opening SOFTWARE hive...")
        If Not RegistryHelper.LoadRegistryHive(Path.Combine(MountDirectory, "Windows", "system32", "config", "SOFTWARE"), "HKLM\zSOFT") = 0 Then Return ""
        Try
            Dim AppxRootKey As RegistryKey = Registry.LocalMachine.OpenSubKey("zSOFT\Microsoft\Windows\CurrentVersion\Appx", False)
            PackageRootPath = AppxRootKey.GetValue("PackageRoot", "")
            AppxRootKey.Close()
        Catch ex As Exception
            DynaLog.LogMessage("Could not check root path. Error message: " & ex.Message)
        Finally
            RegistryHelper.UnloadRegistryHive("HKLM\zSOFT")
        End Try

        ' We have to tweak the obtained path a bit because it's an absolute path. This is not allowed; we need to combine its
        ' path with the mount directory. That IS the expected path.
        Dim PathRoot As String = Path.GetPathRoot(PackageRootPath)
        PackageRootPath = PackageRootPath.Replace(PathRoot, String.Format("{0}\", MountDirectory))

        Return PackageRootPath
    End Function

    Private Function GetAppxPackageRepositoryRootPath(MountDirectory As String) As String
        Dim PackageRepositoryRootPath As String = ""
        If Not Directory.Exists(MountDirectory) Then Return ""

        DynaLog.LogMessage("Opening SOFTWARE hive...")
        If Not RegistryHelper.LoadRegistryHive(Path.Combine(MountDirectory, "Windows", "system32", "config", "SOFTWARE"), "HKLM\zSOFT") = 0 Then Return ""
        Try
            Dim AppxRootKey As RegistryKey = Registry.LocalMachine.OpenSubKey("zSOFT\Microsoft\Windows\CurrentVersion\Appx", False)
            PackageRepositoryRootPath = AppxRootKey.GetValue("PackageRepositoryRoot", "")
            AppxRootKey.Close()
        Catch ex As Exception
            DynaLog.LogMessage("Could not check root path. Error message: " & ex.Message)
        Finally
            RegistryHelper.UnloadRegistryHive("HKLM\zSOFT")
        End Try

        ' We have to tweak the obtained path a bit because it's an absolute path. This is not allowed; we need to combine its
        ' path with the mount directory. That IS the expected path.
        Dim PathRoot As String = Path.GetPathRoot(PackageRepositoryRootPath)
        PackageRepositoryRootPath = PackageRepositoryRootPath.Replace(PathRoot, String.Format("{0}\", MountDirectory))

        Return PackageRepositoryRootPath
    End Function

    Public Sub ClearRootPaths()
        PackageRootPath = ""
        PackageRepositoryRootPath = ""
    End Sub

    Public Sub SetRootPaths(MountDirectory As String)
        PackageRootPath = GetAppxPackageRootPath(MountDirectory)
        PackageRepositoryRootPath = GetAppxPackageRepositoryRootPath(MountDirectory)

        If PackageRootPath = "" Then PackageRootPath = String.Format("{0}\Program Files\WindowsApps", MountDirectory)
    End Sub

    ''' <summary>
    ''' Gets the application display name from the AppX package manifest
    ''' </summary>
    ''' <param name="PackageName">The package name of an application</param>
    ''' <param name="DisplayName">The display name of an application. This parameter is required when there are multiple directories with their names containing <paramref name="PackageName">the package name</paramref></param>
    ''' <returns>pkgName: the suitable package display name</returns>
    ''' <remarks>If pkgName returns Nothing, the callers will hide those options calling this function</remarks>
    Public Function GetPackageDisplayName(MountDirectory As String, PackageName As String, Optional DisplayName As String = "") As String
        DynaLog.LogMessage("Beginning detection of display name from package...")
        DynaLog.LogMessage("- Package name to use for scan process: " & PackageName)
        DynaLog.LogMessage("- Display name to use for scan process: " & DisplayName)

        If PackageRootPath = "" Then
            DynaLog.LogMessage("Getting package root path...")
            PackageRootPath = GetAppxPackageRootPath(MountDirectory)
            If PackageRootPath = "" Then PackageRootPath = String.Format("{0}\Program Files\WindowsApps", MountDirectory)
            DynaLog.LogMessage("AppX Package Root: " & PackageRootPath)
        End If

        Dim pkgName As String = ""

        Try
            DynaLog.LogMessage("Copying AppX package manifest to local directory...")
            If File.Exists(CopiedAppxFilePath) Then File.Delete(CopiedAppxFilePath)
            If File.Exists(String.Format("{0}\{1}\AppxManifest.xml", PackageRootPath, PackageName)) Then
                DynaLog.LogMessage("An AppX manifest file exists in the main directory. There are no variations of any kind")
                ' Copy manifest to startup dir
                File.Copy(String.Format("{0}\{1}\AppxManifest.xml", PackageRootPath, PackageName), CopiedAppxFilePath, True)
                DynaLog.LogMessage("Reading AppX manifest...")
                Dim XmlReaderLines As String() = File.ReadAllLines(CopiedAppxFilePath)
                ' Go through each line until we find the properties tag
                For x = 0 To XmlReaderLines.Count - 1
                    If XmlReaderLines(x).EndsWith("<Properties>") Then
                        ' Go through each line until we find the display name
                        For y = x To XmlReaderLines.Count - 1
                            If XmlReaderLines(y).Replace("<", "").Trim().Replace(">", "").Trim().Replace(" ", "").Trim().StartsWith("DisplayName", StringComparison.OrdinalIgnoreCase) Then
                                DynaLog.LogMessage("Line " & y + 1 & " contains display name information. Grabbing display name...")
                                pkgName = XmlReaderLines(y).Replace("<DisplayName>", "").Trim().Replace("</DisplayName>", "").Trim()
                                DynaLog.LogMessage("Deleting AppX manifest...")
                                File.Delete(CopiedAppxFilePath)
                                DynaLog.LogMessage("Package display name: " & pkgName)
                                DynaLog.LogMessage("Name treatment will be done if needed.")
                                pkgName = pkgName.Replace("&amp;", "&").Replace("&quot;", ControlChars.Quote).Replace("&gt;", ">").Replace("&lt;", "<")
                            End If
                        Next
                    End If
                Next
            Else
                If Directory.GetDirectories(PackageRootPath, String.Format("{0}*", DisplayName), SearchOption.TopDirectoryOnly).Count > 1 Then
                    DynaLog.LogMessage("No AppX manifests exist in the main directory, but there are multiple variations of the package (some architecture-neutral, others architecture-specific)")
                    DynaLog.LogMessage("We will take into account the architecture-specific package...")
                    ' Skip architecture neutral packages
                    DynaLog.LogMessage("Getting directories...")
                    Dim pkgDirs() As String = Directory.GetDirectories(PackageRootPath, String.Format("{0}*", DisplayName), SearchOption.TopDirectoryOnly)
                    DynaLog.LogMessage("Total amount of directories: " & pkgDirs.Count())
                    For Each folder In pkgDirs
                        If Not folder.Contains("neutral") Then
                            DynaLog.LogMessage("We have a possible folder candidate. Checking if AppX manifest exists here...")
                            If Not File.Exists(folder & "\AppxManifest.xml") Then Continue For
                            DynaLog.LogMessage("An AppX manifest exists here. Copying and reading...")
                            ' Copy manifest to startup dir
                            File.Copy(folder & "\AppxManifest.xml", CopiedAppxFilePath, True)
                            Dim XmlReaderLines As String() = File.ReadAllLines(CopiedAppxFilePath)
                            ' Go through each line until we find the properties tag
                            For x = 0 To XmlReaderLines.Count - 1
                                If XmlReaderLines(x).EndsWith("<Properties>") Then
                                    ' Go through each line until we find the display name
                                    For y = x To XmlReaderLines.Count - 1
                                        If XmlReaderLines(y).Replace("<", "").Trim().Replace(">", "").Trim().Replace(" ", "").Trim().StartsWith("DisplayName", StringComparison.OrdinalIgnoreCase) Then
                                            DynaLog.LogMessage("Line " & y + 1 & " contains display name information. Grabbing display name...")
                                            pkgName = XmlReaderLines(y).Replace("<DisplayName>", "").Trim().Replace("</DisplayName>", "").Trim()
                                            DynaLog.LogMessage("Deleting AppX manifest...")
                                            File.Delete(CopiedAppxFilePath)
                                            DynaLog.LogMessage("Package display name: " & pkgName)
                                            DynaLog.LogMessage("Name treatment will be done if needed.")
                                            pkgName = pkgName.Replace("&amp;", "&").Replace("&quot;", ControlChars.Quote).Replace("&gt;", ">").Replace("&lt;", "<")
                                        End If
                                    Next
                                End If
                            Next
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            DynaLog.LogMessage("Could not grab AppX package display name. Error message: " & ex.Message)
        End Try

        If pkgName.StartsWith("ms-resource:") Then
            DynaLog.LogMessage("Name starts with resource PRI identifier. Parsing...")
            pkgName = PriReader.ReadFromPri(String.Format("{0}\{1}", PackageRootPath, PackageName), DisplayName, pkgName)
            If pkgName = "" Then pkgName = DisplayName
        End If

        Return pkgName
    End Function

    Public Function IsPackageRegistered(MountDirectory As String, imgAppxPackage As Object) As Boolean
        If PackageRepositoryRootPath = "" Then
            DynaLog.LogMessage("Getting package repository root path...")
            PackageRepositoryRootPath = GetAppxPackageRepositoryRootPath(MountDirectory)
            If PackageRepositoryRootPath = "" Then PackageRepositoryRootPath = String.Format("{0}\ProgramData\Microsoft\Windows\AppRepository", MountDirectory)
            DynaLog.LogMessage("AppX Package Repository Root: " & PackageRepositoryRootPath)
        End If

        If imgAppxPackage Is Nothing Then Return False

        Dim PackageEntryInRepository As String = ""
        If TypeOf (imgAppxPackage) Is ImageAppxPackage Then
            PackageEntryInRepository = Path.Combine(PackageRepositoryRootPath, "Packages", CType(imgAppxPackage, ImageAppxPackage).PackageFullName)
        ElseIf TypeOf (imgAppxPackage) Is DismAppxPackage Then
            PackageEntryInRepository = Path.Combine(PackageRepositoryRootPath, "Packages", CType(imgAppxPackage, DismAppxPackage).PackageName)
        End If

        If Directory.Exists(PackageEntryInRepository) Then
            Return Directory.EnumerateFiles(PackageEntryInRepository, "*.pckgdep", SearchOption.TopDirectoryOnly).Any()
        Else
            Return False
        End If
    End Function

End Module
