Namespace Classes.AutoInspection

    Public Class AutoInspectionResult

        Private _scannedRule As AutoInspectionRule
        Public Property ScannedRule() As AutoInspectionRule
            Get
                Return _scannedRule
            End Get
            Set(ByVal value As AutoInspectionRule)
                _scannedRule = value
            End Set
        End Property

        Private _occurrenceIdx As Long
        Public Property OccurrenceIndex() As Long
            Get
                Return _occurrenceIdx
            End Get
            Set(ByVal value As Long)
                _occurrenceIdx = value
            End Set
        End Property

        Private _occurrenceLen As Long
        Public Property OccurrenceLength() As Long
            Get
                Return _occurrenceLen
            End Get
            Set(ByVal value As Long)
                _occurrenceLen = value
            End Set
        End Property

        Public Sub New(ByVal rule As AutoInspectionRule, ByVal index As Long, ByVal length As Long)
            Me.ScannedRule = rule
            Me.OccurrenceIndex = index
            Me.OccurrenceLength = length
        End Sub

        Public Overrides Function ToString() As String
            Return ScannedRule.RuleDescription
        End Function
    End Class

End Namespace
