Public Class Converters

    ''' <summary>
    ''' Using math procedures, converts the amount of bytes into a more readable format
    ''' </summary>
    ''' <param name="ByteSize">The amount of bytes, passed as a Long type for integers over (2 ^ 31) - 1</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function BytesToReadableSize(ByteSize As Long) As String
        Select Case ByteSize
            Case 1024 To 1048575
                ' Use kilobyte (kB) format
                Return Math.Round(ByteSize / 1024, 2) & " kB"
            Case 1048576 To 1073741823
                ' Use megabyte (MB) format
                Return Math.Round(ByteSize / 1024 ^ 2, 2) & " MB"
            Case 1073741824 To 1099511627775
                ' Use gigabyte (GB) format
                Return Math.Round(ByteSize / 1024 ^ 3, 2) & " GB"
            Case 1099511627776 To 1125899906842623
                ' Use terabyte (TB) format
                Return Math.Round(ByteSize / 1024 ^ 4, 2) & " TB"
            Case Is >= 1125899906842624
                ' In a hypothetical world where drives that are petabytes big become mainstream, use petabyte (PB) format
                Return Math.Round(ByteSize / 1024 ^ 5, 2) & " PB"
        End Select
        Return Nothing
    End Function

End Class
