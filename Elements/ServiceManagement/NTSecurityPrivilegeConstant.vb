Public Class NTSecurityPrivilegeConstant

    ''' <summary>
    ''' The constant text as defined by Windows NT headers
    ''' </summary>
    ''' <remarks></remarks>
    Public Property ConstantNameText As String

    ''' <summary>
    ''' The user right of this constant
    ''' </summary>
    ''' <remarks></remarks>
    Public Property ConstantUserRight As String

    ''' <summary>
    ''' The description of this constant
    ''' </summary>
    ''' <remarks></remarks>
    Public Property ConstantDescription As String

    ''' <summary>
    ''' Initializes an object of the constant class with specified values
    ''' </summary>
    ''' <param name="text">The constant text as defined by Windows NT headers</param>
    ''' <param name="userRight">The user right of this constant</param>
    ''' <param name="description">The description of this constant</param>
    ''' <remarks></remarks>
    Public Sub New(text As String, userRight As String, description As String)
        Me.ConstantNameText = text
        Me.ConstantUserRight = userRight
        Me.ConstantDescription = description
    End Sub

End Class
