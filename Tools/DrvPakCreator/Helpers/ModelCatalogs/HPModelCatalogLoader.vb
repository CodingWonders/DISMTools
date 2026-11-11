Imports DrvPakCreator.Classes.ModelCatalogs.HP
Imports System.IO

Namespace Helpers.ModelCatalogs

    Public Class HPModelCatalogLoader
        Inherits ModelCatalogLoader

        Private ReadOnly CatalogDownloadedFilePath As String = String.Format("HPClientDriverPackCatalog_{0}.cab", New Random().Next()), _
                         CatalogExpandedFilePath As String = Path.Combine(Path.GetTempPath(), "HP_EXPANDEDCLG\HPClientDriverPackCatalog.xml")

        Public Overrides Function LoadManufacturerCatalog() As Classes.ModelCatalogs.ModelCatalog
            If Not DownloadFile(CatalogDownloadPath, Path.Combine(Path.GetTempPath(), CatalogDownloadedFilePath)) Then Return Nothing
            If Not ExpandCabArchive(Path.Combine(Path.GetTempPath(), CatalogDownloadedFilePath), Path.Combine(Path.GetTempPath(), "HP_EXPANDEDCLG")) Then Return Nothing

            Dim ResultingCatalogModel As Object = LoadCatalogFile(CatalogExpandedFilePath, GetType(HPDataSet))

            If ResultingCatalogModel IsNot Nothing AndAlso TypeOf ResultingCatalogModel Is HPDataSet Then
                Return CType(ResultingCatalogModel, HPDataSet).Catalog
            End If

            Return Nothing
        End Function

        Protected Overrides ReadOnly Property CatalogDownloadPath() As String
            Get
                Return "https://hpia.hpcloud.hp.com/downloads/driverpackcatalog/HPClientDriverPackCatalog.cab"
            End Get
        End Property
    End Class

End Namespace