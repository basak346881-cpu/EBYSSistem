# 🏛️ EBYSSistem | Elektronik Belge Yönetim & İmar/Evrak Takip Otomasyonu

Kamu kurumları, belediyeler ve kurumsal organizasyonlar için geliştirilmiş; gelen/giden evrak akışı, süre takibi, imar parsel sorgulamaları ve belge yaşam döngüsünü masaüstü ortamında optimize eden **WPF (Windows Presentation Foundation) & C#** tabanlı otomasyon sistemi.

---

## 📌 Proje Genel Bakışı & Temel Modüller

### 📑 1. Evrak Kayıt ve Akış Yönetimi
- **Gelen / Giden Evrak Takibi:** Kuruma giren ve birimler arası sevk edilen resmi evrakların sayı, konu, tarih ve gönderici/alıcı bilgileriyle kayıt altına alınması.
- **Evrak Durum Takibi:** Evrakların beklemede, işlemde, onaylandı veya arşive sevk edildi durumlarının yönetimi.

### ⏱️ 2. Süresi Dolan / Yaklaşan Evrak Takip Modülü (`SuresiDolanEvrakView`)
- **Yasal Süre Kontrolü:** Cevaplanması veya işlem yapılması gereken evrakların termin tarihlerine göre otomatik filtrelenmesi.
- **Kritik Süre Uyarıları:** Gecikmeye düşen veya süresi yaklaşan resmi yazışmaların personele görsel uyarılarla listelenmesi.

### 🗺️ 3. Ada / Parsel İmar Arama Modülü (`AdaParselAramaView`)
- **Kadasral & İmar İlişkilendirme:** Ada, parsel ve malik (mülk sahibi) bilgilerine göre hızlı evrak ve dosya sorgulama (`ImarModel`).
- **Belge Eşleştirme:** İmar dosyalarına ait ruhsat, dilekçe ve resmi yazışmaların parsel numarası bazında doğrudan taranabilmesi.

### 🔍 4. Detaylı Arama, Filtreleme ve Raporlama
- Evrak kodu, başvuru sahibi veya tarih aralığına göre anlık veri filtreleme.
- Birim bazlı iş yükü ve evrak işlem istatistiklerinin takibi.

---

## 🛠️ Kullanılan Teknolojiler & Mimari

- **Platform:** .NET Desktop / C#
- **Arayüz Teknolojisi:** WPF (Windows Presentation Foundation), XAML
- **Mimari Yaklaşım:** MVVM (Model-View-ViewModel) ve Modüler `UserControl` yapısı
- **Veritabanı / Veri Erişimi:** Microsoft SQL Server / ADO.NET / EF Core
- **Tasarım & Deneyim:** Modern UI bileşenleri, veri bağlama (Data Binding) mekanizmaları

---

## 📂 Proje Dizin Yapısı

```text
EBYSSistem/
│
├── Views / UserControls/
│   ├── MainWindow.xaml               # Ana yönetim penceresi ve menü navigasyonu
│   ├── MainWindow.xaml.cs            # Ana pencere olay yöneticisi
│   ├── AdaParselAramaView.xaml       # İmar / Ada-Parsel arama arayüzü
│   ├── AdaParselAramaView.xaml.cs    # Ada-Parsel arama arkası mantığı
│   ├── SuresiDolanEvrakView.xaml     # Süresi yaklaşan/dolan evrak ekranı
│   └── SuresiDolanEvrakView.xaml.cs  # Evrak süre analizi kontrolü
│
├── Models/
│   ├── ImarModel.cs                  # Ada, parsel, malik ve evrak kodu veri modeli
│   └── EvrakModel.cs                 # Resmi evrak metaverileri modeli
│
├── App.xaml / App.xaml.cs            # Uygulama yaşam döngüsü ve global kaynaklar
└── EBYSSistem.csproj                 # Proje bağımlılıkları ve derleme ayarları
