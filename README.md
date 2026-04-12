# DISMTools 0.8 Preview — Türkçe Dil Desteği

> **English version below / İngilizce sürüm aşağıda**

---

## 🇹🇷 Türkçe

### Bu Nedir?

Bu depo, [DISMTools](https://github.com/CodingWonders/DISMTools) projesinin **0.8 Preview** sürümünü temel alan ve kapsamlı **Türkçe dil desteği** eklenmiş özel bir fork'udur.

DISMTools, Windows görüntü dosyalarını (WIM/ESD/FFU) yönetmek için kullanılan DISM aracının gelişmiş bir grafik arayüzüdür.

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
- Görüntü yakalama, FFU yakalama
- Birim görüntüsü kaldırma
- Görüntü bakımı (Bileşen deposu temizliği)

**Bilgi Pencereleri**
- Özellik bilgileri, Özellik (Capability) bilgileri
- Sürücü bilgileri, AppX paketi bilgileri
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

**Ana Form**
- Proje ağaç görünümü (ADK Dağıtım Araçları, Bağlama noktası vb.)
- Hata banner'ı ve "Daha fazla bilgi" butonu
- Menü açıklamaları

#### Hata Düzeltmeleri

- **Dinamik ilerleme çubuğu**: DISM işlemi sırasında progress bar artık DISM'in raporladığı yüzdeye göre dinamik olarak ilerliyor (önceden işlem bitince doluyordu)
- **Boş mesaj kutusu**: İşlem devam ederken X butonuna basıldığında boş mesaj kutusu yerine Türkçe onay diyaloğu gösteriliyor
- **BGProcDetails panel konumu**: "Görüntü bilgileri toplanıyor..." panelinin durum çubuğunun üzerine binmesi sorunu düzeltildi

### Derleme

```
powershell.exe -ExecutionPolicy Bypass -File build.ps1
```

Çıktı: `bin\Release\DISMTools.exe`

### Gereksinimler

- Windows 8.1 veya üzeri
- .NET Framework 4.8

---

## 🇬🇧 English

### What Is This?

This repository is a custom fork of [DISMTools](https://github.com/CodingWonders/DISMTools) **0.8 Preview**, with comprehensive **Turkish language support** added.

DISMTools is an advanced graphical front-end for DISM that lets you manage Windows image files (WIM/ESD/FFU).

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
- Image capture, FFU capture
- Volume image removal
- Image cleanup (Component store)

**Info Dialogs**
- Feature info, Capability info
- Driver info, AppX package info
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

**Main Form**
- Project tree view nodes (ADK Deployment Tools, Mount point, etc.)
- Error banner and "Learn more" button
- Menu descriptions

#### Bug Fixes

- **Dynamic progress bar**: The progress bar now updates dynamically based on DISM's reported percentage during operations (previously it only filled after completion)
- **Empty message box on close**: Fixed an issue where closing during background processes showed an empty message box instead of a Turkish confirmation dialog
- **BGProcDetails panel positioning**: Fixed the "Gathering image information..." panel overlapping the status bar

### Building

```
powershell.exe -ExecutionPolicy Bypass -File build.ps1
```

Output: `bin\Release\DISMTools.exe`

### System Requirements

- Windows 8.1 or later
- .NET Framework 4.8

---

*Based on [DISMTools](https://github.com/CodingWonders/DISMTools) by CodingWonders — original project README available in the upstream repository.*
