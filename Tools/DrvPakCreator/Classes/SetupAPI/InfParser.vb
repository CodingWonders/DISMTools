Imports System.Text
Imports System.Runtime.InteropServices
Imports System.ComponentModel

Namespace Classes.SetupAPI

    Public NotInheritable Class InfParser

        Private Sub New()
        End Sub

        Public Shared Function GetInfClassInfo(ByVal infPath As String, ByRef className As String, ByRef classGuid As Guid) As Boolean
            Dim nameBuffer As New StringBuilder(260)
            Dim requiredSize As UInteger = 0UI
            Dim guidValue As Guid = Guid.Empty

            Dim success As Boolean = NativeMethods.SetupDiGetINFClass(infPath, guidValue, nameBuffer, CUInt(nameBuffer.Capacity), requiredSize)
            className = IIf(success, nameBuffer.ToString(), String.Empty)
            classGuid = guidValue
            Return success
        End Function

        Public Shared Function ParseInfFile(ByVal infPath As String) As List(Of InfDriverEntry)
            Dim results As New List(Of InfDriverEntry)
            Dim errorLine As UInteger = 0UI

            ' Both INF styles are combined (Win4 = the modern
            ' Windows 95/98/NT-style INF, OldNT = the legacy NT 3.x format)
            ' so that SetupOpenInfFile accepts both modern and very old
            ' files without us having to guess the format up front.
            Dim hInf As IntPtr = NativeMethods.SetupOpenInfFile(infPath, Nothing, NativeMethods.INF_STYLE_WIN4 Or NativeMethods.INF_STYLE_OLDNT, errorLine)

            If hInf = IntPtr.Zero OrElse hInf = New IntPtr(-1) Then
                Dim win32Error As Integer = Marshal.GetLastWin32Error()
                Throw New Win32Exception(win32Error, String.Format("Could not open INF file '{0}' (invalid line: {1})", infPath, errorLine))
            End If

            Try
                Dim manufacturerLines As List(Of NativeMethods.INFCONTEXT) = EnumerateSection(hInf, "Manufacturer")
                For Each ManufacturerContext As NativeMethods.INFCONTEXT In manufacturerLines
                    Dim ctxCopy As NativeMethods.INFCONTEXT = ManufacturerContext
                    Dim manufacturerName As String = GetStringField(ctxCopy, 0UI)
                    Dim fieldCount As UInteger = NativeMethods.SetupGetFieldCount(ctxCopy)

                    If fieldCount < 1UI Then Continue For

                    ' Base name of the model section is first
                    Dim baseSectionName As String = GetStringField(ctxCopy, 1UI)

                    ' Next are the platform tags; when none are found, we apply to all platforms
                    Dim platformTags As New List(Of String)

                    If fieldCount >= 2UI Then
                        For i As UInteger = 2UI To fieldCount
                            Dim tag As String = GetStringField(ctxCopy, i)
                            If Not String.IsNullOrEmpty(tag) Then platformTags.Add(tag)
                        Next
                    End If

                    If platformTags.Count = 0 Then
                        ParseModelSection(hInf, baseSectionName, baseSectionName, "(any)", manufacturerName, results)
                    Else
                        For Each tag As String In platformTags
                            Dim decoratedSection As String = String.Format("{0}.{1}", baseSectionName, tag)
                            ParseModelSection(hInf, decoratedSection, baseSectionName, tag, manufacturerName, results)
                        Next
                    End If
                Next
            Finally
                NativeMethods.SetupCloseInfFile(hInf)
            End Try

            Return results
        End Function

        Private Shared Sub ParseModelSection(ByVal hInf As IntPtr, ByVal sectionName As String, ByVal fallbackSectionName As String, ByVal platformTag As String, ByVal manufacturerName As String, ByVal results As List(Of InfDriverEntry))
            Dim modelLines As List(Of NativeMethods.INFCONTEXT) = EnumerateSection(hInf, sectionName)

            ' Some very old INFs only declare the base section, without any
            ' platform decoration. If the decorated section does not exist,
            ' fall back to the base section.
            If modelLines.Count = 0 AndAlso Not String.Equals(sectionName, fallbackSectionName, StringComparison.OrdinalIgnoreCase) Then
                modelLines = EnumerateSection(hInf, fallbackSectionName)
            End If

            For Each modelContext As NativeMethods.INFCONTEXT In modelLines
                Dim ctxCopy As NativeMethods.INFCONTEXT = modelContext
                Dim description As String = GetStringField(ctxCopy, 0UI)
                Dim fieldCount As UInteger = NativeMethods.SetupGetFieldCount(ctxCopy)

                If fieldCount < 1UI Then Continue For

                ' Install section is first
                Dim installSectionName As String = GetStringField(ctxCopy, 1UI)

                Dim entry As New InfDriverEntry()
                With entry
                    .Manufacturer = manufacturerName
                    .Description = description
                    .TargetPlatform = platformTag
                    .InstallSection = installSectionName
                End With

                ' Next is the HWID plus compatible IDs
                If fieldCount >= 2UI Then
                    For i As UInteger = 2UI To fieldCount
                        Dim hwid As String = GetStringField(ctxCopy, i)
                        If Not String.IsNullOrEmpty(hwid) AndAlso Not entry.HardwareIDs.Contains(hwid) Then entry.HardwareIDs.Add(hwid)
                    Next
                End If

                entry.ServiceName = GetServiceName(hInf, installSectionName)

                results.Add(entry)
            Next
        End Sub

        Private Shared Function GetServiceName(ByVal hInf As IntPtr, ByVal installSectionName As String) As String
            If String.IsNullOrEmpty(installSectionName) Then Return ""

            Dim servicesSectionName As String = String.Format("{0}.Services", installSectionName)
            Dim context As NativeMethods.INFCONTEXT = Nothing

            If Not NativeMethods.SetupFindFirstLine(hInf, servicesSectionName, "AddService", context) Then Return String.Empty

            Return GetStringField(context, 1UI)
        End Function

        Private Shared Function EnumerateSection(ByVal hInf As IntPtr, ByVal sectionName As String) As List(Of NativeMethods.INFCONTEXT)
            Dim contexts As New List(Of NativeMethods.INFCONTEXT)

            If String.IsNullOrEmpty(sectionName) Then Return contexts

            Dim context As NativeMethods.INFCONTEXT = Nothing
            If Not NativeMethods.SetupFindFirstLine(hInf, sectionName, Nothing, context) Then Return contexts ' non-existent or emtpy section

            contexts.Add(context)

            Dim cursor As NativeMethods.INFCONTEXT = context
            While NativeMethods.SetupFindNextLine(cursor, cursor)
                contexts.Add(cursor)
            End While

            Return contexts
        End Function

        Private Shared Function GetStringField(ByRef context As NativeMethods.INFCONTEXT, ByVal fieldIndex As UInteger) As String
            Dim requiredSize As UInteger = 0UI
            NativeMethods.SetupGetStringField(context, fieldIndex, Nothing, 0UI, requiredSize)

            If requiredSize = 0UI Then Return String.Empty

            Dim buffer As New StringBuilder(CInt(requiredSize))
            If Not NativeMethods.SetupGetStringField(context, fieldIndex, buffer, requiredSize, requiredSize) Then Return String.Empty

            Return buffer.ToString()
        End Function

    End Class

End Namespace