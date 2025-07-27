Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Win32

Namespace Elements

    Public Class KeyboardDrivers

        ''' <summary>
        ''' Layered keyboard drivers
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum LayeredKeyboardDriver

            ''' <summary>
            ''' No keyboard layered driver has been detected
            ''' </summary>
            ''' <remarks></remarks>
            Unknown = 0

            ''' <summary>
            ''' Layered driver: PC/AT Enhanced Keyboard (101/102-Key)
            ''' </summary>
            ''' <remarks></remarks>
            PCATKey = 1

            ''' <summary>
            ''' Layered driver: Korean PC/AT 101-Key Compatible Keyboard/MS Natural Keyboard (Type 1)
            ''' </summary>
            ''' <remarks></remarks>
            K_PCATKeyT1 = 2

            ''' <summary>
            ''' Layered driver: Korean PC/AT 101-Key Compatible Keyboard/MS Natural Keyboard (Type 2)
            ''' </summary>
            ''' <remarks></remarks>
            K_PCATKeyT2 = 3

            ''' <summary>
            ''' Layered driver: Korean PC/AT 101-Key Compatible Keyboard/MS Natural Keyboard (Type 3)
            ''' </summary>
            ''' <remarks></remarks>
            K_PCATKeyT3 = 4

            ''' <summary>
            ''' Layered driver: Korean Keyboard (103/106 Key)
            ''' </summary>
            ''' <remarks></remarks>
            K_103106Key = 5

            ''' <summary>
            ''' Layered driver: Japanese Keyboard (106/109 Key)
            ''' </summary>
            ''' <remarks></remarks>
            J_106109Key = 6
        End Enum

        Public Property LayeredDriver As LayeredKeyboardDriver

        Private keybCode As Integer

        Public Sub New(ByVal kCode As Integer)
            Select Case kCode
                Case 1
                    LayeredDriver = LayeredKeyboardDriver.PCATKey
                Case 2
                    LayeredDriver = LayeredKeyboardDriver.K_PCATKeyT1
                Case 3
                    LayeredDriver = LayeredKeyboardDriver.K_PCATKeyT2
                Case 4
                    LayeredDriver = LayeredKeyboardDriver.K_PCATKeyT3
                Case 5
                    LayeredDriver = LayeredKeyboardDriver.K_103106Key
                Case 6
                    LayeredDriver = LayeredKeyboardDriver.J_106109Key
            End Select
        End Sub

        Public Property keyboardCode As Integer
            Get
                Return keybCode
            End Get
            Set(value As Integer)
                keybCode = value
            End Set
        End Property

        Public Shared Function GetKeyboardDriver(mountDir As String, Optional onlineMode As Boolean = False) As LayeredKeyboardDriver
            DynaLog.LogMessage("Getting keyboard layered driver...")
            DynaLog.LogMessage("- Mount directory: " & Quote & mountDir & Quote)
            DynaLog.LogMessage("- Is active installation being managed? " & If(onlineMode, "Yes", "No"))
            If onlineMode Then
                DynaLog.LogMessage("Attempting to get the keyboard layered driver from the host system...")
                Try
                    ' Grab override keyboard type from registry
                    Dim OverrideKeybReg As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services\i8042prt\Parameters", False)
                    Dim OverrideKeybID As String = OverrideKeybReg.GetValue("OverrideKeyboardIdentifier")
                    OverrideKeybReg.Close()
                    ' Check keyboard ID
                    DynaLog.LogMessage("Override keyboard ID: " & OverrideKeybID)
                    DynaLog.LogMessage("Evaluating keyboard ID...")
                    Select Case OverrideKeybID
                        Case "PCAT_101KEY"
                            Return LayeredKeyboardDriver.PCATKey
                        Case "PCAT_101AKEY"
                            Return LayeredKeyboardDriver.K_PCATKeyT1
                        Case "PCAT_101BKEY"
                            Return LayeredKeyboardDriver.K_PCATKeyT2
                        Case "PCAT_101CKEY"
                            Return LayeredKeyboardDriver.K_PCATKeyT3
                        Case "PCAT_103KEY"
                            Return LayeredKeyboardDriver.K_103106Key
                        Case "PCAT_106KEY"
                            Return LayeredKeyboardDriver.J_106109Key
                    End Select
                Catch ex As Exception
                    DynaLog.LogMessage("Could not get keyboard layered driver. Error message: " & ex.Message)
                    MessageBox.Show(ex.Message)
                    Return LayeredKeyboardDriver.Unknown
                End Try
                Return LayeredKeyboardDriver.Unknown
            End If
            If (mountDir <> "") AndAlso (Directory.Exists(mountDir)) Then
                DynaLog.LogMessage("Attempting to get the keyboard layered driver from the specified Windows image...")
                Try
                    DynaLog.LogMessage("Loading image SYSTEM registry hive...")
                    ' Load the registry key
                    Dim regExitCode As Integer = RegistryHelper.LoadRegistryHive(Path.Combine(mountDir, "Windows", "system32", "config", "SYSTEM"), "HKLM\zSYS")
                    DynaLog.LogMessage("REG process exited with error code " & Hex(regExitCode))
                    If regExitCode <> 0 Then
                        Throw New Exception("Process exited with code " & regExitCode)
                    End If
                    ' Grab override keyboard type from registry
                    Dim OverrideKeybReg As RegistryKey = Registry.LocalMachine.OpenSubKey("zSYS\ControlSet001\Services\i8042prt\Parameters", False)
                    Dim OverrideKeybID As String = OverrideKeybReg.GetValue("OverrideKeyboardIdentifier")
                    OverrideKeybReg.Close()
                    DynaLog.LogMessage("Unloading image SYSTEM registry hive...")
                    ' Unload image registry
                    regExitCode = RegistryHelper.UnloadRegistryHive("HKLM\zSYS")
                    DynaLog.LogMessage("REG process exited with error code " & Hex(regExitCode))
                    If regExitCode <> 0 Then
                        Throw New Exception("Process exited with code " & regExitCode)
                    End If
                    DynaLog.LogMessage("Override keyboard ID: " & OverrideKeybID)
                    DynaLog.LogMessage("Evaluating keyboard ID...")
                    ' Check keyboard ID
                    Select Case OverrideKeybID
                        Case "PCAT_101KEY"
                            Return LayeredKeyboardDriver.PCATKey
                        Case "PCAT_101AKEY"
                            Return LayeredKeyboardDriver.K_PCATKeyT1
                        Case "PCAT_101BKEY"
                            Return LayeredKeyboardDriver.K_PCATKeyT2
                        Case "PCAT_101CKEY"
                            Return LayeredKeyboardDriver.K_PCATKeyT3
                        Case "PCAT_103KEY"
                            Return LayeredKeyboardDriver.K_103106Key
                        Case "PCAT_106KEY"
                            Return LayeredKeyboardDriver.J_106109Key
                    End Select
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                    Return LayeredKeyboardDriver.Unknown
                End Try
            End If
            Return LayeredKeyboardDriver.Unknown
        End Function

    End Class

End Namespace
