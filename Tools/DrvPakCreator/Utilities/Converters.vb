Namespace Utilities

    Public Class Converters

        ''' <summary>
        ''' Using math procedures, converts the amount of bytes into a more readable format
        ''' </summary>
        ''' <param name="ByteSize">The amount of bytes, passed as a Long type for integers over (2 ^ 31) - 1</param>
        ''' <param name="UseCountryRepresentation">Uses a special representation of kB, MB, and GB based on the language. For example, France uses the "octet" representation, for ko, Mo, Go. This assumes the program is run on an OS in french</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function BytesToReadableSize(ByVal ByteSize As Long, Optional ByVal UseCountryRepresentation As Boolean = False) As String
            Select Case ByteSize
                Case 1024 To 1048575
                    ' Use kilobyte (kB) format
                    If UseCountryRepresentation Then
                        Return Math.Round(ByteSize / 1024, 2) & " Ko"
                    Else
                        Return Math.Round(ByteSize / 1024, 2) & " kB"
                    End If
                Case 1048576 To 1073741823
                    ' Use megabyte (MB) format
                    If UseCountryRepresentation Then
                        Return Math.Round(ByteSize / 1024 ^ 2, 2) & " Mo"
                    Else
                        Return Math.Round(ByteSize / 1024 ^ 2, 2) & " MB"
                    End If
                Case 1073741824 To 1099511627775
                    ' Use gigabyte (GB) format
                    If UseCountryRepresentation Then
                        Return Math.Round(ByteSize / 1024 ^ 3, 2) & " Go"
                    Else
                        Return Math.Round(ByteSize / 1024 ^ 3, 2) & " GB"
                    End If
                Case 1099511627776 To 1125899906842623
                    ' Use terabyte (TB) format
                    If UseCountryRepresentation Then
                        Return Math.Round(ByteSize / 1024 ^ 4, 2) & " To"
                    Else
                        Return Math.Round(ByteSize / 1024 ^ 4, 2) & " TB"
                    End If
                Case Is >= 1125899906842624
                    ' In a hypothetical world where drives that are petabytes big become mainstream, use petabyte (PB) format
                    If UseCountryRepresentation Then
                        Return Math.Round(ByteSize / 1024 ^ 5, 2) & " Po"
                    Else
                        Return Math.Round(ByteSize / 1024 ^ 5, 2) & " PB"
                    End If
            End Select
            Return Nothing
        End Function

    End Class
End Namespace

