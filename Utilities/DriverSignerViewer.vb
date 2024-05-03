Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME

Namespace Utilities

    Public Class DriverSignerViewer

        Friend NotInheritable Class NativeMethods

            Public Sub New()
            End Sub

            <DllImport("setupapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
            Shared Function SetupVerifyInfFile(infName As String, altPlatformInfo As IntPtr, ByRef infSignerInfo As SP_INF_SIGNER_INFO) As Boolean
            End Function

        End Class

        Friend Const MAX_PATH As Integer = 260

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Friend Structure SP_INF_SIGNER_INFO
            Public cbSize As Integer
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=MAX_PATH)>
            Public CatalogFile As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=MAX_PATH)>
            Public DigitalSigner As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=MAX_PATH)>
            Public DigitalSignerVersion As String
            Public SignerScore As Integer
        End Structure

        Public Shared Function GetSignerInfo(drvPath As String) As String
            Const ERROR_AUTHENTICODE_TRUSTED_PUBLISHER As Integer = CInt(&HE0000241)
            Const ERROR_AUTHENTICODE_TRUST_NOT_ESTABLISHED As Integer = CInt(&HE0000242)

            Dim signerInfo As SP_INF_SIGNER_INFO = New SP_INF_SIGNER_INFO With {
                .cbSize = Marshal.SizeOf(GetType(SP_INF_SIGNER_INFO))
            }

            If NativeMethods.SetupVerifyInfFile(drvPath, IntPtr.Zero, signerInfo) OrElse
                (Marshal.GetLastWin32Error() = ERROR_AUTHENTICODE_TRUSTED_PUBLISHER OrElse
                 Marshal.GetLastWin32Error() = ERROR_AUTHENTICODE_TRUST_NOT_ESTABLISHED) AndAlso
             Not String.IsNullOrEmpty(signerInfo.DigitalSigner) Then
                Return signerInfo.DigitalSigner
            Else
                Return String.Empty
            End If

            Return Nothing
        End Function

    End Class

End Namespace
