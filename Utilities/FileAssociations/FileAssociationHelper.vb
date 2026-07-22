Imports System.Runtime.InteropServices
Imports Microsoft.Win32

Module FileAssociationHelper

    Private NotInheritable Class NativeMethods

        <DllImport("shell32.dll")>
        Public Shared Sub SHChangeNotify(wEventId As UInteger, uFlags As UInteger, dwItem1 As IntPtr, dwItem2 As IntPtr)
        End Sub

    End Class

    Private Const SHCNE_ASSOCCHANGED As Integer = &H8000000
    Private Const SHCNF_IDLIST As Integer = 0
    Private Const UserClassesRegistryPath As String = "Software\Classes"

    Private Sub RefreshAfterAssociationChange()
        NativeMethods.SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero)
    End Sub

    Private Function SetFileAssociations(FileExtension As String, FileType As String, AssociationCommand As String, AssociationDescription As String, Optional AssociationIconPath As String = "", Optional ForceIconReset As Boolean = False) As Boolean
        If String.IsNullOrEmpty(FileExtension) Then Throw New ArgumentNullException(FileExtension)
        If String.IsNullOrEmpty(FileType) Then Throw New ArgumentNullException(FileType)
        If String.IsNullOrEmpty(AssociationCommand) Then Throw New ArgumentNullException(AssociationCommand)
        If Not String.IsNullOrWhiteSpace(AssociationIconPath) AndAlso Not File.Exists(AssociationIconPath) Then
            DynaLog.LogMessage("The requested association icon does not exist: " & AssociationIconPath)
            Return False
        End If

        Try
            DynaLog.LogMessage("Registering per-user file association for extension " & FileExtension & "...")
            Using userClasses As RegistryKey = Registry.CurrentUser.CreateSubKey(UserClassesRegistryPath)
                Using extensionKey As RegistryKey = userClasses.CreateSubKey(FileExtension)
                    extensionKey.SetValue(Nothing, FileType, RegistryValueKind.String)
                    Using openWithKey As RegistryKey = extensionKey.CreateSubKey("OpenWithProgids")
                        openWithKey.SetValue(FileType, "", RegistryValueKind.String)
                    End Using
                End Using

                Using fileTypeKey As RegistryKey = userClasses.CreateSubKey(FileType)
                    fileTypeKey.SetValue(Nothing, AssociationDescription, RegistryValueKind.String)
                    Using commandKey As RegistryKey = fileTypeKey.CreateSubKey("Shell\Open\Command")
                        commandKey.SetValue(Nothing, AssociationCommand, RegistryValueKind.String)
                    End Using

                    If Not String.IsNullOrWhiteSpace(AssociationIconPath) Then
                        Using iconKey As RegistryKey = fileTypeKey.CreateSubKey("DefaultIcon")
                            iconKey.SetValue(Nothing, AssociationIconPath, RegistryValueKind.String)
                        End Using
                    ElseIf ForceIconReset Then
                        fileTypeKey.DeleteSubKeyTree("DefaultIcon", False)
                    End If
                End Using
            End Using

            RefreshAfterAssociationChange()
            DynaLog.LogMessage("The per-user file association was registered successfully.")
            Return True
        Catch ex As Exception
            DynaLog.LogMessage("Could not register the per-user file association. Error message: " & ex.Message)
            Return False
        End Try
    End Function

    Public Function SetFileAssociation(Extension As String, Type As String, Command As String, Description As String) As Boolean
        Return SetFileAssociations(Extension, Type, Command, Description)
    End Function

    Public Function SetFileAssociation(Extension As String, Type As String, Command As String, Description As String, IconPath As String, Optional ForceIconReset As Boolean = False) As Boolean
        Return SetFileAssociations(Extension, Type, Command, Description, IconPath, ForceIconReset)
    End Function

    Private Function RemoveAssociations(FileExtension As String, FileType As String) As Boolean
        If String.IsNullOrEmpty(FileExtension) Then Throw New ArgumentNullException(FileExtension)
        If String.IsNullOrEmpty(FileType) Then Throw New ArgumentNullException(FileType)

        Try
            DynaLog.LogMessage("Removing per-user file association for extension " & FileExtension & "...")
            Using userClasses As RegistryKey = Registry.CurrentUser.CreateSubKey(UserClassesRegistryPath)
                Dim removeEmptyExtensionKey As Boolean = False
                Using extensionKey As RegistryKey = userClasses.OpenSubKey(FileExtension, True)
                    If extensionKey IsNot Nothing Then
                        Dim registeredType As Object = extensionKey.GetValue(Nothing)
                        If registeredType IsNot Nothing AndAlso registeredType.ToString().Equals(FileType, StringComparison.OrdinalIgnoreCase) Then
                            extensionKey.DeleteValue("", False)
                        End If
                        Using openWithKey As RegistryKey = extensionKey.OpenSubKey("OpenWithProgids", True)
                            If openWithKey IsNot Nothing Then
                                openWithKey.DeleteValue(FileType, False)
                                If openWithKey.GetValueNames().Length = 0 AndAlso openWithKey.GetSubKeyNames().Length = 0 Then
                                    openWithKey.Close()
                                    extensionKey.DeleteSubKeyTree("OpenWithProgids", False)
                                End If
                            End If
                        End Using
                        removeEmptyExtensionKey = extensionKey.GetValueNames().Length = 0 AndAlso
                                                  extensionKey.GetSubKeyNames().Length = 0
                    End If
                End Using
                If removeEmptyExtensionKey Then userClasses.DeleteSubKeyTree(FileExtension, False)
                userClasses.DeleteSubKeyTree(FileType, False)
            End Using

            RefreshAfterAssociationChange()
            DynaLog.LogMessage("The per-user file association was removed successfully.")
            Return True
        Catch ex As Exception
            DynaLog.LogMessage("Could not remove the per-user file association. Error message: " & ex.Message)
            Return False
        End Try
    End Function

    Public Function RemoveFileAssociation(Extension As String, Type As String) As Boolean
        Return RemoveAssociations(Extension, Type)
    End Function

    Public Function GetFileAssociationCmdline(FileType As String) As String
        If String.IsNullOrWhiteSpace(FileType) Then Return ""

        Dim associationRegistryPath As String = String.Format("{0}\Shell\Open\Command", FileType)
        Dim cmdline As String = ""

        Try
            Using assocRk As RegistryKey = Registry.ClassesRoot.OpenSubKey(associationRegistryPath, False)
                If assocRk Is Nothing Then
                    DynaLog.LogMessage("File association command registry key was not found: HKCR\" & associationRegistryPath)
                    Return cmdline
                End If

                Dim cmdlineValue As Object = assocRk.GetValue(Nothing)
                If cmdlineValue IsNot Nothing Then
                    cmdline = cmdlineValue.ToString()
                End If
            End Using
        Catch ex As Exception
            DynaLog.LogMessage("Could not get association. Error message: " & ex.Message)
        End Try

        Return cmdline
    End Function

    Public Function GetFileAssociationIconPath(FileType As String) As String
        If String.IsNullOrWhiteSpace(FileType) Then Return ""

        Dim associationIconRegistryPath As String = String.Format("{0}\DefaultIcon", FileType)
        Dim iconPath As String = ""

        Try
            Using defIconRk As RegistryKey = Registry.ClassesRoot.OpenSubKey(associationIconRegistryPath, False)
                If defIconRk Is Nothing Then
                    DynaLog.LogMessage("File association icon registry key was not found: HKCR\" & associationIconRegistryPath)
                    Return iconPath
                End If

                Dim iconPathValue As Object = defIconRk.GetValue(Nothing)
                If iconPathValue IsNot Nothing Then
                    iconPath = iconPathValue.ToString()
                End If
            End Using
        Catch ex As Exception
            DynaLog.LogMessage("Could not get association icon. Error message: " & ex.Message)
        End Try

        Return iconPath
    End Function

End Module
