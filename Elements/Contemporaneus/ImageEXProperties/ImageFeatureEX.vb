Imports DISMTools.Elements.Contemporaneus
Imports Microsoft.Dism

Namespace Elements.Contemporaneus.ImageEXProperties

    Public Class ImageFeatureEX
        Inherits ImageFeature

        Public Property FeatureDisplayName As String
        Public Property FeatureDescription As String
        Public Property FeatureRestartRequired As DismRestartType
        Public Property FeatureCustomProperties As DismCustomPropertyCollection

        Public Sub New(name As String, state As DismPackageFeatureState)
            MyBase.New(name, state)
        End Sub

    End Class

End Namespace
