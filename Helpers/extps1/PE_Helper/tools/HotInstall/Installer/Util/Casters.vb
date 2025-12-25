Imports Microsoft.Dism

Public Class Casters

    ''' <summary>
    ''' Casts the processor architecture enumerators from the DISM API into readable text
    ''' </summary>
    ''' <param name="Arch">The DISM processor architecture</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function CastDismArchitecture(Arch As DismProcessorArchitecture) As String
        Select Case Arch
            Case DismProcessorArchitecture.None
                Return "Unknown"
            Case DismProcessorArchitecture.Neutral
                Return "Neutral"
            Case DismProcessorArchitecture.Intel
                Return "x86"
            Case DismProcessorArchitecture.IA64
                Return "Itanium"
            Case DismProcessorArchitecture.ARM64
                Return "ARM64"
            Case DismProcessorArchitecture.ARM
                Return "ARM"
            Case DismProcessorArchitecture.AMD64
                Return "AMD64"
        End Select
        Return Nothing
    End Function

End Class
