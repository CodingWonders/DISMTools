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

        ' Windows can send several user preference notifications in a row (for example, when switching between light and dark
        ' modes). This timer, which lives on the UI thread, makes the program apply the color theme only once, after they stop
        Private themeChangeTimer As System.Windows.Forms.Timer
        Private Const ThemeChangeDelayMs As Integer = 500
        ' Last value of AppsUseLightTheme that was applied (-1: unknown)
        Private lastAppliedLightTheme As Integer = -1

        Private Sub Start(sender As Object, e As EventArgs) Handles Me.Startup
            LocalizationService.Initialize()
            lastAppliedLightTheme = GetAppsUseLightTheme()
            DynaLog.LogMessage("Adding startup event handlers...")
            AddHandler Microsoft.Win32.SystemEvents.UserPreferenceChanged, AddressOf SysEvts_UserPreferenceChanged
            AddHandler Microsoft.Win32.SystemEvents.DisplaySettingsChanging, AddressOf SysEvts_DisplaySettingsChanging
            AddHandler Microsoft.Win32.SystemEvents.DisplaySettingsChanged, AddressOf SysEvts_DisplaySettingsChanged
        End Sub

        Private Sub ShutdownApp(sender As Object, e As EventArgs) Handles Me.Shutdown
            ' SystemEvents handlers are static. Remove them so that nothing is called on forms that no longer exist
            RemoveHandler Microsoft.Win32.SystemEvents.UserPreferenceChanged, AddressOf SysEvts_UserPreferenceChanged
            RemoveHandler Microsoft.Win32.SystemEvents.DisplaySettingsChanging, AddressOf SysEvts_DisplaySettingsChanging
            RemoveHandler Microsoft.Win32.SystemEvents.DisplaySettingsChanged, AddressOf SysEvts_DisplaySettingsChanged
            If themeChangeTimer IsNot Nothing Then
                themeChangeTimer.Stop()
                themeChangeTimer.Dispose()
                themeChangeTimer = Nothing
            End If
        End Sub

        Private Sub CatchEmAll(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            DynaLog.LogMessage("Unhandled exception occurred. Gotta catch'em all !")
            DynaLog.LogMessage("Exception information:")
            DynaLog.LogMessage("- Message: " & e.Exception.Message)
            DynaLog.LogMessage("- Code (HRESULT): " & Hex(e.Exception.HResult))
            ExceptionForm.ErrorText.Text = e.Exception.ToString() & CrLf & CrLf &
                                           "Error Message: " & e.Exception.Message & CrLf & CrLf &
                                           "Error Code (HRESULT): " & Hex(e.Exception.HResult)
            Try
                DynaLog.LogMessage("Getting program information for exception report...")
                ' Get version of DISMTools that threw the exception. Include program version, branch, and (possibly) build time
                ' in the case of nightly installers
                ExceptionForm.ErrorText.AppendText(CrLf & CrLf &
                                                   "Program information:" & CrLf &
                                                   " - DISMTools Version: " & My.Application.Info.Version.ToString() & CrLf &
                                                   " - Preview release? " & If(DISMTools.MainForm.dtBranch.Contains("pre"), "Yes", "No") & CrLf &
                                                   " - Branch: " & DISMTools.MainForm.dtBranch & CrLf &
                                                   " - Build time: " & RetrieveLinkerTimestamp().ToString("yyMMdd-HHmm") & CrLf &
                                                   " - Portable copy? " & If(File.Exists(My.Application.Info.DirectoryPath & "\portable"), "Yes", "No") & CrLf)
                ' Get image information if a project has been loaded
                If DISMTools.MainForm.isProjectLoaded And Not DISMTools.MainForm.OnlineManagement Then
                    DynaLog.LogMessage("Getting information about the image/installation for exception report...")
                    Try
                        ExceptionForm.ErrorText.AppendText(CrLf &
                                                           "Information about the image loaded in this project:" & CrLf &
                                                           " - Image name: " & DISMTools.MainForm.Label46.Text & CrLf &
                                                           " - Image description: " & DISMTools.MainForm.Label47.Text & CrLf &
                                                           " - Image version: " & DISMTools.MainForm.Label48.Text)
                    Catch ex As Exception
                        DynaLog.LogMessage("Could not get image/installation information. Error message: " & ex.Message)
                        ' Don't get this info
                    End Try
                End If
                DynaLog.LogMessage("Getting computer, BIOS, OS and processor information for exception report...")
                DynaLog.LogMessage("--- NO PERSONALLY IDENTIFIABLE INFORMATION WILL BE INCLUDED IN THE REPORT ---")
                DynaLog.LogMessage("---- I ONLY GRAB THIS INFORMATION TO HELP ISOLATE BUGS TO A HW/SW CONFIG ----")
                DynaLog.LogMessage("I really care about your privacy and, like you, I hate tracking.")
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
                DynaLog.LogMessage("Could not get system information. Error message: " & ex.Message)
                ' Could not get basic machine information
            End Try
            Try
                DynaLog.LogMessage("Saving exception information to the error report...")
                If Not Directory.Exists(Path.Combine(Windows.Forms.Application.StartupPath, "logs", "errors")) Then
                    DynaLog.LogMessage("Creating directory for error reports...")
                    Directory.CreateDirectory(Path.Combine(Windows.Forms.Application.StartupPath, "logs", "errors"))
                End If
                File.WriteAllText(Path.Combine(Windows.Forms.Application.StartupPath, "logs", "errors") & "\DT-Error-" & Now.ToString().Replace("/", "-").Trim().Replace(":", "-").Trim() & ".log", ExceptionForm.ErrorText.Text, UTF8)
            Catch ex As Exception
                DynaLog.LogMessage("Could not create the error report. Error message: " & ex.Message)
                ' Could not save error information
            End Try
            ExceptionForm.ShowDialog()
            If ExceptionForm.DialogResult = DialogResult.OK Then
                DynaLog.LogMessage("DISMTools will continue execution as user decided to do so.")
                DynaLog.LogMessage("This is not the most recommended thing as it may happen again.")
                e.ExitApplication = False
            ElseIf ExceptionForm.DialogResult = DialogResult.Cancel Then
                DynaLog.LogMessage("DISMTools will stop execution as user decided to do so.")
                e.ExitApplication = True
            End If
        End Sub

        Private Sub SysEvts_UserPreferenceChanged(sender As Object, e As Microsoft.Win32.UserPreferenceChangedEventArgs)
            DynaLog.LogMessage("A user preference of type " & e.Category.ToString() & " has changed.")
            If e.Category <> UserPreferenceCategory.General Then Exit Sub

            ' This may run on the SystemEvents thread. Never touch the default instance here and never block:
            ' use the real form and queue the work on the UI thread.
            Dim uiForm As DISMTools.MainForm = DISMTools.MainForm.Current
            If uiForm Is Nothing OrElse uiForm.IsDisposed OrElse Not uiForm.IsHandleCreated Then Exit Sub

            Try
                uiForm.BeginInvoke(New MethodInvoker(Sub() ScheduleSystemThemeChange(uiForm)))
            Catch ex As InvalidOperationException
                ' The window handle was destroyed in the meantime (the program is closing)
            End Try
        End Sub

        Private Sub ScheduleSystemThemeChange(uiForm As DISMTools.MainForm)
            ' Runs on the UI thread
            If uiForm.IsDisposed OrElse uiForm.ColorMode <> 0 Then Exit Sub

            If themeChangeTimer Is Nothing Then
                themeChangeTimer = New System.Windows.Forms.Timer() With {.Interval = ThemeChangeDelayMs}
                AddHandler themeChangeTimer.Tick, AddressOf ThemeChangeTimer_Tick
            End If
            ' Restart the timer, so that a burst of notifications applies the theme only once
            themeChangeTimer.Stop()
            themeChangeTimer.Start()
        End Sub

        Private Sub ThemeChangeTimer_Tick(sender As Object, e As EventArgs)
            themeChangeTimer.Stop()
            Dim uiForm As DISMTools.MainForm = DISMTools.MainForm.Current
            If uiForm Is Nothing OrElse uiForm.IsDisposed OrElse uiForm.ColorMode <> 0 Then Exit Sub

            DynaLog.LogMessage("Detecting if the system color theme changed...")
            Dim appsUseLightTheme As Integer = GetAppsUseLightTheme()
            If appsUseLightTheme >= 0 AndAlso appsUseLightTheme = lastAppliedLightTheme Then
                DynaLog.LogMessage("The system color theme did not change.")
                Exit Sub
            End If
            DynaLog.LogMessage("Changing color theme...")
            uiForm.ChangePrgColors(0)
            lastAppliedLightTheme = appsUseLightTheme
        End Sub

        ' Gets the value of AppsUseLightTheme, or -1 if it can't be read
        Private Function GetAppsUseLightTheme() As Integer
            Try
                Using personalizeKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", False)
                    If personalizeKey Is Nothing Then Return -1
                    Dim value As Object = personalizeKey.GetValue("AppsUseLightTheme")
                    If value Is Nothing Then Return -1
                    Return CInt(value)
                End Using
            Catch ex As Exception
                Return -1
            End Try
        End Function

        Private Sub SysEvts_DisplaySettingsChanged(sender As Object, e As EventArgs)
            ' Do nothing
        End Sub

        Private Sub SysEvts_DisplaySettingsChanging(sender As Object, e As EventArgs)
            ' Do nothing
        End Sub

    End Class


End Namespace
