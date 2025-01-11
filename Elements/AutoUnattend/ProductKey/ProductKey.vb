Imports System.Text.RegularExpressions
Imports Microsoft.VisualBasic.ControlChars

Namespace Elements

    Public Class ProductKey

        ''' <summary>
        ''' Determines whether the product key is valid using a regex parser (more reliable than the one from CrowdStrike)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Valid As Boolean

        ''' <summary>
        ''' The product key
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Key As String

    End Class

    Public Class ProductKeyValidator

        Public Shared Function ValidateProductKey(key As String) As ProductKey
            DynaLog.LogMessage("Validating product key " & Quote & key & Quote & "...")
            Dim pKey As New ProductKey()
            If Regex.Match(key, "^([2346789BCDFGHJKMPQRTVWXY]{5}-){4}[2346789BCDFGHJKMPQRTVWXY]{5}$").Value <> "" Then
                DynaLog.LogMessage("Regex match completed and returned values. Key is valid in syntax.")
                pKey.Valid = True
                pKey.Key = key
            Else
                DynaLog.LogMessage("Regex match completed but didn't return any values. Key is invalid in syntax.")
                pKey.Valid = False
                pKey.Key = ""
            End If
            Return pKey
        End Function

    End Class

End Namespace
