Imports DrvPakCreator.Classes.ModelCatalogs.Lenovo
Imports System.IO

Namespace Helpers.ModelCatalogs

    Public Class LenovoModelCatalogLoader
        Inherits ModelCatalogLoader

        Private ReadOnly CatalogDownloadedFilePath As String = Path.Combine(Path.GetTempPath(), String.Format("catalogv2_{0}.xml", New Random().Next()))

        Public Overrides Function LoadManufacturerCatalog() As Classes.ModelCatalogs.ModelCatalog
            If Not DownloadFile(CatalogDownloadPath, CatalogDownloadedFilePath) Then Return Nothing
            Dim ResultingCatalogModel As Object = LoadCatalogFile(CatalogDownloadedFilePath, GetType(LenovoDriverPackCatalog))

            If ResultingCatalogModel IsNot Nothing AndAlso TypeOf ResultingCatalogModel Is LenovoDriverPackCatalog Then
                Return CType(ResultingCatalogModel, LenovoDriverPackCatalog)
            End If

            Return Nothing
        End Function

        Protected Overrides ReadOnly Property CatalogDownloadPath() As String
            Get
                Return "https://download.lenovo.com/cdrt/td/catalogv2.xml"
            End Get
        End Property
    End Class

End Namespace