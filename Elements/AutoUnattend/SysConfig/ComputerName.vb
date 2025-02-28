Imports Microsoft.VisualBasic.ControlChars

Namespace Elements

    Public Class ComputerName

        ''' <summary>
        ''' Determines whether to let Windows set a random computer name
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DefaultName As Boolean = True

        ''' <summary>
        ''' The name of the computer
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Name As String

        ''' <summary>
        ''' Determines whether the specified computer name is valid according to checks in ComputerNameValidator
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Valid As Boolean

        ''' <summary>
        ''' The error message from ComputerValidator if computer name is not valid
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ErrorMessage As String

    End Class

    Public Class ComputerNameValidator

        Public Shared Function ValidateComputerName(compName As String) As ComputerName
            DynaLog.LogMessage("Validating computer name " & Quote & compName & Quote & "...")
            Dim cName As New ComputerName()
            ' Assume computer name is valid...
            cName.Valid = True
            ' ...then check

            Try
                If String.IsNullOrWhiteSpace(compName) Then
                    DynaLog.LogMessage("Computer name is nothing.")
                    cName.Valid = False
                    cName.ErrorMessage = "No computer name has been specified"
                    Exit Try
                End If
                If compName.Length > 15 Then
                    DynaLog.LogMessage("Computer name is over 15 characters.")
                    cName.Valid = False
                    cName.ErrorMessage = "The computer name is not valid because it is too long"
                    Exit Try
                End If
                If compName.ToCharArray().Any(Function(c) Char.IsWhiteSpace(c)) Then
                    DynaLog.LogMessage("Computer name contains whitespaces.")
                    cName.Valid = False
                    cName.ErrorMessage = "The computer name is not valid because it contains whitespaces"
                    Exit Try
                End If
                If compName.ToCharArray().All(Function(c) Char.IsDigit(c) AndAlso c <= ChrW(127)) Then
                    DynaLog.LogMessage("Computer name contains ASCII digits.")
                    cName.Valid = False
                    cName.ErrorMessage = "The computer name is not valid because it contains ASCII digits"
                    Exit Try
                End If
                Dim specialChars As Char() = {"{"c, "|"c, "}"c, "~"c, "["c, "\"c, "]"c, "^"c, "'"c, ":"c, ";"c, "<"c, "="c, ">"c, "?"c, "@"c, "!"c, """"c, "#"c, "$"c, "%"c, "`"c, "("c, ")"c, "+"c, "/"c, "."c, ","c, "*"c, "&"c}
                If compName.IndexOfAny(specialChars) > -1 Then
                    DynaLog.LogMessage("Computer name contains special characters.")
                    cName.Valid = False
                    cName.ErrorMessage = "The computer name is not valid because it contains special characters"
                    Exit Try
                End If
            Catch ex As Exception
                DynaLog.LogMessage("Could not check computer name. Error message: " & ex.Message)
                cName.Valid = False
                cName.ErrorMessage = "The computer name is not valid due to the following error: " & ex.Message
                Exit Try
            End Try
            cName.Name = compName
            Return cName
        End Function
    End Class

End Namespace
