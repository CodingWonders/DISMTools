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

    Private Sub RefreshAfterAssociationChange()
        NativeMethods.SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero)
    End Sub

    Private Function SetFileAssociations(FileExtension As String, FileType As String, AssociationCommand As String, AssociationDescription As String, Optional AssociationIconPath As String = "") As Boolean
        If String.IsNullOrEmpty(FileExtension) Then Throw New ArgumentNullException(FileExtension)
        If String.IsNullOrEmpty(FileType) Then Throw New ArgumentNullException(FileType)
        If String.IsNullOrEmpty(AssociationCommand) Then Throw New ArgumentNullException(AssociationCommand)

        Using cmdProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "cmd.exe"),
                .CreateNoWindow = Not Debugger.IsAttached,
                .WindowStyle = If(Debugger.IsAttached, ProcessWindowStyle.Normal, ProcessWindowStyle.Hidden)
            }
        }
            cmdProc.StartInfo.Arguments = String.Format("/c assoc {0}={1}", FileExtension, FileType)
            cmdProc.Start()
            cmdProc.WaitForExit()
            If cmdProc.ExitCode <> 0 Then Return False

            ' Set the file type itself
            cmdProc.StartInfo.Arguments = String.Format("/c ftype {0}={1}", FileType, AssociationCommand)
            cmdProc.Start()
            cmdProc.WaitForExit()
            If cmdProc.ExitCode <> 0 Then Return False

            ' Set the description for the association
            Try
                Dim AssocRk As RegistryKey = Registry.ClassesRoot.OpenSubKey(FileType, True)
                AssocRk.SetValue(Nothing, AssociationDescription, RegistryValueKind.String)
                If AssociationIconPath <> "" AndAlso File.Exists(AssociationIconPath) Then
                    ' We have defined an icon for this association; use it or else we'll have a default file icon.
                    Dim DefaultAssociationIcon As RegistryKey = AssocRk.OpenSubKey("DefaultIcon", True)
                    DefaultAssociationIcon.SetValue(Nothing, AssociationIconPath, RegistryValueKind.String)
                    DefaultAssociationIcon.Close()
                End If
                AssocRk.Close()
            Catch ex As Exception

            End Try

            ' Force a refresh of the icons
            RefreshAfterAssociationChange()
        End Using
        Return True
    End Function

    Public Function SetFileAssociation(Extension As String, Type As String, Command As String, Description As String) As Boolean
        Return SetFileAssociations(Extension, Type, Command, Description)
    End Function

    Public Function SetFileAssociation(Extension As String, Type As String, Command As String, Description As String, IconPath As String) As Boolean
        Return SetFileAssociations(Extension, Type, Command, Description, IconPath)
    End Function

    Private Function RemoveAssociations(FileExtension As String, FileType As String) As Boolean
        If String.IsNullOrEmpty(FileExtension) Then Throw New ArgumentNullException(FileExtension)
        If String.IsNullOrEmpty(FileType) Then Throw New ArgumentNullException(FileType)

        Using cmdProc As New Process() With {
            .StartInfo = New ProcessStartInfo() With {
                .FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "cmd.exe"),
                .CreateNoWindow = Not Debugger.IsAttached,
                .WindowStyle = If(Debugger.IsAttached, ProcessWindowStyle.Normal, ProcessWindowStyle.Hidden)
            }
        }
            cmdProc.StartInfo.Arguments = String.Format("/c assoc {0}=", FileExtension)
            cmdProc.Start()
            cmdProc.WaitForExit()
            If cmdProc.ExitCode <> 0 Then Return False

            ' Set the file type itself
            cmdProc.StartInfo.Arguments = String.Format("/c ftype {0}=", FileType)
            cmdProc.Start()
            cmdProc.WaitForExit()
            If cmdProc.ExitCode <> 0 Then Return False

            RegistryHelper.RemoveRegistryItem(String.Format("HKCR\{0}", FileType), "/f")

            ' Force a refresh of the icons
            RefreshAfterAssociationChange()
        End Using
        Return True
    End Function

    Public Function RemoveFileAssociation(Extension As String, Type As String) As Boolean
        Return RemoveAssociations(Extension, Type)
    End Function

    Public Function GetFileAssociationCmdline(FileType As String) As String
        Dim assocRk As RegistryKey = Nothing
        Dim cmdline As String = ""
        Try
            assocRk = Registry.ClassesRoot.OpenSubKey(String.Format("{0}\Shell\Open\Command", FileType), False)
            cmdline = assocRk.GetValue(Nothing, "")
        Catch ex As Exception
            DynaLog.LogMessage("Could not get association. Error message: " & ex.Message)
        Finally
            If assocRk IsNot Nothing Then assocRk.Close()
        End Try
        Return cmdline
    End Function

End Module
