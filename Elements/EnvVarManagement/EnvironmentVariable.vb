Imports Microsoft.Win32

Public Class EnvironmentVariable

    Public Enum EnvironmentVariableScope As Integer
        Machine = 0
        User = 1
    End Enum

    Public Property Name As String
    Public Property Value As String
    Public Property ValueKind As RegistryValueKind
    Public Property Scope As EnvironmentVariableScope
    Public Property NoLongerExists As Boolean

    Public Sub New(name As String, value As String, scope As EnvironmentVariableScope, valueKind As RegistryValueKind)
        Me.Name = name
        Me.Value = value
        Me.Scope = scope
        Me.ValueKind = valueKind
        Me.NoLongerExists = False
    End Sub

    Public Overrides Function ToString() As String
        Return String.Format("{0} ({1}): {2} -- Value Type from Enum {3}", Name, If(Scope = EnvironmentVariableScope.Machine, "MACHINE", "USER"), Value, ValueKind)
    End Function

End Class
