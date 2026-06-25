Namespace Utilities

    Public Class WMIDiskHelper

        Public Shared Function DriveIdExists(DriveId As String) As Boolean
            Dim DriveMO As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT * FROM Win32_DiskDrive WHERE DeviceID LIKE {0}{1}{0}", Quote, WMIHelper.GetEscapedValue(DriveId)))
            Return DriveMO IsNot Nothing AndAlso DriveMO.Count > 0
        End Function

    End Class

End Namespace
