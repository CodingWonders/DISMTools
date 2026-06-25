Imports System.Runtime.InteropServices

Module PowerManagementHelper

    <Flags>
    Private Enum EXECUTION_STATE
        ES_CONTINUOUS = &H80000000
        ES_SYSTEM_REQUIRED = 1
        ES_DISPLAY_REQUIRED = 2
    End Enum

    Private NotInheritable Class NativeMethods

        <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
        Public Shared Function SetThreadExecutionState(esFlags As EXECUTION_STATE) As EXECUTION_STATE
        End Function

    End Class

    Public Sub DisableSystemSleepMode()
        NativeMethods.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS Or EXECUTION_STATE.ES_SYSTEM_REQUIRED Or EXECUTION_STATE.ES_DISPLAY_REQUIRED)
    End Sub

    Public Sub EnableSystemSleepMode()
        NativeMethods.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS)
    End Sub

End Module
