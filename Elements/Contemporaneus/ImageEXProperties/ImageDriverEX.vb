Imports DISMTools.Elements.Contemporaneus
Imports Microsoft.Dism

Namespace Elements.Contemporaneus.ImageEXProperties

    Public Class ImageDriverEX
        Inherits ImageDriver

        Public Property DriverClassDescription As String
        Public Property DriverClassGuid As String
        Public Property DriverCatalogFile As String
        Public Property DriverBootCritical As Boolean
        Public Property DriverSignature As DismDriverSignature

        Public Sub New(publishedName As String, originalFileName As String, inbox As Boolean, className As String, providerName As String, publishedDate As String, version As Version)
            MyBase.New(publishedName, originalFileName, inbox, className, providerName, publishedDate, version)
        End Sub

    End Class

End Namespace
