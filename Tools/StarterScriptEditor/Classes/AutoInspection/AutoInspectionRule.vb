Imports System.Xml.Serialization

Namespace Classes.AutoInspection

    Public Class AutoInspectionRule

        Private _ruleName As String
        <XmlElement("name")> _
        Public Property RuleName() As String
            Get
                Return _ruleName
            End Get
            Set(ByVal value As String)
                _ruleName = value
            End Set
        End Property

        Private _ruleDescription As String
        <XmlElement("description")> _
        Public Property RuleDescription() As String
            Get
                Return _ruleDescription
            End Get
            Set(ByVal value As String)
                _ruleDescription = value
            End Set
        End Property

        Private _ruleExpression As String
        <XmlElement("expression")> _
        Public Property RuleExpression() As String
            Get
                Return _ruleExpression
            End Get
            Set(ByVal value As String)
                _ruleExpression = value
            End Set
        End Property

        Private _ruleSeverity As AutoInspectionRuleSeverity
        <XmlAttribute("severity")> _
        Public Property RuleSeverity() As AutoInspectionRuleSeverity
            Get
                Return _ruleSeverity
            End Get
            Set(ByVal value As AutoInspectionRuleSeverity)
                _ruleSeverity = value
            End Set
        End Property

    End Class

    <XmlRoot("rules")> _
    Public Class AutoInspectionRules

        Private _rules As List(Of AutoInspectionRule)
        <XmlElement("rule")> _
        Public Property Rules() As List(Of AutoInspectionRule)
            Get
                Return _rules
            End Get
            Set(ByVal value As List(Of AutoInspectionRule))
                _rules = value
            End Set
        End Property

    End Class

End Namespace
