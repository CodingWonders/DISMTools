Imports System.IO
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Uri
Imports Microsoft.VisualBasic.ControlChars

Namespace Utilities

    ''' <summary>
    ''' This is a VB.NET version of the Store App Helper in Bulk Crap Uninstaller (https://github.com/klocman/Bulk-Crap-Uninstaller), adapted to only extract resources from PRI files. It makes use of the SHLoadIndirectString function in shlwapi.dll
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PriReader

        Friend NotInheritable Class NativeMethods

            Private Sub New()
            End Sub

            <DllImport("shlwapi.dll", BestFitMapping:=False, CharSet:=CharSet.Unicode, ExactSpelling:=True, SetLastError:=False, ThrowOnUnmappableChar:=True)>
            Shared Function SHLoadIndirectString(pszSource As String, pszOutBuf As StringBuilder, cchOutBuf As Integer, ppvReserved As IntPtr) As Integer
            End Function

            Shared Function ExtractStringFromPriFile(priFile As String, resKey As String) As String
                DynaLog.LogMessage("Preparing to load indirect string...")
                DynaLog.LogMessage("- PRI file: " & Quote & priFile & Quote)
                DynaLog.LogMessage("- Resource Key: " & Quote & resKey & Quote)
                Dim sWin8ManString = "@{" & priFile & "? " & resKey & "}"
                DynaLog.LogMessage("Manifest string to pass to Windows API: " & sWin8ManString)
                Dim outBuff As StringBuilder = New StringBuilder(1024)
                SHLoadIndirectString(sWin8ManString, outBuff, outBuff.Capacity, IntPtr.Zero)
                Return outBuff.ToString()
            End Function
        End Class

        ''' <summary>
        ''' Creates a path that can access the display name in the resources.pri file
        ''' </summary>
        ''' <param name="appDir">The installation path of an AppX package</param>
        ''' <param name="pkgName">The display name of the package (e.g. "SomeCompany.SomeApp")</param>
        ''' <param name="dispName">The friendly display name of the application (which, in this function, starts with "ms-resource:")</param>
        ''' <returns>The actual friendly display name of an application, stored in the PRI file</returns>
        ''' <remarks></remarks>
        Shared Function ReadFromPri(appDir As String, pkgName As String, dispName As String) As String
            DynaLog.LogMessage("Preparing to read PRI file...")
            DynaLog.LogMessage("- Installation path of AppX package: " & Quote & appDir & Quote)
            DynaLog.LogMessage("- Display name of AppX package: " & pkgName)
            DynaLog.LogMessage("- Friendly display name of AppX package: " & dispName)
            Dim uriVar As Uri = Nothing
            If Not TryCreate(dispName, UriKind.Absolute, uriVar) Then Return dispName
            Dim priPath As String = Path.Combine(appDir, "resources.pri")
            Dim resource As String = "ms-resource://" & pkgName & "/resources/" & uriVar.Segments.Last()
            DynaLog.LogMessage("Location of the PRI file: " & Quote & priPath & Quote)
            DynaLog.LogMessage("Resource in PRI file: " & Quote & resource & Quote)
            Dim name As String = NativeMethods.ExtractStringFromPriFile(priPath, resource)
            DynaLog.LogMessage("Name obtained with specified resource: " & Quote & name & Quote)
            If Not String.IsNullOrEmpty(name.Trim()) Then
                Return name
            End If
            Dim res As String = String.Concat(uriVar.Segments.Skip(1))
            resource = "ms-resource://" & pkgName & "/" & res
            DynaLog.LogMessage("Resource in PRI file: " & Quote & resource & Quote)
            Return NativeMethods.ExtractStringFromPriFile(priPath, resource)
        End Function

    End Class

End Namespace

