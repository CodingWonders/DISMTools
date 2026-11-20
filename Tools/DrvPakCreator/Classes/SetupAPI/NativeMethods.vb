Imports System.Runtime.InteropServices
Imports System.Text

Namespace Classes.SetupAPI

    Friend NotInheritable Class NativeMethods

        Private Sub New()
        End Sub

        Friend Const INF_STYLE_OLDNT As UInteger = &H1UI
        Friend Const INF_STYLE_WIN4 As UInteger = &H2UI

        <StructLayout(LayoutKind.Sequential)> _
        Friend Structure INFCONTEXT
            Public Inf As IntPtr
            Public CurrentInf As IntPtr
            Public Line As UInteger
            Public Reserved As UInteger
        End Structure

        <DllImport("setupapi.dll", EntryPoint:="SetupOpenInfFileW", CharSet:=CharSet.Unicode, SetLastError:=True)> _
        Friend Shared Function SetupOpenInfFile(ByVal FileName As String, ByVal InfClass As String, ByVal InfStyle As UInteger, ByRef ErrorLine As UInteger) As IntPtr
        End Function

        <DllImport("setupapi.dll", EntryPoint:="SetupCloseInfFile", SetLastError:=True)> _
        Friend Shared Sub SetupCloseInfFile(ByVal InfHandle As IntPtr)
        End Sub

        <DllImport("setupapi.dll", EntryPoint:="SetupFindFirstLineW", CharSet:=CharSet.Unicode, SetLastError:=True)> _
        Friend Shared Function SetupFindFirstLine(ByVal InfHandle As IntPtr, ByVal Section As String, ByVal Key As String, ByRef Context As INFCONTEXT) As Boolean
        End Function

        <DllImport("setupapi.dll", EntryPoint:="SetupFindNextLine", SetLastError:=True)> _
        Friend Shared Function SetupFindNextLine(ByRef ContextIn As INFCONTEXT, ByRef ContextOut As INFCONTEXT) As Boolean
        End Function

        <DllImport("setupapi.dll", EntryPoint:="SetupGetFieldCount", SetLastError:=True)> _
        Friend Shared Function SetupGetFieldCount(ByRef Context As INFCONTEXT) As UInteger
        End Function

        <DllImport("setupapi.dll", EntryPoint:="SetupGetStringFieldW", CharSet:=CharSet.Unicode, SetLastError:=True)> _
        Friend Shared Function SetupGetStringField(ByRef Context As INFCONTEXT, ByVal FieldIndex As UInteger, ByVal ReturnBuffer As StringBuilder, ByVal ReturnBufferSize As UInteger, ByRef RequiredSize As UInteger) As Boolean
        End Function

        <DllImport("setupapi.dll", EntryPoint:="SetupDiGetINFClassW", CharSet:=CharSet.Unicode, SetLastError:=True)> _
        Friend Shared Function SetupDiGetINFClass(ByVal InfName As String, ByRef ClassGuid As Guid, ByVal ClassName As StringBuilder, ByVal ClassNameSize As UInteger, ByRef RequiredSize As UInteger) As Boolean
        End Function

        <DllImport("setupapi.dll", EntryPoint:="SetupGetInfDriverStoreLocationW", CharSet:=CharSet.Unicode, SetLastError:=True)> _
        Friend Shared Function SetupGetInfDriverStoreLocation(ByVal FileName As String, ByVal AlternatePlatformInfo As IntPtr, ByVal LocaleName As String, ByVal ReturnBuffer As StringBuilder, ByVal ReturnBufferSize As Integer, ByRef RequiredSize As Integer) As Boolean
        End Function

    End Class

End Namespace