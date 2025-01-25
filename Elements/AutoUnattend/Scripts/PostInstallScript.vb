Imports Microsoft.VisualBasic.ControlChars

Public Class PostInstallScript

    Public Property ScriptContents As String

    Public Enum Stage
        ''' <summary>
        ''' The script will run during system configuration, when processing components in the Specialize pass
        ''' </summary>
        ''' <remarks></remarks>
        Specialize
        ''' <summary>
        ''' The script will run when the first user logs on
        ''' </summary>
        ''' <remarks></remarks>
        FirstRun
        ''' <summary>
        ''' The script will run whenever all users log on for the first time. If the target system is configured with multiple user accounts, the script will run on all of them whenever they log on for the first time
        ''' </summary>
        ''' <remarks></remarks>
        UserFirstLogon
    End Enum

    Public Property ScriptStage As Stage

    Public Sub New()
        Me.ScriptContents = ""
        Me.ScriptStage = Stage.FirstRun
    End Sub

    Public Sub New(scriptContents As String, scriptStage As Stage)
        Me.ScriptContents = scriptContents
        Me.ScriptStage = scriptStage
    End Sub

    Public Overrides Function ToString() As String
        Return "Post-installation script, for pass: " & Me.ScriptStage.ToString() & ". Contents: " & CrLf & CrLf & Me.ScriptContents
    End Function

End Class
