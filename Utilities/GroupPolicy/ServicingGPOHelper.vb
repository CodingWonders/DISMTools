Imports Microsoft.Win32

Module ServicingGPOHelper

    ''' <summary>
    ''' Detects the source for optional feature installs and component repairs from the "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Servicing" registry key
    ''' </summary>
    ''' <returns>Returns GPOSource as the aforementioned source if this function runs correctly. Otherwise, it returns Nothing</returns>
    ''' <remarks>"LocalSourcePath" is updated every time a source is specified in the group policy editor. "GPOSource" pulls the value from "LocalSourcePath", which can be a local folder, a remote server or a Windows image (if it begins with "wim:\")</remarks>
    Function GetSrcFromGPO() As String
        Try
            DynaLog.LogMessage("Getting source of features and component repairs from Group Policy Object...")
            Dim GPOSourceRk As RegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Servicing", False)
            Dim GPOSource As String = GPOSourceRk.GetValue("LocalSourcePath", "")
            GPOSourceRk.Close()
            DynaLog.LogMessage("Obtained source: " & GPOSource)
            Return GPOSource
        Catch ex As Exception
            DynaLog.LogMessage("An error occurred while getting GPO source. Error message: " & ex.Message)
            DynaLog.LogMessage("This could be either because no group policy has been set, or because something is seriously wrong with your system")
            Return Nothing
        End Try
    End Function

End Module
