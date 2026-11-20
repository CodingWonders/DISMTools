Imports DrvPakCreator.Classes.ModelCatalogs
Imports DrvPakCreator.Classes.ModelCatalogs.HP
Imports DrvPakCreator.Classes.ModelCatalogs.Dell
Imports DrvPakCreator.Classes.ModelCatalogs.Lenovo
Imports System.IO

Public Class ModelCatalogViewerForm

    Private ModelCatalogs As New List(Of ModelCatalog)

    Private Function DeserializeHPDriverCatalog(ByVal CatalogPath As String) As HPDriverPackCatalog
        If Not File.Exists(CatalogPath) Then Return Nothing

        Dim obtainedCatalog As HPDriverPackCatalog = Nothing

        Try
            Dim deserializer As New XmlSerializer(GetType(HPDataSet))
            Using fs As New FileStream(CatalogPath, FileMode.Open)
                obtainedCatalog = CType(deserializer.Deserialize(fs), HPDataSet).Catalog
            End Using
            If Debugger.IsAttached Then Debugger.Break()
        Catch ex As Exception

        End Try

        Return obtainedCatalog
    End Function

    Private Function DeserializeDellDriverCatalog(ByVal CatalogPath As String) As DellDriverPackCatalog
        If Not File.Exists(CatalogPath) Then Return Nothing

        Dim obtainedCatalog As DellDriverPackCatalog = Nothing

        Try
            Dim deserializer As New XmlSerializer(GetType(DellDriverPackCatalog))
            Using fs As New FileStream(CatalogPath, FileMode.Open)
                obtainedCatalog = CType(deserializer.Deserialize(fs), DellDriverPackCatalog)
            End Using
            If Debugger.IsAttached Then Debugger.Break()
        Catch ex As Exception

        End Try

        Return obtainedCatalog
    End Function

    Private Function DeserializeLenovoDriverCatalog(ByVal CatalogPath As String) As LenovoDriverPackCatalog
        If Not File.Exists(CatalogPath) Then Return Nothing

        Dim obtainedCatalog As LenovoDriverPackCatalog = Nothing

        Try
            Dim deserializer As New XmlSerializer(GetType(LenovoDriverPackCatalog))
            Using fs As New FileStream(CatalogPath, FileMode.Open)
                obtainedCatalog = CType(deserializer.Deserialize(fs), LenovoDriverPackCatalog)
            End Using
            If Debugger.IsAttached Then Debugger.Break()
        Catch ex As Exception

        End Try

        Return obtainedCatalog
    End Function

    Private Sub btnLoadHpModels_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadHpModels.Click
        If ofdHpModelCatalog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim ObtainedDriverCatalog As HPDriverPackCatalog = DeserializeHPDriverCatalog(ofdHpModelCatalog.FileName)
            Dim lvItems As New List(Of ListViewItem)

            For Each CatalogProduct As HPProductOSDriverPack In ObtainedDriverCatalog.Products.Products
                lvItems.Add(New ListViewItem(New String() {CatalogProduct.SystemName, CatalogProduct.OSName}))
            Next

            lvHpProducts.Items.AddRange(lvItems.ToArray())
        End If
    End Sub

    Private Sub btnLenobodyCares_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLenobodyCares.Click
        If ofdHpModelCatalog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim ObtainedDriverCatalog As LenovoDriverPackCatalog = DeserializeLenovoDriverCatalog(ofdHpModelCatalog.FileName)
            If Debugger.IsAttached Then Debugger.Break()
        End If
    End Sub

    Private Sub btnLoadDellModels_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadDellModels.Click
        If ofdHpModelCatalog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim ObtainedDriverCatalog As DellDriverPackCatalog = DeserializeDellDriverCatalog(ofdHpModelCatalog.FileName)
            If Debugger.IsAttached Then Debugger.Break()
        End If
    End Sub
End Class