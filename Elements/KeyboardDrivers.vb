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

        Public Shared Function GetKeyboardDriver(mountDir As String) As LayeredKeyboardDriver
            If (mountDir <> "") AndAlso (Directory.Exists(mountDir)) Then
                Try
                    ' Load the registry key
                    Dim RegProc As New Process()
                    RegProc.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\reg.exe"
                    RegProc.StartInfo.Arguments = "load HKLM\zSYS " & Quote & mountDir & "\Windows\system32\config\SYSTEM" & Quote
                    If Not Debugger.IsAttached Then
                        RegProc.StartInfo.CreateNoWindow = True
                        RegProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    End If
                    RegProc.Start()
                    RegProc.WaitForExit()
                    If RegProc.ExitCode <> 0 Then
                        Throw New Exception("Process exited with code " & RegProc.ExitCode)
                    End If
                    ' Grab override keyboard type from registry
                    Dim OverrideKeybReg As RegistryKey = Registry.LocalMachine.OpenSubKey("zSYS\ControlSet001\Services\i8042prt\Parameters", False)
                    Dim OverrideKeybID As String = OverrideKeybReg.GetValue("OverrideKeyboardIdentifier")
                    OverrideKeybReg.Close()
                    ' Unload image registry
                    RegProc.StartInfo.Arguments = "unload HKLM\zSYS"
                    RegProc.Start()
                    RegProc.WaitForExit()
                    If RegProc.ExitCode <> 0 Then
                        Throw New Exception("Process exited with code " & RegProc.ExitCode)
                    End If
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
