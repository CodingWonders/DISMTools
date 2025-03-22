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
            If Regex.IsMatch(key, "^([A-Z0-9]{5}-){4}[A-Z0-9]{5}$") Then
                DynaLog.LogMessage("Regex has matches. Key is valid in syntax.")
                pKey.Valid = True
                pKey.Key = key
            Else
                DynaLog.LogMessage("Regex does not have matches. Key is invalid in syntax.")
                pKey.Valid = False
                pKey.Key = ""
            End If
            Return pKey
        End Function

    End Class

End Namespace
