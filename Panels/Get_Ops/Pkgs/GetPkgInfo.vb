Imports System.Windows.Forms
Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.ControlChars
Imports Microsoft.Dism
Imports DISMTools.Utilities

Public Class GetPkgInfoDlg

    Dim PackageInfoExList As New List(Of DismPackageInfoEx)
    Dim PackageInfoList As New List(Of DismPackageInfo)
    Public InstalledPkgInfo As DismPackageCollection

    Private Sub GetPkgInfoDlg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If MainForm.BackColor = Color.FromArgb(48, 48, 48) Then
            Win10Title.BackColor = Color.FromArgb(48, 48, 48)
            BackColor = Color.FromArgb(31, 31, 31)
            ForeColor = Color.White
            ListBox1.BackColor = Color.FromArgb(31, 31, 31)
            ListBox2.BackColor = Color.FromArgb(31, 31, 31)
        ElseIf MainForm.BackColor = Color.FromArgb(239, 239, 242) Then
            Win10Title.BackColor = Color.White
            BackColor = Color.FromArgb(238, 238, 242)
            ForeColor = Color.Black
            ListBox1.BackColor = Color.FromArgb(238, 238, 242)
            ListBox2.BackColor = Color.FromArgb(238, 238, 242)
        End If
        SearchBox1.BackColor = BackColor
        SearchBox1.ForeColor = ForeColor
        SearchPic.Image = If(MainForm.BackColor = Color.FromArgb(48, 48, 48), My.Resources.search_dark, My.Resources.search_light)
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Text = "Get package information"
                        Label1.Text = Text
                        Label2.Text = "What do you want to get information about?"
                        Label3.Text = "Click here to get information about packages that you've installed or that came with the Windows image you're servicing"
                        Label4.Text = "Click here to get information about packages that you want to add to the Windows image you're servicing before proceeding with the package addition process"
                        Label5.Text = "Ready"
                        Label6.Text = "Add or select a package file to view its information here"
                        Label7.Text = "Package information"
                        Label8.Text = "Package name:"
                        Label10.Text = "Is package applicable?"
                        Label12.Text = "Copyright:"
                        Label14.Text = "Product version:"
                        Label16.Text = "Release type:"
                        Label18.Text = "Company:"
                        Label20.Text = "Creation time:"
                        Label22.Text = "Package name:"
                        Label24.Text = "Is package applicable?"
                        Label26.Text = "Copyright:"
                        Label28.Text = "Install time:"
                        Label30.Text = "Last update time:"
                        Label31.Text = "Company:"
                        Label33.Text = "Install package name:"
                        Label36.Text = "Package information"
                        Label37.Text = "Select an installed package to view its information here"
                        Label39.Text = "Display name:"
                        Label41.Text = "Creation time:"
                        Label43.Text = "Description:"
                        Label45.Text = "Product name:"
                        Label47.Text = "Install client:"
                        Label48.Text = "Is a restart required?"
                        Label50.Text = "Support information:"
                        Label52.Text = "State:"
                        Label54.Text = "Is a boot up required for full installation?"
                        Label58.Text = "Custom properties:"
                        Label60.Text = "Features:"
                        Label61.Text = "Capability identity:"
                        Label63.Text = "Description:"
                        Label65.Text = "Install client:"
                        Label67.Text = "Install package name:"
                        Label69.Text = "Install time:"
                        Label71.Text = "Last update time:"
                        Label73.Text = "Display name:"
                        Label75.Text = "Product name:"
                        Label77.Text = "Product version:"
                        Label79.Text = "Release type:"
                        Label81.Text = "Is a restart required?"
                        Label83.Text = "Support information:"
                        Label85.Text = "State:"
                        Label87.Text = "Is a boot up required for full installation?"
                        Label89.Text = "Capability identity:"
                        Label91.Text = "Custom properties:"
                        Label93.Text = "Features:"
                        LinkLabel1.Text = "<- Go back"
                        Button1.Text = "Add package..."
                        Button2.Text = "Remove selected"
                        Button3.Text = "Remove all"
                        Button4.Text = "Save..."
                        InstalledPackageLink.Text = "I want to get information about installed packages in the image"
                        PackageFileLink.Text = "I want to get information about package files"
                        OpenFileDialog1.Title = "Locate package files"
                        SearchBox1.cueBanner = "Type here to search for a package..."
                    Case "ESN"
                        Text = "Obtener información de paquetes"
                        Label1.Text = Text
                        Label2.Text = "¿Acerca de qué le gustaría obtener información?"
                        Label3.Text = "Haga clic aquí para obtener información de paquetes que ha instalado o que vengan con la imagen de Windows a la que está dando servicio"
                        Label4.Text = "Haga clic aquí para obtener información de paquetes que le gustaría añadir a la imagen de Windows a la que está dando servicio antes de proceder con el proceso de adición de paquetes"
                        Label5.Text = "Listo"
                        Label6.Text = "Añada o seleccione un archivo de paquete para ver su información aquí"
                        Label7.Text = "Información de paquete"
                        Label8.Text = "Nombre de paquete:"
                        Label10.Text = "¿El paquete es aplicable?"
                        Label12.Text = "Copyright:"
                        Label14.Text = "Versión de producto:"
                        Label16.Text = "Tipo de paquete:"
                        Label18.Text = "Compañía:"
                        Label20.Text = "Tiempo de creación:"
                        Label22.Text = "Nombre de paquete:"
                        Label24.Text = "¿El paquete es aplicable?"
                        Label26.Text = "Copyright:"
                        Label28.Text = "Tiempo de instalación:"
                        Label30.Text = "Último tiempo de actualización:"
                        Label31.Text = "Compañía:"
                        Label33.Text = "Nombre del paquete de instalación:"
                        Label36.Text = "Información de paquete"
                        Label37.Text = "Seleccione un paquete instalado para ver su información aquí"
                        Label39.Text = "Nombre a mostrar:"
                        Label41.Text = "Tiempo de creación:"
                        Label43.Text = "Descripción:"
                        Label45.Text = "Nombre de producto:"
                        Label47.Text = "Cliente de instalación:"
                        Label48.Text = "¿Se requiere un reinicio?"
                        Label50.Text = "Información de soporte:"
                        Label52.Text = "Estado:"
                        Label54.Text = "¿Se requiere un arranque para una instalación completa?"
                        Label58.Text = "Propiedades personalizadas:"
                        Label60.Text = "Características:"
                        Label61.Text = "Identidad de funcionalidad:"
                        Label63.Text = "Descripción:"
                        Label65.Text = "Cliente de instalación:"
                        Label67.Text = "Nombre del paquete de instalación:"
                        Label69.Text = "Tiempo de instalación:"
                        Label71.Text = "Último tiempo de actualización:"
                        Label73.Text = "Nombre a mostrar:"
                        Label75.Text = "Nombre de producto:"
                        Label77.Text = "Versión de producto:"
                        Label79.Text = "Tipo de paquete:"
                        Label81.Text = "¿Se requiere un reinicio?"
                        Label83.Text = "Información de soporte:"
                        Label85.Text = "Estado:"
                        Label87.Text = "¿Se requiere un arranque para una instalación completa?"
                        Label89.Text = "Identidad de funcionalidad:"
                        Label91.Text = "Propiedades personalizadas:"
                        Label93.Text = "Características:"
                        LinkLabel1.Text = "<- Atrás"
                        Button1.Text = "Añadir paquete..."
                        Button2.Text = "Eliminar selección"
                        Button3.Text = "Eliminar todo"
                        Button4.Text = "Guardar..."
                        InstalledPackageLink.Text = "Deseo obtener información acerca de paquetes instalados en la imagen"
                        PackageFileLink.Text = "Deseo obtener información acerca de archivos de paquetes"
                        OpenFileDialog1.Title = "Ubique los archivos de paquetes"
                        SearchBox1.cueBanner = "Escriba aquí para buscar un paquete..."
                    Case "FRA"
                        Text = "Obtenir des informations sur les paquets"
                        Label1.Text = Text
                        Label2.Text = "Sur quoi souhaitez-vous obtenir des informations ?"
                        Label3.Text = "Cliquez ici pour obtenir des informations sur les paquets que vous avez installés ou qui sont fournis avec l'image Windows dont vous assurez la maintenance."
                        Label4.Text = "Cliquez ici pour obtenir des informations sur les paquets que vous souhaitez ajouter à l'image Windows que vous maintenez avant de procéder à l'ajout de paquets."
                        Label5.Text = "Prêt"
                        Label6.Text = "Ajoutez ou sélectionnez un fichier de paquet pour afficher son information ici"
                        Label7.Text = "Information sur le paquet"
                        Label8.Text = "Nom du paquet :"
                        Label10.Text = "Le paquet est-il applicable ?"
                        Label12.Text = "Copyright :"
                        Label14.Text = "Version du produit :"
                        Label16.Text = "Type de publication :"
                        Label18.Text = "Enterprise :"
                        Label20.Text = "Temps de création :"
                        Label22.Text = "Nom du paquet :"
                        Label24.Text = "Le paquet est-il applicable ?"
                        Label26.Text = "Copyright :"
                        Label28.Text = "Temps d'installation :"
                        Label30.Text = "Dernière heure de mise à jour :"
                        Label31.Text = "Enterprise :"
                        Label33.Text = "Nom du paquet d'installation :"
                        Label36.Text = "Information sur le paquet"
                        Label37.Text = "Sélectionnez un paquet installé pour afficher son information ici"
                        Label39.Text = "Nom d'affichage:"
                        Label41.Text = "Temps de création :"
                        Label43.Text = "Description :"
                        Label45.Text = "Nom du produit :"
                        Label47.Text = "Client d'installation :"
                        Label48.Text = "Un redémarrage est-il nécessaire ?"
                        Label50.Text = "Information de support :"
                        Label52.Text = "État :"
                        Label54.Text = "L'installation complète nécessite-t-elle un démarrage ?"
                        Label58.Text = "Propriétés personnalisées :"
                        Label60.Text = "Caractéristiques :"
                        Label61.Text = "Identité de la capacité :"
                        Label63.Text = "Description :"
                        Label65.Text = "Client d'installation :"
                        Label67.Text = "Nom du paquet d'installation :"
                        Label69.Text = "Temps d'installation :"
                        Label71.Text = "Dernière heure de mise à jour :"
                        Label73.Text = "Nom d'affichage :"
                        Label75.Text = "Nom du produit :"
                        Label77.Text = "Version du produit :"
                        Label79.Text = "Type de publication :"
                        Label81.Text = "Un redémarrage est-il nécessaire ?"
                        Label83.Text = "Information de support :"
                        Label85.Text = "État:"
                        Label87.Text = "L'installation complète nécessite-t-elle un démarrage ?"
                        Label89.Text = "Identité de la capacité :"
                        Label91.Text = "Propriétés personnalisées :"
                        Label93.Text = "Caractéristiques :"
                        LinkLabel1.Text = "<- Retour"
                        Button1.Text = "Ajouter un paquet..."
                        Button2.Text = "Supprimer la sélection"
                        Button3.Text = "Supprimer tout"
                        Button4.Text = "Sauvegarder..."
                        InstalledPackageLink.Text = "Je souhaite obtenir des informations sur les paquets installés dans l'image."
                        PackageFileLink.Text = "Je souhaite obtenir des informations sur les fichiers de paquets"
                        OpenFileDialog1.Title = "Localiser les fichiers des paquets"
                        SearchBox1.cueBanner = "Tapez ici pour rechercher un paquet..."
                    Case "PTB", "PTG"
                        Text = "Obter informações sobre o pacote"
                        Label1.Text = Text
                        Label2.Text = "Sobre o que é que pretende obter informações?"
                        Label3.Text = "Clique aqui para obter informações sobre os pacotes que instalou ou que vieram com a imagem do Windows que está a reparar"
                        Label4.Text = "Clique aqui para obter informações sobre os pacotes que pretende adicionar à imagem do Windows que está a reparar antes de prosseguir com o processo de adição de pacotes"
                        Label5.Text = "Pronto"
                        Label6.Text = "Adicione ou seleccione um ficheiro de pacote para ver as suas informações aqui"
                        Label7.Text = "Informações do pacote"
                        Label8.Text = "Nome do pacote:"
                        Label10.Text = "O pacote é aplicável?"
                        Label12.Text = "Direitos de autor:"
                        Label14.Text = "Versão do produto:"
                        Label16.Text = "Tipo de versão:"
                        Label18.Text = "Empresa:"
                        Label20.Text = "Hora de criação:"
                        Label22.Text = "Nome do pacote:"
                        Label24.Text = "O pacote é aplicável?"
                        Label26.Text = "Direitos de autor:"
                        Label28.Text = "Hora de instalação:"
                        Label30.Text = "Hora da última atualização:"
                        Label31.Text = "Empresa:"
                        Label33.Text = "Nome do pacote de instalação:"
                        Label36.Text = "Informações do pacote"
                        Label37.Text = "Seleccione um pacote instalado para ver as suas informações aqui"
                        Label39.Text = "Nome de apresentação:"
                        Label41.Text = "Hora de criação:"
                        Label43.Text = "Descrição:"
                        Label45.Text = "Nome do produto:"
                        Label47.Text = "Instalar cliente:"
                        Label48.Text = "É necessário reiniciar?"
                        Label50.Text = "Informações de suporte:"
                        Label52.Text = "Estado:"
                        Label54.Text = "É necessário um arranque para a instalação completa?"
                        Label58.Text = "Propriedades personalizadas:"
                        Label60.Text = "Características:"
                        Label61.Text = "Identidade da capacidade:"
                        Label63.Text = "Descrição:"
                        Label65.Text = "Instalar cliente:"
                        Label67.Text = "Nome do pacote de instalação:"
                        Label69.Text = "Hora da instalação:"
                        Label71.Text = "Hora da última atualização:"
                        Label73.Text = "Nome do ecrã:"
                        Label75.Text = "Nome do produto:"
                        Label77.Text = "Versão do produto:"
                        Label79.Text = "Tipo de versão:"
                        Label81.Text = "É necessário reiniciar o sistema?"
                        Label83.Text = "Informações de suporte:"
                        Label85.Text = "Estado:"
                        Label87.Text = "É necessário um arranque para uma instalação completa?"
                        Label89.Text = "Identidade da capacidade:"
                        Label91.Text = "Propriedades personalizadas:"
                        Label93.Text = "Características:"
                        LinkLabel1.Text = "<- Voltar atrás"
                        Button1.Text = "Adicionar pacote..."
                        Button2.Text = "Remover selecionado"
                        Button3.Text = "Remover tudo"
                        Button4.Text = "Guardar..."
                        InstalledPackageLink.Text = "Pretendo obter informações sobre os pacotes instalados na imagem"
                        PackageFileLink.Text = "Pretendo obter informações sobre ficheiros de pacotes"
                        OpenFileDialog1.Title = "Localizar ficheiros de pacotes"
                        SearchBox1.cueBanner = "Digitar aqui para pesquisar um pacote..."
                End Select
            Case 1
                Text = "Get package information"
                Label1.Text = Text
                Label2.Text = "What do you want to get information about?"
                Label3.Text = "Click here to get information about packages that you've installed or that came with the Windows image you're servicing"
                Label4.Text = "Click here to get information about packages that you want to add to the Windows image you're servicing before proceeding with the package addition process"
                Label5.Text = "Ready"
                Label6.Text = "Add or select a package file to view its information here"
                Label7.Text = "Package information"
                Label8.Text = "Package name:"
                Label10.Text = "Is package applicable?"
                Label12.Text = "Copyright:"
                Label14.Text = "Product version:"
                Label16.Text = "Release type:"
                Label18.Text = "Company:"
                Label20.Text = "Creation time:"
                Label22.Text = "Package name:"
                Label24.Text = "Is package applicable?"
                Label26.Text = "Copyright:"
                Label28.Text = "Install time:"
                Label30.Text = "Last update time:"
                Label31.Text = "Company:"
                Label33.Text = "Install package name:"
                Label36.Text = "Package information"
                Label37.Text = "Select an installed package to view its information here"
                Label39.Text = "Display name:"
                Label41.Text = "Creation time:"
                Label43.Text = "Description:"
                Label45.Text = "Product name:"
                Label47.Text = "Install client:"
                Label48.Text = "Is a restart required?"
                Label50.Text = "Support information:"
                Label52.Text = "State:"
                Label54.Text = "Is a boot up required for full installation?"
                Label58.Text = "Custom properties:"
                Label60.Text = "Features:"
                Label61.Text = "Capability identity:"
                Label63.Text = "Description:"
                Label65.Text = "Install client:"
                Label67.Text = "Install package name:"
                Label69.Text = "Install time:"
                Label71.Text = "Last update time:"
                Label73.Text = "Display name:"
                Label75.Text = "Product name:"
                Label77.Text = "Product version:"
                Label79.Text = "Release type:"
                Label81.Text = "Is a restart required?"
                Label83.Text = "Support information:"
                Label85.Text = "State:"
                Label87.Text = "Is a boot up required for full installation?"
                Label89.Text = "Capability identity:"
                Label91.Text = "Custom properties:"
                Label93.Text = "Features:"
                LinkLabel1.Text = "<- Go back"
                Button1.Text = "Add package..."
                Button2.Text = "Remove selected"
                Button3.Text = "Remove all"
                Button4.Text = "Save..."
                InstalledPackageLink.Text = "I want to get information about installed packages in the image"
                PackageFileLink.Text = "I want to get information about package files"
                OpenFileDialog1.Title = "Locate package files"
                SearchBox1.cueBanner = "Type here to search for a package..."
            Case 2
                Text = "Obtener información de paquetes"
                Label1.Text = Text
                Label2.Text = "¿Acerca de qué le gustaría obtener información?"
                Label3.Text = "Haga clic aquí para obtener información de paquetes que ha instalado o que vengan con la imagen de Windows a la que está dando servicio"
                Label4.Text = "Haga clic aquí para obtener información de paquetes que le gustaría añadir a la imagen de Windows a la que está dando servicio antes de proceder con el proceso de adición de paquetes"
                Label5.Text = "Listo"
                Label6.Text = "Añada o seleccione un archivo de paquete para ver su información aquí"
                Label7.Text = "Información de paquete"
                Label8.Text = "Nombre de paquete:"
                Label10.Text = "¿El paquete es aplicable?"
                Label12.Text = "Copyright:"
                Label14.Text = "Versión de producto:"
                Label16.Text = "Tipo de paquete:"
                Label18.Text = "Compañía:"
                Label20.Text = "Tiempo de creación:"
                Label22.Text = "Nombre de paquete:"
                Label24.Text = "¿El paquete es aplicable?"
                Label26.Text = "Copyright:"
                Label28.Text = "Tiempo de instalación:"
                Label30.Text = "Último tiempo de actualización:"
                Label31.Text = "Compañía:"
                Label33.Text = "Nombre del paquete de instalación:"
                Label36.Text = "Información de paquete"
                Label37.Text = "Seleccione un paquete instalado para ver su información aquí"
                Label39.Text = "Nombre a mostrar:"
                Label41.Text = "Tiempo de creación:"
                Label43.Text = "Descripción:"
                Label45.Text = "Nombre de producto:"
                Label47.Text = "Cliente de instalación:"
                Label48.Text = "¿Se requiere un reinicio?"
                Label50.Text = "Información de soporte:"
                Label52.Text = "Estado:"
                Label54.Text = "¿Se requiere un arranque para una instalación completa?"
                Label58.Text = "Propiedades personalizadas:"
                Label60.Text = "Características:"
                Label61.Text = "Identidad de funcionalidad:"
                Label63.Text = "Descripción:"
                Label65.Text = "Cliente de instalación:"
                Label67.Text = "Nombre del paquete de instalación:"
                Label69.Text = "Tiempo de instalación:"
                Label71.Text = "Último tiempo de actualización:"
                Label73.Text = "Nombre a mostrar:"
                Label75.Text = "Nombre de producto:"
                Label77.Text = "Versión de producto:"
                Label79.Text = "Tipo de paquete:"
                Label81.Text = "¿Se requiere un reinicio?"
                Label83.Text = "Información de soporte:"
                Label85.Text = "Estado:"
                Label87.Text = "¿Se requiere un arranque para una instalación completa?"
                Label89.Text = "Identidad de funcionalidad:"
                Label91.Text = "Propiedades personalizadas:"
                Label93.Text = "Características:"
                LinkLabel1.Text = "<- Atrás"
                Button1.Text = "Añadir paquete..."
                Button2.Text = "Eliminar selección"
                Button3.Text = "Eliminar todo"
                Button4.Text = "Guardar..."
                InstalledPackageLink.Text = "Deseo obtener información acerca de paquetes instalados en la imagen"
                PackageFileLink.Text = "Deseo obtener información acerca de archivos de paquetes"
                OpenFileDialog1.Title = "Ubique los archivos de paquetes"
                SearchBox1.cueBanner = "Escriba aquí para buscar un paquete..."
            Case 3
                Text = "Obtenir des informations sur les paquets"
                Label1.Text = Text
                Label2.Text = "Sur quoi souhaitez-vous obtenir des informations ?"
                Label3.Text = "Cliquez ici pour obtenir des informations sur les paquets que vous avez installés ou qui sont fournis avec l'image Windows dont vous assurez la maintenance."
                Label4.Text = "Cliquez ici pour obtenir des informations sur les paquets que vous souhaitez ajouter à l'image Windows que vous maintenez avant de procéder à l'ajout de paquets."
                Label5.Text = "Prêt"
                Label6.Text = "Ajoutez ou sélectionnez un fichier de paquet pour afficher son information ici"
                Label7.Text = "Information sur le paquet"
                Label8.Text = "Nom du paquet :"
                Label10.Text = "Le paquet est-il applicable ?"
                Label12.Text = "Copyright :"
                Label14.Text = "Version du produit :"
                Label16.Text = "Type de publication :"
                Label18.Text = "Enterprise :"
                Label20.Text = "Temps de création :"
                Label22.Text = "Nom du paquet :"
                Label24.Text = "Le paquet est-il applicable ?"
                Label26.Text = "Copyright :"
                Label28.Text = "Temps d'installation :"
                Label30.Text = "Dernière heure de mise à jour :"
                Label31.Text = "Enterprise :"
                Label33.Text = "Nom du paquet d'installation :"
                Label36.Text = "Information sur le paquet"
                Label37.Text = "Sélectionnez un paquet installé pour afficher son information ici"
                Label39.Text = "Nom d'affichage:"
                Label41.Text = "Temps de création :"
                Label43.Text = "Description :"
                Label45.Text = "Nom du produit :"
                Label47.Text = "Client d'installation :"
                Label48.Text = "Un redémarrage est-il nécessaire ?"
                Label50.Text = "Information de support :"
                Label52.Text = "État :"
                Label54.Text = "L'installation complète nécessite-t-elle un démarrage ?"
                Label58.Text = "Propriétés personnalisées :"
                Label60.Text = "Caractéristiques :"
                Label61.Text = "Identité de la capacité :"
                Label63.Text = "Description :"
                Label65.Text = "Client d'installation :"
                Label67.Text = "Nom du paquet d'installation :"
                Label69.Text = "Temps d'installation :"
                Label71.Text = "Dernière heure de mise à jour :"
                Label73.Text = "Nom d'affichage :"
                Label75.Text = "Nom du produit :"
                Label77.Text = "Version du produit :"
                Label79.Text = "Type de publication :"
                Label81.Text = "Un redémarrage est-il nécessaire ?"
                Label83.Text = "Information de support :"
                Label85.Text = "État:"
                Label87.Text = "L'installation complète nécessite-t-elle un démarrage ?"
                Label89.Text = "Identité de la capacité :"
                Label91.Text = "Propriétés personnalisées :"
                Label93.Text = "Caractéristiques :"
                LinkLabel1.Text = "<- Retour"
                Button1.Text = "Ajouter un paquet..."
                Button2.Text = "Supprimer la sélection"
                Button3.Text = "Supprimer tout"
                Button4.Text = "Sauvegarder..."
                InstalledPackageLink.Text = "Je souhaite obtenir des informations sur les paquets installés dans l'image."
                PackageFileLink.Text = "Je souhaite obtenir des informations sur les fichiers de paquets"
                OpenFileDialog1.Title = "Localiser les fichiers des paquets"
                SearchBox1.cueBanner = "Tapez ici pour rechercher un paquet..."
            Case 4
                Text = "Obter informações sobre o pacote"
                Label1.Text = Text
                Label2.Text = "Sobre o que é que pretende obter informações?"
                Label3.Text = "Clique aqui para obter informações sobre os pacotes que instalou ou que vieram com a imagem do Windows que está a reparar"
                Label4.Text = "Clique aqui para obter informações sobre os pacotes que pretende adicionar à imagem do Windows que está a reparar antes de prosseguir com o processo de adição de pacotes"
                Label5.Text = "Pronto"
                Label6.Text = "Adicione ou seleccione um ficheiro de pacote para ver as suas informações aqui"
                Label7.Text = "Informações do pacote"
                Label8.Text = "Nome do pacote:"
                Label10.Text = "O pacote é aplicável?"
                Label12.Text = "Direitos de autor:"
                Label14.Text = "Versão do produto:"
                Label16.Text = "Tipo de versão:"
                Label18.Text = "Empresa:"
                Label20.Text = "Hora de criação:"
                Label22.Text = "Nome do pacote:"
                Label24.Text = "O pacote é aplicável?"
                Label26.Text = "Direitos de autor:"
                Label28.Text = "Hora de instalação:"
                Label30.Text = "Hora da última atualização:"
                Label31.Text = "Empresa:"
                Label33.Text = "Nome do pacote de instalação:"
                Label36.Text = "Informações do pacote"
                Label37.Text = "Seleccione um pacote instalado para ver as suas informações aqui"
                Label39.Text = "Nome de apresentação:"
                Label41.Text = "Hora de criação:"
                Label43.Text = "Descrição:"
                Label45.Text = "Nome do produto:"
                Label47.Text = "Instalar cliente:"
                Label48.Text = "É necessário reiniciar?"
                Label50.Text = "Informações de suporte:"
                Label52.Text = "Estado:"
                Label54.Text = "É necessário um arranque para a instalação completa?"
                Label58.Text = "Propriedades personalizadas:"
                Label60.Text = "Características:"
                Label61.Text = "Identidade da capacidade:"
                Label63.Text = "Descrição:"
                Label65.Text = "Instalar cliente:"
                Label67.Text = "Nome do pacote de instalação:"
                Label69.Text = "Hora da instalação:"
                Label71.Text = "Hora da última atualização:"
                Label73.Text = "Nome do ecrã:"
                Label75.Text = "Nome do produto:"
                Label77.Text = "Versão do produto:"
                Label79.Text = "Tipo de versão:"
                Label81.Text = "É necessário reiniciar o sistema?"
                Label83.Text = "Informações de suporte:"
                Label85.Text = "Estado:"
                Label87.Text = "É necessário um arranque para uma instalação completa?"
                Label89.Text = "Identidade da capacidade:"
                Label91.Text = "Propriedades personalizadas:"
                Label93.Text = "Características:"
                LinkLabel1.Text = "<- Voltar atrás"
                Button1.Text = "Adicionar pacote..."
                Button2.Text = "Remover selecionado"
                Button3.Text = "Remover tudo"
                Button4.Text = "Guardar..."
                InstalledPackageLink.Text = "Pretendo obter informações sobre os pacotes instalados na imagem"
                PackageFileLink.Text = "Pretendo obter informações sobre ficheiros de pacotes"
                OpenFileDialog1.Title = "Localizar ficheiros de pacotes"
                SearchBox1.cueBanner = "Digitar aqui para pesquisar um pacote..."
        End Select
        ListBox1.ForeColor = ForeColor
        ListBox2.ForeColor = ForeColor
        If Environment.OSVersion.Version.Major = 10 Then
            Text = ""
            Win10Title.Visible = True
        End If
        Dim handle As IntPtr = MainForm.GetWindowHandle(Me)
        If MainForm.IsWindowsVersionOrGreater(10, 0, 18362) Then MainForm.EnableDarkTitleBar(handle, MainForm.BackColor = Color.FromArgb(48, 48, 48))

        ' Populate installed package listing
        ListBox2.Items.Clear()
        For Each InstalledPackage As DismPackage In InstalledPkgInfo
            ListBox2.Items.Add(InstalledPackage.PackageName)
        Next

        NoPkgPanel.Visible = True
        PackageFileInfoPanel.Visible = False
        Panel4.Visible = False
        Panel7.Visible = True
        SearchBox1.Text = ""
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MenuPanel.Visible = True
        PackageInfoPanel.Visible = False
    End Sub

    Private Sub InstalledPackageLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles InstalledPackageLink.LinkClicked
        MenuPanel.Visible = False
        PackageInfoPanel.Visible = True
        InfoFromInstalledPkgsPanel.Visible = True
        InfoFromPackageFilesPanel.Visible = False
        Button4.Enabled = True
    End Sub

    Private Sub PackageFileLink_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles PackageFileLink.LinkClicked
        MenuPanel.Visible = False
        PackageInfoPanel.Visible = True
        InfoFromInstalledPkgsPanel.Visible = False
        InfoFromPackageFilesPanel.Visible = True
        Button4.Enabled = ListBox1.Items.Count > 0
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        Try
            If ListBox2.SelectedItems.Count = 1 Then
                ' Background processes need to have completed before showing information
                If MainForm.ImgBW.IsBusy Then
                    Dim msg As String = ""
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    msg = "Background processes need to have completed before showing package information. We'll wait until they have completed"
                                Case "ESN"
                                    msg = "Los procesos en segundo plano deben haber completado antes de obtener información del paquete. Esperaremos hasta que hayan completado"
                                Case "FRA"
                                    msg = "Les processus en plan doivent être terminés avant d'afficher les paquets. Nous attendrons qu'ils soient terminés"
                                Case "PTB", "PTG"
                                    msg = "Os processos em segundo plano precisam de ser concluídos antes de mostrar as informações dos pacotes. Esperamos até que estejam concluídos"
                            End Select
                        Case 1
                            msg = "Background processes need to have completed before showing package information. We'll wait until they have completed"
                        Case 2
                            msg = "Los procesos en segundo plano deben haber completado antes de obtener información del paquete. Esperaremos hasta que hayan completado"
                        Case 3
                            msg = "Les processus en plan doivent être terminés avant d'afficher les paquets. Nous attendrons qu'ils soient terminés"
                        Case 4
                            msg = "Os processos em segundo plano precisam de ser concluídos antes de mostrar as informações dos pacotes. Esperamos até que estejam concluídos"
                    End Select
                    MsgBox(msg, vbOKOnly + vbInformation, Label1.Text)
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    Label5.Text = "Waiting for background processes to finish..."
                                Case "ESN"
                                    Label5.Text = "Esperando a que terminen los procesos en segundo plano..."
                                Case "FRA"
                                    Label5.Text = "Attente de la fin des processus en arrière plan..."
                                Case "PTB", "PTG"
                                    Label5.Text = "À espera que os processos em segundo plano terminem..."
                            End Select
                        Case 1
                            Label5.Text = "Waiting for background processes to finish..."
                        Case 2
                            Label5.Text = "Esperando a que terminen los procesos en segundo plano..."
                        Case 3
                            Label5.Text = "Attente de la fin des processus en arrière plan..."
                        Case 4
                            Label5.Text = "À espera que os processos em segundo plano terminem..."
                    End Select
                    While MainForm.ImgBW.IsBusy
                        Application.DoEvents()
                        Thread.Sleep(500)
                    End While
                End If
                If MainForm.MountedImageDetectorBW.IsBusy Then
                    MainForm.MountedImageDetectorBW.CancelAsync()
                    While MainForm.MountedImageDetectorBW.IsBusy
                        Application.DoEvents()
                        Thread.Sleep(500)
                    End While
                End If
                MainForm.WatcherTimer.Enabled = False
                If MainForm.WatcherBW.IsBusy Then MainForm.WatcherBW.CancelAsync()
                While MainForm.WatcherBW.IsBusy
                    Application.DoEvents()
                    Thread.Sleep(100)
                End While
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                Label5.Text = "Preparing to get package information..."
                            Case "ESN"
                                Label5.Text = "Preparándonos para obtener información del paquete..."
                            Case "FRA"
                                Label5.Text = "Préparation de l'obtention des informations du paquet en cours..."
                            Case "PTB", "PTG"
                                Label5.Text = "Preparar-se para obter informações sobre o pacote..."
                        End Select
                    Case 1
                        Label5.Text = "Preparing to get package information..."
                    Case 2
                        Label5.Text = "Preparándonos para obtener información del paquete..."
                    Case 3
                        Label5.Text = "Préparation de l'obtention des informations du paquet en cours..."
                    Case 4
                        Label5.Text = "Preparar-se para obter informações sobre o pacote..."
                End Select
                Application.DoEvents()
                Try
                    DismApi.Initialize(DismLogLevel.LogErrors)
                    Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                        Select Case MainForm.Language
                            Case 0
                                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                    Case "ENU", "ENG"
                                        Label5.Text = "Getting information from " & Quote & ListBox2.SelectedItem & Quote & "..."
                                    Case "ESN"
                                        Label5.Text = "Obteniendo información de " & Quote & ListBox2.SelectedItem & Quote & "..."
                                    Case "FRA"
                                        Label5.Text = "Obtention des informations de " & Quote & ListBox2.SelectedItem & Quote & " en cours..."
                                    Case "PTB", "PTG"
                                        Label5.Text = "Obter informações de " & Quote & ListBox2.SelectedItem & Quote & "..."
                                End Select
                            Case 1
                                Label5.Text = "Getting information from " & Quote & ListBox2.SelectedItem & Quote & "..."
                            Case 2
                                Label5.Text = "Obteniendo información de " & Quote & ListBox2.SelectedItem & Quote & "..."
                            Case 3
                                Label5.Text = "Obtention des informations de " & Quote & ListBox2.SelectedItem & Quote & " en cours..."
                            Case 4
                                Label5.Text = "Obter informações de " & Quote & ListBox2.SelectedItem & Quote & "..."
                        End Select
                        Dim PkgInfoEx As DismPackageInfoEx = Nothing
                        Dim PkgInfo As DismPackageInfo = Nothing
                        ' On Windows 10 and later, use the extended version, as DISM gets extended package information.
                        ' Windows 8 and earlier cannot use the extended type, as no "Ex" function is declared in their DISM API DLL
                        If Environment.OSVersion.Version.Major >= 10 Then
                            PkgInfoEx = DismApi.GetPackageInfoExByName(imgSession, ListBox2.SelectedItem)
                        Else
                            PkgInfo = DismApi.GetPackageInfoByName(imgSession, ListBox2.SelectedItem)
                        End If
                        Label23.Text = If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.PackageName, PkgInfo.PackageName)
                        Label25.Text = Casters.CastDismApplicabilityStatus(If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.Applicable, PkgInfo.Applicable), True)
                        Label35.Text = If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.Copyright, PkgInfo.Copyright)
                        Label32.Text = If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.Company, PkgInfo.Company)
                        Label40.Text = If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.CreationTime, PkgInfo.CreationTime)
                        Label42.Text = If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.Description, PkgInfo.Description)
                        Label46.Text = If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.InstallClient, PkgInfo.InstallClient)
                        Label34.Text = If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.InstallPackageName, PkgInfo.InstallPackageName)
                        Label27.Text = If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.InstallTime, PkgInfo.InstallTime)
                        Label29.Text = If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.LastUpdateTime, PkgInfo.LastUpdateTime)
                        Label38.Text = If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.DisplayName, PkgInfo.DisplayName)
                        Label44.Text = If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.ProductName, PkgInfo.ProductName)
                        Label15.Text = If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.ProductVersion.ToString(), PkgInfo.ProductVersion.ToString())
                        Label21.Text = Casters.CastDismReleaseType(If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.ReleaseType, PkgInfo.ReleaseType), True)
                        Label13.Text = Casters.CastDismRestartType(If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.RestartRequired, PkgInfo.RestartRequired), True)
                        Label49.Text = If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.SupportInformation, PkgInfo.SupportInformation)
                        Label51.Text = Casters.CastDismPackageState(If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.PackageState, PkgInfo.PackageState), True)
                        Label53.Text = Casters.CastDismFullyOfflineInstallationType(If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.FullyOffline, PkgInfo.FullyOffline), True)
                        If Environment.OSVersion.Version.Major >= 10 Then Label56.Text = PkgInfoEx.CapabilityId Else Label56.Text = ""
                        Label57.Text = ""
                        Dim cProps As DismCustomPropertyCollection = If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.CustomProperties, PkgInfo.CustomProperties)
                        If cProps.Count > 0 Then
                            For Each cProp As DismCustomProperty In cProps
                                Label57.Text &= "- " & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value & CrLf
                            Next
                        Else
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            Label57.Text = "None"
                                        Case "ESN"
                                            Label57.Text = "Ninguna"
                                        Case "FRA"
                                            Label57.Text = "Aucune"
                                        Case "PTB", "PTG"
                                            Label57.Text = "Nenhum"
                                    End Select
                                Case 1
                                    Label57.Text = "None"
                                Case 2
                                    Label57.Text = "Ninguna"
                                Case 3
                                    Label57.Text = "Aucune"
                                Case 4
                                    Label57.Text = "Nenhum"
                            End Select
                        End If
                        Label59.Text = ""
                        Dim pkgFeats As DismFeatureCollection = If(Environment.OSVersion.Version.Major >= 10, PkgInfoEx.Features, PkgInfo.Features)
                        If pkgFeats.Count > 0 Then
                            ' Output all features
                            For Each pkgFeat As DismFeature In pkgFeats
                                Label59.Text &= "- " & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State, True) & ")" & CrLf
                            Next
                        Else
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            Label59.Text = "None"
                                        Case "ESN"
                                            Label59.Text = "Ninguna"
                                        Case "FRA"
                                            Label59.Text = "Aucune"
                                        Case "PTB", "PTG"
                                            Label59.Text = "Nenhum"
                                    End Select
                                Case 1
                                    Label59.Text = "None"
                                Case 2
                                    Label59.Text = "Ninguna"
                                Case 3
                                    Label59.Text = "Aucune"
                                Case 4
                                    Label59.Text = "Nenhum"
                            End Select
                        End If
                    End Using
                    Panel4.Visible = True
                    Panel7.Visible = False
                    Select Case MainForm.Language
                        Case 0
                            Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                Case "ENU", "ENG"
                                    Label5.Text = "Ready"
                                Case "ESN"
                                    Label5.Text = "Listo"
                                Case "FRA"
                                    Label5.Text = "Prêt"
                                Case "PTB", "PTG"
                                    Label5.Text = "Pronto"
                            End Select
                        Case 1
                            Label5.Text = "Ready"
                        Case 2
                            Label5.Text = "Listo"
                        Case 3
                            Label5.Text = "Prêt"
                        Case 4
                            Label5.Text = "Pronto"
                    End Select
                Finally
                    DismApi.Shutdown()
                End Try
            Else
                Panel4.Visible = False
                Panel7.Visible = True
            End If
        Catch ex As Exception
            Panel4.Visible = False
            Panel7.Visible = True
        End Try
    End Sub

    Sub GetPackageFileInformation()
        PackageInfoList.Clear()
        PackageInfoExList.Clear()
        Try
            ' Background processes need to have completed before showing information
            If MainForm.ImgBW.IsBusy Then
                Dim msg As String = ""
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                msg = "Background processes need to have completed before showing package information. We'll wait until they have completed"
                            Case "ESN"
                                msg = "Los procesos en segundo plano deben haber completado antes de obtener información del paquete. Esperaremos hasta que hayan completado"
                            Case "FRA"
                                msg = "Les processus en plan doivent être terminés avant d'afficher les paquets. Nous attendrons qu'ils soient terminés"
                            Case "PTB", "PTG"
                                msg = "Os processos em segundo plano precisam de ser concluídos antes de mostrar as informações dos pacotes. Esperamos até que estejam concluídos"
                        End Select
                    Case 1
                        msg = "Background processes need to have completed before showing package information. We'll wait until they have completed"
                    Case 2
                        msg = "Los procesos en segundo plano deben haber completado antes de obtener información del paquete. Esperaremos hasta que hayan completado"
                    Case 3
                        msg = "Les processus en plan doivent être terminés avant d'afficher les paquets. Nous attendrons qu'ils soient terminés"
                    Case 4
                        msg = "Os processos em segundo plano precisam de ser concluídos antes de mostrar as informações dos pacotes. Esperamos até que estejam concluídos"
                End Select
                MsgBox(msg, vbOKOnly + vbInformation, Label1.Text)
                Select Case MainForm.Language
                    Case 0
                        Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                            Case "ENU", "ENG"
                                Label5.Text = "Waiting for background processes to finish..."
                            Case "ESN"
                                Label5.Text = "Esperando a que terminen los procesos en segundo plano..."
                            Case "FRA"
                                Label5.Text = "Attente de la fin des processus en arrière plan..."
                            Case "PTB", "PTG"
                                Label5.Text = "À espera que os processos em segundo plano terminem..."
                        End Select
                    Case 1
                        Label5.Text = "Waiting for background processes to finish..."
                    Case 2
                        Label5.Text = "Esperando a que terminen los procesos en segundo plano..."
                    Case 3
                        Label5.Text = "Attente de la fin des processus en arrière plan..."
                    Case 4
                        Label5.Text = "À espera que os processos em segundo plano terminem..."
                End Select
                While MainForm.ImgBW.IsBusy
                    Application.DoEvents()
                    Thread.Sleep(500)
                End While
            End If
            If MainForm.MountedImageDetectorBW.IsBusy Then
                MainForm.MountedImageDetectorBW.CancelAsync()
                While MainForm.MountedImageDetectorBW.IsBusy
                    Application.DoEvents()
                    Thread.Sleep(500)
                End While
            End If
            MainForm.WatcherTimer.Enabled = False
            If MainForm.WatcherBW.IsBusy Then MainForm.WatcherBW.CancelAsync()
            While MainForm.WatcherBW.IsBusy
                Application.DoEvents()
                Thread.Sleep(100)
            End While
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label5.Text = "Preparing package information processes..."
                        Case "ESN"
                            Label5.Text = "Preparando procesos de información de paquetes..."
                        Case "FRA"
                            Label5.Text = "Préparation des processus d'information des paquets en cours..."
                        Case "PTB", "PTG"
                            Label5.Text = "Preparar os processos de informação dos pacotes..."
                    End Select
                Case 1
                    Label5.Text = "Preparing package information processes..."
                Case 2
                    Label5.Text = "Preparando procesos de información de paquetes..."
                Case 3
                    Label5.Text = "Préparation des processus d'information des paquets en cours..."
                Case 4
                    Label5.Text = "Preparar os processos de informação dos pacotes..."
            End Select
            Application.DoEvents()
            Try
                DismApi.Initialize(DismLogLevel.LogErrors)
                Using imgSession As DismSession = If(MainForm.OnlineManagement, DismApi.OpenOnlineSession(), DismApi.OpenOfflineSession(MainForm.MountDir))
                    For Each pkgFile In ListBox1.Items
                        If File.Exists(pkgFile) Then
                            Select Case MainForm.Language
                                Case 0
                                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                                        Case "ENU", "ENG"
                                            Label5.Text = "Getting information from package file " & Quote & Path.GetFileName(pkgFile) & Quote & "..." & CrLf & "This may take some time and the program may temporarily freeze"
                                        Case "ESN"
                                            Label5.Text = "Obteniendo información del archivo de paquete " & Quote & Path.GetFileName(pkgFile) & Quote & "..." & CrLf & "Esto puede llevar algo de tiempo y el programa podría congelarse temporalmente"
                                        Case "FRA"
                                            Label5.Text = "Obtention des informations du fichier paquet " & Quote & Path.GetFileName(pkgFile) & Quote & " en cours..." & CrLf & "Cette opération peut prendre un certain temps et le programme peut se bloquer temporairement."
                                        Case "PTB", "PTG"
                                            Label5.Text = "Obter informações do ficheiro do pacote " & Quote & Path.GetFileName(pkgFile) & Quote & "..." & CrLf & "Isto pode demorar algum tempo e o programa pode congelar temporariamente"
                                    End Select
                                Case 1
                                    Label5.Text = "Getting information from package file " & Quote & Path.GetFileName(pkgFile) & Quote & "..." & CrLf & "This may take some time and the program may temporarily freeze"
                                Case 2
                                    Label5.Text = "Obteniendo información del archivo de paquete " & Quote & Path.GetFileName(pkgFile) & Quote & "..." & CrLf & "Esto puede llevar algo de tiempo y el programa podría congelarse temporalmente"
                                Case 3
                                    Label5.Text = "Obtention des informations du fichier paquet " & Quote & Path.GetFileName(pkgFile) & Quote & " en cours..." & CrLf & "Cette opération peut prendre un certain temps et le programme peut se bloquer temporairement."
                                Case 4
                                    Label5.Text = "Obter informações do ficheiro do pacote " & Quote & Path.GetFileName(pkgFile) & Quote & "..." & CrLf & "Isto pode demorar algum tempo e o programa pode congelar temporariamente"
                            End Select
                            Application.DoEvents()
                            Dim pkgInfoEx As DismPackageInfoEx = Nothing
                            Dim pkgInfo As DismPackageInfo = Nothing
                            If Environment.OSVersion.Version.Major >= 10 Then
                                pkgInfoEx = DismApi.GetPackageInfoExByPath(imgSession, pkgFile)
                            Else
                                pkgInfo = DismApi.GetPackageInfoByPath(imgSession, pkgFile)
                            End If
                            If pkgInfoEx IsNot Nothing Then PackageInfoExList.Add(pkgInfoEx)
                            If pkgInfo IsNot Nothing Then PackageInfoList.Add(pkgInfo)
                        End If
                    Next
                End Using
            Catch DISMEx As DismException
                MsgBox(DISMEx.Message & " (HRESULT " & Hex(DISMEx.HResult) & ")", vbOKOnly + vbCritical, Label1.Text)
            Finally
                DismApi.Shutdown()
            End Try
        Catch ex As Exception
            ' Cancel it
        End Try
        Select Case MainForm.Language
            Case 0
                Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                    Case "ENU", "ENG"
                        Label5.Text = "Ready"
                    Case "ESN"
                        Label5.Text = "Listo"
                    Case "FRA"
                        Label5.Text = "Prêt"
                    Case "PTB", "PTG"
                        Label5.Text = "Pronto"
                End Select
            Case 1
                Label5.Text = "Ready"
            Case 2
                Label5.Text = "Listo"
            Case 3
                Label5.Text = "Prêt"
            Case 4
                Label5.Text = "Pronto"
        End Select
    End Sub

    Sub DisplayPackageFileInformation(PkgFile As Integer)
        Label9.Text = If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).PackageName, PackageInfoList(PkgFile).PackageName)
        Label11.Text = Casters.CastDismApplicabilityStatus(If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).Applicable, PackageInfoList(PkgFile).Applicable), True)
        Label17.Text = If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).Copyright, PackageInfoList(PkgFile).Copyright)
        Label19.Text = If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).Company, PackageInfoList(PkgFile).Company)
        Label62.Text = If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).CreationTime, PackageInfoList(PkgFile).CreationTime)
        Label64.Text = If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).Description, PackageInfoList(PkgFile).Description)
        Label66.Text = If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).InstallClient, PackageInfoList(PkgFile).InstallClient)
        Label68.Text = If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).InstallPackageName, PackageInfoList(PkgFile).InstallPackageName)
        Label70.Text = If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).InstallTime, PackageInfoList(PkgFile).InstallTime)
        Label72.Text = If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).LastUpdateTime, PackageInfoList(PkgFile).LastUpdateTime)
        Label74.Text = If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).DisplayName, PackageInfoList(PkgFile).DisplayName)
        Label76.Text = If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).ProductName, PackageInfoList(PkgFile).ProductName)
        Label78.Text = If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).ProductVersion.ToString(), PackageInfoList(PkgFile).ProductVersion.ToString())
        Label80.Text = Casters.CastDismReleaseType(If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).ReleaseType, PackageInfoList(PkgFile).ReleaseType), True)
        Label82.Text = Casters.CastDismRestartType(If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).RestartRequired, PackageInfoList(PkgFile).RestartRequired), True)
        Label84.Text = If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).SupportInformation, PackageInfoList(PkgFile).SupportInformation)
        Label86.Text = Casters.CastDismPackageState(If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).PackageState, PackageInfoList(PkgFile).PackageState), True)
        Label88.Text = Casters.CastDismFullyOfflineInstallationType(If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).FullyOffline, PackageInfoList(PkgFile).FullyOffline), True)
        If Environment.OSVersion.Version.Major >= 10 Then Label90.Text = PackageInfoExList(PkgFile).CapabilityId Else Label90.Text = ""
        Label92.Text = ""
        Dim cProps As DismCustomPropertyCollection = If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).CustomProperties, PackageInfoList(PkgFile).CustomProperties)
        If cProps.Count > 0 Then
            For Each cProp As DismCustomProperty In cProps
                Label92.Text &= "- " & If(cProp.Path <> "", cProp.Path & "\", "") & cProp.Name & ": " & cProp.Value & CrLf
            Next
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label92.Text = "None"
                        Case "ESN"
                            Label92.Text = "Ninguna"
                        Case "FRA"
                            Label92.Text = "Aucune"
                        Case "PTB", "PTG"
                            Label92.Text = "Nenhum"
                    End Select
                Case 1
                    Label92.Text = "None"
                Case 2
                    Label92.Text = "Ninguna"
                Case 3
                    Label92.Text = "Aucune"
                Case 4
                    Label92.Text = "Nenhum"
            End Select
        End If
        Label94.Text = ""
        Dim pkgFeats As DismFeatureCollection = If(Environment.OSVersion.Version.Major >= 10, PackageInfoExList(PkgFile).Features, PackageInfoList(PkgFile).Features)
        If pkgFeats.Count > 0 Then
            ' Output all features
            For Each pkgFeat As DismFeature In pkgFeats
                Label94.Text &= "- " & pkgFeat.FeatureName & " (" & Casters.CastDismFeatureState(pkgFeat.State, True) & ")" & CrLf
            Next
        Else
            Select Case MainForm.Language
                Case 0
                    Select Case My.Computer.Info.InstalledUICulture.ThreeLetterWindowsLanguageName
                        Case "ENU", "ENG"
                            Label94.Text = "None"
                        Case "ESN"
                            Label94.Text = "Ninguna"
                        Case "FRA"
                            Label94.Text = "Aucune"
                        Case "PTB", "PTG"
                            Label94.Text = "Nenhum"
                    End Select
                Case 1
                    Label94.Text = "None"
                Case 2
                    Label94.Text = "Ninguna"
                Case 3
                    Label94.Text = "Aucune"
                Case 4
                    Label94.Text = "Nenhum"
            End Select
        End If
    End Sub

    Private Sub ListBox1_DragEnter(sender As Object, e As DragEventArgs) Handles ListBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ListBox1_DragDrop(sender As Object, e As DragEventArgs) Handles ListBox1.DragDrop
        Dim PackageFiles() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each PackageFile In PackageFiles
            If Path.GetExtension(PackageFile).EndsWith("cab", StringComparison.OrdinalIgnoreCase) Then
                ListBox1.Items.Add(PackageFile)
            End If
        Next
        Button3.Enabled = True
        Button4.Enabled = True
        GetPackageFileInformation()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Try
            If ListBox1.SelectedItems.Count = 1 Then
                NoPkgPanel.Visible = False
                PackageFileInfoPanel.Visible = True
                Button2.Enabled = True
                If PackageInfoExList.Count > 0 Or PackageInfoList.Count > 0 Then DisplayPackageFileInformation(ListBox1.SelectedIndex)
            Else
                NoPkgPanel.Visible = True
                PackageFileInfoPanel.Visible = False
                Button2.Enabled = False
            End If
        Catch ex As Exception
            ListBox1.Items.Remove(ListBox1.SelectedItem)
            NoPkgPanel.Visible = True
            PackageFileInfoPanel.Visible = False
            If ListBox1.Items.Count < 1 Then
                Button2.Enabled = False
                Button3.Enabled = False
                Button4.Enabled = False
            End If
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            PackageInfoList.RemoveAt(ListBox1.SelectedIndex)
        Catch ex As Exception
            ' Not in there
        End Try
        Try
            PackageInfoExList.RemoveAt(ListBox1.SelectedIndex)
        Catch ex As Exception
            ' Not in there
        End Try
        ListBox1.Items.Remove(ListBox1.SelectedItem)
        If ListBox1.Items.Count >= 1 Then
            Button2.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
        Else
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
        End If
        NoPkgPanel.Visible = True
        PackageFileInfoPanel.Visible = False
        Button2.Enabled = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        PackageInfoList.Clear()
        PackageInfoExList.Clear()
        ListBox1.Items.Clear()
        Button2.Enabled = False
        Button3.Enabled = False
        Button4.Enabled = False
        NoPkgPanel.Visible = True
        PackageFileInfoPanel.Visible = False
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        ListBox1.Items.Add(OpenFileDialog1.FileName)
        Button3.Enabled = True
        Button4.Enabled = True
        GetPackageFileInformation()
    End Sub

    Private Sub GetPkgInfoDlg_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not MainForm.MountedImageDetectorBW.IsBusy Then Call MainForm.MountedImageDetectorBW.RunWorkerAsync()
        MainForm.WatcherTimer.Enabled = True
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If MainForm.ImgInfoSFD.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If Not ImgInfoSaveDlg.IsDisposed Then ImgInfoSaveDlg.Dispose()
            If ImgInfoSaveDlg.PackageFiles.Count > 0 Then ImgInfoSaveDlg.PackageFiles.Clear()
            ImgInfoSaveDlg.SourceImage = MainForm.SourceImg
            ImgInfoSaveDlg.SaveTarget = MainForm.ImgInfoSFD.FileName
            ImgInfoSaveDlg.ImgMountDir = If(Not MainForm.OnlineManagement, MainForm.MountDir, "")
            ImgInfoSaveDlg.OnlineMode = MainForm.OnlineManagement
            ImgInfoSaveDlg.SkipQuestions = MainForm.SkipQuestions
            ImgInfoSaveDlg.AutoCompleteInfo = MainForm.AutoCompleteInfo
            ImgInfoSaveDlg.ForceAppxApi = False
            ImgInfoSaveDlg.SaveTask = If(InfoFromPackageFilesPanel.Visible, 3, 2)
            If InfoFromPackageFilesPanel.Visible Then
                For Each pkgFile In ListBox1.Items
                    If File.Exists(pkgFile) Then ImgInfoSaveDlg.PackageFiles.Add(pkgFile)
                Next
            End If
            ImgInfoSaveDlg.ShowDialog()
            InfoSaveResults.Show()
        End If
    End Sub

    Sub SearchPackages(sQuery As String)
        If InstalledPkgInfo.Count > 0 Then
            For Each InstalledPackage As DismPackage In InstalledPkgInfo
                If InstalledPackage.PackageName.ToLower().Contains(sQuery.ToLower()) Then
                    ListBox2.Items.Add(InstalledPackage.PackageName)
                End If
            Next
        End If
    End Sub

    Private Sub SearchBox1_TextChanged(sender As Object, e As EventArgs) Handles SearchBox1.TextChanged
        ListBox2.Items.Clear()
        If SearchBox1.Text <> "" Then
            SearchPackages(SearchBox1.Text)
        Else
            For Each InstalledPackage As DismPackage In InstalledPkgInfo
                ListBox2.Items.Add(InstalledPackage.PackageName)
            Next
        End If
    End Sub
End Class
