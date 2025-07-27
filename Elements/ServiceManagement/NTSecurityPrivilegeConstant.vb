Public Class NTSecurityPrivilegeConstant

    Public Property ConstantNameText As String
    Public Property ConstantUserRight As String
    Public Property ConstantDescription As String

    Public Sub New(text As String, userRight As String, description As String)
        Me.ConstantNameText = text
        Me.ConstantUserRight = userRight
        Me.ConstantDescription = description
    End Sub

End Class
