Imports Microsoft.VisualBasic.ControlChars

Public Class PostInstallScript

    Public Property ScriptContents As String

    Public Enum Stage As Integer
        ''' <summary>
        ''' The script will run during system configuration, when processing components in the Specialize pass
        ''' </summary>
        ''' <remarks></remarks>
        Specialize = 0
        ''' <summary>
        ''' The script will run when the first user logs on
        ''' </summary>
        ''' <remarks></remarks>
        FirstRun = 1
        ''' <summary>
        ''' The script will run whenever all users log on for the first time. If the target system is configured with multiple user accounts, the script will run on all of them whenever they log on for the first time
        ''' </summary>
        ''' <remarks></remarks>
        UserFirstLogon = 2
    End Enum

    Public Enum Extension As Integer
        PowerShell = 0
        Batch = 1
        Reg = 2
        VBScript = 3
        JScript = 4
        Unknown = 5
    End Enum

    Public Property ScriptExtension As Extension

    Public Sub New()
        Me.ScriptContents = ""
        Me.ScriptExtension = Extension.Unknown
    End Sub

    Public Sub New(scriptContents As String)
        Me.ScriptContents = scriptContents
        Me.ScriptExtension = Extension.Unknown
    End Sub

    Public Sub New(scriptContents As String, extension As Extension)
        Me.ScriptContents = scriptContents
        Me.ScriptExtension = extension
    End Sub

    Public Overrides Function ToString() As String
        Return "Post-installation script. Contents: " & CrLf & CrLf & Me.ScriptContents
    End Function

    Public Function Clone() As PostInstallScript
        Return New PostInstallScript() With {
            .ScriptContents = ScriptContents,
            .ScriptExtension = ScriptExtension
        }
    End Function

End Class
