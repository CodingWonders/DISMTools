Imports System.Text.RegularExpressions

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
            Dim pKey As New ProductKey()
            If Regex.Match(key, "^([2346789BCDFGHJKMPQRTVWXY]{5}-){4}[2346789BCDFGHJKMPQRTVWXY]{5}$").Value <> "" Then
                pKey.Valid = True
                pKey.Key = key
            Else
                pKey.Valid = False
                pKey.Key = ""
            End If
            Return pKey
        End Function

    End Class

End Namespace
