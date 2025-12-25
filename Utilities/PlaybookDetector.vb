Imports System.IO
Imports Microsoft.VisualBasic.ControlChars

Public Class PlaybookDetector

    Public Enum VerifiedPlaybooks
        AtlasOS
        ReviOS
        AME
    End Enum

    Public Shared Function DetectInstalledPlaybook(InstalledPlaybook As VerifiedPlaybooks) As Boolean
        ' The Playbook detector will detect any verified AME Playbook and show a warning. We don't know
        ' if these things cause things to break in DT, so use DynaLog logging to check, in order to
        ' facilitate software issue isolation
        Dim OSPackageCount As Integer = 0
        Select Case InstalledPlaybook
            Case VerifiedPlaybooks.AtlasOS
                ' Detect content for Atlas OS
                DynaLog.LogMessage("Detecting if Atlas OS is installed...")
                Try
                    OSPackageCount = (Directory.GetFiles((Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\servicing\packages"), "z-Atlas-NoTelemetry-Package*", SearchOption.TopDirectoryOnly).Count)
                    DynaLog.LogMessage("Number of Atlas OS packages in system servicing folder: " & OSPackageCount)
                Catch ex As Exception
                    DynaLog.LogMessage("Could not detect presence of Atlas OS on this system. Error information:" & CrLf & CrLf & ex.ToString())
                    OSPackageCount = 0
                End Try
            Case VerifiedPlaybooks.ReviOS
                ' Detect content for Revision
                DynaLog.LogMessage("Detecting if Revision is installed...")
                Try
                    OSPackageCount = (Directory.GetFiles((Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\servicing\packages"), "Revision-ReviOS-SystemPackages-Removal*", SearchOption.TopDirectoryOnly).Count)
                    DynaLog.LogMessage("Number of Revision packages in system servicing folder: " & OSPackageCount)
                Catch ex As Exception
                    DynaLog.LogMessage("Could not detect presence of Revision on this system. Error information:" & CrLf & CrLf & ex.ToString())
                    OSPackageCount = 0
                End Try
            Case VerifiedPlaybooks.AME
                ' Detect content for standard AME Playbooks
                DynaLog.LogMessage("Detecting if AME 10/11 is installed...")
                Try
                    OSPackageCount = (Directory.GetFiles((Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\servicing\packages"), "Z-AME-NoDefender-Package*", SearchOption.TopDirectoryOnly).Count)
                    DynaLog.LogMessage("Number of AME packages in system servicing folder: " & OSPackageCount)
                Catch ex As Exception
                    DynaLog.LogMessage("Could not detect presence of AME 10/11 on this system. Error information:" & CrLf & CrLf & ex.ToString())
                    OSPackageCount = 0
                End Try
        End Select
        Return (OSPackageCount > 0)
    End Function

End Class
