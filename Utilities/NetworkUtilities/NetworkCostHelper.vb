Imports Microsoft.Win32

Namespace Utilities.NetworkUtilities

    Public Class NetworkCostHelper

        Private Enum NetworkCostStatus As Integer
            Undetermined = -1
            Unmetered = 0
            Metered = 2
        End Enum

        Private Shared Function GetNetworkCostStatus(InterfaceIndex As Integer) As NetworkCostStatus
            ' dusmsvc does not exist on 8.1 and earlier; do not run and say it's unmetered
            If Environment.OSVersion.Version.Major < 10 Then Return NetworkCostStatus.Unmetered

            Try
                If InterfaceIndex < 0 Then
                    ' Get the interface index of the current network connection so we can work with it
                    Dim ipRouteTableMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery("SELECT InterfaceIndex FROM Win32_IP4RouteTable WHERE Destination = '0.0.0.0'")
                    InterfaceIndex = WMIHelper.GetObjectValue(ipRouteTableMOC(0), "InterfaceIndex")
                End If

                ' Time to get some things
                Dim settingIdMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery(String.Format("SELECT SettingID FROM Win32_NetworkAdapterConfiguration WHERE InterfaceIndex = {0}", InterfaceIndex))
                Dim settingId As String = WMIHelper.GetObjectValue(settingIdMOC(0), "SettingID")

                Dim profilePath As String = String.Format("SOFTWARE\Microsoft\DusmSvc\Profiles\{0}", settingId)
                Dim profileKey As RegistryKey = Registry.LocalMachine.OpenSubKey(profilePath, False)
                Dim profileStatus As NetworkCostStatus = NetworkCostStatus.Undetermined

                If profileKey.GetSubKeyNames().Any() Then
                    For Each profile In profileKey.GetSubKeyNames()
                        Dim profileSubKey As RegistryKey = profileKey.OpenSubKey(profile)
                        profileStatus = profileSubKey.GetValue("UserCost", 0)
                        profileSubKey.Close()
                    Next
                Else
                    profileStatus = NetworkCostStatus.Unmetered
                End If

                profileKey.Close()
                Return profileStatus
            Catch ex As Exception
                DynaLog.LogMessage("Could not get user cost.")
                Return NetworkCostStatus.Undetermined
            End Try
        End Function

        Public Shared Function IsNetworkConnectionMetered() As Boolean
            Return GetNetworkCostStatus(-1) = NetworkCostStatus.Metered
        End Function

        Public Shared Function IsNetworkConnectionMetered(InterfaceIndex As Integer) As Boolean
            Return GetNetworkCostStatus(InterfaceIndex) = NetworkCostStatus.Metered
        End Function

    End Class

End Namespace
