Imports DISMTools.Elements.Contemporaneus
Imports Microsoft.Dism

Namespace Elements.Contemporaneus.ImageEXProperties

    Public Class ImageCapabilityEX
        Inherits ImageCapability

        Public Property CapabilityDisplayName As String
        Public Property CapabilityDescription As String
        Public Property CapabilityDownloadSize As ULong
        Public Property CapabilityInstallSize As ULong

        Public Sub New(name As String, state As DismPackageFeatureState)
            MyBase.New(name, state)
        End Sub

    End Class

End Namespace
