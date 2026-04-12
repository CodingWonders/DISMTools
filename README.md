# DISMTools 0.8 Preview — Türkçe Dil Desteği

> **English version below / İngilizce sürüm aşağıda**

---

## 🇹🇷 Türkçe

### Bu Nedir?

Bu depo, [DISMTools](https://github.com/CodingWonders/DISMTools) projesinin **0.8 Preview** sürümünü temel alan ve kapsamlı **Türkçe dil desteği** eklenmiş özel bir fork'udur.

DISMTools, Windows görüntü dosyalarını (WIM/ESD/FFU) yönetmek için kullanılan DISM aracının gelişmiş bir grafik arayüzüdür.

---

### Yapılan Değişiklikler

#### Türkçe Çeviriler (Dil Kodu: 6)

Aşağıdaki tüm pencere ve diyaloglar Türkçeye çevrilmiştir:

**Görüntü İşlemleri**
- Özellik etkinleştirme / devre dışı bırakma
- Paket ekleme / kaldırma
- AppX paketi ekleme / kaldırma
- Sürücü ekleme / kaldırma / dışa aktarma / içe aktarma
- Özellik (Capability) ekleme / kaldırma
- Windows PE ayarları (Geçici alan, Hedef yol)
- Görüntü yakalama (WIM ve FFU)
- Görüntü uygulama (WIM/SWM/ESD ve FFU)
- Birim görüntüsü kaldırma
- Görüntü bakımı (Bileşen deposu temizliği)
- Çevrimiçi kurulum yönetimi uyarı diyaloğu
- Çevrimdışı kurulum disk seçimi diyaloğu

**Bilgi Pencereleri**
- Özellik bilgileri, Özellik (Capability) bilgileri
- Sürücü bilgileri, AppX paketi bilgileri
- Paket bilgileri (yüklü ve dosya bazlı)
- Windows PE ayarları görüntüleme
- Proje özellikleri (tüm etiketler ve değerler)

**Arka Plan İşlemleri**
- "Görüntü bilgileri toplanıyor..." paneli
- "Arka plan işlemleri devam ediyor" diyaloğu
- Başarısız arka plan işlemleri diyaloğu
- Bildirim balonu

**İlerleme Paneli (ProgressPanel)**
- Tüm görev durum mesajları Türkçe
- "Günlüğü göster / Günlüğü gizle" butonu
- Görev sayacı ("Görevler: 1/1")
- Tüm işlem mesajları (bağlama, ayırma, paket ekleme vb.)

**Ana Form**
- Proje ağaç görünümü (ADK Dağıtım Araçları, Bağlama noktası vb.)
- Hata banner'ı ve "Daha fazla bilgi" butonu
- Menü açıklamaları ve tüm menü öğeleri
- Görüntü yakalama / uygulama dropdown menüleri
- Genişlet/Daralt butonu

**Seçenekler Penceresi**
- Tüm seçenekler ve açıklamaları Türkçe

#### Hata Düzeltmeleri

- **Dinamik ilerleme çubuğu**: DISM işlemi sırasında progress bar artık DISM'in raporladığı yüzdeye göre dinamik olarak ilerliyor
- **Boş mesaj kutusu**: İşlem devam ederken X butonuna basıldığında boş mesaj kutusu yerine Türkçe onay diyaloğu gösteriliyor
- **BGProcDetails panel konumu**: "Görüntü bilgileri toplanıyor..." panelinin durum çubuğunun üzerine binmesi sorunu düzeltildi
- **İtalyanca kalan metinler**: ProgressPanel ve MainForm'daki İtalyanca kalan tüm metinler Türkçeye çevrildi

#### Yardım Sistemi (docs/)

- `docs/tr/` klasörü oluşturuldu
- Türkçe yardım sayfaları: Başlarken, Windows bakımına başlarken, Bilgi diyalogları, Çevrimiçi/Çevrimdışı kurulum yönetimi
- Türkçe dil seçiliyken yardım linkleri otomatik olarak Türkçe sayfalara yönlendiriyor
- `build.ps1` güncellendi: derleme sonrası `docs/` klasörü otomatik olarak `bin/Release/docs/`'a kopyalanıyor

---

### Derleme

```powershell
powershell.exe -ExecutionPolicy Bypass -File build.ps1
```

Çıktı: `bin\Release\DISMTools.exe`

> Not: `build.ps1` otomatik olarak `settings.ini` dosyasında `Language=6` ayarını yapar.

### Gereksinimler

- Windows 8.1 veya üzeri
- .NET Framework 4.8

---

## 🇬🇧 English

### What Is This?

This repository is a custom fork of [DISMTools](https://github.com/CodingWonders/DISMTools) **0.8 Preview**, with comprehensive **Turkish language support** added.

DISMTools is an advanced graphical front-end for DISM that lets you manage Windows image files (WIM/ESD/FFU).

---

### Changes Made

#### Turkish Translations (Language Code: 6)

All dialogs and windows have been translated to Turkish, including:

**Image Operations**
- Feature enable/disable dialogs
- Package add/remove dialogs
- AppX package add/remove dialogs
- Driver add/remove/export/import dialogs
- Capability add/remove dialogs
- Windows PE settings (Scratch space, Target path)
- Image capture (WIM and FFU)
- Image apply (WIM/SWM/ESD and FFU)
- Volume image removal
- Image cleanup (Component store)
- Online installation management warning dialog
- Offline installation disk selection dialog

**Info Dialogs**
- Feature info, Capability info
- Driver info, AppX package info
- Package info (installed and file-based)
- Windows PE settings viewer
- Project properties (all labels and values)

**Background Process Dialogs**
- "Gathering image information..." panel
- Background processes busy dialog
- Failed background processes dialog
- Notification balloon

**Progress Panel**
- All task status messages in Turkish
- "Show log / Hide log" button
- Task counter ("Görevler: 1/1")
- All operation messages (mount, unmount, package add, etc.)

**Main Form**
- Project tree view nodes
- Error banner and "Learn more" button
- Menu descriptions and all menu items
- Image capture / apply dropdown menus
- Expand/Collapse button

**Options Window**
- All options and descriptions in Turkish

#### Bug Fixes

- **Dynamic progress bar**: Progress bar now updates dynamically based on DISM's reported percentage
- **Empty message box on close**: Fixed empty message box when closing during background processes
- **BGProcDetails panel positioning**: Fixed panel overlapping the status bar
- **Remaining Italian text**: All Italian text in ProgressPanel and MainForm translated to Turkish

#### Help System (docs/)

- Created `docs/tr/` folder with Turkish help pages
- Turkish pages: Getting started, Windows servicing intro, Info dialogs, Online/Offline installation management
- Help links automatically redirect to Turkish pages when Turkish language is selected
- `build.ps1` updated: `docs/` folder is automatically copied to `bin/Release/docs/` after build

---

### Building

```powershell
powershell.exe -ExecutionPolicy Bypass -File build.ps1
```

Output: `bin\Release\DISMTools.exe`

> Note: `build.ps1` automatically sets `Language=6` in `settings.ini`.

### System Requirements

- Windows 8.1 or later
- .NET Framework 4.8

---

*Based on [DISMTools](https://github.com/CodingWonders/DISMTools) by CodingWonders — original project README available in the upstream repository.*
