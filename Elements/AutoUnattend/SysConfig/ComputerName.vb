﻿Namespace Elements

    Public Class ComputerName

        Public Property DefaultName As Boolean = True

        Public Property Name As String

        Public Property Valid As Boolean

        Public Property ErrorMessage As String

    End Class

    Public Class ComputerNameValidator

        Public Shared Function ValidateComputerName(compName As String) As ComputerName
            Dim cName As New ComputerName()
            ' Assume computer name is valid...
            cName.Valid = True
            ' ...then check

            Try
                If String.IsNullOrWhiteSpace(compName) Then
                    cName.Valid = False
                    cName.ErrorMessage = "No computer name has been specified"
                    Exit Try
                End If
                If compName.Length > 15 Then
                    cName.Valid = False
                    cName.ErrorMessage = "The computer name is not valid because it is too long"
                    Exit Try
                End If
                If compName.ToCharArray().Any(Function(c) Char.IsWhiteSpace(c)) Then
                    cName.Valid = False
                    cName.ErrorMessage = "The computer name is not valid because it contains whitespaces"
                    Exit Try
                End If
                If compName.ToCharArray().All(Function(c) Char.IsDigit(c) AndAlso c <= ChrW(127)) Then
                    cName.Valid = False
                    cName.ErrorMessage = "The computer name is not valid because it contains ASCII digits"
                    Exit Try
                End If
                Dim specialChars As Char() = {"{"c, "|"c, "}"c, "~"c, "["c, "\"c, "]"c, "^"c, "'"c, ":"c, ";"c, "<"c, "="c, ">"c, "?"c, "@"c, "!"c, """"c, "#"c, "$"c, "%"c, "`"c, "("c, ")"c, "+"c, "/"c, "."c, ","c, "*"c, "&"c}
                If compName.IndexOfAny(specialChars) > -1 Then
                    cName.Valid = False
                    cName.ErrorMessage = "The computer name is not valid because it contains special characters"
                    Exit Try
                End If
            Catch ex As Exception
                cName.Valid = False
                cName.ErrorMessage = "The computer name is not valid due to the following error: " & ex.Message
                Exit Try
            End Try
            cName.Name = compName
            Return cName
        End Function
    End Class

End Namespace
