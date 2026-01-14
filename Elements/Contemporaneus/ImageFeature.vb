Imports Microsoft.Dism

Namespace Elements.Contemporaneus

    Public Class ImageFeature

        Public Property FeatureName As String
        Public Property FeatureState As DismPackageFeatureState

        Public Sub New(name As String, state As DismPackageFeatureState)
            FeatureName = name
            FeatureState = state
        End Sub

    End Class

End Namespace
