Imports Microsoft.Win32
Imports Microsoft.VisualBasic.ControlChars
Imports System.Management
Imports System.IO
Imports System.Text.Encoding

Namespace My

    ' Los siguientes eventos están disponibles para MyApplication:
    ' 
    ' Inicio: se desencadena cuando se inicia la aplicación, antes de que se cree el formulario de inicio.
    ' Apagado: generado después de cerrar todos los formularios de la aplicación. Este evento no se genera si la aplicación termina de forma anómala.
    ' UnhandledException: generado si la aplicación detecta una excepción no controlada.
    ' StartupNextInstance: se desencadena cuando se inicia una aplicación de instancia única y la aplicación ya está activa. 
    ' NetworkAvailabilityChanged: se desencadena cuando la conexión de red está conectada o desconectada.
    Partial Friend Class MyApplication

        Private Sub Start(sender As Object, e As EventArgs) Handles Me.Startup
            AddHandler Microsoft.Win32.SystemEvents.UserPreferenceChanged, AddressOf SysEvts_UserPreferenceChanged
            AddHandler Microsoft.Win32.SystemEvents.DisplaySettingsChanging, AddressOf SysEvts_DisplaySettingsChanging
            AddHandler Microsoft.Win32.SystemEvents.DisplaySettingsChanged, AddressOf SysEvts_DisplaySettingsChanged
        End Sub

        Private Sub CatchEmAll(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            ExceptionForm.ErrorText.Text = e.Exception.ToString() & CrLf & CrLf &
                                           "Error Message: " & e.Exception.Message & CrLf & CrLf &
                                           "Error Code (HRESULT): " & Hex(e.Exception.HResult)
            Try
                ' Get version of DISMTools that threw the exception. Include program version, branch, and (possibly) build time
                ' in the case of nightly installers
                ExceptionForm.ErrorText.AppendText(CrLf & CrLf &
                                                   "Program information:" & CrLf &
                                                   " - DISMTools Version: " & My.Application.Info.Version.ToString() & CrLf &
                                                   " - Preview release? " & If(DISMTools.MainForm.dtBranch.Contains("preview"), "Yes", "No") & CrLf &
                                                   " - Branch: " & DISMTools.MainForm.dtBranch & CrLf &
                                                   " - Build time: " & DISMTools.PrgAbout.RetrieveLinkerTimestamp(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe").ToString("yyMMdd-HHmm"))
                ' Get image information if a project has been loaded
                If DISMTools.MainForm.isProjectLoaded And Not DISMTools.MainForm.OnlineManagement Then
                    Try
                        ExceptionForm.ErrorText.AppendText(CrLf &
                                                           "Information about the image loaded in this project:" & CrLf &
                                                           " - Image name: " & DISMTools.MainForm.Label46.Text & CrLf &
                                                           " - Image description: " & DISMTools.MainForm.Label47.Text & CrLf &
                                                           " - Image version: " & DISMTools.MainForm.Label48.Text)
                    Catch ex As Exception
                        ' Don't get this info
                    End Try
                End If
                ' Get basic information about the system. This does not include any personally identifiable information (PII) or
                ' serial numbers that can identify the computer this program is run on
                Dim CS_Searcher As ManagementObjectSearcher = New ManagementObjectSearcher("SELECT Manufacturer, Model FROM Win32_ComputerSystem")
                Dim BIOS_Searcher As ManagementObjectSearcher = New ManagementObjectSearcher("SELECT Name, Description, SMBIOSBIOSVersion FROM Win32_BIOS")
                Dim Proc_Searcher As ManagementObjectSearcher = New ManagementObjectSearcher("SELECT Name, Caption, Manufacturer, Family FROM Win32_Processor")
                Dim CS_Results As ManagementObjectCollection = CS_Searcher.Get()
                Dim BIOS_Results As ManagementObjectCollection = BIOS_Searcher.Get()
                Dim Proc_Results As ManagementObjectCollection = Proc_Searcher.Get()
                ExceptionForm.ErrorText.AppendText(CrLf &
                                                   "Machine information:" & CrLf)
                For Each CS_Result As ManagementObject In CS_Results
                    ExceptionForm.ErrorText.AppendText(" - Computer manufacturer: " & CS_Result("Manufacturer") & CrLf &
                                                       " - Computer model: " & CS_Result("Model") & CrLf)
                Next
                For Each BIOS_Result As ManagementObject In BIOS_Results
                    ExceptionForm.ErrorText.AppendText(" - BIOS name/description: " & BIOS_Result("Name") & " " & BIOS_Result("Description") & CrLf &
                                                       " - System Management BIOS (SMBIOS) version: " & BIOS_Result("SMBIOSBIOSVersion") & CrLf & CrLf)
                Next
                ExceptionForm.ErrorText.AppendText("Operating system information:" & CrLf &
                                                   " - OS name: " & My.Computer.Info.OSFullName & CrLf &
                                                   " - OS version: " & My.Computer.Info.OSVersion & CrLf &
                                                   " - OS Platform: " & My.Computer.Info.OSPlatform & CrLf &
                                                   " - Is a 64-bit system? " & If(Environment.Is64BitOperatingSystem, "Yes", "No") & CrLf & CrLf &
                                                   "Processor information:" & CrLf)
                For Each Proc_Result As ManagementObject In Proc_Results
                    ExceptionForm.ErrorText.AppendText(" - Processor name: " & Proc_Result("Name") & CrLf &
                                                       " - Processor caption: " & Proc_Result("Caption") & CrLf &
                                                       " - Processor manufacturer: " & Proc_Result("Manufacturer") & CrLf &
                                                       " - Processor family (WMI type): " & Proc_Result("Family") & CrLf &
                                                       "   NOTE: refer to the following website to get the exact family type of your processor:" & CrLf &
                                                       "   https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-processor" & CrLf & CrLf)
                Next
                ExceptionForm.ErrorText.AppendText("This information is gathered to help isolate the issue to a specific hardware or software configuration. " &
                                                   "No information that can be used to identify the user or the exact system is gathered." & CrLf & CrLf &
                                                   "If you don't want to send this information to the developers, paste the text that was copied to the clipboard in a text editor, remove this information, and copy the new text again.")
            Catch ex As Exception
                ' Could not get basic machine information
            End Try
            Try
                If Not Directory.Exists(Path.Combine(Windows.Forms.Application.StartupPath, "logs", "errors")) Then
                    Directory.CreateDirectory(Path.Combine(Windows.Forms.Application.StartupPath, "logs", "errors"))
                End If
                File.WriteAllText(Path.Combine(Windows.Forms.Application.StartupPath, "logs", "errors") & "\DT-Error-" & Now.ToString().Replace("/", "-").Trim().Replace(":", "-").Trim() & ".log", ExceptionForm.ErrorText.Text, UTF8)
            Catch ex As Exception
                ' Could not save error information
            End Try
            ExceptionForm.ShowDialog()
            If ExceptionForm.DialogResult = DialogResult.OK Then
                e.ExitApplication = False
            ElseIf ExceptionForm.DialogResult = DialogResult.Cancel Then
                e.ExitApplication = True
            End If
        End Sub

        Private Sub SysEvts_UserPreferenceChanged(sender As Object, e As Microsoft.Win32.UserPreferenceChangedEventArgs)
            Debug.WriteLine(Date.UtcNow & " UTC - User Preference Category: " & e.Category.ToString())

            ' Prevent the program from freezing. This is a fix for a very long-standing bug that was introduced with the mounted image detector,
            ' where the program would randomly freeze and never come back. And, even while the program itself was still responding, its UI thread
            ' wasn't.
            '
            ' This is a problem with Windows Forms that has been present since .NET Framework 2.0. More on that here: 
            ' https://www.ikriv.com/dev/dotnet/MysteriousHang
            '
            ' Even though this bug took a year and a half to fix, a lot was gathered from previous attempts at fixing this problem:
            ' 1. It's random
            ' 2. It affects all versions of Windows supported by this program (Win8.1-Win11, incl. servers)
            ' 3. It happens when the following conditions occur:
            '    - The mounted image detector is running
            '    - This event is triggered
            '
            ' This fixes the problem by temporarily stopping the mounted image detector, doing the event code, and restarting it afterward.
            ' It hasn't caused any freezes for me yet, but I may be proven wrong.
            If DISMTools.MainForm.MountedImageDetectorBW.IsBusy Then
                DISMTools.MainForm.MountedImageDetectorBW.CancelAsync()
            End If
            Try
                Threading.Thread.Sleep(1000)
                Call DISMTools.MainForm.MountedImageDetectorBW.RunWorkerAsync()
                Threading.Thread.Sleep(250)
            Catch ex As Exception
                DISMTools.MainForm.MountedImageDetectorBWRestarterTimer.Enabled = True
            End Try
        End Sub

        Private Sub SysEvts_DisplaySettingsChanged(sender As Object, e As EventArgs)
            ' Do nothing
        End Sub

        Private Sub SysEvts_DisplaySettingsChanging(sender As Object, e As EventArgs)
            ' Do nothing
        End Sub

    End Class


End Namespace

