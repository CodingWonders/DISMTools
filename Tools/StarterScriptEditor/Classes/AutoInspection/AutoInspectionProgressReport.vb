Namespace Classes.AutoInspection

    Public Class AutoInspectionProgressReport

        Private _percent As Long
        Public Property Percentage() As Long
            Get
                Return _percent
            End Get
            Set(ByVal value As Long)
                _percent = value
            End Set
        End Property

        Private _ruleName As String
        Public Property RuleName() As String
            Get
                Return _ruleName
            End Get
            Set(ByVal value As String)
                _ruleName = value
            End Set
        End Property

        Public Sub New(ByVal Percentage As Integer, ByVal RuleName As String)
            Me.Percentage = Percentage
            Me.RuleName = RuleName
        End Sub

    End Class

End Namespace