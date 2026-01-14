Imports Microsoft.Dism

Namespace Elements.Contemporaneus

    Public Class ImageCapability

        Public Property CapabilityName As String
        Public Property CapabilityState As DismPackageFeatureState

        Public Sub New(name As String, state As DismPackageFeatureState)
            CapabilityName = name
            CapabilityState = state
        End Sub

    End Class

End Namespace
