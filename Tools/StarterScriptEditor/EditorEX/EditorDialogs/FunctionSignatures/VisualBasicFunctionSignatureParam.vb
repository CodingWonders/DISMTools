Namespace EditorEX.EditorDialogs.FunctionSignatures

    Public Class VisualBasicFunctionSignatureParam

        Private _paramName As String
        Public Property ParameterName() As String
            Get
                Return _paramName
            End Get
            Set(ByVal value As String)
                _paramName = value
            End Set
        End Property

        Private _byRef As Boolean
        Public Property ByReference() As Boolean
            Get
                Return _byRef
            End Get
            Set(ByVal value As Boolean)
                _byRef = value
            End Set
        End Property

        Private _optional As Boolean
        Public Property IsOptional() As Boolean
            Get
                Return _optional
            End Get
            Set(ByVal value As Boolean)
                _optional = False
            End Set
        End Property

        Public Sub New(ByVal ParameterName As String)
            Me.ParameterName = ParameterName
        End Sub

        Public Sub New(ByVal ParameterName As String, ByVal ByReference As Boolean, ByVal IsOptional As Boolean)
            Me.ParameterName = ParameterName
            Me.ByReference = ByReference
            Me.IsOptional = IsOptional
        End Sub

    End Class

End Namespace
