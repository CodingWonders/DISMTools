Imports DrvPakCreator.Classes.ModelCatalogs.Dell
Imports System.IO

Namespace Helpers.ModelCatalogs

    Public Class DellModelCatalogLoader
        Inherits ModelCatalogLoader

        Private ReadOnly CatalogDownloadedFilePath As String = String.Format("DriverPackCatalog_{0}.cab", New Random().Next()), _
                         CatalogExpandedFilePath As String = Path.Combine(Path.GetTempPath(), "DELL_EXPANDEDCLG\DriverPackCatalog.xml")

        Public Overrides Function LoadManufacturerCatalog() As Classes.ModelCatalogs.ModelCatalog
            If Not DownloadFile(CatalogDownloadPath, Path.Combine(Path.GetTempPath(), CatalogDownloadedFilePath)) Then Return Nothing
            If Not ExpandCabArchive(Path.Combine(Path.GetTempPath(), CatalogDownloadedFilePath), Path.Combine(Path.GetDirectoryName(CatalogDownloadedFilePath), "DELL_EXPANDEDCLG")) Then Return Nothing

            Dim ResultingCatalogModel As Object = LoadCatalogFile(CatalogExpandedFilePath, GetType(DellDriverPackCatalog))

            If ResultingCatalogModel IsNot Nothing AndAlso TypeOf ResultingCatalogModel Is DellDriverPackCatalog Then
                Return CType(ResultingCatalogModel, DellDriverPackCatalog)
            End If

            Return Nothing
        End Function

        Protected Overrides ReadOnly Property CatalogDownloadPath() As String
            Get
                Return "https://downloads.dell.com/catalog/DriverPackCatalog.cab"
            End Get
        End Property
    End Class

End Namespace