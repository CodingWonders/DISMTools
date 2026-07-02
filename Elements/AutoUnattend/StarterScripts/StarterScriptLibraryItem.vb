Public Class StarterScriptLibraryItem

    Public Property Language As String
    Public Property Name As String
    Public Property Description As String
    Public Property Customizable As Boolean
    Public Property FileName As String

End Class

Public Class StarterScriptIndex

    Public Property scripts As List(Of StarterScriptLibraryItem)

End Class
